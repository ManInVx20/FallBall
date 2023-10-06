using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class MovingPartPolygon2D : MonoBehaviour
    {
        [SerializeField]
        private DrawPolygon2D _drawPolygon2D;
        [SerializeField]
        private int[] _veticeIndexArray;
        [SerializeField]
        private Transform _centerPoint;
        [SerializeField]
        private Transform[] _pointArray;
        [SerializeField]
        private float _speed = 1.0f;
        [SerializeField]
        private bool _calulateCenterPoint = true;
        [SerializeField]
        private bool _updatePolygonRenderer = true;
        [SerializeField]
        private bool _updatePolygonCollider = false;
        [SerializeField]
        private bool _updateEdgeRenderer = false;
        [SerializeField]
        private bool _updateEdgeCollider = false;


        private int _currentIndex;
        private Vector3 _moveDirection;

        private void Start()
        {
            Vector3[] positionArray = new Vector3[_veticeIndexArray.Length];
            for (int i = 0; i < _veticeIndexArray.Length; i++)
            {
                positionArray[i] = _drawPolygon2D.VerticeList[_veticeIndexArray[i]];
            }

            if (_calulateCenterPoint)
            {
                _centerPoint.position = Utilities.FindCenterPosition(positionArray);
            }

            _moveDirection = (_pointArray[_currentIndex].position - _centerPoint.position).normalized;
        }

        private void Update()
        {
            if (Vector3.Distance(_centerPoint.position, _pointArray[_currentIndex].position) < GameConstants.FLOOR_MIN_DISTANCE)
            {
                _currentIndex = (_currentIndex + 1) % _pointArray.Length;
                _moveDirection = (_pointArray[_currentIndex].position - _centerPoint.position).normalized;
            }

            Vector3 deltaPosition = _moveDirection * _speed * Time.deltaTime;
            _centerPoint.position += deltaPosition;
            for (int i = 0; i < _veticeIndexArray.Length; i++)
            {
                _drawPolygon2D.VerticeList[_veticeIndexArray[i]] += (Vector2)deltaPosition;
            }

            if (_updatePolygonRenderer)
            {
                _drawPolygon2D.UpdateMesh();
            }
            if (_updatePolygonCollider)
            {
                _drawPolygon2D.CreatePolygonCollider();
            }
            if (_updateEdgeRenderer)
            {
                _drawPolygon2D.CreateEdgeRenderer();
            }
            if (_updateEdgeCollider)
            {
                _drawPolygon2D.CreateEdgeCollider();
            }
        }
    }
}
