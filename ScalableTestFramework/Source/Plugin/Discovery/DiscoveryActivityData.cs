using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.Discovery
{
    /// <summary>
    /// Network Discovery Activity Data
    /// </summary>
    [DataContract]
    internal class DiscoveryActivityData
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
        /// Printer MAC Address
        /// </summary>
        [DataMember]
        public string PrinterMacAddress { get; set; }

        /// <summary>
        /// user selected tests
        /// </summary>
        [DataMember]
        public Collection<int> SelectedTests { get; set; }

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
        /// Gets or sets the document path
        /// </summary>
        [DataMember]
        public string DocumentPath { get; set; }


        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoveryActivityData"/> class.
        /// </summary>
        public DiscoveryActivityData()
        {
            ProductFamily = string.Empty;
            ProductName = string.Empty;
            IPv4Address = string.Empty;
            SitemapsVersion = string.Empty;
            PrinterMacAddress = string.Empty;
            SelectedTests = new Collection<int>();
        }

        #endregion
    }
}
