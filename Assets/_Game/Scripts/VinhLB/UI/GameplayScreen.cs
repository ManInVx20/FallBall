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
        private Button _settingsButton;
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
            _settingsButton.onClick.AddListener(() =>
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
            _normalButton.onClick.AddListener(OnNormalButtonClicked);
            _rainbowButton.onClick.AddListener(OnRainbowButtonClicked);
            _spikeButton.onClick.AddListener(OnSpikeButtonClicked);
        }

        public override void Open()
        {
            base.Open();

            UpdateLevelText();
        }

        public void UpdateLevelText()
        {
            _levelText.text = $"LEVEL {LevelManager.Instance.CurrentLevelIndex + 1}";
        }

        public void UpdateMovesText()
        {
            _movesText.text = LevelManager.Instance.CurrentLevel.Moves.ToString();
        }

        public void OnNormalButtonClicked()
        {
            if (GameBoosterManager.Instance.CurrentActiveBooster == GameBoosterManager.ActiveBooster.None)
            {
                GameBoosterManager.Instance.CurrentActiveBooster = GameBoosterManager.ActiveBooster.NormalBall;
                _unmaskPanel.OpenUnmask(_normalButton.GetComponent<RectTransform>(), UnmaskPanel.UnmaskShape.Square);
            }
            else
            {
                GameBoosterManager.Instance.CurrentActiveBooster = GameBoosterManager.ActiveBooster.None;
                _unmaskPanel.CloseUnmask(_normalButton.GetComponent<RectTransform>());
            }
        }

        public void OnRainbowButtonClicked()
        {
            if (GameBoosterManager.Instance.CurrentActiveBooster == GameBoosterManager.ActiveBooster.None)
            {
                GameBoosterManager.Instance.CurrentActiveBooster = GameBoosterManager.ActiveBooster.RainbowBall;
                _unmaskPanel.OpenUnmask(_rainbowButton.GetComponent<RectTransform>(), UnmaskPanel.UnmaskShape.Square);
            }
            else
            {
                GameBoosterManager.Instance.CurrentActiveBooster = GameBoosterManager.ActiveBooster.None;
                _unmaskPanel.CloseUnmask(_rainbowButton.GetComponent<RectTransform>());
            }
        }
        
        public void OnSpikeButtonClicked()
        {
            if (GameBoosterManager.Instance.CurrentActiveBooster == GameBoosterManager.ActiveBooster.None)
            {
                GameBoosterManager.Instance.CurrentActiveBooster = GameBoosterManager.ActiveBooster.SpikeBall;
                _unmaskPanel.OpenUnmask(_spikeButton.GetComponent<RectTransform>(), UnmaskPanel.UnmaskShape.Square);
            }
            else
            {
                GameBoosterManager.Instance.CurrentActiveBooster = GameBoosterManager.ActiveBooster.None;
                _unmaskPanel.CloseUnmask(_spikeButton.GetComponent<RectTransform>());
            }
        }
    }
}
