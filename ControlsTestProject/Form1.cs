﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using ConsoleControlLibrary;
using ConsoleControlLibrary.Controls;

namespace ControlsTestProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void consoleControl1_CurrentFormChanged(object sender, EventArgs e) => Debug.WriteLine("CurrentFormChanged");
        private void Form1_Load(object sender, EventArgs e) => consoleControl1.CurrentForm = new ButtonsForm(consoleControl1, new DrawEngine());
        private void consoleControl1_ControlEvent(object sender, ConsoleControlEventArgs e) => MessageBox.Show($@"{sender.GetType().Name}: {e.EventType}");
    }
}
