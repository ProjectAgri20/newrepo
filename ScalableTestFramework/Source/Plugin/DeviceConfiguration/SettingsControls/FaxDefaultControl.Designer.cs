namespace HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls
{
    partial class FaxDefaultControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.resolution_label = new System.Windows.Forms.Label();
            this.folder_Label = new System.Windows.Forms.Label();
            this.FaxSettings_Label = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.field4_TextBox = new System.Windows.Forms.MaskedTextBox();
            this.field1_TextBox = new System.Windows.Forms.TextBox();
            this.field3_TextBox = new System.Windows.Forms.MaskedTextBox();
            this.field2_TextBox = new System.Windows.Forms.TextBox();
            this.modem_groupBox = new System.Windows.Forms.GroupBox();
            this.location_label = new System.Windows.Forms.Label();
            this.company_label = new System.Windows.Forms.Label();
            this.number_label = new System.Windows.Forms.Label();
            this.faxNumber_choiceTextControl = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceTextControl();
            this.companyName_choiceTextControl = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceTextControl();
            this.location_choiceTextControl = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceTextControl();
            this.faxMethod_ComboBox = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.fileFormat_ChoiceControl = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.thirdParty_ChoiceControl = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.scanSettingsUserControl = new HP.ScalableTest.Plugin.DeviceConfiguration.SettingsControls.ScanSettingsUserControl();
            this.resolution_choiceControl = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.enable_ComboBox = new HP.ScalableTest.Plugin.DeviceConfiguration.ChoiceComboControl();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.modem_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.scanSettingsUserControl);
            this.groupBox1.Controls.Add(this.resolution_choiceControl);
            this.groupBox1.Controls.Add(this.resolution_label);
            this.groupBox1.Location = new System.Drawing.Point(15, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(350, 471);
            this.groupBox1.TabIndex = 57;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scan Settings";
            // 
            // resolution_label
            // 
            this.resolution_label.AutoSize = true;
            this.resolution_label.Location = new System.Drawing.Point(6, 416);
            this.resolution_label.Name = "resolution_label";
            this.resolution_label.Size = new System.Drawing.Size(57, 13);
            this.resolution_label.TabIndex = 46;
            this.resolution_label.Text = "Resolution";
            // 
            // folder_Label
            // 
            this.folder_Label.AutoSize = true;
            this.folder_Label.Location = new System.Drawing.Point(371, 34);
            this.folder_Label.Name = "folder_Label";
            this.folder_Label.Size = new System.Drawing.Size(91, 13);
            this.folder_Label.TabIndex = 54;
            this.folder_Label.Text = "Enable Fax Send:";
            // 
            // FaxSettings_Label
            // 
            this.FaxSettings_Label.AutoSize = true;
            this.FaxSettings_Label.Location = new System.Drawing.Point(347, 10);
            this.FaxSettings_Label.Name = "FaxSettings_Label";
            this.FaxSettings_Label.Size = new System.Drawing.Size(65, 13);
            this.FaxSettings_Label.TabIndex = 52;
            this.FaxSettings_Label.Text = "Fax Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.fileFormat_ChoiceControl);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.thirdParty_ChoiceControl);
            this.groupBox2.Location = new System.Drawing.Point(371, 254);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(349, 120);
            this.groupBox2.TabIndex = 58;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Lan Fax Setup";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 50;
            this.label2.Text = "File Format";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 49;
            this.label1.Text = "Third Party Lan Fax Product";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(609, 219);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 58;
            this.label6.Text = "Password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(609, 194);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 56;
            this.label5.Text = "User Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(609, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 54;
            this.label4.Text = "Windows Domain";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(609, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 52;
            this.label3.Text = "UNC Folder Path";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(374, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 60;
            this.label7.Text = "Fax Send Method:";
            // 
            // field4_TextBox
            // 
            this.field4_TextBox.Enabled = false;
            this.field4_TextBox.Location = new System.Drawing.Point(373, 216);
            this.field4_TextBox.Name = "field4_TextBox";
            this.field4_TextBox.Size = new System.Drawing.Size(229, 20);
            this.field4_TextBox.TabIndex = 67;
            // 
            // field1_TextBox
            // 
            this.field1_TextBox.Enabled = false;
            this.field1_TextBox.Location = new System.Drawing.Point(374, 138);
            this.field1_TextBox.Name = "field1_TextBox";
            this.field1_TextBox.Size = new System.Drawing.Size(229, 20);
            this.field1_TextBox.TabIndex = 64;
            // 
            // field3_TextBox
            // 
            this.field3_TextBox.Enabled = false;
            this.field3_TextBox.Location = new System.Drawing.Point(374, 190);
            this.field3_TextBox.Name = "field3_TextBox";
            this.field3_TextBox.Size = new System.Drawing.Size(229, 20);
            this.field3_TextBox.TabIndex = 66;
            // 
            // field2_TextBox
            // 
            this.field2_TextBox.Enabled = false;
            this.field2_TextBox.Location = new System.Drawing.Point(374, 164);
            this.field2_TextBox.Name = "field2_TextBox";
            this.field2_TextBox.Size = new System.Drawing.Size(229, 20);
            this.field2_TextBox.TabIndex = 65;
            // 
            // modem_groupBox
            // 
            this.modem_groupBox.Controls.Add(this.number_label);
            this.modem_groupBox.Controls.Add(this.company_label);
            this.modem_groupBox.Controls.Add(this.location_label);
            this.modem_groupBox.Controls.Add(this.faxNumber_choiceTextControl);
            this.modem_groupBox.Controls.Add(this.companyName_choiceTextControl);
            this.modem_groupBox.Controls.Add(this.location_choiceTextControl);
            this.modem_groupBox.Location = new System.Drawing.Point(374, 380);
            this.modem_groupBox.Name = "modem_groupBox";
            this.modem_groupBox.Size = new System.Drawing.Size(346, 155);
            this.modem_groupBox.TabIndex = 68;
            this.modem_groupBox.TabStop = false;
            this.modem_groupBox.Text = "Device Modem Settings";
            // 
            // location_label
            // 
            this.location_label.AutoSize = true;
            this.location_label.Location = new System.Drawing.Point(4, 17);
            this.location_label.Name = "location_label";
            this.location_label.Size = new System.Drawing.Size(48, 13);
            this.location_label.TabIndex = 51;
            this.location_label.Text = "Location";
            // 
            // company_label
            // 
            this.company_label.AutoSize = true;
            this.company_label.Location = new System.Drawing.Point(4, 61);
            this.company_label.Name = "company_label";
            this.company_label.Size = new System.Drawing.Size(82, 13);
            this.company_label.TabIndex = 52;
            this.company_label.Text = "Company Name";
            // 
            // number_label
            // 
            this.number_label.AutoSize = true;
            this.number_label.Location = new System.Drawing.Point(4, 105);
            this.number_label.Name = "number_label";
            this.number_label.Size = new System.Drawing.Size(64, 13);
            this.number_label.TabIndex = 53;
            this.number_label.Text = "Fax Number";
            // 
            // faxNumber_choiceTextControl
            // 
            this.faxNumber_choiceTextControl.Location = new System.Drawing.Point(4, 120);
            this.faxNumber_choiceTextControl.Name = "faxNumber_choiceTextControl";
            this.faxNumber_choiceTextControl.Size = new System.Drawing.Size(335, 27);
            this.faxNumber_choiceTextControl.TabIndex = 2;
            // 
            // companyName_choiceTextControl
            // 
            this.companyName_choiceTextControl.Location = new System.Drawing.Point(4, 76);
            this.companyName_choiceTextControl.Name = "companyName_choiceTextControl";
            this.companyName_choiceTextControl.Size = new System.Drawing.Size(335, 27);
            this.companyName_choiceTextControl.TabIndex = 1;
            // 
            // location_choiceTextControl
            // 
            this.location_choiceTextControl.Location = new System.Drawing.Point(4, 32);
            this.location_choiceTextControl.Name = "location_choiceTextControl";
            this.location_choiceTextControl.Size = new System.Drawing.Size(335, 27);
            this.location_choiceTextControl.TabIndex = 0;
            // 
            // faxMethod_ComboBox
            // 
            this.faxMethod_ComboBox.AutoSize = true;
            this.faxMethod_ComboBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.faxMethod_ComboBox.Location = new System.Drawing.Point(371, 95);
            this.faxMethod_ComboBox.Name = "faxMethod_ComboBox";
            this.faxMethod_ComboBox.Size = new System.Drawing.Size(338, 27);
            this.faxMethod_ComboBox.TabIndex = 59;
            // 
            // fileFormat_ChoiceControl
            // 
            this.fileFormat_ChoiceControl.AutoSize = true;
            this.fileFormat_ChoiceControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fileFormat_ChoiceControl.Location = new System.Drawing.Point(6, 78);
            this.fileFormat_ChoiceControl.Name = "fileFormat_ChoiceControl";
            this.fileFormat_ChoiceControl.Size = new System.Drawing.Size(338, 27);
            this.fileFormat_ChoiceControl.TabIndex = 51;
            // 
            // thirdParty_ChoiceControl
            // 
            this.thirdParty_ChoiceControl.AutoSize = true;
            this.thirdParty_ChoiceControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.thirdParty_ChoiceControl.Location = new System.Drawing.Point(6, 32);
            this.thirdParty_ChoiceControl.Name = "thirdParty_ChoiceControl";
            this.thirdParty_ChoiceControl.Size = new System.Drawing.Size(338, 27);
            this.thirdParty_ChoiceControl.TabIndex = 48;
            // 
            // scanSettingsUserControl
            // 
            this.scanSettingsUserControl.Location = new System.Drawing.Point(4, 14);
            this.scanSettingsUserControl.Name = "scanSettingsUserControl";
            this.scanSettingsUserControl.Size = new System.Drawing.Size(340, 400);
            this.scanSettingsUserControl.TabIndex = 48;
            // 
            // resolution_choiceControl
            // 
            this.resolution_choiceControl.AutoSize = true;
            this.resolution_choiceControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.resolution_choiceControl.Location = new System.Drawing.Point(6, 432);
            this.resolution_choiceControl.Name = "resolution_choiceControl";
            this.resolution_choiceControl.Size = new System.Drawing.Size(338, 27);
            this.resolution_choiceControl.TabIndex = 47;
            // 
            // enable_ComboBox
            // 
            this.enable_ComboBox.AutoSize = true;
            this.enable_ComboBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.enable_ComboBox.Location = new System.Drawing.Point(371, 50);
            this.enable_ComboBox.Name = "enable_ComboBox";
            this.enable_ComboBox.Size = new System.Drawing.Size(338, 27);
            this.enable_ComboBox.TabIndex = 53;
            // 
            // FaxDefaultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.modem_groupBox);
            this.Controls.Add(this.field4_TextBox);
            this.Controls.Add(this.field1_TextBox);
            this.Controls.Add(this.field3_TextBox);
            this.Controls.Add(this.field2_TextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.faxMethod_ComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.folder_Label);
            this.Controls.Add(this.enable_ComboBox);
            this.Controls.Add(this.FaxSettings_Label);
            this.Name = "FaxDefaultControl";
            this.Size = new System.Drawing.Size(741, 560);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.modem_groupBox.ResumeLayout(false);
            this.modem_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private ScanSettingsUserControl scanSettingsUserControl;
        private ChoiceComboControl resolution_choiceControl;
        private System.Windows.Forms.Label resolution_label;
        private System.Windows.Forms.Label folder_Label;
        public ChoiceComboControl enable_ComboBox;
        private System.Windows.Forms.Label FaxSettings_Label;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private ChoiceComboControl fileFormat_ChoiceControl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private ChoiceComboControl thirdParty_ChoiceControl;
        public ChoiceComboControl faxMethod_ComboBox;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.MaskedTextBox field4_TextBox;
        public System.Windows.Forms.TextBox field1_TextBox;
        public System.Windows.Forms.MaskedTextBox field3_TextBox;
        public System.Windows.Forms.TextBox field2_TextBox;
        private System.Windows.Forms.GroupBox modem_groupBox;
        private System.Windows.Forms.Label number_label;
        private System.Windows.Forms.Label company_label;
        private System.Windows.Forms.Label location_label;
        private ChoiceTextControl faxNumber_choiceTextControl;
        private ChoiceTextControl companyName_choiceTextControl;
        private ChoiceTextControl location_choiceTextControl;
    }
}
