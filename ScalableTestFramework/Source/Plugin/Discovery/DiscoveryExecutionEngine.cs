using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;
using HP.ScalableTest.PluginSupport.Connectivity.SystemConfiguration;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using HP.ScalableTest.Utility;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.Discovery
{
    /// <summary>
    /// Execution control for the NetworkDiscovery plug-in.
    /// </summary>
    public class DiscoveryExecutionEngine : IPluginExecutionEngine
    {
        DiscoveryTests _networkDiscoveryTests;

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            CtcSettings.Initialize(executionData);

            // create activity data
            DiscoveryActivityData activityData = executionData.GetMetadata<DiscoveryActivityData>(CtcMetadataConverter.Converters);
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            // create instance of ews adapter
            EwsWrapper.Instance().Create(family, activityData.ProductName, activityData.IPv4Address, Path.Combine(activityData.SitemapPath,
activityData.SitemapsVersion), BrowserModel.Firefox); 

            //create instance of SNMP wrapper
            SnmpWrapper.Instance().Create(activityData.IPv4Address);

            //create instance of Telnet wrapper
            TelnetWrapper.Instance().Create(activityData.IPv4Address);

            EwsWrapper.Instance().Start();

            EwsWrapper.Instance().WakeUpPrinter();

            EwsWrapper.Instance().SetAdvancedOptions();

            EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();

            BonjourServiceInstallation();

            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.IPv4Address);
            activityData.PrinterMacAddress = printer.MacAddress.Replace(":", string.Empty);

            // create network discovery tests
            if (null == _networkDiscoveryTests)
            {
                _networkDiscoveryTests = new DiscoveryTests(activityData);
            }

            foreach (int testNumber in activityData.SelectedTests)
            {
                try
                {
                    ExecutionServices.SessionRuntime.AsInternal().WaitIfPaused();
                    _networkDiscoveryTests.RunTest(executionData, testNumber, IPAddress.Parse(activityData.IPv4Address), (ProductFamilies)Enum.Parse(typeof(ProductFamilies), activityData.ProductFamily));
                }
                catch (Exception e)
                {
                    TraceFactory.Logger.Info("Error while executing test:{0}  \n ".FormatWith(testNumber, e.Message));
                }
            }

            return new PluginExecutionResult(PluginResult.Passed);
        }

        private static void BonjourServiceInstallation()
        {
            string serviceFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mDNSResponder.exe");
            TraceFactory.Logger.Info("service file path : {0}".FormatWith(serviceFile));
            string serviceName = "Bonjour Service";
            if (!SystemConfiguration.IsServiceExist(serviceName))
            {
                // Creates the Bonjour Service
                ProcessUtil.Execute("cmd.exe", "/C sc create \"{0}\" binPath= \"{1}\"".FormatWith(serviceName, serviceFile));
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
            if (SystemConfiguration.GetServiceStatus(serviceName) != ServiceControllerStatus.Running)
            {
                // Start the Bonjour Service
                ProcessUtil.Execute("cmd.exe", "/C net start \"{0}\"".FormatWith(serviceName));
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }

    }
}
