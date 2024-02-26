using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GameCamera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#nullable enable
namespace Sprint5BeanTeam
{
    public class RenderingManager
    {
        // CONSTRUCTOR VARIABLES
        public Texture2D PixelTexture { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public RenderTarget2D? RenderTarget { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public Effect PrimitiveEffect { get; set; }
        public Camera Camera { get; set; }
        public Game1 Game { get; }

        // CLASS VARIABLES
        public Dictionary<string, Texture2D> textures;
        private Effect? _currentEffect;
        private BloomPostprocess.BloomComponent _currentBloom;
        private HitboxSystem _hitbox;
        private RenderTarget2D? _renderingObject;

        public RenderingManager(Game1 game, HitboxSystem hitbox, RenderTarget2D renderTarget, Effect primitiveEffect)
        {
            PixelTexture = game.pixel;
            SpriteBatch = game._spriteBatch;
            GraphicsDevice = game.GraphicsDevice;
            RenderTarget = renderTarget;
            PrimitiveEffect = primitiveEffect;
            Camera = game.camera;
            Game = game;
            textures = new Dictionary<string, Texture2D>();
            _renderingObject = null;
            _hitbox = hitbox;
            _currentEffect = null;
            _currentBloom = new BloomPostprocess.BloomComponent(game);
            _currentBloom.Settings = new BloomPostprocess.BloomSettings(null, 0.15f, 2, 2.3f, 1, 1.5f, 1);
            Game.Components.Add(_currentBloom);
        }
        public void Update(GameTime gameTime)
        {
        }
        public void DrawScene(GameTime gameTime) // Draws all objects needed for the game scene
        {

            if (RenderTarget != null)
            {
                int height = 0;
                GraphicsDevice.SetRenderTarget(RenderTarget);
                GraphicsDevice.Clear(Color.Transparent);
                SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, _currentEffect);
                // DRAW RENDER TARGET CODE HERE
                // LANES
                ShapeRenderer.DrawLine(SpriteBatch, PixelTexture, new Vector2(240, 0), new Vector2(560, 0), new Color(255, 255, 255), 2); // horizon line
                ShapeRenderer.DrawLine(SpriteBatch, PixelTexture, new Vector2(0, 480), new Vector2(240, 0), new Color(166, 0, 127)); // right edge
                ShapeRenderer.DrawLine(SpriteBatch, PixelTexture, new Vector2(160, 480), new Vector2(304, 0), new Color(166, 0, 127)); // lane 1-2
                ShapeRenderer.DrawLine(SpriteBatch, PixelTexture, new Vector2(320, 480), new Vector2(368, 0), new Color(166, 0, 127)); // lane 2-3
                ShapeRenderer.DrawLine(SpriteBatch, PixelTexture, new Vector2(480, 480), new Vector2(432, 0), new Color(166, 0, 127)); // lane 3-4
                ShapeRenderer.DrawLine(SpriteBatch, PixelTexture, new Vector2(640, 480), new Vector2(496, 0), new Color(166, 0, 127)); // lane 4-5
                ShapeRenderer.DrawLine(SpriteBatch, PixelTexture, new Vector2(800, 480), new Vector2(560, 0), new Color(166, 0, 127)); // left edge
                for (int i = 0; i < 11; i++) // horizontal  lines
                {
                    height = 1 + (i * 20) + height / 2;
                    ShapeRenderer.DrawLine(SpriteBatch, PixelTexture, new Vector2((480 - height) / 2, height), new Vector2(800 - ((480 - height) / 2), height), new Color(166, 0, 127)); // horizontal lines.
                }
                _hitbox.Draw(SpriteBatch, (BasicEffect)PrimitiveEffect, GraphicsDevice);
                Game.gScene.noteManager.DrawNotes(SpriteBatch);
                SpriteBatch.End();
                _currentBloom.BeginDraw(RenderTarget);
                _renderingObject = _currentBloom.Draw(gameTime, RenderTarget);
                GraphicsDevice.SetRenderTarget(null);
            }


            GraphicsDevice.Clear(new Color(13, 2, 33));
            //DRAW PRIMITIVES HERE
            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, PrimitiveEffect, transformMatrix: Camera.GetViewMatrix(Vector2.One));
            ShapeRenderer.DrawQuadWithGradient((BasicEffect)PrimitiveEffect, GraphicsDevice, new Vector2(240, 0), new Vector2(560, 0), new Vector2(800, 480), new Vector2(0, 480), new Color(36, 23, 52), new Color(16, 3, 32));
            SpriteBatch.End();
            // DRAW SPRITE/RENDER TARGET HERE
            SpriteBatch.Begin();

            SpriteBatch.Draw(_renderingObject, Vector2.Zero, Color.White);

            SpriteBatch.End();
        }
        public void DrawUI(GameTime gameTime) // Draw any UI elements needed for menus, etc.
        {
            if (RenderTarget != null)
            {
                SpriteBatch.Begin();
                GraphicsDevice.SetRenderTarget(RenderTarget);
                // DRAW RENDER TARGET CODE HERE
                GraphicsDevice.SetRenderTarget(null);
                _currentBloom.BeginDraw(RenderTarget);
                _renderingObject = _currentBloom.Draw(gameTime, RenderTarget);
                SpriteBatch.End();
            }
            SpriteBatch.Begin();
            // DRAW UI/RENDER TARGET CODE HERE
            SpriteBatch.End();
        }
        public void drawRenderTarget() // Method should be used only DURING batch drawing.
        {
            SpriteBatch.Draw(_renderingObject, Vector2.Zero, Color.White);
        }
        public void addTextureToRenderer(string key, Texture2D value)
        {
            textures.Add(key, value);
        }
    }
}
