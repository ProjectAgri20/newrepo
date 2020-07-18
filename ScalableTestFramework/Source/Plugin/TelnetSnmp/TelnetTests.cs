using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;

namespace HP.ScalableTest.Plugin.TelnetSnmp
{
    class TelnetTests : CtcBaseTests
    {
        TelnetSnmpActivityData _activityData;

        public TelnetTests(TelnetSnmpActivityData data) : base(data.ProductName)
        {
            _activityData = data;
            ProductFamily = data.ProductCategory;
            Sliver = "Telnet";
        }

        private EwsAdapter CreateAdapter()
        {
            EwsSettings settings = new EwsSettings(_activityData.ProductName, Enum<PrinterFamilies>.Parse(_activityData.ProductCategory), _activityData.SitemapsVersion, _activityData.PrinterIP, BrowserModel.Firefox, EwsAdapterType.WebDriverAdapter);
            EwsSeleniumSettings seleniumSettings = new EwsSeleniumSettings();
            string basePath = @"\\etlhubrepo\boi\CTC\Sitemaps";

            seleniumSettings.SeleniumChromeDriverPath = @"\\etlhubrepo\boi\CTC\SeleniumFiles\chromedriver.exe";
            seleniumSettings.SeleniumIEDriverPath32 = @"\\etlhubrepo\boi\CTC\SeleniumFiles\IEDriverServer-x86.exe";
            seleniumSettings.SeleniumIEDriverPath64 = @"\\etlhubrepo\boi\CTC\SeleniumFiles\IEDriverServer-x64.exe";

            string siteMapPath = @"{0}\{1}\{2}\{3}".FormatWith(basePath, _activityData.ProductCategory, _activityData.ProductName, _activityData.SitemapsVersion);
            EwsAdapter adapter = new EwsAdapter(settings, seleniumSettings, siteMapPath);
            return adapter;
        }

        #region VEPTests


        /// <summary>
        /// The intent of this test is to validate telnet connection with/without password
        /// ID          : 406594
        /// Network     : IPv4
        /// Connectivity: Wireless
        /// </summary>
        [TestDetailsAttribute(Id = 406594, Category = "Basic Single Device Network Management", Connectivity = ConnectivityType.Wireless, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS, Description = "Validate telnet connection with/without password")]
        public bool Test_406594()
        {
            bool result = false;
            TelnetTemplates template = new TelnetTemplates(_activityData.PrinterIP);

            if (ProductFamily == "VEP")
            {
                result = (template.Template_403320a_VEP()) &&
                         (template.Template_403320c_VEP());
            }
            else if (ProductFamily == "TPS")
            {
                result = (template.Template_403320a_TPS()) &&
                        (template.Template_403320c_TPS());
            }
            return result;
        }



        /// <summary>
        /// The intent of this test is to validate telnet connection with/without password
        /// ID          : 406581
        /// Network     : IPv4
        /// Connectivity: Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406581, Category = "Basic Single Device Network Management", Connectivity = ConnectivityType.Wired, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS, Description = "Validate telnet connection with/without password")]
        public bool Test_406581()
        {
            bool result = false;
            TelnetTemplates template = new TelnetTemplates(_activityData.PrinterIP);

            if (ProductFamily == "VEP")
            {
                result = (template.Template_403320a_VEP()) &&
                         (template.Template_403320c_VEP());
            }
            else if (ProductFamily == "TPS")
            {
                result = (template.Template_403320a_TPS()) &&
                        (template.Template_403320c_TPS());
            }
            return result;
        }






        /// <summary>
        /// The intent of this test is to Enable/Disable print protocols/device features
        /// ID          : 406618
        /// Network     : IPv4
        /// Connectivity: Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406618, Category = "Basic Single Device Network Management", Connectivity = ConnectivityType.Wired, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS, Description = "Enable/Disable print protocols/device features ")]
        public bool Test_406618()
        {
            bool result = false;
            TelnetTemplates template = new TelnetTemplates(_activityData.PrinterIP);

            if (ProductFamily == "VEP")
            {
                result = template.Template_403369_VEP();
            }
            else if (ProductFamily == "TPS")
            {
                result = template.Template_403369_TPS();
            }
            return result;

        }



        /// <summary>
        /// The intent of this test is to Enable/Disable print protocols/device features
        /// ID          : 406637
        /// Network     : IPv4
        /// Connectivity: Wireless
        /// </summary>
        [TestDetailsAttribute(Id = 406637, Category = "Basic Single Device Network Management", Connectivity = ConnectivityType.Wireless, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS, Description = "Enable/Disable print protocols/device features ")]
        public bool Test_406637()
        {
            bool result = false;
            TelnetTemplates template = new TelnetTemplates(_activityData.PrinterIP);

            if (ProductFamily == "VEP")
            {
                result = template.Template_403369_VEP();
            }
            else if (ProductFamily == "TPS")
            {
                result = template.Template_403369_TPS();
            }
            return result;
        }



