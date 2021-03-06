﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

using HP.ScalableTest.HardwareAutomation.NetworkSwitch;
using HP.ScalableTest.HardwareAutomation.Router;
using HP.ScalableTest.Network;
using HP.ScalableTest.Network.Dhcp;
using HP.ScalableTest.Network.PacketCapture;
using HP.ScalableTest.Plugin.CtcBase;
using HP.ScalableTest.Printer;

namespace HP.ScalableTest.Plugin.IPConfiguration
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

    public enum PrinterConnectionType
    {
        PrimaryWiredInterface,
        SecondaryWiredInterface,
        WirelessInterface
    }

    internal class PrinterInterfaceConfigurationDetails
    {
        public string Address { get; set; }

        public string MacAddress { get; set; }

        public ReservationType PrinterReservationType { get; set; }

        public IPConfigMethod ConfigMethod { get; set; }

        public PrinterConnectionType PrinterInterface{ get; set;}

        public Collection<IPAddress> StatelessAddresses { get; set; }

        public IPAddress LinkLocalAddress { get; set; }

        public IPAddress StatefullAddress { get; set; }

        public int LeaseTime { get; set; }

        public int PortNumber { get; set; }

		/// <summary>
		/// The destination server for hose break
		/// </summary>
		public string destinationServerAddress { get; set; }

		public string DhcpServerAddress { get; set; }

        public PrinterInterfaceConfigurationDetails()
        {
            Address = string.Empty;
            MacAddress = string.Empty;
            PrinterReservationType = ReservationType.Both;
            ConfigMethod = IPConfigMethod.BOOTP;
            PrinterInterface = PrinterConnectionType.PrimaryWiredInterface;
            LinkLocalAddress = IPAddress.None;
            StatelessAddresses = new Collection<IPAddress>();
            StatefullAddress = IPAddress.None;
			destinationServerAddress = string.Empty;
        }
    }

    /// <summary>
    /// IPConfiguration Templates
    /// </summary>
    internal static class IPConfigurationTemplates
    {
        #region Constants

        // TODO: Move appropriate constants to Printer/ other suitable class
        private const int DEFAULT_LEASE_TIME = 691200;
        private const int DEFAULT_PREFERRED_LEASE_TIME = 691200; // 8 days
        private const int DEFAULT_VALID_LEASE_TIME = 1036800; // 12 days
        private const long INFINITE_LEASE_TIME = 4294967295;
        private const int LEASE_WAIT_TIME = 6;
        private const int HOSEBREAK_TIMEOUT = 20;

        private const string ROUTER_IP_FORMAT = "{0}.1";
        private const string ROUTER_USERNAME = "STFRouter";
        private const string ROUTER_PASSWORD = "password";
        private const string SSH_USERNAME = "root";
        private const string SSH_PASSWORD = "socorro!";
        private static TimeSpan ROUTER_PREFERRED_LIFETIME = new TimeSpan(25, 0, 0, 0);
        private static TimeSpan ROUTER_VALID_LIFETIME = new TimeSpan(30, 0, 0, 0);
        private static TimeSpan PREFERRED_LIFETIME = new TimeSpan(0, 2, 0);
        private static TimeSpan VALID_LIFETIME = new TimeSpan(0, 4, 0);

        #endregion

        #region IPConfiguration Templates

        #region IP Acquisition from Server

        /// <summary>
        /// Template ID:96130
        ///Step:Verify device can obtain a basic IP address from DHCP server
        ///Two different ways to obtain an IP address via DHCP
        ///1.Create a reservation in the DHCP server.
        ///2. Create a dynamic DHCP range of IP addresses that provide a scope of dynamic IP addresses that can be assigned
        ///Expected:1.The device should be configured with the reserved dhcp IP address
        ///2. The device should always take IP from the dynamic IP address range provided by the DHCP server.
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool VerifyDhcpIPAcquisition(IPConfigurationActivityData activityData, int testNo)
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

            string invalidMacAddress = string.Empty;
            string currentDeviceAddress = activityData.PrimaryWiredIPv4Address;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                // Create Reservation for the printer for DHCP in DHCP server
                if (serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, currentDeviceAddress, activityData.PrimaryMacAddress) &&
                    serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, currentDeviceAddress, activityData.PrimaryMacAddress, ReservationType.Both))
                {
                    TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for DHCP : Succeeded");
                }
                else
                {
                    TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for DHCP : Failed");
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP))
                {
                    return false;
                }

                // TODO : Step 2 is not automated in phase I.

                // Make sure that the IPv4 Address is not assigned for any other device. A reservation is created for the actual printer IP Address with a dummy MAC Address.
                invalidMacAddress = activityData.PrimaryMacAddress.Remove(1, 2);

                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, currentDeviceAddress, activityData.PrimaryMacAddress);
                serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, currentDeviceAddress, invalidMacAddress, ReservationType.Both);

                // Printer might acquire a new IP Address as no reservation is available.
                currentDeviceAddress = Utility.GetPrinterIPAddress(activityData.PrimaryMacAddress);

                return CheckForPrinterAvailability(currentDeviceAddress);

            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                // Delete the reservation created for DHCP & Create reservation for both DHCP & BOOTP
                serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, currentDeviceAddress, invalidMacAddress);
                serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
                serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Both);

                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
            }
        }

        /// <summary>
        /// Template ID:96136
        ///Step:Verify device gets configured with an IP address from Bootp server
        ///1.Connect device in the network with bootp server.
        ///2. Configure printer to acquire Bootp IP address.
        ///3. Make a reservation in the Bootp server
        ///Expected:1. Device should get configured with IP from Bootp server.
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool VerifyBootpIPAcquisition(IPConfigurationActivityData activityData, int testCaseId)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testCaseId.ToString().ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                if (!(serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress) &&
                    serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Bootp)))
                {
                    return false;
                }

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                // TODO: Packet Capture 

                /*
				if (family.Equals(PrinterFamilies.TPS))
				{
					TraceFactory.Logger.Info("Validating the DHCP Packets by capturing the Packets after power cycle.");
					
					string packetID = RequestPacketData(1, activityData, testCaseId.ToString(), activityData.SessionId);
					printer.PowerCycle(0);

					if (ValidatePacketType(1, activityData, packetID, testCaseId.ToString(), activityData.SessionId).EqualsIgnoreCase("True"))
					{
						TraceFactory.Logger.Info("Successfully validated the DHCP Packets through Network Packets.");
					}
					else
					{
						TraceFactory.Logger.Info("Failed to validate the DHCP Packets through Network Packets.");
						return false;
					}
					 
				}
				else
				{
					printer.PowerCycle();
				}
				 * */

                printer.PowerCycle();

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.BOOTP))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
                serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Both);

                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.PrimaryWiredIPv4Address), TimeSpan.FromSeconds(20)))
                {
                    TraceFactory.Logger.Info("Ping succeeded- Printer IP Address:{0}".FormatWith(activityData.PrimaryWiredIPv4Address));
                }
                else
                {
                    TraceFactory.Logger.Info("Ping failed- Printer IP Address:{0}".FormatWith(activityData.PrimaryWiredIPv4Address));
                }
            }
        }

        #endregion

        #region IP Configuration Method Change

        /* This region contains the templates to verify IP Config method change.
		 * Config 	DHCP	BOOTP	Manual	Auto IP
		 * ------------------------------------------------- 
		 * DHCP		*		96180	96174	96182  
		 * BOOTP	96181	*		96141	96184 
		 * Manual	96175	96176	656076	96179 
		 * Auto IP	96183	96185	96178	* 
		 */

        /// <summary>
        /// Template ID:96174
        ///Step:Verify IP config method change from DHCP to Manual
        ///1. Configure the device via DHCP . 2. Ensure device acquires a DHCP IP
        ///3. Change the configuration from DHCP to manual and assign a manual IP.
        ///Expected:1. The device should release the DHCP acquired IP 
        ///2. The device should get configured with the manual IP
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool DHCPToManual(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP))
                {
                    return false;
                }

                // Setting IP Config method to Manual IP
                IPAddress manualIPAddress = Network.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.PrimaryWiredIPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, manualIPAddress))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, manualIPAddress.ToString()))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
            }
        }

        /// <summary>
        /// Template ID:96175
        ///Step:Verify IP Config method change from Manual to DHCP
        ///1. Set the IP Config method to Manual on the device and assign a manual IP 
        ///2. Change the IP Config method to DHCP
        ///Expected:The device should acquire a DHCP IP
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool ManualToDHCP(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                IPAddress manualIPAddress = Network.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.PrimaryWiredIPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, manualIPAddress))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, manualIPAddress.ToString()))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
            }
        }

        /// <summary>
        /// Template ID:96176
        ///Step:Verify IP Config method change from Manual to Bootp
        ///1. Set the IP Config method to Manual on the device and assign a manual IP 
        ///2. Change the IP Config method to Bootp
        ///Expected:The device should acquire a Bootp IP
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool ManualToBootp(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                IPAddress manualIPAddress = Network.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.PrimaryWiredIPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, manualIPAddress))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, manualIPAddress.ToString()))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.BOOTP))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
            }
        }

        /// <summary>
        /// Template ID:96177
        ///Step:Verify IP Config method change from Bootp to Manual
        ///1. Configure the device with a Bootp IP .
        ///2. Change the configuration from Bootp to manual and assign a manual IP.
        ///Expected:1. The device should get configured with the manual IP
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool BootpToManual(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.BOOTP))
                {
                    return false;
                }

                // Setting IP Config method to Manual IP
                IPAddress manualIPAddress = Network.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.PrimaryWiredIPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, manualIPAddress))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, manualIPAddress.ToString()))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
            }
        }

        /// <summary>
        /// Template ID:96180
        ///Step:Verify IP Config method change from DHCP to Bootp
        ///1. Connect a device in a network with DHCP and Bootp server.
        ///2. Ensure the device has acquired a DHCP IP
        ///3. Change the Config method to Bootp
        ///Expected:The device should release the DHCP IP and should acquire a Bootp IP
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool DHCPToBootp(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.BOOTP))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
            }
        }

        /// <summary>
        /// Template ID:96181
        ///Step:Verify IP Config method change from Bootp to DHCP
        ///1.Connect a device in a network with DHCP and Bootp server.
        ///2. Ensure the device has acquired a Bootp IP
        ///3. Change the Config method to DHCP
        ///Expected:The device should acquire a DHCP IP
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool BootpToDHCP(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.BOOTP))
                {
                    return false;
                } if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
            }
        }

        /// <summary>
        /// Template ID : 96178
        /// 1. Set the IP config method to Auto IP on the device.
        /// 2. Ensure device has acquired an Auto IP.
        /// 3. Change the configuration from Auto IP to manual and do not change the IP.
        /// Expected:
        /// 1. The device should get configured with the manual IP
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool AutoIPToManual(IPConfigurationActivityData activityData, int testNo)
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

            string currentPrinterIPAddress = activityData.PrimaryWiredIPv4Address;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                string autoIPAddress = string.Empty;

                if (!SetAutoIPConfigMethod(activityData.PrimaryDhcpServerIPAddress, activityData.ProductFamily, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, out autoIPAddress))
                {
                    return false;
                }

                currentPrinterIPAddress = autoIPAddress;

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, autoIPAddress))
                {
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(autoIPAddress));
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, validate: false);

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, autoIPAddress))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                // When printer is configured with Manual IP, even though the DHCP server is UP; printer will not acquire IP from server
                // Case 1: If printer is in Auto IP, same will be retained.
                // Case 2: If printer is in Manual IP, Auto IP will be configured.
                EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(currentPrinterIPAddress));

                if (!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) || !activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString()))
                {
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.AUTOIP, validate: false);
                }

                AutoIPPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template ID : 96179
        /// 1. Set the IP config method to Manual on the device and assign a maual IP
        /// 2. Change the IP config method to Auto IP
        /// Expected :
        /// The device should acquire an Auto IP
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool ManualToAutoIP(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                IPAddress manualIPAddress = NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.PrimaryWiredIPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, manualIPAddress);

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, manualIPAddress.ToString()))
                {
                    return false;
                }

                string autoIPAddress = string.Empty;

                // TPS : When Configuration Method is changed to AutoIP, Printer is configured with Auto IP.
                // VEP : When Configuration Method is changed to AutoIP, Printer is configured with DHCP IP. Hence stopping the DHCP server.

                if (!SetAutoIPConfigMethod(activityData.PrimaryDhcpServerIPAddress, activityData.ProductFamily, manualIPAddress.ToString(), activityData.PrimaryMacAddress, out autoIPAddress))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, autoIPAddress))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                AutoIPPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template ID : 96182
        /// 1. Connect a device in a network with DHCP  server.
        /// 2. Ensure the device has acquired a DHCP IP
        /// 3. Change the config method to AUTO IP and enable the option " Send DHCP Request packets when Auto IP configured"
        /// Step 2:1. Connect a device in a network with DHCP  server.      
        /// 2. Ensure the device has acquired a DHCP IP        
        /// 3. Change the config method to AUTO IP and disable the option " Send DHCP Request packets when Auto IP configured"
        /// Expected :
        /// 1. The device should get configured with a Auto IP  since the option " Send DHCP Request packets when Auto IP configured" is disabled
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool DHCPToAutoIP(IPConfigurationActivityData activityData, int testNo)
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

            string currentPrinterIPAddress = activityData.PrimaryWiredIPv4Address;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Below steps are not applicable for TPS since 'Send DHCP Request on Auto IP' option is not supported
                if (!(PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily) || PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily)))
                {
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                    EwsWrapper.Instance().SetSendDHCPRequestOnAutoIP(true);
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.AUTOIP, validate: false);

                    if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP, activityData.PrimaryWiredIPv4Address))
                    {
                        return false;
                    }

                    EwsWrapper.Instance().SetSendDHCPRequestOnAutoIP(false);
                }

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.AUTOIP, validate: false);

                string autoIPAddress = string.Empty;

                if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out autoIPAddress))
                {
                    return false;
                }

                // To validate Printer Auto IP, client machine also needs to be in Auto IP
                // Hence DHCP server is stoppped.
                currentPrinterIPAddress = autoIPAddress;
                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                serviceMethod.Channel.StopDhcpServer();
                NetworkUtil.RenewLocalMachineIP();

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, autoIPAddress))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                // If Send DHCP requests option is disabled, Printer will not acquire IP from DHCP server even though it is UP.
                EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(currentPrinterIPAddress));
                EwsWrapper.Instance().SetSendDHCPRequestOnAutoIP(true);
                AutoIPPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template ID : 96183
        /// 1. Connect a device in a network with no DHCP  server.
        /// 2. Set the config method to Auto IP on the device and ensure device has taken Auto IP
        /// 3. Bring up a DHCP server and change the config method to DHCP on the device
        /// Expected :
        /// The device should get acquired with a DHCP IP from DHCP server
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool AutoIPToDHCP(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.StopDhcpServer();
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.AUTOIP, validate: false);

                string autoIPAddress = string.Empty;

                if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out autoIPAddress))
                {
                    return false;
                }

                NetworkUtil.RenewLocalMachineIP();

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, autoIPAddress))
                {
                    return false;
                }

                // WCF connection timeout might occur if above steps take long time. Creating new object to re-establish connection 
                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.StartDhcpServer();

                EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(autoIPAddress));
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address), validate: false);

                NetworkUtil.RenewLocalMachineIP();

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP, activityData.PrimaryWiredIPv4Address))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                AutoIPPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template ID : 96184
        /// Step 1:1. Connect a device in a network with Bootp  server.
        /// 2. Ensure the device has acquired a Bootp IP
        /// 3. Change the config method to AUTO IP
        /// Expected:
        /// 1. The device should get configured with an Auto IP
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool BootPToAutoIP(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                DhcpApplicationServiceClient serviceMethods = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethods.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
                serviceMethods.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Bootp);

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                string autoIPAddress = string.Empty;

                // When Configuration method is changed to Auto IP -
                // TPS : will acquire Auto IP, VEP : will acquire DHCP IP (so stopping DHCP server before changing configuration method)
                if (!SetAutoIPConfigMethod(activityData.PrimaryDhcpServerIPAddress, activityData.ProductFamily, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, out autoIPAddress))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, autoIPAddress))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                AutoIPPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template ID : 96185
        /// 1. Connect a device in a network with no Bootp  server.
        /// 2. Set the config method to Auto IP on the device and ensure device has taken Auto IP
        /// 3. Bring up a Bootp server and change the config method to Bootp on the device
        /// Expected :
        /// The device should get acquired with a Bootp IP from Bootp server
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool AutoIPToBootP(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string autoIPAddress = string.Empty;
                if (!ConfigurePrinterWithAutoIP(activityData, out autoIPAddress))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, autoIPAddress))
                {
                    return false;
                }

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.StartDhcpServer();

                EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(autoIPAddress));
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address), validate: false);

                NetworkUtil.RenewLocalMachineIP();

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.BOOTP))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                AutoIPPostRequisites(activityData);
            }
        }

        /// <summary>
        /// 656076	
        /// Template Verify Ipconfig method change from Manual to Manual
        /// Verify Ipconfig method change from Manual to Manual IP	
        /// "1.Assign a manual IP to the device.
        /// 2.Change the config method from manual to manual.
        /// 3. Assign a new manual IP  in the same subnet to the device.
        /// 4.Verify the new manual IP is set  in EWS and Control Panel."	
        /// "1.The device should be assigned with manual IP.
        /// 3.On changing the config method to manual, the device should again assigned with the new manual IP.
        /// 4. EWS page with new manual IP should be successful and in the control panel configuration page should be updated with the new manual IP."
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool ManualToManual(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                IPAddress firstManaulIPAddress = Network.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.PrimaryWiredIPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, firstManaulIPAddress))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, firstManaulIPAddress.ToString()))
                {
                    return false;
                }

                IPAddress secondManualIPAddress = Network.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.PrimaryWiredIPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, secondManualIPAddress))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, secondManualIPAddress.ToString()))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
            }
        }

        /// <summary>
        /// 96134	
        /// Template Verify device manual IP configuration behavior
        /// Verify manual IPv4 configuration on device	
        /// "1. Configure device via any method other than Manual.
        /// Using a device UI, set configuration to Manual 
        /// 2. Configure Auto IP address as manually on the device
        /// 3. Reconfigure to manual from any other configuration method and change the IP address "	
        /// Expected: 
        /// "1 Device should become Manual configured with same IP address as previously configured 
        /// 2. Should be able to manually configure Auto IP address on the device 
        /// 3. Device should become Manual configured with the new IP address
        /// Note:
        /// Before changing from any  config method ( DHCP/BootP/Auto IP) to manual, verify that the check box saying ""Use stateless DHCPv4 when manually configured is unchecked. 
        /// After changing to manual from any config method, verify that the check box( in Advanced Tab  under TCP/IP settings) saying ,""Verify use stateless DHCPV4  when manually configured"" is  Checked."
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool VerifyManualIPConfiguration(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            EwsWrapper.Instance().SetStatelessDHCPv4(false);

            string autoIPAddress = string.Empty;
            string currentDeviceAddress = activityData.PrimaryWiredIPv4Address;

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Stateless Dhcp Option is applicable for VEP, LFP. TPS: Not Applicable
                /* *********************************************************************
				 * Family			Config Method		Stateless Dhcp Option
				 * --------------------------------------------------------------------
				 *	VEP				Manual				Disabled
				 *	VEP				Dhcp/ BootP			Disabled
				 *	LFP				Manual				Disabled
				 *	LFP				Dhcp/ BootP			Disabled
				*/

                bool isTPSFamily = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily) ? true : false;
                bool isLfpFamily = PrinterFamilies.LFP.ToString().EqualsIgnoreCase(activityData.ProductFamily) ? true : false;
                bool isVepFamily = PrinterFamilies.VEP.ToString().EqualsIgnoreCase(activityData.ProductFamily) ? true : false;
                bool optionStatus;

                if (!isTPSFamily)
                {
                    optionStatus = EwsWrapper.Instance().GetStatelessDHCPv4();

                    if (optionStatus)
                    {
                        return false;
                    }

                }

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual))
                {
                    return false;
                }

                if (!isTPSFamily)
                {
                    optionStatus = EwsWrapper.Instance().GetStatelessDHCPv4();

                    if ((isLfpFamily && optionStatus) || (isVepFamily && optionStatus))
                    {
                        return false;
                    }
                }

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 60);
                EwsWrapper.Instance().SetStatelessDHCPv4(false);
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                if (!serviceMethod.Channel.StopDhcpServer())
                {
                    TraceFactory.Logger.Info("Failed to stop the DHCP Server.");
                    return false;
                }

                Thread.Sleep(TimeSpan.FromMinutes(LEASE_WAIT_TIME));
                NetworkUtil.RenewLocalMachineIP();

                if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out autoIPAddress))
                {
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(autoIPAddress);

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, validate: false))
                {
                    return false;
                }

                // Here the printer IP changes to auto IP.
                currentDeviceAddress = autoIPAddress;

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, autoIPAddress))
                {
                    return false;
                }

                if (!isTPSFamily)
                {
                    optionStatus = EwsWrapper.Instance().GetStatelessDHCPv4();

                    if ((isLfpFamily && optionStatus) || (isVepFamily && optionStatus))
                    {
                        return false;
                    }
                }

                EwsWrapper.Instance().SetStatelessDHCPv4(false);

                // Not validating the IP Config as the printer acquires a different IP(Auto IP).
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address), validate: false);

                // Bring up the DHCP server
                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                if (!serviceMethod.Channel.StartDhcpServer())
                {
                    TraceFactory.Logger.Info("Failed to stop the DHCP Server.");
                    return false;
                }

                Thread.Sleep(TimeSpan.FromMinutes(LEASE_WAIT_TIME));

                // Client will get a DHCP IP here.
                NetworkUtil.RenewLocalMachineIP();

                if (!CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP))
                {
                    return false;
                }

                if (!isTPSFamily)
                {
                    optionStatus = EwsWrapper.Instance().GetStatelessDHCPv4();

                    if (optionStatus)
                    {
                        return false;
                    }
                }

                IPAddress manualIPAddress = Network.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.PrimaryWiredIPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, manualIPAddress))
                {
                    return false;
                }

                currentDeviceAddress = manualIPAddress.ToString();

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, manualIPAddress.ToString()))
                {
                    return false;
                }

                if (!isTPSFamily)
                {
                    optionStatus = EwsWrapper.Instance().GetStatelessDHCPv4();

                    if ((isLfpFamily && optionStatus) || (isVepFamily && optionStatus))
                    {
                        return false;
                    }
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);

                // If the current device address is auto IP, if the DHCP server is up, bring down the server and set the default IP config method.
                if (currentDeviceAddress == autoIPAddress)
                {
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.AUTOIP, validate: false);

                    DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                    serviceMethod.Channel.StopDhcpServer();

                    NetworkUtil.RenewLocalMachineIP();

                    // Validation is not performed here since the printer might acquire a dhcp IP here.
                    EwsWrapper.Instance().SetDefaultIPConfigMethod(false, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                    AutoIPPostRequisites(activityData);
                }
                else
                {
                    EwsWrapper.Instance().SetDefaultIPConfigMethod(false, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                }

                CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address);
            }
        }

        #endregion

        #region Network Hosebreak

        /// <summary>
        /// Template Id: 96193
        /// Template Verify netstack reset on hose break
        /// Verify netstack reset on hose break	
        /// "1. Connect the device in a network with both Bootp and DHCP server
        /// 2. Ensure device has acquired an IP address.
        /// 3. Disconnect the network cable from he device and connect it back after a minute"	
        /// "1.Device should do netstack reset and should start sending bootp packets. 
        /// If Bootp server is not present then should send DHCP packets. 
        /// 2.Device should get configured with a valid IP address
        /// Note: this testcase is only for TPS"
        /// </summary>
        /// <returns></returns>
        public static bool VerifyNetStackResetOnHoseBreak(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                IPConfigMethod configMethod = EwsWrapper.Instance().GetIPConfigMethod();

                // validate ping, EWS, Telnet, SNMP 
                if (!ValidateIPConfigMethod(activityData, configMethod))
                {
                    return false;
                }

                // Perform Hose break
                if (!Utility.PerformHoseBreak(activityData.PrimaryWiredIPv4Address, activityData.SwitchIP, activityData.PrimaryInterfacePortNo, HOSEBREAK_TIMEOUT))
                {
                    return false;
                }

                // Validate IP Config method
                if (!ValidateIPConfigMethod(activityData, configMethod))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
            }
        }

        /// <summary>
        /// 494361	
        /// Template Verify network hose  break from the same network using AutoIP		
        /// Step 1: Hose break from one network to another network using AutoIP	"
        /// 1. Bring down the both BootP and DHCP server
        /// 2. Verify the device got Auto IP
        /// 3. Remove the network cable from same  network and connect it to the same network 
        /// 4. Verify the device got Auto IP"	
        /// Device should send DHCP discover again after connecting to the another network  and device should Acquire a  new Auto IP Address.	
        /// Expected varies is not matching with the test steps
        /// </summary>
        /// <returns></returns>
        public static bool HoseBreakSameNetworkUsingAutoIP(IPConfigurationActivityData activityData, int testNo)
        {
            string autoIPAddress = string.Empty;

            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            bool result = false;
            PacketDetails validatePacketDetails = null;
            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                //  Bring the printer in auto IP configuration
                if (!ConfigurePrinterWithAutoIP(activityData, out autoIPAddress))
                {
                    return false;
                }

                // Validate the IP Config method
                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, autoIPAddress))
                {
                    return false;
                }

                // Client will lose the connection to the switch after the DHCP Server is brought down.Fetch the next pingable IP of the switch
                // Fetch the vlan numbers of second dhcp server and linux server from the activityData.VirtualLanDetails
                int secondDhcpSeverVirtualLanId = GetVlanNumber(activityData, activityData.SecondDhcpServerIPAddress);
                int linuxVirtualLanId = GetVlanNumber(activityData, activityData.LinuxServerIPAddress);

                string alternateSwitchIpAddress = (from item in activityData.VirtualLanDetails
                                                   where (item.Key.Equals(secondDhcpSeverVirtualLanId) | item.Key.Equals(linuxVirtualLanId))
                                                   && Network.NetworkUtil.PingUntilTimeout(IPAddress.Parse(item.Value), 1)
                                                   select item.Value).FirstOrDefault().ToString();

                string validateGuid = string.Empty;

                try
                {
                    validateGuid = client.Channel.TestCapture(activityData.SessionId, string.Format("{0}_Dhcp_Validation", testNo));

                    if (!Utility.PerformHoseBreak(autoIPAddress, alternateSwitchIpAddress, activityData.PrimaryInterfacePortNo, HOSEBREAK_TIMEOUT))
                    {
                        return false;
                    }
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(2));
                        validatePacketDetails = client.Channel.Stop(validateGuid);
                    }
                }

                // Validate the IP Config method
                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, autoIPAddress))
                {
                    return false;
                }

                result = true;
            }
            finally
            {
                client.Channel.Stop(guid);

                if (result)
                {
                    result = ValidateDhcpDiscoverPacket(activityData.PrimaryDhcpServerIPAddress, validatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress);
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                AutoIPPostRequisites(activityData);
            }

            return result;
        }

        /// <summary>
        /// Performs hose break in same network with different server configurations.
        /// The below templates are covered in this method.
        /// 494362	
        /// Template Verify network hose  break from the same network using BootP IP		
        /// Host break from the same  network	
        /// "1. Bring the device with  Bootp server running  on 2008 server
        /// 2. Verify the device got Bootp IP
        /// 3. Remove the network cable from Bootp 2008 server network and connect it back to the same Bootp server.
        /// 4. Verify the device got Bootp IP "
        /// "Device should send new Bootp Request again after connecting back to the same Bootp Server and device should Acquire a  new Bootp IP Address."
        /// Step 2 : check the Parameter	
        /// Connect back the device to the same network as per the 1st step	Device should get updated with same Parameters from the same Bootp Server
        /// 494363	
        /// Template Verify network hose  break from the same network using DHCP IP			
        /// Step 1  Host break from the same  network	
        /// "1. Bring the device in DHCP server Running setup
        /// 2. Verify the device got DHCP IP
        /// 3. Remove the network cable from DHCP  server network and connect it back to the same DHCP server.
        /// 4. Verify the device got DHCP IP
        /// "Device should send DHCP release packet first and then send DHCP discover process  after connecting back to the same DHCP Server and device should Acquire a  DHCP IP Address.
        /// Step 2 : check the parameters	
        /// Connect back the device to the same network as per the 1st step	Device should get updated with same Parameters from the same DHCP Server
        /// </summary>
        /// <returns></returns>
        public static bool VerifyHoseBreakSameNetwork(IPConfigurationActivityData activityData, IPConfigMethod configMethod, int testNo, PrinterConnectionType interfaceConnection = PrinterConnectionType.PrimaryWiredInterface, bool cleanUp = true)
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

            bool result = false;
            PacketDetails validatePacketDetails = null;
            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {

				Collection<PrinterInterfaceConfigurationDetails> interfaceDetails = GetInterfaceDetails(activityData);
				string address = interfaceDetails.FirstOrDefault(x => x.PrinterInterface == interfaceConnection).Address;
				string macAddress = interfaceDetails.FirstOrDefault(x => x.PrinterInterface == interfaceConnection).MacAddress;
				int portNo = interfaceDetails.FirstOrDefault(x => x.PrinterInterface == interfaceConnection).PortNumber;

				if (interfaceConnection != PrinterConnectionType.PrimaryWiredInterface)
				{
					EwsWrapper.Instance().ChangeDeviceAddress(address);
				}
				else
				{
					TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
				}

                EwsWrapper.Instance().SetIPConfigMethod(configMethod, expectedIPAddress: IPAddress.Parse(address));

                TraceFactory.Logger.Info("Creating Reservation in {0} Server for {0}.".FormatWith(configMethod));

                // Create Reservation for the printer for the specified config method. 
                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, address, macAddress);

                ReservationType reservationType = IPConfigMethod.DHCP.Equals(configMethod) ? ReservationType.Dhcp : ReservationType.Bootp;

                if (!serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, address, macAddress, reservationType))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, configMethod, address))
                {
                    return false;
                }

                string validateGuid = string.Empty;

                try
                {
                    validateGuid = client.Channel.TestCapture(activityData.SessionId, string.Format("{0}_Bootp_Validation", testNo));

                    // Perform Hose break
                    if (!Utility.PerformHoseBreak(address, activityData.SwitchIP, portNo, HOSEBREAK_TIMEOUT))
                    {
                        return false;
                    }
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(2));
                        validatePacketDetails = client.Channel.Stop(validateGuid);
                    }
                }

                // Validate IP Config method
                if (!ValidateIPConfigMethod(activityData, configMethod, address))
                {
                    return false;
                }

                result = true;
            }
            finally
            {
                client.Channel.Stop(guid);

                if (result)
                {
                    if (IPConfigMethod.BOOTP.Equals(configMethod))
                    {
                        result = ValidateBootPPackets(activityData.PrimaryDhcpServerIPAddress, validatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress);
                    }
                    else
                    {
                        result = ValidateDhcpPackets(activityData.PrimaryDhcpServerIPAddress, validatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress, activityData.ProductFamily);
                    }
                }

				if (cleanUp)
				{
					TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

					// Create Reservation for the printer for both DHCP and BOOTP in DHCP server

					DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
					serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, DEFAULT_LEASE_TIME);
					serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
					serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Both);

					EwsWrapper.Instance().SetDefaultIPConfigMethod(false, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

					// When the config method is DHCP(Test ID: 494363), after creating reservation for both printer takes 10 mins to come up.
					if (configMethod == IPConfigMethod.DHCP)
					{
						Thread.Sleep(TimeSpan.FromMinutes(12));
						CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address, TimeSpan.FromSeconds(30));
					}
				}
            }

            return result;
        }

        /// <summary>
        /// 655989	
        /// Template Verify network hose  break from the same network using Manual IP
        /// Host break from the same  network using Manual IP	
        /// "1. Change the config method of the device to 'Manual' IP
        /// 2. Verify that the device as  assigned to Manual IP.
        /// 3. Remove the network cable from the device  and re-connect it to the same network."	
        /// "2.The device should assigned to manual IP.
        /// 3.On reconnecting the cable, the device should still retain the same manual IP which it had previously."
        /// </summary>
        /// <returns></returns>
        public static bool HoseBreakSameNetworkUsingManualIP(IPConfigurationActivityData activityData, int testNo)
        {
            IPAddress manualIPAddress = null;
            bool isManualIpSet = false;

            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Setting IP Config method to Manual IP.
                manualIPAddress = Network.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.PrimaryWiredIPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, manualIPAddress);

                // Validate if the printer acquired the manual IP
                isManualIpSet = CheckForPrinterAvailability(manualIPAddress.ToString());

                // Validate IP Config method
                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, manualIPAddress.ToString()))
                {
                    return false;
                }

                // Perform Hose break
                if (!Utility.PerformHoseBreak(manualIPAddress.ToString(), activityData.SwitchIP, activityData.PrimaryInterfacePortNo, HOSEBREAK_TIMEOUT))
                {
                    return false;
                }

                // Validate IP Config method
                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, manualIPAddress.ToString()))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                if (isManualIpSet)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(manualIPAddress.ToString());
                    EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                }
                else
                {
                    EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                }
            }
        }

        /// <summary>
        /// Performs Hose break from with different server configurations
        /// The below templates are covered under this method.
        /// 463305	
        /// Template Verify network hose  break from one network to another network using DHCP			
        /// Hose break from one network to another network using DHCP and connecting back to the first network.	
        /// "1. Bring up the 2K8 DHCP server with Hostname, Domain name and primary DNS server IP and Secondary DNS server IP etc.
        /// 2. Bring up the 2K12 DHCP server with  Different Hostname, Different Domain name and  Different primary and Secondary DNS server IP etc.
        /// 3. Bring up the device in the 2K8 DHCP Server network using first network and wait for few minutes.
        /// 4. Verify the device behavior with IP address and parameters and verify ping.
        /// 5. Remove  the network cable from the DHCP 2K8 server network and connect it to second network where 2K12 DHCP server is configured. 
        /// 6. Wait for few minutes and verify the device behavior with IP address and parameters and verify ping.
        /// 7. Remove the network cable from the 2K12 DHCP server and connect it back to the first server with 2K8 DHCP server.
        /// 8. Wait for few minutes and verify the device behavior with IP address and parameters and verify ping.
        /// "	"2. Configuration for the both 2K8 and 2K12 server should be successful.
        /// 4. The device should get DHCP IP and should be configured with the parameters set in the 2K8 DHCP server's.
        /// 6. The device should send new DHCP discover again after connecting to the second 2K12 DHCP Server and  the device should acquire a  new DHCP IP Address.
        ///		The device should be configured with the parameters set in the 2K12 DHCP server. 
        ///		Ping should be success after connecting to the new network
        /// 8. Once again device should send DHCP discover after connecting to the first 2K8 DHCP Server and  the device should acquire a  new DHCP IP Address.
        ///		The device should be configured with the parameters set in the 2K8 server and        
        ///		Ping should be success after connecting to the new network"
        /// 474098	
        /// Template Verify network hose  break from one network to another network using BootP	
        /// Hose break from one network to another network using BootP and connecting back to the first network.	
        /// "1. Bring up the 2K8 BootP server with Hostname, Domain name and primary DNS server IP and Secondary DNS server IP etc.
        /// 2. Bring up the 2K12 BootP server with  Different Hostname, Different Domain name and  Different primary and Secondary DNS server IP etc.
        /// 3. Bring up the device in the 2K8 Bootp Server network using first network and wait for few minutes.
        /// 4. Verify the device behavior with IP address and parameters.
        /// 5. Remove  the network cable from the Bootp 2K8 server network and connect it to second network where Bootp server 2K12 is configured. 
        /// 6. Wait for few minutes and verify the device behavior with IP address and parameters  and verify with ping
        /// 7. Remove the network cable from the 2K12 server and connect it back to the first server with 2K8 server.
        /// 8. Wait for few minutes and verify the device behavior with IP address and parameters and verify with ping"
        /// "1. Configuration for the both 2K8 and 2K12 server should be successful.
        /// 4. The device should get BootP IP and should be configured with the parameters set in the 2K8 BootP server's.
        /// 6. The device should send new Bootp Request again after connecting to the second 2K12 Bootp Server and  the device should acquire a  new Bootp IP Address.The device should be configured with the parametes set in the 2K12 server.  Ping should be success after connecting to the new network
        /// 8. Once again device should send new Bootp Request after connecting to the second 2K8 Bootp Server and  the device should acquire a  new Bootp IP Address.The device should be configured with the parametes set in the 2K8 server.  Ping should be success after connecting to the new network "
        /// </summary>
        /// <returns></returns>
        /// </summary>
        /// <returns></returns>
        public static bool VerifyHosBreakAcrossNetworks(IPConfigurationActivityData activityData, IPConfigMethod configMethod, int testNo)
        {
            DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string reservedIpInSecondDhcpServer = string.Empty;
            bool result = false;

            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            PacketDetails validatePacketDetails = null;
            PacketDetails bootPValidatePacketDetails = null;
            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            PacketCaptureServiceClient secondClient = PacketCaptureServiceClient.Create(activityData.SecondDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info("STEP1 : Printer is connected to first {0} Server.".FormatWith(configMethod));

                // Create Reservation for the printer for the specified config method. 
                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
                ReservationType reservationType = IPConfigMethod.DHCP.Equals(configMethod) ? ReservationType.Dhcp : ReservationType.Bootp;

                if (!serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, reservationType))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetIPConfigMethod(configMethod, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    TraceFactory.Logger.Info("Printer is not configured with {0}".FormatWith(configMethod));
                    return false;
                }

                TraceFactory.Logger.Info("Validating the server parameters on the printer configured from Dhcp Server : {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));

                // Get the values from first DHCP Server
                string firstServerHostname = serviceMethod.Channel.GetHostName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);
                string firstServerDomainName = serviceMethod.Channel.GetDomainName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);
                string firstServerprimaryDnsServer = serviceMethod.Channel.GetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);
                string firstServersecondryDnsServer = serviceMethod.Channel.GetSecondaryDnsServer(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                // Validate the server parameters on the printer.
                if (!(ValidateHostName(family, firstServerHostname) && ValidateDomainName(family, firstServerDomainName)
                    && ValidatePrimaryDnsServer(family, firstServerprimaryDnsServer) && ValidateSecondaryDnsServer(family, firstServersecondryDnsServer)))
                {
                    return false;
                }

                if (!CreateReservation(activityData.SecondDhcpServerIPAddress, activityData.PrimaryMacAddress, reservationType, ref reservedIpInSecondDhcpServer))
                {
                    return false;
                }

                TraceFactory.Logger.Info("STEP2 : Connecting the printer to the second DHCP Server : {0}.".FormatWith(activityData.SecondDhcpServerIPAddress));

                string validateGuid = string.Empty;

                try
                {
                    // Validation is required for both DHCP and BootP templates
                    validateGuid = secondClient.Channel.TestCapture(activityData.SessionId, string.Format("{0}_Bootp_Validation", testNo));

                    // Perform Hose break across networks
                    if (!Utility.PerformHoseBreakAcrossNetworks(activityData.SwitchIP, activityData.PrimaryInterfacePortNo, activityData.SecondDhcpServerIPAddress))
                    {
                        return false;
                    }
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(2));
                        validatePacketDetails = secondClient.Channel.Stop(validateGuid);
                    }
                }

                if (!Utility.ValidateHoseBreakAcrossNetworks(activityData.SecondDhcpServerIPAddress, activityData.PrimaryMacAddress, ref reservedIpInSecondDhcpServer))
                {
                    return false;
                }

                // The printer is expected to get the reserved ip address in second server
                if (!reservedIpInSecondDhcpServer.EqualsIgnoreCase(reservedIpInSecondDhcpServer))
                {
                    TraceFactory.Logger.Info("Printer failed to acquire the IP Address {0} in DHCP server {1}".FormatWith(reservedIpInSecondDhcpServer, activityData.SecondDhcpServerIPAddress));
                    return false;
                }

                // Create the EWS, Telnet and SNMP Wrapper instances for the new IP Address.
                EwsWrapper.Instance().ChangeHostName(reservedIpInSecondDhcpServer);
                TelnetWrapper.Instance().Create(reservedIpInSecondDhcpServer);
                SnmpWrapper.Instance().Create(reservedIpInSecondDhcpServer);

                // Connect to the second server and fetch the server parameters.
                serviceMethod = DhcpApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress);

                // Get the values from second DHCP Server
                string secondDhcpServerScope = serviceMethod.Channel.GetDhcpScopeIP(activityData.SecondDhcpServerIPAddress);
                string secondServerHostname = serviceMethod.Channel.GetHostName(activityData.SecondDhcpServerIPAddress, secondDhcpServerScope);
                string secondServerDomainName = serviceMethod.Channel.GetDomainName(activityData.SecondDhcpServerIPAddress, secondDhcpServerScope);
                string secondServerPrimaryDnsServer = serviceMethod.Channel.GetPrimaryDnsServer(activityData.SecondDhcpServerIPAddress, secondDhcpServerScope);
                string secondServerSecondryDnsServer = serviceMethod.Channel.GetSecondaryDnsServer(activityData.SecondDhcpServerIPAddress, secondDhcpServerScope);

                TraceFactory.Logger.Info("Validating the server parameters on the printer configured from Dhcp Server : {0}.".FormatWith(activityData.SecondDhcpServerIPAddress));

                // Validate the second server parameters on the printer.
                if (!(ValidateHostName(family, secondServerHostname) && ValidateDomainName(family, secondServerDomainName)
                    && ValidatePrimaryDnsServer(family, secondServerPrimaryDnsServer) && ValidateSecondaryDnsServer(family, secondServerSecondryDnsServer)))
                {
                    return false;
                }

                TraceFactory.Logger.Info("STEP3 : Connecting the printer back to the first DHCP Server : {0} network.".FormatWith(activityData.PrimaryDhcpServerIPAddress));

                string validateGuid1 = string.Empty;

                try
                {
                    validateGuid1 = client.Channel.TestCapture(activityData.SessionId, string.Format("{0}_Bootp_Validation", testNo));

                    // Perform Hose break across networks
                    if (!Utility.PerformHoseBreakAcrossNetworks(activityData.SwitchIP, activityData.PrimaryInterfacePortNo, activityData.PrimaryDhcpServerIPAddress))
                    {
                        return false;
                    }
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid1))
                    {
                        bootPValidatePacketDetails = client.Channel.Stop(validateGuid1);
                    }
                }

                // Printer is expected to acquire the default ipv4 address after connecting back to first dhcp server.
                string defaultIpAddress = activityData.PrimaryWiredIPv4Address;

                if (!Utility.ValidateHoseBreakAcrossNetworks(activityData.PrimaryDhcpServerIPAddress, activityData.PrimaryMacAddress, ref defaultIpAddress))
                {
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                TelnetWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);
                SnmpWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);

                TraceFactory.Logger.Info("Validating the server parameters on the printer configured from Dhcp Server : {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));

                // Validate the first server parameters on the printer.
                if (!(ValidateHostName(family, firstServerHostname) && ValidateDomainName(family, firstServerDomainName)
                    && ValidatePrimaryDnsServer(family, firstServerprimaryDnsServer) && ValidateSecondaryDnsServer(family, firstServersecondryDnsServer)))
                {
                    return false;
                }

                result = true;
            }
            finally
            {
                PacketDetails details = client.Channel.Stop(guid);

                if (result)
                {
                    if (IPConfigMethod.BOOTP.Equals(configMethod))
                    {
                        result = ValidateBootPPackets(activityData.SecondDhcpServerIPAddress, validatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress)
                                 && ValidateBootPPackets(activityData.PrimaryDhcpServerIPAddress, bootPValidatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress, false);
                    }
                    else
                    {
                        result = ValidateDhcpPackets(activityData.SecondDhcpServerIPAddress, validatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress, activityData.ProductFamily)
                                 && ValidateDhcpPackets(activityData.PrimaryDhcpServerIPAddress, bootPValidatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress, activityData.ProductFamily, false);
                    }
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                // delete the reservation in second dhcp server
                serviceMethod = DhcpApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress);
                serviceMethod.Channel.DeleteReservation(activityData.SecondDhcpServerIPAddress, serviceMethod.Channel.GetDhcpScopeIP(activityData.SecondDhcpServerIPAddress), reservedIpInSecondDhcpServer, activityData.PrimaryMacAddress);

                //Create Reservation in first dhcp server for both dhcp and boot config methods.
                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
                serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Both);

                // TODO : Get the vlan on which the printer is connected. If it is not the default vlan change it to default.
                PostRequisiteHoseBreakAcrossNetworks(activityData);

                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
            }

            return result;
        }

        /// <summary>
        /// 474101	
        /// Template Verify network hose  break from one network to another network using AutoIP
        /// Host break from one network to another network using AutoIP and coming back to first network.	
        /// "1. Bring down the both BootP and DHCP server 
        /// 2. Verify that the  device gets Auto IP
        /// 3. Remove the network cable from the first network and connect it to second network.
        /// 4.Remove the network cable from second network and connect it back to the first network"	
        /// "2.The device should acquire Auto IP.
        /// 3. Device should send DHCP discover again after connecting to the another network  and device should Acquire the same  Auto IP Address as before. 
        /// If any other device in the network has this Auto IP, then the device should acquire a new Auto IP.
        /// 4. The device should acquire the same Auto IP as before. If any other device in the network has this Auto IP, then the device should acquire a new Auto IP."
        /// </summary>
        /// <returns></returns>
        public static bool HoseBreakAcrossNetworksUsingAutoIP(IPConfigurationActivityData activityData, int testNo)
        {
            string autoIPAddress = string.Empty;

            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            DhcpApplicationServiceClient serviceMethods = DhcpApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress);

            bool result = false;
            PacketDetails secondServerPacketDetails = null;
            PacketDetails primaryServerPacketDetails = null;
            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            PacketCaptureServiceClient secondClient = PacketCaptureServiceClient.Create(activityData.SecondDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                //  Bring the printer in auto IP configuration
                if (!ConfigurePrinterWithAutoIP(activityData, out autoIPAddress))
                {
                    return false;
                }

                // Validate the IP Configuration method
                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, autoIPAddress))
                {
                    return false;
                }

                // Before stopping the dhcp server route the IP through linux server
                Utility.AddSourceIPAddress(activityData.SecondDhcpServerIPAddress, activityData.LinuxServerIPAddress);

                // Bring down the second DHCP Server
                if (!serviceMethods.Channel.StopDhcpServer())
                {
                    return false;
                }

                // Client will lose the connection to the switch after the DHCP Server is brought down.
                // Fetch the next ping-able IP of the switch
                TraceFactory.Logger.Debug("Fetching the second VLAN IP Address as the client is in auto IP");
                int linuxSeverVirtualLanId = GetVlanNumber(activityData, activityData.LinuxServerIPAddress);

                string linuxSeverVirtualLanIpAddress = (from item in activityData.VirtualLanDetails where item.Key.Equals(linuxSeverVirtualLanId) select item.Value).FirstOrDefault().ToString();

                // Perform Hose break across networks
                string acquiredIpAddress = string.Empty;
                string validateGuid = string.Empty;

                try
                {
                    validateGuid = secondClient.Channel.TestCapture(activityData.SessionId, string.Format("{0}_Dhcp_Validation", testNo));

                    if (!Utility.PerformHoseBreakAcrossNetworks(linuxSeverVirtualLanIpAddress, activityData.PrimaryInterfacePortNo, activityData.SecondDhcpServerIPAddress))
                    {
                        return false;
                    }
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(2));
                        secondServerPacketDetails = secondClient.Channel.Stop(validateGuid);
                    }
                }

                try
                {
                    validateGuid = client.Channel.TestCapture(activityData.SessionId, string.Format("{0}_Dhcp_Validation", testNo));

                    // Connect the printer back to the first network.
                    if (!Utility.PerformHoseBreakAcrossNetworks(linuxSeverVirtualLanIpAddress, activityData.PrimaryInterfacePortNo, activityData.PrimaryDhcpServerIPAddress, activityData.PrimaryMacAddress, out acquiredIpAddress))
                    {
                        return false;
                    }
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(2));
                        primaryServerPacketDetails = client.Channel.Stop(validateGuid);
                    }
                }

                // Validate the IP Configuration method
                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, acquiredIpAddress))
                {
                    return false;
                }

                result = true;
            }
            finally
            {
                client.Channel.Stop(guid);

                if (result)
                {
                    result = ValidateDhcpDiscoverPacket(activityData.SecondDhcpServerIPAddress, secondServerPacketDetails.PacketsLocation, activityData.PrimaryMacAddress)
                            && ValidateDhcpDiscoverPacket(activityData.PrimaryDhcpServerIPAddress, primaryServerPacketDetails.PacketsLocation, activityData.PrimaryMacAddress);
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                // Bring up the second DHCP Server. The connection would have timeout by the time control reaches this point.
                // So re-creating the service instance.
                serviceMethods = DhcpApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress);
                serviceMethods.Channel.StartDhcpServer();
                AutoIPPostRequisites(activityData);
            }

            return result;
        }

        /// <summary>
        /// 655990	
        /// Template Verify network hose  break from one network to another network using different Manual IP		
        /// Verify Host break from one network to the other  network using different Manual IP	
        /// "1. Change the config method of the device to 'Manual' IP
        /// 2. Verify that the device has assigned to Manual IP.
        /// 3. Access the EWS page of the device and change the manual IP to a different subnet (For eg if the previous manual IP was 192.168.14.10 , change the manual IP to 192.168.15.10)
        /// 4. Now remove the network cable of the device from the network it is connected to and connect it a different network (which has the differnet subnet of last assigned manual IP)
        /// Example:  If the device is now connected to 14 subnet (192.168.14.0 network  ),disconnect the cable and connect it to a different subnet as the last manually configured IP ie 192.168.15.0 network) "	
        /// " 1.The device should assigned with manual IP.
        /// 3. On changing the manual IP using the EWS page, the device would lose connectivity( i.e. pinging the device may not work as  the IP assigned does not match the scope of the subnet it is connected to .
        /// 4.On reconnecting the cable to a different network ,the device should retain the configured manual IP which it had previously and ping should success."
        /// </summary>
        /// <returns></returns>
        public static bool HoseBreakAcrossNetworksUsingDifferentManualIP(IPConfigurationActivityData activityData, int testNo)
        {
            IPAddress manualIPAddress = null;
            IPAddress secondManualIPAddress = null;
            bool isFirstManualIpSet = false;
            bool isSecondManualIpSet = false;

            // Added as part of code analysis
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Setting IP Config method to Manual IP.
                manualIPAddress = Network.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.PrimaryWiredIPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, manualIPAddress);

                // Validate if the printer acquired the manual IP
                isFirstManualIpSet = CheckForPrinterAvailability(manualIPAddress.ToString());

                // Validate IP Config method
                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, manualIPAddress.ToString()))
                {
                    return false;
                }

                // Change the manual IP to a different subnet
                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress);
                string dhcpScopeIP = serviceMethod.Channel.GetDhcpScopeIP(activityData.SecondDhcpServerIPAddress);
                IPAddress subnetMask = IPAddress.Parse(activityData.SecondDhcpServerIPAddress).GetSubnetMask();
                IPAddress defaultGateway = IPAddress.Parse(serviceMethod.Channel.GetRouterAddress(activityData.SecondDhcpServerIPAddress, dhcpScopeIP));

                // Setting IP Config method to Manual IP.
                secondManualIPAddress = Network.NetworkUtil.FetchNextIPAddress(subnetMask, defaultGateway);
                secondManualIPAddress = Network.NetworkUtil.FetchNextIPAddress(subnetMask, secondManualIPAddress);
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, secondManualIPAddress, subnetMask.ToString(), defaultGateway.ToString(), false);

                TraceFactory.Logger.Info("Expected : Ping failure with the manual IP {0}.".FormatWith(secondManualIPAddress));
                TraceFactory.Logger.Info("Actual :");

                if (CheckForPrinterAvailability(secondManualIPAddress.ToString()))
                {
                    return false;
                }

                // Validations are not performed using EWS, Telnet and SNMP since the printer is not available.
                // Perform Hose break across networks
                string acquiredIpAddress = string.Empty;
                if (!Utility.PerformHoseBreakAcrossNetworks(activityData.SwitchIP, activityData.PrimaryInterfacePortNo, activityData.SecondDhcpServerIPAddress, activityData.PrimaryMacAddress, out acquiredIpAddress))
                {
                    return false;
                }

                // Check for the printer availability
                if (CheckForPrinterAvailability(secondManualIPAddress.ToString()))
                {
                    isFirstManualIpSet = false;
                    isSecondManualIpSet = true;
                }
                else
                {
                    return false;
                }

                // Validate IP Config method
                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, secondManualIPAddress.ToString(), false))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                // TODO: Ambily review is required here.
                IPAddress printerIPAddress = isFirstManualIpSet ? manualIPAddress : (isSecondManualIpSet ? secondManualIPAddress : IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                EwsWrapper.Instance().ChangeDeviceAddress(printerIPAddress);
                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                PostRequisiteHoseBreakAcrossNetworks(activityData);
            }
        }

        /// <summary>
        /// 656235	
        /// Template Verify network hose  break while device is switching to different servers				
        /// Step 1: Verify network hose break while switching  the device from bootp network to DHCP network	
        /// "1.Bring up the device in a network where BootP server is available.
        /// 2.Disconnect the device from BootP network.
        /// 3. Connect it to a network where DHCP server is available."	
        /// "1. The device should get BootP IP.
        /// 3. The device should initially try with BootP request, since no BootP server available in the Network, then Device should try to  DHCP IP and Device should get DHCP IP successfully."	
        /// Step 2: Verify network hose break while switching  the device from DHCP network to BootP network	
        /// "1.Bring up the device in a network where DHCP server is available.
        /// 2.Disconnect the device from DHCP network.
        /// 3. Connect it to a network where BootP server is available."	
        /// "1. The device should get DHCP IP.
        /// 3. The device should initially try with DHCP process, since no DHCP server available in the Network, then Device should acquire a AutoIP."
        /// </summary>
        /// <returns></returns>
        public static bool HoseBreakDifferentServers(IPConfigurationActivityData activityData, int testNo)
        {
            DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string reservedIpInSecondServer = string.Empty;
            string hostName = string.Empty;

            // Added as part of code analysis
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("STEP1 : Connecting the device in BOOTP network");

                if (!(serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress) &&
                    serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Bootp)))
                {
                    return false;
                }

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                if (!EwsWrapper.Instance().ValidateConfigMethod(IPConfigMethod.BOOTP.ToString()))
                {
                    TraceFactory.Logger.Info("Printer is not configured with BOOTP");
                    return false;
                }

                if (!CreateReservation(activityData.SecondDhcpServerIPAddress, activityData.PrimaryMacAddress, ReservationType.Dhcp, ref reservedIpInSecondServer))
                {
                    return false;
                }

                TraceFactory.Logger.Info("STEP2 : Connecting the printer to the second DHCP Server : {0} network.".FormatWith(activityData.SecondDhcpServerIPAddress));

                // Perform Hose break across networks
                if (!Utility.PerformHoseBreakAcrossNetworks(activityData.SwitchIP, activityData.PrimaryInterfacePortNo, activityData.SecondDhcpServerIPAddress))
                {
                    return false;
                }

                if (!Utility.ValidateHoseBreakAcrossNetworks(activityData.SecondDhcpServerIPAddress, activityData.PrimaryMacAddress, ref reservedIpInSecondServer))
                {
                    return false;
                }

                // Create the EWS, Telnet and SNMP Wrapper instances for the new IP Address.
                EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(reservedIpInSecondServer));
                TelnetWrapper.Instance().Create(reservedIpInSecondServer);
                SnmpWrapper.Instance().Create(reservedIpInSecondServer);

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP, reservedIpInSecondServer))
                {
                    return false;
                }

                TraceFactory.Logger.Info("STEP3 : Connecting the device in BOOTP network");

                if (!EwsWrapper.Instance().GetIPConfigMethod().Equals(IPConfigMethod.DHCP))
                {
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address), validate: false);
                }

                // TPS will acquire BOOTP IP here, for VEP host name is set to access the EWS as the printer goes to auto IP.
                if (!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    // Set a random hostName so that the device EWS page can be accessed after hose break with hostname.
                    hostName = "host{0}".FormatWith(new Random().Next(10000));
                    EwsWrapper.Instance().SetHostname(hostName);
                }

                // Perform Hose break across networks. Validation is not performed here as the printer goes to auto ip and will not be discovered since the dhcp server is up.
                if (!Utility.PerformHoseBreakAcrossNetworks(activityData.SwitchIP, activityData.PrimaryInterfacePortNo, activityData.PrimaryDhcpServerIPAddress))
                {
                    return false;
                }

                // Validate BOOTP IP for TPS
                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    if (!CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address))
                    {
                        return false;
                    }

                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
                    return ValidateIPConfigMethod(activityData, IPConfigMethod.BOOTP);
                }
                else
                {
                    // Open EWS using the newly set hostname
                    EwsWrapper.Instance().ChangeHostName(hostName);

                    // Check if the IP is auto ip
                    if (EwsWrapper.Instance().GetIPConfigMethod().Equals(IPConfigMethod.AUTOIP))
                    {
                        TraceFactory.Logger.Info("Printer has acquired an auto ip address.");
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Printer has not acquired an auto ip address.");
                        return false;
                    }
                }
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                // delete Reservation from second DHCP Server.
                serviceMethod = DhcpApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress);
                serviceMethod.Channel.DeleteReservation(activityData.SecondDhcpServerIPAddress, serviceMethod.Channel.GetDhcpScopeIP(activityData.SecondDhcpServerIPAddress), reservedIpInSecondServer, activityData.PrimaryMacAddress);

                // Create Reservation in first server for both dhcp and bootp
                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
                serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Both);

                // Wait delay is not required for TPS as it is already in Bootp.
                if (!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    Thread.Sleep(LEASE_WAIT_TIME);
                }

                if (!CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address))
                {
                    // TODO : Get the vlan on which the printer is connected. If it is not the default vlan change it to default.
                    PostRequisiteHoseBreakAcrossNetworks(activityData);
                }
                else
                {
                    TraceFactory.Logger.Info("Ping Successful with {0}".FormatWith(activityData.PrimaryWiredIPv4Address));
                }

                // Cold reset is not required for TPS as host name is not changed.
                if (!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);

                    // Perform cold reset on the printer so that the manually configured host name will be cleared.
                    if (EwsWrapper.Instance().GetHostname().EqualsIgnoreCase(hostName))
                    {
                        ColdReset(activityData, printer);
                        // After cold reset enabling WSDiscovery option
                        EwsWrapper.Instance().SetWSDiscovery(true);
                    }
                }

                // Create the EWS, Telnet and SNMP Wrapper instances for the new IP Address.
                EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                TelnetWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);
                SnmpWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);

                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
            }
        }

        /// <summary>
        /// 657162	
        /// Template Verify network hose  break from one network to another network and coming back to the first network using manual IP				
        /// verify host break from one network to another network using same Manul IP and coming back to the first network.	
        /// "1. Change the config method of the device to 'Manual' IP
        /// 2. Verify the device has assigned to Manual IP.
        /// 3. Remove the network cable from the network it is connected to and connect it to a second network.
        /// 4. wait for few mins
        /// 5. Remove the network cabe from the second network and connect it back to the first network
        /// 6. Verify the Device behaviour using the ping"	"2.The device should assigned to manual IP.
        /// 3.On reconnecting the cable to a different network the device should still retain the same manual IP which it had previously.
        /// 6.  Device should still retain the manual IP and ping should be success."	Can not validate the IP after it is connected to 2nd n/w 
        /// </summary>
        /// <returns></returns>
        public static bool HoseBreakAcrossNetworksUsingSameManualIP(IPConfigurationActivityData activityData, int testNo)
        {
            IPAddress manualIPAddress = null;
            bool isManualIpSet = false;

            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Change the hostname to a unique value so that the manual ip can be validated in the different subnet

                // TODO : Get alternate validation mechanism from arul
                //string hostName = Path.GetRandomFileName().Replace(".","_");

                //EwsWrapper.Instance().SetHostName(hostName);

                // Setting IP Config method to Manual IP.
                manualIPAddress = Network.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.PrimaryWiredIPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, manualIPAddress);

                // Validate if the printer acquired the manual IP
                if (!(isManualIpSet = CheckForPrinterAvailability(manualIPAddress.ToString())))
                {
                    return false;
                }

                // Validate IP Config method
                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, manualIPAddress.ToString()))
                {
                    return false;
                }

                // Perform Hose break across networks
                if (!Utility.PerformHoseBreakAcrossNetworks(activityData.SwitchIP, activityData.PrimaryInterfacePortNo, activityData.SecondDhcpServerIPAddress))
                {
                    return false;
                }

                // TODO : Get alternate validation mechanism from arul
                // Validate if the printer has the same manual ip
                //string result = ProcessEx.Execute("cmd.exe", "/C ping -a \"{0}\"".FormatWith(manualIPAddress));

                //if (result.Contains(hostName) && result.Contains(manualIPAddress.ToString()))
                //{
                //    TraceFactory.Logger.Info("Printer retained the manual IP Address : {0} in the second network.".FormatWith(manualIPAddress));
                //}
                //else
                //{
                //    TraceFactory.Logger.Info("Printer did not retain the manual IP Address : {0} in the second network.".FormatWith(manualIPAddress));
                //    return false;
                //}

                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (!Utility.PerformHoseBreakAcrossNetworks(activityData.SwitchIP, activityData.PrimaryInterfacePortNo, activityData.PrimaryDhcpServerIPAddress))
                {
                    return false;
                }

                if (CheckForPrinterAvailability(manualIPAddress.ToString()))
                {
                    TraceFactory.Logger.Info("Printer retained with the same manual IP Address {0} in first network.".FormatWith(manualIPAddress));
                }
                else
                {
                    return false;
                }

                // Validate IP Config method
                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, manualIPAddress.ToString()))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                //TODO: Ambily change the device address in the above when it is set to manual
                if (isManualIpSet)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(manualIPAddress);
                    EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                }
                else
                {
                    EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                }

                PostRequisiteHoseBreakAcrossNetworks(activityData);
            }
        }

        #endregion

        #region Power Cycle & Cold Reset

        /// <summary>
        /// 96145	
        /// Template  Verify device behavior on cold reset	
        /// Verify device behavior on cold reset	
        /// "1. Connect the device in a network with both Bootp and DHCP server and cold reset the device.
        /// 2. Cold reset the device with only DHCP Server available.
        /// 3. Cold reset the device with no Bootp and DHCP server available."	
        /// "VEP: 1. On cold reset the device should start sending Bootp packets. If the Bootp server is available then the device should get configured with Bootp IP.
        /// 2. If the bootp server is not available, then the device should send DHCP packets should get configured with DHCP IP.
        /// 3. If both Bootp and DHCP servers are not available then the device should get configured with auto IP and should send DHCP packets in the background.
        /// Sequence to get IP for the Device:
        /// 1. Bootp
        /// 2. DHCP
        /// 3.AutoIP
        /// TPS :  On cold reset printer should send DHCP packets,if DHCP server is available, 
        /// it should get configured with DHCP if not ot it should start sending Bootp packets and if bootp is available it should get configured with Bootp.Ans should send dhcp packets in back ground (As for TPS DHCP is the first precedence ).
        /// When no server in available it should acquire Auto IP with DHCP/Bootp packets in background"	
        /// "In Expected – removed bootp( If both Bootp and DHCP servers are not available then the device should get configured with auto IP and should send bootp and DHCP packets in the background not applicable for VEP . 
        /// Normal sequence to get ip after cold reset is added."
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool VerifyDeviceBehaviorOnColdReset(IPConfigurationActivityData activityData, int testCaseId)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testCaseId.ToString().ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                // Delete Reservation
                serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
                serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Both);

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);

                IPConfigMethod defaultConfigMethod = printer.DefaultIPConfigMethod;

                ColdReset(activityData, printer);

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);

                // TODO: Packet validation

                if (!ValidateIPConfigMethod(activityData, defaultConfigMethod, activityData.PrimaryWiredIPv4Address))
                {
                    return false;
                }

                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                // Delete Reservation
                serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
                serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Dhcp);

                ColdReset(activityData, printer);

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);

                // TODO: Packet validation

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP, activityData.PrimaryWiredIPv4Address))
                {
                    return false;
                }

                // Cold reset when both BOOTP and DHCP servers are not available.
                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                if (!serviceMethod.Channel.StopDhcpServer())
                {
                    return false;
                }

                ColdReset(activityData, printer);

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);
                NetworkUtil.RenewLocalMachineIP();

                return ValidateDefaultIP(activityData);
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                AutoIPPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template ID:96149
        ///Step:Verify manual IP configuration after power cycle and cold reset
        ///step 1 : 1.Set a manual IP address on the device
        ///2.Power cycle the device
        ///Step 2:1. Set a manual IP address on the device
        ///2. Cold reset the device
        ///Expected:1. On power cycle the device should retain the previously configured manual IP address. 
        ///2. On cold reset the device should acquire a new IP address using any IP configuration method. It should not retain the previous manual IP address
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool VerifyManualIPAfterPowerCycleAndColdReset(IPConfigurationActivityData activityData, int testNo)
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

            string currentDeviceAddress = activityData.PrimaryWiredIPv4Address;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                IPAddress manualIPAddress = Network.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(currentDeviceAddress).GetSubnetMask(), IPAddress.Parse(currentDeviceAddress));

                if (EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, manualIPAddress))
                {
                    currentDeviceAddress = manualIPAddress.ToString();
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, manualIPAddress.ToString()))
                {
                    return false;
                }

                // create printer object for Manual IP            
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, manualIPAddress.ToString());
                IPConfigMethod defaultConfigMethod = printer.DefaultIPConfigMethod;

                printer.PowerCycle();

                if (printer.PingUntilTimeout(manualIPAddress, TimeSpan.FromSeconds(20)))
                {
                    TraceFactory.Logger.Info("Ping successful with manual IP Address: {0}. The device retained the Manual IP address after Power cycle.".FormatWith(manualIPAddress.ToString()));
                }
                else
                {
                    TraceFactory.Logger.Info("Ping failed with manual IP Address: {0}. The device failed to retain the Manual IP address after Power cycle.".FormatWith(manualIPAddress.ToString()));
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, manualIPAddress.ToString()))
                {
                    return false;
                }

                ColdReset(activityData, printer);

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);

                // Discover and find the current IP Address of the device as cold reset clears the manual IP Configured on the device.
                currentDeviceAddress = Utility.GetPrinterIPAddress(activityData.PrimaryMacAddress);

                if (string.IsNullOrEmpty(currentDeviceAddress))
                {
                    return false;
                }

                if (currentDeviceAddress.Equals(manualIPAddress))
                {
                    TraceFactory.Logger.Info("Printer retained the manual IP address: {0} after cold reset.".FormatWith(currentDeviceAddress));
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer acquired the new IP Address: {0} after cold reset.".FormatWith(currentDeviceAddress));
                }

                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(currentDeviceAddress), TimeSpan.FromSeconds(30)))
                {
                    TraceFactory.Logger.Info("Ping successful with the new IP address: {0} acquired after cold reset.".FormatWith(currentDeviceAddress));
                }
                else
                {
                    TraceFactory.Logger.Info("Ping failed with the new IP address: {0} acquired after cold reset.".FormatWith(currentDeviceAddress));
                    return false;
                }

                return ValidateIPConfigMethod(activityData, defaultConfigMethod, currentDeviceAddress.ToString());
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                if (!string.IsNullOrEmpty(currentDeviceAddress))
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);
                    EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                }
            }
        }

        /// <summary>
        /// Template ID:96150
        ///Step:Verify bootp configuration after power cycle and cold reset
        ///Step 1; Set and verify that device is bootp configured.
        ///Power cycle the device.
        ///Step2: Set and verify that device is bootp configured.
        ///Cold reset the device.
        ///Expected:1. The device should still be configured with Bootp IP on power cycle
        ///2. The device should send Bootp packets and should get configured with Bootp IP
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool VerifyBootpAfterPowerCycleAndColdReset(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
                IPConfigMethod defaultConfigMethod = printer.DefaultIPConfigMethod;

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.BOOTP))
                {
                    return false;
                }

                printer.PowerCycle();

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.BOOTP))
                {
                    return false;
                }

                ColdReset(activityData, printer);

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);

                return ValidateIPConfigMethod(activityData, defaultConfigMethod);
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
            }
        }

        /// <summary>
        /// 96160	
        /// Template Verify stateless IPv6 address on reboot and coldreset	
        /// Verify stateless IPv6 address on reboot and coldreset	
        /// "Step1:1. Power on the device in a network with a router providing stateless address
        /// 2. Reboot the device
        /// Step2:1. Power on the device in a network with a router providing stateless address
        /// 2. Coldreset the device"	
        /// "1. The device should acquire stateless addresses from router after reboot . 
        /// If the router doesn't provide the stateless addresses after reboot then the device should not retain the previous stateless addresses
        /// 2. The device should acquire stateless addresses from router after coldreset .
        /// If the router doesn't provide the stateless addresses after coldreset then the device should not retain the previous stateless addresses."
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool VerifyStatelessIPv6OnRebootAndColdReset(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            IRouter router = null;
            int routerVlanId = 0;
            Collection<IPAddress> routerAddresses = null;

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string routerAddress = ROUTER_IP_FORMAT.FormatWith(activityData.PrimaryWiredIPv4Address.Substring(0, activityData.PrimaryWiredIPv4Address.LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase)));
                router = RouterFactory.Create(IPAddress.Parse(routerAddress), ROUTER_USERNAME, ROUTER_PASSWORD);

                Dictionary<int, IPAddress> routerVlans = router.GetAvailableVirtualLans();

                routerVlanId = routerVlans.Where(x => (null != x.Value) && (x.Value.IsInSameSubnet(IPAddress.Parse(routerAddress)))).FirstOrDefault().Key;

                RouterVirtualLAN routerVlan = router.GetVirtualLanDetails(routerVlanId);

                routerAddresses = router.GetIPv6Addresses(routerVlan.IPv6Details);

                if (!router.EnableIPv6Address(routerVlanId, routerAddresses))
                {
                    return false;
                }

                EwsWrapper.Instance().SetStatelessAddress(true);

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
                printer.PowerCycle();

                if (!VerifyStatelessAddress())
                {
                    return false;
                }

                ColdReset(activityData, printer);

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);

                if (!VerifyStatelessAddress())
                {
                    return false;
                }

                if (!router.DisableIPv6Address(routerVlanId))
                {
                    return false;
                }

                EwsWrapper.Instance().SetStatelessAddress(true);

                printer.PowerCycle();

                if (VerifyStatelessAddress())
                {
                    return false;
                }

                ColdReset(activityData, printer);

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);

                return !VerifyStatelessAddress();
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetStatelessAddress(true);

                if (null != router)
                {
                    router.EnableIPv6Address(routerVlanId, routerAddresses);
                }
            }
        }

        /// <summary>
        /// 96159
        /// Template Verify manual IPv6 address on reboot and cold reset	
        /// Verify manual IPv6 address on reboot and cold reset	
        /// "Step1:1. Power on the device 
        /// 2. Configure a valid manual IPv6 address on the device
        /// 3. Reboot the device
        /// Step2:       
        /// 1. Power on the device 
        /// 2. Configure a valid manual IPv6 address on the device
        /// 3. Cold reset the device"	
        /// "1. The manual IPv6 address should be retained over reboot
        /// 2. Manual IPv6 address should not be retained after cold reset."
        /// </summary>
        /// <returns></returns>
        public static bool VerifyManualIpv6OnReset(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                string ManualIPv6Address = GetManualIPv6Address(activityData);

                if (!EwsWrapper.Instance().SetManualIPv6Address(true, ManualIPv6Address))
                {
                    return false;
                }

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);

                if (!printer.PingUntilTimeout(IPAddress.Parse(ManualIPv6Address), TimeSpan.FromSeconds(30)))
                {
                    TraceFactory.Logger.Info("Ping failed with manual ipv6 address.");
                    return false;
                }

                if (!EwsWrapper.Instance().ValidateManualIpv6Address(ManualIPv6Address))
                {
                    TraceFactory.Logger.Info("EWS validation failed. Manual IPv6 : {0} is not configured on the printer".FormatWith(ManualIPv6Address));
                    return false;
                }

                if (!SnmpWrapper.Instance().GetManualIpv6Address().EqualsIgnoreCase(ManualIPv6Address))
                {
                    TraceFactory.Logger.Info("SNMP Validation failed. Manual IPv6 : {0} is not configured on the printer".FormatWith(ManualIPv6Address));
                    return false;
                }

                if (!printer.PowerCycle())
                {
                    return false;
                }

                if (!printer.PingUntilTimeout(IPAddress.Parse(ManualIPv6Address), TimeSpan.FromSeconds(30)))
                {
                    TraceFactory.Logger.Info("Ping failed with manual ipv6 address.");
                    return false;
                }

                if (!EwsWrapper.Instance().ValidateManualIpv6Address(ManualIPv6Address))
                {
                    TraceFactory.Logger.Info("EWS validation failed. Manual IPv6 : {0} is not configured on the printer".FormatWith(ManualIPv6Address));
                    return false;
                }

                if (!SnmpWrapper.Instance().GetManualIpv6Address().EqualsIgnoreCase(ManualIPv6Address))
                {
                    TraceFactory.Logger.Info("SNMP Validation failed. Manual IPv6 : {0} is not configured on the printer".FormatWith(ManualIPv6Address));
                    return false;
                }

                if (!ColdReset(activityData, printer))
                {
                    return false;
                }

                if (printer.PingUntilTimeout(IPAddress.Parse(ManualIPv6Address), TimeSpan.FromSeconds(30)))
                {
                    TraceFactory.Logger.Info("Ping is successful with manual ipv6.");
                    return false;
                }

                if (EwsWrapper.Instance().ValidateManualIpv6Address(ManualIPv6Address))
                {
                    TraceFactory.Logger.Info("EWS validation failed. Manual IPv6 : {0} is not cleared from the printer".FormatWith(ManualIPv6Address));
                    return false;
                }

                if (SnmpWrapper.Instance().GetManualIpv6Address().EqualsIgnoreCase(ManualIPv6Address))
                {
                    TraceFactory.Logger.Info("SNMP Validation failed. Manual IPv6 : {0} is not cleared from the printer".FormatWith(ManualIPv6Address));
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetManualIPv6Address(false);
            }
        }

        /// <summary>
        /// 96163	
        /// Template Verify IPv6 and IPv4 status over power cycle and cold reset	
        /// Verify IPv6 and IPv4 status over power cycle and cold reset	
        /// "Step 1:1. Disable IPv4, DHCPv6 and IPv6 on the device.
        /// 2. Power cycle the device                           
        /// Step2:1. Disable IPv4, DHCPv6 and IPv6 on the device
        /// 2. Cold reset the device"
        /// "1. IPv4, DHCPv6, and IPv6 should still be disabled over power cycle
        /// 2. IPv4, DHCPv6, and IPv6 should be enabled on cold reset
        /// Note: For VEP, IPv4 disable is not possible, so only with IPv6 disable power cycle or cold reset is valid"
        /// </summary>
        /// <returns></returns>
        public static bool VerifyDhcpv6Ipv6v4StatusOnReset(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    if (!VerifyIPv4OnReset(activityData))
                    {
                        return false;
                    }

                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
                }

                return VerifyDhcpv6IPv6OnReset(activityData);
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address);
            }
        }

        /// <summary>
        /// 96144	
        /// Template Power cycle and cold reset the device when DHCPv4-v6 server has dynamic scope	
        /// Power cycle and cold reset the device when DHCPv4/v6 server has dynamic scope	
        /// "Step1: 1. Connect the device in a network with DHCPv4/v6 server with dynamic scope
        /// 2. Power cycle the device
        /// 3. Repeat step2 once again 
        /// Step2: 1. Connect the device in a network with DHCPv4/v6 server with dynamic scope
        /// 2. Cold-reset the device
        /// 3. Repeat step2 once again"	
        /// 1. When a dynamic range of IP addresses is used, the devices should obtain an IP address as long as there are some addresses left in the DHCP server pool.
        /// </summary>
        /// <returns></returns>
        public static bool VerifyDynamicScopeOnReset(IPConfigurationActivityData activityData, int testNo)
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

            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
            string invalidMacAddress = string.Empty;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Make sure that the printer acquires a DHCPv6 address
                EwsWrapper.Instance().SetDHCPv6OnStartup(true);

                if (!PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily))
                {
                    // Enable disable the ipv6 option so that DHCPv6 address is available on the printer.
                    EwsWrapper.Instance().SetIPv6(false);
                    EwsWrapper.Instance().SetIPv6(true);
                }

                // TODO : adding the template only for dynamic scope of DHCPv4 Server. Extend it to DHCP v6 as well.

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                // Delete Reservation
                if (!serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress))
                {
                    TraceFactory.Logger.Info("Failed to delete reservation from DHCP Server");
                    return false;
                }

                // Make sure that the IPv4 Address is not assigned for any other device. A reservation is created for the actual printer IP Address with a dummy MAC Address.
                invalidMacAddress = activityData.PrimaryMacAddress.Remove(1, 2);
                serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, invalidMacAddress, ReservationType.Both);

                printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
                bool isLfp = Printer.PrinterFamilies.LFP.ToString().EqualsIgnoreCase(activityData.ProductFamily) ? false : true;

                // Power Cycle
                for (int i = 1; i <= 2; i++)
                {
                    printer.PowerCycle();

                    if (!VerifyDynamicIPv4IPv6Address(activityData, ref printer))
                    {
                        return false;
                    }
                }

                // Cold Reset
                for (int i = 1; i <= 2; i++)
                {
                    ColdReset(activityData, printer);

                    // After cold reset enabling WSDiscovery option
                    EwsWrapper.Instance().SetWSDiscovery(true);

                    // IPv6 validation is not applicable to LFP, For VEP: Cold reset won't effect IPv6 status
                    if (!VerifyDynamicIPv4IPv6Address(activityData, ref printer, isLfp))
                    {
                        return false;
                    }
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                // Create reservation for the printer for both DHCP and BOOTP.
                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, invalidMacAddress);

                if (!serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Both))
                {
                    TraceFactory.Logger.Info("Reservation for the MAC Address {0} for IP Address {1} for both BOOTP and DHCP failed.".FormatWith(activityData.PrimaryMacAddress, activityData.PrimaryWiredIPv4Address));
                }

                // After creating reservation power cycle the printer so that it acquires the reserved IP Address.
                printer.PowerCycle();

                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.PrimaryWiredIPv4Address), TimeSpan.FromSeconds(30)))
                {
                    TraceFactory.Logger.Info("Printer acquired the IP address {0}.".FormatWith(activityData.PrimaryWiredIPv4Address));
                }
                else
                {
                    TraceFactory.Logger.Info("Printer failed to acquire the IP address {0}.".FormatWith(activityData.PrimaryWiredIPv4Address));
                }

                EwsWrapper.Instance().SetDHCPv6StatelessConfigurationOption();
            }
        }

        #endregion

        #region Net Stack Reset

        /// <summary>
        /// Template ID:96161
        ///Step:Verify device behavior over host name and domain name changes
        ///Step1:1. Power on the device in a network
        ///2. Change either host name or domain name on the device
        ///Expected:1. When either the host name or the domain name is changed the device should do a net stack reset event. 
        ///2. Device should start IPv4 and IPv6 process and should acquire a valid IP.
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool NetStackResetOnHostNameDomainNameChange(IPConfigurationActivityData activityData, int testNo)
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

            string hostName = "Default_HostName";
            string domainName = "Default_DomainName";

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // TODO : Check why Config precedence is set here?
                TraceFactory.Logger.Info("Setting the config Precedence table with Manual/BOOTP/DHCPv4/DHCPv6/TFTP/Default as precedence configuration method");
                SnmpWrapper.Instance().SetConfigPrecedence("0:2:3:1:4");
                Thread.Sleep(TimeSpan.FromMinutes(1));

                //Packet validation is commented.

                //string packetID = RequestPacketData(0, activityData, testCaseId.ToString(), activityData.SessionId);

                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

                // TODO : Packet Validation

                if (!EwsWrapper.Instance().SetDomainName(domainName))
                {
                    return false;
                }

                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.PrimaryWiredIPv4Address), TimeSpan.FromSeconds(20)))
                {
                    TraceFactory.Logger.Info("Ping is successful with IP Address:{0}".FormatWith(activityData.PrimaryWiredIPv4Address));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Ping failed with IP Address:{0}".FormatWith(activityData.PrimaryWiredIPv4Address));
                    return false;
                }
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                EwsWrapper.Instance().ResetConfigPrecedence();

                ColdReset(activityData, PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address));

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);
            }
        }

        #endregion

        #region IPv4/V6 Enable Disable

        /// <summary>
        /// Template ID:96162
        ///Step1:Verify device when IPv4/IPv6 is enabled or disabled
        ///Step1:1. Power on the device in a network
        ///2. Enable or disable IPv4 and IPv6
        ///Expected:Enabling or disabling IPv4 or IPv6 initiates a net stack reset event on the device                                                
        ///Step2:IPv6 Disabled or enabled  For VEP Only
        ///Step1:1. Power on the device in a network
        ///2. Enable or disable  IPv6
        ///Expected:Enabling IPv6 address should get. Disabling including link local add should not displayed
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool VerifyIPv4IPv6StatusChange(IPConfigurationActivityData activityData, int testNo)
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

            IPAddress linkLocalAddress = null;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // IPV4 enable/ disable
                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
                    linkLocalAddress = printer.IPv6LinkLocalAddress;

                    EwsWrapper.Instance().SetIPv4(false, printer);

                    if (CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address))
                    {
                        TraceFactory.Logger.Info("Failed to disable IPv4 option on the printer.");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Successfully disabled IPv4 option on the printer.");
                    }

                    if (!CheckForPrinterAvailability(linkLocalAddress.ToString(), TimeSpan.FromMinutes(1)))
                    {
                        return false;
                    }

                    // TODO: Validate net stack reset
                    EwsWrapper.Instance().SetIPv4(true, printerIpv4Address: activityData.PrimaryWiredIPv4Address);

                    if (!CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address, TimeSpan.FromMinutes(1)))
                    {
                        TraceFactory.Logger.Info("Failed to enable IPv4 option on the printer.");
                        return false;
                    }

                    //TODO: Validate net stack reset

                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
                }

                EwsWrapper.Instance().SetDHCPv6OnStartup(true);

                // Disable IPv6 on the device
                if (!EwsWrapper.Instance().SetIPv6(false))
                {
                    return false;
                }

                Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(0);

                IPAddress statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
                IPAddress linkLocalIPv6Address = IPv6Utils.GetLinkLocalAddress(ipv6Details);
                Collection<IPAddress> statelessAddresses = IPv6Utils.GetStatelessIPAddress(ipv6Details);

                if (IPAddress.IPv6None.Equals(statefulAddress))
                {
                    TraceFactory.Logger.Info("EWS Validation is successful. State full IPv6 Address is disabled on the printer.");
                }
                else
                {
                    TraceFactory.Logger.Info("EWS Validation failed. State full IPv6 Address is not disabled on the printer.");
                    return false;
                }

                if (IPAddress.IPv6None.Equals(linkLocalIPv6Address))
                {
                    TraceFactory.Logger.Info("EWS Validation is successful. Link Local IPv6 Address is disabled on the printer.");
                }
                else
                {
                    TraceFactory.Logger.Info("EWS Validation failed. Link Local IPv6 Address is not disabled on the printer.");
                    return false;
                }

                if (statelessAddresses.Count == 0)
                {
                    TraceFactory.Logger.Info("EWS Validation is successful. Stateless IPv6 Addresses are disabled on the printer.");
                }
                else
                {
                    TraceFactory.Logger.Info("EWS Validation failed. Stateless IPv6 Addresses are not disabled on the printer.");
                    return false;
                }

                if (!EwsWrapper.Instance().SetIPv6(true))
                {
                    return false;
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));

                int ipv6Count = PrinterFamilies.TPS.ToString() == activityData.ProductFamily ? 3 : 4;

                ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(ipv6Count + 2);
                statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
                linkLocalIPv6Address = IPv6Utils.GetLinkLocalAddress(ipv6Details);
                statelessAddresses = IPv6Utils.GetStatelessIPAddress(ipv6Details);

                if (!IPAddress.IPv6None.Equals(statefulAddress))
                {
                    TraceFactory.Logger.Info("EWS Validation is successful. State full IPv6 Address is available on the printer.");
                }
                else
                {
                    TraceFactory.Logger.Info("EWS Validation failed. State full IPv6 Address is not available on the printer.");
                    return false;
                }

                if (!IPAddress.IPv6None.Equals(linkLocalIPv6Address))
                {
                    TraceFactory.Logger.Info("EWS Validation is successful. Link Local IPv6 Address is available on the printer.");
                }
                else
                {
                    TraceFactory.Logger.Info("EWS Validation failed. Link Local IPv6 Address is not available on the printer.");
                    return false;
                }

                if (statelessAddresses.Count == 0)
                {
                    TraceFactory.Logger.Info("EWS Validation is failed. Stateless IPv6 Addresses are not available on the printer.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("EWS Validation is successful. Stateless IPv6 Addresses are available on the printer.");
                    return true;
                }
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) && !CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address))
                {
                    CheckForPrinterAvailability(linkLocalAddress.ToString());
                    EwsWrapper.Instance().ChangeDeviceAddress(linkLocalAddress.ToString());
                }

                EwsWrapper.Instance().SetDHCPv6StatelessConfigurationOption();
                EwsWrapper.Instance().SetIPv6(true);

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
            }
        }

        #endregion

        #region Lease Time

        /// <summary>
        /// Template ID:96164
        ///Step:Verify that Device can get the proper lease time
        ///1. Connect device in a network with a DHCP4/v6 server. 
        ///2. Configure finite lease on the server 
        ///3. Vary the lease time on the server
        ///Expected:The device should get updated with the lease provided by the server for both DHCPv4 and DHCPv6 addresses. 
        ///The address should be renewed at half the lease time
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool VerifyDeviceGetsProperLeaseTime(IPConfigurationActivityData activityData, int testCaseId)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testCaseId.ToString().ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TimeSpan DHCPv4LeaseTime = TimeSpan.FromMinutes(3);
                TimeSpan preferredLifeTime = TimeSpan.FromMinutes(3);
                TimeSpan validLifeTime = TimeSpan.FromMinutes(4);

                if (!EwsWrapper.Instance().SetDHCPv6OnStartup(true))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetIPv6(true))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetStatelessAddress(true))
                {
                    return false;
                }

                // Set the IP Config method of the printer to DHCP, Since lease time is available only with DHCP
                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                for (int i = 0; i < 2; i++)
                {
                    // Add i = 0, 1 minutes to all the lease times so as to execute the steps below twice.
                    DHCPv4LeaseTime = DHCPv4LeaseTime.Add(TimeSpan.FromMinutes(i));
                    preferredLifeTime = preferredLifeTime.Add(TimeSpan.FromMinutes(i));
                    validLifeTime = validLifeTime.Add(TimeSpan.FromMinutes(i));

                    DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                    if (!serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, (int)DHCPv4LeaseTime.TotalSeconds))
                    {
                        return false;
                    }

                    if (!serviceMethod.Channel.SetPreferredLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, (int)preferredLifeTime.TotalSeconds))
                    {
                        return false;
                    }

                    if (!serviceMethod.Channel.SetValidLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, (int)validLifeTime.TotalSeconds))
                    {
                        return false;
                    }

                    //TODO : Packet validation
                    //TraceFactory.Logger.Info("Validating the address renewed at half the lease time by capturing the Packets");

                    //string packetID = RequestPacketData(3, activityData, testCaseID, activityData.SessionId);
                    //if (ValidatePacketType(3, activityData, packetID, testCaseID, activityData.SessionId).Equals("True"))
                    //{
                    //	TraceFactory.Logger.Info("Successfully validated the address renewed at half the lease time by capturing the Packets");
                    //}
                    //else
                    //{
                    //	TraceFactory.Logger.Info("Failed to validate the address renewed at half the lease time by capturing the Packets");
                    //	return false;
                    //}

                    // If the printer is already in DHCP, lease time won't be updated. So changing the config method to BOOTP and back to DHCP.
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                    // Validate IPv4 Lease time
                    if (!ValidateLeaseTime(activityData.ProductFamily, (int)DHCPv4LeaseTime.TotalSeconds))
                    {
                        TraceFactory.Logger.Info("Printer didn't acquire lease time: {0} seconds.".FormatWith(DHCPv4LeaseTime.TotalSeconds));
                        return false;
                    }

                    int ipv6AddressCount = activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) ? 4 : 5;

                    Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(ipv6AddressCount);

                    if (!CompareIPv6LifeTime(ipv6Details, preferredLifeTime, true))
                    {
                        return false;
                    }

                    if (!CompareIPv6LifeTime(ipv6Details, validLifeTime, false))
                    {
                        return false;
                    }

                    if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.PrimaryWiredIPv4Address), TimeSpan.FromSeconds(20)))
                    {
                        TraceFactory.Logger.Info("Ping succeeded with Printer IP Address:{0}".FormatWith(activityData.PrimaryWiredIPv4Address));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Ping failed with Printer IP Address:{0}".FormatWith(activityData.PrimaryWiredIPv4Address));
                        return false;
                    }
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, DEFAULT_LEASE_TIME);
                serviceMethod.Channel.SetValidLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, DEFAULT_VALID_LEASE_TIME);
                serviceMethod.Channel.SetPreferredLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, DEFAULT_PREFERRED_LEASE_TIME);

                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
            }
        }

        /// <summary>
        /// 96165	
        /// Verify device lease renewal behavior
        /// Template Test handling infinite lease, no renewal sent.	Test handling infinite lease, no renewal sent	
        /// "1.Connect the device in a network with a DHCP server providing infinite lease time.
        /// 2. Wait for 10 minutes and change the lease time to 2 minutes in the DHCP server.
        /// 3.Power cycle the printer. "	
        /// Expected:-
        /// "1. The device should get updated with the infinite lease and the device should not send a renew packet to renew the lease.
        /// 2. On changing the lease time to two minutes, the printer will still retain the previous IP and will not send any request for renewal of the IP. 
        /// The printer's network summary page will not show the lease time as two minutes.
        /// 3.  After power cycling, the printer should acquire a proper DHCP IP and should be configured with the correct lease time  as 2 mins in the network summary page."	
        /// added extra steps whether device is  configure with finite lease after power cycle. When the device was in infinite lease time.
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool VerifyLeaseRenewalBehavior(IPConfigurationActivityData activityData, int testCaseId)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testCaseId.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Set the IP config method of the printer to DHCP, Since lease time is available only with DHCP
                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                if (!serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, INFINITE_LEASE_TIME))
                {
                    return false;
                }

                // Config method is changed to update the lease time on printer
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                if (ValidateLeaseTime(activityData.ProductFamily, -1))
                {
                    TraceFactory.Logger.Info("Printer acquired Infinite lease time.");
                }
                else
                {
                    TraceFactory.Logger.Info("Printer failed to acquire Infinite lease time.");
                    return false;
                }

                // TODO : Packet Validation

                TraceFactory.Logger.Info("Waiting for 600 seconds.");
                Thread.Sleep(TimeSpan.FromMinutes(10));

                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                TimeSpan leaseTime = TimeSpan.FromMinutes(2);

                if (!serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, (int)leaseTime.TotalSeconds))
                {
                    return false;
                }

                // TODO : Packet Validation

                // Check for printer availability
                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.PrimaryWiredIPv4Address), TimeSpan.FromSeconds(20)))
                {
                    TraceFactory.Logger.Info("Ping succeeded with Printer IP Address:{0}".FormatWith(activityData.PrimaryWiredIPv4Address));
                }
                else
                {
                    TraceFactory.Logger.Info("Ping failed with Printer IP Address:{0}".FormatWith(activityData.PrimaryWiredIPv4Address));
                    return false;
                }

                // Checking if infinite lease is set
                if (!ValidateLeaseTime(activityData.ProductFamily, -1))
                {
                    TraceFactory.Logger.Info("Expected: Printer should not acquire lease time of {0} seconds. Actual: Printer acquired lease time of {0} seconds.".FormatWith(leaseTime.TotalSeconds));
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer didn't acquire lease time of {0} seconds as expected.".FormatWith(leaseTime.TotalSeconds));
                }

                // TODO : Validate the lease time from network Summary page. Currently No id is available for the table which displays the Lease time.

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);

                // Not validating power cycle as there are chances that the printer acquires a new IP Address after power cycle since the lease time is 2 minutes.
                printer.PowerCycle();

                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.PrimaryWiredIPv4Address), TimeSpan.FromSeconds(10)))
                {
                    TraceFactory.Logger.Info("Ping succeeded with Printer IP Address:{0}".FormatWith(activityData.PrimaryWiredIPv4Address));
                }
                else
                {
                    TraceFactory.Logger.Info("Ping failed with Printer IP Address:{0}".FormatWith(activityData.PrimaryWiredIPv4Address));
                    return false;
                }

                if (ValidateLeaseTime(activityData.ProductFamily, (int)leaseTime.TotalSeconds))
                {
                    TraceFactory.Logger.Info("Printer acquired lease time of {0} seconds after power cycle.".FormatWith(leaseTime.TotalSeconds));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer failed to acquire lease time of {0} seconds after power cycle.".FormatWith(leaseTime.TotalSeconds));
                    return false;
                }
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, DEFAULT_LEASE_TIME);
                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
            }
        }

        #endregion

        #region Config Precedence

        /// <summary>
        /// 96147	
        /// Template Check - Reinitialize Now behavior	
        /// STEPI. Check for device IP address after “Reinitialize Now” using DHCP IP	
        /// "1. Connect the device in a network with DHCP server.
        /// 2. Confirm that the device has acquired an IP address from the DHCP server.
        /// 3. Click""Reinitialize Now"" in config precedence tab
        /// Note : Only for VEP."	
        /// Expected : "1. Device should start the DHCP process (of sending Discover, Offer,Request and Acknowledge) and acquires same DHCPv4 address which it had before, if the same IP is available or should acquire a new DHCPv4 address."	
        /// "Expected result modified with DHCP process"
        /// STEPII. Check - Reinitialize Now behavior- using BootIP	
        /// "1. Connect the device in a network with BootP server.
        /// 2. Confirm that the device has acquired a BootP  address from the BootP server.
        /// 3. Click""Reinitialize Now"" in config precedence tab
        /// Note : Only for VEP."	
        /// Expected: 1. Device should start the BootP process (of sending Bootp Request and Boot Reply packet) and acquire the same Bootp IPv4 address which it had before , if the same Bootp IP is available.	
        /// The steps was added fully newly for Boot IP process. 
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool VerifyReinializeNowBehavior(IPConfigurationActivityData activityData, string packetFileName, bool performPrerequisites = true)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (performPrerequisites && !TestPreRequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, packetFileName);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!VerifyReinializeNow(activityData, IPConfigMethod.DHCP, packetFileName: packetFileName))
                {
                    return false;
                }

				return VerifyReinializeNow(activityData, IPConfigMethod.BOOTP, packetFileName: packetFileName);
            }
            finally
            {
                client.Channel.Stop(guid);

				if (performPrerequisites)
				{
					TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
					EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
				}
            }
        }

        /// <summary>
        /// 96148	
        /// Template Check Clear previous values and Reinitialize Now behavior	
        /// Template Check Clear previous values and Reinitialize Now behavior	Check for device IP address over “Clear previous values and Reinitialize now”
        /// "1. Connect the device in a network with DHCP server.
        /// 2. Confirm that the device has acquired an IP address from the DHCP server.
        /// 3. Click ""Clear previous values and Reinitialize now "" button in config precedence tab and capture the packets and check
        /// Note : only for VEP"	
        /// Expected: 
        /// "1. The device should try to acquire a BootP IP and  should not send Option 50= “Requested IP address” in the the request packet. If BootP server is available.
        /// If BootP Server is not available, then Device should acquire a DHCP IP.
        /// If Both the BootP or DHCP server is not available, Device should go to AutoIP.
        /// Note:
        /// 1.: It has to follow  the Config method sequence   -ie first try for BootP,  If it fails,s then try for DHCP  IP and if it doesn't find a DHCP server,  then it should acquire  AutoIP.
        /// Note: Clear previous value button should grey out when DUT is configured with Manual IP."	
        /// Modified Expected result as per the device behavior depends on the availability of the server.
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool VerifyClearPreviousValuesAndReinitializeNow(IPConfigurationActivityData activityData, int testNo)
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

            string invalidMacAddress = string.Empty;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info("Case 1: Validate Clear previous values and Reinitialize Now behavior when only DHCP server is available.");

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                // Create a reservation for an invalid mac address so that the printer can get the wiredipv4 address later.
                invalidMacAddress = activityData.PrimaryMacAddress.Remove(1, 2);
                serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
                serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, invalidMacAddress, ReservationType.Both);

                if (!serviceMethod.Channel.ChangeServerType(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, ReservationType.Dhcp))
                {
                    return false;
                }

                if (!ClearPreviousValuesAndReinitializeNow(activityData, IPConfigMethod.DHCP))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Case 2: Validate Clear previous values and Reinitialize Now behavior when only BOOTP server is available.");

                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, invalidMacAddress);
                serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Both);

                if (!ClearPreviousValuesAndReinitializeNow(activityData, IPConfigMethod.BOOTP))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Case 3: Validate Clear previous values and Reinitialize Now behavior when both DHCP and BOOTP servers are not available.");
                return ClearPreviousValuesAndReinitializeNow(activityData, IPConfigMethod.AUTOIP);
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                serviceMethod.Channel.StartDhcpServer();
                serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, invalidMacAddress);

                AutoIPPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template Id: 77410
        /// Step 1:
        /// 1. Bring up the device with DHCP IP and hostname configured using manually.
        /// 2. Configure a small lease of approximately 1 minute on the DHCPv4 server
        /// 3. Configure a hostname using DHCPv4
        /// 4. Configure precedence table with DHCPv4/BootP as highest precedence.
        /// 5. Click the Reinitialize button the config precedence Tab.
        /// 6. Verify whether the hostname configured on the DHCPv4 server is configured on Device.	
        /// Step 2:
        /// 1.After executing step 1 try to edit the option provided by server manually		
        /// Step 3:
        /// 1.After executing step 1 try to edit the option provided by server using telnet
        /// Step 4:
        /// 1.After executing step 1 try to edit the option provided by server using SNMP
        /// Expected:
        /// Step 1: Host name should be configured from the  DHCPv4 server to the device when dhcpv4 as highest precedence.
        /// Step 2: Manual entries should not be accepted
        /// Step 3: Parameters should be shown as read-only and should not be able to edit the options
        /// Step 4: SNMP entries should not be accepted
        /// Note: BOOTP can also be used for this test. It has been sparse out due to the limited number of BOOTP configurable parameters. 
        /// System test should consider testing precedence equality of BOOTP and DHCPv4.
        /// </summary>
        public static bool VerifyDHCPBootpHighestPrecedence(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info("Executing with DHCP config method.");

                if (!ValidateDhcpBootPPrecedence(IPConfigMethod.DHCP, activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Executing with BootP config method.");
                return ValidateDhcpBootPPrecedence(IPConfigMethod.BOOTP, activityData);
            }
            finally
            {
                client.Channel.Stop(guid);
                RestoreServerParameter(activityData);
            }
        }

        /// <summary>
        /// Template Id: 77415
        /// 1. Configure precedence table with BOOTP/DHCPv4 as highest precedence configuration method
        /// 2. Configure a small lease of approximately 1 minute on the DHCPv4 server
        /// 3. Configure a hostname, Domain name, DNS server IP and WINS server IP using DHCPv4
        /// 4. Verify the Device behavior 
        /// 5. Remove hostname, Domain name , DNS server IP and WINS server IP on the DHCPv4 server
        /// 6. Verify whether the new hostname configured manually via  telnet
        /// Expected:
        /// 4. Hostname and other parameters are configured via DHCP server
        /// 6. Hostname and other parameters are configured via manually also using telnet.
        /// </summary>				
        public static bool VerifyParameterConfigurableWithTelnet(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                return VerifyConfiguredParameter(activityData, PrinterAccessType.Telnet);
            }
            finally
            {
                client.Channel.Stop(guid);
            }
        }

        /// <summary>
        /// Template Id: 77416
        /// 1. Configure precedence table with BOOTP/DHCPv4 as highest precedence configuration method
        /// 2. Configure a small lease of approximately 1 minute on the DHCPv4 server
        /// 3. Configure a hostname, Domain name, DNS server IP and WINS server IP using DHCPv4
        /// 4. Verify the Device behavior 
        /// 5. Remove hostname, Domain name , DNS server IP and WINS server IPon the DHCPv4 server
        /// 6. Verify whether the new hostname configured manually via  WEBUI
        /// Expected:
        /// 4. Hostname and other parameters are configured via DHCP server
        /// 6. Hostname and other parameters are configured via manually also using WEBUI.
        /// </summary>				
        public static bool VerifyParameterConfigurableWithWebUI(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                return VerifyConfiguredParameter(activityData, PrinterAccessType.EWS);
            }
            finally
            {
                client.Channel.Stop(guid);
            }
        }

        /// <summary>
        /// Template Id: 77417
        /// 1. Configure precedence table with BOOTP/DHCPv4 as highest precedence configuration method
        /// 2. Configure a small lease of approximately 1 minute on the DHCPv4 server
        /// 3. Configure a hostname, Domain name, DNS server IP and WINS server IP using DHCPv4
        /// 4. Verify the Device behavior 
        /// 5. Remove hostname, Domain name , DNS server IP and WINS server IP on the DHCPv4 server
        /// 6. Verify whether the new hostname configured manually via  SNMP
        /// Expected:
        /// 4. Hostname and other parameters are configured via DHCP server
        /// 6. Hostname and other parameters are configured via manually also using SNMP
        /// </summary>				
        public static bool VerifyParameterConfigurableWithSNMP(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                return VerifyConfiguredParameter(activityData, PrinterAccessType.SNMP);
            }
            finally
            {
                client.Channel.Stop(guid);
            }
        }

        /// <summary>
        /// Template Id: 77419
        /// Configure precedence table with BOOTP/DHCPv4 as highest precedence configuration method
        /// Configure a small lease of approximately 1 minute on the DHCPv4 server
        /// Configure a hostname using DHCPv4
        /// Change hostname on the DHCPv4 server
        /// Verify whether the new hostname configured on the DHCPv4 server is configured on JD after lease renew
        /// Expected:
        /// New host name configured on the DHCPv4 server should be configured on JD
        /// Note: BOOTP can also be used for this test. It has been sparse out due to the limited number of BOOTP configurable parameters.
        /// System test should consider testing precedence equality of BOOTP and DHCPv4.
        /// </summary>
        public static bool VerifyParameterAfterDhcpRenewal(IPConfigurationActivityData activityData, int testNo)
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

            DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!VerifyNewHostnameConfiguration(IPConfigMethod.DHCP, activityData))
                {
                    return false;
                }

                // Setting to default state
                serviceMethod.Channel.SetHostName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.ServerHostName);
                serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, DEFAULT_LEASE_TIME);

                return VerifyNewHostnameConfiguration(IPConfigMethod.BOOTP, activityData);
            }
            finally
            {
                client.Channel.Stop(guid);
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address), validate: false);
                if (PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily))
                {
                    serviceMethod.Channel.DeleteBootPHostname(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);
                }
                RestoreServerParameter(activityData);
                serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, DEFAULT_LEASE_TIME);
            }
        }

        /// <summary>
        /// Template Id: 77421
        /// 1. Access the printer using EWS/SNMP and change the config precedence order.
        /// 2. Cold reset the printer.
        /// 3. Manually configure the host name on the printer and check the full sequence and verify the behavior using EWS and SNMP
        /// Expected:
        ///	Config precedence should reset to default settings after cold reset. 
        ///	It should set Manual as highest precedence and printer should get configured with manual host name and config precedence will updated as per the default sequence in both the EWS and SNMP. 
        /// </summary>
        public static bool VerifyParametersAfterColdReset(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Setting TFTP as highest config precedence.");
                SnmpWrapper.Instance().SetConfigPrecedence("1:0:2:3:4");

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
                ColdReset(activityData, printer);

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);

                string manualHostName = "Manual_HostName";

                EwsWrapper.Instance().SetHostname(manualHostName);

                if (!ValidateManualHighestPrecedence())
                {
                    return false;
                }

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                return ValidateHostName(family, manualHostName);
            }
            finally
            {
                client.Channel.Stop(guid);
                RestoreServerParameter(activityData);
            }
        }

        /// <summary>
        /// Template Id: 77422
        /// 1. Access the printer using WEB UI.
        /// 2. Change the configure precedence to any order.
        /// 3. Perform restore default under the same page.
        /// 4. verify the behavior using SNMP and EWS page
        ///	Expected:
        /// Config precedence should reset to default settings with Manual as highest precedence and printer should get configured accordingly on the both SNMP and EWS.
        /// </summary>
        public static bool VerifyDefaultPrecedenceWithResetOption(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Setting TFTP as highest config precedence.");
                SnmpWrapper.Instance().SetConfigPrecedence("1:0:2:3:4");
                EwsWrapper.Instance().ResetConfigPrecedence();

                return ValidateManualHighestPrecedence();
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().ResetConfigPrecedence();
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        /// <summary>
        /// Template Id: 91829
        /// Step 1:
        /// 1. Configure a DHCPv4 server to supply IP parameters as Hostname, Domain Name and DNS server IP address in stateless configuration mode.
        /// 2. Bring up the device with DHCPv4 IP and set DHCPv4/BootP as highest config precedence.
        /// 2. Enable the  stateless DHCPv4 option  
        /// 3. Change the config method from DHCP to Manual
        /// 4. Check the behavior of options provided by the server.
        /// 5. Change the parameters like Hostname, domain name and DNS server IP address from the DHCP scope
        /// 6. Now disable the stateless DHCPv4 checkbox and check the behavior of the options provided by the server
        /// 7. Change the config method from Manual to Manual
        /// 8. Verify the device behavior
        /// Step 2:		
        /// 1. Configure a BootP/DHCP server to supply IP parameters as Hostname, Domain Name and DNS server IP address in stateless configuration mode.
        /// 2. Bring up the device with BootP IP and set DHCPv4/BootP as highest config precedence.
        /// 2. Enable the  stateless DHCPv4 option  
        /// 3. Change the config method from BootP to Manual
        /// 4. Check the behavior of options provided by the server.
        /// 5. Change the parameters like Hostname, domain name and DNS server IP address from the BootP scope
        /// 6. Now disable the stateless DHCPv4 checkbox and check the behavior of the options provided by the server
        /// 7. Change the config method from Manual to Manual
        /// 8. Verify the device behavior
        /// Expected:
        /// Step 1:
        /// 4.On enabling the DHCPv4 stateless checkbox,the configured IP parameters should be attained by the device  from the DHCPv4 server using DHCP Inform Packet.
        /// 8.On disabling the DHCPv4 stateless checkbox,the device should not attain any IP parameters from the server. DHCP Inform packet should not send while changing manual to manual
        /// Step 2:
        /// 4.On enabling the DHCPv4 stateless checkbox,the configured IP parameters should be attained by the device  from the BootP/DHCPv4 server using DHCP Inform Packet.
        /// 8.On disabling the DHCPv4 stateless checkbox,the device should not attain any IP parameters from the server. DHCP Inform packet should not send while changing manual to manual
        /// </summary>
        public static bool VerifyParameterWithStatelessOption(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!ValidateParameterWithStatelessOption(activityData, IPConfigMethod.DHCP, testNo))
                {
                    return false;
                }

                // Clearing and setting default values
                RestoreServerParameter(activityData, false);

                return ValidateParameterWithStatelessOption(activityData, IPConfigMethod.BOOTP, testNo);
            }
            finally
            {
                client.Channel.Stop(guid);
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address), validate: false);
                RestoreServerParameter(activityData);
            }
        }

        /// <summary>
        /// Template Id: 91831
        /// 1. Bring up the Device with DCHP/BootP and without providing any manually configured parameters.
        /// 2. Configure precedence table on the Device with  Manual/TFTP/DHCPv4Bootp/DHCPv6/Default.
        /// 3.Do not provide host name, Domain name and DNS server IP and WINS server IP on the DHCP server or manually
        /// 4.Power cycle the device to check the behavior.
        /// 4.Verify that all the device parameters such as Hostname,DNS server, WINS server, etc are attained as per the default  precedence 
        /// Expected:
        /// 1.All the device parameters are to be configured based on the precedence option selected.
        /// If the option is not provided by the server / manual , then the precedence option 'Default' will  be automatically selected and
        /// configured with Default Hostname, Domain name, DNS server IP address and WINS server IP address should be blank in the Network Identification Tab.
        /// </summary>
        public static bool VerifyParameterWithDefaultValue(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Setting Manual as highest config precedence.");
                SnmpWrapper.Instance().SetConfigPrecedence("0:1:2:3:4");

                DeleteServerParameter(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
                printer.PowerCycle();

                return ValidateDefaultParameter();
            }
            finally
            {
                client.Channel.Stop(guid);
                RestoreServerParameter(activityData);
            }
        }

        /// <summary>
        /// Template Id: 659573
        /// 1. Access the printer using EWS and change the config precedence order by trying to move the default option.
        /// 2. Verify the behavior using EWS.  
        ///	Expected:
        /// On trying to change the config precedence order for default ,it shouldn't allow the user to change the position of default.
        /// </summary>
        public static bool VerifyPrecedenceOrderForDefault(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Setting Default as highest config precedence.");
                SnmpWrapper.Instance().SetConfigPrecedence("4:0:1:2:3");

                TraceFactory.Logger.Info("Validating whether Default was set as highest precedence");

                return ValidateManualHighestPrecedence();
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().ResetConfigPrecedence();
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        /// <summary>
        /// Template Id: 681951
        /// 1. Bring up the Device with DHCP/BOOTP IP
        /// 2. Check the config precedence sequence
        /// 3. Discover the Device using SNMP
        /// 4. Go to -->hpPrintServer--> nm--> interface -->npCard --> npCfg --> npcfgIPConfigPrecendence  and change the value to 3:2:1:0:4 using set option.
        /// 5. verify the device behavior in EWS page and as well in MIB Browser  
        /// Expected:
        /// 5. Device should be come up with config precedence with the following sequence:
        /// 1. DHCPv6
        /// 2. DHCPv4/BootP
        /// 3. TFTP
        /// 4. Manual
        /// 5. Default and in SNMP using walk should show as 3:2:1:0:4
        /// </summary>
        public static bool VerifyConfigPrecedenceUsingSnmp(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string configPrecedence = "3:2:1:0:4";

                TraceFactory.Logger.Info("Setting DHCPv6 as highest config precedence.");
                SnmpWrapper.Instance().SetConfigPrecedence(configPrecedence);

                if (configPrecedence.EqualsIgnoreCase(SnmpWrapper.Instance().GetConfigPrecedence()))
                {
                    TraceFactory.Logger.Info("Config Precedence is set to: {0} order.".FormatWith(configPrecedence));
                }
                else
                {
                    TraceFactory.Logger.Info("Config Precedence is not set to: {0} order.".FormatWith(configPrecedence));
                    return false;
                }

                configPrecedence = "DHCPv6:DHCPv4/BOOTP:TFTP:Manual:Default";

                if (EwsWrapper.Instance().GetConfigPrecedence(false).Trim().EqualsIgnoreCase(configPrecedence))
                {
                    TraceFactory.Logger.Info("Config Precedence is set to: {0} order.".FormatWith(configPrecedence));
                }
                else
                {
                    TraceFactory.Logger.Info("Config Precedence is not set to: {0} order.".FormatWith(configPrecedence));
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().ResetConfigPrecedence();
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        /// <summary>
        /// Template Id: 681961
        /// 1. Bring up the device with DHCP IP.
        /// 2. Initially Configure the DHCP scope with Hostname and DNS server IP only
        /// 3. Configure a small lease of approximately 1 minute on the DHCPv4 server
        /// 4. Configure the precedence table with DHCPBootPManual/TFTP//DHCPv6/Default.
        /// 5. Verify the parameters details on the device.
        /// 6. Configure Domain name and WINS server IP using Manual on the device
        /// 7. Verify the parameters details on the device.
        /// 6. Again configure DHCP scope for Domain name and WINS server IP address.
        /// 8. Wait for DHCPv4 Lease renew.
        /// 9. Verify parameters details on the device.  
        /// Expected:
        /// 5. Device should configured with hostname and DNS server IP using DHCPv4 option and domain name, WINS server IP should be blank.
        /// 7. Device should accept  domain name and wins server IP as a manual. since the field as blank.
        /// 8. Device should overwrite the Domain name and WINS server IP address with DHCP server parameters. 
        /// </summary>
        public static bool VerifyConfigPrecedenceUsingDifferentMethod(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.DeleteDomainName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);
                serviceMethod.Channel.DeleteWinsServer(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);
                serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 60);

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                TraceFactory.Logger.Info("Setting DHCPv4/BootP as highest config precedence.");
                SnmpWrapper.Instance().SetConfigPrecedence("2:0:1:3:4");
                Thread.Sleep(TimeSpan.FromMinutes(1));

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                if (!ValidateHostName(family, activityData.ServerHostName))
                {
                    return false;
                }

                if (!ValidatePrimaryDnsServer(family, activityData.ServerDNSPrimaryIPAddress))
                {
                    return false;
                }

                if (!ValidateDomainName(family, string.Empty))
                {
                    return false;
                }

                if (!ValidatePrimaryWinsServer(family, string.Empty))
                {
                    return false;
                }

                string manualDomainname = "Manual_Domainname";
                string manualWinsServer = activityData.PrimaryDhcpServerIPAddress;

                if (!EwsWrapper.Instance().SetDomainName(manualDomainname))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetPrimaryWinServerIP(manualWinsServer))
                {
                    return false;
                }

                if (!ValidateDomainName(family, manualDomainname))
                {
                    return false;
                }

                if (!ValidatePrimaryWinsServer(family, manualWinsServer))
                {
                    return false;
                }

                serviceMethod.Channel.SetDomainName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.DomainName);
                serviceMethod.Channel.SetWinsServer(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, "{0}", "{0}".FormatWith(activityData.PrimaryDhcpServerIPAddress));

                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (!ValidateDomainName(family, activityData.DomainName))
                {
                    return false;
                }

                if (!ValidatePrimaryWinsServer(family, activityData.PrimaryDhcpServerIPAddress))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                RestoreServerParameter(activityData);
            }
        }

        /// <summary>
        /// Template Id: 682045
        /// 1.Configure a small lease of approximately 1 minute on the DHCPv4 server and  Configure a hostname using DHCPv4
        /// 2. Bring up the Device with DHCPv4
        /// 3. Configure precedence table with BOOTP/DHCPv4 as highest precedence 
        /// 4. Verify the device Hostname
        /// 5. Change hostname on the DHCPv4 server 
        /// 6. Stop the server until rebind start
        /// 7. Start the server once rebind start
        /// 8.Verify whether the new hostname configured on the DHCPv4 server is configured on Device after lease renew
        /// Expected:
        /// 4. Hostname is configured using DHCPv4 server
        /// 8. New host name configured on the DHCPv4 server should be configured on Device while Rebinding.
        /// Note: BOOTP can also be used for this test. It has been sparse out due to the limited number of BOOTP configurable parameters.
        /// System test should consider testing precedence equality of BOOTP and DHCPv4.
        /// </summary>
        public static bool VerifyParameterWithDhcpRebind(IPConfigurationActivityData activityData, int testNo)
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

            bool result = false;
            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 60);

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                TraceFactory.Logger.Info("Setting DHCPv4/BootP as highest config precedence.");
                SnmpWrapper.Instance().SetConfigPrecedence("2:0:1:3:4");
                Thread.Sleep(TimeSpan.FromMinutes(1));

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                if (!ValidateHostName(family, activityData.ServerHostName))
                {
                    return false;
                }

                string manualHostname = "Manual_Hostname";

                serviceMethod.Channel.SetHostName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, manualHostname);
                serviceMethod.Channel.StopDhcpServer();

                Thread.Sleep(TimeSpan.FromMinutes(1));

                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.StartDhcpServer();

                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (!ValidateHostName(family, manualHostname))
                {
                    return false;
                }

                result = true;
            }
            finally
            {
                PacketDetails details = client.Channel.Stop(guid);

                if (result)
                {
                    result = ValidateDhcpRebindPacket(activityData.PrimaryDhcpServerIPAddress, details.PacketsLocation, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
                }

                RestoreServerParameter(activityData);
            }

            return result;
        }

        /// <summary>
        /// Template Id: 681971
        /// 1. Configure the DHCPv4 scope with Hostname,DNS suffix  using DHCP Server
        /// 2. Configure DHCPv6 scope  with domainnamev6 and DNSv6 IP address  using  SNMP
        /// 3. Bring up the Device with DHCP IP.
        /// 4. Configure the precedence table with Manual/TFTP/DHCPBootPManual/DHCPv6/Default.
        /// 5. Configure Domain name and WINS server IP using Telnet
        /// 5. Verify the parameters details on the device.
        /// Expected:
        /// 5. Device should be able to configured with different UI for different IP parameters.
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool VerifyParameterWithDifferentUI(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info("Assumption: DNS Suffix is configured for {0} scope.".FormatWith(activityData.DHCPScopeIPAddress));

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                serviceMethod.Channel.DeleteDomainName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);
                serviceMethod.Channel.DeleteWinsServer(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);

                string v6DomainName = "hsslv6.com";

                SnmpWrapper.Instance().Setv6DomainName(v6DomainName);
                SnmpWrapper.Instance().SetPrimaryDnsv6Server(activityData.DHCPScopeIPv6Address);

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                // For TPS, resetting config precedence will remove previously set manual values.
                if (!PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                {
                    EwsWrapper.Instance().ResetConfigPrecedence();
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                }

                TelnetWrapper.Instance().SetDomainname(activityData.DomainName);
                TelnetWrapper.Instance().SetPrimaryWinsServer(activityData.PrimaryDhcpServerIPAddress);

                Thread.Sleep(TimeSpan.FromMinutes(3));

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                if (!ValidateHostName(family, activityData.ServerHostName))
                {
                    return false;
                }

                if (!ValidateDomainName(family, activityData.DomainName))
                {
                    return false;
                }

                if (!PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                {
                    if (!ValidateDomainSearchList(family, serviceMethod.Channel.GetDnsSuffix(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress)))
                    {
                        return false;
                    }
                }

                if (!ValidatePrimaryDnsV6Server(family, activityData.DHCPScopeIPv6Address))
                {
                    return false;
                }

                if (!ValidatePrimaryWinsServer(family, activityData.PrimaryDhcpServerIPAddress))
                {
                    return false;
                }

                return Validatev6DomainName(family, v6DomainName);
            }
            finally
            {
                client.Channel.Stop(guid);
                RestoreServerParameter(activityData);
            }

        }

        #endregion

        #region Auto IP

        /// <summary>
        /// 96198	
        /// TEMPLATE Device behavior when both Bootp and DHCP servers are available in the network.
        /// Verify that Device is Bootp configured when both Bootp server and DHCP server is available in network(This Test step is valid for VEP Products )
        /// 1. Connect device in a network with no Bootp and dhcp server.
        /// 2. Cold reset the device.
        /// 3. Verify that the device gets configured with Auto IP 
        /// 4. Bring up both Bootp and DHCP server in the network
        /// 1. The device should get configured with Auto IP as no servers are   available in the network . Initially ,the device  sends only BootP packet, afterwards it should send DHCP discover packets in the background.
        /// Once the Bootp/DHCP server comes up ,the device should comes up  with DHCP IP only. 
        /// TPS: Verify that Device is DHCP configured when both Bootp server and DHCP server is available in network(This Test step is valid for TPS Products )
        /// 1. Cold reset the device.
        /// 2.Connect the device in a network with no BootP and DHCP server.
        /// 3.Verify that the device gets configured with Auto IP
        /// 4. Bring up both the Bootp and DHCP server in the network
        /// Note : Perform this step for TPS  product.
        /// 3.The device should get configured with Auto IP as no serves are available in the network and it should send DHCP BootP packets in the background.
        /// 4.When the DHCP/-BootP server comes up, it should get configured with DHCP IP address as DHCP is the default preferred configuration method.
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool AutoIPWithDhcpBootPServers(IPConfigurationActivityData activityData, int testNo)
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

            string currentDeviceAddress = string.Empty;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                Printer.Printer printer = null;

                // For TPS, first cold reset the printer and bring to Auto IP.
                if (family.Equals(PrinterFamilies.TPS))
                {
                    printer = PrinterFactory.Create(family, IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                    if (!ColdReset(activityData, printer))
                    {
                        return false;
                    }

                    if (!ConfigurePrinterWithAutoIP(activityData, out currentDeviceAddress))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!ConfigurePrinterWithAutoIP(activityData, out currentDeviceAddress))
                    {
                        return false;
                    }
                    printer = PrinterFactory.Create(activityData.ProductFamily, currentDeviceAddress);

                    if (!ColdReset(activityData, printer))
                    {
                        return false;
                    }
                }

                if (!ValidateDefaultIP(activityData))
                {
                    return false;
                }

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.StartDhcpServer();

                Thread.Sleep(TimeSpan.FromMinutes(LEASE_WAIT_TIME));

                NetworkUtil.RenewLocalMachineIP();

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                AutoIPPostRequisites(activityData);
            }
        }

        /// <summary>
        /// 654412
        /// Template  Verify that the device goes to Auto IP when the BootP server is unavailable along with DHCP request option is disabled.	
        /// Verify that the device goes to Auto IP when the BootP server is unavailable.
        /// 1.Bring up the device with BootP IP,
        /// 2.Stop the BootP server and make sure that there is no server in the network 
        /// 3.Open the  EWS page and uncheck the option 'Send dhcp request when in Auto IP/legacy IP' which comes under Advanced Tab in TCP/IP settings.
        /// 4.Power cycle the Device.
        /// 5.Bring up the device with DHCP/Bootp server "	
        /// "5. Device should go to Auto IP. Make sure that no  BootP/DHCP packets are seen in the Wire shark Capture after power cycling"
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns>true for success, else false.</returns>
        public static bool AutoIPWithOnlyBootPAndDHCPRequestDisable(IPConfigurationActivityData activityData, int testNo)
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

            string currentDeviceAddress = activityData.PrimaryWiredIPv4Address;
            string hostName = string.Empty;

            bool result = false;
            PacketDetails validatePacketDetails = null;
            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);

                if (!serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Bootp))
                {
                    return false;
                }

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                serviceMethod.Channel.StopDhcpServer();

                // Since lease time is default (8 days) and printer ip configuration is not renewed, printer will be accessible with the IPv4 address.
                EwsWrapper.Instance().SetSendDHCPRequestOnAutoIP(false);

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);

                // Since lease time is default (8 days) and printer ip configuration is not renewed, printer will be accessible. Once printer reboots, it acquires Auto IP					
                printer.PowerCycle();

                if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out currentDeviceAddress))
                {
                    return false;
                }

                NetworkUtil.RenewLocalMachineIP();

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, currentDeviceAddress))
                {
                    return false;
                }

                string validateGuid = string.Empty;

                try
                {
                    validateGuid = client.Channel.TestCapture(activityData.SessionId, string.Format("{0}_NoPackets_Validation", testNo));

                    // Since the DHCP service is stopped, previously created DhcpApplicationServiceClient won't work.
                    // New object created will route through different network
                    serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                    serviceMethod.Channel.StartDhcpServer();
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(5));
                        validatePacketDetails = client.Channel.Stop(validateGuid);
                    }
                }

                // since the 'Send DHCP Request On AutoIP' option is set to false, the printer acquires Auto IP even when the server is up.
                if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out currentDeviceAddress))
                {
                    return false;
                }

                printer = PrinterFactory.Create(activityData.ProductFamily, currentDeviceAddress);
                currentDeviceAddress = printer.IPv6LinkLocalAddress.ToString();

                if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(currentDeviceAddress), TimeSpan.FromSeconds(20)))
                {
                    TraceFactory.Logger.Debug("Unable to ping {0}".FormatWith(currentDeviceAddress));

                    currentDeviceAddress = printer.HostName;

                    if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(currentDeviceAddress), TimeSpan.FromSeconds(20)))
                    {
                        TraceFactory.Logger.Debug("Unable to ping {0}".FormatWith(currentDeviceAddress));
                        return false;
                    }
                }

                TraceFactory.Logger.Debug("Current device address: {0}".FormatWith(currentDeviceAddress));
                TraceFactory.Logger.Info("Ping and Telnet validation are not performed.");
                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, currentDeviceAddress, validatePing: false, validateTelnet: false))
                {
                    return false;
                }

                result = true;
            }
            finally
            {
                client.Channel.Stop(guid);

                if (result)
                {
                    TraceFactory.Logger.Info("Expected: Dhcp discover packets are not found.");
                    result = !ValidateDhcpDiscoverPacket(activityData.PrimaryDhcpServerIPAddress, validatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress);
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                // Setting the option ' Send DHCP Request On AutoIP ' to true to make sure printer acquires IP Address from DHCP server   
                EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);
                EwsWrapper.Instance().SetSendDHCPRequestOnAutoIP(true);
                AutoIPPostRequisites(activityData);
            }

            return result;
        }

        /// <summary>
        /// Template ID : 654413
        /// 1.Bring up the device with BootP IP. 
        /// 2.Stop the Bootp server
        /// 3. power cycle the printer and wait for Device goes to AutoIP
        /// 4.Bring up the BootP/DHCP server
        /// Expected :
        /// 3. The device should  go to Auto IP.
        /// Note: In the backend,it should send DHCP discover packet as the option 'Send dhcp request when in Auto IP/legacy IP' is not unchecked.
        /// 4. The device should take DHCP IP"
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool AutoIPWithBootP(IPConfigurationActivityData activityData, int testNo)
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

            bool result = false;
            PacketDetails validatePacketDetails = null;
            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails = GetInterfaceDetails(activityData);

            try
            {

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Set DHCP as config method for all interfaces
                printerInterfaceDetails.ForEach(x => { x.PrinterReservationType = ReservationType.Bootp; x.ConfigMethod = IPConfigMethod.BOOTP; });

                // Update the link local address for wireless interface
                Printer.Printer printer = null;
                printerInterfaceDetails.ForEach(x =>
                {
                    if (x.PrinterInterface == PrinterConnectionType.WirelessInterface)
                    {
                        printer = PrinterFactory.Create(activityData.ProductFamily, x.Address);
                        x.LinkLocalAddress = printer.IPv6LinkLocalAddress;
                    }
                });

                if (!ConfigureInterface(printerInterfaceDetails))
                {
                    return false;
                }

                foreach (var item in printerInterfaceDetails)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(item.Address);
                    EwsWrapper.Instance().SetSendDHCPRequestOnAutoIP(true);
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);

                DhcpApplicationServiceClient serviceMethods = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethods.Channel.StopDhcpServer();

                string validateGuid = string.Empty;

                printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);

                try
                {
                    validateGuid = client.Channel.TestCapture(activityData.SessionId, string.Format("{0}_DhcpDiscover_Validation", testNo));
                    printer.PowerCycle();
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        validatePacketDetails = client.Channel.Stop(validateGuid);
                    }
                }

                if (!IsPrinterInAutoIP(ref printerInterfaceDetails))
                {
                    return false;
                }

                NetworkUtil.RenewLocalMachineIP();

                printerInterfaceDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.AUTOIP; });

                if (!ValidateIPConfigMethod(activityData, printerInterfaceDetails))
                {
                    return false;
                }

                // Since the DHCP service is stopped, previously created DhcpApplicationServiceClient won't work.
                // New object created will route through different network
                serviceMethods = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                serviceMethods.Channel.StartDhcpServer();
                // Since Printer was brought up with BootP, printer can acquire only BootP IP.
                // Printer can only acquire DHCP IP when it recovers from Auto IP, hence creating reservation with 'Both' type.

                printerInterfaceDetails = GetInterfaceDetails(activityData);

                if(!ConfigureInterface(printerInterfaceDetails, true, false))
                {
                    return false;
                }

                Thread.Sleep(TimeSpan.FromMinutes(LEASE_WAIT_TIME));

                NetworkUtil.RenewLocalMachineIP();

                // VEP: Printer takes DHCP IP coming from Auto IP.
                // TPS: Printer takes Preferred config method (BootP which is previously set)
                printerInterfaceDetails.ForEach(x => { x.ConfigMethod = ((PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily)) ? IPConfigMethod.BOOTP : IPConfigMethod.DHCP); });

                result = ValidateIPConfigMethod(activityData, printerInterfaceDetails);
            }
            finally
            {
                client.Channel.Stop(guid);

                if (result)
                {
                    result = ValidateDhcpDiscoverPacket(activityData.PrimaryDhcpServerIPAddress, validatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress);
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                AutoIPPostRequisites(activityData, printerInterfaceDetails);
            }

            return result;
        }

        /// <summary>
        /// Template ID : 654417
        /// Obtain a basic IP address from DHCP server for all interfaces when the DHCP server is available	
        /// "1. Configure the DHCP server lease time as 2mins
        /// 2. Brin up the device in DHCP IP for Embedded Wired, Europa Wired and WirelessInterface interfaces.
        /// 3. Make sure the Default IP = AutoIP and 
        /// also confirm check box is enabled for ""send dhcp request if IP address as AutopIP/Legacy""  in the Advanced tab of the EWS page
        /// 4. Stop the DHCP server and wait for 5 mins
        /// 5. Verify the device behaviour.
        /// 6. Power cycle the device. 
        /// 7. Once again verify the device behaviour 
        /// 8. Start the DHCP server
        /// 9. Once again verify the device behaviour"	
        /// "5. Embedded and Europa Wired interfaces and then WirelessInterface Interface on the Device should aquire the AUTO IP.
        /// All the interfaces should send the DHCP Discover Packet during AutoIP in the backend
        /// 7. After powercylcing also Embedded and Europa Wired and then WirelessInterface Interfaces on the Device should aquire the AUTO IP.
        /// All the interfaces should send the DHCP Discover Packet.
        /// 9 .Make sure device acquires DHCP IP for all the interfaces after bringing up the DHCP server"
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool AutoIPWithLessLeasetime(IPConfigurationActivityData activityData, int testNo)
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

            PacketDetails validatePacketDetails = null;
            PacketDetails dhcpValidatePacketDetails = null;
            bool result = false;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                
                DhcpApplicationServiceClient serviceMethods = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethods.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 120);

                // This will make sure printer gets configured with 120 seconds lease time

                Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails = GetInterfaceDetails(activityData);

				// Get the link local address of wireless interface 
				Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
				
                printerInterfaceDetails.ToList().ForEach(x =>
                {
                    if (x.PrinterInterface == PrinterConnectionType.WirelessInterface)
                    {
                        printer = PrinterFactory.Create(activityData.ProductFamily, x.Address);
                        x.LinkLocalAddress = printer.IPv6LinkLocalAddress;
                    }
                });

                // Set BOOTP as config method for all interfaces
                printerInterfaceDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.BOOTP; });

                if (!ConfigureInterface(printerInterfaceDetails, false, true))
                {
                    return false;
                }

                // Set DHCP as config method for all interfaces
                printerInterfaceDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.DHCP; });

                if (!ConfigureInterface(printerInterfaceDetails, false, true))
                {
                    return false;
                }

                foreach (var item in printerInterfaceDetails)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(item.Address);
                    EwsWrapper.Instance().SetSendDHCPRequestOnAutoIP(true);
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);

                if (serviceMethods.Channel.StopDhcpServer())
                {
                    TraceFactory.Logger.Info("DHCP Server is stopped.");
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to stop the DHCP Server.");
                    return false;
                }

                Thread.Sleep(TimeSpan.FromMinutes(5));

                if (!IsPrinterInAutoIP(ref printerInterfaceDetails))
                {
                    return false;
                }

                NetworkUtil.RenewLocalMachineIP();

                printerInterfaceDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.AUTOIP; });

				if (!ValidateIPConfigMethod(activityData, printerInterfaceDetails))
                {
                    return false;
                }

                // Create printer instance with primary inteface auto ip
				printer = PrinterFactory.Create(activityData.ProductFamily, printerInterfaceDetails.FirstOrDefault(x => x.PrinterInterface == PrinterConnectionType.PrimaryWiredInterface).Address);

                string validateGuid = string.Empty;

                try
                {
                    validateGuid = client.Channel.TestCapture(activityData.SessionId, string.Format("{0}_DhcpDiscover_Validation", testNo));
                    printer.PowerCycle();
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        validatePacketDetails = client.Channel.Stop(validateGuid);
                    }
                }

                if (!IsPrinterInAutoIP(ref printerInterfaceDetails))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, printerInterfaceDetails))
                {
                    return false;
                }

                // Since the DHCP service is stopped, previously created DhcpApplicationServiceClient won't work.
                // New object created will route through different network
                serviceMethods = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                try
                {
                    validateGuid = client.Channel.TestCapture(activityData.SessionId, string.Format("{0}_Dhcp_Validation", testNo));
                    serviceMethods.Channel.StartDhcpServer();
                    Thread.Sleep(TimeSpan.FromMinutes(LEASE_WAIT_TIME));
                    NetworkUtil.RenewLocalMachineIP();
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        dhcpValidatePacketDetails = client.Channel.Stop(validateGuid);
                    }
                }

                //Printer acquires DHCP IP here.
                printerInterfaceDetails = GetInterfaceDetails(activityData);
                printerInterfaceDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.DHCP; });
                result = ValidateIPConfigMethod(activityData, printerInterfaceDetails);
            }
            finally
            {
                client.Channel.Stop(guid);

                //if (result)
                //{
                //    result = ValidateDhcpDiscoverPacket(activityData.PrimaryDhcpServerIPAddress, validatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress)
                //            && ValidateDhcpPackets(activityData.PrimaryDhcpServerIPAddress, dhcpValidatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress, activityData.ProductFamily);
                //}

                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                AutoIPPostRequisites(activityData);
            }

            return result;
        }

        /// <summary>
        /// Template ID : 654419
        /// Template Verify device goes to AutoIP and DHCPv6 IP is removed when no server is available with finite lease time	
        /// Verify Device goes to AutoIP after power cycling with finite lease time, when the DHCP server is unavailable.
        /// Configure the Both DHCPv4 and DHCPv6 server with finite lease time( lease time for Both DHCPv4 and DHCPv6 as 3 minutes is configured on the DHCP Server)
        /// Bring up the Device in DHCP network
        /// Stop the DHCP service on the Server
        /// Power cycle the Device 
        /// Verify the Device behavior.
        /// Expected :
        /// Device should acquires a DHCPv4 IP and DHCPv6 IP with the finite lease time.
        /// After power cycling Device should goes to AutoIP for v4 and DHCPv6 IP should be removed.  
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool VerifyAutoIPWithFiniteLease(IPConfigurationActivityData activityData, int testNo)
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

            bool result = false;

            DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());
            PacketDetails validatePacketDetails = null;

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Make sure that the printer acquires a DHCPv6 address
                EwsWrapper.Instance().SetDHCPv6OnStartup(true);

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
                // Fetch the dhcpv6 address
                IPAddress dhcpv6Address = printer.IPv6StateFullAddress;

                // Setting ipv4 and ipv6 preferred lease time to 3 minutes and ipv6 valid lease time to 4 minutes.
                // Note : Valid lease time should be greater than preferred lease time.
                serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 180);
                serviceMethod.Channel.SetPreferredLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, 180);
                serviceMethod.Channel.SetValidLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, 240);

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                serviceMethod.Channel.StopDhcpServer();

                string validateGuid = string.Empty;

                try
                {
                    validateGuid = client.Channel.TestCapture(activityData.SessionId, string.Format("{0}_Dhcp_Validation", testNo));
                    printer.PowerCycle();
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(5));
                        validatePacketDetails = client.Channel.Stop(validateGuid);
                    }
                }

                string autoIPAddress = string.Empty;

                if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out autoIPAddress))
                {
                    return false;
                }

                NetworkUtil.RenewLocalMachineIP();

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, autoIPAddress))
                {
                    return false;
                }

                printer = PrinterFactory.Create(activityData.ProductFamily, autoIPAddress);

                if (null == printer.IPv6StateFullAddress)
                {
                    TraceFactory.Logger.Info("DHCPv6 address is cleared from the printer when the server is down.");
                }
                else
                {
                    TraceFactory.Logger.Info("DHCPv6 address is not cleared from the printer when the server is down.");
                    return false;
                }

                if (PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily))
                {
                    TraceFactory.Logger.Info("SNMP validation for TPS is not implemented.");
                }
                else
                {
                    SnmpWrapper.Instance().Create(autoIPAddress);

                    if (SnmpWrapper.Instance().GetIPv6StateFullAddress().Equals(dhcpv6Address))
                    {
                        TraceFactory.Logger.Info("SNMP Validation failed : DHCPv6 address is not cleared from the printer when the DHCPv6 scope is configured with finite lease time and the server is down.");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("SNMP Validation successful : DHCPv6 address is cleared from the printer when the DHCPv6 scope is configured with finite lease time and the server is down.");
                    }
                }

                // DHCPv6 address will not be present in the printer.
                Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails();

                if (IPv6Utils.GetStatefulAddress(ipv6Details).Equals(dhcpv6Address))
                {
                    TraceFactory.Logger.Info("EWS Validation failed : DHCPv6 address is not cleared from the printer when the DHCPv6 scope is configured with finite lease time and the server is down.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("EWS Validation successful : DHCPv6 address is cleared from the printer when the DHCPv6 scope is configured with finite lease time and the server is down.");
                }

                result = true;
            }
            finally
            {
                client.Channel.Stop(guid);

                if (result)
                {
                    result = ValidateDhcpDiscoverPacket(activityData.PrimaryDhcpServerIPAddress, validatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress);
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                AutoIPPostRequisites(activityData);

                EwsWrapper.Instance().SetDHCPv6StatelessConfigurationOption();

                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                serviceMethod.Channel.SetValidLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, DEFAULT_VALID_LEASE_TIME);
                serviceMethod.Channel.SetPreferredLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, DEFAULT_PREFERRED_LEASE_TIME);
            }

            return result;
        }

        /// <summary>
        /// Template ID : 656192
        /// 1. Bring up the only BootP server
        /// 2. Connect the device to the BootP network	
        /// 2..Change the config method from BootP to DHCP,  but the  DHCP server should not be available. 
        /// Expected :
        /// 2.  Device should acquired the BootP IP.
        /// 3. The device should initally start with DHCP process, since DHCP server is not available in the Network, Device should acquire a Auto IP.
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool AutoIPWithOnlyBootP(IPConfigurationActivityData activityData, int testNo)
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

            PacketDetails validatePacketDetails = null;
            bool result = false;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                DhcpApplicationServiceClient serviceMethods = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethods.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
                serviceMethods.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Bootp);

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.BOOTP))
                {
                    return false;
                }

                string validateGuid = string.Empty;

                try
                {
                    validateGuid = client.Channel.TestCapture(activityData.SessionId, string.Format("{0}_Dhcp_Validation", testNo));

                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address), validate: false);
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        // Since DHCP server is unavailable, Printer will acquire Auto IP for VEP and BOOTP IP for TPS. This will take a minute or so, hence adding a sleep delay
                        Thread.Sleep(TimeSpan.FromMinutes(5));
                        validatePacketDetails = client.Channel.Stop(validateGuid);
                    }
                }

                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    return ValidateIPConfigMethod(activityData, IPConfigMethod.BOOTP, activityData.PrimaryWiredIPv4Address);
                }
                else
                {
                    string autoIPAddress = string.Empty;

                    if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out autoIPAddress))
                    {
                        return false;
                    }

                    // WCF connection timeout might occur if above steps take long time. Creating new object to re-establish connection 
                    serviceMethods = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                    serviceMethods.Channel.StopDhcpServer();

                    NetworkUtil.RenewLocalMachineIP();

                    result = ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, autoIPAddress);
                }
            }
            finally
            {
                client.Channel.Stop(guid);

                if (result)
                {
                    result = ValidateDhcpDiscoverPacket(activityData.PrimaryDhcpServerIPAddress, validatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress);
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                {
                    DhcpApplicationServiceClient serviceMethods = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                    serviceMethods.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
                    serviceMethods.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Both);

                    EwsWrapper.Instance().SetDefaultIPConfigMethod(validate: false, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                }
                else
                {
                    AutoIPPostRequisites(activityData);
                }
            }

            return result;
        }

        /// <summary>
        /// Template ID : 656204
        /// 1. Bring up the only DHCP server
        /// 2. Connect the device to the DHCP network 
        /// 2..Change the config method from DHCP to BootP,  but the BootP server should not be available.
        /// Expected :
        /// 2.  Device should acquired the DHCP IP. 
        /// 3.  The device should initally start with BootP request, since BooTP server is not available in the Network, Device should acquire a DHCP IP. 
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool AutoIPWithDHCP(IPConfigurationActivityData activityData, int testNo)
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

            DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            PacketDetails validatePacketDetails = null;
            bool result = false;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
                serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Dhcp);

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP))
                {
                    return false;
                }

                string validateGuid = string.Empty;

                try
                {
                    validateGuid = client.Channel.TestCapture(activityData.SessionId, string.Format("{0}_Bootp_Dhcp_Validation", testNo));
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address), validate: false);

                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        // Since BootP server is unavailable, Printer will acquire DHCP. This will take a minute or so, hence adding a sleep delay
                        Thread.Sleep(TimeSpan.FromMinutes(5));
                        validatePacketDetails = client.Channel.Stop(validateGuid);
                    }
                }

                // Inkjet acquires auto ip
                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString()))
                {
                    // WCF connection timeout might occur if above steps take long time. Creating new object to re-establish connection 
                    DhcpApplicationServiceClient serviceMethods = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                    serviceMethods.Channel.StopDhcpServer();

                    NetworkUtil.RenewLocalMachineIP();

                    string autoIPAddress = string.Empty;

                    if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out autoIPAddress))
                    {
                        return false;
                    }

                    result = ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, autoIPAddress);
                }
                else
                {
                    if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP))
                    {
                        return false;
                    }
                }

                result = true;
            }
            finally
            {
                if (result)
                {
                    result = ValidateBootPRequestPackets(activityData.PrimaryDhcpServerIPAddress, validatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress)
                        && ((activityData.ProductFamily.EqualsIgnoreCase(activityData.ProductFamily)) ? true : ValidateDhcpPackets(activityData.PrimaryDhcpServerIPAddress, validatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress, activityData.ProductFamily));
                }

                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString()))
                {
                    DhcpApplicationServiceClient serviceMethods = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                    serviceMethods.Channel.StartDhcpServer();

                    NetworkUtil.RenewLocalMachineIP();

                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);

                    serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
                    serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Both);

                    CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address, TimeSpan.FromMinutes(3));
                }
                else
                {
                    serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
                    serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Both);
                }
            }

            return result;
        }

        #region VerifyPartialConfigurationWithInfiniteLease

        /// <summary>
        /// 96166
        /// Template Test renew lease on power cycle, no server, going to partial configuration	
        /// Test renew lease on power cycle, no server, going to partial configuration
        /// 1. Set the DHCP scope to allow for an infinite lease time in the DHCP Server .
        /// 2. Stop the DHCP Services on the Server. 
        /// 3. Power cycle the Device. and Capture a trace. 
        /// 4. Bring up the DHCP server.
        /// 5. Again, stop the DHCP service on the Server. 
        /// 6. Power cycle the Device. 
        /// 7. Once Again Do power cycle the Device 
        /// 8. Bring up the DHCP server once again 
        /// Expected:
        /// 1. The device should get DHCP IP and configured with an infinite lease time.
        /// 2. Even if the server is offline the device should still retain the previous IP.
        /// 3. On power- cycling the printer, it should be retain the  previous IP address and then the configuration page should displays ""UNABLE TO CONNECT TO DHCP SERVER"" 
        ///		and in the packet capture the printer should send continuously  ""DHCP Discover"" packet.
        /// 4.  When the DHCP server is available, the DUT should take a proper DHCP IP and  clear the error i.e  ""UNABLE TO CONNECT TO DHCP SERVER"".
        /// 5. Even if the server is offline the device should still retain the previous IP.
        /// 6. On power- cycling the printer, it should be retain the  previous IP address and then the configuration page should displays ""UNABLE TO CONNECT TO DHCP SERVER""  
        ///		and in the packet capture the printer should send continuously  ""DHCP Discover"" packet.
        /// 7.  Device should go to AutoIP.
        /// 8. Device should acquire a DHCP IP.
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool VerifyPartialConfigurationWithInfiniteLease(IPConfigurationActivityData activityData, int testNo)
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

            PacketDetails validatePacketDetails = null;
            PacketDetails dhcpValidatePacketDetails = null;
            bool result = false;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            Collection<PrinterInterfaceConfigurationDetails> configurationDetails = GetInterfaceDetails(activityData);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("STEP 1: Configure Infinite lease time on the server.");

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                if (!serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, INFINITE_LEASE_TIME))
                {
                    TraceFactory.Logger.Info("Failed to set infinite lease time on the server.");
                    return false;
                }

                TraceFactory.Logger.Info("STEP 2: Configure DHCP IP for the printer.");
                configurationDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.BOOTP; });
               
                if(!ConfigureInterface(configurationDetails, false))
                {
                    return false;
                }

                configurationDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.DHCP; });

                if (!ConfigureInterface(configurationDetails, false))
                {
                    return false;
                }

                configurationDetails.ForEach(x => { x.LeaseTime = -1; });

                if (!ValidateLeaseTime(activityData, configurationDetails))
                {
                    TraceFactory.Logger.Info("Printer failed to acquire Infinite lease time.");
                    return false;
                }

                TraceFactory.Logger.Info("Printer acquired Infinite lease time.");

                TraceFactory.Logger.Info("STEP 3: Stop DHCP Service on DHCP Server.");

                if (!serviceMethod.Channel.StopDhcpServer())
                {
                    TraceFactory.Logger.Info("Failed to stop the DHCP Server {0}".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                    return false;
                }

               if(!VerifySameIPAcquisition(activityData, configurationDetails))
                {
                    return false;
                }

                TraceFactory.Logger.Info("STEP 4: Power Cycle the printer.");

                string validateGuid = string.Empty;

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);

                try
                {
                    validateGuid = client.Channel.TestCapture(activityData.SessionId, string.Format("{0}_DhcpDiscover_Validation", testNo));
                    printer.PowerCycle();
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(2));
                        validatePacketDetails = client.Channel.Stop(validateGuid);
                    }
                }


                if (!VerifySameIPAcquisition(activityData, configurationDetails))
                {
                    return false;
                }

                foreach (var item in configurationDetails)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(item.Address);
                    if (!EwsWrapper.Instance().ValidatePartialConfiguration())
                    {
                        return false;
                    }
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);

                TraceFactory.Logger.Info("STEP 5: Start DHCP Service on DHCP Server.");
                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                if (!serviceMethod.Channel.StartDhcpServer())
                {
                    TraceFactory.Logger.Info("Failed to start the DHCP Server {0}".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                    return false;
                }

                // Wait for some time to reflect the server configuration on Web UI.
                TraceFactory.Logger.Debug("Waiting for {0} minutes for configuration update.".FormatWith(LEASE_WAIT_TIME));
                Thread.Sleep(TimeSpan.FromMinutes(LEASE_WAIT_TIME));

                foreach (var item in configurationDetails)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(item.Address);
                    if (EwsWrapper.Instance().ValidatePartialConfiguration())
                    {
                        return false;
                    }
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);

                TraceFactory.Logger.Info("STEP 6: Stop DHCP Service on DHCP Server.");

                if (!serviceMethod.Channel.StopDhcpServer())
                {
                    TraceFactory.Logger.Info("Failed to stop the DHCP Server {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                    return false;
                }

                if(!VerifySameIPAcquisition(activityData, configurationDetails))
                {
                    return false;
                }

                TraceFactory.Logger.Info("STEP 7: Power Cycle the printer.");

                validateGuid = string.Empty;

                try
                {
                    validateGuid = client.Channel.TestCapture(activityData.SessionId, string.Format("{0}_DhcpDiscover_1_Validation", testNo));

                    printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
                    printer.PowerCycle();
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(2));
                        dhcpValidatePacketDetails = client.Channel.Stop(validateGuid);
                    }
                }

                if (!VerifySameIPAcquisition(activityData, configurationDetails))
                {
                    return false;
                }

                configurationDetails.ForEach(x =>
                {
                    x.LinkLocalAddress = (x.PrinterInterface == PrinterConnectionType.WirelessInterface) ? PrinterFactory.Create(activityData.ProductFamily, x.Address).IPv6LinkLocalAddress : IPAddress.None;
                });

                foreach (var item in configurationDetails)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(item.Address);
                    if (!EwsWrapper.Instance().ValidatePartialConfiguration())
                    {
                        return false;
                    }
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);

                TraceFactory.Logger.Info("STEP 8: Power Cycle the printer.");

                // Power cycle is not validated here as the printer is expected to acquire auto ip.
                printer.PowerCycle();

                string autoIpAddress = string.Empty;

               
                if(!IsPrinterInAutoIP(ref configurationDetails))
                {
                    return false;
                }

                NetworkUtil.RenewLocalMachineIP();

                configurationDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.AUTOIP; });

                if (!ValidateIPConfigMethod(activityData, configurationDetails))
                {
                    return false;
                }

                TraceFactory.Logger.Info("STEP 9: Bring up the DHCP Server.");
                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                if (!serviceMethod.Channel.StartDhcpServer())
                {
                    TraceFactory.Logger.Info("Failed to start the DHCP Server {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));
                    return false;
                }

                Thread.Sleep(TimeSpan.FromMinutes(LEASE_WAIT_TIME));
                NetworkUtil.RenewLocalMachineIP();

                configurationDetails = GetInterfaceDetails(activityData);
                configurationDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.DHCP; });
                result = ValidateIPConfigMethod(activityData, configurationDetails);
            }
            finally
            {
                client.Channel.Stop(guid);

                if (result)
                {
                    result = ValidateDhcpDiscoverPacket(activityData.PrimaryDhcpServerIPAddress, validatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress) &&
                             ValidateDhcpDiscoverPacket(activityData.PrimaryDhcpServerIPAddress, dhcpValidatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress);
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                AutoIPPostRequisites(activityData, configurationDetails);
            }

            return result;
        }

        #endregion

        #region VerifyLeaseRenewalOnServerDown

        /// <summary>
        /// 96169
        /// Template Verify lease renewal behavior if the DHCPv4 server goes down
        /// 1. Connect the device in a network with a DHCPv4 server with a lease of 2 mins 
        /// 2. Now make the DHCP server offline.
        /// 3. Verify the Device sends the rebinding packet in the  packet trace.
        /// Expected:
        /// 1.Ensure device get configured with a DHCPv4 IP with a lease of 2 mins .
        /// 3. After one min lease it should try for lease renewal, since DHCP server is down and ensure that the device sends a rebind packet also after (approx )1min 45 secs.
        /// 4. Device should acquire AutoIP.
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool VerifyLeaseRenewalOnServerDown(IPConfigurationActivityData activityData, int testNo)
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

            bool result = false;
            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                if (!serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 120))
                {
                    return false;
                }

                // This will make sure lease time is set on Printer
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address), validate: false);
                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                if (!ValidateLeaseTime(activityData.ProductFamily, 120))
                {
                    TraceFactory.Logger.Info("Printer failed to acquire 120 seconds lease time.");
                    return false;
                }

                TraceFactory.Logger.Info("Bring down the DHCP Server.");

                if (!serviceMethod.Channel.StopDhcpServer())
                {
                    return false;
                }

                // Check for rebind packet
                Thread.Sleep(TimeSpan.FromMinutes(LEASE_WAIT_TIME));
                NetworkUtil.RenewLocalMachineIP();

                string autoIPAddress = string.Empty;

                if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out autoIPAddress))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, autoIPAddress))
                {
                    return false;
                }

                result = true;
            }
            finally
            {
                PacketDetails details = client.Channel.Stop(guid);

                if (result)
                {
                    result = ValidateDhcpRebindPacket(activityData.PrimaryDhcpServerIPAddress, details.PacketsLocation, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                AutoIPPostRequisites(activityData);
            }

            return result;
        }

        #endregion

        #region VerifyAutoIpOnMultipleColdReset

        /// <summary>
        /// Template ID : 96133
        /// Template  Verify that device is Auto IP configured on a link-local network	
        /// 1.Put a device on a link-local network without a BOOTP/DHCP/RARP server on the network.
        /// 2.Cold reset the device.
        /// 3.Repeat the first two steps twice.
        /// Expected :
        /// 1.Device should sense the network and go to an Auto IP address of 169.254/16 every time
        /// For VEP
        /// The device should send only DHCP Discover packet in the background ( BootP request should not be sent)
        /// For TPS
        /// 2. The device should continue to send Bootp Request and DHCP discover packets in the background.
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns>true for success, else false.</returns>
        public static bool VerifyAutoIPOnMultipleColdReset(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                string autoIPAddress = string.Empty;

                if (!ConfigurePrinterWithAutoIP(activityData, out autoIPAddress))
                {
                    return false;
                }

                // Create printer object				
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, autoIPAddress); ;

                // Performing cold rest twice as per test steps
                for (int i = 0; i < 2; i++)
                {
                    // Cold reset is not validated as the printer acquires auto ip after cold reset.
                    ColdReset(activityData, printer);

                    // After cold reset enabling WSDiscovery option
                    EwsWrapper.Instance().SetWSDiscovery(true);

                    // After cold reset there is a chance that the IP may be changed.
                    string currentDeviceAddress = string.Empty;

                    if (!ValidateDefaultIP(activityData, ref currentDeviceAddress))
                    {
                        return false;
                    }

                    // If the IP address is not returned from ValidateDefaultIP(), discover it again and get the current device address
                    if (string.IsNullOrEmpty(currentDeviceAddress))
                    {
                        currentDeviceAddress = Utility.GetPrinterIPAddress(activityData.PrimaryMacAddress);
                    }

                    if (string.IsNullOrEmpty(currentDeviceAddress))
                    {
                        TraceFactory.Logger.Info("Printer was not discovered after cold reset.");
                        return false;
                    }

                    printer = PrinterFactory.Create(activityData.ProductFamily, currentDeviceAddress);
                }
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                AutoIPPostRequisites(activityData);
            }

            return true;
        }

        #endregion

        #region VerifyAutoIpAfterPowercycleAndColdReset

        /// <summary>
        /// Template ID : 96152
        /// Template Verify Auto IP configuration after power cycle and cold reset	
        /// Step 1: Bring up the Device with AutoIP 
        /// Verify that default type = Auto IP.
        /// Power cycle the device. 
        /// Step 2:  Bring up the Device with AutoIP 
        /// Verify that default type = Auto IP.
        /// Cold reset the device.
        /// Expected:
        /// Step1: The device should still be configured  with the same Auto IP address as before the power cycle.
        /// Step2: On cold reset the device should get configured with any of IP configuration methods if server is available. 
        /// If no server is available then the device should be Auto IP configured.
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool VerifyAutoIPAfterPowerCycleAndColdReset(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                string autoIPAddress = string.Empty;

                if (!ConfigurePrinterWithAutoIP(activityData, out autoIPAddress))
                {
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(autoIPAddress));

                if (!EwsWrapper.Instance().GetDefaultIPType().Equals(DefaultIPType.AutoIP))
                {
                    TraceFactory.Logger.Info("Printer Default IP Type is not Auto IP.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer Default IP Type is Auto IP.");
                }

                // Create printer object
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, autoIPAddress);

                printer.PowerCycle();

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, autoIPAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info("DHCP Server is not available:");

                ColdReset(activityData, printer);

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);

                // Sometimes printer acquires different Auto IP after Cold reset
                if (!ValidateDefaultIP(activityData))
                {
                    return false;
                }

                ColdReset(activityData, printer);

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.StartDhcpServer();

                Thread.Sleep(TimeSpan.FromMinutes(LEASE_WAIT_TIME));
                NetworkUtil.RenewLocalMachineIP();

                TraceFactory.Logger.Info("DHCP Server is available:");

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP))
                {
                    return false;
                }
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                AutoIPPostRequisites(activityData);
            }

            return true;
        }

        #endregion

        #region VerifyLegacyIpAfterPowerCycle

        /// <summary>
        /// Template ID : 96154
        /// Template Verify default 192.0.0.192 configuration after power cycle when default type = legacy default or AutoIP
        /// 1.Place the device on a non link-local network with no BOOTP/DHCP/RARP server. 
        /// 2.Cold reset and let device go to the default IP address of 192.0.0.192 using Carrier sense 
        /// 3.Verify that default type = legacy default IP. 
        /// 4.Power cycle the device.
        /// 5.Via Web UI, change the default type to Auto IP. 
        /// 6. Power cycle the device. 
        /// 7.Ensure that the device acquires AutoIP. 
        /// 8. Change the default  type = legacy default. 
        /// 9.Power cycle the device.
        /// Expected:
        /// 4.After power cycle,  the device  should become default  or legacy IP (192.0.0.192 ) configured.
        /// 6. On applying changes via WebUI and power cycling,the device should become Auto IP configured (169.254.x.x)
        /// 9.The device should still be configured by Auto IP with the same Auto IP address as it had been  before the power cycle.
        /// (The device ignores  the default type while it is  Auto IP configured). But, the default type should now be legacy default.
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool VerifyLegacyIPAfterPowerCycle(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string autoIPAddress = string.Empty;
                string printerHostName = string.Empty;

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
                IPAddress linkLocalAddress = printer.IPv6LinkLocalAddress;
                DefaultIPType defaultType;

                if (PrinterFamilies.VEP.ToString() == activityData.ProductFamily)
                {
                    TraceFactory.Logger.Info("STEP 1: DHCPv4 Server lease time is set to 1 minute, changing Default type to LegacyIP");

                    DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                    serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 60);

                    // This is done to get latest configuration from DHCP server
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                    EwsWrapper.Instance().SetDefaultIPType(DefaultIPType.LegacyIP);

                    TraceFactory.Logger.Info("STEP 2: Stopping DHCP server and waiting for lease to expire.");
                    serviceMethod.Channel.StopDhcpServer();
                    Thread.Sleep(TimeSpan.FromMinutes(3));

                    NetworkUtil.RenewLocalMachineIP();
                }
                else
                {
                    TraceFactory.Logger.Info("STEP 1: Place the device on a non link-local network with no BOOTP/DHCP/RARP server.");

                    IPAddress nonPingingIP = NetworkUtil.FetchNextIPAddress(EwsWrapper.Instance().GetSubnetMask(), IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                    // Start pinging to non existent host before cold reset (sending ARP request)
                    Thread pingNonExistentHostThread = new Thread(() => PingNonExistentHost(nonPingingIP.ToString()));
                    pingNonExistentHostThread.Start();

                    if (!ConfigurePrinterWithAutoIP(activityData, out autoIPAddress))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info("STEP 2: Cold Reset the device and let the device acquire Legacy IP.");

                    printer = PrinterFactory.Create(activityData.ProductFamily, autoIPAddress);
                    ColdReset(activityData, printer);

                    // After cold reset enabling WSDiscovery option
                    EwsWrapper.Instance().SetWSDiscovery(true);

                    // Stop pinging once printer cold reset is completed
                    pingNonExistentHostThread.Abort();
                }

                string defaultIPAddress = string.Empty;

                Utility.IsPrinterInDefaultIP(activityData.PrimaryMacAddress, out defaultIPAddress, out printerHostName, out defaultType);

                if (DefaultIPType.LegacyIP != defaultType)
                {
                    TraceFactory.Logger.Info("Printer failed to acquire Legacy IP Address. The current printer IP Address is {0}.".FormatWith(defaultIPAddress));
                    return false;
                }

                TraceFactory.Logger.Info("STEP 3: Verify that default type = legacy default IP. ");

                EwsWrapper.Instance().ChangeDeviceAddress(printerHostName);

                if (EwsWrapper.Instance().GetDefaultIPType() != DefaultIPType.LegacyIP)
                {
                    TraceFactory.Logger.Info("Validation for default type failed through Web UI.");
                    return false;
                }

                TraceFactory.Logger.Info("STEP 4: Power Cycle the printer.");

                printer = PrinterFactory.Create(activityData.ProductFamily, linkLocalAddress.ToString());
                printer.PowerCycle();

                if (EwsWrapper.Instance().GetDefaultIPType() != DefaultIPType.LegacyIP)
                {
                    TraceFactory.Logger.Info("Printer Default IP Type is not configured as Legacy Default IP after power cycle.");
                    return false;
                }

                TraceFactory.Logger.Info("STEP 5: Via Web UI, change the default type to Auto IP.");

                if (!EwsWrapper.Instance().SetDefaultIPType(DefaultIPType.AutoIP))
                {
                    TraceFactory.Logger.Info("Failed to set default type to Auto IP.");
                    return false;
                }

                TraceFactory.Logger.Info("STEP 6: Power cycle the device.");
                printer.PowerCycle();

                TraceFactory.Logger.Info("STEP 7: Ensure that the device acquires AutoIP.");
                if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out autoIPAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info("STEP 8: Change the default  type = legacy default.");

                EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(autoIPAddress));

                if (!EwsWrapper.Instance().SetDefaultIPType(DefaultIPType.LegacyIP))
                {
                    return false;
                }

                printer.PowerCycle();

                if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out autoIPAddress))
                {
                    return false;
                }

                if (EwsWrapper.Instance().GetDefaultIPType() != DefaultIPType.LegacyIP)
                {
                    TraceFactory.Logger.Info("Printer Default IP Type is not configured as Legacy Default IP after power cycle when the printer is auto ip configured.");
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetDefaultIPType(DefaultIPType.AutoIP);
                AutoIPPostRequisites(activityData);
            }
        }

        #endregion

        #region VerifySendDhcpRequestWithAutoIp(enable and disable)

        /// <summary>
        /// 96195
        /// Verify that Device is DHCP configured when "send DHCP Request when Auto IP configured" is set to TRUE and a DHCP server is added to a link-local network after 30
        /// 1.Place the device on a link-local only network with no BOOTP/DHCP/RARP servers.
        /// 2.Wired: Cold reset the Device.
        /// 3.WirelessInterface: Cold reset the Device and set to infrastructure mode and configure the proper SSID.
        /// 4.Verify that the Device config type is Auto IP and that ""send DHCP Request when Auto IP configured"" is TRUE.
        /// 5.Wait a minimum of 30 mins .
        /// 6.Start the DHCP server  service to the link-local network and wait an additional 10 mins        
        /// Expected:
        /// 1. Device should configured with AutoIP and in the backend DHCP discover packet should be sent continuously until the device configured with DHCP IP.
        /// 2. The DHCP server service should be started successfully.
        /// 3. The device should be configured by DHCP IP even after 30mins when the server comes up.
        /// 96196
        /// 1.Place the Device on a link-local only network with no BOOTP/DHCP/RARP servers.
        /// 2.Wired: Cold reset the Device.
        /// 3.WirelessInterface: Cold reset the Device and set to infrastructure mode and configured with proper SSID.
        /// 4.Verify that the Device config type is Auto IP and that "send DHCP Request when Auto IP configured" is TRUE.
        /// 5. Open the EWS page using Auto ip, uncheck the option "send DHCP Request when Auto IP configured" to set as FALSE.
        /// 6. Start the DHCP server  service to the link-local network and wait 10(Note: Valid for VEP only)
        /// Expected:
        /// 4. Device should be configured with AutoIP.
        /// 6. The Device should NOT be configured by DHCP  IP, but should remain with the same  Auto IP configured address.
        /// In the backend capture, device should not send any DHCP discover packet and should not configured with DHCP IP even though the DHCP server is UP. 
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool VerifySendDhcpRequestWithAutoIP(IPConfigurationActivityData activityData, int testNo, bool enableSendDhcpRequest = true)
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

            string currentDeviceAddress = activityData.PrimaryWiredIPv4Address;
            string hostName = string.Empty;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!ConfigurePrinterWithAutoIP(activityData, out currentDeviceAddress))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, currentDeviceAddress))
                {
                    return false;
                }

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, currentDeviceAddress);
                hostName = printer.HostName;

                ColdReset(activityData, printer);

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);

                // Sometimes printer acquires different Auto IP address
                if (!ValidateDefaultIP(activityData))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().GetDefaultIPType().Equals(DefaultIPType.AutoIP))
                {
                    if (!EwsWrapper.Instance().SetDefaultIPType(DefaultIPType.AutoIP))
                    {
                        return false;
                    }
                }

                EwsWrapper.Instance().SetSendDHCPRequestOnAutoIP(enableSendDhcpRequest);

                if (enableSendDhcpRequest)
                {
                    // Test 96195
                    TraceFactory.Logger.Info("Waiting for 30 minutes.");
                    Thread.Sleep(TimeSpan.FromMinutes(30));
                }

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.StartDhcpServer();

                TraceFactory.Logger.Info("Waiting for 10 minutes.");
                Thread.Sleep(TimeSpan.FromMinutes(10));

                // If send DHCP request is true printer acquires DHCP IP, else Auto IP.
                if (enableSendDhcpRequest)
                {
                    NetworkUtil.RenewLocalMachineIP();

                    if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP))
                    {
                        return false;
                    }
                }
                else
                {
                    // Test 96196
                    if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out currentDeviceAddress))
                    {
                        return false;
                    }

                    printer = PrinterFactory.Create(activityData.ProductFamily, currentDeviceAddress);
                    hostName = printer.HostName;

                    TraceFactory.Logger.Info("Validation for AutoIP is performed only using Discovery mechanism.");
                }
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                if (!enableSendDhcpRequest)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(hostName);
                    EwsWrapper.Instance().SetSendDHCPRequestOnAutoIP(true);
                }

                AutoIPPostRequisites(activityData);
            }

            return true;
        }

        #endregion

        /// <summary>
        /// Template id: 682046
        /// 1. Configure the DHCPv4 scope with Hostname, Domain Name, DNS IP,  DNS Suffix and WINS Server IP using DHCP Server with minimum lease period as 2 mins
        /// 2. Configure the DHCPv6 scope with DNSv6 IP, Domain Search list with valid and preferred lease time as 2 and 4 mins
        /// 2. Bring up the Device with DHCP IP.
        /// 3. Configure the precedence table with DHCPBootP/DHCPv6/Manual/TFTP/Default.
        /// 4. Verify the Hostname behavior on the device
        /// 5. Switch OFF the DHCPv4 and DHCPv6 
        /// 6. Wait for 10 mins
        /// 7. Verify the device behavior.
        /// Expected:
        /// 4. Hostname, Domain name, DNS IP and WINS server are configured from the DHCPv4 server on the device
        /// DNSv6 IP and Domain search list are configured from the DHCPv6 server on the device
        /// 7. Device should go to AutoIP and acquired the Hostname as default and remaining parameters  should be cleared on the Network identification Tab.
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool VerifyAutoIPandDefaultParameters(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info("Assumption: Dns IP is configured for DHCPv6 scope: {0}.".FormatWith(activityData.DHCPScopeIPv6Address));

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                serviceMethod.Channel.SetDomainSearchList(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, "v6domain.com");
                serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 60);
                serviceMethod.Channel.SetPreferredLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, 120);
                serviceMethod.Channel.SetValidLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, 240);

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                TraceFactory.Logger.Info("Setting DHCPv4/BootP as highest config precedence.");
                SnmpWrapper.Instance().SetConfigPrecedence("2:0:1:3:4");
                Thread.Sleep(TimeSpan.FromMinutes(1));

                EwsWrapper.Instance().SetDHCPv6OnStartup(true);
                EwsWrapper.Instance().SetIPv6(false);
                EwsWrapper.Instance().SetIPv6(true);

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                if (!ValidateHostName(family, activityData.ServerHostName))
                {
                    return false;
                }

                if (!ValidateDomainName(family, activityData.DomainName))
                {
                    return false;
                }

                if (!ValidatePrimaryDnsServer(family, activityData.ServerDNSPrimaryIPAddress))
                {
                    return false;
                }

                if (!ValidatePrimaryWinsServer(family, serviceMethod.Channel.GetPrimaryWinsServer(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress)))
                {
                    return false;
                }

                if (!ValidatePrimaryDnsV6Server(family, serviceMethod.Channel.GetPrimaryDnsv6Server(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address)))
                {
                    return false;
                }

                if (!PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                {
                    if (!ValidateDomainSearchList(family, serviceMethod.Channel.GetDnsSuffix(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress)))
                    {
                        return false;
                    }
                }

                serviceMethod.Channel.StopDhcpServer();
                Thread.Sleep(TimeSpan.FromMinutes(10));

                string autoIPaddress = string.Empty;

                if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out autoIPaddress))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire auto IP address.");
                    return false;
                }

                NetworkUtil.RenewLocalMachineIP();

                EwsWrapper.Instance().ChangeDeviceAddress(autoIPaddress);
                SnmpWrapper.Instance().Create(autoIPaddress);
                TelnetWrapper.Instance().Create(autoIPaddress);

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, autoIPaddress);
                printer.PowerCycle();

                return ValidateDefaultParameter();
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address), validate: false);

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                serviceMethod.Channel.StartDhcpServer();
                serviceMethod.Channel.DeleteDomainSearchList(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address);
                serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, DEFAULT_LEASE_TIME);
                serviceMethod.Channel.SetValidLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, DEFAULT_VALID_LEASE_TIME);
                serviceMethod.Channel.SetPreferredLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, DEFAULT_PREFERRED_LEASE_TIME);

                Thread.Sleep(TimeSpan.FromMinutes(6));

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
                SnmpWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);
                TelnetWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);

                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
            }
        }

        #endregion

        #region ARP Ping

        /// <summary>
        /// Template ID: 96131
        /// 1. Put  the device on an isolated link-local network without BOOTP/DHCP/RARP servers.
        /// 2. Cold reset the device.
        /// 3. While the device is initializing, send one ARP request (can do this manually by sending  ping -t to a non-existent host).
        /// 4. After the DUT attains legacy ip ,bring up the  DHCP server 
        /// Note : Valid only for VEP products
        /// Expected:
        /// 1. The device should sense the network and go to the default  Legacy IP address of 192.0.0.192 every time.
        /// 4. The device should get a DHCP IP.
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool VerifyDefaultIPWithARP(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.StopDhcpServer();

                // Start pinging to non existent host before coldreset (sending ARP request)
                IPAddress nonPingingIP = NetworkUtil.FetchNextIPAddress(EwsWrapper.Instance().GetSubnetMask(), IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                // Start pinging to non existent host before cold reset (sending ARP request)
                Thread pingNonExistentHostThread = new Thread(() => PingNonExistentHost(nonPingingIP.ToString()));
                pingNonExistentHostThread.Start();

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
                ColdReset(activityData, printer);

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);

                // Stop pinging once printer coldreset is completed
                pingNonExistentHostThread.Abort();

                if (Printer.Printer.LegacyIPAddress.ToString() != Utility.GetPrinterIPAddress(activityData.PrimaryMacAddress))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire Legacy IP Address: {0}".FormatWith(Printer.Printer.LegacyIPAddress));
                    TraceFactory.Logger.Info("Discovering Again...");

                    if (Printer.Printer.LegacyIPAddress.ToString() != Utility.GetPrinterIPAddress(activityData.PrimaryMacAddress))
                    {
                        TraceFactory.Logger.Info("Printer didn't acquire Legacy IP Address: {0}".FormatWith(Printer.Printer.LegacyIPAddress));
                        return false;
                    }
                }

                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.StartDhcpServer();
                Thread.Sleep(TimeSpan.FromMinutes(LEASE_WAIT_TIME));

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP))
                {
                    return false;
                }
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                AutoIPPostRequisites(activityData);
            }

            return true;
        }

        /// <summary>
        /// Template ID : 96132
        /// Procedure For VEP :
        /// 1. Put the device on an isolated link-local network without BOOTP/DHCP/RARP servers.
        /// 2. Cold reset the device.
        /// 3. While the device is initializing, send one ARP request (can do this manually by sending  ping -t to a non-existent host).
        /// 3. When legacy IP (192.0.0.192) is configured with no servers present  using carrier sense
        /// 3. Add a static entry in the arp table using the command arp -s <ip address> <mac address>        
        /// 4. Ping the IP address.
        /// 5. Power on the device
        /// 
        /// Procedure for TPS :
        /// 1. Connect printer in the network with no Bootp/DHCP server available.
        /// or Configure printer to get configured with AUTO IP by selecting the configuration method as Auto IP.
        /// 2. Add an entry in the ARP table for the device.
        /// arp -s <ip address> <mac address>
        /// Ping <IP address> -t
        /// 3. Observe whether printer reply's to the Ping request.
        /// 
        /// Expected :
        /// 1. The device should get configured with the IP address added in the arp table.
        /// 2. After power cycling should retain the same ip address.
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool VerifyARPIPAddress(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            EwsWrapper.Instance().SetArp(true);
            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
            string linkLocalAddress = printer.IPv6LinkLocalAddress.ToString();

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                IPAddress arpIPAddress = NetworkUtil.FetchNextIPAddress(EwsWrapper.Instance().GetSubnetMask(), IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                if (PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                {
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.AUTOIP, validate: false);
                }
                else
                {
                    DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                    serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 60);

                    // This is done to get latest configuration from DHCP server
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                    EwsWrapper.Instance().SetDefaultIPType(DefaultIPType.LegacyIP);

                    serviceMethod.Channel.StopDhcpServer();
                    Thread.Sleep(TimeSpan.FromMinutes(1));

                    DefaultIPType defaultType;
                    string ipAddress, hostName;

                    Utility.IsPrinterInDefaultIP(activityData.PrimaryMacAddress, out ipAddress, out hostName, out defaultType);

                    if (DefaultIPType.LegacyIP != defaultType)
                    {
                        TraceFactory.Logger.Info("Printer didn't acquire Legacy IP.");
                        TraceFactory.Logger.Debug("Printer IP Address: {0}".FormatWith(ipAddress));
                        return false;
                    }

                    TraceFactory.Logger.Info("Printer is configured with Legacy IP Address");
                }

                NetworkUtil.AddArpEntry(arpIPAddress, activityData.PrimaryMacAddress, GetClientNetworkName(activityData.PrimaryDhcpServerIPAddress));

                if (NetworkUtil.PingUntilTimeout(arpIPAddress, TimeSpan.FromMinutes(10)))
                {
                    TraceFactory.Logger.Info("Printer acquired static ARP IP Address : {0} as expected.".FormatWith(arpIPAddress));
                }
                else
                {
                    TraceFactory.Logger.Info("Static ARP IP Address : {0} is not pinging after adding the entry to ARP table.".FormatWith(arpIPAddress));
                    return false;
                }

                printer = PrinterFactory.Create(activityData.ProductFamily, arpIPAddress.ToString());
                printer.PowerCycle();

                TraceFactory.Logger.Info("After Powercycle:");

                if (NetworkUtil.PingUntilTimeout(arpIPAddress, TimeSpan.FromMinutes(1)))
                {
                    TraceFactory.Logger.Info("Printer acquired static ARP IP Address : {0} as expected.".FormatWith(arpIPAddress));
                }
                else
                {
                    TraceFactory.Logger.Info("Static ARP IP Address : {0} is not pinging".FormatWith(arpIPAddress));
                    return false;
                }

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                NetworkUtil.DeleteArpEntries();
                Thread.Sleep(TimeSpan.FromMinutes(3));

                CheckForPrinterAvailability(linkLocalAddress);
                EwsWrapper.Instance().ChangeDeviceAddress(linkLocalAddress);
                AutoIPPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template ID : 654414
        /// 1. Bring up the device with Auto IP 
        /// 2. Follow the below steps to add the static entry in the arp table: arp -s <ip address> <mac address>        
        /// 3. Ping the IP address.
        /// Expected :
        /// 1. The device should  retain the previously attained Auto IP and should not get configured with the static ARP IP address.
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool VerifyAutoIPWithARP(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            EwsWrapper.Instance().SetArp(true);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                IPAddress subnetMask = EwsWrapper.Instance().GetSubnetMask();
                // Configure Printer with Auto IP
                // Client should not goto Auto IP since we need to add static entry
                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 60);

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                IPAddress arpIPAddress = NetworkUtil.FetchNextIPAddress(subnetMask, IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                NetworkUtil.AddArpEntry(arpIPAddress, activityData.PrimaryMacAddress, GetClientNetworkName(activityData.PrimaryDhcpServerIPAddress));

                serviceMethod.Channel.StopDhcpServer();
                Thread.Sleep(TimeSpan.FromMinutes(LEASE_WAIT_TIME));

                string autoIPAddress = string.Empty;

                if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out autoIPAddress))
                {
                    // Printer might not get discovered during first attempt, discovering again
                    TraceFactory.Logger.Info("Discovering again...");
                    if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out autoIPAddress))
                    {
                        return false;
                    }
                }

                bool isTps = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily);

                IPAddress expectedAddress = isTps ? arpIPAddress : IPAddress.Parse(autoIPAddress);

                /* Printer is in Auto Ip Address and a static non pinging dhcp ip address is added to Arp table. Ping to Arp ip address
				 * VEP: printer should retain autp ip address
				 * TPS: printer should acquire arp ip address
				 */

                bool pingStatus = NetworkUtil.PingUntilTimeout(arpIPAddress, TimeSpan.FromMinutes(3));

                if (isTps)
                {
                    if (pingStatus)
                    {
                        TraceFactory.Logger.Info("Printer acquired static ARP IP Address : {0} as expected.".FormatWith(arpIPAddress));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Printer didn't acquire static ARP IP Address : {0}".FormatWith(arpIPAddress));
                        return false;
                    }
                }
                else
                {
                    if (pingStatus)
                    {
                        TraceFactory.Logger.Info("Printer acquired static ARP IP Address : {0}".FormatWith(arpIPAddress));
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Static ARP IP Address : {0} is not pinging as expected.".FormatWith(arpIPAddress));
                    }

                    // To access printer configured with Auto IP, client also needs to be in Auto IP
                    // Since DHCP server is already down, renewing IPConfig for client machine will get client configured with Auto IP
                    NetworkUtil.RenewLocalMachineIP();

                    if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(autoIPAddress), TimeSpan.FromMinutes(1)))
                    {
                        TraceFactory.Logger.Info("Printer Auto IP address: {0} is not pinging.".FormatWith(autoIPAddress));
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Printer Auto IP address: {0} is pinging.".FormatWith(autoIPAddress));
                    }
                }

                return true;

            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                NetworkUtil.DeleteArpEntries();
                Thread.Sleep(TimeSpan.FromMinutes(3));
                AutoIPPostRequisites(activityData);
            }

        }

        /// <summary>
        /// Template ID: 654416
        /// 1. Bring up the device with DHCP/Bootp and send Arp ping. (Add a static entry in the ARP table using the command )        
        /// arp -s 192.168.45.39 00-01-E6-a2-31-98
        /// 2. Ping the IP address 
        /// Expected:
        /// 2. The device should retain the DHCP IP and should not attain any ARP configured IP.
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool VerifyDHCPWithARP(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            EwsWrapper.Instance().SetArp(true);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Printer will already be under DHCP(TPS)/ BootP(VEP) Config method

                IPAddress arpIPAddress = NetworkUtil.FetchNextIPAddress(EwsWrapper.Instance().GetSubnetMask(), IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                NetworkUtil.AddArpEntry(arpIPAddress, activityData.PrimaryMacAddress, GetClientNetworkName(activityData.PrimaryDhcpServerIPAddress));
                NetworkUtil.PingUntilTimeout(arpIPAddress, TimeSpan.FromMinutes(1));

                if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.PrimaryWiredIPv4Address), TimeSpan.FromMinutes(1)))
                {
                    TraceFactory.Logger.Info("Printer DHCP IP address: {0} is not pinging.".FormatWith(activityData.PrimaryWiredIPv4Address));
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer DHCP IP address: {0} is pinging.".FormatWith(activityData.PrimaryWiredIPv4Address));
                }
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                NetworkUtil.DeleteArpEntries();
                Thread.Sleep(TimeSpan.FromMinutes(3));
                AutoIPPostRequisites(activityData);
            }

            return true;
        }

        #endregion

        #region DHCP IPv6

        /// <summary>
        /// Template ID:96142		
        /// 1. Configure a valid IPv6 address manually on the device
        /// 2. Try to open EWS page using the configured manual IPV6 address.
        /// 3. Try ftp with the manual IPV6 address.
        /// Expected:
        /// 1. Device Should be able to configure a valid IPv6 address. 
        /// 2. EWS page should open with configured manual IPV6 address.
        /// 3. FTP should happen with manual IPv6 address.
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool VerifyManualIPv6(IPConfigurationActivityData activityData, int testNo)
        {
            // Added as part of code analysis
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string ManualIPv6Address = GetManualIPv6Address(activityData);

                if (!EwsWrapper.Instance().SetManualIPv6Address(true, ManualIPv6Address))
                {
                    return false;
                }

                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(ManualIPv6Address), TimeSpan.FromSeconds(30)))
                {
                    TraceFactory.Logger.Info("Ping succeeded- Manual Printer IPV6 Address:{0}".FormatWith(ManualIPv6Address));
                }
                else
                {
                    TraceFactory.Logger.Info("Ping failed- Manual Printer IPV6 Address:{0}".FormatWith(ManualIPv6Address));
                    return false;
                }

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
                if (!printer.IsEwsAccessible(IPAddress.Parse(ManualIPv6Address)))
                {
                    return false;
                }


                string ftpUri = "{0}{1}/{2}".FormatWith("ftp://", string.Concat("[", ManualIPv6Address, "]"), Utility.CreateFile("temp"));
                return printer.IsFTPAccessible(ftpUri);
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                TraceFactory.Logger.Info("Disabling the Manual IPV6 address of the Printer");
                EwsWrapper.Instance().SetManualIPv6Address(false);
            }
        }

        /// <summary>
        /// Template ID:96186
        ///Step:Verify manual IPv6 address on disable and enable of IPv6
        ///1. Enable IPv6 on the device and configure a valid manual IPv6 address.
        ///2. Disable IPv6.
        ///3. Enable IPv6
        ///Expected:Manual IPv6 should be present
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool VerifyManualIPv6Status(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetIPv6(true))
                {
                    return false;
                }

                string ManualIPv6Address = GetManualIPv6Address(activityData);

                // Enable IPv6 on the device
                if (!EwsWrapper.Instance().SetManualIPv6Address(true, ManualIPv6Address))
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

                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(ManualIPv6Address), TimeSpan.FromMinutes(1)))
                {
                    TraceFactory.Logger.Info("Ping succeeded- Printer IP Address: {0}".FormatWith(ManualIPv6Address));
                }
                else
                {
                    TraceFactory.Logger.Info("Ping failed- Printer IP Address: {0}".FormatWith(ManualIPv6Address));
                    return false;
                }

                if (EwsWrapper.Instance().ValidateManualIpv6Address(ManualIPv6Address))
                {
                    TraceFactory.Logger.Info("EWS validation is successful. Manual IPv6: {0} is configured on the printer".FormatWith(ManualIPv6Address));
                }
                else
                {
                    TraceFactory.Logger.Info("EWS validation failed. Manual IPv6: {0} is not configured on the printer".FormatWith(ManualIPv6Address));
                    return false;
                }

                if (SnmpWrapper.Instance().GetManualIpv6Address().EqualsIgnoreCase(ManualIPv6Address))
                {
                    TraceFactory.Logger.Info("SNMP Validation successful. Manual IPv6: {0} is configured on the printer".FormatWith(ManualIPv6Address));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("SNMP Validation failed. Manual IPv6: {0} is not configured on the printer".FormatWith(ManualIPv6Address));
                    return false;
                }
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetManualIPv6Address(false);
            }
        }

        /// <summary>
        /// Template ID: 654999
        /// Template Verify that device behavior when stateless IPv6 address option is enabled or disabled			
        /// Verify the device behavior using stateless IPv6 address while the stateless address option is disabled or enabled.	
        /// 1.Bring up the device in network having router providing stateless-address.  
        /// 2.Disable the stateless address checkbox in TCP/IP Settings-> TCP/IPv6.
        /// 3.Enable the stateless address checkbox
        /// Note: After every action  related to IPV6 such as enabling/disabling stateless addresses, changing the ipv6 options using radio buttons. 
        /// IPv6 should be disabled and enabled for the action to take effect immediately
        /// Expected:
        /// 1. The device should take four stateless addresses and one link-local address.
        /// 2. While the stateless address checkbox is disabled, the  device should have a link-local IP,one stateful IP and should not take any stateless address.
        /// 3. The device should take four stateless addresses and one link-local address as it had done previously and the previous  stateful DHCPv6 address  should not display.
        /// </summary>
        /// <returns>True for success, else false</returns>
        public static bool VerifyIPv6StatelessOption(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info("Assumption: Printer is connected to DHCP server via router which provides finite valid and preferred lease time.");

                EwsWrapper.Instance().SetIPv6(true);
                EwsWrapper.Instance().SetStatelessAddress(true);

                // VEP acquires 4 stateless addresses from router and TPS acquires 3
                int statelessAddressCount = PrinterFamilies.TPS.ToString() == activityData.ProductFamily ? 3 : 4;

                Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(statelessAddressCount + 2);

                Collection<IPAddress> statelessIPAddress = IPv6Utils.GetStatelessIPAddress(ipv6Details);
                IPAddress linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);

                if (statelessAddressCount != statelessIPAddress.Count)
                {
                    TraceFactory.Logger.Info("Printer didn't acquire {0} stateless IP addresses.".FormatWith(statelessAddressCount));
                    TraceFactory.Logger.Debug("Stateless address count: {0}".FormatWith(statelessIPAddress.Count));
                    return false;
                }

                if (IPAddress.IPv6None.Equals(linkLocalAddress))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire linklocal IP Address.");
                    return false;
                }

                TraceFactory.Logger.Info("Printer acquired {0} stateless addresses and a linklocal address".FormatWith(statelessAddressCount));

                EwsWrapper.Instance().SetStatelessAddress(false);

                ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(2);

                statelessIPAddress = IPv6Utils.GetStatelessIPAddress(ipv6Details);
                linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);
                IPAddress statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);

                if (0 != statelessIPAddress.Count)
                {
                    TraceFactory.Logger.Info("Printer stateless IP addresses are not cleared.");
                    TraceFactory.Logger.Debug("Stateless address count: {0}".FormatWith(statelessIPAddress.Count));
                    return false;
                }

                if (IPAddress.IPv6None.Equals(linkLocalAddress))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire linklocal IP Address.");
                    return false;
                }

                if (IPAddress.IPv6None.Equals(statefulAddress))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire stateful IP Address.");
                    return false;
                }

                TraceFactory.Logger.Info("Printer acquired a linklocal and stateful address. Stateless addresses are cleared.");

                EwsWrapper.Instance().SetStatelessAddress(true);

                ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(statelessAddressCount + 2);

                statelessIPAddress = IPv6Utils.GetStatelessIPAddress(ipv6Details);
                linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);
                statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);

                if (statelessAddressCount != statelessIPAddress.Count)
                {
                    TraceFactory.Logger.Info("Printer didn't acquire {0} stateless IP addresses.".FormatWith(statelessAddressCount));
                    TraceFactory.Logger.Debug("Stateless address count: {0}".FormatWith(statelessIPAddress.Count));
                    return false;
                }

                if (IPAddress.IPv6None.Equals(linkLocalAddress))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire linklocal IP Address.");
                    return false;
                }

                if (!IPAddress.IPv6None.Equals(statefulAddress))
                {
                    TraceFactory.Logger.Info("Printer stateful IP Address is not cleared.");
                    return false;
                }

                TraceFactory.Logger.Info("Printer acquired {0} stateless addresses and a linklocal address".FormatWith(statelessAddressCount));

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetStatelessAddress(true);
            }
        }

        /// <summary>
        /// Template ID: 655548	
        /// Template Verify lease renewal behavior if the DHCPv6 server goes down
        /// Verify lease renewal behaviour if the DHCPv6 server goes down	
        /// 1. Connect the device in a network with a DHCPv6 server with preferred  lease as 2 mins and valid lease as 4 mins
        /// 2. Bring up the device.
        /// 3. Now make the DHCPv6 server offline and wait for the lease to expire  4.Check the packet trace.
        /// Expected:
        /// 2. Ensure device get configured a DHCPv6 address with proper preferred and valid lease time.
        /// 4. Ensure that the device sends a rebind packet also after (approx )1min 30 secs.
        /// After expiry of preferred and Valid lease, the summary page(or Config page) should not show the DHCPv6 address which it had.
        /// when the server not was available. But it should  have the link local IP in the summary page. ie the DUT should clean the 
        /// DHCPv6 address and all the lease time information such as valid and preferred lease time also Device should start sending solicit message after the lease expired.
        /// </summary>
        /// <returns></returns>
        public static bool VerifyIPv6LeaseRenewal(IPConfigurationActivityData activityData, int testNo)
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

            bool result = false;
            PacketDetails validatePacketDetails = null;
            DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                serviceMethod.Channel.SetPreferredLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, (int)PREFERRED_LIFETIME.TotalSeconds);
                serviceMethod.Channel.SetValidLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, (int)VALID_LIFETIME.TotalSeconds);

                if (!PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily))
                {
                    EwsWrapper.Instance().SetStatelessAddress(true);
                }

                // Enabling and Disabling IPv6 option to get latest configuration from DHCPv6 server
                EwsWrapper.Instance().SetDHCPv6OnStartup(true);
                EwsWrapper.Instance().SetIPv6(false);
                EwsWrapper.Instance().SetIPv6(true);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails();

                IPAddress statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
                TimeSpan currentLifeTime = IPv6Utils.GetPreferredLifetime(ipv6Details, statefulAddress);

                if (PREFERRED_LIFETIME < currentLifeTime)
                {
                    TraceFactory.Logger.Info("Printer didn't get configured with server preferred lifetime.");
                    return false;
                }

                TraceFactory.Logger.Info("Printer is configured with server preferred lifetime. Current preferred lifetime: {0} seconds.".FormatWith(currentLifeTime.TotalSeconds));

                currentLifeTime = IPv6Utils.GetValidLifetime(ipv6Details, statefulAddress);

                if (VALID_LIFETIME < currentLifeTime)
                {
                    TraceFactory.Logger.Info("Printer didn't get configured with server valid lifetime.");
                    return false;
                }

                TraceFactory.Logger.Info("Printer is configured with server valid lifetime. Current valid lifetime: {0} seconds.".FormatWith(currentLifeTime.TotalSeconds));

                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.StopDhcpServer();

                TraceFactory.Logger.Info("Waiting for {0} seconds for life time to expire.".FormatWith(VALID_LIFETIME.TotalSeconds));
                Thread.Sleep(VALID_LIFETIME);

                ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails();
                statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
                IPAddress linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);

                if (!IPAddress.IPv6None.Equals(statefulAddress))
                {
                    TraceFactory.Logger.Info("Printer stateful address didn't clear");
                    return false;
                }

                TraceFactory.Logger.Info("Stateful address and lifetime are cleared from IPv6 table.");

                if (IPAddress.IPv6None.Equals(linkLocalAddress))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire linklocal address.");
                    return false;
                }

                TraceFactory.Logger.Info("Printer is configured with linklocal address.");

                result = true;
            }
            finally
            {
                validatePacketDetails = client.Channel.Stop(guid);

                if (result)
                {
                    result = ValidateDhcpv6RebindPacket(activityData.PrimaryDhcpServerIPAddress, validatePacketDetails.PacketsLocation, activityData.PrimaryMacAddress);
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.StartDhcpServer();
                serviceMethod.Channel.SetValidLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, DEFAULT_VALID_LEASE_TIME);
                serviceMethod.Channel.SetPreferredLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, DEFAULT_PREFERRED_LEASE_TIME);
            }

            return result;
        }

        /// <summary>
        /// Template Id: 681963
        /// Step 1:
        /// 1. Connect the device in a network where router and DHCPv6 server are  available. (Test network)
        /// 2. In webui ,Change the DHCPv6 policy to 'Perform DHCPv6 when stateless configuration is unsuccessful or disabled' in the IPv6 tab.
        /// 3. Disable the stateless checkbox.
        /// 4. Verify the Device behavior.
        /// Note :For any changes to take effect, disable and enable the IPV6 checkbox
        /// Step 2:
        /// 1. Connect the device in a network where router and DHCPv6 server are  available. (Test network)
        /// 2.In Web UI, change the DHCPv6 policy to 'Perform DHCPv6 when stateless configuration is unsuccessful or disabled' in the IPv6 Tab.
        /// 4.Enable  the stateless checkbox in the IPv6 Tab
        /// Note : For any changes to take effect,disable and enable  the IPv6 checkbox to perform the above action.
        /// Expected: 
        /// Step 1:
        /// 1. The device would acquire a stateful IP address from the DHCPv6 server.
        /// Step 2:
        /// 1. The device should not acquire a stateful IP. Device should be acquired stateless IPv6 addresses
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool VerifyStatelessOptionConfiguration(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                EwsWrapper.Instance().SetIPv6(true);
                EwsWrapper.Instance().SetDHCPv6StatelessConfigurationOption();
                EwsWrapper.Instance().SetStatelessAddress(false);
                EwsWrapper.Instance().SetIPv6(false);
                EwsWrapper.Instance().SetIPv6(true);

                Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(2);
                IPAddress statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);

                if (IPAddress.IPv6None.Equals(statefulAddress))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire stateful address.");
                    return false;
                }

                TraceFactory.Logger.Info("Printer acquired stateful address: {0}".FormatWith(statefulAddress));

                EwsWrapper.Instance().SetStatelessAddress(true);
                EwsWrapper.Instance().SetIPv6(false);
                EwsWrapper.Instance().SetIPv6(true);

                ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(5);
                statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
                Collection<IPAddress> statelessIPAddress = IPv6Utils.GetStatelessIPAddress(ipv6Details);

                if (4 != statelessIPAddress.Count)
                {
                    TraceFactory.Logger.Info("Stateless Addresses are not acquired.");
                    TraceFactory.Logger.Debug("Stateless Addresses count: {0}".FormatWith(statelessIPAddress.Count));
                    return false;
                }

                TraceFactory.Logger.Info("Printer acquired 4 stateless addresses.");

                if (!IPAddress.IPv6None.Equals(statefulAddress))
                {
                    TraceFactory.Logger.Info("Printer stateless address is not cleared.");
                    TraceFactory.Logger.Debug("Printer acquired stateless address: {0}".FormatWith(statefulAddress));
                    return false;
                }

                TraceFactory.Logger.Info("Printer didn't acquire stateful address as expected.");

                return true;
            }
            finally
            {
                client.Channel.Stop(guid);
            }
        }

        #endregion

        #region Linux

        /// <summary>
        /// Template ID : 96137
        /// 1. Connect the device with  Bootp server along with TFTP configuration available as follows:
        /// Configure a bootp file ( i.e bootptab file)with required IP parameters such as IP address,Subnet Mask,Log server,etc  to bring up the  device.
        /// Configure the TFTP parameters in TFTP config file such as host name,Domain name,Primary WINS,Secondary WINS,Primary DNS,Secondary DNS, etc 
        /// 3. Cold reset the Device.
        /// 4. Verify the device behaviour.
        /// Expected :
        /// 4. Device should get configured with Bootp IP address and  get the TFTP parameters provided from TFTP server configuration file such as hostname, Domain Name, Primary WINS,Secondary  WINS,etc.
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool VerifyBootpTftpConfiguration(IPConfigurationActivityData activityData)
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
                IPAddress linuxPrinterIPAddress = IPAddress.None;

                if (!LinuxPreRequisites(activityData, LinuxServiceType.BOOTP, ref linuxPrinterIPAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string hostName = "TftpLinuxHostname";
                string domainName = "TftpLinuxDomainName.com";
                IPAddress linuxServerIPAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                // Both Bootp and Tftf service should be running for configuration to take effect. (Both service can run parallel)							
                ConfigureTftpFile(linuxServerIPAddress, hostName, domainName);

                if (PrinterFamilies.TPS.Equals(family))
                {
                    LinuxUtils.DeleteTftpConfigLastLine(linuxServerIPAddress);
                }

                LinuxUtils.RestartService(linuxServerIPAddress, LinuxServiceType.BOOTP);

                Printer.Printer printer = PrinterFactory.Create(family, linuxPrinterIPAddress);

                if (PrinterFamilies.TPS.Equals(family))
                {
                    EwsWrapper.Instance().SetTftp(true);
                    printer.PowerCycle();
                }
                else
                {
                    ColdReset(activityData, printer);
                    // After cold reset enabling WSDiscovery option
                    EwsWrapper.Instance().SetWSDiscovery(true);
                }


                if (IPConfigMethod.BOOTP != EwsWrapper.Instance().GetIPConfigMethod())
                {
                    TraceFactory.Logger.Info("Printer is not configured with BootP method.");
                    return false;
                }

                TraceFactory.Logger.Info("Printer is configured with BootP method.");

                return ValidateConfiguredParameters(hostName, domainName, activityData.LinuxServerIPAddress, family);
            }
            finally
            {
                LinuxPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template ID : 96151
        /// Make sure Rarpd package is installed on Linux server
        /// Configure the hostname and IP address of the Device in the host file( /etc/hosts) from the linux server
        /// Configure the MAC address and same hostname which is configured in the host file in the ethers file (/etc/ethers) from the linux server
        /// Start the Rarpd Daemon using "rarpd –a" command from the linux server
        /// Bring up the Device in the Rarpd network
        /// Ping IP Address which is configured on the host file Powercycle the device	
        /// Expected :
        /// 1. Configuration should be successful on the linux server
        /// 2. Device should get IP address from the Rarpd server
        /// 3. AFter powercycle also device should still retain the Rarp IP address.
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool VerifyRarpConfiguration(IPConfigurationActivityData activityData)
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
                IPAddress linuxPrinterIPAddress = IPAddress.None;

                if (!LinuxPreRequisites(activityData, LinuxServiceType.DHCP, ref linuxPrinterIPAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info("Assumption: Rarpd package is installed on Linux server.");

                IPAddress linuxServerAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);

                string hostsFilePath = Path.Combine(Path.GetTempPath(), LinuxUtils.HOSTS_FILE);
                string ethersFilePath = Path.Combine(Path.GetTempPath(), LinuxUtils.ETHERS_FILE);

                // Copy file to temp directory
                string linuxFilePath = string.Concat(LinuxUtils.BACKUP_FOLDER_PATH, LinuxUtils.HOSTS_FILE);
                LinuxUtils.CopyFileFromLinux(linuxServerAddress, linuxFilePath, hostsFilePath);

                linuxFilePath = string.Concat(LinuxUtils.BACKUP_FOLDER_PATH, LinuxUtils.ETHERS_FILE);
                LinuxUtils.CopyFileFromLinux(linuxServerAddress, linuxFilePath, ethersFilePath);

                IPAddress nonPingingIP = NetworkUtil.FetchNextIPAddress(EwsWrapper.Instance().GetSubnetMask(), linuxPrinterIPAddress);

                // Modify hosts file
                Collection<string> keyValues = new Collection<string>();

                keyValues.Add(LinuxUtils.KEY_IPADDRESS);
                keyValues.Add(LinuxUtils.KEY_HOST_NAME);

                Collection<string> configureValue = new Collection<string>();

                configureValue.Add(nonPingingIP.ToString());
                configureValue.Add("LinuxHostName");

                LinuxUtils.ConfigureHostsFile(linuxServerAddress, hostsFilePath, keyValues, configureValue);

                // Modify ethers file
                keyValues.Add(LinuxUtils.KEY_MAC_ADDRESS);
                keyValues.Add(LinuxUtils.KEY_HOST_NAME);

                configureValue.Add(activityData.PrimaryMacAddress);
                configureValue.Add("LinuxHostName");

                LinuxUtils.ConfigureEthersFile(linuxServerAddress, ethersFilePath, keyValues, configureValue);

                // Before starting rarp server, other services should be down
                LinuxUtils.StopService(linuxServerAddress, LinuxServiceType.DHCP);
                LinuxUtils.StartService(linuxServerAddress, LinuxServiceType.RARP);

                if (!NetworkUtil.PingUntilTimeout(nonPingingIP, TimeSpan.FromMinutes(5)))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire IP Address from Rarp configuration.");
                    return false;
                }

                TraceFactory.Logger.Info("Printer acquired: {0} IP Address from Rarp configuration.".FormatWith(nonPingingIP));

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, nonPingingIP.ToString());
                printer.PowerCycle();

                if (!NetworkUtil.PingUntilTimeout(nonPingingIP, TimeSpan.FromMinutes(1)))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire Rarp IP Address after power cycle.");
                    return false;
                }

                TraceFactory.Logger.Info("Printer acquired same Rarp IP Address: {0} after power cycle.".FormatWith(nonPingingIP));
                return true;

            }
            finally
            {
                LinuxPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template ID : 96158
        /// 1. Connect  the Device in a network with  BootP Server with TFTP configuration file.
        /// 2. Do a reset ( power cycle ) on the device.
        /// 3. Cold reset the device .
        /// Expected :
        /// For VEP:
        /// 1. The device should get configured with Bootp IP along with TFTP parameters such as Hostname, domain namea and DNS server IP's.
        /// 2.  After powercycling device should still retain the BootIP along with TFTP parameters.
        /// 3.  After cold-reset also device should still retain the BootIP along with TFTP parameters.
        /// For TPS:
        /// 1. The device should get configured with Bootp IP along with TFTP parameters such as Hostname, domain namea and DNS server IP's.
        /// 2. In the background the device should keep on  sending Bootp Request   and  when the TFTP server comes up  the Device should acquire Bootp IP.
        /// 3. The device should be configured with the TFTP Parameters."
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool VerifyBootpTftpConfigurationWithReset(IPConfigurationActivityData activityData)
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
                IPAddress linuxPrinterIPAddress = IPAddress.None;

                if (!LinuxPreRequisites(activityData, LinuxServiceType.BOOTP, ref linuxPrinterIPAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                string hostName = "TftpLinuxHostname";
                string domainName = "TftpLinuxDomainName.com";
                IPAddress linuxServerIPAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                // Both Bootp and Tftf service should be running for configuration to take effect. (Both service can run parallel)				
                ConfigureTftpFile(linuxServerIPAddress, hostName, domainName);

                if (PrinterFamilies.TPS.Equals(family))
                {
                    LinuxUtils.DeleteTftpConfigLastLine(linuxServerIPAddress);
                }

                LinuxUtils.RestartService(linuxServerIPAddress, LinuxServiceType.BOOTP);

                Printer.Printer printer = PrinterFactory.Create(family, linuxPrinterIPAddress);

                EwsWrapper.Instance().SetTftp(true);
                TraceFactory.Logger.Info("Setting the config Precedence table with TFTP as highest precedence");
                SnmpWrapper.Instance().SetConfigPrecedence("1:0:2:3:4");
                Thread.Sleep(TimeSpan.FromMinutes(1));

                printer.PowerCycle();

                if (IPConfigMethod.BOOTP != EwsWrapper.Instance().GetIPConfigMethod())
                {
                    TraceFactory.Logger.Info("Printer is not configured with Bootp method.");
                    return false;
                }

                if (!ValidateConfiguredParameters(hostName, domainName, activityData.LinuxServerIPAddress, family))
                {
                    return false;
                }

                if (!PrinterFamilies.TPS.Equals(family))
                {
                    ColdReset(activityData, printer);

                    // After cold reset enabling WSDiscovery option
                    EwsWrapper.Instance().SetWSDiscovery(true);

                    if (IPConfigMethod.BOOTP != EwsWrapper.Instance().GetIPConfigMethod())
                    {
                        TraceFactory.Logger.Info("Printer is not configured with Bootp method.");
                        return false;
                    }

                    return ValidateConfiguredParameters(hostName, domainName, activityData.LinuxServerIPAddress, family);
                }

                return true;
            }
            finally
            {
                LinuxPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template ID: 96146
        /// 1. Configure a device via a DHCP server with a short lease.
        /// 2. Power down the device close to the lease expiration time.
        /// 3. Make sure DHCP server does not have more IP address available.
        /// 4. When the lease time has expired, make sure another device is configured with the same IP.
        /// Expected:  
        /// 1. The server will not have any more IP addresses available.
        /// 2. The device will try to renew lease.
        /// 3. DHCP will stop responding to the DHCP renewal request.
        /// 4. The device will try sending DHCP packets.
        /// 5. Since the server is not responding, the device should send DHCP Discover packets and if no server responds then should get Auto IP configured.
        /// </summary>
        /// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool VerifyNonAvailabilityOfIPAddress(IPConfigurationActivityData activityData)
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
                IPAddress linuxPrinterIPAddress = IPAddress.None;

                if (!LinuxPreRequisites(activityData, LinuxServiceType.DHCP, ref linuxPrinterIPAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                IPAddress linuxServerAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);

                // Since leases are cleared in post requisites, release and renew will add any entry to leases file
                NetworkUtil.RenewLocalMachineIP();

                IPAddress clientAddress = NetworkUtil.GetLocalAddress(linuxServerAddress.ToString(), linuxServerAddress.GetSubnetMask().ToString());
                string hostName = "DHCPv4LXHName";
                string domainName = "DHCPv4LXDName.in";

                // Modify dhcp file
                ConfigureDhcpFile(linuxServerAddress, clientAddress.ToString(), clientAddress.ToString(), hostName, domainName);
                LinuxUtils.RestartService(linuxServerAddress, LinuxServiceType.DHCP);

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, linuxPrinterIPAddress.ToString());
                printer.PowerCycle();

                string autoIPAddress = string.Empty;
                if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out autoIPAddress))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire Auto IP.");
                    return false;
                }

                TraceFactory.Logger.Info("Printer acquired Auto IP: {0}".FormatWith(autoIPAddress));
                return true;
            }
            finally
            {
                LinuxPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template ID: 77413
        /// 1. Configure hostname in TFTP server
        /// Cold reset device
        /// Configure precedence table with TFTP as highest precedence 
        /// Verify that the TFTP configured hostname was configured on device
        /// 2.After executing above step  try to edit the option provided by server manually
        /// Expected:
        /// 1. TFTP configured hostname should be configured on device		
        /// 2. Manual entry should not be accepted
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool VerifyTftpHighestConfigPrecedence(IPConfigurationActivityData activityData)
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
                IPAddress linuxPrinterIPAddress = IPAddress.None;

                if (!LinuxPreRequisites(activityData, LinuxServiceType.BOOTP, ref linuxPrinterIPAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string hostName = "TftpLinuxHostname";
                string domainName = "TftpLinuxDomainname.com";
                IPAddress linuxServerIPAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                // Both Bootp and Tftf service should be running for configuration to take effect. (Both service can run parallel)				
                ConfigureTftpFile(linuxServerIPAddress, hostName, domainName);

                if (PrinterFamilies.TPS.Equals(family))
                {
                    LinuxUtils.DeleteTftpConfigLastLine(linuxServerIPAddress);
                }

                LinuxUtils.RestartService(linuxServerIPAddress, LinuxServiceType.BOOTP);

                Printer.Printer printer = PrinterFactory.Create(family, linuxPrinterIPAddress);

                ColdReset(activityData, printer);

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);

                EwsWrapper.Instance().SetTftp(true);

                TraceFactory.Logger.Info("Setting the config Precedence table with TFTP as highest precedence");
                SnmpWrapper.Instance().SetConfigPrecedence("1:0:2:3:4");
                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (!ValidateHostName(family, hostName))
                {
                    return false;
                }

                if (EwsWrapper.Instance().SetHostname("InValid_Hostname"))
                {
                    TraceFactory.Logger.Info("Host name is set which is not expected behavior.");
                    return false;
                }

                TraceFactory.Logger.Info("Setting manual host name failed as expected.");
                return true;
            }
            finally
            {
                LinuxPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template Id: 77411 
        /// 1. Configure hostname on DHCPv6 server( Dippler)
        ///    Bring up the Device with DHCPv6 network.
        ///    Configure precedence table with DHCPv6/Manual/BOOTP/DHCPv4/Default  as precedence 
        ///    Powercycle the device to check behavior
        /// 2. After executing step 1 try to edit the option provided by server manually	
        /// 3. After executing step 1 try to edit the option provided by server using SNMP
        /// Expected:
        /// 1. DHCPv6 configured Hostname  should be configured on device
        /// 2. Manual entries are not accepted
        /// 3. SNMP entries should not be accepted
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool VerifyDHCPv6HighestPrecedence(IPConfigurationActivityData activityData)
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
                IPAddress linuxPrinterIPAddress = IPAddress.None;

                if (!LinuxPreRequisites(activityData, LinuxServiceType.DHCP, ref linuxPrinterIPAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string domainName = "DHCPv6LXDName.in";
                string hostName = "DHCPv6LXHName";
                IPAddress linuxServerIPAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                ConfigureDhcpv6File(linuxServerIPAddress, hostName, domainName, family);
                LinuxUtils.RestartService(linuxServerIPAddress, LinuxServiceType.DHCP);
                LinuxUtils.RestartService(linuxServerIPAddress, LinuxServiceType.DHCPV6);

                EwsWrapper.Instance().SetDHCPv6OnStartup(true);

                // Default Config Precedence : Manual/ TFTP/ DHCP-BootP/ DHCPv6 / Default 

                TraceFactory.Logger.Info("Setting DHCPv6 as highest config precedence");
                SnmpWrapper.Instance().SetConfigPrecedence("3:0:2:1:4");

                Printer.Printer printer = PrinterFactory.Create(family, linuxPrinterIPAddress);
                printer.PowerCycle();

                if (!ValidateHostName(family, hostName))
                {
                    return false;
                }

                if (EwsWrapper.Instance().SetHostname("Manual_Hostname"))
                {
                    return false;
                }

                return !SnmpWrapper.Instance().SetHostName("Manual_Hostname");
            }
            finally
            {
                LinuxPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template Id: 77420
        /// Configure precedence table with DHCPv6 as highest precedence configuration method
        /// Configure a small lease of approximately 1 minute on the DHCPv6 server
        /// Configure a hostname using DHCPv6
        /// Change hostname on the DHCPv6 server
        /// Verify whether the new hostname configured on the DHCPv6 server is configured on JD after lease renew
        /// Expected:
        /// New host name configured on the DHCPv6 server should be configured on JD
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool VerifyDHCPv6Renewal(IPConfigurationActivityData activityData)
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
                IPAddress linuxPrinterIPAddress = IPAddress.None;

                if (!LinuxPreRequisites(activityData, LinuxServiceType.DHCP, ref linuxPrinterIPAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Default Config Precedence : Manual/ TFTP/ DHCP-BootP/ DHCPv6 / Default 

                TraceFactory.Logger.Info("Setting DHCPv6 as highest config precedence");
                SnmpWrapper.Instance().SetConfigPrecedence("3:0:2:1:4");
                Thread.Sleep(TimeSpan.FromMinutes(1));

                // Configure DHCPv6 file
                string domainName = "DHCPv6LXDName.in";
                string hostName = "DHCPv6LXHName";
                IPAddress linuxServerIPAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                ConfigureDhcpv6File(linuxServerIPAddress, hostName, domainName, family);
                LinuxUtils.RestartService(linuxServerIPAddress, LinuxServiceType.DHCP);
                LinuxUtils.RestartService(linuxServerIPAddress, LinuxServiceType.DHCPV6);

                EwsWrapper.Instance().SetDHCPv6OnStartup(true);
                EwsWrapper.Instance().SetIPv6(false);
                EwsWrapper.Instance().SetIPv6(true);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (!ValidateHostName(family, hostName))
                {
                    return false;
                }

                hostName = "NewDHCPv6LXHName";
                ConfigureDhcpv6File(linuxServerIPAddress, hostName, domainName, family);
                LinuxUtils.RestartService(linuxServerIPAddress, LinuxServiceType.DHCP);
                LinuxUtils.RestartService(linuxServerIPAddress, LinuxServiceType.DHCPV6);

                // Wait for lease to expire
                Thread.Sleep(TimeSpan.FromMinutes(4));

                if (!ValidateHostName(family, hostName))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                LinuxPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template Id: 91830
        /// I) 1.Configure hostname on DHCPv4server.
        /// 2. Configure hostname on DHCPv6 server( Dippler)
        /// 3. Configure precedence table with Manual/DHCPv6/BOOTP/DHCPv4  as precedence 
        /// 4. Powercycle the device to check behavior			
        /// II) 1.Configure hostname on DHCPv4server 
        /// 2. Configure hostname on DHCPv6 server (Dippler)
        /// 3. Configure precedence table with Manual/BOOTP/DHCPv4/DHCPv6 as precedence configuration method 
        /// 4. Powercycle the device to check behavior	
        /// Expected:
        /// I) DHCPv6 configured hostname should be configured on device
        /// II) DHCPv4 configured hostname should be configured on device
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool VerifyDHCPv4v6HostName(IPConfigurationActivityData activityData)
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

            string currentDeviceAddress = activityData.PrimaryWiredIPv4Address;
            string scopeAddress = string.Empty;
            IRouter router = null;
            int routerVlanId = 0;

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                // Get the address prefix so that scope address, start range, end range etc. can be determined. prefix = x.x.x.
                string prefix = activityData.LinuxServerIPAddress.Substring(0, activityData.LinuxServerIPAddress.LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase) + 1);
                scopeAddress = string.Concat(prefix, "0");

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, scopeAddress, 60);

                // Scope range is taken as "x.x.x.2 to x.x.x.200"
                if (!serviceMethod.Channel.CreateScope(activityData.PrimaryDhcpServerIPAddress, scopeAddress, "remoteScope", string.Concat(prefix, "2"), string.Concat(prefix, "200")))
                {
                    return false;
                }

                string dhcpv4Hostname = "WindowsDHCPv4Hostname";

                // Set scope parameters
                serviceMethod.Channel.SetHostName(activityData.PrimaryDhcpServerIPAddress, scopeAddress, dhcpv4Hostname);

                string routerAddress = ROUTER_IP_FORMAT.FormatWith(activityData.LinuxServerIPAddress.Substring(0, activityData.LinuxServerIPAddress.LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase)));
                router = RouterFactory.Create(IPAddress.Parse(routerAddress), ROUTER_USERNAME, ROUTER_PASSWORD);

                Dictionary<int, IPAddress> routerVlans = router.GetAvailableVirtualLans();

                routerVlanId = routerVlans.Where(x => (null != x.Value) && (x.Value.IsInSameSubnet(IPAddress.Parse(routerAddress)))).FirstOrDefault().Key;

                if (!router.DeleteHelperAddress(routerVlanId, IPAddress.Parse(activityData.LinuxServerIPAddress)))
                {
                    return false;
                }

                if (!router.ConfigureHelperAddress(routerVlanId, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress)))
                {
                    return false;
                }

                IPAddress linuxServerIPAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);

                string dhcpv6Hostname = "DHCPv6LXHName";
                string domainName = "DHCPv6LXDName.in";

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                ConfigureDhcpv6File(linuxServerIPAddress, dhcpv6Hostname, domainName, family);
                EwsWrapper.Instance().SetDHCPv6OnStartup(true);

                LinuxUtils.StopService(linuxServerIPAddress, LinuxServiceType.DHCP);
                LinuxUtils.RestartService(linuxServerIPAddress, LinuxServiceType.DHCPV6);

                int linuxVirtualLanId = GetVlanNumber(activityData, linuxServerIPAddress.ToString());
                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIP));
                networkSwitch.DisablePort(activityData.PrimaryInterfacePortNo);

                if (!networkSwitch.ChangeVirtualLan(activityData.PrimaryInterfacePortNo, linuxVirtualLanId))
                {
                    return false;
                }

                networkSwitch.EnablePort(activityData.PrimaryInterfacePortNo);

                // Wait for sometime for configuration to take effect
                Thread.Sleep(TimeSpan.FromMinutes(3));

                // Printer with Windows IP address might get discovered first
                string address = Utility.GetPrinterIPAddress(activityData.PrimaryMacAddress);

                if (string.IsNullOrEmpty(address))
                {
                    address = Utility.GetPrinterIPAddress(activityData.PrimaryMacAddress);
                    if (string.IsNullOrEmpty(address))
                    {
                        TraceFactory.Logger.Info("Unable to discover printer...");
                        return false;
                    }
                }

                EwsWrapper.Instance().ChangeDeviceAddress(address);
                SnmpWrapper.Instance().Create(address);
                TelnetWrapper.Instance().Create(address);

                TraceFactory.Logger.Info("Setting config precedence to: Manual/DHCPv6/BOOTP/DHCPv4");
                SnmpWrapper.Instance().SetConfigPrecedence("0:3:2:1:4");
                Thread.Sleep(TimeSpan.FromMinutes(1));

                IPAddress linuxPrinterIPAddress = IPAddress.Parse(address);

                Printer.Printer printer = PrinterFactory.Create(family, linuxPrinterIPAddress);
                printer.PowerCycle();

                if (!ValidateHostName(family, dhcpv6Hostname))
                {
                    return false;
                }

                Thread.Sleep(TimeSpan.FromMinutes(3));

                TraceFactory.Logger.Info("Setting config precedence to:  Manual/BOOTP/DHCPv4/DHCPv6");
                SnmpWrapper.Instance().SetConfigPrecedence("0:2:3:1:4");
                Thread.Sleep(TimeSpan.FromMinutes(1));

                printer.PowerCycle();

                return ValidateHostName(family, dhcpv4Hostname);
            }
            finally
            {
                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, scopeAddress, DEFAULT_LEASE_TIME);

                if (null != router)
                {
                    router.DeleteHelperAddress(routerVlanId, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress));
                    router.ConfigureHelperAddress(routerVlanId, IPAddress.Parse(activityData.LinuxServerIPAddress));
                }

                if (!string.IsNullOrEmpty(scopeAddress))
                {
                    serviceMethod.Channel.DeleteScope(activityData.PrimaryDhcpServerIPAddress, scopeAddress);
                }

                LinuxPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template Id: 77423
        /// 1.Configure the hostname on the DHCPv6 server.  
        /// 2.Configure the config precedence table  with DHCPv6 as the highest precedence.
        /// 3.Click on 'Apply' button on the config precedence page. 
        /// 4.Power cycle the device and verify the behaviour using SNMP and EWS.
        /// Expected:
        /// The config precedence table is  to  retain DHCPv6  as the highest precedence after power cycle and the device should acquire new hostname from the  DHCPv6 server in both EWS and SNMP.
        /// </summary>
        public static bool VerifyPrecedenceAfterPowerCycle(IPConfigurationActivityData activityData)
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
                IPAddress linuxPrinterIPAddress = IPAddress.None;

                if (!LinuxPreRequisites(activityData, LinuxServiceType.DHCP, ref linuxPrinterIPAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string hostName = "DHCPv6LXHName";
                IPAddress linuxServerAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                ConfigureDhcpv6File(linuxServerAddress, hostName, "DHCPv6LXDName.in", family);
                LinuxUtils.RestartService(linuxServerAddress, LinuxServiceType.DHCPV6);
                EwsWrapper.Instance().SetDHCPv6OnStartup(true);

                TraceFactory.Logger.Info("Setting DHCPv6 as highest config precedence.");
                SnmpWrapper.Instance().SetConfigPrecedence("3:2:1:0:4");

                Printer.Printer printer = PrinterFactory.Create(family, linuxPrinterIPAddress);

                printer.PowerCycle();

                if (!EwsWrapper.Instance().GetConfigPrecedence().EqualsIgnoreCase("DHCPv6"))
                {
                    TraceFactory.Logger.Info("DHCPv6 was not set as highest config precedence.");
                    return false;
                }

                TraceFactory.Logger.Info("DHCPv6 is set as highest config precedence.");
                return ValidateHostName(family, hostName);
            }
            finally
            {
                LinuxPostRequisites(activityData);
                EwsWrapper.Instance().ResetConfigPrecedence();
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        /// <summary>
        /// Template Id: 77412
        /// Step 1:
        /// 1. Bring up the device without providing hostname from the any Servers.
        /// 2. Verify the device behavior 
        /// 3. Configure precedence table with      Manual/TFTP/BOOTP/DHCPv4/DHCPv6/Default  as precedence
        /// 4. Configure the hostname manually on device using EWS
        /// Step 2:
        /// 1. Configure the Hostname, Domain name and DNS IP on the DHCPv4 server and DNSv6  IP on the DHCPv6 server
        /// 2. Cold reset device and Once device comes up.
        /// 3. Try to edit option like hostname, Domain name and DNSv4 IP  and DNSv6 IP provided by server on Network Identification Tab.
        /// This behavior to happen only on cold reset
        /// Step 3:
        /// 1.After executing step 1 try to edit the option provided by server using telnet
        /// Step 4:
        /// 1.After executing step 1 try to edit the option provided by server using SNMP
        /// Step 5:
        /// 1.After executing step 1 try to edit the option provided by server manually
        /// Expected:
        /// Step 1:
        /// 2. Default hostname should be configured on the Device.
        /// 4. Manually configured hostname should be accepted by the device.
        /// Step 2:
        /// Option provided by server to be overwritten by manual entries.
        /// Step 3:
        /// Parameters should accepted using telnet options
        /// Step 4:
        /// SNMP entries should be accepted
        /// Step 5:
        /// Manual entries should be accepted
        /// </summary>
        public static bool VerifyManualHighestPrecedence(IPConfigurationActivityData activityData)
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
                IPAddress linuxPrinterIPAddress = IPAddress.None;

                if (!LinuxPreRequisites(activityData, LinuxServiceType.DHCP, ref linuxPrinterIPAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().GetHostname().StartsWith("NPI", StringComparison.CurrentCultureIgnoreCase))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire default host name.");
                    return false;
                }

                TraceFactory.Logger.Info("Printer acquired default host name.");

                EwsWrapper.Instance().ResetConfigPrecedence();

                string manualHostname = "Manual_Hostname";
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                bool isTps = PrinterFamilies.TPS.Equals(family) ? true : false;

                EwsWrapper.Instance().SetHostname(manualHostname);

                if (!ValidateHostName(family, manualHostname))
                {
                    return false;
                }

                IPAddress linuxServerIPAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);

                int length = linuxServerIPAddress.ToString().Length;
                int lastIndex = linuxServerIPAddress.ToString().LastIndexOf('.') + 1;

                string fromRange = string.Format(CultureInfo.CurrentCulture, "{0}{1}", linuxServerIPAddress.ToString().Remove(lastIndex, length - lastIndex), "2");
                string toRange = string.Format(CultureInfo.CurrentCulture, "{0}{1}", linuxServerIPAddress.ToString().Remove(lastIndex, length - lastIndex), "200");

                manualHostname = "DHCPv4LXHName";
                string domainName = "DHCPv4LXDName.in";

                ConfigureDhcpFile(linuxServerIPAddress, fromRange, toRange, manualHostname, domainName);
                ConfigureDhcpv6File(linuxServerIPAddress, manualHostname, domainName, family);
                LinuxUtils.ReserveIPAddress(linuxServerIPAddress, linuxPrinterIPAddress, activityData.PrimaryMacAddress, LinuxServiceType.DHCP);
                LinuxUtils.RestartService(linuxServerIPAddress, LinuxServiceType.DHCP);
                LinuxUtils.RestartService(linuxServerIPAddress, LinuxServiceType.DHCPV6);

                Printer.Printer printer = PrinterFactory.Create(family, linuxPrinterIPAddress);
                if (!ColdReset(activityData, printer))
                {
                    return false;
                }

                if (!ConfigureAndVerifyParameters(activityData, PrinterAccessType.Telnet))
                {
                    return false;
                }

                if (!ConfigureAndVerifyParameters(activityData, PrinterAccessType.SNMP))
                {
                    return false;
                }

                return ConfigureAndVerifyParameters(activityData, PrinterAccessType.EWS);
            }
            finally
            {
                LinuxPostRequisites(activityData);
                EwsWrapper.Instance().ResetConfigPrecedence();
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        /// <summary>
        /// Template Id: 682044
        /// 1. Configure the DHCPv4 scope with Hostname using DHCP Server with minimum lease period as 2 mins
        /// 2. Bring up the Device with DHCP IP.
        /// 3. Configure the precedence table with Manual/TFTP/DHCPBootP/DHCPv6/Default.
        /// 4. Verify the Hostname behavior on the device
        /// 5. Remove the Hostname from the DHCPserver and wait for 4 mins
        /// 6. Verify the Hostname behavior on the device
        /// 7. Configure DHCPv6 scope  with FQDN Value in the DHCPv6 server 
        /// 8. Enable and disable the IPv6 options
        /// 9. Verify the Hostname behavior on the device 
        /// Expected:
        /// 4. Hostname should be configured with DHCPv4 IP.
        /// 6. Hostname should be configured as Default.
        /// 9. Hostname should be configured as DHCPv6 IP
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool VerifyHostnameWithDifferentUI(IPConfigurationActivityData activityData)
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
                IPAddress linuxPrinterIPAddress = IPAddress.None;

                if (!LinuxPreRequisites(activityData, LinuxServiceType.DHCP, ref linuxPrinterIPAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                IPAddress linuxServerIPAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);

                int length = linuxServerIPAddress.ToString().Length;
                int lastIndex = linuxServerIPAddress.ToString().LastIndexOf('.') + 1;

                string fromRange = string.Format(CultureInfo.CurrentCulture, "{0}{1}", linuxServerIPAddress.ToString().Remove(lastIndex, length - lastIndex), "1");
                string toRange = string.Format(CultureInfo.CurrentCulture, "{0}{1}", linuxServerIPAddress.ToString().Remove(lastIndex, length - lastIndex), "200");

                string hostName = "DHCPv4LXHName";
                string domainName = "DHCPv4LXDName.in"
;

                // Modify dhcp file
                ConfigureDhcpFile(linuxServerIPAddress, fromRange, toRange, hostName, domainName);
                LinuxUtils.RestartService(linuxServerIPAddress, LinuxServiceType.DHCP);

                EwsWrapper.Instance().ResetConfigPrecedence();

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);
                bool isTps = PrinterFamilies.TPS.Equals(family) ? true : false;

                Printer.Printer printer = Printer.PrinterFactory.Create(family, linuxPrinterIPAddress);
                printer.PowerCycle();

                if (!ValidateHostName(family, hostName))
                {
                    return false;
                }

                LinuxUtils.ChangeDhcpHostname(linuxServerIPAddress, hostName, string.Empty);
                LinuxUtils.RestartService(linuxServerIPAddress, LinuxServiceType.DHCP);

                Thread.Sleep(TimeSpan.FromMinutes(4));

                if (!EwsWrapper.Instance().GetHostname().StartsWith("NPI", StringComparison.CurrentCultureIgnoreCase))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire default host name.");
                    return false;
                }

                TraceFactory.Logger.Info("Printer acquired default host name.");

                hostName = "DHCPv6LXHName";

                ConfigureDhcpv6File(linuxServerIPAddress, hostName, domainName, family);
                LinuxUtils.RestartService(linuxServerIPAddress, LinuxServiceType.DHCPV6);
                EwsWrapper.Instance().SetDHCPv6OnStartup(true);

                EwsWrapper.Instance().SetIPv6(false);
                EwsWrapper.Instance().SetIPv6(true);

                Thread.Sleep(TimeSpan.FromMinutes(1));

                return ValidateHostName(family, hostName);
            }
            finally
            {
                LinuxPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Template ID:96143
        /// Step1:
        /// 1. Connect the device in a network with DHCPv4/v6 server
        /// 2. Make an IPv4/v6 address reservation in a DHCP server.
        /// 3. Power cycle the device
        /// 4. Repeat step3 once again. 
        /// Step2: 
        /// 1. Connect the device in a network with DHCPv4/v6 server
        /// 2. Make an IPv4/v6 address reservation in a DHCP server.
        /// 3. Cold-reset the device
        /// 4. Repeat step3 once again.
        /// Expected:
        /// Device should always get configured with the reserved DHCPv4/v6 address for Powercycle or Cold Reset 
        public static bool VerifyIPAfterPowerCycleColdreset(IPConfigurationActivityData activityData, int testNo)
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

            bool isTPSFamily = PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily) ? true : false;

            string uuid = string.Empty;
            uuid = GetDhcpv6Uuid(activityData, activityData.PrimaryDhcpServerIPAddress, activityData.PrimaryWiredIPv4Address, testNo, activityData.PrimaryMacAddress, isTPSFamily);

            if (string.IsNullOrEmpty(uuid))
            {
                TraceFactory.Logger.Info("Unable to get Uuid of printer.");
                return false;
            }

            TraceFactory.Logger.Debug("Uuid: {0}".FormatWith(uuid));

            try
            {
                IPAddress linuxPrinterIPAddress = IPAddress.None;

                if (!LinuxPreRequisites(activityData, LinuxServiceType.DHCP, ref linuxPrinterIPAddress))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                IPAddress linuxServerAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);

                int length = linuxServerAddress.ToString().Length;
                int lastIndex = linuxServerAddress.ToString().LastIndexOf('.') + 1;

                string fromRange = string.Format(CultureInfo.CurrentCulture, "{0}{1}", linuxServerAddress.ToString().Remove(lastIndex, length - lastIndex), "1");
                string toRange = string.Format(CultureInfo.CurrentCulture, "{0}{1}", linuxServerAddress.ToString().Remove(lastIndex, length - lastIndex), "200");

                string dhcpv4Hostname = "DHCPv4LXHName";
                string dhcpv6Hostname = "DHCPv6LXHName";
                string domainName = "DHCPv4LXDName.in";

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                // Modify dhcp/ dhcpv6 file
                ConfigureDhcpFile(linuxServerAddress, fromRange, toRange, dhcpv4Hostname, domainName);
                ConfigureDhcpv6File(linuxServerAddress, dhcpv6Hostname, domainName, family);

                LinuxUtils.RestartService(linuxServerAddress, LinuxServiceType.DHCP);
                LinuxUtils.RestartService(linuxServerAddress, LinuxServiceType.DHCPV6);

                EwsWrapper.Instance().SetDHCPv6OnStartup(true);
                EwsWrapper.Instance().SetDHCPv6(false);
                EwsWrapper.Instance().SetDHCPv6(true);

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, linuxPrinterIPAddress.ToString());

                Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(6);
                IPAddress statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);

                TraceFactory.Logger.Debug("Stateful Address: {0}".FormatWith(statefulAddress));

                LinuxUtils.ReserveIPAddress(linuxServerAddress, linuxPrinterIPAddress, activityData.PrimaryMacAddress, LinuxServiceType.DHCP);
                LinuxUtils.ReserveIPAddress(linuxServerAddress, statefulAddress, uuid, LinuxServiceType.DHCPV6);

                LinuxUtils.RestartService(linuxServerAddress, LinuxServiceType.DHCP);
                LinuxUtils.RestartService(linuxServerAddress, LinuxServiceType.DHCPV6);

                for (int i = 0; i < 2; i++)
                {
                    printer.PowerCycle();

                    if (!CheckForPrinterAvailability(linuxPrinterIPAddress.ToString()))
                    {
                        return false;
                    }

                    if (!CheckForPrinterAvailability(statefulAddress.ToString()))
                    {
                        return false;
                    }
                }

                if (!activityData.ProductFamily.EqualsIgnoreCase("VEP"))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        ColdReset(activityData, printer);

                        // After cold reset enabling WSDiscovery option
                        EwsWrapper.Instance().SetWSDiscovery(true);

                        // IPv6 on startup option needs to be enabled for printer to acquire IPv6 address
                        EwsWrapper.Instance().SetDHCPv6OnStartup(true);
                        Thread.Sleep(TimeSpan.FromMinutes(1));

                        if (!CheckForPrinterAvailability(linuxPrinterIPAddress.ToString()))
                        {
                            return false;
                        }

                        if (!CheckForPrinterAvailability(statefulAddress.ToString()))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            finally
            {
                LinuxPostRequisites(activityData);
            }
        }

        #endregion

        #region Wireless

        /// <summary>
        /// 678837	
       ///  Assign the wired interface and the Europa Wired/ wireless interface has samesubnet of IP	
       /// "1.  Bring up the Embedded wired  interface as DHCP IP(Ex: 192.168.5.11).
       ///  2. Configure SSID and Bring up wireless interface also from the same same DHCP server as Embedded wired interfaces subnet(Ex: 192.168.5.12)
       /// 3. Bring up the Europa wired interface also from the same DHCP server as Embedded wired Subnet(Ex:192.168.5.13)
       /// 4. Verify the device behaviour with all the interfaces and do ping simulataneously from the server/Client machine"	
       /// 4. All interfaces should acquired IP from the same subnet. and Ping should be successful .
        /// 678836	
        /// Assign the wired interface and the Europa Wired/ wireless interface has different subnet of IP	
        /// "1. Bring up the embeded wired  interface as DHCP IP ( Ex: 192.168.5.11) 
        /// 2. Configure SSID and Bring up wireless interface as DHCP IP with different subnet(Ex: 192.168.3.11)
        /// 3. Bring up the Europa Wired interface also as DHCP IP with another Different subnet(Ex: 10.60.3.11)
        /// 4. Verify the device behaviour and do ping from the each subnet client."	
        /// 3. The device should accept the different subnet of IP address to all interfaces and ping should be successful from the each subnet client  machines.
        /// </summary>
        /// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
        /// <param name="sameSubnet">True if the test is for validating wired and wireless interface in the same subnet, else false.</param>
        /// <returns>true if the test is successful, else false.</returns>
        public static bool VerifySubnetsWiredWirelessInterfaces(IPConfigurationActivityData activityData, bool sameSubnet, int testNo)
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

            string dhcpServerIPAddress = string.Empty;
            string scopeIP = string.Empty;
            string wiredIpv4Address = string.Empty;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails = GetInterfaceDetails(activityData);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                printerInterfaceDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.DHCP; });

                if (!sameSubnet)
                {
                    if (!ConfigureInterfaceSubnets(activityData, ref printerInterfaceDetails, sameSubnet))
                    {
                        return false;
                    }
                }

                if (! ConfigureInterface(printerInterfaceDetails, false))
                {
                    return false;
                }

                return Ping(printerInterfaceDetails);
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                if(!sameSubnet)
                {
                    ConfigureInterfaceSubnets(activityData, ref printerInterfaceDetails);
                }


                printerInterfaceDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.BOOTP; });
                ConfigureInterface(printerInterfaceDetails, false);
            }
        }

        /// <summary>
        /// 96187
        /// Template Verify manual IP address while switching between wired and wireless modes
        /// Verify manual IP address while switching between wired and wireless modes	
        /// "1. Connect the device in wired mode and configure a manual IPv4 address on the device
        /// 2. Switch the device to wireless mode"	
        /// Expected : Manual IP should be retained when moved from wired to wireless and vice versa
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="sameSubnet"></param>
        /// <returns></returns>
        public static bool VerifyManualIPWiredToWireless(IPConfigurationActivityData activityData, int testNo)
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

            INetworkSwitch networkSwitch = null;
            IPAddress manualIPAddress = null;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                manualIPAddress = NetworkUtil.FetchNextIPAddress(IPAddress.Parse(activityData.PrimaryWiredIPv4Address).GetSubnetMask(), IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, manualIPAddress))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, manualIPAddress.ToString()))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().ConfigureWireless(activityData.SsidName))
                {
                    return false;
                }

                // Disconnect the port to bring the TPS printer in wireless mode.
                networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIP));

                if (!networkSwitch.DisablePort(activityData.PrimaryInterfacePortNo))
                {
                    return false;
                }

                // TODO : Check this sleep will ensure that the device goes to wireless mode.
                Thread.Sleep(TimeSpan.FromMinutes(1));

                string wirelessIPAddress = Utility.GetPrinterIPAddress(activityData.WirelessMacAddress);

                if (IPAddress.Parse(wirelessIPAddress).Equals(manualIPAddress))
                {
                    TraceFactory.Logger.Info("Printer has retained the wired manual IP: {0} in the wireless mode.".FormatWith(manualIPAddress.ToString()));
                }
                else
                {
                    TraceFactory.Logger.Info("Printer failed to retain the wired manual IP: {0} in the wireless mode. Current IP Address of the printer is : {1}.".FormatWith(manualIPAddress.ToString(), wirelessIPAddress));
                    return true;
                }

                TraceFactory.Logger.Info("Bringing back the printer to wired mode.");
                if (!networkSwitch.EnablePort(activityData.PrimaryInterfacePortNo))
                {
                    return false;
                }

                // Wait for the printer to acquire wired IP Address
                Thread.Sleep(TimeSpan.FromMinutes(2));

                string wiredIPAddress = Utility.GetPrinterIPAddress(activityData.PrimaryMacAddress);

                if (string.IsNullOrEmpty(wiredIPAddress))
                {
                    TraceFactory.Logger.Info("Printer failed to acquire wired IP address after 2 minutes.");
                    return false;
                }

                // Validating the Mac Address to ensure that the printer is in wired mode.
                if (IPAddress.Parse(wiredIPAddress).Equals(manualIPAddress))
                {
                    TraceFactory.Logger.Info("Printer retained the manual IP: {0} in wired mode.".FormatWith(manualIPAddress));
                }
                else
                {
                    TraceFactory.Logger.Info("Printer failed to retain the manual IP: {0} in wired mode.".FormatWith(manualIPAddress));
                    return false;
                }

                return ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, manualIPAddress.ToString());
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                if (!NetworkUtil.PingUntilTimeout(manualIPAddress, TimeSpan.FromSeconds(30)))
                {
                    // Ensure that the port is enabled.
                    networkSwitch.EnablePort(activityData.PrimaryInterfacePortNo);
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));

                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.PrimaryWiredIPv4Address), TimeSpan.FromSeconds(30)))
                {
                    TraceFactory.Logger.Info("Ping successful with wiredIPv4 address: {0}.".FormatWith(activityData.PrimaryWiredIPv4Address));
                }
                else
                {
                    TraceFactory.Logger.Info("Ping failed with wiredIPv4 address: {0}.".FormatWith(activityData.PrimaryWiredIPv4Address));
                }
            }
        }

        #endregion

        #region Link Speed

        /// <summary>
        /// 682048	
        /// Verify the device behavior when link speed is configured with Auto using DHCP
        /// Link speed configured Auto on the device and connected to 100Tx	
        /// 1. Connect the device on 1000Tx and  Bring up the Device with DHCP IP.
        /// 2. Configure the Link speed on the device as Auto using Networking -->Other settings--> Link settings
        /// 3.Connect the device on the switch its supports 100Tx
        /// 4. Verify the device behavior"	
        /// Expected: Device should start DHCP process and get DHCP IP
        /// In the configuration page device should show port config as 100Tx and Auto Negotiation as On
        /// Link speed configured Auto on the device and connected to 1000Tx	
        /// 1. Connect the device on 100Tx andBring up the Device with DHCP IP.
        /// 2. Configure the Link speed on the device as Auto using Networking -->Other settings--> Link settings
        /// 3.Connect the device on the switch its supports 1000Tx
        /// 4. Verify the device behavior"	
        /// Expected: "Device should start DHCP process and get DHCP IP
        /// In the configuration page device should show port config as 1000Tx and Auto Negotiation as On
        /// 682049	
        /// Verify the device behavior when link speed is configured with Auto using BootP
        /// Link speed configured Auto on the device and connected to 100Tx
        /// 1. Connect the device to 1000TX  switch and  Bring up the Device with BootP IP 
        /// 2. Configure the Link speed on the device as Auto using Networking -->Other settings--> Link settings
        /// 3. Connect the device on the switch its supports 100Tx
        /// 4. Verify the device behavior
        /// Expected: Device should start BootP process and get BootP IP
        /// In the configuration page device should show port config as 100Tx and Auto Negotiation as On
        /// Link speed configured Auto on the device and connected to 1000Tx
        /// 1.connect the device to 100Tx and  Bring up the Device with BootP IP.
        /// 2. Configure the Link speed on the device as Auto using Networking -->Other settings--> Link settings
        /// 3.Connect the device on the switch its supports 1000Tx
        /// 4. Verify the device behavior
        /// Expected:Device should start BootP process and get BootP IP
        /// In the configuration page device should show port config as 1000Tx and Auto Negotiation as On
        /// </summary>
        /// <param name="activityData"></param>
        public static bool VerifyAutoLinkSpeedWithDhcp(IPConfigurationActivityData activityData, IPConfigMethod configMethod, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                bool isTps = false;
                string linkSpeedValue = "100TX HALF";
                string autoNegotiationvalue = "On";

                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIP));

                if (!networkSwitch.SetLinkSpeed(activityData.PrimaryInterfacePortNo, (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString()) ? LinkSpeed.Auto : LinkSpeed.Auto1000Mbps)))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetIPConfigMethod(configMethod, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                if (PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily))
                {
                    isTps = true;
                    linkSpeedValue = "100 Mbps";
                    autoNegotiationvalue = "Half";
                    SnmpWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Auto);
                }
                else
                {
                    EwsWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Auto);
                }

                if (!networkSwitch.SetLinkSpeed(activityData.PrimaryInterfacePortNo, LinkSpeed.FullDuplex100Mbps))
                {
                    return false;
                }

                // TODO: Validate packets

                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (!CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, configMethod))
                {
                    return false;
                }

                if (!(ValidateLinkSpeed(linkSpeedValue, autoNegotiationvalue, isTps)))
                {
                    return false;
                }

                if (!networkSwitch.SetLinkSpeed(activityData.PrimaryInterfacePortNo, LinkSpeed.FullDuplex100Mbps))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetIPConfigMethod(configMethod, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                if (PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily))
                {
                    linkSpeedValue = "1000 Mbps";
                    autoNegotiationvalue = "Full";
                    SnmpWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Auto);
                }
                else
                {
                    isTps = false;
                    linkSpeedValue = "1000T FULL";
                    autoNegotiationvalue = "On";
                    EwsWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Auto);
                }

                if (!networkSwitch.SetLinkSpeed(activityData.PrimaryInterfacePortNo, (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString()) ? LinkSpeed.Auto : LinkSpeed.Auto1000Mbps)))
                {
                    return false;
                }

                // TODO: Validate packets
                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (!CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, configMethod))
                {
                    return false;
                }

                return ValidateLinkSpeed(linkSpeedValue, autoNegotiationvalue, isTps);
            }
            finally
            {
                client.Channel.Stop(guid);
                PostRequisitesLinkSpeed(activityData);
            }
        }

        /// <summary>
        /// 682050	
        /// Verify the device behavior when link speed is configured with 100Tx using BootP	
        /// Link speed configured 100Tx on the device and connected to 100Tx	
        /// "1. Bring up the Device with BootP IP.
        /// 2. Configure the Link speed on the device as 100TX Full using Networking -->Other settings--> Link settings
        /// 3.Connect the device on the switch its supports 100Tx 
        /// 4. Verify the device behavior"	
        /// "Device should start BootP process and get BootP IP
        /// In the configuration page device should show port config as 100Tx  Full and Auto Negotiation as off."
        /// Link speed configured 100Tx on the device and connected to 1000Tx	
        /// "1. Bring up the Device with BootP IP.
        /// 2. Configure the Link speed on the device as 100TX Full using Networking -->Other settings--> Link settings
        /// 3.Connect the device on the switch its supports 1000Tx 
        /// 4. Verify the device behavior"	
        /// "Device should start BootP process and get BootP IP
        /// In the configuration page device should show port config as Disconnected  and Auto Negotiation as off."
        /// 682051
        /// Link speed configured 100Tx on the device and connected to 100Tx
        /// 1.  Bring up the Device with DHCP IP.
        /// 2. Configure the Link speed on the device as 100TX Full using Networking -->Other settings--> Link settings
        /// 3.Connect the device on the switch its supports 100Tx 
        /// 4. Verify the device behavior
        /// Expected : Device should start DHCP process and get DHCP IP
        /// In the configuration page device should show port config as 100Tx  Full and Auto Negotiation as off."
        /// Link speed configured 100Tx on the device and connected to 1000Tx
        /// 1. Bring up the Device with BootP IP.
        /// 2. Configure the Link speed on the device as 100TX Full using Networking -->Other settings--> Link settings
        /// 3.Connect the device on the switch its supports 1000Tx 
        /// 4. Verify the device behavior
        /// Expected : Device should start DHCP process and get DHCP IP
        /// In the configuration page device should show port config as Disconnected  and Auto Negotiation as off.
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Verify100TXLinkSpeed(IPConfigurationActivityData activityData, IPConfigMethod configMethod, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                bool isTps = false;
                string linkSpeedValue = "100TX FULL";
                string autoNegotiationvalue = "Off";

                if (!EwsWrapper.Instance().SetIPConfigMethod(configMethod, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                if (PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily))
                {
                    isTps = true;
                    linkSpeedValue = "100 Mbps";
                    autoNegotiationvalue = "Full";
                    SnmpWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Full100Tx);
                }
                else
                {
                    EwsWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Full100Tx);
                }

                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIP));

                if (!networkSwitch.SetLinkSpeed(activityData.PrimaryInterfacePortNo, LinkSpeed.FullDuplex100Mbps))
                {
                    return false;
                }

                // TODO: Validate packets

                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (!CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, configMethod))
                {
                    return false;
                }

                if (!ValidateLinkSpeed(linkSpeedValue, autoNegotiationvalue, isTps))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetIPConfigMethod(configMethod, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                if (PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily))
                {
                    SnmpWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Full100Tx);
                }
                else
                {
                    EwsWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Full100Tx);
                }

                if (!networkSwitch.SetLinkSpeed(activityData.PrimaryInterfacePortNo, LinkSpeed.Auto1000Mbps))
                {
                    return false;
                }

                // TODO: Validate packets

                Thread.Sleep(TimeSpan.FromMinutes(1));

                return !CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address);
            }
            finally
            {
                client.Channel.Stop(guid);
                PostRequisitesLinkSpeed(activityData);
            }
        }

        /// <summary>
        /// 682052
        /// Verify the device behavior when link speed is configured with 1000Tx using BootP
        /// Link speed configured 1000Tx on the device and connected to 1000Tx
        /// 1. Bring up the Device with BootP IP.
        /// 2. Configure the Link speed on the device as 1000TX Full using Networking -->Other settings--> Link settings
        /// 3.Connect the device on the switch its supports 1000Tx 
        /// 4. Verify the device behavior
        /// Expected : Device should start BootP process and get BootP IP
        /// In the configuration page device should show port config as 1000Tx  and Auto Negotiation as off.
        /// Link speed configured 1000Tx on the device and connected to 100Tx
        /// 1. Bring up the Device with BootP IP.
        /// 2. Configure the Link speed on the device as 1000TX Full using Networking -->Other settings--> Link settings
        /// 3.Connect the device on the switch its supports 100Tx 
        /// 4. Verify the device behavior
        /// Expected : Device should goes to Auto IP
        /// In the configuration page device should show port config as 1000Tx  Full and Auto Negotiation as off.
        /// 682053
        /// Verify the device behavior when link speed is configured with 1000Tx using DHCP
        /// Link speed configured 1000Tx on the device and connected to 1000Tx
        /// 1. Bring up the Device with DHCP IP.
        /// 2. Configure the Link speed on the device as 1000TX Full using Networking -->Other settings--> Link settings
        /// 3.Connect the device on the switch its supports 1000Tx 
        /// 4. Verify the device behavior    
        /// Expected : Device should start DHCP process and get DHCP IP
        /// In the configuration page device should show port config as 1000Tx  and Auto Negotiation as off.
        /// Link speed configured 1000Tx on the device and connected to 100Tx
        /// 1. Bring up the Device with DHCP IP.
        /// 2. Configure the Link speed on the device as 1000TX Full using Networking -->Other settings--> Link settings
        /// 3.Connect the device on the switch its supports 100Tx 
        /// 4. Verify the device behavior
        /// Expected : Device should goes to Auto IP
        /// In the configuration page device should show port config as 1000Tx  Full and Auto Negotiation as off.
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool Verify1000TXLinkSpeed(IPConfigurationActivityData activityData, IPConfigMethod configMethod, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                bool isTps = false;
                string linkSpeedValue = "1000T FULL";
                string autoNegotiationvalue = "Off";

                if (!EwsWrapper.Instance().SetIPConfigMethod(configMethod, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                if (PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily))
                {
                    isTps = true;
                    linkSpeedValue = "1000 Mbps";
                    autoNegotiationvalue = "Full";
                    SnmpWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Full1000T);
                }
                else
                {
                    EwsWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Full1000T);
                }

                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIP));

                if (!networkSwitch.SetLinkSpeed(activityData.PrimaryInterfacePortNo, LinkSpeed.Auto1000Mbps))
                {
                    return false;
                }

                // TODO: Validate packets

                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (!CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address))
                {
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, configMethod))
                {
                    return false;
                }

                if (!ValidateLinkSpeed(linkSpeedValue, autoNegotiationvalue, isTps))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetIPConfigMethod(configMethod, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                if (PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily))
                {
                    SnmpWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Full1000T);
                }
                else
                {
                    EwsWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Full1000T);
                }

                if (!networkSwitch.SetLinkSpeed(activityData.PrimaryInterfacePortNo, LinkSpeed.FullDuplex100Mbps))
                {
                    return false;
                }

                // TODO: Validate packets

                Thread.Sleep(TimeSpan.FromMinutes(1));

                return !CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address);
            }
            finally
            {
                client.Channel.Stop(guid);
                PostRequisitesLinkSpeed(activityData);
            }
        }

        #endregion

        #region LAA

        /// <summary>
        /// 77396	
        /// LAA configuration from Telnet	
        /// Test setting LAA through a Telnet	
        /// "Power on a printer and print server.
        /// Enable and configure the TCP/IP protocol.
        /// Telnet into the print server, and view MAC address parameter on the self-test page.
        /// Verify that the correct MAC address is displayed.
        /// At the command prompt, type: laa 02XXXXXXXXXX, or any valid LAA.
        /// At the command prompt, type: exit and save.
        /// Print a self-test page.
        /// Telnet into the print server."	
        /// Expected: The MAC address should be changed to the LAA entered and  device to acquire new IP address
        /// </summary>
        /// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
        /// <returns>True for pass, else false.</returns>
        public static bool LaaConfigurationTelnet(IPConfigurationActivityData activityData, int testNo)
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

            // Remove last 2 digits and prefix the current mac address with 02.
            string laa = activityData.PrimaryMacAddress.Remove(activityData.PrimaryMacAddress.Length - 2, 2).Insert(0, "02");
            string currentDeviceAddress = activityData.PrimaryWiredIPv4Address;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string macAddress = TelnetWrapper.Instance().GetMacAddress().Replace(":", string.Empty);

                if (macAddress.EqualsIgnoreCase(activityData.PrimaryMacAddress))
                {
                    TraceFactory.Logger.Info("Successfully validated the MAC address from telnet.");
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to validate the MAC address from telnet. Value from telnet: {0}.".FormatWith(macAddress));
                    return false;
                }

                TelnetWrapper.Instance().SetLAA(currentDeviceAddress, laa);
                Thread.Sleep(TimeSpan.FromMinutes(4));

                // When LAA is changed, MAC address is also changed and the printer acquires a new IP address. Discover and find out the current IP address
                currentDeviceAddress = Utility.GetPrinterIPAddress(laa);

                if (string.IsNullOrEmpty(currentDeviceAddress))
                {
                    TraceFactory.Logger.Info("No printer was discovered with the MAC address: {0}.".FormatWith(laa));
                    return false;
                }

                if (!currentDeviceAddress.EqualsIgnoreCase(activityData.PrimaryWiredIPv4Address))
                {
                    TraceFactory.Logger.Info("Printer acquired the new IP address:{0} after changing LAA.".FormatWith(currentDeviceAddress));
                }
                else
                {
                    TraceFactory.Logger.Info("Printer failed to acquire new IP address after changing LAA.");
                    return false;
                }

                TelnetWrapper.Instance().Create(currentDeviceAddress);
                macAddress = TelnetWrapper.Instance().GetMacAddress().Replace(":", string.Empty);

                if (macAddress.EqualsIgnoreCase(activityData.PrimaryMacAddress))
                {
                    TraceFactory.Logger.Info("MAC address is changed to LAA. The currentMAC address is: {0}.".FormatWith(macAddress));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("MAC address is not changed to LAA. The currentMAC address is: {0}.".FormatWith(macAddress));
                    return false;
                }
            }
            finally
            {
                client.Channel.Stop(guid);
                PostRequisitesLaa(activityData, currentDeviceAddress);
            }
        }

        /// <summary>
        /// 77397	
        /// TEMPLATE  LAA configuration from Web UI	
        /// LAA config  through Web UI with DHCP as Config Method	
        /// "1.Power on a printer with a  DHCPv6 server and a router.
        /// 2. Web into the printer.
        /// 3.Change the DHCPv6 policy to 'Always perform DHCPv6 at startup'.
        /// 4. Go to Networking TAB->Other Settings->Misc.Setting and configure LAA  i.e(02XXXXXXXXXX)and
        /// 5.Verify the correct MAC address is displayed.
        /// 6.Verify the behavior of the device in case of  IPV4 and IPV6 after changing LAA.
        /// Note :For any IPV6 changes to take effect, enable/disable and enable the IPv6 checkbox."	
        /// "1.The printer should acquire IPv4 address, stateful DHCPv6 address,stateless IPv6 address and link-local address.
        /// 2.The MAC address should be changed to the LAA entered and the printer should acquire new IP address as per the DHCP process.
        /// Ensure that on changing LAA , it  is reflected in link local , stateless and stateful address in case of IPv6
        /// Note : IP reconfiguration should happen only once and not for a multiple times."
        /// LAA config on WebUI with BootP as config method	
        /// "1.Power on a printer with BootP IP
        /// 2.Web into the printer.
        /// 3.Go to Networking TAB->Other Settings->Misc.Setting
        /// and configure LAA  i.e(02XXXXXXXXXX)
        /// 4.Verify that the correct MAC address is displayed."
        /// "1.The MAC address should be changed to the LAA entered and device should acquire new IP address as per the BootP process.
        /// (i.e.  BootP request and reply packets should be sent.Check the wire shark capture)
        /// Note : IP reconfiguration should happen only once and not for a multiple times."
        /// LAA config on WebUI with Manual as config method	
        /// "1.Power on a printer with Manual IP
        /// 2.Web into the printer.
        /// 3.Go to Networking TAB->Other Settings->Misc.Setting and configure LAA  i.e(02XXXXXXXXXX)
        /// 4.Verify that the correct MAC address is displayed."	
        /// "1.The MAC address should be changed to the LAA entered and device should retain the same manual IP address and should send the DHCP inform packet to DHCP server to get other parameters. 
        /// Check the wire shark capture"
        /// </summary>
        /// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public static bool LaaConfigurationWebUI(IPConfigurationActivityData activityData, int testNo)
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

            // Remove last 2 digits and prefix the current mac address with 02.
            string laa = activityData.PrimaryMacAddress.Remove(activityData.PrimaryMacAddress.Length - 2, 2).Insert(0, "02");
            string currentDeviceAddress = activityData.PrimaryWiredIPv4Address;
            string manualIPAddress = string.Empty;

            bool result = false;
            PacketDetails validatePacketDetails = null;
            PacketDetails dhcpValidatePacketDetails = null;
            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info("STEP1: LAA config  through Web UI with DHCP as Config Method.");

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetDHCPv6OnStartup(true))
                {
                    return false;
                }

                // start

                EwsWrapper.Instance().SetLAA(laa);

                currentDeviceAddress = Utility.GetPrinterIPAddress(laa);

                if (string.IsNullOrEmpty(currentDeviceAddress))
                {
                    TraceFactory.Logger.Info("No printer was discovered with the MAC address: {0}.".FormatWith(laa));
                    return false;
                }

                if (!currentDeviceAddress.EqualsIgnoreCase(activityData.PrimaryWiredIPv4Address))
                {
                    TraceFactory.Logger.Info("Printer acquired the new IP address:{0} after changing LAA.".FormatWith(currentDeviceAddress));
                }
                else
                {
                    TraceFactory.Logger.Info("Printer failed to acquire new IP address after changing LAA.");
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP, currentDeviceAddress))
                {
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);

                if (!ValidateIPv6AddressWithMacAddress(laa))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Getting device to default state.");

                Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, currentDeviceAddress);
                ColdReset(activityData, printer);

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);

                TraceFactory.Logger.Info("STEP2: LAA config on WebUI with BootP as config method.");

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

                laa = activityData.PrimaryMacAddress.Remove(activityData.PrimaryMacAddress.Length - 2, 2).Insert(0, "02");
                string validateGuid = string.Empty;

                try
                {
                    validateGuid = client.Channel.TestCapture(activityData.SessionId, string.Format("{0}_Bootp_Validation", testNo));
                    EwsWrapper.Instance().SetLAA(laa);
                    // To capture packets added delay
                    Thread.Sleep(TimeSpan.FromMinutes(2));
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        // wait for 3 minutes
                        validatePacketDetails = client.Channel.Stop(validateGuid);

                        // validate bootp packets
                        // dhcpv6 -> solicit, advertisement, request, reply (check once)
                    }
                }

                currentDeviceAddress = Utility.GetPrinterIPAddress(laa);

                if (string.IsNullOrEmpty(currentDeviceAddress))
                {
                    TraceFactory.Logger.Info("No printer was discovered with the MAC address: {0}.".FormatWith(laa));
                    return false;
                }

                if (!currentDeviceAddress.EqualsIgnoreCase(activityData.PrimaryWiredIPv4Address))
                {
                    TraceFactory.Logger.Info("Printer acquired the new IP address:{0} after changing LAA.".FormatWith(currentDeviceAddress));
                }
                else
                {
                    TraceFactory.Logger.Info("Printer failed to acquire new IP address after changing LAA.");
                    return false;
                }

                if (!ValidateIPConfigMethod(activityData, IPConfigMethod.BOOTP, currentDeviceAddress))
                {
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);

                TraceFactory.Logger.Info("STEP3: LAA config on WebUI with Manual as config method.");

                manualIPAddress = NetworkUtil.FetchNextIPAddress(IPAddress.Parse(currentDeviceAddress).GetSubnetMask(), IPAddress.Parse(currentDeviceAddress)).ToString();

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, IPAddress.Parse(manualIPAddress)))
                {
                    return false;
                }

                laa = activityData.PrimaryMacAddress.Remove(activityData.PrimaryMacAddress.Length - 2, 2).Insert(0, "02");

                validateGuid = string.Empty;
                try
                {
                    validateGuid = client.Channel.TestCapture(activityData.SessionId, string.Format("{0}_Dhcp_Validation", testNo));
                    EwsWrapper.Instance().SetLAA(laa);
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        dhcpValidatePacketDetails = client.Channel.Stop(validateGuid);
                    }
                }

                currentDeviceAddress = Utility.GetPrinterIPAddress(laa);

                if (string.IsNullOrEmpty(currentDeviceAddress))
                {
                    TraceFactory.Logger.Info("No printer was discovered with the MAC address: {0}.".FormatWith(laa));
                    return false;
                }

                if (currentDeviceAddress.EqualsIgnoreCase(manualIPAddress))
                {
                    TraceFactory.Logger.Info("Printer retained the manual IP address:{0} after changing LAA.".FormatWith(currentDeviceAddress));
                }
                else
                {
                    TraceFactory.Logger.Info("Printer failed to retain the manual IP address after changing LAA.");
                    return false;
                }

                result = ValidateIPConfigMethod(activityData, IPConfigMethod.Manual, currentDeviceAddress);
            }
            finally
            {
                client.Channel.Stop(guid);
                // removed packet validation which is not required for LAA[Rafeek confirmed the same] dated 17/02/2016

                PostRequisitesLaa(activityData, currentDeviceAddress);
            }
            return result;
        }

        /// <summary>
        /// 77407	
        /// LAA set to Default MAC after cold reset		
        /// Default configuration of mac address	
        /// "Power on a printer and print server with a previously configured LAA.
        /// On boot up perform cold reset operation."	
        /// "• The original LAA should be replaced by the default physical MAC address.
        /// • All protocol stacks enabled should restart correctly."
        /// </summary>
        /// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
        /// <returns>returns true for pass, else false.</returns>
        public static bool VerifyLaaAfterColdReset(IPConfigurationActivityData activityData, int testNo)
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

            // Remove last 2 digits and prefix the current mac address with 02.
            string laa = activityData.PrimaryMacAddress.Remove(activityData.PrimaryMacAddress.Length - 2, 2).Insert(0, "02");
            string currentDeviceAddress = activityData.PrimaryWiredIPv4Address;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                EwsWrapper.Instance().SetLAA(laa);

                // When LAA is changed, MAC address is also changed and the printer acquires a new IP address. Discover and find out the current IP address
                currentDeviceAddress = Utility.GetPrinterIPAddress(laa);

                if (string.IsNullOrEmpty(currentDeviceAddress))
                {
                    TraceFactory.Logger.Info("Printer was not discovered.");
                    return false;
                }

                if (!currentDeviceAddress.EqualsIgnoreCase(activityData.PrimaryWiredIPv4Address))
                {
                    TraceFactory.Logger.Info("Printer acquired the new IP address:{0} after changing LAA.".FormatWith(currentDeviceAddress));
                }
                else
                {
                    TraceFactory.Logger.Info("Printer failed to acquire new IP address after changing LAA.");
                    return false;
                }

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, currentDeviceAddress);
                printer.ColdReset();

                // Printer is expected to have the reserved IP address after cold reset. So discovering with the actual MAC address
                currentDeviceAddress = Utility.GetPrinterIPAddress(activityData.PrimaryMacAddress);

                if (string.IsNullOrEmpty(currentDeviceAddress))
                {
                    TraceFactory.Logger.Info("No printer was discovered with the MAC address: {0}.".FormatWith(activityData.PrimaryMacAddress));
                    return false;
                }

                if (currentDeviceAddress.EqualsIgnoreCase(activityData.PrimaryWiredIPv4Address))
                {
                    TraceFactory.Logger.Info("Printer acquired the previous IP address:{0} after Cold reset.".FormatWith(activityData.PrimaryWiredIPv4Address));
                }
                else
                {
                    TraceFactory.Logger.Info("Printer failed to acquire the previous IP address:{0} after Cold reset.".FormatWith(activityData.PrimaryWiredIPv4Address));
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);

                laa = EwsWrapper.Instance().GetLAA();

                if (laa.EqualsIgnoreCase(activityData.PrimaryMacAddress))
                {
                    TraceFactory.Logger.Info("LAA is set to default MAC address after cold reset.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("LAA is not set to default MAC address after cold reset.");
                    return false;
                }

                // TODO: Packet Capture for protocol stack.
            }
            finally
            {
                client.Channel.Stop(guid);
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address);

                TelnetWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);
            }
        }

        #endregion

        #region Router

        /// <summary>
        /// 96205	
        /// TEMPLATE   Verify device behavior when only O flag is set	
        /// Verify device behavior when only O flag is set on the router	
        /// "1. Connect the device in a network with DHCPv6 server and a router. 
        /// 2. Make sure only O flag is set in router
        /// Login into the router  and go to the specific interface which is connected with your network. Type the command "" ipv6 nd  other-config-flag"".
        /// 3. In webui Change the DHCPv6 policy to perform DHCPv6 when requested by router.
        /// 4. Verify the device behavior "	
        /// 1. Device should update the parameters received from the server by sending a DHCPv6 INFORMATION REQUEST message and but device should not acquire the DHCPv6 IP address..
        /// </summary>
        /// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool VerifyOFlag(IPConfigurationActivityData activityData)
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

            IRouter router = null;
            router = RouterFactory.Create(IPAddress.Parse(activityData.RouterAddress), ROUTER_USERNAME, ROUTER_PASSWORD);

            RouterPreRequisites(activityData, ref router);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                int statelessAddressCount = 4;

                // Pre-requisites to set the dns values manually
                EwsWrapper.Instance().ResetConfigPrecedence();
                //added as part of the fix to update DNS Suffix Parameter[Suffix will get updated from server if ipconfig method is DHCP]
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                EwsWrapper.Instance().SetDHCPv6Router(true);
                EwsWrapper.Instance().SetPrimaryDnsv6Server(activityData.DHCPScopeIPv6Address);
                EwsWrapper.Instance().SetSecondaryDnsv6Server(activityData.DHCPScopeIPv6Address);
                EwsWrapper.Instance().DeleteAllSuffixes();

                if (!router.SetRouterFlag(activityData.RouterVlanId, RouterFlags.Other))
                {
                    return false;
                }

                if (PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily))
                {
                    TraceFactory.Logger.Info("Setting DHCPv6 as highest config precedence.");
                    SnmpWrapper.Instance().SetConfigPrecedence("3:2:1:0:4");
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    statelessAddressCount = 3;
                    //To update Server Parameters
                    Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
                    printer.PowerCycle();
                }
                else
                {
                    EwsWrapper.Instance().SetDHCPv6Router(true);
                    TraceFactory.Logger.Info("Setting DHCPv6 as highest config precedence.");
                    SnmpWrapper.Instance().SetConfigPrecedence("3:2:1:0:4");
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    EwsWrapper.Instance().ReinitializeConfigPrecedence();
                }

                Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(statelessAddressCount);

                IPAddress statefullAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
                IPAddress linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);
                Collection<IPAddress> statelessAddresses = IPv6Utils.GetStatelessIPAddress(ipv6Details);

                if (statefullAddress.Equals(IPAddress.IPv6None))
                {
                    TraceFactory.Logger.Info("Printer did not acquire DHCPv6 address as expected.");
                }
                else
                {
                    TraceFactory.Logger.Info("Expected: Printer should not acquire DHCPv6 address as expected. Actual: Printer acquired the DHCPv6 address: {0}".FormatWith(statefullAddress));
                    return false;
                }

                if (linkLocalAddress.Equals(IPAddress.IPv6None))
                {
                    TraceFactory.Logger.Info("Printer did not acquire link local address.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer acquired link local address : {0} as expected.".FormatWith(linkLocalAddress));
                }

                if (statelessAddresses.Count == statelessAddressCount)
                {
                    TraceFactory.Logger.Info("Printer has acquired {0} stateless addresses.".FormatWith(statelessAddressCount));
                    TraceFactory.Logger.Debug("Stateless addresses: {0}".FormatWith(string.Join(",", statelessAddresses)));
                }
                else
                {
                    TraceFactory.Logger.Info("Printer didn't acquire {0} stateless addresses as expected. Current count: {1}".FormatWith(statelessAddressCount, statelessAddresses.Count));
                    TraceFactory.Logger.Debug("Stateless addresses: {0}".FormatWith(string.Join(",", statelessAddresses)));
                    return false;
                }

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                string ipv6ScopePrimary = serviceMethod.Channel.GetIPv6Scope(activityData.PrimaryDhcpServerIPAddress);
                // string ipv6ScopeSecondary = serviceMethod.Channel.GetIPv6Scope(activityData.SecondDhcpServerIPAddress);
                string primaryv6DnsServer = serviceMethod.Channel.GetPrimaryDnsv6Server(activityData.PrimaryDhcpServerIPAddress, ipv6ScopePrimary);
                string secondaryv6DnsServer = serviceMethod.Channel.GetSecondaryDnsv6Server(activityData.PrimaryDhcpServerIPAddress, ipv6ScopePrimary);
                string domainSearchList = serviceMethod.Channel.GetDomainSearchList(activityData.PrimaryDhcpServerIPAddress, ipv6ScopePrimary);

                if (!ValidatePrimaryDnsV6Server((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily), primaryv6DnsServer))
                {
                    return false;
                }

                if (!ValidateSecondaryDnsV6Server((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily), secondaryv6DnsServer))
                {
                    return false;
                }

                if (!PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                {
                    return ValidateDomainSearchList((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily), domainSearchList);
                }
                else
                {
                    return true;
                }
            }
            finally
            {
                RouterPreRequisites(activityData, ref router, false);
            }
        }

        /// <summary>
        /// 96206	
        /// TEMPLATE   Verify Device behavior if only M flag is set	
        /// Verify Device behavior if only M flag is set on the router	
        /// "1. Connect the device in a network with DHCPv6 server and a router. 
        /// 2. Make sure only M flag is set in router.
        /// Login into the router  and go to the specific interface which is connected with your network. 
        /// Type the command 
        /// ""ipv6 nd managed-config-flag"" . 
        /// 3. In webui Change the DHCPv6 policy to perform DHCPv6 when requested by router.
        /// OR
        /// For TPS : Reset the DHCPv6 stack by disable-enable DHCPv6.
        /// 4. Verify the device behavior "	
        /// Expected: 1.Device should acquire the dhcpv6 address offered by the DHCPv6 server and should not get the dhcpv6 parameters.
        /// </summary>
        /// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
        /// <returns>True for success, else false.</returns>
        public static bool VerifyMFlag(IPConfigurationActivityData activityData)
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

            IRouter router = null;
            router = RouterFactory.Create(IPAddress.Parse(activityData.RouterAddress), ROUTER_USERNAME, ROUTER_PASSWORD);

            RouterPreRequisites(activityData, ref router);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Pre-requisites to set the dns values manually
                EwsWrapper.Instance().ResetConfigPrecedence();
                //added as part of the fix to update DNS Suffix Parameter[Suffix will get updated from server if ipconfig method is DHCP]
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                EwsWrapper.Instance().SetDHCPv6Router(true);
                EwsWrapper.Instance().SetPrimaryDnsv6Server(activityData.DHCPScopeIPv6Address);
                EwsWrapper.Instance().SetSecondaryDnsv6Server(activityData.DHCPScopeIPv6Address);
                EwsWrapper.Instance().DeleteAllSuffixes();

                if (!router.SetRouterFlag(activityData.RouterVlanId, RouterFlags.Managed))
                {
                    return false;
                }

                int statelessAddressCount = 4;

                if (PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily))
                {
                    TraceFactory.Logger.Info("Setting DHCPv6 as highest config precedence.");
                    SnmpWrapper.Instance().SetConfigPrecedence("3:2:1:0:4");
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    statelessAddressCount = 3;
                }
                else
                {
                    EwsWrapper.Instance().SetDHCPv6Router(true);
                    TraceFactory.Logger.Info("Setting DHCPv6 as highest config precedence.");
                    SnmpWrapper.Instance().SetConfigPrecedence("3:2:1:0:4");
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    EwsWrapper.Instance().ReinitializeConfigPrecedence();
                }

                Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(statelessAddressCount);

                IPAddress statefullAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
                IPAddress linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);
                Collection<IPAddress> statelessAddresses = IPv6Utils.GetStatelessIPAddress(ipv6Details);

                if (statefullAddress.Equals(IPAddress.IPv6None))
                {
                    TraceFactory.Logger.Info("Expected: Printer should acquire DHCPv6 address as expected. Actual: Printer did not acquire DHCPv6 address.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer acquired the DHCPv6 address: {0} as expected.".FormatWith(statefullAddress));
                }

                if (linkLocalAddress.Equals(IPAddress.IPv6None))
                {
                    TraceFactory.Logger.Info("Printer did not acquire link local address.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer acquired link local address : {0} as expected.".FormatWith(linkLocalAddress));
                }

                if (statelessAddresses.Count == statelessAddressCount)
                {
                    TraceFactory.Logger.Info("Printer has acquired {0} stateless addresses.".FormatWith(statelessAddressCount));
                    TraceFactory.Logger.Debug("Stateless addresses: {0}".FormatWith(string.Join(",", statelessAddresses)));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer didn't acquire {0} stateless addresses as expected. Current count: {1}".FormatWith(statelessAddressCount, statelessAddresses.Count));
                    TraceFactory.Logger.Debug("Stateless addresses: {0}".FormatWith(string.Join(",", statelessAddresses)));
                    return false;
                }

                // For M flag no need to validate the server parameters so the blocks has been removed dated 22/2/2016
            }
            finally
            {
                RouterPreRequisites(activityData, ref router, false);
            }
        }

        /// <summary>
        /// 415932	
        /// TEMPLATE   Verify Device behavior if both  M and O flag is enable and disable on the router	
        /// Verify Device behavior if M and O  flag is set on the router	
        /// "1. Connect the device in a network with DHCPv6 server and a router.
        /// 2. Configure DHCPv6 server to provide IPv6 addresses and some parameters like DNS Addresses and Domain search list.
        /// 3. Make sure M and O flag is set in router.
        /// Login into the router  and go to the specific interface which is connected with your network.
        /// Type the command 
        /// ""ipv6 nd managed-config-flag"" and "" ipv6 nd  other-config-flag"".
        /// 4.  In web ui Change the DHCPv6 policy to perform DHCPv6 when requested by router. OR For TPS : Reset the DHCPv6 stack by disable-enable DHCPv6. 
        /// 5. Verify the device behavior 
        /// 6. Disable the M and O flags on the router
        /// 7. Verify the device behavior"	
        /// "While M and O flag is set on the router
        /// 5. Device should acquire dhcpv6 address offered by the DHCPv6 server 
        /// Device should update the parameters received from the server by sending a DHCPv6 INFORMATION REQUEST message and getting a REPLY message.
        /// 7. Device should not acquire the DHCPv6 address and should clear the previously configured DHCPv6 parameter like DNS address and Domain search list."
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool VerifyMandOFlags(IPConfigurationActivityData activityData)
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

            IRouter router = null;
            router = RouterFactory.Create(IPAddress.Parse(activityData.RouterAddress), ROUTER_USERNAME, ROUTER_PASSWORD);

            RouterPreRequisites(activityData, ref router);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Pre-requisites to set the dns values manually
                EwsWrapper.Instance().ResetConfigPrecedence();
                //added as part of the fix to update DNS Suffix Parameter[Suffix will get updated from server if ipconfig method is DHCP]
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
                EwsWrapper.Instance().SetDHCPv6Router(true);
                EwsWrapper.Instance().SetPrimaryDnsv6Server(activityData.DHCPScopeIPv6Address);
                EwsWrapper.Instance().SetSecondaryDnsv6Server(activityData.DHCPScopeIPv6Address);
                EwsWrapper.Instance().DeleteAllSuffixes();

                if (!router.SetRouterFlag(activityData.RouterVlanId, RouterFlags.Both))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetIPv6(true))
                {
                    return false;
                }

                EwsWrapper.Instance().SetStatelessAddress(true);
                int statelessAddressCount = 4;

                if (PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily))
                {
                    TraceFactory.Logger.Info("Setting DHCPv6 as highest config precedence.");
                    SnmpWrapper.Instance().SetConfigPrecedence("3:2:1:0:4");
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    statelessAddressCount = 3;
                }
                else
                {
                    EwsWrapper.Instance().SetDHCPv6Router(true);
                    TraceFactory.Logger.Info("Setting DHCPv6 as highest config precedence.");
                    SnmpWrapper.Instance().SetConfigPrecedence("3:2:1:0:4");
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    EwsWrapper.Instance().ReinitializeConfigPrecedence();
                }

                Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(statelessAddressCount);

                IPAddress statefullAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
                IPAddress linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);
                Collection<IPAddress> statelessAddresses = IPv6Utils.GetStatelessIPAddress(ipv6Details);

                if (statefullAddress.Equals(IPAddress.IPv6None))
                {
                    TraceFactory.Logger.Info("Expected: Printer should acquire DHCPv6 address as expected. Actual: Printer did not acquire DHCPv6 address.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer acquired the DHCPv6 address: {0} as expected.".FormatWith(statefullAddress));
                }

                if (linkLocalAddress.Equals(IPAddress.IPv6None))
                {
                    TraceFactory.Logger.Info("Printer did not acquire link local address.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer acquired link local address : {0} as expected.".FormatWith(linkLocalAddress));
                }

                if (statelessAddresses.Count == statelessAddressCount)
                {
                    TraceFactory.Logger.Info("Printer has acquired {0} stateless addresses.".FormatWith(statelessAddressCount));
                    TraceFactory.Logger.Debug("Stateless addresses: {0}".FormatWith(string.Join(",", statelessAddresses)));
                }
                else
                {
                    TraceFactory.Logger.Info("Printer didn't acquire {0} stateless addresses as expected. Current count: {1}".FormatWith(statelessAddressCount, statelessAddresses.Count));
                    TraceFactory.Logger.Debug("Stateless addresses: {0}".FormatWith(string.Join(",", statelessAddresses)));
                    return false;
                }

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                string ipv6ScopePrimary = serviceMethod.Channel.GetIPv6Scope(activityData.PrimaryDhcpServerIPAddress);
                // string ipv6ScopeSecondary = serviceMethod.Channel.GetIPv6Scope(activityData.SecondDhcpServerIPAddress);
                string primaryv6DnsServer = serviceMethod.Channel.GetPrimaryDnsv6Server(activityData.PrimaryDhcpServerIPAddress, ipv6ScopePrimary);
                string secondaryv6DnsServer = serviceMethod.Channel.GetSecondaryDnsv6Server(activityData.PrimaryDhcpServerIPAddress, ipv6ScopePrimary);
                string domainSearchList = serviceMethod.Channel.GetDomainSearchList(activityData.PrimaryDhcpServerIPAddress, ipv6ScopePrimary);

                if (!ValidatePrimaryDnsV6Server((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily), primaryv6DnsServer))
                {
                    return false;
                }

                if (!ValidateSecondaryDnsV6Server((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily), secondaryv6DnsServer))
                {
                    return false;
                }

                if (!PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                {
                    if (!ValidateDomainSearchList((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily), domainSearchList))
                    {
                        return false;
                    }
                }

                // Pre-requisites to set the dns values manually
                EwsWrapper.Instance().ResetConfigPrecedence();
                EwsWrapper.Instance().SetPrimaryDnsv6Server(activityData.DHCPScopeIPv6Address);
                EwsWrapper.Instance().SetSecondaryDnsv6Server(activityData.DHCPScopeIPv6Address);
                EwsWrapper.Instance().DeleteAllSuffixes();

                if (!router.DisableRouterFlag(activityData.RouterVlanId))
                {
                    return false;
                }

                if (PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily))
                {
                    TraceFactory.Logger.Info("Setting DHCPv6 as highest config precedence.");
                    SnmpWrapper.Instance().SetConfigPrecedence("3:2:1:0:4");
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                }
                else
                {
                    EwsWrapper.Instance().SetDHCPv6Router(true);
                    TraceFactory.Logger.Info("Setting DHCPv6 as highest config precedence.");
                    SnmpWrapper.Instance().SetConfigPrecedence("3:2:1:0:4");
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    EwsWrapper.Instance().ReinitializeConfigPrecedence();
                }

                ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(statelessAddressCount);

                statefullAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
                linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);
                statelessAddresses = IPv6Utils.GetStatelessIPAddress(ipv6Details);

                if (statefullAddress.Equals(IPAddress.IPv6None))
                {
                    TraceFactory.Logger.Info("Printer has not acquired DHCPv6 address as expected.");
                }
                else
                {
                    TraceFactory.Logger.Info("Expected: Printer should not acquire DHCPv6 address. Actual: Printer has acquired DHCPv6 address {0}.".FormatWith(statefullAddress));
                    return false;
                }

                if (linkLocalAddress.Equals(IPAddress.IPv6None))
                {
                    TraceFactory.Logger.Info("Printer did not acquire link local address.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer acquired link local address : {0} as expected.".FormatWith(linkLocalAddress));
                }

                if (statelessAddresses.Count == statelessAddressCount)
                {
                    TraceFactory.Logger.Info("Printer has acquired {0} stateless addresses.".FormatWith(statelessAddressCount));
                    TraceFactory.Logger.Debug("Stateless addresses: {0}".FormatWith(string.Join(",", statelessAddresses)));
                }
                else
                {
                    TraceFactory.Logger.Info("Printer didn't acquire {0} stateless addresses as expected. Current count: {1}".FormatWith(statelessAddressCount, statelessAddresses.Count));
                    TraceFactory.Logger.Debug("Stateless addresses: {0}".FormatWith(string.Join(",", statelessAddresses)));
                    return false;
                }

                if (ValidatePrimaryDnsV6Server((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily), primaryv6DnsServer))
                {
                    TraceFactory.Logger.Info("Printer has Primary Dns v6 address when M and O flag are disabled. This is not the expected behavior.");
                    return false;
                }

                if (ValidateSecondaryDnsV6Server((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily), secondaryv6DnsServer))
                {
                    TraceFactory.Logger.Info("Printer has Secondary Dns v6 address when M and O flag are disabled. This is not the expected behavior.");
                    return false;
                }

                if (!PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                {
                    return ValidateDomainSearchList((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily), domainSearchList, true);
                }
                else
                {
                    return true;
                }
            }
            finally
            {
                RouterPreRequisites(activityData, ref router, false);
            }
        }

        /// <summary>
        /// 664238	
        /// Verify the device behavior using performs DHCPv6 only when requested by the router, while  stateless checkbox is disabled.	
        /// Verify device behavior on choosing  the IPV6 option 'Perform DHCPv6 only when requested by the router' while the stateless checkbox is disabled.	
        /// "1. Connect the device in a network where router and DHCPv6 server is available. 
        /// 2. Make sure  M and O flag are  set on the  router
        ///		a.Login into the router  and go to the specific interface which is connected with your network. 
        ///		b. Type the command ""ipv6 nd managed-config-flag""
        ///		c. Type the command "" ipv6 nd  other-config-flag"". 
        ///		d. Save  the  configuration in Router
        ///	3.Disable the stateless checkbox in the IPv6 Tab.
        ///	4. In web ui Change the DHCPv6 policy to perform DHCPv6 when requested by router in the IPv6 Tab.
        ///	5. Disable and enable IPv6 checkbox to perform the above action.
        ///	6. Verify the Device behavior."	
        ///	"1. The device should have only a link -local IP address."
        /// </summary>
        /// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
        /// <returns></returns>
        public static bool VerifyStatelessAddressDisabledWithMandOFlagSet(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            IRouter router = null;
            router = RouterFactory.Create(IPAddress.Parse(activityData.RouterAddress), ROUTER_USERNAME, ROUTER_PASSWORD);

            RouterPreRequisites(activityData, ref router);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!router.SetRouterFlag(activityData.RouterVlanId, RouterFlags.Both))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetStatelessAddress(false))
                {
                    return false;
                }

                EwsWrapper.Instance().SetDHCPv6Router(true);

                // Only link local address will be available
                Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(1);

                IPAddress statefullAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
                Collection<IPAddress> statelessAddresses = IPv6Utils.GetStatelessIPAddress(ipv6Details);
                IPAddress linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);

                if (statefullAddress.Equals(IPAddress.IPv6None))
                {
                    TraceFactory.Logger.Info("Printer did not acquire a DHCPv6 address.");
                }
                else
                {
                    TraceFactory.Logger.Info("Expected: Printer should not acquire DHCPv6 address. Actual: Printer acquired the DHCPv6 address: {0}.".FormatWith(statefullAddress));
                    return false;
                }

                if (statelessAddresses.Count == 0)
                {
                    TraceFactory.Logger.Info("Printer did not acquire stateless addresses as expected.");
                }
                else
                {
                    TraceFactory.Logger.Info("Expected: Printer should not acquire stateless addresses. Actual: Printer acquired  stateless addresses.");
                    return false;
                }

                if (!linkLocalAddress.Equals(IPAddress.IPv6None))
                {
                    TraceFactory.Logger.Info("Printer acquired a link local address as expected.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Expected: Printer acquires link local address only. Actual: Printer did not acquire link local address.");
                    return false;
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                client.Channel.Stop(guid);

                RouterPreRequisites(activityData, ref router, false);
            }
        }

        /// <summary>
        /// Template Id: 666762
        /// I)
        /// 1. Connect the device in a network with DHCPv6 server (ie connect the device in an isolated network)
        /// 2. In web ui ,Change the DHCPv6 policy to 'Perform DHCPv6 when stateless configuration is unsuccessful or disabled' in the IPv6 tab.
        /// 3. Disable the stateless checkbox.
        /// 4. Verify the Device behavior.
        /// Note :For any changes to take effect, disable and enable the IPV6 checkbox
        /// II)
        /// 1. Connect the device in a network with DHCPv6 server (ie connect the device in an isolated network)
        /// 2. In web ui ,Change the DHCPv6 policy to 'Perform DHCPv6 when stateless configuration is unsuccessful or disabled' in the IPv6 tab.
        /// 3. Enable the stateless checkbox.
        /// 4. Verify the Device behavior.
        /// Note :For any changes to take effect, disable and enable the IPV6 checkbox
        /// Expected:
        /// I. The device should acquire a stateful IP address from the DHCPv6 server.  
        /// II. The device should acquire a stateful IP address from the DHCPv6 server.
        /// /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool VerifyDhcpOptionWithStateless(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            IRouter router = null;
            router = RouterFactory.Create(IPAddress.Parse(activityData.RouterAddress), ROUTER_USERNAME, ROUTER_PASSWORD);

            RouterPreRequisites(activityData, ref router);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!router.DisableIPv6Address(activityData.RouterVlanId))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetIPv6(true))
                {
                    return false;
                }

                EwsWrapper.Instance().SetDHCPv6StatelessConfigurationOption();

                if (!EwsWrapper.Instance().SetStatelessAddress(false))
                {
                    return false;
                }

                Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(disableEnableIPv6Option: true);

                Collection<IPAddress> statelessIPAddress = IPv6Utils.GetStatelessIPAddress(ipv6Details);
                IPAddress statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);

                if (IPAddress.IPv6None.Equals(statefulAddress))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire Stateful address.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer acquired Stateful address: {0}".FormatWith(statefulAddress));
                }

                if (!EwsWrapper.Instance().SetStatelessAddress(true))
                {
                    return false;
                }

                ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(2);
                statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);

                if (IPAddress.IPv6None.Equals(statefulAddress))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire Stateful address.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Debug("Printer acquired stateful address: {0}".FormatWith(statefulAddress));
                    return true;
                }
            }
            finally
            {
                client.Channel.Stop(guid);

                RouterPreRequisites(activityData, ref router, false);
                EwsWrapper.Instance().SetStatelessAddress(true);
            }
        }

        /// <summary>
        /// Template Id: 666774
        /// 1. Connect the device in a network with DHCPv6 server and a router. Make sure 'M' and 'O' flags are disabled
        /// 2. In webui Change the DHCPv6 policy to ''Always perform DHCPv6 at the startup'
        /// 3. Enable the stateless checkbox and verify the device behavior.
        /// 4. Disable the stateless checkbox and verify the device behavior.
        /// Note :	1. Connect the device to the test network to test this scenario
        ///			2. For any changes to take effect, disable and enable the IPv6 checkbox.
        /// Expected:
        /// 1. While the stateless checkbox is enabled,the device should acquire  stateless IPv6 addresses  and stateful IP address and one link local address.
        /// 2. While the stateless checkbox is disabled,the device should acquire only stateful IP.
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool VerifyDhcpv6OptionOnStartupMOFlagsDisabled(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            IRouter router = null;
            router = RouterFactory.Create(IPAddress.Parse(activityData.RouterAddress), ROUTER_USERNAME, ROUTER_PASSWORD);

            RouterPreRequisites(activityData, ref router);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!router.DisableRouterFlag(activityData.RouterVlanId))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetIPv6(true))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetDHCPv6OnStartup(true))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetStatelessAddress(true))
                {
                    return false;
                }

                int statelessAddressCount = PrinterFamilies.TPS.ToString() == activityData.ProductFamily ? 3 : 4;

                Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(statelessAddressCount + 2);

                Collection<IPAddress> statelessIPAddress = IPv6Utils.GetStatelessIPAddress(ipv6Details);
                IPAddress statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
                IPAddress linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);

                if (0 == statelessIPAddress.Count)
                {
                    TraceFactory.Logger.Info("Stateless Addresses are not configured.");
                    TraceFactory.Logger.Debug("Stateless Addresses count: {0}".FormatWith(statelessIPAddress.Count));
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer is configured with {0} Stateless addresses.".FormatWith(statelessIPAddress.Count));
                }

                if (IPAddress.IPv6None.Equals(statefulAddress))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire Stateful address.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer is configured with stateful address: {0}.".FormatWith(statefulAddress));
                }

                if (IPAddress.IPv6None.Equals(linkLocalAddress))
                {
                    TraceFactory.Logger.Info("Printer is not configured with Link local address.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer is configured with Link local address: {0}.".FormatWith(linkLocalAddress));
                }

                if (!EwsWrapper.Instance().SetStatelessAddress(false))
                {
                    return false;
                }

                EwsWrapper.Instance().SetIPv6(false);
                EwsWrapper.Instance().SetIPv6(true);

                Thread.Sleep(TimeSpan.FromMinutes(1));

                ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails();

                statelessIPAddress = IPv6Utils.GetStatelessIPAddress(ipv6Details);
                statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
                linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);

                if (0 < statelessIPAddress.Count)
                {
                    TraceFactory.Logger.Info("Stateless Addresses are not cleared.");
                    TraceFactory.Logger.Debug("Stateless Addresses count: {0}".FormatWith(statelessIPAddress.Count));
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Stateless Addresses are cleared.");
                }

                if (IPAddress.IPv6None.Equals(statefulAddress))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire Stateful address.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer is configured with Stateful address: {0}.".FormatWith(statefulAddress));
                }

                if (IPAddress.IPv6None.Equals(linkLocalAddress))
                {
                    TraceFactory.Logger.Info("Printer is not configured with Link local address.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer is configured with Link local address: {0}.".FormatWith(linkLocalAddress));
                    return true;
                }
            }
            finally
            {
                client.Channel.Stop(guid);
                RouterPreRequisites(activityData, ref router, false);
            }
        }

        /// <summary>
        ///96139
        ///Verify device acquires an IP address using different IP configuration methods	
        ///Template Verify that device can obtain a basic DHCPv6 IP address	Verify that device can obtain a basic  DHCPv6IP address	
        ///"Step1:
        ///1. Cold-reset a device in an isolated network having DHCPv6 server and ensure no routers are providing stateless addresses.
        /// 2.Verify if device gets a DHCPv6 address. 
        /// Step2:
        /// 1. Cold-reset a device in a network with DHCPv6 server and router providing stateless-address. 
        /// 2. Change the DHCPv6 policy to Always perform DHCPv6 on startup."	
        /// "Step:1
        /// 1. The device should get a valid DHCPv6  stateful address.
        /// Step2:
        /// 1. On doing cold-reset  as mentioned ,the device should come up with stateless IPv6 address. 
        /// 2. On changing the DHCPv6 policy to 'Always perform DHCPv6 on startup, the device should get four stateless IPv6 address, one stateful  DHCPv6 address and one link local address."	
        /// </summary>
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool VerifyDhcpv6IPAcquisition(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            IRouter router = null;
            router = RouterFactory.Create(IPAddress.Parse(activityData.RouterAddress), ROUTER_USERNAME, ROUTER_PASSWORD);

            RouterPreRequisites(activityData, ref router);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Step 1:");

                if (!router.DisableIPv6Address(activityData.RouterVlanId))
                {
                    return false;
                }

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);

                ColdReset(activityData, printer);

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);

                if (!CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address))
                {
                    return false;
                }

                // The device will have 1 link local address and 1 state full address at this point of time.
                Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails();
                IPAddress statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);

                if (IPAddress.IPv6None.Equals(statefulAddress))
                {
                    TraceFactory.Logger.Info("Printer failed to acquire a DHCPv6 address after cold reset when router is not providing stateless address.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer acquired the DHCPv6 address: {0} after cold reset when router is not providing stateless address.".FormatWith(statefulAddress));
                }

                TraceFactory.Logger.Info("Step 2:");

                Collection<IPAddress> routerAddress = new Collection<IPAddress>(activityData.RouterIPv6Addresses.Select(x => IPAddress.Parse(x)).ToList());

                if (!router.EnableIPv6Address(activityData.RouterVlanId, routerAddress))
                {
                    return false;
                }

                // TPS: Doesn't have option for setting DHCP for Startup, router etc. Instead we are enabling/ disabling DHCPv6 option in Advanced to validate Stateful address
                // VEP: Following steps provided above.

                if (PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                {
                    EwsWrapper.Instance().SetDHCPv6(false);
                    Thread.Sleep(TimeSpan.FromMinutes(2));

                    int statelessAddressCount = 3;
                    ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(statelessAddressCount + 1);
                    statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);

                    if (!IPAddress.IPv6None.Equals(statefulAddress))
                    {
                        TraceFactory.Logger.Info("Printer acquired Stateful IP Address.");
                        TraceFactory.Logger.Debug("Stateful address: {0}".FormatWith(statefulAddress));
                        return false;
                    }

                    TraceFactory.Logger.Info("Printer didn't acquire Stateful address as expected.");
                    EwsWrapper.Instance().SetDHCPv6(true);
                    Thread.Sleep(TimeSpan.FromMinutes(2));

                    ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(statelessAddressCount + 1);
                    statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);

                    if (IPAddress.IPv6None.Equals(statefulAddress))
                    {
                        TraceFactory.Logger.Info("Printer didn't acquired Stateful IP Address.");
                        return false;
                    }

                    TraceFactory.Logger.Info("Printer acquired Stateful address: {0}".FormatWith(statefulAddress));
                    return true;

                }
                else
                {
                    ColdReset(activityData, printer);

                    // After cold reset enabling WSDiscovery option
                    EwsWrapper.Instance().SetWSDiscovery(true);

                    if (printer.PingUntilTimeout(IPAddress.Parse(activityData.PrimaryWiredIPv4Address), TimeSpan.FromSeconds(20)))
                    {
                        TraceFactory.Logger.Info("Ping succeeded after cold reset. Printer IP Address:{0}".FormatWith(activityData.PrimaryWiredIPv4Address));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Ping failed after cold reset the. Printer IP Address:{0}".FormatWith(activityData.PrimaryWiredIPv4Address));
                        return false;
                    }

                    // VEP acquires 4 stateless addresses from router and TPS acquires 3
                    int statelessAddressCount = PrinterFamilies.TPS.ToString() == activityData.ProductFamily ? 3 : 4;

                    // The device will have 1 link local address and 3/4 stateless addresses based on the product at this point of time.
                    ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(statelessAddressCount + 1);

                    Collection<IPAddress> statlessAddress = IPv6Utils.GetStatelessIPAddress(ipv6Details);

                    if (statlessAddress.Count == statelessAddressCount)
                    {
                        TraceFactory.Logger.Info("Printer acquired {0} stateless addresses after cold reset when router is providing stateless address.".FormatWith(statelessAddressCount));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Printer failed to acquire {0} stateless addresses after cold reset when router is providing stateless address.".FormatWith(statelessAddressCount));
                        return false;
                    }

                    if (!EwsWrapper.Instance().SetDHCPv6OnStartup(true))
                    {
                        return false;
                    }

                    // The device will have 1 link local address, 1 statefull address and 3/4 stateless addresses based on the product at this point of time.
                    ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(statelessAddressCount + 2);

                    statlessAddress = IPv6Utils.GetStatelessIPAddress(ipv6Details);
                    statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
                    IPAddress linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);

                    if (statlessAddress.Count == statelessAddressCount)
                    {
                        TraceFactory.Logger.Info("Printer acquired {0} stateless addresses.".FormatWith(statelessAddressCount));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Printer failed to acquire {0} stateless addresses.".FormatWith(statelessAddressCount));
                        return false;
                    }

                    if (IPAddress.IPv6None.Equals(statefulAddress))
                    {
                        TraceFactory.Logger.Info("Printer failed to acquire DHCPv6 address.");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Printer acquired DHCPv6 address: {0}.".FormatWith(statefulAddress));
                    }

                    if (IPAddress.IPv6None.Equals(linkLocalAddress))
                    {
                        TraceFactory.Logger.Info("Printer failed to acquire a link local address.");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Printer acquired the link local address: {0}.".FormatWith(statefulAddress));
                        return true;
                    }
                }
            }
            finally
            {
                client.Channel.Stop(guid);

                EwsWrapper.Instance().SetDHCPv6StatelessConfigurationOption();
                RouterPreRequisites(activityData, ref router, false);
            }
        }

        /// <summary>
        /// 96167	
        /// Template Verify Preferred lease time for DHCPv6 and IPv6 stateless addresses is updated on the device		
        ///	Verify Preferred lease time for stateful DHCPv6 and IPv6 stateless addresses is updated on the device	
        ///	"1. Connect the device in a network with a router and DHCPv6 server.
        /// 2. Configure in router and the DHCP server with finite preferred lease time.
        /// 3. Change the DHCPv6 policy to 'Always perform DHCPv6 on startup'
        /// 4.  Disable and Enable IPv6
        /// 5. Verify the lease time on the Device
        /// 6. Capture the network trace on the DHCPv6 server for DHCPv6 renewal packet"	
        /// "4. The device should acquire stateless IPv6 addresses and stateful DHCPv6 address and should get updated with finite preferred lease provided by DHCPv6 server and Router. 
        /// 5. The address should be renewed at half the lease time"
        /// 96168	
        /// Template Verify Valid lease time for DHCPv6 and IPv6 stateless addresses is updated on the device			
        ///	Verify Valid lease time for DHCPv6  addresses is updated on the device	"1. Connect the device in a network with a router and DHCPv6 server.
        /// 2. Configure in router and the DHCP server with finite valid lease time.
        /// 3. Change the DHCPv6 policy to 'Always perform DHCPv6 on startup'
        /// 4.  Disable and Enable IPv6
        /// 5. Verify the lease time on the Device
        /// 6. Capture the network trace on the DHCPv6 server for DHCPv6 renewal packet"	
        /// "4.The device should acquire stateless IPv6 addresses and stateful DHCPv6 address and should get updated with the finite valid lease provided by DHCPv6 server and Router. 
        /// The addresses should be renewed at half the lease time."
        /// </summary>
        /// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
        /// <param name="testNo">The Test No.</param>
        /// <returns>true for success, else false.</returns>
        public static bool VerifyPreferredValidLifeTime(IPConfigurationActivityData activityData, bool isPreferredLifetime, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            IRouter router = null;
            router = RouterFactory.Create(IPAddress.Parse(activityData.RouterAddress), ROUTER_USERNAME, ROUTER_PASSWORD);

            RouterPreRequisites(activityData, ref router);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TimeSpan routerValidLifeTime = new TimeSpan(24, 0, 0);
                TimeSpan routerPreferredLifeTime = new TimeSpan(23, 0, 0);

                if (!router.SetLeaseTime(activityData.RouterVlanId, routerValidLifeTime, routerPreferredLifeTime))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetDHCPv6OnStartup(true))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetStatelessAddress(true))
                {
                    return false;
                }

                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                int preferredLifeTime = serviceMethod.Channel.GetPreferredLifetime(activityData.PrimaryDhcpServerIPAddress, serviceMethod.Channel.GetIPv6Scope(activityData.PrimaryDhcpServerIPAddress));
                int validLifeTime = serviceMethod.Channel.GetValidLifetime(activityData.PrimaryDhcpServerIPAddress, serviceMethod.Channel.GetIPv6Scope(activityData.PrimaryDhcpServerIPAddress));

                int statelessAddressCount = PrinterFamilies.TPS.ToString() == activityData.ProductFamily ? 3 : 4;

                Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(statelessAddressCount + 2);
                IPAddress statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
                Collection<IPAddress> statelessIPAddress = IPv6Utils.GetStatelessIPAddress(ipv6Details);

                if (statelessAddressCount != statelessIPAddress.Count)
                {
                    TraceFactory.Logger.Info("Stateless Addresses are not acquired.");
                    TraceFactory.Logger.Debug("Stateless Addresses count: {0}".FormatWith(statelessIPAddress.Count));
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer acquired {0} stateless addresses.".FormatWith(statelessIPAddress.Count));
                }

                if (IPAddress.IPv6None.Equals(statefulAddress))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire Stateful address.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Debug("Printer acquired stateful address: {0}".FormatWith(statefulAddress));
                }

                return CompareIPv6LifeTime(ipv6Details, isPreferredLifetime ? new TimeSpan(0, 0, preferredLifeTime) : new TimeSpan(0, 0, validLifeTime), isPreferredLifetime, isPreferredLifetime ? routerPreferredLifeTime : routerValidLifeTime);


                // TODO: Packet validation
            }
            finally
            {
                client.Channel.Stop(guid);

                RouterPreRequisites(activityData, ref router, false);
            }
        }

        /// <summary>
        /// Template ID:96141
        /// All the interfaces with DHCPv4 address and DHCPv6 address and along with Stateless IPv6 address.
        /// "1.Connect  a device with all the interfaces  in a network having router which provides six or more stateless addresses.
        /// 2. Disable and enable the IPv6 options on the device.
        /// 3.Verify device gets configured with one link local address using ping
        /// 4. Select the IPv6 option ""Always startup option for stateful address""
        ///  5. Verify the Device behaviour"
        /// "3.Device should get configured by four stateless addresses provided by the router and should discard 
        /// remaining stateless ipv6 addresses from the router for all the interfaces.Device should get configured with a unique link- local address 
        /// Ping with Stateless IPv6 address and link-local address, IPv4 address for all the interfaces simultaneously should success.
        /// 5. Stateless IPv6, stateful DJCPv6, Link local address and DHCPv4 address should ping success for all the interfaces"
        /// <returns>true if template passed
        ///          false if template fails </returns>                
        public static bool VerifyStatelessLinkLocalAddress(IPConfigurationActivityData activityData, int testNo)
        {
            // Added as part of code analysis
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData))
            {
                return false;
            }

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            IRouter router = null;
            router = RouterFactory.Create(IPAddress.Parse(activityData.RouterAddress), ROUTER_USERNAME, ROUTER_PASSWORD);

            RouterPreRequisites(activityData, ref router);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                int routerAddressCount = activityData.RouterIPv6Addresses.Count;

                Collection<string> localRouterAddresses = activityData.RouterIPv6Addresses;

                // If the address count is less than 6
                if (routerAddressCount < 6)
                {
                    for (int i = routerAddressCount + 1; i <= 6; i++)
                    {
                        Random rnd = new Random(i);
                        string randomIpv6Address = localRouterAddresses[0].Replace(localRouterAddresses[0].Substring(activityData.RouterIPv6Addresses[0].ToString().LastIndexOf(':') + 1), rnd.Next().ToString("X", CultureInfo.CurrentCulture).Substring(0, 4).ToLower(CultureInfo.CurrentCulture));
                        localRouterAddresses.Add(randomIpv6Address);
                    }
                }

                TraceFactory.Logger.Info("Adding {0} stateless addresses to router.".FormatWith(activityData.RouterIPv6Addresses.Count));

                Collection<IPAddress> routerAddress = new Collection<IPAddress>(localRouterAddresses.Select(x => IPAddress.Parse(x)).ToList());

                if (!router.EnableIPv6Address(activityData.RouterVlanId, routerAddress))
                {
                    return false;
                }

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);

                Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails = GetInterfaceDetails(activityData);

                // For TPS & LFP perform Cold reset. For VEP, Disable\Enable IPv6 option
                if ((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily) == PrinterFamilies.VEP)
                {
                    foreach (var item in printerInterfaceDetails)
                    {
                        EwsWrapper.Instance().ChangeDeviceAddress(item.Address);
                        EwsWrapper.Instance().SetIPv6(false);
                        EwsWrapper.Instance().SetIPv6(true);
                    }

                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
                }
                else
                {
                    ColdReset(activityData, printer);

                    // After cold reset enabling WSDiscovery option
                    EwsWrapper.Instance().SetWSDiscovery(true);
                }

                foreach (var item in printerInterfaceDetails)
                {
                    TraceFactory.Logger.Info(CtcBaseTests.INTERFACE_HIGHLIGHTER.FormatWith(item.PrinterInterface));

                    printer = PrinterFactory.Create(activityData.ProductFamily, item.Address);

                    if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(item.Address), TimeSpan.FromSeconds(30)))
                    {
                        TraceFactory.Logger.Info("Ping is successful with Printer IP Address:{0}.".FormatWith(item.Address));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Ping failed with Printer IP Address:{0}.".FormatWith(item.Address));
                        return false;
                    }

                    // VEP acquires 4 stateless addresses from router and TPS acquires 3
                    int statelessAddressCount = PrinterFamilies.TPS.ToString() == activityData.ProductFamily ? 3 : 4;

                    EwsWrapper.Instance().ChangeDeviceAddress(item.Address);
                    Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(statelessAddressCount + 1);

                    Collection<IPAddress> statelessIPAddress = IPv6Utils.GetStatelessIPAddress(ipv6Details);
                    IPAddress linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);

                    if (statelessAddressCount != statelessIPAddress.Count)
                    {
                        TraceFactory.Logger.Info("Printer didn't acquire {0} stateless IP addresses.".FormatWith(statelessAddressCount));
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Printer acquired {0} stateless IP addresses as expected.".FormatWith(statelessAddressCount));
                    }

                    if (IPAddress.IPv6None.Equals(linkLocalAddress))
                    {
                        TraceFactory.Logger.Info("Printer didn't acquire link local IP Address.");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Printer acquired link local IP Address.");
                    }

                    // Check if the device got a unique link local address     
                    item.StatelessAddresses = statelessIPAddress;
                    item.LinkLocalAddress = linkLocalAddress;               
                }

                if(!Ping(printerInterfaceDetails))
                {
                    return false;
                }

                foreach (var item in printerInterfaceDetails)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(item.Address);

                    if (!EwsWrapper.Instance().SetDHCPv6OnStartup(true))
                    {
                        return false;
                    }

                    printer = PrinterFactory.Create(activityData.ProductFamily, item.Address);

                    item.StatefullAddress = printer.IPv6StateFullAddress;
                }

                return Ping(printerInterfaceDetails);

            }
            finally
            {
                client.Channel.Stop(guid);

                RouterPreRequisites(activityData, ref router, false);
            }
        }

        #endregion

        #region Remote Subnet

        /// <summary>
        /// 77046	
        /// Template DHCP configuration for IPv4 and IPv6 with server on a remote subnet			
        ///	DHCP configuration with server on a remote subnet	
        ///	"1.Enable a DHCPv4 and DHCPv6 server  on a remote subnet.
        /// 2.Cold reset a printer.
        /// 3.Add the printer to the network.
        /// 4.Get a network trace to confirm that the packet destination is correct."	
        /// Expected:
        /// "1.The printer should be configured from the remote server.
        /// 2.Print a self-test page and review the DHCP manager's Active Lease Windows to confirm that the printer has been configuredOR
        /// Access the EWS page to verity that printer has been configured with the parameters from the DHCP server on the remote subnet."
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyRemoteSubnet(IPConfigurationActivityData activityData, int testNo)
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

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

            string currentDeviceAddress = activityData.PrimaryWiredIPv4Address;
            string scopeAddress = string.Empty;
            IRouter router = null;
            int routerVlanId = 0;

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // 1. Stop the current DHCP server
                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                if (!serviceMethod.Channel.StopDhcpServer())
                {
                    return false;
                }

                //Get the address prefix so that scope address, start range, end range etc. can be determined. prefix = x.x.x.
                string prefix = activityData.PrimaryWiredIPv4Address.Substring(0, activityData.PrimaryWiredIPv4Address.LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase) + 1);
                scopeAddress = string.Concat(prefix, "0");

                //  2. Create a scope in the second dhcp server
                serviceMethod = DhcpApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress);

                // Scope range is taken as "x.x.x.1 to x.x.x.200"
                if (!serviceMethod.Channel.CreateScope(activityData.SecondDhcpServerIPAddress, scopeAddress, "remoteScope", string.Concat(prefix, "2"), string.Concat(prefix, "200")))
                {
                    return false;
                }

                string serverHostName = "RemoteHost";
                string serverDomainName = "RemoteDomain";

                // 3. Set scope parameters
                serviceMethod.Channel.SetHostName(activityData.SecondDhcpServerIPAddress, scopeAddress, serverHostName);
                serviceMethod.Channel.SetDomainName(activityData.SecondDhcpServerIPAddress, scopeAddress, serverDomainName);

                string routerAddress = ROUTER_IP_FORMAT.FormatWith(activityData.PrimaryWiredIPv4Address.Substring(0, activityData.PrimaryWiredIPv4Address.LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase)));
                router = RouterFactory.Create(IPAddress.Parse(routerAddress), ROUTER_USERNAME, ROUTER_PASSWORD);

                Dictionary<int, IPAddress> routerVlans = router.GetAvailableVirtualLans();

                routerVlanId = routerVlans.Where(x => (null != x.Value) && (x.Value.IsInSameSubnet(IPAddress.Parse(routerAddress)))).FirstOrDefault().Key;

                if (!router.DeleteHelperAddress(routerVlanId, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress)))
                {
                    return false;
                }

                if (!router.ConfigureHelperAddress(routerVlanId, IPAddress.Parse(activityData.SecondDhcpServerIPAddress)))
                {
                    return false;
                }

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
                ColdReset(activityData, printer);

                currentDeviceAddress = Utility.GetPrinterIPAddress(activityData.PrimaryMacAddress);

                if (string.IsNullOrEmpty(currentDeviceAddress))
                {
                    TraceFactory.Logger.Info("Printer didn't acquire IP address from the remote subnet after cold reset.");
                    return false;
                }

                if (CheckForPrinterAvailability(currentDeviceAddress))
                {
                    TraceFactory.Logger.Info("Printer acquired the IP address: {0} from the remote subnet after cold reset.".FormatWith(currentDeviceAddress));
                }
                else
                {
                    TraceFactory.Logger.Info("Printer didn't acquire IP address from the remote subnet after cold reset.");
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);
                TelnetWrapper.Instance().Create(currentDeviceAddress);
                SnmpWrapper.Instance().Create(currentDeviceAddress);

                // After cold reset enabling WSDiscovery option
                EwsWrapper.Instance().SetWSDiscovery(true);

                if (ValidateHostName((PrinterFamilies)Enum<PrinterFamilies>.Parse(activityData.ProductFamily, true), serverHostName))
                {
                    TraceFactory.Logger.Info("Printer acquired host name from the remote server.");
                }
                else
                {
                    TraceFactory.Logger.Info("Printer didn't acquire host name from the remote server.");
                    return false;
                }

                if (ValidateDomainName((PrinterFamilies)Enum<PrinterFamilies>.Parse(activityData.ProductFamily, true), serverDomainName))
                {
                    TraceFactory.Logger.Info("Printer acquired domain name from the remote server.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer didn't acquire domain name from the remote server.");
                    return false;
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                client.Channel.Stop(guid);

                if (null != router)
                {
                    router.DeleteHelperAddress(routerVlanId, IPAddress.Parse(activityData.SecondDhcpServerIPAddress));
                    router.ConfigureHelperAddress(routerVlanId, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress));
                }

                if (!string.IsNullOrEmpty(scopeAddress))
                {
                    DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress);
                    serviceMethod.Channel.DeleteScope(activityData.SecondDhcpServerIPAddress, scopeAddress);
                }

                DhcpApplicationServiceClient primaryServiceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                primaryServiceMethod.Channel.StartDhcpServer();

                // Hose break is performed to get back the previous IP Address
                Utility.PerformHoseBreak(currentDeviceAddress, activityData.SwitchIP, activityData.PrimaryInterfacePortNo, (int)TimeSpan.FromSeconds(10).TotalMilliseconds);

                CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address, TimeSpan.FromMinutes(1));

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
                TelnetWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);
                SnmpWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);
            }
        }

        #endregion

        #region SI

        /// <summary>
        /// 726952
        /// Obtain the legacy IP for all interfaces when the DHCP server is unavailable and says duplicates	
        /// 1. Configure the DHCP server lease time as 2mins 
        /// 2. Brin up the device in DHCP IP for Embedded Wired, Europa Wired and WirelessInterface interfaces.
        /// 3. Change the Default IP = Legacy IP and Make sure check box is enabled for ""send dhcp request if IP address as AutopIP/Legacy""  in the Advanced tab of the EWS page
        /// 4. Stop the DHCP server and wait for 5 mins
        /// 5. Verify the device behaviour.
        /// 6. Power cycle the device.
        /// 7. Once again verify the device behaviour
        /// 8. Start the DHCP server
        /// 9. Once again verify the device behaviour"	
        /// Expected:
        /// 5. Embedded and Europa Wired interfaces and then WirelessInterface Interface on the Device should all aquire the Legacy IP as 192.0.0.192 .  
        /// Then in the configuration page of the all the interfaces should shows as Duplicate address detected.All the interfaces should send the DHCP Discover Packetin the backend during Legacy IP.
        /// 7. After powercylcing also Embedded and Europa Wired and then WirelessInterface Interfaces on the Device should aquire the Legacy IP with Duplicate detection . 
        /// All the interfaces should send the DHCP Discover Packet.
        /// 9 .Make sure device acquires DHCP IP for all the interfaces after bringing up the DHCP server"
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyDuplicateLegacyIpAddress(IPConfigurationActivityData activityData, int testNo)
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

            Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails = GetInterfaceDetails(activityData);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(Utility.WriteStep("Step I: Configure the DHCP server lease time as 2mins"));

                using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    dhcpClient.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress), 120);
                }

                TraceFactory.Logger.Info(Utility.WriteStep("Step II: Bring up the device in DHCP IP for Embedded Wired, Europa Wired and WirelessInterface interfaces"));

                // Set Boot as config method for all interfaces and then dhcp to acquire the lease time
                printerInterfaceDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.BOOTP; });

                if (!ConfigureInterface(printerInterfaceDetails, false))
                {
                    return false;
                }

                // Set DHCP as config method for all interfaces
                printerInterfaceDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.DHCP; });

                if (!ConfigureInterface(printerInterfaceDetails, false))
                {
                    return false;
                }

                // Fetch the link local address of each interfcae  for later access
                Printer.Printer printer = null;

                printerInterfaceDetails.ForEach(x =>
                {
                    printer = PrinterFactory.Create(activityData.ProductFamily, x.Address);
                    x.LinkLocalAddress = printer.IPv6LinkLocalAddress;
                });

                TraceFactory.Logger.Info(Utility.WriteStep("Step III. Change the Default IP = Legacy IP  and  Make sure check box is enabled  for \"send dhcp request if IP address as AutopIP / Legacy\"  in the Advanced tab of the EWS page"));

                PacketDetails packetDetials = null;

                using (PacketCaptureServiceClient packetCaptureClient = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    string guid = packetCaptureClient.Channel.TestCapture(activityData.SessionId, "{0}-{1}".FormatWith(testNo.ToString(), "Legacy IP Verification"));

                    foreach (var item in printerInterfaceDetails)
                    {
                        TraceFactory.Logger.Info(CtcBaseTests.INTERFACE_HIGHLIGHTER.FormatWith(item.PrinterInterface));

                        EwsWrapper.Instance().ChangeDeviceAddress(item.Address);

                        if (!EwsWrapper.Instance().SetDefaultIPType(DefaultIPType.LegacyIP))
                        {
                            return false;
                        }

                        if (!EwsWrapper.Instance().SetSendDHCPRequestOnAutoIP(true))
                        {
                            return false;
                        }
                    }

                    TraceFactory.Logger.Info(Utility.WriteStep("Step IV. Stop  the DHCP server and wait for 5 mins"));

                    using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                    {
                        if (dhcpClient.Channel.StopDhcpServer())
                        {
                            TraceFactory.Logger.Info("DHCP server is stopped.");
                        }
                        else
                        {
                            TraceFactory.Logger.Info("Failed to stop the DHCP server.");
                        }

                        TraceFactory.Logger.Info("waiting for 5 minutes after stopping the DHCP server.");

                        Thread.Sleep(TimeSpan.FromMinutes(5));
                    }

                    packetDetials = packetCaptureClient.Channel.Stop(guid);
                }

                TraceFactory.Logger.Info(Utility.WriteStep("Step V: Verify if the interfaces acquired Legacy IP with duplicate address"));

                // Renew local machine IP configuration
                NetworkUtil.RenewLocalMachineIP();

                string address = string.Empty;

                foreach (var item in printerInterfaceDetails)
                {
                    address = Utility.GetPrinterIPAddress(item.MacAddress);

                    if (string.IsNullOrEmpty(address))
                    {
                        // Ping link local address continuously 
                        NetworkUtil.PingUntilTimeout(item.LinkLocalAddress, TimeSpan.FromMinutes(1));
                        EwsWrapper.Instance().ChangeDeviceAddress(item.LinkLocalAddress);

                        address = EwsWrapper.Instance().GetIPv4Address();
                    }

                    if (!IsPrinterInLegacyIP(address))
                    {
                        return false;
                    }

                    // Ping link local address continuously 
                    NetworkUtil.PingUntilTimeout(item.LinkLocalAddress, TimeSpan.FromMinutes(1));
                    EwsWrapper.Instance().ChangeDeviceAddress(item.LinkLocalAddress);

                    if (!EwsWrapper.Instance().ValidateDuplicateIP())
                    {
                        return false;
                    }

                    if (!ValidateDhcpDiscoverPacket(activityData.PrimaryDhcpServerIPAddress, packetDetials.PacketsLocation, item.MacAddress))
                    {
                        return false;
                    }
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);

                TraceFactory.Logger.Info(Utility.WriteStep("Step VI: Power cycle the device"));

                using (PacketCaptureServiceClient packetCaptureClient = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    string guid = packetCaptureClient.Channel.TestCapture(activityData.SessionId, "{0}-{1}".FormatWith(testNo.ToString(), "Legacy IP Verification after power cycle"));

                    printer = PrinterFactory.Create(activityData.ProductFamily, printerInterfaceDetails.FirstOrDefault(x => x.PrinterInterface == PrinterConnectionType.PrimaryWiredInterface).LinkLocalAddress.ToString());
                    printer.PowerCycle();

                    packetDetials = packetCaptureClient.Channel.Stop(guid);
                }

                TraceFactory.Logger.Info(Utility.WriteStep("Step VII: Verify Device behavior"));

                foreach (var item in printerInterfaceDetails)
                {
                    address = Utility.GetPrinterIPAddress(item.MacAddress);

                    if (string.IsNullOrEmpty(address))
                    {
                        // Ping link local address continuously 
                        NetworkUtil.PingUntilTimeout(item.LinkLocalAddress, TimeSpan.FromMinutes(1));
                        EwsWrapper.Instance().ChangeDeviceAddress(item.LinkLocalAddress);

                        address = EwsWrapper.Instance().GetIPv4Address();
                    }

                    if (!IsPrinterInLegacyIP(address))
                    {
                        return false;
                    }

                    // Ping link local address continuously 
                    NetworkUtil.PingUntilTimeout(item.LinkLocalAddress, TimeSpan.FromMinutes(1));
                    EwsWrapper.Instance().ChangeDeviceAddress(item.LinkLocalAddress);

                    if (!EwsWrapper.Instance().ValidateDuplicateIP())
                    {
                        return false;
                    }

                    if (!ValidateDhcpDiscoverPacket(activityData.PrimaryDhcpServerIPAddress, packetDetials.PacketsLocation, item.MacAddress))
                    {
                        return false;
                    }
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);

                TraceFactory.Logger.Info(Utility.WriteStep("Step VII: Start DHCP Server"));

                using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress))
                {
                    if (dhcpClient.Channel.StartDhcpServer())
                    {
                        TraceFactory.Logger.Info("DHCP server is started.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to start the DHCP server.");
                    }
                }

                // Renew local machine IP configuration
                NetworkUtil.RenewLocalMachineIP();

                TraceFactory.Logger.Info(Utility.WriteStep("Step VIII: Verify Device behavior"));

                foreach (var item in printerInterfaceDetails)
                {
                    address = Utility.GetPrinterIPAddress(item.MacAddress);

                    if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP, item.Address))
                    {
                        return false;
                    }
                }

                return true;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                AutoIPPostRequisites(activityData, printerInterfaceDetails);
            }
        }

        /// <summary>
        /// 726958
        /// Wired interface BootP IP to Europa wired	
        /// "1.Bring up the device with BootP IP to Embedded Wired and europa wired interfaces and then wireless interface.
        /// 2. Assign manually the Embedded wired interface IP to Europa Wired interface
        /// 3.  Verify the Device behaviour"	
        /// "1. Device should acquires the BootIP for all the interfaces.
        /// 3. Device should detect Duplicate IP detection to Embedded and Europa wired interfaces."
        /// Wired interface DHCP IP to WirelessInterface interface
        /// "1.Bring up the device with DHCP IP to Embedded Wired and europa wired interfaces and then wireless interface.
        /// 2. Assign manually the Embedded wired interface IP to Europa wireless interface
        /// 3. Verify the Device behaviour"	
        /// "1. Device should acquires the DHCP IP for all the interfaces.
        /// 3. Device should detect Duplicate IP detection to Embedded and Europa wired interfaces.
        /// Aftersometime Embedded wired interfaces should get new ip address from the DHCP server"
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyDuplicateBootpDhcpIpAddress(IPConfigurationActivityData activityData, int testNo)
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


            Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails = GetInterfaceDetails(activityData);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Set BOOTP as config method for all interfaces
                printerInterfaceDetails.ForEach(x => { x.PrinterReservationType = ReservationType.Bootp; x.ConfigMethod = IPConfigMethod.BOOTP; });

                // Fetch the link local address of each interfcae  for later access
                Printer.Printer printer = null;

                printerInterfaceDetails.ForEach(x =>
                {
                    x.ConfigMethod = IPConfigMethod.BOOTP;
                    printer = PrinterFactory.Create(activityData.ProductFamily, x.Address);
                    x.LinkLocalAddress = printer.IPv6LinkLocalAddress;
                });

                if(!VerifyDuplicateIpManual(activityData, printerInterfaceDetails, PrinterConnectionType.SecondaryWiredInterface))
                {
                    return false;
                }

                printerInterfaceDetails.ForEach(x => { x.PrinterReservationType = ReservationType.Dhcp; x.ConfigMethod = IPConfigMethod.DHCP;});

                return VerifyDuplicateIpManual(activityData, printerInterfaceDetails, PrinterConnectionType.WirelessInterface);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                printerInterfaceDetails.ForEach(x => { x.PrinterReservationType = ReservationType.Both; x.ConfigMethod = IPConfigMethod.BOOTP; });
                ConfigureInterface(printerInterfaceDetails);
            }
        }

        /// <summary>
        /// 726953	
        /// Different Config method for all the interfaces	
        /// "1. Configure the BootP and DHCP server in the network. 
        /// 2. Bring up the device in DHCP IP for Embedded Wired, Europa Wired and WirelessInterface interfaces.
        /// 3. Change the config method to Embedded wired as BootP IP, Europa Wired as Manual and wireless IP as remains same with DHCP IP.
        ///  4. Verify the device behaviour and try to Ping all the interfaces.
        /// 5. Power cycle the device 
        /// 6. Once again verify the device behaviour and try to ping all the interfaces.
        /// 7. Cold reset the Device 
        /// 8. Once again verify the device behaviour and try to ping all the interfaces."	
        /// "4. Embedded wired should acquired with BOOTP IP, Euorpa Wired with Manual IP and WirelessInterface remains with DHCP IP.All interfaces IP's should Ping Succes.
        /// 6. After Powercycle also All the interfaces should retain the same config method and ping should success.
        /// 8. The device acquires the BootIP for all the interfaces, since the BootIP server available in the network."
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool VerifyIPAcquistionPowerCycleColdReset(IPConfigurationActivityData activityData)
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


            Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails = GetInterfaceDetails(activityData);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(Utility.WriteStep("Step Ia: Bring up the device in DHCP IP for Embedded Wired, Europa Wired and WirelessInterface interfaces"));

                // Set DHCP as config method for all interfaces
                printerInterfaceDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.DHCP; });

                if (!ConfigureInterface(printerInterfaceDetails, false))
                {
                    return false;
                }

                printerInterfaceDetails.ForEach(x =>
                {
                    x.ConfigMethod =  (x.PrinterInterface == PrinterConnectionType.PrimaryWiredInterface) ? IPConfigMethod.BOOTP : ((x.PrinterInterface == PrinterConnectionType.SecondaryWiredInterface) ? IPConfigMethod.Manual : IPConfigMethod.DHCP);
                });

                TraceFactory.Logger.Info(Utility.WriteStep("Step Ib: Configure Bootp for Embedded Wired, Manual IP Europa Wired and Dhcp for WirelessInterface interfaces"));

                if (!ConfigureInterface(printerInterfaceDetails, false))
                {
                    return false;
                }

                TraceFactory.Logger.Info(Utility.WriteStep("Verify Device behavior."));

                if (!ValidateIPConfigMethod(activityData, printerInterfaceDetails))
                {
                    return false;
                }

                TraceFactory.Logger.Info(Utility.WriteStep("Step Id: Power cycle the device"));

                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
                printer.PowerCycle();

                return ValidateIPConfigMethod(activityData, printerInterfaceDetails);
                // TODO: Check if cold reset can be done with DAT.
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                printerInterfaceDetails.ForEach(x => { x.PrinterReservationType = ReservationType.Both; x.ConfigMethod = IPConfigMethod.BOOTP; });
                ConfigureInterface(printerInterfaceDetails);
            }
        }

        /// <summary>
        /// 726965	
        /// Differnet Link speed configured on the different interfaces	"
        /// 1. Configure the  DHCP server in the network.
        /// 2. Bring up the device in DHCP IP for Embedded Wired, Europa Wired and WirelessInterface interfaces.
        /// 3. Change the link speed to 100TX Full to the Embedded wired interface.    
       ///  by using Networking -->Other settings--> Link settings
       /// 4.Connect the device Embedded wired interfaces on the switch where its same configuration as 100Tx 
       /// 5. Verify the device behaviour
       /// 6. Change the link speed to 1000TX Full to the Europa wired interface.    
      ///  by  using Networking -->Other settings--> Link settings
      /// 7.Connect the device Europa wired interfaces on the switch where its same configuration as1000Tx
      /// 8. Verify the device behaviour "	
      /// "2. Device should come up with DHCP IP for the all the interfaces.
      /// 5. In the configuration page device should show of the embedded wired interfacseport config as 100Tx and Auto Negotiation as off and the interface should able to ping successfully
      /// 8. In the configuration page device should show of the Europa wired interfacse port config as 1000Tx and Auto Negotiation as off and the interface should able to ping successfully"
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool VerifyLinkSpeedOnInterfaces(IPConfigurationActivityData activityData)
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

            Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails = GetInterfaceDetails(activityData);

            INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIP));

            if (!networkSwitch.SetLinkSpeed(activityData.PrimaryInterfacePortNo, LinkSpeed.Auto))
            {
                return false;
            }

            if (!networkSwitch.SetLinkSpeed(activityData.SecondaryInterfacePortNo, LinkSpeed.Auto))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(Utility.WriteStep("Step I: Bring up the device in DHCP IP for Embedded Wired, Europa Wired and WirelessInterface interfaces"));

                // Set DHCP as config method for all interfaces
                printerInterfaceDetails.ForEach(x => { x.PrinterReservationType = ReservationType.Dhcp; x.ConfigMethod = IPConfigMethod.DHCP; });

                if (!ConfigureInterface(printerInterfaceDetails))
                {
                    return false;
                }

                TraceFactory.Logger.Info(Utility.WriteStep("Step II: Change the link speed to 100TX Full to the Embedded wired interface."));

                if (!EwsWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Full100Tx))
                {
                    return false;
                }

                TraceFactory.Logger.Info(Utility.WriteStep("Step III: Connect the device Embedded wired interfaces on the switch where its same configuration as 100Tx"));

                if (!networkSwitch.SetLinkSpeed(activityData.PrimaryInterfacePortNo, LinkSpeed.FullDuplex100Mbps))
                {
                    return false;
                }

                if (!CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address, TimeSpan.FromMinutes(1)))
                {
                    return false;
                }

                if (!ValidateLinkSpeed("100TX FULL", "Off", address: activityData.PrimaryWiredIPv4Address))
                {
                    return false;
                }

                TraceFactory.Logger.Info(Utility.WriteStep("Step IV: Change the link speed to 100TX HALF to the Europa wired interface"));

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.SecondaryWiredIPv4Address);

                if (!EwsWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Half100Tx))
                {
                    return false;
                }

                TraceFactory.Logger.Info(Utility.WriteStep("Step V: Connect the device Embedded wired interfaces on the switch where its same configuration as 100Tx"));

                if (!networkSwitch.SetLinkSpeed(activityData.PrimaryInterfacePortNo, LinkSpeed.HalfDuplex100Mbps))
                {
                    return false;
                }

                if (!CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address, TimeSpan.FromMinutes(1)))
                {
                    return false;
                }

                return ValidateLinkSpeed("100TX HALF", "Off", address: activityData.SecondaryWiredIPv4Address);
            }
            finally
            {
                PostRequisitesLinkSpeed(activityData);
                printerInterfaceDetails.ForEach(x => { x.PrinterReservationType = ReservationType.Both; x.ConfigMethod = IPConfigMethod.BOOTP; });
                ConfigureInterface(printerInterfaceDetails);
            }
        }

        /// <summary>
        /// different IPV6 option for different interfaces	
        /// "1.  Make sure  M and O flag are  set on the  router 
        ///  a.Login into the router and go to the specific interface which is connected with your network.
        ///  b.Type the command ""ipv6 nd managed-config-flag""
        /// c.Type the command "" ipv6 nd other-config-flag"".
        /// d.Save the configuration in Router
        /// 2. Connect the device ina network where router and DHCPv6 server is available and bring up the Embedded, Europa wired and wireless interfaces with cold reset.
        /// 3 Verify the ipv6 optiion for all the interfaces
        /// 4. Change the Europa wired interface IPv6 option to Router requested and then disable and enable the ipv6 for the same interface.
        /// 5. Change the wireless interface IPv6 option to always perform dhcpv6.
        /// 6. Disable the stateless optiion in wired interfaces and verify the device behaviour 
        /// 7. Power cycle the device and veriy the device behaviour.
        /// 8. Do cold reset to the device and verify the behaviour. "	
        /// "3. All the interfaces ipv6 option should be by default with stateless is unsuccessful or disabled.with this all the interfaces should have the Link local IP and four stateless ipv6 address. 
        /// 4. Europa wired interfaces should have link local IP, Four stateless ipv6 address and one DHCPv6 address. 
        /// 5. Wireless interface should have link local IP, Four stateless ipv6 address and one DHCPv6 address.
        /// 6. Wired interface should have one linl local IP and one DHCPv6 address.
        /// 7. After powercycle, device should retain the above IPv6 address as per the options.
        /// 8. After cold reset device with all the interfaces should have the same ipv6 options as Stateless is unsuccessful or disabled.
        /// All the interface should have one link local and four stateless ipv6 address. "
        /// </summary>
        /// <param name="activityData"></param>
        /// <returns></returns>
        public static bool VerifyIpv6Option(IPConfigurationActivityData activityData, int testNo)
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

			PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
			string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

			IRouter router = null;
			router = RouterFactory.Create(IPAddress.Parse(activityData.RouterAddress), ROUTER_USERNAME, ROUTER_PASSWORD);

			RouterPreRequisites(activityData, ref router);


			Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails = GetInterfaceDetails(activityData);

			try
			{
				TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

				TraceFactory.Logger.Info(Utility.WriteStep("Step 1: Set both M and O flag on router."));

				if (!router.SetRouterFlag(activityData.RouterVlanId, RouterFlags.Both))
				{
					return false;
				}

                EwsWrapper.Instance().SetStatelessAddress(true);
				EwsWrapper.Instance().SetDHCPv6StatelessConfigurationOption();

				TraceFactory.Logger.Info(Utility.WriteStep("Verify the ipv6 optiion for all the interfaces"));

				foreach (var item in printerInterfaceDetails)
				{
					TraceFactory.Logger.Info("IPv6 status for {0}".FormatWith(item.PrinterInterface));

					EwsWrapper.Instance().ChangeDeviceAddress(item.Address);

					if (!VerifyIPv6AddressStatus(4, false))
					{
						return false;
					}
				}

				TraceFactory.Logger.Info(Utility.WriteStep("Step 2: IPv6 option to Router requested on secondary wired interface."));

				EwsWrapper.Instance().ChangeDeviceAddress(activityData.SecondaryWiredIPv4Address);

				EwsWrapper.Instance().SetDHCPv6Router(true);

				TraceFactory.Logger.Info("IPv6 status for secondary wired interface");

				if (!VerifyIPv6AddressStatus(4, true))
				{
					return false;
				}

				TraceFactory.Logger.Info(Utility.WriteStep("Step 3: wireless interface IPv6 option to always perform dhcpv6."));

				EwsWrapper.Instance().ChangeDeviceAddress(activityData.WirelessIPv4Address);

				if (!EwsWrapper.Instance().SetDHCPv6OnStartup(true))
				{
					return false;
				}

				TraceFactory.Logger.Info("IPv6 status for wireless interface");

				if (!VerifyIPv6AddressStatus(4, true))
				{
					return false;
				}

				TraceFactory.Logger.Info(Utility.WriteStep("Step 4: Disable stateless option in wired interfaces."));

				foreach (var item in printerInterfaceDetails.Where(x => x.PrinterInterface != PrinterConnectionType.WirelessInterface))
				{
					TraceFactory.Logger.Info(Utility.WriteStep("Disable stateless option and validation of IPv6 status for {0}.".FormatWith(item.PrinterInterface)));

					EwsWrapper.Instance().ChangeDeviceAddress(item.Address);

					if (!EwsWrapper.Instance().SetStatelessAddress(false))
					{
						return false;
					}

					TraceFactory.Logger.Info("IPv6 status for {0}".FormatWith(item.PrinterInterface));

					if (!VerifyIPv6AddressStatus(0, item.PrinterInterface == PrinterConnectionType.PrimaryWiredInterface ? true : false))
					{
						return false;
					}
				}

				Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);

				printer.PowerCycle();

                // Waiting for some time after power cycle.
                Thread.Sleep(TimeSpan.FromMinutes(3));

				foreach (var item in printerInterfaceDetails)
				{
					TraceFactory.Logger.Info(Utility.WriteStep("IPv6 status for {0} after power cycle.".FormatWith(item.PrinterInterface)));

					EwsWrapper.Instance().ChangeDeviceAddress(item.Address);

					if (item.PrinterInterface == PrinterConnectionType.WirelessInterface)
					{
						if (!VerifyIPv6AddressStatus(4, true))
						{
							return false;
						}
					}
					else
					{
						if (!VerifyIPv6AddressStatus(0, item.PrinterInterface == PrinterConnectionType.PrimaryWiredInterface ? true : false))
						{
							return false;
						}
					}
				}

				EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
				return true;
			}
			finally
			{
				RouterPreRequisites(activityData, ref router, false);

				foreach (var item in printerInterfaceDetails.Where(x => x.PrinterInterface != PrinterConnectionType.WirelessInterface))
				{
					TraceFactory.Logger.Info(Utility.WriteStep("Enable stateless option in wired interfaces {0}.".FormatWith(item.PrinterInterface)));

					EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);

					EwsWrapper.Instance().SetStatelessAddress(true);

                    EwsWrapper.Instance().SetDHCPv6StatelessConfigurationOption();
                }

				EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
			}
		}

		/// <summary>
		/// 726966	
		/// Step 1: 
		/// Keep all the interfaces UP and do Reinitialize Now using DHCP IP on Europa wired interface	
		/// "1.  Configure the DHCP server lease time as 10 mins 
		/// 2. Brin up the device in DHCP IP for Embedded Wired, Europa Wired and Wireless interfaces.
		/// 3. Confirm that the device has acquired an IP address from the DHCP server.
		/// 4. Click""Reinitialize Now"" in config precedence tab on the Europa wired interface Note : Only for VEP."	
		/// Expected: 1. Eurpa Wired interfaces should start the DHCP process (of sending Discover, Offer,Request and Acknowledge) and acquires same DHCPv4 address which it had before, 
		/// if the same IP is available or should acquire a new DHCPv4 address. Other interface should not be impact.
		/// Step 2:
		/// Keep all the interfaces UP and do Reinitialize Now using BootP IP on Europa wireless interface  
		/// 1.  Configure the BootP server in the network.
		/// 2. Brin up the device in BootP IP for Embedded Wired, Europa Wired and Wireless interfaces.
		/// 3. Confirm that the device has acquired an IP address from the BootP server.
		/// 4. Click""Reinitialize Now"" in config precedence tab on the Wireless interface Note : Only for VEP. 
		/// Expected: 1. Europa Wireless should start the BootP process (of sending Bootp Request and Boot Reply packet) and acquire the same Bootp IPv4 address which it had before , 
		/// if the same Bootp IP is available. Other interfaces should be not impacted.
		/// Step 3:
		/// Keep all the interfaces UP and do Clear previous value and Reinitialize Now using DHCP IP for Europa Wired interfaces
		/// 1.  Configure the DHCP server lease time as 10 mins
		/// 2. Brin up the device in DHCP IP for Embedded Wired, Europa Wired and Wireless interfaces.
		/// 3. Confirm that the device has acquired an IP address from the DHCP server.
		/// 4. Click"" Clear previous value and Reinitialize Now"" in config precedence tab on the Europa wired interface Note : Only for VEP.
		/// Expected: 1. Euorpa Wired interfaces should start the BootP process  and acquires BootP address. 
		/// If BootP Server is not available in the network, it should acquire the DCHP IP. Other interface should not be impact.
		/// Step 4: 
		/// Keep all the interfaces UP and do Clear previous value and Reinitialize Now using DHCP IP for Wireless interfaces   
		/// 1.  Configure the DHCP server lease time as 10 mins 
		/// 2. Brin up the device in DHCP IP for Embedded Wired, Europa Wired and Wireless interfaces.
		/// 3. Confirm that the device has acquired an IP address from the DHCP server.
		/// 4. Click"" Clear previous value and Reinitialize Now"" in config precedence tab on the wireless interface Note : Only for VEP.
		/// Expected: 1. Wireless  interfaces should start the BootP process  and acquires BootP address. 
		/// If BootP Server is not available in the network, it should acquire the DCHP IP. Other interface should not be impact.
		/// Step 5:
		/// Keep all the interfaces UP and do config method change to Europa wired interfaces
		/// 1. Configure the BootP and DHCP server in the network.
		/// 2. Bring up the device in BootP IP for Embedded Wired, Europa Wired and Wireless interfaces.\
		/// 3. Change the config method to Europa Wired to DHCP IP and wireless IP as remains same with BootP IP.
		/// 4. Verify the device behaviour and try to Ping all the interfaces. 
		/// Expecetd: 4. Embedded wired, Europa and wireless interfaces should acquired with BOOTP IP.
		/// 4. Europa wired interface should successfully changed the config method to DHCP IP and all the interface should ping success Note:  
		/// During config method to Euroap interface other interface should not be impacted like should retain the same config method and should not send any BootP process again on Embedded wired and wireless interfaces
		/// Step 6:
		/// Keep all the interfaces UP and do config method change to wireless interfaces
		/// 1. Configure the BootP and DHCP server in the network.
		/// 2. Bring up the device in DHCP IP for Embedded Wired, Europa Wired and Wireless interfaces.
		/// 3. Change the config method to wireless to Manual IP and Wired interfaces as remains same with BootP IP.
		/// 4. Verify the device behaviour and try to Ping all the interfaces.
		/// Expected: 4. Embedded wired, Europa and wireless interfaces  should acquired with DHCP IP.
		/// Wireless  interface should successfully changed the config method to Manual IP and all the interface should ping success   
		/// Note:  During config method to Wireless interface other interface should not be impacted like should retain the same config method and should not send any DHCP process again on Embedded wired and Europa interfaces
		/// Step 7:
		/// Keep all the interfaces UP and do IPv6 disable and Enable
		/// 1. Configure the  DHCP server in the isolated  network.
		/// 2. Bring up the device in DHCP IP for Embedded Wired, Europa Wired and Wireless interfaces.
		/// 3. Disable IPv6 on the Europa wired interface
		/// 4. Verify the Devie behaviour
		/// 5.Enable the Europa wired interface
		/// 6. Verify the Devie behaviour"	
		/// Expected: 2. All the interface should come up with DHCP IP.
		/// 4. Disabling IPv6 option on the Europa wired interfaces should disable the IPV6 address and IPv4 should not impact on the Europa wired interface. 
		/// But still the Embedded wired interface and wireless interfaces have IPv6 option enabled and able to ping all the ipv6 address.
		/// 5. Enabling IPv6 option on the Europa wired interface, linklocal, and stateful ipv6 address should come up.The same ipv6 address able to ping."
		/// Step 8: 
		/// Keep all the interfaces UP and do IPv6 disable and Enable on Embedded wired interface   
		/// 1. Configure the  DHCP server in the isolated  network.
		/// 2. Bring up the device in DHCP IP for Embedded Wired, Europa Wired and Wireless interfaces.
		/// 3. Disable IPv6 on the Embedded wired interface
		/// 4. Verify the Devie behaviour
		/// 5.Enable the Europa Embedded interface
		/// 6. Verify the Devie behaviour
		/// Expected: 2. All the interface should come up with DHCP IP.
		/// 4. Disabling IPv6 option on the Embedded wired interfaces should disable the IPV6 address and IPv4 should not impact on the Europa wired interface. 
		/// But still the Europa wired interface and wireless interfaces have IPv6 option enabled and able to ping all the ipv6 address.
		/// 5. Enabling IPv6 option on the Embedded wired interface, linklocal, and stateful ipv6 address should come up.The same ipv6 address able to ping."
		/// Step 9:
		/// Keep all the interfaces UP and do IPv6 disable and Enable on Europa Wireless interface
		/// 1. Configure the  DHCP server in the isolated  network.
		/// 2. Bring up the device in DHCP IP for Embedded Wired, Europa Wired and Wireless interfaces.
		/// 3. Disable IPv6 on the Europa Wireless interface
		/// 4. Verify the Devie behaviour
		/// 5.Enable the Europa Wireless interface
		/// 6. Verify the Devie behaviour
		/// Expected: 2. All the interface should come up with DHCP IP.
		/// 4. Disabling IPv6 option on the Europa Wireless interfaces should disable the IPV6 address and IPv4 should not impact on the Europa wired interface. 
		/// But still the Embedded wired interface and wireless interfaces have IPv6 option enabled and able to ping all the ipv6 address.
		/// 5. Enabling IPv6 option on the Europa Wireless interface, linklocal, and stateful ipv6 address should come up.The same ipv6 address able to ping."
		/// Step 10: 
		/// Keep all the interfaces UP and do Enable and disable the DHCPv4 stateless options using DHCP config method on Europa wired interface.
		/// 1. Configure the  DHCP server in the isolated  network. 
		/// 2. Bring up the device in DHCP IP for Embedded Wired, Europa Wired and Wireless interfaces.
		/// 3. Configure a DHCPv4 server to supply IP parameters as Hostname, Domain Name and DNS server IP address
		/// 4  .Enable the stateless DHCPv4 option on Europa wired interface. 
		/// 5. Change the config method from DHCP to Manual on Europa wired interface. 
		/// 6. Check the behavior of the device.
		/// 7. Change the parameters like Hostname, domain name and DNS server IP address from the DHCP scope
		/// 8. Now disable the stateless DHCPv4 checkbox on Europa wired interface and check the behavior of the options provided by the server
		/// 9. Change the config method from Manual to Manual
		/// 10.Verify the device behaviour
		/// Expected: 6.On enabling the DHCPv4 stateless checkbox, the configured IP parameters should be updated by the device from the DHCPv4 server using DHCP Inform Packet on Eurpa wired interfaces.
		/// this changes should not be impacted on the Embedded wired and Europa wireless interfaces
		/// 10.On disabling the DHCPv4 stateless checkbox, the device should not attain any IP parameters from the server.DHCP Inform packet should not send while changing manual to manaul. 
		/// Again disabling should not be impact the Embedded wired and Europa wireless interfaces."
		/// Step 11: 
		/// Keep all the interfaces UP and do Enable and disable the DHCPv4 stateless options using DHCP config method on Europa wireless interface.
		/// 1. Configure the  DHCP server in the isolated  network. 
		/// 2. Bring up the device in DHCP IP for Embedded Wired, Europa Wired and Wireless interfaces.
		/// 3. Configure a DHCPv4 server to supply IP parameters as Hostname, Domain Name and DNS server IP address
		/// 4  .Enable the stateless DHCPv4 option on Europa wireless interface.
		/// 5. Change the config method from DHCP to Manual on Europa wireless interface. 
		/// 6. Check the behavior of the device.
		/// 7. Change the parameters like Hostname, domain name and DNS server IP address from the DHCP scope
		/// 8. Now disable the stateless DHCPv4 checkbox on Europa wireless interface and check the behavior of the options provided by the server
		/// 9. Change the config method from Manual to Manual
		/// 10.Verify the device behaviour
		/// Expected: 6.On enabling the DHCPv4 stateless checkbox, the configured IP parameters should be updated by the device from the DHCPv4 server using DHCP Inform Packet on Eurpa wireless interfaces.
		/// this changes should not be impacted on the Embedded wired and Europa wired interfaces
		/// 10.On disabling the DHCPv4 stateless checkbox wireless, the device should not attain any IP parameters from the server.DHCP Inform packet should not send while changing manual to manaul. 
		/// Again disabling should not be impact the Embedded wired and Europa wired interfaces."
		/// Step 12: 
		/// Keep al the interfaces up and do Host break on the Europa wired interface from the same network	
		/// 1. Configure the DHCP server lease time as 10 mins 
		/// 2. Brin up the device in DHCP IP for Embedded Wired, Europa Wired and Wireless interfaces.
		/// 3. Remove the network cable of the Europa wired interface from DHCP server network and connect it back to the same DHCP server.
		/// 4. Verify the device behaviour along with parameters
		/// Expected: 2. All interfaces should come up with DHCP IP
		/// 4. Europa Interface should send DHCP release packet first and then send DHCP discover process after connecting back to the same DHCP Server and device should Acquire a DHCP IP Address. 
		/// Ping should be success after connecting to the new network.But Ebedded wired and Wirless interfaces should be retain with same DHCP IP.Embedded wired and Wireless IP also should be success ping.
		/// Europa interface should get updated with same Parameters from the same DHCP Server
		/// Step 13: 
		/// Keep al the interfaces up and do Host break on the Embedded wired interface from the same network
		/// 1. Configure the DHCP server lease time as 10 mins
		/// 2. Brin up the device in DHCP IP for Embedded Wired, Europa Wired and Wireless interfaces.
		/// 3. Remove the network cable of the Embedded wired interface from DHCP server network and connect it back to the same DHCP server.
		/// 4. Verify the device behaviour along with parameters  
		/// Expected: 2. All interfaces should come up with DHCP IP 
		/// 4. Embedded Interface should send DHCP release packet first and then send DHCP discover process  after connecting back to the same DHCP Server and device should Acquire a  DHCP IP Address. 
		/// Ping should be success after connecting to the new network.  But  Europa wired and Wirless interfaces should be retain with same DHCP IP.  Europa wired and Wireless IP also should be success ping. Europa interface should get updated with same Parameters from the same DHCP Server   
		/// </summary>
		/// <param name="activityData"></param>
		/// <param name="testNo"></param>
		/// <returns></returns>
		public static bool VerifyMultipleOperationsOnInterfaces(IPConfigurationActivityData activityData, int testNo)
		{
			Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails = GetInterfaceDetails(activityData);

			try
			{
				TraceFactory.Logger.Info("STEP I: Keep all the interfaces UP and do Reinitialize Now using DHCP IP on Secondary wired interface");

				if (!VerifyReinializeNow(activityData, IPConfigMethod.DHCP, PrinterConnectionType.SecondaryWiredInterface, "{0}_SecondaryWiredInterface".FormatWith(testNo)))
				{
					return false;
				}

				TraceFactory.Logger.Info("STEP 2: Keep all the interfaces UP and do Reinitialize Now using BOOTP IP on Secondary wireless interface");

				if (!VerifyReinializeNow(activityData, IPConfigMethod.BOOTP, PrinterConnectionType.WirelessInterface, "{0}_WirelessInterface".FormatWith(testNo)))
				{
					return false;
				}

				TraceFactory.Logger.Info("STEP 3: Keep all the interfaces UP and do Clear previous value and Reinitialize Now using DHCP IP for Europa Wired interfaces");

				if (!ClearPreviousValuesAndReinitializeNow(activityData, IPConfigMethod.DHCP, PrinterConnectionType.SecondaryWiredInterface, "{0}_SecondaryWiredInterface".FormatWith(testNo)))
				{
					return false;
				}

				TraceFactory.Logger.Info("STEP 4: Keep all the interfaces UP and do Clear previous value and Reinitialize Now using DHCP IP for Wireless interfaces");

				if (!ClearPreviousValuesAndReinitializeNow(activityData, IPConfigMethod.DHCP, PrinterConnectionType.WirelessInterface, "{0}_WirelessInterface".FormatWith(testNo)))
				{
					return false;
				}


				TraceFactory.Logger.Info("STEP 5: Keep all the interfaces UP and do config method change to Europa wired interfaces");

				if (!ValidateIPConfigMethodChangeToInterfaces(activityData, printerInterfaceDetails))
				{
					return false;
				}

				printerInterfaceDetails.ForEach(x => { x.ConfigMethod = (x.PrinterInterface == PrinterConnectionType.SecondaryWiredInterface) ? IPConfigMethod.DHCP : IPConfigMethod.BOOTP; });

				if (!ValidateIPConfigMethodChangeToInterfaces(activityData, printerInterfaceDetails))
				{
					return false;
				}

				TraceFactory.Logger.Info("STEP 6: Keep all the interfaces UP and do config method change to wireless interfaces");

				printerInterfaceDetails.ForEach(x => x.ConfigMethod = IPConfigMethod.DHCP);

				if (!ValidateIPConfigMethodChangeToInterfaces(activityData, printerInterfaceDetails))
				{
					return false;
				}

				printerInterfaceDetails.ForEach(x => { x.ConfigMethod = (x.PrinterInterface == PrinterConnectionType.WirelessInterface) ? IPConfigMethod.Manual : IPConfigMethod.DHCP; });

				if (!ValidateIPConfigMethodChangeToInterfaces(activityData, printerInterfaceDetails))
				{
					return false;
				}

				TraceFactory.Logger.Info("STEP 7: Keep all the interfaces UP and do IPv6 disable and Enable on Embedded wired interface");

				if (!ValidateIPv6DisableOnInterfaces(activityData, PrinterConnectionType.PrimaryWiredInterface))
				{
					return false;
				}

				TraceFactory.Logger.Info("STEP 8: Keep all the interfaces UP and do IPv6 disable and Enable on Europa Wireless interface");

				return ValidateIPv6DisableOnInterfaces(activityData, PrinterConnectionType.WirelessInterface);
			}
			finally
			{
				ConfigureInterface(printerInterfaceDetails);
			}
		}

		public static bool VerifyHoseBreakDHCPv4StatelessAddress(IPConfigurationActivityData activityData, int testNo)
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

			Collection<PrinterInterfaceConfigurationDetails> interfaceDetails = GetInterfaceDetails(activityData);

			try
			{

				TraceFactory.Logger.Info("STEP 1: Keep all the interfaces UP and do Enable and disable the DHCPv4 stateless options using DHCP config method on Europa wired interface.");

				if (!ValidateParameterWithStatelessOption(activityData, IPConfigMethod.DHCP, testNo, PrinterConnectionType.SecondaryWiredInterface))
				{
					return false;
				}

				TraceFactory.Logger.Info("STEP 2: Keep all the interfaces UP and do Enable and disable the DHCPv4 stateless options using DHCP config method on Europa wireless interface.");

				if (!ValidateParameterWithStatelessOption(activityData, IPConfigMethod.DHCP, testNo, PrinterConnectionType.WirelessInterface))
				{
					return false;
				}

				TraceFactory.Logger.Info("STEP 3: Keep al the interfaces up and do Host break on the Europa wired interface from the same network");


				TraceFactory.Logger.Info("Change lease time to 10 minute on DHCP server");

				DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
				dhcpClient.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 600);

				if (!VerifyHoseBreakSameSubnetParameters(activityData, PrinterConnectionType.PrimaryWiredInterface, testNo))
				{
					return false;
				}

				TraceFactory.Logger.Info("STEP 3: Keep al the interfaces up and do Hose break on the Embedded wired interface from the same network");

				return VerifyHoseBreakSameSubnetParameters(activityData, PrinterConnectionType.PrimaryWiredInterface, testNo);
			}
			finally
			{
				TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
				DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
				dhcpClient.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, DEFAULT_LEASE_TIME);

				interfaceDetails.ForEach(x => x.ConfigMethod = IPConfigMethod.BOOTP);
				ConfigureInterface(interfaceDetails);
			}
		}

		public static bool VerifyHoseBreakMultipleInterfaces(IPConfigurationActivityData activityData, int testNo)
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

			Collection<PrinterInterfaceConfigurationDetails> interfaceDetails = GetInterfaceDetails(activityData);

			try
			{
				interfaceDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.BOOTP; x.PrinterReservationType = ReservationType.Bootp; });

				if(!ConfigureInterface(interfaceDetails))
				{
					return false;
				}

				if (!HoseBreakSameNetwork(activityData, interfaceDetails, "{0}_Bootp_HoseBreakSameNetwork".FormatWith(testNo)))
				{
					return false;
				}

				interfaceDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.DHCP; x.PrinterReservationType = ReservationType.Dhcp; });

				if (!ConfigureInterface(interfaceDetails))
				{
					return false;
				}

				if (!HoseBreakSameNetwork(activityData, interfaceDetails, "{0}_DHCP_HoseBreakSameNetwork".FormatWith(testNo)))
				{
					return false;
				}

                return true;
			}
			finally
			{ }
		}

		/// <summary>
		/// Performs simultaneous hose break on wired interfaces.
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <param name="interfaceDetails"></param>
		/// <param name="packetFileName"></param>
		/// <returns></returns>
		private static bool HoseBreakSameNetwork(IPConfigurationActivityData activityData, Collection<PrinterInterfaceConfigurationDetails> interfaceDetails, string packetFileName)
		{
			PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
			string guid = client.Channel.TestCapture(activityData.SessionId, packetFileName);

			try
			{
				INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIP));

				foreach (var item in interfaceDetails.Where(x => x.PrinterInterface != PrinterConnectionType.WirelessInterface))
				{
					if (!networkSwitch.DisablePort(item.PortNumber))
					{
						return false;
					}
				}

				Thread.Sleep(TimeSpan.FromSeconds(30));

				foreach (var item in interfaceDetails.Where(x => x.PrinterInterface != PrinterConnectionType.WirelessInterface))
				{
					if (!networkSwitch.DisablePort(item.PortNumber))
					{
						return false;
					}
				}

				foreach (var item in interfaceDetails.Where(x => x.PrinterInterface != PrinterConnectionType.WirelessInterface))
				{
					if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(item.Address), TimeSpan.FromMinutes(1)))
					{
						TraceFactory.Logger.Info("Ping is successful with IP Address: {0} of {1}".FormatWith(item.Address, item.PrinterInterface));
					}
					else
					{
						TraceFactory.Logger.Info("Ping failed with IP Address: {0} of {1}".FormatWith(item.Address, item.PrinterInterface));
						return false;
					}
				}

                foreach (var item in interfaceDetails.Where(x => x.PrinterInterface != PrinterConnectionType.WirelessInterface))
                {
                    if (!ValidateServerParameters(activityData, item.Address))
                    {
                        return false;
                    }
                }

                return true;
			}
			finally
			{
				client.Channel.Stop(guid);
			}
		}

        private static bool ValidateServerParameters(IPConfigurationActivityData activityData, string address)
        {
            // Validating server parameters for Europa interface
            DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);
            string serverhostName = dhcpClient.Channel.GetHostName(activityData.PrimaryDhcpServerIPAddress, scope);
            string domainName = dhcpClient.Channel.GetDomainName(activityData.PrimaryDhcpServerIPAddress, scope);
            string primaryDnsServer = dhcpClient.Channel.GetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress, scope);
            string secondaryDnsServer = dhcpClient.Channel.GetSecondaryDnsServer(activityData.PrimaryDhcpServerIPAddress, scope);
            string primaryWinsServer = dhcpClient.Channel.GetPrimaryWinsServer(activityData.PrimaryDhcpServerIPAddress, scope);
            string secondaryWinsServer = dhcpClient.Channel.GetSecondaryWinsServer(activityData.PrimaryDhcpServerIPAddress, scope);

            if (!ValidateHostName(PrinterFamilies.VEP, serverhostName, address))
            {
                return false;
            }

            if (!ValidateDomainName(PrinterFamilies.VEP, domainName, address))
            {
                return false;
            }

            if (!ValidatePrimaryDnsServer(PrinterFamilies.VEP, primaryDnsServer, address))
            {
                return false;
            }

            if (!ValidateSecondaryDnsServer(PrinterFamilies.VEP, secondaryDnsServer, address))
            {
                return false;
            }

            if (!ValidatePrimaryWinsServer(PrinterFamilies.VEP, primaryWinsServer, address))
            {
                return false;
            }

            return ValidateSecondaryWinsServer(PrinterFamilies.VEP, secondaryWinsServer, address);
        }

		private static bool HoseBreakDifferentNetwork(IPConfigurationActivityData activityData, ref Collection<PrinterInterfaceConfigurationDetails> interfaceDetails)
		{
			TraceFactory.Logger.Info("Disconnecting the printer from source dhcp Server network");

			INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIP));

			foreach (var item in interfaceDetails)
			{
				// Disable the port on first VLAN
				if (networkSwitch.DisablePort(item.PortNumber))
				{
					TraceFactory.Logger.Info("Printer port: {0} is disconnected from the source dhcp server network.".FormatWith(item.PortNumber));
				}
				else
				{
					TraceFactory.Logger.Info("Failed to disconnect the printer port: {0} from source dhcp server network.".FormatWith(item.PortNumber));
					return false;
				}
			}

			foreach (var item in interfaceDetails)
			{
				int destinationVlan = (from vlan in networkSwitch.GetAvailableVirtualLans()
									   where (null != vlan.IPAddress) && vlan.IPAddress.IsInSameSubnet(IPAddress.Parse(item.destinationServerAddress))
									   select vlan.Identifier).FirstOrDefault();

				if (destinationVlan == 0)
				{
					TraceFactory.Logger.Info("No VLAN is found in the {0} network.".FormatWith(item.destinationServerAddress));
					return false;
				}
				// Change the port to second VLAN 
				if (!networkSwitch.ChangeVirtualLan(item.PortNumber, destinationVlan))
				{
					return false;
				}

				TraceFactory.Logger.Info("Connecting the printer port: {0} in destination dhcp server : {1} network.".FormatWith(item.PortNumber, item.destinationServerAddress));
				// Enable Port on second VLAN
				if (networkSwitch.EnablePort(item.PortNumber))
				{
					TraceFactory.Logger.Info("Printer port : {0} is connected to destination dhcp server : {1} network.".FormatWith(item.PortNumber, item.destinationServerAddress));
				}
				else
				{
					TraceFactory.Logger.Info("Failed to connect printer port: {0} to destination dhcp server : {1} network.".FormatWith(item.PortNumber, item.destinationServerAddress));
					return false;
				}
			}

			TraceFactory.Logger.Info("Waiting for 3 minute after performing hose break.");
			// Wait for 3 minute after performing hose break.
			Thread.Sleep(TimeSpan.FromMinutes(3));

			string address = string.Empty;

			foreach (var item in interfaceDetails.Where(x => x.PrinterInterface != PrinterConnectionType.WirelessInterface))
			{
				address = Utility.GetPrinterIPAddress(item.MacAddress);

				if (string.IsNullOrEmpty(address))
				{
					TraceFactory.Logger.Info("Printer with mac address: {0} failed to acquire IP address after hose break to {1} network.".FormatWith(item.MacAddress, item.destinationServerAddress));
					return false;
				}
				else
				{
					TraceFactory.Logger.Info("Printer with mac address: {0} acquire IP address { {1} after hose break to {2} network.".FormatWith(item.MacAddress, address, item.destinationServerAddress));
				}
			}

			return true;
		}

		private static bool VerifyHoseBreakSameSubnetParameters(IPConfigurationActivityData activityData, PrinterConnectionType interfaceConnection, int testNo)
		{
			if (!VerifyHoseBreakSameNetwork(activityData, IPConfigMethod.DHCP, testNo,PrinterConnectionType.SecondaryWiredInterface))
			{
				return false;
			}

			if (!Ping(GetInterfaceDetails(activityData)))
			{
				return false;
			}

			// Validating server parameters for Europa interface
			DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
			string scope = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);
			string serverhostName = dhcpClient.Channel.GetHostName(activityData.PrimaryDhcpServerIPAddress, scope);
			string domainName = dhcpClient.Channel.GetDomainName(activityData.PrimaryDhcpServerIPAddress, scope);
			string primaryDnsServer = dhcpClient.Channel.GetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress, scope);
			string secondaryDnsServer = dhcpClient.Channel.GetSecondaryDnsServer(activityData.PrimaryDhcpServerIPAddress, scope);
			string primaryWinsServer = dhcpClient.Channel.GetPrimaryWinsServer(activityData.PrimaryDhcpServerIPAddress, scope);
			string secondaryWinsServer = dhcpClient.Channel.GetSecondaryWinsServer(activityData.PrimaryDhcpServerIPAddress, scope);

			if (!ValidateHostName(PrinterFamilies.VEP, serverhostName))
			{
				return false;
			}

			if (!ValidateDomainName(PrinterFamilies.VEP, domainName))
			{
				return false;
			}

			if (!ValidatePrimaryDnsServer(PrinterFamilies.VEP, primaryDnsServer))
			{
				return false;
			}

			if (!ValidateSecondaryDnsServer(PrinterFamilies.VEP, secondaryDnsServer))
			{
				return false;
			}

			if (!ValidatePrimaryWinsServer(PrinterFamilies.VEP, primaryWinsServer))
			{
				return false;
			}

			return ValidateSecondaryWinsServer(PrinterFamilies.VEP, secondaryWinsServer);
		}

		/// <summary>
		/// 1. Configure the  DHCP server in the isolated  network. 
		/// 2. Bring up the device in DHCP IP for Embedded Wired, Europa Wired and Wireless interfaces.
		/// 3. Disable IPv6 on the Embedded wired interface
		/// 4. Verify the Devie behaviour 
		/// 5.Enable the Europa Embedded interface
		/// 6. Verify the Devie behaviour
		/// Expected:
		/// 2. All the interface should come up with DHCP IP. 
		/// 4. Disabling IPv6 option on the Embedded wired interfaces should disable the IPV6 address and IPv4 should not impact on the Europa wired interface.
		/// But still the Europa wired interface and wireless interfaces have IPv6 option enabled and able to ping all the ipv6 address.
		/// 5. Enabling IPv6 option on the Embedded wired interface, linklocal, and stateful ipv6 address should come up.The same ipv6 address able to ping.
		/// </summary>
		/// <param name="activityData"></param>
		/// <returns></returns>
		private static bool ValidateIPv6DisableOnInterfaces(IPConfigurationActivityData activityData, PrinterConnectionType interfaceConnection)
		{
			Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails = GetInterfaceDetails(activityData);
			printerInterfaceDetails.ForEach(x => { x.ConfigMethod = IPConfigMethod.DHCP; x.PrinterReservationType = ReservationType.Dhcp; });

			if (!ConfigureInterface(printerInterfaceDetails))
			{
				return false;
			}

			Collection<IPv6Details> ipv6Details = null;
			IPAddress linkLocalAddress = null;
			Collection<IPAddress> statelessAddresses = null;

			// Fetch and keep the IPv6 addresses for later use
			foreach (var item in printerInterfaceDetails)
			{
				EwsWrapper.Instance().ChangeDeviceAddress(item.Address);

				ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(5);

				linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);
				statelessAddresses = IPv6Utils.GetStatelessIPAddress(ipv6Details);

				item.StatelessAddresses = statelessAddresses;
				item.LinkLocalAddress = linkLocalAddress;
			}

			// Fetch the ip address of the required interface
			string address = printerInterfaceDetails.FirstOrDefault(x => x.PrinterInterface == interfaceConnection).Address;
			EwsWrapper.Instance().ChangeDeviceAddress(address);

			// Disable IPV6 on the specified interface
			if(!EwsWrapper.Instance().SetIPv6(false))
			{
				return false;
			}

			if(!VerifyIPv6AddressStatus(0, false))
			{
				return false;
			}

			if(NetworkUtil.PingUntilTimeout(IPAddress.Parse(address), TimeSpan.FromMinutes(1)))
			{
				TraceFactory.Logger.Info("Ping is successful with IPv4 address: {0} of {1}".FormatWith(address, interfaceConnection));
			}
			else
			{
				TraceFactory.Logger.Info("Ping failed with IPv4 address: {0} of {1}".FormatWith(address, interfaceConnection));
				return false;
			}

			foreach (var item in printerInterfaceDetails.Where(x => x.PrinterInterface != interfaceConnection))
			{
				TraceFactory.Logger.Info(CtcBaseTests.INTERFACE_HIGHLIGHTER.FormatWith(item.PrinterInterface));

				EwsWrapper.Instance().ChangeDeviceAddress(address);

				if (EwsWrapper.Instance().GetIPv6())
				{
					TraceFactory.Logger.Info("IPv6 option is enabled on {0}. Expected: IPv6 option enabled.".FormatWith(item.PrinterInterface));
				}
				else
				{ 
					TraceFactory.Logger.Info("IPv6 option is not enabled on {0}. Expected: IPv6 option enabled.".FormatWith(item.PrinterInterface));
					return false;
				}

				foreach (IPAddress ipAddress in item.StatelessAddresses)
				{
					if(NetworkUtil.PingUntilTimeout(ipAddress, TimeSpan.FromMinutes(1)))
					{
						TraceFactory.Logger.Info("Ping is successful with IPv6 address: {0}".FormatWith(ipAddress));
					}
					else
					{
						TraceFactory.Logger.Info("Ping failed with IPv6 address: {0}".FormatWith(ipAddress));
						return false;
					}

					if (NetworkUtil.PingUntilTimeout(item.LinkLocalAddress, TimeSpan.FromMinutes(1)))
					{
						TraceFactory.Logger.Info("Ping is successful with link local address: {0}".FormatWith(item.LinkLocalAddress));
					}
					else
					{
						TraceFactory.Logger.Info("Ping failed with link local address: {0}".FormatWith(item.LinkLocalAddress));
						return false;
					}
				}
			}

			EwsWrapper.Instance().ChangeDeviceAddress(address);

			// Disable IPV6 on the specified interface
			if (!EwsWrapper.Instance().SetIPv6(true))
			{
				return false;
			}

			if (!VerifyIPv6AddressStatus(4, false))
			{
				return false;
			}

			ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(5);

			linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);
			statelessAddresses = IPv6Utils.GetStatelessIPAddress(ipv6Details);

			foreach (IPAddress ipAddress in statelessAddresses)
			{
				if (NetworkUtil.PingUntilTimeout(ipAddress, TimeSpan.FromMinutes(1)))
				{
					TraceFactory.Logger.Info("Ping is successful with IPv6 address: {0}".FormatWith(ipAddress));
				}
				else
				{
					TraceFactory.Logger.Info("Ping failed with IPv6 address: {0}".FormatWith(ipAddress));
					return false;
				}
			}

			if (NetworkUtil.PingUntilTimeout(linkLocalAddress, TimeSpan.FromMinutes(1)))
			{
				TraceFactory.Logger.Info("Ping is successful with link local address: {0}".FormatWith(linkLocalAddress));
				return true;
			}
			else
			{
				TraceFactory.Logger.Info("Ping failed with link local address: {0}".FormatWith(linkLocalAddress));
				return false;
			}
		}

		private static bool ValidateIPConfigMethodChangeToInterfaces(IPConfigurationActivityData activityData, Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails, string packetFileName = "", bool reserveIPAddress = false)
		{
			PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
			string guid = client.Channel.TestCapture(activityData.SessionId, packetFileName);

			try
			{
				if (!ConfigureInterface(printerInterfaceDetails, reserverAddress: reserveIPAddress))
				{
					return false;
				}

				if (!Ping(printerInterfaceDetails))
				{
					return false;
				}

				return ValidateIPConfigMethod(activityData, printerInterfaceDetails);
			}
			finally
			{
				client.Channel.Stop(guid);
			}
		}

        private static Collection<PrinterInterfaceConfigurationDetails> GetInterfaceDetails(IPConfigurationActivityData activityData)
        {
            Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails = new Collection<PrinterInterfaceConfigurationDetails>();
            PrinterInterfaceConfigurationDetails interfaceConfigurationDetails = new PrinterInterfaceConfigurationDetails();
            interfaceConfigurationDetails.Address = activityData.PrimaryWiredIPv4Address;
            interfaceConfigurationDetails.MacAddress = activityData.PrimaryMacAddress;
            interfaceConfigurationDetails.PrinterInterface = PrinterConnectionType.PrimaryWiredInterface;
            interfaceConfigurationDetails.PortNumber = activityData.SecondaryInterfacePortNo;
            printerInterfaceDetails.Add(interfaceConfigurationDetails);

            if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.VEP.ToString()) && activityData.PrinterInterfaceType == CtcBase.Controls.InterfaceType.Single)
            {
                IPAddress address = null;
                if (IPAddress.TryParse(activityData.SecondaryWiredIPv4Address, out address))
                {
                    interfaceConfigurationDetails = new PrinterInterfaceConfigurationDetails();
                    interfaceConfigurationDetails.Address = activityData.SecondaryWiredIPv4Address;
                    interfaceConfigurationDetails.MacAddress = activityData.SecondaryMacAddress;
                    interfaceConfigurationDetails.PrinterInterface = PrinterConnectionType.SecondaryWiredInterface;
                    interfaceConfigurationDetails.PortNumber = activityData.SecondaryInterfacePortNo;
                    printerInterfaceDetails.Add(interfaceConfigurationDetails);
                }

                if (IPAddress.TryParse(activityData.WirelessIPv4Address, out address))
                {
                    interfaceConfigurationDetails = new PrinterInterfaceConfigurationDetails();
                    interfaceConfigurationDetails.Address = activityData.WirelessIPv4Address;
                    interfaceConfigurationDetails.MacAddress = activityData.WirelessMacAddress;
                    interfaceConfigurationDetails.PrinterInterface = PrinterConnectionType.WirelessInterface;
                    interfaceConfigurationDetails.PortNumber = activityData.WirelessInterfacePortNo;
                    printerInterfaceDetails.Add(interfaceConfigurationDetails);
                }
            }

            return printerInterfaceDetails;
        }

        private static bool ConfigureInterface( Collection<PrinterInterfaceConfigurationDetails> configurationDetails, bool reserverAddress = true, bool changeConfigMethod = true)
        {
            if (reserverAddress)
            {
                string dhcpServerIpAddress = string.Empty;

                foreach (var item in configurationDetails)
                {
                    TraceFactory.Logger.Info(Utility.WriteStep("Creating reservation for {0}".FormatWith(item.PrinterInterface)));
                    dhcpServerIpAddress = IPConfigurationConfigurationControl.DHCP_SERVER_IP_FORMAT.FormatWith(item.Address.ToString().Split(new char[] { '.' })[2]);

                    try
                    {
                        using (DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(dhcpServerIpAddress))
                        {
                            dhcpClient.Channel.DeleteReservation(dhcpServerIpAddress, dhcpClient.Channel.GetDhcpScopeIP(dhcpServerIpAddress), item.Address.ToString(), item.MacAddress);

                            if (dhcpClient.Channel.CreateReservation(dhcpServerIpAddress, dhcpClient.Channel.GetDhcpScopeIP(dhcpServerIpAddress), item.Address.ToString(), item.MacAddress, item.PrinterReservationType))
                            {
                                TraceFactory.Logger.Info("Successfully created reservation for IP address: {0} with Mac Address: {1} in server: {2} for {3}".FormatWith(item.Address, item.MacAddress, dhcpServerIpAddress, item.PrinterReservationType));
                            }
                            else
                            {
                                TraceFactory.Logger.Info("Failed to create reservation for IP address: {0} with Mac Address: {1} in server: {2} for {3}".FormatWith(item.Address, item.MacAddress, dhcpServerIpAddress, item.PrinterReservationType));
                                return false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TraceFactory.Logger.Info("Failed to create reservation for IP address: {0} with Mac Address: {1} in server: {2} for {3}".FormatWith(item.Address, item.MacAddress, dhcpServerIpAddress, item.PrinterReservationType));
                        return false;
                    }
                }                
            }

            if (changeConfigMethod)
            {
                foreach (var item in configurationDetails)
                {
                    TraceFactory.Logger.Info(Utility.WriteStep("Setting IP config method to: {0} for {1}".FormatWith(item.ConfigMethod, item.PrinterInterface)));

                    EwsWrapper.Instance().ChangeDeviceAddress(item.Address);

                    if (!EwsWrapper.Instance().SetIPConfigMethod(item.ConfigMethod, expectedIPAddress: IPAddress.Parse(item.Address)))
                    {
                        return false;
                    }
                }
            }

            EwsWrapper.Instance().ChangeDeviceAddress(configurationDetails.Where( x => x.PrinterInterface == PrinterConnectionType.PrimaryWiredInterface).FirstOrDefault().Address);

            return true;
        }

        private static bool ValidateIPConfigMethod(IPConfigurationActivityData activityData, Collection<PrinterInterfaceConfigurationDetails> configurationDetails)
        {
            bool result = true;

            configurationDetails.ForEach(x =>
            {
                TraceFactory.Logger.Info(CtcBaseTests.STEP_DELIMETER.FormatWith(x.PrinterInterface));
                TraceFactory.Logger.Info(Utility.WriteStep("Validating IP Config Method"));
                result &= ValidateIPConfigMethod(activityData, x.ConfigMethod, x.Address);
            });

            EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
            SnmpWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);
            TelnetWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);

            return result;
        }

        private static bool Ping(Collection<PrinterInterfaceConfigurationDetails> configurationDetails)
        {
            bool result = true;

            foreach (var item in configurationDetails)
            {
                TraceFactory.Logger.Info(CtcBaseTests.INTERFACE_HIGHLIGHTER);
                TraceFactory.Logger.Info("Checking ping status for ipv4 address.");
                result &= CheckForPrinterAvailability(item.Address);

                if(item.LinkLocalAddress != IPAddress.None)
                {
                    TraceFactory.Logger.Info("Checking ping status for Link local address.");
                    result &= CheckForPrinterAvailability(item.LinkLocalAddress.ToString(), TimeSpan.FromMinutes(1));
                }

                if(item.StatelessAddresses.Count != 0)
                {
                    TraceFactory.Logger.Info("Checking ping status for stateless address.");
                    item.StatelessAddresses.ForEach(x => result &= CheckForPrinterAvailability(x.ToString(), TimeSpan.FromMinutes(1)));
                }

                if (item.StatefullAddress != IPAddress.None)
                {
                    TraceFactory.Logger.Info("Checking ping status for stateful address.");
                    result &= CheckForPrinterAvailability(item.StatefullAddress.ToString(), TimeSpan.FromMinutes(1));
                }
            }

            return result;
        }

        private static bool ValidateLeaseTime(IPConfigurationActivityData activityData, Collection<PrinterInterfaceConfigurationDetails> configurationDetails)
        {
            bool result = true;
            configurationDetails.ForEach(x => 
            {
                TraceFactory.Logger.Info(CtcBaseTests.INTERFACE_HIGHLIGHTER.FormatWith(x.PrinterInterface));
                TraceFactory.Logger.Info("Validating lease time for {0} interface".FormatWith(x.PrinterInterface)); SnmpWrapper.Instance().Create(x.Address); result &= ValidateLeaseTime(activityData.ProductFamily, x.LeaseTime);
            });

            SnmpWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);
            return result;
        }

        private static bool IsPrinterInAutoIP(ref Collection<PrinterInterfaceConfigurationDetails> configurationDetails)
        {
            bool result = true;
            string autoIPAddress = string.Empty;

            foreach (var item in configurationDetails)
            {
                TraceFactory.Logger.Info(CtcBaseTests.INTERFACE_HIGHLIGHTER.FormatWith(item.PrinterInterface));
                TraceFactory.Logger.Info(Utility.WriteStep("Checking Auto IP status for : {0}".FormatWith(item.PrinterInterface)));

                // Wireless interface is not getting discovered when in auto ip. So using link local address to verify auto ip.
                if (item.PrinterInterface == PrinterConnectionType.WirelessInterface)
                {
                    // Ping Link local ip address first and then access the web page to read the config method
                    NetworkUtil.PingUntilTimeout(item.LinkLocalAddress, TimeSpan.FromMinutes(1));
                    EwsWrapper.Instance().ChangeDeviceAddress(item.LinkLocalAddress);

                    if (EwsWrapper.Instance().GetIPConfigMethod() == IPConfigMethod.AUTOIP)
                    {
                        TraceFactory.Logger.Info("{0} acquired Auto IP.".FormatWith(item.PrinterInterface));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("{0} failed to acquire Auto IP.".FormatWith(item.PrinterInterface));
                        result &= false;
                    }

                    item.Address = EwsWrapper.Instance().GetIPv4Address();
                }
                else
                {
                    if (Utility.IsPrinterInAutoIP(item.MacAddress, out autoIPAddress))
                    {
                        TraceFactory.Logger.Info("{0} acquired Auto IP.".FormatWith(item.PrinterInterface));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("{0} failed to acquire Auto IP.".FormatWith(item.PrinterInterface));
                        result &= false;
                    }

                    item.Address = autoIPAddress;
                }
            }

            EwsWrapper.Instance().ChangeDeviceAddress(configurationDetails.Where(x => x.PrinterInterface == PrinterConnectionType.PrimaryWiredInterface).FirstOrDefault().Address);
            return result;
        }

        private static bool ConfigureInterfaceSubnets(IPConfigurationActivityData activityData, ref Collection<PrinterInterfaceConfigurationDetails> configurationDetails, bool requireSameSubnet = true)
        {
            bool isInterfaceInSameSubnet = false;
            string destinationAddress = string.Empty;
            string newAddress = string.Empty;

            INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIP));

            if (requireSameSubnet)
            {
                // Keep secondary interface in primary network
                if(!networkSwitch.DisablePort(activityData.SecondaryInterfacePortNo))
                {
                    return false;
                }

                int vlanId = GetVlanNumber(activityData, activityData.PrimaryDhcpServerIPAddress);

                if (!networkSwitch.ChangeVirtualLan(activityData.SecondaryInterfacePortNo, vlanId))
                {
                    return false;
                }

                if (!networkSwitch.DisablePort(activityData.SecondaryInterfacePortNo))
                {
                    return false;
                }

                // Primary interface is in linux, so deleting reservation only for secondary interface
                    DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress);
                    string scopeIP = serviceMethod.Channel.GetDhcpScopeIP(activityData.SecondDhcpServerIPAddress);

                    serviceMethod.Channel.DeleteReservation(destinationAddress, scopeIP, configurationDetails.FirstOrDefault(x => x.PrinterInterface == PrinterConnectionType.SecondaryWiredInterface).Address, configurationDetails.FirstOrDefault(x => x.PrinterInterface == PrinterConnectionType.SecondaryWiredInterface).MacAddress);

                // Printer is in different subnet and needs to be placed in same subnet. Used for post requisite
                // linux post requiste for primary interface

                LinuxPostRequisites(activityData);

                // After post requisites printer interface details will be same as original.
                configurationDetails = GetInterfaceDetails(activityData);

                return Ping(configurationDetails);
            }
            else
            {
                foreach (var item in configurationDetails.Where(x => x.PrinterInterface != PrinterConnectionType.WirelessInterface))
                {
                    //Check if the printers wireless and primary wired interfaces are in same subnet
                    isInterfaceInSameSubnet = IPAddress.Parse(item.Address).IsInSameSubnet(IPAddress.Parse(activityData.WirelessIPv4Address));

                    if (isInterfaceInSameSubnet)
                    {
                        TraceFactory.Logger.Info("Printer's {0} and wireless interfaces are connected to same subnet.".FormatWith(item.PrinterInterface));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Printer's {0} and wireless interfaces are connected to different subnet.".FormatWith(item.PrinterInterface));
                    }

                    if (isInterfaceInSameSubnet == requireSameSubnet)
                    {
                        continue;
                    }

                    // if interfaces are in different subnet, bring primary wired and secondary wired interface to wireless network vlan.
                    // If in different subnet, keep secondary wired interface in secondary dhcp server network, primary interface in linux server network.
                    destinationAddress = (!isInterfaceInSameSubnet && requireSameSubnet) ? activityData.WirelessIPv4Address : ((item.PrinterInterface == PrinterConnectionType.PrimaryWiredInterface) ? activityData.LinuxServerIPAddress : activityData.SecondDhcpServerIPAddress);

                    // Check if the printer wired and wireless interfaces are in the same subnet and change the embedded wired interface and external wired interface subnet accordingly.
                    if (isInterfaceInSameSubnet && !requireSameSubnet) // Printer is in same subnet and needs to be placed in different subnet
                    {
                        if (item.PrinterInterface == PrinterConnectionType.PrimaryWiredInterface)
                        {
                            IPAddress linuxAddress = IPAddress.None;

                            if (LinuxPreRequisites(activityData, item.ConfigMethod == IPConfigMethod.DHCP ? LinuxServiceType.DHCP : LinuxServiceType.BOOTP, ref linuxAddress))
                            {
                                item.Address = linuxAddress.ToString();
                            }
                        }
                        else
                        {
                            if (!ChangeVlan(activityData, item.PortNumber, destinationAddress, item.MacAddress, out newAddress))
                            {
                                return false;
                            }

                            item.Address = newAddress;

                            if (!IPAddress.Parse(destinationAddress).Equals(IPAddress.Parse(activityData.LinuxServerIPAddress)))
                            {
                                DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(destinationAddress);
                                string scopeIP = serviceMethod.Channel.GetDhcpScopeIP(destinationAddress);

                                serviceMethod.Channel.DeleteReservation(destinationAddress, scopeIP, item.Address, item.MacAddress);

                                if (serviceMethod.Channel.CreateReservation(destinationAddress, scopeIP, item.Address, item.MacAddress, ReservationType.Both))
                                {
                                    TraceFactory.Logger.Info("Successfully created reservation for IP address: {0} with mac address: {1} in dhcp server: {2} for both dhcp and bootp".FormatWith(item.Address, item.MacAddress, destinationAddress));
                                    return true;
                                }
                                else
                                {
                                    TraceFactory.Logger.Info("Failed to create reservation for IP address: {0} with mac address: {1} in dhcp server: {2} for both dhcp and bootp".FormatWith(item.Address, item.MacAddress, destinationAddress));
                                    return false;
                                }
                            }
                        }
                    }
                }

                return true;
            }
        }


        private static bool ChangeVlan(IPConfigurationActivityData activityData, int portNo, string destinationAddress, string macAddress, out string ipAddress)
        {
            ipAddress = string.Empty;

            int vlanNo = GetVlanNumber(activityData, destinationAddress);

            INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIP));

            networkSwitch.DisablePort(portNo);
            networkSwitch.ChangeVirtualLan(portNo, vlanNo);
            networkSwitch.EnablePort(portNo);

            Thread.Sleep(TimeSpan.FromMinutes(3));

            ipAddress = string.IsNullOrEmpty(ipAddress) ? Utility.GetPrinterIPAddress(macAddress) : ipAddress;

            if (!string.IsNullOrEmpty(ipAddress))
            {
                TraceFactory.Logger.Info("Printer acquired ip address: {0} after changing the vlan: {1}.".FormatWith(ipAddress, vlanNo));
            }
            else
            {
                TraceFactory.Logger.Info("Printer failed to acquire ip address after changing the vlan: {0}.".FormatWith(vlanNo));
                return false;
            }

            return CheckForPrinterAvailability(ipAddress, TimeSpan.FromMinutes(1));
        }

		private static bool PreRequisitesMultipleInterfaces(IPConfigurationActivityData activityData)
		{
			Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails = GetInterfaceDetails(activityData);

			if (!ConfigureInterface(printerInterfaceDetails))
			{
				return false;
			}

			foreach (var item in printerInterfaceDetails)
			{
				if(!NetworkUtil.PingUntilTimeout(IPAddress.Parse(item.Address), TimeSpan.FromMinutes(1)))
				{
					TraceFactory.Logger.Info("Ping failed for {0} with IP address: {1}".FormatWith(item.PrinterInterface, item.Address));
					return false;
				}
			}

			TraceFactory.Logger.Info("Ping is successful for all the interfaces.");
			return true;
		}

        private static bool CreateReservation(string dhcpServerAddress, string scope, string ipAddress, string macAddress)
        {
            DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(dhcpServerAddress);

            // Delete Reservation is not validated to handle cases where reservation is not present
            dhcpClient.Channel.DeleteReservation(dhcpServerAddress, scope, ipAddress, macAddress);

            if (dhcpClient.Channel.CreateReservation(dhcpServerAddress, scope, ipAddress, macAddress, ReservationType.Both))
            {
                TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP for IP Address: {0} Mac Address: {1}: Succeeded".FormatWith(ipAddress, macAddress));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Printer IP Address Reservation in DHCP Server for both DHCP and BOOTP: Failed");
                return false;
            }
        }

        private static bool VerifyDuplicateIpManual(IPConfigurationActivityData activityData, Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails, PrinterConnectionType secondInterface = PrinterConnectionType.SecondaryWiredInterface, bool cleanUp = true )
        {
            try
            {
                TraceFactory.Logger.Info(Utility.WriteStep("Step Ia: Bring up the device in {0} for Embedded Wired, Europa Wired and WirelessInterface interfaces".FormatWith(printerInterfaceDetails.First().ConfigMethod)));

                if (!ConfigureInterface(printerInterfaceDetails))
                {
                    return false;
                }

                TraceFactory.Logger.Info(Utility.WriteStep("Step Ib: Assigning embedded wired IP manually for the {0}".FormatWith(secondInterface)));

                string address = printerInterfaceDetails.Where(x => x.PrinterInterface == secondInterface).FirstOrDefault().Address;
                EwsWrapper.Instance().ChangeDeviceAddress(address);

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, IPAddress.Parse(activityData.PrimaryWiredIPv4Address), validate: false))
                {
                    return false;
                }

                Thread.Sleep(TimeSpan.FromMinutes(5));

                // Validate only for the interfaces where the duplicate manual ip is assigned.
                foreach (var item in printerInterfaceDetails.Where(x => x.PrinterInterface != (secondInterface == PrinterConnectionType.SecondaryWiredInterface ? PrinterConnectionType.WirelessInterface : PrinterConnectionType.SecondaryWiredInterface)))
                {
                    TraceFactory.Logger.Info(CtcBaseTests.INTERFACE_HIGHLIGHTER.FormatWith(item.PrinterInterface));
                    // Ping link local address continuously 
                    NetworkUtil.PingUntilTimeout(item.LinkLocalAddress, TimeSpan.FromMinutes(1));
                    EwsWrapper.Instance().ChangeDeviceAddress(item.LinkLocalAddress);

                    if (!EwsWrapper.Instance().ValidateDuplicateIP())
                    {
                        return false;
                    }
                }

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
                return true;
            }
            finally
            {
                if (cleanUp)
                {
                    printerInterfaceDetails.Where(x => x.PrinterInterface != (secondInterface == PrinterConnectionType.SecondaryWiredInterface ? PrinterConnectionType.WirelessInterface : PrinterConnectionType.SecondaryWiredInterface)).ForEach(x =>
                    {
                        // Ping link local address continuously 
                        NetworkUtil.PingUntilTimeout(x.LinkLocalAddress, TimeSpan.FromMinutes(1));
                        EwsWrapper.Instance().ChangeDeviceAddress(x.LinkLocalAddress);
                        EwsWrapper.Instance().SetIPConfigMethod(x.ConfigMethod, expectedIPAddress: IPAddress.Parse(x.Address));
                    });

                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
                }
            }
        }

		private static bool VerifyIPv6AddressStatus(int statelessAddressCount, bool statefulAddressPresence)
		{
			Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(statelessAddressCount + 1 + (statefulAddressPresence ? 1 : 0));

			IPAddress statefullAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
			IPAddress linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);
			Collection<IPAddress> statelessAddresses = IPv6Utils.GetStatelessIPAddress(ipv6Details);

			if (statefullAddress.Equals(IPAddress.IPv6None))
			{
				TraceFactory.Logger.Info("Printer didnot acquire the DHCPv6.");
				if(statefulAddressPresence) { return false; }
			}
			else
			{
				TraceFactory.Logger.Info("Printer acquired DHCPv6 address.");
				if (!statefulAddressPresence) { return false; }
			}

			if (linkLocalAddress.Equals(IPAddress.IPv6None))
			{
				TraceFactory.Logger.Info("Printer did not acquire link local address.");
				return false;
			}
			else
			{
				TraceFactory.Logger.Info("Printer acquired link local address : {0} as expected.".FormatWith(linkLocalAddress));
			}

			if (statelessAddresses.Count == statelessAddressCount)
			{
				TraceFactory.Logger.Info("Printer has acquired {0} stateless addresses.".FormatWith(statelessAddressCount));
				TraceFactory.Logger.Debug("Stateless addresses: {0}".FormatWith(string.Join(",", statelessAddresses)));
				return true;
			}
			else
			{
				TraceFactory.Logger.Info("Printer didn't acquire {0} stateless addresses as expected. Current count: {1}".FormatWith(statelessAddressCount, statelessAddresses.Count));
				TraceFactory.Logger.Debug("Stateless addresses: {0}".FormatWith(string.Join(",", statelessAddresses)));
				return false;
			}
		}

        /// <summary>
        /// Verify if the printer acquired same ip address after operations like power cycle, cold reset etc.
        /// </summary>
        /// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
        /// <param name="configurationDetails">A collectiopn of <see cref="PrinterInterfaceConfigurationDetails"/></param>
        /// <returns>True if the printer acquired same ip address after specific operations, else false.</returns>
        private static bool VerifySameIPAcquisition(IPConfigurationActivityData activityData, Collection<PrinterInterfaceConfigurationDetails> configurationDetails)
        {
            foreach (var item in configurationDetails)
            {
                EwsWrapper.Instance().ChangeDeviceAddress(item.Address);

                if (EwsWrapper.Instance().ValidateMacAddress(item.MacAddress.ToUpper()))
                {
                    TraceFactory.Logger.Info("Printer retained the IP Address : {0}.".FormatWith(item.Address));
                }
                else
                {
                    TraceFactory.Logger.Info("Printer failed to retain the IP Address : {0}.".FormatWith(item.Address));
                    return false;
                }
            }

            EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
            return true;
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// ColdReset the product
        /// </summary>
        /// <param name="activityData">ActivityData</param>
        /// <param name="printer">printer object to call cold reset function</param>
        /// <returns>true/false</returns>
        private static bool ColdReset(IPConfigurationActivityData activityData, Printer.Printer printer, int timeOut = 0)
		{
			if (!PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily))
			{
				if (0 == timeOut)
				{
					return printer.ColdReset();
				}
				else
				{
					return printer.ColdReset(timeOut);
				}
			}
			else
			{
				IPAddress linkLocalAddress = printer.IPv6LinkLocalAddress;

				if (0 == timeOut)
				{
					printer.ColdReset();
				}
				else
				{
					printer.ColdReset(timeOut);
				}

				// WirelessInterface configuration will be removed on cold reset, configuring wireless using wired ip address.
				if (activityData.PrinterConnectivity.Equals(ConnectivityType.Wireless))
				{
					INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIP));

					#region Enable Switch Port

					if (networkSwitch.EnablePort(activityData.PrimaryInterfacePortNo))
					{
						TraceFactory.Logger.Info("Enabled Port# {0} to fetch wired ip address to configure wireless back after cold reset".FormatWith(activityData.PrimaryInterfacePortNo));
					}
					else
					{
						TraceFactory.Logger.Info("Make sure valid switch port is provided");
						TraceFactory.Logger.Info("Failed to enable port# {0} to fetch wired ip address to configure wireless back after cold reset".FormatWith(activityData.PrimaryInterfacePortNo));
						return false;
					}

					string wiredAddress = string.Empty;
					string hostName = string.Empty;

					BacaBodWrapper.DiscoverDevice(activityData.WirelessMacAddress, BacaBodSourceType.MacAddress, BacaBodDiscoveryType.All, ref wiredAddress, ref hostName);

					if (!CheckForPrinterAvailability(wiredAddress, TimeSpan.FromMinutes(2)))
					{
						return false;
					}

					// To make sure all stack is up and active, waiting for a while
					Thread.Sleep(TimeSpan.FromMinutes(1));

					#endregion

					#region Configure Wireless

					// Changing device address to wired ip to configure wireless
					EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(wiredAddress));

					if (EwsWrapper.Instance().ConfigureWireless(activityData.SsidName))
					{
						TraceFactory.Logger.Info("Successfully Re-configured WirelessInterface on the device after cold reset using the SSID:{0}".FormatWith(activityData.SsidName));
					}
					else
					{
						TraceFactory.Logger.Info("Failed to Re-configure WirelessInterface on the device after cold reset using the SSID:{0}".FormatWith(activityData.SsidName));
						return false;
					}

					#endregion

					#region Disable Switch Port

					if (networkSwitch.DisablePort(activityData.PrimaryInterfacePortNo))
					{
						TraceFactory.Logger.Info("Disabled Port# {0} to retrieve back wireless ip address after cold reset".FormatWith(activityData.PrimaryInterfacePortNo));
					}
					else
					{
						TraceFactory.Logger.Info("Please make sure that you are configured a managed switch with proper port number as input through UI");
						TraceFactory.Logger.Info("Failed to disable port# {0} to retrieve back wireless ip address after cold reset".FormatWith(activityData.PrimaryInterfacePortNo));
						return false;
					}

					if (!CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address, TimeSpan.FromMinutes(4)))
					{
						return false;
					}

					EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

					#endregion
				}

				// IP may change after cold reset, using link local address to set the telnet option
				if(CheckForPrinterAvailability(printer.WiredIPv4Address.ToString()))
				{
					EwsWrapper.Instance().ChangeDeviceAddress(printer.WiredIPv4Address);
				}
				else
				{
					CheckForPrinterAvailability(linkLocalAddress.ToString());
					EwsWrapper.Instance().ChangeDeviceAddress(linkLocalAddress.ToString());
				}

				// Telnet option will be disabled after performing cold reset, enabling the same.
				EwsWrapper.Instance().SetTelnet();
			}

			return true;
		}

		/// <summary>
		/// Creates reservation in the specified DHCP Server for the specified IP Config Method
		/// </summary>
		/// <param name="dhcpServer">The DHCP server IP Address where the reservation needs to be created.</param>
		/// <param name="printerMacAddress">The MAC Address of the printer.</param>
		/// <param name="reservationType"><see cref="ReservationType"/></param>
		/// <param name="reservedIpAddress">The IP Address to be reserved.</param>
		/// <returns>True if the reservation is successful, else false.</returns>
		private static bool CreateReservation(string dhcpServer, string printerMacAddress, ReservationType reservationType, ref string reservedIpAddress, string scopeIpAddress = null)
		{
			DhcpApplicationServiceClient serviceMethods = DhcpApplicationServiceClient.Create(dhcpServer);

			// Get the scope Ip of the second DHCP Server and create a reservation with a next pingable IP with dhcp/Bootp/Both based on the tests.
			// If the ip is not mentioned fetch the next free ip address from the server
			if (string.IsNullOrEmpty(reservedIpAddress))
			{
				scopeIpAddress = serviceMethods.Channel.GetDhcpScopeIP(dhcpServer);
				// Ignore the router IP Address and fetch the next ping able ip.
				reservedIpAddress = Network.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(dhcpServer).GetSubnetMask(), Network.NetworkUtil.FetchNextIPAddress(IPAddress.Parse(dhcpServer).GetSubnetMask(), IPAddress.Parse(scopeIpAddress))).ToString();
			}
			// TODO: Check if the ip is already reserved and if it is already reserved delete the reservation and create a new reservation.

			// DeleteResrvation
			serviceMethods.Channel.DeleteReservation(dhcpServer,scopeIpAddress, reservedIpAddress.ToString(), printerMacAddress);
			// Create Reservation
			if (!serviceMethods.Channel.CreateReservation(dhcpServer, scopeIpAddress, reservedIpAddress.ToString(), printerMacAddress, reservationType))
			{
				TraceFactory.Logger.Info("Failed to Create reservation in DHCP server : {0} for printer : {1} with MAC Address : {2} for {3}.".FormatWith(dhcpServer, reservedIpAddress, printerMacAddress, reservationType));
				return false;
			}

			return true;
		}

		/// <summary>
		/// Ping and check for the printer availability.
		/// </summary>
		/// <param name="ipAddress">IP address of the printer.</param>
		/// <returns>true if the printer is available, else false.</returns>
		private static bool CheckForPrinterAvailability(string ipAddress)
		{
			return CheckForPrinterAvailability(ipAddress, TimeSpan.FromSeconds(30));
		}

		/// <summary>
		/// Ping and check for the printer availability.
		/// </summary>
		/// <param name="ipAddress">IP address of the printer.</param>
		/// <param name="timeOut">TimeOut for ping.</param>
		/// <returns>true if the printer is available, else false.</returns>
		private static bool CheckForPrinterAvailability(string ipAddress, TimeSpan timeOut)
		{
			TraceFactory.Logger.Info("Check for Printer availability");

			// Check for printer availability
			if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(ipAddress), timeOut))
			{
				TraceFactory.Logger.Info("Ping succeeded with Printer IP Address:{0}".FormatWith(ipAddress));
				return true;
			}
			else
			{
				TraceFactory.Logger.Info("Ping failed with Printer IP Address:{0}".FormatWith(ipAddress));
				return false;
			}

		}

		/// <summary>
		/// Validate IP Configuration Method with EWS, SNMP and Telnet with the given IP configuration method
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <param name="ipConfigMethod"><see cref="IPConfigMethod"/>IP Configuration method to validate</param>
		/// <param name="ipAddress">IP Address of the printer</param>
		/// <returns>Returns true if the configuration method is validated with EWS, SNMP and Telnet, false otherwise</returns>
		private static bool ValidateIPConfigMethod(IPConfigurationActivityData activityData, IPConfigMethod ipConfigMethod, string ipAddress = null, bool validatePing = true, bool validateTelnet = true)
		{
			TraceFactory.Logger.Info("Validating IP Configuration Method : {0} ".FormatWith(ipConfigMethod));

			// take activity data IP address if ipAddress is empty.
			ipAddress = string.IsNullOrEmpty(ipAddress) ? activityData.PrimaryWiredIPv4Address : ipAddress;

			#region Ping

			if (validatePing)
			{
				if (!CheckForPrinterAvailability(ipAddress))
				{
					return false;
				}
			}

			#endregion

			#region EWS

			EwsWrapper.Instance().ChangeDeviceAddress(ipAddress);

			if (EwsWrapper.Instance().GetIPConfigMethod() == ipConfigMethod)
			{
				TraceFactory.Logger.Info("EWS: Successfully validated the IP config method:{0}".FormatWith(ipConfigMethod));
			}
			else
			{
				TraceFactory.Logger.Info("EWS: Failed to validate the IP config method:{0}".FormatWith(ipConfigMethod));
				return false;
			}

			#endregion

			#region SNMP

			if (PrinterFamilies.TPS.ToString() == activityData.ProductFamily)
			{
				TraceFactory.Logger.Info("SNMP validation is currently not implemented.");
			}
			else
			{
				SnmpWrapper.Instance().Create(ipAddress);

				if (SnmpWrapper.Instance().GetIPConfigMethod() == ipConfigMethod)
				{
					TraceFactory.Logger.Info("SNMP: Successfully validated the IP config method:{0}".FormatWith(ipConfigMethod));
				}
				else
				{
					TraceFactory.Logger.Info("SNMP: Failed to validate the IP config method:{0}".FormatWith(ipConfigMethod));
					return false;
				}
			}

			#endregion

			#region Telnet

			if (validateTelnet)
			{
				if (PrinterFamilies.TPS.ToString() == activityData.ProductFamily || PrinterFamilies.InkJet.ToString() == activityData.ProductFamily)
				{
					TraceFactory.Logger.Info("Telnet validation is currently not implemented.");
				}
				else
				{
					TelnetWrapper.Instance().Create(ipAddress);

					if (TelnetWrapper.Instance().GetIPConfigMethod((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily)) == ipConfigMethod)
					{
						TraceFactory.Logger.Info("Telnet : Successfully validated the config method:{0}.".FormatWith(ipConfigMethod));
					}
					else
					{
						TraceFactory.Logger.Info("Telnet : Failed to validate the config method:{0}".FormatWith(ipConfigMethod));
						return false;
					}
				}
			}

			#endregion

			return true;
		}

		/// <summary>
		/// Gets the VLAN numbers corresponding to the default DHCP, second DHCP and Linux Servers.
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <param name="serverIpAdress">The Server IP for which the vlan nuber iis to be fetched.</param>
		/// <returns>The vlan identifier for the specific server.</returns>
		private static int GetVlanNumber(IPConfigurationActivityData activityData, string serverIpAdress)
		{
			// Get the VLAN number of default DHCP Server
			int vlanIdentifier = (from vlan in activityData.VirtualLanDetails
								  where IPAddress.Parse(vlan.Value).IsInSameSubnet(IPAddress.Parse(serverIpAdress))
								  select vlan.Key).FirstOrDefault();

			return vlanIdentifier;
		}

		/// <summary>
		/// Configures the printer with Auto IP mode using DHCP server operations.
		/// Following operations are performed:
		/// 1. Change lease time to 60 seconds in DHCP server
		/// 2. Change configuration method to DHCP on Printer
		/// 3. Stop DHCP server - Printer will go into Auto IP mode
		/// 4. Check for Printer Auto IP Address
		/// 5. Renew the local machine IP address to acquire Auto IP
		/// </summary>
		/// <param name="ipAddress">Printer Auto IP Address</param>
		/// <returns>true if Printer is configured with Auto IP Address, false otherwise</returns>
		private static bool ConfigurePrinterWithAutoIP(IPConfigurationActivityData activityData, out string ipAddress)
		{
			ipAddress = string.Empty;

			TraceFactory.Logger.Info("Configuring Printer with Auto IP Address:");

			// Change lease time to 1 minute on DHCP server
			DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
			serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 60);

			// In case printer is already in DHCP, new lease time will not be updated
			EwsWrapper.Instance().ChangeDeviceAddress(IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
			EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address), validate: false);
			EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

			// Stop DHCP server
			serviceMethod.Channel.StopDhcpServer();

			// Wait for Printer to acquire Auto IP
			TraceFactory.Logger.Info("Waiting for {0} minutes for Printer to acquire Auto IP Address.".FormatWith(LEASE_WAIT_TIME));
			Thread.Sleep(TimeSpan.FromMinutes(LEASE_WAIT_TIME));

			// Validate and return Printer Auto IP
			if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out ipAddress))
			{
				TraceFactory.Logger.Info("Since Printer didn't get discovered with Auto IP first time, discovering again..");
				// Sometimes Printer doesn't get discovered, hence discovering again
				if (!Utility.IsPrinterInAutoIP(activityData.PrimaryMacAddress, out ipAddress))
				{
					return false;
				}
			}

			// Renew local machine IP configuration
			NetworkUtil.RenewLocalMachineIP();
			//Checking the printer connectivity
			if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(ipAddress), TimeSpan.FromSeconds(30)))
			{
				TraceFactory.Logger.Info("Auto Ip:{0} is not Pinging even after renewing the Local MachineIP".FormatWith(ipAddress));
				return false;
			}
			return true;
		}

		/// <summary>
		/// Validates if the primary dns server parameter from the server is updated on the printer through EWS, Telnet and SNMP.
		/// </summary>
		/// <param name="serverDomainName">The server domain name.</param>
		/// <returns>true if the primary dns server is updated on the printer, else false.</returns>
		private static bool ValidatePrimaryDnsServer(PrinterFamilies family, string primaryDnsServer, string address = "")
		{
			if (!string.IsNullOrEmpty(address))
			{
				EwsWrapper.Instance().ChangeDeviceAddress(address);
				TelnetWrapper.Instance().Create(address);
				SnmpWrapper.Instance().Create(address);
			}

			// Validate primary dns server through EWS, Telnet and SNMP.
			TraceFactory.Logger.Debug("Validating Primary DNS from Web UI.");

			if (string.Equals(EwsWrapper.Instance().GetPrimaryDnsServer().Trim(), primaryDnsServer, StringComparison.CurrentCultureIgnoreCase))
			{
				TraceFactory.Logger.Info("Successfully validated primary DNS Server : {0} from Web UI.".FormatWith(primaryDnsServer));
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate primary DNS Server : {0} from Web UI.".FormatWith(primaryDnsServer));
				return false;
			}

			TraceFactory.Logger.Debug("Validating Primary DNS from telnet.");

			// Value retrieved will have (Read-Only) post fixed based on config precedence, hence using 'Contains'
			if (TelnetWrapper.Instance().GetConfiguredValue(family, "primarydnsserver").Contains(primaryDnsServer, StringComparison.CurrentCultureIgnoreCase))
			{
				TraceFactory.Logger.Info("Successfully validated primary DNS Server : {0} from telnet.".FormatWith(primaryDnsServer));
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate primary DNS Server : {0} from telnet.".FormatWith(primaryDnsServer));
				return false;
			}

			if (PrinterFamilies.TPS.Equals(family))
			{
				TraceFactory.Logger.Info("SNMP Validation is not implemented for TPS.");
			}
			else
			{
				TraceFactory.Logger.Debug("Validating Primary DNS from SNMP.");

				if (string.Equals(SnmpWrapper.Instance().GetPrimaryDnsServer(), primaryDnsServer, StringComparison.CurrentCultureIgnoreCase))
				{
					TraceFactory.Logger.Info("Successfully validated primary DNS Server : {0} from SNMP.".FormatWith(primaryDnsServer));
				}
				else
				{
					TraceFactory.Logger.Info("Failed to validate primary DNS Server : {0} from SNMP.".FormatWith(primaryDnsServer));
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Validates if the secondary dns server parameter from the server is updated on the printer through EWS, Telnet and SNMP.
		/// </summary>
		/// <param name="serverDomainName">The server domain name.</param>
		/// <returns>true if the secondary dns server is updated on the printer, else false.</returns>
		private static bool ValidateSecondaryDnsServer(PrinterFamilies family, string secondaryDnsServer, string address = "")
		{
			if (!string.IsNullOrEmpty(address))
			{
				EwsWrapper.Instance().ChangeDeviceAddress(address);
				TelnetWrapper.Instance().Create(address);
				SnmpWrapper.Instance().Create(address);
			}

			// Validate primary dns server through EWS, Telnet and SNMP.
			TraceFactory.Logger.Debug("Validating Secondary DNS from Web UI.");

			if (string.Equals(EwsWrapper.Instance().GetSecondaryDnsServer().Trim(), secondaryDnsServer, StringComparison.CurrentCultureIgnoreCase))
			{
				TraceFactory.Logger.Info("Successfully validated secondary DNS Server : {0} from Web UI.".FormatWith(secondaryDnsServer));
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate secondary DNS Server : {0} from Web UI.".FormatWith(secondaryDnsServer));
				return false;
			}

			TraceFactory.Logger.Debug("Validating Secondary DNS from telnet.");

			// Value retrieved will have (Read-Only) post fixed based on config precedence, hence using 'Contains'
			if (TelnetWrapper.Instance().GetConfiguredValue(family, "secondarydnsserver").Contains(secondaryDnsServer, StringComparison.CurrentCultureIgnoreCase))
			{
				TraceFactory.Logger.Info("Successfully validated secondary DNS Server : {0} from telnet.".FormatWith(secondaryDnsServer));
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate secondary DNS Server : {0} from telnet.".FormatWith(secondaryDnsServer));
				return false;
			}

			if (PrinterFamilies.TPS.Equals(family))
			{
				TraceFactory.Logger.Info("SNMP Validation is not implemented for TPS.");
			}
			else
			{
				TraceFactory.Logger.Debug("Validating Secondary DNS from SNMP.");

				if (string.Equals(SnmpWrapper.Instance().GetSecondaryDnsServer(), secondaryDnsServer, StringComparison.CurrentCultureIgnoreCase))
				{
					TraceFactory.Logger.Info("Successfully validated secondary DNS Server : {0} from SNMP.".FormatWith(secondaryDnsServer));
				}
				else
				{
					TraceFactory.Logger.Info("Failed to validate secondary DNS Server : {0} from SNMP.".FormatWith(secondaryDnsServer));
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Validates if the domain name from the server is updated on the printer through EWS, Telnet and SNMP.
		/// </summary>
		/// <param name="serverDomainName">The server domain name.</param>
		/// <returns>true if the domain name is updated on the printer, else false.</returns>
		private static bool ValidateDomainName(PrinterFamilies family, string serverDomainName, string address = "")
		{
			// Validate domainName through EWS, Telnet and SNMP.
			TraceFactory.Logger.Debug("Validating domain name from Web UI.");

			if (!string.IsNullOrEmpty(address))
			{
				EwsWrapper.Instance().ChangeDeviceAddress(address);
				TelnetWrapper.Instance().Create(address);
				SnmpWrapper.Instance().Create(address);
			}

			if (string.Equals(EwsWrapper.Instance().GetDomainName().Trim(), serverDomainName, StringComparison.CurrentCultureIgnoreCase))
			{
				TraceFactory.Logger.Info("Successfully validated domain name : {0} from Web UI.".FormatWith(serverDomainName));
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate domain name : {0} from Web UI.".FormatWith(serverDomainName));
				return false;
			}

			TraceFactory.Logger.Debug("Validating domain name from telnet.");

			// Value retrieved will have (Read-Only) post fixed based on config precedence, hence using 'Contains'
			if (TelnetWrapper.Instance().GetConfiguredValue(family, "domainname").Contains(serverDomainName, StringComparison.CurrentCultureIgnoreCase))
			{
				TraceFactory.Logger.Info("Successfully validated domain name : {0} from telnet.".FormatWith(serverDomainName));
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate domain name : {0} from telnet.".FormatWith(serverDomainName));
				return false;
			}

			TraceFactory.Logger.Debug("Validating domain name from SNMP.");

			if (string.Equals(SnmpWrapper.Instance().GetDomainName(), serverDomainName, StringComparison.CurrentCultureIgnoreCase))
			{
				TraceFactory.Logger.Info("Successfully validated domain name : {0} from SNMP.".FormatWith(serverDomainName));
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate domain name : {0} from SNMP.".FormatWith(serverDomainName));
				return false;
			}

			return true;
		}

		/// <summary>
		/// Validates if the host name from the server is updated on the printer through EWS, Telnet and SNMP.
		/// </summary>
		/// <param name="serverHostname">The server hostname.</param>
		/// <returns>true if the hostname is updated on the printer, else false.</returns>
		private static bool ValidateHostName(PrinterFamilies family, string serverHostname, string address = "")
		{
			// Validate the server parameters through EWS, telnet and SNMP
			TraceFactory.Logger.Debug("Validating host name from Web UI.");

			if (!string.IsNullOrEmpty(address))
			{
				EwsWrapper.Instance().ChangeDeviceAddress(address);
				TelnetWrapper.Instance().Create(address);
				SnmpWrapper.Instance().Create(address);
			}

			if (string.Equals(EwsWrapper.Instance().GetHostname().Trim(), serverHostname, StringComparison.CurrentCultureIgnoreCase))
			{
				TraceFactory.Logger.Info("Successfully validated host name : {0} from Web UI.".FormatWith(serverHostname));
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate host name : {0} from Web UI.".FormatWith(serverHostname));
				return false;
			}

			// Telnet Validation for HostName
			TraceFactory.Logger.Debug("Validating host name from telnet.");

			// Value retrieved will have (Read-Only) post fixed based on config precedence, hence using 'Contains'
			if (TelnetWrapper.Instance().GetConfiguredValue(family, "hostname").Contains(serverHostname, StringComparison.CurrentCultureIgnoreCase))
			{
				TraceFactory.Logger.Info("Successfully validated host name : {0} from telnet.".FormatWith(serverHostname));
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate host name : {0} from telnet.".FormatWith(serverHostname));
				return false;
			}

			// SNMP Validation
			TraceFactory.Logger.Debug("Validating host name from SNMP.");

			if (SnmpWrapper.Instance().GetHostName().Equals(serverHostname))
			{
				TraceFactory.Logger.Info("Successfully validated host name : {0} from SNMP.".FormatWith(serverHostname));
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate host name : {0} from SNMP.".FormatWith(serverHostname));
				return false;
			}

			return true;
		}

		/// <summary>
		/// Validates if the primary wins server parameter from the server is updated on the printer through EWS, Telnet and SNMP.
		/// </summary>
		/// <param name="family"><see cref=" PrinterFamilies"/></param>
		/// <param name="primaryWinsServer">Primary Wins server name.</param>
		/// <returns>true if the primary wins server is updated on the printer, else false.</returns>
		private static bool ValidatePrimaryWinsServer(PrinterFamilies family, string primaryWinsServer, string address = "")
		{
			if (!string.IsNullOrEmpty(address))
			{
				EwsWrapper.Instance().ChangeDeviceAddress(address);
				TelnetWrapper.Instance().Create(address);
				SnmpWrapper.Instance().Create(address);
			}

			/*	Validation when empty value is provided
			 * ----------------------------------------------------
			 * Product |  EWS			|	Telnet		| SNMP
			 * ----------------------------------------------------
			 * VEP	   | Blank			| Not Specified | 0.0.0.0
			 * TPS	   | Not Specified	| 0.0.0.0		| NA
			 * */

			// This block handles for TPS and value is empty
			if (ProductFamilies.TPS.Equals(family) && string.IsNullOrEmpty(primaryWinsServer))
			{
				TraceFactory.Logger.Debug("Validating Primary WINS server from Web UI.");

				primaryWinsServer = "Not Specified";

				// Web UI shows blank sometimes and Not Specified after a while, hence validating either one of them
				if (string.Equals(EwsWrapper.Instance().GetPrimaryWinServerIP().Trim(), primaryWinsServer, StringComparison.CurrentCultureIgnoreCase) || string.IsNullOrEmpty(EwsWrapper.Instance().GetPrimaryWinServerIP().Trim()))
				{
					TraceFactory.Logger.Info("Successfully validated primary WINS Server : {0} from Web UI.".FormatWith(primaryWinsServer));
				}
				else
				{
					TraceFactory.Logger.Info("Failed to validate primary WINS Server : {0} from Web UI.".FormatWith(primaryWinsServer));
					return false;
				}

				TraceFactory.Logger.Debug("Validating Primary WINS server from telnet.");

				primaryWinsServer = "0.0.0.0";

				// Value retrieved will have (Read-Only) post fixed based on config precedence, hence using 'Contains'
				if (TelnetWrapper.Instance().GetConfiguredValue(family, "primarywinsserver").Contains(primaryWinsServer, StringComparison.CurrentCultureIgnoreCase))
				{
					TraceFactory.Logger.Info("Successfully validated primary WINS Server : {0} from telnet.".FormatWith(primaryWinsServer));
				}
				else
				{
					TraceFactory.Logger.Info("Failed to validate primary WINS Server : {0} from telnet.".FormatWith(primaryWinsServer));
					return false;
				}

				TraceFactory.Logger.Info("SNMP Validation is not implemented for TPS.");
			}// Other cases are handled here: VEP & TPS - valid input and VEP - Empty
			else
			{
				// Validate primary dns server through EWS, Telnet and SNMP.
				TraceFactory.Logger.Debug("Validating Primary WINS server from Web UI.");

				if (string.Equals(EwsWrapper.Instance().GetPrimaryWinServerIP().Trim(), primaryWinsServer, StringComparison.CurrentCultureIgnoreCase))
				{
					TraceFactory.Logger.Info("Successfully validated primary WINS Server : {0} from Web UI.".FormatWith(primaryWinsServer));
				}
				else
				{
					TraceFactory.Logger.Info("Failed to validate primary WINS Server : {0} from Web UI.".FormatWith(primaryWinsServer));
					return false;
				}

				TraceFactory.Logger.Debug("Validating Primary WINS server from telnet.");

				// Value retrieved will have (Read-Only) post fixed based on config precedence, hence using 'Contains'
				if (TelnetWrapper.Instance().GetConfiguredValue(family, "primarywinsserver").Contains(primaryWinsServer, StringComparison.CurrentCultureIgnoreCase))
				{
					TraceFactory.Logger.Info("Successfully validated primary WINS Server : {0} from telnet.".FormatWith(primaryWinsServer));
				}
				else
				{
					TraceFactory.Logger.Info("Failed to validate primary WINS Server : {0} from telnet.".FormatWith(primaryWinsServer));
					return false;
				}

				if (PrinterFamilies.TPS.Equals(family))
				{
					TraceFactory.Logger.Info("SNMP Validation is not implemented for TPS.");
				}
				else
				{
					TraceFactory.Logger.Debug("Validating Primary WINS server from SNMP.");

					if (string.IsNullOrEmpty(primaryWinsServer))
					{
						primaryWinsServer = "0.0.0.0";
					}

					if (string.Equals(SnmpWrapper.Instance().GetPrimaryWinsServer().Trim(), primaryWinsServer, StringComparison.CurrentCultureIgnoreCase))
					{
						TraceFactory.Logger.Info("Successfully validated primary WINS Server : {0} from SNMP.".FormatWith(primaryWinsServer));
					}
					else
					{
						TraceFactory.Logger.Info("Failed to validate primary WINS Server : {0} from SNMP.".FormatWith(primaryWinsServer));
						return false;
					}

				}
			}

			return true;
		}

		/// <summary>
		/// Validates if the secondary wins server parameter from the server is updated on the printer through EWS, Telnet and SNMP.
		/// </summary>
		/// /// <param name="family"><see cref=" PrinterFamilies"/></param>
		/// <param name="secondaryWinsServer">Secondary Wins server name.</param>
		/// <returns>true if the secondary wins server is updated on the printer, else false.</returns>
		private static bool ValidateSecondaryWinsServer(PrinterFamilies family, string secondaryWinsServer, string address= "")
		{
			if (!string.IsNullOrEmpty(address))
			{
				EwsWrapper.Instance().ChangeDeviceAddress(address);
				TelnetWrapper.Instance().Create(address);
				SnmpWrapper.Instance().Create(address);
			}

			// Validate secondary dns server through EWS, Telnet and SNMP.
			TraceFactory.Logger.Debug("Validating Secondary WINS server from Web UI.");

			if (string.Equals(EwsWrapper.Instance().GetSecondaryWinServerIP().Trim(), secondaryWinsServer, StringComparison.CurrentCultureIgnoreCase))
			{
				TraceFactory.Logger.Info("Successfully validated secondary WINS Server : {0} from Web UI.".FormatWith(secondaryWinsServer));
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate secondary WINS Server : {0} from Web UI.".FormatWith(secondaryWinsServer));
				return false;
			}

			TraceFactory.Logger.Debug("Validating Secondary WINS server from telnet.");

			// Value retrieved will have (Read-Only) post fixed based on config precedence, hence using 'Contains'
			if (TelnetWrapper.Instance().GetConfiguredValue(family, "secondarywinsserver").Contains(secondaryWinsServer, StringComparison.CurrentCultureIgnoreCase))
			{
				TraceFactory.Logger.Info("Successfully validated secondary WINS Server : {0} from telnet.".FormatWith(secondaryWinsServer));
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate secondary WINS Server : {0} from telnet.".FormatWith(secondaryWinsServer));
				return false;
			}

			if (PrinterFamilies.TPS.Equals(family))
			{
				TraceFactory.Logger.Info("SNMP Validation is not implemented for TPS.");
			}
			{
				TraceFactory.Logger.Debug("Validating Secondary WINS server from SNMP.");

				if (string.Equals(SnmpWrapper.Instance().GetSecondaryWinsServer().Trim(), secondaryWinsServer, StringComparison.CurrentCultureIgnoreCase))
				{
					TraceFactory.Logger.Info("Successfully validated secondary WINS Server : {0} from SNMP.".FormatWith(secondaryWinsServer));
				}
				else
				{
					TraceFactory.Logger.Info("Failed to validate secondary WINS Server : {0} from SNMP.".FormatWith(secondaryWinsServer));
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Validate Primary DnsV6 Server Address
		/// </summary>
		/// /// <param name="family"><see cref=" PrinterFamilies"/></param>
		/// <param name="primaryDnsServer"></param>
		/// <returns></returns>
		private static bool ValidatePrimaryDnsV6Server(PrinterFamilies family, string primaryDnsServer)
		{
			// Validate primary dns server through EWS, Telnet and SNMP.
			TraceFactory.Logger.Debug("Validating Primary DNSv6 from Web UI.");

			if (string.Equals(EwsWrapper.Instance().GetPrimaryDnsv6Server().Trim(), primaryDnsServer, StringComparison.CurrentCultureIgnoreCase))
			{
				TraceFactory.Logger.Info("Web UI is updated with primary DNSv6 address : {0} from the server.".FormatWith(primaryDnsServer));
			}
			else
			{
				TraceFactory.Logger.Info("Web UI is not updated with primary DNSv6 address : {0} from the server.".FormatWith(primaryDnsServer));
				return false;
			}			

			if (PrinterFamilies.TPS.Equals(family))
			{
				TraceFactory.Logger.Info("SNMP Validation is not implemented for TPS.");
			}
			else
			{
				TraceFactory.Logger.Debug("Validating Primary DNSv6 from SNMP.");

				if (string.IsNullOrEmpty(primaryDnsServer))
				{
					primaryDnsServer = "::";
				}

				if (string.Equals(SnmpWrapper.Instance().GetPrimaryDnsV6Address(), primaryDnsServer, StringComparison.CurrentCultureIgnoreCase))
				{
					TraceFactory.Logger.Info("SNMP is updated with primary DNSv6 address : {0} from the server.".FormatWith(primaryDnsServer));
				}
				else
				{
					TraceFactory.Logger.Info("SNMP is not updated with primary DNSv6 address : {0} from the server.".FormatWith(primaryDnsServer));
					return false;
				}
			}			 

			return true;
		}

		/// <summary>
		/// Validate Primary DnsV6 Server Address
		/// </summary>
		/// /// <param name="family"><see cref=" PrinterFamilies"/></param>
		/// <param name="primaryDnsServer"></param>
		/// <returns></returns>
		private static bool ValidateSecondaryDnsV6Server(PrinterFamilies family, string secondaryv6DnsServer)
		{
			// Validate primary dns server through EWS, Telnet and SNMP.
			TraceFactory.Logger.Debug("Validating secondary DNSv6 from Web UI.");

			if (string.Equals(EwsWrapper.Instance().GetSecondaryDnsv6Server().Trim(), secondaryv6DnsServer, StringComparison.CurrentCultureIgnoreCase))
			{
				TraceFactory.Logger.Info("Web UI is updated with secondary DNSv6 address : {0} from the server.".FormatWith(secondaryv6DnsServer));
			}
			else
			{
				TraceFactory.Logger.Info("Web UI is not updated with secondary DNSv6 address : {0} from the server.".FormatWith(secondaryv6DnsServer));
				return false;
			}

			if (PrinterFamilies.TPS.Equals(family))
			{
				TraceFactory.Logger.Info("SNMP Validation is not implemented for TPS.");
			}
			else
			{
				TraceFactory.Logger.Debug("Validating secondary DNSv6 from SNMP.");

				if (string.Equals(SnmpWrapper.Instance().GetSecondaryDnsV6Address(), secondaryv6DnsServer, StringComparison.CurrentCultureIgnoreCase))
				{
					TraceFactory.Logger.Info("SNMP is updated with secondary DNSv6 address : {0} from SNMP.".FormatWith(secondaryv6DnsServer));
				}
				else
				{
					TraceFactory.Logger.Info("SNMP is not updated with secondary DNSv6 address : {0} from SNMP.".FormatWith(secondaryv6DnsServer));
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Validate Domain Search List
		/// </summary>
		/// /// <param name="family"><see cref=" PrinterFamilies"/></param>
		/// <param name="domainSearchList"></param>
		/// <returns></returns>
		private static bool ValidateDomainSearchList(PrinterFamilies family, string domainSearchList, bool validateNegativeCondition = false)
		{
			// Validate primary dns server through EWS, SNMP.
			TraceFactory.Logger.Debug("Validating domain search list from Web UI.");

			List<string> serverDnsValues = new List<string>();

			if (domainSearchList.Contains("|"))
			{
				serverDnsValues = domainSearchList.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
			}
			else
			{
				serverDnsValues.Add(domainSearchList);
			}

			List<string> printerDnsValues = EwsWrapper.Instance().GetDNSSuffixLists();

			if (printerDnsValues.Count == 0 && !validateNegativeCondition)
			{
				TraceFactory.Logger.Info("Failed to fetch Domain search list values through Web UI.");
				return false;
			}

			if (printerDnsValues.Count != 0)
			{
				foreach (string domainList in serverDnsValues)
				{
					if (printerDnsValues.Contains(domainList))
					{
						TraceFactory.Logger.Info("Web UI is updated with the domain search list value: {0} from server.".FormatWith(domainSearchList));
					}
					else
					{
						TraceFactory.Logger.Info("Web UI is not updated with the domain search list value: {0} from server.".FormatWith(domainSearchList));
						return false;
					}
				}
			}
			else
			{
				TraceFactory.Logger.Info("EWS: No entries found in domain search list.");
			}


			if (PrinterFamilies.TPS.Equals(family))
			{
				TraceFactory.Logger.Info("SNMP Validation is not implemented for TPS.");
			}
			else
			{
				TraceFactory.Logger.Debug("Validating domain search list from SNMP.");

				printerDnsValues.Clear();

				printerDnsValues = SnmpWrapper.Instance().GetDomainSearchLists();

				if (printerDnsValues.Count == 0 && !validateNegativeCondition)
				{
					TraceFactory.Logger.Info("Failed to fetch Domain search list values through SNMP.");
					return false;
				}

				if (printerDnsValues.Count != 0)
				{
					foreach (string domainList in serverDnsValues)
					{
						if (printerDnsValues.Contains(domainList))
						{
							TraceFactory.Logger.Info("SNMP is updated with the domain search list value: {0} from server.".FormatWith(domainSearchList));
						}
						else
						{
							TraceFactory.Logger.Info("SNMP is not updated with the domain search list value: {0} from server.".FormatWith(domainSearchList));
							return false;
						}
					}
				}
				else
				{
					TraceFactory.Logger.Info("SNMP: No entries found in domain search list.");
				}
			}		

			return true;
		}

		/// <summary>
		/// Validate Domain Search List
		/// </summary>
		/// /// <param name="family"><see cref=" PrinterFamilies"/></param>
		/// <param name="v6DomainName"></param>
		/// <returns></returns>
		private static bool Validatev6DomainName(PrinterFamilies family, string v6DomainName)
		{
			// Validate primary dns server through EWS, SNMP.
			TraceFactory.Logger.Debug("Validating validate v6 domain name from Web UI.");

			if (string.Equals(EwsWrapper.Instance().Getv6DomainName(), v6DomainName, StringComparison.CurrentCultureIgnoreCase))
			{
				TraceFactory.Logger.Info("Successfully validated v6 domain name : {0} from Web UI.".FormatWith(v6DomainName));
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate v6 domain name : {0} from Web UI.".FormatWith(v6DomainName));
				return false;
			}

			if (PrinterFamilies.TPS.Equals(family))
			{
				TraceFactory.Logger.Info("SNMP Validation is not implemented for TPS.");
			}
			else
			{
				TraceFactory.Logger.Debug("Validating v6 domain name from SNMP.");

				if (string.Equals(SnmpWrapper.Instance().Getv6DomainName(), v6DomainName, StringComparison.CurrentCultureIgnoreCase))
				{
					TraceFactory.Logger.Info("Successfully validated v6 domain name : {0} from SNMP.".FormatWith(v6DomainName));
				}
				else
				{
					TraceFactory.Logger.Info("Failed to validate v6 domain name : {0} from SNMP.".FormatWith(v6DomainName));
					return false;
				}
			}


			return true;
		}

		/// <summary>
		/// Connects the printer in the default vlan.
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <param name="networkSwitch"><see cref="INetworkSwitch"/></param>
		/// <returns>true, if the printer is connected to default vlan.</returns>
		private static bool ConnectDefaultVlan(IPConfigurationActivityData activityData, INetworkSwitch networkSwitch)
		{
			// Fetch the vlan ports for primary vlan
			int defaultVlan = GetVlanNumber(activityData, activityData.PrimaryDhcpServerIPAddress);

			Collection<int> defaultVlanPorts = networkSwitch.GetVlanPorts(defaultVlan);

			if (defaultVlanPorts.Contains(activityData.PrimaryInterfacePortNo))
			{
				// port is connected to default port.
				return true;
			}

			networkSwitch.DisablePort(activityData.PrimaryInterfacePortNo);

			if (!networkSwitch.ChangeVirtualLan(activityData.PrimaryInterfacePortNo, defaultVlan))
			{
				return false;
			}

			// Enable the port
			networkSwitch.EnablePort(activityData.PrimaryInterfacePortNo);

			// Wait for 3 minutes
			Thread.Sleep(TimeSpan.FromMinutes(3));

			return true;
		}

		/// <summary>
		/// Set IP Config Method to Auto IP
		/// TPS: Changing from Any config method to Auto IP will make printer to acquire Auto IP Address
		/// VEP/ LFP: DHCP server should be down before Config method is set to Auto IP
		/// </summary>
		/// <param name="printerFamily">VEP/ TPS/ LFP</param>
		/// <param name="printerIPAddress">Current Printer IP Address</param>
		/// <param name="printerMACAddress">Printer MAC Address</param>
		/// <param name="autoIPAddress">Acquired Auto IP Address</param>
		/// <returns>true if printer acquired Auto IP Address, false otherwise</returns>
		private static bool SetAutoIPConfigMethod(string serverIP, string printerFamily, string printerIPAddress, string printerMACAddress, out string autoIPAddress)
		{
			DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(serverIP);

			if (PrinterFamilies.TPS.ToString() == printerFamily || PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(printerFamily))
			{
				EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.AUTOIP, validate: false);

				if (!Utility.IsPrinterInAutoIP(printerMACAddress, out autoIPAddress))
				{
					return false;
				}

				serviceMethod.Channel.StopDhcpServer();
			}
			else
			{
				serviceMethod.Channel.StopDhcpServer();
				EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.AUTOIP, validate: false);

				if (!Utility.IsPrinterInAutoIP(printerMACAddress, out autoIPAddress))
				{
					return false;
				}
			}

			NetworkUtil.RenewLocalMachineIP();
			return true;
		}

		/// <summary>
		/// Ping continuously to a non existent host
		/// </summary>
		/// <param name="printerIPAddress">Printer IP Address</param>
		private static void PingNonExistentHost(string nonPingingIP)
		{
			ProcessEx.Execute("cmd.exe", "/C ping {0} -t".FormatWith(nonPingingIP));
		}

		/// <summary>
		/// Validate whether finite preferred/ valid lifetime are updated on printer
		/// </summary>
		/// <param name="ipv6ScopeIPAddress">DHCP IPv6 Scope IP Address</param>
		/// <param name="isPreferredLifetime">true if Preferred Lifetime needs to validated, false for Valid Lifetime</param>
		/// <returns>true if validation of preferred/ valid lifetime is successful, false otherwise</returns>
		private static bool VerifyIPv6Lifetime(string ipv6ScopeIPAddress, IPConfigurationActivityData activityData, bool isPreferredLifetime = false)
		{
			DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

			try
			{
				TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
				TraceFactory.Logger.Info("Assumption: Printer is connected to DHCP server via router which provides finite valid and preferred lease time.");
				TraceFactory.Logger.Info("Preset Lifetime on router: Preferred - {0} days, Valid - {1} days".
											FormatWith(ROUTER_PREFERRED_LIFETIME.TotalDays, ROUTER_VALID_LIFETIME.TotalDays));

				serviceMethod.Channel.SetPreferredLifetime(activityData.PrimaryDhcpServerIPAddress, ipv6ScopeIPAddress, (int)PREFERRED_LIFETIME.TotalSeconds);
				serviceMethod.Channel.SetValidLifetime(activityData.PrimaryDhcpServerIPAddress, ipv6ScopeIPAddress, (int)VALID_LIFETIME.TotalSeconds);

				EwsWrapper.Instance().SetDHCPv6OnStartup(true);				
				EwsWrapper.Instance().SetIPv6(true);
				EwsWrapper.Instance().SetStatelessAddress(true);

				int statelessAddressCount = PrinterFamilies.TPS.ToString() == activityData.ProductFamily ? 3 : 4;

				// Get IPv6 table details
				Collection<IPv6Details> ipv6Details      = EwsWrapper.Instance().GetIPv6TableDetails(statelessAddressCount + 2);
				Collection<IPAddress> statelessIPAddress = IPv6Utils.GetStatelessIPAddress(ipv6Details);
				IPAddress statefulAddress                = IPv6Utils.GetStatefulAddress(ipv6Details);

				if (IPAddress.IPv6None == statefulAddress || 0 == statelessIPAddress.Count)
				{
					TraceFactory.Logger.Info("IPv6 table is not updated after waiting for 5 minutes.");
					TraceFactory.Logger.Info("Stateless address count: {0}, Stateful address: {1}".FormatWith(statelessIPAddress.Count, statefulAddress));

					return false;
				}

				if (!CompareIPv6LifeTime(ipv6Details, isPreferredLifetime ? PREFERRED_LIFETIME : VALID_LIFETIME, isPreferredLifetime, isPreferredLifetime ? ROUTER_PREFERRED_LIFETIME : ROUTER_VALID_LIFETIME))
				{
					return false;
				}

				TraceFactory.Logger.Info("Waiting for {0} seconds for DHCPv6 server Preferred/ Valid life time to expire.".FormatWith(VALID_LIFETIME.TotalSeconds));
				Thread.Sleep(VALID_LIFETIME);
				TraceFactory.Logger.Info("Preferred/ Valid Lifetime renewal for router is not handled.");

				ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(statelessAddressCount + 2, false);

				// During lease renewal, IP Addresses might change; capturing again
				statelessIPAddress = IPv6Utils.GetStatelessIPAddress(ipv6Details);
				statefulAddress    = IPv6Utils.GetStatefulAddress(ipv6Details);

				if (IPAddress.IPv6None == statefulAddress || 0 == statelessIPAddress.Count)
				{
					TraceFactory.Logger.Info("IPv6 table is not updated.");
					TraceFactory.Logger.Debug("Stateless address count: {0}, Stateful address: {1}".FormatWith(statelessIPAddress.Count, statefulAddress));
					return false;
				}

				return CompareIPv6LifeTime(ipv6Details, isPreferredLifetime ? PREFERRED_LIFETIME : VALID_LIFETIME, isPreferredLifetime, isPreferredLifetime ? ROUTER_PREFERRED_LIFETIME : ROUTER_VALID_LIFETIME);

			}
			finally
			{
				TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
				// By default 'Perform DHCPv6 when Stateless configuration is unsuccessful or disabled' option should be selected
				EwsWrapper.Instance().SetDHCPv6StatelessConfigurationOption();

				serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
				serviceMethod.Channel.SetValidLifetime(activityData.PrimaryDhcpServerIPAddress, ipv6ScopeIPAddress, DEFAULT_VALID_LEASE_TIME);
				serviceMethod.Channel.SetPreferredLifetime(activityData.PrimaryDhcpServerIPAddress, ipv6ScopeIPAddress, DEFAULT_PREFERRED_LEASE_TIME);
			}
		}

		/// <summary>
		/// Verify if preferred/ valid lifetime are configured properly
		/// </summary>
		/// <param name="ipv6Details">Collection of <see cref=" IPv6Details"/></param>
		/// <param name="statefulAddress">Printer Statefull IP Address</param>
		/// <param name="statelessIPAddress">Printer Stateless IP Address</param>
		/// <param name="isPreferredLifetime">true if Preferred Lifetime needs to validated, false for Valid Lifetime</param>
		/// <returns>true if validation of preferred/ valid lifetime is successful, false otherwise</returns>
		private static bool CompareIPv6LifeTime(Collection<IPv6Details> ipv6Details, TimeSpan lifeTime, bool isPreferredLifetime, TimeSpan? routerLifeTime  = null)
		{
			IPAddress statefulAddress = IPv6Utils.GetStatefulAddress(ipv6Details);
			IPAddress statelessIPAddress = IPv6Utils.GetStatelessIPAddress(ipv6Details)[0];

			if (isPreferredLifetime)
			{
				TimeSpan currentPreferredLifetime = IPv6Utils.GetPreferredLifetime(ipv6Details, statefulAddress);

				if (lifeTime < currentPreferredLifetime)
				{
					TraceFactory.Logger.Info("Preferred Lifetime for Statefull address: {0} is not updated.".FormatWith(statefulAddress));
					return false;
				}

				TraceFactory.Logger.Info("Preferred Lifetime for Statefull address is updated with finite lifetime. Current Preferred Lifetime: {0} seconds.".
											FormatWith(currentPreferredLifetime.TotalSeconds));

				currentPreferredLifetime = IPv6Utils.GetPreferredLifetime(ipv6Details, statelessIPAddress);

				if (null != routerLifeTime)
				{
					if (routerLifeTime < currentPreferredLifetime)
					{
						TraceFactory.Logger.Info("Preferred Lifetime for Stateless address: {0} is not updated.".FormatWith(statelessIPAddress));
						return false;
					}
				}

				TraceFactory.Logger.Info("Preferred Lifetime for Stateless address is updated with finite lifetime. Current Preferred Lifetime: {0}:{1}:{2}:{3} (dd:hh:mm:ss)".
						FormatWith(currentPreferredLifetime.Days, currentPreferredLifetime.Hours, currentPreferredLifetime.Minutes, currentPreferredLifetime.Seconds));
			}
			else
			{
				TimeSpan currentValidLifetime = IPv6Utils.GetValidLifetime(ipv6Details, statefulAddress);

				if (lifeTime < currentValidLifetime)
				{
					TraceFactory.Logger.Info("Valid Lifetime for Statefull address: {0} is not updated.".FormatWith(statefulAddress));
					return false;
				}

				TraceFactory.Logger.Info("Valid Lifetime for Statefull address is updated with finite lifetime. Current Valid Lifetime: {0} seconds.".
											FormatWith(currentValidLifetime.TotalSeconds));

				currentValidLifetime = IPv6Utils.GetValidLifetime(ipv6Details, statelessIPAddress);

				if (null != routerLifeTime)
				{
					if (routerLifeTime < currentValidLifetime)
					{
						TraceFactory.Logger.Info("Valid Lifetime for Stateless address: {0} is not updated.".FormatWith(statelessIPAddress));
						return false;
					}
				}

				TraceFactory.Logger.Info("Valid Lifetime for Stateless address is updated with finite lifetime. Current Valid Lifetime: {0}:{1}:{2}:{3} (dd:hh:mm:ss)".
						FormatWith(currentValidLifetime.Days, currentValidLifetime.Hours, currentValidLifetime.Minutes, currentValidLifetime.Seconds));
			}

			return true;
		}

		/// <summary>
		/// Checks whether the given IP Address is Legacy IP.
		/// </summary>
		/// <param name="ipAddress">The IP Address.</param>
		/// <returns>True if the given IP Address is a legacy IP, else false.</returns>
		private static bool IsPrinterInLegacyIP(string ipAddress)
		{
			if (Printer.Printer.LegacyIPAddress.Equals(IPAddress.Parse(ipAddress)))
			{
				TraceFactory.Logger.Info("Printer has acquired a Legacy IP Address.");
				return true;
			}
			else
			{
				TraceFactory.Logger.Info("Printer failed acquire a Legacy IP Address.");
				return false;
			}
		}

		/// <summary>
		/// Validate Configured Parameters: HostName, DomainName, PrimaryDNSServer, SecondaryDNSServer, PrimaryWINSServer, SecondaryWINSServer using EWS/ Telnet and SNMP
		/// </summary>
		/// <param name="hostName">Host name of Printer to be validated</param>
		/// <param name="domainName">Domain name of Printer to be validated</param>
		/// <param name="address">WINS/ DNS Server IP Address</param>
		/// <param name="family"><see cref=" PrinterFamilies"/></param>
		/// <returns>true if all parameters are validated successfully, false otherwise</returns>
		private static bool ValidateConfiguredParameters(string hostName, string domainName, string address, PrinterFamilies family)
		{
			if (!ValidateHostName(family, hostName))
			{
				return false;
			}

			if (!ValidateDomainName(family, domainName))
			{
				return false;
			}

			if (!ValidatePrimaryDnsServer(family, address))
			{
				return false;
			}

			if (!ValidatePrimaryWinsServer(family, address))
			{
				return false;
			}

			if (!ValidateSecondaryDnsServer(family, address))
			{
				return false;
			}

			// TPS doesnt support Secondary Wins Address
			if (PrinterFamilies.TPS.Equals(family))
			{
				return true;
			}
			else
			{
				return ValidateSecondaryWinsServer(family, address);
			}
		}

		/// <summary>
		/// Configure Bootp and Tffp files.
		/// </summary>
		/// <remarks>
		/// Following steps are followed:
		/// 1. Copy default pre-existing file from share location to temporary location
		/// 2. Modify configuration values
		/// 3. Copy the modified file to Linux server
		/// 4. Restart the service for configuration file to take effect
		/// </remarks>		
		/// <param name="address">Linux server IP Address</param>
		/// <param name="hostName">Host name to be configured</param>
		/// <param name="domainName">Domain name to be configured</param>
		private static void ConfigureTftpFile(IPAddress address, string hostName, string domainName)
		{
			TraceFactory.Logger.Debug("Configuring Tftp file.");

			string winFilePath = Path.Combine(Path.GetTempPath(), LinuxUtils.TFTP_FILE);
			string linuxFilePath = string.Concat(LinuxUtils.BACKUP_FOLDER_PATH, LinuxUtils.TFTP_FILE);

			// Copy to temp directory
			LinuxUtils.CopyFileFromLinux(address, linuxFilePath, winFilePath);

			// Configure all values in tftp file
			Collection<string> keyValue = new Collection<string>();
			
			keyValue.Add(LinuxUtils.KEY_HOST_NAME);
			keyValue.Add(LinuxUtils.KEY_DOMAIN_NAME);
			keyValue.Add(LinuxUtils.KEY_PRIMARY_WINS);
			keyValue.Add(LinuxUtils.KEY_SECONDARY_WINS);
			keyValue.Add(LinuxUtils.KEY_PRIMARY_DNS);
			keyValue.Add(LinuxUtils.KEY_SECONDARY_DNS);

			Collection<string> configureValue = new Collection<string>();

			configureValue.Add(hostName);
			configureValue.Add(domainName);
			configureValue.Add(address.ToString());
			configureValue.Add(address.ToString());
			configureValue.Add(address.ToString());
			configureValue.Add(address.ToString());

			LinuxUtils.ConfigureFile(address, LinuxServiceType.TFTP, winFilePath, keyValue, configureValue);
		}

		/// <summary>
		/// Configure Dhcp file
		/// </summary>
		/// <param name="address">Linux Server IP address</param>
		/// <param name="fromRange">Scope From Range</param>
		/// <param name="toRange">Scope To Range</param>
		/// <param name="hostName">Host name</param>
		/// <param name="domainName">Domain name</param>
		private static void ConfigureDhcpFile(IPAddress address, string fromRange, string toRange, string hostName, string domainName)
		{
			TraceFactory.Logger.Debug("Configuring Dhcp file.");

			string winFilePath = Path.Combine(Path.GetTempPath(), LinuxUtils.DHCP_FILE);
			string linuxFilePath = string.Concat(LinuxUtils.BACKUP_FOLDER_PATH, LinuxUtils.DHCP_FILE);

			int length = address.ToString().Length;
			int lastIndex = address.ToString().LastIndexOf('.') + 1;			
			string subnetMask = string.Format(CultureInfo.CurrentCulture, "{0}{1}", address.ToString().Remove(lastIndex, length - lastIndex), "0");

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
			configureValue.Add(string.Empty);

			LinuxUtils.ConfigureFile(address, LinuxServiceType.DHCP, winFilePath, keyValue, configureValue);
		}

		/// <summary>
		/// Configure DHCPv6 File
		/// </summary>
		/// <param name="address">Linux Server IP Address</param>
		/// <param name="hostname">Host name to be set</param>
		/// <param name="domainname">Domain name to be set</param>
		private static void ConfigureDhcpv6File(IPAddress address, string hostname, string domainname, Printer.PrinterFamilies family)
		{
			TraceFactory.Logger.Debug("Configuring Dhcpv6 file.");

			string winFilePath = Path.Combine(Path.GetTempPath(), LinuxUtils.DHCPV6_FILE);
			string linuxFilePath = string.Concat(LinuxUtils.BACKUP_FOLDER_PATH, LinuxUtils.DHCPV6_FILE);

			/* Hostname Configuration for DHCPv6 differs according to Product Family
			 * --------------------------------------------------------------------
			 * VEP, LFP: First 2 characters are discarded
			 * TPS: Conact(1, 10, hostname, 0) should be used
			 */
			
			if (Printer.PrinterFamilies.TPS.Equals(family))
			{
				hostname = "= concat(1, 10, \"{0}\", 0)".FormatWith(hostname);
			}
			else if (Printer.PrinterFamilies.VEP.Equals(family) || Printer.PrinterFamilies.LFP.Equals(family))
			{
				hostname = string.Format("\"hn{0}\"".FormatWith(hostname));
			}

			// Copy to temp directory
			LinuxUtils.CopyFileFromLinux(address, linuxFilePath, winFilePath);

			Collection<string> keyValue = new Collection<string>();
			keyValue.Add(LinuxUtils.KEY_VALID_LIFETIME);
			keyValue.Add(LinuxUtils.KEY_PREFERED_LIFETIME);
			keyValue.Add(LinuxUtils.KEY_SUBNET_MASK);
			keyValue.Add(LinuxUtils.KEY_FROM_RANGE);
			keyValue.Add(LinuxUtils.KEY_TO_RANGE);
			keyValue.Add(LinuxUtils.KEY_PREFIX);
			keyValue.Add(LinuxUtils.KEY_HOST_NAME);
			keyValue.Add(LinuxUtils.KEY_DOMAIN_NAME);
			keyValue.Add(LinuxUtils.KEY_PRIMARY_DNS);
			keyValue.Add(LinuxUtils.KEY_LOG_SERVER);

			string toRange = LinuxUtils.GetDhcpv6ToRange(address) ;

			Collection<string> configureValue = new Collection<string>();
			configureValue.Add(TimeSpan.FromSeconds(240).TotalSeconds.ToString());
			configureValue.Add(TimeSpan.FromSeconds(120).TotalSeconds.ToString());
			configureValue.Add(LinuxUtils.GetDhcpv6SubnetMask(address));
			configureValue.Add(LinuxUtils.GetDhcpv6FromRange(address));
			configureValue.Add(toRange);
			configureValue.Add(LinuxUtils.GetDhcpv6Prefix(address));
			configureValue.Add( hostname);
			configureValue.Add(domainname);
			configureValue.Add(toRange);
			configureValue.Add(toRange);

			LinuxUtils.ConfigureFile(address, LinuxServiceType.DHCPV6, winFilePath, keyValue, configureValue);
		}

		/// <summary>
		/// Verify whether stateless addresses are updated on EWS
		/// </summary>
		/// <returns>true if Stateless addresses are available, false otherwise</returns>
		private static bool VerifyStatelessAddress()
		{
			Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails();

			if (IPv6Utils.GetStatelessIPAddress(ipv6Details).Count > 0)
			{
				TraceFactory.Logger.Info("Stateless addresses are acquired from router.");
				return true;
			}
			else
			{
				TraceFactory.Logger.Info("Stateless addresses are not acquired from router.");
				return false;
			}
		}

		/// <summary>
		/// Validates if the printer is either in Legacy IP or Auto IP.
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <returns>true if the printer is either in Auto IP or Legacy IP.</returns>
		private static bool ValidateDefaultIP(IPConfigurationActivityData activityData)
		{
			string ipAddress = string.Empty;
			return ValidateDefaultIP(activityData, ref ipAddress);
		}

		/// <summary>
		/// Validates if the printer is either in Legacy IP or Auto IP.
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <param name="ipAddress">Current IP Address of the printer.</param>
		/// <returns>true if the printer is either in Auto IP or Legacy IP.</returns>
		private static bool ValidateDefaultIP(IPConfigurationActivityData activityData, ref string ipAddress)
		{
			string deviceAddress      = string.Empty;
			string hostName           = string.Empty;
			DefaultIPType defaultType = DefaultIPType.None;

			if (Utility.IsPrinterInDefaultIP(activityData.PrimaryMacAddress, out deviceAddress, out hostName, out defaultType))
			{
				ipAddress = deviceAddress;

				if (defaultType == DefaultIPType.AutoIP)
				{
					TraceFactory.Logger.Info("Printer is in Auto IP: {0}".FormatWith(deviceAddress));

					return ValidateIPConfigMethod(activityData, IPConfigMethod.AUTOIP, deviceAddress);
				}
				else if (defaultType == DefaultIPType.LegacyIP)
				{
					TraceFactory.Logger.Info("Printer is in Legacy IP: {0}".FormatWith(deviceAddress));

					// When the device is in Legacy IP, Validate Config Method from Web UI using Host name of the printer.
					EwsWrapper.Instance().ChangeHostName(hostName);

					if (EwsWrapper.Instance().GetIPConfigMethod() == IPConfigMethod.Manual)
					{
						TraceFactory.Logger.Info("EWS: Successfully validated the IP config method Manual.");
						return true;
					}
					else
					{
						TraceFactory.Logger.Info("EWS: Failed to validate the IP config method Manual.");
						return false;
					}
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

		/// <summary>
		/// Verify Reinitialize Now behavior for the specified <see cref="IPConfigMethod"/>.
		/// Please refer to Templates referring the method.
		/// STEPI. Check for device IP address after “Reinitialize Now” using DHCP/BOOTP IP	
		/// "1. Connect the device in a network with DHCP server.
		/// 2. Confirm that the device has acquired an IP address from the DHCP server.
		/// 3. Click""Reinitialize Now"" in config precedence tab
		/// Note : Only for VEP."	
		/// Expected : "1. Device should start the DHCP process (of sending Discover, Offer,Request and Acknowledge) and acquires same DHCPv4 address which it had before, if the same IP is available or should acquire a new DHCPv4 address."	
		/// "Expected result modified with DHCP process"
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <param name="configMethod"><see cref="IPConfigMethod"/></param>
		/// <returns>true if the validation is successful, else false.</returns>
		private static bool VerifyReinializeNow(IPConfigurationActivityData activityData, IPConfigMethod configMethod, PrinterConnectionType printerInterface = PrinterConnectionType.PrimaryWiredInterface, string packetFileName = "", bool cleanUp = true)
		{
			PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
			string guid = client.Channel.TestCapture(activityData.SessionId, packetFileName);

			string ipAddress = string.Empty;

			try
			{
				if (printerInterface != PrinterConnectionType.PrimaryWiredInterface)
				{
					if (configMethod == IPConfigMethod.DHCP)
					{
						TraceFactory.Logger.Info("Change lease time to 10 minute on DHCP server");

						DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
						serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 600);
					}

					TraceFactory.Logger.Info("Bring up the device in {0} IP for Embedded Wired, Europa Wired and Wireless interfaces.".FormatWith(configMethod));

					Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails = GetInterfaceDetails(activityData);

                    printerInterfaceDetails.ForEach(x => x.ConfigMethod = configMethod);
					if (!ConfigureInterface(printerInterfaceDetails, false))
					{
						return false;
					}

					TraceFactory.Logger.Info("Confirm that the device has acquired an IP address from the DHCP server.");

					if (!ValidateIPConfigMethod(activityData, printerInterfaceDetails))
					{
						return false;
					}

					ipAddress = printerInterfaceDetails.Where(x => x.PrinterInterface == printerInterface).FirstOrDefault().Address;
                    EwsWrapper.Instance().ChangeDeviceAddress(ipAddress);
				}
				else
				{
					TraceFactory.Logger.Info("Connect the device in a network with {0} server".FormatWith(configMethod));

					if (!EwsWrapper.Instance().SetIPConfigMethod(configMethod, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
					{
						return false;
					}

					if (!ValidateIPConfigMethod(activityData, configMethod, ipAddress: ipAddress))
					{
						return false;
					}
				}

				if (!EwsWrapper.Instance().ReinitializeConfigPrecedence())
				{
					return false;
				}

				// TODO: Packet Validation

				// Check for printer availability
				if (!CheckForPrinterAvailability(ipAddress))
				{
					return false;
				}

				//serviceMethods.DeleteReservation(activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, "2");
				//serviceMethods.CreateReservation(activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, Enum<ReservationType>.Value(ReservationType.Both));

				return true;
			}
			finally
			{
				client.Channel.Stop(guid);
				EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
				if (configMethod == IPConfigMethod.DHCP)
				{
					TraceFactory.Logger.Info("Change lease time to 10 minute on DHCP server");

					DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
					serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, DEFAULT_LEASE_TIME);
				}
			}
		}

		/// <summary>
		/// Clear Previous Values and Reinitialize now.
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <param name="configMethod"><see cref="IPConfigMethod"/> to be validated.</param>
		/// <returns>returns true for success, else false.</returns>
		private static bool ClearPreviousValuesAndReinitializeNow(IPConfigurationActivityData activityData, IPConfigMethod configMethod, PrinterConnectionType printerInterface = PrinterConnectionType.PrimaryWiredInterface, string packetFileName = "")
		{
			PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
			string guid = client.Channel.TestCapture(activityData.SessionId, packetFileName);

			try
			{
				string currentDeviceAddress = string.Empty;
                string macAddress = string.Empty;

				if (configMethod != IPConfigMethod.AUTOIP)
				{
					if (printerInterface != PrinterConnectionType.PrimaryWiredInterface)
					{
						if (configMethod == IPConfigMethod.DHCP)
						{
							TraceFactory.Logger.Info("Change lease time to 10 minute on DHCP server");

							DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
							serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 600);
						}

						TraceFactory.Logger.Info("Bring up the device in {0} IP for Embedded Wired, Europa Wired and Wireless interfaces.".FormatWith(configMethod));

						Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails = GetInterfaceDetails(activityData);

                        printerInterfaceDetails.ForEach(x => x.ConfigMethod = configMethod);

                        if (!ConfigureInterface(printerInterfaceDetails, false))
						{
							return false;
						}

						TraceFactory.Logger.Info("Confirm that the device has acquired an IP address from the DHCP server.");

						if (!ValidateIPConfigMethod(activityData, printerInterfaceDetails))
						{
							return false;
						}

						currentDeviceAddress = printerInterfaceDetails.Where(x => x.PrinterInterface == printerInterface).FirstOrDefault().Address;
                        macAddress = printerInterfaceDetails.FirstOrDefault(x => x.PrinterInterface == printerInterface).MacAddress;

                        EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);
					}
					else
					{
                        macAddress = activityData.PrimaryMacAddress;

						if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address), validate: false))
						{
							return false;
						}

						currentDeviceAddress = Utility.GetPrinterIPAddress(activityData.PrimaryMacAddress);

						if (string.IsNullOrEmpty(currentDeviceAddress))
						{
							return false;
						}

						EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);

						if (!ValidateIPConfigMethod(activityData, IPConfigMethod.DHCP, currentDeviceAddress))
						{
							return false;
						}

						if (configMethod == IPConfigMethod.BOOTP)
						{
							DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
							serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
							serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Bootp);
						}
					}
				}
				else
				{
					DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

					if (!serviceMethod.Channel.StopDhcpServer())
					{
						return false;
					}
				}

				if (!EwsWrapper.Instance().ClearAndReinitializeConfigPrecedence())
				{
					return false;
				}

				Thread.Sleep(TimeSpan.FromMinutes(3));

				// If the config method Auto IP, renew the client machine and perform the validation.
				if (configMethod == IPConfigMethod.AUTOIP)
				{
					NetworkUtil.RenewLocalMachineIP();
				}

				currentDeviceAddress = Utility.GetPrinterIPAddress(macAddress);

				if (!CheckForPrinterAvailability(currentDeviceAddress))
				{
					return false;
				}

				EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);

				return ValidateIPConfigMethod(activityData, configMethod, currentDeviceAddress);
			}
			finally
			{
				client.Channel.Stop(guid);
			}
		}

		/// <summary>
		/// Set server options to default
		/// Get printer configured with server settings
		/// Reset Config Precedence to default
		/// </summary>
		/// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
		private static void RestoreServerParameter(IPConfigurationActivityData activityData, bool includeLog = true)
		{
			if (includeLog)
			{
				TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
			}

			DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

			serviceMethod.Channel.SetHostName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.ServerHostName);
			serviceMethod.Channel.SetDomainName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.DomainName);
			// We need to add both primary and secondary address for Dns and Wins (Adding Server address as Primary & Secondary address)
			serviceMethod.Channel.SetDnsServer(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress,  "{0} {0}".FormatWith(activityData.PrimaryDhcpServerIPAddress));
			serviceMethod.Channel.SetWinsServer(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, "{0} {0}".FormatWith(activityData.PrimaryDhcpServerIPAddress));

			// To get the settings from server configured on printer, we configure dhcp as highest precedence
			TraceFactory.Logger.Debug("Setting DHCP/ BootP as highest precedence.");
			SnmpWrapper.Instance().SetConfigPrecedence("2:0:1:3:4");
			Thread.Sleep(TimeSpan.FromMinutes(1));

			EwsWrapper.Instance().ResetConfigPrecedence();
			Thread.Sleep(TimeSpan.FromMinutes(1));
		}

		/// <summary>
		/// Validate hostname, domain name, dns primary and wins primary address is set to default using EWS and SNMP.
		/// </summary>
		/// <returns>true if all parameters are set to default, false otherwise</returns>
		private static bool ValidateDefaultParameter()
		{
			// Default values:	NULL - empty
			//		Hostname			|	Domain name		|	other Server IP's
			// ----------------------------------------------------------------------
			// VEP: starts with 'NPI'	|	NULL			|	NULL
			// TPS: starts with 'NPI'	|	Not Specified	|	Not Specified

			string hostName = EwsWrapper.Instance().GetHostname();

			if (!hostName.StartsWith("NPI", StringComparison.CurrentCultureIgnoreCase))
			{
				TraceFactory.Logger.Info("Printer didn't acquire default host name. Current host name: {0}".FormatWith(hostName));
				return false;
			}
			else
			{
				TraceFactory.Logger.Info("Printer has acquired default host name: {0}".FormatWith(hostName));
			}

			string domainName = EwsWrapper.Instance().GetDomainName().Trim();

			if (!(string.IsNullOrEmpty(domainName) || domainName.EqualsIgnoreCase("Not Specified")))
			{
				TraceFactory.Logger.Info("Printer domain name is not cleared. Current domain name: {0}".FormatWith(domainName));
				return false;
			}
			else
			{
				TraceFactory.Logger.Info("Printer domain name is cleared.");
			}

			string dnsPrimaryIP = EwsWrapper.Instance().GetDNSServerIP().Trim();

			if (!(string.IsNullOrEmpty(dnsPrimaryIP) || dnsPrimaryIP.EqualsIgnoreCase("Not Specified")))
			{
				TraceFactory.Logger.Info("Dns Primary IP is not cleared. Current Dns Primary IP: {0}".FormatWith(dnsPrimaryIP));
				return false;
			}
			else
			{
				TraceFactory.Logger.Info("Dns Primary IP is cleared.");
			}

			string winsServerIP = EwsWrapper.Instance().GetPrimaryWinServerIP().Trim();

			if (!(string.IsNullOrEmpty(winsServerIP) || winsServerIP.EqualsIgnoreCase("Not Specified")))
			{
				TraceFactory.Logger.Info("Wins Primary IP is not cleared. Current Wins Primary IP: {0}".FormatWith(winsServerIP));
				return false;
			}
			else
			{
				TraceFactory.Logger.Info("Wins Primary IP is cleared.");
			}

			return true;
		}

		/// <summary>
		/// Verify whether manually configured hostname is reflected on EWS, SNMP and Telnet once server parameters are removed
		/// </summary>
		/// <param name="activityData"></param>
		/// <returns></returns>
		private static bool VerifyConfiguredParameter(IPConfigurationActivityData activityData, PrinterAccessType type)
		{
			DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

			try
			{
				TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

				TraceFactory.Logger.Info("Setting DHCPv4/BootP as highest config precedence.");
				SnmpWrapper.Instance().SetConfigPrecedence("2:0:1:3:4");
				serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 60);

				EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address), validate: false);
				EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address), validate: false);				

				PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

				if (!ValidateHostName(family, activityData.ServerHostName))
				{
					return false;
				}

				if (!ValidateDomainName(family, activityData.DomainName))
				{
					return false;
				}

				if (!ValidatePrimaryDnsServer(family, activityData.ServerDNSPrimaryIPAddress))
				{
					return false;
				}

				if (!ValidatePrimaryWinsServer(family, serviceMethod.Channel.GetPrimaryWinsServer(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress)))
				{
					return false;
				}

				DeleteServerParameter(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);
				Thread.Sleep(TimeSpan.FromMinutes(1));

				string manualHostName = "Manual_HostName";
				string manualDomainName = "Manual_DomainName";
				string serverIP = activityData.PrimaryDhcpServerIPAddress;

				if (PrinterAccessType.EWS.Equals(type))
				{
					EwsWrapper.Instance().SetHostname(manualHostName);
					EwsWrapper.Instance().SetDomainName(manualDomainName);
					EwsWrapper.Instance().SetPrimaryDnsServer(serverIP);
					EwsWrapper.Instance().SetPrimaryWinServerIP(serverIP);
				}
				else if (PrinterAccessType.SNMP.Equals(type))
				{
					SnmpWrapper.Instance().SetHostName(manualHostName);
					SnmpWrapper.Instance().SetDomainName(manualDomainName);

					if (!PrinterFamilies.TPS.Equals(family))
					{
						SnmpWrapper.Instance().SetPrimaryDnsServer(serverIP);
						SnmpWrapper.Instance().SetPrimaryWinsServer(serverIP);
					}
					else
					{
						//after changing domain name of the printer,delay is required to move to the next operation of EWS for TPS
						Thread.Sleep(TimeSpan.FromMinutes(1));
					}
				}
				else if (PrinterAccessType.Telnet.Equals(type))
				{
					TelnetWrapper.Instance().SetHostname(manualHostName);
					TelnetWrapper.Instance().SetDomainname(manualDomainName);
					TelnetWrapper.Instance().SetPrimaryDnsServer(serverIP);
					TelnetWrapper.Instance().SetPrimaryWinsServer(serverIP);
				}

				if (!ValidateHostName(family, manualHostName))
				{
					return false;
				}

				if (!ValidateDomainName(family, manualDomainName))
				{
					return false;
				}

				if (!ValidatePrimaryDnsServer(family, serverIP))
				{
					return false;
				}

				return ValidatePrimaryWinsServer(family, serverIP);
			}
			finally
			{
				RestoreServerParameter(activityData);
			}
		}

		/// <summary>
		/// Delete Hostname, Domain name, Primary Dns and Wins server details from DHCP server
		/// </summary>
		/// <param name="serverAddress">Server Address</param>
		/// <param name="scopeAddress">Scope Address</param>
		private static void DeleteServerParameter(string serverAddress, string scopeAddress)
		{
			DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(serverAddress);

			serviceMethod.Channel.DeleteHostName(serverAddress, scopeAddress);
			serviceMethod.Channel.DeleteDomainName(serverAddress, scopeAddress);
			serviceMethod.Channel.DeleteDnsServer(serverAddress, scopeAddress);
			serviceMethod.Channel.DeleteWinsServer(serverAddress, scopeAddress);
		}

		/// <summary>
		/// Validate whether Manual is configured as highest Config Precedence using EWS and SNMP
		/// </summary>
		/// <returns></returns>
		private static bool ValidateManualHighestPrecedence()
		{
			if (EwsWrapper.Instance().GetConfigPrecedence().EqualsIgnoreCase("Manual"))
			{
				TraceFactory.Logger.Info("Manual is configured as highest config precedence from EWS.");
			}
			else
			{
				TraceFactory.Logger.Info("Manual is not configured as highest config precedence from EWS.");
				return false;
			}

			if (SnmpWrapper.Instance().GetConfigPrecedence().StartsWith("0", StringComparison.CurrentCultureIgnoreCase))
			{
				TraceFactory.Logger.Info("Manual is configured as highest config precedence from SNMP.");
			}
			else
			{
				TraceFactory.Logger.Info("Manual is not configured as highest config precedence from SNMP.");
				return false;
			}

			return true;
		}

		/// <summary>
		/// Get current Lease time
		/// </summary>
		/// <param name="family">Product Family</param>
		/// <param name="expectedLease">Expected lease time in seconds</param>
		/// <returns>t</returns>
		private static bool ValidateLeaseTime(string family, int expectedLease)
		{
			int currentLeaseTime = 0;
			bool validateStatus = false;

			// For TPS, lease time is the current lease on Printer
			if (PrinterFamilies.TPS.ToString().Equals(family))
			{
				currentLeaseTime = int.Parse(TelnetWrapper.Instance().GetConfiguredValue(PrinterFamilies.TPS, "leasetime"), CultureInfo.CurrentCulture);
				if (currentLeaseTime <= expectedLease)
				{
					validateStatus = true;
				}
			}
				// For VEP/ LFP, lease time is DHCP configured time
			else
			{
				currentLeaseTime = int.Parse(SnmpWrapper.Instance().GetLeaseTime(), CultureInfo.CurrentCulture);
				if (currentLeaseTime == expectedLease)
				{
					validateStatus = true;
				}
			}

			TraceFactory.Logger.Debug("Current lease time: {0} seconds.".FormatWith(currentLeaseTime));
			return validateStatus;
		}

		/// <summary>
		/// Verify manual hostname is not accepted when config precedence is set to DHCP/ BootP
		/// </summary>
		/// <param name="configMethod"></param>
		/// <param name="activityData"></param>
		/// <returns></returns>
		private static bool ValidateDhcpBootPPrecedence(IPConfigMethod configMethod, IPConfigurationActivityData activityData)
		{
			string manualHostname = "Manual_Hostname";
			string defaultHostname = activityData.ServerHostName;

			DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
			serviceMethod.Channel.DeleteHostName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);

			EwsWrapper.Instance().SetIPConfigMethod(configMethod, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
			EwsWrapper.Instance().SetHostname(manualHostname);

			serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 60);
			serviceMethod.Channel.SetHostName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, defaultHostname);

			TraceFactory.Logger.Info("Setting DHCPv4/ Bootp as highest precedence");
			SnmpWrapper.Instance().SetConfigPrecedence("2:0:1:3:4");

			PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

			if (!PrinterFamilies.TPS.Equals(family))
			{
				EwsWrapper.Instance().ReinitializeConfigPrecedence();
			}
			else
			{
				//To update Server Parameters
				Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
				printer.PowerCycle();
			}			

			if (!ValidateHostName(family, defaultHostname))
			{
				return false;
			}

			TraceFactory.Logger.Info("Setting parameters manually will fail since DHCP/ BootP is set as highest config precedence as expected:");

			if (EwsWrapper.Instance().SetHostname(manualHostname))
			{
				return false;
			}

			TelnetWrapper.Instance().SetHostname(manualHostname);
			if (manualHostname.EqualsIgnoreCase(TelnetWrapper.Instance().GetConfiguredValue(family, "hostname")))
			{
				return false;
			}

			if (SnmpWrapper.Instance().SetHostName(manualHostname))
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Verify if newly configured hostname is reflected on printer
		/// </summary>
		/// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
		/// <returns>true if hostname is updated, false otherwise</returns>
		private static bool VerifyNewHostnameConfiguration(IPConfigMethod configMethod, IPConfigurationActivityData activityData)
		{
			TraceFactory.Logger.Info("Executing with {0} config method:".FormatWith(configMethod));
			string hostName = "New_Hostname";

			DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
			serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, 60);

			if (IPConfigMethod.BOOTP.Equals(configMethod) && PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily))
			{
				serviceMethod.Channel.SetBootPHostName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, hostName);
			}
			else
			{
				serviceMethod.Channel.SetHostName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, hostName);
			}

			// To get the latest lease time configured on printer, we change the config method
			EwsWrapper.Instance().SetIPConfigMethod(configMethod == IPConfigMethod.DHCP ? IPConfigMethod.BOOTP : IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
			EwsWrapper.Instance().SetIPConfigMethod(configMethod, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

			TraceFactory.Logger.Info("Setting DHCPv4/BootP as highest config precedence.");
			SnmpWrapper.Instance().SetConfigPrecedence("2:0:1:3:4");
			
			Thread.Sleep(TimeSpan.FromSeconds(60));

			PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

			return ValidateHostName(family, hostName);
		}

		/// <summary>
		/// Validate parameters on Stateless option enable/ disable
		/// </summary>
		/// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>		
		private static bool ValidateParameterWithStatelessOption(IPConfigurationActivityData activityData, IPConfigMethod configMethod, int testNo, PrinterConnectionType interfaceConnection = PrinterConnectionType.PrimaryWiredInterface, bool validateInterfaces = false, bool cleanUp = true)
		{
            string address = activityData.PrimaryWiredIPv4Address;
            string macAddress = activityData.PrimaryMacAddress;

            DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string serverHostName = serviceMethod.Channel.GetHostName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);
            string serverDomainName = serviceMethod.Channel.GetDomainName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);
            string serverPrimaryDns = serviceMethod.Channel.GetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);

            string newHostName = "New_Hostname";
            string newDomainName = "New_Domainname";
            string newPrimaryDns = "127.0.0.1";

            try
            {
                if (validateInterfaces)
                {
                    Collection<PrinterInterfaceConfigurationDetails> printerInterfaces = GetInterfaceDetails(activityData);

                    printerInterfaces.ForEach(x => x.ConfigMethod = configMethod);

                    if (!ConfigureInterface(printerInterfaces, false))
                    {
                        return false;
                    }

                    address = printerInterfaces.FirstOrDefault(x => x.PrinterInterface == interfaceConnection).Address;
                    macAddress = printerInterfaces.FirstOrDefault(x => x.PrinterInterface == interfaceConnection).MacAddress;
                    EwsWrapper.Instance().ChangeDeviceAddress(address);

                    serviceMethod.Channel.SetHostName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, newHostName);
                    serviceMethod.Channel.SetDomainName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, newDomainName);
                    serviceMethod.Channel.SetDnsServer(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, newPrimaryDns);
                }

                EwsWrapper.Instance().SetIPConfigMethod(configMethod, expectedIPAddress: IPAddress.Parse(address));

                TraceFactory.Logger.Info("Setting DHCPv4/BootP as highest config precedence on {0}.".FormatWith(interfaceConnection));
                SnmpWrapper.Instance().SetConfigPrecedence("2:0:1:3:4");

                EwsWrapper.Instance().SetStatelessDHCPv4(true);

                string validateGuid = string.Empty;
                PacketDetails validatePacketDetails = null;
                PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                try
                {
                    validateGuid = client.Channel.TestCapture(activityData.SessionId, string.Format("{0}_{1}_Validation", testNo, configMethod == IPConfigMethod.DHCP ? "Dhcp" : "BootP"));
                    EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual);
                }
                finally
                {
                    if (!string.IsNullOrEmpty(validateGuid))
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(3));
                        validatePacketDetails = client.Channel.Stop(validateGuid);
                    }
                }

                // The specified interface takes the newly configured parameters from server
                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                if (!ValidateHostName(family, newHostName, address))
                {
                    return false;
                }

                if (!ValidateDomainName(family, newDomainName, address))
                {
                    return false;
                }

                if (!ValidatePrimaryDnsServer(family, newPrimaryDns, address))
                {
                    return false;
                }

                // Other interfaces should retain the old parameters from servers.
                if (validateInterfaces)
                {
                    foreach (var item in GetInterfaceDetails(activityData).Where(x => x.PrinterInterface != interfaceConnection))
                    {
                        if (!ValidateHostName(family, serverHostName, address))
                        {
                            return false;
                        }

                        if (!ValidateDomainName(family, serverDomainName, address))
                        {
                            return false;
                        }

                        if (!ValidatePrimaryDnsServer(family, serverPrimaryDns, address))
                        {
                            return false;
                        }
                    }
                }

                EwsWrapper.Instance().SetStatelessAddress(false);
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual);

                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (!ValidateHostName(family, serverHostName, address))
                {
                    return false;
                }

                if (!ValidateDomainName(family, serverDomainName, address))
                {
                    return false;
                }

                if (ValidatePrimaryDnsServer(family, serverPrimaryDns, address))
                {
                    if (!ValidateDhcpInformPackets(activityData.PrimaryDhcpServerIPAddress, validatePacketDetails.PacketsLocation, macAddress))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                if (validateInterfaces)
                {
                    foreach (var item in GetInterfaceDetails(activityData).Where(x => x.PrinterInterface != interfaceConnection))
                    {
                        if (!ValidateHostName(family, serverHostName))
                        {
                            return false;
                        }

                        if (!ValidateDomainName(family, serverDomainName))
                        {
                            return false;
                        }

                        if (!ValidatePrimaryDnsServer(family, serverPrimaryDns))
                        {
                            return false;
                        }

                        if (!ValidateDhcpInformPackets(activityData.PrimaryDhcpServerIPAddress, validatePacketDetails.PacketsLocation, item.MacAddress))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            finally
            {
                if (cleanUp)
                {
                    serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                    serviceMethod.Channel.SetHostName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.ServerHostName);
                    serviceMethod.Channel.SetDomainName(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.DomainName);
                    serviceMethod.Channel.SetDnsServer(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.ServerDNSPrimaryIPAddress);
                }
            }
		}

		/// <summary>
		/// Get manual IPv6 address from stateless address
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <returns>The manual IPv6 address</returns>
		private static string GetManualIPv6Address(IPConfigurationActivityData activityData)
		{
			// Ensure that IPv6 is enabled so that an IP can be fetch from the printer.
			EwsWrapper.Instance().SetIPv6(true);

			string manualIpv6Address = string.Empty;

			Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
			Collection<IPAddress> statelessAddresses = printer.IPv6StatelessAddresses;
			IPAddress statelessAddress = (statelessAddresses != null) && statelessAddresses.Count != 0 ? statelessAddresses[0] : IPAddress.None;

			if (!statelessAddress.Equals(IPAddress.None))
			{
				Random rnd = new Random(1);
				manualIpv6Address = statelessAddress.ToString().Replace(statelessAddress.ToString().Substring(statelessAddress.ToString().LastIndexOf(':') + 1), rnd.Next().ToString("X", CultureInfo.CurrentCulture).Substring(0, 4).ToLower(CultureInfo.CurrentCulture));
			}

			return manualIpv6Address;
		}

		/// <summary>
		/// Validate Link speed through Web UI
		/// </summary>
		/// <param name="ewsLinkSpeedValue">The value to be validated from Web UI configuration page.("1000 T FULL", "100TX FULL" etc.")</param>
		/// <param name="autoNegotiation">The Auto Negotiation value("On" or "Off")</param>
        /// <param name="isTPS">true for TPS, else false</param>
        /// <param name="address">IP address of the interface on which link speed is to be validated.</param>
		/// <returns>True if the validation is successful, else false.</returns>
		private static bool ValidateLinkSpeed(string ewsLinkSpeedValue, string autoNegotiation, bool isTPS = false, string address = "")
		{
			string pageLink = isTPS ? "Summary" : "Configuration_Page";

            if(!string.IsNullOrEmpty(address))
            {
                EwsWrapper.Instance().ChangeDeviceAddress(address);
                SnmpWrapper.Instance().Create(address);
            }

			if (EwsWrapper.Instance().SearchTextInPage("{0}".FormatWith(ewsLinkSpeedValue), pageLink))
			{
				TraceFactory.Logger.Info("Successfully validated port config as '{0}' in configuration page.".FormatWith(ewsLinkSpeedValue));
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate port config as '{0}' in configuration page.".FormatWith(ewsLinkSpeedValue));
				return false;
			}

			if (isTPS)
			{
				if (EwsWrapper.Instance().SearchTextInPage("{0}".FormatWith(autoNegotiation), pageLink))
				{
					TraceFactory.Logger.Info("Successfully validated '{0}' in configuration page.".FormatWith(autoNegotiation));
					return true;
				}
				else
				{
					TraceFactory.Logger.Info("Failed to validate '{0}' in configuration page.".FormatWith(autoNegotiation));
					return false;
				}
			}
			else
			{
				string autoNegotiationValue = SnmpWrapper.Instance().GetAutonegotiation();

				TraceFactory.Logger.Debug(autoNegotiationValue);
				autoNegotiationValue = SnmpWrapper.Instance().GetAutonegotiation().Split(':')[1].Trim();
				TraceFactory.Logger.Debug(autoNegotiationValue);

				if (autoNegotiationValue.EqualsIgnoreCase(autoNegotiation))
				{
					TraceFactory.Logger.Info("Successfully validated '{0}' from Snmp.".FormatWith(autoNegotiation));
					return true;
				}
				else
				{
					TraceFactory.Logger.Info("Failed to validate '{0}' from Snmp.".FormatWith(autoNegotiation));
					return false;
				}
			}
		}

		/// <summary>
		/// Validates if the IPv6 addresses are updated w.r.t the MAC Address.
		/// </summary>
		/// <param name="macAddress">Mac address to be validated.</param>
		/// <returns>true if the IPv6 addresses are matching to the Mac address.</returns>
		private static bool ValidateIPv6AddressWithMacAddress(string macAddress)
		{
			// Validate IPv6 address, link local address
			Collection<IPv6Details> ipv6Details = EwsWrapper.Instance().GetIPv6TableDetails(6);

			Collection<IPAddress> statelessIPAddresses = IPv6Utils.GetStatelessIPAddress(ipv6Details);
			IPAddress linkLocalAddress = IPv6Utils.GetLinkLocalAddress(ipv6Details);

			IPAddress linklocalIPv6 = Utility.GetLinkLocalAddress(macAddress);

			TraceFactory.Logger.Debug("Link local address- Generated: {0}, Printer: {1}".FormatWith(linklocalIPv6, linkLocalAddress));

			if (linkLocalAddress.Equals(linklocalIPv6))
			{
				TraceFactory.Logger.Info("Link local address is updated according to the MAC address.");
			}
			else
			{
				TraceFactory.Logger.Info("Link local address is not updated according to the MAC address.");
				return false;
			}

			TraceFactory.Logger.Debug("Stateless Addresses: {0}".FormatWith(string.Join(",", statelessIPAddresses)));

			// Stateless always ends with the last portion of linkLocal address.(eg;- stateless address can be 2000:202:202:202:202:yOP:QRff:feST:UVWX) 
			foreach (IPAddress statelessAddress in statelessIPAddresses)
			{
				if (statelessAddress.ToString().EndsWith(linkLocalAddress.ToString().Replace("fe80::", string.Empty).Trim(), StringComparison.CurrentCultureIgnoreCase))
				{
					TraceFactory.Logger.Info("Stateless address: {0} is updated according to the MAC address.".FormatWith(statelessAddress));
				}
				else
				{
					TraceFactory.Logger.Info("Stateless address: {0} is not updated according to the MAC address.".FormatWith(statelessAddress));
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Configure Hostname, domain name, dns server address, dnsv6 server address manually and validate the same
		/// </summary>
		/// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
		/// <param name="type"><see cref=" PrinterAccessType"/></param>
		/// <returns>true if all values are validated after setting, false otherwise</returns>
		private static bool ConfigureAndVerifyParameters(IPConfigurationActivityData activityData, PrinterAccessType type)
		{
			string manualHostName = "Manual_HostName";
			string manualDomainName = "Manual_DomainName";
			string dnsServerAddress = activityData.ServerDNSPrimaryIPAddress;
			string dnsv6ServerAddress = activityData.DHCPScopeIPv6Address;

			PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

			if (PrinterAccessType.EWS.Equals(type))
			{
				if (!EwsWrapper.Instance().SetHostname(manualHostName))
				{
					TraceFactory.Logger.Info("Host name is not set manually.");
					return false;
				}

				TraceFactory.Logger.Info("Host name is set to: {0} as expected.".FormatWith(manualHostName));

				if (!EwsWrapper.Instance().SetDomainName(manualDomainName))
				{
					TraceFactory.Logger.Info("Domain name is not set manually.");
					return false;
				}

				TraceFactory.Logger.Info("Domain name is set to: {0} as expected.".FormatWith(manualDomainName));

				EwsWrapper.Instance().SetPrimaryDnsServer(dnsServerAddress);

				if (!EwsWrapper.Instance().GetPrimaryDnsServer().EqualsIgnoreCase(dnsServerAddress))
				{
					TraceFactory.Logger.Info("Primary Dns server address is not set manually.");
					return false;
				}

				TraceFactory.Logger.Info("Dns server address is set to: {0} as expected.".FormatWith(dnsServerAddress));

				EwsWrapper.Instance().SetPrimaryDnsv6Server(dnsv6ServerAddress);

				if (!EwsWrapper.Instance().GetPrimaryDnsv6Server().EqualsIgnoreCase(dnsv6ServerAddress))
				{
					TraceFactory.Logger.Info("Primary Dnsv6 server address is not set manually.");
					return false;
				}

				TraceFactory.Logger.Info("Dnsv6 server address is set to: {0} as expected.".FormatWith(dnsv6ServerAddress));
			}
			else if (PrinterAccessType.SNMP.Equals(type))
			{
				if (!SnmpWrapper.Instance().SetHostName(manualHostName))
				{
					TraceFactory.Logger.Info("Host name is not set manually.");
					return false;
				}

				TraceFactory.Logger.Info("Host name is set to: {0} as expected.".FormatWith(manualHostName));

				if (!SnmpWrapper.Instance().SetDomainName(manualDomainName))
				{
					TraceFactory.Logger.Info("Domain name is not set manually.");
					return false;
				}

				TraceFactory.Logger.Info("Domain name is set to: {0} as expected.".FormatWith(manualDomainName));

				if (!PrinterFamilies.TPS.Equals(family))
				{
					if (!SnmpWrapper.Instance().SetPrimaryDnsServer(dnsServerAddress))
					{
						TraceFactory.Logger.Info("Primary Dns server address is not set manually.");
						return false;
					}

					TraceFactory.Logger.Info("Dns server address is set to: {0} as expected.".FormatWith(dnsServerAddress));

					if (!SnmpWrapper.Instance().SetPrimaryDnsv6Server(dnsv6ServerAddress))
					{
						TraceFactory.Logger.Info("Primary Dnsv6 server address is not set manually.");
						return false;
					}

					TraceFactory.Logger.Info("Dnsv6 server address is set to: {0} as expected.".FormatWith(dnsv6ServerAddress));
				}
			}
			else if (PrinterAccessType.Telnet.Equals(type))
			{
				TelnetWrapper.Instance().SetHostname(manualHostName);

				if (!TelnetWrapper.Instance().GetConfiguredValue(family, "hostname").Contains(manualHostName, StringComparison.CurrentCultureIgnoreCase))
				{
					TraceFactory.Logger.Info("Host name is not set manually.");
					return false;
				}

				TraceFactory.Logger.Info("Host name is set to: {0} as expected.".FormatWith(manualHostName));

				TelnetWrapper.Instance().SetDomainname(manualDomainName);

				if (!TelnetWrapper.Instance().GetConfiguredValue(family, "domainname").Contains(manualDomainName, StringComparison.CurrentCultureIgnoreCase))
				{
					TraceFactory.Logger.Info("Domain name is not set manually.");
					return false;
				}

				TraceFactory.Logger.Info("Domain name is set to: {0} as expected.".FormatWith(manualDomainName));

				TelnetWrapper.Instance().SetPrimaryDnsServer(dnsServerAddress);

				if (!TelnetWrapper.Instance().GetConfiguredValue(family, "primarydnsserver").Contains(dnsServerAddress, StringComparison.CurrentCultureIgnoreCase))
				{
					TraceFactory.Logger.Info("Primary Dns server address is not set manually.");
					return false;
				}

				TraceFactory.Logger.Info("Dns server address is set to: {0} as expected.".FormatWith(dnsServerAddress));
			}

			return true;
		}

		/// <summary>
		/// Get Dhcpv6 Duid
		/// </summary>
		/// <param name="hostaddress">Host address where packet needs to be captured</param>
		/// <param name="testNo">Test Number</param>
		/// <param name="macAddress">Mac address of printer</param>
		/// <returns>Dhcp Duid</returns>
		private static string GetDhcpv6Uuid(IPConfigurationActivityData activityData, string hostaddress, string printerAddress, int testNo, string macAddress, bool isTPS = false)
		{
			string dhcpv6Duid = string.Empty;
			PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(hostaddress);

			string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

			//Power cycle for TPS, Disable/ Enabling Ipv6 option for VEP/ LFP to get IPv6 packets 
			if (isTPS)
			{
				Printer.Printer printer = PrinterFactory.Create("TPS", printerAddress);
				printer.PowerCycle();
			}
			else
			{
				EwsWrapper.Instance().SetDHCPv6OnStartup(true);
				EwsWrapper.Instance().SetIPv6(false);
				EwsWrapper.Instance().SetIPv6(true);
			}

			Thread.Sleep(TimeSpan.FromMinutes(1));

			string packetLocation = client.Channel.Stop(guid).PacketsLocation;
			string details = client.Channel.GetPacketData(packetLocation, "dhcpv6", "Value:");

			if (!string.IsNullOrEmpty(details) && details.ToLower().Contains(macAddress.ToLower()))
			{
				dhcpv6Duid = Array.Find(details.Split('|'), x => x.EndsWith(macAddress, StringComparison.CurrentCultureIgnoreCase));
				dhcpv6Duid = dhcpv6Duid.Split(':')[1];
			}

			return dhcpv6Duid.Trim();
		}

		/// <summary>
		/// Validate Dhcp Rebind packets
		/// </summary>
		/// <param name="hostName">Address where the PacketCapture service is hosted</param>
		/// <param name="packetLocation">Packet location path</param>
		/// <param name="printerAddress">Printer Address</param>
		/// <param name="macAddress">Printer Mac Address</param>
		/// <returns></returns>
		private static bool ValidateDhcpRebindPacket(string hostName, string packetLocation, string printerAddress, string macAddress)
		{
			TraceFactory.Logger.Info(CtcBaseTests.PACKET_VALIDATION);

			if (!macAddress.Contains(':'))
			{
				macAddress = Regex.Replace(macAddress, ".{2}", "$0:");
				macAddress = macAddress.Remove(macAddress.Length - 1, 1);
			}

			string error = string.Empty;
			string filter = "ip.src == {0} && bootp && eth.dst == ff:ff:ff:ff:ff:ff".FormatWith(printerAddress);

			PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(hostName);

			if (client.Channel.Validate(packetLocation, filter, ref error, "DHCP: Request"))
			{
				TraceFactory.Logger.Info("Dhcp request packet is successfully validated.");
				TraceFactory.Logger.Debug(error);
				
				string data = client.Channel.GetPacketData(packetLocation, "eth.src == {0} && bootp.type == 1".FormatWith(macAddress), "Client MAC address:");

				if (data.Contains(macAddress))
				{
					TraceFactory.Logger.Info("{0}".FormatWith(data));
					return true;
				}
				else
				{
					TraceFactory.Logger.Info(" Failed to find client mac Address: {0}".FormatWith(macAddress));
					TraceFactory.Logger.Debug("Packet data: {0}".FormatWith(data));
					return false;
				}
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate Dhcp rebind packets.");
				return false;
			}
		}

		/// <summary>
		/// Validate Dhcpv6 Rebind packets
		/// </summary>
		/// <param name="hostName">Address where the PacketCapture service is hosted</param>
		/// <param name="packetLocation">Packet location path</param>
		/// <param name="printerAddress">Printer Address</param>
		/// <param name="macAddress">Printer Mac Address</param>
		/// <returns></returns>
		private static bool ValidateDhcpv6RebindPacket(string hostName, string packetLocation, string macAddress)
		{
			TraceFactory.Logger.Info(CtcBaseTests.PACKET_VALIDATION);

			if (!macAddress.Contains(':'))
			{
				macAddress = Regex.Replace(macAddress, ".{2}", "$0:");
				macAddress = macAddress.Remove(macAddress.Length - 1, 1);
			}

			string error = string.Empty;
			string filter = "dhcpv6 && eth.src == {0}".FormatWith(macAddress);

			PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(hostName);

			if (client.Channel.Validate(packetLocation, filter, ref error, "Message type: Rebind"))
			{
				TraceFactory.Logger.Info("Dhcp rebind packet is successfully validated.");
				TraceFactory.Logger.Debug(error);
				return true;
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate Dhcpv6 rebind packets.");
				return false;
			}
		}

		/// <summary>
		/// validate Dhcp Discover Packet
		/// </summary>
		/// <param name="hostName">Address where the PacketCapture service is hosted</param>
		/// <param name="packetLocation">Packet location path</param>
		/// <param name="macAddress">Printer Mac Address</param>
		/// <returns></returns>
		private static bool ValidateDhcpDiscoverPacket(string hostName, string packetLocation, string macAddress)
		{
			TraceFactory.Logger.Info(CtcBaseTests.PACKET_VALIDATION);
			
			if (!macAddress.Contains(':'))
			{
				macAddress = Regex.Replace(macAddress, ".{2}", "$0:");
				macAddress = macAddress.Remove(macAddress.Length - 1, 1);
			}

			string error = string.Empty;
			string filter = "bootp && eth.src == " + macAddress;

			PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(hostName);
			TraceFactory.Logger.Debug("Filter: {0}".FormatWith(filter));

			if (client.Channel.Validate(packetLocation, filter, ref error, "DHCP: Discover"))
			{				
				TraceFactory.Logger.Info("Dhcp discover packet is successfully validated.");
				TraceFactory.Logger.Debug(error);
				return true;
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate Dhcp discover packets.");
				TraceFactory.Logger.Debug(error);
				return false;
			}
		}		

		/// <summary>
		/// Validate Dhcp Packets: Discover, Offer, Request, Ack
		/// </summary>
		/// <param name="hostName">Address where the PacketCapture service is hosted</param>
		/// <param name="packetLocation">Packet location path</param>
		/// <param name="macAddress">Printer Mac Address</param>
        /// <param name="productFamily">Product Family</param>
        /// <param name="includeLog">True to include log message</param>
		/// <returns></returns>
        private static bool ValidateDhcpPackets(string hostName, string packetLocation, string macAddress, string productFamily, bool includeLog = true)
        {
            if (includeLog)
            {
                TraceFactory.Logger.Info(CtcBaseTests.PACKET_VALIDATION);
            }

            if (!macAddress.Contains(':'))
            {
                macAddress = Regex.Replace(macAddress, ".{2}", "$0:");
                macAddress = macAddress.Remove(macAddress.Length - 1, 1);
            }

            string filter = "bootp.hw.mac_addr == " + macAddress;
            string error = string.Empty;

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(hostName);
            TraceFactory.Logger.Debug("Filter: {0}".FormatWith(filter));
            // For Inkjet printers if the printer is already connected to the network there is no "DHCP: Discover" or "DHCP: Offer" packets sent
            if (productFamily == "InkJet")
            {
                if (client.Channel.Validate(packetLocation, filter, ref error, "DHCP: Request", "DHCP: Ack"))
                {
                    TraceFactory.Logger.Info("Dhcp packets are successfully validated.");
                    TraceFactory.Logger.Debug(error);
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to validate Dhcp packets.");
                    TraceFactory.Logger.Debug(error);
                    return false;
                }
            }
            else
            {
                if (client.Channel.Validate(packetLocation, filter, ref error, "DHCP: Discover", "DHCP: Offer", "DHCP: Request", "DHCP: Ack"))
                {
                    TraceFactory.Logger.Info("Dhcp packets are successfully validated.");
                    TraceFactory.Logger.Debug(error);
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to validate Dhcp packets.");
                    TraceFactory.Logger.Debug(error);
                    return false;
                }
            }
        }

		/// <summary>
		/// Validate Dhcp Inform and Ack packets
		/// </summary>
		/// <param name="hostName">Address where the PacketCapture service is hosted</param>
		/// <param name="packetLocation">Packet location path</param>
		/// <param name="macAddress">Printer Mac Address</param>
		/// <returns></returns>
		private static bool ValidateDhcpInformPackets(string hostName, string packetLocation, string macAddress)
		{
			TraceFactory.Logger.Info(CtcBaseTests.PACKET_VALIDATION);
	
			if (!macAddress.Contains(':'))
			{
				macAddress = Regex.Replace(macAddress, ".{2}", "$0:");
				macAddress = macAddress.Remove(macAddress.Length - 1, 1);
			}

			string error = string.Empty;
			string filter = "bootp.hw.mac_addr == " + macAddress;

			PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(hostName);
			TraceFactory.Logger.Debug("Filter: {0}".FormatWith(filter));

			if (client.Channel.Validate(packetLocation, filter, ref error, "DHCP: Inform"))
			{				
				TraceFactory.Logger.Info("Dhcp Inform packet is successfully validated.");
				TraceFactory.Logger.Debug(error);
				return true;
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate Dhcp Inform packets.");
				TraceFactory.Logger.Debug(error);
				return false;
			}			
		}

		/// <summary>
		/// Validate Dhcp Inform and Ack packets
		/// </summary>
		/// <param name="hostName">Address where the PacketCapture service is hosted</param>
		/// <param name="packetLocation">Packet location path</param>
		/// <param name="macAddress">Printer Mac Address</param>
		/// <returns></returns>
		private static bool ValidateDhcpInformAckPackets(string hostName, string packetLocation, string macAddress)
		{
			TraceFactory.Logger.Info(CtcBaseTests.PACKET_VALIDATION);

			if (!macAddress.Contains(':'))
			{
				macAddress = Regex.Replace(macAddress, ".{2}", "$0:");
				macAddress = macAddress.Remove(macAddress.Length - 1, 1);
			}

			string error = string.Empty;
			string filter = "bootp.hw.mac_addr == " + macAddress;

			PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(hostName);
			TraceFactory.Logger.Debug("Filter: {0}".FormatWith(filter));

			if (client.Channel.Validate(packetLocation, filter, ref error, "DHCP: Inform", "DHCP: Ack"))
			{
				TraceFactory.Logger.Info("Dhcp Inform/ Ack packets are successfully validated.");
				TraceFactory.Logger.Debug(error);
				return true;
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate Dhcp Inform / Ack packets.");
				TraceFactory.Logger.Debug(error);
				return false;
			}
		}

		/// <summary>
		/// Validate Boot Request and Reply packets
		/// </summary>
		/// <param name="hostName">Address where the PacketCapture service is hosted</param>
		/// <param name="packetLocation">Packet location path</param>
		/// <param name="macAddress">Printer Mac Address</param>
		/// <returns></returns>
		private static bool ValidateBootPPackets(string hostName, string packetLocation, string macAddress, bool includeLog = true)
		{
			if (includeLog)
			{
				TraceFactory.Logger.Info(CtcBaseTests.PACKET_VALIDATION);
			}

			if (!macAddress.Contains(':'))
			{
				macAddress = Regex.Replace(macAddress, ".{2}", "$0:");
				macAddress = macAddress.Remove(macAddress.Length - 1, 1);
			}

			string error = string.Empty;
			string filter = "bootp.hw.mac_addr == " + macAddress;

			PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(hostName);
			TraceFactory.Logger.Debug("Filter: {0}".FormatWith(filter));

			if (client.Channel.Validate(packetLocation, filter, ref error, "Boot Request", "Boot Reply"))
			{				
				TraceFactory.Logger.Info("Boot request/ reply packets are successfully validated.");
				TraceFactory.Logger.Debug(error);
				return true;
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate boot request/ reply packets.");
				TraceFactory.Logger.Debug(error);
				return false;
			}			
		}

		/// <summary>
		/// Validate Boot Request 
		/// </summary>
		/// <param name="hostName">Address where the PacketCapture service is hosted</param>
		/// <param name="packetLocation">Packet location path</param>
		/// <param name="macAddress">Printer Mac Address</param>
		/// <returns></returns>
		private static bool ValidateBootPRequestPackets(string hostName, string packetLocation, string macAddress)
		{
			TraceFactory.Logger.Info(CtcBaseTests.PACKET_VALIDATION);

			if (!macAddress.Contains(':'))
			{
				macAddress = Regex.Replace(macAddress, ".{2}", "$0:");
				macAddress = macAddress.Remove(macAddress.Length - 1, 1);
			}

			string error = string.Empty;
			string filter = "bootp.hw.mac_addr == " + macAddress;

			PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(hostName);
			TraceFactory.Logger.Debug("Filter: {0}".FormatWith(filter));

			if (client.Channel.Validate(packetLocation, filter, ref error, "Boot Request"))
			{
				TraceFactory.Logger.Info("Boot request packet is successfully validated.");
				TraceFactory.Logger.Debug(error);
				return true;
			}
			else
			{
				TraceFactory.Logger.Info("Failed to validate boot request packet.");
				TraceFactory.Logger.Debug(error);
				return false;
			}
		}

		/// <summary>
		/// Verify the dynamic IP Addresses acquired by the printer.
		/// Discovers the printer using Mac Address and verifies the IPv4 and IPv6 addresses.
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <param name="printer"><see cref="Printer"/></param>
		/// <param name="isNotLfp">true to verify ping validation for IPv6 address, false otherwise</param>
		/// <returns></returns>
		private static bool VerifyDynamicIPv4IPv6Address(IPConfigurationActivityData activityData, ref Printer.Printer printer, bool isNotLfp = true)
		{
			TraceFactory.Logger.Debug("Waiting for 3 minutes.");
			Thread.Sleep(TimeSpan.FromMinutes(3));

			// Discover and get the IP Address of the printer.
			string ipAddress = Utility.GetPrinterIPAddress(activityData.PrimaryMacAddress);

			printer = PrinterFactory.Create(activityData.ProductFamily, ipAddress);

			if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(ipAddress), TimeSpan.FromSeconds(30)))
			{
				TraceFactory.Logger.Info("The printer has acquired the IP Address : {0} from the DHCP Server with dynamic scope.".FormatWith(ipAddress));
			}
			else
			{
				TraceFactory.Logger.Info("The printer failed to acquire an IP Address the DHCP Server with dynamic scope.");
				return false;
			}

			if (isNotLfp)
			{
				IPAddress newDhcpv6Address = printer.IPv6StateFullAddress;

				if (NetworkUtil.PingUntilTimeout(newDhcpv6Address, TimeSpan.FromSeconds(30)))
				{
					TraceFactory.Logger.Info("The printer has acquired the IPv6 Address : {0} from the DHCP Server with dynamic scope.".FormatWith(newDhcpv6Address));
				}
				else
				{
					TraceFactory.Logger.Info("After Power Cycle, the printer failed to acquire an IPv6 Address the DHCP Server with dynamic scope.");
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Verify the Advanced feature status behaviors on power cycle and cold reset.
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <returns></returns>
		private static bool VerifyIPv4OnReset(IPConfigurationActivityData activityData)
		{
			Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
			IPAddress linkLocalAddress = printer.IPv6LinkLocalAddress;

			try
			{
				EwsWrapper.Instance().SetIPv4(false,printer);

				// Create printer instance with link local address to perform power cycle.
				printer = PrinterFactory.Create(activityData.ProductFamily, linkLocalAddress.ToString());

				printer.PowerCycle();

				if (!CheckForPrinterAvailability(linkLocalAddress.ToString(), TimeSpan.FromMinutes(1)))
				{
					return false;
				}

				EwsWrapper.Instance().ChangeDeviceAddress(linkLocalAddress.ToString());

				if (EwsWrapper.Instance().GetIPv4())
				{
					TraceFactory.Logger.Info("IPv4 option is not disabled after power cycle.");
					return false;
				}
				else
				{
					TraceFactory.Logger.Info("IPv4 option is disabled after power cycle as expected.");
				}

				ColdReset(activityData, printer);

				if (!CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address, TimeSpan.FromMinutes(1)))
				{
					return false;
				}

				EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);

				// After cold reset enabling WSDiscovery option
				EwsWrapper.Instance().SetWSDiscovery(true);

				if (EwsWrapper.Instance().GetIPv4())
				{
					TraceFactory.Logger.Info("IPv4 option is enabled after Cold reset as expected.");
					return true;
				}
				else
				{
					TraceFactory.Logger.Info("IPv4 option is not enabled after Cold reset.");
					return false;
				}
			}
			catch
			{
				if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()) && !CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address))
				{
					// In case IPv4 is disabled, doing cold reset to enable IPv4
					printer = PrinterFactory.Create(activityData.ProductFamily, linkLocalAddress.ToString());
					ColdReset(activityData, printer);
					// After cold reset enabling WSDiscovery option
					EwsWrapper.Instance().SetWSDiscovery(true);
					CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address);
				}

				return false;
			}
		}

		/// <summary>
		/// Verify the Advanced feature status behaviors on power cycle and cold reset.
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <returns></returns>
		private static bool VerifyDhcpv6IPv6OnReset(IPConfigurationActivityData activityData)
		{
			//DHCPv6 is applicable only for TPS.
			if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
			{
				if (!EwsWrapper.Instance().SetDHCPv6(false))
				{
					return false;
				}
			}

			if (!EwsWrapper.Instance().SetIPv6(false))
			{
				return false;
			}

			Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);

			if (!printer.PowerCycle())
			{
				return false;
			}

			if (EwsWrapper.Instance().GetIPv6())
			{
				TraceFactory.Logger.Info("Expected	: After Power Cycle, IPv6 option is disabled.");
				TraceFactory.Logger.Info("Actual	: After Power Cycle, IPv6 option is enabled.");
				return false;
			}
			else
			{
				TraceFactory.Logger.Info("After Power Cycle, IPv6 option is disabled as expected.");
			}

			//DHCPv6 is applicable only for TPS.
			if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
			{
				if (EwsWrapper.Instance().GetDHCPv6())
				{
					TraceFactory.Logger.Info("Expected	: After Power Cycle, DHCPv6 option is disabled.");
					TraceFactory.Logger.Info("Actual	: After Power Cycle, DHCPv6 option is enabled.");
					return false;
				}
				else
				{
					TraceFactory.Logger.Info("After Power Cycle, DHCPv6 option is disabled as expected.");
				}
			}

			if (!ColdReset(activityData, printer))
			{
				return false;
			}

			if (!EwsWrapper.Instance().GetIPv6())
			{
				TraceFactory.Logger.Info("Expected	: After Cold Reset, IPv6 option is enabled.");
				TraceFactory.Logger.Info("Actual	: After Cold Reset, IPv6 option is disabled.");
				return false;
			}
			else
			{
				TraceFactory.Logger.Info("After Cold Reset, IPv6 option is enabled as expected.");
			}

			//DHCPv6 is applicable only for TPS.
			if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
			{
				if (EwsWrapper.Instance().GetDHCPv6())
				{
					TraceFactory.Logger.Info("After Cold Reset, DHCPv6 option is enabled as expected.");
					return true;
				}
				else
				{
					TraceFactory.Logger.Info("Expected	: After Cold Reset, DHCPv6 option is enabled.");
					TraceFactory.Logger.Info("Actual	: After Cold Reset, DHCPv6 option is disabled.");
					return false;
				}
			}
			else
			{
				return true;
			}
		}

		#region Pre/ Post Requisites

		/// <summary>
		/// Post Requisite for Hose break across networks.
		/// Connect the printer back to the default DHCP network.
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <param name="networkSwitch"><see cref="INetworkSwitch"/></param>
		/// <returns>True if the post requisites are successful, else false.</returns>
		private static bool PostRequisiteHoseBreakAcrossNetworks(IPConfigurationActivityData activityData)
		{
			TraceFactory.Logger.Info("Performing Post requisite for Hose break across networks.");
			TraceFactory.Logger.Info("Connecting the printer back to default DHCP network.");

			if (!Utility.PerformHoseBreakAcrossNetworks(activityData.SwitchIP, activityData.PrimaryInterfacePortNo, activityData.PrimaryDhcpServerIPAddress))
			{
				TraceFactory.Logger.Info("Failed to connect the printer back to the default network");
				return false;
			}

			if (!Network.NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.PrimaryWiredIPv4Address), TimeSpan.FromSeconds(20)))
			{
				TraceFactory.Logger.Info("Printer has failed to acquire the reserved IP Address : {0}.".FormatWith(activityData.PrimaryWiredIPv4Address));
				return false;
			}

			return true;
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
		private static bool LinuxPostRequisites(IPConfigurationActivityData activityData)
		{
			TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

			IPAddress linuxServerIPAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);

			LinuxUtils.ReplaceOriginalFiles(linuxServerIPAddress);

			LinuxUtils.StopService(linuxServerIPAddress, LinuxServiceType.DHCP);
			LinuxUtils.StopService(linuxServerIPAddress, LinuxServiceType.DHCPV6);
			LinuxUtils.StopService(linuxServerIPAddress, LinuxServiceType.BOOTP);
			LinuxUtils.StopService(linuxServerIPAddress, LinuxServiceType.RARP);
			LinuxUtils.StartService(linuxServerIPAddress, LinuxServiceType.DHCP);

			int windowsVirtualLanId = GetVlanNumber(activityData, activityData.PrimaryDhcpServerIPAddress);
			INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIP));
			networkSwitch.DisablePort(activityData.PrimaryInterfacePortNo);

			if (!networkSwitch.ChangeVirtualLan(activityData.PrimaryInterfacePortNo, windowsVirtualLanId))
			{
				return false;
			}

			networkSwitch.EnablePort(activityData.PrimaryInterfacePortNo);

			// Wait for sometime for configuration to take effect
			Thread.Sleep(TimeSpan.FromMinutes(3));

			// To clear settings acquired from Linux, performing cold reset
			Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);
			printer.ColdReset();

			EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
			SnmpWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);
			TelnetWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);

			// After cold reset enabling WSDiscovery option
			EwsWrapper.Instance().SetWSDiscovery(true);

			EwsWrapper.Instance().ResetConfigPrecedence();
			Thread.Sleep(TimeSpan.FromMinutes(1));

			return EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
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
		private static bool LinuxPreRequisites(IPConfigurationActivityData activityData, LinuxServiceType type, ref IPAddress address, int portNo = 0)
		{
			try
			{
				TraceFactory.Logger.Info(CtcBaseTests.LINUX_PREREQUISITES);
				IPAddress linuxServerAddress = IPAddress.Parse(activityData.LinuxServerIPAddress);

				// Change configuration method based on reservation type
				EwsWrapper.Instance().SetIPConfigMethod(LinuxServiceType.DHCP == type ? IPConfigMethod.DHCP : IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

				LinuxUtils.ReplaceOriginalFiles(linuxServerAddress);
				LinuxUtils.ClearLease(linuxServerAddress);
				NetworkUtil.RenewLocalMachineIP();

				// Stop all services before starting requested service
				LinuxUtils.StopService(linuxServerAddress, LinuxServiceType.DHCP);
				LinuxUtils.StopService(linuxServerAddress, LinuxServiceType.DHCPV6);
				LinuxUtils.StopService(linuxServerAddress, LinuxServiceType.BOOTP);
				LinuxUtils.StopService(linuxServerAddress, LinuxServiceType.RARP);

				// Fetching unused linux address 
				IPAddress clientAddress = NetworkUtil.GetLocalAddress(linuxServerAddress.ToString(), linuxServerAddress.GetSubnetMask().ToString());
				address = NetworkUtil.FetchNextIPAddress(clientAddress.GetSubnetMask(), clientAddress);

				// Creating reservation for the printer address so as to avoid discovery when changed to Linux environment
				LinuxUtils.ReserveIPAddress(linuxServerAddress, address, activityData.PrimaryMacAddress, type == LinuxServiceType.BOOTP ? LinuxServiceType.BOOTP : LinuxServiceType.DHCP);
				LinuxUtils.StartService(linuxServerAddress, type);

				// Move printer connected port from Windows to Linux environment
				// Disabling the port since the printer sends request packets once port is enabled on linux vlan
				int linuxVirtualLanId = GetVlanNumber(activityData, linuxServerAddress.ToString());
				INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIP));

				portNo = portNo == 0 ? activityData.PrimaryInterfacePortNo : portNo;

				networkSwitch.DisablePort(portNo);

				if (!networkSwitch.ChangeVirtualLan(portNo, linuxVirtualLanId))
				{
					return false;
				}

				networkSwitch.EnablePort(portNo);

				// Wait for sometime for configuration to take effect
				Thread.Sleep(TimeSpan.FromMinutes(3));

				// Ping for a max of 5 minutes to check if printer acquired the reserved ip address
				if(!NetworkUtil.PingUntilTimeout(address, 5))
				{
					TraceFactory.Logger.Info("Unable to get printer ip address under linux environment.");
					return false;
				}

				// Change wrappers to point to linux printer ip address
				EwsWrapper.Instance().ChangeDeviceAddress(address);
				SnmpWrapper.Instance().Create(address.ToString());
				TelnetWrapper.Instance().Create(address.ToString());

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
		/// Perform Router Pre/ Post - requisites
		/// </summary>
		/// <param name="activityData"><see cref=" IPConfigurationActivityData"/></param>
		/// <param name="router">Router object</param>
		/// <param name="isPrerequisites">true for pre-requisites log message, false for post-requisites</param>
		private static bool RouterPreRequisites(IPConfigurationActivityData activityData, ref IRouter router, bool isPrerequisites = true)
		{
			string logMessage = isPrerequisites ? CtcBaseTests.ROUTER_PREREQUISITES : CtcBaseTests.ROUTER_POSTREQUISITES;
			bool result = false;

			TraceFactory.Logger.Info(logMessage);

			if (router != null)
			{
				router.SetRouterFlag(activityData.RouterVlanId, RouterFlags.Both);
				Collection<IPAddress> routerAddress = new Collection<IPAddress>(activityData.RouterIPv6Addresses.Select(x => IPAddress.Parse(x)).ToList());
				result = router.EnableIPv6Address(activityData.RouterVlanId, routerAddress);
			}

			if (!isPrerequisites)
			{
				TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

				Collection<PrinterInterfaceConfigurationDetails> printerInterfaceDetails = GetInterfaceDetails(activityData);

				foreach (var item in printerInterfaceDetails)
				{
					TraceFactory.Logger.Info("Setting DHCPv6 as highest config precedence for {0}.".FormatWith(item.PrinterInterface));

					SnmpWrapper.Instance().Create(item.Address);
					SnmpWrapper.Instance().SetConfigPrecedence("3:2:1:0:4");
					Thread.Sleep(TimeSpan.FromMinutes(1));

					EwsWrapper.Instance().ChangeDeviceAddress(item.Address);
					EwsWrapper.Instance().ReinitializeConfigPrecedence();
				}

				SnmpWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);
				EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
			}

			return result;
		}

		/// <summary>
		/// Following operations are performed:
		/// 1. Start DHCP server
		/// 2. Wait for lease renewal to happen
		/// 3. Check whether printer acquired DHCP IPv4 address
		/// 4. Change lease time to default
		/// 5. Get printer to default configuration method
		/// 6. Change EWS, SNMP, Telnet Instance IP Address
		/// </summary>
		/// <param name="activityData"></param>
		private static void AutoIPPostRequisites(IPConfigurationActivityData activityData, Collection<PrinterInterfaceConfigurationDetails> interfaceDetails = null)
        {
            // TPS products will not switch to DHCP when server comes up and configuration method is Auto IP
            if (PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily) || PrinterFamilies.InkJet.ToString().Equals(activityData.ProductFamily))
            {
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address), validate: false);
            }

            // Start DHCP server
            DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            serviceMethod.Channel.StartDhcpServer();

			if (null != interfaceDetails)
			{
				interfaceDetails.ForEach(x =>
				{
				// Create reservation with 'BOTH' type
				// For deleting reserved IP Address, type is optionally. Hence deletion will work
					serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, x.Address, x.MacAddress);
					serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, x.Address, x.MacAddress, ReservationType.Both);
				});
			}
			else
			{
				serviceMethod.Channel.DeleteReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress);
				serviceMethod.Channel.CreateReservation(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, activityData.PrimaryWiredIPv4Address, activityData.PrimaryMacAddress, ReservationType.Both);
			}

            // Wait for lease time to expire
            Thread.Sleep(TimeSpan.FromMinutes(LEASE_WAIT_TIME));

            // Renew local machine IP configuration
            NetworkUtil.RenewLocalMachineIP();

            // TODO: In case printer is not pinging, get back to default             
            NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.PrimaryWiredIPv4Address), TimeSpan.FromMinutes(2));

            // Change lease time on DHCP server to default
            serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, DEFAULT_LEASE_TIME);

			// // Set the default IP type under Advanced tab in Web UI, so that printer goes to Auto IP when DHCP server is not available.
			if (null == interfaceDetails)
			{
				if (!PrinterFamilies.InkJet.ToString().Equals(activityData.ProductFamily))
				{
					EwsWrapper.Instance().SetDefaultIPType(DefaultIPType.AutoIP);
				}

				EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
			}
			else
			{
				foreach (var item in interfaceDetails)
				{
					EwsWrapper.Instance().ChangeDeviceAddress(item.Address);
					if (!PrinterFamilies.InkJet.ToString().Equals(activityData.ProductFamily))
					{
						EwsWrapper.Instance().SetDefaultIPType(DefaultIPType.AutoIP);
					}

					EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(item.Address));
				}
			}

            EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
            SnmpWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);
            TelnetWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);
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
		private static bool TestPreRequisites(IPConfigurationActivityData activityData)
		{
			TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);

			// Server Configuration
			if (!ServerPrerequisites(activityData))
			{
				return false;
			}
			//check whether the Packet capture service is started
			Utility.StartService("PacketCaptureService", activityData.PrimaryDhcpServerIPAddress);

			// Check whether the client has acquired IP Address from both secondary DHCP server and Linux server
			if (!Utility.IsClientConfiguredWithServerIP(activityData.SecondDhcpServerIPAddress, activityData.LinuxServerIPAddress))
			{
				TraceFactory.Logger.Info("Client has not acquired IP Address from second DHCP server: {0} or Linux Server: {1}".
										FormatWith(activityData.SecondDhcpServerIPAddress, activityData.LinuxServerIPAddress));
				return false;
			}

			// Printer pre-requisites
			if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.PrimaryWiredIPv4Address), TimeSpan.FromSeconds(30)))
			{
				if (!GetPrinterOnline(activityData))
				{
					return false;
				}
			}

			bool isPrinterAccessible = false;

			if (PrinterFamilies.TPS.ToString().EqualsIgnoreCase(activityData.ProductFamily))
			{
				isPrinterAccessible = TestPrerequisites_TPS(activityData);
			}
			else if(PrinterFamilies.VEP.ToString().EqualsIgnoreCase(activityData.ProductFamily))
			{
				isPrinterAccessible = TestPrerequisites_VEP(activityData);
			}
            else if (PrinterFamilies.LFP.ToString().EqualsIgnoreCase(activityData.ProductFamily))
			{
				isPrinterAccessible = TestPrerequisites_LFP(activityData);
				EwsWrapper.Instance().WakeUpPrinter();
			}
            else
            {
                TraceFactory.Logger.Info("Ink prerequisites to be added.");
                isPrinterAccessible = true;
            }

			if (!isPrinterAccessible)
			{
				if (!CheckPrinterAccessiblity(activityData))
				{
					return false;
				}
			}

			EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);

			return EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));
		}

		/// <summary>
		/// Pre requisites for the primary and secondary server
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <returns>True if the server pre-requisites are done.</returns>
		private static bool ServerPrerequisites(IPConfigurationActivityData activityData)
		{
			TraceFactory.Logger.Info("Performing Pre-Requisites for Primary DHCP server: {0}".FormatWith(activityData.PrimaryDhcpServerIPAddress));

			DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

			/* TODO: Check if service running
			if (!serviceMethod.Channel.IsServiceRunning())
			{
				TraceFactory.Logger.Info("IP Configuration Service is not running on primary DHCP server: {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));
				return false;
			}
			*/

			if (!serviceMethod.Channel.IsDhcpServiceRunning())
			{
				TraceFactory.Logger.Debug("DHCP Service is not running on DHCP server: {0}.".FormatWith(activityData.PrimaryDhcpServerIPAddress));

				if (!serviceMethod.Channel.StartDhcpServer())
				{
					return false;
				}

				NetworkUtil.RenewLocalMachineIP();

				// Wait for 6 minutes after starting the DHCP server.
				Thread.Sleep(LEASE_WAIT_TIME);
			}

			long leaseTime = serviceMethod.Channel.GetLeasetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress);

			if (DEFAULT_LEASE_TIME != leaseTime)
			{
				if (!serviceMethod.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPAddress, DEFAULT_LEASE_TIME))
				{
					TraceFactory.Logger.Info("Unable to set Lease time on DHCP scope: {0}".FormatWith(activityData.DHCPScopeIPAddress));
					return false;
				}
			}

			if (!string.IsNullOrEmpty(activityData.DHCPScopeIPv6Address))
			{
				int lease = serviceMethod.Channel.GetValidLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address);

				if (DEFAULT_VALID_LEASE_TIME != lease)
				{
					if (!serviceMethod.Channel.SetValidLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, DEFAULT_VALID_LEASE_TIME))
					{
						TraceFactory.Logger.Info("Unable to set Valid Lifetime on DHCP scope: {0}".FormatWith(activityData.DHCPScopeIPv6Address));
						return false;
					}
				}

				lease = serviceMethod.Channel.GetPreferredLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address);

				if (DEFAULT_PREFERRED_LEASE_TIME != lease)
				{
					if (!serviceMethod.Channel.SetPreferredLifetime(activityData.PrimaryDhcpServerIPAddress, activityData.DHCPScopeIPv6Address, DEFAULT_PREFERRED_LEASE_TIME))
					{
						TraceFactory.Logger.Info("Unable to set Preferred Lifetime on DHCP scope: {0}".FormatWith(activityData.DHCPScopeIPv6Address));
						return false;
					}
				}
			}
			else
			{
				TraceFactory.Logger.Info("Unable to find IPv6 scope.");
				return false;
			}

			TraceFactory.Logger.Debug("All good with primary dhcp server.");
			TraceFactory.Logger.Info("Performing Pre-Requisites for Second DHCP server: {0}".FormatWith(activityData.SecondDhcpServerIPAddress));

			// Secondary DHCP Server
			serviceMethod = DhcpApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress);

			/* TODO: Check if service is running
			if (!serviceMethod.Channel.IsServiceRunning())
			{
				TraceFactory.Logger.Info("IP Configuration Service is not running on DHCP server: {0}.".FormatWith(activityData.SecondDhcpServerIPAddress));
				return false;
			}
			*/

			if (!serviceMethod.Channel.IsDhcpServiceRunning())
			{
				TraceFactory.Logger.Info("DHCP Service is not running on DHCP server: {0}.".FormatWith(activityData.SecondDhcpServerIPAddress));

				if (!serviceMethod.Channel.StartDhcpServer())
				{
					return false;
				}

				NetworkUtil.RenewLocalMachineIP();

				// Wait for 6 minutes after starting the DHCP server.
				Thread.Sleep(LEASE_WAIT_TIME);
			}

			serviceMethod = DhcpApplicationServiceClient.Create(activityData.SecondDhcpServerIPAddress);

			string secondServerScopeIP = serviceMethod.Channel.GetDhcpScopeIP(activityData.SecondDhcpServerIPAddress);

			leaseTime = serviceMethod.Channel.GetLeasetime(activityData.SecondDhcpServerIPAddress, secondServerScopeIP);

			if (DEFAULT_LEASE_TIME != leaseTime)
			{
				if (!serviceMethod.Channel.SetDhcpLeaseTime(activityData.SecondDhcpServerIPAddress, secondServerScopeIP, DEFAULT_LEASE_TIME))
				{
					TraceFactory.Logger.Info("Unable to set Lease time on DHCP scope: {0}".FormatWith(secondServerScopeIP));
					return false;
				}
			}

			TraceFactory.Logger.Debug("All good with secondary dhcp server.");
			return true;
		}
		
		/// <summary>
		/// Prerequisites for wireless Tests.
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <returns>true if the prerequisites are properly configured, else false.</returns>
		private static bool WirelessPreRequisites(IPConfigurationActivityData activityData)
		{
			if (string.IsNullOrEmpty(activityData.WirelessIPv4Address))
			{
				TraceFactory.Logger.Info("WirelessInterface IPv4 address is not available.");
				return false;
			}

			if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.WirelessIPv4Address), TimeSpan.FromSeconds(30)))
			{
				TraceFactory.Logger.Info("Ping failed with the wireless IP Address : {0}.".FormatWith(activityData.WirelessIPv4Address));
				return false;
			}

			// Create printer instance for the wireless IPv4 address
			Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WirelessIPv4Address);

			// Get the wireless Mac address
			string wirelessMacAddress = printer.MacAddress;

			// Create reservation in DHCP Server
			string dhcpServerIPAddress = Printer.Printer.GetDHCPServerIP(IPAddress.Parse(activityData.WirelessIPv4Address)).ToString();

			DhcpApplicationServiceClient serviceMethod = DhcpApplicationServiceClient.Create(dhcpServerIPAddress);
			string scopeIP = serviceMethod.Channel.GetDhcpScopeIP(dhcpServerIPAddress);

			// delete the reservation if it is already existing. Not validated since the reservation may or may not be present.
			serviceMethod.Channel.DeleteReservation(dhcpServerIPAddress, scopeIP, activityData.WirelessIPv4Address, wirelessMacAddress);

			return serviceMethod.Channel.CreateReservation(dhcpServerIPAddress, scopeIP, activityData.WirelessIPv4Address, wirelessMacAddress, ReservationType.Both);

			// TODO : Check the Model Number of the printer to validate if Wired and wireless instances are same. 
		}

		/// <summary>
		/// Prerequisites for VEP
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <returns>True if EWS, Telnet and SNMP are accessible.</returns>
		private static bool TestPrerequisites_VEP(IPConfigurationActivityData activityData)
		{
			Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);

			IPAddress printerIpAddress = IPAddress.Parse(activityData.PrimaryWiredIPv4Address);

			// Case 1: Either EWS or telnet is not accessible and SNMP is accessible
			if (!printer.IsEwsAccessible(printerIpAddress) || !printer.IsTelnetAccessible(printerIpAddress))
			{
				if (!printer.IsSnmpAccessible(IPAddress.Parse(activityData.PrimaryWiredIPv4Address)))
				{
					return false;
				}

				ColdReset(activityData, printer);

				// After cold reset enabling WSDiscovery option
				EwsWrapper.Instance().SetWSDiscovery(true);

				// Check if EWS, Telnet and SNMP are accessible.
				return (printer.IsEwsAccessible(printerIpAddress) && printer.IsTelnetAccessible(printerIpAddress) && printer.IsSnmpAccessible(printerIpAddress));
			}
			else
			{
				return printer.IsSnmpAccessible(printerIpAddress);
			}
		}

		/// <summary>
		/// Prerequisites for TPS
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <returns>True if EWS, Telnet and SNMP are accessible.</returns>
		private static bool TestPrerequisites_TPS(IPConfigurationActivityData activityData)
		{
			Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);

			IPAddress printerIpAddress = IPAddress.Parse(activityData.PrimaryWiredIPv4Address);

			// Case 1: EWS is accessible and telnet is not accessible, Enable telnet from EWS and then check Telnet and SNMP
			if (printer.IsEwsAccessible(printerIpAddress))
			{
				//Restoring the default values,since the TPS printer is not taking the default hostname(NPI...) even after deleting the server hostname
				EwsWrapper.Instance().ResetConfigPrecedence();

				if (!printer.IsTelnetAccessible(printerIpAddress))
				{
					EwsWrapper.Instance().SetTelnet();
					return (printer.IsTelnetAccessible(printerIpAddress) && printer.IsSnmpAccessible(printerIpAddress));
				}
				else
				{
					return printer.IsSnmpAccessible(printerIpAddress);
				}
			}
			else
			{
				// Case 2: EWS is not accessible, SNMP is accessible perform Cold reset
				if (printer.IsSnmpAccessible(printerIpAddress))
				{
					ColdReset(activityData, printer);

					// After cold reset enabling WSDiscovery option
					EwsWrapper.Instance().SetWSDiscovery(true);
				}

				// Check if EWS, Telnet and SNMP are accessible.
				return (printer.IsEwsAccessible(printerIpAddress) && printer.IsTelnetAccessible(printerIpAddress) && printer.IsSnmpAccessible(printerIpAddress));
			}
		}

		/// <summary>
		/// Prerequisites for LFP
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <returns>True if EWS, Telnet, SSH and SNMP are accessible.</returns>
		private static bool TestPrerequisites_LFP(IPConfigurationActivityData activityData)
		{
			Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);

			IPAddress printerIpAddress = IPAddress.Parse(activityData.PrimaryWiredIPv4Address);

			// Case 1: Either EWS or telnet or SNMP is not accessible and SSH is accessible
			if (!printer.IsEwsAccessible(printerIpAddress) || !printer.IsTelnetAccessible(printerIpAddress) || !printer.IsSnmpAccessible(printerIpAddress))
			{
				if (!printer.IsSSHAccessible(printerIpAddress, SSH_USERNAME, SSH_PASSWORD))
				{
					return false;
				}

				ColdReset(activityData, printer);

				// After cold reset enabling WSDiscovery option
				EwsWrapper.Instance().SetWSDiscovery(true);

				// Check if EWS, Telnet and SNMP are accessible.
				return (printer.IsEwsAccessible(printerIpAddress) && printer.IsTelnetAccessible(printerIpAddress) && 
						printer.IsSnmpAccessible(printerIpAddress) && printer.IsSSHAccessible(printerIpAddress, SSH_USERNAME, SSH_PASSWORD));
			}
			else
			{
				return printer.IsSSHAccessible(printerIpAddress, SSH_USERNAME, SSH_PASSWORD);
			}
		}

		/// <summary>
		/// Post Requisites for Link speed tests
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <param name="switchLinkSpeed"><see cref="LinkSpeed"/></param>
		/// <param name="printerLinkSpeed"><see cref="PrinterLinkSpeed"/></param>
		private static void PostRequisitesLinkSpeed(IPConfigurationActivityData activityData)
		{
			TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

			// Change the port link speed
			INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIP));
			networkSwitch.SetLinkSpeed(activityData.PrimaryInterfacePortNo, LinkSpeed.Auto);
			Thread.Sleep(TimeSpan.FromMinutes(1));

			if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.PrimaryWiredIPv4Address), TimeSpan.FromSeconds(30)))
			{
				if (PrinterFamilies.TPS.ToString().Equals(activityData.ProductFamily))
				{
					SnmpWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Auto);
				}
				else
				{
					EwsWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Auto);
				}
			}
		}

		/// <summary>
		/// Post requisites for LAA.
		/// Cold Reset the printer and get back the previous LAA.
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <param name="currentDeviceAddress">Current device address.</param>
		private static void PostRequisitesLaa(IPConfigurationActivityData activityData, string currentDeviceAddress)
		{
			TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

			// Cold reset the printer to set the LAA to default value.
			Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, currentDeviceAddress);
			printer.ColdReset();

			CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address);

			EwsWrapper.Instance().ChangeDeviceAddress(activityData.PrimaryWiredIPv4Address);
			TelnetWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);
			SnmpWrapper.Instance().Create(activityData.PrimaryWiredIPv4Address);

			// After cold reset enabling WSDiscovery option
			EwsWrapper.Instance().SetWSDiscovery(true);
		}

		/// <summary>
		/// Pre-requisites to be performed when the printer is not available.
		/// </summary>
		/// <param name="activityData"><see cref="IPConfigurationActivityData"/></param>
		/// <returns>True if the printer is available after performing the pre-requisites, else false.</returns>
		private static bool GetPrinterOnline(IPConfigurationActivityData activityData)
		{
			bool continueTest = false;

			// Get the switch IP which is pinging and then create the INetworkSwitch instance, for the networks can be down at any time.
			string switchIPAddress = activityData.VirtualLanDetails.Where(item => null != item.Value && NetworkUtil.PingUntilTimeout(IPAddress.Parse(item.Value), TimeSpan.FromSeconds(10))).FirstOrDefault().Value;

			INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(switchIPAddress));

			if (!ConnectDefaultVlan(activityData, networkSwitch))
			{
				return false;
			}

			// Enable the port
			networkSwitch.EnablePort(activityData.PrimaryInterfacePortNo);

			string currentDeviceAddress = string.Empty;
			string hostName = string.Empty;

			// Discover maximum of 3 times to get the current device address
			for (int i = 0; i < 3; i++)
			{
				BacaBodWrapper.DiscoverDevice(activityData.PrimaryMacAddress, BacaBodSourceType.MacAddress, BacaBodDiscoveryType.All, ref currentDeviceAddress, ref hostName);

				if (string.IsNullOrEmpty(currentDeviceAddress))
				{
					Thread.Sleep(TimeSpan.FromMinutes(2));
					TraceFactory.Logger.Debug("No printer was discovered with the Mac Address: {0}. Discovering again..".FormatWith(activityData.PrimaryMacAddress));
					continue;
				}
				else
				{
					break;
				}
			}

			// If the printer is not discovered, fail the test and return
			if (string.IsNullOrEmpty(currentDeviceAddress))
			{
				TraceFactory.Logger.Info("No printer was discovered with the Mac Address: {0}.".FormatWith(activityData.PrimaryMacAddress));

				while (string.IsNullOrEmpty(currentDeviceAddress))
				{
					continueTest = ShowErrorPopup();

					if (!continueTest)
					{
						return false;
					}

					BacaBodWrapper.DiscoverDevice(activityData.PrimaryMacAddress, BacaBodSourceType.MacAddress, BacaBodDiscoveryType.All, ref currentDeviceAddress, ref hostName);
				}
			}

			DefaultIPType ipType = IPAddress.Parse(currentDeviceAddress).IsAutoIP() ? DefaultIPType.AutoIP : (IPAddress.Parse(currentDeviceAddress).Equals(Printer.Printer.LegacyIPAddress) ? DefaultIPType.LegacyIP : DefaultIPType.None);

			DhcpApplicationServiceClient serviceMethods;

			if (ipType == DefaultIPType.AutoIP)
			{
				// When the device is in Auto IP, bring the client also in Auto IP. Accessing the EWS with host name is not working always.
				serviceMethods = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

				serviceMethods.Channel.StopDhcpServer();

				NetworkUtil.RenewLocalMachineIP();

				EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);
			}

			if (ipType == DefaultIPType.LegacyIP)
			{
				// When the device is in Leagcy IP, EWS can be accessible only using host name.
				EwsWrapper.Instance().ChangeHostName(hostName);
				EwsWrapper.Instance().SetDefaultIPType(DefaultIPType.AutoIP);
			}

			if (ipType == DefaultIPType.AutoIP || ipType == DefaultIPType.LegacyIP)
			{
				if (!EwsWrapper.Instance().SetSendDHCPRequestOnAutoIP(true))
				{
					return false;
				}

				EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, validate: false, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

				// Start the DHCP server and wait for 6 minutes
				serviceMethods = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

				serviceMethods.Channel.StartDhcpServer();

				Thread.Sleep(TimeSpan.FromMinutes(LEASE_WAIT_TIME));

				// Discover and find the current device address
				currentDeviceAddress = Utility.GetPrinterIPAddress(activityData.PrimaryMacAddress);
			}

			if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(currentDeviceAddress), TimeSpan.FromMinutes(1)))
			{
				TraceFactory.Logger.Info("Current IP address of the printer is: {0}.".FormatWith(currentDeviceAddress));
			}
			else
			{
				TraceFactory.Logger.Info("Printer: {0} is not available.".FormatWith(currentDeviceAddress));
				return false;
			}

			// Printer may acquire a different IP address, so changing the IP config method to default IP config method so that the device acquires the reserved ip address.
			EwsWrapper.Instance().ChangeDeviceAddress(currentDeviceAddress);

			EwsWrapper.Instance().SetDefaultIPConfigMethod(false, expectedIPAddress: IPAddress.Parse(activityData.PrimaryWiredIPv4Address));

			bool isPrinterAvailable = CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address, TimeSpan.FromMinutes(1));

			// Ping and check if the printer is available.
			while (!isPrinterAvailable)
			{
				continueTest = ShowErrorPopup();

				if (!continueTest)
				{
					return false;
				}

				isPrinterAvailable = CheckForPrinterAvailability(activityData.PrimaryWiredIPv4Address, TimeSpan.FromMinutes(1));
			}

			return isPrinterAvailable;
		}

		/// <summary>
		/// Checks whether EWS, Telnet and SNMP are accessible for the printer.
		/// If not accessible, a pop up will be displayed asking the user to correct the printer.
		/// </summary>
		/// <param name="activityData"></param>
		/// <returns></returns>
		private static bool CheckPrinterAccessiblity(IPConfigurationActivityData activityData)
		{
			Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.PrimaryWiredIPv4Address);

			IPAddress printerIpAddress = IPAddress.Parse(activityData.PrimaryWiredIPv4Address);

			// Check if EWS, Telnet and SNMP are accessible.
			bool isPrinterAccessible = (printer.IsEwsAccessible(printerIpAddress) && printer.IsTelnetAccessible(printerIpAddress) && printer.IsSnmpAccessible(printerIpAddress));

			bool continueTest = false;

			// Check if EWS, Telnet and SNMP are accessible.
			while (!isPrinterAccessible)
			{
				continueTest = ShowErrorPopup();

				if (!continueTest)
				{
					return false;
				}

				isPrinterAccessible = (printer.IsEwsAccessible(printerIpAddress) && printer.IsTelnetAccessible(printerIpAddress) && printer.IsSnmpAccessible(printerIpAddress));
			}

			return isPrinterAccessible;
		}

		/// <summary>
		/// Displays the error popup.
		/// </summary>
		/// <returns>True if Retry is clicked, else false.</returns>
		private static bool ShowErrorPopup()
		{
			DialogResult result = MessageBox.Show("Printer is currently not accessible. \nPlease cold reset the printer and click Retry to continue or Cancel to ignore.", "Printer Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);

			if (result == DialogResult.Retry)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Returns the client network name provided by a particular server.
		/// </summary>
		/// <param name="serverIpAddress">IP Address of the server</param>
		/// <returns>The client network name.</returns>
		private static string GetClientNetworkName(string serverIpAddress)
		{
			TraceFactory.Logger.Info("Fetching Client Network Name");
			foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
			{
				foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
				{
					// Get the NIC having IP address in the server IP range so that the NIC can be disabled.
					if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && ip.Address.IsInSameSubnet(IPAddress.Parse(serverIpAddress), IPAddress.Parse(serverIpAddress).GetSubnetMask()))
					{
						TraceFactory.Logger.Info(item.Name);
						return item.Name;
					}
				}
			}
			return string.Empty;
		}

		#endregion

		#endregion

	}
}
