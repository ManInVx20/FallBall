using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VinhLB
{
    public class Level : MonoBehaviour
    {
        public List<Cannon> CannonList => _cannonList;
        public int Moves => _moves;

        [SerializeField]
        private List<Cannon> _cannonList;
        [SerializeField]
        private List<Slot> _slotList;

        private LevelInfo _levelInfo;
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

        public void Initialize(LevelInfo info)
        {
            _levelInfo = info;
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

        public void Win(bool skipMoves = false, int forcedStarAmount = 3)
        {
            //Debug.Log("Win");
            GameBoosterManager.Instance.CurrentActiveBooster = GameBoosterManager.ActiveBooster.None;

            int starAmount;
            if (skipMoves)
            {
                starAmount = forcedStarAmount;
            }
            else
            {
                if (_moves <= _levelInfo.MaxMoves3Stars)
                {
                    starAmount = 3;
                }
                else if (_moves <= _levelInfo.MaxMoves2Stars)
                {
                    starAmount = 2;
                }
                else
                {
                    starAmount = 1;
                }
            }
            GameUIManager.Instance.GetGameUIScreen<ResultScreen>().OpenWin(starAmount);
        }

        public void Lose()
        {
            //Debug.Log("Lose");
            GameUIManager.Instance.GetGameUIScreen<ResultScreen>().OpenLose();
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
