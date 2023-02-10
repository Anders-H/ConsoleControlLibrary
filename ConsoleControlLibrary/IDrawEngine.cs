using System.Drawing;

namespace ConsoleControlLibrary;

public interface IDrawEngine
{
    int ColumnCount { get; }
    int RowCount { get; }
    double CharacterWidth { get; }
    double CharacterHeight { get; }
    double CharacterOffsetX { get; }
    double CharacterOffsetY { get; }
    void CalculateSizes(Graphics g, ref Font? f, int columnCount, int rowCount, int canvasWidth, int canvasHeight);
    void DrawCharacter(Graphics g, char c, Font f, Brush b, int x, int y);
    void DrawCursor(Graphics g, Brush b, int x, int y);
    void DrawCursor(Graphics g, Pen p, int x, int y);
    Point PhysicalCoordinateToFormCoordinate(int x, int y);
    void OutlineControl(Graphics g, Pen p, Rectangle outline);
    public void FillControl(Graphics g, Brush b, Rectangle outline);
}