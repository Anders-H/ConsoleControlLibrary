#nullable enable
using System;
using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;
using ConsoleControlLibrary.Controls.Events;
using ConsoleControlLibrary.Controls.Picture;

namespace ControlsTestProject
{
    internal class TextBasedVectorImageForm : ConsoleForm
    {
        private readonly IntPtr _handle;
        private readonly Button _button;

        public TextBasedVectorImageForm(IntPtr handle, ConsoleControl parentConsole) : base(parentConsole)
        {
            _handle = handle;
            _button = new Button(this, 10, 30, 17, "To buttons form");
            AddControl(_button);
            var picture = new ClientPicture(this, 0, 0, 90, 20);
            AddControl(picture);
            var _ = new TextBasedVectorImage(parentConsole.DrawEngine, picture, 320, 200, @"

// Clear the drawing area;
CLEAR #000066;

// Single line with color information;
LINE #ff0000 (0,199)-(319,0);
LINE #00ff00 (0,0)-(319,199);

// Polyline without color information;
LINE (10,10)-(20,10)-(20,20)-(10,20)-(10,10);
");
        }

        protected override void EventOccurred(object sender, ConsoleControlEventArgs e)
        {
            if (sender == _button)
            {
                ParentConsole.CurrentForm = new ButtonsForm(_handle, ParentConsole);
            }
        }
    }
}