using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleControlLibrary
{
    public class History
    {
        private class HistoryString
        {
            public bool IsTemporary { get; set; }
            public string Value { get; set; }
        }
        private List<HistoryString> Strings { get; } = new List<HistoryString>();
        private int PositionPointer { get; set; }
        public void Remember(string text)
        {
            Strings.Remove(Strings.FirstOrDefault(x => string.Compare(x.Value, text, StringComparison.CurrentCultureIgnoreCase) == 0));
            Strings.Add(new HistoryString { Value = text, IsTemporary = false });
            PositionPointer = Strings.Count - 1;
            Strings.RemoveAll(x => x.IsTemporary);
        }
        public void RememberTemporary(string text)
        {
            var existing = Strings.FirstOrDefault(x => string.Compare(x.Value, text, StringComparison.CurrentCultureIgnoreCase) == 0);
            if (existing == null)
                Strings.Add(new HistoryString { Value = text, IsTemporary = true });
            else
            {
                Strings.Remove(existing);
                Strings.Add(existing);
            }
            PositionPointer = Strings.Count - 1;
        }
        public bool HasData() => Strings.Count > 0;
        public string Previous()
        {
            //TODO
            return "";
        }
        public string Next()
        {
            //TODO
            return "";
        }
    }
}
