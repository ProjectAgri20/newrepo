using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.SolutionApps.SafeQ;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.SafeQPullPrinting
{
    /// <summary>
    /// SafeQ Pull print manager class
    /// </summary>
    /// <seealso cref="HP.ScalableTest.PluginSupport.PullPrint.PullPrintManager" />
    public class SafeQPullPrintManager : PullPrintManager
    {
        private SafeQPullPrintingActivityData _activityData = null;
        private ISafeQApp _safeQApp = null;
        private List<string> _documentIds = new List<string>();
        private static List<SafeQPrintPullPrintAction> _validationTargets = new List<SafeQPrintPullPrintAction>() { SafeQPrintPullPrintAction.Print };

        public SafeQPullPrintManager(PluginExecutionData pluginExecutionData, SafeQPullPrintingActivityData activityData) : base(pluginExecutionData)
        {
            _activityData = activityData;
            PullPrintSolution = "SafeQ Printing";
            this.LockTimeouts = _activityData.LockTimeouts;
            this.AuthProvider = _activityData.AuthProvider;
        }
        /// <summary>
        /// Returns documents affected by pull operation.
        /// </summary>
        public List<string> AffectedDocIds
        {
            get
            {
                if (!_activityData.SelectAll)
                {
                    List<string> result = new List<string>();
                    if (InitialJobCount > 0)
                    {
                        result.Add(_documentIds.First());
                    }
                    return result;
                }
                return _documentIds;
            }
        }

        /// <summary>
        /// Launches the Pull print solution and pulls the desired number of documents.
        /// </summary>
        /// <returns></returns>
        protected override PluginExecutionResult LaunchAndPull()
        {
            OnDocumentProcessSelected(_activityData.DocumentProcessAction.GetDescription());
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);
            try
            {
                _safeQApp = SafeQAppFactory.Create(Device);
                _safeQApp.WorkflowLogger = WorkflowLogger;

                RecordEvent(DeviceWorkflowMarker.ActivityBegin);

                if (!_activityData.ReleaseOnSignIn)
                {
                    Launch();
                    ExecutePullPrintAction();

                    // List out the documents that we pulled/deleted
                    if (_activityData.DocumentProcessAction == SafeQPrintPullPrintAction.Delete)
                    {
                        AffectedDocIds.ForEach(x => OnStatusUpdate("Deleted document with id = " + x));
                    }
                    else
                    {
                        AffectedDocIds.ForEach(x => OnStatusUpdate("Pulled document with id = " + x));
                    }

                    if (_validationTargets.Contains(_activityData.DocumentProcessAction))
                    {
                        ValidatePull(AffectedDocIds, DeviceInfo);
                    }

                    ExecutionServices.SystemTrace.LogDebug($"Available jobs (Initial)={InitialJobCount}");
                    ExecutionServices.SystemTrace.LogDebug($"Available jobs (Final)={FinalJobCount}");

                }
                else
                {
                    result = SignInAndRelease();
                }
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
            catch (NoJobsFoundException)
            {
                result = new PluginExecutionResult(PluginResult.Skipped, "No jobs found to pull", "Pull print queue empty.");
            }
            catch (Exception ex)
            {
                GatherTriageData(ex.ToString());
                throw;
            }
            finally
            {
                SignOut(_safeQApp);

                SubmitLog(result.Result.ToString());
                CollectMemoryData(_activityData.DeviceMemoryProfilerConfig, "SafeQPullPrint");
                RecordEvent(DeviceWorkflowMarker.ActivityEnd);

                //Release any connections to the device.  This is critical for Omni operations.
                Dispose();
            }
            return result;
        }
        /// <summary>
        /// Defines the flow and executes the pull print action.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void ExecutePullPrintAction()
        {
            if (_activityData.DocumentProcessAction != SafeQPrintPullPrintAction.PrintAll)
            {
                SetInitialJobCount();
                VerifyJobsFound();
            }

            PullPrintLog = SetPullPrintRetrievalLog("SafeQ");

            //Execute the action            
            OnStatusUpdate(_activityData.DocumentProcessAction.ToString());
            switch (_activityData.DocumentProcessAction)
            {
                case SafeQPrintPullPrintAction.Print:
                    Print();
                    break;
                case SafeQPrintPullPrintAction.PrintAll:
                    PrintAll();
                    break;
                case SafeQPrintPullPrintAction.Delete:
                    Delete();
                    break;
            }

            if (!_safeQApp.BannerErrorState())
            {
                VerifyOperation();
            }
            else
            {
                throw new DeviceInvalidOperationException($"Unknown Device Error encountered while trying to verify {_activityData.DocumentProcessAction} operation.");
            }

        }

        /// <summary>
        /// Signs into the device and immediately releases all documents.
        /// </summary>
        /// <returns></returns>
        public bool ExecuteSignInRelease()
        {
            bool result = _safeQApp.SignInReleaseAll(Authenticator);
            if (result)
            {
                VerifyDeviceFinishedPrinting();
            }
            SignOut(_safeQApp);
            return result;
        }

  
        /// <summary>
        /// Launch the SafeQ solution.
        /// </summary>
        private void Launch()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("PrintAll", _activityData.DocumentProcessAction == SafeQPrintPullPrintAction.PrintAll);
            AuthenticationMode am = (_activityData.SafeQPrintAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;

            UpdateLaunchStatus(am, _activityData.AuthProvider, "My print jobs");
            _safeQApp.Launch(Authenticator, am, parameters);
        }

        /// <summary>
        /// Sets the initial job count.
        /// </summary>
        private void SetInitialJobCount()
        {
            _documentIds.Clear();
            InitialJobCount = 0;

            DateTime startTime = DateTime.Now;
            _documentIds = _safeQApp.GetDocumentIds();
            InitialJobCount = _safeQApp.GetDocumentCount();
            RecordEvent(DeviceWorkflowMarker.DocumentListReady);
            OnStatusUpdate($"Available jobs (Initial)={InitialJobCount}");
        }

        /// <summary>
        /// Sets the final job count.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override void SetFinalJobCount()
        {
            DateTime startTime = DateTime.Now;

            // The delete operation happens faster than the UI has time to detect the change.
            Wait.ForTrue(() =>
            {
                FinalJobCount = _safeQApp.GetDocumentCount();
                return InitialJobCount > FinalJobCount;
            }
            , TimeSpan.FromSeconds(10));

            OnTimeStatusUpdate("SetFinalJobCount", startTime, DateTime.Now);
            OnStatusUpdate($"Available jobs (Final)={FinalJobCount}");
        }

        /// <summary>
        /// Signing in to release documents
        /// </summary>
        /// <returns></returns>
        private PluginExecutionResult SignInAndRelease()
        {
            OnStatusUpdate("Signing in to release documents.");
            bool success = ExecuteSignInRelease();
            if (success)
            {
                return new PluginExecutionResult(PluginResult.Passed);
            }
            else
            {
                return new PluginExecutionResult(PluginResult.Failed, "Documents were not released.", "Device workflow error.");
            }
        }

        /// <summary>
        /// Prints the selected jobs, deleting them afterward.
        /// </summary>
        private void Print()
        {
            SelectPrintJobs();
            if (!_activityData.SelectAll)
            {
                SelectPrintOptions();
            }
            RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
            _safeQApp.Print();
            RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
        }

        /// <summary>
        /// Prints the selected jobs, deleting them afterward.
        /// </summary>
        private void PrintAll()
        {
            RecordEvent(DeviceWorkflowMarker.PrintAllBegin);
            _safeQApp.PrintAll();
            RecordEvent(DeviceWorkflowMarker.PrintAllEnd);
        }

        /// <summary>
        /// Deletes the selected jobs (without printing them).
        /// </summary>
        private void Delete()
        {
            SelectPrintJobs();

            RecordEvent(DeviceWorkflowMarker.DeleteBegin);
            _safeQApp.Delete();
            RecordEvent(DeviceWorkflowMarker.DeleteEnd);
        }

        private void SelectPrintJobs()
        {
            if (InitialJobCount > 0)
            {
                if (!_activityData.SelectAll)
                {
                    //selectall
                    RecordEvent(DeviceWorkflowMarker.SelectDocumentBegin);
                    SelectSingleJob();
                    RecordEvent(DeviceWorkflowMarker.SelectDocumentEnd);
                }
                else
                {
                    RecordEvent(DeviceWorkflowMarker.SelectDocumentBegin);
                    SelectAllJob();
                    RecordEvent(DeviceWorkflowMarker.SelectDocumentEnd);
                }
            }
        }

        /// <summary>
        /// Select basic setting options
        /// </summary>
        private void SelectPrintOptions()
        {
            _safeQApp.SelectOption();

            _safeQApp.SetColorMode(_activityData.ColorMode);
            if (_activityData.NumberOfCopies > 1)
            {
                _safeQApp.SetCopyCount(_activityData.NumberOfCopies);
            }
            _safeQApp.SetSides(_activityData.Sides);
            _safeQApp.SaveOption();
        }

        /// <summary>
        /// Select first job
        /// </summary>
        private void SelectSingleJob()
        {
            _safeQApp.SelectFirstDocument(_documentIds.First());
        }

        /// <summary>
        /// Select all job
        /// </summary>
        private void SelectAllJob()
        {
            _safeQApp.SelectAll();
        }

        /// <summary>
        /// Verify operation
        /// </summary>
        private void VerifyOperation()
        {
            switch (_activityData.DocumentProcessAction)
            {
                case SafeQPrintPullPrintAction.Delete:
                    break;
                case SafeQPrintPullPrintAction.Print:
                    _safeQApp.DismissPostPrintOperation();
                    VerifyStartPrinting();
                    VerifyDeviceFinishedPrinting();
                    break;
                case SafeQPrintPullPrintAction.PrintAll:
                    break;
            }
            if (_activityData.DocumentProcessAction != SafeQPrintPullPrintAction.PrintAll)
            {
                SetFinalJobCount();
            }
        }

        /// <summary>
        /// Verifies that the device has started printing the job.
        /// </summary>
        private void VerifyStartPrinting()
        {
            OnStatusUpdate("VerifyStartPrinting()");
            DateTime startTime = DateTime.Now;

            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerBegin);
            _safeQApp.StartedProcessingWork(TimeSpan.FromSeconds(7));
            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerEnd);

            OnTimeStatusUpdate("VerifyStartPrintingJob", startTime, DateTime.Now);
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
            _safeQApp.FinishedProcessingWork();
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            OnTimeStatusUpdate("VerifyDeviceFinishedPrinting", startTime, DateTime.Now);
        }

        

    }
}
