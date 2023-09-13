using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    [ExecuteInEditMode]
    public class Slot : MonoBehaviour, IHasColor
    {
        public event System.Action OnIsFilledChangedAction;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private ColorType _colorType;

        private bool _isFilled;

        private void OnEnable()
        {
            UpdateColor();
        }

        private void Start()
        {
            UpdateColor();
        }

        private void Reset()
        {
            UpdateColor();
        }

        public void UpdateColor()
        {
            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = LevelManager.GetColorByColorType(_colorType);
            }
        }

        public bool IsFilled()
        {
            return _isFilled;
        }

        public void SetIsFilled(bool value)
        {
            _isFilled = value;

            OnIsFilledChangedAction?.Invoke();
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

        public bool IsColorTypeMatching(ColorType colorType)
        {
            return _colorType == colorType || _colorType == ColorType.Rainbow || colorType == ColorType.Rainbow;
        }
    }
}
