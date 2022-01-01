#nullable enable
using System.Drawing;
using System.Text.RegularExpressions;

namespace ConsoleControlLibrary.Controls.Picture.TextEngine
{
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
    }
}