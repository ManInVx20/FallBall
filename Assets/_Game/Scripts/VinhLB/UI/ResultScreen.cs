using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VinhLB
{
    public class ResultScreen : GameUIScreen
    {
        [SerializeField]
        private TMP_Text _titleText;
        [SerializeField]
        private Image[] _starImageArray;
        [SerializeField]
        private Color _lockColor;
        [SerializeField]
        private Color _unlockColor;
        [SerializeField]
        private Button _restartButton;
        [SerializeField]
        private Button _homeButton;
        [SerializeField]
        private Button _nextButton;

        public override void Initialize()
        {
            _restartButton.onClick.AddListener(() =>
            {
                LevelManager.Instance.RestartLevel();

                Close();
            });
            _homeButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ReturnHome();

                Close();
            });
            _nextButton.onClick.AddListener(() =>
            {
                if (LevelManager.Instance.TryLoadLevel(LevelManager.Instance.CurrentLevelIndex + 1))
                {
                    Close();

                    GameUIManager.Instance.GetGameUIScreen<GameplayScreen>().UpdateLevelText();
                }
            });
        }

        public void OpenWin(int starAmount)
        {
            Open();

            _titleText.text = $"LEVEL {LevelManager.Instance.CurrentLevelIndex + 1} COMPLETED!";

            _nextButton.gameObject.SetActive(!LevelManager.Instance.IsLastLevel());

            LevelManager.Instance.CompleteLevel(starAmount);

            SetStar(starAmount);
        }

        public void OpenLose()
        {
            Open();

            _titleText.text = "LOSE";

            _nextButton.gameObject.SetActive(false);
        }

        public void SetStar(int starAchieved)
        {
            for (int i = 0; i < _starImageArray.Length; i++)
            {
                if (i < starAchieved)
                {
                    _starImageArray[i].color = _unlockColor;
                }
                else
                {
                    _starImageArray[i].color = _lockColor;
                }
            }
        }
    }
}
