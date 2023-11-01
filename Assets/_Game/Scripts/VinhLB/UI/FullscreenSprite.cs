using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class FullscreenSprite : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private float _multiplier = 1.0f;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;

            AdjustSprite();
        }

        private void AdjustSprite()
        {
            float cameraHeight = _mainCamera.orthographicSize * 2;
            Vector2 cameraSize = new Vector2(_mainCamera.aspect * cameraHeight, cameraHeight);
            Vector2 spriteSize = _spriteRenderer.sprite.bounds.size;

            Vector2 scale = transform.localScale;
            if (cameraSize.x >= cameraSize.y)
            { // Landscape (or equal)
                scale *= cameraSize.x / spriteSize.x;
            }
            else
            { // Portrait
                scale *= cameraSize.y / spriteSize.y;
            }
            scale *= _multiplier;

            transform.position = Vector2.zero; // Optional
            transform.localScale = scale;
        }
    }
}
