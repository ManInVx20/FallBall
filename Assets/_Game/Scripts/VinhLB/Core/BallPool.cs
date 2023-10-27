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
        [SerializeField]
        private bool _isAllInactive;

        private ObjectPool<Ball> _ballPool;

        private void Awake()
        {
            _ballPool = new ObjectPool<Ball>(_ballPrefab, OnPullCallback, OnPushCallback);
        }

        private void Update()
        {
            _isAllInactive = IsAllInactive();
        }

        public Ball SpawnBall(Vector3 position, Quaternion rotation, 
            BallType ballType = BallType.Normal, ColorType colorType = ColorType.White)
        {
            Ball ball = _ballPool.Pull();
            ball.transform.SetPositionAndRotation(position, rotation);
            ball.SetBallType(ballType);
            ball.SetColorType(colorType);
            ball.ResetState();

            return ball;
        }

        public bool IsAllInactive()
        {
            List<Ball> ballList = _ballPool.GetAll();

            for (int i = 0; i < ballList.Count; i++)
            {
                if (ballList[i].IsActive())
                {
                    return false;
                }
            }

            return true;
        }

        public void RetrieveAll()
        {
            _ballPool.RetrieveAll();
        }

        private void OnPullCallback(Ball ball)
        {
            //ball.ResetState();
        }

        private void OnPushCallback(Ball ball)
        {
            ball.ResetState();
        }
    }
}
