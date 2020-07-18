namespace HP.ScalableTest.Plugin.MyQScan
{
    partial class MyQScanConfigurationControl
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
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.assetSelectionControl1 = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.groupBox_DeviceConfiguration = new System.Windows.Forms.GroupBox();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.groupBox_Authentication = new System.Windows.Forms.GroupBox();
            this.comboBox_AuthMethod = new System.Windows.Forms.ComboBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.deviceMemoryProfilerControl = new HP.ScalableTest.PluginSupport.MemoryCollection.DeviceMemoryProfilerControl();
            this.groupBox_CaptureDeviceMemory = new System.Windows.Forms.GroupBox();
            this.groupBox_EasyScan = new System.Windows.Forms.GroupBox();
            this.radioButton_Email = new System.Windows.Forms.RadioButton();
            this.radioButton_Folder = new System.Windows.Forms.RadioButton();
            this.groupBox_DeviceConfiguration.SuspendLayout();
            this.groupBox_Authentication.SuspendLayout();
            this.groupBox_CaptureDeviceMemory.SuspendLayout();
            this.groupBox_EasyScan.SuspendLayout();
            this.SuspendLayout();
            // 
            // assetSelectionControl1
            // 
            this.assetSelectionControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl1.Location = new System.Drawing.Point(4, 19);
            this.assetSelectionControl1.Name = "assetSelectionControl1";
            this.assetSelectionControl1.Size = new System.Drawing.Size(686, 143);
            this.assetSelectionControl1.TabIndex = 0;
            // 
            // groupBox_DeviceConfiguration
            // 
            this.groupBox_DeviceConfiguration.Controls.Add(this.lockTimeoutControl);
            this.groupBox_DeviceConfiguration.Controls.Add(this.assetSelectionControl1);
            this.groupBox_DeviceConfiguration.Location = new System.Drawing.Point(3, 15);
            this.groupBox_DeviceConfiguration.Name = "groupBox_DeviceConfiguration";
            this.groupBox_DeviceConfiguration.Size = new System.Drawing.Size(686, 226);
            this.groupBox_DeviceConfiguration.TabIndex = 1;
            this.groupBox_DeviceConfiguration.TabStop = false;
            this.groupBox_DeviceConfiguration.Text = "Device Configuration";
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(4, 168);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 1;
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Controls.Add(this.label_AuthMethod);
            this.groupBox_Authentication.Controls.Add(this.comboBox_AuthMethod);
            this.groupBox_Authentication.Location = new System.Drawing.Point(7, 248);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(682, 71);
            this.groupBox_Authentication.TabIndex = 2;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication";
            // 
            // comboBox_AuthMethod
            // 
            this.comboBox_AuthMethod.DisplayMember = "Value";
            this.comboBox_AuthMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthMethod.FormattingEnabled = true;
            this.comboBox_AuthMethod.Location = new System.Drawing.Point(173, 29);
            this.comboBox_AuthMethod.Name = "comboBox_AuthMethod";
            this.comboBox_AuthMethod.Size = new System.Drawing.Size(155, 23);
            this.comboBox_AuthMethod.TabIndex = 0;
            this.comboBox_AuthMethod.ValueMember = "Key";
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(18, 32);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(134, 15);
            this.label_AuthMethod.TabIndex = 1;
            this.label_AuthMethod.Text = "Authentication Method ";
            // 
            // deviceMemoryProfilerControl
            // 
            this.deviceMemoryProfilerControl.Location = new System.Drawing.Point(25, 22);
            this.deviceMemoryProfilerControl.Name = "deviceMemoryProfilerControl";
            this.deviceMemoryProfilerControl.SelectedData.SampleAtCountIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.SampleAtTimeIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleCount = 0;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleTime = System.TimeSpan.Parse("00:00:00");
            this.deviceMemoryProfilerControl.Size = new System.Drawing.Size(320, 70);
            this.deviceMemoryProfilerControl.TabIndex = 3;
            // 
            // groupBox_CaptureDeviceMemory
            // 
            this.groupBox_CaptureDeviceMemory.Controls.Add(this.deviceMemoryProfilerControl);
            this.groupBox_CaptureDeviceMemory.Location = new System.Drawing.Point(3, 397);
            this.groupBox_CaptureDeviceMemory.Name = "groupBox_CaptureDeviceMemory";
            this.groupBox_CaptureDeviceMemory.Size = new System.Drawing.Size(686, 98);
            this.groupBox_CaptureDeviceMemory.TabIndex = 4;
            this.groupBox_CaptureDeviceMemory.TabStop = false;
            this.groupBox_CaptureDeviceMemory.Text = "Capture Device Memory Profile";
            // 
            // groupBox_EasyScan
            // 
            this.groupBox_EasyScan.Controls.Add(this.radioButton_Folder);
            this.groupBox_EasyScan.Controls.Add(this.radioButton_Email);
            this.groupBox_EasyScan.Location = new System.Drawing.Point(7, 326);
            this.groupBox_EasyScan.Name = "groupBox_EasyScan";
            this.groupBox_EasyScan.Size = new System.Drawing.Size(682, 62);
            this.groupBox_EasyScan.TabIndex = 5;
            this.groupBox_EasyScan.TabStop = false;
            this.groupBox_EasyScan.Text = "Easy Scan";
            // 
            // radioButton_Email
            // 
            this.radioButton_Email.AutoSize = true;
            this.radioButton_Email.Location = new System.Drawing.Point(21, 26);
            this.radioButton_Email.Name = "radioButton_Email";
            this.radioButton_Email.Size = new System.Drawing.Size(54, 19);
            this.radioButton_Email.TabIndex = 0;
            this.radioButton_Email.TabStop = true;
            this.radioButton_Email.Text = "Email";
            this.radioButton_Email.UseVisualStyleBackColor = true;
            // 
            // radioButton_Folder
            // 
            this.radioButton_Folder.AutoSize = true;
            this.radioButton_Folder.Location = new System.Drawing.Point(104, 26);
            this.radioButton_Folder.Name = "radioButton_Folder";
            this.radioButton_Folder.Size = new System.Drawing.Size(58, 19);
            this.radioButton_Folder.TabIndex = 1;
            this.radioButton_Folder.TabStop = true;
            this.radioButton_Folder.Text = "Folder";
            this.radioButton_Folder.UseVisualStyleBackColor = true;
            // 
            // MyQScanConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox_EasyScan);
            this.Controls.Add(this.groupBox_CaptureDeviceMemory);
            this.Controls.Add(this.groupBox_Authentication);
            this.Controls.Add(this.groupBox_DeviceConfiguration);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MyQScanConfigurationControl";
            this.Size = new System.Drawing.Size(689, 553);
            this.groupBox_DeviceConfiguration.ResumeLayout(false);
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.groupBox_CaptureDeviceMemory.ResumeLayout(false);
            this.groupBox_EasyScan.ResumeLayout(false);
            this.groupBox_EasyScan.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.AssetSelectionControl assetSelectionControl1;
        private System.Windows.Forms.GroupBox groupBox_DeviceConfiguration;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.Windows.Forms.Label label_AuthMethod;
        private System.Windows.Forms.ComboBox comboBox_AuthMethod;
        private PluginSupport.MemoryCollection.DeviceMemoryProfilerControl deviceMemoryProfilerControl;
        private System.Windows.Forms.GroupBox groupBox_CaptureDeviceMemory;
        private System.Windows.Forms.GroupBox groupBox_EasyScan;
        private System.Windows.Forms.RadioButton radioButton_Folder;
        private System.Windows.Forms.RadioButton radioButton_Email;
    }
}
