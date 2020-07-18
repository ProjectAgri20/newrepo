using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData;
using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;
using System;
using System.Linq;
using System.Net;
using static HP.ScalableTest.Framework.Logger;
using DeviceSimulatorInfo = HP.ScalableTest.Framework.Assets.DeviceSimulatorInfo;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;
using ServerInfo = HP.ScalableTest.Framework.Assets.ServerInfo;

namespace HP.ScalableTest.PluginSupport.JetAdvantageLink
{
    /// <summary>
    /// Base class for managing a plugin scan activity.
    /// </summary>
    public abstract class LinkScanActivityManager : IDisposable
    {
        private readonly string _serverName;        
        private readonly LinkScanOptions _linkScanOptions;
        private readonly LockTimeoutData _lockTimeoutData;

        /// <summary>
        /// Set Exception Category list name for getting for getting Exception.Data
        /// </summary>
        protected string _exceptionCategoryData = "ExceptionCategory";

        /// <summary>
        /// Occurs when the activity status changes.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> ActivityStatusChanged;

        /// <summary>
        /// Occurs when a device is selected for use.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> DeviceSelected;

        /// <summary>
        /// Occurs when a cloud type is selected for use.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> AppNameSelected;

        /// <summary>
        /// Gets the type of the Job Type performed by this <see cref="LinkScanActivityManager" />.
        /// </summary>
        protected abstract string LinkJobType { get; }
        
        /// <summary>
        /// Gets the <see cref="PluginExecutionData" /> for this activity.
        /// </summary>
        protected PluginExecutionData ExecutionData { get; set; }

        /// <summary>
        /// Gets the prefix for the scanned file.
        /// </summary>
        protected ScanFilePrefix FilePrefix { get; }

        /// <summary>
        /// Gets the <see cref="ConnectorJobInputLog" /> for this activity.
        /// </summary>
        protected ConnectorJobInputLog ConnectorLog { get; }

