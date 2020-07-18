using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.Firewall
{
    internal class FirewallTests : CtcBaseTests
    {
        #region Local Variables

        /// <summary>
        /// Instance of FirewallActivityData
        /// </summary>
        FirewallActivityData _activityData;

        #endregion

        #region Constructor

        /// <summary>
        /// FirewallTests Constructor
        /// </summary>
        /// <param name="activityData"><see cref=" FirewallActivityData"/></param>
        public FirewallTests(FirewallActivityData activityData)
            : base(activityData.ProductName)
        {
            _activityData = activityData;
            ProductFamily = activityData.ProductFamily;
            Sliver = "Firewall";
        }

        #endregion

        #region All IP Address

        [TestDetailsAttribute(Id = 51001, Category = "All IP Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IP Addresses - All Services")]
        public bool Test_51001()
        {
            return FirewallTemplates.Template_AllIPAddress_AllService(_activityData);
        }

        [TestDetailsAttribute(Id = 51002, Category = "All IP Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IP Addresses - All Print Services")]
        public bool Test_51002()
        {
            return FirewallTemplates.Template_AllIPAddress_AllPrintService(_activityData);
        }

        [TestDetailsAttribute(Id = 51003, Category = "All IP Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IP Addresses - All Management Services")]
        public bool Test_51003()
        {
            return FirewallTemplates.Template_AllIPAddress_AllManagementService(_activityData);
        }

        [TestDetailsAttribute(Id = 51004, Category = "All IP Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IP Addresses - All Discovery Services")]
        public bool Test_51004()
        {
            return FirewallTemplates.Template_AllIPAddress_AllWSDiscoveryService(_activityData);
        }

        [TestDetailsAttribute(Id = 51005, Category = "All IP Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "All IP Addresses - All Microsoft Web Services")]
        public bool Test_51005()
        {
            return FirewallTemplates.Template_AllIPAddress_AllMicrosoftWebService(_activityData);
        }

        #endregion  All IP Address

        #region All IPv4 Address

        [TestDetailsAttribute(Id = 51006, Category = "All IPv4 Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IPv4 Addresses - All Services")]
        public bool Test_51006()
        {
            return FirewallTemplates.Template_AllIPv4Address_AllService(_activityData);
        }

        [TestDetailsAttribute(Id = 51007, Category = "All IPv4 Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IPv4 Addresses - All Print Services")]
        public bool Test_51007()
        {
            return FirewallTemplates.Template_AllIPv4Address_AllPrintService(_activityData, DefaultAddressTemplates.AllIPv4Addresses);
        }

        [TestDetailsAttribute(Id = 51008, Category = "All IPv4 Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IPv4 Addresses - All Management Services")]
        public bool Test_51008()
        {
            return FirewallTemplates.Template_AllIPv4Address_AllManagementService(_activityData, DefaultAddressTemplates.AllIPv4Addresses);
        }

        [TestDetailsAttribute(Id = 51009, Category = "All IPv4 Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IPv4 Addresses - All Discovery Services")]
        public bool Test_51009()
        {
            return FirewallTemplates.Template_AllIPv4Address_AllDiscoveryService(_activityData, DefaultAddressTemplates.AllIPv4Addresses);
        }

        [TestDetailsAttribute(Id = 51010, Category = "All IPv4 Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "All IPv4 Address - All Microsoft Web Services")]
        public bool Test_51010()
        {
            return FirewallTemplates.Template_AllIPv4Address_AllMicrosoftWebService(_activityData, DefaultAddressTemplates.AllIPv4Addresses);
        }

        #endregion  All IPv4 Address

        #region All IPv6 Address

        [TestDetailsAttribute(Id = 51011, Category = "All IPv6 Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IPv6 Addresses - All Services")]
        public bool Test_51011()
        {
            return FirewallTemplates.Template_AllIPv6Address_AllService(_activityData);
        }

        [TestDetailsAttribute(Id = 51012, Category = "All IPv6 Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IPv6 Addresses - All Print Services")]
        public bool Test_51012()
        {
            return FirewallTemplates.Template_AllIPv6Address_AllPrintService(_activityData);
        }

        [TestDetailsAttribute(Id = 51013, Category = "All IPv6 Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IPv6 Addresses - All Management Services")]
        public bool Test_51013()
        {
            return FirewallTemplates.Template_AllIPv6Address_AllManagementService(_activityData);
        }

        [TestDetailsAttribute(Id = 51014, Category = "All IPv6 Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IPv6 Addresses - All Discovery Services")]
        public bool Test_51014()
        {
            return FirewallTemplates.Template_AllIPv6Address_AllDiscoveryService(_activityData);
        }

        [TestDetailsAttribute(Id = 51015, Category = "All IPv6 Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "All IPv6 Addresses - All Microsoft Web Services")]
        public bool Test_51015()
        {
            return FirewallTemplates.Template_AllIPv6Address_AllMicrosoftWebService(_activityData);
        }

        #endregion  All IPv6 Address

        #region All IPv6 Link Local Address

        [TestDetailsAttribute(Id = 51016, Category = "All IPv6 Link Local Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IPv6 Link Local Addresses - All Services")]
        public bool Test_51016()
        {
            return FirewallTemplates.Template_AllIPv6LinkLocalAddress_AllService(_activityData);
        }

        [TestDetailsAttribute(Id = 51017, Category = "All IPv6 Link Local Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IPv6 Link Local Addresses - All Print Services")]
        public bool Test_51017()
        {
            return FirewallTemplates.Template_AllIPv6LinkLocalAddress_AllPrintService(_activityData);
        }

        [TestDetailsAttribute(Id = 51018, Category = "All IPv6 Link Local Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IPv6 Link Local Addresses - All Management Services")]
        public bool Test_51018()
        {
            return FirewallTemplates.Template_AllIPv6LinkLocalAddress_AllManagementService(_activityData);
        }

        [TestDetailsAttribute(Id = 51019, Category = "All IPv6 Link Local Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IPv6 Link Local Addresses - All Discovery Services")]
        public bool Test_51019()
        {
            return FirewallTemplates.Template_AllIPv6LinkLocalAddress_AllDiscoveryService(_activityData);
        }

        [TestDetailsAttribute(Id = 51020, Category = "All IPv6 Link Local Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "All IPv6 Link Local Addresses - All Microsoft Web Services")]
        public bool Test_51020()
        {
            return FirewallTemplates.Template_AllIPv6LinkLocalAddress_AllMicrosoftWebService(_activityData);
        }

        #endregion  All IPv6 Link Local Address

        #region All IPv6 Non Link Local Address

        [TestDetailsAttribute(Id = 51021, Category = "All IPv6 Non Link Local Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IPv6 Non Link Local Addresses - All Services")]
        public bool Test_51021()
        {
            return FirewallTemplates.Template_AllIPv6NonLinkLocalAddress_AllService(_activityData);
        }

        [TestDetailsAttribute(Id = 51022, Category = "All IPv6 Non Link Local Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IPv6 Non Link Local Addresses - All Print Services")]
        public bool Test_51022()
        {
            return FirewallTemplates.Template_AllIPv6NonLinkLocalAddress_AllPrintService(_activityData);
        }

        [TestDetailsAttribute(Id = 51023, Category = "All IPv6 Non Link Local Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IPv6 Non Link Local Addresses - All Management Services")]
        public bool Test_51023()
        {
            return FirewallTemplates.Template_AllIPv6NonLinkLocalAddress_AllManagementService(_activityData);
        }

        [TestDetailsAttribute(Id = 51024, Category = "All IPv6 Non Link Local Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "All IPv6 Non Link Local Addresses - All Discovery Services")]
        public bool Test_51024()
        {
            return FirewallTemplates.Template_AllIPv6NonLinkLocalAddress_AllDiscoveryService(_activityData);
        }

        [TestDetailsAttribute(Id = 51025, Category = "All IPv6 Non Link Local Addresses", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "All IPv6 Non Link Local Addresses - All Microsoft Web Services")]
        public bool Test_51025()
        {
            return FirewallTemplates.Template_AllIPv6NonLinkLocalAddress_AllMicrosoftWebService(_activityData);
        }

        #endregion  All IPv6 Non Link Local Address

        #region Custom Service Templates

        [TestDetailsAttribute(Id = 51028, Category = "General", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Power Cycle and Cold Reset/Restore Factory Settings")]
        public bool Test_51028()
        {
            return FirewallTemplates.ValidateRuleWithPowerCycleAndColdReset(_activityData, DefaultAddressTemplates.AllIPv4Addresses);
        }

        [TestDetailsAttribute(Id = 51029, Category = "Custom Service", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "Custom Service - P9100")]
        public bool Test_51029()
        {
            return FirewallTemplates.ValidateCustomService(_activityData, "P9100", 51029);
        }

        [TestDetailsAttribute(Id = 51030, Category = "Custom Service", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "Custom Service - SNMP")]
        public bool Test_51030()
        {
            return FirewallTemplates.ValidateCustomService(_activityData, "SNMP", 51030);
        }

        [TestDetailsAttribute(Id = 51031, Category = "Custom Service", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "Custom Service - WS Discovery")]
        public bool Test_51031()
        {
            return FirewallTemplates.ValidateCustomService(_activityData, "WS-Discovery", 51031);
        }

        [TestDetailsAttribute(Id = 51032, Category = "Custom Service", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "Custom Service - HTTP")]
        public bool Test_51032()
        {
            return FirewallTemplates.ValidateCustomService(_activityData, "HTTP", 51032);
        }

        [TestDetailsAttribute(Id = 51033, Category = "Multiple Rules", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "Multiple Rules - Print & Management Services")]
        public bool Test_51033()
        {
            return FirewallTemplates.ValidateCustomServiceWithMultipleRules(_activityData, 51033);
        }

        #endregion

        #region Maximum Rule Creation
        [TestDetailsAttribute(Id = 51034, Category = "Multiple Rules", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Maximum Rules Creation")]
        public bool Test_51034()
        {
            return FirewallTemplates.Template_AllIPAddress_AllService_MaximumRule(_activityData);
        }
        #endregion

        #region Print Cross Plug-in Test Cases

        [TestDetailsAttribute(Id = 51026, Category = "Print", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "Print Protocols - IPv4")]
        public bool Test_51026()
        {
            return FirewallTemplates.Template_AllIPv4Address_AllServices(_activityData, DefaultAddressTemplates.AllIPv4Addresses);
        }

        [TestDetailsAttribute(Id = 51027, Category = "Print", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet, Connectivity = ConnectivityType.Wired, Description = "Print Protocols - IPv6")]
        public bool Test_51027()
        {
            return FirewallTemplates.Template_AllIPv4Address_AllServices(_activityData, DefaultAddressTemplates.AllIPv6Addresses, true);
        }

        #endregion        
    }
}
