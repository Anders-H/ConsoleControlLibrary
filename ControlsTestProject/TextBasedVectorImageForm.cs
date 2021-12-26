using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;
using ConsoleControlLibrary.Controls.Events;
using ConsoleControlLibrary.Controls.Picture;

namespace ControlsTestProject
{
    internal class TextBasedVectorImageForm : ConsoleForm
    {
        private readonly Button _button;

        public TextBasedVectorImageForm(ConsoleControl parentConsole) : base(parentConsole)
        {
            _button = new Button(this, 10, 30, 20, "To buttons form");
            AddControl(_button);
            var picture = new ClientPicture(this, 0, 0, 90, 20);
            AddControl(picture);
            var _ = new TextBasedVectorImage(parentConsole.DrawEngine, picture, 320, 200, @"


");
        }

        protected override void EventOccurred(object sender, ConsoleControlEventArgs e)
        {
            if (sender == _button)
            {
                ParentConsole.CurrentForm = new ButtonsForm(ParentConsole);
            }
        }
    }
}