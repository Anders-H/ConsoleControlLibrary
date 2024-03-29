﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleControlLibrary.Controls;
using ConsoleControlLibrary.Controls.BaseTypes;
using ConsoleControlLibrary.Controls.Events;
using WinForms = System.Windows.Forms;

namespace ConsoleControlLibrary;

public partial class ConsoleControl : WinForms.UserControl
{
    private char[,]? _characterArray;
    private int _columnCount = 40;
    private int _rowCount = 20;
    private int _cursorPosition;
    private Font? _font;
    private bool _needsRecalcSize;
    private bool _hasFocus;
    private History History { get; }
    private bool RowChanged { get; set; }
    private bool ShiftKey { get; set; }
    private bool _waitMode;
    private readonly Mouse _mouse;
    internal IControlColorScheme? DefaultColorScheme { get; set; }
    internal static bool CursorBlink { get; set; }
    public State State { get; }
    public event EventHandler? CurrentFormChanged;
    public event UserInputHandler? UserInput;
    public event ConsoleControlEventHandler? ControlEvent;
    public IDrawEngine DrawEngine { get; set; } = new DrawEngine();

    public ConsoleControl()
    {
        State = new State();
        History = new History();
        InitializeComponent();
        InitializeConsole();
        _waitMode = false;
        _mouse = new Mouse();
        _font = new Font("Courier New", 6.0f);
    }

    public void SetDefaultColorScheme(IControlColorScheme colorScheme) =>
        DefaultColorScheme = colorScheme;

    public void HideCursor() =>
        CursorBlink = false;
        
    public Font? GetConsoleFont() =>
        _font;

    public void SetText(int row, int col, string text)
    {
        if (col < 0 || col >= ColumnCount || row < 0 || row >= RowCount || string.IsNullOrEmpty(text) || text.Length <= 0)
            return;

        for (var i = 0; i < text.Length; i++)
        {
            var x = i + col;
            if (x >= ColumnCount)
                return;
            _characterArray![row, x] = text[i];
        }
    }
        
    public int CursorPosition
    {
        get => _cursorPosition;
        set
        {
            _cursorPosition = value;
            Invalidate();
        }
    }

    [DefaultValue(40)]
    [Category("Console Settings")]
    [Description("Number of columns (3 to 200).")]
    public int ColumnCount
    {
        get => _columnCount;
        set
        {
            if (value is < 3 or > 200)
                throw new SystemException("Value out of range.");

            _columnCount = value;
            InitializeConsole();
        }
    }

    [DefaultValue(20)]
    [Category("Console Settings")]
    [Description("Number of rows (3 to 200).")]
    public int RowCount
    {
        get => _rowCount;
        set
        {
            if (value < 3 || value > 200)
                throw new SystemException("Value out of range.");

            _rowCount = value;
            InitializeConsole();
        }
    }

    [DefaultValue(true)]
    [Category("Console Settings")]
    public bool UppercaseInput { get; set; } = true;

    private void InitializeConsole()
    {
        if (DesignMode)
            return;

        _characterArray = new char[RowCount, ColumnCount];
        _needsRecalcSize = true;
        Invalidate();
    }

    private void CalcSize(Graphics g)
    {
        _needsRecalcSize = false;

        DrawEngine.CalculateSizes(
            g,
            ref _font,
            ColumnCount,
            RowCount,
            Width,
            Height
        );
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        CursorBlink = !CursorBlink;
        Invalidate();
    }

    private void ConsoleControl_Resize(object sender, EventArgs e)
    {
        _needsRecalcSize = true;
        Invalidate();
    }

