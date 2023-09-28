using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class ScaleToFitScreen : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (_spriteRenderer != null)
            {
                float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
                float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

                transform.localScale = new Vector3(
                    worldScreenWidth / _spriteRenderer.sprite.bounds.size.x,
                    worldScreenHeight / _spriteRenderer.sprite.bounds.size.y,
                    1.0f);
            }
        }
    }
}
