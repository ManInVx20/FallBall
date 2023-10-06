using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class LevelManager : MonoSingleton<LevelManager>, IDataPersistence
    {
        public LevelData LevelData => _levelData;
        public Level CurrentLevel => _currentLevel;
        public int CurrentLevelIndex => _currentLevelIndex;

        [SerializeField]
        private LevelInfo[] _levelInfoArray;
        [SerializeField]
        private bool _unlockAll = false;

        private LevelData _levelData;
        private int _currentLevelIndex;
        private Level _currentLevel;

        public bool IsLastLevel()
        {
            return _currentLevelIndex == _levelInfoArray.Length - 1;
        }

        public bool TryLoadLevel(int levelIndex)
        {
            if (levelIndex >= 0 && levelIndex < _levelInfoArray.Length)
            {
                _currentLevelIndex = levelIndex;

                LoadLevel();

                return true;
            }

            return false;
        }

        public void LoadLevel()
        {
            UnloadLevel();

            _currentLevel = Instantiate(_levelInfoArray[_currentLevelIndex].Prefab);
            _currentLevel.Initialize(_levelInfoArray[_currentLevelIndex]);
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

        public void CompleteLevel(int starAchieved)
        {
            _levelData.LevelItemArray[_currentLevelIndex].StarAchieved = starAchieved;

            if (_levelData.LastUnlockedLevelIndex < _currentLevelIndex)
            {
                _levelData.LastUnlockedLevelIndex = _currentLevelIndex + 1;
                _levelData.LevelItemArray[_levelData.LastUnlockedLevelIndex].Unlocked = true;
            }
        }

        public void LoadData(GameData data)
        {
            _levelData = data.LevelData;

            if (_levelData == null)
            {
                _levelData = new LevelData(_levelInfoArray.Length, _unlockAll);
            }
        }

        public void SaveData(GameData data)
        {
            data.LevelData = _levelData;
        }
    }

    [Serializable]
    public class LevelData
    {
        public int LastUnlockedLevelIndex;
        public LevelItem[] LevelItemArray;

        public LevelData()
        {
            LastUnlockedLevelIndex = 0;
        }

        public LevelData(int levelAmount, bool unlockAll) : this()
        {
            if (levelAmount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(levelAmount), "Level amount must be greater than 0");
            }

            LevelItemArray = new LevelItem[levelAmount];
            LevelItemArray[0].Unlocked = true;

            if (unlockAll)
            {
                for (int i = 1; i < LevelItemArray.Length; i++)
                {
                    LevelItemArray[i].Unlocked = true;
                }
            }
        }

        public int GetAllStarAchievedAmount()
        {
            int allStarsAchievedAmount = 0;
            if (LevelItemArray == null)
            {
                return allStarsAchievedAmount;
            }
            else
            {
                for (int i = 0; i < LevelItemArray.Length; i++)
                {
                    if (!LevelItemArray[i].Unlocked)
                    {
                        break;
                    }
                    else
                    {
                        allStarsAchievedAmount += LevelItemArray[i].StarAchieved;
                    }
                }
            }

            return allStarsAchievedAmount;
        }
    }

    [Serializable]
    public struct LevelItem
    {
        public bool Unlocked;
        public int StarAchieved;
    }

    [Serializable]
    public struct LevelInfo
    {
        public Level Prefab;
        public int MaxMoves3Stars;
        public int MaxMoves2Stars;
    }
}
