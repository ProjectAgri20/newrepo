using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.SolutionApps.GeniusBytes;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.GeniusBytes;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.GeniusBytesPullPrinting
{
    /// <summary>
    /// GeniusBytesPullPrintManager class.
    /// </summary>
    public class GeniusBytesPullPrintManager : PullPrintManager
    {
        private GeniusBytesPullPrintingActivityData _activityData = null;
        private IGeniusBytesApp _geniusBytesApp;
        private List<string> _documentIds = new List<string>();
        private static List<GeniusBytesPullPrintAction> _validationTargets = new List<GeniusBytesPullPrintAction>() { GeniusBytesPullPrintAction.PrintAllandDelete };
        private PluginEnvironment _environment;        
        

        /// <summary>
        /// Initializes a new instance of the <see cref="GeniusBytesPullPrintManager"/> class.
        /// </summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        /// <param name="activityData">The activity data.</param>
        public GeniusBytesPullPrintManager(PluginExecutionData pluginExecutionData, GeniusBytesPullPrintingActivityData activityData, PluginEnvironment environment) : base(pluginExecutionData)
        {
            _activityData = activityData;
            PullPrintSolution = "Genius Bytes Pull Printing";
            _environment = environment;
            this.LockTimeouts = _activityData.LockTimeouts;
            this.AuthProvider = _activityData.AuthProvider;
        }

        
        /// <summary>
        /// Initializes the device.
        /// </summary>
        protected override void InitializeDevice()
        {
            try
            {
                Device = DeviceConstructor.Create(DeviceInfo);
                _geniusBytesApp = GeniusBytesAppFactory.Create(Device);
                var preparationManager = new GeniusBytesPreparationManager((JediOmniDevice)Device, _geniusBytesApp);
                preparationManager.InitializeDevice(false);
                _collectMemoryManager = new CollectMemoryManager(Device, DeviceInfo);
            }
            catch (Exception ex)
            {
                // Make sure the device is disposed, if necessary
                if (Device != null)
                {
                    Device.Dispose();
                    Device = null;
                }

                // Log the error and re-throw.
                ExecutionServices.SystemTrace.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// Returns documents affected by pull operation.
        /// </summary>
        public List<string> AffectedDocIds
        {
            get
            {
                if (!_activityData.PrintAll)
                {
                    List<string> result = new List<string>();
                    if (InitialJobCount > 0)
                    {
                        Console.WriteLine($"_documentIds.First() :::: {_documentIds.First()}");                        
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
        protected override PluginExecutionResult LaunchAndPull()
        {
            OnDocumentProcessSelected(_activityData.DocumentProcessAction.GetDescription());
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);
            try
            {
                _geniusBytesApp.WorkflowLogger = WorkflowLogger;
                RecordEvent(DeviceWorkflowMarker.ActivityBegin);

                Console.WriteLine($"Release SignIn ::::::: { _activityData.ReleaseOnSignIn}");

                if (!_activityData.ReleaseOnSignIn)
                {
                    Launch();
                    ExecutePullPrintAction();

                    if (!_activityData.PrintAll)
                    {
                        // List out the documents that we pulled/deleted
                        if (_activityData.DocumentProcessAction == GeniusBytesPullPrintAction.Delete || _activityData.DocumentProcessAction == GeniusBytesPullPrintAction.DeleteAll)
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
                    }
                    
                    ExecutionServices.SystemTrace.LogDebug($"Available jobs (Initial)={InitialJobCount}");
                    ExecutionServices.SystemTrace.LogDebug($"Available jobs (Final)={FinalJobCount}");

                }
                else
                {
                    result = SignInAndRelease();
                }
            }
            catch(DeviceCommunicationException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex.Message, "Device communication error.");
                GatherTriageData(ex.ToString());
            }
            catch(DeviceInvalidOperationException ex)
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
                SignOut();

                SubmitLog(result.Result.ToString());
                CollectMemoryData(_activityData.DeviceMemoryProfilerConfig, "GeniusBytesPullPrint");
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

            PullPrintLog = SetPullPrintRetrievalLog("GeniusBytes");

            //Execute the action            
            OnStatusUpdate(_activityData.DocumentProcessAction.ToString());
            switch (_activityData.DocumentProcessAction)
            {
                case GeniusBytesPullPrintAction.PrintAll:
                    PrintAll();
                    break;
                case GeniusBytesPullPrintAction.PrintAllandDelete:
                    PressPullPrinting();
                    PrintAllandDelete();
                    break;
                case GeniusBytesPullPrintAction.Print:
                    PressPullPrinting();
                    Print();
                    break;
                case GeniusBytesPullPrintAction.PrintandDelete:
                    PressPullPrinting();
                    PrintandDelete();
                    break;
                case GeniusBytesPullPrintAction.Delete:
                    PressPullPrinting();
                    Delete();
                    break;
                case GeniusBytesPullPrintAction.DeleteAll:
                    PressPullPrinting();
                    DeleteAll();
                    break;
            }
            if (!_geniusBytesApp.BannerErrorState())
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
            bool result = _geniusBytesApp.SignInReleaseAll(Authenticator);
            if (result)
            {
                VerifyDeviceFinishedPrinting();
            }
            SignOut();
            return result;
        }

        /// <summary>
        /// Signs the user out of the device.
        /// </summary>
        protected void SignOut()
        {
            try
            {
                DateTime startTime = DateTime.Now;
                _geniusBytesApp.SignOut();
                OnTimeStatusUpdate("SignOut", startTime, DateTime.Now);
                if (!_geniusBytesApp.VerifySignOut())
                {
                    if (!_geniusBytesApp.ExistElementText("Manual Login"))
                    {
                        _geniusBytesApp.SignOut();
                        if (!_geniusBytesApp.VerifySignOut())
                        {
                            throw new DeviceWorkflowException("Failed to Verify SignOut.");
                        }
                    }
                }

            }
            catch (DeviceWorkflowException)
            {
                if (!_geniusBytesApp.VerifySignOut())
                {
                    DateTime startTime = DateTime.Now;
                    OnTimeStatusUpdate("Retry SignOut", startTime, DateTime.Now);
                    _geniusBytesApp.SignOut();
                    if (!_geniusBytesApp.VerifySignOut())
                    {
                        throw new DeviceWorkflowException("Verify SingOut Failure.");
                    }
                }
            }            
        }

        /// <summary>
        /// Launch the Genius Bytes solution.
        /// </summary>
        private void Launch()
        {            
            _geniusBytesApp.Launch(Authenticator);
        }

        /// <summary>
        /// Sets the initial job count.
        /// </summary>
        private void SetInitialJobCount()
        {
            _documentIds.Clear();
            InitialJobCount = 0;

            DateTime startTime = DateTime.Now;

            Wait.ForTrue(() =>
            {
                InitialJobCount = _geniusBytesApp.GetDocumentsCount();
                return InitialJobCount != 0;
            }
            , TimeSpan.FromSeconds(10));

            if (InitialJobCount < 0)
            {
                throw new DeviceWorkflowException("Unknown solution error encountered.");
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
                FinalJobCount = _geniusBytesApp.GetDocumentsCount();
                return InitialJobCount > FinalJobCount;
            }
            , TimeSpan.FromSeconds(10));

            OnTimeStatusUpdate("SetFinalJobCount", startTime, DateTime.Now);
            OnStatusUpdate($"Available jobs (Final)={FinalJobCount}");

        }

        /// <summary>
        /// Sign In and Release
        /// </summary>
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
        /// Prints all jobs, deleting them afterward.
        /// </summary>
        private void PrintAll()
        {
            _geniusBytesApp.PrintAll();
        }

        /// <summary>
        /// Prints all jobs, deleting them afterward.
        /// </summary>
        private void PrintAllandDelete()
        {
            _geniusBytesApp.WaitObjectForAvailable("Print All and Delete", TimeSpan.FromSeconds(5));
            _geniusBytesApp.PrintAllandDelete();
        }

        /// <summary>
        /// Prints the selected jobs.
        /// </summary>
        private void Print()
        {
            SelectSingleJob();
            _geniusBytesApp.Print();
        }

        /// <summary>
        /// Prints the selected jobs, deleting them afterward.
        /// </summary>
        private void PrintandDelete()
        {
            SelectSingleJob();
            _geniusBytesApp.PrintandDelete();
        }

        /// <summary>
        /// Deletes the selected jobs (without printing them).
        /// </summary>
        private void Delete()
        {            
            SelectSingleJob();
            _geniusBytesApp.Delete();
        }

        /// <summary>
        /// Deletes all documents.
        /// </summary>
        private void DeleteAll()
        {
            _geniusBytesApp.DeleteAll();
        }

        /// <summary>
        /// Press pull printing button.
        /// </summary>
        private void PressPullPrinting()
        {
            _geniusBytesApp.PullPrinting();
            _documentIds = _geniusBytesApp.GetDocumentIds();
        }

        /// <summary>
        /// Select first document.
        /// </summary>
        private void SelectSingleJob()
        {
            //_geniusBytesApp.WaitObjectForAvailable("Other", TimeSpan.FromSeconds(4));
            _geniusBytesApp.SelectFirstDocument();
        }

        /// <summary>
        /// Verify operation.
        /// </summary>
        private void VerifyOperation()
        {
            switch (_activityData.DocumentProcessAction)
            {
                case GeniusBytesPullPrintAction.Delete:
                case GeniusBytesPullPrintAction.DeleteAll:
                    _geniusBytesApp.PressConfirmonPopup();
                    _geniusBytesApp.PressBackKey();
                    break;
                case GeniusBytesPullPrintAction.PrintandDelete:                
                case GeniusBytesPullPrintAction.Print:
                    VerifyPrintingColorMode();
                    VerifyStartPrinting();
                    VerifyDeviceFinishedPrinting();
                    _geniusBytesApp.PressBackKey();
                    break;
                case GeniusBytesPullPrintAction.PrintAllandDelete:
                    VerifyPrintingColorMode();
                    VerifyStartPrinting();
                    VerifyPrintAllFinishedPrinting();
                    _geniusBytesApp.PressBackKey();
                    break;
                case GeniusBytesPullPrintAction.PrintAll:
                    VerifyStartPrinting();
                    VerifyPrintAllFinishedPrinting();
                    break;
            }
            _geniusBytesApp.WaitObjectForAvailable("Pull Printing(", TimeSpan.FromSeconds(5));
            SetFinalJobCount();
        }

        /// <summary>
        /// Verify Printing Color mode from GeniusBytes Server.
        /// It only works when tester write "Server Address, Server Id, Server Password".
        /// </summary>
        private void VerifyPrintingColorMode()
        {
            OnStatusUpdate($"VerifyPrintingColorMode({_activityData.UseColorModeNotification})");
            if (_activityData.UseColorModeNotification)
            {
                _geniusBytesApp.PressPrintonPopup();
            }
        }

        /// <summary>
        /// Verifies that the device has started printing the job.
        /// </summary>
        private void VerifyStartPrinting()
        {
            OnStatusUpdate("VerifyStartPrinting()");
            DateTime startTime = DateTime.Now;

            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerBegin);
            _geniusBytesApp.StartedProcessingWork(TimeSpan.FromSeconds(7));
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
            _geniusBytesApp.FinishedProcessingWork();
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            OnTimeStatusUpdate("VerifyDeviceFinishedPrinting", startTime, DateTime.Now);
        }

        /// <summary>
        /// Verifies whether the device has finished printing by checking the job status and
        /// waiting until clear before releasing the control panel to other operations.
        /// Only for print all job, it check again the job status for next job.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        protected void VerifyPrintAllFinishedPrinting()
        {
            DateTime startTime = DateTime.Now;
            bool isPrinting = false;

            RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
            do
            {
                // If FinishedProcessingWork returns false, the code will exit, just like it would do in VerifyDeviceFinishedPrinting
                if (_geniusBytesApp.FinishedProcessingWork())
                {
                    //Check to see if another print job started
                    isPrinting = _geniusBytesApp.StartedProcessingWork(TimeSpan.FromSeconds(3));
                }
            } while (isPrinting);            
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            OnTimeStatusUpdate("VerifyPrintAllFinishedPrinting", startTime, DateTime.Now);
        }

    }
}
