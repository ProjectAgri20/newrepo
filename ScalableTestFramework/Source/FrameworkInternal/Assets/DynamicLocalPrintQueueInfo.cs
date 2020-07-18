using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a local print queue that will be dynamically created on the test client.
    /// </summary>
    [DataContract]
    public class DynamicLocalPrintQueueInfo : PrintQueueInfo
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _address;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PrintDriverInfo _printDriver;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PrinterPortInfo _printerPort;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int _portNumber;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly bool _snmpEnabled;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PrintDriverConfiguration _printDriverConfiguration;

        /// <summary>
        /// Gets the network address to use for this queue.
        /// </summary>
        public string Address => _address;

        /// <summary>
        /// Gets the print driver to use for this queue.
        /// </summary>
        public PrintDriverInfo PrintDriver => _printDriver;

        /// <summary>
        /// Gets the <see cref="PrinterPortInfo" /> to use for this queue.
        /// </summary>
        public PrinterPortInfo PrinterPort => _printerPort;

        /// <summary>
        /// Gets the port number to use for this queue.
        /// </summary>
        public int PortNumber => _portNumber;

        /// <summary>
        /// Gets a value indicating whether SNMP should be enabled for this queue.
        /// </summary>
        public bool SnmpEnabled => _snmpEnabled;

        /// <summary>
        /// Gets the print driver configuration to use for this queue.
        /// </summary>
        public PrintDriverConfiguration PrintDriverConfiguration => _printDriverConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicLocalPrintQueueInfo" /> class.
        /// </summary>
        /// <param name="assetId">The asset identifier for the asset associated with this print queue.</param>
        /// <param name="address">The network address to use for this print queue.</param>
        /// <param name="printDriver">The print driver to use for this print queue.</param>
        /// <param name="printerPort">The <see cref="PrinterPortInfo" /> to use when creating the queue.</param>
        /// <param name="portNumber">The port number to use when creating the queue.</param>
        /// <param name="snmpEnabled">Indicates whether SNMP should be enabled when creating the queue.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="address" /> is null.
        /// <para>or</para>
        /// <paramref name="printDriver" /> is null.
        /// <para>or</para>
        /// <paramref name="printerPort" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="address" /> is an empty string.</exception>
        public DynamicLocalPrintQueueInfo(string assetId, string address, PrintDriverInfo printDriver, PrinterPortInfo printerPort, int portNumber, bool snmpEnabled)
            : this(assetId, address, printDriver, printerPort, portNumber, snmpEnabled, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicLocalPrintQueueInfo" /> class.
        /// </summary>
        /// <param name="assetId">The asset identifier for the asset associated with this print queue.</param>
        /// <param name="address">The network address to use for this print queue.</param>
        /// <param name="printDriver">The print driver to use for this print queue.</param>
        /// <param name="printerPort">The <see cref="PrinterPortInfo" /> to use when creating the queue.</param>
        /// <param name="portNumber">The port number to use when creating the queue.</param>
        /// <param name="snmpEnabled">Indicates whether SNMP should be enabled when creating the queue.</param>
        /// <param name="printDriverConfiguration">The print driver configuration.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="address" /> is null.
        /// <para>or</para>
        /// <paramref name="printDriver" /> is null.
        /// <para>or</para>
        /// <paramref name="printerPort" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="address" /> is an empty string.</exception>
        public DynamicLocalPrintQueueInfo(string assetId, string address, PrintDriverInfo printDriver, PrinterPortInfo printerPort, int portNumber, bool snmpEnabled, PrintDriverConfiguration printDriverConfiguration)
            : base(BuildQueueName(address, portNumber, printDriver, printDriverConfiguration), assetId)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentException("Address cannot be an empty string.", nameof(address));
            }

            _address = address;
            _printDriver = printDriver ?? throw new ArgumentNullException(nameof(printDriver));
            _printerPort = printerPort ?? throw new ArgumentNullException(nameof(printerPort));
            _portNumber = portNumber;
            _snmpEnabled = snmpEnabled;
            _printDriverConfiguration = printDriverConfiguration ?? new PrintDriverConfiguration();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicLocalPrintQueueInfo" /> class.
        /// </summary>
        /// <param name="printDevice">The print device to use for this print queue.</param>
        /// <param name="printDriver">The print driver to use for this print queue.</param>
        /// <param name="printerPort">The <see cref="PrinterPortInfo" /> to use when creating the queue.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="printDevice" /> is null.
        /// <para>or</para>
        /// <paramref name="printDriver" /> is null.
        /// <para>or</para>
        /// <paramref name="printerPort" /> is null.
        /// </exception> 
        public DynamicLocalPrintQueueInfo(IPrinterInfo printDevice, PrintDriverInfo printDriver, PrinterPortInfo printerPort)
            : this(printDevice?.AssetId, printDevice?.Address, printDriver, printerPort, printDevice?.PrinterPort ?? 0, printDevice?.SnmpEnabled ?? false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicLocalPrintQueueInfo" /> class.
        /// </summary>
        /// <param name="printDevice">The print device to use for this print queue.</param>
        /// <param name="printDriver">The print driver to use for this print queue.</param>
        /// <param name="printerPort">The <see cref="PrinterPortInfo" /> to use when creating the queue.</param>
        /// <param name="printDriverConfiguration">The print driver configuration.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="printDevice" /> is null.
        /// <para>or</para>
        /// <paramref name="printDriver" /> is null.
        /// <para>or</para>
        /// <paramref name="printerPort" /> is null.
        /// </exception> 
        public DynamicLocalPrintQueueInfo(IPrinterInfo printDevice, PrintDriverInfo printDriver, PrinterPortInfo printerPort, PrintDriverConfiguration printDriverConfiguration)
            : this(printDevice?.AssetId, printDevice?.Address, printDriver, printerPort, printDevice?.PrinterPort ?? 0, printDevice?.SnmpEnabled ?? false, printDriverConfiguration)
        {
        }

        private static string BuildQueueName(string address, int portNumber, PrintDriverInfo printDriver, PrintDriverConfiguration configuration = null)
        {
            if (configuration == null)
            {
                return $"{printDriver?.DriverName} (IP_{address}:{portNumber})";
            }
            else
            {
                return $"{printDriver?.DriverName} ({configuration.FileName}_IP_{address}:{portNumber})";
            }
        }

        /// <summary>
        /// Creates a new <see cref="PrintQueueDefinition" /> representing this instance.
        /// </summary>
        /// <returns>A <see cref="PrintQueueDefinition" /> that represents this instance.</returns>
        public override PrintQueueDefinition CreateDefinition() => new DynamicLocalPrintQueueDefinition(this);
    }
}
