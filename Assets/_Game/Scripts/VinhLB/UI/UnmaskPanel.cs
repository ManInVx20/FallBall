using Coffee.UIExtensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VinhLB
{
    public class UnmaskPanel : MonoBehaviour
    {
        [System.Serializable]
        public enum UnmaskShape
        {
            Circle = 0,
            Square = 1
        }

        private struct UnmaskData
        {
            public UnmaskShape Shape;
            public Unmask Unmask;
            public UnmaskRaycastFilter Filter;
        }

        [SerializeField]
        private RectTransform _unmaskPanelRectTF;
        [SerializeField]
        private Unmask _unmaskPrefab;
        [SerializeField]
        private Sprite[] unmaskSpriteArray;

        private Level _currentLevel;
        private Dictionary<RectTransform, UnmaskData> _unmaskDataDict = new Dictionary<RectTransform, UnmaskData>();

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

        public void OpenUnmask(RectTransform rectTransform, UnmaskShape shape)
        {
            if (_unmaskDataDict.ContainsKey(rectTransform))
            {
                _unmaskDataDict[rectTransform].Unmask.gameObject.SetActive(true);
                _unmaskDataDict[rectTransform].Filter.enabled = true;
            }
            else
            {
                SetupUnmask(rectTransform, shape);
            }
        }

        public void CloseUnmask(RectTransform rectTransform)
        {
            if (_unmaskDataDict.ContainsKey(rectTransform))
            {
                _unmaskDataDict[rectTransform].Unmask.gameObject.SetActive(false);
                _unmaskDataDict[rectTransform].Filter.enabled = false;
            }
        }

        public void SetupUnmask(RectTransform rectTransform, UnmaskShape shape)
        {
            UnmaskData unmaskData = new UnmaskData();

            Unmask unmask = Instantiate(_unmaskPrefab, _unmaskPanelRectTF);
            unmask.transform.SetAsFirstSibling();
            unmask.fitTarget = rectTransform;
            unmask.GetComponent<Image>().sprite = unmaskSpriteArray[(int)shape];
            unmaskData.Unmask = unmask;

            UnmaskRaycastFilter filter = _unmaskPanelRectTF.gameObject.AddComponent<UnmaskRaycastFilter>();
            filter.targetUnmask = unmask;
            unmaskData.Filter = filter;

            _unmaskDataDict.Add(rectTransform, unmaskData);
        }

        public void Open()
        {
            gameObject.SetActive(true);

            if (_currentLevel != LevelManager.Instance.CurrentLevel)
            {
                _currentLevel = LevelManager.Instance.CurrentLevel;

                foreach (KeyValuePair<RectTransform, UnmaskData> data in _unmaskDataDict)
                {
                    Destroy(data.Value.Unmask.gameObject);
                    Destroy(data.Value.Filter);
                }

                _unmaskDataDict.Clear();

                List<Cannon> cannonList = _currentLevel.CannonList;
                for (int i = 0; i < cannonList.Count; i++)
                {
                    SetupUnmask(cannonList[i].WorldCanvasRectTF,UnmaskShape.Circle);
                }
            }
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
