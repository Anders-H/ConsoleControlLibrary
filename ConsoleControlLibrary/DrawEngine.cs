using System;
using System.Drawing;

namespace ConsoleControlLibrary;

public class DrawEngine : IDrawEngine
{
    public float ScaleX { get; private set; }
    public float ScaleY { get; private set; }
    public int ColumnCount { get; private set; }
    public int RowCount { get; private set; }
    public double CharacterWidth { get; }
    public double CharacterHeight { get; }

    public DrawEngine()
    {
        CharacterWidth = 8;
        CharacterHeight = 8;
    }

    public void CalculateSizes(Graphics g, ref Font? f, int columnCount, int rowCount, int canvasWidth, int canvasHeight)
    {
        double pixelsWidth = columnCount * CharacterWidth;
        double pixelsHeight = rowCount * CharacterHeight;

        if (canvasWidth > pixelsWidth && canvasHeight > pixelsHeight)
        {
            ScaleX = (float)(canvasWidth / (double)pixelsWidth);
            ScaleY = (float)(canvasHeight / (double)pixelsHeight);
        }
        else if (canvasWidth > pixelsWidth)
        {
            ScaleX = (float)(canvasWidth / (double)pixelsWidth);
            ScaleY = 1f;
        }
        else if (canvasHeight > pixelsHeight)
        {
            ScaleX = 1f;
            ScaleY = (float)(canvasHeight / (double)pixelsHeight);
        }
        else
        {
            ScaleX = 1f;
            ScaleY = 1f;
        }
    }

    public void DrawCharacter(Graphics g, char c, Font f, Brush b, int x, int y) =>
        g.DrawString(c.ToString(), f, b, (float)(x * CharacterWidth), (float)(y * CharacterHeight));
        
    public void DrawCursor(Graphics g, Brush b, int x, int y) =>
        g.FillRectangle(b, (float)(x*CharacterWidth), (float)(y*CharacterHeight), (float)CharacterWidth, (float)CharacterHeight);
        
    public void DrawCursor(Graphics g, Pen p, int x, int y) =>
        g.DrawRectangle(p, (float)(x*CharacterWidth), (float)(y*CharacterHeight), (float)CharacterWidth, (float)CharacterHeight);
       
    public void OutlineControl(Graphics g, Pen p, Rectangle outline) =>
        g.DrawRectangle(
            p,
            new Rectangle((int)(outline.X * CharacterWidth),
                (int)(outline.Y * CharacterHeight),
                (int)(outline.Width * CharacterWidth),
                (int)(outline.Height * CharacterHeight))
        );

    public void FillControl(Graphics g, Brush b, Rectangle outline) =>
        g.FillRectangle(
            b,
            new Rectangle((int)(outline.X * CharacterWidth),
                (int)(outline.Y * CharacterHeight),
                (int)(outline.Width * CharacterWidth),
                (int)(outline.Height * CharacterHeight))
        );

    public void DrawUnderline(Graphics g, Brush b, int x, int y, int width)
    {
        var physicalY = (int)(y * CharacterHeight + CharacterHeight - 1);
        g.FillRectangle(b,
            new Rectangle((int)(x * CharacterWidth),
                physicalY,
                (int)(width * CharacterWidth),
                1));
    }
}