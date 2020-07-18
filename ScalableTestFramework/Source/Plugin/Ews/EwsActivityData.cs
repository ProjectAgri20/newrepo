using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.Ews
{
    /// <summary>
    /// EWS Activity Data
    /// </summary>
    [DataContract]
    internal class EwsActivityData
    {
        #region Properties

        /// <summary>
        /// Product Name
        /// </summary>
        [DataMember]
        public string ProductName { get; set; }

        /// <summary>
        /// Printer IP
        /// </summary>
        [DataMember]
        public string PrinterIP { get; set; }

        /// <summary>
        /// Product Category
        /// </summary>
        [DataMember]
        public string ProductCategory { get; set; }

        /// <summary>
        /// Browser
        /// </summary>
        [DataMember]
        public string Browser { get; set; }

        /// <summary>
        /// user selected tests
        /// </summary>
        [DataMember]
        public Collection<int> TestNumbers { get; set; }

        /// <summary>
        /// Browser Number
        /// </summary>
        [DataMember]
        public int BrowserNumber { get; set; }

        [DataMember]
        /// <summary>
		/// Gets or sets the sitemap path
		/// </summary>
        public string SitemapPath { get; set; } = string.Empty;

        /// <summary>
        /// Sitemaps Version number
        /// </summary>        
        [DataMember]
        public string SitemapsVersion      { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EwsActivityData"/> class.
        /// </summary>
        public EwsActivityData()
        {
            ProductName = string.Empty;
            PrinterIP = string.Empty;
            ProductCategory = string.Empty;
            Browser = string.Empty;
            SitemapsVersion = string.Empty;
            TestNumbers = new Collection<int>();
            BrowserNumber = 0;
        }

        #endregion
    }
}
