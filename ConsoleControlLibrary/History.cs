#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleControlLibrary
{
    public class History
    {
        private class HistoryString
        {
            public HistoryString()
            {
                IsTemporary = false;
                Value = "";
            }

            public bool IsTemporary { get; init; }
            public string Value { get; init; }
        }
        
        private List<HistoryString> Strings { get; }
        private int PositionPointer { get; set; }

        public History()
        {
            Strings = new();
        }

        public void Remember(string text)
        {
            if (Strings.FirstOrDefault(x => string.Compare(x.Value, text, StringComparison.CurrentCultureIgnoreCase) == 0) != null)
                return;
            
            Strings.Add(
                new HistoryString
                {
                    Value = text,
                    IsTemporary = false
                }
            );
            
            PositionPointer = Strings.Count - 1;
            
            Strings.RemoveAll(x => x.IsTemporary);
        }
        
        public void RememberTemporary(string text)
        {
            var existing = Strings.FirstOrDefault(x =>
                string.Compare(x.Value, text, StringComparison.CurrentCultureIgnoreCase) == 0
            );

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
        }
        
        public bool HasData() =>
            Strings.Count > 0;
        
        public string Previous()
        {
            if (!HasData())
                return "";

            if (PositionPointer < 0 || PositionPointer >= Strings.Count)
                PositionPointer = Strings.Count - 1;
            
            var ret = Strings[PositionPointer].Value;
            
            if (PositionPointer > 0)
                PositionPointer--;
            
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