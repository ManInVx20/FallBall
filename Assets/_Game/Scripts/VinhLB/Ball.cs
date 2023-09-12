using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        private LayerMask _wallLayerMask;

        private Action<Ball> _onReturnAction;
        private Slot _slot;
        private Coroutine _waitToStickCoroutine;

        private void Start()
        {
            _ballRenderer.color = GameplayManager.Instance.GetColorByColorType(_colorType);
            _doneRenderer.color = Color.clear;
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (collision2D.transform.TryGetComponent<Ball>(out _))
            {
                PushToSides();
            }
        }

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.TryGetComponent<Slot>(out Slot slot))
            {
                if (IsColorTypeMatching(slot.GetColorType()))
                {
                    _slot = slot;
                    _waitToStickCoroutine = StartCoroutine(WaitToStickCoroutine());
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collider2D)
        {
            if (collider2D.TryGetComponent<Slot>(out Slot slot))
            {
                if (_slot == slot)
                {
                    if (_waitToStickCoroutine != null)
                    {
                        StopCoroutine(_waitToStickCoroutine);
                    }
                    _slot = null;
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

        public void StickToSlot()
        {
            if (_slot == null)
            {
                return;
            }

            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            transform.DOMove(_slot.transform.position, 0.2f).SetEase(Ease.OutCubic);
            transform.DORotateQuaternion(Quaternion.identity, 0.2f).SetEase(Ease.OutCubic);
            _doneRenderer.DOColor(Color.white, 0.2f);
        }

        public ColorType GetColorType()
        {
            return _colorType;
        }

        public void SetColorType(ColorType colorType)
        {
            _colorType = colorType;

            _ballRenderer.color = GameplayManager.Instance.GetColorByColorType(_colorType);
        }

        public bool IsColorTypeMatching(ColorType colorType)
        {
            return _colorType == colorType || _colorType == ColorType.Rainbow || colorType == ColorType.Rainbow;
        }

        private void PushToSides()
        {
            float distance = 0.3f, force = 10.0f;
            if (!Physics2D.Raycast(transform.position, Vector2.left, distance, _wallLayerMask))
            {
                _rigidbody2D.AddForce(Vector2.left * force, ForceMode2D.Force);
            }
            else if (!Physics2D.Raycast(transform.position, Vector2.right, distance, _wallLayerMask))
            {
                _rigidbody2D.AddForce(Vector2.right * force, ForceMode2D.Force);
            }
        }

        private IEnumerator WaitToStickCoroutine()
        {
            while (_rigidbody2D.velocity.sqrMagnitude > MIN_SQR_MAGNITUDE)
            {
                yield return null;
            }

            StickToSlot();
        }
    }
}
