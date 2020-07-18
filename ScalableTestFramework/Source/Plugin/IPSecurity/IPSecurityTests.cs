using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.IPSecurity
{
    /// <summary>
    /// IPSecurity Test cases
    /// </summary>
    internal class IPSecurityTests : CtcBaseTests
    {
        #region Local Variables

        /// <summary>
        /// Instance of IPSecurityActivityData
        /// </summary>
        IPSecurityActivityData _activityData;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="activityData"></param>
        public IPSecurityTests(IPSecurityActivityData activityData)
            : base(activityData.ProductName)
        {
            _activityData = activityData;
            ProductFamily = activityData.ProductFamily;
            Sliver = "IP Security";
        }

        #endregion

        #region Test Cases Implementation

        ///////////////////////////////////////////// Web UI tests /////////////////////////////////////////////

        #region IPSecurity Tests
        /// <summary>
        ///  Test case Id=83066
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83066, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web UI", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Enable & Disable - Policy")]
        public bool Test_83066()
        {
            return IPSecurityTemplates.VerifyEnableRuleFunctionality(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83069
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83069, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web UI", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Allow & Drop - Rules")]
        public bool Test_83069()
        {
            return IPSecurityTemplates.VerifyAllowDropFunctionality(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
		///  Test case Id=756109
		/// </summary>
		/// <returns>Returns true if the test is passed else returns false</returns>
		[TestDetailsAttribute(Id = 756109, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web UI", ProductCategory = ProductFamilies.InkJet, Description = "Allow & Drop - Rules")]
        public bool Test_756109()
        {
            return IPSecurityTemplates.VerifyIpsecDefaultAllowDropFunctionality(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
		///  Test case Id=756129
		/// </summary>
		/// <returns>Returns true if the test is passed else returns false</returns>
		[TestDetailsAttribute(Id = 756129, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web UI", ProductCategory = ProductFamilies.InkJet, Description = "IPSec with Custom IPSec Template")]
        public bool Test_756129()
        {
            return IPSecurityTemplates.VerifyIpsecWithCustomIpsecTemplate(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83073
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83073, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web UI", ProductCategory = ProductFamilies.VEP, Description = "Verify Security Debug Page")]
        public bool Test_83073()
        {
            return IPSecurityTemplates.VerifyIPSecurityDebugPage(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=739068
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 739068, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web UI", ProductCategory = ProductFamilies.VEP, Description = "Enable & Disable-Front Panel - Policy")]
        public bool Test_739068()
        {
            return IPSecurityTemplates.VerifyEnableDisablePolicyUsingFrontPanel(_activityData, CtcUtility.GetTestId());
        }

        ///  Test case Id=729261
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 729261, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web UI", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Rule behavior after Reinitialize")]
        public bool Test_729261()
        {
            return IPSecurityTemplates.VerifyIPSecRuleBehaviourAfterReinitialize(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83060
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83060, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web UI", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Behavior using Advanced options")]
        public bool Test_83060()
        {
            return IPSecurityTemplates.VerifyIPSecurityAdvancedOptions(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=729260
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 729260, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web UI", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Encapsulation in Edit Mode")]
        public bool Test_729260()
        {
            return IPSecurityTemplates.VerifyIPSecEncapsulationMode(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83056
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83056, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web UI", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Behavior after deletion of Address, Service and IPSec templates")]
        public bool Test_83056()
        {
            return IPSecurityTemplates.VerifyDeletionOfCustomAddressTemplate(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=729258
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 729258, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web UI", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Modification of Templates from Certificates-Certificates")]
        public bool Test_729258()
        {
            return IPSecurityTemplates.VerifyModificationOfIPSecurityTemplatesFromCertificates(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83061
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83061, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web UI", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Edit- Address/Service/IPsec Templates")]
        public bool Test_83061()
        {
            return IPSecurityTemplates.VerifyEditFunctionalityOfAddressServiceTemplates(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=729259
        /// </summary>
        /// <returns></returns>
        [TestDetailsAttribute(Id = 729259, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web UI", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Edit Kerberos Template from Configuration file to Manual Settings")]
        public bool Test_729259()
        {
            return IPSecurityTemplates.VerifyModificationOfIPSecurityTemplatesFromKerberos(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=729257
        /// </summary>
        /// <returns></returns>
        [TestDetailsAttribute(Id = 729257, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web UI", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Edit Kerberos Template from PreShared Key to Certificates and from Certificates to Kerberos")]
        public bool Test_729257()
        {
            return IPSecurityTemplates.VerifyIdenityAuthenticationOptions(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=756122
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 756122, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web UI", ProductCategory = ProductFamilies.InkJet, Description = "Verify check firewall behaviour with powercycle")]
        public bool Test_756122()
        {
            return IPSecurityTemplates.VerifyFirewallBehaviourWithPowercycle(_activityData, CtcUtility.GetTestId());
        }

        ///////////////////////////////////////////// Dynamic Key tests /////////////////////////////////////////////

        /// <summary>
        ///  Test case Id=729254
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 729254, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "HTTPS Accessibility")]
        public bool Test_729254()
        {
            return IPSecurityTemplates.VerifyHttpsBehaviorWithoutRule(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83007
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83007, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Enable & Disable - IPV4")]
        public bool Test_83007()
        {
            return IPSecurityTemplates.VerifyRuleWithEnableAndDisableState(_activityData, DefaultAddressTemplates.AllIPv4Addresses, DefaultServiceTemplates.AllServices, IKESecurityStrengths.HighInteroperabilityLowsecurity, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83009
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83009, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Enable & Disable - State full IPV6")]
        public bool Test_83009()
        {
            return IPSecurityTemplates.VerifyRuleWithEnableAndDisableState(_activityData, DefaultAddressTemplates.AllIPv6Addresses, DefaultServiceTemplates.AllPrintServices, IKESecurityStrengths.MediumInteroperabilityMediumsecurity, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83010
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83010, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Enable & Disable - LinkLocal IPV6")]
        public bool Test_83010()
        {
            return IPSecurityTemplates.VerifyRuleWithEnableAndDisableState(_activityData, DefaultAddressTemplates.AllIPv6NonLinkLocal, DefaultServiceTemplates.Custom, IKESecurityStrengths.HighSecurityWithAdvancedFeatures, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83011
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83011, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Enable & Disable - Stateless IPV6")]
        public bool Test_83011()
        {
            return IPSecurityTemplates.VerifyRuleWithEnableAndDisableState(_activityData, DefaultAddressTemplates.AllIPv6LinkLocal, DefaultServiceTemplates.AllServices, IKESecurityStrengths.MediumInteroperabilityMediumsecurity, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83045
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83045, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Replay Detection")]
        public bool Test_83045()
        {
            return IPSecurityTemplates.VerifyRuleWithReplayDetection(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83041
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83041, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify AH Authentication")]
        public bool Test_83041()
        {
            return IPSecurityTemplates.VerifyRuleWithOnlyAH(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83040
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83040, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify ESP Authentication")]
        public bool Test_83040()
        {
            return IPSecurityTemplates.VerifyRuleWithOnlyESP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83027
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83027, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Authentication with Sha1 Certificates")]
        public bool Test_83027()
        {
            return IPSecurityTemplates.VerfiyRuleWithSha1Certificates(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=92155
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 92155, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Expired certificates")]
        public bool Test_92155()
        {
            return IPSecurityTemplates.VerifyRuleWithExpriedCertificates(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83038
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83038, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Re-Authentication with SA Life size")]
        public bool Test_83038()
        {
            return IPSecurityTemplates.VerifyReAuthenticationWithSALifeSize(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83037
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83037, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Re-Authentication with SA Lifetime")]
        public bool Test_83037()
        {
            return IPSecurityTemplates.VerifyReAuthenticationWithSALifetime(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83039
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83039, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Re-Authentication with SA Lifetime and Life size")]
        public bool Test_83039()
        {
            return IPSecurityTemplates.VerifyReAuthenticationWithSALifeSizeandSALifetime(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=92154
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 92154, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Mismatch PSK")]
        public bool Test_92154()
        {
            return IPSecurityTemplates.VerifyRuleWithMismatchPSK(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83029
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83029, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Certificates with High strength")]
        public bool Test_83029()
        {
            return IPSecurityTemplates.VerifyRuleWithCertificates(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83044
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83044, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Tunnel option")]
        public bool Test_83044()
        {
            return IPSecurityTemplates.VerifyRuleWithTunnel(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83046
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83046, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify PFS option")]
        public bool Test_83046()
        {
            return IPSecurityTemplates.VerifyPFSOption(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=458093
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 458093, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Connectivity from different Clients")]
        public bool Test_458093()
        {
            return IPSecurityTemplates.VerifyConnectivityFromDifferentClients(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83013
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83013, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Printer accessibility with custom IPv6 address range")]
        public bool Test_83013()
        {
            return IPSecurityTemplates.VerifyCustomIPV6AddressRange(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83014
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 83014, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Printer accessibility with custom IPv6 address prefix")]
        public bool Test_83014()
        {
            return IPSecurityTemplates.VerifyCustomIPV6AddressPrefix(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=714555
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 714555, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify Printer accessibility with IPV6 Option Enabled/Disabled")]
        public bool Test_714555()
        {
            return IPSecurityTemplates.VerifyPrinterAccessibilityWithIPV6Option(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=729255
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 729255, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Printer accessibility with IPV4 Address")]
        public bool Test_729255()
        {
            return IPSecurityTemplates.VerifyPrinterAccessibilityWithIPV4Address(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83032
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 83032, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify Printer accessibility with Kerberos configuration: DES")]
        public bool Test_83032()
        {
            return IPSecurityTemplates.VerifyRuleWithKerberos(_activityData, CtcUtility.GetTestId(), Encryptions.DES, IKESecurityStrengths.LowInteroperabilityHighsecurity);
        }

        /// <summary>
        ///  Test case Id=83033
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 83033, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify Printer accessibility with Kerberos configuration: AES128")]
        public bool Test_83033()
        {
            return IPSecurityTemplates.VerifyRuleWithKerberos(_activityData, CtcUtility.GetTestId(), Encryptions.AES256, IKESecurityStrengths.MediumInteroperabilityMediumsecurity, defaultServiceTemplate: DefaultServiceTemplates.AllManagementServices);
        }

        /// <summary>
        ///  Test case Id=83034
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 83034, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify Printer accessibility with Kerberos configuration: AES256")]
        public bool Test_83034()
        {
            return IPSecurityTemplates.VerifyRuleWithKerberos(_activityData, CtcUtility.GetTestId(), Encryptions.AES128, IKESecurityStrengths.MediumInteroperabilityMediumsecurity);
        }

        /// <summary>
        ///  Test case Id=458092
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 458092, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify Print Rule With Kerberos")]
        public bool Test_458092()
        {
            return IPSecurityTemplates.VerifyPrintRuleWithKerberos(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83071
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 83071, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Printer Accessibility After Reboot")]
        public bool Test_83071()
        {
            return IPSecurityTemplates.VerifyPrinterAccessibilityAfterReboot(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83072
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 83072, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Printer Accessibility After Cold Reset")]
        public bool Test_83072()
        {
            return IPSecurityTemplates.VerifyPrinterAccessibilityAfterColdReset(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=714553
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 714553, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Printer Accessibility After HoseBreak")]
        public bool Test_714553()
        {
            return IPSecurityTemplates.VerifyPrinterAccessibilityAfterHoseBreak(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=729256
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 729256, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify Kerberos Manual Configuration")]
        public bool Test_729256()
        {
            return IPSecurityTemplates.VerifyKerberosManualConfiguration(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=266826
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 266826, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = " Verify Web UI performance when all DNS traffic is allowed")]
        public bool Test_266826()
        {
            return IPSecurityTemplates.VerifyWebUIPerformanceWithDNSTraffic(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=266827
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 266827, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify WebUI performance when  DNS traffic is not included as part of the IPSec service template")]
        public bool Test_266827()
        {
            return IPSecurityTemplates.VerifyWebUIPerformanceWithOutDNSTraffic(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=729288
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 729288, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify Device Behavior with ACL")]
        public bool Test_729288()
        {
            return IPSecurityTemplates.VerifyDeviceBehaviourWithACL(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=611750
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 611750, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Device Behavior With Multiple Rules Creation and Deletion")]
        public bool Test_611750()
        {
            return IPSecurityTemplates.VerifyDeviceBehaviourWithMultipleRulesCreationAndDeletion(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=517173
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 517173, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Connectivity From Different Clients")]
        public bool Test_517173()
        {
            return IPSecurityTemplates.VerifyConnectivityFromDifferentClients(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=408956
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 408956, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify IP Security behavior across subnet")]
        public bool Test_408956()
        {
            return IPSecurityTemplates.VerifyIPsecAcrossSubnets(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83075
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 83075, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Large File Printing From Multiple Hosts")]
        public bool Test_83075()
        {
            return IPSecurityTemplates.VerifyLargeFilePrintFromMultipleHosts(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83074
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 83074, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Long Duration Ping From Multiple Hosts")]
        public bool Test_83074()
        {
            return IPSecurityTemplates.VerifyLongDurationPingFromMultipleHosts(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83020
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 83020, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Rule With Custom Services")]
        public bool Test_83020()
        {
            return IPSecurityTemplates.VerifyRuleWithCustomServices(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=83052
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 83052, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Rule With Different Authentication and Encryption")]
        public bool Test_83052()
        {
            return IPSecurityTemplates.VerifyRuleWithDifferentAuthentication_Encryption(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=408961
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 408961, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Printer Accessibility With Multiple Rules")]
        public bool Test_408961()
        {
            return IPSecurityTemplates.VerifyPrinterAccessibilityWithMultipleRules(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=670714
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 670714, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP, Description = "Verify Printer Accessibility With Multiple Certificates")]
        public bool Test_670714()
        {
            return IPSecurityTemplates.VerifyPrinterAccessibilityWithMultipleCertificates(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=670898
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 670898, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP, Description = "Verify Different ID/CA Certificate")]
        public bool Test_670898()
        {
            return IPSecurityTemplates.VerifyDifferentIDCACertificate(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=678824
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 678824, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP, Description = "Verify Services With Different IPAddresses")]
        public bool Test_678824()
        {
            return IPSecurityTemplates.VerifyServicesWithDifferentIPAddresses(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=729286
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 729286, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP, Description = "Verify In/Out Bound Communication")]
        public bool Test_729286()
        {
            return IPSecurityTemplates.VerifyIn_OutBoundCommunication(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=729287
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 3001, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP, Description = "Verify Printer Accessibility From Multiple Clients")]
        public bool Test_3001()
        {
            return IPSecurityTemplates.VerifyPrinterAccessiblityFromMultipleClients(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=517170
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 517170, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.VEP, Description = "Verify Scan to Network Folder")]
        public bool Test_517170()
        {
            return IPSecurityTemplates.VerifyScantoNetworkFolder(_activityData, CtcUtility.GetTestId());
        }
        /// <summary>
        ///  Test case Id=755968
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 755968, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv6, Category = "Dynamic Keys", ProductCategory = ProductFamilies.InkJet, Description = "Verify IPSec Functionality using IPv6 address range")]
        public bool Test_755968()
        {
            return IPSecurityTemplates.VerifyIPSecFunctionalityWithAddressRange(_activityData, CtcUtility.GetTestId());
        }
        /// <summary>
        ///  Test case Id=755973
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 755973, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.InkJet, Description = "Verify adding custom services using new Service Template")]
        public bool Test_755973()
        {
            return IPSecurityTemplates.VerifyAddingCustomServicesUsingNewServiceTemplate(_activityData, CtcUtility.GetTestId());
        }
        /// <summary>
        ///  Test case Id=756114
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 756114, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.InkJet, Description = "Verify IPSec using Different Authentication and Encryption")]
        public bool Test_756114()
        {
            return IPSecurityTemplates.VerifyRuleWithDifferentAuthentication_EncryptionMethods(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=756069
        /// </summary>
        /// <returns>true if the test passes, false otherwise</returns>
        [TestDetailsAttribute(Id = 756069, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Dynamic Keys", ProductCategory = ProductFamilies.InkJet, Description = "Verify IPSec rules with different services")]
        public bool Test_756069()
        {
            return IPSecurityTemplates.VerifyMultipleRulesWithDifferentServices(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 577016, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "FIPS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify IP-Sec Kerberos authentication in FIPS mode")]
        public bool Test_577016()
        {
            return IPSecurityTemplates.VerifyIPSecKerberosAuthenticationinFIPS(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 729263, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Linux", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify IPv6 Accessibility")]
        public bool Test_729263()
        {
            return IPSecurityTemplates.VerifyAccessbilityWithIPv6Address(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 83051, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Linux", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify Manual Key with Tunnel mode")]
        public bool Test_83051()
        {
            return IPSecurityTemplates.VerifyAccessbilityWithManualKeys(_activityData, CtcUtility.GetTestId(), EncapsulationType.Tunnel);
        }

        [TestDetailsAttribute(Id = 83050, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Linux", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify Manual Key with Transport mode")]
        public bool Test_83050()
        {
            return IPSecurityTemplates.VerifyAccessbilityWithManualKeys(_activityData, CtcUtility.GetTestId(), EncapsulationType.Transport);
        }

        [TestDetailsAttribute(Id = 83012, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Linux", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify IPv6 Accessibility from different machines")]
        public bool Test_83012()
        {
            return IPSecurityTemplates.VerifyIPv6AccessbilityFromTwoMachines(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 505908, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Linux", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Description = "Verify Accessibility with different authentication mode")]
        public bool Test_505908()
        {
            return IPSecurityTemplates.VerifyAccessbilityWithDifferentAuthentication(_activityData, CtcUtility.GetTestId());
        }

        #endregion

        #region Print with IPSecurity Enabled

        /// <summary>
        ///  Test case Id=1
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 1, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Print Cross Plug-in", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify Print with IPSec")]
        public bool Test_VerifyPrintWithIPSec()
        {
            return IPSecurityTemplates.VerifyPrintWithIPSec(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        ///  Test case Id=2
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 2, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv6, Category = "Print Cross Plug-in", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Verify Print with IPSec")]
        public bool Test_VerifyPrintWithIPSec_IPV6()
        {
            return IPSecurityTemplates.VerifyPrintWithIPSec(_activityData, CtcUtility.GetTestId(), true);
        }

        #endregion

        #endregion
    }

}
