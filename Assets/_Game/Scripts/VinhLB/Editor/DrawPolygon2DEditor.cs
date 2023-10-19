using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static VinhLB.DrawPolygon2D;

namespace VinhLB
{
    [CustomEditor(typeof(DrawPolygon2D)), CanEditMultipleObjects]
    public class DrawPolygon2DEditor : Editor
    {
        private static Vector3 pointSnap = Vector3.one * 0.1f;

        private const float HANDLE_SIZE = 0.15f;

        private SerializedProperty _meshUVTypeProp;
        private SerializedProperty _rendererMaterialProp;
        private SerializedProperty _vertexListProp;
        private SerializedProperty _edgeWidthProp;
        private SerializedProperty _edgeOffsetProp;
        private SerializedProperty _loopProp;
        private SerializedProperty _edgeVertexIndexRangeListProp;

        private void OnEnable()
        {
            _meshUVTypeProp = serializedObject.FindProperty("_meshUVType");
            _rendererMaterialProp = serializedObject.FindProperty("_rendererMaterial");
            _vertexListProp = serializedObject.FindProperty("VertexList");
            _edgeWidthProp = serializedObject.FindProperty("_edgeWidth");
            _edgeOffsetProp = serializedObject.FindProperty("_edgeOffset");
            _loopProp = serializedObject.FindProperty("_loop");
            _edgeVertexIndexRangeListProp = serializedObject.FindProperty("_edgeVertexIndexRangeList");
        }

        private void OnSceneGUI()
        {
            DrawPolygon2D drawPolygon2D = target as DrawPolygon2D;
            Transform polygon2DTransform = drawPolygon2D.transform;
            GUIStyle handleLabelStyle = new GUIStyle();
            handleLabelStyle.alignment = TextAnchor.MiddleCenter;
            handleLabelStyle.normal.textColor = Color.green;

            if (drawPolygon2D.VertexList != null)
            {
                for (int i = 0; i < drawPolygon2D.VertexList.Count; i++)
                {
                    Vector3 oldPoint = polygon2DTransform.TransformPoint(drawPolygon2D.VertexList[i]);
                    Vector3 newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, HANDLE_SIZE, pointSnap, Handles.RectangleHandleCap);
                    if (newPoint != oldPoint)
                    {
                        Undo.RecordObject(drawPolygon2D, "Move");
                        drawPolygon2D.VertexList[i] = polygon2DTransform.InverseTransformPoint(newPoint);
                        drawPolygon2D.UpdateMesh();
                    }

                    Handles.Label(newPoint, i.ToString(), handleLabelStyle);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Label("Draw Polygon", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_meshUVTypeProp);
            EditorGUILayout.PropertyField(_rendererMaterialProp);
            EditorGUILayout.PropertyField(_vertexListProp);

            GUILayout.Label("Polygon Collider");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create"))
            {
                foreach (DrawPolygon2D drawPolygon2D in targets)
                {
                    if (!PrefabUtility.IsPartOfPrefabAsset(drawPolygon2D))
                    {
                        drawPolygon2D.CreatePolygonCollider();
                        EditorUtility.SetDirty(drawPolygon2D);
                    }
                }
            }
            if (GUILayout.Button("Clear"))
            {
                foreach (DrawPolygon2D drawPolygon2D in targets)
                {
                    if (!PrefabUtility.IsPartOfPrefabAsset(drawPolygon2D))
                    {
                        drawPolygon2D.ClearPolygonCollider();
                        EditorUtility.SetDirty(drawPolygon2D);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10.0f);

            GUILayout.Label("Draw Edge", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_edgeWidthProp);
            EditorGUILayout.PropertyField(_edgeOffsetProp);
            EditorGUILayout.PropertyField(_loopProp);
            if (!_loopProp.boolValue)
            {
                EditorGUILayout.PropertyField(_edgeVertexIndexRangeListProp);
            }

            GUILayout.Label("Edge Renderer");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create"))
            {
                foreach (DrawPolygon2D drawPolygon2D in targets)
                {
                    if (!PrefabUtility.IsPartOfPrefabAsset(drawPolygon2D))
                    {
                        drawPolygon2D.CreateEdgeRenderer();
                        EditorUtility.SetDirty(drawPolygon2D);
                    }
                }
            }
            if (GUILayout.Button("Clear"))
            {
                foreach (DrawPolygon2D drawPolygon2D in targets)
                {
                    if (!PrefabUtility.IsPartOfPrefabAsset(drawPolygon2D))
                    {
                        drawPolygon2D.ClearEdgeRenderer();
                        EditorUtility.SetDirty(drawPolygon2D);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Label("Edge Collider");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create"))
            {
                foreach (DrawPolygon2D drawPolygon2D in targets)
                {
                    if (!PrefabUtility.IsPartOfPrefabAsset(drawPolygon2D))
                    {
                        drawPolygon2D.CreateEdgeCollider();
                        EditorUtility.SetDirty(drawPolygon2D);
                    }
                }
            }
            if (GUILayout.Button("Clear"))
            {
                foreach (DrawPolygon2D drawPolygon2D in targets)
                {
                    if (!PrefabUtility.IsPartOfPrefabAsset(drawPolygon2D))
                    {
                        drawPolygon2D.ClearEdgeCollider();
                        EditorUtility.SetDirty(drawPolygon2D);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            if (serializedObject.ApplyModifiedProperties() ||
                (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed"))
            {
                foreach (DrawPolygon2D drawPolygon2D in targets)
                {
                    if (!PrefabUtility.IsPartOfPrefabAsset(drawPolygon2D))
                    {
                        drawPolygon2D.UpdateMesh();
                    }
                }
            }
        }
    }
}
