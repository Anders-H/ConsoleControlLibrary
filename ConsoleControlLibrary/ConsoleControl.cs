using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConsoleControlLibrary.Controls.Events;

namespace ConsoleControlLibrary
{
    public partial class ConsoleControl : UserControl
    {
        private char[,] _characterArray;
        private int _columnCount = 40;
        private int _rowCount = 20;
        private int _cursorPosition;
        internal static bool CursorBlink { get; set; }
        private Font _font;
        private bool _needsRecalcSize;
        private bool _hasFocus;
        private History History { get; } = new History();
        private bool RowChanged { get; set; }
        private ConsoleForm _currentForm;
        public event EventHandler CurrentFormChanged;
        public event UserInputHandler UserInput;
        public event ConsoleControlEventHandler ControlEvent;
        public IDrawEngine DrawEngine { get; set; } = new DrawEngine();
        private bool ShiftKey { get; set; }
        public ConsoleControl()
        {
            InitializeComponent();
            InitializeConsole();
        }
        public Font GetConsoleFont() => _font;
        public void SetText(int row, int col, string text)
        {
            if (col < 0 || col >= ColumnCount || row < 0 || row >= RowCount || string.IsNullOrEmpty(text) || text.Length <= 0)
                return;
            for (var i = 0; i < text.Length; i++)
            {
                var x = i + col;
                if (x >= ColumnCount)
                    return;
                _characterArray[row, x] = text[i];
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
                if (value < 3 || value > 200)
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
            DrawEngine.CalculateSizes(g, ref _font, ColumnCount, RowCount, Width, Height);
            System.Diagnostics.Debug.WriteLine(_font.Name);
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
        private void ConsoleControl_Paint(object sender, PaintEventArgs e)
        {
            if (DesignMode)
                return;
            if (_needsRecalcSize)
                CalcSize(e.Graphics);
            if (CurrentForm == null)
            {
                e.Graphics.Clear(BackColor);
                DrawTextConsole(e.Graphics);
            }
            else
            {
                CurrentForm.Draw(e.Graphics, DrawEngine);
            }
        }
        private void DrawTextConsole(Graphics g)
        {
            g.Clear(BackColor);
            var cursy = RowCount - 1;
            using (var b = new SolidBrush(ForeColor))
            {
                for (var y = 0; y < RowCount; y++)
                {
                    for (var x = 0; x < ColumnCount; x++)
                    {
                        if (_characterArray[y, x] > 0 && _characterArray[y, x] != ' ')
                            DrawEngine.DrawCharacter(g, _characterArray[y, x], _font, b, x, y);
                        if (_hasFocus && CursorBlink && x == _cursorPosition && y == cursy)
                        {
                            DrawEngine.DrawCursor(g, b, x, y);
                            using (var bb = new SolidBrush(BackColor))
                                DrawEngine.DrawCharacter(g, _characterArray[y, x], _font, bb, x, y);
                        }
                        else if (!_hasFocus && x == _cursorPosition && y == cursy)
                            using (var p = new Pen(ForeColor))
                                DrawEngine.DrawCursor(g, p, x, y);
                    }
                }
            }
        }
        private void ConsoleControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CurrentForm?.CurrentControl != null)
                return;
            if (e.KeyChar == 13)
            {
                HandleInput();
                return;
            }
            _characterArray[RowCount - 1, CursorPosition] = UppercaseInput ? char.ToUpper(e.KeyChar) : e.KeyChar;
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
            for (var c = col - 1; c < RowCount; c++)
                _characterArray[RowCount - 1, c] = c < RowCount - 1 ? _characterArray[RowCount - 1, c + 1] : (char)0;
        }

