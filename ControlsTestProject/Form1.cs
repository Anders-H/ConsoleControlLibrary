using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ControlsTestProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void consoleControl1_UserInput(object sender, ConsoleControlLibrary.UserInputEventArgs e)
        {
            switch (e.RawInput.Trim().ToLower())
            {
                case "testform1":
                    consoleControl1.CurrentForm = new TestForm1(consoleControl1);
                    break;
            }
        }

        private void consoleControl1_CurrentFormChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("CurrentFormChanged");
        }
    }
}
