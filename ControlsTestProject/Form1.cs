﻿using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ControlsTestProject;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void consoleControl1_CurrentFormChanged(object sender, EventArgs e) =>
        Debug.WriteLine("CurrentFormChanged");

    private void Form1_Load(object sender, EventArgs e)
    {
        consoleControl1.State.CurrentForm = new FreeInputForm(Handle, consoleControl1);
        consoleControl1.SetDefaultColorScheme(new ControlColorScheme());
    }
}