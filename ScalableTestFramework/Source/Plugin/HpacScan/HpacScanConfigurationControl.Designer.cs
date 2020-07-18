namespace HP.ScalableTest.Plugin.HpacScan
{
    partial class HpacScanConfigurationControl
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
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.groupBox_Device = new System.Windows.Forms.GroupBox();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.groupBox_Authentication = new System.Windows.Forms.GroupBox();
            this.label_AuthMethod = new System.Windows.Forms.Label();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.deviceMemoryProfilerControl = new HP.ScalableTest.PluginSupport.MemoryCollection.DeviceMemoryProfilerControl();
            this.groupBox_CaptureDeviceMemory = new System.Windows.Forms.GroupBox();
            this.groupBox_Advanced = new System.Windows.Forms.GroupBox();
            this.jobBuildPageCount_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label_ScanCount = new System.Windows.Forms.Label();
            this.label_JobBuild = new System.Windows.Forms.Label();
            this.label_Quality = new System.Windows.Forms.Label();
            this.label_ColorMode = new System.Windows.Forms.Label();
            this.label_PaperSupply = new System.Windows.Forms.Label();
            this.checkBox_JobBuild = new System.Windows.Forms.CheckBox();
            this.comboBox_ColorMode = new System.Windows.Forms.ComboBox();
            this.comboBox_Quality = new System.Windows.Forms.ComboBox();
            this.comboBox_PaperSupply = new System.Windows.Forms.ComboBox();
            this.groupBox_Device.SuspendLayout();
            this.groupBox_Authentication.SuspendLayout();
            this.groupBox_CaptureDeviceMemory.SuspendLayout();
            this.groupBox_Advanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jobBuildPageCount_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(4, 19);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(676, 122);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // groupBox_Device
            // 
            this.groupBox_Device.Controls.Add(this.lockTimeoutControl);
            this.groupBox_Device.Controls.Add(this.assetSelectionControl);
            this.groupBox_Device.Location = new System.Drawing.Point(3, 15);
            this.groupBox_Device.Name = "groupBox_Device";
            this.groupBox_Device.Size = new System.Drawing.Size(686, 204);
            this.groupBox_Device.TabIndex = 1;
            this.groupBox_Device.TabStop = false;
            this.groupBox_Device.Text = "Device Configuration";
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(4, 146);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 1;
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Controls.Add(this.label_AuthMethod);
            this.groupBox_Authentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBox_Authentication.Location = new System.Drawing.Point(3, 225);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(686, 62);
            this.groupBox_Authentication.TabIndex = 2;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication";
            // 
            // label_AuthMethod
            // 
            this.label_AuthMethod.AutoSize = true;
            this.label_AuthMethod.Location = new System.Drawing.Point(21, 26);
            this.label_AuthMethod.Name = "label_AuthMethod";
            this.label_AuthMethod.Size = new System.Drawing.Size(131, 15);
            this.label_AuthMethod.TabIndex = 1;
            this.label_AuthMethod.Text = "Authentication Method";
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(168, 23);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(121, 23);
            this.comboBox_AuthProvider.TabIndex = 0;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // deviceMemoryProfilerControl
            // 
            this.deviceMemoryProfilerControl.Location = new System.Drawing.Point(15, 17);
            this.deviceMemoryProfilerControl.Name = "deviceMemoryProfilerControl";
            this.deviceMemoryProfilerControl.SelectedData.SampleAtCountIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.SampleAtTimeIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleCount = 0;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleTime = System.TimeSpan.Parse("00:00:00");
            this.deviceMemoryProfilerControl.Size = new System.Drawing.Size(321, 64);
            this.deviceMemoryProfilerControl.TabIndex = 3;
            // 
            // groupBox_CaptureDeviceMemory
            // 
            this.groupBox_CaptureDeviceMemory.Controls.Add(this.deviceMemoryProfilerControl);
            this.groupBox_CaptureDeviceMemory.Location = new System.Drawing.Point(3, 441);
            this.groupBox_CaptureDeviceMemory.Name = "groupBox_CaptureDeviceMemory";
            this.groupBox_CaptureDeviceMemory.Size = new System.Drawing.Size(686, 86);
            this.groupBox_CaptureDeviceMemory.TabIndex = 4;
            this.groupBox_CaptureDeviceMemory.TabStop = false;
            this.groupBox_CaptureDeviceMemory.Text = "Capture Device Memory Profile";
            // 
            // groupBox_Advanced
            // 
            this.groupBox_Advanced.Controls.Add(this.jobBuildPageCount_numericUpDown);
            this.groupBox_Advanced.Controls.Add(this.label_ScanCount);
            this.groupBox_Advanced.Controls.Add(this.label_JobBuild);
            this.groupBox_Advanced.Controls.Add(this.label_Quality);
            this.groupBox_Advanced.Controls.Add(this.label_ColorMode);
            this.groupBox_Advanced.Controls.Add(this.label_PaperSupply);
            this.groupBox_Advanced.Controls.Add(this.checkBox_JobBuild);
            this.groupBox_Advanced.Controls.Add(this.comboBox_ColorMode);
            this.groupBox_Advanced.Controls.Add(this.comboBox_Quality);
            this.groupBox_Advanced.Controls.Add(this.comboBox_PaperSupply);
            this.groupBox_Advanced.Location = new System.Drawing.Point(3, 293);
            this.groupBox_Advanced.Name = "groupBox_Advanced";
            this.groupBox_Advanced.Size = new System.Drawing.Size(686, 146);
            this.groupBox_Advanced.TabIndex = 5;
            this.groupBox_Advanced.TabStop = false;
            this.groupBox_Advanced.Text = "Advanced";
            // 
            // jobBuildPageCount_numericUpDown
            // 
            this.jobBuildPageCount_numericUpDown.Enabled = false;
            this.jobBuildPageCount_numericUpDown.Location = new System.Drawing.Point(404, 23);
            this.jobBuildPageCount_numericUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.jobBuildPageCount_numericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.jobBuildPageCount_numericUpDown.Name = "jobBuildPageCount_numericUpDown";
            this.jobBuildPageCount_numericUpDown.Size = new System.Drawing.Size(121, 23);
            this.jobBuildPageCount_numericUpDown.TabIndex = 11;
            this.jobBuildPageCount_numericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label_ScanCount
            // 
            this.label_ScanCount.AutoSize = true;
            this.label_ScanCount.Location = new System.Drawing.Point(324, 25);
            this.label_ScanCount.Name = "label_ScanCount";
            this.label_ScanCount.Size = new System.Drawing.Size(74, 15);
            this.label_ScanCount.TabIndex = 10;
            this.label_ScanCount.Text = "Scan Count :";
            // 
            // label_JobBuild
            // 
            this.label_JobBuild.AutoSize = true;
            this.label_JobBuild.Location = new System.Drawing.Point(21, 120);
            this.label_JobBuild.Name = "label_JobBuild";
            this.label_JobBuild.Size = new System.Drawing.Size(61, 15);
            this.label_JobBuild.TabIndex = 9;
            this.label_JobBuild.Text = "Job build :";
            // 
            // label_Quality
            // 
            this.label_Quality.AutoSize = true;
            this.label_Quality.Location = new System.Drawing.Point(21, 90);
            this.label_Quality.Name = "label_Quality";
            this.label_Quality.Size = new System.Drawing.Size(51, 15);
            this.label_Quality.TabIndex = 8;
            this.label_Quality.Text = "Quality :";
            // 
            // label_ColorMode
            // 
            this.label_ColorMode.AutoSize = true;
            this.label_ColorMode.Location = new System.Drawing.Point(21, 60);
            this.label_ColorMode.Name = "label_ColorMode";
            this.label_ColorMode.Size = new System.Drawing.Size(76, 15);
            this.label_ColorMode.TabIndex = 7;
            this.label_ColorMode.Text = "Color mode :";
            // 
            // label_PaperSupply
            // 
            this.label_PaperSupply.AutoSize = true;
            this.label_PaperSupply.Location = new System.Drawing.Point(21, 30);
            this.label_PaperSupply.Name = "label_PaperSupply";
            this.label_PaperSupply.Size = new System.Drawing.Size(81, 15);
            this.label_PaperSupply.TabIndex = 6;
            this.label_PaperSupply.Text = "Paper supply :";
            // 
            // checkBox_JobBuild
            // 
            this.checkBox_JobBuild.AutoSize = true;
            this.checkBox_JobBuild.Location = new System.Drawing.Point(168, 120);
            this.checkBox_JobBuild.Name = "checkBox_JobBuild";
            this.checkBox_JobBuild.Size = new System.Drawing.Size(15, 14);
            this.checkBox_JobBuild.TabIndex = 5;
            this.checkBox_JobBuild.UseVisualStyleBackColor = true;
            this.checkBox_JobBuild.CheckedChanged += new System.EventHandler(this.checkBox_JobBuild_CheckedChanged);
            // 
            // comboBox_ColorMode
            // 
            this.comboBox_ColorMode.DisplayMember = "Value";
            this.comboBox_ColorMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_ColorMode.FormattingEnabled = true;
            this.comboBox_ColorMode.Location = new System.Drawing.Point(168, 57);
            this.comboBox_ColorMode.Name = "comboBox_ColorMode";
            this.comboBox_ColorMode.Size = new System.Drawing.Size(121, 23);
            this.comboBox_ColorMode.TabIndex = 4;
            this.comboBox_ColorMode.ValueMember = "Key";
            // 
            // comboBox_Quality
            // 
            this.comboBox_Quality.DisplayMember = "Value";
            this.comboBox_Quality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Quality.FormattingEnabled = true;
            this.comboBox_Quality.Location = new System.Drawing.Point(168, 87);
            this.comboBox_Quality.Name = "comboBox_Quality";
            this.comboBox_Quality.Size = new System.Drawing.Size(121, 23);
            this.comboBox_Quality.TabIndex = 3;
            this.comboBox_Quality.ValueMember = "Key";
            // 
            // comboBox_PaperSupply
            // 
            this.comboBox_PaperSupply.DisplayMember = "Value";
            this.comboBox_PaperSupply.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_PaperSupply.FormattingEnabled = true;
            this.comboBox_PaperSupply.Location = new System.Drawing.Point(168, 22);
            this.comboBox_PaperSupply.Name = "comboBox_PaperSupply";
            this.comboBox_PaperSupply.Size = new System.Drawing.Size(121, 23);
            this.comboBox_PaperSupply.TabIndex = 2;
            this.comboBox_PaperSupply.ValueMember = "Key";
            this.comboBox_PaperSupply.SelectedIndexChanged += new System.EventHandler(this.checkBox_JobBuild_CheckedChanged);
            // 
            // HpacScanConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox_Advanced);
            this.Controls.Add(this.groupBox_CaptureDeviceMemory);
            this.Controls.Add(this.groupBox_Authentication);
            this.Controls.Add(this.groupBox_Device);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "HpacScanConfigurationControl";
            this.Size = new System.Drawing.Size(689, 553);
            this.groupBox_Device.ResumeLayout(false);
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.groupBox_CaptureDeviceMemory.ResumeLayout(false);
            this.groupBox_Advanced.ResumeLayout(false);
            this.groupBox_Advanced.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.jobBuildPageCount_numericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.GroupBox groupBox_Device;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label_AuthMethod;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private PluginSupport.MemoryCollection.DeviceMemoryProfilerControl deviceMemoryProfilerControl;
        private System.Windows.Forms.GroupBox groupBox_CaptureDeviceMemory;
        private System.Windows.Forms.GroupBox groupBox_Advanced;
        private System.Windows.Forms.Label label_JobBuild;
        private System.Windows.Forms.Label label_Quality;
        private System.Windows.Forms.Label label_ColorMode;
        private System.Windows.Forms.Label label_PaperSupply;
        private System.Windows.Forms.CheckBox checkBox_JobBuild;
        private System.Windows.Forms.ComboBox comboBox_ColorMode;
        private System.Windows.Forms.ComboBox comboBox_Quality;
        private System.Windows.Forms.ComboBox comboBox_PaperSupply;
        private System.Windows.Forms.Label label_ScanCount;
        private System.Windows.Forms.NumericUpDown jobBuildPageCount_numericUpDown;
    }
}
