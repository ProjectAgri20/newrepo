using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a virtual printer component.
    /// </summary>
    [DataContract]
    public class VirtualPrinterInfo : AssetInfo, IPrinterInfo
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _address;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int _printerPort;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly bool _snmpEnabled;

        /// <summary>
        /// Gets the network address (IP or hostname) of the virtual printer.
        /// </summary>
        public string Address => _address;

        /// <summary>
        /// Gets the port used for printing to the virtual printer.
        /// </summary>
        public int PrinterPort => _printerPort;

        /// <summary>
        /// Gets a value indicating whether SNMP is enabled for the virtual printer.
        /// </summary>
        public bool SnmpEnabled => _snmpEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualPrinterInfo" /> class.
        /// </summary>
        /// <param name="assetId">The asset identifier.</param>
        /// <param name="attributes">The asset attributes.</param>
        /// <param name="assetType">The asset type.</param>
        /// <param name="address">The virtual printer address.</param>
        /// <param name="printerPort">The virtual printer port.</param>
        /// <param name="snmpEnabled">if set to <c>true</c> SNMP is enabled for the virtual printer.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assetId" /> is null.
        /// <para>or</para>
        /// <paramref name="address" /> is null.
        /// </exception>
        public VirtualPrinterInfo(string assetId, AssetAttributes attributes, string assetType, string address, int printerPort, bool snmpEnabled)
            : base(assetId, attributes, assetType)
        {
            _address = address ?? throw new ArgumentNullException(nameof(address));
            _printerPort = printerPort;
            _snmpEnabled = snmpEnabled;
        }
    }
}
