using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;

namespace ControlsTestProject
{
    public class ButtonsForm : ConsoleForm
    {
        private Button _button1;
        private Button _button2;
        private Button _button3;
        private Button _button4;
        public ButtonsForm(ConsoleControl parentConsole, IDrawEngine drawEngine) : base(parentConsole)
        {
            _button1 = new Button(this, drawEngine, 10, 10, "Disable me");
            AddControl(_button1);
            _button2 = new Button(this, drawEngine, 10, 11, "Hide me");
            AddControl(_button2);
            _button3 = new Button(this, drawEngine, 10, 12, "Restore");
            AddControl(_button3);
            _button4 = new Button(this, drawEngine, 10, 13, "To checkboxes form");
            AddControl(_button4);
        }
        protected override void EventOccured(object sender, ConsoleControlEventArgs e)
        {
            if (sender == _button1)
            {
                _button1.Enabled = false;
                return;
            }
            if (sender == _button2)
            {
                _button2.Visible = false;
                return;
            }
            if (sender == _button3)
            {
                _button1.Enabled = true;
                _button2.Visible = true;
                return;
            }
        }
    }
}
