using System;
using System.Collections.Generic;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.SolutionApps.iSecStar;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework.Synchronization;
using System.Linq;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.iSecStarPullPrinting
{
    /// <summary>
    /// iSecStarPullPrintManager class.
    /// </summary>
    public class iSecStarPullPrintManager : PullPrintManager
    {
        private iSecStarActivityData _activityData = null;
        private IiSecStarApp _iSecStarApp = null;
        private List<string> _expectedDocIds = new List<string>();
        

        /// <summary>
        /// Initializes a new instance of the <see cref="iSecStarPullPrintManager"/> class.
        /// The base class, PullPrintManager, maintains the authentication object, _authentication. It is
        /// instantiated using the userCredential object.
        /// </summary>
        /// <param name="pluginExecutionData">The Plugin Execution data.</param>
        /// <param name="deviceInfo">The device information.</param>
        /// <param name="activityData">The activity data.</param>
        public iSecStarPullPrintManager(PluginExecutionData pluginExecutionData, iSecStarActivityData activityData)
            : base(pluginExecutionData)
        {
            _activityData = activityData;
            PullPrintSolution = "ISecStar Printing";
            this.LockTimeouts = _activityData.LockTimeouts;
            this.AuthProvider = _activityData.AuthProvider;
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

            PullPrintLog = SetPullPrintRetrievalLog("ISecStar");

            //Execute the action            
            OnStatusUpdate(_activityData.DocumentProcessAction.GetDescription());
            switch (_activityData.DocumentProcessAction)
            {
                case iSecStarPullPrintAction.Reprint:
                    PrintKeep();
                    break;
                case iSecStarPullPrintAction.Print:
                    Print();
                    break;
                case iSecStarPullPrintAction.Delete:
                    Delete();
                    break;
            }
            if (!_iSecStarApp.BannerErrorState())
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
                case iSecStarPullPrintAction.Delete:
                    break;
                case iSecStarPullPrintAction.Print:
                case iSecStarPullPrintAction.Reprint:
                    VerifyStartPrintingJob();
                    VerifyDeviceFinishedPrinting();
                    break;
            }
            SetFinalJobCount();
        }

        /// <summary>
        /// Executes SignIn and Releases all documents on a device configured to release all documents.
        /// </summary>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        public bool ExecuteSignInReleaseAction()
        {
            bool result = _iSecStarApp.SignInReleaseAll(Authenticator);
            if (result)
            {
                VerifyDeviceFinishedPrinting();
            }
            SignOut(_iSecStarApp);
            return result;
        }

        /// <summary>
        /// Launch the solution.
        /// </summary>
        private void Launch()
        {
            AuthenticationMode am = (_activityData.ISecStarAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;

            UpdateLaunchStatus(am, _activityData.AuthProvider, "ISecStar Secure Pull Print");
            _iSecStarApp.Launch(Authenticator, am);
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
                InitialJobCount = _iSecStarApp.GetDocumentCount();
                return InitialJobCount > 0;
            }
                , TimeSpan.FromSeconds(10));
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
            try
            {
                FinalJobCount = _iSecStarApp.GetDocumentCount();

                OnTimeStatusUpdate("SetFinalJobCount", startTime, DateTime.Now);
                OnStatusUpdate($"Available jobs (Final)={FinalJobCount}");
            }
            catch (DeviceInvalidOperationException doe)
            {
                if (doe.Message.Contains("The UIContextId is null and there is no active browser"))
                {
                    throw new DeviceInvalidOperationException("ISecStarPullPrintManager.SetFinalJobCount: On home screen, unable to determine the ISecSta final document count.", doe);
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
            _iSecStarApp.StartedProcessingWork(TimeSpan.FromSeconds(10));
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
            _iSecStarApp.FinishedProcessingWork();
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
            string id = string.Empty;
            _iSecStarApp.SelectAllDocuments("ckAll");
        }

        private void SelectSingleJob()
        {
            _iSecStarApp.SelectFirstDocument("checkbox");
        }

        /// <summary>
        /// Prints the selected job.
        /// </summary>
        private void Print()
        {
            SelectPrintJobs();
            RecordEvent(DeviceWorkflowMarker.PrintJobBegin);
            _iSecStarApp.Print();
            RecordEvent(DeviceWorkflowMarker.PrintJobEnd);
        }

        /// <summary>
        /// Prints the selected jobs, keeping them on the server.
        /// </summary>
        private void PrintKeep()
        {
            SelectPrintJobs();
            RecordEvent(DeviceWorkflowMarker.PrintKeepBegin);
            _iSecStarApp.PrintKeep();
            RecordEvent(DeviceWorkflowMarker.PrintKeepEnd);
        }

        /// <summary>
        /// Deletes the selected jobs.
        /// </summary>
        private void Delete()
        {
            SelectPrintJobs();
            RecordEvent(DeviceWorkflowMarker.DeleteBegin);
            _iSecStarApp.Delete();
            RecordEvent(DeviceWorkflowMarker.DeleteEnd);
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
                _iSecStarApp = iSecStarAppFactory.Create(Device);
                _iSecStarApp.WorkflowLogger = WorkflowLogger;

                RecordEvent(DeviceWorkflowMarker.ActivityBegin);
                if (!_activityData.ReleaseOnSignIn)
                {
                    Launch();
                    ExecutePullPrintAction();
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
                SignOut(_iSecStarApp);                

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
    }
}
