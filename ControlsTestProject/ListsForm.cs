﻿using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;
using ConsoleControlLibrary.Controls.Events;
using Button = ConsoleControlLibrary.Controls.Button;
using Label = ConsoleControlLibrary.Controls.Label;
using ListView = ConsoleControlLibrary.Controls.ListView;

namespace ControlsTestProject;

internal class ListsForm : ConsoleForm
{
    private readonly Button _buttonTell;
    private readonly Button _buttonAsk;
    private readonly Button _buttonFirst;
    private readonly Button _buttonLast;
    private readonly Button _button;
    private readonly SimpleList _simpleList;
    private readonly SimpleList _longList;
    private readonly ListView _listView;
    private readonly Button _writeLots;
    private readonly Button _input;


    public ListsForm(IntPtr handle, ConsoleControl parentConsole) : base(handle, parentConsole)
    {
        AddControl(new Label(this, 1, 1, "Short list:"));
        
        _simpleList = new SimpleList(this, 1, 2, 20, 5);
        _simpleList.AddItem(new ListItem(1, "Hello!"));
        _simpleList.AddItem(new ListItem(2, "Item number 2"));
        _simpleList.AddItem(new ListItem(3, "Hello again!"));
        AddControl(_simpleList);

        AddControl(new Label(this, 22, 1, "Long list:"));

        _longList = new SimpleList(this, 22, 2, 20, 5);
        _longList.AddItem(new ListItem(1, "Hello!"));
        _longList.AddItem(new ListItem(2, "Item number 2"));
        _longList.AddItem(new ListItem(3, "Hello again!"));
        _longList.AddItem(new ListItem(4, "Hello again!"));
        _longList.AddItem(new ListItem(5, "Hello again!"));
        _longList.AddItem(new ListItem(6, "Hello again!"));
        _longList.AddItem(new ListItem(7, "Hello again 7"));
        _longList.AddItem(new ListItem(8, "Hello again 8"));
        AddControl(_longList);


        _listView = new ListView(this, 1, 10, 45, 6);
        _listView.AddColumn("Left", 15, HorizontalAlign.Left);
        _listView.AddColumn("Center", 15, HorizontalAlign.Center);
        _listView.AddColumn("Right", 15, HorizontalAlign.Right);
        _listView.AddItem(new ListItem(1, "Hello!"), "Middle column", "Right");
        _listView.AddItem(new ListItem(2, "Item number 2"));
        _listView.AddItem(new ListItem(3, "Hello again!"));
        _listView.AddItem(new ListItem(4, "The fourth!"));
        _listView.AddItem(new ListItem(5, "Hello! 5"));
        _listView.AddItem(new ListItem(6, "Hello! 6"));
        _listView.AddItem(new ListItem(7, "Hello! 7"), "Middle", "Rgt");
        _listView.AddItem(new ListItem(8, "Hello! 8"));
        _listView.AddItem(new ListItem(9, "Hello! 9 Values does not fit"), "qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq", "wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
        _listView.AddItem(new ListItem(10, "The End"), "", "Fine!");
        AddControl(_listView);

        _buttonTell = new Button(this, 3, 23, 24, "Tell");
        _buttonAsk = new Button(this, 3, 24, 24, "Ask");
        _buttonFirst = new Button(this, 3, 25, 24, "Select first");
        _buttonFirst = new Button(this, 3, 26, 24, "Select first");
        _buttonLast = new Button(this, 3, 27, 24, "Select last");
        _button = new Button(this, 3, 28, 24, "To text buttons form");
        AddControl(_buttonTell);
        AddControl(_buttonAsk);
        AddControl(_buttonFirst);
        AddControl(_buttonLast);
        AddControl(_button);

        _writeLots = new Button(this, 28, 18, 24, "Write lots");
        _input = new Button(this, 28, 19, 24, "Do input");
    }

    protected override async void EventOccurred(object sender, ConsoleControlEventArgs e)
    {
        if (sender == _simpleList)
            MessageBox.Show($@"You selected from the simple list: {_simpleList.SelectedItem}");
        else if (sender == _listView)
            MessageBox.Show($@"You selected from the list view: {_listView.SelectedItem}");
        else if (sender == _buttonTell)
            ParentConsole.Tell("Hello!");
        else if (sender == _buttonAsk)
            ParentConsole.Tell($"Your answer: {await ParentConsole.Ask("What?!?")}");
        else if (sender == _button)
            ParentConsole.State.CurrentForm = new ButtonsForm(Handle, ParentConsole);
        else if (sender == _buttonFirst)
            _simpleList.SelectedIndex = 0;
        else if (sender == _buttonLast)
            _simpleList.SelectedIndex = 2;
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