using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VinhLB
{
    [CustomEditor(typeof(ColorSwapRing)), CanEditMultipleObjects]
    public class ColorSwapRingEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Label("References", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_spriteRenderer"));

            GUILayout.Space(10.0f);
            GUILayout.Label("Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_colorTypeList"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_changeColorRate"));

            if (serializedObject.ApplyModifiedProperties() ||
                (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed"))
            {
                foreach (ColorSwapRing colorSwapRing in targets)
                {
                    if (!PrefabUtility.IsPartOfPrefabAsset(colorSwapRing))
                    {
                        colorSwapRing.UpdateColor();
                    }
                }
            }
        }
    }
}
