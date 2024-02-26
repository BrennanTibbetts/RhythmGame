using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.ComponentModel;

namespace Sprint5BeanTeam
{
    public class Button : Component
    {
        private SpriteFont _font;
        private Texture2D _texture;
        public Color color { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 textOffSet { get; set; } = Vector2.Zero;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }
        public string Text { get; set; }

        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            color = Color.White;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var colors = Color.White;

            spriteBatch.Draw(_texture, Rectangle, colors);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = Rectangle.X + Rectangle.Width / 2 - _font.MeasureString(Text).X / 2 + textOffSet.X;
                var y = Rectangle.Y + Rectangle.Height / 2 - _font.MeasureString(Text).Y / 2 + textOffSet.Y;

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), color);
            }
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
