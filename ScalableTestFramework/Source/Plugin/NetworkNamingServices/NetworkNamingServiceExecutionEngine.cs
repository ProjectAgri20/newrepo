using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.XPath;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Dhcp;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.NetworkSwitch;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;
using System.IO;

namespace HP.ScalableTest.Plugin.NetworkNamingServices
{
    /// <summary>
    /// Execution Control for Network Naming Service Plug-in
    /// </summary>
    public partial class NetworkNamingServiceExecutionEngine : IPluginExecutionEngine
    {
        #region Local Variable

        /// <summary>
        /// Initialize <see cref=" NetworkNamingServiceTests"/> instance
        /// </summary>
        private NetworkNamingServiceTests _networkNamingTests = null;

        #endregion

        #region Implementation of IActivityPlugin

        /// <summary>
        /// Process activity defined in Metadata
        /// </summary>
        /// <param name="metadata"></param>
        public void ProcessActivity(IXPathNavigable metadata)
        {

        }

        #endregion

        #region Private Methods

        #region GetVlanDetails

        /// <summary>
		/// Gets the vlan details from the switch ignoring the VLANs with no IP Addresses.
		/// </summary>
		/// <param name="activityData">a<see cref="IPConfigurationActivityData"/></param>
		private static void GetVlanDetails(ref NetworkNamingServiceActivityData activityData)
        {
            activityData.VirtualLanDetails.Clear();

            try
            {
                NetworkNamingServiceActivityData activity = activityData;
                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIpAddress));

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

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            CtcSettings.Initialize(executionData);

            NetworkNamingServiceActivityData activityData = executionData.GetMetadata<NetworkNamingServiceActivityData>(CtcMetadataConverter.Converters);

            #region Scenario Prerequisites

            //Check the Windows Server Service,Packet Capture service and Kiwi Syslog server is up and running            
            CtcUtility.StartService("WindowsServerService", activityData.PrimaryDhcpServerIPAddress);
            CtcUtility.StartService("PacketCaptureService", activityData.PrimaryDhcpServerIPAddress);
            CtcUtility.StartService("Kiwi Syslog Server", activityData.PrimaryDhcpServerIPAddress);

            //Check dns and wins server is up and running on both primary and secondary dhcp server
            CtcUtility.StartService(@"dns", activityData.PrimaryDhcpServerIPAddress);
            CtcUtility.StartService("WINS", activityData.PrimaryDhcpServerIPAddress);
            CtcUtility.StartService(@"dns", activityData.SecondDhcpServerIPAddress);
            CtcUtility.StartService("WINS", activityData.SecondDhcpServerIPAddress);

            //Check the Wireless printer ip is accessible
            if (activityData.SelectedTests.Contains(678968))
            {
                if (!(NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.WirelessIPv4Address), TimeSpan.FromSeconds(20))))
                {
                    MessageBox.Show(string.Concat("Wireless Printer IP Address is not accessible\n\n"),
                                "Wireless IP Address Not Accessible", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new PluginExecutionResult(PluginResult.Failed, "Wireless Printer IP Address is not accessible");
                }
            }

            //Check whether all devices are accesible and the server IP
            CtcUtility.CheckPrinterConnectivity(activityData.WiredIPv4Address, activityData.PrimaryDhcpServerIPAddress,
                                        activityData.SecondPrinterIPAddress, activityData.SecondDhcpServerIPAddress,
                                        activityData.LinuxServerIPAddress, activityData.SwitchIpAddress);

            // Fetch the VLAN details.
            GetVlanDetails(ref activityData);

