using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VinhLB
{
    [CustomEditor(typeof(DrawPolygon2D)), CanEditMultipleObjects]
    public class DrawPolygon2DEditor : Editor
    {
        private static Vector3 pointSnap = Vector3.one * 0.1f;

        private const float HANDLE_SIZE = 0.15f;

        private void OnSceneGUI()
        {
            DrawPolygon2D drawPolygon2D = target as DrawPolygon2D;
            Transform polygon2DTransform = drawPolygon2D.transform;
            GUIStyle handleLabelStyle = new GUIStyle();
            handleLabelStyle.alignment = TextAnchor.MiddleCenter;
            handleLabelStyle.normal.textColor = Color.green;

            if (drawPolygon2D.VerticeList != null)
            {
                for (int i = 0; i < drawPolygon2D.VerticeList.Count; i++)
                {
                    Vector3 oldPoint = polygon2DTransform.TransformPoint(drawPolygon2D.VerticeList[i]);
                    Vector3 newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, HANDLE_SIZE, pointSnap, Handles.RectangleHandleCap);
                    if (newPoint != oldPoint)
                    {
                        Undo.RecordObject(drawPolygon2D, "Move");
                        drawPolygon2D.VerticeList[i] = polygon2DTransform.InverseTransformPoint(newPoint);
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
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_meshUVType"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_rendererMaterial"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("VerticeList"));

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
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_edgeVerticeIndexRangeList"));

            GUILayout.Label("Edge Renderer");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_edgeWidth"));
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
