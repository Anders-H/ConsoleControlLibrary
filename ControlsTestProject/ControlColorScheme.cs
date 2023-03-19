using System.Drawing;
using ConsoleControlLibrary;

namespace ControlsTestProject;

public class ControlColorScheme : IControlColorScheme
{
    public  Color BackColor =>
        Color.FromArgb(30, 15, 5);

    public  Color ForeColor =>
        Color.FromArgb(0, 255, 0);

    public  Color InputControlBackColor =>
        Color.FromArgb(0, 0, 0);

    public  Color ActiveControlForeColor =>
        Color.FromArgb(127, 255, 255);

    public  Color DisabledForeColor =>
        Color.FromArgb(0, 127, 0); 
}