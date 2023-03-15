using System.Collections.Generic;

namespace ConsoleControlLibrary.Controls.ListViewParts;

public class ListViewItem
{
    public object Value { get; set; }
    public List<string> SubValues { get; }

    public ListViewItem(object value)
    {
        Value = value;
        SubValues = new List<string>();
    }
}