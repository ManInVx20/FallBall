using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    [Serializable]
    public class GameData
    {
        public int CurrentLevelIndex;
        public int CurrentStarAmount;

        public GameData()
        {
            CurrentLevelIndex = 0;
            CurrentStarAmount = 0;
        }
    }
}
