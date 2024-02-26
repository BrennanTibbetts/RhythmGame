using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameCamera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sprint5BeanTeam;
using System.Linq;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Input;
using MaiLib;
using Sprint5BeanTeam.ProgressBar;
using Sprint5BeanTeam.Latency;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Input.Touch;

public class GamingScene : IScene
{
    public List<IController> controllers;
    public NoteManager noteManager;
    public List<INoteObject> notes;
    private Camera _camera;
    private RenderTarget2D render;
    private RenderingManager renderer;
    private BasicEffect effect;
    private Texture2D pixel;
    private GraphicsDevice Graphics;
    private Game1 Game;
    private HitboxSystem hitbox;
    public ScoreSystem ScoreSystem { get; private set; }

    private int score;
    private int noteCount;
    private int totalHits;
    private float hitPercentage;

    SoundEffectManager soundObserver;
    protected Dictionary<int, ICommand> levelShortPressKeyBinding;
    protected Dictionary<int, ICommand> levelLongPressKeyBinding;
    protected Dictionary<int, int> levelConflictedKeys;

    protected Dictionary<int, ICommand> levelShortPressButtonBinding;
    protected Dictionary<int, ICommand> levelLongPressButtonBinding;
    protected Dictionary<int, int> levelConflictedButtons;

    private SpriteFont spriteFont;
    private SpriteFont spriteFont2;
    public List<Texture2D> _particleTextures;

    private Texture2D pauseMenu;
    public Texture2D _circle, _singlenote, _holdnote, _barnote;
    public List<Texture2D> noteTextures;
    private Vector2 pausePos;
    private float pauseTime;
    private float pauseScale;
    private bool pauseGrow;

    private float countTime;

    private ProgressBars progressBar;
    private ProgressBarAnimated progressBarAnimated;
    private float songLength;
    public int Time;
    private float currentTime;
    private ContentManager content;
    private string file;

    public GamingScene(string filename, GraphicsDevice graphics, ContentManager content, Game1 game, SpriteFont spriteFont, SpriteFont spriteFont2, Texture2D pauseMenu, float songLength)
    {
        file = filename;

        Game = game;
        this.content = content;
        _singlenote = content.Load<Texture2D>("TapNote");
        _holdnote = content.Load<Texture2D>("singlenote");
        _barnote = content.Load<Texture2D>("barnoteFINALFINAL");
        this.noteTextures = new List<Texture2D>();
        this.noteTextures.Add(_singlenote);
        this.noteTextures.Add(_holdnote);
        this.noteTextures.Add(_barnote);
        this.Graphics = graphics;
        effect = new BasicEffect(Graphics);
        DepthStencilState depthStencilState = new DepthStencilState();
        depthStencilState.DepthBufferEnable = true;
        Graphics.DepthStencilState = depthStencilState;
        this.controllers = game.controllers;
        this.spriteFont = spriteFont;
        this.spriteFont2 = spriteFont2;
        _circle = content.Load<Texture2D>("circle"); // Not good practice, but particle system requires this immediately.
        _particleTextures = new List<Texture2D>
            {
                _circle
            };
        hitbox = new HitboxSystem(_particleTextures);


        this.levelShortPressKeyBinding = new();
        this.levelLongPressKeyBinding = new();
        this.levelConflictedKeys = new();

        this.levelShortPressButtonBinding = new();
        this.levelLongPressButtonBinding = new();
        this.levelConflictedButtons = new();
        this.soundObserver = game.SE;
        this.soundObserver.FastReset();
        // Console.WriteLine("The level manager have sound observer {0}", this.soundObserver is not null);
        this.LoadObjects();
        //this.soundObserver.PlayMainTheme();


        this.pauseScale = 0.5f;
        pauseGrow = true;
        this.pauseMenu = pauseMenu;
        countTime = 3.5f;

        score = 0;
        noteCount = 0;
        totalHits = 0;
        hitPercentage = 0;

        this.songLength = songLength;
        progressBar = new ProgressBars(content, new Vector2(200, 50), this.songLength);
        progressBarAnimated = new ProgressBarAnimated(content, new Vector2(0, 0), this.songLength);
        Time = 1000;

        // textRetrieved = "";

        // viewport = new Rectangle(600, 200, 400, 200);
        // textBox = new TextBox(viewport, 1000, "",
        //     graphics, this.spriteFont, Color.LightGray, Color.Red, 30);

        // float margin = 3;
        // textBox.Area = new Rectangle((int)(viewport.X + margin), viewport.Y, (int)(viewport.Width - margin),
        //     viewport.Height);
        // textBox.Renderer.Color = Color.White;
        // textBox.Cursor.Selection = new Color(Color.Purple, .4f);

        // textBox.Active = true;
        //latencySystem = new LatencySystem(content);
    }

