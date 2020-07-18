using System;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.DeviceAutomation;
using HP.DeviceAutomation;
using static HP.ScalableTest.Framework.Logger;
using HP.ScalableTest.DeviceAutomation.NativeApps.Copy;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.NativeApps.Email;
using System.Collections.Generic;
using HP.ScalableTest.DeviceAutomation.NativeApps.NetworkFolder;
using HP.ScalableTest.DeviceAutomation.NativeApps.USB;
using HP.ScalableTest.DeviceAutomation.NativeApps.JobStorage;
using HP.ScalableTest.DeviceAutomation.NativeApps.Fax;
using HP.ScalableTest.Print;
using System.Printing;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework;
using System.IO;
using System.Linq;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.Plugin.Contention
{
    /// <summary>
    /// Manager class for the Contention plugin.
    /// </summary>
    internal class ContentionManager
    {
        private readonly ContentionData _data;

        /// <summary>
        /// Occurs when the activity status changes.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> ActivityStatusChanged;

        /// <summary>
        /// Gets the <see cref="PluginExecutionData" /> for this activity.
        /// </summary>
        private PluginExecutionData ExecutionData { get; }

        /// <summary>
        /// Gets the <see cref="DeviceWorkflowLogger" /> for this activity.
        /// </summary>
        protected DeviceWorkflowLogger WorkflowLogger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentionManager"/> class.
        /// </summary>
        /// <param name="executionData"></param>
        public ContentionManager(PluginExecutionData executionData)
        {
            if (executionData == null)
            {
                throw new ArgumentNullException(nameof(executionData));
            }

            _data = executionData.GetMetadata<ContentionData>();
            ExecutionData = executionData;
            WorkflowLogger = new DeviceWorkflowLogger(executionData);
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
        /// Performs the Control Panel activity if lock is acquired on the device, otherwise performs the Contention Activity.
        /// </summary>
        /// <returns></returns>
        public PluginExecutionResult RunContentionJob()
        {
            var result = new PluginExecutionResult(PluginResult.Failed);

            try
            {
                IEnumerable<IDeviceInfo> devices = ExecutionData.Assets.OfType<IDeviceInfo>();
                IEnumerable<AssetLockToken> assetTokens = devices.Select(n => new AssetLockToken(n, _data.LockTimeouts));

                RecordEvent(DeviceWorkflowMarker.DeviceLockBegin);
                ExecutionServices.CriticalSection.Run(assetTokens, selectedToken =>
                {
                    IDeviceInfo deviceInfo = (selectedToken as AssetLockToken).AssetInfo as IDeviceInfo;
                    using (IDevice device = DeviceConstructor.Create(deviceInfo))
                    {
                        var retryManager = new PluginRetryManager(ExecutionData, UpdateStatus);
                        result = retryManager.Run(() => RunControlPanelActivity(device));
                    }
                }
                );
                RecordEvent(DeviceWorkflowMarker.DeviceLockEnd);
            }
            catch (AcquireLockTimeoutException)
            {
                //If the user is unable to acquire the lock on device, run the contention activity.
                result = RunContentionActivity();
            }
            catch (HoldLockTimeoutException)
            {
                result = new PluginExecutionResult(PluginResult.Error, $"Automation did not complete within {_data.LockTimeouts.HoldTimeout}.", "Automation timeout exceeded.");
            }
            return result;
        }

        /// <summary>
        /// Selects a control panel activity to run and calls the appropriate method to run it
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        private PluginExecutionResult RunControlPanelActivity(IDevice device)
        {
            UpdateStatus("Running Control Panel Activity...");
            PluginExecutionResult controlPanelActivityResult = new PluginExecutionResult(PluginResult.Failed);

            //Initializing the authenticator
            IAuthenticator authenticator = AuthenticatorFactory.Create(device, ExecutionData.Credential);
            authenticator.WorkflowLogger = WorkflowLogger;

            object controlPanelData = null;
            //Selecting a random control Panel activity.
            Type ControlPanelActivityToRun = SelectRandomActivity(_data.SelectedControlPanelActivities);

            Attribute[] attributes = Attribute.GetCustomAttributes(ControlPanelActivityToRun);
            foreach (Attribute attr in attributes)
            {
                if (attr is ControlPanelActivity)
                {
                    ControlPanelActivity controlPanelAttribute = (ControlPanelActivity)attr;
                    switch (controlPanelAttribute.ActivityName)
                    {
                        case "Copy":
                            //Copy activity is selected
                            UpdateStatus("Selected Control Panel Activity: Copy");
                            controlPanelData = _data.SelectedControlPanelActivities.OfType<CopyActivityData>().Single();
                            controlPanelActivityResult = ExecuteCopy(device, controlPanelData, authenticator);
                            break;

                        case "Scan":
                            //Scan activity is selected
                            controlPanelData = _data.SelectedControlPanelActivities.OfType<ScanActivityData>().Single();
                            controlPanelActivityResult = ExecuteScan(device, controlPanelData, authenticator);
                            break;

                        case "Fax":
                            //Fax activity is selected
                            UpdateStatus("Selected Control Panel Activity: Fax");
                            controlPanelData = _data.SelectedControlPanelActivities.OfType<FaxActivityData>().Single();
                            controlPanelActivityResult = ExecuteFax(device, controlPanelData, authenticator);
                            break;
                    }
                }
            }

            return controlPanelActivityResult;
        }

        /// <summary>
        /// Runs an activity in contention with the Control Panel activity
        /// </summary>
        /// <returns></returns>
        private PluginExecutionResult RunContentionActivity()
        {
            UpdateStatus("Running Contention Activity...");
            var contentionActivityResult = new PluginExecutionResult(PluginResult.Failed);

            object contentionActivityData = null;
            //Selecting a random contention activity.
            Type ContentionActivityToRun = SelectRandomActivity(_data.SelectedContentionActivities);

            Attribute[] attributes = Attribute.GetCustomAttributes(ContentionActivityToRun);
            foreach (Attribute attr in attributes)
            {
                if (attr is ContentionActivity)
                {
                    ContentionActivity contentionAttribute = (ContentionActivity)attr;
                    switch (contentionAttribute.ActivityName)
                    {
                        case "Print":
                            //Print activity is selected
                            UpdateStatus("Selected Contention Activity: Print");
                            contentionActivityData = _data.SelectedContentionActivities.OfType<PrintActivityData>().Single();
                            contentionActivityResult = ExecutePrintJob();
                            break;
                    }
                }
            }

            return contentionActivityResult;
        }

        /// <summary>
        /// Performs Copy job on Control Panel
        /// </summary>
        /// <param name="device"></param>
        /// <param name="controlPanelData"></param>
        /// <returns></returns>
        private PluginExecutionResult ExecuteCopy(IDevice device, object controlPanelData, IAuthenticator authenticator)
        {
            var result = new PluginExecutionResult(PluginResult.Failed);

            CopyActivityData copyData = controlPanelData as CopyActivityData;
            // Make sure the device is in a good state
            UpdateStatus($"Setting up device at address {device.Address} for user {ExecutionData.Credential.UserName}");
            var devicePrepManager = DevicePreparationManagerFactory.Create(device);
            devicePrepManager.WorkflowLogger = WorkflowLogger;
            devicePrepManager.InitializeDevice(true);

            // Load the copy application            
            ICopyApp contentionCopyApp = CopyAppFactory.Create(device);

            //Launch the Copy application
            UpdateStatus("Copy Activity: Launching the Copy application...");
            contentionCopyApp.Launch(authenticator, AuthenticationMode.Lazy);

            //set number of copies
            contentionCopyApp.Options.SetNumCopies(copyData.Copies);
            UpdateStatus("Copy Activity: Number of Copies has been set...");

            try
            {
                ScanExecutionOptions options = new ScanExecutionOptions();
                options.ValidateJobExecution = false;
                if (copyData.PageCount > 1)
                {
                    options.JobBuildSegments = copyData.PageCount;
                }

                //Finish the job
                UpdateStatus("Copy Activity: Finishing the activity...");
                if (contentionCopyApp.ExecuteJob(options))
                {
                    result = new PluginExecutionResult(PluginResult.Passed);
                }

                // Clean up
                try
                {
                    devicePrepManager.NavigateHome();
                    if (devicePrepManager.SignOutRequired())
                    {
                        UpdateStatus("Copy Activity: Signing Out...");
                        devicePrepManager.SignOut();
                    }
                    UpdateStatus("Copy Activity: Activity finished");
                }
                catch (Exception ex) when (ex is DeviceCommunicationException || ex is DeviceInvalidOperationException)
                {
                    // Don't fail the activity if there is an exception here.
                    ExecutionServices.SystemTrace.LogWarn($"Device could not return to home screen: {ex.ToString()}");
                }
            }
            finally
            {
                // End of Copy activity
                ExecutionServices.SystemTrace.LogDebug("Copy Activity Completed");
            }

            return result;
        }

        /// <summary>
        /// Performs Scan job on Control Panel
        /// </summary>
        /// <param name="device"></param>
        /// <param name="controlPanelData"></param>
        /// <returns></returns>
        private PluginExecutionResult ExecuteScan(IDevice device, object controlPanelData, IAuthenticator authenticator)
        {
            var result = new PluginExecutionResult(PluginResult.Failed);

            ScanActivityData scanData = controlPanelData as ScanActivityData;
            switch (scanData.ScanJobType)
            {
                case ContentionScanActivityTypes.Email:
                    UpdateStatus("Selected Control Panel Activity: Scan To Email");
                    result = ExecuteEmailActivity(device, scanData, authenticator);
                    break;
                case ContentionScanActivityTypes.Folder:
                    UpdateStatus("Selected Control Panel Activity: Scan To Folder");
                    result = ExecuteFolderActivity(device, scanData, authenticator);
                    break;
                case ContentionScanActivityTypes.JobStorage:
                    UpdateStatus("Selected Control Panel Activity: Scan To Job Storage");
                    result = ExecuteJobStorageActivity(device, scanData, authenticator);
                    break;
                case ContentionScanActivityTypes.USB:
                    UpdateStatus("Selected Control Panel Activity: Scan To USB");
                    result = ExecuteUsbActivity(device, scanData, authenticator);
                    break;
            }
            return result;
        }

        /// <summary>
        /// Performs ScanToEmail job on Control Panel
        /// </summary>
        /// <param name="device"></param>
        /// <param name="emailScanData"></param>
        /// <returns></returns>
        private PluginExecutionResult ExecuteEmailActivity(IDevice device, ScanActivityData emailScanData, IAuthenticator authenticator)
        {
            var result = new PluginExecutionResult(PluginResult.Failed);

            // Make sure the device is in a good state
            UpdateStatus($"Setting up device at address {device.Address} for user {ExecutionData.Credential.UserName}");
            var devicePrepManager = DevicePreparationManagerFactory.Create(device);
            devicePrepManager.WorkflowLogger = WorkflowLogger;
            devicePrepManager.InitializeDevice(true);

            // Load the email application
            IEmailApp contentionEmailApp = EmailAppFactory.Create(device);

            //Launch the Scan to Email application
            UpdateStatus("ScanToEmail Activity: Launching the Scan To Email application...");
            contentionEmailApp.Launch(authenticator, AuthenticationMode.Lazy);

            //Enter subject and file name
            ScanFilePrefix FilePrefix = new ScanFilePrefix(ExecutionData.SessionId, ExecutionData.Credential.UserName, "Email");
            string fileName = FilePrefix.ToString().ToLowerInvariant();
            contentionEmailApp.EnterSubject(fileName);
            UpdateStatus("ScanToEmail Activity: Email subject entered...");
            contentionEmailApp.EnterFileName(fileName);
            UpdateStatus("ScanToEmail Activity: File name entered...");

            List<string> emailList = new List<string>();
            emailList.Add(emailScanData.EmailAddress);
            //Enter email address
            contentionEmailApp.EnterToAddress(emailList);
            UpdateStatus("ScanToEmail Activity: Email address entered...");

            try
            {
                ScanExecutionOptions options = new ScanExecutionOptions();
                options.ValidateJobExecution = false;
                if (emailScanData.PageCount > 1)
                {
                    options.JobBuildSegments = emailScanData.PageCount;
                }

                //Finish the job
                UpdateStatus("ScanToEmail Activity: Finishing the activity...");
                if (contentionEmailApp.ExecuteJob(options))
                {
                    result = new PluginExecutionResult(PluginResult.Passed);
                }

                // Clean up
                try
                {
                    devicePrepManager.NavigateHome();
                    if (devicePrepManager.SignOutRequired())
                    {
                        UpdateStatus("ScanToEmail Activity: Signing Out...");
                        devicePrepManager.SignOut();
                    }
                    UpdateStatus("ScanToEmail Activity: Activity finished");
                }
                catch (Exception ex) when (ex is DeviceCommunicationException || ex is DeviceInvalidOperationException)
                {
                    // Don't fail the activity if there is an exception here.
                    ExecutionServices.SystemTrace.LogWarn($"Device could not return to home screen: {ex.ToString()}");
                }
            }
            finally
            {
                // End of ScanToEmail activity
                ExecutionServices.SystemTrace.LogDebug("ScanToEmail activity completed");
            }

            return result;
        }

        /// <summary>
        /// Performs ScanToFolder job on Control Panel
        /// </summary>
        /// <param name="device"></param>
        /// <param name="folderScanData"></param>
        /// <returns></returns>
        private PluginExecutionResult ExecuteFolderActivity(IDevice device, ScanActivityData folderScanData, IAuthenticator authenticator)
        {
            var result = new PluginExecutionResult(PluginResult.Failed);

            // Make sure the device is in a good state
            UpdateStatus($"Setting up device at address {device.Address} for user {ExecutionData.Credential.UserName}");
            var devicePrepManager = DevicePreparationManagerFactory.Create(device);
            devicePrepManager.WorkflowLogger = WorkflowLogger;
            devicePrepManager.InitializeDevice(true);

            // Load the network folder application
            INetworkFolderApp contentionFolderApp = NetworkFolderAppFactory.Create(device);

            //Launch the Scan to Network Folder app
            UpdateStatus("ScanToFolder Activity: Launching the Scan To Folder application...");
            contentionFolderApp.Launch(authenticator, AuthenticationMode.Lazy);

            //Network credential being passed as parameter to access the folder and it is used by jediomninetworkfolderapp now
            UpdateStatus("ScanToFolder Activity: Entering folder path...");
            contentionFolderApp.AddFolderPath(folderScanData.FolderPath, ExecutionData.Credential, true);

            // Enter the file name
            ScanFilePrefix FilePrefix = new ScanFilePrefix(ExecutionData.SessionId, ExecutionData.Credential.UserName, "Folder");
            UpdateStatus("ScanToFolder Activity: Entering file name...");
            contentionFolderApp.EnterFileName(FilePrefix.ToString().ToLowerInvariant());

            // Set job build
            contentionFolderApp.Options.SetJobBuildState((folderScanData.PageCount > 1) ? true : false);

            try
            {
                ScanExecutionOptions options = new ScanExecutionOptions();
                options.ValidateJobExecution = false;
                if (folderScanData.PageCount > 1)
                {
                    options.JobBuildSegments = folderScanData.PageCount;
                }

                //Finish the job
                UpdateStatus("ScanToFolder Activity: Finishing the activity...");
                if (contentionFolderApp.ExecuteJob(options))
                {
                    result = new PluginExecutionResult(PluginResult.Passed);
                }
                else
                {
                    throw new DeviceWorkflowException(result.Message);
                }

                // Clean up
                try
                {
                    devicePrepManager.NavigateHome();
                    if (devicePrepManager.SignOutRequired())
                    {
                        UpdateStatus("ScanToFolder Activity: Signing Out...");
                        devicePrepManager.SignOut();
                    }
                    UpdateStatus("ScanToFolder Activity: Activity finished");
                }
                catch (Exception ex) when (ex is DeviceCommunicationException || ex is DeviceInvalidOperationException)
                {
                    // Don't fail the activity if there is an exception here.
                    ExecutionServices.SystemTrace.LogWarn($"Device could not return to home screen: {ex.ToString()}");
                }
            }
            finally
            {
                // End of ScanToFolder activity
                ExecutionServices.SystemTrace.LogDebug("ScanToFolder activity completed");
            }

            return result;
        }

        /// <summary>
        /// Performs ScanToUsb job on Control Panel
        /// </summary>
        /// <param name="device"></param>
        /// <param name="usbScanData"></param>
        /// <returns></returns>
        private PluginExecutionResult ExecuteUsbActivity(IDevice device, ScanActivityData usbScanData, IAuthenticator authenticator)
        {
            var result = new PluginExecutionResult(PluginResult.Failed);

            // Make sure the device is in a good state
            UpdateStatus($"Setting up device at address {device.Address} for user {ExecutionData.Credential.UserName}");
            var devicePrepManager = DevicePreparationManagerFactory.Create(device);
            devicePrepManager.WorkflowLogger = WorkflowLogger;
            devicePrepManager.InitializeDevice(true);

            // Load the network folder application
            IUsbApp contentionUsbApp = UsbAppFactory.Create(device);

            //Launch the Scan To Job Storage application
            UpdateStatus("ScanToUSB Activity: Launching the Scan To USB application...");
            contentionUsbApp.LaunchScanToUsb(authenticator, AuthenticationMode.Lazy);

            //Select the USB device
            UpdateStatus("ScanToUSB Activity: Selecting the USB device...");
            contentionUsbApp.SelectUsbDevice(usbScanData.UsbName);

            // Enter the file name
            ScanFilePrefix FilePrefix = new ScanFilePrefix(ExecutionData.SessionId, ExecutionData.Credential.UserName, "USB");
            UpdateStatus("ScanToUSB Activity: Entering file name...");
            contentionUsbApp.AddJobName(FilePrefix.ToString().ToLowerInvariant());

            // Set job build
            contentionUsbApp.Options.SetJobBuildState((usbScanData.PageCount > 1) ? true : false);

            try
            {
                ScanExecutionOptions options = new ScanExecutionOptions();
                options.ValidateJobExecution = false;
                if (usbScanData.PageCount > 1)
                {
                    options.JobBuildSegments = usbScanData.PageCount;
                }

                //Finish the job
                UpdateStatus("ScanToUSB Activity: Finishing the activity...");
                if (contentionUsbApp.ExecuteScanJob(options))
                {
                    result = new PluginExecutionResult(PluginResult.Passed);
                }

                // Clean up
                try
                {
                    devicePrepManager.NavigateHome();
                    if (devicePrepManager.SignOutRequired())
                    {
                        UpdateStatus("ScanToUSB Activity: Signing Out...");
                        devicePrepManager.SignOut();
                    }
                    UpdateStatus("ScanToUSB Activity: Activity finished");
                }
                catch (Exception ex) when (ex is DeviceCommunicationException || ex is DeviceInvalidOperationException)
                {
                    // Don't fail the activity if there is an exception here.
                    ExecutionServices.SystemTrace.LogWarn($"Device could not return to home screen: {ex.ToString()}");
                }

            }
            finally
            {
                // End of ScanToUSB activity
                ExecutionServices.SystemTrace.LogDebug("ScanToUSB activity completed");
            }

            return result;
        }

        /// <summary>
        /// Performs ScanToJobStorage job on Control Panel
        /// </summary>
        /// <param name="device"></param>
        /// <param name="jobStorageScanData"></param>
        /// <returns></returns>
        private PluginExecutionResult ExecuteJobStorageActivity(IDevice device, ScanActivityData jobStorageScanData, IAuthenticator authenticator)
        {
            var result = new PluginExecutionResult(PluginResult.Failed);

            // Make sure the device is in a good state
            UpdateStatus($"Setting up device at address {device.Address} for user {ExecutionData.Credential.UserName}");
            var devicePrepManager = DevicePreparationManagerFactory.Create(device);
            devicePrepManager.WorkflowLogger = WorkflowLogger;
            devicePrepManager.InitializeDevice(true);

            // Load the Job storage application
            IJobStorageScanApp contentionJobStorageApp = ScanJobStorageAppFactory.Create(device);

            //Launch the Scan To Job Storage application
            UpdateStatus("ScanToJobStorage Activity: Launching the Scan To Job Storage application...");
            contentionJobStorageApp.Launch(authenticator, AuthenticationMode.Lazy);

            //Enter the job name
            ScanFilePrefix FilePrefix = new ScanFilePrefix(ExecutionData.SessionId, ExecutionData.Credential.UserName, "Job Storage");
            FilePrefix.MaxLength = 16;
            UpdateStatus("ScanToJobStorage Activity: Entering job name...");
            contentionJobStorageApp.AddJobName(FilePrefix.ToString().ToLowerInvariant());

            // Set job build
            contentionJobStorageApp.Options.SetJobBuildState((jobStorageScanData.PageCount > 1) ? true : false);

            try
            {
                ScanExecutionOptions options = new ScanExecutionOptions();
                options.ValidateJobExecution = false;
                if (jobStorageScanData.PageCount > 1)
                {
                    options.JobBuildSegments = jobStorageScanData.PageCount;
                }

                //Finish the job
                UpdateStatus("ScanToJobStorage Activity: Finishing the activity...");
                if (contentionJobStorageApp.ExecuteScanJob(options))
                {
                    result = new PluginExecutionResult(PluginResult.Passed);
                }

                // Clean up
                try
                {
                    devicePrepManager.NavigateHome();
                    if (devicePrepManager.SignOutRequired())
                    {
                        UpdateStatus("ScanToJobStorage Activity: Signing Out...");
                        devicePrepManager.SignOut();
                    }
                    UpdateStatus("ScanToJobStorage Activity: Activity finished");
                }
                catch (Exception ex) when (ex is DeviceCommunicationException || ex is DeviceInvalidOperationException)
                {
                    // Don't fail the activity if there is an exception here.
                    ExecutionServices.SystemTrace.LogWarn($"Device could not return to home screen: {ex.ToString()}");
                }
            }
            finally
            {
                // End of ScanToJobStorage activity
                ExecutionServices.SystemTrace.LogDebug("ScanToJobStorage activity completed");
            }

            return result;
        }

        /// <summary>
        /// Performs Fax job on Control Panel
        /// </summary>
        /// <param name="device"></param>
        /// <param name="controlPanelData"></param>
        /// <returns></returns>
        private PluginExecutionResult ExecuteFax(IDevice device, object controlPanelData, IAuthenticator authenticator)
        {
            var result = new PluginExecutionResult(PluginResult.Failed);

            FaxActivityData faxData = controlPanelData as FaxActivityData;

            // Make sure the device is in a good state
            UpdateStatus($"Setting up device at address {device.Address} for user {ExecutionData.Credential.UserName}");
            var devicePrepManager = DevicePreparationManagerFactory.Create(device);
            devicePrepManager.WorkflowLogger = WorkflowLogger;
            devicePrepManager.InitializeDevice(true);

            // Load the fax application
            IFaxApp contentionFaxApp = FaxAppFactory.Create(device);

            //Launch the Fax application
            UpdateStatus("Fax Activity: Launching the Fax application...");
            contentionFaxApp.Launch(authenticator, AuthenticationMode.Lazy);

            ScanFilePrefix FilePrefix = new ScanFilePrefix(ExecutionData.SessionId, ExecutionData.Credential.UserName, "Fax");

            UpdateStatus("Fax Activity: Entering recipient fax number...");
            if (string.IsNullOrEmpty(faxData.FaxNumber) || string.IsNullOrWhiteSpace(faxData.FaxNumber))
            {
                // Apply settings from configuration
                contentionFaxApp.AddRecipient(FilePrefix.ToFaxCode());
            }
            else
            {
                contentionFaxApp.AddRecipient(faxData.FaxNumber);
            }

            //Set job build
            contentionFaxApp.Options.SetJobBuildState((faxData.PageCount > 1) ? true : false);

            try
            {
                // Start the job
                ScanExecutionOptions options = new ScanExecutionOptions();
                options.ValidateJobExecution = false;
                if (faxData.PageCount > 1)
                {
                    options.JobBuildSegments = faxData.PageCount;
                }

                //Finish the job
                UpdateStatus("Fax Activity: Finishing the activity...");
                if (contentionFaxApp.ExecuteJob(options))
                {
                    result = new PluginExecutionResult(PluginResult.Passed);
                }

                // Clean up
                try
                {
                    devicePrepManager.NavigateHome();
                    if (devicePrepManager.SignOutRequired())
                    {
                        UpdateStatus("Fax Activity: Signing Out...");
                        devicePrepManager.SignOut();
                    }
                }
                catch (Exception ex) when (ex is DeviceCommunicationException || ex is DeviceInvalidOperationException)
                {
                    // Don't fail the activity if there is an exception here.
                    ExecutionServices.SystemTrace.LogWarn($"Device could not return to home screen: {ex.ToString()}");
                }
            }
            finally
            {
                // End of fax activity
                ExecutionServices.SystemTrace.LogDebug("Fax activity completed");
            }

            return result;
        }

        /// <summary>
        /// Exexutes the Print Job
        /// </summary>
        /// <returns></returns>
        private PluginExecutionResult ExecutePrintJob()
        {
            PluginExecutionResult printResult = new PluginExecutionResult(PluginResult.Passed);
            try
            {
                PrintQueue defaultPrintQueue;
                PrintQueueInfo printQueueInfo = ExecutionData.PrintQueues.GetRandom();
                UpdateStatus("Print Activity: Retrieving print queue for " + printQueueInfo.QueueName);
                defaultPrintQueue = PrintQueueController.Connect(printQueueInfo);
                PrintingEngine engine = new PrintingEngine();

                // Select a documents to print
                DocumentCollectionIterator documentIterator = new DocumentCollectionIterator(CollectionSelectorMode.ShuffledRoundRobin);
                Document document = documentIterator.GetNext(ExecutionData.Documents);

                // Download the document and log the starting information for the print job
                Guid jobId = SequentialGuid.NewGuid();
                FileInfo localFile = ExecutionServices.FileRepository.GetFile(document);

                UpdateStatus($"Print Activity: Printing {localFile.Name} to {defaultPrintQueue.FullName}");
                var result = engine.Print(localFile, defaultPrintQueue);
                UpdateStatus($"Print Activity: Finished printing {localFile.Name}");

                return printResult;
            }
            catch (Exception genericException)
            {
                printResult = new PluginExecutionResult(PluginResult.Failed, genericException.ToString());
                ExecutionServices.SystemTrace.LogError(genericException.ToString());
                return printResult;
            }
        }

        private Type SelectRandomActivity<T>(List<T> selectedActivities)
        {
            int selectedActivityIndex = (selectedActivities.Count > 1) ? (new Random().Next(0, selectedActivities.Count)) : selectedActivities.Count - 1;
            var activityToRun = selectedActivities[selectedActivityIndex].GetType();
            return activityToRun;
        }

        protected void UpdateStatus(string status)
        {
            LogInfo(status);
            ActivityStatusChanged?.Invoke(this, new StatusChangedEventArgs(status));
        }
    }
}
