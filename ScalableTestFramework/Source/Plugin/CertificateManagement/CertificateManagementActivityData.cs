using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.CertificateManagement
{

    /// <summary>
    /// Certificate Management Activity Data
    /// </summary>	
    [DataContract]
    internal class CertificateManagementActivityData
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Product Family of the printer VEP, TPS etc.
        /// </summary>
        [DataMember]
        public ProductFamilies ProductFamily { set; get; }

        /// <summary>
        /// Gets or sets the Product Name
        /// </summary>
        [DataMember]
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the IPv4 Address of the printer
        /// </summary>
        [DataMember]
        public string Ipv4Address { get; set; }

        /// <summary>
        /// Gets or sets the wireless IPv4 Address of the printer
        /// </summary>
        [DataMember]
        public string WirelessIpv4Address { get; set; }

        /// <summary>
        /// Gets or sets the wireless Mac Address of the printer
        /// </summary>
        [DataMember]
        public string WirelessMacAddress { get; set; }

        /// <summary>
        /// Gets or sets the SSID of the wireless network
        /// </summary>
        [DataMember]
        public string Ssid { get; set; }

        /// <summary>
        /// Gets or sets the IPv4 Address of the switch
        /// </summary>
        [DataMember]
        public string SwitchIp { get; set; }

        /// <summary>
        /// Gets or sets the Authenticator Port
        /// </summary>
        [DataMember]
        public int AuthenticatorPort { get; set; }

        /// <summary>
        /// Gets or sets the Radius Server IPAddress based on <see cref="RadiusServerTypes"/> selected.
        /// </summary>
        [DataMember]
        public string RadiusServerIp { get; set; }

        /// <summary>
        /// Gets or sets the Root SHA1 Radius Server IPAddress
        /// </summary>
        [DataMember]
        public string RootSha1ServerIp { get; set; }

        /// <summary>
        /// Gets or sets the Root SHA2 Radius Server IPAddress
        /// </summary>
        [DataMember]
        public string RootSha2ServerIp { get; set; }

        /// <summary>
        /// Gets or sets the Sub SHA2 Radius Server IPAddress
        /// </summary>
        [DataMember]
        public string SubSha2ServerIp { get; set; }

        /// <summary>
        /// Gets or sets the Radius Server username
        /// </summary>
        [DataMember]
        public string RadiusServerUserName { get; set; }

        /// <summary>
        /// Gets or sets the Radius server password
        /// </summary>
        [DataMember]
        public string RadiusServerPassword { get; set; }

        /// <summary>
        /// Gets or sets the selected test numbers
        /// </summary>
        [DataMember]
        public Collection<int> SelectedTests { get; set; }

        /// <summary>
        /// Gets or sets the sitemap path
        /// </summary>
        [DataMember]
        public string SitemapPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the sitemap version
        /// </summary>
        [DataMember]
        public string SiteMapVersion { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ConnectivityType"/>
        /// </summary>
        [DataMember]
        public ConnectivityType Connectivity { get; set; }

        /// <summary>
        /// Gets the physical machine IP address
        /// </summary>
        [DataMember]
        public string PhysicalMachineIp { get; set; }

        /// <summary>
        /// Gets the IP address of the DHCP server
        /// </summary>
        [DataMember]
        public string DhcpServerIp { get; set; }

        /// <summary>
        /// Gets or sets the mac address of the printer.
        /// </summary>
        [DataMember]
        public string MacAddress { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether print
        /// </summary>
        [DataMember]
        public bool RequirePrintValidation { get; set; }

        /// <summary>
        /// Gets or sets the Europa Wired IPv4 Address of the printer
        /// </summary>
        [DataMember]
        public string EuropaWiredIP { get; set; }

        /// <summary>
        /// Gets or sets the Serpent wireless IPv4 Address of the printer
        /// </summary>
        [DataMember]
        public string SerpentWirelessIP { get; set; }

        /// <summary>
        /// Gets or sets the Europa Port
        /// </summary>
        [DataMember]
        public int EuropaPort { get; set; }

        /// <summary>
        /// Gets or sets the Serpent Port
        /// </summary>
        [DataMember]
        public int SerpentPort { get; set; }

        /// <summary>
        /// Gets or sets the session ID
        /// </summary>
        [IgnoreDataMember]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the driver package path
        /// </summary>
        [DataMember]
        public string DriverPackagePath { get; set; }

        /// <summary>
        /// Gets or sets the driver package path
        /// </summary>
        [DataMember]
        public string DriverModel { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Create an instance of <see cref="CertificateManagementActivityData"/>
        /// </summary>
        public CertificateManagementActivityData()
        {
            SelectedTests = new Collection<int>();
            ProductFamily = ProductFamilies.None;
            ProductName = string.Empty;
            Ipv4Address = string.Empty;
            WirelessIpv4Address = string.Empty;
            WirelessMacAddress = string.Empty;
            SwitchIp = string.Empty;
            RootSha1ServerIp = string.Empty;
            RootSha2ServerIp = string.Empty;
            SubSha2ServerIp = string.Empty;
            RadiusServerUserName = string.Empty;
            RadiusServerPassword = string.Empty;
            EuropaWiredIP = string.Empty;
            SerpentWirelessIP = string.Empty;
            EuropaPort = 1;
            SerpentPort = 1;
            Connectivity = ConnectivityType.Wired;
        }

        #endregion
    }
}