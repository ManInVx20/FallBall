using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public void PlayGame()
        {
            LevelManager.Instance.LoadLevel();

            GameUIManager.Instance.Open<GameplayScreen>();
        }

        public void ReturnHome()
        {
            LevelManager.Instance.UnloadLevel();

            GameUIManager.Instance.Open<HomeScreen>();
        }
    }
}
