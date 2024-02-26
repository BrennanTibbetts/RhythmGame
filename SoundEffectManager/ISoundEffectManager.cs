using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;
using static Sprint5BeanTeam.IState;

namespace Sprint5BeanTeam
{
    public delegate void NotifyObserver(string message);
    public interface ISoundEffectManager
    {
        public void FastReset();
        public void PlayMusic();
        public void Pause();
        public void StopMusic();
        public void Update(string flag);
    }
}
