using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Plugin.DSSInstall.UIMaps;
using HP.ScalableTest.Plugin.DSSInstall.UIMaps.Uninstall;
using HP.ScalableTest.Utility;
using TopCat.TestApi.Enums;
using TopCat.TestApi.GUIAutomation;

namespace HP.ScalableTest.Plugin.DSSInstall
{
    /// <summary>
    /// Used to execute the activity of the DSSInstall plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class DSSInstallExecutionControl : UserControl, IPluginExecutionEngine
    {
        /// <summary>
        /// Initializes a new instance of the DSSInstallExecutionControl class.
        /// </summary>

        private DSSInstallActivityData _activityData;

        public DSSInstallExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Execute the task of the DSSInstall activity.
        /// </summary>
        /// <param name="executionData"></param>
        /// <returns></returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _activityData = executionData.GetMetadata<DSSInstallActivityData>();

            TopCatUIAutomation.Initialize();

            if (!File.Exists(_activityData.SetupFilePath))
            {
                return new PluginExecutionResult(PluginResult.Failed, "Could not find the setup file");
            }

            try
            {
                LocalLockToken dssLockToken = new LocalLockToken("DSS", new LockTimeoutData(TimeSpan.FromSeconds(60), _activityData.TransitionDelay == TimeSpan.FromSeconds(20) ? TimeSpan.FromMinutes(130) : TimeSpan.FromMinutes(10)));

                ExecutionServices.CriticalSection.Run(dssLockToken, StartSetup);
            }
            catch (Exception exception)
            {
                return new PluginExecutionResult(PluginResult.Failed, exception.Message);
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }

        private void StartSetup()
        {
            UpdateStatus("Copying setup file...");
            string localSetupFile = Path.Combine(Path.GetTempPath(), Path.GetFileName(_activityData.SetupFilePath));
            if (File.Exists(localSetupFile))
            {
                File.Delete(localSetupFile);
            }
            File.Copy(_activityData.SetupFilePath, localSetupFile);
            string version = FileVersionInfo.GetVersionInfo(localSetupFile).ProductVersion;
            version = version.Substring(0, version.LastIndexOf(".", StringComparison.OrdinalIgnoreCase));
            UpdateStatus("Starting Setup...");
            ProcessStartInfo pStartInfo = new ProcessStartInfo(localSetupFile)
            {
                UseShellExecute = true,
                Verb = "runas"
            };
            Process.Start(pStartInfo);

            if (_activityData.InstallOption == InstallOptions.Uninstall)
            {
                UpdateStatus($"Uninstalling DSS version {version}");
                Uninstall(version);
                return;
            }
            UpdateStatus($"Installing DSS version {version}");
            Install(version);
        }

        private void Uninstall(string version)
        {
            string installerScreen = "Welcome Screen";

            try
            {
                Uninstall_Welcome welcome = new Uninstall_Welcome(version);
                welcome.WaitForAvailable(60);

                if (welcome.NextButton6943Button.IsAvailable(5))
                {
                    if (welcome.NextButton6943Button.Click() != ResultCode.Passed)
                    {
                        throw new Exception("Failed to proceed beyond welcome screen");
                    }
                }
                Thread.Sleep(_activityData.TransitionDelay);
                Uninstall_Options options = new Uninstall_Options(version);
                installerScreen = "Uninstall Options Screen";
                options.WaitForAvailable(30);

                options.RemoveButton624RadioButton.Select(5);
                if (options.NextButton7196Button.IsAvailable(5))
                {
                    if (options.NextButton7196Button.Click(5) != ResultCode.Passed)
                    {
                        throw new Exception("Failed to Uninstall");
                    }
                }
                Thread.Sleep(_activityData.TransitionDelay);
                Uninstall_Remove remove = new Uninstall_Remove(version);
                installerScreen = "Uninstall Remove Screen";
                remove.WaitForAvailable(30);

                if (remove.RemoveButton733Button.Click() != ResultCode.Passed)
                {
                    throw new Exception("Failed to Uninstall");
                }
                Thread.Sleep(_activityData.TransitionDelay);
                if (remove.ErrorWindow.IsVisible(10))
                {
                    //we have an error, mostly the device is left in the configuration utility
                    remove.NoErrorButton.Click(5);
                }
                Thread.Sleep(_activityData.TransitionDelay);
                Uninstall_Finish finish = new Uninstall_Finish(version);
                installerScreen = "Uninstall Finish Screen";
                finish.WaitForAvailable(240);

                if (finish.FinishButton697Button.Click() != ResultCode.Passed)
                {
                    throw new Exception("Failed to Uninstall");
                }
            }
            catch (Exception)
            {
                throw new Exception($"Failed to proceed beyond: {installerScreen}");
            }

            if (_activityData.ValidateInstall)
            {
                UpdateStatus("Validating Uninstall...");
                ValidateUninstall();
            }
        }

        private void ValidateUninstall()
        {
            if (ServiceController.GetServices().Any(x => x.DisplayName == "HP Digital Sending Software" || x.DisplayName.StartsWith("SQL Server(HPDSS", StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Uninstall failed to remove HP DSS service");
            }

            UpdateStatus("Validated service removal, validating desktop shortcut...");

            string shortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "Configuration Utility.lnk");
            if (File.Exists(shortcut))
            {
                throw new Exception("Uninstall failed to remove desktop shortcut");
            }
            shortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory),
                "Configuration Utility.lnk");
            if (File.Exists(shortcut))
            {
                throw new Exception("Uninstall failed to remove desktop shortcut");
            }

            UpdateStatus("Desktop shortcut removed, validating start menu...");
            //check the start menu folder
            string startMenuFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs\\Hewlett-Packard\\HP Digital Sending Software");
            if (Directory.Exists(startMenuFolder))
            {
                throw new Exception("Uninstall failed to remove start menu folder");
            }

            startMenuFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu),
                "Programs\\Hewlett-Packard\\HP Digital Sending Software");
            if (Directory.Exists(startMenuFolder))
            {
                throw new Exception("Uninstall failed to remove start menu folder");
            }

            UpdateStatus("Validated start menu items, validating installation folder...");
            string installPath = string.IsNullOrEmpty(_activityData.InstallPath) ? (Environment.Is64BitOperatingSystem
                     ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                         "Hewlett-Packard\\HP Digital Sending Software")
                     : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                         "Hewlett-Packard\\HP Digital Sending Software")) : _activityData.InstallPath;

            if (Directory.Exists(installPath))
            {
                if (Directory.GetFiles(installPath).Any())
                {
                    throw new Exception("Uninstall failed to remove the installed files");
                }
            }

            UpdateStatus("Uninstall complete.");
        }

        private void Install(string version)
        {
            int cancelScreen = 0;
            bool upgrade = false;
            if (_activityData.CancelInstall)
            {
                Random randomCancellation = new Random();
                cancelScreen = randomCancellation.Next(1, 8);
            }

            List<UIMaps.UIMap> installScreenList = new List<UIMaps.UIMap>();

            installScreenList.Add(new DSS_Languages(version.Substring(0, version.LastIndexOf(".", StringComparison.OrdinalIgnoreCase))));
            installScreenList.Add(new DSS_Welcome(version));
            installScreenList.Add(new DSS_License(version));
            installScreenList.Add(new DSS_Upgrade(version, _activityData.SaveSettings));
            installScreenList.Add(new DSS_DataCollection(version));
            installScreenList.Add(new DSS_DestinationFolder(version, _activityData.InstallOption == InstallOptions.FullInstall, _activityData.InstallPath));
            installScreenList.Add(new DSS_WindowsFirewall(version));
            if (_activityData.InstallOption == InstallOptions.FullInstall)
            {
                installScreenList.Add(new DSS_ExternalDataBase(version));
            }
            installScreenList.Add(new DSS_ReadyInstall(version));
            installScreenList.Add(new DSS_FinishInstallation(version, _activityData.LaunchApplication, _activityData.ViewReadme));

            for (int i = 0; i < installScreenList.Count; i++)
            {
                try
                {
                    if (installScreenList.ElementAt(i) is DSS_ReadyInstall)
                    {
                        Thread.Sleep(_activityData.TransitionDelay == TimeSpan.FromSeconds(20) ? TimeSpan.FromHours(2) : _activityData.TransitionDelay);
                    }
                    if (installScreenList.ElementAt(i) is DSS_ExternalDataBase && upgrade)
                    {
                        continue;
                    }

                    var result = installScreenList.ElementAt(i).PerformAction(cancelScreen);
                    if (installScreenList.ElementAt(i) is DSS_Upgrade)
                    {
                        switch (result.Result)
                        {
                            case PluginResult.Passed:
                                i = 5;
                                upgrade = true;
                                break;

                            case PluginResult.Skipped:
                                result = new PluginExecutionResult(PluginResult.Passed);
                                upgrade = false;
                                break;
                        }
                    }
                    if (result.Result != PluginResult.Passed)
                    {
                        throw new Exception(result.Message);
                    }
                    Thread.Sleep(_activityData.TransitionDelay);
                }
                catch (Exception exception)
                {
                    throw new Exception($"Failed to proceed beyond {installScreenList.ElementAt(i).ScreenName}", exception.InnerException ?? exception);
                }
            }

            if (_activityData.ValidateInstall)
            {
                UpdateStatus("Validating install...");
                ValidateInstall();
            }
        }

        private void ValidateInstall()
        {
            if (_activityData.InstallOption == InstallOptions.FullInstall)
            {
                if (ServiceController.GetServices().Count(x => (x.DisplayName == "HP Digital Sending Software" || x.DisplayName.StartsWith("SQL Server (HPDSS", StringComparison.OrdinalIgnoreCase)) && x.Status == ServiceControllerStatus.Running) != 2)
                {
                    throw new Exception("HP DSS Service is not present nor running");
                }
                UpdateStatus("Verified service status, checking desktop shortcut...");
            }
            //check for shortcut on desktop
            string shortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Configuration Utility.lnk");
            if (File.Exists(shortcut))
            {
                UpdateStatus("Verified Desktop shortcut, checking start menu...");
                string startMenuFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs\\Hewlett-Packard\\HP Digital Sending Software");
                if (!Directory.Exists(startMenuFolder))
                {
                    throw new Exception("Unable to find DSS folder in Program Files Menu");
                }
            }
            //look in common desktop folder
            shortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory),
                "Configuration Utility.lnk");
            if (File.Exists(shortcut))
            {
                UpdateStatus("Verified Desktop shortcut, checking start menu...");
                string startMenuFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu),
                     "Programs\\Hewlett-Packard\\HP Digital Sending Software");
                if (!Directory.Exists(startMenuFolder))
                {
                    throw new Exception("Unable to find DSS folder in Program Files Menu");
                }
            }
            else
            {
                throw new Exception("Unable to find the shortcut on desktop");
            }

            string installPath = string.IsNullOrEmpty(_activityData.InstallPath) ? (Environment.Is64BitOperatingSystem
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    "Hewlett-Packard\\HP Digital Sending Software")
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "Hewlett-Packard\\HP Digital Sending Software")) : _activityData.InstallPath;
            UpdateStatus("Verified Start Menu, checking installation folder...");
            if (Directory.Exists(installPath))
            {
                if (!Directory.GetFiles(installPath).Any())
                {
                    throw new Exception("Unable to find the installed files in the path");
                }
            }
            else
            {
                throw new Exception("Unable to find the installed files in the path");
            }

            UpdateStatus("Installation complete.");
        }

        /// <summary>
        /// Updates the status text in the execution control display.
        /// </summary>
        /// <param name="statusMsg"></param>
        protected virtual void UpdateStatus(string statusMsg)
        {
            status_RichTextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status=" + statusMsg);
                c.AppendText($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")}  {statusMsg}\n");
                c.Select(c.Text.Length, 0);
                c.ScrollToCaret();
            });
        }
    }
}