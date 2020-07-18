using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.SolutionApps.Equitrac;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.EquitracPullPrinting
{
    /// <summary>
    /// Equitrac pullprint manager
    /// </summary>
    /// <seealso cref="HP.ScalableTest.PluginSupport.PullPrint.PullPrintManager" />
    public class EquitracPullPrintManager : PullPrintManager
    {
        private EquitracActivityData _activityData = null;
        private IEquitracApp _equitracApp = null;
        //private PullPrintJobRetrievalLog _pullPrintLog = null;

        private static List<EquitracPullPrintAction> _validateTargets = new List<EquitracPullPrintAction>()
            { EquitracPullPrintAction.Print, EquitracPullPrintAction.PrintSave };

        /// <summary>
        /// Initializes a new instance of the <see cref="EquitracPullPrintManager"/> class.
        /// </summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        /// <param name="activityData">The activity data.</param>
        public EquitracPullPrintManager(PluginExecutionData pluginExecutionData, EquitracActivityData activityData) : base(pluginExecutionData)
        {
            _activityData = activityData;
            PullPrintSolution = "Follow-You Printing";
            this.LockTimeouts = _activityData.LockTimeouts;
            this.AuthProvider = _activityData.AuthProvider;
        }

        /// <summary>
        /// Gets the affected document ids.
        /// </summary>
        /// <value>
        /// The affected document ids.
        /// </value>
        public List<string> AffectedDocIds { get; private set; } = new List<string>();

        /// <summary>
        /// Launches the solution and pulls documents.
        /// </summary>
        /// <param name="deviceInfo">The device information.</param>
        protected override PluginExecutionResult LaunchAndPull()
        {
            OnDocumentProcessSelected(_activityData.DocumentProcessAction.GetDescription());
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);

            OnStatusUpdate("Logging into device and launching pull print app...");
            try
            {
                _equitracApp = EquitracAppFactory.Create(Device);
                _equitracApp.WorkflowLogger = WorkflowLogger;

                RecordEvent(DeviceWorkflowMarker.ActivityBegin);
                Launch();
                ExecutePullPrintAction();

                // List out the documents that we pulled/deleted
                if (_activityData.DocumentProcessAction == EquitracPullPrintAction.Delete)
                {
                    AffectedDocIds.ForEach(x => OnStatusUpdate("Deleted document with id = " + x));
                }
                else
                {
                    AffectedDocIds.ForEach(x => OnStatusUpdate("Pulled document with id = " + x));
                }

                if (_validateTargets.Contains(_activityData.DocumentProcessAction))
                {
                    ValidatePull(AffectedDocIds, DeviceInfo);
                }

                ExecutionServices.SystemTrace.LogDebug($"Available jobs (Initial)={InitialJobCount}");
                ExecutionServices.SystemTrace.LogDebug($"Available jobs (Final)={FinalJobCount}");
            }
            catch (DeviceCommunicationException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex.Message, "Device communication error.");
                GatherTriageData(ex.ToString());
            }
            catch (DeviceInvalidOperationException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex.Message, "Device automation error.");
                GatherTriageData(ex.ToString());
            }
            catch (DeviceWorkflowException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Device workflow error.");
                GatherTriageData(ex.ToString());
            }
            catch (NoJobsFoundException ex)
            {
                result = new PluginExecutionResult(PluginResult.Skipped, ex.Message, "Pull print queue empty.");
            }
            catch (Exception ex)
            {
                GatherTriageData(ex.ToString());
                throw;
            }
            finally
            {
                SignOut(_equitracApp);
                SubmitLog(result.Result.ToString());
                CollectMemoryData(_activityData.DeviceMemoryProfilerConfig, "EquitracPullPrint");
                RecordEvent(DeviceWorkflowMarker.ActivityEnd);

                //Release any connections to the device.  This is critical for Omni operations.
                Dispose();
            }
            return result;
        }

        /// <summary>
        /// Executes the pull print action.
        /// Defines the flow of the pull print action.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void ExecutePullPrintAction()
        {
            SetInitialJobCount();
            VerifyJobsFound();

            PullPrintLog = SetPullPrintRetrievalLog("Equitrac");

            //Execute the action
            DateTime startTime = ((DateTimeOffset)PullPrintLog.JobStartDateTime).LocalDateTime;
            switch (_activityData.DocumentProcessAction)
            {
                case EquitracPullPrintAction.Print:
                    Print();
                    break;
                case EquitracPullPrintAction.PrintSave:
                    PrintSave();
                    break;
                case EquitracPullPrintAction.Delete:
                    Delete();
                    break;
                case EquitracPullPrintAction.Refresh:
                    Refresh();
                    break;
            }
            OnTimeStatusUpdate(_activityData.DocumentProcessAction.ToString(), startTime, DateTime.Now);
            switch (_activityData.DocumentProcessAction)
            {
                case EquitracPullPrintAction.Delete:
                case EquitracPullPrintAction.Refresh:
                case EquitracPullPrintAction.SelectAll:
                    break;
                case EquitracPullPrintAction.PrintSave:
                case EquitracPullPrintAction.Print:
                    VerifyStartPrintingJob();
                    VerifyDeviceFinishedPrinting();
                    break;

            }

            SetFinalJobCount();
            OnStatusUpdate("PullPrintJobLog Submit");
        }

        /// <summary>
        /// Launch the solution.
        /// </summary>
        private void Launch()
        {
            DateTime startTime = DateTime.Now;
            AuthenticationMode am = (_activityData.EquitracAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;

            UpdateLaunchStatus(am, _activityData.AuthProvider, "Equitrac \"Follow-You Printing\"");
            _equitracApp.Launch(Authenticator, am);

            OnTimeStatusUpdate("Launch", startTime, DateTime.Now);
        }

        /// <summary>
        /// Sets the status of the pull print actions and calls to submit the  data to PullPrintJobRetrieval table
        /// </summary>
        /// <param name="status">The status.</param>
        protected override void SubmitLog(string status)
        {
            if (PullPrintLog != null)
            {
                PullPrintLog.NumberOfCopies = (short)_activityData.NumberOfCopies;
                base.SubmitLog(status);
            }
        }

        /// <summary>
        /// Sets the initial job count.
        /// </summary>
        private void SetInitialJobCount()
        {
            DateTime startTime = DateTime.Now;

            InitialJobCount = 0;
            Wait.ForTrue(() => HasJobCount(), TimeSpan.FromSeconds(30));

            if (InitialJobCount < 0)
            {
                throw new DeviceWorkflowException("Unknown solution error encountered.");
            }

            RecordEvent(DeviceWorkflowMarker.DocumentListReady);
            AffectedDocIds.Clear();

            OnTimeStatusUpdate("SetInitialJobCount", startTime, DateTime.Now);
            OnStatusUpdate($"Available jobs (Initial)={InitialJobCount}");
        }

        /// <summary>
        /// Sets the final job count.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override void SetFinalJobCount()
        {
            DateTime startTime = DateTime.Now;
            try
            {
                Retry.WhileThrowing<DeviceCommunicationException>(() => FinalJobCount = _equitracApp.GetDocumentCount(), 6, TimeSpan.FromSeconds(3));

                OnTimeStatusUpdate("SetFinalJobCount", startTime, DateTime.Now);
                OnStatusUpdate($"Available jobs (Final)={FinalJobCount}");
            }
            catch (DeviceInvalidOperationException doe)
            {
                if (doe.Message.Contains("a non-derived FaultException has been thrown by the target operation"))
                {
                    throw new DeviceInvalidOperationException("EquitracPullPrintManager.SetFinalJobCount: On home screen, unable to determine the Equitrac final document count.", doe);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Verifies whether the device has finished printing by checking the job status and
        /// waiting until clear before releasing the control panel to other operations.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override void VerifyDeviceFinishedPrinting()
        {
            DateTime startTime = DateTime.Now;

            RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
            _equitracApp.FinishedProcessingWork();
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            OnTimeStatusUpdate("VerifyDeviceFinishedPrinting", startTime, DateTime.Now);
        }

        

        /// <summary>
        /// Determines whether Equitrac has any jobs for the current user.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [has job count]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasJobCount()
        {
            InitialJobCount = _equitracApp.GetDocumentCount();
            return InitialJobCount != 0;
        }

        /// <summary>
        /// Verifies the start printing job.
        /// </summary>
        private void VerifyStartPrintingJob()
        {
            DateTime startTime = DateTime.Now;

            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerBegin);
            _equitracApp.StartedProcessingWork(TimeSpan.FromSeconds(20));
            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerEnd);

            OnTimeStatusUpdate("VerifyStartPrintingJob", startTime, DateTime.Now);
        }

        /// <summary>
        /// Selects one or all documents.
        /// </summary>
        private void SelectDocuments()
        {
            RecordEvent(DeviceWorkflowMarker.SelectDocumentBegin);
            if (_activityData.SelectAll)
            {
                AffectedDocIds = _equitracApp.GetDocumentIds().ToList();
                _equitracApp.SelectAll();
            }
            else
            {
                AffectedDocIds.Add(_equitracApp.GetFirstDocumentId());
                _equitracApp.SelectFirstDocument();
            }
            RecordEvent(DeviceWorkflowMarker.SelectDocumentEnd);
        }

        /// <summary>
        /// Prints and deletes the selected documents.
        /// </summary>
        private void Print()
        {
            try
            {
                SelectDocuments();

                if (_activityData.NumberOfCopies > 1)
                {
                    _equitracApp.SetCopyCount(_activityData.NumberOfCopies);
                }

                RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
                _equitracApp.Print();
                RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
            }
            catch (JavaScriptExecutionException ex)
            {
                ExecutionServices.SystemTrace.LogError($"Error executing JavaScript: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Prints and keeps the selected documents from the list.
        /// </summary>
        private void PrintSave()
        {
            try
            {
                SelectDocuments();

                if (_activityData.NumberOfCopies > 1)
                {
                    _equitracApp.SetCopyCount(_activityData.NumberOfCopies);
                }

                RecordEvent(DeviceWorkflowMarker.PrintKeepBegin);
                _equitracApp.PrintSave();
                RecordEvent(DeviceWorkflowMarker.PrintKeepEnd);
            }
            catch (JavaScriptExecutionException ex)
            {
                ExecutionServices.SystemTrace.LogError($"Error executing JavaScript: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deletes single or all jobs.
        /// </summary>
        private void Delete()
        {
            try
            {
                SelectDocuments();
                RecordEvent(DeviceWorkflowMarker.DeleteBegin);
                _equitracApp.Delete();
                RecordEvent(DeviceWorkflowMarker.DeleteEnd);
            }
            catch (JavaScriptExecutionException ex)
            {
                ExecutionServices.SystemTrace.LogError($"Error executing JavaScript: {ex.Message}");
                throw;
            }
        }

        private void Refresh()
        {
            try
            {
                RecordEvent(DeviceWorkflowMarker.RefreshBegin);
                _equitracApp.Refresh();
                RecordEvent(DeviceWorkflowMarker.RefreshEnd);
            }
            catch (JavaScriptExecutionException ex)
            {
                ExecutionServices.SystemTrace.LogError($"Error executing JavaScript: {ex.Message}");
                throw;
            }
        }


    }
}
