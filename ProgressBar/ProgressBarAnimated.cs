using GameCamera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint5BeanTeam.ProgressBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint5BeanTeam
{
    public class ProgressBarAnimated : ProgressBars
    {
        private float _targetValue;
        private readonly float _animationSpeed;
        private Rectangle _animationPart;
        private Vector2 _animationPosition;
        private Color _animationShade;
        private float songLength;
        SpriteFont font;

        public ProgressBarAnimated(ContentManager content, Vector2 pos, float songLength) : base(content, pos, songLength)
        {
            _targetValue = maxValue;
            _animationPart = new(progress.Width, 0, 0, progress.Height);
            _animationPosition = pos;
            _animationShade = Color.DarkGray;
            this.songLength = songLength;
            _animationSpeed = 1f;
            font = content.Load<SpriteFont>("super-mario-bro");
        }

        public override void Update(GameTime gameTime ,float value)
        {
            if (value == currentValue) return;

            _targetValue = value;
            int x;

                if (_targetValue >= currentValue && Timer.Time < songLength)
            {
                currentValue += _animationSpeed * Timer.Time;
                x = (int)(currentValue / maxValue * progress.Width);
                _animationShade = Color.DarkGray * 0.5f;
                if(part.Width <= progress.Width) 
                part.Width = x;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Begin();
            Timer.SpriteBatch.Draw(progress, _animationPosition, _animationPart, _animationShade, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            spriteBatch.End();
        }
    }
}
