using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class Deadzone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.TryGetComponent<Ball>(out Ball ball))
            {
                ball.ReturnToPool();
            }
        }
    }
}
