using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity.Dhcp
{
    /// <summary>
    /// Contains IPv6 Lease Time
    /// </summary>
    public enum IPv6LeaseTime
    {
        /// <summary>
        /// Preferred Life Time for v6 scope
        /// </summary>
        [EnumValue("preferredlifetime")]
        PreferredLifeTime,
        /// <summary>
        /// Valid Life Time for v6 scope
        /// </summary>
        [EnumValue("validlifetime")]
        ValidLifeTime
    }

    /// <summary>
    /// Reservation types in DHCP Server
    /// Values represents netsh command value
    /// </summary>
    [Flags]
    public enum ReservationType
    {
        /// <summary>
        /// DHCP
        /// </summary>
        [EnumValue("0")]
        Dhcp = 1,
        /// <summary>
        /// BOOTP
        /// </summary>
        [EnumValue("1")]
        Bootp = 2,
        /// <summary>
        /// Both DHCP and BOOTP
        /// </summary>
        [EnumValue("2")]
        Both = Dhcp | Bootp
    }

    /// <summary>
    /// Data type formats
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// String type
        /// </summary>
        STRING,
        /// <summary>
        /// Dword type
        /// </summary>
        DWORD,
        /// <summary>
        /// IPAddress type
        /// </summary>
        IPADDRESS,
        /// <summary>
        /// IPv6 Address type
        /// </summary>
        IPV6ADDRESS,
        /// <summary>
        /// Byte type
        /// </summary>
        BYTE
    }

    /// <summary>
    /// Structure to represent Address lease format consisting IP Address and associated Mac Address
    /// </summary>
    public struct AddressLease
    {
        public IPAddress Address
        {
            get;
            set;
        }

        public string MacAddress
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Contains DHCP related operations like create / delete reservation and getting and setting server configurations.
    /// </summary>
    public static class DhcpApplication
    {
        #region Constants

        private const int OPTION_LEASETIME = 51;
        private const int OPTION_HOSTNAME = 12;
        private const int OPTION_DOMAINNAME = 15;
        private const int OPTION_DNSSERVER = 6;
        private const int OPTION_WINSSERVER = 44;
        private const int OPTION_DNSV6 = 23;
        private const int OPTION_DOMAINSEARCHLIST = 24;
        private const int OPTION_WPADSERVER = 252;

        #endregion

        #region Public Methods

        /// <summary>
        /// Setting DHCP lease time
        /// </summary>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="leaseTime">Lease Time</param>
        /// <returns>Returns true if it the lease time is set, else returns false.</returns>
        public static bool SetDhcpLeaseTime(string serverIP, string scope, long leaseTime)
        {
            Logger.LogInfo("Changing DHCP Lease Time to {0}".FormatWith(leaseTime));

            return NetworkUtil.ExecuteCommandAndValidate("netsh dhcp server {0} scope {1} set optionvalue {2} DWORD \"{3}\" \n".FormatWith(serverIP, scope, OPTION_LEASETIME, leaseTime));
        }

        /// <summary>
        /// Create a Reservation for device
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="macAddress">Mac address of device</param>
        /// <param name="reservationIP">IP Address to reserve</param>
        /// <param name="reservationType"><see cref=" ReservationType"/></param>
        public static bool CreateReservation(string serverIP, string scope, string reservationIP, string macAddress, ReservationType reservationType)
        {
            macAddress = macAddress.Replace(":", string.Empty);

            Logger.LogInfo("Creating {0} reservation with IP Address {1} and MAC Address {2}".FormatWith(reservationType, reservationIP, macAddress));

            string command = "netsh dhcp server {0} scope {1} add reservedip {2} {3} {3} \"Created By Connectivity Automation\" {4} \n".
                              FormatWith(serverIP, scope, reservationIP, macAddress, reservationType);

            return NetworkUtil.ExecuteCommandAndValidate(command);
        }

        /// <summary>
        /// Create a Reservation for device
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="macAddress">Mac address of device</param>		
        /// <param name="reservationIP">IP Address to reserve</param>		
        public static bool DeleteReservation(string serverIP, string scope, string reservationIP, string macAddress)
        {
            macAddress = macAddress.Replace(":", string.Empty);

            Logger.LogInfo("Deleting reservation for IP Address {0} and MAC Address {1}".FormatWith(reservationIP, macAddress));

            return NetworkUtil.ExecuteCommandAndValidate("netsh dhcp server {0} scope {1} delete reservedip {2} {3} \n".FormatWith(serverIP, scope, reservationIP, macAddress));
        }

        /// <summary>
        /// Set hostname for a scope
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="hostname">Host name to set</param>		
        public static bool SetHostName(string serverIP, string scope, string hostname)
        {
            Logger.LogInfo("Configuring DHCP server scope {0} Host name to {1}".FormatWith(scope, hostname));

            return NetworkUtil.ExecuteCommandAndValidate("netsh dhcp server {0} scope {1} set optionvalue {2} STRING {3} \n".FormatWith(serverIP, scope, 12, hostname));
        }

        /// <summary>
        /// Delete hostname of specified scope
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>		
        public static bool DeleteHostName(string serverIP, string scope)
        {
            return DeleteScopeOption(serverIP, scope, OPTION_HOSTNAME);
        }

        /// <summary>
        /// Set domainname for a scope
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="domainName">Domain name to set</param>		
        public static bool SetDomainName(string serverIP, string scope, string domainName)
        {
            Logger.LogInfo("Configuring DHCP server scope {0} with option DOMAIN NAME to {1}".FormatWith(scope, domainName));
            return NetworkUtil.ExecuteCommandAndValidate("netsh dhcp server {0} scope {1} set optionvalue {2} STRING {3} \n".FormatWith(serverIP, scope, OPTION_DOMAINNAME, domainName));
        }
        /// <summary>
        /// Set WPAD server for a scope
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="wpadServer">WPAD Server to set</param>		
        public static bool SetWPADServer(string serverIP, string scope, string wpadServer)
        {
            Logger.LogInfo("Configuring WPAD server scope {0} with option WPAD Server to {1}".FormatWith(scope, wpadServer));
            return NetworkUtil.ExecuteCommandAndValidate("netsh dhcp server {0} scope {1} set optionvalue {2} STRING {3} \n".FormatWith(serverIP, scope, OPTION_WPADSERVER, wpadServer));
        }

        /// <summary>
        /// Set DNS server for a scope
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>	
        /// <param name="dnsServers">The DNS servers to be added. Note: Multiple values can be added here.</param>
        public static bool SetDnsServer(string serverIP, string scope, params string[] dnsServers)
        {
            Logger.LogInfo("Configuring DHCP server scope {0} with option DNS SERVER to {1}".FormatWith(scope, serverIP));

            return NetworkUtil.ExecuteCommandAndValidate("netsh dhcp server {0} scope {1} set optionvalue {2} {3} DhcpFullForce {4} \n".FormatWith(serverIP, scope, OPTION_DNSSERVER, DataType.IPADDRESS, string.Join(" ", dnsServers)));
        }

        /// <summary>
        /// Set WINS server for a scope
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>	
        /// <param name="winsAddress">The Wins servers to be added. Note: Multiple values can be added here.</param>
        public static bool SetWinsServer(string serverIP, string scope, params string[] winsAddress)
        {
            Logger.LogInfo("Configuring DHCP server scope {0} with option WINS SERVER to {1}".FormatWith(scope, serverIP));
            return NetworkUtil.ExecuteCommandAndValidate("netsh dhcp server {0} scope {1} set optionvalue {2} {3} DhcpFullForce {4} \n".FormatWith(serverIP, scope, OPTION_WINSSERVER, DataType.IPADDRESS, string.Join(" ", winsAddress)));
        }

        /// <summary>
        /// Remove dns server for a scope
        /// </summary>		
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        public static bool DeleteDnsServer(string serverIP, string scope)
        {
            return DeleteScopeOption(serverIP, scope, OPTION_DNSSERVER);
        }

        /// <summary>
        /// Remove dnsv6 server for a scope
        /// </summary>		
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        public static bool DeleteDnsv6Server(string serverIP, string scope)
        {
            return DeleteScopeOption(serverIP, scope, OPTION_DNSV6);
        }

        /// <summary>
        /// Get DHCP ScopeIP
        /// </summary>
        /// <param name="serverIP">DHCP Server IP</param>
        /// <returns>DHCP Scope IP address</returns>
        /// <remarks>
        /// output of Netsh Command to retrieve scopeip
        /// C:\Users\Administrator>netsh dhcp server 192.168.70.254 show scope
        ///==============================================================================
        /// Scope Address  - Subnet Mask    - State        - Scope Name          -  Commen
        ///==============================================================================
        /// 192.168.70.0   - 255.255.255.0  -Active        -TPS_v4_70            -
        /// 192.168.71.0   - 255.255.255.0  -Active        -TPS_v4_71            -
        /// Total No. of Scopes = 2
        ///Command completed successfully.
        ///</remarks>
        public static string GetDhcpScopeIP(string serverIP)
        {
            Logger.LogInfo("Getting DHCP ScopeIP from the DHCP Server:{0}".FormatWith(serverIP));
            string command = "netsh dhcp server {0} show scope".FormatWith(serverIP);
            Logger.LogDebug("Netsh command used to fetch scope ip of the server:{0}".FormatWith(command));

            string result = NetworkUtil.ExecuteCommand(command);
            string[] resultsplit = result.Split('\n');
            bool status = false;

            foreach (var scope in resultsplit)
            {
                if (scope.StartsWith(" 192"))
                {
                    var res = scope.Split('-');
                    var scopeipSplit = res[0].Trim().Split('.');
                    var serveripSplit = serverIP.Trim().Split('.');
                    for (int state = 0; state < 3; state++)
                    {
                        if (scopeipSplit[state].Equals(serveripSplit[state]))
                        {
                            status = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                    if (status)
                    {
                        result = res[0].Trim();
                    }
                }
            }
            Logger.LogInfo("Scope IP retrieved from DHCP Server:{0}".FormatWith(result));
            return result;
        }

        /// <summary>
        /// Get DHCP Server DNS Suffix of the Printer
        /// </summary>
        /// <returns>DNS Suffix</returns>
        /// <param name="serverIP">DHCP Server IP</param>
        /// <param name="scope">scopeIP of the DHCP Server</param>
        /// <remarks>
        /// output of Netsh Command to retrieve dns suffix[119-option ID for DNS Suffix]
        /// By default for foo.com the byte array[102,111,111,46,99,111,109]
        ///C:\Users\Administrator>netsh dhcp server 192.168.70.254 scope 192.168.70.0 show optionvalue
        ///Changed the current scope context to 192.168.70.0 scope.
        ///Options for Scope 192.168.70.0:
        ///        DHCP Standard Options :
        ///        General Option Values:
        ///        OptionId : 15
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = STRING
        ///                Option Element Value = hssl.com
        ///        OptionId : 6
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = IPADDRESS
        ///                Option Element Value = 192.168.70.254
        ///        OptionId : 51
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = DWORD
        ///                Option Element Value = 691200
        ///        OptionId : 119
        ///        Option Value:
        ///                Number of Option Elements = 7
        ///                Option Element Type = BYTE
        ///                Option Element Value = 102
        ///                Option Element Value = 111
        ///                Option Element Value = 111
        ///                Option Element Value = 46
        ///                Option Element Value = 99
        ///                Option Element Value = 111
        ///                Option Element Value = 109
        ///        OptionId : 3
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = IPADDRESS
        ///                Option Element Value = 192.168.70.1
        ///        OptionId : 12
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = STRING
        ///                Option Element Value = Default_HostName
        ///        For user class [Default BOOTP Class]:
        ///        OptionId : 51
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = DWORD
        ///                Option Element Value = 2592000
        ///Command completed successfully.
        ///</remarks>
        public static string GetDnsSuffix(string serverIP, string scope)
        {
            //// TODO: The implementation is for single value of DNS suffix, multiple values need to be handled.
            //string command, result;

            //string dhcpDNSSuffix = string.Empty;

            //StringBuilder sb = new StringBuilder();
            //Logger.LogInfo("Getting DNS Suffix configured on the DHCP Server");

            //command = "netsh dhcp server {0} scope {1} show optionvalue".FormatWith(serverIP, scope);

            //result = NetworkUtil.ExecuteCommand(command);
            //string[] resultsplit = result.Split(':');

            ///* Data fetched for 119 option:
            //	OptionId : 119 
            //	Option Value: 
            //	Number of Option Elements = 10
            //	Option Element Type = BYTE
            //	Option Element Value = 9
            //	Option Element Value = 3
            //	Option Element Value = 102
            //	Option Element Value = 111
            //	Option Element Value = 111
            //	Option Element Value = 3
            //	Option Element Value = 99
            //	Option Element Value = 111
            //	Option Element Value = 109
            //	Option Element Value = 0			  
            // * */
            //// Value for above fetched data: foo.com


            //for (var optionid = 0; optionid < resultsplit.Length; optionid++)
            //{
            //	//dns suffix option id:119 is static across all servers
            //	if (resultsplit[optionid].Trim().StartsWith("119"))
            //	{
            //		{
            //			// Split to get values on RHS of '='
            //			string[] value = resultsplit[optionid + 1].Split('=');
            //			// Ignore first 4 values of split data as these are extra details provided
            //			for (int i = 5; i < value.Length; i++)
            //			{
            //				// Exclude unnecessary data
            //				dhcpDNSSuffix = value[i].Trim().Split('\r')[0];
            //				// Value 3 represents extension (.) which can't be directly converted. Hence changing the byte value to represent . (byte value for . is 46)
            //				if (dhcpDNSSuffix.Equals("3"))
            //				{
            //					dhcpDNSSuffix = "46";
            //				}
            //				sb.Append(Convert.ToChar(int.Parse(dhcpDNSSuffix)));
            //			}
            //		}
            //	}
            //}
            //// There may be '\0'(the nul characters) in the string, trim these characters to get proper string.
            //return sb.ToString().Trim(new char[] { (char)0 });

            // TODO: It is assumed that the data type is string for the DNS suffix as there are some problems for setting the value if the data type is BYTE.
            // Once the issue is sorted out the data type needs to be changed back to BYTE, the above code is required.

            string dnsSuffix = GetConfiguredParameterValue(serverIP, scope, 119);
            // While getting the value of DNS suffix , initial 2 characters are discarded as the devices takes only the rest of the characters
            string[] dnsSuffices = dnsSuffix.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Remove(0, 2)).ToArray();

            return string.Join("|", dnsSuffices);
        }

        /// <summary>
        /// Set DNS Suffix for the specified scope
        /// </summary>
        /// <param name="serverIP">IP Address of the DHCP server.</param>
        /// <param name="scope">scopeIP of the DHCP Server.</param>
        /// <param name="dnsSuffices">DNS suffices to be set.</param>
        /// <returns>True if the DNS Suffix is set, else false.</returns>
        /// <remarks>
        /// Format:
        /// netsh dhcp Server \\192.168.205.254 Scope 192.168.205.0 set optionvalue 119 BYTE "0" "9" "3" "102" "111" "111" "3" "99" "111" "109" 
        /// </remarks>
        public static bool SetDnsSuffix(string serverIP, string scope, params string[] dnsSuffices)
        {
            // Format: <Total Length> <Length of item> <value> <nul character> for BYTE values
            // For string values, Initial 2 characters will be discarded
            string suffixFormat = "12{0}";

            dnsSuffices = dnsSuffices.Select(x => suffixFormat.FormatWith(x)).ToArray();

            //dnsSuffices = dnsSuffices.Select(x => suffixFormat.FormatWith(x.Length + 2, string.Join(" ", x.Select(y => Convert.ToInt32(y))))).ToArray();
            return SetScopeOptions(serverIP, scope, 119, DataType.STRING, values: string.Join(" ", dnsSuffices));
        }

        /// <summary>
        /// Get Server configured parameters like hostname,domain name, primary DNS server, router ip
        /// specific to scope
        /// </summary>
        /// <param name="serverIP">DHCP Server IP</param>
        /// <param name="scope">scopeIP of the DHCP Server</param>
        /// <param name="optionID">Option Id to get value</param>
        /// <param name="isv6Scope">true if option ID is for v6 scope, false for v4 scope</param>
        /// <returns>based on option id returns the specific value
        /// ex: Option id: 12[Returns hostname],6[primary DNS IP],3[Router IP address]</returns>
        /// <example>
        /// Usage: GetConfiguredParameterValue(serverIP,scopeIP,optionID)
        /// <code>
        /// GetConfiguredParameterValue("192.168.100.254", "192.168.100.0", 6)
        /// </code>
        /// </example>
        /// <remarks>
        /// output of Netsh Command to retrieve server configured parameters
        ///C:\Users\Administrator>netsh dhcp server 192.168.70.254 scope 192.168.70.0 show optionvalue
        ///Changed the current scope context to 192.168.70.0 scope.
        ///Options for Scope 192.168.70.0:
        ///        DHCP Standard Options :
        ///        General Option Values:
        ///        OptionId : 15
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = STRING
        ///                Option Element Value = hssl.com
        ///       OptionId : 6
        ///        Option Value:
        ///               Number of Option Elements = 1
        ///                Option Element Type = IPADDRESS
        ///                Option Element Value = 192.168.70.254
        ///        OptionId : 51
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = DWORD
        ///                Option Element Value = 691200
        ///        OptionId : 119
        ///        Option Value:
        ///                Number of Option Elements = 10
        ///                Option Element Type = BYTE
        ///                Option Element Value = 9
        ///                Option Element Value = 3
        ///                Option Element Value = 102
        ///                Option Element Value = 111
        ///                Option Element Value = 111
        ///                Option Element Value = 3
        ///                Option Element Value = 99
        ///                Option Element Value = 111
        ///                Option Element Value = 109
        ///                Option Element Value = 0
        ///        OptionId : 3
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = IPADDRESS
        ///                Option Element Value = 192.168.70.1
        ///        OptionId : 12
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = STRING
        ///                Option Element Value = Default_HostName
        ///        For user class [Default BOOTP Class]:
        ///        OptionId : 51
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///               Option Element Type = DWORD
        ///                Option Element Value = 2592000
        ///Command completed successfully.
        /// Note : Returns values separated by '|' if multiple values are configured on the server for the given options.
        ///        Eg : If primary and secondary DNS Servers are present in the Server, then the values will be returned as "192.168.*.*|192.168.*.*"
        ///</remarks>
        public static string GetConfiguredParameterValue(string serverIP, string scope, int optionID, bool isv6Scope = false)
        {
            string configuredValues = string.Empty;

            // For IPv6 scope, we need to specify keyword 'v6' before scope keyword
            if (isv6Scope)
            {
                serverIP = string.Format("{0} {1}", serverIP, "v6");
            }

            try
            {
                Logger.LogInfo("Getting DHCP server configured value for optionID: {0}".FormatWith(optionID));

                string command = "netsh dhcp server {0} scope {1} show optionvalue".FormatWith(serverIP, scope);

                string result = NetworkUtil.ExecuteCommand(command);
                string[] resultLines = result.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                // Position the index at the starting of the OptionId information.
                int position = Array.FindIndex(resultLines, x => x.RemoveWhiteSpace().Contains("OptionId", StringComparison.CurrentCultureIgnoreCase)
                                && x.Split(':')[1].Trim().Equals(optionID.ToString()));

                // If the option is not found, return empty value.
                if (position == -1)
                {
                    Logger.LogInfo("Option : {0} is not configured on the server".FormatWith(optionID));
                    return string.Empty;
                }

                // Skip the element immediately after the 'OptionId :' so as to fetch the number of element values.
                position = position + 2;

                // Get the number of element values
                string numberOfElementValues = resultLines[position].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)[1];
                int elementValues = 0;
                int.TryParse(numberOfElementValues, NumberStyles.Integer, CultureInfo.CurrentCulture, out elementValues);

                // Get the element values skipping the immediate value which is 'Option Element Type' value.
                for (int valuePosition = position + 1, counter = 1; counter <= elementValues; counter++)
                {
                    // Split only if there is a valid value
                    configuredValues += string.IsNullOrEmpty(resultLines[valuePosition + counter]) ? string.Empty : resultLines[valuePosition + counter].Split('=')[1].Trim();

                    if (counter == elementValues)
                    {
                        break;
                    }

                    configuredValues = string.Concat(configuredValues, "|");
                }

                return configuredValues;
            }
            catch (IndexOutOfRangeException indexOutOfRangeException)
            {
                Logger.LogInfo("An error occurred while fetching the option value for the option Id : {0}.{1}".FormatWith(optionID, indexOutOfRangeException.Message));
                return string.Empty;
            }
            catch (Exception generalException)
            {
                Logger.LogInfo("An error occurred while fetching the option value for the option Id : {0}.{1}".FormatWith(optionID, generalException.Message));
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the Domain Name
        /// </summary>
        /// <param name="serverIP">Server IP</param>
        /// <param name="scope">Scope</param>
        /// <returns>Returns Domain Name</returns>
        public static string GetDomainName(string serverIP, string scope)
        {
            try
            {
                string domainName = GetConfiguredParameterValue(serverIP, scope, 15);

                // If multiple values are there, first value is the primary DNS Server.
                domainName = domainName.Contains('|') ? domainName.Split('|')[0] : domainName;
                Logger.LogInfo("Domain Name : {0}".FormatWith(domainName));
                return domainName;
            }
            catch (Exception exception)
            {
                Logger.LogInfo("Failed to get domain name.");
                Logger.LogDebug("Exception details : {0}".FormatWith(exception.Message));
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the Host Name
        /// </summary>
        /// <param name="serverIP">Server IP</param>
        /// <param name="scope">Scope</param>
        /// <returns>Returns Domain Name</returns>
        public static string GetHostName(string serverIP, string scope)
        {
            try
            {
                string hostName = GetConfiguredParameterValue(serverIP, scope, 12);

                // If multiple values are there, first value is the primary Host Name.
                hostName = hostName.Contains('|') ? hostName.Split('|')[0] : hostName;
                Logger.LogInfo("Host Name : {0}".FormatWith(hostName));
                return hostName;
            }
            catch (Exception exception)
            {
                Logger.LogInfo("Failed to get host name.");
                Logger.LogDebug("Exception details : {0}".FormatWith(exception.Message));
                return string.Empty;
            }
        }

        /// <summary>
        /// Starts DHCP Server
        /// </summary>
        /// <returns>true if DHCP server is started, false otherwise</returns>
        public static bool StartDhcpServer()
        {
            if (IsDhcpServiceRunning())
            {
                return true;
            }
            else
            {
                return NetworkUtil.ExecuteCommandAndValidate("net start dhcpserver");
            }
        }

        /// <summary>
        /// Stop DHCP Server
        /// </summary>
        /// <returns>true if DHCP server is stopped, false otherwise</returns>
        public static bool StopDhcpServer()
        {
            if (!IsDhcpServiceRunning())
            {
                return true;
            }
            else
            {
                return NetworkUtil.ExecuteCommandAndValidate("net stop dhcpserver");
            }
        }

        /// <summary>
        /// Get v6 Scope IP if available
        /// </summary>
        /// <returns>All v6 Scopes available if found, string.Empty if not</returns>
        /// <param name="serverIP">Server IP Address</param>
        /// <remarks>
        /// Output Format:
        /// ========================================================================================================
        /// Scope Address                        - State         - Scope Name          -  Comment       - Preference
        ///========================================================================================================
        /// 2000:200:200:200::                    -Active        -200v6                -              -64
        /// 2000:200:200:201::                    -Active        -200v6                -              -64
        /// 2000:200:200:202::                    -Active        -200v6                -              -64
        /// Total No. of Scopes = 3 
        ///Command completed successfully.
        ///</remarks>
        public static string GetIPv6Scope(string serverIP)
        {
            char[] dataSeparator = { '\r', '\n' };
            Collection<string> scopeIP = new Collection<string>();

            // Command to get v6 scope list on DHCP server
            string command = "netsh dhcp server {0} v6 show scope".FormatWith(serverIP);
            string data = NetworkUtil.ExecuteCommand(command);

            // If command exists abruptly
            if (!data.Contains("Command completed successfully", StringComparison.CurrentCultureIgnoreCase))
            {
                return string.Empty;
            }

            // Remove unwanted data
            string[] splitData = data.Trim().Replace("=", string.Empty).Split(dataSeparator, StringSplitOptions.RemoveEmptyEntries);

            // string[] 'splitData' after performing split

            // Positive Case :

            // Scope Address                        - State         - Scope Name          -  Comment       - Preference			
            // 2000:200:200:200::                    -Active        -200v6                -              -64
            // 2000:200:200:201::                    -Active        -200v6                -              -64
            // 2000:200:200:202::                    -Active        -200v6                -              -64
            // Total No. of Scopes = 3 
            // Command completed successfully.

            // Negative Case :			

            // Total No. of Scopes = 0 
            // Command completed successfully.

            // When any v6 scope is found, splitData length will be more than 2; if not found we return empty string
            if (splitData.Length > 2)
            {
                // Performing split again with '-'
                dataSeparator = new char[] { '-' };

                for (int i = 1; i < splitData.Length - 2; i++)
                {
                    // With splitData, performing string match satisfying condition: 1.Contains ':' 2. Starts with number
                    scopeIP.Add(splitData[i].RemoveWhiteSpace().Split(dataSeparator, StringSplitOptions.RemoveEmptyEntries).
                        FirstOrDefault(x => x.Contains(':') && Regex.IsMatch(x, @"^\d+")));
                }
            }

            // If no v6 scope is available, return empty or all available scope(s) found
            if (scopeIP.Count > 0)
            {
                return string.Join("|", scopeIP);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets IPv6 address of the local machine
        /// </summary>
        /// <returns>IPv6 address</returns>
        public static string GetIPv6Address()
        {
            foreach (IPAddress address in NetworkUtil.GetLocalAddresses())
            {
                if (address.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    if (address.ToString().EndsWith("254"))
                    {
                        return address.ToString();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Get Preferred Life Time for v6 Scope
        /// </summary>		
        /// <returns>time in seconds</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <remarks>
        /// Output format:
        /// Changed the current scope context to 2000:200:200:20 scope.
        /// Preferred Lifetime setting : 691200
        /// Command completed successfully.
        /// </remarks>
        public static int GetPreferredLifetime(string serverIP, string scope)
        {
            return GetLeasetime(serverIP, scope, IPv6LeaseTime.PreferredLifeTime);
        }

        /// <summary>
        /// Set Preferred Life Time for v6 Scope
        /// </summary>
        /// <returns>true if command executed successful, false otherwise</returns>
        /// <param name="leaseTime">Lease time to set in seconds</param>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        public static bool SetPreferredLifetime(string serverIP, string scope, int leaseTime)
        {
            return SetLeasetime(serverIP, scope, IPv6LeaseTime.PreferredLifeTime, leaseTime);
        }

        /// <summary>
        /// Get Valid Life Time for v6 Scope
        /// </summary>		
        /// <returns>time in seconds</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <remarks>
        /// Output format:
        /// Changed the current scope context to 2000:200:200:20 scope.
        /// Valid Lifetime setting : 777600
        /// Command completed successfully.
        /// </remarks>
        public static int GetValidLifetime(string serverIP, string scope)
        {
            return GetLeasetime(serverIP, scope, IPv6LeaseTime.ValidLifeTime);
        }

        /// <summary>
        /// Set Valid Life Time for v6 Scope
        /// </summary>		
        /// <returns>true if command executed successful, false otherwise</returns>
        /// <param name="leaseTime">Lease time to set in seconds</param>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        public static bool SetValidLifetime(string serverIP, string scope, int leaseTime)
        {
            return SetLeasetime(serverIP, scope, IPv6LeaseTime.ValidLifeTime, leaseTime);
        }

        /// <summary>
        /// Gets the server name.
        /// </summary>
        /// <returns>Name of the Server</returns>
        public static string GetServerName()
        {
            return Environment.MachineName;
        }

        /// <summary>
        /// Returns primary DNS Server IP Address for the specified scope.
        /// </summary>
        /// <param name="dhcpServer">The DHCP Server IP Address.</param>
        /// <param name="scopeIPAddress">The scope IP Address.</param>
        /// <returns>The primary DNS Server IP Address.</returns>
        public static string GetPrimaryDnsServer(string dhcpServer, string scopeIPAddress)
        {
            Logger.LogInfo("Getting Primary DNS Server from DHCP Server : {0} with scope IP Address : {1}.".FormatWith(dhcpServer, scopeIPAddress));

            try
            {
                string primaryDnsServer = GetConfiguredParameterValue(dhcpServer, scopeIPAddress, 6);

                // If multiple values are there, first value is the primary DNS Server.
                primaryDnsServer = primaryDnsServer.Contains('|') ? primaryDnsServer.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim() : primaryDnsServer;

                Logger.LogInfo("Primary DNS Server : {0}".FormatWith(primaryDnsServer));
                return primaryDnsServer;
            }
            catch (Exception serviceException)
            {
                Logger.LogInfo("Exception details : {0}".FormatWith(serviceException.Message));
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns secondary DNS Server IP Address for the specified scope.
        /// </summary>
        /// <param name="dhcpServer">The DHCP Server IP Address.</param>
        /// <param name="scopeIPAddress">The scope IP Address.</param>
        /// <returns>The secondary DNS Server IP Address.</returns>
        public static string GetSecondaryDnsServer(string dhcpServer, string scopeIPAddress)
        {
            Logger.LogInfo("Getting secondary DNS Server from DHCP Server : {0} with scope IP Address : {1}.".FormatWith(dhcpServer, scopeIPAddress));

            try
            {
                string secondaryDnsServer = GetConfiguredParameterValue(dhcpServer, scopeIPAddress, 6);

                // If multiple values are there, first value is the primary DNS Server.
                secondaryDnsServer = secondaryDnsServer.Contains('|') ? secondaryDnsServer.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[1].Trim() : secondaryDnsServer;

                Logger.LogInfo("Secondary DNS Server : {0}".FormatWith(secondaryDnsServer));
                return secondaryDnsServer;
            } // TODO : Catch a more general exception
            catch (Exception serviceException)
            {
                Logger.LogInfo("Exception details : {0}".FormatWith(serviceException.Message));
                return string.Empty;
            }
        }

        /// <summary>
        /// Set BootP host name
        /// </summary>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="hostName">Host name to be set</param>
        /// <returns>true if set successfully, false otherwise</returns>
        public static bool SetBootPHostName(string serverIP, string scope, string hostName)
        {
            Logger.LogInfo("Configuring DHCP server scope {0} with HOST NAME to {1}".FormatWith(scope, hostName));
            return NetworkUtil.ExecuteCommandAndValidate("netsh dhcp server {0} scope {1} set optionvalue {2} {3} user=\"Default BOOTP Class\" {4}".FormatWith(serverIP, scope, OPTION_HOSTNAME, DataType.STRING, hostName));
        }

        /// <summary>
        /// Gets the Lease time
        /// </summary>
        /// <param name="serverIP">Server IP</param>
        /// <param name="scope">Scope</param>
        /// <returns>Returns Lease time in seconds</returns>
        public static long GetLeasetime(string serverIP, string scope)
        {
            try
            {
                string leasetime = GetConfiguredParameterValue(serverIP, scope, 51);

                Logger.LogInfo("Lease time : {0}".FormatWith(leasetime));
                return long.Parse(leasetime);
            }
            catch (Exception exception)
            {
                Logger.LogInfo("Failed to get lease time.");
                Logger.LogDebug("Exception details : {0}".FormatWith(exception.Message));
                return 0;
            }
        }

        /// <summary>
        /// Check whether DHCP Service is running on DHCP server
        /// </summary>
        /// <returns>true if DHCP Service is running, false otherwise</returns>
        public static bool IsDhcpServiceRunning()
        {
            return NetworkUtil.ExecuteCommandAndValidate("sc query DHCPServer");
        }

        /// <summary>
        /// Gets the Router Address
        /// </summary>
        /// <param name="serverIP">Server IP</param>
        /// <param name="scope">Scope</param>
        /// <returns>Returns Router Address</returns>
        public static string GetRouterAddress(string serverIP, string scope)
        {
            try
            {
                string routerAddress = GetConfiguredParameterValue(serverIP, scope, 3);

                Logger.LogInfo("Router Address : {0}".FormatWith(routerAddress));
                return routerAddress;
            }
            catch (Exception exception)
            {
                Logger.LogInfo("Failed to get router address.");
                Logger.LogDebug("Exception details : {0}".FormatWith(exception.Message));
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns primary WINS Server IP Address for the specified scope.
        /// </summary>
        /// <param name="dhcpServer">The DHCP Server IP Address.</param>
        /// <param name="scope">The scope IP Address.</param>
        /// <returns>The primary WINS Server IP Address.</returns>
        public static string GetPrimaryWinsServer(string dhcpServer, string scope)
        {
            Logger.LogInfo("Getting Primary WINS Server from DHCP Server : {0} with scope IP Address : {1}.".FormatWith(dhcpServer, scope));

            try
            {
                string primaryWinsServer = GetConfiguredParameterValue(dhcpServer, scope, 44);

                // If multiple values are there, first value is the primary WINS Server.
                primaryWinsServer = primaryWinsServer.Contains('|') ? primaryWinsServer.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim() : primaryWinsServer;

                Logger.LogInfo("Primary WINS Server : {0}".FormatWith(primaryWinsServer));
                return primaryWinsServer;
            }
            catch (Exception serviceException)
            {
                Logger.LogInfo("Exception details : {0}".FormatWith(serviceException.Message));
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns secondary WINS Server IP Address for the specified scope.
        /// Note: If secondary WINS Server IP Address is not available, primary will be returned as default
        /// </summary>
        /// <param name="dhcpServer">The DHCP Server IP Address.</param>
        /// <param name="scope">The scope IP Address.</param>
        /// <returns>The secondary WINS Server IP Address.</returns>
        public static string GetSecondaryWinsServer(string dhcpServer, string scope)
        {
            Logger.LogInfo("Getting secondary WINS Server from DHCP Server : {0} with scope IP Address : {1}.".FormatWith(dhcpServer, scope));

            try
            {
                string secondaryWinsServer = GetConfiguredParameterValue(dhcpServer, scope, 44);

                secondaryWinsServer = secondaryWinsServer.Contains('|') ? secondaryWinsServer.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[1].Trim() : secondaryWinsServer;

                Logger.LogInfo("Secondary WINS Server : {0}".FormatWith(secondaryWinsServer));
                return secondaryWinsServer;
            }
            catch (Exception serviceException)
            {
                Logger.LogInfo("Exception details : {0}".FormatWith(serviceException.Message));
                return string.Empty;
            }
        }

        /// <summary>
        /// Delete Domain name option on specified scope 
        /// </summary>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="scope">Scope IP Address</param>
        public static bool DeleteDomainName(string serverIP, string scope)
        {
            return DeleteScopeOption(serverIP, scope, OPTION_DOMAINNAME);
        }

        /// <summary>
        /// Delete WPAD server option on specified scope 
        /// </summary>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="scope">Scope IP Address</param>
        public static bool DeleteWPADServer(string serverIP, string scope)
        {
            return DeleteScopeOption(serverIP, scope, OPTION_WPADSERVER);
        }

        /// <summary>
        /// Delete Domain name option on specified scope 
        /// </summary>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="scope">Scope IP Address</param>
        public static bool DeleteWinsServer(string serverIP, string scope)
        {
            return DeleteScopeOption(serverIP, scope, OPTION_WINSSERVER);
        }

        /// <summary>
        /// Get Primary DNS v6 Address
        /// </summary>
        /// <param name="serverIP">Server Address</param>
        /// <param name="scope">v6 Scope Address</param>
        /// <returns>Primary Dns v6 address</returns>
        public static string GetPrimaryDnsv6Server(string serverIP, string scope)
        {
            string primaryDnsv6Server = GetIpv6ConfiguredParameterValue(serverIP, scope, OPTION_DNSV6);

            primaryDnsv6Server = primaryDnsv6Server.Contains('|') ? primaryDnsv6Server.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim() : primaryDnsv6Server;

            return primaryDnsv6Server;
        }

        /// <summary>
        /// Get Secondary DNS v6 Address
        /// </summary>
        /// <param name="serverIP">Server Address</param>
        /// <param name="scope">v6 Scope Address</param>
        /// <returns>Secondary Dns v6 address</returns>
        public static string GetSecondaryDnsv6Server(string serverIP, string scope)
        {
            string secondaryDnsv6Server = GetIpv6ConfiguredParameterValue(serverIP, scope, OPTION_DNSV6);

            secondaryDnsv6Server = secondaryDnsv6Server.Contains('|') ? secondaryDnsv6Server.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[1].Trim() : string.Empty;

            return secondaryDnsv6Server;
        }

        /// <summary>
        /// Get Domain Search List
        /// </summary>
        /// <param name="serverIP">Server Address</param>
        /// <param name="scope">v6 Scope Address</param>
        /// <returns>Domain Search List</returns>
        public static string GetDomainSearchList(string serverIP, string scope)
        {
            return GetIpv6ConfiguredParameterValue(serverIP, scope, OPTION_DOMAINSEARCHLIST);
        }

        /// <summary>
        /// Create a new scope
        /// </summary>
        /// <param name="serverIP">Server Address</param>
        /// <param name="scope">Scope Address</param>
        /// <param name="scopeName">Scope name</param>
        /// <param name="startIpRange">Start range of IP Address for the scope</param>
        /// <param name="endIpRange">End range for the scope</param>
        /// <returns>true if the scope is created, else false</returns>
        public static bool CreateScope(string serverIP, string scope, string scopeName, string startIpRange, string endIpRange)
        {
            NetworkUtil.ExecuteCommand("netsh dhcp server {0} add scope {1} {2} \"{3}\" \"\"".FormatWith(serverIP, scope, IPAddress.Parse(serverIP).GetSubnetMask().ToString(), scopeName));
            return NetworkUtil.ExecuteCommandAndValidate("netsh dhcp server {0} scope {1} add iprange {2} {3}".FormatWith(serverIP, scope, startIpRange, endIpRange));
        }

        /// <summary>
        /// Create a new scope
        /// </summary>
        /// <param name="serverIP">Server Address</param>
        /// <param name="scope">Scope Address</param>
        /// <returns>true if scope deleted, false otherwise</returns>
        public static bool DeleteScope(string serverIP, string scope)
        {
            return NetworkUtil.ExecuteCommandAndValidate("netsh dhcp server {0} delete scope {1} dhcpfullforce".FormatWith(serverIP, scope));
        }

        /// <summary>
        /// Change the server type to DHCP, BOOTP or BOTH.
        /// Note : Deletes all the reservations and change the server type.
        ///	If we want to retain the reservation mention the exceptionAddress and exceptionMacAddress parameters.
        /// </summary>
        /// <param name="serverIP">The IP Address of the DHCP Server</param>
        /// <param name="scope">The scope IP Address</param>
        /// <param name="serverType"><see cref="DhcpServerType"/></param>
        /// <returns>true if the server type is successfully changed, else false</returns>
        public static bool ChangeServerType(string serverIP, string scope, ReservationType serverType, string exceptionAddress = null, string exceptionMacAddress = null)
        {
            if (AddRange(serverIP, scope, serverType))
            {
                return true;
            }
            else if (serverType == ReservationType.Bootp || serverType == ReservationType.Dhcp)
            {
                // Delete all the leases and try to change the server type again.
                DeleteAllLeases(serverIP, scope);
                AddRange(serverIP, scope, serverType);

                return CreateReservation(serverIP, scope, exceptionAddress, exceptionMacAddress, ReservationType.Both);
            }

            return true;
        }

        /// <summary>
        /// Delete BootP Hostname
        /// </summary>
        /// <param name="serverIP">The IP Address of the DHCP Server</param>
        /// <param name="scope">The scope IP Address</param>
        /// <returns>true if deleted successfully, false otherwise</returns>
        public static bool DeleteBootPHostname(string serverIP, string scope)
        {
            return NetworkUtil.ExecuteCommandAndValidate("netsh dhcp server {0} scope {1} delete optionvalue {2} user=\"Default BOOTP Class\"".FormatWith(serverIP, scope, OPTION_HOSTNAME));
        }

        /// <summary>
        /// Set Domain Search List
        /// </summary>
        /// <param name="serverIP">The IP Address of the DHCP Server</param>
        /// <param name="scope">The v6 scope IP Address</param>		
        /// <param name="domainName">Domain name to be set</param>
        /// <returns>true if set, false otherwise</returns>
        public static bool SetDomainSearchList(string serverIP, string scope, string domainName)
        {
            return SetIPv6ScopeOptions(serverIP, scope, OPTION_DOMAINSEARCHLIST, DataType.STRING, domainName);
        }

        /// <summary>
        /// Set DNS V6Primary Server IP 
        /// </summary>
        /// <param name="serverIP">The IP Address of the DHCP Server</param>
        /// <param name="scope">The v6 scope IP Address</param>		
        /// <param name="dnsServer">Domain name to be set</param>
        /// <returns>true if set, false otherwise</returns>
        public static bool SetDnsv6ServerIP(string serverIP, string scope, string dnsServer)
        {
            return SetIPv6ScopeOptions(serverIP, scope, OPTION_DNSV6, DataType.IPV6ADDRESS, dnsServer);
        }

        /// <summary>
        /// Delete Domain Search List option in v6 scope
        /// </summary>
        /// <param name="serverIP">The IP Address of the DHCP Server</param>
        /// <param name="scope">The v6 scope IP Address</param>		
        /// <returns>true if deleted successfully, false otherwise</returns>
        public static bool DeleteDomainSearchList(string serverIP, string scope)
        {
            return Deletev6ScopeOption(serverIP, scope, OPTION_DOMAINSEARCHLIST);
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Get IPv6 scope Lease time
        /// </summary>
        /// <returns>Lease time in seconds</returns>
        /// <param name="serverIP">DHCP Server IP Address</param>
        /// <param name="scopeIPAddress">IPv6 Scope IP</param>
        /// <param name="leaseType"><see cref=" IPv6LeaseTime"/></param>
        /// <remarks>
        /// Output format for valid lifetime :
        /// Changed the current scope context to 2000:200:200:20 scope.
        /// Valid Lifetime setting : 777600
        /// Command completed successfully.
        /// </remarks>
        private static int GetLeasetime(string serverIP, string scopeIPAddress, IPv6LeaseTime leaseType)
        {
            // Command to get v6 scope list on DHCP server
            string command = "netsh dhcp server {0} v6 scope {1} show {2}".FormatWith(serverIP, scopeIPAddress, Enum<IPv6LeaseTime>.Value(leaseType));
            string data = NetworkUtil.ExecuteCommand(command);

            // If command exists abruptly
            if (!data.Contains("Command completed successfully", StringComparison.CurrentCultureIgnoreCase))
            {
                return -1;
            }

            // Splitting data line wise
            char[] dataSeparator = { '\r', '\n' };
            string[] splitData = data.Split(dataSeparator, StringSplitOptions.RemoveEmptyEntries);

            // Since the lease time is in second line, we split this with ':'
            string[] leaseDetails = splitData[1].Split(':');

            if (leaseDetails.Length > 1)
            {
                return int.Parse(leaseDetails[1].Trim());
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Set IPv6 scope Lease time
        /// </summary>
        /// <returns>true if command executed successful, false otherwise</returns>
        /// <param name="serverIP">DHCP Server IP Address</param>
        /// <param name="scopeIPAddress">IPv6 Scope IP Address</param>
        /// <param name="leaseType"><see cref=" IPv6LeaseTime"/></param>
        /// <param name="leaseTime">Lease time to be set</param>
        private static bool SetLeasetime(string serverIP, string scopeIPAddress, IPv6LeaseTime leaseType, int leaseTime)
        {
            string command = "netsh dhcp server {0} v6 scope {1} set {2} {3}".FormatWith(serverIP, scopeIPAddress, Enum<IPv6LeaseTime>.Value(leaseType), leaseTime);
            return NetworkUtil.ExecuteCommandAndValidate(command);
        }

        /// <summary>
        /// Delete option for specified scope
        /// </summary>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="optionID">Option ID to be deleted</param>
        /// <returns>true if successful, false otherwise</returns>
        private static bool DeleteScopeOption(string serverIP, string scope, int optionID)
        {
            string optionName = string.Empty;

            if (15 == optionID)
            {
                optionName = "Domain name";
            }
            else if (44 == optionID)
            {
                optionName = "Wins server";
            }
            else if (6 == optionID)
            {
                optionName = "Dns server";
            }
            else if (12 == optionID)
            {
                optionName = "Host name";
            }
            else if (252 == optionID)
            {
                optionName = "WPAD server";
            }

            Logger.LogInfo("Deleting {0} option for the scope {1}".FormatWith(optionName, scope));
            return NetworkUtil.ExecuteCommandAndValidate("netsh dhcp server {0} scope {1} delete optionvalue {2} \n".FormatWith(serverIP, scope, optionID));
        }

        /// <summary>
        /// Get IPv6 scope configured option value
        /// </summary>
        /// <returns>Option Value</returns>
        private static string GetIpv6ConfiguredParameterValue(string serverIP, string scope, int optionID)
        {
            return GetConfiguredParameterValue(serverIP, scope, optionID, true);
        }

        /// <summary>
        /// Set the values for specified scope options
        /// </summary>
        /// <param name="serverAddress">IP Address of the server.</param>
        /// <param name="scopeAddress">Scope Address.</param>
        /// <param name="optionId">OPtion Id to be set.</param>
        /// <param name="dataType"><see cref="DataType"/></param>
        /// <param name="value">Value to be set for the specified option Id.
        /// Multiple values can be given if the option supports.</param>
        /// <param name="isv6Scope"></param>
        /// <returns>True if the option is set, else false.</returns>
        private static bool SetScopeOptions(string serverAddress, string scopeAddress, int optionId, DataType dataType, bool isv6Scope = false, params string[] values)
        {
            // For IPv6 scope, we need to specify keyword 'v6' before scope keyword
            if (isv6Scope)
            {
                serverAddress = string.Format("{0} {1}", serverAddress, "v6");
            }

            string command = "netsh dhcp server {0} scope {1} set optionvalue {2} {3} DhcpFullForce {4}".FormatWith(serverAddress, scopeAddress, optionId, dataType, string.Join(" ", values));

            if (NetworkUtil.ExecuteCommandAndValidate(command))
            {
                Logger.LogInfo("Successfully set option {0} for scope {1} to {2}".FormatWith(optionId, scopeAddress, string.Join(" ", values)));
                return true;
            }
            else
            {
                Logger.LogInfo("Failed to set option {0} for scope {1} to {2}".FormatWith(optionId, scopeAddress, string.Join(" ", values)));
                return false;
            }
        }

        /// <summary>
        /// Set the values for specified scope options
        /// </summary>
        /// <param name="serverAddress">IP Address of the server.</param>
        /// <param name="scopeAddress">v6 Scope Address.</param>
        /// <param name="optionId">Option Id to be set.</param>
        /// <param name="dataType"><see cref="DataType"/></param>
        /// <param name="value">Value to be set for the specified option Id.
        /// Multiple values can be given if the option supports.</param>
        /// <returns>true if the option is set, else false.</returns>
        private static bool SetIPv6ScopeOptions(string serverAddress, string scopeAddress, int optionId, DataType dataType, params string[] values)
        {
            return SetScopeOptions(serverAddress, scopeAddress, optionId, dataType, true, string.Join(" ", values));
        }

        /// <summary>
        /// Add Range with 
        /// </summary>
        /// <param name="serverIP"></param>
        /// <param name="scope"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        private static bool AddRange(string serverIP, string scope, ReservationType serverType)
        {
            string scopeRange = GetScopeRange(serverIP, scope);

            if (string.IsNullOrEmpty(scopeRange))
            {
                return false;
            }

            return NetworkUtil.ExecuteCommandAndValidate("netsh dhcp Server {0} Scope {1} Add iprange {2} {3}".FormatWith(serverIP, scope, scopeRange, serverType));
        }

        /// <summary>
        /// Gets the IP range of the specified scope.
        /// </summary>
        /// <param name="serverIP">The DHCP server IP Address.</param>
        /// <param name="scope">The scope IP Address.</param>
        /// <returns>the scope range separated by space.</returns>
        private static string GetScopeRange(string serverIP, string scope)
        {
            /****** Data format obtained by executing the command show iprange*******
			 * Changed the current scope context to 192.168.202.0 scope.
			 * =============================================================
			 * Start Address   -    End Address    -     Address Type    
			 * =============================================================
			 * 192.168.202.2   -   192.168.202.200 -  DHCP ONLY
			 * 
			 * No of IP Ranges : 1 in the Scope : 192.168.202.0.
			 * 
			 * Command completed successfully.
			 */

            string data = NetworkUtil.ExecuteCommand("netsh dhcp server {0} scope {1} show iprange".FormatWith(serverIP, scope));

            // Remove unwanted data
            string[] splitData = data.Trim().Replace("=", string.Empty).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // If a valid sope is present the number of elements in the array will be 5.
            if (splitData.Length < 5)
            {
                return string.Empty;
            }

            // Ignoring the first 2 lines as it is heading information
            if (!string.IsNullOrEmpty(splitData[2]) && splitData[2].Contains("-"))
            {
                string[] iprange = splitData[2].RemoveWhiteSpace().Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                return string.Join(" ", iprange[0], iprange[1]);
            }
            else
            {
                return string.Empty;
            }

        }

        /// <summary>
        /// Delete all the leases in the specified scope.
        /// </summary>
        /// <param name="dhcpServer">The DHCP server IP address.</param>
        /// <param name="scopeAddress">The scope address.></param>
        /// <returns>true if all the leases are deleted.</returns>
        private static bool DeleteAllLeases(string dhcpServer, string scopeAddress)
        {
            Collection<AddressLease> adressLeases = GetLeases(dhcpServer, scopeAddress);

            if (null == adressLeases || adressLeases.Count == 0)
            {
                Logger.LogInfo("There are no address leases available.");
                return true;
            }

            foreach (AddressLease lease in adressLeases)
            {
                DeleteLease(dhcpServer, scopeAddress, lease);
            }

            return true;
        }

        /// <summary>
        /// Get the address leases in the specified scope.
        /// </summary>
        /// <param name="dhcpServer">The DHCP server IP address.</param>
        /// <param name="scopeAddress">The scope address.</param>
        /// <returns>The <see cref="AddressLease"/> in the current scope.</returns>
        private static Collection<AddressLease> GetLeases(string dhcpServer, string scopeAddress)
        {
            Collection<AddressLease> addressLeases = new Collection<AddressLease>();

            string data = NetworkUtil.ExecuteCommand("netsh dhcp server {0} scope {1} show clients".FormatWith(dhcpServer, scopeAddress));

            // Remove unwanted data
            string[] splitData = data.Trim().Replace("=", string.Empty).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // If a valid sope is present the number of elements in the array will be 5.
            if (splitData.Length < 5)
            {
                return null;
            }

            AddressLease lease;
            IPAddress address;

            // Ignoring the first 3 lines as it is heading information
            for (int i = 3; i < splitData.Length; i++)
            {
                lease = new AddressLease();

                string[] leaseInfo = splitData[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (IPAddress.TryParse(leaseInfo[0], out address))
                {
                    lease.Address = address;
                    lease.MacAddress = leaseInfo[3].Replace('-', ' ').RemoveWhiteSpace();
                }
                addressLeases.Add(lease);
            }

            return addressLeases;
        }

        /// <summary>
        /// Delete the specified lease in scope.
        /// </summary>
        /// <param name="dhcpServer">The DHCP server IP address.</param>
        /// <param name="scopeAddress">The scope address.</param>
        /// <param name="lease"><see cref="AddressLease"/></param>
        /// <returns>true if the address lease is successfully deleted, else false.</returns>
        private static bool DeleteLease(string dhcpServer, string scopeAddress, AddressLease lease)
        {
            DeleteReservation(dhcpServer, scopeAddress, lease.Address.ToString(), lease.MacAddress);
            Thread.Sleep(TimeSpan.FromSeconds(5));

            return NetworkUtil.ExecuteCommandAndValidate("netsh dhcp server {0} scope {1} delete lease {2}".FormatWith(dhcpServer, scopeAddress, lease.Address));
        }

        /// <summary>
        /// Delete IPv6 scope option
        /// </summary>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="scope">v6 scope IP Address</param>
        /// <param name="optionId">Option Id to be deleted</param>
        /// <returns>true if deleted successfully, false otherwise</returns>
        private static bool Deletev6ScopeOption(string serverIP, string scope, int optionId)
        {
            string optionName = string.Empty;

            if (24 == optionId)
            {
                optionName = "Domain search list";
            }

            Logger.LogInfo("Deleting {0} option for the scope {1}".FormatWith(optionName, scope));
            return NetworkUtil.ExecuteCommandAndValidate("netsh dhcp server {0} v6 scope {1} delete optionvalue {2} \n".FormatWith(serverIP, scope, optionId));
        }

        #endregion
    }
}
