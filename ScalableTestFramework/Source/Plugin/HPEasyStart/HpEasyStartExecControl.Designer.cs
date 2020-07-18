namespace HP.ScalableTest.Plugin.HpEasyStart
{
    partial class HpEasyStartExecControl
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
            this.status_richTextBox = new System.Windows.Forms.RichTextBox();
            this.labelExecStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // status_richTextBox
            // 
            this.status_richTextBox.Location = new System.Drawing.Point(16, 56);
            this.status_richTextBox.Name = "status_richTextBox";
            this.status_richTextBox.Size = new System.Drawing.Size(552, 301);
            this.status_richTextBox.TabIndex = 0;
            this.status_richTextBox.Text = "";
            // 
            // labelExecStatus
            // 
            this.labelExecStatus.AutoSize = true;
            this.labelExecStatus.Location = new System.Drawing.Point(13, 29);
            this.labelExecStatus.Name = "labelExecStatus";
            this.labelExecStatus.Size = new System.Drawing.Size(87, 13);
            this.labelExecStatus.TabIndex = 1;
            this.labelExecStatus.Text = "Execution Status";
            // 
            // HpEasyStartExecControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelExecStatus);
            this.Controls.Add(this.status_richTextBox);
            this.Name = "HpEasyStartExecControl";
            this.Size = new System.Drawing.Size(587, 374);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox status_richTextBox;
        private System.Windows.Forms.Label labelExecStatus;
    }
}
