using System;
using System.Windows.Forms;

namespace SimpleCalculator;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void consoleControl1_UserInput(object sender, ConsoleControlLibrary.UserInputEventArgs e)
    {
        if (string.Compare(e.RawInput, "Ask", StringComparison.CurrentCultureIgnoreCase) == 0)
            consoleControl1.WriteText(5, consoleControl1.Ask("Happy today?").ToString());
        else if (string.Compare(e.RawInput, "Yell", StringComparison.CurrentCultureIgnoreCase) == 0)
            consoleControl1.Tell("Things are bad...");

        try
        {
            var response = Evaluator.Eval(e.RawInput);
            consoleControl1.WriteText(5, response.ToString("n4"));
        }
        catch (Exception ex)
        {
            consoleControl1.WriteText(5, ex.Message);
        }
    }
}