using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.ComplexPrint
{
    /// <summary>
    /// Contains data needed to execute a Print activity.
    /// </summary>
    [DataContract]
    internal class ComplexPrintActivityData
    {
        #region Properties

        /// <summary>
        /// Gets or sets the user selected tests
        /// </summary>
        [DataMember]
        public Collection<int> SelectedTests { get; set; }

        /// <summary>
        /// Gets or sets the printer Family.
        /// </summary>        
        [DataMember]
        public ProductFamilies ProductFamily { set; get; }

        /// <summary>
        /// Gets or sets the printer Name
        /// </summary>
        [DataMember]
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the IPv4 Address
        /// </summary>
        [DataMember]
        public string Ipv4Address { get; set; }

        /// <summary>
        /// Gets or sets the connectivity wired or wireless
        /// </summary>
        [DataMember]
        public ConnectivityType PrinterConnectivity { get; set; }

        /// <summary>
        /// Gets or sets the IPv6 Link Local Address
        /// </summary>
        [DataMember]
        public string Ipv6LinkLocalAddress { get; set; }

        /// <summary>
        /// Gets or sets the IPv6 Stateless Address
        /// </summary>
        [DataMember]
        public string Ipv6StatelessAddress { get; set; }

        /// <summary>
        /// Gets or sets the IPv6 Stateless Address
        /// </summary>
        [DataMember]
        public string Ipv6StateFullAddress { get; set; }

        /// <summary>
        /// Gets or sets the print documents path
        /// </summary>
        [DataMember]
        public string DocumentsPath { get; set; }

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

        /// <summary>
        /// Sitemaps Version
        /// </summary>
        [DataMember]
        public string SitemapsVersion { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexPrintActivityData"/> class.
        /// </summary>
        public ComplexPrintActivityData()
        {
            ProductFamily = ProductFamilies.None;
            ProductName = string.Empty;
            Ipv4Address = string.Empty;
            Ipv6LinkLocalAddress = string.Empty;
            Ipv6StatelessAddress = string.Empty;
            Ipv6StateFullAddress = string.Empty;
            DriverPackagePath = string.Empty;
            DriverModel = string.Empty;
            DocumentsPath = string.Empty;
            SitemapsVersion = string.Empty;
            SelectedTests = new Collection<int>();
        }

        #endregion
    }
}