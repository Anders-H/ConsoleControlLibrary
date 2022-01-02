#nullable enable
using System.Text;

namespace ConsoleControlLibrary.Controls.Picture.TextEngine
{
    internal static class RegexBuilder
    {
        private const string Start = "^";
        private const string End = "$";

        public const string Box = @"(BOX(-FILLED)?)\s*";
        public const string Clear = @"CLEAR\s*";
        public const string Line = @"LINE\s*";
        public const string Color = @"(#[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F])\s*";
        public const string CoordinatesGroup = @"(\(\s*[0-9]+\s*,\s*[0-9]+\s*\)\s*-?\s*)*";
        public const string Rectangle = @"(\(\s*[0-9]+\s*,\s*[0-9]+\s*,\s*[0-9]+\s*,\s*[0-9]+\s*\))";

        public static string Get(params string[] parts)
        {
            var s = new StringBuilder();
            s.Append(Start);
            foreach (var part in parts)
                s.Append(part);
            s.Append(End);
            return s.ToString();
        }
    }
}