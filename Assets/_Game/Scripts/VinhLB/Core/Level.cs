using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VinhLB
{
    public class Level : MonoBehaviour
    {
        [SerializeField]
        private List<Slot> _slotList;

        private void Start()
        {
            for (int i = 0; i < _slotList.Count; i++)
            {
                _slotList[i].OnIsFilledChangedAction += Slot_OnIsFilledChangedAction;
            }
        }

        public void Finish()
        {
            Debug.Log("Finish level");
        }

        private void Slot_OnIsFilledChangedAction()
        {
            if (_slotList.FirstOrDefault(slot => !slot.IsFilled()) == null)
            {
                Finish();
            }
        }
    }
}
