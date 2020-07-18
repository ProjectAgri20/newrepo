using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.CollectDeviceJobInfo
{
    /// <summary>
    /// Control used to perform and monitor the execution of the PluginCollectDeviceJobInfo activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class CollectDeviceJobInfoExecutionControl : UserControl, IPluginExecutionEngine
    {
        private CollectDeviceJobInfoActivityData _data = null;
        protected StringBuilder _logText = new StringBuilder();
        private CollectDeviceJobInfoEngine _collectDeviceJobInfoEngine;

        /// <summary>
        /// Initializes a new instance of this control.
        /// </summary>
        public CollectDeviceJobInfoExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Defines and executes the PluginCollectDeviceJobInfo workflow.
        /// </summary>
        /// <param name="executionData">Information used in the execution of this workflow.</param>
        /// <returns>The result of executing the workflow.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _data = executionData.GetMetadata<CollectDeviceJobInfoActivityData>();
            _collectDeviceJobInfoEngine = new CollectDeviceJobInfoEngine(executionData);
            UpdateStatus("Starting the execution of job collection plugin...");

            // iterate through each applicable asset, acquire the lock and take the memory snapshot, then proceed to next
            var devices = executionData.Assets.OfType<IDeviceInfo>();

            Parallel.ForEach(devices, device =>
            {
                UpdateStatus("Collecting Job Info on Asset ID '" + device.AssetId + "'.");
                try
                {
                    _collectDeviceJobInfoEngine.ProcessJobCollectionByDevice(device);
                }
                catch (Exception ex)
                {
                    _collectDeviceJobInfoEngine.ProblemDevices.Add(device.Address, ex.ToString());
                }
            });

            PluginExecutionResult result;
            if (_collectDeviceJobInfoEngine.ProblemDevices.Any())
            {
                var msg = $"Failed to collect on {string.Join(",", _collectDeviceJobInfoEngine.ProblemDevices.ToArray())}";
                ExecutionServices.SystemTrace.LogError(msg);
                result = new PluginExecutionResult(PluginResult.Failed, msg, "Job info collection error.");
                UpdateStatus(msg);
            }
            else
            {
                result = new PluginExecutionResult(PluginResult.Passed);
            }

            UpdateStatus($"Execution complete. Result = {result.Result}");
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
                _logText.Clear();
                _logText.AppendFormat("{0}  {1}\n", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), text);
                status_RichTextBox.AppendText(_logText.ToString());
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
