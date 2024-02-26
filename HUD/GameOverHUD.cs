using System;
using GameCamera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint5BeanTeam
{
	public class GameOverHUD : IHUD
	{
        private Camera camera;
        private GraphicsDevice graphicsDevice;
        private SpriteFont spriteFont;

        public GameOverHUD(Camera camera, GraphicsDevice graphics, SpriteFont spriteFont)
        {
            this.camera = camera;
            this.graphicsDevice = graphics;
            this.spriteFont = spriteFont;
        }

        public void Update(GameTime gameTime)
        {
            //do nothing
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            graphicsDevice.Clear(Color.Black);
            spriteBatch.DrawString(spriteFont, "GAME OVER!", new Vector2(camera.Position.X + (graphicsDevice.Viewport.Width / 2f) - 100, camera.Position.Y + (graphicsDevice.Viewport.Height / 2f)), Color.White);
            //spriteBatch.Draw(texture: gameOver, new Vector2(camera.Position.X, camera.Position.Y), sourceRectangle: null, color: Color.White, rotation: 0f, origin: Vector2.Zero, scale: new Vector2(1, 1), effects: SpriteEffects.None, layerDepth: 0f);
            spriteBatch.DrawString(spriteFont, "Press [R] to Retry...\n... or [Q] to Exit to Start Menu.", new Vector2(camera.Position.X + (graphicsDevice.Viewport.Width / 2f) - 250, camera.Position.Y + (graphicsDevice.Viewport.Height / 2f) + 100), Color.White);
        }
    }
}
