using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;

namespace VinhLB
{
    [ExecuteInEditMode]
    public class DrawPolygon2D : MonoBehaviour
    {
        public enum UVType
        {
            Fit = 0,
            Repeat = 1
        }

        [System.Serializable]
        public struct EdgeVertexIndexRange
        {
            public int From;
            public int Prev;
            public int To;
            public int Next;
        }

        public List<Vector2> VertexList;

        [SerializeField]
        private UVType _meshUVType;
        [SerializeField]
        private bool _canDropShadow = false;
        [SerializeField]
        private bool _loop = false;
        [SerializeField]
        private List<EdgeVertexIndexRange> _edgeVertexIndexRangeList;
        [SerializeField]
        private float _edgeWidth = 0.06f;
        [SerializeField]
        private float _edgeOffset = 0.03f;

        private GameObject _visualGO;
        private MeshFilter _visualMeshFilter;
        private MeshRenderer _visualMeshRenderer;
        private Mesh _visualMesh;

        private PolygonCollider2D _polygonCollider2D;
        private List<EdgeCollider2D> _edgeCollider2DList;
        private List<LineRenderer> _edgeRendererList;

        private GameObject _dropShadowGO;
        private MeshFilter _dropShadowMeshFilter;
        private MeshRenderer _dropShadowMeshRenderer;
        private Mesh _dropShadowMesh;

        private void OnEnable()
        {
            UpdateComponents();

            UpdateMeshes();
        }

        private void Reset()
        {
            UpdateMeshes();
        }

        public void UpdateMeshes()
        {
            if (VertexList == null)
            {
                return;
            }

            UpdateMeshInternal(VertexList.ToArray(), _visualMesh, _visualMeshFilter, _meshUVType);

            if (_canDropShadow)
            {
                List<Vector2> vertexList = new List<Vector2>();
                if (_loop)
                {
                    Vector3[] pointArray = GetPointArrayFromIndexRange(new EdgeVertexIndexRange
                    {
                        From = 0,
                        To = VertexList.Count - 1
                    }, true, _edgeOffset, false);

                    vertexList.AddRange(pointArray.ToVector2Array());
                }
                else
                {
                    for (int i = 0; i < _edgeVertexIndexRangeList.Count; i++)
                    {
                        Vector3[] pointArray = GetPointArrayFromIndexRange(_edgeVertexIndexRangeList[i], false, _edgeOffset * 2.0f, false);

                        vertexList.AddRange(pointArray.ToVector2Array());
                    }
                }

                UpdateMeshInternal(vertexList.ToArray(), _dropShadowMesh, _dropShadowMeshFilter, _meshUVType);
            }
        }

        public void AlignToCenter()
        {
            if (VertexList.Count < 1)
            {
                return;
            }

            float totalX = 0.0f;
            float totalY = 0.0f;
            for (int i = 0; i < VertexList.Count; i++)
            {
                totalX += VertexList[i].x;
                totalY += VertexList[i].y;
            }
            Vector3 centerPoint = new Vector2(totalX / VertexList.Count, totalY / VertexList.Count);

            for (int i = 0; i < VertexList.Count; i++)
            {
                VertexList[i] -= (Vector2)centerPoint;
            }

            UpdateMeshes();
        }

        public void CreatePolygonCollider()
        {
            if (_polygonCollider2D == null)
            {
                GameObject go = new GameObject();
                go.name = "PolygonCollider";
                go.transform.SetParent(transform);
                go.layer = LayerMask.NameToLayer(GameConstants.WALL_LAYER_NAME);
                _polygonCollider2D = go.AddComponent<PolygonCollider2D>();
            }

            List<Vector2> verticeList = new List<Vector2>();
            for (int i = 0; i < VertexList.Count; i++)
            {
                Vector2 position = transform.rotation * VertexList[i] + transform.position;
                verticeList.Add(position);
            }

            _polygonCollider2D.points = verticeList.ToArray();
        }

        public void ClearPolygonCollider()
        {
            if (_polygonCollider2D != null)
            {
                DestroyImmediate(_polygonCollider2D.gameObject);
            }
        }

        public void CreateEdgeRenderer()
        {
            if (_loop)
            {
                Vector3[] pointArray = GetPointArrayFromIndexRange(new EdgeVertexIndexRange
                {
                    From = 0,
                    To = VertexList.Count - 1
                }, true, _edgeOffset, true);

                CreateEdgeRendererInternal(0, pointArray, true);
            }
            else
            {
                for (int i = 0; i < _edgeVertexIndexRangeList.Count; i++)
                {
                    Vector3[] pointArray = GetPointArrayFromIndexRange(_edgeVertexIndexRangeList[i], false, _edgeOffset, true);

                    CreateEdgeRendererInternal(i, pointArray);
                }
            }
        }

        public void ClearEdgeRenderer()
        {
            if (_edgeRendererList != null)
            {
                for (int i = _edgeRendererList.Count - 1; i >= 0; i--)
                {
                    if (_edgeRendererList[i] != null)
                    {
                        DestroyImmediate(_edgeRendererList[i].gameObject);
                    }
                    _edgeRendererList.RemoveAt(i);
                }
            }
        }

