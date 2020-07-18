using System.ComponentModel;
using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.ScanToSafeQ
{
    [ToolboxItem(false)]
    public partial class ScanToSafeQExecutionControl : ScanExecutionControl, IPluginExecutionEngine
    {

        ScanToSafeQActivityData data = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToSafeQExecutionControl" /> class.
        /// </summary>
        public ScanToSafeQExecutionControl()
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
            data = executionData.GetMetadata<ScanToSafeQActivityData>();
            UpdateStatus("Starting activity.");

            ScanOptions scanOptions = new ScanOptions()
            {
                LockTimeouts = data.LockTimeouts,
                PageCount = data.ScanCount,
            };

            var manager = new ScanToSafeQScanManager(executionData, scanOptions);
            manager.ActivityStatusChanged += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;

            PluginExecutionResult result = manager.RunScanActivity();
            UpdateStatus($"Result = {result.Result}");
            return result;
        }
    }
}
