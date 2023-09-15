using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    T[] componentArray = FindObjectsOfType<T>();
                    if (componentArray.Length > 0)
                    {
                        instance = componentArray[0];
                    }
                    if (componentArray.Length > 1)
                    {
                        Debug.LogError($"There is more than one \"{typeof(T).Name}\" in the scene.");
                    }
                    if (instance == null)
                    {
                        GameObject go = new GameObject();
                        go.name = typeof(T).Name;
                        instance = go.AddComponent<T>();
                    }
                }

                return instance;
            }
        }
    }

    public class PersistentMonoSingleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
