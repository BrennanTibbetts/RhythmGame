using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class ParticleSystemSettings
{
    /// <summary>
    /// Lifespan of each particle, measured in frame updates
    /// </summary>
    public int Lifespan;

    /// <summary>
    /// Density of particle field/point
    /// </summary>
    public int ParticleDensity;

    /// <summary>
    /// Color of particles. Applies to Solid or Spectrum color types.
    /// </summary>
    public Color ParticleColor;

    /// <summary>
    /// The color type assigned to the particles.
    /// SOLID - A single color will be applied to the particle
    /// RANDOM - Any color will be applied to the particle
    /// SPECTRUM - A particle will be assigned similar colors to the one chosen (for example, if you pick orange, system may pick light orange, dark orange, etc.)
    /// </summary>
    public enum ColorType
    {
        Solid, Random, Spectrum
    }
    /// <summary>
    /// The direction particles will travel
    /// RADIAL - All directions in a 360 degree circle
    /// UP, DOWN, LEFT, RIGHT - Determined by respective direction
    /// </summary>
    public enum VelocityType
    {
        Radial, Up, Down, Left, Right
    }
    /// <summary>
    /// Decides whether or not to automatically generate new particles in Update.
    /// </summary>
    public bool emitManually;

    /// <summary>
    /// Transparency of particles. Acceptable values are 0 - 100.
    /// </summary>
    public  int Transparency;

    public ColorType Color;
    public VelocityType Velocity;

    public ParticleSystemSettings(int lifespan, int density, Color color, ColorType colorType, VelocityType velocityType, int transparency, bool manualEmission)
    {
        Lifespan = lifespan;
        ParticleDensity = density;
        ParticleColor = color;
        Color = colorType;
        Velocity = velocityType;
        Transparency = transparency;
        emitManually = manualEmission;
    }
}