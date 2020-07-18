using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading;
using HP.DeviceAutomation;
using HP.ScalableTest.Plugin.DSSConfiguration.UIMaps;
using HP.ScalableTest.Utility;
using TopCat.TestApi.Enums;
using TopCat.TestApi.GUIAutomation.Enums;
using System.Collections.Generic;
using HP.ScalableTest.DeviceAutomation;
using System.Windows.Forms;

namespace HP.ScalableTest.Plugin.DSSConfiguration
{
    /// <summary>
    /// Defines and implements the tasks to be performed by the DSSConfiguration activity.
    /// </summary>
    internal class DssConfigurationTask
    {
        private readonly string _ipAddress;
        private readonly string _sessionId;
        private readonly string _userName;
        private const int ShortTimeout = 5;
        private const int PromptTimeout = 10;
        private const int WindowTimeout = 40;
        private const int WaitTime = 2000;
        private const string DefaultCompanyName = "HP";
        private const string DefaultFaxNumber = "1234";
        private const string DefaultWorkFlowPosition = "1";
        private const string DefaultWorkFlowDescription = "Automation";
        private const string DefaultGroupName = "All Devices";
        private static string ScanType => "DSS";

        /// <summary>
        /// Initializes a new instance of the DSSConfigurationTask class.
        /// </summary>
        public DssConfigurationTask(string sessionId, string userName)
        {
            _sessionId = sessionId;
            _userName = userName;
            _ipAddress = GetLocalIpAddress();
        }

        #region Launch

        /// <summary>
        /// Launches the DSS Configuration Utility to first time configure the DSS Application
        /// </summary>
        [Description("Launches the configuration utility for the first time and adds the SMTP server details")]
        public void FirstLaunchApplication(DssEmailServer emailServer)
        {
            // Get the common desktop directory
            LaunchApp();

            DSSConfig_Prompt smtpPrompt = new DSSConfig_Prompt(_ipAddress);
            if (smtpPrompt.WaitForAvailable(PromptTimeout))
            {
                smtpPrompt.DSSPromptOKButton.Click(ShortTimeout);
                AddSmtpServer(emailServer);
            }
            smtpPrompt.HPDigitalSendinDup0Window.WaitForAvailable(WindowTimeout);
        }

        /// <summary>
        /// Launch the DSS Configuration Utility as and when required for performing further tasks
        /// </summary>
        [Description("Launches the configuration utility")]
        public void LaunchApplication()
        {
            // OnActivityStatusUpdate("Launching DSS Application ");
            // get the common desktop directory
            LaunchApp();

            DSSConfig_Prompt smtpPrompt = new DSSConfig_Prompt(_ipAddress);
            if (smtpPrompt.WaitForAvailable(ShortTimeout))
            {
                smtpPrompt.DSSPromptOKButton.PerformHumanAction(x => x.Click(ShortTimeout));
            }
            smtpPrompt.HPDigitalSendinDup0Window.WaitForAvailable(WindowTimeout);
        }

        private static void LaunchApp()
        {
            // get the common desktop directory
            var shortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory),
                "Configuration Utility.lnk");

