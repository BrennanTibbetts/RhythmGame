using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


public class ParticleSystem {
    // CONSTRUCTOR VARIABLES
    public List<Vector2> EmissionField { get; set; }

    // CLASS VARIABLES
    private Random _random;
    private List<Particle> _particles;
    private List<Texture2D> _textures;
    public ParticleSystemSettings settings;
    
    public ParticleSystem(List<Texture2D> textures, List<Vector2> emissionField) // Modified ParticleSystem allows for either emission field or point-based particle system
        // (pass in single vector2 for point)
    {
        EmissionField = emissionField;
        _textures = textures;
        _particles = new List<Particle>();
        _random = new Random();

        settings = new ParticleSystemSettings(20, 10, Color.White, ParticleSystemSettings.ColorType.Random, ParticleSystemSettings.VelocityType.Radial, 50, false);
    }
    public void Update(GameTime gameTime)
    {
        int total = settings.ParticleDensity;
        if (!settings.emitManually)
        {
            for (int i = 0; i < total; i++)
            {
                _particles.Add(generateParticle());
            }
        }

        for (int j = 0; j < _particles.Count; j++)
        {
            _particles[j].Update(gameTime);
            if (_particles[j].Lifespan <= 0)
            {
                _particles.RemoveAt(j);
                j--;
            }
        }
    }
    public void Draw(SpriteBatch batch)
    {
        for (int i = 0; i < _particles.Count; i++)
        {
            _particles[i].Draw(batch);
        }
    }
    private Particle generateParticle() // Generate a particle based on user settings
    {
        Texture2D texture = _textures[_random.Next(_textures.Count)];
        Vector2 position = EmissionField[_random.Next(EmissionField.Count)];
        Vector2 velocity = new Vector2();

        switch (settings.Velocity)
        {
            case ParticleSystemSettings.VelocityType.Radial:
                velocity = new Vector2(1f * (float)(_random.NextDouble() * 2 - 1), 1f * (float)(_random.NextDouble() * 2 - 1));
                break;
            case ParticleSystemSettings.VelocityType.Up:
                velocity = new Vector2(1f * (float)(_random.NextDouble() * 2 - 1), -1f);
                break;
            case ParticleSystemSettings.VelocityType.Down:
                velocity = new Vector2(1f * (float)(_random.NextDouble() * 2 - 1), 1f);
                break;
            case ParticleSystemSettings.VelocityType.Left:
                velocity = new Vector2(-1f, 1f * (float)(_random.NextDouble() * 2 - 1));
                break;
            case ParticleSystemSettings.VelocityType.Right:
                velocity = new Vector2(1f, 1f * (float)(_random.NextDouble() * 2 - 1));
                break;
            default: break;
        }

        Color color = new Color();
        float angle = 0;
        float angularVelocity = 0.1f * (float) (_random.NextDouble() * 2 - 1);
        
        switch (settings.Color)
        {
            case ParticleSystemSettings.ColorType.Solid:
                color = settings.ParticleColor;
                color.A = (byte)settings.Transparency;
                break;
            case ParticleSystemSettings.ColorType.Spectrum:
                if (settings.ParticleColor.R > 0) color.R = (byte) (settings.ParticleColor.R + _random.Next(0, 20));
                if (settings.ParticleColor.G > 0) color.G = (byte)(settings.ParticleColor.G + _random.Next(0, 20));
                if (settings.ParticleColor.R > 0) color.B = (byte)(settings.ParticleColor.B + _random.Next(0, 20));
                color.A = (byte)settings.Transparency;
                break;
            case ParticleSystemSettings.ColorType.Random:
                color = new Color((float)(_random.NextDouble()), (float)(_random.NextDouble()), (float)(_random.NextDouble()));
                break;
            default: break;
        }
        
        float size = (float) _random.NextDouble();
        int lifespan = settings.Lifespan + _random.Next(settings.Lifespan * 2);
        return new Particle(texture, position, velocity, angle, angularVelocity, color, size, lifespan);
    }
    public void manualGeneration()
    {
        int total = settings.ParticleDensity;
        for (int i = 0; i < total; i++)
        {
            _particles.Add(generateParticle());
        }
    }
}