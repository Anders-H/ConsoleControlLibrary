using System.Drawing;
using System.Windows.Forms;

namespace ConsoleControlLibrary.Controls.BaseTypes;

public interface IControlFormOperations
{
    bool HasFocus { get; set; }
    void KeyPressed(Keys key);
    void CharacterInput(char c);
    void Draw(Graphics g, IDrawEngine drawEngine);
}