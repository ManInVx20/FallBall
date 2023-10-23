using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VinhLB
{
    [ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
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
            public bool LoopThrough;
            public int From;
            public int To;
        }

        public List<Vector2> VertexList;

        [SerializeField]
        private UVType _meshUVType;
        [SerializeField]
        private Material _rendererMaterial;
        [SerializeField]
        private bool _loop = false;
        [SerializeField]
        private List<EdgeVertexIndexRange> _edgeVertexIndexRangeList;
        [SerializeField]
        private float _edgeWidth = 0.06f;
        [SerializeField]
        private float _edgeOffset = 0.03f;

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Mesh _mesh;
        private PolygonCollider2D _polygonCollider2D;
        private List<EdgeCollider2D> _edgeCollider2DList;
        private List<LineRenderer> _edgeRendererList;

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _polygonCollider2D = GetComponentInChildren<PolygonCollider2D>();
            _edgeCollider2DList = new List<EdgeCollider2D>(GetComponentsInChildren<EdgeCollider2D>());
            _edgeRendererList = new List<LineRenderer>(GetComponentsInChildren<LineRenderer>());
        }

        private void OnEnable()
        {
            UpdateMesh();
        }

        private void Reset()
        {
            UpdateMesh();
        }

        public void UpdateMesh()
        {
            if (VertexList == null)
            {
                return;
            }

            Vector2[] vertice2DArray = VertexList.ToArray();

            Triangulator triangulator = new Triangulator(vertice2DArray);
            int[] triangleArray = triangulator.Triangulate();

            Vector3[] vertice3DArray = new Vector3[vertice2DArray.Length];
            for (int i = 0; i < vertice2DArray.Length; i++)
            {
                vertice3DArray[i] = vertice2DArray[i];
            }

            if (_mesh == null)
            {
                _mesh = new Mesh();
                _mesh.name = "PolygonMesh";
                _mesh.hideFlags = HideFlags.HideAndDontSave;
            }
            _mesh.Clear();
            _mesh.vertices = vertice3DArray;
            _mesh.triangles = triangleArray;
            _mesh.RecalculateBounds();
            _mesh.RecalculateNormals();

            _meshFilter.mesh = _mesh;

            Vector2[] uvArray = new Vector2[VertexList.Count];
            if (_meshUVType == UVType.Fit)
            {
                for (int i = 0; i < VertexList.Count; i++)
                {
                    uvArray[i] = new Vector2((VertexList[i].x - _mesh.bounds.min.x) / (_mesh.bounds.max.x - _mesh.bounds.min.x),
                        (VertexList[i].y - _mesh.bounds.min.y) / (_mesh.bounds.max.y - _mesh.bounds.min.y));
                }
            }
            else
            {
                for (int i = 0; i < VertexList.Count; i++)
                {
                    uvArray[i] = new Vector2(VertexList[i].x - _mesh.bounds.min.x, VertexList[i].y - _mesh.bounds.min.y);
                }
            }
            _mesh.uv = uvArray;

            _meshRenderer.material = _rendererMaterial;
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

            UpdateMesh();
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
                }, true);

                CreateEdgeRendererInternal(0, pointArray, true);
            }
            else
            {
                for (int i = 0; i < _edgeVertexIndexRangeList.Count; i++)
                {
                    Vector3[] pointArray = GetPointArrayFromIndexRange(_edgeVertexIndexRangeList[i]);

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
            for (int i = 0; i < _edgeVertexIndexRangeList.Count; i++)
            {
                Vector3[] pointArray = GetPointArrayFromIndexRange(_edgeVertexIndexRangeList[i]);

                CreateEdgeColliderInternal(i, pointArray.ToVector2Array());
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

        private Vector3[] GetPointArrayFromIndexRange(EdgeVertexIndexRange indexRange, bool loop = false)
        {
            List<Vector3> vertexList = new List<Vector3>();
            if (!indexRange.LoopThrough)
            {
                CalculatePoint(vertexList, indexRange.From, indexRange.To, loop);
                //for (int i = indexRange.From; i <= indexRange.To; i++)
                //{
                //    Vector3 point = transform.rotation * VertexList[i] + transform.position;
                //    Vector3 prevPoint, nextPoint, offset;
                //    if (i == indexRange.From)
                //    {
                //        nextPoint = transform.rotation * VertexList[i + 1] + transform.position;
                //        Vector3 v = (nextPoint - point).normalized;
                //        offset = Quaternion.Euler(0.0f, 0.0f, -90.0f) * v * _edgeOffset;
                //    }
                //    else if (i == indexRange.To)
                //    {
                //        prevPoint = transform.rotation * VertexList[i - 1] + transform.position;
                //        Vector3 v = (point - prevPoint).normalized;
                //        offset = Quaternion.Euler(0.0f, 0.0f, -90.0f) * v * _edgeOffset;
                //    }
                //    else
                //    {
                //        prevPoint = transform.rotation * VertexList[i - 1] + transform.position;
                //        nextPoint = transform.rotation * VertexList[i + 1] + transform.position;
                //        Vector3 v1 = (point - prevPoint).normalized;
                //        Vector3 v2 = (nextPoint - point).normalized;
                //        float sign = Mathf.Sign(Vector3.Cross(v1, v2).z);
                //        offset = (sign * (v1 - v2)).normalized;
                //        float angle = Mathf.Abs(90.0f - Vector3.Angle(v1, offset)) * Mathf.Deg2Rad;
                //        offset *= 1.0f / Mathf.Cos(angle) * _edgeOffset;
                //    }
                //    point += offset;
                //    vertexList.Add(point);
                //}
            }
            else
            {
                CalculatePoint(vertexList, indexRange.From, VertexList.Count - 1, loop);
                CalculatePoint(vertexList, 0, indexRange.To, loop);
                //for (int i = indexRange.From; i < VertexList.Count; i++)
                //{
                //    Vector3 position = transform.rotation * VertexList[i] + transform.position;
                //    if (i > indexRange.From)
                //    {
                //        int prevIndex = i - indexRange.From - 1;
                //        Vector3 direction = (position - vertexList[prevIndex]).normalized;
                //        Vector3 offset = Vector3.Cross(direction, Vector3.forward).normalized * _edgeOffset;
                //        vertexList[prevIndex] += offset;
                //        position += offset;
                //    }
                //    vertexList.Add(position);
                //}
                //for (int i = 0; i <= indexRange.To; i++)
                //{
                //    Vector3 position = transform.rotation * VertexList[i] + transform.position;
                //    if (i > 0)
                //    {
                //        int prevIndex = i - 1;
                //        Vector3 direction = (position - vertexList[prevIndex]).normalized;
                //        Vector3 offset = Vector3.Cross(direction, Vector3.forward).normalized * _edgeOffset;
                //        vertexList[prevIndex] += offset;
                //        position += offset;
                //    }
                //    vertexList.Add(position);
                //}
            }

            return vertexList.ToArray();
        }

        private void CalculatePoint(List<Vector3> pointList, int from, int to, bool loop)
        {
            int length = to - from + 1;
            for (int i = from; i <= to; i++)
            {
                Vector3 point = transform.rotation * VertexList[i] + transform.position;
                Vector3 prevPoint, nextPoint;
                Vector3 offset = Vector3.zero;
                if (loop || (i > from && i < to))
                {
                    prevPoint = transform.rotation * VertexList[(i - 1 - from + length) % length + from] + transform.position;
                    nextPoint = transform.rotation * VertexList[(i + 1 - from + length) % length + from] + transform.position;
                    Vector3 v1 = (point - prevPoint).normalized;
                    Vector3 v2 = (nextPoint - point).normalized;
                    float sign = Mathf.Sign(Vector3.Cross(v1, v2).z);
                    offset = (sign * (v1 - v2)).normalized;
                    float angle = Mathf.Abs(90.0f - Vector3.Angle(v1, offset)) * Mathf.Deg2Rad;
                    offset *= 1.0f / Mathf.Cos(angle) * _edgeOffset;
                }
                else if (i == from)
                {
                    nextPoint = transform.rotation * VertexList[i + 1] + transform.position;
                    Vector3 v = (nextPoint - point).normalized;
                    offset = Quaternion.Euler(0.0f, 0.0f, -90.0f) * v * _edgeOffset;
                }
                else if (i == to)
                {
                    prevPoint = transform.rotation * VertexList[i - 1] + transform.position;
                    Vector3 v = (point - prevPoint).normalized;
                    offset = Quaternion.Euler(0.0f, 0.0f, -90.0f) * v * _edgeOffset;
                }
                point += offset;
                pointList.Add(point);
            }
        }
    }
}
