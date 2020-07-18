using System;
using System.Globalization;
using System.Net;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Dhcp;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using System.IO;

namespace HP.ScalableTest.Plugin.Wireless
{
    public class WirelessExecutionEngine : IPluginExecutionEngine
    {
        #region Local Variables
        WirelessTests _tests;
        #endregion

        #region Implementation of IActivityPlugin

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            CtcSettings.Initialize(executionData);

            // create activity data
            WirelessActivityData activityData = executionData.GetMetadata<WirelessActivityData>(CtcMetadataConverter.Converters);

            string ipAddress = activityData.ProductFamily == ProductFamilies.VEP && activityData.PrinterInterfaceType == ProductType.MultipleInterface ? activityData.WirelessInterfaceAddress : activityData.PrimaryInterfaceAddress;


            bool continueTest = true;

            if (activityData.ProductFamily == ProductFamilies.VEP)
            {
                while (continueTest && !NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.WirelessInterfaceAddress), TimeSpan.FromSeconds(10)))
                {
                    continueTest = CtcUtility.ShowErrorPopup($"Printer: {activityData.WirelessInterfaceAddress} is not available.\n Please cold reset the printer.");
                }
            }

            while (continueTest && !NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.PrimaryInterfaceAddress), TimeSpan.FromSeconds(10)))
            {
                continueTest = CtcUtility.ShowErrorPopup($"Printer: {activityData.PrimaryInterfaceAddress} is not available.\n Please cold reset the printer.");
            }

			EwsWrapper.Instance().Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), activityData.ProductName, ipAddress, Path.Combine(activityData.SitemapPath, activityData.SitemapVersion), BrowserModel.Firefox);

			EwsWrapper.Instance().Start();
            EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();

            SnmpWrapper.Instance().Create(ipAddress);

            TelnetWrapper.Instance().Create(ipAddress);

            string dhcpServerIp;

            if (activityData.ProductFamily == ProductFamilies.VEP)
            {
                Printer printer = PrinterFactory.Create(activityData.ProductFamily.ToString(), activityData.WirelessInterfaceAddress);

				if (string.IsNullOrEmpty(printer.MacAddress))
				{
					TraceFactory.Logger.Info($"Failed to discover the mac address from IP address: {activityData.WirelessInterfaceAddress}");
					return new PluginExecutionResult(PluginResult.Failed, $"Failed to discover the mac address from IP address: {activityData.WirelessInterfaceAddress}");
				}

                activityData.WirelessMacAddress = printer.MacAddress;

                string value = activityData.WirelessInterfaceAddress.Split('.')[2];

                dhcpServerIp = string.Format(CultureInfo.CurrentCulture, activityData.DhcpServerIp, value);

                using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(dhcpServerIp))
                {
                    string scope = dhcpClient.Channel.GetDhcpScopeIP(dhcpServerIp);
                    dhcpClient.Channel.DeleteReservation(dhcpServerIp, scope, activityData.WirelessInterfaceAddress, activityData.WirelessMacAddress);

                    if (dhcpClient.Channel.CreateReservation(dhcpServerIp, scope, activityData.WirelessInterfaceAddress, activityData.WirelessMacAddress, ReservationType.Both))
                    {
                        TraceFactory.Logger.Info($"Successfully created reservation for IP address: {activityData.WirelessInterfaceAddress}, Mac address: {activityData.WirelessMacAddress} for {ReservationType.Both}");
                    }
                    else
                    {
                        TraceFactory.Logger.Info($"Failed to create reservation for IP address: {activityData.WirelessInterfaceAddress}, Mac address: {activityData.WirelessMacAddress} for {ReservationType.Both}");
                        return new PluginExecutionResult(PluginResult.Failed, $"Failed to create reservation for IP address: {activityData.WirelessInterfaceAddress}, Mac address: {activityData.WirelessMacAddress} for {ReservationType.Both}");


                    }
                }
            }

            dhcpServerIp = activityData.DhcpServerIp.FormatWith(activityData.PrimaryInterfaceAddress.Split('.')[2]);

            using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(dhcpServerIp))
            {
                string wiredMacAddress = PrinterFactory.Create(activityData.ProductFamily.ToString(), activityData.PrimaryInterfaceAddress).MacAddress;


                if (string.IsNullOrEmpty(wiredMacAddress))
                {
                    TraceFactory.Logger.Info($"Failed to discover the mac address from IP address: {activityData.PrimaryInterfaceAddress}");
                    return new PluginExecutionResult(PluginResult.Failed, $"Failed to discover the mac address from IP address: {activityData.PrimaryInterfaceAddress}");
                }

                string scope = dhcpClient.Channel.GetDhcpScopeIP(dhcpServerIp);
                dhcpClient.Channel.DeleteReservation(dhcpServerIp, scope, activityData.PrimaryInterfaceAddress, wiredMacAddress);

                if (dhcpClient.Channel.CreateReservation(dhcpServerIp, scope, activityData.PrimaryInterfaceAddress, wiredMacAddress, ReservationType.Both))
                {
                    TraceFactory.Logger.Info($"Successfully created reservation for IP address: {activityData.PrimaryInterfaceAddress}, Mac address: {wiredMacAddress} for {ReservationType.Both}");
                }
                else
                {
                    TraceFactory.Logger.Info($"Failed to create reservation for IP address: {activityData.PrimaryInterfaceAddress}, Mac address: {wiredMacAddress} for {ReservationType.Both}");
                    return new PluginExecutionResult(PluginResult.Failed, $"Failed to create reservation for IP address: {activityData.PrimaryInterfaceAddress}, Mac address: {wiredMacAddress} for {ReservationType.Both}");
                }
            }

            activityData.SessionId = executionData.SessionId;

            if (null == _tests)
            {
                _tests = new WirelessTests(activityData);
            }

            // Execute the selected tests
            foreach (int testNumber in activityData.SelectedTests)
            {
                try
                {
                    ExecutionServices.SessionRuntime.AsInternal().WaitIfPaused();
                    _tests.RunTest(executionData, testNumber, IPAddress.Parse(ipAddress), (ProductFamilies)Enum.Parse(typeof(ProductFamilies), activityData.ProductFamily.ToString()));
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Fatal($"Error while executing Test:{testNumber} {ex.Message}\n");
                }
            }

            EwsWrapper.Instance().Stop();

            return new PluginExecutionResult(PluginResult.Passed);
        }

        #endregion
    }
}
