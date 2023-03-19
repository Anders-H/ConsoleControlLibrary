using System.Drawing;

namespace ConsoleControlLibrary;

public interface IControlColorScheme
{
    public Color BackColor { get; }

    public Color ForeColor { get; }

    public Color InputControlBackColor { get; }

    public Color ActiveControlForeColor { get; }

    public Color DisabledForeColor { get; }
}