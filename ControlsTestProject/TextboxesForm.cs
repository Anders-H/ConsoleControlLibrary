using System;
using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;
using ConsoleControlLibrary.Controls.Events;

namespace ControlsTestProject;

public class TextboxesForm : ConsoleForm
{
    private readonly Button _button;

    public TextboxesForm(IntPtr handle, ConsoleControl parentConsole) : base(handle, parentConsole)
    {
        AddControl(new Label(this, 3, 5, "Write some text (5 characters):"));
        var textBox1 = new TextBox(this, false, 3, 6, 10, 5);
        AddControl(textBox1);
        AddControl(new Label(this, 3, 7, "Write some text (40 characters):"));
        var textBox2 = new TextBox(this, false, 3, 8, 10, 40);
        AddControl(textBox2);
        AddControl(new Label(this, 3, 9, "Write a number (0-100):"));
        var textBox3 = new TextBox(this, false, 3, 10, 10, 3);
        AddControl(textBox3);
        _button = new Button(this, 3, 15, 24, "To text adventure form");
        AddControl(_button);
        TriggerFormLoadedEvent();
    }

    protected override void EventOccurred(object sender, ConsoleControlEventArgs e)
    {
        if (sender == _button)
            ParentConsole.State.CurrentForm = new TextAdventureForm(Handle, ParentConsole);
    }
}