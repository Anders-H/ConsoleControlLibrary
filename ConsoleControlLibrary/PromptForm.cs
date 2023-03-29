using System;
using System.Drawing;
using System.Linq;
using ConsoleControlLibrary.Controls;
using ConsoleControlLibrary.Controls.BaseTypes;
using ConsoleControlLibrary.Controls.Events;

namespace ConsoleControlLibrary;

internal class PromptForm : ConsoleForm
{
    private readonly Button _btnOk;
    private readonly Button? _btnCancel;
    private readonly int _columnCount;
    private readonly int _rowCount;
    private readonly int _yBase;

    public PromptForm(IntPtr handle, ConsoleControl parentConsole, int columnCount, int rowCount, bool hasCancelButton, string prompt) : base(handle, parentConsole)
    {
        if (columnCount < 16)
            throw new SystemException("At least 16 columns required.");

        if (rowCount < 6)
            throw new SystemException("At least 6 rows required.");

        _columnCount = columnCount;
        _rowCount = rowCount;
        _yBase = rowCount / 2 - 1;

        if (prompt.Length > _columnCount)
        {
            prompt = prompt.Substring(0, _columnCount);
        }
        else if (prompt.Length < columnCount)
        {
            while (prompt.Length < columnCount)
                prompt = $@" {prompt} ";

            if (prompt.Length > _columnCount)
                prompt = prompt.Substring(0, _columnCount);
        }

        if (hasCancelButton)
        {
            _btnCancel = new Button(this, _columnCount - 8, _yBase + 1, 8, "Cancel");
            _btnOk = new Button(this, _columnCount - 16, _yBase + 1, 8, "OK");
            AddControl(_btnCancel);
        }
        else
        {
            _btnOk = new Button(this, _columnCount - 8, _yBase + 1, 8, "OK");
        }

        AddControl(_btnOk);
        AddControl(new Label(this, 0, _yBase - 1, prompt));
        SetFocus(_btnOk);
    }

    internal void DrawPrompt(Graphics g, IDrawEngine drawEngine, Color outlineColor, SolidBrush background, SolidBrush foreground)
    {
        CurrentColorScheme ??= new CurrentColorScheme(background.Color, foreground, background, foreground, foreground);

        using var shade = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
        drawEngine.FillControl(g, shade, new Rectangle(0, 0, _columnCount, _rowCount));

        drawEngine.FillControl(g, background, new Rectangle(0, _yBase - 1, _columnCount, 3));

        Controls
            .Where(x => x.Visible).Cast<IControlFormOperations>()
            .ToList()
            .ForEach(x => x.Draw(g, drawEngine));

        using var p = new Pen(outlineColor);
        drawEngine.OutlineControl(g, p, new Rectangle(-1, _yBase - 1, _columnCount + 1, 3));
    }

    protected override void EventOccurred(object sender, ConsoleControlEventArgs e) =>
        ParentConsole.EndPrompt(sender == _btnOk);
}