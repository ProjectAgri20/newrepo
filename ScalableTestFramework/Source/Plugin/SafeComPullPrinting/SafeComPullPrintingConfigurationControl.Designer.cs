﻿namespace HP.ScalableTest.Plugin.SafeComPullPrinting
{
    partial class SafeComPullPrintingConfigurationControl
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
            this.tabControlSafeCom = new System.Windows.Forms.TabControl();
            this.tabPageSafeCom = new System.Windows.Forms.TabPage();
            this.groupBoxMemoryPofile = new System.Windows.Forms.GroupBox();
            this.groupBoxSafeComConfiguration = new System.Windows.Forms.GroupBox();
            this.radioButton_PrintAll = new System.Windows.Forms.RadioButton();
            this.checkBox_SelectAll = new System.Windows.Forms.CheckBox();
            this.label_Copies = new System.Windows.Forms.Label();
            this.numericUpDown_Copies = new System.Windows.Forms.NumericUpDown();
            this.radioButton_Delete = new System.Windows.Forms.RadioButton();
            this.radioButton_Retain = new System.Windows.Forms.RadioButton();
            this.radioButton_Print = new System.Windows.Forms.RadioButton();
            this.groupBoxAuthentication = new System.Windows.Forms.GroupBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.radioButton_SafeCom = new System.Windows.Forms.RadioButton();
            this.radioButton_SignInButton = new System.Windows.Forms.RadioButton();
            this.groupBoxDeviceConfiguration = new System.Windows.Forms.GroupBox();
            this.checkBoxReleaseOnSignIn = new System.Windows.Forms.CheckBox();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.tabPagePrinting = new System.Windows.Forms.TabPage();
            this.printingTaskConfigurationControl = new HP.ScalableTest.PluginSupport.PullPrint.PrintingTabConfigControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.deviceMemoryProfilerControl = new HP.ScalableTest.PluginSupport.MemoryCollection.DeviceMemoryProfilerControl();
            this.tabControlSafeCom.SuspendLayout();
            this.tabPageSafeCom.SuspendLayout();
            this.groupBoxMemoryPofile.SuspendLayout();
            this.groupBoxSafeComConfiguration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Copies)).BeginInit();
            this.groupBoxAuthentication.SuspendLayout();
            this.groupBoxDeviceConfiguration.SuspendLayout();
            this.tabPagePrinting.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlSafeCom
            // 
            this.tabControlSafeCom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlSafeCom.Controls.Add(this.tabPageSafeCom);
            this.tabControlSafeCom.Controls.Add(this.tabPagePrinting);
            this.tabControlSafeCom.Location = new System.Drawing.Point(5, 5);
            this.tabControlSafeCom.Name = "tabControlSafeCom";
            this.tabControlSafeCom.SelectedIndex = 0;
            this.tabControlSafeCom.Size = new System.Drawing.Size(685, 560);
            this.tabControlSafeCom.TabIndex = 0;
            // 
            // tabPageSafeCom
            // 
            this.tabPageSafeCom.Controls.Add(this.groupBoxMemoryPofile);
            this.tabPageSafeCom.Controls.Add(this.groupBoxSafeComConfiguration);
            this.tabPageSafeCom.Controls.Add(this.groupBoxAuthentication);
            this.tabPageSafeCom.Controls.Add(this.groupBoxDeviceConfiguration);
            this.tabPageSafeCom.Location = new System.Drawing.Point(4, 22);
            this.tabPageSafeCom.Name = "tabPageSafeCom";
            this.tabPageSafeCom.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSafeCom.Size = new System.Drawing.Size(677, 534);
            this.tabPageSafeCom.TabIndex = 0;
            this.tabPageSafeCom.Text = "Pull Printing";
            this.tabPageSafeCom.UseVisualStyleBackColor = true;
            // 
            // groupBoxMemoryPofile
            // 
            this.groupBoxMemoryPofile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMemoryPofile.Controls.Add(this.deviceMemoryProfilerControl);
            this.groupBoxMemoryPofile.Location = new System.Drawing.Point(5, 409);
            this.groupBoxMemoryPofile.Name = "groupBoxMemoryPofile";
            this.groupBoxMemoryPofile.Size = new System.Drawing.Size(667, 105);
            this.groupBoxMemoryPofile.TabIndex = 4;
            this.groupBoxMemoryPofile.TabStop = false;
            this.groupBoxMemoryPofile.Text = "Capture Device Memory Profile";
            // 
            // groupBoxSafeComConfiguration
            // 
            this.groupBoxSafeComConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSafeComConfiguration.Controls.Add(this.radioButton_PrintAll);
            this.groupBoxSafeComConfiguration.Controls.Add(this.checkBox_SelectAll);
            this.groupBoxSafeComConfiguration.Controls.Add(this.label_Copies);
            this.groupBoxSafeComConfiguration.Controls.Add(this.numericUpDown_Copies);
            this.groupBoxSafeComConfiguration.Controls.Add(this.radioButton_Delete);
            this.groupBoxSafeComConfiguration.Controls.Add(this.radioButton_Retain);
            this.groupBoxSafeComConfiguration.Controls.Add(this.radioButton_Print);
            this.groupBoxSafeComConfiguration.Location = new System.Drawing.Point(5, 320);
            this.groupBoxSafeComConfiguration.Name = "groupBoxSafeComConfiguration";
            this.groupBoxSafeComConfiguration.Size = new System.Drawing.Size(666, 80);
            this.groupBoxSafeComConfiguration.TabIndex = 3;
            this.groupBoxSafeComConfiguration.TabStop = false;
            this.groupBoxSafeComConfiguration.Text = "Pull Print Configuration";
            // 
            // radioButton_PrintAll
            // 
            this.radioButton_PrintAll.AutoSize = true;
            this.radioButton_PrintAll.Location = new System.Drawing.Point(264, 47);
            this.radioButton_PrintAll.Name = "radioButton_PrintAll";
            this.radioButton_PrintAll.Size = new System.Drawing.Size(59, 17);
            this.radioButton_PrintAll.TabIndex = 8;
            this.radioButton_PrintAll.Text = "Print all";
            this.radioButton_PrintAll.UseVisualStyleBackColor = true;
            this.radioButton_PrintAll.CheckedChanged += new System.EventHandler(this.RadioButton_Action_CheckedChanged);
            // 
            // checkBox_SelectAll
            // 
            this.checkBox_SelectAll.AutoSize = true;
            this.checkBox_SelectAll.Location = new System.Drawing.Point(17, 19);
            this.checkBox_SelectAll.Name = "checkBox_SelectAll";
            this.checkBox_SelectAll.Size = new System.Drawing.Size(70, 17);
            this.checkBox_SelectAll.TabIndex = 7;
            this.checkBox_SelectAll.Text = "Select All";
            this.checkBox_SelectAll.UseVisualStyleBackColor = true;
            // 
            // label_Copies
            // 
            this.label_Copies.AutoSize = true;
            this.label_Copies.Location = new System.Drawing.Point(359, 49);
            this.label_Copies.Name = "label_Copies";
            this.label_Copies.Size = new System.Drawing.Size(93, 13);
            this.label_Copies.TabIndex = 6;
            this.label_Copies.Text = "Number of copies:";
            // 
            // numericUpDown_Copies
            // 
            this.numericUpDown_Copies.Location = new System.Drawing.Point(464, 47);
            this.numericUpDown_Copies.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_Copies.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_Copies.Name = "numericUpDown_Copies";
            this.numericUpDown_Copies.Size = new System.Drawing.Size(50, 20);
            this.numericUpDown_Copies.TabIndex = 5;
            this.numericUpDown_Copies.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // radioButton_Delete
            // 
            this.radioButton_Delete.AutoSize = true;
            this.radioButton_Delete.Location = new System.Drawing.Point(179, 47);
            this.radioButton_Delete.Name = "radioButton_Delete";
            this.radioButton_Delete.Size = new System.Drawing.Size(56, 17);
            this.radioButton_Delete.TabIndex = 3;
            this.radioButton_Delete.Text = "Delete";
            this.radioButton_Delete.UseVisualStyleBackColor = true;
            this.radioButton_Delete.CheckedChanged += new System.EventHandler(this.RadioButton_Action_CheckedChanged);
            // 
            // radioButton_Retain
            // 
            this.radioButton_Retain.AutoSize = true;
            this.radioButton_Retain.Location = new System.Drawing.Point(92, 47);
            this.radioButton_Retain.Name = "radioButton_Retain";
            this.radioButton_Retain.Size = new System.Drawing.Size(56, 17);
            this.radioButton_Retain.TabIndex = 2;
            this.radioButton_Retain.Text = "Retain";
            this.radioButton_Retain.UseVisualStyleBackColor = true;
            this.radioButton_Retain.CheckedChanged += new System.EventHandler(this.RadioButton_Action_CheckedChanged);
            // 
            // radioButton_Print
            // 
            this.radioButton_Print.AutoSize = true;
            this.radioButton_Print.Checked = true;
            this.radioButton_Print.Location = new System.Drawing.Point(17, 47);
            this.radioButton_Print.Name = "radioButton_Print";
            this.radioButton_Print.Size = new System.Drawing.Size(46, 17);
            this.radioButton_Print.TabIndex = 1;
            this.radioButton_Print.TabStop = true;
            this.radioButton_Print.Text = "Print";
            this.radioButton_Print.UseVisualStyleBackColor = true;
            this.radioButton_Print.CheckedChanged += new System.EventHandler(this.RadioButton_Action_CheckedChanged);
            // 
            // groupBoxAuthentication
            // 
            this.groupBoxAuthentication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAuthentication.Controls.Add(this.label_AuthMethod);
            this.groupBoxAuthentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBoxAuthentication.Controls.Add(this.radioButton_SafeCom);
            this.groupBoxAuthentication.Controls.Add(this.radioButton_SignInButton);
            this.groupBoxAuthentication.Location = new System.Drawing.Point(5, 217);
            this.groupBoxAuthentication.Name = "groupBoxAuthentication";
            this.groupBoxAuthentication.Size = new System.Drawing.Size(666, 100);
            this.groupBoxAuthentication.TabIndex = 2;
            this.groupBoxAuthentication.TabStop = false;
            this.groupBoxAuthentication.Text = "Authentication Configuration";
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(17, 64);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(114, 13);
            this.label_AuthMethod.TabIndex = 23;
            this.label_AuthMethod.Text = "Authentication Method";
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(160, 61);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(170, 21);
            this.comboBox_AuthProvider.TabIndex = 3;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // radioButton_SafeCom
            // 
            this.radioButton_SafeCom.AutoSize = true;
            this.radioButton_SafeCom.Location = new System.Drawing.Point(160, 33);
            this.radioButton_SafeCom.Name = "radioButton_SafeCom";
            this.radioButton_SafeCom.Size = new System.Drawing.Size(112, 17);
            this.radioButton_SafeCom.TabIndex = 1;
            this.radioButton_SafeCom.Text = "SafeCom Pull Print";
            this.radioButton_SafeCom.UseVisualStyleBackColor = true;
            // 
            // radioButton_SignInButton
            // 
            this.radioButton_SignInButton.AutoSize = true;
            this.radioButton_SignInButton.Checked = true;
            this.radioButton_SignInButton.Location = new System.Drawing.Point(20, 33);
            this.radioButton_SignInButton.Name = "radioButton_SignInButton";
            this.radioButton_SignInButton.Size = new System.Drawing.Size(92, 17);
            this.radioButton_SignInButton.TabIndex = 0;
            this.radioButton_SignInButton.TabStop = true;
            this.radioButton_SignInButton.Text = "Sign In Button";
            this.radioButton_SignInButton.UseVisualStyleBackColor = true;
            // 
            // groupBoxDeviceConfiguration
            // 
            this.groupBoxDeviceConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDeviceConfiguration.Controls.Add(this.checkBoxReleaseOnSignIn);
            this.groupBoxDeviceConfiguration.Controls.Add(this.lockTimeoutControl);
            this.groupBoxDeviceConfiguration.Controls.Add(this.assetSelectionControl);
            this.groupBoxDeviceConfiguration.Location = new System.Drawing.Point(5, 6);
            this.groupBoxDeviceConfiguration.Name = "groupBoxDeviceConfiguration";
            this.groupBoxDeviceConfiguration.Size = new System.Drawing.Size(666, 212);
            this.groupBoxDeviceConfiguration.TabIndex = 1;
            this.groupBoxDeviceConfiguration.TabStop = false;
            this.groupBoxDeviceConfiguration.Text = "Device Configuration";
            // 
            // checkBoxReleaseOnSignIn
            // 
            this.checkBoxReleaseOnSignIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxReleaseOnSignIn.AutoSize = true;
            this.checkBoxReleaseOnSignIn.Location = new System.Drawing.Point(316, 172);
            this.checkBoxReleaseOnSignIn.Name = "checkBoxReleaseOnSignIn";
            this.checkBoxReleaseOnSignIn.Size = new System.Drawing.Size(275, 17);
            this.checkBoxReleaseOnSignIn.TabIndex = 2;
            this.checkBoxReleaseOnSignIn.Text = "Device is configured to release documents on sign in";
            this.checkBoxReleaseOnSignIn.UseVisualStyleBackColor = true;
            this.checkBoxReleaseOnSignIn.CheckedChanged += new System.EventHandler(this.checkBoxReleaseOnSignIn_CheckedChanged);
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(7, 154);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 1;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(6, 19);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(654, 129);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // tabPagePrinting
            // 
            this.tabPagePrinting.Controls.Add(this.printingTaskConfigurationControl);
            this.tabPagePrinting.Location = new System.Drawing.Point(4, 22);
            this.tabPagePrinting.Name = "tabPagePrinting";
            this.tabPagePrinting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePrinting.Size = new System.Drawing.Size(677, 534);
            this.tabPagePrinting.TabIndex = 1;
            this.tabPagePrinting.Text = "Printing";
            this.tabPagePrinting.UseVisualStyleBackColor = true;
            // 
            // printingTaskConfigurationControl
            // 
            this.printingTaskConfigurationControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printingTaskConfigurationControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printingTaskConfigurationControl.Location = new System.Drawing.Point(3, 3);
            this.printingTaskConfigurationControl.Name = "printingTaskConfigurationControl";
            this.printingTaskConfigurationControl.Size = new System.Drawing.Size(671, 528);
            this.printingTaskConfigurationControl.TabIndex = 1;
            // 
            // deviceMemoryProfilerControl
            // 
            this.deviceMemoryProfilerControl.Location = new System.Drawing.Point(7, 20);
            this.deviceMemoryProfilerControl.Name = "deviceMemoryProfilerControl";
            this.deviceMemoryProfilerControl.SelectedData.SampleAtCountIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.SampleAtTimeIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleCount = 0;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleTime = System.TimeSpan.Parse("00:00:00");
            this.deviceMemoryProfilerControl.Size = new System.Drawing.Size(282, 84);
            this.deviceMemoryProfilerControl.TabIndex = 0;
            // 
            // SafeComPullPrintingConfigurationControl
            // 
            this.Controls.Add(this.tabControlSafeCom);
            this.Name = "SafeComPullPrintingConfigurationControl";
            this.Size = new System.Drawing.Size(698, 574);
            this.tabControlSafeCom.ResumeLayout(false);
            this.tabPageSafeCom.ResumeLayout(false);
            this.groupBoxMemoryPofile.ResumeLayout(false);
            this.groupBoxSafeComConfiguration.ResumeLayout(false);
            this.groupBoxSafeComConfiguration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Copies)).EndInit();
            this.groupBoxAuthentication.ResumeLayout(false);
            this.groupBoxAuthentication.PerformLayout();
            this.groupBoxDeviceConfiguration.ResumeLayout(false);
            this.groupBoxDeviceConfiguration.PerformLayout();
            this.tabPagePrinting.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlSafeCom;
        private System.Windows.Forms.TabPage tabPageSafeCom;
        private System.Windows.Forms.TabPage tabPagePrinting;
        private System.Windows.Forms.GroupBox groupBoxDeviceConfiguration;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.GroupBox groupBoxAuthentication;
        private System.Windows.Forms.RadioButton radioButton_SafeCom;
        private System.Windows.Forms.RadioButton radioButton_SignInButton;
        private System.Windows.Forms.GroupBox groupBoxSafeComConfiguration;
        private System.Windows.Forms.CheckBox checkBox_SelectAll;
        private System.Windows.Forms.Label label_Copies;
        private System.Windows.Forms.NumericUpDown numericUpDown_Copies;
        private System.Windows.Forms.RadioButton radioButton_Delete;
        private System.Windows.Forms.RadioButton radioButton_Retain;
        private System.Windows.Forms.RadioButton radioButton_Print;
        private System.Windows.Forms.GroupBox groupBoxMemoryPofile;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.RadioButton radioButton_PrintAll;
        private PluginSupport.PullPrint.PrintingTabConfigControl printingTaskConfigurationControl;
        private System.Windows.Forms.CheckBox checkBoxReleaseOnSignIn;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.Label label_AuthMethod;
        private PluginSupport.MemoryCollection.DeviceMemoryProfilerControl deviceMemoryProfilerControl;
    }
}
