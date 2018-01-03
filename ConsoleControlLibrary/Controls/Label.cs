using System.Drawing;
using System.Windows.Forms;
using ConsoleControlLibrary.Controls.BaseTypes;

namespace ConsoleControlLibrary.Controls
{
    public class Label : ControlBase, IControl, ITextControl
    {
        private string _text;
        private string _visibleText;
        public Label(ConsoleForm parentForm, int x, int y, int width, int height, string text) : base(parentForm, x, y, width, height)
        {
            Text = text ?? "";
            CanGetFocus = false;
            Enabled = true;
            Visible = true;
        }
        public Label(ConsoleForm parentForm, int x, int y, int width, string text) : this(parentForm, x, y, width, 1, text) { }
        public Label(ConsoleForm parentForm, int x, int y, string text) : this(parentForm, x, y, text.Length, 1, text) { }
        public string Text
        {
            get => _text;
            set
            {
                _text = value ?? "";
                _visibleText = _text.Length <= Width ? _text : _text.Substring(0, Width);
                Invalidate();
            }
        }
        protected internal override void KeyPressed(Keys key) { }
        protected internal override void Draw(Graphics g, IDrawEngine drawEngine)
        {
            if (Width <= 0 || _visibleText.Length <= 0)
                return;
            for (var i = 0; i < _visibleText.Length; i++)
                drawEngine.DrawCharacter(g, _visibleText[i], ParentForm.Font, ParentForm.DisabledForeColorBrush, X + i, Y);
        }

    }
}
