namespace HP.ScalableTest.Plugin.SecurityConfiguration
{
    partial class SecurityConfigurationUserControl
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
            this.components = new System.ComponentModel.Container();
            this.security_assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.label1 = new System.Windows.Forms.Label();
            this.Comments = new System.Windows.Forms.Label();
            this.dot1x_groupBox = new System.Windows.Forms.GroupBox();
            this.authenticationPassword_textBox = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.snmpv3_checkBox = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.encryptionStrength_comboBox = new System.Windows.Forms.ComboBox();
            this.custom_panel = new System.Windows.Forms.Panel();
            this.mask_ipAddressControl = new System.Windows.Forms.TextBox();
            this.accesscontrol_ipAddressControl = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.snmpv1v2custom_checkBox = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.snmpreadonly_checkBox = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.enhancedSecurity_radioButton = new System.Windows.Forms.RadioButton();
            this.basicSecurity_radioButton = new System.Windows.Forms.RadioButton();
            this.customSecurity_radioButton = new System.Windows.Forms.RadioButton();
            this.label14 = new System.Windows.Forms.Label();
            this.authProtocolPassphrase_textBox = new System.Windows.Forms.TextBox();
            this.snmpv3UserName_textBox = new System.Windows.Forms.TextBox();
            this.enhanced_panel = new System.Windows.Forms.Panel();
            this.privacyProtocolPassphrase_textBox = new System.Windows.Forms.TextBox();
            this.security_fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.dot1x_groupBox.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.custom_panel.SuspendLayout();
            this.enhanced_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // security_assetSelectionControl
            // 
            this.security_assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.security_assetSelectionControl.Location = new System.Drawing.Point(7, 19);
            this.security_assetSelectionControl.Name = "security_assetSelectionControl";
            this.security_assetSelectionControl.Size = new System.Drawing.Size(607, 145);
            this.security_assetSelectionControl.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 367);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 2;
            // 
            // Comments
            // 
            this.Comments.AutoSize = true;
            this.Comments.Location = new System.Drawing.Point(3, 422);
            this.Comments.Name = "Comments";
            this.Comments.Size = new System.Drawing.Size(0, 13);
            this.Comments.TabIndex = 7;
            // 
            // dot1x_groupBox
            // 
            this.dot1x_groupBox.Controls.Add(this.authenticationPassword_textBox);
            this.dot1x_groupBox.Controls.Add(this.label25);
            this.dot1x_groupBox.Location = new System.Drawing.Point(14, 160);
            this.dot1x_groupBox.Name = "dot1x_groupBox";
            this.dot1x_groupBox.Size = new System.Drawing.Size(237, 55);
            this.dot1x_groupBox.TabIndex = 113;
            this.dot1x_groupBox.TabStop = false;
            this.dot1x_groupBox.Text = "802.1x Authentication";
            // 
            // authenticationPassword_textBox
            // 
            this.authenticationPassword_textBox.Location = new System.Drawing.Point(112, 26);
            this.authenticationPassword_textBox.Name = "authenticationPassword_textBox";
            this.authenticationPassword_textBox.Size = new System.Drawing.Size(100, 20);
            this.authenticationPassword_textBox.TabIndex = 54;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(47, 26);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(53, 13);
            this.label25.TabIndex = 53;
            this.label25.Text = "Password";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(169, 469);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(308, 13);
            this.label12.TabIndex = 124;
            this.label12.Text = "Note: Telnet Power cycle will work only on Windows 7 or above";
            // 
            // snmpv3_checkBox
            // 
            this.snmpv3_checkBox.AutoSize = true;
            this.snmpv3_checkBox.Location = new System.Drawing.Point(209, 17);
            this.snmpv3_checkBox.Name = "snmpv3_checkBox";
            this.snmpv3_checkBox.Size = new System.Drawing.Size(15, 14);
            this.snmpv3_checkBox.TabIndex = 98;
            this.snmpv3_checkBox.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.security_assetSelectionControl);
            this.groupBox5.Location = new System.Drawing.Point(22, 15);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(620, 170);
            this.groupBox5.TabIndex = 129;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Printer Details";
            // 
            // encryptionStrength_comboBox
            // 
            this.encryptionStrength_comboBox.FormattingEnabled = true;
            this.encryptionStrength_comboBox.Items.AddRange(new object[] {
            "Low",
            "Medium",
            "High"});
            this.encryptionStrength_comboBox.Location = new System.Drawing.Point(124, 15);
            this.encryptionStrength_comboBox.Name = "encryptionStrength_comboBox";
            this.encryptionStrength_comboBox.Size = new System.Drawing.Size(101, 21);
            this.encryptionStrength_comboBox.TabIndex = 109;
            // 
            // custom_panel
            // 
            this.custom_panel.Controls.Add(this.mask_ipAddressControl);
            this.custom_panel.Controls.Add(this.accesscontrol_ipAddressControl);
            this.custom_panel.Controls.Add(this.label13);
            this.custom_panel.Controls.Add(this.snmpv1v2custom_checkBox);
            this.custom_panel.Controls.Add(this.dot1x_groupBox);
            this.custom_panel.Controls.Add(this.label17);
            this.custom_panel.Controls.Add(this.label15);
            this.custom_panel.Controls.Add(this.encryptionStrength_comboBox);
            this.custom_panel.Controls.Add(this.label21);
            this.custom_panel.Location = new System.Drawing.Point(374, 223);
            this.custom_panel.Name = "custom_panel";
            this.custom_panel.Size = new System.Drawing.Size(268, 227);
            this.custom_panel.TabIndex = 131;
            // 
            // mask_ipAddressControl
            // 
            this.mask_ipAddressControl.Location = new System.Drawing.Point(126, 121);
            this.mask_ipAddressControl.MaxLength = 9;
            this.mask_ipAddressControl.Name = "mask_ipAddressControl";
            this.mask_ipAddressControl.Size = new System.Drawing.Size(100, 20);
            this.mask_ipAddressControl.TabIndex = 119;
            // 
            // accesscontrol_ipAddressControl
            // 
            this.accesscontrol_ipAddressControl.Location = new System.Drawing.Point(124, 85);
            this.accesscontrol_ipAddressControl.MaxLength = 9;
            this.accesscontrol_ipAddressControl.Name = "accesscontrol_ipAddressControl";
            this.accesscontrol_ipAddressControl.Size = new System.Drawing.Size(100, 20);
            this.accesscontrol_ipAddressControl.TabIndex = 118;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(81, 124);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(33, 13);
            this.label13.TabIndex = 117;
            this.label13.Text = "Mask";
            // 
            // snmpv1v2custom_checkBox
            // 
            this.snmpv1v2custom_checkBox.AutoSize = true;
            this.snmpv1v2custom_checkBox.Checked = true;
            this.snmpv1v2custom_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.snmpv1v2custom_checkBox.Location = new System.Drawing.Point(126, 50);
            this.snmpv1v2custom_checkBox.Name = "snmpv1v2custom_checkBox";
            this.snmpv1v2custom_checkBox.Size = new System.Drawing.Size(15, 14);
            this.snmpv1v2custom_checkBox.TabIndex = 115;
            this.snmpv1v2custom_checkBox.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(45, 50);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(69, 13);
            this.label17.TabIndex = 101;
            this.label17.Text = "SNMPV1/V2";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(14, 15);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(100, 13);
            this.label15.TabIndex = 99;
            this.label15.Text = "Encryption Strength";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(11, 88);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(103, 13);
            this.label21.TabIndex = 103;
            this.label21.Text = "Access Control IPv4";
            // 
            // snmpreadonly_checkBox
            // 
            this.snmpreadonly_checkBox.AutoSize = true;
            this.snmpreadonly_checkBox.Checked = true;
            this.snmpreadonly_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.snmpreadonly_checkBox.Location = new System.Drawing.Point(209, 157);
            this.snmpreadonly_checkBox.Name = "snmpreadonly_checkBox";
            this.snmpreadonly_checkBox.Size = new System.Drawing.Size(15, 14);
            this.snmpreadonly_checkBox.TabIndex = 97;
            this.snmpreadonly_checkBox.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(129, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 84;
            this.label5.Text = "SNMPV3";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(160, 13);
            this.label6.TabIndex = 85;
            this.label6.Text = "SNMPV1/V2 Read Only Access";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(76, 52);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 13);
            this.label8.TabIndex = 87;
            this.label8.Text = "SNMPV3 UserName";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 87);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(175, 13);
            this.label9.TabIndex = 89;
            this.label9.Text = "Authentication Protocol Passphrase";
            // 
            // enhancedSecurity_radioButton
            // 
            this.enhancedSecurity_radioButton.AutoSize = true;
            this.enhancedSecurity_radioButton.Location = new System.Drawing.Point(25, 234);
            this.enhancedSecurity_radioButton.Name = "enhancedSecurity_radioButton";
            this.enhancedSecurity_radioButton.Size = new System.Drawing.Size(156, 17);
            this.enhancedSecurity_radioButton.TabIndex = 128;
            this.enhancedSecurity_radioButton.Text = "Enhanced Security Settings";
            this.enhancedSecurity_radioButton.UseVisualStyleBackColor = true;
            // 
            // basicSecurity_radioButton
            // 
            this.basicSecurity_radioButton.AutoSize = true;
            this.basicSecurity_radioButton.Checked = true;
            this.basicSecurity_radioButton.Location = new System.Drawing.Point(25, 200);
            this.basicSecurity_radioButton.Name = "basicSecurity_radioButton";
            this.basicSecurity_radioButton.Size = new System.Drawing.Size(133, 17);
            this.basicSecurity_radioButton.TabIndex = 127;
            this.basicSecurity_radioButton.TabStop = true;
            this.basicSecurity_radioButton.Text = "Basic Security Settings";
            this.basicSecurity_radioButton.UseVisualStyleBackColor = true;
            // 
            // customSecurity_radioButton
            // 
            this.customSecurity_radioButton.AutoSize = true;
            this.customSecurity_radioButton.Location = new System.Drawing.Point(355, 200);
            this.customSecurity_radioButton.Name = "customSecurity_radioButton";
            this.customSecurity_radioButton.Size = new System.Drawing.Size(142, 17);
            this.customSecurity_radioButton.TabIndex = 130;
            this.customSecurity_radioButton.Text = "Custom Security Settings";
            this.customSecurity_radioButton.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(38, 122);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(142, 13);
            this.label14.TabIndex = 91;
            this.label14.Text = "Privacy Protocol Passphrase";
            // 
            // authProtocolPassphrase_textBox
            // 
            this.authProtocolPassphrase_textBox.Location = new System.Drawing.Point(209, 87);
            this.authProtocolPassphrase_textBox.MaxLength = 9;
            this.authProtocolPassphrase_textBox.Name = "authProtocolPassphrase_textBox";
            this.authProtocolPassphrase_textBox.Size = new System.Drawing.Size(100, 20);
            this.authProtocolPassphrase_textBox.TabIndex = 90;
            // 
            // snmpv3UserName_textBox
            // 
            this.snmpv3UserName_textBox.Location = new System.Drawing.Point(209, 52);
            this.snmpv3UserName_textBox.Name = "snmpv3UserName_textBox";
            this.snmpv3UserName_textBox.Size = new System.Drawing.Size(100, 20);
            this.snmpv3UserName_textBox.TabIndex = 88;
            // 
            // enhanced_panel
            // 
            this.enhanced_panel.Controls.Add(this.snmpv3_checkBox);
            this.enhanced_panel.Controls.Add(this.snmpreadonly_checkBox);
            this.enhanced_panel.Controls.Add(this.label5);
            this.enhanced_panel.Controls.Add(this.label6);
            this.enhanced_panel.Controls.Add(this.label8);
            this.enhanced_panel.Controls.Add(this.label9);
            this.enhanced_panel.Controls.Add(this.label14);
            this.enhanced_panel.Controls.Add(this.privacyProtocolPassphrase_textBox);
            this.enhanced_panel.Controls.Add(this.authProtocolPassphrase_textBox);
            this.enhanced_panel.Controls.Add(this.snmpv3UserName_textBox);
            this.enhanced_panel.Location = new System.Drawing.Point(25, 257);
            this.enhanced_panel.Name = "enhanced_panel";
            this.enhanced_panel.Size = new System.Drawing.Size(321, 181);
            this.enhanced_panel.TabIndex = 132;
            // 
            // privacyProtocolPassphrase_textBox
            // 
            this.privacyProtocolPassphrase_textBox.Location = new System.Drawing.Point(209, 122);
            this.privacyProtocolPassphrase_textBox.MaxLength = 9;
            this.privacyProtocolPassphrase_textBox.Name = "privacyProtocolPassphrase_textBox";
            this.privacyProtocolPassphrase_textBox.Size = new System.Drawing.Size(100, 20);
            this.privacyProtocolPassphrase_textBox.TabIndex = 92;
            // 
            // SecurityConfigurationUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label12);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.custom_panel);
            this.Controls.Add(this.enhancedSecurity_radioButton);
            this.Controls.Add(this.basicSecurity_radioButton);
            this.Controls.Add(this.customSecurity_radioButton);
            this.Controls.Add(this.enhanced_panel);
            this.Controls.Add(this.Comments);
            this.Controls.Add(this.label1);
            this.Name = "SecurityConfigurationUserControl";
            this.Size = new System.Drawing.Size(660, 496);
            this.dot1x_groupBox.ResumeLayout(false);
            this.dot1x_groupBox.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.custom_panel.ResumeLayout(false);
            this.custom_panel.PerformLayout();
            this.enhanced_panel.ResumeLayout(false);
            this.enhanced_panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HP.ScalableTest.Framework.UI.AssetSelectionControl security_assetSelectionControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Comments;
        private System.Windows.Forms.GroupBox dot1x_groupBox;
        private System.Windows.Forms.TextBox authenticationPassword_textBox;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox snmpv3_checkBox;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox encryptionStrength_comboBox;
        private System.Windows.Forms.Panel custom_panel;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox snmpv1v2custom_checkBox;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.CheckBox snmpreadonly_checkBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton enhancedSecurity_radioButton;
        private System.Windows.Forms.RadioButton basicSecurity_radioButton;
        private System.Windows.Forms.RadioButton customSecurity_radioButton;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox authProtocolPassphrase_textBox;
        private System.Windows.Forms.TextBox snmpv3UserName_textBox;
        private System.Windows.Forms.Panel enhanced_panel;
        private System.Windows.Forms.TextBox privacyProtocolPassphrase_textBox;
        private System.Windows.Forms.TextBox mask_ipAddressControl;
        private System.Windows.Forms.TextBox accesscontrol_ipAddressControl;
        private Framework.UI.FieldValidator security_fieldValidator;
    }
}