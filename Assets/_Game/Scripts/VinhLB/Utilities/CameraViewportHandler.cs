using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    [RequireComponent(typeof(Camera)), ExecuteInEditMode]
    public class CameraViewportHandler : MonoSingleton<CameraViewportHandler>
    {
        public enum Constraint
        {
            Landscape = 0,
            Potrait = 1
        }

        [SerializeField]
        private Camera _camera;
        [SerializeField]
        private Constraint _constraint = Constraint.Potrait;
        [SerializeField]
        private Color _wireColor = Color.white;
        [SerializeField]
        private float _unitsSize = 1.0f;
        [SerializeField]
        private bool _execureInUpdate;

        private float _width;
        private float _height;

        private Vector3 _bottomLeft;
        private Vector3 _bottomCenter;
        private Vector3 _bottomRight;

        private Vector3 _midLeft;
        private Vector3 _midCenter;
        private Vector3 _midRight;

        private Vector3 _topLeft;
        private Vector3 _topCenter;
        private Vector3 _topRight;

        public float Width => _width;
        public float Height => _height;
        public Vector3 BottomLeft => _bottomLeft;
        public Vector3 BottomCenter => _bottomCenter;
        public Vector3 BottomRight => _bottomRight;
        public Vector3 MidLeft => _midLeft;
        public Vector3 MidCenter => _midCenter;
        public Vector3 MidRight => _midRight;
        public Vector3 TopLeft => _topLeft;
        public Vector3 TopCenter => _topCenter;
        public Vector3 TopRight => _topRight;

        private void Awake()
        {
            if (_camera == null)
            {
                _camera = GetComponent<Camera>();
            }

            ComputeResolution();
        }

        private void Update()
        {
#if UNITY_EDITOR
            ComputeResolution();
#endif
        }

        private void ComputeResolution()
        {
            if (_constraint == Constraint.Landscape)
            {
                _camera.orthographicSize = 1.0f / _camera.aspect * _unitsSize / 2.0f;
            }
            else
            {
                _camera.orthographicSize = _unitsSize / 2.0f;
            }

            _height = _camera.orthographicSize * 2.0f;
            _width = _height * _camera.aspect;

            float cameraX, cameraY;
            cameraX = _camera.transform.position.x;
            cameraY = _camera.transform.position.y;

            float leftX, rightX, topY, bottomY;
            leftX = cameraX - _width / 2.0f;
            rightX = cameraX + _width / 2.0f;
            topY = cameraY + _height / 2.0f;
            bottomY = cameraY - _height / 2.0f;

            _bottomLeft = new Vector3(leftX, bottomY, 0.0f);
            _bottomCenter = new Vector3(cameraX, bottomY, 0.0f);
            _bottomRight = new Vector3(rightX, bottomY, 0.0f);

            _midLeft = new Vector3(leftX, cameraY, 0.0f);
            _midCenter = new Vector3(cameraX, cameraY, 0.0f);
            _midRight = new Vector3(rightX, cameraY, 0.0f);

            _topLeft = new Vector3(leftX, topY, 0.0f);
            _midCenter = new Vector3(cameraX, topY, 0.0f);
            _topRight = new Vector3(rightX, topY, 0.0f);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = _wireColor;

            Matrix4x4 temp = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            if (_camera.orthographic)
            {
                float spread = _camera.farClipPlane - _camera.nearClipPlane;
                float center = (_camera.farClipPlane + _camera.nearClipPlane) * 0.5f;
                Gizmos.DrawWireCube(new Vector3(0.0f, 0.0f, center),
                    new Vector3(_camera.orthographicSize * 2.0f * _camera.aspect, _camera.orthographicSize * 2.0f, spread));
            }
            else
            {
                Gizmos.DrawFrustum(Vector3.zero, _camera.fieldOfView, _camera.farClipPlane, _camera.nearClipPlane, _camera.aspect);
            }
            Gizmos.matrix = temp;
        }
#endif
    }
}
