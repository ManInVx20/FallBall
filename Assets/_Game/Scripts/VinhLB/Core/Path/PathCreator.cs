using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class PathCreator : MonoBehaviour
    {
        [HideInInspector]
        public Path Path;

        public Color AnchorColor = Color.red;
        public Color ControlColor = Color.blue;
        public Color SegmentColor = Color.green;
        public Color SelectedSegmentColor = Color.yellow;
        public float AnchorDiameter = 0.1f;
        public float ControlDiameter = 0.075f;
        public bool DisplayControlPoints = true;

        private void Reset()
        {
            CreatePath();
        }

        public void CreatePath()
        {
            Path = new Path(transform.position);
        }
    }
}
