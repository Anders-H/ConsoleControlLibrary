﻿using System;
using System.Windows.Forms;
using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;
using ConsoleControlLibrary.Controls.Events;
using Button = ConsoleControlLibrary.Controls.Button;
using Label = ConsoleControlLibrary.Controls.Label;

namespace ControlsTestProject;

internal class ListsForm : ConsoleForm
{
    private readonly Button _buttonFirst;
    private readonly Button _buttonLast;
    private readonly Button _button;
    private readonly SimpleList _simpleList;

    public ListsForm(IntPtr handle, ConsoleControl parentConsole) : base(handle, parentConsole)
    {
        AddControl(new Label(this, 1, 1, "Select something:"));
        _simpleList = new SimpleList(this, 1, 2, 20, 5);
        _simpleList.AddItem(new ListItem(1, "Hello!"));
        _simpleList.AddItem(new ListItem(2, "Item number 2"));
        _simpleList.AddItem(new ListItem(3, "Hello again!"));
        _simpleList.AddItem(new ListItem(4, "The fourth!"));
        _simpleList.AddItem(new ListItem(5, "Hello! 5"));
        _simpleList.AddItem(new ListItem(6, "Hello! 6"));
        _simpleList.AddItem(new ListItem(7, "Hello! 7"));
        _simpleList.AddItem(new ListItem(8, "Hello! 8"));
        _simpleList.AddItem(new ListItem(9, "Hello! 9"));
        _simpleList.AddItem(new ListItem(10, "The End"));
        AddControl(_simpleList);
        _buttonFirst = new Button(this, 3, 27, 24, "Select first");
        _buttonLast = new Button(this, 3, 28, 24, "Select last");
        _button = new Button(this, 3, 29, 24, "To text buttons form");
        AddControl(_buttonFirst);
        AddControl(_buttonLast);
        AddControl(_button);
    }

    protected override void EventOccurred(object sender, ConsoleControlEventArgs e)
    {
        if (sender == _simpleList)
            MessageBox.Show($@"You selected: {_simpleList.SelectedItem}");
        else if (sender == _button)
            ParentConsole.CurrentForm = new ButtonsForm(Handle, ParentConsole);
        else if (sender == _buttonFirst)
            _simpleList.SelectedIndex = 0;
        else if (sender == _buttonLast)
            _simpleList.SelectedIndex = 9;
    }
}

internal class ListItem
{
    public int Id { get; }
    public string Text { get; }

    public ListItem(int id, string text)
    {
        Id = id;
        Text = text;
    }

    public override string ToString() =>
        Text;
}