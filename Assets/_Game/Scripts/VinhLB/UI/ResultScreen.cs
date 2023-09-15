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
                if (LevelManager.Instance.TryLoadNextLevel())
                {
                    Close();
                }
            });
        }

        public void OpenWin()
        {
            Open();

            _titleText.text = "WIN";

            if (LevelManager.Instance.IsLastLevel())
            {
                _nextButton.gameObject.SetActive(false);
            }
        }

        public void OpenLose()
        {
            Open();

            _titleText.text = "LOSE";

            _nextButton.gameObject.SetActive(false);
        }
    }
}
