using DG.Tweening;
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
        private TMP_Text _movesLeftText;
        [SerializeField]
        private Button _restartButton;
        [SerializeField]
        private Button _undoButton;
        [SerializeField]
        private Button _moveButton;
        [SerializeField]
        private Button _rainbowButton;
        [SerializeField]
        private TMP_Text _undoAmount;
        [SerializeField]
        private TMP_Text _moveAmount;
        [SerializeField]
        private TMP_Text _rainbowAmount;
        [SerializeField]
        private SettingsMenu _settingsMenu;
        [SerializeField]
        private UnmaskPanel _unmaskPanel;

        private void Start()
        {
            GameBoosterManager.Instance.OnBoosterAmountDictChanged += GameBoosterManager_OnBoosterAmountDictChanged;
        }

        public override void Initialize()
        {
            _restartButton.onClick.AddListener(() =>
            {
                LevelManager.Instance.RestartLevel();
            });
            _undoButton.onClick.AddListener(() =>
            {
                if (GameBoosterManager.Instance.GetBoosterAmountByType(BoosterType.UndoMove) > 0)
                {
                    if (CommandInvoker.HasCommandToUndo())
                    {
                        GameBoosterManager.Instance.ModifyBoosterAmountByType(BoosterType.UndoMove, -1);

                        CommandInvoker.UndoCommand();
                    }
                }
            });
            _moveButton.onClick.AddListener(() =>
            {
                if (GameBoosterManager.Instance.GetBoosterAmountByType(BoosterType.AddMove) > 0)
                {
                    GameBoosterManager.Instance.ModifyBoosterAmountByType(BoosterType.AddMove, -1);

                    //LevelManager.Instance.CurrentLevel.IncreaseMoves();
                    LevelManager.Instance.CurrentLevel.ModifyMovesLeft(1);
                }
            });
            _rainbowButton.onClick.AddListener(OnRainbowButtonClick);
        }

        public override void Open()
        {
            base.Open();

            UpdateLevelText();

            UpdateBoosterUI();
        }

        public void UpdateLevelText()
        {
            _levelText.text = $"LEVEL {LevelManager.Instance.CurrentLevelIndex + 1}";
        }

        public void UpdateMovesLeftText(bool animated = true)
        {
            _movesLeftText.text = LevelManager.Instance.CurrentLevel.MovesLeft.ToString();

            if (animated)
            {
                _movesLeftText.transform.DOPunchScale(Vector3.one * 0.5f, 0.2f);
            }
        }

        public void UpdateBoosterUI()
        {
            int undoAmount = GameBoosterManager.Instance.GetBoosterAmountByType(BoosterType.UndoMove);
            int moveAmount = GameBoosterManager.Instance.GetBoosterAmountByType(BoosterType.AddMove);
            int rainbowAmount = GameBoosterManager.Instance.GetBoosterAmountByType(BoosterType.AddRainbowBall);

            _undoButton.interactable = undoAmount > 0;
            _moveButton.interactable = moveAmount > 0;
            _rainbowButton.interactable = rainbowAmount > 0;

            _undoAmount.text = undoAmount.ToString();
            _moveAmount.text = moveAmount.ToString();
            _rainbowAmount.text = rainbowAmount.ToString();
        }

        private void GameBoosterManager_OnBoosterAmountDictChanged()
        {
            UpdateBoosterUI();
        }

        public void OnRainbowButtonClick()
        {
            if (GameBoosterManager.Instance.CurrentActiveBoosterType == BoosterType.None)
            {
                if (GameBoosterManager.Instance.GetBoosterAmountByType(BoosterType.AddRainbowBall) > 0)
                {
                    GameBoosterManager.Instance.CurrentActiveBoosterType = BoosterType.AddRainbowBall;
                    _unmaskPanel.OpenUnmask(_rainbowButton.GetComponent<RectTransform>(), UnmaskPanel.UnmaskShape.Square);
                }
            }
            else
            {
                GameBoosterManager.Instance.CurrentActiveBoosterType = BoosterType.None;
                _unmaskPanel.CloseUnmask(_rainbowButton.GetComponent<RectTransform>());
            }
        }
    }
}
