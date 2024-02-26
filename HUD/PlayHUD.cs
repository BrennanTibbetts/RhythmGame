using System;
using GameCamera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint5BeanTeam.ProgressBar;

namespace Sprint5BeanTeam
{
    public class PlayHUD : IHUD
    {
        private Camera camera;
        private GraphicsDevice graphicsDevice;
        private SpriteFont spriteFont;
        private Color textColor;
        private float currentTime;


        public int times;

        public PlayHUD(Camera camera, GraphicsDevice graphics, SpriteFont spriteFont) 
        {
            this.camera = camera;
            this.graphicsDevice = graphics;
            this.spriteFont = spriteFont;
            textColor = Color.White;
            this.times = 0;
        }

        public void Update(GameTime gameTime)
        {
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void UpdatePlayerStats(int points, int lives, int coins)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // spriteBatch.DrawString(spriteFont, "", new Vector2(camera.Position.X + 20, camera.Position.Y), textColor);
            // spriteBatch.DrawString(spriteFont, "", new Vector2(camera.Position.X + 200, camera.Position.Y - 10), textColor);
            // spriteBatch.DrawString(spriteFont, coin, new Vector2(camera.Position.X + 300, camera.Position.Y - 10), textColor);
            // spriteBatch.DrawString(spriteFont, "World\n   1", new Vector2(camera.Position.X + 500, camera.Position.Y), textColor);
            // spriteBatch.DrawString(spriteFont, time, new Vector2(camera.Position.X + 700, camera.Position.Y), textColor);
        }
    }
}

