using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint5BeanTeam;

public class HoldNote : INoteObject
{
    // CONSTRUCTOR VARIABLES
    public int Lane { get; set; }
    public Vector2 Position { get { return _position; } }
    private Texture2D Texture;
    private float Speed;
    private float Size;

    // CLASS-SCOPE VARIABLES
    public bool Guided { get; set; }
    private bool _destroyed;
    private float _size;
    public Vector2 _position;
    public Color noteColor;
    private Rectangle _notePrimitive;
    private Vector2 _scale;
    public double Timing { get; set; }
    private bool canDraw;

    public HoldNote(double timing, Texture2D texture, int lane, int speed, float size)
    {
        // CONSTRUCTOR ASSIGNMENT
        Texture = texture;
        Lane = lane;
        Speed = speed;
        Size = size;
        this.Timing = timing;
        this.canDraw = false;

        // CLASS-SCOPE ASSIGNMENT
        _size = 0.20f;
        _notePrimitive = new Rectangle(new Point(0, 0), new Point(4, 2));
        _destroyed = false;
        findLane(Lane);
        _scale = new Vector2(Size * _size, Size * _size);
        noteColor = getNoteColor(Lane);
    }
    public void Update(GameTime gameTime,double chartTime)
    {
        {
            if (chartTime >= this.Timing)
                this.moveNote(this.Lane);
            this.canDraw = chartTime > this.Timing;
        }
    }
    public void Draw(SpriteBatch batch)
    {

        if (this.canDraw && !_destroyed)
        {
            batch.Draw(Texture, _position, null, noteColor, 0f, new Vector2(Texture.Width / 2, Texture.Height / 2), _scale, SpriteEffects.FlipVertically, 0f);
        }
    }
    public bool checkForDestroy()
    {
        if (_position.Y > 480 || _destroyed)
        {
            return true;
        }
        else return false;
    }
    public void destroy()
    {
        _destroyed = true;
    }
    private void findLane(int lane)
    {
        _position.X = 0;
        switch (lane)
        {
            case 1:
                _position.X = 272f;
                break;
            case 2:
                _position.X = 336f;
                break;
            case 3:
                _position.X = 400f;
                break;
            case 4:
                _position.X = 464f;
                break;
            case 5:
                _position.X = 528f;
                break;
            default: break;
        }
        _position.Y = 0;
    }
    private Color getNoteColor(int lane)
    {
        Color c = Color.White;
        switch (lane)
        {
            case 1:
                c = new Color(231, 138, 0, 20);
                break;
            case 2:
                c = new Color(187, 73, 108, 20);
                break;
            case 3:
                c = new Color(138, 0, 231, 20);
                break;
            case 4:
                c = new Color(80, 96, 192, 20);
                break;
            case 5:
                c = new Color(0, 231, 138, 20);
                break;
            default: break;
        }
        return c;
    }
    private void moveNote(int lane)
    {

        switch (lane)
        {
            case 1:
                _position.Y += 1 * Speed;
                _position.X += (float)(-0.4) * Speed;
                break;
            case 2:
                _position.Y += 1 * Speed;
                _position.X += (float)(-0.2) * Speed;
                break;
            case 3:
                _position.Y += 1 * Speed;
                _position.X = 400f - (_scale.X * (_notePrimitive.Width / 2));
                break;
            case 4:
                _position.Y += 1 * Speed;
                _position.X += (float)(0.2) * Speed;
                break;
            case 5:
                _position.Y += 1 * Speed;
                _position.X += (float)(0.4) * Speed;
                break;
            default: break;
        }
        _size = Math.Clamp((0.001875f * _position.Y) + 0.20f, 0.20f, 1f);
        _scale = new Vector2(_size * Size, _size * Size);
    }

}
