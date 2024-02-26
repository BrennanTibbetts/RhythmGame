using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sprint5BeanTeam
{
    public interface ISprite
    {
        void addAnimation(string name, params int[] animFrames);
        void changeCurrentAnimation(string name);
        void updateSprite(GameTime gameTime, int speedInMilliseconds);
        void drawSprite(SpriteBatch spriteBatch, Vector2 location, bool isFlipped, int scale = 1);
    }
}
