using System;
using System.Diagnostics;
using Sprint5BeanTeam;

namespace Sprint5BeanTeam
{
    public class RestartCommand : ICommand
    {

        private Game1 thisGame;

        public RestartCommand(Game1 game)
        {
            thisGame = game;
        }
        public void Execute()
        {
            
            if (thisGame.CurrentGameState == IState.GameState.StartMenu)
            {
                thisGame.CurrentGameState = IState.GameState.SongSelectionMenu;
            }
            else if (thisGame.CurrentGameState == IState.GameState.GameOver)
            {
                thisGame.CurrentGameState = IState.GameState.SongSelectionMenu;
            }
            else
            {
                thisGame.scene.ReloadScene();
            }
            thisGame.SE.Update("cancel");
        }
    }
}

