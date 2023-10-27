using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class ColorSwapRing : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [Header("Settings")]
        [SerializeField]
        private List<ColorType> _colorTypeList;
        [SerializeField]
        private float _changeColorRate = 1.0f;

        private int _currentColorTypeIndex;
        private float _changeColorTimer;

        private void Start()
        {
            UpdateColor(true);
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

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent<Ball>(out Ball ball))
            {
                if (ball.GetBallType() == BallType.Normal)
                {
                    ball.SetColorType(_colorTypeList[_currentColorTypeIndex]);
                }
            }
        }

        public void UpdateColor(bool instant = false)
        {
            if (_spriteRenderer != null && _colorTypeList.Count > 0)
            {
                Color color = ResourceManager.Instance.GetColorByColorType(_colorTypeList[_currentColorTypeIndex]);
                if (_spriteRenderer.color != color)
                {
                    _spriteRenderer.DOColor(color, instant ? 0.0f : 0.25f);
                }
            }
        }
    }
}
