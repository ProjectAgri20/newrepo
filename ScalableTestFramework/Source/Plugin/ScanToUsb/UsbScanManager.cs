using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.NativeApps.USB;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;
using System.Collections.Generic;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ScanToUsb
{
    public class UsbScanManager : ScanActivityManager
    {
        private readonly ScanToUsbData _data;
        IUsbApp _usbApp = null;

        protected override string ScanType => "USB";

        public UsbScanManager(PluginExecutionData executionData)
            : base(executionData)
        {
            _data = executionData.GetMetadata<ScanToUsbData>(ConverterProvider.GetMetadataConverters());
            ScanOptions = _data.ScanOptions;
        }

        public UsbScanManager(PluginExecutionData executionData, string serverName)
            : base(executionData, serverName)
        {
            _data = executionData.GetMetadata<ScanToUsbData>(ConverterProvider.GetMetadataConverters());
            ScanOptions = _data.ScanOptions;
        }

        /// <summary>
        /// Sets up the scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override void SetupJob(IDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }

            InitializeAuthenticator(_data.AuthProvider, device, ExecutionData);

            // Update the destination type
            ScanLog.ScanType = _data.UsbName;

            UpdateStatus(string.Format("Setting up device at address {0} for user {1}", device.Address, ExecutionData.Credential.UserName));

            // Load the network folder application

            _usbApp = UsbAppFactory.Create(device);

            _usbApp.WorkflowLogger = Authenticator.WorkflowLogger = WorkflowLogger;
            _usbApp.Pacekeeper = Authenticator.Pacekeeper = new Pacekeeper(_data.AutomationPause);

            // need to add the ability for user to set eager or lazy authentication
            AuthenticationMode am = (_data.ApplicationAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;

            if (device is JediOmniDevice)
            {
                _usbApp.LaunchScanToUsb(Authenticator, am);

                // Apply settings from the configuration
                if (_data.UseQuickset)
                {
                    _usbApp.SelectQuickset(_data.QuickSetName);
                }
                else
                {
                    _usbApp.SelectUsbDevice(_data.UsbName);
                }
            }
            else if (device is JediWindjammerDevice)
            {
                if (!_data.UseQuickset)
                {
                    _usbApp.LaunchScanToUsb(Authenticator, am);
                    _usbApp.SelectUsbDevice(_data.UsbName);
                }
                else
                {
                    _usbApp.LaunchScanToUsbByQuickSet(Authenticator, am, _data.QuickSetName);
                }
            }


            // Enter the file name
            _usbApp.AddJobName(FilePrefix.ToString().ToLowerInvariant());

            // Select the appropriate file type
            //_usbApp.Options.SelectFileType(_data.UseOcr ? "Searchable PDF (OCR)" : "PDF");
            //Sets the scan job options
            SetOptions(_data.ScanOptions, _usbApp.Options.GetType(), _usbApp.GetType(), device);
            ScanLog.Ocr = _data.UseOcr;

            // Set job build
            _usbApp.Options.SetJobBuildState(this.UseJobBuild);
        }

        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");
            try
            {
                ScanExecutionOptions options = new ScanExecutionOptions();
                if (this.UseJobBuild)
                {
                    options.JobBuildSegments = _data.ScanOptions.PageCount;
                }

                _usbApp.Pacekeeper = new Pacekeeper(_data.AutomationPause);
                if (_usbApp.ExecuteScanJob(options))
                {
                    result = new PluginExecutionResult(PluginResult.Passed);
                }

            }
            finally
            {
                // We got far enough to start the scan job, so submit the log
                ExecutionServices.DataLogger.Submit(ScanLog);
            }
            return result;
        }
    }
}
