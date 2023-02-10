using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace ConsoleControlLibrary.Controls.Picture.TextEngine.Instructions;

public class Box : DrawInstruction
{
    private static Color _lastForegroundColor;

    private readonly bool _filled;
    private readonly Color _foregroundColor;
    private readonly Rectangle _rectangle;

    static Box()
    {
        _lastForegroundColor = Color.Red;
    }

    public Box(bool filled, Color color, Rectangle rectangle)
    {
        _filled = filled;
        _foregroundColor = color;
        _rectangle = rectangle;
    }

    public static Box? Parse(string s)
    {
        var regexWithoutColor = RegexBuilder.Get(
            RegexBuilder.Box,
            RegexBuilder.Rectangle
        );

        var matchWithoutColor = Regex.Match(s, regexWithoutColor);

        if (matchWithoutColor.Success)
        {
            var filled = matchWithoutColor
                .Groups[1]
                .Value
                .IndexOf("-filled", StringComparison.CurrentCultureIgnoreCase) > -1;

            var rectangle = ParseRectangle(matchWithoutColor.Groups[3]);

            return new Box(filled, _lastForegroundColor, rectangle);
        }

        var regexWithColor = RegexBuilder.Get(
            RegexBuilder.Box,
            RegexBuilder.Color,
            RegexBuilder.Rectangle
        );

        var matchWithColor = Regex.Match(s, regexWithColor);

        if (matchWithColor.Success)
        {
            var filled = matchWithColor
                .Groups[1]
                .Value
                .IndexOf("-filled", StringComparison.CurrentCultureIgnoreCase) > -1;

            _lastForegroundColor = ParseColor(matchWithColor.Groups[3]);

            var rectangle = ParseRectangle(matchWithColor.Groups[4]);

            return new Box(filled, _lastForegroundColor, rectangle);
        }

        return null;
    }

    public override void Draw(ClientPicture picture, Graphics g, VectorImageBase owner)
    {
        if (_filled)
        {
            using var b = new SolidBrush(_foregroundColor);
            g.FillRectangle(b, owner.VirtualToPhysical(_rectangle));
            return;
        }

        using var p = new Pen(_foregroundColor);
        g.DrawRectangle(p, owner.VirtualToPhysical(_rectangle));
    }
}