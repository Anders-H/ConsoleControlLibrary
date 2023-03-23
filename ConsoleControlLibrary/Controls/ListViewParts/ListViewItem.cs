using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms.VisualStyles;

namespace ConsoleControlLibrary.Controls.ListViewParts;

public class ListViewItem
{
    private List<string> _alignedValues;
    public object Value { get; set; }
    public List<string> SubValues { get; }

    public ListViewItem(object value)
    {
        _alignedValues = new List<string>();
        Value = value;
        SubValues = new List<string>();
    }

    public override string ToString() =>
        Value.ToString() ?? "";

    internal string GetAlignedValue(int columnIndex, int width, HorizontalAlign align)
    {
        if (_alignedValues.Count > columnIndex)
            return _alignedValues[columnIndex];

        if (_alignedValues.Count < columnIndex)
            throw new SystemException("Align cache unordered use.");

        var value = "";
        if (columnIndex == 0)
            value = Value.ToString() ?? "";
        else if (columnIndex > 0 && SubValues.Count > columnIndex - 1)
            value = SubValues[columnIndex - 1] ?? "";

        if (value.Length > width)
        {
            _alignedValues.Add(value.Substring(0, width));
        }
        else if (value.Length < width)
        {
            switch (align)
            {
                case HorizontalAlign.Left:
                    _alignedValues.Add(value);
                    break;
                case HorizontalAlign.Center:
                    while (value.Length < width)
                        value = $" {value} ";

                    if (value.Length > width)
                        value = value.Substring(0, width);

                    _alignedValues.Add(value);
                    break;
                case HorizontalAlign.Right:
                    while (value.Length < width)
                        value = $" {value}";

                    _alignedValues.Add(value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return _alignedValues.Last();
    }
}