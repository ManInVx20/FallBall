using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VinhLB
{
    [ExecuteInEditMode, RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class DrawPolygon2D : MonoBehaviour
    {
        private const string WALL_LAYER_NAME = "Wall";

        public enum UVType
        {
            Fit = 0,
            Repeat = 1
        }

        [System.Serializable]
        public struct EdgeVerticeIndexRange
        {
            public bool LoopThrough;
            public int From;
            public int To;
        }

        public List<Vector2> VerticeList;

        [SerializeField]
        private UVType _meshUVType;
        [SerializeField]
        private Material _rendererMaterial;
        [SerializeField]
        private List<EdgeVerticeIndexRange> _edgeVerticeIndexRangeList;

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Mesh _mesh;
        private PolygonCollider2D _polygonCollider2D;
        private List<EdgeCollider2D> _edgeCollider2DList;

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            _polygonCollider2D = GetComponentInChildren<PolygonCollider2D>();
            _edgeCollider2DList = new List<EdgeCollider2D>(GetComponentsInChildren<EdgeCollider2D>());
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
            if (VerticeList == null)
            {
                return;
            }

            Vector2[] vertice2DArray = VerticeList.ToArray();

            Triangulator triangulator = new Triangulator(vertice2DArray);
            int[] indiceArray = triangulator.Triangulate();

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
            _mesh.triangles = indiceArray;
            _mesh.RecalculateBounds();
            _mesh.RecalculateNormals();

            _meshFilter.mesh = _mesh;

            Vector2[] uvArray = new Vector2[VerticeList.Count];
            if (_meshUVType == UVType.Fit)
            {
                for (int i = 0; i < VerticeList.Count; i++)
                {
                    uvArray[i] = new Vector2((VerticeList[i].x - _mesh.bounds.min.x) / (_mesh.bounds.max.x - _mesh.bounds.min.x),
                        (VerticeList[i].y - _mesh.bounds.min.y) / (_mesh.bounds.max.y - _mesh.bounds.min.y));
                }
            }
            else
            {
                for (int i = 0; i < VerticeList.Count; i++)
                {
                    uvArray[i] = new Vector2(VerticeList[i].x - _mesh.bounds.min.x, VerticeList[i].y - _mesh.bounds.min.y);
                }
            }
            _mesh.uv = uvArray;

            _meshRenderer.material = _rendererMaterial;
        }

        public void CreatePolygonCollider()
        {
            if (_polygonCollider2D == null)
            {
                GameObject go = new GameObject();
                go.name = "PolygonCollider";
                go.transform.SetParent(transform);
                go.layer = LayerMask.NameToLayer(WALL_LAYER_NAME);
                _polygonCollider2D = go.AddComponent<PolygonCollider2D>();
            }

            List<Vector2> verticeList = new List<Vector2>();
            for (int i = 0; i < VerticeList.Count; i++)
            {
                Vector2 position = transform.rotation * VerticeList[i] + transform.position;
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

        public void CreateEdgeCollider()
        {
            if (_edgeCollider2DList == null)
            {
                _edgeCollider2DList = new List<EdgeCollider2D>();
            }

            for (int i = 0; i < _edgeVerticeIndexRangeList.Count; i++)
            {
                EdgeCollider2D edgeCollider;
                if (i < _edgeCollider2DList.Count)
                {
                    edgeCollider = _edgeCollider2DList[i];
                }
                else
                {
                    GameObject go = new GameObject();
                    go.name = "EdgeCollider" + (i == 0 ? string.Empty : "_" + i.ToString());
                    go.transform.SetParent(transform);
                    go.layer = LayerMask.NameToLayer(WALL_LAYER_NAME);
                    edgeCollider = go.AddComponent<EdgeCollider2D>();
                }

                List<Vector2> verticeList = new List<Vector2>();
                if (!_edgeVerticeIndexRangeList[i].LoopThrough)
                {
                    for (int j = _edgeVerticeIndexRangeList[i].From; j <= _edgeVerticeIndexRangeList[i].To; j++)
                    {
                        Vector2 position = transform.rotation * VerticeList[j] + transform.position;
                        verticeList.Add(position);
                    }
                }
                else
                {
                    for (int j = _edgeVerticeIndexRangeList[i].From; j < VerticeList.Count; j++)
                    {
                        Vector2 position = transform.rotation * VerticeList[j] + transform.position;
                        verticeList.Add(position);
                    }
                    for (int j = 0; j <= _edgeVerticeIndexRangeList[i].To; j++)
                    {
                        Vector2 position = transform.rotation * VerticeList[j] + transform.position;
                        verticeList.Add(position);
                    }
                }
                edgeCollider.points = verticeList.ToArray();

                if (i >= _edgeCollider2DList.Count)
                {
                    _edgeCollider2DList.Add(edgeCollider);
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
    }
}
