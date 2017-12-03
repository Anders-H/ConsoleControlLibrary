using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;

namespace ControlsTestProject
{
    public class ButtonsForm : ConsoleForm
    {
        public ButtonsForm(ConsoleControl parentConsole, IDrawEngine drawEngine) : base(parentConsole)
        {
            var button1 = new Button(this, drawEngine, 10, 10, 12, 1, "Button 1");
            AddControl(button1);
            var button2 = new Button(this, drawEngine, 10, 11, 12, 1, "Button 2");
            AddControl(button2);
            var button3 = new Button(this, drawEngine, 10, 12, 12, 1, "Third button");
            AddControl(button3);
            var button4 = new Button(this, drawEngine, 10, 13, 12, 1, "Checkboxes");
            AddControl(button4);
        }
    }
}
