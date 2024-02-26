using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint5BeanTeam
{
    public class LaneFourReleaseCommand : ICommand
    {
        private HitboxSystem thisHitbox;

        public LaneFourReleaseCommand(HitboxSystem hitbox)
        {
            thisHitbox = hitbox;
        }
        public void Execute()
        {
            thisHitbox.setDeactive(4);
        }
    }
}
