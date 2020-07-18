using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Plugin.HpEasyStart.UIMaps;
using HP.ScalableTest.Utility;
using TopCat.TestApi.GUIAutomation;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;

namespace HP.ScalableTest.Plugin.HpEasyStart
{
    /// <summary>
    ///  Execution Engine Class for HP Easy Start
    /// </summary>
    [ToolboxItem(false)]
    public partial class HpEasyStartExecControl : UserControl, IPluginExecutionEngine
    {
        private HpEasyStartActivityData _activityData;

        /// <summary>
        /// Initializes a new instance of the HpEasyStartExecControl class.
        /// </summary>
        public HpEasyStartExecControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Execute call for HP Easy Start Execution Engine 
        /// </summary>
        /// <param name="executionData"></param>
        /// <returns></returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _activityData = executionData.GetMetadata<HpEasyStartActivityData>();
            UpdateStatus("Retrieving the Printer Information");

            //Retrieving the printer Randomly from the Selected Assets List
            Framework.Assets.IDeviceInfo printer = executionData.Assets.GetRandom<Framework.Assets.IDeviceInfo>();
            IDevice device = DeviceConstructor.Create(printer);
            string modelName = device.GetDeviceInfo().ModelName;

            UpdateStatus(_activityData.IsWebPackInstallation ? "Retrieving the Full Web Pack Installer Information" : "Retrieving the HP Easy Start Installer Information");
            if (!File.Exists(_activityData.HpEasyStartInstallerPath))
            {
                return new PluginExecutionResult(PluginResult.Failed, "Could not find the HP Easy Start Installer file/Web Pack file");
            }

            TopCatUIAutomation.Initialize();
            UpdateStatus(_activityData.IsWebPackInstallation ? "Copying Full Web Pack Installer to Local Temp Path" : "Copying HP Easy Start Installer to Local Temp Path");
            string localSetupFile = Path.Combine(Path.GetTempPath(), Path.GetFileName(_activityData.HpEasyStartInstallerPath));
            if (!File.Exists(localSetupFile))
            {
                File.Copy(_activityData.HpEasyStartInstallerPath, localSetupFile);
            }

            UpdateStatus(_activityData.IsWebPackInstallation ? "Launching the Full Web Pack Installer" : "Launching the HP Easy Start Installer");
            ProcessStartInfo pStartInfo = new ProcessStartInfo(localSetupFile)
            {
                UseShellExecute = true,
                Verb = "runas"
            };
            Process.Start(pStartInfo);

            //Collate all the UI Maps to the TopCat Related Activity
            List<UIMaps.UIMap> installScreenList = new List<UIMaps.UIMap>
            {
                new HpEasyStartInstall("HP Easy Start Driver Download", printer.Address),
                new HpEasyDriverInstall("HP Easy Start Driver Installation", printer.Address,modelName,_activityData.PrintTestPage, _activityData.SetAsDefaultDriver)
            };

            for (int i = 0; i < installScreenList.Count; i++)
            {
                if (_activityData.IsWebPackInstallation && i == 0)
                {
                    continue;
                }
                else
                {
                    try
                    {
                        var result = installScreenList.ElementAt(i).PerformAction();
                        if (result.Result != PluginResult.Passed)
                        {
                            return result;
                        }
                        Thread.Sleep(20);
                    }
                    catch (Exception)
                    {
                        return new PluginExecutionResult(PluginResult.Error, $"Failed to proceed beyond {installScreenList.ElementAt(i).ScreenName}");
                    }
                }
            }
            return new PluginExecutionResult(PluginResult.Passed);
        }

        /// <summary>
        /// Updates the status text in the execution control display.
        /// </summary>
        /// <param name="statusMsg"></param>
        protected virtual void UpdateStatus(string statusMsg)
        {
            status_richTextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status=" + statusMsg);
                c.AppendText($"{DateTime.Now:MM/dd/yyyy HH:mm:ss.fff}  {statusMsg}\n");
                c.Select(c.Text.Length, 0);
                c.ScrollToCaret();
            });
        }
    }
}
