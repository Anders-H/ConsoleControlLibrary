using ConsoleControlLibrary.Controls.BaseTypes;
using ConsoleControlLibrary.Controls.Events;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ConsoleControlLibrary.Controls.ListViewParts;
using ListViewItem = ConsoleControlLibrary.Controls.ListViewParts.ListViewItem;

namespace ConsoleControlLibrary.Controls;

public class ListView : ListBase, IMultipleClickZoneControl
{
    public List<ListViewItem> Items { get; }
    public List<ListViewColumn> Columns { get; }

    public ListView(ConsoleForm parentForm, int x, int y, int width, int height) : base(parentForm, x, y, width, height)
    {
        Items = new List<ListViewItem>();
        Columns = new List<ListViewColumn>();
    }

    public void AddColumn(string title, int width, HorizontalAlign align) =>
        Columns.Add(new ListViewColumn(title, width, align));

    public ListViewItem? SelectedItem
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
        else if (SelectedIndex >= ViewOffset + Height - 1)
            ViewOffset = SelectedIndex - Height + 2;

        while (ViewOffset + Height > Items.Count + 1)
        {
            ViewOffset--;
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
        var clickIndex = y + ViewOffset - 1;

        if (clickIndex >= 0 && clickIndex < Items.Count)
            SelectedIndex = clickIndex;
    }

    public override void Draw(Graphics g, IDrawEngine drawEngine, bool blockedByModalDialog)
    {
        if (Width <= 0 || Height <= 0)
            return;

        if (ParentForm.Font == null)
            return;

        if (Columns.Count <= 0)
            return;

        if (HasFocus)
            drawEngine.DrawUnderline(g, ParentForm.CurrentColorScheme!.DisabledForeColor, X, Y, Width);

        var visibleIndex = ViewOffset;
        var y = Y;
        var x = X;
        foreach (var c in Columns)
        {
            var title = c.GetAlignedTitle();

            for (var i = 0; i < c.Width; i++)
            {
                if (title.Length > i)
                    drawEngine.DrawCharacter(g, title[i], ParentForm.Font, ParentForm.CurrentColorScheme!.DisabledForeColor, x, y);

                x++;
            }

            if (x >= Width)
                break;
        }

        if (Height <= 1)
            return;

        using var backColor = new SolidBrush(ParentForm.CurrentColorScheme!.BackColor);

        y++;

        for (var height = 1; height < Height; height++)
        {
            if (Items.Count > visibleIndex)
            {
                var itm = Items[visibleIndex];

                if (visibleIndex == SelectedIndex)
                    drawEngine.FillControl(g, HasFocus ? ParentForm.CurrentColorScheme!.ForeColor : ParentForm.CurrentColorScheme.DisabledForeColor, new Rectangle(X, y, Width, 1));

                var columnIndex = -1;
                
                foreach (var col in Columns)
                {
                    columnIndex++;
                    x = columnIndex == 0 ? X : X + GetOffset(Columns, col);
                    var s = itm.GetAlignedValue(columnIndex, col.Width, col.Align);

                    if (HasFocus)
                    {
                        var brush = visibleIndex == SelectedIndex ? backColor : ParentForm.CurrentColorScheme!.ForeColor;

                        foreach (var t in s)
                        {
                            drawEngine.DrawCharacter(g, t, ParentForm.Font, brush, x, y);
                            x++;
                        }
                    }
                    else if (Enabled)
                    {
                        var brush = visibleIndex == SelectedIndex ? ParentForm.CurrentColorScheme!.ForeColor : ParentForm.CurrentColorScheme!.DisabledForeColor;

                        foreach (var t in s)
                        {
                            drawEngine.DrawCharacter(g, t, ParentForm.Font, brush, x, y);
                            x++;
                        }
                    }
                    else
                    {
                        var brush = visibleIndex == SelectedIndex ? backColor : ParentForm.CurrentColorScheme!.DisabledForeColor;

                        foreach (var t in s)
                        {
                            drawEngine.DrawCharacter(g, t, ParentForm.Font, brush, x, y);
                            x++;
                        }
                    }
                }
            }

            y++;
            visibleIndex++;
        }
    }

    private int GetOffset(IEnumerable<ListViewColumn> columns, ListViewColumn column)
    {
        if (column.Offset != null)
            return column.Offset.Value;

        var result = 0;

        foreach (var listViewColumn in columns)
        {
            if (listViewColumn == column)
            {
                column.Offset = result;
                return result;
            }

            result += listViewColumn.Width;
        }

        return 0;
    }
}