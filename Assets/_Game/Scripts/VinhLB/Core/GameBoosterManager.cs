using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class GameBoosterManager : MonoSingleton<GameBoosterManager>
    {
        public enum ActiveBooster
        {
            None = 0,
            NormalBall = 1,
            RainbowBall = 2,
            SpikeBall = 3
        }

        public event Action OnActiveBoosterChanged;

        private ActiveBooster _currentActiveBooster;

        public ActiveBooster CurrentActiveBooster
        {
            get
            { 
                return _currentActiveBooster;
            }
            set
            {
                _currentActiveBooster = value;

                OnActiveBoosterChanged?.Invoke();
            }
        }
    }
}