        /// <summary>
        /// Gets the <see cref="DeviceWorkflowLogger" /> for this activity.
        /// </summary>
        protected DeviceWorkflowLogger WorkflowLogger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkPrintActivityManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>        
        /// <param name="lockTimeoutData"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="executionData" /> is null.
        /// <para>or</para>        
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected LinkScanActivityManager(PluginExecutionData executionData, LockTimeoutData lockTimeoutData)
        {
            if (executionData == null)
            {
                throw new ArgumentNullException(nameof(executionData));
            }
                        
            ExecutionData = executionData;
            FilePrefix = new ScanFilePrefix(executionData.SessionId, executionData.Credential.UserName, LinkJobType);
            WorkflowLogger = new DeviceWorkflowLogger(executionData);
            ConnectorLog = new ConnectorJobInputLog(executionData, LinkJobType);            
            _lockTimeoutData = lockTimeoutData;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkPrintActivityManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="linkScanOptions">The scan configuration.</param>
        /// <param name="lockTimeoutData"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="executionData" /> is null.
        /// <para>or</para>
        /// <paramref name="linkScanOptions" /> is null.
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected LinkScanActivityManager(PluginExecutionData executionData, LinkScanOptions linkScanOptions, LockTimeoutData lockTimeoutData)
        {
            if (executionData == null)
            {
                throw new ArgumentNullException(nameof(executionData));
            }

            if (linkScanOptions == null)
            {
                throw new ArgumentNullException(nameof(linkScanOptions));
            }

            ExecutionData = executionData;
            FilePrefix = new ScanFilePrefix(executionData.SessionId, executionData.Credential.UserName, LinkJobType);            
            WorkflowLogger = new DeviceWorkflowLogger(executionData);
            ConnectorLog = new ConnectorJobInputLog(executionData, LinkJobType);
            _linkScanOptions = linkScanOptions;
            _lockTimeoutData = lockTimeoutData;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkPrintActivityManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="linkScanOptions">The cloud print configuration.</param>
        /// <param name="lockTimeoutData"></param>
        /// <param name="serverName">The server to log with the scan.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="executionData" /> is null.
        /// <para>or</para>
        /// <paramref name="linkScanOptions" /> is null.
        /// </exception>
        protected LinkScanActivityManager(PluginExecutionData executionData, LinkScanOptions linkScanOptions, LockTimeoutData lockTimeoutData, string serverName)
            : this(executionData, linkScanOptions, lockTimeoutData)
        {
            _serverName = serverName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkPrintActivityManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="linkScanOptions">The cloud print configuration.</param>
        /// <param name="lockTimeoutData"></param>
        /// <param name="server">The server to log with the scan.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="executionData" /> is null.
        /// <para>or</para>
        /// <paramref name="linkScanOptions" /> is null.
        /// </exception>
        protected LinkScanActivityManager(PluginExecutionData executionData, LinkScanOptions linkScanOptions, LockTimeoutData lockTimeoutData, ServerInfo server)
            : this(executionData, linkScanOptions, lockTimeoutData)
        {
            _serverName = server?.HostName;
        }

        /// <summary>
        /// Runs the scan activity.
        /// </summary>
        /// <returns>The result of the activity.</returns>
        public PluginExecutionResult RunLinkScanActivity()
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Automation Execution Failure", "Device workflow error.");
            try
            {
                var devices = ExecutionData.Assets.OfType<IDeviceInfo>();
                var assetTokens = devices.Select(n => new AssetLockToken(n, _lockTimeoutData));

                RecordEvent(DeviceWorkflowMarker.DeviceLockBegin);
                ExecutionServices.CriticalSection.Run(assetTokens, selectedToken =>
                {
                    IDeviceInfo deviceInfo = (selectedToken as AssetLockToken).AssetInfo as IDeviceInfo;
                    DeviceSelected?.Invoke(this, new StatusChangedEventArgs(deviceInfo.AssetId));

                    if(_linkScanOptions != null)
                    {
                        AppNameSelected?.Invoke(this, new StatusChangedEventArgs(_linkScanOptions.AppName));
                    }
                    else
                    {
                        AppNameSelected?.Invoke(this, new StatusChangedEventArgs(LinkJobType));
                    }

                    ConnectorLog.DeviceId = deviceInfo.AssetId;

                    ExecutionServices.DataLogger.Submit(new ActivityExecutionAssetUsageLog(ExecutionData, deviceInfo));
                    if (_serverName != null)
                    {
                        ExecutionServices.DataLogger.Submit(new ActivityExecutionServerUsageLog(ExecutionData, _serverName));
                    }

                    using (IDevice device = CreateAutomationDevice(deviceInfo))
                    {
                        var retryManager = new PluginRetryManager(ExecutionData, this.UpdateStatus);
                        result = retryManager.Run(() => ExecuteLinkScan(device, deviceInfo));
                    }
                }
                );

                RecordEvent(DeviceWorkflowMarker.DeviceLockEnd);
            }
            catch (ArgumentException ex)
            {
                result = new PluginExecutionResult(PluginResult.Skipped, ex.Message, "Device automation error.");
            }
            catch (InvalidOperationException ex)
            {
                result = new PluginExecutionResult(PluginResult.Skipped, ex.Message, "Device automation error.");
            }
            catch (AcquireLockTimeoutException)
            {
                result = new PluginExecutionResult(PluginResult.Skipped, "Could not obtain lock on specified device(s).", "Device unavailable.");
            }
            catch (HoldLockTimeoutException)
            {
                result = new PluginExecutionResult(PluginResult.Error, $"Automation did not complete within {_lockTimeoutData.HoldTimeout}.", "Automation timeout exceeded.");
            }
            LogDebug($"Scan activity complete. Result is {result.Result.ToString()}");

            return result;
        }

        /// <summary>
        /// Executes the scan job using the specified device.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="deviceInfo">The device information.</param>
        /// <returns>The result of execution.</returns>
        protected virtual PluginExecutionResult ExecuteLinkScan(IDevice device, IDeviceInfo deviceInfo)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Automation Failure", "Device workflow error.");
            try
            {
                // Make sure the device is in a good state
                var devicePrepManager = DevicePreparationManagerFactory.Create(device);
                devicePrepManager.WorkflowLogger = WorkflowLogger;
                devicePrepManager.InitializeDevice(false);
                devicePrepManager.Reset();                

                // Set up the job (enter parameters, etc.)
                RecordEvent(DeviceWorkflowMarker.ActivityBegin);
                UpdateStatus("Setting up job...");
                SetupJob(device);
                UpdateStatus("Job setup complete.");

                // Finish the job (apply job build options, press start, wait for finish)
                UpdateStatus("Finishing job...");
                result = FinishJob(device);
                UpdateStatus("Job finished.");

                // Clean up
                try
                {
                    SignOut();
                    RecordEvent(DeviceWorkflowMarker.ActivityEnd);

                    try {
                    
                        CollectJetAdvantagelinkMemoryMonitoring(deviceInfo);
                    }
                    catch (Exception ex)
                    {
                        LogError(ex.ToString());

                        SubmitConnectorLog(result.Result.ToString());
                        return result;
                    }
                }
                catch (Exception ex) when (ex is DeviceCommunicationException || ex is DeviceInvalidOperationException)
                {
                    // Don't fail the activity if there is an exception here.
                    LogWarn("Device could not return to home screen.");
                    GatherTriageData(device, $"Device could not return to home screen: {ex.ToString()}");
                }
                UpdateStatus("Activity finished.");
            }
            catch (DeviceCommunicationException ex)
            {
                if (ex.Data.Contains(_exceptionCategoryData) && ex.Data[_exceptionCategoryData].Equals(ConnectorExceptionCategory.EnvironmentError.GetDescription()))
                {
                    result = new PluginExecutionResult(PluginResult.Error, ex, $"Webview communication error - You need check \"Debuggable option enable\" to hpk file: {ex.Message}");
                }
                else
                {
                    result = new PluginExecutionResult(PluginResult.Failed, ex, $"Device communication error: {ex.Message}");
                }
            }
            catch (DeviceInvalidOperationException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex, $"Device automation error: {ex.Message}");
     
            }
            catch (DeviceWorkflowException ex)
            {
                if (ex.Data.Contains(_exceptionCategoryData))
                {
                    result = new PluginExecutionResult(PluginResult.Failed, ex, ex.Data[_exceptionCategoryData].ToString());
                }
                else
                {
                    result = new PluginExecutionResult(PluginResult.Failed, ex, "Device workflow error.");
                }

                GatherTriageData(device, ex.ToString());                
            }
            catch (Exception ex)
            {
                GatherTriageData(device, $"Unexpected exception, gathering triage data: {ex.ToString()}");                               
                throw;
            }
            SubmitConnectorLog(result.Result.ToString());
            return result;
        }

