using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VinhLB
{
    [CustomEditor(typeof(PathCreator))]
    public class PathCreatorEditor : Editor
    {
        private const float SEGMENT_SELECT_DISTANCE_THRESHOLD = 0.1f;

        private PathCreator _creator;

        private int _selectedSegmentIndex = -1;

        private void OnEnable()
        {
            _creator = (PathCreator)target;
            if (_creator.Path == null)
            {
                _creator.CreatePath();
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

            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();

            if (GUILayout.Button("Reset Path"))
            {
                Undo.RecordObject(_creator, "Reset path");
                _creator.CreatePath();
            }

            bool isClosed = EditorGUILayout.Toggle("Is Closed", _creator.Path.IsClosed);
            if (isClosed != _creator.Path.IsClosed)
            {
                Undo.RecordObject(_creator, "Toogle closed");
                _creator.Path.IsClosed = isClosed;
            }

            bool autoSetControlPoints = EditorGUILayout.Toggle("Auto Set Control Points", _creator.Path.AutoSetControlPoints);
            if (autoSetControlPoints != _creator.Path.AutoSetControlPoints)
            {
                Undo.RecordObject(_creator, "Toogle auto set control points");
                _creator.Path.AutoSetControlPoints = autoSetControlPoints;
            }

            if (EditorGUI.EndChangeCheck())
            {
                SceneView.RepaintAll();
            }
        }

        private void HandleInput()
        {
            Event guiEvent = Event.current;
            Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
            {
                if (_selectedSegmentIndex != -1)
                {
                    Undo.RecordObject(_creator, "Split segment");
                    _creator.Path.SplitSegment(mousePos, _selectedSegmentIndex);
                }
                else if (!_creator.Path.IsClosed)
                {
                    Undo.RecordObject(_creator, "Add segment");
                    _creator.Path.AddSegment(mousePos);
                }
            }

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
            {
                float minDistanceToAnchor = _creator.AnchorDiameter * 0.5f;
                int closestAnchorIndex = -1;

                for (int i = 0; i < _creator.Path.PointCount; i += 3)
                {
                    float distance = Vector2.Distance(mousePos, _creator.Path[i]);
                    if (distance < minDistanceToAnchor)
                    {
                        minDistanceToAnchor = distance;
                        closestAnchorIndex = i;
                    }
                }

                if (closestAnchorIndex != -1)
                {
                    Undo.RecordObject(_creator, "Remove segment");
                    _creator.Path.RemoveSegment(closestAnchorIndex);
                }
            }

            if (guiEvent.type == EventType.MouseMove)
            {
                float minDistanceToSegment = SEGMENT_SELECT_DISTANCE_THRESHOLD;
                int newSelectedSegmentIndex = -1;

                for (int i = 0; i < _creator.Path.SegmentCount; i++)
                {
                    Vector2[] pointArray = _creator.Path.GetPointsInSegment(i);
                    float distance = HandleUtility.DistancePointBezier(mousePos, pointArray[0], pointArray[3], pointArray[1], pointArray[2]);
                    if (distance < minDistanceToSegment)
                    {
                        minDistanceToSegment = distance;
                        newSelectedSegmentIndex = i;
                    }
                }

                if (newSelectedSegmentIndex != _selectedSegmentIndex)
                {
                    _selectedSegmentIndex = newSelectedSegmentIndex;
                    HandleUtility.Repaint();
                }
            }

            HandleUtility.AddDefaultControl(0);
        }

        private void Draw()
        {
            for (int i = 0; i < _creator.Path.SegmentCount; i++)
            {
                Vector2[] pointArray = _creator.Path.GetPointsInSegment(i);
                if (_creator.DisplayControlPoints)
                {
                    Handles.color = Color.black;
                    Handles.DrawLine(pointArray[1], pointArray[0]);
                    Handles.DrawLine(pointArray[2], pointArray[3]);
                }
                Color segmentColor = (i == _selectedSegmentIndex && Event.current.shift)
                    ? _creator.SelectedSegmentColor : _creator.SegmentColor;
                Handles.DrawBezier(pointArray[0], pointArray[3], pointArray[1], pointArray[2], segmentColor, null, 2.0f);
            }

            for (int i = 0; i < _creator.Path.PointCount; i++)
            {
                if (i % 3 == 0 || _creator.DisplayControlPoints)
                {
                    Handles.color = (i % 3 == 0) ? _creator.AnchorColor : _creator.ControlColor;
                    float handleSize = (i % 3 == 0) ? _creator.AnchorDiameter : _creator.ControlDiameter;
                    Vector2 newPos = Handles.FreeMoveHandle(_creator.Path[i], Quaternion.identity, handleSize, Vector2.zero, Handles.CylinderHandleCap);
                    if (_creator.Path[i] != newPos)
                    {
                        Undo.RecordObject(_creator, "Move point");
                        _creator.Path.MovePoint(i, newPos);
                    }
                }
            }
        }
    }
}
