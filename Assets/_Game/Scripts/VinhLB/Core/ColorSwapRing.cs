using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    [ExecuteInEditMode]
    public class ColorSwapRing : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private ColorType _colorType;

        private void OnEnable()
        {
            UpdateColor();
        }

        private void Reset()
        {
            UpdateColor();
        }

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.TryGetComponent<Ball>(out Ball ball))
            {
                ball.SetColorType(_colorType);
            }
        }

        public void UpdateColor()
        {
            if (_spriteRenderer != null)
            {
                _spriteRenderer.color = ResourceManager.Instance.GetColorByColorType(_colorType);
            }
        }
    }
}
