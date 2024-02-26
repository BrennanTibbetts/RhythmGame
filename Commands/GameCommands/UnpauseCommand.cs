using System;
using Sprint5BeanTeam;

namespace Sprint5BeanTeam
{
    public class UnpauseCommand : ICommand
    {
        private Game1 thisGame;

        public UnpauseCommand(Game1 game)
        {
            thisGame = game;
        }
        public void Execute()
        {
            //likely need to change this
            if (thisGame.CurrentGameState == IState.GameState.PauseMenu)
            {
                thisGame.CurrentGameState = IState.GameState.Countdown;
            }
        }
    }
}

