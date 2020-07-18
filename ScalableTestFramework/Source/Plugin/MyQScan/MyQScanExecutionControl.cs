using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using HP.ScalableTest.PluginSupport.Scan;

namespace HP.ScalableTest.Plugin.MyQScan
{
    [ToolboxItem(false)]
    public partial class MyQScanExecutionControl : ScanExecutionControl, IPluginExecutionEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyQScanExecutionControl" /> class.
        /// </summary>
        public MyQScanExecutionControl()
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
            MyQScanActivityData data = executionData.GetMetadata<MyQScanActivityData>();

            UpdateStatus("Starting MyQ Scan activity.");

            ScanOptions scanOptions = new ScanOptions()
            {
                LockTimeouts = data.LockTimeouts,
            };

            MyQScanScanManager manager = new MyQScanScanManager(executionData, scanOptions);
            manager.ActivityStatusChanged += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;

            PluginExecutionResult executionResult = manager.RunScanActivity();
            UpdateStatus("Finished activity.");
            UpdateStatus($"Result = {executionResult.Result}");

            return executionResult;
        }

    }
}
