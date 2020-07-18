using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;

namespace HP.ScalableTest.PluginSupport.Connectivity.Printer
{
    /// <summary>
    /// Represents the IPv6 Configuration Method of Printer
    /// </summary>
    public enum IPv6ConfigMethod
    {
        /// <summary>
        /// Link Local Configuration Method
        /// </summary>
        [EnumValue("Link-Local")]
        LinkLocal,
        /// <summary>
        /// Router Configuration Method
        /// </summary>
        [EnumValue("Router")]
        Router,
        /// <summary>
        /// DHCP IPv6 Server Configuration Method
        /// </summary>
        [EnumValue("DHCPv6")]
        DHCPv6
    }

    public class IPv6Details
    {
        /// <summary>
        /// IPv6 Address of Printer
        /// </summary>
        public IPAddress IPv6Address { get; set; }

        /// <summary>
        /// Prefix Length
        /// </summary>
        public int PrefixLength { get; set; }

        /// <summary>
        /// <see cref=" IPv6ConfigMethod"/>
        /// </summary>
        public IPv6ConfigMethod ConfigMethod { get; set; }

        /// <summary>
        /// Valid Lifetime
        /// </summary>
        public TimeSpan ValidLifetime { get; set; }

        /// <summary>
        /// Preferred Lifetime
        /// </summary>
        public TimeSpan PreferredLifetime { get; set; }

    }

    public static class IPv6Utils
    {
        /// <summary>
        /// Get LinkLocal IP Address
        /// </summary>
        /// <param name="ipv6Details">Collection of<see cref=" IPv6Details"/></param>
        /// <returns>LinkLocal IP Address if found, IPAddress.IPv6None otherwise</returns>
        public static IPAddress GetLinkLocalAddress(Collection<IPv6Details> ipv6Details)
        {
            Collection<IPAddress> address = GetAddress(ipv6Details, IPv6ConfigMethod.LinkLocal);

            if (0 == address.Count)
            {
                return IPAddress.IPv6None;
            }
            else
            {
                return address[0];
            }
        }

        /// <summary>
        /// Get Stateful IP Address
        /// </summary>
        /// <param name="ipv6Details">Collection of<see cref=" IPv6Details"/></param>
        /// <returns>Stateful IP Address if found, IPAddress.IPv6None otherwise</returns>
        public static IPAddress GetStatefulAddress(Collection<IPv6Details> ipv6Details)
        {
            Collection<IPAddress> address = GetAddress(ipv6Details, IPv6ConfigMethod.DHCPv6);

            if (0 == address.Count)
            {
                return IPAddress.IPv6None;
            }
            else
            {
                return address[0];
            }
        }

        /// <summary>
        /// Get Collection of Stateless IP Addresses
        /// </summary>
        /// <param name="ipv6Details">Collection of<see cref=" IPv6Details"/></param>
        /// <returns>Collection of IPv6 Stateless IP Addresses, Collection of IPAddress.IPv6None otherwise</returns>
        public static Collection<IPAddress> GetStatelessIPAddress(Collection<IPv6Details> ipv6Details)
        {
            return GetAddress(ipv6Details, IPv6ConfigMethod.Router);
        }

        /// <summary>
        /// Get Valid Lifetime for specified IPv6 Address
        /// </summary>
        /// <param name="ipv6Details">Collection of<see cref=" IPv6Details"/></param>
        /// <param name="ipv6Address">IP Address for which lifetime needs to be fetched</param>
        /// <returns>Valid Lifetime in TimeSpan format</returns>
        public static TimeSpan GetValidLifetime(Collection<IPv6Details> ipv6Details, IPAddress ipv6Address)
        {
            return GetLifeTime(ipv6Details, ipv6Address);
        }

        /// <summary>
        /// Get Preferred Lifetime for specified IPv6 Address
        /// </summary>
        /// <param name="ipv6Details">Collection of<see cref=" IPv6Details"/></param>
        /// <param name="ipv6Address">IP Address for which lifetime needs to be fetched</param>
        /// <returns>Preferred Lifetime in TimeSpan format</returns>
        public static TimeSpan GetPreferredLifetime(Collection<IPv6Details> ipv6Details, IPAddress ipv6Address)
        {
            return GetLifeTime(ipv6Details, ipv6Address, true);
        }

        #region Private Functions		

        /// <summary>
        /// Get IPv6 Address for the requested Config method
        /// </summary>
        /// <param name="ipv6Details">IPv6Details table</param>
        /// <param name="configMethod"><see cref=" IPv6ConfigMethod"/></param>
        /// <returns>IPv6 Address if found</returns>
        private static Collection<IPAddress> GetAddress(Collection<IPv6Details> ipv6Details, IPv6ConfigMethod configMethod)
        {
            Collection<IPAddress> address = new Collection<IPAddress>();

            if (0 == ipv6Details.Count)
            {
                Framework.Logger.LogDebug("IPv6 table is null");
                return address;
            }

            address = new Collection<IPAddress>(ipv6Details.Where(x => x.ConfigMethod.Equals(configMethod)).Select(y => y.IPv6Address).ToList());

            return address;
        }

        /// <summary>
        /// Lifetime of IPv6 Address
        /// </summary>
        /// <param name="ipv6Details">IPv6Details table</param>
        /// <param name="address">IPv6 Address</param>
        /// <param name="isPreferredLifetime">true to get Preferred lifetime, false for Valid lifetime</param>
        /// <returns>Lifetime for IPv6 Address</returns>
        private static TimeSpan GetLifeTime(Collection<IPv6Details> ipv6Details, IPAddress address, bool isPreferredLifetime = false)
        {
            TimeSpan lifetime = new TimeSpan();

            if (0 == ipv6Details.Count || IPAddress.IPv6None == address)
            {
                Framework.Logger.LogDebug("IPv6 table is null or provided IPv6 Address is null");
                return lifetime;
            }

            IPv6Details ipv6Detail = ipv6Details.Where(x => x.IPv6Address.Equals(address)).FirstOrDefault();
            lifetime = isPreferredLifetime == true ? ipv6Detail.PreferredLifetime : ipv6Detail.ValidLifetime;

            return lifetime;
        }

        #endregion
    }
}
