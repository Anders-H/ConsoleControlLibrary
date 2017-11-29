using System.Drawing;

namespace ConsoleControlLibrary.Controls
{
    public abstract class Control
    {
        public Form ParentForm { get; }
        public int TabIndex { get; set; }
        public bool CanGetFocus { get; protected set; }
        public bool HasFocus { get; internal set; }
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
        protected Control(Form parentForm, int x, int y, int width, int height)
        {
            ParentForm = parentForm;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
