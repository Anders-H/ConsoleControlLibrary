using System;
using System.Drawing;
using ConsoleControlLibrary.Controls;

namespace ConsoleControlLibrary;

internal class PromptForm : ConsoleForm
{
    private readonly Button _btnOk;
    private readonly Button? _btnCancel;
    private readonly int _columnCount;
    private readonly int _rowCount;
    private string _prompt;

    public PromptForm(IntPtr handle, ConsoleControl parentConsole, int columnCount, int rowCount, bool hasCancelButton, string prompt) : base(handle, parentConsole)
    {
        if (columnCount < 16)
            throw new SystemException("At least 16 columns required.");

        if (rowCount < 6)
            throw new SystemException("At least 6 rows required.");

        if (prompt.Length > _columnCount)
        {
            _prompt = prompt.Substring(0, _columnCount);
        }
        else if (prompt.Length < columnCount)
        {
            _prompt = prompt;

            while (_prompt.Length < columnCount)
                _prompt = $@" {_prompt} ";

            if (prompt.Length > _columnCount)
                _prompt = prompt.Substring(0, _columnCount);
        }
        else
        {
            _prompt = prompt;
        }

        _columnCount = columnCount;
        _rowCount = rowCount;

        var yBase = _rowCount / 2;

        if (hasCancelButton)
        {
            _btnCancel = new Button(this, _columnCount - 8, yBase + 1, 8, "Cancel");
            _btnOk = new Button(this, _columnCount - 16, yBase + 1, 8, "OK");
            AddControl(_btnCancel);
        }
        else
        {
            _btnOk = new Button(this, _columnCount - 8, yBase + 1, 8, "OK");
        }

        AddControl(_btnOk);
        AddControl(new Label(this, 0, yBase - 1, _prompt));
    }

    internal override void Draw(Graphics g, IDrawEngine drawEngine)
    {

    }
}