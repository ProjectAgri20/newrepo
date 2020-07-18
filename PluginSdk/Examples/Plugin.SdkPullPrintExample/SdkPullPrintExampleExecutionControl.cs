/*
* © Copyright 2016 HP Development Company, L.P.
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using OXPt;
using OXPt.Helpers;

namespace Plugin.SdkPullPrintExample
{
    /// <summary>
    /// Control used to perform and monitor device control panel execution of the OXPd PullPrint demo solution.
    /// </summary>
    public partial class SdkPullPrintExampleExecutionControl : UserControl, IPluginExecutionEngine
    {
        private SdkPullPrintExampleActivityData _data = null;

        /// <summary>
        /// Initializes a new instance of this control.
        /// </summary>
        public SdkPullPrintExampleExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Defines and executes the LocalPullPrintExample workflow.
        /// </summary>
        /// <param name="executionData">Information used in the execution of this workflow.</param>
        /// <returns>The result of executing the workflow.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            var result = new PluginExecutionResult(PluginResult.Error, "Unknown");
            _data = executionData.GetMetadata<SdkPullPrintExampleActivityData>();

            UpdateStatus("Starting execution");

            // Get a random device from the selected assets and log it
            var device = executionData.Assets.GetRandom() as HP.ScalableTest.Framework.Assets.IDeviceInfo;
            UpdateStatus($"Selecting and connecting to device {device.AssetId} ({device.Address})...");
            IDeviceProxy proxy = DeviceProxyFactory.Create(device.Address, device.AdminPassword);

            // Define the parameters for a device lock token
            var token = new AssetLockToken(device, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));

            // Acquire a lock on the device to prevent other STB activities that require exclusive access
            // Perform the action and capture the result
            ExecutionServices.CriticalSection.Run(token, () =>
                {
                    result = PerformPullPrint(executionData, result, device, proxy);
                }
            );

            return result;
        }

        /// <summary>
        /// Performs the pull print.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="result">The result.</param>
        /// <param name="device">The device.</param>
        /// <param name="proxy">The proxy.</param>
        /// <returns>HP.ScalableTest.Framework.Plugin.PluginExecutionResult.</returns>
        private PluginExecutionResult PerformPullPrint(PluginExecutionData executionData, PluginExecutionResult result, HP.ScalableTest.Framework.Assets.IDeviceInfo device, IDeviceProxy proxy)
        {
            try
            {
                // Log the device being used
                ExecutionServices.DataLogger.Submit(new ActivityExecutionAssetUsageLog(executionData, device));
                UpdateStatus($"Executing as user [{executionData?.Credential?.UserName}]");

                // Reset and make sure we're on home screen
                proxy.DeviceUI.PressKey(KeyCode.ResetKey);
                proxy.DeviceUI.WaitForHomeScreen(TimeSpan.FromSeconds(15));

                // Press the PullPrint example solution button
                var solutionButton = _data.TopLevelButtonName;
                UpdateStatus($"Pressing button [{solutionButton}]");
                proxy.DeviceUI.NavigateToSolution(solutionButton);
                proxy.BrowserUI.WaitForIdle(TimeSpan.FromSeconds(15));

                // Get the list of documents available to print
                UpdateStatus("Checking for available documents");
                Dictionary<string, string>[] checkboxes = proxy.BrowserUI.GetElements("span[class='checkbox']");
                UpdateStatus($"{checkboxes.Length} documents found");

                // Set result to skip if no documents found
                if (checkboxes.Length <= 0)
                {
                    result = new PluginExecutionResult(PluginResult.Skipped, "No documents found to print");
                }
                else
                {
                    UpdateStatus("Checking the the first checkbox");
                    // click the first item in the list to check the checkbox                    
                    proxy.BrowserUI.ClickElement(checkboxes[0]["id"]);

                    // press the print button
                    UpdateStatus("Clicking the print button");
                    proxy.BrowserUI.ClickElement("printButton");

                    // wait for idle for the job to finish
                    proxy.DeviceProperties.WaitForIdle(TimeSpan.FromSeconds(30));
                    result = new PluginExecutionResult(PluginResult.Passed);
                }
            }
            finally
            {
                try
                {
                    // Navigate through device sign out procedure (if applicable) and return to home screen
                    proxy.DeviceUI.NavigateSignOut();
                    proxy.DeviceUI.WaitForHomeScreen(TimeSpan.FromSeconds(10));
                }
                catch { }

                UpdateStatus("Execution complete");
                UpdateStatus("Result = " + result.Result.ToString());
            }

            return result;
        }

        /// <summary>
        /// Logs and displays status messages.
        /// </summary>
        /// <param name="text">The status message to be logged and displayed.</param>
        protected virtual void UpdateStatus(string text)
        {
            Action displayText = new Action(() =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status=" + text);
                var msg = $"{DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff")}  {text}\n";
                status_RichTextBox.AppendText(msg);
                status_RichTextBox.Refresh();
            });

            if (status_RichTextBox.InvokeRequired)
            {
                status_RichTextBox.Invoke(displayText);
            }
            else
            {
                displayText();
            }
        }
    }
}
