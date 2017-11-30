using System;

namespace ConsoleControlLibrary.Controls
{
    public class ConsoleControlEventArgs : EventArgs
    {
        public ConsoleControlEventType EventType { get; }
        public ConsoleControlEventArgs(ConsoleControlEventType eventType)
        {
            EventType = eventType;
        }
    }
}
