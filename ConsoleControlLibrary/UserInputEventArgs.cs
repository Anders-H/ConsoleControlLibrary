using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleControlLibrary
{
    public class UserInputEventArgs : EventArgs
    {
        public string RawInput { get; }
        public UserInputEventArgs(string rawInput)
        {
            RawInput = rawInput;
        }
    }
}
