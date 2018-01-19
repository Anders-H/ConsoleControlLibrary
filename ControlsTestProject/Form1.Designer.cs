namespace ControlsTestProject
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ConsoleControlLibrary.DrawEngine drawEngine1 = new ConsoleControlLibrary.DrawEngine();
            this.consoleControl1 = new ConsoleControlLibrary.ConsoleControl();
            this.SuspendLayout();
            // 
            // consoleControl1
            // 
            this.consoleControl1.ColumnCount = 50;
            this.consoleControl1.CurrentForm = null;
            this.consoleControl1.CursorPosition = 0;
            this.consoleControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoleControl1.DrawEngine = drawEngine1;
            this.consoleControl1.Location = new System.Drawing.Point(0, 0);
            this.consoleControl1.Name = "consoleControl1";
            this.consoleControl1.Size = new System.Drawing.Size(932, 665);
            this.consoleControl1.TabIndex = 0;
            this.consoleControl1.CurrentFormChanged += new System.EventHandler(this.consoleControl1_CurrentFormChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 665);
            this.Controls.Add(this.consoleControl1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ConsoleControlLibrary.ConsoleControl consoleControl1;
    }
}

