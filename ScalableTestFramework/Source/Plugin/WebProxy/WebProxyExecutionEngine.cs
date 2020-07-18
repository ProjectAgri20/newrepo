using System;
using System.Net;
using System.Xml.XPath;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Dhcp;
using HP.ScalableTest.PluginSupport.Connectivity.DnsApp;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Selenium;
using HP.ScalableTest.PluginSupport.Connectivity.SystemConfiguration;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using System.IO;

namespace HP.ScalableTest.Plugin.WebProxy
{
    /// <summary>
    /// Execution Control for Web Proxy Plug-in
    /// </summary>
    public class WebProxyExecutionEngine : IPluginExecutionEngine
    {
        #region Local Variable

        /// <summary>
        /// Initialize <see cref=" WebProxyTests"/> instance
        /// </summary>
        private WebProxyTests _webProxyTests;

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

        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            CtcSettings.Initialize(executionData);

            WebProxyActivityData activityData = executionData.GetMetadata<WebProxyActivityData>(CtcMetadataConverter.Converters);

            #region Scenario Prerequisites

            //Check the Windows Server Service is up and running on all servers         
            CtcUtility.StartService("WindowsServerService", activityData.PrimaryDHCPServerIPAddress);
            CtcUtility.StartService("WindowsServerService", activityData.UnsecureWebProxyServerIPAddress);
            CtcUtility.StartService("WindowsServerService", activityData.SecureWebProxyServerIPAddress);
            CtcUtility.StartService("WindowsServerService", activityData.WPADServerIPAddress);

            //Add WPAD entry and Domain Name on the DHCP Server
            DhcpApplicationServiceClient serviceFunction = DhcpApplicationServiceClient.Create(activityData.PrimaryDHCPServerIPAddress);
            activityData.PrimaryDHCPScopeIPAddress = serviceFunction.Channel.GetDhcpScopeIP(activityData.PrimaryDHCPServerIPAddress);
            serviceFunction.Channel.SetWPADServer(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.cURLPathIPAddress);
            activityData.DomainName = "lfpctc.com";

            string recordType = "A";
            serviceFunction.Channel.SetDomainName(activityData.PrimaryDHCPServerIPAddress, activityData.PrimaryDHCPScopeIPAddress, activityData.DomainName);

            // Retrieving hostnames of all servers

            SystemConfigurationClient secureProxy = SystemConfigurationClient.Create(activityData.SecureWebProxyServerIPAddress);
            activityData.SecureWebProxyServerHostName = secureProxy.Channel.GetHostName();
            TraceFactory.Logger.Info("Secure Web Proxy hostname is {0}".FormatWith(activityData.SecureWebProxyServerHostName));

            SystemConfigurationClient unsecureProxy = SystemConfigurationClient.Create(activityData.UnsecureWebProxyServerIPAddress);
            activityData.UnsecureWebProxyServerHostName = unsecureProxy.Channel.GetHostName();
            TraceFactory.Logger.Info("Unsecure Web Proxy hostname is {0}".FormatWith(activityData.UnsecureWebProxyServerHostName));

            SystemConfigurationClient wpadServer = SystemConfigurationClient.Create(activityData.WPADServerIPAddress);
            activityData.WPADServerHostName = wpadServer.Channel.GetHostName();
            TraceFactory.Logger.Info("WPAD hostname is {0}".FormatWith(activityData.WPADServerHostName));

            //Add DNS entry for all servers on the DHCP/DNS server
            DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(activityData.PrimaryDHCPServerIPAddress);
            dnsClient.Channel.AddDomain(activityData.DomainName);
            dnsClient.Channel.AddRecord(activityData.DomainName, activityData.SecureWebProxyServerHostName, recordType, activityData.SecureWebProxyServerIPAddress);
            dnsClient.Channel.AddRecord(activityData.DomainName, activityData.UnsecureWebProxyServerHostName, recordType, activityData.UnsecureWebProxyServerIPAddress);
            dnsClient.Channel.AddRecord(activityData.DomainName, activityData.WPADServerHostName, recordType, activityData.WPADServerIPAddress);

            string curlFqdn = string.Concat(activityData.WPADServerHostName, activityData.DomainName);
            activityData.cURLPathFQDN = string.Concat("http://", curlFqdn, ":80/wpad.dat");
            activityData.SecureWebProxyServerFQDN = string.Concat(activityData.SecureWebProxyServerHostName, activityData.DomainName);

            TraceFactory.Logger.Info(activityData.cURLPathFQDN);
            TraceFactory.Logger.Info(activityData.SecureWebProxyServerFQDN);

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.WiredIPv4Address));

            // create instance of ews adapter
            EwsWrapper.Instance().Create(family, activityData.ProductName, activityData.WiredIPv4Address, Path.Combine(activityData.SitemapPath, activityData.SiteMapVersion), BrowserModel.Firefox);
            EwsWrapper.Instance().Start();

            //Create instance of SNMP Wrapper
            SnmpWrapper.Instance().Create(activityData.WiredIPv4Address);

            //Create instance of Telnet Wrapper
            TelnetWrapper.Instance().Create(activityData.WiredIPv4Address);

            //Wake up the printer and disable sleep mode
            EwsWrapper.Instance().WakeUpPrinter();
            printer.KeepAwake();

            EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();

            #endregion
            if (null == _webProxyTests)
            {
                _webProxyTests = new WebProxyTests(activityData);
            }

            // assign the session id to activity data
            activityData.SessionId = executionData.SessionId;

            foreach (int testNumber in activityData.SelectedTests)
            {
                try
                {
                    ExecutionServices.SessionRuntime.AsInternal().WaitIfPaused();
                    _webProxyTests.RunTest(executionData, testNumber, IPAddress.Parse(activityData.WiredIPv4Address), (ProductFamilies)Enum.Parse(typeof(ProductFamilies), activityData.ProductFamily));
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

    }
}
