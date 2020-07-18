using System.ComponentModel;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;

namespace HP.ScalableTest.Plugin.ScanToUsb
{
    [ToolboxItem(false)]
    public partial class ScanToUsbExecControl : ScanExecutionControl, IPluginExecutionEngine
    {
        public ScanToUsbExecControl()
        {
            InitializeComponent();
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            ScanToUsbData data = executionData.GetMetadata<ScanToUsbData>(ConverterProvider.GetMetadataConverters());

            UsbScanManager manager;
            if (string.IsNullOrWhiteSpace(data.DigitalSendServer))
            {
                manager = new UsbScanManager(executionData);
            }
            else
            {
                manager = new UsbScanManager(executionData, data.DigitalSendServer);
            }

            manager.ActivityStatusChanged += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;
            return manager.RunScanActivity();
        }
    }
}
