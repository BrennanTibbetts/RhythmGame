using Sprint5BeanTeam;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint5BeanTeam
{
    public class SelectSongCommand : ICommand
    {
        private Game1 thisGame;

        public SelectSongCommand(Game1 game)
        {
            thisGame = game;
        }
        public void Execute()
        {
            thisGame.scene.ReloadScene();
            thisGame.SE.Update("confirm");
        }
    }
}
