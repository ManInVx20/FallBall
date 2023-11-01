using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VinhLB
{
    public class InitScreen : GameUIScreen
    {
        [SerializeField]
        private Button _playButton;

        public override void Initialize()
        {
            _playButton.onClick.AddListener(() =>
            {
                if (!DataPersistenceManager.Instance.HasGameData())
                {
                    DataPersistenceManager.Instance.NewGame();
                }

                DataPersistenceManager.Instance.SaveGame();

                GameSceneManager.Instance.LoadScene(GameSceneManager.GameScene.Main);
            });
        }
    }
}
