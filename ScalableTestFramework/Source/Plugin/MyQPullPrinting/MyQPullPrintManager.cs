using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.MyQ;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;

namespace HP.ScalableTest.Plugin.MyQPullPrinting
{
    /// <summary>
    /// MyQ Pull Printing Manager Class.
    /// </summary>
    public class MyQPullPrintManager : PullPrintManager
    {
        private static List<MyQPullPrintAction> _validationTargets = new List<MyQPullPrintAction>() { MyQPullPrintAction.Print };

        private MyQPullPrintingActivityData _activityData = null;
        private IMyQApp _myQApp = null;
        private Dictionary<string, string> _documentIds = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MyQPullPrintManager"/> class.
        /// </summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        /// <param name="activityData">The activity data.</param>
        public MyQPullPrintManager(PluginExecutionData pluginExecutionData, MyQPullPrintingActivityData activityData) : base(pluginExecutionData)
        {
            _activityData = activityData;
            PullPrintSolution = "MyQ Print Release";
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

            OnStatusUpdate("Logging into device and launching pull print solution");

            try
            {
                _myQApp = MyQAppFactory.Create(Device);
                _myQApp.WorkflowLogger = WorkflowLogger;

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
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Device workflow error");
                GatherTriageData(ex.ToString());
            }
            catch (NoJobsFoundException ex)
            {
                result = new PluginExecutionResult(PluginResult.Skipped, ex.Message, "Pull print queue empty");
            }
            catch (Exception ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Unknown Exception");
                GatherTriageData(ex.ToString());
            }
            finally
            {
                //Release any connections to the device.  This is critical for Omni operations.
                SubmitLog(result.Result.ToString());
                CollectMemoryData(_activityData.DeviceMemoryProfilerConfig, "MyQPullPrint");
                RecordEvent(DeviceWorkflowMarker.ActivityEnd);
            }
            return result;
        }

        /// <summary>
        /// Defines the flow and executes the pull print action.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void ExecutePullPrintAction()
        {
            SetInitialJobCount();
            VerifyJobsFound();

            PullPrintLog = SetPullPrintRetrievalLog("MyQ");

            //Execute the action            
            OnStatusUpdate(_activityData.DocumentProcessAction.ToString());

            switch (_activityData.DocumentProcessAction)
            {
                case MyQPullPrintAction.PrintAll:
                    PrintAll();
                    break;
                case MyQPullPrintAction.Print:
                    PressPullPrinting();
                    Print();
                    GotoMainPage();
                    break;
                case MyQPullPrintAction.Delete:
                    PressPullPrinting();
                    Delete();
                    GotoMainPage();
                    break;
            }

            if (!_myQApp.BannerErrorState())
            {
                //Final job Count
                VerifyOperation();
            }
            else
            {
                throw new DeviceInvalidOperationException($"Unknown Device Error encountered while trying to verify {_activityData.DocumentProcessAction} operation.");
            }

        }

        /// <summary>
        /// Launch the MyQ solution.
        /// </summary>
        private void Launch()
        {
            AuthenticationMode am = AuthenticationMode.Eager;

            UpdateLaunchStatus(am, _activityData.AuthProvider, "MyQ");
            _myQApp.Launch(Authenticator, am);
        }

        /// <summary>
        /// Sets the initial job count.
        /// </summary>
        private void SetInitialJobCount()
        {
            InitialJobCount = 0;

            DateTime startTime = DateTime.Now;

            Wait.ForTrue(() =>
            {
                InitialJobCount = _myQApp.GetDocumentCount();
                return InitialJobCount != 0;
            }
            , TimeSpan.FromSeconds(10));

            if(InitialJobCount < 0)
            {
                throw new DeviceWorkflowException("InitialJobCount is 0");
            }

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
                if(_activityData.DocumentProcessAction.Equals(MyQPullPrintAction.PrintAll)){
                    FinalJobCount = _myQApp.GetDocumentCount();
                }
                else
                {
                    FinalJobCount = _myQApp.GetDocumentCount();
                }
                return InitialJobCount > FinalJobCount;
            }
            , TimeSpan.FromSeconds(10));

            OnTimeStatusUpdate("SetFinalJobCount", startTime, DateTime.Now);
            OnStatusUpdate($"Available jobs (Final)={FinalJobCount}");

        }

        private void PrintAll()
        {
            RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
            
            _myQApp.PrintAll();
            
            RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
        }

        /// <summary>
        /// Prints the selected jobs, deleting them afterward.
        /// </summary>
        private void Print()
        {
            SelectPrintJobs();

            RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
            _myQApp.Print();
            RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
        }

        /// <summary>
        /// Deletes the selected jobs (without printing them).
        /// </summary>
        private void Delete()
        {
            SelectPrintJobs();

            RecordEvent(DeviceWorkflowMarker.DeleteBegin);
            _myQApp.Delete();
            RecordEvent(DeviceWorkflowMarker.DeleteEnd);
        }

        /// <summary>
        /// Press pull printing button.
        /// </summary>
        private void PressPullPrinting()
        {
            _myQApp.PressPullPrinting();
        }

        /// <summary>
        /// Go to Main Page.
        /// </summary>
        private void GotoMainPage()
        {
            _myQApp.GotoMainPage();
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

        private void SelectSingleJob()
        {
            _myQApp.SelectFirstDocument();
        }

        private void SelectAllJobs()
        {
            _myQApp.SelectAllDocuments();
        }
        private void VerifyOperation()
        {
            switch (_activityData.DocumentProcessAction)
            {
                case MyQPullPrintAction.Delete:
                    break;
                case MyQPullPrintAction.Print:
                    VerifyStartPrinting();
                    VerifyDeviceFinishedPrinting();
                    break;
            }
            
            SetFinalJobCount();
            SignOut(_myQApp);
        }

        /// <summary>
        /// Verifies that the device has started printing the job.
        /// </summary>
        private void VerifyStartPrinting()
        {
            DateTime startTime = DateTime.Now;

            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerBegin);
            _myQApp.StartedProcessingWork(TimeSpan.FromSeconds(7));
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
            _myQApp.FinishedProcessingWork();
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            OnTimeStatusUpdate("VerifyDeviceFinishedPrinting", startTime, DateTime.Now);
        }
    }
}
