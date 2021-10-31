using ConsoleControlLibrary.Controls.BaseTypes;

namespace ConsoleControlLibrary.Controls
{
    public class Checkbox : CheckboxBase, IControl, IControlFormOperations, ITextControl, ICheckControl
    {
        public Checkbox(ConsoleForm parentForm, bool isChecked, int x, int y, int width, int height, string text)
            : base(parentForm, isChecked, x, y, width, height, text)
        {
        }

        public Checkbox(ConsoleForm parentForm, bool isChecked, int x, int y, int width, string text)
            : this(parentForm, isChecked, x, y, width, 1, text)
        {
        }

        public Checkbox(ConsoleForm parentForm, bool isChecked, int x, int y, string text)
            : this(parentForm, isChecked, x, y, text.Length + 3, 1, text)
        {
        }
        
        protected override char LeftBracket =>
            '[';
        
        protected override char RightBracket =>
            ']';
    }
}