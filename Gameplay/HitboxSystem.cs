using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint5BeanTeam;

public class HitboxSystem
{
    // CONSTRUCTOR VARIABLES
    public ParticleSystem[] ParticleSystems { get; set; }
    // CLASS-SCOPE VARIABLES
    private Rectangle[] _laneCollisionBoxes;
    private bool[] _controllerCalled;
    public bool[] canActivate;
    public bool[] canInput;

    public int notesHit;
    public int currScore;

    public string[] recentNoteStatus;
    public int recentNoteLane;
    public bool noteHit;
    private Color[] _hitboxColors;
    private List<Vector2>[] _particleFields;
    public HitboxSystem(List<Texture2D> particleTextures)
    {
        _laneCollisionBoxes = new Rectangle[5];
        _controllerCalled = new bool[6];
        canActivate = new bool[6];
        canInput = new bool[6];
        _hitboxColors = new Color[6];
        notesHit = 0;
        currScore = 0;
        recentNoteStatus = new string[6];
        for (int i = 0; i < 6; i++)
        {
            recentNoteStatus[i] = "";
        }
        recentNoteLane = 1;
        noteHit = false;

        for (int i = 0; i < canActivate.Length; i++)
        {
            _controllerCalled[i] = false;
            canActivate[i] = false;
            canInput[i] = false;
        }
        _hitboxColors[0] = new Color(231, 138, 0, 20);
        _hitboxColors[1] = new Color(187, 73, 108, 20);
        _hitboxColors[2] = new Color(138, 0, 231, 20);
        _hitboxColors[3] = new Color(80, 96, 192, 20);
        _hitboxColors[4] = new Color(0, 231, 138, 20);
        _hitboxColors[5] = new Color((int)Color.White.R, Color.White.G, Color.White.B, 50);
        assignBoxes();
        _particleFields = new List<Vector2>[5]; // Allows for particles to generate by field rather than by point.
        _particleFields[0] = determineBoundingField(new Vector2(10, 480), new Vector2(50, 400), new Vector2(174, 400), new Vector2(150, 480));
        _particleFields[1] = determineBoundingField(new Vector2(170, 480), new Vector2(194, 400), new Vector2(318, 400), new Vector2(310, 480));
        _particleFields[2] = determineBoundingField(new Vector2(330, 480), new Vector2(338, 400), new Vector2(462, 400), new Vector2(470, 480));
        _particleFields[3] = determineBoundingField(new Vector2(630, 480), new Vector2(490, 480), new Vector2(482, 400), new Vector2(606, 400));
        _particleFields[4] = determineBoundingField(new Vector2(790, 480), new Vector2(650, 480), new Vector2(626, 400), new Vector2(750, 400));
        this.ParticleSystems = new ParticleSystem[5];
        for (int i = 0; i < 5; i++)
        {
            this.ParticleSystems[i] = new ParticleSystem(particleTextures, _particleFields[i]);
            this.ParticleSystems[i].settings.ParticleDensity = 5;
            this.ParticleSystems[i].settings.Lifespan = 25;
            this.ParticleSystems[i].settings.Transparency = 25;
            this.ParticleSystems[i].settings.Velocity = ParticleSystemSettings.VelocityType.Up;
            this.ParticleSystems[i].settings.Color = ParticleSystemSettings.ColorType.Spectrum;
            this.ParticleSystems[i].settings.ParticleColor = _hitboxColors[i];
            this.ParticleSystems[i].settings.emitManually = true;
        }
    }
    public void Update(GameTime gameTime)
    {
        foreach (ParticleSystem pSys in ParticleSystems)
        {
            pSys.Update(gameTime);
        }
    }
    public void playParticleEffect(int lane)
    {
        if (lane < 6)
        {
            this.ParticleSystems[lane - 1].manualGeneration();
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                this.ParticleSystems[i].settings.ParticleColor = new Color(230, 230, 230, 20);
                this.ParticleSystems[i].manualGeneration();
                this.ParticleSystems[i].settings.ParticleColor = _hitboxColors[i];
            }

        }
    }
    public void registerScore(float score, INoteObject note, double timing)
    {
        if (note is not HoldNote)
        {
            noteHit = true;
            note.destroy();
            if (score > 80)
            {
                currScore = 250;
                recentNoteStatus[note.Lane - 1] = "S-CRITICAL\n" + timing + "ms";
            }
            else if (score > 60)
            {
                currScore = 200;
                recentNoteStatus[note.Lane - 1] = "CRITICAL\n" + timing + "ms";
            }
            else if (score > 0)
            {
                currScore = 100;
                recentNoteStatus[note.Lane - 1] = "NEAR\n" + timing + "ms";
            }
        }
        else if (note is HoldNote&&score>0)
        {
            noteHit = true;
            currScore = 50;
            note.destroy();
            recentNoteStatus[note.Lane - 1] = "CRITICAL";
        }
        else
        {
            noteHit = true;
            currScore = 0;
            recentNoteStatus[note.Lane - 1] = "ERROR\n" + timing + "ms";
            note.destroy();
        }
        recentNoteLane = note.Lane;
    }
    public void Draw(SpriteBatch batch, BasicEffect effect, GraphicsDevice graphics)
    { // Vertices are hardcoded because any attempt to automatically calculate the trapezoids would be pointless. They will only ever need to be drawn in their
        // initial position. It was instead easier to calculate the slopes by hand and put them in here.
        int addIfWhite = 0;
        Texture2D pixel = new Texture2D(graphics, 1, 1);
        //if (canActivate[5]) addIfWhite = 5;
        if (canActivate[0])
        {

            ShapeRenderer.DrawQuad(effect, graphics, new Vector2(10, 480), new Vector2(50, 400), new Vector2(174, 400), new Vector2(150, 480), _hitboxColors[Math.Clamp(0 + addIfWhite, 0, 5)]);
        }
        if (canActivate[1])
        {

            ShapeRenderer.DrawQuad(effect, graphics, new Vector2(170, 480), new Vector2(194, 400), new Vector2(318, 400), new Vector2(310, 480), _hitboxColors[Math.Clamp(1 + addIfWhite, 0, 5)]);
        }
        if (canActivate[2])
        {

            ShapeRenderer.DrawQuad(effect, graphics, new Vector2(330, 480), new Vector2(338, 400), new Vector2(462, 400), new Vector2(470, 480), _hitboxColors[Math.Clamp(2 + addIfWhite, 0, 5)]);
        }
        if (canActivate[3])
        {

            ShapeRenderer.DrawQuad(effect, graphics, new Vector2(630, 480), new Vector2(490, 480), new Vector2(482, 400), new Vector2(606, 400), _hitboxColors[Math.Clamp(3 + addIfWhite, 0, 5)]);
        }
        if (canActivate[4])
        {

            ShapeRenderer.DrawQuad(effect, graphics, new Vector2(790, 480), new Vector2(650, 480), new Vector2(626, 400), new Vector2(750, 400), _hitboxColors[Math.Clamp(4 + addIfWhite, 0, 5)]);
        }
        if (canActivate[5])
        {
            addIfWhite = 5;
            ShapeRenderer.DrawQuad(effect, graphics, new Vector2(10, 480), new Vector2(50, 400), new Vector2(174, 400), new Vector2(150, 480), _hitboxColors[Math.Clamp(0 + addIfWhite, 0, 5)]);
            ShapeRenderer.DrawQuad(effect, graphics, new Vector2(170, 480), new Vector2(194, 400), new Vector2(318, 400), new Vector2(310, 480), _hitboxColors[Math.Clamp(1 + addIfWhite, 0, 5)]);
            ShapeRenderer.DrawQuad(effect, graphics, new Vector2(330, 480), new Vector2(338, 400), new Vector2(462, 400), new Vector2(470, 480), _hitboxColors[Math.Clamp(2 + addIfWhite, 0, 5)]);
            ShapeRenderer.DrawQuad(effect, graphics, new Vector2(630, 480), new Vector2(490, 480), new Vector2(482, 400), new Vector2(606, 400), _hitboxColors[Math.Clamp(3 + addIfWhite, 0, 5)]);
            ShapeRenderer.DrawQuad(effect, graphics, new Vector2(790, 480), new Vector2(650, 480), new Vector2(626, 400), new Vector2(750, 400), _hitboxColors[Math.Clamp(4 + addIfWhite, 0, 5)]);

        }
        //for (int i = 0; i < canActivate.Length; i++)
        //{
        //    canActivate[i] = false;
        //    canInput[i] = false;
        //}
        foreach (ParticleSystem pSys in ParticleSystems)
        {
            pSys.Draw(batch);
        }
    }
    public void setDeactive(int lane)
    {
        canActivate[lane - 1] = false;
        canInput[lane - 1] = false;
    }

    public void setAllDeactive()
    {
        canActivate[5] = false;
        canInput[5] = false;
    }
    public Rectangle getCollisionBox(int lane)
    {
        return _laneCollisionBoxes[lane];
    }
    public void setAllActive()
    {
        canActivate[5] = true;
        canInput[5] = true;
    }
    public void setActive(int lane)
    {
        canActivate[lane - 1] = true;
        canInput[lane - 1] = true;
    }
    public Color getHitboxColor(int lane)
    {
        return _hitboxColors[lane - 1];
    }
    private void assignBoxes()
    {
        _laneCollisionBoxes[0] = determineCollisionBox(new Vector2(10, 480), new Vector2(50, 400), new Vector2(174, 400), new Vector2(150, 480));
        _laneCollisionBoxes[1] = determineCollisionBox(new Vector2(170, 480), new Vector2(194, 400), new Vector2(318, 400), new Vector2(310, 480));
        _laneCollisionBoxes[2] = determineCollisionBox(new Vector2(330, 480), new Vector2(338, 400), new Vector2(462, 400), new Vector2(470, 480));
        _laneCollisionBoxes[3] = determineCollisionBox(new Vector2(630, 480), new Vector2(490, 480), new Vector2(482, 400), new Vector2(606, 400));
        _laneCollisionBoxes[4] = determineCollisionBox(new Vector2(790, 480), new Vector2(650, 480), new Vector2(626, 400), new Vector2(750, 400));
    }

    /*
     * This method calculates the two center-most vertices based on X position and creates the largest possible rectangle based on that.
     */
    private Rectangle determineCollisionBox(Vector2 vA, Vector2 vB, Vector2 vC, Vector2 vD)
    {
        const int RECTANGLE_HEIGHT = 80;
        Vector2[] vertices = { vA, vB, vC, vD };
        Vector2[] centerVertices = new Vector2[2];
        Vector2 tempVertex;
        for (int i = 0; i < vertices.Length - 1; i++) // Order vertices via bubble sort
        {
            for (int j = 0; j < vertices.Length - 1 - i; j++)
            {
                if (vertices[j].X > vertices[j + 1].X)
                {
                    tempVertex = vertices[j];
                    vertices[j] = vertices[j + 1];
                    vertices[j + 1] = tempVertex;
                }
            }
        }
        centerVertices[0] = vertices[1]; // Center-most vertices are middle 2
        centerVertices[1] = vertices[2];
        if (centerVertices[0].Y > centerVertices[1].Y)
        {
            centerVertices[0].Y = centerVertices[1].Y;
        }
        return new Rectangle(new Point((int)centerVertices[0].X, (int)centerVertices[0].Y), new Point((int)(centerVertices[1].X - centerVertices[0].X), RECTANGLE_HEIGHT));
    }
    private static List<Vector2> determineBoundingField(Vector2 vA, Vector2 vB, Vector2 vC, Vector2 vD) // Determines the particle field for a given trapezoid.
    {
        List<Vector2> result = new List<Vector2>();
        Vector2[] vertices = { vA, vB, vC, vD };
        Vector2 tempVertex;
        int minX, minY, maxX, maxY;
        for (int i = 0; i < vertices.Length - 1; i++) // Order X vertices via bubble sort
        {
            for (int j = 0; j < vertices.Length - 1 - i; j++)
            {
                if (vertices[j].X > vertices[j + 1].X)
                {
                    tempVertex = vertices[j];
                    vertices[j] = vertices[j + 1];
                    vertices[j + 1] = tempVertex;
                }
            }
        }
        minX = (int)vertices[0].X;
        maxX = (int)vertices[3].X;

        if (vertices[3].Y < vertices[2].Y)
        {
            tempVertex = vertices[3];
            vertices[3] = vertices[2];
            vertices[2] = tempVertex;
        }
        if (vertices[1].Y < vertices[0].Y)
        {
            tempVertex = vertices[1];
            vertices[1] = vertices[0];
            vertices[0] = tempVertex;
        }
        // End result is the vertices sorted smallest to largest X, then smallest to largest Y.

        minY = (int)vertices[0].Y;
        maxY = (int)vertices[3].Y;

        vertices[0] = vA; // Return vertices to original index position. Needed so vectors are guarenteed to be adjacent to each other.
        vertices[1] = vB;
        vertices[2] = vC;
        vertices[3] = vD;

        for (int i = minX; i < maxX; i++) // Algorithm runs through largest box formed by vertices, selecting all integer points in it. Checks if inside trapezoid via
                                          // dot product formula, which ideally gives a sum of angles = 2pi
        {
            for (int j = minY; j < maxY; j++)
            {
                Vector2 test = new Vector2(i, j);
                float angleSum = 0;
                Vector2 a, b;
                for (int k = 0; k < 4; k++)
                {
                    int next = k + 1;
                    if (next == 4) next = 0;
                    a = new Vector2(test.X - vertices[k].X, test.Y - vertices[k].Y);
                    b = new Vector2(test.X - vertices[next].X, test.Y - vertices[next].Y);
                    float dotProduct = Vector2.Dot(a, b);
                    angleSum += MathF.Acos((dotProduct) / (vectorMagnitude(a) * vectorMagnitude(b)));
                }
                if (angleSum >= 6.28f) // Close enough.
                {
                    result.Add(test);
                }
            }
        }
        return result;
    }
    private static float vectorMagnitude(Vector2 v) // Returns a float. Vector2 has Length, but it returns a method group.
    {
        return (float)Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2));
    }
}