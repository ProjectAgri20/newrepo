using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.Framework.Plugin;
using System.ComponentModel;

namespace HP.ScalableTest.Plugin.ScanToJobStorage
{
    /// <summary>
    /// Execution control for the ScanToJobStorage plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class ScanToJobStorageExecControl : ScanExecutionControl, IPluginExecutionEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToJobStorageExecControl"/> class.
        /// </summary>
        public ScanToJobStorageExecControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The execute method for Scan To JobStorage plugin.
        /// </summary>
        /// <param name="executionData">
        /// The execution data.
        /// </param>
        /// <returns>
        /// The <see cref="PluginExecutionResult"/>.
        /// </returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            ScanToJobStorageData data = executionData.GetMetadata<ScanToJobStorageData>(ConverterProvider.GetMetadataConverters());

            JobStorageScanManager manager = new JobStorageScanManager(executionData);
            manager.ActivityStatusChanged += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;
            return manager.RunScanActivity();
        }
    }
}
