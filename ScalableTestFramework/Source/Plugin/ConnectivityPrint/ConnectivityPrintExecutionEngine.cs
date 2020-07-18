using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Dhcp;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.ConnectivityPrint
{
    /// <summary>
    /// This is a demo execution controller for an Activity Plug-in
    /// </summary>
    public class ConnectivityPrintExecutionEngine : IPluginExecutionEngine
    {
        #region Implementation of IPluginExecution interface

        ConnectivityPrintTests _printTests;
        private const string DHCP_SERVER_IP_FORMAT = "192.168.{0}.254";
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            CtcSettings.Initialize(executionData);

            ConnectivityPrintActivityData activityData = executionData.GetMetadata<ConnectivityPrintActivityData>(CtcMetadataConverter.Converters);
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.Ipv4Address));

            if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), TimeSpan.FromSeconds(10)))
            {
                MessageBox.Show(string.Concat("Printer IPv4 Address is not accessible\n\n",
                                                "IPv4 address: {0}\n".FormatWith(activityData.Ipv4Address)),
                                                @"IPv4 Address Not Accessible", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new PluginExecutionResult(PluginResult.Failed, "Printer IPv4 Address is not accessible");
            }

            // create instance of ews adapter
            EwsWrapper.Instance().Create(family, activityData.ProductName, activityData.Ipv4Address, Path.Combine(activityData.SitemapPath, activityData.SiteMapVersion), BrowserModel.Firefox);
            EwsWrapper.Instance().Start();
            EwsWrapper.Instance().WakeUpPrinter();
            EwsWrapper.Instance().SetAdvancedOptions();
            EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();

            //Enabling IPV6 startup 
            EwsWrapper.Instance().SetDHCPv6OnStartup(true);
            EwsWrapper.Instance().SetIPv6(false);
            EwsWrapper.Instance().SetIPv6(true);

            // If printer is not available, assign default IPAddress
            if (printer.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), 1))
            {
                // Get All Ipv6 Addresses 
                activityData.Ipv6LinkLocalAddress = printer.IPv6LinkLocalAddress?.ToString() ?? string.Empty;
                activityData.Ipv6StateFullAddress = printer.IPv6StateFullAddress?.ToString() ?? string.Empty;
                activityData.Ipv6StatelessAddress = printer.IPv6StatelessAddresses.Count == 0 ? string.Empty : printer.IPv6StatelessAddresses[0].ToString();
            }
            else
            {
                activityData.Ipv6LinkLocalAddress = string.Empty;
                activityData.Ipv6StateFullAddress = string.Empty;
                activityData.Ipv6StatelessAddress = string.Empty;
            }

            foreach (Ipv6AddressTypes addressType in activityData.Ipv6AddressTypes)
            {
                if (Ipv6AddressTypes.LinkLocal == addressType)
                {
                    if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv6LinkLocalAddress), TimeSpan.FromSeconds(10)))
                    {
                        MessageBox.Show(string.Concat("Printer Link Local Address is not accessible\n\n",
                                                        "Link local address: {0}\n".FormatWith(activityData.Ipv6LinkLocalAddress),
                                                        "Check if Stateless and Stateful address are pinging if you have selected it.\n",
                                                        "Stateless: {0}, Stateful: {1}".FormatWith(activityData.Ipv6StatelessAddress, activityData.Ipv6StateFullAddress)),
                                                        @"Link Local Address Not Accessible", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return new PluginExecutionResult(PluginResult.Failed, "Printer Link Local Address is not accessible");
                    }
                }
                else if (Ipv6AddressTypes.Stateless == addressType)
                {
                    if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv6StatelessAddress), TimeSpan.FromSeconds(10)))
                    {
                        MessageBox.Show(string.Concat("Printer Stateless Address is not accessible\n\n",
                                                        "Stateless address: {0}\n".FormatWith(activityData.Ipv6StatelessAddress),
                                                        "Check if Stateful address is pinging if you have selected it.\n",
                                                        "Stateful: {0}".FormatWith(activityData.Ipv6StateFullAddress)),
                                                        @"Stateless Address Not Accessible", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return new PluginExecutionResult(PluginResult.Failed, "Printer Stateless Address is not accessible");
                    }
                }
                else if (Ipv6AddressTypes.Stateful == addressType)
                {
                    if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv6StateFullAddress), TimeSpan.FromSeconds(10)))
                    {
                        MessageBox.Show(string.Concat("Printer Stateful Address is not accessible\n\n",
                                                        "Stateful address: {0}\n".FormatWith(activityData.Ipv6StateFullAddress)),
                                                        @"Stateful Address Not Accessible", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return new PluginExecutionResult(PluginResult.Failed, "Printer Stateful Address is not accessible");
                    }
                }
            }

            //Reservation for Primary Printer
            string value = activityData.Ipv4Address.Split(new char[] { '.' })[2];
            string serverIp = DHCP_SERVER_IP_FORMAT.FormatWith(value);
            TraceFactory.Logger.Info("Server IP : {0}".FormatWith(serverIp));
            //string serverIp = Printer.Printer.GetDHCPServerIP(IPAddress.Parse(activityData.Ipv4Address)).ToString();
            string printerMacAddress = printer.MacAddress.Replace(":", string.Empty);
            using (DhcpApplicationServiceClient client = DhcpApplicationServiceClient.Create(serverIp))
            {
                string scope = client.Channel.GetDhcpScopeIP(serverIp);
                TraceFactory.Logger.Info("Scope : {0}".FormatWith(scope));
                client.Channel.DeleteReservation(serverIp, client.Channel.GetDhcpScopeIP(serverIp), activityData.Ipv4Address, printerMacAddress);

                if (client.Channel.CreateReservation(serverIp, client.Channel.GetDhcpScopeIP(serverIp), activityData.Ipv4Address, printerMacAddress, ReservationType.Both))
                {
                    TraceFactory.Logger.Info("Primary Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP : Succeeded");
                }
                else
                {
                    TraceFactory.Logger.Info("Primary Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP: Failed");
                    return new PluginExecutionResult(PluginResult.Failed, "Primary Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP: Failed");
                }
            }

            EwsWrapper.Instance().SetAdvancedOptions();

            string documentsPath = activityData.DocumentsPath;
            string documentsSharePath = Path.Combine(CtcSettings.ConnectivityShare, activityData.ProductFamily.ToString());

            // Combine the connectivity share path with the selected document
            // In case of Re-run, path is already constructed. Hence do not construct it again.
            if (!Directory.Exists(documentsPath))
            {
                if (!documentsPath.StartsWith(documentsSharePath, StringComparison.CurrentCulture))
                {
                    documentsPath = Path.Combine(documentsSharePath, activityData.DocumentsPath, ConnectivityPrintConfigurationControl.DIRECTORY_DOCUMENTS);

                    activityData.DocumentsPath = documentsPath;
                }
            }

            if (activityData.IsWspTestsSelected)
            {
                printer.NotifyWSPrinter += printer_NotifyWSPAddition;
                if (printer.Install(IPAddress.Parse(activityData.Ipv4Address), Printer.Printer.PrintProtocol.WSP, activityData.DriverPackagePath, activityData.DriverModel))
                {
                    MessageBox.Show(@"WS Printer was added successfully.", @"WS Printer Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(@"WS Printer was not added successfully. All WS Print related tests will fail.", @"WS Printer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (null == _printTests)
            {
                _printTests = new ConnectivityPrintTests(activityData);
            }

            foreach (int testNumber in activityData.SelectedTests)
            {
                ExecutionServices.SessionRuntime.AsInternal().WaitIfPaused();
                _printTests.RunTest(executionData, testNumber, IPAddress.Parse(activityData.Ipv4Address), activityData.ProductFamily);
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }

        /// <summary>
        /// Notify User to Add Web Service Printer
        /// </summary>
        private void printer_NotifyWSPAddition()
        {
            MessageBox.Show(@"Printer driver is installed. Add WS printer and press OK to continue.", @"Add WS Printer", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
        }

        #endregion
    }
}