using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VinhLB
{
    public class GameplayScreen : GameUIScreen
    {
        [SerializeField]
        private TMP_Text _levelText;
        [SerializeField]
        private Button _restartButton;
        [SerializeField]
        private Button _homeButton;
        [SerializeField]
        private Button _undoButton;
        [SerializeField]
        private Button _redoButton;
        [SerializeField]
        private Button _normalButton;
        [SerializeField]
        private Button _rainbowButton;
        [SerializeField]
        private Button _spikeButton;
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
            _redoButton.onClick.AddListener(() =>
            {
                CommandInvoker.RedoCommand();
            });
            _normalButton.onClick.AddListener(() => 
            {
                if (!_cannonListPanel.gameObject.activeSelf)
                {
                    _cannonListPanel.Open(BallType.Normal);
                }
                else
                {
                    _cannonListPanel.Close();
                }
            });
            _rainbowButton.onClick.AddListener(() =>
            {
                if (!_cannonListPanel.gameObject.activeSelf)
                {
                    _cannonListPanel.Open(BallType.Rainbow);
                }
                else
                {
                    _cannonListPanel.Close();
                }
            });
            _spikeButton.onClick.AddListener(() =>
            {
                if (!_cannonListPanel.gameObject.activeSelf)
                {
                    _cannonListPanel.Open(BallType.Spike);
                }
                else
                {
                    _cannonListPanel.Close();
                }
            });
        }

        public override void Open()
        {
            base.Open();

            UpdateLevelText();
        }

        public override void Close()
        {
            base.Close();

            _cannonListPanel.Close();
        }

        public void UpdateLevelText()
        {
            _levelText.text = $"LEVEL {LevelManager.Instance.GetCurrentLevelIndex() + 1}";
        }
    }
}
