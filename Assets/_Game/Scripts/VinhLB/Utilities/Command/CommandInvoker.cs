using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VinhLB
{
    public class CommandInvoker
    {
        private static Stack<ICommand> undoStack = new Stack<ICommand>();
        private static Stack<ICommand> redoStack = new Stack<ICommand>();

        public static void ExecuteCommand(ICommand command)
        {
            command.Execute();

            undoStack.Push(command);

            redoStack.Clear();
        }

        public static void UndoCommand()
        {
            if (undoStack.Count > 0)
            {
                ICommand activeCommand = undoStack.Pop();

                redoStack.Push(activeCommand);

                activeCommand.Undo();
            }
        }

        public static void RedoCommand()
        {
            if (redoStack.Count > 0)
            {
                ICommand activeCommand = redoStack.Pop();

                undoStack.Push(activeCommand);

                activeCommand.Execute();
            }
        }

        public static void UndoAllCommands()
        {
            while (undoStack.Count > 0)
            {
                UndoCommand();
            }
        }

        public static void RedoAllCommands()
        {
            while (redoStack.Count > 0)
            {
                RedoCommand();
            }
        }

        public static void Clear()
        {
            undoStack.Clear();
            redoStack.Clear();
        }
    }
}
