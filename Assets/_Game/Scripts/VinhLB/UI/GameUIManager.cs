using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class GameUIManager : MonoSingleton<GameUIManager>
    {
        [SerializeField]
        private GameUIScreen[] _gameUIScreenArray;

        private Dictionary<string, GameUIScreen> _gameUIScreenDict;

        protected override void Awake()
        {
            base.Awake();

            _gameUIScreenDict = new Dictionary<string, GameUIScreen>();
            for (int i = 0; i <  _gameUIScreenArray.Length; i++)
            {
                _gameUIScreenDict[_gameUIScreenArray[i].GetType().Name] = _gameUIScreenArray[i];
            }
        }

        public T GetGameUIScreen<T>() where T : GameUIScreen
        {
            string key = typeof(T).Name;
            if (_gameUIScreenDict.ContainsKey(key) && _gameUIScreenDict[key] is T)
            {
                return _gameUIScreenDict[key] as T;
            }

            return null;
        }
    }
}
