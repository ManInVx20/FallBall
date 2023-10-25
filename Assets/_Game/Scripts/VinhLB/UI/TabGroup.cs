using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class TabGroup : MonoBehaviour
    {
        [SerializeField]
        private List<TabButton> _tabButtonList;
        [SerializeField]
        private List<GameObject> _pageGOList;
        [SerializeField]
        private int _startTabIndex;
        [SerializeField]
        private Sprite _tabIdleSprite;
        [SerializeField]
        private Sprite _tabHoverSprite;
        [SerializeField]
        private Sprite _tabActiveSprite;

        private TabButton _selectedTabButton;

        private void Start()
        {
            if (_tabButtonList.Count > 0)
            {
                if (_startTabIndex < 0 || _startTabIndex >= _tabButtonList.Count)
                {
                    _startTabIndex = 0;
                }

                OnTabSelected(_tabButtonList[_startTabIndex]);
            }
        }

        public void OnTabEnter(TabButton button)
        {
            ResetTabs();

            if (_selectedTabButton == null || button != _selectedTabButton)
            {
                button.ChangeBackgroundSprite(_tabHoverSprite);
            }
        }

        public void OnTabExit(TabButton button)
        {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button)
        {
            if (button == _selectedTabButton)
            {
                return;
            }

            _selectedTabButton = button;

            ResetTabs();

            button.ChangeBackgroundSprite(_tabActiveSprite);

            int index = _tabButtonList.IndexOf(button);
            for (int i = 0; i < _pageGOList.Count; i++)
            {
                if (i == index)
                {
                    _pageGOList[i].SetActive(true);
                }
                else
                {
                    _pageGOList[i].SetActive(false);
                }
            }
        }

        private void ResetTabs()
        {
            for (int i = 0; i < _tabButtonList.Count; i++)
            {
                if (_tabButtonList[i] == _selectedTabButton)
                {
                    continue;
                }

                _tabButtonList[i].ChangeBackgroundSprite(_tabIdleSprite);
            }
        }
    }
}
