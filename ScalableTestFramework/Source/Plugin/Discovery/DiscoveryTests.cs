using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.Discovery
{
    /// <summary>
    /// Network Discovery Tests
    /// </summary>
    internal class DiscoveryTests : CtcBaseTests
    {
        #region Local Variables

        /// <summary>
        /// Instance of NetworkDiscoveryActivityData
        /// </summary>
        DiscoveryActivityData _activityData;

        #endregion

        #region Constructor

        /// <summary>
        /// NetworkDiscoveryTests Constructor
        /// </summary>
        /// <param name="activityData">ActivityData</param>
        public DiscoveryTests(DiscoveryActivityData activityData)
            : base(activityData.ProductName)
        {
            _activityData = activityData;
            ProductFamily = activityData.ProductFamily;
            Sliver = "NW Discovery";
        }

        #endregion

        #region Automated Tests from ALM

        /// <summary>
        /// Test case ID=1
        /// </summary>                   
        [TestDetailsAttribute(Id = 1, Category = "Web Services Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Basic Discovery - IPv4")]
        public bool Test_1()
        {
            return DiscoveryTemplates.WSDiscovery(_activityData);
        }

        /// <summary>
        /// Test case ID=2
        /// </summary>                   
        [TestDetailsAttribute(Id = 2, Category = "Web Services Discovery", Protocol = ProtocolType.IPv6, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Basic Discovery - IPv6")]
        public bool Test_2()
        {
            return DiscoveryTemplates.WSDiscoveryIPv6(_activityData);
        }

        /// <summary>
        /// Test case ID=3
        /// </summary>                   
        [TestDetailsAttribute(Id = 3, Category = "Web Services Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Enable & Disable")]
        public bool Test_3()
        {
            return DiscoveryTemplates.WSDicovery_Enable_Disable(_activityData);
        }

        /// <summary>
        /// Test case ID=4
        /// </summary>                   
        [TestDetailsAttribute(Id = 4, Category = "Web Services Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Power Cycle")]
        public bool Test_4()
        {
            return DiscoveryTemplates.WSDiscovery_PowerCycle(_activityData);
        }

        /// <summary>
        /// Test case ID=5
        /// </summary>                   
        [TestDetailsAttribute(Id = 5, Category = "Web Services Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.InkJet | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Cold Reset / Restore Factory Settings")]
        public bool Test_5()
        {
            return DiscoveryTemplates.WSDiscovery_ColdReset(_activityData);
        }

        /// <summary>
        /// Test case ID=6
        /// </summary>                   
        [TestDetailsAttribute(Id = 6, Category = "Web Services Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Enable & Disable Multicast IPv4")]
        public bool Test_6()
        {
            return DiscoveryTemplates.WSDicovery_MulticastIPv4(_activityData);
        }

        /// <summary>
        /// Test case ID=7
        /// </summary>                   
        [TestDetailsAttribute(Id = 7, Category = "Web Services Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.InkJet | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Different Host Names")]
        public bool Test_7()
        {
            return DiscoveryTemplates.WSDiscovery_Different_HostNames(_activityData);
        }

        /// <summary>
        /// Test case ID=8
        /// </summary>                   
        [TestDetailsAttribute(Id = 8, Category = "Web Services Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Configuration Changes")]
        public bool Test_8()
        {
            return DiscoveryTemplates.WSDiscovery_Configuration_Changes(_activityData);
        }

        /// <summary>
        /// Test case ID=9
        /// </summary>                   
        [TestDetailsAttribute(Id = 9, Category = "SNMP Discovery", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "SNMP Discovery")]
        public bool Test_9()
        {
            return DiscoveryTemplates.SNMP_Discovery(_activityData);
        }

        /// <summary>
        /// Test case ID=10
        /// </summary>                   
        [TestDetailsAttribute(Id = 10, Category = "SLP Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Basic Discovery")]
        public bool Test_10()
        {
            return DiscoveryTemplates.SLPDiscovery(_activityData);
        }

        /// <summary>
        /// Test case ID=11
        /// </summary>                   
        [TestDetailsAttribute(Id = 11, Category = "SLP Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Enable & Disable")]
        public bool Test_11()
        {
            return DiscoveryTemplates.SLPDiscovery_Enable_Disable(_activityData);
        }

        /// <summary>
        /// Test case ID=12
        /// </summary>                   
        [TestDetailsAttribute(Id = 12, Category = "SLP Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Power Cycle")]
        public bool Test_12()
        {
            return DiscoveryTemplates.SLPDiscovery_PowerCycle(_activityData);
        }

        /// <summary>
        /// Test case ID=13
        /// </summary>                   
        [TestDetailsAttribute(Id = 13, Category = "SLP Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.InkJet | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Cold Reset / Restore Factory Settings")]
        public bool Test_13()
        {
            return DiscoveryTemplates.SLPDiscovery_ColdReset(_activityData);
        }

        /// <summary>
        /// Test case ID=14
        /// </summary>                   
        [TestDetailsAttribute(Id = 14, Category = "SLP Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Enable & Disable Multicast IPv4")]
        public bool Test_14()
        {
            return DiscoveryTemplates.SLPDiscovery_MulticastIPv4(_activityData);
        }

        /// <summary>
        /// Test case ID=15
        /// </summary>                   
        [TestDetailsAttribute(Id = 15, Category = "SLP Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Different Host Names")]
        public bool Test_15()
        {
            return DiscoveryTemplates.SLPDiscovery_Different_HostNames(_activityData);
        }

        /// <summary>
        /// Test case ID=16
        /// </summary>                   
        [TestDetailsAttribute(Id = 16, Category = "SLP Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Configuration Changes")]
        public bool Test_16()
        {
            return DiscoveryTemplates.SLPDiscovery_Configuration_Changes(_activityData);
        }

        /// <summary>
        /// Test case ID=17
        /// </summary>                   
        [TestDetailsAttribute(Id = 17, Category = "SLP Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Enable & Disable SLP Client Mode")]
        public bool Test_17()
        {
            return DiscoveryTemplates.SLPClientMode_Enable_Disable(_activityData);
        }

        /// <summary>
        /// Test case ID=18
        /// </summary>                   
        [TestDetailsAttribute(Id = 18, Category = "Bonjour Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Basic Discovery")]
        public bool Test_18()
        {
            return DiscoveryTemplates.BonjourDiscovery(_activityData);
        }

        /// <summary>
        /// Test case ID=19
        /// </summary>                   
        [TestDetailsAttribute(Id = 19, Category = "Bonjour Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Enable & Disable")]
        public bool Test_19()
        {
            return DiscoveryTemplates.BonjourDiscovery_Enable_Disable(_activityData);
        }

        /// <summary>
        /// Test case ID=20
        /// </summary>                   
        [TestDetailsAttribute(Id = 20, Category = "Bonjour Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Power Cycle")]
        public bool Test_20()
        {
            return DiscoveryTemplates.BonjourDiscovery_PowerCycle(_activityData);
        }

        /// <summary>
        /// Test case ID=21
        /// </summary>                   
        [TestDetailsAttribute(Id = 21, Category = "Bonjour Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Cold Reset / Restore Factory Settings")]
        public bool Test_21()
        {
            return DiscoveryTemplates.BonjourDiscovery_ColdReset(_activityData);
        }

        /// <summary>
        /// Test case ID=22
        /// </summary>                   
        [TestDetailsAttribute(Id = 22, Category = "Bonjour Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Enable & Disable Multicast IPv4")]
        public bool Test_22()
        {
            return DiscoveryTemplates.BonjourDiscovery_MulticastIPv4(_activityData);
        }

        /// <summary>
        /// Test case ID=23
        /// </summary>                   
        [TestDetailsAttribute(Id = 23, Category = "Bonjour Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Configuration Changes")]
        public bool Test_23()
        {
            return DiscoveryTemplates.BonjourDiscovery_Configuration_Changes(_activityData);
        }

        /// <summary>
        /// Test case ID=24
        /// </summary>                   
        [TestDetailsAttribute(Id = 24, Category = "Bonjour Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Bonjour Service Name - Power Cycle")]
        public bool Test_24()
        {
            return DiscoveryTemplates.BonjourServiceName_PowerCycle(_activityData);
        }

        /// <summary>
        /// Test case ID=25
        /// </summary>                   
        [TestDetailsAttribute(Id = 25, Category = "Bonjour Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Bonjour Service Name - Cold Reset / Restore Factory Settings")]
        public bool Test_25()
        {
            return DiscoveryTemplates.BonjourServiceName_ColdReset(_activityData);
        }

        /// <summary>
        /// Test case ID=26
        /// </summary>                   
        [TestDetailsAttribute(Id = 26, Category = "Bonjour Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Bonjour Highest Service - Power Cycle")]
        public bool Test_26()
        {
            return DiscoveryTemplates.BonjourHighestService_PowerCycle(_activityData);
        }

        /// <summary>
        /// Test case ID=27
        /// </summary>                   
        [TestDetailsAttribute(Id = 27, Category = "Bonjour Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Bonjour Highest Service - Cold Reset / Restore Factory Settings")]
        public bool Test_27()
        {
            return DiscoveryTemplates.BonjourHighestService_ColdReset(_activityData);
        }

        /// <summary>
        /// Test case ID=28
        /// </summary>                   
        [TestDetailsAttribute(Id = 28, Category = "Bonjour Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Different Bonjour Service Names")]
        public bool Test_28()
        {
            return DiscoveryTemplates.BonjourServiceNames(_activityData);
        }

        /// <summary>
        /// Test case ID=29
        /// </summary>                   
        [TestDetailsAttribute(Id = 29, Category = "Bonjour Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Duplicate Bonjour Service Name")]
        public bool Test_29()
        {
            return DiscoveryTemplates.DuplicateBonjourServiceName(_activityData);
        }

        /// <summary>
        /// Test case ID=30
        /// </summary>                   
        [TestDetailsAttribute(Id = 30, Category = "Bonjour Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Different Bonjour Highest Priority")]
        public bool Test_30()
        {
            return DiscoveryTemplates.BonjourHighestPriorities(_activityData);
        }

        /// <summary>
        /// Test case ID=31
        /// </summary>                   
        [TestDetailsAttribute(Id = 31, Category = "Bonjour Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Preinstalled Print Driver")]
        public bool Test_31()
        {
            return DiscoveryTemplates.BonjourPreinstalledPrintDriver(_activityData);
        }

        /// <summary>
        /// Test case ID=32
        /// </summary>                   
        [TestDetailsAttribute(Id = 32, Category = "Bonjour Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Bonjour Highest Priority - Printing Using Different Protocols")]
        public bool Test_32()
        {
            return DiscoveryTemplates.BonjourHighestPriorityPrinting(_activityData);
        }

        /// <summary>
        /// Test case ID=33
        /// </summary>                   
        [TestDetailsAttribute(Id = 33, Category = "Bonjour Discovery", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.InkJet | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Bonjour Highest Priority - Disable Print Protocol")]
        public bool Test_33()
        {
            return DiscoveryTemplates.BonjourHighestPriorityDisablePrintProtocol(_activityData);
        }

        /// <summary>
        /// Test case ID=34
        /// </summary>                   
        [TestDetailsAttribute(Id = 34, Category = "Multicast IPv4", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Enable & Disable")]
        public bool Test_34()
        {
            return DiscoveryTemplates.MultiCastIPv4_Enable_Disable(_activityData);
        }

        /// <summary>
        /// Test case ID=35
        /// </summary>                   
        [TestDetailsAttribute(Id = 35, Category = "Multicast IPv4", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Power Cycle")]
        public bool Test_35()
        {
            return DiscoveryTemplates.MulticastIPv4_PowerCycle(_activityData);
        }

        /// <summary>
        /// Test case ID=36
        /// </summary>                   
        [TestDetailsAttribute(Id = 36, Category = "Multicast IPv4", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Cold Reset / Restore Factory Settings")]
        public bool Test_36()
        {
            return DiscoveryTemplates.MulticastIPv4_ColdReset(_activityData);
        }

        /// <summary>
        /// Test case ID=37
        /// </summary>                   
        [TestDetailsAttribute(Id = 37, Category = "General", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Firmware Upgrade")]
        public bool Test_37()
        {
            return DiscoveryTemplates.Discovery_FW_Upgrade(_activityData);
        }

        #endregion
    }
}
