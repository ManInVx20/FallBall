using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class ObjectPool<T> : IObjectPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private List<T> _createdList = new List<T>();
        private Stack<T> _pooledStack = new Stack<T>();
        private GameObject _prefab;
        private Action<T> _onPullAction;
        private Action<T> _onPushAction;

        public int PooledCount => _pooledStack.Count;

        public ObjectPool(GameObject prefab, int spawnAmount = 0)
        {
            _prefab = prefab;

            Spawn(spawnAmount);
        }

        public ObjectPool(GameObject prefab, Action<T> onPullAction, Action<T> onPushAction)
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

                _createdList.Add(t);
            }

            t.gameObject.SetActive(true);
            t.PoolSetup(Push);

            _onPullAction?.Invoke(t);

            return t;
        }

        public void Push(T t)
        {
            if (_pooledStack.Contains(t))
            {
                return;
            }

            _pooledStack.Push(t);

            _onPushAction?.Invoke(t);

            t.gameObject.SetActive(false);
        }

        public void RetrieveAll()
        {
            for (int i = 0; i < _createdList.Count; i++)
            {
                if (!_pooledStack.Contains(_createdList[i]))
                {
                    _createdList[i].ReturnToPool();
                }
            }
        }

        public List<T> GetAll()
        {
            return _createdList;
        }

        public void Clear()
        {
            _createdList.Clear();
            _pooledStack.Clear();
        }

        private void Spawn(int spawnAmount)
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                T t = GameObject.Instantiate(_prefab).GetComponent<T>();

                _createdList.Add(t);

                _pooledStack.Push(t);

                t.gameObject.SetActive(false);
            }
        }
    }
}
