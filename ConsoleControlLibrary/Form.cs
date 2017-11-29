using System.Collections.Generic;

namespace ConsoleControlLibrary
{
    public class Form
    {
        private List<Control> Controls { get; }
        public Form()
        {
            Controls = new List<Control>();
        }
    }
}
