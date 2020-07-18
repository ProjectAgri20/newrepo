using System;
using System.Net;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;
using System.IO;

namespace HP.ScalableTest.Plugin.Firewall
{
    /// <summary>
    /// Execution control for Firewall plugin
    /// </summary>
    public class FirewallExecutionEngine : IPluginExecutionEngine
    {
        #region Constructor

        #endregion Constructor

        #region Implementation of IPluginExecutionEngine

        FirewallTests _tests;

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            CtcSettings.Initialize(executionData);

            // create activity data
            FirewallActivityData activityData = executionData.GetMetadata<FirewallActivityData>(CtcMetadataConverter.Converters);
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

            // Instance for EWS adapter and Start WebDriver
            EwsWrapper.Instance().Create(family, activityData.ProductName, activityData.IPv4Address, Path.Combine(activityData.SitemapPath, activityData.SitemapsVersion), BrowserModel.Firefox);
            EwsWrapper.Instance().Start();
            EwsWrapper.Instance().WakeUpPrinter();
            EwsWrapper.Instance().SetWSDiscovery(true);
            EwsWrapper.Instance().SetDHCPv6OnStartup(true);
            EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();

            // Get All IPv6 Addresses

            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
            activityData.IPv6LinkLocalAddress = printer.IPv6LinkLocalAddress?.ToString() ?? string.Empty;
            activityData.IPv6StatefulAddress = printer.IPv6StateFullAddress?.ToString() ?? string.Empty;
            activityData.IPv6StatelessAddress = printer.IPv6StatelessAddresses.Count == 0 ? string.Empty : printer.IPv6StatelessAddresses[0].ToString();

            // Instance for SNMP wrapper
            SnmpWrapper.Instance().Create(activityData.IPv4Address);

            // Instance for Telnet wrapper
            TelnetWrapper.Instance().Create(activityData.IPv4Address);

            // Instance for Firewall Tests
            if (null == _tests)
            {
                _tests = new FirewallTests(activityData);
            }

            foreach (int testNumber in activityData.SelectedTests)
            {
                try
                {
                    ExecutionServices.SessionRuntime.AsInternal().WaitIfPaused();
                    _tests.RunTest(executionData, testNumber, IPAddress.Parse(activityData.IPv4Address), (ProductFamilies)Enum.Parse(typeof(ProductFamilies), activityData.ProductFamily));
                }
                catch (Exception exception)
                {
                    TraceFactory.Logger.Info("Error while executing test {0} with exception {1}. \n".FormatWith(testNumber, exception.Message));
                }
            }

            EwsWrapper.Instance().Stop();

            return new PluginExecutionResult(PluginResult.Passed);
        }

        #endregion
    }
}
