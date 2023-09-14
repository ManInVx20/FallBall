using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VinhLB
{
    public class GameplayScreen : GameUIScreen
    {
        [SerializeField]
        private Button _undoButton;

        private void Awake()
        {
            _undoButton.onClick.AddListener(() =>
            {
                CommandInvoker.UndoCommand();
            });
        }
    }
}