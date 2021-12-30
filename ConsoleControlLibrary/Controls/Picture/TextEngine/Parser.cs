#nullable enable
using System;
using System.Text.RegularExpressions;
using ConsoleControlLibrary.Controls.Picture.TextEngine.Instructions;

namespace ConsoleControlLibrary.Controls.Picture.TextEngine
{
    internal class Parser
    {
        private readonly string _source;

        public Parser(string source)
        {
            _source = source;
        }

        public DrawInstructionList Parse()
        {
            var result = new DrawInstructionList();
            var rows = Regex.Split(_source, ";", RegexOptions.IgnoreCase);

            foreach (var row in rows)
            {
                var r = row
                    .Trim()
                    .ToUpper();

                if (string.IsNullOrEmpty(r) || r.StartsWith("//"))
                    continue;

                var i = Clear.Parse(r);
                if (i != null)
                {
                    result.Add(i);
                    continue;
                }

                throw new SystemException($"Syntax error: {r}");
            }

            return result;
        }
    }
}