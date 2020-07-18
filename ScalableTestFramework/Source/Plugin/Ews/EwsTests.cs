using System;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;


namespace HP.ScalableTest.Plugin.Ews
{
    class EwsTests : CtcBaseTests
    {
        EwsActivityData _activityData;

        public EwsTests(EwsActivityData data)
            : base(data.ProductName)
        {
            _activityData = data;
            ProductFamily = data.ProductCategory;
            Sliver = "EWS";
        }

        private EwsAdapter CreateAdapter()
        {
            BrowserModel browserModel = GetBrowserType(_activityData.BrowserNumber);
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), _activityData.ProductCategory);
            EwsSettings settings = new EwsSettings(_activityData.ProductName, family, _activityData.SitemapsVersion, _activityData.PrinterIP, browserModel, EwsAdapterType.WebDriverAdapter);
            EwsSeleniumSettings seleniumSettings = new EwsSeleniumSettings();

            string basePath = @"\\etlhubrepo\boi\CTC\Sitemaps";
            string siteMapPath = @"{0}\{1}\{2}\{3}".FormatWith(basePath, _activityData.ProductCategory, _activityData.ProductName, _activityData.SitemapsVersion);

            seleniumSettings.SeleniumChromeDriverPath = @"\\etlhubrepo\boi\CTC\SeleniumFiles\chromedriver.exe";
            seleniumSettings.SeleniumIEDriverPath32 = @"\\etlhubrepo\boi\CTC\SeleniumFiles\IEDriverServer-x86.exe";
            seleniumSettings.SeleniumIEDriverPath64 = @"\\etlhubrepo\boi\CTC\SeleniumFiles\IEDriverServer-x64.exe";

