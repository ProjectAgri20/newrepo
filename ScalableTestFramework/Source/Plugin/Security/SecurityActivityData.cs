using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.Security
{
    /// <summary>
    /// Contains data needed to execute a Security activity.
    /// </summary>
    [DataContract]
    public class SecurityActivityData
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
        /// Gets or sets the port number where the wired interface is connected.
        /// </summary>
        [DataMember]
        public int WiredPortNo { get; set; }

        /// <summary>
        /// Gets or sets the mac address of the wired interface
        /// </summary>
        [DataMember]
        public string WiredMacAddress { get; set; }

        /// <summary>
        /// The wireless IPv4 Address
        /// </summary>
        [DataMember]
        public string WirelessIPv4Address { get; set; }

        /// <summary>
        /// Gets or sets the port number where the Europa wired interface is connected.
        /// </summary>
        [DataMember]
        public int WirelessPortNo { get; set; }

        /// <summary>
        /// The Europa Wired Address
        /// </summary>
        [DataMember]
        public string SecondaryWiredIPv4Address { get; set; }

        /// <summary>
        /// Gets or sets the port number where the Europa wired interface is connected.
        /// </summary>
        [DataMember]
        public int SecondaryWiredPortNo { get; set; }

        /// <summary>
        /// Gets or sets the mac address of the Europa wired interface
        /// </summary>
        [DataMember]
        public string SecondaryWiredMacAddress { get; set; }

        /// <summary>
        /// Primary DHCPServer IPAddress
        /// </summary>
        [DataMember]
        public string PrimaryDhcpServerIPAddress { get; set; }

        /// <summary>
        /// The Secondary DHCPServer IPAddress
        /// </summary>
        [DataMember]
        public string SecondaryDhcpServerIPAddress { get; set; }

        /// <summary>
        /// The third DHCPServer IPAddress
        /// </summary>
        [DataMember]
        public string ThirdDhcpServerIPAddress { get; set; }

        /// <summary>
        /// The wireless mac address
        /// </summary>
        [DataMember]
        public string WirelessMacAddress { get; set; }

        /// <summary>
        /// The SSID to of wireless access point
        /// </summary>
        [DataMember]
        public string Ssid { get; set; }

        /// <summary>
        /// The Kerberos Server IPAddress
        /// </summary>
        [DataMember]
        public string KerberosServerIPAddress { get; set; }

        #region Switch Properties
        /// <summary>
        /// Switch IP
        /// </summary>
        [DataMember]
        public string SwitchIPAddress { get; set; }

        /// <summary>
        /// Port No
        /// </summary>
        [DataMember]
        public int PortNo { get; set; }

        #endregion

        #region Sitemaps

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

        #endregion

        #region PrintDriverModel Properties

        /// <summary>
        /// Gets or sets the print driver location
        /// </summary>
        [DataMember]
        public string PrintDriverLocation { get; set; }

        /// <summary>
        /// Print Driver Model
        /// </summary>
        [DataMember]
        public string PrintDriverModel { get; set; }

        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityActivityData"/> class.
        /// </summary>
        public SecurityActivityData()
        {
            ProductFamily = string.Empty;
            ProductName = string.Empty;
            WiredIPv4Address = string.Empty;
            WiredPortNo = 1;
            WiredMacAddress = string.Empty;
            WirelessIPv4Address = string.Empty;
            WirelessPortNo = 1;
            WirelessMacAddress = string.Empty;
            SwitchIPAddress = string.Empty;
            PrimaryDhcpServerIPAddress = string.Empty;
            SecondaryDhcpServerIPAddress = string.Empty;
            ThirdDhcpServerIPAddress = string.Empty;
            SecondaryWiredIPv4Address = string.Empty;
            SecondaryWiredPortNo = 1;
            SecondaryWiredMacAddress = string.Empty;
            WirelessIPv4Address = string.Empty;
            PrintDriverModel = string.Empty;
            PrintDriverLocation = string.Empty;
            Ssid = string.Empty;
            WirelessMacAddress = string.Empty;
            SelectedTests = new Collection<int>();
        }

        #endregion
    }
}