        private void InsertAt(int col)
        {
            if (col == ColumnCount - 1)
                _characterArray[RowCount - 1, ColumnCount - 1] = (char)0;
            for (var i = ColumnCount - 2; i > col; i--)
                _characterArray[RowCount - 1, i] = _characterArray[RowCount - 1, i - 1];
            _characterArray[RowCount - 1, col] = (char)0;
        }
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Shift:
                    return true;
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;
                case Keys.Tab:
                case Keys.Shift | Keys.Tab:
                    return true;
            }
            return base.IsInputKey(keyData);
        }
        private int LastCharacterIndex
        {
            get
            {
                for (var c = ColumnCount - 1; c >= 0; c--)
                    if (_characterArray[RowCount - 1, c] != (char)0 && _characterArray[RowCount - 1, c] != ' ')
                        return c;
                return -1;
            }
        }
        private void ConsoleControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                ShiftKey = true;
                return;
            }
            if (CurrentForm != null)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                CurrentForm.KeyPressed(e.KeyCode, ShiftKey);
                return;
            }
            string text;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (!History.HasData())
                        return;
                    text = GetText(RowCount - 1, 0);
                    if (RowChanged && !string.IsNullOrWhiteSpace(text))
                        History.RememberTemporary(text.Trim());
                    RestoreInput(History.Previous());
                    break;
                case Keys.Down:
                    text = GetText(RowCount - 1, 0);
                    if (RowChanged && !string.IsNullOrWhiteSpace(text))
                        History.RememberTemporary(text.Trim());
                    RestoreInput(History.Next());
                    break;
                case Keys.Insert:
                    e.SuppressKeyPress = true;
                    InsertAt(CursorPosition);
                    Invalidate();
                    break;
                case Keys.Back:
                    e.SuppressKeyPress = true;
                    BackspaceAt(CursorPosition);
                    if (CursorPosition > 0)
                        CursorPosition--;
                    Invalidate();
                    break;
                case Keys.Delete:
                    e.SuppressKeyPress = true;
                    BackspaceAt(CursorPosition + 1);
                    Invalidate();
                    break;
                case Keys.Right:
                    if (CursorPosition < ColumnCount - 1 && CursorPosition < LastCharacterIndex + 1)
                    {
                        RestoreBlink();
                        CursorPosition++;
                    }
                    break;
                case Keys.Left:
                    if (CursorPosition > 0)
                    {
                        RestoreBlink();
                        CursorPosition--;
                    }
                    break;
                case Keys.Home:
                    RestoreBlink();
                    CursorPosition = 0;
                    break;
                case Keys.End:
                    GoToEnd();
                    break;
            }
            Invalidate();
        }
        private void GoToEnd()
        {
            RestoreBlink();
            var lastCharacterIndex = LastCharacterIndex;
            CursorPosition = lastCharacterIndex < ColumnCount - 2 ? lastCharacterIndex + 1 : ColumnCount - 1;
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
                _characterArray[RowCount - 1, i] = (char)0;
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
                s.Append(_characterArray[row, c] == (char)0 ? ' ' : _characterArray[row, c]);
            return s.ToString().Trim();
        }
        private void ScrollUp()
        {
            for (var row = 1; row < RowCount; row++)
                for (var col = 0; col < ColumnCount; col++)
                    _characterArray[row - 1, col] = _characterArray[row, col];
            for (var col = 0; col < ColumnCount; col++)
                _characterArray[RowCount - 1, col] = (char)0;
        }
        public void WriteText(int msDelay, string text)
        {
            var rows = WordWrap((text ?? "").Trim()).Split('\n');
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
                    System.Threading.Thread.Sleep(msDelay);
                }
                ScrollUp();
                Invalidate();
            }
            timer1.Enabled = true;
        }
        private string WordWrap(string text)
        {
            int Break(string breakText, int breakPos, int max)
            {
                var position = max;
                while (position >= 0 && !char.IsWhiteSpace(breakText[breakPos + position]))
                    position--;
                if (position < 0)
                    return max;
                while (position >= 0 && char.IsWhiteSpace(breakText[breakPos + position]))
                    position--;
                return position + 1;
            }
            int wordBreak;
            var s = new StringBuilder();
            for (var charPointer = 0; charPointer < text.Length; charPointer = wordBreak)
            {
                var endOfLine = text.IndexOf("\r\n", charPointer, StringComparison.Ordinal);
                if (endOfLine < 0)
                    endOfLine = text.Length;
                wordBreak = endOfLine < 0 ? text.Length : endOfLine + 2;
                if (endOfLine > charPointer)
                {
                    do
                    {
                        var length = endOfLine - charPointer;
                        if (length > ColumnCount)
                            length = Break(text, charPointer, ColumnCount);
                        s.Append(text, charPointer, length);
                        s.AppendLine();
                        charPointer += length;
                        while (charPointer < endOfLine && char.IsWhiteSpace(text[charPointer]))
                            charPointer++;
                    } while (endOfLine > charPointer);
                    continue;
                }
                s.AppendLine();
            }
            return s.ToString();
        }
        public ConsoleForm CurrentForm
        {
            get => _currentForm;
            set
            {
                if (_currentForm == value)
                    return;
                _currentForm = value;
                _currentForm?.Run();
                Invalidate();
                CurrentFormChanged?.Invoke(this, new EventArgs());
            }
        }
        internal void TriggerEvent(object sender, ConsoleControlEventArgs e) => ControlEvent?.Invoke(sender, e);
        private void ConsoleControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
                ShiftKey = false;
        }
        private void ConsoleControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (CurrentForm == null)
                return;
            var point = DrawEngine.PhysicalCoordinateToFormCoordinate(e.X, e.Y);
            if (point == null)
                return;
            var hit = CurrentForm.GetControlAt(point.Item1, point.Item2);
            if (hit == null)
                return;
            CurrentForm.SetFocus(hit);
            CurrentForm.KeyPressed(Keys.Enter, ShiftKey);
        }
    }
}
