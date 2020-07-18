using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;

namespace HP.ScalableTest.Plugin.CertificateManagement
{
    /// <summary>
    /// CertificateManagement Test cases
    /// </summary>
    internal class CertificateManagementTests : CtcBaseTests
    {
        #region Local Variables

        /// <summary>
        /// Instance of CertificateManagementActivityData
        /// </summary>
        CertificateManagementActivityData _activityData;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="activityData"></param>
        public CertificateManagementTests(CertificateManagementActivityData activityData)
            : base(activityData.ProductName)
        {
            _activityData = activityData;
            ProductFamily = activityData.ProductFamily.ToString();
            Sliver = "CertificateManagement";
        }

        #endregion

        #region CertificateManagement Tests

        #region SelfSigned Certificates
        /// <summary>
        ///  Test case Id = 1
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 1, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Self Signed Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "Self-Signed Certificate - RSA Key Length 1024")]
        public bool Test_1()
        {
            return CertificateManagementTemplates.CreateSelfSignedCertificateAndValidate(_activityData, RSAKeyLength.Rsa1024);
        }

        /// <summary>
        ///  Test case Id = 2
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 2, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Self Signed Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "Self-Signed Certificate - RSA Key Length 2048")]
        public bool Test_2()
        {
            return CertificateManagementTemplates.CreateSelfSignedCertificateAndValidate(_activityData, RSAKeyLength.Rsa2048);
        }

        /// <summary>
        ///  Test case Id = 3
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 3, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Self Signed Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "Self Signed Certificate - Import-Export - Mark Private Key (Enabled:Disabled)")]
        public bool Test_3()
        {
            return CertificateManagementTemplates.ExportAndImportCertificateAndValidate(_activityData, RSAKeyLength.Rsa2048, true, 1);
        }

        /// <summary>
        ///  Test case Id = 4
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 4, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Self Signed Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "Self Signed Certificate - Import-Export - Mark Private Key (Enabled:Enabled)")]
        public bool Test_4()
        {
            return CertificateManagementTemplates.ExportAndImportCertificateAndValidate(_activityData, RSAKeyLength.Rsa2048, true, 1, importPrivateKey: true);
        }

        /// <summary>
        ///  Test case Id = 5
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 5, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Self Signed Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "Self Signed Certificate - Invalid Password")]
        public bool Test_5()
        {
            return CertificateManagementTemplates.ExportAndImportCertificateAndValidate(_activityData, RSAKeyLength.Rsa2048, true, 1, true);
        }

        /// <summary>
        ///  Test case Id = 6
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 6, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "Self Signed Certificate", ProductCategory = ProductFamilies.LFP, Description = "Self Signed Certificate - Enable Wipe Option")]
        public bool Test_6()
        {
            return CertificateManagementTemplates.ValidateCertificateAvailabilityWithWipeOutOptionEnabled(_activityData, RSAKeyLength.Rsa2048);
        }

        #endregion

        #region ID Certificate

        /// <summary>
        ///  Test case Id = 10
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 10, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "ID Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "ID Certificate - RSA Key Length 1024")]
        public bool Test_10()
        {
            return CertificateManagementTemplates.CreateIDCertificateRequestWithServerSigningAndValidate(_activityData, RSAKeyLength.Rsa1024);
        }

        /// <summary>
        ///  Test case Id =11
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 11, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "ID Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "ID Certificate - RSA Key Length 2048")]
        public bool Test_11()
        {
            return CertificateManagementTemplates.CreateIDCertificateRequestWithServerSigningAndValidate(_activityData, RSAKeyLength.Rsa2048);
        }

        /// <summary>
        ///  Test case Id = 12
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 12, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "ID Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "ID Certificate - Reboot")]
        public bool Test_12()
        {
            return CertificateManagementTemplates.CreateIDCertificateRequestWithServerSigningAndReboot(_activityData);
        }

        /// <summary>
        ///  Test case Id = 13
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 13, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "ID Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "ID Certificate - Invalid Certificate")]
        public bool Test_13()
        {
            return CertificateManagementTemplates.CreateIDCertificateRequestWithInvalidCertificate(_activityData);
        }

        /// <summary>
        ///  Test case Id = 14
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 14, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "ID Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "ID Certificate - Import-Export - Mark Private Key (Enabled:Disabled)")]
        public bool Test_14()
        {
            return CertificateManagementTemplates.ExportAndImportIDCertificateAndValidate(_activityData, true, 1);
        }

        /// <summary>
        ///  Test case Id = 15
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 15, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "ID Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "ID Certificate - Import-Export - Mark Private Key (Enabled:Enabled)")]
        public bool Test_15()
        {
            return CertificateManagementTemplates.ExportAndImportIDCertificateAndValidate(_activityData, true, 1, importPrivateKey: true);
        }

        /// <summary>
        ///  Test case Id = 16
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 16, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "ID Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.LFP, Description = "ID Certificate - Invalid Password")]
        public bool Test_16()
        {
            return CertificateManagementTemplates.ExportAndImportIDCertificateAndValidate(_activityData, true, 1, true);
        }

        #endregion

        #region CA Certificate
        /// <summary>
        ///  Test case Id = 30
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 30, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "CA Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "CA Certificate - RSA Key Length 1024")]
        public bool Test_30()
        {
            return CertificateManagementTemplates.InstallCACertificateAndValidate(_activityData, RSAKeyLength.Rsa1024);
        }

        /// <summary>
        ///  Test case Id = 31
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 31, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "CA Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "CA Certificate - RSA Key Length 2048")]
        public bool Test_31()
        {
            return CertificateManagementTemplates.InstallCACertificateAndValidate(_activityData, RSAKeyLength.Rsa2048);
        }

        /// <summary>
        ///  Test case Id = 32
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 32, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "CA Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "CA Certificate - Invalid")]
        public bool Test_32()
        {
            return CertificateManagementTemplates.InstallInvalidCACertificate(_activityData);
        }

        /// <summary>
        ///  Test case Id = 33
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 33, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "CA Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "CA Certificate - Different Formats [CER/DER/PEM/CRT]")]
        public bool Test_33()
        {
            return CertificateManagementTemplates.InstallCACertificateWithDifferentFormat(_activityData);
        }

        /// <summary>
        ///  Test case Id = 34
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 34, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "CA Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "Intermediate CA Certificate - RSA Key Length 1024")]
        public bool Test_34()
        {
            return CertificateManagementTemplates.InstallCACertificateAndValidate(_activityData, RSAKeyLength.Rsa1024, true);
        }

        /// <summary>
        ///  Test case Id = 35
        /// </summary>
        /// <returns>Returns true if the test is passed else returns false</returns>
        [TestDetailsAttribute(Id = 35, Connectivity = ConnectivityType.Wired, Protocol = ProtocolType.IPv4, Category = "CA Certificate", ProductCategory = ProductFamilies.VEP | ProductFamilies.TPS | ProductFamilies.LFP, Description = "Intermediate CA Certificate - RSA Key Length 2048")]
        public bool Test_35()
        {
            return CertificateManagementTemplates.InstallCACertificateAndValidate(_activityData, RSAKeyLength.Rsa2048, true);
        }
        #endregion

        #endregion

    }
}
