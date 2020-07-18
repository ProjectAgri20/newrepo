namespace HP.ScalableTest.Print.Utility
{
    partial class SystemSelectionDialog
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
            this.env_ComboBox = new System.Windows.Forms.ComboBox();
            this.ok_Button = new System.Windows.Forms.Button();
            this.cancel__Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // env_ComboBox
            // 
            this.env_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.env_ComboBox.FormattingEnabled = true;
            this.env_ComboBox.Location = new System.Drawing.Point(140, 20);
            this.env_ComboBox.Name = "env_ComboBox";
            this.env_ComboBox.Size = new System.Drawing.Size(195, 21);
            this.env_ComboBox.TabIndex = 0;
            // 
            // ok_Button
            // 
            this.ok_Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok_Button.Location = new System.Drawing.Point(179, 61);
            this.ok_Button.Name = "ok_Button";
            this.ok_Button.Size = new System.Drawing.Size(75, 23);
            this.ok_Button.TabIndex = 1;
            this.ok_Button.Text = "OK";
            this.ok_Button.UseVisualStyleBackColor = true;
            // 
            // cancel__Button
            // 
            this.cancel__Button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel__Button.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel__Button.Location = new System.Drawing.Point(260, 61);
            this.cancel__Button.Name = "cancel__Button";
            this.cancel__Button.Size = new System.Drawing.Size(75, 23);
            this.cancel__Button.TabIndex = 2;
            this.cancel__Button.Text = "Cancel";
            this.cancel__Button.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select STF Environment";
            // 
            // SystemSelectionDialog
            // 
            this.AcceptButton = this.ok_Button;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel__Button;
            this.ClientSize = new System.Drawing.Size(347, 96);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancel__Button);
            this.Controls.Add(this.ok_Button);
            this.Controls.Add(this.env_ComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SystemSelectionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Environment Selection Dialog";
            this.Load += new System.EventHandler(this.SystemSelectionDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox env_ComboBox;
        private System.Windows.Forms.Button ok_Button;
        private System.Windows.Forms.Button cancel__Button;
        private System.Windows.Forms.Label label1;
    }
}