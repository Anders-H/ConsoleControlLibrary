using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ConsoleControlLibrary.Controls.BaseTypes;

namespace ConsoleControlLibrary.Controls
{
    public class Textbox : ControlBase, IControl, IControlFormOperations, ITextControl
    {
        private int _displayOffset;
        private int _cursorX;
        private char[] _characters;
        public int MaxLength { get; }
        public Textbox(ConsoleForm parentForm, int x, int y, int width, int maxLength) : base(parentForm, x, y, width, 1)
        {
            MaxLength = maxLength;
            _characters = new char[maxLength];
            Enabled = true;
            Visible = true;
            CanGetFocus = true;
        }
        public override void KeyPressed(Keys key)
        {
            switch (key)
            {
                    
            }
        }
        public override void CharacterInput(char c)
        {
            SetChar(c);
            Invalidate();
        }
        private void SetChar(char c)
        {
            Insert();
            _characters[_cursorX] = c;
            if (_cursorX < MaxLength - 1)
                _cursorX++;
            if (_cursorX >= _displayOffset + Width)
                _displayOffset = _cursorX - Width + 1;

        }
        private void Insert()
        {
            if (_cursorX >= MaxLength - 1)
            {
                _cursorX = MaxLength - 1;
                return;
            }
        }
        public override void Draw(Graphics g, IDrawEngine drawEngine)
        {
            if (Width <= 0)
                return;
            var drawCharacters = new char[Width];
            for (var i = 0; i < Width; i++)
            {
                var source = i + _displayOffset;
                drawCharacters[i] = source < _characters.Length ? _characters[i + _displayOffset] : (char)0;
            }
            if (Enabled)
            {
                if (HasFocus)
                {
                    var cursorX = _cursorX + X - _displayOffset;
                    cursorX = cursorX > X + Width - 1 ? X + Width - 1 : cursorX;
                    if (ConsoleControl.CursorBlink)
                        drawEngine.DrawCursor(g, ParentForm.ForeColorBrush, cursorX, Y);
                    for (var i = 0; i < drawCharacters.Length; i++)
                    {
                        var x = X + i;
                        drawEngine.DrawCharacter(g, drawCharacters[i], ParentForm.Font, ParentForm.ForeColorBrush, x, Y);
                    }
                }
                else
                {
                    for (var i = 0; i < drawCharacters.Length; i++)
                    {
                        var x = X + i;
                        drawEngine.DrawCharacter(g, drawCharacters[i], ParentForm.Font, ParentForm.ForeColorBrush, x, Y);
                    }
                }
            }
            else
            {
                for (var i = 0; i < drawCharacters.Length; i++)
                {
                    var x = X + i;
                    drawEngine.DrawCharacter(g, drawCharacters[i], ParentForm.Font, ParentForm.DisabledForeColorBrush, x, Y);
                }
            }
        }
        public string Text
        {
            get => _characters.ToString().Trim();
            set
            {
                Text = "";
                var index = 0;
                foreach (var c in (value ?? "").ToCharArray())
                {
                    if (index >= MaxLength)
                        return;
                    _characters[index] = c;
                    index++;
                }
            }
        }
    }
}
