using System;
using System.Globalization;
using System.Net;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Dhcp;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.NetworkSwitch;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.RadiusServer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;
using System.IO;

namespace HP.ScalableTest.Plugin.DotOneX
{
    public class DotOneXExecutionEngine : IPluginExecutionEngine
    {
        #region Local Variables

        DotOneXTests _tests;

        private const string DOT1X_USERNAME = "{0}user";
        private const string DOT1X_PASSWORD = "1iso*help";
        private const string SHARED_SECRET = "xyzzy";
        private const string NETWORK_POLICY = "policy1";

        #endregion

        #region Constructor

        #endregion

        #region Implementation of IPluginExecutionEngine

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            CtcSettings.Initialize(executionData);

            // create activity data
            DotOneXActivityData activityData = executionData.GetMetadata<DotOneXActivityData>(CtcMetadataConverter.Converters);
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            activityData.DotOneXUserName = DOT1X_USERNAME.FormatWith(activityData.RadiusServerType.ToString().ToLower(CultureInfo.CurrentCulture));
            activityData.DotOneXPassword = DOT1X_PASSWORD;
            activityData.SharedSecret = SHARED_SECRET;
            activityData.PolicyName = NETWORK_POLICY;

            bool continueTest = true;

            while (continueTest && !NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), TimeSpan.FromSeconds(30)))
            {
                continueTest = DotOneXTemplates.ShowErrorPopUp("Printer: {0} is not available.\nPlease cold reset the printer.".FormatWith(activityData.Ipv4Address));
            }

            if (!continueTest)
            {
                return new PluginExecutionResult(PluginResult.Failed, "Printer: {0} is not available.\nPlease cold reset the printer.".FormatWith(activityData.Ipv4Address));
            }

            // create instance of ews adapter
            EwsWrapper.Instance().Create(family, activityData.ProductName, activityData.Ipv4Address, Path.Combine(activityData.SitemapPath, activityData.SiteMapVersion), BrowserModel.Firefox);

            EwsWrapper.Instance().Start();

            EwsWrapper.Instance().WakeUpPrinter();

            EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();

            SnmpWrapper.Instance().Create(activityData.Ipv4Address);
            SnmpWrapper.Instance().SetCommunityName("public");

            TelnetWrapper.Instance().Create(activityData.Ipv4Address);

            //TODO: Migration issue even thought it is not migration issue why it is commented
            //EwsWrapper.Instance().SetWSDiscovery(true);

            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.Ipv4Address);
            activityData.MacAddress = printer.MacAddress;

            using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.DhcpServerIp))
            {
                string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.DhcpServerIp);
                dhcpClient.Channel.DeleteReservation(activityData.DhcpServerIp, scope, activityData.Ipv4Address, activityData.MacAddress);

                if (dhcpClient.Channel.CreateReservation(activityData.DhcpServerIp, scope, activityData.Ipv4Address, activityData.MacAddress, ReservationType.Both))
                {
                    TraceFactory.Logger.Info("Successfully created reservation for IP address: {0}, Mac address: {1} for {2}".FormatWith(activityData.Ipv4Address, activityData.MacAddress, ReservationType.Both));
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to create reservation for IP address: {0}, Mac address: {1} for {2}".FormatWith(activityData.Ipv4Address, activityData.MacAddress, ReservationType.Both));
                    return new PluginExecutionResult(PluginResult.Failed, "Failed to create reservation for IP address: {0}, Mac address: {1} for {2}".FormatWith(activityData.Ipv4Address, activityData.MacAddress, ReservationType.Both));
                }
            }

            if (!ConfigureRadiusServer(activityData))
            {
                return new PluginExecutionResult(PluginResult.Failed, "Failed to configure radius server");
            }

            if (!ConfigureSwitch(activityData))
            {
                return new PluginExecutionResult(PluginResult.Failed, "Failed to configure switch");
            }

            activityData.SessionId = executionData.SessionId;

            if (null == _tests)
            {
                _tests = new DotOneXTests(activityData);
            }

            // Execute the selected tests
            foreach (int testNumber in activityData.SelectedTests)
            {
                try
                {
                    ExecutionServices.SessionRuntime.AsInternal().WaitIfPaused();
                    _tests.RunTest(executionData, testNumber, IPAddress.Parse(activityData.Ipv4Address), (ProductFamilies)Enum.Parse(typeof(ProductFamilies), activityData.ProductFamily));
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Fatal("Error while executing Test:{0} \n".FormatWith(testNumber, ex.Message));
                }
            }

            EwsWrapper.Instance().Stop();

            return new PluginExecutionResult(PluginResult.Passed);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Configure the radius server with the switch client details
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <returns>True for successfull configuration, else false.</returns>
        private static bool ConfigureRadiusServer(DotOneXActivityData activityData)
        {
            using (RadiusApplicationServiceClient radiusCient = RadiusApplicationServiceClient.Create(activityData.RadiusServerIp))
            {
                radiusCient.Channel.DeleteAllClients();

                if (radiusCient.Channel.AddClient("Automation_Switch", activityData.SwitchIp, activityData.SharedSecret))
                {
                    TraceFactory.Logger.Info("Successfully added the client : {0} on radius server: {1}".FormatWith(activityData.SwitchIp, activityData.RadiusServerIp));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to add the client : {0} on radius server: {1}".FormatWith(activityData.SwitchIp, activityData.RadiusServerIp));
                    return false;
                }
            }
        }

        /// <summary>
        /// Configure the switch with the radius server details
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <returns>True for successfull configuration, else false.</returns>
        private static bool ConfigureSwitch(DotOneXActivityData activityData)
        {
            INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));

            networkSwitch.DeConfigureAllRadiusServer();

            return networkSwitch.ConfigureRadiusServer(IPAddress.Parse(activityData.RadiusServerIp), activityData.SharedSecret);
        }

        #endregion
    }
}
