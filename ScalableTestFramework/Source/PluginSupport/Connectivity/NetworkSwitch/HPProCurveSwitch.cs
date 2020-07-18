using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity.NetworkSwitch
{
    internal class HPProCurveSwitch : INetworkSwitch
    {
        private const string controlCharacters = "[24;1H[2K[24;1H[2K[24;1H[1;24r[24;1H";

        IPAddress _ipAddress;

        public HPProCurveSwitch(IPAddress ipAddress)
        {
            _ipAddress = ipAddress;
        }

        /// <summary>
        /// Gets switch model
        /// </summary>
        public string Model { get { return "HP ProCurve"; } }

        /// <summary>
        /// Gets switch make
        /// </summary>
        public string Make { get { return "Hewlett-Packard"; } }

        /// <summary>
        /// Gets switch IP address
        /// </summary>
        public IPAddress IPAddress { get { return _ipAddress; } }

        /// <summary>
        /// Get the List of VLAN's configured on Network Switch
        /// </summary>
        /// <returns>List of <see cref="VirtualLAN"/></returns>
        public Collection<VirtualLAN> GetAvailableVirtualLans()
        {
            Collection<VirtualLAN> vlans = new Collection<VirtualLAN>();
            string ipData = string.Empty;
            string identifierData = string.Empty;

            try
            {
                using (TelnetIpc telnet = new TelnetIpc(IPAddress.ToString(), 23))
                {
                    telnet.Connect();
                    telnet.SendLine(" ");
                    Thread.Sleep(1000);
                    telnet.SendLine("show ip");
                    Thread.Sleep(3000);
                    ipData = telnet.ReceiveUntilMatch("$");
                    telnet.SendLine(" ");
                    telnet.SendLine("show vlan");
                    Thread.Sleep(3000);
                    // Gets the identifier data which contains VLAN numbers.
                    identifierData = telnet.ReceiveUntilMatch("$");
                }

                // Once the connection is disposed, a wait is required to make sure that the connection is closed
                Thread.Sleep(TimeSpan.FromMilliseconds(1000));

                vlans = GetVLans(ipData, identifierData);
            }
            catch (Exception ex)
            {
                Logger.LogInfo(ex.Message);
            }

            return vlans;
        }

        /// <summary>
        /// Enable a Network Switch Port
        /// </summary>
        /// <param name="portNumber">Port Number of Network Switch</param>
        /// <returns>true if Enabled successfully, false otherwise</returns>
        public bool EnablePort(int portNumber)
        {
            return EnablePort(portNumber, true);
        }

        /// <summary>
        /// Diable a Network Switch Port
        /// </summary>
        /// <param name="portNumber">Port Number of Network Switch</param>
        /// <returns>true if Disabled successfully, false otherwise</returns>
        public bool DisablePort(int portNumber)
        {
            return EnablePort(portNumber, false);
        }

        /// <summary>
        /// Check whether a Network Switch Port is Disabled
        /// </summary>
        /// <param name="portNumber">Port Number of Network Switch</param>
        /// <returns>true if Disabled, false otherwise</returns>
        public bool IsPortDisabled(int portNumber)
        {
            bool isDisabled = false;

            // eg: Sample data for one interface
            // Positive case    |    Negative case
            // -----------------------------------
            // interface 5      |    interface 5
            // disable          |    no lacp
            // no lacp          |    exit
            // exit             |

            // telnet works with port number 23
            using (TelnetIpc telnet = new TelnetIpc(IPAddress.ToString(), 23))
            {

                try
                {
                    telnet.Connect();
                    telnet.SendLine(" "); // send space to continue to actual telnet prompt
                    telnet.SendLine("show run"); // command to show the configuration details of the switch
                    Thread.Sleep(3000);

                    string configData = string.Empty;

                    while (true)
                    {
                        string data = telnet.ReceiveUntilMatch("$");

                        configData += data;

                        // if output contains -- MORE -- , few more configurations are available to display
                        // send space to get more details.
                        if (data.Contains("-- MORE --"))
                        {
                            telnet.SendLine(" ");
                            Thread.Sleep(3000);
                        }
                        else
                        {
                            // break the loop if it doesn't contain -- MORE --
                            break;
                        }
                    }

                    string[] lines = configData.Split('\n');

                    // walk through each line from the configData
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string line = lines[i];
                        // check to see if the line contains "interface <port number>"
                        if (line.Contains("interface {0}".FormatWith(portNumber)))
                        {
                            for (int j = i + 1; j < lines.Length; j++)
                            {
                                line = lines[j];

                                // if the interface is disabled then turn on the flag and break the loop.
                                if (line.Contains("disable"))
                                {
                                    isDisabled = true;
                                    break;
                                }

                                // if exit is reached then it is end of interface details.
                                if (line.Contains("exit"))
                                {
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogInfo(ex.Message);
                }
            }

            // Once the connection is disposed, a wait is required to make sure that the connection is closed
            Thread.Sleep(TimeSpan.FromMilliseconds(1000));

            return isDisabled;
        }

        /// <summary>
        /// Get log details of Network Switch
        /// </summary>
        /// <returns>Log Data of Network Switch</returns>
        public string GetLog()
        {
            string logData = string.Empty;
            using (TelnetIpc telnet = new TelnetIpc(IPAddress.ToString(), 23))
            {

                try
                {
                    telnet.Connect();
                    telnet.SendLine(" ");
                    Thread.Sleep(1000);
                    telnet.SendLine("show log -r");
                    Thread.Sleep(3000);
                    string data = telnet.ReceiveUntilMatch("$");

                    while (data.Contains("-- MORE --"))
                    {
                        logData += data;
                        telnet.SendLine(" ");
                        Thread.Sleep(3000);
                        data = telnet.ReceiveUntilMatch("$");
                    }

                    logData += data;
                }
                catch (Exception ex)
                {
                    Logger.LogInfo(ex.Message);
                }
            }

            return logData;
        }
        ///<summary>
		/// Parse raw data to get VLAN details
		/// </summary>
		/// <param name="data">Data when "show ip" telnet command is executed</param>
		/// <param name="vlanIdentifierData">Data from "show vlan" telnet command which contains VLAn numbers information.</param>
		/// <returns><see cref="VirtualLAN"/></returns>        
        private Collection<VirtualLAN> GetVLans(string data, string vlanIdentifierData)
        {
            Collection<VirtualLAN> vlanList = new Collection<VirtualLAN>();

            // Remove '\r' characters from the string
            data = data.Replace("\r", string.Empty);
            string[] lines = data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            // Position the index at the starting of the VLAN information.
            int position = Array.FindIndex(lines, x => x.Contains("VLAN"));

            // If position is -1, no VLANs are configured on the switch.
            if (position == -1)
            {
                Logger.LogInfo("VLANs are not configured on the switch.");
                return vlanList;
            }

            // Move the pointer to the VLAN Data table
            for (int i = ++position; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                VirtualLAN vlanDetails;
                if (GetVLan(line, vlanIdentifierData, out vlanDetails))
                {
                    vlanList.Add(vlanDetails);
                }
            }

            return vlanList;
        }

        /// <summary>
        /// Construct the VLan structure for the data
        /// </summary>
        /// <param name="line">VLan data</param>
        /// <param name="vlanIdentifierData">Data from "show vlan" telnet command which contains VLAn numbers information.</param>
        private bool GetVLan(string line, string vlanIdentifierData, out VirtualLAN vlanDetails)
        {
            Regex ipAddressRegex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
            bool isIPAddress = false;
            vlanDetails = new VirtualLAN();
            string[] localData;

            if (line.Contains('|'))
            {
                vlanDetails.Name = line.Split('|')[0].Trim();
                localData = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < localData.Length; j++)
                {
                    //IP Config Method
                    if ((localData[j].StartsWith("DHCP", StringComparison.CurrentCulture)) || (localData[j].StartsWith("Manual", StringComparison.CurrentCulture)))
                    {
                        vlanDetails.ConfigMethod = localData[j];
                    }

                    //IP Address
                    if (!isIPAddress && ipAddressRegex.IsMatch(localData[j]))
                    {
                        vlanDetails.IPAddress = IPAddress.Parse(localData[j]);
                        isIPAddress = true;
                        continue;
                    }

                    //Subnet mask
                    if (isIPAddress && ipAddressRegex.IsMatch(localData[j]))
                    {
                        vlanDetails.SubnetMask = IPAddress.Parse(localData[j]);
                    }
                }

                // Get the Virtual LAN identifier
                vlanDetails.Identifier = GetVlanIdentifier(vlanIdentifierData, vlanDetails.Name);

                // Get the Virtual LAN port numbers
                vlanDetails.ConfiguredPorts = GetVlanPorts(vlanDetails.Identifier);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the Virtual LAN identifier from the VLAN name.
        /// </summary>
        /// <param name="vlanIdentifierData">Data holding information about VLAN identifier.</param>
        /// <param name="vlanName">The VLAN name.</param>
        /// <returns>The VLAN identifier.</returns>
        private static int GetVlanIdentifier(string vlanIdentifierData, string vlanName)
        {
            int vlanIdentifier = 0;
            vlanIdentifierData = vlanIdentifierData.Replace("\r", string.Empty);

            // Split the data removing empty entries to fetch the VLAN identifier.
            string[] lines = vlanIdentifierData.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Position the index at the starting of the VLAN ID information.
            int position = Array.FindIndex(lines, x => x.Contains("VLAN ID"));

            // If position is -1, no VLANs are configured on the switch.
            if (position == -1)
            {
                Logger.LogInfo("VLANs are not configured on the switch.");
                return -1;
            }

            for (int i = position + 1; i < lines.Count(); i++)
            {
                if (lines[i].Contains(vlanName, StringComparison.CurrentCultureIgnoreCase))
                {
                    vlanIdentifier = Convert.ToInt32(lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0], CultureInfo.CurrentCulture);
                    break;
                }
            }

            return vlanIdentifier;
        }

        /// <summary>
        /// Gets the configured pots for the VLAN number.
        /// </summary>
        /// <param name="vlanIdentifier">The VLAN number.</param>
        /// <returns>The VLAN ports configured.</returns>
        public Collection<int> GetVlanPorts(int vlanIdentifier)
        {
            Collection<int> vlans = new Collection<int>();
            string portData = string.Empty;

            try
            {
                using (TelnetIpc telnet = new TelnetIpc(IPAddress.ToString(), 23))
                {
                    telnet.Connect();
                    telnet.SendLine(" ");
                    Thread.Sleep(1000);
                    telnet.SendLine("show vlan {0}".FormatWith(vlanIdentifier));
                    Thread.Sleep(4000);
                    portData = telnet.ReceiveUntilMatch("$");
                }

                // Once the connection is disposed, a wait is required to make sure that the connection is closed
                Thread.Sleep(TimeSpan.FromMilliseconds(1000));

                portData = portData.Replace("\r", string.Empty);
                string[] lines = portData.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                // Position at the VLAN Port Information table in the array
                int position = Array.FindIndex(lines, x => x.Contains("Port Information"));

                // If position is -1, no VLANs are configured on the switch.
                if (position == -1)
                {
                    Logger.LogInfo("Configured port information is not available on the switch for VLAN : {0}.".FormatWith(vlanIdentifier));
                    return vlans;
                }

                for (++position; position < lines.Count(); position++)
                {
                    if (lines[position].Contains("Untagged"))
                    {
                        vlans.Add(Convert.ToInt32(lines[position].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0], CultureInfo.CurrentCulture));
                    }
                }

                return vlans;
            }
            catch (SocketException ex)
            {
                Logger.LogInfo(ex.Message);
                return vlans;
            }
        }

        /// <summary>
        /// Enable/ Disable a Network Switch Port
        /// </summary>
        /// <param name="portNo">Port Number of Network Switch</param>
        /// <param name="enablePort">true to Enable, false to Disable</param>
        /// <returns>true if successful, false otherwise</returns>
        private bool EnablePort(int portNo, bool enablePort)
        {
            bool result = false;
            string enable = (enablePort.Equals(true) ? " enable" : " disable");

            Logger.LogInfo("{0} port: {1}.".FormatWith(enable, portNo));

            string[] commands = {" ", "configure",
                                "interface ethernet " + portNo + enable
                                };
            using (TelnetIpc telnet = new TelnetIpc(IPAddress.ToString(), 23))
            {

                try
                {
                    telnet.Connect();

                    for (int i = 0; i < commands.Length; i++)
                    {
                        telnet.SendLine(commands[i]);
                        Thread.Sleep(1000);
                    }

                    result = IsPortDisabled(portNo);
                }
                catch (Exception ex)
                {
                    Logger.LogInfo(ex.Message);
                    return false;
                }
            }

            // Once the connection is disposed, a wait is required to make sure that the connection is closed
            Thread.Sleep(TimeSpan.FromMilliseconds(1000));

            result = enablePort.Equals(false) ? result : !result;

            if (result)
            {
                Logger.LogInfo("Successfully {0}d port: {1}.".FormatWith(enable, portNo));
            }
            else
            {
                Logger.LogInfo("Failed to {0} port: {1}.".FormatWith(enable, portNo));
            }

            return result;
        }

        /// <summary>
        /// Enables 802.1X Authentication for the switch port
        /// </summary>
        /// <param name="portNumber">Port to be enabled</param>
        /// <param name="timeOut">time to wait after the telnet connection is disposed</param>
        public void EnableAuthenticatorPort(int portNumber, int timeOut = 1000)
        {
            Logger.LogInfo("Enabling authentication on port: {0}.".FormatWith(portNumber));

            using (TelnetIpc telnet = new TelnetIpc(IPAddress.ToString(), 23))
            {
                try
                {
                    telnet.Connect();
                    telnet.SendLine("/n");
                    telnet.SendLine("conf t");
                    telnet.SendLine(string.Format(CultureInfo.CurrentCulture, "aaa port-access authenticator {0}", portNumber));
                    Thread.Sleep(1000);
                    telnet.SendLine("wr mem");
                }
                catch (Exception ex)
                {
                    Logger.LogInfo(ex.Message);
                    throw;
                }
            }

            // Once the connection is disposed, a wait is required to make sure that the connection is closed
            Thread.Sleep(TimeSpan.FromMilliseconds(1000));
        }

        /// <summary>
        /// Disables 802.1X Authentication for the switch port
        /// </summary>
        /// <param name="portNumber">Port to be disabled</param>
        public void DisableAuthenticatorPort(int portNumber)
        {
            Logger.LogInfo("Disabling authentication on port: {0}.".FormatWith(portNumber));

            using (TelnetIpc telnet = new TelnetIpc(IPAddress.ToString(), 23))
            {
                try
                {
                    telnet.Connect();
                    telnet.SendLine("/n");
                    telnet.SendLine("conf t");
                    telnet.SendLine(string.Format(CultureInfo.CurrentCulture, "no aaa port-access authenticator {0}", portNumber));
                    Thread.Sleep(1000);
                    telnet.SendLine("wr mem");
                }
                catch (Exception ex)
                {
                    Logger.LogInfo(ex.Message);
                }
            }

            // Once the connection is disposed, a wait is required to make sure that the connection is closed
            Thread.Sleep(TimeSpan.FromMilliseconds(1000));
        }

        /// <summary>
        /// Changes the network of a port by keeping the port in the destination VLAN.
        /// </summary>
        /// <param name="portNumber">Port number to be changed to destination VLAN.</param>
        /// <param name="destinationVlanIdentifer">The destination VLAN identifier.</param>
        /// <returns>true if successfully changed the port to the specified VLAN, else false</returns>
        public bool ChangeVirtualLan(int portNumber, int destinationVlanIdentifer)
        {
            try
            {
                using (TelnetIpc telnet = new TelnetIpc(IPAddress.ToString(), 23))
                {
                    telnet.Connect();
                    telnet.SendLine("/n");
                    telnet.SendLine("conf t");
                    telnet.SendLine("vlan {0}".FormatWith(destinationVlanIdentifer));
                    Thread.Sleep(TimeSpan.FromMilliseconds(1000));
                    telnet.SendLine("untagged {0}".FormatWith(portNumber));
                    Thread.Sleep(TimeSpan.FromMilliseconds(1000));
                    telnet.SendLine("wr mem");
                    Thread.Sleep(TimeSpan.FromMilliseconds(1000));
                }

                // Once the connection is disposed, a wait is required to make sure that the connection is closed
                Thread.Sleep(TimeSpan.FromMilliseconds(1000));

                // Validate the port number in destination VLAN
                if (GetVlanPorts(destinationVlanIdentifer).Contains(portNumber))
                {
                    Logger.LogInfo("Successfully changed the network of port number {0} to vlan {1}.".FormatWith(portNumber, destinationVlanIdentifer));
                    return true;
                }
                else
                {
                    Logger.LogInfo("Failed to change the network of port number {0} to vlan {1}.".FormatWith(portNumber, destinationVlanIdentifer));
                    return false;
                }
            }
            catch (SocketException ex)
            {
                Logger.LogInfo("Socket Exception occurred: {0}.".FormatWith(ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Set the link speed of the port to the specified value.
        /// </summary>
        /// <param name="portNumber">The port number to be configured.</param>
        /// <param name="linkSpeed"><see cref="LinkSpeed"/></param>
        /// <returns>True if the link speed is set, else false.</returns>
        public bool SetLinkSpeed(int portNumber, LinkSpeed linkSpeed)
        {
            try
            {
                Logger.LogInfo("Setting link speed: {0} on port number: {1}".FormatWith(Enum<LinkSpeed>.Value(linkSpeed), portNumber));

                using (TelnetIpc telnet = new TelnetIpc(IPAddress.ToString(), 23))
                {
                    telnet.Connect();
                    telnet.SendLine("/n");
                    telnet.SendLine("conf t");
                    telnet.SendLine("interface ethernet {0} speed-duplex {1}".FormatWith(portNumber, Enum<LinkSpeed>.Value(linkSpeed)));
                    Thread.Sleep(TimeSpan.FromSeconds(20));
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));

                // Link Speed validation is removed as the switch link speed changes w.r.t printer connection on the port.
                Logger.LogInfo("Successfully set link speed: {0} on port number: {1}".FormatWith(Enum<LinkSpeed>.Value(linkSpeed), portNumber));
                return true;
            }
            catch (SocketException ex)
            {
                Logger.LogInfo("Failed to set link speed: {0} on port number: {1}".FormatWith(Enum<LinkSpeed>.Value(linkSpeed), portNumber));
                Logger.LogInfo(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Failed to set link speed: {0} on port number: {1}".FormatWith(Enum<LinkSpeed>.Value(linkSpeed), portNumber));
                Logger.LogInfo(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Get the link speed of the specified port.
        /// </summary>
        /// <param name="portNumber">The port number.</param>
        /// <returns>The link speed for the specified port.</returns>
        /// Note : Can not return the <see cref="LinkSpeed"/> since there are multiple enum values mapped to the port link speed.
        /// eg: 1000FDX corresponds to auto and auto-1000
        ///		100FDX corresponds to 100-full and auto-100
        private string GetLinkSpeed(int portNumber)
        {
            /*  ????[2J[?7l[3;23r[?6l[1;1H[?25l[1;1HProCurve J4903A Switch 2824
           Copyright (C) 1991-2009 Hewlett-Packard Co.  All Rights Reserved.
           RESTRICTED RIGHTS LEGEND
           Use, duplication, or disclosure by the Government is subject to restrictions
           as set forth in subdivision (b) (3) (ii) of the Rights in Technical Data and Computer Software clause at 52.227-7013.
           HEWLETT-PACKARD COMPANY, 3000 Hanover St., Palo Alto, CA 94303
           We'd like to keep you up to date about:
           * Software feature updates
           * New product announcements
           * Special events
           Please register your products now at:  www.ProCurve.com
           [24;1HPress any key to continue[1;1H[?25h[24;27H[2J[?7l[3;23r[?6l[24;27H[?25h[24;27H[?6l[1;24r[?7l[2J[24;27H[1;24r[24;27H[2J[?7l[1;24r[?6l[24;1H[1;24r[24;1H[24;1H[2K[24;1H[?25h[24;1H[24;1HProCurve Switch 2824# [24;1H[24;23H[24;1H[?25h[24;23H[24;23Hconf t[24;23H[?25h[24;29H[24;0HE[24;1H[24;29H[24;1H[2K[24;1H[?25h[24;1H[1;24r[24;1H[1;24r[24;1H[24;1H[2K[24;1H[?25h[24;1H[24;1HProCurve Switch 2824(config)# [24;1H[24;31H[24;1H[?25h[24;31H[24;31Hshow inter[24;31H[?25h[24;41H[24;41Hface brief[24;41H[?25h[24;51H[24;51H ethernet [24;51H[?25h[24;61H[24;61H24[24;61H[?25h[24;63H[24;0HE[24;1H[24;63H[24;1H[2K[24;1H[?25h[24;1H[1;24r[24;1H
           Status and Counters - Port Status
                           | Intrusion                           MDI   Flow  Bcast 
           Port  Type      | Alert     Enabled Status Mode       Mode  Ctrl  Limit 
           ----- --------- + --------- ------- ------ ---------- ----- ----- ------
           24    100/1000T | No        Yes     Down   100FDx     MDI   off   0     
           [1;24r[24;1H[24;1H[2K[24;1H[?25h[24;1H[24;1HProCurve Switch 2824(config)# [24;1H[24;31H[24;1H[?25h[24;31H*/

            try
            {
                string result = string.Empty;

                using (TelnetIpc telnet = new TelnetIpc(IPAddress.ToString(), 23))
                {
                    telnet.Connect();
                    telnet.SendLine("/n");
                    telnet.SendLine("conf t");
                    telnet.SendLine("show interface brief ethernet {0}".FormatWith(portNumber));
                    Thread.Sleep(TimeSpan.FromSeconds(20));

                    result = telnet.ReceiveUntilMatch("$");
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (!string.IsNullOrEmpty(result))
                {
                    result = result.Replace("\r", "").Trim();
                    string[] data = result.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    int index = Array.FindIndex(data, x => x.TrimStart().StartsWith("Port", StringComparison.CurrentCultureIgnoreCase));

                    if (-1 == index)
                    {
                        Logger.LogInfo("Failed to fetch the value of link speed for port: {0}.".FormatWith(portNumber));
                        return string.Empty;
                    }

                    // Skip the next line and fetch the speed information for the port.
                    string[] portSpeedData = data[index + 2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    return portSpeedData[6].Trim();
                }
                else
                {
                    Logger.LogInfo("Failed to fetch the value of link speed for port: {0}.".FormatWith(portNumber));
                    return string.Empty;
                }
            }
            catch (SocketException ex)
            {
                Logger.LogInfo("Failed to fetch the value of link speed for port: {0}.".FormatWith(portNumber));
                Logger.LogInfo(ex.Message);
                return string.Empty;
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Failed to fetch the value of link speed for port: {0}.".FormatWith(portNumber));
                Logger.LogInfo(ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// Validate the link speed for the specified port
        /// </summary>
        /// <param name="portNumber">The port number.</param>
        /// <param name="linkSpeed"><see cref="LinkSpeed"/></param>
        /// <returns>true if the link speed is set successfully, else false.</returns>
        /// Note:	10HDX 10-half,auto-10
        ///			10FDX 10-full
        ///			100HDX 100-half
        ///			100FDX 100-full, auto-100
        ///			1000FDX auto, auto-1000
        private bool ValidateLinkSpeed(int portNumber, LinkSpeed linkSpeed)
        {
            string speed = GetLinkSpeed(portNumber);

            if ((linkSpeed == LinkSpeed.HalfDuplex10Mbps | linkSpeed == LinkSpeed.Auto10Mbps) && speed.EqualsIgnoreCase("10HDX"))
            {
                return true;
            }
            else if (linkSpeed == LinkSpeed.FullDuplex10Mbps && speed.EqualsIgnoreCase("10FDX"))
            {
                return true;
            }
            else if ((linkSpeed == LinkSpeed.FullDuplex100Mbps | linkSpeed == LinkSpeed.Auto100Mbps) && speed.EqualsIgnoreCase("100FDX"))
            {
                return true;
            }
            else if ((linkSpeed == LinkSpeed.Auto | linkSpeed == LinkSpeed.Auto1000Mbps) && speed.EqualsIgnoreCase("1000FDX"))
            {
                return true;
            }
            //added this condition since TPS Switch link speed has value 100FDX for auto
            else if ((linkSpeed == LinkSpeed.Auto) && speed.EqualsIgnoreCase("100FDX"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Configure radius server on the switch.
        /// </summary>
        /// <param name="radiusServer">IP address of the radius server.</param>
        /// <param name="sharedSecret">Shared secret for the radius server configuration.</param>
        /// <returns>True if the configuration is successful, else false.</returns>
        public bool ConfigureRadiusServer(IPAddress radiusServer, string sharedSecret)
        {
            try
            {
                using (TelnetIpc telnet = new TelnetIpc(IPAddress.ToString(), 23))
                {
                    telnet.Connect();
                    telnet.SendLine("/n");
                    telnet.SendLine("conf t");
                    telnet.SendLine("aaa authentication port-access eap-radius");
                    telnet.SendLine("radius-server host {0} key {1}".FormatWith(radiusServer.ToString(), sharedSecret));
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    telnet.SendLine("aaa port-access authenticator active");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }

                Thread.Sleep(TimeSpan.FromSeconds(20));

                if (GetRadiusServers().Contains(radiusServer))
                {
                    Logger.LogInfo("Successfully configured the radius server: {0} on the switch.".FormatWith(radiusServer));
                    return true;
                }
                else
                {
                    Logger.LogInfo("Failed to configure the radius server: {0} on the switch.".FormatWith(radiusServer));
                    return false;
                }
            }
            catch (SocketException ex)
            {
                Logger.LogInfo("Failed to configure the radius server: {0} on the switch.".FormatWith(radiusServer));
                Logger.LogInfo(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Failed to configure the radius server: {0} on the switch.".FormatWith(radiusServer));
                Logger.LogInfo(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// DeConfigure/Remove radius server details from the switch.
        /// </summary>
        /// <returns>True if the configuration is successful, else false.</returns>
        public bool DeconfigureRadisuServer(IPAddress radiusServer)
        {
            try
            {
                using (TelnetIpc telnet = new TelnetIpc(IPAddress.ToString(), 23))
                {
                    telnet.Connect();
                    telnet.SendLine("/n");
                    telnet.SendLine("conf t");
                    telnet.SendLine("no radius-server host {0}".FormatWith(radiusServer.ToString()));
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }

                Thread.Sleep(TimeSpan.FromSeconds(20));

                if (!GetRadiusServers().Contains(radiusServer))
                {
                    Logger.LogInfo("Successfully removed the radius server: {0} on the switch.".FormatWith(radiusServer));
                    return true;
                }
                else
                {
                    Logger.LogInfo("Failed to remove the radius server: {0} on the switch.".FormatWith(radiusServer));
                    return false;
                }
            }
            catch (SocketException ex)
            {
                Logger.LogInfo("Failed to remove the radius server: {0} on the switch.".FormatWith(radiusServer));
                Logger.LogInfo(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Failed to remove the radius server: {0} on the switch.".FormatWith(radiusServer));
                Logger.LogInfo(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Gets all the radius servers configured on the switch.
        /// </summary>
        /// <returns></returns>
        public Collection<IPAddress> GetRadiusServers()
        {
            string configData = string.Empty;

            using (TelnetIpc telnet = new TelnetIpc(IPAddress.ToString(), 23))
            {
                telnet.Connect();
                telnet.SendLine("/n");
                telnet.SendLine("show run");
                Thread.Sleep(TimeSpan.FromSeconds(10));

                configData = telnet.ReceiveUntilMatch("$");

                string data = configData;

                while (data.Contains("more", StringComparison.CurrentCultureIgnoreCase))
                {
                    telnet.SendLine(" ");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    data = telnet.ReceiveUntilMatch("$");
                    configData += data;
                }

                configData = configData.Replace(controlCharacters, string.Empty).Replace("/r", string.Empty).Trim();
            }
            // Wait time for the telnet connection to dispose
            Thread.Sleep(TimeSpan.FromSeconds(30));

            string[] radiusServerData = Array.FindAll(configData.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries), x => x.Contains("radius-server", StringComparison.CurrentCultureIgnoreCase));

            Regex ipAddressRegex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");

            return new Collection<IPAddress>(radiusServerData.Select(x => IPAddress.Parse(ipAddressRegex.Match(x).Value)).ToArray());
        }

        /// <summary>
        /// DeConfigure/Remove all radius server details from the switch.
        /// </summary>
        /// <returns>True if the configuration is successful, else false.</returns>
        public bool DeConfigureAllRadiusServer()
        {
            foreach (IPAddress radiusServer in GetRadiusServers())
            {
                if (!DeconfigureRadisuServer(radiusServer))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
