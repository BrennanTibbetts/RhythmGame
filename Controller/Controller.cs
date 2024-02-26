using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Diagnostics.Metrics;
using System.Globalization;

namespace Sprint5BeanTeam
{
    /// <summary>
    /// Abstract controller class: implements basic functionality
    /// </summary>
    public abstract class Controller : IController
    {
        protected const int defaultCommand = -1;
        protected Dictionary<int, ICommand> shortPressKeyBinding;
        protected Dictionary<int, ICommand> longPressKeyBinding;
        protected Dictionary<int, int> conflictedKeys;

        /// <summary>
        /// Constructor of controller
        /// </summary>
        public Controller()
        {
            this.shortPressKeyBinding = new();
            this.conflictedKeys = new();
        }

        public Controller(Dictionary<int, ICommand> keyboardBinding)
        {
            this.shortPressKeyBinding = new(keyboardBinding);
            this.conflictedKeys = new();
        }

        public Controller(Dictionary<int, ICommand> keyboardBinding, Dictionary<int, int> conflictedKeys)
        {
            this.shortPressKeyBinding = new(keyboardBinding);
            this.longPressKeyBinding = new();
            this.conflictedKeys = new(conflictedKeys);
        }
        public Controller(Dictionary<int, ICommand> shortPressKeyboardBinding, Dictionary<int, ICommand> longPressKeyboardBinding, Dictionary<int, int> conflictedKeys)
        {
            this.shortPressKeyBinding = new(shortPressKeyboardBinding);
            this.longPressKeyBinding = new(longPressKeyboardBinding);
            this.conflictedKeys = new(conflictedKeys);
        }

        public virtual void SetShortPressKeyBinding(Dictionary<int, ICommand> keyboardBinding)
        {
            this.shortPressKeyBinding = new(keyboardBinding);
        }

        public virtual void SetLongPressKeyBinding(Dictionary<int, ICommand> keyboardBinding)
        {
            this.longPressKeyBinding = new(keyboardBinding);
        }

        public virtual void SetConflictedKeyBinding(Dictionary<int, int> conflictedKeys)
        {
            this.conflictedKeys = new(conflictedKeys);
        }

        public virtual void ClearConflictedKeys()
        {
            this.conflictedKeys = new();
        }

        public virtual void ClearKeyBinding()
        {
            this.shortPressKeyBinding = new();
            this.longPressKeyBinding = new();
            this.conflictedKeys = new();
        }

        public abstract void UpdateState();
    }
}
