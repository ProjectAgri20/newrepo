using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceModel;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    /// <summary>
    /// Firewall Profile
    /// </summary>
    [Flags]
    public enum FirewallProfile
    {
        /// <summary>
        /// Public Firewall Profile
        /// </summary>           
        Public = 1,
        /// <summary>
        /// Private Firewall Profile
        /// </summary>        
        Private = 2,
        /// <summary>
        /// Domain Firewall Profile
        /// </summary>         
        Domain = 4,
        /// <summary>
        /// Domain and Private Firewall Profile
        /// </summary>
        [EnumValue("8")]
        PrivateAndDomain = Domain | Private,
        /// <summary>
        /// Domain,Private and Public Firewall Profile
        /// </summary>
        [EnumValue("16")]
        PrivateDomainAndPublic = Domain | Private | Public
    }

    /// <summary>
    /// Contains the Ping Static details.
    /// </summary>
    public class PingStatics
    {
        /// <summary>
        /// Gets or sets IP Address
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets Status of the last ping
        /// </summary>
        public IPStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the total number of pings requested
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets ping success count
        /// </summary>
        public int PassCount { get; set; }

        /// <summary>
        /// Gets or sets the pass percentage
        /// </summary>
        public int PassPercentage { get; set; }

        /// <summary>
        /// Gets or sets the Message
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// Connectivity Utility Interface provides different functionalities to check connectivity between printer and client
    /// </summary>
    [ServiceContract]
    public interface IConnectivityUtility
    {
        /// <summary>
        /// Ping IP Address
        /// </summary>
        /// <param name="ipAddress">Printer IP Address</param>
        /// <returns>true if ping successful, false otherwise</returns>
        [OperationContract]
        bool Ping(IPAddress ipAddress);

        /// <summary>
        /// Install and Print file without Drivers
        /// </summary>
        /// <param name="printerFamily">Printer Family</param>
        /// <param name="ipAddress">Printer IP Address</param>
        /// <param name="filePath">Print File Path</param>        
        /// <returns>true if printing is successful, false otherwise</returns>
        [OperationContract]
        bool PrintLpr(string printerFamily, IPAddress ipAddress, string filePath);

        /// <summary>
        /// LPR Print Asynchronously.
        /// Note: To track the status of the job, use GetLPRPrintStatus() and match using Guid returned while calling this function.
        /// </summary>
        /// <param name="printerFamily">Printer Family</param>
        /// <param name="ipAddress">IP Address of printer</param>
        /// <param name="filePath">File Path</param>
        /// <returns>Guid created for the LPR Print Job</returns>
        [OperationContract]
        Guid PrintLprAsync(Printer.PrinterFamilies printerFamily, IPAddress ipAddress, string filePath);

        /// <summary>
        /// Get the LPR Print print status
        /// Note: This needs to called after PrintLprAsync() to get the status
        /// </summary>
        /// <returns>Gets the LPR print status (true/flase)</returns>
        [OperationContract]
        bool GetLprPrintStatus(bool stopThread);

        /// <summary>
        /// Check if Ftp Connection is successful
        /// </summary>
        /// <param name="ipAddress">Printer IP Address</param>
        /// <param name="printerFamily">Printer Family</param>
        /// <returns>true if connection successful, false otherwise</returns>
        [OperationContract]
        bool IsFtpAccessible(IPAddress ipAddress, PrinterFamilies printerFamily);

        /// <summary>
        /// Check if Telnet Connection is successful
        /// </summary>
        /// <param name="ipAddress">Printer IP Address</param>
        /// <param name="printerFamily">Printer Family</param>
        /// <returns>true if connection successful, false otherwise</returns>
        [OperationContract]
        bool IsTelnetAccessible(IPAddress ipAddress, PrinterFamilies printerFamily);

        /// <summary>
        /// Check if Snmp Connection is successful
        /// </summary>
        /// <param name="ipAddress">Printer IP Address</param>
        /// <param name="printerFamily">Printer Family</param>
        /// <returns>true if connection successful, false otherwise</returns>
        [OperationContract]
        bool IsSnmpAccessible(IPAddress ipAddress, PrinterFamilies printerFamily);

        /// <summary>
        /// Create IP Sec rule on Local Client Machine
        /// </summary>
        /// <param name="ruleSettings">Security rule Settings: <see cref="SecurityRuleSettings"/></param>
        /// <param name="enableRule">true to enable rule, false to just create the rule/></param> 
        /// <param name="enableProfiles">true to enable profiles, false to just create the rule/></param>
        /// <returns>true if created successfully, false otherwise</returns>
        [OperationContract]
        bool CreateIPsecRule(SecurityRuleSettings ruleSettings, bool enableRule, bool enableProfiles);

        /// <summary>
        /// Delete IP Sec rule on Local Client Machine
        /// </summary>
        /// <param name="ruleSettings">Security rule Settings: <see cref="SecurityRuleSettings"/></param>
        /// <returns>true if deleted successfully, false otherwise</returns>
        [OperationContract]
        bool DeleteIPsecRule(SecurityRuleSettings ruleSettings);

        /// <summary>
        /// Delete All IPsec rules created on local machine
        /// </summary>
        /// <returns>true if all rules are deleted, false otherwise</returns>
        [OperationContract]
        bool DeleteAllIPsecRules();

        /// <summary>
        /// Setting Firewall Domain and Public Profiles
        /// </summary>
        /// <returns>true if enabled, false otherwise</returns>
        [OperationContract]
        bool SetFirewallDomainAndPublicProfile(bool enable);

        /// <summary>
        /// Enable Firewall profile
        /// </summary>
        /// <param name="profile"><see cref="FirewallProfile"/></param>
        /// <returns>true if profile(s) is enabled</returns>
        [OperationContract]
        bool EnableFirewallProfile(FirewallProfile profile, bool allowInBound = false);

        /// <summary>
        /// Disable Firewall profile
        /// </summary>
        /// <param name="profile"><see cref="FirewallProfile"/></param>
        /// <returns>true if profile(s) is disabled</returns>
        [OperationContract]
        bool DisableFirewallProfile(FirewallProfile profile);

        /// <summary>
        /// Install CA certificate on client machine in 'root' folder
        /// </summary>
        /// <param name="filePath">CA Certificate file path</param>
        /// <returns>true if certificate is installed successfully, false otherwise</returns>
        [OperationContract]
        bool InstallCACertificate(string filePath);

        /// <summary>
        /// Install ID certificate on client machine in 'Personal' folder
        /// </summary>
        /// <param name="filePath">ID Certificate file path</param>
        /// <param name="password">ID Password</param>
        /// <returns>true if certificate is installed successfully, false otherwise</returns>
        [OperationContract]
        bool InstallIDCertificate(string filePath, string password);

        /// <summary>
        /// Pings continuously for the specified time for all the given addresses.
        /// This method doesn't support multi thread.
        /// </summary>
        /// <param name="ipAddressList">List of address should be pinged</param>
        /// <param name="timeOut">Time to ping</param>
        [OperationContract]
        void PingContinuously(Collection<IPAddress> ipAddressList, TimeSpan timeOut);

        /// <summary>
        /// Gets the continuous ping statistics details, before calling this method "PingContinuously" should be called.
        /// </summary>
        /// <returns>Ping Statistic details</returns>
        [OperationContract]
        Collection<PingStatics> GetContinuousPingStatistics();

        /// <summary>
        /// Gets the local address.
        /// </summary>
        /// <param name="subNetConstraint">Constrains the search to a specific SubNet address.</param>
        /// <param name="subNetMask">The sub net mask.</param>
        /// <returns>IPAddress of local machine</returns>
        [OperationContract]
        IPAddress GetLocalAddress(string subNetConstraint = null, string subNetMask = "255.0.0.0");

        /// <summary>
        /// Returns the client network name provided by a particular server.
        /// </summary>
        /// <param name="serverIpAddress">IP Address of the server</param>		
        /// <returns>IPAddress of local machine</returns>
        [OperationContract]
        string GetClientNetworkName(string serverIPAddress);

        /// <summary>
        /// Disables the specified network connection.
        /// </summary>
        /// <param name="name">The name of the network connection.</param>
        [OperationContract]
        void DisableNetworkConnection(string name);

        /// <summary>
        /// Enables the specified network connection.
        /// </summary>
        /// <param name="name">The name of the network connection.</param>
        [OperationContract]
        void EnableNetworkConnection(string name);

        /// <summary>
        /// Reboot Machine
        /// </summary>
        /// <param name="ipAddress">IPAddress of the Printer</param>        
        [OperationContract]
        void RebootMachine(IPAddress ipAddress);
    }
}
