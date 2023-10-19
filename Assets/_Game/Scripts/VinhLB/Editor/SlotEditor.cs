using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VinhLB
{
    [CustomEditor(typeof(Slot)), CanEditMultipleObjects]
    public class SlotEditor : Editor
    {
        private SerializedProperty _spriteRendererProp;
        private SerializedProperty _colorTypeProp;

        private void OnEnable()
        {
            _spriteRendererProp = serializedObject.FindProperty("_spriteRenderer");
            _colorTypeProp = serializedObject.FindProperty("_colorType");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Label("References", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_spriteRendererProp);

            GUILayout.Space(10.0f);
            GUILayout.Label("Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_colorTypeProp);

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
