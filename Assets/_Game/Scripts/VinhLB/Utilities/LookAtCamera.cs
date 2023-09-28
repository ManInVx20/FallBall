using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class LookAtCamera : MonoBehaviour
    {
        private enum Mode
        {
            LookAt = 0,
            LookAtInverted = 1,
            CameraFoward = 2,
            CameraFowardInverted = 3,
        }

        [SerializeField]
        private Mode _mode;

        private Transform _cameraTransform;

        private void Start()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            switch (_mode)
            {
                case Mode.LookAt:
                    transform.LookAt(_cameraTransform);
                    break;
                case Mode.LookAtInverted:
                    Vector3 directionFromCamera = transform.position - _cameraTransform.position;
                    transform.LookAt(transform.position + directionFromCamera);
                    break;
                case Mode.CameraFoward:
                    transform.forward = _cameraTransform.forward;
                    break;
                case Mode.CameraFowardInverted:
                    transform.forward = -_cameraTransform.forward;
                    break;
            }
        }
    }
}
