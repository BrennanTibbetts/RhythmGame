using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System;
using GameCamera;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Threading.Tasks.Sources;
using Sprint5BeanTeam.ProgressBar;
using Scene;
using static Sprint5BeanTeam.IState;

namespace Sprint5BeanTeam
{
    public class Game1 : Game
    {
        public IState.GameState CurrentGameState { get; set; }
        private IState.GameState previousGameState;

        // PUBLIC VARIABLES
        public int score;
        public List<IController> controllers;
        public RenderingManager renderingManager;
        public RenderTarget2D render;
        public IScene scene;
        public GamingScene gScene;
        public SoundEffectManager SE;
        public SpriteBatch _spriteBatch;
        public Camera camera;
        public Texture2D pixel;
        public bool transitionBool;
        public bool transitionBack;

        // PRIVATE VARIABLES
        private GraphicsDeviceManager _graphics;
        private SpriteFont _font;
        private SpriteFont _font2;
        private Texture2D _PauseMenu;
        
        private float _elapsedTime;
        private float _value;
        private const float TOTAL_TIME = 10f;
        public double FrameRate = 60d;

        public BasicEffect _effect;
        private Dictionary<int, ICommand> shortPressKeyBinding;
        private Dictionary<int, ICommand> longPressKeyBinding;
        private Dictionary<int, int> conflictKeyBinding;
        public string filename;

        public Game1()
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics = new GraphicsDeviceManager(this);
            this.SE = new SoundEffectManager(Content);
            KeyboardInput.Initialize(this, 500f, 20);
        }

        protected override void Initialize()
        {
            //Key Binding
            this.shortPressKeyBinding = new();
            this.longPressKeyBinding = new();
            this.conflictKeyBinding = new();

            //DEFINE FPS
            // this.IsFixedTimeStep = true;//false;
            // this.FrameRate = 120d;
            // this.TargetElapsedTime = TimeSpan.FromSeconds(1d / FrameRate); //60);

            // GRAPHICS/RENDERING INITIALIZATION
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            DepthStencilState depthStencilState = new DepthStencilState();
            depthStencilState.DepthBufferEnable = true;
            GraphicsDevice.DepthStencilState = depthStencilState;
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });
            camera = new Camera(GraphicsDevice.Viewport);
            camera.Limits = new Rectangle(0, 0, 4000, 480);
            render = new RenderTarget2D(GraphicsDevice, 800, 480, false, SurfaceFormat.Color, DepthFormat.None);

            // SYSTEM/MANAGER INITIALIZATION
            controllers = new List<IController>();
            this.controllers.Add(new KeyboardController(shortPressKeyBinding));
            this.controllers.Add(new GamePadController());
            this.scene = new StartMenuScene(GraphicsDevice, Content, this);

            // EFFECT INITIALIZATION
            _effect = new BasicEffect(GraphicsDevice);
            _effect.Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1);
            _effect.View = Matrix.Identity;
            _effect.World = Matrix.Identity;
            _effect.LightingEnabled = false;


            // UI INIIALIZATION
            CurrentGameState = IState.GameState.StartMenu;
            score = 0;
            filename = "";

            // CONTROLLER INITIALIZATION

            // MISC. INITIALIZATION
            Sprite playerSprite = new Sprite(pixel, 1, 1);

            base.Initialize();
        }


        public void ChangeFile(string file)
        {
            filename = file;
        }

        protected override void LoadContent()
        {
            // FONTS
            _font = Content.Load<SpriteFont>("super-mario-bro");
            _font2 = Content.Load<SpriteFont>("bigsmb");

            _PauseMenu = Content.Load<Texture2D>("NeonPause");

            Timer.SpriteBatch = _spriteBatch;
            Timer.Content = Content;
        }

        protected override void Update(GameTime gameTime)
        {
            Console.WriteLine(gameTime.ElapsedGameTime.TotalMilliseconds);
            KeyboardInput.Update();
            this.previousGameState = this.CurrentGameState;
            this.scene.UpdateScene(gameTime);
            if (this.CurrentGameState != previousGameState)
                switch (CurrentGameState)
                {
                    case IState.GameState.Play:
                        if (previousGameState != IState.GameState.Countdown)
                        {
                            this.gScene = new GamingScene(filename, GraphicsDevice, Content, this, _font, _font2, _PauseMenu, 100f);
                            this.scene = gScene;
                        }
                        break;
                    case IState.GameState.StartMenu:
                        this.scene = new StartMenuScene(GraphicsDevice, Content, this);
                        break;
                    case IState.GameState.SongSelectionMenu:
                        this.scene = new SongSelectionScene(GraphicsDevice, Content, this, camera);
                        break;
                    case IState.GameState.GameOver:
                        if (previousGameState is not IState.GameState.Play)
                        {
                            throw new InvalidOperationException("GRADING SYSTEM WAS CALLED BEFORE PLAY");
                        }
                        else
                        {
                            ScoreSystem score = (this.scene as GamingScene).ScoreSystem;
                            this.scene = new GradingScene(Content.Load<Texture2D>("NeonPause"), this.GraphicsDevice, _font, score, this);
                        }
                        break;

                }
            transition(gameTime);
            base.Update(gameTime);
            Timer.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Console.WriteLine(gameTime.ElapsedGameTime.TotalMilliseconds);
            this.scene.DrawScene(_spriteBatch, gameTime);
            _spriteBatch.Begin();
            _spriteBatch.Draw(pixel, new Rectangle(0, 0, (int)_value, (int)480), new Color(0, 0, 0, 255));
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        // Transition for song selection -> game
        public void setTransitionForward()
        {
            transitionBool = true;
            transitionBack = false;
            _value = 0f;
        }

        private void transition(GameTime gameTime)
        {
            if (transitionBool)
            {
                float normalTime = _elapsedTime / TOTAL_TIME; // Convert time to fraction for use in lerp.
                _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                normalTime *= 6; // Increase speed for normal time. Adjusted during testing for debugging
                _value = MathHelper.Lerp(0, 810, 1.0f - (1.0f - normalTime) * (1.0f - normalTime)); // Quadratic ease-out for transition animation
                if (_elapsedTime > TOTAL_TIME)
                {
                    _elapsedTime = 0;
                }
                if (_value < 0 && transitionBack)
                {
                    _value = 0;
                    transitionBool = false;
                }
                if (_value > 800) // Lerp automatically returns, so set this value to true in order to draw scoring.
                {
                    transitionBack = true;
                }
            }
        }
    }
}