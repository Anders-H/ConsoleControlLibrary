using ConsoleControlLibrary.Controls.BaseTypes;
using System.Drawing;
using System.Windows.Forms;

namespace ConsoleControlLibrary.Controls
{
    public class ClientPicture : ControlBase, IControl, IControlFormOperations
    {
        public delegate void DrawPictureDelegate(ClientPicture picture, Graphics g);

        private DrawPictureDelegate _drawPictureDelegate;

        public ClientPicture(ConsoleForm parentForm, int x, int y, int width, int height) : base(parentForm, x, y, width, height)
        {
        }

        public DrawPictureDelegate DrawPicture
        {
            get => _drawPictureDelegate;
            set
            {
                _drawPictureDelegate = value;
                ParentForm.Invalidate();
            }
        }

        public override void CharacterInput(char c)
        {
        }

        public override void Draw(Graphics g, IDrawEngine drawEngine)
        {
            if (_drawPictureDelegate != null)
                _drawPictureDelegate.Invoke(this, g);
        }

        public override void KeyPressed(Keys key)
        {
        }
    }
}