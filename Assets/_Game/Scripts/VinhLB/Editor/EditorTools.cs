using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VinhLB
{
    public static class EditorTools
    {
        [MenuItem("Tools/VinhLB/Clear Game Data")]
        private static void ClearGameData()
        {
            FileDataHandler.Delete(GameConstants.DATA_FILE_NAME);
        }

        [MenuItem("Tools/VinhLB/Scenes/Production")]
        private static void OpenProductionScenes()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/_Game/Scenes/Init.unity", OpenSceneMode.Single);
                Scene mainScene = EditorSceneManager.OpenScene("Assets/_Game/Scenes/Main.unity", OpenSceneMode.Additive);
                EditorSceneManager.CloseScene(mainScene, false);
            }
        }

        [MenuItem("Tools/VinhLB/Scenes/Testing")]
        private static void OpenTestingScenes()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/_Game/Test/VinhLB/Test.unity", OpenSceneMode.Single);
            }
        }
    }
}
