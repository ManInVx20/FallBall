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
        private const float MIN_SQR_MAGNITUDE = 0.0015f;

        [SerializeField]
        private Rigidbody2D _rigidbody2D;
        [SerializeField]
        private SpriteRenderer _ballRenderer;
        [SerializeField]
        private SpriteRenderer _doneRenderer;

        [SerializeField]
        private BallType _ballType;
        [SerializeField]
        private ColorType _colorType;
        [SerializeField]
        private LayerMask _obstacleLayerMask;

        private Action<Ball> _onReturnAction;
        private Slot _currentSlot;
        private Coroutine _waitToStickCoroutine;

        private void OnEnable()
        {
            UpdateVisual();
        }

        private void Reset()
        {
            UpdateVisual();
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (collision2D.transform.TryGetComponent<Ball>(out _))
            {
                TryMoveToSides();
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
                    _waitToStickCoroutine = StartCoroutine(WaitToStickCoroutine());
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
            DOTween.Kill(_doneRenderer);

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
        }

        public void UpdateVisual()
        {
            _ballRenderer.sprite = ResourceManager.Instance.GetBallSpriteByType(_ballType);

            switch (_ballType)
            {
                case BallType.Normal:
                    _ballRenderer.color = ResourceManager.Instance.GetColorByColorType(_colorType);
                    break;
                case BallType.Rainbow:
                    _ballRenderer.color = Color.white;
                    break;
            }
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
            return _ballType == BallType.Rainbow || _colorType == slot.GetColorType();
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
                //Debug.Log(_rigidbody2D.velocity.sqrMagnitude);
                if (_rigidbody2D.velocity.sqrMagnitude < MIN_SQR_MAGNITUDE)
                {
                    break;
                }

                yield return null;
            }

            StickToSlot();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0.0f, 0.0f, 30.0f) * Vector2.left * 0.5f);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0.0f, 0.0f, -30.0f) * Vector2.right * 0.5f);
        }
    }
}
