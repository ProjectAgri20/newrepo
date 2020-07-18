using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.RadiusServer;

namespace HP.ScalableTest.Plugin.DotOneX
{
    /// <summary>
    /// 802.1X Test cases
    /// </summary>
    internal class DotOneXTests : CtcBaseTests
    {
        #region Local Variables

        /// <summary>
        /// Instance of DotOneXActivityData
        /// </summary>
        DotOneXActivityData _activityData;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="activityData"></param>
        public DotOneXTests(DotOneXActivityData activityData) : base(activityData.ProductName)
        {
            _activityData = activityData;
            ProductFamily = activityData.ProductFamily;
            Sliver = "802.1x Authentication";
        }

        #endregion

        #region DotoneX Tests

        /// <summary>
        /// Step1: Verify EAP-TLS authentication
        /// Configure the device with a successful TLS authentication
        /// Change the encryption strength setting: Low, Medium and High.Authenticate and print CRC or bomb job. 
        /// Expected : Authentication should be successful.The print job should complete normally
        /// </summary>
        [TestDetailsAttribute(Id = 82988, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "All Encryption Suites")]
        public bool Test_82988()
        {
            return DotOneXTemplates.AuthenticationWithEncryption(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        /// <summary>
        /// Step 1 : Verify PEAP authentication
        /// Configure the device with a successful PEAP authentication
        /// Change the encryption strength setting: Low, Medium and High.Authenticate and print CRC or bomb job.
        /// Expected : 
        /// Authentication should be successful.The print job should complete normally.  
        /// </summary>
        [TestDetailsAttribute(Id = 82990, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "All Encryption Suites")]
        public bool Test_82990()
        {
            return DotOneXTemplates.AuthenticationWithEncryption(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 83003, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Reauthenticate On Apply")]
        public bool Test_83003()
        {
            return DotOneXTemplates.ValidateReAuthentication(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 728596, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Reauthenticate On Apply")]
        public bool Test_728596()
        {
            return DotOneXTemplates.ValidateReAuthentication(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 732842, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Description = "Hostname Change")]
        public bool Test_732842()
        {
            return DotOneXTemplates.AuthenticationWithHostname(ref _activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 10000, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Description = "Hostname Change")]
        public bool Test_10000()
        {
            return DotOneXTemplates.AuthenticationWithHostname(ref _activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 83001, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.LFP, Description = "LAA Change")]
        public bool Test_83001()
        {
            return DotOneXTemplates.AuthenticationWithLaa(ref _activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 714932, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.LFP, Description = "LAA Change")]
        public bool Test_714932()
        {
            return DotOneXTemplates.AuthenticationWithLaa(ref _activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 82997, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Hose Break")]
        public bool Test_82997()
        {
            return DotOneXTemplates.AuthenticationAfterHosebreak(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 82998, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Hose Break")]
        public bool Test_82998()
        {
            return DotOneXTemplates.AuthenticationAfterHosebreak(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 82999, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "IP Configuration Change")]
        public bool Test_82999()
        {
            return DotOneXTemplates.AuthenticationWithIpConfigMethodChange(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 83000, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "IP Configuration Change")]
        public bool Test_83000()
        {
            return DotOneXTemplates.AuthenticationWithIpConfigMethodChange(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 83002, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Link Speed Change")]
        public bool Test_83002()
        {
            return DotOneXTemplates.AuthenticationWithLinkSpeed(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 728595, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Link Speed Change")]
        public bool Test_728595()
        {
            return DotOneXTemplates.AuthenticationWithLinkSpeed(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 82985, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "EAP-TLS Basic Authentication")]
        public bool Test_82985()
        {
            return DotOneXTemplates.ValidateAuthentication(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 82989, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "PEAP Basic Authentication")]
        public bool Test_82989()
        {
            return DotOneXTemplates.ValidateAuthentication(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 92147, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Expired Certificates On Printer")]
        public bool Test_92147()
        {
            return DotOneXTemplates.ExpiredCertificatesOnPrinter(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 729819, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Expired Certificates On Printer")]
        public bool Test_729819()
        {
            return DotOneXTemplates.ExpiredCertificatesOnPrinter(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 92148, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Expired Certificates On Server")]
        public bool Test_92148()
        {
            return DotOneXTemplates.ExpiredCertificatesOnServer(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 730311, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Expired Certificates On Server")]
        public bool Test_730311()
        {
            return DotOneXTemplates.ExpiredCertificatesOnServer(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 472709, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Both", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Description = "Fail Safe Option Enabled")]
        public bool Test_472709()
        {
            return DotOneXTemplates.AuthenticationWithFailSafeEnabled(_activityData, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 472710, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Both", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Fail Safe Option Disabled")]
        public bool Test_472710()
        {
            return DotOneXTemplates.AuthenticationWithFailSafeDisabled(_activityData, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 670899, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Description = "Changes To Certificate Store")]
        public bool Test_670899()
        {
            return DotOneXTemplates.AuthenticationWithDifferentCertificateStore(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 730304, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Description = "Changes To Certificate Store")]
        public bool Test_730304()
        {
            return DotOneXTemplates.AuthenticationWithDifferentCertificateStore(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 82996, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Subordinate CA Certificates")]
        public bool Test_82996()
        {
            return DotOneXTemplates.AuthenticationWithDifferentServerCertificates(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 730310, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Subordinate CA Certificates")]
        public bool Test_730310()
        {
            return DotOneXTemplates.AuthenticationWithDifferentServerCertificates(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 82995, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Description = "Id Certificate Not Associated With Radius Server")]
        public bool Test_82995()
        {
            return DotOneXTemplates.AuthenticationWithUnAssociatedIdCertificate(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 730309, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Description = "Id Certificate Not Associated With Radius Server")]
        public bool Test_730309()
        {
            return DotOneXTemplates.AuthenticationWithUnAssociatedIdCertificate(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 82991, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Description = "Certificate Signed By Certificate Service On Same Machine")]
        public bool Test_82991()
        {
            return DotOneXTemplates.AuthenticationWithAssociatedIdCertificate(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 730306, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Description = "Certificate Signed By Certificate Service On Same Machine")]
        public bool Test_730306()
        {
            return DotOneXTemplates.AuthenticationWithAssociatedIdCertificate(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 680128, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP, Description = "TPM")]
        public bool Test_680128()
        {
            return DotOneXTemplates.AuthenticationWithAssociatedIdCertificate(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString(), true);
        }

        [TestDetailsAttribute(Id = 730312, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP, Description = "TPM")]
        public bool Test_730312()
        {
            return DotOneXTemplates.AuthenticationWithAssociatedIdCertificate(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString(), true);
        }

        [TestDetailsAttribute(Id = 82994, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Description = "Untrusted Certificate")]
        public bool Test_82994()
        {
            return DotOneXTemplates.AuthenticationWithUnTrustedCertificate(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 730308, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Description = "Untrusted Certificate")]
        public bool Test_730308()
        {
            return DotOneXTemplates.AuthenticationWithUnTrustedCertificate(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 718501, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify IPSec Behavior With DotOnexConfigurations for Certificates")]
        public bool Test_718501()
        {
            return DotOneXTemplates.VerifyIPSecBehaviourWithDotOnexConfigurations_Certificates(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString(), true);
        }

        [TestDetailsAttribute(Id = 729262, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify IPSec Behavior With DotOnexConfigurations in Peap mode")]
        public bool Test_729262()
        {
            return DotOneXTemplates.VerifyIPSecBehaviourWithDotOnexConfigurations_Peap(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 576736, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify 802.1x Authentication in FIPS mode")]
        public bool Test_576736()
        {
            return DotOneXTemplates.VerifyDotOnexConfigurationsinFIPS(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 82993, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Description = "Invalid CA Certificate on Power Cycle")]
        public bool Test_82993()
        {
            return DotOneXTemplates.AuthenticationWithInvalidCAPowerCycle(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 729636, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Description = "Invalid CA Certificate on Power Cycle")]
        public bool Test_729636()
        {
            return DotOneXTemplates.AuthenticationWithInvalidCAPowerCycle(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

		[TestDetailsAttribute(Id = 1, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.LFP | ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.InkJet, Description = "Switching between authentication modes")]
		public bool Test_1()
		{
			return DotOneXTemplates.AuthenticationWithAlternateModes(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
		}

        [TestDetailsAttribute(Id = 2, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.LFP | ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.InkJet, Description = "Invalid Username")]
        public bool Test_2()
        {
            return DotOneXTemplates.AuthenticationWithInvalidUserName(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 3, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.LFP | ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.InkJet, Description = "Invalid Username")]
        public bool Test_3()
        {
            return DotOneXTemplates.AuthenticationWithInvalidUserName(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 4, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.LFP | ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.InkJet, Description = "Invalid Password")]
        public bool Test_4()
        {
            return DotOneXTemplates.AuthenticationWithInvalidUserName(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString(), true);
        }

        [TestDetailsAttribute(Id = 5, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.LFP | ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.InkJet, Description = "Invalid Password")]
        public bool Test_5()
        {
            return DotOneXTemplates.AuthenticationWithInvalidUserName(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString(), true);
        }

        [TestDetailsAttribute(Id = 6, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.LFP | ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.InkJet, Description = "Power cycle")]
        public bool Test_6()
        {
            return DotOneXTemplates.AuthenticationAfterPowercycle(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 7, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.LFP| ProductFamilies.InkJet | ProductFamilies.VEP | ProductFamilies.TPS, Description = "Power cycle")]
        public bool Test_7()
        {
            return DotOneXTemplates.AuthenticationAfterPowercycle(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 8, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Description = "Cold Reset")]
        public bool Test_8()
        {
            return DotOneXTemplates.AuthenticationAfterColdReset(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 9, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Description = "Cold Reset")]
        public bool Test_9()
        {
            return DotOneXTemplates.AuthenticationAfterColdReset(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 10, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.VEP | ProductFamilies.TPS, Description = "Server Id")]
        public bool Test_10()
        {
            return DotOneXTemplates.AuthenticationWithServerID(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 11, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.VEP | ProductFamilies.TPS, Description = "Server Id")]
        public bool Test_11()
        {
            return DotOneXTemplates.AuthenticationWithServerID(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 12, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP, Description = "Control panel reset")]
        public bool Test_12()
        {
            return DotOneXTemplates.ResetAuthenticationControlPanel(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 13, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "TLS", ProductCategory = ProductFamilies.VEP, Description = "Control panel reset")]
        public bool Test_13()
        {
            return DotOneXTemplates.ResetAuthenticationControlPanel(_activityData, AuthenticationMode.EAPTLS, CtcUtility.GetTestId().ToString());
        }

        #endregion

        #region DotoneX Print Plug-in Tests

        [TestDetailsAttribute(Id = 14, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "Verify printing with 802.1x enabled")]
        public bool Test_PrintWithDot1x()
        {
            return DotOneXTemplates.VerifyPrintWithDotoneX(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString());
        }

        [TestDetailsAttribute(Id = 15, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv6, Category = "PEAP", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "Verify printing with 802.1x enabled-IPV6")]
        public bool Test_PrintWithDot1x_IPV6()
        {
            return DotOneXTemplates.VerifyPrintWithDotoneX(_activityData, AuthenticationMode.PEAP, CtcUtility.GetTestId().ToString(), true);
        }

        #endregion

    }
}
