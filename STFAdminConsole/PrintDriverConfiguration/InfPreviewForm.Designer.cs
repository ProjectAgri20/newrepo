namespace HP.ScalableTest.LabConsole
{
    partial class InfPreviewForm
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
            this.listBox_Files = new System.Windows.Forms.ListBox();
            this.label_DriverName = new System.Windows.Forms.Label();
            this.textBox_DriverName = new System.Windows.Forms.TextBox();
            this.label_Prompt = new System.Windows.Forms.Label();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox_Files
            // 
            this.listBox_Files.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_Files.DisplayMember = "Value";
            this.listBox_Files.FormattingEnabled = true;
            this.listBox_Files.Location = new System.Drawing.Point(6, 32);
            this.listBox_Files.Name = "listBox_Files";
            this.listBox_Files.Size = new System.Drawing.Size(502, 108);
            this.listBox_Files.TabIndex = 0;
            this.listBox_Files.ValueMember = "Key";
            this.listBox_Files.SelectedValueChanged += new System.EventHandler(this.listBox_Files_SelectedValueChanged);
            // 
            // label_DriverName
            // 
            this.label_DriverName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_DriverName.AutoSize = true;
            this.label_DriverName.Location = new System.Drawing.Point(7, 154);
            this.label_DriverName.Name = "label_DriverName";
            this.label_DriverName.Size = new System.Drawing.Size(69, 13);
            this.label_DriverName.TabIndex = 1;
            this.label_DriverName.Text = "Driver Name:";
            // 
            // textBox_DriverName
            // 
            this.textBox_DriverName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_DriverName.Location = new System.Drawing.Point(82, 151);
            this.textBox_DriverName.Name = "textBox_DriverName";
            this.textBox_DriverName.Size = new System.Drawing.Size(426, 20);
            this.textBox_DriverName.TabIndex = 2;
            // 
            // label_Prompt
            // 
            this.label_Prompt.Location = new System.Drawing.Point(12, 9);
            this.label_Prompt.Name = "label_Prompt";
            this.label_Prompt.Size = new System.Drawing.Size(496, 18);
            this.label_Prompt.TabIndex = 3;
            this.label_Prompt.Text = "Select file you want to use.";
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(433, 178);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 4;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            // 
            // button_Ok
            // 
            this.button_Ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Ok.Location = new System.Drawing.Point(352, 178);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 23);
            this.button_Ok.TabIndex = 5;
            this.button_Ok.Text = "OK";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // InfPreviewForm
            // 
            this.AcceptButton = this.button_Ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(515, 204);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.label_Prompt);
            this.Controls.Add(this.textBox_DriverName);
            this.Controls.Add(this.label_DriverName);
            this.Controls.Add(this.listBox_Files);
            this.Name = "InfPreviewForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InfPreviewForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Files;
        private System.Windows.Forms.Label label_DriverName;
        private System.Windows.Forms.TextBox textBox_DriverName;
        private System.Windows.Forms.Label label_Prompt;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Button button_Ok;
    }
}