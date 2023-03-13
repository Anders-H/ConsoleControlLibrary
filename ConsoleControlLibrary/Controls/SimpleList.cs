using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ConsoleControlLibrary.Controls.BaseTypes;

namespace ConsoleControlLibrary.Controls;

public class SimpleList : ControlBase, IControl, IControlFormOperations
{
    private int _selectedIndex;
    public List<object> Items { get; }
    
    public SimpleList(ConsoleForm parentForm, int x, int y, int width, int height) : base(parentForm, x, y, width, height)
    {
        Items = new List<object>();
        SelectedIndex = 0;
    }

    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            _selectedIndex = value;
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
                return;
            }

            SelectedIndex = -1;
        }
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
    }
}