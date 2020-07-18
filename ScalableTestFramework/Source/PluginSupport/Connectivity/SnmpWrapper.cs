using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Wireless;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    /// <summary>
    /// Represents set of operations with SNMP OIDs
    /// </summary>
    public sealed class SnmpWrapper
    {
        /// <summary>
        /// SNMP object
        /// </summary>
        private Snmp _snmplib;

        /// <summary>
        /// Singleton class object
        /// </summary>
        static readonly SnmpWrapper _instance = new SnmpWrapper();

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SnmpWrapper"/> class.
        /// </summary>
        public SnmpWrapper()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the SNMP wrapper using the parameter
        /// </summary>
        /// <param name="deviceAddress">deviceAddress</param>        
        public void Create(string deviceAddress)
        {
            _snmplib = new Snmp(deviceAddress);
        }

        public void SetCommunityName(string communityName)
        {
            TraceFactory.Logger.Info("Setting SNMP community Name: {0}.".FormatWith(communityName));
            _snmplib.CommunityName = communityName;
        }

        /// <summary>
        /// Gets the singleton object
        /// </summary>
        /// <returns>SNMP Wrapper singleton object</returns>
        public static SnmpWrapper Instance()
        {
            return _instance;
        }

        /// <summary>
        /// Enable/ disable SLP option through SNMP     
        /// </summary>        
        /// <param name="option">true to enable, false to disable</param>
        /// <returns>true if option set successfully, false otherwise</returns>
        public bool SetSLP(bool option)
        {
            try
            {
                string oidslp = "1.3.6.1.4.1.11.2.4.3.7.21.0";
                TraceFactory.Logger.Debug("SLP OID: {0} used to set the SLP option through SNMP".FormatWith(oidslp));

                if (option)
                {
                    TraceFactory.Logger.Debug("Enabling SLP Option through SNMP OID");
                    _snmplib.Set(oidslp, 1);
                }
                else
                {
                    TraceFactory.Logger.Debug("Disabling SLP Option through SNMP OID");
                    _snmplib.Set(oidslp, 0);
                }

                TraceFactory.Logger.Info("SLP Option is set to {0}".FormatWith(option));
            }
            catch (Exception slpSNMPException)
            {
                TraceFactory.Logger.Info(slpSNMPException.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Enable/ disable Bonjour option through SNMP     
        /// </summary>        
        /// <param name="option">true to enable, false to disable</param>
        /// <returns>true if option set successfully, false otherwise</returns>
        public bool SetBonjour(bool option)
        {
            try
            {
                string oidslp = "1.3.6.1.4.1.11.2.4.3.7.29.0";
                TraceFactory.Logger.Debug("Bonjour OID: {0} used to set the Bonjour option through SNMP".FormatWith(oidslp));

                if (option)
                {
                    TraceFactory.Logger.Debug("Enabling Bonjour Option through SNMP OID");
                    _snmplib.Set(oidslp, 1);
                }
                else
                {
                    TraceFactory.Logger.Debug("Disabling Bonjour Option through SNMP OID");
                    _snmplib.Set(oidslp, 0);
                }

                TraceFactory.Logger.Info("Bonjour Option is set to {0}".FormatWith(option));
            }
            catch (Exception bonjourSNMPException)
            {
                TraceFactory.Logger.Info(bonjourSNMPException.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Enable/ disable SLP option through SNMP     
        /// </summary>        
        /// <param name="option">true to enable, false to disable</param>
        /// <returns>true if option set successfully, false otherwise</returns>
        public bool SetSLPClientMode(bool option)
        {
            try
            {
                string oidslpclientmode = "1.3.6.1.4.1.11.2.4.3.7.43.0";
                TraceFactory.Logger.Debug("SLP Client Mode OID: {0} used to set the SLP option through SNMP".FormatWith(oidslpclientmode));

                if (option)
                {
                    TraceFactory.Logger.Debug("Enabling SLP Client Mode Option through SNMP OID");
                    _snmplib.Set(oidslpclientmode, 1);
                }
                else
                {
                    TraceFactory.Logger.Debug("Disabling SLP Client Mode Option through SNMP OID");
                    _snmplib.Set(oidslpclientmode, 0);
                }

                TraceFactory.Logger.Info("SLP Client Mode Option is set to {0}".FormatWith(option));
            }
            catch (Exception slpSNMPException)
            {
                TraceFactory.Logger.Info(slpSNMPException.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Set Bonjour Service Name
        /// </summary>
        /// <param name="bonjourServiceName">Bonjour Service Name to be set</param>
        /// <returns>true if set successfully, false otherwise</returns>
        public bool SetBonjourServiceName(string bonjourServiceName)
        {

            try
            {
                string oidBonjourServiceName = "1.3.6.1.4.1.11.2.4.3.5.44.0";
                _snmplib.Set(oidBonjourServiceName, bonjourServiceName);
                TraceFactory.Logger.Info("Successfully set Bonjour Service Name to {0}".FormatWith(bonjourServiceName));
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set the Bonjour Service Name through SNMP");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }

            return bonjourServiceName.Equals(GetBonjourServiceName());
        }

        /// <summary>
        /// Gets the Bonjour Service Name.
        /// </summary>
        /// <returns>The Bonjour Service Name</returns>
        public string GetBonjourServiceName()
        {
            try
            {

                string oidBonjourServiceName = "1.3.6.1.4.1.11.2.4.3.5.44.0";
                string bonjourServiceName = _snmplib.Get(oidBonjourServiceName);
                return bonjourServiceName;
            }
            catch (Exception bonjourServiceNameSNMPException)
            {
                TraceFactory.Logger.Info("Failed to fetch the Bonjour Service Name through SNMP");
                TraceFactory.Logger.Debug(bonjourServiceNameSNMPException.Message);
                return "false";
            }
        }


        /// <summary>
        /// Set Config Precedence    
        /// </summary>        
        /// <param name="order">order on which config precedence has to set</param>
        public bool SetConfigPrecedence(string order)
        {
            try
            {
                string oIDconfigprecedence = "1.3.6.1.4.1.11.2.4.3.5.59.0";
                _snmplib.Set(oIDconfigprecedence, order);
                TraceFactory.Logger.Info("Successfully configured the config precedence through SNMP");
                return true;
            }
            catch (Exception slpSNMPException)
            {
                TraceFactory.Logger.Info("Failed to set config precedence order through SNMP");
                TraceFactory.Logger.Debug(slpSNMPException.Message);
                return false;
            }
        }

        /// <summary>
        /// Get HostName from the printer
        /// </summary>                
        public string GetHostName()
        {
            try
            {
                string oIDhostname = "1.3.6.1.2.1.1.5.0";
                return _snmplib.Get(oIDhostname);
            }
            catch (Exception hostSNMPException)
            {
                TraceFactory.Logger.Info("Failed to fetch the hostname through SNMP");
                TraceFactory.Logger.Debug(hostSNMPException.Message);
                return "false";
            }
        }

        /// <summary>
        /// GetLease Time from the printer
        /// </summary>                
        public string GetLeaseTime()
        {
            try
            {
                string oIDlease = "1.3.6.1.4.1.11.2.4.3.6.5.0";
                // TODO : return lease time in decimal.
                return _snmplib.Get(oIDlease);
            }
            catch (Exception leaseSNMPException)
            {
                TraceFactory.Logger.Info("Failed to fetch the Lease time configured on the Printer through SNMP");
                TraceFactory.Logger.Info("Please Check manually whether you are able to retrieve the lease time from the printer");
                TraceFactory.Logger.Debug(leaseSNMPException.Message);
                return "false";
            }
        }

        /// <summary>
        /// Get Domain Name from the printer
        /// </summary>                
        public string GetDomainName()
        {
            try
            {
                string oIDdomainname = "1.3.6.1.4.1.11.2.4.3.5.16.0";
                return _snmplib.Get(oIDdomainname);
            }
            catch (Exception domainSNMPException)
            {
                TraceFactory.Logger.Info("Failed to fetch the domain name through SNMP");
                TraceFactory.Logger.Debug(domainSNMPException.Message);
                return "false";
            }
        }
        /// <summary>
        /// Get Config precedence
        /// </summary>                
        public string GetConfigPrecedence()
        {
            try
            {
                string oIDconfigprecedence = "1.3.6.1.4.1.11.2.4.3.5.59.0";
                return _snmplib.Get(oIDconfigprecedence);
            }
            catch (Exception configSNMPException)
            {
                TraceFactory.Logger.Info("Failed to fetch the config precedence through SNMP");
                TraceFactory.Logger.Debug(configSNMPException.Message);
                return "false";
            }
        }

        /// Summary
        /// Sets Web Proxy
        /// </Summary>
        /// <param name = "connectivityTest" >< see cref="ConnectivityTests"/></param>
        public bool SetWebProxy(WebProxyConfigurationDetails configDetails)
        {
            TraceFactory.Logger.Info("Configuring Web Proxy through SNMP using: {0}".FormatWith(configDetails));
            try
            {
                string oIDProxy = "1.3.6.1.4.1.11.2.4.3.7.39.0";
                string oIDcURL = "1.3.6.1.4.1.11.2.4.3.18.20.0";
                string oIDIPAddress = "1.3.6.1.4.1.11.2.4.3.18.12.0";
                string oIDPort = "1.3.6.1.4.1.11.2.4.3.18.13.0";
                string oIDUserName = "1.3.6.1.4.1.11.2.4.3.18.14.0";
                string oIDPassword = "1.3.6.1.4.1.11.2.4.3.18.15.0";
                string oIDAuthType = "1.3.6.1.4.1.11.2.4.3.18.21.0";
                _snmplib.Set(oIDProxy, (int)configDetails.ProxyType);
                Thread.Sleep(TimeSpan.FromSeconds(20));
                if (configDetails.ProxyType == WebProxyType.Curl)
                {
                    _snmplib.Set(oIDcURL, configDetails.cURL);
                    Thread.Sleep(TimeSpan.FromSeconds(20));
                }
                else
                {
                    if (configDetails.ProxyType == WebProxyType.Manual)
                    {
                        _snmplib.Set(oIDIPAddress, configDetails.IPAddress);
                        Thread.Sleep(TimeSpan.FromSeconds(20));
                        _snmplib.Set(oIDPort, configDetails.PortNo);
                        Thread.Sleep(TimeSpan.FromSeconds(20));
                        _snmplib.Set(oIDUserName, configDetails.UserName);
                        Thread.Sleep(TimeSpan.FromSeconds(20));
                        _snmplib.Set(oIDPassword, configDetails.Password);
                        Thread.Sleep(TimeSpan.FromSeconds(20));

                        _snmplib.Set(oIDAuthType, (int)configDetails.AuthType);
                    }
                }
                if (GetWebProxy().Equals(configDetails))
                {
                    TraceFactory.Logger.Info("Successfully configured Web Proxy through SNMP using: {0}".FormatWith(configDetails.ProxyType));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to configure Web Proxy through SNMP using: {0}".FormatWith(configDetails.ProxyType));
                    return false;
                }
            }
            catch (Exception slpSNMPException)
            {
                TraceFactory.Logger.Info("Failed to configure Web Proxy through SNMP");
                TraceFactory.Logger.Debug(slpSNMPException.Message);
                return false;
            }
        }
        /// Summary
        /// Gets Web Proxy
        /// </Summary>
        /// <param name = "connectivityTest" >< see cref="ConnectivityTests"/></param>
        public WebProxyConfigurationDetails GetWebProxy()
        {
            try
            {
                string oIDproxy = "1.3.6.1.4.1.11.2.4.3.7.39.0";
                string oIDcURL = "1.3.6.1.4.1.11.2.4.3.18.20.0";
                string oIDIPAddress = "1.3.6.1.4.1.11.2.4.3.18.12.0";
                string oIDPort = "1.3.6.1.4.1.11.2.4.3.18.13.0";
                string oIDUserName = "1.3.6.1.4.1.11.2.4.3.18.14.0";
                string oIDAuthType = "1.3.6.1.4.1.11.2.4.3.18.21.0";
                WebProxyConfigurationDetails configuredDetails = new WebProxyConfigurationDetails();
                if (_snmplib.Get(oIDproxy) == ((int)WebProxyType.Auto).ToString())
                {
                    configuredDetails.ProxyType = WebProxyType.Auto;
                }
                else if (_snmplib.Get(oIDproxy) == "2")
                {
                    configuredDetails.ProxyType = WebProxyType.Curl;
                }
                else if (_snmplib.Get(oIDproxy) == "3")
                {
                    configuredDetails.ProxyType = WebProxyType.Manual;
                }
                else
                {
                    configuredDetails.ProxyType = WebProxyType.Disable;
                }
                configuredDetails.IPAddress = _snmplib.Get(oIDIPAddress);
                configuredDetails.PortNo = Convert.ToInt32(_snmplib.Get(oIDPort));
                configuredDetails.UserName = _snmplib.Get(oIDUserName);

                configuredDetails.AuthType = (WebProxyAuthType)(Convert.ToInt32(_snmplib.Get(oIDAuthType)));

                configuredDetails.cURL = _snmplib.Get(oIDcURL);
                TraceFactory.Logger.Debug("Web Proxy Parameters Configured: {0}".FormatWith(configuredDetails.ToString()));
                return configuredDetails;
            }
            catch (Exception slpSNMPException)
            {
                TraceFactory.Logger.Info("Failed to configure Web Proxy through SNMP");
                TraceFactory.Logger.Debug(slpSNMPException.Message);
                return null;
            }
        }

        /// <summary>
        /// Configure Wireless     
        /// </summary>        
        /// <param name="SSIDValue">SSID created on the access point</param>
        /// <param name="protocolTypeValue">WPA/WPA2/No security/WEP</param>
        /// <param name="encryptionValue">TKIP/AES/Auto/128 bit hex/64 bit hex</param>
        ///  For WPA ex: ConfigureWireless("HP_WPA_WPA_AES", "WPA","WPA-2/WPA2-3/Auto-4")
        ///              ConfigureWireless("HP_WPA_WPA_AES", "WEP","shared-2/Open-1")
        ///  <param name="key">Value to encrypt or decrypt pass phrase</param>
        /// <summary>
        /// Gets the configured IP Config method
        /// </summary>
        /// <returns>the IPConfig Method</returns>
        public IPConfigMethod GetIPConfigMethod()
        {
            try
            {
                string oIDIpConfigMethod = "1.3.6.1.4.1.11.2.4.3.5.1.0";
                int configMethod = int.Parse(_snmplib.Get(oIDIpConfigMethod).ToString(), CultureInfo.CurrentCulture);

                return IPConfigMethodUtils.GetIPConfigMethod(configMethod.ToString(CultureInfo.CurrentCulture));

            }
            catch (Exception configSNMPException)
            {
                TraceFactory.Logger.Info("Failed to fetch the IP Config Method through SNMP");
                TraceFactory.Logger.Debug(configSNMPException.Message);
                return IPConfigMethod.None;
            }
        }

        /// <summary>
        /// Gets the manual Ipv6 Address configured on the printer
        /// </summary>
        /// <returns>Manual IPv6 Address</returns>
        public string GetManualIpv6Address()
        {
            string oIDManualIpv6Address = "1.3.6.1.4.1.11.2.4.3.5.54.0";
            string manualIpv6Address = _snmplib.Get(oIDManualIpv6Address);
            return manualIpv6Address;
        }

        /// <summary>
        /// Gets the DHCPv6(IPv6 State full) address configured on the printer.
        /// </summary>
        /// <returns>The DHCPv6(IPv6 State full) address.</returns>
        public IPAddress GetIPv6StateFullAddress()
        {
            string oidDhcpv6Address = "1.3.6.1.4.1.11.2.4.3.5.53.1.3.7";
            return IPAddress.Parse(_snmplib.Get(oidDhcpv6Address));
        }

        /// <summary>
        /// Gets the primary DNS Server IP Address.
        /// </summary>
        /// <returns>The primary DNS Server IP Address.</returns>
        public string GetPrimaryDnsServer()
        {
            // TODO : Add the OID 
            string oIDPrimaryDnsServer = "1.3.6.1.4.1.11.2.4.3.5.21.0";
            string primaryDnsServer = _snmplib.Get(oIDPrimaryDnsServer);
            return primaryDnsServer;
        }

        /// <summary>
        /// Gets the secondary DNS Server IP Address.
        /// </summary>
        /// <returns>The secondary DNS Server IP Address.</returns>
        public string GetSecondaryDnsServer()
        {
            // TODO : Add the OID 
            string oIDSecondaryDnsServer = "1.3.6.1.4.1.11.2.4.3.5.47.0";
            string secondaryDnsServer = _snmplib.Get(oIDSecondaryDnsServer);
            return secondaryDnsServer;
        }

        /// <summary>
        /// Set WS Discovery Option
        /// </summary>
        /// <param name="option">true to enable, false to disable</param>
        /// <returns>true if option set successfully, false if fails</returns>
        public bool SetWSDiscovery(bool option)
        {
            string wsDiscoveryOID = "1.3.6.1.4.1.11.2.4.3.7.36.0";

            TraceFactory.Logger.Debug("OID for WS Discovery: {0}".FormatWith(wsDiscoveryOID));

            int setValue = option == true ? 1 : 0;

            try
            {
                _snmplib.Set(wsDiscoveryOID, setValue);
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Unable to set WS Discovery option.");
                TraceFactory.Logger.Debug("Exception details: {0}".FormatWith(defaultException.Message));
                return false;
            }

			TraceFactory.Logger.Info("WS Discovery Option is {0}. from SNMP OID".FormatWith(option == true ? "checked" : "uncheck"));
			return true;
		}

        /// <summary>
        /// Set IP Configuration method
        /// </summary>
        /// <param name="configMethod"><see cref=" IPConfigMethod"/></param>
        /// <returns>true is config method is set successfully, false otherwise</returns>
        public bool SetIPConfigMethod(IPConfigMethod configMethod)
        {
            string ipConfigMethodOID = "1.3.6.1.4.1.11.2.4.3.5.1.0";

            TraceFactory.Logger.Debug("OID for IP configuration method: {0}".FormatWith(ipConfigMethodOID));

            /* IPConfiguration Value
			 
				1: manual-one(1)
				2: bootp-two(2)
				3: manual-three(3)
				4: bootp-four(4)
				5: dhcp(5)
				6: not-configured(6)
				7: default-config(7)
				8: rarp(8)
				9: read-only(9)
				10: auto-ip(10)
			 
			 */

            try
            {
                _snmplib.Set(ipConfigMethodOID, (int)configMethod);
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Unable to set IP Config method.");
                TraceFactory.Logger.Debug("Exception details: {0}".FormatWith(defaultException.Message));
                return false;
            }

			TraceFactory.Logger.Info("IP Config method set to {0} FRom SNMP OID".FormatWith(configMethod));
			return true;
		}

        /// <summary>
        /// Set Default IP Config method
        /// </summary>
        /// <param name="family"><see cref=" PrinterFamilies"/></param>
        /// <returns>true is config method is set successfully, false otherwise</returns>
        public bool SetDefaultIPConfigMethod(string family)
        {
            Printer.Printer printer = PrinterFactory.Create(family, _snmplib.Address);
            return SetIPConfigMethod(printer.DefaultIPConfigMethod);
        }

        /// <summary>
        /// Get Primary Wins Server IP Address
        /// </summary>
        /// <returns>Primary Wins IP Address</returns>
        public string GetPrimaryWinsServer()
        {
            // TODO : Add the OID 
            string oIDPrimaryWinsServer = "1.3.6.1.4.1.11.2.4.3.5.22.0";
            return _snmplib.Get(oIDPrimaryWinsServer).ToString();
        }

        /// <summary>
        /// Get Secondary Wins Server IP Address
        /// </summary>
        /// <returns>Secondary Wins IP Address</returns>
        public string GetSecondaryWinsServer()
        {
            // TODO : Add the OID 
            string oIDSecondaryWinsServer = "1.3.6.1.4.1.11.2.4.3.5.23.0";
            return _snmplib.Get(oIDSecondaryWinsServer).ToString();
        }

        /// <summary>
        /// Set Hostname
        /// </summary>
        /// <param name="hostName">Hostname to set</param>
        /// <returns>true if set successfully, false otherwise</returns>
        public bool SetHostName(string hostName)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                TraceFactory.Logger.Info("Invalid hostname.");
                return false;
            }

            try
            {
                // need to ask BJ about Octal values for set
                string oIDhostname = "1.3.6.1.2.1.1.5.0";
                _snmplib.Set(oIDhostname, hostName);
				TraceFactory.Logger.Info("Successfully set hostname to {0} from SNMP OID".FormatWith(hostName));
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set the hostname through SNMP");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }

            return hostName.Equals(GetHostName());
        }

        /// <summary>
        /// Set Domain name
        /// </summary>
        /// <param name="domainname">Domain name to set</param>
        /// <returns>true if set successfully, false otherwise</returns>
        public bool SetDomainName(string domainname)
        {
            if (string.IsNullOrEmpty(domainname))
            {
                TraceFactory.Logger.Info("Invalid domain name.");
                return false;
            }

            try
            {
                // was set as an octal value
                string oIDdomainname = "1.3.6.1.4.1.11.2.4.3.5.16.0";
                _snmplib.Set(oIDdomainname, domainname);
				TraceFactory.Logger.Info("Successfully set domain name to {0} From SNMP OID".FormatWith(domainname));
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set the domain name through SNMP");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }

            return domainname.Equals(GetDomainName());
        }

        /// <summary>
        /// Set Primary DNS Server
        /// </summary>
        /// <param name="primaryDnsServer">Primary DNS Server to set</param>
        /// <returns>true if set successfully, false otherwise</returns>
        public bool SetPrimaryDnsServer(string primaryDnsServer)
        {
            if (string.IsNullOrEmpty(primaryDnsServer))
            {
                TraceFactory.Logger.Info("Invalid primary DNS server.");
                return false;
            }

            try
            {
                string oIDprimaryDnsServer = "1.3.6.1.4.1.11.2.4.3.5.21.0";
                SnmpOidValue sov = new SnmpOidValue(oIDprimaryDnsServer, primaryDnsServer, 64);
                _snmplib.Set(sov);
                TraceFactory.Logger.Info("Successfully set primary DNS server to {0}".FormatWith(primaryDnsServer));
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set the primary DNS server through SNMP");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }

            return primaryDnsServer.Equals(GetPrimaryDnsServer());
        }

        /// <summary>
        /// Set Primary Wins Server
        /// </summary>
        /// <param name="primaryWinsServer">Primary Wins Server to set</param>
        /// <returns>true if set successfully, false otherwise</returns>
        public bool SetPrimaryWinsServer(string primaryWinsServer)
        {
            if (string.IsNullOrEmpty(primaryWinsServer))
            {
                TraceFactory.Logger.Info("Invalid primary wins server.");
                return false;
            }

            try
            {
                string oIDprimaryWinsServer = "1.3.6.1.4.1.11.2.4.3.5.22.0";
                SnmpOidValue sov = new SnmpOidValue(oIDprimaryWinsServer, primaryWinsServer, 64);
                _snmplib.Set(sov);
				TraceFactory.Logger.Info("Successfully set primary wins server to {0} From SNMP OID".FormatWith(primaryWinsServer));
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set the primary wins server through SNMP");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }

            return primaryWinsServer.Equals(GetPrimaryWinsServer());
        }

        /// <summary>
        /// Get Primary DnsV6 address
        /// </summary>
        /// <returns>Primary DnsV6 address</returns>
        public string GetPrimaryDnsV6Address()
        {
            // TODO : Add the OID 
            string oIDPrimaryDnsV6Addres = "1.3.6.1.4.1.11.2.4.3.5.56.0";
            return _snmplib.Get(oIDPrimaryDnsV6Addres).ToString();
        }

        /// <summary>
        /// Get Secondary DnsV6 address
        /// </summary>
        /// <returns>Secondary DnsV6 address</returns>
        public string GetSecondaryDnsV6Address()
        {
            // TODO : Add the OID 
            string oIDSecondaryDnsV6Addres = "1.3.6.1.4.1.11.2.4.3.5.57.0";
            return _snmplib.Get(oIDSecondaryDnsV6Addres).ToString();
        }

        // TODO : merge GetDomainSearchList() and GetDomainSearchLists

        /// <summary>
        /// Get Domain Search List
        /// </summary>
        /// <returns>Primary DnsV6 address</returns>
        public string GetDomainSearchList()
        {
            // TODO : Add the OID 
            string oIDDomainSearchList = "1.3.6.1.4.1.11.2.4.3.5.61.1.2.1";
            return _snmplib.Get(oIDDomainSearchList).ToString();
        }

        /// <summary>
        /// Get Domain Search List
        /// </summary>
        /// <returns>Primary DnsV6 address</returns>
        public List<string> GetDomainSearchLists()
        {
            List<string> domainDearchList = new List<string>();

            string oIDDnsSuffixCount = "1.3.6.1.4.1.11.2.4.3.5.60.0";
            int dnsSuffixCount = int.Parse(_snmplib.Get(oIDDnsSuffixCount).ToString(), CultureInfo.CurrentCulture);

            string dnsSearchList = "1.3.6.1.4.1.11.2.4.3.5.61.1.2.{0}";

            for (int i = 1; i <= dnsSuffixCount; i++)
            {
                // TODO : Add the OID 
                string oIDDomainSearchList = dnsSearchList.FormatWith(i);
                domainDearchList.Add(_snmplib.Get(oIDDomainSearchList).ToString());
            }

            return domainDearchList;
        }

        /// <summary>
        /// Set Primary Dnsv6 Server
        /// </summary>
        /// <param name="primaryDnsv6Server">Primary Dnsv6 Server to set</param>
        /// <returns>true if set successfully, false otherwise</returns>
        public bool SetPrimaryDnsv6Server(string primaryDnsv6Server)
        {
            if (string.IsNullOrEmpty(primaryDnsv6Server))
            {
                TraceFactory.Logger.Info("Invalid primary dnsv6 server.");
                return false;
            }

            try
            {
                string oIDprimaryDnsv6Server = "1.3.6.1.4.1.11.2.4.3.5.56.0";
                _snmplib.Set(oIDprimaryDnsv6Server, primaryDnsv6Server);
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set the primary dnsv6 server through SNMP");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
            TraceFactory.Logger.Info("Succesfully configured Primary DNSv6 Server Address from SNMP OID");
            return primaryDnsv6Server.Equals(GetPrimaryDnsV6Address());
        }

        /// <summary>
        /// Get v6 Domain name
        /// </summary>
        /// <returns>V6 Domain name</returns>
        public string Getv6DomainName()
        {
            // TODO : Add the OID 
            string oIDDomainSearchList = "1.3.6.1.4.1.11.2.4.3.5.49.0";
            return _snmplib.Get(oIDDomainSearchList);
        }

        /// <summary>
        /// Set v6 Domain name
        /// </summary>
        /// <param name="v6DomainName">Primary Wins Server to set</param>
        /// <returns>true if set successfully, false otherwise</returns>
        public bool Setv6DomainName(string v6DomainName)
        {
            if (string.IsNullOrEmpty(v6DomainName))
            {
                TraceFactory.Logger.Info("Invalid v6 domain name.");
                return false;
            }

            try
            {
                string oIDv6DomainName = "1.3.6.1.4.1.11.2.4.3.5.49.0";
                _snmplib.Set(oIDv6DomainName, v6DomainName);
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set the v6 domain name server through SNMP");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
            TraceFactory.Logger.Info("Successfully configured Domain name IPv6 from SNMP OID");
            return v6DomainName.Equals(Getv6DomainName());
        }

        /// <summary>
        /// Set the DDNS option through SNMP.
        /// </summary>
        /// <param name="option">True to enable, false to disable.</param>
        /// <returns>True if the value is set, else false.</returns>
        public bool SetDdns(bool option)
        {
            try
            {
                TraceFactory.Logger.Info("Setting DDNS option to {0} thru SNMP".FormatWith(option));

                // TODO: Get OID for DDNS
                string oIDDdns = "1.3.6.1.4.1.11.2.4.3.7.77.0";

                if (option)
                {
                    _snmplib.Set(oIDDdns, 1);
                }
                else
                {
                    _snmplib.Set(oIDDdns, 0);
                }

                if (GetDdns().Equals(option))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set DDNS through SNMP");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
        }

        /// <summary>
        /// Get the DDNS option through SNMP.
        /// </summary>
        /// <returns>True if the value is set, else false.</returns>
        public bool GetDdns()
        {
            try
            {
                // TODO: Get OID for DDNS
                string oIDDdns = "1.3.6.1.4.1.11.2.4.3.7.77.0";
                int value = int.Parse(_snmplib.Get(oIDDdns), CultureInfo.CurrentCulture);
                return value == 1 ? true : false;
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to get DDNS through SNMP");
                TraceFactory.Logger.Debug(defaultException.Message);
                throw defaultException;
            }
        }

        /// <summary>
        /// Enable/ disable Multicas IPv4 through SNMP     
        /// </summary>        
        /// <param name="option">true to enable, false to disable</param>
        /// <returns>true if option set successfully, false otherwise</returns>
        public bool SetMulticastIPv4(bool option)
        {
            try
            {
                string oIDMultiCastIPv4 = "1.3.6.1.4.1.11.2.4.3.7.30.0";
                TraceFactory.Logger.Debug("Multicast IPv4 OID: {0} used to set the Multicast IPv4 option through SNMP".FormatWith(oIDMultiCastIPv4));

                if (option)
                {
                    TraceFactory.Logger.Debug("Enabling Multicast IPv4 option through SNMP OID");
                    _snmplib.Set(oIDMultiCastIPv4, 1);
                }
                else
                {
                    TraceFactory.Logger.Debug("Disabling Multicast IPv4 option through SNMP OID");
                    _snmplib.Set(oIDMultiCastIPv4, 0);
                }

                if (GetMulticastIPv4().Equals(option))
                {
                    TraceFactory.Logger.Info("Successfully set Multicast IPv4 option to {0}".FormatWith(option));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to set Multicast IPv4 option to {0}".FormatWith(option));
                    return false;
                }

            }
            catch (Exception multiCastIPv4SNMPException)
            {
                TraceFactory.Logger.Info(multiCastIPv4SNMPException.Message);
                return false;
            }
        }

        /// <summary>
        /// Get the Multicast IPv4 through SNMP.
        /// </summary>
        /// <returns>True if the value is set, else false.</returns>
        public bool GetMulticastIPv4()
        {
            try
            {
                // TODO: Get OID for Multicast IPv4
                string oIDMultiCastIPv4 = "1.3.6.1.4.1.11.2.4.3.7.30.0";
                int value = int.Parse(_snmplib.Get(oIDMultiCastIPv4), CultureInfo.CurrentCulture);
                return value == 1 ? true : false;
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to get Multicast IPv4 through SNMP");
                TraceFactory.Logger.Debug(defaultException.Message);
                throw defaultException;
            }
        }

        /// <summary>
        /// Set Link Speed on the printer
        /// Note: Applicable only for TPS
        /// </summary>
        /// <param name="linkSpeed"><see cref=" PrinterLinkSpeed"/></param>
        /// <returns>true if set successfully, false otherwise</returns>
        public bool SetLinkSpeed(PrinterLinkSpeed linkSpeed)
        {
            if (PrinterLinkSpeed.Auto10T == linkSpeed)
            {
                TraceFactory.Logger.Info("Option {0} is not supported for setting through SNMP.".FormatWith(linkSpeed));
                return false;
            }

            TraceFactory.Logger.Info("Setting link speed to: {0}".FormatWith(linkSpeed));

            try
            {
                string oIDlinkSpeed = "1.3.6.1.4.1.11.2.4.3.5.35.0";
                _snmplib.Set(oIDlinkSpeed, (int)linkSpeed);
            }
            catch (Exception generalException)
            {
                TraceFactory.Logger.Info("Failed to set link speed.");
                TraceFactory.Logger.Debug(generalException.JoinAllErrorMessages());
            }
            TraceFactory.Logger.Info("Successfully configured Link Speed from SNMP OID");
            return linkSpeed.Equals(GetLinkSpeed());
        }

        /// <summary>
        /// Get <see cref=" PrinterLinkSpeed"/> value set on printer
        /// </summary>
        /// <returns><see cref=" PrinterLinkSpeed"/></returns>
        public PrinterLinkSpeed GetLinkSpeed()
        {
            string oIDlinkSpeed = "1.3.6.1.4.1.11.2.4.3.5.35.0";
            return (PrinterLinkSpeed)int.Parse(_snmplib.Get(oIDlinkSpeed), CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Set Bonjour Highest Priority on the printer
        /// </summary>
        /// <param name="bonjourService"><see cref="Bonjour Highest Priority"/></param>
        /// <returns>true if set successfully, false otherwise</returns>
        public bool SetBonjourHighestPriority(int value)
        {

            try
            {
                string oIDbonjourService = "1.3.6.1.4.1.11.2.4.3.5.45.0";
                _snmplib.Set(oIDbonjourService, value);
            }
            catch (Exception generalException)
            {
                TraceFactory.Logger.Info("Failed to set Bonjour Highest Priority.");
                TraceFactory.Logger.Debug(generalException.JoinAllErrorMessages());
            }
            Thread.Sleep(TimeSpan.FromSeconds(30));

            if (value == GetBonjourHighestPriority())
            {
                TraceFactory.Logger.Info("Successfully set the Bonjour Highest Service to: {0} from SNMP".FormatWith(value));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to set the Bonjour Highest Service to: {0} from SNMP".FormatWith(value));
                return false;
            }
        }

        /// <summary>
        /// Get <see cref="Bonjour Highest Priority"/> value set on printer
        /// </summary>
        /// <returns><see cref=" Bonjour Highest Priority"/></returns>
        public int GetBonjourHighestPriority()
        {
            string oIDbonjourService = "1.3.6.1.4.1.11.2.4.3.5.45.0";
            return int.Parse(_snmplib.Get(oIDbonjourService), CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Get Auto Negotiation value set on the printer
        /// Note: Applicable only for VEP
        /// </summary>
        /// <returns>Auto negotiation value</returns>
        public string GetAutonegotiation()
        {
            string oIDautoNegotiation = "1.3.6.1.4.1.11.2.4.3.1.12.1.2.9";
            return _snmplib.Get(oIDautoNegotiation);
        }

        /// <summary>
        /// Sets the InetAddressType value.
        /// </summary>
        /// <param name="value">The value to be set.</param>
        /// <returns>true if the value is set, else false.</returns>
        public bool SetInetAddressType(int value)
        {
            try
            {
                string inetAddressType = "1.3.6.1.4.1.11.2.4.3.22.1.2.1.3.1";

                // Setting InetAddressType
                _snmplib.Set(inetAddressType, value);
                TraceFactory.Logger.Info("Successfully set InetAddressType to {0}".FormatWith(16));
                return true;
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set the InetAddressType through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
        }

        /// <summary>
        /// Sets the InetAddressType value.
        /// </summary>
        /// <returns>The current value of InetAddressType.</returns>
        public int GetInetAddressType()
        {
            try
            {
                string inetAddressType = "1.3.6.1.4.1.11.2.4.3.22.1.2.1.3.1";
                return int.Parse(_snmplib.Get(inetAddressType), CultureInfo.CurrentCulture);
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to get the InetAddressType through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return 0;
            }
        }

        /// <summary>
        /// Sets the InetTrapAddress to the specified address.
        /// </summary>
        /// <param name="address">The value to be set.</param>
        /// <returns>True if the value is set, else false.</returns>
        public bool SetInetTrapAddress(string address)
        {
            try
            {
                string inetTrapAddress = "1.3.6.1.4.1.11.2.4.3.22.1.2.1.4.1";
                _snmplib.Set(inetTrapAddress, address);
                TraceFactory.Logger.Info("Successfully set SetInetTrapAddress to: {0}".FormatWith(address));
                return true;

            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set the InetTrapAddress through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
        }

        /// <summary>
        /// Get the value of InetTrapAddress.
        /// </summary>
        /// <returns>The current value of InetTrapAddress.</returns>
        public string GetInetTrapAddress()
        {
            try
            {
                string inetTrapAddress = "1.3.6.1.4.1.11.2.4.3.22.1.2.1.4.1";
                return _snmplib.Get(inetTrapAddress);
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set the InetTrapAddress through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// Sets the InetTrapRowStatus to the specified value.
        /// </summary>
        /// <param name="value">Value to be set.</param>
        /// <returns>True if the value is set, else false.</returns>
        public bool SetInetTrapRowStatus(int value)
        {
            try
            {
                string inetTrapRowstatus = "1.3.6.1.4.1.11.2.4.3.22.1.2.1.2.1";
                _snmplib.Set(inetTrapRowstatus, value);
                TraceFactory.Logger.Info("Successfully set SetInetTrapRowStatus to: {0}".FormatWith(value));
                return true;

            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set the InetTrapRowstatus through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
        }

        /// <summary>
        /// Gets the InetTrapRowStatus value.
        /// </summary>
        /// <returns>The value of InetTrapRowStatus.</returns>
        public int GetInetTrapRowStatus()
        {
            try
            {
                string inetTrapRowstatus = "1.3.6.1.4.1.11.2.4.3.22.1.2.1.2.1";
                return int.Parse(_snmplib.Get(inetTrapRowstatus), CultureInfo.CurrentCulture);
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to get the InetTrapRowstatus through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return 0;
            }
        }

        /// <summary>
        /// Set the InetTrapTest to the specified value.
        /// </summary>
        /// <param name="value">Value to be set.</param>
        /// <returns>True if the value is set, else false.</returns>
        public bool SetInetTrapTest(int value)
        {
            try
            {
                string inetTrapTest = "1.3.6.1.4.1.11.2.4.3.22.1.3.0";
                _snmplib.Set(inetTrapTest, value);
                TraceFactory.Logger.Info("Successfully set InetTraptest to: {0}".FormatWith(value));
                return true;
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set the InetTrapTest through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
        }

        /// <summary>
        /// Gets the current value of InetTrapTest.
        /// </summary>
        /// <returns>the value of InetTrapTest.</returns>
        public int GetInetTrapTest()
        {
            try
            {
                string inetTrapTest = "1.3.6.1.4.1.11.2.4.3.22.1.3.0";
                return int.Parse(_snmplib.Get(inetTrapTest), CultureInfo.CurrentCulture);
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to get the InetTrapTest through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return 0;
            }
        }

        /// <summary>
        /// Get Tcp Ip Timeout
        /// Note: Applicable only to VEP
        /// </summary>
        /// <returns>Tcp/ Ip Timeout</returns>
        public int GetTcpIpTimeout()
        {
            // TODO : Add the OID 
            string oIDTcpIPTimeout = "1.3.6.1.4.1.11.2.4.3.5.10.0";
            return Convert.ToInt16(_snmplib.Get(oIDTcpIPTimeout).ToString());
        }

        /// <summary>
        /// Get RFC Option Value
        /// </summary>
        /// <returns>true if enabled, false otherwise</returns>
        public bool GetRFCOption()
        {
            string oIDRfcOption = "1.3.6.1.4.1.11.2.4.3.16.4.0";
            return Convert.ToBoolean(Convert.ToInt16(_snmplib.Get(oIDRfcOption).ToString()));
        }


        /// <summary>
        /// Set the FIPS option to enable/disable state.
        /// </summary>
        /// <param name="value">Value to be set.</param>
        /// <returns>True if the value is set, else false.</returns>
        public bool SetFIPS(bool value)
        {
            try
            {
                string oIDFIPS = "1.3.6.1.4.1.11.2.4.3.20.59.0";
                _snmplib.Set(oIDFIPS, value ? 1 : 0);
                TraceFactory.Logger.Info("Successfully set FIPS to: {0} through SNMP".FormatWith(value));
                return true;
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set the FIPS through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
        }

        /// <summary>
        /// Reset Security.
        /// </summary>
        /// <param name="value">Value to be set.</param>
        /// <returns>True if the value is set, else false.</returns>
        public bool ResetSecurity(int value)
        {
            try
            {
                string oIDResetSecurity = "1.3.6.1.4.1.11.2.4.3.20.24.0";
                _snmplib.Set(oIDResetSecurity, value);
                TraceFactory.Logger.Info("Successfully reset the security through SNMP");
                return true;
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to reset the security through SNMP");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
        }

        /// <summary>
        /// Gets admin password status.
        /// </summary>
        /// <returns>True if the password is set.</returns>
        public bool GetAdminPasswordStatus()
        {
            try
            {
                string oidSetPassword = "1.3.6.1.4.1.11.2.4.3.5.29.0";

                if (_snmplib.Get(oidSetPassword).EqualsIgnoreCase("SET"))
                {
					TraceFactory.Logger.Info("Admin password is set from SNMP.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Admin password is not set.");
                    return false;
                }
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to retrieve the admin password status through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
        }

        /// <summary>
        /// Sets admin password.
        /// </summary>
        /// <param name="password">The password to be set.</param>
        /// <returns>True if the operation is successful.</returns>
        public bool SetAdminPassword(string password)
        {
            try
            {
                string oidSetPassword = "1.3.6.1.4.1.11.2.4.3.5.29.0";
                _snmplib.Set(oidSetPassword, password);
                if (GetAdminPasswordStatus())
                {
                    TraceFactory.Logger.Info("Successfully set Admin password through SNMP.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to set Admin password through SNMP.");
                    return false;
                }
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set Admin password through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
        }

        /// <summary>
        /// Sets admin password as the SNMP set community name.
        /// </summary>
        /// <returns>True if the operation is successful.</returns>
        public bool SetAdminPasswordAsSetCommunityName()
        {
            try
            {
                string oidSetPassword = "1.3.6.1.4.1.11.2.4.3.5.66.0";
                _snmplib.Set(oidSetPassword, 1);

                TraceFactory.Logger.Info("Successfully set admin password as SNMP set community name through SNMP.");
                return true;
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set admin password as SNMP set community name through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
        }

        /// <summary>
        /// Sets IPP through SNMP.
        /// </summary>
        /// <param name="value">0 - disable both IPP and IPPS.
        ///						1 - enable both IPP and IPPS.
        ///						2 - enable IPPS only.
        ///						3 - enable IPP only.</param>
        /// <returns>True if the value is set.</returns>
        public bool SetIPP(int value)
        {
            string message = (value == 0 | value == 1) ? "{0} IPP and IPPS".FormatWith(value == 0 ? "Disable" : "Enable") : (value == 2) ? "Enable IPPS" : "Enable IPP";
            try
            {
                TraceFactory.Logger.Info(message);
                string oidIPP = "1.3.6.1.4.1.11.2.4.3.5.18.0";
                _snmplib.Set(oidIPP, value);

                if (GetIPP() == value)
                {
                    TraceFactory.Logger.Info("Succeeded to {0} through SNMP.".FormatWith(message));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to {0} through SNMP.".FormatWith(message));
                    return false;
                }
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to {0} through SNMP.".FormatWith(message));
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
        }

        /// <summary>
        /// Gets the IPP and IPPS status in text format.
        /// </summary>
        /// <returns>The status of IPP and IPPS.</returns>
        public string GetIPPStatus()
        {
            try
            {
                TraceFactory.Logger.Info("Getting the status of IPP and IPPS through SNMP.");
                string oidIPP = "1.3.6.1.4.1.11.2.4.3.5.18.0";

                int value = int.Parse(_snmplib.Get(oidIPP), CultureInfo.CurrentCulture);

                return ((value == 0 | value == 1) ? "both IPP and IPPS are {0}".FormatWith(value == 0 ? "Disabled" : "Enabled") : (value == 2) ? "Only IPPS is enabled" : "Only IPP is enabled");
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to get the status of IPP and IPPS through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the IPP status
        /// </summary>
        /// <returns>The value indicating the IPP and IPPS status.
        /// 0 - both IPP and IPPS are disabled
        ///	1 - enabled both IPP and IPPS.
        ///	2 - Only IPPS is enabled.
        ///	3 - Only IPP is enabled.</returns>
        public int GetIPP()
        {
            try
            {
                TraceFactory.Logger.Info("Getting the status of IPP and IPPS through SNMP.");
                string oidIPP = "1.3.6.1.4.1.11.2.4.3.5.18.0";

                return int.Parse(_snmplib.Get(oidIPP), CultureInfo.CurrentCulture);
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to get the status of IPP and IPPS through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                throw defaultException;
            }
        }


        /// <summary>
        /// Gets the WIFI Direct SSID Name.
        /// </summary>
        /// <returns>The WIFI Direct SSID Name</returns>
        public string GetWIFISSID()
        {
            try
            {
                string oidSSIDNameSuffix = "1.3.6.1.4.1.11.2.4.3.5.87.0";
                string ssidNameSuffix = _snmplib.Get(oidSSIDNameSuffix);
                string oidSSIDNamePrefix = "1.3.6.1.4.1.11.2.4.3.5.86.0";
                string ssidNamePrefix = _snmplib.Get(oidSSIDNamePrefix);
                string wifiSSIDName = ssidNamePrefix + ssidNameSuffix;
                return wifiSSIDName;
            }
            catch (Exception wifi)
            {
                TraceFactory.Logger.Info(wifi.Message);
                return "false";
            }
        }

		/// <summary>
		/// Gets the Mac Address of the Printer.
		/// </summary>
		/// <returns>The Mac Address</returns>
		public string GetMacAddress()
		{
            SnmpOidValue sov = _snmplib.GetRaw(ObjectId.MacAddress);
            var publicKey = BitConverter.ToString(sov.ToBytes()).Replace("-", string.Empty);
            return _snmplib.Get(ObjectId.MacAddress);
		}

        /// <summary>
        /// Gets the WIFI Direct SSID Name-Suffix.
        /// </summary>
        /// <returns>The WIFI Direct SSID Name</returns>
        public string GetWIFISSIDSuffix()
        {
            try
            {
                string oidSSIDNameSuffix = "1.3.6.1.4.1.11.2.4.3.5.87.0";
                return _snmplib.Get(oidSSIDNameSuffix);
            }
            catch (Exception wifi)
            {
                TraceFactory.Logger.Info(wifi.Message);
                return "false";
            }
        }

        /// <summary>
        /// Gets the WIFI Direct SSID Name-Prefix.
        /// </summary>
        /// <returns>The WIFI Direct SSID Name</returns>
        public string GetWIFISSIDPrefix()
        {
            try
            {
                string oidSSIDNamePrefix = "1.3.6.1.4.1.11.2.4.3.5.86.0";
                return _snmplib.Get(oidSSIDNamePrefix);
            }
            catch (Exception wifi)
            {
                TraceFactory.Logger.Info(wifi.Message);
                return "false";
            }
        }

        /// <summary>
        /// Sets the WIFI Direct SSID Name-Prefix.
        /// </summary>
        /// param name="SSID"
        public bool SetWIFISSIDPrefix(string SSID)
        {
            try
            {
                string oidSSIDNamePrefix = "1.3.6.1.4.1.11.2.4.3.5.86.0";
                _snmplib.Set(oidSSIDNamePrefix, SSID);

                TraceFactory.Logger.Info("Successfully set SSID Prefix through SNMP.");
                return true;
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set SSID Prefix through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
        }

        /// <summary>
        /// Sets the WIFI Direct Mode
        /// </summary>
        /// param name="SSID"
        public bool SetWIFIDirectMode(bool value)
        {
            int setOption = value ? 1 : 0;
            try
            {
                string oidWiFiMode = "1.3.6.1.4.1.11.2.4.3.7.74.0";
                _snmplib.Set(oidWiFiMode, setOption);

                TraceFactory.Logger.Info("Successfully set the WiFi Direct Mode");
                return true;
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set the WiFi Direct Mode");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
        }


        /// <summary>
        /// Gets the WIFI Direct Channel number.
        /// </summary>
        /// <returns>The WIFI Direct Channel Number</returns>
        public string GetWIFIChannel()
        {
            try
            {
                string oidWiFiChannel = "1.3.6.1.4.1.11.2.4.3.5.85.0";
                return _snmplib.Get(oidWiFiChannel);
            }
            catch (Exception wifi)
            {
                TraceFactory.Logger.Info(wifi.Message);
                return "false";
            }
        }

        /// <summary>
        /// Gets printer name
        /// </summary>
        /// <returns>The Printer Name</returns>
        public string GetPrinterName()
        {
            try
            {
                string oidWiFiPrinterName = "1.3.6.1.2.1.25.3.2.1.3.1";
                return _snmplib.Get(oidWiFiPrinterName);
            }
            catch (Exception wifi)
            {
                TraceFactory.Logger.Info(wifi.Message);
                return "false";
            }
        }

        /// <summary>
        /// Set WiFi Direct Channel Number
        /// </summary>
        /// <returns>True if the operation is successful.</returns>
        public bool SetWiFiChannel(int channel)
        {
            try
            {
                string oidSetChannel = "1.3.6.1.4.1.11.2.4.3.5.85.0";
                _snmplib.Set(oidSetChannel, channel);

                TraceFactory.Logger.Info("Successfully set wifi direct channel through SNMP.");
                return true;
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set wifi direct channel through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
        }

        #region Wireless

        /// <summary>
        /// Gets the Security public key to configure wireless.
        /// </summary>
        /// <returns>The public key</returns>
        public string GetSecurityPublicKey()
        {
            TraceFactory.Logger.Info("Getting Security Public key");
            SnmpOidValue sov = _snmplib.GetRaw(ObjectId.PublicKey);
            var publicKey = BitConverter.ToString(sov.ToBytes()).Replace("-", string.Empty).Remove(0, 4);
            TraceFactory.Logger.Info($"Security public key: {publicKey}");
            return publicKey;
        }

		public bool ConfigureWireless(WirelessSettings wirelessSettings, WirelessSecuritySettings securitySettings)
		{
			bool result = false;
			switch (securitySettings.WirelessAuthentication)
			{
				case WirelessAuthentications.NoSecurity:
					return false;
				case WirelessAuthentications.Wpa:
					result = ConfigureWPAPersonal(wirelessSettings, securitySettings.WPAPersonalSecurity);
					break;
				case WirelessAuthentications.Wep:
					result = ConfigureWepPersonal(wirelessSettings, securitySettings.WEPPersonalSecurity);
					break;
			}
			return result;
		}

        private bool ConfigureWPAPersonal(WirelessSettings wirelessSettings, WPAPersonalSettings wpaSettings)
        {
            TraceFactory.Logger.Info($"Configuring WPA Personal with basic settings: {wirelessSettings.ToString()} and security settings: {wpaSettings.ToString()}");

            try
            {

                string publicKey = GetSecurityPublicKey();


                //values.Add(new SnmpOidValue(ObjectId.SSIDName, Encoding.ASCII.GetString(Utility.Encrypt(publicKey, wirelessSettings.SsidName))));
                //values.Add(new SnmpOidValue(ObjectId.WPAPassphrase, string.Format("", Utility.Encrypt(publicKey, wpaSettings.passphrase))));
                //values.Add(new SnmpOidValue(ObjectId.WPAVersion, (int)wpaSettings.Version));
                //values.Add(new SnmpOidValue(ObjectId.WPAEncryption, (int)wpaSettings.Encryption));

                //_snmplib.Set(values);
                //Variable ssidName = new Variable(new ObjectIdentifier(ObjectId.SSIDName), new OctetString(Utility.Encrypt(publicKey, wirelessSettings.SsidName)));
                //Variable passphrase = new Variable(new ObjectIdentifier(ObjectId.WPAPassphrase), new OctetString(Utility.Encrypt(publicKey, wpaSettings.passphrase)));
                //Variable wpaVersion = new Variable(new ObjectIdentifier(ObjectId.WPAVersion), new Integer32((int)wpaSettings.Version));
                //Variable encryption = new Variable(new ObjectIdentifier(ObjectId.WPAEncryption), new Integer32((int)wpaSettings.Encryption));


                //// creating List of SNMP to fire on the printer to configure Wireless
                //// if we fire SNMP OID individually printer will loose connection, so sending as a group 
                //IList<Lextm.SharpSnmpLib.Variable> snmpList = new List<Lextm.SharpSnmpLib.Variable>();
                //snmpList.Add(ssidName);
                //snmpList.Add(passphrase);
                //snmpList.Add(wpaVersion);
                //snmpList.Add(encryption);

                //SnmpLib snmp = new SnmpLib(IPAddress.Parse(_snmplib.Address));
                //snmp.Set(snmpList);

                const int octetStringSnmpType = 4;  // Part of the SNMP standard
                List<SnmpOidValue> values = new List<SnmpOidValue>();
                values.Add(new SnmpOidValue(ObjectId.SSIDName, CtcUtility.Encrypt(publicKey, wirelessSettings.SsidName), octetStringSnmpType));
                values.Add(new SnmpOidValue(ObjectId.WPAPassphrase, CtcUtility.Encrypt(publicKey, wpaSettings.passphrase), octetStringSnmpType));
                values.Add(new SnmpOidValue(ObjectId.WPAVersion, (int)wpaSettings.Version));
                values.Add(new SnmpOidValue(ObjectId.WPAEncryption, (int)wpaSettings.Encryption));

                _snmplib.Set(values);
                TraceFactory.Logger.Info("Successfully configured WPA Personal through SNMP.");
                return true;
            }
            catch
            {
                TraceFactory.Logger.Info("Failed to configure WPA Personal through SNMP.");
                return false;
            }
        }

        private bool ConfigureWepPersonal(WirelessSettings wirelessSettings, WEPPersonalSettings wepSettings)
        {
            TraceFactory.Logger.Info($"Configuring WPA Personal with basic settings: {wirelessSettings.ToString()} and security settings: {wepSettings.ToString()}");

            try
            {

                string publicKey = GetSecurityPublicKey();

                //Variable ssidName = new Variable(new ObjectIdentifier(ObjectId.SSIDName), new OctetString(Utility.Encrypt(publicKey, wirelessSettings.SsidName)));
                //Variable wepKey = new Variable(new ObjectIdentifier(ObjectId.WEPKey), new OctetString(Utility.Encrypt(publicKey, wepSettings.WEPKey)));


                //// creating List of SNMP to fire on the printer to configure Wireless
                //// if we fire SNMP OID individually printer will loose connection, so sending as a group 
                //IList<Lextm.SharpSnmpLib.Variable> snmpList = new List<Lextm.SharpSnmpLib.Variable>();
                //snmpList.Add(ssidName);
                //snmpList.Add(wepKey);

                //SnmpLib snmp = new SnmpLib(IPAddress.Parse(_snmplib.Address));
                //snmp.Set(snmpList);
                TraceFactory.Logger.Info("Successfully configured WEP Personal through SNMP.");
                return true;
            }
            catch
            {
                TraceFactory.Logger.Info("Failed to configure WEP Personal through SNMP.");
                return false;
            }
        }

        /// <summary>
        /// Set Wireless Mode
        /// </summary>
        /// <param name="status">True to enable, false to disable</param>
        /// <returns>True if the operation is successful.</returns>
        public bool SetWirelessStatus(bool status)
        {
            try
            {
                //_snmplib.Set(ObjectId.WirelessStatus, status ? 1 : 0);

                TraceFactory.Logger.Info("Successfully set wireless STA mode through SNMP.");
                return true;
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to set wireless STA mode through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
        }

        public bool GetWirelessStatus()
        {
            try
            {
                int status = int.Parse(_snmplib.Get(ObjectId.WirelessStatus), CultureInfo.CurrentCulture);

                return status == 1;
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Failed to get wireless STA mode through SNMP.");
                TraceFactory.Logger.Debug(defaultException.Message);
                return false;
            }
        }

        #endregion

        #endregion
    }
}
