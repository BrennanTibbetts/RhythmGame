using System;
using Sprint5BeanTeam;

namespace Sprint5BeanTeam
{
    public class QuitCommand : ICommand
    {

        private Game1 thisGame;

        public QuitCommand(Game1 game)
        {
            thisGame = game;
        }
        public void Execute()
        {
            switch (thisGame.CurrentGameState)
            {
                case (IState.GameState.StartMenu):
                    thisGame.Exit();
                    break;
                case (IState.GameState.SongSelectionMenu):
                    thisGame.CurrentGameState = IState.GameState.StartMenu;
                    break;
                case (IState.GameState.Play):
                case (IState.GameState.PauseMenu):
                    thisGame.CurrentGameState = IState.GameState.SongSelectionMenu;
                    break;
            }
            thisGame.SE.FastReset();
            

        }
    }
}

