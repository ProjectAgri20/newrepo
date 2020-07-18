namespace HP.ScalableTest.Plugin.UdocxScan
{
    partial class UdocxScanConfigurationControl
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
            this.components = new System.ComponentModel.Container();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.ScanConfiguration_groupBox = new System.Windows.Forms.GroupBox();
            this.EmailAddress_textBox = new System.Windows.Forms.TextBox();
            this.EmailAddress_label = new System.Windows.Forms.Label();
            this.ScantoSharePoint365_radioButton = new System.Windows.Forms.RadioButton();
            this.ScantoOneDrive_radioButton = new System.Windows.Forms.RadioButton();
            this.ScantoEmail_radioButton = new System.Windows.Forms.RadioButton();
            this.AuthenticationConfiguration_groupBox = new System.Windows.Forms.GroupBox();
            this.AuthenticationMethod_label = new System.Windows.Forms.Label();
            this.AuthProvider_comboBox = new System.Windows.Forms.ComboBox();
            this.ScanConfiguration_groupBox.SuspendLayout();
            this.AuthenticationConfiguration_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(11, 285);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(726, 207);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(493, 226);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 1;
            // 
            // ScanConfiguration_groupBox
            // 
            this.ScanConfiguration_groupBox.Controls.Add(this.EmailAddress_textBox);
            this.ScanConfiguration_groupBox.Controls.Add(this.EmailAddress_label);
            this.ScanConfiguration_groupBox.Controls.Add(this.ScantoSharePoint365_radioButton);
            this.ScanConfiguration_groupBox.Controls.Add(this.ScantoOneDrive_radioButton);
            this.ScanConfiguration_groupBox.Controls.Add(this.ScantoEmail_radioButton);
            this.ScanConfiguration_groupBox.Location = new System.Drawing.Point(11, 3);
            this.ScanConfiguration_groupBox.Name = "ScanConfiguration_groupBox";
            this.ScanConfiguration_groupBox.Size = new System.Drawing.Size(723, 170);
            this.ScanConfiguration_groupBox.TabIndex = 2;
            this.ScanConfiguration_groupBox.TabStop = false;
            this.ScanConfiguration_groupBox.Text = "Scan Configuration";
            // 
            // EmailAddress_textBox
            // 
            this.EmailAddress_textBox.Location = new System.Drawing.Point(173, 54);
            this.EmailAddress_textBox.Name = "EmailAddress_textBox";
            this.EmailAddress_textBox.Size = new System.Drawing.Size(240, 27);
            this.EmailAddress_textBox.TabIndex = 6;
            // 
            // EmailAddress_label
            // 
            this.EmailAddress_label.AutoSize = true;
            this.EmailAddress_label.Location = new System.Drawing.Point(71, 57);
            this.EmailAddress_label.Name = "EmailAddress_label";
            this.EmailAddress_label.Size = new System.Drawing.Size(110, 20);
            this.EmailAddress_label.TabIndex = 3;
            this.EmailAddress_label.Text = "Email Address :";
            // 
            // ScantoSharePoint365_radioButton
            // 
            this.ScantoSharePoint365_radioButton.AutoSize = true;
            this.ScantoSharePoint365_radioButton.Location = new System.Drawing.Point(29, 121);
            this.ScantoSharePoint365_radioButton.Name = "ScantoSharePoint365_radioButton";
            this.ScantoSharePoint365_radioButton.Size = new System.Drawing.Size(181, 24);
            this.ScantoSharePoint365_radioButton.TabIndex = 2;
            this.ScantoSharePoint365_radioButton.Text = "Scan to SharePoint 365";
            this.ScantoSharePoint365_radioButton.UseVisualStyleBackColor = true;
            this.ScantoSharePoint365_radioButton.CheckedChanged += new System.EventHandler(this.ScantoEmail_radioButton_CheckedChanged);
            // 
            // ScantoOneDrive_radioButton
            // 
            this.ScantoOneDrive_radioButton.AutoSize = true;
            this.ScantoOneDrive_radioButton.Location = new System.Drawing.Point(29, 91);
            this.ScantoOneDrive_radioButton.Name = "ScantoOneDrive_radioButton";
            this.ScantoOneDrive_radioButton.Size = new System.Drawing.Size(145, 24);
            this.ScantoOneDrive_radioButton.TabIndex = 1;
            this.ScantoOneDrive_radioButton.Text = "Scan to OneDrive";
            this.ScantoOneDrive_radioButton.UseVisualStyleBackColor = true;
            this.ScantoOneDrive_radioButton.CheckedChanged += new System.EventHandler(this.ScantoEmail_radioButton_CheckedChanged);
            // 
            // ScantoEmail_radioButton
            // 
            this.ScantoEmail_radioButton.AutoSize = true;
            this.ScantoEmail_radioButton.Checked = true;
            this.ScantoEmail_radioButton.Location = new System.Drawing.Point(29, 26);
            this.ScantoEmail_radioButton.Name = "ScantoEmail_radioButton";
            this.ScantoEmail_radioButton.Size = new System.Drawing.Size(158, 24);
            this.ScantoEmail_radioButton.TabIndex = 0;
            this.ScantoEmail_radioButton.TabStop = true;
            this.ScantoEmail_radioButton.Text = "Scan to Mail/Drafts";
            this.ScantoEmail_radioButton.UseVisualStyleBackColor = true;
            this.ScantoEmail_radioButton.CheckedChanged += new System.EventHandler(this.ScantoEmail_radioButton_CheckedChanged);
            // 
            // AuthenticationConfiguration_groupBox
            // 
            this.AuthenticationConfiguration_groupBox.Controls.Add(this.AuthProvider_comboBox);
            this.AuthenticationConfiguration_groupBox.Controls.Add(this.AuthenticationMethod_label);
            this.AuthenticationConfiguration_groupBox.Location = new System.Drawing.Point(11, 179);
            this.AuthenticationConfiguration_groupBox.Name = "AuthenticationConfiguration_groupBox";
            this.AuthenticationConfiguration_groupBox.Size = new System.Drawing.Size(476, 100);
            this.AuthenticationConfiguration_groupBox.TabIndex = 3;
            this.AuthenticationConfiguration_groupBox.TabStop = false;
            this.AuthenticationConfiguration_groupBox.Text = "Authentication Configuration";
            // 
            // AuthenticationMethod_label
            // 
            this.AuthenticationMethod_label.AutoSize = true;
            this.AuthenticationMethod_label.Location = new System.Drawing.Point(26, 29);
            this.AuthenticationMethod_label.Name = "AuthenticationMethod_label";
            this.AuthenticationMethod_label.Size = new System.Drawing.Size(169, 20);
            this.AuthenticationMethod_label.TabIndex = 0;
            this.AuthenticationMethod_label.Text = "Authentication Method :";
            // 
            // AuthProvider_comboBox
            // 
            this.AuthProvider_comboBox.DisplayMember = "Value";
            this.AuthProvider_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AuthProvider_comboBox.FormattingEnabled = true;
            this.AuthProvider_comboBox.Location = new System.Drawing.Point(202, 29);
            this.AuthProvider_comboBox.Margin = new System.Windows.Forms.Padding(4);
            this.AuthProvider_comboBox.Name = "AuthProvider_comboBox";
            this.AuthProvider_comboBox.Size = new System.Drawing.Size(225, 28);
            this.AuthProvider_comboBox.TabIndex = 3;
            this.AuthProvider_comboBox.ValueMember = "Key";
            // 
            // UdocxScanConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.AuthenticationConfiguration_groupBox);
            this.Controls.Add(this.ScanConfiguration_groupBox);
            this.Controls.Add(this.lockTimeoutControl);
            this.Controls.Add(this.assetSelectionControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UdocxScanConfigurationControl";
            this.Size = new System.Drawing.Size(757, 500);
            this.ScanConfiguration_groupBox.ResumeLayout(false);
            this.ScanConfiguration_groupBox.PerformLayout();
            this.AuthenticationConfiguration_groupBox.ResumeLayout(false);
            this.AuthenticationConfiguration_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private System.Windows.Forms.GroupBox ScanConfiguration_groupBox;
        private System.Windows.Forms.GroupBox AuthenticationConfiguration_groupBox;
        private System.Windows.Forms.Label AuthenticationMethod_label;
        private System.Windows.Forms.TextBox EmailAddress_textBox;
        private System.Windows.Forms.Label EmailAddress_label;
        private System.Windows.Forms.RadioButton ScantoSharePoint365_radioButton;
        private System.Windows.Forms.RadioButton ScantoOneDrive_radioButton;
        private System.Windows.Forms.RadioButton ScantoEmail_radioButton;
        private System.Windows.Forms.ComboBox AuthProvider_comboBox;
    }
}
