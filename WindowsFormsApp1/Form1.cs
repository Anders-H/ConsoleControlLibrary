﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void consoleControl1_UserInput(object sender, ConsoleControlLibrary.UserInputEventArgs e)
        {
            Text = e.RawInput;
            if (e.RawInput == "WORDWRAP")
                consoleControl1.WriteText(50, "Jag är en äppelhäst som gillar wordwrapping. DettaOrdÄrSåLångtAttDetInteFårPlatsPåEnEndaRadUtanMåsteHeltEnkeltBrytasPåMitten. Så är det. Texten är lång. Det är bra.");
        }
    }
}
