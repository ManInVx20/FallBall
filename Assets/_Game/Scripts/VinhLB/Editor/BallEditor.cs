using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VinhLB
{
    [CustomEditor(typeof(Ball)), CanEditMultipleObjects]
    public class BallEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Label("References", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_rigidbody2D"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_ballRenderer"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_doneRenderer"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_trailRenderer"));

            GUILayout.Space(10.0f);

            GUILayout.Label("Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_ballType"));
            BallType ballType = (BallType)serializedObject.FindProperty("_ballType").enumValueIndex;
            switch (ballType)
            {
                case BallType.Normal:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_colorType"));
                    break;
                case BallType.Rainbow:
                case BallType.Spike:
                    break;
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_obstacleLayerMask"));

            if (serializedObject.ApplyModifiedProperties() ||
                (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed"))
            {
                foreach (Ball ball in targets)
                {
                    if (!PrefabUtility.IsPartOfPrefabAsset(ball))
                    {
                        ball.UpdateVisual();
                    }
                }
            }
        }
    }
}