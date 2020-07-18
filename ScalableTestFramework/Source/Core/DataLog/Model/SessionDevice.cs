using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.DataLog.Model
{
    /// <summary>
    /// A device used by a test session.
    /// </summary>
    [DebuggerDisplay("{DeviceId,nq}")]
    public sealed class SessionDevice
    {
        /// <summary>
        /// Gets or sets the unique identifier for this session device record.
        /// </summary>
        public Guid SessionDeviceId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the session that used the device.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the device that was used.
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the device product codename.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the device name.
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Gets or sets the device firmware revision.
        /// </summary>
        public string FirmwareRevision { get; set; }

        /// <summary>
        /// Gets or sets the device firmware datecode.
        /// </summary>
        public string FirmwareDatecode { get; set; }

        /// <summary>
        /// Gets or sets the firmware bundle version.
        /// </summary>
        public string FirmwareBundleVersion { get; set; }

        /// <summary>
        /// Gets or sets the device firmware type.
        /// </summary>
        public string FirmwareType { get; set; }

        /// <summary>
        /// Gets or sets the device model number.
        /// </summary>
        public string ModelNumber { get; set; }

        /// <summary>
        /// Gets or sets the device IP address.
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the device network card model.
        /// </summary>
        public string NetworkCardModel { get; set; }

        /// <summary>
        /// Gets or sets the device network interface version.
        /// </summary>
        public string NetworkInterfaceVersion { get; set; }
    }
}
