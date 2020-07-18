using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;

namespace HP.ScalableTest.Plugin.TelnetSnmp
{
    class TelnetTemplates
    {
        public string _printerIP = string.Empty;

        #region Constructors
        public TelnetTemplates()
        {

        }

        public TelnetTemplates(string printerIP)
        {
            _printerIP = printerIP;
        }
        #endregion

        #region Implementation of the templates for VEP
        /// TEMPLATE Verify Telnet connection with without password
        /// <summary>
        /// ID: 403320
        /// Open Telnet session using <telnet IP_address> command
        /// Expected :
        /// Telnet session should open with following message
        /// HP JetDirect
        /// Password is not set
        /// Please type "menu" for MENU syatem, or "?" for help, or "/" for current settings
        /// </summary>
        public bool Template_403320a_VEP()
        {
            bool result = false;
            TelnetLib TelnetTest = new TelnetLib(_printerIP);
            TraceFactory.Logger.Info("Connecting through telnet");
            TelnetTest.Connect();
            string output = TelnetTest.ReceiveFile("Press RETURN to continue:");
            TraceFactory.Logger.Info("Checking Telnet connection without password");
            result = (output.Contains("HP JetDirect")) &&
                     (output.Contains("Password is not set")) &&
                     (output.Contains("Please type \"menu\" for the MENU system")) &&
                     (output.Contains("or \"?\" for help, or \"/\" for current settings."));
            TelnetTest.SendLine("exit");
            TraceFactory.Logger.Info("Exiting Telnet");
            Thread.Sleep(5000);
            return result;
        }

        /// TEMPLATE Verify Telnet connection with without password
        /// <summary>
        /// ID: 403320
        /// Open Telnet session using <telnet IP_address> command
        /// Set a password, save it and exit telnet.
        /// Open telnet session again and enter the correct password when prompted
        /// Expected :
        /// Telnet session should open with following message
        /// HP JetDirect
        /// Please type "menu" for MENU syatem, or "?" for help, or "/" for current settings
        /// </summary>
        public bool Template_403320c_VEP()
        {
            bool result = false;
            TelnetLib TelnetTest = new TelnetLib(_printerIP);
            TraceFactory.Logger.Info("Connecting through telnet");
            TelnetTest.Connect();
            string output = TelnetTest.ReceiveFile("Press RETURN to continue:");
            result = (output.Contains("HP JetDirect")) &&
                     (output.Contains("Password is not set")) &&
                     (output.Contains("Please type \"menu\" for the MENU system")) &&
                     (output.Contains("or \"?\" for help, or \"/\" for current settings."));

            // Set the password and save it
            TraceFactory.Logger.Info("Setting username/password = admin/admin");
            TelnetTest.SendLine("passwd admin admin");
            TelnetTest.SendLine("save");
            Thread.Sleep(20000);
            TelnetTest.SendLine("exit");
            Thread.Sleep(8000);


            //Connect again with password
            TelnetTest.Connect();
            TraceFactory.Logger.Info("Connecting through telnet with password");
            string resultAfterPassword = TelnetTest.ReceiveFile("Press RETURN to continue:");

            result = result && (!resultAfterPassword.Contains("Password is not set"));
            TelnetTest.SendLine("admin");
            TelnetTest.SendLine("admin");

            resultAfterPassword = TelnetTest.ReceiveFile("Press RETURN to continue:");
            TraceFactory.Logger.Info("Verifying telnet connection with password");
            result = result && (resultAfterPassword.Contains("Please type \"menu\" for the MENU system")) &&
                     (resultAfterPassword.Contains("or \"?\" for help, or \"/\" for current settings."));
            TraceFactory.Logger.Info("Removing password");
            TelnetTest.SendLine("passwd");
            TelnetTest.SendLine("save");
            Thread.Sleep(20000);
            TelnetTest.SendLine("exit");
            TraceFactory.Logger.Info("Exiting telnet connection");
            Thread.Sleep(20000);
            return result;

        }

