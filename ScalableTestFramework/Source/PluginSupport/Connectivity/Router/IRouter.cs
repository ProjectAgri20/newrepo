using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;

namespace HP.ScalableTest.PluginSupport.Connectivity.Router
{
    /// <summary>
    /// Interface for using the functionalities available for router and router virtual LANs.
    /// </summary>
    public interface IRouter
    {
        /// <summary>
        /// Model of the router.
        /// </summary>
        string Model { get; }

        /// <summary>
        /// Manufacturer name of the router.
        /// </summary>
        string Make { get; }

        /// <summary>
        /// Gets the details of a virtual LAN.
        /// </summary>
        /// <param name="routerVlanId">The virtual LAN id.</param>
        /// <returns><see cref="RouterVirtualLAN"/></returns>
        RouterVirtualLAN GetVirtualLanDetails(int routerVlanId);

        /// <summary>
        /// Gets the identifiers and IP addresses of the available virtual LANs.
        /// </summary>
        /// <returns>returns a dictionary of virtual LAN identifiers and IP Addresses.</returns>
        Dictionary<int, IPAddress> GetAvailableVirtualLans();

        /// <summary>
        /// Enable IPv6 for a particular virtual LAN.
        /// </summary>
        /// <param name="routerVlanId">The virtual LAN id.</param>
        /// <param name="ipv6Addresses">List of addresses to be added for the virtual LAN.</param>
        /// <returns>True if IPv6 is enabled, else false.</returns>
        bool EnableIPv6Address(int routerVlanId, Collection<IPAddress> ipv6Addresses);

        /// <summary>
        /// Disable IPv6 for a particular virtual LAN.
        /// </summary>
        /// <param name="routerVlanId">The virtual LAN id.</param>
        /// <returns>True if IPv6 is disabled, else false.</returns>
        bool DisableIPv6Address(int routerVlanId);

        /// <summary>
        /// Set the Lease time.
        /// </summary>
        /// <param name="routerVlanId">The virtual LAN id.</param>
        /// <param name="validLifeTime"><see cref="LeaseTime"/></param>
        /// <param name="preferredLifeTime">The lease time.</param>
        /// <returns>True if the lease time is set, else false.</returns>
        bool SetLeaseTime(int routerVlanId, TimeSpan validLifeTime, TimeSpan preferredLifeTime);

        /// <summary>
        /// Set the flags for a particular virtual LAN.
        /// </summary>
        /// <param name="routerVlanId">The virtual LAN id.</param>
        /// <param name="routerFlag"><see cref="RouterFlags"/></param>
        /// <returns>True if the flag is set, else false.</returns>
        bool SetRouterFlag(int routerVlanId, RouterFlags routerFlag);

        /// <summary>
        /// Disables both M and O flags for a particular virtual LAN.
        /// </summary>
        /// <param name="routerVlanId">The virtual LAN id.</param>
        /// <returns>True if the flags are disabled, else false.</returns>
        bool DisableRouterFlag(int routerVlanId);

        /// <summary>
        /// Gets the IPv6 addresses from <see cref="RouterIPv6Details"/> collection.
        /// </summary>
        /// <param name="ipv6Details"><see cref="RouterIPv6Details"/></param>
        /// <returns>Collection of IPv6 address.</returns>
        Collection<IPAddress> GetIPv6Addresses(Collection<RouterIPv6Details> ipv6Details);

        /// <summary>
        /// Configure helper address for the specified virtual LAN.
        /// </summary>
        /// <param name="address">The helper address.</param>
        /// <returns>true if the helper address is set, else false.</returns>
        bool ConfigureHelperAddress(int routerVlanId, IPAddress address);

        /// <summary>
        /// Delete helper address for the specified virtual LAN.
        /// </summary>
        /// <param name="helperAddress">The helper address.</param>
        /// <returns>True if the helper address is deleted, else false.</returns>
        bool DeleteHelperAddress(int routerVlanId, IPAddress helperAddress);
    }

    /// <summary>
    /// Represents Router Flags ( M and O) available for the router virtual LANs.
    /// </summary>
    [Flags]
    public enum RouterFlags
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// The M flag.
        /// </summary>
        [EnumValue("ipv6 nd ra managed-config-flag")]
        Managed = 1,

        /// <summary>
        /// The O flag.
        /// </summary>
        [EnumValue("ipv6 nd ra other-config-flag")]
        Other = 2,

        /// <summary>
        /// Both Managed and Other
        /// </summary>
        Both = Managed | Other
    }

    /// <summary>
    /// Represents the lease times available for the router virtual LANs.
    /// </summary>
    public enum LeaseTime
    {
        Preferred,
        Valid
    }
}
