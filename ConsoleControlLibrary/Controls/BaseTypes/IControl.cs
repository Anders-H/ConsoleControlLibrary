using System;
using System.Drawing;

namespace ConsoleControlLibrary.Controls.BaseTypes
{
    public interface IControl
    {
        ConsoleForm ParentForm { get; }
        int TabIndex { get; set; }
        bool CanGetFocus { get; }
        bool HasFocus { get; }
        int X { get; }
        int Y { get; }
        int Width { get; }
        int Height { get; }
        bool Enabled { get; set; }
        bool Visible { get; set; }
        bool HitTest(int x, int y);
        Rectangle ControlOutline { get; }
        DateTime GotActiveAt { get; set; }
    }
}