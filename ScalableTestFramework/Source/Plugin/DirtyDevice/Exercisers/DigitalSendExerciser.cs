using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.Framework.Plugin;
using OXPd.Domain.ServiceDiscovery;
using OXPd.Helpers;
using OXPd.Service.Scan;
using OXPd.Service.Test;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public class DigitalSendExerciser
    {
        public int MaxRetryCount { get; set; } = 3;

        private readonly DirtyDeviceManager _owner;
        private readonly JediOmniDevice _deviceDat;
        private readonly JediWebServices _webServices;
        private JediOmniPreparationManager _preparationManager;
        private EndpointManager _endpointManager;
        protected DirtyDeviceActivityData _activityData;
        protected NetworkCredential _userCredential;
        private string DateTimeStampForNaming;

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSendExerciser" /> class.
        /// </summary>
        /// <param name="device">The <see cref="JediDevice" /> object.</param>
        internal DigitalSendExerciser(DirtyDeviceManager owner, JediOmniDevice deviceDat, JediOmniPreparationManager preparationManager)
        {
            _owner = owner;
            _deviceDat = deviceDat;
            _preparationManager = preparationManager;
            _webServices = _deviceDat.WebServices;
            DateTimeStampForNaming = DateTime.Now.ToString($"yyyyMMdd_HHmmss");
        }

        internal void Exercise(DirtyDeviceActivityData activityData, NetworkCredential userCredential, PluginEnvironment environment)
        {
            _activityData = activityData;
            _userCredential = userCredential;

            string localMachineName = NetUtil.GetFQDN();
            string fileShareServerName = $"{activityData.DigitalSend.OutputFolder.ServerHostName}.{environment.UserDnsDomain}";
            var externalFacingProtocolSpecificPath = activityData.DigitalSend.OutputFolder.MonitorLocation;
            var correspondingSharePath = $@"\\{fileShareServerName}\{externalFacingProtocolSpecificPath.Replace("/", @"\")}";

            EndpointPath fileSharePath = new EndpointPath(
                DestinationType.NetworkFolder,
                fileShareServerName,
                null,
                externalFacingProtocolSpecificPath,
                correspondingSharePath);

            EndpointPath ftpPath = new EndpointPath(
                DestinationType.Ftp,
                localMachineName,
                null,
                externalFacingProtocolSpecificPath,
                correspondingSharePath);

            EndpointPath httpPath = new EndpointPath(
                DestinationType.Http,
                localMachineName,
                EndpointManager.HttpPort,
                externalFacingProtocolSpecificPath,
                correspondingSharePath);

            var pathManager = new PathManager(fileSharePath, ftpPath, httpPath);

            _endpointManager = new EndpointManager(pathManager, environment);
            _endpointManager.UpdateStatus += (s, e) => _owner.OnUpdateStatus(s, e.StatusMessage);

            // eschew "all inspectable pages are in use" exception
            foreach (var protocol in new[] { DestinationType.Ftp, DestinationType.Http, DestinationType.NetworkFolder })
            {
                for (var attempt = 1; attempt <= MaxRetryCount; attempt++)
                {
                    _owner.OnUpdateStatus(this, $"");
                    _preparationManager.InitializeDevice(true);
                    try
                    {
                        ExecuteScanToDestination(protocol);
                        // If we got this far successfully, we are done.  We do not need to retry.
                        break;
                    }
                    catch (Exception x)
                    {
                        _owner.OnUpdateStatus(this, $"  Digital send failed.  (Device: {_deviceDat.Address}; Protocol: {protocol}; Error: {x.Message})");
                        if (attempt >= MaxRetryCount)
                        {
                            throw;
                        }
                        _owner.OnUpdateStatus(this, $"  Will attempt {MaxRetryCount - attempt} more times.");
                    }
                }
            }
            _owner.OnUpdateStatus(this, string.Empty);
            _preparationManager.InitializeDevice(true);
        }

        internal void ExecuteScanToDestination(DestinationType destinationType)
        {
            const int MaxPageCountThresholdForProcessingNotificationError = 2;
            int expectedNumberOfGeneratedFiles = 1;
            int step = 0;
            string uiContextId = "<Missing uiContextId>";
            _owner.OnUpdateStatus(this, $"Step {++step}: Setup {destinationType} scan ticket.");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;

            var tree = DiscoveryTreeTranslator.GetOXPdDiscoveryTree(_deviceDat.Address);
            var scanFactory = new ScanProxyFactory();
            var scanProxy = scanFactory.CreateProxy(tree, _deviceDat.Address, _deviceDat.AdminPassword);
            var uiFactory = new UIConfigurationProxyFactory();
            var uiProxy = uiFactory.CreateProxy(tree, _deviceDat.Address, _deviceDat.AdminPassword);
            var testFactory = new TestProxyFactory();
            var testProxy = testFactory.CreateProxy(tree, _deviceDat.Address, _deviceDat.AdminPassword);

            ScanTicket jobTicket = new ScanTicket();
            jobTicket.transmissionMode = TransmissionMode.Job;
            jobTicket.basicOptions = scanProxy.GetDefaultBasicOptions(jobTicket.transmissionMode);
            jobTicket.basicOptions.mediaSource = MediaSource.Flatbed;
            jobTicket.destination = new OXPd.Common.WebResourceWithTimeoutAndRetry();
            jobTicket.destination.connectionTimeout = 60;
            jobTicket.destination.responseTimeout = 300;
            jobTicket.destination.retryInterval = 1;
            jobTicket.destination.binding = OXPd.Common.Binding.Plain;

            switch (destinationType)
            {
                case DestinationType.Ftp:
                    jobTicket.destination.uri = _endpointManager.FtpServer.EndpointAddress;
                    jobTicket.destination.networkCredentials = new OXPd.Common.NetworkCredentials() { userName = _userCredential.UserName, password = _userCredential.Password };
                    break;
                case DestinationType.Http:
                    jobTicket.destination.uri = _endpointManager.HttpScanReceiver.EndpointAddress;
                    jobTicket.destination.networkCredentials = new OXPd.Common.NetworkCredentials() { userName = "user1", password = "p" };
                    break;
                case DestinationType.NetworkFolder:
                    jobTicket.destination.uri = _endpointManager.NetworkFolder.EndpointAddress;
                    jobTicket.destination.networkCredentials = new OXPd.Common.NetworkCredentials() { domain = _userCredential.Domain, userName = _userCredential.UserName, password = _userCredential.Password };
                    break;
            }

            jobTicket.fileOptions = scanProxy.GetDefaultFileOptions(jobTicket.basicOptions.fileType, jobTicket.basicOptions.colorMode);
            jobTicket.metadata = null;
            jobTicket.scanFileNameBase = DateTimeStampForNaming + "_" + EndpointPath.GetProtocolAsString(destinationType);
            _owner.OnUpdateStatus(this, $"Target Folder as URI (PathManager): {_endpointManager.PathManager.GetEndpointPath(destinationType).GetPathAsUrl()}");
            _owner.OnUpdateStatus(this, $"Target Folder as URI (JobTicket): {jobTicket.destination.uri}");
            _owner.OnUpdateStatus(this, $"Target Folder as Path: {_endpointManager.PathManager.GetEndpointPath(destinationType).CorrespondingFileSystemPath}");
            _owner.OnUpdateStatus(this, $"Target File as Path: {_endpointManager.PathManager.GetEndpointPath(destinationType).CorrespondingFileSystemPath + @"\" + jobTicket.scanFileNameBase + "*.*"}");
            string qualifiedUserName = (jobTicket.destination.networkCredentials.domain == null) ? jobTicket.destination.networkCredentials.userName : $"{jobTicket.destination.networkCredentials.domain}\\{jobTicket.destination.networkCredentials.userName}";
            _owner.OnUpdateStatus(this, $"Credentials: Username = { qualifiedUserName}; Password = { jobTicket.destination.networkCredentials.password}");

            try
            {
                var targetFolder = new DirectoryInfo(_endpointManager.PathManager.GetEndpointPath(destinationType).CorrespondingFileSystemPath);
                int preexistingFileCount = 0;
                string increaseCountMessage = $"This job is expected to create {expectedNumberOfGeneratedFiles} more file(s).";
                if (targetFolder.Exists)
                {
                    _owner.OnUpdateStatus(this, $"Step {++step}: Get pre-existing file count on destination folder.");
                    preexistingFileCount = CurrentFileCount(targetFolder);
                    _owner.OnUpdateStatus(this, $"Found {preexistingFileCount} files. {increaseCountMessage}");
                }
                else
                {
                    _owner.OnUpdateStatus(this, $"Step {++step}: Found {preexistingFileCount} files because destination folder does not exist. {increaseCountMessage}");
                    targetFolder.Create();
                }

                _owner.OnUpdateStatus(this, $"Step {++step}: Reserve UI context.");
                uiContextId = uiProxy.ReserveUIContext(_endpointManager.HttpServerSim.EndpointAddress);
                _owner.OnUpdateStatus(this, $"Step {++step}: Wait for OXPd browser state to be {BrowserState.Idle}.");
                Wait.ForTrue(() => testProxy.GetBrowserState() == BrowserState.Idle, new TimeSpan(0, 0, 30), new TimeSpan(0, 0, 1));
                _owner.OnUpdateStatus(this, $"Step {++step}: Start scan job.");
                string scanJobId = scanProxy.StartScanJob(uiContextId, null, jobTicket);

                _owner.OnUpdateStatus(this, $"Step {++step}: Wait for scan progress window.");
                const string NotificationText = "Processing";
                if (!JediOmniDeviceHelper.WaitForValue(_deviceDat, ".hp-popup-content:visible", "innerHTML", OmniPropertyType.Property, NotificationText, StringMatch.Contains, StringComparison.InvariantCulture, new TimeSpan(0, 0, 5), new TimeSpan(0, 0, 5)))
                {
                    string message = $"Step {step} '{NotificationText}' notification was not detected.";
                    if (expectedNumberOfGeneratedFiles <= MaxPageCountThresholdForProcessingNotificationError)
                    {
                        message += $"  This is probably okay since this is a small scan job.";
                    }
                    else
                    {
                        message = $"Non-fatal error.  {message}  Enough pages are being scanned that this should not happen.";
                    }
                    _owner.OnUpdateStatus(this, message);
                }

                _owner.OnUpdateStatus(this, $"Step {++step}: Verify {nameof(ResultCode)} returned by device scan service.  (Expecting: {ResultCode.Succeeded}).");
                Wait.ForTrue(() => scanProxy.GetScanJobStatus(scanJobId).resultCode == ResultCode.Succeeded, new TimeSpan(0, 0, 30), new TimeSpan(0, 0, 1));

                switch (destinationType)
                {
                    case DestinationType.Http:
                        try
                        {
                            _owner.OnUpdateStatus(this, $"Step {++step}: Verify webserver received {expectedNumberOfGeneratedFiles} requests(s).");
                            _endpointManager.HttpScanReceiver.WaitForRequests(expectedNumberOfGeneratedFiles, new TimeSpan(0, 0, 5 * 60));
                            _owner.OnUpdateStatus(this, $"Step {++step}: Verify webserver requests resulted in {expectedNumberOfGeneratedFiles} uploaded file(s).");
                            var webServerFiles = _endpointManager.HttpScanReceiver.SaveUploadedFiles(targetFolder).ToList();

                            if (webServerFiles.Count() != expectedNumberOfGeneratedFiles)
                            {
                                string paramMessage = $"Expected: {expectedNumberOfGeneratedFiles}; Actual: {webServerFiles.Count}";
                                throw new ActualVsExpectedException($"Web server reports incorrect uploaded file count. {paramMessage}");
                            }

                            _endpointManager.HttpScanReceiver.RequestsReceived.Clear();
                        }
                        catch (ThreadAbortException tax)
                        {
                            _owner.OnUpdateStatus(this, $"{tax.GetType().Name} in Step {step}{Environment.NewLine}{tax.ToString()}.");
                            _owner.OnUpdateStatus(this, $"Will attempt to continue.");
                        }
                        break;
                }

                _owner.OnUpdateStatus(this, $"Step {++step}: Verify that {expectedNumberOfGeneratedFiles} file(s) arrived on server.");
                VerifyFileCount(targetFolder, preexistingFileCount, expectedNumberOfGeneratedFiles, new TimeSpan(0, 5, 0));
            }
            catch (Exception x)
            {
                _owner.OnUpdateStatus(this, $"{x.GetType().Name} in Step {step}{Environment.NewLine}{x.ToString()}");
                throw;
            }
            finally
            {
                try
                {
                    _owner.OnUpdateStatus(this, $"Step {++step}: Release UI Context ID.");
                    uiProxy.ReleaseUIContext(uiContextId);
                }
                catch (Exception idX)
                {
                    _owner.OnUpdateStatus(this, $"Could not release uiContextId: {idX.Message}.");
                }

                _owner.OnUpdateStatus(this, $"Step {++step}: Wait for home screen.");
                JediOmniDeviceHelper.WaitForHome(_deviceDat, new TimeSpan(0, 0, 30));
            }
        }

        public int CurrentFileCount(DirectoryInfo folder)
        {
            var files = folder.GetFiles().Where(file => file.Name != "Thumbs.db");
            return files.Count();
        }

        public void VerifyFileCount(DirectoryInfo folder, int preexistingFileCount, int expectedIncreaseCount, TimeSpan timeout)
        {
            DateTime endTime = DateTime.Now + timeout;
            int totalFileCount = 0;
            int actualGeneratedFileCount = 0;

            while (DateTime.Now < endTime)
            {
                totalFileCount = CurrentFileCount(folder);

                actualGeneratedFileCount = totalFileCount - preexistingFileCount;

                if (actualGeneratedFileCount == expectedIncreaseCount)
                {
                    return;
                }

                Thread.Sleep(500);
            }

            if (totalFileCount < preexistingFileCount)
            {
                throw new ActualVsExpectedException($"File were unexpectedly deleted instead of added. {nameof(folder)}: {folder.FullName}; {nameof(preexistingFileCount)}: {preexistingFileCount}; {nameof(expectedIncreaseCount)}: {expectedIncreaseCount}; {nameof(totalFileCount)}: {totalFileCount}");
            }

            if (expectedIncreaseCount != actualGeneratedFileCount)
            {
                throw new ActualVsExpectedException($"Unexpected file count. {nameof(folder)}: {folder.FullName}; {nameof(preexistingFileCount)}: {preexistingFileCount}; {nameof(expectedIncreaseCount)}: {expectedIncreaseCount}; {nameof(totalFileCount)}: {totalFileCount}");
            }
        }
    }
}
