using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using HP.ScalableTest.Utility;
using OpenQA.Selenium;

namespace HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer
{
    #region Enum

    /// <summary>
    /// Enumerator for Allow/ Drop traffic for firewall
    /// </summary>
    public enum NetworkTraffic
    {
        /// <summary>
        /// Allow traffic
        /// </summary>
        Allow,
        /// <summary>
        /// Drop traffic
        /// </summary>
        Drop
    }

    /// <summary>
    /// Bonjour Highest Service of the printer
    /// Note: Do NOT change the enum values as if is used at multiple places
    /// </summary>
    [Flags]
    public enum BonjourHighestService
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// 9100 Printing
        /// </summary>
        //[EnumValue("1||Bonjour9100Printing||1||9100 Printing")]
        [EnumValue("1||Bonjour9100Printing||1||P9100")]
        Printing9100 = 1,
        /// <summary>
        /// IPP Printing
        /// </summary>
        //[EnumValue("2||BonjourIPPPrinting||2||Internet Printing Protocol")]
        [EnumValue("2||BonjourIPPPrinting||2||IPP")]
        PrintingIPP = 2,
        /// <summary>
        /// LPD Printing (Raw)
        /// </summary>
        [EnumValue("3||BonjourLPDPrinting||3||")]
        PrintingLPDRAW = 3,
        /// <summary>
        /// LPD Printing (Text)
        /// </summary>
        [EnumValue("4||4||4||")]
        PrintingLPDTEXT = 4,
        /// <summary>
        /// LPD Printing (Auto)
        /// </summary>
        [EnumValue("5||5||5||LPD")]
        PrintingLPDAUTO = 5,
        /// <summary>
        /// LPD Printing (BINPS)
        /// </summary>
        [EnumValue("6||6||6||")]
        PrintingLPDBINPS = 6,
        /// <summary>
        /// IPPS Printing
        /// </summary>
        [EnumValue("16||16||16||")]
        PrintingIPPS = 16
    }

    /// <summary>
    /// Signature Algorithm for Certificate Request
    /// </summary>
    public enum SignatureAlgorithm
    {
        /// <summary>
        /// Sha 1
        /// </summary>
        [EnumValue("SHA1")]
        Sha1,
        /// <summary>
        /// Sha 224
        /// </summary>
        [EnumValue("")]
        Sha224,
        /// <summary>
        /// Sha 256
        /// </summary>
        [EnumValue("SHA256")]
        Sha256,
        /// <summary>
        /// Sha 384
        /// </summary>
        [EnumValue("SHA384")]
        Sha384,
        /// <summary>
        /// Sha512
        /// </summary>
        [EnumValue("SHA512")]
        Sha512,
        /// <summary>
        /// MD5
        /// </summary>
        [EnumValue("MD5")]
        MD5
    }

    /// <summary>
    /// Represents the types of SNMP community Names
    /// </summary>
    public enum SNMPCommunity
    {
        None = 1,
        Get = 2,
        Set = 4,
        Both = Get | Set
    }

    /// <summary>
    /// The RSA key Length
    /// </summary>
    public enum RSAKeyLength
    {
        /// <summary>
        /// Key length 1024
        /// </summary>
        Rsa1024 = 1024,
        /// <summary>
        /// Key length 2048
        /// </summary>
        Rsa2048 = 2048
    }

    /// <summary>
    /// Enumerator for Connectivity Test
    /// </summary>
    public enum ConnectivityTest
    {
        /// <summary>
        /// All Test
        /// </summary>
        All,
        /// <summary>
        /// Network Test
        /// </summary>
        Network,
        /// <summary>
        /// Internet Test
        /// </summary>
        Internet,
        /// <summary>
        /// Cloud Test
        /// </summary>
        Cloud,
        /// <summary>
        /// Services Test
        /// </summary>
        Services,
        /// <summary>
        /// Firmware Test
        /// </summary>
        Firmware,
        /// <summary>
        /// Email Test
        /// </summary>
        Email
    }

    ///// <summary>
    ///// Enumerator for WiFiDirectConnectionMode
    ///// </summary>
    //public enum WiFiDirectConnectionMode
    //{
    //    /// <summary>
    //    /// Auto
    //    /// </summary>
    //    Auto,
    //    /// <summary>
    //    /// Manual
    //    /// </summary>
    //    Manual,
    //    /// <summary>
    //    /// Advanced
    //    /// </summary>
    //    Advanced,
    //    /// <summary>
    //    /// No Security
    //    /// </summary>
    //    NoSecurity,
    //    /// <summary>
    //    /// With Security
    //    /// </summary>
    //    WithSecurity,
    //    /// <summary>
    //    /// Automatic
    //    /// </summary>
    //    Automatic
    //}

    #endregion

    /// <summary>
    /// EWS wrapper class on Ews Adapter
    /// </summary>
    public sealed partial class EwsWrapper
    {
        #region Constant

        private const string SET_DEBUG_LOG = "Setting {0} to {1} from EWS";
        private const string SET_DEBUG_OPTION_LOG = "Setting {0} option to {1} from EWS";
        private const string GET_DEBUG_LOG = "Getting {0} value from EWS";
        private const string GET_DEBUG_OPTION_LOG = "Getting {0} option from EWS";
        private const string GET_SUCCESS_LOG = "Current value of {0} is {1}";
        private const string SUCCESS_LOG = "{0} set to {1} from EWS";
        private const string SUCCESS_OPTION_LOG = "{0} option set to {1} from EWS";
        private const string FAILURE_LOG = CtcBaseTests.FAILURE_PREFIX + "Failed to set {0} to {1} from EWS";
        private const string FAILURE_OPTION_LOG = CtcBaseTests.FAILURE_PREFIX + "Failed to set {0} option to {1} from EWS";
        private const string PASSWORD = "1234";

        #endregion

        /// <summary>
        /// Ews adapter
        /// </summary>
        private EwsAdapter _adapter;

        /// <summary>
        /// Singleton class object
        /// </summary>
        static readonly EwsWrapper _instance = new EwsWrapper();

        #region Constructor

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the singleton object
        /// </summary>
        /// <returns>EWS Wrapper singleton object</returns>
        public static EwsWrapper Instance()
        {
            return _instance;
        }

        /// <summary>
        /// Gets the adapter object
        /// </summary>
        public EwsAdapter Adapter
        {
            get
            {
                return _adapter;
            }
        }

        /// <summary>
        /// Gets or sets a flag indicating if the device is Omni/Opus
        /// </summary>
        public bool IsOmniOpus { get; private set; }

        /// <summary>
        /// Creates the Ews adapter using the parameters
        /// </summary>
        /// <param name="productType">Product Family (VEP,TPS,LFP,InkJet) </param>
        /// <param name="productName">Product Name</param>        
        /// <param name="deviceAddress">Printer IP Address</param>
        /// <param name="sitemapPath">The sitemap path</param>
        /// <param name="browser">Browser model</param>
        /// <param name="adapterType">The web driver or seleniumRC adapter</param>
        public void Create(PrinterFamilies productType, string productName, string deviceAddress, string sitemapPath, BrowserModel browser, EwsAdapterType adapterType = EwsAdapterType.WebDriverAdapter)
		{
            TimeSpan pageNavigationDelay = productType == PrinterFamilies.InkJet ? TimeSpan.FromSeconds(20) : (productType == PrinterFamilies.VEP ? TimeSpan.FromSeconds(5) : TimeSpan.FromSeconds(0));
            TimeSpan elementOperationDelay = productType == PrinterFamilies.InkJet ? TimeSpan.FromSeconds(5) : (productType == PrinterFamilies.VEP ? TimeSpan.FromSeconds(0) : TimeSpan.FromSeconds(0));
            TraceFactory.Logger.Info("sitemap-path :{0}".FormatWith(sitemapPath));
            EwsSettings settings = new EwsSettings(productName, productType, sitemapPath, deviceAddress, browser, pageNavigationDelay, elementOperationDelay, adapterType);
            EwsSeleniumSettings seleniumSettings = new EwsSeleniumSettings();

            //var siteMapPath = Path.Combine(CtcSettings.EwsSiteMapLocation, sitemapPath);

            seleniumSettings.SeleniumChromeDriverPath = CtcSettings.SeleniumChromeDriverPath;
            seleniumSettings.SeleniumIEDriverPath32 = CtcSettings.SeleniumIEDriverPath32;
            seleniumSettings.SeleniumIEDriverPath64 = CtcSettings.SeleniumIEDriverPath64;

            _adapter = new EwsAdapter(settings, seleniumSettings, sitemapPath);
        }

        /// <summary>
        /// Start the EWS adapter
        /// </summary>
        public void Start(string hyperText = "https")
        {
            _adapter.Start(hyperText);

            try
            {
                // Check for Omni/Opus UI for VEP/LFP
                if (_adapter.Settings.ProductType == PrinterFamilies.VEP || _adapter.Settings.ProductType == PrinterFamilies.LFP)
                {
                    _adapter.Navigate("SecureCommunication");

                    // Omni/Opus UI
                    if (_adapter.Body.Contains("404 Not Found"))
                    {
                        TraceFactory.Logger.Info("Printer is not OMNI OPUS");
                        IsOmniOpus = false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Printer is OMNI OPUS");
                        IsOmniOpus = true;
                    }
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug("Printer is Non Omni/Opus UI.");
                TraceFactory.Logger.Debug(ex.Message);
            }
        }

        /// <summary>
        /// Stops the EWS adapter
        /// </summary>
        public void Stop()
        {
            _adapter.Stop();
        }

        /// <summary>
        /// Enable/ disable WSDiscovery option through web UI
        /// </summary>        
        /// 
        /// <returns>true if the Password is set</returns>

        public bool SetAdminPassword()
        {

            try
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    StopAdapter();
                    _adapter.Start();
                    _adapter.Navigate("Settings");
                    _adapter.SetText("Password", PASSWORD);
                    _adapter.SetText("Confirm_password", PASSWORD);
                    _adapter.Click("Apply");

                    if (_adapter.SearchText("The changes have been updated successfully."))
                    {
                        _adapter.Click("OK");
                        TraceFactory.Logger.Info("Successfully Set Admin Password");
                        Thread.Sleep(TimeSpan.FromMinutes(2));
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to Set Password");
                                                   
                    }
                }
                return false;
            }
            finally
            {
                StopAdapter();
                _adapter.Start();
            }
         
        }
		/// <summary>
		/// Enable/ disable WSDiscovery option through web UI
		/// </summary>        
		/// <param name="enable">true to enable, false to disable.</param>
		/// <returns>true if the option status is et, else false.</returns>
		public bool SetWSDiscovery(bool enable)
		{
			string state = enable ? "enable" : "disable";
			TraceFactory.Logger.Debug(SET_DEBUG_OPTION_LOG.FormatWith("WS Discovery", state));

            //TODO Option: Branching based on TPS and VEP Products should be removed
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Advanced");
            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                StopAdapter();
                _adapter.Start();
                _adapter.Navigate("WSD");
            }
            else
            {
                _adapter.Navigate("Misc_Settings");
            }

            if (enable)
            {
                _adapter.Check("WS_Discovery");
            }
            else
            {
                _adapter.Uncheck("WS_Discovery");
            }

            _adapter.Click(_adapter.Settings.ProductType == PrinterFamilies.InkJet || _adapter.Settings.ProductType == PrinterFamilies.TPS ? "Apply" : "Apply_Misc");

            try
            {
                ClickonConfirmation();
            }
            catch
            {
                //Do thing 
                TraceFactory.Logger.Info("Alert is not present");
            }
            Thread.Sleep(TimeSpan.FromSeconds(30));

            if (GetWSDiscovery().Equals(enable))
            {
                TraceFactory.Logger.Info(SUCCESS_OPTION_LOG.FormatWith("WS Discovery", state));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info(FAILURE_OPTION_LOG.FormatWith("WS Discovery", state));
                return false;
            }
        }

        /// <summary>
        /// Get status of WSDiscovery option status
        /// </summary>        
        public bool GetWSDiscovery()
        {
            //TODO: Option

            TraceFactory.Logger.Debug(GET_DEBUG_OPTION_LOG.FormatWith("WS Discovery"));

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Advanced");
            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                StopAdapter();
                _adapter.Start();
                _adapter.Navigate("WSD");
            }
            else
            {
                _adapter.Navigate("Misc_Settings");
            }

            TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("WS Discovery", _adapter.IsChecked("WS_Discovery") ? "enable" : "disable"));

            return _adapter.IsChecked("WS_Discovery");
        }

        /// <summary>
        /// Set the hostname on the device
        /// </summary> 
        /// <param name="hostName">Hostname to be set</param>        
        public bool SetHostname(string hostName)
        {
            TraceFactory.Logger.Debug(SET_DEBUG_LOG.FormatWith("Host name", hostName));

            try
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    _adapter.Stop();
                    _adapter.Start();
                }

                _adapter.Navigate("Network_Identification");
                _adapter.SetText("HostName", hostName);
                _adapter.Click("Apply");
                _adapter.ClickOkonAlert();

                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    ClickonConfirmation();
                    _adapter.Wait(TimeSpan.FromSeconds(60));
                    _adapter.Click("Network_Change_Ok");
                }
                _adapter.Wait(TimeSpan.FromSeconds(30));
            }
            catch
            {
                // Do Nothing
            }
            finally
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    Thread.Sleep(TimeSpan.FromMinutes(3));
                }
                // Browser becomes inactive and goes to infinite loop; trying to stop adapter and kill explicitly 
                StopAdapter();
                _adapter.Start();
            }

            if (GetHostname().Trim().EqualsIgnoreCase(hostName))
            {
                TraceFactory.Logger.Info(SUCCESS_LOG.FormatWith("Host name", hostName));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info(FAILURE_LOG.FormatWith("Host name", hostName));
                return false;
            }
        }

		/// <summary>
		/// Get the device hostname
		/// </summary>
		/// <returns>hostName</returns>
		public string GetHostname()
		{
            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Stop();
                _adapter.Start();
            }
			_adapter.Navigate("Network_Identification");
            //Adding these for INK as sometimes it is not getting host name value.
            if (!_adapter.IsElementPresent("HostName"))
            {
                StopAdapter();
                _adapter.Start();
                _adapter.Navigate("Network_Identification");
            }
			string hostName = _adapter.GetValue("HostName");
			TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("Host name", hostName));

            return hostName;
        }

        /// <summary>
        /// Setting IP address of the Printer
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>True if the IP address is set, else false.</returns>
        public bool SetIPaddress(string address)
        {
            return SetIPConfigMethod(IPConfigMethod.Manual, IPAddress.Parse(address));
        }

        /// <summary>
        /// Gets the configured IP Config Method from Web UI.
        /// </summary>
        /// <returns><see cref="IPConfigMethod"/></returns>
        public IPConfigMethod GetIPConfigMethod()
        {
            TraceFactory.Logger.Debug(GET_DEBUG_LOG.FormatWith("IP Configuration method"));

            IPConfigMethod configMethod = IPConfigMethod.None;

            try
            {
                StopAdapter();
                _adapter.Start();

                string ipConfigMethod;
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    _adapter.Navigate("Wired_Status");
                    ipConfigMethod = _adapter.GetText("IP_Configuration_Method");

                    // Inkjet pageload time is more so reading the value once again 
                    if (string.IsNullOrEmpty(ipConfigMethod))
                    {
                        TraceFactory.Logger.Debug("Could not get the IP Config method value from Web UI. Reading it again..");
                        Thread.Sleep(TimeSpan.FromSeconds(10));
                        ipConfigMethod = _adapter.GetText("IP_Configuration_Method");
                    }
                }
                else
                {
                    // TPS: Configured value will be reflected in the label above dropdown 
                    _adapter.Navigate("IPv4_Config");
                    if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                    {
                        ipConfigMethod = _adapter.GetText("Config_By");
                    }
                    else
                    {
                        ipConfigMethod = _adapter.GetValue("IP_Configuration_Method");
                    }
                }

                configMethod = IPConfigMethodUtils.GetIPConfigMethod(ipConfigMethod);
            }
            catch
            {
                // Do Nothing
            }

            TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("IP Configuration method", configMethod));

            return configMethod;
        }

        /// <summary>
        /// Set IP Configuration method and change wrappers with provided expected IP address
        /// If Configuration Method is DHCP/ BootP: If expected IP Address is not provided, discover and get the printer acquired IP address
        /// If Configuration Method is Manual: EWS, Telnet and SNMP wrappers are updated with Manual IP Address
        /// If Configuration Method is AutoIP: Wrappers are not updated with AutoIP since it is unknown and client machine also needs to be in Auto IP to access printer
        /// It is client's responsibility to change the wrappers to Auto IP		
        /// </summary>
        /// <param name="ipConfigMethod"><see cref=" IPConfigMethod"/></param>
        /// <param name="manualIPAddress">Manual IP Address to be set</param>
        /// <param name="subnetMask">Subnet Mask to be set</param>
        /// <param name="defaultGateway">Default Gateway to be set</param>
        /// <param name="validate">Validate configured method</param>
        /// <param name="expectedIPAddress">Expected printer IP Address after configuration method change</param>
        /// <returns>true if configuration method is set, false otherwise</returns>
        public bool SetIPConfigMethod(IPConfigMethod ipConfigMethod, IPAddress manualIPAddress = null, string subnetMask = null, string defaultGateway = null, bool validate = true, IPAddress expectedIPAddress = null)
        {
            // Invalid configuration method handling
            if (IPConfigMethod.None == ipConfigMethod)
            {
                TraceFactory.Logger.Info("Invalid configuration method.");
                return false;
            }

            IPConfigMethod currentConfigurationMethod = GetIPConfigMethod();

            // We may need to configure printer with different manual IP address 
            if (ipConfigMethod == currentConfigurationMethod && IPConfigMethod.Manual != ipConfigMethod)
            {
                TraceFactory.Logger.Info("Printer is already configured with {0} configuration method.".FormatWith(ipConfigMethod));
                return true;
            }

            string ipConfig = ipConfigMethod.ToString();

            // For VEP/ LFP: AUTO IP configuration method value is the Enum Value for setting in EWS page
            if (PrinterFamilies.TPS != _adapter.Settings.ProductType && IPConfigMethod.AUTOIP == ipConfigMethod)
            {
                ipConfig = Enum<IPConfigMethod>.Value(IPConfigMethod.AUTOIP);
            }

            string printerMacAddress = string.Empty;

            // Get Mac address for printer to discover later.
            // Only when there is change in configuration method to DHCP/ BOOTP and expected IP Address is not provided; we discover for newly acquired IP address
            if (null == expectedIPAddress && (IPConfigMethod.BOOTP == ipConfigMethod || IPConfigMethod.DHCP == ipConfigMethod))
            {
                printerMacAddress = GetMacAddress();
            }

            // If configuration method is manual and manual IP address is not provided, get the current acquired IP address
            // Do this only for products other than Inkjet as the same is handled in the inkjet implementation.
            if (_adapter.Settings.ProductType != PrinterFamilies.InkJet && IPConfigMethod.Manual == ipConfigMethod && null == manualIPAddress)
            {
                expectedIPAddress = IPAddress.Parse(GetIPv4Address());
            }

            try
            {
                TraceFactory.Logger.Debug(SET_DEBUG_LOG.FormatWith("IP Configuration method", ipConfigMethod));

                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    TraceFactory.Logger.Debug(SET_DEBUG_LOG.FormatWith("Setting the EWS configuration method {0}", ipConfigMethod));
                    StopAdapter();
                    _adapter.Start();
                    SetConfigMethod(ipConfigMethod, manualIPAddress, subnetMask, defaultGateway, printerMacAddress, ref expectedIPAddress);
                }
                else
                {
                    //Sometimes Page does not load properly so, closing and openeing the browser again.
                    StopAdapter();
                    _adapter.Start();
                    _adapter.Navigate("IPv4_Config");
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                    _adapter.SelectDropDown("IP_Configuration_Method", ipConfig);

                    if (IPConfigMethod.Manual == ipConfigMethod)
                    {
                        if (!string.IsNullOrEmpty(subnetMask))
                        {
                            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                            {
                                _adapter.SelectDropDown("Subnet_Mask", subnetMask);
                            }
                            else
                            {
                                _adapter.SetText("Subnet_Mask", subnetMask);
                            }
                        }

                        if (!string.IsNullOrEmpty(defaultGateway))
                        {
                            _adapter.SetText("Default_Gateway", defaultGateway);
                        }

                        // Only when manual IP address is provided, set it; other scenario is when we change the configuration method to Manual and 
                        // leave previously acquired IP address as Manual IP address
                        if (null != manualIPAddress)
                        {
                            TraceFactory.Logger.Debug("Setting Manual IP Address : {0}".FormatWith(manualIPAddress));
                            _adapter.SetText("IP_Address", manualIPAddress.ToString());
                            expectedIPAddress = manualIPAddress;
                        }
                    }

                    _adapter.Click("Apply");

                    // TPS will Pop up a message for confirmation
                    _adapter.ClickOkonAlert();
                }

                // When a configuration method is changed, printer takes some time to update configurations
                Thread.Sleep(TimeSpan.FromMinutes(2));

                if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
                {
                    TraceFactory.Logger.Info("Delay of 3 minuttes for VEP.");
                    Thread.Sleep(TimeSpan.FromMinutes(3));
                }
            }
            catch (Exception defaultException)
            {
                // In case of exception, wait for browser to be released by used processes
                TraceFactory.Logger.Debug("Exception details: {0}".FormatWith(defaultException.Message));
                TraceFactory.Logger.Debug("Waiting for 2 minutes");
                Thread.Sleep(TimeSpan.FromMinutes(2));
            }
            finally
            {
                // Browser becomes inactive and goes to infinite loop; trying to stop adapter and kill explicitly 
                StopAdapter();

                // Discover Printer with mac address and assign the new IP Address acquired by Printer to wrappers
                if (!string.IsNullOrEmpty(printerMacAddress))
                {
                    TraceFactory.Logger.Debug("Discovering Printers...");
                    expectedIPAddress = IPAddress.Parse(CtcUtility.GetPrinterIPAddress(printerMacAddress));
                }

                // Client holds the responsibility of changing wrappers with Auto IP address when configuration method is changed to AutoIP. (Auto IP Address is unknown) 				
                if (IPConfigMethod.AUTOIP != ipConfigMethod)
                {
                    ChangeDeviceAddress(expectedIPAddress);
                    SnmpWrapper.Instance().Create(expectedIPAddress.ToString());
                    TelnetWrapper.Instance().Create(expectedIPAddress.ToString());
                }

                // When Configuration method is changed to Auto IP, adapter.Start will fail to open EWS page. (Printer will be in Auto IP and local machine in DHCP server IP)
                // This will NOT throw up any exception/ failure.  
                _adapter.Start();
            }

            if (validate)
            {
                return (ipConfigMethod == GetIPConfigMethod());
            }

            TraceFactory.Logger.Info(SUCCESS_LOG.FormatWith("IP Configuration method", ipConfigMethod.ToString()));

            return true;
        }

        /// <summary>
        /// Set Default IP Configuration method according to Printer family
        /// VEP/ LFP: DHCP, TPS: BOOTP
        /// </summary>
        /// <param name="validate">true if validation is required for IP Config method, else false.</param>
        /// <param name="expectedIPAddress">The expected ip address.</param>
        /// <returns>true if default configuration method is configured, false otherwise</returns>
        public bool SetDefaultIPConfigMethod(bool validate = true, IPAddress expectedIPAddress = null)
        {
            Printer.Printer printer = PrinterFactory.Create(_adapter.Settings.ProductType, IPAddress.Parse(_adapter.Settings.DeviceAddress));

            // If the default config method is already set return without setting it again.
            if (GetIPConfigMethod() == printer.DefaultIPConfigMethod)
            {
                TraceFactory.Logger.Debug("Configuration method : {0} is already set on printer.".FormatWith(printer.DefaultIPConfigMethod));
                return true;
            }

            return SetIPConfigMethod(printer.DefaultIPConfigMethod, validate: validate, expectedIPAddress: expectedIPAddress);
        }

        /// <summary>
        /// Get the IPV4 option status from Web UI.
        /// <remarks>If IPv4 option is disabled, use any address other than IPv4 address or host name to access EWS.</remarks>
        /// </summary>   
        /// <returns>True if the option is enabled, else false.</returns>
        public bool GetIPv4()
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Stop();
                _adapter.Start();

                TraceFactory.Logger.Debug(GET_DEBUG_OPTION_LOG.FormatWith("IPv4"));
                _adapter.Navigate("NetProtocol");

                return _adapter.IsChecked("IPv4") || _adapter.IsChecked("IPv4v6");
            }
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug(GET_DEBUG_OPTION_LOG.FormatWith("IPv4"));
                _adapter.Navigate("Advanced");

                TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("IPv4", _adapter.IsChecked("IPv4") ? "enable" : "disable"));

                return _adapter.IsChecked("IPv4");
            }
            else
            {
                TraceFactory.Logger.Debug("IPv4 enable/disable is applicable only for Ink and TPS.");
                return true;
            }
        }

        /// <summary>
        /// Enable/ disable IPV4 option through WebUI        
        /// </summary>        
        /// <param name="enable">true to enable, false to disable</param>
        /// <param name="printer">Printer Object</param>
        /// <param name="printerIpv4Address">Printer IPv4 Address</param>
        public bool SetIPv4(bool enable, Printer.Printer printer = null, string printerIpv4Address = null)
        {
            if (_adapter.Settings.ProductType != PrinterFamilies.TPS || _adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                TraceFactory.Logger.Debug("IPv4 enable/disable is applicable only for TPS and Ink.");
                return true;
            }

            string state = enable ? "enable" : "disable";
            IPAddress linkLocalAddress = IPAddress.None;

            if (printer != null)
            {
                linkLocalAddress = printer.IPv6LinkLocalAddress;
                TraceFactory.Logger.Debug("LinkLocalAddress of the printer is {0}".FormatWith(linkLocalAddress));
            }

            try
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    TraceFactory.Logger.Debug(SET_DEBUG_OPTION_LOG.FormatWith("IPv4", state));
                    _adapter.Navigate("NetProtocol");
                }
                else
                {
                    TraceFactory.Logger.Debug(SET_DEBUG_OPTION_LOG.FormatWith("IPv4", state));
                    _adapter.Navigate("Advanced");
                }

                if (enable)
                {
                    _adapter.Check("IPv4");
                }
                else
                {
                    if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                    {
                        //enabling IPv6 radio button is to disable the "IPv4 enabled" radio button in Ink.
                        _adapter.Check("IPv6");
                    }
                    else
                    {
                        _adapter.Uncheck("IPv4");
                    }
                }

                _adapter.Click("Apply");

                ClickonConfirmation();
            }
            catch
            {
                // Do Nothing
            }
            finally
            {
                // Browser becomes inactive and goes to infinite loop; trying to stop adapter and kill explicitly 
                StopAdapter();

                if (!enable)
                {
                    // As IPv4 address is not available, accessing EWS using link local address.
                    Instance().ChangeDeviceAddress(linkLocalAddress);
                }
                else
                {
                    Instance().ChangeDeviceAddress(printerIpv4Address);
                }
                _adapter.Start();
            }

            if (GetIPv4().Equals(enable))
            {
                TraceFactory.Logger.Info(SUCCESS_OPTION_LOG.FormatWith("IPv4", state));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info(FAILURE_OPTION_LOG.FormatWith("IPv4", state));
                return false;
            }
        }

        /// <summary>
        /// Enable/ disable SLP option on WebUI option        
        /// </summary>        
        /// <param name="enable">true to enable, false to disable</param>
        public bool SetSLP(bool enable)
        {
            string state = enable ? "enable" : "disable";
            TraceFactory.Logger.Debug(SET_DEBUG_OPTION_LOG.FormatWith("Slp", state));

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Advanced");
            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Stop();
                _adapter.Start();
                _adapter.Navigate("SLP");
            }
            else
            {
                _adapter.Navigate("Misc_Settings");
            }

            if (enable)
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    _adapter.Check("SLP_Option_enable");
                }
                else
                {
                    _adapter.Check("SLP");
                }
            }
            else
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    _adapter.Check("SLP_Option_disable");
                }
                else
                {
                    _adapter.Uncheck("SLP");
                }
            }

			_adapter.Click(_adapter.Settings.ProductType == PrinterFamilies.InkJet || _adapter.Settings.ProductType == PrinterFamilies.TPS ? "Apply" : "Apply_Misc");
            try
            {
                ClickonConfirmation();
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Exception occured : {0}". FormatWith(ex.Message));
            }
			Thread.Sleep(TimeSpan.FromSeconds(30));

            if (GetSLP().Equals(enable))
            {
                TraceFactory.Logger.Info(SUCCESS_OPTION_LOG.FormatWith("Slp", state));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info(FAILURE_OPTION_LOG.FormatWith("Slp", state));
                return false;
            }
        }

        /// <summary>
        /// Get Slp status
        /// </summary>
        /// <returns></returns>
        public bool GetSLP()
        {
            //TODO: Option

            TraceFactory.Logger.Debug(GET_DEBUG_OPTION_LOG.FormatWith("Slp"));

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Advanced");
            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Stop();
                _adapter.Start();
                _adapter.Navigate("SLP");
            }
            else
            {
                _adapter.Navigate("Misc_Settings");
            }

            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("Slp", _adapter.IsChecked("SLP_Option_enable") == true ? "enable" : "disable"));
                return _adapter.IsChecked("SLP_Option_enable");
            }
            else
            {
                TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("Slp", _adapter.IsChecked("SLP") == true ? "enable" : "disable"));
                return _adapter.IsChecked("SLP");
            }
        }

        /// <summary>
        /// Enable/ disable P9100 option on WebUI option        
        /// </summary>        
        /// <param name="enable">true to enable, false to disable</param>
        public bool SetP9100(bool enable)
        {
            string state = enable ? "enable" : "disable";
            TraceFactory.Logger.Debug(SET_DEBUG_OPTION_LOG.FormatWith("P9100", state));

            if (_adapter.Settings.ProductType == (PrinterFamilies.TPS))
            {
                _adapter.Navigate("Advanced");
            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Stop();
                _adapter.Start();
                _adapter.Navigate("P9100");
            }
            else
            {
                _adapter.Navigate("Misc_Settings");
            }

            if (enable)
            {
                _adapter.Check("P9100_Printing");
            }
            else
            {
                _adapter.Uncheck("P9100_Printing");
            }

			_adapter.Click(_adapter.Settings.ProductType == PrinterFamilies.InkJet || _adapter.Settings.ProductType ==(PrinterFamilies.TPS) ? "Apply" : "Apply_Misc");
            try
            {
                ClickonConfirmation();
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Exception Occured : {0}".FormatWith(ex.Message));
            }
			Thread.Sleep(TimeSpan.FromSeconds(30));

            if (GetP9100().Equals(enable))
            {
                TraceFactory.Logger.Info(SUCCESS_OPTION_LOG.FormatWith("P9100", state));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info(FAILURE_OPTION_LOG.FormatWith("P9100", state));
                return false;
            }
        }

        /// <summary>
        /// Get P9100 status
        /// </summary>
        /// <returns></returns>
        public bool GetP9100()
        {
            //TODO: Option

            TraceFactory.Logger.Debug(GET_DEBUG_OPTION_LOG.FormatWith("P9100"));

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Advanced");
            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Stop();
                _adapter.Start();
                _adapter.Navigate("P9100");
            }
            else
            {
                _adapter.Navigate("Misc_Settings");
            }

            TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("P9100", _adapter.IsChecked("P9100_Printing") ? "enable" : "disable"));

            return _adapter.IsChecked("P9100_Printing");
        }

        /// <summary>
        /// Enable/ disable LPD option on WebUI option        
        /// </summary>        
        /// <param name="enable">true to enable, false to disable</param>
        public bool SetLPD(bool enable)
        {
            string state = enable ? "enable" : "disable";
            TraceFactory.Logger.Debug(SET_DEBUG_OPTION_LOG.FormatWith("LPD", state));

            if (_adapter.Settings.ProductType == (PrinterFamilies.TPS))
            {
                _adapter.Navigate("Advanced");

            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Navigate("LPD");
            }
            else
            {
                _adapter.Navigate("Misc_Settings");
            }

            if (enable)
            {
                _adapter.Check("LPD_Printing");
            }
            else
            {
                _adapter.Uncheck("LPD_Printing");
            }

			_adapter.Click(_adapter.Settings.ProductType == PrinterFamilies.InkJet || _adapter.Settings.ProductType ==(PrinterFamilies.TPS) ? "Apply" : "Apply_Misc");
            try
            {
                ClickonConfirmation();
            }
            catch (Exception ex)
            {

                TraceFactory.Logger.Info("Exception Occured : {0}".FormatWith(ex.Message));
            }

			Thread.Sleep(TimeSpan.FromSeconds(30));

            if (GetLPD().Equals(enable))
            {
                TraceFactory.Logger.Info(SUCCESS_OPTION_LOG.FormatWith("LPD", state));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info(FAILURE_OPTION_LOG.FormatWith("LPD", state));
                return false;
            }
        }

        /// <summary>
        /// Get LPD status
        /// </summary>
        /// <returns></returns>
        public bool GetLPD()
        {
            //TODO: Option

            TraceFactory.Logger.Debug(GET_DEBUG_OPTION_LOG.FormatWith("LPD"));

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Advanced");
            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Navigate("LPD");
            }
            else
            {
                _adapter.Navigate("Misc_Settings");
            }

            TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("LPD", _adapter.IsChecked("LPD_Printing") ? "enable" : "disable"));

            return _adapter.IsChecked("LPD_Printing");
        }

        /// <summary>
        /// Enable/ disable IPP option on WebUI option        
        /// </summary>        
        /// <param name="enable">true to enable, false to disable</param>
        public bool SetIPP(bool enable)
        {
            string state = enable ? "enable" : "disable";
            TraceFactory.Logger.Debug(SET_DEBUG_OPTION_LOG.FormatWith("IPP", state));

            if (enable)
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                {
                    _adapter.Navigate("Advanced");
                    _adapter.Check("IPP_Printing");
                    _adapter.Click("Apply");
                    ClickonConfirmation();
                }
                else
                {
                    _adapter.Navigate("Misc_Settings");
                    _adapter.Check("IPP_Printing");
                    _adapter.Click("Apply_Misc");
                }

            }
            else
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                {
                    _adapter.Navigate("Advanced");
                    _adapter.Uncheck("IPP_Printing");
                    _adapter.Click("Apply");
                    try
                    {
                        ClickonConfirmation();
                    }
                    catch (Exception ex)
                    {
                        TraceFactory.Logger.Info("Exception Occured : {0}".FormatWith(ex.Message));
                    }
                }
                else
                {
                    _adapter.Navigate("Misc_Settings");
                    _adapter.Uncheck("IPP_Printing");
                    _adapter.Click("Apply_Misc");
                }

            }

            Thread.Sleep(TimeSpan.FromSeconds(5));

            if (GetIPP().Equals(enable))
            {
                TraceFactory.Logger.Info(SUCCESS_OPTION_LOG.FormatWith("IPP", state));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info(FAILURE_OPTION_LOG.FormatWith("IPP", state));
                return false;
            }
        }

        /// <summary>
        /// Get IPP status
        /// </summary>
        /// <returns></returns>
        public bool GetIPP()
        {
            //TODO: Option

            TraceFactory.Logger.Debug(GET_DEBUG_OPTION_LOG.FormatWith("IPP"));

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Advanced");
            }
            else
            {
                _adapter.Navigate("Misc_Settings");
            }

            TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("IPP", _adapter.IsChecked("IPP_Printing") ? "enable" : "disable"));

            return _adapter.IsChecked("IPP_Printing");
        }

        /// <summary>
        /// Enable/ disable IPP option on WebUI option        
        /// </summary>        
        /// <param name="enable">true to enable, false to disable</param>
        public bool SetIPPS(bool enable)
        {
            string state = enable ? "enable" : "disable";
            TraceFactory.Logger.Debug(SET_DEBUG_OPTION_LOG.FormatWith("IPPS", state));

            if (enable)
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                {
                    _adapter.Navigate("Advanced");
                    _adapter.Check("IPPS_Printing");
                    _adapter.Click("Apply");
                    ClickonConfirmation();
                }
                else
                {
                    _adapter.Navigate("Misc_Settings");
                    _adapter.Check("IPPS_Printing");
                    _adapter.Click("Apply_Misc");
                }

            }
            else
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                {
                    _adapter.Navigate("Advanced");
                    _adapter.Uncheck("IPPS_Printing");
                    _adapter.Click("Apply");
                    ClickonConfirmation();
                }
                else
                {
                    _adapter.Navigate("Misc_Settings");
                    _adapter.Uncheck("IPPS_Printing");
                    _adapter.Click("Apply_Misc");
                }

            }

            Thread.Sleep(TimeSpan.FromSeconds(5));

            if (GetIPPS().Equals(enable))
            {
                TraceFactory.Logger.Info(SUCCESS_OPTION_LOG.FormatWith("IPPS", state));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info(FAILURE_OPTION_LOG.FormatWith("IPPS", state));
                return false;
            }
        }

        /// <summary>
        /// Get IPPS status
        /// </summary>
        /// <returns></returns>
        public bool GetIPPS()
        {
            //TODO: Option

            TraceFactory.Logger.Debug(GET_DEBUG_OPTION_LOG.FormatWith("IPPS"));

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Advanced");
            }
            else
            {
                _adapter.Navigate("Misc_Settings");
            }

            TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("IPPS", _adapter.IsChecked("IPPS_Printing") ? "enable" : "disable"));

            return _adapter.IsChecked("IPPS_Printing");
        }

        /// <summary>
        /// Enable/ disable Bonjour option on WebUI       
        /// </summary>        
        /// <param name="enable">true to enable, false to disable</param>
        /// <returns>True if bonjour is enabled, else false.</returns>
        public bool SetBonjour(bool enable)
        {
            string state = enable ? "enable" : "disable";
            TraceFactory.Logger.Debug(SET_DEBUG_OPTION_LOG.FormatWith("Bonjour", state));

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Advanced");
            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Stop();
                _adapter.Start();
                _adapter.Navigate("Bonjour");
            }
            else
            {
                _adapter.Navigate("Misc_Settings");
            }

            if (enable)
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    _adapter.Check("Bonjour_Option_enable");
                }
                else
                {
                    _adapter.Check("Bonjour");
                }
            }
            else
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    _adapter.Check("Bonjour_Option_disable");
                }
                else
                {
                    _adapter.Uncheck("Bonjour");
                }
            }

            _adapter.Click(_adapter.Settings.ProductType == PrinterFamilies.InkJet || _adapter.Settings.ProductType == (PrinterFamilies.TPS) ? "Apply" : "Apply_Misc");

            if (!enable)
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    _adapter.Click("Ok");
                }
            }
            try
            {
                ClickonConfirmation();
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Exception : {0}".FormatWith(ex));
            }
            Thread.Sleep(TimeSpan.FromSeconds(5));

            if (GetBonjour().Equals(enable))
            {
                TraceFactory.Logger.Info(SUCCESS_OPTION_LOG.FormatWith("Bonjour", state));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info(FAILURE_OPTION_LOG.FormatWith("Bonjour", state));
                return false;
            }
        }

        /// <summary>
        /// Get Bonjour status
        /// </summary>
        /// <returns></returns>
        public bool GetBonjour()
        {
            //TODO: Option

            TraceFactory.Logger.Debug(GET_DEBUG_OPTION_LOG.FormatWith("Bonjour"));

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Advanced");
                TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("Bonjour", _adapter.IsChecked("Bonjour") ? "enable" : "disable"));
                return _adapter.IsChecked("Bonjour");

            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Stop();
                _adapter.Start();
                _adapter.Navigate("Bonjour");
                TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("Bonjour", _adapter.IsChecked("Bonjour_Option_enable") ? "enable" : "disable"));
                return _adapter.IsChecked("Bonjour_Option_enable");
            }
            else
            {
                _adapter.Navigate("Misc_Settings");
                TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("Bonjour", _adapter.IsChecked("Bonjour") ? "enable" : "disable"));
                return _adapter.IsChecked("Bonjour");
            }

            //TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("Bonjour", _adapter.IsChecked("Bonjour") || _adapter.IsChecked("Bonjour_Option_enable") ? "enable" : "disable"));

            //return _adapter.Settings.ProductType == PrinterFamilies.InkJet ? _adapter.IsChecked("Bonjour_Option_enable") : _adapter.IsChecked("Bonjour") ;
        }

        /// <summary>
		/// Enable/ disable SLP Client Mode option on WebUI option        
		/// </summary>        
		/// <param name="enable">true to enable, false to disable</param>
		public bool SetSLPClientMode(bool enable)
        {
            string state = enable ? "enable" : "disable";
            TraceFactory.Logger.Debug(SET_DEBUG_OPTION_LOG.FormatWith("SLP Client Mode", state));

            //TODO: Option
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("SLP Client Mode is not supported for TPS products!");
            }
            else
            {
                if (enable)
                {
                    _adapter.Navigate("Advanced");
                    _adapter.Check("SLP_Client_Mode_Only");
                    _adapter.Click("Apply");
                }
                else
                {
                    TraceFactory.Logger.Info("Disabling SLP Client Only Option option");
                    _adapter.Navigate("Advanced");
                    _adapter.Uncheck("SLP_Client_Mode_Only");
                    _adapter.Click("Apply");
                }
            }
            Thread.Sleep(TimeSpan.FromSeconds(5));

            if (GetSLPClientMode().Equals(enable))
            {
                TraceFactory.Logger.Info(SUCCESS_OPTION_LOG.FormatWith("SLP Client Mode", state));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info(FAILURE_OPTION_LOG.FormatWith("SLP Client Mode", state));
                return false;
            }
        }

        /// <summary>
        /// Get Slp Client Mode status
        /// </summary>
        /// <returns></returns>
        public bool GetSLPClientMode()
        {

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("SLP Client Mode is not supported for TPS products!");
                return true;
            }
            else
            {
                TraceFactory.Logger.Debug(GET_DEBUG_OPTION_LOG.FormatWith("SLP Client Mode"));
                _adapter.Navigate("Advanced");
                TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("SLP Client Mode", _adapter.IsChecked("SLP_Client_Mode_Only") ? "enable" : "disable"));
                return _adapter.IsChecked("SLP_Client_Mode_Only");
            }
        }

        /// <summary>
        /// Setting Adapter device address to printer IP address
        /// This function is required if we change IPAddress of the printer
        /// </summary>        
        /// <param name="ipAddress">IP Address of the Printer</param>
        public void SetAdapterAddress(string ipAddress)
        {
            if (!string.IsNullOrEmpty(ipAddress))
            {
                _adapter.Settings.DeviceAddress = ipAddress;
                TraceFactory.Logger.Info("Changed adapter device address to: {0}".FormatWith(ipAddress));
            }
        }

        /// <summary>
        /// Get the device SubnetMask
        /// </summary>
        /// <returns>SubnetMask</returns>
        public IPAddress GetSubnetMask()
        {
            TraceFactory.Logger.Debug(GET_DEBUG_LOG.FormatWith("Subnetmask"));

            IPAddress subnet = IPAddress.None;

            _adapter.Navigate("IPv4_Config");
            _adapter.SelectDropDown("IP_Configuration_Method", IPConfigMethod.Manual.ToString());

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                //For TPS Product Subnet mask will be in disabled state in web-UI,for which we are not able to fetch subnet value, 
                //so we are enabling the option by selecting the IPConfig method to Manual				

                int subnetmask = Convert.ToInt32(_adapter.GetValue("Subnet_Mask"), CultureInfo.InvariantCulture);
                byte[] parts = BitConverter.GetBytes(subnetmask);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(parts);
                }
                return new IPAddress(parts);

            }
            else
            {
                subnet = IPAddress.Parse(_adapter.GetValue("Subnet_Mask"));
            }

            TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("Subnetmask", subnet));

            return subnet;
        }

        /// <summary>
        /// Gets the configured link speed from Web UI.
        /// </summary>
        /// <returns><see cref="PrinterLinkSpeed"/></returns>
        public PrinterLinkSpeed GetLinkSpeed()
        {
            PrinterLinkSpeed linkSpeed = PrinterLinkSpeed.None;
            TraceFactory.Logger.Info("Getting the link speed from Web UI");
            if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
            {
                _adapter.Navigate("Misc_Settings");
                linkSpeed = CtcUtility.GetEnum<PrinterLinkSpeed>(_adapter.GetValue("Link_settings"), _adapter.Settings.ProductType);
            }
            else
            {
                _adapter.Stop();
                _adapter.Start();
                Thread.Sleep(TimeSpan.FromMinutes(1));

                //TODO Need to change the element name once the enable all feature is implemented.
                _adapter.Navigate("NetworkAdvanced_INK");

                if (_adapter.IsElementPresent("Link_settings"))
                {
                    linkSpeed = CtcUtility.GetEnum<PrinterLinkSpeed>(_adapter.GetValue("Link_settings"), _adapter.Settings.ProductType);
                }
                else
                {
                    if (_adapter.IsChecked("LinkAuto"))
                    {
                        linkSpeed = PrinterLinkSpeed.Auto;
                    }
                    else
                    {
                        string speed = _adapter.GetValue("SpeedValue");
                        string mode = _adapter.GetValue("CommunicationModeValue");

                        linkSpeed = CtcUtility.GetEnum<PrinterLinkSpeed>("{0}_{1}".FormatWith(speed, mode), PrinterFamilies.InkJet);
                    }
                }
            }

            TraceFactory.Logger.Info("Current Link speed: {0}".FormatWith(linkSpeed));
            return linkSpeed;
        }

        /// <summary>
        /// Gets the configured Bonjour Highest Service from EWS
        /// </summary>
        /// <returns><see cref="BonjourHighestService"/></returns>

        public BonjourHighestService GetBonjourHighestService()
        {
            BonjourHighestService bonjourService = BonjourHighestService.None;
            TraceFactory.Logger.Info("Getting the Bonjour Highest Service from EWS.");

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Network_Identification");
            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Stop();
                _adapter.Start();
                _adapter.Navigate("Bonjour");
            }
            else
            {
                _adapter.Navigate("Misc_Settings");
            }

            bonjourService = CtcUtility.GetEnum<BonjourHighestService>(_adapter.GetValue("Bonjour_Highest_Priority_Service"), _adapter.Settings.ProductType);
            TraceFactory.Logger.Info("Bonjour Highest Service is: {0}".FormatWith(bonjourService));
            return bonjourService;


        }

        /// Summary
        /// Validates different connectivity tests
        /// </Summary>
        /// <param name = "connectivityTest" >< see cref="ConnectivityTests"/></param>

        public bool ValidateConnectivity(ConnectivityTest connectivityTest)
        {
            TraceFactory.Logger.Info("Validating Connectivity Tests: {0}".FormatWith(connectivityTest));
            _adapter.Navigate("Connectivity_Tests");
            _adapter.Check(CtcUtility.GetEnumvalue(Enum<ConnectivityTest>.Value(connectivityTest), _adapter.Settings.ProductType));
            _adapter.Click("Run_Test");
            Thread.Sleep(TimeSpan.FromSeconds(120));
            if (_adapter.SearchText("Test complete"))
            {
                TraceFactory.Logger.Info("Connectivity Test: {0} test successfully passed".FormatWith(connectivityTest));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Connectivity Test: {0} test failed".FormatWith(connectivityTest));
                return false;
            }
        }

        /// Summary
        /// Sets Web Proxy
        /// </Summary>
        /// <param name = "connectivityTest" >< see cref="ConnectivityTests"/></param>

        public bool SetWebProxy(WebProxyConfigurationDetails configDetails)
        {
            TraceFactory.Logger.Info("Configuring Web Proxy using: {0}".FormatWith(configDetails));
            _adapter.Navigate("Web_Proxy");
            _adapter.Check(CtcUtility.GetEnumvalue(Enum<WebProxyType>.Value(configDetails.ProxyType), _adapter.Settings.ProductType));
            if (configDetails.ProxyType == WebProxyType.Curl)
            {
                _adapter.SetText("CURL_Value", configDetails.cURL);
            }
            else
            {
                if (configDetails.ProxyType == WebProxyType.Manual)
                {
                    _adapter.SetText("ProxyServerAddress", configDetails.IPAddress);
                    _adapter.SetText("ProxyServerPort", configDetails.PortNo.ToString());
                    _adapter.SetText("ProxyServerUsername", configDetails.UserName);
                    _adapter.SetText("ProxyServerPassword", configDetails.Password);

                    _adapter.Uncheck("Basic");
                    _adapter.Uncheck("Digest");

                    if (configDetails.AuthType.HasFlag(WebProxyAuthType.Basic))
                    {
                        _adapter.Check("Basic");
                    }

                    if (configDetails.AuthType.HasFlag(WebProxyAuthType.Digest))
                    {
                        _adapter.Check("Digest");
                    }
                }
            }
            _adapter.Click("Apply");
            Thread.Sleep(TimeSpan.FromSeconds(20));
            if (GetWebProxy().Equals(configDetails))
            {
                TraceFactory.Logger.Info("Successfully configured Web Proxy using: {0}".FormatWith(configDetails.ProxyType));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to configure Web Proxy using: {0}".FormatWith(configDetails.ProxyType));
                return false;
            }
        }

        /// Summary
        /// Gets Web Proxy
        /// </Summary>
        /// <param name = "connectivityTest" >< see cref="ConnectivityTests"/></param>

        public WebProxyConfigurationDetails GetWebProxy()
        {
            _adapter.Navigate("Web_Proxy");
            WebProxyConfigurationDetails configuredDetails = new WebProxyConfigurationDetails();
            if (_adapter.IsChecked("Auto"))
            {
                configuredDetails.ProxyType = WebProxyType.Auto;
            }
            else if (_adapter.IsChecked("Curl"))
            {
                configuredDetails.ProxyType = WebProxyType.Curl;
                configuredDetails.cURL = _adapter.GetValue("CURL_Value");
            }
            else if (_adapter.IsChecked("Manual"))
            {
                configuredDetails.ProxyType = WebProxyType.Manual;
                configuredDetails.IPAddress = _adapter.GetValue("ProxyServerAddress");
                configuredDetails.PortNo = Convert.ToInt32(_adapter.GetValue("ProxyServerPort"));
                configuredDetails.UserName = _adapter.GetValue("ProxyServerUsername");

                configuredDetails.AuthType = WebProxyAuthType.None;

                if (_adapter.IsChecked("Basic"))
                {
                    configuredDetails.AuthType |= WebProxyAuthType.Basic;
                }

                if (_adapter.IsChecked("Digest"))
                {
                    configuredDetails.AuthType |= WebProxyAuthType.Digest;
                }
            }
            else
            {
                configuredDetails.ProxyType = WebProxyType.Disable;
            }
            TraceFactory.Logger.Debug("Web Proxy Parameters Configured: {0}".FormatWith(configuredDetails.ToString()));
            return configuredDetails;
        }

        public bool SetBonjourHighestService(BonjourHighestService bonjourService)
        {

            TraceFactory.Logger.Info("Setting the Bonjour Highest Service to:{0} ".FormatWith(bonjourService));

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Network_Identification");
            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Stop();
                _adapter.Start();
                _adapter.Navigate("Bonjour");
            }
            else
            {
                _adapter.Navigate("Misc_Settings");
            }

            _adapter.SelectByValue("Bonjour_Highest_Priority_Service", CtcUtility.GetEnumvalue(Enum<BonjourHighestService>.Value(bonjourService), _adapter.Settings.ProductType), true);
            _adapter.Click(_adapter.Settings.ProductType == PrinterFamilies.InkJet || _adapter.Settings.ProductType == (PrinterFamilies.TPS) ? "Apply" : "Apply_Misc");

            Thread.Sleep(TimeSpan.FromSeconds(30));
            if (bonjourService == GetBonjourHighestService())
            {
                TraceFactory.Logger.Info("Successfully set the Bonjour Highest Service to: {0} from EWS".FormatWith(bonjourService));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to set the Bonjour Highest Service to: {0} from EWS".FormatWith(bonjourService));
                return false;
            }
        }

        /// <summary>
        /// Sets Link speed to the specified value
        /// </summary>
        /// <param name="linkSpeed"><see cref="PrinterLinkSpeed"/></param>
        public bool SetLinkSpeed(PrinterLinkSpeed linkSpeed)
        {
            TraceFactory.Logger.Info("Setting the link speed to :{0} ".FormatWith(linkSpeed));
            if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
            {
                _adapter.Navigate("Misc_Settings");
                _adapter.SelectByValue("Link_settings", CtcUtility.GetEnumvalue(Enum<PrinterLinkSpeed>.Value(linkSpeed), _adapter.Settings.ProductType));
                _adapter.Click("Apply_Misc");
            }
            else
            {
                _adapter.Stop();
                _adapter.Start();

                //TODO Need to change the element name once the enable all feature is implemented.
                _adapter.Navigate("NetworkAdvanced_INK");

                if (_adapter.IsElementPresent("Link_settings"))
                {
                    _adapter.SelectByValue("Link_settings", CtcUtility.GetEnumvalue(Enum<PrinterLinkSpeed>.Value(linkSpeed), PrinterFamilies.InkJet));
                }
                else
                {
                    if (linkSpeed == PrinterLinkSpeed.Auto)
                    {
                        _adapter.Click("LinkAuto");
                    }
                    else
                    {
                        string value = CtcUtility.GetEnumvalue(Enum<PrinterLinkSpeed>.Value(linkSpeed), PrinterFamilies.InkJet);
                        string communicationMode = value.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)[1];
                        string speed = value.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)[0];

                        _adapter.Click("LinkManual");

                        _adapter.SelectByValue("SpeedValue", speed);
                        _adapter.SelectByValue("CommunicationModeValue", communicationMode);
                    }
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));

                _adapter.Click("Apply");
            }

            Thread.Sleep(TimeSpan.FromMinutes(1));

            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                if (_adapter.SearchText("Printer is processing your request.") || _adapter.SearchText("The changes have been updated successfully."))
                {
                    _adapter.Click("OK");
                    Thread.Sleep(TimeSpan.FromMinutes(2));
                }

                if (linkSpeed == GetLinkSpeed())
                {
                    TraceFactory.Logger.Info("Successfully set the link speed to: {0}".FormatWith(linkSpeed));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to set the link speed to: {0}".FormatWith(linkSpeed));
                    return false;
                }
            }

            if (_adapter.SearchText("Changes have been made successfully"))
            {
                TraceFactory.Logger.Info("Successfully set the link speed to: {0}".FormatWith(linkSpeed));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to set the link speed to: {0}".FormatWith(linkSpeed));
                return false;
            }
        }

        /// <summary>
        /// Sets the LAA value
        /// </summary>
        /// <param name="laaValue">LAA to be set</param>
        /// <param name="expectedAddress">Expected Address</param>
        public bool SetLAA(string laaValue, IPAddress expectedAddress = null)
        {
            try
            {
                TraceFactory.Logger.Info("Changing LAA value to {0}".FormatWith(laaValue));
                _adapter.Navigate("Misc_Settings");
                _adapter.SetText("Locally_Administered_Address", laaValue);
                _adapter.Click("Apply_Misc");
                Thread.Sleep(TimeSpan.FromSeconds(30));
            }
            finally
            {
                StopAdapter();

                // When the LAA is changed printer acquires a new IP address, if the IP config method is not manual.
                if (null == expectedAddress)
                {
                    Thread.Sleep(TimeSpan.FromMinutes(3));
                    expectedAddress = IPAddress.Parse(CtcUtility.GetPrinterIPAddress(laaValue));
                }
                _adapter.Settings.DeviceAddress = expectedAddress.ToString();
                _adapter.Start();
            }

            if (GetLAA().EqualsIgnoreCase(laaValue))
            {
                TraceFactory.Logger.Info("Successfully set LAA: {0} through web UI.".FormatWith(laaValue));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to set LAA: {0} through web UI.".FormatWith(laaValue));
                return false;
            }
        }

        /// <summary>
        /// Get LAA Value set on the printer
        /// </summary>
        /// <returns>LAA value</returns>
        public string GetLAA()
        {
            _adapter.Navigate("Misc_Settings");
            string laaValue = _adapter.GetValue("Locally_Administered_Address");
            TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("LAA", laaValue));
            return laaValue;
        }

        /// <summary>
        /// Validate Wireless configured SSID name of the printer        
        /// </summary>
        /// <returns>SSID Name</returns>        
        public bool ValidateSSIDName(string SSID)
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Summary");
            }
            else
            {
                _adapter.Navigate("Configuration_Page");
            }
            return _adapter.SearchText(SSID);
        }

        /// <summary>
        /// Setting the Dynamic raw port number.
        /// </summary>
        /// <param name="port1Number">Dynamic Raw port 1, value should be between 3000-9000</param>
        /// <param name="port2Number">Dynamic Raw port 2, value should be between 3000-9000</param>
        /// <returns>Returns true if it is successfully set, else returns false</returns>
        public bool SetDynamicRawPorts(int port1Number, int port2Number = 0)
        {
            try
            {
                _adapter.Navigate("Misc_Settings");

                // Setting Dynamic Raw Port 1
                // validate the RAW port number before actually setting
                // port number should be between 3000 to 9000
                if (port1Number >= 3000 && port1Number <= 9000)
                {
                    TraceFactory.Logger.Info("Setting 'Dynamic Raw Port 1' to {0}".FormatWith(port1Number));
                    _adapter.SetText("Dynamic_Raw_Port_1", port1Number.ToString(CultureInfo.CurrentCulture));
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to set 'Dynamic Raw Port 1', port number should be between 3000 to 9000 range");
                    return false;
                }

                // Setting Dynamic Raw Port 2
                // set only it is non zero default value is 0
                if (port2Number != 0)
                {
                    if (port2Number >= 3000 && port2Number <= 9000)
                    {
                        TraceFactory.Logger.Info("Setting 'Dynamic Raw Port 2' to {0}".FormatWith(port2Number));
                        _adapter.SetText("Dynamic_Raw_Port_2", port2Number.ToString(CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to set 'Dynamic Raw Port 2', port number should be between 3000 to 9000 range");
                        return false;
                    }
                }

                // apply the settings
                _adapter.Click("Apply_Misc");

                return true;
            }
            catch (Exception)
            {
                TraceFactory.Logger.Info("Failed to set Dynamic Raw Ports");
                return false;
            }
        }

        /// <summary>
        /// Clears the Dynamic raw port number.
        /// </summary>        
        /// <returns>Returns true if it is successfully clears, else returns false</returns>
        public bool ClearDynamicRawPorts()
        {
            try
            {
                // clearing Dynamic Raw Port 1
                TraceFactory.Logger.Debug("Clearing 'Dynamic Raw Port 1' ");
                _adapter.Navigate("Misc_Settings");
                _adapter.SetText("Dynamic_Raw_Port_1", string.Empty);

                //System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));

                // clearing Dynamic Raw Port 2
                TraceFactory.Logger.Debug("Clearing 'Dynamic Raw Port 2' ");
                _adapter.SetText("Dynamic_Raw_Port_2", string.Empty);

                //System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));

                // apply the settings
                _adapter.Click("Apply_Misc");

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));

                return true;
            }
            catch (Exception)
            {
                TraceFactory.Logger.Info("Failed to clear Dynamic Raw Ports");
                return false;
            }
        }

        /// <summary>
        /// Search for the specified text in the page
        /// </summary>
        /// <param name="navigatePage">Page to navigate</param>
        /// <param name="searchText">Text to be searched</param>
        /// <returns>true if search text is found, false otherwise</returns>
        public bool SearchTextInPage(string searchText, string navigatePage = null)
        {
            if (!string.IsNullOrEmpty(navigatePage))
            {
                _adapter.Navigate(navigatePage);
            }

            return _adapter.SearchText(searchText);
        }

        /// <summary>
        /// Sets the date time on the printer.
        /// Note: Date time settings is not applicable for TPS products
        /// </summary>
        /// <param name="dateTime">date time to be set.</param>
        /// <returns>True if the date time is set, else false.</returns>
        public bool SetDateTime(DateTime dateTime)
        {

            // For TPS date time setting is not available
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                throw new NotSupportedException("Date time settings is not supported for {0}.".FormatWith(PrinterFamilies.TPS));
            }

            TraceFactory.Logger.Info("Setting Date time to: {0}".FormatWith(dateTime.ToShortDateString()));

            try
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    SetDateTimeValue(dateTime);
                }
                else
                {
						//This is not required				
                    //if (IsOmniOpus)
                    //{
                    //    SetDateTimeOmniOpus(dateTime);
                    //}
                    //else
                    //{
                        int month = _adapter.Settings.ProductType == PrinterFamilies.LFP ? dateTime.Month - 1 : dateTime.Month;
                        int day = dateTime.Day;
                        int year = dateTime.Year;
                        int hour = int.Parse(dateTime.ToString("hh"));
                        int minutes = dateTime.Minute;
                        int seconds = dateTime.Second;
                        string amPm = dateTime.ToString("tt").ToUpper();

                        _adapter.Navigate("Date_And_Time");

                        if (_adapter.Settings.ProductType == PrinterFamilies.LFP)
                        {
                            _adapter.SelectByValue("Day", day.ToString());
                            _adapter.SelectByValue("Year", year.ToString());
                            // For LFP, the value for 12 hrs is 0 in the drop down
                            _adapter.SelectByValue("Hour", hour == 12 ? "0" : hour.ToString());
                            _adapter.SelectByValue("Minutes", minutes.ToString());
                            _adapter.SelectByValue("Seconds", seconds.ToString());
                        }
                        else if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
                        {
                            // For VEP seconds is not available in web UI. So discarding the seconds component of the date time for validation.
                            dateTime = DateTime.Parse("{0}/{1}/{2} {3}:{4} {5}".FormatWith(month, day, year, hour, minutes, amPm));
                            _adapter.SetText("Day", day.ToString());
                            _adapter.SetText("Year", year.ToString());
                            _adapter.SetText("Hour", hour.ToString());
                            _adapter.SetText("Minutes", minutes.ToString());
                            _adapter.Uncheck("AutoAdjustClock");
                        }

                        _adapter.SelectByValue("Month", month.ToString());
                        _adapter.SelectDropDown("AMPM", dateTime.ToString("tt").ToUpper());
                        _adapter.Click("Apply");
                    //}
                }

                Thread.Sleep(TimeSpan.FromSeconds(30));

                // Validating whether the date time is in range of 5 minutes from the actual date time, as the time keeps on changing.
                DateTime upperRange = dateTime.AddMinutes(5);
                DateTime printerTime = GetDateTime();

                if (printerTime.ToString("d-MM-yyyy") == dateTime.ToString("d-MM-yyyy"))
                {
                    TraceFactory.Logger.Info("Successfully set the date time to: {0}.".FormatWith(dateTime.ToString("d-MM-yyyy")));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to set the date time to: {0}".FormatWith(dateTime));
                    TraceFactory.Logger.Info("Current Date time: {0}".FormatWith(printerTime.ToString("d-MM-yyyy")));
                    return false;
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Failed to set the date time to: {0}".FormatWith(dateTime));
                TraceFactory.Logger.Info(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// This method can be used to set the date and time values for inkjet products only.
        /// </summary>
        /// <param name="dateTime">The date time value to be set.</param>
        private void SetDateTimeValue(DateTime dateTime)
        {
            // Set the region to india
            _adapter.Stop();
            _adapter.Start();

            _adapter.Navigate("International");
            _adapter.SelectByValue("Region", "india");
            _adapter.Click("Apply");
            Thread.Sleep(TimeSpan.FromSeconds(10));
            _adapter.Click("Ok");
            Thread.Sleep(TimeSpan.FromMinutes(1));

            _adapter.Stop();
            _adapter.Start();

            _adapter.Navigate("Date_And_Time");

            if (_adapter.IsElementPresent("DateTime_SyncNetwork") && _adapter.IsChecked("DateTime_SyncNetwork"))
            {
                _adapter.Uncheck("DateTime_SyncNetwork");
                _adapter.Click("DateTime_SyncOk");
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }

            _adapter.ExecuteScript("$('#~CurrentDate~').val('{0}');".FormatWith(dateTime.ToString("d-MM-yyyy")));

            _adapter.Click("Apply");

            Thread.Sleep(TimeSpan.FromMinutes(1));

            if (_adapter.SearchText("The changes have been updated successfully."))
            {
                _adapter.Click("Ok");
            }
        }

        /// <summary>
		/// This method can be used to set the date and time values for Omni/Opus UI only.
		/// </summary>
		/// <param name="dateTime">The date time value to be set.</param>
        public bool SetDateTimeOmniOpus(DateTime dateTime)
        {
            TraceFactory.Logger.Info(SET_DEBUG_LOG.FormatWith("date", dateTime.Date));
            _adapter.Navigate("Date_And_Time");

            _adapter.Check("Date_Format_0");

            _adapter.ExecuteScript("$('#~CurrentDate~').val('{0}');".FormatWith(dateTime.ToString("d MMM, yyyy")));

            if (_adapter.IsElementPresent("AutoAdjustClock"))
            {
                _adapter.Uncheck("AutoAdjustClock");
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }

            _adapter.Click("Apply");

            Thread.Sleep(TimeSpan.FromSeconds(10));

            if (_adapter.SearchText("successfully."))
            {
                TraceFactory.Logger.Info(SUCCESS_LOG.FormatWith("Date", dateTime.Date));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info(FAILURE_LOG.FormatWith("date", dateTime.Date));
                return false;
            }
        }

        /// <summary>
        /// Gets the current date time on the printer.
        /// </summary>
        /// <returns>The current date time on the printer.</returns>
        public DateTime GetDateTime()
        {
            string dateTimeFormat = "{0}/{1}/{2} {3}:{4}:{5} {6}";
            string currentDateTime = string.Empty;

            _adapter.Navigate("Date_And_Time");

            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                DateTime currentValue = DateTime.ParseExact(_adapter.GetText("CurrentDeviceTime"), "d-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                currentDateTime = dateTimeFormat.FormatWith(currentValue.Month, currentValue.Day, currentValue.Year, currentValue.Hour, currentValue.Minute, currentValue.Second, currentValue.ToString("tt"));
            }
            else
            {
                //if (IsOmniOpus)
                //{
                //    // Only the date is considered here, ignored time values
                //    return  DateTime.Parse(_adapter.GetValue("CurrentDate"));
                //}
                //else
                //{
                    int month = _adapter.Settings.ProductType== PrinterFamilies.LFP ? int.Parse(_adapter.GetValue("Month")) + 1 : int.Parse(_adapter.GetValue("Month"));
                    int day = int.Parse(_adapter.GetValue("Day"));
                    int year = int.Parse(_adapter.GetValue("Year"));
                    int hour = int.Parse(_adapter.GetValue("Hour"));
                    int minutes = int.Parse(_adapter.GetValue("Minutes"));
                    int seconds = int.Parse(_adapter.Settings.ProductType == PrinterFamilies.LFP ? _adapter.GetValue("Seconds") : "0");
                    string ampm = _adapter.Settings.ProductType == PrinterFamilies.LFP ? (_adapter.GetValue("AMPM").EqualsIgnoreCase("0") ? "AM" : "PM") : _adapter.GetValue("AMPM");
                    currentDateTime = dateTimeFormat.FormatWith(month, day, year, hour, minutes, seconds, ampm);
                //}
            }

            return DateTime.Parse(currentDateTime);
        }

        /// <summary>
        /// Set LLMNR on the device        
        /// </summary>        
        /// <param name="option">true to enable, false to disable</param>
        public bool SetLlmnr(bool option)
        {
            TraceFactory.Logger.Info("Setting the LLMNR to : {0} on the device".FormatWith(option));

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Advanced");
            }
            else if ((_adapter.Settings.ProductType == PrinterFamilies.VEP))
            {
                _adapter.Navigate("MngmtProtocol_Other");
            }
            else
            {
                StopAdapter();
                _adapter.Start();
                _adapter.Navigate("LLMNR");
                
            }
            // FOR INK LLMNR is Differnt Page and it has Radio buttonf for Enable and Disable unlike VEP and TPS
			if (option)
			{
                if ((_adapter.Settings.ProductType == PrinterFamilies.InkJet))
                {
                    _adapter.Click("LLMNR_Option_enable");
                }
                else
                {
                    _adapter.Check("LLMNR");
                }
			}
			else
			{
                if ((_adapter.Settings.ProductType == PrinterFamilies.InkJet))
                {
                    _adapter.Click("LLMNR_Option_disable");
                }
                else
                {
                    _adapter.Uncheck("LLMNR");
                }
			}
			_adapter.Click("Apply");
			Thread.Sleep(TimeSpan.FromSeconds(40));
			ClickonConfirmation();

            if (GetLlmnr().Equals(option))
            {
                TraceFactory.Logger.Info("Successfully set the LLMNR value to {0} state".FormatWith(option));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to set the LLMNR value to {0} state".FormatWith(option));
                return false;
            }

        }

        /// <summary>
        /// Get the LLMNR Option has been enabled/disabled
        /// </summary>
        /// <returns>true if enabled or else false</returns>
        public bool GetLlmnr()
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Advanced");
                return (_adapter.IsChecked("LLMNR"));
            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.VEP)

            {
                _adapter.Navigate("MngmtProtocol_Other");
                return (_adapter.IsChecked("LLMNR"));
            }
            else
            {
                StopAdapter();
                _adapter.Start();
                _adapter.Navigate("LLMNR");
                return (_adapter.IsChecked("LLMNR_Option_enable"));

            }
		
		}

        /// <summary>
        /// Get the IPV6 option state
        /// </summary>
        /// <returns>true if enabled or else false</returns>
        public bool GetIPv6()
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Stop();
                _adapter.Start();

                _adapter.Navigate("NetProtocol");

                if (_adapter.IsChecked("IPv4v6") || _adapter.IsChecked("IPv6"))
                {
                    TraceFactory.Logger.Info("IPv6 enabled");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("IPv6 disabled");
                    return false;
                }
            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Advanced");
            }
            else
            {
                _adapter.Navigate("TCPIPv6");
            }

            return (_adapter.IsChecked("IPv6"));
        }

        /// <summary>
        /// Set IPV6 option on the device        
        /// </summary>        
        /// <param name="option">true to enable, false to disable</param>
        /// <returns>true if successfully set, else false</returns>
        public bool SetIPv6(bool option)
        {
            TraceFactory.Logger.Debug("{0} IPv6 option on the device".FormatWith(option ? "Enabling" : "Disabling"));

            try
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    StopAdapter();
                    _adapter.Start();
                    _adapter.Navigate("NetProtocol");
                }
                else if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                {
                    _adapter.Navigate("Advanced");
                }
                else
                {
                    _adapter.Navigate("TCPIPv6");
                }

                if (option)
                {
                    if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
                    {
                        _adapter.Check("IPv6");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Enablaing Both IPv6 and IPv4 options for Inkjet as Selecting IPv6 disables IPv4");
                        _adapter.Check("IPv4v6");
                    }
                   
                }
                else
                {
                    if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
                    {
                        _adapter.Uncheck("IPv6");
                    }
                    else
                    {
                        //enabling the "only IPv4 enabled" option is the method to disable IPv6 in Ink.
                        TraceFactory.Logger.Info("Enablaing IPv4 option for Inkjet so it disables IPv6");

                        _adapter.Check("IPv4");
                    }
                }

                _adapter.Click("Apply");

                ClickonConfirmation();
            }
            catch
            {
                // Do nothing
            }
            finally
            {
                // Browser becomes inactive and goes to infinite loop; trying to stop adapter and kill explicitly 
                StopAdapter();
                _adapter.Start();
            }

            if (GetIPv6().Equals(option))
            {
                TraceFactory.Logger.Info("{0} IPv6 option.".FormatWith(option ? "Enabled" : "Disabled"));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to {0} IPv6 option.".FormatWith(option ? "enable" : "disable"));
                return false;
            }
        }

        public void EnableDisablev6()
        {
            TraceFactory.Logger.Debug("Enable-Disable IPv6 option on the device");
            if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                _adapter.Navigate("TCPIPv6");
                _adapter.Uncheck("IPv6");
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(30));
                TraceFactory.Logger.Debug("Disabled IPV6 successfully");
                StopAdapter();
                _adapter.Start();
                _adapter.Navigate("TCPIPv6");
                _adapter.Check("IPv6");
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(30));
                TraceFactory.Logger.Debug("Enabled IPV6 successfully");
                StopAdapter();
                _adapter.Start();

                if (GetIPv6().Equals(true))
                {
                    TraceFactory.Logger.Info("IPv6 option is Disabled and Enabled again");
                }
                else
                {
                    TraceFactory.Logger.Info("Error while disabling and enabling IPv6 option");
                }

            }
            else
            {
                TraceFactory.Logger.Info("This step is not required for TPS and InkJet. Skipping this step");
            }

        }


		/// <summary>
		/// Get the device domain name
		/// </summary>
		/// <returns>domainName</returns>
		public string GetDomainName()
		{
			TraceFactory.Logger.Info("Getting Domain Name of the printer");
            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                StopAdapter();
                _adapter.Start();
            }
            _adapter.Navigate("Network_Identification");
            string domainName = _adapter.GetValue("DomainName");

            TraceFactory.Logger.Info("The DomainName of the Printer: {0}".FormatWith(domainName));
            return domainName;
        }

		/// <summary>
		/// Sets the Domain Name
		/// </summary>
		/// <param name="domainName">Domain Name to be set</param>
		public bool SetDomainName(string domainName)
		{
			try
			{
				TraceFactory.Logger.Info("Setting domain name to: {0}".FormatWith(domainName));
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    StopAdapter();
                    _adapter.Start();
                }
                _adapter.Navigate("Network_Identification");
                _adapter.SetText("DomainName", domainName);
                _adapter.Click("Apply");
                _adapter.ClickOkonAlert();

                Thread.Sleep(TimeSpan.FromSeconds(40));
            }
            catch
            {
                //Do nothing
            }
            finally
            {
                // Browser becomes inactive and goes to infinite loop; trying to stop adapter and kill explicitly 
                StopAdapter();
                _adapter.Start();
            }

            if (GetDomainName().Equals(domainName))
            {
                TraceFactory.Logger.Info("Successfully set the Domain Name: {0}.".FormatWith(domainName));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to set the Domain Name: {0}.".FormatWith(domainName));
                return false;
            }
        }

        /// <summary>
        /// Get the DNS PrimaryIP of the device
        /// </summary>
        /// <returns>dnsprimaryIP</returns>
        public string GetPrimaryDnsServer()
        {
            TraceFactory.Logger.Info("Getting the primary DNS Server IP Address from Web UI.");
            string primaryDnsServer = string.Empty;

			if (_adapter.Settings.ProductType== PrinterFamilies.InkJet)
			{
                StopAdapter();
                _adapter.Start();
                _adapter.Navigate("Wired_Status");
                primaryDnsServer = _adapter.GetText("DNS_IPv4Primary");
            }
            else
            {
                _adapter.Navigate("Network_Identification");
                primaryDnsServer = _adapter.GetValue("DNS_IPv4Primary");
            }

            //For TPS Printers Primary WinServerIP field value is "Not Specified" for Empty string
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                if (primaryDnsServer == "Not Specified")
                {
                    primaryDnsServer = "0.0.0.0";
                }
            }

            TraceFactory.Logger.Info("The primary DNS server IP Address of the Printer: {0}".FormatWith(primaryDnsServer));
            return primaryDnsServer;
        }

        /// <summary>
        /// Sets the primary DNS IP Address of the Printer
        /// </summary>
        /// <param name="dnsPrimaryIP">DNS Primary IP to set</param>
        public bool SetPrimaryDnsServer(string dnsPrimaryIP)
        {
            TraceFactory.Logger.Info("Setting DNS Primary IP Address to: {0}".FormatWith(dnsPrimaryIP));

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Since TPS is not refreshing the page after setting DNSPrimary IP of the Printer,closing and opening browser");
                _adapter.Stop();
                _adapter.Start();
                _adapter.Navigate("Network_Identification");
                _adapter.SetText("DNS_IPv4Primary", dnsPrimaryIP);
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(20));
                _adapter.ClickOkonAlert();
                //Since the Page still in refreshing mode selenium throws exception if we try to stop the adapter,to address this leaving the browser and starting new browser
                // _adapter.Stop();
                _adapter.Start();
            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                //Inkjet has different format of taking DNSserverIP not as text box.

                if (dnsPrimaryIP == string.Empty)
                {
                    dnsPrimaryIP = "0.0.0.0";
                }
                TraceFactory.Logger.Info("Setting DNS primary server to {0}".FormatWith(dnsPrimaryIP));
                StopAdapter();
                _adapter.Start();
                _adapter.Navigate("IPv4_Config");

                string PrimaryDNSserver = "PreferredDNS_";

                _adapter.Click("Manual_DNS_Server");

                for (int i = 0; i < 4; i++)
                {
                    _adapter.SetText("{0}{1}".FormatWith(PrimaryDNSserver, i), dnsPrimaryIP.ToString().Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[i]);
                }
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(120));
            }

            else
            {
                _adapter.Navigate("Network_Identification");
                _adapter.SetText("DNS_IPv4Primary", dnsPrimaryIP);
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(20));
            }

            if (GetPrimaryDnsServer().Equals(dnsPrimaryIP))
            {
                TraceFactory.Logger.Info("Successfully configured DNS Primary IP to:{0}".FormatWith(dnsPrimaryIP));
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    StopAdapter();
                    _adapter.Start();
                }
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to configure DNS Primary IP to:{0}".FormatWith(dnsPrimaryIP));
                return false;
            }
        }

        /// <summary>
        /// Gets the secondary DNS IP Address from Web UI.
        /// </summary>
        /// <returns>The secondary DNS Server IP Address.</returns>
        public string GetSecondaryDnsServer()
        {
            TraceFactory.Logger.Info("Getting the secondary DNS Server IP Address from Web UI.");
            string secondaryDnsServer = string.Empty;

			if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
			{
                StopAdapter();
                _adapter.Start();
                _adapter.Navigate("Wired_Status");
                secondaryDnsServer = _adapter.GetText("DNS_IPv4_Secondary");
            }
            else
            {
                _adapter.Navigate("Network_Identification");
                secondaryDnsServer = _adapter.GetValue("DNS_IPv4_Secondary");
            }

            //For TPS Printers Primary WinServerIP field value is "Not Specified" for Empty string
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                if (secondaryDnsServer == "Not Specified")
                {
                    secondaryDnsServer = "0.0.0.0";
                }
            }
            TraceFactory.Logger.Info("Secondary server is  : {0}".FormatWith(secondaryDnsServer));
            return secondaryDnsServer;
        }

        /// <summary>
        /// Set Secondary DNS Server IP
        /// </summary>
        /// <param name="dnsServerIP">DNS Server IP</param>
        public bool SetSecondaryDNSServerIP(string dnsServerIP)
        {
            TraceFactory.Logger.Info("Setting Secondary DNS Server IP to {0}.".FormatWith(dnsServerIP));

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Since TPS is not refreshing the page after setting DNSPrimary IP of the Printer,closing and opening browser");
                _adapter.Stop();
                _adapter.Start();
                _adapter.Navigate("Network_Identification");
                _adapter.SetText("DNS_IPv4_Secondary", dnsServerIP);
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(20));
                _adapter.ClickOkonAlert();
                //Since the Page still in refreshing mode selenium throws exception if we try to stop the adapter,to address this leaving the browser and starting new browser
                // _adapter.Stop();
                _adapter.Start();
            }
            else
            {
                _adapter.Navigate("Network_Identification");
                _adapter.SetText("DNS_IPv4_Secondary", dnsServerIP);
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(20));
            }

            if (GetSecondaryDnsServer().Equals(dnsServerIP))
            {
                TraceFactory.Logger.Info("Successfully configured DNS Secondary IP to:{0}".FormatWith(dnsServerIP));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to configure DNS Secondary IP to:{0}".FormatWith(dnsServerIP));
                return false;
            }
        }

        /// <summary>
        /// Get Primary Dnsv6 Server address
        /// </summary>
        /// <returns>Primary Dnsv6 server address</returns>
        public string GetPrimaryDnsv6Server()
        {
            TraceFactory.Logger.Debug("Getting the primary DNSv6 Server IP Address from Web UI.");

            SetIPv6(false);
            SetIPv6(true);

			// Sometimes, configuration is not getting updated on printer. Adding a delay
			Thread.Sleep(TimeSpan.FromSeconds(30));
            StopAdapter();
            _adapter.Start();

            _adapter.Navigate("Network_Identification");
            string primaryDnsServer = _adapter.GetValue("DNS_IPv6_Primary");

            //For TPS Printers Primary WinServerIP field value is "Not Specified" for Empty string
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                if (primaryDnsServer == "Not Specified")
                {
                    return string.Empty;
                }
                else
                {
                    TraceFactory.Logger.Info("The primary DNS server IPv6 Address of the Printer: {0}".FormatWith(primaryDnsServer));
                    return primaryDnsServer;
                }
            }
            else
            {
                TraceFactory.Logger.Info("The primary DNS server IPv6 Address of the Printer: {0}".FormatWith(primaryDnsServer));
                return primaryDnsServer;
            }
        }

        /// <summary>
        /// Set Primary Dnsv6 Server Address
        /// </summary>
        /// <param name="dnsServerIP">Dns Server Address</param>
        public bool SetPrimaryDnsv6Server(string dnsServerIP)
        {
            TraceFactory.Logger.Debug("Setting the primary DNSv6 Server IP Address from Web UI.");

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Since TPS is not refreshing the page after setting DNSPrimary IP of the Printer,closing and opening browser");
                _adapter.Stop();
                _adapter.Start();
                _adapter.Navigate("Network_Identification");
                _adapter.SetText("DNS_IPv6_Primary", dnsServerIP);
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(20));
                _adapter.ClickOkonAlert();
                //Since the Page still in refreshing mode selenium throws exception if we try to stop the adapter,to address this leaving the browser and starting new browser
                // _adapter.Stop();
                _adapter.Start();
            }
            else
            {
                _adapter.Navigate("Network_Identification");
                _adapter.SetText("DNS_IPv6_Primary", dnsServerIP);
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(20));
            }

            if (GetPrimaryDnsv6Server().Equals(dnsServerIP))
            {
                TraceFactory.Logger.Info("Successfully configured DNSv6 Primary IP to:{0}".FormatWith(dnsServerIP));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to configure DNSv6 primary IP to:{0}".FormatWith(dnsServerIP));
                return false;
            }
        }

        /// <summary>
        /// Get secondary Dnsv6 Server address
        /// </summary>
        /// <returns>Secondary Dnsv6 server address</returns>
        public string GetSecondaryDnsv6Server()
        {
            TraceFactory.Logger.Debug("Getting the secondary DNS Server IP Address from Web UI.");

            SetIPv6(false);
            SetIPv6(true);

			// Sometimes, configuration is not getting updated on printer. Adding a delay
			Thread.Sleep(TimeSpan.FromSeconds(30));
            StopAdapter();
            _adapter.Start();
            _adapter.Navigate("Network_Identification");
            string secondaryDnsServer = _adapter.GetValue("DNS_IPv6_Secondary");

            //For TPS Printers Primary WinServerIP field value is "Not Specified" for Empty string
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                if (secondaryDnsServer == "Not Specified")
                {
                    TraceFactory.Logger.Info("The secondary DNS server IP Address of the Printer: {0}".FormatWith(secondaryDnsServer));
                    return "";
                }
                else
                {
                    TraceFactory.Logger.Info("The secondary DNS server IP Address of the Printer: {0}".FormatWith(secondaryDnsServer));
                    return secondaryDnsServer;
                }
            }
            else
            {
                TraceFactory.Logger.Info("The secondary DNS server IP Address of the Printer: {0}".FormatWith(secondaryDnsServer));
                return secondaryDnsServer;
            }
        }

        /// <summary>
        /// Set Secondary Dnsv6 Server Address
        /// </summary>
        /// <param name="dnsServerIP">Dns Server Address</param>
        public bool SetSecondaryDnsv6Server(string dnsServerIP)
        {
            TraceFactory.Logger.Debug("Setting the secondary DNSv6 Server IP Address from Web UI.");

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Since TPS is not refreshing the page after setting DNSPrimary IP of the Printer,closing and opening browser");
                _adapter.Stop();
                _adapter.Start();
                _adapter.Navigate("Network_Identification");
                _adapter.SetText("DNS_IPv6_Secondary", dnsServerIP);
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(20));
                _adapter.ClickOkonAlert();
                //Since the Page still in refreshing mode selenium throws exception if we try to stop the adapter,to address this leaving the browser and starting new browser
                // _adapter.Stop();
                _adapter.Start();
            }
            else
            {
                _adapter.Navigate("Network_Identification");
                _adapter.SetText("DNS_IPv6_Secondary", dnsServerIP);
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(20));
            }

            if (GetSecondaryDnsv6Server().Equals(dnsServerIP))
            {
                TraceFactory.Logger.Info("Successfully configured DNSv6 Secondary IP to:{0}".FormatWith(dnsServerIP));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to configure DNSv6 Secondary IP to:{0}".FormatWith(dnsServerIP));
                return false;
            }
        }

        /// <summary>
        /// Get the status[checked/unchecked] of StatelessDHCPv4 option
        /// </summary>
        /// <returns>true if checked else false</returns>
        public bool GetStatelessDHCPv4()
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Stateless DHCPv4 option is not supported for TPS.");
                return true;
            }

            TraceFactory.Logger.Debug("Getting the checked/unchecked state of StatelessDHCPv4 option");

            _adapter.Navigate("Advanced");
            bool result = _adapter.IsChecked("Use_Stateless_DHCPv4");

            TraceFactory.Logger.Info("StatelessDHCPv4 option is {0}.".FormatWith(result == true ? "enabled" : "disabled"));

            return result;
        }

        /// <summary>
        /// Set the status[checked/unchecked] of StatelessDHCPv4 option
        /// </summary>
        /// <param name="option">true to check, false to uncheck</param>
        public void SetStatelessDHCPv4(bool option)
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Stateless DHCPv4 option is not supported for TPS.");
                return;
            }

            TraceFactory.Logger.Info("Setting the option of StatelessDHCPv4 to {0} state".FormatWith(option));

            _adapter.Navigate("Advanced");
            if (option)
            {
                _adapter.Check("Use_Stateless_DHCPv4");
            }
            else
            {
                _adapter.Uncheck("Use_Stateless_DHCPv4");
            }
            _adapter.Click("Apply");
        }

        /// <summary>
        /// Set the manual ipv6 address of the printer
        /// </summary>
        /// <param name="option">true to enable manual IPv6 address, false to disable.</param>
        /// <param name="IPv6Address">Manual IPv6 address to be set</param>
        public bool SetManualIPv6Address(bool option, string IPv6Address = null)
        {

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Manual Ipv6 address configuration is not supported in TPS. SKIP");
                return true;
            }
            else
            {
                TraceFactory.Logger.Debug("Configuring manual IPv6 address on the printer: {0}".FormatWith(IPv6Address));

            _adapter.Navigate("TCPIPv6");

            if (option)
            {
                _adapter.Check("Enable_Manual_Address");

                _adapter.SetText("ManualIPAddress", IPv6Address.Trim());
                _adapter.SetText("Manual_PrefixLength", "64");
            }
            else
            {
                _adapter.Uncheck("Enable_Manual_Address");
            }

            _adapter.Click("Apply");
            Thread.Sleep(TimeSpan.FromSeconds(20));

                if (_adapter.SearchText("success"))
                {
                    TraceFactory.Logger.Info("Successfully configured Manual ipv6 address of the printer to: {0}".FormatWith(IPv6Address));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to configure Manual ipv6 address of the printer to: {0}".FormatWith(IPv6Address));
                    return false;
                }
            }
        }

        /// <summary>
        /// Get the DNS Suffix List of the Printer
        /// </summary>
        /// <returns>DNS Suffix list configured on the Printer</returns>
        public string GetDNSSuffixList()
        {
            TraceFactory.Logger.Info("Getting the DNS Suffix list of the printer");

            string dnsSuffix = string.Empty;
            _adapter.Navigate("Network_Identification");
            dnsSuffix = _adapter.GetText("DNSSuffixList");

            TraceFactory.Logger.Info("The DNS Suffix List retrieved from the printer: {0}".FormatWith(dnsSuffix));
            return dnsSuffix;
        }

        /// <summary>
        /// Get the DNS Suffix List of the Printer
        /// </summary>
        /// <returns>DNS Suffix list configured on the Printer</returns>
        public List<string> GetDNSSuffixLists()
        {
            TraceFactory.Logger.Info("Getting the DNS Suffix list of the printer");

            string dnsSuffix = string.Empty;
            _adapter.Navigate("Network_Identification");

            return _adapter.GetListItems("DNSSuffixList");
        }

        /// <summary>
        /// Sets the DNS Suffix List of the Printer
        /// </summary>        
        /// Parameter: dnsSuffix: to be set on the Printer  
        public bool SetDNSSuffixList(string dNSSuffix)
        {
            TraceFactory.Logger.Info("Setting the DNS Suffix of the printer manually to: {0}".FormatWith(dNSSuffix));
            _adapter.Navigate("Network_Identification");
            _adapter.SetText("DNS_Suffixe", dNSSuffix);
            _adapter.Click("Add_Suffix");
            Thread.Sleep(TimeSpan.FromSeconds(30));

            if (GetDNSSuffixList().Contains(dNSSuffix))
            {
                TraceFactory.Logger.Info("Successfully configured the DNS Suffix value to:{0}".FormatWith(dNSSuffix));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to configure the DNS Suffix value to:{0}".FormatWith(dNSSuffix));
                return false;
            }
        }

        /// <summary>
        /// Deletes all the DNS suffices
        /// </summary>
        /// <returns>True if all the DNS suffices are deleted, else false.</returns>
        public bool DeleteAllSuffixes()
        {
            // DNS Suffix is not applicable for TPS.
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("DNS Suffix is not applicable for TPS.");
                return true;
            }
            else
            {
                TraceFactory.Logger.Debug("Deleting all DNS suffices.");

                foreach (string item in GetDNSSuffixLists())
                {
                    _adapter.Navigate("Network_Identification");
                    _adapter.Select("DNSSuffixList", item);
                    _adapter.Click("Delete_Suffix");

                    if (!_adapter.SearchText("successfully deleted"))
                    {
                        TraceFactory.Logger.Debug("Failed to delete the DNS suffix: {0}".FormatWith(item));
                        return false;
                    }
                }
                TraceFactory.Logger.Debug("Succesfully Deleted all DNS suffices.");
                return true;
            }
        }

        /// <summary>
        /// Validates the Configured method on the TCPSettings page
        /// </summary>  
        /// Parameter: configMethod for which the method has to validate  
        /// <returns>true if config method is present on the WebUI else false</returns>
        public bool ValidateConfigMethod(string configMethod)
        {
            if (string.IsNullOrEmpty(configMethod))
            {
                TraceFactory.Logger.Info("Config method is not valid.");
                return false;
            }

            TraceFactory.Logger.Info("Validating the configured method:{0} on the Printer".FormatWith(configMethod));

            _adapter.Navigate("IPv4_Config");

            //since the TPS Printer is retrieving the value as prefDHCP for DHCP config method,validating the same with 'contains'
            if (_adapter.GetValue("IP_Configuration_Method").ToLower(CultureInfo.CurrentCulture).Contains(configMethod.ToString().ToLower(CultureInfo.CurrentCulture)))
            {
                TraceFactory.Logger.Info("EWS:Successfully validated the config method:{0} on the Printer WebUI".FormatWith(configMethod));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("EWS:Failed to validated the config method:{0} on the Printer WebUI".FormatWith(configMethod));
                return false;
            }

        }

        /// <summary>
        /// Enable/Disable stateless address
        /// Parameter: option-true/false
        /// </summary>
        public bool SetStatelessAddress(bool option)
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Enable/ Disable stateless address option is not supported for TPS.");
                return true;
            }
            else
            {
                TraceFactory.Logger.Debug("{0} Stateless address option.".FormatWith(option ? "Enabling" : "Disabling"));

                _adapter.Navigate("TCPIPv6");

                if (option)
                {
                    _adapter.Check("Enable_Stateless_Address");
                }
                else
                {
                    _adapter.Uncheck("Enable_Stateless_Address");
                }

                _adapter.Click("Apply");

                Thread.Sleep(TimeSpan.FromSeconds(20));

                if (GetStatelessAddress() == option)
                {
                    TraceFactory.Logger.Info("{0} Stateless address option.".FormatWith(option ? "Enabled" : "Disabled"));
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to {0} Stateless address option.".FormatWith(option ? "enable" : "disable"));
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Get the status[checked/unchecked] of Stateless address checkbox
        /// </summary>
        /// <returns>true if checked else false</returns>
        public bool GetStatelessAddress()
        {
            _adapter.Navigate("TCPIPv6");

            bool result = _adapter.IsChecked("Enable_Stateless_Address");

            TraceFactory.Logger.Info("Stateless address is {0}.".FormatWith(result ? "enabled" : "disabled"));

            return result;
        }

        /// <summary>
        /// Set the status[Enable/Disable] DHCPv6 option 
        /// </summary>
        /// <param name="option">true to enable, false to disable</param>
        public bool SetDHCPv6(bool option)
        {
            // Enable/ disable of DHCPv6 is applicable only for TPS.
            if (_adapter.Settings.ProductType != PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Enable/disable of DHCPv6 is applicable only for TPS.");
                return true;
            }

            TraceFactory.Logger.Debug("{0} the DHCPv6 option through Web UI.".FormatWith(option ? "Enabling" : "Disabling"));

            try
            {
                _adapter.Navigate("Advanced");

                if (option)
                {
                    _adapter.Check("DHCPv6");
                }
                else
                {
                    _adapter.Uncheck("DHCPv6");
                }

                _adapter.Click("Apply");

                Thread.Sleep(TimeSpan.FromSeconds(30));
                ClickonConfirmation();
            }
            catch
            {
                // Do Nothing.
            }
            finally
            {
                StopAdapter();
                _adapter.Start();
            }

            if (GetDHCPv6().Equals(option))
            {
                TraceFactory.Logger.Info("Successfully {0} DHCPv6 option from web UI.".FormatWith(option ? "enabled" : "disabled"));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to {0} DHCPv6 option from web UI.".FormatWith(option ? "enable" : "disable"));
                return false;
            }
        }

        /// <summary>
        /// Get the status of DHCPv6 option status from Web UI.
        /// </summary>
        /// <returns>True if the option is enabled, else false.</returns>
        public bool GetDHCPv6()
        {
            // Enable/ disable of DHCPv6 is applicable only for TPS.
            if (_adapter.Settings.ProductType != PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Enable/disable of DHCPv6 is applicable only for TPS.");
                return true;
            }

            TraceFactory.Logger.Debug("Getting the DHCPv6 status through Web UI.");

            _adapter.Navigate("Advanced");

            return _adapter.IsChecked("DHCPv6");
        }

        /// <summary>
        /// Set the status[Enable/Disable] of 'Always perform DHCPv6 on the startup' option 
        /// </summary>
        /// <param name="option">true to enable, false to disable</param>
        public bool SetDHCPv6OnStartup(bool option)
        {
            // 'Always perform DHCPv6 on the startup' is supported only for VEP/LFP
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS || _adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                TraceFactory.Logger.Debug("Option Always perform DHCPv6 on the startup not available for TPS and InkJet.");
                return true;
            }

            TraceFactory.Logger.Info("{0} the option 'Always perform DHCPv6 on the startup'.".FormatWith(option ? "Enabling" : "Disabling"));

            _adapter.Navigate("TCPIPv6");

            if (option)
            {
                _adapter.Check("Perform_DHCP_Startup");
            }
            else
            {
                _adapter.Uncheck("Perform_DHCP_Startup");
            }

            _adapter.Click("Apply");
            Thread.Sleep(TimeSpan.FromSeconds(30));

            if (_adapter.SearchText("success"))
            {
                TraceFactory.Logger.Info("Successfully set the option 'Always perform DHCPv6 on the startup' to {0} state".FormatWith(option ? "enable" : "disable"));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to set the option 'Always perform DHCPv6 on the startup' to {0} state".FormatWith(option ? "enable" : "disable"));
                return false;
            }
        }

        /// <summary>
        /// Get the status[Enable/Disable] of 'Perform DHCPV6 when requested by router' option  
        /// </summary>
        /// <returns>true if enabled, false if disabled</returns>
        public bool GetDHCPV6Router()
        {
            TraceFactory.Logger.Debug("Getting the option status of 'Perform DHCPV6 when requested by router'");

            _adapter.Navigate("TCPIPv6");
            bool result = _adapter.IsChecked("Perform_DHCPv6_Router");

            TraceFactory.Logger.Debug("The Current status of the option 'Perform DHCPV6 when requested by router' is {0}".FormatWith(result));
            return result;
        }

        /// <summary>
        /// Set the status[Enable/Disable] of 'Perform DHCPV6 when requested by router' option 
        /// </summary>
        /// <returns>true when factory defaults is success and false when it is unsuccessful</returns>
        public bool SetFactoryDefaults(bool validation = true)
        {
            TraceFactory.Logger.Debug("Performing Restore Factory defaults as cold reset option is not available for Inkjet");
            try
            {
                StopAdapter();
                _adapter.Start();

            _adapter.Navigate("FactoryDefaults");
            _adapter.Click("RestoreFactoryDefaults");
            _adapter.Click("yes");

            _adapter.ClickOkonAlert();

            Thread.Sleep(TimeSpan.FromMinutes(1));

            bool result = false;
            if (validation)
            {
                if (_adapter.SearchText("success"))
                {
                    TraceFactory.Logger.Info("Successfully performed the restore factory defaults");
                    result = true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to perform restore factory defaults");
                    result = false;
                }
            }
            else
            {
                result = true;
            }

                Thread.Sleep(TimeSpan.FromMinutes(3));
                return result;
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Exception occured in Factory default :{0} ".FormatWith(ex.Message));
                return false;
            }
        }


        /// <summary>
        /// Performing Network defaults only for Ink printer as Factory defaults makes the printer hang/unaccessible. 
        /// </summary>
        /// <returns>true when Network defaults is success and false when it is unsuccessful</returns>
        public bool SetNetworkDefaults()
        {
            TraceFactory.Logger.Debug("Performing Restore Network defaults as cold reset option is not available for Inkjet");
            _adapter.Stop();
            _adapter.Start();

            _adapter.Navigate("NetworkDefaults");
            _adapter.Click("RestoreNetworkDefaults");
            _adapter.Click("yes");

            _adapter.ClickOkonAlert();

            Thread.Sleep(TimeSpan.FromMinutes(1));

            bool result = false;
            if (_adapter.SearchText("success"))
            {
                TraceFactory.Logger.Info("Successfully performed the restore Network defaults");
                result = true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to perform restore Network defaults");
                result = false;
            }

            Thread.Sleep(TimeSpan.FromMinutes(5));

            return result;
        }

        /// <summary>
        /// Set the status[Enable/Disable] of 'Perform DHCPV6 when requested by router' option 
        /// </summary>
        /// <param name="option">true to enable, false to disable</param>
        public void SetDHCPv6Router(bool option)
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Perform DHCPV6 when requested by router option is not available for TPS.");
                return;
            }

            TraceFactory.Logger.Info("Setting the option of 'Perform DHCPV6 when requested by router' to {0} state".FormatWith(option));
            if (GetDHCPV6Router().Equals(false))
            {
                if (option)
                {
                    _adapter.Navigate("TCPIPv6");
                    _adapter.Click("Perform_DHCPv6_Router");
                    _adapter.Click("Apply");
                    Thread.Sleep(TimeSpan.FromSeconds(20));
                    TraceFactory.Logger.Info("Successfully set the option 'Perform DHCPV6 when requested by router' to enable state");
                }
            }
            else
            {
                if (!option)
                {
                    _adapter.Navigate("TCPIPv6");
                    _adapter.Click("Perform_DHCPv6_Router");
                    _adapter.Click("Apply");
                    Thread.Sleep(TimeSpan.FromSeconds(20));
                    TraceFactory.Logger.Info("Successfully set the option 'Perform DHCPV6 when requested by router' to disable state");
                }
            }
        }

        /// <summary>
        /// Get the status[Enable/Disable] of Wireless option 
        /// </summary>
        /// <returns>true if enabled, false if disabled</returns>
        public bool GetWireless()
        {
            TraceFactory.Logger.Debug("Getting the Wireless Status of the Printer");

            _adapter.Navigate("Wireless_Config");
            bool result = _adapter.IsChecked("WirelessEnable");

            TraceFactory.Logger.Debug("The Current status of the Wireless Option is {0}".FormatWith(result));
            return result;
        }

        /// <summary>
        /// Reset the config precedence value
        /// </summary>        
        public bool ResetConfigPrecedence()
        {
            TraceFactory.Logger.Debug("Resetting the config precedence values to default");

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Since TPS is not refreshing the page after resetting config precedence,closing and opening browser");

                _adapter.Stop();
                _adapter.Start();

                _adapter.Navigate("Network_Identification");
                _adapter.Click("Restore_defaults");
                _adapter.ClickOkonAlert();
                Thread.Sleep(TimeSpan.FromMinutes(1));

                // Browser becomes inactive and goes to infinite loop; trying to stop adapter and kill explicitly 
                StopAdapter();
                _adapter.Start();

                TraceFactory.Logger.Info("Successfully the config precedence values are set to default");
                return true;
            }
            else
            {
                _adapter.Navigate("Config_Preference");
                Thread.Sleep(TimeSpan.FromSeconds(20));
                _adapter.Click("Reset_default");
                Thread.Sleep(TimeSpan.FromSeconds(20));
                if (_adapter.SearchText("success"))
                {
                    TraceFactory.Logger.Info("Successfully the config precedence values are set to default");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to set config precedence values to default through webui");
                    return false;
                }
            }
        }

        /// <summary>
        /// Get the config precedence value
        /// </summary>        
        public string GetConfigPrecedence(bool returnHighestPrecedence = true)
        {
            TraceFactory.Logger.Debug("Getting the config precedence values through EWS");

            string[] config;
            string configValue = string.Empty;
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Network_Identification");
                Thread.Sleep(TimeSpan.FromSeconds(20));
                config = _adapter.GetListItems("Configuration_Precedence").ToArray();
                configValue = config.GetValue(0).ToString();
                TraceFactory.Logger.Info("The highest config precedence set on the Printer retrieved from EWS:{0}".FormatWith(configValue));
            }
            else
            {
                _adapter.Navigate("Config_Preference");
                Thread.Sleep(TimeSpan.FromSeconds(20));
                config = _adapter.GetListItems("Configuration_Methods").ToArray();
                configValue = config.GetValue(0).ToString();
                TraceFactory.Logger.Info("The highest config precedence set on the Printer retrieved from EWS:{0}".FormatWith(configValue));
            }

            if (!returnHighestPrecedence)
            {
                configValue = string.Join(":", config);
            }

            return configValue;
        }

        /// <summary>
        /// Reinitializing the config precedence table for VEP
        /// </summary>  
        public bool ReinitializeConfigPrecedence()
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Reinitialize Now option is not supported.");
                return true;
            }

            TraceFactory.Logger.Debug("Click Reinitialize Now in config precedence tab.");
            // TODO: Need to check if delay of 20 seconds is required
            _adapter.Navigate("Config_Preference");
            Thread.Sleep(TimeSpan.FromSeconds(20));
            _adapter.Click("Reinitialize");

            Thread.Sleep(TimeSpan.FromMinutes(3));

            if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                TraceFactory.Logger.Info("Delay of 3 minuttes for VEP.");
                Thread.Sleep(TimeSpan.FromMinutes(3));
            }

            if (_adapter.SearchText("success"))
            {
                TraceFactory.Logger.Info("Successfully Reinitialized the config precedence.");
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to Reinitialized the config precedence.");
                return false;
            }
        }

        /// <summary>
        /// Clear and Reinitializing the config precedence table for VEP
        /// </summary>  
        public bool ClearAndReinitializeConfigPrecedence()
        {
            TraceFactory.Logger.Info("Click Clear and Reinitialize Now in config precedence tab");
            // TODO: Need to check if delay of 20 seconds is required
            _adapter.Navigate("Config_Preference");
            Thread.Sleep(TimeSpan.FromSeconds(20));
            _adapter.Click("Clear_And_Reinitialize");

            Thread.Sleep(TimeSpan.FromMinutes(1));

            if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                TraceFactory.Logger.Info("Delay of 3 minuttes for VEP.");
                Thread.Sleep(TimeSpan.FromMinutes(3));
            }

            if (_adapter.SearchText("success"))
            {
                TraceFactory.Logger.Info("Successfully Reinitialized the config precedence");
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to Reinitialized the config precedence");
                return false;
            }
        }
        ///<summary>
        ///Restore Factory defaults from EWS only For INK
        ///</summary>
        
        public void RestoreDefaultsINK()
        {
            try
            {
                StopAdapter();
                _adapter.Start();
                _adapter.Navigate("RestoreFactoryDefaults_INK");
                _adapter.Click("RestoreFactoryDefaults");
                if (_adapter.IsElementPresent("Yes"))
                {
                    TraceFactory.Logger.Info("Confirmatio Pop up present");
                    _adapter.Click("Yes");
                }
                Thread.Sleep(TimeSpan.FromMinutes(2));
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Exception : {0}".FormatWith(ex.Message));

            }
            finally
            {
                _adapter.IsElementPresent("ok");
                _adapter.Click("ok");
                Thread.Sleep(TimeSpan.FromSeconds(20));
            }

        }
        /// <summary>
        /// Update adapter device address
        /// </summary>
        public void ChangeDeviceAddress(IPAddress newPrinterIP)
        {
            ChangeDeviceAddress(newPrinterIP.ToString());
        }

        /// <summary>
        /// Update adapter device address
        /// </summary>
        public void ChangeDeviceAddress(string newPrinterIP)
        {
            if (null == newPrinterIP)
            {
                TraceFactory.Logger.Info("Printer IP Address can not be null.");
                return;
            }

            if (newPrinterIP.Contains(':'))
            {
                newPrinterIP = string.Concat("[", newPrinterIP, "]");
            }

            _adapter.Settings.DeviceAddress = newPrinterIP;
        }

        /// <summary>
        /// Update the adapter hostname
        /// Can be used when the EWS needs to be opened with host name.
        /// </summary>
        public void ChangeHostName(string hostName)
        {
            if (string.IsNullOrEmpty(hostName))
            {
                TraceFactory.Logger.Info("host name can not be null or empty.");
                return;
            }

            _adapter.Settings.DeviceAddress = hostName;
        }

        /// <summary>
        /// Validates the manual IPv6 address on the printer
        /// </summary>
        /// <param name="manualIpv6Address">Manual IPv6 Address to be validated</param>
        /// <returns>true if the given manual IPv6 is configured, else false</returns>
        public bool ValidateManualIpv6Address(string manualIpv6Address)
        {
            TraceFactory.Logger.Info("Validating Manual IPv6 Address through Web UI");
            _adapter.Navigate("Configuration_Page");
            if (_adapter.SearchText(manualIpv6Address))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set option "Send DHCP requests if IP address is Auto IP"
        /// Note: Applicable only to VEP/ LFP
        /// </summary>
        /// <param name="checkOption">true for setting the option, false otherwise</param>
        public bool SetSendDHCPRequestOnAutoIP(bool checkOption)
        {
            if (!(PrinterFamilies.TPS == _adapter.Settings.ProductType || PrinterFamilies.InkJet == _adapter.Settings.ProductType))
            {
                TraceFactory.Logger.Debug("Setting 'Send DHCP Request on Auto IP' option to {0}.".FormatWith(checkOption ? "checked" : "uncheck"));
				_adapter.Navigate("Advanced");
                Thread.Sleep(TimeSpan.FromMinutes(1));
                if (checkOption)
                {
                    _adapter.Check("Send_DHCP_Requests");
                }
                else
                {
                    _adapter.Uncheck("Send_DHCP_Requests");
                }

                _adapter.Click("Apply");

				Thread.Sleep(TimeSpan.FromSeconds(20));

				if (!GetSendDHCPRequestOnAutoIP().Equals(checkOption))
				{
					TraceFactory.Logger.Info("Failed to {0} the 'Send DHCP Request if IP address is Auto IP' from Web UI.".FormatWith(checkOption ? "enable" : "disable"));
					return false;
				}

				TraceFactory.Logger.Info("Send DHCP Request if IP address is Auto IP option is {0}.".FormatWith(checkOption ? "enabled" : "disabled"));
			}

            return true;
        }

		/// <summary>
		/// Get the option status "Send DHCP requests if IP address is Auto IP"
		/// </summary>
		/// <returns>true if the option is enabled, else false</returns>
		public bool GetSendDHCPRequestOnAutoIP()
		{
            StopAdapter();
            _adapter.Start();
            _adapter.Navigate("Advanced");
            Thread.Sleep(TimeSpan.FromMinutes(1));
			return _adapter.IsChecked("Send_DHCP_Requests");
		}

        /// <summary>
        /// Get Default IP Type : Applicable only for VEP/ LFP
        /// </summary>
        /// <returns><see cref=" DefaultIPType"/></returns> 
        public DefaultIPType GetDefaultIPType()
        {
            _adapter.Navigate("Advanced");
            string defaultIP = _adapter.GetValue("Default_IP");

            return defaultIP.EqualsIgnoreCase(Enum<DefaultIPType>.Value(DefaultIPType.LegacyIP)) ? DefaultIPType.LegacyIP : DefaultIPType.AutoIP;
        }

        /// <summary>
        /// Set Default IP Type : Applicable only for VEP/ LFP
        /// </summary>
        /// <param name="defaultIPType"><see cref=" DefaultIPType"/></param>
        /// <returns>true if configuration successful, false otherwise</returns>
        public bool SetDefaultIPType(DefaultIPType defaultIPType)
        {
            // Default IP type is supported only for VEP/LFP
			if (_adapter.Settings.ProductType== PrinterFamilies.TPS || _adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Setting Default IP Type to: {0}".FormatWith(defaultIPType.ToString()));

                _adapter.Navigate("Advanced");
                _adapter.SelectByValue("Default_IP", Enum<DefaultIPType>.Value(defaultIPType));
                _adapter.Click("Apply");

                Thread.Sleep(TimeSpan.FromMinutes(3));
                // TODO : Validate success message on the EWS page and return true or false.

                StopAdapter();
                _adapter.Start();

                if (GetDefaultIPType() == defaultIPType)
                {
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to set the default type to {0}".FormatWith(defaultIPType));
                    return false;
                }
            }
        }

        /// <summary>
        /// Get Configured IPv4 Address of Printer
        /// This is mainly required when only printer hostname is known/ validating printer configured IPv4 address
        /// </summary>
        /// <returns>IPv4 Address of Printer</returns>
        public string GetIPv4Address()
        {
            _adapter.Navigate("IPv4_Config");
            // If printer is configured with DHCP/ Bootp configuration method, IP address field will be disabled.
            // Changing configuration method to Manual to get IP Address.
            _adapter.SelectDropDown("IP_Configuration_Method", IPConfigMethod.Manual.ToString());
            return _adapter.GetValue("IP_Address");
        }

        /// <summary>
        /// Validates if the text 'UNABLE TO CONNECT DHCP SVR' is found in the configuration page.
        /// Note : When the printer goes to partial configuration, Configuration page displays the text 'UNABLE TO CONNECT DHCP SVR'.
        /// </summary>
        /// <returns>True if the text 'UNABLE TO CONNECT DHCP SVR' is found in the configuration page, else false.</returns>
        public bool ValidatePartialConfiguration()
        {
            if (EwsWrapper.Instance().SearchTextInPage("UNABLE TO CONNECT DHCP SVR", "Configuration_Page"))
            {
                TraceFactory.Logger.Info("Text: 'UNABLE TO CONNECT DHCP SVR' is found in EWS Configuration page.");
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Text: 'UNABLE TO CONNECT DHCP SVR' is not found in EWS Configuration page.");
                return false;
            }
        }

        // TODO: Implement single function to handle different DHCPv6 options
        /// <summary>
        /// Set 'Perform DHCPV6 when stateless configuration is unsuccessful or disabled' option
        /// </summary>
        public void SetDHCPv6StatelessConfigurationOption()
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Option not supported for TPS.");
                return;
            }
            else
            {
                _adapter.Navigate("TCPIPv6");
                _adapter.Click("Perform_DHCPv6_State");
                _adapter.Click("Apply");

                TraceFactory.Logger.Info("DHCPv6 Addresses is set to 'Perform DHCPV6 when stateless configuration is unsuccessful or disabled' option.");
            }
        }

        /// <summary>
        /// Get IPv6 Table details
        /// </summary>
        /// <returns>Collection of<see cref=" IPv6Details"/></returns>
        public Collection<IPv6Details> GetIPv6TableDetails()
        {
            Collection<IPv6Details> ipv6TableDetails = new Collection<IPv6Details>();

            Collection<Collection<string>> ipv6TableData;

            // For TPS, since there is no header element; selenium throws up an exception for 'Element Not Found', discarding header details
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("IPv6_Config");
                ipv6TableData = _adapter.GetTable("IPv6AddrLst", false, returnValue: false);
            }
            else
            {
                _adapter.Navigate("Summary");
                ipv6TableData = _adapter.GetTable("IPv6AddrLst", returnValue: false);
            }

            Collection<string> ipv6TableHeader = ipv6TableData[0];

            // Column arrangement of IPv6 table differs w.r.t VEP and TPS; getting column numbers for fetching right data
            int ipv6AddressIndex, prefixLengthIndex, configMethodIndex, validLifetimeIndex, preferredLifetimeIndex;

            ipv6AddressIndex = ipv6TableHeader.IndexOf(ipv6TableHeader.Where(x => x.StartsWith("IPv6", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            prefixLengthIndex = ipv6TableHeader.IndexOf(ipv6TableHeader.Where(x => x.StartsWith("Prefix", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            configMethodIndex = ipv6TableHeader.IndexOf(ipv6TableHeader.Where(x => x.StartsWith("Config", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            validLifetimeIndex = ipv6TableHeader.IndexOf(ipv6TableHeader.Where(x => x.StartsWith("Valid", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());
            preferredLifetimeIndex = ipv6TableHeader.IndexOf(ipv6TableHeader.Where(x => x.StartsWith("Preferred", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault());

            // Header information is not required while gathering IPv6 table details
            ipv6TableData.RemoveAt(0);

            foreach (Collection<string> ipv6Detail in ipv6TableData)
            {
                // If an empty row is returned/ invalid data; discard the row
                if ((ipv6Detail.Count != ipv6TableHeader.Count) || string.IsNullOrEmpty(ipv6Detail[ipv6AddressIndex].Trim()))
                {
                    continue;
                }

                IPv6Details ipv6Table = new IPv6Details();
                ipv6Table.IPv6Address = IPAddress.Parse(ipv6Detail[ipv6AddressIndex]);
                ipv6Table.PrefixLength = int.Parse(ipv6Detail[prefixLengthIndex], CultureInfo.InvariantCulture);
                ipv6Table.ConfigMethod = Enum<IPv6ConfigMethod>.Parse(ipv6Detail[configMethodIndex]);
                ipv6Table.ValidLifetime = GetTime(ipv6Detail[validLifetimeIndex]);
                ipv6Table.PreferredLifetime = GetTime(ipv6Detail[preferredLifetimeIndex]);

                ipv6TableDetails.Add(ipv6Table);
            }

            return ipv6TableDetails;
        }

        /// <summary>
        /// Fetch the IPv6 Table details for a particular address count.
        /// If the address count is not matching fetch the IPv6 details every 30 seconds and verify the count for 5 minutes.
        /// </summary>
        /// <param name="addressCount">the number of addresses to be fetched.</param>
        /// <param name="disableEnableIPv6Option">if set to <c>true</c> [disable enable i PV6 option].</param>
        /// <returns>Collection of<see cref=" IPv6Details" /></returns>
        public Collection<IPv6Details> GetIPv6TableDetails(int addressCount = 0, bool disableEnableIPv6Option = true)
        {
            // Get IPv6 option status
            bool ipv6OptionStatus = EwsWrapper.Instance().GetIPv6();

            // Performing disable/ enable only when IPv6 is enabled and disableEnableIPv6Option is true
            // When IPv6 option is disabled; the state of IPv6 option will be changed if we perform disable/ enable, hence not disable/ enabling
            if (_adapter.Settings.ProductType != PrinterFamilies.TPS && ipv6OptionStatus && disableEnableIPv6Option)
            {
                EwsWrapper.Instance().SetIPv6(false);
                EwsWrapper.Instance().SetIPv6(true);
            }

            // Wait for 1 minute to get the IPv6 table details updated.
            Thread.Sleep(TimeSpan.FromMinutes(1));

            TimeSpan duration = new TimeSpan(0, 10, 0);
            DateTime startTime = DateTime.Now;

            Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails();

            // When IPv6 is disabled, directly return the table details.
            if (!ipv6OptionStatus || addressCount == 0)
            {
                return ipv6Details;
            }

            // Fetch the IPv6 Table Details from the summary page. If the count matches then the Table details will be returned, else repeat the same for 5 minutes with an interval of 30 seconds.
            while (ipv6Details.Count != addressCount && DateTime.Now.Subtract(startTime).TotalSeconds <= duration.TotalSeconds)
            {
                // Wait for 30 seconds to get the IPv6 table details updated.
                Thread.Sleep(TimeSpan.FromSeconds(30));

                // This is done for refreshing the page. Navigating to the page doesn't refresh the Summary page.
                _adapter.Navigate("IPv4_Config");

                ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails();
            }


            return ipv6Details;
        }

        /// <summary>
        /// Get Telnet Option status
        /// </summary>
        public bool GetTelnet()
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Advanced");

                // TODO: Modify 'TelnetConfig' to 'Telnet_Config' in code and sitemaps to maintain consistentcy across products
                if (_adapter.IsElementPresent("TelnetConfig"))
                {
                    return (_adapter.IsChecked("TelnetConfig"));
                }
                else
                {
                    return GetHiddenOption("Telnet_Config");
                }
            }
            else
            {
                _adapter.Navigate("Misc_Settings");
                return (_adapter.IsChecked("Telnet_Config"));
            }
        }

        /// <summary>
        /// Get Bonjour Service Name
        /// </summary>
        public string GetBonjourServiceName()
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Stop();
                _adapter.Start();
                _adapter.Navigate("Bonjour");
            }
            else
            {
                _adapter.Navigate("Network_Identification");
            }

            string bonjourServiceName = _adapter.GetValue("Bonjour_Service_Name");
            TraceFactory.Logger.Info("Bonjour Service Name is: {0}".FormatWith(bonjourServiceName));
            return bonjourServiceName;
        }

        /// <summary>
        /// Set Telnet Option to checked
        /// </summary>
        public bool SetTelnet()
        {
            try
            {
                TraceFactory.Logger.Info("Enabling telnet through Web UI.");

                if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                {
                    _adapter.Navigate("Advanced");
                    if (_adapter.IsElementPresent("TelnetConfig"))
                    {
                        // TODO: Modify 'TelnetConfig' to 'Telnet_Config' in code and sitemaps to maintain consistentcy across products
                        _adapter.Check("TelnetConfig");
                        _adapter.Click("Apply");
                        ClickonConfirmation();
                    }
                    else
                    {
                        SetHiddenOption("Telnet_Config", true);
                    }
                }
                else
                {
                    _adapter.Navigate("Misc_Settings");
                    _adapter.Check("Telnet_Config");
                    _adapter.Click("Apply_Misc");
                }
            }
            catch
            {
                // Do nothing
            }
            finally
            {
                // Browser becomes inactive and goes to infinite loop; trying to stop adapter and kill explicitly 
                StopAdapter();
                _adapter.Start();
            }

            return GetTelnet();
        }

        /// <summary>
        /// Check/ Uncheck Arp Ping option
        /// Note: Applicable only for TPS
        /// </summary>
        /// <param name="state"></param>
        public void SetArp(bool state)
        {
            if (_adapter.Settings.ProductType != PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Setting Arp Option is not supported in TPS");
                return;
            }

            TraceFactory.Logger.Info("{0} arp ping option.".FormatWith(state ? "Enabling" : "Disabling"));
            _adapter.Navigate("Advanced");

            if (state)
            {
                if (_adapter.IsElementPresent("ARPPing"))
                {
                    _adapter.Check("ARPPing");
                    _adapter.Click("Apply");
                    ClickonConfirmation();
                }
                else
                {
                    SetHiddenOption("ARPPing", true);
                }
            }
            else
            {
                if (_adapter.IsElementPresent("ARPPing"))
                {
                    _adapter.Uncheck("ARPPing");
                    _adapter.Click("Apply");
                    ClickonConfirmation();
                }
                else
                {
                    SetHiddenOption("ARPPing", false);
                }
            }
        }

        /// <summary>
        /// Get v6 Domain name
        /// </summary>
        /// <returns></returns>
        public string Getv6DomainName()
        {
            _adapter.Navigate("Network_Identification");
            return _adapter.GetValue("DomainName_IPv6").Trim();
        }

        /// <summary>
        /// Set Tftp option
        /// Note: Applicable only for TPS
        /// </summary>
        /// <param name="state"></param>
        public void SetTftp(bool state)
        {
            if (_adapter.Settings.ProductType != PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Setting Tftp Option is not supported in TPS");
                return;
            }

            _adapter.Navigate("Advanced");

            if (state)
            {
                if (_adapter.IsElementPresent("TFTP_File"))
                {
                    _adapter.Check("TFTP_File");
                    _adapter.Click("Apply");
                    ClickonConfirmation();
                }
                else
                {
                    SetHiddenOption("TFTP_File", true);
                }
            }
            else
            {
                if (_adapter.IsElementPresent("TFTP_File"))
                {
                    _adapter.Uncheck("TFTP_File");
                    _adapter.Click("Apply");
                    ClickonConfirmation();
                }
                else
                {
                    SetHiddenOption("TFTP_File", false);
                }
            }
        }

        /// <summary>
        /// Click on 'Wake Up' button
        /// </summary>
        public void WakeUpPrinter()
        {
            if (_adapter.Settings.ProductType != PrinterFamilies.LFP)
            {
                TraceFactory.Logger.Debug("Wake Up via EWS is currently applicable to LFP products");
                return;
            }

            _adapter.Navigate("Sleep");

            if (_adapter.IsElementPresent("WakeUp"))
            {
                _adapter.Click("WakeUp");
                Thread.Sleep(TimeSpan.FromMinutes(1));
                StopAdapter();
                _adapter.Start();
            }
        }

        /// <summary>
        /// Enable DDNS option
        /// </summary>
        public void SetDDNS()
        {
            TraceFactory.Logger.Info("Enabling DDNS option.");

            if (GetDDNS())
            {
                _adapter.Uncheck("EnableDDNS");
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(2));
                SetDDNS();
            }
            else
            {
                _adapter.Navigate("Network_Identification");
                _adapter.Check("EnableDDNS");
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
        }

        //TODO: Remove SetDDNSOption 
        /// <summary>
        /// Enable/ Disable DDNS option through web UI.
        /// </summary>
        /// <param name="enable">true to enable, false to disable DDNS.</param>
        /// <returns>true if the value is set.</returns>
        public bool SetDDNS(bool enable)
        {
            TraceFactory.Logger.Info("{0} DDNS option through web UI.".FormatWith(enable ? "Enabling" : "Disabling"));

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Info("Setting DDNS Option is not supported in TPS");
                return true;
            }
            else
            {
                _adapter.Navigate("Network_Identification");

                if (enable)
                {
                    _adapter.Check("EnableDDNS");
                }
                else
                {
                    _adapter.Uncheck("EnableDDNS");
                }

                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(10));

                if (GetDDNS().Equals(enable))
                {
                    TraceFactory.Logger.Info("Successfully {0} DDNS option through web UI.".FormatWith(enable ? "enabled" : "disabled"));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Successfully {0} DDNS option through web UI.".FormatWith(enable ? "enable" : "disable"));
                    return false;
                }
            }
        }

        /// <summary>
        /// Check if DDNS Option is checked or not
        /// </summary>
        /// <returns>true is option checked, false otherwise</returns>
        public bool GetDDNS()
        {
            _adapter.Navigate("Network_Identification");
            return _adapter.IsChecked("EnableDDNS");
        }

        /// <summary>
        /// Set RFC 4702 Option
        /// </summary>
        /// <param name="option">true if option needs to be checked, false for unchecking</param>
        public void SetRfc(bool option)
        {
            TraceFactory.Logger.Info("{0} RFC Option.".FormatWith(option == true ? "Enabling" : "Disabling"));

            try
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    StopAdapter();
                    _adapter.Start();
                    _adapter.Navigate("NetworkAdvanced_DHCP", "https");
                }
                else
                {
                    _adapter.Navigate("Advanced", "https");
                }
				if (option)
                {
                    _adapter.Check("Enable_DHCPv4_FQDN_Compliance");
                }
                else
                {
                    _adapter.Uncheck("Enable_DHCPv4_FQDN_Compliance");
                }

                _adapter.Click("Apply");
                _adapter.ClickOkonAlert();
            }
            catch
            {
                // Do nothing
            }
            finally
            {
                if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    if (_adapter.IsElementPresent("ok"))
                    {
                        _adapter.Click("ok");
                    }
                    TraceFactory.Logger.Info("Successfully updated the values");
                }
                if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                {
                    StopAdapter();
                    _adapter.Start();
                }
            }

        }

        /// <summary>
        /// Get RFC 4702 Option
        /// </summary>		
        public bool GetRfc()
        {
            if(_adapter.Settings.ProductType == PrinterFamilies.InkJet)
                {
                    StopAdapter();
                    _adapter.Start();
                    _adapter.Navigate("NetworkAdvanced_DHCP", "https");
                }
                else
                {
                    _adapter.Navigate("Advanced", "https");
                }
            TraceFactory.Logger.Debug("RFC Option: {0}".FormatWith(_adapter.IsChecked("Enable_DHCPv4_FQDN_Compliance")));
            return _adapter.IsChecked("Enable_DHCPv4_FQDN_Compliance");
        }

        /// <summary>
        /// Get DNS Server IP
        /// </summary> 
        /// <returns>DNS Server IP Address</returns>
        public string GetDNSServerIP()
        {
            _adapter.Navigate("Network_Identification");
            string result = _adapter.GetText("DNS_IPv4Primary");

            //For TPS Printers Primary WinServerIP field value is "Not Specified" for Empty string
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                if (result == "Not Specified")
                {
                    return string.Empty;
                }
                else
                {
                    return result;
                }
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Set NTS Address
        /// </summary>
        /// <param name="address">NTS Addres to be set</param>
        public void SetNtsAddress(string address)
        {
            TraceFactory.Logger.Info("Setting NTS Address to {0}".FormatWith(address));

            try
            {
                _adapter.Navigate("General");
                Thread.Sleep(TimeSpan.FromSeconds(10));
                _adapter.Click("NtsSettings");
                _adapter.SetText("NTSAddress", address);
                Thread.Sleep(TimeSpan.FromSeconds(10));
                _adapter.Click("SyncNow");

                Thread.Sleep(TimeSpan.FromMinutes(3));
            }
            catch
            {
                // Do Nothing
            }
            finally
            {
                StopAdapter();
                _adapter.Start();
            }
        }

        /// <summary>
        /// Get Primary Wins Server IP
        /// </summary>
        /// <returns>Primary Wins Server IP Address</returns>
        public string GetPrimaryWinServerIP()
        {
            _adapter.Navigate("Network_Identification");
            string result = _adapter.GetValue("Primary_IPv4");
                TraceFactory.Logger.Info("Wins Ip address is : {0}".FormatWith(result));
            //For TPS Printers Primary WinServerIP field value is "Not Specified" for Empty string
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                if (result == "Not Specified")
                {
                    return string.Empty;
                }
                else
                {
                    return result;
                }
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Set Primary Wins Server IP
        /// </summary>
        /// <param name="serverIPAddress">Wins Server IP to be set</param>
        public bool SetPrimaryWinServerIP(string serverIPAddress)
        {
            TraceFactory.Logger.Debug("Setting Primary Win Server IP: {0} through Web UI.".FormatWith(serverIPAddress));

            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                if (serverIPAddress == string.Empty)
                {
                    serverIPAddress = "0.0.0.0";
                }
                StopAdapter();
                _adapter.Start();
                _adapter.Navigate("WINS");
                if (_adapter.IsElementPresent("WINS_Option_enable"))
                {
                    _adapter.Click("WINS_Option_enable");
                    //MessageBox.Show("Enabled");
                    TraceFactory.Logger.Info("Setting WINS server IP address to : {0}" .FormatWith(serverIPAddress));
                    string winsAddressControl = "WINS_Primary_IP_";
                    for (int i = 0; i < 4; i++)
                    {
                        _adapter.SetText("{0}{1}".FormatWith(winsAddressControl, i), serverIPAddress.ToString().Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[i]);
                    }
                }
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(5));
                if (_adapter.SearchText("The changes have been updated successfully."))
                {
                    TraceFactory.Logger.Info("Configured Primary WINS server IP");
                    _adapter.Click("OK");
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
            }
            else
            {
                if (serverIPAddress == string.Empty && _adapter.Settings.ProductType.Equals(PrinterFamilies.TPS))
                {
                    TraceFactory.Logger.Info("TPS does");
                    ResetConfigPrecedence();
                }
            _adapter.Navigate("Network_Identification");
            _adapter.SetText("Primary_IPv4", serverIPAddress);
            _adapter.Click("Apply");
            _adapter.ClickOkonAlert();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            }
            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                TraceFactory.Logger.Info("Successfully set value.");
                return true;
            }
            else
            { 
            if (serverIPAddress.EqualsIgnoreCase(GetPrimaryWinServerIP()))
            {
                TraceFactory.Logger.Info("Successfully set the primary wins server IP: {0} through Web UI.".FormatWith(serverIPAddress));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to set the primary wins server IP: {0} through Web UI.".FormatWith(serverIPAddress));
                return false;
                }
            }
        }

        /// <summary>
        /// Set Bonjour Service Name
        /// </summary>
        /// <param name="bonjourServiceName">Bonjour Service Name to be set</param>
        public bool SetBonjourServiceName(string bonjourServiceName)
        {
            TraceFactory.Logger.Info("Setting Bonjour Service Name: {0} through EWS.".FormatWith(bonjourServiceName));

            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                _adapter.Stop();
                _adapter.Start();
                _adapter.Navigate("Bonjour");
            }
            else
            {
                _adapter.Navigate("Network_Identification");
            }

            _adapter.SetText("Bonjour_Service_Name", bonjourServiceName);
            _adapter.Click("Apply");
            _adapter.ClickOkonAlert();

			Thread.Sleep(TimeSpan.FromSeconds(30));
            if (bonjourServiceName != string.Empty)
            {
                if (bonjourServiceName==null && SearchTextInPage("Setting the Bonjour Service Name failed"))
                {
                    TraceFactory.Logger.Info("Setting Blank name failed. AS Expected");
                    return true;
                }

                if (bonjourServiceName.EqualsIgnoreCase(GetBonjourServiceName()))
                {
                    TraceFactory.Logger.Info("Successfully set the Bonjour Service Name to {0} through EWS.".FormatWith(bonjourServiceName));
                    return true;

                }
                else
                {
                    TraceFactory.Logger.Info("Failed to set the Bonjour Service Name to {0} through EWS.".FormatWith(bonjourServiceName));
                    return false;
                }
            }
            if (bonjourServiceName == string.Empty)
            {
                if (SearchTextInPage("Setting the Bonjour Service Name failed"))
                {
                    TraceFactory.Logger.Info("Printer did not get configured with Blank Bonjour Service name");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get Secondary Wins Server IP
        /// </summary>
        /// <returns>Secondary Wins Server IP Address</returns>
        public string GetSecondaryWinServerIP()
		{
            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                StopAdapter();
                _adapter.Start();
                _adapter.Navigate("Wired_Status");
            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                _adapter.Navigate("Network_Identification");
            }
           
			return _adapter.GetValue("Secondary_IPv4");
		}

        /// <summary>
        /// Set Secondary Wins Server IP
        /// </summary>
        /// <param name="serverIPAddress">if true then check if false then uncheck</param>
        public bool SetSecondaryWinServerIP(string serverIPAddress)
        {
            TraceFactory.Logger.Info("Setting Secondary WINs server IP");
            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                if (serverIPAddress == string.Empty)
                {
                    serverIPAddress = "0.0.0.0";
                }
                StopAdapter();
                _adapter.Start();
                _adapter.Navigate("WINS");
                if (_adapter.IsElementPresent("WINS_Option_enable"))
                {
                    _adapter.Click("WINS_Option_enable");
                    string winsAddressControl = "WINS_Secondary_IP_";
                    for (int i = 0; i < 4; i++)
                    {
                        _adapter.SetText("{0}{1}".FormatWith(winsAddressControl, i), serverIPAddress.ToString().Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[i]);
                    }
                }
                _adapter.Click("Apply");
                Thread.Sleep(TimeSpan.FromSeconds(5));
                if (_adapter.SearchText("The changes have been updated successfully."))
                {
                    TraceFactory.Logger.Info("Configured Secondary WINS server IP");
                    _adapter.Click("OK");
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
            }
            
           else if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
			{
				TraceFactory.Logger.Debug("Setting Secondary Win Server IP: {0} through Web UI.".FormatWith(serverIPAddress));

				_adapter.Navigate("Network_Identification");
				_adapter.SetText("Secondary_IPv4", serverIPAddress);
				_adapter.Click("Apply");
				Thread.Sleep(TimeSpan.FromSeconds(5));
			}
			else
			{
				TraceFactory.Logger.Info("TPS printers doesn't support secondary WINS option");
                return true;
			
			}
            if (!(_adapter.Settings.ProductType == PrinterFamilies.InkJet))
            {
                if (serverIPAddress.EqualsIgnoreCase(GetSecondaryWinServerIP()))
                {
                    TraceFactory.Logger.Info("Successfully set the secondary wins server IP: {0} through Web UI.".FormatWith(serverIPAddress));
                    
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to set the secondary wins server IP: {0} through Web UI.".FormatWith(serverIPAddress));
                    return false;
                }
            }
            else
            {
                TraceFactory.Logger.Info("TPS printers doesn't support secondary WINS option");
                return true;
            }
            return true;
        }

        /// <summary>
        /// Set the syslog server to the specified address.
        /// </summary>
        /// <param name="serverAddress">The syslog server address.</param>
        /// <returns>True if the value is set, else false.</returns>
        public bool SetSyslogServer(string serverAddress)
        {
            TraceFactory.Logger.Debug("Setting Syslog server to: {0}".FormatWith(serverAddress));

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                _adapter.Navigate("Advanced");
                _adapter.Check("Syslog");
                _adapter.SetText("Syslog_Server", serverAddress);
                _adapter.Click("Apply");
            }
            else if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                _adapter.Navigate("Advanced");
                _adapter.SetText("Syslog_Server", serverAddress);
                _adapter.Click("Apply");
            }
            else
            {
                StopAdapter();
                _adapter.Start();
                _adapter.Navigate("NetworkSyslog");
                _adapter.Check("Enable_Syslog");
                _adapter.SetText("Syslog_Server", serverAddress);
                _adapter.Click("Apply");
                if (_adapter.IsElementPresent("Ok"))
                {
                    _adapter.Click("Ok");
                    
                }
            }
			Thread.Sleep(TimeSpan.FromSeconds(10));

            if (GetSyslogServer() == serverAddress)
            {
                TraceFactory.Logger.Info("Successfully set the Syslog server to: {0}".FormatWith(serverAddress));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to set the Syslog server to {0}".FormatWith(serverAddress));
                return false;
            }
        }

		/// <summary>
		/// Get the syslog server configured from Web UI.
		/// </summary>
		/// <returns>The syslog server address.</returns>
		public string GetSyslogServer()
		{
			TraceFactory.Logger.Debug("Getting Syslog server from Web UI.");
           
            if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
            {
                _adapter.Navigate("Advanced");
              
            }
            else
            {
                StopAdapter();
                _adapter.Start();
                _adapter.Navigate("NetworkSyslog");
               
            }
            string syslogServer = _adapter.GetValue("Syslog_Server");

            TraceFactory.Logger.Info("Syslog Server from Web UI: {0}".FormatWith(syslogServer));
            return syslogServer;
        }

        /// <summary>
        /// Enable All Advanced Options 
        /// </summary>
        public void SetAdvancedOptions()
        {
            if (_adapter.Settings.ProductType != PrinterFamilies.InkJet)
            {
                TraceFactory.Logger.Info("Setting Advanced options from EWS");
                if (IsOmniOpus)
                {
                    SetAdvancedOptionsOmniOpus();
                }
                else
                {
                    try
                    {
                        _adapter.Navigate("Printer_Features");
                        IList<IWebElement> elements = _adapter.GetPageElements("Enabled_Features", true, FindType.ByXPath);

                        foreach (IWebElement element in elements)
                        {
                            if (null != element)
                            {
                                if (element.GetAttribute("type").EqualsIgnoreCase("checkbox") && element.Enabled)
                                {
                                    if (!string.IsNullOrEmpty(element.GetAttribute("id")))
                                    {
                                        _adapter.Check(element.GetAttribute("id"), false, FindType.ById);
                                    }
                                    else if (!string.IsNullOrEmpty(element.GetAttribute("name")))
                                    {
                                        _adapter.Check(element.GetAttribute("name"), false, FindType.ByName);
                                    }
                                }
                            }
                        }

                        _adapter.Click("Apply");
                        _adapter.ClickOkonAlert();

                        Thread.Sleep(TimeSpan.FromMinutes(1));

                        if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                        {
                            SetHiddenOptions();
                        }
                    }
                    catch
                    {
                        // Do nothing
                    }
                    finally
                    {
                        StopAdapter();
                        _adapter.Start();
                    }
                }
            }
        }

        /// <summary>
        /// Sets the advanced options in Omni/Opus devices
        /// </summary>
        private void SetAdvancedOptionsOmniOpus()
        {
            try
            {
                _adapter.Navigate("Printer_Features");
                _adapter.Check("SLP");
                _adapter.Check("Bonjour");
                _adapter.Check("Multicast_IPv4");
                _adapter.Check("9100_Printing");
                _adapter.Check("FTP_Printing");
                _adapter.Check("LPD_Printing");
                _adapter.Check("IPP_Printing");
                _adapter.Check("Telnet_Config");
                _adapter.Check("WS_Discovery");
                _adapter.Click("Apply");
                _adapter.ClickOkonAlert();
                Thread.Sleep(TimeSpan.FromSeconds(30));
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug("An error occured while enabling advanced options. Error details: {0}".FormatWith(ex.Message));
            }
        }

        /// Enable/ disable MulticastIPV4 option on WebUI option
        /// Applicable only for VEP
        /// </summary>        
        /// <param name="enable">true to enable, false to disable</param>
        public bool SetMulticastIPv4(bool enable)
        {
            string state = enable ? "enable" : "disable";
            TraceFactory.Logger.Debug(SET_DEBUG_OPTION_LOG.FormatWith("Multicast IPv4", state));

            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Multicast IPv4 is not supported for TPS products!");
            }
            else
            {
                _adapter.Navigate("Misc_Settings");
                if (enable)
                {
                    TraceFactory.Logger.Debug("Enabling Multicast IPV4 option");

                    _adapter.Check("Multicast_IPv4");

                    TraceFactory.Logger.Info("Enabled Multicast IPv4 option");
                }
                else
                {
                    TraceFactory.Logger.Debug("Disabling Multicast IPv4 option");

                    _adapter.Uncheck("Multicast_IPv4");

                    TraceFactory.Logger.Info("Disabled Multicast IPv4 option");
                }
                _adapter.Click("Apply_Misc");
            }
            Thread.Sleep(TimeSpan.FromSeconds(5));
            if (GetMulticastIPv4().Equals(enable))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the status of multicastipv4 option
        /// </summary>
        /// <returns>true if checked or else false</returns>
        public bool GetMulticastIPv4()
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                TraceFactory.Logger.Debug("Multicast IPv4 is not supported for TPS products!");
                return true;
            }
            else
            {
                TraceFactory.Logger.Debug(GET_DEBUG_OPTION_LOG.FormatWith("Multicast IPv4"));
                _adapter.Navigate("Misc_Settings");
                TraceFactory.Logger.Info(GET_SUCCESS_LOG.FormatWith("Multicast IPv4", _adapter.IsChecked("Multicast_IPv4") == true ? "enable" : "disable"));
                return _adapter.IsChecked("Multicast_IPv4");
            }
        }

        /// <summary>
        ///Enables SNMP Read Write Access
        /// </summary>
        /// <returns>true if checked or else false</returns>
        public void EnableSNMPAccess()
        {
            
            if (!(_adapter.Settings.ProductType == PrinterFamilies.InkJet))
            {
                TraceFactory.Logger.Info("Enabling SNMP is not applicable for VEP and TPS");
            }
            else
            {
                TraceFactory.Logger.Info("Enablaing SNMP read Write access for INK");

                    StopAdapter();
                    _adapter.Start();
                try
                 {
                    _adapter.Navigate("SNMP");
                    _adapter.Click("Enable_v1v2_RW_access");
                    _adapter.Click("Apply");

                    Thread.Sleep(TimeSpan.FromSeconds(10));

                    if (_adapter.SearchText("The changes have been updated successfully"))
                    {
                        TraceFactory.Logger.Info("Successfully configured the {0} community name.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to Enable SNMP read-Write Access");

                    }
                }
                finally
                {
                    if (_adapter.IsElementPresent("OK"))
                    {
                        _adapter.Click("OK");
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
            }
       }
        /// <summary>
        /// Enable ACL Feature        
        /// </summary>        
        /// <param name="enable">true to enable, false to disable</param>
        /// TODO: This method applies to enter only one address, need to update this to enter multiple entries of addresses.
		public void SetACL(string address, bool allowHTTP = true, bool save = true)
        {
            TraceFactory.Logger.Info("Setting ACL in the Printer with Address: {0} , AllowHTTP option set to : {1}".FormatWith(address, allowHTTP));

            try
            {
                _adapter.Navigate("Access_Control");
                _adapter.SetText("IPv4_Address_1", address);

			if (save)
			{
                    _adapter.Check("Rule1");
                    //_adapter.Check("Save_1");
			}
			else
			{
                    _adapter.Uncheck("Rule1");
                    //_adapter.Uncheck("Save_1");
                }

                if (allowHTTP)
                {
                    _adapter.Check("Allow_HTTP_Access");
                }

                _adapter.Click("Apply");
                TraceFactory.Logger.Info("Setting ACL is Successfull");
            }
            catch
            {
                TraceFactory.Logger.Info("Failed to set ACL in Printer with Address:{0}".FormatWith(address));
            }
        }

        /// <summary>
        /// Creates an ACL rule with the given details.
        /// </summary>
        /// <param name="ruleSettngs">The details of the rule IP address and subnet mask.</param>
        /// <param name="AllowHttpAccess">True to enable http access, else false</param>
        /// <returns></returns>
		public bool CreateAclRule(Dictionary<string, string> ruleSettngs, bool AllowHttpAccess = true)
        {
            try
            {
                _adapter.Navigate("Access_Control");

                string ipAddressControl = "IPv4_Address_";
                string subnetMaskontrol = "Mask_";
                string saveControl = "Rule";
                int count = 0;

                foreach (var item in ruleSettngs)
                {
                    count += 1;
                    _adapter.SetText("{0}{1}".FormatWith(ipAddressControl, count), item.Key);
                    _adapter.SetText("{0}{1}".FormatWith(subnetMaskontrol, count), (_adapter.Settings.ProductType == PrinterFamilies.TPS && string.IsNullOrEmpty(item.Value)) ? "255.255.255.255" : item.Value);
                    _adapter.Check("{0}{1}".FormatWith(saveControl, count));
                }

                _adapter.Click("Apply");

                if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                {
                    // TPS does not give success message on apply.
                    StopAdapter();
                    _adapter.Start();
                    _adapter.Navigate("Access_Control");
                }
                else
                {
                    if (_adapter.SearchText("Changes have been made successfully"))
                    {
                        if (_adapter.Settings.ProductType == PrinterFamilies.LFP)
                        {
                            _adapter.Click("ACL_Ok");
                        }
                    }
                    else
                    {
                        TraceFactory.Logger.Debug("Failed to add the rule details.");
                        return false;
                    }
                }

                // Validation
                count = 0;
                string mask;

                foreach (var item in ruleSettngs)
                {
                    count += 1;
                    if (item.Key != _adapter.GetValue("{0}{1}".FormatWith(ipAddressControl, count)))
                    {
                        TraceFactory.Logger.Info("IPv4 Address {0} is not added.".FormatWith(item.Key));
                        return false;
                    }

                    mask = (_adapter.Settings.ProductType == PrinterFamilies.TPS && string.IsNullOrEmpty(item.Value) ? "255.255.255.255" : item.Value);
                    if (!_adapter.GetValue("{0}{1}".FormatWith(subnetMaskontrol, count)).EqualsIgnoreCase(mask))
                    {
                        TraceFactory.Logger.Info("Subnet mask {0} is not added.".FormatWith(item.Value));
                        return false;
                    }
                }

                TraceFactory.Logger.Debug("Added the rule details.");

                try
                {
                    if (AllowHttpAccess)
                    {
                        _adapter.Check("Allow_HTTP_Access");
                    }
                    else
                    {
                        _adapter.Uncheck("Allow_HTTP_Access");
                    }

					if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
					{
						_adapter.Check("EnableACL");
					}
					_adapter.Click("Apply");
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    
					if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
					{
						// TPS does not give success message on apply.
						StopAdapter();
						_adapter.Start();
						_adapter.Navigate("Access_Control");
					}
					else
					{
						if (!_adapter.SearchText("Changes have been made successfully"))
						{
							TraceFactory.Logger.Info("Failed to {0} Allow Web Server (Http) Access.".FormatWith(AllowHttpAccess ? "Enable" : "Disable"));
							return false;
						}
						else
						{
							TraceFactory.Logger.Info("Successfully {0} Allow Web Server (Http) Access.".FormatWith(AllowHttpAccess ? "Enabled" : "Disabled"));
							
							if (_adapter.Settings.ProductType == PrinterFamilies.LFP)
                            {
                                _adapter.Click("ACL_Ok");
                            }
                        }
                    }

					return AllowHttpAccess ? _adapter.IsChecked("Allow_HTTP_Access") : !_adapter.IsChecked("Allow_HTTP_Access");
				}
				catch (Exception ex)
				{
                    TraceFactory.Logger.Info("Exception occured : {0}".FormatWith(ex.Message));
                    if (!AllowHttpAccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
			catch (Exception ex)
			{
                TraceFactory.Logger.Info("Exception occured : {0}".FormatWith(ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Gets the ACL rule details from Web UI.
        /// </summary>
        /// <returns>The deatils IP address and subnet mask of a rule.</returns>
        public Dictionary<string, string> GetAclRuleDetails()
        {
            _adapter.Navigate("Access_Control");

            string ipAddressControl = "IPv4_Address_";
            string subnetMaskontrol = "Mask_";

            Dictionary<string, string> ruleDetails = new Dictionary<string, string>();
            string ipAddress = string.Empty;
            string subnetMask = string.Empty;

            for (int i = 1; i <= 10; i++)
            {
                ipAddress = _adapter.GetValue("{0}{1}".FormatWith(ipAddressControl, i));
                subnetMask = _adapter.GetValue("{0}{1}".FormatWith(subnetMaskontrol, i));

                if (!string.IsNullOrEmpty(ipAddress) && !string.IsNullOrEmpty(subnetMask))
                {
                    ruleDetails.Add(ipAddress, subnetMask);
                }
            }

            return ruleDetails;
        }

        /// <summary>
        /// Deletes all the ACL rules.
        /// </summary>
        /// <returns>True if the ACL rules are deleted, else false.</returns>
        public bool DeleteAllAclRules()
        {

            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                StopAdapter();
                _adapter.Start();
            }
            _adapter.Navigate("Access_Control");

            string ipAddressControl = "IPv4_Address_";
            string subnetMaskontrol = "Mask_";
            string saveControl = "Rule";

            for (int i = 1; i <= 10; i++)
            {
                _adapter.Uncheck("{0}{1}".FormatWith(saveControl, i));

                if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
                {
                    _adapter.SetText("{0}{1}".FormatWith(ipAddressControl, i), string.Empty);
                    _adapter.SetText("{0}{1}".FormatWith(subnetMaskontrol, i), string.Empty);
                }
            }

            _adapter.Click("Apply");

            if (_adapter.Settings.ProductType != PrinterFamilies.TPS)
            {
                if (_adapter.SearchText("Changes have been made successfully"))
                {
                    TraceFactory.Logger.Info("Successfully deleted all ACL rules.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to delete all ACL rules.");
                    return false;
                }
            }
            else
            {
                for (int i = 1; i <= 10; i++)
                {
                    if (string.IsNullOrEmpty(_adapter.GetValue("{0}{1}".FormatWith(ipAddressControl, i))) && (string.IsNullOrEmpty(_adapter.GetValue("{0}{1}".FormatWith(subnetMaskontrol, i)))))
                    {
                        continue;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to delete rule.");
                        return false;
                    }
                }

                return true;
            }

        }

        /// <summary>
        /// Sets the SNMP community name
        /// </summary>
        /// <param name="community"><see cref="SNMPCommunity"/></param>
        /// <param name="setCommunityName">The set community name.</param>
        /// <param name="getCommunityName">The get community name.</param>
        /// <returns>True if the community name is set, else false.</returns>
		public bool SetSnmpCommunityName(SNMPCommunity community, string setCommunityName = "", string getCommunityName = "")
        {
            string communityType = SNMPCommunity.None == community ? "Default" : (SNMPCommunity.Both == community) ? "Set and Get" : community.ToString();
            TraceFactory.Logger.Info("Setting SNMP {0} community Name.".FormatWith(communityType));
            if (_adapter.Settings.ProductType == PrinterFamilies.InkJet)
            {
                StopAdapter();
                _adapter.Start();
            }

            _adapter.Navigate("SNMP_Settings");

            _adapter.SetText("Set_Community_Name", setCommunityName);
            _adapter.SetText("Confirm_Set_Community_Name", setCommunityName);

            _adapter.SetText("Get_Community_Name", getCommunityName);
            _adapter.SetText("Confirm_Get_Community_Name", getCommunityName);

            _adapter.Click("SNMP_Apply");
            Thread.Sleep(TimeSpan.FromSeconds(10));

            if (_adapter.SearchText("successfully."))
            {
                TraceFactory.Logger.Info("Successfully configured the {0} community name.".FormatWith(communityType));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to configure the get community name: {0} and set community name: {1}.".FormatWith(getCommunityName, setCommunityName));
                return false;
            }
        }

        /// <summary>
        /// Sets the default SNMP community name
        /// </summary>
        /// <returns>True if the community name is set, else false.</returns>
        public bool SetDefaultSnmpCommunityName()
        {
            TraceFactory.Logger.Info("Configuring the default SNMP community name.");

            return SetSnmpCommunityName(SNMPCommunity.None);
        }

        /// <summary>
        /// Checks if the specified communiy name is set through Web UI
        /// </summary>
        /// <param name="community"><see cref="SNMPCommunity"/></param>
        /// <returns>True if the community name is set, else false.</returns>
		public bool isCommunityNameSet(SNMPCommunity community)
        {
            bool result = true;

			_adapter.Navigate("MngmtProtocol_SNMP");

            if (community.HasFlag(SNMPCommunity.Set))
            {
                if (!(string.IsNullOrEmpty(_adapter.GetValue("Set_Community_Name")) && string.IsNullOrEmpty(_adapter.GetValue("Confirm_Set_Community_Name"))))
                {
                    TraceFactory.Logger.Info("Successfully validated SNMP set community name status as SET from Settings page in Web UI.");
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to validate SNMP set community name status as SET from Settings page in Web UI.");
                    result = false;
                }
            }

            if (community.HasFlag(SNMPCommunity.Get))
            {
                if (!(string.IsNullOrEmpty(_adapter.GetValue("Get_Community_Name")) && string.IsNullOrEmpty(_adapter.GetValue("Confirm_Get_Community_Name"))))
                {
                    TraceFactory.Logger.Info("Successfully validated SNMP set community name status as SET from Settings page in Web UI.");
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to validate SNMP set community name status as SET from Settings page in Web UI.");
                    result = false;
                }
            }

            if (community == SNMPCommunity.None)
            {
                if ((string.IsNullOrEmpty(_adapter.GetValue("Get_Community_Name")) && string.IsNullOrEmpty(_adapter.GetValue("Confirm_Get_Community_Name"))))
                {
                    TraceFactory.Logger.Info("Default SNMP community name is set.");
                }
                else
                {
                    TraceFactory.Logger.Info("Default SNMP community name is not set.");
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Validates the text 'ARP DUPLICATE IP ADDRESS' from configuration page
        /// </summary>
        /// <returns>True if the text 'ARP DUPLICATE IP ADDRESS' is found on configuration page, else false.</returns>
        public bool ValidateDuplicateIP()
        {
            TraceFactory.Logger.Info("Navigating to Configuration Page for validating duplicate IP.");
            _adapter.Navigate("Configuration_Page");

            if (_adapter.SearchText("ARP DUPLICATE IP ADDRESS"))
            {
                TraceFactory.Logger.Info("Successfully validated ARP DUPLICATE IP ADDRESS from configuration page.");
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to validate ARP DUPLICATE IP ADDRESS from configuration page.");
                return false;
            }
        }

        /// <summary>
        /// Checks for the presence of mac address from configuration page.
        /// </summary>
        /// <param name="macAddress">The mac address to be verified.</param>
        /// <returns>True if the mac address is present in configuration page, else false.</returns>
        public bool ValidateMacAddress(string macAddress)
        {
            TraceFactory.Logger.Info("Navigating to Configuration Page for validating mac address.");
            _adapter.Navigate("Configuration_Page");

            if (_adapter.SearchText(macAddress.ToLower()) || _adapter.SearchText(macAddress.ToUpper()))
            {
                TraceFactory.Logger.Info("Successfully validated mac address: {0} from configuration page.".FormatWith(macAddress));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to validate mac address: {0} from configuration page.".FormatWith(macAddress));
                return false;
            }
        }

        /// <summary>
        /// Restores to the security defaults. Option avilable under Security -> Settings -> Restore Defaults
        /// </summary>
        /// <returns>True if the operation is successful, else false.</returns>
        public bool RestoreSecurityDefaults()
        {
            TraceFactory.Logger.Info("Restoring security defaults from web UI.");

            _adapter.Navigate("Restore_Defaults");
            _adapter.Click("Restore_Defaults_Submit");

            ClickonConfirmation();

            if (HP.ScalableTest.Utility.Retry.UntilTrue(() => (_adapter.Body.Contains("successfully") || !_adapter.Body.Contains("Restoring device settings")), 5, TimeSpan.FromSeconds(12)))
            {
                TraceFactory.Logger.Info("Security defaults are successfully restored.");
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Security defaults are not restored.");
                return false;
            }
        }

        /// <summary>
        /// Enables SNMPv1 v2 read write access through web UI.
        /// </summary>
        /// <returns>True if the operation is successful, else false.</returns>
        public bool EnableSnmpv1v2ReadWriteAccess()
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                _adapter.Stop();
                _adapter.Start();
                TraceFactory.Logger.Info("Enabling SNMP option");
                if (!GetSnmpv1v2ReadWriteAccessStatus())
                {
                    TraceFactory.Logger.Info(string.Format(SET_DEBUG_OPTION_LOG, "SNMP v1 v2 Read/Write access", "enable"));
                    _adapter.Navigate("MngmtProtocol_SNMP");
                    _adapter.Check("Enable_SNMPv1_v2_ReadWrite");
                    _adapter.Uncheck("Disable_SNMPv1_v2_default_Get_Community_Name");
                    _adapter.Click("Apply");

                    return GetSnmpv1v2ReadWriteAccessStatus();
                }
                else
                {
                    return true;
                }
            }
            {
                TraceFactory.Logger.Debug("enable SNMP v1, v2 read-write access is currently implementated only for VEP");
                return false;
            }
        }

        /// <summary>
        /// Get 'SNMPv1 v2 read write access' status from web UI.
        /// </summary>
        /// <returns>True if enabled, else false.</returns>
        public bool GetSnmpv1v2ReadWriteAccessStatus()
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.VEP)
            {
                TraceFactory.Logger.Info(string.Format(GET_DEBUG_OPTION_LOG, "SNMP v1 v2 Read/Write access"));
                _adapter.Navigate("MngmtProtocol_SNMP");
                return _adapter.IsChecked("Enable_SNMPv1_v2_ReadWrite");
            }
            {
                TraceFactory.Logger.Debug("get SNMP v1, v2 read-write access is currently implementated only for VEP");
                return false;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Convert Time from string format to TimeSpan
        /// </summary>
        /// <param name="time">Time in string format</param>
        /// <returns>TimeSpan value</returns>
        private static TimeSpan GetTime(string time)
        {
            if (time.Equals("Infinite", StringComparison.CurrentCultureIgnoreCase))
            {
                return TimeSpan.Parse(Timeout.Infinite.ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
            }

            // For TPS, returned time format is: dd:hh:mm
            if (time.Contains(':'))
            {
                // Example- time: 10:11:12 => 10 days, 11 hours, 12 minutes
                // 10:11:12 will be interpreted as 10 hours, 11 minutes, 12 seconds if we parse directly; hence replacing first occurrence of ':' with '.'
                if (time.Split(':').Length == 3)
                {
                    int index = time.IndexOf(':');
                    time = time.Insert(index, ".").Remove(index + 1, 1);
                }

                return TimeSpan.Parse(time, CultureInfo.CurrentCulture);
            }

            long timeInTicks = 0;
            string[] splitTime;

            if (time.Contains('d'))
            {
                splitTime = time.Split('d');
                timeInTicks = int.Parse(splitTime[0], CultureInfo.CurrentCulture) * TimeSpan.TicksPerDay;
                time = splitTime[1];
            }

            if (time.Contains('h'))
            {
                splitTime = time.Split('h');
                timeInTicks += int.Parse(splitTime[0], CultureInfo.CurrentCulture) * TimeSpan.TicksPerHour;
                time = splitTime[1];
            }

            if (time.Contains('m'))
            {
                splitTime = time.Split('m');
                timeInTicks += int.Parse(splitTime[0], CultureInfo.CurrentCulture) * TimeSpan.TicksPerMinute;
                time = splitTime[1];
            }

            if (time.Contains('s'))
            {
                splitTime = time.Split('s');
                timeInTicks += int.Parse(splitTime[0], CultureInfo.CurrentCulture) * TimeSpan.TicksPerSecond;
                time = splitTime[1];
            }

            TimeSpan convertedTime = new TimeSpan(timeInTicks);

            return convertedTime;
        }

        /// <summary>
        /// Stop Ews adapter
        /// </summary>
        public void StopAdapter()
        {
            try
            {
                _adapter.Stop();
            }
            catch (Exception generalException)
            {
                TraceFactory.Logger.Debug("Exception details: {0}".FormatWith(generalException.Message));
                KillBrowser();
            }
        }

        /// <summary>
        /// Kill browser
        /// </summary>
        private void KillBrowser()
        {
            TraceFactory.Logger.Debug("Killing all {0} browsers.".FormatWith(_adapter.Settings.Browser));
            Process[] browserProcess = Process.GetProcessesByName(_adapter.Settings.Browser.ToString());

            try
            {
                foreach (Process process in browserProcess)
                {
                    process.Kill();
                }

                for (int i = 0; i < 4; i++)
                {
                    browserProcess = Process.GetProcessesByName(_adapter.Settings.Browser.ToString());

                    if (0 == browserProcess.Length)
                    {
                        return;
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(30));
                    foreach (Process process in browserProcess)
                    {
                        process.Kill();
                    }
                }
            }
            catch
            {
                // do nothing
            }
        }

        /// <summary>
        /// Get Mac Address of Printer
        /// </summary>
        /// <returns>Mac Address of Printer</returns>
        public string GetMacAddress()
        {
            Printer.Printer printer = PrinterFactory.Create(_adapter.Settings.ProductType, IPAddress.Parse(_adapter.Settings.DeviceAddress));
            string printerMacAddress = printer.MacAddress;
            TraceFactory.Logger.Debug("Printer MAC address : {0}".FormatWith(printerMacAddress));
            return printerMacAddress;
        }

        /// <summary>
        /// Click on confirmation box when advanced option is enabled/ disabled (Only TPS and Inkjet products)
        /// </summary>
        private void ClickonConfirmation()
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS || _adapter.Settings.ProductType == PrinterFamilies.InkJet)
			{
				_adapter.ClickOkonAlert();
				Thread.Sleep(TimeSpan.FromSeconds(5));
                try
                {
                    if (_adapter.IsElementPresent("ConfirmOk"))
                    {
                        _adapter.Click("ConfirmOk");
                        TraceFactory.Logger.Info("Confirm OK");
                        Thread.Sleep(TimeSpan.FromSeconds(40));
                    }
                    
                }
                catch
                {
                    //Do nothing
                    // TraceFactory.Logger.Info("Exception occured : {0} ".FormatWith(ex));
                }
			}
		}

        private void ClickonConfirmationWireless()
        {
            if (_adapter.Settings.ProductType == PrinterFamilies.TPS)
            {
                //_adapter.ClickOkonAlert();
                //Thread.Sleep(TimeSpan.FromSeconds(5));
                if (_adapter.IsElementPresent("YES"))
                {
                    _adapter.Click("YES");
                    Thread.Sleep(TimeSpan.FromSeconds(40));
                }
            }
        }

        /// <summary>
        /// This method is applicable only to inkjet family printers.
        /// </summary>
        /// <param name="ipConfigMethod"><see cref=" IPConfigMethod"/></param>
        /// <param name="manualIPAddress">Manual IP Address to be set</param>
        /// <param name="subnetMask">Subnet Mask to be set</param>
        /// <param name="defaultGateway">Default Gateway to be set</param>
        /// <param name="printerMacAddress">Mac address of the printer.</param>
        /// <param name="expectedIPAddress">Expected printer IP Address after configuration method change</param>
        /// <returns>true if configuration method is set, false otherwise</returns>
        private void SetConfigMethod(IPConfigMethod ipConfigMethod, IPAddress manualIPAddress, string subnetMask, string defaultGateway, string printerMacAddress, ref IPAddress expectedIPAddress)
        {
            //TODO: Remove stop, start of adapter once the dynamic IP address issue is solved.
            _adapter.Stop();
            _adapter.Start();

            _adapter.Navigate("IPv4_Config");

            if (ipConfigMethod == IPConfigMethod.AUTOIP)
            {
                if (_adapter.IsElementPresent("AUTO_IP"))
                {
                    _adapter.Check("AUTO_IP");
                    _adapter.Click("Apply");
                }
            }
            else if (ipConfigMethod == IPConfigMethod.Manual)
            {
                _adapter.Navigate("Wired_Status");

                string currentIpAddress = _adapter.GetText("IPv4_Address");

                if (string.IsNullOrEmpty(currentIpAddress))
                {
                    Thread.Sleep(TimeSpan.FromSeconds(30));
                    currentIpAddress = _adapter.GetText("IPv4_Address");
                }

                manualIPAddress = manualIPAddress ?? IPAddress.Parse(_adapter.GetText("IPv4_Address"));
                subnetMask = (string.IsNullOrEmpty(subnetMask)) ? _adapter.GetText("Subnet_Mask") : subnetMask;
                // If the printer is in auto ip, giving auto ip as the default  gateway as the value.
                defaultGateway = manualIPAddress.IsAutoIP() ? manualIPAddress.ToString() : ((string.IsNullOrEmpty(defaultGateway)) ? _adapter.GetText("Default_Gateway") : defaultGateway);

                // Stopping and starting the WebUI as the control ids keep changing for each navigation to other pages.
                StopAdapter();
                Start();

                TraceFactory.Logger.Debug("Setting Manual IP Address : {0}, Subnet Mask: {1}, Default Gateway: {2}.".FormatWith(manualIPAddress, subnetMask, defaultGateway));

                _adapter.Navigate("IPv4_Config");
                expectedIPAddress = manualIPAddress;

                string ipAddressControl = "IP_";
                string maskControl = "Subnet_";
                string gatewayControl = "Gateway_";

                _adapter.Click("Manual_IP");

                //TODO: Find out a better approach instead of 3 loops
                for (int i = 0; i < 4; i++)
                {
                    _adapter.SetText("{0}{1}".FormatWith(ipAddressControl, i), manualIPAddress.ToString().Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[i]);
                }

                for (int i = 0; i < 4; i++)
                {
                    _adapter.SetText("{0}{1}".FormatWith(maskControl, i), subnetMask.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[i]);
                }

                for (int i = 0; i < 4; i++)
                {
                    _adapter.SetText("{0}{1}".FormatWith(gatewayControl, i), defaultGateway.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[i]);
                }

                _adapter.Click("Apply");

                if (_adapter.IsElementPresent("OK"))
                {
                    _adapter.Click("OK");
                }
            }
            else
            {
                // If Automatic IP option is present in IPV4 settings page, the configuration methods DHCP, BOOTP can be selected from Advanced page.
                if (_adapter.IsElementPresent("Automatic_IP"))
                {
                    // By default Automatic IP option will be selected. So selecting it only if it is not selected so as to avoid the connection issue.
                    if (!_adapter.IsChecked("Automatic_IP"))
                    {
                        _adapter.Click("Automatic_IP");
                        _adapter.Click("Apply");
                        // The printer will lose connection and acquire a new IP address here. Discovering the printer with Mac Address to get the new IP address.
                        Thread.Sleep(TimeSpan.FromMinutes(1));

                        // Discover Printer with mac address and assign the new IP Address acquired by Printer to wrappers
                        if (!string.IsNullOrEmpty(printerMacAddress))
                        {
                            //Added as the bacabod discovery is not working for ink products.
                            expectedIPAddress = IPAddress.Parse(CtcUtility.GetPrinterIPAddress(printerMacAddress));

                            ChangeDeviceAddress(expectedIPAddress);
                        }
                    }

                    //TODO: Remove stop, start of adapter once the dynamic IP address issue is solved.
                    _adapter.Stop();
                    _adapter.Start();

                    _adapter.Navigate("Wired_Advanced");

                    if (!_adapter.IsChecked(ipConfigMethod.ToString().ToUpper()))
                    {
                        _adapter.Click(ipConfigMethod.ToString().ToUpper());
                        _adapter.Click("Apply");
                    }
                }
                else
                {
                    _adapter.Click(ipConfigMethod.ToString().ToUpper());
                    _adapter.Click("Apply");
                }
            }

            Thread.Sleep(TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Set option available in hidden page.
        /// Note: Applicable only for TPS products
        /// </summary>
        /// <param name="optionName">Option Name</param>
        /// <param name="state">true to enable, false to disable</param>
        private void SetHiddenOption(string optionName, bool state)
        {
            try
            {
                // Since hidden page is not accessing closing and opening the browser
                StopAdapter();
                _adapter.Start();

                _adapter.Navigate("Hidden_Page");

                TraceFactory.Logger.Info("Setting option from hidden page.");
                if (state)
                {
                    _adapter.Check(optionName);
                }
                else
                {
                    _adapter.Uncheck(optionName);
                }

                _adapter.Click("Apply");
                ClickonConfirmation();
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
            catch
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Enable Telnet, ARP and TFTP options in hidden page.
        /// Note: Applicable for new TPS products
        /// </summary>
        private void SetHiddenOptions()
        {
            try
            {
                _adapter.Navigate("Hidden_Page");
                // Check if element is present to verify new/ old product
                if (_adapter.IsElementPresent("Telnet_Config"))
                {
                    TraceFactory.Logger.Info("Setting options from hidden page.");
                    _adapter.Check("Telnet_Config");
                    _adapter.Check("TFTP_Config");
                    _adapter.Check("ARPPing");
                    _adapter.Click("Apply");
                    ClickonConfirmation();
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                }
            }
            catch
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Get checkbox option value
        /// Note: Applicable only for TPS products
        /// </summary>
        /// <param name="option">Option Name</param>
        /// <returns>true if checked, false otherwise</returns>
        private bool GetHiddenOption(string option)
        {
            try
            {
                _adapter.Navigate("Hidden_Page");
                // Check if element is present to verify new/ old product
                if (_adapter.IsElementPresent(option))
                {
                    return _adapter.IsChecked(option);
                }
            }
            catch
            {
                // Do nothing
            }

            return false;
        }

        #endregion
    }
}