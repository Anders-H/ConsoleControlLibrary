using System.Drawing;
using System.Text.RegularExpressions;

namespace ConsoleControlLibrary.Controls.Picture.TextEngine;

public abstract class DrawInstruction
{
    public abstract void Draw(
        ClientPicture picture,
        Graphics g,
        VectorImageBase owner
    );

    protected static Point ParsePoint(Capture capture) =>
        ParsePoint(capture.Value);

    protected static Point ParsePoint(string value)
    {
        value = value.Trim();

        if (value.StartsWith('('))
            value = value.Substring(1).Trim();

        if (value.EndsWith('-'))
            value = value.Substring(0, value.Length - 1).Trim();

        if (value.EndsWith(')'))
            value = value.Substring(0, value.Length - 1).Trim();

        var parts = value.Split(',');

        return new Point(
            int.Parse(parts[0].Trim()),
            int.Parse(parts[1].Trim())
        );
    }

    protected static Color ParseColor(Group group) =>
        ParseColor(group.Value);

    protected static Color ParseColor(string htmlValue) =>
        ColorTranslator.FromHtml(htmlValue);

    protected static Rectangle ParseRectangle(Group group) =>
        ParseRectangle(group.Value);

    protected static Rectangle ParseRectangle(string value)
    {
        value = value.Trim();

        if (value.StartsWith('('))
            value = value.Substring(1).Trim();

        if (value.EndsWith('-'))
            value = value.Substring(0, value.Length - 1).Trim();

        if (value.EndsWith(')'))
            value = value.Substring(0, value.Length - 1).Trim();

        var parts = value.Split(',');

        return new Rectangle(
            int.Parse(parts[0].Trim()),
            int.Parse(parts[1].Trim()),
            int.Parse(parts[2].Trim()),
            int.Parse(parts[3].Trim())
        );
    }
}