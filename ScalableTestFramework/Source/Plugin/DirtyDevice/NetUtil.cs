using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public class NetUtil
    {
        public static IPAddress GetAddressForLocalHostIpv4()
        {
            var machineName = Dns.GetHostName();
            IPHostEntry hostInfo = Dns.GetHostEntry(machineName);
            var ipAddress = hostInfo.AddressList.FirstOrDefault(addr => addr.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(addr));

            if (ipAddress == null)
            {
                throw new UriFormatException($"Could not determine a valid non-loopback IPv4 address for localhost '{machineName}'. (Addresses considered: {string.Join("; ", hostInfo.AddressList.Select(addr => addr.ToString()))}");
            }

            return ipAddress;
        }

        public static IPAddress GetAddressIpv4(string machineName)
        {
            IPHostEntry hostInfo = Dns.GetHostEntry(machineName);
            var ipAddress = hostInfo.AddressList.FirstOrDefault(addr => addr.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(addr));

            if (ipAddress == null)
            {
                throw new UriFormatException($"Could not determine a valid non-loopback IPv4 address for '{machineName}'. (Addresses considered: {string.Join("; ", hostInfo.AddressList.Select(addr => addr.ToString()))}");
            }

            return ipAddress;
        }

        public static string GetFQDN()
        {
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = Dns.GetHostName();

            domainName = "." + domainName;
            if (!hostName.EndsWith(domainName))  // if hostname does not already include domain name
            {
                hostName += domainName;   // add the domain name part
            }

            return hostName;                    // return the fully qualified name
        }
    }
}
