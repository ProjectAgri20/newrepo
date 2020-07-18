using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a printer device (either SFP or MFP).
    /// </summary>
    [DataContract]
    public class PrintDeviceInfo : DeviceInfo, IPrinterInfo
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int _printerPort;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly bool _snmpEnabled;

        /// <summary>
        /// Gets the port used for printing to the device.
        /// </summary>
        public int PrinterPort => _printerPort;

        /// <summary>
        /// Gets a value indicating whether SNMP is enabled for the device.
        /// </summary>
        public bool SnmpEnabled => _snmpEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDeviceInfo" /> class.
        /// </summary>
        /// <param name="assetId">The unique identifier for the print device.</param>
        /// <param name="attributes">The <see cref="AssetAttributes" /> for the print device.</param>
        /// <param name="assetType">The print device type.</param>
        /// <param name="address">The print device network address.</param>
        /// <param name="address2">Secondary address used by the device.</param>
        /// <param name="adminPassword">The print device admin password.</param>
        /// <param name="productName">The print device product name.</param>
        /// <param name="printerPort">The port used for printing to the device.</param>
        /// <param name="snmpEnabled">A value indicating whether SNMP is enabled for the device.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assetId" /> is null.
        /// <para>or</para>
        /// <paramref name="address" /> is null.
        /// </exception>
        public PrintDeviceInfo(string assetId, AssetAttributes attributes, string assetType, string address, string address2, string adminPassword, string productName, int printerPort, bool snmpEnabled)
            : base(assetId, attributes, assetType, address, address2, adminPassword, productName)
        {
            _printerPort = printerPort;
            _snmpEnabled = snmpEnabled;
        }
    }
}
