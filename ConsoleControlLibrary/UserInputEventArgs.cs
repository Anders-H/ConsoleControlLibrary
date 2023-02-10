using System;

namespace ConsoleControlLibrary;

public class UserInputEventArgs : EventArgs
{
    public string RawInput { get; }
        
    public UserInputEventArgs(string rawInput)
    {
        RawInput = rawInput;
    }
}