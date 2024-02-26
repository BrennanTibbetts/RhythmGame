using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint5BeanTeam
{
    public class TextRenderer
    {
        public Rectangle Area { get; set; }
        public SpriteFont Font { get; set; }
        public Color Color { get; set; }

        private readonly TextBox box;
        private RenderTarget2D target;
        private SpriteBatch batch;
        private Texture2D text;
        internal readonly short[] X;
        internal readonly short[] Y;

        internal readonly byte[] Width;

        private readonly byte[] row;

        public void Dispose()
        {
            text?.Dispose();
            text = null;
            target?.Dispose();
            target = null;
            Font = null;
            batch?.Dispose();
            batch = null;
        }

        public TextRenderer(TextBox box)
        {
            this.box = box;

            X = new short[this.box.Text.MaxLength];
            Y = new short[this.box.Text.MaxLength];
            Width = new byte[this.box.Text.MaxLength];

            row = new byte[this.box.Text.MaxLength];
        }

        public void Update()
        {
            if (!box.Text.IsDirty)
            {
                return;
            }

            MeasureCharacterWidths();
            text = RenderText();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (text != null)
            {
                spriteBatch.Draw(text, Area, Color.White);
            }
        }

        public int CharAt(Point localLocation)
        {
            Rectangle charRectangle = new Rectangle(0, 0, 0, Font.LineSpacing);

            int r = localLocation.Y / (Font.LineSpacing);

            for (short i = 0; i < box.Text.Length; i++)
            {
                if (row[i] != r)
                {
                    continue;
                }

                charRectangle.X = X[i];
                charRectangle.Y = Y[i];
                charRectangle.Width = Width[i];

                if (charRectangle.Contains(localLocation))
                {
                    return i;
                }

                if (i < box.Text.Length - 1 && row[i + 1] != r)
                {
                    return i;
                }
            }

            // Missed a character so return the end.
            return box.Text.Length;
        }

        private void MeasureCharacterWidths()
        {
            for (int i = 0; i < box.Text.Length; i++)
            {
                Width[i] = MeasureCharacter(i);
            }
        }

        private byte MeasureCharacter(int location)
        {
            string value = box.Text.String;
            float front = Font.MeasureString(value.Substring(0, location)).X;
            float end = Font.MeasureString(value.Substring(0, location + 1)).X;

            return (byte) (end - front);
        }

        private Texture2D RenderText()
        {
            if (batch == null)
            {
                batch = new SpriteBatch(box.GraphicsDevice);
            }
            if (target == null)
            {
                target = new RenderTarget2D(box.GraphicsDevice, Area.Width, Area.Height);
            }

            box.GraphicsDevice.SetRenderTarget(target);

            box.GraphicsDevice.Clear(Color.Transparent);

            int start = 0;
            float height = 0.0f;

            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            while (true)
            {
                start = RenderLine(batch, start, height);

                if (start >= box.Text.Length)
                {
                    batch.End();
                    box.GraphicsDevice.SetRenderTarget(null);

                    return target;
                }

                height += Font.LineSpacing;
            }
        }

        private int RenderLine(SpriteBatch spriteBatch, int start, float height)
        {
            int breakLocation = start;
            float lineLength = 0.0f;
            byte r = (byte) (height / Font.LineSpacing);

            string t = box.Text.String;
            string tempText;

            for (int iCount = start; iCount < box.Text.Length; iCount++)
            {
                X[iCount] = (short) lineLength;
                Y[iCount] = (short) height;
                row[iCount] = r;

                lineLength += Width[iCount];

                if (lineLength > Area.Width)
                {
                    if (breakLocation == start)
                    {
                        tempText = t.Substring(start, iCount - start);
                        spriteBatch.DrawString(Font, tempText, new Vector2(0.0f, height), Color);
                        return iCount + 1;
                    }
                    tempText = t.Substring(start, breakLocation - start);
                    spriteBatch.DrawString(Font, tempText, new Vector2(0.0f, height), Color);
                    return breakLocation + 1;
                }

                switch (box.Text.Characters[iCount])
                {
                    case '\r':
                    case '\n':
                        tempText = t.Substring(start, iCount - start);
                        spriteBatch.DrawString(Font, tempText, new Vector2(0.0f, height), Color);
                        return iCount + 1;
                    case '-':
                    case ' ':
                        breakLocation = iCount + 1;
                        break;
                }
            }

            tempText = t.Substring(start, box.Text.Length - start);
            spriteBatch.DrawString(Font, tempText, new Vector2(0.0f, height), Color);
            return box.Text.Length;
        }
    }
}