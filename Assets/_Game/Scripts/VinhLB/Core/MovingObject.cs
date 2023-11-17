using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VinhLB
{

    public class MovingObject : MonoBehaviour
    {
        public bool IsLoopPath => _isLoopPath;
        public bool ShowPath => _showPath;

        public List<Vector2> PointList;

        [SerializeField]
        private Transform _objectTransform;
        [SerializeField]
        private bool _isLoopPath = true;
        [SerializeField]
        private int _startingPointIndex = 0;
        [SerializeField]
        private float _speed = 1.0f;
        [SerializeField]
        private bool _showPath = false;
        [SerializeField, HideInInspector]
        private Material _lineMaterial;
        [SerializeField, HideInInspector]
        private float _lineWidth = 0.1f;

        private int _currentIndex;
        private int _modifiedValue;
        private LineRenderer _lineRenderer;

        private void Reset()
        {
            UpdateComponents();

            CreatePath();

            if (_showPath)
            {
                DrawPath();
            }
        }

        private void Start()
        {
            _objectTransform.position = PointList[_startingPointIndex];
            _currentIndex = _startingPointIndex;
            _modifiedValue = 1;
        }

        private void Update()
        {
            if (Vector2.Distance(_objectTransform.position, PointList[_currentIndex]) < GameConstants.FLOOR_MIN_DISTANCE)
            {
                if (_isLoopPath)
                {
                    _currentIndex = (_currentIndex + 1) % PointList.Count;
                }
                else
                {
                    if (_currentIndex == 0)
                    {
                        _modifiedValue = 1;
                    }
                    else if (_currentIndex == PointList.Count - 1)
                    {
                        _modifiedValue = -1;
                    }

                    _currentIndex += _modifiedValue;
                }
            }

            _objectTransform.position = Vector2.MoveTowards(_objectTransform.position, PointList[_currentIndex], _speed * Time.deltaTime);
        }

        public void UpdateComponents()
        {
#if UNITY_EDITOR
            _lineMaterial ??= AssetDatabase.LoadAssetAtPath<Material>("Assets/_Game/Materials/Line.mat");
#endif
            if (_lineRenderer == null)
            {
                _lineRenderer = GetComponentInChildren<LineRenderer>();
            }
        }

        public void CreatePath()
        {
            PointList = new List<Vector2>
            {
                Vector2.left,
                Vector2.right
            };
        }

        public void DrawPath()
        {
            if (_lineRenderer == null)
            {
                GameObject go = new GameObject();
                go.name = "PathRenderer";
                go.transform.SetParent(transform, false);

                _lineRenderer = go.AddComponent<LineRenderer>();
                _lineRenderer.numCapVertices = 10;
                _lineRenderer.numCornerVertices = 10;
                _lineRenderer.sortingLayerName = GameConstants.OBJECT_SORTING_LAYER_NAME;
                _lineRenderer.sortingOrder = 20;
            }

            _lineRenderer.startWidth = _lineWidth;
            _lineRenderer.endWidth = _lineWidth;
            _lineRenderer.sharedMaterial = _lineMaterial;

            if (PointList.Count > 2 && _isLoopPath)
            {
                _lineRenderer.loop = true;
            }
            else
            {
                _lineRenderer.loop = false;
            }
            _lineRenderer.positionCount = PointList.Count;
            _lineRenderer.SetPositions(PointList.ToArray().ToVector3Array());
        }

        public void ClearPath()
        {
            if (_lineRenderer != null)
            {
                DestroyImmediate(_lineRenderer.gameObject);
            }
        }
    }
}
