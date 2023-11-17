using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using NUnit.Framework;

namespace VinhLB
{
    [CustomEditor(typeof(MovingObject))]
    public class MovingObjectEditor : Editor
    {
        private static Vector3 pointSnap = Vector3.one * 0.1f;

        private const float HANDLE_SIZE = 0.125f;

        private MovingObject _obj;

        private void OnEnable()
        {
            _obj = (MovingObject)target;

            _obj.UpdateComponents();

            if (_obj.PointList == null)
            {
                _obj.CreatePath();
            }

            if (_obj.ShowPath)
            {
                _obj.DrawPath();
            }
        }

        private void OnSceneGUI()
        {
            HandleInput();
            Draw();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            if (_obj.ShowPath)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_lineMaterial"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_lineWidth"));
                _obj.DrawPath();
            }
            else
            {
                _obj.ClearPath();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void HandleInput()
        {
            Event guiEvent = Event.current;
            Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
            {
                Undo.RecordObject(_obj, "Add point");
                _obj.PointList.Add(mousePos);
                if (_obj.ShowPath)
                {
                    _obj.DrawPath();
                }
            }

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
            {
                for (int i = 0; i < _obj.PointList.Count; i++)
                {
                    if (Vector2.Distance(mousePos, _obj.PointList[i]) < HANDLE_SIZE)
                    {
                        Undo.RecordObject(_obj, "Remove point");
                        _obj.PointList.RemoveAt(i);
                        if (_obj.ShowPath)
                        {
                            _obj.DrawPath();
                        }
                    }
                }
            }

            HandleUtility.AddDefaultControl(0);
        }

        private void Draw()
        {
            GUIStyle handleLabelStyle = new GUIStyle();
            handleLabelStyle.alignment = TextAnchor.MiddleCenter;
            handleLabelStyle.normal.textColor = Color.white;
            handleLabelStyle.fontSize = 16;

            for (int i = 0; i < _obj.PointList.Count; i++)
            {
                Handles.color = Color.green;
                if (i != _obj.PointList.Count - 1)
                {
                    Handles.DrawLine(_obj.PointList[i], _obj.PointList[i + 1]);
                }
                else if (_obj.PointList.Count > 2 && _obj.IsLoopPath)
                {
                    Handles.DrawLine(_obj.PointList[i], _obj.PointList[0]);
                }

                Handles.color = Color.red;
                Vector2 newPos = Handles.FreeMoveHandle(_obj.PointList[i], Quaternion.identity, HANDLE_SIZE, pointSnap, Handles.RectangleHandleCap);
                if (_obj.PointList[i] != newPos)
                {
                    Undo.RecordObject(_obj, "Move point");
                    _obj.PointList[i] = newPos;
                    if (_obj.ShowPath)
                    {
                        _obj.DrawPath();
                    }
                }

                Handles.Label(newPos, i.ToString(), handleLabelStyle);
            }
        }
    }
}
