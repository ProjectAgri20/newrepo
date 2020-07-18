using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using System;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.PluginSupport.MobilePrintApp
{
    /// <summary>
    /// Mobile Print App Print Activity Manager
    /// </summary>
    public abstract class MobilePrintActivityManager : IDisposable
    {
        /// <summary>
        /// Represent Mobile Print App such as PrinterOn, HP ePrint
        /// </summary>
        protected IMobilePrintApp App;

        /// <summary>
        /// Target to Print from mobile
        /// </summary>
        protected ObjectToPrint Target;

        /// <summary>
        /// Mobile Print job option
        /// </summary>
        protected MobilePrintJobOptions Option;

        /// <summary>
        /// Printer ID which will used for select target printer on mobile app
        /// </summary>
        protected string PrinterId;

        /// <summary>
        /// Mobile device identifier which mobile print app is installed
        /// </summary>
        protected string DeviceIdentifier;

        /// <summary>
        /// Gets the <see cref="PluginExecutionData" /> for this activity.
        /// </summary>
        protected PluginExecutionData ExecutionData { get; }

        /// <summary>
        /// Gets the <see cref="DeviceWorkflowLogger" /> for this activity.
        /// </summary>
        protected DeviceWorkflowLogger WorkflowLogger { get; }

        /// <summary>
        /// Occurs when the activity status changes.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> ActivityStatusChanged;

        /// <summary>
        /// Initialize Print Activity manager
        /// </summary>
        /// <param name="executionData">Execution data</param>
        protected MobilePrintActivityManager(PluginExecutionData executionData)
        {
            ExecutionData = executionData;
            WorkflowLogger = new DeviceWorkflowLogger(executionData);
            
        }

        /// <summary>
        /// Run Mobile print activity
        /// </summary>
        /// <returns><see cref="PluginExecutionResult"/> for mobile print job</returns>
        public virtual PluginExecutionResult RunMobilePrintActivity()
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Automation Execution Failure", "Device workflow error.");
            UpdateStatus("Starting Mobile Print Activity");
            
            DeviceSimulatorInfo device = ExecutionData.Assets.GetRandom<DeviceSimulatorInfo>();
            DeviceIdentifier = device.Address;
            UpdateStatus($"Target Mobile Device : {DeviceIdentifier}");
            try
            {
                SetupJob();
                WorkflowLogger.RecordEvent(DeviceWorkflowMarker.AppButtonPress);
                UpdateStatus("Launching App");
                App.LaunchApp();
                WorkflowLogger.RecordEvent(DeviceWorkflowMarker.AppShown);
                UpdateStatus("Select Object to print");
                App.SelectTargetToPrint(Target);
                UpdateStatus("Select Printer");
                App.SelectPrinter(PrinterId);
                UpdateStatus("Set Options");
                App.SetOptions(Option);
                WorkflowLogger.RecordEvent(DeviceWorkflowMarker.PrintJobBegin);
                WorkflowLogger.RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
                UpdateStatus("Click Print button");
                App.Print();
                WorkflowLogger.RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
                UpdateStatus("Wait until print job is done");
                App.WaitUntilPrintDone(TimeSpan.FromMinutes(3));
                WorkflowLogger.RecordEvent(DeviceWorkflowMarker.PrintJobEnd);
                if(App.CheckPrintStatusOnMobile())
                {
                    UpdateStatus("Print Job Success");
                    result = new PluginExecutionResult(PluginResult.Passed);
                }
                else
                {
                    UpdateStatus("Print Job Fail");
                    result = new PluginExecutionResult(PluginResult.Failed, "Fail to upload job");
                }
                App.CloseApp();
            }
            catch(MobileWorkflowException ex)
            {
                UpdateStatus($"Exception : {ex.Message}");
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Mobile Workflow Error");
            }
            catch(NotSupportedException ex)
            {
                UpdateStatus($"Exception : {ex.Message}");
                result = new PluginExecutionResult(PluginResult.Error, ex, "Plugin code error");
            }
            catch(Exception ex)
            {
                UpdateStatus($"Exception : {ex.Message}");
                result = new PluginExecutionResult(PluginResult.Error, ex, "Unknown error");
            }
            
            return result;

        }

        /// <summary>
        /// Setup pre-condition
        /// </summary>
        public abstract void SetupJob();


        /// <summary>
        /// Dispose apps
        /// </summary>
        public virtual void Dispose()
        {
            if(App != null)
            {
                App.Dispose();
            }
            
        }

        /// <summary>
        /// Records a performance event with the specified <see cref="DeviceWorkflowMarker" />.
        /// </summary>
        /// <param name="marker">The <see cref="DeviceWorkflowMarker" />.</param>
        protected void RecordEvent(DeviceWorkflowMarker marker)
        {
            WorkflowLogger.RecordEvent(marker);
        }

        /// <summary>
        /// Updates the status of the plugin execution.
        /// </summary>
        /// <param name="status">The status.</param>
        protected void UpdateStatus(string status)
        {
            LogInfo(status);
            ActivityStatusChanged?.Invoke(this, new StatusChangedEventArgs(status));
        }
    }
}

