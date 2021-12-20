using System;
using System.Drawing;

namespace ConsoleControlLibrary.Controls.Picture
{
    public class VectorImageBase
    {
        protected IDrawEngine DrawEngine { get; }
        protected ClientPicture ClientPicture { get; }
        protected int VirtualWidth { get; }
        protected int VirtualHeight { get; }

        protected int PhysicalX =>
            (int)Math.Round(ClientPicture.X * DrawEngine.CharacterWidth);

        protected int PhysicalY =>
            (int)Math.Round(ClientPicture.Y * DrawEngine.CharacterHeight);

        protected int PhysicalWidth =>
            (int)Math.Round(ClientPicture.Width * DrawEngine.CharacterWidth);

        protected int PhysicalHeight =>
            (int)Math.Round(ClientPicture.Height * DrawEngine.CharacterHeight);

        public VectorImageBase(IDrawEngine drawEngine, ClientPicture clientPicture, int virtualWidth, int virtualHeight)
        {
            DrawEngine = drawEngine;
            ClientPicture = clientPicture;
            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;
            clientPicture.DrawPicture = DrawPicture;
        }

        public virtual void DrawPicture(ClientPicture picture, Graphics g)
        {
        }

        public Point VirtualToPhysical(int x, int y)
        {
            var adjustedX = (int)Math.Round(LinearInterpolation(x, 0, VirtualWidth, PhysicalX, PhysicalX + PhysicalWidth));
            var adjustedY = (int)Math.Round(LinearInterpolation(y, 0, VirtualHeight, PhysicalY, PhysicalY + PhysicalHeight));

            return new Point(adjustedX, adjustedY);
        }

        private static double LinearInterpolation(double value, double inputMin, double inputMax, double outputMin, double outputMax)
        {
            if ((inputMax - inputMin) == 0)
                return (outputMin + outputMax) / 2;

            return outputMin + (value - inputMin) * (outputMax - outputMin) / (inputMax - inputMin);
        }
    }
}
