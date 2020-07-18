using System.ComponentModel;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Plugin.Fax
{
    /// <summary>
    /// Control used to perform and monitor the execution of the PluginFax activity.
    /// </summary>
    [ToolboxItem(false)]
    public partial class FaxExecControl : ScanExecutionControl, IPluginExecutionEngine
    {
        private FaxActivityData _faxActivityData = null;

        /// <summary>
        /// Initializes a new instance of this control.
        /// </summary>
        public FaxExecControl()
        {
            InitializeComponent();
        }

        FaxConfigPrintingEngine _engine = new FaxConfigPrintingEngine();

        /// <summary>
        /// Defines and executes the PluginFax workflow.
        /// </summary>
        /// <param name="executionData">Information used in the execution of this workflow.</param>
        /// <returns>The result of executing the workflow.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _faxActivityData = executionData.GetMetadata<FaxActivityData>(ConverterProvider.GetMetadataConverters());
            PrintQueueInfo item = executionData.PrintQueues.GetRandom();
            if (_faxActivityData.PrintJobSeparator)
            {
                _engine.PrintJobSeparator(executionData);
            }
            FaxScanManager manager;
            if (string.IsNullOrWhiteSpace(_faxActivityData.DigitalSendServer))
            {
                manager = new FaxScanManager(executionData);
            }
            else
            {
                manager = new FaxScanManager(executionData, _faxActivityData.DigitalSendServer);
            }

            manager.ActivityStatusChanged += UpdateStatus;
            manager.DeviceSelected += UpdateDevice;
            return manager.RunScanActivity();
        }

    }
}
