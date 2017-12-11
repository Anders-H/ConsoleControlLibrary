using System.Drawing;
using System.Windows.Forms;
using ConsoleControlLibrary.Controls.BaseTypes;

namespace ConsoleControlLibrary.Controls
{
    public class TextBlock : ControlBase
    {
        private string _text;
        public TextBlock(ConsoleForm parentForm, IDrawEngine drawEngine, int x, int y, int width, int height, string text) : base(parentForm, drawEngine, x, y, width, height)
        {
            Text = text ?? "";
            CanGetFocus = false;
            Enabled = true;
            Visible = true;
        }
        public string Text
        {
            get => _text;
            set
            {
                _text = value ?? "";
                CreateCharacterGrid();
                Invalidate();
            }
        }
        private void CreateCharacterGrid()
        {
            var rows = WordWrapper.WordWrap(Width, Text.Trim()).Split('\n');

        }
        protected internal override void KeyPressed(Keys key) { }
        protected internal override void Draw(Graphics g, IDrawEngine drawEngine)
        {
        }
    }
}
