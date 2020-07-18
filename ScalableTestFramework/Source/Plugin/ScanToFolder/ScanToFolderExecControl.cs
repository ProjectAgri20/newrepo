using System.ComponentModel;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;

namespace HP.ScalableTest.Plugin.ScanToFolder
{
    /// <summary>
    /// Execution control for the ScanToFolder plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class ScanToFolderExecControl : ScanExecutionControl, IPluginExecutionEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToFolderExecControl"/> class.
        /// </summary>
        public ScanToFolderExecControl()
        {
            InitializeComponent();
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            ScanToFolderData data = executionData.GetMetadata<ScanToFolderData>(ConverterProvider.GetMetadataConverters());

            var manager = string.IsNullOrWhiteSpace(data.DigitalSendServer)
                          ? new NetworkFolderScanManager(executionData)
                          : new NetworkFolderScanManager(executionData, data.DigitalSendServer);

            manager.ActivityStatusChanged += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;
            return manager.RunScanActivity();
        }
    }
}