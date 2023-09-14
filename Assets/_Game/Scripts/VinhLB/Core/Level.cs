using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VinhLB
{
    public class Level : MonoBehaviour
    {
        public System.Action<bool> OnLevelFinishedAction;

        [SerializeField]
        private List<Slot> _slotList;

        private void Start()
        {
            for (int i = 0; i < _slotList.Count; i++)
            {
                _slotList[i].OnIsFilledChangedAction += Slot_OnIsFilledChangedAction;
            }
        }

        public void Win()
        {
            Debug.Log("Win");
            OnLevelFinishedAction?.Invoke(true);
        }

        public void Lose()
        {
            Debug.Log("Lose");
            OnLevelFinishedAction?.Invoke(false);
        }

        private void Slot_OnIsFilledChangedAction()
        {
            if (_slotList.FirstOrDefault(slot => !slot.IsFilled()) == null)
            {
                Win();
            }
        }
    }
}
