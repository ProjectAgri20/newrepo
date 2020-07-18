using System;

namespace HP.ScalableTest.PluginSupport.Connectivity
{

    /// <summary>
    /// Enumerator for Web Proxy
    /// </summary>
    public enum WebProxyType
    {
        /// <summary>
        /// Automatic Proxy
        /// </summary>
        Auto = 1,
        /// <summary>
        /// Manual Proxy
        /// </summary>
        Manual = 3,
        /// <summary>
        /// CURL Proxy
        /// </summary>
        Curl = 2,
        /// <summary>
        /// No Proxy
        /// </summary>
        Disable = 0
    }

    [Flags]
    public enum WebProxyAuthType
    {
        None = 0,
        Basic = 1,
        Digest = 2,
        Both = Basic | Digest
    }

    /// <summary>
    /// The configuration details required for configuring web proxy on the printer.
    /// </summary>
    public class WebProxyConfigurationDetails
    {
        /// <summary>
        /// Proxy Type.
        /// </summary>
        public WebProxyType ProxyType { get; set; }

        /// <summary>
        /// IP Address of the Web Proxy Server.
        /// </summary>
        public string IPAddress { get; set; } = string.Empty;

        /// <summary>
        /// Port Number of the Web Proxy Server.
        /// </summary>
        public int PortNo { get; set; }

        /// <summary>
        /// Username for Web Proxy Server authentication.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Password for Web Proxy Server authentication.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Authentication Method
        /// </summary>
        public WebProxyAuthType AuthType { get; set; }

        /// <summary>
        /// cURL Path
        /// </summary>
        public string cURL { get; set; } = string.Empty;

        /// <summary>
        /// Overriding the Equals method
        /// </summary>
        public override bool Equals(object obj)
        {
            WebProxyConfigurationDetails other = (WebProxyConfigurationDetails)obj;
            if (other.ProxyType == WebProxyType.Auto)
            {
                return (this.ProxyType == other.ProxyType);
            }
            else if (other.ProxyType == WebProxyType.Manual)
            {
                return (this.ProxyType == other.ProxyType && this.IPAddress == other.IPAddress && this.PortNo == other.PortNo && this.UserName == other.UserName && this.AuthType.HasFlag(other.AuthType));
            }
            else if (other.ProxyType == WebProxyType.Curl)
            {
                return (this.ProxyType == other.ProxyType && this.cURL == other.cURL);
            }
            else
            {
                return (this.ProxyType == other.ProxyType);
            }
        }

        /// <summary>
        /// Overriding the To string method to display settings to the user as logs
        /// </summary>
        public override string ToString()
        {
            return String.Format("Proxy Type: {0} \n" + "IP Address: {1} \n" + "Port No.: {2} \n" + "Username: {3} \n" + "Password: {4} \n" + "Auth Type: {5} \n" + "cURL: {6} \n", ProxyType, IPAddress, PortNo, UserName, Password, AuthType, cURL);
        }
    }
}
