using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VinhLB
{
    public class CannonListPanel : MonoBehaviour
    {
        [SerializeField]
        private Button _cannonItemButtonPrefab;

        private Level _currentLevel;
        private List<Button> _cannonItemButtonList = new List<Button>();



        public void Open()
        {
            gameObject.SetActive(true);

            if (_currentLevel != LevelManager.Instance.GetCurrentLevel())
            {
                _currentLevel = LevelManager.Instance.GetCurrentLevel();

                for (int i = _cannonItemButtonList.Count - 1; i >= 0 ; i--)
                {
                    Destroy(_cannonItemButtonList[i].gameObject);
                    _cannonItemButtonList.RemoveAt(i);
                }

                List<Cannon> cannonList = _currentLevel.GetCannonList();
                for (int i = 0; i < cannonList.Count; i++)
                {
                    int index = i;
                    Button cannonItemButton = Instantiate(_cannonItemButtonPrefab, transform);
                    cannonItemButton.onClick.AddListener(() =>
                    {
                        ICommand command = new AddCommand(cannonList[index], BallType.Rainbow);
                        CommandInvoker.ExecuteCommand(command);
                    });
                    cannonItemButton.GetComponentInChildren<TMP_Text>().text = (index + 1).ToString();
                    _cannonItemButtonList.Add(cannonItemButton);
                }
            }
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}