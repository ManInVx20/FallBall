using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VinhLB
{
    public class GameUIBliking : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        [SerializeField]
        private float _blinkingCycleDuration = 1.0f;
        [SerializeField]
        private Ease _blinkingEase = Ease.Linear;

        private Tween _blinkingTween;

        private void Awake()
        {
            _blinkingTween = _canvasGroup.DOFade(0.0f, _blinkingCycleDuration)
                .SetEase(_blinkingEase).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnEnable()
        {
            _canvasGroup.alpha = 1.0f;

            _blinkingTween.Play();
        }

        private void OnDisable()
        {
            _blinkingTween.Pause();
        }

    }
}
