using System.Collections.ObjectModel;
using System.Net;

namespace HP.ScalableTest.PluginSupport.Connectivity.NetworkSwitch
{
    /// <summary>
    /// Structure listing the VirtualLan details
    /// </summary>
    public struct VirtualLAN
    {
        /// <summary>
        /// The VirtualLan Identifier
        /// </summary>
        public int Identifier { get; set; }

        /// <summary>
        /// Name of VirtualLan
        /// </summary>        
        public string Name { get; set; }

        /// <summary>
        /// IP Address of VirtualLan
        /// </summary>        
        public IPAddress IPAddress { get; set; }

        /// <summary>
        /// Configuration Method for VirtualLan
        /// </summary>
        public string ConfigMethod { get; set; }

        /// <summary>
        /// Subnet Mask for VirtualLan
        /// </summary>
        public IPAddress SubnetMask { get; set; }

        /// <summary>
        /// Port Numbers for VirualLan
        /// </summary>
        public Collection<int> ConfiguredPorts { get; set; }
    }
}
