using System;
using System.Net;

using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using System.IO;

namespace HP.ScalableTest.Plugin.WiFiDirect
{
    public partial class WiFiDirectExecutionEngine : IPluginExecutionEngine
    {
        #region Local Variables
        WiFiDirectTests _tests = null;
        #endregion

        #region Implementation of IActivityPlugin

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            CtcSettings.Initialize(executionData);

            // create activity data
            WiFiDirectActivityData activityData = executionData.GetMetadata<WiFiDirectActivityData>(CtcMetadataConverter.Converters);

            string ipAddress = activityData.PrimaryInterfaceAddress;
            EwsWrapper.Instance().Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), activityData.ProductName, ipAddress, Path.Combine(activityData.SitemapPath, activityData.SitemapVersion), BrowserModel.Firefox, EwsAdapterType.WebDriverAdapter);

            EwsWrapper.Instance().Start();

            SnmpWrapper.Instance().Create(ipAddress);

            TelnetWrapper.Instance().Create(ipAddress);

            //string DhcpServerIp = string.Empty;

            //if (!(activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) || activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString())))
            //{
            //	Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WirelessInterfaceAddress);

            //	if (null == printer.MacAddress)
            //	{
            //		TraceFactory.Logger.Info("Failed to discover the mac address from IP address: {0}".FormatWith(activityData.WirelessInterfaceAddress));
            //		return new PluginExecutionResult(PluginResult.Failed, "Failed to discover the mac address from IP address: {0}".FormatWith(activityData.WirelessInterfaceAddress));
            //	}

            //	activityData.WirelessMacAddress = printer.MacAddress;

            //	string value = activityData.WirelessInterfaceAddress.Split(new char[] { '.' })[2];

            //	DhcpServerIp = activityData.DhcpServerIp.FormatWith(value);

            //	using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(DhcpServerIp))
            //	{
            //		string scope = dhcpClient.Channel.GetDhcpScopeIP(DhcpServerIp);
            //		dhcpClient.Channel.DeleteReservation(DhcpServerIp, scope, activityData.WirelessInterfaceAddress, activityData.WirelessMacAddress);

            //		if (dhcpClient.Channel.CreateReservation(DhcpServerIp, scope, activityData.WirelessInterfaceAddress, activityData.WirelessMacAddress, ReservationType.Both))
            //		{
            //			TraceFactory.Logger.Info("Successfully created reservation for IP address: {0}, Mac address: {1} for {2}".FormatWith(activityData.WirelessInterfaceAddress, activityData.WirelessMacAddress, ReservationType.Both));
            //		}
            //		else
            //		{
            //			TraceFactory.Logger.Info("Failed to create reservation for IP address: {0}, Mac address: {1} for {2}".FormatWith(activityData.WirelessInterfaceAddress, activityData.WirelessMacAddress, ReservationType.Both));
            //			return new PluginExecutionResult(PluginResult.Failed, "Failed to create reservation for IP address: {0}, Mac address: {1} for {2}".FormatWith(activityData.WirelessInterfaceAddress, activityData.WirelessMacAddress, ReservationType.Both));
            //		}
            //	}
            //}

            //DhcpServerIp = activityData.DhcpServerIp.FormatWith(activityData.PrimaryInterfaceAddress.Split(new char[] { '.' })[2]);

            //using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(DhcpServerIp))
            //{
            //	string wiredMacAddress = SnmpWrapper.Instance().
            //	string scope = dhcpClient.Channel.GetDhcpScopeIP(DhcpServerIp);
            //	dhcpClient.Channel.DeleteReservation(DhcpServerIp, scope, activityData.PrimaryInterfaceAddress, wiredMacAddress);

            //	if (dhcpClient.Channel.CreateReservation(DhcpServerIp, scope, activityData.PrimaryInterfaceAddress, wiredMacAddress, ReservationType.Both))
            //	{
            //		TraceFactory.Logger.Info("Successfully created reservation for IP address: {0}, Mac address: {1} for {2}".FormatWith(activityData.PrimaryInterfaceAddress, wiredMacAddress, ReservationType.Both));
            //	}
            //	else
            //	{
            //		TraceFactory.Logger.Info("Failed to create reservation for IP address: {0}, Mac address: {1} for {2}".FormatWith(activityData.PrimaryInterfaceAddress, wiredMacAddress, ReservationType.Both));
            //		return new PluginExecutionResult(PluginResult.Failed, "Failed to create reservation for IP address: {0}, Mac address: {1} for {2}".FormatWith(activityData.PrimaryInterfaceAddress, wiredMacAddress, ReservationType.Both));
            //	}
            //}

            activityData.SessionId = executionData.SessionId;

            if (null == _tests)
            {
                _tests = new WiFiDirectTests(activityData);
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
                    TraceFactory.Logger.Fatal("Error while executing Test:{0} {1}\n".FormatWith(testNumber, ex.Message));
                    continue;
                }
            }

            EwsWrapper.Instance().Stop();

            return new PluginExecutionResult(PluginResult.Passed);
        }

        #endregion
    }
}
