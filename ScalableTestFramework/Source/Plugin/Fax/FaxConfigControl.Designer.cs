namespace HP.ScalableTest.Plugin.Fax
{
    partial class FaxConfigControl
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
            this.main_TabControl = new System.Windows.Forms.TabControl();
            this.scanConfiguration_TabPage = new System.Windows.Forms.TabPage();
            this.jobSeparator_GroupBox = new System.Windows.Forms.GroupBox();
            this.jobseparator_checkBox = new System.Windows.Forms.CheckBox();
            this.localQueue_button = new System.Windows.Forms.Button();
            this.groupBox_Authentication = new System.Windows.Forms.GroupBox();
            this.radioButton_Fax = new System.Windows.Forms.RadioButton();
            this.radioButton_SignInButton = new System.Windows.Forms.RadioButton();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.FaxOperation_groupBox = new System.Windows.Forms.GroupBox();
            this.FaxOptions_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.PIN_textBox = new System.Windows.Forms.TextBox();
            this.useSpeedDial_Checkbox = new System.Windows.Forms.CheckBox();
            this.labelFaxTimeoutHelp = new System.Windows.Forms.TextBox();
            this.timeSpanControlFaxReceive = new HP.ScalableTest.Framework.UI.TimeSpanControl();
            this.labelFaxTimeout = new System.Windows.Forms.Label();
            this.faxNumber_textBox = new System.Windows.Forms.TextBox();
            this.labelFaxNumber = new System.Windows.Forms.Label();
            this.comboBoxFaxType = new System.Windows.Forms.ComboBox();
            this.groupBoxFaxOperation = new System.Windows.Forms.GroupBox();
            this.radioFaxReceive = new System.Windows.Forms.RadioButton();
            this.radioFaxSend = new System.Windows.Forms.RadioButton();
            this.labelFaxType = new System.Windows.Forms.Label();
            this.ExecutionSettingGroup = new System.Windows.Forms.GroupBox();
            this.RestrictRadioButton = new System.Windows.Forms.RadioButton();
            this.GenerateRadioButton = new System.Windows.Forms.RadioButton();
            this.OptionalRadioButton = new System.Windows.Forms.RadioButton();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.SimulatorAssetSelectionControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.domain_Label = new System.Windows.Forms.Label();
            this.email_ComboBox = new System.Windows.Forms.ComboBox();
            this.notify_CheckBox = new System.Windows.Forms.CheckBox();
            this.page_GroupBox = new System.Windows.Forms.GroupBox();
            this.pageCount_Label = new System.Windows.Forms.Label();
            this.pageCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.pageText_Label = new System.Windows.Forms.Label();
            this.skip_GroupBox = new System.Windows.Forms.GroupBox();
            this.skipText_Label = new System.Windows.Forms.Label();
            this.loggingOptions_TabPage = new System.Windows.Forms.TabPage();
            this.dss_GroupBox = new System.Windows.Forms.GroupBox();
            this.usesDigitalSendServer_CheckBox = new System.Windows.Forms.CheckBox();
            this.digitalSendServer_TextBox = new System.Windows.Forms.TextBox();
            this.digitalSendServerName_Label = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.main_TabControl.SuspendLayout();
            this.scanConfiguration_TabPage.SuspendLayout();
            this.jobSeparator_GroupBox.SuspendLayout();
            this.groupBox_Authentication.SuspendLayout();
            this.FaxOperation_groupBox.SuspendLayout();
            this.groupBoxFaxOperation.SuspendLayout();
            this.ExecutionSettingGroup.SuspendLayout();
            this.panel2.SuspendLayout();
            this.page_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).BeginInit();
            this.skip_GroupBox.SuspendLayout();
            this.loggingOptions_TabPage.SuspendLayout();
            this.dss_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // main_TabControl
            // 
            this.main_TabControl.Controls.Add(this.scanConfiguration_TabPage);
            this.main_TabControl.Controls.Add(this.loggingOptions_TabPage);
            this.main_TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_TabControl.Location = new System.Drawing.Point(0, 0);
            this.main_TabControl.Name = "main_TabControl";
            this.main_TabControl.SelectedIndex = 0;
            this.main_TabControl.Size = new System.Drawing.Size(682, 713);
            this.main_TabControl.TabIndex = 53;
            // 
            // scanConfiguration_TabPage
            // 
            this.scanConfiguration_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.scanConfiguration_TabPage.Controls.Add(this.jobSeparator_GroupBox);
            this.scanConfiguration_TabPage.Controls.Add(this.groupBox_Authentication);
            this.scanConfiguration_TabPage.Controls.Add(this.FaxOperation_groupBox);
            this.scanConfiguration_TabPage.Controls.Add(this.ExecutionSettingGroup);
            this.scanConfiguration_TabPage.Controls.Add(this.assetSelectionControl);
            this.scanConfiguration_TabPage.Controls.Add(this.panel2);
            this.scanConfiguration_TabPage.Location = new System.Drawing.Point(4, 24);
            this.scanConfiguration_TabPage.Name = "scanConfiguration_TabPage";
            this.scanConfiguration_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.scanConfiguration_TabPage.Size = new System.Drawing.Size(674, 685);
            this.scanConfiguration_TabPage.TabIndex = 0;
            this.scanConfiguration_TabPage.Text = "Scan Configuration";
            // 
            // jobSeparator_GroupBox
            // 
            this.jobSeparator_GroupBox.Controls.Add(this.jobseparator_checkBox);
            this.jobSeparator_GroupBox.Controls.Add(this.localQueue_button);
            this.jobSeparator_GroupBox.Location = new System.Drawing.Point(364, 413);
            this.jobSeparator_GroupBox.Name = "jobSeparator_GroupBox";
            this.jobSeparator_GroupBox.Size = new System.Drawing.Size(280, 53);
            this.jobSeparator_GroupBox.TabIndex = 94;
            this.jobSeparator_GroupBox.TabStop = false;
            this.jobSeparator_GroupBox.Text = "Job Separator Configuration";
            // 
            // jobseparator_checkBox
            // 
            this.jobseparator_checkBox.AutoSize = true;
            this.jobseparator_checkBox.Location = new System.Drawing.Point(143, 22);
            this.jobseparator_checkBox.Name = "jobseparator_checkBox";
            this.jobseparator_checkBox.Size = new System.Drawing.Size(123, 19);
            this.jobseparator_checkBox.TabIndex = 56;
            this.jobseparator_checkBox.Text = "Print job separator";
            this.jobseparator_checkBox.UseVisualStyleBackColor = true;
            // 
            // localQueue_button
            // 
            this.localQueue_button.Location = new System.Drawing.Point(20, 18);
            this.localQueue_button.Name = "localQueue_button";
            this.localQueue_button.Size = new System.Drawing.Size(97, 25);
            this.localQueue_button.TabIndex = 19;
            this.localQueue_button.Text = "Local Queue";
            this.localQueue_button.UseVisualStyleBackColor = true;
            this.localQueue_button.Click += new System.EventHandler(this.localQueue_button_Click);
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Controls.Add(this.radioButton_Fax);
            this.groupBox_Authentication.Controls.Add(this.radioButton_SignInButton);
            this.groupBox_Authentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBox_Authentication.Controls.Add(this.label_AuthMethod);
            this.groupBox_Authentication.Location = new System.Drawing.Point(3, 188);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(651, 50);
            this.groupBox_Authentication.TabIndex = 93;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication";
            // 
            // radioButton_Fax
            // 
            this.radioButton_Fax.AutoSize = true;
            this.radioButton_Fax.Checked = true;
            this.radioButton_Fax.Location = new System.Drawing.Point(116, 19);
            this.radioButton_Fax.Name = "radioButton_Fax";
            this.radioButton_Fax.Size = new System.Drawing.Size(42, 19);
            this.radioButton_Fax.TabIndex = 93;
            this.radioButton_Fax.TabStop = true;
            this.radioButton_Fax.Text = "Fax";
            this.radioButton_Fax.UseVisualStyleBackColor = true;
            // 
            // radioButton_SignInButton
            // 
            this.radioButton_SignInButton.AutoSize = true;
            this.radioButton_SignInButton.Checked = true;
            this.radioButton_SignInButton.Location = new System.Drawing.Point(18, 20);
            this.radioButton_SignInButton.Name = "radioButton_SignInButton";
            this.radioButton_SignInButton.Size = new System.Drawing.Size(61, 19);
            this.radioButton_SignInButton.TabIndex = 92;
            this.radioButton_SignInButton.TabStop = true;
            this.radioButton_SignInButton.Text = "Sign In";
            this.radioButton_SignInButton.UseVisualStyleBackColor = true;
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(488, 21);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(130, 23);
            this.comboBox_AuthProvider.TabIndex = 90;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(342, 23);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(131, 15);
            this.label_AuthMethod.TabIndex = 91;
            this.label_AuthMethod.Text = "Authentication Method";
            // 
            // FaxOperation_groupBox
            // 
            this.FaxOperation_groupBox.Controls.Add(this.FaxOptions_Button);
            this.FaxOperation_groupBox.Controls.Add(this.label1);
            this.FaxOperation_groupBox.Controls.Add(this.PIN_textBox);
            this.FaxOperation_groupBox.Controls.Add(this.useSpeedDial_Checkbox);
            this.FaxOperation_groupBox.Controls.Add(this.labelFaxTimeoutHelp);
            this.FaxOperation_groupBox.Controls.Add(this.timeSpanControlFaxReceive);
            this.FaxOperation_groupBox.Controls.Add(this.labelFaxTimeout);
            this.FaxOperation_groupBox.Controls.Add(this.faxNumber_textBox);
            this.FaxOperation_groupBox.Controls.Add(this.labelFaxNumber);
            this.FaxOperation_groupBox.Controls.Add(this.comboBoxFaxType);
            this.FaxOperation_groupBox.Controls.Add(this.groupBoxFaxOperation);
            this.FaxOperation_groupBox.Controls.Add(this.labelFaxType);
            this.FaxOperation_groupBox.Location = new System.Drawing.Point(6, 241);
            this.FaxOperation_groupBox.Name = "FaxOperation_groupBox";
            this.FaxOperation_groupBox.Size = new System.Drawing.Size(648, 171);
            this.FaxOperation_groupBox.TabIndex = 73;
            this.FaxOperation_groupBox.TabStop = false;
            this.FaxOperation_groupBox.Text = "Fax Operation";
            // 
            // FaxOptions_Button
            // 
            this.FaxOptions_Button.Location = new System.Drawing.Point(20, 132);
            this.FaxOptions_Button.Name = "FaxOptions_Button";
            this.FaxOptions_Button.Size = new System.Drawing.Size(114, 29);
            this.FaxOptions_Button.TabIndex = 0;
            this.FaxOptions_Button.Text = "Fax Options";
            this.FaxOptions_Button.UseVisualStyleBackColor = true;
            this.FaxOptions_Button.Click += new System.EventHandler(this.FaxOptions_Button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(375, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 15);
            this.label1.TabIndex = 85;
            this.label1.Text = "PIN";
            // 
            // PIN_textBox
            // 
            this.PIN_textBox.Location = new System.Drawing.Point(501, 113);
            this.PIN_textBox.Name = "PIN_textBox";
            this.PIN_textBox.Size = new System.Drawing.Size(130, 23);
            this.PIN_textBox.TabIndex = 84;
            // 
            // useSpeedDial_Checkbox
            // 
            this.useSpeedDial_Checkbox.AutoSize = true;
            this.useSpeedDial_Checkbox.Location = new System.Drawing.Point(501, 43);
            this.useSpeedDial_Checkbox.Name = "useSpeedDial_Checkbox";
            this.useSpeedDial_Checkbox.Size = new System.Drawing.Size(103, 19);
            this.useSpeedDial_Checkbox.TabIndex = 83;
            this.useSpeedDial_Checkbox.Text = "Use Speed Dial";
            this.useSpeedDial_Checkbox.UseVisualStyleBackColor = true;
            // 
            // labelFaxTimeoutHelp
            // 
            this.labelFaxTimeoutHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFaxTimeoutHelp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.labelFaxTimeoutHelp.Location = new System.Drawing.Point(344, 142);
            this.labelFaxTimeoutHelp.Multiline = true;
            this.labelFaxTimeoutHelp.Name = "labelFaxTimeoutHelp";
            this.labelFaxTimeoutHelp.ReadOnly = true;
            this.labelFaxTimeoutHelp.Size = new System.Drawing.Size(290, 38);
            this.labelFaxTimeoutHelp.TabIndex = 82;
            this.labelFaxTimeoutHelp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // timeSpanControlFaxReceive
            // 
            this.timeSpanControlFaxReceive.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeSpanControlFaxReceive.Location = new System.Drawing.Point(501, 70);
            this.timeSpanControlFaxReceive.Margin = new System.Windows.Forms.Padding(0);
            this.timeSpanControlFaxReceive.Name = "timeSpanControlFaxReceive";
            this.timeSpanControlFaxReceive.Size = new System.Drawing.Size(130, 25);
            this.timeSpanControlFaxReceive.TabIndex = 81;
            // 
            // labelFaxTimeout
            // 
            this.labelFaxTimeout.AutoSize = true;
            this.labelFaxTimeout.Location = new System.Drawing.Point(374, 80);
            this.labelFaxTimeout.Name = "labelFaxTimeout";
            this.labelFaxTimeout.Size = new System.Drawing.Size(115, 15);
            this.labelFaxTimeout.TabIndex = 80;
            this.labelFaxTimeout.Text = "Fax Receive Timeout";
            // 
            // faxNumber_textBox
            // 
            this.faxNumber_textBox.Location = new System.Drawing.Point(501, 14);
            this.faxNumber_textBox.Name = "faxNumber_textBox";
            this.faxNumber_textBox.Size = new System.Drawing.Size(130, 23);
            this.faxNumber_textBox.TabIndex = 79;
            // 
            // labelFaxNumber
            // 
            this.labelFaxNumber.AutoSize = true;
            this.labelFaxNumber.Location = new System.Drawing.Point(374, 19);
            this.labelFaxNumber.Name = "labelFaxNumber";
            this.labelFaxNumber.Size = new System.Drawing.Size(71, 15);
            this.labelFaxNumber.TabIndex = 78;
            this.labelFaxNumber.Text = "Fax Number";
            // 
            // comboBoxFaxType
            // 
            this.comboBoxFaxType.AllowDrop = true;
            this.comboBoxFaxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFaxType.Location = new System.Drawing.Point(72, 21);
            this.comboBoxFaxType.Name = "comboBoxFaxType";
            this.comboBoxFaxType.Size = new System.Drawing.Size(154, 23);
            this.comboBoxFaxType.TabIndex = 77;
            // 
            // groupBoxFaxOperation
            // 
            this.groupBoxFaxOperation.Controls.Add(this.radioFaxReceive);
            this.groupBoxFaxOperation.Controls.Add(this.radioFaxSend);
            this.groupBoxFaxOperation.Location = new System.Drawing.Point(17, 61);
            this.groupBoxFaxOperation.Name = "groupBoxFaxOperation";
            this.groupBoxFaxOperation.Size = new System.Drawing.Size(255, 65);
            this.groupBoxFaxOperation.TabIndex = 76;
            this.groupBoxFaxOperation.TabStop = false;
            this.groupBoxFaxOperation.Text = "Fax Operation";
            // 
            // radioFaxReceive
            // 
            this.radioFaxReceive.AutoSize = true;
            this.radioFaxReceive.Location = new System.Drawing.Point(136, 28);
            this.radioFaxReceive.Name = "radioFaxReceive";
            this.radioFaxReceive.Size = new System.Drawing.Size(85, 19);
            this.radioFaxReceive.TabIndex = 57;
            this.radioFaxReceive.TabStop = true;
            this.radioFaxReceive.Text = "Receive Fax";
            this.radioFaxReceive.UseVisualStyleBackColor = true;
            // 
            // radioFaxSend
            // 
            this.radioFaxSend.AutoSize = true;
            this.radioFaxSend.Location = new System.Drawing.Point(38, 28);
            this.radioFaxSend.Name = "radioFaxSend";
            this.radioFaxSend.Size = new System.Drawing.Size(71, 19);
            this.radioFaxSend.TabIndex = 56;
            this.radioFaxSend.TabStop = true;
            this.radioFaxSend.Text = "Send Fax";
            this.radioFaxSend.UseVisualStyleBackColor = true;
            // 
            // labelFaxType
            // 
            this.labelFaxType.AutoSize = true;
            this.labelFaxType.Location = new System.Drawing.Point(14, 24);
            this.labelFaxType.Name = "labelFaxType";
            this.labelFaxType.Size = new System.Drawing.Size(52, 15);
            this.labelFaxType.TabIndex = 75;
            this.labelFaxType.Text = "Fax Type";
            // 
            // ExecutionSettingGroup
            // 
            this.ExecutionSettingGroup.Controls.Add(this.RestrictRadioButton);
            this.ExecutionSettingGroup.Controls.Add(this.GenerateRadioButton);
            this.ExecutionSettingGroup.Controls.Add(this.OptionalRadioButton);
            this.ExecutionSettingGroup.Location = new System.Drawing.Point(10, 413);
            this.ExecutionSettingGroup.Name = "ExecutionSettingGroup";
            this.ExecutionSettingGroup.Size = new System.Drawing.Size(334, 53);
            this.ExecutionSettingGroup.TabIndex = 72;
            this.ExecutionSettingGroup.TabStop = false;
            this.ExecutionSettingGroup.Text = "Use Image Preview";
            // 
            // RestrictRadioButton
            // 
            this.RestrictRadioButton.AutoSize = true;
            this.RestrictRadioButton.Location = new System.Drawing.Point(216, 21);
            this.RestrictRadioButton.Name = "RestrictRadioButton";
            this.RestrictRadioButton.Size = new System.Drawing.Size(108, 19);
            this.RestrictRadioButton.TabIndex = 2;
            this.RestrictRadioButton.Text = "Restrict Preview";
            this.RestrictRadioButton.UseVisualStyleBackColor = true;
            // 
            // GenerateRadioButton
            // 
            this.GenerateRadioButton.AutoSize = true;
            this.GenerateRadioButton.Location = new System.Drawing.Point(94, 21);
            this.GenerateRadioButton.Name = "GenerateRadioButton";
            this.GenerateRadioButton.Size = new System.Drawing.Size(116, 19);
            this.GenerateRadioButton.TabIndex = 1;
            this.GenerateRadioButton.Text = "Generate Preview";
            this.GenerateRadioButton.UseVisualStyleBackColor = true;
            // 
            // OptionalRadioButton
            // 
            this.OptionalRadioButton.AutoSize = true;
            this.OptionalRadioButton.Checked = true;
            this.OptionalRadioButton.Location = new System.Drawing.Point(17, 21);
            this.OptionalRadioButton.Name = "OptionalRadioButton";
            this.OptionalRadioButton.Size = new System.Drawing.Size(71, 19);
            this.OptionalRadioButton.TabIndex = 0;
            this.OptionalRadioButton.TabStop = true;
            this.OptionalRadioButton.Text = "Optional";
            this.OptionalRadioButton.UseVisualStyleBackColor = true;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(3, 472);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(657, 207);
            this.assetSelectionControl.TabIndex = 67;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.domain_Label);
            this.panel2.Controls.Add(this.email_ComboBox);
            this.panel2.Controls.Add(this.notify_CheckBox);
            this.panel2.Controls.Add(this.page_GroupBox);
            this.panel2.Controls.Add(this.skip_GroupBox);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.panel2.Size = new System.Drawing.Size(661, 179);
            this.panel2.TabIndex = 51;
            // 
            // domain_Label
            // 
            this.domain_Label.AutoSize = true;
            this.domain_Label.Location = new System.Drawing.Point(365, 13);
            this.domain_Label.Name = "domain_Label";
            this.domain_Label.Size = new System.Drawing.Size(81, 15);
            this.domain_Label.TabIndex = 9;
            this.domain_Label.Text = "@domain.test";
            // 
            // email_ComboBox
            // 
            this.email_ComboBox.FormattingEnabled = true;
            this.email_ComboBox.Location = new System.Drawing.Point(146, 10);
            this.email_ComboBox.Name = "email_ComboBox";
            this.email_ComboBox.Size = new System.Drawing.Size(213, 23);
            this.email_ComboBox.Sorted = true;
            this.email_ComboBox.TabIndex = 8;
            // 
            // notify_CheckBox
            // 
            this.notify_CheckBox.AutoSize = true;
            this.notify_CheckBox.Location = new System.Drawing.Point(7, 12);
            this.notify_CheckBox.Name = "notify_CheckBox";
            this.notify_CheckBox.Size = new System.Drawing.Size(133, 19);
            this.notify_CheckBox.TabIndex = 5;
            this.notify_CheckBox.Text = "Email notification to";
            this.notify_CheckBox.UseVisualStyleBackColor = true;
            // 
            // page_GroupBox
            // 
            this.page_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.page_GroupBox.Controls.Add(this.pageCount_Label);
            this.page_GroupBox.Controls.Add(this.pageCount_NumericUpDown);
            this.page_GroupBox.Controls.Add(this.pageText_Label);
            this.page_GroupBox.Location = new System.Drawing.Point(3, 39);
            this.page_GroupBox.Name = "page_GroupBox";
            this.page_GroupBox.Size = new System.Drawing.Size(304, 134);
            this.page_GroupBox.TabIndex = 4;
            this.page_GroupBox.TabStop = false;
            this.page_GroupBox.Text = "Page Count";
            // 
            // pageCount_Label
            // 
            this.pageCount_Label.AutoSize = true;
            this.pageCount_Label.Location = new System.Drawing.Point(37, 82);
            this.pageCount_Label.Name = "pageCount_Label";
            this.pageCount_Label.Size = new System.Drawing.Size(69, 15);
            this.pageCount_Label.TabIndex = 13;
            this.pageCount_Label.Text = "Page Count";
            // 
            // pageCount_NumericUpDown
            // 
            this.pageCount_NumericUpDown.Location = new System.Drawing.Point(118, 80);
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
            // pageText_Label
            // 
            this.pageText_Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pageText_Label.Location = new System.Drawing.Point(7, 18);
            this.pageText_Label.Name = "pageText_Label";
            this.pageText_Label.Size = new System.Drawing.Size(290, 57);
            this.pageText_Label.TabIndex = 11;
            this.pageText_Label.Text = "Select the number of pages for the scanned document.  The page count will be the " +
    "same for all devices, whether physical or virtual.";
            // 
            // skip_GroupBox
            // 
            this.skip_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.skip_GroupBox.Controls.Add(this.lockTimeoutControl);
            this.skip_GroupBox.Controls.Add(this.skipText_Label);
            this.skip_GroupBox.Location = new System.Drawing.Point(310, 39);
            this.skip_GroupBox.Name = "skip_GroupBox";
            this.skip_GroupBox.Size = new System.Drawing.Size(348, 134);
            this.skip_GroupBox.TabIndex = 2;
            this.skip_GroupBox.TabStop = false;
            this.skip_GroupBox.Text = "Skip Delay";
            // 
            // skipText_Label
            // 
            this.skipText_Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skipText_Label.Location = new System.Drawing.Point(7, 18);
            this.skipText_Label.Name = "skipText_Label";
            this.skipText_Label.Size = new System.Drawing.Size(324, 71);
            this.skipText_Label.TabIndex = 10;
            this.skipText_Label.Text = "Exclusive access to the selected device is required to prevent other scan activit" +
    "ies from interfering. If exclusive access cannot be obtained within the time bel" +
    "ow, the activity will be skipped.\r\n";
            // 
            // loggingOptions_TabPage
            // 
            this.loggingOptions_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.loggingOptions_TabPage.Controls.Add(this.dss_GroupBox);
            this.loggingOptions_TabPage.Controls.Add(this.label7);
            this.loggingOptions_TabPage.Location = new System.Drawing.Point(4, 24);
            this.loggingOptions_TabPage.Name = "loggingOptions_TabPage";
            this.loggingOptions_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.loggingOptions_TabPage.Size = new System.Drawing.Size(674, 685);
            this.loggingOptions_TabPage.TabIndex = 1;
            this.loggingOptions_TabPage.Text = "Logging Options";
            // 
            // dss_GroupBox
            // 
            this.dss_GroupBox.Controls.Add(this.usesDigitalSendServer_CheckBox);
            this.dss_GroupBox.Controls.Add(this.digitalSendServer_TextBox);
            this.dss_GroupBox.Controls.Add(this.digitalSendServerName_Label);
            this.dss_GroupBox.Location = new System.Drawing.Point(9, 43);
            this.dss_GroupBox.Name = "dss_GroupBox";
            this.dss_GroupBox.Size = new System.Drawing.Size(317, 104);
            this.dss_GroupBox.TabIndex = 13;
            this.dss_GroupBox.TabStop = false;
            this.dss_GroupBox.Text = "Digital Send Service";
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
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(649, 15);
            this.label7.TabIndex = 2;
            this.label7.Text = "The options selected here will modify the data that is logged by this activity, b" +
    "ut will not affect the parameters of the scan.";
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(6, 80);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 11;
            // 
            // FaxConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.main_TabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FaxConfigControl";
            this.Size = new System.Drawing.Size(682, 713);
            this.main_TabControl.ResumeLayout(false);
            this.scanConfiguration_TabPage.ResumeLayout(false);
            this.jobSeparator_GroupBox.ResumeLayout(false);
            this.jobSeparator_GroupBox.PerformLayout();
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.FaxOperation_groupBox.ResumeLayout(false);
            this.FaxOperation_groupBox.PerformLayout();
            this.groupBoxFaxOperation.ResumeLayout(false);
            this.groupBoxFaxOperation.PerformLayout();
            this.ExecutionSettingGroup.ResumeLayout(false);
            this.ExecutionSettingGroup.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.page_GroupBox.ResumeLayout(false);
            this.page_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).EndInit();
            this.skip_GroupBox.ResumeLayout(false);
            this.loggingOptions_TabPage.ResumeLayout(false);
            this.loggingOptions_TabPage.PerformLayout();
            this.dss_GroupBox.ResumeLayout(false);
            this.dss_GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private HP.ScalableTest.Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.TabControl main_TabControl;
        private System.Windows.Forms.TabPage scanConfiguration_TabPage;
        private System.Windows.Forms.TabPage loggingOptions_TabPage;
        private System.Windows.Forms.GroupBox dss_GroupBox;
        private System.Windows.Forms.CheckBox usesDigitalSendServer_CheckBox;
        private System.Windows.Forms.TextBox digitalSendServer_TextBox;
        private System.Windows.Forms.Label digitalSendServerName_Label;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label domain_Label;
        private System.Windows.Forms.ComboBox email_ComboBox;
        private System.Windows.Forms.CheckBox notify_CheckBox;
        private System.Windows.Forms.GroupBox page_GroupBox;
        private System.Windows.Forms.Label pageCount_Label;
        private System.Windows.Forms.NumericUpDown pageCount_NumericUpDown;
        private System.Windows.Forms.Label pageText_Label;
        private System.Windows.Forms.GroupBox skip_GroupBox;
        private System.Windows.Forms.Label skipText_Label;
        private Framework.UI.SimulatorAssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.GroupBox ExecutionSettingGroup;
        private System.Windows.Forms.RadioButton RestrictRadioButton;
        private System.Windows.Forms.RadioButton GenerateRadioButton;
        private System.Windows.Forms.RadioButton OptionalRadioButton;
        private System.Windows.Forms.Button FaxOptions_Button;
        private System.Windows.Forms.GroupBox FaxOperation_groupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PIN_textBox;
        private System.Windows.Forms.CheckBox useSpeedDial_Checkbox;
        private System.Windows.Forms.TextBox labelFaxTimeoutHelp;
        private Framework.UI.TimeSpanControl timeSpanControlFaxReceive;
        private System.Windows.Forms.Label labelFaxTimeout;
        private System.Windows.Forms.TextBox faxNumber_textBox;
        private System.Windows.Forms.Label labelFaxNumber;
        private System.Windows.Forms.ComboBox comboBoxFaxType;
        private System.Windows.Forms.GroupBox groupBoxFaxOperation;
        private System.Windows.Forms.RadioButton radioFaxReceive;
        private System.Windows.Forms.RadioButton radioFaxSend;
        private System.Windows.Forms.Label labelFaxType;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.Windows.Forms.RadioButton radioButton_Fax;
        private System.Windows.Forms.RadioButton radioButton_SignInButton;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.Label label_AuthMethod;
        private System.Windows.Forms.GroupBox jobSeparator_GroupBox;
        private System.Windows.Forms.CheckBox jobseparator_checkBox;
        private System.Windows.Forms.Button localQueue_button;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
    }
}
