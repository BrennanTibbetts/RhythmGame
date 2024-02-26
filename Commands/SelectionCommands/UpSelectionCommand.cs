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
    public class UpSelectionCommand : ICommand
    {
        private SongSelectionScene thisSong;

        public UpSelectionCommand(SongSelectionScene songSelection)
        {
            thisSong = songSelection;
        }
        public void Execute()
        {
            thisSong.MoveUp();
        }
    }
}