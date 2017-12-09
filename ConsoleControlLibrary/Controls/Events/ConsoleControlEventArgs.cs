using System;
using ConsoleControlLibrary.Controls.Events;

namespace ConsoleControlLibrary.Controls.Events
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
