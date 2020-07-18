using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.NativeApps.NetworkFolder;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Dhcp;
using HP.ScalableTest.PluginSupport.Connectivity.DnsApp;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.KiwiSyslog;
using HP.ScalableTest.PluginSupport.Connectivity.NetworkSwitch;
using HP.ScalableTest.PluginSupport.Connectivity.PacketCapture;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using HP.ScalableTest.PluginSupport.Connectivity.Wins;
using HP.ScalableTest.Utility;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.NetworkNamingServices
{
    /// <summary>
    /// Different Printer Accessibility
    /// </summary>
    public enum PrinterAccessType
    {
        /// <summary>
        /// Web UI
        /// </summary>
        EWS,
        /// <summary>
        /// Telnet
        /// </summary>
        Telnet,
        /// <summary>
        /// Snmp
        /// </summary>
        SNMP
    }

    public static class NetworkNamingServiceTemplates
    {
        #region Constants

        private const string SERVER_DOMAIN = "ctc{0}.automation.com";
        private const string PTR_QUERY = "{0}.in-addr.arpa";
        private const string PACKETS_LOCATION = @"C:\Packets";
        private const string VENDOR_OPTION = "option vendor code 153=boolean;";
        private const string VENDOR_STATUS = "option vendor {0};";
        private const string QUICK_SET_NAME = "CTC-Automation";
        private const string SHARE_FILE_PATH = @"C:\Share\";
        private const string PREFIX = "[Untitled]";
        #endregion

        #region WINS

        /// <summary>
        /// 698315	
        /// TEMPLATE  Verify Device registers with Primary wins server			
        /// Verify Printer registers with Primary wins server	
        /// "1.Power on device.
        /// 2.Configure the device with the primary WINS server.
        /// 3.Power cycle the device.
        /// 4.Wait until the device is ready.
        /// 5.Check the primary WINS server log and verify that device successfully registered.
        /// 6.Verify name, type of request, and IP address."	
        /// Expected: 
        /// 1.The primary WINS server log should contain both the device's host name and IP address
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>
        /// <returns>True if the template is successfully executed, else false.</returns>
        public static bool VerifyPrimaryWinsServerRegistration(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!WinsPrerequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                return VerifyWinsRegistration(activityData);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                client.Channel.Stop(guid);
                WinsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// 702609	
        /// TEMPLATE Verify Device registers with Secondary wins server			
        /// Verify Printer registers with Secondary wins server	
        /// "1.Power on device.
        /// 2.Configure the primary WINS server to a server that does not have the WINS services running.
        /// 3.Configure the secondary WINS server to a valid WINS server.
        /// 4.Power cycle the device.
        /// 5.Wait until the device is ready.
        /// 6.Check the secondary WINS server log and verify that the device successfully registered.
        /// 7.Verify name, type of request, and IP address."	
        /// Expected:
        /// "1.The secondary WINS server log should contain both the device's host name and IP address."
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>
        /// <returns>True if the template is successfully executed, else false.</returns>
        public static bool VerifySecondaryWinsServerRegistration(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!WinsPrerequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.SecondDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                return VerifyWinsRegistration(activityData, false);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                client.Channel.Stop(guid);
                WinsPostrequisites(activityData);

            }
        }

        /// <summary>
        /// 702628	
        /// TEMPLATE Verify that WINS responds to broadcast name query when primary WINS server is not configured and name in query matches device name			
        /// Verify that WINS responds to broadcast name query when primary WINS server is not configured and name in query matches device name	
        /// "1.Power on device.
        /// 2.Configure a host name on the device using Telnet (for example: ""jd087"").dont configure wins server ip in the device and wins server should run.
        /// Disable LLMNR on Device 
        /// 3.Ping or Telnet to the device name (for example: ""jd087""). (dont run WINS SERVER)"	
        /// Expected:
        /// "1.Verify that there was a reply to the ""Ping Device Name
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>
        /// <returns>True if the template is successfully executed, else false.</returns>
        public static bool VerifyWinsBroadcastNameQuery(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!WinsPrerequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                string hostName = CtcUtility.GetUniqueHostName();
                if (activityData.ProductFamily == ProductFamilies.InkJet.ToString())
                {
                    EwsWrapper.Instance().SetHostname(hostName);
                }
                else
                {
                    TelnetWrapper.Instance().SetHostname(hostName);
                }
                if (!EwsWrapper.Instance().GetHostname().EqualsIgnoreCase(hostName))
                {
                    TraceFactory.Logger.Info("Failed to set hostname through telnet");
                    return false;
                }

                // Clear WINS server IP address
                TraceFactory.Logger.Info("Clear WINS server values");
                if (!(activityData.ProductFamily == ProductFamilies.TPS.ToString()))
                {
                    if (!EwsWrapper.Instance().SetPrimaryWinServerIP(string.Empty))
                    {
                        return false;
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("Since TPS does not accpet 0.0.0.0 or blank value need to do restore default ");
                    EwsWrapper.Instance().ResetConfigPrecedence();

                }


                if (!StartWinsService(activityData.PrimaryDhcpServerIPAddress))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetLlmnr(false))
                {
                    return false;
                }

                string reply = ProcessUtil.Execute("cmd.exe", "/C ping -a {0}".FormatWith(hostName)).StandardOutput;

                TraceFactory.Logger.Info(reply);

                if (reply.Contains(hostName, StringComparison.CurrentCultureIgnoreCase) && reply.Contains(activityData.WiredIPv4Address, StringComparison.CurrentCultureIgnoreCase))
                {
                    TraceFactory.Logger.Info("Ping reply contains the host name: {0} and IP address: {1}".FormatWith(hostName, activityData.WiredIPv4Address));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Ping reply doesn't contain the host name: {0} and IP address: {1}".FormatWith(hostName, activityData.WiredIPv4Address));
                    return false;
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                client.Channel.Stop(guid);
                WinsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// 702629	
        /// TEMPLATE Verify that WINS responds to unicast node status request from a different subnet when primary WINS server is configured (P-NODE)			
        /// Verify that WINS responds to unicast node status request from a different subnet with “*” when primary WINS server is configured (P-NODE)	
        /// "1.Power on device.
        /// 2.Configure the device with the primary WINS server.
        /// 3.Using the DOS command prompt, enter ""ping  -a Device_Ip_Address""
        /// 4.Verify that ping returns a ""Pinging Host_Name IP_Address"", for example, ""Pinging jd087 13.32.9.114"" (the host name must be present in the reply)."	
        /// Expected:
        /// 1.The reply from the "ping -1 Device_Ip_Address" should include the device host name
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>
        /// <returns>True if the template is successfully executed, else false.</returns>
        public static bool VerifyPNodeDifferentSubnet(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!WinsPrerequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Configuring primary WINS server on different subnet.");

                if (!EwsWrapper.Instance().SetPrimaryWinServerIP(activityData.SecondDhcpServerIPAddress))
                {
                    return false;
                }

                string hostName = EwsWrapper.Instance().GetHostname();

                string reply = ProcessUtil.Execute("cmd.exe", "/C ping -a {0}".FormatWith(activityData.WiredIPv4Address)).StandardOutput;

                TraceFactory.Logger.Info(reply);

                if (reply.Contains(hostName, StringComparison.CurrentCultureIgnoreCase) && reply.Contains(activityData.WiredIPv4Address, StringComparison.CurrentCultureIgnoreCase))
                {
                    TraceFactory.Logger.Info("Ping reply contains the host name: {0} and IP address: {1}".FormatWith(hostName, activityData.WiredIPv4Address));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Ping reply doesn't contain the host name: {0} and IP address: {1}".FormatWith(hostName, activityData.WiredIPv4Address));
                    return false;
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                client.Channel.Stop(guid);
                WinsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// 702630	
        /// TEMPLATE Verify WINS response to a node status request when primary WINS server is configured  (P-NODE)			
        ///	Verify WINS response to a node status request when primary WINS server is configured  (P-NODE)	
        ///	"1.Power on device.
        /// 2.Configure a device with a host name and a valid primary WINS server.
        /// 3.At the DOS prompt, enter the command ""ping -a IpAddress"" where the IpAddress is the address of the device.
        /// 4.Verify that the reply has both the host name and the IP address of the device"	
        /// 1.The "ping -a IpAddress" should return both the host name and IP address of the device under test
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>
        /// <returns>True if the template is successfully executed, else false.</returns>
        public static bool VerifyPNodeSameSubnet(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!WinsPrerequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetPrimaryWinServerIP(activityData.PrimaryDhcpServerIPAddress))
                {
                    return false;
                }

                string hostName = EwsWrapper.Instance().GetHostname();

                string reply = ProcessUtil.Execute("cmd.exe", "/C ping -a {0}".FormatWith(activityData.WiredIPv4Address)).StandardOutput;

                TraceFactory.Logger.Info(reply);

                if (reply.Contains(hostName, StringComparison.CurrentCultureIgnoreCase) && reply.Contains(activityData.WiredIPv4Address, StringComparison.CurrentCultureIgnoreCase))
                {
                    TraceFactory.Logger.Info("Ping reply contains the host name: {0} and IP address: {1}".FormatWith(hostName, activityData.WiredIPv4Address));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Ping reply doesn't contain the host name: {0} and IP address: {1}".FormatWith(hostName, activityData.WiredIPv4Address));
                    return false;
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                client.Channel.Stop(guid);
                WinsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// 702633	
        /// TEMPLATE Hostname uniqueness verification wrt WINS (primary)			
        ///	Verify negative response from server accepted (primary)	
        /// "1.Power on device.
        /// 2.Configure a device with the primary WINS server and let it register.
        /// 3.Configure a 2nd device or WINS device with a unique IP address and the same host name as the primary WINS server (as the first device).  
        /// 4.Check the log on the WINS server and verify that the 2nd device under test has reported an error."
        /// Expected:
        /// "1.The 2nd device should log an error message"
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>
        /// <returns>True if the template is successfully executed, else false.</returns>
        public static bool VerifyHostNameUniquenessPrimaryServer(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!WinsPrerequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!VerifyWinsRegistration(activityData))
                {
                    return false;
                }

                string hostName = EwsWrapper.Instance().GetHostname();
                //CTSS
                // Assigning the same host name to a different printer
                if (!((activityData.ProductFamily == ProductFamilies.InkJet.ToString()) || (activityData.ProductFamily == ProductFamilies.TPS.ToString())))
                {
                    // Assigning the same host name to a different printer
                    TelnetWrapper.Instance().Create(activityData.SecondPrinterIPAddress);
                    TelnetWrapper.Instance().SetHostname(hostName);
                }
                else
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.SecondPrinterIPAddress);
                    EwsWrapper.Instance().SetHostname(hostName);
                }
                Thread.Sleep(TimeSpan.FromMinutes(1));

                using (WinsApplicationServiceClient winsClient = WinsApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    if (winsClient.Channel.ValidateWinServerLog(activityData.PrimaryDhcpServerIPAddress, hostName, "00", activityData.SecondPrinterIPAddress))
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Primary WINS server log contains device host name: {0} and IP address: {1}".FormatWith(hostName, activityData.SecondPrinterIPAddress));
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Primary WINS server log doesn't contain device host name: {0} and IP address: {1}".FormatWith(hostName, activityData.SecondPrinterIPAddress));
                        return true;
                    }
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                client.Channel.Stop(guid);
                if (!(activityData.ProductFamily == ProductFamilies.InkJet.ToString()))
                {
                    // Perform cold reset on the second printer to clear the duplicate hostname
                    Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.SecondPrinterIPAddress);
                    printer.PowerCycle();

                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                    //CTSS -remove this if 
                    if (!(activityData.ProductFamily == ProductFamilies.TPS.ToString()))
                    {
                        TelnetWrapper.Instance().Create(activityData.WiredIPv4Address);
                    }
                }
                WinsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// 702634	
        /// TEMPLATE Hostname uniqueness verification wrt WINS (secondary)			
        /// Verify negative response from server accepted (secondary)	
        /// "1.Power on device.
        /// 2.Configure a device with a primary WINS server that does not exist (a valid IP address of a host configured with it on the same subnet).
        /// 3.Configure the same device with the secondary WINS server and let it register.
        /// 4.Configure a 2nd device under test with a primary WINS server that does not exist (a valid IP address on the same subnet but without a server).
        /// 5.Configure the 2nd device under test with the same host name and secondary WINS server as the first one (device).  
        /// 6.Verify that device logs an error message to Syslog and the configuration page."
        /// Expected: 
        /// "1. 2nd Device should send an error message and continue using it's host name"
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyHostNameUniquenessSecondaryServer(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!WinsPrerequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string hostName = EwsWrapper.Instance().GetHostname();

                if (!VerifyWinsRegistration(activityData, false))
                {
                    return false;
                }

                // Configure second printer
                // Assigning the same host name to a different printer
                TelnetWrapper.Instance().Create(activityData.SecondPrinterIPAddress);
                TelnetWrapper.Instance().SetHostname(hostName);

                Thread.Sleep(TimeSpan.FromMinutes(1));

                using (WinsApplicationServiceClient winsClient = WinsApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress))
                {
                    if (winsClient.Channel.ValidateWinServerLog(activityData.PrimaryDhcpServerIPAddress, hostName, "00", activityData.SecondPrinterIPAddress))
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Secondary WINS server log contains device host name: {0} and IP address: {1}".FormatWith(hostName, activityData.SecondPrinterIPAddress));
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Secondary WINS server log doesn't contain device host name: {0} and IP address: {1}".FormatWith(hostName, activityData.SecondPrinterIPAddress));
                        return true;
                    }
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                client.Channel.Stop(guid);
                // Perform cold reset on the second printer to clear the duplicate hostname
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.SecondPrinterIPAddress);
                printer.PowerCycle();

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                TelnetWrapper.Instance().Create(activityData.WiredIPv4Address);

                WinsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// 702641	
        /// TEMPLATE Verify retry to primary WINS server			
        /// Verify retry to primary WINS server	
        /// "1.Power on device.
        /// 2.Configure the device under test with a primary WINS server with an address of a host on the local subnet but without a running WINS server
        /// 3.Configure the device under test with a valid secondary WINS server and verify that it successfully registers (check the secondary WINS server log file).
        /// 4.Start a LAN trace.
        /// 5.Shut down the secondary WINS server.
        /// 6.After 36 minutes, capture the trace.
        /// 7.Observe the trace and verify that after three unsuccessful refresh attempts to the secondary WINS server, the next refresh request was sent to the primary WINS server."	
        /// Expected:
        /// "1.After 1/2 TTL, 3 refresh attempts should occur to the Secondary WINS server. No activity should occur for about 15 minutes and then three registration attempts should occur with the primary server"
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyRetryToPrimaryWinsServer(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!WinsPrerequisites(activityData))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                using (WinsApplicationServiceClient winsClient = WinsApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    if (winsClient.Channel.StopWinsService())
                    {
                        TraceFactory.Logger.Info("Successfully stopped WINS service on primary WINS server: {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to stop WINS service on primary WINS server: {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                        return false;
                    }
                }

                if (!VerifyWinsRegistration(activityData, false, activityData.PrimaryDhcpServerIPAddress))
                {
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to validate the entries in wins server");
                    return false;
                }

                using (WinsApplicationServiceClient winsClient = WinsApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress))
                {
                    if (winsClient.Channel.StopWinsService())
                    {
                        TraceFactory.Logger.Info("Successfully stopped WINS service on second WINS server: {0}.".FormatWith(activityData.SecondDhcpServerIPAddress));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to stop WINS service on second WINS server: {0}.".FormatWith(activityData.SecondDhcpServerIPAddress));
                        return false;
                    }
                }

                PacketCaptureServiceClient primaryClient = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                string primaryGuid = primaryClient.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

                PacketCaptureServiceClient secondaryClient = PacketCaptureServiceClient.Create(activityData.SecondDhcpServerIPAddress);
                string secondaryGuid = secondaryClient.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

                // Wait for 36 minutes
                Thread.Sleep(TimeSpan.FromMinutes(36));

                PacketDetails details = primaryClient.Channel.Stop(primaryGuid);

                string packetsTime = primaryClient.Channel.GetPacketData(details.PacketsLocation, "nbns && ip.src == {0}".FormatWith(activityData.WiredIPv4Address), "Arrival Time:");

                string errorInfo = string.Empty;

                // Validate refresh message for each of the packet to confirm that only requests are sent to the server.
                foreach (var time in packetsTime.Replace("Arrival Time:", string.Empty).Trim().Split('|'))
                {
                    string cleanTime = time.Replace("India Standard Time", string.Empty).Trim();

                    // when it is send thru the command line arguments need to append the excape sequence.
                    if (!primaryClient.Channel.Validate(details.PacketsLocation, "nbns && ip.src == {0} && frame.time == \\\"{1}\\\"".FormatWith(activityData.WiredIPv4Address, cleanTime), ref errorInfo, "(Refresh)"))
                    {
                        TraceFactory.Logger.Info(errorInfo);
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to validate the retry packets");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Refresh attempts to the server: {0} occured.".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                    }
                }

                details = secondaryClient.Channel.Stop(secondaryGuid);

                // Validate refresh message for each of the packet to confirm that only requests are sent to the server.
                foreach (var time in packetsTime.Replace("Arrival Time:", string.Empty).Trim().Split('|'))
                {
                    string cleanTime = time.Replace("India Standard Time", string.Empty).Trim();

                    if (!primaryClient.Channel.Validate(details.PacketsLocation, "nbns && ip.src == {0} && frame.time == \\\"{1}\\\"".FormatWith(activityData.WiredIPv4Address, cleanTime), ref errorInfo, "(Refresh)"))
                    {
                        TraceFactory.Logger.Info(errorInfo);
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to validate the retry packets");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Refresh attempts to the server: {0} occurred.".FormatWith(activityData.SecondDhcpServerIPAddress));
                    }
                }

                return true;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                WinsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// 702610	
        /// TEMPLATE Verify device reregisters with WINS  after change in parameters like host name, IP address and Subnet mask.			
        /// Verify WINS behavior after a subnet-mask change	
        /// STEP I:
        /// "1.Power on device.
        /// 2.Configure a device with a non-default host name and a valid primary WINS server.
        /// 3.Verify that this device successfully registered with the WINS server.
        /// 4.Enable Syslog on the Jetdirect.
        /// 5.Telnet into the device and change the subnet mask.
        /// 6.Check Syslog and verify that no registration entries are present."	
        /// Expected:
        /// "1.The WINS application should not send out any new registration packets"
        /// STEP II
        /// Verify WINS behavior after a IP address change	
        /// "1.Power on device.
        /// 2.Configure a Jetdirect with a non-default host name and a valid primary WINS server.
        /// 3.Verify that this Jetdirect successfully registered with the WINS server.
        /// 4.Telnet into the Jetdirect and change the IP address.
        /// 5.Check the WINS log file and verify that the Jetdirect reregistered with the new IP address and the same host name  "	
        /// Expected: 
        /// "The WINS server log file should show that the Jetdirect has the same host name associated with the new IP address"
        /// STEP III
        /// Verify device reregisters with WINS after host name change	
        /// "1.Power on device.
        /// 2.Configure the device under test with a valid primary WINS server.
        /// 3.Check the WINS server log file and verify that the device correctly registered. 
        /// 4.Telnet into the device and change the host name.
        /// 5.Check the WINS server log file and verify that device reregistered with the new host name and same IP address"	
        /// Expected:
        /// "1.The WINS server log file should show that the new host name is registered"
        /// Arul input:Step1-after changing subnetmask registration entries are present in syslog,he need to modify test case
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyDeviceRegistrationWithParameterChange(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!WinsPrerequisites(activityData))
            {
                return false;
            }

            IPAddress currentDeviceAddress = IPAddress.Parse(activityData.WiredIPv4Address);

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Verify WINS behavior after a subnet-mask change"));

                if (!VerifyWinsRegistration(activityData))
                {
                    return false;
                }

                DateTime startTime = DateTime.Now;

                if (!EwsWrapper.Instance().SetSyslogServer(activityData.PrimaryDhcpServerIPAddress))
                {
                    return false;
                }

                // Change the configuration method to Manual. 
                // VEP - changing subnet mask thru telnet it will automatically changes the config method to manul
                // TPS - Need to change the config method to manual then only it will allow to change the subnet mask.
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual);

                if (!TelnetWrapper.Instance().SetSubnetMask((PrinterFamilies)Enum<PrinterFamilies>.Parse(activityData.ProductFamily, true), "255.255.254.0"))
                {
                    return false;
                }

                DateTime endTime = DateTime.Now;

                string hostName = EwsWrapper.Instance().GetHostname();

                using (KiwiSyslogApplicationServiceClient syslogClient = KiwiSyslogApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    if (activityData.ProductFamily == ProductFamilies.TPS.ToString())
                    {
                        if (syslogClient.Channel.ValidateEntry(startTime, endTime, "{0}\tprinter: registered system name {1} with WINS server {2}".FormatWith(activityData.WiredIPv4Address, hostName.ToUpper(CultureInfo.CurrentCulture), activityData.PrimaryDhcpServerIPAddress)))
                        {
                            TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Registration entries are present in the Syslog which is not expected");
                            return false;
                        }
                        else
                        {
                            TraceFactory.Logger.Info("No registration entries are present in the Syslog after changing subnet mask as expected");
                        }
                    }
                    else
                    {
                        if (syslogClient.Channel.ValidateEntry(startTime, endTime, "{0}\tprinter: registered system name {1} with WINS server {2}".FormatWith(activityData.WiredIPv4Address, hostName.ToUpper(CultureInfo.CurrentCulture), activityData.PrimaryDhcpServerIPAddress)))
                        {
                            TraceFactory.Logger.Info("Registration entries are present in the Syslog");
                        }
                        else
                        {
                            TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "No registration entries are present in the Syslog after changing subnet mask");
                            return false;
                        }
                    }
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Verify WINS behavior after a IP address change"));
                currentDeviceAddress = NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.WiredIPv4Address));
                //CTSS
                //Telnet is not supported in INK so setting config method from EWS
                if (!((activityData.ProductFamily == ProductFamilies.InkJet.ToString()) || (activityData.ProductFamily == ProductFamilies.InkJet.ToString())))
                {
                    if (!TelnetWrapper.Instance().SetManualIPAddress((PrinterFamilies)Enum<PrinterFamilies>.Parse(activityData.ProductFamily, true), currentDeviceAddress.ToString()))
                    {
                        return false;
                    }
                }
                else
                {
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual);
                }


                Thread.Sleep(TimeSpan.FromMinutes(5));

                EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);

                hostName = EwsWrapper.Instance().GetHostname();

                //Manual Ip address is not registered on the wins server so resetting the primary dns server
                EwsWrapper.Instance().SetPrimaryDnsServer(string.Empty);
                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);
                Thread.Sleep(TimeSpan.FromSeconds(30));

                using (WinsApplicationServiceClient winsClient = WinsApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    if (winsClient.Channel.ValidateWinServerLog(activityData.PrimaryDhcpServerIPAddress, hostName, "00", currentDeviceAddress.ToString()))
                    {
                        TraceFactory.Logger.Info("WINS server log contains device host name: {0} and IP address: {1}".FormatWith(hostName, currentDeviceAddress.ToString()));
                    }
                    else
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "WINS server log does not contain device host name: {0} and IP address: {1}".FormatWith(hostName, currentDeviceAddress.ToString()));
                        return false;
                    }
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step III: Verify device reregisters with WINS after host name change"));

                hostName = CtcUtility.GetUniqueHostName();

                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

                using (WinsApplicationServiceClient winsClient = WinsApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    if (winsClient.Channel.ValidateWinServerLog(activityData.PrimaryDhcpServerIPAddress, hostName, "00", currentDeviceAddress.ToString()))
                    {
                        TraceFactory.Logger.Info("WINS server log contains device host name: {0} and IP address: {1}".FormatWith(hostName, currentDeviceAddress.ToString()));
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "WINS server log dose not contain device host name: {0} and IP address: {1}".FormatWith(hostName, currentDeviceAddress.ToString()));
                        return false;
                    }
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                client.Channel.Stop(guid);

                EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);

                WinsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// 702611	
        /// TEMPLATE Verify Syslog message for successful WINS registration			
        /// STEP I: Verify Syslog message for successful registration	
        /// "1.Power on device.
        /// 2.Configure the device under test with a valid primary WINS server.
        /// 3.Configure the device under test with a Syslog server.
        /// 4.Power cycle the device.
        /// 5.Check the Syslog and verify that the registration was successful."	
        /// Expected:
        /// 1.The Syslog entry should indicate that the device was successful at registration
        /// STEP II: Verify Syslog message for unsuccessful registration	
        /// "1.Power on device.
        /// 2.Configure the device under test with a primary WINS with an address of a host on the local subnet but without a running WINS server.
        /// 3.Configure the device under test with a secondary WINS server with an address of a host on the local subnet but without a running WINS server.
        /// 3.Configure Syslog.
        /// 4.Power cycle the device.
        /// 5.Check Syslog errors."
        /// Expected:
        /// 1.The Syslog entry should indicate that the device was unsuccessful at registration.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNO"></param>
        /// <returns></returns>
        public static bool VerifySyslogForWinsRegistration(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!WinsPrerequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Verify Syslog message for successful registration"));

                if (!EwsWrapper.Instance().SetPrimaryWinServerIP(activityData.PrimaryDhcpServerIPAddress))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetSyslogServer(activityData.PrimaryDhcpServerIPAddress))
                {
                    return false;
                }

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                string hostName = EwsWrapper.Instance().GetHostname();

                DateTime startTime = DateTime.Now;

                printer.PowerCycle();

                DateTime endTime = DateTime.Now;

                TraceFactory.Logger.Info("Validating Syslog server log.");

                using (KiwiSyslogApplicationServiceClient syslogClient = KiwiSyslogApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    if (syslogClient.Channel.ValidateEntry(startTime, endTime, "{0}\tprinter: registered system name {1} with WINS server {2}".FormatWith(activityData.WiredIPv4Address, hostName.ToUpper(CultureInfo.CurrentCulture), activityData.PrimaryDhcpServerIPAddress)))
                    {
                        TraceFactory.Logger.Info("Registration entries are present in the Syslog.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "No registration entries are present in the Syslog.");
                        return false;
                    }
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Verify Syslog message for unsuccessful registration"));

                if (!EwsWrapper.Instance().SetPrimaryWinServerIP(activityData.PrimaryDhcpServerIPAddress))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetSecondaryWinServerIP(activityData.SecondDhcpServerIPAddress))
                {
                    return false;
                }

                if (!StopWinsService(activityData.PrimaryDhcpServerIPAddress) || !StopWinsService(activityData.SecondDhcpServerIPAddress))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetSyslogServer(activityData.PrimaryDhcpServerIPAddress))
                {
                    return false;
                }

                startTime = DateTime.Now;

                printer.PowerCycle();

                endTime = DateTime.Now;

                TraceFactory.Logger.Info("Validating Syslog server log.");

                using (KiwiSyslogApplicationServiceClient syslogClient = KiwiSyslogApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    // since the WINS service is stopped it should not be registered in the Syslog server
                    if (!syslogClient.Channel.ValidateEntry(startTime, endTime, "{0}\tprinter: registered system name {1} with WINS server {2}".FormatWith(activityData.WiredIPv4Address, hostName.ToUpper(CultureInfo.CurrentCulture), activityData.PrimaryDhcpServerIPAddress)))
                    {
                        TraceFactory.Logger.Info("No registration entries are present in the Syslog as expected.");
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Registration entries are present in the Syslog which is not expected.");
                        return false;
                    }
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                client.Channel.Stop(guid);

                WinsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// 678968	
        /// Verify the Syslog capture with respect to wired and wireless interfaces			
        /// Syslog capture with respect to wired or wireless interfaces
        /// Bring up the device Configure the Syslog to the device in wired interfaces
        /// Verify the Syslog is configured automatically to the wireless interface also.
        /// Do some changes like hostname or domain name or ip address to the device on wired or wireless interfaces
        /// Verify the Syslog capture in wired interfaces
        /// Verify the Syslog capture also with repest to wireless interfaces also  	
        /// "3. Syslog should be configured automatically in the wireless interfaces also
        /// 5. Hostname or domain name or IP address changes should be captured with respect to the interface modifications"
        /// Arul input: Change in Domain name doesnot register the printer on the syslog
        /// so only hostname validation is required
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifySyslogWiredWireless(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!WinsPrerequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

            IPAddress wiredManualAddress = IPAddress.None;
            IPAddress wirelessManualAddress = IPAddress.None;

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Configuring Syslog for the wired interface.");

                if (!EwsWrapper.Instance().SetSyslogServer(activityData.PrimaryDhcpServerIPAddress))
                {
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WirelessIPv4Address);

                if (EwsWrapper.Instance().GetSyslogServer().EqualsIgnoreCase(activityData.PrimaryDhcpServerIPAddress))
                {
                    TraceFactory.Logger.Info("Syslog is automatically configured for the wireless interface.");
                }
                else
                {
                    TraceFactory.Logger.Info("Syslog is NOT automatically configured for the wireless interface.");
                    return false;
                }

                TraceFactory.Logger.Info("Validating Syslog for Parameter Change-hostname on the Wired interface");
                if (!VerifySyslogForParameterChanges(activityData.PrimaryDhcpServerIPAddress, activityData.WiredIPv4Address))
                {
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to validate the entries in syslog application");
                    return false;
                }

                TraceFactory.Logger.Info("Validating Syslog for Parameter Change-hostname on the Wireless interface");
                return VerifySyslogForParameterChanges(activityData.PrimaryDhcpServerIPAddress, activityData.WirelessIPv4Address);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                client.Channel.Stop(guid);

                // Change wired and wireless Manual IP to DHCP IP.
                if (IPAddress.None != wiredManualAddress)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(wiredManualAddress);
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));
                }

                if (IPAddress.None != wirelessManualAddress)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(wirelessManualAddress);
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WirelessIPv4Address));
                }

                WinsPostrequisites(activityData);
            }
        }

        #endregion

        #region DNS

        /// <summary>
        /// 75405	
        /// Configure DNS suffix using  manual entries Wired	
        /// Check the DNS suffix configuration from Web.	
        /// Configure DNS suffix from the web.	
        /// Expected:
        /// The DNS suffixes should be saved successfully and name resolution should be successful.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyManualDnsSuffix(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            string hostName = string.Empty;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetDNSSuffixList(GetDomainName(activityData.PrimaryDhcpServerIPAddress)))
                {
                    return false;
                }

                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);

                hostName = EwsWrapper.Instance().GetHostname();

                using (DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    if (dnsClient.Channel.AddRecord(GetDomainName(activityData.PrimaryDhcpServerIPAddress), hostName, "A", activityData.WiredIPv4Address))
                    {
                        TraceFactory.Logger.Info("Successfully added A record in DNS server.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to add A record in DNS server.");
                        return false;
                    }
                }

                string pingReply = ProcessUtil.Execute("cmd.exe", "/C ping -a {0}".FormatWith(hostName)).StandardOutput;

                TraceFactory.Logger.Info(pingReply);

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                return VerifyPingReply(hostName, pingReply, printer);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                client.Channel.Stop(guid);

                DnsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// 75409	
        /// Using Invalid DNS suffixes entries	
        /// Step I:Using Invalid entries	
        /// "1. Goto WebUI->Networking->TCP/IP Settings->Network Identification 
        /// 2. Enter few DNS Suffixes with @!~ characters"	
        /// Expected: It should not allow any junk characters
        /// Step II: Using duplicate entries from server	
        /// "1. Goto WebUI->Networking->TCP/IP Settings->Network Identification
        /// 2. Enter few DNS Suffixes values 
        /// 3. Now add two more DNS Suffixes with same name (for ex: ctc.com) 
        /// 4. Apply the settings and power cycle the printer"	
        /// Expected: Duplicate names should not be allowed.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyInvalidDnsSuffix(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string invalidDnsSuffix = "abc@!#.com";

                if (EwsWrapper.Instance().SetDNSSuffixList(invalidDnsSuffix))
                {
                    TraceFactory.Logger.Info("The DNS suffix:{0} with invalid characters is added to the list in web UI, it is not expected.".FormatWith(invalidDnsSuffix));
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("The DNS suffix:{0} with invalid characters is not added to the list in web UI as expected.".FormatWith(invalidDnsSuffix));
                }

                string validDnsSuffix = CtcUtility.GetUniqueHostName();

                if (!EwsWrapper.Instance().SetDNSSuffixList(validDnsSuffix))
                {
                    return false;
                }

                // again add it should found only once in the list.
                EwsWrapper.Instance().SetDNSSuffixList(validDnsSuffix);

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                printer.PowerCycle();

                int count = EwsWrapper.Instance().GetDNSSuffixLists().Count(x => x.EqualsIgnoreCase(validDnsSuffix));

                if (count > 1)
                {
                    TraceFactory.Logger.Info("Duplicate DNS suffix: {0} entries are found.".FormatWith(validDnsSuffix));
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Duplicate DNS suffix: {0} are not found as expected.".FormatWith(validDnsSuffix));
                    return true;
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                client.Channel.Stop(guid);
            }
        }

        /// <summary>
        /// 231118	
        /// Ensure that Device is not sending any unexpected DNS queries Wired	
        /// Verify that device is not sending any unexpected DNS queries	
        /// "1.configure DNS Server with active directory on Windows 2003 or 2008 server
        /// 2.Open device WebUI and Configure DNS parameters such as Domain name and Primary DNS server either manually or dynamically 
        /// 3.Apply the changes
        /// 4.Use a sniffer to capture the dns packets"
        /// Expected: "Unexpected DNS A and AAAA queries should not be sent in the network"
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyUnexpectedDnsQueries(NetworkNamingServiceActivityData activityData, bool isIpv4, int testNo)
        {
            string hostName = string.Empty;
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                PacketDetails packetDetails = null;

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

                    if (!EwsWrapper.Instance().SetDomainName(GetDomainName(activityData.PrimaryDhcpServerIPAddress)))
                    {
                        return false;
                    }

                    EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);

                    packetDetails = client.Channel.Stop(guid);
                }

                hostName = EwsWrapper.Instance().GetHostname();

                // Note: Negative validation
                return !ValidateDnsPackets(packetDetails.PacketsLocation, activityData.PrimaryDhcpServerIPAddress, hostName, activityData.WiredIPv4Address, isIpv4);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
            }
        }


        /// <summary>
        /// 702616	
        /// TEMPLATE Verify DNS name resolution using NTS			
        /// DNS name resolution using NTS	
        /// "1. Configure a basic IP address , Subnet Mask, and Gateway on the printer under test.
        /// 2. Configure Domain name and DNS server IP address on the printer.
        /// 3. Select - General tab
        /// 4. Select - Date and Time and set Network Time Server Address to the host name of NTS server and synchronise. 
        /// 5. Make a registration for the NTS server under the domain name which is configured on the printer."	
        /// Expected:
        /// Printer should send DNS query and the NTS host name should be resolved to the IP address
        /// </summary>
        /// <returns></returns>
        public static bool VerifyDnsResolutionUsingNts(NetworkNamingServiceActivityData activityData, bool isIpv4, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            return VerifyNtsNameResolution(activityData, activityData.SecondDhcpServerIPAddress, activityData.WiredIPv4Address, testNo, isIpv4, 1);
        }

        /// <summary>
        /// 702638	
        /// TEMPLATE DNS Name Resolution using Primary DNS server			
        /// Verify Printer registers with Primary DNS server	
        /// "1.Power on device.
        /// 2.Configure the device with the primary DNS server.
        /// 3.Power cycle the device.
        /// 4.Wait until the printer is ready.
        /// 5.Now access the WebUI using FQDN (https://FQDNNAME)
        /// 6.Use a sniffer to capture the DNS trace
        /// 7.Verify name, type of request, and IP address."	
        /// It should resolve using primary address and WebUI should be able to access using Hostname/FQDN Name
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyDnsNameResolutionPrimaryDnsServer(NetworkNamingServiceActivityData activityData, bool isPrimary, bool isIpv4, int testNo)
        {
            if (activityData.ProductFamily == "TPS")
            {
                return VerifyFqdnAccess(activityData, isPrimary, isIpv4, testNo);
            }
            else
            {
                if (null == activityData)
                {
                    TraceFactory.Logger.Error("Parameter activityData can not be null.");
                    return false;
                }

                if (!TestPreRequisites(activityData))
                {
                    return false;
                }

                if (!DnsPrerequisites(activityData))
                {
                    return false;
                }
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                return VerifyNtsNameResolution(activityData, activityData.PrimaryDhcpServerIPAddress, activityData.WiredIPv4Address, testNo, isIpv4);
            }
        }

        /// <summary>
        /// 702639	
        /// TEMPLATE DNS Name Resolution using Secondary DNS server			
        /// Verify Printer registers with Secondary DNS server	
        /// "1.Power on device.
        /// 2.Configure the primary DNS server to a server that does not have the DNS services running.
        /// 3.Configure the secondary DNS server to a valid DNS server.
        /// 4.Power cycle the device.
        /// 5.Wait until the printer is ready.
        /// 6.Now access the WebUI using FQDN (https://FQDNNAME)
        /// 7.Use a sniffer to capture the DNS trace
        /// 8. Verify name, type of request, and IP address."	
        /// Expected:
        /// It should resolve using Secondary address and WebUI should be able to access using Hostname/FQDN Name
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyDnsNameResolutionSecondaryDnsServer(NetworkNamingServiceActivityData activityData, bool isPrimary, bool isIpv4, int testNo)
        {
            if (activityData.ProductFamily == "TPS")
            {
                return VerifyFqdnAccess(activityData, isPrimary, isIpv4, testNo);
            }
            else
            {
                if (null == activityData)
                {
                    TraceFactory.Logger.Error("Parameter activityData can not be null.");
                    return false;
                }

                if (!TestPreRequisites(activityData))
                {
                    return false;
                }

                if (!DnsPrerequisites(activityData))
                {
                    return false;
                }
                return VerifyNtsNameResolution(activityData, activityData.SecondDhcpServerIPAddress, activityData.WiredIPv4Address, testNo, isIpv4);
            }
        }

        /// <summary>
        /// Verifies FQDN access based on the parameters
        /// </summary>
        /// <param name="activityData">Activity Data</param>
        /// <param name="isPrimary">Tells Primary or Secondary server</param>
        /// <param name="isIPv4">Tells IPv4 or IPv6 option</param>
        /// <param name="testNo">Test case number</param>
        /// <returns>Returns true if the access and pakcet validation is successfull else false</returns>
        public static bool VerifyFqdnAccess(NetworkNamingServiceActivityData activityData, bool isPrimary, bool isIPv4, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = null;
            string guid = string.Empty;

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Get the Server IP address
                string serverIP = isPrimary ? activityData.PrimaryDhcpServerIPAddress : activityData.SecondDhcpServerIPAddress;
                string clientIpv4Address = GetClientIpAddress(activityData.PrimaryDhcpServerIPAddress, AddressFamily.InterNetwork).ToString();
                string clientIpv6Address = GetClientIpAddress(activityData.PrimaryDhcpServerIPAddress, AddressFamily.InterNetworkV6).ToString();

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

                // Set the DNS Server address on the printer
                if (isPrimary)
                {
                    EwsWrapper.Instance().SetPrimaryDnsServer(serverIP);
                }
                else
                {
                    EwsWrapper.Instance().SetSecondaryDNSServerIP(serverIP);
                }

                string hostName = CtcUtility.GetUniqueHostName();
                string domainName = GetDomainName(serverIP);

                EwsWrapper.Instance().SetHostname(hostName);
                EwsWrapper.Instance().SetDomainName(domainName);

                if (isIPv4)
                {
                    AddDnsRecord(activityData, serverIP, domainName, hostName, activityData.WiredIPv4Address);
                }
                else
                {
                    AddDnsRecord(activityData, serverIP, domainName, hostName, printer.IPv6LinkLocalAddress.ToString());
                }

				using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
				{
					string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);
					dhcpClient.Channel.SetDnsServer(activityData.PrimaryDhcpServerIPAddress, scope, activityData.PrimaryDhcpServerIPAddress);
				}
				Thread.Sleep(TimeSpan.FromMinutes(1));

                client = PacketCaptureServiceClient.Create(serverIP);
                guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

                printer.PowerCycle();				

				// check the EWS accessbility
				string fqdn = CtcUtility.GetFqdn();                
				bool result = printer.IsEwsAccessible(fqdn, "https");                

                PacketDetails packetDetails = client.Channel.Stop(guid);

                guid = string.Empty;

                // Validate the query and response packets on the fqdn resolution.
                if (isIPv4)
                {
                    if (!ValidateQuery(packetDetails.PacketsLocation, serverIP, DnsRecordType.A, fqdn))
                    {
                        return false;
                    }

                    if (!ValidateResponse(packetDetails.PacketsLocation, serverIP, DnsRecordType.A, fqdn, activityData.WiredIPv4Address))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!ValidateQuery(packetDetails.PacketsLocation, serverIP, DnsRecordType.AAAA, fqdn))
                    {
                        return false;
                    }
                    if (!ValidateResponse(packetDetails.PacketsLocation, serverIP, DnsRecordType.AAAA, fqdn, clientIpv6Address))
                    {
                        return false;
                    }
                }
                return result;
            }
            catch
            {
                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                // Stop the packet capture
                if (null != client && !string.IsNullOrEmpty(guid))
                {
                    client.Channel.Stop(guid);
                }
                using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);
                    dhcpClient.Channel.SetDnsServer(activityData.PrimaryDhcpServerIPAddress, scope, activityData.PrimaryDhcpServerIPAddress);
                }

                // set back the printer IP
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);

                DnsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// 702644	
        /// TEMPLATE Verify DNS Suffix Using Highest precedence configured			
        /// Manually entered DNS Suffixes are overwritten by DHCP Option 119 values-ABQT33	
        /// "1. Add a suffix like for example ""boi.hp.com"" in the server then add the suffix ""hp.com"" manually
        /// 2.powercycle the device with manual as heighest precdence."	
        /// Expected:
        /// Both server configured value and "hp.com" should be retained(Modified according to arul's comments)
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyDnsSuffixWithHighPrecedence(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.SecondDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string manualSuffix = CtcUtility.GetUniqueHostName();

                EwsWrapper.Instance().SetDNSSuffixList(manualSuffix);

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));

                DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);
                string[] serverDnsSuffixes = dhcpClient.Channel.GetDnsSuffix(activityData.PrimaryDhcpServerIPAddress, scope).Split('|');

                List<string> printerDnsSuffixes = EwsWrapper.Instance().GetDNSSuffixLists();

                // check the values before power cycle
                TraceFactory.Logger.Info("Verifying the DNS suffix entries before Power Cycle");
                if (VerifyDnsSuffixes(printerDnsSuffixes, serverDnsSuffixes, manualSuffix))
                {
                    printer.PowerCycle();

                    // check the values after power cycle
                    TraceFactory.Logger.Info("Verifying the DNS suffix entries before Power Cycle");
                    if (VerifyDnsSuffixes(printerDnsSuffixes, serverDnsSuffixes, manualSuffix))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                client.Channel.Stop(guid);
                EwsWrapper.Instance().DeleteAllSuffixes();
            }
        }

        /// <summary>
        /// 702642	
        /// TEMPLATE Verify DNS Configuration parameters provided by DHCP server and manual entries			
        /// Verify DNS Configuration parameters using different config methods	
        /// "Step-1:
        /// 1.Set the Hostname,Domain,Primary and Secondary DNS Server options of the DHCP server
        /// 2.Configure device with DHCP.
        /// 3.Change options or remove options on the DHCP server scope where the JD device was initially configured
        /// 4.Reboot  the JD device.
        /// Step-2:
        /// 1.Set the Hostname,Domain,Primary and Secondary DNS Server options manually using EWS
        /// 2.Reboot the card
        /// Step-3:
        /// 1.Set the DNS options using bootp and reboot(Modified according to arul)
        /// Step-4:
        /// 1.Set the Hostname,Domain,Primary and Secondary DNS Server options manually using control panel or Telnet or SNMP
        /// 2.Reboot the card"	
        /// Expected:
        /// "1.The device should be configured with new values. 
        /// If an option was not set on the server, the default value should have been used.
        /// 2. Manually Configured values should be retained after power cycle
        /// 3. DNS options should be updated on the printer(suffix, primary, secondary dns)
        /// 4.Changes made in one place should reflect in other applications also
        /// For Ex: Changes made in EWS should be reflected in SNMP,Control Panel,Telnet ..."		
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyDnsConfigurationParameters(NetworkNamingServiceActivityData activityData, bool isIPv4, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.SecondDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));
            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            string newHostName = string.Empty;
            string newDomainName = string.Empty;
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Step I:");

                SnmpWrapper.Instance().SetConfigPrecedence("2:0:3:1:4");
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));

                if (!ValidateDnsParameters(activityData, IPConfigMethod.DHCP, isIPv4))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II:");

                if (isIPv4)
                {
                    newHostName = CtcUtility.GetUniqueHostName();
                    newDomainName = CtcUtility.GetUniqueHostName();

                    if (!EwsWrapper.Instance().SetHostname(newHostName))
                    {
                        return false;
                    }

                    if (!EwsWrapper.Instance().SetDomainName(newDomainName))
                    {
                        return false;
                    }

                    EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);
                    EwsWrapper.Instance().SetSecondaryDNSServerIP(activityData.SecondDhcpServerIPAddress);

                    printer.PowerCycle();

                    if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.WiredIPv4Address), TimeSpan.FromSeconds(10)))
                    {
                        TraceFactory.Logger.Info("Ping is successful. Printer is available.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Ping failed. Printer is not available.");
                        return false;
                    }

                    if (EwsWrapper.Instance().GetHostname().EqualsIgnoreCase(newHostName))
                    {
                        TraceFactory.Logger.Info("Host name on the Web UI is updated with the manually set value: {0}.".FormatWith(newHostName));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Host name on the Web UI is not updated with the manually set value: {0}.".FormatWith(newHostName));
                        return false;
                    }

                    if (EwsWrapper.Instance().GetDomainName().EqualsIgnoreCase(newDomainName))
                    {
                        TraceFactory.Logger.Info("Domain name on the Web UI is updated with the manually set value: {0}.".FormatWith(newDomainName));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Domain name on the Web UI is not updated with the manually set value: {0}.".FormatWith(newDomainName));
                        return false;
                    }

                    if (EwsWrapper.Instance().GetPrimaryDnsServer().EqualsIgnoreCase(activityData.PrimaryDhcpServerIPAddress))
                    {
                        TraceFactory.Logger.Info("Primary Dns Server on Web UI is updated with the manually set value: {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Primary Dns Server on Web UI is not cleared though no value is configured on the server: {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                        return false;
                    }

                    if (EwsWrapper.Instance().GetSecondaryDnsServer().EqualsIgnoreCase(activityData.SecondDhcpServerIPAddress))
                    {
                        TraceFactory.Logger.Info("Secondary Dns Server on Web UI is updated with the manually set value: {0}.".FormatWith(activityData.SecondDhcpServerIPAddress));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Secondary Dns Server on Web UI is not cleared though no value is configured on the server: {0}.".FormatWith(activityData.SecondDhcpServerIPAddress));
                        return false;
                    }
                }
                else
                {
                    EwsWrapper.Instance().SetPrimaryDnsv6Server("2000:200:200:200:200:200:200:1");
                    EwsWrapper.Instance().SetSecondaryDnsv6Server("2000:200:200:200:200:200:200:2");

                    printer.PowerCycle();

                    if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.WiredIPv4Address), TimeSpan.FromSeconds(10)))
                    {
                        TraceFactory.Logger.Info("Ping is successful. Printer is available.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Ping failed. Printer is not available.");
                        return false;
                    }

                    if (EwsWrapper.Instance().GetPrimaryDnsv6Server().EqualsIgnoreCase("2000:200:200:200:200:200:200:1"))
                    {
                        TraceFactory.Logger.Info("Primary Dns Server on Web UI is updated with the manually set value: {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Primary Dns Server on Web UI is not cleared though no value is configured on the server: {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                        return false;
                    }

                    if (EwsWrapper.Instance().GetSecondaryDnsv6Server().EqualsIgnoreCase("2000:200:200:200:200:200:200:2"))
                    {
                        TraceFactory.Logger.Info("Secondary Dns Server on Web UI is updated with the manually set value: {0}.".FormatWith(activityData.SecondDhcpServerIPAddress));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Secondary Dns Server on Web UI is not cleared though no value is configured on the server: {0}.".FormatWith(activityData.SecondDhcpServerIPAddress));
                        return false;
                    }
                }

                TraceFactory.Logger.Info("Step III:");

                if (!ValidateDnsParameters(activityData, IPConfigMethod.BOOTP, isIPv4))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step IV: Set DNS parameters manually from telnet."));

                if (isIPv4)
                {
                    newHostName = CtcUtility.GetUniqueHostName();
                    newDomainName = CtcUtility.GetUniqueHostName();

                    TelnetWrapper.Instance().SetHostname(newHostName);
                    TelnetWrapper.Instance().SetDomainname(newDomainName);
                    TelnetWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);

                    printer.PowerCycle();

                    if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.WiredIPv4Address), TimeSpan.FromSeconds(10)))
                    {
                        TraceFactory.Logger.Info("Ping is successful. Printer is available.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Ping failed. Printer is not available.");
                        return false;
                    }

                    if (EwsWrapper.Instance().GetHostname().EqualsIgnoreCase(newHostName))
                    {
                        TraceFactory.Logger.Info("Host name on the Web UI is updated with the manually set value: {0}.".FormatWith(newHostName));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Host name on the Web UI is not updated with the manually set value: {0}.".FormatWith(newHostName));
                        return false;
                    }

                    if (EwsWrapper.Instance().GetDomainName().EqualsIgnoreCase(newDomainName))
                    {
                        TraceFactory.Logger.Info("Domain name on the Web UI is updated with the manually set value: {0}.".FormatWith(newDomainName));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Domain name on the Web UI is not updated with the manually set value: {0}.".FormatWith(newDomainName));
                        return false;
                    }

                    if (EwsWrapper.Instance().GetPrimaryDnsServer().EqualsIgnoreCase(activityData.PrimaryDhcpServerIPAddress))
                    {
                        TraceFactory.Logger.Info("Primary Dns Server on Web UI is updated with the manually set value: {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Primary Dns Server on Web UI is not updated with the manually set value: {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                        return false;
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("For Ipv6 we do not have options to set parameters through telnet");
                    return true;
                }

            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                client.Channel.Stop(guid);
                EwsWrapper.Instance().ResetConfigPrecedence();

                using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);

                    dhcpClient.Channel.SetHostName(activityData.PrimaryDhcpServerIPAddress, scope, CtcUtility.GetUniqueHostName());
                    dhcpClient.Channel.SetDomainName(activityData.PrimaryDhcpServerIPAddress, scope, CtcUtility.GetUniqueDomainName());
                    dhcpClient.Channel.SetDnsServer(activityData.PrimaryDhcpServerIPAddress, scope, activityData.PrimaryDhcpServerIPAddress, activityData.SecondDhcpServerIPAddress);
                }
            }
        }

        /// <summary>
        /// 702649	TEMPLATE Verify Using DNS suffix configuration [Option 119] from DHCP Server			
        /// Step I:
        /// 1.Verfiy with Max supported 32 suffix	
        /// "Procedure 1: 
        /// • Configure Option 119 with suffixes in Windows DHCPv4 server. 
        /// • Cold-reset the Jet-Direct.
        /// Procedure 2: 
        /// • Configure Option 119 with five suffixes in Windows DHCPv4 server. 
        /// • Set the IP-Config to DHCP.
        /// For VEP:
        /// Configure manully via WEBUI DNS suffix in Network Identification tab. Check  DNS suffix accept upto 31 DNS suffix address"	
        /// Expected: "The Device should successfully register with DHCP server with IP, DNS Suffixes and other parameters configured in DHCP server.
        /// For VEP:
        /// Device should accept upto 31 DNS Suffix."
        /// Step II:
        /// 2.With Other DHCP options configured in server	
        /// "Procedure 1: 
        /// • Configure Option 119 with suffixes in Windows DHCPv4 server.
        /// • Cold-reset the Jet-Direct.
        /// Procedure 2: 
        /// • Configure Option 119 with five suffixes in Windows DHCPv4 server.
        /// • Set the IP-Config to DHCP."	
        /// Expected: The Device should successfully register with DHCP server with IP, DNS Suffixes and other parameters configured in DHCP server.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyDnsSuffix(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!DnsPrerequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Verify with Max supported 32 suffix"));

                // To update DNS Suffix from server to printer, ipconfig method should be in DHCP
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));

                EwsWrapper.Instance().DeleteAllSuffixes();

                for (int i = 0; i <= 31; i++)
                {
                    EwsWrapper.Instance().SetDNSSuffixList("a{0}.com".FormatWith(Guid.NewGuid().ToString().Substring(0, 3)));
                }

                List<string> suffices = EwsWrapper.Instance().GetDNSSuffixLists();

                if (suffices.Count == 31)
                {
                    TraceFactory.Logger.Info("Device accepted upto 31 DNS suffices.");
                }
                else
                {
                    TraceFactory.Logger.Info("Device accepted more than 31 DNS suffices");
                    return false;
                }

                EwsWrapper.Instance().DeleteAllSuffixes();

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step IIa: Verify Other DHCP options configured in server"));

                string hostName = string.Empty;
                string domainName = string.Empty;

                string dnsSuffix = CtcUtility.GetUniqueHostName();

                using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);

                    if (dhcpClient.Channel.SetDnsSuffix(activityData.PrimaryDhcpServerIPAddress, scope, dnsSuffix))
                    {
                        TraceFactory.Logger.Info("Successfully set DNS suffix to: {0} for scope: {1}.".FormatWith(dnsSuffix, scope));
                    }
                    else
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to set DNS suffix to: {0} for scope: {1}.".FormatWith(dnsSuffix, scope));
                        return false;
                    }

					hostName = dhcpClient.Channel.GetHostName(activityData.PrimaryDhcpServerIPAddress, scope);
					domainName = dhcpClient.Channel.GetDomainName(activityData.PrimaryDhcpServerIPAddress, scope);
                    dnsSuffix = dhcpClient.Channel.GetDnsSuffix(activityData.PrimaryDhcpServerIPAddress, scope);
				}

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                printer.ColdReset();

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));

                if (EwsWrapper.Instance().GetHostname().EqualsIgnoreCase(hostName))
                {
                    TraceFactory.Logger.Info("Host name:{0} from the server is updated on the Web UI.".FormatWith(hostName));
                }
                else
                {
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Host name:{0} from the server is not updated on the Web UI.".FormatWith(hostName));
                    return false;
                }

                if (EwsWrapper.Instance().GetDomainName().EqualsIgnoreCase(domainName))
                {
                    TraceFactory.Logger.Info("Domain name: {0} from the server is updated on the Web UI.".FormatWith(domainName));
                }
                else
                {
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Domain name: {0} from the server is not updated on the Web UI.".FormatWith(domainName));
                    return false;
                }

                if (EwsWrapper.Instance().GetDNSSuffixLists().Contains(dnsSuffix))
                {
                    TraceFactory.Logger.Info("DNS suffix: {0} from the server is updated on the Web UI.".FormatWith(dnsSuffix));
                }
                else
                {
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "DNS suffix: {0} from the server is not updated on the Web UI.".FormatWith(dnsSuffix));
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step IIb: Configure Option 119 with five suffixes in Windows DHCPv4 server"));

                string serverDnsSuffices = string.Empty;

                using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);

                    for (int i = 0; i < 5; i++)
                    {
                        if (dhcpClient.Channel.SetDnsSuffix(activityData.PrimaryDhcpServerIPAddress, scope, dnsSuffix))
                        {
                            TraceFactory.Logger.Info("Successfully set DNS suffix to: {0} for scope: {1}.".FormatWith(dnsSuffix, scope));
                        }
                        else
                        {
                            TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to set DNS suffix to: {0} for scope: {1}.".FormatWith(dnsSuffix, scope));
                            return false;
                        }
                    }

                    serverDnsSuffices = dhcpClient.Channel.GetDnsSuffix(activityData.PrimaryDhcpServerIPAddress, scope);
                }

                EwsWrapper.Instance().ReinitializeConfigPrecedence();

                List<string> printerDnsSuffices = EwsWrapper.Instance().GetDNSSuffixLists();
                foreach (string serverDnssuffix in serverDnsSuffices.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (printerDnsSuffices.Contains(serverDnssuffix))
                    {
                        TraceFactory.Logger.Info("DNS suffix: {0} from the server is updated on the Web UI.".FormatWith(dnsSuffix));
                    }
                    else
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "DNS suffix: {0} from the server is not updated on the Web UI.".FormatWith(dnsSuffix));
                        return false;
                    }
                }

                return true;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().DeleteAllSuffixes();
            }
        }

        /// <summary>
        /// 702615	
        /// TEMPLATE Verify DNS name resolution using NTS with multiple entries in the DNS server
        /// DNS name resolution using NTS with multiple entries in the DNS server	
        /// "1. Configure a basic IP address , Subnet Mask, and Gateway in the JetDirect device under test.
        /// 2. Configure Domain name and DNS server IP address on the printer.
        /// 3. On the NTS settings page set the NTS server IP to load balancing host name of NTS server. 
        /// ( One host name which maps to multiple IP addresses)
        /// 4. Update the settings and synchronise"	
        /// Expected:
        /// Printer should send DNS query and DNS server should respond with multiple IP addresses.
        /// NTS host name should remain same ( load balanced host name) and it should not change to the referenced NTS server host name.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyNtsNameResolution(NetworkNamingServiceActivityData activityData, bool isIpv4, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            return VerifyNtsNameResolution(activityData, activityData.SecondDhcpServerIPAddress, activityData.WiredIPv4Address, testNo, isIpv4, 3);
        }

        #endregion

        #region DDNS

        /// <summary>
        /// 702624	
        /// TEMPLATE Verify DDNS Functionality using different UI			
        ///	Step I:
        ///	DDNS Enable with Telnet	
        ///	"Place both DUT and DNS Server on same network.Configure DUT with the Hostname, Domain name and DNS Server IP addressEnable DDNS using Telnet(Menu -> select 2 -> select 1 ->change settings press ""Y"" and then change DDNS  value to 1 to enable)
        ///	Use a sniffer to capture the DNS traceVerify the Device registered with A record and PTR record automatically "	
        ///	Expected:
        ///	"Device should send DNS update to the DNS server and registered the A record and PTR record automatically."
        /// Step II:
        /// DDNS Disable using Telnet	
        /// "Place both DUT and DNS Server on same network.Configure DUT with the Hostname, Domain name and DNS Server IP addressDisable the DDNS using Telnet            
        /// (Menu -> select 2 -> select 1 ->change settings press ""Y"" and then change DDNS  value to 0 to disable)
        ///	Use a sniffer to capture the DNS traceVerify the Device registered with A record and PTR record automatically"	
        ///	Expected: Device should not send DNS update to the DNS server and should not registered with A record and PTR record automatically in the DNS Server.  
        ///	Step III:	
        /// DDNS Disable using SNMP	
        /// "Place both DUT and DNS Server on same network.Configure DUT with the Hostname, Domain name and DNS Server IP addressDisable the DDNS using SNMP(private-- enterprises--hpprintserver--nm--interface---npcard---npctl----> npctlDynamicDNSupdate)
        ///	Use a sniffer to capture the DNS traceVerify the Device registered with A record and PTR record automatically "
        ///	Expected:
        ///	Device should not send DNS update to the DNS server and should not registered with A record and PTR record automatically in the DNS Server.  
        ///	Step IV: 
        ///	DDNS Enable with SNMP	
        ///	"Place both DUT and DNS Server on same network.Configure DUT with the Hostname, Domain name and DNS Server IP addressEnable DDNS using SNMP as per below path(private-- enterprises--hpprintserver--nm--interface---npcard---npctl----> npctlDynamicDNSupdate)
        ///	Use a sniffer to capture the DNS traceVerify the Device registered with A record and PTR record automatically "	
        ///	Expected: "Device should send DNS update to the DNS server  and registered  the A record and PTR record automatically."
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyDdnsUsingDifferentUI(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!VerifyDdnsDifferentUI(activityData, PrinterAccessType.Telnet, true, testNo))
                {
                    return false;
                }

                if (VerifyDdnsDifferentUI(activityData, PrinterAccessType.Telnet, false, testNo))
                {
                    return false;
                }

                if (!VerifyDdnsDifferentUI(activityData, PrinterAccessType.SNMP, true, testNo))
                {
                    return false;
                }

                return !VerifyDdnsDifferentUI(activityData, PrinterAccessType.SNMP, false, testNo);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetDDNS(false);
            }
        }

        /// <summary>
        /// 702626	
        /// TEMPLATE Verify DDNS records are update with changes to IP parameters			
        /// Step I: Hostname Change using DDNS	
        /// "Place both DUT and DNS Server on same network.
        /// Configure DUT with the Hostname, Domain name and  Secondary DNS Server IP address
        /// Enable DDNS checkbox using WebUI
        /// Change Hostname using WebUI
        /// Use a sniffer to capture the DNS trace
        /// Verify the Device registered with A record and PTR record automatically"
        /// Expected:
        /// "Device should send DNS update to the DNS server  and registered  with new hostname for  A record and PTR record automatically."
        ///	Step II: IP Address Change using DDNS	
        ///	"Place both DUT and DNS Server on same network.
        ///	Configure DUT with the Hostname, Domain name and  Secondary DNS Server IP address
        ///	Enable DDNS checkbox using WebUI
        ///	Change IP Address using WebUI
        ///	Use a sniffer to capture the DNS trace
        ///	Verify the Device registered with A record and PTR record automatically"	
        ///	Expected: 
        ///	"Device should send DNS update to the DNS server  and registered  with new IP Address for  A record and PTR record automatically."
        ///	Step III: Domain Name Change using DDNS	
        ///	"Place both DUT and DNS Server on same network.
        ///	Configure DUT with the Hostname, Domain name and  Secondary DNS Server IP address
        ///	Enable DDNS checkbox using WebUI
        ///	Change Domain Name using WebUI
        ///	Use a sniffer to capture the DNS trace
        ///	Verify the Device registered with A record and PTR record automatically"
        ///	Expected: 
        ///	"Device should send DNS update to the DNS server  and registered  with new Domain Name for  A record and PTR record automatically."
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyDdnsWithIPParameters(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Hostname Change using DDNS"));
                if (!VerifyDdnsWithHostNameChange(activityData, testNo))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: IP Address Change using DDNS"));
                if (!VerifyDdnsWithIpAddressChange(activityData, testNo))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step III: Domain Name Change using DDNS"));
                return VerifyDdnsWithDomainNameChange(activityData, testNo);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetDDNS(false);
            }
        }

        /// <summary>
        /// 702636	
        /// TEMPLATE Verify device DDNS Functionality with Cold Reset
        /// Cold Reset with DDNS	
        /// "Place both DUT and DNS Server on same network.
        /// Configure DUT with the Hostname, Domain name and  Secondary DNS Server IP address
        /// Enable DDNS checkbox using WebUI and click apply button.
        /// Cold-Reset the Device
        /// Use a sniffer to capture the DNS trace
        /// Verify Networking Tab in WEBUI"	
        /// Expected:
        /// "After cold reset DDNS Checkbox should not be enabled on WEBUI.
        /// Device should not send DNS update to the DNS server and should not registered with A record and PTR record automatically in the DNS Server."
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyDdnsWithColdReset(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string hostName = CtcUtility.GetUniqueHostName();

                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetDomainName(GetDomainName(activityData.PrimaryDhcpServerIPAddress)))
                {
                    return false;
                }

                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);

                if (!EwsWrapper.Instance().SetDDNS(true))
                {
                    return false;
                }

                PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                printer.ColdReset();

                if (!EwsWrapper.Instance().GetDDNS())
                {
                    TraceFactory.Logger.Info("DDNS option is disabled in web UI after cold reset.");
                }
                else
                {
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "DDNS option is not disabled in web UI after cold reset.");
                    return false;
                }

                PacketDetails packetDetails = client.Channel.Stop(guid);

                return !ValidateDnsEntry(activityData.PrimaryDhcpServerIPAddress, activityData.WiredIPv4Address, hostName, packetDetails.PacketsLocation, true);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetDDNS(false);
            }
        }

        /// <summary>
        /// 702637	
        /// TEMPLATE Verify DDNS Functionality with Power cycle.
        /// Power cycle with DDNS	
        /// "Place both DUT and DNS Server on same network.
        /// Configure DUT with the Hostname, Domain name and  Secondary DNS Server IP address
        /// Enable DDNS checkbox using WebUI and click apply button.
        /// Power cycle the Device
        /// Use a sniffer to capture the DNS traceVerify the Device registered with A record and PTR record automatically"	
        /// Expected: "Device should send DNS update to the DNS server and registered with A record and PTR record automatically in the DNS Server."
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyDdnsWithPowerCycle(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            if (!DnsPrerequisites(activityData))
            {
                return false;
            }
            string hostName = string.Empty;

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                hostName = CtcUtility.GetUniqueHostName();

                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetDomainName(GetDomainName(activityData.PrimaryDhcpServerIPAddress)))
                {
                    return false;
                }

                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);

                if (!EwsWrapper.Instance().SetDDNS(true))
                {
                    return false;
                }

                PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

				Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
				printer.PowerCycle();
				Thread.Sleep(TimeSpan.FromMinutes(5));

                PacketDetails packetDetails = client.Channel.Stop(guid);

                return ValidateDnsEntry(activityData.PrimaryDhcpServerIPAddress, activityData.WiredIPv4Address, hostName, packetDetails.PacketsLocation);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetDDNS(false);

                DnsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// 702650	
        /// TEMPLATE Verify when the parameters gets updated with DHCP  renew for DDNS			
        /// DHCP Renew with IP Parameters	
        /// "Configure the Hostname, Domain name and  Primary DNS Server IP address in the DHCP Server Scope options
        /// Set lease time as 2mins in DHCP Server
        /// Bring up the DUT.After DUT comes to ready state, Enable DDNS checkbox using WebUI
        /// Use a sniffer to capture the DNS trace
        /// Verify the Device registered with A record and PTR record automatically
        /// Change the Hostname, Domain name from the DHCP scope options.
        /// Wait for DHCP Renew
        /// Use a sniffer to capture the DNS traceVerify the Device registered with A record and PTR record automatically"	
        /// Expected: 
        /// "Device should send DNS update to the DNS server and registered with A record and PTR record automatically in the DNS Server.
        /// Device should update with new hostname and domain name A record and PTR record in the DNS server."
        /// </summary>
        /// <returns></returns>
        public static bool VerifyDdnsWithDhcpRenew(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            string scopeAddress = string.Empty;
            string hostName = string.Empty;

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                // Retrieve the existing values
                scopeAddress = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);

                hostName = CtcUtility.GetUniqueHostName();

                if (dhcpClient.Channel.SetHostName(activityData.PrimaryDhcpServerIPAddress, scopeAddress, hostName))
                {
                    TraceFactory.Logger.Info("Successfully set hostname for the scope: {0} to {1}.".FormatWith(scopeAddress, hostName));
                }
                else
                {
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to set hostname for the scope: {0} to {1}.".FormatWith(scopeAddress, hostName));
                    return false;
                }

                if (dhcpClient.Channel.SetDomainName(activityData.PrimaryDhcpServerIPAddress, scopeAddress, GetDomainName(activityData.PrimaryDhcpServerIPAddress)))
                {
                    TraceFactory.Logger.Info("Successfully set domain name for the scope: {0} to {1}.".FormatWith(scopeAddress, GetDomainName(activityData.PrimaryDhcpServerIPAddress)));
                }
                else
                {
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to set domain name for the scope: {0} to {1}.".FormatWith(scopeAddress, GetDomainName(activityData.PrimaryDhcpServerIPAddress)));
                    return false;
                }

                if (dhcpClient.Channel.SetDnsServer(activityData.PrimaryDhcpServerIPAddress, scopeAddress, activityData.PrimaryDhcpServerIPAddress))
                {
                    TraceFactory.Logger.Info("Successfully set primary dns server address for the scope: {0} to {1}.".FormatWith(scopeAddress, activityData.PrimaryDhcpServerIPAddress));
                }
                else
                {
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to set primary dns server address for the scope: {0} to {1}.".FormatWith(scopeAddress, activityData.PrimaryDhcpServerIPAddress));
                    return false;
                }

                if (dhcpClient.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, scopeAddress, 120))
                {
                    TraceFactory.Logger.Info("Successfully set lease time for the scope: {0} to {1} seconds.".FormatWith(scopeAddress, activityData.PrimaryDhcpServerIPAddress));
                }
                else
                {
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to set primary lease time for the scope: {0} to {1} seconds.".FormatWith(scopeAddress, activityData.PrimaryDhcpServerIPAddress));
                    return false;
                }

                // Setting DHCP as first priority and powecycling to update the lease time and server parameters on the printer
                SnmpWrapper.Instance().SetConfigPrecedence("2:0:3:1:4");
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                printer.PowerCycle();

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));

                PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

                if (!EwsWrapper.Instance().SetDDNS(true))
                {
                    return false;
                }

                // wait for the lease time, so that server values will get update on the printer
                TraceFactory.Logger.Info("Waiting for the lease renewal.");
                Thread.Sleep(TimeSpan.FromMinutes(2));

                PacketDetails packetDetails = client.Channel.Stop(guid);

                if (!ValidateDnsEntry(activityData.PrimaryDhcpServerIPAddress, activityData.WiredIPv4Address, hostName, packetDetails.PacketsLocation))
                {
                    return false;
                }

                DnsPostrequisites(activityData, true);

                //DDNS option is disabled in DNSPostrequisites so enabling back
                if (!EwsWrapper.Instance().SetDDNS(true))
                {
                    return false;
                }

                client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                guid = client.Channel.TestCapture(activityData.SessionId, "{0}-DHCPRenew".FormatWith(testNo));

                hostName = CtcUtility.GetUniqueHostName();

                if (dhcpClient.Channel.SetHostName(activityData.PrimaryDhcpServerIPAddress, scopeAddress, hostName))
                {
                    TraceFactory.Logger.Info("Successfully set hostname for the scope: {0} to {1}.".FormatWith(scopeAddress, hostName));
                }
                else
                {
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to set hostname for the scope: {0} to {1}.".FormatWith(scopeAddress, hostName));
                    return false;
                }

                TraceFactory.Logger.Info("Waiting for the lease renewal.");
                Thread.Sleep(TimeSpan.FromMinutes(2));

                packetDetails = client.Channel.Stop(guid);

                return ValidateDnsEntry(activityData.PrimaryDhcpServerIPAddress, activityData.WiredIPv4Address, hostName, packetDetails.PacketsLocation);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                EwsWrapper.Instance().ResetConfigPrecedence();
                EwsWrapper.Instance().SetDDNS(false);

                DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                if (dhcpClient.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, scopeAddress, 691200))
                {
                    TraceFactory.Logger.Info("Successfully set lease time for the scope: {0} to {1} seconds.".FormatWith(scopeAddress, activityData.PrimaryDhcpServerIPAddress));
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to set primary lease time for the scope: {0} to {1} seconds.".FormatWith(scopeAddress, activityData.PrimaryDhcpServerIPAddress));
                }

                DnsPostrequisites(activityData, true);
            }
        }

        /// <summary>
        /// 702640	
        /// TEMPLATE DNS Resolution using DDNS Record with different DNS servers	
        /// Step I: 
        /// Printer registers with Secondary DNS server	
        /// "Place both DUT and DNS Server on same network.Configure DUT with the Hostname, Domain name and  Secondary DNS Server IP address
        /// Enable DDNS checkbox using WebUI
        /// Use a sniffer to capture the DNS trace
        /// Verify the Device registered with A record and PTR record automatically
        /// Now access the WebUI using FQDN (https://FQDNNAME)"
        /// Expected:
        /// "Device should send DNS update to the DNS server  and registered  the A record and PTR record automatically with Secondary DNS server 
        /// It should resolve using Secondary address and WebUI should be able to access using Hostname/FQDN Name"
        /// Step II:
        /// Different Primary DNS server	
        /// Place both DUT and DNS Server on same network.
        /// Configure DUT with the Hostname, Domain name and  Primary DNS Server IP address
        /// Enable DDNS checkbox using WebUI
        /// Use a sniffer to capture the DNS trace
        /// Verify the Device registered with A record and PTR record automatically
        /// Change the Primary DNS server  and Domain Name to valid another Primary DNS server 
        /// Use a sniffer to capture the DNS trace
        /// Verify the Device registered with A record and PTR record automatically
        /// Now access the WebUI using FQDN (https://FQDNNAME)  	
        /// Expected:
        /// "Device should send DNS update to the DNS server and registered the A record and PTR record automatically.  
        /// It should resolve using primary address and WebUI should be able to access using Hostname/FQDN Name "
        /// Step III: 
        /// Printer registers with Primary DNS server	
        /// "Place both DUT and DNS Server on same network.
        /// Configure DUT with the Hostname, Domain name and  Primary DNS Server IP address
        /// Enable DDNS checkbox using WebUI
        /// Power cycle the device.
        /// Wait until the printer is ready.Use a sniffer to capture the DNS trace
        /// Verify the Device registered with A record and PTR record automatically"
        /// Expected:
        /// "Device should send DNS update to the DNS server  and registered  the A record and PTR record automatically with Primary DNS server. 
        /// It should resolve using primary address and WebUI should be able to access using Hostname/FQDN Name"
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyDdnsWithDifferentDnsServers(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Printer registers with Secondary DNS server"));

                if (!VerifySecondaryDnsRegistration(activityData, testNo))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Printer registers with Different Primary DNS server"));

                if (!VerifyDnsRegistrationWithDifferentPrimaryDns(activityData, testNo))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step III: Printer registers with Primary DNS server"));

                return VerifyPrimaryDnsRegistration(activityData, testNo);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetDDNS(false);
            }
        }

        /// <summary>
        /// 702625	
        /// TEMPLATE Verify DDNS Functionality with DNS server across Subnet
        /// DDNS with accross Subnets	
        /// "Place both DUT and DNS Server on different network.
        /// Configure DUT with the Hostname, Domain name and DNS Server IP address
        /// Enable DDNS checkbox using WebUI and click apply button
        /// Use a sniffer to capture the DNS trace
        /// Verify the Device registered with A record and PTR record automatically"	
        /// Expected:
        /// "Device should send DNS update to across subnet  DNS server  and registered the A record and PTR record automatically."
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyDdnsAcrossSubnet(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            string clientNetworkName = string.Empty;
            string hostName = string.Empty;

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Keeping client and server in different subnet
                // Get the NIC having IP address in the server IP range so that the NIC can be disabled.
                clientNetworkName = GetClientNetworkName(activityData.SecondDhcpServerIPAddress);

                // Add routing through the primary dhcp server so that the service is accessible.
                CtcUtility.AddSourceIPAddress(activityData.SecondDhcpServerIPAddress, activityData.PrimaryDhcpServerIPAddress);

                // Keeping the printer in a different subnet by disabling the network .
                TraceFactory.Logger.Info("Disabling the network corresponding to DHCP server: {0}".FormatWith(activityData.SecondDhcpServerIPAddress));
                NetworkUtil.DisableNetworkConnection(clientNetworkName);

                hostName = CtcUtility.GetUniqueHostName();

                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetDomainName(GetDomainName(activityData.SecondDhcpServerIPAddress)))
                {
                    return false;
                }

                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.SecondDhcpServerIPAddress);

                PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.SecondDhcpServerIPAddress);
                string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

                if (!EwsWrapper.Instance().SetDDNS(true))
                {
                    return false;
                }

                Thread.Sleep(TimeSpan.FromSeconds(30));

                PacketDetails packetDetails = client.Channel.Stop(guid);

                return ValidateDnsEntry(activityData.SecondDhcpServerIPAddress, activityData.WiredIPv4Address, hostName, packetDetails.PacketsLocation);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                NetworkUtil.EnableNetworkConnection(clientNetworkName);
                EwsWrapper.Instance().SetDDNS(false);

                DnsPostrequisites(activityData);
            }
        }

        #endregion

        #region LLMNR

        /// <summary>
        /// 75401	
        /// Verify parameter values and behavior after Cold reset is performed on the device Wired			
        /// Step I: Verify DNS server address behavior on cold reset
        /// "1.Set the options of the DHCP server
        /// 2.Configure device with DHCP.
        /// 3.Change options or remove options on the DHCP server scope where the JD device was initially configured.
        /// 4.Cold boot the device"	
        /// Expected:
        ///		"1.The device should be configured with new values supplied by server 
        ///		 2.If no server is present in the network,the default value should have been configured to the device.
        ///		 TPS :
        ///		 Printer should get configured according to config precedence.If nothing is provided from Server/Manually, it should take Default values."
        ///	Step II: DHCPv4 Client FQDN Cold-reset behavior.	
        ///	Validate DHCP Client FQDN Cold-reset behavior 	
        ///	Expected:
        ///		"1. By Default, Client FQDN option in EWS should be disabled 
        ///		 2.If vendor specific option is set to true in DHCP Server, then RFC 4702 FQDN Option should be enabled automatically
        ///		 TPS : 
        ///		 1. By Default, Client FQDN option in EWS should get enabled."
        ///	Step III:
        ///	WINS address over cold reset	
        ///	"Procedure1: 
        /// 1.Configure manual WINS address on the device.
        /// 2.Cold reset the device
        /// Procedure 2:1.Configure WINS address from DHCP server. Ensure device has acquired WINS address from DHCP server.
        /// 2.Cold reset the device and remove the WINS server option from DHCP server."	
        /// Expected:
        ///		"1. WINS address should be deleted over cold reset to default value(if any)
        ///		 2. WINS server address should not be retained over cold reset if its not provided from DHCP server."
        ///	Step IV: LLMNR feature cold-reset behavior	
        ///	"1. Disable LLMNR feature
        /// 2. Cold rest the DUT
        /// 3. Verify LLMNR feature status after cold-reset"	
        /// "1. LLMNR feature should get enabled after cold reset
        /// TPS :
        /// Printer should perform Uniqueness verification (A packets over IPv4 and AAAA packets  over IPv6 )after cold reset."
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        public static bool VerifyParameterValuesOnColdReset(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            if (!DnsPrerequisites(activityData))
            {
                return false;
            }
            string newHostName = CtcUtility.GetUniqueHostName();
            string newDomainName = CtcUtility.GetUniqueHostName();

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-HostnameChange".FormatWith(testNo));
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step1: Verify DNS Server Address behaviour on cold reset"));
                if (!ConfigureAndValidateServerParameter(activityData, newHostName, newDomainName))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step2: DHCPv4 Client FQDN Cold-reset behavior"));
                bool status = EwsWrapper.Instance().GetRfc();

                if (activityData.ProductFamily == ProductFamilies.TPS.ToString())
                {
                    if (!status)
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "ClientFQDN Option is not in enabled state after cold reset");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("ClientFQDN Option is in enabled state after cold reset");
                    }
                }
                else
                {
                    if (status)
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "ClientFQDN Option is not in enabled state after cold reset");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("ClientFQDN Option is in enabled state after cold reset");
                    }
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step3: WINS address over cold reset"));
                if (!ValidateWinsAddressOverColdReset(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step4: LLMNR feature cold-reset behavior"));
                PacketDetails packetDetails = client.Channel.Stop(guid);
                if (EwsWrapper.Instance().GetLlmnr())
                {
                    TraceFactory.Logger.Info("LLMNR option is enabled over cold reset");
                }
                else
                {
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "LLMNR option is not enabled over cold reset");
                    return false;
                }

                if (activityData.ProductFamily == "TPS")
                {
                    TraceFactory.Logger.Info("Validating the Uniqueness verification packets");
                    return ValidateLlmnrPackets(activityData, packetDetails.PacketsLocation, newHostName, DnsRecordType.A);
                }

                return true;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                TraceFactory.Logger.Info("Deleting the wins existing entry on the DHCP Server");

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);
                    if (!dhcpClient.Channel.DeleteWinsServer(activityData.PrimaryDhcpServerIPAddress, scope))
                    {
                        TraceFactory.Logger.Info("Failed to delate wins server on DHCP server scope options");
                    }
                    if (activityData.ProductFamily == "LFP")
                    {
                        dhcpClient.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress),
                                                         activityData.WiredIPv4Address, activityData.PrinterMacAddress);
                        if (dhcpClient.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress),
                                                             activityData.WiredIPv4Address, activityData.PrinterMacAddress, ReservationType.Both))
                        {
                            TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for Both : Succeeded");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for Both: Failed");
                        }
                    }

                }

                printer.ColdReset();
            }
        }

        /// <summary>
        /// Template Id: 75439
        /// Devices behavior when Hostname uniqueness verification fails using LLMNR Wired
        /// Verify DUT’s behavior when Hostname uniqueness verification using LLMNR fails
        /// 1. Start capturing Network trace with any network sniffer tool
        /// 2. Make sure LLMNR is enabled on DUT
        /// 3. Set DUT's Hostname to already existing Hostname on network
        /// 4. Verify DUT’s behavior when Hostname uniqueness verification fails
        /// Expected: DUT should stop LLMNR OR picks new Hostname and confirm uniqueness verification, When Hostname uniqueness verification fails	    
        /// TPS: DUT will send A and AAAA packets continuosly instead of sending once,need to validate the same.
        /// <returns></returns>
        /// </summary>
        public static bool VerifyLLMNRHostNameUniqueness(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            EwsWrapper.Instance().SetHostname(CtcUtility.GetUniqueHostName());

            EwsWrapper.Instance().ChangeDeviceAddress(activityData.SecondPrinterIPAddress);
            string hostName = EwsWrapper.Instance().GetHostname();
            EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Making sure LLMNR is enabled on the Printer");
                if (EwsWrapper.Instance().GetLlmnr())
                {
                    TraceFactory.Logger.Info("LLMNR option is in enabled state");
                }
                else
                {
                    if (!EwsWrapper.Instance().SetLlmnr(true))
                    {
                        return false;
                    }
                }

                TraceFactory.Logger.Info("Validating the LLMNR Packets when hostname uniqueness fails");
                string fileName = "{0}-LLMNR{1}".FormatWith(testNo, "HostNameUniqueness");
                hostName = CtcUtility.GetUniqueHostName();
                string packetLocation = ValidateLLMNRUniquenessPackets(activityData, GetClientNetworkId(activityData.WiredIPv4Address), "HostNameChange", fileName, hostName);

                if (!ValidateLLMNRHostNameUniquenessPackets(activityData, packetLocation, hostName, DnsRecordType.A, activityData.WiredIPv4Address))
                {
                    return false;
                }
                if (!ValidateLLMNRHostNameUniquenessPackets(activityData, packetLocation, hostName, DnsRecordType.AAAA, activityData.WiredIPv4Address))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                printer.ColdReset();
            }
        }


        /// <summary>
        /// Template Id: 702632
        /// TEMPLATE Hostname uniqueness verification when LLMNR is enabled
        /// 1. Verify Hostname uniqueness verification using LLMNR at boot up
        /// 1. Start capturing Network trace with any network sniffer tool (Filter using udp.port==5355 )
        /// 2. Make sure that LLMNR is enabled on DUT
        /// 3. Power cycle DUT
        /// 4. Verify if DUT Responder's confirms uniqueness over IPv4 and IPv6 at boot up
        /// Expected: DUT should send LLMNR "A" and "AAAA" queries for its Hostname at boot up to confirm uniqueness
        /// 2.  Verify Hostname uniqueness verification using LLMNR when IP address changes
        /// 1. Start capturing Network trace with any network sniffer tool 
        /// 2. Make sure that LLMNR is enabled on DUT
        /// 3. Change IP address of DUT 
        /// 4. Verify if DUT Responder’s confirms uniqueness after IP address change
        /// Expected:DUT should send LLMNR “A”/“AAAA” queries for its Hostname after IPv4/Ipv6 address change to confirm uniqueness
        /// 3. Verify Hostname uniqueness verification using LLMNR when Hostname changes
        /// 1. Start capturing Network trace with any network sniffer tool 
        /// 2. Make sure that LLMNR is enabled on DUT
        /// 3. Change DUT's Hostname 
        /// 4. Verify if DUT Responder’s confirms uniqueness over IPv4 and IPv6 when Hostname changes
        /// Expected:DUT should send LLMNR “A” and “AAAA” queries for its new Hostname when Hostname changes to confirm uniqueness
        /// 4. Verify Hostname uniqueness verification using LLMNR when LLMNR is enabled
        /// 1. Start capturing Network trace with any network sniffer tool
        /// 2. Disable and Re-Enable LLMNR on DUT
        /// 3. Verify if DUT Responder’s confirms uniqueness over IPv4 and IPv6 at when LLMNR gets Re-Enabled
        /// Expected: DUT should send LLMNR “A” and “AAAA” queries for its Hostname When LLMNR gets Re-Enabled to confirm uniqueness
        /// <returns></returns>
        /// </summary>
        public static bool VerifyLLMNRHostNameIPAddressUniqueness(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            string hostName = EwsWrapper.Instance().GetHostname();
            try
            {
                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step1: Verify Hostname uniqueness verification using LLMNR at boot up"));

                TraceFactory.Logger.Info("Making sure LLMNR is enabled on the Printer");
                if (EwsWrapper.Instance().GetLlmnr())
                {
                    TraceFactory.Logger.Info("LLMNR option is in enabled state");
                }
                else
                {
                    if (!EwsWrapper.Instance().SetLlmnr(true))
                    {
                        return false;
                    }
                }
                TraceFactory.Logger.Info("Validating the LLMNR Packets during bootup");
                string fileName = "{0}-LLMNR{1}".FormatWith(testNo, "LLMNR-Bootup");
                string packetLocation = ValidateLLMNRUniquenessPackets(activityData, GetClientNetworkId(activityData.WiredIPv4Address), "PowerCycle", fileName, hostName);

                //again fetching hostname,for tps sometimes the values is going to default
                hostName = EwsWrapper.Instance().GetHostname();
                if (!ValidateLLMNRHostNameUniquenessPackets(activityData, packetLocation, hostName, DnsRecordType.A, activityData.WiredIPv4Address))
                {
                    return false;
                }
                if (!ValidateLLMNRHostNameUniquenessPackets(activityData, packetLocation, hostName, DnsRecordType.AAAA, activityData.WiredIPv4Address))
                {
                    return false;
                }


                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step2: Verify Hostname uniqueness verification using LLMNR when IP address changes"));

                IPAddress manualIpAddress = NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.WiredIPv4Address));
                fileName = "{0}-LLMNR{1}".FormatWith(testNo, "LLMNR-IPAddressChange");
                packetLocation = ValidateLLMNRUniquenessPackets(activityData, GetClientNetworkId(activityData.WiredIPv4Address), "IPAddressChange", fileName, hostName, manualIpAddress.ToString());

                //again fetching hostname,for tps sometimes the values is going to default
                hostName = EwsWrapper.Instance().GetHostname();
                if (!ValidateLLMNRHostNameUniquenessPackets(activityData, packetLocation, hostName, DnsRecordType.A, manualIpAddress.ToString()))
                {
                    return false;
                }
                if (!ValidateLLMNRHostNameUniquenessPackets(activityData, packetLocation, hostName, DnsRecordType.AAAA, manualIpAddress.ToString()))
                {
                    return false;
                }
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step3: Verify Hostname uniqueness verification using LLMNR when Hostname changes"));

                fileName = "{0}-LLMNR{1}".FormatWith(testNo, "HostNameChange");
                hostName = CtcUtility.GetUniqueHostName();
                packetLocation = ValidateLLMNRUniquenessPackets(activityData, GetClientNetworkId(activityData.WiredIPv4Address), "HostNameChange", fileName, hostName);

                if (!ValidateLLMNRHostNameUniquenessPackets(activityData, packetLocation, hostName, DnsRecordType.A, activityData.WiredIPv4Address))
                {
                    return false;
                }
                if (!ValidateLLMNRHostNameUniquenessPackets(activityData, packetLocation, hostName, DnsRecordType.AAAA, activityData.WiredIPv4Address))
                {
                    return false;
                }


                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step4: Verify Hostname uniqueness verification using LLMNR when LLMNR is enabled"));

                fileName = "{0}-LLMNR{1}".FormatWith(testNo, "ReenableLLMNR");
                hostName = EwsWrapper.Instance().GetHostname();
                packetLocation = ValidateLLMNRUniquenessPackets(activityData, GetClientNetworkId(activityData.WiredIPv4Address), "ReenableLLMNR", fileName, hostName);

                if (!ValidateLLMNRHostNameUniquenessPackets(activityData, packetLocation, hostName, DnsRecordType.A, activityData.WiredIPv4Address))
                {
                    return false;
                }
                if (!ValidateLLMNRHostNameUniquenessPackets(activityData, packetLocation, hostName, DnsRecordType.AAAA, activityData.WiredIPv4Address))
                {
                    return false;
                }
                return true;
            }
            finally
            {
                TraceFactory.Logger.Info("PostRequisites");
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                printer.ColdReset();
            }

        }

        /// <summary>
        /// 75402	
        /// Verify the parameter values and behavior of the device after powercycling ;Wired			
        /// Step I: DHCPv4 Client FQDN Power cycle behavior	
        /// "Jetdirect :
        /// 1. Enable RFC 4702 option in Jetd Manually 
        /// 2. Power cycle the printer
        /// TPS :
        /// 1. Disable RFC 4702 option in TPS product manually from EWS.
        /// 2. Power cycle the device."	"Jetdirect :
        /// 1.By Default, Client FQDN option in EWS should be disabled.
        /// 2.If vendor specific option is set to true in DHCP Server, then RFC 4702 FQDN Option should be enabled automatically
        /// TPS :
        /// 1.By Default, Client FQDN option in EWS is enabled.
        /// 2. When disabled and power cycle, it should remain in the disabled state."
        ///	WINS address over power cycle	
        ///	"Procedure 1 : 1. Configure WINS server IP address manually on the device.
        /// 2. Power cycle the device
        /// Procedure 2: 1. Provide WINS server IP address from DHCP server.
        /// 2. Power off the device.
        /// 3. Delete the WINS IP address from the DHCP server.
        /// 3. Power up the device"	
        /// "WINS server IP address should be retained over power cycle and the device should send WINS regiatration packets and should get registered with WINS server
        /// When the device comes up after power cycle, since the WINS server IP address is not provided from DHCP server it should have default addressTPS : Device should get configured with the value for WINS Address according to config precedence.If nothing is privided it should retain the default address."
        ///	Name resolution using DNS after power cycle	"1.Set the Hostname,Domain,Primary and Secondary DNS Server options of the DHCP server
        /// 2.Configure device with DHCP.
        /// 3.Change options or remove options on the DHCP server scope where the JD device was initially configured.
        /// 4.Reboot the JD device."	
        /// "1.The device should be configured with new values supplied by dhcp server
        /// 2.Changes made in one place should reflect in other applications also
        /// For Ex: Changes made in EWS should be reflected in SNMP,Control Panel and Telnet3. Device should retain these parameters even after power cycle."
        ///	LLMNR feature behavior after power cycle	"Procedure 1 :
        /// 1. Disable LLMNR feature
        /// 2. Power cycle DUT
        /// 3. Verify LLMNR feature status after power cycle.
        /// Procedure 2 :
        /// 1. Enable LLMNR feature
        /// 2. Power cycle DUT
        ///3. Verify Uniquess behavior after power cycle"	
        ///1. LLMNR feature should not get enabled after power cycle
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyParameterValuesOnPowerCycle(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                printer.ColdReset();
            }
        }

        /// <summary>
        /// 702617	TEMPLATE Verify DUT Response to LLMNR A, AAA and PTR queries, when LLMNR is in Enabled and Disabled state			
        /// Step I: LLMNR "A" query resolution when LLMNR is in Enabled State	
        /// "1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Enable LLMNR, IPv4 and IPv6 on DUT
        /// 4. Open Command prompt and execute ""ping -4 [DUT's Hostname]"" to simulate LLMNR ""A"" query
        /// 5. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -4 [DUT's Hostname] command should return DUT's IPv4 address
        ///		Using captured network trace make sure DUT responded to LLMNR ""A"" query both over IPv4 and IPv6 stacks, if client OS posted query on IPv4 and IPv6 multicast address"
        ///	Step II: LLMNR "AAAA" query resolution when LLMNR is in Disabled State	
        ///	"1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Disable LLMNR on DUT
        /// 4. Enable IPv4 and IPv6 on DUT
        /// 5. Open Command prompt and execute ""ping -6 [DUT's Hostname]"" to simulate LLMNR ""AAAA"" query
        /// 6. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -6 [DUT's Hostname] command should not return DUT's IPv6 address
        ///		Using captured network trace make sure DUT discarded LLMNR ""AAAA"" query both over IPv4 and IPv6 stacks"
        ///	Step III: LLMNR "AAAA" query resolution when LLMNR is in Enabled State	
        ///	"1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Enable LLMNR, IPv4 and IPv6 on DUT
        /// 4. Open Command prompt and execute ""ping -6 [DUT's Hostname]"" to simulate LLMNR ""AAAA"" query
        /// 5. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -6 [DUT's Hostname] command should return DUT's IPv6 address
        ///		Using captured network trace make sure DUT responded to LLMNR ""AAAA"" query both over IPv4 and IPv6 stacks, if client OS posted query on IPv4 and IPv6 multicast address"
        ///	Step IV: LLMNR "A" query resolution when LLMNR is in Disabled State	
        ///	"1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Disable LLMNR on DUT
        /// 4. Enable IPv4 and IPv6 on DUT
        /// 5. Open Command prompt and execute ""ping -4 [DUT's Hostname]"" to simulate LLMNR ""A"" query
        /// 6. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -4 [DUT's Hostname] command should return DUT's IPv4 address
        ///		Using captured network trace make sure DUT discarded LLMNR ""A"" query both over IPv4 and IPv6 stacks"
        ///	Step V: LLMNR "PTR" query resolution when LLMNR is in Disabled State	
        ///	"1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Disable LLMNR on DUT
        /// 4 . Enable IPv4 and IPv6 on DUT
        /// 4. Open Command prompt and execute ""ping -a [DUT's IPv4 or IPv6 addresses]"" to simulate LLMNR ""PTR"" query
        /// 5. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -a [DUT's IPv4 or IPv6 addresses] command should return DUT's Host Name 
        ///		Using captured network trace make sure DUT discarded LLMNR ""PTR"" query both over IPv4 and IPv6 stacks"
        ///	Step VI: LLMNR "PTR" query resolution when LLMNR is in Enabled State	
        ///	"1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Enable LLMNR, IPv4 and IPv6 on DUT
        /// 4. Open Command prompt and execute ""ping -a [DUT's IPv4 or IPv6 addresses]"" to simulate LLMNR ""PTR"" query
        /// 5. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -a [DUT's IPv4 or IPv6 addresses] command should return DUT's Host Name 
        ///		Using captured network trace make sure DUT responded to LLMNR ""PTR"" queries both over IPv4 and IPv6 stacks, if client OS posted query on IPv4 and IPv6 multicast address"
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyLlmnrWithLlmnrStateChange(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                //No needed - handled already 
                //EwsWrapper.Instance().SetIPv4(true, printerIpv4Address: activityData.WiredIPv4Address);

                //if (!EwsWrapper.Instance().SetIPv6(true))
                //{
                //    return false;
                //}

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: LLMNR A query resolution when LLMNR is in Enabled State"));

                if (!VerifyQueryWithLLMNR(activityData, testNo, true, DnsRecordType.A))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: LLMNR AAAA query resolution when LLMNR is in Disabled State"));

                if (VerifyQueryWithLLMNR(activityData, testNo, false, DnsRecordType.AAAA))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep(CtcUtility.WriteStep("Step III: LLMNR AAAA query resolution when LLMNR is in Enabled State")));

                if (!VerifyQueryWithLLMNR(activityData, testNo, true, DnsRecordType.AAAA))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step IV: LLMNR A query resolution when LLMNR is in Disabled State"));

                if (VerifyQueryWithLLMNR(activityData, testNo, false, DnsRecordType.A))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step V: LLMNR PTR query resolution when LLMNR is in Disabled State."));

                if (VerifyQueryWithLLMNR(activityData, testNo, false, DnsRecordType.PTR))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step VI: LLMNR PTR query resolution when LLMNR is in Enabled State"));

                return VerifyQueryWithLLMNR(activityData, testNo, true, DnsRecordType.PTR);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                if (!(activityData.ProductFamily == ProductFamilies.InkJet.ToString()))
                {
                    printer.ColdReset();
                }
            }
        }

        /// <summary>
        /// 702618	TEMPLATE Verify DUT response to LLMNR A, AAA and PTR queries, when IPv4 Multicast is in Enabled and Disabled state			
        /// Step I: LLMNR "A" query resolution when IPv4 Multicast is in Enabled State	
        /// "1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Make sure LLMNR, IPv4 Multicast is enabled on DUT
        /// 4. Open Command prompt and execute ""ping -4 [DUT's Hostname]"" to simulate LLMNR ""A"" query
        /// 5. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -4 [DUT's Hostname] command should return DUT's IPv4 address
        ///		Using captured network trace make sure DUT responded to LLMNR ""A"" query both over IPv4 and IPv6 stacks, if client OS posted query on IPv4 and IPv6 multicast address"
        ///	Step II: LLMNR "AAAA" query resolution when IPv4 Multicast is in Disabled State	
        ///	"1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Make sure LLMNR, IPv4 and IPv6 is enabled on DUT
        /// 4. Disable Multicast IPv4 on DUT
        /// 5. Open Command prompt and execute ""ping -6 [DUT's Hostname]"" to simulate LLMNR ""AAAA"" query
        /// 6. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -6 [DUT's Hostname] command should not return DUT's IPv6 address
        ///		Using captured network trace make sure DUT responded to LLMNR ""AAAA"" query only over IPv6 stacks, if client OS posted query on IPv4 and IPv6 multicast address"
        ///	Step III: LLMNR "AAAA" query resolution when IPv4 Multicast is in Enabled State	
        ///	"1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Make sure LLMNR, IPv4, IPv6 and Multicast IPv4 is enabled on DUT
        /// 4. Open Command prompt and execute ""ping -6 [DUT's Hostname]"" to simulate LLMNR ""AAAA"" query
        /// 5. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -6 [DUT's Hostname] command should return DUT's IPv6 address
        ///		Using captured network trace make sure DUT responded to LLMNR ""AAAA"" query both over IPv4 and IPv6 stacks, if client OS posted query on IPv4 and IPv6 multicast address"
        ///	Step IV: LLMNR "A" query resolution when IPv4 Multicast is in Disable State	
        ///	"1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Make sure LLMNR, IPv4 and IPv6 is enabled on DUT
        /// 4. Disable Multicast IPv4 on DUT
        /// 5. Open Command prompt and execute ""ping -4 [DUT's Hostname]"" to simulate LLMNR ""A"" query
        /// 6. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -4 [DUT's Hostname] command should return DUT's IPv4 address
        ///		Using captured network trace make sure DUT responded to LLMNR ""A"" query only over IPv6 stack, if client OS posted query on IPv4 and IPv6 multicast address"
        /// Step V: LLMNR "PTR" query resolution when IPv4 Multicast is in Disabled State	
        /// "1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Make sure LLMNR, IPv4 and IPv6  is enabled on DUT
        /// 4. Disable Multicast IPv4 stack on DUT
        /// 4. Open Command prompt and execute ""ping -a [DUT's IPv4 or IPv6 address]"" to simulate LLMNR ""PTR"" query
        /// 5. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -a [DUT's IPv6 address] command should return DUT's Host Name 
        ///		Using captured network trace make sure DUT responded to LLMNR ""PTR"" queries only over IPv6 stacks, if client OS posted query on IPv4 and IPv6 multicast address"
        ///	Step VI: LLMNR "PTR" query resolution when IPv4 Multicast is in Enabled State	
        ///	"1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Enable LLMNR, IPv4,IPv6 and Multicast IPv4 on DUT
        /// 4. Open Command prompt and execute ""ping -a [DUT's IPv4 or IPv6 address]"" to simulate LLMNR ""PTR"" query
        /// 5. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -a [DUT's IPv4 or IPv6 address] command should return DUT's Host Name 
        ///		Using captured network trace make sure DUT responded to LLMNR ""PTR"" queries both over IPv4 and IPv6 stacks, if client OS posted query on IPv4 and IPv6 multicast address"
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyLlmnrWithMulticastIpv4StateChange(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetLlmnr(true))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: LLMNR A query resolution when IPv4 Multicast is in Enabled State"));

                if (!VerifyQueryWithMulticastIpv4(activityData, testNo, true, DnsRecordType.A))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: LLMNR AAAA query resolution when IPv4 Multicast is in Disabled State"));

                if (!VerifyQueryWithMulticastIpv4(activityData, testNo, false, DnsRecordType.AAAA))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step III: LLMNR AAAA query resolution when IPv4 Multicast is in Enabled State"));

                if (!VerifyQueryWithMulticastIpv4(activityData, testNo, true, DnsRecordType.AAAA))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step IV: LLMNR A query resolution when IPv4 Multicast is in Disable State"));

                if (!VerifyQueryWithMulticastIpv4(activityData, testNo, false, DnsRecordType.A))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step V: LLMNR PTR query resolution when IPv4 Multicast is in Disabled State"));

                if (!VerifyQueryWithMulticastIpv4(activityData, testNo, false, DnsRecordType.PTR))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step VI: LLMNR PTR query resolution when IPv4 Multicast is in Enabled State"));

                return VerifyQueryWithMulticastIpv4(activityData, testNo, true, DnsRecordType.PTR);
            }
            finally
            {
                if (!(activityData.ProductFamily == ProductFamilies.InkJet.ToString()))
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                    printer.ColdReset();
                }
            }
        }

        /// <summary>
        /// 702619	TEMPLATE Verify DUT response to LLMNR A, AAA and PTR queries, when IP Stack is in Enabled and Disabled state			
        /// Step I: LLMNR "A" query resolution when IPv6 stack is in Enabled State	
        /// "1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Make sure LLMNR, IPv6 is enabled on DUT
        /// 4. Open Command prompt and execute ""ping -4 [DUT's Hostname]"" to simulate LLMNR ""A"" query
        /// 5. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -4 [DUT's Hostname] command should return DUT's IPv4 address
        ///		Using captured network trace make sure DUT responded to LLMNR ""A"" query both over IPv4 and IPv6 stacks, if client OS posted query on IPv4 and IPv6 multicast address"
        ///	Step II: LLMNR "PTR" query resolution when IPv6 stack is in Disabled State	
        /// "1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Make sure LLMNR, IPv4 is enabled on DUT
        /// 4. Disable IPv6 stack on DUT
        /// 4. Open Command prompt and execute ""ping -a [DUT's IPv6 address]"" to simulate LLMNR ""PTR"" query
        /// 5. Verify the DUT Responder's behavior"
        ///	Expected:
        ///		"ping -a [DUT's IPv6 address] command should not DUT's Host Name 
        ///		Using captured network trace make sure DUT discarded LLMNR ""PTR"" query both over IPv4 and IPv6 stacks"
        /// Step III: LLMNR "PTR" query resolution when IPv6 stack is in Enabled State	
        /// "1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Enable LLMNR, IPv4 and IPv6 on DUT
        /// 4. Open Command prompt and execute ""ping -a [DUT's IPv6 address]"" to simulate LLMNR ""PTR"" query
        /// 5. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -a [DUT's IPv4 address] command should return DUT's Host Name 
        ///		Using captured network trace make sure DUT responded to LLMNR ""PTR"" queries both over IPv4 and IPv6 stacks, if client OS posted query on IPv4 and IPv6 multicast address"
        /// Step IV: LLMNR "AAAA" query resolution when IPv6 stack is in Disabled State	
        /// "1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Make sure LLMNR and IPv4 is enabled on DUT
        /// 4. Disable IPv6 on DUT
        /// 5. Open Command prompt and execute ""ping -6 [DUT's Hostname]"" to simulate LLMNR ""AAAA"" query
        /// 6. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -6 [DUT's Hostname] command should not return DUT's IPv6 address
        ///		Using captured network trace make sure DUT responded to LLMNR ""AAAA"" with RCODE=0 and empty answer section over IPv4 stack, if client OS posted query on IPv4 and IPv6 multicast address"
        /// Step V: LLMNR "AAAA" query resolution when IPv6 stack is in Enabled State	
        /// "1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Make sure LLMNR, IPv4, IPv6 is enabled on DUT
        /// 4. Open Command prompt and execute ""ping -6 [DUT's Hostname]"" to simulate LLMNR ""AAAA"" query
        /// 5. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -6 [DUT's Hostname] command should return DUT's IPv6 address
        ///		Using captured network trace make sure DUT responded to LLMNR ""AAAA"" query both over IPv4 and IPv6 stacks, if client OS posted query on IPv4 and IPv6 multicast address"
        /// Step VI: LLMNR "A" query resolution when IPv6 stack is in Disable State	
        /// "1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Make sure LLMNR and IPv4 is enabled on DUT
        /// 4. Disable IPv6 on DUT
        /// 5. Open Command prompt and execute ""ping -4 [DUT's Hostname]"" to simulate LLMNR ""A"" query
        /// 6. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -4 [DUT's Hostname] command should return DUT's IPv4 address
        ///		Using captured network trace make sure DUT responded to LLMNR ""A"" query only over IPv4 stack, if client OS posted query on IPv4 and IPv6 multicast address"
        /// Step VII: LLMNR "A" query resolution when IPv4 stack is in Enabled State	
        /// "1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Make sure LLMNR, IPv4 is enabled on DUT
        /// 4. Open Command prompt and execute ""ping -4 [DUT's Hostname]"" to simulate LLMNR ""A"" query
        /// 5. Verify the DUT Responder's behavior"	
        /// Expected: 
        ///		"ping -4 [DUT's Hostname] command should return DUT's IPv4 address
        ///		Using captured network trace make sure DUT responded to LLMNR ""A"" query both over IPv4 and IPv6 stacks, if client OS posted query on IPv4 and IPv6 multicast address"
        /// Step VIII: LLMNR "PTR" query resolution when IPv4 stack is in Disabled State	
        /// "1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Make sure LLMNR, IPv6 is enabled on DUT
        /// 4. Disable IPv4 stack on DUT
        /// 4. Open Command prompt and execute ""ping -a [DUT's IPv4 address]"" to simulate LLMNR ""PTR"" query
        /// 5. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -a [DUT's IPv4 address] command should return error
        ///		Using captured network trace make sure DUT discarded LLMNR ""PTR"" query both over IPv4 and IPv6 stacks"
        /// Step IX:	LLMNR "PTR" query resolution when IPv4 stack is in Enabled State	
        ///"1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Enable LLMNR, IPv4 and IPv6 on DUT
        /// 4. Open Command prompt and execute ""ping -a [DUT's IPv4 address]"" to simulate LLMNR ""PTR"" query
        /// 5. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -a [DUT's IPv4 address] command should return DUT's Host Name 
        /// Using captured network trace make sure DUT responded to LLMNR ""PTR"" queries both over IPv4 and IPv6 stacks, if client OS posted query on IPv4 and IPv6 multicast address"
        /// Step X:	LLMNR "AAAA" query resolution when IPv4 stack is in Disabled State	
        /// "1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Make sure LLMNR and IPv6 is enabled on DUT
        /// 4. Disable IPv4 on DUT
        /// 5. Open Command prompt and execute ""ping -6 [DUT's Hostname]"" to simulate LLMNR ""AAAA"" query
        /// 6. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -6 [DUT's Hostname] command should return DUT's IPv6 address
        ///		Using captured network trace make sure DUT responded to LLMNR ""AAAA"" only over IPv6 stack, if client OS posted query on IPv4 and IPv6 multicast address"
        /// Step XI:	LLMNR "AAAA" query resolution when IPv4 stack is in Enabled State	
        ///"1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Make sure LLMNR, IPv4, IPv6 is enabled on DUT
        /// 4. Open Command prompt and execute ""ping -6 [DUT's Hostname]"" to simulate LLMNR ""AAAA"" query
        /// 5. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -6 [DUT's Hostname] command should return DUT's IPv6 address
        ///		Using captured network trace make sure DUT responded to LLMNR ""AAAA"" query both over IPv4 and IPv6 stacks, if client OS posted query on IPv4 and IPv6 multicast address"
        /// Step XII: LLMNR "A" query resolution when IPv4 stack is in Disable State	
        /// "1. Place both DUT and LLMNR supported OS on same network.
        /// 2. Start capturing Network trace with any network sniffer tool
        /// 3. Make sure LLMNR and IPv6 is enabled on DUT
        /// 4. Disable IPv4 on DUT
        /// 5. Open Command prompt and execute ""ping -4 [DUT's Hostname]"" to simulate LLMNR ""A"" query
        /// 6. Verify the DUT Responder's behavior"	
        /// Expected:
        ///		"ping -4 [DUT's Hostname] command should return error
        ///		Using captured network trace make sure DUT responded to ""A"" query with RCODE=0 and empty answer section over IPv6"
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyLlmnrWithIpStackStateChange(NetworkNamingServiceActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: LLMNR A query resolution when IPv6 stack is in Enabled State"));

                if (!VerifyQueryWithIpv6(activityData, testNo, true, DnsRecordType.A))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: LLMNR PTR query resolution when IPv6 stack is in Disabled State"));

                if (VerifyQueryWithIpv6(activityData, testNo, false, DnsRecordType.PTR))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step III: LLMNR PTR query resolution when IPv6 stack is in Enabled State"));

                if (!VerifyQueryWithIpv6(activityData, testNo, true, DnsRecordType.PTR))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step IV: LLMNR AAAA query resolution when IPv6 stack is in Disabled State"));

                if (VerifyQueryWithIpv6(activityData, testNo, false, DnsRecordType.AAAA))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step V: LLMNR AAAA query resolution when IPv6 stack is in Enabled State"));

                if (!VerifyQueryWithIpv6(activityData, testNo, true, DnsRecordType.AAAA))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step VI: LLMNR A query resolution when IPv6 stack is in Disable State"));

                if (VerifyQueryWithIpv6(activityData, testNo, false, DnsRecordType.A))
                {
                    return false;
                }

                //The following steps are applicable only for TPS since it has enabling/disabling ipv4 stack option                
                if (activityData.ProductFamily == ProductFamilies.TPS.ToString())
                {
                    TraceFactory.Logger.Info(CtcUtility.WriteStep("Step VII: LLMNR A query resolution when IPv4 stack is in Enabled State"));
                    if (!VerifyQueryWithIpv4(activityData, testNo, true, DnsRecordType.A))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info(CtcUtility.WriteStep("Step VIII: LLMNR PTR query resolution when IPv4 stack is in Disabled State"));
                    if (VerifyQueryWithIpv4(activityData, testNo, false, DnsRecordType.PTR))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info(CtcUtility.WriteStep("Step IX:	LLMNR PTR query resolution when IPv4 stack is in Enabled State"));
                    if (!VerifyQueryWithIpv4(activityData, testNo, true, DnsRecordType.PTR))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info(CtcUtility.WriteStep("Step X:	LLMNR AAAA query resolution when IPv4 stack is in Disabled State"));
                    if (VerifyQueryWithIpv4(activityData, testNo, false, DnsRecordType.AAAA))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info(CtcUtility.WriteStep("Step XI: LLMNR AAAA query resolution when IPv4 stack is in Enabled State"));
                    if (!VerifyQueryWithIpv4(activityData, testNo, true, DnsRecordType.AAAA))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info(CtcUtility.WriteStep("Step XII: LLMNR A query resolution when IPv4 stack is in Disable State"));

                    return !VerifyQueryWithIpv4(activityData, testNo, false, DnsRecordType.A);
                }
                return true;

            }
            finally
            {
                if (!(activityData.ProductFamily == ProductFamilies.InkJet.ToString()))
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                    printer.ColdReset();
                }
            }
        }

        #endregion

        #region SNMPTrap

        /// <summary>
        /// 702614(Note: Steps received from arul. different from steps in ALM)
        /// 0. Device should be up and running with hostname, domain name with invalid one AND DNS SUFFIX WITH VALID ONE.
        /// 1. ( SYTEM-INTERFACE-NPCARD-INETTRAP-INETRRAPDESTINATION- INETtrapdestination table- inettrapdestinationENTRY-in inettrapaddresstype to be set as 16 in 1st instances.
        /// 2.in inettrapAddress to be set as snmptrap running machine's Hostname.
        /// 3.in inettraprowstatus to be set as 1 in 1st instances.
        /// 4.aDD THE a ENTRY OF UR SNMP TRAP RUNNING HOSTNAME TO DNS SERVER,
        /// 5.IN SNMP SELECT INETTRPTEST AND SET THE VALUE AS ANYTHING EXAMPLE 55
        /// 6.CHECK THE WIRESHARK. IT WILL RESOVLE OF INVALID DOMAIN NAME AND FINALLY RESOLVE WILL SUCCESFUL BY DNS SUFFIX NAME.( PARALLELL U WILL BEEP SOUND IN SNMP TRAP RUNNING MACHINE)
        /// 7. CHECK SNMP TRAP RINGER  CONSOLE IN BELOW MENU OF THE SNMP PAGE
        /// AND CHECK THAT NPCARDALERT MEESAGE HAS RECEIVED.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifySnmpTrap(NetworkNamingServiceActivityData activityData, bool isIPv4, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            string localHostName = Environment.MachineName;
            string clientIpv4Address = GetClientIpAddress(activityData.PrimaryDhcpServerIPAddress, AddressFamily.InterNetwork).ToString();
            string clientIpv6Address = GetClientIpAddress(activityData.PrimaryDhcpServerIPAddress, AddressFamily.InterNetworkV6).ToString();
            string validDomainName = GetDomainName(activityData.PrimaryDhcpServerIPAddress);

            try
            {
                if (!EwsWrapper.Instance().SetHostname(CtcUtility.GetUniqueHostName()))
                {
                    return false;
                }

                string invalidDomainName = CtcUtility.GetUniqueHostName();

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));

                if (!EwsWrapper.Instance().SetDomainName(invalidDomainName))
                {
                    return false;
                }

                EwsWrapper.Instance().DeleteAllSuffixes();

                if (!EwsWrapper.Instance().SetDNSSuffixList(validDomainName))
                {
                    return false;
                }

                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);

                if (isIPv4)
                {
                    return ValidateSnmpTrap(activityData, activityData.PrimaryDhcpServerIPAddress, testNo, localHostName, validDomainName, invalidDomainName, clientIpv4Address);
                }
                else
                {
                    return ValidateSnmpTrap(activityData, activityData.PrimaryDhcpServerIPAddress, testNo, localHostName, validDomainName, invalidDomainName, clientIpv6Address);
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));
                DnsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// 702613	TEMPLATE Hostname-FQDN resolution using DNS	
        /// Verify Hostname/FQDN resolution using DNS	
        /// "Step-1:
        /// 1. Place both DUT and DNS supported OS on same network.
        /// 2.Configure DUT with the Hostname,Domain name and DNS Server IP address
        /// 3.Now access the WebUI using FQDN (https://FQDNNAME)
        /// 4.Use a sniffer to capture the DNS trace
        /// Step-2:
        /// Use SNMP Traps Test to resolve DNS "	
        /// "Step-1:
        /// It should resolve and WebUI should be able to access using Hostname/FQDN Name
        /// Step-2:
        /// It should resolve using DNS and traps should be sent successfully "
        /// Note: As per Arul input need to exclude step2 till he comes back because step2 is not working manually also
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyHostNameResolution(NetworkNamingServiceActivityData activityData, bool isIPv4, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            string hostName = CtcUtility.GetUniqueHostName();
            string domainName = GetDomainName(activityData.PrimaryDhcpServerIPAddress);

            try
            {
                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetDomainName(domainName))
                {
                    return false;
                }

                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

                if (isIPv4)
                {
                    AddDnsRecord(activityData, activityData.PrimaryDhcpServerIPAddress, domainName, hostName, activityData.WiredIPv4Address);
                }
                else
                {
                    AddDnsRecord(activityData, activityData.PrimaryDhcpServerIPAddress, domainName, hostName, printer.IPv6LinkLocalAddress.ToString());
                }

                using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);
                    dhcpClient.Channel.SetDnsServer(activityData.PrimaryDhcpServerIPAddress, scope, activityData.PrimaryDhcpServerIPAddress);
                }
                Thread.Sleep(TimeSpan.FromMinutes(1));

                string fqdn = "{0}.{1}".FormatWith(hostName, domainName);

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

                    if (!printer.IsEwsAccessible(fqdn, "https"))
                    {
                        return false;
                    }

                    Thread.Sleep(TimeSpan.FromMinutes(1));

                    PacketDetails packetDetails = client.Channel.Stop(guid);

                    if (!ValidateQuery(packetDetails.PacketsLocation, activityData.PrimaryDhcpServerIPAddress, DnsRecordType.A, fqdn))
                    {
                        return false;
                    }

                    if (!ValidateResponse(packetDetails.PacketsLocation, activityData.PrimaryDhcpServerIPAddress, DnsRecordType.A, fqdn, activityData.WiredIPv4Address))
                    {
                        return false;
                    }
                }
                return true;
                //As per Arul input need to exclude step2 till he comes back because step2 is not working manually also
                //hostName = Utility.GetUniqueHostName();

                //if (!EwsWrapper.Instance().SetHostName(hostName))
                //{
                //    return false;
                //}

                //return ValidateSnmpTrap(activityData.PrimaryDhcpServerIPAddress, testNo, hostName, domainName, activityData.WiredIPv4Address);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                DnsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// 702622	TEMPLATE Host Name resolution across routers			
        /// Verify Name resolution across routers  with DNS server,Print device and computer client each  residing on different subnets	
        /// "Steps to Reproduce:
        /// 1.Place print device on one subnet
        /// 2.Place DNS server on one subnet
        /// 3.Place Client machine on one subnet 
        /// 4.Have a common router in place
        /// 5.Clear the router cache before actually performing dns name resolution
        /// 6.Configure device SNMP Traps to resolve hostname on client machine"	
        /// Expected: It should be able to resolve DNS name and traps should be sent successfully across subnets
        /// Arul:Printer and DNS Server should be on one network client should be on another network
        /// Since Manual team are working on snmp trap just we are resolving host across routers
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool HostNameResolutionAcrossRouters(NetworkNamingServiceActivityData activityData, bool isIpv4, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPreRequisites(activityData))
            {
                return false;
            }
            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            string firstClientNetworkName = string.Empty;
            string secondClientNetworkName = string.Empty;
            string secondaryServerIpv6 = string.Empty;
            string scope = string.Empty;
            string scopev6 = string.Empty;
            string primaryServerIpv6 = string.Empty;

            string hostName = CtcUtility.GetUniqueHostName();
            string domainName = GetDomainName(activityData.SecondDhcpServerIPAddress);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Keeping client and server in different subnet
                // Get the NIC having IP address in the server IP range so that the NIC can be disabled.
                firstClientNetworkName = GetClientNetworkName(activityData.PrimaryDhcpServerIPAddress);
                secondClientNetworkName = GetClientNetworkName(activityData.SecondDhcpServerIPAddress);

                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

				if (!EwsWrapper.Instance().SetDomainName(domainName))
				{
					return false;
				}
                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.SecondDhcpServerIPAddress);

                //Setting up the secondary dns server on the printer
                if (isIpv4)
				{					
					using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
					{
						scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);
						dhcpClient.Channel.SetDnsServer(activityData.PrimaryDhcpServerIPAddress, scope, activityData.SecondDhcpServerIPAddress);
					}
					using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress))
					{
						scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.SecondDhcpServerIPAddress);
						dhcpClient.Channel.SetDnsServer(activityData.SecondDhcpServerIPAddress, scope, activityData.SecondDhcpServerIPAddress);
					}
				}
				else
				{
					using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress))
					{
						secondaryServerIpv6 = dhcpClient.Channel.GetIPv6Address();
						scopev6 = dhcpClient.Channel.GetIPv6Scope(activityData.SecondDhcpServerIPAddress);
						dhcpClient.Channel.DeleteDnsv6Server(activityData.SecondDhcpServerIPAddress, scopev6);
						dhcpClient.Channel.SetDnsv6ServerIP(activityData.SecondDhcpServerIPAddress, scopev6, secondaryServerIpv6);
					}
					using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
					{						
						scopev6 = dhcpClient.Channel.GetIPv6Scope(activityData.PrimaryDhcpServerIPAddress);
						dhcpClient.Channel.DeleteDnsv6Server(activityData.PrimaryDhcpServerIPAddress, scopev6);
						dhcpClient.Channel.SetDnsv6ServerIP(activityData.PrimaryDhcpServerIPAddress, scopev6, secondaryServerIpv6);
					}
                    EwsWrapper.Instance().SetPrimaryDnsv6Server(secondaryServerIpv6);
                }
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

				using (DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress))
				{
					if (isIpv4)
					{
						if (dnsClient.Channel.AddRecord(domainName, hostName, "A", activityData.WiredIPv4Address))
						{
							TraceFactory.Logger.Info("Successfully added 'A' record for the host name: {0} and IP Address: {1} to secondary dhcp server:{2}"
									.FormatWith(hostName, activityData.WiredIPv4Address, activityData.SecondDhcpServerIPAddress));
						}
						else
						{
							TraceFactory.Logger.Info("Failed to add 'A' record for the host name: {0} and IP Address: {1} to secondary dhcp server:{2}"
									.FormatWith(hostName, activityData.WiredIPv4Address, activityData.SecondDhcpServerIPAddress));
							return false;
						}
					}
					else
					{
                        EwsWrapper.Instance().SetDHCPv6OnStartup(true);
                        EwsWrapper.Instance().SetIPv6(false);
                        EwsWrapper.Instance().SetIPv6(true);
                        if (dnsClient.Channel.AddRecord(domainName, hostName, "AAAA", printer.IPv6StateFullAddress.ToString()))
                        {
                            TraceFactory.Logger.Info("Successfully added 'AAAA' record for the host name: {0} and IP Address: {1} to secondary dhcp server:{2}"
                                    .FormatWith(hostName, printer.IPv6StateFullAddress.ToString(), activityData.SecondDhcpServerIPAddress));
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Failed to add 'AAAA' record for the host name: {0} and IP Address: {1} to secondary dhcp server:{2}"
                                    .FormatWith(hostName, printer.IPv6StateFullAddress.ToString(), activityData.SecondDhcpServerIPAddress));
                            return false;
                        }
                    }
                }

                string fqdn = "{0}.{1}".FormatWith(hostName, domainName);
                if (printer.IsEwsAccessible(fqdn, "https"))
                {
                    TraceFactory.Logger.Info("EWS Page is accessible with FQDN:{0}".FormatWith(fqdn));
                }
               
                // Keeping the printer and client in primary subnet by disabling the secondary network on client .
                TraceFactory.Logger.Info("Disabling the network corresponding to DHCP server: {0}".FormatWith(activityData.SecondDhcpServerIPAddress));
				NetworkUtil.DisableNetworkConnection(secondClientNetworkName);

                //Since it was taking a long time to open FQDN through EWS Page, trying the access with delay
                for (int i = 0; i <= 4; i++)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(60));
                    if (printer.IsEwsAccessible(fqdn, "https"))
                    {
                        return true;
                    }                    
                }
                return false;
				//As per Arul input need to exclude step2 till he comes back because step2 is not working manually also
				//return ValidateSnmpTrap(activityData.SecondDhcpServerIPAddress, testNo, hostName, domainName, "", activityData.WiredIPv4Address);
			}
			finally
			{       
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);

                TraceFactory.Logger.Info("Enabling back the network corresponding to DHCP server: {0}".FormatWith(activityData.SecondDhcpServerIPAddress));
                NetworkUtil.EnableNetworkConnection(secondClientNetworkName);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    if (isIpv4)
                    {
                        scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);
                        dhcpClient.Channel.SetDnsServer(activityData.PrimaryDhcpServerIPAddress, scope, activityData.PrimaryDhcpServerIPAddress);
                    }
                    else
                    {
                        primaryServerIpv6 = dhcpClient.Channel.GetIPv6Address();
                        scopev6 = dhcpClient.Channel.GetIPv6Scope(activityData.PrimaryDhcpServerIPAddress);
                        dhcpClient.Channel.SetDnsv6ServerIP(activityData.PrimaryDhcpServerIPAddress, scopev6, primaryServerIpv6);
                    }
                }
                DnsPostrequisites(activityData);
            }
        }

        #endregion

        #region Linux

        /// <summary>
        /// Template Id: 75396
        /// 1. Web into EWS using a web browser. 
        /// 2. Go to Networking -> “Advanced” tab. 
        /// 3. Manually Enable DHCPv4 FQDN RFC compliance 
        /// 4. Apply the changes	
        /// Expected:
        /// DHCPv4 FQDN RFC compliance option should be enabled. 
        /// Changes made in EWS should be reflected in SNMP also
        /// Template: 75397 
        /// Procedure 
        /// 1. Web into EWS using a web browser. 
        /// 2. Go to Networking -> "Advanced" tab. 
        /// 3. Manually Disable DHCPv4 FQDN RFC compliance 
        /// 4. Apply the changes
        /// Expected:
        /// DHCPv4 FQDN RFC compliance option should be disabled. 
        /// Changes made in EWS should be reflected in SNMP also
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="rfcOption"></param>
        /// <returns></returns>
        public static bool VerifyRfcComplianceOption(NetworkNamingServiceActivityData activityData, bool rfcOption)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            try
            {
                IPAddress linuxPrinterAddress = IPAddress.None;

                if (!LinuxPreRequisites(activityData, LinuxServiceType.DHCP, ref linuxPrinterAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                bool isTps = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily) ? true : false;
                EwsWrapper.Instance().SetRfc(rfcOption);

                return VerifyRfcOption(rfcOption, isTps);
            }
            finally
            {
                LinuxPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template Id: 702631
        /// 1. Set Hostname (INDIA), Domain name (HP.COM) and FQDN values (CTC.JETD.COM) in DHCPv4    Server scope options 
        /// 2. Disable RFC 4702 option in Jetd Manually 
        /// 3. Configure the card via DHCP and set lease time to 2 mins 
        /// 4. Set DHCP as highest config precedence 
        /// 5. Power cycle the printer or wait for the lease renewal 
        /// 6. Check the DHCPv4 FQDN Data in JD	
        /// Expected:
        /// The device should be configured with Hostname and Domain name values supplied by DHCP Server
        /// Template Id: 702635
        /// 1. Set Hostname (INDIA), Domain name (HP.COM) and FQDN values (CTC.JETD.COM) in DHCPv4    Server scope options 
        /// 2. Enable RFC 4702 option in Jetd Manually 
        /// 3. Configure the card via DHCP and set lease time to 2 mins 
        /// 4. Set DHCP as highest config precedence 
        /// 5. Power cycle the printer or wait for the lease renewal 
        /// 6. Check the DHCPv4 FQDN Data in JD	
        /// Expected:
        /// The device should be configured with FQDN Supplied by DHCP server 
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="rfcOption"></param>
        /// <returns></returns>
        public static bool VerifyHostNameWithRfcCompliance(NetworkNamingServiceActivityData activityData, bool rfcOption)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            try
            {
                IPAddress linuxPrinterAddress = IPAddress.None;

                if (!LinuxPreRequisites(activityData, LinuxServiceType.DHCP, ref linuxPrinterAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                IPAddress linuxServerAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);
                string hostName = "LinuxHostname";
                string domainName = "LinuxDomainname.com";
                string fqdnHost = "FqdnHostname";
                string fqdnDomain = "FqdnDomain.com";
                string fqdn = "{0}.{1}".FormatWith(fqdnHost, fqdnDomain);

                EwsWrapper.Instance().SetRfc(rfcOption);
                ConfigureDhcpFile(linuxServerAddress, hostName, domainName, fqdn);
                LinuxUtils.ReplaceValue(linuxServerAddress, LinuxUtils.LINUX_DHCP_PATH, "#{0}".FormatWith(LinuxUtils.KEY_VENDOR_OPTION), VENDOR_OPTION);
                LinuxUtils.ReplaceValue(linuxServerAddress, LinuxUtils.LINUX_DHCP_PATH, "#{0}".FormatWith(LinuxUtils.KEY_VENDOR_STATUS), VENDOR_STATUS.FormatWith(rfcOption.ToString().ToLowerInvariant()));
                LinuxUtils.RestartService(linuxServerAddress, LinuxServiceType.DHCP);
                if (!(activityData.ProductFamily == ProductFamilies.InkJet.ToString()))
                {
                    TraceFactory.Logger.Debug("Setting DHCP as highest precedence.");
                    SnmpWrapper.Instance().SetConfigPrecedence("2:0:1:3:4");
                    Thread.Sleep(TimeSpan.FromMinutes(1));

                    EwsWrapper.Instance().ReinitializeConfigPrecedence();
                }
                Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, linuxPrinterAddress.ToString());
                printer.PowerCycle();

                string validateHostname = rfcOption ? fqdnHost : hostName;
                string validateDomainname = rfcOption ? fqdnDomain : domainName;

                return ValidateHostDomainName(validateHostname, validateDomainname);
            }
            finally
            {
                LinuxPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template Id: 702646
        /// 1. Set Hostname (CTC), Domain name (HP.COM) and FQDN values (DHCP.SERVER.COM) 
        /// 2. Configure the card via DHCP and set lease time to 2 mins 
        /// 3. Power cycle the printer or wait for the lease renewal 
        /// 4. Check the FQDN Data in device Variance is disabled :
        /// 1.Specify Hostname and Domain name in Server 
        /// 2.With max supported 255 characters	
        /// Expected:
        /// Device should accept Hostname and Domain name to form FQDN (CTC.HP.COM)and ignore 
        /// Client FQDN  value (DHCP.SERVER.COM)
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool VerifyFqdnConfiguration(NetworkNamingServiceActivityData activityData)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            try
            {
                IPAddress linuxPrinterAddress = IPAddress.None;

                if (!LinuxPreRequisites(activityData, LinuxServiceType.DHCP, ref linuxPrinterAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                IPAddress linuxServerAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);
                string hostName = "LinuxHostname";
                string domainName = "LinuxDomainname.com";
                string fqdnHost = "FqdnHostname";
                string fqdnDomain = "FqdnDomain.com";
                string fqdn = "{0}.{1}".FormatWith(fqdnHost, fqdnDomain);

                Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, linuxPrinterAddress.ToString());
                bool isTps = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily) ? true : false;

                if (!isTps)
                {
                    ConfigureDhcpFile(linuxServerAddress, hostName, domainName, fqdn);
                    LinuxUtils.ReplaceValue(linuxServerAddress, LinuxUtils.LINUX_DHCP_PATH, "#{0}".FormatWith(LinuxUtils.KEY_VENDOR_OPTION), VENDOR_OPTION);
                    LinuxUtils.ReplaceValue(linuxServerAddress, LinuxUtils.LINUX_DHCP_PATH, "#{0}".FormatWith(LinuxUtils.KEY_VENDOR_STATUS), VENDOR_STATUS.FormatWith("false"));
                    LinuxUtils.RestartService(linuxServerAddress, LinuxServiceType.DHCP);

                    printer.PowerCycle();

					if (!VerifyRfcOption(false))
					{
						return false;
					}
                    Thread.Sleep(TimeSpan.FromMinutes(1));
					if (!ValidateHostDomainName(hostName, domainName))
					{
						return false;
					}
				}

                hostName = "63characterhostnamelength_qazxswedcvfrtgbnhyujmkiolp123456789";
                domainName = "sixtythreecharacterdomainnamelength_qazxswedcvfrtgbnhyujmkiolp1.sixtythreecharacterdomainnamelength_qazxswedcvfrtgbnhyujmkiolp1.sixtythreecharacterdomainnamelength_qazxswedcvfrtgbnhyujmkiolp1";

                ConfigureDhcpFile(linuxServerAddress, hostName, domainName, string.Empty);
                LinuxUtils.RestartService(linuxServerAddress, LinuxServiceType.DHCP);

				printer.PowerCycle();                
				if (!isTps)
				{
					hostName = hostName.Substring(0, 32);
				}
                Thread.Sleep(TimeSpan.FromMinutes(1));
                return ValidateHostDomainName(hostName, domainName);
            }
            finally
            {
                LinuxPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template Id: 75402
        /// Step 1:
        /// Jetdirect : 
        /// 1. Enable RFC 4702 option in Jetd Manually 
        /// 2. Power cycle the printer 
        /// TPS : 
        /// 1. Disable RFC 4702 option in TPS product manually from EWS. 
        /// 2. Power cycle the device.	
        /// Step 2:
        /// Procedure 1 : 1. Configure WINS server IP address manually on the device. 
        /// 2. Power cycle the device 
        /// Procedure 2: 1. Provide WINS server IP address from DHCP server. 
        /// 2. Power off the device. 
        /// 3. Delete the WINS IP address from the DHCP server. 
        /// 3. Power up the device			
        /// Step 3:
        /// 1.Set the Hostname,Domain,Primary and Secondary DNS Server options of the DHCP server 
        /// 2.Configure device with DHCP. 
        /// 3.Change options or remove options on the DHCP server scope where the JD device was initially configured. 
        /// 4.Reboot the JD device.
        /// Step 4:
        /// Procedure 1 : 
        /// 1. Disable LLMNR feature 
        /// 2. Power cycle DUT 
        /// 3. Verify LLMNR feature status after power cycle. 
        /// Procedure 2 : 
        /// 1. Enable LLMNR feature 
        /// 2. Power cycle DUT 
        /// 3. Verify Uniquess behavior after power cycle	
        /// Expected:
        /// Step 1:
        /// Jetdirect : 
        /// 1.By Default, Client FQDN option in EWS should be disabled. 
        /// 2.If vendor specific option is set to true in DHCP Server, then RFC 4702 FQDN Option should be enabled automatically 
        /// TPS : 
        /// 1.By Default, Client FQDN option in EWS is enabled. 
        /// 2. When disabled and power cycle, it should remain in the disabled state.  
        /// Step 2:
        /// WINS server IP address should be retained over power cycle and the device should send WINS regiatration packets and should get registered with WINS server 
        /// When the device comes up after power cycle, since the WINS server IP address is not provided from DHCP server it should have default addressTPS : Device should get configured with the value for WINS Address according to config precedence.If nothing is privided it should retain the default address.
        /// Step 3:
        /// 1.The device should be configured with new values supplied by dhcp server 
        /// 2.Changes made in one place should reflect in other applications also 
        /// For Ex: Changes made in EWS should be reflected in SNMP,Control Panel and Telnet3. Device should retain these parameters even after power cycle.
        /// Step 4:
        /// 1. LLMNR feature should not get enabled after power cycle
        /// 2. LLMR should be enabled
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool VerifyBehaviorAfterPowercycle(NetworkNamingServiceActivityData activityData)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            IPAddress linuxPrinterAddress = IPAddress.None;

            try
            {
                if (!LinuxPreRequisites(activityData, LinuxServiceType.DHCP, ref linuxPrinterAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                bool isTps = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily) ? true : false;
                bool rfcOption = isTps ? true : false;
                IPAddress linuxServerAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step 1: DHCPv4 Client FQDN Power cycle behavior"));

                TraceFactory.Logger.Info("Vendor specific option is configured to false on server.");

                ConfigureDhcpFile(linuxServerAddress, "LinuxHostname", "LinuxDomainname.com");
                LinuxUtils.ReplaceValue(linuxServerAddress, LinuxUtils.LINUX_DHCP_PATH, "#{0}".FormatWith(LinuxUtils.KEY_VENDOR_OPTION), VENDOR_OPTION);
                LinuxUtils.ReplaceValue(linuxServerAddress, LinuxUtils.LINUX_DHCP_PATH, "#{0}".FormatWith(LinuxUtils.KEY_VENDOR_STATUS), VENDOR_STATUS.FormatWith(rfcOption.ToString().ToLowerInvariant()));
                LinuxUtils.RestartService(linuxServerAddress, LinuxServiceType.DHCP);

                EwsWrapper.Instance().SetRfc(!rfcOption);

                Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, linuxPrinterAddress.ToString());
                printer.PowerCycle();

                if (EwsWrapper.Instance().GetRfc().Equals(true))
                {
                    TraceFactory.Logger.Info("Option Rfc 4702 is not disabled.");
                    return false;
                }

                TraceFactory.Logger.Info("Option Rfc 4702 is disabled.");
                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step 2: WINS address over power cycle"));

                int length = linuxServerAddress.ToString().Length;
                int lastIndex = linuxServerAddress.ToString().LastIndexOf('.') + 1;
                string manualWinsAddress = string.Format(CultureInfo.CurrentCulture, "{0}{1}", linuxServerAddress.ToString().Remove(lastIndex, length - lastIndex), "100");

                EwsWrapper.Instance().SetPrimaryWinServerIP(manualWinsAddress);
                printer.PowerCycle();

                if (!EwsWrapper.Instance().GetPrimaryWinServerIP().EqualsIgnoreCase(manualWinsAddress))
                {
                    TraceFactory.Logger.Debug("Primary Wins server IP: {0}".FormatWith(EwsWrapper.Instance().GetPrimaryWinServerIP()));
                    TraceFactory.Logger.Info("Primary Wins Server is not same after powercycle.");
                    return false;
                }

                TraceFactory.Logger.Debug("Setting DHCP as highest precedence.");
                SnmpWrapper.Instance().SetConfigPrecedence("2:0:1:3:4");
                Thread.Sleep(TimeSpan.FromMinutes(1));

                string wninsAddress = "option netbios-name-servers {0};".FormatWith(linuxServerAddress);

                LinuxUtils.ReplaceValue(linuxServerAddress, LinuxUtils.LINUX_DHCP_PATH, "#{0}".FormatWith(LinuxUtils.KEY_PRIMARY_WINS), wninsAddress);
                LinuxUtils.RestartService(linuxServerAddress, LinuxServiceType.DHCP);

                printer.PowerCycle();

                if (!EwsWrapper.Instance().GetPrimaryWinServerIP().EqualsIgnoreCase(linuxServerAddress.ToString()))
                {
                    TraceFactory.Logger.Debug("Primary Wins server IP: {0}".FormatWith(EwsWrapper.Instance().GetPrimaryWinServerIP()));
                    TraceFactory.Logger.Info("Primary Wins Server is not updated from server after powercycle.");
                    return false;
                }

                LinuxUtils.ReplaceValue(linuxServerAddress, LinuxUtils.LINUX_DHCP_PATH, wninsAddress, "#");
                LinuxUtils.RestartService(linuxServerAddress, LinuxServiceType.DHCP);

                printer.PowerCycle();

                bool validateWins = false;

                /* Behavioral changes
				 * TPS: If printer is previously configured manually, when DHCP server is not providing any values; manually configured value will be set on printer
				 * VEP/ LFP: When DHCP server is not providing any values, no values will be set on printer
				* */

                if (isTps)
                {
                    validateWins = EwsWrapper.Instance().GetPrimaryWinServerIP().EqualsIgnoreCase(manualWinsAddress);
                }
                else
                {
                    validateWins = string.IsNullOrEmpty(EwsWrapper.Instance().GetPrimaryWinServerIP());
                }

                if (!validateWins)
                {
                    TraceFactory.Logger.Debug("Primary Wins server IP: {0}".FormatWith(EwsWrapper.Instance().GetPrimaryWinServerIP()));
                    TraceFactory.Logger.Info("Primary Wins Server is not in default state after powercycle.");
                    return false;
                }

                EwsWrapper.Instance().ResetConfigPrecedence();
                Thread.Sleep(TimeSpan.FromMinutes(1));

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step 3: Name resolution using DNS after power cycle"));

                string hostname = "Linuxhostname";
                string domainname = "LinuxDomainname.com";
                string dnsAddress = "option domain-name-servers {0}, {0};".FormatWith(linuxServerAddress);

                ConfigureDhcpFile(linuxServerAddress, hostname, domainname);
                LinuxUtils.ReplaceValue(linuxServerAddress, LinuxUtils.LINUX_DHCP_PATH, "#{0}".FormatWith(LinuxUtils.KEY_PRIMARY_DNS), dnsAddress);
                LinuxUtils.RestartService(linuxServerAddress, LinuxServiceType.DHCP);

                printer.PowerCycle();

                if (!ValidateHostDomainName(hostname, domainname, validateDns: true, dns: linuxServerAddress.ToString()))
                {
                    return false;
                }

                hostname = "ManualHostname";
                domainname = "ManualDomainname.com";

                SnmpWrapper.Instance().SetHostName(hostname);
                //TelnetWrapper.Instance().SetDomainname(domainname); 
                SnmpWrapper.Instance().SetDomainName(domainname);


                printer.PowerCycle();

                if (!ValidateHostDomainName(hostname, domainname))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step 4: LLMNR feature behavior after power cycle"));

                EwsWrapper.Instance().SetLlmnr(false);
                printer.PowerCycle();

                if (EwsWrapper.Instance().GetLlmnr().Equals(true))
                {
                    return false;
                }

                TraceFactory.Logger.Info("LLMNR feature is disabled as expected.");

                EwsWrapper.Instance().SetLlmnr(true);
                printer.PowerCycle();

                if (EwsWrapper.Instance().GetLlmnr().Equals(false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("LLMNR feature is enabled as expected.");

                return true;
            }
            finally
            {
                LinuxPostRequisites(activityData);
            }

        }

        /// <summary>
        /// Template Id: 702647
        /// 1. Set and Enable Hostname, Domainname and FQDN options in DHCPv4 Server
        /// 2. Also Set and Enable Vendor specific option to true in DHCP Server
        /// 3. Cold reset the printer
        /// 4. Check the FQDN Data in JD
        /// Expected:
        /// RFC 4702 FQDN option should be enabled automatically
        /// Hostname and Domain name should be configured with FQDN Data (option 81) specified by DHCP Server
        /// Tempalte Id: 702648
        /// 1. Set and Enable Hostname, Domainname and FQDN options in DHCPv4 Server
        /// 2. Also Set and Enable Vendor specific option to false in DHCP Server
        /// 3. Cold reset the printer
        /// 4. Check the FQDN Data in JD
        /// Expected:
        /// RFC 4702 FQDN option should be disabled automatically
        /// Hostname and Domain name should not be configured with FQDN Data (option 29) specified by DHCP Server
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="rfcOption">true for enabling option, false to disable</param>
        /// <returns></returns>
        public static bool VerifyFQDNWithVendorOption(NetworkNamingServiceActivityData activityData, bool rfcOption)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            try
            {
                IPAddress linuxPrinterAddress = IPAddress.None;

                if (!LinuxPreRequisites(activityData, LinuxServiceType.DHCP, ref linuxPrinterAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                IPAddress linuxServerAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);
                string hostName = "LinuxHostname";
                string domainName = "LinuxDomainname.com";
                string fqdnHost = "FqdnHostname";
                string fqdnDomain = "FqdnDomain.com";
                string fqdn = "{0}.{1}".FormatWith(fqdnHost, fqdnDomain);

                EwsWrapper.Instance().SetRfc(rfcOption);
                ConfigureDhcpFile(linuxServerAddress, hostName, domainName, fqdn);
                LinuxUtils.ReplaceValue(linuxServerAddress, LinuxUtils.LINUX_DHCP_PATH, "#{0}".FormatWith(LinuxUtils.KEY_VENDOR_OPTION), VENDOR_OPTION);
                LinuxUtils.ReplaceValue(linuxServerAddress, LinuxUtils.LINUX_DHCP_PATH, "#{0}".FormatWith(LinuxUtils.KEY_VENDOR_STATUS), VENDOR_STATUS.FormatWith(rfcOption.ToString().ToLowerInvariant()));
                LinuxUtils.RestartService(linuxServerAddress, LinuxServiceType.DHCP);
                if (!(activityData.ProductFamily == ProductFamilies.InkJet.ToString()))
                {
                    Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, linuxPrinterAddress.ToString());
                    printer.ColdReset();
                }
                EwsWrapper.Instance().SetTelnet();

                string validateHostname = rfcOption ? fqdnHost : hostName;
                string validateDomainname = rfcOption ? fqdnDomain : domainName;

                return ValidateHostDomainName(validateHostname, validateDomainname);
            }
            finally
            {
                LinuxPostRequisites(activityData);
            }
        }

        #endregion

        #region DNS Outbound

        /// <summary>
        /// Validate Scan to Network Folder workflow using the DNS outbound (DNS entries configured)
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>
        /// <param name="testNo">Test no</param>
        /// <param name="isIPv4">true for IPv4, false for IPv6</param>
        /// <param name="isPrimary">true for Primary DNS server, false for Secondary</param>
        /// <returns>Returns true if the test is passed else returns false</returns>
        public static bool VerifyScanToNetworkFolder(NetworkNamingServiceActivityData activityData, int testNo, bool isIPv4 = true, bool isPrimary = true, bool invalidEntries = false, bool dnsSuffix = false)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            if (!DnsPrerequisites(activityData))
            {
                return false;
            }

            string serverAddress = isPrimary ? activityData.PrimaryDhcpServerIPAddress : activityData.SecondDhcpServerIPAddress;

            // Since the new firmware is not capturing packets of Scan to network folder without rebooting the Printer, so adding reboot
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            printer.PowerCycle();

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Configure Host Name, Domain Name
                if (!(EwsWrapper.Instance().SetHostname(CtcUtility.GetUniqueHostName()) && EwsWrapper.Instance().SetDomainName(GetDomainName(serverAddress))))
                {
                    return false;
                }

                string recordType = string.Empty;
                AddressFamily addressFamily;

                // Configure values based on test requirement
                if (isIPv4)
                {
                    recordType = "A";
                    addressFamily = AddressFamily.InterNetwork;

                    if (isPrimary)
                    {
                        if (!EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!(EwsWrapper.Instance().SetPrimaryDnsServer("100.100.100.100") && EwsWrapper.Instance().SetSecondaryDNSServerIP(activityData.SecondDhcpServerIPAddress)))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    recordType = "AAAA";
                    addressFamily = AddressFamily.InterNetworkV6;

                    if (isPrimary)
                    {
                        if (!EwsWrapper.Instance().SetPrimaryDnsv6Server(activityData.PrimaryDhcpIPv6Address))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!(EwsWrapper.Instance().SetPrimaryDnsv6Server("100:100:100:100::") && EwsWrapper.Instance().SetSecondaryDnsv6Server(activityData.SecondaryDhcpIPv6Address)))
                        {
                            return false;
                        }
                    }
                }

                // Get local machine IP address
                IPAddress localMachineIP = GetClientIpAddress(serverAddress, addressFamily);

                // Add the DNS record on the server
                DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(serverAddress);
                dnsClient.Channel.AddRecord(GetDomainName(serverAddress), Environment.MachineName, recordType, localMachineIP.ToString());
                TraceFactory.Logger.Info("Added record with Server :{0}".FormatWith(serverAddress));
                // if invalid entries are required add them to the server
                if (invalidEntries)
                {
                    AddInvalidEntries(serverAddress, isIPv4, localMachineIP.ToString());
                }

                //if resolution is through dnssuffix add an invalid domain name and a valid dnssuffix
                if (dnsSuffix)
                {
                    if (!EwsWrapper.Instance().SetDomainName(CtcUtility.GetUniqueDomainName()))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetDNSSuffixList(GetDomainName(serverAddress)))
                    {
                        return false;
                    }
                }

                return ScanToNetworkFolder(activityData, isPrimary, "{0}-BasicScan".FormatWith(testNo), localMachineIP, testNo, isIPv4);
            }
            catch
            {
                TraceFactory.Logger.Info("Error Occurred while executing the test");
                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                if (dnsSuffix && !(activityData.ProductFamily == "TPS"))
                {
                    EwsWrapper.Instance().DeleteAllSuffixes();
                }
                DnsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// Adds 76 invalid IPv4 and IPv6 address
        /// It may also contain the valid clinet address also
        /// </summary>
        /// <param name="serverIP">Server IP Address</param>
        private static void AddInvalidEntries(string serverIP, bool isIpv4, string localMachineIP)
        {
            if (isIpv4)
            {
                //Need to add 75 invalid DNS entries for IPv4 and 75 invalid entries for IPv6
                DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(serverIP);
                string domainName = GetDomainName(serverIP);

                int length = localMachineIP.Length;
                int lastIndex = localMachineIP.LastIndexOf('.') + 1;
                string serverAddressFormat = serverIP.Remove(lastIndex, length - lastIndex) + "{0}";

                for (int i = 2; i <= 77; i++)
                {
                    if (!serverAddressFormat.FormatWith(i).EqualsIgnoreCase(localMachineIP))
                    {
                        dnsClient.Channel.AddRecord(domainName, "a" + i + ".hssl.com", "A", serverAddressFormat.FormatWith(i));
                    }
                }
            }
            else
            {
                //Need to add 75 invalid DNS entries for IPv4 and 75 invalid entries for IPv6
                DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(serverIP);
                string domainName = GetDomainName(serverIP);

                int length = localMachineIP.Length;
                int lastIndex = localMachineIP.LastIndexOf(':') + 1;
                string serverAddressFormat = localMachineIP.Remove(lastIndex, length - lastIndex) + "{0}";

                for (int i = 2; i <= 77; i++)
                {
                    if (!serverAddressFormat.FormatWith(i).EqualsIgnoreCase(localMachineIP))
                    {
                        dnsClient.Channel.AddRecord(domainName, "a" + i + ".hssl.com", "AAAA", serverAddressFormat.FormatWith(i));
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Verify Primary WINS server registration
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>		
        /// <returns>True if the registration is successful, else false.</returns>
        private static bool VerifyWinsRegistration(NetworkNamingServiceActivityData activityData, bool isPrimary = true, string primaryIPAddress = "100.100.100.100")
        {
            if (isPrimary)
            {
                if (!EwsWrapper.Instance().SetPrimaryWinServerIP(activityData.PrimaryDhcpServerIPAddress) || !EwsWrapper.Instance().SetSecondaryWinServerIP(string.Empty))
                {
                    return false;
                }
            }
            else
            {
                if (!EwsWrapper.Instance().SetPrimaryWinServerIP(primaryIPAddress) || !EwsWrapper.Instance().SetSecondaryWinServerIP(activityData.SecondDhcpServerIPAddress))
                {
                    return false;
                }
            }

            string hostName = CtcUtility.GetUniqueHostName();

            if (!EwsWrapper.Instance().SetHostname(hostName))
            {
                return false;
            }

            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            printer.PowerCycle();

            string serverIP = isPrimary ? activityData.PrimaryDhcpServerIPAddress : activityData.SecondDhcpServerIPAddress;

            using (WinsApplicationServiceClient winsClient = WinsApplicationServiceClient.Create(serverIP))
            {
                if (winsClient.Channel.ValidateWinServerLog(serverIP, hostName, "00", activityData.WiredIPv4Address))
                {
                    TraceFactory.Logger.Info("{0} WINS server log contains device host name: {1} and IP address: {2}".FormatWith(isPrimary ? "Primary" : "Secondary", hostName, activityData.WiredIPv4Address));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("{0} WINS server log doesn't contain device host name: {1} and IP address: {2}".FormatWith(isPrimary ? "Primary" : "Secondary", hostName, activityData.WiredIPv4Address));
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to validate Wins Server Log");
                    return false;
                }
            }
        }

        private static bool VerifySyslogForParameterChanges(string serverIPAddress, string printerIpAddress)
        {
            EwsWrapper.Instance().ChangeDeviceAddress(printerIpAddress);
            //In case if the test case failed to perform postrequisite[deleting the win server ip]
            //Setting the already existing winserver do not trigger the registration,so setting null and then assigning serverip
            EwsWrapper.Instance().SetPrimaryWinServerIP(string.Empty);
            EwsWrapper.Instance().SetPrimaryWinServerIP(serverIPAddress);

            DateTime startTime = DateTime.Now;

            string hostName = CtcUtility.GetUniqueHostName();

            if (!EwsWrapper.Instance().SetHostname(hostName))
            {
                return false;
            }

            DateTime endTime = DateTime.Now;

            using (KiwiSyslogApplicationServiceClient syslogClient = KiwiSyslogApplicationServiceClient.Create(serverIPAddress))
            {
                if (syslogClient.Channel.ValidateEntry(startTime, endTime, "{0}\tprinter: registered system name {1} with WINS server {2}".FormatWith(printerIpAddress, hostName.ToUpper(CultureInfo.CurrentCulture), serverIPAddress)))
                {
                    TraceFactory.Logger.Info("Registration entries are present in the Syslog.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("No registration entries are present in the Syslog.");
                    return false;
                }
            }
        }

        private static bool ValidateDnsParameters(NetworkNamingServiceActivityData activityData, IPConfigMethod configMethod, bool isIPV4)
        {
            try
            {
                if (activityData.ProductFamily == ProductFamilies.TPS.ToString())
                {
                    EwsWrapper.Instance().ResetConfigPrecedence();
                }
                if (!EwsWrapper.Instance().SetIPConfigMethod(configMethod, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    return false;
                }

                string newHostName = CtcUtility.GetUniqueHostName();
                string newDomainName = CtcUtility.GetUniqueHostName();
                string scope = string.Empty;
                string dnsSuffix = CtcUtility.GetUniqueHostName();
                string primaryServerIpv6 = string.Empty;
                string scopev6 = string.Empty;
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

                using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    if (isIPV4)
                    {
                        scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);

                        if (!dhcpClient.Channel.SetHostName(activityData.PrimaryDhcpServerIPAddress, scope, newHostName))
                        {
                            TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to set host name in dhcp server.");
                            return false;
                        }

                        if (!dhcpClient.Channel.SetDomainName(activityData.PrimaryDhcpServerIPAddress, scope, newDomainName))
                        {
                            TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to set domain name in dhcp server.");
                            return false;
                        }

                        if (!dhcpClient.Channel.SetDnsServer(activityData.PrimaryDhcpServerIPAddress, scope, activityData.SecondDhcpServerIPAddress))
                        {
                            TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to set dns server in dhcp server");
                            return false;
                        }
                    }
                    else
                    {
                        primaryServerIpv6 = dhcpClient.Channel.GetIPv6Address();
                        scopev6 = dhcpClient.Channel.GetIPv6Scope(activityData.PrimaryDhcpServerIPAddress);
                        dhcpClient.Channel.DeleteDnsv6Server(activityData.PrimaryDhcpServerIPAddress, scopev6);
                        dhcpClient.Channel.SetDnsv6ServerIP(activityData.PrimaryDhcpServerIPAddress, scopev6, primaryServerIpv6);
                    }
                }

                SnmpWrapper.Instance().SetConfigPrecedence("2:0:3:1:4");
                printer.PowerCycle();

                if (isIPV4)
                {
                    if (EwsWrapper.Instance().GetHostname().EqualsIgnoreCase(newHostName))
                    {
                        TraceFactory.Logger.Info("Host name on the Web UI is updated with the new value: {0} from the server.".FormatWith(newHostName));
                    }
                    else
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Host name on the Web UI is not updated with the new value: {0} from the server.".FormatWith(newHostName));
                        return false;
                    }

                    if (EwsWrapper.Instance().GetDomainName().EqualsIgnoreCase(newDomainName))
                    {
                        TraceFactory.Logger.Info("Domain name on the Web UI is updated with the new value: {0} from the server.".FormatWith(newDomainName));
                    }
                    else
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Domain name on the Web UI is not updated with the new value: {0} from the server.".FormatWith(newDomainName));
                        return false;
                    }

                    if (EwsWrapper.Instance().GetPrimaryDnsServer().EqualsIgnoreCase(activityData.SecondDhcpServerIPAddress))
                    {
                        TraceFactory.Logger.Info("Primary Dns Server on Web UI is updated with new value:{0}".FormatWith(activityData.SecondDhcpServerIPAddress));
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Primary Dns Server on Web UI is not updated with new value:{0}".FormatWith(activityData.SecondDhcpServerIPAddress));
                        return false;
                    }
                }
                else
                {
                    EwsWrapper.Instance().SetIPv6(true);
                    EwsWrapper.Instance().SetDHCPv6OnStartup(true);
                    SnmpWrapper.Instance().SetConfigPrecedence("3:0:1:2:4");

                    if (EwsWrapper.Instance().GetPrimaryDnsv6Server().EqualsIgnoreCase(primaryServerIpv6))
                    {
                        TraceFactory.Logger.Info("Primary Dns Server on Web UI is updated with new value:{0}".FormatWith(primaryServerIpv6));
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Primary Dns Server on Web UI is not updated with new value:{0}".FormatWith(primaryServerIpv6));
                        return false;
                    }
                }
            }
            finally
            {
                EwsWrapper.Instance().SetDHCPv6OnStartup(false);
                EwsWrapper.Instance().ResetConfigPrecedence();
            }
        }

        /// <summary>
        /// Ram: Start WINS service on primary and secondary servers and Stop the DNS service on primary and secondary servers.
        /// </summary>
        /// <param name="activityData">Activity Data</param>
        /// <returns>Returns true if all the services states are set properly</returns>
        private static bool WinsPrerequisites(NetworkNamingServiceActivityData activityData)
        {
            return StartWinsService(activityData.PrimaryDhcpServerIPAddress) &&
                   StartWinsService(activityData.SecondDhcpServerIPAddress) &&
                   StopDnsService(activityData.PrimaryDhcpServerIPAddress) &&
                   StopDnsService(activityData.SecondDhcpServerIPAddress);
        }

        /// <summary>
        /// Start the WINS Service on the server
        /// </summary>
        /// <param name="serverIP">Server IP address</param>
        /// <returns>Returns true if the service is started successfully</returns>
        private static bool StartWinsService(string serverIP)
        {
            using (WinsApplicationServiceClient winsClient = WinsApplicationServiceClient.Create(serverIP))
            {
                if (winsClient.Channel.StartWinsService())
                {
                    TraceFactory.Logger.Info("Successfully started WINS service on: {0} server.".FormatWith(serverIP));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to start WINS service on: {0} server.".FormatWith(serverIP));
                    return false;
                }
            }
        }

        /// <summary>
        /// Stop WINS service on the server
        /// </summary>
        /// <param name="serverIP">Server IP address</param>
        /// <returns>Returns true if the service is stopped successfully</returns>
        private static bool StopWinsService(string serverIP)
        {
            using (WinsApplicationServiceClient winsClient = WinsApplicationServiceClient.Create(serverIP))
            {
                if (winsClient.Channel.StopWinsService())
                {
                    TraceFactory.Logger.Info("Successfully stopped WINS service on: {0} server.".FormatWith(serverIP));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to stop WINS service on: {0} server.".FormatWith(serverIP));
                    return false;
                }
            }
        }

        /// <summary>
        /// Start DNS service on the server
        /// </summary>
        /// <param name="serverIP">Server IP address</param>
        /// <returns>Returns true if the service is started successfully</returns>
        private static bool StartDnsService(string serverIP)
        {
            using (DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(serverIP))
            {
                if (dnsClient.Channel.StartDnsService())
                {
                    TraceFactory.Logger.Info("Successfully started DNS service on: {0}.".FormatWith(serverIP));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to start DNS service on: {0}.".FormatWith(serverIP));
                    return false;
                }
            }
        }

        /// <summary>
        /// Stop DNS service on the server
        /// </summary>
        /// <param name="serverIP">Server IP address</param>
        /// <returns>Returns true if the service is stopped successfully</returns>
        private static bool StopDnsService(string serverIP)
        {
            using (DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(serverIP))
            {
                if (dnsClient.Channel.StopDnsService())
                {
                    TraceFactory.Logger.Info("Successfully stopped DNS service on: {0}.".FormatWith(serverIP));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to stop DNS service on: {0}.".FormatWith(serverIP));
                    return false;
                }
            }
        }

        /// <summary>
        /// Clear Primary and Secondary WINS server IPs on printer.
        /// </summary>
        /// <param name="activityData">Activity Data</param>
        private static void WinsPostrequisites(NetworkNamingServiceActivityData activityData)
        {
            // clearing Primary and Secondary WINS server IPs on printer.
            EwsWrapper.Instance().SetPrimaryWinServerIP(string.Empty);
            EwsWrapper.Instance().SetSecondaryWinServerIP(string.Empty);

            using (WinsApplicationServiceClient winsClient = WinsApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
            {
                winsClient.Channel.DeleteAllRecords(activityData.PrimaryDhcpServerIPAddress);
            }

            using (WinsApplicationServiceClient winsClient = WinsApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress))
            {
                winsClient.Channel.DeleteAllRecords(activityData.SecondDhcpServerIPAddress);
            }
        }

        private static bool DnsPrerequisites(NetworkNamingServiceActivityData activityData)
        {
            //Primary DHCPServer
            using (WinsApplicationServiceClient winsClient = WinsApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
            {
                if (winsClient.Channel.StopWinsService())
                {
                    TraceFactory.Logger.Info("Successfully stopped WINS service on Primary DHCP Server: {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to stop WINS service on Primary DHCP Server: {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                    return false;
                }
            }

            using (DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
            {
                //if (!dnsClient.Channel.StartDnsService())
                if (!CtcUtility.StartService("dns", activityData.PrimaryDhcpServerIPAddress))
                {
                    TraceFactory.Logger.Info("Failed to start DNS service on Primary DHCP Server: {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Successfully started DNS service on Primary DHCP Server: {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                }
            }
            if (!(activityData.ProductFamily == ProductFamilies.InkJet.ToString()))
            {
                //Secondary DHCPServer
                using (WinsApplicationServiceClient winsClient = WinsApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress))
                {
                    if (winsClient.Channel.StopWinsService())
                    {
                        TraceFactory.Logger.Info("Successfully stopped WINS service on Secondary DHCP Server: {0}.".FormatWith(activityData.SecondDhcpServerIPAddress));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to stop WINS service on Secondary DHCP Server: {0}.".FormatWith(activityData.SecondDhcpServerIPAddress));
                        return false;
                    }
                }


                using (DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress))
                {
                    //if (!dnsClient.Channel.StartDnsService())
                    if (!CtcUtility.StartService("dns", activityData.SecondDhcpServerIPAddress))
                    {
                        TraceFactory.Logger.Info("Failed to start DNS service on Secondary DHCP Server: {0}.".FormatWith(activityData.SecondDhcpServerIPAddress));
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Successfully started DNS service on Secondary DHCP Server: {0}.".FormatWith(activityData.SecondDhcpServerIPAddress));
                        return true;
                    }
                }
            }
            return true;

        }

        /// <summary>
        /// Delete the DNS Record-A from the DNS Server
        /// </summary>
        /// <param name="activityData">Activity Data</param>
        private static void DnsPostrequisites(NetworkNamingServiceActivityData activityData, bool isDDNS = false, string fqdn = null)
        {
            using (DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
            {
                if (dnsClient.Channel.DeleteAllRecords(GetDomainName(activityData.PrimaryDhcpServerIPAddress), fqdn))
                {
                    TraceFactory.Logger.Info("Successfully deleted all record(s) from DNS server {0}".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                }
                else
                {
                    TraceFactory.Logger.Debug("Failed to delete record(s) from DNS server {0}".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                }
            }

            using (DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress))
            {
                if (dnsClient.Channel.DeleteAllRecords(GetDomainName(activityData.SecondDhcpServerIPAddress), fqdn))
                {
                    TraceFactory.Logger.Info("Successfully deleted all record(s) from DNS server {0}".FormatWith(activityData.SecondDhcpServerIPAddress));
                }
                else
                {
                    TraceFactory.Logger.Debug("Failed to delete record(s) from DNS server {0}".FormatWith(activityData.SecondDhcpServerIPAddress));
                }
            }

            if (activityData.ProductFamily == ProductFamilies.TPS.ToString())
            {
                //Changed this part since TPS we cant set blank value.
             
                TraceFactory.Logger.Info("Restoring default values for Network Identification Page.");
                EwsWrapper.Instance().ResetConfigPrecedence();
                //TODO: How about v6 address ?
            }
            else if (activityData.ProductFamily == ProductFamilies.VEP.ToString())
            {
                EwsWrapper.Instance().SetPrimaryDnsServer(string.Empty);
                EwsWrapper.Instance().SetPrimaryDnsv6Server(string.Empty);
                EwsWrapper.Instance().SetSecondaryDNSServerIP(string.Empty);
                EwsWrapper.Instance().SetSecondaryDnsv6Server(string.Empty);
                if (isDDNS)
                {
                    EwsWrapper.Instance().SetDDNS(false);
                }
            }
            //If INK DO nothing
        }

        private static bool VerifyDnsSuffixes(List<string> printerDnsSuffixes, string[] serverDnsSuffixes, string manualSuffix)
        {
            // first verify manual entry exists or not
            if (printerDnsSuffixes.Contains(manualSuffix))
            {
                TraceFactory.Logger.Info("Manual DNS suffix {0} is updated on the printer".FormatWith(manualSuffix));
            }
            else
            {
                TraceFactory.Logger.Info("Manual DNS suffix {0} is not updated on the printer".FormatWith(manualSuffix));
                return false;
            }

            // second verify all the server entries found on the printer or not
            foreach (string serverDnsSuffix in serverDnsSuffixes)
            {
                if (printerDnsSuffixes.Contains(serverDnsSuffix))
                {
                    TraceFactory.Logger.Info("Server DNS suffix {0} is updated on the printer".FormatWith(serverDnsSuffix));
                }
                else
                {
                    TraceFactory.Logger.Info("Server DNS suffix {0} is not updated on the printer".FormatWith(serverDnsSuffix));
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checked whether the ping reply contains hostname and either one of the IP address in the output.
        /// </summary>
        /// <param name="hostName">Hostname of the printer</param>
        /// <param name="pingReply">Ping output</param>
        /// <param name="printer">Printer object</param>
        /// <returns>Returns true if host name and either of the IPv4 or IPv6 address exists else returns false</returns>
        private static bool VerifyPingReply(string hostName, string pingReply, Printer.Printer printer)
        {
            // Ping reply should contain hostname and either IPv6 address or IPv4 address depending on the availability of the address on the printer.
            if (pingReply.Contains(hostName, StringComparison.CurrentCultureIgnoreCase))
            {
                TraceFactory.Logger.Info("Ping reply contains the host name: {0}".FormatWith(hostName));

                return (VerifyIpv4InPingReply(printer.WiredIPv4Address.ToString(), pingReply) || VerifyIpv6InPingReply(pingReply, printer));
            }
            else
            {
                TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Ping reply doesn't contain the host name: {0}".FormatWith(hostName));
                return false;
            }
        }

        private static bool VerifyIpv4InPingReply(string printerIpAddress, string pingReply)
        {
            // check for IPv4 address
            if (pingReply.Contains(printerIpAddress, StringComparison.CurrentCultureIgnoreCase))
            {
                TraceFactory.Logger.Info("Ping reply contains the {0} ".FormatWith(printerIpAddress));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Ping reply does not contain the {0} ".FormatWith(printerIpAddress));
                return false;
            }
        }

        private static bool VerifyIpv6InPingReply(string pingReply, Printer.Printer printer)
        {
            // check link local			
            if (!(printer.IPv6LinkLocalAddress == null))
            {
                if (pingReply.Contains(printer.IPv6LinkLocalAddress.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    TraceFactory.Logger.Info("Ping reply contains the {0} ".FormatWith(printer.IPv6LinkLocalAddress.ToString()));
                    return true;
                }
            }

            if (!(printer.IPv6StateFullAddress == null))
            {
                // check with Statefull address
                if (pingReply.Contains(printer.IPv6StateFullAddress.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    TraceFactory.Logger.Info("Ping reply contains the {0} ".FormatWith(printer.IPv6StateFullAddress.ToString()));
                    return true;
                }
            }
            // check with Stateless address
            foreach (IPAddress address in printer.IPv6StatelessAddresses)
            {
                if (pingReply.Contains(address.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    TraceFactory.Logger.Info("Ping reply contains the {0} ".FormatWith(address.ToString()));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Verify DDNS using different UI(Telnet, SNMP)
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>
        /// <param name="accessType"><see cref="PrinterAccessType"/></param>
        /// <param name="enableDdns">True to enable and false to disable DDNS.</param>
        /// <param name="testNo">test Number</param>
        /// <returns>true if DDNS validation is successful, else false.</returns>
        private static bool VerifyDdnsDifferentUI(NetworkNamingServiceActivityData activityData, PrinterAccessType accessType, bool enableDdns, int testNo)
        {
            string hostName = CtcUtility.GetUniqueHostName();

            try
            {
                if (!EwsWrapper.Instance().SetDDNS(false))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetDomainName(GetDomainName(activityData.PrimaryDhcpServerIPAddress)))
                {
                    return false;
                }

                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);

                PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-{1}-DDNS{2}".FormatWith(testNo, accessType, enableDdns ? "enabled" : "disabled"));

                if (accessType == PrinterAccessType.Telnet)
                {
                    if (!TelnetWrapper.Instance().SetDdns((PrinterFamilies)Enum<PrinterFamilies>.Parse(activityData.ProductFamily, true), enableDdns))
                    {
                        return false;
                    }
                }
                else if (accessType == PrinterAccessType.SNMP)
                {
                    if (!SnmpWrapper.Instance().SetDdns(enableDdns))
                    {
                        return false;
                    }
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));

                PacketDetails packetDetails = client.Channel.Stop(guid);

                return ValidateDnsEntry(activityData.PrimaryDhcpServerIPAddress, activityData.WiredIPv4Address, hostName, packetDetails.PacketsLocation, !enableDdns);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                DnsPostrequisites(activityData);
            }
        }

        private static bool VerifyDdnsWithHostNameChange(NetworkNamingServiceActivityData activityData, int testNo)
        {
            string hostName = CtcUtility.GetUniqueHostName();
            string newHostName = string.Empty;

            try
            {
                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetDomainName(GetDomainName(activityData.PrimaryDhcpServerIPAddress)))
                {
                    return false;
                }

                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);

                PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-HostnameChange".FormatWith(testNo));

                if (!EwsWrapper.Instance().SetDDNS(true))
                {
                    return false;
                }

                newHostName = CtcUtility.GetUniqueHostName();

                if (!EwsWrapper.Instance().SetHostname(newHostName))
                {
                    return false;
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));

                PacketDetails packetDetails = client.Channel.Stop(guid);

                return ValidateDnsEntry(activityData.PrimaryDhcpServerIPAddress, activityData.WiredIPv4Address, newHostName, packetDetails.PacketsLocation);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                DnsPostrequisites(activityData, true);
            }
        }

        private static bool VerifyDdnsWithIpAddressChange(NetworkNamingServiceActivityData activityData, int testNo)
        {
            string hostName = CtcUtility.GetUniqueHostName();
            IPAddress manualIpAddress = IPAddress.None;

            try
            {
                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetDomainName(GetDomainName(activityData.PrimaryDhcpServerIPAddress)))
                {
                    return false;
                }

                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);

                PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-IPAddressChange".FormatWith(testNo));

                if (!EwsWrapper.Instance().SetDDNS(true))
                {
                    return false;
                }

                manualIpAddress = NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.WiredIPv4Address));

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, manualIpAddress))
                {
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(manualIpAddress);

                Thread.Sleep(TimeSpan.FromMinutes(1));

                PacketDetails packetDetails = client.Channel.Stop(guid);

                return ValidateDnsEntry(activityData.PrimaryDhcpServerIPAddress, manualIpAddress.ToString(), hostName, packetDetails.PacketsLocation);
            }
            finally
            {
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                DnsPostrequisites(activityData, true);
            }
        }

        private static bool VerifyDdnsWithDomainNameChange(NetworkNamingServiceActivityData activityData, int testNo)
        {
            string hostName = CtcUtility.GetUniqueHostName();

            try
            {
                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetDomainName(GetDomainName(activityData.PrimaryDhcpServerIPAddress)))
                {
                    return false;
                }

                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);
                EwsWrapper.Instance().SetSecondaryDNSServerIP(activityData.SecondDhcpServerIPAddress);

                if (!EwsWrapper.Instance().SetDDNS(true))
                {
                    return false;
                }

                PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.SecondDhcpServerIPAddress);
                string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-DomainnameChange".FormatWith(testNo));

                if (!EwsWrapper.Instance().SetDomainName(GetDomainName(activityData.SecondDhcpServerIPAddress)))
                {
                    return false;
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));

                PacketDetails packetDetails = client.Channel.Stop(guid);

                return ValidateDnsEntry(activityData.SecondDhcpServerIPAddress, activityData.WiredIPv4Address, hostName, packetDetails.PacketsLocation);
            }
            finally
            {
                DnsPostrequisites(activityData);
            }
        }



        private static bool VerifyPrimaryDnsRegistration(NetworkNamingServiceActivityData activityData, int testNo)
        {
            string hostName = string.Empty;
            try
            {
                //DDNS option is disabled in DNSPostrequisites so enabling back
                if (!EwsWrapper.Instance().SetDDNS(true))
                {
                    return false;
                }
                hostName = CtcUtility.GetUniqueHostName();

                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

                PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-PrimaryWinsServer".FormatWith(testNo));


                if (!EwsWrapper.Instance().SetDomainName(GetDomainName(activityData.PrimaryDhcpServerIPAddress)))
                {
                    return false;
                }

                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);

                if (!EwsWrapper.Instance().SetDDNS(true))
                {
                    return false;
                }

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

                printer.PowerCycle();

                if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.WiredIPv4Address), TimeSpan.FromSeconds(10)))
                {
                    TraceFactory.Logger.Info("Printer is not available after power cycle.");
                    return false;
                }

                PacketDetails packetDetails = client.Channel.Stop(guid);

                return ValidateDnsEntry(activityData.PrimaryDhcpServerIPAddress, activityData.WiredIPv4Address, hostName, packetDetails.PacketsLocation);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                DnsPostrequisites(activityData, true);
            }
        }

        private static bool VerifyDnsRegistrationWithDifferentPrimaryDns(NetworkNamingServiceActivityData activityData, int testNo)
        {
            string hostName = string.Empty;
            try
            {
                //DDNS option is disabled in DNSPostrequisites so enabling back
                if (!EwsWrapper.Instance().SetDDNS(true))
                {
                    return false;
                }
                hostName = CtcUtility.GetUniqueHostName();
                string firstClientNetworkName = string.Empty;

                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

                PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.SecondDhcpServerIPAddress);
                string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-DifferentPrimaryDnsServer".FormatWith(testNo));

                if (!EwsWrapper.Instance().SetDomainName(GetDomainName(activityData.SecondDhcpServerIPAddress)))
                {
                    return false;
                }

                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.SecondDhcpServerIPAddress);

                using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);
                    dhcpClient.Channel.SetDnsServer(activityData.PrimaryDhcpServerIPAddress, scope, activityData.SecondDhcpServerIPAddress);
                }

                firstClientNetworkName = GetClientNetworkName(activityData.PrimaryDhcpServerIPAddress);
                //Refreshing the client nic card to update dns server ip
                NetworkUtil.DisableNetworkConnection(firstClientNetworkName);
                NetworkUtil.EnableNetworkConnection(firstClientNetworkName);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                PacketDetails packetDetails = client.Channel.Stop(guid);

                if (!ValidateDnsEntry(activityData.SecondDhcpServerIPAddress, activityData.WiredIPv4Address, hostName, packetDetails.PacketsLocation))
                {
                    return false;
                }

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                string fqdn = "{0}.{1}".FormatWith(hostName, GetDomainName(activityData.SecondDhcpServerIPAddress));

                return printer.IsEwsAccessible(fqdn, "https");
            }
            finally
            {
                using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);
                    dhcpClient.Channel.SetDnsServer(activityData.PrimaryDhcpServerIPAddress, scope, activityData.PrimaryDhcpServerIPAddress);
                }
                DnsPostrequisites(activityData, true);
            }
        }

        private static bool VerifySecondaryDnsRegistration(NetworkNamingServiceActivityData activityData, int testNo)
        {
            string hostName = string.Empty;
            try
            {
                hostName = CtcUtility.GetUniqueHostName();

                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetDomainName(GetDomainName(activityData.SecondDhcpServerIPAddress)))
                {
                    return false;
                }

                EwsWrapper.Instance().SetSecondaryDNSServerIP(activityData.SecondDhcpServerIPAddress);

                PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.SecondDhcpServerIPAddress);
                string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-SecondaryDnsServer".FormatWith(testNo));

                if (!EwsWrapper.Instance().SetDDNS(true))
                {
                    return false;
                }

                Thread.Sleep(TimeSpan.FromSeconds(30));

                PacketDetails packetDetails = client.Channel.Stop(guid);

                if (!ValidateDnsEntry(activityData.SecondDhcpServerIPAddress, activityData.WiredIPv4Address, hostName, packetDetails.PacketsLocation))
                {
                    return false;
                }

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                string fqdn = "{0}.{1}".FormatWith(hostName, GetDomainName(activityData.SecondDhcpServerIPAddress));

                return printer.IsEwsAccessible(fqdn, "https");
            }
            finally
            {
                DnsPostrequisites(activityData, true);
            }
        }

        /// <summary>
        /// Validates DNS entry from the server and from the wireshark traces.
        /// </summary>
        /// <param name="serverIpAddress">The server IP address.</param>
        /// <param name="printerIpAddress">The printer IP address.</param>
        /// <param name="hostName">The Host name of the printer.</param>
        /// <param name="packetLocation">The packet location.</param>
        /// <returns>True if the DNS validation is successful from the packets as well as from the DNS server.</returns>
        private static bool ValidateDnsEntry(string serverIpAddress, string printerIpAddress, string hostName, string packetLocation, bool validateNegative = false)
        {
            bool result = true;

            // Validate entry in the DNS server
            using (DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(serverIpAddress))
            {
                if (dnsClient.Channel.ValidateRecord(serverIpAddress, GetDomainName(serverIpAddress), DnsRecordType.A, hostName, printerIpAddress))
                {
                    TraceFactory.Logger.Info("A record was added automatically to the domain: {0}.".FormatWith(GetDomainName(serverIpAddress)));
                }
                else
                {
                    TraceFactory.Logger.Info("A record is not added automatically to the domain: {0}.".FormatWith(GetDomainName(serverIpAddress)));

                    if (validateNegative) { result &= false; } else { return false; }
                }

                if (dnsClient.Channel.ValidateRecord(serverIpAddress, GetDomainName(serverIpAddress), DnsRecordType.PTR, hostName, printerIpAddress))
                {
                    TraceFactory.Logger.Info("PTR record was added automatically to the domain: {0}.".FormatWith(GetDomainName(serverIpAddress)));
                }
                else
                {
                    TraceFactory.Logger.Info("PTR record is not added automatically to the domain: {0}.".FormatWith(GetDomainName(serverIpAddress)));
                    if (validateNegative) { result &= false; } else { return false; }
                }
            }

            // Packet Validation for DNS
            using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(serverIpAddress))
            {
                string aQueryFilter = "dns.resp.type == 1 && udp.port ==53 && ip.addr == {0} && ip.src == {0} && ip.dst == {1}".FormatWith(printerIpAddress, serverIpAddress);
                string aResponseFilter = "dns.resp.type == 1 && udp.port ==53 && ip.addr == {0} && ip.src == {1} && ip.dst == {0}".FormatWith(printerIpAddress, serverIpAddress);
                string ptrQueryFilter = "dns.resp.type == 12 && udp.port ==53 && ip.addr == {0} && ip.src == {0} && ip.dst == {1}".FormatWith(printerIpAddress, serverIpAddress);
                string ptrResponseFilter = "dns.resp.type == 12 && udp.port ==53 && ip.addr == {0} && ip.src == {1} && ip.dst == {0}".FormatWith(printerIpAddress, serverIpAddress);

                string fqdn = "{0}.{1}".FormatWith(hostName, GetDomainName(serverIpAddress));
                string errorMessage = string.Empty;

                if (client.Channel.Validate(packetLocation, aQueryFilter, ref errorMessage, fqdn))
                {
                    TraceFactory.Logger.Info("A query is present in the wireshark traces.");
                }
                else
                {
                    TraceFactory.Logger.Info("A query is not present in the wireshark traces.");
                    if (validateNegative) { result &= false; } else { return false; }
                }

                if (client.Channel.Validate(packetLocation, aResponseFilter, ref errorMessage, fqdn))
                {
                    TraceFactory.Logger.Info("Response for A query is present in the wireshark traces.");

                    if (client.Channel.GetPacketData(packetLocation, aResponseFilter, "Addr").Contains(printerIpAddress))
                    {
                        TraceFactory.Logger.Info("Response for A query contains the IP address: {0} in the wireshark traces.".FormatWith(printerIpAddress));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Response for A query does not contain the IP address: {0} in the wireshark traces.".FormatWith(printerIpAddress));
                        if (validateNegative) { result &= false; } else { return false; }
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("Response for A query is not present in the wireshark traces.");
                    if (validateNegative) { result &= false; } else { return false; }
                }

                // For PTR query request will be using the "<reverse IP address>.in-addr.arpa" and response is with the "<hostname>.<domainname>"

                string[] ipBits = printerIpAddress.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                string ptrRequest = PTR_QUERY.FormatWith("{0}.{1}.{2}.{3}".FormatWith(ipBits[3], ipBits[2], ipBits[1], ipBits[0]));

                if (client.Channel.Validate(packetLocation, ptrQueryFilter, ref errorMessage, ptrRequest))
                {
                    TraceFactory.Logger.Info("PTR query is present in the wireshark traces.");
                }
                else
                {
                    TraceFactory.Logger.Info("PTR query is not present in the wireshark traces.");
                    if (validateNegative) { result &= false; } else { return false; }
                }

                if (client.Channel.Validate(packetLocation, ptrResponseFilter, ref errorMessage, ptrRequest))
                {
                    TraceFactory.Logger.Info("Response for PTR query is present in the wireshark traces.");

                    if (client.Channel.GetPacketData(packetLocation, ptrResponseFilter, "Domain name").Contains(fqdn))
                    {
                        TraceFactory.Logger.Info("Response for PTR query contains the host name: {0} in the wireshark traces.".FormatWith(fqdn));
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Response for PTR query does not contain the host name: {0} in the wireshark traces.".FormatWith(fqdn));
                        if (validateNegative) { result &= false; } else { return false; }
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("Response for PTR query is not present in the wireshark traces.");
                    if (validateNegative) { result &= false; } else { return false; }
                }
            }

            return result;
        }

        /// <summary>
        /// Verifies the A query with LLMNR enabled and disabled states
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>
        /// <param name="testNo">The test number.</param>
        /// <param name="enableLlmnr">True to enable LLMNR, false to disable.</param>
        /// <returns>True for success, else false.</returns>
        private static bool VerifyQueryWithLLMNR(NetworkNamingServiceActivityData activityData, int testNo, bool enableLlmnr, DnsRecordType recordType)
        {
            string hostName = string.Empty;
            try
            {
                string fileName = "{0}-{1}-LLMNR{2}".FormatWith(testNo, recordType, enableLlmnr ? "enabled" : "disabled");

                if (!EwsWrapper.Instance().SetLlmnr(enableLlmnr))
                {
                    return false;
                }

                EwsWrapper.Instance().SetIPv4(true, printerIpv4Address: activityData.WiredIPv4Address);

                if (!EwsWrapper.Instance().SetIPv6(true))
                {
                    return false;
                }

                hostName = CtcUtility.GetUniqueHostName();

                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

                string command = "ping -{0} {1}".FormatWith(recordType == DnsRecordType.A ? "4" : (recordType == DnsRecordType.AAAA ? "6" : "a"), recordType == DnsRecordType.PTR ? activityData.WiredIPv4Address : hostName);

                string pingReply = string.Empty;

                string packetLocation = StartCapture(GetClientNetworkId(activityData.WiredIPv4Address), command, fileName, out pingReply);
                TraceFactory.Logger.Info("Saving Packets at location : {0}".FormatWith(packetLocation));
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

                if (recordType == DnsRecordType.A)
                {
                    if (!(VerifyIpv4InPingReply(activityData.WiredIPv4Address, pingReply) && enableLlmnr))
                    {
                        return false;
                    }
                }
                else if (recordType == DnsRecordType.AAAA)
                {
                    if (!(VerifyIpv6InPingReply(pingReply, printer) && enableLlmnr))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!(VerifyPingReply(hostName, pingReply, printer) && enableLlmnr))
                    {
                        return false;
                    }
                }

                return ValidateLlmnrPackets(activityData, packetLocation, hostName, recordType);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                DnsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// Verifies the AAAA query with Multicast IPv4 enabled and disabled states
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>
        /// <param name="testNo">The test number.</param>
        /// <param name="enableLlmnr">True to enable Multicast IPv4, false to disable.</param>
        /// <returns>True for success, else false.</returns>
        private static bool VerifyQueryWithMulticastIpv4(NetworkNamingServiceActivityData activityData, int testNo, bool enableMulticastIpv4, DnsRecordType recordType)
        {
            string fileName = "{0}-{1}-LLMNR{2}".FormatWith(testNo, recordType, enableMulticastIpv4 ? "enabled" : "disabled");

            try
            {
                if (!EwsWrapper.Instance().SetLlmnr(true))
                {
                    return false;
                }

                EwsWrapper.Instance().SetMulticastIPv4(enableMulticastIpv4);

                string hostName = CtcUtility.GetUniqueHostName();

                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

                string command = "ping -{0} {1}".FormatWith(recordType == DnsRecordType.A ? "4" : (recordType == DnsRecordType.AAAA ? "6" : "a"), recordType == DnsRecordType.PTR ? activityData.WiredIPv4Address : hostName);

                string pingReply = string.Empty;

                string packetLocation = StartCapture(GetClientNetworkId(activityData.WiredIPv4Address), command, fileName, out pingReply);

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

                if (recordType == DnsRecordType.A)
                {
                    if (!VerifyIpv4InPingReply(activityData.WiredIPv4Address, pingReply))
                    {
                        return false;
                    }
                }
                else if (recordType == DnsRecordType.AAAA)
                {
                    if (!VerifyIpv6InPingReply(pingReply, printer))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!VerifyPingReply(hostName, pingReply, printer))
                    {
                        return false;
                    }
                }

                return ValidateLlmnrPackets(activityData, packetLocation, hostName, recordType);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                DnsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// Verifies the AAAA query with IPv4 enabled and disabled states
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>
        /// <param name="testNo">The test number.</param>
        /// <param name="enableLlmnr">True to enable IPv4, false to disable.</param>
        /// <returns>True for success, else false.</returns>
        private static bool VerifyQueryWithIpv4(NetworkNamingServiceActivityData activityData, int testNo, bool enableIpv4, DnsRecordType recordType)
        {
            string pingReply = string.Empty;
            string printerIP = activityData.WiredIPv4Address;
            try
            {
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

                string printerIpv6Address = string.Empty;
                string hostName = EwsWrapper.Instance().GetHostname();

                string networkName = GetClientNetworkId(activityData.WiredIPv4Address);
                string fileName = "{0}-{1}-LLMNR{1}".FormatWith(testNo, recordType, enableIpv4 ? "enabled" : "disabled");

                if (!enableIpv4)
                {
                    printerIpv6Address = printer.IPv6LinkLocalAddress.ToString();
                }

                if (!EwsWrapper.Instance().SetLlmnr(true))
                {
                    return false;
                }

                EwsWrapper.Instance().SetIPv4(enableIpv4, printerIpv4Address: activityData.WiredIPv4Address);

                if (!enableIpv4)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(printerIpv6Address);
                    activityData.WiredIPv4Address = printerIpv6Address;
                }

                string command = "ping -{0} {1}".FormatWith(recordType == DnsRecordType.A ? "4" : (recordType == DnsRecordType.AAAA ? "6" : "a"), recordType == DnsRecordType.PTR ? activityData.WiredIPv4Address : hostName);
                string packetLocation = StartCapture(networkName, command, fileName, out pingReply);

                if (recordType == DnsRecordType.A)
                {
                    if (!(VerifyIpv4InPingReply(activityData.WiredIPv4Address, pingReply) && enableIpv4))
                    {
                        return false;
                    }
                }
                else if (recordType == DnsRecordType.AAAA)
                {
                    if (!(VerifyIpv6InPingReply(pingReply, printer) && enableIpv4))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!(VerifyPingReply(hostName, pingReply, printer) && enableIpv4))
                    {
                        return false;
                    }
                }

                return ValidateLlmnrPackets(activityData, packetLocation, hostName, recordType);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                if (!enableIpv4)
                {
                    EwsWrapper.Instance().SetIPv4(true, printerIpv4Address: activityData.WiredIPv4Address);
                    EwsWrapper.Instance().ChangeDeviceAddress(printerIP);
                    activityData.WiredIPv4Address = printerIP;
                    if (!(NetworkUtil.PingUntilTimeout(IPAddress.Parse(printerIP), TimeSpan.FromSeconds(20))))
                    {
                        TraceFactory.Logger.Info("Printer is not accesible after enabling ipv4 option: {0}".FormatWith(printerIP));
                    }
                }
            }
        }

        /// <summary>
        /// Verifies the AAAA query with IPv6 enabled and disabled states
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>
        /// <param name="testNo">The test number.</param>
        /// <param name="enableLlmnr">True to enable IPv6, false to disable.</param>
        /// <returns>True for success, else false.</returns>
        private static bool VerifyQueryWithIpv6(NetworkNamingServiceActivityData activityData, int testNo, bool enableIpv6, DnsRecordType recordType)
        {
            string fileName = "{0}-{1}-LLMNR{2}".FormatWith(testNo, recordType, enableIpv6 ? "enabled" : "disabled");
            try
            {

                if (!EwsWrapper.Instance().SetLlmnr(true))
                {
                    return false;
                }

                EwsWrapper.Instance().SetIPv6(enableIpv6);

                string hostName = EwsWrapper.Instance().GetHostname();

                string command = "ping -{0} {1}".FormatWith(recordType == DnsRecordType.A ? "4" : (recordType == DnsRecordType.AAAA ? "6" : "a"), recordType == DnsRecordType.PTR ? activityData.WiredIPv4Address : hostName);

                string pingReply = string.Empty;

                string packetLocation = StartCapture(GetClientNetworkId(activityData.WiredIPv4Address), command, fileName, out pingReply);

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

                if (recordType == DnsRecordType.A)
                {
                    if (!(VerifyIpv4InPingReply(activityData.WiredIPv4Address, pingReply) && enableIpv6))
                    {
                        return false;
                    }
                }
                else if (recordType == DnsRecordType.AAAA)
                {
                    if (!(VerifyIpv6InPingReply(pingReply, printer) && enableIpv6))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!(VerifyPingReply(hostName, pingReply, printer) && enableIpv6))
                    {
                        return false;
                    }
                }

                return ValidateLlmnrPackets(activityData, packetLocation, hostName, recordType);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                DnsPostrequisites(activityData);
                EwsWrapper.Instance().SetIPv6(true);
            }
        }

        /// <summary>
        /// Validates the DNS packets for A query
        /// </summary>
        /// <param name="packetsLocation">The packet location</param>
        /// <param name="serverAddress">IP Address of the DHCP server.</param>
        /// <param name="printerIpAddress">IP Address of the printer.</param>
        /// <param name="hostName">Host name of the printer.</param>
        /// <param name="responseAddresses">Expected addresses in the response of wireshark trace.</param>
        /// <returns>True if the packet validation is successful.</returns>
        private static bool ValidateDnsPackets(string packetsLocation, string serverAddress, string hostName, string printerIpAddress, bool isIpv4, Collection<IPAddress> responseAddresses = null)
        {
            TraceFactory.Logger.Info(CtcBaseTests.PACKET_VALIDATION);
            string fqdn = "{0}.{1}".FormatWith(hostName, EwsWrapper.Instance().GetDomainName());

            if (null == responseAddresses)
            {
                responseAddresses = new Collection<IPAddress>();
            }

            responseAddresses.Add(IPAddress.Parse(printerIpAddress));

            using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(serverAddress))
            {
                string aQueryFilter = "dns.qry.type == 1 && dns.qry.name == \\\"{0}\\\" && udp.port == 53 && ip.addr == {1} && ip.src == {1}".FormatWith(fqdn, printerIpAddress);
                string aaaaQueryFilter = "dns.qry.type == 28 && dns.qry.name == \\\"{0}\\\" && udp.port == 53 && ip.addr == {1} && ip.src == {1}".FormatWith(fqdn, printerIpAddress);
                string aResponseFilter = "dns.qry.type == 1 && dns.qry.name == \\\"{0}\\\" && udp.port == 53 && ip.addr == {1} && ip.src == {2}".FormatWith(fqdn, printerIpAddress, serverAddress);
                string aaaaResponseFilter = "dns.qry.type == 28 && dns.qry.name == \\\"{0}\\\" && udp.port == 53 && ip.addr == {1} && ip.src == {2}".FormatWith(fqdn, printerIpAddress, serverAddress);

                string errorMessage = string.Empty;

                if (isIpv4)
                {
                    if (client.Channel.Validate(packetsLocation, aQueryFilter, ref errorMessage, fqdn))
                    {
                        TraceFactory.Logger.Info("A query is present in the wireshark traces.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("A query is not present in the wireshark traces.");
                        return false;
                    }

                    TraceFactory.Logger.Info(errorMessage);

                    if (client.Channel.Validate(packetsLocation, aResponseFilter, ref errorMessage, fqdn))
                    {
                        TraceFactory.Logger.Info("Response for A query is present in the wireshark traces.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Response for A query is not present in the wireshark traces.");
                        return false;
                    }
                }
                else
                {
                    if (client.Channel.Validate(packetsLocation, aaaaQueryFilter, ref errorMessage, fqdn))
                    {
                        TraceFactory.Logger.Info("AAAA query is present in the wireshark traces.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("AAAA query is not present in the wireshark traces.");
                        return false;
                    }

                    TraceFactory.Logger.Info(errorMessage);

                    if (client.Channel.Validate(packetsLocation, aaaaResponseFilter, ref errorMessage, fqdn))
                    {
                        TraceFactory.Logger.Info("Response for AAAA query is present in the wireshark traces.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Response for AAAA query is not present in the wireshark traces.");
                        return false;
                    }
                }
            }

            return true;
        }

        private static string GetDomainName(string serverIP)
        {
            string[] ipBits = serverIP.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            return SERVER_DOMAIN.FormatWith(ipBits[2]);
        }

        /// <summary>
        /// Verify DNS resolution using NTS server.
        /// </summary>
        /// <param name="activityData">Activity Data</param>
        /// <param name="serverAddress">IP Address of DNS server.</param>
        /// <param name="printerIp">IP Address of the printer.</param>
        /// <param name="testNo">Test No.</param>
        /// <returns>true if the resolution is successful, else false.</returns>
        private static bool VerifyNtsNameResolution(NetworkNamingServiceActivityData activityData, string serverAddress, string printerIp, int testNo, bool isIpv4, int addressCount = 1)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
            string ntsHostName = string.Empty;
            TraceFactory.Logger.Info("Setting Count for DNS entries to 3");
            addressCount = 3;
            try
            {
                string domainName = GetDomainName(serverAddress);
                ntsHostName = CtcUtility.GetUniqueHostName();

                if (!EwsWrapper.Instance().SetDomainName(domainName))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetPrimaryDnsServer(serverAddress))
                {
                    return false;
                }

                // Generate random IPv4 & IPv6 addresses
                Collection<IPAddress> addresses = new Collection<IPAddress>();
                IPAddress address = NetworkUtil.FetchNextIPAddress(IPAddress.Parse(printerIp).GetSubnetMask(), IPAddress.Parse(printerIp));

                for (int i = 0; i < addressCount; i++)
                {
                    addresses.Add(address);
                    address = NetworkUtil.FetchNextIPAddress(address.GetSubnetMask(), address);
                }

                for (int i = 0; i < addressCount; i++)
                {
                    string hexString = "ABCDEF0123456789";
                    Random random = new Random();
                    string result = new string(
                        Enumerable.Repeat(hexString, 16)
                            .Select(s => s[random.Next(s.Length)])
                            .ToArray());

                    result = Regex.Replace(result, ".{2}", "$0:");
                    address = IPAddress.Parse(result.Remove(result.LastIndexOf(":", StringComparison.CurrentCultureIgnoreCase)));
                    addresses.Add(address);
                }

                using (DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(serverAddress))
                {
                    // Create NTS server entries in DNS server with some dummy IPv4 and IPv6 addresses.
                    foreach (IPAddress ip in addresses)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            if (isIpv4)
                            {
                                if (dnsClient.Channel.AddRecord(GetDomainName(serverAddress), ntsHostName, "A", ip.ToString()))
                                {
                                    TraceFactory.Logger.Info("Successfully added 'A' record for the host name: {0} and IP Address: {1}".FormatWith(ntsHostName, ip.ToString()));
                                }
                                else
                                {
                                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to add 'A' record for the host name: {0} and IP Address: {1}".FormatWith(ntsHostName, ip.ToString()));
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            if (!isIpv4)
                            {
                                if (dnsClient.Channel.AddRecord(GetDomainName(serverAddress), ntsHostName, "AAAA", ip.ToString()))
                                {
                                    TraceFactory.Logger.Info("Successfully added 'AAAA' record for the host name: {0} and IP Address: {1}".FormatWith(ntsHostName, ip.ToString()));
                                }
                                else
                                {
                                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to add 'AAAA' record for the host name: {0} and IP Address: {1}".FormatWith(ntsHostName, ip.ToString()));
                                    return false;
                                }
                            }
                        }
                    }
                }

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(serverAddress))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString(CultureInfo.CurrentCulture));

                    EwsWrapper.Instance().SetNtsAddress(ntsHostName);
                    TraceFactory.Logger.Debug("Successfully configured NTS hostname");
                    PacketDetails packetDetails = client.Channel.Stop(guid);
                    TraceFactory.Logger.Debug("Stopped capturing Packets");
                    return ValidateDnsPackets(packetDetails.PacketsLocation, serverAddress, ntsHostName, printerIp, isIpv4, addresses);
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                DnsPostrequisites(activityData);
            }
        }

        /// <summary>
        /// Validates LLMNR packets for A, AAAA queries
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>
        /// <param name="packetsPath">Packets location.</param>
        /// <param name="serverAddress">IP address of the server.</param>
        /// <param name="hostName">Host name of the printer.</param>
        /// <param name="recordType"><see cref="DnsRecordType"/></param>
        /// <returns>True if the validation is successful, else false.</returns>
        private static bool ValidateLlmnrPackets(NetworkNamingServiceActivityData activityData, string packetsPath, string hostName, DnsRecordType recordType)
        {
            TraceFactory.Logger.Info(CtcBaseTests.PACKET_VALIDATION);
            string errorMessage = string.Empty;
            bool result = true;

            string localMachineAddress = NetworkUtil.GetLocalAddress(activityData.PrimaryDhcpServerIPAddress, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();
            IPAddress localMachineLinkLocalAddress = IPAddress.None;

            // Keeping client and server in different subnet
            foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
            {
                UnicastIPAddressInformationCollection addresses = item.GetIPProperties().UnicastAddresses;

                if (addresses.Any(x => x.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && x.Address.IsInSameSubnet(IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress), IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask())))
                {
                    localMachineLinkLocalAddress = addresses.Where(x => x.Address.IsIPv6LinkLocal).FirstOrDefault().Address;
                    break;
                }
            }

            // Link local IPv6 address may contain the scope id usually followed by %. So removing the scope id from the link local ipv6 address
            string localLinkLocalIPv6Address = localMachineLinkLocalAddress.ToString().Remove(localMachineLinkLocalAddress.ToString().IndexOf("%", StringComparison.CurrentCultureIgnoreCase));

            string queryFilter = string.Empty;
            string responseFilter = string.Empty;
            string ptrRequest = string.Empty;

            if (recordType == DnsRecordType.A)
            {
                queryFilter = "dns.qry.name == \"{0}\" && dns.qry.type == 1 && dns.flags == 0x0000".FormatWith(hostName);
                responseFilter = "dns.qry.name == \"{0}\" && dns.qry.type == 1 && dns.flags == 0x8000".FormatWith(hostName);
            }
            else if (recordType == DnsRecordType.AAAA)
            {
                queryFilter = "dns.qry.name == \"{0}\" && dns.qry.type == 28 && dns.flags == 0x0000".FormatWith(hostName);
                responseFilter = "dns.qry.name == \"{0}\" && dns.qry.type == 28 && dns.flags == 0x8000".FormatWith(hostName);
            }
            else
            {
                // For PTR query request will be using the "<reverse IP address>.in-addr.arpa" and response is with the "<hostname>.<domainname>"
                string[] ipBits = activityData.WiredIPv4Address.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                ptrRequest = PTR_QUERY.FormatWith("{0}.{1}.{2}.{3}".FormatWith(ipBits[3], ipBits[2], ipBits[1], ipBits[0]));

                queryFilter = "dns.qry.name == \"{0}\" && dns.flags == 0x0000".FormatWith(ptrRequest);
                responseFilter = "dns.qry.name == \"{0}\" && dns.flags == 0x8000".FormatWith(ptrRequest);
            }

            if (Validate(packetsPath, queryFilter, localMachineAddress, localLinkLocalIPv6Address.ToString()))
            {
                TraceFactory.Logger.Info("{0} queries with IP addresses: {1} and {2} are present in the wireshark traces.".FormatWith(recordType, localMachineAddress, localLinkLocalIPv6Address));
            }
            else
            {
                TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "{0} queries with IP addresses: {1} and {2} are not present in the wireshark traces.".FormatWith(recordType, localMachineAddress, localLinkLocalIPv6Address));
                result &= false;
            }

            if (Validate(packetsPath, responseFilter, "standard query response"))
            {
                TraceFactory.Logger.Info("{0} response is present in the wireshark traces.".FormatWith(recordType));

                string responseAddresses = GetPacketData(packetsPath, responseFilter, "Addr");

                if (recordType == DnsRecordType.AAAA)
                {
                    Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

                    Collection<IPAddress> ipv6Addresses = new Collection<IPAddress>();
                    ipv6Addresses.Add(printer.IPv6LinkLocalAddress);
                    ipv6Addresses.Add(printer.IPv6StateFullAddress);

                    ipv6Addresses = new Collection<IPAddress>(ipv6Addresses.Union(printer.IPv6StatelessAddresses).ToArray());

                    foreach (var item in ipv6Addresses)
                    {
                        if (responseAddresses.Contains(item.ToString()))
                        {
                            TraceFactory.Logger.Info("{0} response contains the ip address: {1}.".FormatWith(recordType, item.ToString()));
                            result = true;
                            return result;
                        }
                        else
                        {
                            TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "{0} response doesn't contain the ip address: {0}.".FormatWith(recordType, item.ToString()));
                            result &= false;
                        }
                    }
                }
                else if (recordType == DnsRecordType.A)
                {
                    if (responseAddresses.Contains(activityData.WiredIPv4Address))
                    {
                        TraceFactory.Logger.Info("A response contains the ip address: {0}.".FormatWith(activityData.WiredIPv4Address));
                    }
                    else
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "A response doesn't contain the ip address: {0}.".FormatWith(activityData.WiredIPv4Address));
                        result &= false;
                    }
                }
                else
                {
                    if (GetPacketData(packetsPath, responseFilter, "Name:").Contains(ptrRequest))
                    {
                        TraceFactory.Logger.Info("PTR response contains the IP address: {0}.".FormatWith(ptrRequest));
                    }
                    else
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "PTR response doesnot contain the IP address: {0}.".FormatWith(activityData.WiredIPv4Address));
                        result &= false;
                    }
                }
            }
            else
            {
                TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "AAAA response is not present in the wireshark traces.");
                result &= false;
            }

            return result;
        }

        /// <summary>
        /// Validates LLMNR packets for A, AAAA queries
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>
        /// <param name="packetsPath">Packets location.</param>
        /// <param name="serverAddress">IP address of the server.</param>
        /// <param name="hostName">Host name of the printer.</param>
        /// <param name="recordType"><see cref="DnsRecordType"/></param>
        /// <returns>True if the validation is successful, else false.</returns>
        private static bool ValidateLLMNRHostNameUniquenessPackets(NetworkNamingServiceActivityData activityData, string packetsPath, string hostName, DnsRecordType recordType, string printerIPAddress)
        {
            bool result = true;
            string queryFilter = string.Empty;

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, printerIPAddress);
            string localLinkLocalIPv6Address = printer.IPv6LinkLocalAddress.ToString();

            if (recordType == DnsRecordType.A)
            {
                queryFilter = "dns.qry.name == \"{0}\" && dns.qry.type == 1 && dns.flags == 0x0000".FormatWith(hostName);
                if (Validate(packetsPath, queryFilter, printerIPAddress))
                {
                    TraceFactory.Logger.Info("{0} queries with IP address: {1} and hostname:{2} are present in the wireshark traces.".FormatWith(recordType, printerIPAddress, hostName));
                }
                else
                {
                    TraceFactory.Logger.Info("{0} queries with IP address: {1} and hostname:{2} are not present in the wireshark traces.".FormatWith(recordType, printerIPAddress, hostName));
                    result &= false;
                }
            }
            else
            {
                queryFilter = "dns.qry.name == \"{0}\" && dns.qry.type == 28 && dns.flags == 0x0000".FormatWith(hostName);
                if (Validate(packetsPath, queryFilter, localLinkLocalIPv6Address))
                {
                    TraceFactory.Logger.Info("{0} queries with IP address: {1} and hostname:{2} are present in the wireshark traces.".FormatWith(recordType, localLinkLocalIPv6Address, hostName));
                }
                else
                {
                    TraceFactory.Logger.Info("{0} queries with IP address: {1} and hostname:{2} are not present in the wireshark traces.".FormatWith(recordType, localLinkLocalIPv6Address, hostName));
                    result &= false;
                }
            }
            return result;
        }

        /// <summary>
        /// Validate the LLMNR Uniqueness packets
        /// </summary>
        /// <param name="networkName">Name of the network winterface where to capture the packets.</param>
        /// <param name="command">The ping command to be executed.</param>
        /// <param name="fileName">Name of the pcap file.</param>
        /// <param name="pingReply">Reply of the ping command.</param>
        /// <returns>The packet location.</returns>
        private static string ValidateLLMNRUniquenessPackets(NetworkNamingServiceActivityData activityData, string networkName, string command, string fileName, string hostName, string manualIPAddress = null)
        {
            if (!Directory.Exists(PACKETS_LOCATION))
            {
                Directory.CreateDirectory(PACKETS_LOCATION);
            }

            string packetsPath = Path.Combine(PACKETS_LOCATION, "{0}.pcap".FormatWith(fileName));

            //Before start pinging the ip, flushing all dns records so that the test case should not refer previously configured hostname
            if (!(ProcessUtil.Execute("cmd.exe", "/C {0}".FormatWith("ipconfig /flushdns"))).StandardOutput.Contains("Success"))
            {
                TraceFactory.Logger.Info("Failed to flush the dns entries");
            }

            if (File.Exists(packetsPath))
            {
                File.Delete(packetsPath);
            }

            using (Process captureProcess = new Process
            {
                StartInfo = { FileName = "cmd.exe", Arguments = "/C dumpcap -i \"\\Device\\NPF_{0}\" -w {1}".FormatWith(networkName, packetsPath), CreateNoWindow = true, UseShellExecute = false },
                EnableRaisingEvents = true
            })
            {
                captureProcess.Start();
                if (command == "HostNameChange")
                {
                    TraceFactory.Logger.Info("Setting Printer hostname to already existing hostname on the network");
                    EwsWrapper.Instance().SetHostname(hostName);
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                }
                else if (command == "PowerCycle")
                {
                    TraceFactory.Logger.Info("Power Cycle the Printer");
                    Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                    printer.PowerCycle();
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                }
                else if (command == "IPAddressChange")
                {
                    TraceFactory.Logger.Info("Changing the IPaddress of the printer to Manual");
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, IPAddress.Parse(manualIPAddress));
                    EwsWrapper.Instance().ChangeDeviceAddress(manualIPAddress);

                    Thread.Sleep(TimeSpan.FromMinutes(2));
                }
                else if (command == "ReenableLLMNR")
                {
                    TraceFactory.Logger.Info("Disabling and Enabling back the LLMNR option on printer");
                    EwsWrapper.Instance().SetLlmnr(false);
                    EwsWrapper.Instance().SetLlmnr(true);
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                }

                if (!captureProcess.HasExited)
                    captureProcess.Kill();

                // Kill dumpcap process if it is still running
                ProcessUtil.KillProcess("dumpcap");
            }

            return packetsPath;
        }

        /// <summary>
        /// Starts the packet capture asynchronously with the ping command.
        /// </summary>
        /// <param name="networkName">Name of the network winterface where to capture the packets.</param>
        /// <param name="command">The ping command to be executed.</param>
        /// <param name="fileName">Name of the pcap file.</param>
        /// <param name="pingReply">Reply of the ping command.</param>
        /// <returns>The packet location.</returns>
        private static string StartCapture(string networkName, string command, string fileName, out string pingReply)
        {
            if (!Directory.Exists(PACKETS_LOCATION))
            {
                Directory.CreateDirectory(PACKETS_LOCATION);
            }

            string packetsPath = Path.Combine(PACKETS_LOCATION, "{0}.pcap".FormatWith(fileName));
            NetworkCredential domainAdminCredential = new NetworkCredential(CtcSettings.Domain, CtcSettings.GetSetting("DomainAdminUserName"), CtcSettings.GetSetting("DomainAdminPassword"));

            //Before start pinging the ip, flushing all dns records so that the test case should not refer previously configured hostname
            if (!(ProcessUtil.Execute("cmd.exe", "/C {0}".FormatWith("ipconfig /flushdns"), domainAdminCredential, new TimeSpan(0, 10, 0))).StandardOutput.Contains("Success"))
            {
                TraceFactory.Logger.Info("Failed to flush the dns entries");
            }

            if (File.Exists(packetsPath))
            {
                File.Delete(packetsPath);
            }

            using (Process captureProcess = new Process
            {
                StartInfo = { FileName = "cmd.exe", Arguments = "/C dumpcap -i \"\\Device\\NPF_{0}\" -w {1}".FormatWith(networkName, packetsPath), CreateNoWindow = true, UseShellExecute = false },
                EnableRaisingEvents = true
            })
            {
                captureProcess.Start();

                pingReply = ProcessUtil.Execute("cmd.exe", "/C {0}".FormatWith(command), domainAdminCredential, new TimeSpan(0, 10, 0)).StandardOutput;

                if (!captureProcess.HasExited)
                    captureProcess.Kill();

                // Kill dumpcap process if it is still running
                ProcessUtil.KillProcess("dumpcap");
            }

            return packetsPath;
        }

        /// <summary>
        /// Returns the client network name provided by a particular server.
        /// </summary>
        /// <param name="serverIpAddress">IP Address of the server</param>
        /// <returns>The client network name.</returns>
        private static string GetClientNetworkName(string serverIpAddress)
        {
            foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    // Get the NIC having IP address in the server IP range so that the NIC can be disabled.
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && ip.Address.IsInSameSubnet(IPAddress.Parse(serverIpAddress), IPAddress.Parse(serverIpAddress).GetSubnetMask()))
                    {
                        return item.Name;
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns the client network Id corresponding to a particular server.
        /// </summary>
        /// <param name="serverIpAddress">IP Address of the server</param>
        /// <returns>The client network name.</returns>
        private static string GetClientNetworkId(string serverIpAddress)
        {
            foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    // Get the NIC having IP address in the server IP range so that the NIC can be disabled.
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && ip.Address.IsInSameSubnet(IPAddress.Parse(serverIpAddress), IPAddress.Parse(serverIpAddress).GetSubnetMask()))
                    {
                        return item.Id;
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns the client IP Address corresponding to a particular server.
        /// </summary>
        /// <param name="serverIpAddress">IP Address of the server</param>
        /// <returns>The client IP address.</returns>
        private static IPAddress GetClientIpAddress(string serverIpAddress, AddressFamily addressFamily,
            bool isLinkLocal = false)
        {
            foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
            {
                UnicastIPAddressInformationCollection addresses = item.GetIPProperties().UnicastAddresses;

                if (
                    addresses.Any(x => x.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && x.Address.IsInSameSubnet(IPAddress.Parse(serverIpAddress),
                                IPAddress.Parse(serverIpAddress).GetSubnetMask())))
                {
                    return
                        addresses.Where(x => x.Address.AddressFamily == addressFamily && (isLinkLocal ? x.Address.IsIPv6LinkLocal : true)).FirstOrDefault().Address;
                }
            }
            return IPAddress.None;
        }

        /// <summary>
        /// Validate A, AAAA queries
        /// </summary>
        /// <param name="packetsPath">The packets location.</param>
        /// <param name="serverAddress">IP address of the DHCP server.</param>
        /// <param name="recordType"><see cref="DnsRecordType"/></param>
        /// <param name="fqdn">The FQDN</param>
        /// <returns>True if the validation is successful, else false.</returns>
        private static bool ValidateQuery(string packetsPath, string serverAddress, DnsRecordType recordType, string fqdn)
        {
            TraceFactory.Logger.Info(CtcBaseTests.PACKET_VALIDATION);
            string queryFilter = "udp.port == 53 && dns.flags == 0x0100 && dns.qry.type == {0} && dns.qry.name == \"{1}\"";

            using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(serverAddress))
            {
                string errorMessage = string.Empty;

                foreach (DnsRecordType item in Enum.GetValues(typeof(DnsRecordType)))
                {
                    if (!recordType.HasFlag(item))
                    {
                        continue;
                    }
                    queryFilter = queryFilter.FormatWith(item == DnsRecordType.A ? 0x0001 : 0x001c, fqdn);

                    if (client.Channel.Validate(packetsPath, queryFilter, ref errorMessage, "standard query"))
                    {
                        TraceFactory.Logger.Info("{0} query is present in the wireshark traces.".FormatWith(item));
                    }
                    else
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "{0} query is not present in the wireshark traces.".FormatWith(item));
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Validate A, AAAA responses
        /// </summary>
        /// <param name="packetsPath">The packets location.</param>
        /// <param name="serverAddress">IP address of the DHCP server.</param>
        /// <param name="recordType"><see cref="DnsRecordType"/></param>
        /// <param name="fqdn">The FQDN</param>
        /// <returns>True if the validation is successful, else false.</returns>
        private static bool ValidateResponse(string packetsPath, string serverAddress, DnsRecordType recordType, string fqdn, params string[] addresses)
        {
            TraceFactory.Logger.Info(CtcBaseTests.PACKET_VALIDATION);

            string responseFilter = "udp.port == 53 && dns.flags == 0x8580 && dns.qry.type == {0} && dns.qry.name == \"{1}\"";
            string errorMessage = string.Empty;

            using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(serverAddress))
            {
                foreach (DnsRecordType item in Enum.GetValues(typeof(DnsRecordType)))
                {
                    if (!recordType.HasFlag(item))
                    {
                        continue;
                    }

                    responseFilter = responseFilter.FormatWith(item == DnsRecordType.A ? 0x0001 : 0x001c, fqdn);

                    //if (client.Channel.Validate(packetsPath, responseFilter, ref errorMessage, "Standard query response, No error"))
                    if (client.Channel.Validate(packetsPath, responseFilter, ref errorMessage, "Standard query response"))
                    {
                        TraceFactory.Logger.Info("Response for {0} query is present in the wireshark traces.".FormatWith(recordType));
                        string responseAddresses = client.Channel.GetPacketData(packetsPath, responseFilter, "Addr:");

                        // For A record, IPv4 address & for AAAA, ipv6 address is validated.
                        string[] addressList = (item == DnsRecordType.A) ? addresses.Where(x => IPAddress.Parse(x).AddressFamily == AddressFamily.InterNetwork).Select(y => y.ToString()).ToArray()
                            : addresses.Where(x => IPAddress.Parse(x).AddressFamily == AddressFamily.InterNetwork).Select(y => y.ToString()).ToArray();

                        foreach (string address in addressList)
                        {
                            if (responseAddresses.Contains(address, StringComparison.CurrentCultureIgnoreCase))
                            {
                                TraceFactory.Logger.Info("Response for {0} query contains the IP Address: {1}.".FormatWith(item, address));
                            }
                            else
                            {
                                TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Response for {0} query doesnot contain the IP Address: {1}.".FormatWith(item, address));
                                return false;
                            }
                        }
                    }
                    else
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Response for {0} query is not present in the wireshark traces.".FormatWith(item));
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Validate SNMP trap for the specified hostname, domain name.
        /// </summary>
        /// <param name="serverAddress">IP Address of the server</param>
        /// <param name="testNo">Test No.</param>
        /// <param name="clientHostName">Host name of the client.</param>
        /// <param name="validDomainName">Domain name of the valid server.</param>
        /// <param name="invalidDomainName">invalid domain name if any.</param>
        /// <param name="clientAddresses">IP Addresses(IPv4, IPv6) of the client.</param>
        /// <returns>true if trap is successful, else false.</returns>
        private static bool ValidateSnmpTrap(NetworkNamingServiceActivityData activityData, string serverAddress, int testNo, string clientHostName, string validDomainName, string invalidDomainName = "", params string[] clientAddresses)
        {
            if (!SnmpWrapper.Instance().SetInetAddressType(16))
            {
                return false;
            }

            //Since the test case is failing delay is required to set the next snmp
            Thread.Sleep(TimeSpan.FromSeconds(30));
            if (!SnmpWrapper.Instance().SetInetTrapAddress(clientHostName))
            {
                return false;
            }

            //Since the test case is failing delay is required to set the next snmp
            Thread.Sleep(TimeSpan.FromSeconds(20));
            if (!SnmpWrapper.Instance().SetInetTrapRowStatus(1))
            {
                return false;
            }

            using (DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(serverAddress))
            {
                foreach (string clientAddress in clientAddresses)
                {
                    string recordType = IPAddress.Parse(clientAddress).AddressFamily == AddressFamily.InterNetwork ? "A" : "AAAA";

                    if (dnsClient.Channel.AddRecord(validDomainName, clientHostName, recordType, clientAddress.ToString()))
                    {
                        TraceFactory.Logger.Info("Successfully added {0} record with hostname: {1} and IP Address: {2} to the DNS server: {3}"
                                .FormatWith(recordType, clientHostName, clientAddress, serverAddress));
                    }
                    else
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to add {0} record with hostname: {1} and IP Address: {2} to the DNS server: {3}"
                                .FormatWith(recordType, clientHostName, clientAddress, serverAddress));
                        return false;
                    }
                }
            }

            using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(serverAddress))
            {
                string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-SNMPTrap".FormatWith(testNo.ToString(CultureInfo.CurrentCulture)));

                SnmpWrapper.Instance().SetInetTrapTest(90);

                Thread.Sleep(TimeSpan.FromSeconds(80));
                PacketDetails packetDetails = client.Channel.Stop(guid);

                string fqdn = "{0}.{1}".FormatWith(clientHostName, invalidDomainName);

                if (!ValidateQuery(packetDetails.PacketsLocation, serverAddress, DnsRecordType.A | DnsRecordType.AAAA, fqdn))
                {
                    return false;
                }

                if (ValidateResponse(packetDetails.PacketsLocation, serverAddress, DnsRecordType.A | DnsRecordType.AAAA, fqdn, clientAddresses))
                {
                    return false;
                }

                fqdn = "{0}.{1}".FormatWith(clientHostName, validDomainName);

                if (!ValidateQuery(packetDetails.PacketsLocation, serverAddress, DnsRecordType.A | DnsRecordType.AAAA, fqdn))
                {
                    return false;
                }

                return ValidateResponse(packetDetails.PacketsLocation, serverAddress, DnsRecordType.A | DnsRecordType.AAAA, fqdn, clientAddresses);
            }
        }

        /// <summary>
        /// Validates the packets for the existence of specified search strings.
        /// </summary>
        /// <param name="packetsPath">The pcap file location.</param>
        /// <param name="filter">The filter to be applied for the packets capture.</param>
        /// <remarks>If the filter is invalid, no contents will be read.</remarks>
        /// <param name="errorMessage">Error Info.</param>
        /// <param name="searchValues">The values to be searched in the packets.</param>
        /// <returns>True if all the search values are present in the string, else true.</returns>
        private static bool Validate(string packetsPath, string filter, params string[] searchValues)
        {
            string outputText = ReadPackets(packetsPath, filter);

            if (string.IsNullOrEmpty(outputText))
            {
                TraceFactory.Logger.Info("Either the filter is invalid or no packets are captured.{0}".FormatWith(Environment.NewLine));
                return false;
            }

            bool isValuePresent = true;

            foreach (string value in searchValues)
            {
                if (outputText.Contains(value, StringComparison.CurrentCultureIgnoreCase))
                {
                    TraceFactory.Logger.Info("The search value '{0}' is present in the packets.{1}".FormatWith(value, Environment.NewLine));
                }
                else
                {
                    TraceFactory.Logger.Info("The search value '{0}' is not present in the packets.{1}".FormatWith(value, Environment.NewLine));
                    isValuePresent &= false;
                }
            }

            return isValuePresent;
        }

        /// <summary>
        /// Read the pcap file contents to a string.
        /// </summary>
        /// <param name="packetsPath">The pcap file location.</param>
        /// <param name="filter">The filter to be applied for the packets capture.</param>
        /// <remarks>If the filter is invalid, no contents will be read.</remarks>
        /// <returns>the pcap file contents as string.</returns>
        private static string ReadPackets(string packetsPath, string filter)
        {
            try
            {
                // Apply the specified filter and read the contents of the pcap file.
                // Note: If the filter is invalid no content will be read.
                string arguments = string.Empty;
                string tSharkExecutable = string.Empty;

                // First check for the 64 bit version and if it is not found look for x86
                if (File.Exists(@"C:\Program Files\Wireshark\tshark.exe"))
                {
                    tSharkExecutable = @"C:\Program Files\Wireshark\tshark.exe";
                }
                else if (File.Exists(@"C:\Program Files (x86)\Wireshark\tshark.exe"))
                {
                    tSharkExecutable = @"C:\Program Files (x86)\Wireshark\tshark.exe";
                }

                if (string.IsNullOrEmpty(filter))
                {
                    arguments = "-V -r \"{0}\"".FormatWith(packetsPath);
                }
                else
                {
                    arguments = "-R \"{0}\" -V -r \"{1}\"".FormatWith(filter, packetsPath);
                }

                return ProcessUtil.Execute(tSharkExecutable, arguments).StandardOutput;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the line containing the specified searchValue from the packets. 
        /// If multiple values are matching the search string, '|' separated values will be returned.
        /// </summary>
        /// <param name="packetsPath">The pcap file location.</param>
        /// <param name="filter">The filter to be applied for the packets capture.</param>
        /// <remarks>If the filter is invalid, no contents will be read.</remarks>
        /// <param name="searchValue">The values to be searched in the captured packets.</param>
        /// <returns>The string contains the specified information.</returns>
        private static string GetPacketData(string packetsPath, string filter, string searchValue)
        {
            string outputText = ReadPackets(packetsPath, filter);

            if (string.IsNullOrEmpty(outputText))
            {
                TraceFactory.Logger.Info("Either the filter is invalid or no packets are captured.");
                return string.Empty;
            }

            // Split the text into lines and find the line containing the specified info
            return string.Join("|", Array.FindAll(outputText.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries), x => x.Contains(searchValue)).Select(x => x.Trim(new char[] { '\n', '\r', ' ' })).Distinct());
        }

        /// <summary>
        /// Validate RFC Option via EWS, SNMP and Telnet
        /// </summary>
        /// <param name="expectedValue">true if expecting enabled value, false for disabled</param>
        /// <returns>true if all the validations are successful, false otherwise</returns>
        private static bool VerifyRfcOption(bool expectedValue, bool isTps = false)
        {
            string optionValue = expectedValue == true ? "enabled" : "disabled";

            TraceFactory.Logger.Debug("Expected value: {0}".FormatWith(expectedValue));

            if (!EwsWrapper.Instance().GetRfc().Equals(expectedValue))
            {
                TraceFactory.Logger.Info("EWS: RFC option is not {0}.".FormatWith(optionValue));
                return false;
            }

            TraceFactory.Logger.Info("EWS: RFC option is {0}.".FormatWith(optionValue));

            if (!SnmpWrapper.Instance().GetRFCOption().Equals(expectedValue))
            {
                TraceFactory.Logger.Info("SNMP: RFC option is not {0}.".FormatWith(optionValue));
                return false;
            }

            TraceFactory.Logger.Info("SNMP: RFC option is {0}.".FormatWith(optionValue));
            //ctss
            // Telnet validation is only supported for TPS products
            //if (isTps)
            //{
            //    if (!TelnetWrapper.Instance().GetRFCOption().Equals(expectedValue))
            //    {
            //        TraceFactory.Logger.Info("Telnet: RFC option is not {0}.".FormatWith(optionValue));
            //        return false;
            //    }

            //    TraceFactory.Logger.Info("Telnet: RFC option is {0}.".FormatWith(optionValue));
            //}

            return true;
        }

        /// <summary>
        /// Configure Dhcp file
        /// </summary>
        /// <param name="address">Linux Server IP address</param>
        /// <param name="fromRange">Scope From Range</param>
        /// <param name="toRange">Scope To Range</param>
        /// <param name="hostName">Host name</param>
        /// <param name="domainName">Domain name</param>
        private static void ConfigureDhcpFile(IPAddress address, string hostName, string domainName, string fqdn = null)
        {
            TraceFactory.Logger.Debug("Configuring Dhcp file.");

            string winFilePath = Path.Combine(Path.GetTempPath(), LinuxUtils.DHCP_FILE);
            string linuxFilePath = string.Concat(LinuxUtils.BACKUP_FOLDER_PATH, LinuxUtils.DHCP_FILE);

            int length = address.ToString().Length;
            int lastIndex = address.ToString().LastIndexOf('.') + 1;
            string subnetMask = string.Format(CultureInfo.CurrentCulture, "{0}{1}", address.ToString().Remove(lastIndex, length - lastIndex), "0");
            string fromRange = string.Format(CultureInfo.CurrentCulture, "{0}{1}", address.ToString().Remove(lastIndex, length - lastIndex), "2");
            string toRange = string.Format(CultureInfo.CurrentCulture, "{0}{1}", address.ToString().Remove(lastIndex, length - lastIndex), "200");

            if (string.IsNullOrEmpty(fqdn))
            {
                fqdn = string.Empty;
            }

            // Copy to temp directory
            LinuxUtils.CopyFileFromLinux(address, linuxFilePath, winFilePath);

            Collection<string> keyValue = new Collection<string>();

            keyValue.Add(LinuxUtils.KEY_SUBNET_MASK);
            keyValue.Add(LinuxUtils.KEY_FROM_RANGE);
            keyValue.Add(LinuxUtils.KEY_TO_RANGE);
            keyValue.Add(LinuxUtils.KEY_DOMAIN_NAME);
            keyValue.Add(LinuxUtils.KEY_HOST_NAME);
            keyValue.Add(LinuxUtils.KEY_DEFAULT_LEASE_TIME);
            keyValue.Add(LinuxUtils.KEY_FQDN);

            Collection<string> configureValue = new Collection<string>();

            configureValue.Add(subnetMask);
            configureValue.Add(fromRange);
            configureValue.Add(toRange);
            configureValue.Add(domainName);
            configureValue.Add(hostName);
            configureValue.Add(TimeSpan.FromSeconds(120).TotalSeconds.ToString());
            configureValue.Add(fqdn);

            LinuxUtils.ConfigureFile(address, LinuxServiceType.DHCP, winFilePath, keyValue, configureValue);
        }

        /// <summary>
        /// Perform Post requisites on Linux server
        /// </summary>
        /// <remarks>
        /// Following operations are performed: 
        /// 1. Replace original configuration files
        /// 2. Start default service (DHCP)
        /// 3. Change port configuration from Linux to Windows Vlan
        /// 4. Change wrappers
        /// 5. Set default IP Configuration method
        /// </remarks>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        private static bool LinuxPostRequisites(NetworkNamingServiceActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
            TraceFactory.Logger.Info("Changing the port back to Original Vlan");
            int windowsVirtualLanId = GetVlanNumber(activityData, activityData.PrimaryDhcpServerIPAddress);
            INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIpAddress));
            networkSwitch.DisablePort(activityData.PortNumber);

            if (!networkSwitch.ChangeVirtualLan(activityData.PortNumber, windowsVirtualLanId))
            {
                return false;
            }

            networkSwitch.EnablePort(activityData.PortNumber);

            // Wait for sometime for configuration to take effect
            Thread.Sleep(TimeSpan.FromMinutes(3));

            IPAddress linuxServerIPAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);

            LinuxUtils.ReplaceOriginalFiles(linuxServerIPAddress);

            LinuxUtils.StopService(linuxServerIPAddress, LinuxServiceType.DHCP);
            LinuxUtils.StopService(linuxServerIPAddress, LinuxServiceType.DHCPV6);
            LinuxUtils.StopService(linuxServerIPAddress, LinuxServiceType.BOOTP);
            LinuxUtils.StopService(linuxServerIPAddress, LinuxServiceType.RARP);
            LinuxUtils.StartService(linuxServerIPAddress, LinuxServiceType.DHCP);

           
            // Wait for sometime for configuration to take effect
            Thread.Sleep(TimeSpan.FromMinutes(3));

            // To clear settings acquired from Linux, performing cold reset
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            if (activityData.ProductFamily == ProductFamilies.TPS.ToString())
            {
                TraceFactory.Logger.Info("Cold Resetting the printer");
                printer.ColdReset();
            }
            else if (activityData.ProductFamily == ProductFamilies.InkJet.ToString())
            {
                TraceFactory.Logger.Info("INK - Restore Factory Defaults from EWS page");
                EwsWrapper.Instance().RestoreDefaultsINK();
            }
            else
            {

                TraceFactory.Logger.Info("TCP Reset on Printer");
                printer.TCPReset();
            }

            EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
            SnmpWrapper.Instance().Create(activityData.WiredIPv4Address);
            //ctss
            //TelnetWrapper.Instance().Create(activityData.WiredIPv4Address);

            //EwsWrapper.Instance().SetTelnet();
            EwsWrapper.Instance().ResetConfigPrecedence();

            Thread.Sleep(TimeSpan.FromMinutes(2));

            return EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));
        }

        /// <summary>
        /// Perform Prerequisites on Linux server
        /// </summary>
        /// <remarks>
        /// Following operations are performed:
        /// 1. Change configuration method based on service type
        /// 2. Change port configuration from Windows to Linux Vlan
        /// 3. Start requested Service
        /// 4. Discover Printer under Linux server and get Printer IP Address
        /// 5. Change wrappers
        /// </remarks>
        /// <param name="address">Linux Printer IP Address</param>
        /// <param name="type"><see cref=" LinuxServiceType"/></param>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        private static bool LinuxPreRequisites(NetworkNamingServiceActivityData activityData, LinuxServiceType type, ref IPAddress address)
        {
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.LINUX_PREREQUISITES);
                IPAddress linuxServerAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);

                // Change configuration method based on reservation type
                EwsWrapper.Instance().SetIPConfigMethod(LinuxServiceType.DHCP == type ? IPConfigMethod.DHCP : IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));

                LinuxUtils.ReplaceOriginalFiles(linuxServerAddress);

                // Stop all services before starting requested service
                LinuxUtils.StopService(linuxServerAddress, LinuxServiceType.DHCP);
                LinuxUtils.StopService(linuxServerAddress, LinuxServiceType.DHCPV6);
                LinuxUtils.StopService(linuxServerAddress, LinuxServiceType.BOOTP);
                LinuxUtils.StopService(linuxServerAddress, LinuxServiceType.RARP);

                // In case of BootP server, Reservation for device should be configured so that, device gets BootP Address
                if (LinuxServiceType.BOOTP == type)
                {
                    IPAddress clientAddress = NetworkUtil.GetLocalAddress(linuxServerAddress.ToString(), linuxServerAddress.GetSubnetMask().ToString());
                    address = NetworkUtil.FetchNextIPAddress(clientAddress.GetSubnetMask(), clientAddress);

                    LinuxUtils.ReserveIPAddress(linuxServerAddress, address, activityData.PrinterMacAddress, LinuxServiceType.BOOTP);
                }

                LinuxUtils.StartService(linuxServerAddress, type);

                int linuxVirtualLanId = GetVlanNumber(activityData, linuxServerAddress.ToString());
                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIpAddress));
                networkSwitch.DisablePort(activityData.PortNumber);

                if (!networkSwitch.ChangeVirtualLan(activityData.PortNumber, linuxVirtualLanId))
                {
                    return false;
                }

                networkSwitch.EnablePort(activityData.PortNumber);

                // Wait for sometime for configuration to take effect
                Thread.Sleep(TimeSpan.FromMinutes(3));

                // Printer with Windows IP address might get discovered first
                address = IPAddress.Parse(CtcUtility.GetPrinterIPAddress(activityData.PrinterMacAddress));
                if (address.IsInSameSubnet(IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress)))
                {
                    address = IPAddress.Parse(CtcUtility.GetPrinterIPAddress(activityData.PrinterMacAddress));
                }

                if (address.IsInSameSubnet(IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress)))
                {
                    TraceFactory.Logger.Info("Unable to get Printer IP address connected in Linux server.");
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(address);
                SnmpWrapper.Instance().Create(address.ToString());
                if (!((activityData.ProductFamily == ProductFamilies.InkJet.ToString()) || (activityData.ProductFamily == ProductFamilies.TPS.ToString())))
                {
                    TelnetWrapper.Instance().Create(address.ToString());
                }

                return true;
            }
            catch (Exception defaultException)
            {
                TraceFactory.Logger.Info("Unable to perform Linux pre-requisites.");
                TraceFactory.Logger.Debug("Exception details: {0}".FormatWith(defaultException.JoinAllErrorMessages()));
                return false;
            }
        }

        /// <summary>
        /// Validate hostname, domain name and Dns primary & secondary address
        /// </summary>
        /// <param name="hostname">hostname</param>
        /// <param name="domainname">domain name</param>
        /// <param name="validateDns">true to validate Dns entries, false otherwise</param>
        /// <param name="dns">Dns value</param>
        /// <returns></returns>
        private static bool ValidateHostDomainName(string hostname, string domainname, bool validateDns = false, string dns = null)
        {
            if (!EwsWrapper.Instance().GetHostname().EqualsIgnoreCase(hostname))
            {
                TraceFactory.Logger.Info("Printer failed to acquire {0} hostname.".FormatWith(hostname));
                return false;
            }

            TraceFactory.Logger.Info("Printer acquired {0} hostname.".FormatWith(hostname));

            if (!EwsWrapper.Instance().GetDomainName().EqualsIgnoreCase(domainname))
            {
                TraceFactory.Logger.Info("Printer failed to acquire {0} domainname.".FormatWith(domainname));
                return false;
            }

            TraceFactory.Logger.Info("Printer acquired {0} domainname.".FormatWith(domainname));

            if (validateDns)
            {
                if (!EwsWrapper.Instance().GetPrimaryDnsServer().EqualsIgnoreCase(dns))
                {
                    TraceFactory.Logger.Info("Printer failed to acquire {0} primary dns server address.".FormatWith(dns));
                    return false;
                }

                TraceFactory.Logger.Info("Printer acquired {0} primary dns address.".FormatWith(dns));

                if (!EwsWrapper.Instance().GetSecondaryDnsServer().EqualsIgnoreCase(dns))
                {
                    TraceFactory.Logger.Info("Printer failed to acquire {0} secondary dns server address.".FormatWith(dns));
                    return false;
                }

                TraceFactory.Logger.Info("Printer acquired {0} secondary dns address.".FormatWith(dns));
            }

            return true;
        }

        /// <summary>
        /// Configures the server parameter and validates
        /// </summary>
        /// <param name="hostname">hostname</param>
        /// <param name="domainname">domain name</param>
        /// <param name="validateDns">true to validate server entries, false otherwise</param>
        /// <param name="dns">Dns value</param>
        /// <returns></returns>
        private static bool ConfigureAndValidateServerParameter(NetworkNamingServiceActivityData activityData, string newHostName, string newDomainName)
        {
            using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
            {
                string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);

                if (!dhcpClient.Channel.SetHostName(activityData.PrimaryDhcpServerIPAddress, scope, newHostName))
                {
                    TraceFactory.Logger.Info("Failed to set host name on server.");
                    return false;
                }

                if (!dhcpClient.Channel.SetDomainName(activityData.PrimaryDhcpServerIPAddress, scope, newDomainName))
                {
                    TraceFactory.Logger.Info("Failed to set domain name on server.");
                    return false;
                }
            }

            TraceFactory.Logger.Info("Configuring device with DHCP");
            if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address)))
            {
                return false;
            }

            TraceFactory.Logger.Info("Changing options-hostname on the DHCP Server");
            newHostName = CtcUtility.GetUniqueHostName();
            using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
            {
                string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);

                if (!dhcpClient.Channel.SetHostName(activityData.PrimaryDhcpServerIPAddress, scope, newHostName))
                {
                    TraceFactory.Logger.Info("Failed to set host name on server.");
                    return false;
                }
            }

            TraceFactory.Logger.Info("Cold Reset the Printer");
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            printer.ColdReset();

            EwsWrapper.Instance().ResetConfigPrecedence();
            Thread.Sleep(TimeSpan.FromSeconds(30));

            TraceFactory.Logger.Info("Validating the device is configured with new value");
            if (EwsWrapper.Instance().GetHostname().EqualsIgnoreCase(newHostName))
            {
                TraceFactory.Logger.Info("Host name on the Web UI is updated with the new value: {0} from the server.".FormatWith(newHostName));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Host name on the Web UI is not updated with the new value: {0} from the server.".FormatWith(newHostName));
                return false;
            }
        }

        /// <summary>
        /// Validates Wins Server address over cold reset
        /// </summary>
        /// <param name="hostname">hostname</param>
        /// <param name="domainname">domain name</param>
        /// <param name="validateDns">true to validate server entries, false otherwise</param>
        /// <param name="dns">Dns value</param>
        /// <returns></returns>
        private static bool ValidateWinsAddressOverColdReset(NetworkNamingServiceActivityData activityData)
        {
            TraceFactory.Logger.Info("Configuring manual WINS address on the device");
            EwsWrapper.Instance().SetPrimaryWinServerIP(activityData.PrimaryDhcpServerIPAddress);

            TraceFactory.Logger.Info("Deleting the wins existing entry on the DHCP Server");
            using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
            {
                string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);
                if (!dhcpClient.Channel.DeleteWinsServer(activityData.PrimaryDhcpServerIPAddress, scope))
                {
                    TraceFactory.Logger.Info("Failed to delete wins server on DHCP server scope options");
                    return false;
                }
            }

            TraceFactory.Logger.Info("Cold Reset the Printer");
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            printer.ColdReset();

            EwsWrapper.Instance().ResetConfigPrecedence();
            Thread.Sleep(TimeSpan.FromSeconds(30));

            TraceFactory.Logger.Info("Validating Wins manual entry is deleted over cold reset");
            if (EwsWrapper.Instance().GetPrimaryWinServerIP().EqualsIgnoreCase(string.Empty))
            {
                TraceFactory.Logger.Info("Manually configured Wins entry is deleted over cold reset");
            }
            else
            {
                TraceFactory.Logger.Info("Manually configured Wins entry is not deleted over cold reset");
                return false;
            }

            TraceFactory.Logger.Info("Configuring Wins Server on the DHCP Server Scope Options");
            using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
            {
                string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);
                if (!dhcpClient.Channel.SetWinsServer(activityData.PrimaryDhcpServerIPAddress, scope, activityData.PrimaryDhcpServerIPAddress))
                {
                    TraceFactory.Logger.Info("Failed to set wins server on DHCP server scope options");
                    return false;
                }
            }

            //Server parameter is not updating values to the printer[LFP] since the reservation is for "Both"
            //changing the printer reservation to DHCP
            if (activityData.ProductFamily == "LFP")
            {
                using (DhcpApplicationServiceClient client = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    client.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, client.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress),
                                                     activityData.WiredIPv4Address, activityData.PrinterMacAddress);

                    if (client.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, client.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress),
                                                         activityData.WiredIPv4Address, activityData.PrinterMacAddress, ReservationType.Dhcp))
                    {
                        TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for DHCP : Succeeded");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for DHCP Failed");
                        return false;
                    }
                }
            }

            printer.ColdReset();
            EwsWrapper.Instance().ResetConfigPrecedence();
            Thread.Sleep(TimeSpan.FromSeconds(30));

            TraceFactory.Logger.Info("Validating the device is configured with the dhcp server configured value");
            if (EwsWrapper.Instance().GetPrimaryWinServerIP().EqualsIgnoreCase(activityData.PrimaryDhcpServerIPAddress))
            {
                TraceFactory.Logger.Info("Wins Server IP on the Web UI is updated with the DHCP server configured value");
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Wins Server IP on the Web UI is not updated with the DHCP server configured value");
                return false;
            }
        }

        /// <summary>
        /// Gets the VLAN numbers corresponding to the default DHCP, second DHCP and Linux Servers.
        /// </summary>
        /// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
        /// <param name="serverIpAdress">The Server IP for which the vlan nuber iis to be fetched.</param>
        /// <returns>The vlan identifier for the specific server.</returns>
        private static int GetVlanNumber(NetworkNamingServiceActivityData activityData, string serverIpAdress)
        {
            // Get the VLAN number of default DHCP Server
            int vlanIdentifier = (from vlan in activityData.VirtualLanDetails
                                  where IPAddress.Parse(vlan.Value).IsInSameSubnet(IPAddress.Parse(serverIpAdress))
                                  select vlan.Key).FirstOrDefault();

            return vlanIdentifier;
        }

        /// <summary>
        /// Adding A/AAAA Record to the DNS Server
        /// </summary>
        /// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
        /// <param name="serverIpAdress">The Server IP for which records need to be added</param>
        /// <param name="domainName">domainname of the printer</param>
        /// <param name="hostName">hostName</param>
        /// <param name="recordType">A/AAAA</param>
        private static bool AddDnsRecord(NetworkNamingServiceActivityData activityData, string serverIP, string domainName, string hostName, params string[] clientAddresses)
        {
            using (DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(serverIP))
            {
                foreach (string clientAddress in clientAddresses)
                {
                    string recordType = IPAddress.Parse(clientAddress).AddressFamily == AddressFamily.InterNetwork ? "A" : "AAAA";

                    if (dnsClient.Channel.AddRecord(domainName, hostName, recordType, clientAddress.ToString()))
                    {
                        TraceFactory.Logger.Info("Successfully added {0} record with hostname: {1} and IP Address: {2} to the DNS server: {3}"
                                .FormatWith(recordType, hostName, clientAddress, serverIP));
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to add {0} record with hostname: {1} and IP Address: {2} to the DNS server: {3}"
                                .FormatWith(recordType, hostName, clientAddress, serverIP));
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Following Pre-requisites are performed:
        /// Ping Printer IP Address
        /// Second DHCP & Linux server are providing IP Address
        /// Primary DHCP Server:
        ///     IPConfig Service & DHCP Service
        ///     Default Lease time - IPv4
        ///     Preferred Life time - IPv6
        ///     Valid Life time - IPv6
        ///     Reserve Printer IP Address with 'Both' type 
        ///  Secondary DHCP Server:
        ///     IPConfig Service & DHCP Service
        ///     Default Lease time - IPv4		
        /// Set Default Configuration method - EWS
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns>true if all the above steps are successful, false otherwise</returns>
        private static bool TestPreRequisites(NetworkNamingServiceActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);

            if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.WiredIPv4Address), TimeSpan.FromSeconds(30)))
            {
                TraceFactory.Logger.Info("Printer IP Address:{0} is not accessible".FormatWith(activityData.WiredIPv4Address));
                return false;
            }

            EwsWrapper.Instance().ChangeDeviceAddress(activityData.SecondPrinterIPAddress);
            EwsWrapper.Instance().WakeUpPrinter();
            EwsWrapper.Instance().SetAdvancedOptions();
            EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
            EwsWrapper.Instance().WakeUpPrinter();
            EwsWrapper.Instance().SetAdvancedOptions();

			EwsWrapper.Instance().WakeUpPrinter();
            EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();

            bool isPrinterAccessible = false;
            if (!(activityData.ProductFamily == ProductFamilies.InkJet.ToString()))
            {
                if (!isPrinterAccessible)
                {
                    if (!CtcUtility.CheckPrinterAccessiblity(activityData.ProductFamily, activityData.WiredIPv4Address))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                if (!(EwsWrapper.Instance().ResetConfigPrecedence()))
                {
                    TraceFactory.Logger.Info(CtcBaseTests.FAILURE_PREFIX + "Failed to reset the config precedence table");
                }
            }
            else
            {
                TraceFactory.Logger.Info("INK Printer");
                EwsWrapper.Instance().EnableSNMPAccess();
            }


            return true;
        }

        /// <summary>
        /// Scan FB document to a pre-configured network folder.
        /// </summary>
        /// <param name="activityData"><see cref="NetworkNamingServiceActivityData"/></param>
        /// <param name="isPrimary">Verify Packet Capture on Primary or Secondary Server</param>		
        /// <param name="packetFileName">Packet Capture, pcap file name</param>
        /// <param name="testNo">Test Number</param>
        /// <returns></returns>
        private static bool ScanToNetworkFolder(NetworkNamingServiceActivityData activityData, bool isPrimary, string packetFileName, IPAddress localMachineAddress, int testNo, bool isIpv4)
        {
            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(isPrimary ? activityData.PrimaryDhcpServerIPAddress : activityData.SecondDhcpServerIPAddress);
            string serverIP = isPrimary ? activityData.PrimaryDhcpServerIPAddress : activityData.SecondDhcpServerIPAddress;
            DnsRecordType recordType = isIpv4 ? DnsRecordType.A : DnsRecordType.AAAA;
            string guid = client.Channel.TestCapture(activityData.SessionId, packetFileName);

            try
            {
                // Device Automation to scan the document
                IDevice device = DeviceFactory.Create(activityData.WiredIPv4Address);

                // Go to Home screen if incase it has any catridge popup errors
                if (GoToHomeScreen(device))
                {
                    TraceFactory.Logger.Info("Cleared catridge errors and successfully landed to home page");
                }
                else
                {
                    TraceFactory.Logger.Info("Printer control panel screen has errors, please clear the errors and repeat the test");
                    return false;
                }

                // Create network folder app object
                INetworkFolderApp app = NetworkFolderAppFactory.Create(device);

                // Launch the app
                app.Launch();

                // Select the predefined setting, this setting should be available on the printer
                app.SelectQuickset(QUICK_SET_NAME);

                // Set the file name 
                string fileName = "{0}-{1}.pdf".FormatWith(QUICK_SET_NAME, testNo);
                app.EnterFileName(fileName);

                // Setting the scan job options
                ScanExecutionOptions options = new ScanExecutionOptions();
                options.JobBuildSegments = 1;

                // Execute the scan job
                app.ExecuteJob(options);

                string scanFilePath = string.Concat(PREFIX, fileName);
                TraceFactory.Logger.Info("Filename : {0}".FormatWith(scanFilePath));
                //VEP all the scan options have default name as Untitled, either we need to clear that first or Let the testname append to it, chose the second option
                string scanDocFileName = Path.Combine(SHARE_FILE_PATH, scanFilePath);

                TraceFactory.Logger.Info("Find for : {0}".FormatWith(scanDocFileName));
                //Waiting for 1 min to verify the file is present in the specified location
                Thread.Sleep(TimeSpan.FromMinutes(4));
                TraceFactory.Logger.Debug(scanDocFileName);

                string fqdn = System.Environment.MachineName + "." + GetDomainName(serverIP).ToUpper();
                TraceFactory.Logger.Info("FQDN : {0}".FormatWith(fqdn));

                // check the file exist or not in the location
                if (File.Exists(scanDocFileName))
                {
                    TraceFactory.Logger.Info("Document is scanned and placed at {0} on the client machine.".FormatWith(scanDocFileName));

                    PacketDetails packetdetails = client.Channel.Stop(guid);
                    if (!ValidateQuery(packetdetails.PacketsLocation, activityData.PrimaryDhcpServerIPAddress,recordType, fqdn))
                    {
                        return false;
                    }
                    if (isIpv4)
                    {
                        if (!ValidateResponse(packetdetails.PacketsLocation, activityData.PrimaryDhcpServerIPAddress,recordType, fqdn, localMachineAddress.ToString()))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!ValidateResponse(packetdetails.PacketsLocation, activityData.PrimaryDhcpServerIPAddress, recordType, fqdn, GetClientIpAddress(serverIP, AddressFamily.InterNetworkV6).ToString()))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to scan the document");
                    return false;
                }
            }
            catch (Exception e)
            {
                TraceFactory.Logger.Info(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Go to HomeScreen
        /// This functionality works only for Missing Catridge popups
        /// </summary>        
        /// <param name="device">device ip</param>
        /// <returns></returns>
        private static bool GoToHomeScreen(IDevice device)
        {
            try
            {
                EwsWrapper.Instance().Start("https");
                if (EwsWrapper.Instance().IsOmniOpus)
                {
                    JediOmniDevice _device = new JediOmniDevice(device.Address);
                    _device.PowerManagement.Wake();
                    return true;
                }

                //Navigating the control back to home page if the control panel has error screen			
                JediWindjammerDevice jd = device as JediWindjammerDevice;
                JediWindjammerControlPanel cp = jd.ControlPanel;

                //validating whether the current page is in Home
                while (!cp.CurrentForm().Contains("Home"))
                {
                    //Pressing hide button to move the page to home
                    cp.Press("mButton1");
                }
                return true;
            }
            catch (Exception devicePopup)
            {
                //returning false if the error message is other than missing Catridge errors
                TraceFactory.Logger.Info("Devive Pop up message : {0} ".FormatWith(devicePopup.Message));
                return false;
            }
        }

        #endregion
    }
}
