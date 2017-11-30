using System.Windows.Forms;

namespace ConsoleControlLibrary.Controls
{
    public class Button : ControlBase
    {
        private string _text;
        public Button(Form parentForm, int x, int y, int width, int height, string text) : base(parentForm, x, y, width, height)
        {
            Text = text;
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                Invalidate();
            }
        }
        protected internal override void KeyPressInfo(Keys key)
        {
            if (key == Keys.Enter)
            {
                ParentForm.TriggerEvent(this, new ConsoleControlEventArgs(ConsoleControlEventType.Click));
                return;
            }
        }
    }
}
