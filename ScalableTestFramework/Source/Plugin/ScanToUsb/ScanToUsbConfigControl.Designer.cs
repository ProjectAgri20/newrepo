namespace HP.ScalableTest.Plugin.ScanToUsb
{
    partial class ScanToUsbConfigControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox_Authentication = new System.Windows.Forms.GroupBox();
            this.radioButton_ScanToUSB = new System.Windows.Forms.RadioButton();
            this.radioButton_SignInButton = new System.Windows.Forms.RadioButton();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.scanoptions_groupBox = new System.Windows.Forms.GroupBox();
            this.ScanOptions_Button = new System.Windows.Forms.Button();
            this.useOcr_CheckBox = new System.Windows.Forms.CheckBox();
            this.usb_TextBox = new System.Windows.Forms.TextBox();
            this.quickSet_TextBox = new System.Windows.Forms.TextBox();
            this.quickSet_RadioButton = new System.Windows.Forms.RadioButton();
            this.usbName_RadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pageCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.scanConfiguration_TabPage = new System.Windows.Forms.TabPage();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.SimulatorAssetSelectionControl();
            this.loggingOptions_TabPage = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.usesDigitalSendServer_CheckBox = new System.Windows.Forms.CheckBox();
            this.digitalSendServer_TextBox = new System.Windows.Forms.TextBox();
            this.digitalSendServerName_Label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.panel1.SuspendLayout();
            this.groupBox_Authentication.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.scanoptions_groupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.scanConfiguration_TabPage.SuspendLayout();
            this.loggingOptions_TabPage.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox_Authentication);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(661, 300);
            this.panel1.TabIndex = 0;
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Controls.Add(this.radioButton_ScanToUSB);
            this.groupBox_Authentication.Controls.Add(this.radioButton_SignInButton);
            this.groupBox_Authentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBox_Authentication.Controls.Add(this.label_AuthMethod);
            this.groupBox_Authentication.Location = new System.Drawing.Point(1, 108);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(658, 49);
            this.groupBox_Authentication.TabIndex = 95;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication";
            // 
            // radioButton_ScanToUSB
            // 
            this.radioButton_ScanToUSB.AutoSize = true;
            this.radioButton_ScanToUSB.Checked = true;
            this.radioButton_ScanToUSB.Location = new System.Drawing.Point(116, 20);
            this.radioButton_ScanToUSB.Name = "radioButton_ScanToUSB";
            this.radioButton_ScanToUSB.Size = new System.Drawing.Size(84, 19);
            this.radioButton_ScanToUSB.TabIndex = 93;
            this.radioButton_ScanToUSB.TabStop = true;
            this.radioButton_ScanToUSB.Text = "ScanToUSB";
            this.radioButton_ScanToUSB.UseVisualStyleBackColor = true;
            // 
            // radioButton_SignInButton
            // 
            this.radioButton_SignInButton.AutoSize = true;
            this.radioButton_SignInButton.Location = new System.Drawing.Point(18, 20);
            this.radioButton_SignInButton.Name = "radioButton_SignInButton";
            this.radioButton_SignInButton.Size = new System.Drawing.Size(61, 19);
            this.radioButton_SignInButton.TabIndex = 92;
            this.radioButton_SignInButton.Text = "Sign In";
            this.radioButton_SignInButton.UseVisualStyleBackColor = true;
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(404, 17);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(170, 23);
            this.comboBox_AuthProvider.TabIndex = 90;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(264, 22);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(131, 15);
            this.label_AuthMethod.TabIndex = 91;
            this.label_AuthMethod.Text = "Authentication Method";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.scanoptions_groupBox);
            this.groupBox3.Controls.Add(this.useOcr_CheckBox);
            this.groupBox3.Controls.Add(this.usb_TextBox);
            this.groupBox3.Controls.Add(this.quickSet_TextBox);
            this.groupBox3.Controls.Add(this.quickSet_RadioButton);
            this.groupBox3.Controls.Add(this.usbName_RadioButton);
            this.groupBox3.Location = new System.Drawing.Point(0, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(658, 104);
            this.groupBox3.TabIndex = 49;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Scan Destination";
            // 
            // scanoptions_groupBox
            // 
            this.scanoptions_groupBox.Controls.Add(this.ScanOptions_Button);
            this.scanoptions_groupBox.Location = new System.Drawing.Point(507, 41);
            this.scanoptions_groupBox.Name = "scanoptions_groupBox";
            this.scanoptions_groupBox.Size = new System.Drawing.Size(145, 59);
            this.scanoptions_groupBox.TabIndex = 57;
            this.scanoptions_groupBox.TabStop = false;
            this.scanoptions_groupBox.Text = "Scan Options";
            // 
            // ScanOptions_Button
            // 
            this.ScanOptions_Button.Location = new System.Drawing.Point(15, 19);
            this.ScanOptions_Button.Name = "ScanOptions_Button";
            this.ScanOptions_Button.Size = new System.Drawing.Size(114, 34);
            this.ScanOptions_Button.TabIndex = 0;
            this.ScanOptions_Button.Text = "Scan Options";
            this.ScanOptions_Button.UseVisualStyleBackColor = true;
            this.ScanOptions_Button.Click += new System.EventHandler(this.ScanOptions_Button_Click);
            // 
            // useOcr_CheckBox
            // 
            this.useOcr_CheckBox.AutoSize = true;
            this.useOcr_CheckBox.Location = new System.Drawing.Point(531, 23);
            this.useOcr_CheckBox.Name = "useOcr_CheckBox";
            this.useOcr_CheckBox.Size = new System.Drawing.Size(110, 19);
            this.useOcr_CheckBox.TabIndex = 52;
            this.useOcr_CheckBox.Text = "Use OCR for job";
            this.useOcr_CheckBox.UseVisualStyleBackColor = true;
            this.useOcr_CheckBox.CheckedChanged += new System.EventHandler(this.useOcr_CheckBox_CheckedChanged);
            // 
            // usb_TextBox
            // 
            this.usb_TextBox.Location = new System.Drawing.Point(141, 26);
            this.usb_TextBox.Name = "usb_TextBox";
            this.usb_TextBox.Size = new System.Drawing.Size(355, 23);
            this.usb_TextBox.TabIndex = 51;
            // 
            // quickSet_TextBox
            // 
            this.quickSet_TextBox.Enabled = false;
            this.quickSet_TextBox.Location = new System.Drawing.Point(141, 63);
            this.quickSet_TextBox.Name = "quickSet_TextBox";
            this.quickSet_TextBox.Size = new System.Drawing.Size(355, 23);
            this.quickSet_TextBox.TabIndex = 50;
            // 
            // quickSet_RadioButton
            // 
            this.quickSet_RadioButton.AutoSize = true;
            this.quickSet_RadioButton.Location = new System.Drawing.Point(21, 64);
            this.quickSet_RadioButton.Name = "quickSet_RadioButton";
            this.quickSet_RadioButton.Size = new System.Drawing.Size(114, 19);
            this.quickSet_RadioButton.TabIndex = 49;
            this.quickSet_RadioButton.TabStop = true;
            this.quickSet_RadioButton.Text = "Named QuickSet";
            this.quickSet_RadioButton.UseVisualStyleBackColor = true;
            // 
            // usbName_RadioButton
            // 
            this.usbName_RadioButton.AutoSize = true;
            this.usbName_RadioButton.Checked = true;
            this.usbName_RadioButton.Location = new System.Drawing.Point(21, 27);
            this.usbName_RadioButton.Name = "usbName_RadioButton";
            this.usbName_RadioButton.Size = new System.Drawing.Size(81, 19);
            this.usbName_RadioButton.TabIndex = 48;
            this.usbName_RadioButton.TabStop = true;
            this.usbName_RadioButton.Text = "USB Name";
            this.usbName_RadioButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.usbName_RadioButton.UseVisualStyleBackColor = true;
            this.usbName_RadioButton.CheckedChanged += new System.EventHandler(this.usbName_RadioButton_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.pageCount_NumericUpDown);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(3, 162);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(296, 125);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Page Count";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "Page Count";
            // 
            // pageCount_NumericUpDown
            // 
            this.pageCount_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pageCount_NumericUpDown.Location = new System.Drawing.Point(118, 94);
            this.pageCount_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pageCount_NumericUpDown.Name = "pageCount_NumericUpDown";
            this.pageCount_NumericUpDown.Size = new System.Drawing.Size(107, 23);
            this.pageCount_NumericUpDown.TabIndex = 12;
            this.pageCount_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(7, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(282, 57);
            this.label3.TabIndex = 11;
            this.label3.Text = "Select the number of pages for the scanned document.  The page count will be the " +
    "same for all devices, whether physical or virtual.";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.lockTimeoutControl);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Location = new System.Drawing.Point(302, 164);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(359, 125);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Skip Delay";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(7, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(345, 57);
            this.label2.TabIndex = 10;
            this.label2.Text = "Exclusive access to the selected device is required to prevent other scan activit" +
    "ies from interfering. If exclusive access cannot be obtained within the time bel" +
    "ow, the activity will be skipped.\r\n";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.scanConfiguration_TabPage);
            this.tabControl.Controls.Add(this.loggingOptions_TabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(675, 560);
            this.tabControl.TabIndex = 48;
            // 
            // scanConfiguration_TabPage
            // 
            this.scanConfiguration_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.scanConfiguration_TabPage.Controls.Add(this.assetSelectionControl);
            this.scanConfiguration_TabPage.Controls.Add(this.panel1);
            this.scanConfiguration_TabPage.Location = new System.Drawing.Point(4, 24);
            this.scanConfiguration_TabPage.Name = "scanConfiguration_TabPage";
            this.scanConfiguration_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.scanConfiguration_TabPage.Size = new System.Drawing.Size(667, 532);
            this.scanConfiguration_TabPage.TabIndex = 0;
            this.scanConfiguration_TabPage.Text = "Scan Configuration";
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(3, 303);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(661, 226);
            this.assetSelectionControl.TabIndex = 1;
            // 
            // loggingOptions_TabPage
            // 
            this.loggingOptions_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.loggingOptions_TabPage.Controls.Add(this.groupBox7);
            this.loggingOptions_TabPage.Controls.Add(this.label1);
            this.loggingOptions_TabPage.Location = new System.Drawing.Point(4, 24);
            this.loggingOptions_TabPage.Name = "loggingOptions_TabPage";
            this.loggingOptions_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.loggingOptions_TabPage.Size = new System.Drawing.Size(667, 532);
            this.loggingOptions_TabPage.TabIndex = 1;
            this.loggingOptions_TabPage.Text = "Logging Options";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.usesDigitalSendServer_CheckBox);
            this.groupBox7.Controls.Add(this.digitalSendServer_TextBox);
            this.groupBox7.Controls.Add(this.digitalSendServerName_Label);
            this.groupBox7.Location = new System.Drawing.Point(9, 46);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(317, 104);
            this.groupBox7.TabIndex = 11;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Digital Send Service";
            // 
            // usesDigitalSendServer_CheckBox
            // 
            this.usesDigitalSendServer_CheckBox.AutoSize = true;
            this.usesDigitalSendServer_CheckBox.Checked = true;
            this.usesDigitalSendServer_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.usesDigitalSendServer_CheckBox.Location = new System.Drawing.Point(21, 27);
            this.usesDigitalSendServer_CheckBox.Name = "usesDigitalSendServer_CheckBox";
            this.usesDigitalSendServer_CheckBox.Size = new System.Drawing.Size(245, 19);
            this.usesDigitalSendServer_CheckBox.TabIndex = 2;
            this.usesDigitalSendServer_CheckBox.Text = "Activity will use Digital Send Service (DSS)";
            this.usesDigitalSendServer_CheckBox.UseVisualStyleBackColor = true;
            this.usesDigitalSendServer_CheckBox.CheckedChanged += new System.EventHandler(this.usesDigitalSendServer_CheckBox_CheckedChanged);
            // 
            // digitalSendServer_TextBox
            // 
            this.digitalSendServer_TextBox.Location = new System.Drawing.Point(98, 62);
            this.digitalSendServer_TextBox.Name = "digitalSendServer_TextBox";
            this.digitalSendServer_TextBox.Size = new System.Drawing.Size(202, 23);
            this.digitalSendServer_TextBox.TabIndex = 1;
            // 
            // digitalSendServerName_Label
            // 
            this.digitalSendServerName_Label.AutoSize = true;
            this.digitalSendServerName_Label.Location = new System.Drawing.Point(18, 65);
            this.digitalSendServerName_Label.Name = "digitalSendServerName_Label";
            this.digitalSendServerName_Label.Size = new System.Drawing.Size(74, 15);
            this.digitalSendServerName_Label.TabIndex = 0;
            this.digitalSendServerName_Label.Text = "Server Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(649, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "The options selected here will modify the data that is logged by this activity, b" +
    "ut will not affect the parameters of the scan.";
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(6, 66);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 11;
            // 
            // ScanToUsbConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ScanToUsbConfigControl";
            this.Size = new System.Drawing.Size(675, 560);
            this.panel1.ResumeLayout(false);
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.scanoptions_groupBox.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.scanConfiguration_TabPage.ResumeLayout(false);
            this.loggingOptions_TabPage.ResumeLayout(false);
            this.loggingOptions_TabPage.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown pageCount_NumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox quickSet_TextBox;
        private System.Windows.Forms.RadioButton quickSet_RadioButton;
        private System.Windows.Forms.RadioButton usbName_RadioButton;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage scanConfiguration_TabPage;
        private System.Windows.Forms.TabPage loggingOptions_TabPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox usb_TextBox;
        private System.Windows.Forms.CheckBox useOcr_CheckBox;
        private Framework.UI.SimulatorAssetSelectionControl assetSelectionControl;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox usesDigitalSendServer_CheckBox;
        private System.Windows.Forms.TextBox digitalSendServer_TextBox;
        private System.Windows.Forms.Label digitalSendServerName_Label;
        private System.Windows.Forms.GroupBox scanoptions_groupBox;
        private System.Windows.Forms.Button ScanOptions_Button;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.Windows.Forms.RadioButton radioButton_ScanToUSB;
        private System.Windows.Forms.RadioButton radioButton_SignInButton;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.Label label_AuthMethod;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
    }
}



