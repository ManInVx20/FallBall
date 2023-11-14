using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class SpawnCommand : ICommand
    {
        private Cannon _cannon;
        private Ball _ball;

        public SpawnCommand(Cannon cannon)
        {
            _cannon = cannon;
        }

        public void Execute()
        {
            _ball = _cannon.SpawnBall();

            //LevelManager.Instance.CurrentLevel.DecreaseMoves();
            LevelManager.Instance.CurrentLevel.ModifyMovesLeft(-1);
        }

        public void Undo()
        {
            _cannon.RetrieveBall(_ball);

            //LevelManager.Instance.CurrentLevel.IncreaseMoves();
            LevelManager.Instance.CurrentLevel.ModifyMovesLeft(1);
        }
    }
}
