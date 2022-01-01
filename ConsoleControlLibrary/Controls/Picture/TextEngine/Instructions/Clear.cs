#nullable enable
using System.Drawing;
using System.Text.RegularExpressions;

namespace ConsoleControlLibrary.Controls.Picture.TextEngine.Instructions
{
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
            var matchWithoutColor = Regex.Match(
                s,
                @"^CLEAR$"
            );

            if (matchWithoutColor.Success)
                return new Clear(_lastBackgroundColor);

            var matchWithColor = Regex.Match(
                s,
                @"^CLEAR\s*(#[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F])$"
            );

            if (matchWithColor.Success)
            {
                _lastBackgroundColor = ColorTranslator.FromHtml(
                    matchWithColor.Groups[1].Value
                );

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
}