using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.SolutionApps.PaperCut;
using HP.ScalableTest.DeviceAutomation.SolutionApps.PaperCutAgentless;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.PaperCutAgentless
{
    /// <summary>
    /// PaerCutAgentless Pull Printing Manager Class.
    /// </summary>
    public class PaperCutAgentlessPullPrintManager : PullPrintManager
    {
        private static List<PaperCutAgentlessPullPrintAction> _validationTargets = new List<PaperCutAgentlessPullPrintAction>() { PaperCutAgentlessPullPrintAction.Print };

        private PaperCutAgentlessActivityData _activityData = null;
        private IPaperCutAgentlessApp _paperCutAgentlessApp = null;
        private Dictionary<string, string> _documentIds = new Dictionary<string, string>();
        

        /// <summary>
        /// Initializes a new instance of the <see cref="PaperCutAgentlessPullPrintManager"/> class.
        /// </summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        /// <param name="activityData">The activity data.</param>
        public PaperCutAgentlessPullPrintManager(PluginExecutionData pluginExecutionData, PaperCutAgentlessActivityData activityData) : base(pluginExecutionData)
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
                _paperCutAgentlessApp = PaperCutAgentlessAppFactory.Create(Device);
                _paperCutAgentlessApp.WorkflowLogger = WorkflowLogger;

                RecordEvent(DeviceWorkflowMarker.ActivityBegin);
                if (!_activityData.ReleaseOnSignIn)
                {
                    Launch();
                    ExecutePullPrintAction();

                    // List out the documents that we pulled/deleted
                    if (_activityData.DocumentProcessAction == PaperCutAgentlessPullPrintAction.Delete)
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
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Device workflow error");
                GatherTriageData(ex.ToString());
            }
            catch (NoJobsFoundException ex)
            {
                result = new PluginExecutionResult(PluginResult.Skipped, ex.Message, "Pull print queue empty");
            }
            catch (Exception ex)
            {
                GatherTriageData(ex.ToString());
                throw;
            }
            finally
            {
                SignOut(_paperCutAgentlessApp);

                SubmitLog(result.Result.ToString());
                CollectMemoryData(_activityData.DeviceMemoryProfilerConfig, "PaperCutAgentlessPullPrint");
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

            PullPrintLog = SetPullPrintRetrievalLog("PaperCutAgentless");

            //Execute the action            
            OnStatusUpdate(_activityData.DocumentProcessAction.ToString());
            switch (_activityData.DocumentProcessAction)
            {
                case PaperCutAgentlessPullPrintAction.Print:
                    Print();
                    break;
                case PaperCutAgentlessPullPrintAction.Delete:
                    Delete();
                    break;
            }

            if (!_paperCutAgentlessApp.BannerErrorState())
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
            bool result = _paperCutAgentlessApp.SignInReleaseAll(Authenticator);
            if (result)
            {
                VerifyDeviceFinishedPrinting();
            }
            SignOut(_paperCutAgentlessApp);

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
        /// Launch the PaperCut (Agentless) solution.
        /// </summary>
        private void Launch()
        {
            // PaperCutAgentless always use "Eager" mode, because it cannot display app list before sign in.
            AuthenticationMode am = AuthenticationMode.Eager;

            UpdateLaunchStatus(am, _activityData.AuthProvider, "Print Release");
            _paperCutAgentlessApp.Launch(Authenticator, am);
        }

        /// <summary>
        /// Sets the initial job count.
        /// </summary>
        private void SetInitialJobCount()
        {
            _documentIds.Clear();
            InitialJobCount = 0;

            DateTime startTime = DateTime.Now;
            _documentIds = _paperCutAgentlessApp.GetDocumentIds();
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
                FinalJobCount = _paperCutAgentlessApp.GetDocumentCount();
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
            if (_activityData.UseSingleJobOptions)
            {
                SetSingleJobOptions();
            }
            else
            {
                SelectPrintJobs();
                SetPrintListOptions();
            }

            RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
            _paperCutAgentlessApp.Print();
            RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
        }

        /// <summary>
        /// Deletes the selected jobs (without printing them).
        /// </summary>
        private void Delete()
        {
            SelectPrintJobs();

            RecordEvent(DeviceWorkflowMarker.DeleteBegin);
            _paperCutAgentlessApp.Delete();
            RecordEvent(DeviceWorkflowMarker.DeleteEnd);
        }

        private void SelectPrintJobs()
        {
            if (!_activityData.SelectAll)
            {
                RecordEvent(DeviceWorkflowMarker.SelectDocumentBegin);
                SelectSingleJob();
                RecordEvent(DeviceWorkflowMarker.SelectDocumentEnd);
            }
            else
            {
                RecordEvent(DeviceWorkflowMarker.SelectDocumentBegin);
                SelectAllJobs();
                RecordEvent(DeviceWorkflowMarker.SelectDocumentEnd);
            }
        }

        private void SetSingleJobOptions()
        {
            _paperCutAgentlessApp.SetSingleJobOptions(_documentIds.Values.ToList()[0], _activityData.SingleJobCopies, _activityData.SingleJobDuplex, _activityData.SingleJobGrayScale);
        }

        private void SetPrintListOptions()
        {
            if (_activityData.ForceGrayscale)
            {
                _paperCutAgentlessApp.SetForcegrayscale();
            }
            if (_activityData.Force2sided)
            {
                _paperCutAgentlessApp.SetForce2sided();
            }
        }

        private void SelectSingleJob()
        {
            _paperCutAgentlessApp.SelectFirstDocument(new Collection<string>(_documentIds.Values.ToList()));
        }

        private void SelectAllJobs()
        {
            _paperCutAgentlessApp.SelectAllDocuments();
        }

        private void VerifyOperation()
        {
            switch (_activityData.DocumentProcessAction)
            {
                case PaperCutAgentlessPullPrintAction.Delete:
                    break;
                case PaperCutAgentlessPullPrintAction.Print:
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
            _paperCutAgentlessApp.StartedProcessingWork(TimeSpan.FromSeconds(7));
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
            _paperCutAgentlessApp.FinishedProcessingWork();
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            OnTimeStatusUpdate("VerifyDeviceFinishedPrinting", startTime, DateTime.Now);
        }

        
    }
}
