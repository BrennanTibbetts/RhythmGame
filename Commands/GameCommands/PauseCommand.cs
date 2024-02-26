using System;
using Sprint5BeanTeam;

namespace Sprint5BeanTeam
{
    public class PauseCommand : ICommand
    {
        private Game1 thisGame;

        public PauseCommand(Game1 game)
        {
            thisGame = game;
        }
        public void Execute()
        {
            //likely need to change this
            if (thisGame.CurrentGameState == IState.GameState.Play)
            {
                thisGame.SE.Pause();
                thisGame.CurrentGameState = IState.GameState.PauseMenu;
            }
        }
    }
}

