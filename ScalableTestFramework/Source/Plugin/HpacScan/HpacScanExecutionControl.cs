using System;
using System.ComponentModel;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;

namespace HP.ScalableTest.Plugin.HpacScan
{
    [ToolboxItem(false)]
    public partial class HpacScanExecutionControl : ScanExecutionControl, IPluginExecutionEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HpacScanExecutionControl" /> class.
        /// </summary>
        public HpacScanExecutionControl()
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
            HpacScanActivityData data = executionData.GetMetadata<HpacScanActivityData>();
            UpdateStatus("Starting activity.");

            ScanOptions scanOptions = new ScanOptions()
            {
                LockTimeouts = data.LockTimeouts,
            };

            HpacScanScanManager manager = new HpacScanScanManager(executionData, scanOptions);
            manager.ActivityStatusChanged += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;

            PluginExecutionResult result = manager.RunScanActivity();
            UpdateStatus($"Result = {result.Result}");
            return result;
        }
    }
}
