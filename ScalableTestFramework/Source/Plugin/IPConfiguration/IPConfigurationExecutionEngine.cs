using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Dhcp;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.NetworkSwitch;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Router;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;
using System.IO;

namespace HP.ScalableTest.Plugin.IPConfiguration
{
    /// <summary>
    /// Execution control for the NetworkDiscovery plug-in.
    /// </summary>
    public class IPConfigurationExecutionEngine : IPluginExecutionEngine
    {
        #region Constructor

        #endregion

        #region Local Varible

        private const string ROUTER_IP_FORMAT = "{0}.1";
        private const string ROUTER_USERNAME = "STFRouter";
        private const string ROUTER_PASSWORD = "password";

        #endregion

        #region Implementation of IActivityPlugin

        IPConfigurationTests _ipConfigTests;

        #region ProcessActivity

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            CtcSettings.Initialize(executionData);

            // Create Activity Data
            IPConfigurationActivityData activityData = executionData.GetMetadata<IPConfigurationActivityData>(CtcMetadataConverter.Converters);

            // Check Printer Accessibility
            if (!(NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.WiredIPv4Address), TimeSpan.FromSeconds(20))) &&
                  CtcUtility.IsClientConfiguredWithServerIP(activityData.SecondDhcpServerIPAddress, activityData.LinuxServerIPAddress))
            {
                MessageBox.Show(string.Concat("Printer IP Address is not accessible or Client is not configured with Server IP Address.\n\n",
                                                "Make sure you have provided valid Printer IP Address and is accessible.\n",
                                                "Check if Client has acquired IPv4 Address from Secondary DHCP Server.\n",
                                                "Check if Client has acquired IPv4 Address from Linux Server."),
                                @"IP Address Not Accessible", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new PluginExecutionResult(PluginResult.Failed, "Printer IP Address is not accessible or Client is not configured with Server IP Address.");
            }

            // Check Services on DHCP Servers
            if (!IsServicesRunning(activityData.PrimaryDhcpServerIPAddress, activityData.SecondDhcpServerIPAddress))
            {
                return new PluginExecutionResult(PluginResult.Failed, "Services are not running on DHCP server.");
            }

            // Server Configurations

            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            TraceFactory.Logger.Info("Printer : {0}".FormatWith(printer));
            TraceFactory.Logger.Info("printer.MacAddress :{0}".FormatWith(printer.MacAddress));
            activityData.PrinterMacAddress = printer.MacAddress.Replace(":", string.Empty);
            TraceFactory.Logger.Info("PrinterMacAddress : {0}".FormatWith(activityData.PrinterMacAddress));

            DhcpApplicationServiceClient serviceFunctionPrimaryServer = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            activityData.PrimaryDHCPServerIPv6Address = serviceFunctionPrimaryServer.Channel.GetIPv6Address();
            activityData.DHCPScopeIPAddress = serviceFunctionPrimaryServer.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);
            activityData.DHCPScopeIPv6Address = serviceFunctionPrimaryServer.Channel.GetIPv6Scope(activityData.PrimaryDhcpServerIPAddress);

            DhcpApplicationServiceClient serviceFunctionSecondaryServer = DhcpApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress);
            activityData.SecondaryDHCPServerIPv6Address = serviceFunctionSecondaryServer.Channel.GetIPv6Address();
            activityData.SecondaryDHCPServerIPv4Scope = serviceFunctionSecondaryServer.Channel.GetDhcpScopeIP(activityData.SecondDhcpServerIPAddress);
            activityData.SecondaryDHCPServerIPv6Scope = serviceFunctionSecondaryServer.Channel.GetIPv6Scope(activityData.SecondDhcpServerIPAddress);

            ServerConfiguration(ref activityData);
            DhcpApplicationServiceClient serviceFunction = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

            activityData.ServerDNSPrimaryIPAddress = serviceFunction.Channel.GetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);
            activityData.SecondaryDnsIPAddress = serviceFunction.Channel.GetSecondaryDnsServer(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);
            activityData.ServerHostName = serviceFunction.Channel.GetHostName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);
            activityData.DomainName = serviceFunction.Channel.GetDomainName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);
            activityData.ServerRouterIPAddress = serviceFunction.Channel.GetRouterAddress(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);
            activityData.ServerDNSSuffix = serviceFunction.Channel.GetDnsSuffix(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);

            string ipv6Scope = serviceFunction.Channel.GetIPv6Scope(activityData.PrimaryDhcpServerIPAddress);
            if (string.IsNullOrEmpty(ipv6Scope))
            {
                MessageBox.Show(string.Concat("Unable to fetch IPv6 Scope \n\n",
                                                "Check whether DHCP IPv6 Scope is configured on DHCP server."),
                                @"IPv6 Scope Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new PluginExecutionResult(PluginResult.Failed, "Check whether DHCP IPv6 Scope is configured on DHCP server.");
            }

            activityData.DHCPScopeIPv6Address = serviceFunction.Channel.GetIPv6Scope(activityData.PrimaryDhcpServerIPAddress);

            // Fetch and Validate the Switch VLAN details.
            GetVlanDetails(ref activityData);

            if (3 != activityData.VirtualLanDetails.Count)
            {
                MessageBox.Show(string.Concat("Unable to fetch Switch VLAN details\n\n",
                                                "Check whether Printer is connected to Network Switch.\n",
                                                "Switch should be configured with 3 network virtual LAN."),
                                @"Network switch not found", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new PluginExecutionResult(PluginResult.Failed, "Unable to fetch Switch VLAN details. Check whether Printer is connected to Network Switch. Switch should be configured with 3 network virtual LAN.");
            }

            // Fetching Router Details
            activityData.RouterIPv4Address = ROUTER_IP_FORMAT.FormatWith(activityData.WiredIPv4Address.Substring(0, activityData.WiredIPv4Address.LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase)));
            var router = RouterFactory.Create(IPAddress.Parse(activityData.RouterIPv4Address), ROUTER_USERNAME, ROUTER_PASSWORD);
            Dictionary<int, IPAddress> routerVlans = router.GetAvailableVirtualLans();
            activityData.RouterVirtualLanId = routerVlans.FirstOrDefault(x => (null != x.Value) && (x.Value.IsInSameSubnet(IPAddress.Parse(activityData.RouterIPv4Address)))).Key;

            RouterVirtualLAN routerVlan = router.GetVirtualLanDetails(activityData.RouterVirtualLanId);
            Collection<IPAddress> routerIPv6Addresses = router.GetIPv6Addresses(routerVlan.IPv6Details);
            activityData.RouterIPv6Addresses = new Collection<string>(routerIPv6Addresses.Select(x => x.ToString()).ToList());

            // Add Source IP Address
            CtcUtility.AddSourceIPAddress(activityData.PrimaryDhcpServerIPAddress, activityData.LinuxServerIPAddress);

            // Create Instance of EWS Wrapper
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            EwsWrapper.Instance().Create(family, activityData.ProductName, activityData.WiredIPv4Address, Path.Combine(activityData.SitemapPath, activityData.SitemapsVersion), BrowserModel.Firefox);
			EwsWrapper.Instance().Start();
			EwsWrapper.Instance().WakeUpPrinter();
			EwsWrapper.Instance().SetAdvancedOptions();
			EwsWrapper.Instance().SetDefaultIPType(DefaultIPType.AutoIP);
            EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();

            // Create Instance of Telnet Wrapper
            TelnetWrapper.Instance().Create(activityData.WiredIPv4Address);

            // Create Instance of SNMP Wrapper
            SnmpWrapper.Instance().Create(activityData.WiredIPv4Address);

            // Delete Reservation is not validated to handle cases where reservation is not present
            serviceFunction.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.WiredIPv4Address, activityData.PrinterMacAddress);

            if (serviceFunction.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.WiredIPv4Address, activityData.PrinterMacAddress, ReservationType.Both))
            {
                TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP: Succeeded");
            }
            else
            {
                TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP: Failed");
                return new PluginExecutionResult(PluginResult.Failed, "Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP: Failed");
            }

            TraceFactory.Logger.Info("The Server Configured Value retrieved from the DHCP Server are as follows:");
            TraceFactory.Logger.Info("MacAddress:{0}, DNSPrimaryIP:{1}, HostName:{2}, RouterIP:{3}, DNSSuffix:{4}".FormatWith(activityData.PrinterMacAddress, activityData.ServerDNSPrimaryIPAddress,
                                     activityData.ServerHostName, activityData.ServerRouterIPAddress, activityData.ServerDNSSuffix));

            // assign the session id to activity id
            activityData.SessionId = executionData.SessionId;

            if (null == _ipConfigTests)
            {
                _ipConfigTests = new IPConfigurationTests(activityData);
            }

            foreach (int testNumber in activityData.SelectedTests)
            {
                try
                {
                    ExecutionServices.SessionRuntime.AsInternal().WaitIfPaused();
                    _ipConfigTests.RunTest(executionData, testNumber, IPAddress.Parse(activityData.WiredIPv4Address), (ProductFamilies)Enum.Parse(typeof(ProductFamilies), activityData.ProductFamily));
                }
                catch (Exception generalException)
                {
                    TraceFactory.Logger.Info("Test {0} failed with error: {1}".FormatWith(testNumber, generalException.Message));
                }
            }

            EwsWrapper.Instance().Stop();

            return new PluginExecutionResult(PluginResult.Passed);
        }

        #endregion

        #endregion

        #region Private Methods		

        #region IsServiceRunning

        /// <summary>
        /// Check whether IP Config Service and DHCP service is up and running.
        /// </summary>
        /// <returns>true if both the services are running, false otherwise</returns>
        private static bool IsServicesRunning(string primaryServer, string secondaryServer)
        {
            // TODO: Check if WindowsServerService is running
            DhcpApplicationServiceClient primaryService = DhcpApplicationServiceClient.Create(primaryServer);
            DhcpApplicationServiceClient secondaryService = DhcpApplicationServiceClient.Create(secondaryServer);

            if (!(primaryService.Channel.IsDhcpServiceRunning() && secondaryService.Channel.IsDhcpServiceRunning()))
            {
                MessageBox.Show(string.Concat("WindowsServerService is not running on DHCP server.\n\n",
                                                    "Check whether DHCP and WindowsServerService are running on DHCP server.\n",
                                                    "Start 'DHCPServer' and 'WindowsServerService' services if not started on both Primary: {0} and Secondary: {1} server.\n".FormatWith(primaryServer, secondaryServer),
                                                    "Check if Time Zone and Date & Time are in sync between DHCP server and Client."),
                    @"DHCP/ IP Configuration Service Not Running", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        #endregion

        #region GetVlanDetails

        /// <summary>
        /// Gets the vlan details from the switch ignoring the VLANs with no IP Addresses.
        /// </summary>
        /// <param name="activityData">a<see cref="IPConfigurationActivityData"/></param>
        private static void GetVlanDetails(ref IPConfigurationActivityData activityData)
        {
            activityData.VirtualLanDetails.Clear();

            try
            {
                IPConfigurationActivityData activity = activityData;
                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIP));
                TraceFactory.Logger.Info("switch IP : {0}".FormatWith(activityData.SwitchIP));

                // Fetch the vlan details for primary DHCP Server, second DHCP Server and Linux Server
                List<VirtualLAN> vlans = networkSwitch.GetAvailableVirtualLans().Where
                                        (item => (null != item.IPAddress) && (!string.IsNullOrEmpty(activity.LinuxServerIPAddress) && item.IPAddress.IsInSameSubnet(IPAddress.Parse(activity.LinuxServerIPAddress))
                                         || !string.IsNullOrEmpty(activity.SecondDhcpServerIPAddress) && item.IPAddress.IsInSameSubnet(IPAddress.Parse(activity.SecondDhcpServerIPAddress))
                                         || !string.IsNullOrEmpty(activity.PrimaryDhcpServerIPAddress) && item.IPAddress.IsInSameSubnet(IPAddress.Parse(activity.PrimaryDhcpServerIPAddress)))).ToList();

                foreach (VirtualLAN vlan in vlans)
                {
                    activityData.VirtualLanDetails.Add(vlan.Identifier, vlan.IPAddress.ToString());
                }
            }
            catch (FormatException)
            {
                // Do nothing
            }
            catch (Exception)
            {
                // Do Nothing
            }
        }

        #endregion

        #region ServerConfiguration

        private static void ServerConfiguration(ref IPConfigurationActivityData activityData)
        {

            // Configuring the Primary DHCP Server

            string primaryVLAN = (activityData.PrimaryDhcpServerIPAddress).Split(new char[] { '.' })[2];
            string defaultDomainName = $"ctc{primaryVLAN}.automation.com";
            string defaultDNSSuffix = $"ctc{primaryVLAN}.automation.com";
            string defaultHostName = $"ctc{primaryVLAN}HostName";
            string defaultWINSDNSv4Address = $"{activityData.PrimaryDhcpServerIPAddress} {activityData.SecondDhcpServerIPAddress}";
            string defaultWINSDNSv6Address = $"{activityData.PrimaryDHCPServerIPv6Address} {activityData.SecondaryDHCPServerIPv6Address}";

            TraceFactory.Logger.Info($"{primaryVLAN} {defaultDomainName} {defaultDNSSuffix} {defaultHostName} {defaultWINSDNSv4Address} {defaultWINSDNSv6Address}");


            DhcpApplicationServiceClient serviceMethodPrimaryServer = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

            serviceMethodPrimaryServer.Channel.SetDnsServer(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, defaultWINSDNSv4Address);
            serviceMethodPrimaryServer.Channel.SetWinsServer(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, defaultWINSDNSv4Address);
            serviceMethodPrimaryServer.Channel.SetHostName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, defaultHostName);
            serviceMethodPrimaryServer.Channel.SetDomainName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, defaultDomainName);
            serviceMethodPrimaryServer.Channel.SetDnsSuffix(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, defaultDNSSuffix);

            serviceMethodPrimaryServer.Channel.SetDnsv6ServerIP(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, defaultWINSDNSv6Address);
            serviceMethodPrimaryServer.Channel.SetDomainSearchList(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, defaultDNSSuffix);

            // Configuring the Secondary DHCP Server

            string secondaryVLAN = (activityData.SecondDhcpServerIPAddress).Split(new char[] { '.' })[2];
            defaultDomainName = $"ctc{secondaryVLAN}.automation.com";
            defaultDNSSuffix = $"ctc{secondaryVLAN}.automation.com";
            defaultHostName = $"ctc{secondaryVLAN}HostName";
            defaultWINSDNSv4Address = $"{activityData.SecondDhcpServerIPAddress} {activityData.PrimaryDhcpServerIPAddress}";
            defaultWINSDNSv6Address = $"{activityData.SecondaryDHCPServerIPv6Address} {activityData.PrimaryDHCPServerIPv6Address}";

            TraceFactory.Logger.Info($"{secondaryVLAN} {defaultDomainName} {defaultDNSSuffix} {defaultHostName} {defaultWINSDNSv4Address} {defaultWINSDNSv6Address}");

            DhcpApplicationServiceClient serviceMethodSecondaryServer = DhcpApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress);

            serviceMethodSecondaryServer.Channel.SetDnsServer(activityData.SecondDhcpServerIPAddress, activityData.SecondaryDHCPServerIPv4Scope, defaultWINSDNSv4Address);
            serviceMethodSecondaryServer.Channel.SetWinsServer(activityData.SecondDhcpServerIPAddress, activityData.SecondaryDHCPServerIPv4Scope, defaultWINSDNSv4Address);
            serviceMethodSecondaryServer.Channel.SetHostName(activityData.SecondDhcpServerIPAddress, activityData.SecondaryDHCPServerIPv4Scope, defaultHostName);
            serviceMethodSecondaryServer.Channel.SetDomainName(activityData.SecondDhcpServerIPAddress, activityData.SecondaryDHCPServerIPv4Scope, defaultDomainName);
            serviceMethodSecondaryServer.Channel.SetDnsSuffix(activityData.SecondDhcpServerIPAddress, activityData.SecondaryDHCPServerIPv4Scope, defaultDNSSuffix);

            serviceMethodSecondaryServer.Channel.SetDnsv6ServerIP(activityData.SecondDhcpServerIPAddress, activityData.SecondaryDHCPServerIPv6Scope, defaultWINSDNSv6Address);
            serviceMethodSecondaryServer.Channel.SetDomainSearchList(activityData.SecondDhcpServerIPAddress, activityData.SecondaryDHCPServerIPv6Scope, defaultDNSSuffix);

        }
        #endregion

        #endregion
    }
}
