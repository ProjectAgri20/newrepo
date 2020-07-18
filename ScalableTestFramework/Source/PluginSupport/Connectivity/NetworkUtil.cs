using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    /// <summary>
    /// Various methods for accessing network-related information.
    /// </summary>
    public static class NetworkUtil
    {
        private static readonly string _addCommandWithGateway = "interface ip add address name=\"{0}\" addr={1} mask={2} gateway={3} gwmetric={4}";
        private static readonly string _addCommand = "interface ip add address name=\"{0}\" addr={1} mask={2}";
        private const string _autoIPFormat = "169.254";
        private const string _defaultIPFormat = "192.0";

        /// <summary>
        /// Gets the name of the system domain.
        /// </summary>
        /// <value>The name of the system domain.</value>
        public static string SystemDomainName
        {
            get
            {
                string domain = Dns.GetHostEntry("127.0.0.1").HostName;
                return domain.Substring(domain.IndexOf('.') + 1);
            }
        }

        /// <summary>
        /// Gets the name of the system host.
        /// </summary>
        /// <value>The name of the system host.</value>
        public static string SystemHostName
        {
            get
            {
                return Dns.GetHostEntry("localhost").HostName;
            }
        }

        /// <summary>
        /// Gets the local address.
        /// </summary>
        /// <param name="subNetConstraint">Constrains the search to a specific SubNet address.</param>
        /// <param name="subNetMask">The sub net mask.</param>
        /// <returns></returns>
        public static IPAddress GetLocalAddress(string subNetConstraint = null, string subNetMask = "255.0.0.0")
        {
            IPAddress subNetConstraintIp = null;
            if (!string.IsNullOrEmpty(subNetConstraint))
            {
                subNetConstraintIp = IPAddress.Parse(subNetConstraint);
            }

            IPAddress subNetMaskIp = null;
            if (subNetConstraint != null)
            {
                subNetMaskIp = IPAddress.Parse(subNetMask);
            }

            var addresses = Dns.GetHostAddresses(Dns.GetHostName()).Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToList();

            if (addresses.Count == 1)
            {
                // If there is only one address found of type InterNetwork, then as long as it meets any
                // constraints and is not a loopback address, then return it.
                var address = addresses.First();
                if ((subNetConstraintIp == null || subNetMaskIp == null))
                {
                    if (!IPAddress.IsLoopback(address))
                    {
                        return address;
                    }
                }
                else if (address.IsInSameSubnet(subNetConstraintIp, subNetMaskIp))
                {
                    return address;
                }
            }
            else
            {
                // Otherwise, spin through all the addresses found of type InterNetwork and return
                // if one is found that meets the specifications.
                foreach (var address in addresses)
                {
                    if (subNetConstraintIp == null || subNetMaskIp == null)
                    {
                        // If is not loop back (127.0.0.1) and is not a private IP (169.254.x.x, 192.168.x.x))
                        if (!IPAddress.IsLoopback(address) && !IsNonRoutableIpAddress(address.ToString()))
                        {
                            return address;
                        }
                    }
                    else
                    {
                        // see if current ip matches subNet constraint
                        if (address.IsInSameSubnet(subNetConstraintIp, subNetMaskIp))
                        {
                            return address;
                        }
                    }
                }

                //we have not found a routable address in the list, we will host the service on the private network itself
                if (addresses.Any() && !IPAddress.IsLoopback(addresses.First()))
                {
                    return addresses.First();
                }
            }

            // Couldn't be found.
            return null;
        }

        /// <summary>
        /// IPs the address list.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static IPAddress[] IPAddressList(string address)
        {
            return Dns.GetHostEntry(address).AddressList;
        }

        /// <summary>
        /// Adds the address to controller.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="address">The address.</param>
        /// <param name="mask">The mask.</param>
        public static void AddAddressToController(string name, IPAddress address, IPAddress mask)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            if (mask == null)
            {
                throw new ArgumentNullException("mask");
            }

            AddAddressToController(name, address.ToString(), mask.ToString());
        }

        /// <summary>
        /// Adds the address to controller.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="address">The address.</param>
        /// <param name="mask">The mask.</param>
        /// <param name="gateway">The gateway.</param>
        /// <param name="metric">The metric.</param>
        public static void AddAddressToController(string name, string address, string mask, string gateway, int metric)
        {
            AddAddress(_addCommandWithGateway.FormatWith(name, address, mask, gateway, metric));
        }

        /// <summary>
        /// Adds the address to controller.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="address">The address.</param>
        /// <param name="mask">The mask.</param>
        public static void AddAddressToController(string name, string address, string mask)
        {
            AddAddress(_addCommand.FormatWith(name, address, mask));
        }

        /// <summary>
        /// Adds the address.
        /// </summary>
        /// <param name="command">The command.</param>
        private static void AddAddress(string command)
        {
            ProcessUtil.Execute("netsh.exe", command);
        }

        /// <summary>
        /// Enables the specified network connection.
        /// </summary>
        /// <param name="name">The name of the network connection.</param>
        public static void EnableNetworkConnection(string name)
        {
            ProcessUtil.Execute("netsh.exe", "interface set interface \"{0}\" ENABLE".FormatWith(name));
        }

        /// <summary>
        /// Disables the specified network connection.
        /// </summary>
        /// <param name="name">The name of the network connection.</param>
        public static void DisableNetworkConnection(string name)
        {
            ProcessUtil.Execute("netsh.exe", "interface set interface \"{0}\" DISABLE".FormatWith(name));
        }

        /// <summary>
        /// Sets the priority of the specified network connection.
        /// </summary>
        /// <param name="adapterName">The name of the network connection.</param>
        /// <param name="priority">The desired priority (a lower number is higher priority).</param>
        public static void SetNetworkPriority(string adapterName, int priority)
        {
            ProcessUtil.Execute("netsh.exe", "interface ipv4 set interface \"{0}\" metric={1}".FormatWith(adapterName, priority.ToString()));
        }

        /// <summary>
        /// Fetch next non-ping able IPAddress
        /// If we want to assign an IPAddress to the printer manually
        /// this method fetches the non-ping-able IPAddress from the subnet
        /// </summary>
        /// <param name="subnetMask">subnet mask of the printer</param>
        /// <param name="printerIP">IPAddress of the printer</param>
        /// <returns>Returns a new non-ping-able IPAddress</returns>
        public static IPAddress FetchNextIPAddress(IPAddress subnetMask, IPAddress printerIP)
        {
            IPAddress newIPAddress = IPAddress.None;

            newIPAddress = IPAddressUtil.NextAssignable(printerIP, subnetMask);
            while (PingUntilTimeout(newIPAddress, 0))
            {
                newIPAddress = IPAddressUtil.NextAssignable(newIPAddress, subnetMask);
            }

            return newIPAddress;
        }

        /// <summary>
        /// Determines whether the specified IP address string is non-routable.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsNonRoutableIpAddress(string ipAddress)
        {
            //if the ip address string is empty or null string, we consider it to be non-routable
            if (String.IsNullOrEmpty(ipAddress))
            {
                return true;
            }

            //if we cannot parse the Ipaddress, then we consider it non-routable
            IPAddress tempIpAddress = null;
            if (!IPAddress.TryParse(ipAddress, out tempIpAddress))
            {
                return true;
            }

            return IsNonRoutableIpAddress(tempIpAddress);
        }

        /// <summary>
        /// Determines whether the specified IP address is non-routable.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <remarks>A null or empty string passed as the ipAddress will return true. An invalid ipAddress will be returned as true. </remarks>
        /// <returns></returns>
        public static bool IsNonRoutableIpAddress(IPAddress ipAddress)
        {
            //Reference: http://en.wikipedia.org/wiki/Reserved_IP_addresses

            byte[] ipAddressBytes = ipAddress.GetAddressBytes();

            //if ipAddress is IPv4
            if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                if (IsIpAddressInRange(ipAddressBytes, "10.0.0.0/8")) //Class A Private network check
                {
                    return true;
                }
                else if (IsIpAddressInRange(ipAddressBytes, "172.16.0.0/12")) //Class B private network check
                {
                    return true;
                }
                else if (IsIpAddressInRange(ipAddressBytes, "192.168.0.0/16")) //Class C private network check
                {
                    return true;
                }
                else if (IsIpAddressInRange(ipAddressBytes, "127.0.0.0/8")) //Loopback
                {
                    return true;
                }
                else if (IsIpAddressInRange(ipAddressBytes, "0.0.0.0/8"))   //reserved for broadcast messages
                {
                    return true;
                }
                else if (IsIpAddressInRange(ipAddressBytes, "169.254.0.0/16"))
                {
                    return true;
                }

                //its routable if its ipv4 and meets none of the criteria
                return false;
            }
            //if ipAddress is IPv6
            else if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                //incomplete
                if (IsIpAddressInRange(ipAddressBytes, "::/128"))       //Unspecified address
                {
                    return true;
                }
                else if (IsIpAddressInRange(ipAddressBytes, "::1/128"))     //loopback address for localhost
                {
                    return true;
                }
                else if (IsIpAddressInRange(ipAddressBytes, "2001:db8::/32"))   //Addresses used in documentation
                {
                    return true;
                }

                return false;
            }
            else
            {
                //we default to non-routable if its not Ipv4 or Ipv6
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipAddressBytes"></param>
        /// <param name="reservedIpAddress"></param>
        /// <returns></returns>
        public static bool IsIpAddressInRange(byte[] ipAddressBytes, string reservedIpAddress)
        {
            if (String.IsNullOrEmpty(reservedIpAddress))
            {
                return false;
            }

            if (ipAddressBytes == null)
            {
                return false;
            }

            //Split the reserved ip address into a bitmask and ip address
            string[] ipAddressSplit = reservedIpAddress.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (ipAddressSplit.Length != 2)
            {
                return false;
            }

            string ipAddressRange = ipAddressSplit[0];

            IPAddress ipAddress = null;
            if (!IPAddress.TryParse(ipAddressRange, out ipAddress))
            {
                return false;
            }

            // Convert the IP address to bytes.
            byte[] ipBytes = ipAddress.GetAddressBytes();

            //parse the bits
            int bits = 0;
            if (!int.TryParse(ipAddressSplit[1], out bits))
            {
                bits = 0;
            }

            // BitConverter gives bytes in opposite order to GetAddressBytes().
            byte[] maskBytes = null;
            if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                uint mask = ~(uint.MaxValue >> bits);
                maskBytes = BitConverter.GetBytes(mask).Reverse().ToArray();
            }
            else if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                //128 places
                BitArray bitArray = new BitArray(128, false);

                //shift <bits> times to the right
                ShiftRight(bitArray, bits, true);

                //turn into byte array
                maskBytes = ConvertToByteArray(bitArray).Reverse().ToArray();
            }


            bool result = true;

            //Calculate
            for (int i = 0; i < ipBytes.Length; i++)
            {
                result &= (byte)(ipAddressBytes[i] & maskBytes[i]) == ipBytes[i];

            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitArray"></param>
        /// <param name="shiftN"></param>
        /// <param name="fillValue"></param>
        private static void ShiftRight(BitArray bitArray, int shiftN, bool fillValue)
        {
            for (int i = shiftN; i < bitArray.Count; i++)
            {
                bitArray[i - shiftN] = bitArray[i];
            }

            //fill the shifted bits as false
            for (int index = bitArray.Count - shiftN; index < bitArray.Count; index++)
            {
                bitArray[index] = fillValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitArray"></param>
        /// <returns></returns>
        private static byte[] ConvertToByteArray(BitArray bitArray)
        {
            // pack (in this case, using the first bool as the lsb - if you want
            // the first bool as the msb, reverse things ;-p)
            int bytes = (bitArray.Length + 7) / 8;
            byte[] arr2 = new byte[bytes];
            int bitIndex = 0;
            int byteIndex = 0;

            for (int i = 0; i < bitArray.Length; i++)
            {
                if (bitArray[i])
                {
                    arr2[byteIndex] |= (byte)(1 << bitIndex);
                }

                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }

            return arr2;
        }

        /// <summary>
        /// Determines whether this ip address is within the same subnet as given ip address.
        /// </summary>
        /// <param name="address2">The address2.</param>
        /// <param name="address">The address.</param>
        /// <param name="subnetMask">The subnet mask.</param>
        /// <returns></returns>
        public static bool IsInSameSubnet(this IPAddress address2, IPAddress address, IPAddress subnetMask)
        {
            IPAddress network1 = address.GetNetworkAddress(subnetMask);
            IPAddress network2 = address2.GetNetworkAddress(subnetMask);

            return network1.Equals(network2);
        }

        /// <summary>
        /// Gets the network address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="subnetMask">The subnet mask.</param>
        /// <returns></returns>
        private static IPAddress GetNetworkAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
            }
            return new IPAddress(broadcastAddress);
        }

        /// <summary>
        /// Pings the printer until it is success or time out happens
        /// </summary>
        /// <param name="address">IP Address of the printer</param>
        /// <param name="timeoutInMinutes">Number of minutes to wait</param>
        /// <returns>Returns true if the printer is pinging in the timeout else return false</returns>
        public static bool PingUntilTimeout(IPAddress address, int timeoutInMinutes)
        {
            return PingUntilTimeout(address, TimeSpan.FromMinutes(timeoutInMinutes));
        }

        /// <summary>
        /// Pings the printer until it is success or time out happens
        /// </summary>
        /// <param name="address">IP Address of the printer</param>
        /// <param name="timeout">Timeout for ping.</param>
        /// <returns>Returns true if the printer is pinging in the timeout else return false</returns>
        public static bool PingUntilTimeout(IPAddress address, TimeSpan timeout)
        {
            using (Ping ping = new Ping())
            {
                DateTime endTime = DateTime.Now + timeout;

                PingReply pingStatus = ping.Send(address);

                // ping the printer until it is success or time out occurs
                while ((pingStatus.Status != IPStatus.Success) && (DateTime.Now < endTime))
                {
                    pingStatus = ping.Send(address);
                }

                return (pingStatus.Status == IPStatus.Success ? true : false);
            }
        }

        /// <summary>
        /// Determines whether this ip address is within the same subnet as given ip address.
        /// </summary>
        /// <param name="address2">The address2.</param>
        /// <param name="address">The address.</param>
        /// <returns>true if both address2 and address are in the same subnet.</returns>
        public static bool IsInSameSubnet(this IPAddress address2, IPAddress address)
        {
            return address2.IsInSameSubnet(address, address.GetSubnetMask());
        }

        /// <summary>
        /// Determines whether the IP address is auto ip.
        /// </summary>
        /// <param name="address">The IP Address.</param>
        /// <returns>true if the address is an Auto IP, else false.</returns>
        public static bool IsAutoIP(this IPAddress address)
        {
            return address.ToString().StartsWith(_autoIPFormat);
        }

        /// <summary>
        /// Determines whether the IP address is default ip.
        /// </summary>
        /// <param name="address">The IP Address.</param>
        /// <returns>true if the address is an default IP, else false.</returns>
        public static bool IsDefaultIP(this IPAddress address)
        {
            return address.ToString().StartsWith(_defaultIPFormat);
        }

        /// <summary>
        /// Gets the subnet mask from an IP Address.
        /// IP4 addresses are categorized into 5 classes. For the  first three classes we have predefined subnet mask. 
        /// So if we can detect the class of an IP address , we can determine the corresponding subnet mask.
        /// Class   | Leading bits  | Start     | End               | CIDR suffix   | Default subnet mask
        /// ---------------------------------------------------------------------------------   
        /// Class A | 0             | 0.0.0.0   | 127.255.255.255   | /8            | 255.0.0.0    
        /// Class B | 10            | 128.0.0.0 | 191.255.255.255   | /16           | 255.255.0.0
        /// Class C | 110           | 192.0.0.0 | 223.255.255.255   | /24           | 255.255.255.0
        /// Class D | 1110          | 224.0.0.0 | 239.255.255.255   | /4            | not defined
        /// Class E | 1111          | 240.0.0.0 | 255.255.255.255   | /4            | not defined
        /// </summary>
        /// <param name="address">The IP Address.</param>
        /// <returns>The subnet mask.</returns>
        public static IPAddress GetSubnetMask(this IPAddress address)
        {
            // Gets first Octet from the IP Address. For eg: if the IP Address is 192.168.200.10, 192 will be returned.
            uint firstOctet = (uint)address.GetAddressBytes()[0];

            if (firstOctet >= 0 && firstOctet <= 127)
            {
                return IPAddress.Parse("255.0.0.0");
            }
            else if (firstOctet >= 128 && firstOctet <= 191)
            {
                return IPAddress.Parse("255.255.0.0");
            }
            else if (firstOctet >= 192 && firstOctet <= 223)
            {
                return IPAddress.Parse("255.255.255.0");
            }
            else
            {
                return IPAddress.Any;
            }
        }

        /// <summary>
        /// Gets all IP addresses configured in local machine
        /// </summary>
        /// <returns>All the local addresses.</returns>
        public static IPAddress[] GetLocalAddresses()
        {
            return IPAddressList(Dns.GetHostName());
        }

        /// <summary>
        /// Returns the MAC address with the fastest speed from the local machines collection of NICs.
        /// </summary>
        /// <returns>The MAC address</returns>
        public static string GetLocalMacAddress()
        {
            var nics = NetworkInterface.GetAllNetworkInterfaces().Where(n => n.OperationalStatus == OperationalStatus.Up && n.NetworkInterfaceType != NetworkInterfaceType.Loopback);
            var sortedNetworks = nics.OrderByDescending(n => n.Speed);

            return sortedNetworks.First().GetPhysicalAddress().ToString();
        }

        /// <summary>
        /// Renew Local Machine IP configuration
        /// This will ensure the machine acquires the configuration from DHCP server if available
        /// or it acquires Auto IP configuration if no server is available
        /// </summary>
        public static void RenewLocalMachineIP()
        {
            Logger.LogInfo("Renewing Local Machine IP Address.");
            ProcessUtil.Execute("cmd.exe", "/C ipconfig /release", TimeSpan.FromMinutes(3));
            ProcessUtil.Execute("cmd.exe", "/C ipconfig /renew", TimeSpan.FromMinutes(3));
           
        }

        /// <summary>
        /// Add static non pinging IP Address to ARP table
        /// </summary>
        /// <param name="address">Arp IP Address</param>
        /// <param name="deviceMacAddress">Device MAC Address</param>
        /// <param name="deletePreviousEntries">true to delete all entries under Arp table, false otherwise</param>		
        public static void AddArpEntry(IPAddress address, string deviceMacAddress, string networkInterface, bool deletePreviousEntries = false)
        {
            if (string.IsNullOrEmpty(deviceMacAddress))
            {
                Logger.LogInfo("Mac address is null.");
                return;
            }

            // Arp accepts Mac address in format: AB-CD-12-34-56-78
            // If Mac address value is: ABCD12345678, output should be: AB-CD-12-34-56-78

            if (deviceMacAddress.Contains(':'))
            {
                deviceMacAddress = deviceMacAddress.Replace(':', '-');
            }
            else if (!deviceMacAddress.Contains('-'))
            {
                deviceMacAddress = deviceMacAddress.Insert(2, "-").Insert(5, "-").Insert(8, "-").Insert(11, "-").Insert(14, "-");
            }

            if (deletePreviousEntries)
            {
                DeleteArpEntries();
            }

            //To add static ARP entry to specific interface ex: CTC-170
            Logger.LogInfo("Adding static ARP IP Address: {0} with MAC address: {1} to ARP table.".FormatWith(address, deviceMacAddress));
            ProcessUtil.Execute("cmd.exe", "/C netsh interface ipv4 add neighbors {0} {1} {2}".FormatWith(networkInterface, address, deviceMacAddress));
        }

        /// <summary>
        /// Delete all existing ARP entries from ARP table
        /// </summary>
        public static void DeleteArpEntries()
        {
            Logger.LogInfo("Deleting existing ARP entries from ARP table.");
            ProcessUtil.Execute("cmd.exe", "/C arp -d");
        }

        /// <summary>
        /// Execute a command from run line command
        /// </summary>
        /// <param name="command">command to be executed</param>
        /// <returns>Returns command output</returns>
        public static string ExecuteCommand(string command)
        {
            Process process = null;
            string result = string.Empty;

            Logger.LogDebug("Executing Command: {0}".FormatWith(command));

            try
            {
                process = new Process();
                string app = string.Format("{0}\\cmd.exe", Environment.SystemDirectory);
                string arguments = string.Format("/C {0}", command);

                ProcessStartInfo ProcessStartInfo = new ProcessStartInfo(app, arguments);

                ProcessStartInfo.CreateNoWindow = true;
                ProcessStartInfo.UseShellExecute = false;
                ProcessStartInfo.RedirectStandardOutput = true;
                ProcessStartInfo.RedirectStandardInput = true;
                ProcessStartInfo.RedirectStandardError = true;

                process.StartInfo = ProcessStartInfo;
                process.Start();

                if (!process.HasExited)
                {
                    process.StandardInput.WriteLine('y');
                }
                result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            finally
            {
                // close process and do cleanup 
                process.Close();
                //process.Dispose();
                //process = null;
            }

            return result;
        }

        /// <summary>
        /// Execute a command from run line command
        /// </summary>
        /// <param name="command">command to be executed</param>
        /// <returns>True if the command is executed successfully, else returns false</returns>
        public static bool ExecuteCommandAndValidate(string command)
        {
            List<string> results = new List<string>() { "Command completed successfully",
                                                             "ALREADY_EXISTS", "The specified option does not exist",
                                                             "already been started", "started successfully", "running",
                                                             "stopped successfully","not started", "Ok", "No rules"};

            string output = ExecuteCommand(command);

            foreach (string result in results)
            {
                if (output.Contains(result, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
