using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Properties;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    #region Enumerator

    /// <summary>
    /// Linux Service Type
    /// </summary>
    public enum LinuxServiceType
    {
        /// <summary>
        /// The Dhcp Service
        /// </summary>
        DHCP,
        /// <summary>
        /// The Bootp service
        /// </summary>
        BOOTP,
        /// <summary>
        /// The TFTP service
        /// </summary>
        TFTP,
        /// <summary>
        /// The RARP service
        /// </summary>
        RARP,
        /// <summary>
        /// The Dibbler service
        /// </summary>
        DHCPV6
    }

    #endregion

    /// <summary>
    /// This class consists of static methods for Linux related operations. (tested on Ubuntu 14)
    /// For configuring any Linux file, a local static file is edited and replaced on Linux server.
    /// For copying files between Windows and Linux, pscp.exe is used.
    /// DHCPv6 server configuration is been customized and used instead of third party software: Dibbler.
    /// </summary>
    public static class LinuxUtils
    {
        #region Constants

        private const string USER_NAME = "administrator";
        private const string PASSWORD = "1iso*help";

        public const string LINUX_DHCP_PATH = @"/etc/dhcp/dhcpd.conf";
        private const string LINUX_DHCPV6_PATH = @"/etc/dhcp/dhcpd6.conf";
        private const string LINUX_BOOTP_PATH = @"/etc/bootptab";
        private const string LINUX_TFTP_PATH = @"/tftpboot/tftp.cfg";
        private const string LINUX_ETHERS_PATH = @"/etc/ethers";
        private const string LINUX_HOSTS_PATH = @"/etc/hosts";
        private const string DEFAULT_FILES_PATH = @"/etc/Original_Files/";
        private const string LINUX_DHCP_LEASES_PATH = @"/var/lib/dhcp/dhcpd.leases";
        private const string LINUX_DHCPV6_LEASES_PATH = @"/var/lib/dhcp/dhcpd6.leases";

#pragma warning disable 1591

        public const string TEMP_FILE_PATH = @"/etc/temp.txt";
        public const string LINUX_IPSEC_PATH = @"/etc/ipsec/ipsec.conf";
        public const string LINUX_PSK_PATH = @"/etc/ipsec/psk.txt";
        public const string LINUX_RACOON_PATH = @"/etc/ipsec/racoon.conf";
        public const string BACKUP_FOLDER_PATH = @"/etc/Backup_ConfigFiles/";
        public const string DHCP_FILE = "dhcpd.conf";
        public const string DHCPV6_FILE = "dhcpd6.conf";
        public const string BOOTP_FILE = "bootptab";
        public const string TFTP_FILE = "tftp.cfg";
        public const string ETHERS_FILE = "ethers";
        public const string HOSTS_FILE = "hosts";
        public const string DHCP_LEASES_FILE = "dhcpd.leases";
        public const string DHCPV6_LEASES_FILE = "dhcpd6.leases";
        public const string PSK_FILE = "psk.txt";
        public const string IPSEC_FILE = "ipsec.conf";
        public const string RACOON_FILE = "racoon.conf";
        public const string CREATE_RULE_FILE = "rule.sh";
        public const string DELETE_RULE_FILE = "clear.sh";
        public const string IPSEC_AH_FILE = "ipsec_ah.conf";
        public const string IPSEC_ESP_AH_FILE = "ipsec_esp_ah.conf";

        public const string KEY_IPADDRESS = "<IPADDRESS>";
        public const string KEY_MAC_ADDRESS = "<MACADDRESS>";
        public const string KEY_SUBNET_MASK = "<SUBNETMASK>";
        public const string KEY_LOG_SERVER = "<LOG_SERVER>";
        public const string KEY_HOST_NAME = "<HOSTNAME>";
        public const string KEY_DOMAIN_NAME = "<DOMAINNAME>";
        public const string KEY_PRIMARY_WINS = "<PRIMARY_WINSSERVER>";
        public const string KEY_SECONDARY_WINS = "<SECONDARY_WINSSERVER>";
        public const string KEY_PRIMARY_DNS = "<PRIMARY_DNSSERVER>";
        public const string KEY_SECONDARY_DNS = "<SECONDARY_DNSSERVER>";
        public const string KEY_FROM_RANGE = "<FROM_RANGE>";
        public const string KEY_TO_RANGE = "<TO_RANGE>";
        public const string KEY_DEFAULT_LEASE_TIME = "<DEFAULT_LEASE_TIME>";
        public const string KEY_GATEWAY = "<GATEWAY>";
        public const string KEY_INTERFACE_NAME = "<INTERFACE_NAME>";
        public const string KEY_PREFERED_LIFETIME = "<PREFERED_LIFETIME>";
        public const string KEY_VALID_LIFETIME = "<VALID_LIFETIME>";
        public const string KEY_IPV6_SCOPE = "<IPV6_SCOPE>";
        public const string KEY_FQDN = "<FQDN>";
        public const string KEY_PREFIX = "<PREFIX>";
        public const string KEY_UUID = "<UUID>";
        public const string KEY_VENDOR_OPTION = "<VENDOR_OPTION>";
        public const string KEY_VENDOR_STATUS = "<VENDOR_STATUS>";

        // IPsec keys below
        public const string KEY_PRINTERADDRESS = "<PRINTER_ADDRESS>";
        public const string KEY_CLIENTADDRESS = "<CLIENT_ADDRESS>";
        public const string KEY_PSK = "<PSK_VALUE>";
        public const string KEY_LINUXCLIENTADDRESS = "<LINUX_CLIENT_ADDRESS>";
        public const string KEY_LINUXPRINTERADDRESS = "<LINUX_PRINTER_ADDRESS>";
        public const string KEY_ENCAPSULATION = "<ENCAPSULATION>";
        public const string KEY_ENCAPSULATIONMODE = "<ENCAPSULATION_MODE>";
        public const string KEY_TUNNELOUTADDRESS = "<TUNNEL_OUT_ADDRESS>";
        public const string KEY_TUNNELINADDRESS = "<TUNNEL_IN_ADDRESS>";
        public const string KEY_ESPOUT = "<ESP_OUT>";
        public const string KEY_ESPIN = "<ESP_IN>";
        public const string KEY_AHOUT = "<AH_OUT>";
        public const string KEY_AHIN = "<AH_IN>";
        public const string KEY_ENCRYPTIONIN = "<ENCRYPTION_IN>";
        public const string KEY_ENCRYPTIONOUT = "<ENCRYPTION_OUT>";
        public const string KEY_AUTHENTICATIONIN = "<AUTHENTICATION_IN>";
        public const string KEY_AUTHENTICATIONOUT = "<AUTHENTICATION_OUT>";
        public const string KEY_PRINTERENCRYPTION = "<PRINTER_ENCRYPTION>";
        public const string KEY_PRINTERAUTHENTICATION = "<PRINTER_AUTHENTICATION>";
        public const string KEY_CLIENTENCRYPTION = "<CLIENT_ENCRYPTION>";
        public const string KEY_CLIENTAUTHENTICATION = "<CLIENT_AUTHENTICATION>";
        public const string KEY_PID = "<PID>";

#pragma warning restore 1591

        #endregion

        #region Public Functions

        /// <summary>
        /// Start service on Linux server
        /// </summary>
        /// <param name="address">Linux Server IP Address</param>
        /// <param name="type"><see cref=" LinuxServiceType"/></param>
        public static void StartService(IPAddress address, LinuxServiceType type)
        {
            string command = string.Empty;

            if (LinuxServiceType.DHCP == type)
            {
                command = "/etc/init.d/isc-dhcp-server start";
            }
            else if (LinuxServiceType.BOOTP == type)
            {
                command = "/etc/init.d/bootp start";
            }
            else if (LinuxServiceType.TFTP == type)
            {
                command = "/etc/init.d/xinetd start";
            }
            else if (LinuxServiceType.RARP == type)
            {
                command = "service rarpd start";
            }
            else if (LinuxServiceType.DHCPV6 == type)
            {
                command = "/etc/init.d/isc-dhcp-server6 start";
            }

            TraceFactory.Logger.Info("Starting {0} service.".FormatWith(type));
            ExecuteCommand(address, command);
        }

        /// <summary>
        /// Restart service on Linux server
        /// </summary>
        /// <param name="address">Linux Server IP Address</param>
        /// <param name="type"><see cref=" LinuxServiceType"/></param>
        public static void RestartService(IPAddress address, LinuxServiceType type)
        {
            string command = string.Empty;

            if (LinuxServiceType.DHCP == type)
            {
                command = "/etc/init.d/isc-dhcp-server restart";
            }
            else if (LinuxServiceType.BOOTP == type)
            {
                command = "/etc/init.d/bootp restart";
            }
            else if (LinuxServiceType.TFTP == type)
            {
                command = "/etc/init.d/xinetd restart";
            }
            else if (LinuxServiceType.RARP == type)
            {
                command = "service rarpd restart";
            }
            else if (LinuxServiceType.DHCPV6 == type)
            {
                command = "/etc/init.d/isc-dhcp-server6 restart";
            }

            TraceFactory.Logger.Info("Restarting {0} service.".FormatWith(type));
            ExecuteCommand(address, command);
        }

        /// <summary>
        /// Stop service on Linux server
        /// </summary>
        /// <param name="address">Linux Server IP Address</param>
        /// <param name="type"><see cref=" LinuxServiceType"/></param>
        public static void StopService(IPAddress address, LinuxServiceType type)
        {
            string command = string.Empty;

            if (LinuxServiceType.DHCP == type)
            {
                command = "/etc/init.d/isc-dhcp-server stop";
            }
            else if (LinuxServiceType.BOOTP == type)
            {
                command = "/etc/init.d/bootp stop";
            }
            else if (LinuxServiceType.TFTP == type)
            {
                command = "/etc/init.d/xinetd stop";
            }
            else if (LinuxServiceType.RARP == type)
            {
                command = "service rarpd stop";
            }
            else if (LinuxServiceType.DHCPV6 == type)
            {
                command = "/etc/init.d/isc-dhcp-server6 stop";
            }

            TraceFactory.Logger.Info("Stopping {0} service.".FormatWith(type));
            ExecuteCommand(address, command);
        }

        /// <summary>
        /// Reserve IP Address
        /// </summary>
        /// <param name="linuxServerIPAddress">Linux server IP Address</param>
        /// <param name="addressToReserve">Address to reserve</param>
        /// <param name="macAddress">Mac address of device</param>
        /// <param name="reservationIndex">Reservation Index</param>
        /// <param name="gateway">Gateway</param>
        /// <param name="subnetMask">Subnet Mask</param>
        /// <param name="type"><see cref=" LinuxServiceType"/></param>
        public static void ReserveIPAddress(IPAddress linuxServerIPAddress, IPAddress addressToReserve, string macAddress, LinuxServiceType type, int reservationIndex = 0, IPAddress gateway = null, IPAddress subnetMask = null)
        {
            TraceFactory.Logger.Info("Reserving {0} IP Address in {1}".FormatWith(addressToReserve, type));
            ReserveIP(linuxServerIPAddress, addressToReserve, macAddress, type, gateway, subnetMask);
        }

        /// <summary>
        /// Configure <see cref=" LinuxServiceType"/> file
        /// </summary>
        /// <param name="address">Linux server IP Address</param>
        /// <param name="type"><see cref=" LinuxServiceType"/></param>
        /// <param name="winFilePath">Configuration File Path</param>
        /// <param name="keyValue">Current value in file</param>
        /// <param name="configureValue">New value to be configured</param>
        public static void ConfigureFile(IPAddress address, LinuxServiceType type, string winFilePath, Collection<string> keyValue, Collection<string> configureValue)
        {
            string linuxFilePath = string.Empty;

            if (LinuxServiceType.DHCP == type)
            {
                linuxFilePath = LINUX_DHCP_PATH;
            }
            else if (LinuxServiceType.BOOTP == type)
            {
                linuxFilePath = LINUX_BOOTP_PATH;
            }
            else if (LinuxServiceType.TFTP == type)
            {
                linuxFilePath = LINUX_TFTP_PATH;
            }
            else if (LinuxServiceType.DHCPV6 == type)
            {
                linuxFilePath = LINUX_DHCPV6_PATH;
            }
            else
            {
                TraceFactory.Logger.Info("Invalid type: {0} for configuring file.".FormatWith(type));
                return;
            }

            ConfigureFile(address, winFilePath, linuxFilePath, keyValue, configureValue);
        }

        /// <summary>
        /// Configure Ethers file
        /// </summary>
        /// <param name="address">Linux server IP Address</param>
        /// <param name="filePath">File path from where file needs to be fetched</param>
        /// <param name="keyValue">Current value in file</param>
        /// <param name="configureValue">New value to be configured</param>
        public static void ConfigureEthersFile(IPAddress address, string filePath, Collection<string> keyValue, Collection<string> configureValue)
        {
            ConfigureFile(address, filePath, LINUX_ETHERS_PATH, keyValue, configureValue);
        }

        /// <summary>
        /// Configure Hosts file
        /// </summary>
        /// <param name="address">Linux server IP Address</param>
        /// <param name="filePath">File path from where file needs to be fetched</param>
        /// <param name="keyValue">Current value in file</param>
        /// <param name="configureValue">New value to be configured</param>
        public static void ConfigureHostsFile(IPAddress address, string filePath, Collection<string> keyValue, Collection<string> configureValue)
        {
            ConfigureFile(address, filePath, LINUX_HOSTS_PATH, keyValue, configureValue);
        }

        /// <summary>
        /// Configure PSK file
        /// </summary>
        /// <param name="address">Linux server IP Address</param>
        /// <param name="filePath">File path from where file needs to be fetched</param>
        /// <param name="keyValue">Current value in file</param>
        /// <param name="configureValue">New value to be configured</param>
        public static void ConfigurePSKFile(IPAddress address, string filePath, Collection<string> keyValue, Collection<string> configureValue)
        {
            TraceFactory.Logger.Info("Configuring PSK File with Values :{0}".FormatWith(configureValue));
            ConfigureFile(address, filePath, LINUX_PSK_PATH, keyValue, configureValue);
        }

        /// <summary>
        /// Configure IPsec file
        /// </summary>
        /// <param name="address">Linux server IP Address</param>
        /// <param name="filePath">File path from where file needs to be fetched</param>
        /// <param name="keyValue">Current value in file</param>
        /// <param name="configureValue">New value to be configured</param>
        public static void ConfigureIPsecFile(IPAddress address, string filePath, Collection<string> keyValue, Collection<string> configureValue)
        {
            TraceFactory.Logger.Info("Configuring IPSecurity File with Values : {0}".FormatWith(configureValue));
            ConfigureFile(address, filePath, LINUX_IPSEC_PATH, keyValue, configureValue);
        }

        /// <summary>
        /// Configure Racoon file
        /// </summary>
        /// <param name="address">Linux server IP Address</param>
        /// <param name="filePath">File path from where file needs to be fetched</param>
        /// <param name="keyValue">Current value in file</param>
        /// <param name="configureValue">New value to be configured</param>
        public static void ConfigureRacoonFile(IPAddress address, string filePath, Collection<string> keyValue, Collection<string> configureValue)
        {
            TraceFactory.Logger.Info("COnfiguring Racoon File with Values: {0}".FormatWith(configureValue));
            ConfigureFile(address, filePath, LINUX_RACOON_PATH, keyValue, configureValue);
        }

        /// <summary>
        /// Create IPsec Rule
        /// </summary>
        /// <param name="address">Linux server IP Address</param>
        public static void CreateIPSecRule(IPAddress address)
        {
            TraceFactory.Logger.Info("Creating IPSecurity Rule");
            string command = "{0}{1}".FormatWith(BACKUP_FOLDER_PATH, CREATE_RULE_FILE);

            ExecuteCommand(address, command);
            TraceFactory.Logger.Info("Successfully Created IPSecurity Rule in Linux");
        }

        /// <summary>
        /// Copy file from one location to other
        /// </summary>
        /// <param name="address">Linux server IP Address</param>
        /// <param name="fromFilePath">File path from where file needs to be fetched</param>
        /// <param name="toFilePath">File location to be copied</param>
        public static void CopyFile(IPAddress address, string fromFilePath, string toFilePath)
        {
            string command = "cp {0} {1}".FormatWith(fromFilePath, toFilePath);
            ExecuteCommand(address, command);
        }

        /// <summary>
        /// Replace backed up config files
        /// </summary>
        /// <param name="address"></param>
        public static void ReplaceOriginalFiles(IPAddress address)
        {
            CopyFile(address, string.Concat(DEFAULT_FILES_PATH, DHCP_FILE), LINUX_DHCP_PATH);
            CopyFile(address, string.Concat(DEFAULT_FILES_PATH, BOOTP_FILE), LINUX_BOOTP_PATH);
            CopyFile(address, string.Concat(DEFAULT_FILES_PATH, TFTP_FILE), LINUX_TFTP_PATH);
            CopyFile(address, string.Concat(DEFAULT_FILES_PATH, ETHERS_FILE), LINUX_ETHERS_PATH);
            CopyFile(address, string.Concat(DEFAULT_FILES_PATH, HOSTS_FILE), LINUX_HOSTS_PATH);
            CopyFile(address, string.Concat(DEFAULT_FILES_PATH, DHCPV6_FILE), LINUX_DHCPV6_PATH);
        }

        /// <summary>
        /// Clear DHCPv4 and DHCPv6 leases
        /// </summary>
        /// <param name="address"></param>
        public static void ClearLease(IPAddress address)
        {
            CopyFile(address, string.Concat(DEFAULT_FILES_PATH, DHCP_LEASES_FILE), LINUX_DHCP_LEASES_PATH);
            CopyFile(address, string.Concat(DEFAULT_FILES_PATH, DHCPV6_LEASES_FILE), LINUX_DHCPV6_LEASES_PATH);
        }

        /// <summary>
        /// Change DHCP Host name
        /// </summary>
        /// <param name="address">Linux Server IP Address</param>
        /// <param name="oldHostname">Old Hostname</param>
        /// <param name="newHostname">New Hostname to be set</param>
        public static void ChangeDhcpHostname(IPAddress address, string oldHostname, string newHostname)
        {
            TraceFactory.Logger.Info("Changing hostname from \"{0}\" to \"{1}\"".FormatWith(oldHostname, newHostname));
            string command = "sed -i 's/{0}/{1}/' {2}".FormatWith(oldHostname, newHostname, LINUX_DHCP_PATH);
            ExecuteCommand(address, command);
        }

        /// <summary>
        /// Find and replace a value in specified file
        /// </summary>
        /// <param name="address">Linux server address</param>
        /// <param name="path">Path to file</param>
        /// <param name="findString">Find string</param>
        /// <param name="replaceString">Replace string</param>
        public static void ReplaceValue(IPAddress address, string path, string findString, string replaceString)
        {
            string command = @"find {0} -type f -exec sed -i 's/{1}/{2}/g' {3} \;".FormatWith(path, findString, replaceString, "{}");
            ExecuteCommand(address, command);
        }

        /// <summary>
        /// Ping IPv4 address
        /// </summary>
        /// <param name="serverAddress">Linux server IP Address</param>
        /// <param name="addressToPing">IP Address to ping</param>
        /// <returns>Ping details</returns>
        public static string PingIPv4Address(IPAddress serverAddress, IPAddress addressToPing)
        {
            TraceFactory.Logger.Info("Pinging IPV4 Address: {0}".FormatWith(addressToPing));
            string command = "ping -c 4 {0} > {1}".FormatWith(addressToPing, TEMP_FILE_PATH);

            ExecuteCommand(serverAddress, command);

            Thread.Sleep(TimeSpan.FromMinutes(1));
            string windowsPath = Path.GetTempFileName();
            CopyFileFromLinux(serverAddress, TEMP_FILE_PATH, windowsPath);

            Thread.Sleep(TimeSpan.FromSeconds(50));
            return File.ReadAllText(windowsPath).ToString().Trim();
        }

        /// <summary>
        /// Removes comments in the specified file.
        /// Note: Replaces '#' character with empty string. If there are multiple occurrences, call same function multiple times.
        /// </summary>
        /// <param name="address">Linux server IP Address</param>
        /// <param name="filePath">File Path to remove comments</param>
        public static void RemoveComments(IPAddress address, string filePath)
        {
            string command = "sed -i 's/{0}/{1}/' {2}".FormatWith("#", string.Empty, filePath);
            ExecuteCommand(address, command);
        }

        /// <summary>
        /// Copy file from Linux machine to Windows machine
        /// </summary>
        /// <param name="address">Linux server IP Address</param>
        /// <param name="fromFilePath">File path from where file needs to be fetched</param>
        /// <param name="toFilePath">File path to where file has to be copied</param>
        public static void CopyFileFromLinux(IPAddress address, string fromFilePath, string toFilePath)
        {
            string pscpFilePath = "pscp.exe";

            try
            {
                fromFilePath = string.Format(CultureInfo.CurrentCulture, "{0}@{1}:{2}", USER_NAME, address, fromFilePath);

                if (!File.Exists(pscpFilePath))
                {
                    File.WriteAllBytes(pscpFilePath, Resource.pscp);
                }

                // Command format: <pscpexe_path> -pw <linuxserver_password> <windows_filepath> <linuxserver_userid>@<linuxserver_ip>:<linux_filepath>
                // '< Y' is added at end for adding server's host key to registry (when command is run first time, pscp will expect user to press 'y/n' to continue)
                // Direct command execution throws exception hence commands are executed using batch file
                string command = string.Concat(pscpFilePath, " -pw {0} {1} {2} < {3}".FormatWith(PASSWORD, fromFilePath, toFilePath, CtcUtility.CreateFile("Y")));
                TraceFactory.Logger.Info("Command to copy file from Linux to Windows: {0}".FormatWith(command));
                RunBatchFile(command);
                Thread.Sleep(TimeSpan.FromSeconds(30));
            }
            finally
            {
                //
            }
        }

        /// <summary>
        /// Execute command on Linux server
        /// </summary>
        /// <param name="address">Linux server IP Address</param>
        /// <param name="command">Command to be executed</param>
        public static string ExecuteCommand(IPAddress address, string command)
        {
            TraceFactory.Logger.Debug("Executing command: {0}".FormatWith(command));
            SSHProtocol sshProtocol = new SSHProtocol(USER_NAME, PASSWORD, address);

            if (sshProtocol.Connect())
            {
                // Login as root before executing commands
                LoginAsRoot(sshProtocol);
                sshProtocol.Send(command);
                return sshProtocol.Send("\n");
            }
            else
            {
                return string.Empty;
            }
        }

        #region DHCPv6

        /// <summary>
        /// Get DHCPv6 Subnet mask
        /// </summary>
        /// <param name="address">Linux server address</param>
        /// <returns>DHCPv6 Subnet mask</returns>
        public static string GetDhcpv6SubnetMask(IPAddress address)
        {
            // From DHCPv6 file, find line matching 'subnet6' key; split values based on ' ' and pick the 2nd value from it
            string command = "grep subnet6 {0} | cut -d ' ' -f2 > {1}".FormatWith(LINUX_DHCPV6_PATH, TEMP_FILE_PATH);
            ExecuteCommand(address, command);

            string windowsPath = Path.GetTempFileName();
            CopyFileFromLinux(address, TEMP_FILE_PATH, windowsPath);

            return File.ReadAllText(windowsPath).ToString().Trim();
        }

        /// <summary>
        /// Ping IPv6 Address
        /// </summary>
        /// <param name="linuxAddress">Linux server IP Address</param>
        /// <param name="ipv6Address">IPv6 address to ping</param>
        /// <returns>Ping details</returns>
        public static string PingIPv6Address(IPAddress linuxAddress, IPAddress ipv6Address)
        {
            string command = "ping6 -c 4 {0} > {1}".FormatWith(ipv6Address, TEMP_FILE_PATH);

            ExecuteCommand(linuxAddress, command);

            Thread.Sleep(TimeSpan.FromMinutes(1));
            string windowsPath = Path.GetTempFileName();
            CopyFileFromLinux(linuxAddress, TEMP_FILE_PATH, windowsPath);

            return File.ReadAllText(windowsPath).ToString().Trim();
        }

        /// <summary>
        /// Get DHCPv6 From Range
        /// </summary>
        /// <param name="address">Linux server address</param>
        /// <returns>DHCPv6 From Range</returns>
        public static string GetDhcpv6FromRange(IPAddress address)
        {
            // From DHCPv6 file, find line matching 'range6' key, split values based on ' ' and pick 2nd value from it
            string command = "grep range6 {0} | cut -d ' ' -f2 > {1}".FormatWith(LINUX_DHCPV6_PATH, TEMP_FILE_PATH);
            ExecuteCommand(address, command);

            string windowsPath = Path.GetTempFileName();
            CopyFileFromLinux(address, TEMP_FILE_PATH, windowsPath);

            return File.ReadAllText(windowsPath).ToString().Trim();
        }

        /// <summary>
        /// Get DHCPv6 To Range
        /// </summary>
        /// <param name="address">Linux server address</param>
        /// <returns>DHCPv6 To Range</returns>
        public static string GetDhcpv6ToRange(IPAddress address)
        {
            // From DHCPv6 file, find line matching 'range6' key, split values based on ' ' and pick 3rd value from it. Semicolon will be post fixed to this value, discard it.
            string command = "grep range6 {0} | cut -d ' ' -f3 | cut -d ';' -f1 > {1}".FormatWith(LINUX_DHCPV6_PATH, TEMP_FILE_PATH);
            ExecuteCommand(address, command);

            string windowsPath = Path.GetTempFileName();
            CopyFileFromLinux(address, TEMP_FILE_PATH, windowsPath);

            return File.ReadAllText(windowsPath).ToString().Trim();
        }

        /// <summary>
        /// Get DHCPv6 Prefix
        /// </summary>
        /// <param name="address">Linux server address</param>
        /// <returns>DHCPv6 Prefix</returns>
        public static string GetDhcpv6Prefix(IPAddress address)
        {
            // From DHCPv6 file, find line matching 'range6' key, split values based on ' ' and pick 3rd value from it. Semicolon will be post fixed to this value, discard it.
            string command = "grep prefix6 {0} | cut -d ' ' -f2 > {1}".FormatWith(LINUX_DHCPV6_PATH, TEMP_FILE_PATH);
            ExecuteCommand(address, command);

            string windowsPath = Path.GetTempFileName();
            CopyFileFromLinux(address, TEMP_FILE_PATH, windowsPath);

            return File.ReadAllText(windowsPath).ToString().Trim();
        }

        /// <summary>
        /// Delete last line of TFTP config file
        /// </summary>
        /// <param name="address">Linux server address</param>
        public static void DeleteTftpConfigLastLine(IPAddress address)
        {
            string command = "sed -ie '$d' {0}".FormatWith(LINUX_TFTP_PATH);
            ExecuteCommand(address, command);
        }

        /// <summary>
        /// Get all stateless addresses
        /// </summary>
        /// <param name="address">Linux server IP Address</param>
        /// <returns></returns>
        public static Collection<IPAddress> GetStatelessAddress(IPAddress address)
        {
            string command = "ip -6 addr show | grep inet6 | sed 's/.*inet6//' > {0}".FormatWith(TEMP_FILE_PATH);

            ExecuteCommand(address, command);

            Thread.Sleep(TimeSpan.FromSeconds(30));
            string windowsPath = Path.GetTempFileName();
            CopyFileFromLinux(address, TEMP_FILE_PATH, windowsPath);

            Thread.Sleep(TimeSpan.FromMinutes(1));
            string[] addressDetails = System.IO.File.ReadAllLines(windowsPath);
            //string[] addressDetails = System.IO.File.ReadAllLines(@"C:\Users\ADMINI~1\AppData\Local\Temp\tmp8B20.tmp");
            Collection<IPAddress> statelessAddress = new Collection<IPAddress>();

            foreach (string addressDetail in addressDetails)
            {
                if (!addressDetail.Trim().StartsWith("fe80") && addressDetail.Trim().EndsWith("dynamic"))
                {
                    statelessAddress.Add(IPAddress.Parse(addressDetail.Trim().Split(' ')[0].Replace("/64", string.Empty).Replace("/128", string.Empty)));
                }
            }

            return statelessAddress;
        }

        #endregion

        #endregion

        #region Private Functions

        /// <summary>
        /// Login with root privileges
        /// </summary>
        /// <param name="sshProtocol"><see cref=" SSHProtocol"/></param>
        private static void LoginAsRoot(SSHProtocol sshProtocol)
        {
            sshProtocol.Send("su -");
            string resultString = sshProtocol.Send("\n");

            if (resultString.Contains("Password", StringComparison.CurrentCultureIgnoreCase))
            {
                sshProtocol.Send(PASSWORD);
                sshProtocol.Send("\n");
            }
        }

        /// <summary>
        /// Replace configuration values in specified file and copy to linux server
        /// </summary>
        /// <param name="address">Linux server IP Address</param>
        /// <param name="winFilePath">File Path</param>
        /// <param name="linuxFilePath">Linux file path to be copied</param>
        /// <param name="keyValue">Current value in file</param>
        /// <param name="configureValue">New value to be configured</param>
        /// <param name="isReservation">true for configuring file for reservation</param>
        private static void ConfigureFile(IPAddress address, string winFilePath, string linuxFilePath, Collection<string> keyValue, Collection<string> configureValue, bool isReservation = false)
        {
            if (keyValue.Count != configureValue.Count)
            {
                TraceFactory.Logger.Info("Parameter count mismatch.");
                return;
            }

            if (!File.Exists(winFilePath))
            {
                TraceFactory.Logger.Info("File: {0} not found.".FormatWith(winFilePath));
                return;
            }

            string fileContents = File.ReadAllText(winFilePath);

            for (int i = 0; i < keyValue.Count; i++)
            {
                fileContents = fileContents.Replace(keyValue[i], configureValue[i]);
            }

            if (isReservation)
            {
                if (fileContents.Contains("# "))
                {
                    fileContents = fileContents.Replace("# ", string.Empty);
                }
            }

            File.WriteAllText(winFilePath, fileContents);
            Thread.Sleep(TimeSpan.FromMinutes(1));
            CopyFileToLinux(address, winFilePath, linuxFilePath);
        }

        /// <summary>
        /// Copy file from Windows machine to Linux machine
        /// </summary>
        /// <param name="address">Linux server IP Address</param>
        /// <param name="fromFilePath">File path from where file needs to be fetched</param>
        /// <param name="toFilePath">File path to where file has to be copied</param>
        private static void CopyFileToLinux(IPAddress address, string fromFilePath, string toFilePath)
        {
            TraceFactory.Logger.Info("Copy file from Windows to Linux machine");
            string pscpFilePath = "pscp.exe";

            try
            {
                toFilePath = string.Format(CultureInfo.CurrentCulture, "{0}@{1}:{2}", USER_NAME, address, toFilePath);

                if (!File.Exists(pscpFilePath))
                {
                    File.WriteAllBytes(pscpFilePath, Resource.pscp);
                }

                // Command format: <pscpexe_path> -pw <linuxserver_password> <windows_filepath> <linuxserver_userid>@<linuxserver_ip>:<linux_filepath>
                // '< Y' is added at end for adding server's host key to registry (when command is run first time, pscp will expect user to press 'y/n' to continue)
                // Direct command execution throws exception hence commands are executed using batch file
                string command = string.Concat(pscpFilePath, " -pw {0} {1} {2} < {3}".FormatWith(PASSWORD, fromFilePath, toFilePath, CtcUtility.CreateFile("Y")));
                TraceFactory.Logger.Info("Command to copy file from Windows to Linux Machine : {0}".FormatWith(command));
                RunBatchFile(command);
            }
            finally
            {
                //
            }
        }

        /// <summary>
        /// Reserve IP Address on requested service
        /// </summary>
        /// <param name="linuxServerAddress">Linux server IP Address</param>
        /// <param name="addressToReserve">Address to reserve</param>
        /// <param name="macAddress">Mac Address of device</param>
        /// <param name="type"><see cref=" LinuxServiceType"/></param>		
        /// <param name="gateway">Gateway for Scope</param>
        /// <param name="subnetMask">Subnet mask of scope</param>
        private static void ReserveIP(IPAddress linuxServerAddress, IPAddress addressToReserve, string macAddress, LinuxServiceType type, IPAddress gateway = null, IPAddress subnetMask = null)
        {
            string linuxFilePath = string.Empty;
            string linuxBackupPath = string.Empty;
            string winFilePath = Path.GetTempPath();

            Collection<string> keyValue = new Collection<string>();
            Collection<string> configureValue = new Collection<string>();

            // SubnetMask is mandatory: BootP
            if (null == subnetMask)
            {
                subnetMask = IPAddress.Parse("255.255.255.0");
            }

            if (null == gateway)
            {
                int length = linuxServerAddress.ToString().Length;
                int lastIndex = linuxServerAddress.ToString().LastIndexOf('.') + 1;

                // Configure gateway
                gateway = IPAddress.Parse(string.Format(CultureInfo.CurrentCulture, "{0}{1}", linuxServerAddress.ToString().Remove(lastIndex, length - lastIndex), "1"));
            }

            if (macAddress.Contains("-"))
            {
                macAddress = macAddress.Replace('-', ':');
            }
            else if (!macAddress.Contains(":"))
            {
                // Insert ':' char at every 3rd position of mac address
                macAddress = Regex.Replace(macAddress, ".{2}", "$0:");
                macAddress = macAddress.Remove(macAddress.Length - 1, 1);
            }

            keyValue.Add(KEY_IPADDRESS);
            keyValue.Add(KEY_SUBNET_MASK);
            keyValue.Add(KEY_GATEWAY);
            keyValue.Add(KEY_MAC_ADDRESS);

            configureValue.Add(addressToReserve.ToString());
            configureValue.Add(subnetMask.ToString());
            configureValue.Add(gateway.ToString());
            configureValue.Add(macAddress);

            if (LinuxServiceType.DHCP == type)
            {
                // Format
                /*
					host Aeneas1 {
					hardware ethernet 00:9c:02:84:55:46;
					fixed-address 192.168.2.65;
					}
				 * */

                linuxBackupPath = linuxFilePath = LINUX_DHCP_PATH;
                winFilePath = Path.Combine(winFilePath, DHCP_FILE);
            }
            else if (LinuxServiceType.BOOTP == type)
            {
                // Format
                /*				
				client:\
				ha="00:9c:02:84:c1:6e":\
				ip=192.168.192.160:\
				gw=192.168.192.1:\
				sm=255.255.255.0:\
				:T144="/tftpboot/tftp.cfg":
				 * */

                // Bootp File doesn't have any data initially, hence directly picking default file
                linuxBackupPath = string.Concat(BACKUP_FOLDER_PATH, BOOTP_FILE);
                winFilePath = Path.Combine(winFilePath, BOOTP_FILE);
                linuxFilePath = LINUX_BOOTP_PATH;
            }
            else if (LinuxServiceType.DHCPV6 == type)
            {
                // Format
                /*
				 host Aeneas2 {
				 host-identifier option dhcp6.client-id 00:02:00:00:00:0b:2c:59:e5:7a:92:f4;
				 fixed -address6 2000:192:192:192:192:192:192:125;
				 }
				 * */

                // Uuid is specific to only Dhcpv6
                keyValue.Add(KEY_UUID);
                configureValue.Add(macAddress);

                linuxBackupPath = linuxFilePath = LINUX_DHCPV6_PATH;
                winFilePath = Path.Combine(winFilePath, DHCPV6_FILE);
            }
            else
            {
                // Need to implement for other services also
                throw new NotImplementedException();
            }

            // Copy configuration file from Linux to local temporary folder
            CopyFileFromLinux(linuxServerAddress, linuxBackupPath, winFilePath);

            // Replace values and copy back to Linux
            ConfigureFile(linuxServerAddress, winFilePath, linuxFilePath, keyValue, configureValue, isReservation: true);
        }

        /// <summary>
        /// Create batch file and execute command
        /// </summary>
        /// <param name="command">Command to be executed</param>
        private static void RunBatchFile(string command)
        {
            TraceFactory.Logger.Debug("Executing command: {0} from working directory.".FormatWith(command));
            string batFilePath = Path.Combine(Directory.GetCurrentDirectory(), System.Guid.NewGuid() + ".bat");
            using (StreamWriter writer = new StreamWriter(batFilePath, false))
            {
                writer.WriteLine(command);
                writer.Close();
                Process.Start(batFilePath, Environment.NewLine);
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        #endregion
    }
}
