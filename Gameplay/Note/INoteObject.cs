using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface INoteObject
{
    public bool Guided { get; set; }
    public double Timing { get; set; }
    public int Lane { get; set; }
    public Vector2 Position { get; }
    public void Update(GameTime gameTime,double chartTime);
    public bool checkForDestroy();
    public void destroy();
    public void Draw(SpriteBatch batch);
}
