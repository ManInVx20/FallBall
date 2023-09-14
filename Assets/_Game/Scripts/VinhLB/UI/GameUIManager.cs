using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class GameUIManager : MonoSingleton<GameUIManager>
    {
        [SerializeField]
        private GameUIScreen[] _gameUIScreenArray;

        private GameUIScreen _currentGameUIScreen;
        private Stack<GameUIScreen> _historyGameUIScreenStack = new Stack<GameUIScreen>();

        public static T GetGameUIScreen<T>() where T : GameUIScreen
        {
            for (int i = 0; i  != Instance._gameUIScreenArray.Length; i++)
            {
                if (Instance._gameUIScreenArray[i] is T)
                {
                    return Instance._gameUIScreenArray[i] as T;
                }
            }

            return null;
        }

        public static void Open<T>(bool rememeber = true) where T : GameUIScreen
        {
            for (int i = 0; i != Instance._gameUIScreenArray.Length; i++)
            {
                if (Instance._gameUIScreenArray[i] is T)
                {
                    Open(Instance._gameUIScreenArray[i], rememeber);
                }
            }
        }

        public static void Open(GameUIScreen gameUIScreen, bool remember = true)
        {
            if (Instance._currentGameUIScreen != null)
            {
                if (remember)
                {
                    Instance._historyGameUIScreenStack.Push(Instance._currentGameUIScreen);
                }

                Instance._currentGameUIScreen.Close();
            }

            gameUIScreen.Open();

            Instance._currentGameUIScreen = gameUIScreen;
        }

        public static void OpenLast()
        {
            if (Instance._historyGameUIScreenStack.Count > 0)
            {
                Open(Instance._historyGameUIScreenStack.Pop(), false);
            }
        }
    }
}
