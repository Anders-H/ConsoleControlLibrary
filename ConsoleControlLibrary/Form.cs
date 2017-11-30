using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ConsoleControlLibrary.Controls;

namespace ConsoleControlLibrary
{
    public class Form
    {
        protected List<ControlBase> Controls { get; }
        protected internal ControlBase CurrentControl { get; private set; }
        protected int CurrentControlIndex { get; private set; }
        protected ConsoleControl ParentConsole { get; }
        public Form(ConsoleControl parentConsole)
        {
            ParentConsole = parentConsole;
            Controls = new List<ControlBase>();
        }
        public void AddControl(ControlBase control)
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
        internal void TriggerEvent(object sender, ConsoleControlEventArgs e) => ParentConsole.TriggerEvent(sender, e);
        internal void Draw(Graphics g)
        {
            g.Clear(ControlColorScheme.BackgroundColor);
            Controls.ForEach(x => x.Draw(g));
        }
    }
}
