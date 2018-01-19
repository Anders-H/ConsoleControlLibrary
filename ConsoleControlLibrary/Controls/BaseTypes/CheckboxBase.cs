using System;
using System.Drawing;
using System.Windows.Forms;
using ConsoleControlLibrary.Controls.Events;

namespace ConsoleControlLibrary.Controls.BaseTypes
{
    public abstract class CheckboxBase : ControlBase
    {
        private string _text;
        private string _visibleText;
        private bool _checked;
        protected CheckboxBase(ConsoleForm parentForm, bool isChecked, int x, int y, int width, int height, string text) : base(parentForm, x, y, width, height)
        {
            if (width < 3)
                throw new ArgumentOutOfRangeException(nameof(width));
            Checked = isChecked;
            Text = text ?? "";
            CanGetFocus = true;
            Enabled = true;
            Visible = true;
        }
        protected CheckboxBase(ConsoleForm parentForm, bool isChecked, int x, int y, int width, string text) : this(parentForm, isChecked, x, y, width, 1, text) { }
        protected CheckboxBase(ConsoleForm parentForm, bool isChecked, int x, int y, string text) : this(parentForm, isChecked, x, y, text.Length + 3, 1, text) { }
        public bool Checked
        {
            get => _checked;
            set
            {
                if (value == _checked)
                    return;
                _checked = value;
                _visibleText = null;
                Invalidate();
                CheckedChanged();
            }
        }
        protected virtual void CheckedChanged() { }
        public string Text
        {
            get => _text;
            set
            {
                _text = value ?? "";
                Invalidate();
            }
        }
        public override void KeyPressed(Keys key)
        {
            if (key != Keys.Enter)
                return;
            Checked = !Checked;
            ParentForm.TriggerEvent(this, new ConsoleControlEventArgs(ConsoleControlEventType.CheckChange));
        }
        public override void CharacterInput(char c) { }
        protected abstract char LeftBracket { get; }
        protected abstract char RightBracket { get; }
        private string VisibleText => _visibleText ?? (_visibleText = $"{LeftBracket}{(Checked ? "X" : " ")}{RightBracket}" + (_text.Length <= Width - 3 ? _text : _text.Substring(0, Width - 3)));
        public override void Draw(Graphics g, IDrawEngine drawEngine)
        {
            if (Width <= 0)
                return;
            if (Enabled)
            {
                for (var i = 0; i < VisibleText.Length; i++)
                {
                    if (i == 1 && HasFocus && ConsoleControl.CursorBlink)
                    {
                        drawEngine.DrawCursor(g, ParentForm.ForeColorBrush, X + 1, Y);
                        drawEngine.DrawCharacter(g, VisibleText[i], ParentForm.Font, ParentForm.BackColorBrush, X + i, Y);
                        continue;
                    }
                    drawEngine.DrawCharacter(g, VisibleText[i], ParentForm.Font, ParentForm.ForeColorBrush, X + i, Y);
                }
                return;
            }
            for (var i = 0; i < VisibleText.Length; i++)
            {
                drawEngine.DrawCharacter(g, VisibleText[i], ParentForm.Font, ParentForm.DisabledForeColorBrush, X + i, Y);
            }
        }
    }
}
