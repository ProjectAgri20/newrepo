using System.ComponentModel;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;

namespace HP.ScalableTest.Plugin.UdocxScan
{
    [ToolboxItem(false)]
    public partial class UdocxScanExecutionControl : ScanExecutionControl, IPluginExecutionEngine
    {
        UdocxScanActivityData data = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="UdocxScanExecutionControl" /> class.
        /// </summary>
        public UdocxScanExecutionControl()
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
            data = executionData.GetMetadata<UdocxScanActivityData>();

            UpdateStatus("Starting activity");

            ScanOptions scanOptions = new ScanOptions()
            {
                LockTimeouts = data.LockTimeouts,
                //PageCount = data.ScanCount,
            };
            
            var manager = new UdocxScanManager(executionData, scanOptions);
            manager.ActivityStatusChanged += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;
            PluginExecutionResult result = manager.RunScanActivity();
            return result;
        }
    }
}
