using System.Drawing;

namespace ConsoleControlLibrary.Controls.Picture
{
    public class VectorImageBase
    {
        protected IDrawEngine DrawEngine { get; }
        protected ClientPicture ClientPicture { get; }
        protected int PhysicalX { get; }
        protected int PhysicalY { get; }
        protected int PhysicalWidth { get; }
        protected int PhysicalHeight { get; }
        protected int VirtualWidth { get; }
        protected int VirtualHeight { get; }

        public VectorImageBase(IDrawEngine drawEngine, ClientPicture clientPicture, int virtualWidth, int virtualHeight)
        {
            DrawEngine = drawEngine;
            ClientPicture = clientPicture;
            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;

            PhysicalX = (int)(clientPicture.X * drawEngine.CharacterWidth);
            PhysicalY = (int)(clientPicture.Y * drawEngine.CharacterHeight);
            PhysicalWidth = (int)(clientPicture.Width * drawEngine.CharacterWidth);
            PhysicalHeight = (int)(clientPicture.Height * drawEngine.CharacterHeight);
        }

        public virtual void DrawPicture(ClientPicture picture, Graphics g)
        {
        }

        public Point VirtualToPhysical(int x, int y)
        {

        }
    }
}