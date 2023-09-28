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
        private TMP_Text _movesText;
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
        private UnmaskPanel _unmaskPanel;

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
                if (GameBoosterManager.Instance.CurrentActiveBooster == GameBoosterManager.ActiveBooster.None)
                {
                    GameBoosterManager.Instance.CurrentActiveBooster = GameBoosterManager.ActiveBooster.NormalBall;
                    _unmaskPanel.SetupUnmask(_normalButton.GetComponent<RectTransform>());
                }
                else
                {
                    GameBoosterManager.Instance.CurrentActiveBooster = GameBoosterManager.ActiveBooster.None;
                }
            });
            _rainbowButton.onClick.AddListener(() =>
            {
                if (GameBoosterManager.Instance.CurrentActiveBooster == GameBoosterManager.ActiveBooster.None)
                {
                    GameBoosterManager.Instance.CurrentActiveBooster = GameBoosterManager.ActiveBooster.RainbowBall;
                    _unmaskPanel.SetupUnmask(_rainbowButton.GetComponent<RectTransform>());
                }
                else
                {
                    GameBoosterManager.Instance.CurrentActiveBooster = GameBoosterManager.ActiveBooster.None;
                }
            });
        }

        public override void Open()
        {
            base.Open();

            UpdateLevelText();
        }

        public void UpdateLevelText()
        {
            _levelText.text = $"LEVEL {LevelManager.Instance.GetCurrentLevelIndex() + 1}";
        }

        public void UpdateMovesText()
        {
            _movesText.text = LevelManager.Instance.GetCurrentLevel().GetMoves().ToString();
        }
    }
}
