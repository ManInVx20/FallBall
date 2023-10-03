using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class GameManager : PersistentMonoSingleton<GameManager>
    {
        protected override void Awake()
        {
            base.Awake();

            Initialize();
        }

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

        private void Initialize()
        {
            Application.targetFrameRate = 60;
        }
    }
}
