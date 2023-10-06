using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VinhLB
{
    public class LevelItemButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;
        [SerializeField]
        private GameObject _lockGO;
        [SerializeField]
        private GameObject _unlockGO;
        [SerializeField]
        private GameObject _activeIndicatorGO;
        [SerializeField]
        private Image[] _starImageArray;
        [SerializeField]
        private TMP_Text _levelText;
        [SerializeField]
        private Color _lockColor;
        [SerializeField]
        private Color _unlockColor;

        private int _levelIndex;

        private void Awake()
        {
            _button.onClick.AddListener(() =>
            {
                LevelManager.Instance.TryLoadLevel(_levelIndex);

                GameUIManager.Instance.Open<GameplayScreen>();
            });
        }

        public void Setup(LevelItem item, int levelIndex, bool activeLevel)
        {
            if (item.Unlocked)
            {
                _button.interactable = true;
                _lockGO.SetActive(false);
                _unlockGO.SetActive(true);
                _activeIndicatorGO.SetActive(activeLevel);
                _levelIndex = levelIndex;
                _levelText.text = (_levelIndex + 1).ToString();
                SetStar(item.StarAchieved);
            }
            else
            {
                _button.interactable = false;
                _lockGO.SetActive(true);
                _unlockGO.SetActive(false);
            }
        }

        public void SetStar(int starAchieved)
        {
            for (int i = 0;  i < _starImageArray.Length; i++)
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
