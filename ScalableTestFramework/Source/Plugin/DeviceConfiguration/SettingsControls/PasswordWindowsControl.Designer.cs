namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    partial class PasswordWindowsControl
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
            this.passwordComplexity_CheckBox = new System.Windows.Forms.CheckBox();
            this.defaultWindows_Label = new System.Windows.Forms.Label();
            this.windowsAuth_CheckBox = new System.Windows.Forms.CheckBox();
            this.passwordLength_Label = new System.Windows.Forms.Label();
            this.windowsDomain_Label = new System.Windows.Forms.Label();
            this.defaultDomain_TextBox = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceTextControl();
            this.winDomains_TextBox = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceTextControl();
            this.maxPasswordLength_Textbox = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceTextControl();
            this.SuspendLayout();
            // 
            // passwordComplexity_CheckBox
            // 
            this.passwordComplexity_CheckBox.AutoSize = true;
            this.passwordComplexity_CheckBox.Location = new System.Drawing.Point(14, 15);
            this.passwordComplexity_CheckBox.Name = "passwordComplexity_CheckBox";
            this.passwordComplexity_CheckBox.Size = new System.Drawing.Size(161, 17);
            this.passwordComplexity_CheckBox.TabIndex = 36;
            this.passwordComplexity_CheckBox.Text = "Enable Password Complexity";
            this.passwordComplexity_CheckBox.UseVisualStyleBackColor = true;
            // 
            // defaultWindows_Label
            // 
            this.defaultWindows_Label.AutoSize = true;
            this.defaultWindows_Label.Location = new System.Drawing.Point(11, 93);
            this.defaultWindows_Label.Name = "defaultWindows_Label";
            this.defaultWindows_Label.Size = new System.Drawing.Size(215, 13);
            this.defaultWindows_Label.TabIndex = 34;
            this.defaultWindows_Label.Text = "Default Windows Domain (Required above):";
            // 
            // windowsAuth_CheckBox
            // 
            this.windowsAuth_CheckBox.AutoSize = true;
            this.windowsAuth_CheckBox.Location = new System.Drawing.Point(14, 125);
            this.windowsAuth_CheckBox.Name = "windowsAuth_CheckBox";
            this.windowsAuth_CheckBox.Size = new System.Drawing.Size(177, 17);
            this.windowsAuth_CheckBox.TabIndex = 37;
            this.windowsAuth_CheckBox.Text = "Enable Windows Authentication";
            this.windowsAuth_CheckBox.UseVisualStyleBackColor = true;
            // 
            // passwordLength_Label
            // 
            this.passwordLength_Label.AutoSize = true;
            this.passwordLength_Label.Location = new System.Drawing.Point(11, 37);
            this.passwordLength_Label.Name = "passwordLength_Label";
            this.passwordLength_Label.Size = new System.Drawing.Size(112, 13);
            this.passwordLength_Label.TabIndex = 31;
            this.passwordLength_Label.Text = "Min Password Length:";
            // 
            // windowsDomain_Label
            // 
            this.windowsDomain_Label.AutoSize = true;
            this.windowsDomain_Label.Location = new System.Drawing.Point(11, 63);
            this.windowsDomain_Label.Name = "windowsDomain_Label";
            this.windowsDomain_Label.Size = new System.Drawing.Size(209, 13);
            this.windowsDomain_Label.TabIndex = 30;
            this.windowsDomain_Label.Text = "Windows Domains (SemiColon Separated):";
            // 
            // defaultDomain_TextBox
            // 
            this.defaultDomain_TextBox.Location = new System.Drawing.Point(237, 87);
            this.defaultDomain_TextBox.Name = "defaultDomain_TextBox";
            this.defaultDomain_TextBox.Size = new System.Drawing.Size(361, 26);
            this.defaultDomain_TextBox.TabIndex = 40;
            // 
            // winDomains_TextBox
            // 
            this.winDomains_TextBox.Location = new System.Drawing.Point(237, 58);
            this.winDomains_TextBox.Name = "winDomains_TextBox";
            this.winDomains_TextBox.Size = new System.Drawing.Size(361, 26);
            this.winDomains_TextBox.TabIndex = 39;
            // 
            // maxPasswordLength_Textbox
            // 
            this.maxPasswordLength_Textbox.Location = new System.Drawing.Point(237, 31);
            this.maxPasswordLength_Textbox.Name = "maxPasswordLength_Textbox";
            this.maxPasswordLength_Textbox.Size = new System.Drawing.Size(361, 26);
            this.maxPasswordLength_Textbox.TabIndex = 38;
            // 
            // PasswordWindowsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.defaultDomain_TextBox);
            this.Controls.Add(this.winDomains_TextBox);
            this.Controls.Add(this.maxPasswordLength_Textbox);
            this.Controls.Add(this.passwordComplexity_CheckBox);
            this.Controls.Add(this.defaultWindows_Label);
            this.Controls.Add(this.windowsAuth_CheckBox);
            this.Controls.Add(this.passwordLength_Label);
            this.Controls.Add(this.windowsDomain_Label);
            this.Name = "PasswordWindowsControl";
            this.Size = new System.Drawing.Size(705, 177);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox passwordComplexity_CheckBox;
        private System.Windows.Forms.Label defaultWindows_Label;
        private System.Windows.Forms.CheckBox windowsAuth_CheckBox;
        private System.Windows.Forms.Label passwordLength_Label;
        private System.Windows.Forms.Label windowsDomain_Label;
        private ChoiceTextControl maxPasswordLength_Textbox;
        private ChoiceTextControl winDomains_TextBox;
        private ChoiceTextControl defaultDomain_TextBox;
    }
}
