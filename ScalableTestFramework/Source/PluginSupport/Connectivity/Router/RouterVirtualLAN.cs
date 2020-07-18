using System.Collections.ObjectModel;
using System.Net;

namespace HP.ScalableTest.PluginSupport.Connectivity.Router
{
    /// <summary>
    /// Class representing the router virtual LAN details
    /// </summary>
    public class RouterVirtualLAN
    {
        /// <summary>
        /// Gets or sets the name of the router virtual LAN.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets name of the router virtual LAN.
        /// </summary>
        public int Identifier { get; set; }

        /// <summary>
        /// Gets or sets the IPv4 address of the router virtual LAN.
        /// </summary>
        public IPAddress IPv4Address { get; set; }

        /// <summary>
        /// Gets or sets the config method of the router virtual LAN.
        /// </summary>
        public string ConfigMethod { get; set; }

        /// <summary>
        /// Gets or sets the subnet amsk of the router virtual LAN.
        /// </summary>
        public IPAddress SubnetMask { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="RouterIPv6Details"/> configured for the virtual LAN.
        /// </summary>
        public Collection<RouterIPv6Details> IPv6Details { get; internal set; }

        /// <summary>
        /// Gets or sets the link local address configured for the router virtual LAN.
        /// </summary>
        public IPAddress LinkLocalAddress { get; set; }

        /// <summary>
        /// Gets or sets the IPv4 helper (server) address configured for the router virtual LAN.
        /// </summary>
        public IPAddress IPv4HelperAddress { get; set; }

        /// <summary>
        /// Gets or sets the IPv6 helper (server) address configured for the router virtual LAN.
        /// </summary>
        public IPAddress IPv6HelperAddress { get; set; }

        /// <summary>
        /// Gets or sets the Ipv6 status for the router virtual LAN.
        /// </summary>
        public bool IPv6Status { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="RouterVirtualLAN"/>.
        /// </summary>
        public RouterVirtualLAN()
        {
            IPv6Details = new Collection<RouterIPv6Details>();
        }
    }

    /// <summary>
    /// Struct representing the IPv6 details.
    /// </summary>
    public struct RouterIPv6Details
    {
        /// <summary>
        /// IPv6 Address
        /// </summary>
        public IPAddress IPv6Address { get; set; }

        /// <summary>
        /// The Address prefix
        /// </summary>
        public IPAddress AddressPrefix { get; set; }

        /// <summary>
        /// Prefix length
        /// </summary>
        public int PrefixLength { get; set; }

        /// <summary>
        /// Valid life time
        /// </summary>
        public int ValidLeaseTime { get; set; }

        /// <summary>
        /// Preferred life time
        /// </summary>
        public int PreferredLeaseTime { get; set; }

        /// <summary>
        /// Are these instances equal to each other?
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <returns>true if they're equal.</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Gets a hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Are these instances equal?
        /// </summary>
        /// <param name="objectA">First object</param>
        /// <param name="objectB">Second object</param>
        /// <returns>true if they are equal.</returns>
        public static bool operator ==(RouterIPv6Details objectA, RouterIPv6Details objectB)
        {
            return objectA.Equals(objectB);
        }

        /// <summary>
        /// Are these instances not equal?
        /// </summary>
        /// <param name="objectA">First object</param>
        /// <param name="objectB">Second object</param>
        /// <returns>true if they are equal.</returns>
        public static bool operator !=(RouterIPv6Details objectA, RouterIPv6Details objectB)
        {
            return !objectA.Equals(objectB);
        }
    }
}
