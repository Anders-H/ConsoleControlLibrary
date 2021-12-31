#nullable enable
using System;
using System.Drawing;

namespace ConsoleControlLibrary.Controls.Picture
{
    public class VectorImageBase
    {
        protected IDrawEngine DrawEngine { get; }
        protected ClientPicture ClientPicture { get; }
        protected internal int VirtualWidth { get; }
        protected internal int VirtualHeight { get; }

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

        public int VirtualToPhysical(bool width, int v) =>
            width
                ? (int)Math.Round(
                    LinearInterpolation(
                        v,
                        0,
                        VirtualWidth,
                        PhysicalX,
                        PhysicalX + PhysicalWidth
                    )
                )
                : (int)Math.Round(
                    LinearInterpolation(
                        v,
                        0,
                        VirtualHeight,
                        PhysicalY,
                        PhysicalY + PhysicalHeight
                    )
                );

        public Point VirtualToPhysical(int x, int y)
        {
            var adjustedX = VirtualToPhysical(true, x);
            var adjustedY = VirtualToPhysical(false, y);

            return new Point(adjustedX, adjustedY);
        }

        public Rectangle VirtualToPhysical(int x, int y, int width, int height)
        {
            var adjustedX = VirtualToPhysical(true, x);
            var adjustedY = VirtualToPhysical(false, y);
            var adjustedWidth = VirtualToPhysical(true, width);
            var adjustedHeight = VirtualToPhysical(false, height);

            return new Rectangle(adjustedX, adjustedY, adjustedWidth, adjustedHeight);
        }

        private static double LinearInterpolation(double value, double inputMin, double inputMax, double outputMin, double outputMax)
        {
            if ((inputMax - inputMin) == 0)
                return (outputMin + outputMax) / 2;

            return outputMin + (value - inputMin) * (outputMax - outputMin) / (inputMax - inputMin);
        }
    }
}