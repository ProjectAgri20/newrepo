namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    partial class IRMUserControl
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
            this.irmTabControl = new System.Windows.Forms.TabControl();
            this.irmAD_TabPage = new System.Windows.Forms.TabPage();
            this.dataStorage_GroupBox = new System.Windows.Forms.GroupBox();
            this.database_RadioButton = new System.Windows.Forms.RadioButton();
            this.ldapServer_RadioButton = new System.Windows.Forms.RadioButton();
            this.cardsCodes_RadioButton = new System.Windows.Forms.RadioButton();
            this.ldap_RadioButton = new System.Windows.Forms.RadioButton();
            this.general_RadioButton = new System.Windows.Forms.RadioButton();
            this.deviceAuthenticationMethod_GroupBox = new System.Windows.Forms.GroupBox();
            this.cardAndCode_RadioButton = new System.Windows.Forms.RadioButton();
            this.codeOnly_RadioButton = new System.Windows.Forms.RadioButton();
            this.cardOrCode_RadioButton = new System.Windows.Forms.RadioButton();
            this.cardOnly_RadioButton = new System.Windows.Forms.RadioButton();
            this.ldapServerPassword_TextBox = new System.Windows.Forms.TextBox();
            this.ldapServerPassword_Label = new System.Windows.Forms.Label();
            this.ldapServerUsername_TextBox = new System.Windows.Forms.TextBox();
            this.ldapServerUsername_Label = new System.Windows.Forms.Label();
            this.ldapServer_textBox = new System.Windows.Forms.TextBox();
            this.ldapServer_Label = new System.Windows.Forms.Label();
            this.card_TextBox = new System.Windows.Forms.TextBox();
            this.cardAttribute_Label = new System.Windows.Forms.Label();
            this.code_TextBox = new System.Windows.Forms.TextBox();
            this.codeAttribute_Label = new System.Windows.Forms.Label();
            this.display_Label = new System.Windows.Forms.Label();
            this.adUserEditTabPage = new System.Windows.Forms.TabPage();
            this.codeNumber_TextBox = new System.Windows.Forms.TextBox();
            this.codeNumber_Label = new System.Windows.Forms.Label();
            this.username_Label = new System.Windows.Forms.Label();
            this.cardNumber_TextBox = new System.Windows.Forms.TextBox();
            this.username_TextBox = new System.Windows.Forms.TextBox();
            this.cardNumber_Label = new System.Windows.Forms.Label();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.irmTabControl.SuspendLayout();
            this.irmAD_TabPage.SuspendLayout();
            this.dataStorage_GroupBox.SuspendLayout();
            this.deviceAuthenticationMethod_GroupBox.SuspendLayout();
            this.adUserEditTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // irmTabControl
            // 
            this.irmTabControl.Controls.Add(this.irmAD_TabPage);
            this.irmTabControl.Controls.Add(this.adUserEditTabPage);
            this.irmTabControl.Location = new System.Drawing.Point(3, 3);
            this.irmTabControl.Name = "irmTabControl";
            this.irmTabControl.SelectedIndex = 0;
            this.irmTabControl.Size = new System.Drawing.Size(654, 348);
            this.irmTabControl.TabIndex = 0;
            this.irmTabControl.SelectedIndexChanged += new System.EventHandler(this.irmTabControl_SelectedIndexChanged);
            // 
            // irmAD_TabPage
            // 
            this.irmAD_TabPage.Controls.Add(this.dataStorage_GroupBox);
            this.irmAD_TabPage.Controls.Add(this.cardsCodes_RadioButton);
            this.irmAD_TabPage.Controls.Add(this.ldap_RadioButton);
            this.irmAD_TabPage.Controls.Add(this.general_RadioButton);
            this.irmAD_TabPage.Controls.Add(this.deviceAuthenticationMethod_GroupBox);
            this.irmAD_TabPage.Controls.Add(this.ldapServerPassword_TextBox);
            this.irmAD_TabPage.Controls.Add(this.ldapServerPassword_Label);
            this.irmAD_TabPage.Controls.Add(this.ldapServerUsername_TextBox);
            this.irmAD_TabPage.Controls.Add(this.ldapServerUsername_Label);
            this.irmAD_TabPage.Controls.Add(this.ldapServer_textBox);
            this.irmAD_TabPage.Controls.Add(this.ldapServer_Label);
            this.irmAD_TabPage.Controls.Add(this.card_TextBox);
            this.irmAD_TabPage.Controls.Add(this.cardAttribute_Label);
            this.irmAD_TabPage.Controls.Add(this.code_TextBox);
            this.irmAD_TabPage.Controls.Add(this.codeAttribute_Label);
            this.irmAD_TabPage.Controls.Add(this.display_Label);
            this.irmAD_TabPage.Location = new System.Drawing.Point(4, 22);
            this.irmAD_TabPage.Name = "irmAD_TabPage";
            this.irmAD_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.irmAD_TabPage.Size = new System.Drawing.Size(646, 322);
            this.irmAD_TabPage.TabIndex = 0;
            this.irmAD_TabPage.Text = "IRM AD";
            this.irmAD_TabPage.UseVisualStyleBackColor = true;
            // 
            // dataStorage_GroupBox
            // 
            this.dataStorage_GroupBox.Controls.Add(this.database_RadioButton);
            this.dataStorage_GroupBox.Controls.Add(this.ldapServer_RadioButton);
            this.dataStorage_GroupBox.Location = new System.Drawing.Point(255, 162);
            this.dataStorage_GroupBox.Name = "dataStorage_GroupBox";
            this.dataStorage_GroupBox.Size = new System.Drawing.Size(190, 87);
            this.dataStorage_GroupBox.TabIndex = 31;
            this.dataStorage_GroupBox.TabStop = false;
            this.dataStorage_GroupBox.Text = "Data Storage";
            // 
            // database_RadioButton
            // 
            this.database_RadioButton.AutoSize = true;
            this.database_RadioButton.Location = new System.Drawing.Point(10, 48);
            this.database_RadioButton.Name = "database_RadioButton";
            this.database_RadioButton.Size = new System.Drawing.Size(71, 17);
            this.database_RadioButton.TabIndex = 4;
            this.database_RadioButton.TabStop = true;
            this.database_RadioButton.Text = "Database";
            this.database_RadioButton.UseVisualStyleBackColor = true;
            // 
            // ldapServer_RadioButton
            // 
            this.ldapServer_RadioButton.AutoSize = true;
            this.ldapServer_RadioButton.Checked = true;
            this.ldapServer_RadioButton.Location = new System.Drawing.Point(10, 25);
            this.ldapServer_RadioButton.Name = "ldapServer_RadioButton";
            this.ldapServer_RadioButton.Size = new System.Drawing.Size(87, 17);
            this.ldapServer_RadioButton.TabIndex = 3;
            this.ldapServer_RadioButton.TabStop = true;
            this.ldapServer_RadioButton.Text = "LDAP Server";
            this.ldapServer_RadioButton.UseVisualStyleBackColor = true;
            // 
            // cardsCodes_RadioButton
            // 
            this.cardsCodes_RadioButton.AutoSize = true;
            this.cardsCodes_RadioButton.Location = new System.Drawing.Point(6, 97);
            this.cardsCodes_RadioButton.Name = "cardsCodes_RadioButton";
            this.cardsCodes_RadioButton.Size = new System.Drawing.Size(87, 17);
            this.cardsCodes_RadioButton.TabIndex = 35;
            this.cardsCodes_RadioButton.TabStop = true;
            this.cardsCodes_RadioButton.Text = "Cards\\Codes";
            this.cardsCodes_RadioButton.UseVisualStyleBackColor = true;
            this.cardsCodes_RadioButton.CheckedChanged += new System.EventHandler(this.irmTabs_CheckedChanged);
            // 
            // ldap_RadioButton
            // 
            this.ldap_RadioButton.AutoSize = true;
            this.ldap_RadioButton.Location = new System.Drawing.Point(6, 67);
            this.ldap_RadioButton.Name = "ldap_RadioButton";
            this.ldap_RadioButton.Size = new System.Drawing.Size(87, 17);
            this.ldap_RadioButton.TabIndex = 34;
            this.ldap_RadioButton.TabStop = true;
            this.ldap_RadioButton.Text = "LDAP Server";
            this.ldap_RadioButton.UseVisualStyleBackColor = true;
            this.ldap_RadioButton.CheckedChanged += new System.EventHandler(this.irmTabs_CheckedChanged);
            // 
            // general_RadioButton
            // 
            this.general_RadioButton.AutoSize = true;
            this.general_RadioButton.Checked = true;
            this.general_RadioButton.Location = new System.Drawing.Point(6, 37);
            this.general_RadioButton.Name = "general_RadioButton";
            this.general_RadioButton.Size = new System.Drawing.Size(62, 17);
            this.general_RadioButton.TabIndex = 33;
            this.general_RadioButton.TabStop = true;
            this.general_RadioButton.Text = "General";
            this.general_RadioButton.UseVisualStyleBackColor = true;
            this.general_RadioButton.CheckedChanged += new System.EventHandler(this.irmTabs_CheckedChanged);
            // 
            // deviceAuthenticationMethod_GroupBox
            // 
            this.deviceAuthenticationMethod_GroupBox.Controls.Add(this.cardAndCode_RadioButton);
            this.deviceAuthenticationMethod_GroupBox.Controls.Add(this.codeOnly_RadioButton);
            this.deviceAuthenticationMethod_GroupBox.Controls.Add(this.cardOrCode_RadioButton);
            this.deviceAuthenticationMethod_GroupBox.Controls.Add(this.cardOnly_RadioButton);
            this.deviceAuthenticationMethod_GroupBox.Location = new System.Drawing.Point(255, 37);
            this.deviceAuthenticationMethod_GroupBox.Name = "deviceAuthenticationMethod_GroupBox";
            this.deviceAuthenticationMethod_GroupBox.Size = new System.Drawing.Size(190, 119);
            this.deviceAuthenticationMethod_GroupBox.TabIndex = 30;
            this.deviceAuthenticationMethod_GroupBox.TabStop = false;
            this.deviceAuthenticationMethod_GroupBox.Text = "Device Authentication Method";
            // 
            // cardAndCode_RadioButton
            // 
            this.cardAndCode_RadioButton.AutoSize = true;
            this.cardAndCode_RadioButton.Location = new System.Drawing.Point(9, 94);
            this.cardAndCode_RadioButton.Name = "cardAndCode_RadioButton";
            this.cardAndCode_RadioButton.Size = new System.Drawing.Size(150, 17);
            this.cardAndCode_RadioButton.TabIndex = 5;
            this.cardAndCode_RadioButton.TabStop = true;
            this.cardAndCode_RadioButton.Text = "Card And Code(two-factor)";
            this.cardAndCode_RadioButton.UseVisualStyleBackColor = true;
            // 
            // codeOnly_RadioButton
            // 
            this.codeOnly_RadioButton.AutoSize = true;
            this.codeOnly_RadioButton.Location = new System.Drawing.Point(9, 46);
            this.codeOnly_RadioButton.Name = "codeOnly_RadioButton";
            this.codeOnly_RadioButton.Size = new System.Drawing.Size(74, 17);
            this.codeOnly_RadioButton.TabIndex = 4;
            this.codeOnly_RadioButton.TabStop = true;
            this.codeOnly_RadioButton.Text = "Code Only";
            this.codeOnly_RadioButton.UseVisualStyleBackColor = true;
            // 
            // cardOrCode_RadioButton
            // 
            this.cardOrCode_RadioButton.AutoSize = true;
            this.cardOrCode_RadioButton.Location = new System.Drawing.Point(9, 70);
            this.cardOrCode_RadioButton.Name = "cardOrCode_RadioButton";
            this.cardOrCode_RadioButton.Size = new System.Drawing.Size(87, 17);
            this.cardOrCode_RadioButton.TabIndex = 3;
            this.cardOrCode_RadioButton.TabStop = true;
            this.cardOrCode_RadioButton.Text = "Card or Code";
            this.cardOrCode_RadioButton.UseVisualStyleBackColor = true;
            // 
            // cardOnly_RadioButton
            // 
            this.cardOnly_RadioButton.AutoSize = true;
            this.cardOnly_RadioButton.Checked = true;
            this.cardOnly_RadioButton.Location = new System.Drawing.Point(9, 22);
            this.cardOnly_RadioButton.Name = "cardOnly_RadioButton";
            this.cardOnly_RadioButton.Size = new System.Drawing.Size(71, 17);
            this.cardOnly_RadioButton.TabIndex = 2;
            this.cardOnly_RadioButton.TabStop = true;
            this.cardOnly_RadioButton.Text = "Card Only";
            this.cardOnly_RadioButton.UseVisualStyleBackColor = true;
            // 
            // ldapServerPassword_TextBox
            // 
            this.ldapServerPassword_TextBox.Location = new System.Drawing.Point(316, 124);
            this.ldapServerPassword_TextBox.Name = "ldapServerPassword_TextBox";
            this.ldapServerPassword_TextBox.Size = new System.Drawing.Size(200, 20);
            this.ldapServerPassword_TextBox.TabIndex = 29;
            this.ldapServerPassword_TextBox.Visible = false;
            // 
            // ldapServerPassword_Label
            // 
            this.ldapServerPassword_Label.AutoSize = true;
            this.ldapServerPassword_Label.Location = new System.Drawing.Point(182, 124);
            this.ldapServerPassword_Label.Name = "ldapServerPassword_Label";
            this.ldapServerPassword_Label.Size = new System.Drawing.Size(116, 13);
            this.ldapServerPassword_Label.TabIndex = 28;
            this.ldapServerPassword_Label.Text = "LDAP server Password";
            this.ldapServerPassword_Label.Visible = false;
            // 
            // ldapServerUsername_TextBox
            // 
            this.ldapServerUsername_TextBox.Location = new System.Drawing.Point(316, 82);
            this.ldapServerUsername_TextBox.Name = "ldapServerUsername_TextBox";
            this.ldapServerUsername_TextBox.Size = new System.Drawing.Size(200, 20);
            this.ldapServerUsername_TextBox.TabIndex = 27;
            this.ldapServerUsername_TextBox.Visible = false;
            // 
            // ldapServerUsername_Label
            // 
            this.ldapServerUsername_Label.AutoSize = true;
            this.ldapServerUsername_Label.Location = new System.Drawing.Point(182, 82);
            this.ldapServerUsername_Label.Name = "ldapServerUsername_Label";
            this.ldapServerUsername_Label.Size = new System.Drawing.Size(116, 13);
            this.ldapServerUsername_Label.TabIndex = 26;
            this.ldapServerUsername_Label.Text = "LDAP server username";
            this.ldapServerUsername_Label.Visible = false;
            // 
            // ldapServer_textBox
            // 
            this.ldapServer_textBox.Location = new System.Drawing.Point(316, 40);
            this.ldapServer_textBox.Name = "ldapServer_textBox";
            this.ldapServer_textBox.Size = new System.Drawing.Size(200, 20);
            this.ldapServer_textBox.TabIndex = 25;
            this.ldapServer_textBox.Visible = false;
            // 
            // ldapServer_Label
            // 
            this.ldapServer_Label.AutoSize = true;
            this.ldapServer_Label.Location = new System.Drawing.Point(182, 41);
            this.ldapServer_Label.Name = "ldapServer_Label";
            this.ldapServer_Label.Size = new System.Drawing.Size(67, 13);
            this.ldapServer_Label.TabIndex = 24;
            this.ldapServer_Label.Text = "LDAP server";
            this.ldapServer_Label.Visible = false;
            // 
            // card_TextBox
            // 
            this.card_TextBox.Location = new System.Drawing.Point(272, 82);
            this.card_TextBox.Name = "card_TextBox";
            this.card_TextBox.Size = new System.Drawing.Size(200, 20);
            this.card_TextBox.TabIndex = 23;
            this.card_TextBox.Visible = false;
            // 
            // cardAttribute_Label
            // 
            this.cardAttribute_Label.AutoSize = true;
            this.cardAttribute_Label.Location = new System.Drawing.Point(182, 82);
            this.cardAttribute_Label.Name = "cardAttribute_Label";
            this.cardAttribute_Label.Size = new System.Drawing.Size(71, 13);
            this.cardAttribute_Label.TabIndex = 20;
            this.cardAttribute_Label.Text = "Card Attribute";
            this.cardAttribute_Label.Visible = false;
            // 
            // code_TextBox
            // 
            this.code_TextBox.Location = new System.Drawing.Point(272, 40);
            this.code_TextBox.Name = "code_TextBox";
            this.code_TextBox.Size = new System.Drawing.Size(200, 20);
            this.code_TextBox.TabIndex = 21;
            this.code_TextBox.Visible = false;
            // 
            // codeAttribute_Label
            // 
            this.codeAttribute_Label.AutoSize = true;
            this.codeAttribute_Label.Location = new System.Drawing.Point(182, 43);
            this.codeAttribute_Label.Name = "codeAttribute_Label";
            this.codeAttribute_Label.Size = new System.Drawing.Size(74, 13);
            this.codeAttribute_Label.TabIndex = 22;
            this.codeAttribute_Label.Text = "Code Attribute";
            this.codeAttribute_Label.Visible = false;
            // 
            // display_Label
            // 
            this.display_Label.AutoSize = true;
            this.display_Label.Location = new System.Drawing.Point(182, 69);
            this.display_Label.Name = "display_Label";
            this.display_Label.Size = new System.Drawing.Size(381, 13);
            this.display_Label.TabIndex = 19;
            this.display_Label.Text = "The database storgae type in general tab doesnt support code or card attribute.";
            this.display_Label.Visible = false;
            // 
            // adUserEditTabPage
            // 
            this.adUserEditTabPage.Controls.Add(this.codeNumber_TextBox);
            this.adUserEditTabPage.Controls.Add(this.codeNumber_Label);
            this.adUserEditTabPage.Controls.Add(this.username_Label);
            this.adUserEditTabPage.Controls.Add(this.cardNumber_TextBox);
            this.adUserEditTabPage.Controls.Add(this.username_TextBox);
            this.adUserEditTabPage.Controls.Add(this.cardNumber_Label);
            this.adUserEditTabPage.Location = new System.Drawing.Point(4, 22);
            this.adUserEditTabPage.Name = "adUserEditTabPage";
            this.adUserEditTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.adUserEditTabPage.Size = new System.Drawing.Size(646, 322);
            this.adUserEditTabPage.TabIndex = 1;
            this.adUserEditTabPage.Text = "AD User";
            this.adUserEditTabPage.UseVisualStyleBackColor = true;
            // 
            // codeNumber_TextBox
            // 
            this.codeNumber_TextBox.Location = new System.Drawing.Point(19, 169);
            this.codeNumber_TextBox.Name = "codeNumber_TextBox";
            this.codeNumber_TextBox.Size = new System.Drawing.Size(177, 20);
            this.codeNumber_TextBox.TabIndex = 16;
            // 
            // codeNumber_Label
            // 
            this.codeNumber_Label.AutoSize = true;
            this.codeNumber_Label.Location = new System.Drawing.Point(16, 153);
            this.codeNumber_Label.Name = "codeNumber_Label";
            this.codeNumber_Label.Size = new System.Drawing.Size(97, 13);
            this.codeNumber_Label.TabIndex = 15;
            this.codeNumber_Label.Text = "New Code Number";
            // 
            // username_Label
            // 
            this.username_Label.AutoSize = true;
            this.username_Label.Location = new System.Drawing.Point(16, 25);
            this.username_Label.Name = "username_Label";
            this.username_Label.Size = new System.Drawing.Size(55, 13);
            this.username_Label.TabIndex = 11;
            this.username_Label.Text = "Username";
            // 
            // cardNumber_TextBox
            // 
            this.cardNumber_TextBox.Location = new System.Drawing.Point(19, 104);
            this.cardNumber_TextBox.Name = "cardNumber_TextBox";
            this.cardNumber_TextBox.Size = new System.Drawing.Size(178, 20);
            this.cardNumber_TextBox.TabIndex = 14;
            // 
            // username_TextBox
            // 
            this.username_TextBox.Location = new System.Drawing.Point(19, 41);
            this.username_TextBox.Name = "username_TextBox";
            this.username_TextBox.Size = new System.Drawing.Size(177, 20);
            this.username_TextBox.TabIndex = 12;
            // 
            // cardNumber_Label
            // 
            this.cardNumber_Label.AutoSize = true;
            this.cardNumber_Label.Location = new System.Drawing.Point(16, 88);
            this.cardNumber_Label.Name = "cardNumber_Label";
            this.cardNumber_Label.Size = new System.Drawing.Size(94, 13);
            this.cardNumber_Label.TabIndex = 13;
            this.cardNumber_Label.Text = "New Card Number";
            // 
            // IRMUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.irmTabControl);
            this.Name = "IRMUserControl";
            this.Size = new System.Drawing.Size(676, 428);
            this.irmTabControl.ResumeLayout(false);
            this.irmAD_TabPage.ResumeLayout(false);
            this.irmAD_TabPage.PerformLayout();
            this.dataStorage_GroupBox.ResumeLayout(false);
            this.dataStorage_GroupBox.PerformLayout();
            this.deviceAuthenticationMethod_GroupBox.ResumeLayout(false);
            this.deviceAuthenticationMethod_GroupBox.PerformLayout();
            this.adUserEditTabPage.ResumeLayout(false);
            this.adUserEditTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl irmTabControl;
        private System.Windows.Forms.TabPage irmAD_TabPage;
        private System.Windows.Forms.TabPage adUserEditTabPage;
        private System.Windows.Forms.TextBox codeNumber_TextBox;
        private System.Windows.Forms.Label codeNumber_Label;
        private System.Windows.Forms.Label username_Label;
        private System.Windows.Forms.TextBox cardNumber_TextBox;
        private System.Windows.Forms.TextBox username_TextBox;
        private System.Windows.Forms.Label cardNumber_Label;
        private System.Windows.Forms.Label display_Label;
        private System.Windows.Forms.TextBox card_TextBox;
        private System.Windows.Forms.Label cardAttribute_Label;
        private System.Windows.Forms.TextBox code_TextBox;
        private System.Windows.Forms.Label codeAttribute_Label;
        private System.Windows.Forms.Label ldapServer_Label;
        private System.Windows.Forms.TextBox ldapServer_textBox;
        private System.Windows.Forms.Label ldapServerUsername_Label;
        private System.Windows.Forms.TextBox ldapServerUsername_TextBox;
        private System.Windows.Forms.Label ldapServerPassword_Label;
        private System.Windows.Forms.TextBox ldapServerPassword_TextBox;
        private System.Windows.Forms.GroupBox deviceAuthenticationMethod_GroupBox;
        private System.Windows.Forms.RadioButton cardAndCode_RadioButton;
        private System.Windows.Forms.RadioButton codeOnly_RadioButton;
        private System.Windows.Forms.RadioButton cardOrCode_RadioButton;
        private System.Windows.Forms.RadioButton cardOnly_RadioButton;
        private System.Windows.Forms.GroupBox dataStorage_GroupBox;
        private System.Windows.Forms.RadioButton database_RadioButton;
        private System.Windows.Forms.RadioButton ldapServer_RadioButton;
        private System.Windows.Forms.RadioButton cardsCodes_RadioButton;
        private System.Windows.Forms.RadioButton ldap_RadioButton;
        private System.Windows.Forms.RadioButton general_RadioButton;
        private Framework.UI.FieldValidator fieldValidator;
    }
}
