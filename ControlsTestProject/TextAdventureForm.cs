using System;
using System.Threading;
using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;
using ConsoleControlLibrary.Controls.Events;

namespace ControlsTestProject
{
    internal class TextAdventureForm : ConsoleForm
    {
        private readonly ClientPicture _picture;
        private readonly TextBlock _output;
        private readonly Textbox _input;

        public TextAdventureForm(ConsoleControl parentConsole) : base(parentConsole)
        {
            _picture = new ClientPicture(this, 0, 0, 90, 20);
            AddControl(_picture);
            _output = new TextBlock(this, 0, 20, 90, 19, "", 20, HorizontalTextAlignment.Bottom);
            AddControl(_output);
            _input = new Textbox(this, 0, 39, 90, 90);
            AddControl(_input);
            TriggerFormLoadedEvent();
        }

        protected override void EventOccurred(object sender, ConsoleControlEventArgs e)
        {
            switch (e.EventType)
            {
                case ConsoleControlEventType.FormLoaded:
                    _output.Write("Type QUIT to return to the buttons form.");
                    break;
                case ConsoleControlEventType.TextboxEnter:
                    _output.Add(_input.Text);
                    _input.Text = "";
                    ParentConsole.Refresh();
                    Thread.Sleep(100);

                    switch (DateTime.Now.Second % 4)
                    {
                        case 0:
                            _output.Write("Yes.");
                            break;
                        case 1:
                            _output.Write("Perhaps.");
                            break;
                        case 2:
                            _output.Write("No.");
                            break;
                        case 3:
                            _output.Write("No way!");
                            break;
                    }
                    break;
            }
        }
    }
}