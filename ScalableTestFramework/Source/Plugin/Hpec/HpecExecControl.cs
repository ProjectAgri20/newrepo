using System.ComponentModel;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;

namespace HP.ScalableTest.Plugin.Hpec
{
    /// <summary>
    /// Execution control for the Hpec plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class HpecExecControl : ScanExecutionControl, IPluginExecutionEngine
    {
        /// <summary>
        /// Executes the HPEC plugin.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>HP.ScalableTest.Framework.Plugin.PluginExecutionResult.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            HpecActivityData data = executionData.GetMetadata<HpecActivityData>();
            ScanOptions scanOptions = new ScanOptions()
            {
                LockTimeouts = data.LockTimeouts,
                PageCount = data.PageCount,
                UseAdf = false,
            };

            HpecScanManager manager = new HpecScanManager(executionData, scanOptions);
            manager.ActivityStatusChanged += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;
            return manager.RunScanActivity();
        }
    }
}