using System.ComponentModel;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;

namespace HP.ScalableTest.Plugin.AutoStore
{
    [ToolboxItem(false)]
    public partial class AutoStoreExecControl : ScanExecutionControl, IPluginExecutionEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoStoreExecControl" /> class.
        /// </summary>
        public AutoStoreExecControl()
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
            AutoStoreActivityData data = executionData.GetMetadata<AutoStoreActivityData>();

            ScanOptions scanOptions = new ScanOptions()
            {
                LockTimeouts = data.ScanOptions.LockTimeouts,
                PageCount = data.ScanOptions.PageCount,
                FileType = data.ScanOptions.FileType,
            };

            var manager = new AutoStoreScanManager(executionData);

            UpdateStatus("Starting activity.");

            manager.ActivityStatusChanged += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;

            PluginExecutionResult executionResult = manager.RunScanActivity();

            UpdateStatus("Finished activity.");
            UpdateStatus($"Result = {executionResult.Result}");

            return executionResult;
        }
    }
}
