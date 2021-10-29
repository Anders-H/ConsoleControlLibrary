using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;
using ConsoleControlLibrary.Controls.Events;

namespace ControlsTestProject
{
    public class TextboxesForm : ConsoleForm
    {
        private readonly Textbox _textbox1;
        private readonly Textbox _textbox2;
        private readonly Textbox _textbox3;
        private readonly Button _button;

        public TextboxesForm(ConsoleControl parentConsole) : base(parentConsole)
        {
            AddControl(new Label(this, 3, 5, "Write some text (5 characters):"));
            _textbox1 = new Textbox(this, 3, 6, 10, 5);
            AddControl(_textbox1);
            AddControl(new Label(this, 3, 7, "Write some text (40 characters):"));
            _textbox2 = new Textbox(this, 3, 8, 10, 40);
            AddControl(_textbox2);
            AddControl(new Label(this, 3, 9, "Write a number (0-100):"));
            _textbox3 = new Textbox(this, 3, 10, 10, 3);
            AddControl(_textbox3);
            _button = new Button(this, 3, 15, 17, "To buttons form");
            AddControl(_button);
        }
        protected override void EventOccured(object sender, ConsoleControlEventArgs e)
        {
            if (sender == _button)
            {
                ParentConsole.CurrentForm = new ButtonsForm(ParentConsole);
            }
        }
    }
}