        /// <summary>
        /// The intent of this test is to verify telnet connection status when it is disabled
        /// ID          : 406710
        /// Network     : IPv4
        /// Connectivity: Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406710, Category = "Basic Single Device Network Management", Connectivity = ConnectivityType.Wired, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS, Description = "Verify telnet connection status when it is disabled")]
        public bool Test_406710()
        {

            bool result = false;
            TelnetTemplates template = new TelnetTemplates(_activityData.PrinterIP);

            if (ProductFamily == "VEP")
            {
                result = template.Template_403381(CreateAdapter());
            }
            else if (ProductFamily == "TPS")
            {
                result = template.Template_403381_TPS(CreateAdapter());
            }
            return result;

        }



        /// <summary>
        /// The intent of this test is to verify telnet connection status when it is disabled
        /// ID          : 406712
        /// Network     : IPv6
        /// Connectivity: Wireless
        /// </summary>
        [TestDetailsAttribute(Id = 406712, Category = "Basic Single Device Network Management", Connectivity = ConnectivityType.Wireless, ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS, Description = "Verify telnet connection status when it is disabled")]
        public bool Test_406712()
        {
            bool result = false;
            TelnetTemplates template = new TelnetTemplates(_activityData.PrinterIP);

            if (ProductFamily == "VEP")
            {
                result = template.Template_403381(CreateAdapter());
            }
            else if (ProductFamily == "TPS")
            {
                result = template.Template_403381_TPS(CreateAdapter());
            }
            return result;
        }




        #endregion

        #region LFP Tests
        /// <summary>
        /// The intent of this test is to validate telnet connection with/without password
        /// ID          : 406581
        /// Network     : IPv4
        /// Connectivity: Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406581, Category = "Basic Single Device Network Management", Connectivity = ConnectivityType.Wired, ProductCategory = ProductFamilies.LFP, Description = "Validate telnet connection with/without password")]
        public bool Test_406581_LFP()
        {
            bool result = false;
            TelnetTemplates template = new TelnetTemplates(_activityData.PrinterIP);
            result = (template.Template_403320a_LFP()) &&
                     (template.Template_403320c_LFP());
            return result;
        }



        /// <summary>
        /// The intent of this test is to validate telnet connection with/without password
        /// ID          : 406594
        /// Network     : IPv4
        /// Connectivity: Wireless
        /// </summary>
        [TestDetailsAttribute(Id = 406594, Category = "Basic Single Device Network Management", Connectivity = ConnectivityType.Wireless, ProductCategory = ProductFamilies.LFP, Description = "Validate telnet connection with/without password")]
        public bool Test_406594_LFP()
        {
            bool result = false;
            TelnetTemplates template = new TelnetTemplates(_activityData.PrinterIP);
            result = (template.Template_403320a_LFP()) &&
                     (template.Template_403320c_LFP());
            return result;
        }





        /// <summary>
        /// The intent of this test is to Enable/Disable print protocols/device features
        /// ID          : 406618
        /// Network     : IPv4
        /// Connectivity: Wired
        /// </summary>
        [TestDetailsAttribute(Id = 406618, Category = "Basic Single Device Network Management", Connectivity = ConnectivityType.Wired, ProductCategory = ProductFamilies.LFP, Description = "Enable/Disable print protocols/device features")]
        public bool Test_406618_LFP()
        {
            TelnetTemplates template = new TelnetTemplates(_activityData.PrinterIP);
            return template.Template_403369_LFP();
        }



        /// <summary>
        /// The intent of this test is to Enable/Disable print protocols/device features
        /// ID          : 406637
        /// Network     : IPv4
        /// Connectivity: Wireless
        /// </summary>
        [TestDetailsAttribute(Id = 406637, Category = "Basic Single Device Network Management", Connectivity = ConnectivityType.Wireless, ProductCategory = ProductFamilies.LFP, Description = "Enable/Disable print protocols/device features")]
        public bool Test_406637_LFP()
        {
            TelnetTemplates template = new TelnetTemplates(_activityData.PrinterIP);
            return template.Template_403369_LFP();
        }



        /// TEMPLATE: 403381

        /// <summary>
        /// The intent of this test is to verify telnet connection status when it is disabled
        /// ID          : 406710
        /// Network     : IPv4
        /// Connectivity: Wired
        /// </summary>
        ///[TestDetailsAttribute(Id = 406710, Category = "Basic Single Device Network Management", Description = "Verify telnet connection status when it is disabled")]
        public void Test_406710_LFP()
        {
            TelnetTemplates template = new TelnetTemplates(_activityData.PrinterIP);
            template.Template_403381(CreateAdapter());
        }


        /// <summary>
        /// The intent of this test is to verify telnet connection status when it is disabled
        /// ID          : 406712
        /// Network     : IPv6
        /// Connectivity: Wireless
        /// </summary>
        ///[TestDetailsAttribute(Id = 406712, Category = "Basic Single Device Network Management", Description = "Verify telnet connection status when it is disabled")]
        public void Test_406712_LFP()
        {
            TelnetTemplates template = new TelnetTemplates(_activityData.PrinterIP);
            template.Template_403381(CreateAdapter());
        }


        #endregion



    }
}
