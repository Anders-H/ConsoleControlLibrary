using System.Drawing;
using System.Windows.Forms;

namespace ConsoleControlLibrary;

internal class Mouse
{
    public int X { get; set; }
    public int Y { get; set; }

    public Mouse()
    {
        X = -1;
        Y = -1;
    }

    public void UpdateMousePosition(MouseEventArgs? e, IDrawEngine drawEngine)
    {
        if (e == null)
            return;

        if (e.X == 0 || drawEngine.ScaleX < 0.0001f)
            X = 0;
        else
            X = (int)(e.X / drawEngine.ScaleX);

        if (e.Y == 0 || drawEngine.ScaleY < 0.0001f)
            Y = 0;
        else
            Y = (int)(e.Y / drawEngine.ScaleY);
    }

    public Point AsPoint() =>
        new(X, Y);

    public Point ToCharacterPosition(IDrawEngine drawEngine) =>
        new((int)(X / drawEngine.CharacterWidth), (int)(Y / drawEngine.CharacterHeight));
}