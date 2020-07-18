using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.IPSecurity
{
    /// <summary>
    /// Contains data needed to execute a IPSecurity activity.
    /// </summary>
    [DataContract]
    public class IPSecurityActivityData
    {
        #region Properties

        #region Printer Properties
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
        /// The Europa Wired Address
        /// </summary>
        [DataMember]
        public string EuropaWiredIPv4Address { get; set; }

        /// <summary>
        /// PrinterMacAddress
        /// </summary>
        [DataMember]
        public string PrinterMacAddress { get; set; }

        /// <summary>
        /// MessageBox CheckBox
        /// </summary>
        [DataMember]
        public bool MessageBoxCheckBox { get; set; }

        /// <summary>
        /// Wired Interface RadioButton
        /// </summary>
        [DataMember]
        public bool WiredInterface { get; set; }

        /// <summary>
        /// Wireless Interface RadioButton
        /// </summary>
        [DataMember]
        public bool WirelessInterface { get; set; }

        /// <summary>
        /// Europa Interface RadioButton
        /// </summary>
        [DataMember]
        public bool EuropaInterface { get; set; }

        /// <summary>
        /// IPv6 Statefull Address
        /// </summary>
        [DataMember]
        public string IPV6StatefullAddress { get; set; }

        /// <summary>
        /// IPV6 Stateless Address
        /// </summary>
        [DataMember]
        public string IPV6StatelessAddress { get; set; }

        /// <summary>
        /// Link Local Address
        /// </summary>
        [DataMember]
        public string LinkLocalAddress { get; set; }

        #endregion

        #region Client Properties
        /// <summary>
        /// The Windows Secondary IP Address
        /// </summary>
        [DataMember]
        public string WindowsSecondaryClientIPAddress { get; set; }

        /// <summary>
        /// The Linux Fedora Client IP Address
        /// </summary>
        [DataMember]
        public string LinuxFedoraClientIPAddress { get; set; }

        #endregion

        #region Server Properties
        /// <summary>
        /// Prmary DHCPServer IPAddress
        /// </summary>
        [DataMember]
        public string PrimaryDhcpServerIPAddress { get; set; }

        /// <summary>
        /// The Secondary DHCPServer IPAddress
        /// </summary>
        [DataMember]
        public string SecondDhcpServerIPAddress { get; set; }

        /// <summary>
        /// The Kerberos Server IPAddress
        /// </summary>
        [DataMember]
        public string KerberosServerIPAddress { get; set; }

        #endregion

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

        /// <summary>
        /// Sets or gets session id
        /// </summary>
        [IgnoreDataMember]
        public string SessionId { get; set; }

        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="IPSecurityActivityData"/> class.
        /// </summary>
        public IPSecurityActivityData()
        {
            ProductFamily = string.Empty;
            ProductName = string.Empty;
            WiredIPv4Address = string.Empty;
            WirelessIPv4Address = string.Empty;
            SwitchIPAddress = string.Empty;
            PrimaryDhcpServerIPAddress = string.Empty;
            SecondDhcpServerIPAddress = string.Empty;
            KerberosServerIPAddress = string.Empty;
            EuropaWiredIPv4Address = string.Empty;
            WindowsSecondaryClientIPAddress = string.Empty;
            LinuxFedoraClientIPAddress = string.Empty;
            PrintDriverModel = string.Empty;
            PrintDriverLocation = string.Empty;
            SelectedTests = new Collection<int>();
        }

        #endregion
    }
}