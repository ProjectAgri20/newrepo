using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.Security
{
    /// <summary>
    /// Security Test cases
    /// </summary>
    public class SecurityTests : CtcBaseTests
    {
        #region Local Variables

        /// <summary>
        /// Instance of SecurityActivityData
        /// </summary>
        SecurityActivityData _activityData;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="activityData"></param>
        public SecurityTests(SecurityActivityData activityData)
            : base(activityData.ProductName)
        {
            _activityData = activityData;
            ProductFamily = activityData.ProductFamily;
            Sliver = "Security";
        }

        #endregion

        #region Tests

        #region FIPS
        [TestDetailsAttribute(Id = 577010, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "FIPS-MI", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "CA Certificate in FIPS")]
        public bool Test_577010()
        {
            return SecurityTemplates.VerifyInstallOfCACertificateinFIPS(_activityData, true);
        }

        [TestDetailsAttribute(Id = 66577010, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv6, Category = "FIPS-MI", ProductCategory = ProductFamilies.VEP, Description = "CA Certificate in FIPS -IPV6")]
        public bool Test_66577010()
        {
            return SecurityTemplates.VerifyInstallOfCACertificateinFIPS(_activityData, false);
        }

        [TestDetailsAttribute(Id = 577014, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "FIPS-MI", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "ID Certificate in FIPS")]
        public bool Test_577014()
        {
            return SecurityTemplates.VerifyInstallOfIDCertificateinFIPS(_activityData);
        }

        [TestDetailsAttribute(Id = 577071, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "FIPS-MI", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "EWS (https) access in FIPS")]
        public bool Test_577071()
        {
            return SecurityTemplates.VerifyEWSAccessUsingHTTPSinFIPS(_activityData, true);
        }

        [TestDetailsAttribute(Id = 66577071, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv6, Category = "FIPS-MI", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "EWS (https) access in FIPS-IPV6")]
        public bool Test_66577071()
        {
            return SecurityTemplates.VerifyEWSAccessUsingHTTPSinFIPS(_activityData, false);
        }

        [TestDetailsAttribute(Id = 576697, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "FIPS-MI", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "FIPS enable/disable")]
        public bool Test_576697()
        {
            return SecurityTemplates.VerifyDeviceBehaviourinFIPSEnableDisableMode(_activityData);
        }

        [TestDetailsAttribute(Id = 576699, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "FIPS-MI", ProductCategory = ProductFamilies.LFP, Description = "Reboot and Reset in FIPS")]
        public bool Test_576699()
        {
            return SecurityTemplates.VerifyRebootResetinFIPS(_activityData);
        }

        [TestDetailsAttribute(Id = 576693, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "FIPS-MI", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Interdependency check from EWS and SNMP in FIPS")]
        public bool Test_576693()
        {
            return SecurityTemplates.VerifyInterdependencyCheckfromWebUIandSNMPinFIPS(_activityData);
        }

        [TestDetailsAttribute(Id = 579476, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "FIPS-MI", ProductCategory = ProductFamilies.VEP, Description = "Firmware Upgrade and Downgrade in FIPS")]
        public bool Test_579476()
        {
            return SecurityTemplates.VerifyFirmwareUpgradeandDowngradeinFIPS(_activityData);
        }

        [TestDetailsAttribute(Id = 678828, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "FIPS-SI", ProductCategory = ProductFamilies.VEP, Description = "FIPS status across interfaces")]
        public bool Test_678828()
        {
            return SecurityTemplates.VerifyFIPSAcrossInterfaces(_activityData, CtcUtility.GetTestId());
        }

        [TestDetailsAttribute(Id = 577009, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "FIPS-SI", ProductCategory = ProductFamilies.None, Description = "FIPS on Multiple JDI Networks")]
        public bool Test_577009()
        {
            return SecurityTemplates.VerifyFIPSonMultipleJDINetworks(_activityData, CtcUtility.GetTestId());
        }

        #endregion

        #region Web Encryption

        [TestDetailsAttribute(Id = 1000, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web Encryption", ProductCategory = ProductFamilies.None, Description = "Secure Protocols")]
        public bool Test_1000()
        {
            return SecurityTemplates.VerifySecureProtocolOptions(_activityData);
        }

        [TestDetailsAttribute(Id = 2000, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web Encryption", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Strengths")]
        public bool Test_2000()
        {
            return SecurityTemplates.VerifyWebEncryptionStrengths(_activityData);
        }

        [TestDetailsAttribute(Id = 3000, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web Encryption", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Encrypt Web Communication - Enable & Disable ")]
        public bool Test_3000()
        {
            return SecurityTemplates.VerifyEncryptWebCommunicationOption(_activityData);
        }


        [TestDetailsAttribute(Id = 4000, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Web Encryption", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "Power Cycle & Restore Factory Settings")]
        public bool Test_4000()
        {
            return SecurityTemplates.VerifyWebEncryptionOnResets(_activityData);
        }

        #endregion

        #region ACL

        [TestDetailsAttribute(Id = 75628, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "ACL-(MI + SI)", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "IP Address with 'Allow Web Server Access' is disabled.")]
        public bool Test_75628()
        {
            return SecurityTemplates.ValidateAclRule(_activityData, false, false);
        }

        [TestDetailsAttribute(Id = 75629, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "ACL-(MI + SI)", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "IP Address with 'Allow Web Server Access' is enabled.")]
        public bool Test_75629()
        {
            return SecurityTemplates.ValidateAclRule(_activityData, true, false);
        }

        [TestDetailsAttribute(Id = 75630, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "ACL-(MI + SI)", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "IP Address & subnet mask with 'Allow Web Server Access' is disabled.")]
        public bool Test_75630()
        {
            return SecurityTemplates.ValidateAclRule(_activityData, false, true);
        }

        [TestDetailsAttribute(Id = 75631, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "ACL-(MI + SI)", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "IP Address & subnet mask with 'Allow Web Server Access' is enabled.")]
        public bool Test_75631()
        {
            return SecurityTemplates.ValidateAclRule(_activityData, true, true);
        }

        [TestDetailsAttribute(Id = 726946, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "ACL-SI", ProductCategory = ProductFamilies.VEP, Description = "Across interfaces in different subnets.")]
        public bool Test_726946()
        {
            return SecurityTemplates.ValidateAclRuleInMultipleInterfacesInDifferentSubnet(_activityData);
        }

        [TestDetailsAttribute(Id = 1, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "ACL-(MI + SI)", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "Power cycle & Cold reset.")]
        public bool Test_1()
        {
            return SecurityTemplates.ValidateAclRuleWithPowerCycleColdReset(_activityData);
        }

        #endregion

        #region Admin Password

        [TestDetailsAttribute(Id = 75608, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Admin Password-(MI + SI)", ProductCategory = ProductFamilies.None, Description = "To enable the option Use the Admin password as the set community name and verify the results.")]
        public bool Test_75608()
        {
            return SecurityTemplates.VerifyPasswordAsSetcommunityname(_activityData);
        }

        [TestDetailsAttribute(Id = 75611, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Admin Password-(MI + SI)", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS, Description = "Validate admin password")]
        public bool Test_75611()
        {
            return SecurityTemplates.VerifyPasswordInNetworkingPeripheral(_activityData);
        }

        [TestDetailsAttribute(Id = 75612, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Admin Password-(MI + SI)", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS, Description = "Admin password synchronization-telnet.")]
        public bool Test_75612()
        {
            return SecurityTemplates.VerifyPasswordInNetworkingPeripheralTelnet(_activityData);
        }

        [TestDetailsAttribute(Id = 407331, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Admin Password-(SI)", ProductCategory = ProductFamilies.VEP, Description = "Admin password synchronization in multiple interfaces.")]
        public bool Test_407331()
        {
            return SecurityTemplates.VerifyPasswordSynchronization(_activityData);
        }

        [TestDetailsAttribute(Id = 678826, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Admin Password-(SI)", ProductCategory = ProductFamilies.VEP, Description = "Admin password synchronization in multiple interfaces with set password, delete password, power cycle.")]
        public bool Test_678826()
        {
            return SecurityTemplates.VerifyPasswordSynchronizationPowercycle(_activityData);
        }

        [TestDetailsAttribute(Id = 577413, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "FIPS-Admin Password-MI", ProductCategory = ProductFamilies.VEP, Description = "Admin Password in FIPS")]
        public bool Test_577413()
        {
            return SecurityTemplates.VerifyDeviceUILoginusingAdminPasswordinFIPS(_activityData, true);
        }

        [TestDetailsAttribute(Id = 66577413, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv6, Category = "FIPS-Admin Password-MI", ProductCategory = ProductFamilies.VEP, Description = "Admin Password in FIPS-IPV6")]
        public bool Test_66577413()
        {
            return SecurityTemplates.VerifyDeviceUILoginusingAdminPasswordinFIPS(_activityData, false);
        }

        [TestDetailsAttribute(Id = 7269461, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "ACL-SI", ProductCategory = ProductFamilies.VEP, Description = "Across interfaces in same subnet.")]
        public bool Test_7269461()
        {
            return SecurityTemplates.ValidateAclRuleInMultipleInterfacesInSameSubnet(_activityData);
        }
        #endregion

        #endregion
    }
}
