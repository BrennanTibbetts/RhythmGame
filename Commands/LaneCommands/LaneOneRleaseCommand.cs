using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint5BeanTeam
{
    public class LaneOneReleaseCommand : ICommand
    {
        private HitboxSystem thisHitbox;

        public LaneOneReleaseCommand(HitboxSystem hitbox)
        {
            thisHitbox = hitbox;
        }
        public void Execute()
        {
            thisHitbox.setDeactive(1);
        }
    }
}
