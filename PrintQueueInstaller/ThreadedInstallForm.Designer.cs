namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Form to define how many threads to use for installation.
    /// </summary>
    partial class ThreadedInstallForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThreadedInstallForm));
            this.multiThread_GroupBox = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.threadCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.multiThread_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadCount_NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // multiThread_GroupBox
            // 
            this.multiThread_GroupBox.Controls.Add(this.label5);
            this.multiThread_GroupBox.Controls.Add(this.threadCount_NumericUpDown);
            this.multiThread_GroupBox.Controls.Add(this.textBox1);
            this.multiThread_GroupBox.Location = new System.Drawing.Point(12, 12);
            this.multiThread_GroupBox.Name = "multiThread_GroupBox";
            this.multiThread_GroupBox.Size = new System.Drawing.Size(327, 112);
            this.multiThread_GroupBox.TabIndex = 40;
            this.multiThread_GroupBox.TabStop = false;
            this.multiThread_GroupBox.Text = "Advanced Queue Install Feature";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "Thread Count";
            // 
            // threadCount_NumericUpDown
            // 
            this.threadCount_NumericUpDown.Location = new System.Drawing.Point(84, 80);
            this.threadCount_NumericUpDown.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.threadCount_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.threadCount_NumericUpDown.Name = "threadCount_NumericUpDown";
            this.threadCount_NumericUpDown.Size = new System.Drawing.Size(78, 20);
            this.threadCount_NumericUpDown.TabIndex = 0;
            this.threadCount_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.textBox1.ForeColor = System.Drawing.Color.DarkRed;
            this.textBox1.Location = new System.Drawing.Point(6, 19);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(315, 55);
            this.textBox1.TabIndex = 37;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // ok_Button
            // 
            this.ok_Button.Location = new System.Drawing.Point(183, 130);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 41;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            this.ok_Button.Click += new System.EventHandler(this.ok_Button_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(264, 130);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(75, 23);
            this.cancel_Button.TabIndex = 42;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // ThreadedInstallForm
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(352, 165);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.multiThread_GroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ThreadedInstallForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Multi-Threaded Queue Install";
            this.Load += new System.EventHandler(this.ThreadedInstallForm_Load);
            this.multiThread_GroupBox.ResumeLayout(false);
            this.multiThread_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadCount_NumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox multiThread_GroupBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown threadCount_NumericUpDown;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel_Button;
    }
}