using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    [ExecuteInEditMode]
    public class Ball : MonoBehaviour, IPoolable<Ball>, IHasColor
    {
        [SerializeField]
        private Rigidbody2D _rigidbody2D;
        [SerializeField]
        private SpriteRenderer _ballRenderer;
        [SerializeField]
        private SpriteRenderer _doneRenderer;
        [SerializeField]
        private TrailRenderer _trailRenderer;

        [SerializeField]
        private BallType _ballType;
        [SerializeField]
        private ColorType _colorType;
        [SerializeField]
        private LayerMask _obstacleLayerMask;

        private Action<Ball> _onReturnAction;
        private Slot _currentSlot;
        private IEnumerator _waitToStickCoroutine;
        private float _despawnTimer = 0.0f;
        private float _despawnTimerMax = 1.0f;
        private bool _blinkingToDespawn = false;
        private Tween _blinkingToDespawnTween;

        private void OnEnable()
        {
            UpdateVisual();
        }

        private void Reset()
        {
            UpdateVisual();
        }

        private void Update()
        {
            if (_currentSlot == null && _rigidbody2D.velocity.sqrMagnitude < GameConstants.BALL_MIN_SQR_MAGNITUDE)
            {
                if (!_blinkingToDespawn)
                {
                    //Debug.Log("a");
                    _blinkingToDespawn = true;
                    _blinkingToDespawnTween = _ballRenderer.DOColor(Color.black, 0.25f).SetLoops(-1, LoopType.Yoyo);
                }

                _despawnTimer += Time.deltaTime;
                if (_despawnTimer >= _despawnTimerMax)
                {
                    _despawnTimer = 0.0f;
                    ReturnToPool();
                }
            }
            else
            {
                if (_blinkingToDespawn)
                {
                    //Debug.Log("b");
                    _blinkingToDespawn = false;
                    _blinkingToDespawnTween.Kill();
                    _ballRenderer.color = ResourceManager.Instance.GetColorByColorType(_colorType);
                }

                _despawnTimer = 0.0f;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (collision2D.transform.TryGetComponent<Ball>(out Ball ball))
            {
                if (_ballType == BallType.Spike || ball.GetBallType() == BallType.Spike)
                {
                    ReturnToPool();
                }
                else
                {
                    TryMoveToSides();
                }
            }
        }

        public void EnterSlot(Slot slot)
        {
            if (IsSlotMatching(slot))
            {
                if (_currentSlot != slot)
                {
                    if (_waitToStickCoroutine != null)
                    {
                        StopCoroutine(_waitToStickCoroutine);
                    }

                    _currentSlot = slot;
                    _waitToStickCoroutine = WaitToStickCoroutine();
                    StartCoroutine(_waitToStickCoroutine);
                }
            }
        }

        public void ExitSlot(Slot slot)
        {
            if (_currentSlot == slot)
            {
                if (_waitToStickCoroutine != null)
                {
                    StopCoroutine(_waitToStickCoroutine);
                }
                _currentSlot = null;
            }
        }

        public void PoolSetup(Action<Ball> onReturnAction)
        {
            _onReturnAction = onReturnAction;
        }

        public void ReturnToPool()
        {
            _onReturnAction?.Invoke(this);
        }

        public void ResetState()
        {
            DOTween.Kill(transform);
            DOTween.Kill(_ballRenderer);
            DOTween.Kill(_doneRenderer);

            _blinkingToDespawn = false;
            _despawnTimer = 0.0f;

            _rigidbody2D.constraints = RigidbodyConstraints2D.None;
            _rigidbody2D.velocity = Vector3.zero;
            _rigidbody2D.angularVelocity = 0.0f;
            _rigidbody2D.inertia = 0.0f;

            if (_currentSlot != null)
            {
                _currentSlot.SetIsFilled(false);
                _currentSlot = null;
            }

            _doneRenderer.DOFade(0.0f, 0.0f);
            _trailRenderer.Clear();
        }

        public void Push(Vector2 force)
        {
            _rigidbody2D.AddForce(force, ForceMode2D.Impulse);
        }

        public void StickToSlot()
        {
            if (_currentSlot == null)
            {
                return;
            }

            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

            float tweenDuration = 0.25f;
            transform.DOMove(_currentSlot.transform.position, tweenDuration).SetEase(Ease.OutCubic);
            transform.DORotateQuaternion(Quaternion.identity, tweenDuration).SetEase(Ease.OutCubic);
            _doneRenderer.DOColor(Color.white, tweenDuration).OnComplete(() =>
            {
                _currentSlot.SetIsFilled(true);
            });

            EffectPool.Instance.SpawnHitVFX(_currentSlot.transform.position, Quaternion.identity);
        }

        public void Teleport(Vector3 position)
        {
            transform.position = position;
            _trailRenderer.Clear();
        }

        public void UpdateVisual()
        {
            _ballRenderer.sprite = ResourceManager.Instance.GetBallSpriteByType(_ballType);

            Color color = Color.white;
            switch (_ballType)
            {
                case BallType.Normal:
                    color = ResourceManager.Instance.GetColorByColorType(_colorType);
                    break;
                case BallType.Rainbow:
                case BallType.Spike:
                    color = Color.white;
                    break;
            }

            _ballRenderer.color = color;
            _trailRenderer.startColor = color;
        }

        public BallType GetBallType()
        {
            return _ballType;
        }

        public void SetBallType(BallType ballType)
        {
            _ballType = ballType;

            UpdateVisual();
        }

        public ColorType GetColorType()
        {
            return _colorType;
        }

        public void SetColorType(ColorType colorType)
        {
            _colorType = colorType;

            UpdateVisual();
        }

        public bool IsSlotMatching(Slot slot)
        {
            return (_ballType == BallType.Normal && _colorType == slot.GetColorType())
                || (_ballType == BallType.Rainbow);
        }

        private bool TryMoveToSides()
        {
            float distance = 0.5f;
            Vector2 leftDir = Quaternion.Euler(0.0f, 0.0f, 30.0f) * Vector2.left;
            Vector2 rightDir = Quaternion.Euler(0.0f, 0.0f, -30.0f) * Vector2.right;
            if (!Physics2D.Raycast(transform.position, leftDir, distance, _obstacleLayerMask))
            {
                _rigidbody2D.AddForce(leftDir, ForceMode2D.Impulse);

                return true;
            }
            else if (!Physics2D.Raycast(transform.position, rightDir, distance, _obstacleLayerMask))
            {
                _rigidbody2D.AddForce(rightDir, ForceMode2D.Impulse);

                return true;
            }

            return false;
        }

        private IEnumerator WaitToStickCoroutine()
        {
            while (true)
            {
                if (_rigidbody2D.velocity.sqrMagnitude < GameConstants.BALL_MIN_SQR_MAGNITUDE)
                {
                    break;
                }

                yield return null;
            }

            StickToSlot();

            _waitToStickCoroutine = null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0.0f, 0.0f, 30.0f) * Vector2.left * 0.5f);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0.0f, 0.0f, -30.0f) * Vector2.right * 0.5f);
        }
    }
}