    private void ConsoleControl_Paint(object sender, WinForms.PaintEventArgs e)
    {
        if (DesignMode)
            return;

        if (_needsRecalcSize)
            CalcSize(e.Graphics);
        
        if (State.CurrentForm == null)
        {
            e.Graphics.Clear(BackColor);
            using var b = new SolidBrush(BackColor);
            DrawTextConsole(e.Graphics, b);

            if (State.HasForm)
                throw new SystemException("No form is active.");
        }
        else
        {
            if (State is { HasForm: true, CurrentPrompt: { } })
            {
                State.CurrentForm.Draw(e.Graphics, DrawEngine, true);
                State.DrawPrompt(e.Graphics, DrawEngine, State.CurrentForm.CurrentColorScheme!);
            }
            else
            {
                State.CurrentForm.Draw(e.Graphics, DrawEngine, false);
            }
        }
    }

    private void DrawTextConsole(Graphics g, SolidBrush background)
    {
        g.ScaleTransform(DrawEngine.ScaleX, DrawEngine.ScaleY);

        g.Clear(BackColor);

        if (_font == null)
            return;

        using var foreground = new SolidBrush(ForeColor);

        var cursY = RowCount - 1;

        for (var y = 0; y < RowCount; y++)
        {
            for (var x = 0; x < ColumnCount; x++)
            {
                if (_characterArray![y, x] > 0 && _characterArray[y, x] != ' ')
                    DrawEngine.DrawCharacter(g, _characterArray[y, x], _font, foreground, x, y);
                    
                if (_hasFocus && CursorBlink && x == _cursorPosition && y == cursY)
                {
                    DrawEngine.DrawCursor(g, foreground, x, y);
                    DrawEngine.DrawCharacter(g, _characterArray[y, x], _font, background, x, y);
                }
                else if (!_hasFocus && x == _cursorPosition && y == cursY)
                {
                    using var p = new Pen(ForeColor);
                    DrawEngine.DrawCursor(g, p, x, y);
                }
            }
        }
    }

    private void ConsoleControl_KeyPress(object sender, WinForms.KeyPressEventArgs e)
    {
        if (State.Form != null)
        {
            State.Form.CharacterInput(e.KeyChar);
            return;
        }

        if (e.KeyChar == 13)
        {
            HandleInput();
            return;
        }

        _characterArray![RowCount - 1, CursorPosition] = UppercaseInput ? char.ToUpper(e.KeyChar) : e.KeyChar;
            
        if (CursorPosition < ColumnCount - 1)
            CursorPosition++;
            
        RowChanged = true;
    }

    private void ConsoleControl_Enter(object sender, EventArgs e)
    {
        _hasFocus = true;
        Invalidate();
    }

    private void ConsoleControl_Leave(object sender, EventArgs e)
    {
        _hasFocus = false;
        Invalidate();
    }

    private void BackspaceAt(int col)
    {
        if (col <= 0)
            return;

        for (var c = col - 1; c < ColumnCount; c++)
            _characterArray![RowCount - 1, c] = c < RowCount - 1
                ? _characterArray![RowCount - 1, c + 1]
                : (char)0;
    }

    private void InsertAt(int col)
    {
        if (col == ColumnCount - 1)
            _characterArray![RowCount - 1, ColumnCount - 1] = (char)0;

        for (var i = ColumnCount - 1; i > col; i--)
            _characterArray![RowCount - 1, i] = _characterArray[RowCount - 1, i - 1];
            
        _characterArray![RowCount - 1, col] = (char)0;
    }

    protected override bool IsInputKey(WinForms.Keys keyData) =>
        Keyboard.IsInputKey(keyData) || base.IsInputKey(keyData);

    private int LastCharacterIndex
    {
        get
        {
            for (var c = ColumnCount - 1; c >= 0; c--)
                if (_characterArray![RowCount - 1, c] != (char)0 && _characterArray[RowCount - 1, c] != ' ')
                    return c;

            return -1;
        }
    }

    private void ConsoleControl_KeyDown(object sender, WinForms.KeyEventArgs e)
    {
        if (e.KeyCode == WinForms.Keys.ShiftKey)
        {
            ShiftKey = true;
            return;
        }

        if (State.Form != null && IsControlKey(e.KeyCode))
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            State.Form.KeyPressed(e.KeyCode, ShiftKey);
            return;
        }
            
