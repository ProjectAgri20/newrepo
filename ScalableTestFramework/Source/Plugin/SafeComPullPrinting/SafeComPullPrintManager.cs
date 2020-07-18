using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.SolutionApps.SafeCom;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.SafeComPullPrinting
{
    /// <summary>
    /// SafeComPullPrintManager class.
    /// </summary>
    public class SafeComPullPrintManager : PullPrintManager
    {
        private static List<SafeComPullPrintAction> _validateTargets = new List<SafeComPullPrintAction>()
            { SafeComPullPrintAction.PrintAll, SafeComPullPrintAction.Print, SafeComPullPrintAction.PrintRetain, SafeComPullPrintAction.PrintRetainAll };

        private SafeComActivityData _activityData = null;
        private ISafeComApp _safeComApp = null;
        private List<string> _expectedDocIds = new List<string>();
        private string _retainedDocumentName = string.Empty;

        public List<string> AffectedDocIds => _expectedDocIds;

        /// <summary>
        /// Initializes a new instance of the <see cref="EquitracPullPrintManager"/> class.
        /// </summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        /// <param name="activityData">The activity data.</param>
        public SafeComPullPrintManager(PluginExecutionData pluginExecutionData, SafeComActivityData activityData) : base(pluginExecutionData)
        {
            _activityData = activityData;
            PullPrintSolution = "Follow-You Printing";
            this.LockTimeouts = _activityData.LockTimeouts;
            this.AuthProvider = _activityData.AuthProvider;
        }

        protected override PluginExecutionResult LaunchAndPull()
        {
            OnDocumentProcessSelected(_activityData.DocumentProcessAction.GetDescription());
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);

            OnStatusUpdate("Logging into device and launching pull print app...");
            try
            {
                _safeComApp = SafeComAppFactory.Create(Device);
                _safeComApp.WorkflowLogger = WorkflowLogger;

                RecordEvent(DeviceWorkflowMarker.ActivityBegin);
                if (!_activityData.ReleaseOnSignIn)
                {
                    Launch();
                    ExecutePullPrintAction();

                    // List out the documents that we pulled/deleted
                    if (_activityData.DocumentProcessAction == SafeComPullPrintAction.Delete)
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
            catch (NoJobsFoundException ex)
            {
                result = new PluginExecutionResult(PluginResult.Skipped, ex.Message, "Pull print queue empty.");
            }
            catch (Exception ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Unknown Exception.");
                GatherTriageData(ex.ToString());
                throw;
            }
            finally
            {
                SignOut(_safeComApp);

                SubmitLog(result.Result.ToString());
                CollectMemoryData(_activityData.DeviceMemoryProfilerConfig, "SafeComPullPrint");
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

            PullPrintLog = SetPullPrintRetrievalLog("SafeCom");

            //Execute the action
            DateTime startTime = ((DateTimeOffset)PullPrintLog.JobStartDateTime).LocalDateTime;
            switch (_activityData.DocumentProcessAction)
            {
                case SafeComPullPrintAction.PrintAll:
                    // Print All Button not select all and print
                    PrintAllJobs();
                    break;
                case SafeComPullPrintAction.DeleteAll:
                    DeleteAllJobs();
                    break;
                case SafeComPullPrintAction.Delete:
                    DeleteSingleJob();
                    break;
                case SafeComPullPrintAction.PrintRetainAll:
                    //PrintRetainAllJobs(); -- dwa 05/26/2017 retain all not implemented at this time
                    _activityData.SelectAll = false;
                    PrintRetainSingleJob();
                    break;
                case SafeComPullPrintAction.PrintRetain:
                    PrintRetainSingleJob();
                    break;
                default:
                    if (_activityData.SelectAll)
                    {
                        SelectAllAndPrint();
                    }
                    else
                    {
                        PrintSingleJob();
                    }
                    break;
            }
            OnTimeStatusUpdate(_activityData.DocumentProcessAction.ToString(), startTime, DateTime.Now);

            switch (_activityData.DocumentProcessAction)
            {
                case SafeComPullPrintAction.Delete:
                case SafeComPullPrintAction.Refresh:
                case SafeComPullPrintAction.DeleteAll:
                    break;
                case SafeComPullPrintAction.PrintAll:
                case SafeComPullPrintAction.Print:
                    VerifyStartPrintingJob();
                    VerifyDeviceFinishedPrinting();
                    break;
                case SafeComPullPrintAction.PrintRetain:
                case SafeComPullPrintAction.PrintRetainAll:
                    VerifyRetainStatusChange();
                    break;

            }
            SetFinalJobCount();
        }

        private PluginExecutionResult SignInAndRelease()
        {
            OnStatusUpdate("Signing in to release documents.");

            PullPrintLog = SetPullPrintRetrievalLog("SafeCom");
            try
            {
                _safeComApp.SignInReleaseAll(Authenticator);

                if (! _safeComApp.StartedProcessingWork(TimeSpan.FromSeconds(10)))
                {
                    return new PluginExecutionResult(PluginResult.Failed, "Documents were not released.", "Device workflow error.");
                }

                RecordEvent(DeviceWorkflowMarker.PrintAllBegin);
                VerifyDeviceFinishedPrinting();
                RecordEvent(DeviceWorkflowMarker.PrintAllEnd);
                return new PluginExecutionResult(PluginResult.Passed);

            }
            finally
            {
                SignOut(_safeComApp);
            }
        }
        private void VerifyRetainStatusChange()
        {
            string docName = string.Empty;

            if (_safeComApp.IsNewVersion())
            {
                docName = _safeComApp.GetFirstDocumentId();
            }
            else
            {
                docName = GetDocumentName(_expectedDocIds[0]);
            }
            if (docName.Equals(_retainedDocumentName))
            {
                throw new DeviceWorkflowException("Document retain status did not change.");
            }
        }

        /// <summary>
        /// Launch the solution.
        /// </summary>
        private void Launch()
        {
            DateTime startTime = DateTime.Now;
            AuthenticationMode am = (_activityData.SafeComAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;

            _safeComApp.WorkflowLogger = Authenticator.WorkflowLogger;

            UpdateLaunchStatus(am, _activityData.AuthProvider, "SafeCom \"Pull Print\"");
            _safeComApp.Launch(Authenticator, am);
            OnTimeStatusUpdate("Launch", startTime, DateTime.Now);
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
                InitialJobCount = _safeComApp.GetDocumentCount();
                return InitialJobCount != 0;
            }
               , TimeSpan.FromSeconds(10));


            if (InitialJobCount < 0)
            {
                throw new DeviceWorkflowException("Unknown solution error encountered.");
            }

            RecordEvent(DeviceWorkflowMarker.DocumentListReady);
            _expectedDocIds.Clear();

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
                FinalJobCount = _safeComApp.GetDocumentCount();

                OnTimeStatusUpdate("SetFinalJobCount", startTime, DateTime.Now);
                OnStatusUpdate($"Available jobs (Final)={FinalJobCount}");
            }
            catch (DeviceInvalidOperationException doe)
            {
                string msg = doe.Message;
                if (msg.Contains("The UIContextId is null and there is no active browser") || msg.Contains("a non-derived FaultException has been thrown by the target operation"))
                {
                    throw new DeviceInvalidOperationException("SafePullPrintManager.SetFinalJobCount: On home screen, unable to determine the Safecom final document count.", doe);
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

            // where there are large amount of documents the start processing work is returning after only six seconds
            // but the icon isn't visible so then we blow by the finishing printing check. waiting for 20 seconds on the
            // list for print all should ensure enough time to verify
            if (_activityData.DocumentProcessAction == SafeComPullPrintAction.PrintAll && InitialJobCount > 50)
            {
                _safeComApp.StartedProcessingWork(TimeSpan.FromSeconds(20));
            }
            else
            {
                _safeComApp.StartedProcessingWork(TimeSpan.FromSeconds(6));
            }
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
            _safeComApp.FinishedProcessingWork();
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            OnTimeStatusUpdate("VerifyDeviceFinishedPrinting", startTime, DateTime.Now);
        }

        
        private void PrintSingleJob()
        {
            try
            {
                string firstDocId = _safeComApp.GetFirstDocumentId();
                _expectedDocIds.Add(firstDocId);

                SelectPrintJobs();
                SetCopyCount();

                RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
                _safeComApp.PrintDelete();
                RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
            }
            catch (JavaScriptExecutionException ex)
            {
                ExecutionServices.SystemTrace.LogError(ex);
                throw;
            }
        }

        private void SelectAllAndPrint()
        {
            try
            {
                _expectedDocIds = _safeComApp.GetDocumentIds().ToList();
                SelectPrintJobs();
                SetCopyCount();

                RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
                _safeComApp.PrintDelete();
                RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
            }
            catch (JavaScriptExecutionException ex)
            {
                ExecutionServices.SystemTrace.LogError(ex);
                throw;
            }
        }

        private void PrintAllJobs()
        {
            SetCopyCount();

            RecordEvent(DeviceWorkflowMarker.PrintAllBegin);
            _safeComApp.PrintAll();
            RecordEvent(DeviceWorkflowMarker.PrintAllEnd);
        }

        private void DeleteAllJobs()
        {
            try
            {
                _expectedDocIds = _safeComApp.GetDocumentIds().ToList();
                SelectPrintJobs();

                RecordEvent(DeviceWorkflowMarker.DeleteBegin);
                _safeComApp.Delete();
                RecordEvent(DeviceWorkflowMarker.DeleteEnd);
            }
            catch (JavaScriptExecutionException ex)
            {
                ExecutionServices.SystemTrace.LogError(ex);
                throw;
            }
        }

        private void DeleteSingleJob()
        {
            try
            {
                string firstDocId = _safeComApp.GetFirstDocumentId();
                _expectedDocIds.Add(firstDocId);

                SelectPrintJobs();
                RecordEvent(DeviceWorkflowMarker.DeleteBegin);
                _safeComApp.Delete();
                RecordEvent(DeviceWorkflowMarker.DeleteEnd);
            }
            catch (JavaScriptExecutionException ex)
            {
                ExecutionServices.SystemTrace.LogError(ex);
                throw;
            }
        }

        private void PrintRetainAllJobs()
        {
            _expectedDocIds = _safeComApp.GetDocumentIds().ToList();
            SelectPrintJobs();
            SetCopyCount();

            RecordEvent(DeviceWorkflowMarker.PrintAllBegin);
            _safeComApp.PrintKeep();
            RecordEvent(DeviceWorkflowMarker.PrintAllEnd);
        }

        /// <summary>
        /// Print Keep Single Print Job
        /// </summary>
        private void PrintRetainSingleJob()
        {
            // javaScript is returning a one quote at the beginning of the string
            string firstDocId = _safeComApp.GetFirstDocumentId().TrimStart('"');
            _expectedDocIds.Add(firstDocId);

            _retainedDocumentName = GetDocumentName(firstDocId);

            SelectPrintJobs();
            _safeComApp.PrintKeep();
        }

        private string GetDocumentName(string firstDocId)
        {
            string docName = string.Empty;
            int idx = 1;

            string[] values = firstDocId.Split('-');
            if (values.Length < 2)
            {
                throw new DeviceWorkflowException("Unable to determine document name for ID " + firstDocId);
            }

            if (values[idx].Contains("Microsoft"))
            {
                idx++;
            }

            if (_safeComApp.IsNewVersion())
            {
                docName = values[idx];
            }
            else
            {
                string docLabelId = values[idx] + "label";

                docName = _safeComApp.GetDocumentNameById(docLabelId).Trim('"');
            }
            return docName;
        }

        private void SelectPrintJobs()
        {
            RecordEvent(DeviceWorkflowMarker.SelectDocumentBegin);
            if (_activityData.SelectAll)
            {
                if (_safeComApp.IsOmni() && _safeComApp.IsNewVersion())
                {
                    List<string> docIds = new List<string>();
                    foreach (string doc in _expectedDocIds)
                    {
                        docIds.Add(GetDocumentId(doc));
                    }

                    _safeComApp.SelectDocuments(docIds);
                }
                else
                {
                    _safeComApp.SelectAllDocuments();
                }
            }
            else
            {
                if (_safeComApp.IsOmni() && _safeComApp.IsNewVersion())
                {
                    _safeComApp.SelectFirstDocument(GetDocumentId(_expectedDocIds.FirstOrDefault()));
                }
                else
                {
                    _safeComApp.SelectFirstDocument();
                }
            }
            RecordEvent(DeviceWorkflowMarker.SelectDocumentEnd);
        }

        private string GetDocumentId(string docIdName)
        {
            string docId = string.Empty;
            if (docIdName.Contains('-'))
            {
                string[] docIds = docIdName.Split('-');
                if (docIds.Count() > 0)
                {
                    docId = docIds[0];
                }
            }

            if (string.IsNullOrEmpty(docId))
            {
                throw new DeviceWorkflowException("SafeCom Document Name not formatted as expected: " + docIdName);
            }

            return docId;
        }

        private bool IsPrinting(JobStatusCheckBy checkBy)
        {
            try
            {
                return _safeComApp.IsPrinting(checkBy);
            }
            catch (Exception ex)
            {
                ExecutionServices.SystemTrace.LogError("Error checking job status via control panel.", ex);
            }

            return false;
        }
        private void SetCopyCount()
        {
            if (_activityData.NumberOfCopies > 1)
            {
                _safeComApp.SetCopyCount(_activityData.NumberOfCopies);
            }
        }


    }
}
