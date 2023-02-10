using System;

namespace ConsoleControlLibrary.Controls.Events;

public class ConsoleControlEventArgs : EventArgs
{
    public ConsoleControlEventType EventType { get; }
        
    public ConsoleControlEventArgs(ConsoleControlEventType eventType)
    {
        EventType = eventType;
    }
}