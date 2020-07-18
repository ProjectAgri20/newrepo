namespace HP.ScalableTest.UI
{
    partial class AssetPoolEditForm
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
            this.formatLabel = new System.Windows.Forms.Label();
            this.poolNameLabel = new System.Windows.Forms.Label();
            this.poolNameTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.reservation_Checkbox = new System.Windows.Forms.CheckBox();
            this.administratorComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // formatLabel
            // 
            this.formatLabel.Location = new System.Drawing.Point(-55, 39);
            this.formatLabel.Name = "formatLabel";
            this.formatLabel.Size = new System.Drawing.Size(137, 24);
            this.formatLabel.TabIndex = 110;
            this.formatLabel.Text = "Administrator";
            this.formatLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // poolNameLabel
            // 
            this.poolNameLabel.Location = new System.Drawing.Point(-14, 9);
            this.poolNameLabel.Name = "poolNameLabel";
            this.poolNameLabel.Size = new System.Drawing.Size(96, 24);
            this.poolNameLabel.TabIndex = 108;
            this.poolNameLabel.Text = "Pool Name";
            this.poolNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // poolNameTextBox
            // 
            this.poolNameTextBox.Location = new System.Drawing.Point(114, 9);
            this.poolNameTextBox.Name = "poolNameTextBox";
            this.poolNameTextBox.Size = new System.Drawing.Size(180, 20);
            this.poolNameTextBox.TabIndex = 109;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(90, 132);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(100, 32);
            this.okButton.TabIndex = 119;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(196, 132);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 32);
            this.cancelButton.TabIndex = 120;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // reservation_Checkbox
            // 
            this.reservation_Checkbox.AutoSize = true;
            this.reservation_Checkbox.Location = new System.Drawing.Point(113, 98);
            this.reservation_Checkbox.Name = "reservation_Checkbox";
            this.reservation_Checkbox.Size = new System.Drawing.Size(119, 17);
            this.reservation_Checkbox.TabIndex = 121;
            this.reservation_Checkbox.Text = "Track Reservations";
            this.reservation_Checkbox.UseVisualStyleBackColor = true;
            // 
            // administratorComboBox
            // 
            this.administratorComboBox.FormattingEnabled = true;
            this.administratorComboBox.Location = new System.Drawing.Point(113, 41);
            this.administratorComboBox.Name = "administratorComboBox";
            this.administratorComboBox.Size = new System.Drawing.Size(181, 21);
            this.administratorComboBox.TabIndex = 110;
            // 
            // AssetPoolEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 176);
            this.Controls.Add(this.administratorComboBox);
            this.Controls.Add(this.reservation_Checkbox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.formatLabel);
            this.Controls.Add(this.poolNameLabel);
            this.Controls.Add(this.poolNameTextBox);
            this.Name = "AssetPoolEditForm";
            this.Text = "Asset Pool Edit Form";
            this.Load += new System.EventHandler(this.AssetPoolEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label formatLabel;
        private System.Windows.Forms.Label poolNameLabel;
        private System.Windows.Forms.TextBox poolNameTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox reservation_Checkbox;
        private System.Windows.Forms.ComboBox administratorComboBox;
    }
}