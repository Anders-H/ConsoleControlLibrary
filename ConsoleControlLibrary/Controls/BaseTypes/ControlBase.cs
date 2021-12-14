using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConsoleControlLibrary.Controls.BaseTypes
{
    public abstract class ControlBase
    {
        public ConsoleForm ParentForm { get; }
        public int TabIndex { get; set; }
        public bool CanGetFocus { get; protected set; }
        public bool HasFocus { get; set; }
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
        public DateTime GotActiveAt { get; set; }
        private bool _enabled;
        private bool _visible;

        protected ControlBase(ConsoleForm parentForm, int x, int y, int width, int height)
        {
            ParentForm = parentForm;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle ControlOutline =>
            new Rectangle(X, Y, Width, Height);

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                if (!_enabled && HasFocus)
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
                if (!_visible && HasFocus)
                    ParentForm.FocusNextControl();
                Invalidate();
            }
        }

        protected void Invalidate() =>
            ParentForm.Invalidate();

        public abstract void KeyPressed(Keys key);

        public abstract void CharacterInput(char c);

        public abstract void Draw(Graphics g, IDrawEngine drawEngine);

        public bool HitTest(int x, int y) =>
            x >= X && y >= Y && x < X + Width && y < Y + Height;

        protected bool ConsiderAsActiveNow() =>
            DateTime.Now.Subtract(GotActiveAt).TotalSeconds < 0.1;
    }
}