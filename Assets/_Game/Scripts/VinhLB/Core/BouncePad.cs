using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class BouncePad : MonoBehaviour
    {
        [SerializeField]
        private float _bounciness = 1.0f;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent<Ball>(out Ball ball))
            {
                ball.Push(Vector2.up * _bounciness);
            }
        }
    }
}
