using System.ComponentModel;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Plugin.Copy
{
    /// <summary>
    /// Execution control for the Copy plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class CopyExecControl : ScanExecutionControl, IPluginExecutionEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CopyExecControl"/> class.
        /// </summary>
        public CopyExecControl()
        {
            InitializeComponent();
        }

        CopyConfigPrintingEngine _engine = new CopyConfigPrintingEngine();

        /// <summary>   
        /// The execute.
        /// </summary>
        /// <param name="executionData">
        /// The execution data.
        /// </param>
        /// <returns>
        /// The <see cref="PluginExecutionResult"/>.
        /// </returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            CopyData data = executionData.GetMetadata<CopyData>(ConverterProvider.GetMetadataConverters());
            PrintQueueInfo item = executionData.PrintQueues.GetRandom();
            ScanOptions scanOptions = new ScanOptions()
            {
                LockTimeouts = data.LockTimeouts,
                PageCount = data.PageCount
            };
            if (data.PrintJobSeparator)
            {
                _engine.PrintJobSeparator(executionData);
            }
            var manager = new CopyManager(executionData, scanOptions);

            manager.ActivityStatusChanged += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;
            return manager.RunScanActivity();
        }

    }
}
