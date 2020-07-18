using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.Threading;
using HP.ScalableTest.PluginSupport.Connectivity;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.PluginSupport.ConnectivityUtilityService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class ConnectivityUtility : IConnectivityUtility
    {
        #region Local variables

        /// <summary>
        /// Used to track PingContinuously when called asynchronously 
        /// </summary>
        Thread _pingThread = null;

        /// <summary>
        /// Printer object used for async lpr print
        /// </summary>
        Printer.Printer _lprPrinterObject = null;

        #endregion

        #region IConnectivityUtility implemented methods

        #region Ping

        /// <summary>
        /// Ping to the IP Address provided
        /// Note: Ping will try for max of 1 minute if ping is unsuccessful
        /// </summary>
        /// <param name="ipAddress">IP Address to ping</param>
        /// <returns>true if ping successful, false otherwise</returns>
        public bool Ping(IPAddress ipAddress)
        {
            return NetworkUtil.PingUntilTimeout(ipAddress, TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Pings continuously for the specified time for all the given addresses.
        /// This method doesn't support multi thread.
        /// </summary>
        /// <param name="ipAddressList">List of address should be pinged</param>
        /// <param name="timeOut">Time to ping</param>
        public void PingContinuously(Collection<IPAddress> ipAddressList, TimeSpan timeOut)
        {
            _pingThread = new Thread(() => CtcUtility.PingContinuously(ipAddressList, timeOut));
            _pingThread.Start();
        }

        /// <summary>
        /// Gets the continuous ping statistics details, before calling this method "PingContinuously" should be called.
        /// </summary>
        /// <returns>Ping Statistic details</returns>
        public Collection<PingStatics> GetContinuousPingStatistics()
        {
            _pingThread.Abort();
            return CtcUtility.GetContinuousPingStatistics();
        }

        #endregion

        #region Print

        /// <summary>
        /// Install and Print file without drivers
        /// </summary>
        /// <param name="printerFamily">Printer Family</param>
        /// <param name="ipAddress">Printer IP Address</param>        
        /// <param name="filePath">Print File Path</param>        
        /// <returns>true if printing is successful, false otherwise</returns>
        public bool PrintLpr(string printerFamily, IPAddress ipAddress, string filePath)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create(printerFamily, ipAddress.ToString());

            return printer.PrintLpr(ipAddress, filePath);
        }

        /// <summary>
        /// LPR Print Asynchronously.
        /// Note: To track the status of the job, use GetLPRPrintStatus() and match using Guid returned while calling this function.
        /// </summary>
        /// <param name="printerFamily">Printer Family</param>
        /// <param name="ipAddress">IP Address of printer</param>
        /// <param name="filePath">File Path</param>
        /// <returns>Guid created for the LPR Print Job</returns>
        public Guid PrintLprAsync(Printer.PrinterFamilies printerFamily, IPAddress ipAddress, string filePath)
        {
            _lprPrinterObject = Printer.PrinterFactory.Create(printerFamily, ipAddress);

            return _lprPrinterObject.PrintLprAsync(ipAddress, filePath);
        }

        /// <summary>
        /// Get the LPR Print print status
        /// </summary>
        /// <returns>Gets the LPR print status (true/flase)</returns>
        public bool GetLprPrintStatus(bool stopThread)
        {
            if (_lprPrinterObject.GetLprPrintStatus().Count > 0)
            {
                // Stop the thread based on the caller request
                if (stopThread)
                {
                    _lprPrinterObject.GetLprPrintStatus()[0].JobThread.Abort();
                }

                return _lprPrinterObject.GetLprPrintStatus()[0].JobStatus;
            }

            return false;

        }

        #endregion

        #region Accessibility

        /// <summary>
        /// Check if Ftp Connection is successful
        /// </summary>
        /// <param name="ipAddress">Printer IP Address</param>
        /// <param name="printerFamily">Printer Family</param>
        /// <returns>true if connection successful, false otherwise</returns>
        public bool IsFtpAccessible(IPAddress ipAddress, Printer.PrinterFamilies printerFamily)
        {
            string address = string.Empty;

            if (ipAddress.ToString().Contains(":"))
            {
                address = string.Concat("[", ipAddress, "]");
            }
            else
            {
                address = ipAddress.ToString();
            }

            string ftpUri = "{0}{1}/{2}".FormatWith("ftp://", address, CtcUtility.CreateFile("temp"));
            Printer.Printer printer = Printer.PrinterFactory.Create(printerFamily, ipAddress);

            return printer.IsFTPAccessible(ftpUri);
        }

        /// <summary>
        /// Check if Telnet Connection is successful
        /// </summary>
        /// <param name="ipAddress">Printer IP Address</param>
        /// <param name="printerFamily">Printer Family</param>
        /// <returns>true if connection successful, false otherwise</returns>
        public bool IsTelnetAccessible(IPAddress ipAddress, Printer.PrinterFamilies printerFamily)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create(printerFamily, ipAddress);

            return printer.IsTelnetAccessible(ipAddress);
        }

        /// <summary>
        /// Check if Snmp Connection is successful
        /// </summary>
        /// <param name="ipAddress">Printer IP Address</param>
        /// <param name="printerFamily">Printer Family</param>
        /// <returns>true if connection successful, false otherwise</returns>
        public bool IsSnmpAccessible(IPAddress ipAddress, Printer.PrinterFamilies printerFamily)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create(printerFamily, ipAddress);

            return printer.IsSnmpAccessible(ipAddress);
        }

        #endregion

        #region Firewall Rule

        /// <summary>
        /// Create IP Sec rule on Local Client Machine
        /// </summary>
        /// <param name="ruleSettings">Security rule Settings: <see cref="SecurityRuleSettings"/></param>
        /// <param name="enableRule">true to enable rule, false to just create the rule</param>
        /// <returns>true if created successfully, false otherwise</returns>
        public bool CreateIPsecRule(SecurityRuleSettings ruleSettings, bool enableRule, bool enableProfiles)
        {
            return CtcUtility.CreateIPsecRule(ruleSettings, enableRule, enableProfiles);
        }

        /// <summary>
        /// Delete IP Sec rule on Local Client Machine
        /// </summary>
        /// <param name="ruleSettings">Security rule Settings: <see cref="SecurityRuleSettings"/></param>
        /// <returns>true if deleted successfully, false otherwise</returns>
        public bool DeleteIPsecRule(SecurityRuleSettings ruleSettings)
        {
            return CtcUtility.DeleteIPsecRule(ruleSettings);
        }

        /// <summary>
        /// Delete All IPsec rules created on local machine
        /// </summary>
        /// <returns>true if all rules are deleted, false otherwise</returns>
        public bool DeleteAllIPsecRules()
        {
            return CtcUtility.DeleteAllIPsecRules();
        }

        /// <summary>
        /// Setting Firewall Domain and Public Profiles
        /// </summary>
        /// <param name="enable">true to turn on  false to turn off</param>
        /// <returns>true if enabled, false otherwise</returns>
        public bool SetFirewallDomainAndPublicProfile(bool enable)
        {
            return CtcUtility.SetFirewallDomainAndPublicProfile(enable);
        }

        /// <summary>
        /// Enable Firewall profile
        /// </summary>
        /// <param name="profile"><see cref="FirewallProfile"/></param>
        /// <returns>true if profile(s) is enabled</returns>
        public bool EnableFirewallProfile(FirewallProfile profile, bool allowInBound = false)
        {
            return CtcUtility.EnableFirewallProfile(profile, allowInBound);
        }

        /// <summary>
        /// Disable Firewall profile
        /// </summary>
        /// <param name="profile"><see cref="FirewallProfile"/></param>
        /// <returns>true if profile(s) is disabled</returns>
        public bool DisableFirewallProfile(FirewallProfile profile)
        {
            return CtcUtility.DisableFirewallProfile(profile);
        }

        #endregion

        #region Certificate

        /// <summary>
        /// Install CA certificate on client machine in 'root' folder
        /// </summary>
        /// <param name="filePath">CA Certificate file path</param>
        /// <returns>true if certificate is installed successfully, false otherwise</returns>
        public bool InstallCACertificate(string filePath)
        {
            return CtcUtility.InstallCACertificate(filePath);
        }

        /// <summary>
        /// Install ID certificate on client machine in 'Personal' folder
        /// </summary>
        /// <param name="filePath">ID Certificate file path</param>
        /// <param name="password">ID Password</param>
        /// <returns>true if certificate is installed successfully, false otherwise</returns>
        public bool InstallIDCertificate(string filePath, string password)
        {
            return CtcUtility.InstallIDCertificate(filePath, password);
        }

        #endregion

        #region Misc

        /// <summary>
        /// Gets the local address.
        /// </summary>
        /// <param name="subNetConstraint">Constrains the search to a specific SubNet address.</param>
        /// <param name="subNetMask">The sub net mask.</param>
        /// <returns>IPAddress of local machine</returns>
        public IPAddress GetLocalAddress(string subNetConstraint = null, string subNetMask = "255.0.0.0")
        {
            return NetworkUtil.GetLocalAddress(subNetConstraint, subNetMask);
        }

        /// <summary>
        /// Returns the client network name provided by a particular server.
        /// </summary>
        /// <param name="serverIpAddress">IP Address of the server</param>		
        /// <returns>IPAddress of local machine</returns>
        public string GetClientNetworkName(string serverIPAddress)
        {
            return CtcUtility.GetClientNetworkName(serverIPAddress);
        }

        /// <summary>
        /// Disables the specified network connection.
        /// </summary>
        /// <param name="name">The name of the network connection.</param>
        public void DisableNetworkConnection(string name)
        {
            NetworkUtil.DisableNetworkConnection(name);
        }

        /// <summary>
        /// Enables the specified network connection.
        /// </summary>
        /// <param name="name">The name of the network connection.</param>
        public void EnableNetworkConnection(string name)
        {
            NetworkUtil.EnableNetworkConnection(name);
        }

        /// <summary>
        /// Reboot machine
        /// </summary>
        /// <param name="name">The name of the network connection.</param>
        public void RebootMachine(IPAddress address)
        {
            //Utility.RebootMachine(address);
            Process.Start("shutdown", "/r /t 0");
        }

        #endregion

        #endregion
    }
}
