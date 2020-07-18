using HP.ScalableTest.Framework;
using HP.ScalableTest.PluginSupport.Connectivity.Dhcp;

namespace HP.ScalableTest.PluginSupport.WindowsServerService
{
    internal class DhcpApplication : IDhcpApplication
    {
        public DhcpApplication()
        {
            Logger.LogInfo("DHCP application started...");
        }

        public bool SetDhcpLeaseTime(string serverIP, string scope, long leasetime)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.SetDhcpLeaseTime(serverIP, scope, leasetime);
        }

        public bool CreateReservation(string serverIP, string scope, string reservationIP, string macAddress, ReservationType reservationType)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.CreateReservation(serverIP, scope, reservationIP, macAddress, reservationType);
        }

        public bool DeleteReservation(string serverIP, string scope, string reservationIP, string macAddress)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.DeleteReservation(serverIP, scope, reservationIP, macAddress);
        }

        public bool SetHostName(string serverIP, string scope, string hostname)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.SetHostName(serverIP, scope, hostname);
        }

        public bool DeleteHostName(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.DeleteHostName(serverIP, scope);
        }

        public bool DeleteWinsServer(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.DeleteWinsServer(serverIP, scope);
        }

        public bool SetDomainName(string serverIP, string scope, string domainName)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.SetDomainName(serverIP, scope, domainName);
        }

        public bool SetWPADServer(string serverIP, string scope, string wpadServer)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.SetWPADServer(serverIP, scope, wpadServer);
        }

        public bool SetDnsServer(string serverIP, string scope, params string[] dnsServers)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.SetDnsServer(serverIP, scope, dnsServers);
        }

        public bool DeleteDnsServer(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.DeleteDnsServer(serverIP, scope);
        }

        public bool DeleteWPADServer(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.DeleteWPADServer(serverIP, scope);
        }

        public string GetDhcpScopeIP(string serverIP)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetDhcpScopeIP(serverIP);
        }

        public string GetDnsSuffix(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetDnsSuffix(serverIP, scope);
        }

        public bool SetDnsSuffix(string serverIP, string scope, params string[] dnsSuffices)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.SetDnsSuffix(serverIP, scope, dnsSuffices);
        }

        public string GetConfiguredParameterValue(string serverIP, string scope, int optionID)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetConfiguredParameterValue(serverIP, scope, optionID);
        }

        public string GetDomainName(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetDomainName(serverIP, scope);
        }

        public string GetHostName(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetHostName(serverIP, scope);
        }

        public bool StartDhcpServer()
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.StartDhcpServer();
        }

        public bool StopDhcpServer()
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.StopDhcpServer();
        }

        public string GetIPv6Scope(string serverIP)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetIPv6Scope(serverIP);
        }

        public string GetIPv6Address()
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetIPv6Address();
        }

        public int GetPreferredLifetime(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetPreferredLifetime(serverIP, scope);
        }

        public bool SetPreferredLifetime(string serverIP, string scope, int leaseTime)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.SetPreferredLifetime(serverIP, scope, leaseTime);
        }

        public int GetValidLifetime(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetValidLifetime(serverIP, scope);
        }

        public bool SetValidLifetime(string serverIP, string scope, int leaseTime)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.SetValidLifetime(serverIP, scope, leaseTime);
        }

        public string GetServerName()
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetServerName();
        }

        public string GetPrimaryDnsServer(string dhcpServer, string scopeIPAddress)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetPrimaryDnsServer(dhcpServer, scopeIPAddress);
        }

        public string GetSecondaryDnsServer(string dhcpServer, string scopeIPAddress)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetSecondaryDnsServer(dhcpServer, scopeIPAddress);
        }

        public bool SetBootPHostName(string serverIP, string scope, string hostName)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.SetBootPHostName(serverIP, scope, hostName);
        }

        public bool DeleteDomainName(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.DeleteDomainName(serverIP, scope);
        }

        public string GetPrimaryDnsv6Server(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetPrimaryDnsv6Server(serverIP, scope);
        }

        public string GetSecondaryDnsv6Server(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetSecondaryDnsv6Server(serverIP, scope);
        }

        public string GetDomainSearchList(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetDomainSearchList(serverIP, scope);
        }

        public bool IsDhcpServiceRunning()
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.IsDhcpServiceRunning();
        }

        public string GetRouterAddress(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetRouterAddress(serverIP, scope);
        }

        public string GetPrimaryWinsServer(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetPrimaryWinsServer(serverIP, scope);
        }

        public string GetSecondaryWinsServer(string dhcpServer, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetSecondaryWinsServer(dhcpServer, scope);
        }

        public bool SetWinsServer(string serverIP, string scope, params string[] winsAddress)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.SetWinsServer(serverIP, scope, winsAddress);
        }

        public long GetLeasetime(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.GetLeasetime(serverIP, scope);
        }

        public bool DeleteScope(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.DeleteScope(serverIP, scope);
        }

        public bool CreateScope(string serverIP, string scope, string scopeName, string startIpRange, string endIpRange)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.CreateScope(serverIP, scope, scopeName, startIpRange, endIpRange);
        }

        public bool ChangeServerType(string serverIP, string scope, ReservationType serverType, string exceptionAddress = null, string exceptionMacAddress = null)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.ChangeServerType(serverIP, scope, serverType, exceptionAddress, exceptionMacAddress);
        }

        public bool DeleteBootPHostname(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.DeleteBootPHostname(serverIP, scope);
        }

        public bool SetDomainSearchList(string serverIP, string scope, string domainName)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.SetDomainSearchList(serverIP, scope, domainName);
        }

        public bool DeleteDomainSearchList(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.DeleteDomainSearchList(serverIP, scope);
        }

        public bool SetDnsv6ServerIP(string serverIP, string scope, string dnsServer)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.SetDnsv6ServerIP(serverIP, scope, dnsServer);
        }

        public bool DeleteDnsv6Server(string serverIP, string scope)
        {
            return PluginSupport.Connectivity.Dhcp.DhcpApplication.DeleteDnsv6Server(serverIP, scope);
        }
    }
}
