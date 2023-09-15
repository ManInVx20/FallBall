using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VinhLB
{
    public class GameplayScreen : GameUIScreen
    {
        [SerializeField]
        private Button _homeButton;
        [SerializeField]
        private Button _undoButton;

        public override void Initialize()
        {
            _homeButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ReturnHome();
            });
            _undoButton.onClick.AddListener(() =>
            {
                CommandInvoker.UndoCommand();
            });
        }
    }
}
