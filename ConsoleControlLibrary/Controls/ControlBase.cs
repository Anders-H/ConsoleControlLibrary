using System.Drawing;
using System.Windows.Forms;

namespace ConsoleControlLibrary.Controls
{
    public abstract class ControlBase
    {
        public Form ParentForm { get; }
        public int TabIndex { get; set; }
        public bool CanGetFocus { get; protected set; }
        public bool HasFocus { get; internal set; }
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
        private bool _enabled;
        protected ControlBase(Form parentForm, int x, int y, int width, int height)
        {
            ParentForm = parentForm;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                Invalidate();
            }
        }
        protected void Invalidate() => ParentForm.Invalidate();
        protected internal abstract void KeyPressInfo(Keys key);
        protected internal abstract void Draw(Graphics g, IDrawEngine drawEngine);
    }
}
