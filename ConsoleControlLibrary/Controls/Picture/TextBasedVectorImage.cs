using System.Drawing;
using ConsoleControlLibrary.Controls.Picture.TextEngine;

namespace ConsoleControlLibrary.Controls.Picture
{
    public class TextBasedVectorImage : VectorImageBase
    {
        private readonly DrawInstructionList _drawInstructions;

        public TextBasedVectorImage(IDrawEngine drawEngine, ClientPicture clientPicture, int virtualWidth, int virtualHeight, string drawData) : base(drawEngine, clientPicture, virtualWidth, virtualHeight)
        {
            _drawInstructions = new Parser(drawData)
                .Parse();
        }

        public override void DrawPicture(ClientPicture picture, Graphics g)
        {
            foreach (var drawInstruction in _drawInstructions)
            {
                drawInstruction.Draw(picture, g, this);
            }
        }
    }
}