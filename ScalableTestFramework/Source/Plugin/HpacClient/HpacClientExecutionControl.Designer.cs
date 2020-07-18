namespace HP.ScalableTest.Plugin.HpacClient
{
    partial class HpacClientExecutionControl
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
            this.statusRichTextBox = new System.Windows.Forms.RichTextBox();
            this.executionStatuslabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // statusRichTextBox
            // 
            this.statusRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusRichTextBox.Location = new System.Drawing.Point(19, 55);
            this.statusRichTextBox.Name = "statusRichTextBox";
            this.statusRichTextBox.Size = new System.Drawing.Size(533, 291);
            this.statusRichTextBox.TabIndex = 0;
            this.statusRichTextBox.Text = "";
            // 
            // executionStatuslabel
            // 
            this.executionStatuslabel.AutoSize = true;
            this.executionStatuslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.executionStatuslabel.Location = new System.Drawing.Point(16, 18);
            this.executionStatuslabel.Name = "executionStatuslabel";
            this.executionStatuslabel.Size = new System.Drawing.Size(122, 16);
            this.executionStatuslabel.TabIndex = 1;
            this.executionStatuslabel.Text = "Execution Status";
            // 
            // HpacClientExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.executionStatuslabel);
            this.Controls.Add(this.statusRichTextBox);
            this.Name = "HpacClientExecutionControl";
            this.Size = new System.Drawing.Size(583, 416);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox statusRichTextBox;
        private System.Windows.Forms.Label executionStatuslabel;
    }
}
