using System;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.HpRoam;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.HpRoam
{
    /// <summary></summary>
    /// Manages Pull Print operations via Roam on OXPd device.
    /// <seealso cref="HP.ScalableTest.PluginSupport.PullPrint.PullPrintManager" />
    public class RoamOxpdPullPrintManager : PullPrintManager
    {
        private HpRoamActivityData _activityData = null;
        private PullPrintJobRetrievalLog _pullPrintLog = null;
        private IHpRoamApp _hpRoamApp = null;

        /// <summary>Initializes a new instance of the <see cref="RoamOxpdPullPrintManager"/> class.</summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        /// <param name="activityData">The activity data.</param>
        public RoamOxpdPullPrintManager(PluginExecutionData pluginExecutionData, HpRoamActivityData activityData)
            : base(pluginExecutionData)
        {
            _activityData = activityData;
            PullPrintSolution = "HP Roam Oxpd Pullprint Activity";
            this.LockTimeouts = _activityData.LockTimeouts;
            this.AuthProvider = _activityData.AuthProvider;
        }

        /// <summary>Launch the solution.</summary>
        private void Launch()
        {
            AuthenticationMode am = (_activityData.HPRoamAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;
            if (am.Equals(AuthenticationMode.Eager))
            {
                OnStatusUpdate("Authenticating by pressing the Sign In button first.");
            }
            else // AuthenticationMode.Lazy
            {
                OnStatusUpdate("Authenticating by pressing the Hp Roam Button first.");
            }

            _hpRoamApp.Launch(Authenticator, am);
            OnStatusUpdate("Authentication complete.");
        }

        /// <summary>Launches the Pull print solution and pulls the desired number of documents.</summary>
        /// <returns></returns>
        protected override PluginExecutionResult LaunchAndPull()
        {
            OnDocumentProcessSelected(_activityData.DocumentProcessAction.GetDescription());
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);

            OnStatusUpdate("Logging into device and launching pull print app...");
            try
            {
                _hpRoamApp = HpRoamAppFactory.Create(Device);
                _hpRoamApp.WorkflowLogger = WorkflowLogger;

                RecordEvent(DeviceWorkflowMarker.ActivityBegin);
                Launch();
                ExecutePullPrintAction();

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
                SignOut(_hpRoamApp);

                SubmitLog(result.Result.ToString());
                CollectMemoryData(_activityData.DeviceMemoryProfilerConfig, "HpRoamPullPrint");
                RecordEvent(DeviceWorkflowMarker.ActivityEnd);

                //Release any connections to the device.  This is critical for Omni operations.
                Dispose();
            }

            return result;
        }

        /// <summary>Executes the pull print action.
        /// Print Single, Print All, Delete Single, etc.</summary>
        /// <exception cref="DeviceInvalidOperationException">Unknown Device Error encountered while trying to process pull print.</exception>
        private void ExecutePullPrintAction()
        {
            SetInitialJobCount();
            VerifyJobsFound();

            PullPrintLog = SetPullPrintRetrievalLog("HpRoam");

            //Execute the action            
            OnStatusUpdate(_activityData.DocumentProcessAction.ToString());
            switch (_activityData.DocumentProcessAction)
            {
                case HpRoamPullPrintAction.Print:
                    Print();
                    JobProcessing();
                    break;
                case HpRoamPullPrintAction.PrintAll:
                    PrintAll();
                    JobProcessing();
                    break;
                case HpRoamPullPrintAction.Delete:
                    Delete();
                    break;
            }

            CheckForBannerError();
            SetFinalJobCount();
        }

        private void JobProcessing()
        {
            CheckForBannerError();

            VerifyStartPrintingJob();
            VerifyDeviceFinishedPrinting();
        }

        protected override void SetFinalJobCount()
        {
            DateTime startTime = DateTime.Now;
            try
            {
                FinalJobCount = _hpRoamApp.GetDocumentCount();

                OnTimeStatusUpdate("SetFinalJobCount", startTime, DateTime.Now);
                OnStatusUpdate($"Available jobs (Final)={FinalJobCount}");
            }
            catch (DeviceInvalidOperationException doe)
            {
                string msg = doe.Message;
                if (msg.Contains("The UIContextId is null and there is no active browser") || msg.Contains("a non-derived FaultException has been thrown by the target operation"))
                {
                    throw new DeviceInvalidOperationException("HpRoamPullPrintManager.SetFinalJobCount: On home screen, unable to determine the HpRoam final document count.", doe);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>Sets the initial job count.</summary>
        private void SetInitialJobCount()
        {
            DateTime startTime = DateTime.Now;

            OnStatusUpdate("Getting initial job count.");
            InitialJobCount = 0;
            FinalJobCount = -1;

            Wait.ForTrue(() =>
            {
                InitialJobCount = _hpRoamApp.GetDocumentCount();
                return InitialJobCount > 0;
            }
                , TimeSpan.FromSeconds(25));
            RecordEvent(DeviceWorkflowMarker.DocumentListReady);

            OnStatusUpdate($"Available jobs (Initial)={InitialJobCount}");
        }

        /// <summary>
        /// Verifies the start printing job.
        /// </summary>
        private void VerifyStartPrintingJob()
        {
            DateTime startTime = DateTime.Now;

            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerBegin);
            _hpRoamApp.StartedProcessingWork(TimeSpan.FromSeconds(7));
            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerEnd);

            OnTimeStatusUpdate("VerifyStartPrintingJob", startTime, DateTime.Now);
        }

        /// <summary>Verifies whether the device has finished printing.</summary>
        protected override void VerifyDeviceFinishedPrinting()
        {
            DateTime startTime = DateTime.Now;
            TimeSpan waitTime;

            switch (_activityData.DocumentProcessAction)
            {
                case HpRoamPullPrintAction.PrintAll:
                    //The lock hold timeout will have already started counting down, so we want this to timeout before that one does.
                    waitTime = _activityData.LockTimeouts.HoldTimeout.Subtract(TimeSpan.FromMinutes(3));
                    break;
                default:
                    waitTime = TimeSpan.FromMinutes(2);
                    break;
            }

            RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
            FinishedProcessingJob(waitTime);
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            OnTimeStatusUpdate("VerifyDeviceFinishedPrinting", startTime, DateTime.Now);
        }

        /// <summary>Finished the processing job.</summary>
        private void FinishedProcessingJob(TimeSpan waitTime)
        {
            bool result = Wait.ForTrue(() =>
            {
                bool finished = CheckForJobFinished();
                _hpRoamApp.KeepAwake();
                CheckForRoamAlert();

                return finished;
            }
            , waitTime);

            if (!result)
            {
                throw new PullPrintTimeoutException($"Print operation did not finish within {waitTime.TotalMinutes} minutes.  Returning to home screen.");
            }
        }

        private bool CheckForJobFinished()
        {
            return Wait.ForTrue(() => _hpRoamApp.FinishedProcessingJob()
            , TimeSpan.FromSeconds(5));
        }

        private void CheckForRoamAlert()
        {
            string alertText = _hpRoamApp.GetAlertText();
            if (!string.IsNullOrEmpty(alertText))
            {
                _hpRoamApp.HandleAlert();
                throw new DeviceWorkflowException(alertText);
            }
        }

        private void CheckForBannerError()
        {
            if (_hpRoamApp.BannerErrorState())
            {
                throw new DeviceWorkflowException("Unknown Device Error detected in device masthead.");
            }
        }

        private void SelectSingleJob()
        {
            RecordEvent(DeviceWorkflowMarker.SelectDocumentBegin);
            _hpRoamApp.SelectFirstDocument();
            RecordEvent(DeviceWorkflowMarker.SelectDocumentEnd);
        }

        private void ApplyPullPrintDelay()
        {
            //Allow the device time to get info about the doc(s) from the Roam server.
            OnStatusUpdate($"Delaying {_activityData.DelayBeforePullPrint} seconds.");
            Delay.Wait(TimeSpan.FromSeconds(_activityData.DelayBeforePullPrint));
        }

        /// <summary>
        /// Prints all jobs.
        /// </summary>
        private void PrintAll()
        {
            // Assumes all documents are selected by default, hence no performance markers.
            ApplyPullPrintDelay();

            RecordEvent(DeviceWorkflowMarker.PrintAllBegin);
            _hpRoamApp.PrintAll();
            RecordEvent(DeviceWorkflowMarker.PrintAllEnd);
        }

        /// <summary>
        /// Prints the first job listed.
        /// </summary>
        private void Print()
        {
            SelectSingleJob();
            ApplyPullPrintDelay();

            RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
            _hpRoamApp.Print();
            RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
        }

        /// <summary>
        /// Deletes the selected jobs.
        /// </summary>
        private void Delete()
        {
            RecordEvent(DeviceWorkflowMarker.DeleteBegin);
            _hpRoamApp.Delete();
            _hpRoamApp.FinishProcessDelete(InitialJobCount);
            RecordEvent(DeviceWorkflowMarker.DeleteEnd);
        }
    }
}
