using System;
using HP.ScalableTest.PluginSupport.Connectivity.SystemConfiguration;

namespace HP.ScalableTest.PluginSupport.WindowsServerService
{
    internal class SystemConfiguration : ISystemConfiguration
    {
        public bool SetSystemTime(DateTime dateTime, TimeSpan? resetTime = null)
        {
            return PluginSupport.Connectivity.SystemConfiguration.SystemConfiguration.SetSystemTime(dateTime, resetTime);
        }

        public DateTime GetSystemTime()
        {
            return PluginSupport.Connectivity.SystemConfiguration.SystemConfiguration.GetSystemTime();
        }


        public bool SetStaticIPAddress(string networkName, System.Net.IPAddress ipAddress, System.Net.IPAddress subnetMask, System.Net.IPAddress gateWay = null)
        {
            return PluginSupport.Connectivity.SystemConfiguration.SystemConfiguration.SetStaticIPAddress(networkName, ipAddress, subnetMask, gateWay);
        }

        public bool SetDhcpIPAddress(string networkName)
        {
            return PluginSupport.Connectivity.SystemConfiguration.SystemConfiguration.SetDhcpIPAddress(networkName);
        }

        public string GetFullyQualifiedname()
        {
            return PluginSupport.Connectivity.SystemConfiguration.SystemConfiguration.GetFullyQualifiedname();
        }

        public string GetHostName()
        {
            return PluginSupport.Connectivity.SystemConfiguration.SystemConfiguration.GetHostName();
        }
    }
}
