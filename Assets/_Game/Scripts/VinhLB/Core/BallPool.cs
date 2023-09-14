using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class BallPool : MonoSingleton<BallPool>
    {
        [SerializeField]
        private GameObject _ballPrefab;

        private ObjectPool<Ball> _ballPool;

        protected override void Awake()
        {
            base.Awake();

            _ballPool = new ObjectPool<Ball>(_ballPrefab, OnPullCallback, OnPushCallback);
        }

        public Ball SpawnBall(Vector3 position, Quaternion rotation, ColorType colorType)
        {
            Ball ball = _ballPool.Pull();
            ball.transform.SetPositionAndRotation(position, rotation);
            ball.SetColorType(colorType);

            return ball;
        }

        public void RetrieveAll()
        {
            _ballPool.RetrieveAll();
        }

        private void OnPullCallback(Ball ball)
        {
            ball.ResetState();
        }

        private void OnPushCallback(Ball ball)
        {
            ball.ResetState();
        }
    }
}
