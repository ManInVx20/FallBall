using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VinhLB
{
    [CustomEditor(typeof(MovingPartPolygon2D))]
    public class MovingPartPolygon2DEditor : Editor
    {
        private MovingPartPolygon2D _movingPartPolygon2D;

        private void OnEnable()
        {
            _movingPartPolygon2D = (MovingPartPolygon2D)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();

            if (GUILayout.Button("Calculate Center Point"))
            {
                _movingPartPolygon2D.CalculateCenterPoint();
                EditorUtility.SetDirty(_movingPartPolygon2D);
            }

            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
        }
    }
}
