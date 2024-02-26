using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint5BeanTeam
{
    public class LaneThreeCommand : ICommand
    {
        private HitboxSystem thisHitbox;
        private NoteManager noteManager;

        public LaneThreeCommand(HitboxSystem hitbox, NoteManager noteManager)
        {
            thisHitbox = hitbox;
            this.noteManager = noteManager;
        }
        public void Execute()
        {
            thisHitbox.setActive(3);
            this.noteManager.HandleInput(3);
        }
    }
}
