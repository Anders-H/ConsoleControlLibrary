using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;

namespace ControlsTestProject
{
    public class TestForm1 : Form
    {
        public TestForm1(ConsoleControl parentConsole) : base(parentConsole)
        {
            var button1 = new Button(this, 10, 10, 10, 1, "Button 1");
            var button2 = new Button(this, 10, 11, 10, 1, "Button 2");
            var button3 = new Button(this, 10, 12, 10, 1, "Third button");
        }
    }
}
