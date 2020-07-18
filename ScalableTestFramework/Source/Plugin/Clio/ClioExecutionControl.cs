using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.ScalableTest.DeviceAutomation.LinkApps.VerticalConnector.Clio;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.Clio
{
    [ToolboxItem(false)]
    public partial class ClioExecutionControl : LinkExecutionControl, IPluginExecutionEngine
    {
        private ClioActivityData _data = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClioExecutionControl" /> class.
        /// </summary>
        public ClioExecutionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Executes this plugin's workflow using the specified <see cref="PluginExecutionData" />.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>A <see cref="PluginExecutionResult" /> indicating the outcome of the execution.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _data = executionData.GetMetadata<ClioActivityData>();

            if (_data.JobType.Equals(ClioJobType.Print))
            {
                return ExecutePrint(executionData);
            }
            else if (_data.JobType.Equals(ClioJobType.Scan))
            {
                return ExecuteScan(executionData);
            }

            return new PluginExecutionResult(PluginResult.Failed, $"Unrecognized Clio Job Type: {_data.JobType}", ConnectorExceptionCategory.FalseAlarm.GetDescription());
        }

        /// <summary>
        /// Execute Print Job.
        /// </summary>
        /// <returns></returns>
        public PluginExecutionResult ExecutePrint(PluginExecutionData excecutionData)
        {
            using (ClioPrintManager manager = new ClioPrintManager(excecutionData, _data.PrintOptions, _data.LockTimeouts))
            {
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
            using (ClioScanManager manager = new ClioScanManager(executionData, _data.ScanOptions, _data.LockTimeouts))
            {
                UpdateStatus("Starting execution to scan");
                manager.ActivityStatusChanged += UpdateStatus;
                manager.DeviceSelected += UpdateDevice;
                manager.AppNameSelected += UpdateAppName;
                return manager.RunLinkScanActivity();
            }
        }
    }
}
