using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class ObjectPool<T> : IObjectPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private Stack<T> _pooledStack = new Stack<T>();
        private GameObject _prefab;
        private System.Action<T> _onPullAction;
        private System.Action<T> _onPushAction;

        public int PooledCount => _pooledStack.Count;

        public ObjectPool(GameObject prefab, int spawnAmount = 0)
        {
            _prefab = prefab;

            Spawn(spawnAmount);
        }

        public ObjectPool(GameObject prefab, System.Action<T> onPullAction, System.Action<T> onPushAction)
        {
            _prefab = prefab;
            _onPullAction = onPullAction;
            _onPushAction = onPushAction;
        }

        public T Pull()
        {
            T t;
            if (_pooledStack.Count > 0)
            {
                t = _pooledStack.Pop();
            }
            else
            {
                t = GameObject.Instantiate(_prefab).GetComponent<T>();
            }

            t.gameObject.SetActive(true);
            t.PoolSetup(Push);

            _onPullAction?.Invoke(t);

            return t;
        }

        public void Push(T t)
        {
            _pooledStack.Push(t);

            _onPushAction?.Invoke(t);

            t.gameObject.SetActive(false);
        }

        private void Spawn(int spawnAmount)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                T t = GameObject.Instantiate(_prefab).GetComponent<T>();

                _pooledStack.Push(t);

                t.gameObject.SetActive(false);
            }
        }
    }
}
