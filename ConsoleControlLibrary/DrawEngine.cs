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
            CharacterOffsetX = (CharacterWidth / 2.0) - (csize.Width / 2.0);
            CharacterOffsetY = (CharacterHeight / 2.0) - (csize.Height / 2.0);
        }

        public Tuple<int, int> PhysicalCoordinateToFormCoordinate(int x, int y)
        {
            var sourcex = x - CharacterOffsetX;
            var sourcey = y - CharacterOffsetY;
            var cx = x > 0 ? (int)(sourcex / CharacterWidth) : 0;
            var cy = y > 0 ? (int)(sourcey / CharacterHeight) : 0;
            return Tuple.Create(cx, cy);
        }

        public void DrawCharacter(Graphics g, char c, Font f, Brush b, int x, int y) =>
            g.DrawString(c.ToString(), f, b, (float)(x*CharacterWidth + CharacterOffsetX), (float)(y*CharacterHeight + CharacterOffsetY));
        
        public void DrawCursor(Graphics g, Brush b, int x, int y) =>
            g.FillRectangle(b, (float)(x*CharacterWidth), (float)(y*CharacterHeight), (float)CharacterWidth, (float)CharacterHeight);
        
        public void DrawCursor(Graphics g, Pen p, int x, int y) =>
            g.DrawRectangle(p, (float)(x*CharacterWidth), (float)(y*CharacterHeight), (float)CharacterWidth, (float)CharacterHeight);
       
        public void OutlineControl(Graphics g, Pen p, Rectangle outline) =>
            g.DrawRectangle(p, new Rectangle((int)(outline.X * CharacterWidth), (int)(outline.Y * CharacterHeight), (int)(outline.Width * CharacterWidth), (int)(outline.Height * CharacterHeight)));
    }
}