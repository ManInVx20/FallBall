using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VinhLB
{
    public class SettingsMenuItem : MonoBehaviour
    {
        public RectTransform RectTransform => _rectTransform;
        public CanvasGroup CanvasGroup => _canvasGroup;

        [SerializeField]
        private RectTransform _rectTransform;
        [SerializeField]
        private CanvasGroup _canvasGroup;
        [SerializeField]
        private Button _button;

        private SettingsMenu _menu;
        private int _index;

        public void Initialize(SettingsMenu menu, int index)
        {
            _menu = menu;
            _index = index;

            _button.onClick.AddListener(() =>
            {
                _menu.OnMenuItemClick(_index);
            });
        }
    }
}
