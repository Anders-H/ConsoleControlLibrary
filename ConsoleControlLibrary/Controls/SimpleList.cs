using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ConsoleControlLibrary.Controls.BaseTypes;

namespace ConsoleControlLibrary.Controls;

public class SimpleList : ControlBase, IControl, IControlFormOperations
{
    private int _selectedIndex;
    private int _viewOffset;
    public List<object> Items { get; }

    public SimpleList(ConsoleForm parentForm, int x, int y, int width, int height) : base(parentForm, x, y, width, height)
    {
        Items = new List<object>();
        SelectedIndex = 0;
        CanGetFocus = true;
        Enabled = true;
        Visible = true;
        _viewOffset = 0;
    }

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
        }
    }

    private void EnsureVisible()
    {
        // TODO: Fix offset.
    }

    public void AddItem(object item) =>
        Items.Add(item);

    public override void KeyPressed(Keys key)
    {
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
                var s = Items[visibleIndex]?.ToString() ?? "";
                
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