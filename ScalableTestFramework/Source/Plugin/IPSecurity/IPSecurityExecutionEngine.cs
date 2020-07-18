using System;
using System.Net;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Dhcp;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using HP.ScalableTest.Utility;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;
using System.IO;

namespace HP.ScalableTest.Plugin.IPSecurity
{
    public partial class IPSecurityExecutionEngine : IPluginExecutionEngine
    {
        #region Local Variables

        IPSecurityTests _ipSecuritytests = null;

        #endregion

        #region Implementation of IActivityPlugin

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            CtcSettings.Initialize(executionData);

            // create activity data
            IPSecurityActivityData activityData = executionData.GetMetadata<IPSecurityActivityData>(CtcMetadataConverter.Converters);

            #region Scenario Prerequisites

            // TODO: Need to enable broadcast and multicast option in failsafe
            // TODO: Cleaning up the rules on client side and on the printer


            //Check the Windows Server Service,Packet Capture service and Kiwi Sys log server is up and running            
            CtcUtility.StartService("WindowsServerService", activityData.PrimaryDhcpServerIPAddress);
            CtcUtility.StartService("PacketCaptureService", activityData.PrimaryDhcpServerIPAddress);

            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            //Reservation for Primary Printer
            using (DhcpApplicationServiceClient client = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
            {
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
            }

            #endregion

			// create instance of EWS adapter
			EwsWrapper.Instance().Create(Enum<PrinterFamilies>.Parse(activityData.ProductFamily), activityData.ProductName, activityData.WiredIPv4Address, Path.Combine(activityData.SitemapPath, activityData.SitemapsVersion), BrowserModel.Firefox, EwsAdapterType.WebDriverAdapter);

            EwsWrapper.Instance().Start();
            EwsWrapper.Instance().WakeUpPrinter();

            //create instance of SNMP wrapper
            SnmpWrapper.Instance().Create(activityData.WiredIPv4Address);

            //create instance of Telnet wrapper
            TelnetWrapper.Instance().Create(activityData.WiredIPv4Address);

            //IPv6 and Dhcpv6 on startup should be enabled to get IPv6 addresses. Applicable only for VEP
            if(PrinterFamilies.VEP.ToString().EqualsIgnoreCase(activityData.ProductFamily) ||
                PrinterFamilies.LFP.ToString().EqualsIgnoreCase(activityData.ProductFamily))
            {
                EwsWrapper.Instance().SetDHCPv6(true);
                EwsWrapper.Instance().SetDHCPv6OnStartup(true);
                EwsWrapper.Instance().SetIPv6(false);
                EwsWrapper.Instance().SetIPv6(true);
                EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();
            }

            TraceFactory.Logger.Info("Collecting Ipv6 addresses of the printer");
            activityData.IPV6StatefullAddress = printer.IPv6StateFullAddress.ToString();
            activityData.IPV6StatelessAddress = printer.IPv6StatelessAddresses[0].ToString();
            activityData.LinkLocalAddress = printer.IPv6LinkLocalAddress.ToString();

            // assign session id to activity data
            activityData.SessionId = executionData.SessionId;

            if (null == _ipSecuritytests)
            {
                _ipSecuritytests = new IPSecurityTests(activityData);
            }

            // Execute the selected tests
            foreach (int testNumber in activityData.SelectedTests)
            {
                try
                {
                    ExecutionServices.SessionRuntime.AsInternal().WaitIfPaused();
                    _ipSecuritytests.RunTest(executionData, testNumber, IPAddress.Parse(activityData.WiredIPv4Address), (ProductFamilies)Enum.Parse(typeof(ProductFamilies), activityData.ProductFamily));
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Fatal("Error while executing Test:{0} \n".FormatWith(testNumber, ex.Message));
                    continue;
                }
            }

            EwsWrapper.Instance().Stop();

            return new PluginExecutionResult(PluginResult.Passed);
        }

        #endregion
    }
}
