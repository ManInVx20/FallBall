using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class LevelManager : MonoSingleton<LevelManager>, IDataPersistence
    {
        [SerializeField]
        private Level[] _levelArray;

        private int _currentLevelIndex;
        private Level _currentLevel;

        public int GetCurrentLevelIndex()
        {
            return _currentLevelIndex;
        }

        public Level GetCurrentLevel()
        {
            return _currentLevel;
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
            UnloadLevel();

            _currentLevel = Instantiate(_levelArray[_currentLevelIndex]);
            _currentLevel.OnLevelFinishedAction = (won) =>
            {
                if (won)
                {
                    GameUIManager.Instance.GetGameUIScreen<ResultScreen>().OpenWin();
                    GameBoosterManager.Instance.CurrentActiveBooster = GameBoosterManager.ActiveBooster.None;
                }
                else
                {
                    GameUIManager.Instance.GetGameUIScreen<ResultScreen>().OpenLose();
                }
            };
        }

        public void UnloadLevel()
        {
            CommandInvoker.Clear();

            BallPool.Instance.RetrieveAll();

            if (_currentLevel != null)
            {
                Destroy(_currentLevel.gameObject);
            }
        }

        public void RestartLevel()
        {
            CommandInvoker.UndoAllCommands();

            CommandInvoker.Clear();
        }

        public void LoadData(GameData data)
        {
            _currentLevelIndex = data.CurrentLevelIndex;
        }

        public void SaveData(GameData data)
        {
            data.CurrentLevelIndex = _currentLevelIndex;
        }
    }
}
