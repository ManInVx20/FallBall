using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class GameUIManager : MonoSingleton<GameUIManager>
    {
        [SerializeField]
        private GameUIScreen _startingGameUIScreen;
        [SerializeField]
        private GameUIScreen[] _gameUIScreenArray;

        private GameUIScreen _currentGameUIScreen;
        private Stack<GameUIScreen> _historyGameUIScreenStack = new Stack<GameUIScreen>();

        private void Start()
        {
            for (int i = 0; i < _gameUIScreenArray.Length; i++)
            {
                _gameUIScreenArray[i].Initialize();

                _gameUIScreenArray[i].Close();
            }

            if (_startingGameUIScreen != null)
            {
                Open(_startingGameUIScreen);
            }
        }

        public T GetGameUIScreen<T>() where T : GameUIScreen
        {
            for (int i = 0; i != _gameUIScreenArray.Length; i++)
            {
                if (_gameUIScreenArray[i] is T)
                {
                    return _gameUIScreenArray[i] as T;
                }
            }

            return null;
        }

        public void Open<T>(bool rememeber = true) where T : GameUIScreen
        {
            for (int i = 0; i != _gameUIScreenArray.Length; i++)
            {
                if (_gameUIScreenArray[i] is T)
                {
                    Open(_gameUIScreenArray[i], rememeber);
                }
            }
        }

        public void Open(GameUIScreen gameUIScreen, bool remember = true)
        {
            if (_currentGameUIScreen != null)
            {
                if (remember)
                {
                    _historyGameUIScreenStack.Push(_currentGameUIScreen);
                }

                _currentGameUIScreen.Close();
            }

            gameUIScreen.Open();

            _currentGameUIScreen = gameUIScreen;
        }

        public void OpenLast()
        {
            if (_historyGameUIScreenStack.Count > 0)
            {
                Open(_historyGameUIScreenStack.Pop(), false);
            }
        }
    }
}
