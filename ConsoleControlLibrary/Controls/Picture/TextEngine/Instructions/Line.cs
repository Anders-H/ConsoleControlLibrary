using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable enable

namespace ConsoleControlLibrary.Controls.Picture.TextEngine.Instructions
{
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
            var matchWithoutColor = Regex.Match(
                s,
                @"^LINE\s*(\([0-9]+,[0-9]+\)\s*-?\s*)*$"
            );

            if (matchWithoutColor.Success)
            {
                var result = new Line(_lastForegroundColor);

                foreach (Capture capture in matchWithoutColor.Groups[1].Captures)
                    result._points.Add(ParsePoint(capture));

                return result;
            }

            var matchWithColor = Regex.Match(
                s,
                @"^LINE\s*(#[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F])\s*(\([0-9]+,[0-9]+\)\s*-?\s*)*$"
            );

            if (matchWithColor.Success)
            {
                _lastForegroundColor = ColorTranslator.FromHtml(
                    matchWithColor.Groups[1].Value
                );

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
}