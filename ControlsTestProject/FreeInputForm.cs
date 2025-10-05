using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;
using ConsoleControlLibrary.Controls.Events;
using System;

namespace ControlsTestProject;

public class FreeInputForm : ConsoleForm
{
    private readonly FreeInput _freeInput;
    private readonly Button _button;

    public FreeInputForm(IntPtr handle, ConsoleControl parentConsole) : base(handle, parentConsole)
    {
        _freeInput = new FreeInput(this, 4, 4, 40, 20);
        AddControl(_freeInput);

        _button = new Button(this, 3, 28, 24, "To list form");
        AddControl(_button);

        SetFocus(_freeInput);
    }

    protected override void EventOccurred(object sender, ConsoleControlEventArgs e)
    {
        if (sender == _button)
            ParentConsole.State.CurrentForm = new ListsForm(Handle, ParentConsole);
    }
}