using System.ComponentModel;
using HP.ScalableTest.DeviceAutomation.LinkApps.VerticalConnector.iManage;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.iManage
{
    [ToolboxItem(false)]
    public partial class iManageExecutionControl : LinkExecutionControl, IPluginExecutionEngine
    {
        private iManageActivityData _data = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="iManageExecutionControl" /> class.
        /// </summary>
        public iManageExecutionControl()
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
            _data = executionData.GetMetadata<iManageActivityData>();

            if (_data.JobType.Equals(iManageJobType.Print))
            {
                return ExecutePrint(executionData);
            }
            else if (_data.JobType.Equals(iManageJobType.Scan))
            {
                return ExecuteScan(executionData);
            }

            return new PluginExecutionResult(PluginResult.Failed, $"Unrecognized IManage Job Type: {_data.JobType}", iManageExceptionCategory.FalseAlarm.GetDescription());
        }

        /// <summary>
        /// Execute Print Job.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns></returns>
        public PluginExecutionResult ExecutePrint(PluginExecutionData executionData)
        {
            using (iManagePrintManager manager = new iManagePrintManager(executionData, _data.PrintOptions, _data.LockTimeouts))
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
            using (iManageScanManager manager = new iManageScanManager(executionData, _data.ScanOptions, _data.LockTimeouts))
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
