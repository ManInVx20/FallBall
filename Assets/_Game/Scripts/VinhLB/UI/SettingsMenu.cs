using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VinhLB
{
    public class SettingsMenu : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Button _settingsButton;
        [SerializeField]
        private SettingsMenuItem[] _settingsMenuItemArray;
        [SerializeField]
        private InvisibleBackground _invisibleBackground;

        [Header("Settings")]
        [SerializeField]
        private float _toggleDelay = 0.5f;
        [SerializeField]
        private Vector2 _spaceBetweenMenuItems = Vector2.zero;
        [SerializeField]
        private float _rotationDuration = 0.3f;
        [SerializeField]
        private Ease _rotationEase = Ease.Linear;
        [SerializeField]
        private float _expandDuration = 0.3f;
        [SerializeField]
        private float _collapseDuration = 0.4f;
        [SerializeField]
        private Ease _expandEase = Ease.OutBack;
        [SerializeField]
        private Ease _collapseEase = Ease.InBack;
        [SerializeField]
        private float _expandFadeDuration = 0.3f;
        [SerializeField]
        private float _collapseFadeDuration = 0.5f;

        private Vector2 _settingsButtonPosition;
        private bool _isExpanded;
        private float _toggleTimer;

        private void Awake()
        {
            _settingsButton.onClick.AddListener(ToggleMenu);

            for (int i = 0; i < _settingsMenuItemArray.Length; i++)
            {
                _settingsMenuItemArray[i].Initialize(this, i);
            }

            _settingsButton.transform.SetAsLastSibling();

            _settingsButtonPosition = _settingsButton.GetComponent<RectTransform>().anchoredPosition;
        }

        private void OnEnable()
        {
            ResetState();
        }

        private void Update()
        {
            if (_toggleTimer > 0.0f)
            {
                _toggleTimer -= Time.deltaTime;
            }
        }

        public void OnMenuItemClick(int index)
        {
            switch (index)
            {
                case 0:
                    Debug.Log("Music");
                    break;
                case 1:
                    Debug.Log("Sound");
                    break;
                case 2:
                    Debug.Log("Vibration");
                    break;
                case 3:
                    Debug.Log("Return");
                    GameManager.Instance.ReturnHome();
                    break;

            }
        }

        public void ResetState()
        {
            _isExpanded = false;

            for (int i = 0; i < _settingsMenuItemArray.Length; i++)
            {
                _settingsMenuItemArray[i].RectTransform.anchoredPosition = _settingsButtonPosition;
                _settingsMenuItemArray[i].CanvasGroup.alpha = 0.0f;
            }

            _settingsButton.transform.eulerAngles = Vector3.zero;

            _invisibleBackground?.Setup(null, false);
        }

        private void ToggleMenu()
        {
            if (_toggleTimer > 0.0f)
            {
                return;
            }

            _toggleTimer = _toggleDelay;

            _isExpanded = !_isExpanded;

            if (_isExpanded)
            {
                for (int i = 0; i < _settingsMenuItemArray.Length; i++)
                {
                    //_settingsMenuItemArray[i].RectTransform.anchoredPosition = _settingsButtonPosition + _spaceBetweenMenuItems * (i + 1);
                    _settingsMenuItemArray[i].RectTransform
                        .DOAnchorPos(_settingsButtonPosition + _spaceBetweenMenuItems * (i + 1), _expandDuration)
                        .SetEase(_expandEase);
                    _settingsMenuItemArray[i].CanvasGroup.DOFade(1.0f, _expandFadeDuration).From(0.0f);
                }

                _invisibleBackground?.Setup(ToggleMenu, true);
            }
            else
            {
                for (int i = 0; i < _settingsMenuItemArray.Length; i++)
                {
                    //_settingsMenuItemArray[i].RectTransform.anchoredPosition = _settingsButtonPosition;
                    _settingsMenuItemArray[i].RectTransform
                        .DOAnchorPos(_settingsButtonPosition, _collapseDuration)
                        .SetEase(_collapseEase);
                    _settingsMenuItemArray[i].CanvasGroup.DOFade(0.0f, _collapseFadeDuration);
                }

                _invisibleBackground?.Setup(null, false);
            }

            Vector3 targetRotation = _settingsButton.transform.eulerAngles - Vector3.forward * 180.0f;
            _settingsButton.transform
                .DORotate(targetRotation, _rotationDuration)
                .SetEase(_rotationEase);
        }
    }
}
