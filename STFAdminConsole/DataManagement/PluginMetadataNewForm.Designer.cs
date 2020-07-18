namespace HP.ScalableTest.LabConsole.DataManagement
{
    partial class PluginMetadataNewForm
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
            this.PluginComboBox = new System.Windows.Forms.ComboBox();
            this.NewFormLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancel_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PluginComboBox
            // 
            this.PluginComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PluginComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PluginComboBox.FormattingEnabled = true;
            this.PluginComboBox.Location = new System.Drawing.Point(11, 66);
            this.PluginComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.PluginComboBox.Name = "PluginComboBox";
            this.PluginComboBox.Size = new System.Drawing.Size(408, 21);
            this.PluginComboBox.TabIndex = 0;
            // 
            // NewFormLabel
            // 
            this.NewFormLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewFormLabel.Location = new System.Drawing.Point(11, 9);
            this.NewFormLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.NewFormLabel.Name = "NewFormLabel";
            this.NewFormLabel.Size = new System.Drawing.Size(408, 52);
            this.NewFormLabel.TabIndex = 2;
            this.NewFormLabel.Text = "Label1";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(303, 102);
            this.okButton.Margin = new System.Windows.Forms.Padding(2);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(56, 24);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancel_Button
            // 
            this.cancel_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_Button.Location = new System.Drawing.Point(363, 102);
            this.cancel_Button.Margin = new System.Windows.Forms.Padding(2);
            this.cancel_Button.Name = "cancel_Button";
            this.cancel_Button.Size = new System.Drawing.Size(56, 24);
            this.cancel_Button.TabIndex = 9;
            this.cancel_Button.Text = "Cancel";
            this.cancel_Button.UseVisualStyleBackColor = true;
            // 
            // PluginMetadataNewForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel_Button;
            this.ClientSize = new System.Drawing.Size(430, 128);
            this.Controls.Add(this.cancel_Button);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.NewFormLabel);
            this.Controls.Add(this.PluginComboBox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PluginMetadataNewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Plugin Reference";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox PluginComboBox;
        private System.Windows.Forms.Label NewFormLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancel_Button;
    }
}