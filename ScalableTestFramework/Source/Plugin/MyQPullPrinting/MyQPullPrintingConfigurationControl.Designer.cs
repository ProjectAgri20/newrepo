namespace HP.ScalableTest.Plugin.MyQPullPrinting
{
    partial class MyQPullPrintingConfigurationControl
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
            this.tabControl_Main = new System.Windows.Forms.TabControl();
            this.tabPage_PullPrinting = new System.Windows.Forms.TabPage();
            this.groupBox_Authentication = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_AuthProvider = new System.Windows.Forms.ComboBox();
            this.groupBox_PullPrintConfig = new System.Windows.Forms.GroupBox();
            this.checkBox_SelectAll = new System.Windows.Forms.CheckBox();
            this.radioButton_Delete = new System.Windows.Forms.RadioButton();
            this.radioButton_Print = new System.Windows.Forms.RadioButton();
            this.radioButton_PrintAll = new System.Windows.Forms.RadioButton();
            this.groupBox_CaptureDeviceMemory = new System.Windows.Forms.GroupBox();
            this.deviceMemoryProfilerControl = new HP.ScalableTest.PluginSupport.MemoryCollection.DeviceMemoryProfilerControl();
            this.groupBox_Device = new System.Windows.Forms.GroupBox();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.tabPage_Printing = new System.Windows.Forms.TabPage();
            this.printingConfigurationControl = new HP.ScalableTest.PluginSupport.PullPrint.PrintingTabConfigControl();
            this.tabControl_Main.SuspendLayout();
            this.tabPage_PullPrinting.SuspendLayout();
            this.groupBox_Authentication.SuspendLayout();
            this.groupBox_PullPrintConfig.SuspendLayout();
            this.groupBox_CaptureDeviceMemory.SuspendLayout();
            this.groupBox_Device.SuspendLayout();
            this.tabPage_Printing.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_Main
            // 
            this.tabControl_Main.Controls.Add(this.tabPage_PullPrinting);
            this.tabControl_Main.Controls.Add(this.tabPage_Printing);
            this.tabControl_Main.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Main.Name = "tabControl_Main";
            this.tabControl_Main.SelectedIndex = 0;
            this.tabControl_Main.Size = new System.Drawing.Size(689, 553);
            this.tabControl_Main.TabIndex = 0;
            // 
            // tabPage_PullPrinting
            // 
            this.tabPage_PullPrinting.Controls.Add(this.groupBox_Authentication);
            this.tabPage_PullPrinting.Controls.Add(this.groupBox_PullPrintConfig);
            this.tabPage_PullPrinting.Controls.Add(this.groupBox_CaptureDeviceMemory);
            this.tabPage_PullPrinting.Controls.Add(this.groupBox_Device);
            this.tabPage_PullPrinting.Location = new System.Drawing.Point(4, 24);
            this.tabPage_PullPrinting.Name = "tabPage_PullPrinting";
            this.tabPage_PullPrinting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_PullPrinting.Size = new System.Drawing.Size(681, 525);
            this.tabPage_PullPrinting.TabIndex = 0;
            this.tabPage_PullPrinting.Text = "Pull Printing";
            this.tabPage_PullPrinting.UseVisualStyleBackColor = true;
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Controls.Add(this.label1);
            this.groupBox_Authentication.Controls.Add(this.comboBox_AuthProvider);
            this.groupBox_Authentication.Location = new System.Drawing.Point(3, 220);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(676, 54);
            this.groupBox_Authentication.TabIndex = 7;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication Configuration";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Authentication Method";
            // 
            // comboBox_AuthProvider
            // 
            this.comboBox_AuthProvider.DisplayMember = "Value";
            this.comboBox_AuthProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_AuthProvider.FormattingEnabled = true;
            this.comboBox_AuthProvider.Location = new System.Drawing.Point(159, 19);
            this.comboBox_AuthProvider.Name = "comboBox_AuthProvider";
            this.comboBox_AuthProvider.Size = new System.Drawing.Size(121, 23);
            this.comboBox_AuthProvider.TabIndex = 0;
            this.comboBox_AuthProvider.ValueMember = "Key";
            // 
            // groupBox_PullPrintConfig
            // 
            this.groupBox_PullPrintConfig.Controls.Add(this.checkBox_SelectAll);
            this.groupBox_PullPrintConfig.Controls.Add(this.radioButton_Delete);
            this.groupBox_PullPrintConfig.Controls.Add(this.radioButton_Print);
            this.groupBox_PullPrintConfig.Controls.Add(this.radioButton_PrintAll);
            this.groupBox_PullPrintConfig.Location = new System.Drawing.Point(3, 280);
            this.groupBox_PullPrintConfig.Name = "groupBox_PullPrintConfig";
            this.groupBox_PullPrintConfig.Size = new System.Drawing.Size(676, 78);
            this.groupBox_PullPrintConfig.TabIndex = 6;
            this.groupBox_PullPrintConfig.TabStop = false;
            this.groupBox_PullPrintConfig.Text = "Pull Print Configuration";
            // 
            // checkBox_SelectAll
            // 
            this.checkBox_SelectAll.AutoSize = true;
            this.checkBox_SelectAll.Location = new System.Drawing.Point(25, 47);
            this.checkBox_SelectAll.Name = "checkBox_SelectAll";
            this.checkBox_SelectAll.Size = new System.Drawing.Size(74, 19);
            this.checkBox_SelectAll.TabIndex = 4;
            this.checkBox_SelectAll.Text = "Select All";
            this.checkBox_SelectAll.UseVisualStyleBackColor = true;
            this.checkBox_SelectAll.CheckedChanged += new System.EventHandler(this.checkBox_SelectAll_CheckedChanged);
            // 
            // radioButton_Delete
            // 
            this.radioButton_Delete.AutoSize = true;
            this.radioButton_Delete.Location = new System.Drawing.Point(186, 47);
            this.radioButton_Delete.Name = "radioButton_Delete";
            this.radioButton_Delete.Size = new System.Drawing.Size(58, 19);
            this.radioButton_Delete.TabIndex = 3;
            this.radioButton_Delete.TabStop = true;
            this.radioButton_Delete.Text = "Delete";
            this.radioButton_Delete.UseVisualStyleBackColor = true;
            // 
            // radioButton_Print
            // 
            this.radioButton_Print.AutoSize = true;
            this.radioButton_Print.Location = new System.Drawing.Point(119, 46);
            this.radioButton_Print.Name = "radioButton_Print";
            this.radioButton_Print.Size = new System.Drawing.Size(50, 19);
            this.radioButton_Print.TabIndex = 2;
            this.radioButton_Print.TabStop = true;
            this.radioButton_Print.Text = "Print";
            this.radioButton_Print.UseVisualStyleBackColor = true;
            // 
            // radioButton_PrintAll
            // 
            this.radioButton_PrintAll.AutoSize = true;
            this.radioButton_PrintAll.Checked = true;
            this.radioButton_PrintAll.Location = new System.Drawing.Point(25, 22);
            this.radioButton_PrintAll.Name = "radioButton_PrintAll";
            this.radioButton_PrintAll.Size = new System.Drawing.Size(67, 19);
            this.radioButton_PrintAll.TabIndex = 0;
            this.radioButton_PrintAll.TabStop = true;
            this.radioButton_PrintAll.Text = "Print All";
            this.radioButton_PrintAll.UseVisualStyleBackColor = true;
            this.radioButton_PrintAll.CheckedChanged += new System.EventHandler(this.checkBox_SelectAll_CheckedChanged);
            // 
            // groupBox_CaptureDeviceMemory
            // 
            this.groupBox_CaptureDeviceMemory.Controls.Add(this.deviceMemoryProfilerControl);
            this.groupBox_CaptureDeviceMemory.Location = new System.Drawing.Point(3, 364);
            this.groupBox_CaptureDeviceMemory.Name = "groupBox_CaptureDeviceMemory";
            this.groupBox_CaptureDeviceMemory.Size = new System.Drawing.Size(676, 87);
            this.groupBox_CaptureDeviceMemory.TabIndex = 5;
            this.groupBox_CaptureDeviceMemory.TabStop = false;
            this.groupBox_CaptureDeviceMemory.Text = "Capture Device Memory Profile";
            // 
            // deviceMemoryProfilerControl
            // 
            this.deviceMemoryProfilerControl.Location = new System.Drawing.Point(7, 16);
            this.deviceMemoryProfilerControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.deviceMemoryProfilerControl.Name = "deviceMemoryProfilerControl";
            this.deviceMemoryProfilerControl.SelectedData.SampleAtCountIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.SampleAtTimeIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleCount = 0;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleTime = System.TimeSpan.Parse("00:00:00");
            this.deviceMemoryProfilerControl.Size = new System.Drawing.Size(345, 71);
            this.deviceMemoryProfilerControl.TabIndex = 3;
            // 
            // groupBox_Device
            // 
            this.groupBox_Device.Controls.Add(this.assetSelectionControl);
            this.groupBox_Device.Controls.Add(this.lockTimeoutControl);
            this.groupBox_Device.Location = new System.Drawing.Point(3, 6);
            this.groupBox_Device.Name = "groupBox_Device";
            this.groupBox_Device.Size = new System.Drawing.Size(676, 204);
            this.groupBox_Device.TabIndex = 4;
            this.groupBox_Device.TabStop = false;
            this.groupBox_Device.Text = "Device Configuration";
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(3, 17);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(666, 122);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(3, 145);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 1;
            // 
            // tabPage_Printing
            // 
            this.tabPage_Printing.Controls.Add(this.printingConfigurationControl);
            this.tabPage_Printing.Location = new System.Drawing.Point(4, 24);
            this.tabPage_Printing.Name = "tabPage_Printing";
            this.tabPage_Printing.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Printing.Size = new System.Drawing.Size(681, 525);
            this.tabPage_Printing.TabIndex = 1;
            this.tabPage_Printing.Text = "Printing";
            this.tabPage_Printing.UseVisualStyleBackColor = true;
            // 
            // printingConfigurationControl
            // 
            this.printingConfigurationControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printingConfigurationControl.Location = new System.Drawing.Point(10, 0);
            this.printingConfigurationControl.Name = "printingConfigurationControl";
            this.printingConfigurationControl.Size = new System.Drawing.Size(667, 500);
            this.printingConfigurationControl.TabIndex = 0;
            // 
            // MyQPullPrintingConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl_Main);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MyQPullPrintingConfigurationControl";
            this.Size = new System.Drawing.Size(689, 553);
            this.tabControl_Main.ResumeLayout(false);
            this.tabPage_PullPrinting.ResumeLayout(false);
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.groupBox_PullPrintConfig.ResumeLayout(false);
            this.groupBox_PullPrintConfig.PerformLayout();
            this.groupBox_CaptureDeviceMemory.ResumeLayout(false);
            this.groupBox_Device.ResumeLayout(false);
            this.tabPage_Printing.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.TabControl tabControl_Main;
        private System.Windows.Forms.TabPage tabPage_PullPrinting;
        private System.Windows.Forms.TabPage tabPage_Printing;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private PluginSupport.PullPrint.PrintingTabConfigControl printingConfigurationControl;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private PluginSupport.MemoryCollection.DeviceMemoryProfilerControl deviceMemoryProfilerControl;
        private System.Windows.Forms.GroupBox groupBox_Device;
        private System.Windows.Forms.GroupBox groupBox_CaptureDeviceMemory;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.Windows.Forms.GroupBox groupBox_PullPrintConfig;
        private System.Windows.Forms.RadioButton radioButton_Delete;
        private System.Windows.Forms.RadioButton radioButton_Print;
        private System.Windows.Forms.RadioButton radioButton_PrintAll;
        private System.Windows.Forms.ComboBox comboBox_AuthProvider;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_SelectAll;
    }
}
