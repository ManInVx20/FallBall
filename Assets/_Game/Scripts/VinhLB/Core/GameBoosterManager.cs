using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class GameBoosterManager : MonoSingleton<GameBoosterManager>, IDataPersistence
    {
        public event Action OnActiveBoosterTypeChanged;
        public event Action OnBoosterAmountDictChanged;

        [SerializeField]
        private int _initialAmount = 0;

        private Dictionary<BoosterType, int> _boosterAmountDict;
        private BoosterType _currentActiveBoosterType;

        public BoosterType CurrentActiveBoosterType
        {
            get
            { 
                return _currentActiveBoosterType;
            }
            set
            {
                _currentActiveBoosterType = value;

                OnActiveBoosterTypeChanged?.Invoke();
            }
        }

        public int GetBoosterAmountByType(BoosterType type)
        {
            if (!_boosterAmountDict.ContainsKey(type))
            {
                Debug.LogError($"BoosterAmountDict does not have {type} booster type.");

                return -1;
            }
            
            return _boosterAmountDict[type];
        }

        public void ModifyBoosterAmountByType(BoosterType type, int value)
        {
            if (!_boosterAmountDict.ContainsKey(type))
            {
                Debug.LogError($"BoosterAmountDict does not have {type} booster type.");

                return;
            }

            _boosterAmountDict[type] += value;

            OnBoosterAmountDictChanged?.Invoke();
        }

        public void LoadData(GameData data)
        {
            _boosterAmountDict = data.BoosterAmountDict;

            if (_boosterAmountDict == null)
            {
                _boosterAmountDict = new Dictionary<BoosterType, int>();

                foreach (BoosterType type in Enum.GetValues(typeof(BoosterType)))
                {
                    if (type == BoosterType.None)
                    {
                        continue;
                    }

                    _boosterAmountDict[type] = _initialAmount;
                }
            }
        }

        public void SaveData(GameData data)
        {
            data.BoosterAmountDict = _boosterAmountDict;
        }
    }

    public enum BoosterType
    {
        None = 0,
        UndoMove = 1,
        AddMove = 2,
        AddRainbowBall = 3,
    }
}