        public void CreateEdgeCollider()
        {
            if (_loop)
            {
                List<Vector3> pointList = GetPointArrayFromIndexRange(new EdgeVertexIndexRange
                {
                    From = 0,
                    To = VertexList.Count - 1
                }, true, _edgeOffset, false).ToList();
                pointList.Add(pointList[0]);

                CreateEdgeColliderInternal(0, pointList.ToArray().ToVector2Array());
            }
            else
            {
                for (int i = 0; i < _edgeVertexIndexRangeList.Count; i++)
                {
                    Vector3[] pointArray = GetPointArrayFromIndexRange(_edgeVertexIndexRangeList[i], false, _edgeOffset, true);

                    CreateEdgeColliderInternal(i, pointArray.ToVector2Array());
                }
            }
        }

        public void ClearEdgeCollider()
        {
            if (_edgeCollider2DList != null)
            {
                for (int i = _edgeCollider2DList.Count - 1; i >= 0; i--)
                {
                    if (_edgeCollider2DList[i] != null)
                    {
                        DestroyImmediate(_edgeCollider2DList[i].gameObject);
                    }
                    _edgeCollider2DList.RemoveAt(i);
                }
            }
        }

        public void CreateDropShadowGameObject()
        {
            if (_dropShadowGO == null)
            {
                _dropShadowGO = new GameObject();
                _dropShadowGO.name = "DropShadow";
                _dropShadowGO.transform.SetParent(transform, false);
                _dropShadowMeshFilter = _dropShadowGO.AddComponent<MeshFilter>();
                _dropShadowMeshRenderer = _dropShadowGO.AddComponent<MeshRenderer>();
#if UNITY_EDITOR
                _dropShadowMeshRenderer.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Game/Materials/SpriteDropShadow.mat");
#endif
                SortingGroup sortingGroup = _dropShadowGO.AddComponent<SortingGroup>();
                sortingGroup.sortingLayerName = GameConstants.OBJECT_SORTING_LAYER_NAME;
                sortingGroup.sortingOrder = 9;
            }
        }

        public void ClearDropShadowGameObject()
        {
            if (_dropShadowGO != null)
            {
                DestroyImmediate(_dropShadowGO);
            }
        }

        private void UpdateComponents()
        {
            if (_visualGO == null)
            {
                _visualGO = transform.Find("Visual")?.gameObject;
                if (_visualGO != null)
                {
                    _visualMeshFilter = _visualGO.GetComponent<MeshFilter>();
                    _visualMeshRenderer = _visualGO.GetComponent<MeshRenderer>();
                    _visualMesh = _visualMeshFilter.sharedMesh;
                }
                else
                {
                    _visualGO = new GameObject();
                    _visualGO.name = "Visual";
                    _visualGO.transform.SetParent(transform, false);
                    _visualMeshFilter = _visualGO.AddComponent<MeshFilter>();
                    _visualMeshRenderer = _visualGO.AddComponent<MeshRenderer>();
#if UNITY_EDITOR
                    _visualMeshRenderer.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Game/Materials/Tube.mat");
#endif
                    SortingGroup sortingGroup = _visualGO.AddComponent<SortingGroup>();
                    sortingGroup.sortingLayerName = GameConstants.OBJECT_SORTING_LAYER_NAME;
                    sortingGroup.sortingOrder = 10;
                }
            }

            if (_polygonCollider2D == null)
            {
                _polygonCollider2D = GetComponentInChildren<PolygonCollider2D>();
            }

            if (_edgeCollider2DList == null)
            {
                _edgeCollider2DList = new List<EdgeCollider2D>(GetComponentsInChildren<EdgeCollider2D>());
            }

            if (_edgeRendererList == null)
            {
                _edgeRendererList = new List<LineRenderer>(GetComponentsInChildren<LineRenderer>());
            }

            if (_canDropShadow)
            {
                if (_dropShadowGO == null)
                {
                    _dropShadowGO = transform.Find("DropShadow")?.gameObject;
                    if (_dropShadowGO != null)
                    {
                        _dropShadowMeshFilter = _dropShadowGO.GetComponent<MeshFilter>();
                        _dropShadowMeshRenderer = _dropShadowGO.GetComponent<MeshRenderer>();
                        _dropShadowMesh = _dropShadowMeshFilter.sharedMesh;
                    }
                    else
                    {
                        CreateDropShadowGameObject();
                    }
                }
            }
            else
            {
                ClearDropShadowGameObject();
            }
        }

