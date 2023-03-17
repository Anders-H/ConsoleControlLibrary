using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms.VisualStyles;

namespace ConsoleControlLibrary.Controls.ListViewParts;

public class ListViewColumn
{
    private string? _alignedTitle;
    public string Title { get; }
    public int Width { get; set; }
    public HorizontalAlign Align { get;}

    public ListViewColumn(string title, int width, HorizontalAlign align)
    {
        _alignedTitle = null;
        Title = title;
        Width = width;
        Align = align;
    }

    public string GetAlignedTitle()
    {
        if (_alignedTitle == null)
        {
            if (Title.Length > Width)
            {
                _alignedTitle = Title.Substring(0, Width);
            }
            else if (Title.Length < Width)
            {
                _alignedTitle = Title;

                switch (Align)
                {
                    case HorizontalAlign.Left:
                        while (_alignedTitle.Length < Width)
                            _alignedTitle = $"{_alignedTitle} ";
                        break;
                    case HorizontalAlign.Center:
                        while (_alignedTitle.Length < Width)
                            _alignedTitle = $" {_alignedTitle} ";
                        if (_alignedTitle.Length > Width)
                            _alignedTitle = _alignedTitle.Substring(0, Width);
                        break;
                    case HorizontalAlign.Right:
                        while (_alignedTitle.Length < Width)
                            _alignedTitle = $" {_alignedTitle}";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                _alignedTitle = Title;
            }
        }


        return _alignedTitle;
    }
}