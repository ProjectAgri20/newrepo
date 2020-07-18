using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.SolutionApps.PaperCut;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.PaperCut
{
    /// <summary>
    /// PaerCut Pull Printing Manager Class.
    /// </summary>
    public class PaperCutPullPrintManager : PullPrintManager
    {

        private static List<PaperCutPullPrintAction> _validationTargets = new List<PaperCutPullPrintAction>()
            { PaperCutPullPrintAction.Print };

        private PaperCutActivityData _activityData = null;
        private IPaperCutApp _paperCutApp = null;
        private Dictionary<string, string> _documentIds = new Dictionary<string, string>();
        


        /// <summary>
        /// Initializes a new instance of the <see cref="PaperCutPullPrintManager"/> class.
        /// </summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        /// <param name="activityData">The activity data.</param>
        public PaperCutPullPrintManager(PluginExecutionData pluginExecutionData, PaperCutActivityData activityData) : base(pluginExecutionData)
        {
            _activityData = activityData;
            PullPrintSolution = "PaperCut Print Release";
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

            OnStatusUpdate("Logging into device and launching pull print solution...");
            try
            {
                _paperCutApp = PaperCutAppFactory.Create(Device);
                _paperCutApp.WorkflowLogger = WorkflowLogger;

                RecordEvent(DeviceWorkflowMarker.ActivityBegin);
                if (!_activityData.ReleaseOnSignIn)
                {
                    Launch();
                    ExecutePullPrintAction();

                    // List out the documents that we pulled/deleted
                    if (_activityData.DocumentProcessAction == PaperCutPullPrintAction.Delete)
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
                    result = ExecuteSignInRelease();
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
                SignOut(_paperCutApp);

                SubmitLog(result.Result.ToString());
                CollectMemoryData(_activityData.DeviceMemoryProfilerConfig, "PaperCutPullPrint");
                RecordEvent(DeviceWorkflowMarker.ActivityEnd);

                //Release any connections to the device.  This is critical for Omni operations.
                Dispose();
            }
            return result;
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
                        result.Add(_documentIds.Keys.First());
                    }
                    return result;
                }
                return _documentIds.Keys.ToList();
            }
        }

        /// <summary>
        /// Defines the flow and executes the pull print action.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void ExecutePullPrintAction()
        {
            SetInitialJobCount();
            VerifyJobsFound();

            PullPrintLog = SetPullPrintRetrievalLog("PaperCut");

            //Execute the action            
            OnStatusUpdate(_activityData.DocumentProcessAction.ToString());
            switch (_activityData.DocumentProcessAction)
            {
                case PaperCutPullPrintAction.Print:
                    Print();
                    break;
                case PaperCutPullPrintAction.Delete:
                    Delete();
                    break;
            }

            if (!_paperCutApp.BannerErrorState())
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
        public PluginExecutionResult ExecuteSignInRelease()
        {
            bool result = _paperCutApp.SignInReleaseAll(Authenticator);
            if (result)
            {
                VerifyDeviceFinishedPrinting();
            }
            SignOut(_paperCutApp);
            if (result)
            {
                return new PluginExecutionResult(PluginResult.Passed);
            }
            else
            {
                return new PluginExecutionResult(PluginResult.Failed, "Documents were not released.", "Device workflow error.");
            }
        }

        
        /// <summary>
        /// Launch the PaperCut solution.
        /// </summary>
        private void Launch()
        {
            AuthenticationMode am = (_activityData.PaperCutAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;

            UpdateLaunchStatus(am, _activityData.AuthProvider, "Print Release");
            _paperCutApp.Launch(Authenticator, am);
        }

        /// <summary>
        /// Sets the initial job count.
        /// </summary>
        private void SetInitialJobCount()
        {
            _documentIds.Clear();
            InitialJobCount = 0;

            DateTime startTime = DateTime.Now;
            _documentIds = _paperCutApp.GetDocumentIds();
            InitialJobCount = _documentIds.Count;

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
                FinalJobCount = _paperCutApp.GetDocumentCount();
                return InitialJobCount > FinalJobCount;
            }
            , TimeSpan.FromSeconds(10));

            OnTimeStatusUpdate("SetFinalJobCount", startTime, DateTime.Now);
            OnStatusUpdate($"Available jobs (Final)={FinalJobCount}");

        }

        /// <summary>
        /// Prints the selected jobs, deleting them afterward.
        /// </summary>
        private void Print()
        {
            SelectPrintJobs();

            RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
            _paperCutApp.Print();
            RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
        }

        /// <summary>
        /// Deletes the selected jobs (without printing them).
        /// </summary>
        private void Delete()
        {
            SelectPrintJobs();

            RecordEvent(DeviceWorkflowMarker.DeleteBegin);
            _paperCutApp.Delete();
            RecordEvent(DeviceWorkflowMarker.DeleteEnd);
        }

        private void SelectPrintJobs()
        {
            // PaperCut defaults to all documents selected.  So if selectAll = true
            // there is nothing to do.
            if (!_activityData.SelectAll)
            {
                RecordEvent(DeviceWorkflowMarker.SelectDocumentBegin);
                SelectSingleJob();
                RecordEvent(DeviceWorkflowMarker.SelectDocumentEnd);
            }
        }

        private void SelectSingleJob()
        {
            _paperCutApp.SelectFirstDocument(new Collection<string>(_documentIds.Values.ToList()));
        }

        private void VerifyOperation()
        {
            if (_paperCutApp.ExistPrintPopupMessage())
            {
                _paperCutApp.DismissPostOperationPopup();
            }
            switch (_activityData.DocumentProcessAction)
            {
                case PaperCutPullPrintAction.Delete:
                    break;
                case PaperCutPullPrintAction.Print:
                    VerifyStartPrinting();
                    VerifyDeviceFinishedPrinting();
                    break;
            }

            SetFinalJobCount();
        }

        /// <summary>
        /// Verifies that the device has started printing the job.
        /// </summary>
        private void VerifyStartPrinting()
        {
            DateTime startTime = DateTime.Now;

            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerBegin);
            _paperCutApp.StartedProcessingWork(TimeSpan.FromSeconds(7));
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
            _paperCutApp.FinishedProcessingWork();
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            OnTimeStatusUpdate("VerifyDeviceFinishedPrinting", startTime, DateTime.Now);
        }

        

    }
}
