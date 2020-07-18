using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;

namespace HP.ScalableTest.Plugin.JetAdvantageScan
{
    [ToolboxItem(false)]
    public partial class JetAdvantageScanExecControl : ScanExecutionControl, IPluginExecutionEngine
    {
        private JetAdvantageScanAutoController _controller = null;
        private const string JETADVANTAGEURL = @"https://pp-micro-staging.hpfoghorn.com";
        private JetAdvantageScanActivityData _activityData;
        private JetAdvantageScanRetrievalLog _dataLog = null;
        private IDeviceInfo _device;
        private int waitTime = 30000;

        private PluginExecutionData _pluginExecutionData = null;

        public JetAdvantageScanExecControl()
        {
            InitializeComponent();
        }

        #region IActivityPlugin Members

        /// <summary>
        /// Processes the activity given to the plugin.
        /// </summary>
        /// <param name="executionData"></param>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);
            _activityData = _pluginExecutionData.GetMetadata<JetAdvantageScanActivityData>();
            _device = (IDeviceInfo)executionData.Assets.First();
            UpdateStatus("Starting JetAdvantage " + "Scan to Cloud Repository");
            SetDataLogger(_device);

            if (CheckJetAdvantageAvailability())
            {
                ScanOptions scanOptions = new ScanOptions()
                {
                    LockTimeouts = _activityData.LockTimeouts,
                    PageCount = _activityData.PageCount,
                    UseAdf = _activityData.UseAdf,
                };
                _controller = new JetAdvantageScanAutoController(executionData, scanOptions);
                _controller.ActivityStatusChanged += UpdateStatus;
                _controller.DeviceSelected += UpdateDevice;
                UpdateDataLogger(result);
                return _controller.RunScanActivity();
            }

            else
            {
                result = new PluginExecutionResult(PluginResult.Failed);
                UpdateDataLogger(result);
                return result;
            }

        }
        /// <summary>
        /// Sets the data logger.
        /// Set data loggers for capturing results and submit what we know so far...
        /// </summary>
        private void SetDataLogger(IDeviceInfo device)
        {
            _dataLog = new JetAdvantageScanRetrievalLog(_pluginExecutionData)
            {
                Username = _pluginExecutionData.Credential.UserName,
                DeviceId = device.AssetId,
                JobStartDateTime = DateTime.Now,
                JetAdvantageLoginId = _activityData.JetAdvantageLoginId.ToString(),
            };
            ExecutionServices.DataLogger.Submit(_dataLog);
        }

        /// <summary>
        /// Checks the jet advantage availability.
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckJetAdvantageAvailability()
        {
            bool isPingable = true;
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(JETADVANTAGEURL);
                request.AllowAutoRedirect = false;
                request.Timeout = waitTime;
                request.Method = "HEAD";

                using (var response = request.GetResponse())
                {
                    isPingable = true;
                }
            }
            catch
            {
                isPingable = false;
                UpdateStatus("Unable to contact JetAdvantage at " + JETADVANTAGEURL);
            }

            return isPingable;
        }

        /// <summary>
        /// Updates the data logger.
        /// </summary>
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

        #endregion
    }
}
