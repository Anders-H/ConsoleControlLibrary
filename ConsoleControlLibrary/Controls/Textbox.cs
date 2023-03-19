using System.Drawing;
using System.Windows.Forms;
using ConsoleControlLibrary.Controls.BaseTypes;
using ConsoleControlLibrary.Controls.Events;

namespace ConsoleControlLibrary.Controls;

public class TextBox : ControlBase, IControl, IControlFormOperations, ITextControl
{
    private int _displayOffset;
    private int _cursorX;
    private readonly char[] _characters;
    public int MaxLength { get; }

    public TextBox(ConsoleForm parentForm, int x, int y, int width, int maxLength) : base(parentForm, x, y, width, 1)
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
            case Keys.Left:
                if (_cursorX > 0)
                {
                    _cursorX--;
                    Invalidate();
                }
                break;
            case Keys.Right:
                if (_cursorX < MaxLength - 1 && _cursorX < LastCharacterIndex + 1)
                {
                    _cursorX++;
                    Invalidate();
                }
                break;
            case Keys.Back:
                BackspaceAt(_cursorX);
                if (_cursorX > 0)
                    _cursorX--;
                Invalidate();
                break;
            case Keys.Insert:
                InsertAt(_cursorX);
                Invalidate();
                break;
            case Keys.Delete:
                BackspaceAt(_cursorX + 1);
                Invalidate();
                break;
            case Keys.Enter:
                ParentForm.TriggerEvent(
                    this,
                    new ConsoleControlEventArgs(ConsoleControlEventType.TextBoxEnter)
                );
                break;
        }
    }

    private void BackspaceAt(int col)
    {
        if (col <= 0)
            return;

        for (var c = col - 1; c < MaxLength; c++)
            _characters[c] = c < MaxLength - 1
                ? _characters[c + 1]
                : (char)0;
    }

    private int LastCharacterIndex
    {
        get
        {
            for (var c = MaxLength - 1; c >= 0; c--)
                if (_characters[c] != (char)0 && _characters[c] != ' ')
                    return c;

            return -1;
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
            _cursorX = MaxLength - 1;
    }

    private void InsertAt(int col)
    {
        if (col == MaxLength - 1)
            _characters[MaxLength - 1] = (char)0;

        for (var i = MaxLength - 1; i > col; i--)
            _characters[i] = _characters[i - 1];
            
        _characters[col] = (char)0;
    }

    public override void Draw(Graphics g, IDrawEngine drawEngine)
    {
        if (Width <= 0)
            return;

        if (ParentForm.Font == null)
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
                    drawEngine.DrawCursor(g, ParentForm.CurrentColorScheme!.ForeColor, cursorX, Y);
                    
                for (var i = 0; i < drawCharacters.Length; i++)
                {
                    var x = X + i;
                    drawEngine.DrawCharacter(g, drawCharacters[i], ParentForm.Font, ParentForm.CurrentColorScheme!.ForeColor, x, Y);
                }
            }
            else
            {
                for (var i = 0; i < drawCharacters.Length; i++)
                {
                    var x = X + i;
                    drawEngine.DrawCharacter(g, drawCharacters[i], ParentForm.Font, ParentForm.CurrentColorScheme!.ForeColor, x, Y);
                }
            }
        }
        else
        {
            for (var i = 0; i < drawCharacters.Length; i++)
            {
                var x = X + i;
                drawEngine.DrawCharacter(g, drawCharacters[i], ParentForm.Font, ParentForm.CurrentColorScheme!.DisabledForeColor, x, Y);
            }
        }
    }
        
    public string Text
    {
        get => (new string(_characters)).Trim();
        set
        {
            for (var i = 0; i < MaxLength; i++)
                _characters[i] = (char)0;

            _cursorX = 0;

            var index = 0;
            foreach (var c in value.ToCharArray())
            {
                if (index >= MaxLength)
                    return;

                _characters[index] = c;
                index++;
            }
        }
    }
}