using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Contains details and availability information for a print device used in an enterprise test.
    /// </summary>
    [DataContract]
    [AssetHost("PrintDeviceHost")]
    public class PrintDeviceDetail : DeviceAssetDetail
    {
        /// <summary>
        /// Gets or sets the secondary address for the print device.
        /// </summary>
        /// <value>
        /// The secondary IP Address.
        /// </value>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the admin password. Default is 'admin'
        /// </summary>
        /// <value>
        /// The admin password.
        /// </value>
        [DataMember]
        public string AdminPassword { get; set; }

        /// <summary>
        /// Gets or sets the port number.
        /// </summary>
        [DataMember]
        public int PortNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SNMP is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if SNMP is enabled; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool SnmpEnabled { get; set; }

        /// <summary>
        /// Gets or sets the Firmware Type.
        /// </summary>
        [DataMember]
        public string FirmwareType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether CRC mode needs to be turned on.
        /// </summary>
        [DataMember]
        public bool UseCrc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating device capability.
        /// </summary>
        [DataMember]
        public AssetAttributes Capability { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDeviceDetail"/> class.
        /// </summary>
        public PrintDeviceDetail(string deviceId, DateTime? availabilityStartTime, DateTime? availabilityEndTime)
            : base(deviceId, availabilityStartTime, availabilityEndTime)
        {
            PortNumber = 9100;          // Convention for physical printers
            SnmpEnabled = true;
            UseCrc = true;
            Capability = 0;
        }

        /// <summary>
        /// Returns a string with assetId, Address, and Description
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join(" ", AssetId, Address, Description);
        }
    }
}