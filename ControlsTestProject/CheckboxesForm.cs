using System;
using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;
using ConsoleControlLibrary.Controls.Events;
using Button = ConsoleControlLibrary.Controls.Button;
using Label = ConsoleControlLibrary.Controls.Label;

namespace ControlsTestProject;

public class CheckboxesForm : ConsoleForm
{
    private readonly Checkbox _checkbox1;
    private readonly Checkbox _checkbox2;
    private readonly Button _button;

    public CheckboxesForm(IntPtr handle, ConsoleControl parentConsole) : base(handle, parentConsole)
    {
        AddControl(new Label(this, 1, 8, "Two checkboxes, two radiobuttons and a button:"));
        _checkbox1 = new Checkbox(this, true, 1, 10, 16, "Enable button");
        AddControl(_checkbox1);
        _checkbox2 = new Checkbox(this, true, 1, 11, 16, "Show button");
        AddControl(_checkbox2);
        AddControl(new Radiobutton(this, false, "group", 1, 12, 16, "Radio button 1"));
        AddControl(new Radiobutton(this, false, "group", 1, 13, 16, "Radio button 2"));
        _button = new Button(this, 1, 15, 19, "To textboxes form");
        AddControl(_button);
        TriggerFormLoadedEvent();
    }

    protected override void EventOccurred(object sender, ConsoleControlEventArgs e)
    {
        if (sender == _checkbox1)
        {
            _button.Enabled = _checkbox1.Checked;
            return;
        }
        if (sender == _checkbox2)
        {
            _button.Visible = _checkbox2.Checked;
            return;
        }
        if (sender == _button)
        {
            ParentConsole.State.CurrentForm = new TextboxesForm(Handle, ParentConsole);
        }
    }
}