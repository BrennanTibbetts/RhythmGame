using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint5BeanTeam
{
    public interface IHUD
    {
        void Update(GameTime gametime);
        void Draw(SpriteBatch spriteBatch);
    }
}

