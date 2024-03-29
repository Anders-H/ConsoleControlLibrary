﻿using System;
using System.Drawing;
using System.Windows.Forms;
using ConsoleControlLibrary.Controls.Events;

namespace ConsoleControlLibrary.Controls.BaseTypes;

public abstract class CheckboxBase : ControlBase
{
    private string _text;
    private string? _visibleText;
    private bool _checked;
        
    protected CheckboxBase(ConsoleForm parentForm, bool isChecked, int x, int y, int width, int height, string text) : base(parentForm, x, y, width, height)
    {
        if (width < 3)
            throw new ArgumentOutOfRangeException(nameof(width));

        Checked = isChecked;
        _text = "";
        Text = text;
        CanGetFocus = true;
        Enabled = true;
        Visible = true;
    }

    protected CheckboxBase(ConsoleForm parentForm, bool isChecked, int x, int y, int width, string text) : this(parentForm, isChecked, x, y, width, 1, text)
    {
    }

    protected CheckboxBase(ConsoleForm parentForm, bool isChecked, int x, int y, string text) : this(parentForm, isChecked, x, y, text.Length + 3, 1, text)
    {
    }
        
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

    protected virtual void CheckedChanged()
    {
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

    public override void KeyPressed(Keys key)
    {
        if (key != Keys.Enter)
            return;

        Checked = !Checked;
        ParentForm.TriggerEvent(this, new ConsoleControlEventArgs(ConsoleControlEventType.CheckChange));
    }

    public override void CharacterInput(char c)
    {
    }
        
    protected abstract char LeftBracket { get; }
        
    protected abstract char RightBracket { get; }

    private string VisibleText =>
        _visibleText ??= $"{LeftBracket}{(Checked ? "X" : " ")}{RightBracket}" + (_text.Length <= Width - 3 ? _text : _text.Substring(0, Width - 3));

    public override void Draw(Graphics g, IDrawEngine drawEngine, bool blockedByModalDialog)
    {
        if (Width <= 0)
            return;

        if (ParentForm.Font == null)
            return;

        if (Enabled)
        {
            var activeNow = ConsiderAsActiveNow();

            var foreColor = activeNow
                ? ParentForm.CurrentColorScheme!.ActiveControlForeColor
                : ParentForm.CurrentColorScheme!.ForeColor;

            using var backColor = new SolidBrush(ParentForm.CurrentColorScheme!.BackColor);

            drawEngine.FillControl(g, backColor, new Rectangle(X, Y, Width, Height));

            for (var i = 0; i < VisibleText.Length; i++)
            {
                if (i == 1 && HasFocus && ConsoleControl.CursorBlink && !blockedByModalDialog)
                {
                    drawEngine.DrawCursor(g, foreColor, X + 1, Y);
                    drawEngine.DrawCharacter(g, VisibleText[i], ParentForm.Font, backColor, X + i, Y);
                    continue;
                }

                drawEngine.DrawCharacter(g, VisibleText[i], ParentForm.Font, foreColor, X + i, Y);
            }

            return;
        }

        for (var i = 0; i < VisibleText.Length; i++)
        {
            drawEngine.DrawCharacter(g, VisibleText[i], ParentForm.Font, ParentForm.CurrentColorScheme!.DisabledForeColor, X + i, Y);
        }
    }
}