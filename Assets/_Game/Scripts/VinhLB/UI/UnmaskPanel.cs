using Coffee.UIExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VinhLB
{
    public class UnmaskPanel : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _unmaskPanelRectTF;
        [SerializeField]
        private Unmask _unmaskPrefab;

        private Level _currentLevel;
        private List<Unmask> _unmaskList = new List<Unmask>();
        private List<UnmaskRaycastFilter> _unmaskRaycastFilterList = new List<UnmaskRaycastFilter>();

        private void Start()
        {
            GameBoosterManager.Instance.OnActiveBoosterChanged += GameBoosterManager_OnActiveBoosterChanged;

            GameBoosterManager_OnActiveBoosterChanged();
        }

        private void GameBoosterManager_OnActiveBoosterChanged()
        {
            if (GameBoosterManager.Instance.CurrentActiveBooster != GameBoosterManager.ActiveBooster.None)
            {
                Open();
            }
            else
            {
                Close();
            }
        }

        public void SetupUnmask(RectTransform rectTransform)
        {
            Unmask unmask = Instantiate(_unmaskPrefab, _unmaskPanelRectTF);
            unmask.transform.SetAsFirstSibling();
            unmask.fitTarget = rectTransform;
            _unmaskList.Add(unmask);

            UnmaskRaycastFilter unmaskRaycastFilter = _unmaskPanelRectTF.gameObject.AddComponent<UnmaskRaycastFilter>();
            unmaskRaycastFilter.targetUnmask = unmask;
            _unmaskRaycastFilterList.Add(unmaskRaycastFilter);
        }

        public void Open()
        {
            gameObject.SetActive(true);

            if (_currentLevel != LevelManager.Instance.GetCurrentLevel())
            {
                _currentLevel = LevelManager.Instance.GetCurrentLevel();

                for (int i = _unmaskList.Count - 1; i >= 0; i--)
                {
                    Destroy(_unmaskList[i].gameObject);
                    _unmaskList.RemoveAt(i);

                    Destroy(_unmaskRaycastFilterList[i]);
                    _unmaskRaycastFilterList.RemoveAt(i);
                }

                List<Cannon> cannonList = _currentLevel.GetCannonList();
                for (int i = 0; i < cannonList.Count; i++)
                {
                    SetupUnmask(cannonList[i].GetWorldCanvasRectTF());
                }
            }
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
