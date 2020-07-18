using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.SolutionApps.Hpac;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.HpacPullPrinting
{
    /// <summary>
    /// HpacPullPrintManager class.
    /// </summary>
    public class HpacPullPrintManager : PullPrintManager
    {
        private HpacActivityData _activityData = null;
        private IHpacApp _hpacApp = null;
        private List<string> AffectedDocIds { get; set; } = new List<string>();

        private static List<HpacPullPrintAction> _validateTargets = new List<HpacPullPrintAction>()
            { HpacPullPrintAction.PrintAll, HpacPullPrintAction.PrintDelete, HpacPullPrintAction.PrintKeep };

        /// <summary>
        /// Initializes a new instance of the <see cref="HpacPullPrintManager"/> class.
        /// </summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        /// <param name="activityData">The activity data.</param>
        public HpacPullPrintManager(PluginExecutionData pluginExecutionData, HpacActivityData activityData) : base(pluginExecutionData)
        {
            _activityData = activityData;
            PullPrintSolution = "HP AC Pull Printing";
            this.LockTimeouts = _activityData.LockTimeouts;
            this.AuthProvider = _activityData.AuthProvider;
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
                _hpacApp = HpacAppFactory.Create(Device);
                RecordEvent(DeviceWorkflowMarker.ActivityBegin);

                if (!_activityData.ReleaseOnSignIn)
                {
                    _hpacApp.WorkflowLogger = WorkflowLogger;
                    Launch();
                    ExecutePullPrintAction();

                    // List out the documents that we pulled/deleted
                    if (_activityData.DocumentProcessAction == HpacPullPrintAction.Delete)
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
                SignOut(_hpacApp);

                SubmitLog(result.Result.ToString());
                CollectMemoryData(_activityData.DeviceMemoryProfilerConfig, "HpacPullPrint");
                RecordEvent(DeviceWorkflowMarker.ActivityEnd);

                //Release any connections to the device.  This is critical for Omni operations.
                Dispose();
            }
            return result;
        }

        private PluginExecutionResult SignInAndRelease()
        {
            OnStatusUpdate("Signing in to release documents.");
            bool success = ExecuteSignInReleaseAction();
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
        /// Executes the pull print action.
        /// Defines the flow of the pull print action.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void ExecutePullPrintAction()
        {
            SetInitialJobCount();
            VerifyJobsFound();

            PullPrintLog = SetPullPrintRetrievalLog("HPAC");

            //Execute the action            
            OnStatusUpdate(_activityData.DocumentProcessAction.ToString());
            switch (_activityData.DocumentProcessAction)
            {
                case HpacPullPrintAction.PrintAll:
                    // Print All Button not select all and print
                    PrintAll();
                    break;
                case HpacPullPrintAction.PrintDelete:
                    PrintDelete();
                    break;
                case HpacPullPrintAction.PrintKeep:
                    PrintKeep();
                    break;
                case HpacPullPrintAction.Delete:
                    Delete();
                    break;
                case HpacPullPrintAction.Refresh:
                    Refresh();
                    break;
            }
            if (!_hpacApp.BannerErrorState())
            {
                ProcessPullRequest();
            }
            else
            {
                throw new DeviceInvalidOperationException("Unknown Device Error encountered while trying to process pull print.");
            }

        }
        private void ProcessPullRequest()
        {

            switch (_activityData.DocumentProcessAction)
            {
                case HpacPullPrintAction.Delete:
                case HpacPullPrintAction.Refresh:
                    break;
                case HpacPullPrintAction.PrintAll:
                case HpacPullPrintAction.PrintDelete:
                case HpacPullPrintAction.PrintKeep:
                    VerifyStartPrintingJob();
                    VerifyDeviceFinishedPrinting();
                    break;
            }

            SetFinalJobCount();
        }

        public bool ExecuteSignInReleaseAction()
        {
            bool result = _hpacApp.SignInReleaseAll(Authenticator);
            if (result)
            {
                VerifyDeviceFinishedPrinting();
            }
            SignOut(_hpacApp);
            return result;
        }

        /// <summary>
        /// Launch the solution.
        /// </summary>
        private void Launch()
        {
            AuthenticationMode am = (_activityData.HpacAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;

            UpdateLaunchStatus(am, _activityData.AuthProvider, "HP AC Secure Pull Print");
            _hpacApp.Launch(Authenticator, am);
        }

        /// <summary>
        /// Sets the initial job count.
        /// </summary>
        private void SetInitialJobCount()
        {
            DateTime startTime = DateTime.Now;

            InitialJobCount = 0;
            Wait.ForTrue(() =>
            {
                InitialJobCount = _hpacApp.GetDocumentCount();
                return InitialJobCount != 0;
            }
                , TimeSpan.FromSeconds(10));


            if (InitialJobCount < 0)
            {
                throw new DeviceWorkflowException("Unknown solution error encountered.");
            }

            RecordEvent(DeviceWorkflowMarker.DocumentListReady);
            AffectedDocIds.Clear();

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
                FinalJobCount = _hpacApp.GetDocumentCount();

                OnTimeStatusUpdate("SetFinalJobCount", startTime, DateTime.Now);
                OnStatusUpdate($"Available jobs (Final)={FinalJobCount}");
            }
            catch (DeviceInvalidOperationException doe)
            {
                string msg = doe.Message;
                if (msg.Contains("The UIContextId is null and there is no active browser") || msg.Contains("a non-derived FaultException has been thrown by the target operation"))
                {
                    throw new DeviceInvalidOperationException("HpacPullPrintManager.SetFinalJobCount: On home screen, unable to determine the HPAC final document count.", doe);
                }
                else
                {
                    throw;
                }
            }
        }
        
        /// <summary>
        /// Verifies the start printing job.
        /// </summary>
        private void VerifyStartPrintingJob()
        {
            DateTime startTime = DateTime.Now;

            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerBegin);
            _hpacApp.StartedProcessingWork(TimeSpan.FromSeconds(10));
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
            _hpacApp.FinishedProcessingWork();
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            OnTimeStatusUpdate("VerifyDeviceFinishedPrinting", startTime, DateTime.Now);
        }

        private void SelectPrintJobs()
        {
            RecordEvent(DeviceWorkflowMarker.SelectDocumentBegin);
            if (_activityData.SelectAll)
            {
                SelectAllJobs();
            }
            else
            {
                SelectSingleJob();
            }
            RecordEvent(DeviceWorkflowMarker.SelectDocumentEnd);
        }

        private void SelectAllJobs()
        {
            AffectedDocIds = _hpacApp.GetDocumentIds().ToList();
            if (_hpacApp.IsNewVersion())
            {
                _hpacApp.SelectAllDocuments(AffectedDocIds);
            }
            else
            {
                HpacInput checkbox = _hpacApp.GetCheckboxOnchangeValue();
                _hpacApp.SelectAllDocuments(checkbox.OnchangeValue);
            }
        }

        private void SelectSingleJob()
        {
            string firstDocId = _hpacApp.GetFirstDocumentId();
            AffectedDocIds.Add(firstDocId);

            if (_hpacApp.IsNewVersion())
            {
                _hpacApp.SelectFirstDocument(firstDocId);
            }
            else
            {
                HpacInput checkbox = _hpacApp.GetCheckboxOnchangeValue();
                if (string.IsNullOrEmpty(checkbox.CheckboxId))
                {
                    checkbox.CheckboxId = firstDocId;
                }

                _hpacApp.SelectFirstDocument(checkbox.CheckboxId, checkbox.OnchangeValue);
            }
        }

        /// <summary>
        /// Prints all jobs.
        /// </summary>
        private void PrintAll()
        {
            RecordEvent(DeviceWorkflowMarker.PrintAllBegin);
            _hpacApp.PrintAll();
            RecordEvent(DeviceWorkflowMarker.PrintAllEnd);
        }

        /// <summary>
        /// Prints the selected jobs, deleting them afterward.
        /// </summary>
        private void PrintDelete()
        {
            SelectPrintJobs();

            RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
            _hpacApp.PrintDelete();
            RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
        }

        /// <summary>
        /// Prints the selected jobs, keeping them on the server.
        /// </summary>
        private void PrintKeep()
        {
            SelectPrintJobs();

            RecordEvent(DeviceWorkflowMarker.PrintKeepBegin);
            _hpacApp.PrintKeep();
            RecordEvent(DeviceWorkflowMarker.PrintKeepEnd);
        }

        /// <summary>
        /// Deletes the selected jobs.
        /// </summary>
        private void Delete()
        {
            SelectPrintJobs();

            RecordEvent(DeviceWorkflowMarker.DeleteBegin);
            _hpacApp.Delete();
            RecordEvent(DeviceWorkflowMarker.DeleteEnd);
        }

        /// <summary>
        /// Refreshes the control panel.
        /// </summary>
        private void Refresh()
        {
            RecordEvent(DeviceWorkflowMarker.RefreshBegin);
            _hpacApp.Refresh();
            RecordEvent(DeviceWorkflowMarker.RefreshEnd);
        }

    }
}
