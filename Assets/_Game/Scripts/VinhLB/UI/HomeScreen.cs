using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VinhLB
{
    public class HomeScreen : GameUIScreen
    {
        [SerializeField]
        private TMP_Text _startAmountText;
        [SerializeField]
        private RectTransform _levelItemButtonHolderTF;
        [SerializeField]
        private LevelItemButton _levelItemButtonPrefab;

        private List<LevelItemButton> _levelItemButtonList = new List<LevelItemButton>();

        public override void Initialize()
        {
            LevelItem[] levelItemArray = LevelManager.Instance.LevelData.LevelItemArray;
            for (int i = 0; i < levelItemArray.Length; i++)
            {
                LevelItemButton levelItemButton = Instantiate(_levelItemButtonPrefab, _levelItemButtonHolderTF);
                levelItemButton.Setup(levelItemArray[i], i, 
                    i == LevelManager.Instance.LevelData.LastUnlockedLevelIndex);

                _levelItemButtonList.Add(levelItemButton);
            }
        }

        public override void Open()
        {
            base.Open();

            _startAmountText.text = LevelManager.Instance.LevelData.GetAllStarAchievedAmount().ToString();

            LevelItem[] levelItemArray = LevelManager.Instance.LevelData.LevelItemArray;
            for (int i = 0; i < _levelItemButtonList.Count; i++)
            {
                _levelItemButtonList[i].Setup(levelItemArray[i], i,
                    i == LevelManager.Instance.LevelData.LastUnlockedLevelIndex);
            }
        }
    }
}
