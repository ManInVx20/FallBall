using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class Portal : MonoBehaviour
    {
        private const float MIN_DISTANCE = 0.25f;

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

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent<Ball>(out Ball ball))
            {
                if (Vector2.Distance(ball.transform.position, transform.position) > MIN_DISTANCE)
                {
                    ball.Teleport(_connectedPortal.transform.position);
                }
            }
        }

        private void UpdatePortalVisual()
        {
            _portalRenderer.material.SetColor(_colorPropID, _color);
        }
    }
}
