using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Service.Monitor.Output
{
    /// <summary>
    /// Class for logging data about a digital send notification message.
    /// </summary>
    public class DigitalSendJobNotificationLogger : FrameworkDataLog
    {
        /// <summary>
        /// The name of the table to log to.
        /// </summary>
        public override string TableName => "DigitalSendJobNotification";

        /// <summary>
        /// The primary key column name of the log table.
        /// </summary>
        public override string PrimaryKeyColumn => nameof(DigitalSendJobNotificationId);

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSendJobNotificationLogger"/> class.
        /// </summary>
        public DigitalSendJobNotificationLogger()
        {
            DigitalSendJobNotificationId = SequentialGuid.NewGuid();
            FilePrefix = string.Empty;
            SessionId = string.Empty;
            NotificationResult = null;
            NotificationDestination = null;
            NotificationReceivedDateTime = null;
        }

        /// <summary>
        /// Gets the digital send notification log id.
        /// </summary>
        [DataLogProperty]
        public Guid DigitalSendJobNotificationId { get; private set; }

        /// <summary>
        /// Gets or sets the session id.
        /// </summary>
        /// <value>
        /// The session id.
        /// </value>
        [DataLogProperty]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the file prefix.
        /// </summary>
        /// <value>
        /// The file prefix.
        /// </value>
        [DataLogProperty(MaxLength = 255)]
        public string FilePrefix { get; set; }

        /// <summary>
        /// Gets or sets the notification destination.
        /// </summary>
        /// <value>
        /// The notification destination.
        /// </value>
        [DataLogProperty(MaxLength = 255)]
        public string NotificationDestination { get; set; }

        /// <summary>
        /// Gets or sets the notification received time.
        /// </summary>
        /// <value>
        /// The notification received.
        /// </value>
        [DataLogProperty]
        public DateTimeOffset? NotificationReceivedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the notification result.
        /// </summary>
        /// <value>
        /// The notification result.
        /// </value>
        [DataLogProperty]
        public string NotificationResult { get; set; }
    }
}
