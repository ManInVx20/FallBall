using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    [Serializable]
    public class GameData
    {
        public LevelData LevelData;
        public int CurrentStarAmount;

        public GameData()
        {
            CurrentStarAmount = 0;
        }
    }
}
