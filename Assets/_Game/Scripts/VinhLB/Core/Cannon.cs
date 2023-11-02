using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VinhLB
{
    public class Cannon : MonoBehaviour
    {
        public RectTransform WorldCanvasRectTF => _worldCanvasRectTF;

        [Header("Referances")]
        [SerializeField]
        private Transform _spawnPoint;
        [SerializeField]
        private RectTransform _worldCanvasRectTF;
        [SerializeField]
        private Button _spawnButton;
        [SerializeField]
        private Button _normalButton;
        [SerializeField]
        private Button _rainbowButton;
        [SerializeField]
        private Button _spikeButton;
        [SerializeField]
        private SpriteRenderer[] _rendererArray;
        [SerializeField]
        private Material _idleMaterial;
        [SerializeField]
        private Material _highlightMaterial;
        [SerializeField]
        private ParticleSystem _shootVFX;

        [Header("Settings")]
        [SerializeField]
        private ColorType _ballColorType;
        [SerializeField]
        private float _spawnRate = 1.0f;
        [SerializeField]
        private float _spawnForce = 3.0f;

        private bool _canSpawn;
        private float _spawnTime;
        private Stack<BallType> _specialBallTypeStack = new Stack<BallType>();

        private void Awake()
        {
            _spawnButton.onClick.AddListener(() =>
            {
                if (GameBoosterManager.Instance.CurrentActiveBooster == GameBoosterManager.ActiveBooster.None)
                {
                    if (_canSpawn && LevelManager.Instance.CurrentLevel.MovesLeft > 0)
                    {
                        _canSpawn = false;

                        ICommand command = new SpawnCommand(this);
                        CommandInvoker.ExecuteCommand(command);

                        _shootVFX.Play();
                    }
                }
                else
                {
                    if (GameBoosterManager.Instance.CurrentActiveBooster == GameBoosterManager.ActiveBooster.NormalBall)
                    {
                        AddBall(BallType.Normal);

                        GameBoosterManager.Instance.CurrentActiveBooster = GameBoosterManager.ActiveBooster.None;
                    }
                    else if (GameBoosterManager.Instance.CurrentActiveBooster == GameBoosterManager.ActiveBooster.RainbowBall)
                    {
                        AddBall(BallType.Rainbow);

                        GameBoosterManager.Instance.CurrentActiveBooster = GameBoosterManager.ActiveBooster.None;
                    }
                    else if (GameBoosterManager.Instance.CurrentActiveBooster == GameBoosterManager.ActiveBooster.SpikeBall)
                    {
                        AddBall(BallType.Spike);

                        GameBoosterManager.Instance.CurrentActiveBooster = GameBoosterManager.ActiveBooster.None;
                    }
                }

            });
            _normalButton.onClick.AddListener(() =>
            {
                AddBall(BallType.Normal);
            });
            _rainbowButton.onClick.AddListener(() =>
            {
                AddBall(BallType.Rainbow);
            });
            _spikeButton.onClick.AddListener(() =>
            {
                AddBall(BallType.Spike);
            });
        }

        private void Start()
        {
            _canSpawn = true;
            _spawnTime = 0.0f;

            UpdateVisual(BallType.Normal);
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
            Vector2 force = transform.rotation * Vector2.down * _spawnForce;
            ball.Push(force);

            return ball;
        }

        public void RetrieveBall(Ball ball)
        {
            ball.ReturnToPool();
            PushBallType(ball.GetBallType());
        }

        public void PushBallType(BallType ballType)
        {
            if (ballType != BallType.Normal)
            {
                _specialBallTypeStack.Push(ballType);
            }

            if (_specialBallTypeStack.Count > 0)
            {
                UpdateVisual(_specialBallTypeStack.Peek());
            }
            else
            {
                UpdateVisual(BallType.Normal);
            }
        }

        public BallType PullBallType()
        {
            BallType ballType = BallType.Normal;
            if (_specialBallTypeStack.Count > 0)
            {
                ballType = _specialBallTypeStack.Pop();
            }

            if (_specialBallTypeStack.Count > 0)
            {
                UpdateVisual(_specialBallTypeStack.Peek());
            }
            else
            {
                UpdateVisual(BallType.Normal);
            }

            return ballType;
        }

        public void AddBall(BallType ballType)
        {
            ICommand command = new AddCommand(this, ballType);
            CommandInvoker.ExecuteCommand(command);
        }

        public void ChangeMaterial(bool highlight)
        {
            for (int i = 0; i < _rendererArray.Length; i++)
            {
                if (highlight)
                {
                    _rendererArray[i].material = _highlightMaterial;
                }
                else
                {
                    _rendererArray[i].material = _idleMaterial;
                }
            }
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
                case BallType.Spike:
                    _spawnButton.image.color = Color.white;
                    break;
            }
        }
    }
}
