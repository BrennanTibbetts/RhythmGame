using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sprint5BeanTeam.IState;
using MaiLib;
using System.IO;
using System.Reflection;
using System.Collections.Immutable;
using GameCamera;
using Sprint5BeanTeam;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Scene
{
    public class SongSelectionScene : IScene
    {
        public List<IController> controllers;
        private SimaiTokenizer tokenizer = new SimaiTokenizer();
        private List<Component> _components;
        private SpriteFont buttonFont;
        private ContentManager contentManager;

        string[] trackInformation = new string[10];
        private Texture2D buttonTexture;
        private Texture2D selectionTexture;

        private Texture2D songMenu;
        private Sprite backSprite;
        private Vector2 startPos;
        private int movement;
        private int firstSong;
        private int lastSong;
        private Game1 Game;
        protected Dictionary<int, ICommand> songSelectionShortPressKeyBinding = new();
        protected Dictionary<int, ICommand> songSelectionLongPressKeyBinding = new();
        protected Dictionary<int, int> songSelectionConflictedKeys = new();

        protected Dictionary<int, ICommand> songSelectionShortPressButtonBinding = new();
        protected Dictionary<int, ICommand> songSelectionLongPressButtonBinding = new();
        protected Dictionary<int, int> songSelectionConflictedButton = new();

        private Rectangle selection;
        private string[] songs;
        public string songChosen;
        NotifyObserver observer;
        GraphicsDevice graphics;
        public SongSelectionScene(GraphicsDevice graphics, ContentManager content, Game1 game, Camera camera)
        {
            this.graphics = graphics;
            contentManager = content;
            startPos = new Vector2(camera.Position.X, camera.Position.Y);
            buttonTexture = contentManager.Load<Texture2D>("Button");
            songMenu = contentManager.Load<Texture2D>("TitleBackground");
            backSprite = new Sprite(songMenu, 13, 7);
            backSprite.addAnimation("background", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 87, 86, 85, 84, 83, 82, 81, 80, 79, 78, 77, 76, 75, 74, 73, 72, 71, 70, 69, 68, 67, 66, 65, 64, 63, 62, 61, 60, 59, 58, 57, 56, 55, 54, 53, 52, 51, 50, 49, 48, 47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32, 31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1);
            backSprite.changeCurrentAnimation("background");
            selectionTexture = contentManager.Load<Texture2D>("Selection");
            buttonFont = contentManager.Load<SpriteFont>("super-mario-bro");

            firstSong = 200;
            selection = new Rectangle(300, firstSong, buttonTexture.Width, buttonTexture.Height);
            Game = game;
            this.observer = new NotifyObserver(Game.SE.Update);

            controllers = game.controllers;
            songChosen = "";

            songs = Directory.GetFiles("Chart", "*.*", SearchOption.AllDirectories);
            string[] tokens = new string[10];
            movement = 50;

            int i = 0;
            foreach (string line in songs)
            {
                tokenizer = new SimaiTokenizer();
                tokenizer.UpdateFromPath(line);
                TrackInformation TrackInformation = tokenizer.SimaiTrackInformation;
                string name = TrackInformation.TrackName;
                trackInformation[i] = name;
                tokens = tokenizer.Tokens(line);
                i++;
            }
            lastSong = 350;

            //Create Visual Buttons for each song available to play
            #region Buttons
            var newGameButton1 = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, firstSong),
                Text = trackInformation[0],
            };

            var newGameButton2 = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, firstSong + 50),
                Text = trackInformation[1],
            };

            var newGameButton3 = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, firstSong + 100),
                Text = trackInformation[2],
            };

            var newGameButton4 = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, lastSong),
                Text = trackInformation[3],
            };

            _components = new List<Component>()
      {
        newGameButton1,
        newGameButton2,
        newGameButton3,
        newGameButton4,
        };
            #endregion
            LoadObjects();
        }

        public void LoadObjects()
        {
            songSelectionShortPressKeyBinding.Add((int)Keys.W, new UpSelectionCommand(this));
            songSelectionShortPressKeyBinding.Add((int)Keys.A, new DownSelectionCommand(this));
            songSelectionShortPressKeyBinding.Add((int)Keys.Enter, new SelectSongCommand(Game));
            songSelectionShortPressKeyBinding.Add((int)Keys.Escape, new EscapeCommand(Game));

            songSelectionShortPressButtonBinding.Add((int)Buttons.DPadUp, new UpSelectionCommand(this));
            songSelectionShortPressButtonBinding.Add((int)Buttons.DPadDown, new DownSelectionCommand(this));
            songSelectionShortPressButtonBinding.Add((int)Buttons.A, new SelectSongCommand(Game));
            songSelectionShortPressButtonBinding.Add((int)Buttons.BigButton, new EscapeCommand(Game));

            foreach (IController controller in controllers)
            {
                if (controller is KeyboardController)
                {
                    controller.SetShortPressKeyBinding(songSelectionShortPressKeyBinding);
                    controller.SetLongPressKeyBinding(songSelectionLongPressKeyBinding);
                    controller.SetConflictedKeyBinding(songSelectionConflictedKeys);
                }
                else if (controller is GamePadController)
                {
                    controller.SetShortPressKeyBinding(songSelectionShortPressButtonBinding);
                    controller.SetLongPressKeyBinding(songSelectionLongPressButtonBinding);
                    controller.SetConflictedKeyBinding(songSelectionConflictedButton);
                }
            }
        }

        public void MoveUp()
        {
            if (selection.Top == 200)
            {
                firstSong = 350;
                selection = new Rectangle(300, firstSong, buttonTexture.Width, buttonTexture.Height);
            }
            else
            {
                firstSong = firstSong - movement;
                selection = new Rectangle(300, firstSong, buttonTexture.Width, buttonTexture.Height);
            }

        }

        public void MoveDown()
        {

            if (selection.Top == 350)
            {
                firstSong = 200;
                selection = new Rectangle(300, firstSong, buttonTexture.Width, buttonTexture.Height);
            }
            else
            {
                firstSong = firstSong + movement;
                selection = new Rectangle(300, firstSong, buttonTexture.Width, buttonTexture.Height);
            }
        }

        public void ReloadScene()
        {
            string name = SelectSong();
            this.Game.ChangeFile(name);
            Game.setTransitionForward();
            Game.CurrentGameState = GameState.Play;
            foreach (IController controller in controllers)
            {
                controller.ClearKeyBinding();
            }
        }

        public string SelectSong()
        {
            switch (selection.Top)
            {
                case 200:
                    songChosen = songs[0];
                    break;
                case 250:
                    songChosen = songs[1];
                    break;
                case 300:
                    songChosen = songs[2];
                    break;
                case 350:
                    songChosen = songs[3];
                    break;
                default:
                    return null;
            }
            if (songChosen.Contains("001"))
            {
                this.observer.Invoke("select_bbc");
            }
            else if (songChosen.Contains("002"))
            {
                this.observer.Invoke("select_lrs");
            }
            else if (songChosen.Contains("003"))
            {
                this.observer.Invoke("select_cld");
            }
            else if (songChosen.Contains("004"))
            {
                this.observer.Invoke("select_ukt");
            }
            return songChosen;
        }

        public void UpdateScene(GameTime gameTime)
        {
            foreach (IController controller in controllers) controller.UpdateState();
            foreach (var component in _components)
                component.Update(gameTime);
            backSprite.updateSprite(gameTime, 100);
        }

        public void DrawScene(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
                backSprite.drawSprite(spriteBatch, new Vector2(0, 10), false, 1);
                spriteBatch.DrawString(buttonFont, "Choose a song to play \n Move up with W and down with A", new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - 300, Game.GraphicsDevice.Viewport.Height / 2 - 200), Color.White);
                foreach (var component in _components)
                {
                    component.Draw(spriteBatch);
                }
                spriteBatch.Draw(selectionTexture, selection, Color.Gray);
            spriteBatch.End();
        }

        private void NewGameButton(object sender, EventArgs e)
        {

        }
        
        
    }
}
