using System.Collections.Generic;
using ConsoleControlLibrary.Controls;

namespace ConsoleControlLibrary
{
    public class Form
    {
        private List<Control> Controls { get; }
        private ConsoleControl ParentConsole { get; }
        public Form(ConsoleControl parentConsole)
        {
            ParentConsole = parentConsole;
            Controls = new List<Control>();
        }
        public void AddControl(Control control)
        {
            Controls.Add(control);
        }
    }
}
