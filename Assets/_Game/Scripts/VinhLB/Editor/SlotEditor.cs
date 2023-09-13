using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VinhLB
{
    [CustomEditor(typeof(Slot)), CanEditMultipleObjects]
    public class SlotEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Label("References", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_spriteRenderer"));

            GUILayout.Space(10.0f);
            GUILayout.Label("Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_colorType"));

            if (serializedObject.ApplyModifiedProperties() ||
                (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed"))
            {
                foreach (Slot slot in targets)
                {
                    if (!PrefabUtility.IsPartOfPrefabAsset(slot))
                    {
                        slot.UpdateColor();
                    }
                }
            }
        }
    }
}
