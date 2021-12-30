#nullable enable
using System.Linq;
using ConsoleControlLibrary.Controls.BaseTypes;

namespace ConsoleControlLibrary.Controls
{
    public class Radiobutton : CheckboxBase, IControl, IControlFormOperations, ITextControl, ICheckControl
    {
        public object Group { get; }
        
        public Radiobutton(ConsoleForm parentForm, bool isChecked, object group, int x, int y, int width, int height, string text) : base(parentForm, isChecked, x, y, width, height, text)
        {
            Group = group;
        }

        public Radiobutton(ConsoleForm parentForm, bool isChecked, object group, int x, int y, int width, string text)
            : this(parentForm, isChecked, group, x, y, width, 1, text)
        {
        }

        public Radiobutton(ConsoleForm parentForm, bool isChecked, object group, int x, int y, string text)
            : this(parentForm, isChecked, group, x, y, text.Length + 3, 1, text)
        {
        }
        
        protected override char LeftBracket =>
            '(';

        protected override char RightBracket =>
            ')';

        protected override void CheckedChanged()
        {
            if (!Checked)
                return;
            
            var radioButtons = ParentForm.GetControls()
                .Where(x => x.GetType() == typeof(Radiobutton));
            
            var group = radioButtons
                .Where(x => ((Radiobutton)x).Group == Group)
                .ToList();
            
            foreach (var g in @group.Where(g => g != this))
                ((Radiobutton)g).Checked = false;
        }
    }
}