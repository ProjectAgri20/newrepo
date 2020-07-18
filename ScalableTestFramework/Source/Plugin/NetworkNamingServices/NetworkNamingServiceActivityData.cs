using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.NetworkNamingServices
{
    /// <summary>
    /// Network Naming Service Activity Data
    /// </summary>
    [DataContract]
    public class NetworkNamingServiceActivityData
    {
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
        /// IP Address of the second printer.
        /// </summary>
        [DataMember]
        public string SecondPrinterIPAddress { get; set; }

        [DataMember]
        /// <summary>
		/// Gets or sets the sitemap path
		/// </summary>
        public string SitemapPath { get; set; } = string.Empty;

        /// <summary>
        /// Sitemaps Version
        /// </summary>
        [DataMember]
        public string SiteMapVersion { get; set; }

        /// <summary>
        /// Selected Tests
        /// </summary>
        [DataMember]
        public Collection<int> SelectedTests { get; set; }

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
        /// DHCPServer IPAddress
        /// </summary>
        [DataMember]
        public string PrimaryDhcpServerIPAddress { get; set; }

        /// <summary>
        /// The Second DHCPServer IPAddress
        /// </summary>
        [DataMember]
        public string SecondDhcpServerIPAddress { get; set; }

        /// <summary>
        /// Primary DHCP v6 scope address
        /// </summary>
        [DataMember]
        public string PrimaryDhcpIPv6Address { get; set; }

        /// <summary>
        /// Secondary DHCP v6 scope address
        /// </summary>
        [DataMember]
        public string SecondaryDhcpIPv6Address { get; set; }

        /// <summary>
        /// The Linux Server IPAddress
        /// </summary>
        [DataMember]
        public string LinuxServerIPAddress { get; set; }

        /// <summary>
        /// Server HostName
        /// </summary>
        [DataMember]
        public string PrimaryDhcpServerHostName { get; set; }

        /// <summary>
        /// The Server domain name
        /// </summary>
        [DataMember]
        public string PrimaryDhcpDomainName { get; set; }

        /// <summary>
        /// Printer Mac Address
        /// </summary>
        [DataMember]
        public string PrinterMacAddress { get; set; }

        /// <summary>
        /// Switch IP Address
        /// </summary>
        [DataMember]
        public string SwitchIpAddress { get; set; }

        /// <summary>
        /// Switch port Number
        /// </summary>
        [DataMember]
        public int PortNumber { get; set; }

        /// <summary>
        /// Represents Virtual LAN Identifier and IP Address details.
        /// </summary>
        [DataMember]
        public Dictionary<int, string> VirtualLanDetails { get; set; }

        /// <summary>
        /// Gets or sets session ID
        /// </summary>
        [IgnoreDataMember]
        public string SessionId { get; set; }

        /// <summary>
        /// Constructor for <see cref=" NetworkNamingServiceActivityData"/>
        /// </summary>
        public NetworkNamingServiceActivityData()
        {
            ProductFamily = string.Empty;
            ProductName = string.Empty;
            WiredIPv4Address = string.Empty;
            WirelessIPv4Address = string.Empty;
            SecondPrinterIPAddress = string.Empty;
            SiteMapVersion = string.Empty;
            PrimaryDhcpServerIPAddress = string.Empty;
            SecondDhcpServerIPAddress = string.Empty;
            PrimaryDhcpIPv6Address = string.Empty;
            SecondaryDhcpIPv6Address = string.Empty;
            LinuxServerIPAddress = string.Empty;
            SwitchIpAddress = string.Empty;
            SelectedTests = new Collection<int>();
            VirtualLanDetails = new Dictionary<int, string>();
        }
    }
}
