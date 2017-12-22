using System.Text.RegularExpressions;
using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;
using ConsoleControlLibrary.Controls.Events;
using Button = ConsoleControlLibrary.Controls.Button;
using Label = ConsoleControlLibrary.Controls.Label;

namespace ControlsTestProject
{
    public class CheckboxesForm : ConsoleForm
    {
        private readonly Checkbox _checkbox1;
        private readonly Checkbox _checkbox2;
        private readonly Button _button;
        public CheckboxesForm(ConsoleControl parentConsole) : base(parentConsole)
        {
            AddControl(new Label(this, 10, 8, "Two checkboxes and a button:"));
            _checkbox1 = new Checkbox(this, true, 10, 10, "Enable button");
            AddControl(_checkbox1);
            _checkbox2 = new Checkbox(this, true, 10, 11, "Show button");
            AddControl(_checkbox2);
            AddControl(new Radiobutton(this, false, "group", 10, 12, "Enable button"));
            AddControl(new Radiobutton(this, false, "group", 10, 13, "Show button"));
            _button = new Button(this, 10, 15, "To buttons form");
            AddControl(_button);
        }
        protected override void EventOccured(object sender, ConsoleControlEventArgs e)
        {
            if (sender == _checkbox1)
            {
                _button.Enabled = _checkbox1.Checked;
                return;
            }
            if (sender == _checkbox2)
            {
                _button.Visible = _checkbox2.Checked;
                return;
            }
            if (sender == _button)
            {
                ParentConsole.CurrentForm = new ButtonsForm(ParentConsole, ParentConsole.DrawEngine);
            }
        }
    }
}