            // Check if Switch VLAN details are fetched
            if (3 != activityData.VirtualLanDetails.Count)
            {
                MessageBox.Show(string.Concat("Unable to fetch Switch VLAN details\n\n",
                                                "Check whether Printer is connected to Network Switch.\n",
                                                "Switch should be configured with 3 network virtual LAN."),
                                "Network switch not found", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new PluginExecutionResult(PluginResult.Failed, "Unable to fetch Switch VLAN details");
            }

            //Reservation for Primary Printer
            using (DhcpApplicationServiceClient client = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
            {
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                activityData.PrinterMacAddress = printer.MacAddress.Replace(":", string.Empty);
                client.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, client.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress),
                                                 activityData.WiredIPv4Address, activityData.PrinterMacAddress);

                if (client.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, client.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress),
                                                     activityData.WiredIPv4Address, activityData.PrinterMacAddress, ReservationType.Both))
                {
                    TraceFactory.Logger.Info("Primary Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP : Succeeded");
                }
                else
                {
                    TraceFactory.Logger.Info("Primary Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP: Failed");
                    return new PluginExecutionResult(PluginResult.Failed, "Primary Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP: Failed");
                }

                activityData.PrimaryDhcpIPv6Address = client.Channel.GetIPv6Address();
            }

            //Reservation for Secondary Printer[user may give input as primary server ip/secondary server ip ,so getting server ip based on the printer ipaddress]           
            string secondDhcpServerIPAddress = Printer.Printer.GetDHCPServerIP(IPAddress.Parse(activityData.SecondPrinterIPAddress)).ToString();
            using (DhcpApplicationServiceClient client = DhcpApplicationServiceClient.Create(secondDhcpServerIPAddress))
            {
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.SecondPrinterIPAddress);
                string secondPrinterMacAddress = printer.MacAddress.Replace(":", string.Empty);
                client.Channel.DeleteReservation(secondDhcpServerIPAddress, client.Channel.GetDhcpScopeIP(secondDhcpServerIPAddress),
                                                 activityData.SecondPrinterIPAddress, secondPrinterMacAddress);

                if (client.Channel.CreateReservation(secondDhcpServerIPAddress, client.Channel.GetDhcpScopeIP(secondDhcpServerIPAddress),
                                                     activityData.SecondPrinterIPAddress, secondPrinterMacAddress, ReservationType.Both))
                {
                    TraceFactory.Logger.Info("Secondary Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP : Succeeded");
                }
                else
                {
                    TraceFactory.Logger.Info("Secondary Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP: Failed");
                    return new PluginExecutionResult(PluginResult.Failed, "Secondary Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP: Failed");
                }
            }

            using (DhcpApplicationServiceClient client = DhcpApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress))
            {
                activityData.SecondaryDhcpIPv6Address = client.Channel.GetIPv6Address();
            }

            #endregion

            // create instance of ews adapter
            EwsWrapper.Instance().Create(Enum<PrinterFamilies>.Parse(activityData.ProductFamily), activityData.ProductName, activityData.WiredIPv4Address, Path.Combine(activityData.SitemapPath, activityData.SiteMapVersion), BrowserModel.Firefox);

            EwsWrapper.Instance().Start();
            EwsWrapper.Instance().WakeUpPrinter();
            EwsWrapper.Instance().SetAdvancedOptions();

            //create instance of SNMP wrapper
            SnmpWrapper.Instance().Create(activityData.WiredIPv4Address);

            //create instance of Telnet wrapper
            TelnetWrapper.Instance().Create(activityData.WiredIPv4Address);

            //Enabling IPV6 startup 
            EwsWrapper.Instance().SetDHCPv6OnStartup(true);
            EwsWrapper.Instance().SetIPv6(false);
            EwsWrapper.Instance().SetIPv6(true);
            EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();

            if (null == _networkNamingTests)
            {
                _networkNamingTests = new NetworkNamingServiceTests(activityData);
            }

            // assign the session id to activity data
            activityData.SessionId = executionData.SessionId;

            foreach (int testNumber in activityData.SelectedTests)
            {
                try
                {
                    ExecutionServices.SessionRuntime.AsInternal().WaitIfPaused();
                    _networkNamingTests.RunTest(executionData, testNumber, IPAddress.Parse(activityData.WiredIPv4Address), (ProductFamilies)Enum.Parse(typeof(ProductFamilies), activityData.ProductFamily));
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
    }
}
