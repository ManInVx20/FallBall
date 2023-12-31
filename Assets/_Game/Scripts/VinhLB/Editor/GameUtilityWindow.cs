using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace VinhLB
{
    public class GameUtilityWindow : EditorWindow
    {
        [MenuItem("Tools/VinhLB/Game Utility Panel")]
        public static void ShowWindow()
        {
            GetWindow<GameUtilityWindow>(true, "Game Utility Panel");
        }

        private void OnGUI()
        {
            GUILayout.Label("Game Designer", EditorStyles.boldLabel);
            //_tubePrefab = (Material)EditorGUILayout.ObjectField("Tube Material", _tubeMaterial, typeof(Material), true);
            if (GUILayout.Button("Create Tube"))
            {
                GameObject gameObject = new GameObject();
                gameObject.name = "Tube";
                gameObject.AddComponent<DrawPolygon2D>();
                gameObject.GetComponent<Renderer>().material =
                    AssetDatabase.LoadAssetAtPath<Material>("Assets/_Game/Materials/Tube.mat");
                SortingGroup sortingGroup = gameObject.AddComponent<SortingGroup>();
                sortingGroup.sortingLayerName = "Object";
                sortingGroup.sortingOrder = 10;
            }

            GUILayout.Space(10.0f);
            GUILayout.Label("Gameplay Features", EditorStyles.boldLabel);
            GUILayout.Label("Move");
            if (GUILayout.Button("Undo"))
            {
                CommandInvoker.UndoCommand();
            }
            if (GUILayout.Button("Redo"))
            {
                CommandInvoker.RedoCommand();
            }
            GUILayout.Label("Ball");
            if (GUILayout.Button("Normal"))
            {
                //foreach (GameObject gameObject in Selection.gameObjects)
                //{
                //    if (gameObject.TryGetComponent<Cannon>(out Cannon cannon))
                //    {
                //        cannon.AddBall(BallType.Normal);
                //    }
                //}
                GameUIManager.Instance.GetGameUIScreen<GameplayScreen>().OnNormalButtonClicked();
            }
            if (GUILayout.Button("Rainbow"))
            {
                //foreach (GameObject gameObject in Selection.gameObjects)
                //{
                //    if (gameObject.TryGetComponent<Cannon>(out Cannon cannon))
                //    {
                //        cannon.AddBall(BallType.Rainbow);
                //    }
                //}
                GameUIManager.Instance.GetGameUIScreen<GameplayScreen>().OnRainbowButtonClicked();
            }
            if (GUILayout.Button("Spike"))
            {
                //foreach (GameObject gameObject in Selection.gameObjects)
                //{
                //    if (gameObject.TryGetComponent<Cannon>(out Cannon cannon))
                //    {
                //        cannon.AddBall(BallType.Spike);
                //    }
                //}
                GameUIManager.Instance.GetGameUIScreen<GameplayScreen>().OnSpikeButtonClicked();
            }

            GUILayout.Space(10.0f);
            GUILayout.Label("Level Features", EditorStyles.boldLabel);
            if (GUILayout.Button("Restart"))
            {
                LevelManager.Instance.RestartLevel();
            }
            //if (GUILayout.Button("Next"))
            //{
            //    LevelManager.Instance.TryLoadNextLevel();
            //}
            if (GUILayout.Button("Win 1 Stars"))
            {
                LevelManager.Instance.CurrentLevel.Win(true, 1);
            }
            if (GUILayout.Button("Win 2 Stars"))
            {
                LevelManager.Instance.CurrentLevel.Win(true, 2);
            }
            if (GUILayout.Button("Win 3 Stars"))
            {
                LevelManager.Instance.CurrentLevel.Win(true, 3);
            }
        }
    }
}
