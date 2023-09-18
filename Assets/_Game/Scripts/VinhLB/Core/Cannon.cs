using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VinhLB
{
    public class Cannon : MonoBehaviour
    {
        [Header("Referances")]
        [SerializeField]
        private Transform _spawnPoint;
        [SerializeField]
        private Button _spawnButton;
        [SerializeField]
        private Button _addButton;
        [SerializeField]
        private Button _rainbowButton;
        [SerializeField]
        private TMP_Text _amountText;

        [Header("Settings")]
        [SerializeField]
        private ColorType _ballColorType;
        [SerializeField]
        private int _maxBallAmount;
        [SerializeField]
        private float _spawnRate = 1.0f;

        private bool _canSpawn;
        private float _spawnTime;
        private Stack<BallType> _ballTypeStack = new Stack<BallType>();

        private void Awake()
        {
            _spawnButton.onClick.AddListener(() =>
            {
                if (_canSpawn && _ballTypeStack.Count > 0)
                {
                    _canSpawn = false;
                    ICommand command = new SpawnCommand(this);
                    CommandInvoker.ExecuteCommand(command);
                }
            });
            _addButton.onClick.AddListener(() =>
            {
                ICommand command = new AddCommand(this, BallType.Normal);
                CommandInvoker.ExecuteCommand(command);
            });
            _rainbowButton.onClick.AddListener(() =>
            {
                ICommand command = new AddCommand(this, BallType.Rainbow);
                CommandInvoker.ExecuteCommand(command);
            });
        }

        private void Start()
        {
            for (int i = 0; i < _maxBallAmount; i++)
            {
                PushBallType(BallType.Normal);
            }

            _canSpawn = true;
            _spawnTime = 0.0f;
        }

        private void Update()
        {
            if (!_canSpawn)
            {
                _spawnTime += Time.deltaTime;
                if (_spawnTime >= 1.0f / _spawnRate)
                {
                    _canSpawn = true;
                    _spawnTime = 0.0f;
                }
            }
        }

        public Ball SpawnBall()
        {
            Ball ball = BallPool.Instance.SpawnBall(_spawnPoint.position, _spawnPoint.rotation,
                PullBallType(), _ballColorType);

            return ball;
        }

        public void RetrieveBall(Ball ball)
        {
            ball.ReturnToPool();
            PushBallType(ball.GetBallType());
        }

        public void PushBallType(BallType ballType)
        {
            _ballTypeStack.Push(ballType);
            _amountText.text = _ballTypeStack.Count.ToString();

            UpdateVisual(_ballTypeStack.Peek());
        }

        public BallType PullBallType()
        {
            BallType ballType = _ballTypeStack.Pop();
            _amountText.text = _ballTypeStack.Count.ToString();

            if (_ballTypeStack.Count > 0)
            {
                UpdateVisual(_ballTypeStack.Peek());
            }

            return ballType;
        }

        private void UpdateVisual(BallType ballType)
        {
            _spawnButton.image.sprite = ResourceManager.Instance.GetBallSpriteByType(ballType);

            switch (ballType)
            {
                case BallType.Normal:
                    _spawnButton.image.color = ResourceManager.Instance.GetColorByColorType(_ballColorType);
                    break;
                case BallType.Rainbow:
                    _spawnButton.image.color = Color.white;
                    break;
            }
        }
    }
}
