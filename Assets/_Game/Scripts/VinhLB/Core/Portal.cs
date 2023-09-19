using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class Portal : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField]
        private Portal _connectedPortal;
        [SerializeField]
        private Transform _goThroughTransform;
    }
}
