using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.WebProxy
{
    public class WebProxyTests : CtcBaseTests
    {
        #region Local Variables

        WebProxyActivityData _activityData;

        #endregion

        #region Constructor

        /// <summary>
        /// WebProxyTests Constructor
        /// </summary>
        /// <param name="activityData">ActivityData</param>
        public WebProxyTests(WebProxyActivityData activityData)
            : base(activityData.ProductName)
        {
            _activityData = activityData;
            ProductFamily = activityData.ProductFamily;
            Sliver = "Web Proxy";
        }

        #endregion

        #region Tests
        /// <summary>
        /// Test case ID=1
        /// </summary>                   
        [TestDetailsAttribute(Id = 1, Category = "Manual Proxy", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Unsecure Web Proxy")]
        public bool Test_1()
        {
            return WebProxyTemplates.UnsecureWebProxy(_activityData);
        }

        /// <summary>
        /// Test case ID=2
        /// </summary>                   
        [TestDetailsAttribute(Id = 2, Category = "Manual Proxy", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Secure Web Proxy")]
        public bool Test_2()
        {
            return WebProxyTemplates.SecureWebProxy(_activityData);
        }

        /// <summary>
        /// Test case ID=3
        /// </summary>                   
        [TestDetailsAttribute(Id = 3, Category = "Manual Proxy", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Invalid Web Proxy")]
        public bool Test_3()
        {
            return WebProxyTemplates.InvalidWebProxy(_activityData);
        }

        /// <summary>
        /// Test case ID=4
        /// </summary>                   
        [TestDetailsAttribute(Id = 4, Category = "Manual Proxy", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Power Cycle")]
        public bool Test_4()
        {
            return WebProxyTemplates.ManualProxyPowerCycle(_activityData);
        }

        /// <summary>
        /// Test case ID=5
        /// </summary>                   
        [TestDetailsAttribute(Id = 5, Category = "Manual Proxy", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Restore Factory Settings")]
        public bool Test_5()
        {
            return WebProxyTemplates.ManualProxyColdReset(_activityData);
        }

        /// <summary>
        /// Test case ID=6
        /// </summary>                   
        [TestDetailsAttribute(Id = 6, Category = "Manual Proxy", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Manual Proxy - FQDN")]
        public bool Test_6()
        {
            return WebProxyTemplates.FQDNWebProxy(_activityData);
        }

        /// <summary>
        /// Test case ID=7
        /// </summary>                   
        [TestDetailsAttribute(Id = 7, Category = "Manual Proxy", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Secure Web Proxy - SNMP")]
        public bool Test_7()
        {
            return WebProxyTemplates.SecureWebProxySNMP(_activityData);
        }

        /// <summary>
        /// Test case ID=8
        /// </summary>                   
        [TestDetailsAttribute(Id = 8, Category = "Disable Proxy", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "No Web Proxy")]
        public bool Test_8()
        {
            return WebProxyTemplates.DisableWebProxy(_activityData);
        }

        /// <summary>
        /// Test case ID=9
        /// </summary>                   
        [TestDetailsAttribute(Id = 9, Category = "Disable Proxy", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Disable Proxy - SNMP")]
        public bool Test_9()
        {
            return WebProxyTemplates.DisableWebProxySNMP(_activityData);
        }

        /// <summary>
        /// Test case ID=10
        /// </summary>                   
        [TestDetailsAttribute(Id = 10, Category = "cURL", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Valid cURL")]
        public bool Test_10()
        {
            return WebProxyTemplates.cURLWebProxy(_activityData);
        }

        /// <summary>
        /// Test case ID=11
        /// </summary>                   
        [TestDetailsAttribute(Id = 11, Category = "cURL", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Invalid cURL")]
        public bool Test_11()
        {
            return WebProxyTemplates.InvalidcURL(_activityData);
        }

        /// <summary>
        /// Test case ID=12
        /// </summary>                   
        [TestDetailsAttribute(Id = 12, Category = "cURL", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "cURL - FQDN")]
        public bool Test_12()
        {
            return WebProxyTemplates.cURLFQDN(_activityData);
        }

        /// <summary>
        /// Test case ID=13
        /// </summary>                   
        [TestDetailsAttribute(Id = 13, Category = "cURL", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "cURL - SNMP")]
        public bool Test_13()
        {
            return WebProxyTemplates.cURLWebProxySNMP(_activityData);
        }

        /// <summary>
        /// Test case ID=14
        /// </summary>                   
        [TestDetailsAttribute(Id = 14, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Automatic Proxy (DHCP Discovery) - IP Address")]
        public bool Test_14()
        {
            return WebProxyTemplates.AutoWebProxy(_activityData);
        }

        /// <summary>
        /// Test case ID=15
        /// </summary>                   
        [TestDetailsAttribute(Id = 15, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Automatic Proxy (DHCP Discovery) - FQDN")]
        public bool Test_15()
        {
            return WebProxyTemplates.AutoWebProxyFQDN(_activityData);
        }

        /// <summary>
        /// Test case ID=16
        /// </summary>                   
        [TestDetailsAttribute(Id = 16, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "IP Config Method Change - BOOTP to DHCP")]
        public bool Test_16()
        {
            return WebProxyTemplates.AutoWebProxyBOOTPDHCP(_activityData);
        }

        /// <summary>
        /// Test case ID=17
        /// </summary>                   
        [TestDetailsAttribute(Id = 17, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "IP Config Method Change - DHCP to BOOTP")]
        public bool Test_17()
        {
            return WebProxyTemplates.AutoWebProxyDHCPBOOTP(_activityData);
        }

        /// <summary>
        /// Test case ID=18
        /// </summary>                   
        [TestDetailsAttribute(Id = 18, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "IP Config Method Change - Manual to DHCP")]
        public bool Test_18()
        {
            return WebProxyTemplates.AutoWebProxyManualDHCP(_activityData);
        }

        /// <summary>
        /// Test case ID=19
        /// </summary>                   
        [TestDetailsAttribute(Id = 19, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "IP Config Method Change - DHCP to Manual")]
        public bool Test_19()
        {
            return WebProxyTemplates.AutoWebProxyDHCPManual(_activityData);
        }

        /// <summary>
        /// Test case ID=20
        /// </summary>                   
        [TestDetailsAttribute(Id = 20, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Stateless IPv4 - Enabled")]
        public bool Test_20()
        {
            return WebProxyTemplates.AutoWebProxyStatelessDHCPv4Enabled(_activityData);
        }

        /// <summary>
        /// Test case ID=21
        /// </summary>                   
        [TestDetailsAttribute(Id = 21, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Stateless IPv4 - Disabled")]
        public bool Test_21()
        {
            return WebProxyTemplates.AutoWebProxyStatelessDHCPv4Disabled(_activityData);
        }

        /// <summary>
        /// Test case ID=22
        /// </summary>                   
        [TestDetailsAttribute(Id = 22, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Lease Time Expiry - Proxy Retention")]
        public bool Test_22()
        {
            return WebProxyTemplates.AutoWebProxyLeaseTimeProxyRetention(_activityData);
        }

        /// <summary>
        /// Test case ID=23
        /// </summary>                   
        [TestDetailsAttribute(Id = 23, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Lease Time Expiry - No Proxy Server")]
        public bool Test_23()
        {
            return WebProxyTemplates.AutoWebProxyLeaseTimeNoProxy(_activityData);
        }

        /// <summary>
        /// Test case ID=24
        /// </summary>                   
        [TestDetailsAttribute(Id = 24, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Lease Time Expiry - New Proxy Server")]
        public bool Test_24()
        {
            return WebProxyTemplates.AutoWebProxyLeaseTimeNewProxy(_activityData);
        }

        /// <summary>
        /// Test case ID=25
        /// </summary>                   
        [TestDetailsAttribute(Id = 25, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Lease Time Expiry - DHCP Discovery to DNS Discovery")]
        public bool Test_25()
        {
            return WebProxyTemplates.AutoWebProxyLeaseTimeDHCPToDNS(_activityData);
        }

        /// <summary>
        /// Test case ID=26
        /// </summary>                   
        [TestDetailsAttribute(Id = 26, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Lease Time Expiry - DNS Discovery to DHCP Discovery")]
        public bool Test_26()
        {
            return WebProxyTemplates.AutoWebProxyLeaseTimeDNSToDHCP(_activityData);
        }

        /// <summary>
        /// Test case ID=27
        /// </summary>                   
        [TestDetailsAttribute(Id = 27, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Automatic Proxy (DNS Discovery) - Domain Name")]
        public bool Test_27()
        {
            return WebProxyTemplates.AutoWebProxyDNSDiscoveryDomainName(_activityData);
        }

        /// <summary>
        /// Test case ID=28
        /// </summary>                   
        [TestDetailsAttribute(Id = 28, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Automatic Proxy (DNS Discovery) - DNS Suffix")]
        public bool Test_28()
        {
            return WebProxyTemplates.AutoWebProxyDNSDiscoveryDNSSuffix(_activityData);
        }

        /// <summary>
        /// Test case ID=29
        /// </summary>                   
        [TestDetailsAttribute(Id = 29, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Proxy Expiry - Proxy Retention")]
        public bool Test_29()
        {
            return WebProxyTemplates.AutoWebProxyExpiryTimeProxyRetention(_activityData);
        }

        /// <summary>
        /// Test case ID=30
        /// </summary>                   
        [TestDetailsAttribute(Id = 30, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Proxy Expiry - New Proxy Server")]
        public bool Test_30()
        {
            return WebProxyTemplates.AutoWebProxyExpiryTimeNewProxy(_activityData);
        }

        /// <summary>
        /// Test case ID=31
        /// </summary>                   
        [TestDetailsAttribute(Id = 31, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Discovery Priority")]
        public bool Test_31()
        {
            return WebProxyTemplates.AutoWebProxyDiscoveryPriority(_activityData);
        }

        /// <summary>
        /// Test case ID=32
        /// </summary>                   
        [TestDetailsAttribute(Id = 32, Category = "Automatic", Protocol = ProtocolType.IPv4, ProductCategory = ProductFamilies.LFP, Connectivity = ConnectivityType.Wired, Description = "Automatic Proxy - SNMP")]
        public bool Test_32()
        {
            return WebProxyTemplates.AutoWebProxySNMP(_activityData);
        }

        #endregion

    }
}