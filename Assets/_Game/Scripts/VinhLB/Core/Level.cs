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
        public int MovesLeft => _movesLeft;
        public bool Won => _won;
        public int StarAmount => _starAmount;

        [SerializeField]
        private List<Cannon> _cannonList;
        [SerializeField]
        private List<Slot> _slotList;

        private LevelInfo _levelInfo;
        private List<Slot> _notFilledSlotList;
        private int _movesLeft;
        private bool _won;
        private int _starAmount;
        private Coroutine _waitingToLoseCoroutine;

        public void Initialize(LevelInfo info)
        {
            _levelInfo = info;
            _notFilledSlotList = _slotList.ToList();
            for (int i = 0; i < _notFilledSlotList.Count; i++)
            {
                _notFilledSlotList[i].OnIsFilledChangedAction += Slot_OnIsFilledChangedAction;
            }

            ResetState();
        }

        public void ResetState()
        {
            _movesLeft = _levelInfo.MaxMoves;
            _won = false;
            _starAmount = 0;

            GameUIManager.Instance.GetGameUIScreen<GameplayScreen>().UpdateMovesLeftText();
        }

        public void IncreaseMoves()
        {
            _movesLeft += 1;

            GameUIManager.Instance.GetGameUIScreen<GameplayScreen>().UpdateMovesLeftText();

            if (_waitingToLoseCoroutine != null)
            {
                StopCoroutine(_waitingToLoseCoroutine);
            }
        }

        public void DecreaseMoves()
        {
            _movesLeft -= 1;

            GameUIManager.Instance.GetGameUIScreen<GameplayScreen>().UpdateMovesLeftText();

            if (_movesLeft == 0)
            {
                if (_waitingToLoseCoroutine == null)
                {
                    _waitingToLoseCoroutine = StartCoroutine(WaitingToLoseCoroutine());
                }
            }
        }

        public void Win(bool skipMoves = false, int forcedStarAmount = 3)
        {
            _won = true;

            GameBoosterManager.Instance.CurrentActiveBooster = GameBoosterManager.ActiveBooster.None;

            if (skipMoves)
            {
                _starAmount = forcedStarAmount;
            }
            else
            {
                int movesUsed = _levelInfo.MaxMoves - _movesLeft;
                if (movesUsed <= _levelInfo.MaxMoves3Stars)
                {
                    _starAmount = 3;
                }
                else if (movesUsed <= _levelInfo.MaxMoves2Stars)
                {
                    _starAmount = 2;
                }
                else
                {
                    _starAmount = 1;
                }
            }

            GameUIManager.Instance.Open<ResultScreen>();
        }

        public void Lose()
        {
            _won = false;
            _starAmount = 0;

            GameUIManager.Instance.Open<ResultScreen>();
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

        private IEnumerator WaitingToLoseCoroutine()
        {
            while (true)
            {
                if (BallPool.Instance.IsAllInactive())
                {
                    break;
                }

                yield return null;
            }

            Lose();

            _waitingToLoseCoroutine = null;
        }
    }
}
