using System.ComponentModel;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;

namespace HP.ScalableTest.Plugin.ScanToEmail
{
    /// <summary>
    /// Execution control for the ScanToEmail plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class ScanToEmailExecControl : ScanExecutionControl, IPluginExecutionEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToEmailExecControl"/> class.
        /// </summary>
        public ScanToEmailExecControl()
        {
            InitializeComponent();
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            ScanToEmailData data = executionData.GetMetadata<ScanToEmailData>(ConverterProvider.GetMetadataConverters());

            var manager = string.IsNullOrWhiteSpace(data.DigitalSendServer)
                          ? new EmailScanManager(executionData)
                          : new EmailScanManager(executionData, data.DigitalSendServer);

            manager.ActivityStatusChanged += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;
            return manager.RunScanActivity();
        }
    }
}