        /// <summary>
        /// Sets up the scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected abstract void SetupJob(IDevice device);

        /// <summary>
        /// Finishes the scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>The result of the scan.</returns>
        protected abstract PluginExecutionResult FinishJob(IDevice device);

        /// <summary>
        /// Press SignOut.
        /// </summary>                
        protected abstract void SignOut();

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public abstract void Dispose();

        /// <summary>
        /// Collecct Android MemoryMonitoring.
        /// </summary>        
        protected abstract void CollectJetAdvantagelinkMemoryMonitoring(IDeviceInfo deviceInfo);
        

        /// <summary>
        /// Set options for the scan job.
        /// </summary>
        /// <param name="scanOptionsManager">The JetAdvantageLinkScanOptionManager.</param>
        /// <param name="scanOptions">The LinkScanOptions.</param>
        /// <returns>The result of the scan.</returns>
        protected virtual void SetOptions(JetAdvantageLinkScanOptionManager scanOptionsManager, LinkScanOptions scanOptions)
        {            
            UpdateStatus($"Select option activity with File Name {FilePrefix.ToString().ToLowerInvariant()} is being started");
            scanOptionsManager.SetFileName(FilePrefix.ToString().ToLowerInvariant());
            UpdateStatus($"Select option activity with File Name {FilePrefix.ToString().ToLowerInvariant()} is being completed");

            UpdateStatus($"Set the options screen");
            scanOptionsManager.SetOptionsScreen();

            if (scanOptions.UseFileTypeandResolution)
            {
                UpdateStatus($"Select option activity with File Type {scanOptions.FileType.GetDescription()}, Resolution {scanOptions.Resolution.GetDescription()} is being started");
                scanOptionsManager.SetFileTypeAndResolution(scanOptions.FileType, scanOptions.Resolution);
                UpdateStatus($"Select option activity with File Type {scanOptions.FileType.GetDescription()}, Resolution {scanOptions.Resolution.GetDescription()} is being completed");
            }

            if (scanOptions.UseOriginalSides)
            {
                UpdateStatus($"Select option activity with Original Sides {scanOptions.OriginalSides.GetDescription()} is being started");
                scanOptionsManager.SetOriginalSides(scanOptions.OriginalSides);
                UpdateStatus($"Select option activity with Original Sides {scanOptions.OriginalSides.GetDescription()} is being completed");
            }

            if (scanOptions.UseColorBlack)
            {
                UpdateStatus($"Select option activity with Color/Black {scanOptions.ColorBlack.GetDescription()} is being started");
                scanOptionsManager.SetColorBlack(scanOptions.ColorBlack);
                UpdateStatus($"Select option activity with Color/Black {scanOptions.ColorBlack.GetDescription()} is being completed");
            }

            if (scanOptions.UseOriginalSize)
            {
                UpdateStatus($"Select option activity with Original Size {scanOptions.OriginalSize.GetDescription()} is being started");
                scanOptionsManager.SetOriginalSize(scanOptions.OriginalSize);
                UpdateStatus($"Select option activity with Original Size {scanOptions.OriginalSize.GetDescription()} is being completed");
            }

            if (scanOptions.UseContentOrientation)
            {
                UpdateStatus($"Select option activity with ContentOrientation {scanOptions.ContentOrientation.GetDescription()} is being started");
                scanOptionsManager.SetOrientation(scanOptions.ContentOrientation);
                UpdateStatus($"Select option activity with ContentOrientation {scanOptions.ContentOrientation.GetDescription()} is being completed");
            }            
        }

