namespace HP.ScalableTest.Plugin.Copy
{
    partial class CopyConfigControl
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
            this.scanDestination_GroupBox = new System.Windows.Forms.GroupBox();
            this.quickSet_RadioButton = new System.Windows.Forms.RadioButton();
            this.copyApp_RadioButton = new System.Windows.Forms.RadioButton();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.logging_Label = new System.Windows.Forms.Label();
            this.usesDigitalSendServer_CheckBox = new System.Windows.Forms.CheckBox();
            this.digitalSendServer_TextBox = new System.Windows.Forms.TextBox();
            this.digitalSendServerName_Label = new System.Windows.Forms.Label();
            this.scanConfiguration_TabPage = new System.Windows.Forms.TabPage();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.SimulatorAssetSelectionControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.jobSeparator_GroupBox = new System.Windows.Forms.GroupBox();
            this.jobseparator_checkBox = new System.Windows.Forms.CheckBox();
            this.localQueue_button = new System.Windows.Forms.Button();
            this.groupBox_Authentication = new System.Windows.Forms.GroupBox();
            this.radioButton_Copy = new System.Windows.Forms.RadioButton();
            this.radioButton_SignInButton = new System.Windows.Forms.RadioButton();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.copyOptions_groupBox = new System.Windows.Forms.GroupBox();
            this.preferences_button = new System.Windows.Forms.Button();
            this.quickSet_GroupBox = new System.Windows.Forms.GroupBox();
            this.quickset_Label = new System.Windows.Forms.Label();
            this.quickSet_TextBox = new System.Windows.Forms.TextBox();
            this.launchFromApp_RadioButton = new System.Windows.Forms.RadioButton();
            this.launchFromHome_RadioButton = new System.Windows.Forms.RadioButton();
            this.digitalSendService_GroupBox = new System.Windows.Forms.GroupBox();
            this.loggingOptions_TabPage = new System.Windows.Forms.TabPage();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.skipDelay_GroupBox = new System.Windows.Forms.GroupBox();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.skipDelayMessage_Label = new System.Windows.Forms.Label();
            this.pageCount_GroupBox = new System.Windows.Forms.GroupBox();
            this.jobBuild_Label = new System.Windows.Forms.Label();
            this.pageCount_NumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.pageCountMessage_Label = new System.Windows.Forms.Label();
            this.note_Label = new System.Windows.Forms.Label();
            this.scanDestination_GroupBox.SuspendLayout();
            this.scanConfiguration_TabPage.SuspendLayout();
            this.panel1.SuspendLayout();
            this.jobSeparator_GroupBox.SuspendLayout();
            this.groupBox_Authentication.SuspendLayout();
            this.copyOptions_groupBox.SuspendLayout();
            this.quickSet_GroupBox.SuspendLayout();
            this.digitalSendService_GroupBox.SuspendLayout();
            this.loggingOptions_TabPage.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.skipDelay_GroupBox.SuspendLayout();
            this.pageCount_GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // scanDestination_GroupBox
            // 
            this.scanDestination_GroupBox.Controls.Add(this.quickSet_RadioButton);
            this.scanDestination_GroupBox.Controls.Add(this.copyApp_RadioButton);
            this.scanDestination_GroupBox.Location = new System.Drawing.Point(10, 5);
            this.scanDestination_GroupBox.Name = "scanDestination_GroupBox";
            this.scanDestination_GroupBox.Size = new System.Drawing.Size(355, 48);
            this.scanDestination_GroupBox.TabIndex = 49;
            this.scanDestination_GroupBox.TabStop = false;
            this.scanDestination_GroupBox.Text = "Scan Destination";
            // 
            // quickSet_RadioButton
            // 
            this.quickSet_RadioButton.AutoSize = true;
            this.quickSet_RadioButton.Location = new System.Drawing.Point(267, 20);
            this.quickSet_RadioButton.Name = "quickSet_RadioButton";
            this.quickSet_RadioButton.Size = new System.Drawing.Size(67, 17);
            this.quickSet_RadioButton.TabIndex = 1;
            this.quickSet_RadioButton.Text = "Quickset";
            this.quickSet_RadioButton.UseVisualStyleBackColor = true;
            // 
            // copyApp_RadioButton
            // 
            this.copyApp_RadioButton.AutoSize = true;
            this.copyApp_RadioButton.Checked = true;
            this.copyApp_RadioButton.Location = new System.Drawing.Point(15, 20);
            this.copyApp_RadioButton.Name = "copyApp_RadioButton";
            this.copyApp_RadioButton.Size = new System.Drawing.Size(73, 17);
            this.copyApp_RadioButton.TabIndex = 0;
            this.copyApp_RadioButton.TabStop = true;
            this.copyApp_RadioButton.Text = "Copy APP";
            this.copyApp_RadioButton.UseVisualStyleBackColor = true;
            // 
            // logging_Label
            // 
            this.logging_Label.AutoSize = true;
            this.logging_Label.Location = new System.Drawing.Point(6, 13);
            this.logging_Label.Name = "logging_Label";
            this.logging_Label.Size = new System.Drawing.Size(575, 13);
            this.logging_Label.TabIndex = 0;
            this.logging_Label.Text = "The options selected here will modify the data that is logged by this activity, b" +
    "ut will not affect the parameters of the scan.";
            // 
            // usesDigitalSendServer_CheckBox
            // 
            this.usesDigitalSendServer_CheckBox.AutoSize = true;
            this.usesDigitalSendServer_CheckBox.Checked = true;
            this.usesDigitalSendServer_CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.usesDigitalSendServer_CheckBox.Location = new System.Drawing.Point(21, 27);
            this.usesDigitalSendServer_CheckBox.Name = "usesDigitalSendServer_CheckBox";
            this.usesDigitalSendServer_CheckBox.Size = new System.Drawing.Size(227, 17);
            this.usesDigitalSendServer_CheckBox.TabIndex = 2;
            this.usesDigitalSendServer_CheckBox.Text = "Activity will use Digital Send Service (DSS)";
            this.usesDigitalSendServer_CheckBox.UseVisualStyleBackColor = true;
            // 
            // digitalSendServer_TextBox
            // 
            this.digitalSendServer_TextBox.Location = new System.Drawing.Point(98, 62);
            this.digitalSendServer_TextBox.Name = "digitalSendServer_TextBox";
            this.digitalSendServer_TextBox.Size = new System.Drawing.Size(202, 20);
            this.digitalSendServer_TextBox.TabIndex = 1;
            // 
            // digitalSendServerName_Label
            // 
            this.digitalSendServerName_Label.AutoSize = true;
            this.digitalSendServerName_Label.Location = new System.Drawing.Point(18, 65);
            this.digitalSendServerName_Label.Name = "digitalSendServerName_Label";
            this.digitalSendServerName_Label.Size = new System.Drawing.Size(69, 13);
            this.digitalSendServerName_Label.TabIndex = 0;
            this.digitalSendServerName_Label.Text = "Server Name";
            // 
            // scanConfiguration_TabPage
            // 
            this.scanConfiguration_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.scanConfiguration_TabPage.Controls.Add(this.assetSelectionControl);
            this.scanConfiguration_TabPage.Controls.Add(this.panel1);
            this.scanConfiguration_TabPage.Location = new System.Drawing.Point(4, 22);
            this.scanConfiguration_TabPage.Name = "scanConfiguration_TabPage";
            this.scanConfiguration_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.scanConfiguration_TabPage.Size = new System.Drawing.Size(667, 510);
            this.scanConfiguration_TabPage.TabIndex = 0;
            this.scanConfiguration_TabPage.Text = "Scan Configuration";
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(3, 362);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(661, 145);
            this.assetSelectionControl.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.skipDelay_GroupBox);
            this.panel1.Controls.Add(this.pageCount_GroupBox);
            this.panel1.Controls.Add(this.jobSeparator_GroupBox);
            this.panel1.Controls.Add(this.groupBox_Authentication);
            this.panel1.Controls.Add(this.copyOptions_groupBox);
            this.panel1.Controls.Add(this.scanDestination_GroupBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(661, 359);
            this.panel1.TabIndex = 0;
            // 
            // jobSeparator_GroupBox
            // 
            this.jobSeparator_GroupBox.Controls.Add(this.jobseparator_checkBox);
            this.jobSeparator_GroupBox.Controls.Add(this.localQueue_button);
            this.jobSeparator_GroupBox.Location = new System.Drawing.Point(371, 5);
            this.jobSeparator_GroupBox.Name = "jobSeparator_GroupBox";
            this.jobSeparator_GroupBox.Size = new System.Drawing.Size(280, 48);
            this.jobSeparator_GroupBox.TabIndex = 2;
            this.jobSeparator_GroupBox.TabStop = false;
            this.jobSeparator_GroupBox.Text = "Job Separator Configuration";
            // 
            // jobseparator_checkBox
            // 
            this.jobseparator_checkBox.AutoSize = true;
            this.jobseparator_checkBox.Location = new System.Drawing.Point(145, 19);
            this.jobseparator_checkBox.Name = "jobseparator_checkBox";
            this.jobseparator_checkBox.Size = new System.Drawing.Size(111, 17);
            this.jobseparator_checkBox.TabIndex = 56;
            this.jobseparator_checkBox.Text = "Print job separator";
            this.jobseparator_checkBox.UseVisualStyleBackColor = true;
            // 
            // localQueue_button
            // 
            this.localQueue_button.Location = new System.Drawing.Point(21, 16);
            this.localQueue_button.Name = "localQueue_button";
            this.localQueue_button.Size = new System.Drawing.Size(97, 25);
            this.localQueue_button.TabIndex = 19;
            this.localQueue_button.Text = "Local Queue";
            this.localQueue_button.UseVisualStyleBackColor = true;
            this.localQueue_button.Click += new System.EventHandler(this.localQueue_button_Click);
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Controls.Add(this.radioButton_Copy);
            this.groupBox_Authentication.Controls.Add(this.radioButton_SignInButton);
            this.groupBox_Authentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBox_Authentication.Controls.Add(this.label_AuthMethod);
            this.groupBox_Authentication.Location = new System.Drawing.Point(5, 156);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(646, 49);
            this.groupBox_Authentication.TabIndex = 94;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication";
            // 
            // radioButton_Copy
            // 
            this.radioButton_Copy.AutoSize = true;
            this.radioButton_Copy.Checked = true;
            this.radioButton_Copy.Location = new System.Drawing.Point(116, 20);
            this.radioButton_Copy.Name = "radioButton_Copy";
            this.radioButton_Copy.Size = new System.Drawing.Size(49, 17);
            this.radioButton_Copy.TabIndex = 93;
            this.radioButton_Copy.TabStop = true;
            this.radioButton_Copy.Text = "Copy";
            this.radioButton_Copy.UseVisualStyleBackColor = true;
            // 
            // radioButton_SignInButton
            // 
            this.radioButton_SignInButton.AutoSize = true;
            this.radioButton_SignInButton.Location = new System.Drawing.Point(18, 20);
            this.radioButton_SignInButton.Name = "radioButton_SignInButton";
            this.radioButton_SignInButton.Size = new System.Drawing.Size(58, 17);
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
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(404, 17);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(170, 21);
            this.comboBox_AuthProvider.TabIndex = 90;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(264, 22);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(114, 13);
            this.label_AuthMethod.TabIndex = 91;
            this.label_AuthMethod.Text = "Authentication Method";
            // 
            // copyOptions_groupBox
            // 
            this.copyOptions_groupBox.Controls.Add(this.preferences_button);
            this.copyOptions_groupBox.Controls.Add(this.quickSet_GroupBox);
            this.copyOptions_groupBox.Location = new System.Drawing.Point(6, 59);
            this.copyOptions_groupBox.Name = "copyOptions_groupBox";
            this.copyOptions_groupBox.Size = new System.Drawing.Size(645, 97);
            this.copyOptions_groupBox.TabIndex = 51;
            this.copyOptions_groupBox.TabStop = false;
            this.copyOptions_groupBox.Text = "Options";
            // 
            // preferences_button
            // 
            this.preferences_button.Location = new System.Drawing.Point(9, 19);
            this.preferences_button.Name = "preferences_button";
            this.preferences_button.Size = new System.Drawing.Size(103, 34);
            this.preferences_button.TabIndex = 16;
            this.preferences_button.Text = "Copy Options";
            this.preferences_button.UseVisualStyleBackColor = true;
            this.preferences_button.Click += new System.EventHandler(this.preferences_button_Click);
            // 
            // quickSet_GroupBox
            // 
            this.quickSet_GroupBox.Controls.Add(this.quickset_Label);
            this.quickSet_GroupBox.Controls.Add(this.quickSet_TextBox);
            this.quickSet_GroupBox.Controls.Add(this.launchFromApp_RadioButton);
            this.quickSet_GroupBox.Controls.Add(this.launchFromHome_RadioButton);
            this.quickSet_GroupBox.Location = new System.Drawing.Point(151, 11);
            this.quickSet_GroupBox.Name = "quickSet_GroupBox";
            this.quickSet_GroupBox.Size = new System.Drawing.Size(488, 80);
            this.quickSet_GroupBox.TabIndex = 18;
            this.quickSet_GroupBox.TabStop = false;
            this.quickSet_GroupBox.Text = "Quickset Options";
            // 
            // quickset_Label
            // 
            this.quickset_Label.AutoSize = true;
            this.quickset_Label.Location = new System.Drawing.Point(6, 16);
            this.quickset_Label.Name = "quickset_Label";
            this.quickset_Label.Size = new System.Drawing.Size(86, 13);
            this.quickset_Label.TabIndex = 55;
            this.quickset_Label.Text = "Named Quickset";
            // 
            // quickSet_TextBox
            // 
            this.quickSet_TextBox.Location = new System.Drawing.Point(98, 13);
            this.quickSet_TextBox.Name = "quickSet_TextBox";
            this.quickSet_TextBox.Size = new System.Drawing.Size(314, 20);
            this.quickSet_TextBox.TabIndex = 54;
            // 
            // launchFromApp_RadioButton
            // 
            this.launchFromApp_RadioButton.AutoSize = true;
            this.launchFromApp_RadioButton.Checked = true;
            this.launchFromApp_RadioButton.Location = new System.Drawing.Point(6, 37);
            this.launchFromApp_RadioButton.Name = "launchFromApp_RadioButton";
            this.launchFromApp_RadioButton.Size = new System.Drawing.Size(202, 17);
            this.launchFromApp_RadioButton.TabIndex = 1;
            this.launchFromApp_RadioButton.TabStop = true;
            this.launchFromApp_RadioButton.Text = "Launch Copy App then load Quickset";
            this.launchFromApp_RadioButton.UseVisualStyleBackColor = true;
            // 
            // launchFromHome_RadioButton
            // 
            this.launchFromHome_RadioButton.AutoSize = true;
            this.launchFromHome_RadioButton.Location = new System.Drawing.Point(6, 59);
            this.launchFromHome_RadioButton.Name = "launchFromHome_RadioButton";
            this.launchFromHome_RadioButton.Size = new System.Drawing.Size(243, 17);
            this.launchFromHome_RadioButton.TabIndex = 0;
            this.launchFromHome_RadioButton.Text = "Launch from Quick Set button on homescreen";
            this.launchFromHome_RadioButton.UseVisualStyleBackColor = true;
            // 
            // digitalSendService_GroupBox
            // 
            this.digitalSendService_GroupBox.Controls.Add(this.usesDigitalSendServer_CheckBox);
            this.digitalSendService_GroupBox.Controls.Add(this.digitalSendServer_TextBox);
            this.digitalSendService_GroupBox.Controls.Add(this.digitalSendServerName_Label);
            this.digitalSendService_GroupBox.Location = new System.Drawing.Point(9, 46);
            this.digitalSendService_GroupBox.Name = "digitalSendService_GroupBox";
            this.digitalSendService_GroupBox.Size = new System.Drawing.Size(317, 104);
            this.digitalSendService_GroupBox.TabIndex = 11;
            this.digitalSendService_GroupBox.TabStop = false;
            this.digitalSendService_GroupBox.Text = "Digital Send Service";
            // 
            // loggingOptions_TabPage
            // 
            this.loggingOptions_TabPage.BackColor = System.Drawing.SystemColors.Control;
            this.loggingOptions_TabPage.Controls.Add(this.digitalSendService_GroupBox);
            this.loggingOptions_TabPage.Controls.Add(this.logging_Label);
            this.loggingOptions_TabPage.Location = new System.Drawing.Point(4, 22);
            this.loggingOptions_TabPage.Name = "loggingOptions_TabPage";
            this.loggingOptions_TabPage.Padding = new System.Windows.Forms.Padding(3);
            this.loggingOptions_TabPage.Size = new System.Drawing.Size(667, 510);
            this.loggingOptions_TabPage.TabIndex = 1;
            this.loggingOptions_TabPage.Text = "Logging Options";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.scanConfiguration_TabPage);
            this.tabControl.Controls.Add(this.loggingOptions_TabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(675, 536);
            this.tabControl.TabIndex = 49;
            // 
            // skipDelay_GroupBox
            // 
            this.skipDelay_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.skipDelay_GroupBox.Controls.Add(this.lockTimeoutControl);
            this.skipDelay_GroupBox.Controls.Add(this.skipDelayMessage_Label);
            this.skipDelay_GroupBox.Location = new System.Drawing.Point(317, 217);
            this.skipDelay_GroupBox.Name = "skipDelay_GroupBox";
            this.skipDelay_GroupBox.Size = new System.Drawing.Size(334, 125);
            this.skipDelay_GroupBox.TabIndex = 96;
            this.skipDelay_GroupBox.TabStop = false;
            this.skipDelay_GroupBox.Text = "Skip Delay";
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(6, 69);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 11;
            // 
            // skipDelayMessage_Label
            // 
            this.skipDelayMessage_Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skipDelayMessage_Label.Location = new System.Drawing.Point(7, 18);
            this.skipDelayMessage_Label.Name = "skipDelayMessage_Label";
            this.skipDelayMessage_Label.Size = new System.Drawing.Size(320, 57);
            this.skipDelayMessage_Label.TabIndex = 10;
            this.skipDelayMessage_Label.Text = "Exclusive access to the selected device is required to prevent other scan activit" +
    "ies from interfering. If exclusive access cannot be obtained within the time bel" +
    "ow, the activity will be skipped.\r\n";
            // 
            // pageCount_GroupBox
            // 
            this.pageCount_GroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pageCount_GroupBox.Controls.Add(this.note_Label);
            this.pageCount_GroupBox.Controls.Add(this.jobBuild_Label);
            this.pageCount_GroupBox.Controls.Add(this.pageCount_NumericUpDown);
            this.pageCount_GroupBox.Controls.Add(this.pageCountMessage_Label);
            this.pageCount_GroupBox.Location = new System.Drawing.Point(6, 217);
            this.pageCount_GroupBox.Name = "pageCount_GroupBox";
            this.pageCount_GroupBox.Size = new System.Drawing.Size(296, 122);
            this.pageCount_GroupBox.TabIndex = 95;
            this.pageCount_GroupBox.TabStop = false;
            this.pageCount_GroupBox.Text = "Page Count";
            // 
            // jobBuild_Label
            // 
            this.jobBuild_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.jobBuild_Label.AutoSize = true;
            this.jobBuild_Label.Location = new System.Drawing.Point(7, 75);
            this.jobBuild_Label.Name = "jobBuild_Label";
            this.jobBuild_Label.Size = new System.Drawing.Size(109, 13);
            this.jobBuild_Label.TabIndex = 13;
            this.jobBuild_Label.Text = "Job Build Page Count";
            // 
            // pageCount_NumericUpDown
            // 
            this.pageCount_NumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pageCount_NumericUpDown.Location = new System.Drawing.Point(127, 75);
            this.pageCount_NumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pageCount_NumericUpDown.Name = "pageCount_NumericUpDown";
            this.pageCount_NumericUpDown.Size = new System.Drawing.Size(107, 20);
            this.pageCount_NumericUpDown.TabIndex = 12;
            this.pageCount_NumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // pageCountMessage_Label
            // 
            this.pageCountMessage_Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pageCountMessage_Label.Location = new System.Drawing.Point(7, 18);
            this.pageCountMessage_Label.Name = "pageCountMessage_Label";
            this.pageCountMessage_Label.Size = new System.Drawing.Size(282, 57);
            this.pageCountMessage_Label.TabIndex = 11;
            this.pageCountMessage_Label.Text = "Select the number of pages for the scanned document.  The page count will be the " +
    "same for all devices, whether physical or virtual.";
            // 
            // note_Label
            // 
            this.note_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.note_Label.AutoSize = true;
            this.note_Label.Location = new System.Drawing.Point(7, 97);
            this.note_Label.Name = "note_Label";
            this.note_Label.Size = new System.Drawing.Size(203, 13);
            this.note_Label.TabIndex = 14;
            this.note_Label.Text = "Note : Page Count >1 to enable Job Build";
            // 
            // CopyConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "CopyConfigControl";
            this.Size = new System.Drawing.Size(675, 536);
            this.scanDestination_GroupBox.ResumeLayout(false);
            this.scanDestination_GroupBox.PerformLayout();
            this.scanConfiguration_TabPage.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.jobSeparator_GroupBox.ResumeLayout(false);
            this.jobSeparator_GroupBox.PerformLayout();
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.copyOptions_groupBox.ResumeLayout(false);
            this.quickSet_GroupBox.ResumeLayout(false);
            this.quickSet_GroupBox.PerformLayout();
            this.digitalSendService_GroupBox.ResumeLayout(false);
            this.digitalSendService_GroupBox.PerformLayout();
            this.loggingOptions_TabPage.ResumeLayout(false);
            this.loggingOptions_TabPage.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.skipDelay_GroupBox.ResumeLayout(false);
            this.pageCount_GroupBox.ResumeLayout(false);
            this.pageCount_GroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageCount_NumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox scanDestination_GroupBox;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.Label logging_Label;
        private System.Windows.Forms.CheckBox usesDigitalSendServer_CheckBox;
        private System.Windows.Forms.TextBox digitalSendServer_TextBox;
        private System.Windows.Forms.Label digitalSendServerName_Label;
        private System.Windows.Forms.TabPage scanConfiguration_TabPage;
        private Framework.UI.SimulatorAssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox digitalSendService_GroupBox;
        private System.Windows.Forms.TabPage loggingOptions_TabPage;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.RadioButton quickSet_RadioButton;
        private System.Windows.Forms.RadioButton copyApp_RadioButton;
        private System.Windows.Forms.GroupBox quickSet_GroupBox;
        private System.Windows.Forms.Button preferences_button;
        private System.Windows.Forms.Label quickset_Label;
        private System.Windows.Forms.TextBox quickSet_TextBox;
        private System.Windows.Forms.RadioButton launchFromApp_RadioButton;
        private System.Windows.Forms.RadioButton launchFromHome_RadioButton;
        private System.Windows.Forms.GroupBox copyOptions_groupBox;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.Windows.Forms.RadioButton radioButton_Copy;
        private System.Windows.Forms.RadioButton radioButton_SignInButton;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.Label label_AuthMethod;
        private System.Windows.Forms.Button localQueue_button;
        private System.Windows.Forms.GroupBox jobSeparator_GroupBox;
        private System.Windows.Forms.CheckBox jobseparator_checkBox;
        private System.Windows.Forms.GroupBox skipDelay_GroupBox;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private System.Windows.Forms.Label skipDelayMessage_Label;
        private System.Windows.Forms.GroupBox pageCount_GroupBox;
        private System.Windows.Forms.Label note_Label;
        private System.Windows.Forms.Label jobBuild_Label;
        private System.Windows.Forms.NumericUpDown pageCount_NumericUpDown;
        private System.Windows.Forms.Label pageCountMessage_Label;
    }
}
