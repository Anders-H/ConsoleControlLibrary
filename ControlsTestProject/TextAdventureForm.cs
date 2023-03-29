using System;
using System.Drawing;
using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;
using ConsoleControlLibrary.Controls.Events;
using ConsoleControlLibrary.Controls.Picture;

namespace ControlsTestProject;

internal class TextAdventureForm : ConsoleForm
{
    private readonly TextBlock _output;
    private readonly TextBox _input;

    public TextAdventureForm(IntPtr handle, ConsoleControl parentConsole) : base(handle, parentConsole)
    {
        var picture = new ClientPicture(this, 0, 0, 90, 15);
        AddControl(picture);
        _output = new TextBlock(Handle, this, 0, 15, 90, 15, "", 2, HorizontalTextAlignment.Bottom);
        AddControl(_output);
        _input = new TextBox(this, true, 0, 29, 90, 90);
        AddControl(_input);
        TriggerFormLoadedEvent();
        var _ = new TestImage(parentConsole.DrawEngine, picture, 320, 200);
    }

    private class TestImage : VectorImageBase
    {
        public TestImage(IDrawEngine drawEngine, ClientPicture clientPicture, int virtualWidth, int virtualHeight) : base(drawEngine, clientPicture, virtualWidth, virtualHeight)
        {
        }

        public override void DrawPicture(ClientPicture picture, Graphics g)
        {
            g.DrawLine(Pens.Red, VirtualToPhysical(0, 199), VirtualToPhysical(319, 0));
            g.DrawLine(Pens.Green, VirtualToPhysical(0, 0), VirtualToPhysical(319, 199));
        }
    }

    protected override void EventOccurred(object sender, ConsoleControlEventArgs e)
    {
        switch (e.EventType)
        {
            case ConsoleControlEventType.FormLoaded:
                _output.Write("Type QUIT to continue to the text based vector image form.");
                _output.Write("");
                break;
            case ConsoleControlEventType.TextBoxEnter:
                var t = _input.Text.Trim();
                _output.Add(t);
                _input.Text = "";
                ParentConsole.Refresh();

                if (string.Compare(t, "quit", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    ParentConsole.State.CurrentForm = new TextBasedVectorImageForm(Handle, ParentConsole);
                    return;
                }

                switch (DateTime.Now.Second % 4)
                {
                    case 0:
                        _output.Write("Yes.");
                        _output.Write("");
                        break;
                    case 1:
                        _output.Write("Perhaps.");
                        _output.Write("");
                        break;
                    case 2:
                        _output.Write("No.");
                        _output.Write("");
                        break;
                    case 3:
                        _output.Write("No way!");
                        _output.Write("");
                        break;
                }
                break;
        }
    }
}