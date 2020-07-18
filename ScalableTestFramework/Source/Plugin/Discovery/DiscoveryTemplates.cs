using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Printing;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Dhcp;
using HP.ScalableTest.PluginSupport.Connectivity.Discovery;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using HP.ScalableTest.PluginSupport.TopCat;
using HP.ScalableTest.Print.Drivers;
using HP.ScalableTest.Utility;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.Discovery
{

    /// <summary>
    /// Network Discovery Templates
    /// </summary>
    internal static class DiscoveryTemplates
    {
        /// <summary>
        /// Represents the folder types used for print
        /// </summary>
        enum FolderType
        {
            [EnumValue("SimpleFiles")]
            SimpleFiles,
            [EnumValue("ContinousFiles")]
            ContinousFiles,
            [EnumValue("CancelFiles")]
            CancelFiles,
            [EnumValue("HoseBreakFiles")]
            HoseBreakFiles,
            [EnumValue("PauseFiles")]
            PauseFiles,
            [EnumValue("FTPSimpleFiles")]
            FTPSimpleFiles,
            [EnumValue("FTPContinousFiles")]
            FTPContinousFiles,
            [EnumValue("FTPCancelFiles")]
            FTPCancelFiles,
            [EnumValue("FTPHoseBreakFiles")]
            FTPHoseBreakFiles
        }

        #region Network Discovery Templates

        #region  WSDiscovery

        /// <summary>
        /// Web Services Discovery using IP Address/ MAC Address / Bacabod Tool / Windows Explorer.
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool WSDiscovery(DiscoveryActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                string clientNetworkName = string.Empty;

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                    {
                        return false;
                    }
                }

                if (!EwsWrapper.Instance().SetWSDiscovery(true))
                {
                    return false;
                }
                try
                {
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                        // Step 1

                        TraceFactory.Logger.Info("Step 01/05 - Web Services Discovery - IP Address - Bacabod Tool");
                        if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.WSDV4))
                        {
                            return false;
                        }

                        // Step 2

                        TraceFactory.Logger.Info("Step 02/05 - Web Services Discovery - MAC Address - Bacabod Tool");
                        if (!BacabodDiscovery(activityData.PrinterMacAddress, true, BacaBodSourceType.MacAddress, BacaBodDiscoveryType.WSDV4))
                        {
                            return false;
                        }

                        // Step 3
                        TraceFactory.Logger.Info("Step 03/05 - Web Services Discovery - Windows Explorer");
                        if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), true, false))
                        {
                            return false;
                        }

                        // Step 4
                        TraceFactory.Logger.Info("Step 04/05 - Web Services Discovery - Client v4/v6 and Printer v4/v6");
                        if (!EwsWrapper.Instance().SetDHCPv6OnStartup(true))
                        {
                            return false;
                        }
                        if (!EwsWrapper.Instance().SetIPv6(false))
                        {
                            return false;
                        }
                        if (!EwsWrapper.Instance().SetIPv6(true))
                        {
                            return false;
                        }
                        if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), true, false))
                        {
                            return false;
                        }

                        // Step 5
                        TraceFactory.Logger.Info("Step 05/05 - Web Services Discovery - Client v4/v6 and Printer v4");
                        if (!EwsWrapper.Instance().SetIPv6(false))
                        {
                            return false;
                        }
                        if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), true, false))
                        {
                            return false;
                        }
                    }

                    // CURRENTLY THESE TEST CASES ARE NOT ENABLED BECAUSE STF NETWORK DOES NOT SUPPORT ACROSS NETWORK DISCOVERY
                    // ONCE THE INFRASTRUCTURE ISSUE IS FIXED, UNCOMMENT THE BELOW CODE WITH APPROPRIATE NUMBERING

                    //// Disabling the client NIC which has the same subnet as the printer.
                    //TraceFactory.Logger.Info("Disabling the client NIC corresponding to printer subnet.");
                    //clientNetworkName = Utility.GetClientNetworkName(printer.WiredIPv4Address.ToString());
                    //NetworkUtil.DisableNetworkConnection(clientNetworkName);

                    //// Step XX

                    //TraceFactory.Logger.Info("Step:XX - Web Services Discovery Across Subnets - IP Address - Bacabod Tool");
                    //if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.WSDV4))
                    //{
                    //    return false;
                    //}

                    //// Step XX

                    //TraceFactory.Logger.Info("Step:XX - Web Services Discovery Across Subnets - MAC Address - Bacabod Tool");
                    //if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.MacAddress, BacaBodDiscoveryType.WSDV4))
                    //{
                    //    return false;
                    //}

                    //// Step XX
                    //TraceFactory.Logger.Info("Step:XX - Web Services Discovery Across Subnets - Windows Explorer");
                    //if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), true, false))
                    //{
                    //    return false;
                    //}

                    return true;
                }
                catch (Exception discoveryException)
                {
                    TraceFactory.Logger.Info(discoveryException.Message);
                    return false;
                }
            }
        }

        #endregion

        #region  WSDiscoveryIPv6

        /// <summary>
        /// Web Services Discovery using various IPv6 Addresses (Link Local, Stateless, Stateful using Bacabod Tool / Windows Explorer.
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool WSDiscoveryIPv6(DiscoveryActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }
                // create printer object
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                    {
                        return false;
                    }
                }

                if (!EwsWrapper.Instance().SetWSDiscovery(true))
                {
                    return false;
                }

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    if (!EwsWrapper.Instance().SetDHCPv6OnStartup(true))
                    {
                        return false;
                    }
                }

                if (!EwsWrapper.Instance().SetIPv6(false))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetIPv6(true))
                {
                    return false;
                }

                Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(6);
                IPAddress StatefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
                IPAddress StatelessAddress = IPv6Utils.GetStatelessIPAddress(ipv6Details)[0];
                IPAddress LinkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);

                Random rnd = new Random(1);
                string manualIpv6Address = StatelessAddress.ToString().Replace(StatelessAddress.ToString().Substring(StatelessAddress.ToString().LastIndexOf(':') + 1), rnd.Next().ToString("X", CultureInfo.CurrentCulture).Substring(0, 4).ToLower(CultureInfo.CurrentCulture));
                if (!EwsWrapper.Instance().SetManualIPv6Address(true, manualIpv6Address))
                {
                    return false;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    // Step 1

                    TraceFactory.Logger.Info("Step 01/04 - Web Services Discovery - Link Local Address - Bacabod Tool");
                    if (!BacabodDiscovery(LinkLocalAddress.ToString(), true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.WSDV6))
                    {
                        return false;
                    }

                    // Step 2
                    TraceFactory.Logger.Info("Step 02/04 - Web Services Discovery - Stateful Address - Bacabod Tool");
                    if (!BacabodDiscovery(StatefulAddress.ToString(), true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.WSDV6))
                    {
                        return false;
                    }

                    // Step 3
                    TraceFactory.Logger.Info("Step 03/04 - Web Services Discovery - Stateless Address - Bacabod Tool");
                    if (!BacabodDiscovery(StatelessAddress.ToString(), true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.WSDV6))
                    {
                        return false;
                    }

                    // Step 4
                    if (!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                    {
                        TraceFactory.Logger.Info("Step 04/04 - Web Services Discovery - Manual IPv6 Address - Bacabod Tool");
                        if (!BacabodDiscovery(manualIpv6Address.ToString(), true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.WSDV6))
                        {
                            return false;
                        }
                    }

                    // Step 5

                    if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                    {
                        TraceFactory.Logger.Info("Step 04/04 - Web Services Discovery - Client v4/v6 and Printer v6 - Bacabod Tool");
                        TraceFactory.Logger.Info("Disabling IPv4 on the printer");
                        if (!EwsWrapper.Instance().SetIPv4(false, printer))
                        {
                            return false;
                        }

                        if (!BacabodDiscovery(LinkLocalAddress.ToString(), true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.WSDV6))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                catch (Exception discoveryException)
                {
                    TraceFactory.Logger.Info(discoveryException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    EwsWrapper.Instance().ChangeDeviceAddress(LinkLocalAddress);
                    EwsWrapper.Instance().SetIPv4(true);
                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.IPv4Address);
                }
            }
        }
        #endregion

        #region WSDiscovery_PowerCycle

        /// <summary>
        /// Template ID:79282
        /// TEMPLATE Verify DUT WSD status after reboot.
        /// 1st Step
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Disable 'WS Discovery' from web UI
        /// 4. Power cycle
        /// 5. Check WS Discovery is disabled or not.
        /// 6. Printer should not be discovered in network explorer.
        /// 
        /// 2nd Step
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Enable 'WS Discovery' from web UI
        /// 4. Power cycle
        /// 5. Check WS Discovery is enabled or not.
        /// 6. Printer should discover in network explorer.
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>
        public static bool WSDiscovery_PowerCycle(DiscoveryActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            //validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameter is null!!");

                return false;
            }

            // create printer object
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
            }
            if (!EwsWrapper.Instance().SetWSDiscovery(true))
            {
                return false;
            }
            if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                TraceFactory.Logger.Info("Check if SNMP is enabled ");
                EwsWrapper.Instance().EnableSNMPAccess();
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step-1
                TraceFactory.Logger.Info("Step 01/02 - Disable Web Services Discovery option from EWS and Power Cycle");
                if (!EwsWrapper.Instance().SetWSDiscovery(false))
                {
                    return false;
                }

                printer.PowerCycle();

                TraceFactory.Logger.Debug("Validating Web Services Discovery from EWS");
                bool wsDiscoveryOption = EwsWrapper.Instance().GetWSDiscovery();

                if (wsDiscoveryOption)
                {
                    TraceFactory.Logger.Info("Not as expected: Web Services Discovery option is enabled after Power Cycle");

                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: Web Services Discovery option is disabled after Power Cycle");
                }

                if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), false, false))
                {
                    return false;
                }

                // Step-2
                TraceFactory.Logger.Info("Step 02/02 - Enable Web Services Discovery option from EWS and Power Cycle");
                if (!EwsWrapper.Instance().SetWSDiscovery(true))
                {
                    return false;
                }

                printer.PowerCycle();

                TraceFactory.Logger.Debug("Validating Web Services Discovery from EWS");

                wsDiscoveryOption = EwsWrapper.Instance().GetWSDiscovery();

                if (!wsDiscoveryOption)
                {
                    TraceFactory.Logger.Info("Not as expected: Web Services Discovery option is disabled after Power Cycle");

                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: Web Services Discovery option is enabled after Power Cycle");
                }

                if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), true, false))
                {
                    return false;
                }

                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);

                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetWSDiscovery(true);
            }
        }

        #endregion

        #region WSDiscovery_Different_HostNames

        /// <summary>
        /// Template ID:79264
        /// TEMPLATE Verify WS-Discovery with different host names.
        /// 1st step
        /// 1.Change the host name of the printer to maximum number of characters.
        /// 2.printer should be displayed in the network explorer of the client after host name change
        ///
        /// 2nd step
        /// 1.Change the host name of the printer to a value containing special characters.
        /// 2.printer should be displayed in the network explorer of the client after host name change
        ///
        /// 3rd step
        /// 1.Change the host name of the printer to a single character
        /// 2.printer should be displayed in the network explorer of the client after host name change 
        ///
        /// NOTE:For Inkjet printer use hostname in the range of 2-15 characters
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData.</param>
        /// <param name="protocol">IPv4/IPv6 protocol.</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>       
        public static bool WSDiscovery_Different_HostNames(DiscoveryActivityData activityData, PluginSupport.Connectivity.ProtocolType protocol = PluginSupport.Connectivity.ProtocolType.IPv4)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            // validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameter is null!!");

                return false;
            }

            IPAddress ipv6Address = IPAddress.IPv6None;

            // create printer object
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
            }
            if (!EwsWrapper.Instance().SetWSDiscovery(true))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step-1

                TraceFactory.Logger.Info("Step 01/03 - Web Services Discovery with Hostname - Maximum Character - 32");
                if (!EwsWrapper.Instance().SetHostname("CTCNetworkAutomationTestCaseCTC"))
                {
                    return false;
                }

                if (!WindowsExplorerDiscovery(printer.WiredIPv4Address, true, false))
                {
                    return false;
                }

                // Step-2
                TraceFactory.Logger.Info("Step 02/03 - Web Services Discovery with Hostname - Special Character");

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    if (!EwsWrapper.Instance().SetHostname("host_name"))
                    {
                        return false;
                    }

                    if (!WindowsExplorerDiscovery(printer.WiredIPv4Address, true, false))
                    {
                        return false;
                    }
                }
                // Step-3

                TraceFactory.Logger.Info("Step 03/03 - Web Services Discovery with Hostname - Single Character");
                if (!EwsWrapper.Instance().SetHostname("a"))
                {
                    return false;
                }

                if (!WindowsExplorerDiscovery(printer.WiredIPv4Address, true, false))
                {
                    return false;
                }

                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);
                return false;
            }
            finally
            {
                EwsWrapper.Instance().SetHostname("default");
            }
        }

        #endregion

        #region WSDicovery_Enable_Disable

        /// <summary>
        /// Enable and Disable Web Services Discovery using SNMP/Telnet/EWS and validate discovery.
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool WSDicovery_Enable_Disable(DiscoveryActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                // create printer object
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                    {
                        return false;
                    }
                }
                if (!EwsWrapper.Instance().SetWSDiscovery(true))
                {
                    return false;
                }

                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    // Step 1
                    if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                    {
                        TraceFactory.Logger.Info("Check if SNMP is enabled ");
                        EwsWrapper.Instance().EnableSNMPAccess();
                    }
                    TraceFactory.Logger.Info("Step 01/06 - Web Services Discovery - Disable from EWS");
                    if (!EwsWrapper.Instance().SetWSDiscovery(false))
                    {
                        return false;
                    }
                    if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), false, false))
                    {
                        return false;
                    }

                    // Step 2

                    TraceFactory.Logger.Info("Step 02/06 - Web Services Discovery - Enable from EWS");
                    if (!EwsWrapper.Instance().SetWSDiscovery(true))
                    {
                        return false;
                    }
                    if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), true, false))
                    {
                        return false;
                    }

                    // Step 3

                    TraceFactory.Logger.Info("Step 03/06 - Web Services Discovery - Disable from SNMP");
                    if (!SnmpWrapper.Instance().SetWSDiscovery(false))
                    {
                        return false;
                    }
                    Thread.Sleep(TimeSpan.FromMinutes(2));
                    if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), false, false))
                    {
                        return false;
                    }

                    // Step 4

                    TraceFactory.Logger.Info("Step 04/06 - Web Services Discovery - Enable from SNMP");
                    if (!SnmpWrapper.Instance().SetWSDiscovery(true))
                    {
                        return false;
                    }
                    Thread.Sleep(TimeSpan.FromMinutes(2));
                    if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), true, false))
                    {
                        return false;
                    }

                    if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                    {
                        TraceFactory.Logger.Info("Step 05/06 & 06/06 - Not Applicable for Inkjet");
                    }
                    //CTSS
                    if (!(activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()) || activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.TPS.ToString())))
                    {
                        // Step 5

                        TraceFactory.Logger.Info("Step 05/06 - Web Services Discovery - Disable from Telnet");
                        if (!TelnetWrapper.Instance().ToggleParameter(PrinterParameters.WSD, family, activityData.IPv4Address, false))
                        {
                            return false;
                        }
                        Thread.Sleep(TimeSpan.FromMinutes(2));
                        if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), false, false))
                        {
                            return false;
                        }

                        // Step 6

                        TraceFactory.Logger.Info("Step 06/06 - Web Services Discovery - Enable from Telnet");
                        if (!TelnetWrapper.Instance().ToggleParameter(PrinterParameters.WSD, family, activityData.IPv4Address, true))
                        {
                            return false;
                        }
                        Thread.Sleep(TimeSpan.FromMinutes(2));
                        if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), true, false))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                catch (Exception discoveryException)
                {
                    TraceFactory.Logger.Info(discoveryException.Message);

                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    EwsWrapper.Instance().SetWSDiscovery(true);
                }
            }
        }

        #endregion

        #region WSDiscovery_Configuration_Changes

        /// <summary>
        /// Template ID:79212
        /// TEMPLATE Verify WS-Discovery over configuration changes
        /// 1st Step
        /// 1. Change the IP address
        /// 2. Printer should be displayed on the network explorer.
        /// 2nd Step
        /// 1. Change the host name
        /// 2. Printer should be displayed in the network explorer."
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool WSDiscovery_Configuration_Changes(DiscoveryActivityData activityData, PluginSupport.Connectivity.ProtocolType protocol = PluginSupport.Connectivity.ProtocolType.IPv4)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            // validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameter is null!!");

                return false;
            }

            IPAddress newIPAddress = IPAddress.None;
            IPAddress ipv6Address = IPAddress.IPv6None;

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
            }
            if (!EwsWrapper.Instance().SetWSDiscovery(true))
            {
                return false;
            }

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                string serverIP = Printer.Printer.GetDHCPServerIP(IPAddress.Parse(activityData.IPv4Address)).ToString();
                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(serverIP);
                string scopeIP = serviceMethod.Channel.GetDhcpScopeIP(serverIP);
                // Create Reservation for the printer for DHCP in DHCP server
                if (serviceMethod.Channel.DeleteReservation(serverIP, scopeIP, activityData.IPv4Address, activityData.PrinterMacAddress) &&
                    serviceMethod.Channel.CreateReservation(serverIP, scopeIP, activityData.IPv4Address, activityData.PrinterMacAddress, ReservationType.Both))
                {
                    TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for DHCP : Succeeded");
                }
                else
                {
                    TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for DHCP : Failed");
                    return false;
                }
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step-1

                TraceFactory.Logger.Info("Step 01/02 - Web Services Discovery after IP Address / Config Method Change");

                TraceFactory.Logger.Debug("Fetching the next available free IPv4 Address");

                newIPAddress = PluginSupport.Connectivity.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.IPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.IPv4Address));
                TraceFactory.Logger.Info("New IPv4 Address used for Windows Network Discovery is {0}".FormatWith(newIPAddress));

                TraceFactory.Logger.Info("Configuring Manual IPv4 Address:" + newIPAddress);
                if (!EwsWrapper.Instance().SetIPaddress(newIPAddress.ToString()))
                {
                    return false;
                }
                Thread.Sleep(TimeSpan.FromMinutes(2));

                if (!WindowsExplorerDiscovery(newIPAddress, true, false))
                {
                    return false;
                }


                TraceFactory.Logger.Info("Reverting IP Configuration Method from Manual to DHCP");
                if (!EwsWrapper.Instance().SetIPConfigMethod(Printer.IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.IPv4Address)))
                {
                    return false;
                }
                Thread.Sleep(TimeSpan.FromMinutes(2));

                // Step-2

                TraceFactory.Logger.Info("Step 02/02 - Web Services Discovery after Hostname Change");
                if (!EwsWrapper.Instance().SetHostname("temporaryhostname"))
                {
                    return false;
                }

                if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), true, false))
                {
                    return false;
                }
                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);
                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetIPConfigMethod(Printer.IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.IPv4Address));
                EwsWrapper.Instance().SetHostname("Default");
            }
        }

        #endregion

        #region WSDicovery_MulticastIPv4

        /// <summary>
        /// Template ID:133361
        /// TEMPLATE Verify DUT WSD behavior when Multicast IPv4 is Enabled and Disabled.
        ///
        /// 1st step
        /// 1.bacabod -> Select IP address option.->Enter IP address of the printer ->Start Discovery
        /// 2.Printer should get discovered in BacaBod
        ///
        /// 2nd step:Printer discovery using BacaBod software over WSD when multicast IPV4 is disabled.
        /// 1.bacabod ->Select All devices option->Select WSDv4 check box -> Start Discovery.
        /// 2.Printer should not get discovered in BacaBod
        ///
        /// 3rd step:Printer discovery using BacaBod software over WSD when multicast IPV4 is disabled.
        /// 1.bacabod ->Select All devices option->Select WSDv4 check box ->Start Discovery.
        /// 2.Printer should get discovered in BacaBod
        ///
        /// 4th step:Printer discovery over WSDV4 independent of multicast IPV4.
        /// 1.Disable Multicast IPv4 on the printer from web UI/telnet/SNMP.
        /// 2.Change the hostname of the printer from web UI/telnet/SNMP.
        /// 3.Printer should not send a probe match and not get discovered in vista with the new hostname
        ///
        /// 5th step:Printer discovery over WSDV4 when multicast IPV4 is disabled.
        /// 1.Disable Multicast IPv4 on the printer from web UI/telnet/SNMP.
        /// 2.Change the hostname of the printer from web UI/telnet/SNMP.
        /// 3.Printer should not send a probe match over IPv4 and not get discovered in vista with the new hostname
        ///
        ///Note: Updated Test case based on Amitha input[Removed step 3 and step5 since it is similar to step2 and step4]
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool WSDicovery_MulticastIPv4(DiscoveryActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            // validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameters are null!!");
                return false;
            }

            if (!EwsWrapper.Instance().SetDHCPv6OnStartup(true))
            {
                return false;
            }
            if (!EwsWrapper.Instance().SetIPv6(false))
            {
                return false;
            }
            if (!EwsWrapper.Instance().SetIPv6(true))
            {
                return false;
            }
            if (!EwsWrapper.Instance().SetMulticastIPv4(true))
            {
                return false;
            }
            if (!EwsWrapper.Instance().SetWSDiscovery(true))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step 1a

                TraceFactory.Logger.Info("Step 01/07 - Web Services Discovery when Multicast IPv4 is enabled - Bacabod Tool");
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }

                if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.WSDV4))
                {
                    return false;
                }

                // Step 2
                TraceFactory.Logger.Info("Step 02/07 - Web Services Discovery when Multicast IPv4 is enabled - Windows Network Explorer");
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
                if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), true, false))
                {
                    return false;
                }

                //Step 3

                TraceFactory.Logger.Info("Step 03/07 - Web Services Discovery when Multicast IPv4 is disabled - Bacabod Tool");
                if (!EwsWrapper.Instance().SetMulticastIPv4(false))
                {
                    return false;
                }
                if (!BacabodDiscovery(activityData.IPv4Address, false, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.WSDV4))
                {
                    return false;
                }

                // Step 4
                TraceFactory.Logger.Info("Step 04/07 - Web Services Discovery when Multicast IPv4 is disabled - Windows Network Explorer");
                if (!EwsWrapper.Instance().SetMulticastIPv4(false))
                {
                    return false;
                }
                if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), false, false))
                {
                    return false;
                }

                // Step 5

                TraceFactory.Logger.Info("Step 05/07 - Web Services Discovery when Multicast IPv4 is disabled and Hostname changed - Bacabod Tool");
                if (!EwsWrapper.Instance().SetMulticastIPv4(false))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetHostname("temporaryhostname"))
                {
                    return false;
                }
                if (!BacabodDiscovery(activityData.IPv4Address, false, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.WSDV4))
                {
                    return false;
                }

                // Step 6
                TraceFactory.Logger.Info("Step 06/07 - Web Services Discovery when Multicast IPv4 is disabled and Hostname changed - Windows Network Explorer");
                if (!EwsWrapper.Instance().SetMulticastIPv4(false))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetHostname("xyz"))
                {
                    return false;
                }
                if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), false, false))
                {
                    return false;
                }

                // Step 7

                Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(6);
                IPAddress StatefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);

                TraceFactory.Logger.Info("Step 07/07 - Web Services Discovery using IPv6 when Multicast IPv4 is disabled - Bacabod Tool");
                TraceFactory.Logger.Info("IPv6 Address used for Bacabod Tool is {0}".FormatWith(StatefulAddress));
                if (!EwsWrapper.Instance().SetMulticastIPv4(false))
                {
                    return false;
                }

                if (!BacabodDiscovery(StatefulAddress.ToString(), true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.WSDV6))
                {
                    return false;
                }

                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);
                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetMulticastIPv4(true);
                EwsWrapper.Instance().SetHostname("Default");
            }
        }

        #endregion

        #region WSDiscovery_ColdReset

        /// <summary>
        /// Template ID:79284
        /// TEMPLATE Verify DUT WSD status after cold reset.
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Disable WSD
        /// 4. Cold Reset
        /// 5. WSD should be enabled.
        /// 6. Printer should be discover in network explorer."
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool WSDiscovery_ColdReset(DiscoveryActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            // validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameters are null!!");

                return false;
            }

            // create printer object
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
            }
            if (!EwsWrapper.Instance().SetWSDiscovery(true))
            {
                return false;
            }
            if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                TraceFactory.Logger.Info("Check if SNMP is enabled ");
                EwsWrapper.Instance().EnableSNMPAccess();
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step 01

                TraceFactory.Logger.Info("Step 01/02 - Disable Web Services Discovery option from EWS and Cold Reset/Restore Factory Settings");
                if (!EwsWrapper.Instance().SetWSDiscovery(false))
                {
                    return false;
                }
                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    printer.ColdReset();
                    //CTSS
                    if (!(activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.TPS.ToString())))
                    {
                        if (!EwsWrapper.Instance().SetTelnet())
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (!EwsWrapper.Instance().SetFactoryDefaults())
                    {
                        return false;
                    }
                }

                bool wsDiscoveryOption = EwsWrapper.Instance().GetWSDiscovery();
                if (!wsDiscoveryOption)
                {
                    TraceFactory.Logger.Info("Not as expected: Web Services Discovery option is disabled after Cold Reset / Restore Factory Settings");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: Web Services Discovery option is enabled after Cold Reset / Restore Factory Settings");
                }

                if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), true, false))
                {
                    return false;
                }

                // Step 02

                TraceFactory.Logger.Info("Step 02/02 - Enable Web Services Discovery option from EWS and Cold Reset/Restore Factory Settings");

                if (!EwsWrapper.Instance().SetWSDiscovery(true))
                {
                    return false;
                }
                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    printer.ColdReset();
                    //CTSS
                    if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                    {
                        if (!EwsWrapper.Instance().SetTelnet())
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (!EwsWrapper.Instance().SetFactoryDefaults())
                    {
                        return false;
                    }
                }

                wsDiscoveryOption = EwsWrapper.Instance().GetWSDiscovery();
                if (!wsDiscoveryOption)
                {
                    TraceFactory.Logger.Info("Not as expected: Web Services Discovery option is disabled after Cold Reset / Restore Factory Settings");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: Web Services Discovery option is enabled after Cold Reset / Restore Factory Settings");
                }

                if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), true, false))
                {
                    return false;
                }

                return true;
            }
            catch (Exception discoveryexception)
            {
                TraceFactory.Logger.Info(discoveryexception.Message);
                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetWSDiscovery(true);
            }
        }

        #endregion

        #region SNMP_Discovery

        /// <summary>
        /// Template ID:79224
        /// TEMPLATE Verify DUT SNMP discovery
        /// 1. WJA -> Select Discovery -> Enter IP address -> Start Discovery          
        /// 2. The Printer should be discovered in WJA"
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool SNMP_Discovery(DiscoveryActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            // validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameters are null!!");

                return false;
            }
            if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                TraceFactory.Logger.Info("Check if SNMP is enabled ");
                EwsWrapper.Instance().EnableSNMPAccess();
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                //Step 1

                TraceFactory.Logger.Info("Validating SNMP Discovery");

                if (PrinterDiscovery.ProbeDevice(IPAddress.Parse(activityData.IPv4Address)).Equals(null))
                {
                    if (PrinterDiscovery.ProbeDevice(IPAddress.Parse(activityData.IPv4Address)).Equals(null))
                    {
                        TraceFactory.Logger.Info("Not as expected: Printer {0} is not discovered through SNMP".FormatWith(activityData.IPv4Address));

                        return false;
                    }
                }

                TraceFactory.Logger.Info("As expected: Printer {0} is discovered through SNMP".FormatWith(activityData.IPv4Address));

                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);

                return false;
            }
        }

        #endregion

        #region  SLPDiscovery

        /// <summary>
        /// SLP Discovery using IP Address/ MAC Address / Bacabod Tool.
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool SLPDiscovery(DiscoveryActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                string clientNetworkName = string.Empty;

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                    {
                        return false;
                    }
                }
                if (!EwsWrapper.Instance().SetSLP(true))
                {
                    return false;
                }

                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    // Step 1

                    TraceFactory.Logger.Info("Step 01/04 - SLP Discovery - IP Address - Bacabod Tool");
                    if (!EwsWrapper.Instance().SetSLP(true))
                    {
                        return false;
                    }
                    if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                    {
                        return false;
                    }
                    if (!WindowsExplorerDiscovery(IPAddress.Parse(activityData.IPv4Address), true, false))
                    {
                        return false;
                    }
                    //Step 2

                    TraceFactory.Logger.Info("Step 02/04 - SLP Discovery - MAC Address - Bacabod Tool");
                    if (!EwsWrapper.Instance().SetSLP(true))
                    {
                        return false;
                    }

                    if (!BacabodDiscovery(activityData.PrinterMacAddress, true, BacaBodSourceType.MacAddress, BacaBodDiscoveryType.SLP))
                    {
                        return false;
                    }

                    if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                    {
                        TraceFactory.Logger.Info("Step 03/04 & 04/04 Not Applicable for TPS/Inkjet Products");
                    }

                    // Step 3

                    if (!(activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) || activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString())))
                    {
                        TraceFactory.Logger.Info("Step 03/04 - SLP Discovery when SLP Client Mode Enabled - IP Address - Bacabod Tool");

                        if (!EwsWrapper.Instance().SetSLP(true))
                        {
                            return false;
                        }
                        if (!EwsWrapper.Instance().SetSLPClientMode(true))
                        {
                            return false;
                        }
                        if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                        {
                            return false;
                        }
                    }

                    // Step 4

                    if (!(activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) || activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString())))
                    {
                        TraceFactory.Logger.Info("Step 04/04 - SLP Discovery when SLP Client Mode Enabled - MAC Address - Bacabod Tool");
                        if (!EwsWrapper.Instance().SetSLP(true))
                        {
                            return false;
                        }
                        if (!EwsWrapper.Instance().SetSLPClientMode(true))
                        {
                            return false;
                        }
                        if (!BacabodDiscovery(activityData.PrinterMacAddress, true, BacaBodSourceType.MacAddress, BacaBodDiscoveryType.SLP))
                        {
                            return false;
                        }
                    }

                    // CURRENTLY THESE TEST CASES ARE NOT ENABLED BECAUSE STF NETWORK DOES NOT SUPPORT ACROSS NETWORK DISCOVERY
                    // ONCE THE INFRASTRUCTURE ISSUE IS FIXED, UNCOMMENT THE BELOW CODE WITH APPROPRIATE NUMBERING

                    //// Disabling the client NIC which has the same subnet as the printer.
                    //TraceFactory.Logger.Info("Disabling the client NIC corresponding to printer subnet.");
                    //clientNetworkName = Utility.GetClientNetworkName(activityData.IPv4Address);
                    //NetworkUtil.DisableNetworkConnection(clientNetworkName);

                    //// Step XX

                    //TraceFactory.Logger.Info("Step:XX - SLP Discovery Across Subnets - IP Address - Bacabod Tool");
                    //if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                    //{
                    //    return false;
                    //}

                    //// Step XX

                    //TraceFactory.Logger.Info("Step:XX - SLP Discovery Across Subnets - MAC Address - Bacabod Tool");
                    //if (!BacabodDiscovery(activityData.PrinterMacAddress, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                    //{
                    //    return false;
                    //}

                    return true;
                }
                catch (Exception discoveryException)
                {
                    TraceFactory.Logger.Info(discoveryException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    if (!(activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) || activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString())))
                    {
                        EwsWrapper.Instance().SetSLPClientMode(false);
                    }
                }
            }
        }

        #endregion

        #region SLPDiscovery_PowerCycle

        /// <summary>
        /// Template ID:79282
        /// TEMPLATE Verify DUT SLP status after reboot.
        /// 1st Step
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Disable 'SLP'/'SLP Client Mode' from web UI
        /// 4. Power cycle
        /// 5. Check 'SLP'/'SLP Client Mode' is disabled or not.
        /// 6. Printer should not be discovered in network explorer.
        /// 
        /// 2nd Step
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Enable 'SLP'/'SLP Client Mode' from web UI
        /// 4. Power cycle
        /// 5. Check 'SLP'/'SLP Client Mode' is enabled or not.
        /// 6. Printer should discover in network explorer.
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>
        public static bool SLPDiscovery_PowerCycle(DiscoveryActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            bool SLPClientModeOnlyOption = false;

            //validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameter is null!!");

                return false;
            }

            // create printer object
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
            }
            if (!EwsWrapper.Instance().SetSLP(true))
            {
                return false;
            }
            if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                TraceFactory.Logger.Info("Check if SNMP is enabled ");
                EwsWrapper.Instance().EnableSNMPAccess();
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step 1

                TraceFactory.Logger.Info("Step 01/04 - Disable SLP option from EWS and Power Cycle");
                if (!EwsWrapper.Instance().SetSLP(false))
                {
                    return false;
                }
                printer.PowerCycle();

                TraceFactory.Logger.Debug("Validating SLP option from EWS");
                bool SLPDiscoveryOption = EwsWrapper.Instance().GetSLP();
                if (SLPDiscoveryOption)
                {
                    TraceFactory.Logger.Info("Not as expected: SLP Discovery option is enabled after Power Cycle");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: SLP Discovery option is disabled after Power Cycle");
                }
                if (!BacabodDiscovery(activityData.IPv4Address, false, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                {
                    return false;
                }

                // Step 2

                TraceFactory.Logger.Info("Step 02/04 - Enable SLP option from EWS and Power Cycle");
                if (!EwsWrapper.Instance().SetSLP(true))
                {
                    return false;
                }

                printer.PowerCycle();

                TraceFactory.Logger.Debug("Validating SLP option from EWS");
                SLPDiscoveryOption = EwsWrapper.Instance().GetSLP();
                if (!SLPDiscoveryOption)
                {
                    TraceFactory.Logger.Info("Not as expected: SLP Discovery option is disabled after Power Cycle");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: SLP Discovery option is enabled after Power Cycle");
                }
                if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                {
                    return false;
                }

                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) || (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString())))
                {
                    TraceFactory.Logger.Info("Steps 03/04 & 04/04 Not Applicable for TPS and Inkjet Products");
                }

                // Step 3

                if ((!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString())) || (!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString())))
                {
                    TraceFactory.Logger.Info("Step 03/04 - Disable SLP option & SLP Client Mode option from EWS and Power Cycle");

                    if (!EwsWrapper.Instance().SetSLP(false))
                    {
                        return false;
                    }

                    if (!EwsWrapper.Instance().SetSLPClientMode(false))
                    {
                        return false;
                    }
                    printer.PowerCycle();
                    TraceFactory.Logger.Debug("Validating SLP option & SLP Client Mode option from EWS");
                    SLPDiscoveryOption = EwsWrapper.Instance().GetSLP();
                    SLPClientModeOnlyOption = EwsWrapper.Instance().GetSLPClientMode();
                    if (SLPDiscoveryOption)
                    {
                        TraceFactory.Logger.Info("Not as expected: SLP Discovery option is enabled after Power Cycle");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("As expected: SLP Discovery option is disabled after Power Cycle");
                    }
                    if (SLPClientModeOnlyOption)
                    {
                        TraceFactory.Logger.Info("Not as expected: SLP Client Mode option is enabled after Power Cycle");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("As expected: SLP Client Mode option is disabled after Power Cycle");
                    }
                }

                // Step 4
                TraceFactory.Logger.Info("Step 04/04 - Enable SLP option & SLP Client Mode option from EWS and Power Cycle");
                if (!EwsWrapper.Instance().SetSLP(true))
                {
                    return false;
                }
				//Not supported for TPS and INK
                if ((!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString())) || (!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString())))
                {
                    if (!EwsWrapper.Instance().SetSLPClientMode(true))
                    {
                        return false;
                    }
                }

                printer.PowerCycle();

                TraceFactory.Logger.Debug("Validating SLP option & SLP Client Mode option from EWS");
                SLPDiscoveryOption = EwsWrapper.Instance().GetSLP();
             	if ((!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString())) || (!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString())))
                {
			  		  SLPClientModeOnlyOption = EwsWrapper.Instance().GetSLPClientMode();
				}
                if (!SLPDiscoveryOption)
                {
                    TraceFactory.Logger.Info("Not as expected: SLP Discovery option is disabled after Power Cycle");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: SLP Discovery option is enabled after Power Cycle");
                }

                if ((!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString())) || (!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString())))
                {
                    SLPClientModeOnlyOption = EwsWrapper.Instance().GetSLPClientMode();

                    if (!SLPClientModeOnlyOption)
                    {
                        TraceFactory.Logger.Info("Not as expected: SLP Client Mode option is disabled after Power Cycle");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("As expected: SLP Client Mode option is enabled after Power Cycle");
                    }
                }
                return true;
            }
            catch (Exception discoveryexception)
            {
                TraceFactory.Logger.Info(discoveryexception.Message);

                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetSLP(true);
            }
        }

        #endregion

        #region SLPDiscovery_Different_HostNames

        /// <summary>
        /// Template ID:79264
        /// TEMPLATE Verify SLP Discovery with different host names.
        /// 1st step
        /// 1.Change the host name of the printer to maximum number of characters.
        /// 2.printer should be displayed in the network explorer of the client after host name change
        ///
        /// 2nd step
        /// 1.Change the host name of the printer to a value containing special characters.
        /// 2.printer should be displayed in the network explorer of the client after host name change
        ///
        /// 3rd step
        /// 1.Change the host name of the printer to a single character
        /// 2.printer should be displayed in the network explorer of the client after host name change 
        ///
        /// NOTE:For Inkjet printer use hostname in the range of 2-15 characters
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData.</param>
        /// <param name="protocol">IPv4/IPv6 protocol.</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>       
        public static bool SLPDiscovery_Different_HostNames(DiscoveryActivityData activityData, PluginSupport.Connectivity.ProtocolType protocol = PluginSupport.Connectivity.ProtocolType.IPv4)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            // validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameter is null!!");

                return false;
            }

            IPAddress ipv6Address = IPAddress.IPv6None;

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
            }
            if (!EwsWrapper.Instance().SetSLP(true))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step-1

                TraceFactory.Logger.Info("Step 01/03 - SLP Discovery with Hostname - Maximum Character - 32");
                if (!EwsWrapper.Instance().SetHostname("CTCNetworkAutomationTestCaseCTC"))
                {
                    return false;
                }

                if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                {
                    return false;
                }

                // Step-2

                TraceFactory.Logger.Info("Step 02/03 - SLP Discovery with Hostname - Special Character");
                if (!EwsWrapper.Instance().SetHostname("host_name"))
                {
                    return false;
                }

                if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                {
                    return false;
                }

                // Step-3

                TraceFactory.Logger.Info("Step 03/03 - SLP Discovery with Hostname - Single Character");
                if (!EwsWrapper.Instance().SetHostname("a"))
                {
                    return false;
                }

                if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                {
                    return false;
                }

                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);
                return false;
            }
            finally
            {
                EwsWrapper.Instance().SetHostname("Default");
            }
        }

        #endregion

        #region SLPDiscovery_Enable_Disable

        /// <summary>
        /// Enable and Disable SLP Discovery using SNMP/Telnet/EWS and validate discovery.
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool SLPDiscovery_Enable_Disable(DiscoveryActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                    {
                        return false;
                    }
                }
                if (!EwsWrapper.Instance().SetSLP(true))
                {
                    return false;
                }
                if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    TraceFactory.Logger.Info("Check if SNMP is enabled ");
                    EwsWrapper.Instance().EnableSNMPAccess();
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    // Step 1

                    TraceFactory.Logger.Info("Step 01/06 - SLP Discovery - Disable from EWS");
                    if (!EwsWrapper.Instance().SetSLP(false))
                    {
                        return false;
                    }

                    if (!BacabodDiscovery(activityData.IPv4Address, false, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                    {
                        return false;
                    }

                    // Step 2

                    TraceFactory.Logger.Info("Step 02/06 - SLP Discovery - Enable from EWS");

                    if (!EwsWrapper.Instance().SetSLP(true))
                    {
                        return false;
                    }

                    if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                    {
                        return false;
                    }

                    // Step 3

                    TraceFactory.Logger.Info("Step 03/06 - SLP Discovery - Disable from SNMP");

                    if (!SnmpWrapper.Instance().SetSLP(false))
                    {
                        return false;
                    }
                    Thread.Sleep(TimeSpan.FromMinutes(2));

                    if (!BacabodDiscovery(activityData.IPv4Address, false, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                    {
                        return false;
                    }

                    // Step 4

                    TraceFactory.Logger.Info("Step 04/06 - SLP Discovery - Enable from SNMP");

                    if (!SnmpWrapper.Instance().SetSLP(true))
                    {
                        return false;
                    }
                    Thread.Sleep(TimeSpan.FromMinutes(2));

                    if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                    {
                        return false;
                    }

                    if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                    {
                        TraceFactory.Logger.Info("Step 05/06 & 06/06 - Not Applicable for Inkjet Printers");
                    }
                    //CTSS
                    if (!(activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString())) || activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.TPS.ToString()))
                    {
                        // Step 5

                        TraceFactory.Logger.Info("Step 05/06 - SLP Discovery - Disable from Telnet");

                        if (!TelnetWrapper.Instance().ToggleParameter(PrinterParameters.SLP, family, activityData.IPv4Address, false))
                        {
                            return false;
                        }
                        Thread.Sleep(TimeSpan.FromMinutes(2));

                        if (!BacabodDiscovery(activityData.IPv4Address, false, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                        {
                            return false;
                        }

                        // Step 6

                        TraceFactory.Logger.Info("Step 06/06 - SLP Discovery - Enable from Telnet");

                        if (!TelnetWrapper.Instance().ToggleParameter(PrinterParameters.SLP, family, activityData.IPv4Address, true))
                        {
                            return false;
                        }
                        Thread.Sleep(TimeSpan.FromMinutes(2));

                        if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                        {
                            return false;
                        }
                    }

                    return true;
                }
                catch (Exception discoveryException)
                {
                    TraceFactory.Logger.Info(discoveryException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    EwsWrapper.Instance().SetSLP(true);
                }
            }
        }

        #endregion

        #region SLPDiscovery_Configuration_Changes

        /// <summary>
        /// Template ID:79212
        /// TEMPLATE Verify SLP-Discovery over configuration changes
        /// 1st Step
        /// 1. Change the IP address
        /// 2. Printer should be displayed on the network explorer.
        /// 2nd Step
        /// 1. Change the host name
        /// 2. Printer should be displayed in the network explorer."
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool SLPDiscovery_Configuration_Changes(DiscoveryActivityData activityData, PluginSupport.Connectivity.ProtocolType protocol = PluginSupport.Connectivity.ProtocolType.IPv4)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            // validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameter is null!!");

                return false;
            }

            IPAddress newIPAddress = IPAddress.None;
            IPAddress ipv6Address = IPAddress.IPv6None;

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
            }
            if (!EwsWrapper.Instance().SetSLP(true))
            {
                return false;
            }

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                string serverIP = Printer.Printer.GetDHCPServerIP(IPAddress.Parse(activityData.IPv4Address)).ToString();
                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(serverIP);
                string scopeIP = serviceMethod.Channel.GetDhcpScopeIP(serverIP);
                // Create Reservation for the printer for DHCP in DHCP server
                if (serviceMethod.Channel.DeleteReservation(serverIP, scopeIP, activityData.IPv4Address, activityData.PrinterMacAddress) &&
                    serviceMethod.Channel.CreateReservation(serverIP, scopeIP, activityData.IPv4Address, activityData.PrinterMacAddress, ReservationType.Both))
                {
                    TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for DHCP : Succeeded");
                }
                else
                {
                    TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for DHCP : Failed");
                    return false;
                }
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step 1

                TraceFactory.Logger.Info("Step 01/04 - SLP after IP Address / Config Method Change");
                TraceFactory.Logger.Debug("Fetching the next available free IPv4 Address");

                newIPAddress = PluginSupport.Connectivity.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.IPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.IPv4Address));

                TraceFactory.Logger.Info("New IPv4 Address used for Windows Network Discovery is {0}".FormatWith(newIPAddress));
                TraceFactory.Logger.Info("Configuring Manual IPv4 Address:" + newIPAddress);

                if (!EwsWrapper.Instance().SetIPaddress(newIPAddress.ToString()))
                {
                    return false;
                }

                if (!BacabodDiscovery(newIPAddress.ToString(), true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                {
                    return false;
                }
                TraceFactory.Logger.Info("Reverting IP Configuration Method from Manual to DHCP");
                if (!EwsWrapper.Instance().SetIPConfigMethod(Printer.IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.IPv4Address)))
                {
                    return false;
                }

                // Step 2

                TraceFactory.Logger.Info("Step 02/04 - SLP Discovery after Hostname Change");
                if (!EwsWrapper.Instance().SetHostname("temporaryhostname"))
                {
                    return false;
                }

                if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                {
                    return false;
                }

                // Step 3
                if (!(activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString())) || activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {

                    TraceFactory.Logger.Info("Step 03/04 - SLP after IP Address / Config Method Change when SLP Client Only Mode Enabled");
                    if (!EwsWrapper.Instance().SetSLPClientMode(true))
                    {
                        return false;
                    }
                    TraceFactory.Logger.Debug("Fetching the next available free IPv4 Address");

                    newIPAddress = PluginSupport.Connectivity.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.IPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.IPv4Address));

                    TraceFactory.Logger.Info("New IPv4 Address used for Windows Network Discovery is {0}".FormatWith(newIPAddress));
                    TraceFactory.Logger.Info("Configuring Manual IPv4 Address:" + newIPAddress);

                    if (!EwsWrapper.Instance().SetIPaddress(newIPAddress.ToString()))
                    {
                        return false;
                    }

                    if (!BacabodDiscovery(newIPAddress.ToString(), true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info("Reverting IP Configuration Method from Manual to DHCP");
                    if (!EwsWrapper.Instance().SetIPConfigMethod(Printer.IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.IPv4Address)))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetSLPClientMode(false))
                    {
                        return false;
                    }

                }

                // Step 4 
                if (!(activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString())) || activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    if (!EwsWrapper.Instance().SetSLPClientMode(true))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info("Step 04/04 - SLP Discovery after Hostname Change when SLP Client Only Mode in Enabled");
                    EwsWrapper.Instance().SetHostname("temporaryhostname");

                    if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                    {
                        return false;
                    }

                    if (!EwsWrapper.Instance().SetSLPClientMode(false))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);

                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetIPConfigMethod(Printer.IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.IPv4Address));
                EwsWrapper.Instance().SetHostname("Default");
            }
        }

        #endregion

        #region SLPDicovery_MulticastIPv4

        /// <summary>
        /// Template ID:133361
        /// TEMPLATE Verify DUT SLP behavior when Multicast IPv4 is Enabled and Disabled.
        ///
        /// 1st step
        /// 1.bacabod -> Select IP address option.->Enter IP address of the printer ->Start Discovery
        /// 2.Printer should get discovered in BacaBod
        ///
        /// 2nd step:Printer discovery using BacaBod software over SLP when multicast IPV4 is disabled.
        /// 1.bacabod ->Select All devices option->Select SLP check box -> Start Discovery.
        /// 2.Printer should not get discovered in BacaBod
        ///
        /// 3rd step:Printer discovery using BacaBod software over SLP when multicast IPV4 is disabled.
        /// 1.bacabod ->Select All devices option->Select SLP check box ->Start Discovery.
        /// 2.Printer should get discovered in BacaBod
        ///
        /// 4th step:Printer discovery over SLP independent of multicast IPV4.
        /// 1.Disable Multicast IPv4 on the printer from web UI/telnet/SNMP.
        /// 2.Change the hostname of the printer from web UI/telnet/SNMP.
        /// 3.Printer should not send a probe match and not get discovered in vista with the new hostname
        ///
        /// 5th step:Printer discovery over SLP when multicast IPV4 is disabled.
        /// 1.Disable Multicast IPv4 on the printer from web UI/telnet/SNMP.
        /// 2.Change the hostname of the printer from web UI/telnet/SNMP.
        /// 3.Printer should not send a probe match over IPv4 and not get discovered in vista with the new hostname
        ///
        ///Note: Updated Test case based on Amitha input[Removed step 3 and step5 since it is similar to step2 and step4]
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool SLPDiscovery_MulticastIPv4(DiscoveryActivityData activityData)
        {
            bool flag;
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            // validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameters are null!!");
                return false;
            }

            if (!EwsWrapper.Instance().SetDHCPv6OnStartup(true))
            {
                return false;
            }
            if (!EwsWrapper.Instance().SetIPv6(false))
            {
                return false;
            }
            if (!EwsWrapper.Instance().SetIPv6(true))
            {
                return false;
            }
            if (!EwsWrapper.Instance().SetMulticastIPv4(true))
            {
                return false;
            }
            if (!EwsWrapper.Instance().SetSLP(true))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step 1

                TraceFactory.Logger.Info("Step 01/03 - SLP Discovery when Multicast IPv4 is enabled - Bacabod Tool");
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }

                if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                {
                    return false;
                }

                //Step 2

                TraceFactory.Logger.Info("Step 02/03 - SLP Discovery when Multicast IPv4 is disabled - Bacabod Tool");
                if (!EwsWrapper.Instance().SetMulticastIPv4(false))
                {
                    return false;
                }
                //Printer gets discovered if disocvery is unicast.. so even if multicast is disabled printer gets discovered
                flag = activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.LFP.ToString()) ? true : false;
                if (BacabodDiscovery(activityData.IPv4Address, flag, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                {
                    return false;
                }

                // Step 3
                //Printer gets discovered if disocvery is unicast.. so even if multicast is disabled printer gets discovered
                TraceFactory.Logger.Info("Step 03/03 - SLP Discovery when Multicast IPv4 is disabled and Hostname changed - Bacabod Tool");

                if (!EwsWrapper.Instance().SetMulticastIPv4(false))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetHostname("temporaryhostname"))
                {
                    return false;
                }
                if (BacabodDiscovery(activityData.IPv4Address, flag, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                {
                    return false;
                }

                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);

                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetMulticastIPv4(true);
                EwsWrapper.Instance().SetHostname("Default");
            }
        }

        #endregion

        #region SLPDiscovery_ColdReset

        /// <summary>
        /// Template ID:79284
        /// TEMPLATE Verify DUT SLP status after cold reset.
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Disable SLP/SLP Client Mode
        /// 4. Cold Reset
        /// 5. WSD should be enabled.
        /// 6. Printer should be discover in network explorer."
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool SLPDiscovery_ColdReset(DiscoveryActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            // validating the input parameters

            bool SLPClientModeOnlyOption = false;

            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameters are null!!");

                return false;
            }

            // create printer object
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
            }
            if (!EwsWrapper.Instance().SetSLP(true))
            {
                return false;
            }
            if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                TraceFactory.Logger.Info("Check if SNMP is enabled ");
                EwsWrapper.Instance().EnableSNMPAccess();
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step 1

                TraceFactory.Logger.Info("Step 01/04 - Disable SLP option from EWS and Cold Reset/Restore Factory Settings");
                if (!EwsWrapper.Instance().SetSLP(false))
                {
                    return false;
                }
                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    printer.ColdReset();
                    //CTSS
                    if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.TPS.ToString()))
                    {
                        if (!EwsWrapper.Instance().SetTelnet())
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (!EwsWrapper.Instance().SetFactoryDefaults())
                    {
                        return false;
                    }
                }

                TraceFactory.Logger.Debug("Validating SLP option from EWS");
                bool SLPDiscoveryOption = EwsWrapper.Instance().GetSLP();
                if (!SLPDiscoveryOption)
                {
                    TraceFactory.Logger.Info("Not as expected: SLP Discovery option is disabled after Cold Reset / Restore Factory Settings");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: SLP Discovery option is enabled after Cold Reset / Restore Factory Settings");
                }
                if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                {
                    return false;
                }

                // Step 2

                TraceFactory.Logger.Info("Step 02/04 - Enable SLP option from EWS and Cold Reset/Restore Factory Setttings");
                if (!EwsWrapper.Instance().SetSLP(true))
                {
                    return false;
                }

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    printer.ColdReset();
                    //CTSS
                    if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.TPS.ToString()))
                    {
                        if (!EwsWrapper.Instance().SetTelnet())
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (!EwsWrapper.Instance().SetFactoryDefaults())
                    {
                        return false;
                    }
                }

                TraceFactory.Logger.Debug("Validating SLP option from EWS");
                SLPDiscoveryOption = EwsWrapper.Instance().GetSLP();
                if (!SLPDiscoveryOption)
                {
                    TraceFactory.Logger.Info("Not as expected: SLP Discovery option is disabled after Cold Reset / Restore Factory Settings");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: SLP Discovery option is enabled after Cold Reset / Restore Factory Settings");
                }
                if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                {
                    return false;
                }

                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) || (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString())))
                {
                    TraceFactory.Logger.Info("Steps 03/04 & 04/04 Not Applicable for TPS and Inkjet Products");
                }

                // Step 3

                if ((!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString())) || (!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString())))
                {
                    TraceFactory.Logger.Info("Step 03/04 - Disable SLP option & SLP Client Mode option from EWS and Cold Reset/Restore Factory Settings");

                    if (!EwsWrapper.Instance().SetSLP(false))
                    {
                        return false;
                    }

                    if (!EwsWrapper.Instance().SetSLPClientMode(false))
                    {
                        return false;
                    }
                    printer.ColdReset();
                    TraceFactory.Logger.Debug("Validating SLP option & SLP Client Mode option from EWS");
                    SLPDiscoveryOption = EwsWrapper.Instance().GetSLP();
                    SLPClientModeOnlyOption = EwsWrapper.Instance().GetSLPClientMode();
                    if (!SLPDiscoveryOption)
                    {
                        TraceFactory.Logger.Info("Not as expected: SLP Discovery option is disabled after Cold Reset / Restore Factory Settings");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("As expected: SLP Discovery option is enabled after Cold Reset / Restore Factory Settings");
                    }
                    if (SLPClientModeOnlyOption)
                    {
                        TraceFactory.Logger.Info("Not as expected: SLP Client Mode option is enabled after Cold Reset / Restore Factory Settings");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("As expected: SLP Client Mode option is disabled after Cold Reset / Restore Factory Settings");
                    }
                }

                // Step 4
                if ((!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString())) || (!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString())))
                {
                    
                    TraceFactory.Logger.Info("Step 04/04 - Enable SLP option & SLP Client Mode option from EWS and Cold Reset/Restore Factory Setttings");
                    if (!EwsWrapper.Instance().SetSLP(true))
                    {
                        return false;
                    }
                    if (!EwsWrapper.Instance().SetSLPClientMode(true))
                    {
                        return false;
                    }

                printer.ColdReset();



                TraceFactory.Logger.Debug("Validating SLP option & SLP Client Mode option from EWS");
                SLPDiscoveryOption = EwsWrapper.Instance().GetSLP();
                SLPClientModeOnlyOption = EwsWrapper.Instance().GetSLPClientMode();

                if (!SLPDiscoveryOption)
                {
                    TraceFactory.Logger.Info("Not as expected: SLP Discovery option is disabled after Cold Reset / Restore Factory Settings");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: SLP Discovery option is enabled after Cold Reset / Restore Factory Settings");
                }

                    if (SLPClientModeOnlyOption)
                    {
                        TraceFactory.Logger.Info("Not as expected: SLP Client Mode option is enabled after Cold Reset / Restore Factory Settings");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("As expected: SLP Client Mode option is disabled after Cold Reset / Restore Factory Settings");
                    }
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Step 04/04 : It is not applicable for TPS and INK");
                    return true;

                }
            }
            catch (Exception discoveryexception)
            {
                TraceFactory.Logger.Info(discoveryexception.Message);

                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetSLP(true);
            }
        }

        #endregion

        #region SLPClientMode_Enable_Disable

        /// <summary>
        /// Enable and Disable SLP Client using SNMP/Telnet/EWS and validate packets.
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool SLPClientMode_Enable_Disable(DiscoveryActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetSLP(true))
                {
                    return false;
                }
                if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    TraceFactory.Logger.Info("Check if SNMP is enabled ");
                    EwsWrapper.Instance().EnableSNMPAccess();
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    // Step 1

                    TraceFactory.Logger.Info("Step 01/06 - SLP Client Mode Only - Disable from EWS");
                    if (!EwsWrapper.Instance().SetSLPClientMode(false))
                    {
                        return false;
                    }

                    if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                    {
                        return false;
                    }

                    // Step 2

                    TraceFactory.Logger.Info("Step 02/06 - SLP Client Mode Only - Enable from EWS");
                    if (!EwsWrapper.Instance().SetSLPClientMode(true))
                    {
                        return false;
                    }

                    if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                    {
                        return false;
                    }

                    // Step 3

                    TraceFactory.Logger.Info("Step 03/06 - SLP Client Mode Only - Disable from SNMP");
                    if (!SnmpWrapper.Instance().SetSLPClientMode(false))
                    {
                        return false;
                    }

                    if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                    {
                        return false;
                    }

                    // Step 4

                    TraceFactory.Logger.Info("Step 04/06 - SLP Client Mode Only - Enable from SNMP");
                    if (!SnmpWrapper.Instance().SetSLPClientMode(true))
                    {
                        return false;
                    }

                    if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                    {
                        return false;
                    }

                    // Step 5

                    TraceFactory.Logger.Info("Step 05/06 - SLP Client Mode Only - Disable from Telnet");
                    if (!TelnetWrapper.Instance().ToggleParameter(PrinterParameters.SLPCLIENTMODE, family, activityData.IPv4Address, false))
                    {
                        return false;
                    }

                    if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                    {
                        return false;
                    }

                    // Step 6

                    TraceFactory.Logger.Info("Step 06/06 - SLP Client Mode Only - Enable from Telnet");
                    if (!TelnetWrapper.Instance().ToggleParameter(PrinterParameters.SLPCLIENTMODE, family, activityData.IPv4Address, true))
                    {
                        return false;
                    }

                    if (!BacabodDiscovery(activityData.IPv4Address, true, BacaBodSourceType.IPAddress, BacaBodDiscoveryType.SLP))
                    {
                        return false;
                    }

                    return true;
                }
                catch (Exception discoveryException)
                {
                    TraceFactory.Logger.Info(discoveryException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    EwsWrapper.Instance().SetSLPClientMode(false);
                }
            }
        }

        #endregion

        #region BonjourDiscovery

        /// <summary>
        /// Bonjour Discovery
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool BonjourDiscovery(DiscoveryActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                    {
                        return false;
                    }
                }
                if (!EwsWrapper.Instance().SetBonjour(true))
                {
                    return false;
                }

                string bonjourServiceName = EwsWrapper.Instance().GetBonjourServiceName();
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    // Step 1

                    TraceFactory.Logger.Info("Step 01/01 - Bonjour Discovery");
                    if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), bonjourServiceName, true))
                    {
                        return false;
                    }
                    return true;
                }
                catch (Exception discoveryException)
                {
                    TraceFactory.Logger.Info(discoveryException.Message);
                    return false;
                }
            }
        }

        #endregion

        #region BonjourDiscovery_PowerCycle

        /// <summary>
        /// Template ID:79282
        /// TEMPLATE Verify DUT Bonjour status after reboot.
        /// 1st Step
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Disable 'Bonjour' from web UI
        /// 4. Power cycle
        /// 5. Check 'Bonjour' is disabled or not.
        /// 6. Printer should not be discovered.
        /// 
        /// 2nd Step
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Enable 'Bonjour' from web UI
        /// 4. Power cycle
        /// 5. Check 'Bonjour' is enabled or not.
        /// 6. Printer should discover.
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>
        public static bool BonjourDiscovery_PowerCycle(DiscoveryActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            //validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameter is null!!");

                return false;
            }

            // create printer object
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
            }
            if (!EwsWrapper.Instance().SetBonjour(true))
            {
                return false;
            }
            string defaultBonjourServiceName = EwsWrapper.Instance().GetBonjourServiceName();
            if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                TraceFactory.Logger.Info("Check if SNMP is enabled ");
                EwsWrapper.Instance().EnableSNMPAccess();
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step 1

                TraceFactory.Logger.Info("Step 01/02 - Disable Bonjour Discovery from EWS and Power Cycle");
                if (!EwsWrapper.Instance().SetBonjour(false))
                {
                    return false;
                }

                printer.PowerCycle();

                TraceFactory.Logger.Debug("Validating Bonjour option from EWS");
                bool bonjourDiscoveryOption = EwsWrapper.Instance().GetBonjour();

                if (bonjourDiscoveryOption)
                {
                    TraceFactory.Logger.Info("Not as expected: Bonjour Discovery option is enabled after Power Cycle");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: Bonjour Discovery option is disabled after Power Cycle");
                }

                if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), defaultBonjourServiceName, false))
                {
                    return false;
                }


                // Step 2
                TraceFactory.Logger.Info("Step 02/02 - Enable Bonjour Discovery option from EWS and Power Cycle");
                if (!EwsWrapper.Instance().SetBonjour(true))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetBonjourServiceName(defaultBonjourServiceName))
                {
                    return false;
                }

                printer.PowerCycle();

                TraceFactory.Logger.Debug("Validating Bonjour option from EWS");

                bonjourDiscoveryOption = EwsWrapper.Instance().GetBonjour();

                if (!bonjourDiscoveryOption)
                {
                    TraceFactory.Logger.Info("Not as expected: Bonjour Discovery option is disabled after Power Cycle");

                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: Bonjour Discovery option is enabled after Power Cycle");
                }

                if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), defaultBonjourServiceName, true))
                {
                    return false;
                }
                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);
                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetBonjour(true);
                EwsWrapper.Instance().SetBonjourServiceName(defaultBonjourServiceName);
            }
        }

        #endregion

        #region BonjourDiscovery_Cold Reset

        /// <summary>
        /// Template ID:79282
        /// TEMPLATE Verify DUT Bonjour status after cold reset.
        /// 1st Step
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Disable 'Bonjour' from web UI
        /// 4. Cold Reset
        /// 5. Check 'Bonjour' is disabled or not.
        /// 6. Printer should not be discovered.
        /// 
        /// 2nd Step
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Enable 'Bonjour' from web UI
        /// 4. Cold Reset
        /// 5. Check 'Bonjour' is enabled or not.
        /// 6. Printer should discover.
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>
        public static bool BonjourDiscovery_ColdReset(DiscoveryActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            //validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameter is null!!");

                return false;
            }

            // create printer object
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
            }
            if (!EwsWrapper.Instance().SetBonjour(true))
            {
                return false;
            }

            string defaultBonjourServiceName = EwsWrapper.Instance().GetBonjourServiceName();
            if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                TraceFactory.Logger.Info("Check if SNMP is enabled ");
                EwsWrapper.Instance().EnableSNMPAccess();
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step 1

                TraceFactory.Logger.Info("Step 01/02 - Disable Bonjour Discovery from EWS and Cold Reset / Restore Factory Settings");

                if (!EwsWrapper.Instance().SetBonjour(false))
                {
                    return false;
                }

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    printer.ColdReset();
                }
                else
                {
                    if (!EwsWrapper.Instance().SetFactoryDefaults())
                    {
                        return false;
                    }
                }

                TraceFactory.Logger.Debug("Validating Bonjour option from EWS");
                bool BonjourDiscoveryOption = EwsWrapper.Instance().GetBonjour();

                if (!BonjourDiscoveryOption)
                {
                    TraceFactory.Logger.Info("Not as expected: Bonjour Discovery option is disabled after Cold Reset / Restore Factory Settings");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: Bonjour Discovery option is enabled after Cold Reset / Restore Factory Settings");
                }

                if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), defaultBonjourServiceName, true))
                {
                    return false;
                }


                // Step 2
                TraceFactory.Logger.Info("Step 02/02 - Enable Bonjour Discovery option from EWS and Cold Reset / Restore Factory Settings");
                if (!EwsWrapper.Instance().SetBonjour(true))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetBonjourServiceName(defaultBonjourServiceName))
                {
                    return false;
                }

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    printer.ColdReset();
                }
                else
                {
                    if (!EwsWrapper.Instance().SetFactoryDefaults())
                    {
                        return false;
                    }
                }

                TraceFactory.Logger.Debug("Validating Bonjour option from EWS");

                BonjourDiscoveryOption = EwsWrapper.Instance().GetBonjour();

                if (!BonjourDiscoveryOption)
                {
                    TraceFactory.Logger.Info("Not as expected: Bonjour Discovery option is disabled after Cold Reset / Restore Factory Settings");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: Bonjour Discovery option is enabled after Cold Reset / Restore Factory Settings");
                }

                if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), defaultBonjourServiceName, true))
                {
                    return false;
                }
                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);
                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetBonjour(true);
                EwsWrapper.Instance().SetBonjourServiceName(defaultBonjourServiceName);
            }
        }

        #endregion

        #region BonjourDiscovery_Enable_Disable

        /// <summary>
        /// Enable and Disable Bonjour Discovery using SNMP/Telnet/EWS and validate discovery.
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool BonjourDiscovery_Enable_Disable(DiscoveryActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                    {
                        return false;
                    }
                }
                if (!EwsWrapper.Instance().SetBonjour(true))
                {
                    return false;
                }
                if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    TraceFactory.Logger.Info("Check if SNMP is enabled ");
                    EwsWrapper.Instance().EnableSNMPAccess();
                }
                string defaultBonjourServiceName = EwsWrapper.Instance().GetBonjourServiceName();

                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    // Step 1

                    TraceFactory.Logger.Info("Step 01/06 - Bonjour Discovery - Disable from EWS");
                    if (!EwsWrapper.Instance().SetBonjour(false))
                    {
                        return false;
                    }

                    if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), defaultBonjourServiceName, false))
                    {
                        return false;
                    }

                    // Step 2

                    TraceFactory.Logger.Info("Step 02/06 - Bonjour Discovery - Enable from EWS");
                    if (!EwsWrapper.Instance().SetBonjour(true))
                    {
                        return false;
                    }
                    if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), defaultBonjourServiceName, true))
                    {
                        return false;
                    }

                    // Step 3

                    TraceFactory.Logger.Info("Step 03/06 - Bonjour Discovery - Disable from SNMP");
                    if (!SnmpWrapper.Instance().SetBonjour(false))
                    {
                        return false;
                    }

                    Thread.Sleep(TimeSpan.FromMinutes(2));

                    if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), defaultBonjourServiceName, false))
                    {
                        return false;
                    }

                    // Step 4

                    TraceFactory.Logger.Info("Step 04/06 - Bonjour Discovery - Enable from SNMP");
                    if (!SnmpWrapper.Instance().SetBonjour(true))
                    {
                        return false;
                    }

                    Thread.Sleep(TimeSpan.FromMinutes(2));

                    if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), defaultBonjourServiceName, true))
                    {
                        return false;
                    }

                    if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                    {
                        TraceFactory.Logger.Info("Steps 05/06 & 06/06 Not Applicable for Inkjet Products");
                    }

                    // Step 5
                    //CTSS
                    if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()) || !activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.TPS.ToString()))
                    {
                        TraceFactory.Logger.Info("Step 05/06 - Bonjour Discovery - Disable from Telnet");
                        if (!TelnetWrapper.Instance().ToggleParameter(PrinterParameters.BONJOUR, family, activityData.IPv4Address, false))
                        {
                            return false;
                        }

                        Thread.Sleep(TimeSpan.FromMinutes(2));

                        if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), defaultBonjourServiceName, false))
                        {
                            return false;
                        }

                        // Step 6

                        TraceFactory.Logger.Info("Step 06/06 - Bonjour Discovery - Enable from Telnet");
                        if (!TelnetWrapper.Instance().ToggleParameter(PrinterParameters.BONJOUR, family, activityData.IPv4Address, true))
                        {
                            return false;
                        }

                        Thread.Sleep(TimeSpan.FromMinutes(2));

                        if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), defaultBonjourServiceName, true))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                catch (Exception discoveryException)
                {
                    TraceFactory.Logger.Info(discoveryException.Message);

                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    EwsWrapper.Instance().SetBonjour(true);
                }
            }
        }

        #endregion

        #region BonjourDiscovery_Configuration_Changes

        /// <summary>
        /// Template ID:79212
        /// TEMPLATE Verify Bonjour Discovery over configuration changes
        /// 1st Step
        /// 1. Change the IP address
        /// 2. Printer should be displayed on the network explorer.
        /// 2nd Step
        /// 1. Change the host name
        /// 2. Printer should be displayed in the network explorer."
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool BonjourDiscovery_Configuration_Changes(DiscoveryActivityData activityData, PluginSupport.Connectivity.ProtocolType protocol = PluginSupport.Connectivity.ProtocolType.IPv4)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            // validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameter is null!!");

                return false;
            }

            IPAddress newIPAddress = IPAddress.None;
            IPAddress ipv6Address = IPAddress.IPv6None;

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
            }
            if (!EwsWrapper.Instance().SetBonjour(true))
            {
                return false;
            }

            string defaultBonjourServiceName = EwsWrapper.Instance().GetBonjourServiceName();
            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                string serverIP = Printer.Printer.GetDHCPServerIP(IPAddress.Parse(activityData.IPv4Address)).ToString();
                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(serverIP);
                string scopeIP = serviceMethod.Channel.GetDhcpScopeIP(serverIP);

            // Create Reservation for the printer for DHCP in DHCP server
            if (serviceMethod.Channel.DeleteReservation(serverIP, scopeIP, activityData.IPv4Address, activityData.PrinterMacAddress) &&
                serviceMethod.Channel.CreateReservation(serverIP, scopeIP, activityData.IPv4Address, activityData.PrinterMacAddress, ReservationType.Both))
            {
                TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for DHCP : Succeeded");
            }
            else
            {
                TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for DHCP : Failed");
                return false;
            }

            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step 1

                TraceFactory.Logger.Info("Step 01/02 - Bonjour after IP Address / Config Method Change");
                TraceFactory.Logger.Debug("Fetching the next available free IPv4 Address");

                newIPAddress = PluginSupport.Connectivity.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.IPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.IPv4Address));

                TraceFactory.Logger.Info("New IPv4 Address used for Windows Network Discovery is {0}".FormatWith(newIPAddress));
                TraceFactory.Logger.Info("Configuring Manual IPv4 Address:" + newIPAddress);

                if (!EwsWrapper.Instance().SetIPaddress(newIPAddress.ToString()))
                {
                    return false;
                }

                if (!BonjourDiscovery(newIPAddress, defaultBonjourServiceName, true))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Reverting IP Configuration Method from Manual to DHCP");

                if (!EwsWrapper.Instance().SetIPConfigMethod(Printer.IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.IPv4Address)))
                {
                    return false;
                }
                // Step 2

                TraceFactory.Logger.Info("Step 02/02 - Bonjour Discovery after Hostname Change");

                if (!EwsWrapper.Instance().SetHostname("temporaryhostname"))
                {
                    return false;
                }

                if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), defaultBonjourServiceName, true))
                {
                    return false;
                }
                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);

                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetIPConfigMethod(Printer.IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.IPv4Address));
                EwsWrapper.Instance().SetHostname("Default");
            }
        }

        #endregion

        #region BonjourDiscovery_MulticastIPv4

        /// <summary>
        /// BonjourDicovery - Enable and Disable Multicast IPv4
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool BonjourDiscovery_MulticastIPv4(DiscoveryActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            // validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameters are null!!");
                return false;
            }

            if (!EwsWrapper.Instance().SetDHCPv6OnStartup(true))
            {
                return false;
            }
            if (!EwsWrapper.Instance().SetIPv6(false))
            {
                return false;
            }
            if (!EwsWrapper.Instance().SetIPv6(true))
            {
                return false;
            }
            if (!EwsWrapper.Instance().SetMulticastIPv4(true))
            {
                return false;
            }
            if (!EwsWrapper.Instance().SetBonjour(true))
            {
                return false;
            }

            string BonjourServiceName = EwsWrapper.Instance().GetBonjourServiceName();
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step 1

                TraceFactory.Logger.Info("Step 01/01 - Bonjour Discovery when Multicast IPv4 is enabled");
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
                if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), BonjourServiceName, true))
                {
                    return false;
                }

                //Step 2

                TraceFactory.Logger.Info("Step 02/02 - Bonjour Discovery when Multicast IPv4 is disabled");
                if (!EwsWrapper.Instance().SetMulticastIPv4(false))
                {
                    return false;
                }
                if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), BonjourServiceName, false))
                {
                    return false;
                }

                // Step 3

                TraceFactory.Logger.Info("Step 03/03 - Bonjour Discovery when Multicast IPv4 is disabled and Hostname changed");

                if (!EwsWrapper.Instance().SetMulticastIPv4(false))
                {
                    return false;
                }
                if (!EwsWrapper.Instance().SetHostname("temporaryhostname"))
                {
                    return false;
                }
                if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), BonjourServiceName, false))
                {
                    return false;
                }
                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);
                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetMulticastIPv4(true);
                EwsWrapper.Instance().SetHostname("Default");
            }
        }

        #endregion

        #region BonjourServiceName_PowerCycle

        /// <summary>
        /// Template ID:79282
        /// TEMPLATE Verify Bonjour Service Name after reboot.
        /// 1st Step
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Disable 'WS Discovery' from web UI
        /// 4. Power cycle
        /// 5. Check WS Discovery is disabled or not.
        /// 6. Printer should not be discovered in network explorer.

        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>
        public static bool BonjourServiceName_PowerCycle(DiscoveryActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            //validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameter is null!!");

                return false;
            }

            // create printer object
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
            }
            if (!EwsWrapper.Instance().SetBonjour(true))
            {
                return false;
            }
            string defaultBonjourServiceName = EwsWrapper.Instance().GetBonjourServiceName();
            if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                TraceFactory.Logger.Info("Check if SNMP is enabled ");
                EwsWrapper.Instance().EnableSNMPAccess();
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step 1
                TraceFactory.Logger.Info("Step 01/01 - Set Bonjour Service Name from EWS and Power Cycle");
                string newBonjourServiceName = "newbonjourname";
                if (!EwsWrapper.Instance().SetBonjourServiceName(newBonjourServiceName))
                {
                    return false;
                }
                printer.PowerCycle();

                TraceFactory.Logger.Debug("Validating Bonjour Service Name from EWS");

                if (EwsWrapper.Instance().GetBonjourServiceName() != newBonjourServiceName)
                {
                    TraceFactory.Logger.Info("Not as expected: Bonjour Service Name is not retained after Power Cycle");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: Bonjour Service Name is retained after Power Cycle");
                }
                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);
                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetBonjourServiceName(defaultBonjourServiceName);
            }
        }

        #endregion

        #region BonjourServiceName_ColdReset

        /// <summary>
        /// Template ID:79282
        /// TEMPLATE Verify Bonjour Service Name after cold reset.
        /// 1st Step
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Disable 'WS Discovery' from web UI
        /// 4. Power cycle
        /// 5. Check WS Discovery is disabled or not.
        /// 6. Printer should not be discovered in network explorer.
        /// 
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>
        public static bool BonjourServiceName_ColdReset(DiscoveryActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            //validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameter is null!!");
                return false;
            }

            // create printer object
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
            }
            if (!EwsWrapper.Instance().SetBonjour(true))
            {
                return false;
            }

            string defaultBonjourServiceName = EwsWrapper.Instance().GetBonjourServiceName();
            if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                TraceFactory.Logger.Info("Check if SNMP is enabled ");
                EwsWrapper.Instance().EnableSNMPAccess();
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step 1
                TraceFactory.Logger.Info("Step 01/01 - Set Bonjour Service Name from EWS and Cold Reset/Restore Factory Settings");
                string newBonjourServiceName = "newbonjourname";

                if (!EwsWrapper.Instance().SetBonjourServiceName(newBonjourServiceName))
                {
                    return false;
                }
                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    printer.ColdReset();
                }
                else
                {
                    if (!EwsWrapper.Instance().SetFactoryDefaults())
                    {
                        return false;
                    }
                }
                TraceFactory.Logger.Debug("Validating Bonjour Service Name from EWS");

                if (EwsWrapper.Instance().GetBonjourServiceName() != defaultBonjourServiceName)
                {
                    TraceFactory.Logger.Info("Not as expected: Bonjour Service Name is not reverted to default after Cold Reset");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: Bonjour Service Name is reverted to default after Cold Reset");
                }
                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);

                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetBonjourServiceName(defaultBonjourServiceName);
            }
        }

        #endregion

        #region BonjourHighestService_PowerCycle

        /// <summary>
        /// Template ID:79282
        /// TEMPLATE Verify Bonjour Service Priority after reboot.
        /// 1st Step
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Disable 'WS Discovery' from web UI
        /// 4. Power cycle
        /// 5. Check WS Discovery is disabled or not.
        /// 6. Printer should not be discovered in network explorer.

        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>
        public static bool BonjourHighestService_PowerCycle(DiscoveryActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            //validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameter is null!!");

                return false;
            }

            // create printer object
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
            }
            if (!EwsWrapper.Instance().SetBonjour(true))
            {
                return false;
            }

            BonjourHighestService defaultHighestBonjourService = new BonjourHighestService();

            if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.LFP.ToString()))
            {
                //defaultHighestBonjourService = BonjourHighestService.PrintingLPDBINPS;
                if (EwsWrapper.Instance().GetIPPS())
                {
                    defaultHighestBonjourService = BonjourHighestService.PrintingIPPS;
                }
                else if (EwsWrapper.Instance().GetIPP())
                {
                    defaultHighestBonjourService = BonjourHighestService.PrintingIPP;
                }
                else
                {
                    defaultHighestBonjourService = BonjourHighestService.PrintingLPDBINPS;
                }
            }
			else if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                TraceFactory.Logger.Info("Check if SNMP is enabled ");
                EwsWrapper.Instance().EnableSNMPAccess();
            }    	
            else
            {
                defaultHighestBonjourService = BonjourHighestService.PrintingIPP;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step  1
                TraceFactory.Logger.Info("Step 01/01 - Set Bonjour Highest Precedence from EWS and Power Cycle");
                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    if (!EwsWrapper.Instance().SetBonjourHighestService(BonjourHighestService.PrintingLPDRAW))
                    {
                        return false;
                    }
                    printer.PowerCycle();

                    TraceFactory.Logger.Debug("Validating Bonjour Highest Service from EWS");

                    if (EwsWrapper.Instance().GetBonjourHighestService() != BonjourHighestService.PrintingLPDRAW)
                    {
                        TraceFactory.Logger.Info("Not as expected: Bonjour Highest Service is not retained after Power Cycle");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("As expected: Bonjour Highest Service is retained after Power Cycle");
                    }
                }
                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);
                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetBonjourHighestService(defaultHighestBonjourService);
            }
        }

        #endregion

        #region BonjourHighestService_ColdReset

        /// <summary>
        /// Template ID:79282
        /// TEMPLATE Verify Bonjour Service Priority after cold reset.
        /// 1st Step
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Disable 'WS Discovery' from web UI
        /// 4. Cold Reset
        /// 5. Check WS Discovery is disabled or not.
        /// 6. Printer should not be discovered in network explorer.
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>
        public static bool BonjourHighestService_ColdReset(DiscoveryActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            //validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameter is null!!");

                return false;
            }

            // create printer object
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
            }
            if (!EwsWrapper.Instance().SetBonjour(true))
            {
                return false;
            }

            BonjourHighestService defaultHighestBonjourService = new BonjourHighestService();
            if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
            {
                TraceFactory.Logger.Info("Check if SNMP is enabled ");
                EwsWrapper.Instance().EnableSNMPAccess();
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step-1
                TraceFactory.Logger.Info("Step 01 / 01 - Set Bonjour Highest Precedence from EWS and Cold Reset/Restore Factory Settings");
                if (EwsWrapper.Instance().GetIPPS())
                {
                    defaultHighestBonjourService = BonjourHighestService.PrintingIPPS;
                }
                else if (EwsWrapper.Instance().GetIPP())
                {
                    defaultHighestBonjourService = BonjourHighestService.PrintingIPP;
                }
                else
                {
                    defaultHighestBonjourService = BonjourHighestService.PrintingLPDBINPS;
                }

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    if (!EwsWrapper.Instance().SetBonjourHighestService(BonjourHighestService.PrintingLPDRAW))
                    {
                        return false;
                    }
                    printer.ColdReset();
                }
                else
                {
                    if (!EwsWrapper.Instance().SetFactoryDefaults())
                    {
                        return false;
                    }
                }
                TraceFactory.Logger.Debug("Validating Bonjour Highest Service from EWS");

                if (EwsWrapper.Instance().GetBonjourHighestService() != defaultHighestBonjourService)
                {
                    TraceFactory.Logger.Info("Not as expected: Bonjour Highest Service is not reverted to default after Cold Reset");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: Bonjour Highest Service is reverted to default after Cold Reset");
                }
                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);

                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetBonjourHighestService(defaultHighestBonjourService);
            }
        }

        #endregion

        #region BonjourServiceNames

        /// <summary>
        /// Enable and Disable SLP Client using SNMP/Telnet/EWS and validate packets.
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool BonjourServiceNames(DiscoveryActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                EwsWrapper.Instance().SetAdvancedOptions();
                // create printer object
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                    {
                        return false;
                    }
                }
                if (!EwsWrapper.Instance().SetBonjour(true))
                {
                    return false;
                }

                string defaultBonjourServiceName = EwsWrapper.Instance().GetBonjourServiceName();
                if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    TraceFactory.Logger.Info("Check if SNMP is enabled ");
                    EwsWrapper.Instance().EnableSNMPAccess();
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    string bonjourServiceName = string.Empty;
                    // Step 1

                    TraceFactory.Logger.Info("Step 01/10 - Bonjour Service Name - Unicode Characters - Set from EWS");
                    bonjourServiceName = "கபாலி டா";
                    if (!EwsWrapper.Instance().SetBonjourServiceName(bonjourServiceName))
                    {
                        return false;
                    }

                    printer.PowerCycle();

                    TraceFactory.Logger.Debug("Validating Bonjour Service Name from EWS");
                    if (EwsWrapper.Instance().GetBonjourServiceName() == bonjourServiceName)
                    {
                        TraceFactory.Logger.Info("As expected: Bonjour Service Name is retained after Power Cycle");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Not as expected: Bonjour Service Name is not retained after Power Cycle");
                        return false;
                    }

                    if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), bonjourServiceName, true))
                    {
                        return false;
                    }

                    // Step 2

                    TraceFactory.Logger.Info("Step 02/10 - Bonjour Service Name - Blank - Set from EWS");
                    bonjourServiceName = string.Empty;
                                   
                    if(EwsWrapper.Instance().SetBonjourServiceName(bonjourServiceName))
                    {
                        TraceFactory.Logger.Info("As expected: Bonjour Service Name is not set to blank.");
                      
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Bonjour Service Name is set to blank.");
                        return false;
                    }
                    // Step 3

                    TraceFactory.Logger.Info("Step 03/10 - Bonjour Service Name - Minimum Character - Set from EWS");
                    bonjourServiceName = "x";
                    if (!EwsWrapper.Instance().SetBonjourServiceName(bonjourServiceName))
                    {
                        return false;
                    }
                    printer.PowerCycle();
                    TraceFactory.Logger.Debug("Validating Bonjour Service Name from EWS");
                    if (EwsWrapper.Instance().GetBonjourServiceName() == bonjourServiceName)
                    {
                        TraceFactory.Logger.Info("As expected: Bonjour Service Name is retained after Power Cycle");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Not as expected: Bonjour Service Name is not retained after Power Cycle");
                        return false;
                    }

                    if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), bonjourServiceName, true))
                    {
                        return false;
                    }

                    // Step 4

                    TraceFactory.Logger.Info("Step 04/10 - Bonjour Service Name - Maximum Characters - Set from EWS");
                    bonjourServiceName = "123456789012345678901234567890123456789012345678901234567890123";
                    if (!EwsWrapper.Instance().SetBonjourServiceName(bonjourServiceName))
                    {
                        return false;
                    }
                    printer.PowerCycle();
                    TraceFactory.Logger.Debug("Validating Bonjour Service Name from EWS");
                    if (EwsWrapper.Instance().GetBonjourServiceName() == bonjourServiceName)
                    {
                        TraceFactory.Logger.Info("As expected: Bonjour Service Name is retained after Power Cycle");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Not as expected: Bonjour Service Name is not retained after Power Cycle");
                        return false;
                    }

                    if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), bonjourServiceName, true))
                    {
                        return false;
                    }

                    //Step 5

                    TraceFactory.Logger.Info("Step 05/10 - Bonjour Service Name - Blank - Set through SNMP");
                    bonjourServiceName = string.Empty;
                    if (SnmpWrapper.Instance().SetBonjourServiceName(bonjourServiceName))
                    {
                        return false;
                    }                    

                    // Step 6

                    TraceFactory.Logger.Info("Step 06/10 - Bonjour Service Name - Minimum Character - Set through SNMP");
                    bonjourServiceName = "x";
                    if (!SnmpWrapper.Instance().SetBonjourServiceName(bonjourServiceName))
                    {
                        return false;
                    }
                    printer.PowerCycle();
                    TraceFactory.Logger.Debug("Validating Bonjour Service Name from SNMP");
                    if (SnmpWrapper.Instance().GetBonjourServiceName() == bonjourServiceName)
                    {
                        TraceFactory.Logger.Info("As expected: Bonjour Service Name is retained after Power Cycle");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Not as expected: Bonjour Service Name is not retained after Power Cycle");
                        return false;
                    }
                    if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), bonjourServiceName, true))
                    {
                        return false;
                    }

                    // Step 7

                    TraceFactory.Logger.Info("Step 07/10 - Bonjour Service Name - Maximum Characters - Set through SNMP");
                    bonjourServiceName = "123456789012345678901234567890123456789012345678901234567890123";
                    if (!SnmpWrapper.Instance().SetBonjourServiceName(bonjourServiceName))
                    {
                        return false;
                    }
                    printer.PowerCycle();
                    TraceFactory.Logger.Debug("Validating Bonjour Service Name from SNMP");
                    if (SnmpWrapper.Instance().GetBonjourServiceName() == bonjourServiceName)
                    {
                        TraceFactory.Logger.Info("As expected: Bonjour Service Name is retained after Power Cycle");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Not as expected: Bonjour Service Name is not retained after Power Cycle");
                        return false;
                    }

                    if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), bonjourServiceName, true))
                    {
                        return false;
                    }

                    if (activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                    {
                        TraceFactory.Logger.Info("Steps 08/10 , 09/10 & 10/10 Not Applicable for Inkjet printers");
                    }

                    //CTSS
                    if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()) || !activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.TPS.ToString()))
                    {
                        // Step 8
                        TraceFactory.Logger.Info("Step 08/10 - Bonjour Service Name - Blank - Set through Telnet");
                        bonjourServiceName = string.Empty;
                        if (TelnetWrapper.Instance().SetParameter(PrinterParameters.BONJOURSERVICENAME, family, activityData.IPv4Address, bonjourServiceName))
                        {
                            return false;
                        }
                        
                        // Step 9

                        TraceFactory.Logger.Info("Step 09/10 - Bonjour Service Name - Minimum Character - Set through Telnet");
                        bonjourServiceName = "x";
                        if (!TelnetWrapper.Instance().SetParameter(PrinterParameters.BONJOURSERVICENAME, family, activityData.IPv4Address, bonjourServiceName))
                        {
                            return false;
                        }
                        printer.PowerCycle();
                        TraceFactory.Logger.Debug("Validating Bonjour Service Name from Telnet");
                        if (TelnetWrapper.Instance().GetConfiguredValue(family, PrinterParameters.BONJOURSERVICENAME.ToString()) == bonjourServiceName)
                        {
                            TraceFactory.Logger.Info("As expected: Bonjour Service Name is retained after Power Cycle");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Not as expected: Bonjour Service Name is not retained after Power Cycle");
                            return false;
                        }
                        if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), bonjourServiceName, true))
                        {
                            return false;
                        }

                        // Step 10

                        TraceFactory.Logger.Info("Step 10/10 - Bonjour Service Name - Maximum Characters - Set through Telnet");
                        bonjourServiceName = "123456789012345678901234567890123456789012345678901234567890123";
                        if (!TelnetWrapper.Instance().SetParameter(PrinterParameters.BONJOURSERVICENAME, family, activityData.IPv4Address, bonjourServiceName))
                        {
                            return false;
                        }
                        printer.PowerCycle();
                        TraceFactory.Logger.Debug("Validating Bonjour Service Name from Telnet");
                        if (TelnetWrapper.Instance().GetConfiguredValue(family, PrinterParameters.BONJOURSERVICENAME.ToString()) == bonjourServiceName)
                        {
                            TraceFactory.Logger.Info("As expected: Bonjour Service Name is retained after Power Cycle");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Not as expected: Bonjour Service Name is not retained after Power Cycle");
                            return false;
                        }
                        if (!BonjourDiscovery(IPAddress.Parse(activityData.IPv4Address), bonjourServiceName, true))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                catch (Exception discoveryException)
                {
                    TraceFactory.Logger.Info(discoveryException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    EwsWrapper.Instance().SetBonjourServiceName(defaultBonjourServiceName);
                }
            }
        }

        #endregion

        #region DuplicateBonjourServiceName

        /// <summary>
        /// Bonjour Discovery
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool DuplicateBonjourServiceName(DiscoveryActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                    {
                        return false;
                    }
                }
                if (!EwsWrapper.Instance().SetBonjour(true))
                {
                    return false;
                }

                string defaultBonjourServiceName = EwsWrapper.Instance().GetBonjourServiceName();

                IPAddress[] localAddresses = NetworkUtil.GetLocalAddresses().Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToArray();
                Dictionary<IPAddress, string> networkDetails = new Dictionary<IPAddress, string>();

                foreach (var item in localAddresses)
                {
                    networkDetails.Add(item, CtcUtility.GetClientNetworkName(item.ToString()));
                }

                IPAddress localAddress = networkDetails.Where(x => x.Key.IsInSameSubnet(IPAddress.Parse(activityData.IPv4Address))).FirstOrDefault().Key;

                // Disable all the NICs except the one with same subnet as that of printer
                foreach (var item in networkDetails.Where(x => !(x.Key.Equals(localAddress))))
                {
                    NetworkUtil.DisableNetworkConnection(item.Value.ToString());
                }

                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    // Step 1

                    TraceFactory.Logger.Info("Step 01/01 - Duplicate Bonjour Name");
                    string duplicateBonjourServiceName = string.Empty;
                    XmlDocument configFile = new XmlDocument();

                    foreach (string lines in File.ReadLines(BonjourDiscovery()))
                    {
                        if (lines.Contains("HP") && (!lines.Contains(defaultBonjourServiceName)))
                        {
                            string[] parts = lines.Split('.');
                            duplicateBonjourServiceName = parts[4].Trim();
                            TraceFactory.Logger.Info("Duplicate Bonjour Service name to be used: {0}".FormatWith(duplicateBonjourServiceName));
                            break;
                        }
                    }
                    EwsWrapper.Instance().SetBonjourServiceName(duplicateBonjourServiceName);

                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    string newBonjourServiceName = EwsWrapper.Instance().GetBonjourServiceName();

                    if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                    {
                        if (!(newBonjourServiceName == duplicateBonjourServiceName))
                        {
                            TraceFactory.Logger.Info("As expected: Duplicate Bonjour Service name not set. Bonjour Service Name is: {0}".FormatWith(newBonjourServiceName));
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Not as expected: Duplicate Bonjour Service name is set. Bonjour Service Name is: {0}".FormatWith(newBonjourServiceName));
                            return false;
                        }
                    }
                    else
                    {
                        if (newBonjourServiceName == string.Concat(duplicateBonjourServiceName, " [", activityData.PrinterMacAddress.Substring(6).ToUpperInvariant(), "]"))
                        {
                            TraceFactory.Logger.Info("As expected: Duplicate Bonjour Service name not set. Bonjour Service Name is: {0}".FormatWith(newBonjourServiceName));
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Not as expected: Duplicate Bonjour Service name is set. Bonjour Service Name is: {0}".FormatWith(newBonjourServiceName));
                            return false;
                        }
                    }
                    return true;
                }
                catch (Exception discoveryException)
                {
                    TraceFactory.Logger.Info(discoveryException.Message);

                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    foreach (var item in networkDetails)
                    {
                        NetworkUtil.EnableNetworkConnection(item.Value);
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(30));
                    EwsWrapper.Instance().SetBonjourServiceName(defaultBonjourServiceName);
                }
            }
        }

        #endregion

        #region BonjourHighestPriorities

        /// <summary>
        /// Validate different Bonjour Highest Priorities from EWS/Telnet/SNMP
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool BonjourHighestPriorities(DiscoveryActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                // create printer object
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

                if (!EwsWrapper.Instance().SetBonjour(true))
                {
                    return false;
                }

                BonjourHighestService defaultHighestBonjourService = new BonjourHighestService();
                string getValue = null;

                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.LFP.ToString()))
                {
                    if (EwsWrapper.Instance().GetIPPS())
                    {
                        defaultHighestBonjourService = BonjourHighestService.PrintingIPPS;
                    }
                    else if (EwsWrapper.Instance().GetIPP())
                    {
                        defaultHighestBonjourService = BonjourHighestService.PrintingIPP;
                    }
                    else
                    {
                        defaultHighestBonjourService = BonjourHighestService.PrintingLPDBINPS;
                    }
                }
                else
                {
                    defaultHighestBonjourService = BonjourHighestService.PrintingIPP;
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                    // Step 1

                    TraceFactory.Logger.Info("Step 01/01 - Bonjour Highest Priority - P9100 - Set from EWS");
                    if (!ValidateBonjourHighestPriorityEWS(BonjourHighestService.Printing9100))
                    {
                        return false;
                    }

                    // Step 1b

                    TraceFactory.Logger.Info("Step:01b - Bonjour Highest Priority - IPP - Set from EWS");
                    if (!ValidateBonjourHighestPriorityEWS(BonjourHighestService.PrintingIPP))
                    {
                        return false;
                    }

                    if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                    {
                        // Step 1c

                        TraceFactory.Logger.Info("Step:01c - Bonjour Highest Priority - LPD(RAW) - Set from EWS");
                        if (!ValidateBonjourHighestPriorityEWS(BonjourHighestService.PrintingLPDRAW))
                        {
                            return false;
                        }
                    }

                    if (!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                    // Step 1d
                    {
                        if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
             			{
                            if (EwsWrapper.Instance().GetIPPS())
                            {
                                TraceFactory.Logger.Info("Step:01d - Bonjour Highest Priority - IPPS - Set from EWS");
                                if (!ValidateBonjourHighestPriorityEWS(BonjourHighestService.PrintingIPPS))
                                {
                                    return false;
                                }
                            }

                            // Step 1e

                            TraceFactory.Logger.Info("Step:01e - Bonjour Highest Priority - LPD(TEXT) - Set from EWS");
                            if (!ValidateBonjourHighestPriorityEWS(BonjourHighestService.PrintingLPDTEXT))
                            {
                                return false;
                            }

                            // Step 1f

                            TraceFactory.Logger.Info("Step:01f - Bonjour Highest Priority - LPD(AUTO) - Set from EWS");
                            if (!ValidateBonjourHighestPriorityEWS(BonjourHighestService.PrintingLPDAUTO))
                            {
                                return false;
                            }

                            // Step 1g

                            //TraceFactory.Logger.Info("Step:01g - Bonjour Highest Priority - LPD(BINPS) - Set from EWS");
                            //if (!ValidateBonjourHighestPriorityEWS(BonjourHighestService.PrintingLPDBINPS))
                            //{
                            //    return false;
                            //}
                        }

                    }

                    // Step 2a
                    //CTSS
                    if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()) || !activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.TPS.ToString()))
                    {

                        TraceFactory.Logger.Info("Step:02a - Bonjour Highest Priority - P9100 - Set from Telnet");
                        if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                        {
                            getValue = "9100 printing";
                        }

                        if (!ValidateBonjourHighestPriorityTelnet(activityData, PrinterParameters.BONJOURHIGHESTPRIORITY, family, BonjourHighestService.Printing9100, (int)BonjourHighestService.Printing9100, getValue))
                        {
                            return false;
                        }

                        // Step 2b

                        TraceFactory.Logger.Info("Step:02b - Bonjour Highest Priority - IPP - Set from Telnet");
                        if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                        {
                            getValue = "IPP printing";
                        }

                        if (!ValidateBonjourHighestPriorityTelnet(activityData, PrinterParameters.BONJOURHIGHESTPRIORITY, family, BonjourHighestService.PrintingIPP, (int)BonjourHighestService.PrintingIPP, getValue))
                        {
                            return false;
                        }
                    }

                    // Step 2c

                    if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                    {
                        TraceFactory.Logger.Info("Step:02c - Bonjour Highest Priority - LPD(RAW) - Set from Telnet");
                        if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                        {
                            getValue = "LPD RAW printing";
                        }

                        if (!ValidateBonjourHighestPriorityTelnet(activityData, PrinterParameters.BONJOURHIGHESTPRIORITY, family, BonjourHighestService.PrintingLPDRAW, (int)BonjourHighestService.PrintingLPDRAW, getValue))
                        {
                            return false;
                        }
                    }

                    if (!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                    {
                        // Step 2d

                      if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
             		  {
                            if (EwsWrapper.Instance().GetIPPS())
                            {
                                TraceFactory.Logger.Info("Step:02d - Bonjour Highest Priority - IPPS - Set from Telnet");
                                if (!ValidateBonjourHighestPriorityTelnet(activityData, PrinterParameters.BONJOURHIGHESTPRIORITY, family, BonjourHighestService.PrintingIPPS, (int)BonjourHighestService.PrintingIPPS))
                                {
                                    return false;
                                }
                            }

                            // Step 2e

                            TraceFactory.Logger.Info("Step:02e - Bonjour Highest Priority - LPD(TEXT) - Set from Telnet");
                            if (!ValidateBonjourHighestPriorityTelnet(activityData, PrinterParameters.BONJOURHIGHESTPRIORITY, family, BonjourHighestService.PrintingLPDTEXT, (int)BonjourHighestService.PrintingLPDTEXT))
                            {
                                return false;
                            }

                            // Step 2f

                            TraceFactory.Logger.Info("Step:02f - Bonjour Highest Priority - LPD(AUTO) - Set from Telnet");
                            if (!ValidateBonjourHighestPriorityTelnet(activityData, PrinterParameters.BONJOURHIGHESTPRIORITY, family, BonjourHighestService.PrintingLPDAUTO, (int)BonjourHighestService.PrintingLPDAUTO))
                            {
                                return false;
                            }

                            // Step 2g

                            //TraceFactory.Logger.Info("Step:02g - Bonjour Highest Priority - LPD(BINPS) - Set from Telnet");
                            //if (!ValidateBonjourHighestPriorityTelnet(activityData, PrinterParameters.BONJOURHIGHESTPRIORITY, family, BonjourHighestService.PrintingLPDBINPS, (int)BonjourHighestService.PrintingLPDBINPS))
                            //{
                            //    return false;
                            //}
                        }
                        // Step 3a

                        TraceFactory.Logger.Info("Step:03a - Bonjour Highest Priority - P9100 - Set from SNMP");
                        if (!ValidateBonjourHighestPrioritySNMP(BonjourHighestService.Printing9100, (int)BonjourHighestService.Printing9100))
                        {
                            return false;
                        }

                        // Step 3b

                        TraceFactory.Logger.Info("Step:03b - Bonjour Highest Priority - IPP - Set from SNMP");
                        if (!ValidateBonjourHighestPrioritySNMP(BonjourHighestService.PrintingIPP, (int)BonjourHighestService.PrintingIPP))
                        {
                            return false;
                        }

                        if (!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                        {
                            if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                            {

                                // Step 3c

                                TraceFactory.Logger.Info("Step:03c - Bonjour Highest Priority - LPD(RAW) - Set from SNMP");
                                if (!ValidateBonjourHighestPrioritySNMP(BonjourHighestService.PrintingLPDRAW, (int)BonjourHighestService.PrintingLPDRAW))
                                {
                                    return false;
                                }

                                // Step 3d
                                if (EwsWrapper.Instance().GetIPPS())
                                {
                                    TraceFactory.Logger.Info("Step:03d - Bonjour Highest Priority - IPPS - Set from SNMP");
                                    if (!ValidateBonjourHighestPrioritySNMP(BonjourHighestService.PrintingIPPS, (int)BonjourHighestService.PrintingIPPS))
                                    {
                                        return false;
                                    }
                                }

                                // Step 3e

                                TraceFactory.Logger.Info("Step:03e - Bonjour Highest Priority - LPD(TEXT) - Set from SNMP");
                                if (!ValidateBonjourHighestPrioritySNMP(BonjourHighestService.PrintingLPDTEXT, (int)BonjourHighestService.PrintingLPDTEXT))
                                {
                                    return false;
                                }

                                // Step 3f

                                TraceFactory.Logger.Info("Step:03f - Bonjour Highest Priority - LPD(AUTO) - Set from SNMP");
                                if (!ValidateBonjourHighestPrioritySNMP(BonjourHighestService.PrintingLPDAUTO, (int)BonjourHighestService.PrintingLPDAUTO))
                                {
                                    return false;
                                }

                                // Step 3g

                                //TraceFactory.Logger.Info("Step:03g - Bonjour Highest Priority - LPD(BINPS) - Set from SNMP");
                                //if (!ValidateBonjourHighestPrioritySNMP(BonjourHighestService.PrintingLPDBINPS, (int)BonjourHighestService.PrintingLPDBINPS))
                                //{
                                //    return false;
                                //}
                            }
                        }
                    }

                    return true;
                }
                catch (Exception discoveryException)
                {
                    TraceFactory.Logger.Info(discoveryException.Message);

                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    EwsWrapper.Instance().SetBonjourHighestService(defaultHighestBonjourService);
                }
            }
        }

        #endregion

        #region BonjourPreinstalledPrintDriver

        /// <summary>
        /// Bonjour Highest Priority - Print Driver Preinstalled
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool BonjourPreinstalledPrintDriver(DiscoveryActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                if (PrinterInstall(activityData))
                {
                    TraceFactory.Logger.Info("Print driver installed successfully");
                }
                else
                {
                    TraceFactory.Logger.Info("Print driver failed to install"); ;
                }

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                    {
                        return false;
                    }
                }
                if (!EwsWrapper.Instance().SetBonjour(true))
                {
                    return false;
                }
                string bonjourServiceName = EwsWrapper.Instance().GetBonjourServiceName();

                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    // Step 1

                    TraceFactory.Logger.Info("Step 01/01 - Automatic selection of pre-installed print driver");
                    if (ValidateAutomaticDriverSelection(bonjourServiceName))
                    {
                        TraceFactory.Logger.Info("As expected: The pre-installed driver is automatically used for Bonjour printer installation.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Not as expected: The pre-installed driver is not automatically used for Bonjour printer installation.");
                        return false;
                    }

                    return true;
                }
                catch (Exception discoveryException)
                {
                    TraceFactory.Logger.Info(discoveryException.Message);
                    return false;
                }
            }
        }

        #endregion

        #region BonjourHighestPriorityDisablePrintProtocol

        /// <summary>
        /// Bonjour Highest Priority - Print Driver Disabled
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool BonjourHighestPriorityDisablePrintProtocol(DiscoveryActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                BonjourHighestService defaultHighestBonjourService = new BonjourHighestService();

                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.LFP.ToString()))
                {
                    defaultHighestBonjourService = BonjourHighestService.PrintingLPDBINPS;
                }
                else
                {
                    defaultHighestBonjourService = BonjourHighestService.PrintingIPP;
                }

                if (PrinterInstall(activityData))
                {
                    TraceFactory.Logger.Info("Print driver installed successfully");
                }
                else
                {
                    TraceFactory.Logger.Info("Print driver failed to install"); ;
                }

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    EwsWrapper.Instance().SetMulticastIPv4(true);
                }
                EwsWrapper.Instance().SetBonjour(true);
                string bonjourServiceName = EwsWrapper.Instance().GetBonjourServiceName();

                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    // Step 01

                    TraceFactory.Logger.Info("Step:01 - Bonjour Highest Priority (P9100) - P9100 Disable");
                    EwsWrapper.Instance().SetBonjourHighestService(BonjourHighestService.Printing9100);
                    EwsWrapper.Instance().SetP9100(false);
                    Thread.Sleep(TimeSpan.FromMinutes(1));

                    // The Bonjour Highest Service value is assigned based on the UI Mapper application result for P9100
                    string bonjourHighestService = "IPP";

                    if (ValidateBonjourHighestService(bonjourServiceName, bonjourHighestService))
                    {
                        TraceFactory.Logger.Info("As expected: Bonjour Printer Wizard set Bonjour Highest Priority as IPP since P9100 is disabled");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Not as expected: Bonjour Printer Wizard failed to set Bonjour Highest Priority as IPP");
                        return false;
                    }
                    ProcessUtil.KillProcess("PrinterWizard.exe");
                    EwsWrapper.Instance().SetP9100(true);

                    // Step 02

                    if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                    {
                        TraceFactory.Logger.Info("Step:02 - Bonjour Highest Priority (LPD) - LPD Disable");
                        EwsWrapper.Instance().SetBonjourHighestService(BonjourHighestService.PrintingLPDRAW);
                        EwsWrapper.Instance().SetLPD(false);
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                        // The Bonjour Highest Service value is assigned based on the UI Mapper application result for P9100
                        bonjourHighestService = "IPP";

                        if (ValidateBonjourHighestService(bonjourServiceName, bonjourHighestService))
                        {
                            TraceFactory.Logger.Info("As expected: Bonjour Printer Wizard set Bonjour Highest Priority as IPP since LPD is disabled");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Not as expected: Bonjour Printer Wizard failed to set Bonjour Highest Priority as IPP");
                            return false;
                        }
                        EwsWrapper.Instance().SetLPD(true);

                        // Step 03

                        TraceFactory.Logger.Info("Step:03 - Bonjour Highest Priority (IPP) - IPP Disable");
                        EwsWrapper.Instance().SetBonjourHighestService(BonjourHighestService.PrintingIPP);
                        EwsWrapper.Instance().SetIPP(false);
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                        // The Bonjour Highest Service value is assigned based on the UI Mapper application result for P9100
                        bonjourHighestService = "LPR";

                        if (ValidateBonjourHighestService(bonjourServiceName, bonjourHighestService))
                        {
                            TraceFactory.Logger.Info("As expected: Bonjour Printer Wizard set Bonjour Highest Priority as LPD since IPP is disabled");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Not as expected: Bonjour Printer Wizard failed to set Bonjour Highest Priority as LPD");
                            return false;
                        }
                        EwsWrapper.Instance().SetIPP(true);


                        // Step 04

                        TraceFactory.Logger.Info("Step:04 - Bonjour Highest Priority (IPPS) - IPPS Disable");
                        EwsWrapper.Instance().SetBonjourHighestService(BonjourHighestService.PrintingIPPS);
                        EwsWrapper.Instance().SetIPPS(false);
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                        // The Bonjour Highest Service value is assigned based on the UI Mapper application result for P9100
                        bonjourHighestService = "IPP";

                        if (ValidateBonjourHighestService(bonjourServiceName, bonjourHighestService))
                        {
                            TraceFactory.Logger.Info("As expected: Bonjour Printer Wizard set Bonjour Highest Priority as IPP since IPPS is disabled");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Not as expected: Bonjour Printer Wizard failed to set Bonjour Highest Priority as IPP");
                            return false;
                        }
                        EwsWrapper.Instance().SetIPPS(true);

                        // Step 05

                        TraceFactory.Logger.Info("Step:05 - Bonjour Highest Priority (IPP) - IPP/IPPS Disable");
                        EwsWrapper.Instance().SetBonjourHighestService(BonjourHighestService.PrintingIPP);
                        EwsWrapper.Instance().SetIPP(false);
                        EwsWrapper.Instance().SetIPPS(false);
                        Thread.Sleep(TimeSpan.FromMinutes(1));

                        // The Bonjour Highest Service value is assigned based on the UI Mapper application result for P9100
                        bonjourHighestService = "LPR";

                        if (ValidateBonjourHighestService(bonjourServiceName, bonjourHighestService))
                        {
                            TraceFactory.Logger.Info("As expected: Bonjour Printer Wizard set Bonjour Highest Priority as LPD since IPP/IPPS is disabled");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Not as expected: Bonjour Printer Wizard failed to set Bonjour Highest Priority as LPD");
                            return false;
                        }

                        // Step 06

                        TraceFactory.Logger.Info("Step:06 - Bonjour Highest Priority (IPPS) - IPP/IPPS Disable");
                        EwsWrapper.Instance().SetBonjourHighestService(BonjourHighestService.PrintingIPP);
                        EwsWrapper.Instance().SetIPP(false);
                        EwsWrapper.Instance().SetIPPS(false);
                        Thread.Sleep(TimeSpan.FromMinutes(1));

                        // The Bonjour Highest Service value is assigned based on the UI Mapper application result for P9100
                        bonjourHighestService = "LPR";

                        if (ValidateBonjourHighestService(bonjourServiceName, bonjourHighestService))
                        {
                            TraceFactory.Logger.Info("As expected: Bonjour Printer Wizard set Bonjour Highest Priority as LPD since IPP/IPPS is disabled");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Not as expected: Bonjour Printer Wizard failed to set Bonjour Highest Priority as LPD");
                            return false;
                        }
                    }

                    return true;
                }
                catch (Exception discoveryException)
                {
                    TraceFactory.Logger.Info(discoveryException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    EwsWrapper.Instance().SetBonjourHighestService(defaultHighestBonjourService);
                    EwsWrapper.Instance().SetAdvancedOptions();
                }
            }
        }

        #endregion

        #region BonjourHighestPriorityPrinting

        /// <summary>
        /// Bonjour Highest Priority
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool BonjourHighestPriorityPrinting(DiscoveryActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                if (PrinterInstall(activityData))
                {
                    TraceFactory.Logger.Info("Print driver installed successfully");
                }
                else
                {
                    TraceFactory.Logger.Info("Print driver failed to install"); ;
                }

                BonjourHighestService defaultHighestBonjourService = new BonjourHighestService();

                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.LFP.ToString()))
                {
                    defaultHighestBonjourService = BonjourHighestService.PrintingLPDBINPS;
                }
                else
                {
                    defaultHighestBonjourService = BonjourHighestService.PrintingIPP;
                }
              	if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
             	{
                    EwsWrapper.Instance().SetMulticastIPv4(true);
                }
                EwsWrapper.Instance().SetBonjour(true);

                string bonjourServiceName = EwsWrapper.Instance().GetBonjourServiceName();
                LocalPrintServer localPrintServer = new LocalPrintServer();
                //PrintQueue defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();

                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    // Step 01

                    TraceFactory.Logger.Info("Step:01 - Bonjour Highest Service - P9100 Printing");

                    EwsWrapper.Instance().SetBonjourHighestService(BonjourHighestService.Printing9100);
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    // The Bonjour Highest Service value is assigned based on the UI Mapper application result for P9100
                    string bonjourHighestService = "Raw";

                    if (ValidateBonjourHighestServiceInstallation(bonjourServiceName, bonjourHighestService))
                    {
                        TraceFactory.Logger.Info("Printer installation using P9100 protocol in progress");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Printer installation using P9100 protocol failed");
                        return false;
                    }

                    string portNumber = GetPrinterPortNumber();

                    if (portNumber.Contains("9100"))
                    {
                        TraceFactory.Logger.Info("As expexted: Successfully installed printer using P9100 protocol");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Not as expected: Failed to install printer using P9100 protocol");
                        return false;
                    }
                    PrintQueue defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();

                    // string[] files = GetFiles(@"\\etlhubrepo\boi\CTC\VEP\EVA\Documents", FolderType.SimpleFiles);
                    // string[] files = GetFiles(@"\\Etlhubrepo.etl.psr.rd.hpicorp.net\boi\CTC\InkJet\WeberPDL\Documents", FolderType.SimpleFiles);
                    string[] files = GetFiles(@"\\etlrepo\boi\CTC\InkJet\WeberPDL\Documents", FolderType.SimpleFiles);                    

                    PrintSystemJobInfo myPrintJob = defaultPrintQueue.AddJob("SimpleFiles");
                    StreamReader myStreamReader = new StreamReader(files.FirstOrDefault());                    

                    // Write a Byte buffer to the JobStream and close the stream
                    Stream anotherStream = myPrintJob.JobStream;
                    Byte[] anotherByteBuffer = UnicodeEncoding.Unicode.GetBytes(myStreamReader.ReadToEnd());
                    anotherStream.Write(anotherByteBuffer, 0, anotherByteBuffer.Length);
                    anotherStream.Close();
                    myPrintJob.Refresh();
                    Thread.Sleep(TimeSpan.FromMinutes(2));
                    myPrintJob.Refresh();

                    if ((myPrintJob.IsDeleted) || (myPrintJob.IsPrinting))
                    {
                        TraceFactory.Logger.Info("Successfully printed the print jobs");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to print the print jobs");
                        return false;
                    }

                    PrintQueueDeletion(defaultPrintQueue.FullName);

                    if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                    {

                        TraceFactory.Logger.Info("Step:02 - Bonjour Highest Service - LPD Printing");

                        EwsWrapper.Instance().SetBonjourHighestService(BonjourHighestService.PrintingLPDRAW);
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                        // The Bonjour Highest Service value is assigned based on the UI Mapper application result for P9100
                        bonjourHighestService = "LPR";

                        if (ValidateBonjourHighestServiceInstallation(bonjourServiceName, bonjourHighestService))
                        {
                            TraceFactory.Logger.Info("Printer installation using LPD protocol in progress");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Printer installation using LPD protocol failed");
                            return false;
                        }

                        portNumber = GetPrinterPortNumber();

                        if (portNumber.Contains("LPR"))
                        {
                            TraceFactory.Logger.Info("As expected: Successfully installed printer using LPD protocol");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Not as expected: Failed to install printer using LPD protocol");
                            return false;
                        }
                        PrintQueueDeletion(defaultPrintQueue.FullName);
                    }

                    //TraceFactory.Logger.Info("Step:03 - Bonjour Highest Service - IPP Printing");

                    //EwsWrapper.Instance().SetBonjourHighestService(BonjourHighestService.PrintingIPP);
                    //Thread.Sleep(TimeSpan.FromMinutes(1));
                    //// The Bonjour Highest Service value is assigned based on the UI Mapper application result for P9100
                    //bonjourHighestService = "IPP";

                    //if (ValidateBonjourHighestServiceInstallation(bonjourServiceName, bonjourHighestService))
                    //{
                    //    TraceFactory.Logger.Info("Printer installation using IPP protocol in progress");
                    //}
                    //else
                    //{
                    //    TraceFactory.Logger.Info("Printer installation using IPP protocol failed");
                    //    return false;
                    //}

                    //portNumber = GetPrinterPortNumber();
                    //if (portNumber.Contains("IPP"))
                    //{
                    //    TraceFactory.Logger.Info("As expected: Successfully installed printer using IPP protocol");
                    //}
                    //else
                    //{
                    //    TraceFactory.Logger.Info("Not as expected: Failed to install printer using IPP protocol");
                    //    return false;
                    //}
                    //PrintQueueDeletion();
                    //TraceFactory.Logger.Info("Step:04 - Bonjour Highest Service - IPPS Printing");

                    //EwsWrapper.Instance().SetBonjourHighestService(BonjourHighestService.PrintingIPPS);
                    //Thread.Sleep(TimeSpan.FromMinutes(1));
                    //// The Bonjour Highest Service value is assigned based on the UI Mapper application result for P9100
                    //bonjourHighestService = "IPP";

                    //if (ValidateBonjourHighestServiceInstallation(bonjourServiceName, bonjourHighestService))
                    //{
                    //    TraceFactory.Logger.Info("Printer installation using IPPS protocol in progress");
                    //}
                    //else
                    //{
                    //    TraceFactory.Logger.Info("Printer installation using IPPS protocol failed");
                    //    return false;
                    //}

                    //portNumber = GetPrinterPortNumber();
                    //if (portNumber.Contains("IPP"))
                    //{
                    //    TraceFactory.Logger.Info("As expected: Successfully installed printer using IPPS protocol");
                    //}
                    //else
                    //{
                    //    TraceFactory.Logger.Info("Not as expected: Failed to install printer using IPPS protocol");
                    //    return false;
                    //}
                    //PrintQueueDeletion();
                    return true;

                }
                catch (Exception discoveryException)
                {
                    TraceFactory.Logger.Info(discoveryException.Message);

                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    EwsWrapper.Instance().SetBonjourHighestService(defaultHighestBonjourService);                    
                }
            }
        }

        #endregion

        #region MultiCastIPv4_Enable_Disable

        /// <summary>
        /// Enable and Disable Multicast IPv4 using SNMP/Telnet/EWS.
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool MultiCastIPv4_Enable_Disable(DiscoveryActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }

                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                    // Step 1

                    TraceFactory.Logger.Info("Step 01/06 - MultiCast IPv4 - Disable from EWS");
                    if (EwsWrapper.Instance().SetMulticastIPv4(false))
                    {
                        TraceFactory.Logger.Info("As expected: Multicast IPv4 successfully disabled from EWS");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Not as expected: Multicast IPv4 not disabled from EWS");
                        return false;
                    }

                    // Step 2

                    TraceFactory.Logger.Info("Step 02/06 - MultiCast IPv4 - Enable from EWS");
                    if (EwsWrapper.Instance().SetMulticastIPv4(true))
                    {
                        TraceFactory.Logger.Info("As expected: Multicast IPv4 successfully enabled from EWS");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Not as expected: Multicast IPv4 not enabled from EWS");
                        return false;
                    }
                    // Step 3

                    TraceFactory.Logger.Info("Step 03/06 - MultiCast IPv4 - Disable from Telnet");
                    if (TelnetWrapper.Instance().ToggleParameter(PrinterParameters.MULTICASTIPV4, family, activityData.IPv4Address, false))
                    {
                        TraceFactory.Logger.Info("As expected: Multicast IPv4 successfully disabled from Telnet");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Not as expected: Multicast IPv4 not disabled from Telnet");
                        return false;
                    }

                    // Step 4

                    TraceFactory.Logger.Info("Step 04/06 - Multicast IPv4 - Enable from Telnet");
                    if (TelnetWrapper.Instance().ToggleParameter(PrinterParameters.MULTICASTIPV4, family, activityData.IPv4Address, true))
                    {
                        TraceFactory.Logger.Info("As expected: Multicast IPv4 successfully enabled from Telnet");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Not as expected: Multicast IPv4 not enabled from Telnet");
                        return false;
                    }

                    // Step 5

                    TraceFactory.Logger.Info("Step 05/06 - MultiCast IPv4 - Disable from SNMP");
                    if (SnmpWrapper.Instance().SetMulticastIPv4(false))
                    {
                        TraceFactory.Logger.Info("As expected: Multicast IPv4 successfully disabled from SNMP");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Not as expected: Multicast IPv4 not disabled from SNMP");
                        return false;
                    }

                    // Step 6

                    TraceFactory.Logger.Info("Step 06/06 - Multicast Ipv4 - Enable from SNMP");
                    if (SnmpWrapper.Instance().SetMulticastIPv4(true))
                    {
                        TraceFactory.Logger.Info("As expected: Multicast IPv4 successfully enabled from SNMP");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Not as expected: Multicast IPv4 not enabled from SNMP");
                        return false;
                    }
                    return true;
                }
                catch (Exception discoveryException)
                {
                    TraceFactory.Logger.Info(discoveryException.Message);
                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                    EwsWrapper.Instance().SetMulticastIPv4(true);
                }
            }
        }

        #endregion

        #region MulticastIPv4_PowerCycle

        /// <summary>
        /// Template ID:79282
        /// TEMPLATE Verify DUT Multicast IPv4 status after reboot.
        /// 1st Step
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Disable 'Multicast IPv4' from web UI
        /// 4. Power cycle
        /// 5. Check 'Multicast IPv4' is disabled or not.
        /// 
        /// 2nd Step
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Enable 'Multicast IPv4' from web UI
        /// 4. Power cycle
        /// 5. Check 'Multicast IPv4' is enabled or not.
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>
        public static bool MulticastIPv4_PowerCycle(DiscoveryActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            //validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameter is null!!");

                return false;
            }

            // create printer object
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

            if (!EwsWrapper.Instance().SetMulticastIPv4(true))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step 1

                TraceFactory.Logger.Info("Step 01/02 - Disable Multicast IPv4 from EWS and Power Cycle");

                if (!EwsWrapper.Instance().SetMulticastIPv4(false))
                {
                    return false;
                }
                printer.PowerCycle();
                TraceFactory.Logger.Debug("Validating Multicast IPv4 option from EWS");

                if (EwsWrapper.Instance().GetMulticastIPv4())
                {
                    TraceFactory.Logger.Info("Not as expected: Multicast IPv4 option is enabled after Power Cycle");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: Multicast IPv4 option is disabled after Power Cycle");
                }

                // Step 2
                TraceFactory.Logger.Info("Step 02/02 - Enable Multicast IPv4 option from EWS and Power Cycle");

                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }
                printer.PowerCycle();

                TraceFactory.Logger.Debug("Validating Multicast IPv4 option from EWS");

                if (!EwsWrapper.Instance().GetMulticastIPv4())
                {
                    TraceFactory.Logger.Info("Not as expected: Multicast IPv4 option is disabled after Power Cycle");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: Multicast IPv4 option is enabled after Power Cycle");
                }
                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);

                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetMulticastIPv4(true);
            }
        }

        #endregion

        #region MulticastIPv4_ColdReset

        /// <summary>
        /// Template ID:79282
        /// TEMPLATE Verify DUT Multicast IPv4 status after cold reset.
        /// 1st Step
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Disable 'Multicast IPv4' from web UI
        /// 4. Cold Reset
        /// 5. Check 'Multicast IPv4' is disabled or not.
        /// 
        /// 2nd Step
        /// 1. Connect Printer
        /// 2. Open EWS -> Navigate to Networking tab-> Advanced
        /// 3. Enable 'Multicast IPv4' from web UI
        /// 4. Cold Reset
        /// 5. Check 'Multicast IPv4' is enabled or not.
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>
        public static bool MulticastIPv4_ColdReset(DiscoveryActivityData activityData)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
            //validating the input parameters
            if (null == activityData)
            {
                TraceFactory.Logger.Fatal("Input Parameter is null!!");

                return false;
            }

            // create printer object
            PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
            Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));

            if (!EwsWrapper.Instance().SetMulticastIPv4(true))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                // Step 1

                TraceFactory.Logger.Info("Step 01/02 - Disable Multicast IPv4 from EWS and Cold Reset/Restore Factory Settings");
                if (!EwsWrapper.Instance().SetMulticastIPv4(false))
                {
                    return false;
                }
                printer.ColdReset();

                TraceFactory.Logger.Debug("Validating Multicast IPv4 option from EWS");

                if (!EwsWrapper.Instance().GetMulticastIPv4())
                {
                    TraceFactory.Logger.Info("Not as expected: Multicast IPv4 option is disabled after Cold Reset/Restore Factory Settings");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: Multicast IPv4 option is enabled after Cold Reset/Restore Factory Settings");
                }

                // Step 2
                TraceFactory.Logger.Info("Step 02/02 - Enable Multicast IPv4 option from EWS and Cold Reset/Restore Factory Settings");

                if (!EwsWrapper.Instance().SetMulticastIPv4(true))
                {
                    return false;
                }

                printer.ColdReset();

                TraceFactory.Logger.Debug("Validating Multicast IPv4 option from EWS");

                if (!EwsWrapper.Instance().GetMulticastIPv4())
                {
                    TraceFactory.Logger.Info("Not as expected: Multicast IPv4 option is disabled after Cold Reset/Restore Factory Settings");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("As expected: Multicast IPv4 option is enabled after Cold Reset/Restore Factory Settings");
                }

                return true;
            }
            catch (Exception discoveryException)
            {
                TraceFactory.Logger.Info(discoveryException.Message);
                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetMulticastIPv4(true);
            }
        }

        #endregion

        #region Discovery_FW_Upgrade

        /// <summary>
        /// Perform FW Upgrade when all discovery protocols are enabled/disabled
        /// </summary>
        /// <param name="activityData">NetworkDiscoveryActivityData</param>
        /// <returns>true if template passed
        ///          false if template fails </returns>        
        public static bool Discovery_FW_Upgrade(DiscoveryActivityData activityData)
        {
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);
                // validating the input parameters
                if (null == activityData)
                {
                    TraceFactory.Logger.Info("Input Parameter is null!!");
                    return false;
                }

                // create printer object
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.IPv4Address));
                string FIRMWAREBASELOCATION = Path.Combine(CtcSettings.ConnectivityShare, @"connectivityShare\FirmwareFiles");

                string firmwareUpgradeFilePath = FIRMWAREBASELOCATION + Path.DirectorySeparatorChar + activityData.ProductFamily + Path.DirectorySeparatorChar + activityData.ProductName + Path.DirectorySeparatorChar + "UpgradeFile";
                string firmwareDowngradeFilePath = FIRMWAREBASELOCATION + Path.DirectorySeparatorChar + activityData.ProductFamily + Path.DirectorySeparatorChar + activityData.ProductName + Path.DirectorySeparatorChar + "DowngradeFile";
                DirectoryInfo upgradeFirmwareDir = new DirectoryInfo(firmwareUpgradeFilePath);
                DirectoryInfo downgradeFirmwareDir = new DirectoryInfo(firmwareDowngradeFilePath);

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    EwsWrapper.Instance().SetMulticastIPv4(true);
                }
                EwsWrapper.Instance().SetSLP(true);
                EwsWrapper.Instance().SetWSDiscovery(true);
                EwsWrapper.Instance().SetBonjour(true);

                if (!activityData.ProductFamily.EqualsIgnoreCase(ProductFamilies.InkJet.ToString()))
                {
                    EwsWrapper.Instance().InstallFirmware(downgradeFirmwareDir.GetFiles()[0].FullName);
                }
                try
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                    // Step 1

                    TraceFactory.Logger.Info("Step:01 - Disable Discovery Protocols - Firmware Upgrade");
                    EwsWrapper.Instance().SetMulticastIPv4(false);
                    EwsWrapper.Instance().SetSLP(false);
                    EwsWrapper.Instance().SetWSDiscovery(false);
                    EwsWrapper.Instance().SetBonjour(false);

                    EwsWrapper.Instance().InstallFirmware(upgradeFirmwareDir.GetFiles()[0].FullName);

                    TraceFactory.Logger.Debug("Validating Discovery options from EWS");

                    if ((EwsWrapper.Instance().GetBonjour()) || (EwsWrapper.Instance().GetSLP()) || (EwsWrapper.Instance().GetMulticastIPv4()) || (EwsWrapper.Instance().GetWSDiscovery()))
                    {
                        TraceFactory.Logger.Info("Not as expected: One or more Discovery option(s) is/are enabled after Firmware Upgrade");
                        EwsWrapper.Instance().GetMulticastIPv4();
                        EwsWrapper.Instance().GetBonjour();
                        EwsWrapper.Instance().GetSLP();
                        EwsWrapper.Instance().GetWSDiscovery();
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("As expected: All Discovery options are disabled after Firmware Upgrade");
                    }

                    EwsWrapper.Instance().InstallFirmware(downgradeFirmwareDir.GetFiles()[0].FullName);

                    // Step 2

                    TraceFactory.Logger.Info("Step:02 - Enable Discovery Protocols - Firmware Upgrade");
                    EwsWrapper.Instance().SetMulticastIPv4(true);
                    EwsWrapper.Instance().SetSLP(true);
                    EwsWrapper.Instance().SetWSDiscovery(true);
                    EwsWrapper.Instance().SetBonjour(true);

                    EwsWrapper.Instance().InstallFirmware(upgradeFirmwareDir.GetFiles()[0].FullName);

                    TraceFactory.Logger.Debug("Validating Discovery options from EWS");

                    if (!(EwsWrapper.Instance().GetBonjour()) || !(EwsWrapper.Instance().GetSLP()) || !(EwsWrapper.Instance().GetMulticastIPv4()) || !(EwsWrapper.Instance().GetWSDiscovery()))
                    {
                        TraceFactory.Logger.Info("Not as expected: One or more Discovery option(s) is/are disabled after Firmware Upgrade");
                        EwsWrapper.Instance().GetMulticastIPv4();
                        EwsWrapper.Instance().GetBonjour();
                        EwsWrapper.Instance().GetSLP();
                        EwsWrapper.Instance().GetWSDiscovery();
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("As expected: All Discovery options are enabled after Firmware Upgrade");
                    }

                    return true;
                }
                catch (Exception discoveryException)
                {
                    TraceFactory.Logger.Info(discoveryException.Message);

                    return false;
                }
                finally
                {
                    TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                    EwsWrapper.Instance().SetMulticastIPv4(true);
                    EwsWrapper.Instance().SetSLP(true);
                    EwsWrapper.Instance().SetWSDiscovery(true);
                    EwsWrapper.Instance().SetBonjour(true);
                }
            }
        }

        #endregion


        #region Private Methods

        private static bool BacabodDiscovery(string ipAddress, bool expectedStatus, BacaBodSourceType sourceType, BacaBodDiscoveryType discoveryType)
        {
            TraceFactory.Logger.Info("Expected Result: Bacabod Discovery {0}".FormatWith(expectedStatus ? "Success" : "Failure"));
            TraceFactory.Logger.Info("Validating printer discovery using BacaBod tool");

            bool actualStatus = BacaBodWrapper.DiscoverDevice(ipAddress, sourceType, discoveryType);

            if (!actualStatus)
            {
                actualStatus = BacaBodWrapper.DiscoverDevice(ipAddress, sourceType, discoveryType);
                if (!actualStatus)
                {
                    TraceFactory.Logger.Info("Actual Result: Printer {0} is not discovered through BacaBod tool".FormatWith(ipAddress));
                    return actualStatus == expectedStatus;
                }
            }
            TraceFactory.Logger.Info("Actual Result: Printer {0} is discovered through BacaBod tool".FormatWith(ipAddress));
            return actualStatus == expectedStatus;
        }

        private static bool WindowsExplorerDiscovery(IPAddress ipAddress, bool expectedStatus, bool discoverOnIPv6)
        {
            DeviceInfo deviceInfo = null;
            TraceFactory.Logger.Info("Expected Result: Windows Explorer Discovery {0}".FormatWith(expectedStatus ? "Success" : "Failure"));
            TraceFactory.Logger.Info("Validating printer discovery through Windows Explorer");

            bool actualStatus = PrinterDiscovery.Discover(ipAddress, out deviceInfo, discoverOnIPv6: discoverOnIPv6);

            if (actualStatus)
            {
                TraceFactory.Logger.Info("Actual Result: Printer {0} is discovered in Windows Network Explorer".FormatWith(ipAddress));

            }
            else
            {
                TraceFactory.Logger.Info("Actual Result: Printer {0} is not discovered in Windows Network Explorer".FormatWith(ipAddress));
            }

            return actualStatus == expectedStatus;
        }

        private static bool BonjourDiscovery(IPAddress ipAddress, string bonjourServiceName, bool expectedStatus)
        {
            TraceFactory.Logger.Info("Expected Result: Bonjour Discovery {0}".FormatWith(expectedStatus ? "Success" : "Failure"));
            TraceFactory.Logger.Info("Validating printer discovery through Bonjour");

            bool actualStatus = BonjourDiscovery(bonjourServiceName);

            if (actualStatus)
            {
                TraceFactory.Logger.Info("Actual Result: Printer {0} is discovered through Bonjour".FormatWith(ipAddress));

            }
            else
            {
                TraceFactory.Logger.Info("Actual Result: Printer {0} is not discovered through Bonjour".FormatWith(ipAddress));
            }

            return actualStatus == expectedStatus;
        }

        private static bool BonjourDiscovery(string BonjourServiceName)
        {
            string tempPath = Path.GetTempFileName();
            string discoveredDevices = String.Empty;
            //string serviceFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "dns-sd.exe");
            string serviceFile = "dns-sd.exe";

            try
            {
                ProcessUtil.Execute("cmd.exe", "/C {0} -B > {1}".FormatWith(serviceFile, tempPath), TimeSpan.FromSeconds(10));
            }
            catch
            { }
            finally
            {
                Process[] activeProcess = Process.GetProcesses();
                if (null != activeProcess.FirstOrDefault(x => x.ProcessName.EqualsIgnoreCase("dns-sd")))
                {
                    activeProcess.FirstOrDefault(x => x.ProcessName.EqualsIgnoreCase("dns-sd")).Kill();
                }

                Thread.Sleep(TimeSpan.FromSeconds(10));
            }

            discoveredDevices = File.ReadAllText(tempPath);
            TraceFactory.Logger.Debug(discoveredDevices);

            if (string.IsNullOrEmpty(discoveredDevices))
            {
                TraceFactory.Logger.Info("No printers discovered using Bonjour");
                return false;
            }
            else
            {
                if (!discoveredDevices.Contains(BonjourServiceName))
                {
                    return false;
                }
            }
            return true;
        }

        private static string BonjourDiscovery()
        {
            string tempPath = Path.GetTempFileName();
            string serviceFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "dns-sd.exe");

            try
            {
                ProcessUtil.Execute("cmd.exe", "/C {0} -B > {1}".FormatWith(serviceFile, tempPath), TimeSpan.FromSeconds(10));
            }
            catch
            { }
            finally
            {
                Process[] activeProcess = Process.GetProcesses();
                if (null != activeProcess.FirstOrDefault(x => x.ProcessName.EqualsIgnoreCase("dns-sd")))
                {
                    activeProcess.FirstOrDefault(x => x.ProcessName.EqualsIgnoreCase("dns-sd")).Kill();
                }

                Thread.Sleep(TimeSpan.FromSeconds(10));
            }

            return tempPath;
        }

        private static bool ValidateBonjourHighestServiceInstallation(string bonjourServiceName, string bonjourHighestService)
        {

            var tempScriptDirectory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "BonjourHighestServiceInstallation"));
            File.WriteAllBytes(Path.Combine(tempScriptDirectory.FullName, "BonjourHighestServiceInstallation.tcx"), Properties.Resources.BonjourHighestServiceInstallation);
            File.WriteAllBytes(Path.Combine(tempScriptDirectory.FullName, "InstallationPrinterList.tcc"),
                Properties.Resources.InstallationPrinterList);
            File.WriteAllBytes(Path.Combine(tempScriptDirectory.FullName, "InstallationFinishScreen.tcc"),
                Properties.Resources.InstallationFinishScreen);

            TopCatScript _configureScript;
            _configureScript = new TopCatScript
            (
                Path.Combine(tempScriptDirectory.FullName, "BonjourHighestServiceInstallation.tcx"),
                "BonjourHighestServiceInstallation"
            );

            _configureScript.Properties.Add
            (
                "BonjourHighestService",
                bonjourHighestService
            );
            _configureScript.Properties.Add
            (
                "BonjourServiceName",
                bonjourServiceName
            );

            TopCatExecutionController tcExecutionController = new TopCatExecutionController(_configureScript);
            tcExecutionController.InstallPrerequisites(CtcSettings.GetSetting("TopCatSetup"));
            tcExecutionController.ExecuteTopCatTest();

            string resultFile = tcExecutionController.GetResultFilePath(CtcSettings.GetSetting("DomainAdminUserName"));
            if (!string.IsNullOrEmpty(resultFile))
            {
                if(!File.Exists(resultFile))
                {
                    File.Create(resultFile);                    
                }                
                XDocument resultDoc = XDocument.Load(resultFile);                
                var successTests = resultDoc.Descendants("SuccessfulTests").First().Descendants("test");                
                return successTests.Any();
            }
            else
            {
                return false;
            }
        }

        private static bool ValidateAutomaticDriverSelection(string bonjourServiceName)
        {

            var tempScriptDirectory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "AutomaticDriverInstallation"));

            File.WriteAllBytes(Path.Combine(tempScriptDirectory.FullName, "AutomaticDriverInstallation.tcx"), Properties.Resources.AutomaticDriverInstallation);
            File.WriteAllBytes(Path.Combine(tempScriptDirectory.FullName, "InstallationPrinterList.tcc"),
                Properties.Resources.InstallationPrinterList);
            File.WriteAllBytes(Path.Combine(tempScriptDirectory.FullName, "InstallationFinishScreen.tcc"),
                Properties.Resources.InstallationFinishScreen);

            TopCatScript _configureScript;
            _configureScript = new TopCatScript
            (
                Path.Combine(tempScriptDirectory.FullName, "AutomaticDriverInstallation.tcx"),
                "AutomaticDriverInstallation"
            );

            _configureScript.Properties.Add
            (
                "BonjourServiceName",
                bonjourServiceName
            );

            TopCatExecutionController tcExecutionController = new TopCatExecutionController(_configureScript);
            tcExecutionController.InstallPrerequisites(CtcSettings.GetSetting("TopCatSetup"));
            tcExecutionController.ExecuteTopCatTest();

            string resultFile = tcExecutionController.GetResultFilePath(CtcSettings.GetSetting("DomainAdminUserName"));
            if (!string.IsNullOrEmpty(resultFile))
            {
                XDocument resultDoc = XDocument.Load(resultFile);
                var successTests = resultDoc.Descendants("SuccessfulTests").First().Descendants("test");
                return successTests.Any();
            }
            else
            {
                return false;
            }
        }

        private static bool ValidateBonjourHighestService(string bonjourServiceName, string bonjourHighestService)
        {


            var tempScriptDirectory = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), "BonjourHighestService"));
            File.WriteAllBytes(Path.Combine(tempScriptDirectory.FullName, "BonjourHighestService.tcx"), Properties.Resources.BonjourHighestService);
            File.WriteAllBytes(Path.Combine(tempScriptDirectory.FullName, "InstallationPrinterList.tcc"),
                Properties.Resources.InstallationPrinterList);
            File.WriteAllBytes(Path.Combine(tempScriptDirectory.FullName, "InstallationFinishScreen.tcc"),
                Properties.Resources.InstallationFinishScreen);

            TopCatScript _configureScript;
            _configureScript = new TopCatScript
            (
                Path.Combine(tempScriptDirectory.FullName, "BonjourHighestService.tcx"),
                "BonjourHighestService"
            );

            _configureScript.Properties.Add
            (
                "BonjourHighestService",
                bonjourHighestService
            );
            _configureScript.Properties.Add
            (
                "BonjourServiceName",
                bonjourServiceName
            );

            TopCatExecutionController tcExecutionController = new TopCatExecutionController(_configureScript);
            tcExecutionController.InstallPrerequisites(CtcSettings.GetSetting("TopCatSetup"));
            tcExecutionController.ExecuteTopCatTest();

            string resultFile = tcExecutionController.GetResultFilePath(CtcSettings.GetSetting("DomainAdminUserName"));
            if (!string.IsNullOrEmpty(resultFile))
            {
                XDocument resultDoc = XDocument.Load(resultFile);
                var successTests = resultDoc.Descendants("SuccessfulTests").First().Descendants("test");
                return successTests.Any();
            }
            else
            {
                return false;
            }
        }

        private static bool PrinterInstall(DiscoveryActivityData activityData)
        {
            var drivers = DriverController.LoadFromDirectory(activityData.DriverPackagePath, true, SearchOption.AllDirectories);
            // Select the first driver that matches the current architecture
            var driver =
                (
                    from d in drivers
                    where d.Architecture == DriverController.LocalArchitecture &&
                        d.Name.Equals(activityData.DriverModel, StringComparison.OrdinalIgnoreCase)
                    select d
                ).FirstOrDefault();

            // Exit if current architecture doesn't match
            if (driver == null)
            {
                TraceFactory.Logger.Info("Printer installation failed, unable to find current architecture for this driver");
                return false;
            }

            DriverDetails driverProperty = new DriverDetails();
            driverProperty.InfPath = driver.InfPath;
            driverProperty.Architecture = driver.Architecture;
            driverProperty.Name = driver.Name;

            DriverInstaller.Install(driverProperty);
            return true;
        }

        private static bool PrintQueueDeletion(string defaultQueueName)
        {
            LocalPrintServer localPrintServer = new LocalPrintServer();
            PrintQueue defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();
            TraceFactory.Logger.Info("The printer installed with name: {0}".FormatWith(defaultQueueName));
            if (defaultPrintQueue.FullName.EqualsIgnoreCase(defaultQueueName))
            {
                if (LocalPrintServer.DeletePrintQueue(defaultQueueName))
                {
                    TraceFactory.Logger.Info("Successfully deleted the print queue.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to delete the print queue");
                    return false;
                }
            }
            else
            {
                TraceFactory.Logger.Debug("No print queue exists");
                return true;
            }

        }

        private static string GetPrinterPortNumber()
        {
            LocalPrintServer localPrintServer = new LocalPrintServer();
            PrintQueue defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();
            TraceFactory.Logger.Info("The printer installed with port number: {0}".FormatWith(defaultPrintQueue.QueuePort.Name));
            return defaultPrintQueue.QueuePort.Name;
        }

        private static bool ValidateBonjourHighestPriorityEWS(BonjourHighestService bonjourService)
        {
            if (!EwsWrapper.Instance().SetBonjourHighestService(bonjourService))
            {
                return false;
            }
            return true;
        }

        private static bool ValidateBonjourHighestPriorityTelnet(DiscoveryActivityData activityData, PrinterParameters parametername, PrinterFamilies family, BonjourHighestService bonjourService, int setValue, string getValue = null)
        {

            if (!TelnetWrapper.Instance().SetParameter(PrinterParameters.BONJOURHIGHESTPRIORITY, family, activityData.IPv4Address, setValue.ToString(), getValue))
            {
                return false;
            }

            if (bonjourService == EwsWrapper.Instance().GetBonjourHighestService())
            {
                TraceFactory.Logger.Info("Successfully validated the Bonjour Highest Service from EWS: {0}".FormatWith(bonjourService));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to validated the Bonjour Highest Service from EWS: {0}".FormatWith(bonjourService));
                return false;
            }
        }

        private static bool ValidateBonjourHighestPrioritySNMP(BonjourHighestService bonjourService, int value)
        {

            if (!SnmpWrapper.Instance().SetBonjourHighestPriority(value))
            {
                return false;
            }
            if (bonjourService == EwsWrapper.Instance().GetBonjourHighestService())
            {
                TraceFactory.Logger.Info("Successfully validated the Bonjour Highest Service from EWS: {0}".FormatWith(bonjourService));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to validated the Bonjour Highest Service from EWS: {0}".FormatWith(bonjourService));
                return false;
            }
        }

        /// <summary>
        /// Get all files available in folder path
        /// </summary>
        /// <param name="folderPath">Path from where files need to be retrieved</param>
        /// <param name="folder"><see cref=" FolderType"/></param>
        /// <returns>Collection of files</returns>
        private static string[] GetFiles(string folderPath, FolderType folder)
        {
            string relativePath = Path.Combine(folderPath, Enum<FolderType>.Value(folder));
            if (Directory.Exists(relativePath))
            {
                return Directory.GetFiles(relativePath);
            }
            else
            {
                TraceFactory.Logger.Info("Directory {0} is not found.".FormatWith(relativePath));
                return null;
            }
        }

        #endregion
    }
}
#endregion



