using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint5BeanTeam
{
    /// <summary>
    /// This is the interface of controllers that provide general control over different input devices
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// This defines the accepted input type of Controller
        /// </summary>
        public enum ControllerType
        {
            [Description("Undefined")]
            Undefined,
            [Description("Keyboard")]
            Keyboard,
            [Description("GamePad")]
            GamePad,
        }

        /// <summary>
        /// Update the states of controller, and translate keypress to commands
        /// </summary>
        public void UpdateState();

        public void SetShortPressKeyBinding(Dictionary<int, ICommand> keyBinding);
        public void SetLongPressKeyBinding(Dictionary<int, ICommand> keyBinding);
        public void SetConflictedKeyBinding(Dictionary<int, int> keyBinding);

        public void ClearKeyBinding();
    }
}
