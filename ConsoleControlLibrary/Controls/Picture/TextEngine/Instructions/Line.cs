using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace ConsoleControlLibrary.Controls.Picture.TextEngine.Instructions;

public class Line : DrawInstruction
{
    private static Color _lastForegroundColor;

    private readonly Color _foregroundColor;
    private readonly List<Point> _points;

    static Line()
    {
        _lastForegroundColor = Color.Red;
    }

    public Line(Color foregroundColor)
    {
        _foregroundColor = foregroundColor;
        _points = new List<Point>();
    }

    public static Line? Parse(string s)
    {
        var regexWithoutColor = RegexBuilder.Get(
            RegexBuilder.Line,
            RegexBuilder.CoordinatesGroup
        );

        var matchWithoutColor = Regex.Match(s, regexWithoutColor);

        if (matchWithoutColor.Success)
        {
            var result = new Line(_lastForegroundColor);

            foreach (Capture capture in matchWithoutColor.Groups[1].Captures)
                result._points.Add(ParsePoint(capture));

            return result;
        }

        var regexWithColor = RegexBuilder.Get(
            RegexBuilder.Line,
            RegexBuilder.Color,
            RegexBuilder.CoordinatesGroup
        );

        var matchWithColor = Regex.Match(s, regexWithColor);

        if (matchWithColor.Success)
        {
            _lastForegroundColor = ParseColor(matchWithColor.Groups[1]);
            var result = new Line(_lastForegroundColor);

            foreach (Capture capture in matchWithColor.Groups[2].Captures)
                result._points.Add(ParsePoint(capture));

            return result;
        }

        return null;
    }

    public override void Draw(ClientPicture picture, Graphics g, VectorImageBase owner)
    {
        if (_points.Count < 2)
            return;

        using var p = new Pen(_foregroundColor);

        for (var i = 1; i < _points.Count; i++)
            g.DrawLine(
                p,
                owner.VirtualToPhysical(_points[i - 1]),
                owner.VirtualToPhysical(_points[i])
            );
    }
}