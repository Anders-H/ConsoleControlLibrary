#undef DEBUGRENDER

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ConsoleControlLibrary.Controls.Events;
using ConsoleControlLibrary.Controls.BaseTypes;

namespace ConsoleControlLibrary
{
    public class ConsoleForm : IDisposable
    {
        private delegate void TriggerFormLoadedDelegate();

        private Thread _thread;
        protected List<IControl> Controls { get; }
        protected internal IControl CurrentControl { get; private set; }
        protected internal IControl ActiveControl { get; set; }
        protected int CurrentControlIndex { get; private set; }
        protected ConsoleControl ParentConsole { get; }
        protected internal Brush BackColorBrush { get; }
        protected internal Brush ForeColorBrush { get; }
        protected internal Brush DisabledForeColorBrush { get; }
        protected internal Brush ActiveControlBackColor { get; }
        protected internal Brush ActiveControlForeColor { get; }

        public ConsoleForm(ConsoleControl parentConsole)
        {
            BackColorBrush = new SolidBrush(ControlColorScheme.BackColor);
            ForeColorBrush = new SolidBrush(ControlColorScheme.ForeColor);
            DisabledForeColorBrush = new SolidBrush(ControlColorScheme.DisabledForeColor);
            ActiveControlBackColor = new SolidBrush(ControlColorScheme.ActiveControlBackColor);
            ActiveControlForeColor = new SolidBrush(ControlColorScheme.ActiveControlForeColor);
            ParentConsole = parentConsole;
            Controls = new List<IControl>();
        }

        public void Dispose()
        {
            BackColorBrush.Dispose();
            ForeColorBrush.Dispose();
            DisabledForeColorBrush.Dispose();
            ActiveControlBackColor.Dispose();
            ActiveControlForeColor.Dispose();
        }

        internal void HideCursor() =>
            ParentConsole.HideCursor();

        public void AddControl<T>(T control) where T : IControl, IControlFormOperations =>
            Controls.Add(control);

        public void Run()
        {
            if (Controls.Count <= 0)
                return;

            CurrentControl = Controls.OrderBy(x => x.TabIndex).First();
            CurrentControlIndex = Controls.IndexOf(CurrentControl);
            var c = (IControlFormOperations)CurrentControl;
            c.HasFocus = true;
            
            if (!CurrentControl.CanGetFocus || !CurrentControl.Enabled)
                FocusNextControl();
        }

        internal void Invalidate() =>
            ParentConsole.Invalidate();

        internal void Refresh() =>
            ParentConsole.Refresh();

        protected void TriggerFormLoadedEvent()
        {
            _thread = new Thread(DoTriggerFormLoadEvent);
            _thread.Start();
        }

        private void DoTriggerFormLoadEvent()
        {
            Thread.Sleep(200);
            var executeTriggerLoadEvent = new TriggerFormLoadedDelegate(ExecuteTriggerLoadEvent);
            ParentConsole.Invoke(executeTriggerLoadEvent);
        }

        private void ExecuteTriggerLoadEvent()
        {
            TriggerEvent(this, new ConsoleControlEventArgs(ConsoleControlEventType.FormLoaded));
        }

        internal void TriggerEvent(object sender, ConsoleControlEventArgs e)
        {
            Thread.Sleep(100);
            EventOccurred(sender, e);
            ParentConsole.TriggerEvent(sender, e);
        }

        internal void Draw(Graphics g, IDrawEngine drawEngine)
        {
            g.Clear(ControlColorScheme.BackColor);

            Controls
                .Where(x => x.Visible).Cast<IControlFormOperations>()
                .ToList()
                .ForEach(x => x.Draw(g, drawEngine));

#if DEBUGRENDER
            using (var p = new Pen(Color.FromArgb(40, 40, 40)))
            {
                for (var y = 0; y < drawEngine.RowCount; y++)
                    for (var x = 0; x < drawEngine.ColumnCount; x++)
                        drawEngine.DrawCursor(g, p, x, y);
            }
            using (var p = new Pen(Color.FromArgb(0, 0, 255)))
                foreach (var c in Controls.Where(x => x.Visible && !x.HasFocus))
                    drawEngine.OutlineControl(g, p, c.ControlOutline);
            using (var p = new Pen(Color.FromArgb(0, 255, 255)))
                foreach (var c in Controls.Where(x => x.Visible && x.HasFocus))
                    drawEngine.OutlineControl(g, p, c.ControlOutline);
#endif
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
                ((IControlFormOperations)CurrentControl).KeyPressed(key);
        }

        public void CharacterInput(char c)
        {
            if (CurrentControl == null)
                return;

            if (CurrentControl.Visible && CurrentControl.Enabled)
                ((IControlFormOperations)CurrentControl).CharacterInput(c);
        }

        public void SetFocus(IControl control)
        {
            Controls.Cast<IControlFormOperations>().ToList().ForEach(x => x.HasFocus = false);
            CurrentControl = control;
            ((IControlFormOperations)CurrentControl).HasFocus = true;
            CurrentControlIndex = Controls.IndexOf(CurrentControl);
            ParentConsole.RestoreBlink();
        }

        protected internal void FocusPreviousControl()
        {
            if (Controls.Count <= 0)
                return;

            Controls.Cast<IControlFormOperations>().ToList().ForEach(x => x.HasFocus = false);
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
                ((IControlFormOperations)CurrentControl).HasFocus = true;
                ParentConsole.RestoreBlink();
                break;
            } while (nextindex != startindex);
        }

        protected internal void FocusNextControl()
        {
            if (Controls.Count <= 0)
                return;

            Controls.Cast<IControlFormOperations>().ToList().ForEach(x => x.HasFocus = false);
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
                ((ControlBase)CurrentControl).HasFocus = true;
                ParentConsole.RestoreBlink();
                break;
            } while (nextindex != startindex);
        }
        
        internal Font Font =>
            ParentConsole.GetConsoleFont();

        protected virtual void EventOccurred(object sender, ConsoleControlEventArgs e)
        {
        }
        
        public IControl GetControlAt(int x, int y) =>
            Controls.FirstOrDefault(c =>
                c.HitTest(x, y) && c.Enabled && c.CanGetFocus && c.Visible
            );
        
        public List<IControl> GetControls() =>
            Controls;
    }
}