using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity.Telnet
{
    #region Enum

    /// <summary>
    /// Enumerator for Telnet Parameters
    /// </summary>
    public enum PrinterParameters
    {
        /// <summary>
        /// Configure Option SLP
        /// </summary>
        SLP,
        /// <summary>
        /// Configure Option SLP Client Mode
        /// </summary>
        SLPCLIENTMODE,
        /// <summary>
        /// Configure Option Web Services Discovery
        /// </summary>
        WSD,
        /// <summary>
        /// Configure Option Bonjour
        /// </summary>
        BONJOUR,
        /// <summary>
        /// Configure Option Bonjour Highest Priority
        /// </summary>
        BONJOURHIGHESTPRIORITY,
        /// <summary>
        /// Configure Option Bonjour Service Name
        /// </summary>
        BONJOURSERVICENAME,
        /// <summary>
        /// Configure Option Multicast IPv4
        /// </summary>
        MULTICASTIPV4,
        /// <summary>
        /// Configure LAA
        /// </summary>
        LAA
    }
    #endregion

    /// <summary>
    /// Represents set of operations through telnet
    /// </summary>
    public sealed class TelnetWrapper
    {
        /// <summary>
        /// SNMP object
        /// </summary>
        private TelnetIpc _telnetlib;

        /// <summary>
        /// Singleton class object
        /// </summary>
        static readonly TelnetWrapper _instance = new TelnetWrapper();

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TelnetWrapper"/> class.
        /// </summary>
        public TelnetWrapper()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the Telnet wrapper using the parameter
        /// </summary>
        /// <param name="deviceAddress">deviceAddress</param>         
        public void Create(string deviceAddress)
        {
            // telnet port 23
            _telnetlib = new TelnetIpc(deviceAddress, 23);
        }

        /// <summary>
        /// Gets the singleton object
        /// </summary>
        /// <returns>Telnet Wrapper singleton object</returns>
        public static TelnetWrapper Instance()
        {
            return _instance;
        }

        # region Toggle Parameter
        /// <summary>
        /// Toggle Telnet Parameter
        /// </summary>
        /// <param name="parametername"> Printer Parameter to be Toggled</param>
        /// <param name="printerFamily"> Printer Family</param>
        /// <param name="printerIPAddress">Printer IP Address</param>
        /// <param name="option">True to Enable, False to Disable</param>
        public bool ToggleParameter(PrinterParameters parametername, PrinterFamilies family, string printerIPAddress, bool option)
        {
            string value = option ? "1" : "0";
            ConfigureValue(family, parametername.ToString(), value);
            value = option ? "Enabled" : "Disabled";
            if (GetConfiguredValue(family, parametername.ToString()).EqualsIgnoreCase(value))
            {
                TraceFactory.Logger.Info("Successfully {0} {1}.".FormatWith(option ? "enabled" : "disabled", parametername.ToString()));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to {0} {1}.".FormatWith(option ? "enable" : "disable", parametername.ToString()));
                return false;
            }
        }
        #endregion

        #region Set Parameter
        /// <summary>
        /// Set Telnet Parameter
        /// </summary>
        /// <param name="parametername"> Printer Parameter to be Set</param>
        /// <param name="printerFamily"> Printer Family</param>
        /// <param name="printerIPAddress">Printer IP Address</param>
        /// <param name="value">Value to be set</param>
        public bool SetParameter(PrinterParameters parametername, PrinterFamilies family, string printerIPAddress, string setValue, string getValue = null, bool validate = true)
        {
            if (getValue == null)
            {
                getValue = setValue;
            }

            ConfigureValue(family, parametername.ToString(), setValue);

            if (validate)
            {
                if (GetConfiguredValue(family, parametername.ToString()).EqualsIgnoreCase(getValue))
                {
                    //TraceFactory.Logger.Info("Successfully set {0} with {1} through Telnet.".FormatWith(parametername.ToString(), getValue));
                    return true;
                }
                else
                {
                    //TraceFactory.Logger.Info("Failed to set {0} with {1} through Telnet.".FormatWith(parametername.ToString(), getValue));
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        #endregion

        /// <summary>
        /// Sets the link speed
        /// </summary>
        /// <param name="printerIPAddress">IP Address of the printer</param>
        /// <param name="linkSpeed">link speed value to be set.</param>
        public void SetLinkSpeed(string printerIPAddress, string linkSpeed)
        {
            try
            {
                using (_telnetlib)
                {
                    TraceFactory.Logger.Info("Connecting Printer: {0} through Telnet".FormatWith(printerIPAddress));
                    _telnetlib.Connect();

                    TraceFactory.Logger.Info("Setting link Speed to {0}".FormatWith(linkSpeed));
                    _telnetlib.SendLine("link-type {0}".FormatWith(linkSpeed));
                    _telnetlib.SendLine("save");

                    TraceFactory.Logger.Info("Link speed is set to {0} through Telnet".FormatWith(linkSpeed));
                }
            }
            catch (Exception telnetError)
            {
                TraceFactory.Logger.Info(telnetError.Message);
                throw new Exception(telnetError.Message);
            }
            finally
            {
                _telnetlib.Dispose();
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Set Host name
        /// </summary>
        /// 
        /// <param name="hostName">Host name to be set</param>
        public void SetHostname(string hostName)
        {
            // Telnet keys for setting an option is same for VEP/ TPS.
            // Display keys differ from VEP/ TPS. TPS: Display and set values are same; hence using TPS as default
            ConfigureValue(PrinterFamilies.TPS, "hostname", hostName);
        }

        /// <summary>
        /// Set Domain name
        /// </summary>
        /// 
        /// <param name="domainname">Domain name to set</param>
        public void SetDomainname(string domainname)
        {
            // Telnet keys for setting an option is same for VEP/ TPS.
            // Display keys differ from VEP/ TPS. TPS: Display and set values are same; hence using TPS as default
            ConfigureValue(PrinterFamilies.TPS, "domainname", domainname);
        }

        /// <summary>
        /// Set Primary Dns Server IP Address
        /// </summary>
        /// 
        /// <param name="serverAddress">Dns Address</param>
        public void SetPrimaryDnsServer(string serverAddress)
        {
            // Telnet keys for setting an option is same for VEP/ TPS.
            // Display keys differ from VEP/ TPS. TPS: Display and set values are same; hence using TPS as default
            ConfigureValue(PrinterFamilies.TPS, "primarydnsserver", serverAddress);
        }

        /// <summary>
        /// Set Primary Wins Server IP Address
        /// </summary>
        /// 
        /// <param name="serverAddress">Wins Address</param>
        public void SetPrimaryWinsServer(string serverAddress)
        {
            // Telnet keys for setting an option is same for VEP/ TPS.
            // Display keys differ from VEP/ TPS. TPS: Display and set values are same; hence using TPS as default
            ConfigureValue(PrinterFamilies.TPS, "primarywinsserver", serverAddress);
        }

        /// <summary>
        /// Gets the configured option value
        /// </summary>
        /// <param name="option">option for which value is required</param>        
        public string GetConfiguredValue(string option)
        {
            string resultList = string.Empty;
            string telnetResult = string.Empty;

            try
            {
                _telnetlib.Connect();
                _telnetlib.SendLine("/");
                Thread.Sleep(TimeSpan.FromSeconds(5));

                // Retrieve the resultList until either the option is found or the end of the file is reached. End of file is marked with '>' character.
                do
                {
                    resultList = string.Empty;
                    _telnetlib.Send(Environment.NewLine);
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    resultList = _telnetlib.ReceiveUntilMatch("$");
                } while (!resultList.Contains(option) && !resultList.EndsWith(">", StringComparison.CurrentCultureIgnoreCase));

                if (resultList.Contains(option))
                {
                    // Remove 'press RETURN to continue:' & '\b' from the resultList
                    resultList = Regex.Replace(resultList, "Press RETURN to continue:", "", RegexOptions.IgnoreCase).Trim();
                    resultList = Regex.Replace(resultList, "\b", "", RegexOptions.IgnoreCase).Trim();

                    // Store each line in the resultList to an array
                    string[] newLine = { Environment.NewLine };
                    string[] result = resultList.Split(newLine, StringSplitOptions.RemoveEmptyEntries);

                    // result is in the format - '<option> : <value>', splitting the result to fetch the required data
                    string[] dataValue = result.Where(item => item.Contains(option)).FirstOrDefault().Split(':');

                    // Hardware address contains ':', after the split dataValue contains the splitted mac address
                    if (option.EqualsIgnoreCase("Hardware Address"))
                    {
                        telnetResult = "{0}:{1}:{2}:{3}:{4}:{5}".FormatWith(dataValue[1].Trim(), dataValue[2].Trim(), dataValue[3].Trim(), dataValue[4].Trim(), dataValue[5].Trim(), dataValue[6].Trim());
                    }
                    else
                    {
                        telnetResult = dataValue[1].ToString().Trim();
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to retrieve the value for {0} through telnet.".FormatWith(option));
                }

                TraceFactory.Logger.Debug("Successfully retrieved the value for {0} through Telnet: {1}".FormatWith(option, telnetResult));
                return telnetResult;
            }
            catch (Exception telnetError)
            {
                TraceFactory.Logger.Info("Failed to retrieve the value for {0} through telnet.".FormatWith(option));
                TraceFactory.Logger.Debug(telnetError.Message);
                return telnetResult;
            }
            finally
            {
                _telnetlib.Dispose();
                // Once the connection is disposed, a wait is required to make sure that the connection is closed
                Thread.Sleep(TimeSpan.FromMilliseconds(1000));
            }
        }

        /// <summary>
        /// Validates and returns the Telnet "Key" for the specific parameter
        /// </summary>
        /// <param name="parameterName">Parameter Name</param>
        /// <param name="family"><see cref="PrinterFamilies"/></param>
        /// <returns>The value for the specified key. Empty if no matching key is found.</returns>
        public string GetConfiguredValue(PrinterFamilies family, string parameterName)
        {
            // Keys are different for VEP/LFP and TPS products. So fetching the key name based on the printer family. Eg: The key for "Host Name" is "host-name" for TPS in telnet.
            string telnetOption = GetTelnetKeyName(family, parameterName, get: true);

            if (telnetOption.Equals("No Data", StringComparison.CurrentCultureIgnoreCase))
            {
                TraceFactory.Logger.Info("The parameter {0} is not supported in {1}.".FormatWith(parameterName, family));
                return string.Empty;
            }
            else
            {
                return GetConfiguredValue(telnetOption);
            }
        }

        /// <summary>
        /// Gets IP Configuration method
        /// </summary>
        /// <param name="family"><see cref="PrinterFamilies"/></param>
        /// <returns><see cref="IPConfigMethod"/></returns>
        public IPConfigMethod GetIPConfigMethod(PrinterFamilies family)
        {
            return IPConfigMethodUtils.GetIPConfigMethod(GetConfiguredValue(family, "configmethod"));
        }

        /// <summary>
        /// Configure Router flags      
        /// </summary>
        /// <param name="command">Command to set Router flags</param>    
        /// <param name="user">User name to connect to the router</param>
        /// <param name="password">Password to connect to the router</param>
        public void ConfigureRouter(string command, string user, string password)
        {
            try
            {
                using (_telnetlib)
                {
                    _telnetlib.Connect();
                    _telnetlib.SendLine(user);
                    _telnetlib.SendLine(password);
                    _telnetlib.SendLine("configure terminal");
                    _telnetlib.SendLine("interface ethernet");

                    switch (command)
                    {
                        case "Enable M":
                            _telnetlib.SendLine("ipv6 nd managed-config-flag");
                            _telnetlib.SendLine("save");
                            break;
                        case "Enable o":
                            _telnetlib.SendLine("ipv6 nd other-config-flag");
                            _telnetlib.SendLine("save");
                            break;
                        case "Enable M o":
                            _telnetlib.SendLine("ipv6 nd managed-config-flag");
                            _telnetlib.SendLine("ipv6 nd other-config-flag");
                            _telnetlib.SendLine("save");
                            break;
                        case "Disable M":
                            _telnetlib.SendLine("no ipv6 nd managed-config-flag");
                            _telnetlib.SendLine("save");
                            break;
                        case "Disable o":
                            _telnetlib.SendLine("no ipv6 nd other-config-flag");
                            _telnetlib.SendLine("save");
                            break;
                        case "Disable M o":
                            _telnetlib.SendLine("no ipv6 nd managed-config-flag");
                            _telnetlib.SendLine("no ipv6 nd other-config-flag");
                            _telnetlib.SendLine("save");
                            break;
                    }
                    _telnetlib.SendLine("exit");
                }
            }
            catch (Exception slpOptionTelnetError)
            {
                TraceFactory.Logger.Info("Failed to configure Router through telnet");
                TraceFactory.Logger.Info(slpOptionTelnetError.Message);
            }
        }

        /// <summary>
        /// Get the configured subnet mask from telnet.
        /// </summary>
        /// <param name="family"><see cref="PrinterFamilies"/></param>
        /// <returns>The subnet mask from telnet.</returns>
        public string GetSubnetMask(PrinterFamilies family)
        {
            return GetConfiguredValue(family, "subnetmask");
        }

        /// <summary>
        /// Sets subnet mask through telnet.
        /// </summary>
        /// <param name="family"><see cref="PrinterFamilies"/></param>
        /// <param name="subnetMask">The subnet mask to be set.</param>
        /// <returns>True if the subnet mask is set.</returns>
        public bool SetSubnetMask(PrinterFamilies family, string subnetMask)
        {
            // Set uses subnet-mask as the key word which is the value for TPS, so passing TPS as default
            ConfigureValue(PrinterFamilies.TPS, "subnetmask", subnetMask);

            if (GetSubnetMask(family).EqualsIgnoreCase(subnetMask))
            {
                TraceFactory.Logger.Info("Successfully set the subnet mask to: {0} through telnet.".FormatWith(subnetMask));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to set the subnet mask to: {0} through telnet.".FormatWith(subnetMask));
                return false;
            }
        }
        /// <summary>
        /// Get the ip address through telnet.
        /// </summary>
        /// <param name="family"><see cref="PrinterFamilies"/></param>
        /// <returns>The ip address from telnet</returns>
        public string GetIPAddress(PrinterFamilies family)
        {
            return GetConfiguredValue(family, "ipaddress");
        }

        /// <summary>
        /// Set the manual ip address through telnet.
        /// </summary>
        /// <param name="family"><see cref="PrinterFamilies"/></param>
        /// <param name="address">IP address to be set.</param>
        /// <returns>True if the ip address is set, else false.</returns>
        public bool SetManualIPAddress(PrinterFamilies family, string address)
        {
            ConfigureValue(PrinterFamilies.TPS, "ipaddress", address);

            _telnetlib.Dispose();
            Thread.Sleep(10000);

            TelnetWrapper.Instance().Create(address);

            // TODO: Add a delay and validate if the manual ip address is set or not
            return true;

            //if (GetIPAddress(family).EqualsIgnoreCase(address))
            //{
            //    TraceFactory.Logger.Info("Successfully set the printer IP address to: {0} through telnet.".FormatWith(address));
            //    return true;
            //}
            //else
            //{
            //    TraceFactory.Logger.Info("Failed to set the printer IP address to: {0} through telnet.".FormatWith(address));
            //    return false;
            //}
        }

        /// <summary>
        /// Set the DDNS option through telnet.
        /// </summary>
        /// <param name="family"><see cref="PrinterFamilies"/></param>
        /// <param name="option">True to enable, false to disable.</param>
        /// <returns>True if the value is set, else false.</returns>
        public bool SetDdns(PrinterFamilies family, bool option)
        {
            string value = option ? "1" : "0";

            ConfigureValue(PrinterFamilies.TPS, "ddnsupdate", value);

            value = option ? "Enabled" : "Disabled";

            if (GetConfiguredValue(family, "ddnsupdate").EqualsIgnoreCase(value))
            {
                TraceFactory.Logger.Info("Successfully {0} DDNS update.".FormatWith(value));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to {0} DDNS update.".FormatWith(option ? "enable" : "disable"));
                return false;
            }
        }

        /// <summary>
        /// Get RFC 4702 Option value
        /// Note: This is only applicable for TPS products
        /// </summary>
        /// <returns>true if option is enabled, false otherwise</returns>
        public bool GetRFCOption()
        {
            return GetConfiguredValue("dhcp-fqdn-conf").EqualsIgnoreCase("disabled") ? false : true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the Telnet Key for Get / Set parameters based on product families.
        /// Currently, for "Set" parameter the telnet keys are same irrespective of product families
        /// For "Get" parameter, if differs between TPS and VEP/LFP.
        /// </summary>
        /// <param name="family"><see cref="PrinterFamilies"/></param>
        /// <param name="settingName">Name of the Parameter</param>
        /// <param name="get">Retrieve "Set" Key or "Get" Key</param>
        private static string GetTelnetKeyName(PrinterFamilies family, string settingName, bool get = false)
        {
            switch (settingName.ToLowerInvariant())
            {
                case "hostname":
                    return (get && family != PrinterFamilies.TPS) ? "Host Name" : "host-name";
                case "configmethod":
                    return (get && family != PrinterFamilies.TPS) ? "IP Config Method" : "ip-config";
                case "domainname":
                    return (get && family != PrinterFamilies.TPS) ? "Domain Name" : "domain-name";
                case "primarydnsserver":
                    return (get && family != PrinterFamilies.TPS) ? "Pri DNS Server" : "pri-dns-svr";
                case "secondarydnsserver":
                    return (get && family != PrinterFamilies.TPS) ? "Sec DNS Server" : "sec-dns-svr";
                case "primarywinsserver":
                    return (get && family != PrinterFamilies.TPS) ? "Pri WINS Server" : "pri-wins-svr";
                case "secondarywinsserver":
                    return (get && family != PrinterFamilies.TPS) ? "Sec WINS Server" : "sec-wins-svr";
                case "leasetime":
                    return (get && family != PrinterFamilies.TPS) ? "DHCP Lease Time" : "dhcp-lease-time";
                case "subnetmask":
                    return (get && family != PrinterFamilies.TPS) ? "Subnet Mask" : "subnet-mask";
                case "multicastipv4":
                    return (get && family != PrinterFamilies.TPS) ? "IPv4 Multicast" : "ipv4-multicast";
                case "ipaddress":
                    return (get && family != PrinterFamilies.TPS) ? "IP Address" : "ip";
                case "ddnsupdate":
                    return (get && family != PrinterFamilies.TPS) ? "DDNS Update" : "ddns-update";
                case "slp":
                    return (get && family != PrinterFamilies.TPS) ? "SLP Config" : "slp-config";
                case "slpclientmode":
                    return (get && family != PrinterFamilies.TPS) ? "SLP Client-Mode" : "slp-client-mode";
                case "wsd":
                    return (get && family != PrinterFamilies.TPS) ? "WS Discovery" : "ws-discovery-conf";
                case "bonjour":
                    return (get && family != PrinterFamilies.TPS) ? "Bonjour Config" : "bonjour-config";
                case "bonjourhighestpriority":
                    return (get && family != PrinterFamilies.TPS) ? "Bonjour Services" : "bonjour-services";
                case "bonjourservicename":
                    return (get && family != PrinterFamilies.TPS) ? "Bonjour Svc Name" : "bonjour-svc-name";
                case "bonjourdomainname":
                    return (get && family != PrinterFamilies.TPS) ? "Bonjour Domain Name" : "bonjour-domain-name";
                case "bonjourgoodbye":
                    return (get && family != PrinterFamilies.TPS) ? "Bonjour Good-bye 1W" : "mdns-goodbye-1w";
                case "laa":
                    return (get && family != PrinterFamilies.TPS) ? "LAA" : "LAA";
                default:
                    return (get && family != PrinterFamilies.TPS) ? "No Data" : "No Data";
            }
        }

        /// <summary>
        /// Sets Parameter using Telnet commands
        /// </summary>
        /// <param name="family"><see cref=" PrinterFamilies"/></param>
        /// <param name="parameterName">Parameter name</param>
        /// <param name="parameterValue">Parameter value</param>
        private void ConfigureValue(PrinterFamilies family, string parameterName, string parameterValue)
        {
            try
            {
                using (_telnetlib)
                {
                    _telnetlib.Connect();
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    string keyName = GetTelnetKeyName(family, parameterName);
                    TraceFactory.Logger.Info("Setting {0} through Telnet using command: {1} {2}".FormatWith(parameterName, keyName, parameterValue));
                    Thread.Sleep(TimeSpan.FromSeconds(15));
                    _telnetlib.SendLine("{0} {1}".FormatWith(keyName, parameterValue));
                    Thread.Sleep(TimeSpan.FromSeconds(15));
                    _telnetlib.SendLine("save");
                    Thread.Sleep(TimeSpan.FromSeconds(15));
                    TraceFactory.Logger.Info("Successfully set {0} to {1} through Telnet.".FormatWith(parameterName, parameterValue));
                }
            }
            catch (Exception telnetError)
            {
                TraceFactory.Logger.Info("Failed to set {0} to {1} through Telnet".FormatWith(parameterName, parameterValue));
                TraceFactory.Logger.Debug(telnetError.Message);
                throw new Exception(telnetError.Message);
            }
            finally
            {
                _telnetlib.Dispose();
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }

        #endregion
    }
}
