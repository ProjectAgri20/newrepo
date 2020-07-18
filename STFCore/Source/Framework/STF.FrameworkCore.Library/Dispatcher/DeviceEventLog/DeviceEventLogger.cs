using System;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Framework.Dispatcher.DeviceEventLog
{
    internal class DeviceEventLogger : FrameworkDataLog
    {
        public override string TableName => "DeviceEvent";

        public override string PrimaryKeyColumn => nameof(DeviceEventId);

        public DeviceEventLogger()
        {
            DeviceEventId = SequentialGuid.NewGuid();
        }

        /// <summary>
        /// Gets the device events log identifier; primary key.
        /// </summary>
        /// <value>
        /// The device events log identifier.
        /// </value>
        [DataLogProperty]
        public Guid DeviceEventId { get; private set; }

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        [DataLogProperty]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the asset identifier, LTL value of the device.
        /// </summary>
        /// <value>
        /// The asset identifier.
        /// </value>
        [DataLogProperty]
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets the IP address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [DataLogProperty]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the start time for the session. This is used to determine if the devices
        /// event occurred during the session of testing.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        [DataLogProperty]
        public DateTimeOffset StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the event date time.
        /// </summary>
        /// <value>
        /// The event date time.
        /// </value>
        [DataLogProperty]
        public DateTimeOffset EventDateTime { get; set; }

        /// <summary>
        /// Gets or sets the type of the event, Error, Warning, Info.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        [DataLogProperty(MaxLength = 20)]
        public string EventType { get; set; }

        /// <summary>
        /// Gets or sets the event code.
        /// </summary>
        /// <value>
        /// The event code.
        /// </value>
        [DataLogProperty(MaxLength = 20)]
        public string EventCode { get; set; }

        /// <summary>
        /// Gets or sets the event description.
        /// </summary>
        /// <value>
        /// The event description.
        /// </value>
        [DataLogProperty(MaxLength = 1024, TruncationAllowed = true)]
        public string EventDescription { get; set; }
    }
}
