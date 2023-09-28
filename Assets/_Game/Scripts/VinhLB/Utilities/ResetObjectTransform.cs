using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class ResetObjectTransform : MonoBehaviour
    {
        [SerializeField]
        private bool _position;
        [SerializeField]
        private bool _rotation;
        [SerializeField]
        private bool _scale;

        private void LateUpdate()
        {
            if (_position)
            {
                transform.position = Vector3.zero;
            }
            if (_rotation)
            {
                transform.rotation = Quaternion.identity;
            }
            if (_scale)
            {
                transform.localScale = Vector3.one;
            }
        }
    }
}
