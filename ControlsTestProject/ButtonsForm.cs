using System;
using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;
using ConsoleControlLibrary.Controls.Events;
using Label = ConsoleControlLibrary.Controls.Label;

namespace ControlsTestProject
{
    public class ButtonsForm : ConsoleForm
    {
        private readonly TextBlock _textBlock;
        private readonly Button _button1;
        private readonly Button _button2;
        private readonly Button _button3;
        private readonly Button _button4;
        private readonly Button _button5;

        public ButtonsForm(ConsoleControl parentConsole) : base(parentConsole)
        {
            const string text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed quis justo vel eros egestas commodo sed vel ante.";
            
            _textBlock = new TextBlock(this, 0, 0, parentConsole.ColumnCount, 7, text, 10, HorizontalTextAlignment.Top);
            AddControl(_textBlock);
            AddControl(new Label(this, 10, 8, "Five buttons:"));
            _button1 = new Button(this, 10, 10, 16, "Disable me");
            AddControl(_button1);
            _button2 = new Button(this, 10, 11, 16, "Hide me");
            AddControl(_button2);
            _button3 = new Button(this, 10, 12, 16, "Restore");
            AddControl(_button3);
            _button4 = new Button(this, 10, 13, 16, "Add text");
            AddControl(_button4);
            _button5 = new Button(this, 10, 15, 20, "To checkboxes form");
            AddControl(_button5);
            TriggerFormLoadedEvent();
        }

        protected override void EventOccurred(object sender, ConsoleControlEventArgs e)
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

            if (sender == _button4)
            {
                ParentConsole.BeginWait();
                switch (new Random().Next(0, 6))
                {
                    case 0:
                        _textBlock.Write("Hello!");
                        break;
                    case 1:
                        _textBlock.Write("Owner of a lonely heart!");
                        break;
                    case 2:
                        _textBlock.Write("Sune Mangs!");
                        break;
                    case 3:
                        _textBlock.Write("Paul Stanley!");
                        break;
                    case 4:
                        _textBlock.Write("Freddie Mercury!");
                        break;
                    case 5:
                        _textBlock.Write("While the sun hangs in the sky and the desert has sand. While the waves crash in the sea and meet the land. While there's a wind and the stars and the rainbow. Till the mountains crumble into the plain.");
                        break;
                }

                ParentConsole.EndWait();
            }

            if (sender == _button5)
            {
                ParentConsole.CurrentForm = new CheckboxesForm(ParentConsole);
            }
        }
    }
}