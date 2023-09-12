using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _ballPrefab;
        [SerializeField]
        private ColorType _ballColorType;
        [SerializeField]
        private KeyCode _spawnKeyCode;

        private ObjectPool<Ball> _ballPool;

        private void Awake()
        {
            _ballPool = new ObjectPool<Ball>(_ballPrefab, OnPullCallback, OnPushCallback);
        }

        private void Update()
        {
            if (Input.GetKeyDown(_spawnKeyCode))
            {
                _ballPool.Pull();
            }
        }

        private void OnPullCallback(Ball ball)
        {
            ball.transform.position = transform.position;
            ball.transform.rotation = Quaternion.identity;
            ball.SetColorType(_ballColorType);
        }

        private void OnPushCallback(Ball ball)
        {
            
        }
    }
}
