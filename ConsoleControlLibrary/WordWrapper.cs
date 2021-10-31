using System;
using System.Text;

namespace ConsoleControlLibrary
{
    internal static class WordWrapper
    {
        public static string WordWrap(int columnCount, string text)
        {
            static int Break(string breakText, int breakPos, int max)
            {
                var position = max;

                while (position >= 0 && !char.IsWhiteSpace(breakText[breakPos + position]))
                    position--;
                
                if (position < 0)
                    return max;
                
                while (position >= 0 && char.IsWhiteSpace(breakText[breakPos + position]))
                    position--;
                
                return position + 1;
            }

            int wordBreak;
            var s = new StringBuilder();

            for (var charPointer = 0; charPointer < text.Length; charPointer = wordBreak)
            {
                var endOfLine = text.IndexOf("\r\n", charPointer, StringComparison.Ordinal);
                
                if (endOfLine < 0)
                    endOfLine = text.Length;
                
                wordBreak = endOfLine < 0 ? text.Length : endOfLine + 2;
                
                if (endOfLine > charPointer)
                {
                    do
                    {
                        var length = endOfLine - charPointer;
                        if (length > columnCount)
                            length = Break(text, charPointer, columnCount);
                        s.Append(text, charPointer, length);
                        s.AppendLine();
                        charPointer += length;
                        while (charPointer < endOfLine && char.IsWhiteSpace(text[charPointer]))
                            charPointer++;
                    } while (endOfLine > charPointer);

                    continue;
                }

                s.AppendLine();
            }

            return s.ToString();
        }
    }
}