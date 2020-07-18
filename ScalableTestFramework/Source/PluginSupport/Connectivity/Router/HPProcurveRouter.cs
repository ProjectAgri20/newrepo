using System;
using System.Collections.Generic;
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

namespace HP.ScalableTest.PluginSupport.Connectivity.Router
{
    /// <summary>
    /// Class represents the functionalities available for HP Procurve Router
    /// </summary>
    internal class HPProcurveRouter : IRouter
    {
        #region Private Variables

        IPAddress _adress;
        string _userName;
        string _password;
        Collection<RouterVirtualLAN> _routerVlans;

        #endregion

        #region Contants

        const string controlCharacters = "[24;1H[2K[24;1H[2K[24;1H[1;24r[24;1H";
        const int VALID_LIFE_TIME = 2592000;
        const int PREFERRED_LIFE_TIME = 2160000;

        #endregion

        #region Private properties

        private Collection<RouterVirtualLAN> RouterVlans
        {
            get
            {
                if (null == _routerVlans)
                {
                    GetRouterVirtualLans();
                }

                return _routerVlans;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of <see cref="HPProcurveRouter"/>
        /// </summary>
        /// <param name="address">IP address of the router.</param>
        /// <param name="userName">User name of the router.</param>
        /// <param name="password">Password of the router.</param>
        public HPProcurveRouter(IPAddress address, string userName, string password)
        {
            _adress = address;
            _userName = userName;
            _password = password;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Model of the router.
        /// </summary>
        public string Model
        {
            get { return "Procurve"; }
        }

        /// <summary>
        /// Manufacturer name of the router.
        /// </summary>
        public string Make
        {
            get { return "HP"; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the IPv6 addresses from <see cref="RouterIPv6Details"/> collection.
        /// </summary>
        /// <param name="ipv6Details"><see cref="RouterIPv6Details"/></param>
        /// <returns>Collection of IPv6 address.</returns>
        public Collection<IPAddress> GetIPv6Addresses(Collection<RouterIPv6Details> ipv6Details)
        {
            return new Collection<IPAddress>((from n in ipv6Details select n.IPv6Address).ToList());
        }

        /// <summary>
        /// Gets the identifiers and IP addresses of the available virtual LANs.
        /// </summary>
        /// <returns>returns a dictionary of virtual LAN identifiers and IP Addresses.</returns>
        public Dictionary<int, IPAddress> GetAvailableVirtualLans()
        {
            Dictionary<int, IPAddress> vlanDetails = new Dictionary<int, IPAddress>();

            foreach (var item in RouterVlans)
            {
                vlanDetails.Add(item.Identifier, item.IPv4Address);
            }

            return vlanDetails;
        }

        /// <summary>
        /// Gets the details of a virtual LAN.
        /// </summary>
        /// <param name="routerVlanId">The virtual LAN id.</param>
        /// <returns><see cref="RouterVirtualLAN"/></returns>
        public RouterVirtualLAN GetVirtualLanDetails(int routerVlanId)
        {
            string vlanData = string.Empty;
            RouterVirtualLAN routerVlan = RouterVlans.Where(x => x.Identifier.Equals(routerVlanId)).FirstOrDefault();

            using (TelnetIpc telnet = new TelnetIpc(_adress.ToString(), 23))
            {
                telnet.Connect();
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(_userName);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(_password);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine("en");
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine("show run vlan {0}".FormatWith(routerVlanId));
                Thread.Sleep(TimeSpan.FromSeconds(10));

                vlanData = telnet.ReceiveUntilMatch("$");

                while (vlanData.Contains("more", StringComparison.CurrentCultureIgnoreCase))
                {
                    vlanData = vlanData.Remove(vlanData.IndexOf("-- more", StringComparison.CurrentCultureIgnoreCase));
                    telnet.SendLine(" ");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    vlanData += telnet.ReceiveUntilMatch("$");
                }

                vlanData = vlanData.Replace(controlCharacters, string.Empty).Replace("/r", string.Empty).Trim();
            }


            GetVlanDetails(vlanData, ref routerVlan);

            return routerVlan;
        }

        /// <summary>
        /// Enable IPv6 for a particular virtual LAN.
        /// </summary>
        /// <param name="routerVlanId">The virtual LAN id.</param>
        /// <param name="ipv6Addresses">List of addresses to be added for the virtual LAN.</param>
        /// <returns>True if IPv6 is enabled, else false.</returns>
        public bool EnableIPv6Address(int routerVlanId, Collection<IPAddress> ipv6Addresses)
        {
            if (null == ipv6Addresses || ipv6Addresses.Count == 0)
            {
                Logger.LogInfo("IPv6 addresses can not be empty.");
                return false;
            }

            Logger.LogInfo("Enabling IPv6 addresses in router.");

            // TODO: Remove hard coded prefix value.
            string ipData = "ipv6 address {0}/64\n";
            string prefixData = "ipv6 nd ra prefix {0}/{1} {2} {3}\n";
            string command = string.Empty;

            foreach (IPAddress ipv6Address in ipv6Addresses)
            {
                command += ipData.FormatWith(ipv6Address.ToString());
                //TODO: Get a logic to fetch the address prefix
                command += prefixData.FormatWith(GetPrefix(ipv6Address), 64, VALID_LIFE_TIME, PREFERRED_LIFE_TIME);
            }

            using (TelnetIpc telnet = new TelnetIpc(_adress.ToString(), 23))
            {
                telnet.Connect();
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(_userName);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(_password);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine("en");
                telnet.SendLine("conf t");
                telnet.SendLine("vlan {0}".FormatWith(routerVlanId));
                telnet.SendLine(command);
                telnet.SendLine("wr mem");
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }

            // Validation
            RouterVirtualLAN routerVlan = GetVirtualLanDetails(routerVlanId);
            bool isv6AddressPresesnt = false;

            foreach (IPAddress address in ipv6Addresses)
            {

                if (null == routerVlan.IPv6Details.Where(x => x.IPv6Address.Equals(address)).FirstOrDefault())
                {
                    isv6AddressPresesnt = false;
                    Logger.LogInfo("Failed to enable IPv6 addresses on VLAN: {0}".FormatWith(routerVlanId));
                    return false;
                }
                else
                {
                    isv6AddressPresesnt = true;
                }
            }

            if (isv6AddressPresesnt)
            {
                Logger.LogInfo("Successfully enabled IPv6 addresses on VLAN: {0}".FormatWith(routerVlanId));
            }

            return isv6AddressPresesnt;
        }

        /// <summary>
        /// Disable IPv6 for a particular virtual LAN.
        /// </summary>
        /// <param name="routerVlanId">The virtual LAN id.</param>
        /// <returns>True if IPv6 is disabled, else false.</returns>
        public bool DisableIPv6Address(int routerVlanId)
        {
            Logger.LogInfo("Disabling IPv6 addresses in router.");

            RouterVirtualLAN routerVlan = GetVirtualLanDetails(routerVlanId);

            Collection<RouterIPv6Details> routerIpv6Details = routerVlan.IPv6Details;


            SetRouterFlag(routerVlanId, RouterFlags.None);

            // TODO: Remove hard coded prefix value.

            string ipData = "no ipv6 address {0}/{1}\n";
            string prefixData = "no ipv6 nd ra prefix {0}/{1}\n";
            string disableIPv6 = "no ipv6 enable\n";
            string reachableTime = "no ipv6 nd reachable-time\n";
            string reachableTimera = "no ipv6 nd ra reachable-time\n";
            string linklocal = "no ipv6 address {0} link-local\n".FormatWith(routerVlan.LinkLocalAddress);
            //  string ipv6helperAddress = "no ipv6 helper-address unicast {0}\n".FormatWith(routerVlan.IPv6HelperAddress);

            string command = string.Empty;
            command += disableIPv6;
            foreach (RouterIPv6Details ipv6Details in routerIpv6Details)
            {
                if (!ipv6Details.IPv6Address.Equals(IPAddress.IPv6None))
                {
                    command += ipData.FormatWith(ipv6Details.IPv6Address, ipv6Details.PrefixLength);
                }

                command += prefixData.FormatWith(ipv6Details.AddressPrefix, ipv6Details.PrefixLength);
            }
            command += reachableTime;
            command += reachableTimera;
            command += linklocal;
            // command += ipv6helperAddress;

            using (TelnetIpc telnet = new TelnetIpc(_adress.ToString(), 23))
            {
                telnet.Connect();
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(_userName);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(_password);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine("en");
                telnet.SendLine("conf t");
                telnet.SendLine("vlan {0}".FormatWith(routerVlanId));
                telnet.SendLine(command);

                telnet.SendLine("wr mem");
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }

            // Validation
            routerVlan = GetVirtualLanDetails(routerVlanId);

            if (routerVlan.IPv6Details.Count == 0)
            {
                Logger.LogInfo("Successfully disabled IPv6 addresses on VLAN: {0}".FormatWith(routerVlanId));
                return true;
            }
            else
            {
                Logger.LogInfo("Failed to disable IPv6 addresses on VLAN: {0}".FormatWith(routerVlanId));
                return false;
            }
        }

        /// <summary>
        /// Set the Lease time.
        /// </summary>
        /// <param name="routerVlanId">The virtual LAN id.</param>
        /// <param name="validLifeTime"><see cref="LeaseTime"/></param>
        /// <param name="preferredLifeTime">The lease time.</param>
        /// <returns>True if the lease time is set, else false.</returns>
        public bool SetLeaseTime(int routerVlanId, TimeSpan validLifeTime, TimeSpan preferredLifeTime)
        {
            RouterVirtualLAN routerVlan = GetVirtualLanDetails(routerVlanId);

            Collection<RouterIPv6Details> ipv6Details = routerVlan.IPv6Details;

            string setLifeTime = "ipv6 nd ra prefix {0}/{1} {2} {3}\n";
            string clearLifeTime = "no ipv6 nd ra prefix {0}/{1}\n";
            string command = string.Empty;

            if (validLifeTime.TotalSeconds < preferredLifeTime.TotalSeconds)
            {
                Logger.LogInfo("Valid life time should be more than preferred life time.");
                return false;
            }

            foreach (RouterIPv6Details v6Details in ipv6Details)
            {
                command += clearLifeTime.FormatWith(v6Details.AddressPrefix, v6Details.PrefixLength);
                command += setLifeTime.FormatWith(v6Details.AddressPrefix, v6Details.PrefixLength, validLifeTime.TotalSeconds, preferredLifeTime.TotalSeconds);
            }

            using (TelnetIpc telnet = new TelnetIpc(_adress.ToString(), 23))
            {
                telnet.Connect();
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(_userName);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(_password);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine("en");
                telnet.SendLine("conf t");
                telnet.SendLine("vlan {0}".FormatWith(routerVlanId));
                telnet.SendLine(command);
                telnet.SendLine("wr mem");
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }

            routerVlan = GetVirtualLanDetails(routerVlanId);

            return routerVlan.IPv6Details.Any(x => !x.ValidLeaseTime.Equals(validLifeTime.TotalSeconds))
                    && routerVlan.IPv6Details.Any(x => !x.PreferredLeaseTime.Equals(preferredLifeTime.TotalSeconds));

        }

        /// <summary>
        /// Set the flags for a particular virtual LAN.
        /// </summary>
        /// <param name="routerVlanId">The virtual LAN id.</param>
        /// <param name="routerFlag"><see cref="RouterFlags"/></param>
        /// <returns>True if the flag is set, else false.</returns>
        public bool SetRouterFlag(int routerVlanId, RouterFlags routerFlag)
        {
            bool result = false;
            string flagDescription = RouterFlags.None == routerFlag ? "Disabling M and O" : "Setting {0}".FormatWith(routerFlag);

            Logger.LogInfo("{0} flag on router VLAN : {1}".FormatWith(flagDescription, routerVlanId));

            using (TelnetIpc telnet = new TelnetIpc(_adress.ToString(), 23))
            {
                telnet.Connect();
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(_userName);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(_password);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine("en");
                telnet.SendLine("conf t");
                telnet.SendLine("vlan {0}".FormatWith(routerVlanId));

                // TODO: Remove multiple if else
                if (routerFlag == RouterFlags.Managed)
                {
                    telnet.SendLine(Enum<RouterFlags>.Value(RouterFlags.Managed));
                    telnet.SendLine(("no {0}".FormatWith(Enum<RouterFlags>.Value(RouterFlags.Other))));
                }
                else if (routerFlag == RouterFlags.Other)
                {
                    telnet.SendLine(Enum<RouterFlags>.Value(RouterFlags.Other));
                    telnet.SendLine(("no {0}".FormatWith(Enum<RouterFlags>.Value(RouterFlags.Managed))));
                }
                else if (routerFlag == RouterFlags.Both)
                {
                    telnet.SendLine(Enum<RouterFlags>.Value(RouterFlags.Other));
                    telnet.SendLine(Enum<RouterFlags>.Value(RouterFlags.Managed));
                }
                else
                {
                    telnet.SendLine(("no {0}".FormatWith(Enum<RouterFlags>.Value(RouterFlags.Managed))));
                    telnet.SendLine(("no {0}".FormatWith(Enum<RouterFlags>.Value(RouterFlags.Other))));
                }

                telnet.SendLine("wr mem");

                Thread.Sleep(TimeSpan.FromSeconds(10));

                telnet.SendLine("sh run vlan {0}".FormatWith(routerVlanId));
                Thread.Sleep(TimeSpan.FromSeconds(10));

                string vlanData = telnet.ReceiveUntilMatch("$");

                while (vlanData.Contains("more", StringComparison.CurrentCultureIgnoreCase))
                {
                    vlanData = vlanData.Remove(vlanData.IndexOf("-- more", StringComparison.CurrentCultureIgnoreCase));
                    telnet.SendLine(" ");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    vlanData += telnet.ReceiveUntilMatch("$");
                }

                vlanData = vlanData.Replace(controlCharacters, string.Empty);

                // TODO: Remove multiple if else's
                if (routerFlag == RouterFlags.Managed)
                {
                    result = vlanData.Contains(Enum<RouterFlags>.Value(RouterFlags.Managed)) && !vlanData.Contains(Enum<RouterFlags>.Value(RouterFlags.Other));
                }
                else if (routerFlag == RouterFlags.Other)
                {
                    result = vlanData.Contains(Enum<RouterFlags>.Value(RouterFlags.Other)) && !vlanData.Contains(Enum<RouterFlags>.Value(RouterFlags.Managed));
                }
                else if (routerFlag == RouterFlags.Both)
                {
                    result = vlanData.Contains(Enum<RouterFlags>.Value(RouterFlags.Managed)) && vlanData.Contains(Enum<RouterFlags>.Value(RouterFlags.Other));
                }
                else
                {
                    result = !vlanData.Contains(Enum<RouterFlags>.Value(RouterFlags.Managed)) && !vlanData.Contains(Enum<RouterFlags>.Value(RouterFlags.Other));
                }

                flagDescription = RouterFlags.None == routerFlag ? "disabled M and O" : "set {0}".FormatWith(routerFlag);

                if (result)
                {
                    Logger.LogInfo("Successfully {0} flag on router VLAN : {1}".FormatWith(flagDescription, routerVlanId));
                }
                else
                {
                    Logger.LogInfo("Failed to {0} flag on router VLAN : {1}".FormatWith(flagDescription, routerVlanId));
                }

                return result;
            }
        }

        /// <summary>
        /// Disables both M and O flags for a particular virtual LAN.
        /// </summary>
        /// <param name="routerVlanId">The virtual LAN id.</param>
        /// <returns>True if the flags are disabled, else false.</returns>
        public bool DisableRouterFlag(int routerVlanId)
        {
            return SetRouterFlag(routerVlanId, RouterFlags.None);
        }

        /// <summary>
        /// Configure helper address for the specified virtual LAN.
        /// </summary>
        /// <param name="helperAddress">The helper address.</param>
        /// <returns>true if the helper address is set, else false.</returns>
        public bool ConfigureHelperAddress(int routerVlanId, IPAddress helperAddress)
        {
            Logger.LogInfo("Configuring helper-address:{0} on router VLAN : {1}".FormatWith(helperAddress, routerVlanId));

            string command = string.Empty;

            if (helperAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                command = "ip helper-address {0}".FormatWith(helperAddress);
            }
            else if (helperAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                command = "ipv6 helper-address unicast {0}".FormatWith(helperAddress);
            }

            string data = string.Empty;

            using (TelnetIpc telnet = new TelnetIpc(_adress.ToString(), 23))
            {
                telnet.Connect();
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(_userName);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(_password);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine("en");
                telnet.SendLine("conf t");
                telnet.SendLine("vlan {0}".FormatWith(routerVlanId));
                telnet.SendLine(command);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine("wr mem");
                Thread.Sleep(TimeSpan.FromSeconds(5));

                // Validation
                telnet.SendLine("sh run vlan {0}".FormatWith(routerVlanId));
                Thread.Sleep(TimeSpan.FromSeconds(10));

                data = telnet.ReceiveUntilMatch("$");

                while (data.Contains("more", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = data.Remove(data.IndexOf("-- more", StringComparison.CurrentCultureIgnoreCase));
                    telnet.SendLine(" ");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    data += telnet.ReceiveUntilMatch("$");
                }

                data = data.Replace(controlCharacters, string.Empty);
            }

            if (data.Contains(command))
            {
                Logger.LogInfo("Configured helper-address: {0} on VLAN: {1}".FormatWith(helperAddress, routerVlanId));
                return true;
            }
            else
            {
                Logger.LogInfo("Failed to configure helper-address: {0} on VLAN: {1}".FormatWith(helperAddress, routerVlanId));
                return false;
            }
        }

        /// <summary>
        /// Delete helper address for the specified virtual LAN.
        /// </summary>
        /// <param name="helperAddress">The helper address.</param>
        /// <returns>True if the helper address is deleted, else false.</returns>
        public bool DeleteHelperAddress(int routerVlanId, IPAddress helperAddress)
        {
            Logger.LogInfo("Deleting helper-address:{0} on router VLAN : {1}".FormatWith(helperAddress, routerVlanId));

            string command = string.Empty;

            if (helperAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                command = "no ip helper-address {0}".FormatWith(helperAddress);
            }
            else if (helperAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                command = "no ipv6 helper-address unicast {0}".FormatWith(helperAddress);
            }

            string data = string.Empty;

            using (TelnetIpc telnet = new TelnetIpc(_adress.ToString(), 23))
            {
                telnet.Connect();
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(_userName);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(_password);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine("en");
                telnet.SendLine("conf t");
                telnet.SendLine("vlan {0}".FormatWith(routerVlanId));
                telnet.SendLine(command);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine("wr mem");
                Thread.Sleep(TimeSpan.FromSeconds(5));

                // Validation
                telnet.SendLine("sh run vlan {0}".FormatWith(routerVlanId));
                Thread.Sleep(TimeSpan.FromSeconds(10));

                data = telnet.ReceiveUntilMatch("$");

                while (data.Contains("more", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = data.Remove(data.IndexOf("-- more", StringComparison.CurrentCultureIgnoreCase));
                    telnet.SendLine(" ");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    data += telnet.ReceiveUntilMatch("$");
                }

                data = data.Replace(controlCharacters, string.Empty);
            }

            if (data.Contains(command.Replace("no ", string.Empty).Trim()))
            {
                Logger.LogInfo("Failed to delete helper-address: {0} on VLAN: {1}".FormatWith(helperAddress, routerVlanId));
                return false;
            }
            else
            {
                Logger.LogInfo("Deleted helper-address: {0} on VLAN: {1}".FormatWith(helperAddress, routerVlanId));
                return true;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get the vlan details
        /// </summary>
        /// <param name="ipData">string obtained by executing the "show ip" command.</param>
        /// <param name="identifierData">string obtained by executing the "show vlans" command.</param>
        /// <returns>Collection of <see cref="RouterVirtualLAN"/></returns>
        private static Collection<RouterVirtualLAN> GetVlanDetails(string ipData, string identifierData)
        {
            Collection<RouterVirtualLAN> vlanList = new Collection<RouterVirtualLAN>();

            // Remove '\r' characters from the string
            ipData = ipData.Replace("\r", string.Empty);
            string[] lines = ipData.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

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

                RouterVirtualLAN vlanDetails;

                if (GetVLan(line, identifierData, out vlanDetails))
                {
                    //FillVirtualLanDetails(ref vlanDetails);
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
        /// <param name="vlanDetails"><see cref="RouterVirtualLAN"/></param>
        private static bool GetVLan(string line, string vlanIdentifierData, out RouterVirtualLAN vlanDetails)
        {
            Regex ipAddressRegex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");

            bool isIPAddress = false;
            vlanDetails = new RouterVirtualLAN();
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
                        vlanDetails.IPv4Address = IPAddress.Parse(localData[j]);
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
        /// Get the additional details of a VLan
        /// </summary>
        /// <param name="vlanData">The data obtained with the command "sh run vlan vlan number".</param>
        /// <param name="routerVlan"></param>
        /// <returns><see cref="RouterVirtualLAN"/></returns>
        private static void GetVlanDetails(string vlanData, ref RouterVirtualLAN routerVlan)
        {
            vlanData = vlanData.Replace(controlCharacters, string.Empty).Replace("/r", string.Empty).Trim();

            string[] vlanDetails = vlanData.Split(new char[] { '\n' });

            // Get all the IPv4 details
            string ipv4Details = Array.Find(vlanDetails, x => x.Contains("ip address", StringComparison.CurrentCultureIgnoreCase));

            if (null != ipv4Details)
            {
                routerVlan.IPv4Address = GetIPAddress(ipv4Details);
                routerVlan.SubnetMask = GetIPAddress(ipv4Details, true);
            }

            string ipv6StatusDetails = Array.Find(vlanDetails, x => x.Contains("ipv6 enable", StringComparison.CurrentCultureIgnoreCase));

            if (null != ipv6StatusDetails)
            {
                routerVlan.IPv6Status = true;
            }

            // Get all the IPv6 addresses
            string[] ipv6Addresses = Array.FindAll(vlanDetails, x => x.Contains("ipv6 address", StringComparison.CurrentCultureIgnoreCase) && !x.Contains("/") && !x.Contains("link-local", StringComparison.CurrentCultureIgnoreCase));

            Collection<IPAddress> IPv6AddressList = new Collection<IPAddress>();

            if (null != ipv6Addresses && ipv6Addresses.Count() > 0)
            {
                foreach (string ipv6Address in ipv6Addresses)
                {
                    IPv6AddressList.Add(GetIPAddress(ipv6Address));
                }
            }

            routerVlan.IPv6Details = GetIPv6Details(vlanDetails, IPv6AddressList);

            // Get the ipv4 helper address
            string ipv4helperAddressData = Array.Find(vlanDetails, x => x.Contains("ip helper-address", StringComparison.CurrentCultureIgnoreCase));

            if (null != ipv4helperAddressData)
            {
                routerVlan.IPv4HelperAddress = GetIPAddress(ipv4helperAddressData);
            }

            // Get IPv6 helper address
            string ipv6HelperAddress = Array.Find(vlanDetails, x => x.Contains("ipv6 helper-address", StringComparison.CurrentCultureIgnoreCase));

            if (null != ipv6HelperAddress)
            {
                routerVlan.IPv6HelperAddress = GetIPAddress(ipv6HelperAddress);
            }

            // Get Link Local address
            string LinkLocalAddress = Array.Find(vlanDetails, x => x.Contains("link-local", StringComparison.CurrentCultureIgnoreCase));

            if (null != LinkLocalAddress)
            {
                routerVlan.LinkLocalAddress = GetIPAddress(LinkLocalAddress);
            }
        }

        private static Collection<RouterIPv6Details> GetIPv6Details(string[] vlanDetails, Collection<IPAddress> ipv6AddressList)
        {
            Collection<RouterIPv6Details> v6AddressDetails = new Collection<RouterIPv6Details>();

            // Gets the details like preferred, valid life time, prefix length etc. for the virtual lan.
            string[] ipv6PrefixDetails = Array.FindAll(vlanDetails, x => x.Contains("prefix", StringComparison.CurrentCultureIgnoreCase));

            if (null != ipv6PrefixDetails && ipv6PrefixDetails.Count() > 0)
            {
                foreach (string details in ipv6PrefixDetails)
                {
                    RouterIPv6Details routerIpv6Details = new RouterIPv6Details();

                    string[] ipv6Details = details.Split(new char[] { ' ' });

                    // Fetch the prefix length and address prefix.
                    string prefixData = Array.Find(ipv6Details, x => x.Contains("/", StringComparison.CurrentCultureIgnoreCase));

                    if (null != prefixData)
                    {
                        routerIpv6Details.AddressPrefix = IPAddress.Parse(prefixData.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[0]);
                        routerIpv6Details.PrefixLength = int.Parse(prefixData.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[1]);

                        IPAddress ipv6Address = ipv6AddressList.Where(x => x.IsInSameSubnet(routerIpv6Details.AddressPrefix, routerIpv6Details.AddressPrefix)).FirstOrDefault();

                        // Add the IPv6 address
                        if (null != ipv6Address)
                        {
                            routerIpv6Details.IPv6Address = ipv6Address;
                        }
                        else
                        {
                            routerIpv6Details.IPv6Address = IPAddress.IPv6None;
                        }
                    }

                    int lifeTime = 0;

                    ipv6Details.Where(x => int.TryParse(x.ToString(), out lifeTime)).FirstOrDefault();

                    routerIpv6Details.PreferredLeaseTime = lifeTime;

                    ipv6Details.Where(x => int.TryParse(x.ToString(), out lifeTime)).LastOrDefault();

                    routerIpv6Details.ValidLeaseTime = lifeTime;

                    v6AddressDetails.Add(routerIpv6Details);
                }
            }

            // Add all the IPv6 addresses are available in the list.
            foreach (IPAddress address in ipv6AddressList)
            {
                if (!v6AddressDetails.Any(x => x.IPv6Address.Equals(address))) //v6AddressDetails.Where(x => !x.IPv6Address.Equals(address)).FirstOrDefault())
                {
                    v6AddressDetails.Add(new RouterIPv6Details()
                    {
                        IPv6Address = address,
                        AddressPrefix = GetPrefix(address),
                        PrefixLength = 64,
                        PreferredLeaseTime = PREFERRED_LIFE_TIME,
                        ValidLeaseTime = VALID_LIFE_TIME
                    });
                }
            }
            return v6AddressDetails;
        }

        /// <summary>
        /// Get the IP Address from a particular string
        /// </summary>
        /// <param name="ipAddressData">The IP Address Data</param>
        /// <param name="addressFamily"><see cref="AddressFamily"/></param>
        /// <param name="isSubnetMask">True if the address to be fetched is subnet mask, else false.</param>
        /// <returns>The IP Address from the given string.</returns>
        private static IPAddress GetIPAddress(string ipAddressData, bool isSubnetMask = false)
        {
            string[] data = ipAddressData.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            IPAddress address = null;

            if (isSubnetMask)
            {
                data.Where(x => IPAddress.TryParse(x.Replace('\r', ' ').Trim(), out address) == true).LastOrDefault();
            }
            else
            {
                data.Where(x => IPAddress.TryParse(x.Replace('\r', ' ').Trim(), out address) == true).FirstOrDefault();
            }

            return address;
        }

        /// <summary>
        /// Gets the available virtual Lans
        /// </summary>
        private void GetRouterVirtualLans()
        {
            string ipData = string.Empty;
            string identifierData = string.Empty;

            using (TelnetIpc telnet = new TelnetIpc(_adress.ToString(), 23))
            {
                telnet.Connect();
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(_userName);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                telnet.SendLine(_password);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                // Get the VLAN IP details
                telnet.SendLine("show ip");
                Thread.Sleep(TimeSpan.FromSeconds(10));
                string data = string.Empty;

                data = telnet.ReceiveUntilMatch("$");

                while (data.Contains("more", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = data.Remove(data.IndexOf("-- more", StringComparison.CurrentCultureIgnoreCase));
                    telnet.SendLine(" ");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    data += telnet.ReceiveUntilMatch("$");
                }

                ipData = data.Replace(controlCharacters, string.Empty); ;

                data = string.Empty;

                // Gets the Virtual LAN Id details
                telnet.SendLine("show vlan");
                Thread.Sleep(TimeSpan.FromSeconds(10));
                data = telnet.ReceiveUntilMatch("$");

                while (data.Contains("more", StringComparison.CurrentCultureIgnoreCase))
                {
                    data = data.Remove(data.IndexOf("-- more", StringComparison.CurrentCultureIgnoreCase));
                    telnet.SendLine(" ");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    data += telnet.ReceiveUntilMatch("$");
                }

                identifierData = data.Replace(controlCharacters, string.Empty);
            }

            Thread.Sleep(TimeSpan.FromSeconds(30));

            _routerVlans = GetVlanDetails(ipData, identifierData);
        }

        private static IPAddress GetPrefix(IPAddress address)
        {
            // TODO: Get an alternate logic to get prefix
            string[] data = address.ToString().Split(':');
            string prefix = "{0}:{1}:{2}:{3}::".FormatWith(data[0], data[1], data[2], data[3]);
            return IPAddress.Parse(prefix);
        }

        #endregion
    }
}
