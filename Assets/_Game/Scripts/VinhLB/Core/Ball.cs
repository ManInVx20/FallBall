using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class Ball : MonoBehaviour, IPoolable<Ball>, IHasColor
    {
        private const float MIN_SQR_MAGNITUDE = 0.01f;

        [Header("References")]
        [SerializeField]
        private Rigidbody2D _rigidbody2D;
        [SerializeField]
        private SpriteRenderer _ballRenderer;
        [SerializeField]
        private SpriteRenderer _doneRenderer;

        [Header("Settings")]
        [SerializeField]
        private ColorType _colorType;
        [SerializeField]
        private LayerMask _obstacleLayerMask;

        private Action<Ball> _onReturnAction;
        private Slot _currentSlot;
        private Coroutine _waitToStickCoroutine;

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (collision2D.transform.TryGetComponent<Ball>(out _))
            {
                TryMoveToSides();
            }
        }

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.TryGetComponent<Slot>(out Slot slot))
            {
                if (IsColorTypeMatching(slot.GetColorType()))
                {
                    _currentSlot = slot;
                    _waitToStickCoroutine = StartCoroutine(WaitToStickCoroutine());
                }
            }
            else if (collider2D.TryGetComponent<Deadzone>(out _))
            {
                ReturnToPool();
            }
        }

        private void OnTriggerExit2D(Collider2D collider2D)
        {
            if (collider2D.TryGetComponent<Slot>(out Slot slot))
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

        public ColorType GetColorType()
        {
            return _colorType;
        }

        public void SetColorType(ColorType colorType)
        {
            _colorType = colorType;

            _ballRenderer.color = LevelManager.GetColorByColorType(_colorType);
        }

        public bool IsColorTypeMatching(ColorType colorType)
        {
            return _colorType == colorType || _colorType == ColorType.Rainbow || colorType == ColorType.Rainbow;
        }

        private bool TryMoveToSides()
        {
            float distance = 0.4f;
            if (!Physics2D.Raycast(transform.position, Vector2.left, distance, _obstacleLayerMask))
            {
                //Debug.Log("a");
                _rigidbody2D.velocity += Vector2.left;

                return true;
            }
            else if (!Physics2D.Raycast(transform.position, Vector2.right, distance, _obstacleLayerMask))
            {
                //Debug.Log("b");
                _rigidbody2D.velocity += Vector2.right;

                return true;
            }

            return false;
        }

        private IEnumerator WaitToStickCoroutine()
        {
            while (_rigidbody2D.velocity.sqrMagnitude > MIN_SQR_MAGNITUDE)
            {
                yield return null;
            }

            StickToSlot();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(transform.position, Vector2.left * 0.4f);
            Gizmos.DrawRay(transform.position, Vector2.right * 0.4f);
        }
    }
}
