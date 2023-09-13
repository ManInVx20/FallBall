using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