            if (File.Exists(shortcut))
            {
                ProcessStartInfo processInfo = new ProcessStartInfo(shortcut)
                {
                    UseShellExecute = true,
                    Verb = "runas"
                };

                Process.Start(processInfo);
                Thread.Sleep(TimeSpan.FromSeconds(ShortTimeout));

                DSSConfig_LaunchApp launchApp = new DSSConfig_LaunchApp(UIAFramework.ManagedUIA);
                launchApp.HPDigitalSendinWindow.WaitForAvailable(WindowTimeout);
                launchApp.ThisComputerRadioButton.Select(ShortTimeout);
                launchApp.OKButton.Click(ShortTimeout);
            }
            else
            {
                throw new FileNotFoundException("Desktop Shortcut not found");
            }
        }

        #endregion Launch

        #region Email

        /// <summary>
        /// Configuring Email Server. Adds the SMTP Server in the Global Email Tab in DSS Server
        /// </summary>
        /// <param name="emailServer"></param>
        [Description("Specify SMTP server to be used")]
        public void AddSmtpServer(DssEmailServer emailServer)
        {
            DSSConfig_Email emailTab = new DSSConfig_Email(_ipAddress);
            if (!emailTab.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }
            emailTab.EmailTabItem.WaitForAvailable(WindowTimeout);
            emailTab.EmailTabItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));

            emailTab.AddSmtpGatewayButton.PerformHumanAction(x => x.Click(ShortTimeout));

            emailTab.AddSMTPGatewayWindow.WaitForAvailable(ShortTimeout);
            emailTab.SmtpServerTextBoxEdit.PerformHumanAction(x => x.EnterText(emailServer.ServerAddress, ShortTimeout));
            emailTab.SmtpPortTextBox.PerformHumanAction(x => x.EnterText(emailServer.Port.ToString(), ShortTimeout));

            if (emailServer.UseSsl)
            {
                emailTab.SmtpEnableSSLCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
            }
            else
            {
                emailTab.SmtpEnableSSLCheckBox.PerformHumanAction(x => x.Uncheck(ShortTimeout));
            }

            if (emailServer.RequireAuthentication)
            {
                if (!emailTab.SmtpRequiresAuthCheckBox.IsChecked(ShortTimeout))
                {
                    emailTab.SmtpRequiresAuthCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
                }

                if (emailServer.AlwaysUseCredentials)
                {
                    TopCatUiHelper.ComboBoxSetValue(emailTab.SmtpUseWhichCredentialsComboBox, SmtpAuthenticationType.AlwaysUse.GetDescription(), WindowTimeout); // CR:2525 Always Use control is added in activity data
                }
                emailTab.SmtpUsernameTextBox.PerformHumanAction(x => x.EnterText(emailServer.Credential.UserName));
                emailTab.SmtpPasswordBox.PerformHumanAction(x => x.EnterText(emailServer.Credential.Password));
            }

            if (emailServer.SplitSize > 0)
            {
                emailTab.SmtpFileSizeTextBox.PerformHumanAction(x => x.EnterText(emailServer.SplitSize.ToString(CultureInfo.InvariantCulture)));
            }

            emailTab.SmtpOKButton.WaitForEnabled(ShortTimeout);
            emailTab.SmtpOKButton.PerformHumanAction(x => x.Click(ShortTimeout));

            // Fixing CR:2519
            if (emailTab.HPDigitalSendinDup1Window.IsAvailable(ShortTimeout))
            {
                // This DSS prompt appears when the The Smtp Server to be added Already exists.
                // In this Scenario clear the Popup and Cancel adding Smtp Server
                emailTab.DSSPromptOKButton.PerformHumanAction(x => x.Click(ShortTimeout));
                emailTab.CancelButton132Button.PerformHumanAction(x => x.Click(ShortTimeout));
            }
            else
            {
                emailTab.ApplyButton.PerformHumanAction(x => x.Click(ShortTimeout));
            }
        }

        /// <summary>
        /// Configuring Email Settings for Specific Device
        /// </summary>
        /// <param name="emailServer"></param>
        [Description("Specify default email server to be used")]
        public void ConfigureEmailForDevice(EmailSetupforDevice emailServer)
        {
            var device = DeviceFactory.Create(emailServer.DeviceAddress, emailServer.DevicePassword);
            string modelName = device.GetDeviceInfo().ModelName;

            LaunchConfigureDeviceWindow(device.Address, modelName);

            DeviceEmailConfig emailConfig = new DeviceEmailConfig(_ipAddress);
            emailConfig.SetDeviceInfo(device.Address, modelName);

            emailConfig.DeviceEmailTabItem.WaitForAvailable(WindowTimeout);
            emailConfig.DeviceEmailTabItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));

            emailConfig.MaximizeButton.PerformHumanAction(x => x.ClickWithMouse(WindowTimeout));
            emailConfig.EnableSendtoEmailCheckBox.PerformHumanAction(x => x.Check(WindowTimeout));
            TopCatUiHelper.ComboBoxSetValue(emailConfig.ServiceTypeComboBox, FaxServiceType.ViaDss.GetDescription(), WindowTimeout);
            emailConfig.DefaultEmailTextBox.PerformHumanAction(x => x.EnterText(emailServer.DefaultEmail));

            if (emailServer.ScanToMe)
            {
                //Implementing Scan To Me(CR - 2780).               
                TopCatUiHelper.ComboBoxSetValue(emailConfig.ComboBox51181A4ComboBox, "User's address (sign-in required)", WindowTimeout);
                emailConfig.UsereditableCheDup1CheckBox.PerformHumanAction(x => x.Uncheck(WindowTimeout));
            }
            else
            {
                TopCatUiHelper.ComboBoxSetValue(emailConfig.ComboBox51181A4ComboBox, "Blank", WindowTimeout);
                emailConfig.UsereditableCheDup1CheckBox.PerformHumanAction(x => x.Check(WindowTimeout));
            }

            emailConfig.UsereditableCheDup6CheckBox.PerformHumanAction(x => x.Check(WindowTimeout));
            TopCatUiHelper.ComboBoxSetValue(emailConfig.ComboBoxA6DF22FComboBox, emailServer.Signing.GetDescription(), ShortTimeout);

            emailConfig.UsereditableCheDup7CheckBox.PerformHumanAction(x => x.Check(WindowTimeout));
            TopCatUiHelper.ComboBoxSetValue(emailConfig.ComboBoxC331B21ComboBox, emailServer.Encryption.GetDescription(), ShortTimeout);

            //Notification Settings
            ConfigureNotificationSettings(emailServer.NotificationSettings, emailConfig);
            //Scan Settings
            ConfigureScanSettingsForDevice(emailServer.ScanSettings, emailConfig);
            //Enabling or Disabling Job Build
            if (emailServer.JobBuild)
            {
                emailConfig.JobBuildCheckBoCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
            }
            else
            {
                emailConfig.JobBuildCheckBoCheckBox.PerformHumanAction(x => x.Uncheck(ShortTimeout));
            }

            //File Settings
            ConfigureFileSettingForDevice(emailServer.FileSettings, emailConfig);
            //Enabling or Disabling Blank Page Suppression.
            if (emailServer.BlankPageSuppression)
            {
                emailConfig.EnableBlankPageCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
            }
            else
            {
                emailConfig.EnableBlankPageCheckBox.PerformHumanAction(x => x.Uncheck(ShortTimeout));
            }

            if (emailConfig.OKButton.IsEnabled(ShortTimeout))
            {
                emailConfig.OKButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            }
            else
            {
                throw new ArgumentException("Please Check the Configurations.Apply button is not enabled due to Some error in Configuration/Addressing is already configured");
            }

            if (emailConfig.YesButtonAE4B0CButton.IsAvailable(PromptTimeout))
            {
                emailConfig.YesButtonAE4B0CButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            }

        }

        /// <summary>
        /// Configuring Scan Settings For device
        /// </summary>  
        /// <param name="dssScanSetting"></param>
        /// <param name="deviceConfig"></param>
        private static void ConfigureScanSettingsForDevice(DssScanSetting dssScanSetting, DeviceEmailConfig deviceConfig)
        {
            if (dssScanSetting.OriginalSize == SizeSetting.Ledger || dssScanSetting.OriginalSize == SizeSetting.A3 || dssScanSetting.OriginalSize == SizeSetting.B4)
            {
                throw new Exception("The option is not avalaiable.");
            }

            TopCatUiHelper.ComboBoxSetValue(deviceConfig.ComboBox1ed9470ComboBox, dssScanSetting.OriginalSize == SizeSetting.Oficio ? "Oficio (216x340 mm)" : dssScanSetting.OriginalSize.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(deviceConfig.ComboBox0bf3c30ComboBox, dssScanSetting.OriginalSides.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(deviceConfig.ComboBox7a26f01ComboBox, dssScanSetting.Optimize.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(deviceConfig.ComboBoxbec0157ComboBox, dssScanSetting.Orientation.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(deviceConfig.ComboBoxd01f295ComboBox, dssScanSetting.BackgroundCleanup.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(deviceConfig.ComboBox8bdd792ComboBox, dssScanSetting.Sharpness.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(deviceConfig.ComboBoxd4a708cComboBox, dssScanSetting.Darkness.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(deviceConfig.ComboBoxd702915ComboBox, dssScanSetting.Contrast.GetDescription(), ShortTimeout);
        }

        /// <summary>
        /// Configuring Notification
        /// </summary>
        /// <param name="notificationSettings"></param>
        /// <param name="deviceConfig"></param>
        private static void ConfigureNotificationSettings(DssNotificationSettings notificationSettings, DeviceEmailConfig deviceConfig)
        {
            if (notificationSettings.NotificationCondition != NotifyCondition.DoNotNotify)
            {
                TopCatUiHelper.ComboBoxSetValue(deviceConfig.ComboBoxacd621dComboBox, notificationSettings.NotificationCondition.GetDescription(), ShortTimeout);
                TopCatUiHelper.ComboBoxSetValue(deviceConfig.ComboBox116120eComboBox, notificationSettings.NotificationDeliveryMethod.GetDescription(), ShortTimeout);

                if (notificationSettings.NotificationDeliveryMethod == NotifyMethod.Email)
                {
                    deviceConfig.TextBox23C3F3A3Edit.PerformHumanAction(x => x.EnterText(notificationSettings.NotificationEmail, ShortTimeout));
                }
                if (notificationSettings.IncludeThumbnail)
                {
                    deviceConfig.IncludeThumbnaiCheckBox.PerformHumanAction(x => x.Check(WindowTimeout));
                }
                else
                {
                    deviceConfig.IncludeThumbnaiCheckBox.PerformHumanAction(x => x.Uncheck(WindowTimeout));
                }
            }
            else
            {
                TopCatUiHelper.ComboBoxSetValue(deviceConfig.ComboBoxacd621dComboBox, NotifyCondition.DoNotNotify.GetDescription(), ShortTimeout);
            }
        }

        /// <summary>
        /// Configuring File Settings
        /// </summary>
        /// <param name="fileSetting"></param>
        /// <param name="deviceConfig"></param>  
        private static void ConfigureFileSettingForDevice(DssFileSetting fileSetting, DeviceEmailConfig deviceConfig)
        {
            TopCatUiHelper.ComboBoxSetValue(deviceConfig.ComboBox8936adcComboBox, fileSetting.ColorPreference.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(deviceConfig.ComboBoxb02f84eComboBox, fileSetting.OutputQuality.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(deviceConfig.ComboBox11bcd8aComboBox, fileSetting.FileType.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(deviceConfig.ComboBox7dbab3fComboBox, fileSetting.HighCompression ? "High" : "Normal", ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(deviceConfig.ComboBoxfea42eaComboBox, fileSetting.FileType == FileType.Pdf && fileSetting.HighCompression ? Resolution.Dpi300.GetDescription() : fileSetting.Resolution.GetDescription(), ShortTimeout);

            if (fileSetting.FileType == FileType.Pdf)
            {
                if (fileSetting.PdfEncryption)
                {
                    deviceConfig.PDFEncryptionChCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
                }
                else
                {
                    deviceConfig.PDFEncryptionChCheckBox.PerformHumanAction(x => x.Uncheck(ShortTimeout));
                }
            }
        }

        private void LaunchConfigureDeviceWindow(string address, string modelName)
        {
            DSSConfig_DeviceConfig deviceConfigTab = new DSSConfig_DeviceConfig(_ipAddress);
            deviceConfigTab.SetDeviceInfo(address, modelName);

            if (!deviceConfigTab.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }

            // traverse to Device Configuration Tab
            if (deviceConfigTab.WaitForAvailable(WindowTimeout))
            {
                deviceConfigTab.DeviceConfiguraTabItem.PerformHumanAction(x => x.Select(ShortTimeout));
                // Get the Device List Item  Based on the Device IP address
                if (deviceConfigTab.ListView141cb8bDataGrid.RowCount() > 0)
                {
                    if (deviceConfigTab.SelectedDeviceDataItem != null)
                    {
                        deviceConfigTab.SelectedDeviceDataItem.PerformHumanAction(x => x.Select(ShortTimeout));
                        // Launch Device Configuration
                        deviceConfigTab.SelectedDeviceDataItem.PerformHumanAction(
                            x => x.ClickWithMouse(MouseButton.DoubleLeft, ShortTimeout));
                    }
                    else
                    {
                        throw new DeviceInvalidOperationException("The device is not managed by this DSS server");
                    }
                }
            }
        }

        #endregion Email

        #region BackupRestore

        /// <summary>
        /// Backing up DSS Configurations
        /// </summary>
        [Description("Does a backup of configuration with optional encryption")]
        public void BackupDss(DssBackupFile backup, bool cancelBackup)
        {
            DSSConfig_General generalTab = new DSSConfig_General(_ipAddress);
            if (generalTab.WaitForAvailable(WindowTimeout))
            {
                generalTab.GeneralTabItem.PerformHumanAction(x => x.Select(ShortTimeout));
                generalTab.BackupButton.PerformHumanAction(x => x.Click(ShortTimeout));

                DSSConfig_Backup backupPrompt = new DSSConfig_Backup(_ipAddress);
                if (backupPrompt.WaitForAvailable(WindowTimeout))
                {
                    File.Delete(backup.FileName);
                    backupPrompt.BrowseButton.PerformHumanAction(x => x.Click(ShortTimeout));
                    backupPrompt.FilenameAppContComboBox.PerformHumanAction(x => x.SetText(backup.FileName, ShortTimeout));
                    backupPrompt.SaveButton1Button.PerformHumanAction(x => x.Click(ShortTimeout));

                    if (!string.IsNullOrEmpty(backup.EncryptionKey) && !backup.EncryptionKey.Equals("Empty", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!backupPrompt.EncryptBackupCheckBox.IsChecked(ShortTimeout))
                        {
                            backupPrompt.EncryptBackupCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
                        }
                        backupPrompt.EncryptionKeyTextBoxEdit.PerformHumanAction(x => x.EnterText(backup.EncryptionKey, ShortTimeout));
                    }
                    else
                    {
                        if (backupPrompt.EncryptBackupCheckBox.IsChecked(ShortTimeout))
                        {
                            backupPrompt.EncryptBackupCheckBox.PerformHumanAction(x => x.Uncheck(ShortTimeout));
                        }
                    }
                    if (backupPrompt.BackupOKButton.IsEnabled(ShortTimeout))
                    {
                        backupPrompt.BackupOKButton.PerformHumanAction(x => x.Click(ShortTimeout));

                        if (cancelBackup)
                        {
                            DSSConfig_PleaseWait pleaseWaitPrompt = new DSSConfig_PleaseWait(_ipAddress);
                            pleaseWaitPrompt.HPDigitalCancelButton.WaitForAvailable(ShortTimeout);
                            pleaseWaitPrompt.HPDigitalCancelButton.PerformHumanAction(x => x.Click(ShortTimeout));
                        }
                    }
                    else
                    {
                        throw new FileLoadException("Backup process failed, please check the screen for further information");
                    }
                }
            }
        }

        /// <summary>
        /// Restoring the DSS Configuration based on the Restore File
        /// </summary>
        [Description("Does a restore of a previous backup")]
        public void RestoreDss(DssRestoreFile restoreFile)
        {
            DSSConfig_General generalTab = new DSSConfig_General(_ipAddress);
            if (generalTab.WaitForAvailable(WindowTimeout))
            {
                generalTab.GeneralTabItem.PerformHumanAction(x => x.Select(ShortTimeout));
                generalTab.RestoreButton.PerformHumanAction(x => x.Click(ShortTimeout));

                DSSConfig_Restore restore = new DSSConfig_Restore(_ipAddress);
                restore.WaitForAvailable(ShortTimeout);

                restore.FilenameEdit114Edit.PerformHumanAction(x => x.EnterText(restoreFile.BackupFile.FileName));
                restore.OpenButton1Button.PerformHumanAction(x => x.Click(ShortTimeout));

                if (!string.IsNullOrEmpty(restoreFile.BackupFile.EncryptionKey) && !restoreFile.BackupFile.EncryptionKey.Equals("Empty", StringComparison.OrdinalIgnoreCase))
                {
                    if (restore.EncryptionKeyTextBoxEdit.IsVisible(ShortTimeout))
                    {
                        restore.EncryptionKeyTextBoxEdit.PerformHumanAction(x => x.EnterText(restoreFile.BackupFile.EncryptionKey));
                    }
                }
                else
                {
                    DSSConfig_CustomMessageBox customMessageBox = new DSSConfig_CustomMessageBox(_ipAddress);
                    if (customMessageBox.HPDigitalCustomMessageWindow.IsVisible(ShortTimeout))
                    {
                        customMessageBox.HPDigitalYesButton.PerformHumanAction(x => x.Click(ShortTimeout));
                    }
                    else
                    {
                        //there is a chance that the user has not entered encryption key but has selected encrypted file for restoration, confirm and then throw exception
                        if (restore.ThisbackupfiledText.IsVisible(ShortTimeout))
                        {
                            throw new FileLoadException("Backup file is encrypted, Please select encryption key and try again");
                        }
                    }
                }

                DssRestoreTabs(restoreFile, restore);
                RestoreMergeSetting(restoreFile, restore);
                restore.RestoreOKButton.Click(ShortTimeout);

                restore.DSSConfiguratioPane.WaitForAvailable(WindowTimeout);

                restore.OKButtonAE4B0C6Button.PerformHumanAction(x => x.Click(ShortTimeout));

                DSSConfig_LaunchApp launchApp = new DSSConfig_LaunchApp(UIAFramework.ManagedUIA);
                launchApp.HPDigitalSendinWindow.WaitForAvailable(60);
                Thread.Sleep(WaitTime);
                launchApp.ThisComputerRadioButton.PerformHumanAction(x => x.Select(ShortTimeout));
                launchApp.OKButton.PerformHumanAction(x => x.Click(ShortTimeout));
                generalTab.WaitForAvailable(WindowTimeout);
            }
        }

        /// <summary>
        /// Restoring Merge Settings
        /// </summary>
        /// <param name="restoreFile"></param>
        /// <param name="restore"></param>
        private static void RestoreMergeSetting(DssRestoreFile restoreFile, DSSConfig_Restore restore)
        {
            if (restoreFile.MergeTabsList.Count > 0)
            {
                foreach (var dssTabItem in restoreFile.MergeTabsList)
                {
                    switch (dssTabItem)
                    {
                        case DssTabs.SendToFolder:
                            {
                                if (!restore.MergeSendToFoldercheckBox.IsChecked(ShortTimeout))
                                {
                                    restore.MergeSendToFoldercheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
                                }
                            }
                            break;

                        case DssTabs.Workflows:
                            {
                                if (!restore.MergeWorkflowcheckBox.IsChecked(ShortTimeout))
                                {
                                    restore.MergeWorkflowcheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
                                }
                            }
                            break;

                        case DssTabs.Templates:
                            {
                                if (!restore.MergeTemplatecheckBox.IsChecked(ShortTimeout))
                                {
                                    restore.MergeTemplatecheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
                                }
                            }
                            break;

                        case DssTabs.Email:
                            {
                                if (!restore.MergeEmailcheckBox.IsChecked(ShortTimeout))
                                {
                                    restore.MergeEmailcheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
                                }
                            }
                            break;

                        case DssTabs.AddressBook:
                            {
                                if (!restore.MergeAddressbookcheckBox.IsChecked(ShortTimeout))
                                {
                                    restore.MergeAddressbookcheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
                                }
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Restore DSS Tabs
        /// </summary>
        /// <param name="restoreFile"></param>
        /// <param name="restore"></param>
        private static void DssRestoreTabs(DssRestoreFile restoreFile, DSSConfig_Restore restore)
        {
            if (restoreFile.RestoreTabsList.Count == 0)
            {
                if (!restore.FullRestoreRadioButton.IsSelected(ShortTimeout))
                {
                    restore.FullRestoreRadioButton.PerformHumanAction(x => x.Select(ShortTimeout));
                }
            }
            else
            {
                if (!restore.SelectiveRestoreRadioButton.IsSelected(ShortTimeout))
                {
                    restore.SelectiveRestoreRadioButton.PerformHumanAction(x => x.Select(ShortTimeout));
                }

                foreach (var tabItem in restoreFile.RestoreTabsList)
                {
                    switch (tabItem)
                    {
                        case DssTabs.General:
                            {
                                if (!restore.GeneralSettingsCheckbox.IsChecked(ShortTimeout))
                                {
                                    restore.GeneralSettingsCheckbox.PerformHumanAction(x => x.Check(ShortTimeout));
                                }
                            }
                            break;

                        case DssTabs.AddressBook:
                            {
                                if (!restore.AddressbookCheckbox.IsChecked(ShortTimeout))
                                {
                                    restore.AddressbookCheckbox.PerformHumanAction(x => x.Check(ShortTimeout));
                                }
                            }
                            break;

                        case DssTabs.Addressing:
                            {
                                if (!restore.AddressingCheckbox.IsChecked(ShortTimeout))
                                {
                                    restore.AddressingCheckbox.PerformHumanAction(x => x.Check(ShortTimeout));
                                }
                            }
                            break;

                        case DssTabs.Authentication:
                            {
                                if (!restore.AuthenticationCheckbox.IsChecked(ShortTimeout))
                                {
                                    restore.AuthenticationCheckbox.PerformHumanAction(x => x.Check(ShortTimeout));
                                }
                            }
                            break;

                        case DssTabs.Devices:
                            {
                                if (!restore.DeviceCheckbox.IsChecked(ShortTimeout))
                                {
                                    restore.DeviceCheckbox.PerformHumanAction(x => x.Check(ShortTimeout));
                                }
                            }
                            break;

                        case DssTabs.Email:
                            {
                                if (!restore.EmailCheckbox.IsChecked(ShortTimeout))
                                {
                                    restore.EmailCheckbox.PerformHumanAction(x => x.Check(ShortTimeout));
                                }
                            }
                            break;

                        case DssTabs.Fax:
                            {
                                if (!restore.FaxCheckbox.IsChecked(ShortTimeout))
                                {
                                    restore.FaxCheckbox.PerformHumanAction(x => x.Check(ShortTimeout));
                                }
                            }
                            break;

                        case DssTabs.SendToFolder:
                            {
                                if (!restore.SendFolderCheckbox.IsChecked(ShortTimeout))
                                {
                                    restore.SendFolderCheckbox.PerformHumanAction(x => x.Check(ShortTimeout));
                                }
                            }
                            break;

                        case DssTabs.Templates:
                            {
                                if (!restore.TemplateCheckbox.IsChecked(ShortTimeout))
                                {
                                    restore.TemplateCheckbox.PerformHumanAction(x => x.Check(ShortTimeout));
                                }
                            }
                            break;

                        case DssTabs.Workflows:
                            {
                                if (!restore.WorkflowCheckbox.IsChecked(ShortTimeout))
                                {
                                    restore.WorkflowCheckbox.PerformHumanAction(x => x.Check(ShortTimeout));
                                }
                            }
                            break;
                    }
                }
            }
        }

        #endregion BackupRestore

        #region Authentication

        /// <summary>
        /// Configuring the LDAP Authentication in DSS Global Authentication Tab
        /// </summary>
        /// <param name="ldap"></param>
        [Description("Configures LDAP server for authentication")]
        public void ConfigureLdapAuthentication(DssAuthenticationLdap ldap)
        {
            DSS_Authentication_Global ldapAuthentication = new DSS_Authentication_Global(_ipAddress);

            if (!ldapAuthentication.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }

            ldapAuthentication.AuthenticationTabItem.WaitForAvailable(WindowTimeout);
            ldapAuthentication.AuthenticationTabItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));

            TopCatUiHelper.ComboBoxSetValue(ldapAuthentication.AuthenticationTypeComboBox, AuthenticationType.Ldap.GetDescription(), ShortTimeout);

            ldapAuthentication.DomainServerTextBox.PerformHumanAction(x => x.EnterText(ldap.DomainServerAddress));
            ldapAuthentication.PortNumberTextBox.PerformHumanAction(x => x.EnterText(ldap.PortNumber.ToString()));

            ldapAuthentication.BindPrefixTextBox.PerformHumanAction(x => x.EnterText(ldap.BindPrefix));
            ldapAuthentication.BindandSearchTextBox.PerformHumanAction(x => x.EnterText(ldap.BindandSearch));

            ldapAuthentication.MatchAttributeTextBox.PerformHumanAction(x => x.EnterText(ldap.MatchAttribute));
            ldapAuthentication.EmailAttributeTextBox.PerformHumanAction(x => x.EnterText(ldap.EmailAttribute));

            ldapAuthentication.UserNameAttributeTextBox.PerformHumanAction(x => x.EnterText(ldap.UserNameAttribute));
            ldapAuthentication.UserGroupAttributeTextBox.PerformHumanAction(x => x.EnterText(ldap.UserGroupAttribute));

            ldapAuthentication.UserNameTextBox.PerformHumanAction(x => x.EnterText(ldap.UserName));
            ldapAuthentication.LDAPPasswordBox.PerformHumanAction(x => x.EnterText(ldap.Password));

            if (ldapAuthentication.LDAPTestButton.IsEnabled(ShortTimeout))
            {
                ldapAuthentication.LDAPTestButton.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
                // Use the pleasewait prompt and then continue as it is, else if the please wait prompt
                // goes on for more than 10 seconds we will end up with failure
                PauseForPleaseWait();
            }
            else
            {
                // If the User specified Credentials doest match the format test button is not enabled
                throw new InvalidCredentialException("Check Domain Credentials. Test Button is not Enabled");
            }

            ldapAuthentication.GlobalApplyButton.PerformHumanAction(x => x.Click(ShortTimeout));
        }

        /// <summary>
        /// Configuring the Windows Authntication in DSS Global Authentication Tab
        /// </summary>
        /// <param name="windowsAuthentication"></param>
        [Description("Configures Windows Authentication")]
        public void ConfigureWindowsAuthentication(DssAuthenticationWindows windowsAuthentication)
        {
            DSS_Authentication_Global windowsAuthenticationGlobal = new DSS_Authentication_Global(_ipAddress);
            if (!windowsAuthenticationGlobal.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }

            windowsAuthenticationGlobal.AuthenticationTabItem.WaitForAvailable(WindowTimeout);
            windowsAuthenticationGlobal.AuthenticationTabItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));

            TopCatUiHelper.ComboBoxSetValue(windowsAuthenticationGlobal.AuthenticationTypeComboBox, AuthenticationType.Windows.GetDescription(), ShortTimeout);

            windowsAuthenticationGlobal.DomainTextBox.PerformHumanAction(x => x.EnterText(windowsAuthentication.Domain));
            windowsAuthenticationGlobal.AddServerButton.PerformHumanAction(x => x.Click(ShortTimeout));
            //Fixing CR-2611
            TopCatUiHelper.ComboBoxSetValue(windowsAuthenticationGlobal.ComboBoxbd29c3eComboBox, windowsAuthentication.Domain, ShortTimeout);

            windowsAuthenticationGlobal.WindowsUserNameTextBox.PerformHumanAction(x => x.EnterText(windowsAuthentication.UserName));
            windowsAuthenticationGlobal.WindowsPasswordBox.PerformHumanAction(x => x.EnterText(windowsAuthentication.Password));

            if (windowsAuthenticationGlobal.WindowsTestButton.IsEnabled(ShortTimeout))
            {
                windowsAuthenticationGlobal.WindowsTestButton.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
                PauseForPleaseWait();
            }
            else
            {
                // If the User specified Credentials doest match the format test button is not enabled
                throw new InvalidCredentialException("Check Domain Credentials. Test Button is not Enabled");
            }
            windowsAuthenticationGlobal.GlobalApplyButton.PerformHumanAction(x => x.Click(ShortTimeout));
        }

        /// <summary>
        /// Configuring Device authentication
        /// </summary>
        /// <param name="ldap"></param>
        [Description("Configure device for LDAP authentication ")]
        public void ConfigureDeviceLdapAuthentication(DssAuthenticationLdap ldap)
        {
            var device = DeviceFactory.Create(ldap.DeviceAddress, ldap.DevicePassword);
            string modelName = device.GetDeviceInfo().ModelName;

            LaunchConfigureDeviceWindow(device.Address, modelName);

            DSSConfig_Authentication authDeviceConfig = new DSSConfig_Authentication(_ipAddress);
            authDeviceConfig.SetDeviceInfo(ldap.DeviceAddress, modelName);
            authDeviceConfig.ConfigureDeviceWindow.WaitForAvailable(WindowTimeout);

            // Progress bar, wait till it dissapear
            while (authDeviceConfig.AuthenticationTabProgressBar.IsVisible(ShortTimeout))
            {
                //Do Nothing.Until progress bar is not seen
            }
            authDeviceConfig.AuthenticationTabItem.PerformHumanAction(x => x.Select(ShortTimeout));
            if (authDeviceConfig.ErrorTextBlockText.IsVisible(ShortTimeout))
            {
                throw new InvalidCredentialException(authDeviceConfig.ErrorTextBlockText.Name());
            }
            authDeviceConfig.LDAPSignInSetupGroup.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            authDeviceConfig.EnableLDAPSignICheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
            authDeviceConfig.LDAPServerAddress.PerformHumanAction(x => x.EnterText(ldap.DomainServerAddress));
            authDeviceConfig.PortNumberTextBox.PerformHumanAction(x => x.EnterText(ldap.PortNumber.ToString()));
            authDeviceConfig.BindPrefixTextBox.PerformHumanAction(x => x.EnterText(ldap.BindPrefix));
            authDeviceConfig.BindAndSearchRootTextBox.PerformHumanAction(x => x.EnterText(ldap.BindandSearch));
            authDeviceConfig.MatchTheNameTextBox.PerformHumanAction(x => x.EnterText(ldap.MatchAttribute));
            authDeviceConfig.RetrieveUserEMail.PerformHumanAction(x => x.EnterText(ldap.EmailAttribute));
            authDeviceConfig.RetrieveDeviceUserName.PerformHumanAction(x => x.EnterText(ldap.UserNameAttribute));
            authDeviceConfig.RetrieveDeviceUserGroup.PerformHumanAction(x => x.EnterText(ldap.UserGroupAttribute));
            authDeviceConfig.UserNameTextBox.PerformHumanAction(x => x.EnterText(ldap.UserName));
            authDeviceConfig.PasswordTextBox.PerformHumanAction(x => x.EnterText(ldap.Password));

            if (authDeviceConfig.LDAPTestButton.IsEnabled(ShortTimeout))
            {
                authDeviceConfig.LDAPTestButton.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
                // Use the pleasewait prompt and then continue as it is, else if the please wait prompt
                // goes on for more than 10 seconds we will end up with failure
                PauseForPleaseWait();
            }

            if (authDeviceConfig.ApplyButtonFinal.IsEnabled(ShortTimeout))
            {
                authDeviceConfig.ApplyButtonFinal.PerformHumanAction(x => x.Click(ShortTimeout));
            }
            else
            {
                throw new ArgumentException("Please check the configurations.Apply button is not enabled due to some error in the Configuration/LdapAuthentication is already configured");
            }

            if (authDeviceConfig.YesButtonAE4B0CButton.IsAvailable(PromptTimeout))
            {
                authDeviceConfig.YesButtonAE4B0CButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            }

            PauseForPleaseWait();

            if (authDeviceConfig.OKButton8c9182aButton.IsAvailable(PromptTimeout))
            {
                authDeviceConfig.OKButton8c9182aButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            }
        }

        /// <summary>
        /// Configuring Device authentication
        /// </summary>
        /// <param name="windowsAuthentication"></param>
        [Description("Configures device for Windows Authentication ")]
        public void ConfigureDeviceWindowsAuthentication(DssAuthenticationWindows windowsAuthentication)
        {
            var device = DeviceFactory.Create(windowsAuthentication.DeviceAddress, windowsAuthentication.DevicePassword);
            string modelName = device.GetDeviceInfo().ModelName;

            LaunchConfigureDeviceWindow(device.Address, modelName);

            DSSConfig_Authentication authDeviceConfig = new DSSConfig_Authentication(_ipAddress);
            authDeviceConfig.SetDeviceInfo(windowsAuthentication.DeviceAddress, modelName);
            authDeviceConfig.ConfigureDeviceWindow.WaitForAvailable(WindowTimeout);

            // Progress bar, wait till it dissapear
            while (authDeviceConfig.AuthenticationTabProgressBar.IsVisible(ShortTimeout))
            {
                //Do Nothing.Until progress bar is not seen
            }

            authDeviceConfig.AuthenticationTabItem.PerformHumanAction(x => x.Select(ShortTimeout));
            if (authDeviceConfig.ErrorTextBlockText.IsVisible(ShortTimeout))
            {
                throw new InvalidCredentialException(authDeviceConfig.ErrorTextBlockText.Name());
            }
            authDeviceConfig.WindowsSignInSeGroup.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            authDeviceConfig.TrustedDomainEditBox.PerformHumanAction(x => x.EnterText(windowsAuthentication.Domain));
            authDeviceConfig.TrustedDomainAddButton.PerformHumanAction(x => x.Click(ShortTimeout));
            authDeviceConfig.WindowsUserName.PerformHumanAction(x => x.EnterText(windowsAuthentication.UserName));
            authDeviceConfig.WindowsPasswordEditBox.PerformHumanAction(x => x.EnterText(windowsAuthentication.Password));

            if (authDeviceConfig.WindowsTestButton.IsEnabled(ShortTimeout))
            {
                authDeviceConfig.WindowsTestButton.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
                PauseForPleaseWait();
            }
            else
            {
                // If the User specified Credentials doest match the format test button is not enabled
                throw new InvalidCredentialException("Check Domain Credentials. Test Button is not Enabled");
            }

            if (authDeviceConfig.ApplyButtonFinal.IsEnabled(ShortTimeout))
            {
                authDeviceConfig.ApplyButtonFinal.PerformHumanAction(x => x.Click(ShortTimeout));
            }
            else
            {
                throw new ArgumentException("Please check the configurations.Apply button is not enabled due to some error in the Configuration/WindowsAuthentication is already configured");
            }

            if (authDeviceConfig.YesButtonAE4B0CButton.IsAvailable(PromptTimeout))
            {
                authDeviceConfig.YesButtonAE4B0CButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            }

            PauseForPleaseWait();

            if (authDeviceConfig.OKButton8c9182aButton.IsAvailable(PromptTimeout))
            {
                authDeviceConfig.OKButton8c9182aButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            }
        }

        #endregion Authentication

        #region AddDevice

        /// <summary>
        /// Adds the Device to DSS Server
        /// </summary>
        /// <param name="deviceConfiguration"></param>
        [Description("Adds the device to DSS")]
        public void AddDevice(DssDeviceConfiguration deviceConfiguration)
        {
            DSSConfig_DeviceConfig deviceConfigTab = new DSSConfig_DeviceConfig(_ipAddress);
            if (!deviceConfigTab.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }
            deviceConfigTab.DeviceConfiguraTabItem.WaitForAvailable(WindowTimeout);
            deviceConfigTab.DeviceConfiguraTabItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));

            // select group to add device
            if (deviceConfiguration.GroupName.Equals(DefaultGroupName, StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrEmpty(deviceConfiguration.GroupName))
            {
                deviceConfigTab.AllDevicesTreeVTreeItem.Select(ShortTimeout);
            }
            else
            {
                deviceConfigTab.AllDevicesTreeVTreeItem.Select(ShortTimeout);
                deviceConfigTab.AddNewGroupName(deviceConfiguration.GroupName);

                if (!deviceConfigTab.GroupNameTreeItem.IsVisible(ShortTimeout))
                {
                    deviceConfigTab.AddGroupButtonFButton.PerformHumanAction(x => x.Click(ShortTimeout));
                    deviceConfigTab.GroupNameEditTextBox.PerformHumanAction(x => x.EnterText(deviceConfiguration.GroupName, ShortTimeout));
                    deviceConfigTab.DeviceConfigurationGroupWindow.ClickWithMouse(ShortTimeout);
                    deviceConfigTab.OkGroupNameButton.PerformHumanAction(x => x.Click(ShortTimeout));
                    deviceConfigTab.AllDevicesTreeVTreeItem.PerformHumanAction(x => x.Select(ShortTimeout));
                }
            }

            var device = DeviceFactory.Create(deviceConfiguration.DeviceAddress, deviceConfiguration.DevicePassword);
            deviceConfigTab.SetDeviceInfo(device.Address, device.GetDeviceInfo().ModelName);
            deviceConfigTab.ContextMenuItem.SetGroupName(deviceConfiguration.GroupName);

            // first check if the device already exists in the device list
            if (deviceConfigTab.SelectedDeviceDataItem == null)
            {
                deviceConfigTab.AddDeviceButtonButton.PerformHumanAction(x => x.Click(ShortTimeout));

                DSSConfig_AddDevice addMfpDevice = new DSSConfig_AddDevice(_ipAddress);
                if (addMfpDevice.WaitForAvailable(WindowTimeout))
                {
                    addMfpDevice.HostNameComboBox.PerformHumanAction(x => x.SetText(deviceConfiguration.DeviceAddress, ShortTimeout));
                    addMfpDevice.AddDevice1Button.PerformHumanAction(x => x.Click(ShortTimeout));

                    DSSConfig_AddDeviceAdminPrompt adminPrompt = new DSSConfig_AddDeviceAdminPrompt(_ipAddress);
                    if (adminPrompt.WaitForAvailable(WindowTimeout))
                    {
                        // enter the credentials...
                        adminPrompt.DeviceUserNameTextBox.PerformHumanAction(x => x.EnterText(deviceConfiguration.DeviceUserName, ShortTimeout));
                        adminPrompt.DevicePasswordBox.PerformHumanAction(x => x.EnterText(deviceConfiguration.DevicePassword, ShortTimeout));
                        adminPrompt.DeviceOKButton.PerformHumanAction(x => x.Click(ShortTimeout));
                    }

                    DSSConfig_PleaseWait pleaseWaitWindow = new DSSConfig_PleaseWait(_ipAddress);
                    DSSConfig_AddDevice_Reclaim reclaimWindow = new DSSConfig_AddDevice_Reclaim(_ipAddress);
                    while (pleaseWaitWindow.HPDigitalPleaseWaitWindow.IsVisible(ShortTimeout))
                    {
                        if (reclaimWindow.HPDigitalSendinDup1Window.IsVisible(ShortTimeout))
                        {
                            //this device is managed by other DSS Server, let's not reclaim it
                            var errorMessage =
                                $"The device {deviceConfiguration.DeviceAddress} is managed by other server: {reclaimWindow.A157723170TextEText.Name(ShortTimeout)}. The device cannot be added.";

                            reclaimWindow.NoButton938CF5BButton.PerformHumanAction(x => x.Click(ShortTimeout));
                            throw new DeviceInvalidOperationException(errorMessage);
                        }
                        // do nothing. Waiting till the please wait Window closes
                    }

                    if (addMfpDevice.HPDigitalCustomMessageWindow.IsVisible(ShortTimeout))
                    {
                        addMfpDevice.OKButtonAE4B0C6Button.PerformHumanAction(x => x.Click(ShortTimeout));
                        addMfpDevice.CancelButton50BButton.PerformHumanAction(x => x.Click(ShortTimeout));
                        throw new DeviceInvalidOperationException("Unable to add the device.");
                    }

                    addMfpDevice.OKButtonA1ABC07Button.PerformHumanAction(x => x.Click(ShortTimeout));

                    while (pleaseWaitWindow.HPDigitalPleaseWaitWindow.IsVisible(ShortTimeout))
                    {
                        // do nothing until the please wait window disappers
                    }
                }
            }
            if (deviceConfigTab.SelectedDeviceDataItem != null)
            {
                if (!deviceConfiguration.GroupName.Equals(DefaultGroupName, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(deviceConfiguration.GroupName))
                {
                    deviceConfigTab.SelectedDeviceDataItem.PerformHumanAction(x => x.Select(ShortTimeout));
                    deviceConfigTab.SelectedDeviceDataItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Right, ShortTimeout));

                    deviceConfigTab.ContextMenuItem.AddtoGroupMenuItem.PerformHumanAction(x => x.Expand(ShortTimeout));
                    if (deviceConfigTab.ContextMenuItem.GroupNameMenuItem.IsVisible(ShortTimeout))
                    {
                        deviceConfigTab.ContextMenuItem.GroupNameMenuMenuItem.PerformHumanAction(x => x.Click(ShortTimeout));
                    }
                }
            }
            else
            {
                throw new DeviceInvalidOperationException("Unable to add the device");
            }
        }

        #endregion AddDevice

        #region TemplateConfiguration

        /// <summary>
        /// Apply Template to Specific Device
        /// </summary>
        /// <param name="deviceConfiguration"></param>
        [Description("Applies an existing template to the device")]
        public void ApplyTemplate(DssDeviceConfiguration deviceConfiguration)
        {
            DSSConfig_DeviceConfig deviceConfigTab = new DSSConfig_DeviceConfig(_ipAddress);
            if (deviceConfigTab.WaitForAvailable(WindowTimeout))
            {
                deviceConfigTab.DeviceConfiguraTabItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));

                if (deviceConfigTab.ListView141cb8bDataGrid.RowCount() > 0)
                {
                    var device = DeviceFactory.Create(deviceConfiguration.DeviceAddress, deviceConfiguration.DevicePassword);

                    deviceConfigTab.SetDeviceInfo(device.Address, device.GetDeviceInfo().ModelName);
                    deviceConfigTab.ContextMenuItem.SetTemplateName(deviceConfiguration.Template.Name);

                    deviceConfigTab.SelectedDeviceDataItem.PerformHumanAction(x => x.Select(ShortTimeout));
                    deviceConfigTab.SelectedDeviceDataItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Right, ShortTimeout));
                    deviceConfigTab.ContextMenuItem.ApplyTemplateMeMenuItem.PerformHumanAction(x => x.Expand(ShortTimeout));
                    if (deviceConfigTab.ContextMenuItem.TemplateMenuItem.IsVisible(ShortTimeout))
                    {
                        deviceConfigTab.ContextMenuItem.TemplateMenuItem.PerformHumanAction(x => x.Click(ShortTimeout));
                    }
                    else
                    {
                        throw new ArgumentException("Unable to find the specified template");
                    }

                    PauseForPleaseWait();
                    DSSConfig_DeviceConfig.TemplateSummary templateSummary = new DSSConfig_DeviceConfig.TemplateSummary(_ipAddress, deviceConfiguration.Template.Name);
                    if (templateSummary.TemplateApplyWindow.IsVisible(ShortTimeout))
                    {
                        templateSummary.OKButtonCA11C16Button.PerformHumanAction(x => x.Click(ShortTimeout));
                    }
                }
            }
        }

        /// <summary>
        /// Create the Template
        /// </summary>
        /// <param name="createTemplate"></param>
        [Description("Creates a new Template")]
        public void CreateTemplate(DssCreateTemplate createTemplate)
        {
            DSSConfig_DeviceConfig deviceConfigTab = new DSSConfig_DeviceConfig(_ipAddress);
            DSSConfig_DeviceConfig.CreateTemplate creatingTemplate = new DSSConfig_DeviceConfig.CreateTemplate(_ipAddress);
            ScanFilePrefix filePrefix = new ScanFilePrefix(_sessionId, _userName, ScanType);
            var device = DeviceFactory.Create(createTemplate.DeviceAddress, createTemplate.DevicePassword);
            string modelName = device.GetDeviceInfo().ModelName;
            deviceConfigTab.SetDeviceInfo(device.Address, modelName);

            if (!deviceConfigTab.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }

            if (deviceConfigTab.WaitForAvailable(WindowTimeout))
            {
                deviceConfigTab.DeviceConfiguraTabItem.PerformHumanAction(x => x.Select(ShortTimeout));
            }

            if (deviceConfigTab.ListView141cb8bDataGrid.RowCount() > 0)
            {
                if (deviceConfigTab.SelectedDeviceDataItem != null)
                {
                    deviceConfigTab.SelectedDeviceDataItem.PerformHumanAction(x => x.Select(ShortTimeout));
                    deviceConfigTab.CreateTemplateBButton.PerformHumanAction(x => x.Click(ShortTimeout));
                    creatingTemplate.SetDeviceInfo(device.Address, modelName);
                    creatingTemplate.TextBox02EC542DEdit.PerformHumanAction(x => x.EnterText(filePrefix.ToString()));
                    creatingTemplate.TextBox212A3C47Edit.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
                    creatingTemplate.OKButton98812F5Button.PerformHumanAction(x => x.Click(ShortTimeout));
                    PauseForPleaseWait();
                }
                else
                {
                    throw new DeviceInvalidOperationException("The device is not managed by this DSS server");
                }

            }
            else
            {
                throw new Exception("There is no device in this DSS server to create template");
            }

        }

        #endregion TemplateConfiguration

        #region FaxConfiguration

        /// <summary>
        /// Configure Lan Fax for the Specified Device
        /// </summary>
        /// <param name="lanFax"></param>
        [Description("Configures LAN Fax")]
        public void ConfigureLanFax(DssLanFax lanFax)
        {
            DSSConfig_FaxConfig faxConfig = new DSSConfig_FaxConfig(_ipAddress);
            DSSConfig_PleaseWait pleaseWait = new DSSConfig_PleaseWait(_ipAddress);
            // Launch Application if Not running Already
            if (!faxConfig.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }
            faxConfig.HPDigitalSendinWindow.WaitForAvailable(WindowTimeout);

            faxConfig.GlobalFaxTabItem.PerformHumanAction(x => x.Select(ShortTimeout));
            faxConfig.EnableFaxSendCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));

            TopCatUiHelper.ComboBoxSetValue(faxConfig.FaxServiceComboBox, FaxServiceType.LanFax.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(faxConfig.LanFaxProductComboBox, lanFax.LanFaxDevice.GetDescription(), ShortTimeout);

            // Enter Folder Setting Details
            faxConfig.FolderPathEditTextBox.PerformHumanAction(x => x.EnterText(lanFax.FolderSetting.FolderPath));
            faxConfig.UserNameEditTextBox.PerformHumanAction(x => x.EnterText(lanFax.FolderSetting.Credential.UserName));
            faxConfig.PasswordEditPasswordBox.PerformHumanAction(x => x.EnterText(lanFax.FolderSetting.Credential.Password));
            faxConfig.DomainEditTextBox.PerformHumanAction(x => x.EnterText(lanFax.FolderSetting.Credential.Domain));
            // Verify Folder Access
            if (faxConfig.VerifyFolderAccessButton.IsEnabled(ShortTimeout))
            {
                faxConfig.VerifyFolderAccessButton.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
                // Use the please wait prompt and then continue as it is, else if the please wait prompt
                // goes on for more than 10 seconds we will end up with failure
                PauseForPleaseWait();
            }
            // Configure Retry Settings
            if (faxConfig.TextBoxMaxRetryAttemptEdit.IsEnabled(ShortTimeout))
            {
                faxConfig.TextBoxMaxRetryAttemptEdit.PerformHumanAction(x => x.EnterText(lanFax.LanFaxDialingSettings.MaxRetryAttempts.ToString()));
            }
            if (faxConfig.RetryIntervalEditTextBox.IsEnabled(ShortTimeout))
            {
                faxConfig.RetryIntervalEditTextBox.PerformHumanAction(x => x.EnterText(lanFax.LanFaxDialingSettings.RetryInterval.ToString()));
            }

            // Fax Input Settings
            // Notification Enable/Disable
            if (faxConfig.NotificationComboBox.IsEnabled(ShortTimeout))
            {
                TopCatUiHelper.ComboBoxSetValue(faxConfig.NotificationComboBox, lanFax.LanFaxInputSettings.Notification ? SettingStatus.Enable.GetDescription() : SettingStatus.Disable.GetDescription(), ShortTimeout);
            }

            // Error Correction Enable/Disable
            if (faxConfig.ErrorCorrectionComboBox.IsEnabled(ShortTimeout))
            {
                TopCatUiHelper.ComboBoxSetValue(faxConfig.ErrorCorrectionComboBox, lanFax.LanFaxInputSettings.ErrorCorrection ? SettingStatus.Enable.GetDescription() : SettingStatus.Disable.GetDescription(), ShortTimeout);
            }

            // Apply Notification Timeout
            if (faxConfig.NotificationTimeoutEditTextBox.IsEnabled(ShortTimeout))
            {
                faxConfig.NotificationTimeoutEditTextBox.EnterText(lanFax.LanFaxInputSettings.NotificationTimeout.ToString());
            }
            // Fax Output Settings
            if (faxConfig.LanFaxSpeedComboBox.IsEnabled(ShortTimeout))
            {
                TopCatUiHelper.ComboBoxSetValue(faxConfig.LanFaxSpeedComboBox, lanFax.LanFaxOutputSettings.Speed.ToString(), ShortTimeout);
            }
            if (faxConfig.LanFaxCoverPageComboBox.IsEnabled(ShortTimeout))
            {
                TopCatUiHelper.ComboBoxSetValue(faxConfig.LanFaxCoverPageComboBox, lanFax.LanFaxOutputSettings.CoverPage ? SettingStatus.Enable.GetDescription() : SettingStatus.Disable.GetDescription(), ShortTimeout);
            }
            // Apply all Settings
            if (faxConfig.GlobalApplyButton.IsEnabled(ShortTimeout))
            {
                faxConfig.GlobalApplyButton.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
            }
            // when you apply, it takes time to apply the setting and the please wait comes up, catch this
            while (pleaseWait.HPDigitalPleaseWaitWindow.IsVisible(ShortTimeout))
            {
                // do nothing.until the Please Wait window is closed
            }
            faxConfig.DeviceConfiguraTabItem.WaitForAvailable(WindowTimeout);
            faxConfig.DeviceConfiguraTabItem.PerformHumanAction(x => x.Select(ShortTimeout));

            var device = DeviceFactory.Create(lanFax.DeviceAddress, lanFax.DevicePassword);
            string deviceModelName = device.GetDeviceInfo().ModelName;

            LaunchConfigureDeviceWindow(device.Address, deviceModelName);

            DSS_LanFaxForDevice configLanFax = new DSS_LanFaxForDevice(_ipAddress);
            configLanFax.SetDeviceInfo(device.Address, deviceModelName);

            //Fixing CR-2610
            DSS_FaxForDevice faxDevice = new DSS_FaxForDevice(_ipAddress);
            faxDevice.SetDeviceInfo(device.Address, deviceModelName);

            configLanFax.FaxforDeviceTabItem.WaitForAvailable(WindowTimeout);
            configLanFax.FaxforDeviceTabItem.PerformHumanAction(x => x.Select(ShortTimeout));
            configLanFax.MaximizeMaximizButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            configLanFax.EnableFaxSendCheckBox.WaitForAvailable(WindowTimeout); // Getting Device Information takes Longer time

            if (!configLanFax.EnableFaxSendCheckBox.IsChecked(ShortTimeout))
            {
                configLanFax.EnableFaxSendCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
            }

            try
            {
                //Adding the Country,DefaultCompanyName,DefaultFaxNumber which are present in Analog Fax Tab and are mandate fields for LanFax
                TopCatUiHelper.ComboBoxSetValue(faxDevice.FaxServiceTypeComboBox, FaxServiceType.AnalogFax.GetDescription(), ShortTimeout);

                if (faxDevice.CountryComboBox.IsEnabled(ShortTimeout))
                {
                    TopCatUiHelper.ComboBoxSetValue(faxDevice.CountryComboBox, Country.UnitedStates.GetDescription(), ShortTimeout);
                }

                faxDevice.CompanyNameEditTextBox.PerformHumanAction(x => x.EnterText(DefaultCompanyName));
                faxDevice.PhoneNumberEditTextBox.PerformHumanAction(x => x.EnterText(DefaultFaxNumber));
            }
            catch
            {
                throw new ArgumentException("Exception being thrown when setting the value for either Country, DefaultCompanyName or DefaultFaxNumber fields");
            }

            configLanFax.LanfaxServiceTypeComboBox.WaitForAvailable(ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(configLanFax.LanfaxServiceTypeComboBox, FaxServiceType.ViaDss.GetDescription(), ShortTimeout);

            if (configLanFax.NotificationComboBox.IsEnabled(ShortTimeout))
            {
                TopCatUiHelper.ComboBoxSetValue(configLanFax.NotificationComboBox, NotifyCondition.DoNotNotify.GetDescription(), ShortTimeout);
            }

            configLanFax.DeviceConfigurationOKButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
        }

        /// <summary>
        /// Configure Analog Fax for the Specified Device
        /// </summary>
        /// <param name="analogFax"></param>
        [Description("Configures Analog Fax")]
        public void ConfigureAnalogFax(DssAnalogFax analogFax)
        {
            var device = DeviceFactory.Create(analogFax.DeviceAddress, analogFax.DevicePassword);
            string modelName = device.GetDeviceInfo().ModelName;

            LaunchConfigureDeviceWindow(device.Address, modelName);

            DSS_FaxForDevice analogFaxConfig = new DSS_FaxForDevice(_ipAddress);
            analogFaxConfig.SetDeviceInfo(device.Address, modelName);

            analogFaxConfig.ConfigureDeviceWindow.WaitForAvailable(WindowTimeout);
            analogFaxConfig.ConfigureDeviceWindow.BringToFront(ShortTimeout);

            analogFaxConfig.FaxforDeviceTabItem.PerformHumanAction(x => x.ClickWithMouse(WindowTimeout)); //CR:2520 is fixed by increasing WindowTimeout time
            analogFaxConfig.MaximizeMaximizButton.PerformHumanAction(x => x.Click(WindowTimeout));

            analogFaxConfig.EnableFaxSendCheckBox.WaitForAvailable(WindowTimeout);
            if (analogFaxConfig.EnableFaxSendCheckBox.IsEnabled(ShortTimeout))
            {
                analogFaxConfig.EnableFaxSendCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
            }
            try
            {
                TopCatUiHelper.ComboBoxSetValue(analogFaxConfig.FaxServiceTypeComboBox, FaxServiceType.AnalogFax.GetDescription(), ShortTimeout);
            }
            catch (Exception)
            {
                throw new DeviceCommunicationException("Check if the Internal Modem is Configured on the device for Analog Fax");
            }
            if (analogFaxConfig.CountryComboBox.IsEnabled(ShortTimeout))
            {
                TopCatUiHelper.ComboBoxSetValue(analogFaxConfig.CountryComboBox, analogFax.FaxModemSetting.ModemCountry.GetDescription(), WindowTimeout);
            }

            analogFaxConfig.CompanyNameEditTextBox.PerformHumanAction(x => x.EnterText(analogFax.FaxModemSetting.CompanyName));
            analogFaxConfig.PhoneNumberEditTextBox.PerformHumanAction(x => x.EnterText(analogFax.FaxModemSetting.PhoneNumber));

            TopCatUiHelper.ComboBoxSetValue(analogFaxConfig.NotifyConditionComboBox, analogFax.FaxSendNotificationSettings.NotificationCondition.GetDescription(), ShortTimeout);

            if (analogFaxConfig.NotifyMethodComboBox.IsEnabled(ShortTimeout))
            {
                TopCatUiHelper.ComboBoxSetValue(analogFaxConfig.NotifyMethodComboBox, analogFax.FaxSendNotificationSettings.NotificationDeliveryMethod.GetDescription(), ShortTimeout);
            }

            if (analogFaxConfig.NotificationEmailEditTextBox.IsEnabled())
            {
                analogFaxConfig.NotificationEmailEditTextBox.PerformHumanAction(x => x.EnterText(analogFax.FaxSendNotificationSettings.NotificationEmail));
                //Fixing CR-2609
                analogFaxConfig.NotificationEmaDup0Text.ClickWithMouse(ShortTimeout);  //  CR 2520 : to enable the ok button
            }

            if (analogFaxConfig.ConfigureDeviceOKButton.IsEnabled(ShortTimeout))
            {
                analogFaxConfig.ConfigureDeviceOKButton.ClickWithMouse(WindowTimeout);
                //CR:2520 When the Continue Prompt appears while configuring for the first time
                if (analogFaxConfig.ContinuePromptWindow.IsAvailable(WindowTimeout))
                {
                    analogFaxConfig.ContinueWindowYesButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
                }
            }
            else
            {
                throw new DeviceCommunicationException("OK button is not enabled");
            }
        }

        /// <summary>
        /// Configure Internet Fax for the Specified Device
        /// </summary>
        /// <param name="internetFax"></param>
        [Description("Configures Internet Fax")]
        public void ConfigureInternetFax(DssInternetFax internetFax)
        {
            DSSConfig_FaxConfig faxConfig = new DSSConfig_FaxConfig(_ipAddress);
            if (!faxConfig.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }
            faxConfig.HPDigitalSendinWindow.WaitForAvailable(WindowTimeout);
            faxConfig.GlobalFaxTabItem.PerformHumanAction(x => x.Select(ShortTimeout));
            faxConfig.EnableFaxSendCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
            // Select Internet Fax Service Type
            TopCatUiHelper.ComboBoxSetValue(faxConfig.FaxServiceComboBox, FaxServiceType.InternetFax.GetDescription(), ShortTimeout);

            DSS_InternetFaxSetUp iFax = new DSS_InternetFaxSetUp(_ipAddress);
            iFax.AddSMTPServerButton.PerformHumanAction(x => x.Click(ShortTimeout));
            iFax.MaximizeMaximizButton.PerformHumanAction(x => x.Click(ShortTimeout));
            iFax.ServerAddressEditTextBox.PerformHumanAction(x => x.EnterText(internetFax.EmailServer.ServerAddress));
            iFax.PortEditTextBox.PerformHumanAction(x => x.EnterText(internetFax.EmailServer.Port.ToString()));

            if (internetFax.EmailServer.UseSsl)
            {
                iFax.EnableSMTPSSLCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
            }
            else
            {
                iFax.EnableSMTPSSLCheckBox.PerformHumanAction(x => x.Uncheck(ShortTimeout));
            }
            if (internetFax.EmailServer.RequireAuthentication)
            {
                iFax.ServerRequiresAuthenticationCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
                iFax.SMTPUserNameEditTextBox.PerformHumanAction(x => x.EnterText(internetFax.EmailServer.Credential.UserName));
                iFax.SMTPPasswordEditBox.PerformHumanAction(x => x.EnterText(internetFax.EmailServer.Credential.Password));
            }
            else
            {
                iFax.ServerRequiresAuthenticationCheckBox.PerformHumanAction(x => x.Uncheck(ShortTimeout));
            }
            iFax.SplitSizeEditTextBox.PerformHumanAction(x => x.EnterText(internetFax.EmailServer.SplitSize.ToString(CultureInfo.InvariantCulture)));
            iFax.SMTPPromptOKButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));

            DSSConfig_Prompt prompt = new DSSConfig_Prompt(_ipAddress);
            if (prompt.DSSPromptOKButton.IsAvailable(PromptTimeout))
            {
                prompt.DSSPromptOKButton.PerformHumanAction(x => x.Click(ShortTimeout));
                iFax.PromptCancelButton.WaitForAvailable(ShortTimeout);
                iFax.PromptCancelButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            }

            iFax.FaxProviderEditTextBox.PerformHumanAction(x => x.EnterText(internetFax.FaxProviderDomain));
            iFax.FaxAccountEmailEditTextBox.PerformHumanAction(x => x.EnterText(internetFax.FaxAccountEmailAddress));
            iFax.T37PrefixEditTextBox.PerformHumanAction(x => x.EnterText(internetFax.T37Prefix));

            if (internetFax.UseEmailAddress)
            {
                iFax.IfavailableuseEmailCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
            }
            else
            {
                iFax.IfavailableuseEmailCheckBox.PerformHumanAction(x => x.Uncheck(ShortTimeout));
            }

            if (internetFax.AutoComplete)
            {
                iFax.AutocompleteCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
            }
            else
            {
                iFax.AutocompleteCheckBox.PerformHumanAction(x => x.Uncheck(ShortTimeout));
            }

            if (iFax.GlobalApplyButton.IsEnabled(ShortTimeout))
            {
                iFax.GlobalApplyButton.PerformHumanAction(x => x.Click(ShortTimeout));
            }

            var device = DeviceFactory.Create(internetFax.DeviceAddress, internetFax.DevicePassword);
            string modelName = device.GetDeviceInfo().ModelName;
            LaunchConfigureDeviceWindow(device.Address, modelName);

            DSS_FaxForDevice faxDevice = new DSS_FaxForDevice(_ipAddress);
            faxDevice.SetDeviceInfo(device.Address, modelName);

            faxDevice.FaxforDeviceTabItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowTimeout)); //CR:2527 control is not getting enabled in the given time
            if (faxDevice.EnableFaxSendCheckBox.IsAvailable(WindowTimeout))
            {
                faxDevice.EnableFaxSendCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
            }
            faxDevice.MaximizeMaximizButton.PerformHumanAction(x => x.Click(ShortTimeout));

            try
            {
                TopCatUiHelper.ComboBoxSetValue(faxDevice.FaxServiceTypeComboBox, FaxServiceType.AnalogFax.GetDescription(), ShortTimeout);
                if (faxDevice.CountryComboBox.IsEnabled(ShortTimeout))
                {
                    TopCatUiHelper.ComboBoxSetValue(faxDevice.CountryComboBox, Country.UnitedStates.GetDescription(), ShortTimeout);
                }

                faxDevice.CompanyNameEditTextBox.PerformHumanAction(x => x.EnterText(DefaultCompanyName));
                faxDevice.PhoneNumberEditTextBox.PerformHumanAction(x => x.EnterText(DefaultFaxNumber));
            }
            catch
            {
                // this block is executed only when the device has the Fax Cable attached and the Analog Fax is Enabled
                // ignore exception
            }

            faxDevice.FaxServiceTypeComboBox.WaitForAvailable(ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(faxDevice.FaxServiceTypeComboBox, FaxServiceType.ViaDss.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(faxDevice.NotifyConditionComboBox, NotifyCondition.DoNotNotify.GetDescription(), ShortTimeout);
            faxDevice.ConfigureDeviceOKButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
        }

        #endregion FaxConfiguration

        #region SendtoWorkflows

        /// <summary>
        /// Configuring Scan to Worflow with Folder Settings
        /// </summary>
        /// <param name="folderWorkflow"></param>
        [Description("Creates a workflow for Folder")]
        public void CreateFolderWorkflow(DssFolderWorkflowForm folderWorkflow)
        {
            DSSConfig_WorkflowMenu dssFolderWorkflow = new DSSConfig_WorkflowMenu(_ipAddress);
            if (!dssFolderWorkflow.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }

            dssFolderWorkflow.SetGroupMenuName(folderWorkflow.GroupName, folderWorkflow.MenuName, folderWorkflow.WorkflowForm.FormName);
            if (dssFolderWorkflow.WaitForAvailable(WindowTimeout))
            {
                AddWorkflowMenu(dssFolderWorkflow);
                if (dssFolderWorkflow.WorkflowFormWindow.IsVisible(ShortTimeout))
                {
                    dssFolderWorkflow.FormNameEditTextBox.PerformHumanAction(x => x.EnterText(folderWorkflow.WorkflowForm.FormName, ShortTimeout));
                    TopCatUiHelper.ComboBoxSetValue(dssFolderWorkflow.DestinationTypeComboBox, WorkflowDestinationType.Folder.GetDescription(), ShortTimeout);
                    dssFolderWorkflow.FolderPathEditTextBox.PerformHumanAction(x => x.EnterText(folderWorkflow.FolderPath, ShortTimeout));
                    // Authentication Settings
                    ConfigureAuthenticationSettings(folderWorkflow.WorkflowForm.AuthenticationSetting, dssFolderWorkflow);
                    // Scan settings
                    ConfigureScanSettings(folderWorkflow.WorkflowForm.ImagePreset, folderWorkflow.WorkflowForm.ScanSettings, dssFolderWorkflow);
                    // Enhanced workflow
                    ConfigureEnhanceWorkflow(folderWorkflow.WorkflowForm.EnhancedWorkflow, dssFolderWorkflow);
                    // file settings
                    ConfigureFileSetting(folderWorkflow.WorkflowForm.FileSettings, folderWorkflow.WorkflowForm.MetadataFileFormat, dssFolderWorkflow);
                    //Adding OCR Prompts for None MetadataFileFormat
                    if (folderWorkflow.WorkflowForm.MetadataFileFormat == MetaDataFileFormat.None)
                    {
                        AddOcrPrompts(dssFolderWorkflow, folderWorkflow.WorkflowForm.Prompts);
                    }
                    if (dssFolderWorkflow.WorkflowFormOkButton.IsEnabled(ShortTimeout))
                    {
                        dssFolderWorkflow.WorkflowFormOkButton.PerformHumanAction(x => x.Click(ShortTimeout));
                    }
                    else
                    {
                        throw new DeviceCommunicationException("Please Check WorkFlow Form Configurations. OK button is not enabled due to Some error in Configuration");
                    }

                    dssFolderWorkflow.ApplyButtonButton.PerformHumanAction(x => x.Click(ShortTimeout));
                }
            }
        }

        /// <summary>
        /// Configuring Scan to Workflow with Sharepoint settings
        /// </summary>
        /// <param name="sharepointWorkflow"></param>
        [Description("Creates a workflow for SharePoint")]
        public void CreateSharePointWorkflow(DssSharePointWorkflowForm sharepointWorkflow)
        {
            // Apply the Configurations to Device
            DSSConfig_WorkflowMenu dssSharePointWorkflow = new DSSConfig_WorkflowMenu(_ipAddress);
            if (!dssSharePointWorkflow.WaitForAvailable(WindowTimeout))  // CR:2521 is fixed launch added
            {
                LaunchApplication();
            }
            dssSharePointWorkflow.SetGroupMenuName(sharepointWorkflow.GroupName, sharepointWorkflow.MenuName, sharepointWorkflow.WorkflowForm.FormName);
            if (dssSharePointWorkflow.WaitForAvailable(WindowTimeout))
            {
                AddWorkflowMenu(dssSharePointWorkflow);
                if (dssSharePointWorkflow.WorkflowFormWindow.IsVisible(ShortTimeout))
                {
                    dssSharePointWorkflow.FormNameEditTextBox.PerformHumanAction(x => x.EnterText(sharepointWorkflow.WorkflowForm.FormName, ShortTimeout));
                    TopCatUiHelper.ComboBoxSetValue(dssSharePointWorkflow.DestinationTypeComboBox, WorkflowDestinationType.SharePoint.GetDescription(), ShortTimeout);
                    dssSharePointWorkflow.SharePointEditTextBox.PerformHumanAction(x => x.EnterText(sharepointWorkflow.SharePointUrl.ToString(), ShortTimeout));

                    if (sharepointWorkflow.OverwriteExisting)
                    {
                        dssSharePointWorkflow.OverwriteexistiCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
                    }
                    else
                    {
                        dssSharePointWorkflow.OverwriteexistiCheckBox.PerformHumanAction(x => x.Uncheck(ShortTimeout));
                    }

                    // Authentication Settings
                    ConfigureAuthenticationSettings(sharepointWorkflow.WorkflowForm.AuthenticationSetting, dssSharePointWorkflow);
                    // Scan settings
                    ConfigureScanSettings(sharepointWorkflow.WorkflowForm.ImagePreset, sharepointWorkflow.WorkflowForm.ScanSettings, dssSharePointWorkflow);
                    // Enhanced workflow
                    ConfigureEnhanceWorkflow(sharepointWorkflow.WorkflowForm.EnhancedWorkflow, dssSharePointWorkflow);
                    // file settings
                    ConfigureFileSetting(sharepointWorkflow.WorkflowForm.FileSettings, sharepointWorkflow.WorkflowForm.MetadataFileFormat, dssSharePointWorkflow);
                    //Adding OCR Prompts for None MetadataFileFormat
                    if (sharepointWorkflow.WorkflowForm.MetadataFileFormat == MetaDataFileFormat.None)
                    {
                        AddOcrPrompts(dssSharePointWorkflow, sharepointWorkflow.WorkflowForm.Prompts);
                    }
                    if (dssSharePointWorkflow.WorkflowFormOkButton.IsEnabled(ShortTimeout))
                    {
                        dssSharePointWorkflow.WorkflowFormOkButton.PerformHumanAction(x => x.Click(ShortTimeout));
                    }
                    else
                    {
                        throw new ArgumentException("Please Check WorkFlow Form Configurations. OK button is not enabled due to Some error in Configuration");
                    }

                    dssSharePointWorkflow.ApplyButtonButton.PerformHumanAction(x => x.Click(ShortTimeout));
                }
            }
        }


        ///<summary>
        /// Configuring Scan to Worflow with FTP Settings
        ///</summary>
        /// <param name="ftpWorkflow"></param>
        [Description("Creates a workflow for FTP Server")]
        public void CreateFtpSiteWorkflow(DssFtpWorkflowForm ftpWorkflow)
        {
            DSSConfig_WorkflowMenu dssFtpSiteWorkflow = new DSSConfig_WorkflowMenu(_ipAddress);
            if (!dssFtpSiteWorkflow.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }

            dssFtpSiteWorkflow.SetGroupMenuName(ftpWorkflow.GroupName, ftpWorkflow.MenuName, ftpWorkflow.FormName);
            if (dssFtpSiteWorkflow.WaitForAvailable(WindowTimeout))
            {
                AddWorkflowMenu(dssFtpSiteWorkflow);
                if (dssFtpSiteWorkflow.WorkflowFormWindow.IsVisible(ShortTimeout))
                {
                    dssFtpSiteWorkflow.FormNameEditTextBox.PerformHumanAction(x => x.EnterText(ftpWorkflow.FormName, ShortTimeout));
                    TopCatUiHelper.ComboBoxSetValue(dssFtpSiteWorkflow.DestinationTypeComboBox, WorkflowDestinationType.Ftp.GetDescription(), ShortTimeout);
                    dssFtpSiteWorkflow.TextBox2d151aefEdit.PerformHumanAction(x => x.EnterText(ftpWorkflow.FtpServer, ShortTimeout));
                    dssFtpSiteWorkflow.TextBox6bb6284fEdit.PerformHumanAction(x => x.EnterText(ftpWorkflow.FtpPath, ShortTimeout));
                    dssFtpSiteWorkflow.CredentialUserNameEditTextBox.PerformHumanAction(x => x.EnterText(ftpWorkflow.UserName, ShortTimeout));
                    dssFtpSiteWorkflow.CredentialPasswordEditTextBox.PerformHumanAction(x => x.EnterText(ftpWorkflow.Password, ShortTimeout));
                    dssFtpSiteWorkflow.VerifyAccessButButton.PerformHumanAction(x => x.Click(ShortTimeout));
                    if (dssFtpSiteWorkflow.HPDigitalSendinDup1Window.IsAvailable(WindowTimeout))
                    {
                        if (dssFtpSiteWorkflow.TextBox85726F33Edit.IsVisible(ShortTimeout))
                        {
                            dssFtpSiteWorkflow.OKButtonAE4B0C6Button.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
                        }
                        else
                        {
                            throw new InvalidCredentialException("Invalid User Name, Password. Verify your Sign In information and retry.");
                        }
                    }

                    // Scan settings
                    ConfigureScanSettings(ftpWorkflow.ImagePreset, ftpWorkflow.ScanSettings, dssFtpSiteWorkflow);
                    // Enhanced workflow
                    ConfigureEnhanceWorkflow(ftpWorkflow.EnhancedWorkflow, dssFtpSiteWorkflow);
                    // File settings
                    ConfigureFileSetting(ftpWorkflow.FileSettings, ftpWorkflow.MetadataFileFormat, dssFtpSiteWorkflow);
                    //Adding OCR Prompts for None MetadataFileFormat
                    if (ftpWorkflow.MetadataFileFormat == MetaDataFileFormat.None)
                    {
                        AddOcrPrompts(dssFtpSiteWorkflow, ftpWorkflow.Prompts);
                    }
                    if (dssFtpSiteWorkflow.WorkflowFormOkButton.IsEnabled(ShortTimeout))
                    {
                        dssFtpSiteWorkflow.WorkflowFormOkButton.PerformHumanAction(x => x.Click(ShortTimeout));
                    }
                    else
                    {
                        throw new ArgumentException("Please Check WorkFlow Form Configurations. OK button is not enabled due to Some error in Configuration");
                    }

                    dssFtpSiteWorkflow.ApplyButtonButton.PerformHumanAction(x => x.Click(ShortTimeout));
                }
            }
        }

        /// <summary>
        /// Configuring Scan to Worflow with Printer Settings
        /// </summary>
        /// <param name="printerWorkflow"></param>
        [Description("Creates a workflow for printer")]
        public void CreatePrinterWorkflow(DssPrinterWorkflowForm printerWorkflow)
        {
            DSSConfig_WorkflowMenu dssPrinterWorkflow = new DSSConfig_WorkflowMenu(_ipAddress);
            if (!dssPrinterWorkflow.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }

            dssPrinterWorkflow.SetGroupMenuName(printerWorkflow.GroupName, printerWorkflow.MenuName, printerWorkflow.FormName);
            if (dssPrinterWorkflow.WaitForAvailable(WindowTimeout))
            {
                AddWorkflowMenu(dssPrinterWorkflow);
                if (dssPrinterWorkflow.WorkflowFormWindow.IsVisible(ShortTimeout))
                {
                    dssPrinterWorkflow.FormNameEditTextBox.PerformHumanAction(x => x.EnterText(printerWorkflow.FormName, ShortTimeout));
                    TopCatUiHelper.ComboBoxSetValue(dssPrinterWorkflow.DestinationTypeComboBox, WorkflowDestinationType.Printer.GetDescription(), ShortTimeout);
                    TopCatUiHelper.ComboBoxSetValue(dssPrinterWorkflow.ComboBox19b6064ComboBox, printerWorkflow.PrinterName, ShortTimeout);
                    // Scan settings
                    ConfigureScanSettings(printerWorkflow.ImagePreset, printerWorkflow.ScanSettings, dssPrinterWorkflow);
                    // Enhanced workflow
                    ConfigureEnhanceWorkflow(printerWorkflow.EnhancedWorkflow, dssPrinterWorkflow);
                    if (dssPrinterWorkflow.WorkflowFormOkButton.IsEnabled(ShortTimeout))
                    {
                        dssPrinterWorkflow.WorkflowFormOkButton.PerformHumanAction(x => x.Click(ShortTimeout));
                    }
                    else
                    {
                        throw new ArgumentException("Please Check WorkFlow Form Configurations. OK button is not enabled due to Some error in Configuration");
                    }

                    dssPrinterWorkflow.ApplyButtonButton.PerformHumanAction(x => x.Click(ShortTimeout));
                }
            }
        }


        [Description("Configures a workflow for the device")]
        public void ConfigureWorkflowForDevice(WorkFlowforDevice deviceworkflow)
        {
            // Handling the Device Configuration in Separate task for Scan to WorkFlow

            var device = DeviceFactory.Create(deviceworkflow.DeviceAddress, deviceworkflow.DevicePassword);
            string modelName = device.GetDeviceInfo().ModelName;
            LaunchConfigureDeviceWindow(device.Address, modelName);

            WorkFlowForDevice workFlowDevice = new WorkFlowForDevice(_ipAddress);
            workFlowDevice.SetDeviceInfo(device.Address, modelName);
            workFlowDevice.SetGroupName(deviceworkflow.GroupName);
            //Fixing CR-2613
            workFlowDevice.DeviceSendtoWorkflowTabItem.WaitForAvailable(WindowTimeout);
            workFlowDevice.DeviceSendtoWorkflowTabItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowTimeout));
            workFlowDevice.EnableSendtoWorCheckBox.PerformHumanAction(x => x.Check(WindowTimeout));
            if (workFlowDevice.UseEnhancedWorkRadioButton.IsAvailable(ShortTimeout))
            {
                workFlowDevice.UseEnhancedWorkRadioButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            }
            workFlowDevice.MaximizeButton.PerformHumanAction(x => x.ClickWithMouse(WindowTimeout));
            TopCatUiHelper.ComboBoxSetValue(workFlowDevice.GroupNameComboBox, deviceworkflow.GroupName, ShortTimeout);
            workFlowDevice.PositionTextBox.PerformHumanAction(x => x.EnterText(DefaultWorkFlowPosition));
            workFlowDevice.WorkFlowDescriptionTextBox.PerformHumanAction(x => x.EnterText(DefaultWorkFlowDescription));

            if (ResultCode.Passed == workFlowDevice.OKButton.WaitForEnabled(WindowTimeout))
            {
                workFlowDevice.OKButton.PerformHumanAction(x => x.ClickWithMouse(WindowTimeout));
                //Fixing CR-2613
                if (workFlowDevice.Window336E32508Window.IsAvailable(WindowTimeout))
                {
                    workFlowDevice.ContinueWindowYesButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
                }
                // Fixing CR:2521 When the Continue Prompt appears
                if (workFlowDevice.ContinuePromptWindow.IsAvailable(WindowTimeout))
                {
                    workFlowDevice.ContinueWindowYesButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
                }
            }
            else
            {
                throw new ArgumentException("Please Check WorkFlow Configurations. OK button is not enabled due to Some error in Configuration");
            }
        }

        private void AddOcrPrompts(DSSConfig_WorkflowMenu dssWorkflowMenu, string prompts)
        {
            DSS_OCRPrompt ocr = new DSS_OCRPrompt(_ipAddress);
            dssWorkflowMenu.AddButtond376aeButton.PerformHumanAction(x => x.Click(ShortTimeout));
            ocr.ListBox62d278ccList.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            ocr.OCRFILENAMEListListItem.PerformHumanAction(x => x.Select(ShortTimeout));
            if (ocr.EditButtonb4df6Button.IsEnabled(WindowTimeout))
            {
                ocr.EditButtonb4df6Button.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            }

            ocr.TextBoxc93b2243Edit.PerformHumanAction(x => x.EnterText(prompts));
            ocr.UserMustSupplyaCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
            ocr.OKButton249af4fButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            if (ocr.OKButton5fc8b2bButton.IsEnabled(WindowTimeout))
            {
                ocr.OKButton5fc8b2bButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            }
        }

        /// <summary>
        /// Adding workflow menu
        /// </summary>
        /// <param name="dssWorkflowMenu"></param>
        private void AddWorkflowMenu(DSSConfig_WorkflowMenu dssWorkflowMenu)
        {
            dssWorkflowMenu.WorkflowsTabItem.Select(ShortTimeout);

            if (!dssWorkflowMenu.GroupTreeItem.IsVisible(ShortTimeout))
            {
                dssWorkflowMenu.AddGroupButton.Click(ShortTimeout);
                DSSConfig_AddGroupPrompt groupPrompt = new DSSConfig_AddGroupPrompt(_ipAddress);
                groupPrompt.GroupNameEditTextBox.PerformHumanAction(x => x.EnterText(dssWorkflowMenu.GroupName, ShortTimeout));
                groupPrompt.GroupNameOKButton.PerformHumanAction(x => x.Click(ShortTimeout));
            }

            if (dssWorkflowMenu.GroupTreeItem.IsCollapsed(ShortTimeout))
            {
                dssWorkflowMenu.GroupTreeItem.Expand(ShortTimeout);
            }

            if (!dssWorkflowMenu.MenuTreeViewItem.IsVisible(ShortTimeout))
            {
                dssWorkflowMenu.AddMenuNameButton.PerformHumanAction(x => x.Click(ShortTimeout));
                DSSConfig_AddMenuPrompt menuPrompt = new DSSConfig_AddMenuPrompt(_ipAddress);
                menuPrompt.MenuNameEditTextBox.PerformHumanAction(x => x.EnterText(dssWorkflowMenu.MenuName, ShortTimeout));
                menuPrompt.MenuNameOKButton.PerformHumanAction(x => x.Click(ShortTimeout));
            }

            if (dssWorkflowMenu.MenuTreeViewItem.IsCollapsed(ShortTimeout))
            {
                dssWorkflowMenu.MenuTreeViewItem.PerformHumanAction(x => x.Expand(ShortTimeout));
            }
            if (!dssWorkflowMenu.AddFormButton.IsEnabled(ShortTimeout))
            {
                dssWorkflowMenu.MenuTreeViewItem.PerformHumanAction(x => x.Select(ShortTimeout));
            }

            if (!dssWorkflowMenu.FormNameTreeItem.IsVisible(ShortTimeout))
            {
                dssWorkflowMenu.AddFormButton.PerformHumanAction(x => x.Click(ShortTimeout));
            }
            else
            {
                dssWorkflowMenu.FormNameTreeItem.PerformHumanAction(x => x.Select(ShortTimeout));
                dssWorkflowMenu.EditButton252ecButton.PerformHumanAction(x => x.Click(ShortTimeout));
            }
        }

        /// <summary>
        /// Configuring Authentication Settings
        /// </summary>
        /// <param name="dssAuthenticationSetting"></param>
        /// <param name="dssWorkflow"></param>
        private static void ConfigureAuthenticationSettings(DssAuthenticationSetting dssAuthenticationSetting, DSSConfig_WorkflowMenu dssWorkflow)
        {
            if (dssAuthenticationSetting.AlwaysUseCredential)
            {
                dssWorkflow.AlwaysusethesecRadioButton.PerformHumanAction(x => x.Select(ShortTimeout));
                dssWorkflow.CredentialUserNameEditTextBox.PerformHumanAction(x => x.EnterText(dssAuthenticationSetting.Credential.UserName, ShortTimeout));
                dssWorkflow.CredentialPasswordEditTextBox.PerformHumanAction(x => x.EnterText(dssAuthenticationSetting.Credential.Password, ShortTimeout));
                dssWorkflow.CredentialDomainEditTextBox.PerformHumanAction(x => x.EnterText(dssAuthenticationSetting.Credential.Domain, ShortTimeout));
                //Fixing CR - 2625
                dssWorkflow.VerifyAccessButButton.PerformHumanAction(x => x.Click(ShortTimeout));
                if (dssWorkflow.HPDigitalSendinDup1Window.IsAvailable(WindowTimeout))
                {
                    if (dssWorkflow.TextBox85726F33Edit.IsVisible(ShortTimeout))
                    {
                        dssWorkflow.OKButtonAE4B0C6Button.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
                    }
                    else
                    {
                        throw new InvalidCredentialException("Invalid User Name, Password. Verify your Sign In information and retry.");
                    }
                }
            }
            else
            {
                dssWorkflow.UsecredentialsoRadioButton.PerformHumanAction(x => x.Select(ShortTimeout));
            }
        }

        /// <summary>
        /// Configuring Scan Settings
        /// </summary>
        /// <param name="imagePresets"></param>
        /// /// <param name="dssScanSetting"></param>
        /// <param name="dssWorkflow"></param>
        private static void ConfigureScanSettings(ImagePresets imagePresets, DssScanSetting dssScanSetting, DSSConfig_WorkflowMenu dssWorkflow)
        {
            TopCatUiHelper.ComboBoxSetValue(dssWorkflow.ImagePresetsComboBox, imagePresets.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(dssWorkflow.OriginalSizeComboBox, dssScanSetting.OriginalSize.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(dssWorkflow.OriginalSideComboBox, dssScanSetting.OriginalSides.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(dssWorkflow.OptimizeComboBox, dssScanSetting.Optimize.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(dssWorkflow.OrientationComboBox, dssScanSetting.Orientation.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(dssWorkflow.BackgroundCleanupComboBox, dssScanSetting.BackgroundCleanup.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(dssWorkflow.SharpnessComboBox, dssScanSetting.Sharpness.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(dssWorkflow.DarknessComboBox, dssScanSetting.Darkness.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(dssWorkflow.ContrastComboBox, dssScanSetting.Contrast.GetDescription(), ShortTimeout);
        }

        /// <summary>
        /// Configuring Enhanced WorkFlow
        /// </summary>
        /// <param name="enhancedWorkflowworkflow"></param>
        /// <param name="dssWorkflow"></param>
        private static void ConfigureEnhanceWorkflow(DssEnhancedWorkflow enhancedWorkflowworkflow, DSSConfig_WorkflowMenu dssWorkflow)
        {
            TopCatUiHelper.ComboBoxSetValue(dssWorkflow.ComboBox1B4345AComboBox, enhancedWorkflowworkflow.OriginalSize.GetDescription(), ShortTimeout);
            if (enhancedWorkflowworkflow.BlankPageSuppression)
            {
                dssWorkflow.BlankPageSuppreCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
            }
            else
            {
                dssWorkflow.BlankPageSuppreCheckBox.PerformHumanAction(x => x.Uncheck(ShortTimeout));
            }

            if (enhancedWorkflowworkflow.NotificationCondition != NotifyCondition.DoNotNotify)
            {
                TopCatUiHelper.ComboBoxSetValue(dssWorkflow.ComboBoxmEWNotiDup0ComboBox, enhancedWorkflowworkflow.NotificationCondition.GetDescription(), ShortTimeout);
                TopCatUiHelper.ComboBoxSetValue(dssWorkflow.ComboBoxmEWNotiDup1ComboBox, enhancedWorkflowworkflow.NotificationDeliveryMethod.GetDescription(), ShortTimeout);

                if (enhancedWorkflowworkflow.NotificationDeliveryMethod == NotifyMethod.Email)
                {
                    dssWorkflow.TextBoxmEWNotifEdit.PerformHumanAction(x => x.EnterText(enhancedWorkflowworkflow.NotificationEmailAddress, ShortTimeout));
                }
            }
            else
            {
                TopCatUiHelper.ComboBoxSetValue(dssWorkflow.ComboBoxmEWNotiDup0ComboBox, NotifyCondition.DoNotNotify.GetDescription(), ShortTimeout);
            }
        }

        /// <summary>
        /// Configuring File Settings
        /// </summary>
        /// <param name="fileSetting"></param>
        /// <param name="metaDataFileFormat"></param>
        /// <param name="dssWorkflow"></param>
        private static void ConfigureFileSetting(DssFileSetting fileSetting, MetaDataFileFormat metaDataFileFormat, DSSConfig_WorkflowMenu dssWorkflow)
        {
            TopCatUiHelper.ComboBoxSetValue(dssWorkflow.ColorPreferenceComboBox, fileSetting.ColorPreference.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(dssWorkflow.OutputQualityComboBox, fileSetting.OutputQuality.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(dssWorkflow.FileTypeComboBox, fileSetting.FileType.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(dssWorkflow.ResolutionComboBox, fileSetting.Resolution.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(dssWorkflow.CompressionCombobox, fileSetting.HighCompression ? "High" : "Normal", ShortTimeout);

            if (fileSetting.FileType == FileType.Pdf)
            {
                if (fileSetting.PdfEncryption)
                {
                    dssWorkflow.PDFEncryptionChCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
                }
                else
                {
                    dssWorkflow.PDFEncryptionChCheckBox.PerformHumanAction(x => x.Uncheck(ShortTimeout));
                }
            }

            // metadata settings
            TopCatUiHelper.ComboBoxSetValue(dssWorkflow.MetadataFileTypeComboBox, metaDataFileFormat.GetDescription(), ShortTimeout);
        }

        #endregion SendtoWorkflows

        #region SendToFolder

        /// <summary>
        /// Configure Send to Folder in both Global and Device Configuration tab
        /// </summary>
        /// <param name="folderserver"></param>
        [Description("Configures Send To Folder")]
        public void ConfigureSendToFolder(SendToFolderSetup folderserver)
        {
            SendToFolderGlobal configureSnfGlobal = new SendToFolderGlobal(_ipAddress);

            if (!configureSnfGlobal.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }

            configureSnfGlobal.GlobalSnfTabItem.PerformHumanAction(x => x.Select(ShortTimeout));
            configureSnfGlobal.SnfAddButton.WaitForAvailable(WindowTimeout);
            configureSnfGlobal.SnfAddButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));

            SendToFolderAdd addButton = new SendToFolderAdd(_ipAddress);
            addButton.FolderNameTextBox.PerformHumanAction(x => x.EnterText(folderserver.FolderName));
            addButton.FolderDescriptionTextBox.PerformHumanAction(x => x.EnterText(folderserver.FolderDescription));
            addButton.FolderAddButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));

            SendToFolderAddDialogue addDialogue = new SendToFolderAddDialogue(_ipAddress);
            addDialogue.FolderPathTextBox.WaitForAvailable(ShortTimeout);
            addDialogue.FolderPathTextBox.PerformHumanAction(x => x.EnterText(folderserver.FolderPath));

            if (folderserver.UseCommonCredentials)
                addDialogue.UseCommonCredenRadioButton.PerformHumanAction(x => x.Select(WindowTimeout));
            else
                addDialogue.UsePathSpecificRadioButton.PerformHumanAction(x => x.Select(WindowTimeout));

            addDialogue.UserNameTextBox.WaitForAvailable(WindowTimeout);
            if (addDialogue.UserNameTextBox.IsEnabled(WindowTimeout))
            {
                addDialogue.UserNameTextBox.PerformHumanAction(x => x.EnterText(folderserver.UserName));
                addDialogue.PasswordTextBox.PerformHumanAction(x => x.EnterText(folderserver.Password));
                addDialogue.DomainTextBox.PerformHumanAction(x => x.EnterText(folderserver.Domain));
            }
            if (addDialogue.VerifyAccessButButton.IsEnabled(ShortTimeout))
            {
                addDialogue.VerifyAccessButButton.PerformHumanAction(x => x.Click(ShortTimeout));
            }
            if (addDialogue.AddDialogueOKButton.IsEnabled(ShortTimeout))
            {
                addDialogue.AddDialogueOKButton.WaitForAvailable(WindowTimeout);
                addDialogue.AddDialogueOKButton.PerformHumanAction(x => x.Click(ShortTimeout));
            }
            else
            {
                throw new DirectoryNotFoundException($"Please Check Folder Configurations. Error: {addDialogue.A1FolderPathisrText.Name(ShortTimeout)}");
            }

            addButton.OKButton.WaitForAvailable(WindowTimeout);
            if (addButton.OKButton.IsEnabled(ShortTimeout))
                addButton.OKButton.PerformHumanAction(x => x.Click(ShortTimeout));
            else
            {
                throw new DirectoryNotFoundException($"Unable to add the folder. Error: {addButton.A1AFolderQuickSText.Name(ShortTimeout)}");
            }

            configureSnfGlobal.ApplyButton.WaitForAvailable(WindowTimeout);
            configureSnfGlobal.ApplyButton.PerformHumanAction(x => x.Click(ShortTimeout));

            var device = DeviceFactory.Create(folderserver.DeviceAddress, folderserver.DevicePassword);
            string modelName = device.GetDeviceInfo().ModelName;

            LaunchConfigureDeviceWindow(device.Address, modelName);

            SendToFolderDevice snfDeviceConfig = new SendToFolderDevice(_ipAddress);
            snfDeviceConfig.SetDeviceInfo(device.Address, modelName);
            snfDeviceConfig.SnfTabItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, WindowTimeout)); //CR:2522 is fixed by increasing WindowTimeout
            snfDeviceConfig.EnableSavetoNetCheckBox.PerformHumanAction(x => x.Check(WindowTimeout));
            snfDeviceConfig.MaximizeButton.PerformHumanAction(x => x.ClickWithMouse(WindowTimeout));
            TopCatUiHelper.ComboBoxSetValue(snfDeviceConfig.ServiceTypeComboBox, FaxServiceType.ViaDss.GetDescription(), ShortTimeout);

            //Adding Prefix and Suffix (CR- 2781)
            snfDeviceConfig.TextBox8a012dfaEdit.PerformHumanAction(x => x.EnterText(folderserver.Prefix));
            snfDeviceConfig.TextBox4429f27eEdit.PerformHumanAction(x => x.EnterText(folderserver.Suffix));

            snfDeviceConfig.OKButton.PerformHumanAction(x => x.Click(ShortTimeout));

            //Closing the application (CR- 2827)
            if (configureSnfGlobal.WaitForAvailable(WindowTimeout))
            {
                configureSnfGlobal.OKButtonCC2EF49Button.PerformHumanAction(x => x.Click(ShortTimeout));
            }
        }

        #endregion SendToFolder

        #region Import
        /// <summary>
        /// Importing DSS Configurations
        /// </summary>
        /// <param name="import"></param>
        [Description("Does the Importing of Devices")]
        public void ImportDss(DssImport import)
        {
            DSSConfig_DeviceConfig deviceConfigTab = new DSSConfig_DeviceConfig(_ipAddress);

            if (!deviceConfigTab.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }

            if (deviceConfigTab.WaitForAvailable(WindowTimeout))
            {
                deviceConfigTab.DeviceConfiguraTabItem.PerformHumanAction(x => x.Select(ShortTimeout));
            }

            deviceConfigTab.ImportButtona69Button.PerformHumanAction(x => x.Click(ShortTimeout));
            DSS_DeviceConfig_Import importDss = new DSS_DeviceConfig_Import(_ipAddress);
            importDss.FilenameEdit114Edit.PerformHumanAction(x => x.EnterText(import.FileName));
            importDss.OpenButton1Button.PerformHumanAction(x => x.Click(ShortTimeout));
            if (importDss.OKButton2Button.IsAvailable(PromptTimeout))
            {
                throw new FileLoadException("Invalid csv filename.");
            }

            // Retry untill the Import final screen is avaliable.
            Retry.UntilTrue(() => IsScreenAvaliable(importDss, import), 150, TimeSpan.FromSeconds(5));

            if (importDss.OKButtonEBFC423Button.IsEnabled(WindowTimeout))
            {
                importDss.OKButtonEBFC423Button.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            }
        }

        private bool IsScreenAvaliable(DSS_DeviceConfig_Import importDss, DssImport import)
        {
            if (importDss.AdministratorcrWindow.IsVisible(WindowTimeout))
            {
                importDss.TextBox67E46C7BEdit.PerformHumanAction(x => x.EnterText(import.UserName));
                importDss.PasswordBox1355Edit.PerformHumanAction(x => x.EnterText(import.Password));
                importDss.OKButton03CAA02Button.PerformHumanAction(
                    x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
                if (importDss.InvalidUserNameText.IsVisible(PromptTimeout))
                {
                    throw new InvalidCredentialException(
                        "Invalid User Name, Password, or Access Code. Verify your Sign In information and retry.");
                }
            }
            if (importDss.OKButtonEBFC423Button.IsEnabled(ShortTimeout))
            {
                return true;
            }
            return false;
        }

        #endregion Import

        #region Export
        /// <summary>
        /// Exporting DSS Configurations
        /// </summary>
        [Description("Does the Exporting")]
        public void ExportDss()
        {
            ScanFilePrefix filePrefix = new ScanFilePrefix(_sessionId, _userName, ScanType);
            DSSConfig_DeviceConfig deviceConfigTab = new DSSConfig_DeviceConfig(_ipAddress);
            DSS_DeviceConfig_Export exportDss = new DSS_DeviceConfig_Export(_ipAddress);
            DSSConfig_PleaseWait pleaseWaitWindow = new DSSConfig_PleaseWait(_ipAddress);

            if (!deviceConfigTab.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }

            if (deviceConfigTab.WaitForAvailable(WindowTimeout))
            {
                deviceConfigTab.DeviceConfiguraTabItem.PerformHumanAction(x => x.Select(ShortTimeout));
            }

            if (deviceConfigTab.ListView141cb8bDataGrid.RowCount() > 0)
            {
                deviceConfigTab.ExportButtonef5Button.PerformHumanAction(x => x.Click(ShortTimeout));
                exportDss.FilenameEdit100Edit.PerformHumanAction(x => x.EnterText(filePrefix.ToString()));
                exportDss.SaveButton1Button.PerformHumanAction(x => x.ClickWithMouse(WindowTimeout));

                // Do Nothing.Until Please wait Window is Closed
                Retry.WhileThrowing(() => pleaseWaitWindow.HPDigitalPleaseWaitWindow.WaitForInvisible(),
                    5,
                    TimeSpan.FromSeconds(5),
                    new List<Type>() { typeof(Exception) });
            }
            else
            {
                throw new Exception("There is no device in this DSS server to export");
            }
        }

        #endregion Export

        #region Addressing
        /// <summary>
        /// Configuring the Addressing
        /// </summary>
        /// <param name="addressing"></param>
        [Description("Configures Addressing")]
        public void ConfigureAddressing(DssAddressing addressing)
        {
            DSS_Address addressLdapReplication = new DSS_Address(_ipAddress);
            if (!addressLdapReplication.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }

            addressLdapReplication.SystemWindowsCoDup6TabItem.WaitForAvailable(WindowTimeout);
            addressLdapReplication.SystemWindowsCoDup6TabItem.PerformHumanAction(x => x.Select(ShortTimeout));
            addressLdapReplication.EnableNetworkCoCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
            if (!addressLdapReplication.TextBox858933a3Edit.IsVisible(ShortTimeout))
            {
                addressLdapReplication.ButtonHeaderSitDup0Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
            }

            addressLdapReplication.TextBox858933a3Edit.PerformHumanAction(x => x.EnterText(addressing.LdapServerAddress));
            if (addressing.UseSsl)
            {
                addressLdapReplication.UseasecureconneCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
            }
            else
            {
                addressLdapReplication.UseasecureconneCheckBox.PerformHumanAction(x => x.Uncheck(ShortTimeout));
            }

            if (!addressLdapReplication.ServerrequiresaRadioButton.IsVisible(ShortTimeout))
            {
                addressLdapReplication.ButtonHeaderSitDup1Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
            }

            addressLdapReplication.ServerrequiresaRadioButton.PerformHumanAction(x => x.Select(ShortTimeout));
            TopCatUiHelper.ComboBoxSetValue(addressLdapReplication.ComboBox5cd52d1ComboBox, "Simple", ShortTimeout);
            addressLdapReplication.TextBoxb54cac9eEdit.PerformHumanAction(x => x.EnterText(addressing.UserName));
            addressLdapReplication.PasswordBox8ed9Edit.PerformHumanAction(x => x.EnterText(addressing.Password));
            if (!addressLdapReplication.TextBox2e2cfbd5Edit.IsVisible(ShortTimeout))
            {
                addressLdapReplication.ButtonHeaderSitDup2Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
            }

            addressLdapReplication.TextBox2e2cfbd5Edit.PerformHumanAction(x => x.EnterText(addressing.BindandSearch));
            addressLdapReplication.UseCustomAttribRadioButton.PerformHumanAction(x => x.Select(ShortTimeout));
            addressLdapReplication.TextBox0fd71265Edit.PerformHumanAction(x => x.EnterText(addressing.MatchNameAttribute));
            addressLdapReplication.TextBoxc34b5023Edit.PerformHumanAction(x => x.EnterText(addressing.EmailAttribute));

            TopCatUiHelper.ComboBoxSetValue(addressLdapReplication.ComboBox2137ab3ComboBox, addressing.MaximunLdapAddress.GetDescription(), ShortTimeout);
            TopCatUiHelper.ComboBoxSetValue(addressLdapReplication.ComboBox290a3adComboBox, addressing.MaximunSearchTime.GetDescription(), ShortTimeout);

            //To make sure next button is visible
            addressLdapReplication.ButtonHeaderSitDup2Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
            if (!addressLdapReplication.TextBoxf206c2f6Edit.IsVisible(ShortTimeout))
            {
                addressLdapReplication.ButtonHeaderSitDup3Button.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
            }

            addressLdapReplication.TextBoxf206c2f6Edit.PerformHumanAction(x => x.EnterText(addressing.EnterCharatersToVerify));
            addressLdapReplication.TestButtone6d54Button.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));

            if (addressLdapReplication.OKButton4F49CDFButton.IsAvailable(WindowTimeout))
            {
                addressLdapReplication.OKButton4F49CDFButton.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
            }
            else
            {
                throw new ArgumentException("The operation failed. Please verify your configuration and try again.");
            }

            if (addressLdapReplication.ApplyButton61E2Button.IsEnabled(ShortTimeout))
            {
                addressLdapReplication.ApplyButton61E2Button.PerformHumanAction(x => x.Click(ShortTimeout));
            }
            else
            {
                throw new ArgumentException("Please Check the Configurations.Apply button is not enabled due to Some error in Configuration/Addressing is already configured");
            }
        }

        /// <summary>
        /// Configuring the Personal Contact Addressing
        /// </summary>
        /// <param name="addressing"></param>
        [Description("Configures Personal Contact")]
        public void ConfigurePersonalContactAddressing(DssPersonalContactAddressing addressing)
        {
            DSS_Address addressPersonalContact = new DSS_Address(_ipAddress);
            if (!addressPersonalContact.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }

            addressPersonalContact.SystemWindowsCoDup6TabItem.WaitForAvailable(WindowTimeout);
            addressPersonalContact.SystemWindowsCoDup6TabItem.PerformHumanAction(x => x.Select(ShortTimeout));
            addressPersonalContact.EnablePersonalCCheckBox.PerformHumanAction(x => x.Check(ShortTimeout));
            addressPersonalContact.TextBox0857B9D0Edit.PerformHumanAction(x => x.EnterText(addressing.Domain));
            addressPersonalContact.TextBox9B0DA4ABEdit.PerformHumanAction(x => x.EnterText(addressing.UserName));
            addressPersonalContact.PasswordBox6B3CEdit.PerformHumanAction(x => x.EnterText(addressing.Password));
            addressPersonalContact.TextBoxDE0C2350Edit.PerformHumanAction(x => x.EnterText(addressing.EnterCharatersToVerify));
            addressPersonalContact.TestButtonA90BEButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));

            if (addressPersonalContact.OkButtonPersonalContactButtonButton.IsAvailable(WindowTimeout))
            {
                addressPersonalContact.OkButtonPersonalContactButtonButton.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
            }
            else
            {
                throw new ArgumentException("The operation failed. Please verify your configuration and try again.");
            }

            if (addressPersonalContact.ApplyButton61E2Button.IsEnabled(ShortTimeout))
            {
                addressPersonalContact.ApplyButton61E2Button.PerformHumanAction(x => x.Click(ShortTimeout));
            }
            else
            {
                throw new ArgumentException("Please Check the Configurations.Apply button is not enabled due to Some error in Configuration/Addressing is already configured");
            }
        }

        /// <summary>
        /// Exporting DSS Address book
        /// </summary>
        [Description("Does the Exporting of DSS Address book")]
        public void ExportDssAddressBook()
        {
            ScanFilePrefix filePrefix = new ScanFilePrefix(_sessionId, _userName, ScanType);
            DSS_Address addressing = new DSS_Address(_ipAddress);
            DSS_AddressBookManager addressBookManager = new DSS_AddressBookManager(_ipAddress);
            DSS_AddressBookManager.DSS_AddressBookManagerExport addressBookManagerExport = new DSS_AddressBookManager.DSS_AddressBookManagerExport(_ipAddress);

            if (!addressing.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }

            addressing.SystemWindowsCoDup6TabItem.WaitForAvailable(WindowTimeout);
            addressing.SystemWindowsCoDup6TabItem.PerformHumanAction(x => x.ClickWithMouse(WindowTimeout));
            addressing.AddressBookManaButton.PerformHumanAction(x => x.Click(WindowTimeout));
            addressBookManager.PublicTextBlockText.ClickWithMouse(WindowTimeout);
            addressBookManager.ExportButtonA1CButton.PerformHumanAction(x => x.Click(ShortTimeout));
            addressBookManagerExport.SelectedButton6Button.PerformHumanAction(x => x.Click(ShortTimeout));
            //The enterd text in FilenameEdit100Edit is not getting reflected on clcikng save button when the name is passed through EnterText method.
            //Hence using the SendKeys method. 
            SendKeys.SendWait(filePrefix.ToString());
            addressBookManagerExport.SaveButton1Button.PerformHumanAction(x => x.ClickWithMouse(WindowTimeout));
            PauseForPleaseWait();
            addressBookManager.OKButtonFA1AC0AButton.PerformHumanAction(x => x.ClickWithMouse(WindowTimeout));
        }

        /// <summary>
        /// Importing DSS Address book
        /// </summary>
        /// <param name="import"></param>
        [Description("Does the importing of DSS Address book")]
        public void ImportDssAddressBook(DssAddressBookImport import)
        {
            DSS_Address addressing = new DSS_Address(_ipAddress);
            DSS_AddressBookManager addressBookManager = new DSS_AddressBookManager(_ipAddress);
            DSS_AddressBookManager.DSS_AddressBookManagerImport addressBookManagerImport = new DSS_AddressBookManager.DSS_AddressBookManagerImport(_ipAddress);

            if (!addressing.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }

            addressing.SystemWindowsCoDup6TabItem.WaitForAvailable(WindowTimeout);
            addressing.SystemWindowsCoDup6TabItem.PerformHumanAction(x => x.ClickWithMouse(WindowTimeout));
            addressing.AddressBookManaButton.PerformHumanAction(x => x.Click(WindowTimeout));
            addressBookManager.PublicTextBlockText.ClickWithMouse(WindowTimeout);
            addressBookManager.ImportAddressBoButton.PerformHumanAction(x => x.Click(ShortTimeout));
            addressBookManagerImport.FilenameEdit114Edit.PerformHumanAction(x => x.EnterText(import.FileName));
            addressBookManagerImport.OpenButton1Button.PerformHumanAction(x => x.Click(ShortTimeout));
            PauseForPleaseWait();
            addressBookManager.OKButtonFA1AC0AButton.PerformHumanAction(x => x.ClickWithMouse(WindowTimeout));
        }

        /// <summary>
        /// Creating Contacts in DSS Address book
        /// </summary>
        /// <param name="contacts"></param>
        [Description("Creating Contacts in DSS Address book")]
        public void CreatingContactsInDssAddressBook(DssAddressBookContacts contacts)
        {
            DSS_Address addressing = new DSS_Address(_ipAddress);
            DSS_AddressBookManager addressBookManager = new DSS_AddressBookManager(_ipAddress);
            DSS_AddressBookManager.Contacts addressBookManagerContacts = new DSS_AddressBookManager.Contacts(_ipAddress);
            if (!addressing.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }

            addressing.SystemWindowsCoDup6TabItem.WaitForAvailable(WindowTimeout);
            addressing.SystemWindowsCoDup6TabItem.PerformHumanAction(x => x.ClickWithMouse(WindowTimeout));
            addressing.AddressBookManaButton.PerformHumanAction(x => x.Click(WindowTimeout));
            addressBookManager.PublicTextBlockText.ClickWithMouse(WindowTimeout);
            addressBookManager.AddContactButtoButton.PerformHumanAction(x => x.ClickWithMouse(WindowTimeout));
            addressBookManagerContacts.TextBoxA2BAD0DFEdit.PerformHumanAction(x => x.EnterText(contacts.ContactName));
            addressBookManagerContacts.TextBoxAC47DC0BEdit.PerformHumanAction(x => x.EnterText(contacts.EmailAddress));
            addressBookManagerContacts.TextBox1C07E514Edit.PerformHumanAction(x => x.EnterText(contacts.FaxNumber));

            if (addressBookManagerContacts.SaveButtonC5786Button.IsEnabled(ShortTimeout))
            {
                addressBookManagerContacts.SaveButtonC5786Button.PerformHumanAction(x => x.ClickWithMouse(WindowTimeout));
            }
            else
            {
                throw new ArgumentException("Please check the Configurations.Save button is not enabled due to some error in the configuration");
            }

            if (addressBookManagerContacts.OKButton1Button.IsVisible(ShortTimeout))
            {
                addressBookManagerContacts.OKButton1Button.PerformHumanAction(x => x.Click(ShortTimeout));
            }

            addressBookManager.OKButtonFA1AC0AButton.PerformHumanAction(x => x.ClickWithMouse(WindowTimeout));
        }


        #endregion Addressing

        #region Remove Device
        /// <summary>
        /// Removing the Specific Device
        /// </summary>
        /// <param name="removeDevice"></param>
        [Description("Removing the Specific Device")]
        public void RemoveDeviceFromDss(RemoveDevice removeDevice)
        {
            var device = DeviceFactory.Create(removeDevice.DeviceAddress, removeDevice.DevicePassword);
            string modelName = device.GetDeviceInfo().ModelName;
            DSSConfig_DeviceConfig deviceConfigTab = new DSSConfig_DeviceConfig(_ipAddress);
            DSSConfig_PleaseWait pleaseWaitWindow = new DSSConfig_PleaseWait(_ipAddress);
            deviceConfigTab.SetDeviceInfo(device.Address, modelName);

            if (!deviceConfigTab.WaitForAvailable(WindowTimeout))
            {
                LaunchApplication();
            }

            // traverse to Device Configuration Tab
            if (deviceConfigTab.WaitForAvailable(WindowTimeout))
            {
                deviceConfigTab.DeviceConfiguraTabItem.PerformHumanAction(x => x.Select(ShortTimeout));
                // Get the Device List Item  Based on the Device IP address
                if (deviceConfigTab.ListView141cb8bDataGrid.RowCount() > 0)
                {
                    if (deviceConfigTab.SelectedDeviceDataItem != null)
                    {
                        deviceConfigTab.SelectedDeviceDataItem.PerformHumanAction(x => x.Select(ShortTimeout));
                        // Launch Device Configuration
                        deviceConfigTab.SelectedDeviceDataItem.PerformHumanAction(x => x.ClickWithMouse(MouseButton.Left, ShortTimeout));
                        if (deviceConfigTab.RemoveDeviceButButton.IsEnabled(WindowTimeout))
                        {
                            deviceConfigTab.RemoveDeviceButButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
                        }

                        if (deviceConfigTab.YesButtonAE4B0CButton.IsEnabled(WindowTimeout))
                        {
                            deviceConfigTab.YesButtonAE4B0CButton.PerformHumanAction(x => x.ClickWithMouse(ShortTimeout));
                        }

                        // Do Nothing.Until Please wait Window is Closed
                        Retry.WhileThrowing(() => pleaseWaitWindow.HPDigitalPleaseWaitWindow.WaitForInvisible(),
                            20,
                            TimeSpan.FromSeconds(5),
                            new List<Type>() { typeof(Exception) });
                    }
                    else
                    {
                        throw new DeviceInvalidOperationException("The device is not managed by this DSS server");
                    }
                }
                else
                {
                    throw new Exception("There is no device in this DSS server ");
                }
            }

        }

        #endregion Remove Device

        #region Utility

        /// <summary>
        /// Retriving the LocalHost IP Address
        /// </summary>
        /// <returns></returns>
        private static string GetLocalIpAddress()
        {
            // check to see if the address is stored in an environment variable (this will happen in vmware test cases)
            string serverIpAddress = Environment.GetEnvironmentVariable("DSS_SERVER_IP");
            if (string.IsNullOrEmpty(serverIpAddress))
            {
                // obtain server ipaddress from .net
                string hostName = Dns.GetHostName();
                serverIpAddress = GetIpAddressFromHostname(hostName);
            }
            return serverIpAddress;
        }

        private static string GetIpAddressFromHostname(string hostName)
        {
            //////////////////////////////////////////////////////////////////
            // ALGORITHM:
            // 1. Use first non-IPV6 address that is not a 192.168.x.x format
            // 2. Use first IPV6 address
            // 3. Use first 192.168.x.x format address
            //////////////////////////////////////////////////////////////////
            int retry = 1;
            string serverIpAddress = string.Empty;
            while (retry >= 0)
            {
                retry--;
                serverIpAddress = string.Empty;
                string firstIp192168 = string.Empty;
                string firstIpV6 = string.Empty;
                IPHostEntry ipHostEntry = Dns.GetHostEntry(hostName);
                foreach (IPAddress ipAddress in ipHostEntry.AddressList)
                {
                    // Save off the first IPV4 (non-IPV6) address that is not a 192.168.xxx.xxx
                    // since this is sent to Mfp's as the Dss server address.  Legacy Mfp's
                    // only support IPV4 format addresses.
                    if (ipAddress.AddressFamily != AddressFamily.InterNetworkV6)
                    {
                        if (ipAddress.ToString().StartsWith("192.168."))
                        {
                            if (string.IsNullOrEmpty(firstIp192168))
                            {
                                // Save off the first 192.168.xxx.xxx address we come across incase there is not a non-192.168 address on this pc to use.
                                firstIp192168 = ipAddress.ToString();
                            }
                        }
                        else if (string.IsNullOrEmpty(serverIpAddress))
                        {
                            serverIpAddress = ipAddress.ToString();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(firstIpV6))
                        {
                            // Save off the first IPV6 address we come across in case
                            // there is not a IPV4 or 192.168 address on this pc to use.
                            firstIpV6 = ipAddress.ToString();
                        }
                    }
                }

                // Check to see if there was no IPV4 address except for
                // 192.168.xxx.xxx format address or only an IPV6 address.
                if (string.IsNullOrEmpty(serverIpAddress))
                {
                    if (!string.IsNullOrEmpty(firstIp192168))
                    {
                        serverIpAddress = firstIp192168;
                    }
                    else if (!string.IsNullOrEmpty(firstIpV6))
                    {
                        serverIpAddress = firstIpV6;
                        if (serverIpAddress.Contains("%"))
                        {
                            serverIpAddress = "[" + serverIpAddress + "]";
                        }
                    }
                    else
                    {
                        // Saftey net to prevent empty or null address
                        serverIpAddress = "127.0.0.1";
                    }
                }

                if (serverIpAddress.Equals("127.0.0.1", StringComparison.CurrentCultureIgnoreCase) && retry >= 0)
                {
                    Thread.Sleep(5000); //if we can't figure the ip-address, we retry after 5seconds.
                }
                else
                    break;
            }

            return serverIpAddress;
        }

        private void PauseForPleaseWait()
        {
            DSSConfig_PleaseWait pleaseWaitPrompt = new DSSConfig_PleaseWait(_ipAddress);
            while (pleaseWaitPrompt.HPDigitalPleaseWaitWindow.IsVisible(ShortTimeout))
            {
                // Do Nothing.Until Please wait Window is Closed
            }

            DSSConfig_Prompt prompt = new DSSConfig_Prompt(_ipAddress);
            if (prompt.WaitForAvailable(WindowTimeout))
            {
                string alertMsg = prompt.PromptMessageEditTextBox.Text();

                prompt.DSSPromptOKButton.PerformHumanAction(x => x.Click(ShortTimeout));

                if (!string.IsNullOrEmpty(alertMsg) && alertMsg.Contains("failed"))
                {
                    throw new Exception($"Activity Failed with message: {alertMsg}");
                }
            }
        }

        #endregion Utility
    }
}