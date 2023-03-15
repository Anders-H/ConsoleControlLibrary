﻿using ConsoleControlLibrary.Controls.BaseTypes;
using ConsoleControlLibrary.Controls.Events;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ConsoleControlLibrary.Controls.ListViewParts;
using ListViewItem = ConsoleControlLibrary.Controls.ListViewParts.ListViewItem;

namespace ConsoleControlLibrary.Controls;

public class ListView : ControlBase, IControl, IControlFormOperations
{
    private int _selectedIndex;
    private int _viewOffset;
    public List<ListViewItem> Items { get; }
    public List<ListViewColumn> Columns { get; }

    public ListView(ConsoleForm parentForm, int x, int y, int width, int height) : base(parentForm, x, y, width, height)
    {
        Items = new List<ListViewItem>();
        Columns = new List<ListViewColumn>();
        SelectedIndex = 0;
        CanGetFocus = true;
        Enabled = true;
        Visible = true;
        _viewOffset = 0;
    }

    public void AddColumn(string title, int width, HorizontalAlign align) =>
        Columns.Add(new ListViewColumn(title, width, align));

    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            _selectedIndex = value;
            EnsureVisible();
            Invalidate();
        }
    }

    public object? SelectedItem
    {
        get
        {
            if (Items.Count <= 0)
                return null;

            if (SelectedIndex >= 0 && SelectedIndex < Items.Count)
                return Items[SelectedIndex];

            return null;
        }
        set
        {
            if (value == null || Items.Count <= 0)
                SelectedIndex = -1;

            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i] != value)
                    continue;

                SelectedIndex = i;
                EnsureVisible();
                return;
            }

            SelectedIndex = -1;
            Invalidate();
        }
    }

    private void EnsureVisible()
    {
        if (Height <= 0 || Items.Count <= 0 || SelectedIndex < 0)
            return;

        if (_viewOffset > SelectedIndex)
            _viewOffset = SelectedIndex;
        else if (SelectedIndex >= _viewOffset + Height)
            _viewOffset = SelectedIndex - Height + 1;

        while (_viewOffset + Height > Items.Count)
        {
            _viewOffset--;
        }
    }

    public void AddItem(ListViewItem item) =>
        Items.Add(item);

    public void AddItem(object value, params string[] subValues)
    {
        var i = new ListViewItem(value);

        foreach (var subValue in subValues)
            i.SubValues.Add(subValue);

        Items.Add(i);
    }

    public override void KeyPressed(Keys key)
    {
        if (Items.Count <= 0)
            return;

        if (key == Keys.Down && SelectedIndex < Items.Count - 1)
            SelectedIndex++;
        else if (key == Keys.Up && SelectedIndex > 0)
            SelectedIndex--;
        else if (key == Keys.PageDown)
        {
            var newIndex = SelectedIndex += (Height - 1);

            if (newIndex >= Items.Count)
                newIndex = Items.Count - 1;

            SelectedIndex = newIndex;
        }
        else if (key == Keys.PageUp)
        {
            var newIndex = SelectedIndex -= (Height - 1);

            if (newIndex < 0)
                newIndex = 0;

            SelectedIndex = newIndex;
        }
        else if (key == Keys.Home)
            SelectedIndex = 0;
        else if (key == Keys.End)
            SelectedIndex = Items.Count - 1;
        else if (key == Keys.Enter && SelectedIndex >= 0)
            ParentForm.TriggerEvent(this, new ConsoleControlEventArgs(ConsoleControlEventType.Click));
    }

    public void MouseClick(Point point)
    {
        var y = point.Y - Y;
        var clickIndex = y - _viewOffset;

        if (clickIndex >= 0 && clickIndex < Items.Count)
            SelectedIndex = clickIndex;
    }

    public override void CharacterInput(char c)
    {
    }

    public override void Draw(Graphics g, IDrawEngine drawEngine)
    {
        if (Width <= 0 || Height <= 0)
            return;

        if (ParentForm.Font == null)
            return;

        var visibleIndex = _viewOffset;
        var y = Y;

        for (var height = 0; height < Height; height++)
        {
            if (Items.Count > visibleIndex)
            {
                var s = Items[visibleIndex].Value.ToString() ?? "";

                if (s.Length > Width)
                    s = s[..Width];

                var x = X;

                if (HasFocus)
                {
                    var brush = visibleIndex == SelectedIndex ? ParentForm.BackColorBrush : ParentForm.ForeColorBrush;

                    if (visibleIndex == SelectedIndex)
                        drawEngine.FillControl(g, ParentForm.ForeColorBrush, new Rectangle(x, y, Width, 1));

                    foreach (var t in s)
                    {
                        drawEngine.DrawCharacter(g, t, ParentForm.Font, brush, x, y);
                        x++;
                    }
                }
                else if (Enabled)
                {
                    var brush = visibleIndex == SelectedIndex ? ParentForm.ForeColorBrush : ParentForm.DisabledForeColorBrush;

                    if (visibleIndex == SelectedIndex)
                        drawEngine.FillControl(g, ParentForm.DisabledForeColorBrush, new Rectangle(x, y, Width, 1));

                    foreach (var t in s)
                    {
                        drawEngine.DrawCharacter(g, t, ParentForm.Font, brush, x, y);
                        x++;
                    }
                }
                else
                {
                    var brush = visibleIndex == SelectedIndex ? ParentForm.BackColorBrush : ParentForm.DisabledForeColorBrush;

                    if (visibleIndex == SelectedIndex)
                        drawEngine.FillControl(g, ParentForm.DisabledForeColorBrush, new Rectangle(x, y, Width, 1));

                    foreach (var t in s)
                    {
                        drawEngine.DrawCharacter(g, t, ParentForm.Font, brush, x, y);
                        x++;
                    }
                }
            }

            y++;
            visibleIndex++;
        }
    }
}