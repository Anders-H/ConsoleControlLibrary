#nullable enable
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
            _text = "";
            _visibleText = "";
            Text = text ?? "";
            CanGetFocus = true;
            Enabled = true;
            Visible = true;
        }

        public Button(ConsoleForm parentForm, int x, int y, int width, string text)
            : this(parentForm, x, y, width, 1, text)
        {
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value ?? "";

                if (Width > 2)
                    _visibleText = $"[{EnsureLength(_text, Width - 2)}]";
                else
                    _visibleText = _text.Length <= Width
                        ? _text
                        : _text.Substring(0, Width);

                Invalidate();
            }
        }

        public string EnsureLength(string s, int length)
        {
            if (s.Length == length)
                return s;

            if (s.Length > length)
                return s.Substring(0, length);

            while (s.Length < length)
                s += " ";

            return s;
        }

        public override void KeyPressed(Keys key)
        {
            if (key != Keys.Enter)
                return;

            ParentForm.TriggerEvent(this, new ConsoleControlEventArgs(ConsoleControlEventType.Click));
        }

        public override void CharacterInput(char c)
        {
        }

        public override void Draw(Graphics g, IDrawEngine drawEngine)
        {
            if (Width <= 0)
                return;

            if (_visibleText.Length <= 0)
                return;

            if (ParentForm.Font == null)
                return;

            if (Enabled)
            {
                var activeNow = ConsiderAsActiveNow();

                var foreColor = activeNow
                    ? ParentForm.ActiveControlForeColor
                    : ParentForm.ForeColorBrush;

                var backColor = activeNow
                    ? ParentForm.ActiveControlBackColor
                    : ParentForm.BackColorBrush;

                drawEngine.FillControl(g, backColor, new Rectangle(X, Y, Width, Height));

                for (var i = 0; i < _visibleText.Length; i++)
                {
                    if (i == 0 && HasFocus && ConsoleControl.CursorBlink)
                    {
                        drawEngine.DrawCursor(g, ParentForm.ForeColorBrush, X, Y);
                        drawEngine.DrawCharacter(g, _visibleText[i], ParentForm.Font, foreColor, X + i, Y);
                        continue;
                    }

                    drawEngine.DrawCharacter(g, _visibleText[i], ParentForm.Font, foreColor, X + i, Y);
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