    public void UpdateScore(int score, int notes, int totalHits, float hitPercentage)
    {
        this.score = score;
        this.noteCount = notes;
        this.totalHits = totalHits;
        this.hitPercentage = hitPercentage;
    }

    public void LoadObjects()
    {
        // PARTICLE LIST


        render = new RenderTarget2D(this.Game.GraphicsDevice, 800, 480, false, SurfaceFormat.Color, DepthFormat.None);
        renderer = new RenderingManager(Game, hitbox, render, null);
        renderer.PrimitiveEffect = this.Game._effect;

        // Example of loading chart using MaiLib
        SimaiTokenizer tokenizer = new SimaiTokenizer();
        tokenizer.UpdateFromPath(file);
        TrackInformation trackInformation = tokenizer.SimaiTrackInformation;
        string[] simaiToken = tokenizer.ChartCandidates["6"];
        SimaiParser parser = new SimaiParser();
        Chart takingChart = parser.ChartOfToken(simaiToken);
        // // Console.WriteLine(trackInformation.TrackName);
        // // Console.WriteLine(trackInformation.TrackComposer);
        // // Console.WriteLine(trackInformation.TrackID);
        // foreach (String x in trackInformation.TrackLevels)
        // {
        //     // Console.WriteLine(x);
        // }


        notes = new List<INoteObject>();
        _camera = new Camera(this.Graphics.Viewport);
        _camera.Limits = new Rectangle(0, 0, 4000, 480);
        pixel = new Texture2D(Graphics, 1, 1);
        pixel.SetData(new Color[] { Color.White });
        noteManager = new NoteManager(takingChart, noteTextures, hitbox, Game);
        ScoreSystem = new ScoreSystem(hitbox, noteManager, spriteFont);

        this.changeToGamingBinding();

        // if (this.soundObserver is null) throw new NullReferenceException("SOUND OBSERVER IS NULL!");
        // int width = level.tileWidth * level.tileScale;
        // int height = level.tileHeight * level.tileScale;
        // collision.DistributeObjects(hitbox, noteManager.NoteOnTime);
    }

    public void ReloadScene()
    {
        ScoreSystem.ResetSystem();
        this.soundObserver.FastReset();
        LoadObjects();
        //Game.controllers.Add(new KeyboardController(Game, Hitbox));
        //Game.controllers.Add(new GamePadController(Game, Players));
    }

