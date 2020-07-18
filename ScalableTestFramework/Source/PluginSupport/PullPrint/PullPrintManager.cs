using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using HP.ScalableTest.Utility;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.PluginSupport.PullPrint
{
    /// <summary>
    /// Base class for pull print activities
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public abstract class PullPrintManager : IDisposable
    {
        private DeviceWorkflowLogger _workflowLogger = null;        
        
        /// <summary>
        /// Gets or sets the configuration for Acquire and Hold lock timeouts.
        /// </summary>
        protected LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Get or sets the requested authentication provider.
        /// </summary>
        protected AuthenticationProvider AuthProvider { get; set; }

        /// <summary>
        /// Gets or sets the logger for pull print related operations.
        /// </summary>
        protected PullPrintJobRetrievalLog PullPrintLog { get; set; }

        /// <summary>
        /// Gets or sets the pull print solution.
        /// </summary>
        /// <value>
        /// The pull print solution.
        /// </value>
        public virtual string PullPrintSolution { get; set; }

        /// <summary>
        /// The collect memory manager
        /// </summary>
        protected CollectMemoryManager _collectMemoryManager;

        /// <summary>
        /// Gets the execution data.
        /// </summary>
        /// <value>
        /// The execution data.
        /// </value>
        protected PluginExecutionData ExecutionData { get; }

        /// <summary>
        /// Gets the authenticator.
        /// </summary>
        /// <value>
        /// The authenticator.
        /// </value>
        protected IAuthenticator Authenticator { get; set; }

        /// <summary>
        /// Gets the device information.
        /// </summary>
        /// <value>
        /// The device information.
        /// </value>
        protected IDeviceInfo DeviceInfo { get; set; }

        /// <summary>
        /// Gets the device.
        /// </summary>
        /// <value>
        /// The device.
        /// </value>
        public IDevice Device { get; protected set; }

        /// <summary>
        /// Occurs when [status update].
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusUpdate;

        /// <summary>
        /// Occurs when [time status update].
        /// </summary>
        public event EventHandler<TimeStatusEventArgs> TimeStatusUpdate;

        /// <summary>
        /// Occurs when a device is selected for use.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> DeviceSelected;

        /// <summary>
        /// Occurs when [document process selected].
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> DocumentActionSelected;

        /// <summary>
        /// Occurs when time to update the session ID on the exec ctlr label
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> SessionIdUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="PullPrintManager"/> class.
        /// </summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        protected PullPrintManager(PluginExecutionData pluginExecutionData)
        {
            ExecutionData = pluginExecutionData;
            WorkflowLogger = new DeviceWorkflowLogger(pluginExecutionData);
        }

        /// <summary>
        /// Gets the initial job count.
        /// </summary>
        public int InitialJobCount { get; protected set; }

        /// <summary>
        /// Gets the final job count.
        /// </summary>
        public int FinalJobCount { get; protected set; }

        /// <summary>
        /// Gets or sets the <see cref="DeviceWorkflowLogger" /> used by this PullPrintManager.
        /// </summary>
        /// <value>The workflow logger.</value>
        public DeviceWorkflowLogger WorkflowLogger
        {
            get { return _workflowLogger; }
            set
            {
                _workflowLogger = value;
                if (Authenticator != null)
                {
                    Authenticator.WorkflowLogger = _workflowLogger;
                }
            }
        }

        /// <summary>
        /// Collects the memory data.
        /// </summary>
        /// <param name="memoryProfilerConfig">The memory profiler configuration.</param>
        /// <param name="snapshotLabel">The snapshot label.</param>
        public void CollectMemoryData(DeviceMemoryProfilerConfig memoryProfilerConfig, string snapshotLabel)
        {
            _collectMemoryManager.CollectDeviceMemoryProfile(memoryProfilerConfig, snapshotLabel);
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            Device.Dispose();
            Device = null;
        }

        /// <summary>
        /// Executes the pull print operation.
        /// </summary>
        /// <returns></returns>
        public virtual PluginExecutionResult ExecutePullPrintOperation()
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Automation Failure", "Device workflow error.");
            try
            {
                var devices = ExecutionData.Assets.OfType<IDeviceInfo>();
                var assetTokens = devices.Select(n => new AssetLockToken(n, LockTimeouts));

                //If there is no device to run
                if (assetTokens.Count() == 0)
                {
                    //Skip when there are no device to execute on.
                    return new PluginExecutionResult(PluginResult.Skipped, "No Asset available to run.");
                }

                RecordEvent(DeviceWorkflowMarker.DeviceLockBegin);

                ExecutionServices.CriticalSection.Run(assetTokens, selectedToken =>
                {
                    DeviceInfo = (selectedToken as AssetLockToken).AssetInfo as IDeviceInfo;
                    InitializeDevice();
                    InitializeAuthenticator(AuthProvider);

                    OnDeviceSelected(DeviceInfo.AssetId);
                    //OnDocumentProcessSelected(_activityData.DocumentProcessAction.GetDescription());
                    OnSessionIdUpdate(ExecutionData.SessionId);
                    LogDevice(DeviceInfo);

                    using (Device)
                    {
                        PluginRetryManager retryManager = new PluginRetryManager(ExecutionData, this.OnStatusUpdate);
                        result = retryManager.Run(() => LaunchAndPull());
                    }
                });
            }
            catch (AcquireLockTimeoutException)
            {
                result = new PluginExecutionResult(PluginResult.Skipped, "Could not obtain lock on specified device(s).", "Device unavailable.");
            }
            catch (HoldLockTimeoutException)
            {
                result = new PluginExecutionResult(PluginResult.Error, $"Automation did not complete within {LockTimeouts.HoldTimeout}.", "Automation timeout exceeded.");
            }
            finally
            {
                OnStatusUpdate($"Finished {PullPrintSolution} activity");

                WorkflowLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockEnd);
            }
            return result;
        }

        /// <summary>
        /// Launches the Pull print solution (including Auth) and pulls the desired number of documents.
        /// </summary>
        /// <returns></returns>
        protected abstract PluginExecutionResult LaunchAndPull();

        /// <summary>
        /// Verifies that one or more jobs were found.
        /// </summary>
        protected virtual void VerifyJobsFound()
        {
            if (InitialJobCount < 1)
            {
                throw new NoJobsFoundException("No jobs found to print.");
            }
        }

        /// <summary>
        /// Logs workflow information on the device.
        /// </summary>
        /// <param name="deviceInfo">The device information.</param>
        protected virtual void LogDevice(IDeviceInfo deviceInfo)
        {
            var log = new ActivityExecutionAssetUsageLog(ExecutionData, deviceInfo.AssetId);
            ExecutionServices.DataLogger.Submit(log);
        }

        /// <summary>
        /// Sets the pull print retrieval log.
        /// </summary>
        /// <param name="solutionType">Type of the solution.</param>
        /// <returns></returns>
        protected PullPrintJobRetrievalLog SetPullPrintRetrievalLog(string solutionType)
        {
            PullPrintJobRetrievalLog log = new PullPrintJobRetrievalLog(ExecutionData)
            {
                DeviceId = DeviceInfo.AssetId,
                JobStartDateTime = DateTime.Now,
                InitialJobCount = (short)InitialJobCount,
                JobEndStatus = "Failed",
                SolutionType = solutionType,
                UserName = ExecutionData.Credential.UserName,
                NumberOfCopies = 1,
            };
            return log;
        }

        /// <summary>
        /// Submits the pull print retrieval log.
        /// </summary>
        /// <param name="log">The log.</param>
        protected void SubmitPullPrintRetrievalLog(PullPrintJobRetrievalLog log)
        {
            log.FinalJobCount = (short)FinalJobCount;
            log.JobEndDateTime = DateTime.Now;

            ExecutionServices.DataLogger.Submit(log);
        }

        /// <summary>
        /// Fires the StatusUpdate event when the Launch method is called.
        /// </summary>
        protected void UpdateLaunchStatus(AuthenticationMode authMode, AuthenticationProvider authProvider, string solutionButtonText)
        {
            StringBuilder statusMessage = new StringBuilder();

            if (authMode == AuthenticationMode.Eager)
            {
                switch (authProvider)
                {
                    case AuthenticationProvider.Card:
                        statusMessage.Append("Swiping badge on card reader, ");
                        break;
                    case AuthenticationProvider.Skip:
                        statusMessage.Append("Skipping authentication step, ");
                        break;
                    default:
                        statusMessage.Append("Pressing the Sign In button, ");
                        break;
                }
                statusMessage.Append("then pressing the ");
                statusMessage.Append(solutionButtonText);
                statusMessage.Append(" button.");
            }
            else // AuthenticationMode.Lazy
            {
                statusMessage.Append("pressing the ");
                statusMessage.Append(solutionButtonText);
                statusMessage.Append(" button, then authenticating.");
            }

            OnStatusUpdate(statusMessage.ToString());
        }

        /// <summary>
        /// Validates that the document(s) were pulled.
        /// </summary>
        /// <param name="expectedDocIds">The expected document ids.</param>
        /// <param name="deviceInfo">The device information.</param>
        protected virtual void ValidatePull(IList<string> expectedDocIds, IDeviceInfo deviceInfo)
        {
            if (expectedDocIds != null && expectedDocIds.Any())
            {
                OnStatusUpdate("Validating that documents were pulled...");
                ExecutionServices.SessionRuntime.AsInternal().MonitorForDocuments(ExecutionData, deviceInfo, expectedDocIds);
            }
        }

        /// <summary>
        /// Sets the final job count.
        /// </summary>
        protected abstract void SetFinalJobCount();

        /// <summary>
        /// Verifies whether the device has finished printing.
        /// </summary>
        protected abstract void VerifyDeviceFinishedPrinting();

        /// <summary>
        /// Signs the user out of the device.
        /// </summary>
        /// <param name="_pullPrintApp"></param>
        protected void SignOut(IDeviceWorkflowLogSource _pullPrintApp)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                IDevicePreparationManager manager = DevicePreparationManagerFactory.Create(Device);
                manager.WorkflowLogger = _pullPrintApp.WorkflowLogger;
                manager.NavigateHome();
                manager.SignOut();
            }
            catch (Exception ex)
            {
                OnStatusUpdate(ex.Message);
                ExecutionServices.SystemTrace.LogError(ex.ToString());
            }

            OnTimeStatusUpdate("SignOut", startTime, DateTime.Now);
        }

        /// <summary>
        /// Invoke the StatusUpdate Event.
        /// </summary>
        /// <param name="message"></param>
        protected void OnStatusUpdate(string message)
        {
            StatusUpdate?.Invoke(this, new StatusChangedEventArgs(message));
        }

        /// <summary>
        /// Invokes the SessionIdUpdate event 
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        protected void OnSessionIdUpdate(string sessionId)
        {
            SessionIdUpdate?.Invoke(this, new StatusChangedEventArgs(sessionId));
        }

        /// <summary>
        /// Called when [device selected].
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        protected void OnDeviceSelected(string deviceId)
        {
            DeviceSelected?.Invoke(this, new StatusChangedEventArgs(deviceId));
        }

        /// <summary>
        /// Called when [document process selected].
        /// </summary>
        /// <param name="documentProcess">The document process.</param>
        protected void OnDocumentProcessSelected(string documentProcess)
        {
            DocumentActionSelected?.Invoke(this, new StatusChangedEventArgs(documentProcess));
        }

        /// <summary>
        /// Invoke the TimeStatusUpdate Event.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        protected void OnTimeStatusUpdate(string eventName, DateTime start, DateTime end)
        {
            TimeStatusUpdate?.Invoke(this, new TimeStatusEventArgs(eventName, start, end));
        }

        /// <summary>
        /// Records a performance event with the specified event name.
        /// </summary>
        /// <param name="marker">The marker.</param>
        protected void RecordEvent(DeviceWorkflowMarker marker)
        {
            WorkflowLogger?.RecordEvent(marker);
        }
        /// <summary>
        /// Records a performance event with the specified event name concatenating the given parameter to the event.
        /// </summary>
        /// <param name="marker">The <see cref="DeviceWorkflowMarker" />.</param>
        /// <param name="parameter">The parameter to include in the logged marker.</param>
        protected void RecordEvent(DeviceWorkflowMarker marker, string parameter)
        {
            WorkflowLogger?.RecordEvent(marker, parameter);
        }

        /// <summary>
        /// Gathers the triage data for prosperity.
        /// </summary>
        /// <param name="reason">The reason.</param>
        protected virtual void GatherTriageData(string reason)
        {
            ITriage triage = TriageFactory.Create(Device, ExecutionData);
            triage.CollectTriageData(reason);
            triage.Submit();
        }

        /// <summary>
        /// Initializes the device.
        /// </summary>
        protected virtual void InitializeDevice()
        {
            try
            {
                Device = DeviceConstructor.Create(DeviceInfo);
                var preparationManager = DevicePreparationManagerFactory.Create(Device);
                preparationManager.InitializeDevice(AuthProvider != AuthenticationProvider.Skip);
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

                ExecutionServices.SessionRuntime.ReportAssetError(DeviceInfo);

                // Log the error and re-throw.
                ExecutionServices.SystemTrace.LogError(ex);
                throw;
            }
        }

        /// <summary>
        /// Initializes the authenticator.
        /// </summary>
        /// <param name="provider">The provider.</param>
        protected virtual void InitializeAuthenticator(AuthenticationProvider provider)
        {
            switch (provider)
            {
                case AuthenticationProvider.HpId:
                    ExternalCredentialInfo externalCredential = ExecutionData.ExternalCredentials.Where(x => x.Provider == ExternalCredentialType.HpId).FirstOrDefault();
                    AuthenticationCredential authCredential = new AuthenticationCredential(externalCredential.UserName, externalCredential.Password, ExecutionData.Credential.Domain);
                    Authenticator = AuthenticatorFactory.Create(Device, authCredential, provider);
                    break;
                default:
                    Authenticator = AuthenticatorFactory.Create(DeviceInfo.AssetId, Device, provider, ExecutionData);
                    break;
            }

            if (Authenticator != null)
            {
                Authenticator.WorkflowLogger = _workflowLogger;
            }
        }

        /// <summary>
        /// Executes the print job. Sends the selected document(s) to the solution's print server.
        /// </summary>
        /// <param name="documentCollectionIterator">The document collection iterator.</param>
        /// <param name="usePrintServerNotification">if set to <c>true</c> [use print server notification].</param>
        /// <param name="delayAfterPrint">The delay after print.</param>
        public void ExecutePrintJob(DocumentCollectionIterator documentCollectionIterator, bool usePrintServerNotification, int delayAfterPrint)
        {
            OnStatusUpdate("Print queues and documents specified, sending document(s) to the print server...");

            PrintManager printManager = new PrintManager(ExecutionData, documentCollectionIterator);
            try
            {
                printManager.Execute();

                if (!usePrintServerNotification)
                {
                    TimeSpan delay = TimeSpan.FromSeconds(delayAfterPrint);
                    OnStatusUpdate("Print completed. Delaying pull printing by " + delayAfterPrint.ToString() + " seconds.");
                    Thread.Sleep(delay);
                }
                else
                {
                    printManager.WaitOnPrintServerNotification();
                }
            }
            catch (PrintQueueNotAvailableException ex)
            {
                GatherTriageData(ex.ToString());
                throw;
            }
        }
        /// <summary>
        /// Submit PullPrintJobRetrievalLog
        /// </summary>
        /// <param name="status"></param>
        protected virtual void SubmitLog(string status)
        {
            if (PullPrintLog != null)
            {
                PullPrintLog.JobEndStatus = status;
                SubmitPullPrintRetrievalLog(PullPrintLog);
            }
        }

    }
}