using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceMemory.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData;
using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.DeviceAutomation.LinkApps.Kiosk.RegusKiosk;
using HP.ScalableTest.Plugin.RegusKiosk.Options;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.Utility;
using System;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.RegusKiosk
{
    public class RegusKioskPrintManager : LinkPrintActivityManager
    {
        private readonly RegusKioskActivityData _data;
        private readonly RegusKioskPrintOptions _regusKioskPrintOptions;
        RegusKioskApp _regusKioskApp = null;

        protected override string LinkJobType => RegusKioskJobType.Print.GetDescription();

        /// <summary>
        /// Initializes a new instance of the <see cref="RegusKioskPrintManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="printOptions">The scan options.</param>
        public RegusKioskPrintManager(PluginExecutionData executionData, RegusKioskPrintOptions printOptions, LockTimeoutData lockTimeoutData)
                    : base(executionData, lockTimeoutData)
        {
            _data = executionData.GetMetadata<RegusKioskActivityData>();
            _regusKioskPrintOptions = _data.PrintOptions;
        }

        /// <summary>
        /// Executes the scan job using the specified device.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="deviceInfo">The device information.</param>
        /// <returns>The result of execution.</returns>
        protected override PluginExecutionResult ExecuteLinkPrint(IDevice device, IDeviceInfo deviceInfo)
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

                _regusKioskApp = new RegusKioskApp(device);
                _regusKioskApp.WorkflowLogger = WorkflowLogger;                
                _regusKioskApp.DeviceInfo = deviceInfo;

                SetupJob(device);
                UpdateStatus("Job setup complete.");

                // Finish the job (apply job build options, press start, wait for finish)
                UpdateStatus("Finishing job...");
                result = FinishJob(device);
                UpdateStatus("Job finished.");

                // Clean up
                try
                {
                    UpdateStatus("SignOut Start");
                    SignOut();
                    UpdateStatus("SignOut finished");

                    RecordEvent(DeviceWorkflowMarker.ActivityEnd);

                    try
                    {
                        CollectJetAdvantagelinkMemoryMonitoring(deviceInfo);
                    }
                    catch (Exception ex)
                    {
                        UpdateStatus(ex.ToString());

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
        /// Sets up the RegusKiosk print job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override void SetupJob(IDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }

            UpdateStatus($"Starting RegusKiosk plugin: {_data.JobType} with {_data.AuthType}");

            _regusKioskApp.RegusKioskInitialize();

            switch (_data.AuthType)
            {
                case RegusKioskAuthType.Login:
                    _regusKioskApp.RegusKioskLoginAuthenticate(_data.ID, _data.Password);
                    break;
                case RegusKioskAuthType.Card:
                    _regusKioskApp.RegusKioskCardAuthenticate(ExecutionData);
                    break;
                case RegusKioskAuthType.Pin:
                    _regusKioskApp.RegusKioskPinAuthenticate(_data.Pin);
                    break;
                default:
                    DeviceWorkflowException e = new DeviceWorkflowException($"Kiosk Auth Type is invalid: {_data.AuthType}");
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignIn.GetDescription());
                    throw e;
            }

            UpdateStatus($"Print from {_data.PrintOptions.PrintSource}");

            _regusKioskApp.LaunchPrint(_data.JobType, _data.PrintOptions.PrintSource, _data.PrintOptions.Path);

            if (!RegusKioskPrintSource.PrinterOn.Equals(_data.PrintOptions.PrintSource))
            {
                SetOptions(_regusKioskApp.KioskOptionManager);
            }
            
        }

        /// <summary>
        /// Set options for the copy job.
        /// </summary>                
        protected void SetOptions(RegusKioskOptionsManager regusKioskOptionsManager)
        {
            try
            {
                UpdateStatus($"Select option activity with Color Mode {_regusKioskPrintOptions.ColorMode.GetDescription()} is being started");
                regusKioskOptionsManager.SetColorMode(_regusKioskPrintOptions.ColorMode);
                UpdateStatus($"Select option activity with Color Mode {_regusKioskPrintOptions.ColorMode.GetDescription()} is being completed");

                UpdateStatus($"Select option activity with Duplex {_regusKioskPrintOptions.Duplex.GetDescription()} is being started");
                regusKioskOptionsManager.SetDuplexPrint(_regusKioskPrintOptions.Duplex);
                UpdateStatus($"Select option activity with Duplex {_regusKioskPrintOptions.Duplex.GetDescription()} is being completed");

                UpdateStatus($"Select option activity with N-Up {_regusKioskPrintOptions.Duplex.GetDescription()} is being started");
                regusKioskOptionsManager.SetPaperSource(_regusKioskPrintOptions.PaperSource);
                UpdateStatus($"Select option activity with N-Up {_regusKioskPrintOptions.Duplex.GetDescription()} is being completed");

                UpdateStatus($"Select option activity with auto fit {_regusKioskPrintOptions.AutoFit} is being started");
                regusKioskOptionsManager.SetAutoFit(_regusKioskPrintOptions.AutoFit);
                UpdateStatus($"Select option activity with auto fit {_regusKioskPrintOptions.AutoFit} is being completed");

                UpdateStatus($"Select option activity with Copies {_regusKioskPrintOptions.PageCount} is being started");
                regusKioskOptionsManager.SetPrintCopies(_regusKioskPrintOptions.PageCount);
                UpdateStatus($"Select option activity with Copies {_regusKioskPrintOptions.PageCount} is being completed");
            }
            catch (DeviceWorkflowException ex)
            {
                DeviceWorkflowException e = new DeviceWorkflowException(ex.Message + $" :: {_data.JobType.GetDescription()}", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SelectOptions.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Finish up the RegusKiosk print job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");
            
            UpdateStatus($"Starting Execution Print Job on {_data.PrintOptions.PrintSource} :: RegusKiosk");
            _regusKioskApp.ExecutionJob(_data.JobType);

            result = new PluginExecutionResult(PluginResult.Passed);

            return result;            
        }

        /// <summary>
        /// Navigates to the home screen.
        /// </summary>
        protected override void SignOut()
        {
            if(_regusKioskApp != null)
            {
                RecordEvent(DeviceWorkflowMarker.DeviceSignOutBegin);
                _regusKioskApp.SignOut();
                RecordEvent(DeviceWorkflowMarker.DeviceSignOutEnd);
            }            
        }

        /// <summary>
        /// Dispose RegusKiosk
        /// </summary>
        public override void Dispose()
        {
            if(_regusKioskApp != null)
            {
                _regusKioskApp.Dispose();
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

            JetAdvantageLinkMemoryMonitoring jalMem = new JetAdvantageLinkMemoryMonitoring(_regusKioskApp.LinkUI, _regusKioskApp.KioskPackageName, ExecutionData, deviceInfo);
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
                if (_regusKioskApp != null)
                {                    
                    JetAdvantageLinkTriage triage = new JetAdvantageLinkTriage(_regusKioskApp.Device, _regusKioskApp.LinkUI, ExecutionData);
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
                if(_regusKioskApp != null)
                {
                    UpdateStatus($"Dispose RegusKioskApp on GatherTriageData");
                    _regusKioskApp.Dispose();
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
                ConnectorLog.AppName = "RegusKiosk";
                ConnectorLog.JobEndStatus = result;
                ConnectorLog.OptionsData = Serializer.Serialize(_regusKioskPrintOptions).ToString();

                UpdateStatus($"SubmitConnectorLog with {ConnectorLog.AppName} :: {_data.JobType}");
                ExecutionServices.DataLogger.Submit(ConnectorLog);
            }
        }
    }
}
