using System.ComponentModel;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;

namespace HP.ScalableTest.Plugin.ScanToHpcr
{
    /// <summary>
    /// Execution control for the ScanToHpcr plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class ScanToHpcrExecControl : ScanExecutionControl, IPluginExecutionEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanToHpcrExecControl"/> class.
        /// </summary>
        public ScanToHpcrExecControl()
        {
            InitializeComponent();
        }

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            ScanToHpcrActivityData data = executionData.GetMetadata<ScanToHpcrActivityData>();

            ScanOptions scanOptions = new ScanOptions()
            {
                LockTimeouts = data.LockTimeouts,
                PageCount = data.PageCount,
            };
            var manager = new HpcrScanManager(executionData, scanOptions);

            manager.ActivityStatusChanged += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;

            PluginExecutionResult result = manager.RunScanActivity();

            return result;
        }

    }
}