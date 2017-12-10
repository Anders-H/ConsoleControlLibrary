using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;
using ConsoleControlLibrary.Controls.Events;

namespace ControlsTestProject
{
    public class CheckboxesForm : ConsoleForm
    {
        private readonly Checkbox _checkbox1;
        private readonly Checkbox _checkbox2;
        private readonly Button _button;
        public CheckboxesForm(ConsoleControl parentConsole, IDrawEngine drawEngine) : base(parentConsole)
        {
            AddControl(new Label(this, drawEngine, 10, 8, "Two checkboxes and a button:"));
            _checkbox1 = new Checkbox(this, drawEngine, true, 10, 10, "Enable button");
            AddControl(_checkbox1);
            _checkbox2 = new Checkbox(this, drawEngine, true, 10, 11, "Show button");
            AddControl(_checkbox2);
            _button = new Button(this, drawEngine, 10, 13, "To buttons form");
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
