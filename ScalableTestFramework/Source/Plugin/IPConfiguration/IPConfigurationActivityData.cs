using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.IPConfiguration
{
    /// <summary>
    /// IPConfiguration Activity Data
    /// </summary>
    [DataContract]
    internal class IPConfigurationActivityData
    {
        #region Properties

        /// <summary>
        /// Product Family
        /// </summary>
        [DataMember]
        public string ProductFamily { get; set; }

        /// <summary>
        /// Product Name
        /// </summary>
        [DataMember]
        public string ProductName { get; set; }

        /// <summary>
        /// The wired IPv4 Address
        /// </summary>
        [DataMember]
        public string WiredIPv4Address { get; set; }

        /// <summary>
        /// The wireless IPv4 Address
        /// </summary>
        [DataMember]
        public string WirelessIPv4Address { get; set; }

        /// <summary>
        /// The wireless MAC Address
        /// </summary>
        [DataMember]
        public string WirelessMacAddress { get; set; }

        /// <summary>
        /// Switch IP
        /// </summary>
        [DataMember]
        public string SwitchIP { get; set; }

        /// <summary>
        /// Port No
        /// </summary>
        [DataMember]
        public int PortNo { get; set; }

        /// <summary>
        /// SSID Name
        /// </summary>
        [DataMember]
        public string SsidName { get; set; }

        /// <summary>
        /// Primary DHCP Server IPv6 Address
        /// </summary>
        [DataMember]
        public string PrimaryDHCPServerIPv6Address { get; set; }




        /// <summary>
        /// DHCPServer IPAddress
        /// </summary>
        [DataMember]
        public string PrimaryDhcpServerIPAddress { get; set; }


        /// <summary>
        /// The Linux Server IPAddress
        /// </summary>
        [DataMember]
        public string LinuxServerIPAddress { get; set; }

        /// <summary>
        /// DHCPScope IPAddress
        /// </summary>
        [DataMember]
        public string DHCPScopeIPAddress { get; set; }

        /// <summary>
        /// Server HostName
        /// </summary>
        [DataMember]
        public string ServerHostName { get; set; }

        /// <summary>
        /// The Server domain name
        /// </summary>
        [DataMember]
        public string DomainName { get; set; }

        /// <summary>
        /// Server DNS PrimaryIP
        /// </summary>
        [DataMember]
        public string ServerDNSPrimaryIPAddress { get; set; }

        /// <summary>
        /// Server DNS Secondary IP
        /// </summary>
        [DataMember]
        public string SecondaryDnsIPAddress { get; set; }

        /// <summary>
        /// Server Router IPAddress
        /// </summary>
        [DataMember]
        public string ServerRouterIPAddress { get; set; }

        /// <summary>
        /// Server DNS Suffix
        /// </summary>
        [DataMember]
        public string ServerDNSSuffix { get; set; }

        /// <summary>
        /// Printer Mac Address
        /// </summary>
        [DataMember]
        public string PrinterMacAddress { get; set; }


        /// <summary>
        /// DHCP IPv6 Scope Address
        /// </summary>
        [DataMember]
        public string DHCPScopeIPv6Address { get; set; }



        /// <summary>
        /// Secondary DHCP Server IPv6 Address
        /// </summary>
        [DataMember]
        public string SecondaryDHCPServerIPv6Address { get; set; }

        /// <summary>
        /// Secondary DHCP Server IPv6 Scope
        /// </summary>
        [DataMember]
        public string SecondaryDHCPServerIPv6Scope { get; set; }

        /// <summary>
        /// Secondary DHCP Server IPv4 Address
        /// </summary>
        [DataMember]
        public string SecondDhcpServerIPAddress { get; set; }

        /// <summary>
        /// Secondary DHCP Server IPv4 Scope
        /// </summary>
        [DataMember]
        public string SecondaryDHCPServerIPv4Scope { get; set; }

        /// <summary>
        /// Gets or sets the connectivity wired or wireless
        /// </summary>
        [DataMember]
        public ConnectivityType PrinterConnectivity { get; set; }

        [DataMember]
        /// <summary>
		/// Gets or sets the sitemap path
		/// </summary>
        public string SitemapPath { get; set; } = string.Empty;

        /// <summary>
        /// Sitemaps Version number
        /// </summary>
        [DataMember]
        public string SitemapsVersion { get; set; }

        /// <summary>
        /// user selected tests
        /// </summary>
        [DataMember]
        public Collection<int> SelectedTests { get; set; }

        /// <summary>
        /// Gets or sets session ID
        /// </summary>
        [IgnoreDataMember]
        public string SessionId { get; set; }

        /// <summary>
        /// Represents Router Vlan Identifier
        /// </summary>
        [DataMember]
        public int RouterVirtualLanId { get; set; }

        /// <summary>
        /// Represents Router IP Address
        /// </summary>
        [DataMember]
        public string RouterIPv4Address { get; set; }

        /// <summary>
        /// Represents Router IPv6 Addresses
        /// </summary>
        [DataMember]
        public Collection<string> RouterIPv6Addresses { get; set; }

        /// <summary>
        /// Represents Virtual LAN Identifier and IP Address details.
        /// </summary>
        [DataMember]
        public Dictionary<int, string> VirtualLanDetails { get; set; }



        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="IPConfigurationActivityData"/> class.
        /// </summary>
        public IPConfigurationActivityData()
        {
            ProductFamily = string.Empty;
            ProductName = string.Empty;
            WiredIPv4Address = string.Empty;
            WirelessIPv4Address = string.Empty;
            WirelessMacAddress = string.Empty;
            SwitchIP = string.Empty;
            SsidName = string.Empty;
            SitemapsVersion = string.Empty;
            PrimaryDhcpServerIPAddress = string.Empty;
            SecondaryDnsIPAddress = string.Empty;
            LinuxServerIPAddress = string.Empty;

            SelectedTests = new Collection<int>();


            PrimaryDHCPServerIPv6Address = string.Empty;
            SecondaryDHCPServerIPv6Address = string.Empty;

            RouterVirtualLanId = 0;
            RouterIPv4Address = string.Empty;
            RouterIPv6Addresses = new Collection<string>();
            VirtualLanDetails = new Dictionary<int, string>();


        }

        #endregion
    }
}
