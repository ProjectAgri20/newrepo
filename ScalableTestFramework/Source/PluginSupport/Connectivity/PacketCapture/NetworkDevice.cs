using System.Net;

namespace HP.ScalableTest.PluginSupport.Connectivity.PacketCapture
{
    /// <summary>
    /// Network device details
    /// </summary>
    public class NetworkDevice
    {
        /// <summary>
        /// Gets or sets network device description
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets IPv4 address
        /// </summary>
        public IPAddress IPv4Address { get; set; }
    }
}
