using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ConsoleControlLibrary.Controls.Events;
using ConsoleControlLibrary.Controls.BaseTypes;

namespace ConsoleControlLibrary
{
    public class ConsoleForm : IDisposable
    {
        protected List<ControlBase> Controls { get; }
        protected internal ControlBase CurrentControl { get; private set; }
        protected int CurrentControlIndex { get; private set; }
        protected ConsoleControl ParentConsole { get; }
        protected internal Brush BackColorBrush { get; }
        protected internal Brush ForeColorBrush { get; }
        protected internal Brush DisabledForeColorBrush { get; }
        public ConsoleForm(ConsoleControl parentConsole)
        {
            BackColorBrush = new SolidBrush(ControlColorScheme.BackColor);
            ForeColorBrush = new SolidBrush(ControlColorScheme.ForeColor);
            DisabledForeColorBrush = new SolidBrush(ControlColorScheme.DisabledForeColor);
            ParentConsole = parentConsole;
            Controls = new List<ControlBase>();
        }
        public void Dispose()
        {
            BackColorBrush.Dispose();
            ForeColorBrush.Dispose();
        }
        internal void HideCursor() => ParentConsole.HideCursor();
        public void AddControl(ControlBase control) => Controls.Add(control);
        public void Run()
        {
            if (Controls.Count <= 0)
                return;
            CurrentControl = Controls.OrderBy(x => x.TabIndex).First();
            CurrentControlIndex = Controls.IndexOf(CurrentControl);
            CurrentControl.HasFocus = true;
            if (!CurrentControl.CanGetFocus || !CurrentControl.Enabled)
                FocusNextControl();
        }
        internal void Invalidate() => ParentConsole.Invalidate();
        internal void Refresh() => ParentConsole.Refresh();
        internal void TriggerEvent(object sender, ConsoleControlEventArgs e)
        {
            EventOccured(sender, e);
            ParentConsole.TriggerEvent(sender, e);
        }
        internal void Draw(Graphics g, IDrawEngine drawEngine)
        {
            g.Clear(ControlColorScheme.BackColor);
            Controls.Where(x => x.Visible).ToList().ForEach(x => x.Draw(g, drawEngine));
        }
        internal void KeyPressed(Keys key, bool shift)
        {
            if (key == Keys.Tab)
            {
                if (shift)
                    FocusPreviousControl();
                else
                    FocusNextControl();
                return;
            }
            if (CurrentControl == null)
                return;
            if (CurrentControl.Visible && CurrentControl.Enabled)
                CurrentControl.KeyPressed(key);
        }
        public void SetFocus(ControlBase control)
        {
            Controls.ForEach(x => x.HasFocus = false);
            CurrentControl = control;
            CurrentControl.HasFocus = true;
            CurrentControlIndex = Controls.IndexOf(CurrentControl);
            ParentConsole.RestoreBlink();
        }
        protected internal void FocusPreviousControl()
        {
            if (Controls.Count <= 0)
                return;
            Controls.ForEach(x => x.HasFocus = false);
            var startindex = CurrentControlIndex;
            var nextindex = CurrentControlIndex;
            do
            {
                nextindex--;
                if (nextindex < 0)
                    nextindex = Controls.Count - 1;
                if (!Controls[nextindex].Enabled || !Controls[nextindex].Visible || !Controls[nextindex].CanGetFocus)
                    continue;
                CurrentControlIndex = nextindex;
                CurrentControl = Controls[nextindex];
                CurrentControl.HasFocus = true;
                ParentConsole.RestoreBlink();
                break;
            } while (nextindex != startindex);
        }
        protected internal void FocusNextControl()
        {
            if (Controls.Count <= 0)
                return;
            Controls.ForEach(x => x.HasFocus = false);
            var startindex = CurrentControlIndex;
            var nextindex = CurrentControlIndex;
            do
            {
                nextindex++;
                if (nextindex >= Controls.Count)
                    nextindex = 0;
                if (!Controls[nextindex].Enabled || !Controls[nextindex].Visible || !Controls[nextindex].CanGetFocus)
                    continue;
                CurrentControlIndex = nextindex;
                CurrentControl = Controls[nextindex];
                CurrentControl.HasFocus = true;
                ParentConsole.RestoreBlink();
                break;
            } while (nextindex != startindex);
        }
        internal Font Font => ParentConsole.GetConsoleFont();
        protected virtual void EventOccured(object sender, ConsoleControlEventArgs e) { }
        public ControlBase GetControlAt(int x, int y) => Controls.FirstOrDefault(c => c.HitTest(x, y) && c.Enabled && c.CanGetFocus && c.Visible);
    }
}
