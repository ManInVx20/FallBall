using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VinhLB
{
    public class HomeScreen : GameUIScreen
    {
        [SerializeField]
        private Button _playButton;

        public override void Initialize()
        {
            _playButton.onClick.AddListener(() =>
            {
                if (!DataPersistenceManager.Instance.IsGameDataExist())
                {
                    DataPersistenceManager.Instance.NewGame();
                }

                GameManager.Instance.PlayGame();
            });
        }
    }
}
