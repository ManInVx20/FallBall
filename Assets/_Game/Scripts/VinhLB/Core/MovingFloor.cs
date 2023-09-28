using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{

    public class MovingFloor : MonoBehaviour
    {
        [SerializeField]
        private Transform _floorTransform;
        [SerializeField]
        private Transform[] _pointArray;
        [SerializeField]
        private int _startingPointIndex = 0;
        [SerializeField]
        private float _speed = 1.0f;

        private int _currentIndex;

        private void Start()
        {
            _floorTransform.position = _pointArray[_startingPointIndex].position;
        }

        private void Update()
        {
            if (Vector2.Distance(_floorTransform.position, _pointArray[_currentIndex].position) < GameConstants.FLOOR_MIN_DISTANCE)
            {
                _currentIndex = (_currentIndex + 1) % _pointArray.Length;
            }

            _floorTransform.position = Vector2.MoveTowards(_floorTransform.position, 
                _pointArray[_currentIndex].position, _speed * Time.deltaTime);
        }
    }
}
