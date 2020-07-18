using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.SafeComUC;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HP.ScalableTest.Plugin.SafeComUCPullPrinting
{
    /// <summary>
    /// SafeComPullPrintManager class.
    /// </summary>
    public class SafeComUCPullPrintManager : PullPrintManager
    {
        private static List<SafeComUCPullPrintAction> _validateTargets = new List<SafeComUCPullPrintAction>()
            { SafeComUCPullPrintAction.PrintAll, SafeComUCPullPrintAction.Print, SafeComUCPullPrintAction.PrintRetain, SafeComUCPullPrintAction.PrintRetainAll };

        private SafeComUCPullPrintingActivityData _activityData = null;
        private ISafeComUCApp _safeComUCApp = null;
        private List<string> _expectedDocIds = new List<string>();
        private string _retainedDocumentName = string.Empty;

        public List<string> AffectedDocIds => _expectedDocIds;

        /// <summary>
        /// Initializes a new instance of the <see cref="EquitracPullPrintManager"/> class.
        /// </summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        /// <param name="activityData">The activity data.</param>
        public SafeComUCPullPrintManager(PluginExecutionData pluginExecutionData, SafeComUCPullPrintingActivityData activityData) : base(pluginExecutionData)
        {
            _activityData = activityData;
            PullPrintSolution = "SafeCom Unified Client Pull Printing";
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
                _safeComUCApp = SafeComUCAppFactory.Create(Device);
                _safeComUCApp.WorkflowLogger = WorkflowLogger;

                RecordEvent(DeviceWorkflowMarker.ActivityBegin);
                if (_activityData.ReleaseOnSignIn)
                {
                    result = SignInAndRelease();
                }
                else
                {
                    Launch();

                    switch (_activityData.DocumentProcessAction)
                    {
                        case SafeComUCPullPrintAction.PrintAllApp:
                            ExecutePrintAllAction(); // "Print All" tile
                            break;
                        default:

                            SetInitialJobCount();
                            VerifyJobsFound();
                            ExecutePullPrintAction();

                            // List out the documents that we pulled/deleted
                            if (_activityData.DocumentProcessAction == SafeComUCPullPrintAction.Delete)
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

                            break;
                    }
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
            catch (PullPrintTimeoutException ex)
            {
                // Pull Printing operation took too long.  No real reason to fail the activity, but need to return some information.
                RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
                result = new PluginExecutionResult(PluginResult.Passed, ex.Message, "Pull Print Timeout.");
            }
            catch (Exception ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Unknown Exception.");
                GatherTriageData(ex.ToString());
                throw;
            }
            finally
            {
                SignOut(_safeComUCApp);

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
            PullPrintLog = SetPullPrintRetrievalLog("SafeComUC");

            //Execute the action
            DateTime startTime = ((DateTimeOffset)PullPrintLog.JobStartDateTime).LocalDateTime;
            switch (_activityData.DocumentProcessAction)
            {
                case SafeComUCPullPrintAction.PrintAll:
                    // Print All Button not select all and print
                    PrintAllJobs();
                    break;
                case SafeComUCPullPrintAction.DeleteAll:
                    DeleteAllJobs();
                    break;
                case SafeComUCPullPrintAction.Delete:
                    DeleteSingleJob();
                    break;
                case SafeComUCPullPrintAction.PrintRetainAll:
                    PrintRetainAllJobs();
                    break;
                case SafeComUCPullPrintAction.PrintRetain:
                    PrintRetainSingleJob();
                    break;
                case SafeComUCPullPrintAction.PrintUnretainAll:
                    PrintUnretainAllJobs();
                    break;
                case SafeComUCPullPrintAction.PrintUnretain:
                    PrintUnretainSingleJob();
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
                case SafeComUCPullPrintAction.Delete:
                case SafeComUCPullPrintAction.Refresh:
                case SafeComUCPullPrintAction.DeleteAll:
                    VerifyDeleteJob();
                    break;
                case SafeComUCPullPrintAction.PrintAll:
                case SafeComUCPullPrintAction.Print:
                case SafeComUCPullPrintAction.PrintRetain:
                case SafeComUCPullPrintAction.PrintRetainAll:
                case SafeComUCPullPrintAction.PrintUnretain:
                case SafeComUCPullPrintAction.PrintUnretainAll:
                    VerifyStartPrintingJob();
                    VerifyDeviceFinishedPrinting();
                    break;
            }
            SetFinalJobCount();
        }

        /// <summary>
        /// Executes the Print All action, initiating from the tile on the home screen.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ExecutePrintAllAction()
        {
            PullPrintLog = SetPullPrintRetrievalLog("SafeComUC");
            DateTime startTime = ((DateTimeOffset)PullPrintLog.JobStartDateTime).LocalDateTime;
            
            // At this point, the PrintAll operation has already occurred.  We should be logged in and at the home screen.
            RecordEvent(DeviceWorkflowMarker.AppShown);
            VerifyDeviceFinishedPrinting();                       
        }

        /// <summary>
        /// Used when the sign in and release option is checked
        /// </summary>
        /// <returns>Signin and release action</returns>
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
        /// Used when the sign in and release option is checked.        
        /// </summary>
        /// <returns>true: processing work icon is displayed; false: processing work icon is not displayed</returns>
        public bool ExecuteSignInReleaseAction()
        {
            PullPrintLog = SetPullPrintRetrievalLog("SafeComUC");
            _safeComUCApp.SignInReleaseAll(Authenticator);
            bool result = _safeComUCApp.StartedProcessingWork(_activityData.DocumentProcessAction, TimeSpan.FromSeconds(10));
            if (result)
            {
                RecordEvent(DeviceWorkflowMarker.PrintAllBegin);
                VerifyDeviceFinishedPrinting();
                RecordEvent(DeviceWorkflowMarker.PrintAllEnd);
            }
            SignOut(_safeComUCApp);
            return result;
        }

        /// <summary>
        /// Launch the solution.
        /// </summary>
        private void Launch()
        {
            DateTime startTime = DateTime.Now;
            AuthenticationMode am = (_activityData.SafeComAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;

            _safeComUCApp.WorkflowLogger = Authenticator.WorkflowLogger;

            if(_activityData.DocumentProcessAction == SafeComUCPullPrintAction.PrintAllApp)
            {
                UpdateLaunchStatus(am, _activityData.AuthProvider, "SafeComUC \"Print All\"");
                _safeComUCApp.LaunchPrintAll(Authenticator, am);
            }
            else
            {
                UpdateLaunchStatus(am, _activityData.AuthProvider, "SafeComUC \"Pull Print\"");
                _safeComUCApp.Launch(Authenticator, am);
            }
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
                InitialJobCount = _safeComUCApp.GetDocumentCount();
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
                FinalJobCount = _safeComUCApp.GetDocumentCount();

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

            TimeSpan waitTime;
            switch (_activityData.DocumentProcessAction)
            {
                case SafeComUCPullPrintAction.PrintAll:
                case SafeComUCPullPrintAction.PrintAllApp:
                case SafeComUCPullPrintAction.PrintRetainAll:
                    waitTime = TimeSpan.FromSeconds(20);
                    break;
                default:
                    waitTime = TimeSpan.FromSeconds(6);
                    break;
            }

            if (_safeComUCApp.StartedProcessingWork(_activityData.DocumentProcessAction, waitTime) == false)
            {
                throw new DeviceWorkflowException($"{_activityData.DocumentProcessAction} operation did not begin printing within {waitTime} seconds.");
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
            if (_safeComUCApp.FinishedProcessingWork(_activityData.DocumentProcessAction) == false)
            {
                throw new PullPrintTimeoutException($"{_activityData.DocumentProcessAction} operation did not finish within {_safeComUCApp.DeviceInactivityTimeout.TotalMinutes} minutes.  Returning to home screen.");
            }
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            OnTimeStatusUpdate("VerifyDeviceFinishedPrinting", startTime, DateTime.Now);
        }


        /// <summary>
        /// Verify Retain Status changing
        ///  - Retain icon should be displayed to job list after retain and print test
        ///  - Retain status should be changed to "Unretain" for next test
        /// </summary>
        private void VerifyRetainStatusChange()
        {
            OnStatusUpdate("Verify icon Status.");
            _safeComUCApp.WaitForJobList(InitialJobCount, InitialJobCount * 5);

            if (!_safeComUCApp.WaitForRetainIcon(InitialJobCount * 10))
            {
                throw new DeviceWorkflowException("Document retain icon is not displayed.");
            }

            SelectPrintJobs();

            _safeComUCApp.Menu();
            _safeComUCApp.Unretain();

            OnStatusUpdate("Verify Retain icon disappearing.");

            if (!_safeComUCApp.WaitForRetainIconDisappeared(InitialJobCount * 10))
            {
                throw new DeviceWorkflowException("Document retain icon did not disappear.");
            }
        }

        /// <summary>
        /// Verifies delete job result.
        /// Documents count after delete should be smaller than InitialDocumentCount
        /// </summary>
        protected void VerifyDeleteJob()
        {
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(20);

            while (DateTime.Now < endTime)
            {
                if (_safeComUCApp.GetDocumentCount() < InitialJobCount)
                {
                    OnTimeStatusUpdate("VerifyDeleteJob", startTime, DateTime.Now);
                    break;
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(1000));
            }
            
            
        }

        /// <summary>
        /// It use for a single job print.        
        /// </summary>
        private void PrintSingleJob()
        {
            try
            {
                string firstDocId = _safeComUCApp.GetFirstDocumentId();
                _expectedDocIds.Add(firstDocId);

                SelectPrintJobs();
                SetCopyCount();

                RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
                _safeComUCApp.Print();
                RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
            }
            catch (JavaScriptExecutionException ex)
            {
                ExecutionServices.SystemTrace.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// It use for all job print.        
        /// </summary>
        private void SelectAllAndPrint()
        {
            try
            {
                _expectedDocIds = _safeComUCApp.GetDocumentIds().ToList();
                SelectPrintJobs();
                SetCopyCount();

                RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
                _safeComUCApp.Print();
                RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
            }
            catch (JavaScriptExecutionException ex)
            {
                ExecutionServices.SystemTrace.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// It use for a "Print All" job on the menu
        /// </summary>
        private void PrintAllJobs()
        {
            SetCopyCount();

            _safeComUCApp.Menu();

            RecordEvent(DeviceWorkflowMarker.PrintAllBegin);            
            _safeComUCApp.PrintAll();
            RecordEvent(DeviceWorkflowMarker.PrintAllEnd);
        }

        /// <summary>
        /// It use for a all job delete
        /// </summary>
        private void DeleteAllJobs()
        {
            try
            {
                _expectedDocIds = _safeComUCApp.GetDocumentIds().ToList();
                SelectPrintJobs();
                                
                _safeComUCApp.Menu();

                RecordEvent(DeviceWorkflowMarker.DeleteBegin);
                _safeComUCApp.Delete();
                RecordEvent(DeviceWorkflowMarker.DeleteEnd);
            }
            catch (JavaScriptExecutionException ex)
            {
                ExecutionServices.SystemTrace.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// It use for a single job delete.
        /// </summary>
        private void DeleteSingleJob()
        {
            try
            {
                string firstDocId = _safeComUCApp.GetFirstDocumentId();
                _expectedDocIds.Add(firstDocId);

                SelectPrintJobs();

                _safeComUCApp.Menu();

                RecordEvent(DeviceWorkflowMarker.DeleteBegin);
                _safeComUCApp.Delete();
                RecordEvent(DeviceWorkflowMarker.DeleteEnd);
            }
            catch (JavaScriptExecutionException ex)
            {
                ExecutionServices.SystemTrace.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// It use for all job print and retain.
        /// </summary>
        private void PrintRetainAllJobs()
        {
            _expectedDocIds = _safeComUCApp.GetDocumentIds().ToList();

            SelectPrintJobs();

            if (!_safeComUCApp.WaitForRetainIcon(3))
            {
                _safeComUCApp.Menu();
                _safeComUCApp.Retain();

                if (!_safeComUCApp.WaitForRetainIcon(InitialJobCount * 10))
                {
                    throw new DeviceWorkflowException("Retain icon is not appeared on retain all jobs.");
                }
            }
            
            SetCopyCount();

            RecordEvent(DeviceWorkflowMarker.PrintAllBegin);
            _safeComUCApp.Print();
            RecordEvent(DeviceWorkflowMarker.PrintAllEnd);
        }

        /// <summary>
        /// It use for a single job print and retain
        /// </summary>
        private void PrintRetainSingleJob()
        {
            // javaScript is returning a one quote at the beginning of the string
            string firstDocId = _safeComUCApp.GetFirstDocumentId().TrimStart('"');
            _expectedDocIds.Add(firstDocId);

            _retainedDocumentName = _safeComUCApp.GetFirstDocumentName();

            SelectPrintJobs();

            if (!_safeComUCApp.WaitForRetainIcon(3))
            {
                _safeComUCApp.Menu();
                _safeComUCApp.Retain();

                if (!_safeComUCApp.WaitForRetainIcon(10))
                {
                    throw new DeviceWorkflowException("Retain icon is not appeared on retain single job.");
                }
            }
                        
            SetCopyCount();
            
            RecordEvent(DeviceWorkflowMarker.PrintJobBegin);
            _safeComUCApp.Print();
            RecordEvent(DeviceWorkflowMarker.PrintJobEnd);
        }

        /// <summary>
        /// It use for all job print and unretain.
        /// </summary>
        private void PrintUnretainAllJobs()
        {
            _expectedDocIds = _safeComUCApp.GetDocumentIds().ToList();

            SelectPrintJobs();

            if (_safeComUCApp.WaitForRetainIcon(3))
            {
                _safeComUCApp.Menu();
                _safeComUCApp.Unretain();

                if (!_safeComUCApp.WaitForRetainIconDisappeared(InitialJobCount * 10))
                {
                    throw new DeviceWorkflowException("Retain icon is not disapeared on unretain all jobs.");
                }
            }
            
            SetCopyCount();

            RecordEvent(DeviceWorkflowMarker.PrintAllBegin);
            _safeComUCApp.Print();
            RecordEvent(DeviceWorkflowMarker.PrintAllEnd);
        }

        /// <summary>
        /// It use for a single job print and unretain
        /// </summary>
        private void PrintUnretainSingleJob()
        {
            // javaScript is returning a one quote at the beginning of the string
            string firstDocId = _safeComUCApp.GetFirstDocumentId().TrimStart('"');
            _expectedDocIds.Add(firstDocId);

            _retainedDocumentName = _safeComUCApp.GetFirstDocumentName();

            SelectPrintJobs();

            if (_safeComUCApp.WaitForRetainIcon(3))
            {
                _safeComUCApp.Menu();
                _safeComUCApp.Unretain();

                if (!_safeComUCApp.WaitForRetainIconDisappeared(10))
                {
                    throw new DeviceWorkflowException("Retain icon is not disapeared.");
                }
            }
            
            SetCopyCount();

            RecordEvent(DeviceWorkflowMarker.PrintJobBegin);
            _safeComUCApp.Print();
            RecordEvent(DeviceWorkflowMarker.PrintJobEnd);
        }

        /// <summary>
        /// Select Jobs (Select all or First job)
        /// It verify the checkbox is checked or not before check the checkbox.
        /// If there is only one job on the job list, the checkbox start with checked.
        /// </summary>
        private void SelectPrintJobs()
        {            
            RecordEvent(DeviceWorkflowMarker.SelectDocumentBegin);
            
            if (_activityData.SelectAll)
            {
                if (!_safeComUCApp.IsSelectAllCheckBoxChecked())
                {
                    _safeComUCApp.SelectAllDocuments();
                }
            }
            else
            {
                if (!_safeComUCApp.IsFirstJobCheckBoxChecked())
                {
                    _safeComUCApp.SelectFirstDocument();
                }
            }            
            
            RecordEvent(DeviceWorkflowMarker.SelectDocumentEnd);
        }

        /// <summary>
        /// Set Copy count when selected number is bigger than 1.
        /// </summary>
        private void SetCopyCount()
        {
            if (_activityData.NumberOfCopies > 1)
            {
                _safeComUCApp.SetCopyCount(_activityData.NumberOfCopies);
            }
        }
    }
}
