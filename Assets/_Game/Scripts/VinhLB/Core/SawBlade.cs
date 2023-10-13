using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class SawBlade : MonoBehaviour
    {
        [SerializeField]
        private float _rotationSpeed = 36.0f;

        private void Update()
        {
            transform.Rotate(-Vector3.forward * _rotationSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (collision2D.collider.TryGetComponent<Ball>(out Ball ball))
            {
                ball.SelfDestroy();
            }
        }
    }
}
