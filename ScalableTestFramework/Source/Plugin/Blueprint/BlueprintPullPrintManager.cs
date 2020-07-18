using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.SolutionApps.Blueprint;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.Blueprint
{
    /// <summary>
    /// BlueprintPullPrintManager Class
    /// </summary>
    public class BlueprintPullPrintManager : PullPrintManager
    {

        private static List<BlueprintPullPrintAction> _validateTargets = new List<BlueprintPullPrintAction>()
            { BlueprintPullPrintAction.PrintAll, BlueprintPullPrintAction.Print, BlueprintPullPrintAction.Delete };

        private BlueprintActivityData _activityData = null;
        private IBlueprintApp _bluePrintApp = null;
        private List<string> _expectedDocIds = new List<string>();

        private List<string> _bluePrintDocuments = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BlueprintPullPrintManager" /> class.
        /// The base class, PullPrintManager, maintains the authentication object, _authentication. It is
        /// instantiated using the userCredential object.
        /// </summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        /// <param name="activityData">The activity data.</param>
        public BlueprintPullPrintManager(PluginExecutionData pluginExecutionData, BlueprintActivityData activityData) : base(pluginExecutionData)
        {
            _activityData = activityData;
            PullPrintSolution = "BluePrint Print Release";
            this.LockTimeouts = _activityData.LockTimeouts;
            this.AuthProvider = _activityData.AuthProvider;
        }
        /// <summary>
        /// Gets the affected document ids.
        /// </summary>
        /// <value>
        /// The affected document ids.
        /// </value>
        public List<string> AffectedDocIds => _expectedDocIds;

        /// <summary>
        /// Launches the Pull print solution and pulls the desired number of documents.
        /// </summary>
        /// <returns>PluginExecutionResult</returns>
        protected override PluginExecutionResult LaunchAndPull()
        {
            OnDocumentProcessSelected(_activityData.DocumentProcessAction.GetDescription());
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);

            OnStatusUpdate("Logging into device and launching pull print app...");
            try
            {
                _bluePrintApp = BlueprintAppFactory.Create(Device);
                _bluePrintApp.WorkflowLogger = WorkflowLogger;

                RecordEvent(DeviceWorkflowMarker.ActivityBegin);
                Launch();
                ExecutePullPrintAction();

                // List out the documents that we pulled/deleted
                if (_activityData.DocumentProcessAction == BlueprintPullPrintAction.Delete)
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
                SignOut(_bluePrintApp);
                SubmitLog(result.Result.ToString());
                CollectMemoryData(_activityData.DeviceMemoryProfilerConfig, "BluePrintPullPrint");
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

            PullPrintLog = SetPullPrintRetrievalLog("Blueprint");

            //Execute the action            
            OnStatusUpdate(_activityData.DocumentProcessAction.ToString());
            switch (_activityData.DocumentProcessAction)
            {
                case BlueprintPullPrintAction.PrintAll:
                    // Print All Button not select all and print
                    PrintAll();
                    break;
                case BlueprintPullPrintAction.Print:
                    Print();
                    break;
                case BlueprintPullPrintAction.Delete:
                    Delete();
                    break;
            }
            if (!_bluePrintApp.BannerErrorState())
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
                case BlueprintPullPrintAction.Delete:
                    break;
                case BlueprintPullPrintAction.PrintAll:
                case BlueprintPullPrintAction.Print:
                    VerifyStartPrintingJob();
                    VerifyDeviceFinishedPrinting();
                    break;
            }

            SetFinalJobCount();
        }


        /// <summary>
        /// Launch the solution.
        /// </summary>
        private void Launch()
        {
            AuthenticationMode am = (_activityData.BlueprintAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;

            UpdateLaunchStatus(am, _activityData.AuthProvider, "Blueprint Blue Print");
            _bluePrintApp.Launch(Authenticator, am);
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
                InitialJobCount = _bluePrintApp.GetDocumentCount();
                return InitialJobCount != 0;
            }
                , TimeSpan.FromSeconds(10));
            RecordEvent(DeviceWorkflowMarker.DocumentListReady);
            _expectedDocIds.Clear();

            if (InitialJobCount < 0)
            {
                throw new DeviceWorkflowException("Unknown solution error encountered.");
            }

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
                FinalJobCount = _bluePrintApp.GetDocumentCount();

                OnTimeStatusUpdate("SetFinalJobCount", startTime, DateTime.Now);
                OnStatusUpdate($"Available jobs (Final)={FinalJobCount}");
            }
            catch (DeviceInvalidOperationException doe)
            {
                string msg = doe.Message;
                if (msg.Contains("The UIContextId is null and there is no active browser") || msg.Contains("a non-derived FaultException has been thrown by the target operation"))
                {
                    throw new DeviceInvalidOperationException("BlueprintPullPrintManager.SetFinalJobCount: On home screen, unable to determine the Blueprint final document count.", doe);
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
            _bluePrintApp.StartedProcessingWork(TimeSpan.FromSeconds(7));
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
            _bluePrintApp.FinishedProcessingWork();
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            OnTimeStatusUpdate("VerifyDeviceFinishedPrinting", startTime, DateTime.Now);
        }

        
        private void SelectPrintJobs()
        {
            RecordEvent(DeviceWorkflowMarker.SelectDocumentBegin);

            SelectSingleJob();

            RecordEvent(DeviceWorkflowMarker.SelectDocumentEnd);
        }

        private void SelectSingleJob()
        {
            string firstDocId = _bluePrintApp.GetFirstDocumentId();
            _expectedDocIds.Add(firstDocId);
            _bluePrintApp.SelectFirstDocument(firstDocId);
        }

        /// <summary>
        /// Prints all jobs.
        /// </summary>
        private void PrintAll()
        {
            RecordEvent(DeviceWorkflowMarker.PrintAllBegin);
            _bluePrintApp.PrintAll();
            RecordEvent(DeviceWorkflowMarker.PrintAllEnd);
        }

        /// <summary>
        /// Prints the selected jobs, keeping them on the server.
        /// </summary>
        private void Print()
        {
            SelectPrintJobs();

            RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
            _bluePrintApp.Print();
            RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
        }

        /// <summary>
        /// Deletes the selected jobs.
        /// </summary>
        private void Delete()
        {
            SelectPrintJobs();

            RecordEvent(DeviceWorkflowMarker.DeleteBegin);
            _bluePrintApp.Delete();
            RecordEvent(DeviceWorkflowMarker.DeleteEnd);
        }

    }
}
