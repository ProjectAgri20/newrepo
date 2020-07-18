using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.CloudConnector
{
    /// <summary>
    /// Control used to perform and monitor the execution of the CloudConnector activity.
    /// </summary>
    public partial class CloudConnectorExecutionControl : LinkExecutionControl, IPluginExecutionEngine
    {
        private CloudConnectorActivityData _data = null;

        /// <summary>
        /// Initializes a new instance of this control.
        /// </summary>
        public CloudConnectorExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Defines and executes the CloudConnector workflow.
        /// </summary>
        /// <param name="executionData">Information used in the execution of this workflow.</param>
        /// <returns>The result of executing the workflow.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {            
            _data = executionData.GetMetadata<CloudConnectorActivityData>();
                        
            if (_data.CloudJobType.Equals(ConnectorJobType.Print.GetDescription()) || _data.CloudJobType.Equals(ConnectorJobType.MultiplePrint.GetDescription()))
            {
                return ExecutePrint(executionData);
            }

            else if (_data.CloudJobType.Equals(ConnectorJobType.Scan.GetDescription()))
            {
                return ExecuteScan(executionData);
            }

            return new PluginExecutionResult(PluginResult.Failed, $"Unrecognized Connector Job Type: {_data.CloudJobType}", ConnectorExceptionCategory.FalseAlarm.GetDescription());
        }

        /// <summary>
        /// Execute Print Job.
        /// </summary>
        /// <returns></returns>
        public PluginExecutionResult ExecutePrint(PluginExecutionData executionData)
        {
            using (CloudConnectorPrintManager manager = new CloudConnectorPrintManager(executionData, _data.CloudPrintOptions, _data.LockTimeouts)) { 
                UpdateStatus("Starting execution to print");
                manager.ActivityStatusChanged += UpdateStatus;
                manager.DeviceSelected += UpdateDevice;
                manager.AppNameSelected += UpdateAppName;
                return manager.RunLinkPrintActivity();
            }
        }

        /// <summary>
        /// Execute Scan Job.
        /// </summary>
        /// <returns></returns>
        public PluginExecutionResult ExecuteScan(PluginExecutionData executionData)
        {
            using (CloudConnectorScanManager manager = new CloudConnectorScanManager(executionData, _data.CloudScanOptions, _data.LockTimeouts)) { 
                UpdateStatus("Starting execution to scan");
                manager.ActivityStatusChanged += UpdateStatus;
                manager.DeviceSelected += UpdateDevice;
                manager.AppNameSelected += UpdateAppName;
                return manager.RunLinkScanActivity();
            }
        }
    }
}
