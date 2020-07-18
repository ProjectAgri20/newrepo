using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Plugin.HpacClient.UIMaps;
using HP.ScalableTest.Print;
using HP.ScalableTest.Print.Drivers;
using HP.ScalableTest.Utility;
using TopCat.TestApi.GUIAutomation;

namespace HP.ScalableTest.Plugin.HpacClient
{
    /// <summary>
    ///  Execution Control Class for HPAC Client Plugin
    /// </summary>
    [ToolboxItem(false)]
    public partial class HpacClientExecutionControl : UserControl, IPluginExecutionEngine
    {
        private HpacClientActivityData _activityData;
        private SettingsDictionary _pluginSettings;
        private string _hpacServerIP;

        /// <summary>
        /// Constructor for HPAC Client Plugin Execution Control
        /// </summary>
        public HpacClientExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Entrypoint for the Plugin Task Execution for HPAC Client Plugin
        /// </summary>
        /// <param name="executionData"></param>
        /// <returns></returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _activityData = executionData.GetMetadata<HpacClientActivityData>();
            _pluginSettings = executionData.Environment.PluginSettings;
            _hpacServerIP = executionData.Servers.FirstOrDefault().Address;

            try
            {
                LocalLockToken hpacClientLockToken = new LocalLockToken("HPACClient",
                    new LockTimeoutData(TimeSpan.FromSeconds(60), TimeSpan.FromMinutes(10)));

                ExecutionServices.CriticalSection.Run(hpacClientLockToken, PluginTaskExecution);
            }
            catch (Exception exception)
            {
                return new PluginExecutionResult(PluginResult.Failed, exception.Message);
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }

        /// <summary>
        /// Triggers all Plugin Activities
        /// </summary>
        private void PluginTaskExecution()
        {
            InstallPrintDriverWithLPRQueue();
            if (_activityData.InstallHpacClient)
            {
                UpdateStatus(string.Format("Trigerring the HPAC Client Installation from {0}", _activityData.HpacClientInstallerPath));
                TopCatUIAutomation.Initialize();
                if (!File.Exists(_activityData.HpacClientInstallerPath))
                {
                    throw new Exception("Could not find the HPAC Client Setup file Specified");
                }
                HpacClientInstallationSetup();
                HpacClientInstall();
            }
        }

        /// <summary>
        /// Installs the PrintDriver With LPR Queue Details as in activity Data
        /// </summary>
        private void InstallPrintDriverWithLPRQueue()
        {
            DriverDetails driver = CreateDriver(_activityData.PrintDriver, _pluginSettings["PrintDriverServer"]);
            UpdateStatus($"Installing driver from {driver.InfPath}");
            ExecutionServices.SystemTrace.LogDebug($"Installing driver from {driver.InfPath}");
            DriverInstaller.Install(driver);
            UpdateStatus("Driver Installation Completed");

            UpdateStatus(string.Format("Creating LPR Port connecting to HPAC Server :{0}, QueueName : {1}", _hpacServerIP, _activityData.LprQueueName));
            ExecutionServices.SystemTrace.LogDebug($"Creating LPR Port connecting to HPAC Server :{_hpacServerIP}, QueueName : {_activityData.LprQueueName}");
            string portName = string.Format("_IP {0}_{1}", _hpacServerIP, _activityData.LprQueueName);
            PrintPortManager.AddLprPort(portName, LprPrinterPortInfo.DefaultPortNumber, _hpacServerIP, _activityData.LprQueueName);
            UpdateStatus("Port Creation Completed");

            UpdateStatus(string.Format("Creating LocalPrintDevice with Driver :{0} and port : {1}", driver.Name, portName));
            ExecutionServices.SystemTrace.LogDebug(string.Format("Creating LocalPrintDevice with Driver :{0} and port : {1}", driver.Name, portName));

            string queueName = string.Format("{0} ({1})", driver.Name, portName);
            if (!PrintQueueInstaller.IsInstalled(queueName))
            {
                PrintQueueInstaller.CreatePrintQueue(queueName, driver.Name, portName, driver.PrintProcessor);
                PrintQueueInstaller.WaitForInstallationComplete(queueName, driver.Name);
                UpdateStatus("Print Device Installation Completed");
            }

            PrintQueue queue = PrintQueueController.GetPrintQueue(queueName);
            if (_activityData.IsDefaultPrinter)
            {
                PrintQueueController.SetDefaultQueue(queue);
                UpdateStatus("Setting the Installed Print Device as a default Print Device");
            }

            ConfigurePrinterAttributes(queue);
            UpdateStatus("Printer Attributes Configuration Completed");
        }

