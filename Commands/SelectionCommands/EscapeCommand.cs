using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprint5BeanTeam;

namespace Sprint5BeanTeam
{
    public class EscapeCommand : ICommand
    {
        private Game1 thisGame;
        public EscapeCommand(Game1 game)
        {
            thisGame = game;
        }

        public void Execute()
        {
            thisGame.CurrentGameState = IState.GameState.StartMenu;
            thisGame.SE.Update("cancel");
        }
    }
}
