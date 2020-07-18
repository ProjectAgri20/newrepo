namespace HP.ScalableTest.Development.UI
{
    partial class CriticalSectionMockForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.holdTimeoutRadioButton = new System.Windows.Forms.RadioButton();
            this.acquireTimeoutRadioButton = new System.Windows.Forms.RadioButton();
            this.runActionRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(340, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the desired behavior for the critical section service mock.\r\n";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.holdTimeoutRadioButton);
            this.groupBox1.Controls.Add(this.acquireTimeoutRadioButton);
            this.groupBox1.Controls.Add(this.runActionRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(15, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 110);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Critical Section Behavior";
            // 
            // holdTimeoutRadioButton
            // 
            this.holdTimeoutRadioButton.AutoSize = true;
            this.holdTimeoutRadioButton.Location = new System.Drawing.Point(16, 72);
            this.holdTimeoutRadioButton.Name = "holdTimeoutRadioButton";
            this.holdTimeoutRadioButton.Size = new System.Drawing.Size(148, 19);
            this.holdTimeoutRadioButton.TabIndex = 2;
            this.holdTimeoutRadioButton.TabStop = true;
            this.holdTimeoutRadioButton.Text = "Simulate Hold Timeout";
            this.holdTimeoutRadioButton.UseVisualStyleBackColor = true;
            // 
            // acquireTimeoutRadioButton
            // 
            this.acquireTimeoutRadioButton.AutoSize = true;
            this.acquireTimeoutRadioButton.Location = new System.Drawing.Point(16, 47);
            this.acquireTimeoutRadioButton.Name = "acquireTimeoutRadioButton";
            this.acquireTimeoutRadioButton.Size = new System.Drawing.Size(163, 19);
            this.acquireTimeoutRadioButton.TabIndex = 1;
            this.acquireTimeoutRadioButton.TabStop = true;
            this.acquireTimeoutRadioButton.Text = "Simulate Acquire Timeout";
            this.acquireTimeoutRadioButton.UseVisualStyleBackColor = true;
            // 
            // runActionRadioButton
            // 
            this.runActionRadioButton.AutoSize = true;
            this.runActionRadioButton.Location = new System.Drawing.Point(16, 22);
            this.runActionRadioButton.Name = "runActionRadioButton";
            this.runActionRadioButton.Size = new System.Drawing.Size(84, 19);
            this.runActionRadioButton.TabIndex = 0;
            this.runActionRadioButton.TabStop = true;
            this.runActionRadioButton.Text = "Run Action";
            this.runActionRadioButton.UseVisualStyleBackColor = true;
            // 
            // CriticalSectionMockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 159);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CriticalSectionMockForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Critical Section Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CriticalSectionMockForm_FormClosing);
            this.Shown += new System.EventHandler(this.CriticalSectionMockForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton holdTimeoutRadioButton;
        private System.Windows.Forms.RadioButton acquireTimeoutRadioButton;
        private System.Windows.Forms.RadioButton runActionRadioButton;
    }
}