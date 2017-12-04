using System.Drawing;
using System.Windows.Forms;

namespace ConsoleControlLibrary.Controls
{
    public abstract class ControlBase
    {
        public ConsoleForm ParentForm { get; }
        public int TabIndex { get; set; }
        public bool CanGetFocus { get; protected set; }
        public bool HasFocus { get; internal set; }
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
        private bool _enabled;
        private bool _visible;
        private IDrawEngine DrawEngine { get; }
        
        protected ControlBase(ConsoleForm parentForm, IDrawEngine drawEngine, int x, int y, int width, int height)
        {
            ParentForm = parentForm;
            DrawEngine = drawEngine;
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
                if (!_enabled)
                    ParentForm.FocusNextControl();
                Invalidate();
            }
        }
        public bool Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                if (!_visible)
                    ParentForm.FocusNextControl();
                Invalidate();
            }
        }
        protected void Invalidate() => ParentForm.Invalidate();
        protected internal abstract void KeyPressed(Keys key);
        protected internal abstract void Draw(Graphics g, IDrawEngine drawEngine);
    }
}
