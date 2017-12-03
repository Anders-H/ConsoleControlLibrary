using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;

namespace ControlsTestProject
{
    public class TestForm1 : Form
    {
        public TestForm1(ConsoleControl parentConsole, IDrawEngine drawEngine) : base(parentConsole)
        {
            var button1 = new Button(this, drawEngine, 10, 10, 10, 1, "Button 1");
            AddControl(button1);
            var button2 = new Button(this, drawEngine, 10, 11, 10, 1, "Button 2");
            AddControl(button2);
            var button3 = new Button(this, drawEngine, 10, 12, 10, 1, "Third button");
            AddControl(button3);
        }
    }
}