        /// TEMPLATE Verify device parameter configuration using telnet command line interface
        /// <summary>
        ///  Open telnet connection.
        ///  Configure following parameters using the telnet commands:
        ///  sys-location, sys-contact, host-name, domain-name, add, delete, default, add string, delete string,
        ///  bonjour-svc-name, get-cmnty-name, set-cmnty-name, support-contact, support-number, support-url,
        ///  tech-support-url, ipp/ipps-printing, raw-port, syslog-max, syslog-priority, syslog-facility, hoplimit-wsd,
        ///  tcp-mss, tcp-msl, default-ip, default-ip-dhcp, duid, dns-cache-ttl, dhcp-arbitration, panic-behavior,
        ///  link-type, laa, 1000t-ms-conf, 1000t-pause-conf, xml-services-conf, ws-discovery-conf, rtc-enforce,
        ///  printer-dns-svr, sec-dns-svr, printer-wins-svr, sec-wins-svr, allow, syslog-svr, trap-dest
        ///  Save the changes and exit telnet.
        ///  Telnet to the device again and check the configured parameters using / command.
        ///  Expected :
        ///  Configured parameters should get saved and changes should be reflected.
        /// </summary>
        /// 
        public bool Template_403343_VEP()
        {
            bool result = true;
            string[,] TelnetCommands = new string[,] {  {"raw-port", "0"},{"allow","0.0.0.0"},
                {"sys-location", "sysloc"}, {"sys-contact", "syscontact"},
                {"host-name", "hostname"},
                {"defaultq", "RAW"}, {"bonjour-svc-name", "bonjourservice"},
                {"get-cmnty-name", "cmntynameget"}, {"set-cmnty-name", "cmntynameset"},
                {"support-contact", "supportcontact"}, {"support-number", "supportnumber"},
                {"ipp/ipps-printing", "0"},  {"syslog-max", "1"},{"duid", "dd"},
                {"syslog-priority", "5"},
                {"tcp-mss", "1"}, {"tcp-msl", "20"}, {"default-ip", "DEFAULT_IP"},
                {"default-ip-dhcp", "0"},  {"dns-cache-ttl", "50"},
                {"dhcp-arbitration", "6"}, {"panic-behavior", "JUST_HALT"},
                {"link-type", "100AUTO"}, {"1000t-ms-conf", "0"}, {"1000t-pause-conf", "1"},
                {"xml-services-conf", "0"}, {"ws-discovery-conf", "0"}, {"rtc-enforce", "2"},
                {"pri-dns-svr", "16.110.135.52"},{"sec-dns-svr", "16.110.135.51"}, {"pri-wins-svr", "16.238.57.248"},
                {"sec-wins-svr", "16.230.57.248"}, {"allow", IPaddressFetch() }, {"syslog-svr", "2.2.2.2"},
                {"raw-port", "8080"},{"trap-dest", "3.3.3.3"}
                //{"domain-name", "domainname"}, {"syslog-facility", "LPR"}, {"hoplimit-wsd", "200"},
            };


            string[,] validation = new string[,] {   {"System Location", "sysloc"}, {"System Contact", "syscontact"},
                {"Host Name", "hostname"},
                {"LPD Default Queue", "RAW"}, {"Bonjour Svc Name", "bonjourservice"},
                {"Get Cmnty Name", "Specified"}, {"Set Cmnty Name", "Specified"},
                {"Support Contact", "supportcontact"}, {"Phone Number", "supportnumber"},
                {"IPP/IPPS Printing", "0"}, {"Syslog MaxMsg/Min", "1"},
                {"Syslog Priority", "5"},
                {"TCP MSS", "1"}, {"TCP MSL", "20 Seconds"}, {"Default IP", "Default IP"},
                {"Default IP DHCP", "Disabled"},  {"DNS Cache TTL", "50"},
                {"DHCP arbitration", "6"}, {"Panic Behavior", "JUST_HALT"}, {"Link Type", "AUTO"},
                {"1000T Master/Slave", "AUTO"}, {"1000T Pause Frame", "AUTO"},
                {"HP XML Services", "Disabled"}, {"WS Discovery", "Disabled"}, {"RTC Enforce", "2"},
                {"Pri DNS Server", "16.110.135.52"}, {"Sec DNS Serve", "16.110.135.51"},
                {"Pri WINS Server", "16.238.57.248"}, {"Sec WINS Server", "16.230.57.248"},{"DHCP unique ID", "dd"},
                {"Allow", IPaddressFetch()}, {"Syslog Server", "2.2.2.2"}, {"Raw print port", "8080"},
                {"trap-dest", "3.3.3.3"}
                // {"Domain Name", "domainname"},{"Syslog Facility", "LPR"}, {"HopLimit/WSD", "200"},
            };

            TelnetLib TelnetTest = new TelnetLib(_printerIP);



            for (int i = 0; i < (TelnetCommands.Length / 3); i++)
            {
                if (TelnetCommands[i, 0] == "bonjour-svc-name")
                {
                    TelnetTest.Connect();
                    TraceFactory.Logger.Info("Connecting through telnet12000");
                    TelnetTest.SendLine("advanced");

                    TraceFactory.Logger.Info("Runing Command : " + TelnetCommands[i, 0] + " " + TelnetCommands[i, 1]);
                    TelnetTest.SendLine(TelnetCommands[i, 0] + " " + TelnetCommands[i, 1]);
                    TelnetTest.SendLine("save");

                    string status1 = TelnetTest.ReceiveFile("Press RETURN to continue:");
                    TelnetTest.SendLine("Exit");
                    Thread.Sleep(12000);
                    break;
                }
            }



            TelnetTest.Connect();

            TelnetTest.SendLine("/");
            Thread.Sleep(5000);
            string status = TelnetTest.ReceiveFile("Press RETURN to continue:");
            for (int i = 0; i < (validation.Length) / 2; i++)
            {

                result = result && (Regex.IsMatch(TelnetTest.getValue(validation[i, 0]), validation[i, 1]));
                TraceFactory.Logger.Info("Verifying the value for: " + validation[i, 0] + " " + validation[i, 1] + ":" + result);
            }

            // Executing addq, deleteq, addstring, deletestring commands
            TelnetTest.SendLine("addq add");
            TelnetTest.SendLine("addstring stringadded string");
            TelnetTest.SendLine("advanced");
            TelnetTest.SendLine("/");
            Thread.Sleep(5000);
            status = TelnetTest.ReceiveFile("Press RETURN to continue:");
            result = result && (Regex.IsMatch(TelnetTest.getValue("Queue Name"), "add")) &&
                     (Regex.IsMatch(TelnetTest.getValue("String Name"), "stringadded"));
            TelnetTest.SendLine("deleteq add");
            TelnetTest.SendLine("deletestring stringadded");
            TelnetTest.SendLine("advanced");
            TelnetTest.SendLine("/");
            TelnetTest.ReceiveFile("Press RETURN to continue:");

            //Check for the deleted que and deleted string values
            result = result && !(Regex.IsMatch(TelnetTest.getValue("Queue Name"), "add")) &&
                     !(Regex.IsMatch(TelnetTest.getValue("String Name"), "addstring"));
            TelnetTest.SendLine("Exit");
            TelnetTest.SendLine("N");
            Thread.Sleep(5000);
            return result;
        }