        private static DriverDetails CreateDriver(PrintDriverInfo printDriverInfo, string printDriverServer)
        {
            var driver = new DriverDetails();
            driver.Name = printDriverInfo.DriverName;
            driver.PrintProcessor = printDriverInfo.PrintProcessor;

            if (!Environment.Is64BitOperatingSystem)
            {
                driver.Architecture = DriverArchitecture.NTx86;
                driver.InfPath = Path.Combine(printDriverServer, printDriverInfo.InfX86);
            }
            else
            {
                driver.Architecture = DriverArchitecture.NTAMD64;
                driver.InfPath = Path.Combine(printDriverServer, printDriverInfo.InfX64);
            }

            return driver;
        }

        /// <summary>
        /// Configures Printer Attributes
        /// </summary>
        /// <param name="printerName"></param>
        private void ConfigurePrinterAttributes(PrintQueue printQueue)
        {
            UpdateStatus(string.Format("Configuring Bidirectional Support for Printer {0}", printQueue.Name));
            PrintQueueController.ChangeAttributes(printQueue, PrintQueueAttributes.EnableBidi, _activityData.EnableBidi);

            UpdateStatus(string.Format("Configuring Print after Spooling for Printer {0}", printQueue.Name));
            PrintQueueController.ChangeAttributes(printQueue, PrintQueueAttributes.Queued, _activityData.PrintAfterSpooling);
        }

        /// <summary>
        /// Setting up for HPAC Client Installation
        /// </summary>
        private void HpacClientInstallationSetup()
        {
            UpdateStatus("Copying setup file to local Temp Location...");
            string localSetupFile = Path.Combine(Path.GetTempPath(), Path.GetFileName(_activityData.HpacClientInstallerPath));
            if (File.Exists(localSetupFile))
            {
                File.Delete(localSetupFile);
            }
            File.Copy(_activityData.HpacClientInstallerPath, localSetupFile);

            UpdateStatus("Copy Completed.Launching Setup...");

            Process process = new Process();
            process.StartInfo.FileName = "msiexec.exe";
            process.StartInfo.WorkingDirectory = Path.GetTempPath();
            process.StartInfo.Arguments = "/i \"" + localSetupFile + "\"";
            process.StartInfo.Verb = "runas";
            process.Start();
        }


        /// <summary>
        /// Triggers the UI based Automation for HPAC Client Installation
        /// </summary>
        private void HpacClientInstall()
        {
            UpdateStatus("Installing HPAC Client ...");
            List<UIMaps.UIMap> installScreenList = new List<UIMaps.UIMap>();

            installScreenList.Add(new HpacClient_WelcomePage());
            installScreenList.Add(new HpacClientSelectInstallationFolder());
            installScreenList.Add(new HpacClientServerSelection(_activityData.HpacJAServerName, _activityData.HpacIPMServerName, _activityData.HpacPullPrintServerName));
            installScreenList.Add(new HpacClientConfigSelection(_activityData.Quota, _activityData.Ipm, _activityData.Delegate, _activityData.LocalJobStorage));
            installScreenList.Add(new HpacClientConfirmInstallation());
            installScreenList.Add(new HpacClientInstallationComplete());

            for (int i = 0; i < installScreenList.Count; i++)
            {
                try
                {
                    UpdateStatus(string.Format("Triggering UI Action at Screen : {0}", installScreenList.ElementAt(i).ScreenName));
                    var result = installScreenList.ElementAt(i).PerformAction();

                    if (result.Result != PluginResult.Passed)
                    {
                        throw new Exception(result.Message);
                    }
                    UpdateStatus(string.Format("Completed UI Action at Screen : {0}", installScreenList.ElementAt(i).ScreenName));
                }
                catch (Exception exception)
                {
                    throw new Exception(string.Format("Failed to proceed beyond {0}. {1}", installScreenList.ElementAt(i).ScreenName, exception.Message), exception.InnerException ?? exception);
                }
            }
        }

        /// <summary>
        /// Update the Task Status
        /// </summary>
        /// <param name="statusMsg"></param>
        protected virtual void UpdateStatus(string statusMsg)
        {
            statusRichTextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status=" + statusMsg);
                c.AppendText($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")}  {statusMsg}\n");
                c.Select(c.Text.Length, 0);
                c.ScrollToCaret();
            });
        }

    }
}
