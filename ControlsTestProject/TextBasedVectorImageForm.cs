﻿using System;
using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;
using ConsoleControlLibrary.Controls.Events;
using ConsoleControlLibrary.Controls.Picture;

namespace ControlsTestProject;

internal class TextBasedVectorImageForm : ConsoleForm
{
    private readonly Button _button;

    public TextBasedVectorImageForm(IntPtr handle, ConsoleControl parentConsole) : base(handle, parentConsole)
    {
        _button = new Button(this, 10, 29, 17, "To lists form");
        AddControl(_button);
        var picture = new ClientPicture(this, 0, 0, 90, 20);
        AddControl(picture);
        var _ = new TextBasedVectorImage(parentConsole.DrawEngine, picture, 320, 200, @"

// Clear the drawing area;
CLEAR #000044;

// Single line with color information;
LINE #ff0000 (0,199)-(319,0);
LINE #00ff00 (0,0)-(319,199);

// Polyline without color information;
LINE (10,10)-(30,10)-(30,30)-(10,30)-(10,10);

// Box outline with and without color;
BOX #00ff00 (20,20,20,20);
BOX (30,30,20,20);

// Filled box with and without color;
BOX-FILLED #0000ff (40,40,20,20);
BOX-FILLED (50,50,20,20);

");
    }

    protected override void EventOccurred(object sender, ConsoleControlEventArgs e)
    {
        if (sender == _button)
            ParentConsole.State.CurrentForm = new ListsForm(Handle, ParentConsole);
    }
}