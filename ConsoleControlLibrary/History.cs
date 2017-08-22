using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
#if DEBUG
            Debug.WriteLine(ToString());
#endif
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
            if (PositionPointer > 0)
                PositionPointer--;
#if DEBUG
            Debug.WriteLine(ToString());
#endif
        }
        public bool HasData() => Strings.Count > 0;
        public string Previous()
        {
            if (!HasData())
                return "";
            if (PositionPointer < 0 || PositionPointer >= Strings.Count)
                PositionPointer = Strings.Count - 1;
            var ret = Strings[PositionPointer].Value;
            if (PositionPointer > 0)
                PositionPointer--;
#if DEBUG
            Debug.WriteLine(ToString());
#endif
            return ret;
        }
        public string Next()
        {
            if (!HasData())
                return "";
            PositionPointer++;
            if (PositionPointer >= Strings.Count)
                return "";
            if (PositionPointer < 0 || PositionPointer >= Strings.Count)
                PositionPointer = 0;
#if DEBUG
            Debug.WriteLine(ToString());
#endif
            return Strings[PositionPointer].Value;
        }
        public override string ToString()
        {
            var s = new StringBuilder();
            s.AppendLine();
            s.AppendLine($"---- {Strings.Count} records ----");
            if (HasData())
                for (var i = 0; i < Strings.Count; i++)
                    s.AppendLine($"{(i == PositionPointer ? ">>" : "  ")}{(Strings[i].IsTemporary ? "*" : " ")} {i:00} {Strings[i].Value}");
            s.AppendLine();
            return s.ToString();
        }
    }
}
