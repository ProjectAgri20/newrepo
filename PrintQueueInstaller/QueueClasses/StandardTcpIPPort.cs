using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Win32;

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
    internal class StandardTcpIPPort
    {
        private string _portName = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardTcpIPPort" /> class.
        /// </summary>
        public StandardTcpIPPort(string portAddress, int portNumber)
        {
            Address = portAddress;
            PortNumber = portNumber;
            PortName = $"IP_{Address}:{PortNumber}";
        }

        /// <summary>
        /// Returns a list of printer ports installed on the executing machine.
        /// </summary>
        public static Collection<StandardTcpIPPort> InstalledPorts
        {
            get
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Print\Monitors\Standard TCP/IP Port\Ports"))
                {
                    Collection<StandardTcpIPPort> installedPorts = new Collection<StandardTcpIPPort>();
                    foreach (string subKeyName in key.GetSubKeyNames())
                    {
                        using (RegistryKey subKey = key.OpenSubKey(subKeyName))
                        {
                            installedPorts.Add(new StandardTcpIPPort(subKey));
                        }
                    }

                    return installedPorts;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardTcpIPPort"/> class.
        /// </summary>
        /// <param name="key">Registry key that contains printer port data information.</param>
        private StandardTcpIPPort(RegistryKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            CultureInfo cultureInfo = CultureInfo.CurrentCulture;

            string[] parts = key.Name.Split('\\');
            PortName = parts[parts.Length - 1];

            Address = key.GetValue("HostName").ToString();
            PortNumber = Convert.ToInt32(key.GetValue("PortNumber"), cultureInfo);
            Protocol = (int)key.GetValue("Protocol");
            SnmpCommunity = key.GetValue("SNMP Community").ToString();
            SnmpDevIndex = Convert.ToInt32(key.GetValue("SNMP Index"), cultureInfo);
            SnmpEnabled = Convert.ToUInt32(key.GetValue("SNMP Enabled"), cultureInfo) == 1 ? true : false;
        }

        /// <summary>
        /// Gets or sets the protocol.
        /// </summary>
        /// <value>The protocol.</value>
        public int Protocol { get; set; }

        /// <summary>
        /// Gets or sets the port number.
        /// </summary>
        /// <value>The port number.</value>
        public int PortNumber { get; set; }

        /// <summary>
        /// Gets the name of the port.
        /// </summary>
        /// <value>The name of the port.</value>
        public string PortName { get; set; }

        /// <summary>
        /// Gets or sets the port address.
        /// </summary>
        /// <value>The port address.</value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [SNMP enabled].
        /// </summary>
        /// <value><c>true</c> if [SNMP enabled]; otherwise, <c>false</c>.</value>
        public bool SnmpEnabled { get; set; }

        /// <summary>
        /// Gets or sets the SNMP community.
        /// </summary>
        /// <value>The SNMP community.</value>
        public string SnmpCommunity { get; set; }

        /// <summary>
        /// Gets or sets the index of the SNMP dev.
        /// </summary>
        /// <value>The index of the SNMP dev.</value>
        public int SnmpDevIndex { get; set; }

        /// <summary>
        /// Gets or sets the queue assigned to this port.
        /// </summary>
        public string Queue { get; set; }

        /// <summary>
        /// Creates a new printer port.
        /// </summary>
        public void CreatePort()
        {
            switch (Protocol)
            {
                case 1: // RAW
                    PrintPortManager.AddRawPort(PortName, PortNumber, Address, SnmpEnabled, SnmpCommunity, SnmpDevIndex);
                    break;

                case 2: // LPR
                    PrintPortManager.AddLprPort(PortName, PortNumber, Address, Queue, SnmpEnabled, SnmpCommunity, SnmpDevIndex);
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
