using System;
using System.Linq;
using System.Net;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;
using DeviceSimulatorInfo = HP.ScalableTest.Framework.Assets.DeviceSimulatorInfo;
using ServerInfo = HP.ScalableTest.Framework.Assets.ServerInfo;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Framework.Assets;
using System.Collections.Generic;
using System.Reflection;


namespace HP.ScalableTest.PluginSupport.Scan
{
    /// <summary>
    /// Base class for managing a plugin scan activity.
    /// </summary>
    public abstract class ScanActivityManager
    {
        private readonly string _serverName;

        private bool _adfDocumentsLoaded = false;

        /// <summary>
        /// Occurs when the activity status changes.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> ActivityStatusChanged;

        /// <summary>
        /// Occurs when a device is selected for use.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> DeviceSelected;

        /// <summary>
        /// Gets the type of the scan performed by this <see cref="ScanActivityManager" />.
        /// </summary>
        protected abstract string ScanType { get; }

        /// <summary>
        /// Gets the <see cref="PluginExecutionData" /> for this activity.
        /// </summary>
        protected PluginExecutionData ExecutionData { get; }

        /// <summary>
        /// Gets the prefix for the scanned file.
        /// </summary>
        protected ScanFilePrefix FilePrefix { get; }

        /// <summary>
        /// Gets the <see cref="DigitalSendJobInputLog" /> for this activity.
        /// </summary>
        protected DigitalSendJobInputLog ScanLog { get; }

        /// <summary>
        /// Gets the <see cref="DeviceWorkflowLogger" /> for this activity.
        /// </summary>
        protected DeviceWorkflowLogger WorkflowLogger { get; }

