using DG.Tweening;
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
        private List<ColorType> _colorTypeList;
        [SerializeField]
        private float _changeColorRate = 0.5f;

        private int _currentColorTypeIndex;
        private float _changeColorTimer;

        private void OnEnable()
        {
            UpdateColor();
        }

        private void Reset()
        {
            UpdateColor();
        }

        private void Update()
        {
            if (_colorTypeList.Count > 1)
            {
                _changeColorTimer += Time.deltaTime;
                if (_changeColorTimer >= 1.0f / _changeColorRate)
                {
                    _currentColorTypeIndex = (_currentColorTypeIndex + 1) % _colorTypeList.Count;

                    UpdateColor();

                    _changeColorTimer = 0.0f;
                }
            }
        }

        private void OnDestroy()
        {
            DOTween.Kill(_spriteRenderer);
        }

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.TryGetComponent<Ball>(out Ball ball))
            {
                ball.SetColorType(_colorTypeList[_currentColorTypeIndex]);
            }
        }

        public void UpdateColor()
        {
            if (_spriteRenderer != null && _colorTypeList.Count > 0)
            {
                Color color = ResourceManager.Instance.GetColorByColorType(_colorTypeList[_currentColorTypeIndex]);
                if (_spriteRenderer.color != color)
                {
                    _spriteRenderer.DOColor(color, 0.25f);
                }
            }
        }
    }
}
