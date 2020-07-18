using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.SolutionApps.Celiveo;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.Celiveo
{
    /// <summary>
    /// Celiveo Pull print manager class
    /// </summary>
    /// <seealso cref="HP.ScalableTest.PluginSupport.PullPrint.PullPrintManager" />
    public class CeliveoPullPrintManager : PullPrintManager
    {
        private CeliveoActivityData _activityData = null;
        private ICeliveoApp _celiveoApp = null;
        private List<string> _documentIds = new List<string>();
        private static List<CeliveoPullPrintAction> _validationTargets = new List<CeliveoPullPrintAction>() { CeliveoPullPrintAction.Print };

        public CeliveoPullPrintManager(PluginExecutionData pluginExecutionData, CeliveoActivityData activityData) : base(pluginExecutionData)
        {
            _activityData = activityData;
            PullPrintSolution = "Celiveo Printing";
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
                _celiveoApp = CeliveoAppFactory.Create(Device);
                _celiveoApp.WorkflowLogger = WorkflowLogger;

                RecordEvent(DeviceWorkflowMarker.ActivityBegin);
                if (!_activityData.ReleaseOnSignIn)
                {
                    Launch();
                    ExecutePullPrintAction();

                    // List out the documents that we pulled/deleted
                    if (_activityData.DocumentProcessAction == CeliveoPullPrintAction.Delete)
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
                SignOut(_celiveoApp);
                SubmitLog(result.Result.ToString());
                CollectMemoryData(_activityData.DeviceMemoryProfilerConfig, "CeliveoPullPrint");

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
            SetInitialJobCount();
            VerifyJobsFound();

            PullPrintLog = SetPullPrintRetrievalLog("Celiveo");

            //Execute the action            
            OnStatusUpdate(_activityData.DocumentProcessAction.ToString());
            switch (_activityData.DocumentProcessAction)
            {
                case CeliveoPullPrintAction.Print:
                    Print();
                    break;
                case CeliveoPullPrintAction.PrintBW:
                    PrintBW();
                    break;
                case CeliveoPullPrintAction.Delete:
                    Delete();
                    break;
            }

            if (!_celiveoApp.BannerErrorState())
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
            bool result = _celiveoApp.SignInReleaseAll(Authenticator);
            if (result)
            {
                VerifyDeviceFinishedPrinting();
            }
            SignOut(_celiveoApp);
            return result;
        }

        

        /// <summary>
        /// Launch the Celiveo solution.
        /// </summary>
        private void Launch()
        {
            AuthenticationMode am = (_activityData.CeliveoAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;

            UpdateLaunchStatus(am, _activityData.AuthProvider, "My print jobs");
            _celiveoApp.Launch(Authenticator, am);
        }

        /// <summary>
        /// Sets the initial job count.
        /// </summary>
        private void SetInitialJobCount()
        {
            _documentIds.Clear();
            InitialJobCount = 0;

            Wait.ForTrue(() =>
            {
                InitialJobCount = _celiveoApp.GetDocumentCount();
                return InitialJobCount != 0;
            }
            , TimeSpan.FromSeconds(25));

            if (InitialJobCount < 0)
            {
                throw new DeviceWorkflowException("Unknown solution error encountered.");
            }
            RecordEvent(DeviceWorkflowMarker.DocumentListReady);

            _celiveoApp.SetCurrentVersion();
            _documentIds = _celiveoApp.GetDocumentIds();
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
                FinalJobCount = _celiveoApp.GetDocumentCount();
                return InitialJobCount > FinalJobCount;
            }
            , TimeSpan.FromSeconds(10));

            OnTimeStatusUpdate("SetFinalJobCount", startTime, DateTime.Now);
            OnStatusUpdate($"Available jobs (Final)={FinalJobCount}");

        }
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

            if (_activityData.NumberOfCopies > 1)
            {
                _celiveoApp.SetCopyCount(_activityData.NumberOfCopies);
            }

            RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
            _celiveoApp.Print();
            RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
        }

        /// <summary>
        /// Prints the selected jobs, deleting them afterward.
        /// </summary>
        private void PrintBW()
        {
            SelectPrintJobs();

            if (_activityData.NumberOfCopies > 1)
            {
                _celiveoApp.SetCopyCount(_activityData.NumberOfCopies);
            }

            RecordEvent(DeviceWorkflowMarker.PrintDeleteBegin);
            _celiveoApp.PrintBW();
            RecordEvent(DeviceWorkflowMarker.PrintDeleteEnd);
        }

        /// <summary>
        /// Deletes the selected jobs (without printing them).
        /// </summary>
        private void Delete()
        {
            SelectPrintJobs();

            RecordEvent(DeviceWorkflowMarker.DeleteBegin);
            _celiveoApp.Delete();
            RecordEvent(DeviceWorkflowMarker.DeleteEnd);
        }

        private void SelectPrintJobs()
        {
            // Celiveo defaults selected the document when it have only one job.
            // So, if InitialJobCount is less or equal than 1, there is nothing to do from this function.
            if (InitialJobCount > 1)
            {
                if (!_activityData.SelectAll)
                {
                    RecordEvent(DeviceWorkflowMarker.SelectDocumentBegin);
                    SelectSingleJob();
                    RecordEvent(DeviceWorkflowMarker.SelectDocumentEnd);
                }
                else
                {
                    //selectall
                    RecordEvent(DeviceWorkflowMarker.SelectDocumentBegin);
                    SelectAllJob();
                    RecordEvent(DeviceWorkflowMarker.SelectDocumentEnd);
                }
            }
        }

        private void SelectSingleJob()
        {
            _celiveoApp.SelectFirstDocument(_documentIds.First());
        }

        private void SelectAllJob()
        {
            _celiveoApp.SelectAll();
        }

        private void VerifyOperation()
        {
            switch (_activityData.DocumentProcessAction)
            {
                case CeliveoPullPrintAction.Delete:
                    VerifyDelete();
                    _celiveoApp.DismissPostDeleteOperation();
                    break;
                case CeliveoPullPrintAction.Print:
                case CeliveoPullPrintAction.PrintBW:
                    _celiveoApp.DismissPostPrintOperation();
                    VerifyStartPrinting();
                    VerifyDeviceFinishedPrinting();
                    break;
            }
            SetFinalJobCount();
        }

        /// <summary>
        /// Verifies that the device has started printing the job.
        /// </summary>
        private void VerifyDelete()
        {
            OnStatusUpdate("VerifyDelete()");
            DateTime startTime = DateTime.Now;

            _celiveoApp.VerifyDeleteJob();
            OnTimeStatusUpdate("VerifyDeleteJob", startTime, DateTime.Now);
        }

        /// <summary>
        /// Verifies that the device has started printing the job.
        /// </summary>
        private void VerifyStartPrinting()
        {
            OnStatusUpdate("VerifyStartPrinting()");
            DateTime startTime = DateTime.Now;

            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerBegin);
            _celiveoApp.StartedProcessingWork(TimeSpan.FromSeconds(7));
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
            _celiveoApp.FinishedProcessingWork();
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            OnTimeStatusUpdate("VerifyDeviceFinishedPrinting", startTime, DateTime.Now);
        }

        
    }
}
