using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint5BeanTeam
{
    public class BarNoteCommand : ICommand
    {
        private HitboxSystem thisHitbox;
        private NoteManager noteManager;

        public BarNoteCommand(HitboxSystem hitbox, NoteManager noteManager)
        {
            thisHitbox = hitbox;
            this.noteManager = noteManager;
        }
        public void Execute()
        {
            thisHitbox.setAllActive();
            this.noteManager.HandleInput(6);
        }
    }
}