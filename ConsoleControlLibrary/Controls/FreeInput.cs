using System.Drawing;
using System.Windows.Forms;
using ConsoleControlLibrary.Controls.BaseTypes;

namespace ConsoleControlLibrary.Controls;

public class FreeInput : ControlBase, IControl, IControlFormOperations
{
    private readonly char[,] _matrix;
    private int _cursorX;
    private int _cursorY;

    public FreeInput(ConsoleForm parentForm, int x, int y, int width, int height) : base(parentForm, x, y, width, height)
    {
        _matrix = new char[width, height];
        _cursorX = 0;
        _cursorY = 0;
        CanGetFocus = true;
        Enabled = true;
        Visible = true;
    }

    public override void KeyPressed(Keys key)
    {
        
    }

    public override void CharacterInput(char c)
    {
        
    }

    public override void Draw(Graphics g, IDrawEngine drawEngine, bool blockedByModalDialog)
    {
        if (ParentForm.Font == null)
            return;

#if DEBUG
        drawEngine.OutlineControl(g, Pens.White, new Rectangle(X, Y, Width, Height));
#endif

        using var b = new SolidBrush(ParentForm.CurrentColorScheme!.BackColor);

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (HasFocus && x == _cursorX && y == _cursorY && ConsoleControl.CursorBlink && !blockedByModalDialog)
                {
                    drawEngine.DrawCursor(g, ParentForm.CurrentColorScheme!.ForeColor, x + X, y + Y);
                    drawEngine.DrawCharacter(g, _matrix[x, y], ParentForm.Font, b, x + X, y + Y);
                }
                else
                {
                    drawEngine.DrawCharacter(g, _matrix[x, y], ParentForm.Font, ParentForm.CurrentColorScheme!.ForeColor, x + X, y + Y);
                }
            }
        }
    }
}