using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Discovery;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.NetworkSwitch;
using HP.ScalableTest.PluginSupport.Connectivity.PacketCapture;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.RadiusServer;
using HP.ScalableTest.PluginSupport.Connectivity.SystemConfiguration;
using HP.ScalableTest.Utility;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.DotOneX
{
    /// <summary>
    /// Contains the templates for 802.1X test cases
    /// </summary>
    internal static class DotOneXTemplates
    {
        #region Local Variables

        // TODO: Change the certificate path after design change
        private const string CA_CERTIFICATEPATH = @"ConnectivityShare\Certificates\802.1x_Certificates\{0}\{1}\CA_certificate.cer";
        private const string ID_CERTIFICATEPATH = @"ConnectivityShare\Certificates\802.1x_Certificates\{0}\{1}\ID_certificate.pfx";
        private const string INVALID_CA_CERTIFICATEPATH = @"ConnectivityShare\Certificates\802.1x_Certificates\InvalidCertificates\CA_certificate.cer";
        private const string INVALID_ID_CERTIFICATEPATH = @"ConnectivityShare\Certificates\802.1x_Certificates\InvalidCertificates\ID_certificate.pfx";
        private const string DYNAMIC_ID_CERTIFICATEPATH = @"\\{0}\Dynamic Certificates";
        private const string SERVER_DYNAMIC_ID_CERTIFICATEPATH = @"C:\\Dynamic Certificates";
        private const string SERVER_CA_CERTIFICATEPATH = @"C:\Certificates\CA_certificate.cer";
        private const string ID_CERTIFICATE_PASSWORD = "xyzzy";
        private static TimeSpan CAPTURE_TIMEOUT_SUCCESS = new TimeSpan(0, 1, 0);
        private static TimeSpan CAPTURE_TIMEOUT_FAILURE = new TimeSpan(0, 2, 30);
        private static string CERTIFICATE_TEMPLATE_NAME = "Copy of Web Server";

        private const string PRESHAREDKEY = "AutomationPSK";
        private const string IPSECTEMPLATENAME = "IPSecTemplate-{0}";
        private const string CLIENT_RULE_NAME = "ClientRule-{0}";

        #endregion

        #region Templates

        #region Dot1x Templates
        /// <summary>
        /// Validates 802.1x authentication for the specified <see cref="AuthenticationMode"/> for Low, Medium, High Encryption Strengths
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="authenticationMode"><see cref="AuthenticationMode"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool AuthenticationWithEncryption(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = true
                };

                TraceFactory.Logger.Info("Step I: {0} authentication with encryption strength: {1}".FormatWith(authenticationMode, configDetails.EncryptionStrength));

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}-{3}".FormatWith(testId, activityData.RadiusServerType, authenticationMode, configDetails.EncryptionStrength), cleanUp: false))
                {
                    return false;
                }

                configDetails.EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.High : EncryptionStrengths.Medium;

                TraceFactory.Logger.Info("Step II: {0} authentication with encryption strength: {1}".FormatWith(authenticationMode, configDetails.EncryptionStrength));

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}-{3}".FormatWith(testId, activityData.RadiusServerType, authenticationMode, configDetails.EncryptionStrength), authenticationOnPort: false, installCertificates: false, cleanUp: false))
                {

                    return false;
                }

                if (!EwsWrapper.Instance().IsOmniOpus)
                {
                    configDetails.EncryptionStrength = EncryptionStrengths.High;

                    TraceFactory.Logger.Info("Step III: {0} authentication with encryption strength: {1}".FormatWith(authenticationMode, configDetails.EncryptionStrength));

                    return Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}-{3}".FormatWith(testId, activityData.RadiusServerType, authenticationMode, configDetails.EncryptionStrength), authenticationOnPort: false, installCertificates: false, cleanUp: false);
                }

                return true;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        /// <summary>
        /// Validates the re-authenticate on apply option with invalid certificates
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="authenticationMode"><see cref="AuthenticationMode"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool ValidateReAuthentication(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = false
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}-ValidCertificates".FormatWith(testId, authenticationMode, activityData.RadiusServerType)))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: {0} authentication with invalid ID certificate.".FormatWith(authenticationMode));

                filter = authenticationMode == AuthenticationMode.EAPTLS ? "ssl.record.content_type == 21 && !radius" : "eap.code == 3 && !radius";
                tokenInPackets = authenticationMode == AuthenticationMode.EAPTLS ? "Access Denied" : "Success";

                configDetails.ReAuthenticate = true;

                return Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}-InValidIdCertificate".FormatWith(testId, authenticationMode, activityData.RadiusServerType), validCaCertificate: true, validIdCertificate: false, cleanUp: false);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        /// <summary>
        /// Validates the authentication with LAA.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="authenticationMode"><see cref="AuthenticationMode"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool AuthenticationWithLaa(ref DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            string laaValue = CtcUtility.GetLaa();
            bool isLaaSet = false;

            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address)))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EncryptionStrengths.Medium,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = false
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}-beforeLAAChange".FormatWith(testId, activityData.RadiusServerType, authenticationMode), cleanUp: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: {0} authentication after LAA change.".FormatWith(authenticationMode));

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-{1}-{2}-afterLAAChange".FormatWith(testId, activityData.RadiusServerType, authenticationMode));

                    if (!EwsWrapper.Instance().SetLAA(laaValue, IPAddress.Parse(activityData.Ipv4Address)))
                    {
                        return false;
                    }

                    isLaaSet = true;

                    // For VEP, cold reset is not performed, so updating the new laa value to the mac address in activitydata.
                    if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.VEP.ToString()))
                    {
                        activityData.MacAddress = laaValue;
                    }

                    Thread.Sleep(CAPTURE_TIMEOUT_SUCCESS);

                    PacketDetails packetDetails = client.Channel.Stop(guid);

                    if (!Basic802Dot1XValidation(activityData, authenticationMode))
                    {
                        return false;
                    }

                    if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                    {
                        return false;
                    }

                    return PrintValidation(activityData, testId);
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                if (isLaaSet)
                {
                    Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.Ipv4Address);
                    printer.ColdReset();
                }

                PostRequisites802Dot1XAuthentication(activityData);
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address));
            }
        }

        /// <summary>
        /// Validates the authentication Hostname change.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="authenticationMode"><see cref="AuthenticationMode"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool AuthenticationWithHostname(ref DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address)))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EncryptionStrengths.Medium,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = false
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}-beforeHostNameChange".FormatWith(testId, activityData.RadiusServerType, authenticationMode), cleanUp: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: Changing hostname after {0} authentication.".FormatWith(authenticationMode));

                string hostName = CtcUtility.GetUniqueHostName();

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-{1}-{2}-PostHostNameChange".FormatWith(testId, activityData.RadiusServerType, authenticationMode));

                    if (!EwsWrapper.Instance().SetHostname(hostName))
                    {
                        return false;
                    }

                    if (hostName == EwsWrapper.Instance().GetHostname())
                    {
                        TraceFactory.Logger.Info("Successfully changed hostname after basic authentication");
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to change the hostname after basic authentication");
                        return false;
                    }


                    Thread.Sleep(CAPTURE_TIMEOUT_FAILURE);

                    PacketDetails packetDetails = client.Channel.Stop(guid);

                    TraceFactory.Logger.Info("Expected: Negotiation does not happen after host name change.");
                    return !Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets);
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address));
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        /// <summary>
        /// Validates the authentication with IP Configuration method change.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="authenticationMode"><see cref="AuthenticationMode"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool AuthenticationWithIpConfigMethodChange(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address)))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = false
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}-Manual".FormatWith(testId, activityData.RadiusServerType, authenticationMode), cleanUp: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: {0} authentication after IP configuration method change.".FormatWith(authenticationMode));

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-{1}-{2}-DHCP".FormatWith(testId, activityData.RadiusServerType, authenticationMode));

                    if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address)))
                    {
                        return false;
                    }

                    Thread.Sleep(CAPTURE_TIMEOUT_FAILURE);

                    PacketDetails packetDetails = client.Channel.Stop(guid);

                    if (Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                    {
                        return false;
                    }

                    return PrintValidation(activityData, testId);
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address));
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        /// <summary>
        /// Validates the authentication with hose break.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="authenticationMode"><see cref="AuthenticationMode"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool AuthenticationAfterHosebreak(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address)))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = false
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}-BeforeHoseBreak".FormatWith(testId, activityData.RadiusServerType, authenticationMode), cleanUp: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: {0} authentication hose break.".FormatWith(authenticationMode));

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-{1}-{2}-AfterHoseBreak".FormatWith(testId, activityData.RadiusServerType, authenticationMode));

                    if (!CtcUtility.PerformHoseBreak(activityData.Ipv4Address, activityData.SwitchIp, activityData.AuthenticatorPort, 30))
                    {
                        return false;
                    }

                    Thread.Sleep(CAPTURE_TIMEOUT_SUCCESS);

                    PacketDetails packetDetails = client.Channel.Stop(guid);

                    if (!Basic802Dot1XValidation(activityData, authenticationMode))
                    {
                        return false;
                    }

                    if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                    {
                        return false;
                    }

                    return PrintValidation(activityData, testId);
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address));
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        /// <summary>
        /// Validates the authentication with Link Speed.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="authenticationMode"><see cref="AuthenticationMode"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool AuthenticationWithLinkSpeed(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = true
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, authenticationMode), authenticationMode, cleanUp: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: {0} authentication after link speed change.".FormatWith(authenticationMode));

                PacketDetails packetDetails = null;

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-{1}-{2}-AfterLinkSpeedChange".FormatWith(testId, activityData.RadiusServerType, authenticationMode));

                    try
                    {
                        INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));

                        if (!networkSwitch.SetLinkSpeed(activityData.AuthenticatorPort, LinkSpeed.Auto))
                        {
                            return false;
                        }

                        // All the ink products will not support setting link speed through SNMP.
                        if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString()))
                        {
                            if (!EwsWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Full10T))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!SnmpWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Full100Tx))
                            {                                
                                if (!SnmpWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Full100Tx))
                                {
                                    return false;
                                }
                            }
                        }

                        // TPS will restart after setting the link speed.
                        if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.TPS.ToString()))
                        {
                            Thread.Sleep(TimeSpan.FromMinutes(3));

                            if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), TimeSpan.FromSeconds(30)))
                            {
                                TraceFactory.Logger.Info("Ping successful. Printer is in ready state after power cycle.");
                            }
                            else
                            {
                                TraceFactory.Logger.Info("Ping failed. Printer is not in ready state after power cycle.");
                                return false;
                            }
                        }

                        Thread.Sleep(CAPTURE_TIMEOUT_SUCCESS);
                    }
                    finally
                    {
                        packetDetails = client.Channel.Stop(guid);
                    }

                    if (!Basic802Dot1XValidation(activityData, authenticationMode))
                    {
                        return false;
                    }

                    if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                    {
                        return false;
                    }

                    return PrintValidation(activityData, testId);
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData);

                if (!activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString()))
                {
                    SnmpWrapper.Instance().SetLinkSpeed(PrinterLinkSpeed.Auto);
                }

                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                networkSwitch.SetLinkSpeed(activityData.AuthenticatorPort, LinkSpeed.Auto);
            }
        }

        /// <summary>
        /// Validates the authentication for different authentication modes.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="authenticationMode"><see cref="AuthenticationMode"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool ValidateAuthentication(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = false
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";
                string packetFileName = "{0}-{1}-{2}-OnPrinterandServer".FormatWith(testId, activityData.RadiusServerType, authenticationMode);

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, packetFileName))
                {
                    return false;
                }
                
                // Only one authentication can be selected at a time in ink
                if (!PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                {
                    configDetails.AuthenticationProtocol = AuthenticationMode.EAPTLS | AuthenticationMode.PEAP;
                    packetFileName = "{0}-{1}-{2}-Priority".FormatWith(testId, activityData.RadiusServerType, authenticationMode);

                    TraceFactory.Logger.Info("Step II: {0} authentication with {1} priority.".FormatWith(configDetails.AuthenticationProtocol, authenticationMode));

                    if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, packetFileName, AuthenticationMode.PEAP | AuthenticationMode.EAPTLS, authenticationMode))
                    {
                        return false;
                    }

                    filter = "eap.code == 4";
                    tokenInPackets = "Failure";
                    AuthenticationMode serverAuthenticationMode = (authenticationMode == AuthenticationMode.EAPTLS) ? AuthenticationMode.PEAP : AuthenticationMode.EAPTLS;
                    packetFileName = "{0}-{1}-{2}OnPrinter-{3}OnServer".FormatWith(testId, activityData.RadiusServerType, authenticationMode, serverAuthenticationMode);
                    configDetails.AuthenticationProtocol = authenticationMode;

                    TraceFactory.Logger.Info("Step III: Authentication with {0} on Printer {1} on server.".FormatWith(configDetails.AuthenticationProtocol, serverAuthenticationMode));
                    return Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, packetFileName, serverAuthenticationMode, cleanUp: false);
                }
                else
                {
                    return true;
                }
			}
			finally
			{
				TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
				PostRequisites802Dot1XAuthentication(activityData);
			}
		}

        /// <summary>
        /// Validates the authentication with expired certificates on the printer.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="authenticationMode"><see cref="AuthenticationMode"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool ExpiredCertificatesOnPrinter(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = true
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";
                string packetFileName = "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, authenticationMode);

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, packetFileName, authenticationMode, cleanUp: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: Authentication with expired certificates on printer.");

                PacketDetails packetDetails = null;

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-{1}-{2}-ExpiredCertificateOnPrinter".FormatWith(testId, activityData.RadiusServerType, authenticationMode));

                    try
                    {
                        string certificatePath = (authenticationMode == AuthenticationMode.PEAP) ? Path.Combine(CtcSettings.ConnectivityShare, CA_CERTIFICATEPATH.FormatWith(activityData.ProductFamily, activityData.RadiusServerType))
                                                    : Path.Combine(CtcSettings.ConnectivityShare, ID_CERTIFICATEPATH.FormatWith(activityData.ProductFamily, activityData.RadiusServerType));

                        // Get the expiry date of the certificate, add 1 day and set the new date on the printer so that the certificate is expired according to printer date.
                        CertificateDetails certificateDetails = CertificateUtility.GetCertificateDetails(certificatePath, (authenticationMode == AuthenticationMode.PEAP) ? null : ID_CERTIFICATE_PASSWORD);

                        DateTime expiryDate = certificateDetails.ExpiryDate;
                        DateTime newDate = expiryDate.AddDays(1);

                        TraceFactory.Logger.Info("Set the printer time 1 day more than the certificate expiry date.");

                        if (!EwsWrapper.Instance().SetDateTime(newDate))
                        {
                            return false;
                        }

                        INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                        networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);
                        networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);

						Thread.Sleep(CAPTURE_TIMEOUT_FAILURE);
					}
					finally
					{
						packetDetails = client.Channel.Stop(guid);
					}

                    //Inkjets looks for Failure packet here.
                    if (PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                    {
                        filter = "eapol";
                        tokenInPackets = "Failure";
                    }
                    else
                    {
                        filter = "eap.type == 3 && eapol";
                        tokenInPackets = "Legacy Nak";
                    }

                    if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                    {
                        return false;
                    }

                    return !PrintValidation(activityData, testId, true);
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData);
                EwsWrapper.Instance().SetDateTime(DateTime.Now);
            }
        }

        /// <summary>
        /// Validates the authentication with expired certificates on the server.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="authenticationMode"><see cref="AuthenticationMode"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool ExpiredCertificatesOnServer(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = true
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";
                string packetFileName = "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, authenticationMode);

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, packetFileName, authenticationMode, cleanUp: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: Authentication with expired certificates on server.");

                PacketDetails packetDetails = null;

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-{1}-{2}-ExpiredCertificateOnServer".FormatWith(testId, activityData.RadiusServerType, authenticationMode));

                    try
                    {
                        string certificatePath = Path.Combine(CtcSettings.ConnectivityShare, CA_CERTIFICATEPATH.FormatWith(activityData.ProductFamily, activityData.RadiusServerType));

                        // Get the expiry date of the certificate, add 1 year and set the new date on the server so that the certificate is expired according to server date.
                        CertificateDetails certificateDetails = CertificateUtility.GetCertificateDetails(certificatePath);

                        DateTime expiryDate = certificateDetails.ExpiryDate;
                        DateTime newDate = expiryDate.AddYears(1);

                        TraceFactory.Logger.Info("Setting the server time 1 year more than the certificate expiry date: {0}.".FormatWith(expiryDate));

                        using (SystemConfigurationClient radiusClient = SystemConfigurationClient.Create(activityData.RadiusServerIp))
                        {
                            if (radiusClient.Channel.SetSystemTime(newDate))
                            {
                                TraceFactory.Logger.Info("Successfully set the server time to: {0}.".FormatWith(newDate));
                            }
                            else
                            {
                                TraceFactory.Logger.Info("Failed to set the server time to: {0}.".FormatWith(newDate));
                                return false;
                            }
                        }

                        INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                        networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);
                        networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);

                        Thread.Sleep(CAPTURE_TIMEOUT_FAILURE);
                    }
                    finally
                    {
                        packetDetails = client.Channel.Stop(guid);
                    }
                }

				if (authenticationMode == AuthenticationMode.EAPTLS)
				{
                    //Inkjet looks for failure packet.
                    if (PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                    {
                        filter = "eapol";
                        tokenInPackets = "Failure";
                    }
                    else
                    {
                        filter = "eap.type == 3 && eapol";
                        tokenInPackets = "Legacy Nak";
                    }

                    if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                    {
                        return false;
                    }

                    return !PrintValidation(activityData, testId, true);
                }

                if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                {
                    return false;
                }

                return PrintValidation(activityData, testId);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                TraceFactory.Logger.Info("Resetting the server time to: {0}.".FormatWith(DateTime.Now));

                using (SystemConfigurationClient radiusClient = SystemConfigurationClient.Create(activityData.RadiusServerIp))
                {
                    DateTime currentTime = DateTime.Now;

                    if (radiusClient.Channel.SetSystemTime(currentTime))
                    {
                        TraceFactory.Logger.Info("Successfully set the server time to: {0}.".FormatWith(currentTime));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to set the server time to: {0}.".FormatWith(currentTime));
                    }
                }

                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        /// <summary>
        /// Validates the authentication with failsafe enabled.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool AuthenticationWithFailSafeEnabled(DotOneXActivityData activityData, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = AuthenticationMode.EAPTLS,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = true
                };

                TraceFactory.Logger.Info("Step I: {0} Authentication with failsafe option enabled.".FormatWith(configDetails.AuthenticationProtocol));

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";
                string packetFileName = "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, configDetails.AuthenticationProtocol);

				if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, packetFileName, configDetails.AuthenticationProtocol, cleanUp: false))
				{
					return false;
				}
                // Need to disable the port, since Authenticatio method will be changed in the next step. If port is no disabled, Printer loses connectivity
                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);

                configDetails.AuthenticationProtocol = AuthenticationMode.PEAP;
                packetFileName = "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, configDetails.AuthenticationProtocol);

                TraceFactory.Logger.Info("Step II: {0} Authentication with failsafe option enabled.".FormatWith(configDetails.AuthenticationProtocol));

				if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, packetFileName, configDetails.AuthenticationProtocol, installCertificates: false, cleanUp: false))
				{
					return false;
				}
               
                // Need to disable the port, since Authenticatio method will be changed in the next step. If port is no disabled, Printer loses connectivity
                networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);
				
			    if (PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                {
                    return true;
                }
                configDetails.AuthenticationProtocol = AuthenticationMode.PEAP | AuthenticationMode.EAPTLS;
                packetFileName = "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, configDetails.AuthenticationProtocol);

                TraceFactory.Logger.Info("Step III: {0} Authentication with failsafe option enabled.".FormatWith(configDetails.AuthenticationProtocol));


                return Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, packetFileName, configDetails.AuthenticationProtocol, AuthenticationMode.EAPTLS, installCertificates: false, cleanUp: false);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        /// <summary>
        /// Validates the authentication with failsafe disabled.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool AuthenticationWithFailSafeDisabled(DotOneXActivityData activityData, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = AuthenticationMode.EAPTLS,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Block,
                    ReAuthenticate = true
                };

                TraceFactory.Logger.Info("Step I: {0} Authentication with failsafe option disabled.".FormatWith(configDetails.AuthenticationProtocol));

                if (!InstallCertificate(activityData.RadiusServerType, activityData.ProductFamily))
                {
                    return false;
                }

                if (!ValidateBlockNetwork(activityData, configDetails, "{0}-{1}".FormatWith(testId, configDetails.AuthenticationProtocol)))
                {
                    return false;
                }

                         

                TraceFactory.Logger.Info("Step II: {0} Authentication with failsafe option disabled.".FormatWith(configDetails.AuthenticationProtocol));
                configDetails.AuthenticationProtocol = AuthenticationMode.PEAP;

                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);
                if (!EwsWrapper.Instance().Reset802Dot1XAuthentication())
                {
                    return false;
                }           

                if (!ValidateBlockNetwork(activityData, configDetails, "{0}-{1}".FormatWith(testId, configDetails.AuthenticationProtocol)))
                {
                    return false;
                }

               TraceFactory.Logger.Info("Step III: {0} Authentication with failsafe option disabled.".FormatWith(configDetails.AuthenticationProtocol));
               configDetails.AuthenticationProtocol = AuthenticationMode.PEAP | AuthenticationMode.EAPTLS;

                networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);
                if (!EwsWrapper.Instance().Reset802Dot1XAuthentication())
                {
                    return false;
                }                
                
                string filter         = "((eapol) && (eap.code == 3)) && !(radius)";
				string tokenInPackets = "Success";
				string packetFileName = "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, configDetails.AuthenticationProtocol);
                //Need to disable the port before 3rd step, else priter connectivity is lost                
                networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);

                return Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, packetFileName, configDetails.AuthenticationProtocol, AuthenticationMode.EAPTLS, installCertificates: false, cleanUp: false);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        /// <summary>
        /// Validates the authentication after power cycle.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool AuthenticationWithInvalidCAPowerCycle(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address)))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = true
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";
                string packetFileName = "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, authenticationMode);

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, packetFileName, cleanUp: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: Authentication after Power Cycle with invalid CA Certificate");

                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);

                EwsWrapper.Instance().UnInstallAllCertificates();

                PacketDetails packetDetails = null;

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-{1}-{2}-AfterPowerCycle".FormatWith(testId, activityData.RadiusServerType, authenticationMode));

                    try
                    {
                        if (!InstallCertificate(activityData.RadiusServerType, activityData.ProductFamily, validCaCertificate: false))
                        {
                            return false;
                        }

                        Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.Ipv4Address);
                        printer.PowerCycleAsync();

                        networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);

						// Waiting for 5 minutes to get the printer back in ready state. Can not verify ping since the printer loses connectivity
						Thread.Sleep(TimeSpan.FromMinutes(5));
					}
					finally
					{
						packetDetails = client.Channel.Stop(guid);
					}
				}
                //Inkjets looks for Failure packet here.
                if (PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                {
                    filter = "eapol";
                    tokenInPackets = "Failure";
                }
                else
                {
                    filter = "eap.type == 3 && !radius";
                    tokenInPackets = "Legacy Nak";
                }

                if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                {
                    return false;
                }

                return !PrintValidation(activityData, testId, true);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData, true);
                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address));
            }
        }

        /// <summary>
        /// Validates the authentication with different certificate store.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool AuthenticationWithDifferentCertificateStore(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                bool IsInk = PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily);

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = true
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";
                string packetFileName = "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, authenticationMode);

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, packetFileName, authenticationMode, cleanUp: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: {0} authentication with invalid ID certificate.".FormatWith(authenticationMode));

                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);

                PacketDetails packetDetails = null;

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-{1}-{2}-InvalidIDCertificate".FormatWith(testId, activityData.RadiusServerType, authenticationMode));

                    try
                    {
                        if (!EwsWrapper.Instance().InstallIDCertificate(Path.Combine(CtcSettings.ConnectivityShare, INVALID_ID_CERTIFICATEPATH), ID_CERTIFICATE_PASSWORD))
                        {
                            return false;
                        }

                        networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);

                        Thread.Sleep((authenticationMode == AuthenticationMode.PEAP) ? CAPTURE_TIMEOUT_SUCCESS : CAPTURE_TIMEOUT_FAILURE);
                    }
                    finally
                    {
                        packetDetails = client.Channel.Stop(guid);
                    }
                }

                // For PEAP authentication is successful when invalid id certificate is installed.
                if (authenticationMode == AuthenticationMode.PEAP)
                {
                    if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                    {
                        return false;
                    }

                    if (!PrintValidation(activityData, testId))
                    {
                        return false;
                    }
                }
                else
                {
                    //Inkjet devices send failure packet here.
                    if (IsInk)
                    {
                        filter = "eapol";
                        tokenInPackets = "Failure";
                    }
                    else
                    {
                        filter = "eap.type == 3 && eapol";
                        tokenInPackets = "Legacy Nak";
                    }

                    if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                    {
                        return false;
                    }

                    if (!IsInk)
                    {
                        filter = "ssl.alert_message.level == 2 && !radius";
                        tokenInPackets = "TLSv1 Record Layer: Alert (Level: Fatal, Description: Access Denied)";
                        if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                        {
                            return false;
                        }
                    }
                    if (PrintValidation(activityData, testId, true))
                    {
                        return false;
                    }
                }

                networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);
                //MessageBox.Show("Disabled port.. check ping");
                networkSwitch.DisablePort(activityData.AuthenticatorPort);
                networkSwitch.EnablePort(activityData.AuthenticatorPort);

                if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), TimeSpan.FromMinutes(2)))
                {
                    TraceFactory.Logger.Info("Connection to the printer is lost.");
                    return false;
                }

                TraceFactory.Logger.Info("Step III: {0} authentication with invalid CA certificate.".FormatWith(authenticationMode));

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-{1}-{2}-InvalidCACertificate".FormatWith(testId, activityData.RadiusServerType, authenticationMode));

                    try
                    {
                        if (!EwsWrapper.Instance().UnInstallCACertificate(Path.Combine(CtcSettings.ConnectivityShare, CA_CERTIFICATEPATH.FormatWith(activityData.ProductFamily, activityData.RadiusServerType))))
                        {
                            return false;
                        }

                        if (!EwsWrapper.Instance().InstallCACertificate(Path.Combine(CtcSettings.ConnectivityShare, INVALID_CA_CERTIFICATEPATH)))
                        {
                            return false;
                        }

                        networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);

                        Thread.Sleep(CAPTURE_TIMEOUT_FAILURE);
                    }
                    finally
                    {
                        packetDetails = client.Channel.Stop(guid);
                    }
                }

                if (IsInk)
                {
                    filter = "eapol";
                    tokenInPackets = "Failure";
                }
                else
                {
                    filter = "eapol && eap.type == 3";
                    tokenInPackets = "Legacy Nak";
                }

                if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                {
                    return false;
                }

                return !PrintValidation(activityData, testId, true);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        /// <summary>
        /// Validates the authentication with different certificate store.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool AuthenticationWithDifferentServerCertificates(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            if (activityData.RadiusServerType != RadiusServerTypes.SubSha2)
            {
                TraceFactory.Logger.Info("The test is applicable only for Sub SHA2 radius server.");
                TraceFactory.Logger.Debug("{0} is the radius server under execution".FormatWith(activityData.RadiusServerType));
                return false;
            }

            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = true
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";
                string packetFileName = "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, authenticationMode);

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, packetFileName, authenticationMode, cleanUp: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: Configuring Switch with Root SHA2 server");

                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));

                PacketDetails packetDetails = null;

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-RootSHA2-{1}".FormatWith(testId, authenticationMode));

                    try
                    {
                        if (!networkSwitch.DeConfigureAllRadiusServer())
                        {
                            return false;
                        }

                        if (!networkSwitch.ConfigureRadiusServer(IPAddress.Parse(activityData.RootSha2ServerIp), activityData.SharedSecret))
                        {
                            return false;
                        }

                        networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);
                        networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);

                        Thread.Sleep(CAPTURE_TIMEOUT_FAILURE);
                    }
                    finally
                    {
                        packetDetails = client.Channel.Stop(guid);
                    }
                }

                filter = "eap.code == 4 && !radius";
                tokenInPackets = "Failure";

                if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                {
                    return false;
                }

                if (PrintValidation(activityData, testId, true))
                {
                    return false;
                }

                networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);

                if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), TimeSpan.FromMinutes(2)))
                {
                    TraceFactory.Logger.Info("Connection to the printer is lost.");
                    return false;
                }

                TraceFactory.Logger.Info("Step III: Root SHA2 ID and CA Certificates on the printer");

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-RootSHA2-ID-CA-Certificates-{1}".FormatWith(testId, authenticationMode));

                    try
                    {
                        if (!networkSwitch.DeConfigureAllRadiusServer())
                        {
                            return false;
                        }

                        if (!networkSwitch.ConfigureRadiusServer(IPAddress.Parse(activityData.RadiusServerIp), activityData.SharedSecret))
                        {
                            return false;
                        }

                        if (!InstallCertificate(RadiusServerTypes.RootSha2, activityData.ProductFamily, false, false))
                        {
                            return false;
                        }

                        if (!EwsWrapper.Instance().Set802Dot1XAuthentication(configDetails))
                        {
                            return false;
                        }

                        networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);

                        Thread.Sleep(CAPTURE_TIMEOUT_FAILURE);
                    }
                    finally
                    {
                        packetDetails = client.Channel.Stop(guid);
                    }
                }

                filter = "eapol && eap.type == 3";
                tokenInPackets = "Legacy Nak";

                if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                {
                    return false;
                }

                if (PrintValidation(activityData, testId, true))
                {
                    return false;
                }

                networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);

                if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), TimeSpan.FromMinutes(2)))
                {
                    TraceFactory.Logger.Info("Connection to the printer is lost.");
                    return false;
                }

                TraceFactory.Logger.Info("Step IV: Root SHA2 ID and Sub SHA2 CA certificates on printer.");

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-RootSHA2_ID-SubSHA2_CA-Certificates-{1}".FormatWith(testId, authenticationMode));

                    try
                    {
                        string connectivityShare = CtcSettings.ConnectivityShare;

                        if (!EwsWrapper.Instance().InstallCACertificate(Path.Combine(connectivityShare, CA_CERTIFICATEPATH.FormatWith(activityData.ProductFamily, RadiusServerTypes.SubSha2)), true))
                        {
                            return false;
                        }

                        if (!EwsWrapper.Instance().InstallIDCertificate(Path.Combine(connectivityShare, ID_CERTIFICATEPATH.FormatWith(activityData.ProductFamily, RadiusServerTypes.RootSha2)), ID_CERTIFICATE_PASSWORD))
                        {
                            return false;
                        }

                        if (!EwsWrapper.Instance().Set802Dot1XAuthentication(configDetails))
                        {
                            return false;
                        }

                        networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);

                        Thread.Sleep((authenticationMode == AuthenticationMode.PEAP) ? CAPTURE_TIMEOUT_SUCCESS : CAPTURE_TIMEOUT_FAILURE);
                    }
                    finally
                    {
                        packetDetails = client.Channel.Stop(guid);
                    }
                }

                if (authenticationMode == AuthenticationMode.PEAP)
                {
                    filter = "eap.code == 3 && !radius";
                    tokenInPackets = "Success";

                    if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                    {
                        return false;
                    }

                    return PrintValidation(activityData, testId);
                }
                else
                {
                    filter = "ssl.alert_message.level == 2 && !radius";
                    tokenInPackets = "Fatal";

                    if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                    {
                        return false;
                    }

                    return !PrintValidation(activityData, testId, true);
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                networkSwitch.DeConfigureAllRadiusServer();
                networkSwitch.ConfigureRadiusServer(IPAddress.Parse(activityData.RadiusServerIp), activityData.SharedSecret);

                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        /// <summary>
        /// Validates the authentication with id certificate not associated with the radius server user.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool AuthenticationWithUnAssociatedIdCertificate(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                EwsWrapper.Instance().UnInstallAllCertificates();

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Step I: {0} authentication with Id certificate not associated with the radius user.".FormatWith(authenticationMode));

                if (!ConfigureRadiusServer(activityData, authenticationMode))
                {
                    return false;
                }

                string connectivityShare = CtcSettings.ConnectivityShare;

                if (!EwsWrapper.Instance().InstallCACertificate(Path.Combine(connectivityShare, CA_CERTIFICATEPATH.FormatWith(activityData.ProductFamily, activityData.RadiusServerType)), activityData.RadiusServerType == RadiusServerTypes.SubSha2))
                {
                    return false;
                }

                string idCertificatePath = string.Empty;

                if (!GenerateIdCertificate(activityData, out idCertificatePath))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().InstallCertificate(idCertificatePath))
                {
                    return false;
                }

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = true
                };

                if (!EwsWrapper.Instance().Set802Dot1XAuthentication(configDetails))
                {
                    return false;
                }

                string packetFileName = "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, authenticationMode);

                PacketDetails packetDetails = null;

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-UnAssociatedIdCertificate-{1}".FormatWith(testId, authenticationMode));

                    try
                    {
                        INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                        networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);

						Thread.Sleep(authenticationMode == AuthenticationMode.EAPTLS ? CAPTURE_TIMEOUT_FAILURE : CAPTURE_TIMEOUT_SUCCESS);
					}
					finally
					{
						packetDetails = client.Channel.Stop(guid);
					}
				}

                string filter = string.Empty;
                string tokenInPackets = string.Empty;

                if (activityData.ProductFamily.EqualsIgnoreCase(PrinterFamilies.InkJet.ToString()))                
                {
                    filter = (authenticationMode == AuthenticationMode.EAPTLS) ? "eapol" : "eap.code == 3 && !radius";
                    tokenInPackets = (authenticationMode == AuthenticationMode.EAPTLS) ? "Failure" : "Success";
                }
                else
                {
                    filter = (authenticationMode == AuthenticationMode.EAPTLS) ? "eap.type == 3 && !radius" : "eap.code == 3 && !radius";
                    tokenInPackets = (authenticationMode == AuthenticationMode.EAPTLS) ? "Legacy Nak" : "Success";
                }

                if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                {
                    return false;
                }

                if (authenticationMode == AuthenticationMode.PEAP)
                {
                    return PrintValidation(activityData, testId);
                }
                else
                {
                    return !PrintValidation(activityData, testId, true);
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        /// <summary>
        /// Validates the authentication with id certificate associated with the radius server user.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool AuthenticationWithAssociatedIdCertificate(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId, bool isTPM = false)
        {
            if (!TestPrerequisites(activityData))
            {
                return false;
            }

            TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

            TraceFactory.Logger.Info("Step I: {0} authentication with RADIUS server certificate signed by certificate service on same machine.".FormatWith(authenticationMode));

            if (isTPM)
            {
                if (!AuthenticationWithIdCertificate(activityData, authenticationMode, testId))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: {0} authentication with RADIUS server certificate signed by certificate service on same machine with export key.".FormatWith(authenticationMode));

                return AuthenticationWithIdCertificate(activityData, authenticationMode, testId, true);
            }
            else
            {
                return AuthenticationWithIdCertificate(activityData, authenticationMode, testId);
            }
        }

        /// <summary>
        /// Validates the authentication with id certificate not trusted by radius server user.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool AuthenticationWithUnTrustedCertificate(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";
                string packetFileName = "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, authenticationMode);

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = true
                };

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, packetFileName, cleanUp: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: {0} authentication with certificate not trusted.".FormatWith(authenticationMode));

                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);

                using (RadiusApplicationServiceClient client = RadiusApplicationServiceClient.Create(activityData.RadiusServerIp))
                {
                    if (client.Channel.DeleteCACertificate(SERVER_CA_CERTIFICATEPATH))
                    {
                        TraceFactory.Logger.Info("Successfully deleted the certificate: {0} from trusted root.".FormatWith(SERVER_CA_CERTIFICATEPATH));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to delete the certificate: {0} from trusted root.".FormatWith(SERVER_CA_CERTIFICATEPATH));
                        return false;
                    }


                    // Deleting CA certificates twice as there is a chance of having two certificates in root store.
                    client.Channel.DeleteCACertificate(SERVER_CA_CERTIFICATEPATH);
                    TraceFactory.Logger.Debug("CA certificate is deleted.");
                }

                packetFileName = "{0}-{1}-{2}-NotTrustedCertificate".FormatWith(testId, activityData.RadiusServerType, authenticationMode);

                PacketDetails packetDetails = null;

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, packetFileName);

                    try
                    {
                        Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.Ipv4Address);
                        printer.PowerCycleAsync();

                        networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);

                        Thread.Sleep((authenticationMode == AuthenticationMode.EAPTLS) ? CAPTURE_TIMEOUT_FAILURE : CAPTURE_TIMEOUT_SUCCESS);
                    }
                    finally
                    {
                        packetDetails = client.Channel.Stop(guid);
                    }
                }

                if (authenticationMode == AuthenticationMode.EAPTLS)
                {
                    filter = "ssl.record.content_type == 21 && !radius";
                    tokenInPackets = "Access Denied";

					if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
					{
						return false;
					}

                    //Inkjets looks for Failure packet here.
                    if (PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                    {
                        filter = "eapol";
                        tokenInPackets = "Failure";
                    }
                    else
                    {
                        filter = "eap.type == 3 && eapol";
                        tokenInPackets = "Legacy Nak";
                    }

                    if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                    {
                        return false;
                    }

                    return !PrintValidation(activityData, testId, true);
                }

                if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                {
                    return false;
                }

				return PrintValidation(activityData, testId);
			}
			finally
			{
				TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                //Prniter is not accessible since CA cert is removed so disabling the Authentication Port to accessprinter back 
                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);

                using (RadiusApplicationServiceClient client = RadiusApplicationServiceClient.Create(activityData.RadiusServerIp))
                {
                    if (client.Channel.AddCACertificate(SERVER_CA_CERTIFICATEPATH))
                    {
                        TraceFactory.Logger.Info("Successfully added the certificate: {0} to trusted root.".FormatWith(SERVER_CA_CERTIFICATEPATH));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to add the certificate: {0} to trusted root.".FormatWith(SERVER_CA_CERTIFICATEPATH));
                    }
                }
                //MessageBox.Show("Add CA certificate on radius server: {0} and click on OK".FormatWith(activityData.RadiusServerIp), "Certificate Addition", MessageBoxButtons.OK);

                PostRequisites802Dot1XAuthentication(activityData);
            }
        }
        #endregion

        #region IPSecurity on Dot1x
        /// <summary>
        /// Test ID: 718501
        /// 1. Generate a certificate signing request and get the certificate from a CA.
        /// 2.  Install the certificate on the printer.	
        /// 3.  Validate the Authentication with Radius Server.
        /// 4.  Create IPsec rule with All IP Address, All Services and Certificate with Low security strength.
        /// 5.  Create similar rule in Client.
        /// 6.  Check the connectivity of the Printer.
        /// 7.  Validate the HTTPS operation on the Printer.
        /// 8.  Discover the Printer from WJA.
        /// 9.  Repeat step1 to 8 for interface2
        /// 10. Generate a certificate signing request on TPM (When creating a CSR unchecked the option "Mark   
        ///       Private Key as exportable") and get the certificate signed from a CA
        /// 11. Install the certificate on the printer.	
        /// 12.	Validate the Authentication with Radius Server.
        /// 13.	Create IPsec rule with All IP Address, All Services and Certificate with Low security strength.
        /// 14.	Create similar rule in Client.
        /// 15.	Check the connectivity of the Printer.
        /// 16.	Validate the HTTPS operation on the Printer.
        /// 17.	Discovery the Printer from WJA.
        /// 18.	Repeat step9 to 16 for interface2
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool VerifyIPSecBehaviourWithDotOnexConfigurations_Certificates(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId, bool isTPM = false)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPrerequisites(activityData))
            {
                return false;
            }

            int authenticatorPort = activityData.AuthenticatorPort;
            string printerIPAddress = activityData.Ipv4Address;
            //string europaIPAddress = activityData.EuropaWiredIP;
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step1: Validating Dot1x, IPsecurity, HTTPS, Discovery with Interface1"));
                if (!ValidateIPsecBehaviourWithDotOnexConfiguration(activityData, authenticationMode, testId))
                {
                    return false;
                }

                IPAddress secondaryAddress = null;
                if (activityData.ProductFamily == PrinterFamilies.VEP.ToString() && IPAddress.TryParse(activityData.EuropaWiredIP, out secondaryAddress))
                {
                    TraceFactory.Logger.Info(CtcUtility.WriteStep("Step1: Validating Dot1x, IPsecurity, HTTPS, Discovery with Interface2"));
                    activityData.Ipv4Address = activityData.EuropaWiredIP;
                    activityData.AuthenticatorPort = activityData.EuropaPort;
                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.EuropaWiredIP);
                    if (!ValidateIPsecBehaviourWithDotOnexConfiguration(activityData, authenticationMode, testId))
                    {
                        return false;
                    }
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step2: Validating Dot1x, IPsecurity, HTTPS, Discovery using private key with Interface1"));
                activityData.Ipv4Address = printerIPAddress;
                activityData.AuthenticatorPort = authenticatorPort;
                EwsWrapper.Instance().ChangeDeviceAddress(printerIPAddress);
                if (!ValidateIPsecBehaviourWithDotOnexConfiguration(activityData, authenticationMode, testId, true))
                {
                    return false;
                }

                if (activityData.ProductFamily == PrinterFamilies.VEP.ToString() && IPAddress.TryParse(activityData.EuropaWiredIP, out secondaryAddress))
                {
                    TraceFactory.Logger.Info(CtcUtility.WriteStep("Step2: Validating Dot1x, IPsecurity, HTTPS, Discovery using private key with Interface2"));
                    activityData.Ipv4Address = activityData.EuropaWiredIP;
                    activityData.AuthenticatorPort = activityData.EuropaPort;
                    EwsWrapper.Instance().ChangeDeviceAddress(activityData.EuropaWiredIP);
                    return ValidateIPsecBehaviourWithDotOnexConfiguration(activityData, authenticationMode, testId, true);
                }

                return true;
            }
            finally
            {
                activityData.Ipv4Address = printerIPAddress;
                activityData.AuthenticatorPort = authenticatorPort;
                EwsWrapper.Instance().ChangeDeviceAddress(printerIPAddress);
                PostRequisites802Dot1XAuthentication(activityData, deleteDynamicIdCertificate: true);
            }
        }

        /// <summary>
        /// TestID: 729262
        /// 1.	Configure Radius server with PEAP-MS-CHAP2 authentication mode.	
        /// 2. Install the Certificates on the device and configure the device using PEAP along with user credentials.
        /// 3. Connect the Device in the Authenticated port.
        /// 4. Should authenticate successfully.
        /// 5. Create an IP Sec rules using All IP Address template, All Services template and IP Sec template using 
        ///    pre-shared Key.
        /// 6. Ensure that Default IP Sec policy is "Drop" and enable IP Sec policy on Device.
        /// 7. Create similar rule on the DOT1.x Server.
        /// 8. Verify the Peap authentication.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool VerifyIPSecBehaviourWithDotOnexConfigurations_Peap(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPrerequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = false
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";
                string packetFileName = "{0}-{1}-{2}-OnPrinterandServer".FormatWith(testId, activityData.RadiusServerType, authenticationMode);

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, packetFileName))
                {
                    return false;
                }

                configDetails.AuthenticationProtocol = AuthenticationMode.EAPTLS | AuthenticationMode.PEAP;
                packetFileName = "{0}-{1}-{2}-Priority".FormatWith(testId, activityData.RadiusServerType, authenticationMode);

                TraceFactory.Logger.Info("Step II: {0} authentication with {1} priority.".FormatWith(configDetails.AuthenticationProtocol, authenticationMode));

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, packetFileName, AuthenticationMode.PEAP | AuthenticationMode.EAPTLS, authenticationMode))
                {
                    return false;
                }

                filter = "eap.code == 4";
                tokenInPackets = "Failure";
                AuthenticationMode serverAuthenticationMode = (authenticationMode == AuthenticationMode.EAPTLS) ? AuthenticationMode.PEAP : AuthenticationMode.EAPTLS;
                packetFileName = "{0}-{1}-{2}OnPrinter-{3}OnServer".FormatWith(testId, activityData.RadiusServerType, authenticationMode, serverAuthenticationMode);
                configDetails.AuthenticationProtocol = authenticationMode;

                TraceFactory.Logger.Info("Step III: Authentication with {0} on Printer {1} on server.".FormatWith(configDetails.AuthenticationProtocol, serverAuthenticationMode));

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, packetFileName, serverAuthenticationMode, cleanUp: false))
                {
                    TraceFactory.Logger.Info("Dot1x Authentication Failed");
                    return false;
                }

                // Since the last step has validation on negative scenario, bringing back the printer
                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);

                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), TimeSpan.FromMinutes(6)))
                {
                    TraceFactory.Logger.Info("Printer: {0} is available.".FormatWith(activityData.Ipv4Address));
                }
                else
                {
                    TraceFactory.Logger.Info("Connection to the printer is lost.");
                    return false;
                }

                TraceFactory.Logger.Info("Step IV: IPSecurity Validation");

                return ValidateIPsecBehaviour(activityData, testId);
            }
            finally
            {
                CtcUtility.DeleteAllIPsecRules();
                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        #endregion

        #region Print on Dot1x

        /// <summary>
        /// Validates 802.1x authentication for the specified <see cref="AuthenticationMode"/> for Low, Medium, High Encryption Strengths
        /// -	Enable the 802.1x authentication.
        /// -	Install the printer (all Protocols)
        /// -	Send few print jobs
        /// -	Printing should be successful.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="authenticationMode"><see cref="AuthenticationMode"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool VerifyPrintWithDotoneX(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId, bool isIPV6 = false)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily.ToString()), IPAddress.Parse(activityData.Ipv4Address));
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

				BitArray resultArray = new BitArray(8, true);
				bool result        = true;
				string address     = null;
				string outHostName = string.Empty;

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = true
                };

                TraceFactory.Logger.Info("Enabling Dot1x Authentication");
                TraceFactory.Logger.Info("Authentication with encryption strength: {1}".FormatWith(authenticationMode, configDetails.EncryptionStrength));

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}-{3}".FormatWith(testId, activityData.RadiusServerType, authenticationMode, configDetails.EncryptionStrength), cleanUp: false))
                {
                    return false;
                }

				if(isIPV6)
				{
                    EwsWrapper.Instance().SetDHCPv6OnStartup(true);
                    EwsWrapper.Instance().SetIPv6(false);
                    EwsWrapper.Instance().SetIPv6(true);
                    Thread.Sleep(TimeSpan.FromMinutes(1));
					address = printer.IPv6StateFullAddress.ToString();
				}
				else
				{
					address = activityData.Ipv4Address;
				}
				// Validating Print for all protocols
				TraceFactory.Logger.Info("Installing driver with P9100 and validating print");
				resultArray[0] = CtcUtility.InstallandPrint(IPAddress.Parse(address), Printer.Printer.PrintProtocol.RAW, activityData.ProductFamily, activityData.PrintDriver, activityData.PrintDriverModel, 9100);
				TraceFactory.Logger.Info("Installing driver with LPD and validating print");
				resultArray[1] = CtcUtility.InstallandPrint(IPAddress.Parse(address), Printer.Printer.PrintProtocol.LPD, activityData.ProductFamily, activityData.PrintDriver, activityData.PrintDriverModel);                								

                foreach (bool item in resultArray)
                {
                    result &= item;
                }

				TraceFactory.Logger.Info("****************** Validation Summary for Print Protocols *****************");
				TraceFactory.Logger.Info("---------------------------------------------------------------------------");
				TraceFactory.Logger.Info("Services   * P9100  *   LPD  * ");
				TraceFactory.Logger.Info("---------------------------------------------------------------------------");
				TraceFactory.Logger.Info("Expected   *  " + string.Join("  *  ", "True") + "  * " + string.Join("  *  ", "True") + "  * ");
				TraceFactory.Logger.Info("---------------------------------------------------------------------------");
				TraceFactory.Logger.Info("Actual     *  " + string.Join("  *  ", resultArray[0]) + "  * " + string.Join("  *  ", resultArray[1]) + " * " );
				TraceFactory.Logger.Info("---------------------------------------------------------------------------");

                return result;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        #endregion

        #region FIPS on Dot1x

        /// <summary>
        /// Verify 802.1x authentication in FIPS mode[576736]
        ///1.	Configure radius server with EAP-TLS Authentication mode, install SHA1/SHA2 certificate on the device, enable FIPS option and submit print job.
        ///2.	The Print job should be successful.
        ///3.	Disable FIPS and submit Print job.
        ///4.	The Print job should be successful.
        ///5.	Configure radius server with EAP-TLS Authentication mode, enable only SSL3.0 on the radius server, install SHA1/SHA2 certificate on the device.
        ///6.	Disable SSL3.0 on the device and enable TLS 1.0,1.1,1.2
        ///7.	Try to authenticate the server.
        ///8.	Authentication should fail.
        ///9.	Configure radius server with PEAP-MS-CHAP2 Authentication mode, enable only SSL3.0 on the radius server, install SHA1/SHA2 certificate on the device and perform the following operation
        ///     (i)	Disable SSL3.0 on the device and enable TLS 1.0,1.1,1.2
        ///     (ii)	 Try to Authenticate the server
        ///     (iii)	Authentication should fail
        ///     (iv)	Enable FIPS on the device and send print job
        ///     (v)	Verify PEAP Authentication
        ///     (vi)	Should Authenticate successfully and the print job should complete
        ///     (vii)	Disable FIPS option
        ///     (viii)	Should Authenticate successfully and the print job should complete
        ///10.	Configure radius server with PEAP-MS-CHAP2 Authentication mode, enable only SSL3.0 on the radius server, install intermediate CA certificate on the device and perform the following operation
        ///     (i)	Disable SSL3.0 on the device and enable TLS 1.0,1.1,1.2
        ///     (ii)	 Try to Authenticate the server
        ///     (iii)	Authentication should fail
        ///     (iv)	Enable FIPS on the device and send print job
        ///     (v)	Verify PEAP Authentication
        ///     (vi)	Should Authenticate successfully and the print job should complete
        ///     (vii)	Disable FIPS option
        ///     (viii)	Should Authenticate successfully and the print job should complete
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool VerifyDotOnexConfigurationsinFIPS(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            string idCertificatePath = string.Empty;

            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }
            if (!TestPrerequisites(activityData))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                EwsWrapper.Instance().RestoreSecurityDefaults();
                    
				if (!ValidateDotOnexConfiguration(activityData, AuthenticationMode.PEAP, testId, out idCertificatePath))
				{
					return false;
				}
                                
				// enable FIPS
				if(!EwsWrapper.Instance().SetFipsOption(true))
				{
					return false;
				}
				//submit print job
			   if(! PrintValidation(activityData, testId))
			   {
				   return false;
			   }
			   TraceFactory.Logger.Info("Print Job is successful after enabling FIPS with Dot1x");

                // disable FIPS
                if (!EwsWrapper.Instance().SetFipsOption(false))
                {
                    return false;
                }

                //submit print job
                if (!PrintValidation(activityData, testId))
                {
                    return false;
                }
                TraceFactory.Logger.Info("Print Job is successful after disabling FIPS with Dot1x");
                return true;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

                PostRequisites802Dot1XAuthentication(activityData, deleteDynamicIdCertificate: true);
            }
        }

        /// <summary>
        /// Validates the dot1x configuration 
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="authenticationMode"><see cref="authenticationMode"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        public static bool ValidateDotOnexConfiguration(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId, out string idCertificatePath)
        {

            idCertificatePath = string.Empty;
            if (!ConfigureRadiusServer(activityData, authenticationMode))
            {
                return false;
            }

            if (!GenerateIdCertificate(activityData, out idCertificatePath, false))
            {
                return false;
            }

            if (!EwsWrapper.Instance().InstallCertificate(idCertificatePath))
            {
                return false;
            }

            string connectivityShare = CtcSettings.ConnectivityShare;

            if (!EwsWrapper.Instance().InstallCACertificate(Path.Combine(connectivityShare, CA_CERTIFICATEPATH.FormatWith(activityData.ProductFamily, activityData.RadiusServerType)), activityData.RadiusServerType == RadiusServerTypes.SubSha2))
            {
                return false;
            }

            string certificatePath = Path.Combine(SERVER_DYNAMIC_ID_CERTIFICATEPATH, Path.GetFileName(idCertificatePath));


            using (RadiusApplicationServiceClient client = RadiusApplicationServiceClient.Create(activityData.RadiusServerIp))
            {
                if (client.Channel.MapIdCertificate(activityData.DotOneXUserName, certificatePath))
                {
                    TraceFactory.Logger.Info("Successfully added the certificate: {0} to user: {1}".FormatWith(certificatePath, activityData.DotOneXUserName));
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to add the certificate: {0} to user: {1}".FormatWith(certificatePath, activityData.DotOneXUserName));
                    return false;
                }
            }

            Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
            {
                AuthenticationProtocol = authenticationMode,
                UserName = activityData.DotOneXUserName,
                Password = activityData.DotOneXPassword,
              //  EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                EncryptionStrength= EncryptionStrengths.High, 
                FailSafe = FallbackOption.Connect,
                ReAuthenticate = true
            };

            if (!EwsWrapper.Instance().Set802Dot1XAuthentication(configDetails))
            {
                return false;
            }

            string packetFileName = "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, authenticationMode);

            PacketDetails packetDetails = null;

            using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
            {
                string guid = client.Channel.TestCapture(activityData.SessionId, packetFileName);

                try
                {
                    INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                    networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);

                    Thread.Sleep(CAPTURE_TIMEOUT_SUCCESS);
                }
                finally
                {
                    packetDetails = client.Channel.Stop(guid);
                }
            }

            if (!Basic802Dot1XValidation(activityData, authenticationMode))
            {
                return false;
            }

            string filter = "eap.code == 3 && !radius";
            string tokenInPackets = "Success";

            return Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets);
        }

        #endregion

        #region New

        public static bool AuthenticationWithAlternateModes(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = AuthenticationMode.PEAP,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = true
                };

				string filter = "eap.code == 3 && !radius";
				string tokenInPackets = "Success";
                
                for (int i = 0; i < 5; i++)
				{
					configDetails.AuthenticationProtocol = AuthenticationMode.PEAP;

                    TraceFactory.Logger.Info("{0} : {1} authentication".FormatWith(i, authenticationMode));

                    if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}-Iteration({3})".FormatWith(testId, activityData.RadiusServerType, authenticationMode, i)))
                    {
                        return false;
                    }

                    configDetails.AuthenticationProtocol = AuthenticationMode.EAPTLS;

                    TraceFactory.Logger.Info("{0} : {1} authentication".FormatWith(i, authenticationMode));

                    if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}-Iteration({3})".FormatWith(testId, activityData.RadiusServerType, authenticationMode, i)))
                    {
                        return false;
                    }

                    if (PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily) && i > 2)
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info(ex.Message);
                return false;
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        public static bool AuthenticationWithInvalidUserName(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId, bool invalidPassword = false)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = true
                };

                TraceFactory.Logger.Info("Step I: {0} Basic Authentication".FormatWith(authenticationMode));

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, authenticationMode), cleanUp: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: Authentication with {0}".FormatWith(invalidPassword ? "Invalid Password" : "Invalid Username"));

                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-{1}-{2}-{3}".FormatWith(testId, activityData.RadiusServerType, authenticationMode, invalidPassword ? "Invalid Password" : "Invalid Username"));
                    PacketDetails packetDetails = null;

                    if (invalidPassword)
                    {
                        configDetails.Password = CtcUtility.GetUniqueHostName();
                    }
                    else
                    {
                        configDetails.UserName = CtcUtility.GetUniqueHostName();
                    }

                    try
                    {

                        if (!EwsWrapper.Instance().Set802Dot1XAuthentication(configDetails))
                        {
                            return false;
                        }

                        networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);

                        Thread.Sleep(invalidPassword ? (authenticationMode == AuthenticationMode.EAPTLS ? CAPTURE_TIMEOUT_SUCCESS : CAPTURE_TIMEOUT_FAILURE) : CAPTURE_TIMEOUT_FAILURE);
                    }
                    finally
                    {
                        packetDetails = client.Channel.Stop(guid);
                    }
                    // This condition if it is Invalid Password and PEAP, since this will not have any Success of Failuare packets in the Packets capture
                    //Invalid Password + PEAP = FAIL
                    if (invalidPassword && authenticationMode == AuthenticationMode.PEAP)
                    {
                        filter = "eap.code == 4 && !radius";
                        tokenInPackets = "Success";
                        if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                        {
                            TraceFactory.Logger.Info("Success message is present in the packet caoture which is not expected in case of Invalid Password and PEAP.");
                            return false;
                        }
                        return true;
                    }//Invalid PAssword + EAP TLS = PASS
                    else if (invalidPassword && authenticationMode == AuthenticationMode.EAPTLS)
                    {
                       // tokenInPackets = "Success";
                        if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                        {
                            TraceFactory.Logger.Info("Success message is not present in the packet caoture which is not expected in case of Invalid Password and TLS.");
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        //Invalid Username + EAPTLS =FAIL
                        if (!invalidPassword && authenticationMode == AuthenticationMode.EAPTLS)
                        {
                            if (!Basic802Dot1XValidation(activityData))
                            {
                                TraceFactory.Logger.Info("As Expected : Invalid Username with EAP TLS Fails");
                                return true;
                            }
                            return Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets);
                        }
                        //Invalid Username +PEAP = FAILS 
                        if (Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                        {
                            if (invalidPassword && authenticationMode == AuthenticationMode.EAPTLS)
                            {
                                return true;
                            }
							return false;
                        }

                        filter = "eap.code == 4 && !radius";
                        tokenInPackets = "Failure";

						return Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets);
                    }

                   // return Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets);
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        public static bool AuthenticationAfterPowercycle(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address)))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EncryptionStrengths.Medium,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = false
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, authenticationMode), cleanUp: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: {0} Authentication after power cycle.".FormatWith(authenticationMode));

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = string.Empty;
                    PacketDetails packetDetails = null;

                    try
                    {
                        guid = client.Channel.TestCapture(activityData.SessionId, "{0}-{1}-{2}-AfterPowerCycle".FormatWith(testId, activityData.RadiusServerType, authenticationMode));

                        Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.Ipv4Address);
                        printer.PowerCycle();

                        Thread.Sleep(CAPTURE_TIMEOUT_SUCCESS);
                    }
                    finally
                    {
                        packetDetails = client.Channel.Stop(guid);
                    }

                    TraceFactory.Logger.Info("Expected: Negotiation happens after power cycle.");
                    return Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets);
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        public static bool AuthenticationAfterColdReset(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address)))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EncryptionStrengths.Medium,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = false
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, authenticationMode), cleanUp: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: {0} Authentication after cold reset.".FormatWith(authenticationMode));

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, "{0}-{1}-{2}-AfterColdReset".FormatWith(testId, activityData.RadiusServerType, authenticationMode));

					Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.Ipv4Address);

                    if (!PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                    {
                        printer.ColdReset();
                    }
                    else
                    {
                        EwsWrapper.Instance().SetNetworkDefaults();
                    }

                    Thread.Sleep(CAPTURE_TIMEOUT_FAILURE);

                    PacketDetails packetDetails = client.Channel.Stop(guid);

                    TraceFactory.Logger.Info("Expected: Negotiation doesnot happens after cold reset.");
                    return !Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets);
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        public static bool AuthenticationWithServerID(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address)))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step I: {0} Basic authentication.".FormatWith(authenticationMode));

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EncryptionStrengths.Medium,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = false
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, authenticationMode), cleanUp: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: {0} Authentication with valid Server ID.".FormatWith(authenticationMode));

                PacketDetails packetDetails = null;
                string guid = string.Empty;

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                    networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);

                    try
                    {
                        guid = client.Channel.TestCapture(activityData.SessionId, "{0}-{1}-{2}-ValidServerID".FormatWith(testId, activityData.RadiusServerType, authenticationMode));

                        string fqdn = string.Empty;

                        using (SystemConfigurationClient radiusClient = SystemConfigurationClient.Create(activityData.RadiusServerIp))
                        {
                            fqdn = radiusClient.Channel.GetFullyQualifiedname();
                        }

                        if (string.IsNullOrEmpty(fqdn))
                        {
                            TraceFactory.Logger.Info("Failed to get the fully qualified name of the server: {0}.".FormatWith(activityData.RadiusServerIp));
                            return false;
                        }

                        TraceFactory.Logger.Info("FQDn:{0}".FormatWith(fqdn));
                        configDetails.ServerId = fqdn;

                        if (!EwsWrapper.Instance().Set802Dot1XAuthentication(configDetails))
                        {
                            return false;
                        }

                        networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);

                        Thread.Sleep(CAPTURE_TIMEOUT_SUCCESS);
                    }
                    finally
                    {
                        packetDetails = client.Channel.Stop(guid);
                    }

                    if (!Basic802Dot1XValidation(activityData, authenticationMode))
                    {
                        return false;
                    }

                    if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                    {
                        return false;
                    }
                }

                TraceFactory.Logger.Info("Step III: {0} Authentication with invalid Server ID.".FormatWith(authenticationMode));

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                    networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);

                    try
                    {
                        guid = client.Channel.TestCapture(activityData.SessionId, "{0}-{1}-{2}-InvalidServerID".FormatWith(testId, activityData.RadiusServerType, authenticationMode));

                        configDetails.ServerId = "abcd.com";
                        //Invalid server id, authentication should fail : Lagacy NAK
                        filter = "eap.type == 3 && eapol";
                        tokenInPackets = "Legacy Nak";

                        if (!EwsWrapper.Instance().Set802Dot1XAuthentication(configDetails))
                        {
                            return false;
                        }

                        networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);

                        Thread.Sleep(CAPTURE_TIMEOUT_SUCCESS);
                    }
                    finally
                    {
                        packetDetails = client.Channel.Stop(guid);
                    }
                    //Changed this part since the test was failing with all correct behavior
                    return Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets);
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        /// <summary>
        /// Reset 802.1x authentication from control panel
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="authenticationMode"></param>
        /// <param name="testId"></param>
        /// <returns></returns>
        public static bool ResetAuthenticationControlPanel(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId)
        {
            try
            {
                if (!TestPrerequisites(activityData))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.Manual, expectedIPAddress: IPAddress.Parse(activityData.Ipv4Address)))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step I: Basic {0} authentication.".FormatWith(authenticationMode));

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EncryptionStrengths.Medium,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = false
                };

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";

                if (!Validate802Dot1XAuthentication(activityData, configDetails, filter, tokenInPackets, "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, authenticationMode), cleanUp: false))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: Reset authentication from Control Panel.".FormatWith(authenticationMode));

                if (!CtcUtility.ResetDot1xControlPanel(activityData.Ipv4Address))
                {
                    return false;
                }

                if (Basic802Dot1XValidation(activityData, validateConfiguration: false))
                {
                    return false;
                }

                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);

                if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), TimeSpan.FromMinutes(2)))
                {
                    TraceFactory.Logger.Info("Printer is not available.");
                    return false;
                }

                return !EwsWrapper.Instance().Get802Dot1xStatus();
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData);
            }
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Shows the error popup
        /// </summary>
        /// <param name="errorMessage">The message to be shown.</param>
        /// <returns>True if the user clicks retry, else false.</returns>
        public static bool ShowErrorPopUp(string errorMessage)
        {
            DialogResult result = MessageBox.Show(errorMessage + "Click Retry to continue or Cancel to ignore.", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);

            if (result == DialogResult.Retry)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool GenerateIdCertificate(DotOneXActivityData activityData, out string certificatePath, bool exportPrivateKey = false)
        {
            certificatePath = string.Empty;
            try
            {
                string certificateRequest = string.Empty;

                if (!EwsWrapper.Instance().CreateIDCertificateRequest((activityData.RadiusServerType == RadiusServerTypes.RootSha1) ? SignatureAlgorithm.Sha1 : SignatureAlgorithm.Sha512, out certificateRequest, exportPrivateKey))
                {
                    return false;
                }

                Encoding encoding = (PrinterFamilies)Enum<PrinterFamilies>.Parse(activityData.ProductFamily) == PrinterFamilies.InkJet ? Encoding.Base64 : Encoding.DER;

                if (!RadiusApplication.GenerateCertificate(certificateRequest, activityData.RadiusServerIp, CtcUtility.SERVER_USERNAME, CtcUtility.SERVER_PASSWORD, out certificatePath, CERTIFICATE_TEMPLATE_NAME, encoding))
                {
                    return false;
                }

                string serverCertificatePath = DYNAMIC_ID_CERTIFICATEPATH.FormatWith(activityData.RadiusServerIp);

                using (UserImpersonator localUser = new UserImpersonator())
                {
                    // TODO: Get the user name and password from base plugin.
                    localUser.Impersonate(CtcUtility.SERVER_USERNAME, CtcUtility.SERVER_DOMAIN, CtcUtility.SERVER_PASSWORD);
                    if (Directory.Exists(serverCertificatePath))
                    {
                        foreach (FileInfo file in (new DirectoryInfo(serverCertificatePath).GetFiles()))
                        {
                            TraceFactory.Logger.Info("File: {0} is deleted".FormatWith(file.Name));
                            file.Delete();
                        }

                        // Copy the certificate to \\<server IP>\Dynamic Certificate folder.
                        File.Copy(certificatePath, Path.Combine(serverCertificatePath, Path.GetFileName(certificatePath)));

                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("The path:{0} does not exist".FormatWith(serverCertificatePath));
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info(ex.Message);
                return false;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Install both CA and ID certificate on the printer based on the <see cref="RadiusServerTypes"/>
        /// </summary>
        /// <param name="serverType"><see cref="RadiusServerTypes"/></param>
        /// <param name="validCaCertificate">True to install valid CA certificate.</param>
        /// <param name="validIdCertificate">True to install valid ID certificate.</param>
        /// <returns>true if the certificate installation is successful, else false.</returns>
        private static bool InstallCertificate(RadiusServerTypes serverType, string productFamily, bool validCaCertificate = true, bool validIdCertificate = true)
        {
            string caCerticatePath = string.Empty;
            string idCertificatePath = string.Empty;

            string connectivityShare = CtcSettings.ConnectivityShare;
            caCerticatePath = validCaCertificate ? Path.Combine(connectivityShare, CA_CERTIFICATEPATH.FormatWith(productFamily, serverType)) : Path.Combine(connectivityShare, INVALID_CA_CERTIFICATEPATH);
            idCertificatePath = validIdCertificate ? Path.Combine(connectivityShare, ID_CERTIFICATEPATH.FormatWith(productFamily, serverType)) : Path.Combine(connectivityShare, INVALID_ID_CERTIFICATEPATH);
            bool allowInterMediate = (serverType == RadiusServerTypes.SubSha2 && validCaCertificate) ? true : false;

            string message = "{0} and {1}".FormatWith(validCaCertificate ? "Valid CA" : "invalid CA", validIdCertificate ? "valid ID" : "invalid ID");

            if (EwsWrapper.Instance().InstallCACertificate(caCerticatePath, allowInterMediate)
                       && EwsWrapper.Instance().InstallIDCertificate(idCertificatePath, ID_CERTIFICATE_PASSWORD))
            {
                TraceFactory.Logger.Info("{0} {1} certificates are successfully installed on the printer".FormatWith(validCaCertificate && validIdCertificate ? serverType.ToString() : string.Empty, message));
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to install {0} Radius server certificates".FormatWith(validCaCertificate && validIdCertificate ? serverType.ToString() : string.Empty, message));
                return false;
            }
        }

        /// <summary>
        /// Test prerequisites 
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <returns>True if the test prerequisites are in place, else false.</returns>
        private static bool TestPrerequisites(DotOneXActivityData activityData)
        {
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);

                bool continueTest = true;

                while (continueTest && !NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), TimeSpan.FromSeconds(30)))
                {
                    continueTest = ShowErrorPopUp("Printer: {0} is not available.\n Please cold reset the printer.".FormatWith(activityData.Ipv4Address));
                }

                while (continueTest && !NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.PhysicalMachineIp), TimeSpan.FromSeconds(30)))
                {
                    continueTest = ShowErrorPopUp("Physical machine: {0} is not available.\n Check if physical machine is accessible.".FormatWith(activityData.PhysicalMachineIp));
                }

                while (continueTest && !NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.SwitchIp), TimeSpan.FromSeconds(30)))
                {
                    continueTest = ShowErrorPopUp("Switch: {0} is not available.\n Please check the switch configurations.");
                }

                if (!continueTest)
                {
                    return false;
                }

                // TODO: 1. Check radius server pre-requisites:- switch client
                // 2. Check switch for the radius server details

                CtcUtility.StartService("PacketCaptureService", activityData.PhysicalMachineIp);
                CtcUtility.StartService("WindowsServerService", activityData.RadiusServerIp);

                EwsWrapper.Instance().WakeUpPrinter();
                EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Configure and validate the 802.1x Authentication
        /// Steps followed:
        /// Configure Radius server -> Install certificates on the printer -> Set authentication on printer -> Start packet capture -> Configure authenticator port on switch -> Stop packet capture and perform validations.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="configurationDetails"><see cref="configurationDetails"/></param>
        /// <param name="packetsFilter">Filter to be applied for packet validation.</param>
        /// <param name="tokenInPackets">Search value in packets.</param>
        /// <param name="packetFileName">File name for the captured wireshark traces.</param>
        /// <param name="serverAuthenticationModes">The <see cref="AuthenticationMode"/> to be configured on server.</param>
        /// <param name="serverPriority">The <see cref="AuthenticationMode"/> to be configured on server as priority.</param>
        /// <param name="authenticationOnPort">True to enable authentication on port.</param>
        /// <param name="installCertificates">True to install certificates.</param>
        /// <param name="validCaCertificate">True to install valid ca certificates.</param>
        /// <param name="validIdCertificate">True to install valid id certificates.</param>
        /// <param name="cleanUp">True to perform clean up.</param>
        /// <returns>True if the configuration and validations are successful.</returns>
        private static bool Validate802Dot1XAuthentication(DotOneXActivityData activityData, Dot1XConfigurationDetails configurationDetails, string packetsFilter, string tokenInPackets, string packetFileName,
            AuthenticationMode serverAuthenticationModes = AuthenticationMode.None, AuthenticationMode serverPriority = AuthenticationMode.None, bool authenticationOnPort = true, bool installCertificates = true, bool validCaCertificate = true, bool validIdCertificate = true, bool cleanUp = true)
        {
            try
            {
                serverAuthenticationModes = serverAuthenticationModes == AuthenticationMode.None ? configurationDetails.AuthenticationProtocol : serverAuthenticationModes;

                if (!ConfigureRadiusServer(activityData, serverAuthenticationModes, serverPriority))
                {
                    return false;
                }

                if (installCertificates && !InstallCertificate(activityData.RadiusServerType, activityData.ProductFamily, validCaCertificate, validIdCertificate))
                {
                    return false;
                }

                // Authentication fails when, 1. EAPTS, Invalid CA or Id certificates 2. PEAP, Invalid CA certificate 3. When the server authentication mode and printer authentication modes are not matching.
                bool isFailureCase = (configurationDetails.AuthenticationProtocol == AuthenticationMode.EAPTLS && (!validCaCertificate || !validIdCertificate))
                                        || (configurationDetails.AuthenticationProtocol == AuthenticationMode.PEAP && !validCaCertificate)
                                        || ((configurationDetails.AuthenticationProtocol & serverAuthenticationModes) == 0);

                PacketDetails packetDetails = null;

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, packetFileName);

                    try
                    {
                        if (!EwsWrapper.Instance().Set802Dot1XAuthentication(configurationDetails))
                        {
                            return false;
                        }

                        if (authenticationOnPort)
                        {
                            INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                            networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);
                        }

                        // Explicitly giving more time for failure cases.
                        Thread.Sleep(isFailureCase ? CAPTURE_TIMEOUT_FAILURE : CAPTURE_TIMEOUT_SUCCESS);
                    }
                    finally
                    {
                        packetDetails = client.Channel.Stop(guid);
                    }
                }

                if (isFailureCase)
                {
                    if (Basic802Dot1XValidation(activityData, (serverPriority != AuthenticationMode.None) ? serverPriority : configurationDetails.AuthenticationProtocol))
                    {
                        return false;
                    }

                    if (PrintValidation(activityData, packetFileName, isFailureCase))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!Basic802Dot1XValidation(activityData, (serverPriority != AuthenticationMode.None) ? serverPriority : configurationDetails.AuthenticationProtocol))
                    {
                        return false;
                    }

                    if (!PrintValidation(activityData, packetFileName, isFailureCase))
                    {
                        return false;
                    }
                }

                return Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, packetsFilter, tokenInPackets);
            }
            finally
            {
                if (cleanUp)
                {
                    PostRequisites802Dot1XAuthentication(activityData);
                }
            }
        }

        /// <summary>
        /// Validates ping and EWS page for the configured authentication mode.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="mode"><see cref="AuthenticationMode"/></param>
        /// <returns></returns>
        private static bool Basic802Dot1XValidation(DotOneXActivityData activityData, AuthenticationMode mode = AuthenticationMode.EAPTLS, bool validateConfiguration = true)
        {
            if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), TimeSpan.FromMinutes(3)))
            {
                TraceFactory.Logger.Info("Ping with IP address: {0} is successful.".FormatWith(activityData.Ipv4Address));
            }
            else
            {
                TraceFactory.Logger.Info("Connection to the printer is lost");
                return false;
            }

            if (validateConfiguration)
            {
                return EwsWrapper.Instance().ValidateConfiguredAuthentication(mode);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Prints a file.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="failureCase">True to specify failure cases, else false.</param>
        /// <param name="data">The data to be printed.</param>
        /// <returns>true if the print job is successful, else false.</returns>
        private static bool PrintValidation(DotOneXActivityData activityData, string data, bool failureCase = false)
        {
            if (activityData.RequirePrintValidation)
            {
                Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.Ipv4Address);
                return printer.PrintLpr(IPAddress.Parse(activityData.Ipv4Address), CtcUtility.CreateFile(data));
            }
            else
            {
                TraceFactory.Logger.Info("Print is not validated.");
                return !failureCase && true;
            }
        }

        /// <summary>
        /// Validates the captured wireshark traces for the specified search value with the specified filter.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="packetLocation">The packets file location.</param>
        /// <param name="filter">Filter to be applied for validation.</param>
        /// <param name="tokenInPackets">The search value in wire shark traces.</param>
        /// <returns>True if the validation is successful, else false.</returns>
        private static bool Advanced802Dot1XValidation(DotOneXActivityData activityData, string packetLocation, string filter, string tokenInPackets)
        {
            TraceFactory.Logger.Info(CtcBaseTests.PACKET_VALIDATION);

            using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
            {
                string errorMessage = string.Empty;

                if (client.Channel.Validate(packetLocation, filter, ref errorMessage, tokenInPackets))
                {
                    TraceFactory.Logger.Info("Successfully validated the wireshark traces from {0} with filter: {1} and search value: {2}.".FormatWith(Path.GetFileName(packetLocation), filter, tokenInPackets));
                    TraceFactory.Logger.Info(errorMessage);
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to validate the wireshark traces from {0} with filter: {1} and search value: {2}.".FormatWith(Path.GetFileName(packetLocation), filter, tokenInPackets));
                    TraceFactory.Logger.Info(errorMessage);
                    return false;
                }
            }
        }

        /// <summary>
        /// Post requisites for 802.1X authentication.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <returns>True if the post requisites are successful, else false.</returns>
        private static bool PostRequisites802Dot1XAuthentication(DotOneXActivityData activityData, bool performHosebreak = false, bool deleteDynamicIdCertificate = false)
        {
            TraceFactory.Logger.Info("Cleaning Up 802.1x authentication settings.");

			INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
			networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);
            //Adding this code since in Fail case, Printer need to be reconnected to the port, so performing Disable and enable
            TraceFactory.Logger.Info("Disabling and Enabling the port again.");

            networkSwitch.DisablePort(activityData.AuthenticatorPort);
            Thread.Sleep(TimeSpan.FromSeconds(10));
            networkSwitch.EnablePort(activityData.AuthenticatorPort);
            //This is old code 
            if (performHosebreak)
            {
                networkSwitch.DisablePort(activityData.AuthenticatorPort);
                Thread.Sleep(TimeSpan.FromSeconds(10));
                networkSwitch.EnablePort(activityData.AuthenticatorPort);
            }

            if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), TimeSpan.FromMinutes(6)))
            {
                TraceFactory.Logger.Info("Printer: {0} is available.".FormatWith(activityData.Ipv4Address));
            }
            else
            {
                TraceFactory.Logger.Info("Connection to the printer is lost.");
            }

            if (deleteDynamicIdCertificate)
            {
                using (RadiusApplicationServiceClient client = RadiusApplicationServiceClient.Create(activityData.RadiusServerIp))
                {
                    if (Directory.Exists(SERVER_DYNAMIC_ID_CERTIFICATEPATH))
                    {
                        foreach (var item in Directory.GetFiles(SERVER_DYNAMIC_ID_CERTIFICATEPATH, "*.cer"))
                        {
                            if (client.Channel.DeleteIdCertificate(activityData.DotOneXUserName, item))
                            {
                                TraceFactory.Logger.Info("Successfully deleted the certificate: {0} from user: {1}".FormatWith(item, activityData.DotOneXUserName));
                            }
                            else
                            {
                                TraceFactory.Logger.Info("Failed to delete the certificate: {0} from user: {1}".FormatWith(item, activityData.DotOneXUserName));
                            }
                        }
                    }
                }
            }

            if (PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily))
            {
                EwsWrapper.Instance().SetNetworkDefaults();
               // if (activityData.ProductName == "Photon")
               // {
               //     MessageBox.Show("Perform reset Do1x authentication from front panel");
               // }
            }
            else
            {
                if (!EwsWrapper.Instance().Reset802Dot1XAuthentication())
                {
                    return false;
                }
            }

            return true; //EwsWrapper.Instance().UnInstallAllCertificates();
        }

        /// <summary>
        /// Configure radius server with the specified authentication modes.
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="authenticationModes">The <see cref="AuthenticationMode"/> to be set on radius server.</param>
        /// <param name="priority">The priority <see cref="AuthenticationMode"/> on server.</param>
        /// <returns>True if the configuration is successful, else false.</returns>
        private static bool ConfigureRadiusServer(DotOneXActivityData activityData, AuthenticationMode authenticationModes, AuthenticationMode priority = AuthenticationMode.None)
        {
            using (RadiusApplicationServiceClient radiusClient = RadiusApplicationServiceClient.Create(activityData.RadiusServerIp))
            {
                if (radiusClient.Channel.SetAuthenticationMode(activityData.PolicyName, authenticationModes, priority))
                {
                    TraceFactory.Logger.Info("Successfully configured radius server with : {0} {1}.".FormatWith(authenticationModes, (priority == AuthenticationMode.None ? string.Empty : "with {0} as priority.".FormatWith(priority))));

                    if (priority != AuthenticationMode.None)
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                    }

                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to configure radius server with : {0} {1}.".FormatWith(authenticationModes, (priority == AuthenticationMode.None ? string.Empty : "with {0} as priority.".FormatWith(priority))));
                    return false;
                }
            }
        }

        /// <summary>
        /// Validates the block network feature
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="configDetails"><see cref="Dot1XConfigurationDetails"/></param>
        /// <returns>True for success, else false.</returns>
        private static bool ValidateBlockNetwork(DotOneXActivityData activityData, Dot1XConfigurationDetails configDetails, string printData)
        {
            try
            {
                if (!ConfigureRadiusServer(activityData, configDetails.AuthenticationProtocol))
                {
                    return false;
                }

                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                networkSwitch.DisableAuthenticatorPort(activityData.AuthenticatorPort);

                if (!EwsWrapper.Instance().Set802Dot1XAuthentication(configDetails))
                {
                    return false;
                }

                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), TimeSpan.FromMinutes(3)))
                {
                    TraceFactory.Logger.Info("Ping with IP address: {0} is successful.".FormatWith(activityData.Ipv4Address));
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Connection to the printer is lost");
                }

                return !PrintValidation(activityData, printData, true);
            }
            finally
            {
                // Enable authentication on port to bring the printer back to network.
                INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);

                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.Ipv4Address), TimeSpan.FromMinutes(2)))
                {
                    TraceFactory.Logger.Info("Ping with IP address: {0} is successful.".FormatWith(activityData.Ipv4Address));
                    if (!EwsWrapper.Instance().Reset802Dot1XAuthentication())
                    {
					TraceFactory.Logger.Info("Failed to reset");
                    }
                }  
				else
				{
					TraceFactory.Logger.Info("Error : Connection to the printer is lost");
				}
                

            }
        }

        /// <summary>
        /// Generate Id Certificate and verify authentication
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="authenticationMode"><see cref="AuthenticationMode"/></param>
        /// <param name="testId">Test Id</param>
        /// <param name="exportPrivateKey">true to export private key, false otherwise</param>
        /// <returns></returns>
        private static bool AuthenticationWithIdCertificate(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId, bool exportPrivateKey = false)
        {
            try
            {
                if (!ConfigureRadiusServer(activityData, authenticationMode))
                {
                    return false;
                }

                string idCertificatePath = string.Empty;

                if (!GenerateIdCertificate(activityData, out idCertificatePath, exportPrivateKey))
                {
                    return false;
                }

                if (!EwsWrapper.Instance().InstallCertificate(idCertificatePath))
                {
                    return false;
                }

                string connectivityShare = CtcSettings.ConnectivityShare;

                if (!EwsWrapper.Instance().InstallCACertificate(Path.Combine(connectivityShare, CA_CERTIFICATEPATH.FormatWith(activityData.ProductFamily, activityData.RadiusServerType)), activityData.RadiusServerType == RadiusServerTypes.SubSha2))
                {
                    return false;
                }

                string certificatePath = Path.Combine(SERVER_DYNAMIC_ID_CERTIFICATEPATH, Path.GetFileName(idCertificatePath));

                using (RadiusApplicationServiceClient client = RadiusApplicationServiceClient.Create(activityData.RadiusServerIp))
                {
                    if (client.Channel.MapIdCertificate(activityData.DotOneXUserName, certificatePath))
                    {
                        TraceFactory.Logger.Info("Successfully added the certificate: {0} to user: {1}".FormatWith(certificatePath, activityData.DotOneXUserName));
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Failed to add the certificate: {0} to user: {1}".FormatWith(certificatePath, activityData.DotOneXUserName));
                        return false;
                    }
                }

                Dot1XConfigurationDetails configDetails = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = authenticationMode,
                    UserName = activityData.DotOneXUserName,
                    Password = activityData.DotOneXPassword,
                    EncryptionStrength = EwsWrapper.Instance().IsOmniOpus ? EncryptionStrengths.Medium : EncryptionStrengths.Low,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = true
                };

                if (!EwsWrapper.Instance().Set802Dot1XAuthentication(configDetails))
                {
                    return false;
                }

                string packetFileName = "{0}-{1}-{2}".FormatWith(testId, activityData.RadiusServerType, authenticationMode);

                PacketDetails packetDetails = null;

                using (PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PhysicalMachineIp))
                {
                    string guid = client.Channel.TestCapture(activityData.SessionId, packetFileName);

                    try
                    {
                        INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIp));
                        networkSwitch.EnableAuthenticatorPort(activityData.AuthenticatorPort);

                        Thread.Sleep(CAPTURE_TIMEOUT_SUCCESS);
                    }
                    finally
                    {
                        packetDetails = client.Channel.Stop(guid);
                    }
                }

                if (!Basic802Dot1XValidation(activityData, authenticationMode))
                {
                    return false;
                }

                string filter = "eap.code == 3 && !radius";
                string tokenInPackets = "Success";

                if (!Advanced802Dot1XValidation(activityData, packetDetails.PacketsLocation, filter, tokenInPackets))
                {
                    return false;
                }

                return PrintValidation(activityData, testId);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                PostRequisites802Dot1XAuthentication(activityData, deleteDynamicIdCertificate: true);
                EwsWrapper.Instance().UnInstallAllCertificates();
            }
        }

        #region IPSecurity
        /// <summary>
        /// Validate the Dot1x Authentication, IPSec Behavior, HTTPS, Discovery operations
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        private static bool ValidateIPsecBehaviourWithDotOnexConfiguration(DotOneXActivityData activityData, AuthenticationMode authenticationMode, string testId, bool exportPrivateKey = false)
        {
            try
            {
                TraceFactory.Logger.Info("Validating Dot1x AUthentication");

                Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.Ipv4Address);

                if (!AuthenticationWithIdCertificate(activityData, authenticationMode, testId, exportPrivateKey))
                {
                    TraceFactory.Logger.Info("Failed to authenticate Dot1x");
                    return false;
                }

                if (!ValidateIPsecBehaviour(activityData, testId))
                {
                    return false;
                }

                if (!printer.IsEwsAccessible("https"))
                {
                    return false;
                }

                if (PrinterDiscovery.ProbeDevice(printer.WiredIPv4Address).Equals(null))
                {
                    if (PrinterDiscovery.ProbeDevice(printer.WiredIPv4Address).Equals(null))
                    {
                        TraceFactory.Logger.Info("Printer IP: {0} is not discovered through WJA".FormatWith(printer.WiredIPv4Address));
                        return false;
                    }
                }

                TraceFactory.Logger.Info("Printer IP: {0} is discovered through WJA".FormatWith(printer.WiredIPv4Address));
                return true;
            }
            finally
            {
                CtcUtility.DeleteAllIPsecRules();
                EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Validate IPSec Behavior, HTTPS, Discovery operations
        /// </summary>
        /// <param name="activityData"><see cref="DotOneXActivityData"/></param>
        /// <param name="testId">The test case Id.</param>
        /// <returns>true for success, else false.</returns>
        private static bool ValidateIPsecBehaviour(DotOneXActivityData activityData, string testId)
        {
            TraceFactory.Logger.Info("Validating IPSecurity With Dot1x");
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.Ipv4Address);

            // Get the basic rule settings
            SecurityRuleSettings settings = GetPSKRuleSettings(int.Parse(testId));

            // create rule with basic settings
            EwsWrapper.Instance().CreateRule(settings, true);

            // set the default action to "Drop"
            EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

            // Enable firewall option
            EwsWrapper.Instance().SetIPsecFirewall(true);

            // create similar rule on the client
            CtcUtility.CreateIPsecRule(settings);

            // since the rule is enabled state, ping should work    
            return CtcUtility.ValidateDeviceServices(printer, activityData.Ipv4Address, ping: DeviceServiceState.Pass);
        }

        private static SecurityRuleSettings GetPSKRuleSettings(int testNo, DefaultAddressTemplates address = DefaultAddressTemplates.AllIPAddresses, DefaultServiceTemplates service = DefaultServiceTemplates.AllServices,
                                                       IKESecurityStrengths strength = IKESecurityStrengths.HighInteroperabilityLowsecurity, IKEPhase1Settings phase1Settings = null, IKEPhase2Settings phase2Settings = null, DefaultAction defaultAction = DefaultAction.Drop)
        {
            AddressTemplateSettings addressTemplate = new AddressTemplateSettings(address);
            ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(service);

            return GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, strength, phase1Settings, phase2Settings, defaultAction: defaultAction);
        }

        public static SecurityRuleSettings GetPSKRuleSettings(int testNo, AddressTemplateSettings address, ServiceTemplateSettings service, IKESecurityStrengths strength = IKESecurityStrengths.HighInteroperabilityLowsecurity,
                                                       IKEPhase1Settings phase1Settings = null, IKEPhase2Settings phase2Settings = null, DefaultAction defaultAction = DefaultAction.Drop)
        {
            IKEv1Settings ikeV1Settings = null;

            // For Custom strength, provide Phase 1 and Phase 2 settings
            if (IKESecurityStrengths.Custom == strength)
            {
                if (null == phase1Settings)
                {
                    throw new ArgumentNullException("phase1Settings", "Invalid phase 1 settings.");
                }

                if (null == phase2Settings)
                {
                    throw new ArgumentNullException("phase2Settings", "Invalid phase 2 settings.");
                }

                ikeV1Settings = new IKEv1Settings(PRESHAREDKEY, phase1Settings, phase2Settings);
            }
            else
            {
                ikeV1Settings = new IKEv1Settings(PRESHAREDKEY);
            }

            DynamicKeysSettings dynamicSettings = new DynamicKeysSettings(strength, ikeV1Settings);
            IPsecTemplateSettings ipsecTemplate = new IPsecTemplateSettings(IPSECTEMPLATENAME.FormatWith(testNo), dynamicSettings);

            SecurityRuleSettings securitySettings = new SecurityRuleSettings(CLIENT_RULE_NAME.FormatWith(testNo), address, service, IPsecFirewallAction.ProtectedWithIPsec, ipsecTemplate, defaultAction);

            return securitySettings;
        }

        #endregion

        #endregion

    }
}
