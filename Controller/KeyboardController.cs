using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sprint5BeanTeam
{
    /// <summary>
    /// The Keyboard class implementation of controller
    /// </summary>
    public class KeyboardController : Controller
    {
        private KeyboardState previousState;

        public KeyboardController() : base()
        {
        }

        public KeyboardController(Dictionary<int, ICommand> keyBinding) : base(keyBinding,new Dictionary<int, ICommand>(), new Dictionary<int, int>())
        {
        }

        public KeyboardController(Dictionary<int, ICommand> keyBinding, Dictionary<int, int> conflictedKeys) : base(keyBinding,new Dictionary<int, ICommand>(), conflictedKeys)
        {
        }

        public KeyboardController(Dictionary<int, ICommand> shortPressKeyboardBinding, Dictionary<int, ICommand> longPressKeyboardBinding, Dictionary<int, int> conflictedKeys) : base(shortPressKeyboardBinding, longPressKeyboardBinding, conflictedKeys)
        {
        }


        public override void UpdateState()
        {
            KeyboardState currentState = Keyboard.GetState();

            foreach (KeyValuePair<int, ICommand> keyBinds in this.shortPressKeyBinding)
            {
                bool correspondKeyPressed = currentState.GetPressedKeys().Contains((Keys)keyBinds.Key);
                bool hasConflictedKey = this.conflictedKeys.ContainsKey(keyBinds.Key);
                bool conflictedKeyPressed = hasConflictedKey && currentState.GetPressedKeys().Contains((Keys)this.conflictedKeys[keyBinds.Key]);
                if (!previousState.IsKeyDown((Keys)keyBinds.Key) && correspondKeyPressed && !conflictedKeyPressed)
                {
                    keyBinds.Value.Execute();
                }

            }

            foreach (KeyValuePair<int, ICommand> keyBinds in this.longPressKeyBinding)
            {
                bool correspondKeyPressed = currentState.GetPressedKeys().Contains((Keys)keyBinds.Key);
                bool previousKeyNotPressed = previousState.IsKeyUp((Keys)keyBinds.Key) && correspondKeyPressed;
                bool currentKeyReleased = previousState.IsKeyDown((Keys)keyBinds.Key) && !correspondKeyPressed;
                bool hasConflictedKey = this.conflictedKeys.ContainsKey(keyBinds.Key);
                bool conflictedKeyPressed = hasConflictedKey && currentState.GetPressedKeys().Contains((Keys)this.conflictedKeys[keyBinds.Key]);
                //if (!previousState.IsKeyDown((Keys)keyBinds.Key) && correspondKeyPressed && !conflictedKeyPressed && previousKeyChanged)
                if (previousKeyNotPressed && !conflictedKeyPressed)
                {
                    keyBinds.Value.Execute();
                }
                else if (currentKeyReleased && !conflictedKeyPressed)
                {
                    if (!this.longPressKeyBinding.ContainsKey(-keyBinds.Key))
                        this.longPressKeyBinding[defaultCommand].Execute();
                    else this.longPressKeyBinding[-keyBinds.Key].Execute();
                }
                // else if (this.longPressKeyBinding[defaultCommand]!=null) this.longPressKeyBinding[defaultCommand].Execute();
                // else throw new NullReferenceException("Default case is not handled!");
            }
            this.previousState = currentState;
        }
    }
}