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

namespace HP.ScalableTest.Plugin.HpacSimulation
{
    [ToolboxItem(false)]
    public partial class HpacSimulationExecutionControl : UserControl, IPluginExecutionEngine
    {
        private PluginExecutionData _executionData = null;
        private HpacSimulationController _hpacController = null;
        private StringBuilder _logText = new StringBuilder();

        public HpacSimulationExecutionControl()
        {
            InitializeComponent();
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _executionData = executionData;
            HpacSimulationActivityData activityData = _executionData.GetMetadata<HpacSimulationActivityData>();
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);

            IPrinterInfo assetInfo = null;
            ServerInfo hpacServer = null;
            try
            {
                assetInfo = (IPrinterInfo)_executionData.Assets.GetRandom();
                hpacServer = _executionData.Servers.First();
                LogUsageData(assetInfo, hpacServer);
            }
            catch (ArgumentNullException ex)
            {
                return new PluginExecutionResult(PluginResult.Error, ex);
            }
            catch (InvalidOperationException ex)
            {
                return new PluginExecutionResult(PluginResult.Error, ex);
            }

            if (_hpacController == null)
            {
                _hpacController = new HpacSimulationController(_executionData.Environment.UserDnsDomain, hpacServer.HostName, _executionData.Credential);
            }

            UpdateUI(hpacServer.HostName, assetInfo.AssetId, activityData.PullAllDocuments);
            UpdateStatus("Starting HPAC Pull Activity...");

            //On error or skip, Implement Retries
            PluginRetryManager retryManager = new PluginRetryManager(_executionData, UpdateStatus);
            result = retryManager.Run(() => ExecuteHandler(activityData, assetInfo));

            return result;
        }

        private PluginExecutionResult ExecuteHandler(HpacSimulationActivityData activityData, IPrinterInfo assetInfo)
        {
            HpacSimulationTask hpacSimTask = new HpacSimulationTask(_hpacController, activityData, assetInfo);
            hpacSimTask.StatusUpdate += (s, e) => UpdateStatus(e.StatusMessage);

            try
            {
                hpacSimTask.Execute();
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

        private void UpdateUI(string serverName, string assetId, bool printAll)
        {
            UpdateLabel(sessionId_Label, _executionData.SessionId);
            UpdateLabel(assetId_Label, assetId);
            UpdateLabel(hpacServer_Label, serverName);
            UpdateCheckBox(pullAll_CheckBox, printAll);
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

        private void UpdateCheckBox(CheckBox checkBox, bool isChecked)
        {
            // Make sure we're on the UI thread.
            if (checkBox.InvokeRequired)
            {
                checkBox.Invoke(new MethodInvoker(() => UpdateCheckBox(checkBox, isChecked)));
            }
            else
            {
                checkBox.Checked = isChecked;
                checkBox.Refresh();
            }
        }

        private void UpdateStatus(string text)
        {
            // Make sure we're on the UI thread.
            if (status_RichTextBox.InvokeRequired)
            {
                status_RichTextBox.Invoke(new MethodInvoker(() => UpdateStatus(text)));
            }
            else
            {
                _logText.Clear();
                _logText.Append(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                _logText.Append("  ");
                _logText.AppendLine(text);
                status_RichTextBox.AppendText(_logText.ToString());
                status_RichTextBox.Refresh();
                ExecutionServices.SystemTrace.LogDebug(text);

            }
        }

    }
}
