namespace ConsoleControlLibrary
{
    partial class ConsoleControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ConsoleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Name = "ConsoleControl";
            this.Size = new System.Drawing.Size(504, 327);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ConsoleControl_Paint);
            this.Enter += new System.EventHandler(this.ConsoleControl_Enter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ConsoleControl_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ConsoleControl_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ConsoleControl_KeyUp);
            this.Leave += new System.EventHandler(this.ConsoleControl_Leave);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ConsoleControl_MouseClick);
            this.Resize += new System.EventHandler(this.ConsoleControl_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
    }
}
