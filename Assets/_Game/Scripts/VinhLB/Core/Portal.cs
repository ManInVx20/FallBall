using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class Portal : MonoBehaviour
    {
        private const float MAX_DISTANCE = 0.2f;

        [Header("Preferences")]
        [SerializeField]
        private Portal _connectedPortal;

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
    }
}
