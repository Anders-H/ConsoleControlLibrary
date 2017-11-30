using System.Collections.Generic;
using System.Linq;
using ConsoleControlLibrary.Controls;

namespace ConsoleControlLibrary
{
    public class Form
    {
        protected List<Control> Controls { get; }
        protected Control CurrentControl { get; private set; }
        protected int CurrentControlIndex { get; private set; }
        protected ConsoleControl ParentConsole { get; }
        public Form(ConsoleControl parentConsole)
        {
            ParentConsole = parentConsole;
            Controls = new List<Control>();
        }
        public void AddControl(Control control)
        {
            Controls.Add(control);
        }
        public void Run()
        {
            if (Controls.Count <= 0)
                return;
            CurrentControl = Controls.OrderBy(x => x.TabIndex).First();
            CurrentControlIndex = Controls.IndexOf(CurrentControl);
        }
        internal void Invalidate() => ParentConsole.Invalidate();
    }
}
