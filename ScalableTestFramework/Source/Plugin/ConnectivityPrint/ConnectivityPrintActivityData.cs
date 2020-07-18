using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.ConnectivityPrint
{
    #region Enumerations

    /// <summary>
    /// Represents the IPv6 Address Type
    /// </summary>
    [Flags]
    public enum Ipv6AddressTypes
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// The Link Local IPv6 Address
        /// </summary>
        LinkLocal = 1,
        /// <summary>
        /// The Stateless IPv6 Address
        /// </summary>
        Stateless = 2,
        /// <summary>
        /// The stateful IPv6 Address
        /// </summary>
        Stateful = 4
    }

    /// <summary>
    /// Represents the folder types used for print
    /// </summary>
    enum FolderType
    {
        [EnumValue("SimpleFiles")]
        SimpleFiles,
        [EnumValue("ContinousFiles")]
        ContinousFiles,
        [EnumValue("CancelFiles")]
        CancelFiles,
        [EnumValue("HoseBreakFiles")]
        HoseBreakFiles,
        [EnumValue("PauseFiles")]
        PauseFiles,
        [EnumValue("FTPSimpleFiles")]
        FTPSimpleFiles,
        [EnumValue("FTPContinousFiles")]
        FTPContinousFiles,
        [EnumValue("FTPCancelFiles")]
        FTPCancelFiles,
        [EnumValue("FTPHoseBreakFiles")]
        FTPHoseBreakFiles,
        [EnumValue("CancelFromFrontPanel")]
        CancelFromFrontPanel
    }

    #endregion

    /// <summary>
    /// Contains data needed to execute a Print activity.
    /// </summary>
    [DataContract]
    public class ConnectivityPrintActivityData
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
        /// <value>
        /// The name.
        /// </value>
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
        /// Gets or sets the Ipv6 Address type
        /// </summary>
        [DataMember]
        public Collection<Ipv6AddressTypes> Ipv6AddressTypes { get; set; }

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
        /// Gets or sets IP Address of the switch
        /// </summary>
        [DataMember]
        public string SwitchIPAddress { get; set; }

        /// <summary>
        /// Gets or sets the switch port number
        /// </summary>
        [DataMember]
        public int SwitchPortNumber { get; set; }

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
        /// Gets or sets the additional information about the tests like duration
        /// </summary>
        [DataMember]
        public Collection<PrintTestData> TestDetails { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that the WSP protocol tests are selected.
        /// This is a calculated field and will be set in the validating event of the ConnectivityPrintEditControl.
        /// User need not set the value while saving the activity data.
        /// </summary>
        [DataMember]
        public bool IsWspTestsSelected { get; set; }

        [DataMember]
        /// <summary>
		/// Gets or sets the sitemap path
		/// </summary>
        public string SitemapPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or Sets Sitemap Version
        /// </summary>
        [DataMember]
        public string SiteMapVersion { get; set; }

        /// <summary>
        /// Gets or sets the PaperlessMode
        /// </summary>
		[DataMember]
        public bool PaperlessMode { get; set; }


        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectivityPrintActivityData"/> class.
        /// </summary>
        public ConnectivityPrintActivityData()
        {
            ProductFamily = ProductFamilies.None;
            ProductName = string.Empty;
            Ipv4Address = string.Empty;
            SwitchIPAddress = string.Empty;
            Ipv6LinkLocalAddress = string.Empty;
            Ipv6StatelessAddress = string.Empty;
            Ipv6StateFullAddress = string.Empty;
            DriverPackagePath = string.Empty;
            DriverModel = string.Empty;
            DocumentsPath = string.Empty;
            SelectedTests = new Collection<int>();
            TestDetails = new Collection<PrintTestData>();
            Ipv6AddressTypes = new Collection<Ipv6AddressTypes>();
            SiteMapVersion = string.Empty;
        }

        #endregion
    }
}