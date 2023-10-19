using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace VinhLB
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(PathCreator))]
    public class MeshCreator : MonoBehaviour
    {
        [Range(0.1f, 1.5f)]
        public float Spacing = 1.0f;
        public float Width = 1.0f;
        public float TextureTiling = 1.0f;
        public bool AutoUpdate = false;

        private PathCreator _pathCreator;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Mesh _mesh;

        public void UpdateMesh()
        {
            if (_pathCreator == null)
            {
                _pathCreator = GetComponent<PathCreator>();
            }
            if (_meshFilter == null)
            {
                _meshFilter = GetComponent<MeshFilter>();
            }
            if (_meshRenderer == null)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }

            Vector2[] pointArray = _pathCreator.Path.CalculateEvenlySpacedPoints(Spacing);
            _meshFilter.mesh = CreateMesh(pointArray, _pathCreator.Path.IsClosed);

            int textureRepeat = Mathf.RoundToInt(TextureTiling * pointArray.Length * Spacing * 0.05f);
            _meshRenderer.sharedMaterial.mainTextureScale = new Vector2(1.0f, textureRepeat);
        }

        private Mesh CreateMesh(Vector2[] pointArray, bool isClosed)
        {
            Vector3[] vertexArray = new Vector3[pointArray.Length * 2];
            Vector2[] uvArray = new Vector2[vertexArray.Length];
            int triangleCount = 2 * (pointArray.Length - 1) + (isClosed ? 2 : 0);
            int[] triangleArray = new int[triangleCount * 3];
            int vertexIndex = 0;
            int triangleIndex = 0;

            for (int i = 0; i < pointArray.Length; i++)
            {
                Vector2 forward = Vector2.zero;
                if (i < pointArray.Length - 1)
                {
                    forward += pointArray[(i + 1) % pointArray.Length] - pointArray[i];
                }
                if (i > 0)
                {
                    forward += pointArray[i] - pointArray[(i - 1) % pointArray.Length];
                }
                forward.Normalize();
                Vector2 left = new Vector2(-forward.y, forward.x);

                vertexArray[vertexIndex] = pointArray[i] + left * Width * 0.5f;
                vertexArray[vertexIndex + 1] = pointArray[i] - left * Width * 0.5f;

                if (i == pointArray.Length - 1 && isClosed)
                {
                    vertexArray[vertexIndex] = pointArray[i] + left * Width * 0.5f + forward * 0.02f;
                    vertexArray[vertexIndex + 1] = pointArray[i] - left * Width * 0.5f + forward * 0.02f;
                }

                float completionPercent = i / (float)(pointArray.Length - 1);
                float v = 1 - Mathf.Abs(2.0f * completionPercent - 1);
                uvArray[vertexIndex] = new Vector2(0.0f, v);
                uvArray[vertexIndex + 1] = new Vector2(1.0f, v);

                if (i < pointArray.Length - 1 || isClosed)
                {
                    triangleArray[triangleIndex] = vertexIndex;
                    triangleArray[triangleIndex + 1] = (vertexIndex + 2) % vertexArray.Length;
                    triangleArray[triangleIndex + 2] = vertexIndex + 1;

                    triangleArray[triangleIndex + 3] = vertexIndex + 1;
                    triangleArray[triangleIndex + 4] = (vertexIndex + 2) % vertexArray.Length;
                    triangleArray[triangleIndex + 5] = (vertexIndex + 3) % vertexArray.Length;
                }

                vertexIndex += 2;
                triangleIndex += 6;
            }

            if (_mesh == null)
            {
                _mesh = new Mesh();
                _mesh.name = "Mesh";
            }
            _mesh.Clear();
            _mesh.vertices = vertexArray;
            _mesh.triangles = triangleArray;
            _mesh.uv = uvArray;

            return _mesh;
        }
    }
}
