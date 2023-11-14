using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    [Serializable]
    public class GameData
    {
        public int CurrentStarAmount;
        public LevelData LevelData;
        public Dictionary<BoosterType, int> BoosterAmountDict;

        public GameData()
        {
            CurrentStarAmount = 0;
        }
    }
}