    public void UpdateScene(GameTime gameTime)
    {
        foreach (IController controller in this.controllers) controller.UpdateState();
        if (Game.CurrentGameState == IState.GameState.Play && !Game.transitionBool)
        {
            // if (textRetrieved!=null&&!textRetrieved.Equals(""))
            //     textRetrieved = textBox.number;
            noteManager.Update(gameTime);
            currentTime += (float)Timer.Time;
            progressBar.Update(gameTime, (float)this.noteManager.ProcessOfSong);
            progressBarAnimated.Update(gameTime, (float)this.noteManager.ProcessOfSong * 100);

            if (currentTime >= 1f)
            {
                Time--;
                currentTime -= 1f;
            }
            hitbox.Update(gameTime);
            ScoreSystem.Update(gameTime);
        }
        else if (Game.CurrentGameState == IState.GameState.PauseMenu || Game.CurrentGameState == IState.GameState.Countdown && !Game.transitionBool)
        {
            float textureWidth = pauseMenu.Width * pauseScale;
            float textureHeight = pauseMenu.Height * pauseScale;

            // Calculate the position to center the texture
            pausePos = new Vector2(Graphics.Viewport.Width / 2 - 1200, Graphics.Viewport.Height / 2 - 1200);
            if (pauseTime >= 3)
            {
                pauseGrow = false;
            }
            else if (pauseTime <= 0)
            {

                pauseGrow = true;
            }

            if (pauseGrow)
            {
                pauseTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                pauseTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            pauseScale = 0.5f + pauseTime * 0.1f;

            if (Game.CurrentGameState == IState.GameState.Countdown)
            {
                countTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (countTime <= 0.5)
                {
                    this.Game.CurrentGameState = IState.GameState.Play;
                    this.soundObserver.PlayMusic();
                }
            }
            else
            {
                countTime = 3.5f;
            }

            float lerpAmount = (float)(gameTime.TotalGameTime.TotalMilliseconds % 500f / 500f);
            // textBox.Cursor.Color = Color.Lerp(Color.DarkGray, Color.LightGray, lerpAmount);

            renderer.Update(gameTime);
            // if (this.Game.CurrentGameState!=IState.GameState.Play)textBox.Update();


            //latencySystem.AdjustVideoLatency(Convert.ToInt32(textRetrieved));
            //latencySystem.AdjustAudioLatency(Convert.ToInt32(textRetrieved));
        }
        // else if (Game.CurrentGameState == IState.GameState.GameOver)
        // {
        //     grading.Update(gameTime);
        // }

    }

    public void DrawScene(SpriteBatch spriteBatch, GameTime gameTime)
    {
        switch (Game.CurrentGameState)
        {
            case (IState.GameState.Play):
                if (Game.transitionBack)
                {
                    renderer.DrawScene(gameTime);
                    progressBarAnimated.Draw(spriteBatch);
                    ScoreSystem.Draw(spriteBatch);
                }
                break;
            case (IState.GameState.Countdown):
            case (IState.GameState.PauseMenu):
                spriteBatch.Begin();
                spriteBatch.Draw(texture: pauseMenu, position: pausePos, sourceRectangle: null, color: Color.White, rotation: 0f, origin: Vector2.Zero, scale: new Vector2(pauseScale, pauseScale), effects: SpriteEffects.None, layerDepth: 0f);
                spriteBatch.DrawString(spriteFont, "PAUSED", new Vector2(Graphics.Viewport.Width / 2, Graphics.Viewport.Height / 2 - 200), Color.White);
                spriteBatch.DrawString(spriteFont, "Use Arrow Key to ", new Vector2(Graphics.Viewport.Width / 2 - 300, Graphics.Viewport.Height / 2 - 50), Color.White);
                spriteBatch.DrawString(spriteFont, "change latency (-10 to 10): " + (Math.Round(this.noteManager.AudioOffset* 1000))+"ms", new Vector2(Graphics.Viewport.Width / 2 - 300, Graphics.Viewport.Height / 2 - 20), Color.White);
                spriteBatch.DrawString(spriteFont, "Q to Continue", new Vector2(Graphics.Viewport.Width / 2, Graphics.Viewport.Height / 2 + 100), Color.White);
                spriteBatch.DrawString(spriteFont, "ESC to Quit", new Vector2(Graphics.Viewport.Width / 2, Graphics.Viewport.Height / 2 + 150), Color.White);
                // textBox.Draw(spriteBatch);
                if (Game.CurrentGameState == IState.GameState.Countdown && (countTime % 1 < 0.17))
                {
                    spriteBatch.DrawString(spriteFont2, ((int)countTime).ToString(), new Vector2(Graphics.Viewport.Width / 2 - 150, Graphics.Viewport.Height / 2 - 75), Color.White);
                }

                spriteBatch.End();
                break;
                // case (IState.GameState.GameOver):
                //     grading.Draw(spriteBatch);
                //     break;
        }

    }

    private void changeToPauseBinding()
    {
        this.levelShortPressKeyBinding.Add((int)Keys.Q, new UnpauseCommand(this.Game));
        this.levelShortPressKeyBinding.Add((int)Keys.Escape, new QuitCommand(this.Game));
        this.levelShortPressKeyBinding.Add((int)Keys.Left, new MinusOffsetCommand(this.noteManager));
        this.levelShortPressKeyBinding.Add((int)Keys.Right, new AddOffsetCommand(this.noteManager));

        this.levelShortPressButtonBinding.Add((int)Buttons.Start, new UnpauseCommand(this.Game));
        this.levelShortPressButtonBinding.Add((int)Buttons.BigButton, new QuitCommand(this.Game));
        this.levelShortPressButtonBinding.Add((int)Buttons.DPadLeft, new MinusOffsetCommand(this.noteManager));
        this.levelShortPressButtonBinding.Add((int)Buttons.DPadRight, new MinusOffsetCommand(this.noteManager));

        foreach (IController controller in this.controllers)
        {
            if (controller is KeyboardController)
            {
                controller.ClearKeyBinding();
                controller.SetShortPressKeyBinding(this.levelShortPressKeyBinding);
                controller.SetLongPressKeyBinding(this.levelLongPressKeyBinding);
                controller.SetConflictedKeyBinding(this.levelConflictedKeys);
            }
            else if (controller is GamePadController)
            {
                controller.ClearKeyBinding();
                controller.SetShortPressKeyBinding(this.levelShortPressButtonBinding);
                controller.SetLongPressKeyBinding(this.levelLongPressButtonBinding);
                controller.SetConflictedKeyBinding(this.levelConflictedButtons);
            }
        }
    }

    private void changeToGamingBinding()
    {
        this.levelLongPressKeyBinding.Add((int)Keys.D, new LaneOneCommand(hitbox, noteManager));
        this.levelLongPressKeyBinding.Add((int)Keys.F, new LaneTwoCommand(hitbox, noteManager));
        this.levelLongPressKeyBinding.Add((int)Keys.G, new LaneThreeCommand(hitbox, noteManager));
        this.levelLongPressKeyBinding.Add((int)Keys.H, new LaneThreeCommand(hitbox, noteManager));
        this.levelLongPressKeyBinding.Add((int)Keys.J, new LaneFourCommand(hitbox, noteManager));
        this.levelLongPressKeyBinding.Add((int)Keys.K, new LaneFiveCommand(hitbox, noteManager));
        // this.levelLongPressKeyBinding.Add((int)Keys.C, new BarNoteCommand(hitbox, noteManager));
        // this.levelLongPressKeyBinding.Add((int)Keys.M, new BarNoteCommand(hitbox, noteManager));
        this.levelLongPressKeyBinding.Add((int)Keys.Space, new BarNoteCommand(hitbox, noteManager));

        this.levelLongPressButtonBinding.Add((int)Buttons.DPadLeft, new LaneOneCommand(hitbox, noteManager));
        this.levelLongPressButtonBinding.Add((int)Buttons.DPadUp, new LaneTwoCommand(hitbox, noteManager));
        this.levelLongPressButtonBinding.Add((int)Buttons.DPadRight, new LaneThreeCommand(hitbox, noteManager));
        this.levelLongPressButtonBinding.Add((int)Buttons.Y, new LaneThreeCommand(hitbox, noteManager));
        this.levelLongPressButtonBinding.Add((int)Buttons.X, new LaneFourCommand(hitbox, noteManager));
        this.levelLongPressButtonBinding.Add((int)Buttons.A, new LaneFiveCommand(hitbox, noteManager));
        this.levelLongPressButtonBinding.Add((int)Buttons.LeftShoulder, new BarNoteCommand(hitbox, noteManager));
        this.levelLongPressButtonBinding.Add((int)Buttons.RightShoulder, new BarNoteCommand(hitbox, noteManager));

        this.levelLongPressKeyBinding.Add(-(int)Keys.D, new LaneOneReleaseCommand(hitbox));
        this.levelLongPressKeyBinding.Add(-(int)Keys.F, new LaneTwoReleaseCommand(hitbox));
        this.levelLongPressKeyBinding.Add(-(int)Keys.G, new LaneThreeReleaseCommand(hitbox));
        this.levelLongPressKeyBinding.Add(-(int)Keys.H, new LaneThreeReleaseCommand(hitbox));
        this.levelLongPressKeyBinding.Add(-(int)Keys.J, new LaneFourReleaseCommand(hitbox));
        this.levelLongPressKeyBinding.Add(-(int)Keys.K, new LaneFiveReleaseCommand(hitbox));
        // this.levelLongPressKeyBinding.Add(-(int)Keys.C, new BarNoteReleaseCommand(hitbox));
        // this.levelLongPressKeyBinding.Add(-(int)Keys.M, new BarNoteReleaseCommand(hitbox));
        this.levelLongPressKeyBinding.Add(-(int)Keys.Space, new BarNoteReleaseCommand(hitbox));

        this.levelLongPressButtonBinding.Add(-(int)Buttons.DPadLeft, new LaneOneReleaseCommand(hitbox));
        this.levelLongPressButtonBinding.Add(-(int)Buttons.DPadUp, new LaneTwoReleaseCommand(hitbox));
        this.levelLongPressButtonBinding.Add(-(int)Buttons.DPadRight, new LaneThreeReleaseCommand(hitbox));
        this.levelLongPressButtonBinding.Add(-(int)Buttons.Y, new LaneThreeReleaseCommand(hitbox));
        this.levelLongPressButtonBinding.Add(-(int)Buttons.X, new LaneFourReleaseCommand(hitbox));
        this.levelLongPressButtonBinding.Add(-(int)Buttons.A, new LaneFiveReleaseCommand(hitbox));
        this.levelLongPressButtonBinding.Add(-(int)Buttons.LeftShoulder, new BarNoteReleaseCommand(hitbox));
        this.levelLongPressButtonBinding.Add(-(int)Buttons.RightShoulder, new BarNoteReleaseCommand(hitbox));

        this.levelLongPressKeyBinding.Add(-1, new DefaultCommand());
        this.levelLongPressButtonBinding.Add(-100, new DefaultCommand());

        this.levelShortPressKeyBinding.Add((int)Keys.P, new PauseCommand(this.Game));
        this.levelShortPressKeyBinding.Add((int)Keys.M, new MuteSoundCommand(this.Game.SE));
        this.levelShortPressButtonBinding.Add((int)Buttons.Back, new PauseCommand(this.Game));

        this.levelShortPressKeyBinding.Add((int)Keys.Q, new UnpauseCommand(this.Game));
        this.levelShortPressKeyBinding.Add((int)Keys.Escape, new QuitCommand(this.Game));
        this.levelShortPressKeyBinding.Add((int)Keys.Left, new MinusOffsetCommand(this.noteManager));
        this.levelShortPressKeyBinding.Add((int)Keys.Right, new AddOffsetCommand(this.noteManager));
        this.levelShortPressKeyBinding.Add((int)Keys.Up, new AutoCommand(this.noteManager));

        this.levelShortPressButtonBinding.Add((int)Buttons.Start, new UnpauseCommand(this.Game));
        this.levelShortPressButtonBinding.Add((int)Buttons.BigButton, new QuitCommand(this.Game));
        this.levelShortPressButtonBinding.Add((int)Buttons.DPadLeft, new AddOffsetCommand(this.noteManager));
        this.levelShortPressButtonBinding.Add((int)Buttons.DPadRight, new MinusOffsetCommand(this.noteManager));

        foreach (IController controller in this.controllers)
        {
            if (controller is KeyboardController)
            {
                controller.ClearKeyBinding();
                controller.SetShortPressKeyBinding(this.levelShortPressKeyBinding);
                controller.SetLongPressKeyBinding(this.levelLongPressKeyBinding);
                controller.SetConflictedKeyBinding(this.levelConflictedKeys);
            }
            else if (controller is GamePadController)
            {
                controller.ClearKeyBinding();
                controller.SetShortPressKeyBinding(this.levelShortPressButtonBinding);
                controller.SetLongPressKeyBinding(this.levelLongPressButtonBinding);
                controller.SetConflictedKeyBinding(this.levelConflictedButtons);
            }
        }
    }

}