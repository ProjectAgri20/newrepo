using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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

namespace HP.ScalableTest.Plugin.Security
{
    public class SecurityExecutionEngine : IPluginExecutionEngine
    {
        #region Local Variables

        SecurityTests _securitytests;

        #endregion

        #region Implementation of IPluginExecutionEngine

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            CtcSettings.Initialize(executionData);

            // create activity data
            SecurityActivityData activityData = executionData.GetMetadata<SecurityActivityData>(CtcMetadataConverter.Converters);

            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            activityData.WiredMacAddress = printer.MacAddress;

            TraceFactory.Logger.Info("Wired Mac Address: {0}".FormatWith(activityData.WiredMacAddress));

            if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.VEP.ToString()))
            {
                IPAddress ipAddress;

                if (IPAddress.TryParse(activityData.SecondaryWiredIPv4Address, out ipAddress))
                {
                    printer = PrinterFactory.Create(activityData.ProductFamily, activityData.SecondaryWiredIPv4Address);
                    activityData.SecondaryWiredMacAddress = printer.MacAddress;
                    TraceFactory.Logger.Info("Secondary Wired Mac Address: {0}".FormatWith(activityData.SecondaryWiredMacAddress));
                }

                if (IPAddress.TryParse(activityData.WirelessIPv4Address, out ipAddress))
                {
                    printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WirelessIPv4Address);
                    activityData.WirelessMacAddress = printer.MacAddress;

                    TraceFactory.Logger.Info("Wireless Mac Address: {0}".FormatWith(activityData.WirelessMacAddress));
                }
            }

            List<NetworkInterface> clientNetworks = NetworkInterface.GetAllNetworkInterfaces().Where(n => n.OperationalStatus == OperationalStatus.Up).ToList();
            List<IPAddress> localAddresses = NetworkUtil.GetLocalAddresses().Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToList();

            EwsWrapper.Instance().Create(Enum<PrinterFamilies>.Parse(activityData.ProductFamily), activityData.ProductName, activityData.WiredIPv4Address, Path.Combine(activityData.SitemapPath, activityData.SitemapsVersion), BrowserModel.Firefox);
            EwsWrapper.Instance().Start();
            EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();

            if (!SecurityScenarioPrerequisites(activityData, clientNetworks, localAddresses))
            {
                return new PluginExecutionResult(PluginResult.Failed, "Failed in Scenario pre-requisites");
            }
            //create instance of SNMP wrapper
            SnmpWrapper.Instance().Create(activityData.WiredIPv4Address);

            //create instance of Telnet wrapper
            TelnetWrapper.Instance().Create(activityData.WiredIPv4Address);

            if (null == _securitytests)
            {
                _securitytests = new SecurityTests(activityData);
            }

            // Execute the selected tests
            foreach (int testNumber in activityData.SelectedTests)
            {
                try
                {
                    ExecutionServices.SessionRuntime.AsInternal().WaitIfPaused();
                    _securitytests.RunTest(executionData, testNumber, IPAddress.Parse(activityData.WiredIPv4Address), (ProductFamilies)Enum.Parse(typeof(ProductFamilies), activityData.ProductFamily));
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Fatal("Error while executing Test:{0} \n".FormatWith(testNumber, ex.Message));
                }
            }

            EwsWrapper.Instance().Stop();
            ManageClientReservation(clientNetworks, localAddresses, activityData, true);

            return new PluginExecutionResult(PluginResult.Passed);
        }

        private bool SecurityScenarioPrerequisites(SecurityActivityData activityData, List<NetworkInterface> clientNetworks, List<IPAddress> localAddresses)
        {
            bool continueTest = true;

            while (continueTest && !NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.WiredIPv4Address), TimeSpan.FromSeconds(30)))
            {
                continueTest = SecurityTemplates.ShowErrorPopUp("Printer: {0} is not available.\nPlease cold reset the printer.".FormatWith(activityData.WiredIPv4Address));
            }

            if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.VEP.ToString()))
            {
                IPAddress ipAddress;

                if (IPAddress.TryParse(activityData.SecondaryWiredIPv4Address, out ipAddress))
                {
                    if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.SecondaryWiredIPv4Address), TimeSpan.FromSeconds(10)))
                    {
                        SecurityTemplates.ShowErrorPopUp("The IP address: {0} is not available.\nPlease cold reset the printer.".FormatWith(activityData.SecondaryWiredIPv4Address));
                    }

                    //Utility.StartService(activityData.SecondaryDhcpServerIPAddress, "WindowsServerService");

                    CreateReservation(activityData.SecondaryWiredIPv4Address, activityData.SecondaryDhcpServerIPAddress, activityData.SecondaryWiredMacAddress);

                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.SecondaryWiredIPv4Address);
                    EwsWrapper.Instance().Start();
                    EwsWrapper.Instance().SetAdvancedOptions();
                }

                if (IPAddress.TryParse(activityData.WirelessIPv4Address, out ipAddress))
                {
                    if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.WirelessIPv4Address), TimeSpan.FromSeconds(10)))
                    {
                        SecurityTemplates.ShowErrorPopUp("The IP address: {0} is not available.\nPlease cold reset the printer.".FormatWith(activityData.WirelessIPv4Address));
                    }

                    //Utility.StartService(activityData.ThirdDhcpServerIPAddress, "WindowsServerService");

                    CreateReservation(activityData.WirelessIPv4Address, activityData.ThirdDhcpServerIPAddress, activityData.WirelessMacAddress);

                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.WirelessIPv4Address);
                    EwsWrapper.Instance().SetAdvancedOptions();
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
            }

            if (!CtcUtility.IsClientConfiguredWithServerIP(activityData.SecondaryDhcpServerIPAddress))
            {
                SecurityTemplates.ShowErrorPopUp("Check if Client has acquired IPv4 Address from Secondary DHCP Server.");
            }

            //Utility.StartService(activityData.PrimaryDhcpServerIPAddress, "WindowsServerService");

            ManageClientReservation(clientNetworks, localAddresses, activityData);

            EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
            EwsWrapper.Instance().SetAdvancedOptions();

            CreateReservation(activityData.WiredIPv4Address, activityData.PrimaryDhcpServerIPAddress, activityData.WiredMacAddress);

            return true;
        }

        #endregion

        #region Private Methods

        private bool CreateReservation(string ipAddress, string dhcpServer, string macAddress)
        {
            if (!string.IsNullOrEmpty(ipAddress))
            {
                using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(dhcpServer))
                {
                    string scope = dhcpClient.Channel.GetDhcpScopeIP(dhcpServer);

                    dhcpClient.Channel.DeleteReservation(dhcpServer, scope, ipAddress, macAddress);

                    if (dhcpClient.Channel.CreateReservation(dhcpServer, scope, ipAddress, macAddress, ReservationType.Both))
                    {
                        TraceFactory.Logger.Info("Successfully created reservation for IP address: {0}, Mac address: {1} for  both dhcp and bootp in server: {2}".FormatWith(ipAddress, macAddress, dhcpServer));
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to create reservation for IP address: {0}, Mac address: {1} for  both dhcp and bootp in server: {2}".FormatWith(ipAddress, macAddress, dhcpServer));
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        private bool DeleteReservation(string ipAddress, string dhcpServer, string macAddress)
        {
            if (!string.IsNullOrEmpty(ipAddress))
            {
                using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(dhcpServer))
                {
                    string scope = dhcpClient.Channel.GetDhcpScopeIP(dhcpServer);

                    if (dhcpClient.Channel.DeleteReservation(dhcpServer, scope, ipAddress, macAddress))
                    {
                        TraceFactory.Logger.Info("Successfully created reservation for IP address: {0}, Mac address: {1} for  both dhcp and bootp in server: {2}".FormatWith(ipAddress, macAddress, dhcpServer));
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to create reservation for IP address: {0}, Mac address: {1} for  both dhcp and bootp in server: {2}".FormatWith(ipAddress, macAddress, dhcpServer));
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        private bool ManageClientReservation(List<NetworkInterface> clientNetworks, List<IPAddress> clientAddresses, SecurityActivityData activityData, bool deleteReservation = false)
        {
            try
            {
                IPAddress primaryAddress = clientAddresses.FirstOrDefault(x => x.IsInSameSubnet(IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress)));
                IPAddress secondaryAddress = clientAddresses.FirstOrDefault(x => x.IsInSameSubnet(IPAddress.Parse(activityData.SecondaryDhcpServerIPAddress)));

                clientNetworks.ForEach(x => TraceFactory.Logger.Info("network" + x.Name));
                clientAddresses.ForEach(x => TraceFactory.Logger.Info("Address" + x.ToString()));

                string primaryNetworkName = CtcUtility.GetClientNetworkName(clientAddresses.FirstOrDefault(x => x.Equals(primaryAddress)).ToString());
                string secondaryNetworkName = CtcUtility.GetClientNetworkName(clientAddresses.FirstOrDefault(x => x.Equals(secondaryAddress)).ToString());

                if (deleteReservation)
                {
                    DeleteReservation(primaryAddress.ToString(), activityData.PrimaryDhcpServerIPAddress, clientNetworks.FirstOrDefault(x => x.Name.EqualsIgnoreCase(primaryNetworkName)).GetPhysicalAddress().ToString());
                    DeleteReservation(secondaryAddress.ToString(), activityData.SecondaryDhcpServerIPAddress, clientNetworks.FirstOrDefault(x => x.Name.EqualsIgnoreCase(secondaryNetworkName)).GetPhysicalAddress().ToString());
                }
                else
                {
                    DeleteReservation(primaryAddress.ToString(), activityData.PrimaryDhcpServerIPAddress, clientNetworks.FirstOrDefault(x => x.Name.EqualsIgnoreCase(primaryNetworkName)).GetPhysicalAddress().ToString());
                    DeleteReservation(secondaryAddress.ToString(), activityData.SecondaryDhcpServerIPAddress, clientNetworks.FirstOrDefault(x => x.Name.EqualsIgnoreCase(secondaryNetworkName)).GetPhysicalAddress().ToString());
                    CreateReservation(primaryAddress.ToString(), activityData.PrimaryDhcpServerIPAddress, clientNetworks.FirstOrDefault(x => x.Name.EqualsIgnoreCase(primaryNetworkName)).GetPhysicalAddress().ToString());
                    CreateReservation(secondaryAddress.ToString(), activityData.SecondaryDhcpServerIPAddress, clientNetworks.FirstOrDefault(x => x.Name.EqualsIgnoreCase(secondaryNetworkName)).GetPhysicalAddress().ToString());
                }

                return true;
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug(ex.Message);
                return false;
            }
        }

        #endregion
    }
}
