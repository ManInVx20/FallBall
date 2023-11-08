using Coffee.UIExtensions;
using DG.Tweening;
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
        [SerializeField]
        private ParticleSystem _confettiVFX;
        [SerializeField]
        private UIParticle _winParticle;

        public override void Initialize()
        {
            _restartButton.onClick.AddListener(() =>
            {
                LevelManager.Instance.RestartLevel();

                GameUIManager.Instance.Open<GameplayScreen>();
            });
            _homeButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ReturnHome();
            });
            _nextButton.onClick.AddListener(() =>
            {
                if (LevelManager.Instance.TryLoadLevel(LevelManager.Instance.CurrentLevelIndex + 1))
                {
                    GameUIManager.Instance.Open<GameplayScreen>();

                    GameUIManager.Instance.GetGameUIScreen<GameplayScreen>().UpdateLevelText();
                }
            });
        }

        public override void Open()
        {
            base.Open();

            Level level = LevelManager.Instance.CurrentLevel;
            if (level.Won)
            {
                OpenWin(level.StarAmount);
            }
            else
            {
                OpenLose();
            }
        }

        public override void Close()
        {
            _winParticle.Stop();

            base.Close();
        }

        public void OpenWin(int starAmount)
        {
            _titleText.text = $"LEVEL {LevelManager.Instance.CurrentLevelIndex + 1} COMPLETED!";

            _nextButton.gameObject.SetActive(!LevelManager.Instance.IsLastLevel());

            LevelManager.Instance.CompleteLevel(starAmount);

            SetStar(starAmount);

            _winParticle.Play();
        }

        public void OpenLose()
        {
            _titleText.text = "LOSE";

            _nextButton.gameObject.SetActive(false);

            SetStar(0, false);
        }

        public void SetStar(int starAchieved, bool animated = true)
        {
            Sequence sequence = DOTween.Sequence();

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

                if (animated)
                {
                    _starImageArray[i].transform.localScale = Vector3.zero;
                    sequence.Append(_starImageArray[i].transform.DOScale(1.0f, 0.2f).SetEase(Ease.OutBack));
                }
                else
                {
                    _starImageArray[i].transform.localScale = Vector3.one;
                }
            }
        }
    }
}
