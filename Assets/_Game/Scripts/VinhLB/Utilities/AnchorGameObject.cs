using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    [ExecuteInEditMode]
    public class AnchorGameObject : MonoBehaviour
    {
        public enum AnchorType
        {
            BottomLeft = 0,
            BottomCenter = 1,
            BottomRight = 2,
            MiddleLeft = 3,
            MiddleCenter = 4,
            MiddleRight = 5,
            TopLeft = 6,
            TopCenter = 7,
            TopRight = 8,
        }

        [SerializeField]
        private AnchorType _anchorType;
        [SerializeField]
        private Vector3 _anchorOffset;
        [SerializeField]
        private bool _executeInUpdate;

        private IEnumerator _updateAnchorCoroutine;

        private void Start()
        {
            _updateAnchorCoroutine = UpdateAnchorCoroutine();
            StartCoroutine(_updateAnchorCoroutine);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (_updateAnchorCoroutine == null && _executeInUpdate)
            {
                _updateAnchorCoroutine = UpdateAnchorCoroutine();
                StartCoroutine(_updateAnchorCoroutine);
            }
#endif
        }

        private IEnumerator UpdateAnchorCoroutine()
        {
            uint cameraWaitCycles = 0;
            while (CameraViewportHandler.Instance == null)
            {
                cameraWaitCycles += 1;
                yield return new WaitForEndOfFrame();
            }

            if (cameraWaitCycles > 0)
            {
                print(string.Format("CameraAnchor found CameraFit instance after waiting {0} frame(s). " +
                    "You might want to check that CameraFit has an earlier execution order.", cameraWaitCycles));
            }

            UpdateAnchor();
            _updateAnchorCoroutine = null;
        }

        private void UpdateAnchor()
        {
            switch (_anchorType)
            {
                case AnchorType.BottomLeft:
                    SetAnchor(CameraViewportHandler.Instance.BottomLeft);
                    break;
                case AnchorType.BottomCenter:
                    SetAnchor(CameraViewportHandler.Instance.BottomCenter);
                    break;
                case AnchorType.BottomRight:
                    SetAnchor(CameraViewportHandler.Instance.BottomRight);
                    break;
                case AnchorType.MiddleLeft:
                    SetAnchor(CameraViewportHandler.Instance.MidLeft);
                    break;
                case AnchorType.MiddleCenter:
                    SetAnchor(CameraViewportHandler.Instance.MidCenter);
                    break;
                case AnchorType.MiddleRight:
                    SetAnchor(CameraViewportHandler.Instance.MidRight);
                    break;
                case AnchorType.TopLeft:
                    SetAnchor(CameraViewportHandler.Instance.TopLeft);
                    break;
                case AnchorType.TopCenter:
                    SetAnchor(CameraViewportHandler.Instance.TopCenter);
                    break;
                case AnchorType.TopRight:
                    SetAnchor(CameraViewportHandler.Instance.TopRight);
                    break;

            }
        }

        private void SetAnchor(Vector3 anchor)
        {
            Vector3 newPosition = anchor + _anchorOffset;
            if (transform.position != newPosition)
            {
                transform.position = newPosition;
            }
        }
    }
}
