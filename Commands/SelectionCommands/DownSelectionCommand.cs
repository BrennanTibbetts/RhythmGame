using Scene;
using Sprint5BeanTeam;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint5BeanTeam
{
    public class DownSelectionCommand : ICommand
    {
        private SongSelectionScene thisSong;

        public DownSelectionCommand(SongSelectionScene songSelection)
        {
            thisSong = songSelection;
        }
        public void Execute()
        {
            thisSong.MoveDown();
        }
    }
}