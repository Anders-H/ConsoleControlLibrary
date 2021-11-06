using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ConsoleControlLibrary.Controls.BaseTypes;

namespace ConsoleControlLibrary.Controls
{
    public class TextBlock : ControlBase, IControl, IControlFormOperations, ITextControl
    {
        private string _text;
        
        private char[,] _characterGrid;
        
        private int _firstFreeRow;
        
        public TextBlock(ConsoleForm parentForm, int x, int y, int width, int height, string text, int characterDelayMs, HorizontalTextAlignment horizontalTextAlignment) : base(parentForm, x, y, width, height)
        {
            HorizontalTextAlignment = horizontalTextAlignment;
            CanGetFocus = false;
            Enabled = true;
            Visible = true;
            Text = text ?? "";
            CharacterDelayMs = characterDelayMs;
        }

        public TextBlock(ConsoleForm parentForm, int x, int y, int width, int height, string text, HorizontalTextAlignment horizontalTextAlignment)
            : this(parentForm, x, y, width, height, text, 0, horizontalTextAlignment)
        {
        }

        public TextBlock(ConsoleForm parentForm, int x, int y, int width, int height, HorizontalTextAlignment horizontalTextAlignment)
            : this(parentForm, x, y, width, height, "", 0, horizontalTextAlignment)
        {
        }
        
        public HorizontalTextAlignment HorizontalTextAlignment { get; }
        
        [DefaultValue(0)]
        public int CharacterDelayMs { get; set; }

        public string Text
        {
            get => _text;
            set
            {
                _text = value ?? "";
                CreateCharacterGrid();
                Invalidate();
            }
        }

        private List<string> PrepareText(string text)
        {
            var rowList = new List<string>();
            var rows = WordWrapper.WordWrap(Width, text.Trim()).Split('\n');
            rowList.AddRange(rows);
            if (rowList.Last().Trim() == "")
                rowList.RemoveAt(rowList.Count - 1);
            return rowList;
        }

        private void CreateCharacterGrid()
        {
            _characterGrid = new char[Width, Height];

            if (string.IsNullOrWhiteSpace(Text))
                return;
            
            var rowList = PrepareText(Text);
            
            switch (HorizontalTextAlignment)
            {
                case HorizontalTextAlignment.Top:
                    var rowIndex = 0;
                    foreach (var row in rowList)
                    {
                        var r = row.Trim();
                        for (var i = 0; i < r.Length; i++)
                            _characterGrid[i, rowIndex] = r[i];
                        if (rowIndex >= Height - 1 && row != rowList.Last())
                            ScrollUp();
                        else
                            rowIndex++;
                    }
                    _firstFreeRow = rowIndex - 1;
                    break;
                case HorizontalTextAlignment.Bottom:
                    _firstFreeRow = Height - 1;
                    DoWrite(rowList, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Write(string text)
        {
            if (_firstFreeRow >= Height - 1 && _characterGrid[0, Height - 1] > (char)0)
                ScrollUp();
            
            var rowList = PrepareText(text);
            DoWrite(rowList, CharacterDelayMs);
        }

        private void DoWrite(List<string> rowList, int delay)
        {
            if (_firstFreeRow < Height - 1)
                for (var y = 0; y < Height; y++)
                {
                    if (_characterGrid[0, y] > 0)
                        continue;
                    _firstFreeRow = y;
                    break;
                }

            ParentForm.HideCursor();
            
            foreach (var row in rowList)
            {
                var r = row.Trim();
                
                for (var i = 0; i < r.Length; i++)
                {
                    _characterGrid[i, _firstFreeRow] = r[i];
                    if (delay <= 0)
                        continue;
                    System.Threading.Thread.Sleep(delay);
                    ParentForm.Refresh();
                }
                
                if (_firstFreeRow < Height - 1)
                    _firstFreeRow++;
                else if (row != rowList.Last())
                    ScrollUp();
            }
        }

        public override void KeyPressed(Keys key)
        {
        }

        public override void CharacterInput(char c)
        {
        }
        
        public override void Draw(Graphics g, IDrawEngine drawEngine)
        {
            for(var y = 0; y < Height; y++)
                for(var x = 0; x < Width; x++)
                    drawEngine.DrawCharacter(g, _characterGrid[x, y], ParentForm.Font, ParentForm.ForeColorBrush, x + X, y + Y);
        }

        private void ScrollUp()
        {
            if (Height < 2)
                return;

            for (var y = 0; y < Height - 1; y++)
                for (var x = 0; x < Width; x++)
                    _characterGrid[x, y] = _characterGrid[x, y + 1];

            for (var x = 0; x < Width; x++)
                _characterGrid[x, Height - 1] = (char)0;
        }
    }
}