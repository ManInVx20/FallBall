using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VinhLB
{
    public class GameUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private float _downScaleValue = 0.9f;
        [SerializeField]
        private float _downDuration = 0.1f;
        [SerializeField]
        private float _upDuration = 0.5f;
        [SerializeField]
        private Ease _downEase = Ease.Linear;
        [SerializeField]
        private Ease _upEase = Ease.OutElastic;

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.DOScale(_downScaleValue, _downDuration).SetEase(_downEase);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.DOScale(1.0f, _upDuration).SetEase(_upEase);
        }
    }
}