            EwsAdapter adapter = new EwsAdapter(settings, seleniumSettings, siteMapPath);
            return adapter;
        }

        private BrowserModel GetBrowserType(int browserNumber)
        {
            BrowserModel model;
            switch (browserNumber)
            {
                case 1:
                    model = BrowserModel.Explorer;
                    break;

                case 2:
                    model = BrowserModel.Firefox;
                    break;

                case 3:
                    model = BrowserModel.Chrome;
                    break;

                case 4:
                    model = BrowserModel.Safari;
                    break;

                case 5:
                    model = BrowserModel.Opera;
                    break;

                default:
                    model = BrowserModel.Firefox;
                    break;

            }

            return model;
        }

        #region AllTests
        /// <summary>
        /// TestCase ID  : 406591
        /// Verification of enable-disable of tcp print services
        /// OS           : WinXP-64
        /// Browser      : Firefox
        /// Network      : IPv6
        /// Connectivity : Wireless
        /// </summary>
        [TestDetailsAttribute(Id = 406591, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.TPS, Description = "Verification of enable-disable of tcp print services")]
        public bool Test_406591()
        {
            return EwsTemplates.TemplateTCPPrintServicesTPS(CreateAdapter());
        }




        /// <summary>
        /// TestCase ID  : 406715
        /// Verify the information displayed in network summary page
        /// OS           : Win8-32
        /// Browser      : IE8
        /// Network      : IPv4
        /// Connectivity : Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406715, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.VEP, Description = "Verify the information displayed in network summary page")]
        public bool Test_406715()
        {
            return EwsTemplates.TemplateNetworkSummaryVEP(CreateAdapter());
        }

        /// <summary>
        /// TestCase ID  : 103750
        /// Verification of enable-disable of device discovery services
        /// OS           : WinXP-32
        /// Browser      : IE7
        /// Network      : IPv4
        /// Connectivity : Wired
        /// </summary>
        [TestDetailsAttribute(Id = 103750, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.LFP | ProductFamilies.TPS | ProductFamilies.VEP, Description = "Verification of enable-disable of device discovery services")]
        public bool Test_103750()
        {
            bool result = false;

            if (ProductFamily == "LFP")
            {
                result = EwsTemplates.TemplateDeviceDiscoveryLFP(CreateAdapter());
            }
            else if (ProductFamily == "TPS")
            {
                result = EwsTemplates.TemplateDeviceDiscoveryTPS(CreateAdapter());
            }
            else if (ProductFamily == "VEP")
            {
                result = EwsTemplates.TemplateDeviceDiscoveryVEP(CreateAdapter());
            }

            return result;
        }


        /// <summary>
        /// TestCase ID  : 406572
        /// Verification of enable-disable of management services
        /// OS           : Win2003-64
        /// Browser      : IE7
        /// Network      : IPv4
        /// Connectivity : Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406572, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.VEP, Description = "Verification of enable-disable of management services")]
        public bool Test_406572()
        {
            return EwsTemplates.TemplateManagementServicesVEP(CreateAdapter());
        }


        /// <summary>
        /// TestCase ID  : 406580
        /// Verification of enable-disable of naming resolution services
        /// OS           : Win7-64
        /// Browser      : IE8
        /// Network      : IPv6
        /// Connectivity : Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406580, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS, Description = "Verification of enable-disable of naming resolution services")]
        public bool Test_406580()
        {
            bool result = false;

            if (ProductFamily == "TPS")
            {
                result = EwsTemplates.TemplateNamingResolutionTPS(CreateAdapter());
            }
            else if (ProductFamily == "VEP")
            {
                result = EwsTemplates.TemplateNamingResolutionVEP(CreateAdapter());
            }

            return result;

        }

        /// <summary>
        /// TestCase ID  : 103777
        /// Verification of enable-disable of naming resolution services
        /// OS           : WinXP-32
        /// Browser      : Firefox
        /// Network      : IPv6
        /// Connectivity : Wireless
        /// </summary>
        [TestDetailsAttribute(Id = 103777, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.TPS, Description = "Verification of enable-disable of naming resolution services")]
        public bool Test_103777()
        {

            return EwsTemplates.TemplateNamingResolutionTPS(CreateAdapter());

        }

        /// <summary>
        /// TestCase ID  : 406742
        /// Verify the information displayed in network summary page
        /// OS           : Win2000-32
        /// Browser      : IE8
        /// Network      : IPv4
        /// Connectivity : Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406742, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.LFP | ProductFamilies.VEP | ProductFamilies.TPS, Description = "Verify the information displayed in network summary page")]
        public bool Test_406742()
        {
            bool result = false;

            if (ProductFamily == "TPS")
            {
                result = EwsTemplates.TemplateNetworkSummaryTPS(CreateAdapter());
            }
            else if (ProductFamily == "VEP")
            {
                result = EwsTemplates.TemplateNetworkSummaryVEP(CreateAdapter());
            }
            else if (ProductFamily == "LFP")
            {
                result = EwsTemplates.TemplateNetworkSummaryLFP(CreateAdapter());
            }
            return result;
        }

        /// <summary>
        /// TestCase ID  : 406720
        /// Verify configuration of parameters in Network Identification tab
        /// OS           : Win7-32
        /// Browser      : Firefox
        /// Network      : IPv4
        /// Connectivity : Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406720, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.VEP, Description = "Verify configuration of parameters in Network Identification tab")]
        public bool Test_406720()
        {
            bool result = false;

            if (ProductFamily == "TPS")
            {
                result = EwsTemplates.TemplateNetworkIdentificationTPS(CreateAdapter());
            }
            else if (ProductFamily == "VEP")
            {
                result = EwsTemplates.TemplateNetworkIdentificationVEP(CreateAdapter());
            }

            return result;
        }

        /// <summary>
        /// TestCase ID  : 406740
        /// Verify configuration of system and support information
        /// OS           : WinVista-64
        /// Browser      : Firefox
        /// Network      : IPv4
        /// Connectivity : Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406740, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.LFP | ProductFamilies.VEP, Description = "Verify configuration of system and support information")]
        public bool Test_406740()
        {

            bool result = false;

            if (ProductFamily == "LFP")
            {
                result = ((EwsTemplates.TemplateSystemAndSupportInformation1LFP(CreateAdapter())) &&
            (EwsTemplates.TemplateSystemAndSupportInformation2LFP(CreateAdapter())));

            }
            else if (ProductFamily == "VEP")
            {
                result = ((EwsTemplates.TemplateSystemAndSupportInformation1VEP(CreateAdapter())) &&
            (EwsTemplates.TemplateSystemAndSupportInformation2VEP(CreateAdapter())));

            }

            return result;
        }

        /// <summary>
        /// TestCase ID  : 406738
        /// Verify configuration of  idle time  and refresh rates
        /// OS           : Win2003-64
        /// Browser      : Firefox
        /// Network      : IPv4
        /// Connectivity : Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406738, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.VEP, Description = "Verify configuration of  idle time  and refresh rates")]
        public bool Test_406738()
        {
            return EwsTemplates.TemplateRefreshRateVEP(CreateAdapter());
        }



        /// <summary>
        /// TestCase ID  : 402187
        /// Verification of Cancel button under Networking Tab
        /// OS           : Win7-32
        /// Browser      : [Not Mentioned]
        /// Network      : IPv4
        /// Connectivity : Wireless
        /// </summary>
        [TestDetailsAttribute(Id = 402187, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wireless, Description = "Verification of Cancel button under Networking Tab")]
        public bool Test_402187()
        {
            bool result = false;

            if (ProductFamily == "TPS")
            {
                result = EwsTemplates.TemplateCancelButtonTPS(CreateAdapter());
            }
            else if (ProductFamily == "VEP")
            {
                result = EwsTemplates.TemplateCancelButtonVEP(CreateAdapter());
            }

            return result;
        }



        /// <summary>
        /// TestCase ID  : 405905
        /// Verify access of EWS page on various link speeds
        /// OS           : [Not Mentioned]
        /// Browser      : IE7
        /// Network      : [Not Mentioned]
        /// Connectivity : Wired
        /// </summary>
        [TestDetailsAttribute(Id = 405905, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.VEP, Description = "Verify access of EWS page on various link speeds")]
        public bool Test_405905()
        {
            return EwsTemplates.TemplateLinkSpeedVEP(CreateAdapter(), "100TX AUTO");
        }

        /// <summary>
        /// TestCase ID  : 406128
        /// Verify access of EWS page on various link speeds
        /// OS           : Win2008
        /// Browser      : IE8
        /// Network      : [Not Mentioned]
        /// Connectivity : Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406128, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.VEP, Description = "Verify access of EWS page on various link speeds (100TX HALF)")]
        public bool Test_406128()
        {
            return EwsTemplates.TemplateLinkSpeedVEP(CreateAdapter(), "100TX HALF");
        }

        /// <summary>
        /// TestCase ID  : 406130
        /// Verify access of EWS page on various link speeds
        /// OS           : [Not Mentioned]
        /// Browser      : Firefox
        /// Network      : [Not Mentioned]
        /// Connectivity : [Not Mentioned]
        /// </summary>
        [TestDetailsAttribute(Id = 406130, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.VEP, Description = "Verify access of EWS page on various link speeds (100TX FULL)")]
        public bool Test_406130()
        {
            return EwsTemplates.TemplateLinkSpeedVEP(CreateAdapter(), "100TX FULL");
        }

        /// <summary>
        /// TestCase ID  : 406133
        /// Verify access of EWS page on various link speeds
        /// OS           : Win2003
        /// Browser      : IE8
        /// Network      : [Not Mentioned]
        /// Connectivity : Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406133, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.VEP, Description = "Verify access of EWS page on various link speeds (100T FULL)")]
        public bool Test_406133()
        {
            return EwsTemplates.TemplateLinkSpeedVEP(CreateAdapter(), "1000T FULL");
        }


        /// <summary>
        /// TestCase ID  : 406255
        /// Verify access of EWS page with various Encryption strength
        /// OS           : [Not Mentioned]
        /// Browser      : IE7
        /// Network      : IPv4
        /// Connectivity : Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406255, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.LFP | ProductFamilies.VEP, Description = "Verify access of EWS page with various Encryption strength")]
        public bool Test_406255()
        {
            bool result = false;

            if (ProductFamily == "LFP")
            {
                result = EwsTemplates.TemplateEncryptionStrengthLFP(CreateAdapter(), "Low");
            }
            else if (ProductFamily == "VEP")
            {
                result = EwsTemplates.TemplateEncryptionStrengthVEP(CreateAdapter(), "Low");
            }

            return result;
        }




        /// <summary>
        /// TestCase ID  : 402261
        /// Check and Navigate through 'Support', 'Home' and 'Shop for Supplies' pages
        /// OS           : Win2008-32
        /// Browser      : IE8
        /// Network      : IPv4
        /// Connectivity : Wired
        /// </summary>
        [TestDetailsAttribute(Id = 402261, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.LFP | ProductFamilies.TPS | ProductFamilies.VEP, Description = "Check and Navigate through 'Support', 'Home' and 'Shop for Supplies' pages")]
        public bool Test_402261()
        {
            bool result = false;

            if (ProductFamily == "LFP")
            {
                result = EwsTemplates.TemplateNavigateSupportHomeShopLFP(CreateAdapter());
            }
            else if (ProductFamily == "VEP")
            {
                result = EwsTemplates.TemplateNavigateSupportHomeShopVEP(CreateAdapter());
            }
            else if (ProductFamily == "TPS")
            {
                result = EwsTemplates.TemplateNavigateSupportHomeShopTPS(CreateAdapter());
            }

            return result;
        }



        /// <summary>
        /// TestCase ID  : 406729
        /// Check Enable/Disable IPv6 of IPv6 protocol
        /// OS           : WinXP-64
        /// Browser      : Firefox
        /// Network      : IPv4
        /// Connectivity : Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406729, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Enable/Disable of IPv6 protocol")]
        public bool Test_406729()
        {
            var result = EwsTemplates.TemplateEnableDisableIpv6(CreateAdapter(), _activityData);

            return result;
        }

        /// <summary>
        /// TestCase ID  : 406732
        /// Verify values in configuration page
        /// OS           : WinXP-64
        /// Browser      : Firefox
        /// Network      : IPv4
        /// Connectivity : Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406732, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify values in configuration page")]
        public bool Test_406732()
        {
            bool result = false;

            if (ProductFamily == "VEP")
            {
                result = EwsTemplates.TemplateVerifyNWConfigurationPageVEP(CreateAdapter(), _activityData);
            }
            else if (ProductFamily == "LFP")
            {
                result = EwsTemplates.TemplateVerifyNWConfigurationPageLFP(CreateAdapter(), _activityData);
            }

            return result;
        }

        /// <summary>
        /// TestCase ID  : 402014
        /// Access of pages through HTTP & HTTPS
        /// OS           : WinXP-64
        /// Browser      : Firefox
        /// Network      : IPv4
        /// Connectivity : Wired
        /// </summary>
        [TestDetailsAttribute(Id = 402014, Category = "Basic Single Device Network Management", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify values in configuration page")]
        public bool Test_402014()
        {
            bool result = false;
            if (ProductFamily == "VEP")
            {
                result = EwsTemplates.Template_DeviceAccessHTTPS_VEP(CreateAdapter(), _activityData);
            }
            else if (ProductFamily == "LFP")
            {
                result = EwsTemplates.Template_DeviceAccessHTTPS_LFP(CreateAdapter(), _activityData);
            }

            return result;
        }

        #endregion
    }
}
