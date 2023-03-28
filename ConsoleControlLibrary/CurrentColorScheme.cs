using System;
using System.Drawing;

namespace ConsoleControlLibrary;

public class CurrentColorScheme : IDisposable
{
    public Color BackColor { get; }

    public SolidBrush ForeColor { get; }

    public SolidBrush InputControlBackColor { get; }

    public SolidBrush ActiveControlForeColor { get; }

    public SolidBrush DisabledForeColor { get; }

    public CurrentColorScheme(Color backColor, Color foreColor, Color inputControlBackColor, Color activeControlForeColor, Color disabledForeColor)
    {
        BackColor = backColor;
        ForeColor = new SolidBrush(foreColor);
        InputControlBackColor = new SolidBrush(inputControlBackColor);
        ActiveControlForeColor = new SolidBrush(activeControlForeColor);
        DisabledForeColor = new SolidBrush(disabledForeColor);
    }

    public CurrentColorScheme(Color backColor, SolidBrush foreColor, SolidBrush inputControlBackColor, SolidBrush activeControlForeColor, SolidBrush disabledForeColor)
    {
        BackColor = backColor;
        ForeColor = foreColor;
        InputControlBackColor = inputControlBackColor;
        ActiveControlForeColor = activeControlForeColor;
        DisabledForeColor = disabledForeColor;
    }

    public static CurrentColorScheme GetDefaultColorScheme() =>
        new(
            Color.FromArgb(70, 70, 70),
            Color.FromArgb(127, 255, 127),
            Color.FromArgb(0, 0, 0),
            Color.FromArgb(255, 255, 255),
            Color.FromArgb(120, 120, 120)
        );

    public void Dispose()
    {
        ForeColor.Dispose();
        InputControlBackColor.Dispose();
        ActiveControlForeColor.Dispose();
        DisabledForeColor.Dispose();
    }
}