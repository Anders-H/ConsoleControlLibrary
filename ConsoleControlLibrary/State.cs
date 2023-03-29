using System;
using System.Drawing;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using ConsoleControlLibrary.Controls.BaseTypes;

namespace ConsoleControlLibrary;

public class State
{
    internal PromptForm? CurrentPrompt { get; set; }
    internal bool PromptResult { get; set; }
    internal ConsoleState ConsoleState { get; set; }
    public ConsoleForm? CurrentForm { get; set; }

    internal State()
    {
        CurrentForm = null;
        CurrentPrompt = null;
        PromptResult = false;
        ConsoleState = ConsoleState.RunningWithoutForm;
    }

    internal bool HasForm =>
        ConsoleState == ConsoleState.RunningWithoutForm;

    internal async Task WaitForMessageBoxToClose()
    {
        do
        {
            Thread.Yield();
            await Task.Delay(50);
        } while (ConsoleState == ConsoleState.MessageBox);
    }

    internal void DrawPrompt(Graphics g, IDrawEngine drawEngine, CurrentColorScheme c) =>
        CurrentPrompt!.DrawPrompt(g, drawEngine, c.ForeColor.Color, c.InputControlBackColor, c.ForeColor);

    internal ConsoleForm? Form =>
        CurrentPrompt ?? CurrentForm;

    internal void SetActiveControl(IControl control)
    {
        Form!.SetFocus(control);
        Form!.ActiveControl = control;
        Form!.ActiveControl.GotActiveAt = DateTime.Now;
    }

    internal void Ask(IntPtr handle, ConsoleControl parentControl, int columnCount, int rowCount, string prompt)
    {
        CurrentPrompt = new PromptForm(handle, parentControl, columnCount, rowCount, true, prompt);
        ConsoleState = ConsoleState.MessageBox;
    }

    internal void Tell(IntPtr handle, ConsoleControl parentControl, int columnCount, int rowCount, string prompt)
    {
        CurrentPrompt = new PromptForm(handle, parentControl, columnCount, rowCount, false, prompt);
        ConsoleState = ConsoleState.MessageBox;
    }

    internal void EndPrompt(bool ok)
    {
        ConsoleState = CurrentForm == null ? ConsoleState.RunningWithoutForm : ConsoleState.RunningWithForm;
        CurrentPrompt = null;
        PromptResult = ok;
    }
}