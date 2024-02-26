using System;
using GameCamera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint5BeanTeam.Latency;

namespace Sprint5BeanTeam
{
	public class PauseHUD : IHUD
	{
        private Texture2D pauseMenu;
        private Camera camera;
        private GraphicsDevice graphicsDevice;
        private SpriteFont spriteFont;
        private Vector2 pausePos;
        private float pauseTime;
        private float pauseScale;
        private bool pauseGrow;
        private TextBox textBox;
        private Rectangle viewport;
        public string textRetrieved;

        public PauseHUD(Texture2D pauseMenu, Camera camera, GraphicsDevice graphics, SpriteFont spriteFont)
        {
            this.camera = camera;
            this.graphicsDevice = graphics;
            this.spriteFont = spriteFont;
            this.pauseScale = 3f;
            pauseGrow = true;
            this.pauseMenu = pauseMenu;
            textRetrieved = "";

            viewport = new Rectangle(50, 50, 400, 200);
            textBox = new TextBox(viewport, 3, "This is a test. Move the cursor, select, delete, write...",
                graphics, this.spriteFont, Color.LightGray, Color.DarkGreen, 30);

            float margin = 3;
            textBox.Area = new Rectangle((int)(viewport.X + margin), viewport.Y, (int)(viewport.Width - margin),
                viewport.Height);
            textBox.Renderer.Color = Color.White;
            textBox.Cursor.Selection = new Color(Color.Purple, .4f);

            textBox.Active = true;
        }

        public void Update(GameTime gameTime)
        {

            float textureWidth = pauseMenu.Width * pauseScale;
            float textureHeight = pauseMenu.Height * pauseScale;

            // Calculate the position to center the texture
            pausePos = new Vector2(camera.Position.X + 800 / 2, camera.Position.Y + 200 / 2) - new Vector2(textureWidth / 2, textureHeight / 2);
            if (pauseTime >= 3)
            {
                pauseGrow = false;
            }
            else if (pauseTime <= 0)
            {

                pauseGrow = true;
            }

            if (pauseGrow)
            {
                pauseTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                pauseTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            pauseScale = 3 + pauseTime * 0.1f;

            float lerpAmount = (float)(gameTime.TotalGameTime.TotalMilliseconds % 500f / 500f);
            textBox.Cursor.Color = Color.Lerp(Color.DarkGray, Color.LightGray, lerpAmount);

            textBox.Update();
            textRetrieved = textBox.Text.ToString();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: pauseMenu, position: pausePos, sourceRectangle: null, color: Color.White, rotation: 0f, origin: Vector2.Zero, scale: new Vector2(pauseScale, pauseScale), effects: SpriteEffects.None, layerDepth: 0f);
            spriteBatch.DrawString(spriteFont, "[P] to Continue", new Vector2(camera.Position.X - 180 + (graphicsDevice.Viewport.Width / 2f), camera.Position.Y + (graphicsDevice.Viewport.Height / 2f) -100), Color.Black);
            spriteBatch.DrawString(spriteFont, "[Q] to Exit", new Vector2(camera.Position.X - 180 + (graphicsDevice.Viewport.Width / 2f), camera.Position.Y + (graphicsDevice.Viewport.Height / 2f) - 70), Color.Black);
            textBox.Draw(spriteBatch);
        }
    }
}

