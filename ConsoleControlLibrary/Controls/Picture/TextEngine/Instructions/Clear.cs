using System.Drawing;
using System.Text.RegularExpressions;

namespace ConsoleControlLibrary.Controls.Picture.TextEngine.Instructions;

public class Clear : DrawInstruction
{
    private static Color _lastBackgroundColor;

    private readonly Color _backgroundColor;

    static Clear()
    {
        _lastBackgroundColor = Color.Green;
    }

    public Clear(Color backgroundColor)
    {
        _backgroundColor = backgroundColor;
    }

    public static Clear? Parse(string s)
    {
        var regexWithoutColor = RegexBuilder.Get(
            RegexBuilder.Clear
        );

        var matchWithoutColor = Regex.Match(s, regexWithoutColor);

        if (matchWithoutColor.Success)
            return new Clear(_lastBackgroundColor);

        var regexWithColor = RegexBuilder.Get(
            RegexBuilder.Clear,
            RegexBuilder.Color
        );

        var matchWithColor = Regex.Match(s, regexWithColor);

        if (matchWithColor.Success)
        {
            _lastBackgroundColor = ParseColor(matchWithColor.Groups[1]);
            return new Clear(_lastBackgroundColor);
        }

        return null;
    }

    public override void Draw(ClientPicture picture, Graphics g, VectorImageBase owner)
    {
        using var b = new SolidBrush(_backgroundColor);
        g.FillRectangle(b, owner.VirtualToPhysical(0, 0, owner.VirtualWidth, owner.VirtualHeight));
    }
}