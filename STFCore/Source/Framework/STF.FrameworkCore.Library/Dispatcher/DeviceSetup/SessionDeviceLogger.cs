using System;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Framework.Dispatcher.DeviceSetup
{
    /// <summary>
    /// DataLog table, SessionDevice, properties
    /// </summary>
    internal class SessionDeviceLogger : FrameworkDataLog
    {
        public override string TableName => "SessionDevice";

        public override string PrimaryKeyColumn => nameof(SessionDeviceId);

        public SessionDeviceLogger()
        {
            SessionDeviceId = SequentialGuid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the session device identifier.
        /// </summary>
        /// <value>
        /// The session device identifier.
        /// </value>
        [DataLogProperty]
        public Guid SessionDeviceId { get; set; }

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        [DataLogProperty]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>
        /// The device identifier.
        /// </value>
        [DataLogProperty]
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        [DataLogProperty(MaxLength = 255)]
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        /// <value>
        /// The name of the device.
        /// </value>
        [DataLogProperty(MaxLength = 255)]
        public string DeviceName { get; set; }

        /// <summary>
        /// Gets or sets the firmware revision.
        /// </summary>
        /// <value>
        /// The firmware revision.
        /// </value>
        [DataLogProperty]
        public string FirmwareRevision { get; set; }

        /// <summary>
        /// Gets or sets the firmware date code.
        /// </summary>
        /// <value>
        /// The firmware date code.
        /// </value>
        [DataLogProperty(MaxLength = 20)]
        public string FirmwareDatecode { get; set; }

        /// <summary>
        /// Gets or sets the type of the firmware.
        /// </summary>
        /// <value>
        /// The type of the firmware.
        /// </value>
        [DataLogProperty]
        public string FirmwareType { get; set; }

        /// <summary>
        /// Gets or sets the firmware bundle version.
        /// </summary>
        /// <value>
        /// The firmware bundle version.
        /// </value>
        [DataLogProperty]
        public string FirmwareBundleVersion { get; set; }

        /// <summary>
        /// Gets or sets the model number.
        /// </summary>
        /// <value>
        /// The model number.
        /// </value>
        [DataLogProperty]
        public string ModelNumber { get; set; }

        /// <summary>
        /// Gets or sets the network card model number.
        /// </summary>
        /// <value>
        /// The network card model.
        /// </value>
        [DataLogProperty]
        public string NetworkCardModel { get; set; }

        /// <summary>
        /// Gets or sets the network adapter version.
        /// </summary>
        /// <value>
        /// The network adapter version.
        /// </value>
        [DataLogProperty(MaxLength = 255, TruncationAllowed = true)]
        public string NetworkInterfaceVersion { get; set; }

        /// <summary>
        /// Gets or sets the IP address.
        /// </summary>
        /// <value>
        /// The IP address.
        /// </value>
        [DataLogProperty]
        public string IpAddress { get; set; }
    }
}
