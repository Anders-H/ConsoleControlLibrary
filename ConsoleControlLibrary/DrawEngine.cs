using System;
using System.Drawing;

namespace ConsoleControlLibrary
{
    public class DrawEngine : IDrawEngine
    {
        public int ColumnCount { get; private set; }
        public int RowCount { get; private set; }
        public double CharacterWidth { get; private set; }
        public double CharacterHeight { get; private set; }
        public double CharacterOffsetX { get; private set; }
        public double CharacterOffsetY { get; private set; }
        public void CalculateSizes(Graphics g, ref Font f, int columnCount, int rowCount, int canvasWidth, int canvasHeight)
        {
            ColumnCount = columnCount;
            RowCount = rowCount;
            var width = Math.Max(canvasWidth, 30.0);
            var height = Math.Max(canvasHeight, 30.0);
            CharacterWidth = width / ColumnCount;
            CharacterHeight = height / RowCount;
            var fontSize = (float)(Math.Min(CharacterWidth, CharacterHeight) - 1);
            for (var i = fontSize + 20; i >= fontSize; i -= 0.5f)
            {
                f?.Dispose();
                f = new Font("Consolas", fontSize);
                if (g.MeasureString("WM", f).Width >= fontSize || g.MeasureString("Åj", f).Height >= fontSize)
                    break;
            }
            var csize = g.MeasureString("W", f);
            //TODO: Detta fungerar skit. Skriv om. Sen.
            CharacterOffsetX = Math.Abs((CharacterWidth / 2.0) - (csize.Width / 2.0));
            CharacterOffsetY = Math.Abs((CharacterWidth / 2.0) - (csize.Height / 2.0));
            if (csize.Width > canvasWidth)
                CharacterOffsetX = -CharacterOffsetX;
            if (csize.Height > canvasHeight)
                CharacterOffsetY = -CharacterOffsetY;
        }

        public void DrawCharacter(Graphics g, char c, Font f, Brush b, int x, int y) =>
            g.DrawString(c.ToString(), f, b, (float)(x * CharacterWidth + CharacterOffsetX), (float)(y * CharacterHeight + CharacterOffsetY));
        public void DrawCursor(Graphics g, Brush b, int x, int y) =>
            g.FillRectangle(b, (float)(x * CharacterWidth + CharacterOffsetX), (float)(y * CharacterHeight + CharacterOffsetY), (float)CharacterWidth, (float)CharacterHeight);
        public void DrawCursor(Graphics g, Pen p, int x, int y) =>
            g.DrawRectangle(p, (float)(x * CharacterWidth + CharacterOffsetX), (float)(y * CharacterHeight + CharacterOffsetY), (float)CharacterWidth, (float)CharacterHeight);
    }
}
