using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows.Forms;

using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.HardwareAutomation.NetworkSwitch;
using HP.ScalableTest.HardwareAutomation.Router;
using HP.ScalableTest.Network;
using HP.ScalableTest.Network.Dhcp;
using HP.ScalableTest.Plugin.CtcBase;
using HP.ScalableTest.Printer;
using HP.ScalableTest.Web;

namespace HP.ScalableTest.Plugin.IPConfiguration
{
    /// <summary>
    /// Execution control for the NetworkDiscovery plug-in.
    /// </summary>
    public partial class IPConfigurationExecutionEngine : IPluginExecutionEngine
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the NetworkDiscoveryExecutionControl class.
		/// </summary>        
		public IPConfigurationExecutionEngine()
		{

        }

		#endregion

		#region Local Varible

		private const string ROUTER_IP_FORMAT = "{0}.1";
		private const string ROUTER_USERNAME = "STFRouter";
		private const string ROUTER_PASSWORD = "password";

		#endregion

		#region Implementation of IActivityPlugin

		IPConfigurationTests _ipConfigTests = null;

        #region ProcessActivity

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            // create activity data
            IPConfigurationActivityData activityData = executionData.GetMetadata<IPConfigurationActivityData>();

            // Check if printer is accessible

            if (!(NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.PrimaryWiredIPv4Address), TimeSpan.FromSeconds(20))) &&
                  Utility.IsClientConfiguredWithServerIP(activityData.SecondDhcpServerIPAddress, activityData.LinuxServerIPAddress))
            {
                MessageBox.Show(string.Concat("Printer IP Address is not accessible or Client is not configured with Server IP Address.\n\n",
                                                "Make sure you have provided valid Printer IP Address and is accessible.\n",
                                                "Check if Client has acquired IPv4 Address from Secondary DHCP Server.\n",
                                                "Check if Client has acquired IPv4 Address from Linux Server."),
                                "IP Address Not Accessible", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new PluginExecutionResult(PluginResult.Failed, "Printer IP Address is not accessible or Client is not configured with Server IP Address.");
            }

            // Check if the required services are running on DHCP server
            if (!IsServicesRunning(activityData.PrimaryDhcpServerIPAddress, activityData.SecondDhcpServerIPAddress))
            {
                return new PluginExecutionResult(PluginResult.Failed, "Services are not running on DHCP server.");
            }

            // create instance of ews adapter
            EwsWrapper.Instance().Create(activityData.ProductFamily, activityData.ProductName, activityData.PrimaryWiredIPv4Address, activityData.SitemapsVersion,
                                         BrowserModel.Firefox, EwsAdapterType.WebDriverAdapter);

            EwsWrapper.Instance().Start();


            EwsWrapper.Instance().WakeUpPrinter();
            EwsWrapper.Instance().SetAdvancedOptions();

            IPAddress secondaryAddress = null;

            if (IPAddress.TryParse(activityData.SecondaryWiredIPv4Address, out secondaryAddress))
            {
                EwsWrapper.Instance().ChangeDeviceAddress(secondaryAddress);
                EwsWrapper.Instance().SetAdvancedOptions();
            }

            if (IPAddress.TryParse(activityData.WirelessIPv4Address, out secondaryAddress))
            {
                EwsWrapper.Instance().ChangeDeviceAddress(secondaryAddress);
                EwsWrapper.Instance().SetAdvancedOptions();
            }

            EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);

            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);

            try
            {
                activityData.PrimaryMacAddress = printer.MacAddress.Replace(":", string.Empty);
            }
            catch
            { }

            if (string.IsNullOrEmpty(activityData.PrimaryMacAddress))
            {
                TraceFactory.Logger.Info("Could not get the mac address for Secondary wired interface.");
                return new PluginExecutionResult(PluginResult.Failed, "Could not get the mac address for Secondary wired interface.");
            }

            if(activityData.PrinterInterfaceType == CtcBase.Controls.InterfaceType.Single)
            {
                if (IPAddress.TryParse(activityData.SecondaryWiredIPv4Address, out secondaryAddress))
                {
                    try
                    {
                        printer = PrinterFactory.Create(activityData.ProductFamily, activityData.SecondaryWiredIPv4Address);
                        activityData.SecondaryMacAddress = printer.MacAddress;
                    }
                    catch { }

                    if(string.IsNullOrEmpty(activityData.SecondaryMacAddress))
                    {
                        TraceFactory.Logger.Info("Could not get the mac address for Secondary wired interface.");
                        return new PluginExecutionResult(PluginResult.Failed, "Could not get the mac address for Secondary wired interface.");
                    }
                }

                if (IPAddress.TryParse(activityData.WirelessIPv4Address, out secondaryAddress))
                {
                    try
                    {
                        printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WirelessIPv4Address);
                        activityData.WirelessMacAddress = printer.MacAddress;
                    }catch { }

                    if (string.IsNullOrEmpty(activityData.WirelessMacAddress))
                    {
                        TraceFactory.Logger.Info("Could not get the mac address for Secondary wired interface.");
                        return new PluginExecutionResult(PluginResult.Failed, "Could not get the mac address for wireless interface.");
                    }
                }
            }

            DhcpApplicationServiceClient serviceFunction = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

            activityData.DHCPScopeIPAddress = serviceFunction.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);
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
                                "IPv6 Scope Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new PluginExecutionResult(PluginResult.Failed, "Check whether DHCP IPv6 Scope is configured on DHCP server.");
            }

            activityData.DHCPScopeIPv6Address = serviceFunction.Channel.GetIPv6Scope(activityData.PrimaryDhcpServerIPAddress);

            // Fetch the VLAN details.
            GetVlanDetails(ref activityData);

            // Check if Switch VLAN details are fetched
            if (3 != activityData.VirtualLanDetails.Count)
            {
                MessageBox.Show(string.Concat("Unable to fetch Switch VLAN details\n\n",
                                                "Check whether Printer is connected to Network Switch.\n",
                                                "Switch should be configured with 3 network virtual LAN."),
                                "Network switch not found", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new PluginExecutionResult(PluginResult.Failed, "Unable to fetch Switch VLAN details. Check whether Printer is connected to Network Switch. Switch should be configured with 3 network virtual LAN.");
            }

            // Router Details: Address, Id, IPv6 Addresses
            IRouter router = null;
            activityData.RouterAddress = ROUTER_IP_FORMAT.FormatWith(activityData.PrimaryWiredIPv4Address.Substring(0, activityData.PrimaryWiredIPv4Address.LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase)));
            router = RouterFactory.Create(IPAddress.Parse(activityData.RouterAddress), ROUTER_USERNAME, ROUTER_PASSWORD);

            Dictionary<int, IPAddress> routerVlans = router.GetAvailableVirtualLans();
            activityData.RouterVlanId = routerVlans.Where(x => (null != x.Value) && (x.Value.IsInSameSubnet(IPAddress.Parse(activityData.RouterAddress)))).FirstOrDefault().Key;

            RouterVirtualLAN routerVlan = router.GetVirtualLanDetails(activityData.RouterVlanId);
            Collection<IPAddress> routerIPv6Addresses = router.GetIPv6Addresses(routerVlan.IPv6Details);

            activityData.RouterIPv6Addresses = new Collection<string>(routerIPv6Addresses.Select(x => x.ToString()).ToList());

            // Add source IP Address
            Utility.AddSourceIPAddress(activityData.PrimaryDhcpServerIPAddress, activityData.LinuxServerIPAddress);

            //create instance of SNMP wrapper
            SnmpWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);

            //create instance of Telnet wrapper
            TelnetWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);

           if(!ManageReservation(activityData))
            {
                return new PluginExecutionResult(PluginResult.Failed, "Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP: Failed");
            }

            TraceFactory.Logger.Info("The Server Configured Value retrieved from the DHCP Server are as follows:");
            TraceFactory.Logger.Info("MacAddress:{0}, DNSPrimaryIP:{1}, HostName:{2}, RouterIP:{3}, DNSSuffix:{4}".FormatWith(activityData.PrimaryMacAddress, activityData.ServerDNSPrimaryIPAddress,
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
                    ApplicationFlowControl.Instance.CheckWait(LogPauseState, LogResumeState);
                    _ipConfigTests.RunTest(executionData, testNumber, activityData.PrimaryWiredIPv4Address, (ProductFamilies)Enum.Parse(typeof(ProductFamilies), activityData.ProductFamily));
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

        /// <summary>
        /// Executes LogPauseState to pause execution
        /// </summary>
        private void LogPauseState()
        {
            TraceFactory.Logger.Info("Execution is paused !!");
        }

        /// <summary>
		/// Executes LogResumeState to resume execution
		/// </summary>
		private void LogResumeState()
        {
            TraceFactory.Logger.Info("Execution is resumed..");
        }

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
					"DHCP/ IP Configuration Service Not Running", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private bool ManageReservation(IPConfigurationActivityData activityData)
        {
            if(!CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress))
            {
                return false;
            }

            if(activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.VEP.ToString()) && activityData.PrinterInterfaceType == CtcBase.Controls.InterfaceType.Single)
            {
                IPAddress address = null;
                if (IPAddress.TryParse(activityData.SecondaryWiredIPv4Address, out address))
                {
                    TraceFactory.Logger.Info("Creating reservation for Secondary wired interface.");
                    if (!CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.SecondaryWiredIPv4Address, activityData.SecondaryMacAddress))
                    {
                        return false;
                    }
                }

                if (IPAddress.TryParse(activityData.WirelessIPv4Address, out address))
                {
                    TraceFactory.Logger.Info("Creating reservation for Secondary wired interface.");
                    if (!CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.WirelessIPv4Address, activityData.WirelessMacAddress))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool CreateReservation(string dhcpServerAddress, string scope, string ipAddress, string macAddress)
        {
            DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(dhcpServerAddress);

            // Delete Reservation is not validated to handle cases where reservation is not present
            dhcpClient.Channel.DeleteReservation(dhcpServerAddress, scope, ipAddress, macAddress);

            if (dhcpClient.Channel.CreateReservation(dhcpServerAddress, scope, ipAddress, macAddress, ReservationType.Both))
            {
                TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP for IP Address: {0} Mac Address: {1}: Succeeded".FormatWith(ipAddress, macAddress));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP: Failed");
                return false;
            }
        }

        #endregion
    }
}
