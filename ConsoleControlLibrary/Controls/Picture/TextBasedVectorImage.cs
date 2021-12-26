using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleControlLibrary.Controls.Picture
{
    public class TextBasedVectorImage : VectorImageBase
    {
        public TextBasedVectorImage(IDrawEngine drawEngine, ClientPicture clientPicture, int virtualWidth, int virtualHeight, string drawData) : base(drawEngine, clientPicture, virtualWidth, virtualHeight)
        {
        }
    }
}