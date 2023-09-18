using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class AddCommand : ICommand
    {
        private Cannon _cannon;
        private BallType _ballType;

        public AddCommand(Cannon cannon, BallType ballType)
        {
            _cannon = cannon;
            _ballType = ballType;
        }

        public void Execute()
        {
            _cannon.PushBallType(_ballType);
        }

        public void Undo()
        {
            _cannon.PullBallType();
        }
    }
}
