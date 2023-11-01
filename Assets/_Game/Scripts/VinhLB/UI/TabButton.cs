using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VinhLB
{
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private Image _background;

        private TabGroup _tabGroup;

        public void Initialize(TabGroup group)
        {
            _tabGroup = group;
        }

        public void ChangeBackgroundSprite(Sprite sprite)
        {
            _background.sprite = sprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _tabGroup.OnTabSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tabGroup.OnTabExit(this);
        }
    }
}
