using GameCamera;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Sprint5BeanTeam
{
    public class GameHUD
    {
        public Game1 Game;
        private Camera Cameras;
        private SpriteFont spriteFont;
        private Color textColor;
        public Sprite coinSprite;

        private IState.GameState gameState;
        private IHUD currentHUD;
        public StartMenuHUD startMenuHUD;
        public PlayHUD playHUD;
        private GameOverHUD gameOverHUD;
        private PauseHUD pauseHUD;
        public int points, lives, coins, times, timePoints;

        public GameHUD(Game1 game, Camera camera)
        {
            Game = game;
            Cameras = camera;

            this.gameState = IState.GameState.StartMenu;
            spriteFont = Game.Content.Load<SpriteFont>("super-mario-bro");
            textColor = Color.White;


            startMenuHUD = new StartMenuHUD(game.Content.Load<Texture2D>("smb_music"), camera, game.GraphicsDevice, spriteFont);
            playHUD = new PlayHUD(camera, game.GraphicsDevice, spriteFont);
            //player.AttachHudObserver(playHUD);
            gameOverHUD = new GameOverHUD(camera, game.GraphicsDevice, spriteFont);
            pauseHUD = new PauseHUD(game.Content.Load<Texture2D>("smb_pause"), camera, game.GraphicsDevice, spriteFont);
            currentHUD = startMenuHUD;
        }

        public void Update(GameTime gameTime, IState.GameState gameState)
        {
            this.gameState = gameState;
            // Console.WriteLine(gameState);
            switch (gameState)
            {
                case (IState.GameState.StartMenu):
                    currentHUD = startMenuHUD;
                    break;
                case (IState.GameState.Play):
                    // currentHUD = playHUD;
                    currentHUD = playHUD;
                    break;
                case (IState.GameState.GameOver):
                    currentHUD = gameOverHUD;
                    break;
                case (IState.GameState.PauseMenu):
                    currentHUD = pauseHUD;
                    break;
            }
            currentHUD.Update(gameTime);

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            currentHUD.Draw(spriteBatch);
        }
    }
}

