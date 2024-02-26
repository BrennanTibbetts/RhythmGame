using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sprint5BeanTeam
{
    public class GradingScene : IScene
    {
        // CONSTRUCTOR VARIABLES
        private Game1 Game;
        private Texture2D GradingMenuTexture;
        private GraphicsDevice Graphics;
        private SpriteFont Font;
        private ScoreSystem Score;
        public List<IController> controllers;

        // CLASS VARIABLES
        private Texture2D _pixel;
        private Color fadeColor;
        private bool _transition;
        private float _elapsedTime;
        private bool _transitionBack;
        private float _value;

        protected Dictionary<int, ICommand> startMenuShortPressKeyBinding;
        protected Dictionary<int, ICommand> startMenuLongPressKeyBinding;
        protected Dictionary<int, int> startMenuConflictedKeys;

        protected Dictionary<int, ICommand> startMenuShortPressButtonBinding;
        protected Dictionary<int, ICommand> startMenuLongPressButtonBinding;
        protected Dictionary<int, int> startMenuConflictedButtons;

        private const float TOTAL_TIME = 10f;
        public GradingScene(Texture2D gradeMenu, GraphicsDevice graphics, SpriteFont spriteFont, ScoreSystem score, Game1 game)
        {
            GradingMenuTexture = gradeMenu;
            Graphics = graphics;
            Font = spriteFont;
            Game = game;
            Score = score;
            _pixel = new Texture2D(Graphics, 1, 1);
            _pixel.SetData(new Color[] { Color.White });
            _elapsedTime = 0;
            _transitionBack = false;
            _value = 0;
            fadeColor = Color.White;
            this.startMenuShortPressKeyBinding = new();
            this.controllers = game.controllers;
            this.controllers = game.controllers;
            this.startMenuShortPressKeyBinding = new();
            this.startMenuLongPressKeyBinding = new();
            this.startMenuConflictedKeys = new();
            this.startMenuShortPressButtonBinding = new();
            this.startMenuLongPressButtonBinding = new();
            this.startMenuConflictedButtons = new();
            LoadObjects();
            this.Game.SE.FastReset();
            this.setTransitionForward();
        }

        public void ReloadScene()
        {
            this.Game.CurrentGameState = IState.GameState.SongSelectionMenu;
            foreach (IController controller in this.controllers)
            {
                if (controller is KeyboardController)
                {
                    controller.ClearKeyBinding();
                }
                else if (controller is GamePadController)
                {
                    controller.ClearKeyBinding();
                }
            }
        }

        public void LoadObjects()
        {
            this.startMenuShortPressKeyBinding.Add((int)Keys.Space, new RestartCommand(this.Game));
            this.startMenuShortPressButtonBinding.Add((int)Buttons.Back, new RestartCommand(this.Game));
            foreach (IController controller in this.controllers)
            {
                if (controller is KeyboardController)
                {
                    controller.ClearKeyBinding();
                    controller.SetShortPressKeyBinding(this.startMenuShortPressKeyBinding);
                    controller.SetLongPressKeyBinding(this.startMenuLongPressKeyBinding);
                    controller.SetConflictedKeyBinding(this.startMenuConflictedKeys);
                }
                else if (controller is GamePadController)
                {
                    controller.ClearKeyBinding();
                    controller.SetShortPressKeyBinding(this.startMenuShortPressButtonBinding);
                    controller.SetLongPressKeyBinding(this.startMenuLongPressButtonBinding);
                    controller.SetConflictedKeyBinding(this.startMenuConflictedButtons);
                }
            }
        }
        public void setTransitionForward()
        {
            _transition = true;
            _transitionBack = false;
        }
        private void transition(GameTime gameTime)
        {
            if (_transition)
            {
                float normalTime = _elapsedTime / TOTAL_TIME; // Convert time to fraction for use in lerp.
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                normalTime *= 6; // Increase speed for normal time. Adjusted during testing for debugging
                _value = MathHelper.Lerp(0, 810, 1.0f - (1.0f - normalTime) * (1.0f - normalTime)); // Quadratic ease-out for transition animation
                if (_elapsedTime > TOTAL_TIME)
                {
                    _elapsedTime = 0;
                }
                if (_value < 0)
                {
                    _value = 0;
                    _transition = false;
                }
                if (_value > 800) // Lerp automatically returns, so set this value to true in order to draw scoring.
                {
                    _transitionBack = true;
                }
            }
        }
        public void UpdateScene(GameTime gameTime)
        {
            foreach (IController controller in this.controllers) controller.UpdateState();
            if (_transition)
            {
                transition(gameTime);
            }
        }
        public void DrawScene(SpriteBatch batch, GameTime gameTime)
        {
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            if (_transitionBack)
            {
                batch.Draw(GradingMenuTexture, new Rectangle(0, 0, 800, 480), Color.White);
                batch.DrawString(Font, Score.displayFinalScore(), new Vector2(240, 190), Color.White);
                batch.DrawString(Font, "PRESS SPACE TO RETURN TO SONG SELECTION", new Vector2(100, 0), Color.White);
            }
            batch.Draw(_pixel, new Rectangle(0, 0, (int)_value, (int)480), new Color(0, 0, 0, 255));
            batch.End();
        }
    }
}