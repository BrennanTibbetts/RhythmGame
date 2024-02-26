using System;

namespace Sprint5BeanTeam
{
    public class MenuSelectCommand : ICommand
    {

        private Game1 thisGame;

        public MenuSelectCommand(Game1 game)
        {
            thisGame = game;
        }
        public void Execute()
        {
            //dont worry about this yet
        }
    }
}