        /// <summary>
        /// Fetches the IPV4 address of the client machine which starts with ip 192 series
        /// </summary>
        /// <returns></returns>
        private string IPaddressFetch()
        {
            var hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = (from ip in hostEntry.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
            string IP = "0.0.0.0";
            foreach (var ipAddress1 in ipAddress)
            {
                if (ipAddress1.Contains("192."))
                {
                    IP = ipAddress1;
                }
            }
            return IP.ToString();
        }




        /// TEMPLATE Verity Enable Disable of device parameter using Telnet
        /// <summary>
        /// ID: 403369
        /// Open Telnet connection
        /// Enable/Disable config parameters
        /// Expected :
        /// Configured parameters should get saved and changes should be reflected
        /// </summary>
        public bool Template_403369_VEP()
        {
            bool result = false;

            // Commands to be sent to printer {<command>,<disabled value>,<enabled value>}
            string[,] telnetCommands = new string[,] {{"9100-printing", "0", "1"}, {"ftp-printing", "0", "1"},
                {"ws-printing", "0", "1"}, {"lpd-printing", "0", "1"},
                {"interlock", "0", "1"}, {"mult-tcp-conn", "1", "0"},
                {"buffer-packing", "1", "0"}, {"syslog-config", "0", "1"},
                {"slp-config", "0", "1"}, {"slp-keep-alive", "0", "1"},
                {"slp-client-mode", "0", "1"}, {"ttl-slp", "-1", "4"},
                {"bonjour-services", "2", "6"},{"bonjour-config", "0", "1"},
                {"llmnr", "0", "1"}, {"ipv4-multicast", "0", "1"},
                {"idle-timeout", "0", "270"}, {"user-timeout", "0", "900"},
                //{"icmp-ts-config", "0", "1"}
            };

            // Status displayed after the / command. {<Parameter name>,<Disabled status>, <Enabled Status>}
            string[,] telnetStatus = new string[,] {{"9100 Printing", "Disabled", "Enabled"}, {"FTP Printing", "Disabled", "Enabled"},
                {"WS Printing", "Disabled", "Enabled"}, {"LPD Printing", "Disabled", "Enabled"},
                {"Interlock Mode", "Disabled", "Enabled"}, {"Mult-tcp-conn", "Disabled", "Enabled"},
                {"Buffer Packing", "Disabled", "Enabled"}, {"Syslog Config", "Disabled", "Enabled"},
                {"SLP Config", "Disabled", "Enabled"}, {"SLP Keep Alive", "0", "1"},
                {"SLP Client-Mode", "Disabled", "Enabled"}, {"TTL/SLP", "Disabled", "4 Hops"},
                {"Bonjour Config", "Disabled", "Enabled"},
                {"LLMNR", "Disabled", "Enabled"}, {"IPv4 Multicast", "Disabled", "Enabled"},
                {"Idle Timeout", "0 Seconds", "270 Seconds"}, {"User Timeout", "0 Seconds", "900 Seconds"},
                //{"ICMPv4 TS requests", "Disabled", "Enabled"}
            };

            TelnetLib TelnetTest = new TelnetLib(_printerIP);
            TraceFactory.Logger.Info("Connecting through telnet");
            TelnetTest.Connect();
            string retreivedResult = TelnetTest.ReceiveFile("Press RETURN to continue:");
            result = (retreivedResult.Contains("HP JetDirect"));

            TelnetTest.SendLine("advanced");

            // Disable the config parameters
            for (int i = 0; i < (telnetCommands.Length / 3); i++)
            {
                TraceFactory.Logger.Info("Running command : " + telnetCommands[i, 0] + " " + telnetCommands[i, 1]);
                TelnetTest.SendLine(telnetCommands[i, 0] + " " + telnetCommands[i, 1]);
                Thread.Sleep(2000);
            }

            TelnetTest.SendLine("save");
            Thread.Sleep(5000);

            TelnetTest.Connect();
            retreivedResult = TelnetTest.ReceiveFile("Press RETURN to continue:");
            result = result && (retreivedResult.Contains("HP JetDirect"));

            // Retrieve the values and check if changed have taken effect
            TelnetTest.SendLine("advanced");
            TelnetTest.SendLine("/");

            retreivedResult = TelnetTest.ReceiveFile("Press RETURN to continue:");

            for (int i = 0; i < (telnetStatus.Length / 3); i++)
            {
                TraceFactory.Logger.Info("Verifying the value for : " + telnetStatus[i, 0]);
                result = result && (Regex.IsMatch(TelnetTest.getValue(telnetStatus[i, 0]), telnetStatus[i, 1]));
            }


            // Enable the config parameters
            TelnetTest.SendLine("advanced");
            for (int j = 0; j < (telnetCommands.Length / 3); j++)
            {
                TraceFactory.Logger.Info("Running command : " + telnetCommands[j, 0] + " " + telnetCommands[j, 2]);
                TelnetTest.SendLine(telnetCommands[j, 0] + " " + telnetCommands[j, 2]);
                Thread.Sleep(2000);
            }

            TelnetTest.SendLine("save");
            Thread.Sleep(5000);

            // Retrieve the values and check if changed have taken effect

            TelnetTest.Connect();
            retreivedResult = TelnetTest.ReceiveFile("Press RETURN to continue:");
            result = result && (retreivedResult.Contains("HP JetDirect"));

            TelnetTest.SendLine("advanced");
            TelnetTest.SendLine("/");

            retreivedResult = TelnetTest.ReceiveFile("Press RETURN to continue:");
            TelnetTest.SendLine("exit");
            Thread.Sleep(3000);

            for (int k = 0; k < (telnetStatus.Length / 3); k++)
            {
                TraceFactory.Logger.Info("Verifying the value for : " + telnetStatus[k, 0]);
                result = result && (Regex.IsMatch(TelnetTest.getValue(telnetStatus[k, 0]), telnetStatus[k, 2]));
            }

            return result;
        }

        /// TEMPLATE Verity telnet disable functionality
        /// <summary>
        /// Web into device go to networking->other settings and disable telnet.
        /// Open telnet command using telnet device ip address.
        /// Expected :
        /// Telnet connection should not get established.
        /// </summary>
        public bool Template_403381(EwsAdapter adapter)
        {
            bool result = false;
            adapter.Start();
            adapter.Wait(TimeSpan.FromSeconds(5));
            TraceFactory.Logger.Info("Disabling telnet through EWS");
            adapter.Navigate("Misc_Settings");
            adapter.Uncheck("Telnet_Config");
            adapter.Click("Apply_Misc");
            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Navigate("Misc_Settings");
            result = (!adapter.IsChecked("Telnet_Config"));
            TraceFactory.Logger.Info("Attempt to connect through telnet");
            TelnetLib TelnetTest = new TelnetLib(adapter.Settings.DeviceAddress);
            result = result && (!TelnetTest.Connect());
            TraceFactory.Logger.Info("Enable telnet through EWS");
            adapter.Navigate("Misc_Settings");
            adapter.Check("Telnet_Config");
            adapter.Click("Apply_Misc");
            adapter.Wait(TimeSpan.FromSeconds(5));
            adapter.Stop();
            TelnetTest.Connect();
            TelnetTest.SendLine("Exit");
            Thread.Sleep(5000);
            return result;
        }
        #endregion

        #region Implementation of the templates for LFP
        /// TEMPLATE Verify Telnet connection with without password
        /// <summary>
        /// ID: 403320
        /// Open Telnet session using <telnet IP_address> command
        /// Expected :
        /// Telnet session should open with following message
        /// HP JetDirect
        /// Password is not set
        /// Please type "menu" for MENU syatem, or "?" for help, or "/" for current settingsexit
        /// </summary>
        public bool
            Template_403320a_LFP()
        {
            bool result = false;
            TelnetLib TelnetTest = new TelnetLib(_printerIP);
            TraceFactory.Logger.Info("Connecting trhough telnet");
            TelnetTest.Connect();
            string output = TelnetTest.ReceiveFile("Press RETURN to continue:");
            TraceFactory.Logger.Info("Validating telnet connection without password");
            result = (output.Contains("HP")) &&
                                   (output.Contains("Password is not set")) &&
                                   (output.Contains("Please type \"menu\" for the MENU system")) &&
                                   (output.Contains("or \"?\" for help, or \"/\" for current settings."));
            TraceFactory.Logger.Info("Exiting telnet");
            TelnetTest.SendLine("exit");
            Thread.Sleep(5000);
            return result;
        }

        /// TEMPLATE Verify Telnet connection with without password
        /// <summary>
        /// ID: 403320
        /// Open Telnet session using <telnet IP_address> command
        /// Set a password, save it and exit telnet.
        /// Open telnet session again and enter the correct password when prompted
        /// Expected :
        /// Telnet session should open with following message
        /// HP JetDirect
        /// Please type "menu" for MENU syatem, or "?" for help, or "/" for current settings
        /// </summary>
        public bool Template_403320c_LFP()
        {
            bool result = false;
            TelnetLib TelnetTest = new TelnetLib(_printerIP);

            TraceFactory.Logger.Info("Connecting trhough telnet");
            TelnetTest.Connect();
            string output = TelnetTest.ReceiveFile("Press RETURN to continue:");
            result = (output.Contains("HP")) &&
                                   (output.Contains("Password is not set")) &&
                                   (output.Contains("Please type \"menu\" for the MENU system")) &&
                                   (output.Contains("or \"?\" for help, or \"/\" for current settings."));

            // Set the password and save it
            TraceFactory.Logger.Info("Setting username/password as admin/admin");
            TelnetTest.SendLine("passwd admin admin");
            TelnetTest.SendLine("save");
            Thread.Sleep(20000);


            //Connect again with password
            TraceFactory.Logger.Info("Connecting trhough telnet");
            TelnetTest.Connect();
            string resultAfterPassword = TelnetTest.ReceiveFile("Press RETURN to continue:");

            TraceFactory.Logger.Info("Validating telnet connection with password");
            result = result && (!resultAfterPassword.Contains("Password is not set"));
            TelnetTest.SendLine("admin");
            TelnetTest.SendLine("admin");

            resultAfterPassword = TelnetTest.ReceiveFile("Press RETURN to continue:");

            result = result && (resultAfterPassword.Contains("Please type \"menu\" for the MENU system")) &&
                     (resultAfterPassword.Contains("or \"?\" for help, or \"/\" for current settings."));

            TelnetTest.SendLine("passwd");
            TelnetTest.SendLine("save");
            TraceFactory.Logger.Info("Exiting telnet");
            Thread.Sleep(20000);
            return result;
        }

        /// TEMPLATE Verify device parameter configuration using telnet command line interface
        /// <summary>
        ///  Open telnet connection.
        ///  Configure following parameters using the telnet commands:
        ///  sys-location, sys-contact, host-name, domain-name, add, delete, default, add string, delete string,
        ///  bonjour-svc-name, get-cmnty-name, set-cmnty-name, support-contact, support-number, support-url,
        ///  tech-support-url, ipp/ipps-printing, raw-port, syslog-max, syslog-priority, syslog-facility, hoplimit-wsd,
        ///  tcp-mss, tcp-msl, default-ip, default-ip-dhcp, duid, dns-cache-ttl, dhcp-arbitration, panic-behavior,
        ///  link-type, laa, 1000t-ms-conf, 1000t-pause-conf, xml-services-conf, ws-discovery-conf, rtc-enforce,
        ///  printer-dns-svr, sec-dns-svr, printer-wins-svr, sec-wins-svr, allow, syslog-svr, trap-dest
        ///  Save the changes and exit telnet.
        ///  Telnet to the device again and check the configured parameters using / command.
        ///  Expected :
        ///  Configured parameters should get saved and changes should be reflected.
        /// </summary>
        public bool Template_403343_LFP()
        {
            bool result = true;
            string[,] TelnetCommands = new string[,] {   {"sys-location", "sysloc"}, {"sys-contact", "syscontact"},
                {"host-name", "hostname"}, {"domain-name", "domain"},
                {"bonjour-svc-name", "bonjourservice"}, {"get-cmnty-name", "cmntynameget"},
                {"set-cmnty-name", "cmntynameset"}, {"support-contact", "supportcontact"},
                {"support-number", "supportnumber"}, {"raw-port", "8080"},
                {"hoplimit-wsd", "200"}, {"default-ip", "DEFAULT_IP"}, {"default-ip-dhcp", "0"},
                {"duid", "dd"}, {"link-type", "100AUTO"},
                {"ws-discovery-conf", "0"}, {"pri-dns-svr", "16.110.135.52"},
                {"sec-dns-svr", "16.110.135.51"}, {"pri-wins-svr", "16.238.57.248"},
                {"sec-wins-svr", "16.230.57.248"}
            };


            string[,] validation = new string[,] {   {"System Location", "sysloc"}, {"System Contact", "syscontact"},
                {"Host Name", "hostname"}, {"Domain Name", "domain"},
                {"Bonjour Svc Name", "bonjourservice"}, {"Get Cmnty Name", "Specified"},
                {"Set Cmnty Name", "Specified"}, {"Support Contact", "supportcontact"},
                {"Phone Number", "supportnumber"}, {"Raw print port", "8080"},
                {"HopLimit/WSD", "200"}, {"Default IP", "Default IP"},
                {"Default IP DHCP", "Disabled"}, {"DHCP unique ID", "dd"},
                {"Link Type", "AUTO"},
                {"WS Discovery", "Disabled"}, {"Pri DNS Server", "16.110.135.52"},
                {"Sec DNS Serve", "16.110.135.51"}, {"Pri WINS Server", "16.238.57.248"},
                {"Sec WINS Server", "16.230.57.248"}
            };

            TelnetLib TelnetTest = new TelnetLib(_printerIP);
            TraceFactory.Logger.Info("Connecting trhough telnet");
            TelnetTest.Connect();
            TelnetTest.SendLine("advanced");
            for (int i = 0; i < (TelnetCommands.Length) / 2; i++)
            {
                TraceFactory.Logger.Info("Running Command : " + TelnetCommands[i, 0] + " " + TelnetCommands[i, 1]);
                TelnetTest.SendLine(TelnetCommands[i, 0] + " " + TelnetCommands[i, 1]);
            }
            string status1 = TelnetTest.ReceiveFile("Press RETURN to continue:");

            TelnetTest.SendLine("/");
            string status = TelnetTest.ReceiveFile("Press RETURN to continue:");
            for (int i = 0; i < (validation.Length) / 2; i++)
            {
                TraceFactory.Logger.Info("Validating : " + validation[i, 0]);
                result = result && (Regex.IsMatch(TelnetTest.getValue(validation[i, 0]), validation[i, 1]));
                Thread.Sleep(1000);
            }
            TelnetTest.SendLine("Exit");
            TelnetTest.SendLine("N");
            TraceFactory.Logger.Info("Exiting telnet");
            Thread.Sleep(5000);
            return result;
        }

        /// TEMPLATE Verity Enable Disable of device parameter using Telnet
        /// <summary>
        /// ID: 403369
        /// Open Telnet connection
        /// Enable/Disable config parameters
        /// Expected :
        /// Configured parameters should get saved and changes should be reflected
        /// </summary>
        public bool Template_403369_LFP()
        {
            bool result = false;

            // Commands to be sent to printer {<command>,<disabled value>,<enabled value>}
            string[,] telnetCommands = new string[,] {{"9100-printing", "0", "1"}, {"ftp-printing", "0", "1"},
                {"ws-printing", "0", "1"}, {"lpd-printing", "0", "1"},
                {"interlock", "0", "1"}, {"slp-config", "0", "1"},
                {"slp-keep-alive", "0", "1"}, {"slp-client-mode", "0", "1"},
                {"ttl-slp", "-1", "4"}, {"bonjour-config", "0", "1"},
                {"llmnr", "0", "1"}, {"ipv4-multicast", "0", "1"},
                {"idle-timeout", "0", "270"}, {"user-timeout", "0", "900"}
            };

            // Status displayed after the / command. {<Parameter name>,<Disabled status>, <Enabled Status>}
            string[,] telnetStatus = new string[,] {{"9100 Printing", "Disabled", "Enabled"}, {"FTP Printing", "Disabled", "Enabled"},
                {"WS Printing", "Disabled", "Enabled"}, {"LPD Printing", "Disabled", "Enabled"},
                {"Interlock Mode", "Disabled", "Enabled"}, {"SLP Config", "Disabled", "Enabled"},
                {"SLP Keep Alive", "0", "1"}, {"SLP Client-Mode", "Disabled", "Enabled"},
                {"TTL/SLP", "Disabled", "4 Hops"}, {"Bonjour Config", "Disabled", "Enabled"},
                {"LLMNR", "0", "1"}, {"IPv4 Multicast", "Disabled", "Enabled"},
                {"Idle Timeout", "0 Seconds", "270 Seconds"}, {"User Timeout", "0 Seconds", "900 Seconds"},
            };

            TelnetLib TelnetTest = new TelnetLib(_printerIP);
            TraceFactory.Logger.Info("Connecting through telnet");
            TelnetTest.Connect();
            string retreivedResult = TelnetTest.ReceiveFile("Press RETURN to continue:");
            result = (retreivedResult.Contains("HP"));

            TelnetTest.SendLine("advanced");

            // Disable the config parameters
            for (int i = 0; i < (telnetCommands.Length / 3); i++)
            {
                TraceFactory.Logger.Info("Running Command : " + telnetCommands[i, 0] + " " + telnetCommands[i, 1]);
                TelnetTest.SendLine(telnetCommands[i, 0] + " " + telnetCommands[i, 1]);
                Thread.Sleep(1000);
            }

            TelnetTest.SendLine("save");

            Thread.Sleep(20000);
            TelnetTest.Connect();
            retreivedResult = TelnetTest.ReceiveFile("Press RETURN to continue:");
            result = result && (retreivedResult.Contains("HP"));

            // Retrieve the values and check if changed have taken effect
            TelnetTest.SendLine("advanced");
            TelnetTest.SendLine("/");
            retreivedResult = TelnetTest.ReceiveFile("Press RETURN to continue:");

            for (int i = 0; i < (telnetStatus.Length / 3); i++)
            {
                TraceFactory.Logger.Info("Validating :" + telnetStatus[i, 0]);
                result = result && (Regex.IsMatch(TelnetTest.getValue(telnetStatus[i, 0]), telnetStatus[i, 1]));
            }

            // Enable the config parameters
            TelnetTest.SendLine("advanced");
            for (int j = 0; j < (telnetCommands.Length / 3); j++)
            {
                TraceFactory.Logger.Info("Running command :" + telnetCommands[j, 0] + " " + telnetCommands[j, 2]);
                TelnetTest.SendLine(telnetCommands[j, 0] + " " + telnetCommands[j, 2]);
                Thread.Sleep(1000);
            }

            TelnetTest.SendLine("save");
            Thread.Sleep(20000);

            // Retrieve the values and check if changed have taken effect

            TelnetTest.Connect();
            retreivedResult = TelnetTest.ReceiveFile("Press RETURN to continue:");
            result = result && (retreivedResult.Contains("HP"));
            TelnetTest.SendLine("advanced");
            TelnetTest.SendLine("/");

            retreivedResult = TelnetTest.ReceiveFile("Press RETURN to continue:");
            TelnetTest.SendLine("exit");
            Thread.Sleep(3000);

            for (int k = 0; k < (telnetStatus.Length / 3); k++)
            {
                TraceFactory.Logger.Info("Validating :" + telnetStatus[k, 0]);
                result = result && (Regex.IsMatch(TelnetTest.getValue(telnetStatus[k, 0]), telnetStatus[k, 2]));
            }

            return result;
        }
        #endregion

        #region Implementation of the templates for TPS
        /// TEMPLATE Verify Telnet connection with without password
        /// <summary>
        /// ID: 403320
        /// Open Telnet session using <telnet IP_address> command
        /// Expected :
        /// Telnet session should open with following message
        /// HP JetDirect
        /// Password is not set
        /// Please type "menu" for MENU syatem, or "?" for help, or "/" for current settings
        /// </summary>
        public bool Template_403320a_TPS()
        {
            bool result = false;
            TelnetLib telnetObj = new TelnetLib(_printerIP);
            TraceFactory.Logger.Info("Connecting through telnet");

            if (telnetObj.Connect())
            {
                Thread.Sleep(5000);
                telnetObj.SendLine("/");
                Thread.Sleep(5000);

                telnetObj.ReceiveFile("Press RETURN to continue:");
                TraceFactory.Logger.Info("Checking Telnet connection without password");

                result = (Regex.IsMatch(telnetObj.getValue("passwd"), "Not Specified"));
                TraceFactory.Logger.Info("Telnet connection without password state is:" + result);

                telnetObj.SendLine("exit");
                TraceFactory.Logger.Info("Exiting Telnet");
                Thread.Sleep(5000);
            }
            return result;
        }

        /// TEMPLATE Verify Telnet connection with without password
        /// <summary>
        /// ID: 403320
        /// Open Telnet session using <telnet IP_address> command
        /// Set a password, save it and exit telnet.
        /// Open telnet session again and enter the correct password when prompted
        /// Expected :
        /// Telnet session should open with following message
        /// HP JetDirect
        /// Please type "menu" for MENU syatem, or "?" for help, or "/" for current settings
        /// </summary>
        public bool Template_403320c_TPS()
        {
            bool result = false;
            TelnetLib telnetObj = new TelnetLib(_printerIP);
            TraceFactory.Logger.Info("Connecting through telnet");

            if (telnetObj.Connect())
            {
                Thread.Sleep(5000);
                telnetObj.SendLine("/");
                Thread.Sleep(5000);

                telnetObj.ReceiveFile("Press RETURN to continue:");
                TraceFactory.Logger.Info("Checking Telnet connection without password");

                result = (Regex.IsMatch(telnetObj.getValue("passwd"), "Not Specified"));
                TraceFactory.Logger.Info("Telnet connection without password state is:" + result);

                if (AssertThatResult(result, "IsEqualTo", "True"))
                {
                    // Set the password and save it
                    TraceFactory.Logger.Info("Setting username/password = admin/admin");
                    telnetObj.SendLine("passwd admin admin");
                    Thread.Sleep(5000);
                    telnetObj.SendLine("save");
                    Thread.Sleep(20000);
                    telnetObj.SendLine("exit");
                    Thread.Sleep(8000);


                    //Connect again with password
                    if (telnetObj.Connect())
                    {
                        TraceFactory.Logger.Info("Connecting through telnet with password");
                        telnetObj.ReceiveFile("Press RETURN to continue:");

                        result = result && (!(Regex.IsMatch(telnetObj.getValue("passwd"), "Not Specified")));
                        TraceFactory.Logger.Info("Telnet connection with password state is:" + result);

                        if (AssertThatResult(result, "IsEqualTo", "True"))
                        {
                            telnetObj.SendLine("admin");
                            TraceFactory.Logger.Info("Entering Password");

                            Thread.Sleep(5000);
                            string resultAfterPassword = telnetObj.ReceiveFile("Press RETURN to continue:");
                            TraceFactory.Logger.Info("Verifying telnet connection with password");

                            result = result && (resultAfterPassword.Contains(">"));
                            TraceFactory.Logger.Info("Removing password");

                            telnetObj.SendLine("passwd");
                            TraceFactory.Logger.Info("Saving without Password");
                            Thread.Sleep(5000);

                            telnetObj.SendLine("save");
                            Thread.Sleep(20000);

                            telnetObj.SendLine("exit");
                            TraceFactory.Logger.Info("Exiting telnet connection");
                            Thread.Sleep(20000);
                        }
                    }
                }
            }
            return result;

        }

        /// TEMPLATE Verify device parameter configuration using telnet command line interface
        /// <summary>
        ///  Open telnet connection.
        ///  Configure following parameters using the telnet commands:
        ///  sys-location, sys-contact, host-name, domain-name, add, delete, default, add string, delete string,
        ///  bonjour-svc-name, get-cmnty-name, set-cmnty-name, support-contact, support-number, support-url,
        ///  tech-support-url, ipp/ipps-printing, raw-port, syslog-max, syslog-priority, syslog-facility, hoplimit-wsd,
        ///  tcp-mss, tcp-msl, default-ip, default-ip-dhcp, duid, dns-cache-ttl, dhcp-arbitration, panic-behavior,
        ///  link-type, laa, 1000t-ms-conf, 1000t-pause-conf, xml-services-conf, ws-discovery-conf, rtc-enforce,
        ///  printer-dns-svr, sec-dns-svr, printer-wins-svr, sec-wins-svr, allow, syslog-svr, trap-dest
        ///  Save the changes and exit telnet.
        ///  Telnet to the device again and check the configured parameters using / command.
        ///  Expected :
        ///  Configured parameters should get saved and changes should be reflected.
        /// </summary>
        /// 
        public bool Template_403343_TPS()
        {
            bool result = true;
            string[,] TelnetCommands = new string[,] {  {"raw-port", "0"},{"allow","0.0.0.0"},
                {"sys-location", "sysloc"}, {"sys-contact", "syscontact"},
                {"host-name", "hostname"},
                {"defaultq", "RAW"}, {"bonjour-svc-name", "bonjourservice"},
                {"get-cmnty-name", "cmntynameget"}, {"set-cmnty-name", "cmntynameset"},
                {"support-contact", "supportcontact"}, {"support-number", "supportnumber"},
                {"ipp/ipps-printing", "0"},  {"syslog-max", "1"},{"duid", "dd"},
                {"syslog-priority", "5"},
                {"tcp-mss", "1"}, {"tcp-msl", "20"}, {"default-ip", "DEFAULT_IP"},
                {"default-ip-dhcp", "0"},  {"dns-cache-ttl", "50"},
                {"dhcp-arbitration", "6"}, {"panic-behavior", "JUST_HALT"},
                {"link-type", "100AUTO"}, {"1000t-ms-conf", "0"}, {"1000t-pause-conf", "1"},
                {"xml-services-conf", "0"}, {"ws-discovery-conf", "0"}, {"rtc-enforce", "2"},
                {"pri-dns-svr", "16.110.135.52"},{"sec-dns-svr", "16.110.135.51"}, {"pri-wins-svr", "16.238.57.248"},
                {"sec-wins-svr", "16.230.57.248"}, {"allow", IPaddressFetch() }, {"syslog-svr", "2.2.2.2"},
                {"raw-port", "8080"},{"trap-dest", "3.3.3.3"}
                //{"domain-name", "domainname"}, {"syslog-facility", "LPR"}, {"hoplimit-wsd", "200"},
            };


            string[,] validation = new string[,] {   {"System Location", "sysloc"}, {"System Contact", "syscontact"},
                {"Host Name", "hostname"},
                {"LPD Default Queue", "RAW"}, {"Bonjour Svc Name", "bonjourservice"},
                {"Get Cmnty Name", "Specified"}, {"Set Cmnty Name", "Specified"},
                {"Support Contact", "supportcontact"}, {"Phone Number", "supportnumber"},
                {"IPP/IPPS Printing", "0"}, {"Syslog MaxMsg/Min", "1"},
                {"Syslog Priority", "5"},
                {"TCP MSS", "1"}, {"TCP MSL", "20 Seconds"}, {"Default IP", "Default IP"},
                {"Default IP DHCP", "Disabled"},  {"DNS Cache TTL", "50"},
                {"DHCP arbitration", "6"}, {"Panic Behavior", "JUST_HALT"}, {"Link Type", "AUTO"},
                {"1000T Master/Slave", "AUTO"}, {"1000T Pause Frame", "AUTO"},
                {"HP XML Services", "Disabled"}, {"WS Discovery", "Disabled"}, {"RTC Enforce", "2"},
                {"Pri DNS Server", "16.110.135.52"}, {"Sec DNS Serve", "16.110.135.51"},
                {"Pri WINS Server", "16.238.57.248"}, {"Sec WINS Server", "16.230.57.248"},{"DHCP unique ID", "dd"},
                {"Allow", IPaddressFetch()}, {"Syslog Server", "2.2.2.2"}, {"Raw print port", "8080"},
                {"trap-dest", "3.3.3.3"}
                // {"Domain Name", "domainname"},{"Syslog Facility", "LPR"}, {"HopLimit/WSD", "200"},
            };

            TelnetLib TelnetTest = new TelnetLib(_printerIP);



            for (int i = 0; i < (TelnetCommands.Length / 3); i++)
            {
                if (TelnetCommands[i, 0] == "bonjour-svc-name")
                {
                    TelnetTest.Connect();
                    TraceFactory.Logger.Info("Connecting through telnet12000");
                    TelnetTest.SendLine("advanced");

                    TraceFactory.Logger.Info("Runing Command : " + TelnetCommands[i, 0] + " " + TelnetCommands[i, 1]);
                    TelnetTest.SendLine(TelnetCommands[i, 0] + " " + TelnetCommands[i, 1]);
                    TelnetTest.SendLine("save");

                    string status1 = TelnetTest.ReceiveFile("Press RETURN to continue:");
                    TelnetTest.SendLine("Exit");
                    Thread.Sleep(12000);
                    break;
                }
            }



            TelnetTest.Connect();

            TelnetTest.SendLine("/");
            Thread.Sleep(5000);
            string status = TelnetTest.ReceiveFile("Press RETURN to continue:");
            for (int i = 0; i < (validation.Length) / 2; i++)
            {

                result = result && (Regex.IsMatch(TelnetTest.getValue(validation[i, 0]), validation[i, 1]));
                TraceFactory.Logger.Info("Verifying the value for: " + validation[i, 0] + " " + validation[i, 1] + ":" + result);
            }

            // Executing addq, deleteq, addstring, deletestring commands
            TelnetTest.SendLine("addq add");
            TelnetTest.SendLine("addstring stringadded string");
            TelnetTest.SendLine("advanced");
            TelnetTest.SendLine("/");
            Thread.Sleep(5000);
            status = TelnetTest.ReceiveFile("Press RETURN to continue:");
            result = result && (Regex.IsMatch(TelnetTest.getValue("Queue Name"), "add")) &&
                     (Regex.IsMatch(TelnetTest.getValue("String Name"), "stringadded"));
            TelnetTest.SendLine("deleteq add");
            TelnetTest.SendLine("deletestring stringadded");
            TelnetTest.SendLine("advanced");
            TelnetTest.SendLine("/");
            TelnetTest.ReceiveFile("Press RETURN to continue:");

            //Check for the deleted que and deleted string values
            result = result && !(Regex.IsMatch(TelnetTest.getValue("Queue Name"), "add")) &&
                     !(Regex.IsMatch(TelnetTest.getValue("String Name"), "addstring"));
            TelnetTest.SendLine("Exit");
            TelnetTest.SendLine("N");
            Thread.Sleep(5000);
            return result;
        }




        /// TEMPLATE Verity Enable Disable of device parameter using Telnet
        /// <summary>
        /// ID: 403369
        /// Open Telnet connection
        /// Enable/Disable config parameters
        /// Expected :
        /// Configured parameters should get saved and changes should be reflected
        /// </summary>
        public bool Template_403369_TPS()
        {
            bool result = false;

            // Commands to be sent to printer {<command>,<disabled value>,<enabled value>}
            string[,] telnetCommands = new string[,] {{"9100-printing", "0", "1"}, {"ftp-printing", "0", "1"},
                {"ws-printing", "0", "1"}, {"lpd-printing", "0", "1"},
                {"syslog-config", "0", "1"},
                {"slp-config", "0", "1"},
                {"bonjour-services", "2", "6"},{"bonjour-config", "0", "1"},
                {"llmnr", "0", "1"}, {"user-timeout", "500", "7200"}

            };

            // Status displayed after the / command. {<Parameter name>,<Disabled status>, <Enabled Status>}
            string[,] telnetStatus = new string[,] {{"9100-printing", "Disabled", "Enabled"}, {"ftp-printing", "Disabled", "Enabled"},
                {"ws-printing", "Disabled", "Enabled"}, {"lpd-printing", "Disabled", "Enabled"},
                {"syslog-config", "Disabled", "Enabled"},
                {"slp-config", "Disabled", "Enabled"},
                {"bonjour-config", "Disabled", "Enabled"},
                {"llmnr", "Disabled", "Enabled"}, {"user-timeout", "500", "7200"}
            };

            TelnetLib telnetObj = new TelnetLib(_printerIP);
            TraceFactory.Logger.Info("Connecting through telnet");
            if (telnetObj.Connect())
            {
                Thread.Sleep(5000);

                string retreivedResult = telnetObj.ReceiveFile("Press RETURN to continue:");
                result = (retreivedResult.Contains("Type \"help or ?\" for information."));
                telnetObj.SendLine("exit");

                Thread.Sleep(5000);

                // Disable the config parameters

                for (int i = 0; i < (telnetCommands.Length / 3); i++)
                {
                    if (telnetObj.Connect())
                    {
                        TelnetCommandExecution(telnetCommands, telnetObj, i, 1);
                    }
                }


                Thread.Sleep(5000);
                if (telnetObj.Connect())
                {
                    Thread.Sleep(5000);
                    retreivedResult = telnetObj.ReceiveFile("Press RETURN to continue:");
                    result = result && (retreivedResult.Contains("Type \"help or ?\" for information."));
                    if (AssertThatResult(result, "IsEqualTo", "True"))
                    {
                        // Retrieve the values and check if changed have taken effect

                        telnetObj.SendLine("/");
                        Thread.Sleep(5000);
                        retreivedResult = telnetObj.ReceiveFile("Press RETURN to continue:");
                        Thread.Sleep(5000);
                        telnetObj.SendLine("exit");
                        Thread.Sleep(5000);

                        for (int i = 0; i < (telnetStatus.Length / 3); i++)
                        {
                            TraceFactory.Logger.Info("Verifying the value for : " + telnetStatus[i, 0]);
                            result = result && (Regex.IsMatch(telnetObj.getValue(telnetStatus[i, 0]), telnetStatus[i, 1]));
                        }


                        // Enable the config parameters

                        for (int j = 0; j < (telnetCommands.Length / 3); j++)
                        {
                            if (telnetObj.Connect())
                            {
                                TelnetCommandExecution(telnetCommands, telnetObj, j, 2);
                            }

                        }


                        Thread.Sleep(5000);

                        // Retrieve the values and check if changed have taken effect

                        telnetObj.Connect();
                        Thread.Sleep(5000);
                        retreivedResult = telnetObj.ReceiveFile("Press RETURN to continue:");
                        result = result && (retreivedResult.Contains("Type \"help or ?\" for information."));

                        telnetObj.SendLine("/");
                        Thread.Sleep(5000);

                        retreivedResult = telnetObj.ReceiveFile("Press RETURN to continue:");
                        telnetObj.SendLine("exit");
                        Thread.Sleep(3000);

                        for (int k = 0; k < (telnetStatus.Length / 3); k++)
                        {
                            TraceFactory.Logger.Info("Verifying the value for : " + telnetStatus[k, 0]);
                            result = result && (Regex.IsMatch(telnetObj.getValue(telnetStatus[k, 0]), telnetStatus[k, 2]));
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Telnet single Command Execution 
        /// </summary>
        /// <param name="telnetCommands"></param>
        /// <param name="telnetObj"></param>
        /// <param name="i"></param>
        /// <param name="commandsequence"></param>
        private static void TelnetCommandExecution(string[,] telnetCommands, TelnetLib telnetObj, int i, int commandsequence)
        {
            Thread.Sleep(5000);
            TraceFactory.Logger.Info("Running command : " + telnetCommands[i, 0] + " " + telnetCommands[i, commandsequence]);
            telnetObj.SendLine(telnetCommands[i, 0] + " " + telnetCommands[i, commandsequence]);
            Thread.Sleep(5000);

            telnetObj.SendLine("save");
            Thread.Sleep(20000);

            telnetObj.ReceiveFile("Press RETURN to continue:");
            Thread.Sleep(5000);

            telnetObj.SendLine("exit");
            Thread.Sleep(5000);
        }

        /// TEMPLATE Verity telnet disable functionality
        /// <summary>
        /// Web into device go to networking->other settings and disable telnet.
        /// Open telnet command using telnet device ip address.
        /// Expected :
        /// Telnet connection should not get established.
        /// </summary>
        public bool Template_403381_TPS(EwsAdapter adapter)
        {
            bool result = false;
            adapter.Start();

            adapter.Wait(TimeSpan.FromSeconds(5));
            TraceFactory.Logger.Info("Disabling telnet through EWS");

            adapter.Navigate("Advanced");
            adapter.Uncheck("TelnetConfig");
            adapter.Click("Apply");

            adapter.Wait(TimeSpan.FromSeconds(5));

            adapter.Navigate("Advanced");
            result = (!adapter.IsChecked("TelnetConfig"));
            TraceFactory.Logger.Info("Telnet Configuration checbox check is" + result);

            if (AssertThatResult(result, "IsEqualTo", "True"))
            {
                TraceFactory.Logger.Info("Attempt to connect through telnet");
                TelnetLib telnetObj = new TelnetLib(adapter.Settings.DeviceAddress);
                result = result && (!telnetObj.Connect());
                if (AssertThatResult(result, "IsEqualTo", "True"))
                {
                    TraceFactory.Logger.Info("Telnet Connection Status:" + result);
                    TraceFactory.Logger.Info("Enable telnet through EWS");

                    adapter.Navigate("Advanced");
                    adapter.Check("TelnetConfig");
                    adapter.Click("Apply");

                    adapter.Wait(TimeSpan.FromSeconds(5));
                    adapter.Stop();
                    if (telnetObj.Connect())
                    {
                        telnetObj.SendLine("Exit");
                    }
                    else
                    {
                        result = false;
                        TraceFactory.Logger.Info("Telnet Connection failed");
                    }
                }
            }
            Thread.Sleep(5000);
            return result;
        }
        #endregion

        #region Assert Implementation
        #region ComparisonType enum

        /// <summary>
        /// Comparison types used in the AssertThat method
        /// </summary>
        public enum ComparisonType
        {
            /// <summary>
            /// Represents the IsEqualTo comparison
            /// </summary>
            IsEqualTo,
            /// <summary>
            /// Represents the IsNotEqualTo comparison
            /// </summary>
            IsNotEqualTo,
            /// <summary>
            /// Represents the IsGreaterThan comparison
            /// </summary>
            IsGreaterThan,
            /// <summary>
            /// Represents the IsLessThan comparison
            /// </summary>
            IsLessThan,
            /// <summary>
            /// Represents the IsAtLeast comparison
            /// </summary>
            IsAtLeast,
            /// <summary>
            /// Represents the IsAtMost comparison
            /// </summary>
            IsAtMost,
            /// <summary>
            /// Represents the Contains comparison
            /// </summary>
            Contains,
            /// <summary>
            /// Represents the NotContains comparison
            /// </summary>
            NotContains,
            /// <summary>
            /// Represents the StartsWith comparison
            /// </summary>
            StartsWith,
        }

        #endregion
        /// <summary>
        /// Get the value of the control and compare with the expected value, returning the equality.
        /// The value of the control follow the same rules of GetValue.
        /// - Textboxes return the text value of the element. 
        /// - List elements (dropdownlist, listboxes, radiobuttonlist) return the index (start at 0) of the selected element. In case more than one item is selected, return a string with all indexes separated with commas.
        /// - Single Checkbox or Radiobutton return 'True' or 'False' for the checked attribute.
        /// - Labels, spans and divs return the InnerHTML of the element.
        /// <example>EWS.Verify("Page", "Control", "2")</example>
        /// </summary>
        /// <param name="value">Value that will be manipulated in the method to be value.</param>
        /// <param name="expected">Expected value to be compared with the return of the element</param>
        /// <param name="predicate"></param>
        public bool AssertThatResult(object value, string predicate, string expected)
        {
            bool result = false;
            TraceFactory.LogDebugResult("AssertThat", string.Format("Asserting that '{0}' {1} '{2}'.", value, predicate, expected));

            ComparisonType compType = ComparisonType.IsEqualTo;
            if (Enum.IsDefined(typeof(ComparisonType), predicate))
                compType = (ComparisonType)Enum.Parse(typeof(ComparisonType), predicate);
            else
                throw new ArgumentException("Invalid entered predicate on AssertThat", predicate);

            if (value == null)
                value = string.Empty;
            if (expected == null)
                expected = string.Empty;

            if (value != null && expected != null)
            {
                float nValue = 0;
                float.TryParse(value.ToString(), out nValue);
                float nExpected = 0;
                float.TryParse(expected, out nExpected);

                switch (compType)
                {
                    case ComparisonType.IsEqualTo:
                        result = value.ToString() == expected;
                        break;
                    case ComparisonType.IsNotEqualTo:
                        result = value.ToString().ToUpperInvariant() != expected.ToUpperInvariant();
                        break;
                    case ComparisonType.IsGreaterThan:
                        result = nValue > nExpected;
                        break;
                    case ComparisonType.IsLessThan:
                        result = nValue < nExpected;
                        break;
                    case ComparisonType.IsAtLeast:
                        result = nValue >= nExpected;
                        break;
                    case ComparisonType.IsAtMost:
                        result = nValue <= nExpected;
                        break;
                    case ComparisonType.Contains:
                        result = value.ToString().ToUpperInvariant().Contains(expected.ToUpperInvariant());
                        break;
                    case ComparisonType.NotContains:
                        result = !value.ToString().ToUpperInvariant().Contains(expected.ToUpperInvariant());
                        break;
                    case ComparisonType.StartsWith:
                        result = value.ToString().ToUpperInvariant().StartsWith(expected.ToUpperInvariant());
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Get the value of the control and compare with the expected value, returning the equality.
        /// The value of the control follow the same rules of GetValue.
        /// - Textboxes return the text value of the element. 
        /// - List elements (dropdownlist, listboxes, radiobuttonlist) return the index (start at 0) of the selected element. In case more than one item is selected, return a string with all indexes separated with commas.
        /// - Single Checkbox or Radiobutton return 'True' or 'False' for the checked attribute.
        /// - Labels, spans and divs return the InnerHTML of the element.
        /// <example>EWS.Verify("Page", "Control", "2")</example>
        /// </summary>
        /// <param name="actual">Value that will be manipulated in the method to be value.</param>
        /// <param name="expected">Expected value to be compared with the return of the element</param>
        /// <param name="predicate">IsEqualTo, Contains etc..</param>
        /// <param name="searchParameter">The control to be evaluated.Optional parameter..Can be used in web UI Operations</param>
        public void AssertThat(object actual, string predicate, object expected, string searchParameter = null)
        {
            if (!AssertThatResult(actual.ToString(), predicate, expected.ToString()))
            {
                if (searchParameter != null)
                    ThrowException("Verify", string.Format("Verify failed for :{0}. Expected: {1}; Actual: {2}; ComparisonType: {3}",
            searchParameter, expected, actual, predicate));
                else
                    ThrowException("Verify", string.Format("Verify failed. Expected: {0}; Actual: {1}; ComparisonType: {2}",
            expected, actual, predicate));
            }
        }

        /// <summary>
        /// Throw a exception and stop the test. Also create a html log of the page in the output folder.
        /// </summary>
        /// <param name="method">Name of the method to be used in the CTF logger.</param>
        /// <param name="message">Message to be used in the CTF logger and thrown in the exception.</param>        
        public void ThrowException(string method, string message)
        {
            ThrowException(method, message, GetType().Name, null);
        }

        /// <summary>
        /// Throw a exception and stop the test. Also create a html log of the page in the output folder.
        /// </summary>
        /// <param name="method">Name of the method to be used in the CTF logger.</param>
        /// <param name="message">Message to be used in the CTF logger and thrown in the exception.</param>
        /// <param name="className">Class that is throwing the exception.</param>
        /// <param name="ex">The exception that was orignaly thrown.</param>
        public static void ThrowException(string method, string message, string className, Exception ex)
        {
            string InnerHtml = "Exception info: " + method + " - " + message + Environment.NewLine;
            TraceFactory.LogDebugResult(InnerHtml, null);
            throw new System.Exception(message, ex);
        }


        #endregion
    }
}
