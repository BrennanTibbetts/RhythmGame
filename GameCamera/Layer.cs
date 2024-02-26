using GameCamera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint5BeanTeam;
using System;
using System.Collections.Generic;

namespace Sprint5BeanTeam
{
    public class Layer
    {
        public Layer(Camera camera)
        {
            _camera = camera;
            Parallax = Vector2.One;
            Sprites = new List<Background>();
        }

        public Vector2 Parallax { get; set; }
        public List<Background> Sprites { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, _camera.GetViewMatrix(Parallax));
            foreach (Background sprite in Sprites)
                sprite.Draw(spriteBatch, _camera.Limits);
            spriteBatch.End();
        }

        private readonly Camera _camera;
    }




    public struct Background
    {
        public Texture2D Texture;
        public Vector2 Position;

        public void Draw(SpriteBatch spriteBatch, Rectangle? limits)
        {
            if (Texture != null)
            {
                int textureWidth = Texture.Width;
                int numberOfTiles = limits.Value.Width / textureWidth;

                for (int i = 0; i < numberOfTiles; i++)
                {
                    Vector2 position = new Vector2(Position.X + i * textureWidth, Position.Y);
                    spriteBatch.Draw(Texture, position, Color.White);
                }
            }
        }
    }
}


