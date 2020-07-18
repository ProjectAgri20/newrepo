using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.PullPrint.Simulation;

namespace HP.ScalableTest.Plugin.SafeComSimulation
{
    [ToolboxItem(false)]
    public partial class SafeComSimulationExecutionControl : UserControl, IPluginExecutionEngine
    {
        private PluginExecutionData _executionData = null;
        private SafeComSimulationController _safecomController = null;
        private StringBuilder _logText = new StringBuilder();

        public SafeComSimulationExecutionControl()
        {
            InitializeComponent();
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _executionData = executionData;
            SafeComSimulationActivityData activityData = _executionData.GetMetadata<SafeComSimulationActivityData>();
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);

            DeviceInfo assetInfo = null;
            ServerInfo safecomServer = null;

            try
            {
                assetInfo = (DeviceInfo)_executionData.Assets.GetRandom();
                safecomServer = _executionData.Servers.First();
                LogUsageData(assetInfo, safecomServer);
            }
            catch (ArgumentNullException ex)
            {
                return new PluginExecutionResult(PluginResult.Error, ex);
            }
            catch (InvalidOperationException ex)
            {
                return new PluginExecutionResult(PluginResult.Error, ex);
            }

            if (_safecomController == null)
            {
                _safecomController = new SafeComSimulationController(_executionData.Credential, activityData.SafeComAuthenticationMode, assetInfo.Address, activityData.AssetMacAddress, safecomServer.HostName);
            }

            UpdateUI(safecomServer.HostName, assetInfo.Address);
            UpdateStatus("Starting SafeCom Pull Activity...");

            AssetLockToken lockToken = new AssetLockToken(assetInfo, TimeSpan.FromHours(12), TimeSpan.FromHours(12));

            //On error, Implement Retries before relinquishing the lock.
            ExecutionServices.CriticalSection.Run(lockToken, () =>
            {
                PluginRetryManager retryManager = new PluginRetryManager(_executionData, UpdateStatus);
                result = retryManager.Run(() => ExecuteTask(activityData, assetInfo));
            });

            return result;
        }

        private PluginExecutionResult ExecuteTask(SafeComSimulationActivityData activityData, IDeviceInfo assetInfo)
        {
            SafeComSimulationTask safecomSimTask = new SafeComSimulationTask(_safecomController, activityData, (IPrinterInfo)assetInfo);
            safecomSimTask.StatusUpdate += (s, e) => UpdateStatus(e.StatusMessage);

            try
            {
                safecomSimTask.Execute();
            }
            catch (EmptyQueueException ex)
            {
                return new PluginExecutionResult(PluginResult.Skipped, ex);
            }
            catch (PullPrintingSimulationException ex)
            {
                return new PluginExecutionResult(PluginResult.Error, ex);
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }

        private void LogUsageData(IAssetInfo asset, ServerInfo hpacServer)
        {
            //Log device usage
            ActivityExecutionAssetUsageLog deviceLog = new ActivityExecutionAssetUsageLog(_executionData, asset);
            ExecutionServices.DataLogger.Submit(deviceLog);

            //Log ePrintServer usage
            ActivityExecutionServerUsageLog serverLog = new ActivityExecutionServerUsageLog(_executionData, hpacServer);
            ExecutionServices.DataLogger.Submit(serverLog);
        }

        private void UpdateUI(string serverName, string deviceIP)
        {
            UpdateLabel(session_Label, _executionData.SessionId);
            UpdateLabel(server_Label, serverName);
            UpdateLabel(deviceIP_Label, deviceIP);
        }

        private void UpdateLabel(Label label, string text)
        {
            // Make sure we're on the UI thread.
            if (label.InvokeRequired)
            {
                label.Invoke(new MethodInvoker(() => UpdateLabel(label, text)));
            }
            else
            {
                label.Text = text;
                label.Refresh();
            }
        }

        private void UpdateStatus(string text)
        {
            // Make sure we're on the UI thread.
            if (status_TextBox.InvokeRequired)
            {
                status_TextBox.Invoke(new MethodInvoker(() => UpdateStatus(text)));
            }
            else
            {
                status_TextBox.AppendText(text);
                status_TextBox.AppendText(Environment.NewLine);
                status_TextBox.Refresh();

                ExecutionServices.SystemTrace.LogDebug(text);
            }
        }

        private static string GetUserPin(string userName)
        {
            return userName.Substring(1);
        }
    }
}
