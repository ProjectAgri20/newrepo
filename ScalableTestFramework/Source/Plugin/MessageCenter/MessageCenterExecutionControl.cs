using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.Plugin.MessageCenter
{
    /// <summary>
    /// A class that implements the execution portion of the plug-in.
    /// </summary>
    /// <remarks>
    /// This class implements the <see cref="IPluginExecutionEngine"/> interface.
    ///
    /// <seealso cref="IPluginExecutionEngine"/>
    /// </remarks>
    [ToolboxItem(false)]
    public partial class MessageCenterExecutionControl : UserControl, IPluginExecutionEngine
    {
        private IDevice _device;
        private MessageCenterActivityData _activityData;
        private readonly StringBuilder _logText = new StringBuilder();

        /// <summary>
        /// Constructor
        /// </summary>
        public MessageCenterExecutionControl()
        {
            InitializeComponent();
        }

        #region IPluginExecutionEngine implementation

        /// <summary>
        /// The Main Entry point for Execution
        /// </summary>
        /// <param name="executionData"></param>
        /// <returns></returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            _activityData = executionData.GetMetadata<MessageCenterActivityData>();
            var printDeviceInfo = (PrintDeviceInfo)executionData.Assets.First();
            _device = DeviceConstructor.Create(printDeviceInfo);
            var executionResult = EvaluateMessageCenter();
            _device.Dispose();
            return executionResult;
        }

        private PluginExecutionResult EvaluateMessageCenter()
        {
            if (_device is JediOmniDevice)
            {
                if(_activityData.UseEws)
                    return EvaluateByEws();

                return EvaluateByControlPanel();
            }
            return new PluginExecutionResult(PluginResult.Skipped, "Only Jedi Omni devices supported");
        }

        private PluginExecutionResult EvaluateByEws()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            bool foundMessage= false;
            WebClient ewsWebClient = new WebClient();
            var responseBodyString = ewsWebClient.DownloadString($"https://{_device.Address}/hp/device/MessageCenter/Summary");
            var messageCenter = JsonConvert.DeserializeObject<MessageCenterMessages>(responseBodyString);

            var messages = _activityData.Message.GetDescription().Split(',');
            foreach (var message in messages)
            {
                if (messageCenter.Notifications.Any(x =>
                    Regex.Match(x.Message, message.Trim(), RegexOptions.IgnoreCase).Success))
                    foundMessage = true;
            }
            UpdateStatus("Found following notifications:");
            UpdateStatus(string.Join(",", messageCenter.Notifications.Select(x => x.Message)));
            ExecutionServices.SystemTrace.LogDebug($"Found following notifications: {string.Join(",", messageCenter.Notifications.Select(x=>x.Message))}");
            var resultMessage = _activityData.Presence
                ? $"Checked for presence of {_activityData.Message}"
                : $"Checked for absence of {_activityData.Message}";

           

            return foundMessage == _activityData.Presence
                ? new PluginExecutionResult(PluginResult.Passed, resultMessage)
                : new PluginExecutionResult(PluginResult.Failed, resultMessage);

            
        }

        private PluginExecutionResult EvaluateByControlPanel()
        {
            try
            {
                //navigate to message center
                var jediDevice = _device as JediOmniDevice;
                if (jediDevice == null)
                {
                    return new PluginExecutionResult(PluginResult.Skipped, "Only Jedi Omni devices supported");
                }
                var preparationManager = DevicePreparationManagerFactory.Create(jediDevice);
                preparationManager.WakeDevice();
                jediDevice.ControlPanel.PressHome();
                if (!jediDevice.ControlPanel.WaitForState(".hp-button-message-center", OmniElementState.Useable,
                    TimeSpan.FromSeconds(5)))
                {
                    //if message center is not available it means that we might not have any events. check if we are checking for presence or absence of message here and pass or fail the activity accordingly
                    controlPanel_pictureBox.Image = jediDevice.ControlPanel.ScreenCapture();
                    return !_activityData.Presence
                        ? new PluginExecutionResult(PluginResult.Passed,
                            "Messege center is not active, no warning or errors found. Passing the activity.")
                        : new PluginExecutionResult(PluginResult.Failed,
                            "Message center is not active, no warning or errors found. Failing the activity.");
                }

                jediDevice.ControlPanel.PressWait(".hp-button-message-center", "#hpid-message-center-screen",
                    TimeSpan.FromSeconds(5));
                controlPanel_pictureBox.Image = jediDevice.ControlPanel.ScreenCapture();

                var description = _activityData.Message.GetDescription();
                var foundMessage = FindMessage(jediDevice, description);
                var resultMessage = _activityData.Presence
                    ? $"Checked for presence of {_activityData.Message}"
                    : $"Checked for absence of {_activityData.Message}";
                jediDevice.ControlPanel.PressHome();
                return foundMessage == _activityData.Presence
                    ? new PluginExecutionResult(PluginResult.Passed, resultMessage)
                    : new PluginExecutionResult(PluginResult.Failed, resultMessage);
            }
            catch (OmniInvalidOperationException e)
            {
                return new PluginExecutionResult(PluginResult.Failed, e);
            }
            catch (DeviceCommunicationException d)
            {
                return new PluginExecutionResult(PluginResult.Failed, d);
            }
        }

        private static bool FindMessage(JediOmniDevice jediDevice, string message)
        {
            var listItems = jediDevice.ControlPanel.GetIds("#hpid-message-center-list", OmniIdCollectionType.Descendants);

            foreach (var listItem in listItems)
            {
                var innerText = jediDevice.ControlPanel.GetValue($"#{listItem} .hp-listitem-text", "innerText", OmniPropertyType.Property);
                var messages = message.Split(',');
                foreach (var msg in messages)
                {
                    if (Regex.Match(innerText, msg.Trim(), RegexOptions.IgnoreCase).Success)
                        return true;
                }
            }

            return false;
        }

        protected virtual void UpdateStatus(string text)
        {
            status_richTextBox.InvokeIfRequired(c =>
                {
                    ExecutionServices.SystemTrace.LogInfo(text);
                    _logText.Clear();
                    _logText.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff"));
                    _logText.Append(":  ");
                    _logText.AppendLine(text);
                    status_richTextBox.AppendText(_logText.ToString());
                    status_richTextBox.Refresh();
                }
            );
        }

        #endregion IPluginExecutionEngine implementation
    }
}