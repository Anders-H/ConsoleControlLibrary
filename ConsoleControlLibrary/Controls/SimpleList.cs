using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ConsoleControlLibrary.Controls.BaseTypes;
using ConsoleControlLibrary.Controls.Events;

namespace ConsoleControlLibrary.Controls;

public class SimpleList : ListBase, IMultipleClickZoneControl
{
    public List<object> Items { get; }

    public SimpleList(ConsoleForm parentForm, int x, int y, int width, int height) : base(parentForm, x, y, width, height)
    {
        Items = new List<object>();
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

    protected override void EnsureVisible()
    {
        if (Height <= 0 || Items.Count <= 0 || SelectedIndex < 0)
            return;

        if (ViewOffset > SelectedIndex)
            ViewOffset = SelectedIndex;
        else if (SelectedIndex >= ViewOffset + Height)
            ViewOffset = SelectedIndex - Height + 1;

        while (ViewOffset + Height > Items.Count)
        {
            ViewOffset--;
        }
    }

    public void AddItem(object item) =>
        Items.Add(item);

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
        var clickIndex = y - ViewOffset;

        if (clickIndex >= 0 && clickIndex < Items.Count)
            SelectedIndex = clickIndex;
    }

    public override void Draw(Graphics g, IDrawEngine drawEngine, bool blockedByModalDialog)
    {
        if (Width <= 0 || Height <= 0)
            return;

        if (ParentForm.Font == null)
            return;

        var visibleIndex = ViewOffset;
        var y = Y;

        using var backColor = new SolidBrush(ParentForm.CurrentColorScheme!.BackColor);

        for (var height = 0; height < Height; height++)
        {
            if (Items.Count > visibleIndex)
            {
                var s = Items[visibleIndex].ToString() ?? "";
                
                if (s.Length > Width)
                    s = s[..Width];

                var x = X;

                if (HasFocus)
                {
                    var brush = visibleIndex == SelectedIndex ? backColor : ParentForm.CurrentColorScheme!.ForeColor;

                    if (visibleIndex == SelectedIndex)
                        drawEngine.FillControl(g, ParentForm.CurrentColorScheme!.ForeColor, new Rectangle(x, y, Width, 1));

                    foreach (var t in s)
                    {
                        drawEngine.DrawCharacter(g, t, ParentForm.Font, brush, x, y);
                        x++;
                    }
                }
                else if (Enabled)
                {
                    var brush = visibleIndex == SelectedIndex ? ParentForm.CurrentColorScheme!.ForeColor : ParentForm.CurrentColorScheme!.DisabledForeColor;

                    if (visibleIndex == SelectedIndex)
                        drawEngine.FillControl(g, ParentForm.CurrentColorScheme!.DisabledForeColor, new Rectangle(x, y, Width, 1));

                    foreach (var t in s)
                    {
                        drawEngine.DrawCharacter(g, t, ParentForm.Font, brush, x, y);
                        x++;
                    }
                }
                else
                {
                    var brush = visibleIndex == SelectedIndex ? backColor : ParentForm.CurrentColorScheme!.DisabledForeColor;

                    if (visibleIndex == SelectedIndex)
                        drawEngine.FillControl(g, ParentForm.CurrentColorScheme!.DisabledForeColor, new Rectangle(x, y, Width, 1));

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