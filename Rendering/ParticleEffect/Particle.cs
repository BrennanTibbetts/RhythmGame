using System;
using System.Threading.Tasks.Dataflow;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Particle
{
    public Texture2D ParticleTexture { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Angle { get; set; }
    public float AngularVelocity { get; set; }
    public Color Color { get; set; }
    public float Size { get; set; }
    public int Lifespan { get; set; }
    public Particle (Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angleVelocity, Color color, float size, int lifespan)
    {
        ParticleTexture = texture;
        Position = position;
        Velocity = velocity;
        Angle = angle;
        AngularVelocity = angleVelocity;
        Color = color;
        Size = size;
        Lifespan = lifespan;
    }
    public void Update(GameTime gameTime)
    {
        Lifespan--;
        Position += Velocity;
        Angle += AngularVelocity;
    }
    public void Draw(SpriteBatch batch)
    {
        Rectangle sourceRectangle = new Rectangle(0, 0, ParticleTexture.Width, ParticleTexture.Height);
        Vector2 origin = new Vector2(ParticleTexture.Width / 2, ParticleTexture.Height / 2);
        batch.Draw(ParticleTexture, Position, sourceRectangle, Color, Angle, origin, Size, SpriteEffects.None, 0f);
    }
}