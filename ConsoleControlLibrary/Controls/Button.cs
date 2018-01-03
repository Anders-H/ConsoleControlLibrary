using System.Drawing;
using System.Windows.Forms;
using ConsoleControlLibrary.Controls.BaseTypes;
using ConsoleControlLibrary.Controls.Events;

namespace ConsoleControlLibrary.Controls
{
    public class Button : ControlBase, IControl, IControlFormOperations, ITextControl
    {
        private string _text;
        private string _visibleText;
        public Button(ConsoleForm parentForm, int x, int y, int width, int height, string text) : base(parentForm, x, y, width, height)
        {
            Text = text ?? "";
            CanGetFocus = true;
            Enabled = true;
            Visible = true;
        }
        public Button(ConsoleForm parentForm, int x, int y, int width, string text) : this(parentForm, x, y, width, 1, text) { }
        public Button(ConsoleForm parentForm, int x, int y, string text) : this(parentForm, x, y, text.Length, 1, text) { }
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
        public override void KeyPressed(Keys key)
        {
            if (key != Keys.Enter)
                return;
            ParentForm.TriggerEvent(this, new ConsoleControlEventArgs(ConsoleControlEventType.Click));
        }
        public override void Draw(Graphics g, IDrawEngine drawEngine)
        {
            if (Width <= 0 || _visibleText.Length <= 0)
                return;
            if (Enabled)
            {
                for (var i = 0; i < _visibleText.Length; i++)
                {
                    if (i == 0 && HasFocus && ConsoleControl.CursorBlink)
                    {
                        drawEngine.DrawCursor(g, ParentForm.ForeColorBrush, X, Y);
                        drawEngine.DrawCharacter(g, _visibleText[i], ParentForm.Font, ParentForm.BackColorBrush, X + i, Y);
                        continue;
                    }
                    drawEngine.DrawCharacter(g, _visibleText[i], ParentForm.Font, ParentForm.ForeColorBrush, X + i, Y);
                }
                return;
            }
            for (var i = 0; i < _visibleText.Length; i++)
            {
                drawEngine.DrawCharacter(g, _visibleText[i], ParentForm.Font, ParentForm.DisabledForeColorBrush, X + i, Y);
            }
        }
    }
}
