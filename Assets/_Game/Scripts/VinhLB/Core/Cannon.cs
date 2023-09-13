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
        private TMP_Text _amountText;

        [Header("Settings")]
        [SerializeField]
        private ColorType _ballColorType;
        [SerializeField]
        private int _maxBallAmount;
        [SerializeField]
        private float _spawnRate = 1.0f;

        private int _ballAmount;
        private bool _canSpawn;
        private float _spawnTime;

        private void Awake()
        {
            _spawnButton.onClick.AddListener(() =>
            {
                if (_canSpawn && _ballAmount > 0)
                {
                    _canSpawn = false;
                    ICommand command = new SpawnCommand(this);
                    CommandInvoker.ExecuteCommand(command);
                }
            });
        }

        private void Start()
        {
            _spawnButton.image.color = LevelManager.GetColorByColorType(_ballColorType);

            _ballAmount = _maxBallAmount;
            _amountText.text = _ballAmount.ToString();

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

        public void ModifyBallAmount(int value)
        {
            _ballAmount += value;
            _amountText.text = _ballAmount.ToString();
        }

        public Ball SpawnBall()
        {
            Ball ball = BallPool.Instance.SpawnBall(_spawnPoint.position, _spawnPoint.rotation, _ballColorType);
            ModifyBallAmount(-1);

            return ball;
        }

        public void RetrieveBall(Ball ball)
        {
            ball.ReturnToPool();
            ModifyBallAmount(1);
        }
    }
}
