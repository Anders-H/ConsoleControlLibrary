using System;
using System.Diagnostics;
using System.Windows.Forms;
using ConsoleControlLibrary;

namespace ControlsTestProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void consoleControl1_CurrentFormChanged(object sender, EventArgs e) => Debug.WriteLine("CurrentFormChanged");
        private void Form1_Load(object sender, EventArgs e) => consoleControl1.CurrentForm = new ButtonsForm(consoleControl1);
    }
}
