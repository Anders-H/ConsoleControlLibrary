﻿using System;
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