using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint5BeanTeam
{
    public class Cursor
    {
        public Color Color { get; set; }
        public Color Selection { get; set; }
        public Rectangle Icon { get; set; }

        public bool Active { get; set; }

        private bool visible;
        private readonly int ticksPerBlink;
        private int ticks;

        public int TextCursor
        {
            get { return textCursor; }
            set { textCursor = value.Clamp(0, textBox.Text.Length); }
        }

        public int? SelectedChar
        {
            get { return selectedChar; }
            set
            {
                if (value.HasValue)
                {
                    if (value.Value != TextCursor)
                    {
                        selectedChar = (short) (value.Value.Clamp(0, textBox.Text.Length));
                    }
                }
                else
                {
                    selectedChar = null;
                }
            }
        }

        private readonly TextBox textBox;

        private int textCursor;
        private int? selectedChar;

        public Cursor(TextBox textBox, Color color, Color selection, Rectangle icon, int ticksPerBlink)
        {
            this.textBox = textBox;
            Color = color;
            Selection = selection;
            Icon = icon;
            Active = true;
            visible = false;
            this.ticksPerBlink = ticksPerBlink;
            ticks = 0;
        }

        public void Update()
        {
            ticks++;

            if (ticks <= ticksPerBlink)
            {
                return;
            }

            visible = !visible;
            ticks = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int x = textBox.Renderer.Area.X;
            int y = textBox.Renderer.Area.Y;

            Point cp = GetPosition(x, y, TextCursor);
            if (selectedChar.HasValue)
            {
                Point sc = GetPosition(x, y, selectedChar.Value);
                if (sc.X > cp.X)
                {
                    spriteBatch.Draw(spriteBatch.GetWhitePixel(),
                        new Rectangle(cp.X, cp.Y, sc.X - cp.X, textBox.Renderer.Font.LineSpacing), Icon, Selection);
                }
                else
                {
                    spriteBatch.Draw(spriteBatch.GetWhitePixel(),
                        new Rectangle(sc.X, sc.Y, cp.X - sc.X, textBox.Renderer.Font.LineSpacing), Icon, Selection);
                }
            }

            if (!visible)
            {
                return;
            }

            spriteBatch.Draw(spriteBatch.GetWhitePixel(),
                new Rectangle(cp.X, cp.Y, Icon.Width, textBox.Renderer.Font.LineSpacing), Icon, Color);
        }

        private Point GetPosition(int x, int y, int pos)
        {
            if (pos > 0)
            {
                if (textBox.Text.Characters[pos - 1] == '\n'
                    || textBox.Text.Characters[pos - 1] == '\r')
                {
                    y += textBox.Renderer.Y[pos - 1] + textBox.Renderer.Font.LineSpacing;
                }
                else if (pos == textBox.Text.Length)
                {
                    x += textBox.Renderer.X[pos - 1] + textBox.Renderer.Width[pos - 1];
                    y += textBox.Renderer.Y[pos - 1];
                }
                else
                {
                    x += textBox.Renderer.X[pos];
                    y += textBox.Renderer.Y[pos];
                }
            }
            return new Point(x, y);
        }
    }
}