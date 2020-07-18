using System;
using System.ComponentModel;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.Utility;


namespace HP.ScalableTest.Plugin.GeniusBytesScan
{
    [ToolboxItem(false)]
    public partial class GeniusBytesScanExecutionControl : ScanExecutionControl, IPluginExecutionEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeniusBytesScanExecutionControl" /> class.
        /// </summary>
        public GeniusBytesScanExecutionControl()
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
            GeniusBytesScanActivityData data = executionData.GetMetadata<GeniusBytesScanActivityData>();

            UpdateStatus("Starting activity.");

            ScanOptions scanOptions = new ScanOptions()
            {
                LockTimeouts = data.LockTimeouts,
                PageCount = data.ScanCount,
            };

            GeniusBytesScanManager manager = new GeniusBytesScanManager(executionData, scanOptions);
            manager.ActivityStatusChanged += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;

            PluginExecutionResult result = manager.RunScanActivity();

            UpdateStatus($"Result = {result.Result}");
            UpdateStatus("Finished activity.");
            return result;
        }

        /// <summary>
        /// Updates the status displayed in the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusChangedEventArgs" /> instance containing the event data.</param>
        protected new void UpdateStatus(object sender, StatusChangedEventArgs e)
        {
            UpdateStatus(e.StatusMessage);
        }
    }
}
