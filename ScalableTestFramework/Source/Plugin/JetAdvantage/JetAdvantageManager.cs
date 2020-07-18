using System;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.SolutionApps.JetAdvantage;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.JetAdvantage
{
    public class JetAdvantageManager : DeviceWorkflowLogSource
    {
        public event EventHandler<StatusChangedEventArgs> StatusUpdate;

        private readonly PluginExecutionData _pluginExecutionData;
        private readonly JetAdvantageActivityData _activityData;
        private readonly IDevice _device;
        private readonly PrintDeviceInfo _printDeviceInfo;

        private IJetAdvantageApp _jetAdvantageApp = null;

        private JetAdvantagePullPrintJobRetrievalLog _dataLog = null;

        public JetAdvantageManager(PluginExecutionData executionData, DeviceWorkflowLogger workflowLogger, IDevice device, PrintDeviceInfo printDeviceInfo)
        {
            _pluginExecutionData = executionData;
            _activityData = executionData.GetMetadata<JetAdvantageActivityData>();
            _device = device;
            _printDeviceInfo = printDeviceInfo;
            WorkflowLogger = workflowLogger;
        }

        public PluginExecutionResult RunJetAdvantage()
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);
            try
            {
                SetDataLogger();
                _jetAdvantageApp = JetAdvantageFactory.Create(_device);
                _jetAdvantageApp.WorkflowLogger = WorkflowLogger;

                IDevicePreparationManager dpm = DevicePreparationManagerFactory.Create(_device);
                dpm.InitializeDevice(true);
                dpm.WorkflowLogger = WorkflowLogger;

                _jetAdvantageApp.ActivityStatusChange += _jetAdvantageApp_ActivityStatusChange;
                LaunchJetAdvantage();
                _jetAdvantageApp.RunHPJetAdvantage(_activityData.PrintAllDocuments, _activityData.DeleteAfterPrint);

                RecordEvent(DeviceWorkflowMarker.DeviceSignOutBegin);
                _jetAdvantageApp.Logout();
                RecordEvent(DeviceWorkflowMarker.DeviceSignOutEnd);

                if (!_jetAdvantageApp.DocumentPrinted)
                {
                    string logText = string.Format("No print jobs found to pull; user {0}", _activityData.JetAdvantageLoginId);
                    result = new PluginExecutionResult(PluginResult.Skipped, logText);
                    OnStatusUpdate(logText);
                }

                UpdateDataLogger(result);

                dpm.NavigateHome();
                dpm.SignOut();
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                if (!string.IsNullOrEmpty(message) && message.Length > 1024)
                {
                    message = message.Substring(0, 1024);
                }
                result = new PluginExecutionResult(PluginResult.Failed, message, "JetAdvantage Error");

                UpdateDataLogger(result);
                throw;
            }
            return result;
        }

        private void LaunchJetAdvantage()
        {
            try
            {
                OnStatusUpdate("Logging into device and launching JetAdvantage PullPrinting application");
                _jetAdvantageApp.Launch();

                RecordEvent(DeviceWorkflowMarker.AuthenticationBegin);

                _jetAdvantageApp.SignIn(_activityData.JetAdvantageLoginId, _activityData.JetAdvantagePassword, _activityData.UseLoginPin, _activityData.JetAdvantageLoginPin);
                RecordEvent(DeviceWorkflowMarker.AuthenticationEnd);
            }
            catch (Exception ex)
            {
                OnStatusUpdate(string.Format("Error: {0}", ex.Message));
                throw;
            }
        }

        private void _jetAdvantageApp_ActivityStatusChange(object sender, StatusChangedEventArgs e)
        {
            OnStatusUpdate(e.StatusMessage);
        }
        /// <summary>
        /// Sets the data logger.
        /// Set data loggers for capturing results and submit what we know so far...
        /// </summary>
        private void SetDataLogger()
        {
            _dataLog = new JetAdvantagePullPrintJobRetrievalLog(_pluginExecutionData)
            {
                Username = _pluginExecutionData.Credential.UserName,
                DeviceId = _device.Address,
                JobStartDateTime = DateTime.Now,
                JetAdvantageLoginId = _activityData.JetAdvantageLoginId,
                SolutionType = "Atlas - JetAdvantage Pull Print",
            };
            ExecutionServices.DataLogger.Submit(_dataLog);

            // Log the device information
            LogDevice();
        }

        /// <summary>
        /// Logs the device.
        /// </summary>
        private void LogDevice()
        {
            ExecutionServices.DataLogger.Submit(new ActivityExecutionAssetUsageLog(_pluginExecutionData, _printDeviceInfo.AssetId));
        }
        /// <summary>
        /// Updates the data logger.
        /// </summary>
        /// <param name="result">The job end status.</param>
        private void UpdateDataLogger(PluginExecutionResult result)
        {
            if (_dataLog != null)
            {
                _dataLog.JobEndDateTime = DateTime.Now;
                _dataLog.JobEndStatus = result.Result.ToString();
                if (!string.IsNullOrWhiteSpace(result.Message))
                {
                    _dataLog.ErrorMessage = result.Message;
                }
                ExecutionServices.DataLogger.Update(_dataLog);
            }
        }
        /// <summary>
        /// Invoke the StatusUpdate Event.
        /// </summary>
        /// <param name="message"></param>
        protected void OnStatusUpdate(string message)
        {
            StatusUpdate?.Invoke(this, new StatusChangedEventArgs(message));
        }
    }
}
