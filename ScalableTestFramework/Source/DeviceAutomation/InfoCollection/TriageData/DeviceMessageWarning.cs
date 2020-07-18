using HP.DeviceAutomation.Jedi;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData
{
    /// <summary>
    /// uses web client of look for warning messages on a device
    /// </summary>
    public class DeviceMessageWarning
    {
        /// <summary>
        /// Gets the device message warnings.
        /// </summary>
        /// <value>
        /// Private 'Set' for the device warnings.
        /// </value>
        public string MessageWarnings { get; private set; }

        private JediOmniDevice _device = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceMessageWarning"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public DeviceMessageWarning(JediOmniDevice device)
        {
            _device = device;
        }

        /// <summary>
        /// Determines whether the device has message warnings.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if true will set the info in property MessageWarnings <c>false</c>.
        /// </returns>
        public bool IsMessageWarnings()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            WebClient ewsWebClient = new WebClient();
            var responseBodyString = ewsWebClient.DownloadString($"https://{_device.Address}/hp/device/MessageCenter/Summary");
            var messageCenter = JsonConvert.DeserializeObject<MessageCenterMessages>(responseBodyString);

            int cnt = messageCenter.Notifications.Count();
            
            StringBuilder messageWarningBuilder = new StringBuilder();
            foreach (var messageCenterNotification in messageCenter.Notifications)
            {
                messageWarningBuilder.AppendFormat("{0} - {1}, Priority: {2};", messageCenterNotification.Type,
                    messageCenterNotification.Message.Trim(), messageCenterNotification.Priority);
            }

            MessageWarnings = messageWarningBuilder.ToString();
            return cnt > 0;
            //if (cnt > 0)
            //{
            //    for (int idx = 0; idx < cnt; idx++)
            //    {
            //        string msg = messageCenter.Notifications[0].Type + "-";
            //        msg += messageCenter.Notifications[0].Message.Trim();
            //        msg += "Priority: " + messageCenter.Notifications[0].Priority;

            //        if (idx == 0)
            //        {
            //            MessageWarnings = msg;
            //        }
            //        else
            //        {
            //            MessageWarnings += ";" + msg;
            //        }
            //    }
            //    foundMessage = true;
            //}

            //return foundMessage;
        }



    }

    /// <summary>
    /// Class to hold deserialized data of message center
    /// </summary>

    internal class MessageCenterMessages
    {
        [JsonProperty("messages")]
        internal List<MessageCenterNotification> Notifications { get; set; }

        internal MessageCenterMessages()
        {
            Notifications = new List<MessageCenterNotification>();
        }
    }

    /// <summary>
    /// class representing message center notification from EWS
    /// </summary>
    internal class MessageCenterNotification
    {
        //{"messages":[{"type":"notification","msg":"Sleep mode on","priority":"299"}]}

        [JsonProperty("type")]
        internal string Type { get; set; }

        [JsonProperty("msg")]
        internal string Message { get; set; }

        [JsonProperty("priority")]
        internal int Priority { get; set; }
    }
}
