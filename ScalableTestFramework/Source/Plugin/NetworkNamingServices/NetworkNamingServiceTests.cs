using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.NetworkNamingServices
{
    public class NetworkNamingServiceTests : CtcBaseTests
    {
        #region Local Variables

        NetworkNamingServiceActivityData _activityData;

        #endregion

        #region Constructor

        /// <summary>
        /// NetworkDiscoveryTests Constructor
        /// </summary>
        /// <param name="activityData">ActivityData</param>
        public NetworkNamingServiceTests(NetworkNamingServiceActivityData activityData)
            : base(activityData.ProductName)
        {
            _activityData = activityData;
            ProductFamily = activityData.ProductFamily;
            Sliver = "NNS";
        }

        #endregion

        #region Tests

        [TestDetailsAttribute(Id = 702617, Category = "LLMNR", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "LLMNR Enabled and Disabled state")]
        public bool Test_702617()
        {
            return NetworkNamingServiceTemplates.VerifyLlmnrWithLlmnrStateChange(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702618, Category = "LLMNR", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "IPv4 Multicast Enabled and Disabled state")]
        public bool Test_702618()
        {
            return NetworkNamingServiceTemplates.VerifyLlmnrWithMulticastIpv4StateChange(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702619, Category = "LLMNR", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "IP Stack Enabled and Disabled state")]
        public bool Test_702619()
        {
            return NetworkNamingServiceTemplates.VerifyLlmnrWithIpStackStateChange(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 75439, Category = "LLMNR", ProductCategory = ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Devices behavior when Hostname uniqueness verification fails using LLMNR Wired")]
        public bool Test_75439()
        {
            return NetworkNamingServiceTemplates.VerifyLLMNRHostNameUniqueness(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702632, Category = "LLMNR", ProductCategory = ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Hostname uniqueness verification when LLMNR is enabled")]
        public bool Test_702632()
        {
            return NetworkNamingServiceTemplates.VerifyLLMNRHostNameIPAddressUniqueness(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 698315, Category = "WINS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "WINS Registration - Primary Server")]
        public bool Test_698315()
        {
            return NetworkNamingServiceTemplates.VerifyPrimaryWinsServerRegistration(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 678968, Category = "WINS", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Description = "Syslog with wired & wireless interfaces")]
        public bool Test_678968()
        {
            return NetworkNamingServiceTemplates.VerifySyslogWiredWireless(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702609, Category = "WINS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "WINS Registration - Secondary Server")]
        public bool Test_702609()
        {
            return NetworkNamingServiceTemplates.VerifySecondaryWinsServerRegistration(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702610, Category = "WINS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "WINS Registration - After changing Host name, IP address and Subnet mask")]
        public bool Test_702610()
        {
            return NetworkNamingServiceTemplates.VerifyDeviceRegistrationWithParameterChange(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702611, Category = "WINS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "WINS Registration - Syslog message")]
        public bool Test_702611()
        {
            return NetworkNamingServiceTemplates.VerifySyslogForWinsRegistration(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702628, Category = "WINS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "WINS responds to broadcast - Primary WINS server is not configured")]
        public bool Test_702628()
        {
            return NetworkNamingServiceTemplates.VerifyWinsBroadcastNameQuery(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702629, Category = "WINS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "WINS responds to unicast - Primary WINS server is configured")]
        public bool Test_702629()
        {
            return NetworkNamingServiceTemplates.VerifyPNodeDifferentSubnet(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702630, Category = "WINS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "WINS response to a node status - Primary WINS server is configured")]
        public bool Test_702630()
        {
            return NetworkNamingServiceTemplates.VerifyPNodeSameSubnet(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702633, Category = "WINS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Hostname uniqueness verification - Primary Server")]
        public bool Test_702633()
        {
            return NetworkNamingServiceTemplates.VerifyHostNameUniquenessPrimaryServer(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702634, Category = "WINS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Hostname uniqueness verification - Secondary Server")]
        public bool Test_702634()
        {
            return NetworkNamingServiceTemplates.VerifyHostNameUniquenessSecondaryServer(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702641, Category = "WINS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Retry to primary WINS server")]
        public bool Test_702641()
        {
            return NetworkNamingServiceTemplates.VerifyRetryToPrimaryWinsServer(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 75405, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "DNS suffix using  manual entries")]
        public bool Test_75405()
        {
            return NetworkNamingServiceTemplates.VerifyManualDnsSuffix(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 75409, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "DNS suffixes using invalid entries")]
        public bool Test_75409()
        {
            return NetworkNamingServiceTemplates.VerifyInvalidDnsSuffix(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 231118, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Device not sending any unexpected DNS queries")]
        public bool Test_231118()
        {
            return NetworkNamingServiceTemplates.VerifyUnexpectedDnsQueries(_activityData, true, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702615, Category = "DNS", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Description = "Resolution using NTS with multiple entries in the DNS server")]
        public bool Test_702615()
        {
            return NetworkNamingServiceTemplates.VerifyNtsNameResolution(_activityData, true, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 66702615, Category = "DNS", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv6, Description = "Resolution using NTS with multiple entries in the DNS server")]
        public bool Test_66702615()
        {
            return NetworkNamingServiceTemplates.VerifyNtsNameResolution(_activityData, false, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702616, Category = "DNS", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Description = "DNS name resolution using NTS")]
        public bool Test_702616()
        {
            return NetworkNamingServiceTemplates.VerifyDnsResolutionUsingNts(_activityData, true, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 66702616, Category = "DNS", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv6, Description = "DNS name resolution using NTS")]
        public bool Test_66702616()
        {
            return NetworkNamingServiceTemplates.VerifyDnsResolutionUsingNts(_activityData, false, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702638, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "DNS Name Resolution - Primary DNS server")]
        public bool Test_702638()
        {
            return NetworkNamingServiceTemplates.VerifyDnsNameResolutionPrimaryDnsServer(_activityData, true, true, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702639, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "DNS Name Resolution - Secondary DNS server")]
        public bool Test_702639()
        {
            return NetworkNamingServiceTemplates.VerifyDnsNameResolutionSecondaryDnsServer(_activityData, false, true, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 66702638, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv6, Description = "DNS Name Resolution - Primary DNS server")]
        public bool TPSTest_66702638()
        {
            return NetworkNamingServiceTemplates.VerifyDnsNameResolutionPrimaryDnsServer(_activityData, true, false, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 66702639, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv6, Description = "DNS Name Resolution - Secondary DNS server")]
        public bool TPSTest_66702639()
        {
            return NetworkNamingServiceTemplates.VerifyDnsNameResolutionSecondaryDnsServer(_activityData, false, false, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702642, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "DNS parameters provided by DHCP server and manual entries")]
        public bool Test_702642()
        {
            return NetworkNamingServiceTemplates.VerifyDnsConfigurationParameters(_activityData, true, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 66702642, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Protocol = ProtocolType.IPv6, Connectivity = ConnectivityType.Wired, Description = "DNS parameters provided by DHCP server and manual entries")]
        public bool Test_66702642()
        {
            return NetworkNamingServiceTemplates.VerifyDnsConfigurationParameters(_activityData, false, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702644, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "DNS Suffix Using Highest precedence configured")]
        public bool Test_702644()
        {
            return NetworkNamingServiceTemplates.VerifyDnsSuffixWithHighPrecedence(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 75401, Category = "DNS", ProductCategory = ProductFamilies.TPS | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Verify parameter values and behavior after Cold reset is performed on the device Wired")]
        public bool Test_75401()
        {
            return NetworkNamingServiceTemplates.VerifyParameterValuesOnColdReset(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702624, Category = "DDNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "DDNS Functionality using different UI")]
        public bool Test_702624()
        {
            return NetworkNamingServiceTemplates.VerifyDdnsUsingDifferentUI(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702625, Category = "DDNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "DDNS Functionality with DNS server across Subnet")]
        public bool Test_702625()
        {
            return NetworkNamingServiceTemplates.VerifyDdnsAcrossSubnet(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702626, Category = "DDNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "DDNS records are update with changes to IP parameters")]
        public bool Test_702626()
        {
            return NetworkNamingServiceTemplates.VerifyDdnsWithIPParameters(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702636, Category = "DDNS", ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "DDNS Functionality with Cold Reset")]
        public bool Test_702636()
        {
            return NetworkNamingServiceTemplates.VerifyDdnsWithColdReset(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702637, Category = "DDNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "DDNS Functionality with Power cycle")]
        public bool Test_702637()
        {
            return NetworkNamingServiceTemplates.VerifyDdnsWithPowerCycle(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702640, Category = "DDNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "DNS Resolution using DDNS Record with different DNS servers")]
        public bool Test_702640()
        {
            return NetworkNamingServiceTemplates.VerifyDdnsWithDifferentDnsServers(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702650, Category = "DDNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "DHCP renew for DDNS")]
        public bool Test_702650()
        {
            return NetworkNamingServiceTemplates.VerifyDdnsWithDhcpRenew(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702649, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "DNS suffix configuration [Option 119] from DHCP Server")]
        public bool Test_702649()
        {
            return NetworkNamingServiceTemplates.VerifyDnsSuffix(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702614, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Client DNS resolution using SNMP trap")]
        public bool Test_702614()
        {
            return NetworkNamingServiceTemplates.VerifySnmpTrap(_activityData, true, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 66702614, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Protocol = ProtocolType.IPv6, Connectivity = ConnectivityType.Wired, Description = "Client DNS resolution using SNMP trap")]
        public bool Test_66702614()
        {
            return NetworkNamingServiceTemplates.VerifySnmpTrap(_activityData, false, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702613, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Hostname-FQDN resolution using DNS and SNMP trap")]
        public bool Test_702613()
        {
            return NetworkNamingServiceTemplates.VerifyFqdnAccess(_activityData, true, true, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 66702613, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Protocol = ProtocolType.IPv6, Connectivity = ConnectivityType.Wired, Description = "Hostname-FQDN resolution using DNS and SNMP trap")]
        public bool Test_66702613()
        {
            return NetworkNamingServiceTemplates.VerifyFqdnAccess(_activityData, true, false, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 702622, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Host Name resolution across routers using SNMP trap")]
        public bool Test_702622()
        {
            return NetworkNamingServiceTemplates.HostNameResolutionAcrossRouters(_activityData, true, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 66702622, Category = "DNS", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Protocol = ProtocolType.IPv6, Connectivity = ConnectivityType.Wired, Description = "Host Name resolution across routers using SNMP trap")]
        public bool Test_66702622()
        {
            return NetworkNamingServiceTemplates.HostNameResolutionAcrossRouters(_activityData, false, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 75396, Category = "RFC", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "RFC compliance option Enabled from UI")]
        public bool Test_75396()
        {
            return NetworkNamingServiceTemplates.VerifyRfcComplianceOption(_activityData, true);
        }

        [TestDetailsAttribute(Id = 75397, Category = "RFC", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "RFC compliance option Disabled from UI")]
        public bool Test_75397()
        {
            return NetworkNamingServiceTemplates.VerifyRfcComplianceOption(_activityData, false);
        }

        [TestDetailsAttribute(Id = 702631, Category = "RFC", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "RFC option disabled and both hostname and domain name are specified in server")]
        public bool Test_702631()
        {
            return NetworkNamingServiceTemplates.VerifyHostNameWithRfcCompliance(_activityData, false);
        }

        [TestDetailsAttribute(Id = 702635, Category = "RFC", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "RFC option enabled and both hostname and domain name are specified in server")]
        public bool Test_702635()
        {
            return NetworkNamingServiceTemplates.VerifyHostNameWithRfcCompliance(_activityData, true);
        }

        [TestDetailsAttribute(Id = 702646, Category = "RFC", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "FQDN configuration Using hostname and Domain name Provided by server")]
        public bool Test_702646()
        {
            return NetworkNamingServiceTemplates.VerifyFqdnConfiguration(_activityData);
        }

        [TestDetailsAttribute(Id = 75402, Category = "RFC", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP | ProductFamilies.TPS, Connectivity = ConnectivityType.Wired, Description = "Verify parameter values and behavior of the device after power cycle")]
        public bool Test_75402()
        {
            return NetworkNamingServiceTemplates.VerifyBehaviorAfterPowercycle(_activityData);
        }

        [TestDetailsAttribute(Id = 702647, Category = "RFC", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "FQDN Configuration  With vendor specific option enabled")]
        public bool Test_702647()
        {
            return NetworkNamingServiceTemplates.VerifyFQDNWithVendorOption(_activityData, true);
        }

        [TestDetailsAttribute(Id = 702648, Category = "RFC", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "FQDN Configuration  With vendor specific option disabled")]
        public bool Test_702648()
        {
            return NetworkNamingServiceTemplates.VerifyFQDNWithVendorOption(_activityData, false);
        }

        [TestDetailsAttribute(Id = 702612, Category = "DNS Outbound", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Description = "Scan to Network Folder - Primary Server")]
        public bool Test_702612()
        {
            return NetworkNamingServiceTemplates.VerifyScanToNetworkFolder(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 66702612, Category = "DNS Outbound", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv6, Description = "Scan to Network Folder - Primary Server")]
        public bool Test_66702612()
        {
            return NetworkNamingServiceTemplates.VerifyScanToNetworkFolder(_activityData, CtcUtility.GetTestId(), isIPv4: false);
        }

        [TestDetailsAttribute(Id = 2702612, Category = "DNS Outbound", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Description = "Scan to Network Folder - Secondary Server")]
        public bool Test_S702612()
        {
            return NetworkNamingServiceTemplates.VerifyScanToNetworkFolder(_activityData, CtcUtility.GetTestId(), isPrimary: false);
        }

        [TestDetailsAttribute(Id = 266702612, Category = "DNS Outbound", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv6, Description = "Verify Scan to Network Folder - Secondary Server")]
        public bool Test_S66702612()
        {
            return NetworkNamingServiceTemplates.VerifyScanToNetworkFolder(_activityData, CtcUtility.GetTestId(), isIPv4: false, isPrimary: false);
        }

        [TestDetailsAttribute(Id = 702621, Category = "DNS Outbound", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Description = "Scan to Network Folder with Invalid entries - Primary Server")]
        public bool Test_702621()
        {
            return NetworkNamingServiceTemplates.VerifyScanToNetworkFolder(_activityData, CtcUtility.GetTestId(), invalidEntries: true);
        }

        [TestDetailsAttribute(Id = 66702621, Category = "DNS Outbound", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv6, Description = "Scan to Network Folder with Invalid entries - Primary Server")]
        public bool Test_66702621()
        {
            return NetworkNamingServiceTemplates.VerifyScanToNetworkFolder(_activityData, CtcUtility.GetTestId(), isIPv4: false, invalidEntries: true);
        }

        [TestDetailsAttribute(Id = 2702621, Category = "DNS Outbound", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Description = "Scan to Network Folder with Invalid entries - Secondary Server")]
        public bool Test_S702621()
        {
            return NetworkNamingServiceTemplates.VerifyScanToNetworkFolder(_activityData, CtcUtility.GetTestId(), isPrimary: false, invalidEntries: true);
        }

        [TestDetailsAttribute(Id = 266702621, Category = "DNS Outbound", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv6, Description = "Scan to Network Folder with Invalid entries - Secondary Server")]
        public bool Test_S66702621()
        {
            return NetworkNamingServiceTemplates.VerifyScanToNetworkFolder(_activityData, CtcUtility.GetTestId(), isIPv4: false, isPrimary: false, invalidEntries: true);
        }

        [TestDetailsAttribute(Id = 702623, Category = "DNS Outbound", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Description = "Scan to network folder-Configure DNS Suffixes dynamically and verify resolution - Primary Server")]
        public bool Test_702623()
        {
            return NetworkNamingServiceTemplates.VerifyScanToNetworkFolder(_activityData, CtcUtility.GetTestId(), dnsSuffix: true);
        }

        [TestDetailsAttribute(Id = 66702623, Category = "DNS Outbound", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv6, Description = "Scan to network folder-Configure DNS Suffixes dynamically and verify resolution - Primary Server")]
        public bool Test_66702623()
        {
            return NetworkNamingServiceTemplates.VerifyScanToNetworkFolder(_activityData, CtcUtility.GetTestId(), isIPv4: false, dnsSuffix: true);
        }

        [TestDetailsAttribute(Id = 2702623, Category = "DNS Outbound", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Description = "Scan to network folder-Configure DNS Suffixes dynamically and verify resolution - Secondary Server")]
        public bool Test_S702623()
        {
            return NetworkNamingServiceTemplates.VerifyScanToNetworkFolder(_activityData, CtcUtility.GetTestId(), isPrimary: false, dnsSuffix: true);
        }

        [TestDetailsAttribute(Id = 266702623, Category = "DNS Outbound", ProductCategory = ProductFamilies.VEP, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv6, Description = "Scan to network folder-Configure DNS Suffixes dynamically and verify resolution - Secondary Server")]
        public bool Test_S66702623()
        {
            return NetworkNamingServiceTemplates.VerifyScanToNetworkFolder(_activityData, CtcUtility.GetTestId(), isIPv4: false, isPrimary: false, dnsSuffix: true);
        }

        #endregion

    }
}