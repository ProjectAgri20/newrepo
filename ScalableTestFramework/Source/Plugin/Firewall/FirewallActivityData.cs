using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.Firewall
{
    /// <summary>
    /// Firewall Activity Data
    /// </summary> 
    [DataContract]
    internal class FirewallActivityData
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
        /// IPv4 Address
        /// </summary>
        [DataMember]
        public string IPv4Address { get; set; }

        /// <summary>
        /// IPv6 Link Local Address
        /// </summary>
        [DataMember]
        public string IPv6LinkLocalAddress { get; set; }

        /// <summary>
        /// IPv6 Stateless Address
        /// </summary>
        [DataMember]
        public string IPv6StatelessAddress { get; set; }

        /// <summary>
        /// IPv6 Stateful Address
        /// </summary>
        [DataMember]
        public string IPv6StatefulAddress { get; set; }

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
        /// User Selected tests
        /// </summary>
        [DataMember]
        public Collection<int> SelectedTests { get; set; }

        /// <summary>
        /// Print Driver Location
        /// </summary>
        [DataMember]
        public string PrintDriver { get; set; }

        /// <summary>
        /// Printer Driver Model
        /// </summary>
        [DataMember]
        public string PrintDriverModel { get; set; }

        /// <summary>
        /// Gets or sets the connectivity wired or wireless
        /// </summary>
        [DataMember]
        public ConnectivityType PrinterConnectivity { get; set; }

        /// <summary>
        /// The wireless MAC Address
        /// </summary>
        [DataMember]
        public string WiredMacAddress { get; set; }

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
        /// IPv4 enable/disabled status
        /// </summary>
        [DataMember]
        public bool IPv4Enable { get; set; }

        /// <summary>
        /// Gets or sets Link local option
        /// </summary>
        [DataMember]
        public bool LinkLocal { get; set; }

        /// <summary>
        /// Gets or sets stateful option
        /// </summary>
        [DataMember]
        public bool Stateful { get; set; }

        /// <summary>
        /// Gets or Sets Stateless option
        /// </summary>
        [DataMember]
        public bool Stateless { get; set; }


        /// <summary>
        /// Gets or sets the debug flag
        /// </summary>
        [DataMember]
        public bool Debug { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FirewallActivityData"/> class.
        /// </summary>
        public FirewallActivityData()
        {
            ProductFamily = string.Empty;
            ProductName = string.Empty;
            IPv4Address = string.Empty;
            SitemapsVersion = string.Empty;
            SelectedTests = new Collection<int>();
            PrintDriver = string.Empty;
            PrintDriverModel = string.Empty;
            IPv6LinkLocalAddress = string.Empty;
            IPv6StatefulAddress = string.Empty;
            IPv6StatelessAddress = string.Empty;
            WiredMacAddress = string.Empty;
            SwitchIP = string.Empty;
            SsidName = string.Empty;
        }

        #endregion Constructor
    }
}
