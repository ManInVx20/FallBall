using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VinhLB
{
    public class InvisibleBackground : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private GraphicRaycaster _raycaster;

        private Action _onPointerClickCallback;

        public void Setup(Action onPointerClickCallback, bool active)
        {
            _onPointerClickCallback = onPointerClickCallback;

            gameObject.SetActive(active);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _onPointerClickCallback?.Invoke();

            List<RaycastResult> resultList = new List<RaycastResult>();
            _raycaster.Raycast(eventData, resultList);
            Debug.Log(resultList.Count);
            for (int i = 0; i < resultList.Count; i++)
            {
                Debug.Log("Hit " + resultList[i].gameObject.name);
                if (resultList[i].gameObject.GetComponent<Selectable>())
                {
                    ExecuteEvents.Execute(resultList[i].gameObject, eventData, ExecuteEvents.pointerClickHandler);
                }
            }
        }
    }

}