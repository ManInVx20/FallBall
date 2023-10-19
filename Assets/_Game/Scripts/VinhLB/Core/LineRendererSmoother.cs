using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineRendererSmoother : MonoBehaviour
    {
        public LineRenderer LineRenderer;
        public Vector3[] InitialPointArray;
        public float SmoothingLength = 2.0f;
        public int SmoothingSections = 10;
    }
}
