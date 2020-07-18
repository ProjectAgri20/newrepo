using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.IPConfiguration
{
    /// <summary>
    /// IP Configuration Tests
    /// </summary>
    internal class IPConfigurationTests : CtcBaseTests
    {
        // Inkjet Modification Execution Details
        /*	Date			ProductFamily		Name		SessionId		Result
			25 May 2016		VEP					Maple		959CFEE3		4/5 Passed ( 96133- Existing Issue)
			08 June 2016	TPS					Dorado		5395901C		9/9 Passed
			20 May 2016		InkJet				IcemanHi	77733964		9/9 Passed
			28 June 2016	InkJet				IcemanHi	A72299B6		6/7 Passed
			28 June 2016	InkJet				IcemanHi	AD95C7DE		1/2 Passed
			28 June 2016	InkJet				IcemanHi	AD0A9FD4		1/1 Passed
			20 June 2016	TPS					Dorado		77CFBF80		4/8 Passed			
			21 June 2016	TPS					Dorado		7D4EC8A2		3/3 Passed
			17 June 2016	VEP					Jaz			65FC96A5		5/6 Passed
			20 June 2016	VEP					Jaz			77C49822		1/1 Passed	
		*/

        #region Local Variables

        /// <summary>
        /// Instance of NetworkDiscoveryActivityData
        /// </summary>
        IPConfigurationActivityData _activityData;

        #endregion

        #region Constructor

        /// <summary>
        /// IP ConfigurationTests Constructor
        /// </summary>
        /// <param name="activityData"></param>
        public IPConfigurationTests(IPConfigurationActivityData activityData)
            : base(activityData.ProductName)
        {
            _activityData = activityData;
            ProductFamily = activityData.ProductFamily;
            Sliver = "IPConfiguration";
            NetworkConnectivity = activityData.PrinterConnectivity;
        }

        #endregion

        #region Basic IP Configuration Tests

        #region Enable/Disable

        /// <summary>
        /// Test case ID=96162
        /// </summary>                   
        [TestDetailsAttribute(Id = 96162, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Enable & Disable - IPv4 & IPv6")]
        public bool Test_96162()
        {
            return IPConfigurationTemplates.VerifyIPv4IPv6StatusChange(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96186
        /// </summary>                   
        [TestDetailsAttribute(Id = 96186, Category = "Basic", Protocol = ProtocolType.IPv6, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Enable & Disable - Manual IPv6 Address")]
        public bool Test_96186()
        {
            return IPConfigurationTemplates.VerifyManualIPv6Status(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=654999
        /// </summary>        
        [TestDetailsAttribute(Id = 654999, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Enable & Disable - Stateless IPv6")]
        public bool Test_654999()
        {
            return IPConfigurationTemplates.VerifyIPv6StatelessOption(_activityData, CtcUtility.GetTestId());
        }

        #endregion

        #region IP Acquisition

        /// <summary>
        /// Test case ID=96136
        /// </summary>                   
        [TestDetailsAttribute(Id = 96136, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Acquisition - BOOTP Server")]
        public bool Test_96136()
        {
            return IPConfigurationTemplates.VerifyBootpIPAcquisition(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96130
        /// </summary>                   
        [TestDetailsAttribute(Id = 96130, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Acquisition - DHCPv4 Server")]
        public bool Test_96130()
        {
            return IPConfigurationTemplates.VerifyDhcpIPAcquisition(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96139
        /// </summary>                   
        [TestDetailsAttribute(Id = 96139, Category = "Basic", Protocol = ProtocolType.IPv6, ProductCategory = ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Acquisition - DHCPv6 Server")]
        public bool Test_96139()
        {
            return IPConfigurationTemplates.VerifyDhcpv6IPAcquisition(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=654412
        /// </summary>        
        [TestDetailsAttribute(Id = 654412, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "IP Acquisition - Acquire AutoIP - BOOTP Server Unavailable - DHCP Request Option Disabled")]
        public bool Test_654412()
        {
            return IPConfigurationTemplates.AutoIPWithOnlyBootPAndDHCPRequestDisable(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=654419
        /// </summary>        
        [TestDetailsAttribute(Id = 654419, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Acquisition - Acquire AutoIP & DHCPv6 IP Removed - No Server with Finite Lease Time")]
        public bool Test_654419()
        {
            return IPConfigurationTemplates.VerifyAutoIPWithFiniteLease(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96141
        /// </summary>                   
        [TestDetailsAttribute(Id = 96141, Category = "Basic", Protocol = ProtocolType.IPv6, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Acquisition - Stateless IPv6 - Only Four Addresses")]
        public bool Test_96141()
        {
            return IPConfigurationTemplates.VerifyStatelessLinkLocalAddress(_activityData, CtcUtility.GetTestId());
        }

        #endregion

        #region IP Config Method Change

        /// <summary>
        /// Test case Id=96182
        /// </summary>        
        [TestDetailsAttribute(Id = 96182, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Config Method Change - DHCP to AutoIP")]
        public bool Test_96182()
        {
            return IPConfigurationTemplates.DHCPToAutoIP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96180
        /// </summary>                   
        [TestDetailsAttribute(Id = 96180, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Config Method Change - DHCP to BOOTP")]
        public bool Test_96180()
        {
            return IPConfigurationTemplates.DHCPToBootp(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96174
        /// </summary>                   
        [TestDetailsAttribute(Id = 96174, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Config Method Change - DHCP to Manual")]
        public bool Test_96174()
        {
            return IPConfigurationTemplates.DHCPToManual(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=96179
        /// </summary>        
        [TestDetailsAttribute(Id = 96179, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Config Method Change - Manual to AutoIP")]
        public bool Test_96179()
        {
            return IPConfigurationTemplates.ManualToAutoIP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96176
        /// </summary>                   
        [TestDetailsAttribute(Id = 96176, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Config Method Change - Manual to BOOTP")]
        public bool Test_96176()
        {
            return IPConfigurationTemplates.ManualToBootp(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96175
        /// </summary>                   
        [TestDetailsAttribute(Id = 96175, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Config Method Change - Manual to DHCP")]
        public bool Test_96175()
        {
            return IPConfigurationTemplates.ManualToDHCP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=656076
        /// </summary>
        [TestDetailsAttribute(Id = 656076, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Config Method Change - Manual to Manual")]
        public bool Test_656076()
        {
            return IPConfigurationTemplates.ManualToManual(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=692967
        /// </summary>        
        [TestDetailsAttribute(Id = 692967, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Config Method Change - BOOTP to AutoIP")]
        public bool Test_692967()
        {
            return IPConfigurationTemplates.BootPToAutoIP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96181
        /// </summary>                   
        [TestDetailsAttribute(Id = 96181, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Config Method Change - BOOTP to DHCP")]
        public bool Test_96181()
        {
            return IPConfigurationTemplates.BootpToDHCP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96177
        /// </summary>                   
        [TestDetailsAttribute(Id = 96177, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Config Method Change - BOOTP to Manual")]
        public bool Test_96177()
        {
            return IPConfigurationTemplates.BootpToManual(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=96185
        /// </summary>        
        [TestDetailsAttribute(Id = 96185, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Config Method Change - AutoIP to BOOTP")]
        public bool Test_96185()
        {
            return IPConfigurationTemplates.AutoIPToBootP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=96183
        /// </summary>        
        [TestDetailsAttribute(Id = 96183, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Config Method Change - AutoIP to DHCP")]
        public bool Test_96183()
        {
            return IPConfigurationTemplates.AutoIPToDHCP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=96178
        /// </summary>        
        [TestDetailsAttribute(Id = 96178, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Config Method Change - AutoIP to Manual")]
        public bool Test_96178()
        {
            return IPConfigurationTemplates.AutoIPToManual(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=656192
        /// </summary>        
        [TestDetailsAttribute(Id = 656192, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Config Method Change - BOOTP to DHCP - DHCP Server not Available")]
        public bool Test_656192()
        {
            return IPConfigurationTemplates.AutoIPWithOnlyBootP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=656204
        /// </summary>        
        [TestDetailsAttribute(Id = 656204, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Config Method Change - DHCP to BOOTP - BOOTP Server not Available")]
        public bool Test_656204()
        {
            return IPConfigurationTemplates.AutoIPWithDHCP(_activityData, CtcUtility.GetTestId());
        }

        #endregion

        #region IP Configuration

        /// <summary>
        /// Test case Id=96133
        /// </summary>        
        [TestDetailsAttribute(Id = 96133, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "AutoIP Configuration - Link Local Network")]
        public bool Test_96133()
        {
            return IPConfigurationTemplates.VerifyAutoIPOnMultipleColdReset(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>		
        /// Test case Id=96137
        /// </summary>        
        [TestDetailsAttribute(Id = 96137, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "BOOTP IP Configuration - TFTP Server")]
        public bool Test_96137()
        {
            return IPConfigurationTemplates.VerifyBootpTftpConfiguration(_activityData);
        }

        /// <summary>
        /// Test case Id=96131
        /// </summary>        
        [TestDetailsAttribute(Id = 96131, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Description = "Default IP Configuration - Non Link Local Network")]
        public bool Test_96131()
        {
            return IPConfigurationTemplates.VerifyDefaultIPWithARP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=96195
        /// </summary>        
        [TestDetailsAttribute(Id = 96195, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "DHCP Configuration - DHCP set to TRUE - DHCP Server Added to Link Local Network After 30 min")]
        public bool Test_96195()
        {
            return IPConfigurationTemplates.VerifySendDhcpRequestWithAutoIP(_activityData, CtcUtility.GetTestId(), true);
        }

        /// <summary>
        /// Test case Id=96196
        /// </summary>        
        [TestDetailsAttribute(Id = 96196, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "DHCP Configuration  - DHCP set to FALSE - DHCP Server Added to Link Local Network")]
        public bool Test_96196()
        {
            return IPConfigurationTemplates.VerifySendDhcpRequestWithAutoIP(_activityData, CtcUtility.GetTestId(), false);
        }

        /// <summary>
        /// Test case Id=96132
        /// </summary>        
        [TestDetailsAttribute(Id = 96132, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS, Description = "IP Address Configuration - ARP Ping")]
        public bool Test_96132()
        {
            return IPConfigurationTemplates.VerifyARPIPAddress(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=96198
        /// </summary>        
        [TestDetailsAttribute(Id = 96198, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Address Configuration - Both DHCP & BOOTP Available in the Network")]
        public bool Test_96198()
        {
            return IPConfigurationTemplates.AutoIPWithDhcpBootPServers(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=654413
        /// </summary>        
        [TestDetailsAttribute(Id = 654413, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Address Configuration - BOOTP Server not Available - Brought up Later")]
        public bool Test_654413()
        {
            return IPConfigurationTemplates.AutoIPWithBootP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=654417
        /// </summary>        
        [TestDetailsAttribute(Id = 654417, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "IP Address Configuration - DHCP Server not Available - Brought up Later")]
        public bool Test_654417()
        {
            return IPConfigurationTemplates.AutoIPWithLessLeasetime(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96134
        /// </summary>                   
        [TestDetailsAttribute(Id = 96134, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Address Configuration - Manual IPv4")]
        public bool Test_96134()
        {
            return IPConfigurationTemplates.VerifyManualIPConfiguration(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96142
        /// </summary>                   
        [TestDetailsAttribute(Id = 96142, Category = "Basic", Protocol = ProtocolType.IPv6, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "IP Address Configuration - Manual IPv6")]
        public bool Test_96142()
        {
            return IPConfigurationTemplates.VerifyManualIPv6(_activityData, CtcUtility.GetTestId());
        }

        #endregion

        #region Power Cycle / Cold Reset

        /// <summary>
        /// Test case Id=96146
        /// </summary>        
        [TestDetailsAttribute(Id = 96146, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "Power Cycle - No IP in Server")]
        public bool Test_96146()
        {
            return IPConfigurationTemplates.VerifyNonAvailabilityOfIPAddress(_activityData);
        }

        /// <summary>
        /// Test case Id=96151
        /// </summary>        
        [TestDetailsAttribute(Id = 96151, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP | ProductFamilies.TPS, Description = "Power Cycle - RARP Configuration")]
        public bool Test_96151()
        {
            return IPConfigurationTemplates.VerifyRarpConfiguration(_activityData);
        }

        /// <summary>
        /// Test case Id=96154
        /// </summary>        
        [TestDetailsAttribute(Id = 96154, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Power Cycle - Default 192.0.0.192 Configuration")]
        public bool Test_96154()
        {
            return IPConfigurationTemplates.VerifyLegacyIPAfterPowerCycle(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=96166
        /// </summary>        
        [TestDetailsAttribute(Id = 96166, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Power Cycle - Partial Configuration - No Server")]
        public bool Test_96166()
        {
            return IPConfigurationTemplates.VerifyPartialConfigurationWithInfiniteLease(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96145
        /// </summary>                   
        [TestDetailsAttribute(Id = 96145, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP | ProductFamilies.TPS | ProductFamilies.InkJet, Description = "Cold Reset - Server Availability")]
        public bool Test_96145()
        {
            return IPConfigurationTemplates.VerifyDeviceBehaviorOnColdReset(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=96158
        /// </summary>        
        [TestDetailsAttribute(Id = 96158, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Cold Reset - Only BOOTP - TFTP Configuration File")]
        public bool Test_96158()
        {
            return IPConfigurationTemplates.VerifyBootpTftpConfigurationWithReset(_activityData);
        }

        /// <summary>
        /// Test case Id=96152
        /// </summary>        
        [TestDetailsAttribute(Id = 96152, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Power Cycle & Cold Reset - AutoIP")]
        public bool Test_96152()
        {
            return IPConfigurationTemplates.VerifyAutoIPAfterPowerCycleAndColdReset(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=96163
        /// </summary>
        [TestDetailsAttribute(Id = 96163, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP | ProductFamilies.TPS | ProductFamilies.InkJet, Description = "Power Cycle & Cold Reset - IPv4 & IPv6")]
        public bool Test_96163()
        {
            return IPConfigurationTemplates.VerifyDhcpv6Ipv6v4StatusOnReset(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96150
        /// </summary>                   
        [TestDetailsAttribute(Id = 96150, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP | ProductFamilies.TPS | ProductFamilies.InkJet, Description = "Power Cycle & Cold Reset - BOOTP Configuration")]
        public bool Test_96150()
        {
            return IPConfigurationTemplates.VerifyBootpAfterPowerCycleAndColdReset(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96149
        /// </summary>                   
        [TestDetailsAttribute(Id = 96149, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP | ProductFamilies.TPS | ProductFamilies.InkJet, Description = "Power Cycle & Cold Reset - Manual IPv4 Configuration")]
        public bool Test_96149()
        {
            return IPConfigurationTemplates.VerifyManualIPAfterPowerCycleAndColdReset(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=96159
        /// </summary>
        [TestDetailsAttribute(Id = 96159, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Description = "Power Cycle & Cold Reset - Manual IPv6 Configuration")]
        public bool Test_96159()
        {
            return IPConfigurationTemplates.VerifyManualIpv6OnReset(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96160
        /// </summary>                   
        [TestDetailsAttribute(Id = 96160, Category = "Basic", Protocol = ProtocolType.IPv6, ProductCategory = ProductFamilies.LFP, Description = "Power Cycle & Cold Reset - Stateless IPv6")]
        public bool Test_96160()
        {
            return IPConfigurationTemplates.VerifyStatelessIPv6OnRebootAndColdReset(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96143
        /// </summary>                   
        [TestDetailsAttribute(Id = 96143, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Power Cycle & Cold Reset - DHCP v4-v6 Reservation")]
        public bool Test_96143()
        {
            return IPConfigurationTemplates.VerifyIPAfterPowerCycleColdreset(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=96144
        /// </summary>
        [TestDetailsAttribute(Id = 96144, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Power Cycle & Cold Reset - DHCP v4-v6 with Dynamic Scope")]
        public bool Test_96144()
        {
            return IPConfigurationTemplates.VerifyDynamicScopeOnReset(_activityData, CtcUtility.GetTestId());
        }

        #endregion

        #region Network Hose Break

        /// <summary>
        /// Test case Id=696094
        /// </summary>
        [TestDetailsAttribute(Id = 696094, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "Network Hose Break - Same Network - AutoIP")]
        public bool Test_696094()
        {
            return IPConfigurationTemplates.HoseBreakSameNetworkUsingAutoIP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=494362
        /// </summary>
        [TestDetailsAttribute(Id = 494362, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "Network Hose Break - Same Network - BOOTP IP")]
        public bool Test_494362()
        {
            return IPConfigurationTemplates.VerifyHoseBreakSameNetwork(_activityData, IPConfigMethod.BOOTP, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=494363
        /// </summary>
        [TestDetailsAttribute(Id = 494363, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "Network Hose Break - Same Network - DHCP IP")]
        public bool Test_494363()
        {
            return IPConfigurationTemplates.VerifyHoseBreakSameNetwork(_activityData, IPConfigMethod.DHCP, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=655989
        /// </summary>
        [TestDetailsAttribute(Id = 655989, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "Network Hose Break - Same Network - Manual IP")]
        public bool Test_655989()
        {
            return IPConfigurationTemplates.HoseBreakSameNetworkUsingManualIP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=474101
        /// </summary>
        [TestDetailsAttribute(Id = 474101, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "Network Hose Break - Different Networks - AutoIP")]
        public bool Test_474101()
        {
            return IPConfigurationTemplates.HoseBreakAcrossNetworksUsingAutoIP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=474098
        /// </summary>
        [TestDetailsAttribute(Id = 474098, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "Network Hose Break - Different Networks - BOOTP IP")]
        public bool Test_474098()
        {
            return IPConfigurationTemplates.VerifyHosBreakAcrossNetworks(_activityData, IPConfigMethod.BOOTP, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=463305
        /// </summary>
        [TestDetailsAttribute(Id = 463305, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "Network Hose Break - Different Networks - DHCP IP")]
        public bool Test_463305()
        {
            return IPConfigurationTemplates.VerifyHosBreakAcrossNetworks(_activityData, IPConfigMethod.DHCP, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=655990
        /// </summary>
        [TestDetailsAttribute(Id = 655990, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "Network Hose Break - Different Networks - Manual IP")]
        public bool Test_655990()
        {
            return IPConfigurationTemplates.HoseBreakAcrossNetworksUsingDifferentManualIP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=657162
        /// </summary>
        [TestDetailsAttribute(Id = 657162, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "Network Hose Break - Different Networks - Revert")]
        public bool Test_657162()
        {
            return IPConfigurationTemplates.HoseBreakAcrossNetworksUsingSameManualIP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=656235
        /// </summary>
        [TestDetailsAttribute(Id = 656235, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Network Hose Break - Different Servers")]
        public bool Test_656235()
        {
            return IPConfigurationTemplates.HoseBreakDifferentServers(_activityData, CtcUtility.GetTestId());
        }


        #endregion

        #region Config Precedence

        /// <summary>
        /// Test case ID=96147
        /// </summary>                   
        [TestDetailsAttribute(Id = 96147, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Reinitialize Now")]
        public bool Test_96147()
        {
            return IPConfigurationTemplates.VerifyReinializeNowBehavior(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96148
        /// </summary>                   
        [TestDetailsAttribute(Id = 96148, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Clear Previous Values and Reinitialize Now")]
        public bool Test_96148()
        {
            return IPConfigurationTemplates.VerifyClearPreviousValuesAndReinitializeNow(_activityData, CtcUtility.GetTestId());
        }

        #endregion

        #region Lease Time

        /// <summary>
        /// Test case ID=96164
        /// </summary>                   
        [TestDetailsAttribute(Id = 96164, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Lease Time Acquisition")]
        public bool Test_96164()
        {
            return IPConfigurationTemplates.VerifyDeviceGetsProperLeaseTime(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96165
        /// </summary>                   
        [TestDetailsAttribute(Id = 96165, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Infinite Lease - No Renewal")]
        public bool Test_96165()
        {
            return IPConfigurationTemplates.VerifyLeaseRenewalBehavior(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96167
        /// </summary>                   
        [TestDetailsAttribute(Id = 96167, Category = "Basic", Protocol = ProtocolType.IPv6, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Preferred Lease Time - DHCPv6 & IPv6 Stateless Address")]
        public bool Test_96167()
        {
            return IPConfigurationTemplates.VerifyPreferredValidLifeTime(_activityData, true, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96168
        /// </summary>                   
        [TestDetailsAttribute(Id = 96168, Category = "Basic", Protocol = ProtocolType.IPv6, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Valid Lease Time - DHCPv6 & IPv6 Stateless Address")]
        public bool Test_96168()
        {
            return IPConfigurationTemplates.VerifyPreferredValidLifeTime(_activityData, false, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=96169
        /// </summary>        
        [TestDetailsAttribute(Id = 96169, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Lease Renew - DHCPv4 Server Down")]
        public bool Test_96169()
        {
            return IPConfigurationTemplates.VerifyLeaseRenewalOnServerDown(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=655548
        /// </summary>        
        [TestDetailsAttribute(Id = 655548, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Lease Renew - DHCPv6 Server Down")]
        public bool Test_655548()
        {
            return IPConfigurationTemplates.VerifyIPv6LeaseRenewal(_activityData, CtcUtility.GetTestId());
        }

        #endregion

        #region ARP Ping

        /// <summary>
        /// Test case Id=654414
        /// </summary>        
        [TestDetailsAttribute(Id = 654414, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "ARP Ping - AutoIP")]
        public bool Test_654414()
        {
            return IPConfigurationTemplates.VerifyAutoIPWithARP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=654416
        /// </summary>        
        [TestDetailsAttribute(Id = 654416, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "ARP Ping - DHCP IP")]
        public bool Test_654416()
        {
            return IPConfigurationTemplates.VerifyDHCPWithARP(_activityData, CtcUtility.GetTestId());
        }

        #endregion

        #region Wireless

        /// <summary>
        /// Test case Id=96187
        /// </summary> 
        [TestDetailsAttribute(Id = 96187, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.InkJet, Description = "Manual IP Address - Switching Between Wired & Wireless")]
        public bool Test_96187()
        {
            return IPConfigurationTemplates.VerifyManualIPWiredToWireless(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=678837
        /// </summary> 
        [TestDetailsAttribute(Id = 678837, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.InkJet, Description = "Interfaces - Different Subnet of IP")]
        public bool Test_678837()
        {
            return IPConfigurationTemplates.VerifySubnetsWiredWirelessInterfaces(_activityData, false, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=678836
        /// </summary> 
        [TestDetailsAttribute(Id = 678836, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.InkJet, Description = "Interfaces - Same Subnet of IP")]
        public bool Test_678836()
        {
            return IPConfigurationTemplates.VerifySubnetsWiredWirelessInterfaces(_activityData, true, CtcUtility.GetTestId());
        }

        #endregion

        #region TPS Specific Tests

        /// <summary>
        /// Test case Id=96193
        /// </summary>
        [TestDetailsAttribute(Id = 96193, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS | ProductFamilies.InkJet, Description = "Net stack Reset on Hose Break")]
        public bool Test_96193()
        {
            return IPConfigurationTemplates.VerifyNetStackResetOnHoseBreak(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case ID=96161
        /// </summary>                   
        [TestDetailsAttribute(Id = 96161, Category = "Basic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.TPS, Description = "Host Name and Domain Name Changes")]
        public bool Test_96161()
        {
            return IPConfigurationTemplates.NetStackResetOnHostNameDomainNameChange(_activityData, CtcUtility.GetTestId());
        }

        #endregion

        #endregion

        #region Advanced IPv6 Configuration Tests

        #region Config Precedence

        /// <summary>
        /// Test case Id=77410
        /// </summary>        
        [TestDetailsAttribute(Id = 77410, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Config Precedence - DHCP/BOOTP Highest Priority")]
        public bool Test_77410()
        {
            return IPConfigurationTemplates.VerifyDHCPBootpHighestPrecedence(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=77411
        /// </summary>        
        [TestDetailsAttribute(Id = 77411, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Config Precedence - DHCPv6 Highest Priority")]
        public bool Test_77411()
        {
            return IPConfigurationTemplates.VerifyDHCPv6HighestPrecedence(_activityData);
        }

        /// <summary>
        /// Test case Id=77412
        /// </summary>        
        [TestDetailsAttribute(Id = 77412, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP | ProductFamilies.TPS, Description = "Config Precedence - Manual Highest Priority")]
        public bool Test_77412()
        {
            return IPConfigurationTemplates.VerifyManualHighestPrecedence(_activityData);
        }

        /// <summary>
        /// Test case Id=77413
        /// </summary>        
        [TestDetailsAttribute(Id = 77413, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Config Precedence - TFTP Highest Priority")]
        public bool Test_77413()
        {
            return IPConfigurationTemplates.VerifyTftpHighestConfigPrecedence(_activityData);
        }

        /// <summary>
        /// Test case Id=77422
        /// </summary>        
        [TestDetailsAttribute(Id = 77422, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Reset to Default Schema")]
        public bool Test_77422()
        {
            return IPConfigurationTemplates.VerifyDefaultPrecedenceWithResetOption(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=659573
        /// </summary>        
        [TestDetailsAttribute(Id = 659573, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Config Precedence - Default Option")]
        public bool Test_659573()
        {
            return IPConfigurationTemplates.VerifyPrecedenceOrderForDefault(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=681951
        /// </summary>        
        [TestDetailsAttribute(Id = 681951, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Config Precedence - SNMP")]
        public bool Test_681951()
        {
            return IPConfigurationTemplates.VerifyConfigPrecedenceUsingSnmp(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=681961
        /// </summary>        
        [TestDetailsAttribute(Id = 681961, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Config Precedence - DHCP-Manual-DHCP")]
        public bool Test_681961()
        {
            return IPConfigurationTemplates.VerifyConfigPrecedenceUsingDifferentMethod(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=77421
        /// </summary>        
        [TestDetailsAttribute(Id = 77421, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP | ProductFamilies.TPS, Description = "Cold Reset - Config Precedence Table")]
        public bool Test_77421()
        {
            return IPConfigurationTemplates.VerifyParametersAfterColdReset(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=77423
        /// </summary>        
        [TestDetailsAttribute(Id = 77423, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Power Cycle - Config Precedence Table")]
        public bool Test_77423()
        {
            return IPConfigurationTemplates.VerifyPrecedenceAfterPowerCycle(_activityData);
        }

        #endregion

        #region IP Parameter Configuration

        /// <summary>
        /// Test case Id=77415
        /// </summary>        
        [TestDetailsAttribute(Id = 77415, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Parameter Configuration - Telnet")]
        public bool Test_77415()
        {
            return IPConfigurationTemplates.VerifyParameterConfigurableWithTelnet(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=77416
        /// </summary>        
        [TestDetailsAttribute(Id = 77416, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Parameter Configuration - WebUI")]
        public bool Test_77416()
        {
            return IPConfigurationTemplates.VerifyParameterConfigurableWithWebUI(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=77417
        /// </summary>        
        [TestDetailsAttribute(Id = 77417, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Parameter Configuration - SNMP")]
        public bool Test_77417()
        {
            return IPConfigurationTemplates.VerifyParameterConfigurableWithSNMP(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=91829
        /// </summary>        
        [TestDetailsAttribute(Id = 91829, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "IP Parameter Configuration - Stateless DHCPv4")]
        public bool Test_91829()
        {
            return IPConfigurationTemplates.VerifyParameterWithStatelessOption(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=91830
        /// </summary>        
        [TestDetailsAttribute(Id = 91830, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Parameter Configuration - DHCP v4-v6 Servers")]
        public bool Test_91830()
        {
            return IPConfigurationTemplates.VerifyDHCPv4v6HostName(_activityData);
        }

        /// <summary>
        /// Test case Id=91831
        /// </summary>        
        [TestDetailsAttribute(Id = 91831, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Parameter Configuration - Precedence Table with Default Value")]
        public bool Test_91831()
        {
            return IPConfigurationTemplates.VerifyParameterWithDefaultValue(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=681971
        /// </summary>        
        [TestDetailsAttribute(Id = 681971, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Parameter Configuration - Different UI")]
        public bool Test_681971()
        {
            return IPConfigurationTemplates.VerifyParameterWithDifferentUI(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=682046
        /// </summary>  
        [TestDetailsAttribute(Id = 682046, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Parameter Update - AutoIP")]
        public bool Test_682046()
        {
            return IPConfigurationTemplates.VerifyAutoIPandDefaultParameters(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=77419
        /// </summary>        
        [TestDetailsAttribute(Id = 77419, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Parameter Update -  DHCPv4 renewal")]
        public bool Test_77419()
        {
            return IPConfigurationTemplates.VerifyParameterAfterDhcpRenewal(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=77420
        /// </summary>        
        [TestDetailsAttribute(Id = 77420, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Parameter Update -  DHCPv6 renewal")]
        public bool Test_77420()
        {
            return IPConfigurationTemplates.VerifyDHCPv6Renewal(_activityData);
        }

        /// <summary>
        /// Test case Id=682044
        /// </summary>        
        [TestDetailsAttribute(Id = 682044, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Parameter Update - Different Config Precedence")]
        public bool Test_682044()
        {
            return IPConfigurationTemplates.VerifyHostnameWithDifferentUI(_activityData);
        }

        /// <summary>
        /// Test case Id=682045
        /// </summary>        
        [TestDetailsAttribute(Id = 682045, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "IP Parameter Update - DHCPv4 Rebinding")]
        public bool Test_682045()
        {
            return IPConfigurationTemplates.VerifyParameterWithDhcpRebind(_activityData, CtcUtility.GetTestId());
        }

        #endregion

        #region LAA

        /// <summary>
        /// Test case Id=77396
        /// </summary> 
        [TestDetailsAttribute(Id = 77396, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Description = "LAA Configuration - Telnet")]
        public bool Test_77396()
        {
            return IPConfigurationTemplates.LaaConfigurationTelnet(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=77397
        /// </summary> 
        [TestDetailsAttribute(Id = 77397, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Description = "LAA Configuration - WebUI")]
        public bool Test_77397()
        {
            return IPConfigurationTemplates.LaaConfigurationWebUI(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=77407
        /// </summary> 
        [TestDetailsAttribute(Id = 77407, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Description = "Cold Reset - LAA")]
        public bool Test_77407()
        {
            return IPConfigurationTemplates.VerifyLaaAfterColdReset(_activityData, CtcUtility.GetTestId());
        }

        #endregion

        #region Advanced IPv6

        /// <summary>
        /// Test case Id=664238
        /// </summary> 
        [TestDetailsAttribute(Id = 664238, Category = "Advanced", Protocol = ProtocolType.IPv6, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Perform DHCPv6 only when Requested by the Router - Stateless Checkbox is Disabled")]
        public bool Test_664238()
        {
            return IPConfigurationTemplates.VerifyStatelessAddressDisabledWithMandOFlagSet(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=666762
        /// </summary> 
        [TestDetailsAttribute(Id = 666762, Category = "Advanced", Protocol = ProtocolType.IPv6, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Perform DHCPv6 when Stateless Configuration is Unsuccessful or Disabled - Link Local Network")]
        public bool Test_666762()
        {
            return IPConfigurationTemplates.VerifyDhcpOptionWithStateless(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id - 681963
        /// </summary>        
        [TestDetailsAttribute(Id = 681963, Category = "Advanced", Protocol = ProtocolType.IPv6, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Perform DHCPv6 when Stateless Configuration is Unsuccessful or Disabled - Non Link Local Network")]
        public bool Test_681963()
        {
            return IPConfigurationTemplates.VerifyStatelessOptionConfiguration(_activityData, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=666774
        /// </summary> 
        [TestDetailsAttribute(Id = 666774, Category = "Advanced", Protocol = ProtocolType.IPv6, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Always Perform DHCPv6 on the Startup - Non Link Local Network")]
        public bool Test_666774()
        {
            return IPConfigurationTemplates.VerifyDhcpv6OptionOnStartupMOFlagsDisabled(_activityData, CtcUtility.GetTestId());
        }


        #endregion

        #region Link Speed

        /// <summary>
        /// Test case Id=682048
        /// </summary>   
        [TestDetailsAttribute(Id = 682048, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Link Speed - Auto - DHCP")]
        public bool Test_682048()
        {
            return IPConfigurationTemplates.VerifyAutoLinkSpeedWithDhcp(_activityData, IPConfigMethod.DHCP, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=682049
        /// </summary>   
        [TestDetailsAttribute(Id = 682049, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "Link Speed - Auto - BOOTP")]
        public bool Test_682049()
        {
            return IPConfigurationTemplates.VerifyAutoLinkSpeedWithDhcp(_activityData, IPConfigMethod.BOOTP, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=682050
        /// </summary>   
        [TestDetailsAttribute(Id = 682050, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Link Speed - 100Tx - BOOTP")]
        public bool Test_682050()
        {
            return IPConfigurationTemplates.Verify100TXLinkSpeed(_activityData, IPConfigMethod.BOOTP, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=682051
        /// </summary>   
        [TestDetailsAttribute(Id = 682051, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.All, Description = "Link Speed - 100Tx - DHCP")]
        public bool Test_682051()
        {
            return IPConfigurationTemplates.Verify100TXLinkSpeed(_activityData, IPConfigMethod.DHCP, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=682052
        /// </summary>   
        [TestDetailsAttribute(Id = 682052, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Link Speed - 1000Tx - BOOTP")]
        public bool Test_682052()
        {
            return IPConfigurationTemplates.Verify1000TXLinkSpeed(_activityData, IPConfigMethod.BOOTP, CtcUtility.GetTestId());
        }

        /// <summary>
        /// Test case Id=682053
        /// </summary>   
        [TestDetailsAttribute(Id = 682053, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Link Speed - 1000Tx - DHCP")]
        public bool Test_682053()
        {
            return IPConfigurationTemplates.Verify1000TXLinkSpeed(_activityData, IPConfigMethod.DHCP, CtcUtility.GetTestId());
        }

        #endregion

        #region Router

        /// <summary>
        /// Test case Id=96205
        /// </summary> 
        [TestDetailsAttribute(Id = 96205, Category = "Advanced", Protocol = ProtocolType.IPv6, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Only O Flag Set")]
        public bool Test_96205()
        {
            return IPConfigurationTemplates.VerifyOFlag(_activityData);
        }

        /// <summary>
        /// Test case Id=96206
        /// </summary> 
        [TestDetailsAttribute(Id = 96206, Category = "Advanced", Protocol = ProtocolType.IPv6, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Only M Flag Set")]
        public bool Test_96206()
        {
            return IPConfigurationTemplates.VerifyMFlag(_activityData);
        }

        /// <summary>
        /// Test case Id=415932
        /// </summary> 
        [TestDetailsAttribute(Id = 415932, Category = "Advanced", Protocol = ProtocolType.IPv6, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "Enable & Disable - M & O Flags")]
        public bool Test_415932()
        {
            return IPConfigurationTemplates.VerifyMandOFlags(_activityData);
        }

        #endregion

        #region IP Acquisition

        /// <summary>
        /// Test case Id=77046
        /// </summary> 
        [TestDetailsAttribute(Id = 77046, Category = "Advanced", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Description = "DHCP Configuration - IPv4 &  IPv6 - Server on Remote Subnet")]
        public bool Test_77046()
        {
            return IPConfigurationTemplates.VerifyRemoteSubnet(_activityData, CtcUtility.GetTestId());
        }

        #endregion

        #endregion

    }
}
