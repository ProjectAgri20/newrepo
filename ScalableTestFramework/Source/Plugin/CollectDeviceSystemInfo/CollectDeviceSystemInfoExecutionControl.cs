using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Plugin.CollectDeviceSystemInfo;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.CollectDeviceSystemInfo
{
    /// <summary>
    /// Control used to perform and monitor the execution of the PluginCollectDeviceSystemInfo activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class CollectDeviceSystemInfoExecutionControl : UserControl, IPluginExecutionEngine
    {
        private CollectDeviceSystemInfoActivityData _data = null;
        protected StringBuilder _logText = new StringBuilder();

        private CollectDeviceSystemInfoEngine _collectDeviceInfoEngine;


        /// <summary>
        /// Initializes a new instance of this control.
        /// </summary>
        public CollectDeviceSystemInfoExecutionControl()
        {
            InitializeComponent();           
        }

        /// <summary>
        /// Defines and executes the PluginCollectDeviceSystemInfo workflow.
        /// </summary>
        /// <param name="executionData">Information used in the execution of this workflow.</param>
        /// <returns>The result of executing the workflow.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _data = executionData.GetMetadata<CollectDeviceSystemInfoActivityData>(ConverterProvider.GetMetadataConverters());
            _collectDeviceInfoEngine = new CollectDeviceSystemInfoEngine(executionData);

            UpdateStatus("Starting execution");

            // iterate through each applicable asset, acquire the lock and take the memory snapshot, then proceed to next
            var devices = executionData.Assets.OfType<IDeviceInfo>();

            Parallel.ForEach(devices, device =>
            {
                UpdateStatus("Collecting Memory on Asset ID '" + device.AssetId + "'.");
                _collectDeviceInfoEngine.ProcessMemCollectionByDevice(device);
                if (_collectDeviceInfoEngine.IsError)
                {                    
                    UpdateStatus(_collectDeviceInfoEngine.ErrorMessage);
                    _collectDeviceInfoEngine.ErrorMessage = string.Empty;
                }
            });


            PluginExecutionResult result;
            if (_collectDeviceInfoEngine.ProblemDevices.Any())
            {
                var msg = $"Failed to collect on {string.Join(",", _collectDeviceInfoEngine.ProblemDevices.ToArray())}";
                result = new PluginExecutionResult(PluginResult.Failed, msg, "Memory collection error.");
                UpdateStatus(msg);
            }
            else
            {
                result = new PluginExecutionResult(PluginResult.Passed);
            }

            UpdateStatus("Completing execution");
            UpdateStatus($"Result = {result.Result}");
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
