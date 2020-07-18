﻿namespace HP.ScalableTest.Plugin.iSecStarPullPrinting
{
    partial class iSecStarPullPrintingConfigurationControl
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
            this.tabControlISecStar = new System.Windows.Forms.TabControl();
            this.tabPagePullPrinting = new System.Windows.Forms.TabPage();
            this.groupBoxMemoryPofile = new System.Windows.Forms.GroupBox();
            this.groupBox_PullPrintConfig = new System.Windows.Forms.GroupBox();
            this.radioButton_Delete = new System.Windows.Forms.RadioButton();
            this.checkBox_SelectAll = new System.Windows.Forms.CheckBox();
            this.radioButton_Print = new System.Windows.Forms.RadioButton();
            this.radioButton_RePrint = new System.Windows.Forms.RadioButton();
            this.groupBoxAuthentication = new System.Windows.Forms.GroupBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.radioButton_iSecStar = new System.Windows.Forms.RadioButton();
            this.radioButton_SignInButton = new System.Windows.Forms.RadioButton();
            this.groupBoxDeviceConfiguration = new System.Windows.Forms.GroupBox();
            this.checkBoxReleaseOnSignIn = new System.Windows.Forms.CheckBox();
            this.tabPagePrinting = new System.Windows.Forms.TabPage();
            this.deviceMemoryProfilerControl = new HP.ScalableTest.PluginSupport.MemoryCollection.DeviceMemoryProfilerControl();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.printingTaskConfigurationControl = new HP.ScalableTest.PluginSupport.PullPrint.PrintingTabConfigControl();
            this.tabControlISecStar.SuspendLayout();
            this.tabPagePullPrinting.SuspendLayout();
            this.groupBoxMemoryPofile.SuspendLayout();
            this.groupBox_PullPrintConfig.SuspendLayout();
            this.groupBoxAuthentication.SuspendLayout();
            this.groupBoxDeviceConfiguration.SuspendLayout();
            this.tabPagePrinting.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlISecStar
            // 
            this.tabControlISecStar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlISecStar.Controls.Add(this.tabPagePullPrinting);
            this.tabControlISecStar.Controls.Add(this.tabPagePrinting);
            this.tabControlISecStar.Location = new System.Drawing.Point(7, 9);
            this.tabControlISecStar.Name = "tabControlISecStar";
            this.tabControlISecStar.SelectedIndex = 0;
            this.tabControlISecStar.Size = new System.Drawing.Size(685, 557);
            this.tabControlISecStar.TabIndex = 0;
            // 
            // tabPagePullPrinting
            // 
            this.tabPagePullPrinting.AutoScroll = true;
            this.tabPagePullPrinting.Controls.Add(this.groupBoxMemoryPofile);
            this.tabPagePullPrinting.Controls.Add(this.groupBox_PullPrintConfig);
            this.tabPagePullPrinting.Controls.Add(this.groupBoxAuthentication);
            this.tabPagePullPrinting.Controls.Add(this.groupBoxDeviceConfiguration);
            this.tabPagePullPrinting.Location = new System.Drawing.Point(4, 22);
            this.tabPagePullPrinting.Name = "tabPagePullPrinting";
            this.tabPagePullPrinting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePullPrinting.Size = new System.Drawing.Size(677, 531);
            this.tabPagePullPrinting.TabIndex = 0;
            this.tabPagePullPrinting.Text = "PullPrinting";
            this.tabPagePullPrinting.UseVisualStyleBackColor = true;
            // 
            // groupBoxMemoryPofile
            // 
            this.groupBoxMemoryPofile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxMemoryPofile.Controls.Add(this.deviceMemoryProfilerControl);
            this.groupBoxMemoryPofile.Location = new System.Drawing.Point(5, 407);
            this.groupBoxMemoryPofile.Name = "groupBoxMemoryPofile";
            this.groupBoxMemoryPofile.Size = new System.Drawing.Size(667, 105);
            this.groupBoxMemoryPofile.TabIndex = 4;
            this.groupBoxMemoryPofile.TabStop = false;
            this.groupBoxMemoryPofile.Text = "Capture Device Memory Profile";
            // 
            // groupBox_PullPrintConfig
            // 
            this.groupBox_PullPrintConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_PullPrintConfig.Controls.Add(this.radioButton_Delete);
            this.groupBox_PullPrintConfig.Controls.Add(this.checkBox_SelectAll);
            this.groupBox_PullPrintConfig.Controls.Add(this.radioButton_Print);
            this.groupBox_PullPrintConfig.Controls.Add(this.radioButton_RePrint);
            this.groupBox_PullPrintConfig.Location = new System.Drawing.Point(5, 321);
            this.groupBox_PullPrintConfig.Name = "groupBox_PullPrintConfig";
            this.groupBox_PullPrintConfig.Size = new System.Drawing.Size(666, 80);
            this.groupBox_PullPrintConfig.TabIndex = 3;
            this.groupBox_PullPrintConfig.TabStop = false;
            this.groupBox_PullPrintConfig.Text = "Pull Print Configuration";
            // 
            // radioButton_Delete
            // 
            this.radioButton_Delete.AutoSize = true;
            this.radioButton_Delete.Location = new System.Drawing.Point(168, 47);
            this.radioButton_Delete.Name = "radioButton_Delete";
            this.radioButton_Delete.Size = new System.Drawing.Size(56, 17);
            this.radioButton_Delete.TabIndex = 9;
            this.radioButton_Delete.TabStop = true;
            this.radioButton_Delete.Text = "Delete";
            this.radioButton_Delete.UseVisualStyleBackColor = true;
            this.radioButton_Delete.CheckedChanged += new System.EventHandler(this.RadioButton_Action_CheckedChanged);
            // 
            // checkBox_SelectAll
            // 
            this.checkBox_SelectAll.AutoSize = true;
            this.checkBox_SelectAll.Location = new System.Drawing.Point(20, 24);
            this.checkBox_SelectAll.Name = "checkBox_SelectAll";
            this.checkBox_SelectAll.Size = new System.Drawing.Size(70, 17);
            this.checkBox_SelectAll.TabIndex = 8;
            this.checkBox_SelectAll.Text = "Select All";
            this.checkBox_SelectAll.UseVisualStyleBackColor = true;
            // 
            // radioButton_Print
            // 
            this.radioButton_Print.AutoSize = true;
            this.radioButton_Print.Location = new System.Drawing.Point(102, 47);
            this.radioButton_Print.Name = "radioButton_Print";
            this.radioButton_Print.Size = new System.Drawing.Size(46, 17);
            this.radioButton_Print.TabIndex = 2;
            this.radioButton_Print.TabStop = true;
            this.radioButton_Print.Text = "Print";
            this.radioButton_Print.UseVisualStyleBackColor = true;
            this.radioButton_Print.CheckedChanged += new System.EventHandler(this.RadioButton_Action_CheckedChanged);
            // 
            // radioButton_RePrint
            // 
            this.radioButton_RePrint.AutoSize = true;
            this.radioButton_RePrint.Checked = true;
            this.radioButton_RePrint.Location = new System.Drawing.Point(20, 47);
            this.radioButton_RePrint.Name = "radioButton_RePrint";
            this.radioButton_RePrint.Size = new System.Drawing.Size(60, 17);
            this.radioButton_RePrint.TabIndex = 1;
            this.radioButton_RePrint.TabStop = true;
            this.radioButton_RePrint.Text = "RePrint";
            this.radioButton_RePrint.UseVisualStyleBackColor = true;
            this.radioButton_RePrint.CheckedChanged += new System.EventHandler(this.RadioButton_Action_CheckedChanged);
            // 
            // groupBoxAuthentication
            // 
            this.groupBoxAuthentication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAuthentication.Controls.Add(this.label_AuthMethod);
            this.groupBoxAuthentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBoxAuthentication.Controls.Add(this.radioButton_iSecStar);
            this.groupBoxAuthentication.Controls.Add(this.radioButton_SignInButton);
            this.groupBoxAuthentication.Location = new System.Drawing.Point(5, 218);
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
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(168, 61);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(170, 21);
            this.comboBox_AuthProvider.TabIndex = 2;
            this.comboBox_AuthProvider.ValueMember = "Key";
            this.comboBox_AuthProvider.SelectedIndexChanged += new System.EventHandler(this.comboBox_AuthProvider_SelectedIndexChanged);
            // 
            // radioButton_iSecStar
            // 
            this.radioButton_iSecStar.AutoSize = true;
            this.radioButton_iSecStar.Location = new System.Drawing.Point(169, 33);
            this.radioButton_iSecStar.Name = "radioButton_iSecStar";
            this.radioButton_iSecStar.Size = new System.Drawing.Size(147, 17);
            this.radioButton_iSecStar.TabIndex = 1;
            this.radioButton_iSecStar.Text = "ISecStar Secure Pull Print";
            this.radioButton_iSecStar.UseVisualStyleBackColor = true;
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
            // tabPagePrinting
            // 
            this.tabPagePrinting.AutoScroll = true;
            this.tabPagePrinting.Controls.Add(this.printingTaskConfigurationControl);
            this.tabPagePrinting.Location = new System.Drawing.Point(4, 22);
            this.tabPagePrinting.Name = "tabPagePrinting";
            this.tabPagePrinting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePrinting.Size = new System.Drawing.Size(677, 531);
            this.tabPagePrinting.TabIndex = 1;
            this.tabPagePrinting.Text = "Printing";
            this.tabPagePrinting.UseVisualStyleBackColor = true;
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
            // printingTaskConfigurationControl
            // 
            this.printingTaskConfigurationControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printingTaskConfigurationControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printingTaskConfigurationControl.Location = new System.Drawing.Point(3, 3);
            this.printingTaskConfigurationControl.Name = "printingTaskConfigurationControl";
            this.printingTaskConfigurationControl.Size = new System.Drawing.Size(671, 525);
            this.printingTaskConfigurationControl.TabIndex = 1;
            // 
            // iSecStarPullPrintingConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlISecStar);
            this.Name = "iSecStarPullPrintingConfigurationControl";
            this.Size = new System.Drawing.Size(698, 574);
            this.tabControlISecStar.ResumeLayout(false);
            this.tabPagePullPrinting.ResumeLayout(false);
            this.groupBoxMemoryPofile.ResumeLayout(false);
            this.groupBox_PullPrintConfig.ResumeLayout(false);
            this.groupBox_PullPrintConfig.PerformLayout();
            this.groupBoxAuthentication.ResumeLayout(false);
            this.groupBoxAuthentication.PerformLayout();
            this.groupBoxDeviceConfiguration.ResumeLayout(false);
            this.groupBoxDeviceConfiguration.PerformLayout();
            this.tabPagePrinting.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlISecStar;
        private System.Windows.Forms.TabPage tabPagePullPrinting;
        private System.Windows.Forms.TabPage tabPagePrinting;
        private System.Windows.Forms.GroupBox groupBoxDeviceConfiguration;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.GroupBox groupBoxAuthentication;
        private System.Windows.Forms.RadioButton radioButton_iSecStar;
        private System.Windows.Forms.RadioButton radioButton_SignInButton;
        private System.Windows.Forms.GroupBox groupBox_PullPrintConfig;
        private System.Windows.Forms.RadioButton radioButton_Print;
        private System.Windows.Forms.RadioButton radioButton_RePrint;
        private System.Windows.Forms.CheckBox checkBox_SelectAll;
        private System.Windows.Forms.GroupBox groupBoxMemoryPofile;
        private PluginSupport.PullPrint.PrintingTabConfigControl printingTaskConfigurationControl;
        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.CheckBox checkBoxReleaseOnSignIn;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.Label label_AuthMethod;
        private PluginSupport.MemoryCollection.DeviceMemoryProfilerControl deviceMemoryProfilerControl;
        private System.Windows.Forms.RadioButton radioButton_Delete;
    }
}