        private static IDevice CreateAutomationDevice(IDeviceInfo deviceInfo)
        {
            // Jedi simulators stop working if TLS 1.1 or 1.2 are allowed.  Exact cause is unknown, but could be because
            // Windows supports those protocols and the simulator does not.  The negotiation with Windows establishes that
            // one of those should be used, but then the simulator closes the connection because it can't use the selected protocol.
            if (deviceInfo is DeviceSimulatorInfo)
            {
                NetworkConfiguration.SecurityProtocolOverrideValue = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            }
            else
            {
                NetworkConfiguration.SecurityProtocolOverrideValue = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            }

            IDevice device = null;
            try
            {
                device = DeviceConstructor.Create(deviceInfo);
                return device;
            }
            catch (Exception ex)
            {
                // Make sure the device is disposed, if necessary
                if (device != null)
                {
                    device.Dispose();
                    device = null;
                }

                // Log the error and post the exception back to the dispatcher
                LogError($"Error creating device: {ex.Message}");
                ExecutionServices.SessionRuntime.ReportAssetError(deviceInfo);

                throw;
            }
        }

        /// <summary>
        /// Submit TrageData to DB
        /// </summary>
        /// <param name="device"></param>
        /// <param name="reason"></param>    
        protected virtual void GatherTriageData(IDevice device, string reason)
        {
            try
            {
                if (device != null)
                {
                    ITriage triage = TriageFactory.Create(device, ExecutionData);
                    triage.CollectTriageData(reason);
                    triage.Submit();
                }
                else
                {
                    LogDebug("Device is null - cannot gather triage data.");
                }
            }
            catch (Exception)
            {
                UpdateStatus($"GatherTriageData failed - cannot gather triage data.");
            }
        }

        /// <summary>
        /// Records a performance event with the specified <see cref="DeviceWorkflowMarker" />.
        /// </summary>
        /// <param name="marker">The <see cref="DeviceWorkflowMarker" />.</param>
        protected void RecordEvent(DeviceWorkflowMarker marker)
        {
            WorkflowLogger.RecordEvent(marker);
        }

        /// <summary>
        /// Updates the status of the plugin execution.
        /// </summary>
        /// <param name="status">The status.</param>
        protected void UpdateStatus(string status)
        {
            LogInfo(status);
            ActivityStatusChanged?.Invoke(this, new StatusChangedEventArgs(status));
        }

        /// <summary>
        /// Submits the ConnectorJobInputLog.
        /// </summary>
        /// <param name="result"></param>        
        protected virtual void SubmitConnectorLog(string result)
        {
            if(ConnectorLog != null)
            {
                ConnectorLog.AppName = _linkScanOptions.AppName;
                ConnectorLog.JobEndStatus = result;
                ConnectorLog.OptionsData = Serializer.Serialize(_linkScanOptions).ToString();

                UpdateStatus($"SubmitConnectorLog with {ConnectorLog.AppName}");
                ExecutionServices.DataLogger.Submit(ConnectorLog);
            }            
        }
    }
}
