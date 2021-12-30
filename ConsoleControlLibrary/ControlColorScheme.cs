#nullable enable
using System.Drawing;

namespace ConsoleControlLibrary
{
    public class ControlColorScheme
    {
        public static Color BackColor =>
            Color.Black;

        public static Color ForeColor =>
            Color.FromArgb(0, 255, 0);

        public static Color ActiveControlBackColor =>
            Color.FromArgb(0, 127, 0);

        public static Color ActiveControlForeColor =>
            Color.FromArgb(255, 255, 127);

        public static Color DisabledForeColor =>
            Color.FromArgb(0, 127, 0);
    }
}