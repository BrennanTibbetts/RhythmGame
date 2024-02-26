using System;
using GameCamera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint5BeanTeam
{
	public class StartMenuHUD : IHUD
	{
        private Vector2 startPos;
        private float startTime;
        private float startScale;
        private bool startGrow;
        private Texture2D startMenu;
        private Camera camera;
        private GraphicsDevice graphicsDevice;
        private SpriteFont spriteFont;
        private Texture2D mario;
        private Sprite marioSprite;

        public StartMenuHUD(Texture2D startMenu, Camera camera, GraphicsDevice graphics, SpriteFont spriteFont)
        {
            this.startMenu = startMenu;
            this.camera = camera;
            this.graphicsDevice = graphics;
            this.spriteFont = spriteFont;
            this.startScale = 3f;
            startGrow = true;
        }

        public void Update(GameTime gameTime)
        {
            float textureWidth = startMenu.Width * startScale;
            float textureHeight = startMenu.Height * startScale;

            // Calculate the position to center the texture
            startPos = new Vector2(camera.Position.X + 800 / 2, camera.Position.Y + 480 / 2) - new Vector2(textureWidth / 2, textureHeight / 2);
            if (startTime >= 3)
            {
                startGrow = false;
            }
            else if (startTime <= 0)
            {

                startGrow = true;
            }

            if (startGrow)
            {
                startTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                startTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            startScale = 3 + startTime * 0.1f;
            marioSprite.updateSprite(gameTime, 100);
        }

        public void giveAnimation(Texture2D mario)
        {
            this.mario = mario;
            marioSprite = new Sprite(mario, 8, 7);
            marioSprite.addAnimation("marioRunSmall", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52);
            marioSprite.changeCurrentAnimation("marioRunSmall");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            graphicsDevice.Clear(Color.CadetBlue);
            spriteBatch.Draw(texture: startMenu, position: startPos, sourceRectangle: null, color: Color.White, rotation: 0f, origin: Vector2.Zero, scale: new Vector2(startScale, startScale), effects: SpriteEffects.None, layerDepth: 0f);
            spriteBatch.DrawString(spriteFont, "Press [R] to START", new Vector2(camera.Position.X - 100 + (graphicsDevice.Viewport.Width / 2f), camera.Position.Y + (graphicsDevice.Viewport.Height / 2f) + 200), Color.Black);
            marioSprite.drawSprite(spriteBatch, new Vector2(camera.Position.X - 500 + (graphicsDevice.Viewport.Width / 2f), camera.Position.Y + (graphicsDevice.Viewport.Height / 2f) - 300), false, 5);
        }
    }
}

