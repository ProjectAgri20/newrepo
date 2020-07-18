namespace HP.ScalableTest.Plugin.PSPullPrint
{
    partial class PSPullPrintExecutionControl
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
            this.status_RichTextBox = new System.Windows.Forms.RichTextBox();
            this.execution_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // status_RichTextBox
            // 
            this.status_RichTextBox.BackColor = System.Drawing.Color.LightGray;
            this.status_RichTextBox.Location = new System.Drawing.Point(12, 27);
            this.status_RichTextBox.Name = "status_RichTextBox";
            this.status_RichTextBox.ReadOnly = true;
            this.status_RichTextBox.Size = new System.Drawing.Size(504, 243);
            this.status_RichTextBox.TabIndex = 0;
            this.status_RichTextBox.Text = "";
            // 
            // execution_label
            // 
            this.execution_label.AutoSize = true;
            this.execution_label.Location = new System.Drawing.Point(9, 11);
            this.execution_label.Name = "execution_label";
            this.execution_label.Size = new System.Drawing.Size(90, 13);
            this.execution_label.TabIndex = 1;
            this.execution_label.Text = "Execution Status:";
            // 
            // PSPullPrintExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.execution_label);
            this.Controls.Add(this.status_RichTextBox);
            this.Name = "PSPullPrintExecutionControl";
            this.Size = new System.Drawing.Size(526, 283);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox status_RichTextBox;
        private System.Windows.Forms.Label execution_label;
    }
}
