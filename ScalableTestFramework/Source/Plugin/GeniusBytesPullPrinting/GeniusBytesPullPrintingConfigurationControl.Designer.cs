namespace HP.ScalableTest.Plugin.GeniusBytesPullPrinting
{
    partial class GeniusBytesPullPrintingConfigurationControl
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox_ServerConfiguration = new System.Windows.Forms.GroupBox();
            this.label_ServerConfigurationNotify = new System.Windows.Forms.Label();
            this.checkBox_DeletionNotification = new System.Windows.Forms.CheckBox();
            this.checkBox_ColorModeNotification = new System.Windows.Forms.CheckBox();
            this.groupBox_CaptureDeviceMemory = new System.Windows.Forms.GroupBox();
            this.deviceMemoryProfilerControl = new HP.ScalableTest.PluginSupport.MemoryCollection.DeviceMemoryProfilerControl();
            this.groupBox_PullPrint = new System.Windows.Forms.GroupBox();
            this.radioButton_DeleteAll = new System.Windows.Forms.RadioButton();
            this.radioButton_PrintAll = new System.Windows.Forms.RadioButton();
            this.radioButton_Delete = new System.Windows.Forms.RadioButton();
            this.radioButton_PrintandDelete = new System.Windows.Forms.RadioButton();
            this.radioButton_Print = new System.Windows.Forms.RadioButton();
            this.radioButton_PrintAllandDelete = new System.Windows.Forms.RadioButton();
            this.groupBox_Authentication = new System.Windows.Forms.GroupBox();
            this.radioButton_ProximityCardLogin = new System.Windows.Forms.RadioButton();
            this.radioButton_GuestLogin = new System.Windows.Forms.RadioButton();
            this.radioButton_PINLogin = new System.Windows.Forms.RadioButton();
            this.radioButton_ManualLogin = new System.Windows.Forms.RadioButton();
            this.groupBox_Device = new System.Windows.Forms.GroupBox();
            this.checkBox_ReleaseOnSignIn = new System.Windows.Forms.CheckBox();
            this.lockTimeoutControl = new HP.ScalableTest.Framework.UI.LockTimeoutControl();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.printingConfigurationControl = new HP.ScalableTest.PluginSupport.PullPrint.PrintingTabConfigControl();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.fieldValidator = new HP.ScalableTest.Framework.UI.FieldValidator(this.components);
            this.tabControl_Main.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox_ServerConfiguration.SuspendLayout();
            this.groupBox_CaptureDeviceMemory.SuspendLayout();
            this.groupBox_PullPrint.SuspendLayout();
            this.groupBox_Authentication.SuspendLayout();
            this.groupBox_Device.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_Main
            // 
            this.tabControl_Main.Controls.Add(this.tabPage1);
            this.tabControl_Main.Controls.Add(this.tabPage2);
            this.tabControl_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Main.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Main.Name = "tabControl_Main";
            this.tabControl_Main.SelectedIndex = 0;
            this.tabControl_Main.Size = new System.Drawing.Size(689, 553);
            this.tabControl_Main.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox_ServerConfiguration);
            this.tabPage1.Controls.Add(this.groupBox_CaptureDeviceMemory);
            this.tabPage1.Controls.Add(this.groupBox_PullPrint);
            this.tabPage1.Controls.Add(this.groupBox_Authentication);
            this.tabPage1.Controls.Add(this.groupBox_Device);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(681, 525);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Pull Printing";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox_ServerConfiguration
            // 
            this.groupBox_ServerConfiguration.Controls.Add(this.label_ServerConfigurationNotify);
            this.groupBox_ServerConfiguration.Controls.Add(this.checkBox_DeletionNotification);
            this.groupBox_ServerConfiguration.Controls.Add(this.checkBox_ColorModeNotification);
            this.groupBox_ServerConfiguration.Location = new System.Drawing.Point(9, 429);
            this.groupBox_ServerConfiguration.Name = "groupBox_ServerConfiguration";
            this.groupBox_ServerConfiguration.Size = new System.Drawing.Size(676, 70);
            this.groupBox_ServerConfiguration.TabIndex = 7;
            this.groupBox_ServerConfiguration.TabStop = false;
            this.groupBox_ServerConfiguration.Text = "Server Configuration";
            // 
            // label_ServerConfigurationNotify
            // 
            this.label_ServerConfigurationNotify.AutoSize = true;
            this.label_ServerConfigurationNotify.ForeColor = System.Drawing.Color.Red;
            this.label_ServerConfigurationNotify.Location = new System.Drawing.Point(21, 21);
            this.label_ServerConfigurationNotify.Name = "label_ServerConfigurationNotify";
            this.label_ServerConfigurationNotify.Size = new System.Drawing.Size(335, 15);
            this.label_ServerConfigurationNotify.TabIndex = 9;
            this.label_ServerConfigurationNotify.Text = "Make sure these settings match the Genius MFP client settings";
            // 
            // checkBox_DeletionNotification
            // 
            this.checkBox_DeletionNotification.AutoSize = true;
            this.checkBox_DeletionNotification.Location = new System.Drawing.Point(330, 43);
            this.checkBox_DeletionNotification.Name = "checkBox_DeletionNotification";
            this.checkBox_DeletionNotification.Size = new System.Drawing.Size(223, 19);
            this.checkBox_DeletionNotification.TabIndex = 1;
            this.checkBox_DeletionNotification.Text = "Use pull printing deletion notification";
            this.checkBox_DeletionNotification.UseVisualStyleBackColor = true;
            // 
            // checkBox_ColorModeNotification
            // 
            this.checkBox_ColorModeNotification.AutoSize = true;
            this.checkBox_ColorModeNotification.Location = new System.Drawing.Point(24, 43);
            this.checkBox_ColorModeNotification.Name = "checkBox_ColorModeNotification";
            this.checkBox_ColorModeNotification.Size = new System.Drawing.Size(241, 19);
            this.checkBox_ColorModeNotification.TabIndex = 8;
            this.checkBox_ColorModeNotification.Text = "Use pull printing color mode notification";
            this.checkBox_ColorModeNotification.UseVisualStyleBackColor = true;
            // 
            // groupBox_CaptureDeviceMemory
            // 
            this.groupBox_CaptureDeviceMemory.Controls.Add(this.deviceMemoryProfilerControl);
            this.groupBox_CaptureDeviceMemory.Location = new System.Drawing.Point(9, 336);
            this.groupBox_CaptureDeviceMemory.Name = "groupBox_CaptureDeviceMemory";
            this.groupBox_CaptureDeviceMemory.Size = new System.Drawing.Size(676, 87);
            this.groupBox_CaptureDeviceMemory.TabIndex = 6;
            this.groupBox_CaptureDeviceMemory.TabStop = false;
            this.groupBox_CaptureDeviceMemory.Text = "Capture Device Memory Profile";
            // 
            // deviceMemoryProfilerControl
            // 
            this.deviceMemoryProfilerControl.Location = new System.Drawing.Point(7, 0);
            this.deviceMemoryProfilerControl.Name = "deviceMemoryProfilerControl";
            this.deviceMemoryProfilerControl.SelectedData.SampleAtCountIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.SampleAtTimeIntervals = false;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleCount = 0;
            this.deviceMemoryProfilerControl.SelectedData.TargetSampleTime = System.TimeSpan.Parse("00:00:00");
            this.deviceMemoryProfilerControl.Size = new System.Drawing.Size(318, 104);
            this.deviceMemoryProfilerControl.TabIndex = 0;
            // 
            // groupBox_PullPrint
            // 
            this.groupBox_PullPrint.Controls.Add(this.radioButton_DeleteAll);
            this.groupBox_PullPrint.Controls.Add(this.radioButton_PrintAll);
            this.groupBox_PullPrint.Controls.Add(this.radioButton_Delete);
            this.groupBox_PullPrint.Controls.Add(this.radioButton_PrintandDelete);
            this.groupBox_PullPrint.Controls.Add(this.radioButton_Print);
            this.groupBox_PullPrint.Controls.Add(this.radioButton_PrintAllandDelete);
            this.groupBox_PullPrint.Location = new System.Drawing.Point(6, 270);
            this.groupBox_PullPrint.Name = "groupBox_PullPrint";
            this.groupBox_PullPrint.Size = new System.Drawing.Size(678, 60);
            this.groupBox_PullPrint.TabIndex = 5;
            this.groupBox_PullPrint.TabStop = false;
            this.groupBox_PullPrint.Text = "Pull Print Configuration";
            // 
            // radioButton_DeleteAll
            // 
            this.radioButton_DeleteAll.AutoSize = true;
            this.radioButton_DeleteAll.Location = new System.Drawing.Point(543, 28);
            this.radioButton_DeleteAll.Name = "radioButton_DeleteAll";
            this.radioButton_DeleteAll.Size = new System.Drawing.Size(75, 19);
            this.radioButton_DeleteAll.TabIndex = 6;
            this.radioButton_DeleteAll.TabStop = true;
            this.radioButton_DeleteAll.Text = "Delete All";
            this.radioButton_DeleteAll.UseVisualStyleBackColor = true;
            // 
            // radioButton_PrintAll
            // 
            this.radioButton_PrintAll.AutoSize = true;
            this.radioButton_PrintAll.Checked = true;
            this.radioButton_PrintAll.Location = new System.Drawing.Point(15, 28);
            this.radioButton_PrintAll.Name = "radioButton_PrintAll";
            this.radioButton_PrintAll.Size = new System.Drawing.Size(67, 19);
            this.radioButton_PrintAll.TabIndex = 4;
            this.radioButton_PrintAll.TabStop = true;
            this.radioButton_PrintAll.Text = "Print All";
            this.radioButton_PrintAll.UseVisualStyleBackColor = true;
            this.radioButton_PrintAll.CheckedChanged += new System.EventHandler(this.radioButton_PullPrinting_CheckedChanged);
            // 
            // radioButton_Delete
            // 
            this.radioButton_Delete.AutoSize = true;
            this.radioButton_Delete.Location = new System.Drawing.Point(463, 28);
            this.radioButton_Delete.Name = "radioButton_Delete";
            this.radioButton_Delete.Size = new System.Drawing.Size(58, 19);
            this.radioButton_Delete.TabIndex = 3;
            this.radioButton_Delete.Text = "Delete";
            this.radioButton_Delete.UseVisualStyleBackColor = true;
            this.radioButton_Delete.CheckedChanged += new System.EventHandler(this.radioButton_PullPrinting_CheckedChanged);
            // 
            // radioButton_PrintandDelete
            // 
            this.radioButton_PrintandDelete.AutoSize = true;
            this.radioButton_PrintandDelete.Location = new System.Drawing.Point(333, 28);
            this.radioButton_PrintandDelete.Name = "radioButton_PrintandDelete";
            this.radioButton_PrintandDelete.Size = new System.Drawing.Size(109, 19);
            this.radioButton_PrintandDelete.TabIndex = 2;
            this.radioButton_PrintandDelete.Text = "Print and Delete";
            this.radioButton_PrintandDelete.UseVisualStyleBackColor = true;
            this.radioButton_PrintandDelete.CheckedChanged += new System.EventHandler(this.radioButton_PullPrinting_CheckedChanged);
            // 
            // radioButton_Print
            // 
            this.radioButton_Print.AutoSize = true;
            this.radioButton_Print.Location = new System.Drawing.Point(253, 28);
            this.radioButton_Print.Name = "radioButton_Print";
            this.radioButton_Print.Size = new System.Drawing.Size(50, 19);
            this.radioButton_Print.TabIndex = 1;
            this.radioButton_Print.Text = "Print";
            this.radioButton_Print.UseVisualStyleBackColor = true;
            this.radioButton_Print.CheckedChanged += new System.EventHandler(this.radioButton_PullPrinting_CheckedChanged);
            // 
            // radioButton_PrintAllandDelete
            // 
            this.radioButton_PrintAllandDelete.AutoSize = true;
            this.radioButton_PrintAllandDelete.Location = new System.Drawing.Point(104, 28);
            this.radioButton_PrintAllandDelete.Name = "radioButton_PrintAllandDelete";
            this.radioButton_PrintAllandDelete.Size = new System.Drawing.Size(126, 19);
            this.radioButton_PrintAllandDelete.TabIndex = 0;
            this.radioButton_PrintAllandDelete.Text = "Print All and Delete";
            this.radioButton_PrintAllandDelete.UseVisualStyleBackColor = true;
            this.radioButton_PrintAllandDelete.CheckedChanged += new System.EventHandler(this.radioButton_PullPrinting_CheckedChanged);
            // 
            // groupBox_Authentication
            // 
            this.groupBox_Authentication.Controls.Add(this.radioButton_ProximityCardLogin);
            this.groupBox_Authentication.Controls.Add(this.radioButton_GuestLogin);
            this.groupBox_Authentication.Controls.Add(this.radioButton_PINLogin);
            this.groupBox_Authentication.Controls.Add(this.radioButton_ManualLogin);
            this.groupBox_Authentication.Location = new System.Drawing.Point(6, 207);
            this.groupBox_Authentication.Name = "groupBox_Authentication";
            this.groupBox_Authentication.Size = new System.Drawing.Size(678, 57);
            this.groupBox_Authentication.TabIndex = 4;
            this.groupBox_Authentication.TabStop = false;
            this.groupBox_Authentication.Text = "Authentication Configuration";
            // 
            // radioButton_ProximityCardLogin
            // 
            this.radioButton_ProximityCardLogin.AutoSize = true;
            this.radioButton_ProximityCardLogin.Location = new System.Drawing.Point(335, 22);
            this.radioButton_ProximityCardLogin.Name = "radioButton_ProximityCardLogin";
            this.radioButton_ProximityCardLogin.Size = new System.Drawing.Size(136, 19);
            this.radioButton_ProximityCardLogin.TabIndex = 5;
            this.radioButton_ProximityCardLogin.Text = "Proximity Card Login";
            this.radioButton_ProximityCardLogin.UseVisualStyleBackColor = true;
            // 
            // radioButton_GuestLogin
            // 
            this.radioButton_GuestLogin.AutoSize = true;
            this.radioButton_GuestLogin.Enabled = false;
            this.radioButton_GuestLogin.Location = new System.Drawing.Point(15, 22);
            this.radioButton_GuestLogin.Name = "radioButton_GuestLogin";
            this.radioButton_GuestLogin.Size = new System.Drawing.Size(88, 19);
            this.radioButton_GuestLogin.TabIndex = 4;
            this.radioButton_GuestLogin.Text = "Guest Login";
            this.radioButton_GuestLogin.UseVisualStyleBackColor = true;
            // 
            // radioButton_PINLogin
            // 
            this.radioButton_PINLogin.AutoSize = true;
            this.radioButton_PINLogin.Checked = true;
            this.radioButton_PINLogin.Location = new System.Drawing.Point(131, 22);
            this.radioButton_PINLogin.Name = "radioButton_PINLogin";
            this.radioButton_PINLogin.Size = new System.Drawing.Size(77, 19);
            this.radioButton_PINLogin.TabIndex = 3;
            this.radioButton_PINLogin.TabStop = true;
            this.radioButton_PINLogin.Text = "PIN Login";
            this.radioButton_PINLogin.UseVisualStyleBackColor = true;
            // 
            // radioButton_ManualLogin
            // 
            this.radioButton_ManualLogin.AutoSize = true;
            this.radioButton_ManualLogin.Location = new System.Drawing.Point(230, 22);
            this.radioButton_ManualLogin.Name = "radioButton_ManualLogin";
            this.radioButton_ManualLogin.Size = new System.Drawing.Size(98, 19);
            this.radioButton_ManualLogin.TabIndex = 2;
            this.radioButton_ManualLogin.Text = "Manual Login";
            this.radioButton_ManualLogin.UseVisualStyleBackColor = true;
            // 
            // groupBox_Device
            // 
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
            this.checkBox_ReleaseOnSignIn.AutoSize = true;
            this.checkBox_ReleaseOnSignIn.Location = new System.Drawing.Point(278, 152);
            this.checkBox_ReleaseOnSignIn.Name = "checkBox_ReleaseOnSignIn";
            this.checkBox_ReleaseOnSignIn.Size = new System.Drawing.Size(304, 19);
            this.checkBox_ReleaseOnSignIn.TabIndex = 3;
            this.checkBox_ReleaseOnSignIn.Text = "Device is configured to release documents on sign in";
            this.checkBox_ReleaseOnSignIn.UseVisualStyleBackColor = true;
            // 
            // lockTimeoutControl
            // 
            this.lockTimeoutControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockTimeoutControl.Location = new System.Drawing.Point(7, 143);
            this.lockTimeoutControl.Name = "lockTimeoutControl";
            this.lockTimeoutControl.Size = new System.Drawing.Size(241, 53);
            this.lockTimeoutControl.TabIndex = 1;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(6, 22);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(639, 114);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.printingConfigurationControl);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(681, 525);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Printing";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // printingConfigurationControl
            // 
            this.printingConfigurationControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printingConfigurationControl.Location = new System.Drawing.Point(0, 0);
            this.printingConfigurationControl.Name = "printingConfigurationControl";
            this.printingConfigurationControl.Size = new System.Drawing.Size(690, 526);
            this.printingConfigurationControl.TabIndex = 0;
            // 
            // GeniusBytesPullPrintingConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl_Main);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "GeniusBytesPullPrintingConfigurationControl";
            this.Size = new System.Drawing.Size(689, 553);
            this.tabControl_Main.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox_ServerConfiguration.ResumeLayout(false);
            this.groupBox_ServerConfiguration.PerformLayout();
            this.groupBox_CaptureDeviceMemory.ResumeLayout(false);
            this.groupBox_PullPrint.ResumeLayout(false);
            this.groupBox_PullPrint.PerformLayout();
            this.groupBox_Authentication.ResumeLayout(false);
            this.groupBox_Authentication.PerformLayout();
            this.groupBox_Device.ResumeLayout(false);
            this.groupBox_Device.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private System.Windows.Forms.TabControl tabControl_Main;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox_Device;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private Framework.UI.LockTimeoutControl lockTimeoutControl;
        private System.Windows.Forms.CheckBox checkBox_ReleaseOnSignIn;
        private System.Windows.Forms.GroupBox groupBox_PullPrint;
        private System.Windows.Forms.GroupBox groupBox_Authentication;
        private System.Windows.Forms.GroupBox groupBox_CaptureDeviceMemory;
        private PluginSupport.MemoryCollection.DeviceMemoryProfilerControl deviceMemoryProfilerControl;
        private System.Windows.Forms.RadioButton radioButton_Delete;
        private System.Windows.Forms.RadioButton radioButton_PrintandDelete;
        private System.Windows.Forms.RadioButton radioButton_Print;
        private System.Windows.Forms.RadioButton radioButton_PrintAllandDelete;
        private PluginSupport.PullPrint.PrintingTabConfigControl printingConfigurationControl;
        private System.Windows.Forms.RadioButton radioButton_PINLogin;
        private System.Windows.Forms.RadioButton radioButton_ManualLogin;
        private System.Windows.Forms.RadioButton radioButton_GuestLogin;
        private System.Windows.Forms.RadioButton radioButton_PrintAll;
        private System.Windows.Forms.RadioButton radioButton_ProximityCardLogin;
        private System.Windows.Forms.RadioButton radioButton_DeleteAll;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox_ServerConfiguration;
        private System.Windows.Forms.CheckBox checkBox_DeletionNotification;
        private System.Windows.Forms.CheckBox checkBox_ColorModeNotification;
        private System.Windows.Forms.Label label_ServerConfigurationNotify;
    }
}
