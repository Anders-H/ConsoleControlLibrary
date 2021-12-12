using ConsoleControlLibrary.Controls.BaseTypes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConsoleControlLibrary.Controls
{
    public class ClientPicture : ControlBase, IControl, IControlFormOperations
    {
        public delegate void DrawPictureDelegate();

        private readonly DrawPictureDelegate _drawPictureDelegate;

        public ClientPicture(ConsoleForm parentForm, int x, int y, int width, int height) : base(parentForm, x, y, width, height)
        {
            if (_drawPictureDelegate != null)
                _drawPictureDelegate.Invoke();
        }

        public override void CharacterInput(char c)
        {
        }

        public override void Draw(Graphics g, IDrawEngine drawEngine)
        {
            throw new NotImplementedException();
        }

        public override void KeyPressed(Keys key)
        {
        }
    }
}