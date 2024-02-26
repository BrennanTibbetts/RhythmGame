using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sprint5BeanTeam
{
    /// <summary>
    /// The GamePad Controller implementation
    /// </summary>
    public class GamePadController : Controller
    {
        private const int defaultController = 0;
        private GamePadState previousState;

        public GamePadController() : base()
        {
        }

        public GamePadController(Dictionary<int, ICommand> keyBinding) : base(keyBinding)
        {
        }

        public GamePadController(Dictionary<int, ICommand> shortPressGamePadBinding, Dictionary<int, int> conflictedKeys) : base(shortPressGamePadBinding, conflictedKeys)
        {
        }

        public GamePadController(Dictionary<int, ICommand> shortPressGamePadBinding, Dictionary<int, ICommand> longPressGamePadBinding, Dictionary<int, int> conflictedKeys) : base(shortPressGamePadBinding, longPressGamePadBinding, conflictedKeys)
        {
        }

        public override void UpdateState()
        {
            GamePadState currentState = GamePad.GetState(0);

            foreach (KeyValuePair<int, ICommand> keyBinds in this.shortPressKeyBinding)
            {
                bool correspondKeyPressed = currentState.IsButtonDown((Buttons)keyBinds.Key);
                bool hasConflictedKey = this.conflictedKeys.ContainsKey(keyBinds.Key);
                bool conflictedKeyPressed = hasConflictedKey && currentState.IsButtonDown((Buttons)this.conflictedKeys[keyBinds.Key]);
                if (!previousState.IsButtonDown((Buttons)keyBinds.Key) && correspondKeyPressed && !conflictedKeyPressed)
                {
                    keyBinds.Value.Execute();
                }

            }

            foreach (KeyValuePair<int, ICommand> keyBinds in this.longPressKeyBinding)
            {
                bool correspondKeyPressed = currentState.IsButtonDown((Buttons)keyBinds.Key);
                bool previousKeyNotPressed = previousState.IsButtonDown((Buttons)keyBinds.Key) && correspondKeyPressed;
                bool currentKeyReleased = previousState.IsButtonDown((Buttons)keyBinds.Key) && !correspondKeyPressed;
                bool hasConflictedKey = this.conflictedKeys.ContainsKey(keyBinds.Key);
                bool conflictedKeyPressed = hasConflictedKey && currentState.IsButtonDown((Buttons)this.conflictedKeys[keyBinds.Key]);
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