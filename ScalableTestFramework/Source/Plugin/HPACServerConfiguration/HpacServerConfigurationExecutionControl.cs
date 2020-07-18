using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Plugin.HpacServerConfiguration
{
    /// <summary>
    /// Execution control for the HpacServerConfiguration plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class HpacServerConfigurationExecutionControl : UserControl, IPluginExecutionEngine
    {
        private PluginExecutionData _executionData = null;
        private HpacServerConfigurationController _hpacController = null;
        private StringBuilder _logText = new StringBuilder();
        ServerInfo _hpacServer;
        private string _hostName;
        /// <summary>
        /// Initializes a new instance of the <see cref="HpacServerConfigurationExecutionControl"/> class.
        /// </summary>
        public HpacServerConfigurationExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Processes the activity given to the plugin.
        /// </summary>
        /// <param name="executionData"></param>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _executionData = executionData;
            HpacServerConfigurationActivityData activityData = _executionData.GetMetadata<HpacServerConfigurationActivityData>();
            if (executionData.Assets.Count > 0)
            {
                activityData.DeviceData.Asset = (IDeviceInfo) executionData.Assets.FirstOrDefault();
            }

            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);

           

            
            try
            {
                _hpacServer = _executionData.Servers.First();
                _hostName = _hpacServer.HostName;
                if (!_hostName.Contains(executionData.Environment.UserDnsDomain))
                {
                    _hostName += "." + executionData.Environment.UserDnsDomain;
                }
                LogUsageData(_hpacServer);
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
                _hpacController = new HpacServerConfigurationController(_executionData.Environment.UserDnsDomain, _hostName, _hpacServer.Address, _executionData.Credential);
            }

            UpdateUI(_hostName, activityData.HpacConfigTile.ToString());
            UpdateStatus("Starting HPAC Server Configuring  Activity...");

            var retryManager = new PluginRetryManager(executionData, UpdateStatus);
            result = retryManager.Run(() => ExecuteTask(activityData));

            return result;
        }

        private PluginExecutionResult ExecuteTask(HpacServerConfigurationActivityData activityData)
        {
            HpacServerConfigurationTask hpacServConfigTask = new HpacServerConfigurationTask(_hpacController, activityData);
            hpacServConfigTask.StatusUpdate += (s, e) => UpdateStatus(e.StatusMessage);


            try
            {
                ExecutionServices.CriticalSection.Run(new GlobalLockToken(_hostName, TimeSpan.FromMinutes(20), TimeSpan.FromMinutes(10)), () => {hpacServConfigTask.Execute();} );
            }
            catch (InvalidOperationException ex)
            {
                return new PluginExecutionResult(PluginResult.Error, ex);
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }

        private void LogUsageData(ServerInfo hpacServer)
        {
            //Log ePrintServer usage
            ActivityExecutionServerUsageLog serverLog = new ActivityExecutionServerUsageLog(_executionData, hpacServer);
            ExecutionServices.DataLogger.Submit(serverLog);
        }

        private void UpdateUI(string serverName, string hpacTile)
        {
            UpdateLabel(sessionId_Label, _executionData.SessionId);
            UpdateLabel(hpacServer_Label, serverName);
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
            if (status_RichTextBox.InvokeRequired)
            {
                status_RichTextBox.Invoke(new MethodInvoker(() => UpdateStatus(text)));
            }
            else
            {
                _logText.Clear();
                _logText.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
                _logText.Append("  ");
                _logText.AppendLine(text);
                status_RichTextBox.AppendText(_logText.ToString());
                status_RichTextBox.Refresh();
                ExecutionServices.SystemTrace.LogDebug(text);
            }
        }
    }
}