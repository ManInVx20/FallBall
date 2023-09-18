using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VinhLB
{
    public class GameplayScreen : GameUIScreen
    {
        [SerializeField]
        private Button _restartButton;
        [SerializeField]
        private Button _homeButton;
        [SerializeField]
        private Button _undoButton;
        [SerializeField]
        private Button _rainbowButton;
        [SerializeField]
        private CannonListPanel _cannonListPanel;

        public override void Initialize()
        {
            _restartButton.onClick.AddListener(() =>
            {
                LevelManager.Instance.RestartLevel();
            });
            _homeButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ReturnHome();
            });
            _undoButton.onClick.AddListener(() =>
            {
                CommandInvoker.UndoCommand();
            });
            _rainbowButton.onClick.AddListener(() =>
            {
                if (!_cannonListPanel.gameObject.activeSelf)
                {
                    _cannonListPanel.Open();
                }
                else
                {
                    _cannonListPanel.Close();
                }
            });
        }

        public override void Close()
        {
            base.Close();

            _cannonListPanel.Close();
        }
    }
}