        /// <summary>
        /// Gets and sets the <see cref="ScanOptions" /> for this activity.
        /// </summary>
        protected ScanOptions ScanOptions { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Authenticator"/> for this activity
        /// </summary>
        protected IAuthenticator Authenticator { get; set; }

        /// <summary>
        /// Gets a value indicating whether to use job build.
        /// </summary>
        /// <value><c>true</c> if job build should be used; otherwise, <c>false</c>.</value>
        protected bool UseJobBuild
        {
            get { return !_adfDocumentsLoaded && ScanOptions.PageCount > 1; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanActivityManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <exception cref="ArgumentNullException" />
        /// <paramref name="executionData" /> is null.
        /// <para>or</para>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        protected ScanActivityManager(PluginExecutionData executionData)
        {
            if (executionData == null)
            {
                throw new ArgumentNullException(nameof(executionData));
            }

            ExecutionData = executionData;
            FilePrefix = new ScanFilePrefix(executionData.SessionId, executionData.Credential.UserName, ScanType);
            ScanLog = new DigitalSendJobInputLog(executionData, FilePrefix, ScanType);
            WorkflowLogger = new DeviceWorkflowLogger(executionData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanActivityManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="serverName">The server to log with the scan.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="executionData" /> is null.
        /// <para>or</para>
        /// </exception>
        protected ScanActivityManager(PluginExecutionData executionData, string serverName)
            : this(executionData)
        {
            _serverName = serverName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanActivityManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="server">The server to log with the scan.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="executionData" /> is null.
        /// <para>or</para>
        /// </exception>
        protected ScanActivityManager(PluginExecutionData executionData, ServerInfo server)
            : this(executionData)
        {
            _serverName = server?.HostName;
        }

        /// <summary>
        /// Runs the scan activity.
        /// </summary>
        /// <returns>The result of the activity.</returns>
        public PluginExecutionResult RunScanActivity()
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Automation Execution Failure", "Device workflow error.");
            try
            {
                var devices = ExecutionData.Assets.OfType<IDeviceInfo>();
                var assetTokens = devices.Select(n => new AssetLockToken(n, ScanOptions.LockTimeouts));

                //If there is no device to run
                if (assetTokens.Count() == 0)
                {
                    //Skip When there are no device to execute on.
                    return new PluginExecutionResult(PluginResult.Skipped, "No Asset available to run.");
                }

                RecordEvent(DeviceWorkflowMarker.DeviceLockBegin);
                ExecutionServices.CriticalSection.Run(assetTokens, selectedToken =>
                    {
                        IDeviceInfo deviceInfo = (selectedToken as AssetLockToken).AssetInfo as IDeviceInfo;
                        DeviceSelected?.Invoke(this, new StatusChangedEventArgs(deviceInfo.AssetId));

                        // Log the device and server used for this activity
                        ScanLog.DeviceId = deviceInfo.AssetId;
                        ExecutionServices.DataLogger.Submit(new ActivityExecutionAssetUsageLog(ExecutionData, deviceInfo));
                        if (_serverName != null)
                        {
                            ExecutionServices.DataLogger.Submit(new ActivityExecutionServerUsageLog(ExecutionData, _serverName));
                        }

                        using (IDevice device = CreateAutomationDevice(deviceInfo))
                        {
                            var retryManager = new PluginRetryManager(ExecutionData, this.UpdateStatus);
                            result = retryManager.Run(() => ExecuteScan(device, deviceInfo));
                        }
                    }
                );

                RecordEvent(DeviceWorkflowMarker.DeviceLockEnd);
            }
            catch (AcquireLockTimeoutException)
            {
                result = new PluginExecutionResult(PluginResult.Skipped, "Could not obtain lock on specified device(s).", "Device unavailable.");
            }
            catch (HoldLockTimeoutException)
            {
                result = new PluginExecutionResult(PluginResult.Error, $"Automation did not complete within {ScanOptions.LockTimeouts.HoldTimeout}.", "Automation timeout exceeded.");
            }
            LogDebug("Scan activity complete.");
            return result;
        }

        /// <summary>
        /// Executes the scan job using the specified device.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="deviceInfo">The device information.</param>
        /// <returns>The result of execution.</returns>
        protected virtual PluginExecutionResult ExecuteScan(IDevice device, IDeviceInfo deviceInfo)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Automation Failure", "Device workflow error.");
            try
            {
                // Make sure the device is in a good state
                var devicePrepManager = DevicePreparationManagerFactory.Create(device);
                devicePrepManager.WorkflowLogger = WorkflowLogger;
                devicePrepManager.InitializeDevice(true);

                // Configure the device (apply settings, load simulator ADF, etc.)
                UpdateStatus("Configuring device...");
                ConfigureDevice(deviceInfo);
                ScanLog.PageCount = (short)ScanOptions.PageCount;
                ScanLog.Sender = ExecutionData.Credential.UserName;

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
                    devicePrepManager.NavigateHome();

                    if (devicePrepManager.SignOutRequired())
                    {
                        devicePrepManager.SignOut();
                    }
                    RecordEvent(DeviceWorkflowMarker.ActivityEnd);
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
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Device communication error.");
                GatherTriageData(device, ex.ToString());
            }
            catch (DeviceInvalidOperationException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Device automation error.");
                GatherTriageData(device, ex.ToString());
            }
            catch (DeviceWorkflowException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Device workflow error.");
                GatherTriageData(device, ex.ToString());
            }
            catch (Exception ex)
            {
                GatherTriageData(device, $"Unexpected exception, gathering triage data: {ex.ToString()}");
                throw;
            }
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
        /// Sets the job end status.
        /// </summary>
        /// <param name="result">The result.</param>
        protected void SetJobEndStatus(PluginExecutionResult result)
        {
            if (result.Result.Equals(PluginResult.Passed))
            {
                ScanLog.JobEndStatus = "Success";
            }
            else if (result.Result.Equals(PluginResult.Skipped))
            {
                ScanLog.JobEndStatus = "Skipped";
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
                //throw new DeviceCommunicationException();
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

        private void ConfigureDevice(IDeviceInfo deviceInfo)
        {
            // If this is a Jedi simulator, load documents into the ADF
            DeviceSimulatorInfo simulatorInfo = deviceInfo as DeviceSimulatorInfo;
            if (simulatorInfo?.SimulatorType == "Jedi" && ScanOptions.UseAdf)
            {
                _adfDocumentsLoaded = true;
                LoadAdf(deviceInfo.Address, ScanOptions.PageCount);
            }
        }

        private void LoadAdf(string address, int pageCount)
        {
            if (!ExecutionData.Documents.Any())
            {
                throw new ArgumentException("Document selection data resulted in 0 documents.");
            }

            // Clear the ADF before we do anything else
            AdfSimulator.Clear(address);

            // Iterate over the documents until we reach the desired page count
            DocumentCollectionIterator iterator = new DocumentCollectionIterator(CollectionSelectorMode.RoundRobin);
            for (int i = 0; i < pageCount; i++)
            {
                // Select and log document(s)
                Document document = iterator.GetNext(ExecutionData.Documents);
                ActivityExecutionDocumentUsageLog documentLog = new ActivityExecutionDocumentUsageLog(ExecutionData, document);
                ExecutionServices.DataLogger.Submit(documentLog);

                // Convert relative paths to absolute network paths
                string sharePath = ExecutionServices.FileRepository.AsInternal().GetDocumentSharePath(document);

                // Load it into the ADF
                AdfSimulator.LoadPage(address, sharePath);
            }
        }

        private void GatherTriageData(IDevice device, string reason)
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
        /// Initializes the authenticator that will be used in the run, including setting up the badgebox if needed.
        /// </summary>
        /// <param name="provider">The authentication provider to create</param>
        /// <param name="device">The device that will be used for the run.</param>
        /// <param name="executionData">This is to pass the credentials for authentication</param>
        protected void InitializeAuthenticator(AuthenticationProvider provider, IDevice device, PluginExecutionData executionData)
        {
            Authenticator = AuthenticatorFactory.Create(ScanLog.DeviceId, device, provider, executionData);
            Authenticator.WorkflowLogger = WorkflowLogger;
        }

        #region Memory Collection



        #endregion
        /// <summary>
        /// Common method to set the setting options for all Scan and Fax plugins
        /// </summary>
        /// <param name="scanOptions"><c>ScanOptions</c> instance having user selection data</param>
        /// <param name="OptionsManager">Runtime instance of the OPtions manager implmenting the settings code</param>
        /// <param name="appType">Runtime instance of Plugin App </param>
        /// <param name="device">Device information used for the run</param>
        protected void SetOptions(ScanOptions scanOptions, Type OptionsManager, Type appType, IDevice device)
        {
            Type property = appType.GetProperty("Options").PropertyType;
            object[] methodData = new object[] { device };
            object instance = Activator.CreateInstance(OptionsManager, methodData);
            object[] parameters = null;
            
            foreach (MethodInfo info in property.GetMethods())
            {
                //Checks if the base class of the OptionManger is of type Jedi Omni
                if (OptionsManager.BaseType.Equals(typeof(HP.ScalableTest.DeviceAutomation.Helpers.JediOmni.JediOmniJobOptionsManager)))
                {
                    parameters = GetSettingParametersJediOmni(scanOptions, info.Name, info.GetParameters().Length);
                }
                else
                {
                    parameters = GetSettingParametersJediWindjammer(scanOptions, info.Name, info.GetParameters().Length);
                }
                
                if (parameters != null)
                {                    
                    info.Invoke(instance, parameters);
                    UpdateStatus(string.Format("Option {0} has been set  ...... ", info.Name));
                }
            }
            UpdateStatus(string.Format("All options setting completed sucessfully ! "));
        }
               
        private object[] GetSettingParametersJediOmni(ScanOptions scanOptions, string settingType, int arguementCount)
        {
            object[] settingParameters = null;
            switch (settingType)
            {
                case "SelectFileType":
                    {
                        if (!scanOptions.FileType.Equals(FileType.DeviceDefault))
                        {
                            settingParameters = new object[] { scanOptions.FileType.GetDescription() };
                        }
                        break;
                    }
                case "SelectResolution":
                case "SelectFaxResolution":
                    {
                        if (!scanOptions.ResolutionType.Equals(ResolutionType.None))
                        {
                            settingParameters = new object[] { scanOptions.ResolutionType };
                        }
                        break;
                    }
                case "SelectOriginalSides":
                    {
                        if (!scanOptions.OriginalSides.Equals(OriginalSides.None))
                        {
                            settingParameters = new object[] { scanOptions.OriginalSides, scanOptions.PageFlipup };
                        }
                        break;
                    }
                case "SelectColorOrBlack":
                case "SetColor":
                    {
                        if (!scanOptions.Color.Equals(ColorType.None))
                        {
                            settingParameters = new object[] { scanOptions.Color };
                        }
                        break;
                    }
                case "SelectOriginalSize":
                case "SetOriginalSize":
                    {
                        if (!scanOptions.OriginalSize.Equals(OriginalSize.None))
                        {
                            settingParameters = new object[] { scanOptions.OriginalSize };
                        }
                        break;
                    }
                case "SelectContentOrientation":
                case "SetOrientation":
                    {
                        if (!scanOptions.ContentOrientationOption.Equals(ContentOrientation.None))
                        {
                            settingParameters = new object[] { scanOptions.ContentOrientationOption };
                        }
                        break;
                    }
                case "SelectOptimizeTextOrPicture":
                    {
                        if (!scanOptions.OptimizeTextorPic.Equals(OptimizeTextPic.None))
                        {
                            settingParameters = new object[] { scanOptions.OptimizeTextorPic };
                        }
                        break;
                    }
                case "SelectCropOption":
                    {
                        if (!scanOptions.Cropping.Equals(Cropping.None))
                        {
                            settingParameters = new object[] { scanOptions.Cropping };
                        }
                        break;
                    }
                case "SelectBlankPageSupress":
                    {
                        if (!scanOptions.BlankPageSupressoption.Equals(BlankPageSupress.None))
                        {
                            settingParameters = new object[] { scanOptions.BlankPageSupressoption };
                        }
                        break;
                    }
                case "CreateMultipleFiles":
                    {
                        settingParameters = new object[] { scanOptions.CreateMultiFile, scanOptions.MaxPageperFile };
                        break;
                    }
                case "EnablePrintNotification":
                    {
                        if (!scanOptions.notificationCondition.Equals(NotifyCondition.NeverNotify))
                        {
                            if (scanOptions.PrintorEmailNotificationMethod == "Print")
                            {
                                settingParameters = new object[] { scanOptions.notificationCondition, scanOptions.IncludeThumbNail };
                            }
                        }
                        break;
                    }
                case "EnableEmailNotification":
                    {
                        if (!scanOptions.notificationCondition.Equals(NotifyCondition.NeverNotify))
                        {
                            if (scanOptions.PrintorEmailNotificationMethod.Equals("Email"))
                            {
                                settingParameters = new object[] { scanOptions.notificationCondition, scanOptions.EmailNotificationText, scanOptions.IncludeThumbNail };
                            }
                        }
                        break;
                    }
                case "SetEraseEdges":
                    {
                        if (scanOptions.SetEraseEdges)
                        {
                            Dictionary<EraseEdgesType, decimal> eraseEdgeList = new Dictionary<EraseEdgesType, decimal>();
                            foreach (var prop in ScanOptions.EraseEdgesValue.GetType().GetProperties())
                            {
                                if (prop.GetValue(scanOptions.EraseEdgesValue, null).ToString() != string.Empty)
                                {
                                    decimal propValue = Convert.ToDecimal(prop.GetValue(scanOptions.EraseEdgesValue, null));
                                    if (propValue >0  && !propValue.Equals(0.00))
                                    {
                                        foreach (EraseEdgesType edge in Enum.GetValues(typeof(EraseEdgesType)))
                                        {
                                            if (string.Equals(edge.ToString(), prop.Name, StringComparison.CurrentCultureIgnoreCase))
                                            {
                                                eraseEdgeList.Add(edge, propValue);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (eraseEdgeList.Count > 0)
                            {
                                settingParameters = new object[] { eraseEdgeList, scanOptions.ApplySameWidth, scanOptions.MirrorFrontSide, ScanOptions.UseInches };
                            }
                        }
                        break;
                    }
                case "SetImageAdjustments":
                    {
                        if (scanOptions.SetImageAdjustment)
                        {
                            //This code is for Scan to Job storage as it does not support Auto Tone
                            if (arguementCount == 4)
                            {
                                settingParameters = new object[] { scanOptions.ImageAdjustSharpness, scanOptions.ImageAdjustDarkness, scanOptions.ImageAdjustContrast, scanOptions.ImageAdjustbackgroundCleanup };
                            }
                            else
                            {
                                settingParameters = new object[] { scanOptions.ImageAdjustSharpness, scanOptions.ImageAdjustDarkness, scanOptions.ImageAdjustContrast, scanOptions.ImageAdjustbackgroundCleanup, scanOptions.AutomaticTone };
                            }
                        }
                        break;
                    }
                case "SelectSigningAndEncrypt":
                    {
                        if (scanOptions.SignOrEncrypt > -1)
                        {
                            bool sign = scanOptions.SignOrEncrypt == 0 || scanOptions.SignOrEncrypt == 2;
                            bool encrypt = scanOptions.SignOrEncrypt == 1 || scanOptions.SignOrEncrypt == 2;
                            settingParameters = new object[] { sign, encrypt };
                        }
                        break;
                    }
                case "SetSides":
                    {
                        if (scanOptions.SetSides)
                        {
                            settingParameters = new object[] { scanOptions.OriginalOneSided, scanOptions.OutputOneSided, scanOptions.OriginalPageflip, scanOptions.OutputPageflip };
                        }
                        break;
                    }
                case "SetScanMode":
                    {
                        settingParameters = new object[] { scanOptions.ScanModes };
                        break;
                    }
                case "SetReduceEnlarge":
                    {
                        settingParameters = new object[] { scanOptions.ReduceEnlargeOptions, scanOptions.IncludeMargin, scanOptions.ZoomSize };
                        break;
                    }
                case "SetPaperSelection":
                    {
                        settingParameters = new object[] { scanOptions.PaperSelectionPaperSize, scanOptions.PaperSelectionPaperType, scanOptions.PaperSelectionPaperTray };
                        break;
                    }
                case "SetBooklet":
                    {
                        if (scanOptions.BookLetFormat)
                        {
                            settingParameters = new object[] { scanOptions.BookLetFormat, scanOptions.BorderOnEachPage };
                        }
                        break;
                    }
                case "SetPagesPerSheet":
                    {
                        if (scanOptions.SetPagesPerSheet)
                        {
                            settingParameters = new object[] { scanOptions.PagesPerSheetElement, scanOptions.PagesPerSheetAddBorder };
                        }
                        break;
                    }
                case "SetEdgeToEdge":
                    {
                        settingParameters = new object[] { scanOptions.EdgeToEdge };
                        break;
                    }
                case "SetCollate":
                    {
                        settingParameters = new object[] { scanOptions.Collate };
                        break;
                    }
            }
            return settingParameters;
        }

        private object[] GetSettingParametersJediWindjammer(ScanOptions scanOptions, string settingType, int arguementCount)
        {
            object[] settingParameters = null;
            switch (settingType)
            {
                case "SelectFileType":
                    {
                        if (!scanOptions.FileType.Equals(FileType.DeviceDefault))
                        {
                            settingParameters = new object[] { scanOptions.FileType.GetDescription() };
                        }
                        break;
                    }
                case "SetColor":
                    {
                        if (!scanOptions.Color.Equals(ColorType.None))
                        {
                            settingParameters = new object[] { scanOptions.Color };
                        }
                        break;
                    }
                case "SelectOptimizeTextOrPicture":
                    {
                        if (!scanOptions.OptimizeTextorPic.Equals(OptimizeTextPic.None))
                        {
                            settingParameters = new object[] { scanOptions.OptimizeTextorPic };
                        }
                        break;
                    }
            }
            return settingParameters;
        }
    }
}
