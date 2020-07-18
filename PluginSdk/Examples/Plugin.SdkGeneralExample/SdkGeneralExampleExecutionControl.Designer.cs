namespace Plugin.SdkGeneralExample
{
    partial class SdkGeneralExampleExecutionControl
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
            this.status_Label = new System.Windows.Forms.Label();
            this.status_RichTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxActivity = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // status_Label
            // 
            this.status_Label.AutoSize = true;
            this.status_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status_Label.Location = new System.Drawing.Point(4, 30);
            this.status_Label.Name = "status_Label";
            this.status_Label.Size = new System.Drawing.Size(100, 15);
            this.status_Label.TabIndex = 0;
            this.status_Label.Text = "Execution Status";
            // 
            // status_RichTextBox
            // 
            this.status_RichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.status_RichTextBox.Location = new System.Drawing.Point(6, 48);
            this.status_RichTextBox.Name = "status_RichTextBox";
            this.status_RichTextBox.ReadOnly = true;
            this.status_RichTextBox.Size = new System.Drawing.Size(341, 199);
            this.status_RichTextBox.TabIndex = 1;
            this.status_RichTextBox.Text = "";
            this.status_RichTextBox.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Activity Label:";
            // 
            // textBoxActivity
            // 
            this.textBoxActivity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxActivity.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxActivity.Enabled = false;
            this.textBoxActivity.Location = new System.Drawing.Point(92, 4);
            this.textBoxActivity.Name = "textBoxActivity";
            this.textBoxActivity.Size = new System.Drawing.Size(255, 23);
            this.textBoxActivity.TabIndex = 3;
            // 
            // SdkGeneralExampleExecutionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxActivity);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.status_RichTextBox);
            this.Controls.Add(this.status_Label);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SdkGeneralExampleExecutionControl";
            this.Size = new System.Drawing.Size(350, 250);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label status_Label;
        private System.Windows.Forms.RichTextBox status_RichTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxActivity;
    }
}
