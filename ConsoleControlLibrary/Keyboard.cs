using System.Windows.Forms;

namespace ConsoleControlLibrary;

internal static class Keyboard
{
    public static bool IsInputKey(Keys keyData) =>
        keyData switch
        {
            Keys.Shift => true,
            Keys.Right => true,
            Keys.Left => true,
            Keys.Up => true,
            Keys.Down => true,
            Keys.Shift | Keys.Right => true,
            Keys.Shift | Keys.Left => true,
            Keys.Shift | Keys.Up => true,
            Keys.Shift | Keys.Down => true,
            Keys.Tab => true,
            Keys.Shift | Keys.Tab => true,
            _ => false
        };

    public static bool IsControlKey(Keys key) =>
        key switch
        {
            Keys.Up => true,
            Keys.Down => true,
            Keys.Left => true,
            Keys.Right => true,
            Keys.Home => true,
            Keys.End => true,
            Keys.Tab => true,
            Keys.Enter => true,
            Keys.Back => true,
            Keys.Delete => true,
            Keys.PageUp => true,
            Keys.PageDown => true,
            _ => false
        };
}