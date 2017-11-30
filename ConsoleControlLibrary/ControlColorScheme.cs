using System.Drawing;

namespace ConsoleControlLibrary
{
    public class ControlColorScheme
    {
        public static Color BackgroundColor => Color.Black;
        public static Color ForeColor => Color.FromArgb(0, 255, 0);
        public static Color ActiveControlBackColor => Color.FromArgb(0, 0, 100);
        public static Color DisabledForeColor => Color.FromArgb(0, 127, 0);
    }
}
