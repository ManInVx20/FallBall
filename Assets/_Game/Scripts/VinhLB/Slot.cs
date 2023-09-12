using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class Slot : MonoBehaviour, IHasColor
    {
        [Header("References")]
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [Header("Settings")]
        [SerializeField]
        private ColorType _colorType;

        private void Start()
        {
            _spriteRenderer.color = GameplayManager.Instance.GetColorByColorType(_colorType);
        }

        public ColorType GetColorType()
        {
            return _colorType;
        }

        public void SetColorType(ColorType colorType)
        {
            _colorType = colorType;

            _spriteRenderer.color = GameplayManager.Instance.GetColorByColorType(_colorType);
        }

        public bool IsColorTypeMatching(ColorType colorType)
        {
            return _colorType == colorType || _colorType == ColorType.Rainbow || colorType == ColorType.Rainbow;
        }
    }
}
