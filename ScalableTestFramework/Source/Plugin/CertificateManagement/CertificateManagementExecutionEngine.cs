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
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;
using System.IO;

namespace HP.ScalableTest.Plugin.CertificateManagement
{
    public partial class CertificateManagementExecutionEngine : IPluginExecutionEngine
    {
        #region Local Variables

        CertificateManagementTests _tests = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of CertificateManagementExecutionControl class
        /// </summary>
        public CertificateManagementExecutionEngine()
        {

        }

        #endregion

        #region Implementation of IPluginExecutionEngine

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            CtcSettings.Initialize(executionData);

            // create activity data
            CertificateManagementActivityData activityData = executionData.GetMetadata<CertificateManagementActivityData>(CtcMetadataConverter.Converters);
            var ipV4Address = IPAddress.Parse(activityData.Ipv4Address);
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(activityData.ProductFamily));
            // Create instance of ews adapter 
            // Remember to give Combined sitemapPath + sitemapVersion
            EwsWrapper.Instance().Create(family, activityData.ProductName, activityData.Ipv4Address, Path.Combine(activityData.SitemapPath,activityData.SiteMapVersion), BrowserModel.Firefox);

            EwsWrapper.Instance().Start();
            EwsWrapper.Instance().WakeUpPrinter();
            EwsWrapper.Instance().SetAdvancedOptions();
            EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();

            SnmpWrapper.Instance().Create(activityData.Ipv4Address);
            SnmpWrapper.Instance().SetCommunityName("public");

            TelnetWrapper.Instance().Create(activityData.Ipv4Address);

          //  PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), Enum<ProductFamilies>.Value(activityData.ProductFamily));
            Printer.Printer printer = PrinterFactory.Create(family, ipV4Address);
            activityData.MacAddress = printer.MacAddress;

            //Check the Windows Server Service,Packet Capture service and Kiwi Syslog server is up and running            
            CtcUtility.StartService("WindowsServerService", activityData.DhcpServerIp);
            CtcUtility.StartService(@"dns", activityData.DhcpServerIp);

            using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.DhcpServerIp))
            {
                string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.DhcpServerIp);
                dhcpClient.Channel.DeleteReservation(activityData.DhcpServerIp, scope, activityData.Ipv4Address, activityData.MacAddress);

                if (dhcpClient.Channel.CreateReservation(activityData.DhcpServerIp, scope, ipV4Address.ToString(), activityData.MacAddress, ReservationType.Both))
                {
                    TraceFactory.Logger.Info("Successfully created reservation for IP address: {0}, Mac address: {1} for {2}".FormatWith(ipV4Address, activityData.MacAddress, ReservationType.Both));
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to create reservation for IP address: {0}, Mac address: {1} for {2}".FormatWith(ipV4Address, activityData.MacAddress, ReservationType.Both));
                    return new PluginExecutionResult(PluginResult.Failed, "Failed to create reservation for IP address: {0}, Mac address: {1} for {2}".FormatWith(ipV4Address, activityData.MacAddress, ReservationType.Both));
                }
            }

            if (null == _tests)
            {
                _tests = new CertificateManagementTests(activityData);
            }

            // Execute the selected tests
            foreach (int testNumber in activityData.SelectedTests)
            {
                try
                {
                    ExecutionServices.SessionRuntime.AsInternal().WaitIfPaused();
                    _tests.RunTest(executionData, testNumber, ipV4Address, activityData.ProductFamily);
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Fatal("** Error while executing Test:{0} \n".FormatWith(testNumber, ex.Message));
                    continue;
                }
            }

            EwsWrapper.Instance().Stop();

            return new PluginExecutionResult(PluginResult.Passed);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        void LogWhenPaused()
        {
            TraceFactory.Logger.Debug("Trace out a message only when your plugin is paused...");
        }

        /// <summary>
        /// 
        /// </summary>
        void LogWhenResumed()
        {
            TraceFactory.Logger.Debug("Trace out a message only when your plugin is resumed...");
        }

        #endregion
    }
}
