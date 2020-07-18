using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.WebProxy
{
    /// <summary>
    /// Web Proxy Activity Data
    /// </summary>
    [DataContract]
    public class WebProxyActivityData
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
        /// Wired IPv4 Address
        /// </summary>
        [DataMember]
        public string WiredIPv4Address { get; set; }

        // <summary>
        /// Unsecure Web Proxy IPAddress
        /// </summary>
        [DataMember]
        public string UnsecureWebProxyServerIPAddress { get; set; }

        /// <summary>
        /// Unsecure Web Proxy Host Name
        /// </summary>
        [IgnoreDataMember]
        public string UnsecureWebProxyServerHostName { get; set; }

        // <summary>
        /// Unsecure Web Proxy Port Number
        /// </summary>
        [DataMember]
        public int UnsecureWebProxyServerPortNumber { get; set; }

        // <summary>
        /// Secure Web Proxy IPAddress
        /// </summary>
        [DataMember]
        public string SecureWebProxyServerIPAddress { get; set; }

        /// <summary>
        /// Secure Web Proxy Host Name
        /// </summary>
        [IgnoreDataMember]
        public string SecureWebProxyServerHostName { get; set; }

        /// <summary>
        /// Secure Web Proxy FQDN
        /// </summary>
        [IgnoreDataMember]
        public string SecureWebProxyServerFQDN { get; set; }

        // <summary>
        /// Secure Web Proxy Port Number
        /// </summary>
        [DataMember]
        public int SecureWebProxyServerPortNumber { get; set; }

        // <summary>
        /// Secure Web Proxy Username
        /// </summary>
        [DataMember]
        public string SecureWebProxyServerUsername { get; set; }

        // <summary>
        /// Secure Web Proxy Password
        /// </summary>
        [DataMember]
        public string SecureWebProxyServerPassword { get; set; }

        // <summary>
        /// cURL Path IPAddress
        /// </summary>
        [DataMember]
        public string cURLPathIPAddress { get; set; }

        // <summary>
        /// cURL Path FQDN
        /// </summary>
        [DataMember]
        public string cURLPathFQDN { get; set; }

        // <summary>
        /// WPAD Server IPAddress
        /// </summary>
        [DataMember]
        public string WPADServerIPAddress { get; set; }

        /// <summary>
        /// WPAD Server Host Name
        /// </summary>
        [IgnoreDataMember]
        public string WPADServerHostName { get; set; }

        /// <summary>
        /// DHCP Server IPAddress
        /// </summary>
        [DataMember]
        public string PrimaryDHCPServerIPAddress { get; set; }

        /// <summary>
        /// Second DHCP Server IPAddress
        /// </summary>
        [DataMember]
        public string SecondaryDHCPServerIPAddress { get; set; }

        /// <summary>
        /// DHCP Scope
        /// </summary>
        [DataMember]
        public string PrimaryDHCPScopeIPAddress { get; set; }

        /// <summary>
        /// Switch IP Address
        /// </summary>
        [DataMember]
        public string SwitchIPAddress { get; set; }

        /// <summary>
        /// Switch Port Number
        /// </summary>
        [DataMember]
        public int PortNumber { get; set; }

        /// <summary>
        /// Session ID
        /// </summary>
        [IgnoreDataMember]
        public string SessionId { get; set; }

        /// <summary>
        /// Domain Name
        /// </summary>
        [IgnoreDataMember]
        public string DomainName { get; set; }

        /// <summary>
        /// Constructor for <see cref=" WebProxyActivityData"/>
        /// </summary>
        public WebProxyActivityData()
        {
            ProductFamily = string.Empty;
            ProductName = string.Empty;
            SiteMapVersion = string.Empty;
            WiredIPv4Address = string.Empty;
            UnsecureWebProxyServerIPAddress = string.Empty;
            SecureWebProxyServerIPAddress = string.Empty;
            SecureWebProxyServerUsername = string.Empty;
            SecureWebProxyServerPassword = string.Empty;
            PrimaryDHCPServerIPAddress = string.Empty;
            SecondaryDHCPServerIPAddress = string.Empty;
            cURLPathIPAddress = string.Empty;
            cURLPathFQDN = string.Empty;
            WPADServerIPAddress = string.Empty;
            SwitchIPAddress = string.Empty;
            SelectedTests = new Collection<int>();
        }
    }
}