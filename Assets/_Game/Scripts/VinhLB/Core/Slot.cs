using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class Slot : MonoBehaviour, IHasColor
    {
        public event System.Action<Slot> OnIsFilledChangedAction;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private ColorType _colorType;

        private bool _isFilled;

        private void OnEnable()
        {
            UpdateColor();
        }

        private void Reset()
        {
            UpdateColor();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent<Ball>(out Ball ball))
            {
                ball.EnterSlot(this);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.TryGetComponent<Ball>(out Ball ball))
            {
                ball.ExitSlot(this);
            }
        }

        public void UpdateColor()
        {
            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = ResourceManager.Instance.GetColorByColorType(_colorType);
            }
        }

        public bool IsFilled()
        {
            return _isFilled;
        }

        public void SetIsFilled(bool value)
        {
            _isFilled = value;

            OnIsFilledChangedAction?.Invoke(this);
        }

        public ColorType GetColorType()
        {
            return _colorType;
        }

        public void SetColorType(ColorType colorType)
        {
            _colorType = colorType;

            UpdateColor();
        }
    }
}
