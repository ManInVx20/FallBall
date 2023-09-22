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
        private List<Cannon> _cannonList;
        [SerializeField]
        private List<Slot> _slotList;

        private List<Slot> _notFilledSlotList;
        private int _moves;

        private void Start()
        {
            _notFilledSlotList = _slotList.ToList();
            for (int i = 0; i < _notFilledSlotList.Count; i++)
            {
                _notFilledSlotList[i].OnIsFilledChangedAction += Slot_OnIsFilledChangedAction;
            }

            _moves = 0;

            GameUIManager.Instance.GetGameUIScreen<GameplayScreen>().UpdateMovesText();
        }

        public List<Cannon> GetCannonList()
        {
            return _cannonList;
        }

        public int GetMoves()
        {
            return _moves;
        }

        public void IncreaseMoves()
        {
            _moves += 1;

            GameUIManager.Instance.GetGameUIScreen<GameplayScreen>().UpdateMovesText();
        }

        public void DecreaseMoves()
        {
            _moves -= 1;

            GameUIManager.Instance.GetGameUIScreen<GameplayScreen>().UpdateMovesText();
        }

        public void Win()
        {
            //Debug.Log("Win");
            OnLevelFinishedAction?.Invoke(true);
        }

        public void Lose()
        {
            //Debug.Log("Lose");
            OnLevelFinishedAction?.Invoke(false);
        }

        private void Slot_OnIsFilledChangedAction(Slot slot)
        {
            if (slot.IsFilled() && _notFilledSlotList.Contains(slot))
            {
                _notFilledSlotList.Remove(slot);

                if (_notFilledSlotList.Count == 0)
                {
                    Win();
                }
            }
            else if (!slot.IsFilled() && !_notFilledSlotList.Contains(slot))
            {
                _notFilledSlotList.Add(slot);
            }
        }
    }
}
