namespace HP.ScalableTest.Plugin.SafeQPullPrinting
{
    partial class SafeQPullPrintingConfigurationControl
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabContral_Main = new System.Windows.Forms.TabPage();
            this.groupBox_CaptureDeviceMemory = new System.Windows.Forms.GroupBox();
            this.deviceMemoryProfilerControl = new HP.ScalableTest.PluginSupport.MemoryCollection.DeviceMemoryProfilerControl();
            this.groupBox_PullPrint = new System.Windows.Forms.GroupBox();
            this.comboBox_Sides = new System.Windows.Forms.ComboBox();
            this.comboBox_ColorMode = new System.Windows.Forms.ComboBox();
            this.label_Sides = new System.Windows.Forms.Label();
            this.label_ColorMode = new System.Windows.Forms.Label();
            this.radioButton_Delete = new System.Windows.Forms.RadioButton();
            this.radioButton_PrintAll = new System.Windows.Forms.RadioButton();
            this.numericUpDown_copies = new System.Windows.Forms.NumericUpDown();
            this.label_copies = new System.Windows.Forms.Label();
            this.radioButton_Print = new System.Windows.Forms.RadioButton();
            this.checkBox_SelectAll = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.label_AuthMode = new System.Windows.Forms.Label();
            this.radioButton_SafeQPrint = new System.Windows.Forms.RadioButton();
            this.radioButton_SignInButton = new System.Windows.Forms.RadioButton();
            this.groupBox_Device = new System.Windows.Forms.GroupBox();
            this.checkBox_ReleaseOnSignIn = new System.Windows.Forms.CheckBox();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.printingConfigurationControl = new HP.ScalableTest.PluginSupport.PullPrint.PrintingTabConfigControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.tabControl1.SuspendLayout();
            this.tabContral_Main.SuspendLayout();
            this.groupBox_CaptureDeviceMemory.SuspendLayout();
            this.groupBox_PullPrint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_copies)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox_Device.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabContral_Main);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(698, 538);
            this.tabControl1.TabIndex = 0;
            // 
            // tabContral_Main
            // 
            this.tabContral_Main.Controls.Add(this.groupBox_CaptureDeviceMemory);
            this.tabContral_Main.Controls.Add(this.groupBox_PullPrint);
            this.tabContral_Main.Controls.Add(this.groupBox1);
            this.tabContral_Main.Controls.Add(this.groupBox_Device);
            this.tabContral_Main.Location = new System.Drawing.Point(4, 24);
            this.tabContral_Main.Name = "tabContral_Main";
            this.tabContral_Main.Padding = new System.Windows.Forms.Padding(3);
            this.tabContral_Main.Size = new System.Drawing.Size(690, 510);
            this.tabContral_Main.TabIndex = 0;
            this.tabContral_Main.Text = "Pull Printing";
            this.tabContral_Main.UseVisualStyleBackColor = true;
            // 
            // groupBox_CaptureDeviceMemory
            // 
            this.groupBox_CaptureDeviceMemory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_CaptureDeviceMemory.Controls.Add(this.deviceMemoryProfilerControl);
            this.groupBox_CaptureDeviceMemory.Location = new System.Drawing.Point(6, 387);
            this.groupBox_CaptureDeviceMemory.Name = "groupBox_CaptureDeviceMemory";
            this.groupBox_CaptureDeviceMemory.Size = new System.Drawing.Size(678, 119);
            this.groupBox_CaptureDeviceMemory.TabIndex = 7;
            this.groupBox_CaptureDeviceMemory.TabStop = false;
            this.groupBox_CaptureDeviceMemory.Text = "Capture Device Memory Profile";
            // 
            // deviceMemoryProfilerControl
            // 
            this.deviceMemoryProfilerControl.Location = new System.Drawing.Point(7, 22);
            this.deviceMemoryProfilerControl.Name = "deviceMemoryProfilerControl";
            this.deviceMemoryProfilerControl.SelectedData.SampleAtCountIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.SampleAtTimeIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleCount = 0;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleTime = System.TimeSpan.Parse("00:00:00");
            this.deviceMemoryProfilerControl.Size = new System.Drawing.Size(338, 84);
            this.deviceMemoryProfilerControl.TabIndex = 0;
            // 
            // groupBox_PullPrint
            // 
            this.groupBox_PullPrint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_PullPrint.Controls.Add(this.comboBox_Sides);
            this.groupBox_PullPrint.Controls.Add(this.comboBox_ColorMode);
            this.groupBox_PullPrint.Controls.Add(this.label_Sides);
            this.groupBox_PullPrint.Controls.Add(this.label_ColorMode);
            this.groupBox_PullPrint.Controls.Add(this.radioButton_Delete);
            this.groupBox_PullPrint.Controls.Add(this.radioButton_PrintAll);
            this.groupBox_PullPrint.Controls.Add(this.numericUpDown_copies);
            this.groupBox_PullPrint.Controls.Add(this.label_copies);
            this.groupBox_PullPrint.Controls.Add(this.radioButton_Print);
            this.groupBox_PullPrint.Controls.Add(this.checkBox_SelectAll);
            this.groupBox_PullPrint.Location = new System.Drawing.Point(6, 304);
            this.groupBox_PullPrint.Name = "groupBox_PullPrint";
            this.groupBox_PullPrint.Size = new System.Drawing.Size(678, 77);
            this.groupBox_PullPrint.TabIndex = 6;
            this.groupBox_PullPrint.TabStop = false;
            this.groupBox_PullPrint.Text = "Pull Print Configuration";
            // 
            // comboBox_Sides
            // 
            this.comboBox_Sides.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Sides.FormattingEnabled = true;
            this.comboBox_Sides.Location = new System.Drawing.Point(551, 44);
            this.comboBox_Sides.Name = "comboBox_Sides";
            this.comboBox_Sides.Size = new System.Drawing.Size(121, 23);
            this.comboBox_Sides.TabIndex = 13;
            // 
            // comboBox_ColorMode
            // 
            this.comboBox_ColorMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ColorMode.FormattingEnabled = true;
            this.comboBox_ColorMode.Location = new System.Drawing.Point(551, 15);
            this.comboBox_ColorMode.Name = "comboBox_ColorMode";
            this.comboBox_ColorMode.Size = new System.Drawing.Size(121, 23);
            this.comboBox_ColorMode.TabIndex = 12;
            // 
            // label_Sides
            // 
            this.label_Sides.AutoSize = true;
            this.label_Sides.Location = new System.Drawing.Point(505, 47);
            this.label_Sides.Name = "label_Sides";
            this.label_Sides.Size = new System.Drawing.Size(37, 15);
            this.label_Sides.TabIndex = 11;
            this.label_Sides.Text = "Sides:";
            // 
            // label_ColorMode
            // 
            this.label_ColorMode.AutoSize = true;
            this.label_ColorMode.Location = new System.Drawing.Point(472, 15);
            this.label_ColorMode.Name = "label_ColorMode";
            this.label_ColorMode.Size = new System.Drawing.Size(73, 15);
            this.label_ColorMode.TabIndex = 10;
            this.label_ColorMode.Text = "Color Mode:";
            // 
            // radioButton_Delete
            // 
            this.radioButton_Delete.AutoSize = true;
            this.radioButton_Delete.Location = new System.Drawing.Point(142, 16);
            this.radioButton_Delete.Name = "radioButton_Delete";
            this.radioButton_Delete.Size = new System.Drawing.Size(58, 19);
            this.radioButton_Delete.TabIndex = 9;
            this.radioButton_Delete.TabStop = true;
            this.radioButton_Delete.Text = "Delete";
            this.radioButton_Delete.UseVisualStyleBackColor = true;
            // 
            // radioButton_PrintAll
            // 
            this.radioButton_PrintAll.AutoSize = true;
            this.radioButton_PrintAll.Location = new System.Drawing.Point(86, 45);
            this.radioButton_PrintAll.Name = "radioButton_PrintAll";
            this.radioButton_PrintAll.Size = new System.Drawing.Size(129, 19);
            this.radioButton_PrintAll.TabIndex = 8;
            this.radioButton_PrintAll.TabStop = true;
            this.radioButton_PrintAll.Text = "Print All from Login";
            this.radioButton_PrintAll.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_copies
            // 
            this.numericUpDown_copies.Location = new System.Drawing.Point(317, 16);
            this.numericUpDown_copies.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_copies.Name = "numericUpDown_copies";
            this.numericUpDown_copies.Size = new System.Drawing.Size(49, 23);
            this.numericUpDown_copies.TabIndex = 7;
            this.numericUpDown_copies.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label_copies
            // 
            this.label_copies.AutoSize = true;
            this.label_copies.Location = new System.Drawing.Point(206, 18);
            this.label_copies.Name = "label_copies";
            this.label_copies.Size = new System.Drawing.Size(105, 15);
            this.label_copies.TabIndex = 6;
            this.label_copies.Text = "Number of copies:";
            // 
            // radioButton_Print
            // 
            this.radioButton_Print.AutoSize = true;
            this.radioButton_Print.Location = new System.Drawing.Point(86, 16);
            this.radioButton_Print.Name = "radioButton_Print";
            this.radioButton_Print.Size = new System.Drawing.Size(50, 19);
            this.radioButton_Print.TabIndex = 4;
            this.radioButton_Print.TabStop = true;
            this.radioButton_Print.Text = "Print";
            this.radioButton_Print.UseVisualStyleBackColor = true;
            // 
            // checkBox_SelectAll
            // 
            this.checkBox_SelectAll.AutoSize = true;
            this.checkBox_SelectAll.Checked = true;
            this.checkBox_SelectAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_SelectAll.Location = new System.Drawing.Point(6, 17);
            this.checkBox_SelectAll.Name = "checkBox_SelectAll";
            this.checkBox_SelectAll.Size = new System.Drawing.Size(74, 19);
            this.checkBox_SelectAll.TabIndex = 0;
            this.checkBox_SelectAll.Text = "Select All";
            this.checkBox_SelectAll.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox_AuthProvider);
            this.groupBox1.Controls.Add(this.label_AuthMode);
            this.groupBox1.Controls.Add(this.radioButton_SafeQPrint);
            this.groupBox1.Controls.Add(this.radioButton_SignInButton);
            this.groupBox1.Location = new System.Drawing.Point(6, 207);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(678, 93);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Authentication Configuration";
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(198, 57);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(176, 23);
            this.comboBox_AuthProvider.TabIndex = 5;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // label_AuthMode
            // 
            this.label_AuthMode.AutoSize = true;
            this.label_AuthMode.Location = new System.Drawing.Point(15, 60);
            this.label_AuthMode.Name = "label_AuthMode";
            this.label_AuthMode.Size = new System.Drawing.Size(131, 15);
            this.label_AuthMode.TabIndex = 4;
            this.label_AuthMode.Text = "Authentication Method";
            // 
            // radioButton_SafeQPrint
            // 
            this.radioButton_SafeQPrint.AutoSize = true;
            this.radioButton_SafeQPrint.Location = new System.Drawing.Point(198, 31);
            this.radioButton_SafeQPrint.Name = "radioButton_SafeQPrint";
            this.radioButton_SafeQPrint.Size = new System.Drawing.Size(84, 19);
            this.radioButton_SafeQPrint.TabIndex = 2;
            this.radioButton_SafeQPrint.Text = "SafeQ Print";
            this.radioButton_SafeQPrint.UseVisualStyleBackColor = true;
            // 
            // radioButton_SignInButton
            // 
            this.radioButton_SignInButton.AutoSize = true;
            this.radioButton_SignInButton.Checked = true;
            this.radioButton_SignInButton.Location = new System.Drawing.Point(18, 31);
            this.radioButton_SignInButton.Name = "radioButton_SignInButton";
            this.radioButton_SignInButton.Size = new System.Drawing.Size(100, 19);
            this.radioButton_SignInButton.TabIndex = 1;
            this.radioButton_SignInButton.TabStop = true;
            this.radioButton_SignInButton.Text = "Sign In Button";
            this.radioButton_SignInButton.UseVisualStyleBackColor = true;
            // 
            // groupBox_Device
            // 
            this.groupBox_Device.Controls.Add(this.checkBox_ReleaseOnSignIn);
            this.groupBox_Device.Controls.Add(this.lockTimeoutControl);
            this.groupBox_Device.Controls.Add(this.assetSelectionControl);
            this.groupBox_Device.Location = new System.Drawing.Point(6, 6);
            this.groupBox_Device.Name = "groupBox_Device";
            this.groupBox_Device.Size = new System.Drawing.Size(678, 197);
            this.groupBox_Device.TabIndex = 0;
            this.groupBox_Device.TabStop = false;
            this.groupBox_Device.Text = "Device Configuration";
            // 
            // checkBox_ReleaseOnSignIn
            // 
            this.checkBox_ReleaseOnSignIn.AutoSize = true;
            this.checkBox_ReleaseOnSignIn.Location = new System.Drawing.Point(287, 152);
            this.checkBox_ReleaseOnSignIn.Name = "checkBox_ReleaseOnSignIn";
            this.checkBox_ReleaseOnSignIn.Size = new System.Drawing.Size(304, 19);
            this.checkBox_ReleaseOnSignIn.TabIndex = 2;
            this.checkBox_ReleaseOnSignIn.Text = "Device is configured to release documents on sign in";
            this.checkBox_ReleaseOnSignIn.UseVisualStyleBackColor = true;
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(6, 142);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 1;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(6, 22);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(666, 114);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.printingConfigurationControl);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(690, 510);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Printing";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // printingConfigurationControl
            // 
            this.printingConfigurationControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printingConfigurationControl.Location = new System.Drawing.Point(3, 3);
            this.printingConfigurationControl.Name = "printingConfigurationControl";
            this.printingConfigurationControl.Size = new System.Drawing.Size(684, 520);
            this.printingConfigurationControl.TabIndex = 0;
            // 
            // SafeQPullPrintingConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SafeQPullPrintingConfigurationControl";
            this.Size = new System.Drawing.Size(698, 554);
            this.tabControl1.ResumeLayout(false);
            this.tabContral_Main.ResumeLayout(false);
            this.groupBox_CaptureDeviceMemory.ResumeLayout(false);
            this.groupBox_PullPrint.ResumeLayout(false);
            this.groupBox_PullPrint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_copies)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox_Device.ResumeLayout(false);
            this.groupBox_Device.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabContral_Main;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox_Device;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private System.Windows.Forms.CheckBox checkBox_ReleaseOnSignIn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_SignInButton;
        private System.Windows.Forms.RadioButton radioButton_SafeQPrint;
        private System.Windows.Forms.Label label_AuthMode;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.GroupBox groupBox_PullPrint;
        private System.Windows.Forms.RadioButton radioButton_PrintAll;
        private System.Windows.Forms.NumericUpDown numericUpDown_copies;
        private System.Windows.Forms.Label label_copies;
        private System.Windows.Forms.RadioButton radioButton_Print;
        private System.Windows.Forms.CheckBox checkBox_SelectAll;
        private System.Windows.Forms.RadioButton radioButton_Delete;
        private System.Windows.Forms.Label label_Sides;
        private System.Windows.Forms.Label label_ColorMode;
        private System.Windows.Forms.ComboBox comboBox_Sides;
        private System.Windows.Forms.ComboBox comboBox_ColorMode;
        private System.Windows.Forms.GroupBox groupBox_CaptureDeviceMemory;
        private PluginSupport.MemoryCollection.DeviceMemoryProfilerControl deviceMemoryProfilerControl;
        private PluginSupport.PullPrint.PrintingTabConfigControl printingConfigurationControl;
    }
}
