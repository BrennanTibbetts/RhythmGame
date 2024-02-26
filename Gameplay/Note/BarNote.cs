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

public class BarNote : INoteObject
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

    public BarNote(double timing, Texture2D texture, int speed, float size)
    {
        // CONSTRUCTOR ASSIGNMENT
        Texture = texture;
        Speed = speed;
        Size = size;
        Lane = 6;
        this.Timing = timing;
        this.canDraw = false;

        // CLASS-SCOPE ASSIGNMENT
        _size = 0.20f;
        _notePrimitive = new Rectangle(new Point(0, 0), new Point(4, 2));
        _destroyed = false;
        _scale = new Vector2(Size * _size, Size * _size);
        noteColor = Color.White;
        _position.X = 290;
    }
    public void Update(GameTime gameTime,double chartTime)
    {
        {
            if (chartTime >= this.Timing)
                this.moveNote();
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
    private void moveNote()
    {
        _position.Y += 1 * Speed;
        _position.X = 400f - (_scale.X * (_notePrimitive.Width / 2));
        _size = Math.Clamp((0.001875f * _position.Y) + 0.20f, 0.20f, 1f);
        _scale = new Vector2(_size * Size, _size * Size);
    }

}
