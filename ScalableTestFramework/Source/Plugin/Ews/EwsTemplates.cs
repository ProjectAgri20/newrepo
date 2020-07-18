using System;
using System.Net;
using System.Threading;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.Ews
{
    internal static class EwsTemplates
    {

        /// <summary>
        /// ID: 406714 (VEP)
        /// Access of network summary page & validation of the contents of the page in TCP/IP settings tab
        /// 1) Access the device using IPv4 or IPv6 address. 
        /// 2) Click the Networking-> TCP/IP Settings->summary tab
        /// </summary>
        public static bool TemplateNetworkSummaryVEP(EwsAdapter adapter)
        {
            bool result = false;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));


            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");
            TraceFactory.Logger.Info("Navigating to Summary tab");

            adapter.Navigate("Summary", "https");

            result = (adapter.SearchText("Summary"));
            TraceFactory.Logger.Info("Result for accessing summary page is" + result);
            //adapter.Click("Refresh");
            adapter.SearchText("Host Name");
            string ipAddress = adapter.Settings.DeviceAddress;

            result = result && (adapter.SearchText(ipAddress)) &&
                               (adapter.SearchText("TCP/IP(v4) Settings")) &&
                               (adapter.SearchText("TCP/IP(v6) Settings"));
            TraceFactory.Logger.Info("Searching the below items in Summary page for validation");
            TraceFactory.Logger.Info("IP Address: " + adapter.SearchText(ipAddress));
            TraceFactory.Logger.Info("TCP/IP(v4) Settings: " + adapter.SearchText("TCP/IP(v4) Settings"));
            TraceFactory.Logger.Info("TCP/IP(v6) Settings: " + adapter.SearchText("TCP/IP(v6) Settings"));
            TraceFactory.Logger.Info("Result after verification is: " + result);

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;
        }

        /// <summary>
        /// ID: 406714 (TPS)
        /// Access of network summary page & validation of the contents of the page in TCP/IP settings tab
        /// 1) Access the device using IPv4 or IPv6 address. 
        /// 2) Click the Networking-> TCP/IP Settings->summary tab
        /// </summary>
        public static bool TemplateNetworkSummaryTPS(EwsAdapter adapter)
        {
            bool result = false;
            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            result = (adapter.SearchText("Supplies"));
            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");
            TraceFactory.Logger.Info("Navigating to Summary page");
            adapter.Navigate("Summary", "https");

            result = result && (adapter.SearchText("Summary"));
            TraceFactory.Logger.Info("Summary page contains summary text and result is " + result);
            //adapter.Click("Refresh");
            adapter.SearchText("Host Name");
            TraceFactory.Logger.Info("Validate contents in summary page");
            string ipAddress = adapter.Settings.DeviceAddress;
            result = result && (adapter.SearchText(ipAddress)) &&
                               (adapter.SearchText("TCP/IP(v4)")) &&
                               (adapter.SearchText("TCP/IP(v6)")) &&
                               (adapter.SearchText("Network Identification")) &&
                               (adapter.SearchText("Wired Network Configuration")) &&
                               (adapter.SearchText("Enabled Features")) &&
                               (adapter.SearchText("Security"));
            TraceFactory.Logger.Info("Searching the below items in Summary page for validation");
            TraceFactory.Logger.Info("IP Address: " + adapter.SearchText(ipAddress));
            TraceFactory.Logger.Info("TCP/IP(v4) Settings: " + adapter.SearchText("TCP/IP(v4) Settings"));
            TraceFactory.Logger.Info("TCP/IP(v6) Settings: " + adapter.SearchText("TCP/IP(v6) Settings"));
            TraceFactory.Logger.Info("Network Identification: " + adapter.SearchText("Network Identification"));
            TraceFactory.Logger.Info("Wired Network Configuration: " + adapter.SearchText("Wired Network Configuration"));
            TraceFactory.Logger.Info("Enabled Features: " + adapter.SearchText("Enabled Features"));
            TraceFactory.Logger.Info("Security: " + adapter.SearchText("Security"));
            TraceFactory.Logger.Info("Result after verification is: " + result);

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;
        }

        /// <summary>
        /// ID: 406714 (LFP)
        /// Access of network summary page & validation of the contents of the page in TCP/IP settings tab
        /// 1) Access the device using IPv4 or IPv6 address. 
        /// 2) Click the Networking-> TCP/IP Settings->summary tab
        /// </summary>        
        public static bool TemplateNetworkSummaryLFP(EwsAdapter adapter)
        {
            return TemplateNetworkSummaryVEP(adapter);
        }

        /// <summary>
        /// ID: 406718 (VEP)
        /// Verification of refresh rate functinality
        /// Design Steps:
        /// 1. Web into the printer and go to Networking-->Settings--> Refresh rate 
        /// 2. Modify refresh rate to any value between 60-300 and apply the changes 
        /// 3.Now navigate to any page having Refresh button and wait to observe the refresh of page.
        /// Expected:
        /// A message “Changes have been made successfully. 
        /// Press the OK button to return to configuration page” should be displayed.
        /// Press OK.After returning to Refresh rate page, check that the given value is seen in the Refresh rate field. 
        /// The refresh of page should happen at the time specified in refresh rate and refreshing message 
        /// should be seen at left bottom corner of the page.
        /// </summary>
        public static bool TemplateRefreshRateVEP(EwsAdapter adapter)
        {
            bool result = false;
            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");

            TraceFactory.Logger.Info("Navigating to Refresh Rate tab");
            adapter.Navigate("Refresh_Rate", "https");

            TraceFactory.Logger.Info("Setting Refresh Rate value to 60");
            adapter.SetText("Set_Refresh_Rate", "60");

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);
            //while (adapter.SearchText("OK").Equals(true)) ;
            TraceFactory.Logger.Info("Navigating to Refresh Rate tab to check whether the value is retained");
            adapter.Navigate("Refresh_Rate");

            result = (adapter.GetValue("Set_Refresh_Rate") == "60");
            TraceFactory.Logger.Info("Refresh Rate: " + adapter.GetValue("Set_Refresh_Rate"));
            TraceFactory.Logger.Info("Result after verification is: " + result);

            TraceFactory.Logger.Info("Navigating to Summary tab");
            adapter.Navigate("Summary");
            Thread.Sleep(70000);
            result = result && (adapter.SearchText("Summary"));
            TraceFactory.Logger.Info("Result after text search is " + result);

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;
        }

        /// <summary>
        /// ID: 406718 (LFP)
        /// Verification of refresh rate functinality
        /// Design Steps:
        /// 1. Web into the printer and go to Networking-->Settings--> Refresh rate 
        /// 2. Modify refresh rate to any value between 60-300 and apply the changes 
        /// 3.Now navigate to any page having Refresh button and wait to observe the refresh of page.
        /// Expected:
        /// A message “Changes have been made successfully. 
        /// Press the OK button to return to configuration page” should be displayed.
        /// Press OK.After returning to Refresh rate page, check that the given value is seen in the Refresh rate field. 
        /// The refresh of page should happen at the time specified in refresh rate and refreshing message 
        /// should be seen at left bottom corner of the page.
        /// </summary>
        public static bool TemplateRefreshRateLFP(EwsAdapter adapter)
        {
            return TemplateRefreshRateVEP(adapter);
        }

        /// <summary>
        /// ID: 401875
        /// Verification of Cancel button under Networking Tab
        /// 1) Connect the printer to the network and make sure printer gets up address.
        /// 2) Go to Networking-> Network Identification->Do some changes and Click on 'Cancel'
        /// 3) Go to Networking-> Advanced->Do some changes and Click on 'Cancel'
        /// </summary>
        public static bool TemplateCancelButtonVEP(EwsAdapter adapter)
        {
            bool result = false;
            string hostName = string.Empty;
            string IPV4domainName = string.Empty;
            string IPV6domainName = string.Empty;
            string IPV4DNSPrimary = string.Empty;
            string IPV4DNSSecondary = string.Empty;
            string IPV6DNSPrimary = string.Empty;
            string IPV6DNSSecondary = string.Empty;
            string winsPrimary = string.Empty;
            string winsSecondary = string.Empty;
            string bonjourServiceName = string.Empty;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");
            TraceFactory.Logger.Info("Navigate to network identification tab");
            adapter.Navigate("Network_Identification", "https");

            // Get the default values before editing

            hostName = adapter.GetValue("HostName");
            IPV4domainName = adapter.GetValue("DomainName_IPv4");
            IPV6domainName = adapter.GetValue("DomainName_IPv6");
            IPV4DNSPrimary = adapter.GetValue("DNS_IPv4Primary");
            IPV4DNSSecondary = adapter.GetValue("DNS_IPv4_Secondary");
            IPV6DNSPrimary = adapter.GetValue("DNS_IPv6_Primary");
            IPV6DNSSecondary = adapter.GetValue("DNS_IPv6_Secondary");
            winsPrimary = adapter.GetValue("WINS_IPv4_Primary");
            winsSecondary = adapter.GetValue("WINS_IPv4_Secondary");
            bonjourServiceName = adapter.GetValue("Bonjour_Service_Name");

            // Edit the values
            TraceFactory.Logger.Info("Setting values for Host Name - > test_hostname, DomainName IPv4 -> test_ipv4, DomainName IPv6 -> test_ipv6, DNS_IPv4Primary ->test_ipv4primary, ");
            TraceFactory.Logger.Info("DNS_IPv4_Secondary ->test_ipv4secondary,DNS_IPv6_Primary ->test_ipv6primary,DNS_IPv6_Secondary ->test_ipv6secondary,WINS_IPv4_Primary -> test_winsprimary");
            TraceFactory.Logger.Info("WINS_IPv4_Secondary ->test_winssecondary,Bonjour_Service_Name -> test_bonjourservicename");
            adapter.SetText("HostName", "test_hostname");
            adapter.SetText("DomainName_IPv4", "test_ipv4");
            adapter.SetText("DomainName_IPv6", "test_ipv6");
            adapter.SetText("DNS_IPv4Primary", "test_ipv4primary");
            adapter.SetText("DNS_IPv4_Secondary", "test_ipv4secondary");
            adapter.SetText("DNS_IPv6_Primary", "test_ipv6primary");
            adapter.SetText("DNS_IPv6_Secondary", "test_ipv6secondary");
            adapter.SetText("WINS_IPv4_Primary", "test_winsprimary");
            adapter.SetText("WINS_IPv4_Secondary", "test_winssecondary");
            adapter.SetText("Bonjour_Service_Name", "test_bonjourservicename");

            TraceFactory.Logger.Info("Clicking Cancel button");
            adapter.Click("Cancel");//ByName

            Thread.Sleep(5000);

            // Check whether the previous values where retained after "Cancel" was clicked
            TraceFactory.Logger.Info("Validating if previous values were retained");
            result = (adapter.GetValue("HostName") == hostName) &&
                     (adapter.GetValue("DomainName_IPv4") == IPV4domainName) &&
                     (adapter.GetValue("DomainName_IPv6") == IPV6domainName) &&
                     (adapter.GetValue("DNS_IPv4Primary") == IPV4DNSPrimary) &&
                     (adapter.GetValue("DNS_IPv4_Secondary") == IPV4DNSSecondary) &&
                     (adapter.GetValue("DNS_IPv6_Primary") == IPV6DNSPrimary) &&
                     (adapter.GetValue("DNS_IPv6_Secondary") == IPV6DNSSecondary) &&
                     (adapter.GetValue("WINS_IPv4_Primary") == winsPrimary) &&
                     (adapter.GetValue("WINS_IPv4_Secondary") == winsSecondary) &&
                     (adapter.GetValue("Bonjour_Service_Name") == bonjourServiceName);

            TraceFactory.Logger.Info("HostName: " + adapter.GetValue("HostName"));
            TraceFactory.Logger.Info("DomainName_IPv4: " + adapter.GetValue("DomainName_IPv4"));
            TraceFactory.Logger.Info("DomainName_IPv6: " + adapter.GetValue("DomainName_IPv6"));
            TraceFactory.Logger.Info("DNS_IPv4Primary: " + adapter.GetValue("DNS_IPv4Primary"));
            TraceFactory.Logger.Info("DNS_IPv4_Secondary: " + adapter.GetValue("DNS_IPv4_Secondary"));
            TraceFactory.Logger.Info("DNS_IPv6_Primary: " + adapter.GetValue("DNS_IPv6_Primary"));
            TraceFactory.Logger.Info("DNS_IPv6_Secondary: " + adapter.GetValue("DNS_IPv6_Secondary"));
            TraceFactory.Logger.Info("WINS_IPv4_Primary: " + adapter.GetValue("WINS_IPv4_Primary"));
            TraceFactory.Logger.Info("WINS_IPv4_Secondary: " + adapter.GetValue("WINS_IPv4_Secondary"));
            TraceFactory.Logger.Info("Bonjour_Service_Name: " + adapter.GetValue("Bonjour_Service_Name"));

            TraceFactory.Logger.Info("Result after verification is: " + result);

            string idleTimeout = string.Empty;
            string LPDBanner = string.Empty;
            string sysContact = string.Empty;
            string sysLocation = string.Empty;
            string proxyServer = string.Empty;
            string proxyPort = string.Empty;
            string proxyUserName = string.Empty;
            string proxyPassport = string.Empty;
            string proxyException = string.Empty;
            string defaultIP = string.Empty;
            string DHCPRequest = string.Empty;
            string statelessDHCPv4 = string.Empty;
            string enableDHCPv4 = string.Empty;
            string SLP = string.Empty;
            string hopLimit = string.Empty;
            string TTL = string.Empty;
            string syslogServer = string.Empty;
            string syslogProtocol = string.Empty;
            string syslogPort = string.Empty;
            string syslogMaxMsg = string.Empty;
            string syslogPriority = string.Empty;
            string enableCCC = string.Empty;

            TraceFactory.Logger.Info("Navigating to Advanced tab");
            adapter.Navigate("Advanced");

            // Get the values
            idleTimeout = adapter.GetValue("Idle_Timeout");
            LPDBanner = adapter.GetValue("LPD_Banner_Page");
            sysContact = adapter.GetValue("System_Contact");
            sysLocation = adapter.GetValue("System_Location");
            proxyServer = adapter.GetValue("Proxy_Server");
            proxyPort = adapter.GetValue("Proxy_Server_Port");
            proxyUserName = adapter.GetValue("Proxy_Server_Username");
            proxyPassport = adapter.GetValue("Proxy_Server_Password");
            proxyException = adapter.GetValue("Proxy_Server_Exception_List");
            defaultIP = adapter.GetValue("Default_IP");
            //DHCPRequest = adapter.GetValue("Send_DHCP_Requests");
            statelessDHCPv4 = adapter.GetValue("Use_Stateless_DHCPv4");
            enableDHCPv4 = adapter.GetValue("Enable_DHCPv4_FQDN_Compliance");
            SLP = adapter.GetValue("SLP_Client_Mode_Only");
            hopLimit = adapter.GetValue("Hop_Limit_WSD");
            TTL = adapter.GetValue("TTL_SLP");
            syslogServer = adapter.GetValue("Syslog_Server");
            //syslogProtocol = adapter.GetValue("Syslog_Protocol");
            //syslogPort = adapter.GetValue("Syslog_Port");
            syslogMaxMsg = adapter.GetValue("Syslog_Maximum_Messages");
            syslogPriority = adapter.GetValue("Syslog_Priority");
            //enableCCC = adapter.GetValue("Enable_CCC_Logging");

            TraceFactory.Logger.Info("Setting values for Idle_Timeout - > 100, LPD_Banner_Page -> Disable, System_Contact ->test_syscontact");
            TraceFactory.Logger.Info("System_Location ->test_syslocation, Proxy_Server ->test_proxyserver, Proxy_Server_Port -> 100");
            TraceFactory.Logger.Info("Proxy_Server_Username ->test_username, Proxy_Server_Password -> test_password, Proxy_Server_Exception_List ->test_exception");
            TraceFactory.Logger.Info("Default_IP->Legacy Default IP, Hop_Limit_WSD -> 100, TTL_SLP -> 100, Syslog_server -> test_server, Syslog_Maximum_messages -> 100, Syslog_priority ->100");
            // Edit data
            TraceFactory.Logger.Info("Check Use_stateless_DHCPv4");
            TraceFactory.Logger.Info("Check Enable_DHCPv4_FQDN_Compliance");
            TraceFactory.Logger.Info("Check SLP_Client_Mode_Only");

            adapter.SetText("Idle_Timeout", "100");
            adapter.SelectDropDown("LPD_Banner_Page", "Disable");
            adapter.SetText("System_Contact", "test_syscontact");
            adapter.SetText("System_Location", "test_syslocation");
            adapter.SetText("Proxy_Server", "test_proxyserver");
            adapter.SetText("Proxy_Server_Port", "100");
            adapter.SetText("Proxy_Server_Username", "test_username");
            adapter.SetText("Proxy_Server_Password", "test_password");
            adapter.SetText("Proxy_Server_Exception_List", "test_exception");
            adapter.SelectDropDown("Default_IP", "Legacy Default IP");
            adapter.Check("Use_Stateless_DHCPv4");
            adapter.Check("Enable_DHCPv4_FQDN_Compliance");
            adapter.Check("SLP_Client_Mode_Only");
            adapter.SetText("Hop_Limit_WSD", "100");
            adapter.SetText("TTL_SLP", "100");
            adapter.SetText("Syslog_Server", "test_server");
            adapter.SetText("Syslog_Maximum_Messages", "100");
            adapter.SetText("Syslog_Priority", "100");
            //adapter.SelectDropDown("Syslog_Protocol", "TCP");
            //adapter.SetText("Syslog_Port", "100");
            //adapter.Check("Send_DHCP_Requests");
            //adapter.Check("Enable_CCC_Logging");

            TraceFactory.Logger.Info("Clicking the cancel button");
            adapter.Click("Cancel");//ByName

            Thread.Sleep(5000);

            TraceFactory.Logger.Info("Validating if the previous values were retained");
            result = result &&
                        ((adapter.GetValue("Idle_Timeout") == idleTimeout) &&
                        (adapter.GetValue("LPD_Banner_Page") == LPDBanner) &&
                        (adapter.GetValue("System_Contact") == sysContact) &&
                        (adapter.GetValue("System_Location") == sysLocation) &&
                        (adapter.GetValue("Proxy_Server") == proxyServer) &&
                        (adapter.GetValue("Proxy_Server_Port") == proxyPort) &&
                        (adapter.GetValue("Proxy_Server_Username") == proxyUserName) &&
                        (adapter.GetValue("Proxy_Server_Password") == proxyPassport) &&
                        (adapter.GetValue("Proxy_Server_Exception_List") == proxyException) &&
                        (adapter.GetValue("Default_IP") == defaultIP) &&
                        (adapter.GetValue("Use_Stateless_DHCPv4") == statelessDHCPv4) &&
                        (adapter.GetValue("Enable_DHCPv4_FQDN_Compliance") == enableDHCPv4) &&
                        (adapter.GetValue("SLP_Client_Mode_Only") == SLP) &&
                        (adapter.GetValue("Hop_Limit_WSD") == hopLimit) &&
                        (adapter.GetValue("TTL_SLP") == TTL) &&
                        (adapter.GetValue("Syslog_Server") == syslogServer) &&
                        (adapter.GetValue("Syslog_Maximum_Messages") == syslogMaxMsg) &&
                        (adapter.GetValue("Syslog_Priority") == syslogPriority));
            //(adapter.GetValue("Send_DHCP_Requests") == DHCPRequest) &&
            //(adapter.GetValue("Syslog_Protocol") == syslogProtocol) &&
            //(adapter.GetValue("Syslog_Port") == syslogPort) &&
            //&&
            //(adapter.GetValue("Enable_CCC_Logging") == enableCCC));


            TraceFactory.Logger.Info("Idle_Timeout: " + adapter.GetValue("Idle_Timeout"));
            TraceFactory.Logger.Info("LPD_Banner_Page: " + adapter.GetValue("LPD_Banner_Page"));
            TraceFactory.Logger.Info("System_Contact: " + adapter.GetValue("System_Contact"));
            TraceFactory.Logger.Info("System_Location: " + adapter.GetValue("System_Location"));
            TraceFactory.Logger.Info("Proxy_Server: " + adapter.GetValue("Proxy_Server"));
            TraceFactory.Logger.Info("Proxy_Server_Port: " + adapter.GetValue("Proxy_Server_Port"));
            TraceFactory.Logger.Info("Proxy_Server_Username: " + adapter.GetValue("Proxy_Server_Username"));
            TraceFactory.Logger.Info("Proxy_Server_Password: " + adapter.GetValue("Proxy_Server_Password"));

            TraceFactory.Logger.Info("Proxy_Server_Exception_List: " + adapter.GetValue("Proxy_Server_Exception_List"));
            TraceFactory.Logger.Info("Default_IP: " + adapter.GetValue("Default_IP"));
            TraceFactory.Logger.Info("Use_Stateless_DHCPv4: " + adapter.GetValue("Use_Stateless_DHCPv4"));
            TraceFactory.Logger.Info("Enable_DHCPv4_FQDN_Compliance: " + adapter.GetValue("Enable_DHCPv4_FQDN_Compliance"));

            TraceFactory.Logger.Info("SLP_Client_Mode_Only: " + adapter.GetValue("SLP_Client_Mode_Only"));
            TraceFactory.Logger.Info("Hop_Limit_WSD: " + adapter.GetValue("Hop_Limit_WSD"));
            TraceFactory.Logger.Info("TTL_SLP: " + adapter.GetValue("TTL_SLP"));
            TraceFactory.Logger.Info("Syslog_Server: " + adapter.GetValue("Syslog_Server"));
            TraceFactory.Logger.Info("Syslog_Maximum_Messages: " + adapter.GetValue("Syslog_Maximum_Messages"));
            TraceFactory.Logger.Info("Syslog_Priority: " + adapter.GetValue("Syslog_Priority"));


            TraceFactory.Logger.Info("Result after verification is: " + result);

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;
        }

        /// <summary>
        /// ID: 401875
        /// Verification of Enable\Disable of TCP Print Services
        /// 1) Connect the printer to the network and make sure printer gets up address.
        /// 2) Go to Networking-> Network Identification->Do some changes and Click on 'Cancel'
        /// 3) Go to Networking-> Advanced->Do some changes and Click on 'Cancel'
        /// </summary>
        public static bool TemplateCancelButtonTPS(EwsAdapter adapter)
        {
            bool result = false;
            string hostName = string.Empty;
            string IPV4domainName = string.Empty;
            bool LPDPrinting = false;
            bool DHCPv6 = false;
            bool LLMNR = false;
            bool WSDiscovery = false;
            bool Bonjour = false;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            // Go to Networking-> Network Identification
            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");
            TraceFactory.Logger.Info("Navigating to network identification tab");
            adapter.Navigate("Network_Identification", "https");

            // Get the default values before editing
            hostName = adapter.GetValue("HostName");
            IPV4domainName = adapter.GetValue("DomainName_IPv4");
            TraceFactory.Logger.Info("Getting default values of hostname: " + hostName + " and Domain Name(IPv4): " + IPV4domainName);

            // Edit few values
            TraceFactory.Logger.Info("Set values for various fields");
            adapter.SetText("HostName", "test_hostname");
            adapter.SetText("DomainName_IPv4", "test_ipv4");
            TraceFactory.Logger.Info("Setting new values for hostname: test_hostname & for IPv4 Domainname: test_IPv4");
            //Click on 'Cancel'
            TraceFactory.Logger.Info("Clicking Cancel button");
            adapter.ClickByName("Cancel");

            Thread.Sleep(5000);

            // Assert the values of the changed fields
            result = (adapter.GetValue("HostName") == hostName) &&
                     (adapter.GetValue("DomainName_IPv4") == IPV4domainName);

            TraceFactory.Logger.Info("Checking the values after clicking cancel");
            TraceFactory.Logger.Info("HostName: " + adapter.GetValue("HostName"));
            TraceFactory.Logger.Info("DomainName_IPv4: " + adapter.GetValue("DomainName_IPv4"));

            TraceFactory.Logger.Info("Result after checking is " + result);

            // Navigate to Advanced Tab
            TraceFactory.Logger.Info("Navigating to advanced tab");
            adapter.Navigate("Advanced");

            LPDPrinting = adapter.IsChecked("LPD_Printing");
            DHCPv6 = adapter.IsChecked("DHCPv6");
            LLMNR = adapter.IsChecked("LLMNR");
            WSDiscovery = adapter.IsChecked("WS_Discovery");
            Bonjour = adapter.IsChecked("Bonjour");

            TraceFactory.Logger.Info("Set values for the various fields in the page");
            TraceFactory.Logger.Info("Check LPD Printing");
            adapter.Check("LPD_Printing");

            TraceFactory.Logger.Info("Check DHCPv6");
            adapter.Check("DHCPv6");

            TraceFactory.Logger.Info("Check LLMNR");
            adapter.Check("LLMNR");

            TraceFactory.Logger.Info("Check WS Discovery");
            adapter.Check("WS_Discovery");

            TraceFactory.Logger.Info("Check Bonjour");
            adapter.Check("Bonjour");

            TraceFactory.Logger.Info("Clicking the cancel button");
            adapter.Click("Cancel");

            Thread.Sleep(5000);

            TraceFactory.Logger.Info("Validate if the previous values were retained");
            result = result &&
                               (adapter.IsChecked("LPD_Printing") == LPDPrinting) &&
                               (adapter.IsChecked("DHCPv6") == DHCPv6) &&
                               (adapter.IsChecked("LLMNR") == LLMNR) &&
                               (adapter.IsChecked("WS_Discovery") == WSDiscovery) &&
                               (adapter.IsChecked("Bonjour") == Bonjour);

            TraceFactory.Logger.Info("LPD_Printing check: " + adapter.IsChecked("LPD_Printing"));
            TraceFactory.Logger.Info("DHCPv6 check: " + adapter.IsChecked("DHCPv6"));
            TraceFactory.Logger.Info("LLMNR check: " + adapter.IsChecked("LLMNR"));
            TraceFactory.Logger.Info("WS_Discovery check: " + adapter.IsChecked("WS_Discovery"));
            TraceFactory.Logger.Info("Bonjour check: " + adapter.IsChecked("Bonjour"));

            TraceFactory.Logger.Info("Result after verification is: " + result);

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;
        }

        /// <summary>
        /// ID: 401875
        /// Verification of Cancel button under Networking Tab
        /// 1) Connect the printer to the network and make sure printer gets up address.
        /// 2) Go to Networking-> Network Identification->Do some changes and Click on 'Cancel'
        /// 3) Go to Networking-> Advanced->Do some changes and Click on 'Cancel'
        /// </summary>
        public static bool TemplateCancelButtonLFP(EwsAdapter adapter)
        {
            bool result = false;
            string hostName = string.Empty;
            string IPV4domainName = string.Empty;
            string IPV6domainName = string.Empty;
            string IPV4DNSPrimary = string.Empty;
            string IPV4DNSSecondary = string.Empty;
            string IPV6DNSPrimary = string.Empty;
            string IPV6DNSSecondary = string.Empty;
            string winsPrimary = string.Empty;
            string winsSecondary = string.Empty;
            string bonjourServiceName = string.Empty;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            TraceFactory.Logger.Info("Navigate to network identification tab");
            adapter.Navigate("Network_Identification", "https");

            // Get the default values before editing
            hostName = adapter.GetValue("Host_Name");
            IPV4domainName = adapter.GetValue("DomainName_IPv4");
            IPV6domainName = adapter.GetValue("DomainName_IPv6");
            IPV4DNSPrimary = adapter.GetValue("DNS_IPv4_Primary");
            IPV4DNSSecondary = adapter.GetValue("DNS_IPv4_Secondary");
            IPV6DNSPrimary = adapter.GetValue("DNS_IPv6_Primary");
            IPV6DNSSecondary = adapter.GetValue("DNS_IPv6_Secondary");
            winsPrimary = adapter.GetValue("WINS_Primary");
            winsSecondary = adapter.GetValue("WINS_Secondary");
            bonjourServiceName = adapter.GetValue("Bonjour_Service_Name");

            // Edit the values
            TraceFactory.Logger.Info("Edit values for the various fields in the page");
            adapter.SetText("Host_Name", "test_hostname");
            adapter.SetText("DomainName_IPv4", "test_ipv4");
            adapter.SetText("DomainName_IPv6", "test_ipv6");
            adapter.SetText("DNS_IPv4_Primary", "test_ipv4primary");
            adapter.SetText("DNS_IPv4_Secondary", "test_ipv4secondary");
            adapter.SetText("DNS_IPv6_Primary", "test_ipv6primary");
            adapter.SetText("DNS_IPv6_Secondary", "test_ipv6secondary");
            adapter.SetText("WINS_Primary", "test_winsprimary");
            adapter.SetText("WINS_Secondary", "test_winssecondary");
            adapter.SetText("Bonjour_Service_Name", "test_bonjourservicename");

            TraceFactory.Logger.Info("Click the cancel button");
            adapter.Click("Cancel");

            Thread.Sleep(5000);

            TraceFactory.Logger.Info("Check whether the previous values where retained after \"Cancel\" was clicked");
            result = (adapter.GetValue("Host_Name") == hostName) &&
                     (adapter.GetValue("DomainName_IPv4") == IPV4domainName) &&
                     (adapter.GetValue("DomainName_IPv6") == IPV6domainName) &&
                     (adapter.GetValue("DNS_IPv4_Primary") == IPV4DNSPrimary) &&
                     (adapter.GetValue("DNS_IPv4_Secondary") == IPV4DNSSecondary) &&
                     (adapter.GetValue("DNS_IPv6_Primary") == IPV6DNSPrimary) &&
                     (adapter.GetValue("DNS_IPv6_Secondary") == IPV6DNSSecondary) &&
                     (adapter.GetValue("WINS_Primary") == winsPrimary) &&
                     (adapter.GetValue("WINS_Secondary") == winsSecondary) &&
                     (adapter.GetValue("Bonjour_Service_Name") == bonjourServiceName);

            TraceFactory.Logger.Info("validating whether the items are retained");
            TraceFactory.Logger.Info("Host_Name: " + adapter.GetValue("Host_Name"));
            TraceFactory.Logger.Info("DomainName_IPv4: " + adapter.GetValue("DomainName_IPv4"));
            TraceFactory.Logger.Info("DomainName_IPv6: " + adapter.GetValue("DomainName_IPv6"));
            TraceFactory.Logger.Info("DNS_IPv4_Primary: " + adapter.GetValue("DNS_IPv4_Primary"));
            TraceFactory.Logger.Info("DNS_IPv4_Secondary: " + adapter.GetValue("DNS_IPv4_Secondary"));
            TraceFactory.Logger.Info("DNS_IPv6_Primary: " + adapter.GetValue("DNS_IPv6_Primary"));
            TraceFactory.Logger.Info("DNS_IPv6_Secondary: " + adapter.GetValue("DNS_IPv6_Secondary"));
            TraceFactory.Logger.Info("WINS_Primary: " + adapter.GetValue("WINS_Primary"));
            TraceFactory.Logger.Info("WINS_Secondary: " + adapter.GetValue("WINS_Secondary"));
            TraceFactory.Logger.Info("Bonjour_Service_Name: " + adapter.GetValue("Bonjour_Service_Name"));


            TraceFactory.Logger.Info("Result after verification is: " + result);

            string idleTimeout = string.Empty;
            string LPDBanner = string.Empty;
            string sysContact = string.Empty;
            string sysLocation = string.Empty;
            string proxyServer = string.Empty;
            string proxyPort = string.Empty;
            string proxyUserName = string.Empty;
            string proxyPassport = string.Empty;
            string proxyException = string.Empty;
            string defaultIP = string.Empty;
            string DHCPRequest = string.Empty;
            string statelessDHCPv4 = string.Empty;
            string enableDHCPv4 = string.Empty;
            string SLP = string.Empty;
            string hopLimit = string.Empty;
            string TTL = string.Empty;
            string syslogServer = string.Empty;
            string syslogProtocol = string.Empty;
            string syslogPort = string.Empty;
            string syslogMaxMsg = string.Empty;
            string syslogPriority = string.Empty;
            string enableCCC = string.Empty;

            TraceFactory.Logger.Info("Navigate to advanced tab");
            adapter.Navigate("Advanced");

            // Get the values
            idleTimeout = adapter.GetValue("Idle_Timeout");
            sysContact = adapter.GetValue("System_Contact");
            sysLocation = adapter.GetValue("System_Location");
            defaultIP = adapter.GetValue("Default_IP");
            DHCPRequest = adapter.GetValue("Send_DHCP_Requests");
            SLP = adapter.GetValue("SLP_Client_Mode_Only");
            hopLimit = adapter.GetValue("Hop_Limit_WSD");
            TTL = adapter.GetValue("TTL_SLP");

            TraceFactory.Logger.Info("Edit data for the various fields in the tab");
            adapter.SetText("Idle_Timeout", "100");
            adapter.SetText("System_Contact", "test_syscontact");
            adapter.SetText("System_Location", "test_syslocation");
            adapter.SelectDropDown("Default_IP", "Legacy Default IP");
            adapter.Check("Send_DHCP_Requests");
            adapter.Check("SLP_Client_Mode_Only");
            adapter.SetText("Hop_Limit_WSD", "100");
            adapter.SetText("TTL_SLP", "100");

            TraceFactory.Logger.Info("CLick on cancel button");
            adapter.Click("Cancel");

            Thread.Sleep(5000);

            TraceFactory.Logger.Info("Verify if the previous values were retained");
            result = result && (adapter.GetValue("Idle_Timeout") == idleTimeout) &&
                               (adapter.GetValue("System_Contact") == sysContact) &&
                               (adapter.GetValue("System_Location") == sysLocation) &&
                               (adapter.GetValue("Default_IP") == defaultIP) &&
                               (adapter.GetValue("Send_DHCP_Requests") == DHCPRequest) &&
                               (adapter.GetValue("SLP_Client_Mode_Only") == SLP) &&
                               (adapter.GetValue("Hop_Limit_WSD") == hopLimit) &&
                               (adapter.GetValue("TTL_SLP") == TTL);

            TraceFactory.Logger.Info("validating whether the items are retained");
            TraceFactory.Logger.Info("Idle_Timeout: " + adapter.GetValue("Idle_Timeout"));
            TraceFactory.Logger.Info("System_Contact: " + adapter.GetValue("System_Contact"));
            TraceFactory.Logger.Info("System_Location: " + adapter.GetValue("System_Location"));
            TraceFactory.Logger.Info("Default_IP: " + adapter.GetValue("Default_IP"));
            TraceFactory.Logger.Info("Send_DHCP_Requests: " + adapter.GetValue("Send_DHCP_Requests"));
            TraceFactory.Logger.Info("SLP_Client_Mode_Only: " + adapter.GetValue("SLP_Client_Mode_Only"));
            TraceFactory.Logger.Info("Hop_Limit_WSD: " + adapter.GetValue("Hop_Limit_WSD"));
            TraceFactory.Logger.Info("TTL_SLP: " + adapter.GetValue("TTL_SLP"));
            TraceFactory.Logger.Info("Result after verification is: " + result);

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;
        }

        /// <summary>
        /// ID: 404135
        /// Verification of access of EWS page on various link speeds.
        /// 1. Connect to the printer.
        /// 2. Goto Networking -> Other Settings
        /// 3. Change Link Speed and click Apply
        /// 4. Check if EWS page is accesible
        /// </summary>
        public static bool TemplateLinkSpeedVEP(EwsAdapter adapter, string linkSpeed)
        {
            bool result = false;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));
            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");

            TraceFactory.Logger.Info("Navigating to Misc Settings page");
            adapter.Navigate("Misc_Settings", "https");

            TraceFactory.Logger.Info("Setting the link speed to : " + linkSpeed);
            adapter.SelectDropDown("Link_settings", linkSpeed);

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply_Misc");//ByName
            Thread.Sleep(10000);

            TraceFactory.Logger.Info("Navigating to Misc Settings page to check whether value is retained");
            adapter.Navigate("Misc_Settings");

            result = adapter.SearchText(linkSpeed);
            TraceFactory.Logger.Info("Result after verification is: " + result);

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();

            return result;
        }

        /// <summary>
        /// ID: 404135
        /// Verification of access of EWS page on various link speeds.
        /// 1. Connect to the printer.
        /// 2. Goto Networking -> Other Settings
        /// 3. Change Link Speed and click Apply
        /// 4. Check if EWS page is accesible
        /// </summary>
        public static bool TemplateLinkSpeedLFP(EwsAdapter adapter, string linkSpeed)
        {
            bool result = false;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");

            TraceFactory.Logger.Info("Navigating to Misc Settings page");
            adapter.Navigate("Misc_Settings", "https");
            TraceFactory.Logger.Info("Setting link speed to:" + linkSpeed);
            adapter.SelectDropDown("Link_settings", linkSpeed);

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(20000);

            TraceFactory.Logger.Info("Navigating to Misc Settings page");
            adapter.Navigate("Misc_Settings");
            Thread.Sleep(20000);
            TraceFactory.Logger.Info("Validating if previous link speed was set");
            result = adapter.SearchText(linkSpeed);

            TraceFactory.Logger.Info("Result after verification is: " + result);
            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();

            return result;
        }

        /// <summary>
        /// ID: 97190 (VEP)
        /// Verification of Enable\Disable of the device discovery services and verify the changes across management protocols
        /// 1. Connect to the printer.
        /// 2. Go to Networking-> Mgmt Protocol-> Other.
        /// 3. Enable\Disable WSD, SLP, Multicast IPV4 and Bonjour from the EWS page.
        /// 4. Check if changes are getting reflected
        /// </summary>
        public static bool TemplateDeviceDiscoveryVEP(EwsAdapter adapter)
        {
            bool result = false;
            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");
            TraceFactory.Logger.Info("Navigating to Management Protocol tab");
            adapter.Navigate("MngmtProtocol_Other", "https");

            TraceFactory.Logger.Info("Disabling Device Discovery Services");
            TraceFactory.Logger.Info("Unchecking SLP");
            adapter.Uncheck("SLP");

            TraceFactory.Logger.Info("Unchecking Bonjour");
            adapter.Uncheck("Bonjour");

            TraceFactory.Logger.Info("Unchecking Multicast IPv4");
            adapter.Uncheck("Multicast_IPv4");

            TraceFactory.Logger.Info("Unchecking WS Discovery");
            adapter.Uncheck("WS_Discovery");

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Navigate("Security_Status");
            Thread.Sleep(3000);

            TraceFactory.Logger.Info("Navigating to Management Protocol tab again to check if changes were retained");
            adapter.Navigate("MngmtProtocol_Other");


            result = (!adapter.IsChecked("SLP")) &&
                     (!adapter.IsChecked("Bonjour")) &&
                     (!adapter.IsChecked("Multicast_IPv4")) &&
                     (!adapter.IsChecked("WS_Discovery"));

            TraceFactory.Logger.Info("Validating the fields: ");
            TraceFactory.Logger.Info("SLP Uncheck: " + !adapter.IsChecked("SLP"));
            TraceFactory.Logger.Info("Bonjour Uncheck: " + !adapter.IsChecked("Bonjour"));
            TraceFactory.Logger.Info("Multicast_IPv4 Uncheck: " + !adapter.IsChecked("Multicast_IPv4"));
            TraceFactory.Logger.Info("WS_Discovery Uncheck: " + !adapter.IsChecked("WS_Discovery"));

            TraceFactory.Logger.Info("Result for disabling options is" + result);

            TraceFactory.Logger.Info("Enabling Device Discovery Services");
            TraceFactory.Logger.Info("Checking SLP");
            adapter.Check("SLP");

            TraceFactory.Logger.Info("Checking Bonjour");
            adapter.Check("Bonjour");

            TraceFactory.Logger.Info("Checking Multicast IPv4");
            adapter.Check("Multicast_IPv4");

            TraceFactory.Logger.Info("Checking WS Discovery");
            adapter.Check("WS_Discovery");

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Navigate("Security_Status");
            Thread.Sleep(3000);

            TraceFactory.Logger.Info("Navigating to Management Protocol tab again to check if changes are retained");
            adapter.Navigate("MngmtProtocol_Other");

            result = result && (adapter.IsChecked("SLP")) &&
                               (adapter.IsChecked("Bonjour")) &&
                               (adapter.IsChecked("Multicast_IPv4")) &&
                               (adapter.IsChecked("WS_Discovery"));

            TraceFactory.Logger.Info("Validating the fields: ");
            TraceFactory.Logger.Info("SLP Check: " + adapter.IsChecked("SLP"));
            TraceFactory.Logger.Info("Bonjour Check: " + adapter.IsChecked("Bonjour"));
            TraceFactory.Logger.Info("Multicast_IPv4 Check: " + adapter.IsChecked("Multicast_IPv4"));
            TraceFactory.Logger.Info("WS_Discovery Check: " + adapter.IsChecked("WS_Discovery"));

            TraceFactory.Logger.Info("Result for enabling options is" + result);

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();

            return result;
        }

        /// <summary>
        /// ID: 97190 (TPS)
        /// Verification of Enable\Disable of the device discovery services and verify the changes across management protocols
        /// 1. Connect to the printer.
        /// 2. Enable\Disable WSD, SLP and Bonjour from the EWS page.
        /// 3. Check if changes are getting reflected
        /// </summary>
        public static bool TemplateDeviceDiscoveryTPS(EwsAdapter adapter)
        {
            bool result = false;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");
            TraceFactory.Logger.Info("Navigating to Advanced tab");
            adapter.Navigate("Advanced", "https");

            TraceFactory.Logger.Info("Disabling Device Discovery Services");
            TraceFactory.Logger.Info("Unchecking SLP");
            adapter.Uncheck("SLP");

            TraceFactory.Logger.Info("Unchecking Bonjour");
            adapter.Uncheck("Bonjour");

            TraceFactory.Logger.Info("Unchecking WS Discovery");
            adapter.Uncheck("WS_Discovery");

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Navigate("Summary");
            Thread.Sleep(3000);

            TraceFactory.Logger.Info("Navigating to Advanced tab again to Check if changes for the modified values are retained");
            adapter.Navigate("Advanced");


            result = (!adapter.IsChecked("SLP")) &&
                     (!adapter.IsChecked("Bonjour")) &&
                     (!adapter.IsChecked("WS_Discovery"));

            TraceFactory.Logger.Info("Validating the fields: ");
            TraceFactory.Logger.Info("SLP Uncheck: " + !adapter.IsChecked("SLP"));
            TraceFactory.Logger.Info("Bonjour Uncheck: " + !adapter.IsChecked("Bonjour"));
            TraceFactory.Logger.Info("WS_Discovery Uncheck: " + !adapter.IsChecked("WS_Discovery"));

            TraceFactory.Logger.Info("Result for disabling options is" + result);

            TraceFactory.Logger.Info("Enabling Device Discovery Services");
            TraceFactory.Logger.Info("Checking SLP");
            adapter.Check("SLP");

            TraceFactory.Logger.Info("Checking Bonjour");
            adapter.Check("Bonjour");

            TraceFactory.Logger.Info("Checking WS Discovery");
            adapter.Check("WS_Discovery");

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Navigate("Summary");
            Thread.Sleep(3000);
            TraceFactory.Logger.Info("Navigating to Management Protocol tab to Check if changes for the modified values are retained");
            adapter.Navigate("Advanced");

            result = result && (adapter.IsChecked("SLP")) &&
                               (adapter.IsChecked("Bonjour")) &&
                               (adapter.IsChecked("WS_Discovery"));

            TraceFactory.Logger.Info("Validating the fields: ");
            TraceFactory.Logger.Info("SLP Check: " + adapter.IsChecked("SLP"));
            TraceFactory.Logger.Info("Bonjour Check: " + adapter.IsChecked("Bonjour"));
            TraceFactory.Logger.Info("WS_Discovery Check: " + adapter.IsChecked("WS_Discovery"));


            TraceFactory.Logger.Info("Result for Enabling options is" + result);
            adapter.Navigate("Summary");

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();

            return result;
        }

        /// <summary>
        /// ID: 97190 (LFP)
        /// Verification of Enable\Disable of the device discovery services and verify the changes across management protocols
        /// 1. Connect to the printer.
        /// 2. Go to Networking-> Mgmt Protocol-> Other.
        /// 3. Enable\Disable WSD, SLP, Multicast IPV4 and Bonjour from the EWS page.
        /// 4. Check if changes are getting reflected
        /// </summary>
        public static bool TemplateDeviceDiscoveryLFP(EwsAdapter adapter)
        {
            return TemplateDeviceDiscoveryVEP(adapter);
        }

        /// <summary>
        /// ID: 97188 (VEP)
        /// Verification of Enable\Disable of TCP Print Services
        /// 1. Connect to the printer. Networking -> Other Settings.
        /// 2. Enable\Disable P9100,LPD,FTP, WS-Print, IPP from the EWS page.
        /// 3. Check if changes are getting reflected
        /// </summary>
        public static bool TemplateTCPPrintServicesVEP(EwsAdapter adapter)
        {
            bool result = false;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));
            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");

            TraceFactory.Logger.Info("Navigating to Misc Settings tab");
            adapter.Navigate("Misc_Settings", "https");

            TraceFactory.Logger.Info("Disable options");
            adapter.Uncheck("9100_Printing");
            adapter.Uncheck("LPD_Printing");
            adapter.Uncheck("FTP_Printing");
            adapter.Uncheck("Web_Services_Print");
            adapter.Uncheck("IPP_Printing");

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Navigate("Security_Status");
            Thread.Sleep(3000);
            adapter.Navigate("Misc_Settings");

            TraceFactory.Logger.Info("Checking if changes have were retained");
            result = (!adapter.IsChecked("9100_Printing")) &&
                     (!adapter.IsChecked("LPD_Printing")) &&
                     (!adapter.IsChecked("FTP_Printing")) &&
                     (!adapter.IsChecked("Web_Services_Print")) &&
                     (!adapter.IsChecked("IPP_Printing"));

            TraceFactory.Logger.Info("9100_Printing Uncheck: " + !adapter.IsChecked("9100_Printing"));
            TraceFactory.Logger.Info("LPD_Printing Uncheck: " + !adapter.IsChecked("LPD_Printing"));
            TraceFactory.Logger.Info("FTP_Printing Uncheck: " + !adapter.IsChecked("FTP_Printing"));
            TraceFactory.Logger.Info("Web_Services_Print Uncheck: " + !adapter.IsChecked("Web_Services_Print"));
            TraceFactory.Logger.Info("IPP_Printing Uncheck: " + !adapter.IsChecked("IPP_Printing"));

            TraceFactory.Logger.Info("Result after verification is: " + result);

            TraceFactory.Logger.Info("Enabling Options");
            adapter.Check("9100_Printing");
            adapter.Check("LPD_Printing");
            adapter.Check("FTP_Printing");
            adapter.Check("Web_Services_Print");
            adapter.Check("IPP_Printing");

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Navigate("Security_Status");
            Thread.Sleep(3000);
            adapter.Navigate("Misc_Settings");

            TraceFactory.Logger.Info("Checking if changes were retained");
            result = result && (adapter.IsChecked("9100_Printing")) &&
                              (adapter.IsChecked("LPD_Printing")) &&
                              (adapter.IsChecked("FTP_Printing")) &&
                              (adapter.IsChecked("Web_Services_Print")) &&
                              (adapter.IsChecked("IPP_Printing"));

            TraceFactory.Logger.Info("9100_Printing Check: " + adapter.IsChecked("9100_Printing"));
            TraceFactory.Logger.Info("LPD_Printing Check: " + adapter.IsChecked("LPD_Printing"));
            TraceFactory.Logger.Info("FTP_Printing Check: " + adapter.IsChecked("FTP_Printing"));
            TraceFactory.Logger.Info("Web_Services_Print Check: " + adapter.IsChecked("Web_Services_Print"));
            TraceFactory.Logger.Info("IPP_Printing Check: " + adapter.IsChecked("IPP_Printing"));

            TraceFactory.Logger.Info("Result after verification is: " + result);


            adapter.Navigate("Security_Status");

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();

            return result;
        }

        /// <summary>
        /// ID: 97188 (TPS)
        /// Verification of Enable\Disable of TCP Print Services
        /// 1. Connect to the printer. Networking -> Advanced.
        /// 2. Enable\Disable LPD Printing
        /// 3. Check if changes are getting reflected
        /// </summary>
        public static bool TemplateTCPPrintServicesTPS(EwsAdapter adapter)
        {
            bool result = false;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));
            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");
            TraceFactory.Logger.Info("Navigating to Advanced tab");
            adapter.Navigate("Advanced", "https");

            TraceFactory.Logger.Info("Disabling LPD Printing option");
            adapter.Uncheck("LPD_Printing");

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Navigate("Summary");
            Thread.Sleep(3000);
            TraceFactory.Logger.Info("Navigating to Advanced tab again to check if changes are retained");
            adapter.Navigate("Advanced");

            result = (!adapter.IsChecked("LPD_Printing"));
            TraceFactory.Logger.Info("Result after verification is: " + result);

            TraceFactory.Logger.Info("Enabling LPD Printing option");
            adapter.Check("LPD_Printing");

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Navigate("Summary");
            Thread.Sleep(3000);

            TraceFactory.Logger.Info("Navigating to Advanced tab again to check if changes are retained");
            adapter.Navigate("Advanced");


            result = result && (adapter.IsChecked("LPD_Printing"));
            TraceFactory.Logger.Info("Result after verification is: " + result);

            adapter.Navigate("Summary");

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();

            return result;
        }

        /// <summary>
        /// ID: 97188 (LFP)
        /// Verification of Enable\Disable of TCP Print Services
        /// 1. Connect to the printer. Networking -> Other Settings.
        /// 2. Enable\Disable P9100,LPD,FTP, WS-Print, IPP from the EWS page.
        /// 3. Check if changes are getting reflected
        /// </summary>
        public static bool TemplateTCPPrintServicesLFP(EwsAdapter adapter)
        {
            bool result = false;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            TraceFactory.Logger.Info("Navigate to Misc Settings");
            adapter.Navigate("Misc_Settings", "https");

            TraceFactory.Logger.Info("Disable options");
            adapter.Uncheck("9100_Printing");
            adapter.Uncheck("LPD_Printing");
            adapter.Uncheck("FTP_Printing");
            adapter.Uncheck("Web_Services_Print");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Navigate("Security_Status");
            Thread.Sleep(3000);
            adapter.Navigate("Misc_Settings");

            TraceFactory.Logger.Info("Check if changes have are retained");
            result = (!adapter.IsChecked("9100_Printing")) &&
                     (!adapter.IsChecked("LPD_Printing")) &&
                     (!adapter.IsChecked("FTP_Printing")) &&
                     (!adapter.IsChecked("Web_Services_Print"));

            TraceFactory.Logger.Info("Enable Options");
            adapter.Check("9100_Printing");
            adapter.Check("LPD_Printing");
            adapter.Check("FTP_Printing");
            adapter.Check("Web_Services_Print");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Navigate("Security_Status");
            Thread.Sleep(3000);
            adapter.Navigate("Misc_Settings");

            TraceFactory.Logger.Info("Check if changes have are retained");
            result = result && (adapter.IsChecked("9100_Printing")) &&
                               (adapter.IsChecked("LPD_Printing")) &&
                               (adapter.IsChecked("FTP_Printing")) &&
                               (adapter.IsChecked("Web_Services_Print"));
            adapter.Navigate("Security_Status");

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();

            return result;
        }

        /// <summary>
        /// ID: 97192 (VEP)
        /// Verification of Enable\Disable of Naming Resolution Services
        /// 1. Connect the printer to the network and make sure printer gets ip address
        /// 2. Web into the printer and go to Networking--> Managment Protocols
        /// 3. Now Enable/Disable LLMNR ,WINS port and wins registration
        /// </summary>
        public static bool TemplateNamingResolutionVEP(EwsAdapter adapter)
        {
            bool result = false;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));
            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");
            TraceFactory.Logger.Info("Navigating to Management Protocol tab");
            adapter.Navigate("MngmtProtocol_Other", "https");

            TraceFactory.Logger.Info("Disabling Naming Resolution Services");
            TraceFactory.Logger.Info("Unchecking LLMNR");
            adapter.Uncheck("LLMNR");

            TraceFactory.Logger.Info("Unchecking WINS_Port");
            adapter.Uncheck("Enable_WINS_Port");

            TraceFactory.Logger.Info("Unchecking WINS_Registration");
            adapter.Uncheck("WINS_Registration");

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            TraceFactory.Logger.Info("Navigating to security status tab");
            adapter.Navigate("Security_Status");
            Thread.Sleep(3000);

            TraceFactory.Logger.Info("Navigating to Management Protocol tab again to check if changes are retained");
            adapter.Navigate("MngmtProtocol_Other");

            result = (!adapter.IsChecked("LLMNR")) &&
                     (!adapter.IsChecked("Enable_WINS_Port")) &&
                     (!adapter.IsChecked("WINS_Registration"));

            TraceFactory.Logger.Info("LLMNR Uncheck: " + !adapter.IsChecked("LLMNR"));
            TraceFactory.Logger.Info("Enable_WINS_Port Uncheck: " + !adapter.IsChecked("Enable_WINS_Port"));
            TraceFactory.Logger.Info("WINS_Registration Uncheck: " + !adapter.IsChecked("WINS_Registration"));

            TraceFactory.Logger.Info("Result after verification is: " + result);

            TraceFactory.Logger.Info("Enabling Naming Resolution Services");
            TraceFactory.Logger.Info("Checking LLMNR");
            adapter.Check("LLMNR");

            TraceFactory.Logger.Info("Checking WINS_Port");
            adapter.Check("Enable_WINS_Port");

            TraceFactory.Logger.Info("Checking WINS_Registration");
            adapter.Check("WINS_Registration");

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Navigate("Security_Status");
            Thread.Sleep(3000);

            TraceFactory.Logger.Info("Navigating to Management Protocol tab again to check if changes are retained");
            adapter.Navigate("MngmtProtocol_Other");
            result = result && (adapter.IsChecked("LLMNR")) &&
                               (adapter.IsChecked("Enable_WINS_Port")) &&
                               (adapter.IsChecked("WINS_Registration"));

            TraceFactory.Logger.Info("LLMNR Check: " + adapter.IsChecked("LLMNR"));
            TraceFactory.Logger.Info("Enable_WINS_Port Check: " + adapter.IsChecked("Enable_WINS_Port"));
            TraceFactory.Logger.Info("WINS_Registration Check: " + adapter.IsChecked("WINS_Registration"));

            TraceFactory.Logger.Info("Result after verification is: " + result);

            adapter.Navigate("Summary");

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();

            return result;
        }

        /// <summary>
        /// ID: 97192 (TPS)
        /// Verification of Enable\Disable of Naming Resolution Services
        /// 1. Connect to the printer. Networking -> Mngmt Protocols
        /// 2. Enable\Disable LLMNR
        /// 3. Check if changes are getting reflected
        /// </summary>
        public static bool TemplateNamingResolutionTPS(EwsAdapter adapter)
        {
            bool result = false;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));
            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");
            TraceFactory.Logger.Info("Navigating to Advanced tab");
            adapter.Navigate("Advanced", "https");

            TraceFactory.Logger.Info("Disabling Naming Resolution Services");
            TraceFactory.Logger.Info("Disabling LLMNR");
            adapter.Uncheck("LLMNR");

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Navigate("Summary");
            Thread.Sleep(3000);
            TraceFactory.Logger.Info("Navigating to Advanced tab again to check if changes are retained");
            adapter.Navigate("Advanced");

            result = (!adapter.IsChecked("LLMNR"));
            TraceFactory.Logger.Info("Result after verification is: " + result);

            TraceFactory.Logger.Info("Enabling Naming Resolution Services");
            TraceFactory.Logger.Info("Enabling LLMNR");
            adapter.Check("LLMNR");

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Navigate("Summary");
            Thread.Sleep(3000);
            adapter.Navigate("Advanced");

            TraceFactory.Logger.Info("Navigating to Advanced tab again to check if changes are retained");

            result = result && (adapter.IsChecked("LLMNR"));
            TraceFactory.Logger.Info("Result after verification is: " + result);

            adapter.Navigate("Summary");

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();

            return result;
        }

        /// <summary>
        /// ID: 97192 (LFP)
        /// Verification of Enable\Disable of Naming Resolution Services
        /// 1. Connect the printer to the network and make sure printer gets ip address
        /// 2. Web into the printer and go to Networking--> Managment Protocols
        /// 3. Now Enable/Disable LLMNR ,WINS port and wins registration
        /// </summary>
        public static bool TemplateNamingResolutionLFP(EwsAdapter adapter)
        {
            return TemplateNamingResolutionVEP(adapter);
        }

        /// <summary>
        /// ID: 406719 (VEP)
        /// Verification of Enable Disable of IPV6 protocol
        /// 1) Connect the printer to the network and make sure printer gets IP address.
        /// 2) Go to Networking-<IPV6 tab >->Enable/Disable IPV6
        /// 3) Try to  access EWS page and also try to navigate  through all the Networking tabs
        /// </summary>>
        public static bool TemplateIPv6VEP(EwsAdapter adapter)
        {
            //bool result = false; 

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));
            TraceFactory.Logger.Info("Navigate to TCPIP tab");
            adapter.Navigate("TCPIPv6", "https");

            TraceFactory.Logger.Info("Enable IPv6");
            adapter.Click("IPv6_Enable");
            adapter.Click("Apply");

            adapter.Navigate("TCPIPv6");
            adapter.Click("IPv6_Enable");

            adapter.Click("Apply");

            adapter.Navigate("Summary");
            adapter.Navigate("Network_Identification");
            adapter.Navigate("IPv4_Config");
            adapter.Navigate("TCPIPv6");
            adapter.Navigate("Config_Preference");
            adapter.Navigate("Advanced");

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();

            return true;
        }

        /// <summary>
        /// ID: 406719 (TPS)
        /// Verification of Enable Disable of IPV6 protocol
        /// 1) Connect the printer to the network and make sure printer gets IP address.
        /// 2) Go to Networking-<IPV6 tab >->Enable/Disable IPV6
        /// 3) Try to  access EWS page and also try to navigate  through all the Networking tabs
        /// </summary>>
        public static bool TemplateIPv6TPS(EwsAdapter adapter)
        {
            bool result = false;
            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            adapter.Navigate("Advanced", "https");

            adapter.Click("IPv6");
            adapter.Click("Apply");

            adapter.Navigate("Advanced");
            adapter.Click("IPv6");
            adapter.Click("Apply");

            adapter.Navigate("Summary");
            adapter.Navigate("Network_Identification");
            adapter.Navigate("IPv4_Config");
            adapter.Navigate("TCPIPv6");
            adapter.Navigate("Config_Preference");
            adapter.Navigate("Advanced");

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            result = true;
            return result;
        }

        /// <summary>
        /// ID: 406719 (LFP)
        /// Verification of Enable Disable of IPV6 protocol
        /// 1) Connect the printer to the network and make sure printer gets IP address.
        /// 2) Go to Networking-<IPV6 tab >->Enable/Disable IPV6
        /// 3) Try to  access EWS page and also try to navigate  through all the Networking tabs
        /// </summary>>
        public static bool TemplateIPv6LFP(EwsAdapter adapter)
        {
            return TemplateIPv6VEP(adapter);
        }

        /// <summary>
        /// ID: 406716
        /// Verify configuration of parameters in Network Identification tab
        /// 1) Connect the printer to the network and make sure printer gets ip address.
        /// 2) Go to Networking->Network Identification-> Modify Host Name, Domain Name (IPv4/IPv6),
        /// Domain Name (IPv6 only), Configuration Precedence, DNS Suffix, Bonjour Service Name,WINS and
        /// Bonjour Highest Priority Service and click on Apply
        /// 3) Web into the printer and validate the Network Identification tab and Network Summary tab
        /// </summary>
        public static bool TemplateNetworkIdentificationVEP(EwsAdapter adapter)
        {
            string hostName = string.Empty;
            string IPV4domainName = string.Empty;
            string IPV6domainName = string.Empty;
            string IPV4DNSPrimary = string.Empty;
            string IPV4DNSSecondary = string.Empty;
            string IPV6DNSPrimary = string.Empty;
            string IPV6DNSSecondary = string.Empty;
            string winsPrimary = string.Empty;
            string winsSecondary = string.Empty;
            string bonjourServiceName = string.Empty;
            string newhostName = string.Empty;
            string newIPV4domainName = string.Empty;
            string newIPV6domainName = string.Empty;
            string newIPV4DNSPrimary = string.Empty;
            string newIPV4DNSSecondary = string.Empty;
            string newIPV6DNSPrimary = string.Empty;
            string newIPV6DNSSecondary = string.Empty;
            string newwinsPrimary = string.Empty;
            string newwinsSecondary = string.Empty;
            string newbonjourServiceName = string.Empty;
            bool result = false;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));
            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");
            TraceFactory.Logger.Info("Navigating to Network Identification tab");
            adapter.Navigate("Network_Identification", "https");

            TraceFactory.Logger.Info("Getting the default values of HostName,IPv4 DomainName, IPv6 DomainName,Bonjour Service Name before editing");
            try
            {
                hostName = adapter.GetValue("HostName");
                IPV4domainName = adapter.GetValue("DomainName_IPv4");
                IPV6domainName = adapter.GetValue("DomainName_IPv6");
                bonjourServiceName = adapter.GetValue("Bonjour_Service_Name");
            }
            catch (Exception e)
            {
                TraceFactory.Logger.Info("Getting Null Value exception:" + e.Message);
                return false;
            }
            TraceFactory.Logger.Info("Setting the new values for HostName->test_hostname, IPv4 DomainName->test_ipv4, IPv6 DomainName->test_ipv6, Bonjour Service Name->test_bonjourservicename");
            newhostName = "test_hostname";
            newIPV4domainName = "test_ipv4";
            newIPV6domainName = "test_ipv6";
            newbonjourServiceName = "test_bonjourservicename";

            adapter.SetText("HostName", newhostName);
            adapter.SetText("DomainName_IPv4", newIPV4domainName);
            adapter.SetText("DomainName_IPv6", newIPV6domainName);
            adapter.SetText("Bonjour_Service_Name", newbonjourServiceName);

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");

            Thread.Sleep(3000);

            TraceFactory.Logger.Info("Navigating to Network Identification tab to Check whether the previous values are updated with latest values");
            adapter.Navigate("Network_Identification");

            result = (adapter.GetValue("HostName") == newhostName) &&
                     (adapter.GetValue("DomainName_IPv4") == newIPV4domainName) &&
                     (adapter.GetValue("DomainName_IPv6") == newIPV6domainName) &&
                     (adapter.GetValue("Bonjour_Service_Name") == newbonjourServiceName);

            TraceFactory.Logger.Info("HostName: " + adapter.GetValue("HostName"));
            TraceFactory.Logger.Info("DomainName_IPv4: " + adapter.GetValue("DomainName_IPv4"));
            TraceFactory.Logger.Info("DomainName_IPv6: " + adapter.GetValue("DomainName_IPv6"));
            TraceFactory.Logger.Info("Bonjour_Service_Name: " + adapter.GetValue("Bonjour_Service_Name"));

            TraceFactory.Logger.Info("Result after verification is: " + result);

            TraceFactory.Logger.Info("Setting to former values for HostName,IPv4 DomainName, IPv6 DomainName,Bonjour Service Name");
            adapter.SetText("HostName", hostName);
            adapter.SetText("DomainName_IPv4", IPV4domainName);
            adapter.SetText("DomainName_IPv6", IPV6domainName);
            adapter.SetText("Bonjour_Service_Name", bonjourServiceName);

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;
        }

        /// <summary>
        /// ID: 406716
        /// Verify configuration of parameters in Network Identification tab
        /// 1) Connect the printer to the network and make sure printer gets ip address.
        /// 2) Go to Networking->Network Identification-> Modify Host Name, Domain Name (IPv4/IPv6),
        /// Domain Name (IPv6 only), Configuration Precedence, DNS Suffix, Bonjour Service Name,WINS and
        /// Bonjour Highest Priority Service and click on Apply
        /// 3) Web into the printer and validate the Network Identification tab and Network Summary tab
        /// </summary>
        public static bool TemplateNetworkIdentificationLFP(EwsAdapter adapter)
        {
            string hostName = string.Empty;
            string IPV4domainName = string.Empty;
            string IPV6domainName = string.Empty;
            string IPV4DNSPrimary = string.Empty;
            string IPV4DNSSecondary = string.Empty;
            string IPV6DNSPrimary = string.Empty;
            string IPV6DNSSecondary = string.Empty;
            string winsPrimary = string.Empty;
            string winsSecondary = string.Empty;
            string bonjourServiceName = string.Empty;
            string newhostName = string.Empty;
            string newIPV4domainName = string.Empty;
            string newIPV6domainName = string.Empty;
            string newIPV4DNSPrimary = string.Empty;
            string newIPV4DNSSecondary = string.Empty;
            string newIPV6DNSPrimary = string.Empty;
            string newIPV6DNSSecondary = string.Empty;
            string newwinsPrimary = string.Empty;
            string newwinsSecondary = string.Empty;
            string newbonjourServiceName = string.Empty;
            bool result = false;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            adapter.Navigate("Network_Identification", "https");

            TraceFactory.Logger.Info("Get the default values before editing");
            hostName = adapter.GetValue("Host_Name");
            IPV4domainName = adapter.GetValue("DomainName_IPv4");
            IPV6domainName = adapter.GetValue("DomainName_IPv6");
            bonjourServiceName = adapter.GetValue("Bonjour_Service_Name");

            TraceFactory.Logger.Info("Set new values");
            newhostName = "test_hostname";
            newIPV4domainName = "test_ipv4";
            newIPV6domainName = "test_ipv6";

            newbonjourServiceName = "test_bonjourservicename";

            TraceFactory.Logger.Info("Edit the values");
            adapter.SetText("Host_Name", newhostName);
            adapter.SetText("DomainName_IPv4", newIPV4domainName);
            adapter.SetText("DomainName_IPv6", newIPV6domainName);
            adapter.SetText("Bonjour_Service_Name", newbonjourServiceName);

            adapter.Click("Apply");

            Thread.Sleep(3000);

            adapter.Navigate("Network_Identification");

            TraceFactory.Logger.Info("Check whether the previous values are updated with latest values");
            result = (adapter.GetValue("Host_Name") == newhostName) &&
                     (adapter.GetValue("DomainName_IPv4") == newIPV4domainName) &&
                     (adapter.GetValue("DomainName_IPv6") == newIPV6domainName) &&
                     (adapter.GetValue("Bonjour_Service_Name") == newbonjourServiceName);

            TraceFactory.Logger.Info("Setting to former values");
            adapter.SetText("Host_Name", hostName);
            adapter.SetText("DomainName_IPv4", IPV4domainName);
            adapter.SetText("DomainName_IPv6", IPV6domainName);
            adapter.SetText("Bonjour_Service_Name", bonjourServiceName);

            adapter.Click("Apply");

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;
        }

        /// <summary>
        /// ID: 406716
        /// Verify configuration of parameters in Network Identification tab
        /// 1) Connect the printer to the network and make sure printer gets ip address.
        /// 2) Go to Networking->Network Identification-> Modify Host Name, Domain Name (IPv4/IPv6),
        /// Domain Name (IPv6 only), Configuration Precedence, DNS Suffix, Bonjour Service Name,WINS and
        /// Bonjour Highest Priority Service and click on Apply
        /// 3) Go to Network Identification->click on 'Restore Default'
        /// </summary>
        public static bool TemplateNetworkIdentificationTPS(EwsAdapter adapter)
        {
            string hostName = string.Empty;
            string IPV4domainName = string.Empty;
            string newhostName = string.Empty;
            string newIPV4domainName = string.Empty;
            bool result = false;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");
            TraceFactory.Logger.Info("Navigating to Network Identification");
            adapter.Navigate("Network_Identification", "https");

            TraceFactory.Logger.Info("Getting the default values of Host Name & Domain Name before editing");
            hostName = adapter.GetValue("HostName");
            IPV4domainName = adapter.GetValue("Domain_Name_IPv4_IPv6");

            TraceFactory.Logger.Info("Setting New values for Host Name & IPv4Domain i.e., test_hostname & test_ipv4 respectively");
            newhostName = "test_hostname";
            newIPV4domainName = "test_ipv4";

            adapter.SetText("HostName", newhostName);
            adapter.SetText("Domain_Name_IPv4_IPv6", newIPV4domainName);

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(15000);

            TraceFactory.Logger.Info("Navigating to Network Identification to check whether the previous values are updated with latest values");
            adapter.Navigate("Network_Identification");

            result = (adapter.GetValue("HostName") == newhostName) &&
                     (adapter.GetValue("Domain_Name_IPv4_IPv6") == newIPV4domainName);

            TraceFactory.Logger.Info("HostName: " + adapter.GetValue("HostName"));
            TraceFactory.Logger.Info("Domain_Name_IPv4_IPv6: " + adapter.GetValue("Domain_Name_IPv4_IPv6"));

            TraceFactory.Logger.Info("Result after verification is: " + result);

            TraceFactory.Logger.Info("Restoring old values");
            adapter.SetText("HostName", hostName);
            adapter.SetText("Domain_Name_IPv4_IPv6", IPV4domainName);

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(15000);

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;
        }

        /// <summary>
        /// ID: 406717_a (VEP)
        /// Verify configuration of system and support information
        /// 1) Connect the printer to the network and make sure printer gets ip address.
        /// 2) Got to Tcp/Ip settings Tab ->Modify  system contact, system location
        /// 3) go to other setting tab->support->Support info tab
        /// 4) Change the values and click on apply
        /// </summary>
        public static bool TemplateSystemAndSupportInformation1VEP(EwsAdapter adapter)
        {
            string systemContact = string.Empty;
            string systemLocation = string.Empty;
            string supportContact = string.Empty;
            string supportPhoneNumber = string.Empty;
            string newsystemContact = string.Empty;
            string newsystemLocation = string.Empty;
            string newsupportContact = string.Empty;
            string newsupportPhoneNumber = string.Empty;
            bool result = false;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            newsystemContact = "David";
            newsystemLocation = "California";
            newsupportContact = "Mary";
            newsupportPhoneNumber = "123-456-7890";

            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");
            TraceFactory.Logger.Info("Navigating to Advanced tab");
            adapter.Navigate("Advanced", "https");

            systemContact = adapter.GetValue("System_Contact");
            systemLocation = adapter.GetValue("System_Location");

            TraceFactory.Logger.Info("Getting the default value of System Contact & the value is " + systemContact);
            TraceFactory.Logger.Info("Getting the default value of System Location & the value is " + systemContact);

            TraceFactory.Logger.Info("Entering New System Contact:" + newsystemContact);
            TraceFactory.Logger.Info("Entering New System Location:" + newsystemLocation);

            adapter.SetText("System_Contact", newsystemContact);
            adapter.SetText("System_Location", newsystemLocation);

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Navigate("Summary");

            TraceFactory.Logger.Info("Navigating to Advanced tab to check whether the values are retained");
            adapter.Navigate("Advanced");

            Thread.Sleep(3000);
            TraceFactory.Logger.Info("System Contact value is:" + adapter.GetValue("System_Contact"));
            TraceFactory.Logger.Info("System Location value is:" + adapter.GetValue("System_Location"));

            result = (adapter.GetValue("System_Contact") == newsystemContact) &&
                     (adapter.GetValue("System_Location") == newsystemLocation);

            TraceFactory.Logger.Info("Result after verification is:" + result);

            TraceFactory.Logger.Info("Setting default System contact Value:" + systemContact);
            TraceFactory.Logger.Info("Setting default System Location Value:" + systemLocation);

            adapter.SetText("System_Contact", systemContact);
            adapter.SetText("System_Location", systemLocation);

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            //Support Info verification
            TraceFactory.Logger.Info("Navigating to Support Info tab");
            adapter.Navigate("Support_Info");

            supportContact = adapter.GetValue("Support_Contact");
            supportPhoneNumber = adapter.GetValue("Support_Phone_Number");

            TraceFactory.Logger.Info("Getting default System Contact Value:" + supportContact);
            TraceFactory.Logger.Info("Getting default System Phone Number Value:" + supportPhoneNumber);

            TraceFactory.Logger.Info("Setting New System Contact Value:" + newsupportContact);
            TraceFactory.Logger.Info("Setting New System Phone Number Value:" + newsupportPhoneNumber);

            adapter.SetText("Support_Contact", newsupportContact);
            adapter.SetText("Support_Phone_Number", newsupportPhoneNumber);

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Navigate("Summary");
            TraceFactory.Logger.Info("Navigating to Support Info tab to check whether the values are retained");
            adapter.Navigate("Support_Info");

            Thread.Sleep(3000);

            result = result && (adapter.GetValue("Support_Contact") == newsupportContact) &&
                               (adapter.GetValue("Support_Phone_Number") == newsupportPhoneNumber);

            TraceFactory.Logger.Info("Getting New System Contact Value:" + adapter.GetValue("Support_Contact"));
            TraceFactory.Logger.Info("Getting New System Phone Number Value:" + adapter.GetValue("Support_Phone_Number"));
            TraceFactory.Logger.Info("Result after verification is: " + result);

            adapter.SetText("Support_Contact", supportContact);
            adapter.SetText("Support_Phone_Number", supportPhoneNumber);

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;
        }


        /// <summary>
        /// ID: 406717_a (LFP)
        /// Verify configuration of system and support information
        /// 1) Connect the printer to the network and make sure printer gets ip address.
        /// 2) Got to Tcp/Ip settings Tab ->Modify  system contact, system location
        /// 3) go to other setting tab->support->Support info tab
        /// 4) Change the values and click on apply
        /// </summary>
        public static bool TemplateSystemAndSupportInformation1LFP(EwsAdapter adapter)
        {
            return TemplateSystemAndSupportInformation1VEP(adapter);
        }

        /// <summary>
        /// ID: 406717_b (VEP)
        /// Reset- Access of System and support info tab and validate the contents
        /// 1) Connect the printer to the network and make sure printer gets ip address.
        /// 2) go to other setting tab->support->Support info tab
        /// 3) Change the values and click on restore default
        /// </summary>
        public static bool TemplateSystemAndSupportInformation2VEP(EwsAdapter adapter)
        {
            string newsupportContact = string.Empty;
            string newsupportPhoneNumber = string.Empty;
            bool result = false;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");

            newsupportContact = "Mary";
            newsupportPhoneNumber = "123-456-7890";

            TraceFactory.Logger.Info("Navigating to Support Info tab");
            adapter.Navigate("Support_Info", "https");

            TraceFactory.Logger.Info("Setting New Support Contact Value:" + newsupportContact);
            TraceFactory.Logger.Info("Setting New Support Phone Number Value:" + newsupportPhoneNumber);

            adapter.SetText("Support_Contact", newsupportContact);
            adapter.SetText("Support_Phone_Number", newsupportPhoneNumber);

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            adapter.Navigate("Summary");
            TraceFactory.Logger.Info("Navigating to Support Info tab to check whether the values are retained");
            adapter.Navigate("Support_Info");

            Thread.Sleep(3000);

            // 1) The browser should retrieve the requested page and display it properly.
            result = (adapter.GetValue("Support_Contact") == newsupportContact) &&
                     (adapter.GetValue("Support_Phone_Number") == newsupportPhoneNumber);

            TraceFactory.Logger.Info("Getting New Support Contact Value:" + adapter.GetValue("Support_Contact"));
            TraceFactory.Logger.Info("Getting New Support Phone Number Value:" + adapter.GetValue("Support_Phone_Number"));
            TraceFactory.Logger.Info("Result after verification is: " + result);

            // 2) On click on Restore default button, all the setting should get reflected to factory default values.
            adapter.Navigate("Support_Info");
            adapter.Click("Restore_Defaults");

            adapter.Navigate("Support_Info");
            Thread.Sleep(3000);
            result = result && (adapter.GetValue("Support_Contact") == "") &&
                               (adapter.GetValue("Support_Phone_Number") == "");

            TraceFactory.Logger.Info("Result after verification is: " + result);

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;
        }

        /// <summary>
        /// ID: 406717_b (LFP)
        /// Reset- Access of System and support info tab and validate the contents
        /// 1) Connect the printer to the network and make sure printer gets ip address.
        /// 2) go to other setting tab->support->Support info tab
        /// 3) Change the values and click on restore default
        /// </summary>
        public static bool TemplateSystemAndSupportInformation2LFP(EwsAdapter adapter)
        {
            return TemplateSystemAndSupportInformation2VEP(adapter);
        }

        /// <summary>
        /// ID: 404141 (VEP)
        /// 1) Connect the printer to the network and make sure printer gets up address.
        /// 2) Go to Network tab->Security settings ->Mgmt . protocol ->
        /// Change the  Encryption Strength (Low,  Medium,High)-> Click onApply
        /// 3) Open a browser and request a page from the device under test.
        /// </summary>
        public static bool TemplateEncryptionStrengthVEP(EwsAdapter adapter, string encryptionStrength)
        {
            bool result = false;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));
            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");
            TraceFactory.Logger.Info("Navigating to Management Protocol Web Management tab");
            adapter.Navigate("MngmtProtocol_WebMngmt", "https");

            TraceFactory.Logger.Info("Encryption Strength set to " + encryptionStrength);
            adapter.SelectDropDown("EncryptionStrength", encryptionStrength);

            TraceFactory.Logger.Info("Clicking Apply button");
            adapter.Click("ApplySecurity");//ByName
            adapter.Wait(TimeSpan.FromSeconds(5));


            TraceFactory.Logger.Info("Navigating to Summary page");
            adapter.Navigate("Summary");

            result = (adapter.SearchText("Summary"));
            TraceFactory.Logger.Info("Result for searching summary text is " + result);

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;
        }

        /// <summary>
        /// ID: 404141 (LFP)
        /// 1) Connect the printer to the network and make sure printer gets up address.
        /// 2) Go to Network tab->Security settings ->Mgmt . protocol ->
        /// Change the  Encryption Strength (Low,  Medium,High)-> Click onApply
        /// 3) Open a browser and request a page from the device under test.
        /// </summary>
        public static bool TemplateEncryptionStrengthLFP(EwsAdapter adapter, string encryptionStrength)
        {
            return TemplateEncryptionStrengthVEP(adapter, encryptionStrength);
        }

        /// <summary>
        /// ID: 97191
        /// Enable and disable of Management Services_VEP
        /// 1. Connect the printer to the network and make sure printer gets ip address.
        /// 2. Go to Networking->mgmt. protocols >> other
        /// 3. Enable/ Disable Telnet, TFTP, HP jetDirect XML service and Certificate managment service
        /// </summary>
        public static bool TemplateManagementServicesVEP(EwsAdapter adapter)
        {
            bool result = false;
            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to Networking tab");
            TraceFactory.Logger.Info("Navigating to Management Protocol tab");
            adapter.Navigate("MngmtProtocol_Other", "https");

            TraceFactory.Logger.Info("Disable Telnet, HP Jetdirect XML Services, TFTP Configuration File and Certificate Mgmt Service");
            adapter.Uncheck("Enable_Telnet");
            adapter.Uncheck("HP_Jetdirect_XML_Services");
            adapter.Uncheck("TFTP_Configuration_File");
            adapter.Uncheck("Certificate_Mgmt_Service");

            TraceFactory.Logger.Info("Apply the changes by clicking on Apply button");
            adapter.Click("Apply"); //ByName

            adapter.Navigate("Security_Status");

            TraceFactory.Logger.Info("Navigate again to Networking-> Mgmt. protocols-> Other");
            adapter.Navigate("MngmtProtocol_Other");

            result = (!adapter.IsChecked("Enable_Telnet")) &&
            (!adapter.IsChecked("HP_Jetdirect_XML_Services")) &&
            (!adapter.IsChecked("TFTP_Configuration_File")) &&
            (!adapter.IsChecked("Certificate_Mgmt_Service"));

            TraceFactory.Logger.Info("Enable Telnet, HP Jetdirect XML Services, TFTP Configuration File and Certificate Mgmt Service");
            adapter.Check("Enable_Telnet");
            adapter.Check("HP_Jetdirect_XML_Services");
            adapter.Check("TFTP_Configuration_File");
            adapter.Check("Certificate_Mgmt_Service");

            TraceFactory.Logger.Info("Apply the changes by clicking on Apply button");
            adapter.Click("Apply"); //ByName

            adapter.Navigate("Security_Status");

            TraceFactory.Logger.Info("Navigate again to Networking-> Mgmt. protocols-> Other");
            adapter.Navigate("MngmtProtocol_Other");

            result = result && (adapter.IsChecked("Enable_Telnet")) &&
            (adapter.IsChecked("HP_Jetdirect_XML_Services")) &&
            (adapter.IsChecked("TFTP_Configuration_File")) &&
            (adapter.IsChecked("Certificate_Mgmt_Service"));

            TraceFactory.Logger.Info("Close the browser");
            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();

            return result;
        }

        /// <summary>
        /// ID: 97191
        /// Enable and disable of Management Services_VEP
        /// 1. Connect the printer to the network and make sure printer gets ip address.
        /// 2. Go to Networking->mgmt. protocols >> other
        /// 3. Enable/ Disable Telnet, TFTP, HP jetDirect XML service and Certificate managment service
        /// </summary>
        public static bool TemplateManagementServicesLFP(EwsAdapter adapter)
        {
            bool result = false;
            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            adapter.Navigate("MngmtProtocol_Other", "https");

            TraceFactory.Logger.Info("Disable Telnet, HP Jetdirect XML Services, TFTP Configuration File and Certificate Mgmt Service");
            adapter.Click("Enable_Telnet");
            adapter.Click("HP_Jetdirect_XML_Services");
            adapter.Click("Certificate_Mgmt_Service");

            TraceFactory.Logger.Info("Apply the changes by clicking on Apply button");
            adapter.Click("Apply");

            adapter.Navigate("Security_Status");

            TraceFactory.Logger.Info("Navigate again to Networking-> Mgmt. protocols-> Other");
            adapter.Navigate("MngmtProtocol_Other");

            TraceFactory.Logger.Info("Enable Telnet, HP Jetdirect XML Services, TFTP Configuration File and Certificate Mgmt Service");
            adapter.Click("Enable_Telnet");
            adapter.Click("HP_Jetdirect_XML_Services");
            adapter.Click("Certificate_Mgmt_Service");

            TraceFactory.Logger.Info("Apply the changes by clicking on Apply button");
            adapter.Click("Apply");

            TraceFactory.Logger.Info("Close the browser");
            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;
        }

        /// <summary>
        /// ID: 401869 (VEP)
        /// Access of Device through HTTPS
        /// 1. Open WebUI
        /// 2. Go to Networking -> Mgmt Protocol
        /// 3. Enable Encrypt All Web Communication
        /// 4. Navigate through all the tabs under networking
        /// 5. Go to Networking -> Mgmt Protocol
        /// 6. Disable Encrypt All Web Communication
        /// 7. Open a new browser and try to access through HTTP
        /// </summary>
        public static bool Template_DeviceAccessHTTPS_VEP(EwsAdapter adapter, EwsActivityData _activityData)
        {
            string[] tabs = new string[] { "Network_Settings", "Misc_Settings", "Language", "Security_Status",
                                           "Admin_Account", "802.1x_Authentication", "IPsec_Firewall", "Network_Statistics",
                                           "ProtocolInfo_TCPIP", "Configuration_Page", "MngmtProtocol_WebMngmt", "Summary"
                                         };

            string[] tabTitles = new string[] {"Network Settings", "Other Settings", "Language", "Status",
                                           "Authorization", "802.1X Authentication", "IPsec/Firewall Policy", "Network Statistics",
                                           "Protocol Info", "Jetdirect Configuration Page", "Mgmt. Protocols", "TCP/IP Settings"
                                         };
            bool result = true; adapter.Start(); adapter.Wait(TimeSpan.FromSeconds(5));

            // Enable Encrypt All Web Communication
            TraceFactory.Logger.Info("Navigating to Management protocol");
            adapter.Navigate("MngmtProtocol_WebMngmt", "https");

            TraceFactory.Logger.Info("Enabling https");
            adapter.Check("Encrypt_All");

            adapter.Click("ApplySecurity");
            Thread.Sleep(3000);
            int i = 0;

            // Navigate to each tab on the networking page
            foreach (string tab in tabs)
            {
                adapter.Navigate(tab, "https");
                result = result && adapter.SearchText(tabTitles[i]);
                TraceFactory.Logger.Info("Navigating to " + tab + " protocol:" + result);
                i++;
            }

            // Disable Encrypt All Web Communication
            TraceFactory.Logger.Info("Navigating to Management protocol");
            adapter.Navigate("MngmtProtocol_WebMngmt");

            TraceFactory.Logger.Info("Disabling https");
            adapter.Uncheck("Encrypt_All");
            adapter.Click("ApplySecurity");
            Thread.Sleep(3000);

            // Navigate to each tab on the networking page
            i = 0;
            foreach (string tab in tabs)
            {
                adapter.Navigate(tab);
                result = result && adapter.SearchText(tabTitles[i]);
                TraceFactory.Logger.Info("Navigating to " + tab + " protocol:" + result);
                i++;
            }

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;
        }


        /// <summary>
        /// ID: 401869 (LFP)
        /// Access of Device through HTTPS
        /// 1. Open WebUI
        /// 2. Go to Networking -> Mgmt Protocol
        /// 3. Enable Encrypt All Web Communication
        /// 4. Navigate through all the tabs under networking
        /// 5. Go to Networking -> Mgmt Protocol
        /// 6. Disable Encrypt All Web Communication
        /// 7. Open a new browser and try to access through HTTP
        /// </summary>
        public static bool Template_DeviceAccessHTTPS_LFP(EwsAdapter adapter, EwsActivityData _activityData)
        {
            string[] tabs = new string[] { "Network_Settings", "Misc_Settings",
                                           "Admin_Account", "802.1x_Authentication", "IPsec_Firewall", "Network_Statistics",
                                           "ProtocolInfo_TCPIP", "Configuration_Page", "MngmtProtocol_WebMngmt", "Summary"
                                         };

            string[] tabTitles = new string[] {"Network Settings", "Other Settings",
                                           "Authorization", "802.1X Authentication", "IPsec/Firewall Policy", "Network Statistics",
                                           "Protocol Info", "Jetdirect Configuration Page", "Mgmt. Protocols", "TCP/IP Settings"
                                         };
            bool result = true; adapter.Start(); adapter.Wait(TimeSpan.FromSeconds(5));

            // Enable Encrypt All Web Communication
            TraceFactory.Logger.Info("Navigating to Management protocol");
            adapter.Navigate("MngmtProtocol_WebMngmt", "https");

            TraceFactory.Logger.Info("Enabling https");
            adapter.Check("Encrypt_All");

            adapter.Click("Apply");
            Thread.Sleep(3000);
            int i = 0;

            // Navigate to each tab on the networking page
            foreach (string tab in tabs)
            {
                adapter.Navigate(tab, "https");
                result = result && adapter.SearchText(tabTitles[i]);
                TraceFactory.Logger.Info("Navigating to " + tab + " protocol:" + result);
                i++;
            }

            // Disable Encrypt All Web Communication
            TraceFactory.Logger.Info("Navigating to Management protocol");
            adapter.Navigate("MngmtProtocol_WebMngmt");

            TraceFactory.Logger.Info("Disabling https");
            adapter.Uncheck("Encrypt_All");
            adapter.Click("Apply");
            Thread.Sleep(3000);

            // Navigate to each tab on the networking page
            i = 0;
            foreach (string tab in tabs)
            {
                adapter.Navigate(tab);
                result = result && adapter.SearchText(tabTitles[i]);
                TraceFactory.Logger.Info("Navigating to " + tab + " protocol:" + result);
                i++;
            }

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;
        }


        /// <summary>
        /// ID: 401872 (VEP)
        /// Note: 'Home' and 'Shop for Supplies' pages not available for VEP
        /// Check and Navigate through 'Support', 'Home' and 'Shop for Supplies' pages
        /// 1. Open printer WebUI, Navigate to Networking
        /// 2. Click on 'Support' button on all the vertical menu pages under Networking.
        /// 3. Click on 'Home' button on all the vertical menu pages under Networking.
        /// 4. Click on 'Shop for Supplies' on all the vertical menu pages under Networking.
        /// </summary>
        public static bool TemplateNavigateSupportHomeShopVEP(EwsAdapter adapter)
        {

            string[] tabs = new string[] { "Summary", "Network_Settings", "Misc_Settings", "Language", "Security_Status",
                                           "Admin_Account", "MngmtProtocol_WebMngmt", "802.1x_Authentication", "IPsec_Firewall",
                                           "Network_Statistics", "Announcement_Agent", "ProtocolInfo_TCPIP", "Configuration_Page"
                                         };

            bool result = true;

            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));

            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            adapter.Navigate("Support", "https");
            foreach (string tab in tabs)
            {
                TraceFactory.Logger.Info("Navigating to " + tab + " Tab");
                adapter.Navigate(tab);
                Thread.Sleep(3000);
                TraceFactory.Logger.Info("Navigating to Support Page");
                adapter.Navigate("Support");
                Thread.Sleep(3000);
                result = result && (adapter.SearchText("Support"));
                TraceFactory.Logger.Info("Result after searching 'Support' text is " + result);
            }

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;

        }

        /// <summary>
        /// ID: 401872 (TPS)
        /// Check and Navigate through 'Support', 'Home' and 'Shop for Supplies' pages
        /// 1. Open printer WebUI, Navigate to Networking
        /// 2. Click on 'Support' button on all the vertical menu pages under Networking.
        /// 3. Click on 'Home' button on all the vertical menu pages under Networking.
        /// 4. Click on 'Shop for Supplies' on all the vertical menu pages under Networking.
        /// </summary>
        public static bool TemplateNavigateSupportHomeShopTPS(EwsAdapter adapter)
        {

            string[] tabs = new string[] { "Summary", "IPv4_Config", "IPv6_Config", "Wireless_Config",
                                           "Wireless_Direct_Setup", "Network_Identification",
                                           "Advanced", "Certificates"
                                         };

            bool result = true;
            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));
            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            adapter.Navigate("Support", "https");
            // Navigate to 'Support' Tab and verify
            foreach (string tab in tabs)
            {
                TraceFactory.Logger.Info("Navigating to " + tab + " Tab");
                adapter.Navigate(tab);
                TraceFactory.Logger.Info("Navigating to Support Page");
                adapter.Navigate("Support");
                result = result && (adapter.SearchText("Support"));
                TraceFactory.Logger.Info("Result after searching 'Support' text is " + result);
            }

            // Navigate to 'Home' Tab and verify
            foreach (string tab in tabs)
            {
                TraceFactory.Logger.Info("Navigating to " + tab + " Tab");
                adapter.Navigate(tab);
                TraceFactory.Logger.Info("Navigating to Home Page");
                adapter.Navigate("Home");
                result = result && (adapter.SearchText("Device Status"));
                TraceFactory.Logger.Info("Result after searching 'Device Status' text is " + result);
            }

            // Navigate to "Shop for supplies" and verify
            foreach (string tab in tabs)
            {
                TraceFactory.Logger.Info("Navigating to " + tab + " Tab");
                adapter.Navigate(tab);
                TraceFactory.Logger.Info("Navigating to Order Supplies Page");
                adapter.Navigate("Order_Supplies");
                result = result && (adapter.SearchText("Privacy Statement"));
                TraceFactory.Logger.Info("Result after searching 'Privacy Statement' text is " + result);
            }

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;


        }

        /// <summary>
        /// ID: 401872 (LFP)
        /// Note: 'Home' and 'Shop for Supplies' pages not available for LFP
        /// Check and Navigate through 'Support', 'Home' and 'Shop for Supplies' pages
        /// 1. Open printer WebUI, Navigate to Networking
        /// 2. Click on 'Support' button on all the vertical menu pages under Networking.
        /// 3. Click on 'Home' button on all the vertical menu pages under Networking.
        /// 4. Click on 'Shop for Supplies' on all the vertical menu pages under Networking.
        /// </summary>
        public static bool TemplateNavigateSupportHomeShopLFP(EwsAdapter adapter)
        {

            string[] tabs = new string[] { "Summary", "Network_Settings", "Misc_Settings", "Security_Status",
                                           "Admin_Account", "MngmtProtocol_WebMngmt", "IPsec_Firewall",
                                           "Network_Statistics", "ProtocolInfo_TCPIP", "Configuration_Page"
                                         };

            bool result = true;
            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));
            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            adapter.Navigate("Support", "https");

            foreach (string tab in tabs)
            {
                TraceFactory.Logger.Info("Navigating to " + tab + " Tab");
                adapter.Navigate(tab);
                TraceFactory.Logger.Info("Navigating to Support Page");
                adapter.Navigate("Support");
                result = result && (adapter.SearchText("Product Support"));
                TraceFactory.Logger.Info("Result after searching 'Product Support' text is " + result);
            }

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            return result;
        }


        /// <summary>
        /// ID: 406719
        /// Go to TCPIP settings under networking tab - > Enable/Disable IPv6
        /// Navigate through IPV6 whether it can navigate through IPv6 or not
        /// </summary>
        public static bool TemplateEnableDisableIpv6(EwsAdapter adapter, EwsActivityData _activityData)
        {
            bool result = false;

            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), _activityData.ProductCategory);
            Printer.Printer printer = PrinterFactory.Create(family, System.Net.IPAddress.Parse(_activityData.PrinterIP));

            IPAddress ipAddress = IPAddress.Parse(_activityData.PrinterIP);
            IPAddress wiredIPv6StatelessAddr = printer.IPv6StatelessAddresses[0];

            TraceFactory.Logger.Info("Fetched IPv6 address is: " + wiredIPv6StatelessAddr);

            adapter.Start();

            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to TCP/IP settings");
            adapter.Navigate("TCPIPv6");

            TraceFactory.Logger.Info("Disable IPv6");
            adapter.Uncheck("IPv6_Enable");


            adapter.Click("Apply");

            TraceFactory.Logger.Info("Navigating through IPv6 address");
            adapter.NavigateIPv6(wiredIPv6StatelessAddr.ToString());
            Thread.Sleep(TimeSpan.FromMinutes(1));

            if ((!adapter.SearchText("Continue to this website (not recommended)")) || (!adapter.SearchText("This Connection is Untrusted")))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            TraceFactory.Logger.Info("Result after verification: " + result);

            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to TCP/IP settings");
            adapter.Navigate("TCPIPv6");

            TraceFactory.Logger.Info("Enable IPv6");
            adapter.Check("IPv6_Enable");

            adapter.Click("Apply");

            TraceFactory.Logger.Info("Navigating through IPv6 address");
            adapter.NavigateIPv6(wiredIPv6StatelessAddr.ToString());
            Thread.Sleep(TimeSpan.FromMinutes(2));

            //"Device Status" checking for VEP
            //"Status" checking for LFP
            if (adapter.SearchText("Continue to this website (not recommended)") || adapter.SearchText("This Connection is Untrusted") || adapter.SearchText("Device Status") || adapter.SearchText("Status"))
            {
                result = true;
            }
            else
            {
                result = false;
            }

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();

            TraceFactory.Logger.Info("Result after verification: " + result);
            return result;

        }


        /// <summary>
        /// ID: 406719
        /// Go to TCPIP settings under networking tab - > Enable/Disable IPv6
        /// Navigate through IPV6 whether it can navigate through IPv6 or not
        /// </summary>
        public static bool TemplateVerifyNWConfigurationPageVEP(EwsAdapter adapter, EwsActivityData _activityData)
        {
            bool result = false;
            string hostName = string.Empty;
            string domainNameIPv4 = string.Empty;
            string domainNameIPv6 = string.Empty;
            string primaryDNSServer = string.Empty;
            string secondaryDNSServer = string.Empty;

            string newHostName = "test_ctc";
            string newDomainNameIPv4 = "testDomainNamev4";
            string newDomainNameIPv6 = "testDomainNamev6";
            string newPrimaryDNSServer = "192.168.80.6";
            string newSecondaryDNSServer = "192.168.80.4";

            adapter.Start();

            //Verification of configured values in configuration pages
            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to TCP/IP settings");
            adapter.Navigate("Network_Identification", "https");

            hostName = adapter.GetValue("HostName");
            domainNameIPv4 = adapter.GetValue("DomainName_IPv4");
            domainNameIPv6 = adapter.GetValue("DomainName_IPv6");
            primaryDNSServer = adapter.GetValue("DNS_IPv4Primary");
            secondaryDNSServer = adapter.GetValue("DNS_IPv4_Secondary");

            adapter.SetText("HostName", newHostName);
            adapter.SetText("DomainName_IPv4", newDomainNameIPv4);
            adapter.SetText("DomainName_IPv6", newDomainNameIPv6);
            adapter.SetText("DNS_IPv4Primary", newPrimaryDNSServer);
            adapter.SetText("DNS_IPv4_Secondary", newSecondaryDNSServer);

            adapter.Click("Apply");

            adapter.Navigate("Configuration_Page");
            if (adapter.SearchText(newHostName) && adapter.SearchText(newDomainNameIPv4) && adapter.SearchText(newDomainNameIPv6) && adapter.SearchText(newPrimaryDNSServer) && adapter.SearchText(newSecondaryDNSServer))
            {
                result = true;
            }

            TraceFactory.Logger.Info("Result after verification: " + result);

            adapter.Navigate("Network_Identification");

            adapter.SetText("HostName", hostName);
            adapter.SetText("DomainName_IPv4", domainNameIPv4);
            adapter.SetText("DomainName_IPv6", domainNameIPv6);
            adapter.SetText("DNS_IPv4Primary", primaryDNSServer);
            adapter.SetText("DNS_IPv4_Secondary", secondaryDNSServer);

            adapter.Click("Apply");


            //Verification of protocol values in protocol info page
            adapter.Navigate("MngmtProtocol_SNMP");

            adapter.Click("Enable_SNMPv1_v2_ReadWrite");
            adapter.SetText("Set_Community_Name", "Set");
            adapter.SetText("Confirm_Set_Community_Name", "Set");
            adapter.SetText("Get_Community_Name", "get");
            adapter.SetText("Confirm_Get_Community_Name", "get");

            adapter.Click("Enable_SNMPv3");
            adapter.SetText("User_Name", "admin");
            adapter.SetText("AuthenticationProtocolPassphrase", "12345678");
            adapter.SetText("PrivacyProtocolPassphrase", "12345678");
            adapter.Click("Apply");

            adapter.Navigate("ProtocolInfo_SNMP");

            if (adapter.SearchText("Set") && adapter.SearchText("admin"))
            {
                result = true;
            }
            TraceFactory.Logger.Info("Result after verification:" + result);

            //Verification of network statistics values in network statistics page

            //adapter.Navigate("Network_Statistics");
            //adapter.GetValue();

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();

            return result;
        }


        /// <summary>
        /// ID: 406719
        /// Go to TCPIP settings under networking tab - > Enable/Disable IPv6
        /// Navigate through IPV6 whether it can navigate through IPv6 or not
        /// </summary>
        public static bool TemplateVerifyNWConfigurationPageLFP(EwsAdapter adapter, EwsActivityData _activityData)
        {
            bool result = false;
            string hostName = string.Empty;
            string domainNameIPv4 = string.Empty;
            string domainNameIPv6 = string.Empty;
            string primaryDNSServer = string.Empty;
            string secondaryDNSServer = string.Empty;

            string newHostName = "test_ctc";
            string newDomainNameIPv4 = "testDomainNamev4";
            string newDomainNameIPv6 = "testDomainNamev6";
            string newPrimaryDNSServer = "192.168.80.6";
            string newSecondaryDNSServer = "192.168.80.4";

            adapter.Start();

            //Verification of configured values in configuration pages
            TraceFactory.Logger.Info("Opening the URL https:\\" + adapter.Settings.DeviceAddress);
            TraceFactory.Logger.Info("Navigating to TCP/IP settings");
            adapter.Navigate("Network_Identification", "https");

            hostName = adapter.GetValue("HostName");
            domainNameIPv4 = adapter.GetValue("DomainName_IPv4");
            domainNameIPv6 = adapter.GetValue("DomainName_IPv6");
            primaryDNSServer = adapter.GetValue("DNS_IPv4Primary");
            secondaryDNSServer = adapter.GetValue("DNS_IPv4_Secondary");

            adapter.SetText("HostName", newHostName);
            adapter.SetText("DomainName_IPv4", newDomainNameIPv4);
            adapter.SetText("DomainName_IPv6", newDomainNameIPv6);
            adapter.SetText("DNS_IPv4Primary", newPrimaryDNSServer);
            adapter.SetText("DNS_IPv4_Secondary", newSecondaryDNSServer);

            adapter.Click("Apply");

            adapter.Navigate("Configuration_Page");
            if (adapter.SearchText(newHostName) && adapter.SearchText(newDomainNameIPv4) && adapter.SearchText(newDomainNameIPv6) && adapter.SearchText(newPrimaryDNSServer) && adapter.SearchText(newSecondaryDNSServer))
            {
                result = true;
            }

            TraceFactory.Logger.Info("Result after verification: " + result);

            adapter.Navigate("Network_Identification");

            adapter.SetText("HostName", hostName);
            adapter.SetText("DomainName_IPv4", domainNameIPv4);
            adapter.SetText("DomainName_IPv6", domainNameIPv6);
            adapter.SetText("DNS_IPv4Primary", primaryDNSServer);
            adapter.SetText("DNS_IPv4_Secondary", secondaryDNSServer);

            adapter.Click("Apply");


            //Verification of protocol values in protocol info page
            adapter.Navigate("MngmtProtocol_SNMP");

            adapter.Click("Enable_SNMPv1_v2_ReadWrite");
            adapter.SetText("Set_Community_Name", "Set");
            adapter.SetText("Confirm_Set_Community_Name", "Set");
            adapter.SetText("Get_Community_Name", "get");
            adapter.SetText("Confirm_Get_Community_Name", "get");

            adapter.Click("Enable_SNMPv3");
            adapter.SetText("User_Name", "admin");
            adapter.SetText("AuthenticationProtocolPassphrase", "fffffffffffffff");
            adapter.SetText("PrivacyProtocolPassphrase", "fffffffffffffff");
            adapter.Click("Apply");

            adapter.Navigate("ProtocolInfo_SNMP");

            if (adapter.SearchText("Set") && adapter.SearchText("admin"))
            {
                result = true;
            }
            TraceFactory.Logger.Info("Result after verification:" + result);

            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();

            return result;
        }

    }
}
