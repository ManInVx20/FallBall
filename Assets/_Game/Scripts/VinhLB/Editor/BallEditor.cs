using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VinhLB
{
    [CustomEditor(typeof(Ball)), CanEditMultipleObjects]
    public class BallEditor : Editor
    {
        private SerializedProperty _rigidbody2DProp;
        private SerializedProperty _ballRendererProp;
        private SerializedProperty _doneRendererProp;
        private SerializedProperty _trailRendererProp;
        private SerializedProperty _ballTypeProp;
        private SerializedProperty _colorTypeProp;
        private SerializedProperty _obstacleLayerMaskProp;

        private void OnEnable()
        {
            _rigidbody2DProp = serializedObject.FindProperty("_rigidbody2D");
            _ballRendererProp = serializedObject.FindProperty("_ballRenderer");
            _doneRendererProp = serializedObject.FindProperty("_doneRenderer");
            _trailRendererProp = serializedObject.FindProperty("_trailRenderer");
            _ballTypeProp = serializedObject.FindProperty("_ballType");
            _colorTypeProp = serializedObject.FindProperty("_colorType");
            _obstacleLayerMaskProp = serializedObject.FindProperty("_obstacleLayerMask");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Label("References", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_rigidbody2DProp);
            EditorGUILayout.PropertyField(_ballRendererProp);
            EditorGUILayout.PropertyField(_doneRendererProp);
            EditorGUILayout.PropertyField(_trailRendererProp);

            GUILayout.Space(10.0f);

            GUILayout.Label("Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_ballTypeProp);
            BallType ballType = (BallType)_ballTypeProp.enumValueIndex;
            switch (ballType)
            {
                case BallType.Normal:
                    EditorGUILayout.PropertyField(_colorTypeProp);
                    break;
                case BallType.Rainbow:
                case BallType.Spike:
                    break;
            }
            EditorGUILayout.PropertyField(_obstacleLayerMaskProp);

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