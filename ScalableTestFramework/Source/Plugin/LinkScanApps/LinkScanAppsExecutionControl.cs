using System;
using System.ComponentModel;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.LinkScanApps
{
    [ToolboxItem(false)]
    public partial class LinkScanAppsExecutionControl : LinkExecutionControl, IPluginExecutionEngine
    {
        private LinkScanAppsActivityData _data = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkScanAppsExecutionControl" /> class.
        /// </summary>
        public LinkScanAppsExecutionControl()
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
            _data = executionData.GetMetadata<LinkScanAppsActivityData>();

            UpdateStatus("Starting activity.");

            PluginExecutionResult executionResult = new PluginExecutionResult(PluginResult.Passed);

            return ExecuteScan(executionData);
        }

        /// <summary>
        /// Execute Scan Job.
        /// </summary>
        /// <param name="executionData">PluginExecutionData</param>
        /// <returns>A <see cref="PluginExecutionResult" /> RunLinkScanActivity</returns>
        public PluginExecutionResult ExecuteScan(PluginExecutionData executionData)
        {
            using (LinkScanAppsScanManager manager = new LinkScanAppsScanManager(executionData, _data.ScanOptions, _data.LockTimeouts))
            {
                UpdateStatus("Starting execution to scan");
                manager.ActivityStatusChanged += UpdateStatus;
                manager.DeviceSelected += UpdateDevice;
                manager.AppNameSelected += UpdateAppName;
                return manager.RunLinkScanActivity();
            }
        } 
    }
}
