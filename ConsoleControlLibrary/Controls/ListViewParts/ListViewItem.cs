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

    public override string ToString() =>
        Value.ToString() ?? "";

    public IEnumerable<string> Values
    {
        get
        {
            var res = new List<string> { Value.ToString() ?? "" }; // TODO Needs optimization
            res.AddRange(SubValues);
            return res;
        }
    }
}