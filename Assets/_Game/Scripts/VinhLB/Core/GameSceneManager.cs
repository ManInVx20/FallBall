using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManagement = UnityEngine.SceneManagement;

namespace VinhLB
{
    public class GameSceneManager : PersistentMonoSingleton<GameSceneManager>
    {
        public enum Scene
        {
            Init = 0,
            Game = 1,
        }

        public event Action OnSceneLoadStarted;
        public event Action<float> OnSceneLoadingProgressChange;
        public event Action OnSceneLoadCompleted;

        public string GetActiveSceneName()
        {
            return UnitySceneManagement.SceneManager.GetActiveScene().name;
        }

        public bool IsSceneActive(Scene scene)
        {
            return UnitySceneManagement.SceneManager.GetActiveScene().name.Equals(scene.ToString());
        }

        public void LoadScene(Scene scene)
        {
            StartCoroutine(HandleLoadingSceneCoroutine(UnitySceneManagement.SceneManager.LoadSceneAsync(scene.ToString())));
        }

        private IEnumerator HandleLoadingSceneCoroutine(AsyncOperation asyncOperation)
        {
            if (asyncOperation == null)
            {
                yield break;
            }

            OnSceneLoadStarted?.Invoke();

            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                OnSceneLoadingProgressChange?.Invoke(asyncOperation.progress);

                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }

            OnSceneLoadCompleted?.Invoke();
        }
    }

}
