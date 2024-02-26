using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

	public static class ShapeRenderer
	{
	/// <summary>
	/// Draw a line using traditional spritebatch and single pixel texture.
	/// </summary>
	/// <param name="batch"></param> The SpriteBatch to draw to
	/// <param name="polygonTexture"></param> The single pixel texture
	/// <param name="start"></param> Starting point
	/// <param name="end"></param> Ending point
	/// <param name="col"></param> The color of the line
	/// <param name="lineWidth"></param> The width, or thickness, or the line
		public static void DrawLine(SpriteBatch batch, Texture2D polygonTexture, Vector2 start, Vector2 end, Color col, int lineWidth = 1)
			{
				Vector2 length = end - start;
				float angle = (float)Math.Atan2(length.Y, length.X);
				batch.Draw(polygonTexture, start, null, col, angle, Vector2.Zero, new Vector2(length.Length(), lineWidth), SpriteEffects.None, 0);
			}
    /// <summary>
    /// Draws a single pixel wide line using primitives
    /// </summary>
    /// <param name="e"></param> The BasicEffect, passed in order to apply color to the line
    /// <param name="g"></param> The game's GraphicsDevice, used to render primitives
    /// <param name="start"></param> Starting point
    /// <param name="end"></param> Ending point
    /// <param name="col"></param> The color of the line
    public static void DrawLinePrimitive(BasicEffect e, GraphicsDevice g, Vector2 start, Vector2 end, Color col)
	{
        VertexPositionColor[] vertices = new VertexPositionColor[2];
        vertices[0] = new VertexPositionColor(new Vector3(start.X, start.Y, 0), col);
        vertices[1] = new VertexPositionColor(new Vector3(end.X, end.Y, 0), col);
        e.DiffuseColor = new Vector3(col.R / 255f, col.G / 255f, col.B / 255f);
        e.CurrentTechnique.Passes[0].Apply();
        g.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1);
    }
    /// <summary>
    /// Traditional rectangle drawing using spritebatch and single pixel texture.
    /// </summary>
    /// <param name="batch"></param> The SpriteBatch to draw to
    /// <param name="polygonTexture"></param> The single pixel texture
    /// <param name="a"></param> Vertex of one corner of the rectangle
    /// <param name="b"></param> Vertex of another corner of a rectangle
    /// <param name="col"></param> Color of the rectangle
    public static void DrawRectangle(SpriteBatch batch, Texture2D polygonTexture, Vector2 a, Vector2 b, Color col)
			{
				int xLength = (int) (a.X - b.X);
				xLength = Math.Abs(xLength);
				int yLength = (int) (a.Y - b.Y);
				yLength = Math.Abs(yLength);
				Vector2 leftmost = leftVertice(a, b);
				Rectangle r = new Rectangle((int)leftmost.X, (int)leftmost.Y, xLength, yLength);
				batch.Draw(polygonTexture, r, null, col);
			}
	/// <summary>
	/// Draws a color-filled triangle using three vertices.
	/// </summary>
	/// <param name="e"></param> The BasicEffect, passed in order to apply color to the triangle
	/// <param name="g"></param> The game's GraphicsDevice, used to render primitives
	/// <param name="a"></param> First vertex
	/// <param name="b"></param> Second vertex
	/// <param name="c"></param> Third vertex
	/// <param name="col"></param> Color of the triangle
		public static void DrawTriangle(BasicEffect e, GraphicsDevice g, Vector2 a, Vector2 b, Vector2 c, Color col)
			{
				VertexPositionColor[] vertices = new VertexPositionColor[3];
				vertices[0] = new VertexPositionColor(new Vector3(a.X, a.Y, 0), col);
				vertices[1] = new VertexPositionColor(new Vector3(b.X, b.Y, 0), col);
				vertices[2] = new VertexPositionColor(new Vector3(c.X, c.Y, 0), col);
				e.DiffuseColor = new Vector3(col.R / 255f, col.G / 255f, col.B / 255f);
				e.CurrentTechnique.Passes[0].Apply();
				g.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 1);
			}
	/// <summary>
	/// Draws a color-filled quadrilateral using four vertices. It is important to go through all points in either
	/// clockwise or counter-clockwise order, starting with any point.
	/// </summary>
	/// <param name="e"></param> The BasicEffect, passed in order to apply color to the quad
	/// <param name="g"></param> The game's GraphicsDevice, used to render primitives
	/// <param name="a"></param> First vertex
	/// <param name="b"></param> Second vertex
	/// <param name="c"></param> Third vertex
	/// <param name="d"></param> Fourth vertex
	/// <param name="col"></param> Color of the quad
		public static void DrawQuad(BasicEffect e, GraphicsDevice g, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color col)
	{
        VertexPositionColor[] vertices = new VertexPositionColor[4];
		e.VertexColorEnabled = true;
        e.DiffuseColor = new Vector3(1, 1, 1);
        vertices[0] = new VertexPositionColor(new Vector3(a.X, a.Y, -1), col);
        vertices[1] = new VertexPositionColor(new Vector3(b.X, b.Y, -1), col);
        vertices[2] = new VertexPositionColor(new Vector3(c.X, c.Y, -1), col);
        vertices[3] = new VertexPositionColor(new Vector3(d.X, d.Y, -1), col);
        short[] indices = { 0, 1, 2, 0, 2, 3 };
        e.CurrentTechnique.Passes[0].Apply();
        g.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0, 2);
    }

    /// <summary>
    /// Draws a quadrilateral with a simple linear two color gradient.
    /// </summary>
    /// <param name="e"></param> The BasicEffect, passed in order to apply color to the quad
    /// <param name="g"></param> The game's GraphicsDevice, used to render primitives
    /// <param name="a"></param> First vertex
    /// <param name="b"></param> Second vertex
    /// <param name="c"></param> Third vertex
    /// <param name="d"></param> Fourth vertex
    /// <param name="col1"></param> First color of the quad
    /// <param name="col2"></param> Second color of the quad
    public static void DrawQuadWithGradient(BasicEffect e, GraphicsDevice g, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color col1, Color col2)
    {
        VertexPositionColor[] vertices = new VertexPositionColor[4];
        e.VertexColorEnabled = true;
        e.DiffuseColor = new Vector3(1, 1, 1);
        vertices[0] = new VertexPositionColor(new Vector3(a.X, a.Y, -1), col1);
        vertices[1] = new VertexPositionColor(new Vector3(b.X, b.Y, -1), col1);
        vertices[2] = new VertexPositionColor(new Vector3(c.X, c.Y, -1), col2);
        vertices[3] = new VertexPositionColor(new Vector3(d.X, d.Y, -1), col2);
        short[] indices = { 0, 1, 2, 0, 2, 3 };
        e.CurrentTechnique.Passes[0].Apply();
        g.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 4, indices, 0, 2);
    }


    private static Vector2 leftVertice (Vector2 a, Vector2 b)
			{
				float leftX = 0;
				float leftY = 0;
				if (a.X < b.X) leftX = a.X;
				else leftX = b.X;
				if (a.Y < b.Y) leftY = a.Y;
				else leftY = b.Y;
				return new Vector2 (leftX, leftY);
			} 
	}