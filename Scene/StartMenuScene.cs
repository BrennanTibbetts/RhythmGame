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

public class StartMenuScene : IScene
{
    public List<IController> controllers;
    private Camera _camera;
    private BasicEffect effect;
    private Texture2D pixel;
    GraphicsDevice Graphics;
    Game1 Game;
    private Vector2 startPos;
    private float startTime;
    private float startScale;
    private bool startGrow;
    private Texture2D background;
    private Texture2D TitleCard;
    private Texture2D startMenuSprite;
    private SpriteFont spriteFont;
    private Sprite backgroundSprite;
    private ContentManager content;

    SoundEffectManager soundObserver;
    protected Dictionary<int, ICommand> startMenuShortPressKeyBinding;
    protected Dictionary<int, ICommand> startMenuLongPressKeyBinding;
    protected Dictionary<int, int> startMenuConflictedKeys;

    protected Dictionary<int, ICommand> startMenuShortPressButtonBinding;
    protected Dictionary<int, ICommand> startMenuLongPressButtonBinding;
    protected Dictionary<int, int> startMenuConflictedButtons;

    public StartMenuScene(GraphicsDevice graphics, ContentManager content, Game1 game)
    {
        this.startScale = 0.1f;
        startGrow = true;
        Game = game;
        this.controllers = game.controllers;
        this.Graphics = graphics;
        this.content = content;
        effect = new BasicEffect(Graphics);
        DepthStencilState depthStencilState = new DepthStencilState();
        depthStencilState.DepthBufferEnable = true;
        Graphics.DepthStencilState = depthStencilState;
        this.controllers = game.controllers;
        this.startMenuShortPressKeyBinding = new();
        this.startMenuLongPressKeyBinding = new();
        this.startMenuConflictedKeys = new();
        this.startMenuShortPressButtonBinding = new();
        this.startMenuLongPressButtonBinding = new();
        this.startMenuConflictedButtons = new();
        this.soundObserver = game.SE;
        this.soundObserver.FastReset();
        // Console.WriteLine("The level manager have sound observer {0}", this.soundObserver is not null);
        this.LoadObjects();
        //this.soundObserver.PlayMainTheme();

    }

    public void LoadObjects()
    {
        _camera = new Camera(this.Graphics.Viewport);
        _camera.Limits = new Rectangle(0, 0, 4000, 480);
        pixel = new Texture2D(Graphics, 1, 1);
        pixel.SetData(new Color[] { Color.White });
        this.startMenuSprite = this.content.Load<Texture2D>("bowzaNeo");
        this.background = this.content.Load<Texture2D>("TitleBackground");
        this.TitleCard = this.content.Load<Texture2D>("BEANJAM");
        this.backgroundSprite = new Sprite(background, 13, 7);
        backgroundSprite.addAnimation("background", 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 87, 86, 85, 84, 83, 82, 81, 80, 79, 78, 77, 76, 75, 74, 73, 72, 71, 70, 69, 68, 67, 66, 65, 64, 63, 62, 61, 60, 59, 58, 57, 56, 55, 54, 53, 52, 51, 50, 49, 48, 47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32, 31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1);
        this.backgroundSprite.changeCurrentAnimation("background");
        this.spriteFont = this.content.Load<SpriteFont>("super-mario-bro");

        this.startMenuShortPressKeyBinding.Add((int)Keys.Space, new RestartCommand(this.Game));
        this.startMenuShortPressKeyBinding.Add((int)Keys.Escape, new QuitCommand(this.Game));

        this.startMenuShortPressKeyBinding.Add((int)Buttons.Start, new UnpauseCommand(this.Game));
        this.startMenuShortPressKeyBinding.Add((int)Buttons.BigButton, new QuitCommand(this.Game));
        // this.startMenuShortPressKeyBinding.Add((int)Keys.P, new PauseCommand(this.Game));
        foreach (IController controller in this.controllers)
        {
            if (controller is KeyboardController)
            {
                controller.SetShortPressKeyBinding(this.startMenuShortPressKeyBinding);
                controller.SetLongPressKeyBinding(this.startMenuLongPressKeyBinding);
                controller.SetConflictedKeyBinding(this.startMenuConflictedKeys);
            }
            else if (controller is GamePadController)
            {
                controller.SetShortPressKeyBinding(this.startMenuShortPressButtonBinding);
                controller.SetLongPressKeyBinding(this.startMenuLongPressButtonBinding);
                controller.SetConflictedKeyBinding(this.startMenuConflictedButtons);
            }
        }
        // if (this.soundObserver is null) throw new NullReferenceException("SOUND OBSERVER IS NULL!");
        // int width = level.tileWidth * level.tileScale;
        // int height = level.tileHeight * level.tileScale;
    }

    public void ReloadScene()
    {
        // _loading = true;
        // this.soundObserver.FastReset();
        // //Game.controllers.Add(new KeyboardController(Game, Hitbox));
        // //Game.controllers.Add(new GamePadController(Game, Players));
        //this.startMenuShortPressKeyBinding.Clear();
        //this.startMenuLongPressKeyBinding.Clear();
        //this.startMenuConflictedKeys.Clear();
        // this.LoadObjects();
        // _loading = false;
        this.Game.CurrentGameState = IState.GameState.Play;
        foreach (IController controller in this.controllers)
        {
            controller.ClearKeyBinding();
        }
    }

    public void UpdateScene(GameTime gameTime)
    {
        foreach (IController controller in this.controllers) controller.UpdateState();
        float textureWidth = this.startMenuSprite.Width * startScale;
        float textureHeight = this.startMenuSprite.Height * startScale;

        // Calculate the position to center the texture
        startPos = new Vector2(_camera.Position.X + 800 / 2, _camera.Position.Y + 600 / 2) - new Vector2(textureWidth / 2, textureHeight / 2);
        if (startTime >= 3)
        {
            startGrow = false;
        }
        else if (startTime <= 0)
        {

            startGrow = true;
        }

        if (startGrow)
        {
            startTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            startTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        startScale = 0.3f + startTime * 0.02f;
        backgroundSprite.updateSprite(gameTime, 100);
    }

    public void DrawScene(SpriteBatch spriteBatch, GameTime gameTime)
    {

        spriteBatch.Begin();
        backgroundSprite.drawSprite(spriteBatch, new Vector2(0, 10), false, 1);
        spriteBatch.Draw(texture: this.startMenuSprite, position: startPos, sourceRectangle: null, color: Color.White, rotation: 0f, origin: Vector2.Zero, scale: new Vector2(startScale, startScale), effects: SpriteEffects.None, layerDepth: 0f);
        // Draw rectangle at the top of the screen
        Texture2D pixelTexture = new Texture2D(Graphics, 1, 1);
        pixelTexture.SetData(new[] { Color.White });
        spriteBatch.Draw(pixelTexture, new Rectangle(0, 0, Graphics.Viewport.Width, 30), Color.HotPink);
        spriteBatch.Draw(TitleCard, new Rectangle(0, 30, Graphics.Viewport.Width, 90), Color.White);
        spriteBatch.DrawString(spriteFont, "Press SPACE to START", new Vector2(_camera.Position.X - 165 + (this.Graphics.Viewport.Width / 2f), _camera.Position.Y + (this.Graphics.Viewport.Height / 2f) - 235), Color.White);

        spriteBatch.End();
    }
}