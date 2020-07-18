namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Contains the necessary data that describes a printing port.
    /// </summary>
    /// <example>
    /// The following code creates a port at 10.10.5.123:17800 with SNMP enabled
    /// and the SNMP community set to "public".
    /// <code>
    /// TcpIPPortManager data = TcpIPPortManager.CreateRawPortData(
    ///     "10.10.5.123", 17800, true, "MyPrinterPort");
    /// data.SnmpCommunity = "\"public\"";
    /// data.CreatePort();
    /// </code>
    /// </example>
    public class TcpIPPortInstaller
    {
        private enum PortProtocolType
        {
            Raw = 1,
            LPR = 2,
        }

        private PortProtocolType _protocol;

        /// <summary>
        /// Default constructor that is needed for the serializer to work.
        /// DO NOT USE THIS CONSTRUCTOR!
        /// </summary>
        public TcpIPPortInstaller()
        {
        }

        /// <summary>
        /// Creates the configuration for a RAW port.
        /// </summary>
        /// <param name="portAddress">The port address.</param>
        /// <param name="portNumber">The port number.</param>
        /// <param name="portName">Name of the port.</param>
        /// <param name="snmpEnabled">if set to <c>true</c>, SNMP is enabled.</param>
        /// <returns>Port configuration with RAW settings.</returns>
        public static TcpIPPortInstaller CreateRawPortManager(
            string portAddress,
            int portNumber,
            string portName,
            bool snmpEnabled)
        {
            return CreatePortManager(PortProtocolType.Raw, portAddress, string.Empty, portNumber, portName, snmpEnabled);
        }

        /// <summary>
        /// Creates the configuration for an LPR port.
        /// </summary>
        /// <param name="portAddress">The port address.</param>
        /// <param name="queue">The queue.</param>
        /// <param name="portNumber">The port number.</param>
        /// <param name="portName">Name of the port.</param>
        /// <param name="snmpEnabled">if set to <c>true</c>, SNMP is enabled.</param>
        /// <returns>Port configuration with LPR settings.</returns>
        public static TcpIPPortInstaller CreateLPRPortManager(
            string portAddress,
            string queue,
            int portNumber,
            string portName,
            bool snmpEnabled)
        {
            return CreatePortManager(PortProtocolType.LPR, portAddress, queue, portNumber, portName, snmpEnabled);
        }

        private static TcpIPPortInstaller CreatePortManager(
            PortProtocolType protocol,
            string portAddress,
            string queue,
            int portNumber,
            string portName,
            bool snmpEnabled)
        {
            TcpIPPortInstaller portManager = new TcpIPPortInstaller
            {
                Address = portAddress,
                PortNumber = portNumber,
                _protocol = protocol,
                PortName = portName,
                SnmpEnabled = snmpEnabled
            };

            if (protocol == PortProtocolType.LPR)
            {
                portManager.Queue = queue;
            }
            return portManager;
        }

        /// <summary>
        /// Gets or sets the port number.
        /// </summary>
        /// <value>The port number.</value>
        public int PortNumber { get; private set; }

        /// <summary>
        /// Gets the name of the port.
        /// </summary>
        /// <value>The name of the port.</value>
        public string PortName { get; private set; }

        /// <summary>
        /// Gets or sets the port address.
        /// </summary>
        /// <value>The port address.</value>
        public string Address { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [SNMP enabled].
        /// </summary>
        /// <value><c>true</c> if [SNMP enabled]; otherwise, <c>false</c>.</value>
        public bool SnmpEnabled { get; private set; }

        /// <summary>
        /// Gets or sets the queue assigned to this port.
        /// </summary>
        public string Queue { get; private set; }

        /// <summary>
        /// Creates a new printer port.
        /// </summary>
        public void CreatePort()
        {
            switch (_protocol)
            {
                case PortProtocolType.Raw:
                    PrintPortManager.AddRawPort(PortName, PortNumber, Address, SnmpEnabled, "public", 1);
                    break;

                case PortProtocolType.LPR:
                    PrintPortManager.AddLprPort(PortName, PortNumber, Address, Queue, SnmpEnabled, "public", 1);
                    break;
            }
        }

        /// <summary>
        /// Deletes an existing printer port.
        /// </summary>
        public void DeletePort()
        {
            PrintPortManager.DeletePort(PortName);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return PortName;
        }
    }
}
