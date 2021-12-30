#nullable enable
using System.Drawing;

namespace ConsoleControlLibrary.Controls.Picture.TextEngine
{
    public abstract class DrawInstruction
    {
        public abstract void Draw(
            ClientPicture picture,
            Graphics g,
            VectorImageBase owner
        );
    }
}