        private void UpdateMeshInternal(Vector2[] vertexArray, Mesh mesh, MeshFilter filter, UVType UVtype)
        {
            Triangulator triangulator = new Triangulator(vertexArray);
            int[] triangleArray = triangulator.Triangulate();

            if (mesh == null)
            {
                mesh = new Mesh();
                mesh.name = "CustomMesh";
                mesh.hideFlags = HideFlags.HideAndDontSave;
            }

            mesh.Clear();
            mesh.vertices = vertexArray.ToVector3Array();
            mesh.triangles = triangleArray;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            filter.mesh = mesh;

            Vector2[] uvArray = new Vector2[vertexArray.Length];
            if (UVtype == UVType.Fit)
            {
                for (int i = 0; i < vertexArray.Length; i++)
                {
                    uvArray[i] = new Vector2((vertexArray[i].x - mesh.bounds.min.x) / (mesh.bounds.max.x - mesh.bounds.min.x),
                        (vertexArray[i].y - mesh.bounds.min.y) / (mesh.bounds.max.y - mesh.bounds.min.y));
                }
            }
            else
            {
                for (int i = 0; i < vertexArray.Length; i++)
                {
                    uvArray[i] = new Vector2(vertexArray[i].x - mesh.bounds.min.x, vertexArray[i].y - mesh.bounds.min.y);
                }
            }
            mesh.uv = uvArray;
        }

        private void CreateEdgeRendererInternal(int index, Vector3[] pointArray, bool loop = false)
        {
            if (_edgeRendererList == null)
            {
                _edgeRendererList = new List<LineRenderer>();
            }

            LineRenderer edgeRenderer;
            if (index < _edgeRendererList.Count)
            {
                edgeRenderer = _edgeRendererList[index];
            }
            else
            {
                GameObject go = new GameObject();
                go.name = "EdgeRenderer" + (index == 0 ? string.Empty : "_" + index.ToString());
                go.transform.SetParent(transform);
                edgeRenderer = go.AddComponent<LineRenderer>();
                edgeRenderer.loop = loop;
                edgeRenderer.startWidth = _edgeWidth;
                edgeRenderer.endWidth = _edgeWidth;
#if UNITY_EDITOR
                edgeRenderer.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Game/Materials/Edge.mat");
#endif
                edgeRenderer.sortingLayerName = GameConstants.OBJECT_SORTING_LAYER_NAME;
                edgeRenderer.sortingOrder = 20;

                _edgeRendererList.Add(edgeRenderer);
            }

            edgeRenderer.positionCount = pointArray.Length;
            edgeRenderer.SetPositions(pointArray);
        }

        private void CreateEdgeColliderInternal(int index, Vector2[] pointArray)
        {
            if (_edgeCollider2DList == null)
            {
                _edgeCollider2DList = new List<EdgeCollider2D>();
            }

            EdgeCollider2D edgeCollider;
            if (index < _edgeCollider2DList.Count)
            {
                edgeCollider = _edgeCollider2DList[index];
            }
            else
            {
                GameObject go = new GameObject();
                go.name = "EdgeCollider" + (index == 0 ? string.Empty : "_" + index.ToString());
                go.transform.SetParent(transform);
                go.layer = LayerMask.NameToLayer(GameConstants.WALL_LAYER_NAME);
                edgeCollider = go.AddComponent<EdgeCollider2D>();
                edgeCollider.edgeRadius = _edgeWidth * 0.5f;

                _edgeCollider2DList.Add(edgeCollider);
            }

            edgeCollider.points = pointArray;
        }

        private Vector3[] GetPointArrayFromIndexRange(EdgeVertexIndexRange indexRange, bool loop, float offsetMultiplier, bool relative)
        {
            List<Vector3> vertexList = new List<Vector3>();
            int length = indexRange.To - indexRange.From + 1;
            for (int i = indexRange.From; i <= indexRange.To; i++)
            {
                Vector3 point = VertexList[i];
                if (relative)
                {
                    point = transform.rotation * point + transform.position;
                }
                Vector3 prevPoint = Vector3.zero;
                Vector3 nextPoint = Vector3.zero;
                if (loop || (i > indexRange.From && i < indexRange.To))
                {
                    prevPoint = VertexList[(i - 1 - indexRange.From + length) % length + indexRange.From];
                    nextPoint = VertexList[(i + 1 - indexRange.From + length) % length + indexRange.From];
                }
                else if (i == indexRange.From)
                {
                    prevPoint = VertexList[indexRange.Prev];
                    nextPoint = VertexList[i + 1];
                }
                else if (i == indexRange.To)
                {
                    prevPoint = VertexList[i - 1];
                    nextPoint = VertexList[indexRange.Next];
                }

                if (relative)
                {
                    prevPoint = transform.rotation * prevPoint + transform.position;
                    nextPoint = transform.rotation * nextPoint + transform.position;
                }

                Vector3 v1 = (point - prevPoint).normalized;
                Vector3 v2 = (nextPoint - point).normalized;
                float sign = Mathf.Sign(Vector3.Cross(v1, v2).z);
                Vector3 offset = (sign * (v1 - v2)).normalized;
                float angle = Mathf.Abs(90.0f - Vector3.Angle(v1, offset)) * Mathf.Deg2Rad;
                offset *= 1.0f / Mathf.Cos(angle) * offsetMultiplier;

                point += offset;
                vertexList.Add(point);
            }

            return vertexList.ToArray();
        }
    }
}
