namespace HP.ScalableTest.Plugin.Celiveo
{
    partial class CeliveoConfigurationControl
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
            this.tabControl_Main = new System.Windows.Forms.TabControl();
            this.tabPage_pullPrinting = new System.Windows.Forms.TabPage();
            this.groupBox_CaptureDeviceMemory = new System.Windows.Forms.GroupBox();
            this.groupBox_PullPrint = new System.Windows.Forms.GroupBox();
            this.radioButton_PrintBW = new System.Windows.Forms.RadioButton();
            this.numericUpDown_copies = new System.Windows.Forms.NumericUpDown();
            this.label_copies = new System.Windows.Forms.Label();
            this.radioButton_Delete = new System.Windows.Forms.RadioButton();
            this.radioButton_Print = new System.Windows.Forms.RadioButton();
            this.checkBox_SelectAll = new System.Windows.Forms.CheckBox();
            this.groupBox_Authentication = new System.Windows.Forms.GroupBox();
            this.label_AuthMode = new System.Windows.Forms.Label();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.radioButton_Celiveo = new System.Windows.Forms.RadioButton();
            this.radioButton_SignInButton = new System.Windows.Forms.RadioButton();
            this.groupBox_Device = new System.Windows.Forms.GroupBox();
            this.checkBox_ReleaseOnSignIn = new System.Windows.Forms.CheckBox();
            this.tabPage_printing = new System.Windows.Forms.TabPage();
            this.deviceMemoryProfilerControl = new HP.ScalableTest.PluginSupport.MemoryCollection.DeviceMemoryProfilerControl();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.printingConfigurationControl = new HP.ScalableTest.PluginSupport.PullPrint.PrintingTabConfigControl();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.tabControl_Main.SuspendLayout();
            this.tabPage_pullPrinting.SuspendLayout();
            this.groupBox_CaptureDeviceMemory.SuspendLayout();
            this.groupBox_PullPrint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_copies)).BeginInit();
            this.groupBox_Authentication.SuspendLayout();
            this.groupBox_Device.SuspendLayout();
            this.tabPage_printing.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_Main
            // 
            this.tabControl_Main.Controls.Add(this.tabPage_pullPrinting);
            this.tabControl_Main.Controls.Add(this.tabPage_printing);
            this.tabControl_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Main.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Main.Name = "tabControl_Main";
            this.tabControl_Main.SelectedIndex = 0;
            this.tabControl_Main.Size = new System.Drawing.Size(698, 554);
            this.tabControl_Main.TabIndex = 0;
            // 
            // tabPage_pullPrinting
            // 
            this.tabPage_pullPrinting.Controls.Add(this.groupBox_CaptureDeviceMemory);
            this.tabPage_pullPrinting.Controls.Add(this.groupBox_PullPrint);
            this.tabPage_pullPrinting.Controls.Add(this.groupBox_Authentication);
            this.tabPage_pullPrinting.Controls.Add(this.groupBox_Device);
            this.tabPage_pullPrinting.Location = new System.Drawing.Point(4, 24);
            this.tabPage_pullPrinting.Name = "tabPage_pullPrinting";
            this.tabPage_pullPrinting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_pullPrinting.Size = new System.Drawing.Size(690, 526);
            this.tabPage_pullPrinting.TabIndex = 0;
            this.tabPage_pullPrinting.Text = "Pull Printing";
            this.tabPage_pullPrinting.UseVisualStyleBackColor = true;
            // 
            // groupBox_CaptureDeviceMemory
            // 
            this.groupBox_CaptureDeviceMemory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_CaptureDeviceMemory.Controls.Add(this.deviceMemoryProfilerControl);
            this.groupBox_CaptureDeviceMemory.Location = new System.Drawing.Point(6, 389);
            this.groupBox_CaptureDeviceMemory.Name = "groupBox_CaptureDeviceMemory";
            this.groupBox_CaptureDeviceMemory.Size = new System.Drawing.Size(678, 119);
            this.groupBox_CaptureDeviceMemory.TabIndex = 6;
            this.groupBox_CaptureDeviceMemory.TabStop = false;
            this.groupBox_CaptureDeviceMemory.Text = "Capture Device Memory Profile";
            // 
            // groupBox_PullPrint
            // 
            this.groupBox_PullPrint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_PullPrint.Controls.Add(this.radioButton_PrintBW);
            this.groupBox_PullPrint.Controls.Add(this.numericUpDown_copies);
            this.groupBox_PullPrint.Controls.Add(this.label_copies);
            this.groupBox_PullPrint.Controls.Add(this.radioButton_Delete);
            this.groupBox_PullPrint.Controls.Add(this.radioButton_Print);
            this.groupBox_PullPrint.Controls.Add(this.checkBox_SelectAll);
            this.groupBox_PullPrint.Location = new System.Drawing.Point(6, 306);
            this.groupBox_PullPrint.Name = "groupBox_PullPrint";
            this.groupBox_PullPrint.Size = new System.Drawing.Size(678, 77);
            this.groupBox_PullPrint.TabIndex = 5;
            this.groupBox_PullPrint.TabStop = false;
            this.groupBox_PullPrint.Text = "Pull Print Configuration";
            // 
            // radioButton_PrintBW
            // 
            this.radioButton_PrintBW.AutoSize = true;
            this.radioButton_PrintBW.Location = new System.Drawing.Point(98, 35);
            this.radioButton_PrintBW.Name = "radioButton_PrintBW";
            this.radioButton_PrintBW.Size = new System.Drawing.Size(76, 19);
            this.radioButton_PrintBW.TabIndex = 8;
            this.radioButton_PrintBW.TabStop = true;
            this.radioButton_PrintBW.Text = "Print B/W";
            this.radioButton_PrintBW.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_copies
            // 
            this.numericUpDown_copies.Location = new System.Drawing.Point(455, 32);
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
            this.label_copies.Location = new System.Drawing.Point(344, 34);
            this.label_copies.Name = "label_copies";
            this.label_copies.Size = new System.Drawing.Size(105, 15);
            this.label_copies.TabIndex = 6;
            this.label_copies.Text = "Number of copies:";
            // 
            // radioButton_Delete
            // 
            this.radioButton_Delete.AutoSize = true;
            this.radioButton_Delete.Location = new System.Drawing.Point(236, 36);
            this.radioButton_Delete.Name = "radioButton_Delete";
            this.radioButton_Delete.Size = new System.Drawing.Size(58, 19);
            this.radioButton_Delete.TabIndex = 5;
            this.radioButton_Delete.TabStop = true;
            this.radioButton_Delete.Text = "Delete";
            this.radioButton_Delete.UseVisualStyleBackColor = true;
            // 
            // radioButton_Print
            // 
            this.radioButton_Print.AutoSize = true;
            this.radioButton_Print.Location = new System.Drawing.Point(180, 36);
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
            this.checkBox_SelectAll.Location = new System.Drawing.Point(18, 35);
            this.checkBox_SelectAll.Name = "checkBox_SelectAll";
            this.checkBox_SelectAll.Size = new System.Drawing.Size(74, 19);
            this.checkBox_SelectAll.TabIndex = 0;
            this.checkBox_SelectAll.Text = "Select All";
            this.checkBox_SelectAll.UseVisualStyleBackColor = true;
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Authentication.Controls.Add(this.label_AuthMode);
            this.groupBox_Authentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBox_Authentication.Controls.Add(this.radioButton_Celiveo);
            this.groupBox_Authentication.Controls.Add(this.radioButton_SignInButton);
            this.groupBox_Authentication.Location = new System.Drawing.Point(6, 207);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(678, 93);
            this.groupBox_Authentication.TabIndex = 4;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication Configuration";
            // 
            // label_AuthMode
            // 
            this.label_AuthMode.AutoSize = true;
            this.label_AuthMode.Location = new System.Drawing.Point(15, 60);
            this.label_AuthMode.Name = "label_AuthMode";
            this.label_AuthMode.Size = new System.Drawing.Size(131, 15);
            this.label_AuthMode.TabIndex = 3;
            this.label_AuthMode.Text = "Authentication Method";
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(198, 57);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(176, 23);
            this.comboBox_AuthProvider.TabIndex = 2;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // radioButton_Celiveo
            // 
            this.radioButton_Celiveo.AutoSize = true;
            this.radioButton_Celiveo.Location = new System.Drawing.Point(198, 31);
            this.radioButton_Celiveo.Name = "radioButton_Celiveo";
            this.radioButton_Celiveo.Size = new System.Drawing.Size(64, 19);
            this.radioButton_Celiveo.TabIndex = 1;
            this.radioButton_Celiveo.Text = "Celiveo";
            this.radioButton_Celiveo.UseVisualStyleBackColor = true;
            // 
            // radioButton_SignInButton
            // 
            this.radioButton_SignInButton.AutoSize = true;
            this.radioButton_SignInButton.Checked = true;
            this.radioButton_SignInButton.Location = new System.Drawing.Point(18, 31);
            this.radioButton_SignInButton.Name = "radioButton_SignInButton";
            this.radioButton_SignInButton.Size = new System.Drawing.Size(100, 19);
            this.radioButton_SignInButton.TabIndex = 0;
            this.radioButton_SignInButton.TabStop = true;
            this.radioButton_SignInButton.Text = "Sign In Button";
            this.radioButton_SignInButton.UseVisualStyleBackColor = true;
            // 
            // groupBox_Device
            // 
            this.groupBox_Device.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_Device.Controls.Add(this.checkBox_ReleaseOnSignIn);
            this.groupBox_Device.Controls.Add(this.lockTimeoutControl);
            this.groupBox_Device.Controls.Add(this.assetSelectionControl);
            this.groupBox_Device.Location = new System.Drawing.Point(6, 6);
            this.groupBox_Device.Name = "groupBox_Device";
            this.groupBox_Device.Size = new System.Drawing.Size(678, 197);
            this.groupBox_Device.TabIndex = 3;
            this.groupBox_Device.TabStop = false;
            this.groupBox_Device.Text = "Device Configuration";
            // 
            // checkBox_ReleaseOnSignIn
            // 
            this.checkBox_ReleaseOnSignIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_ReleaseOnSignIn.AutoSize = true;
            this.checkBox_ReleaseOnSignIn.Location = new System.Drawing.Point(287, 157);
            this.checkBox_ReleaseOnSignIn.Name = "checkBox_ReleaseOnSignIn";
            this.checkBox_ReleaseOnSignIn.Size = new System.Drawing.Size(304, 19);
            this.checkBox_ReleaseOnSignIn.TabIndex = 3;
            this.checkBox_ReleaseOnSignIn.Text = "Device is configured to release documents on sign in";
            this.checkBox_ReleaseOnSignIn.UseVisualStyleBackColor = true;
            // 
            // tabPage_printing
            // 
            this.tabPage_printing.Controls.Add(this.printingConfigurationControl);
            this.tabPage_printing.Location = new System.Drawing.Point(4, 24);
            this.tabPage_printing.Name = "tabPage_printing";
            this.tabPage_printing.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_printing.Size = new System.Drawing.Size(690, 526);
            this.tabPage_printing.TabIndex = 1;
            this.tabPage_printing.Text = "Printing";
            this.tabPage_printing.UseVisualStyleBackColor = true;
            // 
            // deviceMemoryProfilerControl
            // 
            this.deviceMemoryProfilerControl.Location = new System.Drawing.Point(7, 22);
            this.deviceMemoryProfilerControl.Name = "deviceMemoryProfilerControl";
            this.deviceMemoryProfilerControl.SelectedData.SampleAtCountIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.SampleAtTimeIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleCount = 0;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleTime = System.TimeSpan.Parse("00:00:00");
            this.deviceMemoryProfilerControl.Size = new System.Drawing.Size(318, 91);
            this.deviceMemoryProfilerControl.TabIndex = 0;
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(6, 142);
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
            this.assetSelectionControl.Location = new System.Drawing.Point(6, 22);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(666, 114);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // printingConfigurationControl
            // 
            this.printingConfigurationControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printingConfigurationControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printingConfigurationControl.Location = new System.Drawing.Point(3, 3);
            this.printingConfigurationControl.Name = "printingConfigurationControl";
            this.printingConfigurationControl.Size = new System.Drawing.Size(684, 522);
            this.printingConfigurationControl.TabIndex = 0;
            // 
            // CeliveoConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl_Main);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "CeliveoConfigurationControl";
            this.Size = new System.Drawing.Size(698, 554);
            this.tabControl_Main.ResumeLayout(false);
            this.tabPage_pullPrinting.ResumeLayout(false);
            this.groupBox_CaptureDeviceMemory.ResumeLayout(false);
            this.groupBox_PullPrint.ResumeLayout(false);
            this.groupBox_PullPrint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_copies)).EndInit();
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.groupBox_Device.ResumeLayout(false);
            this.groupBox_Device.PerformLayout();
            this.tabPage_printing.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.TabControl tabControl_Main;
        private System.Windows.Forms.TabPage tabPage_pullPrinting;
        private System.Windows.Forms.TabPage tabPage_printing;
        private PluginSupport.PullPrint.PrintingTabConfigControl printingConfigurationControl;
        private System.Windows.Forms.GroupBox groupBox_Device;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.Windows.Forms.GroupBox groupBox_PullPrint;
        private System.Windows.Forms.GroupBox groupBox_CaptureDeviceMemory;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private System.Windows.Forms.RadioButton radioButton_SignInButton;
        private System.Windows.Forms.RadioButton radioButton_Celiveo;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.Label label_AuthMode;
        private System.Windows.Forms.CheckBox checkBox_SelectAll;
        private System.Windows.Forms.RadioButton radioButton_Delete;
        private System.Windows.Forms.RadioButton radioButton_Print;
        private PluginSupport.MemoryCollection.DeviceMemoryProfilerControl deviceMemoryProfilerControl;
        private System.Windows.Forms.Label label_copies;
        private System.Windows.Forms.NumericUpDown numericUpDown_copies;
        private System.Windows.Forms.CheckBox checkBox_ReleaseOnSignIn;
        private System.Windows.Forms.RadioButton radioButton_PrintBW;
    }
}
