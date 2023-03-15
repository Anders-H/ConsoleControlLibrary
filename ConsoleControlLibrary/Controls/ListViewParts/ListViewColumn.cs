using System.Windows.Forms.VisualStyles;

namespace ConsoleControlLibrary.Controls.ListViewParts;

public class ListViewColumn
{
    public string Title { get; }
    public int Width { get; set; }
    public HorizontalAlign Align { get;}

    public ListViewColumn(string title, int width, HorizontalAlign align)
    {
        Title = title;
        Width = width;
        Align = align;
    }
}