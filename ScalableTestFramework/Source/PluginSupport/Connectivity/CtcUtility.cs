using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.ScalableTest.DeviceAutomation.NativeApps.NetworkFolder;
using HP.ScalableTest.PluginSupport.Connectivity.Dhcp;
using HP.ScalableTest.PluginSupport.Connectivity.Discovery;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.NetworkSwitch;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Properties;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using HP.ScalableTest.Utility;
using Sirius = HP.ScalableTest.PluginSupport.Connectivity.Printer.SiriusPrinter;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    /// <summary>
    /// Tells IP address type
    /// </summary>
    public enum IPAddressType
    {
        /// <summary>
        /// IPv4 Address
        /// </summary>
        IPv4,

        /// <summary>
        /// State full IPv6 address
        /// </summary>
        StatefullIPv6,

        /// <summary>
        /// State less IPv6 address
        /// </summary>
        StatelessIPv6,

        /// <summary>
        /// Link Local IPv6 address
        /// </summary>
        LinkLocalIPv6,

        /// <summary>
        /// Non Link Local IPv6 address
        /// </summary>
        NonLinkLocalIPv6
    }

    /// <summary>
    /// The parameters required to perform cold reset and any operations after the cold reset.
    /// </summary>
    public class ColdResetParameters
    {
        /// <summary>
        /// The family where the printer belongs to.
        /// <see cref="PrinterFamilies"/>
        /// </summary>
        public PrinterFamilies PrinterFamily { get; set; }

        /// <summary>
        /// The IP address of the printer.
        /// </summary>
        public IPAddress IpAddress { get; set; }

        /// <summary>
        /// The printer Connectivity. <see cref="ConnectivityType"/>
        /// </summary>
        public ConnectivityType PrinterConnectivityType { get; set; }

        /// <summary>
        /// The Wired mac address of the printer.
        /// </summary>
        public string MacAddress { get; set; }

        /// <summary>
        /// Flag to decide whether to set advanced options after cold reset.
        /// </summary>
        public bool SetAdvancedOptions { get; set; }

        /// <summary>
        /// The ip address of the switch where the printer is connected.
        /// </summary>
        public IPAddress switchIpAddress { get; set; }

        /// <summary>
        /// The port number where the printer is connected in the switch.
        /// </summary>
        public int PortNumber { get; set; }

        /// <summary>
        /// The SSID provided by the wireless access point.
        /// </summary>
        public string WirelessSSID { get; set; }

        /// <summary>
        /// The wireless mac address of the printer. This is used by TPS printers when the printer connectivity is wireless.
        /// When cold reset is performed printer loses the wireless IP address and acquires wired IP address only when the port is enabled.
        /// </summary>
        public string WirelessMacAddress { get; set; }

        /// <summary>
        /// The DHCP server IP address which provides IP for the printer.
        /// This is required in situations where the printer is in auto ip and EWS needs to be accessed and client is not in auto IP.
        /// Dhcp server address will be used to bring down the server and make the client to acquire auto IP.
        /// </summary>
        public string DhcpServerIpAddress { get; set; }

        public ColdResetParameters()
        {
            PrinterFamily = PrinterFamilies.VEP;
            IpAddress = IPAddress.None;
            PrinterConnectivityType = ConnectivityType.Wired;
            MacAddress = string.Empty;
            SetAdvancedOptions = false;
            switchIpAddress = IPAddress.None;
            PortNumber = 0;
            WirelessSSID = string.Empty;
            WirelessMacAddress = string.Empty;
        }
    }

    /// <summary>
    /// Different Printer Accessibility
    /// </summary>
    public enum PrinterAccessType
    {
        /// <summary>
        /// Web UI
        /// </summary>
        EWS,
        /// <summary>
        /// Telnet
        /// </summary>
        Telnet,
        /// <summary>
        /// Snmp
        /// </summary>
        SNMP,
        ControlPanel
    }

    /// <summary>
    /// Utility contains the methods which can be used across the plug-ins
    /// </summary>
    public static class CtcUtility
    {
        #region Constants

        const int SHORT_TIMEOUT_TPS_IWS_LFP = 30;
        const string AUTOIP_FORMAT = "169.254";
        const string FIREWALL_RULE_PREFIX = "netsh advfirewall consec";
        const int MAINMODE_LIFETIME = 480;

        #endregion

        #region Public Constants

        /// <summary>
        /// User name credential for the server.
        /// </summary>
        public const string SERVER_USERNAME = "administrator";

        /// <summary>
        /// Password credential for the server.
        /// </summary>
        public const string SERVER_PASSWORD = "1iso*help";

        /// <summary>
        /// Domain name of the server.
        /// </summary>
        public const string SERVER_DOMAIN = "WORKGROUP";

        #endregion

        #region Local Variables

        private static Collection<PingStatics> _continuousPingStatistics;
        private static string IDCERTIFICATEPATH = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\IPPS\ID\SPIJetdirectCert.pfx");
        private static string CACERTIFICATEPATH = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\IPPS\CA\SPIRootCA.cer");
        private const string IDCERTIFICATE_PSWD = "JetDirect";

        #endregion

        #region Public Methods

        /// <summary>
        /// Get TCP/IP connection timeout for VEP devices
        /// </summary>
        /// <param name="printerIp">IP Address of the device</param>
        /// <returns>timeout in seconds</returns>
        public static int GetTCPIPConnectionTimeout(string printerIp)
        {
            // TODO: Implement for TPS products. Currently available only for VEP products

            TelnetLib protocolTelnet = null;

            try
            {
                int timeOut = 0;

                protocolTelnet = new TelnetLib(printerIp);

                if (!protocolTelnet.Connect())
                {
                    TraceFactory.Logger.Fatal("Telnet failed!!");
                    return timeOut;
                }

                protocolTelnet.SendLine("advanced");
                protocolTelnet.SendLine("/");

                protocolTelnet.ReceiveFile("Press RETURN to continue:");

                // splitData will have the data similar to "270 seconds"
                string[] splitData = protocolTelnet.getValue("Idle Timeout").Split(' ');

                var timeout = splitData[0];

                if (Int32.TryParse(timeout, out timeOut))
                {
                    return timeOut;
                }
                else
                {
                    TraceFactory.Logger.Info("Unable to fetch TCPIP Connection timeout.");
                    return timeOut;
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug(ex.Message);
                throw new InvalidOperationException();
            }
            finally
            {
                protocolTelnet.Dispose();
            }
        }

        /// <summary>
        /// Performs hose break
        /// </summary>
        /// <param name="printerIp">IP Address of the printer.</param>
        /// <param name="portNumber">Port Number to which the printer is connected.</param>
        /// <param name="productFamily">Product family of the printer.</param>
        /// <param name="networkSwitch"><see cref="PrinterFamilies"/></param>
        /// <param name="timeout">Time to wait after hose break is performed.</param>
        /// <returns>returns true if success</returns>
        public static bool PerformHoseBreak(string printerIp, int portNumber, PrinterFamilies productFamily, INetworkSwitch networkSwitch, int timeout = 0) //TODO: printerFamilies can be removed after checking
        {
            if (null == networkSwitch)
            {
                TraceFactory.Logger.Info("INetworkSwitch instance cannot be null");
                return false;
            }

            if (timeout == 0)
            {
                // If the product family is VEP, get timeout using telnet
                // Long: tcptimeout * 1.5, Short: tcptimeout / 2
                if (productFamily.Equals(PrinterFamilies.VEP))
                {
                    int tcpTimeOut = GetTCPIPConnectionTimeout(printerIp);

                    if (tcpTimeOut.Equals(0))
                    {
                        TraceFactory.Logger.Info("Failed to get TCP/IP connection timeout from telnet");
                        return false;
                    }

                    timeout = tcpTimeOut / 2;

                }
                else
                {
                    timeout = SHORT_TIMEOUT_TPS_IWS_LFP;
                }
            }

            // Perform Hose Break
            //INetworkSwitch switchObject = SwitchFactory.Create(IPAddress.Parse(switchIP));
            TraceFactory.Logger.Info("Performing Hose break");
            networkSwitch.DisablePort(portNumber);

            if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(printerIp), TimeSpan.FromSeconds(20)))
            {
                TraceFactory.Logger.Info("Connection to Printer is lost.");
            }
            else
            {
                TraceFactory.Logger.Info("Connection to printer is NOT lost after 1 minutes.");
                return false;
            }

            // Wait for timeout
            Thread.Sleep(timeout);
            TraceFactory.Logger.Info("Waiting for {0} seconds for hose break.".FormatWith(timeout));

            // Enable Switch port
            TraceFactory.Logger.Info("Printer is connected back.");
            networkSwitch.EnablePort(portNumber);

            if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(printerIp), TimeSpan.FromMinutes(2)))
            {
                TraceFactory.Logger.Info("Connection to Printer is established.");
                return true;
            }
            TraceFactory.Logger.Info("Connection to printer is NOT established after 2 minutes.");
            return false;
        }

        /// <summary>
        /// Performs hose break
        /// </summary>
        /// <param name="printerIpAddress">IP Address of the printer.</param>
        /// <param name="switchIpAddress">The switch IP Address.</param>
        /// <param name="portNumber">Port Number on the switch to which the printer is connected.</param>
        /// <param name="timeout">Time in seconds to wait after hose break is performed.</param>
        /// <returns>returns true if success</returns>
        public static bool PerformHoseBreak(string printerIpAddress, string switchIpAddress, int portNumber, int timeout = 0)
        {
            // Perform Hose Break
            TraceFactory.Logger.Info("switch addres :{0}".FormatWith(switchIpAddress));

            INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(switchIpAddress));

            TraceFactory.Logger.Info("Performing Hose break");

            if (!networkSwitch.DisablePort(portNumber))
            {
                TraceFactory.Logger.Info("Failed to disable the switch port.");
                return false;
            }

            if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(printerIpAddress), TimeSpan.FromSeconds(20)))
            {
                TraceFactory.Logger.Info("Connection to Printer is lost due to hose break --Expected.");
            }
            else
            {
                TraceFactory.Logger.Info("Connection to printer is NOT lost after host break.");
                return false;
            }

            TraceFactory.Logger.Info("Waiting for {0} seconds for hose break.".FormatWith(timeout));
            // Wait for timeout
            Thread.Sleep(timeout);

            // Enable Switch port
            TraceFactory.Logger.Info("Connecting Printer back by enabling the port.");
                      
            if (!networkSwitch.EnablePort(portNumber))
            {
                TraceFactory.Logger.Info("Failed to enable the switch port.");
                return false;
            }

            if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(printerIpAddress), TimeSpan.FromMinutes(2)))
            {
                TraceFactory.Logger.Info("Connection to Printer is established -- After Hose break.");
                return true;
            }
            TraceFactory.Logger.Info("Connection to printer is NOT established after 2 minutes - hose break.");
            return false;
        }

        /// <summary>
        /// Performs hose break across different networks.
        /// </summary>
        /// <param name="printerMacAddress">The MAC Address of the printer.</param>
        /// <param name="switchIpAddress">The IP Address of the switch where the printer is connected.</param>
        /// <param name="portNumber">Port number to which the printer is connected.</param>
        /// <param name="destinationDhcpServerIp">The destination DHCP Server where the printer will be connected.</param>
        /// <param name="acquiredIpAddress">The IP Address acquired by the printer after hose break.</param>
        /// <returns>True for success, else false.</returns>
        public static bool PerformHoseBreakAcrossNetworks(string switchIpAddress, int portNumber, string destinationDhcpServerIp, string printerMacAddress, out string acquiredIpAddress)
        {
            acquiredIpAddress = string.Empty;

            if (!PerformHoseBreakAcrossNetworks(switchIpAddress, portNumber, destinationDhcpServerIp))
            {
                return false;
            }

            if (!ValidateHoseBreakAcrossNetworks(destinationDhcpServerIp, printerMacAddress, ref acquiredIpAddress))
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// Performs hose break across different networks.
        /// </summary>
        /// <param name="switchIpAddress">The IP Address of the switch where the printer is connected.</param>
        /// <param name="portNumber">Port number to which the printer is connected.</param>
        /// <param name="destinationDhcpServerIp">The destination DHCP Server where the printer will be connected.</param>
        /// <returns>True for success, else false.</returns>
        public static bool PerformHoseBreakAcrossNetworks(string switchIpAddress, int portNumber, string destinationDhcpServerIp)
        {
            TraceFactory.Logger.Info("Disconnecting the printer from source dhcp Server network");

            INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(switchIpAddress));

            // Disable the port on first VLAN
            if (networkSwitch.DisablePort(portNumber))
            {
                TraceFactory.Logger.Info("Printer port: {0} is disconnected from the source dhcp server network.".FormatWith(portNumber));
            }
            else
            {
                TraceFactory.Logger.Info("Failed to disconnect the printer port: {0} from source dhcp server network.".FormatWith(portNumber));
                return false;
            }

            int destinationVlan = (from vlan in networkSwitch.GetAvailableVirtualLans()
                                   where (null != vlan.IPAddress) && vlan.IPAddress.IsInSameSubnet(IPAddress.Parse(destinationDhcpServerIp))
                                   select vlan.Identifier).FirstOrDefault();

            if (destinationVlan == 0)
            {
                TraceFactory.Logger.Info("No VLAN is found in the {0} network.".FormatWith(destinationDhcpServerIp));
                return false;
            }

            // Change the port to second VLAN 
            if (!networkSwitch.ChangeVirtualLan(portNumber, destinationVlan))
            {
                return false;
            }

            TraceFactory.Logger.Info("Connecting the printer port: {0} in destination dhcp server : {1} network.".FormatWith(portNumber, destinationDhcpServerIp));
            // Enable Port on second VLAN
            if (networkSwitch.EnablePort(portNumber))
            {
                TraceFactory.Logger.Info("Printer port : {0} is connected to destination dhcp server : {1} network.".FormatWith(portNumber, destinationDhcpServerIp));
            }
            else
            {
                TraceFactory.Logger.Info("Failed to connect printer port: {0} to destination dhcp server : {1} network.".FormatWith(portNumber, destinationDhcpServerIp));
                return false;
            }

            TraceFactory.Logger.Info("Waiting for 6 minute after performing hose break.");
            // Wait for 3 minute after performing hose break.
            Thread.Sleep(TimeSpan.FromMinutes(6));

            return true;
        }

        /// <summary>
        /// Validate Hose break across networks.
        /// </summary>
        /// <param name="destinationDhcpServer">The DHCP server providing IP for the printer before hose break. </param>
        /// <param name="macAddress">The MAC address of the printer.</param>
        /// <param name="printerIpAddress">The new IP Address acquired by the printer after hose break.</param>
        /// <returns>True if hose break is successful, else false.</returns>
        public static bool ValidateHoseBreakAcrossNetworks(string destinationDhcpServer, string macAddress, ref string printerIpAddress)
        {
            TraceFactory.Logger.Info("Validating hose break.");

            // Discover the printer with Mac address and fetch the IP Address
            // Discovering the printer is done twice as it will not be discovered sometimes after network hose break.
            if (string.IsNullOrEmpty(printerIpAddress) && string.IsNullOrEmpty(printerIpAddress = GetPrinterIPAddress(macAddress)))
            {
                // Wait for 3 minutes for the printer to acquire IP from the DHCP Server.
                Thread.Sleep(TimeSpan.FromMinutes(1));
                printerIpAddress = GetPrinterIPAddress(macAddress);
            }

            // Validate that the IPAddress is from the second DHCP Server
            Printer.Printer printer = PrinterFactory.Create(PrinterFamilies.VEP, IPAddress.Parse(printerIpAddress));

            // Ping and validate the new IP Address
            if (printer.PingUntilTimeout(IPAddress.Parse(printerIpAddress), TimeSpan.FromMinutes(3)))
            {
                TraceFactory.Logger.Info("Ping successful with the IP Address : {0} acquired after hose break.".FormatWith(printerIpAddress));
            }
            else
            {
                TraceFactory.Logger.Info("Ping failed with the IP Address : {0} acquired after hose break.".FormatWith(printerIpAddress));
                return false;
            }

            if (!printerIpAddress.StartsWith(AUTOIP_FORMAT, StringComparison.CurrentCultureIgnoreCase))
            {
                if (IPAddress.Parse(destinationDhcpServer).IsInSameSubnet(IPAddress.Parse(printerIpAddress)))
                {
                    TraceFactory.Logger.Info("Printer has acquired IP from the DHCP Server : {0}".FormatWith(destinationDhcpServer));
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to acquire new IP Address after hose break across different networks.");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Encrypting the value using RSA Cryptography Algorithm using Public Key 
        /// </summary>
        /// <param name="deviceAddress">The security public key of the device</param>
        /// <param name="value">Pass phrase/WEP Key</param>
        /// returns encrypted Pass phrase in Bytes
        public static byte[] Encrypt(string publicKey, string value)
        {
            TraceFactory.Logger.Info($"Encrypting the value:{value} with public Key: {publicKey}");

            byte[] cipherAsBytes;

            try
            {
                byte[] pubicKeyInBytes = ConvertHexStringToByteArray(publicKey);

                Oid oid = new Oid("1.2.840.113549.1.1.1");  // OID which represents the public key format

                AsnEncodedData asnEncodedPublicKey = new AsnEncodedData(pubicKeyInBytes);

                AsnEncodedData keyParam = new AsnEncodedData(new byte[] { 05, 00 });

                /*Using the ASN.1 encoded public key and OID construct a PublicKey object. 
				*When constructed this object will have all the information like exponent, modulus, key length etc 
				* placed in relevant fields. This information is extracted from the asn.1 encoded public key passed 
				*/
                PublicKey pub = new PublicKey(oid, keyParam, asnEncodedPublicKey);

                /*Extract the RSACryptoServiceProvider from Public key. This RSACryptoServiceProvider will have all the relevant information
				*like modulus, exponent in RSAParameters. An Object of RSACryptoServiceProvider can be used to encrypt data
				*/
                RSACryptoServiceProvider rsaKey = (RSACryptoServiceProvider)pub.Key;

                var cipher = RSAEncryption(rsaKey, value);
                cipherAsBytes = ConvertHexStringToByteArray(cipher);

                TraceFactory.Logger.Info("The Pass phrase/WEP Key:{0} is encrypted".FormatWith(value));
                return cipherAsBytes;
            }
            catch (FormatException exception)
            {
                TraceFactory.Logger.Info("Exception in Encrypt block" + exception.Message);
                throw new FormatException(exception.Message);
            }
        }

        /// <summary>
        /// RSAEncryption
        /// </summary>
        /// <param name="rsaKey"></param>
        /// <param name="passPhrase"></param>
        /// <returns></returns>
        public static string RSAEncryption(RSACryptoServiceProvider rsaKey, string passPhrase)
        {
            byte[] passphareInBytes = Encoding.UTF8.GetBytes(passPhrase);
            //Encrypt the data            
            byte[] cipherInBytes = rsaKey.Encrypt(passphareInBytes, false);
            string cipherText = BitConverter.ToString(cipherInBytes);
            cipherText = cipherText.Replace("-", string.Empty);
            return cipherText;
        }

        /// <summary>
        /// ConvertHexStringToByteArray
        /// </summary>
        /// <param name="hexValue"></param>
        /// <returns></returns>
        public static byte[] ConvertHexStringToByteArray(string hexValue)
        {
            if (string.IsNullOrEmpty(hexValue))
            {
                TraceFactory.Logger.Info("The hex string to be converted can not be empty.");
                return new byte[0];
            }

            if (hexValue.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexValue));
            }

            byte[] HexAsBytes = new byte[hexValue.Length / 2];
            for (int index = 0; index < HexAsBytes.Length; index++)
            {
                string byteValue = hexValue.Substring(index * 2, 2);
                try
                {
                    HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Info("Failed to convert Hex string '{0}' to byte array".FormatWith(hexValue, CultureInfo.CurrentCulture));
                    TraceFactory.Logger.Error(ex.Message + " Should be a HEX string." + Environment.NewLine + Environment.NewLine + "String: " + hexValue);  // catching and throwing again in order to pass the hexString info
                }

            }

            return HexAsBytes;
        }

        /// <summary>
        /// Creates a text file and returns the Folder Path
        /// <param name="data">Data to be written in temp file</param>		
        /// </summary>
        /// <returns>file location</returns>
        public static string CreateFile(string data)
        {
            // creating folder inside the Temp directory to make to separate
            string tempDirectory = Path.GetTempPath() + "\\TempFolder";
            Directory.CreateDirectory(tempDirectory);
            string filePath = tempDirectory + "\\TestStartPage.txt";

            FileStream stream = null;

            try
            {
                stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                using (StreamWriter writer = new StreamWriter(stream))
                {
                    if (File.Exists(filePath))
                    {
                        writer.Write(data);
                    }
                }
            }
            catch (IOException ex)
            {
                TraceFactory.Logger.Debug("Exception : ".FormatWith(ex.Message));
                filePath = string.Empty;
            }
            finally
            {
                if (null != stream)
                {
                    stream.Dispose();
                }
            }

            return filePath;
        }

        /// <summary>
        /// Add Static Source IP Address to Local Machine
        /// </summary>
        /// <param name="dhcpServerIpAddress">DHCP Server IP Address(The source IP Address)</param>
        /// <param name="routingIpAddress">The routing IP address.</param>
        public static void AddSourceIPAddress(string dhcpServerIpAddress, string routingIpAddress)
        {
            IPAddress[] ipAddresses = NetworkUtil.GetLocalAddresses();

            var diffNetworkIpAddress = (from ipAddress in ipAddresses
                                        where ipAddress.AddressFamily.Equals(AddressFamily.InterNetwork) && ipAddress.IsInSameSubnet(IPAddress.Parse(routingIpAddress))
                                        select ipAddress.ToString()).FirstOrDefault();

            ProcessUtil.Execute("cmd.exe", "/C ping {0} -S {1}".FormatWith(dhcpServerIpAddress, diffNetworkIpAddress));
        }

        /// <summary>
        /// Check if Local Client machine is configured with Secondary DHCP Server and Linux Server
        /// </summary>
        /// <param name="secondaryDhcpServerIp">Secondary DHCP Server IP Address</param>
        /// <param name="linuxServerIp">Linux Server IP Address</param>
        /// <returns>true if client is configured with both server, false otherwise</returns>
        public static bool IsClientConfiguredWithServerIP(string secondaryDhcpServerIp, string linuxServerIp)
        {
            bool secondaryServerIpConfigured = false;
            bool linuxServerIpConfigured = false;
            IPAddress[] localMachineIpAddresses = NetworkUtil.GetLocalAddresses();

            foreach (IPAddress ipAddress in localMachineIpAddresses)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork && ipAddress.IsInSameSubnet(IPAddress.Parse(secondaryDhcpServerIp)))
                {
                    secondaryServerIpConfigured = true;
                    TraceFactory.Logger.Info("Client's has Secodary server IP as expected. ");
                }
                else if (ipAddress.AddressFamily == AddressFamily.InterNetwork && ipAddress.IsInSameSubnet(IPAddress.Parse(linuxServerIp)))
                {
                    linuxServerIpConfigured = true;
                    TraceFactory.Logger.Info("Client has Linux server IP as expected. ");
                }
            }
            TraceFactory.Logger.Info("SecondaryIPconfigured - {0}, LiniIPconfigured - {0}  ".FormatWith(secondaryServerIpConfigured , linuxServerIpConfigured));
            return secondaryServerIpConfigured && linuxServerIpConfigured;
        }

        // TODO: Call this method where multiple DHCP server availability is checked
        /// <summary>
        /// Check if Local Client machine is configured with DHCP Server
        /// </summary>
        /// <param name="secondaryDhcpServerIp">The DHCP Server IP Address</param>
        /// <returns>true if client is configured with the server, false otherwise</returns>
        public static bool IsClientConfiguredWithServerIP(string secondaryDhcpServerIp)
        {
            IPAddress[] localMachineIpAddresses = NetworkUtil.GetLocalAddresses();

            return localMachineIpAddresses.Any(ipAddress => ipAddress.AddressFamily == AddressFamily.InterNetwork && ipAddress.IsInSameSubnet(IPAddress.Parse(secondaryDhcpServerIp)));
        }

        /// <summary>
        /// Release Local Machine IP configuration
        /// </summary>
        public static void ReleaseLocalMachineIP()
        {
            TraceFactory.Logger.Debug("Releasing Local Machine IP Address.");
            ProcessUtil.Execute("cmd.exe", "/C ipconfig /release");
        }

        /// <summary>
        /// Check whether printer with macAddress has acquired AutoIP or Legacy IP.
        /// </summary>
        /// <param name="macAddress">MAC address of printer</param>
        /// <param name="ipAddress">The ip address.</param>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="defaultType"><see cref="DefaultIPType" /></param>
        /// <returns>true if printer matching MAC Address acquired Auto IP/ Legacy IP, false otherwise</returns>
        public static bool IsPrinterInDefaultIP(string macAddress, out string ipAddress, out string hostName, out DefaultIPType defaultType)
        {
            ipAddress = string.Empty;
            hostName = string.Empty;

            // Discovering with Discovery Option All, reason is if SLP mode is disabled it will discover with WSDv4 and WSDv6
            BacaBodWrapper.DiscoverDevice(macAddress, BacaBodSourceType.MacAddress, BacaBodDiscoveryType.All, ref ipAddress, ref hostName);

            return IsPrinterInDefaultIP(ipAddress, out defaultType);
        }

        /// <summary>
        /// Checks if the specified IP address is default IP.
        /// </summary>
        /// <param name="ipAddress">The IP address to be verified</param>
        /// <param name="defaultType"><see cref="DefaultIPType"/></param>
        /// <returns></returns>
        public static bool IsPrinterInDefaultIP(string ipAddress, out DefaultIPType defaultType)
        {
            IPAddress address;
            defaultType = DefaultIPType.None;

            if (IPAddress.TryParse(ipAddress, out address))
            {
                if (address.IsAutoIP())
                {
                    defaultType = DefaultIPType.AutoIP;
                    TraceFactory.Logger.Info("IP address: {0} is auto IP.".FormatWith(ipAddress));
                    return true;
                }
                if (address.Equals(Printer.Printer.LegacyIPAddress))
                {
                    defaultType = DefaultIPType.LegacyIP;
                    TraceFactory.Logger.Info("IP address: {0} is legacy IP.".FormatWith(ipAddress));
                    return true;
                }
                // Could be DHCP / BootP IP
                TraceFactory.Logger.Info("IP address: {0} is not auto IP or legacy IP.".FormatWith(ipAddress));
                return false;
            }
            TraceFactory.Logger.Info("Invalid IP address.");
            return false;
        }

        public static bool IsPrinterInDefaultIP(string address)
        {
            DefaultIPType defaultType;
            return IsPrinterInDefaultIP(address, out defaultType);
        }

        /// <summary>
        /// Check whether printer with macAddress has acquired AutoIP
        /// </summary>
        /// <param name="macAddress">MAC address of printer</param>		
        /// <param name="autoIpAddress">Auto IP Address of the printer</param>
        /// <returns>true if printer matching MAC Address acquired Auto IP, false otherwise</returns>
        public static bool IsPrinterInAutoIP(string macAddress, out string autoIpAddress)
        {
            autoIpAddress = GetPrinterIPAddress(macAddress);

            IPAddress address;

            if (IPAddress.TryParse(autoIpAddress, out address))
            {
                return address.IsAutoIP();
            }
            return false;
        }

        /// <summary>
        /// Check whether printer with macAddress has acquired DefaultIP
        /// </summary>
        /// <param name="macAddress">MAC address of printer</param>		
        /// <param name="autoIpAddress">Auto IP Address of the printer</param>
        /// <returns>true if printer matching MAC Address acquired Auto IP, false otherwise</returns>
        public static bool IsPrinterInDefaultIP(string macAddress, out string autoIpAddress)
        {
            autoIpAddress = GetPrinterIPAddress(macAddress);

            IPAddress address;

            if (IPAddress.TryParse(autoIpAddress, out address))
            {
                return address.IsDefaultIP();
            }
            return false;
        }

        /// <summary>
        /// Get Printer IP Address using Discovery based on MAC address of the printer
        /// </summary>
        /// <param name="macAddress">MAC address of the printer</param>
        /// <returns>IP Address of the printer if MAC address is matching with discovered devices, empty string otherwise</returns>
        public static string GetPrinterIPAddress(string macAddress)
        {
            string ipAddress = string.Empty;
            string hostName = string.Empty;

            // Discovering with Discovery Option All, reason is if SLP mode is disabled it will discover with WSDv4 and WSDv6
            BacaBodWrapper.DiscoverDevice(macAddress, BacaBodSourceType.MacAddress, BacaBodDiscoveryType.All, ref ipAddress, ref hostName, CastType.Unicast);

            // Discover Again in case not discovered
            if (string.IsNullOrEmpty(ipAddress))
            {
                TraceFactory.Logger.Debug("Printer not discovered. Discovering again..");
                BacaBodWrapper.DiscoverDevice(macAddress, BacaBodSourceType.MacAddress, BacaBodDiscoveryType.All, ref ipAddress, ref hostName, CastType.Unicast);
            }

            return ipAddress;
        }

        /// <summary>
        /// Generate IPv6 Link local address using Printer Mac Address
        /// </summary>
        /// <param name="macAddress">Mac Address of printer</param>
        /// <returns>Link Local Address</returns>
        public static IPAddress GetLinkLocalAddress(string macAddress)
        {
            // usage
            // --------------------------------------------
            // mac address		-	02:bc:2b:57:fe:fb
            // ipv6 link local	-	FE80::bc:2bFF:FE57:fefb
            // ---------------------------------------------
            // Link local address => FE80:(02 ^ 2):bc:eb:FF:FE:57:fe:fb =>	 (02 ^ 2) - (1st element XOR with numeric 2)

            if (!macAddress.Contains(':'))
            {
                macAddress = Regex.Replace(macAddress, ".{2}", "$0:");
                macAddress = macAddress.Remove(macAddress.Length - 1, 1);
            }

            string linkLocalAddressFormat = "FE80::{0}:{1}FF:FE{2}:{3}";
            string[] mac = macAddress.Split(':');

            string element = (Convert.ToInt32(mac[0], 16) ^ 2).ToString("X");

            return IPAddress.Parse(linkLocalAddressFormat.FormatWith(string.Concat(element, mac[1]), mac[2], mac[3], string.Concat(mac[4], mac[5])));
        }

        /// <summary>
        /// Gets a random generated LAA value
        /// </summary>
        /// <returns>The random LAA value</returns>
        public static string GetLaa()
        {
            return "02{0}".FormatWith(Guid.NewGuid().ToString("N").Substring(0, 10));
        }

        /// <summary>
        /// Starts service on machine
        /// </summary>
        /// <param name="serverAddress">IP Address of server on which the service needs to be started, empty for local machine</param>
        /// <param name="serviceName">Service name to be started</param>
        /// <returns>true if the specified service is started, false otherwise</returns>
        public static bool StartService(string serviceName, string serverAddress)
        {
            // Impersonating the user as the servers are on different domain
            using (UserImpersonator localUser = new UserImpersonator())
            {
                localUser.Impersonate(SERVER_USERNAME, SERVER_DOMAIN, SERVER_PASSWORD);
                return SystemConfiguration.SystemConfiguration.StartService(serviceName, serverAddress);
            }
        }

        /// <summary>
        /// Check if service is running
        /// </summary>
        /// <param name="serverAddress">IP Address of server on which the service needs to be started, empty for local machine</param>
        /// <param name="serviceName">Service name to be started</param>
        /// <returns>true if Service is running, false otherwise</returns>
        public static bool IsServiceRunning(string serviceName, string serverAddress)
        {
            // Impersonating the user as the servers are on different domain
            using (UserImpersonator localUser = new UserImpersonator())
            {
                localUser.Impersonate(SERVER_USERNAME, SERVER_DOMAIN, SERVER_PASSWORD);
                return SystemConfiguration.SystemConfiguration.IsServiceRunning(serviceName, serverAddress);
            }
        }

        /// <summary>
        /// Check whether the printer ip address ,dhcp server ipaddress and switch ipaddress are accessible
        /// </summary>
        /// <returns>true if accessible, false otherwise</returns>
        public static void CheckPrinterConnectivity(string primaryPrinterAddress, string primaryDhcpServerIPaddress,
                                              string secondaryPrinterAddress = null, string secondaryDhcpServerIPaddress = null,
                                              string linuxServerIPaddress = null, string switchIpAddress = null)
        {
            // Check if Primary Printer ipaddress and Primary DHCP Server is accessible
            if (!(NetworkUtil.PingUntilTimeout(IPAddress.Parse(primaryPrinterAddress), TimeSpan.FromSeconds(20))) &&
                (NetworkUtil.PingUntilTimeout(IPAddress.Parse(primaryDhcpServerIPaddress), TimeSpan.FromSeconds(20))))
            {
                MessageBox.Show(string.Concat("Primary Printer IP Address or Primary DHCP Server is not accessible \n\n",
                    "Make sure you have provided valid Primary Printer IP Address and DHCP Server and is accessible.\n"),
                    @"IP Address Not Accessible", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!(string.IsNullOrEmpty(secondaryPrinterAddress)) && !string.IsNullOrEmpty(secondaryDhcpServerIPaddress))
            {
                // Check if Primary Printer ipaddress and Primary DHCP Server is accessible
                if (
                    !(NetworkUtil.PingUntilTimeout(IPAddress.Parse(secondaryPrinterAddress), TimeSpan.FromSeconds(20))) &&
                    (NetworkUtil.PingUntilTimeout(IPAddress.Parse(secondaryDhcpServerIPaddress),
                        TimeSpan.FromSeconds(20))))
                {
                    MessageBox.Show(
                        string.Concat("Primary Printer IP Address or Primary DHCP Server is not accessible \n\n",
                            "Make sure you have provided valid Primary Printer IP Address and DHCP Server and is accessible.\n"),
                            @"IP Address Not Accessible", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!string.IsNullOrEmpty(linuxServerIPaddress))
                {
                    // Check if linux ip address is accessible
                    if (!(NetworkUtil.PingUntilTimeout(IPAddress.Parse(linuxServerIPaddress), TimeSpan.FromSeconds(20))))
                    {
                        MessageBox.Show(
                            string.Concat("Linux Server IP Address is not accessible \n\n",
                                "Make sure you have provided valid linux serverr IP Address and is accessible.\n"),
                                @"Linux Server IP Address Not Accessible", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!IsClientConfiguredWithServerIP(secondaryDhcpServerIPaddress, linuxServerIPaddress))
                    {
                        MessageBox.Show(string.Concat("Client is not configured with Server IP Address.\n\n",
                            "Check if Client has acquired IPv4 Address from Secondary DHCP Server.\n",
                            "Check if Client has acquired IPv4 Address from Linux Server."),
                            @"Client machine failed to configure ipaddress ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        /// <summary>
        /// Checks whether EWS, Telnet and SNMP are accessible for the printer.
        /// If not accessible, a pop up will be displayed asking the user to correct the printer.
        /// </summary>
        /// <param name="PrinterIPAddress">Printer IP Address</param>
        /// <param name="productFamily">Printer Family</param>
        /// <returns></returns>
        public static bool CheckPrinterAccessiblity(string productFamily, string PrinterIPAddress)
        {
            Printer.Printer printer = PrinterFactory.Create(productFamily, PrinterIPAddress);

            IPAddress printerIpAddress = IPAddress.Parse(PrinterIPAddress);

            // Check if EWS, Telnet and SNMP are accessible.
            //bool isPrinterAccessible = (printer.IsEwsAccessible(printerIpAddress) && printer.IsTelnetAccessible(printerIpAddress) && printer.IsSnmpAccessible(printerIpAddress));
            //CTSS
            bool isPrinterAccessible = (printer.IsEwsAccessible(printerIpAddress) && printer.IsSnmpAccessible(printerIpAddress));

            // Check if EWS, Telnet and SNMP are accessible.
            while (!isPrinterAccessible)
            {
                var continueTest = ShowErrorPopup("Printer is currently not accessible. \nPlease cold reset the printer");

                if (!continueTest)
                {
                    return false;
                }

                isPrinterAccessible = (printer.IsEwsAccessible(printerIpAddress) && printer.IsTelnetAccessible(printerIpAddress) && printer.IsSnmpAccessible(printerIpAddress));
            }

            return true;
        }

        /// <summary>
        /// Gets the printer FQDN (Fully qualified domain name) address
        /// </summary>
        /// <returns>FQDN address</returns>
        public static string GetFqdn()
        {
            return "{0}.{1}".FormatWith(EwsWrapper.Instance().GetHostname(), EwsWrapper.Instance().GetDomainName());
        }

        /// <summary>
        /// Displays the error popup.
        /// </summary>
        /// <returns>True if Retry is clicked, else false.</returns>
        public static bool ShowErrorPopup(string errorMessage)
        {
            DialogResult result = MessageBox.Show(errorMessage + @"Click Retry to continue or Cancel to ignore.", @"Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);

            if (result == DialogResult.Retry)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Writes the step with delimeter
        /// </summary>
        /// <returns>step with delimeter</returns>
        public static string WriteStep(string description)
        {
            return "{0} {1} {2}".FormatWith(CtcBaseTests.STEP_DELIMETER, description, CtcBaseTests.STEP_DELIMETER);
        }

        /// <summary>
        /// Gets a unique host name.
        /// </summary>
        /// <returns>The unique host name.</returns>
        public static string GetUniqueHostName()
        {
            // prefixing the alphabet, if incase guid returns all integers.
            // DHCP server will not like if hostname is all numbers. But the printer will accept without any issues.
            return ("a{0}".FormatWith(Guid.NewGuid().ToString().Substring(0, 5))).ToLowerInvariant();
        }

        /// <summary>
        /// Gets a unique domain name.
        /// </summary>
        /// <returns>The unique domain name.</returns>
        public static string GetUniqueDomainName()
        {
            return ("a{0}.com".FormatWith(Guid.NewGuid().ToString().Substring(0, 4))).ToLowerInvariant();
        }

        /// <summary>
        /// Get the Enumerator value based on Product family
        /// </summary>
        /// <param name="value">Enumerator value separated by '||', if there are multiple values within a family, separated by '|'</param>
        /// <param name="family"><see cref=" PrinterFamilies"/></param>
        /// <returns>Enum value</returns>
        public static string GetEnumvalue(string value, PrinterFamilies family)
        {
            string enumValue;

            if (value.Contains("||"))
            {
                string[] splitString = { "||" };
                string[] enumValues = value.Split(splitString, StringSplitOptions.None);

                switch (family)
                {
                    case PrinterFamilies.VEP:
                        enumValue = enumValues[0];
                        break;
                    case PrinterFamilies.TPS:
                        enumValue = enumValues[1];
                        break;
                    case PrinterFamilies.LFP:
                        enumValue = enumValues[2];
                        break;
                    default:
                        enumValue = enumValues[3];
                        break;
                }
            }
            else
            {
                enumValue = value;
            }

            return enumValue;
        }

        /// <summary>
        /// Gets the Enum Type based on the EnumValue passed.
        /// </summary>
        /// <typeparam name="T">The Enum</typeparam>
        /// <param name="value"></param>
        /// <param name="family"></param>
        /// <returns></returns>
        public static T GetEnum<T>(string value, PrinterFamilies family)
        {
            int valueIndex = (int)family;
            Dictionary<object, string> enumValues = Enum<T>.EnumValues;

            string splitSymbol = "||";
            string[] splitString = { "||" };
            // If a value which is not contained in the [EnumValue] attribute is given, null will be retrieved here.
            KeyValuePair<object, string> keyValPair = enumValues.FirstOrDefault(x => !string.IsNullOrEmpty(x.Value) && x.Value.Contains(splitSymbol) && x.Value.Split(splitString, StringSplitOptions.None)[valueIndex].EqualsIgnoreCase(value));


            if (keyValPair.Equals(default(KeyValuePair<object, string>)))
            {
                return default(T);
            }
            //return (T)Enum<T>.Parse(enumValues.Where(x => x.Value.Contains(value)).FirstOrDefault().Value.ToString());
            return Enum<T>.Parse(keyValPair.Value);
        }

        /// <summary>
        /// Create IP Sec rule on local client machine
        /// </summary>
        /// <param name="ruleSettings"><see cref="SecurityRuleSettings"/></param>
        /// <param name="enableRule">true to enable rule, false otherwise</param>
        /// <param name="enableProfiles">true to enable rule, false otherwise</param>
        /// <returns>true if rule is created successfully, false otherwise</returns>
        public static bool CreateIPsecRule(SecurityRuleSettings ruleSettings, bool enableRule = true, bool enableProfiles = true)
        {
            TraceFactory.Logger.Debug("Creating Ipsec rule on client machine.");

            ConfigureExternalSettings(ruleSettings, enableProfiles);

            // Format: add rule name = <rule_name> <address> <service> <ipsec> action = requireinrequireout enable = <yes | no>
            string createRule = FIREWALL_RULE_PREFIX + " add rule name=\"{0}\" {1} {2} {3} action=requireinrequireout enable={4}";

            Collection<string> address = GetAddressRule(ruleSettings.AddressTemplate);
            Collection<string> service = GetServiceRule(ruleSettings.ServiceTemplate);
            string ipsec = GetIPsecrule(ruleSettings.IPsecTemplate);

            bool ruleResult = true;

            foreach (string addressRule in address)
            {
                foreach (string serviceRule in service)
                {
                    string ruleService = serviceRule.Contains(" - ") ? serviceRule.Replace(" - ", ",") : serviceRule;
                    var rule = createRule.FormatWith(ruleSettings.Name, addressRule, ruleService, ipsec, enableRule ? "yes" : "no");
                    TraceFactory.Logger.Debug("Rule: {0}".FormatWith(rule));
                    ruleResult &= NetworkUtil.ExecuteCommandAndValidate(rule);
                }
            }

            if (ruleResult)
            {
                TraceFactory.Logger.Info("Client rule created successfully in local Client.");
            }
            else
            {
                TraceFactory.Logger.Info("Client rule creation failed in Local Client");
                DeleteIPsecRule(ruleSettings);
            }

            return ruleResult;
        }

        /// <summary>
        /// Delete IP sec rule on client machine
        /// </summary>
        /// <param name="ruleSettings"><see cref="SecurityRuleSettings"/></param>
        /// <returns>true if all rules are deleted, false otherwise</returns>
        public static bool DeleteIPsecRule(SecurityRuleSettings ruleSettings)
        {
            // Format: delete rule name = <rule_name> <address> <service>
            string deleteRule = FIREWALL_RULE_PREFIX + " delete rule name = \"{0}\" {1} {2}";

            Collection<string> address = GetAddressRule(ruleSettings.AddressTemplate);
            Collection<string> service = GetServiceRule(ruleSettings.ServiceTemplate);

            bool ruleResult = true;

            foreach (string addressRule in address)
            {
                foreach (string serviceRule in service)
                {
                    deleteRule = deleteRule.FormatWith(ruleSettings.Name, addressRule, serviceRule);
                    ruleResult &= NetworkUtil.ExecuteCommandAndValidate(deleteRule);
                }
            }

            TraceFactory.Logger.Info(ruleResult ? "Rule deleted successfully." : "Rule deletion failed");

            return ruleResult;
        }

        /// <summary>
        /// Deletes all IPsec rules created on client machine
        /// </summary>
        /// <returns></returns>
        public static bool DeleteAllIPsecRules()
        {
            // Before resetting all the firewall options, existing rules are deleted first
            bool executionStatus = NetworkUtil.ExecuteCommandAndValidate("netsh advfirewall consec delete rule name = all");

            ResetFirewallSettings();

            return executionStatus;
        }

        /// <summary>
        /// Disable specified rule on client machine
        /// </summary>
        /// <param name="ruleName">IPsec rule name</param>
        /// <returns></returns>
        public static bool DisableIPsecRule(string ruleName)
        {
            TraceFactory.Logger.Debug("Disabling IPSec Rule in Client");
            return NetworkUtil.ExecuteCommandAndValidate("netsh advfirewall consec set rule name=\"{0}\" new enable=no".FormatWith(ruleName));
        }

        /// <summary>
        /// Enable specified rule on client machine
        /// </summary>
        /// <param name="ruleName">IPsec rule name</param>
        /// <returns>true if rule is enabled otherwise false</returns>
        public static bool EnableIPsecRule(string ruleName)
        {
            TraceFactory.Logger.Debug("Enabling Rule in Client");
            return NetworkUtil.ExecuteCommandAndValidate("netsh advfirewall consec set rule name=\"{0}\" new enable=yes".FormatWith(ruleName));
        }

        /// <summary>
        /// Enable/Disable Firewall public, private and domain profiles
        /// </summary>
        /// <param name="enable">true to turn on, false to turn off</param>
        /// <returns>true if state is changed, false otherwise</returns>
        public static bool SetFirewallProfiles(bool enable = true)
        {
            TraceFactory.Logger.Debug("Setting Public profile state to {0}:".FormatWith(enable ? "on" : "off"));

            string configurationCmd = "netsh advfirewall set public state {0}".FormatWith(enable ? "on" : "off");
            TraceFactory.Logger.Debug("Public profile command: {0}".FormatWith(configurationCmd));
            if (!NetworkUtil.ExecuteCommandAndValidate(configurationCmd))
            {
                return false;
            }

            TraceFactory.Logger.Debug("Setting Private profile state to {0}:".FormatWith(enable ? "on" : "off"));

            configurationCmd = "netsh advfirewall set private state {0}".FormatWith(enable ? "on" : "off");
            TraceFactory.Logger.Debug("Private profile command: {0}".FormatWith(configurationCmd));
            if (!NetworkUtil.ExecuteCommandAndValidate(configurationCmd))
            {
                return false;
            }

            TraceFactory.Logger.Debug("Setting Domain profile state to {0}:".FormatWith(enable ? "on" : "off"));

            configurationCmd = "netsh advfirewall set domain state {0}".FormatWith(enable ? "on" : "off");
            TraceFactory.Logger.Debug("Private profile command: {0}".FormatWith(configurationCmd));

            return NetworkUtil.ExecuteCommandAndValidate(configurationCmd);
        }

        /// <summary>
        /// Set Firewall Public profile to on/off
        /// </summary>
        /// <param name="enable">true to turn on, false to turn off</param>
        /// <returns>true if public profile is turned on/off based on input, false otherwise</returns>
        public static bool SetFirewallPublicProfile(bool enable = true)
        {
            TraceFactory.Logger.Debug("Setting Public profile state to {0}:".FormatWith(enable ? "on" : "off"));

            string configurationCmd = "netsh advfirewall set public state {0}".FormatWith(enable ? "on" : "off");
            TraceFactory.Logger.Debug("Public profile command: {0}".FormatWith(configurationCmd));

            return NetworkUtil.ExecuteCommandAndValidate(configurationCmd);
        }

        /// <summary>
        /// Enable specified Firewall profile(s)
        /// </summary>
        /// <param name="profile"><see cref="FirewallProfile"/></param>
        /// <param name="allowInBound"></param>
        /// <returns>true is profile(s) are enabled</returns>
        public static bool EnableFirewallProfile(FirewallProfile profile, bool allowInBound = false)
        {
            bool result = true;

            if (profile.HasFlag(FirewallProfile.Public))
            {
                TraceFactory.Logger.Debug("Setting Public profile state to on");
                result &= NetworkUtil.ExecuteCommandAndValidate("netsh advfirewall set public state on");
            }

            if (profile.HasFlag(FirewallProfile.Private))
            {
                TraceFactory.Logger.Debug("Setting Private profile state to on");
                result &= NetworkUtil.ExecuteCommandAndValidate("netsh advfirewall set private state on");
            }

            if (profile.HasFlag(FirewallProfile.Domain))
            {
                TraceFactory.Logger.Debug("Setting Domain profile state to on");
                result &= NetworkUtil.ExecuteCommandAndValidate("netsh advfirewall set domain state on");
            }
            if (allowInBound)
            {
                TraceFactory.Logger.Debug("Setting Allow Inbound profile state to allow");
                result &= NetworkUtil.ExecuteCommandAndValidate("netsh advfirewall set allprofiles firewallpolicy allowinbound,allowoutbound");
            }

            return result;
        }

        /// <summary>
        /// Disable specified Firewall profile(s)
        /// </summary>
        /// <param name="profile"><see cref="FirewallProfile"/></param>
        /// <returns>true is profile(s) are disabled</returns>
        public static bool DisableFirewallProfile(FirewallProfile profile)
        {
            bool result = true;

            if (profile.HasFlag(FirewallProfile.Public))
            {
                TraceFactory.Logger.Debug("Setting Public profile state to off");
                result &= NetworkUtil.ExecuteCommandAndValidate("netsh advfirewall set public state off");
            }

            if (profile.HasFlag(FirewallProfile.Private))
            {
                TraceFactory.Logger.Debug("Setting Private profile state to off");
                result &= NetworkUtil.ExecuteCommandAndValidate("netsh advfirewall set private state off");
            }

            if (profile.HasFlag(FirewallProfile.Domain))
            {
                TraceFactory.Logger.Debug("Setting Domain profile state to off");
                result &= NetworkUtil.ExecuteCommandAndValidate("netsh advfirewall set domain state off");
            }

            return result;
        }

        /// <summary>
        /// Validate requested services with specified IP Address.
        /// </summary>
        /// <param name="printer">Printer Object</param>
        /// <param name="ipAddress">Printer IPAddress, if null; will take ipv4 address from printer object</param>
        /// <param name="ping">Ping Service</param>
        /// <param name="telnet">Telnet Service</param>
        /// <param name="snmp">SNMP Service</param>
        /// <param name="snmpSet"></param>
        /// <param name="http">HTTP Service</param>
        /// <param name="https">HTTPS Service</param>
        /// <param name="ftp">FTP Service</param>
        /// <param name="p9100">p9100 print Service</param>
        /// <param name="lpd">LDP print Service</param>
        /// <param name="wsd">WS Discovery Service</param>
        /// <param name="printerDriver">Printer Driver path</param>
        /// <param name="printerDriverModel">Printer Model path</param>
        /// <param name="isMessageBoxChecked"></param>
        /// <param name="snmpGet"></param>
        /// <param name="snmpSetCommunityName"></param>
        /// <param name="isPingRequiredP9100Install"></param>
        /// <returns>true if all requested services are validated successfully, false if any 1 fails</returns>
        public static bool ValidateDeviceServices(Printer.Printer printer, string ipAddress = null,
            DeviceServiceState ping = DeviceServiceState.Skip, DeviceServiceState telnet = DeviceServiceState.Skip, DeviceServiceState snmp = DeviceServiceState.Skip,
            DeviceServiceState snmpGet = DeviceServiceState.Skip, DeviceServiceState snmpSet = DeviceServiceState.Skip,
            DeviceServiceState http = DeviceServiceState.Skip, DeviceServiceState https = DeviceServiceState.Skip,
            DeviceServiceState ftp = DeviceServiceState.Skip, DeviceServiceState p9100 = DeviceServiceState.Skip, DeviceServiceState lpd = DeviceServiceState.Skip,
            DeviceServiceState wsd = DeviceServiceState.Skip, string printerDriver = null, string printerDriverModel = null, bool isMessageBoxChecked = false, string snmpSetCommunityName = null, bool isPingRequiredP9100Install = true)
        {
            // wait for one minute before actually starting the validation, some times validation may not work as expected without delay
            Thread.Sleep(TimeSpan.FromMinutes(4));

            if (isMessageBoxChecked)
            {
                MessageBox.Show("Security rules are created on the Printer and Client, perform validation and click OK to continue for the automation validation.", "Debug Option", MessageBoxButtons.OK);
            }

            bool serviceResult;

            var printerIpAddress = string.IsNullOrEmpty(ipAddress) ? printer.WiredIPv4Address : IPAddress.Parse(ipAddress);

            Collection<string> requestedServices = new Collection<string>();
            Collection<string> expectedResult = new Collection<string>();
            Collection<string> actualResult = new Collection<string>();
            Collection<bool> testResult = new Collection<bool>();

            if (DeviceServiceState.Skip != ping)
            {
                serviceResult = printer.PingUntilTimeout(printerIpAddress, TimeSpan.FromSeconds(20));

                TraceFactory.Logger.Info(serviceResult
                    ? "Ping with {0} IP Address is successful.".FormatWith(printerIpAddress)
                    : "Ping with {0} IP Address is not successful.".FormatWith(printerIpAddress));

                requestedServices.Add("Ping");
                expectedResult.Add(ping.ToString());
                actualResult.Add(serviceResult ? DeviceServiceState.Pass.ToString() : DeviceServiceState.Fail.ToString());
            }
            // Sirius printers doesn't support telnet
            if ((DeviceServiceState.Skip != telnet) && (!(printer is Sirius)))
            {
                serviceResult = printer.IsTelnetAccessible(printerIpAddress);

                requestedServices.Add("Telnet");
                expectedResult.Add(telnet.ToString());
                actualResult.Add(serviceResult ? DeviceServiceState.Pass.ToString() : DeviceServiceState.Fail.ToString());
            }

            if (DeviceServiceState.Skip != snmp)
            {
                serviceResult = printer.IsSnmpAccessible(printerIpAddress);

                requestedServices.Add("SNMP");
                expectedResult.Add(snmp.ToString());
                actualResult.Add(serviceResult ? DeviceServiceState.Pass.ToString() : DeviceServiceState.Fail.ToString());
            }

            if (DeviceServiceState.Skip != snmpGet)
            {
                try
                {
                    SnmpWrapper.Instance().Create(printerIpAddress.ToString());
                    SnmpWrapper.Instance().GetDomainName();
                    //SnmpWrapper.Instance().GetIPP();
                    // If SNMP get works the code execution reaches the below line.
                    serviceResult = true;
                }
                catch
                {
                    serviceResult = false;
                }

                requestedServices.Add("SNMPGet");
                expectedResult.Add(snmpGet.ToString());
                actualResult.Add(serviceResult ? DeviceServiceState.Pass.ToString() : DeviceServiceState.Fail.ToString());
            }

            if (DeviceServiceState.Skip != snmpSet)
            {
                try
                {
                    SnmpWrapper.Instance().Create(printerIpAddress.ToString());

                    // Set the community name only if it is specified.
                    if (null != snmpSetCommunityName)
                    {
                        SnmpWrapper.Instance().SetCommunityName(snmpSetCommunityName);
                    }

                    //serviceResult = SnmpWrapper.Instance().SetIPP(2);
                    serviceResult = SnmpWrapper.Instance().SetDomainName("ACLValidation.com");
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                }
                catch (Exception)
                {
                    serviceResult = false;
                }

                requestedServices.Add("SNMPSet");
                expectedResult.Add(snmpSet.ToString());
                actualResult.Add(serviceResult ? DeviceServiceState.Pass.ToString() : DeviceServiceState.Fail.ToString());
            }

            if (DeviceServiceState.Skip != http)
            {
                serviceResult = printer.IsEwsAccessible(printerIpAddress);

                requestedServices.Add("HTTP");
                expectedResult.Add(http.ToString());
                actualResult.Add(serviceResult ? DeviceServiceState.Pass.ToString() : DeviceServiceState.Fail.ToString());
            }

            if (DeviceServiceState.Skip != https)
            {
                serviceResult = printer.IsEwsAccessible(printerIpAddress, "https");

                requestedServices.Add("HTTPS");
                expectedResult.Add(https.ToString());
                actualResult.Add(serviceResult ? DeviceServiceState.Pass.ToString() : DeviceServiceState.Fail.ToString());
            }
            // Sirius printers doesn't support ftp
            if ((DeviceServiceState.Skip != ftp) && (!(printer is Sirius)))
            {
                string ftpAddress = printerIpAddress.ToString();

                // Check if address passed is IPv6, if so add square braces between ip address 
                if (ftpAddress.Contains(':'))
                {
                    ftpAddress = string.Concat("[", ftpAddress, "]");
                }

                string ftpUri = "{0}{1}/{2}";
                ftpUri = ftpUri.FormatWith("ftp://", ftpAddress, Path.GetFileName(CreateFile("temp")));


                serviceResult = printer.IsFTPAccessible(ftpUri);

                requestedServices.Add("FTP");
                expectedResult.Add(ftp.ToString());
                actualResult.Add(serviceResult ? DeviceServiceState.Pass.ToString() : DeviceServiceState.Fail.ToString());
            }

            if (DeviceServiceState.Skip != p9100)
            {
                serviceResult = printer.Install(printerIpAddress, Printer.Printer.PrintProtocol.RAW, printerDriver, printerDriverModel, 9100, isPingRequired: isPingRequiredP9100Install);

                if (serviceResult)
                {
                    serviceResult = printer.Print(CreateFile("Test file for validating P9100 print."));
                }

                TraceFactory.Logger.Info(serviceResult
                    ? "P9100 Print with {0} IP Address is successful.".FormatWith(printerIpAddress)
                    : "P9100 Print with {0} IP Address is not successful.".FormatWith(printerIpAddress));

                requestedServices.Add("P9100");
                expectedResult.Add(p9100.ToString());
                actualResult.Add(serviceResult ? DeviceServiceState.Pass.ToString() : DeviceServiceState.Fail.ToString());
            }

			if (DeviceServiceState.Skip != lpd)
			{
				// if we run the LPD command through Command prompt then it won't have any dependency on SNMP, the true behaviour is if SNMP is disables LPD should fail, so updated
				if(!printer.Install(printerIpAddress, Printer.Printer.PrintProtocol.LPD, printerDriver, printerDriverModel, 515, isPingRequired: isPingRequiredP9100Install))
                {
                    return false;
                }
				serviceResult = printer.Print(CreateFile("Test file for validating LPD print."));

                TraceFactory.Logger.Info(serviceResult
                    ? "LPD Print with {0} IP Address is successful.".FormatWith(printerIpAddress)
                    : "LPD Print with {0} IP Address is not successful.".FormatWith(printerIpAddress));

                requestedServices.Add("LPD");
                expectedResult.Add(lpd.ToString());
                actualResult.Add(serviceResult ? DeviceServiceState.Pass.ToString() : DeviceServiceState.Fail.ToString());
            }

            if (DeviceServiceState.Skip != wsd)
            {
                DeviceInfo deviceInfo;
                serviceResult = PrinterDiscovery.Discover(printerIpAddress, out deviceInfo);

                TraceFactory.Logger.Info(serviceResult
                    ? "WS Discovery with {0} IP Address is successful.".FormatWith(printerIpAddress)
                    : "WS Discovery with {0} IP Address is not successful.".FormatWith(printerIpAddress));

                requestedServices.Add("WSD");
                expectedResult.Add(wsd.ToString());
                actualResult.Add(serviceResult ? DeviceServiceState.Pass.ToString() : DeviceServiceState.Fail.ToString());
            }

            for (int i = 0; i < expectedResult.Count; i++)
            {
                testResult.Add(expectedResult[i].Equals(actualResult[i]));
            }

            TraceFactory.Logger.Info("****************** Validation Summary : {0} *****************".FormatWith(printerIpAddress));
            TraceFactory.Logger.Info("---------------------------------------------------------------------------");
            TraceFactory.Logger.Info("Services   *  " + string.Join("  *  ", requestedServices) + "  *");
            TraceFactory.Logger.Info("---------------------------------------------------------------------------");
            TraceFactory.Logger.Info("Expected   *  " + string.Join("  *  ", expectedResult) + "  *");
            TraceFactory.Logger.Info("---------------------------------------------------------------------------");
            TraceFactory.Logger.Info("Actual     *  " + string.Join("  *  ", actualResult) + "  *");
            TraceFactory.Logger.Info("---------------------------------------------------------------------------");

            return testResult.Aggregate(true, (current, res) => current & res);
        }

        /// <summary>
        /// Prints the results in a tabular format
        /// </summary>
        /// <param name="services">Service names</param>
        /// <param name="expectedResults">Expected results</param>
        /// <param name="actualResults">Actual results</param>
        private static void PrintValidationSummary(Collection<string> services, Collection<string> expectedResults, Collection<string> actualResults)
        {
            TraceFactory.Logger.Info("****************** Validation Summary *****************");

            int firstColumnSize = 12;
            int allColumnsSize = 8;

            string separator = "+" + Repeat("-", firstColumnSize) + "+";

            foreach (string service in services)
            {
                separator += Repeat("-", allColumnsSize) + "+";
            }

            string servicesString = "+" + "  Services  " + "+";

            foreach (string service in services)
            {
                servicesString += "  " + service.PadRight(allColumnsSize - 2, ' ') + "+";
            }

            string expectedString = "+" + "  Expected  " + "+";

            foreach (string result in expectedResults)
            {
                expectedString += "  " + result + " " + "+";
            }

            string actualString = "+" + "  Actual  " + " + ";

            foreach (string result in actualResults)
            {
                actualString += "  " + result + " " + " + ";
            }

            TraceFactory.Logger.Info(separator);
            TraceFactory.Logger.Info(servicesString);
            TraceFactory.Logger.Info(separator);
            TraceFactory.Logger.Info(expectedString);
            TraceFactory.Logger.Info(separator);
            TraceFactory.Logger.Info(actualString);
            TraceFactory.Logger.Info(separator);
        }

        /// <summary>
        /// Repeats the given string in specified number of times
        /// </summary>
        /// <param name="str">string to be repeated</param>
        /// <param name="count">Number of times to be repeated</param>
        /// <returns>Repeated string with specified number of times</returns>
        public static string Repeat(string str, int count)
        {
            if (count == 0)
                return "";

            return string.Concat(Enumerable.Repeat(str, count));
        }

        /// <summary>
        /// Gets the corresponding printer address based on the request.
        /// </summary>
        /// <param name="printer">Printer object</param>
        /// <param name="addressType">IP Address Type</param>
        /// <returns>Returns IP address from the printer based on the request</returns>
        public static IPAddress GetIPAddress(Printer.Printer printer, IPAddressType addressType)
        {
            // Fail safe option is to return wired IPv4 address
            IPAddress ipAddress = printer.WiredIPv4Address;

            switch (addressType)
            {
                case IPAddressType.IPv4:
                    ipAddress = printer.WiredIPv4Address;
                    break;

                case IPAddressType.StatefullIPv6:
                    ipAddress = printer.IPv6StateFullAddress;
                    break;

                case IPAddressType.StatelessIPv6:
                    ipAddress = printer.IPv6StatelessAddresses[0];
                    break;

                case IPAddressType.LinkLocalIPv6:
                    ipAddress = printer.IPv6LinkLocalAddress;
                    break;
            }

            return ipAddress;
        }

        /// <summary>
        /// Install CA certificate on client machine in 'root' folder
        /// </summary>
        /// <param name="filePath">CA Certificate file path</param>
        /// <returns>true if certificate is installed successfully, false otherwise</returns>
        public static bool InstallCACertificate(string filePath)
        {
            return NetworkUtil.ExecuteCommandAndValidate("certutil -f -addstore \"root\" \"{0}\"".FormatWith(filePath));
        }

        /// <summary>
        /// Install ID certificate on client machine in 'Personal' folder
        /// </summary>
        /// <param name="filePath">ID Certificate file path</param>
        /// <param name="password">ID Password</param>
        /// <returns>true if certificate is installed successfully, false otherwise</returns>
        public static bool InstallIDCertificate(string filePath, string password = null)
        {
            if (password != null)
            {
                return NetworkUtil.ExecuteCommandAndValidate("certutil -f -p \"{0}\" -importPFX \"{1}\"".FormatWith(password, filePath));
            }
            return NetworkUtil.ExecuteCommandAndValidate("certutil -importPFX \"{0}\" NoExport".FormatWith(filePath));
        }

        /// <summary>
		/// Delete CA Certificate
		/// </summary>
		/// <param name="filePath">CA Certificate file path</param>
		/// <returns>true is certificate is deleted</returns>
        public static bool DeleteCACertificate(string filePath)
        {
            return NetworkUtil.ExecuteCommandAndValidate("certutil -delstore \"root\" {0}".FormatWith(CertificateUtility.GetCertificateDetails(filePath).IssuedTo));
        }

        /// <summary>
        /// Delete ID Certificate
        /// </summary>
        /// <param name="filePath">ID Certificate file path</param>
        /// <param name="password">ID Certificate password</param>
        /// <returns>true is certificate is deleted</returns>
        public static bool DeleteIDCertificate(string filePath, string password)
        {
            return NetworkUtil.ExecuteCommandAndValidate("certutil -delstore \"My\" {0}".FormatWith(CertificateUtility.GetCertificateDetails(filePath, password).IssuedTo));
        }

        /// <summary>
        /// Setting Firewall Domain and Public Profiles
        /// </summary>
        /// <param name="enable">true to turn on  false to turn off</param>
        /// <returns>true if enabled, false otherwise</returns>
        public static bool SetFirewallDomainAndPublicProfile(bool enable)
        {
            TraceFactory.Logger.Debug("Setting Firewall Public and Domain Profiles to:{0}".FormatWith(enable));

            string status = enable ? "on" : "off";
            string configurationCmd = "netsh advfirewall set public state {0}".FormatWith(status);
            NetworkUtil.ExecuteCommandAndValidate(configurationCmd);

            configurationCmd = "netsh advfirewall set domain state {0}".FormatWith(status);
            NetworkUtil.ExecuteCommandAndValidate(configurationCmd);

            return true;
        }

        /// <summary>
        /// Get Test Id from TestDetailsAttribute
        /// </summary>
        /// <returns></returns>
        public static int GetTestId()
        {
            StackFrame frame = new StackFrame(1);
            var method = frame.GetMethod();

            TestDetailsAttribute testDetails = (TestDetailsAttribute)method.GetCustomAttributes(typeof(TestDetailsAttribute), true)[0];
            return testDetails.Id;
        }

        /// <summary>
        /// Returns the client network name provided by a particular server.
        /// </summary>
        /// <param name="serverIpAddress">IP Address of the server</param>
        /// <returns>The client network name.</returns>
        public static string GetClientNetworkName(string serverIpAddress)
        {
            foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.GetIPProperties().UnicastAddresses.Any(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork && ip.Address.IsInSameSubnet(IPAddress.Parse(serverIpAddress), IPAddress.Parse(serverIpAddress).GetSubnetMask())))
                {
                    TraceFactory.Logger.Info("Network Name: {0}".FormatWith(item.Name));
                    return item.Name;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Reboot machine
        /// Note: Credentials for executing machine should be same as IP Address provided machine
        /// </summary>
        /// <param name="address">IP Address of machine</param>
        public static void RebootMachine(IPAddress address = null)
        {
            if (null == address)
            {
                address = NetworkUtil.GetLocalAddress();
            }

            string cmd = "shutdown /r /m \\\\{0}".FormatWith(address);
            TraceFactory.Logger.Debug("Command to Reboot the Secondary Client Machine".FormatWith(cmd));
            NetworkUtil.ExecuteCommand(cmd);
        }

        /// <summary>
        /// Scan a document to network folder
        /// </summary>
        /// <param name="printerIpAddress">Printer Address</param>
        /// <param name="quickSetname">Quickset name</param>
        /// <param name="networkFolderPath">Network Folder Path</param>
        /// <param name="testNo">Test no for filename identification</param>
        /// <returns></returns>
        public static bool ScanToNetworkFolder(IPAddress printerIpAddress, string quickSetname, string networkFolderPath, int testNo)
        {
            // Device Automation to scan the document
            IDevice device = DeviceFactory.Create(printerIpAddress);

            // Go to Home screen if incase it has any cartridge popup errors
            if (GoToHomeScreen(device))
            {
                TraceFactory.Logger.Info("Cleared cartridge errors and successfully landed to home page");
            }
            else
            {
                TraceFactory.Logger.Info("Printer control panel screen has errors, please clear the errors and repeat the test");
                return false;
            }

            // Create network folder app object
            INetworkFolderApp app = NetworkFolderAppFactory.Create(device);

            // Launch the app
            app.Launch();

            // Select the predefined setting, this setting should be available on the printer
            app.SelectQuickset(quickSetname);

            // Set the file name 
            string fileName = "{0}-{1}.pdf".FormatWith(quickSetname, testNo);
            app.EnterFileName(fileName);

            // Setting the scan job options
            ScanExecutionOptions options = new ScanExecutionOptions { JobBuildSegments = 1 };

            // Execute the scan job
            app.ExecuteJob(options);

            string scanDocFileName = Path.Combine(networkFolderPath, fileName);

            //Waiting for 6 min to verify the file is present in the specified location
            Thread.Sleep(TimeSpan.FromMinutes(6));
            TraceFactory.Logger.Debug(scanDocFileName);

            if (File.Exists(scanDocFileName))
            {
                TraceFactory.Logger.Info("Document is scanned and placed at {0} on the client machine.".FormatWith(scanDocFileName));
                return true;
            }
            TraceFactory.Logger.Info("Failed to scan the document");
            return false;
        }

        /// <summary>
        /// Get SecurityRuleSettings for Kerberos Authentication type
        /// </summary>
        /// <param name="testNo">Test Number</param>
        /// <param name="kerberosSettings">Kerberos Settings to be configured</param>
        /// <param name="address">Address template</param>
        /// <param name="service">Service template</param>
        /// <param name="strength">IKE Security Strength</param>
        /// <param name="defaultAction"></param>
        /// <returns>Security Rule Settings <see cref="SecurityRuleSettings"/></returns>
        public static SecurityRuleSettings GetKerberosRuleSettings(int testNo, KerberosSettings kerberosSettings, AddressTemplateSettings address, ServiceTemplateSettings service, IKESecurityStrengths strength, DefaultAction defaultAction = DefaultAction.Drop)
        {
            string IPSECTEMPLATENAME = "IPSecTemplate-{0}";
            string CLIENT_RULE_NAME = "ClientRule-{0}";
            IKEv1Settings ikeV1Settings = new IKEv1Settings(kerberosSettings);
            DynamicKeysSettings dynamicSettings = new DynamicKeysSettings(strength, ikeV1Settings);
            IPsecTemplateSettings ipsecTemplate = new IPsecTemplateSettings(IPSECTEMPLATENAME.FormatWith(testNo), dynamicSettings);

            SecurityRuleSettings securitySettings = new SecurityRuleSettings(CLIENT_RULE_NAME.FormatWith(testNo), address, service, IPsecFirewallAction.ProtectedWithIPsec, ipsecTemplate, defaultAction: defaultAction);

            return securitySettings;
        }

        /// <summary>
        /// Checks if password is prompted in telnet session
        /// Note: Password is compulsory for the validation when password is configured on the printer. 
        /// </summary>
        /// <param name="family"></param>
        /// <param name="deviceAddress">IP address of the device</param>
        /// <param name="userName">The User name</param>
        /// <param name="password">The password</param>
        /// <param name="checkForPassword"></param>
        /// <returns></returns>
        public static bool CheckTelnetPasswordPrompt(PrinterFamilies family, string deviceAddress, string userName = "", string password = "", bool checkForPassword = true)
        {
            string plinkFilePath = "plink.exe";
            string tempFilePath = Path.GetTempFileName();
            string batFilePath = Path.Combine(Directory.GetCurrentDirectory(), "{0}.bat".FormatWith(Guid.NewGuid()));

            try
            {
                File.WriteAllBytes(plinkFilePath, Resource.plink);

                string command;
                if (!checkForPassword)
                {
                    command = "plink.exe -telnet {0} < \"{1}\" >\"{2}\"".FormatWith(deviceAddress, CreateFile("Exit{0}".FormatWith("\n")), tempFilePath);
                }
                else
                {
                    if (family == PrinterFamilies.TPS)
                    {
                        command = "plink.exe -telnet {0} < \"{1}\" >\"{2}\"".FormatWith(deviceAddress, CreateFile("{1}{0}Exit{0}".FormatWith("\n", password)), tempFilePath);
                    }
                    else
                    {
                        command = "plink.exe -telnet {0} < \"{1}\" >\"{2}\"".FormatWith(deviceAddress, CreateFile("{1}{0}{2}{0}Exit{0}".FormatWith("\n", userName, password)), tempFilePath);
                    }
                }

                using (StreamWriter writer = new StreamWriter(batFilePath, false))
                {
                    writer.WriteLine(command);
                    writer.Close();
                }

                try
                {
                    ProcessUtil.Execute(@batFilePath, Environment.NewLine, new TimeSpan(0, 1, 0));
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Info(ex.Message);
                }

                Process[] activeProcess = Process.GetProcesses();
                if (null != activeProcess.FirstOrDefault(x => x.ProcessName.EqualsIgnoreCase("plink")))
                {
                    activeProcess.FirstOrDefault(x => x.ProcessName.EqualsIgnoreCase("plink")).Kill();
                }

                Thread.Sleep(TimeSpan.FromSeconds(20));

                if (File.ReadAllText(tempFilePath).Contains("Enter username") || File.ReadAllText(tempFilePath).Contains("Password:"))
                {
                    TraceFactory.Logger.Debug(File.ReadAllText(tempFilePath));
                    TraceFactory.Logger.Info("Password is prompted in the telnet session for IP:{0}.".FormatWith(deviceAddress));
                    return (checkForPassword & true);
                }
                else
                {
                    TraceFactory.Logger.Info("Password is not prompted in the telnet session for IP:{0}.".FormatWith(deviceAddress));
                    return (!checkForPassword & true);
                }
            }
            finally
            {
                if (File.Exists(plinkFilePath))
                {
                    File.Delete(plinkFilePath);
                }

                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }

                if (File.Exists(batFilePath))
                {
                    File.Delete(batFilePath);
                }
            }
        }

        /// <summary>
        /// Pings continuously for the specified time for all the given addresses.
        /// This method doesn't support multi thread.
        /// </summary>
		/// <param name="ipAddressList">List of address should be pinged</param>
		/// <param name="timeOut">Time to ping</param>
		public static void PingContinuously(Collection<IPAddress> ipAddressList, TimeSpan timeOut)
        {
            DateTime endTime = DateTime.Now.Add(timeOut);
            int i = 0;

            // reinitialize the local variable
            _continuousPingStatistics = new Collection<PingStatics>();

            // loop till the timer expires
            while (DateTime.Now < endTime)
            {
                // walk thru all the address list.
                foreach (IPAddress address in ipAddressList)
                {
                    // if it is first time add ping statics object in the collection
                    if (i == 0)
                    {
                        PingStatics pingStatics = new PingStatics
                        {
                            IPAddress = address.ToString(),
                            Status = IPStatus.Unknown
                        };

                        _continuousPingStatistics.Add(pingStatics);
                    }

                    using (var ping = new Ping())
                    {
                        ping.PingCompleted += ping_PingCompleted;
                        ping.SendAsync(address, new object());

                        var pingStatics = _continuousPingStatistics.FirstOrDefault(n => n.IPAddress == address.ToString());
                        pingStatics.TotalCount++;

                        // Sleep for sometime to avoid ping queue
                        Thread.Sleep(TimeSpan.FromMilliseconds(100));
                    }
                }

                i++;
            }
        }

        /// <summary>
        /// Gets the continuous ping statistics details, before calling this method "PingContinuously" should be called.
        /// </summary>
        /// <returns>Ping Statistic details</returns>
        public static Collection<PingStatics> GetContinuousPingStatistics()
        {
            // walk thru all the items and update the final message before sending it
            foreach (PingStatics pingStatics in _continuousPingStatistics)
            {
                int loss = pingStatics.TotalCount - pingStatics.PassCount;
                pingStatics.Message = "Ping statistics for " + pingStatics.IPAddress + ":\n\t Packets: Sent = " + pingStatics.TotalCount + ", Received = " + pingStatics.PassCount + " Lost = " + loss + " <(" + ((loss * 100) / pingStatics.TotalCount) + "% loss>";
            }

            return _continuousPingStatistics;
        }

        /// <summary>
        /// Install and Print for specified PrintProtocol and Port no
        /// </summary>
        /// <param name="ipAddress">Ip Address of printer</param>
        /// <param name="protocol"><see cref="Printer.PrintProtocol"/></param>
        /// <param name="productFamily"><see cref=" productFamily"/></param>
        /// <param name="printDriver">Driver Path</param>
        /// <param name="printDriverModel">Driver Model</param>
        /// <param name="portNo">Print protocol port no</param>
        /// <param name="hostName"></param>
        /// <returns>true if installation and print is successful, false otherwise</returns>
        public static bool InstallandPrint(IPAddress ipAddress, Printer.Printer.PrintProtocol protocol, string productFamily, string printDriver, string printDriverModel, int portNo = 515, string hostName = null)
        {
            bool result = false;

            PrinterFamilies printerFamily = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), productFamily);
            Printer.Printer printer = PrinterFactory.Create(printerFamily, ipAddress);

            if (protocol == Printer.Printer.PrintProtocol.WSP)
            {
                printer.NotifyWSPrinter += printer_NotifyWSPAddition;
                if (printer.Install(ipAddress, Printer.Printer.PrintProtocol.WSP, printDriver, printDriverModel))
                {
                    MessageBox.Show(@"WS Printer was added successfully.", @"WS Printer Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(@"WS Printer was not added successfully. All WS Print related tests will fail.", @"WS Printer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (printer.Install(ipAddress, protocol, printDriver, printDriverModel, portNo, hostName))
            {
                result = printer.Print(CreateFile("Test File for Firewall Security."));
            }
            else
            {
                TraceFactory.Logger.Info("Printer Installation is unsuccessful for {0} IP Address with {1} Protocol.".FormatWith(ipAddress, protocol));
            }

            if (result)
            {
                TraceFactory.Logger.Info("Print Job is successful for {0} IP Address with {1} Protocol.".FormatWith(ipAddress, protocol));
            }
            else
            {
                TraceFactory.Logger.Info("Print Job is unsuccessful for {0} IP Address with {1} Protocol.".FormatWith(ipAddress, protocol));
                printer.DeleteAllPrintQueueJobs();
            }

            return result;
        }

        /// <summary>
        /// Prerequistes for IPPS Protocol related testcases
        /// 1. Installation of CA and ID certificates in Printer and Client.[Pre Generated Certificate should have hostname]
        /// 2. The hostname of the certificate has to be set in the Printer and in the server.
        /// 3. WinServerIP Address has to be set in the printer and in the server.
        /// 4. Make sure the Win server is up and running in DHCP server.       
        /// </summary>
        /// <param name="printerIpAddress">Printer IP Address</param>              
        /// <param name="outHostName">host name from the certificate</param> 
        /// <returns>Returns true prerequisites passed else returns false</returns>
        public static bool IPPS_Prerequisite(IPAddress printerIpAddress, out string outHostName)
        {
            string serverIp = Printer.Printer.GetDHCPServerIP(printerIpAddress).ToString();

            DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(serverIp);
            string scopeIp = serviceMethod.Channel.GetDhcpScopeIP(serverIp);

            // Getting hostname from the certificate
            CertificateDetails certParameters = CertificateUtility.GetCertificateDetails(IDCERTIFICATEPATH, IDCERTIFICATE_PSWD);
            outHostName = certParameters.IssuedTo;

            // Installing CA and ID Certificate in Printer and in the client
            if (!EwsWrapper.Instance().InstallCACertificate(CACERTIFICATEPATH))
            {
                return false;
            }
            if (!EwsWrapper.Instance().InstallIDCertificate(IDCERTIFICATEPATH, IDCERTIFICATE_PSWD))
            {
                return false;
            }
            if (!InstallCACertificate(CACERTIFICATEPATH))
            {
                return false;
            }
            if (!InstallIDCertificate(IDCERTIFICATEPATH, IDCERTIFICATE_PSWD))
            {
                return false;
            }

            // Setting hostname and winserver ip address in printer and in the server
            if (!EwsWrapper.Instance().SetHostname(outHostName))
            {
                return false;
            }
            if (!EwsWrapper.Instance().SetPrimaryWinServerIP(serverIp))
            {
                return false;
            }
            if (!serviceMethod.Channel.SetHostName(serverIp, scopeIp, outHostName))
            {
                return false;
            }
            if (!serviceMethod.Channel.SetWinsServer(serverIp, scopeIp, serverIp))
            {
                return false;
            }

            //check whether the Wins is up and running in DHCP Server
            return StartService("WINS", serverIp);
        }

        /// <summary>
        /// PostRequisites for IPPS Protocol related testcases
        /// 1. Uninstall CA and ID certificates in Printer and Client.               
        /// </summary>                     
        /// <returns>Returns true if postrequisites passed else returns false</returns>
        public static bool IPPS_PostRequisite()
        {
            // Setting the default value for the hostname
            EwsWrapper.Instance().SetHostname("DefaultHost");
            if (!EwsWrapper.Instance().UnInstallCACertificate(CACERTIFICATEPATH))
            {
                return false;
            }
            return EwsWrapper.Instance().UnInstallIDCertificate(IDCERTIFICATEPATH, IDCERTIFICATE_PSWD);
        }

        /// <summary>
        /// Notify User to Add Web Service Printer
        /// </summary>
        public static void printer_NotifyWSPAddition()
        {
            MessageBox.Show(@"Printer driver is installed.\nAdd WS printer and press OK to continue.", @"Add WS Printer", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Cold reset the printer based on the <see cref="ColdResetParameters"/>
        /// </summary>
        /// <param name="coldResetParams"><see cref="ColdResetParameters"/></param>
        /// <param name="currentDeviceAddress">The IP address of the printer after cold reset.</param>
        /// <returns></returns>
        public static bool ColdReset(ColdResetParameters coldResetParams, out IPAddress currentDeviceAddress)
        {
            try
            {
                IPAddress linkLocalAddress = IPAddress.None;
                currentDeviceAddress = coldResetParams.IpAddress;

                Printer.Printer printer = PrinterFactory.Create(coldResetParams.PrinterFamily, currentDeviceAddress);

                if (coldResetParams.SetAdvancedOptions)
                {
                    linkLocalAddress = printer.IPv6LinkLocalAddress;
                }

                if (PrinterFamilies.VEP == coldResetParams.PrinterFamily)
                {
                    // There is a chance of losing IP address here
                    printer.TCPReset();

                    Thread.Sleep(TimeSpan.FromMinutes(3));

                    // Discover and get the current device address 
                    // Case 1: When the IP address provided is IPv6
                    // Case 2: When ping with the current device address is not successful.
                    if (coldResetParams.IpAddress.AddressFamily == AddressFamily.InterNetwork && NetworkUtil.PingUntilTimeout(coldResetParams.IpAddress, TimeSpan.FromMinutes(2)))
                    {
                        TraceFactory.Logger.Info("Ping with IP address: {0} is successful.".FormatWith(coldResetParams.IpAddress));
                        currentDeviceAddress = coldResetParams.IpAddress;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Ping with IP address: {0} is successful.".FormatWith(coldResetParams.IpAddress));
                        if (!IPAddress.TryParse(GetPrinterIPAddress(coldResetParams.MacAddress), out currentDeviceAddress))
                        {
                            if (!IPAddress.TryParse(GetPrinterIPAddress(coldResetParams.MacAddress), out currentDeviceAddress))
                            {
                                TraceFactory.Logger.Debug("IP address was not discovered after TCP reset.");
                                return false;
                            }
                        }
                    }

                    if (NetworkUtil.PingUntilTimeout(currentDeviceAddress, TimeSpan.FromMinutes(1)))
                    {
                        printer = PrinterFactory.Create(coldResetParams.PrinterFamily, currentDeviceAddress);
                    }
                    else
                    {
                        if (NetworkUtil.PingUntilTimeout(linkLocalAddress, TimeSpan.FromMinutes(2)))
                        {
                            printer = PrinterFactory.Create(coldResetParams.PrinterFamily, linkLocalAddress);
                        }
                        else if (currentDeviceAddress.IsAutoIP())
                        {
                            // Case 2: If ping with auto IP is successful, change the device address to auto IP
                            // Case 3: Ping with auto IP failed. Bring down the specified DHCP Server, release renew client IP, Check ping status with auto IP.
                            using (DhcpApplicationServiceClient client = DhcpApplicationServiceClient.Create(coldResetParams.DhcpServerIpAddress))
                            {
                                client.Channel.StopDhcpServer();
                            }

                            Thread.Sleep(TimeSpan.FromSeconds(30));
                            NetworkUtil.RenewLocalMachineIP();

                            if (NetworkUtil.PingUntilTimeout(currentDeviceAddress, TimeSpan.FromSeconds(30)))
                            {
                                printer = PrinterFactory.Create(coldResetParams.PrinterFamily, currentDeviceAddress);
                            }
                            else
                            {
                                TraceFactory.Logger.Info("Ping with IP: {0} failed.".FormatWith(currentDeviceAddress));
                                return false;
                            }
                        }
                        else if (currentDeviceAddress.Equals(Printer.Printer.LegacyIPAddress))
                        {
                            TraceFactory.Logger.Info("IP is: {0} is legacy IP. Can not perform cold reset with legacy IP address.".FormatWith(currentDeviceAddress));
                            return false;
                        }
                    }

                    TraceFactory.Logger.Debug("Performing TCP Reset with IP address: {0}".FormatWith(printer.WiredIPv4Address.ToString()));
                    printer.PowerCycle();
                }
                else
                {
                    printer.ColdReset();
                }

                if (PrinterFamilies.TPS == coldResetParams.PrinterFamily)
                {
                    // Wireless configuration will be removed on cold reset, configuring wireless using wired ip address.
                    if (coldResetParams.PrinterConnectivityType.Equals(ConnectivityType.Wireless))
                    {
                        INetworkSwitch networkSwitch = SwitchFactory.Create(coldResetParams.switchIpAddress);

                        #region Enable Switch Port

                        if (networkSwitch.EnablePort(coldResetParams.PortNumber))
                        {
                            TraceFactory.Logger.Info("Enabled Port# {0} to fetch wired ip address to configure wireless back after cold reset".FormatWith(coldResetParams.PortNumber));
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Make sure valid switch port is provided");
                            TraceFactory.Logger.Info("Failed to enable port# {0} to fetch wired ip address to configure wireless back after cold reset".FormatWith(coldResetParams.PortNumber));
                            return false;
                        }

                        string wiredAddress = string.Empty;
                        string hostName = string.Empty;

                        BacaBodWrapper.DiscoverDevice(coldResetParams.MacAddress, BacaBodSourceType.MacAddress, BacaBodDiscoveryType.All, ref wiredAddress, ref hostName);

                        if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(wiredAddress), TimeSpan.FromMinutes(2)))
                        {
                            return false;
                        }

                        // To make sure all stack is up and active, waiting for a while
                        Thread.Sleep(TimeSpan.FromMinutes(1));

                        #endregion

                        #region Configure Wireless

                        // Changing device address to wired ip to configure wireless
                        EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(wiredAddress));
                        WirelessSettings settings = new WirelessSettings() { SsidName = coldResetParams.WirelessSSID };
                        if (EwsWrapper.Instance().ConfigureWireless(settings, new WirelessSecuritySettings()))
                        {
                            TraceFactory.Logger.Info("Successfully Re-configured Wireless on the device after cold reset using the SSID:{0}".FormatWith(coldResetParams.WirelessSSID));
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Failed to Re-configure Wireless on the device after cold reset using the SSID:{0}".FormatWith(coldResetParams.WirelessSSID));
                            return false;
                        }

                        #endregion

                        #region Disable Switch Port

                        if (networkSwitch.DisablePort(coldResetParams.PortNumber))
                        {
                            TraceFactory.Logger.Info("Disabled Port# {0} to retrieve back wireless ip address after cold reset".FormatWith(coldResetParams.PortNumber));
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Please make sure that you are configured a managed switch with proper port number as input through UI");
                            TraceFactory.Logger.Info("Failed to disable port# {0} to retrieve back wireless ip address after cold reset".FormatWith(coldResetParams.PortNumber));
                            return false;
                        }

                        if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(wiredAddress), TimeSpan.FromMinutes(2)))
                        {
                            return false;
                        }

                        EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(coldResetParams.IpAddress.ToString()));

                        #endregion
                    }
                }

                // Discover and get the current device address 
                // Case 1: When the IP address provided is IPv6
                // Case 2: When ping with the current device address is not successful.
                if (coldResetParams.IpAddress.AddressFamily == AddressFamily.InterNetwork && NetworkUtil.PingUntilTimeout(currentDeviceAddress, TimeSpan.FromMinutes(2)))
                {
                    //currentDeviceAddress = coldResetParams.IpAddress;
                }
                else
                {
                    if (!IPAddress.TryParse(GetPrinterIPAddress(coldResetParams.MacAddress), out currentDeviceAddress))
                    {
                        if (!IPAddress.TryParse(GetPrinterIPAddress(coldResetParams.MacAddress), out currentDeviceAddress))
                        {
                            TraceFactory.Logger.Info("IP address was not discovered after cold reset.");
                        }
                    }
                }

                if (coldResetParams.SetAdvancedOptions)
                {
                    if (NetworkUtil.PingUntilTimeout(currentDeviceAddress, TimeSpan.FromMinutes(1)))
                    {
                        TraceFactory.Logger.Debug("Setting advanced options with IP address: {0}".FormatWith(currentDeviceAddress));
                        EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);

                        // Telnet option will be disabled after performing cold reset, enabling the same.
                        EwsWrapper.Instance().SetAdvancedOptions();
                    }
                    else
                    {
                        if (NetworkUtil.PingUntilTimeout(linkLocalAddress, TimeSpan.FromMinutes(2)))
                        {
                            if (PrinterFactory.Create(coldResetParams.PrinterFamily, linkLocalAddress).IsEwsAccessible())
                            {
                                TraceFactory.Logger.Debug("Setting advanced options with IP address: {0}".FormatWith(linkLocalAddress.ToString()));
                                EwsWrapper.Instance().ChangeDeviceAddress(linkLocalAddress);

                                // Telnet option will be disabled after performing cold reset, enabling the same.
                                EwsWrapper.Instance().SetAdvancedOptions();
                            }
                        }
                        else
                        {
                            TraceFactory.Logger.Debug("Setting advanced options with IP address: {0}".FormatWith(currentDeviceAddress));

                            // Case 1: Printer is auto IP
                            if (currentDeviceAddress.IsAutoIP())
                            {
                                // Case 2: If ping with auto IP is successful, change the device address to auto IP
                                if (NetworkUtil.PingUntilTimeout(currentDeviceAddress, TimeSpan.FromSeconds(30)))
                                {
                                    EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);
                                }
                                else
                                {
                                    // Case 3: Ping with auto IP failed. Bring down the specified DHCP Server, release renew client IP, Check ping status with auto IP.
                                    using (DhcpApplicationServiceClient client = DhcpApplicationServiceClient.Create(coldResetParams.DhcpServerIpAddress))
                                    {
                                        client.Channel.StopDhcpServer();
                                    }

                                    Thread.Sleep(TimeSpan.FromSeconds(30));
                                    NetworkUtil.RenewLocalMachineIP();

                                    if (NetworkUtil.PingUntilTimeout(currentDeviceAddress, TimeSpan.FromSeconds(30)))
                                    {
                                        EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);
                                    }
                                    else
                                    {
                                        TraceFactory.Logger.Info("Ping with IP: {0} failed.".FormatWith(currentDeviceAddress));
                                        return false;
                                    }

                                    EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);

                                    // Telnet option will be disabled after performing cold reset, enabling the same.
                                    EwsWrapper.Instance().SetAdvancedOptions();
                                }
                            }
                            else if (currentDeviceAddress.Equals(Printer.Printer.LegacyIPAddress))
                            {
                                TraceFactory.Logger.Info("IP is: {0} is legacy IP. Can not set advanced options with legacy IP address.".FormatWith(currentDeviceAddress));
                            }
                        }
                    }
                }

                return true;
            }
            finally
            {
				EwsWrapper.Instance().ChangeDeviceAddress(coldResetParams.IpAddress);
                EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();
            }
        }

        //public static PrinterDetails GetPrinterDetails(PrinterDetails printerDetails)
        //{
        //    if(printerDetails.PrinterInterface == InterfaceMode.EmbeddedWired)
        //    {
        //        return printerDetails;
        //    }
        //    else if(printerDetails.PrinterInterface == InterfaceMode.Wireless)
        //    {
        //        string wirelessAddress = printerDetails.WirelessAddress;
        //        string primaryAddress = printerDetails.PrimaryInterfaceAddress;

        //        printerDetails.PrimaryInterfaceAddress = wirelessAddress;
        //        printerDetails.WirelessAddress = primaryAddress;
        //    }
        //    else
        //    {
        //        string primaryWired = printerDetails.PrimaryInterfaceAddress;
        //        string secondaryAddress = printerDetails.SecondaryInterfaceAddress;

        //        printerDetails.SecondaryInterfaceAddress = primaryWired;
        //        printerDetails.PrimaryInterfaceAddress = secondaryAddress;
        //    }


        //    return printerDetails;
        //}

        /// <summary>
        /// Configures wireless through control panel for Omni devices for No security configuration.
        /// </summary>
        /// <param name="address">IP Address of the printer.</param>
        /// <param name="ssid">The SSID to be configured.</param>
        /// <returns>True if the configuration is successful, else false.</returns>
        public static bool ConfigureWirelessControlPanel(IPAddress address, string ssid)
        {
            try
            {
                TraceFactory.Logger.Info("Configuring wireless no security settings with SSID: {0} through control panel.".FormatWith(ssid));
                using (JediOmniDevice device = new JediOmniDevice(address))
                {
                    device.PowerManagement.Wake();
                    device.ControlPanel.PressHome();
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    device.ControlPanel.ScrollToItem("#hpid-settings-homescreen-button");
                    device.ControlPanel.PressWait("#hpid-settings-homescreen-button", "#hpid-tree-node-listitem-networkingandiomenu");
                    device.ControlPanel.PressWait("#hpid-tree-node-listitem-networkingandiomenu", "#hpid-tree-node-listitem-jetdirectmenu2");
                    device.ControlPanel.PressWait("#hpid-tree-node-listitem-jetdirectmenu2", "#hpid-tree-node-listitem-0x3");
                    device.ControlPanel.PressWait("#hpid-tree-node-listitem-0x3", "#hpid-tree-node-listitem-0xe_2");
                    device.ControlPanel.PressWait("#hpid-tree-node-listitem-0xe_2", "#0xe_2");
                    device.ControlPanel.PressWait("#0xe_2", "#hpid-keyboard");
                    device.ControlPanel.TypeOnVirtualKeyboard(ssid);
                    device.ControlPanel.PressWait("#hpid-keyboard-key-done", "#hpid-ok-setting-button");
                    device.ControlPanel.Press("#hpid-ok-setting-button");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    device.ControlPanel.PressWait("#hpid-tree-node-listitem-0xf_2", "#0");
                    device.ControlPanel.Press("#0");
                    device.ControlPanel.Press("#hpid-ok-setting-button");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    JediOmniNavigateHome(device);
                    TraceFactory.Logger.Info("Successfully Configured wireless no security settings with SSID: {0} through control panel.".FormatWith(ssid));
                    return true;
                }
            }
            catch
            {
                TraceFactory.Logger.Info("Failed to Configure wireless no security settings with SSID: {0} through control panel.".FormatWith(ssid));
                return false;
            }
        }

        /// <summary>
        /// Jedi Omni method for navigating home.
        /// </summary>
        /// <param name="device">The device.</param>
        public static void JediOmniNavigateHome(IDevice device)
        {
            JediOmniDevice omni = device as JediOmniDevice;
            if (omni != null)
            {
                while (omni.ControlPanel.WaitForState(".hp-button-back:last", OmniElementState.Useable, TimeSpan.FromSeconds(10)))
                {
                    omni.ControlPanel.Press(".hp-button-back:last");
                    Thread.Sleep(250);
                }

                var controls = omni.ControlPanel.GetIds("div", OmniIdCollectionType.Children);
                if (controls.Contains("hpid-button-reset"))
                {
                    if (omni.ControlPanel.WaitForState("#hpid-button-reset", OmniElementState.Useable, TimeSpan.FromSeconds(10)))
                    {
                        omni.ControlPanel.Press("#hpid-button-reset");
                    }
                    else
                    {
                        string value = omni.ControlPanel.GetValue("#hp-button-signin-or-signout", "innerText", OmniPropertyType.Property).Trim();
                        if (value.Contains("Sign Out"))
                        {
                            if (omni.ControlPanel.WaitForState("#hp-button-signin-or-signout", OmniElementState.Useable, TimeSpan.FromSeconds(10)))
                            {
                                omni.ControlPanel.Press("#hp-button-signin-or-signout");
                            }
                        }
                    }
                }

                if (!omni.ControlPanel.WaitForState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(5)))
                {
                    throw new Exception("Home Page is not accessible");
                }
                JediOmniNotificationPanel notificationPanel = new JediOmniNotificationPanel(omni);
                bool success = notificationPanel.WaitForNotDisplaying("Initializing scanner", "Clearing settings");
                if (!success)
                {
                    throw new TimeoutException($"Timed out waiting for notification panel state: Contains Initializing scanner, Clearing settings");
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Build rule for IP sec settings
        /// </summary>
        /// <param name="ipsecTemplateSettings"><see cref="IPsecTemplateSettings"/></param>
        /// <returns>Authentication and Encryption rule</returns>
        private static string GetIPsecrule(IPsecTemplateSettings ipsecTemplateSettings)
        {
            string ipsecRule = string.Empty;

            // Dynamic Key
            if (ipsecTemplateSettings.KeyType == SecurityKeyType.Dynamic)
            {
                // Split it based on version
                // IKEVersion1
                if (IKEVersion.IKEv1 == ipsecTemplateSettings.DynamicKeysSettings.Version)
                {
                    // Build rule based on Authentication type
                    if (IKEAAuthenticationTypes.PreSharedKey == ipsecTemplateSettings.DynamicKeysSettings.V1Settings.AuthenticationTypes)
                    {
                        ipsecRule = GetPreSharedKeyRule(ipsecTemplateSettings.DynamicKeysSettings.V1Settings.PSKValue, IKEVersion.IKEv1);
                    }
                    else if (IKEAAuthenticationTypes.Certificates == ipsecTemplateSettings.DynamicKeysSettings.V1Settings.AuthenticationTypes)
                    {
                        ipsecRule = GetCertificateRule(ipsecTemplateSettings.DynamicKeysSettings.V1Settings.CACertificatePath, IKEVersion.IKEv1);
                    }
                    else if (IKEAAuthenticationTypes.Kerberos == ipsecTemplateSettings.DynamicKeysSettings.V1Settings.AuthenticationTypes)
                    {
                        ipsecRule = GetKerberosRule(IKEVersion.IKEv1);
                    }

                    if (IKESecurityStrengths.Custom == ipsecTemplateSettings.DynamicKeysSettings.Strengths)
                    {
                        Encryptions encryption = Encryptions.None;
                        Authentications ahAuthentication = Authentications.None;
                        Authentications espAuthentication = Authentications.None;

                        // Phase 2 Settings
                        if (ipsecTemplateSettings.DynamicKeysSettings.V1Settings.IKEv1Phase2Settings.AHEnable)
                        {
                            ahAuthentication = ipsecTemplateSettings.DynamicKeysSettings.V1Settings.IKEv1Phase2Settings.AHAuthentication;
                        }

                        if (ipsecTemplateSettings.DynamicKeysSettings.V1Settings.IKEv1Phase2Settings.ESPEnable)
                        {
                            if (Authentications.None != ipsecTemplateSettings.DynamicKeysSettings.V1Settings.IKEv1Phase2Settings.ESPAuthentication)
                            {
                                espAuthentication = ipsecTemplateSettings.DynamicKeysSettings.V1Settings.IKEv1Phase2Settings.ESPAuthentication;
                            }

                            if (Encryptions.None != ipsecTemplateSettings.DynamicKeysSettings.V1Settings.IKEv1Phase2Settings.ESPEncryption)
                            {
                                encryption = ipsecTemplateSettings.DynamicKeysSettings.V1Settings.IKEv1Phase2Settings.ESPEncryption;
                            }
                        }

                        ipsecRule += GetPhase2Rule(ipsecTemplateSettings.DynamicKeysSettings.V1Settings.IKEv1Phase2Settings.Encapsulation, encryption, espAuthentication, ahAuthentication, ipsecTemplateSettings.DynamicKeysSettings.V1Settings.IKEv1Phase2Settings.TunnelLocalAddress, ipsecTemplateSettings.DynamicKeysSettings.V1Settings.IKEv1Phase2Settings.TunnelRemoteAddress, ipsecTemplateSettings.DynamicKeysSettings.V1Settings.IKEv1Phase2Settings.SALifeTime, ipsecTemplateSettings.DynamicKeysSettings.V1Settings.IKEv1Phase2Settings.SASize);

                        if (ipsecRule.EndsWith(","))
                        {
                            ipsecRule = ipsecRule.Remove(ipsecRule.Length - 1, 1) + " ";
                        }

                        // In case of PFS, configure rule for the same
                        if (null != ipsecTemplateSettings.DynamicKeysSettings.V1Settings.IKEv1Phase2Settings.AdvancedIKESettings)
                        {
                            if (ipsecTemplateSettings.DynamicKeysSettings.V1Settings.IKEv1Phase2Settings.AdvancedIKESettings.KeyPFS)
                            {
                                // By feault, we are adding DH Group14 for all pfs
                                ipsecRule += "qmpfs=dhgroup14";
                            }
                        }
                    }
                }
            }

            return ipsecRule;
        }

        /// <summary>
        /// Build Authentication and Encryption rule
        /// </summary>
        /// <param name="iKeSecurityStrengths">Settings containing Authentication and Encryption details<see cref="IKESecurityStrengths"/></param>
        /// <returns>Authentication + Encryptions built rule</returns>
        private static string GetAuthenticationEncryptionRule(IKESecurityStrengths iKeSecurityStrengths)
        {
            string rule = string.Empty;

            // IKESecurityStrengths here refers to Interoperability values
            /* Interoperability		|	Security
			 * --------------------------------------
			 * Low					|	High
			 * Medium				|	Medium
			 * High					|	Low
			 * */

            // High Security
            // Below predefined rules are constructed based on Manual tester suggestion where highest group, encryption and authentication for each type are used.
            if (IKESecurityStrengths.HighInteroperabilityLowsecurity == iKeSecurityStrengths)
            {
                rule = "dhgroup14:aes128-sha1";
            }
            // Medium Security
            else if (IKESecurityStrengths.MediumInteroperabilityMediumsecurity == iKeSecurityStrengths)
            {
                rule = "dhgroup2:3des-sha1";
            }
            // Low Security
            else if (IKESecurityStrengths.LowInteroperabilityHighsecurity == iKeSecurityStrengths)
            {
                rule = "dhgroup14:aes128-sha1";
            }
            else if (IKESecurityStrengths.HighSecurityWithAdvancedFeatures == iKeSecurityStrengths)
            {
                // TODO: Only Linux 
                rule = "";
            }

            return rule;
        }

        /// <summary>
        /// Build customize Authentication/ Encryption rule
        /// </summary>
        /// <param name="encapsulation"><see cref="EncapsulationType"/></param>
        /// <param name="espEncryption"><see cref="Encryptions"/></param>
        /// <param name="espAuthentication"><see cref="Authentications"/></param>
        /// <param name="ahAuthentication"><see cref="Authentications"/></param>
        /// <param name="localAddress">Local Address</param>
        /// <param name="remoteAddress">Remote Address</param>
        /// <param name="lifetime">lifetime of rule in seconds</param>
        /// <param name="lifeSize"></param>
        /// <returns>Authentication + Encryptions built rule</returns>
        private static string GetPhase2Rule(EncapsulationType encapsulation, Encryptions espEncryption = Encryptions.None, Authentications espAuthentication = Authentications.None,
                                            Authentications ahAuthentication = Authentications.None, string localAddress = null, string remoteAddress = null, long lifetime = 3600, long lifeSize = 0)
        {
            string rule = string.Empty;

            // lifetime passed to function is in seconds and client rule expects in minutes. By default 60 minutes is set if no value is passed
            lifetime = lifetime / 60;

            if (Authentications.None != espAuthentication || Authentications.None != ahAuthentication)
            {
                rule += "qmsecmethods=";
            }

            // Format qmsecmethods=ah:integrity+esp:integrity-authentication+Lifetime(min)+Lifetime(kb)


            /* rule format for different cases:
			 * -----------------------------------------------------------
			 * Both ah and esp=>    ah:integrity+esp:integrity-encryption
			 * Only ah=>            ah:integrity
			 * Only esp=>           esp:integrity-encryption
			 * */

            // The first two cases mentioned above are handled in the below condition ie, any structure having atleast 1 ah athentication configured
            // Ah will take the format: ah:Integrity
            if (Authentications.None != ahAuthentication)
            {
                // AESXCBC, Sha384, Sha512 are not applicable for Windows machine

                // Grouping all authentication. When atleast 1 ah authentication is selected, all esp authentication will also be part of ah authentication
                ahAuthentication |= espAuthentication;

                if (ahAuthentication.HasFlag(Authentications.MD5))
                {
                    rule += GenerateAhEspRule(Authentications.MD5, espEncryption, lifetime, lifeSize);
                }

                if (ahAuthentication.HasFlag(Authentications.SHA1))
                {
                    rule += GenerateAhEspRule(Authentications.SHA1, espEncryption, lifetime, lifeSize);
                }

                if (ahAuthentication.HasFlag(Authentications.SHA256))
                {
                    rule += GenerateAhEspRule(Authentications.SHA256, espEncryption, lifetime, lifeSize);
                }
            }
            else
            {
                if (Authentications.None != espAuthentication)
                {
                    // AESXCBC, Sha384, Sha512 are not applicable for Windows machine
                    if (espAuthentication.HasFlag(Authentications.MD5))
                    {
                        rule += GenerateEspRule(Authentications.MD5, espEncryption, lifetime, lifeSize);
                    }

                    if (espAuthentication.HasFlag(Authentications.SHA1))
                    {
                        rule += GenerateEspRule(Authentications.SHA1, espEncryption, lifetime, lifeSize);
                    }

                    if (espAuthentication.HasFlag(Authentications.SHA256))
                    {
                        rule += GenerateEspRule(Authentications.SHA256, espEncryption, lifetime, lifeSize);
                    }
                }
            }

            if (rule.Length > 2)
            {
                rule = rule.Remove(rule.Length - 1, 1);
            }

            if (EncapsulationType.Transport == encapsulation)
            {
                rule += " mode=transport";
            }
            else
            {
                rule += " mode=tunnel";
            }


            if (!string.IsNullOrEmpty(remoteAddress))

            {
                rule += " localtunnelendpoint={0} ".FormatWith(remoteAddress);
            }

            if (!string.IsNullOrEmpty(localAddress))
            {
                rule += " remotetunnelendpoint={0} ".FormatWith(localAddress);
            }

            // Extra space as postfix will make sure rules dont get jumbled up
            return rule + " ";
        }

        /// <summary>
        /// Generate Rule with ESP encryption/Authentication settings
        /// </summary>
        /// <param name="espAuthentication"><see cref="Authentications"/></param>
        /// <param name="espEncryption"><see cref="Encryptions"/></param>
        /// <param name="lifeTime">Lifetime in seconds</param>
        /// <param name="lifeSize">Lifetime in size(kb)</param>
        /// <returns></returns>
        private static string GenerateEspRule(Authentications espAuthentication, Encryptions espEncryption, long lifeTime, long lifeSize)
        {
            string rule = "esp:{0}".FormatWith(espAuthentication.ToString().ToLowerInvariant());
            string lifesize = string.Empty;

            if (0 != lifeSize)
            {
                lifesize = "+{0}kb".FormatWith(lifeSize);
            }

            if (Encryptions.None != espEncryption)
            {
                if (espEncryption.HasFlag(Encryptions.AES128))
                {
                    rule = CheckESPEncryptionPrefix(rule, espAuthentication);
                    rule += "-{0}+{1}min{2}".FormatWith(Encryptions.AES128.ToString().ToLowerInvariant(), lifeTime, lifesize);
                }

                if (espEncryption.HasFlag(Encryptions.AES192))
                {
                    rule = CheckESPEncryptionPrefix(rule, espAuthentication);
                    rule += "-{0}+{1}min{2}".FormatWith(Encryptions.AES192.ToString().ToLowerInvariant(), lifeTime, lifesize);
                }

                if (espEncryption.HasFlag(Encryptions.AES256))
                {
                    rule = CheckESPEncryptionPrefix(rule, espAuthentication);
                    rule += "-{0}+{1}min{2}".FormatWith(Encryptions.AES256.ToString().ToLowerInvariant(), lifeTime, lifesize);
                }

                if (espEncryption.HasFlag(Encryptions.DES))
                {
                    rule = CheckESPEncryptionPrefix(rule, espAuthentication);
                    rule += "-{0}+{1}min{2}".FormatWith(Encryptions.DES.ToString().ToLowerInvariant(), lifeTime, lifesize);
                }

                if (espEncryption.HasFlag(Encryptions.DES3))
                {
                    rule = CheckESPEncryptionPrefix(rule, espAuthentication);
                    rule += "-{0}+{1}min{2}".FormatWith(Encryptions.DES3.ToString().ToLowerInvariant(), lifeTime, lifesize);
                }
            }
            return rule + ",";
        }

        /// <summary>
        /// Create Rule with Authentication and Encryption details
        /// This function will construct only Authentication Rule
        /// </summary>
        /// <param name="ahAuthentication">Ah + ESP Authentication<see cref="Authentications"/></param>
        /// <param name="espEncryption">Esp Encryption<see cref="Encryptions"/></param>
        /// <param name="lifeTime">Lifetime in minutes</param>
        /// <param name="lifeSize">Lifetime in size(kb)</param>
        /// <returns>Authentication Built rule</returns>
        private static string GenerateAhEspRule(Authentications ahAuthentication, Encryptions espEncryption, long lifeTime, long lifeSize)
        {
            // Format=> ah:integrity+esp:integrity-authentication+Lifetime

            var rule = "ah:{0}".FormatWith(ahAuthentication.ToString().ToLowerInvariant());

            switch (ahAuthentication)
            {
                case Authentications.MD5:
                    return GenerateAhEspEncryptionRule(rule, ahAuthentication, espEncryption, lifeTime, lifeSize);
                case Authentications.SHA1:
                    return GenerateAhEspEncryptionRule(rule, ahAuthentication, espEncryption, lifeTime, lifeSize);
                case Authentications.SHA256:
                    return GenerateAhEspEncryptionRule(rule, ahAuthentication, espEncryption, lifeTime, lifeSize);
            }

            return rule;
        }

        /// <summary>
        /// Create Specific Encryption rule
        /// This fucntion will construct all applicable encryptions for a Authentication
        /// </summary>
        /// <param name="rule">Authentication Construted rule</param>
        /// <param name="ahAuthentication">Ah Authentication <see cref="Authentications"/></param>
        /// <param name="espEncryption">Esp Encryption <see cref="Encryptions"/></param>
        /// <param name="lifeTime">Lifetime in minutes</param>
        /// <param name="lifeSize">Lifetime in size(kb)</param>
        /// <returns>Authentication + Encryption Built rule</returns>
        private static string GenerateAhEspEncryptionRule(string rule, Authentications ahAuthentication, Encryptions espEncryption, long lifeTime, long lifeSize)
        {
            string lifesize = string.Empty;

            if (0 != lifeSize)
            {
                lifesize = "+{0}kb".FormatWith(lifeSize);
            }

            // Esp will take the format: esp:Integrity-Encryption
            if (Encryptions.None != espEncryption)
            {
                if (espEncryption.HasFlag(Encryptions.AES128))
                {
                    rule = CheckAhAthenticationPrefix(rule, ahAuthentication);
                    rule += "+esp:{0}-{1}+{2}min{3}".FormatWith(ahAuthentication.ToString().ToLowerInvariant(), Encryptions.AES128.ToString().ToLowerInvariant(), lifeTime, lifesize);
                }

                if (espEncryption.HasFlag(Encryptions.AES192))
                {
                    rule = CheckAhAthenticationPrefix(rule, ahAuthentication);
                    rule += "+esp:{0}-{1}+{2}min{3}".FormatWith(ahAuthentication.ToString().ToLowerInvariant(), Encryptions.AES192.ToString().ToLowerInvariant(), lifeTime, lifesize);
                }

                if (espEncryption.HasFlag(Encryptions.AES256))
                {
                    rule = CheckAhAthenticationPrefix(rule, ahAuthentication);
                    rule += "+esp:{0}-{1}+{2}min{3}".FormatWith(ahAuthentication.ToString().ToLowerInvariant(), Encryptions.AES256.ToString().ToLowerInvariant(), lifeTime, lifesize);
                }

                if (espEncryption.HasFlag(Encryptions.DES))
                {
                    rule = CheckAhAthenticationPrefix(rule, ahAuthentication);
                    rule += "+esp:{0}-{1}+{2}min{3}".FormatWith(ahAuthentication.ToString().ToLowerInvariant(), Encryptions.DES.ToString().ToLowerInvariant(), lifeTime, lifesize);
                }

                if (espEncryption.HasFlag(Encryptions.DES3))
                {
                    rule = CheckAhAthenticationPrefix(rule, ahAuthentication);
                    rule += "+esp:{0}-{1}+{2}min{3}".FormatWith(ahAuthentication.ToString().ToLowerInvariant(), Encryptions.DES3.ToString().ToLowerInvariant(), lifeTime, lifesize);
                }
            }
            return rule + ",";
        }

        /// <summary>
        /// Check if the rule constructed ends with Ah Authentication prefix
        /// </summary>
        /// <param name="rule">Constructed Rule</param>
        /// <param name="ahAuthentication">Ah Authentication<see cref="Authentications"/></param>
        /// <returns>Rule added with Ah Authentication prefix</returns>
        private static string CheckAhAthenticationPrefix(string rule, Authentications ahAuthentication)
        {
            if (!rule.EndsWith("ah:{0}".FormatWith(ahAuthentication.ToString().ToLowerInvariant())))
            {
                rule += ",ah:{0}".FormatWith(ahAuthentication.ToString().ToLowerInvariant());
            }

            return rule;
        }

        /// <summary>
        /// Check if the rule constructed ends with Esp Authentication prefix
        /// </summary>
        /// <param name="rule">Constructed Rule</param>
        /// <param name="espAthentication">Esp Authentication<see cref="Authentications"/></param>
        /// <returns>Rule added with Esp Authentication prefix</returns>
        private static string CheckESPEncryptionPrefix(string rule, Authentications espAthentication)
        {
            if (!rule.EndsWith("esp:{0}".FormatWith(espAthentication.ToString().ToLowerInvariant())))
            {
                rule += ",esp:{0}".FormatWith(espAthentication.ToString().ToLowerInvariant());
            }
            return rule;
        }

        /// <summary>
        /// Build Certificate rule
        /// </summary>
        /// <param name="certificatePath">CA Certificate path</param>
        /// <param name="version">IKEVersion to specify Auth1/ Auth2</param>
        /// <returns>Certificate built rule</returns>
        private static string GetCertificateRule(string certificatePath, IKEVersion version)
        {
            string keyVersion = IKEVersion.IKEv1 == version ? "auth1" : "auth2";
            CertificateDetails details = CertificateUtility.GetCertificateDetails(certificatePath);

            // Issuer data received from CertificateUtility will not be in correct format for rule creation, modifying according to rule creation requirement
            string[] data = details.Issuer.Split(',');

            string cmd = string.Empty;

            for (int i = data.Length - 1; i >= 0; i--)
            {
                cmd += data[i] + ",";
            }

            cmd = cmd.Remove(cmd.Length - 1, 1);

            return "{0}=computercert {0}ca=\"{1}\" ".FormatWith(keyVersion, cmd);
        }

        /// <summary>
        /// Build pre shared key authentication rule
        /// </summary>
        /// <param name="presharedKeyValue">PSK value</param>
        /// <param name="version">IKEversion to specify auth1/ auth2</param>
        /// <returns>Pre shared key built rule</returns>
        private static string GetPreSharedKeyRule(string presharedKeyValue, IKEVersion version)
        {
            string keyVersion = IKEVersion.IKEv1 == version ? "auth1" : "auth2";
            return "{0}=computerpsk {0}psk=\"{1}\" ".FormatWith(keyVersion, presharedKeyValue);
        }

        /// <summary>
        /// Get kerberos authentication rule
        /// </summary>
        /// <param name="version">IKEversion to specify auth1/auth2</param>
        /// <returns></returns>
        private static string GetKerberosRule(IKEVersion version)
        {
            string keyVersion = IKEVersion.IKEv1 == version ? "auth1" : "auth2";
            return "{0}=computerkerb ".FormatWith(keyVersion);
        }

        /// <summary>
        /// Build Service Rule
        /// </summary>
        /// <param name="serviceTemplateSettings"><see cref="ServiceTemplateSettings"/></param>
        /// <returns>Network Port address</returns>
        private static Collection<string> GetServiceRule(ServiceTemplateSettings serviceTemplateSettings)
        {
            string serviceRule = "port1={0} port2={1} protocol={2}";
            Collection<string> serviceRules = new Collection<string>();

            if (DefaultServiceTemplates.Custom == serviceTemplateSettings.DefaultTemplate)
            {
                foreach (Service customService in serviceTemplateSettings.Services)
                {
                    serviceRules.Add(serviceRule.FormatWith(customService.RemotePort, customService.PrinterPort, customService.Protocol.ToString().ToLowerInvariant()));
                }
            }
            else
            {
                if (DefaultServiceTemplates.AllServices == serviceTemplateSettings.DefaultTemplate)
                {
                    // For all service, ports can not be specified and protocol will be any.
                    serviceRules.Add("protocol=any");
                }
                else if (DefaultServiceTemplates.AllPrintServices == serviceTemplateSettings.DefaultTemplate)
                {
                    serviceRules.Add(serviceRule.FormatWith("any", "21", ServiceProtocolType.TCP.ToString().ToLowerInvariant())); // FTP
                    serviceRules.Add(serviceRule.FormatWith("any", "20", ServiceProtocolType.TCP.ToString().ToLowerInvariant())); // FTP
                    serviceRules.Add(serviceRule.FormatWith("any", "5120,5122", ServiceProtocolType.TCP.ToString().ToLowerInvariant())); // FTP
                    serviceRules.Add(serviceRule.FormatWith("any", "9100", ServiceProtocolType.TCP.ToString().ToLowerInvariant())); // P9100
                    serviceRules.Add(serviceRule.FormatWith("any", "515", ServiceProtocolType.TCP.ToString().ToLowerInvariant())); // LPD
                }
                else if (DefaultServiceTemplates.AllManagementServices == serviceTemplateSettings.DefaultTemplate)
                {
                    serviceRules.Add(serviceRule.FormatWith("any", "161", ServiceProtocolType.UDP.ToString().ToLowerInvariant())); // SNMP - UPP
                    serviceRules.Add(serviceRule.FormatWith("any", "161,162", ServiceProtocolType.TCP.ToString().ToLowerInvariant())); // SNMP - TCP
                    serviceRules.Add(serviceRule.FormatWith("any", "80", ServiceProtocolType.TCP.ToString().ToLowerInvariant())); // HTTP
                    serviceRules.Add(serviceRule.FormatWith("any", "23", ServiceProtocolType.TCP.ToString().ToLowerInvariant())); // Telnet
                }
                else if (DefaultServiceTemplates.AllDigitalSendServices == serviceTemplateSettings.DefaultTemplate)
                {
                    serviceRules.Add(serviceRule.FormatWith("any", "7627", ServiceProtocolType.TCP.ToString().ToLowerInvariant())); // HTTP - TCP
                    serviceRules.Add(serviceRule.FormatWith("7627", "any", ServiceProtocolType.TCP.ToString().ToLowerInvariant())); // HTTP - TCP
                    serviceRules.Add(serviceRule.FormatWith("any", "7627", ServiceProtocolType.TCP.ToString().ToLowerInvariant())); // HTTPS - TCP
                    serviceRules.Add(serviceRule.FormatWith("7627", "any", ServiceProtocolType.TCP.ToString().ToLowerInvariant())); // HTTPS - TCP
                    serviceRules.Add(serviceRule.FormatWith("any", "25", ServiceProtocolType.TCP.ToString().ToLowerInvariant())); // SMTP - TCP
                    serviceRules.Add(serviceRule.FormatWith("any", "25", ServiceProtocolType.UDP.ToString().ToLowerInvariant())); // SMTP - UDP
                    serviceRules.Add(serviceRule.FormatWith("any", "445", ServiceProtocolType.TCP.ToString().ToLowerInvariant())); // WINS - TCP
                }
                // All discovery is not supported for IP sec rule
            }

            return serviceRules;
        }

        /// <summary>
        /// Build Address rule
        /// </summary>
        /// <param name="addressTemplateSettings"><see cref="AddressTemplateSettings"/></param>
        /// <returns>Address endpoint rule</returns>
        private static Collection<string> GetAddressRule(AddressTemplateSettings addressTemplateSettings)
        {
            Collection<string> addresses = new Collection<string>();
            string address = "endpoint1={0} endpoint2={1}";

            // endpoint1 should have machine address and endpoint2 should have printer address
            // the structure passed to this function would be constructed keeping printer as the local address. Hence local and remote address are reordered
            if (DefaultAddressTemplates.Custom == addressTemplateSettings.DefaultTemplate)
            {
                if (CustomAddressTemplateType.IPAddress == addressTemplateSettings.CustomTemplateType)
                {
                    address = address.FormatWith(addressTemplateSettings.RemoteAddress, addressTemplateSettings.LocalAddress);
                }
                else if (CustomAddressTemplateType.IPAddressPrefix == addressTemplateSettings.CustomTemplateType)
                {
                    address = address.FormatWith(addressTemplateSettings.RemoteAddress.Replace('|', '/'), addressTemplateSettings.LocalAddress.Replace('|', '/'));
                }
                else if (CustomAddressTemplateType.IPAddressRange == addressTemplateSettings.CustomTemplateType)
                {
                    address = address.FormatWith(addressTemplateSettings.RemoteAddress.Replace('|', '-'), addressTemplateSettings.LocalAddress.Replace('|', '-'));
                }
                else if (CustomAddressTemplateType.PredefinedAddresses == addressTemplateSettings.CustomTemplateType)
                {
                    string localAddress, remoteAddress;
                    localAddress = remoteAddress = string.Empty;

                    // Local
                    if (Enum<DefaultAddressTemplates>.Value(DefaultAddressTemplates.AllIPv4Addresses).Contains(addressTemplateSettings.RemoteAddress))
                    {
                        localAddress = "0.0.0.0-223.255.255.255";
                    }
                    else if (Enum<DefaultAddressTemplates>.Value(DefaultAddressTemplates.AllIPv6Addresses).Contains(addressTemplateSettings.RemoteAddress))
                    {
                        localAddress = "0000:0000:0000:0000:0000:0000:0000:0000-FEFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF";
                    }
                    else if (Enum<DefaultAddressTemplates>.Value(DefaultAddressTemplates.AllIPv6LinkLocal).Contains(addressTemplateSettings.RemoteAddress))
                    {
                        localAddress = "FE80::-FE80:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF";
                    }

                    // Remote
                    if (Enum<DefaultAddressTemplates>.Value(DefaultAddressTemplates.AllIPv4Addresses).Contains(addressTemplateSettings.LocalAddress))
                    {
                        remoteAddress = "0.0.0.0-223.255.255.255";
                    }
                    else if (Enum<DefaultAddressTemplates>.Value(DefaultAddressTemplates.AllIPv6Addresses).Contains(addressTemplateSettings.LocalAddress))
                    {
                        remoteAddress = "0000:0000:0000:0000:0000:0000:0000:0000-FEFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF";
                    }
                    else if (Enum<DefaultAddressTemplates>.Value(DefaultAddressTemplates.AllIPv6LinkLocal).Contains(addressTemplateSettings.LocalAddress))
                    {
                        remoteAddress = "FE80::-FE80:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF";
                    }

                    address = address.FormatWith(localAddress, remoteAddress);
                }

                addresses.Add(address);
            }
            else
            {
                if (DefaultAddressTemplates.AllIPAddresses == addressTemplateSettings.DefaultTemplate)
                {
                    addresses.Add(address.FormatWith("any", "any"));
                }
                else if (DefaultAddressTemplates.AllIPv4Addresses == addressTemplateSettings.DefaultTemplate)
                {
                    addresses.Add(address.FormatWith("0.0.0.0-223.255.255.255", "0.0.0.0-223.255.255.255"));
                }
                else if (DefaultAddressTemplates.AllIPv6Addresses == addressTemplateSettings.DefaultTemplate)
                {
                    addresses.Add(address.FormatWith("0000:0000:0000:0000:0000:0000:0000:0000-FEFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF", "0000:0000:0000:0000:0000:0000:0000:0000-FEFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF"));
                }
                else if (DefaultAddressTemplates.AllIPv6LinkLocal == addressTemplateSettings.DefaultTemplate)
                {
                    addresses.Add(address.FormatWith("FE80::-FE80:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF", "FE80::-FE80:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF"));
                }
                else if (DefaultAddressTemplates.AllIPv6NonLinkLocal == addressTemplateSettings.DefaultTemplate)
                {
                    // TODO: Handle multiple address
                    addresses.Add(address.FormatWith("0000:0000:0000:0000:0000:0000:0000:0000-FE7F:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF", "0000:0000:0000:0000:0000:0000:0000:0000-FE7F:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF"));
                    addresses.Add(address.FormatWith("0000:0000:0000:0000:0000:0000:0000:0000-FE7F:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF", "FEC0::-FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF"));
                    addresses.Add(address.FormatWith("FEC0::-FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF", "0000:0000:0000:0000:0000:0000:0000:0000-FE7F:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF"));
                    addresses.Add(address.FormatWith("FEC0::-FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF", "FEC0::-FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF:FFFF"));
                }
            }

            return addresses;
        }

        /// <summary>
        /// Configure IP Sec Phase 1 and Phase 2 rules. 
        /// This needs to be configured before creating the IP Sec rule.
        /// </summary>
        /// <param name="ruleSettings"><see cref="SecurityRuleSettings"/></param>
        /// <param name="enableProfiles"></param>
        /// <returns>true if both phase1 and phase2 rules are executed successfully, false otherwise</returns>
        private static bool ConfigureExternalSettings(SecurityRuleSettings ruleSettings, bool enableProfiles = true)
        {
            SetFirewallProfiles(enableProfiles);

            long mainModeLifetime = MAINMODE_LIFETIME;

            // If lifetime is not specified in the settings, use default: 480 minutes. If configured, the value will be specified in seconds; convert to minutes.
            if (null != ruleSettings.IPsecTemplate.DynamicKeysSettings.V1Settings.IKEv1Phase1Settings)
            {
                mainModeLifetime = ruleSettings.IPsecTemplate.DynamicKeysSettings.V1Settings.IKEv1Phase1Settings.SALifeTime / 60;
            }

            TraceFactory.Logger.Debug("Lifetime configuration:");
            // Maintaing a default timeout for all rules.
            var configurationCmd = "netsh advfirewall set global mainmode mmkeylifetime {0}min".FormatWith(mainModeLifetime);
            TraceFactory.Logger.Debug("Lifetime setting command: {0}".FormatWith(configurationCmd));

            if (!NetworkUtil.ExecuteCommandAndValidate(configurationCmd))
            {
                return false;
            }

            string phase1Rule = string.Empty;

            // Build Authentication-Encryption rule based on custom settings
            if (IKESecurityStrengths.Custom == ruleSettings.IPsecTemplate.DynamicKeysSettings.Strengths)
            {
                // Grathering all Authentication and Encryption for constructing phase 1 rule
                Authentications authentication = Authentications.None;
                Encryptions encryption = Encryptions.None;

                if (null != ruleSettings.IPsecTemplate.DynamicKeysSettings.V1Settings.IKEv1Phase1Settings)
                {
                    authentication |= ruleSettings.IPsecTemplate.DynamicKeysSettings.V1Settings.IKEv1Phase1Settings.Authentication;
                    encryption |= ruleSettings.IPsecTemplate.DynamicKeysSettings.V1Settings.IKEv1Phase1Settings.Encryption;
                }

                if (Encryptions.None != encryption)
                {
                    if (encryption.HasFlag(Encryptions.AES128))
                    {
                        phase1Rule = GeneratePhase1Rule(phase1Rule, Encryptions.AES128, authentication);
                    }

                    if (encryption.HasFlag(Encryptions.AES192))
                    {
                        phase1Rule = GeneratePhase1Rule(phase1Rule, Encryptions.AES192, authentication);
                    }

                    if (encryption.HasFlag(Encryptions.AES256))
                    {
                        phase1Rule = GeneratePhase1Rule(phase1Rule, Encryptions.AES256, authentication);
                    }

                    if (encryption.HasFlag(Encryptions.DES))
                    {
                        phase1Rule = GeneratePhase1Rule(phase1Rule, Encryptions.DES, authentication);
                    }

                    if (encryption.HasFlag(Encryptions.DES3))
                    {
                        phase1Rule = GeneratePhase1Rule(phase1Rule, Encryptions.DES3, authentication);
                    }

                    phase1Rule = phase1Rule.Remove(phase1Rule.Length - 1, 1);
                }
            }
            else
            {
                phase1Rule = GetAuthenticationEncryptionRule(ruleSettings.IPsecTemplate.DynamicKeysSettings.Strengths);
            }

            TraceFactory.Logger.Debug("Phase 1 configuration:");
            configurationCmd = "netsh advfirewall set global mainmode mmsecmethods {0}".FormatWith(phase1Rule);
            TraceFactory.Logger.Debug("Rule: {0}".FormatWith(configurationCmd));

            return NetworkUtil.ExecuteCommandAndValidate(configurationCmd);
        }

        /// <summary>
        /// Reset Advanced Firewall settings and set Public, Domain & Private profiles to 'off'
        /// </summary>
        private static void ResetFirewallSettings()
        {
            TraceFactory.Logger.Debug("Resetting all advanced firewall settings:");

            string configurationCmd = "netsh advfirewall reset";
            TraceFactory.Logger.Debug("Reset command: {0}".FormatWith(configurationCmd));
            NetworkUtil.ExecuteCommandAndValidate(configurationCmd);

            TraceFactory.Logger.Debug("Setting Public, Private and Domain profile states to off:");

            configurationCmd = "netsh advfirewall set public state off";
            TraceFactory.Logger.Debug("Public profile command: {0}".FormatWith(configurationCmd));
            NetworkUtil.ExecuteCommandAndValidate(configurationCmd);

            configurationCmd = "netsh advfirewall set private state off";
            TraceFactory.Logger.Debug("Private profile command: {0}".FormatWith(configurationCmd));
            NetworkUtil.ExecuteCommandAndValidate(configurationCmd);

            configurationCmd = "netsh advfirewall set domain state off";
            TraceFactory.Logger.Debug("Domain profile command: {0}".FormatWith(configurationCmd));
            NetworkUtil.ExecuteCommandAndValidate(configurationCmd);
        }

        /// <summary>
        /// Build Phase 1 rule
        /// </summary>
        /// <param name="phase1Rule">Pre-existing rule</param>
        /// <param name="encryptions">All Encryptions<see cref="Encryptions"/></param>
        /// <param name="authentication">All Authentications<see cref="Authentications"/></param>
        /// <returns>Bulit Phase 1 rule</returns>
        private static string GeneratePhase1Rule(string phase1Rule, Encryptions encryptions, Authentications authentication)
        {
            string group = "dhgroup14:";

            // AESXCBC, Sha384, Sha512 are not applicable for Windows machine
            if (authentication.HasFlag(Authentications.MD5))
            {
                phase1Rule = CheckGroupPrefix(phase1Rule, group);
                phase1Rule += "{0}-{1},".FormatWith(encryptions.ToString().ToLowerInvariant(), Authentications.MD5.ToString().ToLowerInvariant());
            }

            if (authentication.HasFlag(Authentications.SHA1))
            {
                phase1Rule = CheckGroupPrefix(phase1Rule, group);
                phase1Rule += "{0}-{1},".FormatWith(encryptions.ToString().ToLowerInvariant(), Authentications.SHA1.ToString().ToLowerInvariant());
            }

            if (authentication.HasFlag(Authentications.SHA256))
            {
                phase1Rule = CheckGroupPrefix(phase1Rule, group);
                phase1Rule += "{0}-{1},".FormatWith(encryptions.ToString().ToLowerInvariant(), Authentications.SHA256.ToString().ToLowerInvariant());
            }

            return phase1Rule;
        }

        /// <summary>
        /// Check if the rule ends with group
        /// </summary>
        /// <param name="phase1Rule">Pre-Existing Phase 1 rule</param>
        /// <param name="group">Group to check</param>
        /// <returns>Rule constructed with group prefix</returns>
        private static string CheckGroupPrefix(string phase1Rule, string group)
        {
            if (!phase1Rule.EndsWith(group))
            {
                phase1Rule += group;
            }

            return phase1Rule;
        }

        /// <summary>
        /// Returns the client IP Address corresponding to a particular server.
        /// </summary>
        /// <param name="serverIpAddress">IP Address of the server</param>
        /// <param name="addressFamily"></param>
        /// <param name="isLinkLocal"></param>
        /// <returns>The client IP address.</returns>
        public static IPAddress GetClientIpAddress(string serverIpAddress, AddressFamily addressFamily, bool isLinkLocal = false)
        {
            foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
            {
                UnicastIPAddressInformationCollection addresses = item.GetIPProperties().UnicastAddresses;

                if (
                    addresses.Any(x => x.Address.AddressFamily == AddressFamily.InterNetwork && x.Address.IsInSameSubnet(IPAddress.Parse(serverIpAddress),
                                IPAddress.Parse(serverIpAddress).GetSubnetMask())))
                {
                    return
                        addresses.FirstOrDefault(x => x.Address.AddressFamily == addressFamily && (isLinkLocal ? x.Address.IsIPv6LinkLocal : true)).Address;
                }
            }
            return IPAddress.None;
        }

        /// <summary>
        /// Go to HomeScreen
        /// This functionality works only for Missing Cartridge popup
        /// </summary>        
        /// <param name="device">device ip</param>
        /// <returns></returns>
        private static bool GoToHomeScreen(IDevice device)
        {
            try
            {
                //Navigating the control back to home page if the control panel has error screen			
                JediWindjammerDevice jd = device as JediWindjammerDevice;
                JediWindjammerControlPanel cp = jd.ControlPanel;

                //validating whether the current page is in Home
                while (!cp.CurrentForm().Contains("Home"))
                {
                    //Pressing hide button to move the page to home
                    cp.Press("mButton1");
                }
                return true;
            }
            catch (Exception devicePopup)
            {
                //returning false if the error message is other than missing Catridge errors
                TraceFactory.Logger.Info(devicePopup.Message);
                return false;
            }
        }

        /// <summary>
        /// Event raised for every ping request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ping_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            // e.Reply.Address exists for If the status is success else it will have all zero 0.0.0.0
            if (e.Reply.Status == IPStatus.Success)
            {
                var pingStatics = _continuousPingStatistics.FirstOrDefault(n => n.IPAddress == e.Reply.Address.ToString());

                if (e.Reply.Status == IPStatus.Success)
                {
                    pingStatics.PassCount++;
                }

                // assign the status
                pingStatics.Status = e.Reply.Status;

                // calculate pass percentage
                pingStatics.PassPercentage = (pingStatics.PassCount / pingStatics.TotalCount) * 100;
            }
        }

        /// <summary>
        /// Resets the 802.1x security from control panel. Currently not avaiable for Omni/Opus UI.
        /// </summary>
        /// <param name="printerIpAddress">IP Address of the printer.</param>
        /// <returns>true if the operation is successfull, else false.</returns>
        public static bool ResetDot1xControlPanel(string printerIpAddress)
        {
            IDevice device = DeviceFactory.Create(printerIpAddress);

            TraceFactory.Logger.Debug("Enabling IPSecurity through Front Panel");
            try
            {
                //Navigating the control back to home page if the control panel has error screen			
                JediWindjammerDevice jd = device as JediWindjammerDevice;
                JediWindjammerControlPanel cp = jd.ControlPanel;

                //validating whether the current page is in Home
                while (!cp.CurrentForm().Contains("Home"))
                {
                    //Pressing hide button to move the page to home
                    cp.Press("mButton1");
                }

                cp.Press("AdminApp");

                //scroll IncrementButton to get networking control
                while (!cp.GetControls().Contains("NetworkingAndIOMenu"))
                {
                    // Even after the increment button is pressed in the front panel exception occurred, so added try catch block
                    try
                    {
                        cp.Press("IncrementButton");
                    }
                    catch
                    {
                        // Do Nothing
                    }
                }

                //Enabling IPSec through Front Panel                
                cp.Press("NetworkingAndIOMenu");
                cp.Press("JetDirectMenu4");     //Embedded jet direct option
                cp.Press("0x9");                // Security option	
                cp.PressToNavigate("0x48_4", "IOMgrMenuDataSelection", true);

                cp.Press("RESET");

                cp.Press("m_OKButton");
                Thread.Sleep(TimeSpan.FromMinutes(1));

                TraceFactory.Logger.Info("Successfully reset 802.1X Authentication through Front Panel");
                return true;
            }
            catch
            {
                //returning false if the error message is other than missing Cartridge errors   
                TraceFactory.Logger.Info("Failed to reset 802.1X Authentication through Front Panel");
                return false;
            }
        }

        #endregion
    }
}
