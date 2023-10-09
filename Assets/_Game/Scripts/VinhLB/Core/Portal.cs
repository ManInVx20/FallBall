using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class Portal : MonoBehaviour
    {
        private const float MAX_DISTANCE = 0.25f;

        [Header("Preferences")]
        [SerializeField]
        private Portal _connectedPortal;
        [SerializeField]
        private SpriteRenderer _portalRenderer;

        [Header("Settings")]
        [SerializeField]
        [ColorUsage(true, true)]
        private Color _color;

        private int _colorPropID;

        private void Start()
        {
            _colorPropID = Shader.PropertyToID("_Color");

            UpdatePortalVisual();
        }

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.TryGetComponent<Ball>(out Ball ball))
            {
                if (Vector2.Distance(ball.transform.position, transform.position) > MAX_DISTANCE)
                {
                    ball.Teleport(_connectedPortal.transform.position);
                }
            }
        }

        private void UpdatePortalVisual()
        {
            //ParticleSystem.ColorOverLifetimeModule col = _portalVFX.colorOverLifetime;
            //GradientColorKey[] gradientColorKeys = col.color.gradient.colorKeys;
            //for (int i = 0; i < gradientColorKeys.Length - 1; i++)
            //{
            //    Color color = gradientColorKeys[i].color;
            //    color.r = Utilities.RepeatValue(color.r + _colorOffset, 0.0f, 1.0f);
            //    color.g = Utilities.RepeatValue(color.g + _colorOffset, 0.0f, 1.0f);
            //    color.b = Utilities.RepeatValue(color.b + _colorOffset, 0.0f, 1.0f);
            //    gradientColorKeys[i].color = color;
            //}
            //Gradient gradient = new Gradient();
            //gradient.SetKeys(gradientColorKeys, col.color.gradient.alphaKeys);
            //col.color = gradient;

            _portalRenderer.material.SetColor(_colorPropID, _color);
        }
    }
}
