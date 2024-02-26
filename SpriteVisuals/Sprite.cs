using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static System.Formats.Asn1.AsnWriter;
using static System.Reflection.Metadata.BlobBuilder;

//necessary while using system.drawing
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Sprint5BeanTeam
{
    public class Sprite : ISprite
    {
        public Dictionary<string, Rectangle[]> spriteAnimation { get; set; }
        public bool isInvincible { get; set; }
        private bool colliderOn;
        private Texture2D _Texture;
        private Texture2D CollisionTexture;
        private Rectangle collisionBox;
        private Color boxColor;
        private Color _spriteColor;
        private int _SpritesheetRows;
        private int _SpritesheetColumns;
        private int _timeSinceLastFrame;
        private int _currentFrame;
        private int _totalFrames;
        private Rectangle[] _currentAnimation;

        public Sprite(Texture2D texture, int rows, int columns)
        {
            spriteAnimation = new Dictionary<string, Rectangle[]>();
            _Texture = texture;
            _SpritesheetRows = rows;
            _SpritesheetColumns = columns;
            _currentFrame = 0;
            _currentAnimation = new Rectangle[0];
            _totalFrames = 1;
            _spriteColor = Color.White;
            isInvincible = false;
            colliderOn = false;
            boxColor = Color.White;
            CollisionTexture = new Texture2D(texture.GraphicsDevice, 1, 1);
        }

        public void addAnimation(string name, params int[] animFrames)
        {
            spriteAnimation.Add(name, setFramesOfAnimation(_SpritesheetRows, _SpritesheetColumns, animFrames));
        }

        public void changeCurrentAnimation(string name)
        {
            spriteAnimation.TryGetValue(name, out _currentAnimation);
            _totalFrames = _currentAnimation.Length;
        }

        private Rectangle[] setFramesOfAnimation(int rows, int columns, int[] animFrames)
        {
            Rectangle[] rectangleArray = new Rectangle[animFrames.Length];
            for (int i = 0; i < animFrames.Length; i++)
            {
                int width = _Texture.Width / columns;
                int height = _Texture.Height / rows;
                int row = animFrames[i] / columns;
                int column = animFrames[i] % columns;
                rectangleArray[i] = new Rectangle(width * column, height * row, width, height);
            }
            return rectangleArray;
        }

        public void updateSprite(GameTime gameTime, int speedInMilliseconds)
        {
            if (isInvincible)
            {
                int colorSinWave = (int)Math.Abs((127 * Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 100)) + 128);
                int colorCosWave = (int)Math.Abs((127 * Math.Cos(gameTime.TotalGameTime.TotalMilliseconds / 100)) + 128);
                _spriteColor = new Color(colorSinWave, colorCosWave, colorSinWave * colorCosWave);
            }
            _timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (_timeSinceLastFrame > speedInMilliseconds)
            {
                _timeSinceLastFrame -= speedInMilliseconds;
                _currentFrame++;
            }
            if (_currentFrame == _totalFrames)
            {
                _currentFrame = 0;
            }
        }

        public void drawSprite(SpriteBatch spriteBatch, Vector2 location, bool facingRight, int scale = 1)
        {
            int width = _Texture.Width / _SpritesheetColumns;
            int height = _Texture.Height / _SpritesheetRows;
            int frameCount = _currentAnimation.Length;
            Rectangle sourceRectangle = new Rectangle(0, 0, width, height);
            if (_currentFrame >= _currentAnimation.Length)
            {
                _currentFrame = 0;
            }
            if (_currentAnimation.Length > 0)
            {
                sourceRectangle = _currentAnimation[_currentFrame];
            }
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width * scale, height * scale);

            if (facingRight)
                spriteBatch.Draw(_Texture, destinationRectangle, sourceRectangle, _spriteColor, 0, new Vector2(0, 0), SpriteEffects.FlipHorizontally, 1);
            else
                spriteBatch.Draw(_Texture, destinationRectangle, sourceRectangle, _spriteColor);

            if (colliderOn)
                spriteBatch.Draw(CollisionTexture, collisionBox, boxColor);

        }

        public void updateCollision(Rectangle collisionBox)
        {
            this.collisionBox = collisionBox;
        }

        public void updateCollisionColor(Color color)
        {
            boxColor = new Color((int)color.R, color.G, color.B, 128);
            //CollisionTexture.SetData(new Color[] { color });
        }

        public void showCollision()
        {
            if (colliderOn)
                colliderOn = false;
            else
            {
                colliderOn = true;
            }
        }

        public Sprite Duplicate()
        {
            Sprite result = new Sprite(this._Texture, this._SpritesheetRows, this._SpritesheetColumns);
            result.spriteAnimation = this.spriteAnimation;

            return result;
        }
    }
}