﻿using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceMemory.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData;
using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Plugin.Kiosk.Controls;
using HP.ScalableTest.Plugin.Kiosk.Options;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.Utility;
using System;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.Kiosk
{
    public class KioskCopyManager : LinkScanActivityManager
    {
        private readonly KioskActivityData _data;
        private readonly KioskCopyOptions _kioskCopyOptions;
        KioskApp _kioskApp = null;

        protected override string LinkJobType => KioskJobType.Copy.GetDescription();

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudConnectorPrintManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="printOptions">The scan options.</param>
        public KioskCopyManager(PluginExecutionData executionData, KioskCopyOptions copyOptions, LockTimeoutData lockTimeoutData)
                    : base(executionData, lockTimeoutData)
        {
            _data = executionData.GetMetadata<KioskActivityData>();
            _kioskCopyOptions = _data.CopyOptions;
        }

        /// <summary>
        /// Executes the scan job using the specified device.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="deviceInfo">The device information.</param>
        /// <returns>The result of execution.</returns>
        protected override PluginExecutionResult ExecuteLinkScan(IDevice device, IDeviceInfo deviceInfo)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Automation Failure", "Device workflow error.");
            try
            {
                // Make sure the device is in a good state
                var devicePrepManager = DevicePreparationManagerFactory.Create(device);
                devicePrepManager.WorkflowLogger = WorkflowLogger;
                //devicePrepManager.InitializeDevice();

                // Set up the job (enter parameters, etc.)
                RecordEvent(DeviceWorkflowMarker.ActivityBegin);
                UpdateStatus("Setting up job...");

                _kioskApp = new KioskApp(device);
                _kioskApp.WorkflowLogger = WorkflowLogger;                
                _kioskApp.DeviceInfo = deviceInfo;

                SetupJob(device);
                UpdateStatus("Job setup complete.");

                // Finish the job (apply job build options, press start, wait for finish)
                UpdateStatus("Finishing job...");
                result = FinishJob(device);
                UpdateStatus("Job finished.");

                // Clean up
                try
                {
                    RecordEvent(DeviceWorkflowMarker.ActivityEnd);

                    try
                    {
                        CollectJetAdvantagelinkMemoryMonitoring(deviceInfo);
                    }
                    catch (Exception ex)
                    {
                        UpdateStatus(ex.ToString());
                        UpdateStatus(_kioskApp.GetTotalPrice());
                        SubmitConnectorLog(result.Result.ToString());
                        return result;
                    }
                }
                catch (Exception ex) when (ex is DeviceCommunicationException || ex is DeviceInvalidOperationException)
                {
                    // Don't fail the activity if there is an exception here.
                    UpdateStatus("Device could not return to home screen.");
                    GatherTriageData(device, $"Device could not return to home screen: {ex.ToString()}");
                }
                UpdateStatus("Activity finished.");
            }
            catch (DeviceCommunicationException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Device communication error.");

            }
            catch (DeviceInvalidOperationException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Device automation error.");

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
        /// Sets up the cloud print job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override void SetupJob(IDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }

            UpdateStatus($"Starting Kiosk plugin: {_data.JobType} with {_data.AuthType}");

            _kioskApp.KioskInitialize();

            switch (_data.AuthType)
            {
                case KioskAuthType.Login:
                    _kioskApp.KioskLoginAuthenticate(_data.ID, _data.Password);
                    break;
                case KioskAuthType.Card:
                    _kioskApp.KioskCardAuthenticate(ExecutionData.Assets, ExecutionData.Credential);
                    break;
                default:
                    DeviceWorkflowException e = new DeviceWorkflowException($"Kiosk Auth Type is invalid: {_data.AuthType}");
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignIn.GetDescription());
                    throw e;
            }

            _kioskApp.Launch(_data.JobType, _data.CopyOptions);

            SetOptions(_kioskApp.KioskOptionManager);
        }

        /// <summary>
        /// Set options for the copy job.
        /// </summary>                
        protected void SetOptions(KioskOptionsManager kioskOptionsManager)
        {
            try
            {
                UpdateStatus($"Select option activity with Color Mode {_kioskCopyOptions.ColorMode.GetDescription()} is being started");
                kioskOptionsManager.SetColorMode(_kioskCopyOptions.ColorMode);
                UpdateStatus($"Select option activity with Color Mode {_kioskCopyOptions.ColorMode.GetDescription()} is being completed");

                UpdateStatus($"Select option activity with Original Size {_kioskCopyOptions.OriginalSize.GetDescription()} is being started");
                kioskOptionsManager.SetOriginalSize(_kioskCopyOptions.OriginalSize);
                UpdateStatus($"Select option activity with Original Size {_kioskCopyOptions.OriginalSize.GetDescription()} is being completed");

                UpdateStatus($"Select option activity with N-Up {_kioskCopyOptions.Nup.GetDescription()} is being started");
                kioskOptionsManager.SetNUp(_kioskCopyOptions.Nup);
                UpdateStatus($"Select option activity with N-Up {_kioskCopyOptions.Nup.GetDescription()} is being completed");

                UpdateStatus($"Select option activity with Original Orientation {_kioskCopyOptions.OriginalOrientation.GetDescription()} is being started");
                kioskOptionsManager.SetOriginalOrientation(_kioskCopyOptions.OriginalOrientation);
                UpdateStatus($"Select option activity with Original Orientation {_kioskCopyOptions.OriginalOrientation.GetDescription()} is being completed");

                UpdateStatus($"Select option activity with Duplex Original {_kioskCopyOptions.DuplexOriginal.GetDescription()} is being started");
                kioskOptionsManager.SetDuplexOriginal(_kioskCopyOptions.DuplexOriginal);
                UpdateStatus($"Select option activity with Duplex Original {_kioskCopyOptions.DuplexOriginal.GetDescription()} is being completed");

                UpdateStatus($"Select option activity with Duplex Output {_kioskCopyOptions.DuplexOutput.GetDescription()} is being started");
                kioskOptionsManager.SetDuplexOutput(_kioskCopyOptions.DuplexOutput);
                UpdateStatus($"Select option activity with Duplex Output {_kioskCopyOptions.DuplexOutput.GetDescription()} is being completed");

                UpdateStatus($"Select option activity with ReduceEnlarge index is {_kioskCopyOptions.ReduceEnlargeIndex} is being started");
                kioskOptionsManager.SetReduceEnlarge(_kioskCopyOptions.ReduceEnlargeIndex);
                UpdateStatus($"Select option activity with ReduceEnlarge {_kioskCopyOptions.ReduceEnlargeIndex} is being completed");

                UpdateStatus($"Select option activity with Page Count {_kioskCopyOptions.PageCount} is being started");
                kioskOptionsManager.SetCopyCopies(_kioskCopyOptions.PageCount);
                UpdateStatus($"Select option activity with Page Count {_kioskCopyOptions.PageCount} is being completed");
            }
            catch(DeviceWorkflowException ex)
            {
                DeviceWorkflowException e = new DeviceWorkflowException(ex.Message + $" :: {_data.JobType.GetDescription()}", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SelectOptions.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Finish up the Cloud Connector print job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");
            
            UpdateStatus($"Starting Execution Copy Job :: Kiosk");
            _kioskApp.ExecutionJob(_data.CopyOptions);

            result = new PluginExecutionResult(PluginResult.Passed, $"Job is completed with successful condition :: {_data.JobType}");

            return result;            
        }

        /// <summary>
        /// Navigates to the home screen.
        /// </summary>
        protected override void SignOut()
        {
            if(_kioskApp != null)
            {
                _kioskApp.SignOut();
            }            
        }

        /// <summary>
        /// Dispose Cloud Connector
        /// </summary>
        public override void Dispose()
        {
            if(_kioskApp != null)
            {
                _kioskApp.Dispose();
            }            
        }

        /// <summary>
        /// Collecct and Submit JetAdvantageLink MemoryMonitoring.
        /// </summary>        
        protected override void CollectJetAdvantagelinkMemoryMonitoring(IDeviceInfo deviceInfo)
        {
            if (deviceInfo == null)
            {
                throw new ArgumentNullException(nameof(deviceInfo));
            }

            JetAdvantageLinkMemoryMonitoring jalMem = new JetAdvantageLinkMemoryMonitoring(_kioskApp.LinkUI, _kioskApp.KioskPackageName, ExecutionData, deviceInfo);
            jalMem.CollectMemoryMonitoringData();
            jalMem.Submit();
        }

        /// <summary>
        /// Submit TrageData to DB
        /// </summary>
        /// <param name="device"></param>
        /// <param name="reason"></param>    
        protected override void GatherTriageData(IDevice device, string reason)
        {            
            try
            {
                if (_kioskApp != null)
                {                    
                    JetAdvantageLinkTriage triage = new JetAdvantageLinkTriage(_kioskApp.Device, _kioskApp.LinkUI, ExecutionData);
                    triage.CollectTriageData(reason);                    
                    triage.Submit();                    
                }
                else
                {
                    UpdateStatus("Device is null - cannot gather triage data.");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"GatherTriageData failed - cannot gather triage data. {ex.ToString()}");
            }
            finally
            {
                if(_kioskApp != null)
                {
                    UpdateStatus($"Dispose CloudApp on GatherTriageData");
                    _kioskApp.Dispose();
                }                
            }
        }

        /// <summary>
        /// Submits the ConnectorJobInputLog.
        /// </summary>
        /// <param name="result"></param>        
        protected override void SubmitConnectorLog(string result)
        {
            if(ConnectorLog != null)
            {
                ConnectorLog.AppName = "Kiosk";
                ConnectorLog.JobEndStatus = result;
                ConnectorLog.OptionsData = Serializer.Serialize(_kioskCopyOptions).ToString();

                if(_kioskApp != null)
                {
                    if (!string.IsNullOrEmpty(_kioskApp.GetTotalPrice()))
                    {
                        ConnectorLog.FilePrefix = _kioskApp.GetTotalPrice();
                    }
                }

                UpdateStatus($"SubmitConnectorLog with {ConnectorLog.AppName} :: {_data.JobType}");
                ExecutionServices.DataLogger.Submit(ConnectorLog);
            }            
        }
    }
}
