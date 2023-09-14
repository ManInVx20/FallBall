using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        [SerializeField]
        private Level[] _levelArray;

        private int _currentLevelIndex;
        private Level _currentLevel;

        private void Start()
        {
            _currentLevelIndex = 0;

            LoadLevel();
        }

        public int GetCurrentLevel()
        {
            return _currentLevelIndex + 1;
        }

        public bool IsLastLevel()
        {
            return _currentLevelIndex == _levelArray.Length - 1;
        }

        public bool TryLoadNextLevel()
        {
            if (_currentLevelIndex + 1 < _levelArray.Length)
            {
                _currentLevelIndex += 1;

                LoadLevel();

                return true;
            }

            return false;
        }

        public void LoadLevel()
        {
            CommandInvoker.Clear();

            BallPool.Instance.RetrieveAll();

            if (_currentLevel != null)
            {
                Destroy(_currentLevel.gameObject);
            }

            _currentLevel = Instantiate(_levelArray[_currentLevelIndex]);
            _currentLevel.OnLevelFinishedAction = (won) =>
            {
                if (won)
                {
                    GameUIManager.GetGameUIScreen<ResultScreen>().OpenWin();
                }
                else
                {
                    GameUIManager.GetGameUIScreen<ResultScreen>().OpenLose();
                }
            };
        }

        public void RestartLevel()
        {
            CommandInvoker.UndoAllCommands();
        }

        public static Color GetColorByColorType(ColorType colorType)
        {
            Color color = Color.white;
            switch (colorType)
            {
                case ColorType.None:
                    color = Color.white;
                    break;
                case ColorType.Red:
                    color = Color.red;
                    break;
                case ColorType.Green:
                    color = Color.green;
                    break;
                case ColorType.Blue:
                    color = Color.blue;
                    break;
                case ColorType.Yellow:
                    color = Color.yellow;
                    break;
                case ColorType.Cyan:
                    color = Color.cyan;
                    break;
                case ColorType.Magenta:
                    color = Color.magenta;
                    break;
            }

            return color;
        }
    }

    [System.Serializable]
    public enum ColorType
    {
        None = 0,
        Red = 1,
        Green = 2,
        Blue = 3,
        Yellow = 4,
        Cyan = 5,
        Magenta = 6,
        Rainbow = 7,
    }
}
