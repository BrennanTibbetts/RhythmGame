using GameCamera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint5BeanTeam.ProgressBar;

namespace Sprint5BeanTeam
{
    public class ProgressBars
    {
        protected readonly Texture2D bar;
        protected readonly Texture2D progress;
        protected readonly Vector2 position;

        private ContentManager contentManager;

        protected readonly float maxValue;
        protected float currentValue;
        protected Rectangle part;

        public ProgressBars(ContentManager content, Vector2 position, float songLength)
        {
            contentManager = content;
            bar = content.Load<Texture2D>("ProgressBar");
            progress = content.Load<Texture2D>("progressStar");

            maxValue = songLength;
            currentValue = 0f;
            part = new Rectangle(0, 0, progress.Width, progress.Height);
            this.position = position;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Timer.SpriteBatch.Draw(bar, position, null, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
            Timer.SpriteBatch.Draw(progress, position, part, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 1f);
            spriteBatch.End();
        }

        public virtual void Update(GameTime gameTime, float value)
        {
            currentValue = value;
            part.Width = (int)(currentValue / maxValue * bar.Width);
        }
    }
}
