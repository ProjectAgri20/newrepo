using System;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.NativeApps.Email;
using HP.ScalableTest.Email;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;
using System.Collections.Generic;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ScanToEmail
{
    public class EmailScanManager : ScanActivityManager
    {
        private readonly ScanToEmailData _data;
        IEmailApp _emailApp = null;
        protected override string ScanType => "Email";

        public EmailScanManager(PluginExecutionData executionData)
            : base(executionData)
        {
            _data = executionData.GetMetadata<ScanToEmailData>(ConverterProvider.GetMetadataConverters());
            ScanOptions = _data.ScanOptions;
        }

        public EmailScanManager(PluginExecutionData executionData, string serverName)
            : base(executionData, serverName)
        {
            _data = executionData.GetMetadata<ScanToEmailData>(ConverterProvider.GetMetadataConverters());
            ScanOptions = _data.ScanOptions;
        }

        /// <summary>
        /// Sets up the scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override void SetupJob(IDevice device)
        {
            ScanLog.JobEndStatus = "Failed";
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            UpdateStatus($"Setting up device at address {device.Address} for user {ExecutionData.Credential.UserName}");

            InitializeAuthenticator(_data.AuthProvider, device, ExecutionData);

            // Load the email application
            _emailApp = EmailAppFactory.Create(device);

            // need to add the ability for user to set eager or lazy authentication
            AuthenticationMode am = (_data.ApplicationAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;

            // Special handling for Sirius/Phoenix
            var emailAppWithAddressSource = _emailApp as IEmailAppWithAddressSource;
            if (emailAppWithAddressSource != null)
            {
                emailAppWithAddressSource.AddressSource = _data.AddressSource;
                emailAppWithAddressSource.FromAddress = ExecutionData.Credential.UserName;
            }

            _emailApp.WorkflowLogger = Authenticator.WorkflowLogger = WorkflowLogger;
            _emailApp.Pacekeeper = Authenticator.Pacekeeper = new Pacekeeper(_data.AutomationPause);
            if (_data.UseQuickset)
            {
                if (!_data.LaunchQuicksetFromApp)
                {
                    _emailApp.LaunchFromQuickSet(Authenticator, am, _data.QuickSetName);
                }
                else
                {
                    _emailApp.Launch(Authenticator, am);
                    _emailApp.SelectQuickset(_data.QuickSetName);
                }
                EnterFileName();
            }
            else
            {
                _emailApp.Launch(Authenticator, am);

                // Apply settings from the configuration
                List<string> emailAddresses = new List<string>();
                string emailAddress = string.Empty;
                string[] emailsList = _data.EmailAddress.Split(';');
                foreach (string email in emailsList)
                {
                    emailAddress = email;
                    if (emailAddress != string.Empty)
                    {
                        if (!IsValidEmail(emailAddress))
                        {
                            emailAddress = new EmailBuilder(emailAddress, ExecutionData).ToString();
                        }

                        emailAddresses.Add(emailAddress);
                    }
                }

                _emailApp.EnterToAddress(emailAddresses);
                EnterFileName();

                // Select the appropriate file type
                // emailApp.Options.SelectFileType(_data.UseOcr ? "Searchable PDF (OCR)" : "PDF");
                //Sets the scan job options
                SetOptions(_data.ScanOptions, _emailApp.Options.GetType(), _emailApp.GetType(), device);
            }

            ScanLog.Ocr = _data.UseOcr;

            // Set job build
            _emailApp.Options.SetJobBuildState(UseJobBuild);
        }

        private void EnterFileName()
        {
            string fileName = FilePrefix.ToString().ToLowerInvariant();
            _emailApp.EnterSubject(fileName);
            _emailApp.EnterFileName(fileName);
        }

        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");
            // Start the job           
            try
            {
                ScanExecutionOptions options = new ScanExecutionOptions();
                if (UseJobBuild)
                {
                    options.JobBuildSegments = _data.ScanOptions.PageCount;
                }
                options.ImagePreview = (ImagePreviewOption)_data.ImagePreviewOptions;

                _emailApp = EmailAppFactory.Create(device);

                // Special handling for Sirius/Phoenix
                var emailAppWithAddressSource = _emailApp as IEmailAppWithAddressSource;
                if (emailAppWithAddressSource != null)
                {
                    emailAppWithAddressSource.AddressSource = _data.AddressSource;
                    emailAppWithAddressSource.FromAddress = ExecutionData.Credential.UserName;
                }

                _emailApp.WorkflowLogger = WorkflowLogger;
                _emailApp.Pacekeeper = new Pacekeeper(_data.AutomationPause);
                if (_emailApp.ExecuteJob(options))
                {
                    result = new PluginExecutionResult(PluginResult.Passed);
                    ScanLog.JobEndStatus = "Success";
                }
            }
            finally
            {
                // We got far enough to start the scan job, so submit the log
                ExecutionServices.DataLogger.Submit(ScanLog);
            }
            return result;
        }

        private static bool IsValidEmail(string text)
        {
            return text.Contains("@");
        }

    }
}