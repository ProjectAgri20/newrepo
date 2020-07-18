using HP.ScalableTest.Plugin.DeviceInspector.FieldControls;
using HP.ScalableTest.Plugin.DeviceInspector.SettingsControls;
namespace HP.ScalableTest.Plugin.DeviceInspector
{
    partial class DeviceInspectorConfigurationControl
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.deviceSelection_Tab = new System.Windows.Forms.TabPage();
            this.assetSelectionControl = new HP.ScalableTest.Framework.UI.AssetSelectionControl();
            this.TimeLanguage_Settings = new System.Windows.Forms.TabPage();
            this.NotificationsTabPage = new System.Windows.Forms.TabPage();
            this.WarningLabel = new System.Windows.Forms.Label();
            this.enablePassword_CheckBox = new System.Windows.Forms.CheckBox();
            this.TabbedSettingsTabControl = new System.Windows.Forms.TabControl();
            this.emailDefaults_Tab = new System.Windows.Forms.TabPage();
            this.copyDefaults_Tab = new System.Windows.Forms.TabPage();
            this.printDefaults_Tab = new System.Windows.Forms.TabPage();
            this.folderDefaults_Tab = new System.Windows.Forms.TabPage();
            this.jobDefaults_Tab = new System.Windows.Forms.TabPage();
            this.usbDefaults_Tab = new System.Windows.Forms.TabPage();
            this.quickSet_Tab = new System.Windows.Forms.TabPage();
            this.protocolDefaults_Tab = new System.Windows.Forms.TabPage();
            this.passwordWindowsControl = new HP.ScalableTest.Plugin.DeviceInspector.SettingsControls.PasswordWindowsControl();
            this.generalSettingsControl = new HP.ScalableTest.Plugin.DeviceInspector.SettingsControls.GeneralSettingsControl();
            this.emailSettingsControl = new HP.ScalableTest.Plugin.DeviceInspector.SettingsControls.EmailDefaultControl();
            this.copyDefaultControl = new HP.ScalableTest.Plugin.DeviceInspector.SettingsControls.CopyDefaultControl();
            this.printDefaultControl = new HP.ScalableTest.Plugin.DeviceInspector.SettingsControls.PrintDefaultControl();
            this.folderDefaultControl = new HP.ScalableTest.Plugin.DeviceInspector.SettingsControls.FolderDefaultControl();
            this.jobStorageDefaultControl = new HP.ScalableTest.Plugin.DeviceInspector.SettingsControls.JobStorageDefaultControl();
            this.scanToUsbDefaultControl = new HP.ScalableTest.Plugin.DeviceInspector.SettingsControls.ScanToUsbDefaultControl();
            this.quickSetControl = new HP.ScalableTest.Plugin.DeviceInspector.SettingsControls.QuickSetControl();
            this.protocolDefaultControl = new HP.ScalableTest.Plugin.DeviceInspector.SettingsControls.ProtocolDefaultControl();
            this.faxDefaults_Tab = new System.Windows.Forms.TabPage();
            this.faxDefaultControl = new HP.ScalableTest.Plugin.DeviceInspector.SettingsControls.FaxDefaultControl();
            this.deviceSelection_Tab.SuspendLayout();
            this.TimeLanguage_Settings.SuspendLayout();
            this.NotificationsTabPage.SuspendLayout();
            this.TabbedSettingsTabControl.SuspendLayout();
            this.emailDefaults_Tab.SuspendLayout();
            this.copyDefaults_Tab.SuspendLayout();
            this.printDefaults_Tab.SuspendLayout();
            this.folderDefaults_Tab.SuspendLayout();
            this.jobDefaults_Tab.SuspendLayout();
            this.usbDefaults_Tab.SuspendLayout();
            this.quickSet_Tab.SuspendLayout();
            this.protocolDefaults_Tab.SuspendLayout();
            this.faxDefaults_Tab.SuspendLayout();
            this.SuspendLayout();
            // 
            // deviceSelection_Tab
            // 
            this.deviceSelection_Tab.Controls.Add(this.assetSelectionControl);
            this.deviceSelection_Tab.Location = new System.Drawing.Point(4, 24);
            this.deviceSelection_Tab.Name = "deviceSelection_Tab";
            this.deviceSelection_Tab.Padding = new System.Windows.Forms.Padding(3);
            this.deviceSelection_Tab.Size = new System.Drawing.Size(830, 649);
            this.deviceSelection_Tab.TabIndex = 2;
            this.deviceSelection_Tab.Text = "Device Selection";
            this.deviceSelection_Tab.UseVisualStyleBackColor = true;
            // 
            // assetSelectionControl
            // 
            this.assetSelectionControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assetSelectionControl.Location = new System.Drawing.Point(0, 0);
            this.assetSelectionControl.Name = "assetSelectionControl";
            this.assetSelectionControl.Size = new System.Drawing.Size(768, 358);
            this.assetSelectionControl.TabIndex = 0;
            // 
            // TimeLanguage_Settings
            // 
            this.TimeLanguage_Settings.BackColor = System.Drawing.SystemColors.Window;
            this.TimeLanguage_Settings.Controls.Add(this.generalSettingsControl);
            this.TimeLanguage_Settings.Location = new System.Drawing.Point(4, 24);
            this.TimeLanguage_Settings.Name = "TimeLanguage_Settings";
            this.TimeLanguage_Settings.Padding = new System.Windows.Forms.Padding(3);
            this.TimeLanguage_Settings.Size = new System.Drawing.Size(830, 649);
            this.TimeLanguage_Settings.TabIndex = 1;
            this.TimeLanguage_Settings.Text = "Time/Language Settings";
            // 
            // NotificationsTabPage
            // 
            this.NotificationsTabPage.BackColor = System.Drawing.SystemColors.Window;
            this.NotificationsTabPage.Controls.Add(this.WarningLabel);
            this.NotificationsTabPage.Controls.Add(this.enablePassword_CheckBox);
            this.NotificationsTabPage.Controls.Add(this.passwordWindowsControl);
            this.NotificationsTabPage.Location = new System.Drawing.Point(4, 24);
            this.NotificationsTabPage.Name = "NotificationsTabPage";
            this.NotificationsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.NotificationsTabPage.Size = new System.Drawing.Size(830, 649);
            this.NotificationsTabPage.TabIndex = 0;
            this.NotificationsTabPage.Text = "Usage Settings";
            // 
            // WarningLabel
            // 
            this.WarningLabel.AutoSize = true;
            this.WarningLabel.ForeColor = System.Drawing.Color.Red;
            this.WarningLabel.Location = new System.Drawing.Point(49, 20);
            this.WarningLabel.Name = "WarningLabel";
            this.WarningLabel.Size = new System.Drawing.Size(281, 15);
            this.WarningLabel.TabIndex = 32;
            this.WarningLabel.Text = "Plugin should only be used with Clean/New Devices";
            // 
            // enablePassword_CheckBox
            // 
            this.enablePassword_CheckBox.AutoSize = true;
            this.enablePassword_CheckBox.Location = new System.Drawing.Point(52, 62);
            this.enablePassword_CheckBox.Name = "enablePassword_CheckBox";
            this.enablePassword_CheckBox.Size = new System.Drawing.Size(155, 19);
            this.enablePassword_CheckBox.TabIndex = 30;
            this.enablePassword_CheckBox.Text = "Enable Default Password";
            this.enablePassword_CheckBox.UseVisualStyleBackColor = true;
            // 
            // TabbedSettingsTabControl
            // 
            this.TabbedSettingsTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabbedSettingsTabControl.Controls.Add(this.deviceSelection_Tab);
            this.TabbedSettingsTabControl.Controls.Add(this.NotificationsTabPage);
            this.TabbedSettingsTabControl.Controls.Add(this.TimeLanguage_Settings);
            this.TabbedSettingsTabControl.Controls.Add(this.emailDefaults_Tab);
            this.TabbedSettingsTabControl.Controls.Add(this.copyDefaults_Tab);
            this.TabbedSettingsTabControl.Controls.Add(this.printDefaults_Tab);
            this.TabbedSettingsTabControl.Controls.Add(this.faxDefaults_Tab);
            this.TabbedSettingsTabControl.Controls.Add(this.folderDefaults_Tab);
            this.TabbedSettingsTabControl.Controls.Add(this.jobDefaults_Tab);
            this.TabbedSettingsTabControl.Controls.Add(this.usbDefaults_Tab);
            this.TabbedSettingsTabControl.Controls.Add(this.quickSet_Tab);
            this.TabbedSettingsTabControl.Controls.Add(this.protocolDefaults_Tab);
            this.TabbedSettingsTabControl.Location = new System.Drawing.Point(3, 3);
            this.TabbedSettingsTabControl.Name = "TabbedSettingsTabControl";
            this.TabbedSettingsTabControl.SelectedIndex = 0;
            this.TabbedSettingsTabControl.Size = new System.Drawing.Size(838, 677);
            this.TabbedSettingsTabControl.TabIndex = 4;
            // 
            // emailDefaults_Tab
            // 
            this.emailDefaults_Tab.Controls.Add(this.emailSettingsControl);
            this.emailDefaults_Tab.Location = new System.Drawing.Point(4, 24);
            this.emailDefaults_Tab.Name = "emailDefaults_Tab";
            this.emailDefaults_Tab.Padding = new System.Windows.Forms.Padding(3);
            this.emailDefaults_Tab.Size = new System.Drawing.Size(830, 649);
            this.emailDefaults_Tab.TabIndex = 3;
            this.emailDefaults_Tab.Text = "Email Default";
            this.emailDefaults_Tab.UseVisualStyleBackColor = true;
            // 
            // copyDefaults_Tab
            // 
            this.copyDefaults_Tab.Controls.Add(this.copyDefaultControl);
            this.copyDefaults_Tab.Location = new System.Drawing.Point(4, 24);
            this.copyDefaults_Tab.Name = "copyDefaults_Tab";
            this.copyDefaults_Tab.Size = new System.Drawing.Size(830, 649);
            this.copyDefaults_Tab.TabIndex = 5;
            this.copyDefaults_Tab.Text = "Copy Default";
            this.copyDefaults_Tab.UseVisualStyleBackColor = true;
            // 
            // printDefaults_Tab
            // 
            this.printDefaults_Tab.Controls.Add(this.printDefaultControl);
            this.printDefaults_Tab.Location = new System.Drawing.Point(4, 24);
            this.printDefaults_Tab.Name = "printDefaults_Tab";
            this.printDefaults_Tab.Size = new System.Drawing.Size(830, 649);
            this.printDefaults_Tab.TabIndex = 6;
            this.printDefaults_Tab.Text = "Print Default";
            this.printDefaults_Tab.UseVisualStyleBackColor = true;
            // 
            // folderDefaults_Tab
            // 
            this.folderDefaults_Tab.Controls.Add(this.folderDefaultControl);
            this.folderDefaults_Tab.Location = new System.Drawing.Point(4, 24);
            this.folderDefaults_Tab.Name = "folderDefaults_Tab";
            this.folderDefaults_Tab.Size = new System.Drawing.Size(830, 649);
            this.folderDefaults_Tab.TabIndex = 7;
            this.folderDefaults_Tab.Text = "Folder Default";
            this.folderDefaults_Tab.UseVisualStyleBackColor = true;
            // 
            // jobDefaults_Tab
            // 
            this.jobDefaults_Tab.Controls.Add(this.jobStorageDefaultControl);
            this.jobDefaults_Tab.Location = new System.Drawing.Point(4, 24);
            this.jobDefaults_Tab.Name = "jobDefaults_Tab";
            this.jobDefaults_Tab.Size = new System.Drawing.Size(830, 649);
            this.jobDefaults_Tab.TabIndex = 8;
            this.jobDefaults_Tab.Text = "Job Storage";
            this.jobDefaults_Tab.UseVisualStyleBackColor = true;
            // 
            // usbDefaults_Tab
            // 
            this.usbDefaults_Tab.Controls.Add(this.scanToUsbDefaultControl);
            this.usbDefaults_Tab.Location = new System.Drawing.Point(4, 24);
            this.usbDefaults_Tab.Name = "usbDefaults_Tab";
            this.usbDefaults_Tab.Size = new System.Drawing.Size(830, 649);
            this.usbDefaults_Tab.TabIndex = 9;
            this.usbDefaults_Tab.Text = "Scan To Usb Default";
            this.usbDefaults_Tab.UseVisualStyleBackColor = true;
            // 
            // quickSet_Tab
            // 
            this.quickSet_Tab.Controls.Add(this.quickSetControl);
            this.quickSet_Tab.Location = new System.Drawing.Point(4, 24);
            this.quickSet_Tab.Name = "quickSet_Tab";
            this.quickSet_Tab.Padding = new System.Windows.Forms.Padding(3);
            this.quickSet_Tab.Size = new System.Drawing.Size(830, 649);
            this.quickSet_Tab.TabIndex = 4;
            this.quickSet_Tab.Text = "QuickSets";
            this.quickSet_Tab.UseVisualStyleBackColor = true;
            // 
            // protocolDefaults_Tab
            // 
            this.protocolDefaults_Tab.Controls.Add(this.protocolDefaultControl);
            this.protocolDefaults_Tab.Location = new System.Drawing.Point(4, 24);
            this.protocolDefaults_Tab.Name = "protocolDefaults_Tab";
            this.protocolDefaults_Tab.Size = new System.Drawing.Size(830, 649);
            this.protocolDefaults_Tab.TabIndex = 10;
            this.protocolDefaults_Tab.Text = "Protocol Default";
            this.protocolDefaults_Tab.UseVisualStyleBackColor = true;
            // 
            // passwordWindowsControl
            // 
            this.passwordWindowsControl.Location = new System.Drawing.Point(38, 87);
            this.passwordWindowsControl.Name = "passwordWindowsControl";
            this.passwordWindowsControl.Size = new System.Drawing.Size(705, 177);
            this.passwordWindowsControl.TabIndex = 31;
            // 
            // generalSettingsControl
            // 
            this.generalSettingsControl.Location = new System.Drawing.Point(16, 18);
            this.generalSettingsControl.Name = "generalSettingsControl";
            this.generalSettingsControl.Size = new System.Drawing.Size(725, 481);
            this.generalSettingsControl.TabIndex = 0;
            // 
            // emailSettingsControl
            // 
            this.emailSettingsControl.Location = new System.Drawing.Point(29, 24);
            this.emailSettingsControl.Name = "emailSettingsControl";
            this.emailSettingsControl.Size = new System.Drawing.Size(712, 553);
            this.emailSettingsControl.TabIndex = 0;
            // 
            // copyDefaultControl
            // 
            this.copyDefaultControl.Location = new System.Drawing.Point(10, 24);
            this.copyDefaultControl.Name = "copyDefaultControl";
            this.copyDefaultControl.Size = new System.Drawing.Size(810, 584);
            this.copyDefaultControl.TabIndex = 0;
            // 
            // printDefaultControl
            // 
            this.printDefaultControl.Location = new System.Drawing.Point(10, 24);
            this.printDefaultControl.Name = "printDefaultControl";
            this.printDefaultControl.Size = new System.Drawing.Size(608, 584);
            this.printDefaultControl.TabIndex = 0;
            // 
            // folderDefaultControl
            // 
            this.folderDefaultControl.Location = new System.Drawing.Point(3, 3);
            this.folderDefaultControl.Name = "folderDefaultControl";
            this.folderDefaultControl.Size = new System.Drawing.Size(824, 650);
            this.folderDefaultControl.TabIndex = 0;
            // 
            // jobStorageDefaultControl
            // 
            this.jobStorageDefaultControl.Location = new System.Drawing.Point(10, 24);
            this.jobStorageDefaultControl.Name = "jobStorageDefaultControl";
            this.jobStorageDefaultControl.Size = new System.Drawing.Size(733, 517);
            this.jobStorageDefaultControl.TabIndex = 0;
            // 
            // scanToUsbDefaultControl
            // 
            this.scanToUsbDefaultControl.Location = new System.Drawing.Point(3, 3);
            this.scanToUsbDefaultControl.Name = "scanToUsbDefaultControl";
            this.scanToUsbDefaultControl.Size = new System.Drawing.Size(824, 593);
            this.scanToUsbDefaultControl.TabIndex = 0;
            // 
            // quickSetControl
            // 
            this.quickSetControl.Location = new System.Drawing.Point(7, 16);
            this.quickSetControl.Name = "quickSetControl";
            this.quickSetControl.Size = new System.Drawing.Size(817, 538);
            this.quickSetControl.TabIndex = 0;
            // 
            // protocolDefaultControl
            // 
            this.protocolDefaultControl.Location = new System.Drawing.Point(3, 3);
            this.protocolDefaultControl.Name = "protocolDefaultControl";
            this.protocolDefaultControl.Size = new System.Drawing.Size(663, 554);
            this.protocolDefaultControl.TabIndex = 0;
            // 
            // faxDefaults_Tab
            // 
            this.faxDefaults_Tab.Controls.Add(this.faxDefaultControl);
            this.faxDefaults_Tab.Location = new System.Drawing.Point(4, 24);
            this.faxDefaults_Tab.Name = "faxDefaults_Tab";
            this.faxDefaults_Tab.Size = new System.Drawing.Size(830, 649);
            this.faxDefaults_Tab.TabIndex = 11;
            this.faxDefaults_Tab.Text = "Fax Default";
            this.faxDefaults_Tab.UseVisualStyleBackColor = true;
            // 
            // faxDefaultControl
            // 
            this.faxDefaultControl.Location = new System.Drawing.Point(-6, 0);
            this.faxDefaultControl.Name = "faxDefaultControl";
            this.faxDefaultControl.Size = new System.Drawing.Size(831, 637);
            this.faxDefaultControl.TabIndex = 0;
            // 
            // DeviceInspectorConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TabbedSettingsTabControl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DeviceInspectorConfigurationControl";
            this.Size = new System.Drawing.Size(844, 683);
            this.deviceSelection_Tab.ResumeLayout(false);
            this.TimeLanguage_Settings.ResumeLayout(false);
            this.NotificationsTabPage.ResumeLayout(false);
            this.NotificationsTabPage.PerformLayout();
            this.TabbedSettingsTabControl.ResumeLayout(false);
            this.emailDefaults_Tab.ResumeLayout(false);
            this.copyDefaults_Tab.ResumeLayout(false);
            this.printDefaults_Tab.ResumeLayout(false);
            this.folderDefaults_Tab.ResumeLayout(false);
            this.jobDefaults_Tab.ResumeLayout(false);
            this.usbDefaults_Tab.ResumeLayout(false);
            this.quickSet_Tab.ResumeLayout(false);
            this.protocolDefaults_Tab.ResumeLayout(false);
            this.faxDefaults_Tab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Framework.UI.FieldValidator fieldValidator;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TabPage deviceSelection_Tab;
        private Framework.UI.AssetSelectionControl assetSelectionControl;
        private System.Windows.Forms.TabPage TimeLanguage_Settings;
        private System.Windows.Forms.TabPage NotificationsTabPage;
        private System.Windows.Forms.TabControl TabbedSettingsTabControl;
        private System.Windows.Forms.CheckBox enablePassword_CheckBox;
        private GeneralSettingsControl generalSettingsControl;
        private System.Windows.Forms.TabPage emailDefaults_Tab;
        private SettingsControls.EmailDefaultControl emailSettingsControl;
        private SettingsControls.PasswordWindowsControl passwordWindowsControl;
        private System.Windows.Forms.Label WarningLabel;
        private System.Windows.Forms.TabPage quickSet_Tab;
        private SettingsControls.QuickSetControl quickSetControl;
        private System.Windows.Forms.TabPage copyDefaults_Tab;
        private SettingsControls.CopyDefaultControl copyDefaultControl;
        private System.Windows.Forms.TabPage printDefaults_Tab;
        private SettingsControls.PrintDefaultControl printDefaultControl;
        private System.Windows.Forms.TabPage folderDefaults_Tab;
        private SettingsControls.FolderDefaultControl folderDefaultControl;
        private System.Windows.Forms.TabPage jobDefaults_Tab;
        private SettingsControls.JobStorageDefaultControl jobStorageDefaultControl;
        private System.Windows.Forms.TabPage usbDefaults_Tab;
        private SettingsControls.ScanToUsbDefaultControl scanToUsbDefaultControl;
        private System.Windows.Forms.TabPage protocolDefaults_Tab;
        private ProtocolDefaultControl protocolDefaultControl;
        private System.Windows.Forms.TabPage faxDefaults_Tab;
        private FaxDefaultControl faxDefaultControl;
    }
}
