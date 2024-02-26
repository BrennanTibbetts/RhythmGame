using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint5BeanTeam
{
    public class LaneOneCommand : ICommand
    {
        private HitboxSystem thisHitbox;
        private NoteManager noteManager;

        public LaneOneCommand(HitboxSystem hitbox, NoteManager noteManager)
        {
            thisHitbox = hitbox;
            this.noteManager = noteManager;
        }
        public void Execute()
        {
            thisHitbox.setActive(1);
            this.noteManager.HandleInput(1);
        }
    }
}
