using System.ComponentModel;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;

namespace HP.ScalableTest.Plugin.ScanToWorkflow
{
    /// <summary>
    /// Execution control for the ScanToWorkflow plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class ScanToWorkflowExecControl : ScanExecutionControl, IPluginExecutionEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToWorkflowExecControl"/> class.
        /// </summary>
        public ScanToWorkflowExecControl()
        {
            InitializeComponent();
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            ScanToWorkflowData data = executionData.GetMetadata<ScanToWorkflowData>(ConverterProvider.GetMetadataConverters());
            ScanOptions scanOptions = new ScanOptions()
            {
                LockTimeouts = data.LockTimeouts,
                PageCount = data.PageCount,
                UseAdf = data.UseAdf,
            };

            WorkflowScanManager manager;
            if (string.IsNullOrWhiteSpace(data.DigitalSendServer))
            {
                manager = new WorkflowScanManager(executionData, scanOptions);
            }
            else
            {
                manager = new WorkflowScanManager(executionData, scanOptions, data.DigitalSendServer);
            }

            manager.ActivityStatusChanged += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;
            return manager.RunScanActivity();
        }
    }
}