        string text;
            
        switch (e.KeyCode)
        {
            case WinForms.Keys.Up:
                if (!History.HasData())
                    return;
                text = GetText(RowCount - 1, 0);
                if (RowChanged && !string.IsNullOrWhiteSpace(text))
                    History.RememberTemporary(text.Trim());
                RestoreInput(History.Previous());
                break;
            case WinForms.Keys.Down:
                text = GetText(RowCount - 1, 0);
                if (RowChanged && !string.IsNullOrWhiteSpace(text))
                    History.RememberTemporary(text.Trim());
                RestoreInput(History.Next());
                break;
            case WinForms.Keys.Insert:
            {
                if (State.Form?.CurrentControl is TextBox textBox)
                {
                    textBox.KeyPressed(WinForms.Keys.Insert);
                    return;
                }

                e.SuppressKeyPress = true;
                InsertAt(CursorPosition);
                Invalidate();
                break;
            }
            case WinForms.Keys.Back:
                e.SuppressKeyPress = true;
                BackspaceAt(CursorPosition);

                if (CursorPosition > 0)
                    CursorPosition--;

                Invalidate();
                break;
            case WinForms.Keys.Delete:
            {
                if (State.Form?.CurrentControl is TextBox textBox)
                {
                    textBox.KeyPressed(WinForms.Keys.Delete);
                    return;
                }

                e.SuppressKeyPress = true;
                BackspaceAt(CursorPosition + 1);
                Invalidate();
                break;
            }
            case WinForms.Keys.Right:
                if (CursorPosition < ColumnCount - 1 && CursorPosition < LastCharacterIndex + 1)
                {
                    RestoreBlink();
                    CursorPosition++;
                }
                break;
            case WinForms.Keys.Left:
                if (CursorPosition > 0)
                {
                    RestoreBlink();
                    CursorPosition--;
                }
                break;
            case WinForms.Keys.Home:
                RestoreBlink();
                CursorPosition = 0;
                break;
            case WinForms.Keys.End:
                GoToEnd();
                break;
        }
        Invalidate();
    }

    private bool IsControlKey(WinForms.Keys key) =>
        Keyboard.IsControlKey(key);

    private void GoToEnd()
    {
        RestoreBlink();
        var lastCharacterIndex = LastCharacterIndex;

        CursorPosition = lastCharacterIndex < ColumnCount - 2
            ? lastCharacterIndex + 1
            : ColumnCount - 1;
    }

    public void RestoreBlink()
    {
        timer1.Stop();
        CursorBlink = true;
        timer1.Start();
        Invalidate();
    }

    private void RestoreInput(string text)
    {
        for (var i = 0; i < ColumnCount; i++)
            _characterArray![RowCount - 1, i] = (char)0;

        SetText(RowCount - 1, 0, text);
        GoToEnd();
        RowChanged = false;
    }

    private void HandleInput()
    {
        var text = GetText(RowCount - 1, 0);

        if (!string.IsNullOrWhiteSpace(text))
            History.Remember(text.Trim());
            
        RowChanged = false;
        ScrollUp();
        CursorPosition = 0;
        UserInput?.Invoke(this, new UserInputEventArgs(text));
    }

    private string GetText(int row, int col)
    {
        var s = new StringBuilder();

        for (var c = col; c < ColumnCount; c++)
            s.Append(_characterArray![row, c] == (char)0 ? ' ' : _characterArray[row, c]);
            
        return s.ToString().Trim();
    }

    private void ScrollUp()
    {
        for (var row = 1; row < RowCount; row++)
        for (var col = 0; col < ColumnCount; col++)
            _characterArray![row - 1, col] = _characterArray[row, col];

        for (var col = 0; col < ColumnCount; col++)
            _characterArray![RowCount - 1, col] = (char)0;
    }

    public void WriteText(int msDelay, string text)
    {
        var rows = WordWrapper.WordWrap(ColumnCount, text.Trim()).Split('\n');
        timer1.Enabled = false;
        CursorBlink = false;

        foreach (var row in rows)
        {
            var r = row.Trim();

            if (string.IsNullOrEmpty(r) && row == rows.Last())
                continue;
                
            for (var i = 0; i < r.Length; i++)
            {
                SetText(RowCount - 1, i, r[i].ToString());
                if (msDelay <= 0)
                    continue;
                Refresh();
                WinForms.Application.DoEvents();
                Thread.Sleep(msDelay);
            }

            ScrollUp();
            Invalidate();
        }
        timer1.Enabled = true;

        if (ParentForm != null)
        {
            WindowsInput.Buffer.FlushKeyboardBuffer(ParentForm.Handle);
            WindowsInput.Buffer.FlushMouseBuffer(ParentForm.Handle);
        }
    }

    internal void TriggerEvent(object sender, ConsoleControlEventArgs e) =>
        ControlEvent?.Invoke(sender, e);

    private void ConsoleControl_KeyUp(object sender, WinForms.KeyEventArgs e)
    {
        if (e.KeyCode == WinForms.Keys.ShiftKey)
            ShiftKey = false;
    }

    private void ConsoleControl_MouseClick(object sender, WinForms.MouseEventArgs e)
    {
        if (State.Form == null)
            return;

        var point = _mouse.ToCharacterPosition(DrawEngine);
        var hit = State.Form.GetControlAt(point);

        if (hit == null)
            return;

        State.SetActiveControl(hit);

        if (hit is IMultipleClickZoneControl c)
        {
            c.MouseClick(point);
            return;
        }

        State.Form.KeyPressed(WinForms.Keys.Enter, ShiftKey);
    }

    private void ConsoleControl_MouseDoubleClick(object sender, WinForms.MouseEventArgs e)
    {
        var hit = State.Form?.GetControlAt(_mouse.AsPoint());

        if (hit is IMultipleClickZoneControl)
            State.Form?.KeyPressed(WinForms.Keys.Enter, ShiftKey);
    }

    private void ConsoleControl_MouseMove(object sender, WinForms.MouseEventArgs e)
    {
        _mouse.UpdateMousePosition(e, DrawEngine);
        
        if (_waitMode)
            return;

        SetCursor();
    }

    private void SetCursor()
    {
        if (State.Form == null)
        {
            Cursor = WinForms.Cursors.Arrow;
            return;
        }

        var hit = State.Form.GetControlAt(_mouse.ToCharacterPosition(DrawEngine));

        if (hit == null)
        {
            Cursor = WinForms.Cursors.Arrow;
            return;
        }

        var type = hit.GetType();

        if (type == typeof(Button) || type == typeof(Checkbox) || type == typeof(Radiobutton))
        {
            Cursor = WinForms.Cursors.Hand;
            return;
        }

        Cursor = WinForms.Cursors.Arrow;
    }

    public void BeginWait()
    {
        _waitMode = true;
        Cursor = WinForms.Cursors.WaitCursor;
    }

    public void EndWait()
    {
        if (ParentForm != null)
        {
            WindowsInput.Buffer.FlushKeyboardBuffer(ParentForm.Handle);
            WindowsInput.Buffer.FlushMouseBuffer(ParentForm.Handle);
        }
        _waitMode = false;
        SetCursor();
    }

    public async Task<bool> Ask(string prompt)
    {
        if (State.CurrentForm != null)
            State.Ask(Handle, this, _columnCount, _rowCount, prompt);
        
        await State.WaitForMessageBoxToClose();
        Invalidate();
        return State.PromptResult;
    }

    public void Tell(string prompt)
    {
        if (State.CurrentForm != null)
            State.Tell(Handle, this, _columnCount, _rowCount, prompt);

        Invalidate();
    }
}