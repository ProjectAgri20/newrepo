using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.Dhcp;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.NetworkSwitch;
using HP.ScalableTest.PluginSupport.Connectivity.PacketCapture;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.Utility;
using Printer = HP.ScalableTest.PluginSupport.Connectivity.Printer;

namespace HP.ScalableTest.Plugin.IPSecurity
{
    /// <summary>
    /// Contains the templates for IPSecurity test cases
    /// </summary>
    public static class IPSecurityTemplates
    {
        public static PacketCaptureUtility _captureUtility = new PacketCaptureUtility();

        #region Local Variables

        private const string PRESHAREDKEY = "AutomationPSK";
        private const string ADDRESSTEMPLATENAME = "AddressTemplate-";
        private const string SERVICENAME = "Service-";
        private const string SERVICETEMPLATENAME = "ServiceTemplate-";
        private const string IPSECTEMPLATENAME = "IPSecTemplate-{0}";
        private const string CLIENT_RULE_NAME = "ClientRule-{0}";
        private const string IDCERTIFICATE_PSWD_SET1 = "xyzzy";
        private const string IDCERTIFICATE_PSWD_SET2 = "xyzzy";
        private const string IDCERTIFICATE__PSWD_INVALID = "xyzzy";
        private const string KERBEROS_PSWD = "1iso*help";
        private const string KERBEROS_USER_AES256 = "aes256sha1";
        private const string KERBEROS_USER_AES128 = "aes128sha1";
        private const string KERBEROS_DOMAIN = "KBS2008.COM";
        private const string QUICK_SET_NAME = "CTC-Automation";
        private const string SHARE_FILE_PATH = @"C:\Share\";

        private static string CACERTIFICATE_SET1 = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\IPSecuritySet1Certificates\CA_certificate.cer");
        private static string IDCERTIFICATE_SET1 = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\IPSecuritySet1Certificates\ID_certificate.pfx");
        private static string CACERTIFICATE_SET2 = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\IPSecuritySet2Certificates\CA_certificateset2.cer");
        private static string IDCERTIFICATE_SET2 = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\IPSecuritySet2Certificates\ID_certificateset2.pfx");
        private static string CACERTIFICATE_INVALID = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\IPSecurityInvalidCertificates\CA_certificate.cer");
        private static string IDCERTIFICATE_INVALID = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Certificates\IPSecurityInvalidCertificates\ID_certificate.pfx");
        private static string KERBEROS_CONFIGFILE = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Kerberos\{0}\krb5.conf");
        private static string KERBEROS_DES = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Kerberos\desmd5.keytab");
        private static string KERBEROS_AES128 = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Kerberos\aes128sha1.keytab");
        private static string KERBEROS_AES256 = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\Kerberos\aes256sha1.keytab");
        private static string PRINTFILEPATH = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\PrintFiles\test.doc");
        private static string PRINTLARGEFILEPATH = Path.Combine(CtcSettings.ConnectivityShare, @"ConnectivityShare\PrintFiles\large.doc");

        #endregion

        #region Enum

        public enum CertificateInstall
        {
            ValidSet1,
            ValidSet2,
            Invalid
        }

        #endregion

        #region Web UI

        /// <summary>
        /// Verify enable rule functionality
        ///Step 1: Verify Enable rule functionality
        ///1. Create a rule with All Address, All Services, IPsec with PSK on the printer.
        ///2. Set the default rule to Drop.
        ///3. Enable the IPsec policy on the printer.
        ///4. Create a similar rule on the client.
        ///5. Printer should be able to accessible from client via ping.

        ///Step 2: Verify Disable rule functionality
        ///1. Disable the rule which is created in the above step.
        ///2. Printer will not be able to accessible from client. 
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        /// Note: Changed the template WRT Deviation Report dated 7/10/2016
        public static bool VerifyEnableRuleFunctionality(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Verify Enable rule functionality"));

                // Get the basic rule settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo);

                // create rule with basic settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Drop				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Creating Rule in Client
                CtcUtility.CreateIPsecRule(settings, true);

                TraceFactory.Logger.Info("Validating the printer accessibility from Primary Client");
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                {
                    return false;
                }
                TraceFactory.Logger.Info("Since the rule is enabled in printer and client, able to access the Printer from Primary CLient");

                TraceFactory.Logger.Debug("Disabling Rule in Client Machine to connect the Secondary Client from Primary");
                CtcUtility.DisableIPsecRule(CLIENT_RULE_NAME.FormatWith(testNo));
                CtcUtility.SetFirewallPublicProfile(false);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                TraceFactory.Logger.Info("Validating the access from Secondary Client");
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
                if (connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Info("Even after the rule is not enabled in secondary client, still the printer is pinging");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Since the rule is not enabled in Secondary Client, printer fails to ping");
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Verify Disable rule functionality"));

                EwsWrapper.Instance().DisableAllRules();
                EwsWrapper.Instance().SetIPsecFirewall(false);
                CtcUtility.EnableIPsecRule(CLIENT_RULE_NAME.FormatWith(testNo));
                CtcUtility.SetFirewallPublicProfile(true);
                Thread.Sleep(TimeSpan.FromSeconds(10));

                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Fail))
                {
                    TraceFactory.Logger.Info("Even after the rule is disabled in Printer and enabled in primary client, still the printer is pinging");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Since the rule is disabled in Printer and enabled in Primary Client, printer fails to ping");
                }

                TraceFactory.Logger.Debug("Disabling Rule in Client Machine to connect the Secondary Client from Primary");
                CtcUtility.DeleteAllIPsecRules();
                CtcUtility.SetFirewallPublicProfile(false);
                Thread.Sleep(TimeSpan.FromSeconds(40));

                TraceFactory.Logger.Info("Validating the accessibility of the Printer from Secondary Client");
                if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Info("Even after the rule is disabled in printer, still the printer fails to ping in secondary client");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Since the rule is disabled in printer, printer successfully pings in secondary client");
                }
                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, true);
            }
        }

        /// <summary>
        /// Verify enable rule functionality
        ///Step 1: Verify Enable rule functionality
        ///1. Create a rule with All Address, All Services, IPsec with PSK on the printer.
        ///2. Set the default rule to Drop.
        ///3. Enable the IPsec policy on the printer.
        ///4. Create a similar rule on the client.
        ///5. Printer should be able to accessible from client via ping.

        ///Step 2: Verify Disable rule functionality
        ///1. Disable the rule which is created in the above step.
        ///2. Printer will not be able to accessible from client. 
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        /// Note: Changed the template WRT Deviation Report dated 7/10/2016
        public static bool VerifyEnableDisableRuleFunctionality(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Verifying with Basic IPSec rule All IP Addresses, All Services with PSK, with default rule as drop"));
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(DefaultServiceTemplates.AllManagementServices);
                SecurityRuleSettings securityRule = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.HighInteroperabilityLowsecurity);
                // Creating rule with All Addresses, All Services and IPsec with PSK.
                EwsWrapper.Instance().CreateRule(securityRule, true);

                // Setting default rule action to Allow				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // Setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
                connectivityService.Channel.CreateIPsecRule(securityRule, true, false);

                // Enable Domain,Private and public firewall profiles and allow inbound firewall policy to communicate back to the primary client when firewall is enabled in secondary client
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);

                if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Info(" Not able to access the IPV4 address of the Printer from Secondary Client even after the rule is enabled on Printer and secondary client");
                    return false;
                }

                // Setting IPsec option to false
                EwsWrapper.Instance().SetIPsecFirewall(false);
                if (connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Info("Able to access the IPV4 address of the Printer from Secondary Client even after the rule is disabled on Printer and enabled on secondary client");
                    return false;
                }
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                {
                    return false;
                }
                // Setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);
                if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Info(" Not able to access the IPV4 address of the Printer from Secondary Client even after the rule is enabled on Printer and secondary client");
                    return false;
                }
                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData, secondaryCleanup: true);
            }
        }

        /// <summary>
        /// Verify IPSec Default “Allow” and "Drop" functionality
        ///Step 1: Verify default IPsec rule in Allow state
        ///1. Create a rule with All IP Address, All ManagementServices and IPsec with PSK.
        ///2. Set default IPsec rule to Allow
        ///3. Enable IPsec policy
        ///4. Create similar IPSec rule on client.
        ///5. Printer should be accessible from client with ipsec rule via management services like http or snmp.
        ///6. Printer should be accessible from client without ipsec rule via ping as default rule is allow.

        ///Step 2: Verify default IPsec rule in Drop state
        ///1. Set the default IPsec rule to Drop
        ///2. Printer should be accessible from client with ipsec rule via management services like http or snmp.
        ///3. Printer should not be accessible from client without ipsec rule via ping as default rule is drop.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        /// This test case has been Updated as per the input from the manual team
        /// Changes:1.All IP address- All Management Service-Preshared key .
        ////2. Drop Functionality needs to be validated from both client 1 and client2 
        //// Changed the Template wrt Deviation report provided by CTC Team dated:7/10/2016
        public static bool VerifyIpsecDefaultAllowDropFunctionality(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Verifying with Basic IPSec rule All IP Addresses, All Management Services with PSK, with default rule as allow"));
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(DefaultServiceTemplates.AllManagementServices);
                SecurityRuleSettings securityRule = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.LowInteroperabilityHighsecurity);
                // Creating rule with All Addresses, All Management Services and IPsec with PSK.
                EwsWrapper.Instance().CreateRule(securityRule, true);

                // Setting default rule action to Allow				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);

                // Setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Creating Rule in Client
                CtcUtility.CreateIPsecRule(securityRule, true);

                TraceFactory.Logger.Info("Validating the http and functionality from Primary Client");
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, http: DeviceServiceState.Pass, ping: DeviceServiceState.Pass))
                {
                    return false;
                }

                TraceFactory.Logger.Debug("Disabling Rule in Client Machine to connect the Secondary Client from Primary");
                CtcUtility.DisableIPsecRule(CLIENT_RULE_NAME.FormatWith(testNo));
                CtcUtility.SetFirewallPublicProfile(false);
                Thread.Sleep(TimeSpan.FromSeconds(10));

                TraceFactory.Logger.Info("Validating the accessibility from Secondary Client");
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
                // Accessibility should pass from secondary client since default rule is allow.
                if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep(" Step II: Verify default IPsec rule in Drop state"));
                // setting default rule action to Drop				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // Creating Rule in Client
                CtcUtility.CreateIPsecRule(securityRule, true);

                TraceFactory.Logger.Info("Validating the http and ping functionality from Primary Client");
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, http: DeviceServiceState.Pass, ping: DeviceServiceState.Fail))
                {
                    return false;
                }

                TraceFactory.Logger.Debug("Disabling Rule in Client Machine to connect the Secondary Client from Primary");
                CtcUtility.DisableIPsecRule(CLIENT_RULE_NAME.FormatWith(testNo));
                CtcUtility.SetFirewallPublicProfile(false);
                Thread.Sleep(TimeSpan.FromSeconds(10));

                TraceFactory.Logger.Info("Validating the accessibility from Secondary Client");
                // Accessibility should fail from secondary client since default rule is drop.
                if (connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData, true);
            }
        }

        /// <summary>
        /// Verify IPSec with rule with custom IPSec template
        /// Step 1: Create an IPSec rule with All IP Addresses, All Services, Custom IPSec Template with PSK. 
        ///1. Create a rule with All IP Address, Services and Custom IPsec template with PSK.
        ///2. Set default IPsec rule to Drop.
        ///3. Enable IPsec policy.
        ///4. Create similar IPSec rule on client.
        ///5. Printer should be accessible from client with ipsec rule via http over IPv6.
        ///6. Printer should be able to process print jobs over IPv6.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyIpsecWithCustomIpsecTemplate(IPSecurityActivityData activityData, int testNo)
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


            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            string ipv6StatefullAddress = printer.IPv6StateFullAddress.ToString();
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Create an IPSec rule with All IP Addresses, All Services, Custom IPSec Template with PSK. "));

                AddressTemplateSettings addressTemplate = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);
                IKEPhase1Settings phase1 = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.SHA1, 28800);
                IKEPhase2Settings phase2 = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, 2800, 600000);

                // Get the Custom Service Template settings rule
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.Custom, phase1, phase2);

                // create rule with Custom Service Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Allow
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);
                // Creating Rule in Client
                CtcUtility.CreateIPsecRule(settings, true);

                TraceFactory.Logger.Info("Validating the http over IPv6 and print jobs over IPv6 from Primary Client");
                if (!CtcUtility.ValidateDeviceServices(printer, ipv6StatefullAddress, isMessageBoxChecked: activityData.MessageBoxCheckBox, http: DeviceServiceState.Pass, lpd: DeviceServiceState.Pass))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Verify default “Allow” and "Drop" functionality
        ///Step 1: Verify default IPsec rule in Allow state
        ///1. Create a rule with All Address, All ManagementServices and IPsec with PSK.
        ///2. Set default IPsec rule to Allow
        ///3. Enable IPsec policy
        ///4. Printer should be able to accessible via all services except FTP from the client even though it doesn’t have rule on the client side.

        ///Step 2: Verify default IPsec rule in Drop state
        ///1. Set the default IPsec rule to Drop
        ///2. Printer should not be able to accessible via Telnet, SNMP, Http, Ping, Ftp, Print 9100 from the client. 
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        /// This test case has been Updated as per the input from the manual team
        /// Changes:1.All IP address- All Management Service-Preshared key .
        ////2. Drop Functionality needs to be validated from both client 1 and client2 
        //// Changed the Template wrt Deviation report provided by CTC Team dated:7/10/2016
        public static bool VerifyAllowDropFunctionality(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep(" Step I: Verify default IPsec rule in Allow state"));

                TraceFactory.Logger.Info("Creating rule with All Address, All Management Services, IPsec with PSK in the printer");

                // Get the basic rule settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, service: DefaultServiceTemplates.AllManagementServices);

                // create rule with basic settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Allow				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                CtcUtility.CreateIPsecRule(settings, true);

                TraceFactory.Logger.Info("Validating the telnet/http functionality from Primary Client");
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, telnet: DeviceServiceState.Fail, http: DeviceServiceState.Fail, ping: DeviceServiceState.Pass))
                {
                    return false;
                }

                TraceFactory.Logger.Debug("Disabling Rule in Client Machine to connect the Secondary Client from Primary");
                CtcUtility.DisableIPsecRule(CLIENT_RULE_NAME.FormatWith(testNo));
                CtcUtility.SetFirewallPublicProfile(false);
                Thread.Sleep(TimeSpan.FromSeconds(10));

                TraceFactory.Logger.Info("Validating the accessibility from Secondary Client");
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
                if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())
                {
                    if (connectivityService.Channel.IsTelnetAccessible(IPAddress.Parse(activityData.WiredIPv4Address), (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily)))
                    {
                        TraceFactory.Logger.Info("Even after the rule is not enabled in secondary client, still the printer is able to access by telnet");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Since the rule is not enabled in Secondary Client, printer fails to connect through telnet");
                    }
                }
                else
                {
                    if (connectivityService.Channel.IsSnmpAccessible(IPAddress.Parse(activityData.WiredIPv4Address), PrinterFamilies.InkJet))
                    {
                        TraceFactory.Logger.Info("Even after the rule is not enabled in secondary client, still the printer is able to access by snmp");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Since the rule is not enabled in Secondary Client, printer fails to connect through snmp");
                    }
                }


                TraceFactory.Logger.Info(CtcUtility.WriteStep(" Step II: Verify default IPsec rule in Drop state"));

                // setting default rule action to Drop				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                CtcUtility.EnableIPsecRule(CLIENT_RULE_NAME.FormatWith(testNo));
                CtcUtility.SetFirewallPublicProfile(true);
                Thread.Sleep(TimeSpan.FromSeconds(10));

                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, telnet: DeviceServiceState.Pass, http: DeviceServiceState.Pass, ping: DeviceServiceState.Fail))
                {
                    TraceFactory.Logger.Info("Even after the default rule is dropped in Printer, still the printer is accessible through ping in Primary Client");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Since the rule is dropped in Printer-Primary Client, printer fails to connect through ping");
                }

                EwsWrapper.Instance().DeleteAllRules();

                TraceFactory.Logger.Debug("Disabling Rule in Client Machine to connect the Secondary Client from Primary");
                CtcUtility.DeleteAllIPsecRules();
                CtcUtility.SetFirewallPublicProfile(false);
                Thread.Sleep(TimeSpan.FromSeconds(10));

                TraceFactory.Logger.Info("Validating the Printer Accessibilty in Secondary Client-Telnet");
                if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())
                {
                    if (!connectivityService.Channel.IsTelnetAccessible(IPAddress.Parse(activityData.WiredIPv4Address), (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily)))
                    {
                        TraceFactory.Logger.Info("As there are no IPSec rules on the printer and secondary client, telnet should be accessible");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("As there are no IPSec rules on the printer and secondary client, Printer connects through telnet in Secondary Client");
                    }
                }
                else
                {
                    if (!connectivityService.Channel.IsSnmpAccessible(IPAddress.Parse(activityData.WiredIPv4Address), PrinterFamilies.InkJet))
                    {
                        TraceFactory.Logger.Info("As there are no IPSec rules on the printer and secondary client, snmp should be accessible");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("As there are no IPSec rules on the printer and secondary client, Printer connects through snmp in Secondary Client");
                    }
                }
                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData, true);
            }
        }

        /// <summary>
        /// Verify that  IP SA and IKE information is printed on security debug page
        ///Create rule on the printer with All Addresses, All Services, IKEv1 with High Security option.
        ///Create similar rule in Client Machine
        ///Print the security configuration page from the control panel Configuration Device -> EIO Jet Direct Menu -> Information -> Print Security Page. Use the control panel automation of DAT libraries for this.
        ///Write a note on the logs saying that the tester need to verify those settings on the printed page. 
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyIPSecurityDebugPage(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            IDevice device = DeviceFactory.Create(activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Get the basic rule settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo);

                // create rule with basic settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Drop				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Creating Rule in Client Machine
                CtcUtility.CreateIPsecRule(settings, true);

                TraceFactory.Logger.Info("From printer front panel printing the security debug page,please collect the same and validate the IPsec Configuration Information");

                return PrintSecurityDebugPageFrontPanel(device, activityData);
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Verify Enable and disable policy functionality using Front Panel
        ///Step 1: Enabling IPsec rule from Front Panel.
        ///1. Create a rule with All Addresses, All Services and IPsec with PSK High Security.
        ///2. Set the Default rule action to “Drop”
        ///3. Enable IPsec policy on the printer from Front Panel [ Administration -> Network Settings -> Embedded Jet direct Menu -> Security -> Select IPSEC -> select enable and save the changes and come back to home screen.]
        ///4. Create a similar rule on the client.
        ///5. User should not be able to enable IPSec policy using printer front panel. Enabling IPSec is limited only to WebUI
        ///6. Enable IPSec policy from WebUI of the Device.
        ///7. Ensure that Jet direct is accessible from the Host for which the specific IPSec rules were created.

        ///Step 2: Disabling IPsec rule from Front Panel.
        ///1. Disable the IPsec policy on the printer from Front Panel [ Administration -> Network Settings -> Embedded Jet direct Menu -> Security -> Select IPSEC -> select disable and save the changes and come back to home screen.]
        ///2. User should be able to disable IPSec policy using printer Front Panel.	
        ///3. Printer should not be able to accessible via ping, SNMP, telnet. 
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyEnableDisablePolicyUsingFrontPanel(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            IDevice device = DeviceFactory.Create(activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Enabling IPsec rule from Front Panel"));

                // Get the basic rule settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo);

                // create rule with basic settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Drop				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true from Front Panel
                if (!EnableIPSecFrontPanel(device, true, activityData))
                {
                    return false;
                }

                // Creating Rule in Client Machine
                CtcUtility.CreateIPsecRule(settings, true);

                // Printer accessibility should Fail since enabling IPSec is limited only to WebUI
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, telnet: DeviceServiceState.Fail))
                {
                    return false;
                }

                // Disabling the rule in client to access the printer
                CtcUtility.DisableIPsecRule(CLIENT_RULE_NAME.FormatWith(testNo));

                // setting IPsec option to true from WebUI
                EwsWrapper.Instance().SetIPsecFirewall(true);

                CtcUtility.EnableIPsecRule(CLIENT_RULE_NAME.FormatWith(testNo));

                // Printer accessibility should Pass since rule is created on both printer and client
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, telnet: DeviceServiceState.Pass))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Disabling IPsec rule from Front Panel"));

                if (!EnableIPSecFrontPanel(device, false, activityData))
                {
                    return false;
                }

                // Printer accessibility should fail since IPsec policy is disabled in printer                
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, telnet: DeviceServiceState.Fail))
                {
                    TraceFactory.Logger.Info("Able to connect to printer, which is not expected when IPSec Policy is disabled on printer and IPSec policy is enabled on the host of the default Drop rule");
                    return false;
                }
                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Verify IPsec rule behavior after the reinitialize operation
        ///Step 1: Verifying the connection after “Reinitialize Now” operation.
        ///1. Create a rule with All Addresses, All Services and IPsec with PSK High Security.
        ///2. Set the Default rule action to “Drop”
        ///3. Enable IPsec policy on the printer
        ///4. Create a similar rule on the client.
        ///5. Click on “Reinitialize Now” on the configuration page.
        ///6. Printer should be able to accessible via ping, SNMP, telnet.

        ///Step 2: Verifying the connection after “Clear previous values and Reinitialize Now” operation.
        ///1. Click on “Clear Previous Values and Reinitialize Now” on the configuration page.
        ///2. Verify the configuration method it should be BOOTP.
        ///3. Printer should be able to accessible via ping, SNMP, telnet.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyIPSecRuleBehaviourAfterReinitialize(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Verifying the connection after Reinitialize Now operation"));

                // Get the basic rule settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo);

                // Create rule with basic settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // Setting default rule action to Drop				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // Setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // TODO: PC - Server : TBD

                // Creating Rule in Client
                CtcUtility.CreateIPsecRule(settings, true);

                // Clicking on reinitialize button
                if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())
                {
                    EwsWrapper.Instance().ReinitializeConfigPrecedence();
                }

                // Printer accessibility should pass since rule is created in printer and client
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                {
                    return false;
                }

                // Disable rule on client and validate on server

                // TODO: PC ValidateDhcpRebindPacket

                if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())
                {
                    TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Verifying the connection after Clear previous values and Reinitialize Now operation"));

                    // Click on “Clear Previous Values and Reinitialize Now” option
                    EwsWrapper.Instance().ClearAndReinitializeConfigPrecedence();

                    if (!EwsWrapper.Instance().ValidateConfigMethod(IPConfigMethod.BOOTP.ToString()))
                    {
                        // if not set to BOOTP, set the configuration method to BOOTP
                        EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.BOOTP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));
                    }

                    // Printer accessibility should pass
                    return CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass);

                    // TODO: PC - Server : bootp request, bootp reply
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Verify IPSec Advanced options – DHCPv4		
        ///1. Bring up the device with less lease time( ex : 2 minutes lease time)
        ///2. On Device WebUI IPSec main page, click on Advanced, and ensure that the "DHCPv4" option checked - i.e. DHCPv4 Multicast/Broadcast  traffic will be exempt from IPSec.
        ///3. From Device IPSec main page in the Web UI, create an IPSec rule: All IP Address, All Services and IPSec template using 
        ///   pre shared keys using default settings for IKE.(High Interoperability).Create a rule with all IP Addresses and Services with PSK as IPsec template.
        ///4. Ensure that Default IPSec policy is "Drop".
        ///5. Enable the IPsec policy on the printer.
        ///6. Try to FTP or Telnet to Jet direct from a host where IPSec policy is not enable. and browse to Jet direct WebUI using HTTPS://<deviceIP>
        ///7. Start ethereal network trace on the DHCP server  and  wait for 5 minutes
        ///8. Verify Device behavior while DHCPv4 renew.
        ///9. Reboot the device.
        ///10. ValidateVerify Device behavior after reboot
        ///Expected: 1. User should not be able to telnet or ftp to Jet direct. User should be able to access Jet direct WebUI using HTTPS when Failsafe option is enabled,
        /// 2. Device should get DHCPv4 address using rebinding packet with the broadcast address and ethereal trace should show DHCPv4 traffic when DHCPv4 traffic is exempt.
        /// 3. Device should get IP address using the DHCP process while rebooting.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyIPSecurityAdvancedOptions(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            DhcpApplicationServiceClient dhcpClient = DhcpApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
            string scopeAddress = string.Empty;
            string autoIPAddress = string.Empty;

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Setting less Lease time
                scopeAddress = dhcpClient.Channel.GetDhcpScopeIP(activityData.PrimaryDhcpServerIPAddress);
                dhcpClient.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, scopeAddress, 120);

                EwsWrapper.Instance().SetIPConfigMethod(IPConfigMethod.DHCP, expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));

                // Enable DHCPV4 option in security page
                EwsWrapper.Instance().SetSecurityDHCPV4(true);

                // Get the basic rule settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo);

                // Create rule with basic settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // Setting default rule action to Drop				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // Setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Fail, https: DeviceServiceState.Pass, telnet: DeviceServiceState.Fail))
                {
                    return false;
                }

                string guid = client.Channel.TestCapture(activityData.SessionId, testNo.ToString());

                TraceFactory.Logger.Info("Performing reinitialize,since the rule is not created in client, unable to perform power cycle[SNMP OID not able to retrieve]");

                EwsWrapper.Instance().ReinitializeConfigPrecedence();

                PacketDetails details = client.Channel.Stop(guid);

                TraceFactory.Logger.Info("Validating the Rebind Packets");

                if (!ValidateDhcpRebindPacket(activityData.PrimaryDhcpServerIPAddress, details.PacketsLocation, activityData.WiredIPv4Address))
                {
                    return false;
                }
                TraceFactory.Logger.Info("Successfully validated DHCP rebind packets");
                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // setting back the lease time
                dhcpClient.Channel.SetDhcpLeaseTime(activityData.PrimaryDhcpServerIPAddress, scopeAddress, 691200);

                //setting back the default configuration method
                EwsWrapper.Instance().SetDefaultIPConfigMethod(expectedIPAddress: IPAddress.Parse(activityData.WiredIPv4Address));

                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Verify different Encapsulation modes in the IPsec template in the edit mode
        /// Step 1: Verifying with Basic rule (All Addresses, All Services and IPsec with PSK High Security, Transport mode)
        /// 1. Create a rule with All Addresses, All Services and IPsec with PSK High Security, Transport mode.
        /// 2. Set the Default rule action to “Drop”
        /// 3. Enable IPsec policy on the printer
        /// 4. Create a similar rule on the client.
        /// 5. Printer should be able to accessible via ping, SNMP, telnet.

        ///Step 2: Changing IPsec template from Transport to Tunnel mode
        ///1. Edit the IPsec template with Tunnel Mode with custom settings.
        ///2. Drop all the rules on the client
        ///3. Create new rule on the client (with Tunnel mode) [TBD: Are there any difference on the client side from file or manual settings]
        ///4. Printer should be able to accessible via ping, SNMP, telnet. 
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyIPSecEncapsulationMode(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            // Retrieving the remote address
            string remoteAddress = NetworkUtil.GetLocalAddress(activityData.PrimaryDhcpServerIPAddress, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Verifying with Basic rule (All Addresses, All Services and IPsec with PSK High Security, Transport mode)"));

                // Setting default service and custom address templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, remoteAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                IKEPhase1Settings phase1Printer = new IKEPhase1Settings(DiffieHellmanGroups.DH14 | DiffieHellmanGroups.DH1 | DiffieHellmanGroups.DH2 | DiffieHellmanGroups.DH5 | DiffieHellmanGroups.DH14 | DiffieHellmanGroups.DH15 | DiffieHellmanGroups.DH16 |
                                                                 DiffieHellmanGroups.DH17 | DiffieHellmanGroups.DH18, Encryptions.AES128 | Encryptions.DES | Encryptions.DES3 | Encryptions.AES192 | Encryptions.AES256, Authentications.SHA1 |
                                                                 Authentications.MD5 | Authentications.SHA256 | Authentications.SHA384 | Authentications.SHA512, 28800);
                IKEPhase2Settings phase2Printer = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1 | Authentications.MD5 | Authentications.SHA256 | Authentications.SHA384 | Authentications.SHA512,
                                                                 Encryptions.AES128 | Encryptions.DES | Encryptions.DES3 | Encryptions.AES192 | Encryptions.AES256, 36000, 0, false);

                // Get the Pre shared Custom rule settings, by default the authentication type is set to transport mode while retrieving the settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.Custom, phase1Printer, phase2Printer);

                // Creating rule with All Addresses, All Services and IPsec with PSK High Security, Transport mode.
                EwsWrapper.Instance().CreateRule(settings, true);

                // Setting default rule action to Drop				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // Setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Get the Pre shared Custom rule settings with encapsulation type- Tunnel	
                IKEPhase1Settings phase1Client = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.SHA1, 28800);
                IKEPhase2Settings phase2Client = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.AES128, 36000, 0, false);

                settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.Custom, phase1Client, phase2Client);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    // Creating rule in Client
                    CtcUtility.CreateIPsecRule(settings, true);

                    // Accessibility should pass since rule is enabled in printer and same rule is created in client
                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Changing IPsec template from Transport to Tunnel mode"));

                // Dropping all rules in client
                CtcUtility.DeleteAllIPsecRules();

                // Get the Pre shared Custom rule settings with encapsulation type- Tunnel								
                phase2Printer = new IKEPhase2Settings(EncapsulationType.Tunnel, Authentications.SHA1 | Authentications.MD5 | Authentications.SHA256 | Authentications.SHA384 | Authentications.SHA512,
                                                                 Encryptions.AES128 | Encryptions.DES | Encryptions.DES3 | Encryptions.AES192 | Encryptions.AES256, 36000, 0, false);

                settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.Custom, phase1Printer, phase2Printer);

                EwsWrapper.Instance().EditIPSecTemplate(settings.IPsecTemplate, true);

                TraceFactory.Logger.Info("Creating rule in Client Machine with Tunnel Mode");

                phase2Client = new IKEPhase2Settings(EncapsulationType.Tunnel, Authentications.SHA1, Encryptions.AES128, 36000, 0, false);
                settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.Custom, phase1Client, phase2Client);

                try
                {
                    guid = Guid.Empty;
                    packetLocation = string.Empty;

                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CtcUtility.CreateIPsecRule(settings);

                    // Accessibility should pass since rule is enabled in printer and same rule is created in client with Tunnel Mode
                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                return ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily);
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Verify device behavior using deletion of Address, Service and IPSec templates
        ///Step: 1 Verify Deletion of Custom Addresses template 
        ///1. Create one rule (tester can choose any settings) with custom address template.
        ///2. Delete the custom address template, since it is part of the rule and it is enabled, it will not allow to delete the address template.
        ///3. Delete the rule(s).
        ///4. Delete the custom address template, it should be able to delete the newly created template.
        ///5. Delete all Rules.         

        /// Step 2: Verify Deletion of custom service template.
        ///1. Create one rule (tester can choose any settings) with custom service template.
        ///2. Delete the custom service template, since it is part of the rule and it is enabled, it will not allow to delete the service template.
        ///3. Delete the rule(s).
        ///4. Delete the custom address template, it should be able to delete the newly created template.
        ///5. Delete all Rules. 
        ///Expected:4. The selected custom service template should be deleted if it is not being used by an existing IPSec rule. If the template is being used by an existing IPSec rule, user should get a message that the custom service template cannot be deleted.

        ///Step 3: Verify Deletion of custom IPSec template.
        ///1. Create one rule (tester can choose any settings) with custom IPsec template.
        ///2. Delete the custom IPsec template, since it is part of the rule and it is enabled, it will not allow to delete the rule.
        ///3. Delete the rule(s).
        ///4. Delete the custom IPsec template, it should be able to delete the newly created template.
        ///5. Delete all Rules. 
        ///Expected:6.User should be able to delete a custom IPsec template  if not being used by any existing rule. If the custom IPsec template  is being used by a any rule, user should get an error message stating that the custom IPSec template cannot be deleted.

        ///Step 4: Verify Deletion of custom services
        ///1. Create one rule (tester can choose any settings) with custom service part of custom service template
        ///2. Delete the custom service created in the above step, since it is part of the custom service, it will not allow to delete the service.
        ///3. Delete all the rule(s).
        ///4. Delete the custom service which is create in the above step.
        ///5. Delete the custom service template, it should be able to delete the newly created template.
        ///6. Delete All Rules. 
        ///Expected: User should be able to delete a custom service port if not being used by a rule or custom service template. If the custom service port is being used by a service template, user should get an error message stating that the custom service port cannot be deleted.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyDeletionOfCustomAddressTemplate(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Verify Deletion of Custom Addresses template"));

                TraceFactory.Logger.Debug("Creating Custom Address Template Rule");

                // Retrieving the remote address
                string remoteAddress = NetworkUtil.GetLocalAddress(activityData.PrimaryDhcpServerIPAddress, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();

                // Setting default service and custom address templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, remoteAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                // Get the Custom Address Template settings rule
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                // create rule with Custom Address Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Allow
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Deleting Custom Address Template, delete should not happen since the rule still exists
                EwsWrapper.Instance().DeleteAddressTemplate(ADDRESSTEMPLATENAME + testNo);

                // Deleting the Rule without deleting the custom address template
                EwsWrapper.Instance().DeleteAllRules(false);

                // Deleting the custom address template after deleting the rule, delete should happen now since we already deleted the rule
                EwsWrapper.Instance().DeleteAddressTemplate(ADDRESSTEMPLATENAME + testNo);

                // Deleting all rules With Custom Templates before proceeding to next step
                EwsWrapper.Instance().DeleteAllRules();

                // Most of the controls in IPSec template are temp ids, so Stopping and starting
                if (activityData.ProductFamily == PrinterFamilies.InkJet.ToString())
                {
                    EwsWrapper.Instance().Adapter.Stop();
                    EwsWrapper.Instance().Adapter.Start();
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Verify Deletion of Custom service template."));

                TraceFactory.Logger.Debug("Creating Rule with Custom service template");

                // setting service
                Service defaultCustomService = new Service();
                defaultCustomService.IsDefault = true;
                defaultCustomService.Name = "Bonjour";
                defaultCustomService.Protocol = ServiceProtocolType.UDP;
                defaultCustomService.PrinterPort = "Any";
                defaultCustomService.ServiceType = ServiceType.Remote;
                defaultCustomService.RemotePort = "5353";

                Collection<Service> ipSecService = new Collection<Service>();
                ipSecService.Add(defaultCustomService);

                // Setting default Address and custom Service Templates
                addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                serviceTemplateSettings = new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo, ipSecService);

                IKEPhase1Settings phase1 = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.SHA1, 28800);
                IKEPhase2Settings phase2 = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, 2800, 600000);

                // Get the Custom Service Template settings rule
                settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.Custom, phase1, phase2);

                // create rule with Custom Service Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Allow
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Deleting Custom Service Template, delete should not happen since the rule still exists
                EwsWrapper.Instance().DeleteServiceTemplate(SERVICETEMPLATENAME + testNo);

                // Deleting the Rule without deleting the custom service template
                EwsWrapper.Instance().DeleteAllRules(false);

                // Deleting the custom service template after deleting the rule, delete should happen now since we already deleted the rule
                EwsWrapper.Instance().DeleteServiceTemplate(SERVICETEMPLATENAME + testNo);

                // Deleting all rules With Custom Templates before proceeding to next step
                EwsWrapper.Instance().DeleteAllRules();

                // Most of the controls in IPSec template are temp ids, so Stopping and starting
                if (activityData.ProductFamily == PrinterFamilies.InkJet.ToString())
                {
                    EwsWrapper.Instance().Adapter.Stop();
                    EwsWrapper.Instance().Adapter.Start();
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step III: Verify Deletion of Custom IPSec template"));

                // Setting default address and service templates
                addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                phase1 = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.SHA1, 28800);
                phase2 = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, 2800, 600000);

                // Get the Pre shared Custom rule settings, by default the authentication type is set to transport mode while retrieving the settings
                settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.Custom, phase1, phase2, defaultAction: DefaultAction.Allow);

                // Creating rule with All Addresses, All Services and IPsec with PSK High Security, Transport mode.
                EwsWrapper.Instance().CreateRule(settings, true);

                // Setting default rule action to Allow				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);

                // Setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Deleting Custom IPSec Template, delete should not happen since the rule still exists
                EwsWrapper.Instance().DeleteIPsecTemplate(IPSECTEMPLATENAME.FormatWith(testNo));

                // Deleting the Rule without deleting the custom service template
                EwsWrapper.Instance().DeleteAllRules(false);

                // Deleting the custom service template after deleting the rule, delete should happen now since we already deleted the rule
                EwsWrapper.Instance().DeleteIPsecTemplate(IPSECTEMPLATENAME.FormatWith(testNo));

                // Deleting all rules With Custom Templates before proceeding to next step
                EwsWrapper.Instance().DeleteAllRules();

                // Most of the controls in IPSec template are temp ids, so Stopping and starting
                if (activityData.ProductFamily == PrinterFamilies.InkJet.ToString())
                {
                    EwsWrapper.Instance().Adapter.Stop();
                    EwsWrapper.Instance().Adapter.Start();
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step IV: Verify Deletion of Custom Services"));

                Collection<Service> ipSecServiceCustomService = new Collection<Service>();
                TraceFactory.Logger.Debug("Creating Rule with Custom service part of custom service template");

                Service customService = new Service();
                customService.IsDefault = false;
                customService.Name = SERVICENAME + testNo;
                customService.Protocol = ServiceProtocolType.UDP;
                customService.PrinterPort = "";
                customService.ServiceType = ServiceType.Remote;
                customService.RemotePort = "";

                ipSecServiceCustomService.Add(customService);

				serviceTemplateSettings = new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo, ipSecServiceCustomService);
				addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);

                phase1 = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.SHA1, 28800);
                phase2 = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, 2800, 600000);

                // Get the Custom Service Template settings rule
                settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.Custom, phase1, phase2);

                // create rule with Custom Service Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Allow
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Deleting Custom Service Template, delete should not happen since the rule still exists
                EwsWrapper.Instance().DeleteCustomService(SERVICENAME + testNo);

                // Deleting the Rule without deleting the custom service template
                EwsWrapper.Instance().DeleteAllRules(false);

                // Deleting the custom service template after deleting the rule, delete should happen now since we already deleted the rule
                EwsWrapper.Instance().DeleteCustomService(SERVICENAME + testNo);

                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Verify edit functionality of the Address / Service / IPsec templates
        ///Step 1: Verify modification of custom address template
        ///1. Create a rule with specific IP host address (first client) with all the services and IPsec with PSK.
        ///2. Set the default IPsec rule to drop
        ///3. Enable the IPsec policy on the printer.
        ///4. Create a rule on the client with the same settings.
        ///5. Printer should be able to accessible ping.
        ///6. Edit the address template with secondary host (second client).
        ///7. First client will fail to access the printer.
        ///8. Create the rule on the second client.
        ///9. Second client should be able to access the printer.
        ///10. Delete all rules on the printer.
        ///11. Delete all rules on all the clients.

        ///Step 2: Verify modification of custom service template
        ///1. Create a rule with custom service with HTTPS and IPsec with PSK.
        ///2. Set the default IPsec rule to drop
        ///3. Enable the IPsec policy on the printer.
        ///4. Create a rule on the client with the same settings.
        ///5. Printer should be able to accessible via HTTPS.
        ///6. Edit the service template with Telnet.
        ///7. Client will fail to access the printer via HTTPS.
        ///8. Client will be able to access the printer via Telnet.
        ///9. Delete all rules on the printer.
        ///10. Delete all rules on all the clients.

        ///Step 3: Verify modification of custom IPsec template
        ///1. Create a rule with All Address, All Services and IPsec with PSK.
        ///2. Set the default IPsec rule to drop.
        ///3. Enable the IPsec policy on the printer.
        ///4. Crate a rule on the client with the same settings.
        ///5. Printer should be able to accessible via ping.
        ///6. Edit the IPsec template with different PSK.
        ///7. Client will fail to access the printer via ping.
        ///8. Delete all the rules on the client side.
        ///9. Create a rule on the client with new PSK.
        ///10. Printer should be able to accessible via ping. 
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyEditFunctionalityOfAddressServiceTemplates(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Verify modification of custom address template"));

                // Retrieving the remote address
                string remoteAddress = NetworkUtil.GetLocalAddress(activityData.PrimaryDhcpServerIPAddress, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();

                // Setting default service and custom address templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, remoteAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                // Get the Custom Address Template settings rule
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                // create rule with Custom Address Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Drop
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Create rule in client
                CtcUtility.CreateIPsecRule(settings, true);

                // Accessibility should pass since rule is enabled in printer and same rule is created in client
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Editing the address template with secondary client as remote address");

                CtcUtility.DeleteAllIPsecRules();

                addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, activityData.WindowsSecondaryClientIPAddress);
                settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                EwsWrapper.Instance().EditAddressTemplate(addressTemplateSettings, true);
                TraceFactory.Logger.Info("Pinging to IPv4 address from Primary client. Expected is Fail since rule Address template is changed");
                // Accessibility should fail since rule is updated with secondary client as remote address
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Fail))
                {
                    return false;
                }

                // Creating Rule in Secondary Client
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
                connectivityService.Channel.CreateIPsecRule(settings, true, false);

                // Enable Domain,Private and public firewall profiles and allow inbound firewall policy to communicate back to the primary client when firewall is enabled in secondary client
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);
                TraceFactory.Logger.Info("Pinging from Secodary client to Printer Ipv4. Expected Pass");
                // Accessibility should pass since rule is created in secondary client also
                if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    return false;
                }

                // Deleting all rules
                EwsWrapper.Instance().DeleteAllRules();
                TraceFactory.Logger.Info("Deleting Ipsec Rules from Secodary client");
                connectivityService.Channel.DeleteAllIPsecRules();

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Verify modification of custom service template"));

                // setting service
                Service defaultCustomService = new Service();
                defaultCustomService.IsDefault = true;
                defaultCustomService.Name = "TELNET";
                defaultCustomService.Protocol = ServiceProtocolType.TCP;
                defaultCustomService.PrinterPort = "23";
                defaultCustomService.ServiceType = ServiceType.Printer;
                defaultCustomService.RemotePort = "Any";

                Collection<Service> ipSecService = new Collection<Service>();
                ipSecService.Add(defaultCustomService);

                // Setting default Address and custom Service Templates
                addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                serviceTemplateSettings = new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo, ipSecService);

                // Get the Custom Service Template settings rule
                settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                // create rule with Custom Service Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Allow
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Creating rule in client with same setting
                CtcUtility.CreateIPsecRule(settings);

                // Accessibility should pass since rule is enabled in printer and same rule is created in client
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, telnet: DeviceServiceState.Pass))
                {
                    return false;
                }

                CtcUtility.DeleteAllIPsecRules();

                TraceFactory.Logger.Info("Editing the service template from Telnet to SNMP");

                defaultCustomService.Name = "SNMP";
                defaultCustomService.PrinterPort = "161";
                defaultCustomService.Protocol = ServiceProtocolType.UDP;
                defaultCustomService.ServiceType = ServiceType.Printer;
                defaultCustomService.RemotePort = "Any";

                Collection<Service> ipSecServiceEdit = new Collection<Service>();
                ipSecServiceEdit.Add(defaultCustomService);

                serviceTemplateSettings = new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo, ipSecServiceEdit);
                EwsWrapper.Instance().EditServiceTemplate(serviceTemplateSettings, true);

                // Get the Custom Service Template settings rule
                settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);
                settings.IPsecTemplate.Name = "IPSecTemplateSNMP";

                // Creating rule in client with same setting
                CtcUtility.CreateIPsecRule(settings);

                // Accessibility should fail to access the printer via Telnet
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, telnet: DeviceServiceState.Fail))
                {
                    return false;
                }

                // Accessibility should pass to access the printer via SNMP
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, snmp: DeviceServiceState.Pass))
                {
                    return false;
                }

                // Deleting all rules
                CtcUtility.DeleteAllIPsecRules();
                EwsWrapper.Instance().DeleteAllRules();

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step III: Verify modification of custom IPsec template"));

                // Setting default address and service templates
                addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                IKEPhase1Settings phase1 = new IKEPhase1Settings(DiffieHellmanGroups.DH14 | DiffieHellmanGroups.DH1 | DiffieHellmanGroups.DH2 | DiffieHellmanGroups.DH5 | DiffieHellmanGroups.DH14 | DiffieHellmanGroups.DH15 | DiffieHellmanGroups.DH16 |
                                                                 DiffieHellmanGroups.DH17 | DiffieHellmanGroups.DH18, Encryptions.AES128 | Encryptions.DES | Encryptions.DES3 | Encryptions.AES192 | Encryptions.AES256, Authentications.SHA1 |
                                                                 Authentications.MD5 | Authentications.SHA256 | Authentications.SHA384 | Authentications.SHA512, 28800);
                IKEPhase2Settings phase2 = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1 | Authentications.MD5 | Authentications.SHA256 | Authentications.SHA384 | Authentications.SHA512,
                                                                 Encryptions.AES128 | Encryptions.DES | Encryptions.DES3 | Encryptions.AES192 | Encryptions.AES256, 36000, 0, false);

                // Get the Pre shared Custom rule settings, by default the authentication type is set to transport mode while retrieving the settings
                settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.Custom, phase1, phase2);

                // create rule with basic settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Drop				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                IKEPhase1Settings phase1Client = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.SHA1, 28800);
                IKEPhase2Settings phase2Client = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.AES128, 36000, 0, false);

                settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.Custom, phase1Client, phase2Client);

                // Creating rule in Client
                CtcUtility.CreateIPsecRule(settings, true);

                // accessibility should pass since rule is enabled in printer and same rule is created on both client and printer
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Editing IPSec Template with different PreShared Key");

                CtcUtility.DeleteAllIPsecRules();

                // Get settings to edit PreShared key
                IKEv1Settings ikeV1Settings = new IKEv1Settings(PRESHAREDKEY + 1, phase1, phase2);

                DynamicKeysSettings dynamicKeysSettings = new DynamicKeysSettings(IKEVersion.IKEv1, IKESecurityStrengths.Custom, ikeV1Settings, null);
                IPsecTemplateSettings IPsecTemplateSettings = new IPsecTemplateSettings(IPSECTEMPLATENAME.FormatWith(testNo), SecurityKeyType.Dynamic, dynamicKeysSettings, null);

                EwsWrapper.Instance().EditIPSecTemplate(IPsecTemplateSettings, true);

                // Accessibility should fail to access the printer since template has been updated with new PreSharedKey
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Fail))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Creating new rule with new PreShared key in Client");

                ikeV1Settings = new IKEv1Settings(PRESHAREDKEY + 1, phase1Client, phase2Client);

                dynamicKeysSettings = new DynamicKeysSettings(IKEVersion.IKEv1, IKESecurityStrengths.Custom, ikeV1Settings, null);
                IPsecTemplateSettings = new IPsecTemplateSettings(IPSECTEMPLATENAME.FormatWith(testNo), SecurityKeyType.Dynamic, dynamicKeysSettings, null);

                SecurityRuleSettings securityRuleSettings = new SecurityRuleSettings(testNo.ToString(), addressTemplateSettings, serviceTemplateSettings,
                                                                                     IPsecFirewallAction.ProtectedWithIPsec, IPsecTemplateSettings, defaultAction: DefaultAction.Drop);
                CtcUtility.CreateIPsecRule(securityRuleSettings);

                // Accessibility should pass to access the printer since new rule has been created in client with new PreSharedKey
                return CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass);
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData, true);
            }
        }

        /// <summary>
        ///Verify modification of IPSec templates from one certificates to another certificates.
        /// Step 1: Verifying with Basic rule (All Addresses, All Services and IPsec with Certificates)
        /// 1. Create a rule with All Addresses, All Services and IPsec with Certificates.
        /// 2. Set the Default rule action to “Drop”
        /// 3. Enable IPsec policy on the printer
        /// 4. Create a similar rule on the client.
        /// 5. Printer should be able to accessible via ping, SNMP, telnet.

        ///Step 2: Changing IPsec template from one Certificate set to another certificate set.
        ///1. Install different set of certificates (valid) on the printer. (TBD: Do we need to disable the rule before doing this?)
        ///2. Drop all the rules on the client
        ///3. Create new rule on the client (with different set of certificates)
        ///4. Printer should be able to accessible via ping, SNMP, telnet. 
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyModificationOfIPSecurityTemplatesFromCertificates(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Once rule is created, share folder can't be accessed. Copying required certificates 
                string idCertificatePath = Path.Combine(Path.GetTempPath(), "ID_certificateset2.pfx");
                string caCertificatePath = Path.Combine(Path.GetTempPath(), "CA_certificateset2.cer");

                File.Copy(IDCERTIFICATE_SET2, idCertificatePath, true);
                File.Copy(CACERTIFICATE_SET2, caCertificatePath, true);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Creating basic rule with Certificates"));

                // Setting default service and custom address templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                // Get the settings with certificates
                SecurityRuleSettings settings = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, install: CertificateInstall.ValidSet1);

                // create rule with Custom Address Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Drop				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                TraceFactory.Logger.Info("Creating similar IPsecrule in Client Machine");

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CtcUtility.CreateIPsecRule(settings, true);

                    // Accessibility should pass since rule is enabled in printer and same rule is created in client
                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString()))
                {
                    return false;
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Changing IPsec template from one Certificate set to another certificate set"));

                // installing certificates in printer
                EwsWrapper.Instance().InstallCACertificate(caCertificatePath, validate: false);
                EwsWrapper.Instance().InstallIDCertificate(idCertificatePath, IDCERTIFICATE_PSWD_SET2, validate: false);

                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Fail))
                {
                    return false;
                }

                // Dropping all rules in client machine
                CtcUtility.DeleteAllIPsecRules();

                TraceFactory.Logger.Info("Deleting CA and ID certificate on client machine.");

                CtcUtility.DeleteCACertificate(CACERTIFICATE_SET1);
                CtcUtility.DeleteIDCertificate(IDCERTIFICATE_SET1, IDCERTIFICATE_PSWD_SET1);

                // Creating new rule with different set of certificates
                TraceFactory.Logger.Info("Creating new rule on the client with different set of certificates");

                // Get the settings with second set of certificates
                settings = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, install: CertificateInstall.ValidSet2);

                try
                {
                    guid = Guid.Empty;
                    packetLocation = string.Empty;
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CtcUtility.CreateIPsecRule(settings);
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                {
                    return false;
                }

                return ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString());
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData, deleteCertificates: true);
            }
        }

        /// <summary>
        /// Modification of Kerberos rule from Conf file to Manual
        ///Step 1: Verifying with Basic rule (All Addresses, All Services and IPsec with Kerberos from Configuration file)
        ///1. Create a rule with All Addresses, All Services and IPsec with Kerberos from Configuration file.
        ///2. Validate the Kerberos Connection while creating the rule.

        ///Step 2: Changing IPsec template from Configuration file to Manual settings
        ///1. Edit the IPsec template with Manual settings of Kerberos.
        ///2. Validate the Kerberos Connection while creating the rule.
        ///Note: As discussed with Manual team, since the test case is WebUI related creating rule in client is not required,so we are validating the Kerberos only from the printer end
        /// </summary>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyModificationOfIPSecurityTemplatesFromKerberos(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Creating rule with Specific IP address, All services using kerberos with Conf file"));

                // Setting default service and custom address templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, activityData.WindowsSecondaryClientIPAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                KerberosImportSettings importSettings = new KerberosImportSettings(KERBEROS_CONFIGFILE.FormatWith(activityData.ProductFamily), KERBEROS_AES256);
                KerberosSettings kerberos = new KerberosSettings(importSettings);

                // Get the settings with Kerberos                
                SecurityRuleSettings settings = GetKerberosRuleSettings(testNo, kerberos, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.HighInteroperabilityLowsecurity);

                // If Kerberos Connection is not success, this method will throw an exception and returns false
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Allow
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                TraceFactory.Logger.Info("Creating Rule in Secondary Client with Kerberos Conf Settings");

                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
                connectivityService.Channel.CreateIPsecRule(settings, enableRule: true, enableProfiles: false);
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);

                TraceFactory.Logger.Info("Validating the accessibility of the Printer in Secondary Client for Kerberos");
                // Printer accessibility should pass from secondary client since rule is created in printer and client
                try
                {
                    if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                    {
                        TraceFactory.Logger.Info("Failed to access the printer from Secondary Client");
                        return false;
                    }
                }
                catch
                {
                    TraceFactory.Logger.Info("Failed to access the printer from Secondary Client");
                    return false;
                }
                TraceFactory.Logger.Info("Since the rule is created in Secondary Client, able to access the printer");

                TraceFactory.Logger.Info("Disabling the Rule in Printer and in Secondary Client");
                connectivityService.Channel.DeleteAllIPsecRules();

                // Rebooting secondary client machine                 
                connectivityService.Channel.RebootMachine(IPAddress.Parse(activityData.WindowsSecondaryClientIPAddress));
                Thread.Sleep(TimeSpan.FromMinutes(2));

                EwsWrapper.Instance().SetIPsecFirewall(false);
                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Changing IPsec template from Configuration file to Manual settings"));

                KerberosManualSettings manualKerberosSettings = new KerberosManualSettings(activityData.KerberosServerIPAddress, KERBEROS_USER_AES256 + '@' + KERBEROS_DOMAIN, KERBEROS_PSWD, KerberosEncryptionType.AES256SHA1);

                kerberos = new KerberosSettings(manualKerberosSettings);
                IKEv1Settings ikeV1Settings = new IKEv1Settings(kerberos);

                DynamicKeysSettings dynamicKeysSettings = new DynamicKeysSettings(IKESecurityStrengths.LowInteroperabilityHighsecurity, ikeV1Settings);
                IPsecTemplateSettings IPsecTemplateSettings = new IPsecTemplateSettings(IPSECTEMPLATENAME.FormatWith(testNo), SecurityKeyType.Dynamic, dynamicKeysSettings, null);

                // Editing IPSecTemplate from Configuration to Manual settings,if Kerberos Connection is not success, this method will throw an exception and returns false
                EwsWrapper.Instance().EditIPSecTemplate(IPsecTemplateSettings, true);

                settings.IPsecTemplate = IPsecTemplateSettings;
                EwsWrapper.Instance().SetIPsecFirewall(true);

                connectivityService.Channel.CreateIPsecRule(settings, enableRule: true, enableProfiles: false);
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);

                if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Info("Failed to access the Printer even after the modified rule is updated in secondary client");
                    return false;
                }

                TraceFactory.Logger.Info("Since the modified Template rule is created in Secondary Client, able to access the printer");

                // Rebooting secondary client machine to clear the kerberos settings                 
                connectivityService.Channel.RebootMachine(IPAddress.Parse(activityData.WindowsSecondaryClientIPAddress));
                Thread.Sleep(TimeSpan.FromMinutes(1));

                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData, true);
            }
        }

        /// <summary>
        /// Authentication Options on the IPsec template, Verify different Identity Authentication options in the IPsec template in the edit mode.
        ///Step 1: Creating IPsec template with Certificates
        ///1. Create IPsec template with certificates
        ///2. Validate the Kerberos Connection while creating the rule.

        ///Step 2: Changing IPsec template from Certificates to Kerberos
        ///1. Edit the IPsec template with Kerberos options
        ///2. Validate the Kerberos Connection while creating the rule.
        /// </summary>
        ///Note: As discussed with Manual team, since the test case is WebUI related creating rule in client is not required,so we are validating the Kerberos only from the printer end
        /// <returns>Returns true if the rule is created successfully else returns false</returns>
        public static bool VerifyIdenityAuthenticationOptions(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Creating IPsec template with Preshared Key"));

                // Setting default service and custom address templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                // Get the settings with preshared
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.HighInteroperabilityLowsecurity);

                EwsWrapper.Instance().CreateRule(settings, true);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Creating Rule in Client
                CtcUtility.CreateIPsecRule(settings, true);

                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                {
                    return false;
                }

                CtcUtility.DeleteAllIPsecRules();
                EwsWrapper.Instance().SetIPsecFirewall(false);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Creating IPsec template with Certificates"));

                // Get the settings with certificates
               settings = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, install: CertificateInstall.ValidSet1);

                // Disable FIPs on the Printer, if it is enabled Printer does not allow to install ID certificate 
               if (!EwsWrapper.Instance().SetFipsOption(false))
               {
                    return false;
               }

                if (!EwsWrapper.Instance().CheckCertificateInstallation(settings))
                {
                    TraceFactory.Logger.Info("Failed to install certificates before creating IPsec rule.");
                    return false;
                }

                EwsWrapper.Instance().EditIPSecTemplate(settings.IPsecTemplate, true);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Creating Rule in Client
                CtcUtility.CreateIPsecRule(settings, true);

                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validation after Editing Preshared to Certificate: Success, able to access the printer with Certificate Rule enabled");
                CtcUtility.DeleteAllIPsecRules();
                EwsWrapper.Instance().SetIPsecFirewall(false);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step III: Changing IPsec template from Certificates to Kerberos"));

                addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + 1 + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, activityData.WindowsSecondaryClientIPAddress);
                KerberosImportSettings importSettings = new KerberosImportSettings(KERBEROS_CONFIGFILE.FormatWith(activityData.ProductFamily), KERBEROS_AES256);
                KerberosSettings kerberos = new KerberosSettings(importSettings);

                settings = GetKerberosRuleSettings(testNo, kerberos, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.LowInteroperabilityHighsecurity);

                // Editing IPSecTemplate from Configuration to Manual settings, if Kerberos Connection is not success, this method will throw an exception and returns false
                EwsWrapper.Instance().EditIPSecTemplate(settings.IPsecTemplate, true);

                settings.IPsecTemplate.Name = "KerberosTemplate";
                EwsWrapper.Instance().CreateRule(settings, true);

                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Creating Rule in Client
                CtcUtility.CreateIPsecRule(settings, true);

                TraceFactory.Logger.Info("Validating the Accessibility of the Printer in Secondary Client");
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);

                TraceFactory.Logger.Debug("Rebooting secondary client machine");
                TraceFactory.Logger.Info("Secondary client IP : {0}".FormatWith(activityData.WindowsSecondaryClientIPAddress));
              //  MessageBox.Show("Check Kerberos machine is rebooting ");
                connectivityService.Channel.RebootMachine(IPAddress.Parse(activityData.WindowsSecondaryClientIPAddress));
               // TraceFactory.Logger.Debug("Waiting for secondary client to reboot and come to ready state.");
                Thread.Sleep(TimeSpan.FromMinutes(2));
                if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WindowsSecondaryClientIPAddress)))
                {
                    TraceFactory.Logger.Info("Client is not pinging");
                }
            
                connectivityService.Channel.CreateIPsecRule(settings, enableRule: true, enableProfiles: false);
                TraceFactory.Logger.Info("Check IPsec rule created in client: Rule Enabled , Profile False");
                // Enable Domain,Private and public firewall profiles and allow inbound firewall policy to communicate back to the primary client when firewall is enabled in secondary client
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);
                TraceFactory.Logger.Info("Enabling Firewall in Sec Client: Enabled");
                // Since the Kerberos is taking long time to establish connection, given delay of 2 min
                Thread.Sleep(TimeSpan.FromMinutes(2));

                if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Info("Failed to access the Printer in Secondary Client, even after enabling the Kereberos Rule");
                    return false;
                }

                TraceFactory.Logger.Info("Validation after Editing Certificate to Kerberos: Success, able to access the printer from secondary client with Kerberos Rule enabled");
                return true;
            }
            catch (Exception ipSecException)
            {
                TraceFactory.Logger.Info("Exception Occured :{0}".FormatWith(ipSecException.Message));
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData, true, deleteCertificates: true);
            }
        }

        /// <summary>
        ///Verify check firewall behaviour with powercycle
        /// Step 1: Verifying with Basic rule with powercycle (All IPv4 Addresses, All Print Services and Firewall Action Allow )
        /// 1. Create a rule with All IPv4 Addresses, All Print Services and Firewall Action Allow.
        /// 2. Set the Default rule action to “Drop”
        /// 3. Enable IPsec policy on the printer
        /// 4. Printer should be able to accessible via any printing protocols like P9100/LPD
        /// 5. Powercycle the printer
        /// 6. Printer should come to ready state
        /// 7. Printer should be able to accessible via any printing protocols like P9100/LPD

        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyFirewallBehaviourWithPowercycle(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I:fVerifying with Basic rule with powercycle (All IPv4 Addresses, All Print Services and Firewall Action Allow )"));

                // Setting default service and address templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPv4Addresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplateSettings;
                securitySettings.ServiceTemplate = serviceTemplateSettings;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;

                EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                TraceFactory.Logger.Info("Validating IPsec rule with P9100 print.");

                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, p9100: DeviceServiceState.Pass, isPingRequiredP9100Install: false, printerDriver: activityData.PrintDriverLocation, printerDriverModel: activityData.PrintDriverModel))
                {
                    return false;
                }

                printer.PowerCycle();

                TraceFactory.Logger.Info("Validating the rule with P9100 print after the printer powercycle.");
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, p9100: DeviceServiceState.Pass, isPingRequiredP9100Install: false, printerDriver: activityData.PrintDriverLocation, printerDriverModel: activityData.PrintDriverModel))
                {
                    return false;
                }

                return true;
            }

            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }

            finally
            {
                TestPostRequisites(activityData);
            }

        }

        #endregion

        #region Dynamic Keys

        /// <summary>
        /// Verify printer accessibility without rule on both printer and client
        /// Step 1: No IPsec rule on the Printer and Client, Printer Default IPsec rule action is Drop.
        /// 1.	Delete all IPsec rules on the client.
        /// 2.	Delete all IPsec rules on the printer.
        /// 3.	Set the Default rule action to “Drop” on the printer. 
        /// 4.	Enable IPsec Policy on the printer.
        /// 5.	From client Ping, Telnet, FTP and HTTP accessibility will fail and printer is only accessible with HTTPS since FailSafe option is enabled.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyHttpsBehaviorWithoutRule(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Verifying Printer accessibility when no IPsec rule on Printer and Client, Printer Default IPsec rule action is Drop."));

                // delete all rules on the client
                CtcUtility.DeleteAllIPsecRules();

                // delete all rules on the printer
                EwsWrapper.Instance().DeleteAllRules();

                // set the default action to "Drop"
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // enable IPsec policy on the printer
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Ping, Telnet, FTP, HTTP accessibility should fail since default action is drop and since fail safe option is enabled HTTPS will work.
                return CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Fail, telnet: DeviceServiceState.Fail, ftp: DeviceServiceState.Fail, http: DeviceServiceState.Fail, https: DeviceServiceState.Pass);
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // IPsecFirewall option will be disabled when rules are deleted. Disable IPsec/Firewall option manually since it was enabled with no rules created.
                EwsWrapper.Instance().SetIPsecFirewall(false);
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Step 1: Checking Printer accessibility when the client IPsec rule is in Enable state
        /// 1.	Create a rule on printer with All Addresses, All Services and IPsec with PSK High Security.
        /// 2.	Set the Default rule action to “Drop”.
        /// 3.	Enable IPsec policy on the printer.
        /// 4.	Create a similar rule on the client.
        /// 5.	Ping should work from the client.
        /// 
        /// Step 2: Checking Printer accessibility when the Client IPsec rule Disable state
        /// 1.	Disable the rule on the first client.
        /// 2.	Create the similar rule on the second client and mark it as disable state.
        /// 3.	Ping the printer from second client, it should fail.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyRuleWithEnableAndDisableState(IPSecurityActivityData activityData, DefaultAddressTemplates defaultAddresssTemplate, DefaultServiceTemplates defaultServiceTemplate, IKESecurityStrengths securityStrength, int testNo)
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

            bool isCertifcate = false;
            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            string ipv6StatefullAddress = activityData.IPV6StatefullAddress;
            string ipv6StatelessAddress = activityData.IPV6StatelessAddress;
            string ipv6LinkLocalAddress = activityData.LinkLocalAddress;
            TraceFactory.Logger.Info("Check Printer IPV6 addresses are available by pinging them.");
            // In some cases all IPV6 addresses are not accessible due to infra related issues, so validating the same
            if (!printer.PingUntilTimeout(IPAddress.Parse(ipv6StatefullAddress), TimeSpan.FromSeconds(20)) &&
			   !printer.PingUntilTimeout(IPAddress.Parse(ipv6LinkLocalAddress), TimeSpan.FromSeconds(20))  &&
			   !printer.PingUntilTimeout(IPAddress.Parse(ipv6StatelessAddress), TimeSpan.FromSeconds(20)))
			{
                if(activityData.ProductFamily != PrinterFamilies.InkJet.ToString())  // Need to remove once the setting network protocol issue is resolved
                {
                    EwsWrapper.Instance().SetIPv6(false);
                    EwsWrapper.Instance().SetIPv6(true);
                }

				if (!printer.PingUntilTimeout(IPAddress.Parse(ipv6StatefullAddress), TimeSpan.FromSeconds(20)) &&
				   !printer.PingUntilTimeout(IPAddress.Parse(ipv6LinkLocalAddress), TimeSpan.FromSeconds(20)) &&
				   !printer.PingUntilTimeout(IPAddress.Parse(ipv6StatelessAddress), TimeSpan.FromSeconds(20)))
				{
					TraceFactory.Logger.Info("IPv6 address is not accessible");
					return false;
				}
			}
            //Disable FIPS on the Printer: if FIPS is enabled Installation of ID certificate fails
            if (!EwsWrapper.Instance().SetFipsOption(false))
            {
                return false;
            }
            string ipv6Address = ipv6StatefullAddress;
            string linkLocalAddress = ipv6LinkLocalAddress;

            Service defaultCustomService = new Service();
            defaultCustomService.IsDefault = true;
            defaultCustomService.Name = "P9100";
            defaultCustomService.Protocol = ServiceProtocolType.TCP;
            defaultCustomService.PrinterPort = "9100";
            defaultCustomService.ServiceType = ServiceType.Printer;
            defaultCustomService.RemotePort = "Any";

            Service defaultCustomServiceSNMP = new Service();
            defaultCustomServiceSNMP.IsDefault = true;
            defaultCustomServiceSNMP.Name = "SNMP";
            defaultCustomServiceSNMP.Protocol = ServiceProtocolType.UDP;
            defaultCustomServiceSNMP.PrinterPort = "161";
            defaultCustomServiceSNMP.ServiceType = ServiceType.Printer;
            defaultCustomServiceSNMP.RemotePort = "Any";

            Service defaultCustomServiceSNMP162 = new Service();
            defaultCustomServiceSNMP162.IsDefault = true;
            defaultCustomServiceSNMP162.Name = "SNMP";
            defaultCustomServiceSNMP162.Protocol = ServiceProtocolType.TCP;
            defaultCustomServiceSNMP162.PrinterPort = "161 - 162";
            defaultCustomServiceSNMP162.ServiceType = ServiceType.Printer;
            defaultCustomServiceSNMP162.RemotePort = "Any";

            Collection<Service> ipSecServiceP9100 = new Collection<Service>();
            ipSecServiceP9100.Add(defaultCustomService);
            ipSecServiceP9100.Add(defaultCustomServiceSNMP);
            if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())
            {
                ipSecServiceP9100.Add(defaultCustomServiceSNMP162);
            }

            Collection<Service> ipSecServiceSNMP = new Collection<Service>();
            ipSecServiceSNMP.Add(defaultCustomServiceSNMP);
            if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())
            {
                ipSecServiceSNMP.Add(defaultCustomServiceSNMP162);
            }


            AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(defaultAddresssTemplate);
            ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(defaultServiceTemplate);
            ServiceTemplateSettings serviceTempalteSNMP = new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo + "SNMP", ipSecServiceSNMP);
            ServiceTemplateSettings serviceTempalteP9100 = new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo + "P9100", ipSecServiceP9100);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step I: Checking Printer accessibility with primary client when IPsec rule is in Enable state"));
                SecurityRuleSettings settings = new SecurityRuleSettings();

                if (defaultAddresssTemplate.Equals(DefaultAddressTemplates.AllIPv6Addresses) ||
                    defaultAddresssTemplate.Equals(DefaultAddressTemplates.AllIPv4Addresses))
                {
                    settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, securityStrength);
                }
                else if (defaultAddresssTemplate.Equals(DefaultAddressTemplates.AllIPv6NonLinkLocal))
                {
                    settings = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTempalteP9100, securityStrength);
                    isCertifcate = true;

                }
                else
                {
                    settings = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, securityStrength);
                    isCertifcate = true;
                }

                // create rule with basic settings
                EwsWrapper.Instance().CreateRule(settings, true);

                SecurityRuleSettings settingsSNMPPreShare = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTempalteSNMP, securityStrength);
                SecurityRuleSettings settingsSNMPCertificate = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTempalteSNMP, securityStrength);

				if (defaultAddresssTemplate.Equals(DefaultAddressTemplates.AllIPv6Addresses))
				{
					settingsSNMPPreShare.IPsecTemplate.Name = "IPSECSNMP";
                    if ((ProductFamilies.LFP.ToString() == activityData.ProductFamily))
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                    }
					EwsWrapper.Instance().CreateRule(settingsSNMPPreShare, true);
				}

                // set the default action to "Drop"
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // Enable firewall option
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Manually on client siderule is not created with specific IPV6NonLink Local Address hence on client we are creating allipv6 address
                if (defaultAddresssTemplate.Equals(DefaultAddressTemplates.AllIPv6NonLinkLocal))
                {
                    settings.AddressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPAddresses;
                }
                CtcUtility.CreateIPsecRule(settings);

                if (defaultAddresssTemplate.Equals(DefaultAddressTemplates.AllIPv6Addresses))
                {
                    // create similar rule on the client for SNMP
                    CtcUtility.CreateIPsecRule(settingsSNMPPreShare);
                }

                if (defaultAddresssTemplate.Equals(DefaultAddressTemplates.AllIPv6NonLinkLocal) || defaultAddresssTemplate.Equals(DefaultAddressTemplates.AllIPv6Addresses))
                {
                    TraceFactory.Logger.Info("Validating P9100 Service with Statefull IPV6 Address: {0}".FormatWith(ipv6Address));

                    if (!CtcUtility.ValidateDeviceServices(printer, ipv6Address, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Fail, p9100: DeviceServiceState.Pass, isPingRequiredP9100Install: false,
                                                        printerDriver: activityData.PrintDriverLocation, printerDriverModel: activityData.PrintDriverModel))
                    {
                        TraceFactory.Logger.Info("Even though rule is created for with P9100 service, fail to access printer through P9100");
                        return false;
                    }
                }
                else if (defaultAddresssTemplate.Equals(DefaultAddressTemplates.AllIPv6LinkLocal))
                {
                    // Since printer is not established connection with link local for first time, disabling and enabling again
                    CtcUtility.DisableIPsecRule(CLIENT_RULE_NAME.FormatWith(testNo));
                    EwsWrapper.Instance().DisableAllRules();

                    EwsWrapper.Instance().EnableAllRules();
                    CtcUtility.EnableIPsecRule(CLIENT_RULE_NAME.FormatWith(testNo));

                    TraceFactory.Logger.Info("Validating the Link Local Address Accessibility : {0}".FormatWith(linkLocalAddress));

                    if (!CtcUtility.ValidateDeviceServices(printer, linkLocalAddress, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        TraceFactory.Logger.Info("Even though rule is created with all service, fail to access printer");
                        return false;
                    }
                }
                else
                {
                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        TraceFactory.Logger.Info("Even though rule is created with all service, fail to access printer");
                        return false;
                    }
                }

                TraceFactory.Logger.Info(CtcUtility.WriteStep("Step II: Checking Printer accessibility with secondary client when the IPsec rule is in Disable state"));

                EwsWrapper.Instance().SetIPsecFirewall(false);

                // Disabling ipsec rule on client machine to establish communication to secondary client.				
                CtcUtility.DeleteAllIPsecRules();
                CtcUtility.SetFirewallPublicProfile(false);
                Thread.Sleep(TimeSpan.FromSeconds(10));

                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);

                TraceFactory.Logger.Info("Performing Ping from Secondary client:");

                // since the rule is in disabled state, ping will fail
                if (!connectivityService.Channel.Ping(IPAddress.Parse(ipv6Address)))
                {
                    TraceFactory.Logger.Info("Ping to Printer:{0} is Unsuccessful in Secondary Client even after disabling rule in Printer".FormatWith(ipv6Address));
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Ping to Printer:{0} is Successful in Secondary Client since the rule is disabled in the printer".FormatWith(ipv6Address));
                    return true;
                }

            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, deleteCertificates: isCertifcate);
            }
        }

        /// <summary>
        /// 1.	Create a rule on printer with All Addresses, All Services and IPsec with following details.
        ///     a.	IKEv1 with Custom Strengths
        ///     b.	Authentication type – certificates
        ///     c.	Phase I – Any settings will do
        ///     d.	Phase II - Encapsulation type: Transport, Any Encryption and/or 1 of the category of Authentication (Either ESP/AH), Advanced IKE Settings: Enable Replay Detection
        /// 2.	Set the Default rule action to “Drop”
        /// 3.	Enable IPsec policy on the printer
        /// 4.	Create a similar rule on the client.
        /// 5.	Ping the printer, it should work from the client.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">83045</param>
        /// <returns>true if the test case passes, false otherwise</returns>
        public static bool VerifyRuleWithReplayDetection(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Build Rule with all settings
                IKEPhase1Settings phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.SHA1, 28800);
                AdvancedIKESettings advancedSettings = new AdvancedIKESettings(true, false, DiffieHellmanGroups.DH14);
                IKEPhase2Settings phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.AES128, 36000, 0, false);
                phase2Settings.AdvancedIKESettings = advancedSettings;

                SecurityRuleSettings securityRule = GetCertificatesRuleSettings(testNo, strength: IKESecurityStrengths.Custom, phase1Settings: phase1Settings, phase2Settings: phase2Settings);

                // Create rule on printer, set default action to Drop and enable the rule
                EwsWrapper.Instance().CreateRule(securityRule, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    if (!CtcUtility.CreateIPsecRule(securityRule))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info("Validating IPsec rule with Ping.");

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                return ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily);
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, deleteCertificates: true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, deleteCertificates: true);
            }
        }

        /// <summary>
        /// 1. Create a rule on printer with All Addresses, All Services and IPsec with following details.
        ///     a.	IKEv1 with Custom Strengths
        ///     b.	Authentication type – certificates
        ///     c.	Phase I – Any settings will do
        ///     d.	Phase II - Encapsulation type: Transport. Enable AH and disable ESP. Select any 1 authentication option in AH.
        /// 2.   Set the Default rule action to “Drop”
        /// 3.	Enable IPsec policy on the printer
        /// 4.	Create a similar rule on the client.
        /// 5.	Ping the printer, it should work from the client.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">83041</param>
        /// <returns>true if the test case passes, false otherwise</returns>
        public static bool VerifyRuleWithOnlyAH(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Build Rule with all settings
                IKEPhase1Settings phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.SHA1, 28800);
                IKEPhase2Settings phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, 36000, 0);
                phase2Settings.AdvancedIKESettings = null;

                SecurityRuleSettings securityRule = GetCertificatesRuleSettings(testNo, strength: IKESecurityStrengths.Custom, phase1Settings: phase1Settings, phase2Settings: phase2Settings);

                // Create rule on printer, set default action to Drop and enable the rule
                EwsWrapper.Instance().CreateRule(securityRule, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    if (!CtcUtility.CreateIPsecRule(securityRule))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info("Validating IPsec rule with Ping.");

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                return ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily);
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, deleteCertificates: true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, deleteCertificates: true);
            }
        }

        /// <summary>
        /// 1. Create a rule on printer with All Addresses, All Services and IPsec with following details.
        ///     a. IKEv1 with Custom Strengths
        ///     b. Authentication type – certificates
        ///     c. Phase I – Any settings will do
        ///     d. Phase II - Encapsulation type: Transport, Enable only ESP and disable AH. Select any 1 authentication and 1 encryption in ESP. (Don’t select any Authentication in AH) 
        /// 2.	 Set the Default rule action to “Drop”
        /// 3.	Enable IPsec policy on the printer
        /// 4.	Create a similar rule on the client.
        /// 5.	Ping the printer, it should work from the client.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">83040</param>
        /// <returns>true if the test case passes, false otherwise</returns>
        public static bool VerifyRuleWithOnlyESP(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Build Rule with all settings
                IKEPhase1Settings phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.SHA1, 28800);
                IKEPhase2Settings phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.AES128, 36000, 0, false);
                phase2Settings.AdvancedIKESettings = null;

                SecurityRuleSettings securityRule = GetCertificatesRuleSettings(testNo, strength: IKESecurityStrengths.Custom, phase1Settings: phase1Settings, phase2Settings: phase2Settings);

                // Create rule on printer, set default action to Drop and enable the rule
                EwsWrapper.Instance().CreateRule(securityRule, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    if (!CtcUtility.CreateIPsecRule(securityRule))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info("Validating IPsec rule with Ping.");

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                return ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily);

            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, deleteCertificates: true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, deleteCertificates: true);
            }
        }

        /// <summary>
        /// 1. Create a rule on printer with All Addresses, All Services and IPsec with IKEv1, Low Security. Authentication- Certificates.
        /// 2. Set the Default rule action to “Drop”
        /// 3. Enable IPsec policy on the printer
        /// 4. Create a similar rule on the client.
        /// 5. Ping the printer, it should work from the client.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">83027</param>
        /// <returns>true if the test case passes, false otherwise</returns>
        public static bool VerfiyRuleWithSha1Certificates(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPv4Addresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllManagementServices);
                // Build Rule with all settings
                SecurityRuleSettings securityRule = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, strength: IKESecurityStrengths.LowInteroperabilityHighsecurity);

                // Create rule on printer, set default action to Drop and enable the rule
                EwsWrapper.Instance().CreateRule(securityRule, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    if (!CtcUtility.CreateIPsecRule(securityRule))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info("Validating the accessibility of the Printer IPV4 address from Primary Client");

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, telnet: DeviceServiceState.Pass, http: DeviceServiceState.Pass))
                    {
                        return false;
                    }

                    ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);

                    TraceFactory.Logger.Info("Validating the accessibility of the Printer IPV4 Address from Secondary client:");

                    // since the rule is in disabled state, ping will fail
                    if (connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                    {
                        TraceFactory.Logger.Info("Able to access the IPV4 address of the Printer from Secondary Client even after the rule is enabled in Printer");
                        return false;
                    }
                    TraceFactory.Logger.Info("Failed to access the IPV4 address of the Printer from Secondary Client since there is no rule enabled");
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                return ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily);
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, deleteCertificates: true);
            }
        }

        /// <summary>
        /// 1. Create a rule on printer with All Addresses, All Services and IPsec with IKEv1, Low Security. Authentication- Certificates. (Use expired certificates)
        /// 2. Set the Default rule action to “Drop”
        /// 3. Enable IPsec policy on the printer
        /// 4. Create a similar rule on the client.
        /// 5. Ping the printer, it should NOT work from the client.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">92155</param>
        /// <returns>true if the test case passes, false otherwise</returns>
        public static bool VerifyRuleWithExpriedCertificates(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Build Rule with all settings
                SecurityRuleSettings securityRule = GetCertificatesRuleSettings(testNo, strength: IKESecurityStrengths.LowInteroperabilityHighsecurity, install: CertificateInstall.Invalid);

                // Create rule on printer, set default action to Drop and enable the rule
                EwsWrapper.Instance().CreateRule(securityRule, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                if (!CtcUtility.CreateIPsecRule(securityRule))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validating IPsec rule with Ping.");

                return CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Fail);
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, deleteCertificates: true);
            }
        }

        /// <summary>
        /// 1.	All IP Addresses, All Services and IPsec with following details:
        ///     a.	IKEv1, Custom strength
        ///     b.	Authentication – Certificates
        ///     c.	Phase I settings: Any settings will do
        ///     d.	Phase II settings: Encapsulation type- transport, Enable ESP and disable AH. Select any 1 authentication and 1 Encryption from ESP. Set SA Lifetime to 60000KB.
        /// 2.	Create similar rule on client machine.
        /// 3.	Ping the printer, it should work from client.
        /// 4.	Start Packet capture on local client machine.
        /// 5.	Send print jobs which exceeds 60000KB data.
        /// 6.	After print jobs are successful, (wait for extra 1 minute to be safe) stop packet capture.
        /// 7.	Validate packets: 
        ///     a.	Filter – <TBD>
        ///     b.	Data to be searched - <TBD>
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">83038</param>
        /// <returns>true if the test case passes, false otherwise</returns>
        public static bool VerifyReAuthenticationWithSALifeSize(IPSecurityActivityData activityData, int testNo)
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

			Guid guid = Guid.Empty;
			string packetLocation = string.Empty;
			Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            string ipv6StatefullAddress = activityData.IPV6StatefullAddress;

            if (!printer.PingUntilTimeout(IPAddress.Parse(ipv6StatefullAddress), TimeSpan.FromSeconds(20)))
            {
                if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())  // Need to remove once the setting network protocol issue is resolved
                {
                    EwsWrapper.Instance().SetDHCPv6OnStartup(true);
                    EwsWrapper.Instance().SetIPv6(false);
                    EwsWrapper.Instance().SetIPv6(true);
                }

                if (!printer.PingUntilTimeout(IPAddress.Parse(ipv6StatefullAddress), TimeSpan.FromSeconds(20)))
                {
                    TraceFactory.Logger.Info("IPv6 address is not accessible");
                    return false;
                }
            }
			 //SET FIPS to Disable since if FIPS is enabled ID certificate installation fails 

            if (!EwsWrapper.Instance().SetFipsOption(false))
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPv6Addresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);
                // Build Rule with all settings
                IKEPhase1Settings phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.SHA1, 2400);
                IKEPhase2Settings phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.AES128, 1200, 60000, false);
                phase2Settings.AdvancedIKESettings = null;

                SecurityRuleSettings securityRule = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, strength: IKESecurityStrengths.Custom, phase1Settings: phase1Settings, phase2Settings: phase2Settings);

                // Create rule on printer, set default action to Drop and enable the rule
                EwsWrapper.Instance().CreateRule(securityRule, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    if (!CtcUtility.CreateIPsecRule(securityRule))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info("Validating the accessibility of IPV6 address");

                    if (string.IsNullOrEmpty(ipv6StatefullAddress))
                    {
                        TraceFactory.Logger.Info("Statefull address is null or empty");
                        return false;
                    }
                    if (!CtcUtility.ValidateDeviceServices(printer, ipv6StatefullAddress, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info("Validating the Printer IPV4 accessibility which will fail since the rule is created with IPV6 address Template");
                    if (!CtcUtility.ValidateDeviceServices(printer, activityData.WiredIPv4Address, ping: DeviceServiceState.Fail))
                    {
                        return false;
                    }
                    TraceFactory.Logger.Debug("Printing large file after setting SA Life time:{0} from Quick Mode to validate reauthentication from Path:{1}".FormatWith("60000KB", PRINTLARGEFILEPATH));
                    //changed this earlier print job was sent over ip4 but ipv4 will not be active since rule is enabled. so sending print job over link local
                    printer.Install(IPAddress.Parse(activityData.LinkLocalAddress), Printer.Printer.PrintProtocol.RAW, activityData.PrintDriverLocation, activityData.PrintDriverModel);
                    printer.Print(PRINTLARGEFILEPATH, TimeSpan.FromMinutes(5));
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }
                TraceFactory.Logger.Info("Waiting for Reauthentication to happen");

                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily))
                {
                    TraceFactory.Logger.Info("Fails to Reauthenticate after setting SA Life Time:{0}".FormatWith("60000 KB"));
                    return false;
                }
                TraceFactory.Logger.Info("Successfully Reauthenticated after setting SA Life Time:{0} in Quick Mode".FormatWith("60000 KB"));
                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, deleteCertificates: true);
                return false;
            }
            finally
            {
                TraceFactory.Logger.Info("Successfully Reauthenticated");
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, deleteCertificates: true);
            }
        }

        /// <summary>
        /// Step 1: Checking Printer re-authentication with IKE Phase I SA Lifetime settings.
        ///     1.  All IP Addresses, All Services and IPsec with following details:
        ///         a.   IKEv1, Custom strength
        ///         b.   Authentication – Certificates
        ///         c.	Phase I settings: DH 14 group, Any 1 Encryption and 1 Authentication method, SA Lifetime to 600 seconds.
        ///         d.	Phase II settings: Encapsulation type- transport, Enable ESP and disable AH. Select any 1 authentication and 1 encryption from ESP. Leave other settings to default.
        ///     2.	Create similar rule on client machine.
        ///     3.	Ping the printer, it should work from client.
        ///     4.	Start Packet capture on local client machine.
        ///     5.	Wait for 600 seconds + additional 1 minute and stop packet capture.
        ///     6.	Validate packets: 
        ///         a.	Filter – <TBD>
        ///         b.	Data to be searched – <TBD>
        /// Step 2: Delete all rules on printer and client machine.
        /// Step 3: Checking Printer re-authentication with IKE Phase II SA Lifetime settings.
        ///     1.  All IP Addresses, All Services and IPsec with following details:
        ///         a.   IKEv1, Custom strength
        ///         b.   Authentication – Certificates
        ///         c.	Phase I settings: DH 14 group, Any 1 Encryption and 1 Authentication method, SA Lifetime to 800 seconds.
        ///         d.	Phase II settings: Encapsulation type- transport, Enable ESP and disable AH. Select any 1 authentication and 1 encryption from ESP. SA Lifetime to 600 seconds
        ///     2.   Create similar rule on client machine.
        ///     3.   Ping the printer, it should work from client.
        ///     4.   Start Packet capture on local client machine.
        ///     5.   Wait for 600 seconds + additional 1 minute and stop packet capture.
        ///     6.   Validate packets: 
        ///         a.	Filter – <TBD>
        ///         b.	Data to be searched - <TBD>
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">83037</param>
        /// <returns>true if the test case passes, false otherwise</returns>
        public static bool VerifyReAuthenticationWithSALifetime(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            string ipv6StatefullAddress = activityData.IPV6StatefullAddress;
            if (!printer.PingUntilTimeout(IPAddress.Parse(ipv6StatefullAddress), TimeSpan.FromSeconds(20)))
            {
                if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())  // Need to remove once the setting network protocol issue is resolved
                {
                    EwsWrapper.Instance().SetIPv6(false);
                    EwsWrapper.Instance().SetIPv6(true);
                }

                if (!printer.PingUntilTimeout(IPAddress.Parse(ipv6StatefullAddress), TimeSpan.FromSeconds(20)))
                {
                    TraceFactory.Logger.Info("IPv6 address is not accessible");
                    return false;
                }
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                CtcUtility.WriteStep("Step 1: Checking Printer re-authentication with SA Lifetime settings.");

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPv4Addresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                // Build Rule with all settings
                IKEPhase1Settings phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.SHA1, 600);
                IKEPhase2Settings phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.AES128, 3600, 0, false);
                phase2Settings.AdvancedIKESettings = null;

                SecurityRuleSettings securityRule = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, strength: IKESecurityStrengths.Custom, phase1Settings: phase1Settings, phase2Settings: phase2Settings);

                // Create rule on printer, set default action to Drop and enable the rule
                EwsWrapper.Instance().CreateRule(securityRule, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    if (!CtcUtility.CreateIPsecRule(securityRule))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info("Validating the accessibility of Ipv4 address");

                    if (!CtcUtility.ValidateDeviceServices(printer, activityData.WiredIPv4Address, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info("Validating the Printer IPV6 accessibility which will fail since the rule is created with IPV4 address Template");
                    if (!CtcUtility.ValidateDeviceServices(printer, printer.IPv6StateFullAddress.ToString(), ping: DeviceServiceState.Fail))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info("Time Spent to set SA Life Time: {0} in Main Mode".FormatWith("600 Seconds"));
                    TraceFactory.Logger.Info("Waiting for 12 min for ReAuthentication to happen");
                    Thread.Sleep(TimeSpan.FromMinutes(12));
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                TraceFactory.Logger.Info("Validating the Packets for Reauthentication");
                return ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily);

            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, deleteCertificates: true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, deleteCertificates: true);
            }
        }

        /// <summary>
        /// 1.  All IP Addresses, All Services and IPsec with following details:
        ///     a.   IKEv1, Custom strength
        ///     b.   Authentication – Certificates
        ///     c.   Phase I settings: DH 14 group, Any 1 Encryption and 1 Authentication method, SA Lifetime to 600 seconds.
        ///     d. Phase II settings: Encapsulation type- transport, Enable ESP and disable AH. Select any 1 authentication and 1 encryption from ESP. Set SA Lifetime to 600 seconds and 60000KB for time and data fields respectively.
        /// 2.	Create similar rule on client machine.
        /// 3.   Ping the printer, it should work from client.
        /// 4.   Start Packet capture on local client machine.
        /// 5.   Send print jobs which exceeds 60000KB data.
        /// 6.   Wait for additional 1 minute and stop packet capture.
        /// 7.   Validate packets: 
        ///     a. Filter – <TBD>
        ///     b. Data to be searched – <TBD>
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">83039</param>
        /// <returns>true if the test case passes, false otherwise</returns>
        public static bool VerifyReAuthenticationWithSALifeSizeandSALifetime(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPv4Addresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                // Build Rule with all settings
                IKEPhase1Settings phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.SHA1, 1200);
                IKEPhase2Settings phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.AES128, 600, 60000, false);
                phase2Settings.AdvancedIKESettings = null;

                SecurityRuleSettings securityRule = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, strength: IKESecurityStrengths.Custom, phase1Settings: phase1Settings, phase2Settings: phase2Settings);

                // Create rule on printer, set default action to Drop and enable the rule
                EwsWrapper.Instance().CreateRule(securityRule, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);
                    if (!CtcUtility.CreateIPsecRule(securityRule))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info("Validating the Printer IPV4 accessibility");
                    if (!CtcUtility.ValidateDeviceServices(printer, activityData.WiredIPv4Address, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }

					TraceFactory.Logger.Info("Validating the Printer IPV6 accessibility which will fail since the rule is created with IPV4 address Template");
					if (!CtcUtility.ValidateDeviceServices(printer, activityData.IPV6StatefullAddress, ping: DeviceServiceState.Fail))
					{
						return false;
					}
				}
				finally
				{
					packetLocation = StopPacketCapture(guid);
				}
				TraceFactory.Logger.Info("Values of SALifeTime: {0} and SALifeSize:{1} which has been set in the Printer while creating rule".FormatWith("600 Seconds", "60000kb"));
				TraceFactory.Logger.Info("Printing large file");
				printer.Install(IPAddress.Parse(activityData.WiredIPv4Address), Printer.Printer.PrintProtocol.RAW, activityData.PrintDriverLocation, activityData.PrintDriverModel);
				printer.Print(PRINTLARGEFILEPATH, TimeSpan.FromMinutes(5));

                TraceFactory.Logger.Info("Validating the Reauthentication by the captured packets");
                return ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily);

            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, deleteCertificates: true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, deleteCertificates: true);
            }
        }

        /// <summary>
        /// 1.	Create a rule on printer with All Addresses, All Services and IPsec with IKEv1, Low Security. Authentication- PSK. 
        /// 2.	Set the Default rule action to “Drop” 
        /// 3.	Enable IPsec policy on the printer
        /// 4.	Create client rule with different PSK value
        /// 5.	Ping the printer, it should NOT work.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">92154</param>
        /// <returns>true if the test case passes, false otherwise</returns>
        public static bool VerifyRuleWithMismatchPSK(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPv4Addresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                // Build Rule with all settings
                SecurityRuleSettings securityRule = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                // Create rule on printer, set default action to Drop and enable the rule
                EwsWrapper.Instance().CreateRule(securityRule, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                securityRule.IPsecTemplate.DynamicKeysSettings.V1Settings.PSKValue = "Invalid";

                if (!CtcUtility.CreateIPsecRule(securityRule))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validating IPsec rule with Ping.");

                return CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Fail);
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// 1.	Create a rule on printer with All Addresses, All Services and IPsec with IKEv1, High Security, and Authentication- Certificates.
        /// 2.	Set the default rule action to “Drop”
        /// 3.	Enable IPsec on printer
        /// 4.	Create similar rule on client machine
        /// 5.	Ping the printer, it should work
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">83029</param>
        /// <returns>true if the test case passes, false otherwise</returns>
        public static bool VerifyRuleWithCertificates(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Build Rule with all settings
                SecurityRuleSettings securityRule = GetCertificatesRuleSettings(testNo, strength: IKESecurityStrengths.HighInteroperabilityLowsecurity);

                // Create rule on printer, set default action to Drop and enable the rule
                EwsWrapper.Instance().CreateRule(securityRule, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    if (!CtcUtility.CreateIPsecRule(securityRule))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info("Validating the Printer Accessibility");
                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                return ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily);
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, deleteCertificates: true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, deleteCertificates: true);
            }
        }

        /// <summary>
        /// 1.	Create a rule on printer with All IP Addresses, All Services and following IPsec settings:
        ///     a.	IKEv1, Custom strength
        ///     b.	Phase I settings: Any settings
        ///     c.	Phase II settings: Encapsulation – tunnel, Enable ESP and disable AH. Select 1 encryption and 1 authentication method from ESP. leave other settings to default.
        /// 2.	Set the Default rule action to “Drop”
        /// 3.	Enable IPsec policy on the printer
        /// 4.	Create similar rule on client machine
        /// 5.	Ping to printer, it should work
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">83044</param>
        /// <returns>true if the test case passes, false otherwise</returns>
        public static bool VerifyRuleWithTunnel(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string tunnelRemoteAddress = NetworkUtil.GetLocalAddress(activityData.PrimaryDhcpServerIPAddress, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();
                string tunnelLocalAddress = activityData.WiredIPv4Address;

                // Build Rule with all settings
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, tunnelLocalAddress, tunnelRemoteAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                IKEPhase1Settings phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.SHA1, 28800);
                IKEPhase2Settings phase2Settings = new IKEPhase2Settings(EncapsulationType.Tunnel, Authentications.SHA1, Encryptions.AES128, 36000, 0, false, tunnelLocalAddress, tunnelRemoteAddress);
                phase2Settings.AdvancedIKESettings = null;

                SecurityRuleSettings securityRule = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, strength: IKESecurityStrengths.Custom, phase1Settings: phase1Settings, phase2Settings: phase2Settings);

                // Create rule on printer, set default action to Drop and enable the rule
                EwsWrapper.Instance().CreateRule(securityRule, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    if (!CtcUtility.CreateIPsecRule(securityRule))
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info("Validating IPsec rule with Ping.");

                    if (!CtcUtility.ValidateDeviceServices(printer, ping: DeviceServiceState.Pass))
                    {
                        TraceFactory.Logger.Info("Failed to access Printer from Primary Client");
                        return false;
                    }
                    TraceFactory.Logger.Info("Printer is accessible from Primary Client");

                    TraceFactory.Logger.Debug("Disabling the rule in Client to access Secondary Client");
                    CtcUtility.DeleteAllIPsecRules();
                    CtcUtility.SetFirewallPublicProfile(false);
                    Thread.Sleep(TimeSpan.FromSeconds(10));

                    TraceFactory.Logger.Info("Validating the ipaddress accessibility from Secondary Client");
                    ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
                    if (connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                    {
                        TraceFactory.Logger.Info("Even after there is no rule exists in Secondary Client, still the Printer is accessible from Secondary Client");
                        return false;
                    }
                    TraceFactory.Logger.Info("Since there is no Rule enabled in Secondary Client, Printer: {0} failed to access".FormatWith(activityData.WiredIPv4Address));
                    return true;
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                // return ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString());
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, deleteCertificates: true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, deleteCertificates: true);
            }
        }

        /// <summary>
        ///  Verify Printer accessibility with PFS option enabled
        /// 1. Create a rule on printer with Default All Address, Default All Service and following IPsec template settings:
        ///         a.	Authentication – PSK, Strength – Custom
        ///         b.	Phase I settings: Any settings will do
        ///         c.	Phase II settings: Transport mode, Select any 1 authentication, 1 encryption from ESP and disable AH. Under Advanced Settings, Enable PFS and select DH Group 14. Other settings will remain default.
        /// 2.	Set the Default rule action to “Drop”.
        /// 3.  Enable IPsec policy on the printer.
        /// 4.	Create similar rule on client machine.
        /// 5.	Ping printer with IPv4 address, it should work.
        /// </summary> 
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyPFSOption(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Setting default address and service templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                IKEPhase1Settings phase1 = new IKEPhase1Settings(DiffieHellmanGroups.DH14 | DiffieHellmanGroups.DH1 | DiffieHellmanGroups.DH2 | DiffieHellmanGroups.DH5 | DiffieHellmanGroups.DH15 | DiffieHellmanGroups.DH16 |
                                                                 DiffieHellmanGroups.DH17 | DiffieHellmanGroups.DH18, Encryptions.AES128 | Encryptions.DES | Encryptions.DES3 | Encryptions.AES192 | Encryptions.AES256, Authentications.SHA1 |
                                                                 Authentications.MD5 | Authentications.SHA256 | Authentications.SHA384 | Authentications.SHA512, 28800);
                AdvancedIKESettings advancedSettings = new AdvancedIKESettings(false, true, DiffieHellmanGroups.DH14);
                IKEPhase2Settings phase2 = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.AES128, 3600, 0, false);
                phase2.AdvancedIKESettings = advancedSettings;

                // Get the Pre shared Custom rule settings, by default the authentication type is set to transport mode while retrieving the settings
                SecurityRuleSettings settings = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.Custom, phase1Settings: phase1, phase2Settings: phase2);

                // Creating rule with All Addresses, All Services and IPsec with PSK High Security, Transport mode.
                EwsWrapper.Instance().CreateRule(settings, true);

                // Setting default rule action to Allow				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // Setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);
                    // Create rule in Client with similar settings
                    CtcUtility.CreateIPsecRule(settings);

                    TraceFactory.Logger.Info("Validating the Printer Accessibility from Primary Client");
                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }

                    // Disabling rule in Primary Client to coonect to secondary
                    CtcUtility.DeleteAllIPsecRules();
                    CtcUtility.SetFirewallPublicProfile(false);
                    Thread.Sleep(TimeSpan.FromMinutes(1));

                    ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);

                    TraceFactory.Logger.Info("Validating the accessibility of the Printer address from Secondary client");

                    if (connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                    {
                        TraceFactory.Logger.Info("Able to access the IPV4 address of the Printer from Secondary Client even after the rule is enabled in Printer");
                        return false;
                    }
                    TraceFactory.Logger.Info("Failed to access the Printer Ipaddress from Secondary Client since the rule is not enabled in Secondary Client");
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                return ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily);

            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, deleteCertificates: true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, deleteCertificates: true);
            }
        }

        /// <summary>
        ///  Verify connectivity from different clients.
        ///   Checking Printer accessibility from Primary and Secondary client with PSK authentication
        /// 1.Create a rule on printer with Default All  Address, All Service and PSK with Low Security strength IPsec template.
        /// 2.Set the Default rule action to “Drop”.
        /// 3.Enable IPsec policy on the printer.
        /// 4.Create rule on primary client machine with Any IP Address, Any Management Services and PSK.
        /// 5.Don’t create any rule on secondary client.
        /// 6.Ping from both clients.
        /// 7.All services should work from primary client and should fail from secondary client since rule is not created.
        /// </summary> 
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyConnectivityFromDifferentClients(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Setting default address and service templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPv4Addresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllManagementServices);

                // Get the Pre shared Custom rule settings, by default the authentication type is set to transport mode while retrieving the settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                // Creating rule with All Addresses, All Services and IPsec with PSK High Security, Transport mode.
                EwsWrapper.Instance().CreateRule(settings, true);

                // Setting default rule action to Allow				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // Setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.LowInteroperabilityHighsecurity);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    // Create rule in Client with similar settings
                    CtcUtility.CreateIPsecRule(settings);

                    // Printer accessibility should pass from first client
                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, telnet: DeviceServiceState.Pass, http: DeviceServiceState.Pass, ping: DeviceServiceState.Fail))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily))
                {
                    return false;
                }

                CtcUtility.DeleteAllIPsecRules();
                CtcUtility.SetFirewallPublicProfile(false);
                Thread.Sleep(TimeSpan.FromSeconds(10));

                TraceFactory.Logger.Info("Validating the ipaddress accessibility from Secondary Client");
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
                if (connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Info("Even after there is no rule exists in Secondary Client, still the Printer is accessible from Secondary Client");
                    return false;
                }
                TraceFactory.Logger.Info("Since there is no Rule enabled in Secondary Client, Printer ip:{0} fail to access".FormatWith(activityData.WiredIPv4Address));
                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, true);
            }
        }

        /// <summary>
        ///   Verify Printer accessibility with custom IPv6 address range         
        /// 1. Enable IPv6 option and set option Always perform DHCPv6 on startup.
        /// 2. Create a rule on printer with Custom address template: IPv6 address range, All Service and PSK Low security IPsec template.
        /// 3. Set the Default rule action to “Drop”.
        /// 4. Enable IPsec policy on the printer.
        /// 5. Create rule on first client machine with ipv6 range that includes the range specified in the printer with similar services and IPSec settings.
        /// 6. Create rule on second client machine with ipv6 range which doesn’t include the range specified on printer with similar services and IPSec settings.
        /// 7. Ping printer with ipv6 address provided above.
        /// 8. From client 1, ping should work. Client 2, ping will fail due to different ipv6 range.
        /// </summary> 
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyCustomIPV6AddressRange(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            string statelessAddressSubString = string.Empty;
            string localAddress, remoteAddress = string.Empty;

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                //// Enabling IPV6 option
                //EwsWrapper.Instance().SetIPv6(true);

                //// Setting option ‘Always perform DHCPv6 on startup’
                //EwsWrapper.Instance().SetDHCPv6OnStartup(true);

                TraceFactory.Logger.Debug("Creating Custom Address Template Rule");

                IPAddress statelessIPV6Address = printer.IPv6StatelessAddresses[0];
                localAddress = remoteAddress = GetPrinterIPV6AddressRange(statelessIPV6Address, out statelessAddressSubString);

                // Both the Local and remote ipv6 address range will be same only if the range exists in both printer and client               
                if (!ValidateIPV6AddressRange(statelessAddressSubString))
                {
                    TraceFactory.Logger.Debug("Failed to validate the ipv6 address:{0} range of printer and client".FormatWith(statelessAddressSubString));
                    return false;
                }

                // Setting default service and custom address templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddressRange, localAddress, remoteAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllPrintServices);

                // Get the Custom Address Template settings rule
                SecurityRuleSettings settings = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                // create rule with Custom Address Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                Service defaultCustomServiceSNMP = new Service();
                defaultCustomServiceSNMP.IsDefault = true;
                defaultCustomServiceSNMP.Name = "SNMP";
                defaultCustomServiceSNMP.Protocol = ServiceProtocolType.UDP;
                defaultCustomServiceSNMP.PrinterPort = "161";
                defaultCustomServiceSNMP.ServiceType = ServiceType.Printer;
                defaultCustomServiceSNMP.RemotePort = "Any";

                Service defaultSNMP = new Service();
                defaultSNMP.IsDefault = true;
                defaultSNMP.Name = "SNMP";
                defaultSNMP.Protocol = ServiceProtocolType.TCP;
                defaultSNMP.PrinterPort = "161-162";
                defaultSNMP.ServiceType = ServiceType.Printer;
                defaultSNMP.RemotePort = "Any";

                Collection<Service> ipSecService = new Collection<Service>();
                ipSecService.Add(defaultCustomServiceSNMP);
                if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())
                {
                    ipSecService.Add(defaultSNMP);
                }

                serviceTemplateSettings = new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo + "SNMP", ipSecService);
                addressTemplateSettings.Name = "AddressTemplateSNMP";

                SecurityRuleSettings settingsSNMP = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);
                settingsSNMP.IPsecTemplate.Name = "SNMPService";

                // create rule to set SNMP Service
                EwsWrapper.Instance().CreateRule(settingsSNMP, true);

                // setting default rule action to Allow
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    // Creating Rule in Client
                    CtcUtility.CreateIPsecRule(settings, true);

                    CtcUtility.CreateIPsecRule(settingsSNMP, true);

                    TraceFactory.Logger.Info("Validating the Ipaddress: {0} which has been created in the Range for Print Service".FormatWith(statelessIPV6Address));
                    // accessibility should pass since rule is enabled in printer and same rule is created on both client and printer

                    if (!CtcUtility.ValidateDeviceServices(printer, ipAddress: statelessIPV6Address.ToString(), isMessageBoxChecked: activityData.MessageBoxCheckBox, p9100: DeviceServiceState.Pass, isPingRequiredP9100Install: false,
                                                         printerDriver: activityData.PrintDriverLocation, printerDriverModel: activityData.PrintDriverModel))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                TraceFactory.Logger.Info("Creating rule in Secondary Client");

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                IPAddress statelessIPV6AddressSecondaryCLient = printer.IPv6StatelessAddresses[1];
                localAddress = remoteAddress = GetPrinterIPV6AddressRange(statelessIPV6AddressSecondaryCLient, out statelessAddressSubString);

                // Both the Local and remote ipv6 address range will be same only if the range exists in both printer and client, so validating for the same                
                if (!ValidateIPV6AddressRange(statelessAddressSubString))
                {
                    TraceFactory.Logger.Debug("Failed to validate the ipv6 address range of printer and client");
                    return false;
                }
                addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddressRange, localAddress, remoteAddress);

                // Get the Custom Address Template settings rule
                settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                CtcUtility.DeleteAllIPsecRules();

                // If Public profile is enabled, communication between the clients won't be successful; turning off.
                CtcUtility.SetFirewallPublicProfile(false);
                Thread.Sleep(TimeSpan.FromSeconds(10));

                // Creating Rule in Secondary Client
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
                connectivityService.Channel.CreateIPsecRule(settings, true, false);
                // connectivityService.Channel.CreateIPsecRule(settingsSNMP, true, false);

                // Ping the printer stateless ipv6 address which is not defined in the range of the created rule,and the result should fail
                TraceFactory.Logger.Debug("Perform the Print Service for printer stateless ipv6 address in Secondary Client which is not defined in the range of the created rule, and the result should fail");
                if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())
                {
                    if (connectivityService.Channel.IsFtpAccessible(statelessIPV6Address, family))
                    {
                        TraceFactory.Logger.Info("Able to access the device using FTP in secondary client, from the Ip address which is not in the range where Rule was created");
                        return false;
                    }
                }
                else
                {
                    if (connectivityService.Channel.PrintLpr(activityData.ProductFamily, statelessIPV6Address, PRINTFILEPATH))
                    {
                        TraceFactory.Logger.Info("Able to access the device using print lpr in secondary client, from the Ip address which is not in the range where Rule was created");
                        return false;
                    }
                }

                TraceFactory.Logger.Info("Fail to access the Printer using FTP in Secondary Client using the IPAddress which is not in the Range");
                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true, deleteCertificates: true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, true, deleteCertificates: true);
            }
        }

        /// <summary>
		///   Verify Printer accessibility with custom IPv6 address range         
		/// 1. Enable IPv6 option and set option Always perform DHCPv6 on startup.
		/// 2. Create a rule on printer with Custom address template: IPv6 address range, All Service and PSK Low security IPsec template.
		/// 3. Set the Default rule action to “Drop”.
		/// 4. Enable IPsec policy on the printer.
		/// 5. Create rule on first client machine with ipv6 range that includes the range specified in the printer with similar services and IPSec settings.
		/// 6. Create rule on second client machine with ipv6 range which doesn’t include the range specified on printer with similar services and IPSec settings.
		/// 7. Ping printer with ipv6 address provided above.
		/// 8. From client 1, ping should work. Client 2, ping will fail due to different ipv6 range.
		/// </summary> 
		/// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
		/// <param name="testNo">Test Case number</param>
		/// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyIPSecFunctionalityWithAddressRange(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            string statelessAddressSubString = string.Empty;
            string localAddress, remoteAddress = string.Empty;

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Enabling IPV6 option
                EwsWrapper.Instance().SetIPv6(true);

                // Setting option ‘Always perform DHCPv6 on startup’
                EwsWrapper.Instance().SetDHCPv6OnStartup(true);

                TraceFactory.Logger.Debug("Creating Custom Address Template Rule");

                IPAddress statelessIPV6Address = printer.IPv6StatelessAddresses[0];
                localAddress = remoteAddress = GetPrinterIPV6AddressRange(statelessIPV6Address, out statelessAddressSubString);

                // Both the Local and remote ipv6 address range will be same only if the range exists in both printer and client               
                if (!ValidateIPV6AddressRange(statelessAddressSubString))
                {
                    TraceFactory.Logger.Debug("Failed to validate the ipv6 address:{0} range of printer and client".FormatWith(statelessAddressSubString));
                    return false;
                }

                // Setting default service and custom address templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddressRange, localAddress, remoteAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                // Get the Custom Address Template settings rule
                //SecurityRuleSettings settings = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                // create rule with Custom Address Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                Service defaultCustomServiceSNMP = new Service();
                defaultCustomServiceSNMP.IsDefault = true;
                defaultCustomServiceSNMP.Name = "SNMP";
                defaultCustomServiceSNMP.Protocol = ServiceProtocolType.UDP;
                defaultCustomServiceSNMP.PrinterPort = "161";
                defaultCustomServiceSNMP.ServiceType = ServiceType.Printer;
                defaultCustomServiceSNMP.RemotePort = "Any";

                Service defaultSNMP = new Service();
                defaultSNMP.IsDefault = true;
                defaultSNMP.Name = "SNMP";
                defaultSNMP.Protocol = ServiceProtocolType.TCP;
                defaultSNMP.PrinterPort = "161-162";
                defaultSNMP.ServiceType = ServiceType.Printer;
                defaultSNMP.RemotePort = "Any";

                Collection<Service> ipSecService = new Collection<Service>();
                ipSecService.Add(defaultCustomServiceSNMP);
                if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())
                {
                    ipSecService.Add(defaultSNMP);
                }

                serviceTemplateSettings = new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo + "SNMP", ipSecService);
                addressTemplateSettings.Name = "AddressTemplateSNMP";

                //SecurityRuleSettings settingsSNMP = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);
                SecurityRuleSettings settingsSNMP = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);
                settingsSNMP.IPsecTemplate.Name = "SNMPService";

                // create rule to set SNMP Service
                EwsWrapper.Instance().CreateRule(settingsSNMP, true);

                // setting default rule action to Allow
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    // Creating Rule in Client
                    CtcUtility.CreateIPsecRule(settings, true);

                    CtcUtility.CreateIPsecRule(settingsSNMP, true);

                    TraceFactory.Logger.Info("Validating the Ipaddress: {0} which has been created in the Range for Print Service".FormatWith(statelessIPV6Address));
                    // accessibility should pass since rule is enabled in printer and same rule is created on both client and printer

                    if (!CtcUtility.ValidateDeviceServices(printer, ipAddress: statelessIPV6Address.ToString(), isMessageBoxChecked: activityData.MessageBoxCheckBox, p9100: DeviceServiceState.Pass, isPingRequiredP9100Install: false,
                                                         printerDriver: activityData.PrintDriverLocation, printerDriverModel: activityData.PrintDriverModel))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                TraceFactory.Logger.Info("Creating rule in Secondary Client");

                PrinterFamilies family = (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily);

                IPAddress statelessIPV6AddressSecondaryCLient = printer.IPv6StatelessAddresses[1];
                localAddress = remoteAddress = GetPrinterIPV6AddressRange(statelessIPV6AddressSecondaryCLient, out statelessAddressSubString);

                // Both the Local and remote ipv6 address range will be same only if the range exists in both printer and client, so validating for the same                
                if (!ValidateIPV6AddressRange(statelessAddressSubString))
                {
                    TraceFactory.Logger.Debug("Failed to validate the ipv6 address range of printer and client");
                    return false;
                }
                addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddressRange, localAddress, remoteAddress);

                // Get the Custom Address Template settings rule
                settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                CtcUtility.DeleteAllIPsecRules();

                // If Public profile is enabled, communication between the clients won't be successful; turning off.
                CtcUtility.SetFirewallPublicProfile(false);
                Thread.Sleep(TimeSpan.FromSeconds(10));

                // Creating Rule in Secondary Client
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
                connectivityService.Channel.CreateIPsecRule(settings, true, false);
                // connectivityService.Channel.CreateIPsecRule(settingsSNMP, true, false);

                // Ping the printer stateless ipv6 address which is not defined in the range of the created rule,and the result should fail
                TraceFactory.Logger.Debug("Perform the Print Service for printer stateless ipv6 address in Secondary Client which is not defined in the range of the created rule, and the result should fail");
                /*if(connectivityService.Channel.IsFtpAccessible(statelessIPV6Address, family))
				{
					TraceFactory.Logger.Info("Able to access the device using FTP in secondary client, from the Ip address which is not in the range where Rule was created");
					return false;
				}*/
                if (connectivityService.Channel.PrintLpr(activityData.ProductFamily, statelessIPV6Address, PRINTFILEPATH))
                {
                    TraceFactory.Logger.Info("Able to access the device using print lpr in secondary client, from the Ip address which is not in the range where Rule was created");
                    return false;
                }

                TraceFactory.Logger.Info("Fail to access the Printer using print lpr/ftp in Secondary Client using the IPAddress which is not in the Range");
                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, true);
            }
        }

        /// <summary>
		/// Test no: 756114
		/// 1.	Create IPsec rule on printer with All IP Address, All Services and IPsec template with following settings:
		///		(i)	Dynamic with IKEv1
		///		(ii)	Strength – Medium
		///		(iii)	Authentication – Pre shared key
		///	2.	Enable rule and Failsafe option, set default action to Drop.
		///	3.	Create rule on client machine with following settings IP Address, All Services, IPsec template with Dynamic IKEv1, Pre shared authentication mode, Custom strength. Phase I and Phase II settings are mentioned below:
		///		(i)		Phase I – DES and MD5, Phase II – ESP –DES and AH –MD5.
		///		(ii)	Phase I – DES and Sha2, Phase II – ESP –DES and AH –Sha2.
		///		(iii)	Phase I – 3DES and MD5, Phase II – ESP –3DES and AH –MD5.
		///		(iv)	Phase I – 3DES and Sha2, Phase II – ESP –3DES and AH –Sha2.
		///		(v)		Phase I – AES256 and MD5, Phase II – ESP – AES256 and AH –MD5.
		///		(vi)	Phase I – AES256 and Sha2, Phase II – ESP – AES256 and AH –Sha.
		///	4.	For the above step, each time the rule is created, ping to printer IPv4 address. It should be successful. 
		///	Once this step is performed, delete the existing rule on client machine and move to next rule settings. Perform Step 4 till all the IPsec configurations are specified in Step 3 are completed.
		/// </summary>
		/// <param name="activityData"></param>
		/// <param name="testNo"></param>
		/// <returns></returns>
		public static bool VerifyRuleWithDifferentAuthentication_EncryptionMethods(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                SecurityRuleSettings ruleSettings = GetPSKRuleSettings(testNo, DefaultAddressTemplates.AllIPAddresses, DefaultServiceTemplates.AllServices, IKESecurityStrengths.HighInteroperabilityLowsecurity);

                EwsWrapper.Instance().CreateRule(ruleSettings, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                AddressTemplateSettings addressTemplate = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                IKEPhase1Settings phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.DES, Authentications.MD5, 28800);
                IKEPhase2Settings phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.MD5, Encryptions.DES, 36000, 0, false);

                TraceFactory.Logger.Info("Client rule with Phase I: DES and MD5 and Phase II: ESP -DES, MD5");

                ruleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.Custom, phase1Settings, phase2Settings);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CreateClientRule(Encryptions.DES, Authentications.MD5);

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);

                }
                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily))
                {
                    return false;
                }

                CtcUtility.DeleteAllIPsecRules();

                phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.DES, Authentications.SHA1, 28800);
                phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.DES, 36000, 0, false);

                TraceFactory.Logger.Info("Client rule with Phase I: DES and Sha1 and Phase II: ESP -DES, Sha1");

                ruleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.Custom, phase1Settings, phase2Settings);

                try
                {
                    guid = Guid.Empty;
                    packetLocation = string.Empty;
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CreateClientRule(Encryptions.DES, Authentications.SHA1);

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);

                }
                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily))
                {
                    return false;
                }

                CtcUtility.DeleteAllIPsecRules();

                phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.DES3, Authentications.MD5, 28800);
                phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.MD5, Encryptions.DES3, 36000, 0, false);

                TraceFactory.Logger.Info("Client rule with Phase I: DES3 and MD5 and Phase II: ESP -DES3, MD5");

                ruleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.Custom, phase1Settings, phase2Settings);

                try
                {
                    guid = Guid.Empty;
                    packetLocation = string.Empty;
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CreateClientRule(Encryptions.DES3, Authentications.MD5);

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }
                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily))
                {
                    return false;
                }
                CtcUtility.DeleteAllIPsecRules();

                phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.DES3, Authentications.SHA1, 28800);
                phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.DES3, 36000, 0, false);

                TraceFactory.Logger.Info("Client rule with Phase I: DES3 and Sha1 and Phase II: ESP -DES3, Sha1");

                ruleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.Custom, phase1Settings, phase2Settings);

                try
                {
                    guid = Guid.Empty;
                    packetLocation = string.Empty;
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CreateClientRule(Encryptions.DES3, Authentications.SHA1);

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);

                }
                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily))
                {
                    return false;
                }


                CtcUtility.DeleteAllIPsecRules();

                phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES256, Authentications.MD5, 28800);
                phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.MD5, Encryptions.AES256, 36000, 0, false);

                TraceFactory.Logger.Info("Client rule with Phase I: AES128 and MD5 and Phase II: ESP -AES128, MD5");

                ruleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.Custom, phase1Settings, phase2Settings);

                try
                {
                    guid = Guid.Empty;
                    packetLocation = string.Empty;
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CreateClientRule(Encryptions.AES256, Authentications.MD5);

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }
                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily))
                {
                    return false;
                }


                CtcUtility.DeleteAllIPsecRules();

                phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES256, Authentications.SHA1, 28800);
                phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.AES256, 36000, 0, false);

                TraceFactory.Logger.Info("Client rule with Phase I: AES128 and Sha1 and Phase II: ESP -AES128, Sha1");

                ruleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.Custom, phase1Settings, phase2Settings);

                try
                {
                    guid = Guid.Empty;
                    packetLocation = string.Empty;
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CreateClientRule(Encryptions.AES256, Authentications.SHA1);

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);

                }
                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily))
                {
                    return false;
                }

                return true;


            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData);
            }
        }



        /***************************************************/
        /// <summary>
		///   Verify Printer accessibility with custom IPv6 address range         
		/// 1. Enable IPv6 option and set option Always perform DHCPv6 on startup.
		/// 2. Create a rule on printer with Custom address template: IPv6 address range, All Service and PSK Low security IPsec template.
		/// 3. Set the Default rule action to “Drop”.
		/// 4. Enable IPsec policy on the printer.
		/// 5. Create rule on first client machine with ipv6 range that includes the range specified in the printer with similar services and IPSec settings.
		/// 6. Create rule on second client machine with ipv6 range which doesn’t include the range specified on printer with similar services and IPSec settings.
		/// 7. Ping printer with ipv6 address provided above.
		/// 8. From client 1, ping should work. Client 2, ping will fail due to different ipv6 range.
		/// </summary> 
		/// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
		/// <param name="testNo">Test Case number</param>
		/// <returns>Returns true if the test case passed else returns false</returns>
		public static bool VerifyAddingCustomServiceWithNewServiceTemplate(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);


                // Setting default service and custom address templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings();

                Service customManageServiceHTTP1 = new Service();
                customManageServiceHTTP1.IsDefault = false;
                customManageServiceHTTP1.Name = "HTTP1";
                customManageServiceHTTP1.Protocol = ServiceProtocolType.TCP;
                customManageServiceHTTP1.PrinterPort = "Any";
                customManageServiceHTTP1.ServiceType = ServiceType.Remote;
                customManageServiceHTTP1.RemotePort = "80";

                Service customManageServiceHTTP2 = new Service();
                customManageServiceHTTP2.IsDefault = false;
                customManageServiceHTTP2.Name = "HTTP2";
                customManageServiceHTTP2.Protocol = ServiceProtocolType.TCP;
                customManageServiceHTTP2.PrinterPort = "80";
                customManageServiceHTTP2.ServiceType = ServiceType.Printer;
                customManageServiceHTTP2.RemotePort = "Any";

                Service customManageServiceHTTP3 = new Service();
                customManageServiceHTTP3.IsDefault = false;
                customManageServiceHTTP3.Name = "HTTP3";
                customManageServiceHTTP3.Protocol = ServiceProtocolType.TCP;
                customManageServiceHTTP3.PrinterPort = "8080";
                customManageServiceHTTP3.ServiceType = ServiceType.Printer;
                customManageServiceHTTP3.RemotePort = "Any";

                Collection<Service> ipSecService = new Collection<Service>();
                ipSecService.Add(customManageServiceHTTP1);
                ipSecService.Add(customManageServiceHTTP2);
                ipSecService.Add(customManageServiceHTTP3);

                serviceTemplateSettings = new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo + "HTTP", ipSecService);


                // Get the Custom Address Template settings rule
                //SecurityRuleSettings settings = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                // create rule with Custom Address Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Allow
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);


                    // Creating Rule in Client
                    CtcUtility.CreateIPsecRule(settings, true);

                    // accessibility should pass since rule is enabled in printer and same rule is created on both client and printer

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, http: DeviceServiceState.Pass))
                    {
                        return false;
                    }

                    return true;
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);

                }

            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, true);
            }
        }

        /// <summary>
        ///   Verify Printer accessibility with custom IPv6 address prefix
        /// 1.	Enable IPv6 option and set option ‘Always perform DHCPv6 on startup’
        /// 2.	Create a rule on printer with Custom address template: IPv6 address prefix, All Service and PSK Low security IPsec template.
        /// 3.	Set the Default rule action to “Drop”.
        /// 4.	Enable IPsec policy on the printer.
        /// 5.	Create rule on first client machine with All IPv6 address & same subnet mask with similar services and IPSec settings.
        /// 6.	Create rule on second client machine with All IPv6 address but different subnet mask with similar services and IPSec settings.
        /// 7.	Ping printer with ipv6 address provided above.
        /// 8.	From client 1, ping should work. Client 2, ping will fail due to different subnet mask. 
        /// </summary> 
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyCustomIPV6AddressPrefix(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            string statelessAddressSubString = string.Empty;
            string localAddress, remoteAddress = string.Empty;

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Enabling IPV6 option
                EwsWrapper.Instance().SetIPv6(true);

                // Setting option ‘Always perform DHCPv6 on startup’
                EwsWrapper.Instance().SetDHCPv6OnStartup(true);


                TraceFactory.Logger.Debug("Creating Custom Address Template Rule");

                IPAddress statelessIPV6Address = printer.IPv6StatelessAddresses[0];
                GetPrinterIPV6AddressRange(statelessIPV6Address, out statelessAddressSubString);

                // Setting prefix to the address,string format: 2000:200:200:203::1|64                
                localAddress = remoteAddress = statelessAddressSubString + "::" + "|" + 64;

                // Both the Local and remote ipv6 address range will be same only if the range exists in both printer and client               
                if (!ValidateIPV6AddressRange(statelessAddressSubString))
                {
                    TraceFactory.Logger.Debug("Failed to validate the ipv6 address:{0} range of printer and client".FormatWith(statelessAddressSubString));
                    return false;
                }

                Service defaultCustomService = new Service();
                defaultCustomService.IsDefault = true;
                defaultCustomService.Name = "P9100";
                defaultCustomService.Protocol = ServiceProtocolType.TCP;
                defaultCustomService.PrinterPort = "9100";
                defaultCustomService.ServiceType = ServiceType.Printer;
                defaultCustomService.RemotePort = "Any";

                Service defaultCustomServiceSNMP = new Service();
                defaultCustomServiceSNMP.IsDefault = true;
                defaultCustomServiceSNMP.Name = "SNMP";
                defaultCustomServiceSNMP.Protocol = ServiceProtocolType.UDP;
                defaultCustomServiceSNMP.PrinterPort = "161";
                defaultCustomServiceSNMP.ServiceType = ServiceType.Printer;
                defaultCustomServiceSNMP.RemotePort = "Any";

                Service defaultCustomServiceSNMP162 = new Service();
                defaultCustomServiceSNMP162.IsDefault = true;
                defaultCustomServiceSNMP162.Name = "SNMP";
                defaultCustomServiceSNMP162.Protocol = ServiceProtocolType.TCP;
                defaultCustomServiceSNMP162.PrinterPort = "161 - 162";
                defaultCustomServiceSNMP162.ServiceType = ServiceType.Printer;
                defaultCustomServiceSNMP162.RemotePort = "Any";

                Collection<Service> ipSecService = new Collection<Service>();
                ipSecService.Add(defaultCustomService);
                ipSecService.Add(defaultCustomServiceSNMP);
                if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())
                {
                    ipSecService.Add(defaultCustomServiceSNMP162);
                }

                // Setting default service and custom address templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddressPrefix, localAddress, remoteAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo, ipSecService);

                // Get the Custom Address Template settings rule
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                // create rule with Custom Address Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Allow
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    // Creating Rule in Client
                    CtcUtility.CreateIPsecRule(settings, true);

                    // accessibility should pass since rule is enabled in printer and same rule is created on both client and printer
                    if (!CtcUtility.ValidateDeviceServices(printer, ipAddress: statelessIPV6Address.ToString(), isMessageBoxChecked: activityData.MessageBoxCheckBox, p9100: DeviceServiceState.Pass, isPingRequiredP9100Install: false,
                                                         printerDriver: activityData.PrintDriverLocation, printerDriverModel: activityData.PrintDriverModel))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                // TODO: Enable validation
                //if (!ValidatePackets(packetLocation, statelessIPV6Address.ToString(), NetworkUtil.GetLocalAddress(statelessIPV6Address.ToString(), isIPv4Address: false).ToString()))
                //{
                //	return false;
                //}

                TraceFactory.Logger.Info("Creating rule in Secondary Client");

                IPAddress statelessIPV6AddressSecondaryCLient = printer.IPv6StatelessAddresses[1];
                GetPrinterIPV6AddressRange(statelessIPV6AddressSecondaryCLient, out statelessAddressSubString);

                // Setting prefix to the address string format: 2000:200:200:203::1|64                
                localAddress = remoteAddress = statelessAddressSubString + "::" + "|" + 64;

                // Both the Local and remote ipv6 address range will be same only if the range exists in both printer and client, so validating for the same                
                if (!ValidateIPV6AddressRange(statelessAddressSubString))
                {
                    TraceFactory.Logger.Debug("Failed to validate the ipv6 address range of printer and client");
                    return false;
                }
                addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddressPrefix, localAddress, remoteAddress);

                // Get the Custom Address Template settings rule
                settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                CtcUtility.DeleteAllIPsecRules();

                // If Public profile is enabled, communication between the clients won't be successful; turning off.
                CtcUtility.SetFirewallPublicProfile(false);
                Thread.Sleep(TimeSpan.FromSeconds(10));

                // Creating Rule in Secondary Client
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
                connectivityService.Channel.CreateIPsecRule(settings, true, false);

                // Ping the printer stateless ipv6 address which is not defined in the range of the created rule,and the result should fail
                TraceFactory.Logger.Info("Ping the printer stateless ipv6 address which is not defined in the range of the created rule,and the result should fail");
                if (connectivityService.Channel.Ping(statelessIPV6Address))
                {
                    TraceFactory.Logger.Info("Able to access the printer ip address:{0} in Secondary Client which is not defined in the range".FormatWith(statelessIPV6Address));
                    return false;
                }
                TraceFactory.Logger.Info("Failed to access the printer ip address:{0} in Secondary Client which is not defined in the range".FormatWith(statelessIPV6Address));
                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, true);
            }
        }

        /// <summary>
        /// Verify printer accessibility based on ipv6 option state.
        /// Checking Printer accessibility with IPv6 address when IPv6 option is enabled/disabled
        ///1.Enable IPv6 option on printer, DHCPv6 on startup option.
        ///2.Create IPsec rule with All IP Address, All Services and PSK with Low security strength.
        ///3.Set the Default rule action to “Drop”.
        ///4.Enable IPsec policy on the printer.
        ///5.Create similar rule on client machine.
        ///6.Don’t create any rule on Secondary machine.
        ///7.Ping printer from primary client with Stateless, state full and Link local addresses from client, all addresses should work.		
        ///8.Disable IPv6 option on printer and Ping with Stateless, state full and Link local address, ping should fail since ipv6 option is disabled.		
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyPrinterAccessibilityWithIPV6Option(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            string linkLocalAddress = CtcUtility.GetIPAddress(printer, IPAddressType.LinkLocalIPv6).ToString();
            string statelessAddress = CtcUtility.GetIPAddress(printer, IPAddressType.StatelessIPv6).ToString();

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Enabling IPV6 option
                EwsWrapper.Instance().SetIPv6(true);

                // Setting option ‘Always perform DHCPv6 on startup’
                EwsWrapper.Instance().SetDHCPv6OnStartup(true);

                // Setting default service and default address templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                // Get the Custom Address Template settings rule
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                // create rule with Custom Address Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Allow
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Creating Rule in Client
                CtcUtility.CreateIPsecRule(settings, true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    // Accessing the printer by pinging Link Local Address,ping should pass
                    if (!CtcUtility.ValidateDeviceServices(printer, ipAddress: linkLocalAddress, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }

					// Accessing the printer by pinging state full ipv6 address,ping should pass
					if (!CtcUtility.ValidateDeviceServices(printer, ipAddress: activityData.IPV6StatefullAddress, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
					{
						return false;
					}

					// Accessing the printer by pinging state less ipv6 address,ping should pass
					if (!CtcUtility.ValidateDeviceServices(printer, ipAddress: activityData.IPV6StatelessAddress, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
					{
						return false;
					}
				}
				finally
				{
					packetLocation = StopPacketCapture(guid);
				}

                // TODO: Enable validation
                //if (!ValidatePackets(packetLocation, statelessAddress, NetworkUtil.GetLocalAddress(statelessAddress, isIPv4Address: false).ToString()))
                //{
                //	return false;
                //}

                //if (!ValidatePackets(packetLocation, linkLocalAddress, NetworkUtil.GetLocalAddress(linkLocalAddress, isIPv4Address: false).ToString()))
                //{
                //	return false;
                //}

                // Disabling IPV6 option
                EwsWrapper.Instance().SetIPv6(false);



                // Accessing the printer by pinging Link Local Address,ping should fail since the ipv6 option is disabled
                if (!CtcUtility.ValidateDeviceServices(printer, ipAddress: linkLocalAddress, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Fail))
                {
                    return false;
                }

				// Accessing the printer by pinging state full ipv6 address,ping should fail since the ipv6 option is disabled
				if (!CtcUtility.ValidateDeviceServices(printer, ipAddress: activityData.IPV6StatefullAddress, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Fail))
				{
					return false;
				}

				// Accessing the printer by pinging state less ipv6 address,ping should fail since the ipv6 option is disabled
				if (!CtcUtility.ValidateDeviceServices(printer, ipAddress:activityData.IPV6StatelessAddress, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Fail))
				{
					return false;
				}
				return true;
			}
			catch (Exception ipSecException)
			{
				CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
				return false;
			}
			finally
			{
				// clean up the printer and primary, secondary clients to default state
				TestPostRequisites(activityData);

                // Enabling IPV6 option
                EwsWrapper.Instance().SetIPv6(true);

            }
        }

        /// <summary>
        /// Verify Printer accessibility with IPv4 address
        ///  Checking Printer accessibility with IPv4 address
        ///  1.Create IPsec rule with All IP Address, All Services and PSK with Custom security strength.
        ///     a.	Phase I: Any settings will do.
        ///     b.	Phase II: Select 1 Encryption and 1 Authentication from ESP. Disable AH. Leave other settings to default.
        ///  2.	Set the Default rule action to “Drop”.
        ///  3.	Enable IPsec policy on the printer.
        ///  4.	Create similar rule on client machine.
        ///  5.	Ping with IPv4 address from client machine, it should work.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyPrinterAccessibilityWithIPV4Address(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Setting default address and service templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                IKEPhase1Settings phase1 = new IKEPhase1Settings(DiffieHellmanGroups.DH14 | DiffieHellmanGroups.DH1 | DiffieHellmanGroups.DH2 | DiffieHellmanGroups.DH5 | DiffieHellmanGroups.DH14 | DiffieHellmanGroups.DH15 | DiffieHellmanGroups.DH16 |
                                                                 DiffieHellmanGroups.DH17 | DiffieHellmanGroups.DH18, Encryptions.AES128 | Encryptions.DES | Encryptions.DES3 | Encryptions.AES192 | Encryptions.AES256, Authentications.SHA1 |
                                                                 Authentications.MD5 | Authentications.SHA256 | Authentications.SHA384 | Authentications.SHA512, 28800);
                IKEPhase2Settings phase2 = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.AES128, 2800, 600000, false);

                // Get the Pre shared Custom rule settings, by default the authentication type is set to transport mode while retrieving the settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.Custom, phase1, phase2);

                // Creating rule with All Addresses, All Services and IPsec with PSK High Security, Transport mode.
                EwsWrapper.Instance().CreateRule(settings, true);

                // Setting default rule action to Allow				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // Setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    // Creating Rule in Client
                    CtcUtility.CreateIPsecRule(settings, true);

                    // Printer accessibility should pass since rule is created in printer and client
                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                return ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily);


            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and client to default state
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// 1. Create Rule with Kerberos settings on Printer.
        /// 2. Create Similar Rule on Secondary client and validate.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <param name="encryption"><see cref="Encryptions"/></param>
        /// <param name="strength"><see cref="IKESecurityStrengths"/></param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyRuleWithKerberos(IPSecurityActivityData activityData, int testNo, Encryptions encryption, IKESecurityStrengths strength,
                                                  DefaultServiceTemplates defaultServiceTemplate = DefaultServiceTemplates.AllServices, DefaultAddressTemplates defaultAddressTemplate = DefaultAddressTemplates.AllIPAddresses)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
            connectivityService.Channel.RebootMachine(IPAddress.Parse(activityData.WindowsSecondaryClientIPAddress));
            Thread.Sleep(TimeSpan.FromMinutes(2));

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                Service telnetService = new Service();
                telnetService.IsDefault = true;
                telnetService.Name = "TELNET";
                telnetService.Protocol = ServiceProtocolType.TCP;
                telnetService.PrinterPort = "23";
                telnetService.ServiceType = ServiceType.Printer;
                telnetService.RemotePort = "Any";

                Collection<Service> ipSecService = new Collection<Service>();
                ipSecService.Add(telnetService);

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, activityData.WindowsSecondaryClientIPAddress);
                ServiceTemplateSettings serviceTemplateSettings = encryption.Equals(Encryptions.AES128) ? new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo, ipSecService) :
                                                                  new ServiceTemplateSettings(defaultServiceTemplate);

                string keytabFilePath = string.Empty;

                switch (encryption)
                {
                    case Encryptions.DES:
                        keytabFilePath = KERBEROS_DES;
                        break;
                    case Encryptions.AES128:
                        keytabFilePath = KERBEROS_AES128;
                        break;
                    case Encryptions.AES256:
                        keytabFilePath = KERBEROS_AES256;
                        break;
                    default: break;
                }

                KerberosImportSettings importSettings = new KerberosImportSettings(KERBEROS_CONFIGFILE.FormatWith(activityData.ProductFamily), keytabFilePath);
                KerberosSettings kerberos = new KerberosSettings(importSettings);
                TraceFactory.Logger.Info("Kerberos : {0}".FormatWith(kerberos));
                SecurityRuleSettings settings = GetKerberosRuleSettings(testNo, kerberos, addressTemplateSettings, serviceTemplateSettings, strength);

                EwsWrapper.Instance().CreateRule(settings, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                CtcUtility.CreateIPsecRule(settings, true);

                TraceFactory.Logger.Debug("Rebooting secondary client machine");
                connectivityService.Channel.RebootMachine(IPAddress.Parse(activityData.WindowsSecondaryClientIPAddress));
                TraceFactory.Logger.Debug("Waiting for secondary client to reboot and come to ready state.");
                Thread.Sleep(TimeSpan.FromMinutes(2));

                // Create Rule on secondary client                
                TraceFactory.Logger.Debug("Successfully Rebooted the Secondary Client Machine");
                TraceFactory.Logger.Info("Creating Rule in Secondary Client");
                connectivityService.Channel.CreateIPsecRule(settings, enableRule: true, enableProfiles: false);

                // Enable Domain,Private and public firewall profiles and allow inbound firewall policy to communicate back to the primary client when firewall is enabled in secondary client
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);

                // Since the Kerberos is taking long time to establish connection, given delay of 1 min
                Thread.Sleep(TimeSpan.FromMinutes(1));

                TraceFactory.Logger.Info("Validating the accessibility of the Printer from Secondary Client");
                if (!connectivityService.Channel.IsTelnetAccessible(IPAddress.Parse(activityData.WiredIPv4Address), (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily)))
                {
                    TraceFactory.Logger.Info("Failed to access the Printer through telnet from Secondary Client");
                    return false;
                }
                TraceFactory.Logger.Info("Since the Rule is created in Secondary Client, able to telnet the Printer IPaddress");
                TraceFactory.Logger.Info("Deleting Ipsec Rules from Secodary client");
                connectivityService.Channel.DeleteAllIPsecRules();

                return true; ;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// 1. Create Rule with Kerberos settings on Printer.
        /// 2. Create Similar Rule on Secondary client and validate.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <param name="encryption"><see cref="Encryptions"/></param>
        /// <param name="strength"><see cref="IKESecurityStrengths"/></param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyPrintRuleWithKerberos(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                string clientIpv6Address = CtcUtility.GetClientIpAddress(activityData.WindowsSecondaryClientIPAddress, AddressFamily.InterNetworkV6).ToString();
                string printerIPv6Address = activityData.IPV6StatefullAddress;

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, printerIPv6Address, clientIpv6Address);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllPrintServices);

                KerberosImportSettings importSettings = new KerberosImportSettings(KERBEROS_CONFIGFILE.FormatWith(activityData.ProductFamily), KERBEROS_AES256);
                KerberosSettings kerberos = new KerberosSettings(importSettings);

                SecurityRuleSettings settings = GetKerberosRuleSettings(testNo, kerberos, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.MediumInteroperabilityMediumsecurity);

                EwsWrapper.Instance().CreateRule(settings, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // TODO: PC - secondary client

                TraceFactory.Logger.Info("Validating the accessibility of the Printer in Secondary Client");
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);

                // Rebooting secondary client machine 
                connectivityService.Channel.RebootMachine(IPAddress.Parse(activityData.WindowsSecondaryClientIPAddress));
                TraceFactory.Logger.Debug("Waiting for secondary client to reboot and come to ready state.");
                Thread.Sleep(TimeSpan.FromMinutes(2));

                connectivityService.Channel.CreateIPsecRule(settings, enableRule: true, enableProfiles: false);

                // Enable Domain,Private and public firewall profiles and allow inbound firewall policy to communicate back to the primary client when firewall is enabled in secondary client
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);

                // Since the Kerberos is taking long time to establish connection, given delay of 1 min
                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (!connectivityService.Channel.PrintLpr(activityData.ProductFamily, IPAddress.Parse(printerIPv6Address), PRINTFILEPATH))
                {
                    TraceFactory.Logger.Info("Unable to access the Printer from Secondary Client");
                    return false;
                }

                TraceFactory.Logger.Info("Since the rule is created in Secondary Client,able to access the Printer ipv6 address from Secondary Client throgh LPD Print Service");
                TraceFactory.Logger.Info("Deleting Ipsec Rules from Secodary client");
                connectivityService.Channel.DeleteAllIPsecRules();

                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, true);
            }
        }

        /// <summary>
        /// 1.	Create a rule on printer with All Addresses, All Services and IPsec with PSK High Security.
        /// 2.	Set the Default rule action to “Drop”.
        /// 3.	Enable IPsec policy on the printer.
        /// 4.	Create a similar rule on the client.
        /// 5.	Ping ipv4/ipv6 address from the client, it should work.
        /// 6.  Access Telnet/HTTP, it should work.
        /// 7.  Reboot Printer.
        /// 8.  Ping ipv4/ipv6 address from the client, it should work.
        /// 9.  Access Telnet/HTTP, it should work.
        /// 10. Should retain all rules created after reboot
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>        
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyPrinterAccessibilityAfterReboot(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Get the basic rule settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo);

                // Create rule with basic settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // Setting default rule action to Drop				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // Setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Creating Rule in Client
                CtcUtility.CreateIPsecRule(settings, true);

                // Printer accessibility should pass since rule is created in printer and client
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass, http: DeviceServiceState.Pass, telnet: DeviceServiceState.Pass))
                {
                    return false;
                }

                Collection<IPAddress> ipAddress = new Collection<IPAddress>();
                ipAddress.Add(IPAddress.Parse(activityData.WiredIPv4Address));

                TraceFactory.Logger.Info("Starting a continuous ping from the host for which the matching IPSec policy has been enabled for 10 minutes");
                TimeSpan pingTimeOut = TimeSpan.FromMinutes(10);

                Thread primaryPing = new Thread(() => CtcUtility.PingContinuously(ipAddress, pingTimeOut));
                printer.PowerCycle();

                primaryPing.Start();

                Thread.Sleep(pingTimeOut.Add(TimeSpan.FromMinutes(5)));

                primaryPing.Abort();

                Collection<PingStatics> primaryStatistics = CtcUtility.GetContinuousPingStatistics();
                TraceFactory.Logger.Info("Ping Statistics: {0}".FormatWith(primaryStatistics[0].Message));

                // Printer accessibility should pass after power cycle since rule is created in printer and client
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass, http: DeviceServiceState.Pass, telnet: DeviceServiceState.Pass))
                {
                    return false;
                }

                // Should retain all rules after power cycle
                return EwsWrapper.Instance().IsRuleExists();
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// 1.	Create a rule on printer with All Addresses, All Services and IPsec with PSK High Security.
        /// 2.  Create a rule on printer with All Addresses, All Services and IPsec with PSK Low Security.
        /// 2.	Set the Default rule action to “Drop”.
        /// 3.	Enable IPsec policy on the printer.          
        /// 4.  ColdReset the Printer.
        /// 5.  IP Sec Should be disabled and all rules should be deleted
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>        
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyPrinterAccessibilityAfterColdReset(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Get the basic rule settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, strength: IKESecurityStrengths.HighInteroperabilityLowsecurity);
                settings.IPsecTemplate.Name = settings.IPsecTemplate.Name + "1";

                // Create rule with basic settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // Get the basic rule settings
                SecurityRuleSettings settingsLowInterop = GetPSKRuleSettings(testNo, strength: IKESecurityStrengths.LowInteroperabilityHighsecurity);
                settingsLowInterop.IPsecTemplate.Name = settingsLowInterop.IPsecTemplate.Name + "2";

				// Creating Multiple Rules
				EwsWrapper.Instance().CreateRule(settingsLowInterop, true);

                // Setting default rule action to Drop				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // Setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                if (PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                {
                    EwsWrapper.Instance().SetFactoryDefaults(false);
                }
                else
                {
                    CtcUtility.CreateIPsecRule(settings, true);
                    CtcUtility.CreateIPsecRule(settingsLowInterop, true);
                    Thread.Sleep(TimeSpan.FromMinutes(1));
                    printer.ColdReset();
                }
				
				// After cold reset no rules exists in printer
				return !(EwsWrapper.Instance().IsRuleExists());
			}
			catch (Exception ipSecException)
			{
				CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
				return false;
			}
			finally
			{
				// clean up the printer and primary, secondary clients to default state
				TestPostRequisites(activityData);
			}                      
		}

        /// <summary>
        /// 1.	Create a rule on printer with All Addresses, All Services and IPsec with PSK High Security.        
        /// 2.	Set the Default rule action to “Drop”.
        /// 3.	Enable IPsec policy on the printer.          
        /// 4.	Create a similar rule on the client.
        /// 5.	Ping ipv4/ipv6 address from the client, it should work.
        /// 6.  Remove the Ethernet cable on the device.
        /// 7.  Ping ipv4/ipv6 address from the client, it should not work.
        /// 8.  Connect back the Ethernet cable.
        /// 9.  Ping ipv4/ipv6 address from the client, it should work.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>        
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyPrinterAccessibilityAfterHoseBreak(IPSecurityActivityData activityData, int testNo)
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

            if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())  // Need to remove once the setting network protocol issue is resolved
            {
                EwsWrapper.Instance().SetIPv6(true);
                EwsWrapper.Instance().SetDHCPv6OnStartup(true);
            }

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            INetworkSwitch networkSwitch = SwitchFactory.Create(IPAddress.Parse(activityData.SwitchIPAddress));

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Rule with Preshared Key");
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo);

                if (activityData.ProductFamily == PrinterFamilies.InkJet.ToString())
                {
                    TraceFactory.Logger.Info("Rule with Preshared Key");
                    if (!RuleWithHoseBreak(testNo, activityData, settings, networkSwitch))
                    {
                        return false;
                    }
                    CtcUtility.DeleteAllIPsecRules();
                    EwsWrapper.Instance().DeleteAllRules();
                }
                else
                {
                    TraceFactory.Logger.Info("Rule with Kerberos");
                    AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, activityData.WindowsSecondaryClientIPAddress);
                    ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                    KerberosImportSettings importSettings = new KerberosImportSettings(KERBEROS_CONFIGFILE.FormatWith(activityData.ProductFamily), KERBEROS_AES256);

                    KerberosSettings kerberos = new KerberosSettings(importSettings);
                    settings = GetKerberosRuleSettings(testNo, kerberos, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.LowInteroperabilityHighsecurity);
                    settings.IPsecTemplate.Name = "KerberosTemplate-{0}".FormatWith(testNo);

                    if (!RuleWithHoseBreak(testNo, activityData, settings, networkSwitch))
                    {
                        return false;
                    }
                }


                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())
                {
                    TestPostRequisites(activityData, secondaryCleanup: true);
                }
                else
                {
                    TestPostRequisites(activityData);
                }
            }
        }

        /// <summary>
        /// 1.	Create a rule on printer with All Addresses, All Services and IPsec with PSK High Security.        
        /// 2.	Set the Default rule action to “Drop”.
        /// 3.	Enable IPsec policy on the printer.          
        /// 4.	Create a similar rule on the client.
        /// 5.	Telnet/HTTP from the client, it should work.
        /// 6.  Create a similar rule on the second client.
        /// 7.  Telnet/HTTP from the client, it should work.
        /// 8.  Print Large Files from first and second client simultaneously.        
        /// 10. Print job should be successful.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>        
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyLargeFilePrintFromMultipleHosts(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            string remoteAddress = NetworkUtil.GetLocalAddress(activityData.PrimaryDhcpServerIPAddress, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, remoteAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                // Get the basic rule settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                // Create rule with basic settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // Creating second rule for secondary client to communicate
                addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo + "SecondClient", CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, activityData.WindowsSecondaryClientIPAddress);
                SecurityRuleSettings settingsSecondaryClient = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);
                settingsSecondaryClient.IPsecTemplate.Name = settingsSecondaryClient.IPsecTemplate.Name + "Second Rule";

                // Create rule with basic settings
                EwsWrapper.Instance().CreateRule(settingsSecondaryClient, true);

                // Setting default rule action to Drop				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // Setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);
                    // Creating Rule in Client
                    CtcUtility.CreateIPsecRule(settings, true);

                    // Printer accessibility should pass since rule is created in printer and client
                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, telnet: DeviceServiceState.Pass, http: DeviceServiceState.Pass))
                    {
                        return false;
                    }

                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily))
                {
                    return false;
                }

                // Creating Rule in secondary client
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
                connectivityService.Channel.CreateIPsecRule(settingsSecondaryClient, enableRule: true, enableProfiles: false);
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);

                // Printer accessibility should pass from secondary client since rule is created in printer and client
                if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Printing through LPR Command from both Primary and Secondary Clients");
                if (!printer.PrintLpr(IPAddress.Parse(activityData.WiredIPv4Address), PRINTFILEPATH))
                {
                    return false;
                }
                TraceFactory.Logger.Info("Successfully Printed the file from Primary Client");

                if (!connectivityService.Channel.PrintLpr(activityData.ProductFamily, IPAddress.Parse(activityData.WiredIPv4Address), PRINTFILEPATH))
                {
                    TraceFactory.Logger.Info("Failed to Print File by LPR Command from Secondary Client");
                    return false;
                }
                TraceFactory.Logger.Info("Successfully Printed the File by LPR Command from Secondary Client");
                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, true);
            }
        }

        /// <summary>
        /// Creating IPSec Template with Kerberos Manual Settings.        
        ///1. Create a rule with All Addresses, All Services and IPsec with Kerberos from Configuration file.        
        ///2. Set the Default rule action to “Drop”.
        ///3. Enable IPsec policy on the printer.          
        ///4. Create a similar rule on the client.
        ///5. Ping from the client, it should work.
        /// </summary>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyKerberosManualConfiguration(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Setting default service and custom address templates                
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, activityData.WindowsSecondaryClientIPAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                KerberosManualSettings manualKerberosSettings = new KerberosManualSettings(activityData.KerberosServerIPAddress, KERBEROS_USER_AES256 + '@' + KERBEROS_DOMAIN, KERBEROS_PSWD, KerberosEncryptionType.AES256SHA1);
                KerberosSettings kerberos = new KerberosSettings(manualKerberosSettings);

                // Get the settings with Kerberos
                SecurityRuleSettings settings = GetKerberosRuleSettings(testNo, kerberos, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.LowInteroperabilityHighsecurity);

                // If Kerberos Connection is not success, this method will throw an exception and returns false
                EwsWrapper.Instance().CreateRule(settings, true);

                // Setting default rule action to Drop				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // Setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                TraceFactory.Logger.Info("Validating the Accessibility of the Printer in Secondary Client");
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
                connectivityService.Channel.CreateIPsecRule(settings, enableRule: true, enableProfiles: false);

                // Enable Domain,Private and public firewall profiles and allow inbound firewall policy to communicate back to the primary client when firewall is enabled in secondary client
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);

                // Since the Kerberos is taking long time to establish connection, given delay of 2 min
                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Info("Failed to access the Printer in Secondary Client, even after enabling the Kereberos Rule");
                    return false;
                }
                TraceFactory.Logger.Info(" Since the Kerberos rule is enabled in Secondary Client, able to access the Printer");
                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData, true);
            }
        }

        /// <summary>
        /// 1.	Create a rule on printer with custom address[remote address as primary client], All Services and IPsec with PSK High Security.        
        /// 2.	Create one more rule on printer with custom address[remote address as secondary client], All Services and IPsec with PSK High Security.
        /// 3.	Set the Default rule action to “Drop”.
        /// 4.	Enable IPsec policy on the printer.          
        /// 5.	Create a similar rule on the primary client with the similar settings done in the printer[first rule].
        /// 6.  Create a similar rule on the secondary client with the similar settings created in the printer[second rule].
        /// 7.	From both primary and secondary the printer should be accessible.
        /// 8.  Start a continuous ping using command ping -t IP address from first and second client.
        /// 9.  Wait for 12 hours.
        /// 10. Calculate the pass percentage of ping reply and return to the user.        
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>        
        /// <returns>Returns true if the ping reply percentage is greater than 90% else returns false</returns>
        public static bool VerifyLongDurationPingFromMultipleHosts(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
            string remoteAddress = NetworkUtil.GetLocalAddress(activityData.PrimaryDhcpServerIPAddress, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, remoteAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                // Get the basic rule settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                // Create rule with basic settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // Creating second rule for secondary client to communicate
                addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo + "SecondRule", CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, activityData.WindowsSecondaryClientIPAddress);
                SecurityRuleSettings settingsSecondaryClient = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);
                settingsSecondaryClient.IPsecTemplate.Name = settingsSecondaryClient.IPsecTemplate.Name + "SecondRule";

                // Create rule with basic settings
                EwsWrapper.Instance().CreateRule(settingsSecondaryClient, true);

                // Setting default rule action to Drop				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // Setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Creating Rule in secondary client				
                connectivityService.Channel.CreateIPsecRule(settingsSecondaryClient, enableRule: true, enableProfiles: false);
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);

                // Printer accessibility should pass from secondary client since rule is created in printer and client
                if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    return false;
                }

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);
                    // Creating Rule in primary Client
                    CtcUtility.CreateIPsecRule(settings, true);

                    // Printer accessibility should pass since rule is created in printer and client
                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily))
                {
                    return false;
                }

                Collection<IPAddress> ipAddress = new Collection<IPAddress>();
                ipAddress.Add(IPAddress.Parse(activityData.WiredIPv4Address));

                TimeSpan pingTimeOut = TimeSpan.FromMinutes(10);

                Thread primaryPing = new Thread(() => CtcUtility.PingContinuously(ipAddress, pingTimeOut));
                connectivityService.Channel.PingContinuously(ipAddress, pingTimeOut);

                primaryPing.Start();

                Thread.Sleep(pingTimeOut.Add(TimeSpan.FromSeconds(30)));

                primaryPing.Abort();

                bool pingStatus = true;

                Collection<PingStatics> primaryStatistics = CtcUtility.GetContinuousPingStatistics();
                Collection<PingStatics> secondaryStatistics = connectivityService.Channel.GetContinuousPingStatistics();

				TraceFactory.Logger.Debug("Primary client ping Statistics: {0}".FormatWith(primaryStatistics[0].Message));
				TraceFactory.Logger.Debug("Secondary client ping Statistics: {0}".FormatWith(secondaryStatistics[0].Message));  
                
				pingStatus &= ((((primaryStatistics[0].PassCount * 100) / primaryStatistics[0].TotalCount) > 90) && (((secondaryStatistics[0].PassCount * 100)/ secondaryStatistics[0].TotalCount)) > 90);
                
				if (pingStatus)
				{
					TraceFactory.Logger.Info("Successfully pinged the printer simultaneously from both primary and secondary client");
					return true;
				}
				else
				{
					TraceFactory.Logger.Info("Ping not successful from primary and secondary client");
					return false;
				}
			}
			catch (Exception ipSecException)
			{
				CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress,true);
				return false;
			}
			finally
			{
				// clean up the printer and primary, secondary clients to default state
				TestPostRequisites(activityData, true);
			}
		}

        /// <summary>
        /// Verify IPSEC Web UI performance when all DNS traffic is allowed in the IPSec service template  
        ///1. Enter Valid DNS address in the WebUI.
        ///2. Create a rule with All IPAddresses and custom service templates [services:Telnet,HTTPS,DNS] and IPsec with PSK.
        ///3. Set the default IPsec rule to drop
        ///4. Enable the IPsec policy on the printer.
        ///5. Create a rule on the client with the same settings.
        ///6. Printer should be able to accessible ping.
        ///7. Access WebUI,it should work       
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyWebUIPerformanceWithDNSTraffic(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // setting valid DNS Server IP Address
                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);

                // setting service for telnet
                Service defaultCustomServiceTelnet = new Service();
                defaultCustomServiceTelnet.IsDefault = true;
                defaultCustomServiceTelnet.Name = "TELNET";
                defaultCustomServiceTelnet.Protocol = ServiceProtocolType.TCP;
                defaultCustomServiceTelnet.PrinterPort = "23";
                defaultCustomServiceTelnet.ServiceType = ServiceType.Printer;
                defaultCustomServiceTelnet.RemotePort = "Any";

                // setting service for https
                Service defaultCustomServiceHttps = new Service();
                defaultCustomServiceHttps.IsDefault = true;
                defaultCustomServiceHttps.Name = "HTTPS";
                defaultCustomServiceHttps.Protocol = ServiceProtocolType.TCP;
                defaultCustomServiceHttps.PrinterPort = "443";
                defaultCustomServiceHttps.ServiceType = ServiceType.Printer;
                defaultCustomServiceHttps.RemotePort = "Any";

                // setting service for DNS
                Service defaultCustomServiceDNS = new Service();
                defaultCustomServiceDNS.IsDefault = true;
                defaultCustomServiceDNS.Name = "DNS";
                defaultCustomServiceDNS.Protocol = ServiceProtocolType.TCP;
                defaultCustomServiceDNS.PrinterPort = "Any";
                defaultCustomServiceDNS.ServiceType = ServiceType.Remote;
                defaultCustomServiceDNS.RemotePort = "53";

                Collection<Service> ipSecService = new Collection<Service>();
                if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())
                {
                    ipSecService.Add(defaultCustomServiceTelnet);
                }
                ipSecService.Add(defaultCustomServiceHttps);
                ipSecService.Add(defaultCustomServiceDNS);

                // Setting default Address and custom Service Templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo, ipSecService);

                // Get the Custom Service Template settings rule
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                // create rule with Custom Service Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Allow
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Creating rule in client with same setting
                CtcUtility.CreateIPsecRule(settings);

                // Accessibility should pass only for https since rule is created with services -https,telnet and DNS
                return CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, https: DeviceServiceState.Pass, ping: DeviceServiceState.Fail);
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Verify IPSEC WebUI performance when  DNS traffic is not included as part of the IPSec service  template
        ///1. Enter Valid DNS address in the WebUI.
        ///2. Create a rule with All IPAddresses and custom service templates [services:Telnet,HTTPS,without DNS traffic] and IPsec with PSK.
        ///3. Set the default IPsec rule to drop
        ///4. Enable the IPsec policy on the printer.
        ///5. Create a rule on the client with the same settings.
        ///6. Printer should be able to accessible ping.
        ///7. Access WebUI,it should work       
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyWebUIPerformanceWithOutDNSTraffic(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // setting valid DNS Server IP Address
                EwsWrapper.Instance().SetPrimaryDnsServer(activityData.PrimaryDhcpServerIPAddress);

                // setting service for telnet
                Service defaultCustomServiceTelnet = new Service();
                defaultCustomServiceTelnet.IsDefault = true;
                defaultCustomServiceTelnet.Name = "TELNET";
                defaultCustomServiceTelnet.Protocol = ServiceProtocolType.TCP;
                defaultCustomServiceTelnet.PrinterPort = "23";
                defaultCustomServiceTelnet.ServiceType = ServiceType.Printer;
                defaultCustomServiceTelnet.RemotePort = "Any";

                // setting service for https
                Service defaultCustomServiceHttps = new Service();
                defaultCustomServiceHttps.IsDefault = true;
                defaultCustomServiceHttps.Name = "HTTPS";
                defaultCustomServiceHttps.Protocol = ServiceProtocolType.TCP;
                defaultCustomServiceHttps.PrinterPort = "443";
                defaultCustomServiceHttps.ServiceType = ServiceType.Printer;
                defaultCustomServiceHttps.RemotePort = "Any";

                Collection<Service> ipSecService = new Collection<Service>();
                if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())
                {
                    ipSecService.Add(defaultCustomServiceTelnet);
                }
                ipSecService.Add(defaultCustomServiceHttps);

                // Setting default Address and custom Service Templates
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo, ipSecService);

                // Get the Custom Service Template settings rule
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                // create rule with Custom Service Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Allow
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Creating rule in client with same setting
                CtcUtility.CreateIPsecRule(settings);

                // Accessibility should pass only for https since rule is created with services -https,telnet
                return CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, https: DeviceServiceState.Pass, ping: DeviceServiceState.Fail);
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// 1.	Create IPsec rule with All IP Address, All Services templates, IPsec Template-SHA1 and AES-256     
        ///      In Printer, Client1 and Client2.
        /// 2.  Start a printing job from client1 for 10-15 min.
        /// 3.  Perform Telnet from client2 while client1 is printing.
        /// 4.  Device should process the print job.
        /// 5.  Telnet should work.      
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyIPsecRequestFromMultipleClients(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            string remoteAddress = NetworkUtil.GetLocalAddress(activityData.PrimaryDhcpServerIPAddress, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, remoteAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                IKEPhase1Settings phase1Printer = new IKEPhase1Settings(DiffieHellmanGroups.DH14 | DiffieHellmanGroups.DH1 | DiffieHellmanGroups.DH2 | DiffieHellmanGroups.DH5 | DiffieHellmanGroups.DH14 | DiffieHellmanGroups.DH15 | DiffieHellmanGroups.DH16 |
                                                                 DiffieHellmanGroups.DH17 | DiffieHellmanGroups.DH18, Encryptions.AES256, Authentications.SHA1, 28800);
                IKEPhase2Settings phase2Printer = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.AES256, 36000, 0, false);

                // Get the Pre shared Custom rule settings, by default the authentication type is set to transport mode while retrieving the settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.Custom, phase1Printer, phase2Printer, defaultAction: DefaultAction.Allow);

                // create rule with Custom Service Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Allow
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Creating rule in first client with same setting
                CtcUtility.CreateIPsecRule(settings);

                // Creating Rule in secondary client
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
                connectivityService.Channel.CreateIPsecRule(settings, enableRule: true, enableProfiles: false);

                // Printing from first Client
                if (!printer.PrintLpr(IPAddress.Parse(activityData.WiredIPv4Address), PRINTFILEPATH))
                {
                    return false;
                }

                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);

                //Performing Telnet from Client2
                return connectivityService.Channel.IsTelnetAccessible(IPAddress.Parse(activityData.WiredIPv4Address), (PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), activityData.ProductFamily));
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData, true);
            }
        }

        /// <summary>
        /// 1.	Create IPsec rule with All IP Address, Printing Services, IPsec Template-SHA1 and AES-256     
        ///     In Printer using PSK with Allow.
        /// 2.  Create IPsec rule with All IP Address, Printing Services [9100], IPsec Template-SHA1 and AES-256     
        ///     In Printer using PSK with Allow.
        /// 3.  Create similar rule in Client Machine.
        /// 4.  Make sure client host is installed Printer using P9100 protocol.
        /// 5.  Fire a Print Job from the client machine using P9100 protocol.
        /// 6.  Print Job Should be Successful.
        /// 7.  Delete the first rule which is created with printing service template.
        /// 8.  Fire a Print Job from the client machine after deleting rule.
        /// 9.  Print job should fail.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyDeviceBehaviourWithMultipleRulesCreationAndDeletion(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            string remoteAddress = NetworkUtil.GetLocalAddress(activityData.PrimaryDhcpServerIPAddress, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllPrintServices);

                IKEPhase1Settings phase1Printer = new IKEPhase1Settings(DiffieHellmanGroups.DH14 | DiffieHellmanGroups.DH1 | DiffieHellmanGroups.DH2 | DiffieHellmanGroups.DH5 | DiffieHellmanGroups.DH14 | DiffieHellmanGroups.DH15 | DiffieHellmanGroups.DH16 |
                                                                 DiffieHellmanGroups.DH17 | DiffieHellmanGroups.DH18, Encryptions.AES256, Authentications.SHA1, 28800);
                IKEPhase2Settings phase2Printer = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.AES256, 36000, 0, false);

                // Get the Pre shared Custom rule settings, by default the authentication type is set to transport mode while retrieving the settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.Custom, phase1Printer, phase2Printer);

                // create rule with Custom Service Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // Creating third rule with printing service 9100                
                Service defaultCustomService = new Service();
                defaultCustomService.IsDefault = true;
                defaultCustomService.Name = "P9100";
                defaultCustomService.Protocol = ServiceProtocolType.TCP;
                defaultCustomService.PrinterPort = "9100";
                defaultCustomService.ServiceType = ServiceType.Printer;
                defaultCustomService.RemotePort = "Any";

                // Creating third rule with printing service 9100                
                Service defaultCustomServiceSNMP = new Service();
                defaultCustomServiceSNMP.IsDefault = true;
                defaultCustomServiceSNMP.Name = "SNMP";
                defaultCustomServiceSNMP.Protocol = ServiceProtocolType.UDP;
                defaultCustomServiceSNMP.PrinterPort = "161";
                defaultCustomServiceSNMP.ServiceType = ServiceType.Printer;
                defaultCustomServiceSNMP.RemotePort = "Any";

                Service defaultCustomServiceSNMP162 = new Service();
                defaultCustomServiceSNMP162.IsDefault = true;
                defaultCustomServiceSNMP162.Name = "SNMP";
                defaultCustomServiceSNMP162.Protocol = ServiceProtocolType.TCP;
                defaultCustomServiceSNMP162.PrinterPort = "161 - 162";
                defaultCustomServiceSNMP162.ServiceType = ServiceType.Printer;
                defaultCustomServiceSNMP162.RemotePort = "Any";

                Collection<Service> ipSecService = new Collection<Service>();
                ipSecService.Add(defaultCustomService);
                ipSecService.Add(defaultCustomServiceSNMP);
                if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())
                {
                    ipSecService.Add(defaultCustomServiceSNMP162);
                }

                // Setting default Address and custom Service Templates                
                serviceTemplateSettings = new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo, ipSecService);

                SecurityRuleSettings settingsService = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.Custom, phase1Printer, phase2Printer, defaultAction: DefaultAction.Allow);
                settingsService.IPsecTemplate.Name = settings.IPsecTemplate.Name + "P9100PrintService";

                EwsWrapper.Instance().Stop();
                EwsWrapper.Instance().Start();
                Thread.Sleep(TimeSpan.FromMinutes(1));
				// create rule with Custom Service Template settings
				EwsWrapper.Instance().CreateRule(settingsService, true);

                // setting default rule action to Allow
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Creating rule in client with printing service 9100
                CtcUtility.CreateIPsecRule(settingsService);

                CtcUtility.CreateIPsecRule(settings);

                if (!CtcUtility.ValidateDeviceServices(printer, p9100: DeviceServiceState.Pass, printerDriver: activityData.PrintDriverLocation, printerDriverModel: activityData.PrintDriverModel, isPingRequiredP9100Install: false))
                {
                    TraceFactory.Logger.Info("Failed to Print the job with P9100 proocol");
                    return false;
                }
                TraceFactory.Logger.Info("Successfully printed the job with P9100 Protocol");

                // Deleting the first rule created
                TraceFactory.Logger.Info("Deleting the job created with Print Services");
                if (!EwsWrapper.Instance().DeleteRule(settings.IPsecTemplate.Name))
                {
                    return false;
                }
                Thread.Sleep(TimeSpan.FromSeconds(40));

                // Since the rule with Printing services has been deleted from printer, print job with LPR will fail
                return !(printer.PrintLpr(IPAddress.Parse(activityData.WiredIPv4Address), CtcUtility.CreateFile("Test file for validating LPR print.")));
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// 1.  Go to Networking->Authorization menu->Access Control.
        /// 2.  Enter specific host IPv4 address on the network without subnet mask and check the save checkbox 
        ///     and press apply button.  
        /// 3.  Check the "Allow Web server (HTTP) access" checkbox and press apply button.        
        /// 4.  From the IPsec main page of the Web UI create an IPsec rules using All IP Address template, 
        ///     specific services with FTP, Telnet and HTTP and IPsec Template using pre shared key.
        /// 5.  Ensure that Default IPsec policy is "Drop" and enable IPsec policy on the web UI of the device.
        /// 6.  Create a similar rule in client.        
        /// 7.  Web UI should be accessible, telnet and HTTP also should work from the specified host.     
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyDeviceBehaviourWithACL(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            string remoteAddress = NetworkUtil.GetLocalAddress(activityData.PrimaryDhcpServerIPAddress, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Setting up the ACL
                EwsWrapper.Instance().SetACL(remoteAddress);

                // Creating service with Telnet              
                Service telnetService = new Service();
                telnetService.IsDefault = true;
                telnetService.Name = "TELNET";
                telnetService.Protocol = ServiceProtocolType.TCP;
                telnetService.PrinterPort = "23";
                telnetService.ServiceType = ServiceType.Printer;
                telnetService.RemotePort = "Any";

                // Creating service with FTP              
                Service ftpService = new Service();
                ftpService.IsDefault = true;
                ftpService.Name = "FTP";
                ftpService.Protocol = ServiceProtocolType.TCP;
                ftpService.PrinterPort = "21";
                ftpService.ServiceType = ServiceType.Printer;
                ftpService.RemotePort = "Any";

                // Creating service with HTTP              
                Service httpService = new Service();
                httpService.IsDefault = true;
                httpService.Name = "HTTP";
                httpService.Protocol = ServiceProtocolType.TCP;
                httpService.PrinterPort = "80";
                httpService.ServiceType = ServiceType.Printer;
                httpService.RemotePort = "Any";

                Collection<Service> ipSecService = new Collection<Service>();
                ipSecService.Add(telnetService);
                ipSecService.Add(ftpService);
                ipSecService.Add(httpService);

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo, ipSecService);

                // Get the Pre shared Custom rule settings, by default the authentication type is set to transport mode while retrieving the settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);

                // create rule with Custom Service Template settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Allow
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Creating rule in first client with same setting
                CtcUtility.CreateIPsecRule(settings);

                return CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, telnet: DeviceServiceState.Pass, http: DeviceServiceState.Pass);
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                TestPostRequisites(activityData);
                EwsWrapper.Instance().SetACL(remoteAddress, save: false);
            }
        }

        /// <summary>
        /// 1.  Create IPsec rule with All IP Address, All Services, IP sec Template using Pre shared Key.
        /// 2.  Create similar rule in client in a different network.
        /// 3.  Ping the Printer, it should work.
        /// 4.  Create IPsec rule with All IP Address, All Services, IP sec Template using Certificates.
        /// 5.  Create similar rule in client in a different network.
        /// 6.  Ping the Printer, it should work.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyIPsecAcrossSubnets(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            string clientNetwork = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            string remoteSecondaryAddress = NetworkUtil.GetLocalAddress(activityData.SecondDhcpServerIPAddress, IPAddress.Parse(activityData.SecondDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Adding Routing, Command used:{0}".FormatWith("/C ping -S {0} {1}".FormatWith(remoteSecondaryAddress, activityData.WiredIPv4Address)));
                // Add Routing
                ProcessUtil.Execute("cmd.exe", "/C ping -S {0} {1}".FormatWith(remoteSecondaryAddress, activityData.WiredIPv4Address));

                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, defaultAction: DefaultAction.Allow);
                if (!Rule_AcrossInterface(activityData, settings, printer))
                {
                    return false;
                }

                settings = GetCertificatesRuleSettings(testNo, defaultAction: DefaultAction.Allow);
                if (!Rule_AcrossInterface(activityData, settings, printer))
                {
                    return false;
                }
                return true;

                // In STF Environment Kerberos across interface is not working, if 80 network is disabled in secondary client, printer will ping and i will be able to access the primary client
                // from secondary but am not able to access the secondary client ip from Primary, hence after pinging the result am not able to get in Primary
                //AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, activityData.WindowsSecondaryClientIPAddress);
                //ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                //KerberosImportSettings importSettings = new KerberosImportSettings(KERBEROS_CONFIGFILE.FormatWith(activityData.ProductFamily), KERBEROS_AES256);
                //KerberosSettings kerberos = new KerberosSettings(importSettings);

                //settings = Utility.GetKerberosRuleSettings(testNo, kerberos, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.LowInteroperabilityHighsecurity, defaultAction: DefaultAction.Allow);
                //return Rule_AcrossInterface(activityData, settings, printer);  
            }
            catch (Exception ipSecException)
            {
                NetworkUtil.EnableNetworkConnection(clientNetwork.ToString());
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                NetworkUtil.EnableNetworkConnection(clientNetwork.ToString());
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Test no: 83020
        /// 1.	Create rule on printer with All Address, Custom service with ICMP, SNMP, Telnet, FTP & p9100 services and IPsec template with Dynamic IKEv1, PSK with Medium strength.
        ///	2.	Enable rule and Failsafe option. 
        ///	3.	Set default action as “Drop”.
        ///	4.	Create similar rule on client machine.
        ///	5.	Perform Ping, SNMP, Telnet, FTP and p9100 operations with IPv4 address.
        ///	6.	All services should be successful.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyRuleWithCustomServices(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // setting service for http
                Service httpService = new Service();
                httpService.IsDefault = true;
                httpService.Name = "HTTP";
                httpService.Protocol = ServiceProtocolType.TCP;
                httpService.PrinterPort = "80";
                httpService.ServiceType = ServiceType.Printer;
                httpService.RemotePort = "Any";

                // setting service for snmp
                Service snmpService = new Service();
                snmpService.IsDefault = true;
                snmpService.Name = "SNMP";
                snmpService.Protocol = ServiceProtocolType.UDP;
                snmpService.PrinterPort = "161";
                snmpService.ServiceType = ServiceType.Printer;
                snmpService.RemotePort = "Any";

                // setting service for telnet
                Service telnetService = new Service();
                telnetService.IsDefault = true;
                telnetService.Name = "TELNET";
                telnetService.Protocol = ServiceProtocolType.TCP;
                telnetService.PrinterPort = "23";
                telnetService.ServiceType = ServiceType.Printer;
                telnetService.RemotePort = "Any";

                // setting service for ftp: 21 port
                Service ftpService1 = new Service();
                ftpService1.IsDefault = true;
                ftpService1.Name = "FTP";
                ftpService1.Protocol = ServiceProtocolType.TCP;
                ftpService1.PrinterPort = "21";
                ftpService1.ServiceType = ServiceType.Printer;
                ftpService1.RemotePort = "Any";

                // setting service for ftp: 20 port
                Service ftpService2 = new Service();
                ftpService2.IsDefault = true;
                ftpService2.Name = "FTP";
                ftpService2.Protocol = ServiceProtocolType.TCP;
                ftpService2.PrinterPort = "20";
                ftpService2.ServiceType = ServiceType.Remote;
                ftpService2.RemotePort = "Any";

                Collection<Service> services = new Collection<Service>();

                services.Add(httpService);
                services.Add(snmpService);
                if (activityData.ProductFamily != PrinterFamilies.InkJet.ToString())
                {
                    services.Add(telnetService);
                    services.Add(ftpService1);
                    services.Add(ftpService2);
                }

                AddressTemplateSettings addressTemplate = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo, services);

                SecurityRuleSettings ruleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.MediumInteroperabilityMediumsecurity);

                EwsWrapper.Instance().CreateRule(ruleSettings, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CtcUtility.CreateIPsecRule(ruleSettings, true);

                    // FTP in Automation is popping up a message, hence not included FTP Validation here
                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, snmp: DeviceServiceState.Pass, telnet: DeviceServiceState.Pass, http: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
                if (connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Info("Even after the rule is not enabled in secondary client, still the printer is pinging");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Since the rule is not enabled in Secondary Client, printer fails to ping");
                }
                return ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily);

            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Test no: 83052
        /// 1.	Create IPsec rule on printer with All IP Address, All Services and IPsec template with following settings:
        ///		(i)	Dynamic with IKEv1
        ///		(ii)	Strength – Medium
        ///		(iii)	Authentication – Pre shared key
        ///	2.	Enable rule and Failsafe option, set default action to Drop.
        ///	3.	Create rule on client machine with following settings IP Address, All Services, IPsec template with Dynamic IKEv1, Pre shared authentication mode, Custom strength. Phase I and Phase II settings are mentioned below:
        ///		(i)		Phase I – DES and MD5, Phase II – ESP –DES and AH –MD5.
        ///		(ii)	Phase I – DES and Sha1, Phase II – ESP –DES and AH –Sha1.
        ///		(iii)	Phase I – 3DES and MD5, Phase II – ESP –3DES and AH –MD5.
        ///		(iv)	Phase I – 3DES and Sha1, Phase II – ESP –3DES and AH –Sha1.
        ///		(v)		Phase I – AES128 and MD5, Phase II – ESP – AES128 and AH –MD5.
        ///		(vi)	Phase I – AES128 and Sha1, Phase II – ESP – AES128 and AH –Sha1.
        ///	4.	For the above step, each time the rule is created, ping to printer IPv4 address. It should be successful. 
        ///	Once this step is performed, delete the existing rule on client machine and move to next rule settings. Perform Step 4 till all the IPsec configurations are specified in Step 3 are completed.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyRuleWithDifferentAuthentication_Encryption(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                SecurityRuleSettings ruleSettings = GetPSKRuleSettings(testNo, DefaultAddressTemplates.AllIPAddresses, DefaultServiceTemplates.AllServices, IKESecurityStrengths.HighInteroperabilityLowsecurity);

                EwsWrapper.Instance().CreateRule(ruleSettings, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                AddressTemplateSettings addressTemplate = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                IKEPhase1Settings phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.DES, Authentications.MD5, 28800);
                IKEPhase2Settings phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.MD5, Encryptions.DES, 36000, 0, false);

                TraceFactory.Logger.Info("Client rule with Phase I: DES and MD5 and Phase II: ESP -DES, MD5");

                ruleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.Custom, phase1Settings, phase2Settings);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CreateClientRule(Encryptions.DES, Authentications.MD5);

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }
                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily))
                {
                    return false;
                }

                CtcUtility.DeleteAllIPsecRules();

                phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.DES, Authentications.SHA1, 28800);
                phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.DES, 36000, 0, false);

                TraceFactory.Logger.Info("Client rule with Phase I: DES and Sha1 and Phase II: ESP -DES, Sha1");

                ruleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.Custom, phase1Settings, phase2Settings);

                try
                {
                    guid = Guid.Empty;
                    packetLocation = string.Empty;
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CreateClientRule(Encryptions.DES, Authentications.SHA1);

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }
                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily))
                {
                    return false;
                }

                CtcUtility.DeleteAllIPsecRules();

                phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.DES3, Authentications.MD5, 28800);
                phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.MD5, Encryptions.DES3, 36000, 0, false);

                TraceFactory.Logger.Info("Client rule with Phase I: DES3 and MD5 and Phase II: ESP -DES3, MD5");

                ruleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.Custom, phase1Settings, phase2Settings);

                try
                {
                    guid = Guid.Empty;
                    packetLocation = string.Empty;
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CreateClientRule(Encryptions.DES3, Authentications.MD5);

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }
                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily))
                {
                    return false;
                }
                CtcUtility.DeleteAllIPsecRules();

                phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.DES3, Authentications.SHA1, 28800);
                phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.DES3, 36000, 0, false);

                TraceFactory.Logger.Info("Client rule with Phase I: DES3 and Sha1 and Phase II: ESP -DES3, Sha1");

                ruleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.Custom, phase1Settings, phase2Settings);

                try
                {
                    guid = Guid.Empty;
                    packetLocation = string.Empty;
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CreateClientRule(Encryptions.DES3, Authentications.SHA1);

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }
                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily))
                {
                    return false;
                }


                CtcUtility.DeleteAllIPsecRules();

                phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.MD5, 28800);
                phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.MD5, Encryptions.AES128, 36000, 0, false);

                TraceFactory.Logger.Info("Client rule with Phase I: AES128 and MD5 and Phase II: ESP -AES128, MD5");

                ruleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.Custom, phase1Settings, phase2Settings);

                try
                {
                    guid = Guid.Empty;
                    packetLocation = string.Empty;
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CreateClientRule(Encryptions.AES128, Authentications.MD5);

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }
                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily))
                {
                    return false;
                }


                CtcUtility.DeleteAllIPsecRules();

                phase1Settings = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.SHA1, 28800);
                phase2Settings = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.AES128, 36000, 0, false);

                TraceFactory.Logger.Info("Client rule with Phase I: AES128 and Sha1 and Phase II: ESP -AES128, Sha1");

                ruleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.Custom, phase1Settings, phase2Settings);

                try
                {
                    guid = Guid.Empty;
                    packetLocation = string.Empty;
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CreateClientRule(Encryptions.AES128, Authentications.SHA1);

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                return ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString(), activityData.ProductFamily);
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Test no: 408961
        /// 1.	Create IPsec rule on printer with Custom IP Address – Specific IP Address, All Services and IPsec template with following settings:
        ///		(i)		Dynamic with IKEv1
        ///		(ii)	Strength – High
        ///		(iii)	Authentication – Pre shared key
        ///	2.	Enable rule and Failsafe option, set default action to Drop.
        ///	3.	Create similar rule on client machine.
        ///	4.	Ping with printer IPv4 address, it should be successful.
        ///	5.	Create similar as mentioned in Step 1 but change the IP Address to client 2 and strength to Low.
        ///	6.	Create rule on client 2.
        ///	7.	Ping with printer IPv4 address, it should be successful.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyPrinterAccessibilityWithMultipleRules(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string clientAddress = NetworkUtil.GetLocalAddress(activityData.PrimaryDhcpServerIPAddress, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, clientAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                SecurityRuleSettings ruleSettingsPrimary = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.HighInteroperabilityLowsecurity);

                EwsWrapper.Instance().CreateRule(ruleSettingsPrimary, true);

                // Creating Rule to communicate with Secondary Client
                addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo + "SecondRule", CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, activityData.WindowsSecondaryClientIPAddress);
                SecurityRuleSettings ruleSettingsSecondary = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.HighInteroperabilityLowsecurity);
                ruleSettingsSecondary.IPsecTemplate.Name = ruleSettingsSecondary.IPsecTemplate.Name + "Second Rule";
                ruleSettingsSecondary.IPsecTemplate.DynamicKeysSettings.V1Settings.PSKValue = "SecondAutomationPSK";


                EwsWrapper.Instance().CreateRule(ruleSettingsSecondary, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CtcUtility.CreateIPsecRule(ruleSettingsPrimary, true);

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                TraceFactory.Logger.Info("Validating the Printer accessibility in Secondary Client, which will fail since the Preshared Key is different");
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);

                // Enabling only private and domain profiles
                connectivityService.Channel.CreateIPsecRule(ruleSettingsSecondary, enableRule: true, enableProfiles: false);
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateAndDomain, true);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Info("Eventhough the Preshared key is different for both rules, still we are able to access the Printer");
                    return false;
                }

                TraceFactory.Logger.Info("Since the Preshared Key is different, fail to access the Printer from Secondary Client");
                TraceFactory.Logger.Info("Deleting Ipsec Rules from Secodary client");
                connectivityService.Channel.DeleteAllIPsecRules();

                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Test no: 670714
        /// 1.	Create IPsec rule on printer with custom address template -specific IPv4 address, all services and IPsec template with Dynamic IKv1, High strength, certificates authentication mode. Set ID certificate as Network Identity Certificate.
        ///	2.	Enable rule and Failsafe option. Set Default action to ‘Drop’.
        ///	3.	Install certificates on client 1.
        ///	4.	Create similar rule on client machine and Ping to printer with IPv4 address, it should work.
        ///	5.	Create 1 more rule with similar settings as mentioned in Step 1 but change the specific IPv4 address to Client 2 IPv4 address and use a different set of certificates. Set ID certificate as Network Identity Certificate.
        ///	6.	Install certificates on client 2.
        ///	7.	Create similar rule on secondary client and ping to printer with IPv4 address, it should work.
        ///	8.	Create 1 more rule with similar settings as mentioned in Step 1 but change the specific IPv4 address to Client 1 IPv4 address and use a different set of certificates. Set ID certificate as Network Identity Certificate.
        ///	9.	Install certificates on client 1.
        ///	10.	Create similar rule on client machine and Ping to printer with IPv4 address, it should work.
        ///	Note: With Medium Encryption etrength , the des was not working in secondary client and updated encryption strength with High after discussing with AMitha dated 18/5/2016
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyPrinterAccessibilityWithMultipleCertificates(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string clientAddress = NetworkUtil.GetLocalAddress(activityData.PrimaryDhcpServerIPAddress, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo + "0", CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, clientAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                SecurityRuleSettings ruleSettingsPrimary = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.HighInteroperabilityLowsecurity);

                ruleSettingsPrimary.IPsecTemplate.Name = ruleSettingsPrimary.IPsecTemplate.Name + "0";
                ruleSettingsPrimary.Name = ruleSettingsPrimary.Name + "0";

                EwsWrapper.Instance().CreateRule(ruleSettingsPrimary, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CtcUtility.CreateIPsecRule(ruleSettingsPrimary, true);

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString()))
                {
                    return false;
                }

                // Creating Rule on Secondary Client
                addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo + "1", CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, activityData.WindowsSecondaryClientIPAddress);
                SecurityRuleSettings ruleSettingsSecondary = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.HighInteroperabilityLowsecurity, CertificateInstall.ValidSet2, installCertificate: false);

                ruleSettingsSecondary.IPsecTemplate.Name = ruleSettingsSecondary.IPsecTemplate.Name + "1";
                ruleSettingsSecondary.Name = ruleSettingsSecondary.Name + "1";

                // Disabling ipsec since 1st rule will not allow to proceed further while finalizing the rule (browser goes to infinite loop)
                CtcUtility.DeleteAllIPsecRules();
                EwsWrapper.Instance().SetIPsecFirewall(false);
                EwsWrapper.Instance().CreateRule(ruleSettingsSecondary, true);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);

                connectivityService.Channel.InstallCACertificate(CACERTIFICATE_SET2);
                connectivityService.Channel.InstallIDCertificate(IDCERTIFICATE_SET2, IDCERTIFICATE_PSWD_SET2);

                connectivityService.Channel.CreateIPsecRule(ruleSettingsSecondary, enableRule: true, enableProfiles: false);
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                TraceFactory.Logger.Info("Performing Ping from secondary client.");

                bool pingStatus = connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address));
                TraceFactory.Logger.Info("Deleting Ipsec Rules from Secodary client");
                connectivityService.Channel.DeleteAllIPsecRules();

                if (!pingStatus)
                {
                    TraceFactory.Logger.Info("Ping from secondary client failed.");
                    return false;
                }

                TraceFactory.Logger.Info("Ping from secondary client is successful.");

                addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo + "2", CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, clientAddress);
                ruleSettingsPrimary = GetCertificatesRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.HighInteroperabilityLowsecurity, CertificateInstall.ValidSet2);

                ruleSettingsPrimary.IPsecTemplate.Name = ruleSettingsPrimary.IPsecTemplate.Name + "2";
                ruleSettingsPrimary.Name = ruleSettingsPrimary.Name + "2";

                // Disabling ipsec since 1st rule will not allow to proceed further while finalizing the rule (browser goes to infinite loop)
                EwsWrapper.Instance().SetIPsecFirewall(false);
                EwsWrapper.Instance().CreateRule(ruleSettingsPrimary);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = Guid.Empty;
                    packetLocation = string.Empty;
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CtcUtility.CreateIPsecRule(ruleSettingsPrimary, true);

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                return ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString());
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, deleteCertificates: true);
            }
        }

        /// <summary>
        /// Test no: 670898
        /// 1.	Create rule on printer with All Address, All Services and IPsec template with following settings: Dynamic IKEv1, Certification authentication mode with medium strength.
        ///	2.	Install both ID and CA certificate on client.
        ///	3.	Enable rule and Failsafe option. 
        ///	4.	Set default action as “Drop”.
        ///	5.	Create similar rule on client machine.
        ///	6.	Ping to printer with IPv4 address, it should be successful.
        ///	7.	With above rules active, install different ID certificate and tag as Network Identity Certificate.
        ///	8.	Ping to printer with IPv4 address, it should be successful since connection will be active till SA lifetime expires.
        ///	9.	Install different CA certificate on printer.
        ///	10.	Ping to printer with IPv4 address, it should not be successful since upon installation of CA certificate, re-authentication occurs.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyDifferentIDCACertificate(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                SecurityRuleSettings ruleSettings = GetCertificatesRuleSettings(testNo, DefaultAddressTemplates.AllIPAddresses, DefaultServiceTemplates.AllServices, IKESecurityStrengths.MediumInteroperabilityMediumsecurity);

                // Once rule is created, share folder can't be accessed. Copying required certificates 
                string idCertificatePath = Path.Combine(Path.GetTempPath(), "ID_certificateset2.pfx");
                string caCertificatePath = Path.Combine(Path.GetTempPath(), "CA_certificateset2.cer");

                File.Copy(IDCERTIFICATE_SET2, idCertificatePath, true);
                File.Copy(CACERTIFICATE_SET2, caCertificatePath, true);

                EwsWrapper.Instance().CreateRule(ruleSettings, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CtcUtility.CreateIPsecRule(ruleSettings, true);

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString()))
                {
                    return false;
                }

                try
                {
                    guid = Guid.Empty;
                    packetLocation = string.Empty;
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    EwsWrapper.Instance().InstallIDCertificate(idCertificatePath, IDCERTIFICATE_PSWD_SET2);

                    TraceFactory.Logger.Info("Ping after new ID certificate installation.");

                    if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                if (!ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString()))
                {
                    return false;
                }

                EwsWrapper.Instance().InstallCACertificate(caCertificatePath, validate: false);

                TraceFactory.Logger.Info("Ping after new CA certificate installation.");

                return CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Fail);
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Test no: 678824
        /// 1.	Create rule on printer with Custom address template: Specific IPv4 Wired address, all services and IPsec template with Dynamic IKEv1, PSK with Medium strength.
        ///	2.	Enable rule and Failsafe option. 
        ///	3.	Set default action as “Drop”.
        ///	4.	Create similar rule on client machine. 
        ///	5.	Send print job (p9100/FTP) and discover printer with any other IP addresses of the printer.
        ///	6.	Both services should be successful.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyServicesWithDifferentIPAddresses(IPSecurityActivityData activityData, int testNo)
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

            Guid guid = Guid.Empty;
            string packetLocation = string.Empty;
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string clientAddress = NetworkUtil.GetLocalAddress(activityData.PrimaryDhcpServerIPAddress, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, clientAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                SecurityRuleSettings ruleSettings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.HighInteroperabilityLowsecurity);

                EwsWrapper.Instance().CreateRule(ruleSettings, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                try
                {
                    guid = StartPacketCapture(activityData.WiredIPv4Address, testNo);

                    CtcUtility.CreateIPsecRule(ruleSettings, true);

                    Printer.Printer validationPrinterObject = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.EuropaWiredIPv4Address);

                    TraceFactory.Logger.Info("Validating p9100 and discovery with Europa Wired IP Address: {0}".FormatWith(activityData.EuropaWiredIPv4Address));

                    if (!CtcUtility.ValidateDeviceServices(validationPrinterObject, p9100: DeviceServiceState.Pass, wsd: DeviceServiceState.Pass,
                                                         printerDriver: activityData.PrintDriverLocation, printerDriverModel: activityData.PrintDriverModel))
                    {
                        return false;
                    }
                }
                finally
                {
                    packetLocation = StopPacketCapture(guid);
                }

                return ValidatePackets(packetLocation, activityData.WiredIPv4Address, NetworkUtil.GetLocalAddress(activityData.WiredIPv4Address, IPAddress.Parse(activityData.WiredIPv4Address).GetSubnetMask().ToString()).ToString());
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Test no: 729286
        /// 1.	Step 1: Different Subnets using Inbound communications
        ///		(i)		Create rule on printer with Specific Address, all Services and IPsec template with Dynamic IKEv1, PSK with medium strength. (Use 1st interface IP Address)
        ///		(ii)	Enable rule and Failsafe option. Set default action as “Drop”.
        ///		(iii)	Create similar rule on Client 1.
        ///		(iv)	Ping with IPv4 and IPv6, FTP with IPv4 from Client 1.
        ///		(v)		All services should be successful.
        ///		(vi)	Delete all rules.
        ///		(vii)	Repeat Step 1 to Step 5 with different interface on Client 2.
        ///	2.	Step 2: Different Subnets using Outbound communications
        ///		(i)		Create rule on printer with Specific Address, all Services and IPsec template with Dynamic IKEv1, PSK with medium strength. (Use 1st interface IP Address)
        ///		(ii)	Enable rule and Failsafe option. Set default action as “Drop”.
        ///		(iii)	Create similar on Dhcp server.
        ///		(iv)	Ping from Dhcp server, it should be successful.
        ///		(v)		Start Packet capture.
        ///		(vi)	Enable DDNS option on printer.
        ///		(vii)	Stop packet capture and validate packets. 
        ///		(viii)	Repeat Step 1 to Step 7 for different interface.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyIn_OutBoundCommunication(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Step 1: Different Subnets using Inbound communications");

                string clientAddress = NetworkUtil.GetLocalAddress(activityData.PrimaryDhcpServerIPAddress, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, clientAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                SecurityRuleSettings ruleSettings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.HighInteroperabilityLowsecurity);

                EwsWrapper.Instance().CreateRule(ruleSettings, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                CtcUtility.CreateIPsecRule(ruleSettings, true);

                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                {
                    return false;
                }

                CtcUtility.DeleteAllIPsecRules();
                EwsWrapper.Instance().DeleteAllRules();

                addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.EuropaWiredIPv4Address, activityData.WindowsSecondaryClientIPAddress);
                serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                ruleSettings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.HighInteroperabilityLowsecurity);

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.EuropaWiredIPv4Address);
                EwsWrapper.Instance().CreateRule(ruleSettings, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);

                connectivityService.Channel.CreateIPsecRule(ruleSettings, enableRule: true, enableProfiles: false);
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                TraceFactory.Logger.Info("Performing Ping from secondary client.");

                bool pingStatus = connectivityService.Channel.Ping(IPAddress.Parse(activityData.EuropaWiredIPv4Address));
                TraceFactory.Logger.Info("Deleting Ipsec Rules from Secodary client");
                connectivityService.Channel.DeleteAllIPsecRules();
                EwsWrapper.Instance().DeleteAllRules();

                if (!pingStatus)
                {
                    TraceFactory.Logger.Info("Ping from secondary client failed.");
                    return false;
                }

                TraceFactory.Logger.Info("Ping is successful from secondary client.");
                TraceFactory.Logger.Info("Step 2: Different Subnets using Outbound communications");

                addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, activityData.PrimaryDhcpServerIPAddress);
                serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                ruleSettings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.HighInteroperabilityLowsecurity);
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);

                EwsWrapper.Instance().CreateRule(ruleSettings, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                connectivityService = ConnectivityUtilityServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);

                connectivityService.Channel.CreateIPsecRule(ruleSettings, enableRule: true, enableProfiles: false);
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                TraceFactory.Logger.Info("Performing Ping from Primary DHCP server.");

                pingStatus = connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address));
                TraceFactory.Logger.Info("Deleting Ipsec Rules from Secodary client");
                connectivityService.Channel.DeleteAllIPsecRules();

                if (!pingStatus)
                {
                    TraceFactory.Logger.Info("Ping failed from Primary DHCP server.");
                    return false;
                }

                TraceFactory.Logger.Info("Ping is successful from Primary DHCP server.");

                // TODO: PC

                EwsWrapper.Instance().SetDDNS(true);

                // TODO: PC

                EwsWrapper.Instance().DeleteAllRules();

                addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.EuropaWiredIPv4Address, activityData.SecondDhcpServerIPAddress);
                addressTemplateSettings.Name = addressTemplateSettings.Name + "2";
                serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                ruleSettings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.HighInteroperabilityLowsecurity);
                ruleSettings.IPsecTemplate.Name = ruleSettings.IPsecTemplate.Name + "2";

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.EuropaWiredIPv4Address);
                EwsWrapper.Instance().CreateRule(ruleSettings, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                connectivityService = ConnectivityUtilityServiceClient.Create(activityData.SecondDhcpServerIPAddress);

                connectivityService.Channel.CreateIPsecRule(ruleSettings, enableRule: true, enableProfiles: false);
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                TraceFactory.Logger.Info("Performing Ping from Secondary DHCP server.");

                pingStatus = connectivityService.Channel.Ping(IPAddress.Parse(activityData.EuropaWiredIPv4Address));
                TraceFactory.Logger.Info("Deleting Ipsec Rules from Secodary client");
                connectivityService.Channel.DeleteAllIPsecRules();

                if (!pingStatus)
                {
                    TraceFactory.Logger.Info("Ping failed from Secondary DHCP server.");
                    return false;
                }

                TraceFactory.Logger.Info("Ping is successful from Secondary DHCP server.");

                // TODO: PC

                EwsWrapper.Instance().SetDDNS(true);

                // TODO: PC

                EwsWrapper.Instance().DeleteAllRules();
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);

                return true;
            }
            catch (Exception ipSecException)
            {
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
		/// Template for All IPAddress and All Service to test maximun rule creation
		/// </summary>
		/// <param name="activityData"></param>
		/// <returns></returns>
		public static bool VerifyMaximumNoOfRule(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Allow Rule, Default: Drop
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings();
                addressTemplate.DefaultTemplate = DefaultAddressTemplates.AllIPAddresses;

                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings();
                serviceTemplate.DefaultTemplate = DefaultServiceTemplates.AllServices;

                SecurityRuleSettings securitySettings = new SecurityRuleSettings();
                securitySettings.AddressTemplate = addressTemplate;
                securitySettings.ServiceTemplate = serviceTemplate;
                securitySettings.Action = IPsecFirewallAction.AllowTraffic;
                try
                {
                    for (int i = 0; i < 5; i++)
                    {
                        EwsWrapper.Instance().CreateRule(securitySettings, true, true);
                    }
                }
                catch (Exception maximumRuleException)
                {
                    TraceFactory.Logger.Debug(maximumRuleException.Message);
                }

                if (PrinterFamilies.InkJet.ToString().EqualsIgnoreCase(activityData.ProductFamily))
                {
                    if (!EwsWrapper.Instance().CheckMaxRules())
                    {
                        return false;

                    }
                }
                return true;

            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                // clean up the printer to default state
                TestPostRequisites(activityData);
            }
        }


        /// <summary>
        /// Test no: 729287		
        ///	1.	Create one more rule on printer with specific address, all services and IPsec template with Dynamic IKEv1, different PSK authentication mode and Medium strength.
        ///	2.	Create one more rule on printer with specific address, all services and IPsec template with Dynamic IKEv1, different PSK authentication mode and High strength.		
        ///	3.	Create all above rules on client 1, client 2.
        ///	4.	From 2 machines simultaneously ping to IPv4, Link local, Stateful and 4 Stateless addresses of the printer.
        ///	5.	Ping for all the addresses from both clients should be successful.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyPrinterAccessiblityFromMultipleClients(IPSecurityActivityData activityData, int testNo)
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

            TraceFactory.Logger.Info("This test case is not in QC, with Arul input retained the test case with new Test case ID, previous id:729287");
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                EwsWrapper.Instance().SetIPv6(true);
                EwsWrapper.Instance().SetDHCPv6OnStartup(true);

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.EuropaWiredIPv4Address);
                EwsWrapper.Instance().SetIPv6(true);
                EwsWrapper.Instance().SetDHCPv6OnStartup(true);
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);

                IPAddress ipv4Address = IPAddress.Parse(activityData.WiredIPv4Address);
                IPAddress statefulAddress = printer.IPv6StateFullAddress;

                Collection<IPAddress> localPingAddresses = new Collection<IPAddress>();
                localPingAddresses.Add(ipv4Address);
                //localPingAddresses.Add(statefulAddress);				

                Printer.Printer remotePrinter = PrinterFactory.Create(activityData.ProductFamily, activityData.EuropaWiredIPv4Address);

                ipv4Address = IPAddress.Parse(activityData.EuropaWiredIPv4Address);
                statefulAddress = remotePrinter.IPv6StateFullAddress;

                Collection<IPAddress> remotePingAddresses = new Collection<IPAddress>();
                remotePingAddresses.Add(ipv4Address);
                //remotePingAddresses.Add(statefulAddress);						

                string remoteAddress = NetworkUtil.GetLocalAddress(activityData.PrimaryDhcpServerIPAddress, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, remoteAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                SecurityRuleSettings localRuleSettings1 = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.MediumInteroperabilityMediumsecurity);
                SecurityRuleSettings localRuleSettings2 = GetPSKRuleSettings(testNo, DefaultAddressTemplates.AllIPv6Addresses, DefaultServiceTemplates.AllServices, IKESecurityStrengths.MediumInteroperabilityMediumsecurity);
                localRuleSettings2.IPsecTemplate.Name = localRuleSettings2.IPsecTemplate.Name + "1";

                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);

				remoteAddress = connectivityService.Channel.GetLocalAddress(activityData.SecondDhcpServerIPAddress, IPAddress.Parse(activityData.SecondDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();
				addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo+"1", CustomAddressTemplateType.IPAddress, activityData.EuropaWiredIPv4Address, remoteAddress);

				SecurityRuleSettings remoteRuleSettings1 = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.HighInteroperabilityLowsecurity);
                remoteRuleSettings1.IPsecTemplate.Name = remoteRuleSettings1.IPsecTemplate.Name + "11";
                SecurityRuleSettings remoteRuleSettings2 = GetPSKRuleSettings(testNo, DefaultAddressTemplates.AllIPv6Addresses, DefaultServiceTemplates.AllServices, IKESecurityStrengths.HighInteroperabilityLowsecurity);
				remoteRuleSettings2.IPsecTemplate.Name = remoteRuleSettings2.IPsecTemplate.Name + "111";

                EwsWrapper.Instance().CreateRule(localRuleSettings1, true);
                EwsWrapper.Instance().CreateRule(localRuleSettings2, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                EwsWrapper.Instance().ChangeDeviceAddress(activityData.EuropaWiredIPv4Address);
                EwsWrapper.Instance().CreateRule(remoteRuleSettings1, true);
                EwsWrapper.Instance().CreateRule(remoteRuleSettings2, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                connectivityService.Channel.CreateIPsecRule(remoteRuleSettings1, enableRule: true, enableProfiles: false);
                connectivityService.Channel.CreateIPsecRule(remoteRuleSettings2, enableRule: true, enableProfiles: false);
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);

                Thread.Sleep(TimeSpan.FromMinutes(1));

                TimeSpan pingTimeOut = TimeSpan.FromMinutes(2);
                connectivityService.Channel.PingContinuously(remotePingAddresses, pingTimeOut);

                CtcUtility.CreateIPsecRule(localRuleSettings1);
                CtcUtility.CreateIPsecRule(localRuleSettings2);

                Thread primaryPing = new Thread(() => CtcUtility.PingContinuously(localPingAddresses, pingTimeOut));

                primaryPing.Start();

                Thread.Sleep(pingTimeOut.Add(TimeSpan.FromSeconds(30)));

                primaryPing.Abort();

                bool pingStatus = true;

                // Deleting all rules on primary client machine to access secondary client
                CtcUtility.DeleteAllIPsecRules();
                Collection<PingStatics> primaryStatics = CtcUtility.GetContinuousPingStatistics();
                Collection<PingStatics> secondaryStatics = connectivityService.Channel.GetContinuousPingStatistics();

                TraceFactory.Logger.Debug("Ping statics of primary client:");

                foreach (PingStatics ping in primaryStatics)
                {
                    TraceFactory.Logger.Debug(ping.Message);
                    pingStatus &= ping.Status == IPStatus.Success;
                }

                TraceFactory.Logger.Debug("Ping statics of secondary client:");

                foreach (PingStatics ping in secondaryStatics)
                {
                    TraceFactory.Logger.Debug(ping.Message);
                    pingStatus &= ping.Status == IPStatus.Success;
                }

                EwsWrapper.Instance().DeleteAllRules();
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);

                if (pingStatus)
                {
                    TraceFactory.Logger.Info("Ping with Ipv4 addresses are successful from primary and secondary clients.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Ping with Ipv4 addresses is not successful from primary and secondary clients.");
                    return false;
                }
            }
            catch (Exception ipSecException)
            {
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.EuropaWiredIPv4Address);
                EwsWrapper.Instance().DeleteAllRules();
                EwsWrapper.Instance().ChangeDeviceAddress(activityData.WiredIPv4Address);

                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, secondaryCleanup: true);
            }
        }

        /// <summary>
        /// Test Id: 517170
        /// 1.	Create rule with All Address, All Services and IPsec template with Dynamic IKEv1 settings – PSK and Medium strength.
        /// 2.	Enable rule and Failsafe option. Set Default action to ‘Drop’.
        /// 3.	Create similar rule on client machine and DNS server.
        /// 4.	Add a DNS ‘A’ record for client IPv4 address on DNS server.
        /// 5.	Start Packet Capture on DNS server.
        /// 6.	Configure a quickset for Scan to Network from EWS and create a share folder on client machine as a pre-requisite step.
        /// 7.	From Control Panel, traverse to Scan to Network Folder and scan a document to configured quickset.
        /// 8.	Stop Packet Capture on DNS server.
        /// 9.	Check if the document scanned exists in the shared folder.
        /// 10.	Validate packets captured. Filter and search items <TBD>.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyScantoNetworkFolder(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                string clientAddress = NetworkUtil.GetLocalAddress(activityData.PrimaryDhcpServerIPAddress, IPAddress.Parse(activityData.PrimaryDhcpServerIPAddress).GetSubnetMask().ToString()).ToString();
                SecurityRuleSettings ruleSettings = GetPSKRuleSettings(testNo, DefaultAddressTemplates.AllIPAddresses, DefaultServiceTemplates.AllServices, IKESecurityStrengths.MediumInteroperabilityMediumsecurity);

                EwsWrapper.Instance().CreateRule(ruleSettings, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                CtcUtility.CreateIPsecRule(ruleSettings, true);

                //DnsApplicationServiceClient dnsClient = DnsApplicationServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                //dnsClient.Channel.AddRecord(EwsWrapper.Instance().GetDomainName(), EwsWrapper.Instance().GetHostname(), "A", clientAddress);

                //ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.PrimaryDhcpServerIPAddress);
                //connectivityService.Channel.CreateIPsecRule(ruleSettings, enableRule: true, enableProfiles: true);                

                // TODO: PC - Server

                if (!CtcUtility.ScanToNetworkFolder(IPAddress.Parse(activityData.WiredIPv4Address), QUICK_SET_NAME, SHARE_FILE_PATH, testNo))
                {
                    return false;
                }

                // TODO: PC

                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Verify adding custom services using new Service Template.
        /// Step 1: No IPsec rule on the Printer and Client, Printer Default IPsec rule action is Drop. Verify the IPSec connection.
        /// 1.	Create IPSec rule with All IP, Custom managed service(HTTP), PSK on printer and client.        
        /// 2.	Set the Default rule action to “Drop” on the printer. 
        /// 4.	Enable IPsec Policy on the printer.
        /// 5.	From client http should be accessible.              
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyAddingCustomServicesUsingNewServiceTemplate(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                Service defaultCustomServiceHTTP = new Service();
                defaultCustomServiceHTTP.IsDefault = false;
                defaultCustomServiceHTTP.Name = "P80";
                defaultCustomServiceHTTP.Protocol = ServiceProtocolType.TCP;
                defaultCustomServiceHTTP.PrinterPort = "80";
                defaultCustomServiceHTTP.ServiceType = ServiceType.Printer;
                defaultCustomServiceHTTP.RemotePort = "Any";

                Collection<Service> ipSecServiceP80 = new Collection<Service>();
                ipSecServiceP80.Add(defaultCustomServiceHTTP);

                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(SERVICETEMPLATENAME + testNo, ipSecServiceP80);
                IKEPhase1Settings phase1 = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.SHA1, 28800);
                IKEPhase2Settings phase2 = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, 2800, 600000);

                // Get the Custom Service Template settings rule
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings);
                TraceFactory.Logger.Info("Creating IPSec Rule with custom services");
                EwsWrapper.Instance().CreateRule(settings, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);
                //Creating IPSec rule on the client                
                if (!CtcUtility.CreateIPsecRule(settings))
                {
                    return false;
                }
                TraceFactory.Logger.Info("IPSec Rule with custom services created");

                TraceFactory.Logger.Info("Validating the rule HTTP session.");
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, http: DeviceServiceState.Pass))
                {
                    return false;
                }

                return true;
            }

            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }

            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData);
            }
        }

        /// <summary>
        /// Verify Multiple IPSec rules with different services.
        /// Step 1: No IPsec rule on the Printer and Client, Printer Default IPsec rule action is Drop.
        /// 1.	Create 1st IPSec rule with All IP, All Print Services with SHA1/SHA2, AES128/AES256 with PSK
        /// 2.	Create 2nd IPSec rule with All IP, All Management Services with SHA1/SHA2, AES128/AES256 with PSK        
        /// 3.	Set the Default rule action to “Drop” on the printer. 
        /// 4.	Enable IPsec Policy on the printer.
        /// 5.	From client give a print job and http should be accessible.
        /// 6.  Delete the 1st IPSec rule.
        /// 7.  From client HTTP, SNMP should be accessible and print job should fail. 
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyMultipleRulesWithDifferentServices(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(DefaultAddressTemplates.AllIPAddresses);
                ServiceTemplateSettings serviceTemplateSettings1 = new ServiceTemplateSettings(DefaultServiceTemplates.AllPrintServices);
                ServiceTemplateSettings serviceTemplateSettings2 = new ServiceTemplateSettings(DefaultServiceTemplates.AllManagementServices);
                IKEPhase1Settings phase1 = new IKEPhase1Settings(DiffieHellmanGroups.DH14, Encryptions.AES128, Authentications.SHA1, 28800);
                IKEPhase2Settings phase2 = new IKEPhase2Settings(EncapsulationType.Transport, Authentications.SHA1, Encryptions.AES128, 36000, 0, false);
                phase2.AdvancedIKESettings = null;

                // Get the Custom Service Template settings rule
                SecurityRuleSettings settings1 = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings1, IKESecurityStrengths.Custom, phase1, phase2);
                SecurityRuleSettings settings2 = GetPSKRuleSettings(testNo, addressTemplateSettings, serviceTemplateSettings2, IKESecurityStrengths.Custom, phase1, phase2);
                settings2.IPsecTemplate.Name = "Second_rule";
                TraceFactory.Logger.Info("Creating 1st IPSec Rule");
                EwsWrapper.Instance().CreateRule(settings1, true, true);
                //Creating IPSec rule on the client
                if (!CtcUtility.CreateIPsecRule(settings1))
                {
                    return false;
                }
                TraceFactory.Logger.Info("1st IPSec Rule created");

                TraceFactory.Logger.Info("Creating 2nd IPSec Rule");
                EwsWrapper.Instance().CreateRule(settings2, true, true);
                if (!CtcUtility.CreateIPsecRule(settings2))
                {
                    return false;
                }
                TraceFactory.Logger.Info("2nd IPSec Rule created");

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);


                TraceFactory.Logger.Info("Validating the rule with P9100 print and HTTP session.");
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, p9100: DeviceServiceState.Pass, http: DeviceServiceState.Pass, isPingRequiredP9100Install: false, printerDriver: activityData.PrintDriverLocation, printerDriverModel: activityData.PrintDriverModel))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Deleting the 1st IPSec Rule");
                // ##########Check if any modification is required with respect to inkjet #########
                if (!EwsWrapper.Instance().DeleteRule(settings1.IPsecTemplate.Name))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validating the rule with P9100 print, HTTP & SNMP session.");
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, p9100: DeviceServiceState.Fail, http: DeviceServiceState.Pass, snmp: DeviceServiceState.Pass, isPingRequiredP9100Install: false, printerDriver: activityData.PrintDriverLocation, printerDriverModel: activityData.PrintDriverModel))
                {
                    return false;
                }
            }

            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }

            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData);
            }

            return true;
        }

        #endregion

        #region Linux

        /*	General procedure for Linux tests:
		 * 1. Create 2 rule on Linux machine, for creating rule: 3 files are configured - PSK, Racoon, IPSec.
		 *	a. Rule 1 - Linux-Printer rule: test specific rule. 
		 *	b. Rule 2 - Linux-Windows rule: communication between machine.		 
		 * 2. Create rule on Windows for communication with Linux.
		 * 3. Communicate with Linux and validate Ping service.
		 * Note: Before creating rule on linux, printer address needs to be pinged continously; for validation to work. (If this is not done, ping is not working after rule creation)
		 */

        /// <summary>
        /// Test no: 729263
        /// 1.	Create rule on printer with Custom specific IPv6 Address, All Service and IPsec configuration: Dynamic IKEv1 settings with PSK authentication mode and Medium strength.
        ///	2.	Enable rule and set default rule to ‘Drop’. Enable fail safe option.
        ///	3.  Create rule on Windows and Linux for contacting each other.
        ///	4.	Create similar rule on Linux client machine.
        ///	5.	Ping with printer stateless address from linux.
        ///	6.	Ping should be successful.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyAccessbilityWithIPv6Address(IPSecurityActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData, true))
            {
                return false;
            }

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            string ipv6StatelessAddress = printer.IPv6StatelessAddresses[0].ToString();

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Enabling IPV6 option
                EwsWrapper.Instance().SetIPv6(true);

                // Setting option ‘Always perform DHCPv6 on startup’
                EwsWrapper.Instance().SetDHCPv6OnStartup(true);



                IPAddress linuxServerAddress = IPAddress.Parse(activityData.LinuxFedoraClientIPAddress);
                Collection<IPAddress> linuxStatelessAddresses = LinuxUtils.GetStatelessAddress(linuxServerAddress);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                IPAddress linuxStatelessAddress = linuxStatelessAddresses[0];
                // Based on the printer stateless address, get corresponding subnet linux stateless address
                foreach (IPAddress address in linuxStatelessAddresses)
                {
                    if (address.ToString().Split(':')[3].EqualsIgnoreCase(ipv6StatelessAddress.Split(':')[3]))
                    {
                        linuxStatelessAddress = address;
                        break;
                    }
                }

                // Stateless IPaddress are not accessible with loopback address, so enabling again the loopback address
                LinuxUtils.ExecuteCommand(IPAddress.Parse(activityData.LinuxFedoraClientIPAddress), "reboot");
                Thread.Sleep(TimeSpan.FromMinutes(3));

                AddressTemplateSettings addressTemplate = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, ipv6StatelessAddress, linuxStatelessAddress.ToString());
                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                SecurityRuleSettings ruleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.HighInteroperabilityLowsecurity);

                CreateRuleinLinux(testNo, ruleSettings, linuxServerAddress, EncapsulationType.Transport, activityData.WiredIPv4Address, serviceTemplate);

                return PingStatelessAddress(linuxServerAddress, IPAddress.Parse(ipv6StatelessAddress));
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, performLinuxPostrequisites: true, linuxServerAddress: activityData.LinuxFedoraClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, performLinuxPostrequisites: true);
            }
        }

        /// <summary>
        /// Test no: 83051
        /// Step 1: Verify Manual key with AH protocol only.
        ///	1.	Create rule on printer with specific IPv4 Address, All Service and IPsec configuration: 
        ///		(i)	Manual IKE settings.
        ///		(ii) Phase I:
        ///			a)	Encapsulation type: Tunnel.
        ///			b)	Enable AH and select Sha1 authentication.
        ///			c)	Disable ESP.
        ///		(iii) Phase II:
        ///			a)	SPI: Hex format with 100 and 4000 as In and Out values for AH
        ///			b)	Key: ASCII format with 20 character string for In (abcde12345abcde12345) and Out (abcde12345abcde12346) values for Authentication.
        ///			c)	Leave other settings to default.
        ///	2.	Enable rule and set default rule to ‘Drop’. Enable fail safe option.
        ///	3.	Create similar rule on Linux client machine.
        ///	4.	Ping with IPv4 address.
        ///	5.	Ping should be successful.
        ///	Step 2: Verify Manual key with ESP protocol only.
        ///	1.	Create rule on printer with specific IPv4 Address, All Service and IPsec configuration: 
        ///		(i)	Manual IKE settings.
        ///		(ii) Phase I:
        ///			a)	Encapsulation type: Tunnel.
        ///			b)	Enable ESP and select Sha1 authentication.
        ///			c)	Disable AH.
        ///		(iii)	Phase II:
        ///			a)	SPI: Hex format with 100 and 4000 as In and Out values for ESP
        ///			b)	Key: ASCII format with 20 character string for In (abcde12345abcde12345) and Out (abcde12345abcde12346) values for Authentication.
        ///			c)	Leave other settings to default.
        ///	2.	Enable rule and set default rule to ‘Drop’. Enable fail safe option.
        ///	3.	Create similar rule on Linux client machine.
        ///	4.	Ping with IPv4 address.
        ///	5.	Ping should be successful.
        ///	Test no: 83050
        ///	Step 1: Verify Manual key with AH protocol only.
        ///	1.	Create rule on printer with specific IPv4 Address, All Service and IPsec configuration: 
        ///		(i)	Manual IKE settings.
        ///		(ii) Phase I:
        ///			a)	Encapsulation type: Transport.
        ///			b)	Enable AH and select Sha1 authentication.
        ///			c)	Disable ESP.
        ///		(iii) Phase II:
        ///			a)	SPI: Hex format with 100 and 4000 as In and Out values for AH
        ///			b)	Key: ASCII format with 20 character string for In (abcde12345abcde12345) and Out (abcde12345abcde12346) values for Authentication.
        ///			c)	Leave other settings to default.
        ///	2.	Enable rule and set default rule to ‘Drop’. Enable fail safe option.
        ///	3.	Create similar rule on Linux client machine.
        ///	4.	Ping with IPv4 address.
        ///	5.	Ping should be successful.
        ///	Step 2: Verify Manual key with ESP protocol only.
        ///	6.	Create rule on printer with specific IPv4 Address, All Service and IPsec configuration: 
        ///		(i)	Manual IKE settings.
        ///		(ii) Phase I:
        ///			a)	Encapsulation type: Transport.
        ///			b)	Enable ESP and select Sha1 authentication.
        ///			c)	Disable AH.
        ///		(iii) Phase II:
        ///			a)	SPI: Hex format with 100 and 4000 as In and Out values for ESP
        ///			b)	Key: ASCII format with 20 character string for In (abcde12345abcde12345) and Out (abcde12345abcde12346) values for Authentication.
        ///			c)	Leave other settings to default.
        ///	7.	Enable rule and set default rule to ‘Drop’. Enable fail safe option.
        ///	8.	Create similar rule on Linux client machine.
        ///	9.	Ping with IPv4 address.
        ///	10.	Ping should be successful.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyAccessbilityWithManualKeys(IPSecurityActivityData activityData, int testNo, EncapsulationType encapsulationType)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData, true))
            {
                return false;
            }

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                IPAddress linuxServerAddress = IPAddress.Parse(activityData.LinuxFedoraClientIPAddress);

                TraceFactory.Logger.Info("Validation for ESP -Cryptographic Parameter");
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, linuxServerAddress.ToString());
                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);
                BasicManualKeysSettings basicSettings = new BasicManualKeysSettings(Encryptions.DES3, Authentications.SHA1, encapsulationType, activityData.WiredIPv4Address, linuxServerAddress.ToString());
                AdvancedManualKeysSettings advancedSettings = new AdvancedManualKeysSettings(1000, 2000, "123451234512345123451234", "123451234512345123451234", "12345123451234512345", "12345123451234512345");

                SecurityRuleSettings ruleSettings = GetManualRuleSettings(testNo, addressTemplate, serviceTemplate, basicSettings, advancedSettings);
                CreateRuleinLinux(testNo, ruleSettings, linuxServerAddress, encapsulationType, activityData.WiredIPv4Address, serviceTemplate, true);

                if (!PingIPv4Address(linuxServerAddress, IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Info("ESP: Printer IPV4 address is not accessible from Linux Machine");
                    return false;
                }
                TraceFactory.Logger.Info("ESP: Printer IPV4 address is accessible from Linux Machine");

                LinuxUtils.ExecuteCommand(IPAddress.Parse(activityData.LinuxFedoraClientIPAddress), "reboot");
                Thread.Sleep(TimeSpan.FromMinutes(2));

                CtcUtility.DeleteAllIPsecRules();
                EwsWrapper.Instance().DeleteAllRules();

                TraceFactory.Logger.Info("Validation for AH -Cryptographic Parameter");
                basicSettings.AHEnable = true;
                basicSettings.ESPEnable = false;
                basicSettings.AHAuthentication = Authentications.SHA1;

                advancedSettings = new AdvancedManualKeysSettings(1000, 2000, string.Empty, string.Empty, "12345123451234512345", "12345123451234512345");
                advancedSettings.AHSPIIn = 1000;
                advancedSettings.AHSPIOut = 2000;

                ruleSettings = GetManualRuleSettings(testNo, addressTemplate, serviceTemplate, basicSettings, advancedSettings);
                CreateRuleinLinux(testNo, ruleSettings, linuxServerAddress, encapsulationType, activityData.WiredIPv4Address, serviceTemplate, true);

                if (!PingIPv4Address(linuxServerAddress, IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Info("AH: Printer IPV4 address is not accessible from Linux Machine");
                    return false;
                }
                TraceFactory.Logger.Info("AH: Printer IPV4 address is accessible from Linux Machine");

                if (!encapsulationType.Equals(EncapsulationType.Tunnel))
                {
                    LinuxUtils.ExecuteCommand(IPAddress.Parse(activityData.LinuxFedoraClientIPAddress), "reboot");
                    TraceFactory.Logger.Info("Please vaidate whether the loopback address is disabled in linux client machine");
                    Thread.Sleep(TimeSpan.FromMinutes(3));

                    CtcUtility.DeleteAllIPsecRules();
                    EwsWrapper.Instance().DeleteAllRules();

                    TraceFactory.Logger.Info("Validation for ESP and AH Cryptographic Parameter");
                    basicSettings = new BasicManualKeysSettings();
                    basicSettings.ESPEncryption = Encryptions.DES3;
                    basicSettings.AHAuthentication = Authentications.SHA1;
                    basicSettings.Encapsulation = encapsulationType;
                    basicSettings.LocalAddress = activityData.WiredIPv4Address;
                    basicSettings.RemoteAddress = linuxServerAddress.ToString();
                    basicSettings.AHEnable = true;
                    basicSettings.ESPEnable = true;

                    advancedSettings = new AdvancedManualKeysSettings(1000, 2000, "123451234512345123451234", "123451234512345123451234", "12345123451234512345", "12345123451234512345");
                    advancedSettings.AHSPIIn = 1000;
                    advancedSettings.AHSPIOut = 2000;

                    ruleSettings = GetManualRuleSettings(testNo, addressTemplate, serviceTemplate, basicSettings, advancedSettings);
                    CreateRuleinLinux(testNo, ruleSettings, linuxServerAddress, encapsulationType, activityData.WiredIPv4Address, serviceTemplate, true);

                    if (!PingIPv4Address(linuxServerAddress, IPAddress.Parse(activityData.WiredIPv4Address)))
                    {
                        TraceFactory.Logger.Info("ESP and AH: Printer IPV4 address is not accessible from Linux Machine");
                        return false;
                    }
                    TraceFactory.Logger.Info("ESP and AH: Printer IPV4 address is accessible from Linux Machine");
                }
                else
                {
                    TraceFactory.Logger.Info("Since ESP+AH is not working in Tunnel Mode manually also, hence not automated");
                }

                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, performLinuxPostrequisites: true, linuxServerAddress: activityData.LinuxFedoraClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, performLinuxPostrequisites: true);
            }
        }

        /// <summary>
        /// Test no: 83012
        /// 1.	Create rule on printer with Specific IPv6 address, All Service and IPsec configuration: Dynamic IKEv1 settings with PSK authentication mode and Medium strength.
        ///	2.	Enable rule and set default rule to ‘Drop’. Enable fail safe option.
        ///	3.	Create similar rule on Linux client machine.
        ///	4.	Ping with IPv6 Stateful address, ping should be successful from linux machine.
        ///	5.	Create similar rule on Windows client machine.
        ///	6.	Ping with IPv6 Stateful address, ping will fail.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyIPv6AccessbilityFromTwoMachines(IPSecurityActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData, true))
            {
                return false;
            }

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            string ipv6StatelessAddress = printer.IPv6StatelessAddresses[0].ToString();

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // Enabling IPV6 option
                EwsWrapper.Instance().SetIPv6(true);

                // Setting option ‘Always perform DHCPv6 on startup’
                EwsWrapper.Instance().SetDHCPv6OnStartup(true);



                IPAddress linuxServerAddress = IPAddress.Parse(activityData.LinuxFedoraClientIPAddress);
                Collection<IPAddress> linuxStatelessAddresses = LinuxUtils.GetStatelessAddress(linuxServerAddress);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                IPAddress linuxStatelessAddress = linuxStatelessAddresses[0];

                foreach (IPAddress address in linuxStatelessAddresses)
                {
                    if (address.ToString().Split(':')[3].EqualsIgnoreCase(ipv6StatelessAddress.Split(':')[3]))
                    {
                        linuxStatelessAddress = address;
                        break;
                    }
                }

                AddressTemplateSettings addressTemplate = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, ipv6StatelessAddress, linuxStatelessAddress.ToString());
                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                SecurityRuleSettings ruleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.HighInteroperabilityLowsecurity);

                CreateRuleinLinux(testNo, ruleSettings, linuxServerAddress, EncapsulationType.Transport, activityData.WiredIPv4Address, serviceTemplate);

                if (!PingStatelessAddress(linuxServerAddress, IPAddress.Parse(ipv6StatelessAddress)))
                {
                    return false;
                }

                CtcUtility.CreateIPsecRule(ruleSettings);

                if (printer.PingUntilTimeout(IPAddress.Parse(ipv6StatelessAddress), TimeSpan.FromMinutes(1)))
                {
                    TraceFactory.Logger.Info("Printer");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, performLinuxPostrequisites: true, linuxServerAddress: activityData.LinuxFedoraClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData, performLinuxPostrequisites: true);
            }
        }

        /// <summary>
        /// Test no: 505908
        /// 1.	Create IPsec rule on printer with Custom specific IPv4 Address, All Services and IPsec template with following settings:
        ///		(i)	Dynamic with IKEv1
        ///		(ii) Strength – Medium
        ///		(iii) Authentication – Pre shared key
        ///	2.	Enable rule and Failsafe option, set default action to Drop.
        ///	3.	Create rule on client machine with following settings Specific IPv4 Address, All Services, IPsec template with Dynamic IKEv1, Pre shared authentication mode, Custom strength. Phase I and Phase II settings are mentioned below:		
        ///		(i) Phase I – AES128 and Sha2, Phase II – ESP –AES, Sha2_384.
        ///		(ii) Phase I – 3DES and Sha_512, Phase II – ESP –3DES, Sha_512.
        ///		(iii) Phase I – AES_256 and MD5, Phase II – ESP –AES_256, MD5.
        ///		(iv) Phase I – AES_256 and Sha1, Phase II – ESP – AES_256, Sha1.
        ///		(v) Phase I – AES_256 and Sha2_512, Phase II – ESP – AES_256, Sha2_512.
        ///	4.	For the above step, each time the rule is created, ping to printer IPv4 address. It should be successful. Once this step is performed, delete the existing rule and move to next rule settings. Perform Step 4 till all the IPsec configurations are specified in Step 3 are completed.
        /// </summary>
        /// <param name="activityData"></param>
        /// <param name="testNo"></param>
        /// <returns></returns>
        public static bool VerifyAccessbilityWithDifferentAuthentication(IPSecurityActivityData activityData, int testNo)
        {
            if (null == activityData)
            {
                TraceFactory.Logger.Error("Parameter activityData can not be null.");
                return false;
            }

            if (!TestPreRequisites(activityData, true))
            {
                return false;
            }

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                IPAddress linuxServerAddress = IPAddress.Parse(activityData.LinuxFedoraClientIPAddress);
                IPAddress printerAddress = IPAddress.Parse(activityData.WiredIPv4Address);

                AddressTemplateSettings addressTemplate = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, linuxServerAddress.ToString());
                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                SecurityRuleSettings ruleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.HighInteroperabilityLowsecurity);

                EwsWrapper.Instance().CreateRule(ruleSettings, true);
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
                EwsWrapper.Instance().SetIPsecFirewall(true);

                if (!VerifyPingWithAuthenticationEncryption(linuxServerAddress, activityData.WiredIPv4Address, testNo, ruleSettings, Encryptions.AES128, Authentications.SHA1))
                {
                    return false;
                }

                if (!VerifyPingWithAuthenticationEncryption(linuxServerAddress, activityData.WiredIPv4Address, testNo, ruleSettings, Encryptions.DES3, Authentications.SHA512))
                {
                    return false;
                }

                if (!VerifyPingWithAuthenticationEncryption(linuxServerAddress, activityData.WiredIPv4Address, testNo, ruleSettings, Encryptions.AES256, Authentications.MD5))
                {
                    return false;
                }

                if (!VerifyPingWithAuthenticationEncryption(linuxServerAddress, activityData.WiredIPv4Address, testNo, ruleSettings, Encryptions.AES256, Authentications.SHA1))
                {
                    return false;
                }

                if (!VerifyPingWithAuthenticationEncryption(linuxServerAddress, activityData.WiredIPv4Address, testNo, ruleSettings, Encryptions.AES256, Authentications.SHA512))
                {
                    return false;
                }

                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData);
            }
        }

        #endregion

        #region FIPS
        /// <summary>
        /// note: while configuring kerberos if test case throws exception with des, then no update required if it return false den need to change return type of createrule method and pdate accordingly
        /// Verify IP-sec Kerberos authentication in FIPS mode[577016]
        ///1.	Enable FIPS on the device.
        ///2.	Create IPsec rule on printer with All IP Address, All Services and IPsec template with following settings:
        ///(i)	Dynamic with IKEv1 Phase1- DES and MD5
        ///(ii)	Strength – Medium
        ///(iii)	 Authentication- Kerberos
        ///3.	Enable rule and Failsafe option, set default action to Drop 
        ///4.	Kerberos Authentication should fail in FIPS mode.
        ///5.	Disable FIPS and Validate Kerberos Authentication.
        ///6.	Kerberos Authentication should pass in non FIPS mode.
        ///7.	Delete the rule created in the printer.
        ///8.	Enable FIPS in device.
        ///9.	Create IPsec rule on printer with All IP Address, All Services and IPsec template with following settings:
        ///(i)	Dynamic with IKEv1 Phase1- AES128 and SHA1
        ///(ii)	Strength – Medium
        ///(iii)	 Authentication- Kerberos
        ///10.	Create similar rule in client and validate Ping.
        ///11.	Ping should pass.
        ///12.	Disable the FIPS in device and validate Ping.
        ///13.	Ping should pass.
        ///14.	Delete the IP-Sec rule in the printer.
        ///15.	Repeat step 1-14 for AES 256 and SHA1
        ///16.	Create IPsec rule on printer with All IP Address, All Services and IPsec template with following settings:
        ///(i)	Dynamic with IKEv1 Phase1- AES128 and SHA1
        ///(ii)	Strength – Medium
        ///(iii)	 Authentication- Kerberos using configuration file mode
        ///17.	Import configuration file with cipher AES128/SHA1.
        ///18.	Create similar rule in client and validate Ping.
        ///19.	Ping should pass.
        ///20.	Enable the FIPS in device and validate Ping.
        ///21.	Ping should pass.
        ///22.	Delete all rules in printer.
        ///23.	Repeat step 16-21 for AES 256 and SHA1
        ///24.	Disable the FIPS in device.
        ///25.	Create IPsec rule on printer with All IP Address, All Services and IPsec template with following settings:
        ///(i)	Dynamic with IKEv1 Phase1- DES and MD5
        ///(ii)	Strength – Medium
        ///(iii)	 Authentication- Kerberos using configuration file mode
        ///26.	Import configuration file with cipher MD5/DES.
        ///27.	Create similar rule in client and validate Ping.
        ///28.	Ping should pass.
        /// </summary>
        /// <param name="activityData"><see cref="SecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyIPSecKerberosAuthenticationinFIPS(IPSecurityActivityData activityData, int testNo)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                // enable FIPS in device
                if (!EwsWrapper.Instance().SetFipsOption(true))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Validating IP-Sec- Kerberos in DES- Manual mode");
                AddressTemplateSettings addressTemplateSettings = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, activityData.WiredIPv4Address, activityData.WindowsSecondaryClientIPAddress);
                ServiceTemplateSettings serviceTemplateSettings = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                KerberosManualSettings manualKerberosSettings = new KerberosManualSettings(activityData.KerberosServerIPAddress, KERBEROS_USER_AES256 + '@' + KERBEROS_DOMAIN, KERBEROS_PSWD, KerberosEncryptionType.DESCBCMD5);
                KerberosSettings kerberos = new KerberosSettings(manualKerberosSettings);

                SecurityRuleSettings settings = CtcUtility.GetKerberosRuleSettings(testNo, kerberos, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.LowInteroperabilityHighsecurity);

                // If Kerberos Connection is not success, this method will throw an exception and returns false
                try
                {
                    EwsWrapper.Instance().CreateRule(settings, true);
                }
                catch
                {
                    TraceFactory.Logger.Info("Failed to authenticate Kerberos in FIPs mode with DES");
                    EwsWrapper.Instance().GotoMainIPsecPage();
                }
                EwsWrapper.Instance().DeleteAddressTemplate(ADDRESSTEMPLATENAME + testNo);

                TraceFactory.Logger.Info("Validating IP-Sec- Kerberos in AES-256- Manual mode");
                manualKerberosSettings = new KerberosManualSettings(activityData.KerberosServerIPAddress, KERBEROS_USER_AES256 + '@' + KERBEROS_DOMAIN, KERBEROS_PSWD, KerberosEncryptionType.AES256SHA1);
                kerberos = new KerberosSettings(manualKerberosSettings);

                settings = CtcUtility.GetKerberosRuleSettings(testNo, kerberos, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.LowInteroperabilityHighsecurity, defaultAction: DefaultAction.Allow);

                EwsWrapper.Instance().CreateRule(settings, true);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);

                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Create Rule on secondary client
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);

                // Rebooting secondary client machine 
                connectivityService.Channel.RebootMachine(IPAddress.Parse(activityData.WindowsSecondaryClientIPAddress));
                TraceFactory.Logger.Debug("Waiting for secondary client to reboot and come to ready state.");
                Thread.Sleep(TimeSpan.FromMinutes(2));
                TraceFactory.Logger.Debug("Creating Rule in Secodary client");

                connectivityService.Channel.CreateIPsecRule(settings, enableRule: true, enableProfiles: false);

                // Enable Domain,Private and public firewall profiles and allow inbound firewall policy to communicate back to the primary client when firewall is enabled in secondary client
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);

				// Since the Kerberos is taking long time to establish connection, given delay of 1 min
				Thread.Sleep(TimeSpan.FromMinutes(1));
                try
                {
                    if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                    {
                       
                        TraceFactory.Logger.Debug("Ping from the Secodary client is unsuccessful ");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Info("Exception Message: {0}".FormatWith(ex.Message));


                }
                TraceFactory.Logger.Info("Deleting Ipsec Rules from Secodary client");
                connectivityService.Channel.DeleteAllIPsecRules();
                EwsWrapper.Instance().DeleteAllRules();

                TraceFactory.Logger.Info("Validating IP-Sec- Kerberos in AES-128- Manual mode");
                manualKerberosSettings = new KerberosManualSettings(activityData.KerberosServerIPAddress, KERBEROS_USER_AES128 + '@' + KERBEROS_DOMAIN, KERBEROS_PSWD, KerberosEncryptionType.AES128SHA1);
                kerberos = new KerberosSettings(manualKerberosSettings);

                settings = CtcUtility.GetKerberosRuleSettings(testNo, kerberos, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.LowInteroperabilityHighsecurity, defaultAction: DefaultAction.Allow);

                EwsWrapper.Instance().CreateRule(settings, true);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);

                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Create Rule on secondary client
                connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);

                // Rebooting secondary client machine 
                connectivityService.Channel.RebootMachine(IPAddress.Parse(activityData.WindowsSecondaryClientIPAddress));
                TraceFactory.Logger.Debug("Waiting for secondary client to reboot and come to ready state.");
                Thread.Sleep(TimeSpan.FromMinutes(2));

                connectivityService.Channel.CreateIPsecRule(settings, enableRule: true, enableProfiles: false);

                // Enable Domain,Private and public firewall profiles and allow inbound firewall policy to communicate back to the primary client when firewall is enabled in secondary client
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);

                // Since the Kerberos is taking long time to establish connection, given delay of 1 min
                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Debug("Ping from the Secodary client is unsuccessful ");
                    return false;
                }
                TraceFactory.Logger.Info("Deleting Ipsec Rules from Secodary client");
                connectivityService.Channel.DeleteAllIPsecRules();
                EwsWrapper.Instance().DeleteAllRules();

                TraceFactory.Logger.Info("Validating IP-Sec- Kerberos in DES- configuration mode");
                KerberosImportSettings importSettings = new KerberosImportSettings(KERBEROS_CONFIGFILE.FormatWith(activityData.ProductFamily), KERBEROS_DES);
                kerberos = new KerberosSettings(importSettings);

                settings = CtcUtility.GetKerberosRuleSettings(testNo, kerberos, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.LowInteroperabilityHighsecurity);

                try
                {
                    EwsWrapper.Instance().CreateRule(settings, true);
                }
                catch
                {
                    TraceFactory.Logger.Info("Failed to authenticate Kerberos in FIPs mode with DES");
                    EwsWrapper.Instance().GotoMainIPsecPage();
                }
                EwsWrapper.Instance().DeleteAddressTemplate(ADDRESSTEMPLATENAME + testNo);

                TraceFactory.Logger.Info("Validating IP-Sec- Kerberos in AES-128- configuration mode");
                importSettings = new KerberosImportSettings(KERBEROS_CONFIGFILE.FormatWith(activityData.ProductFamily), KERBEROS_AES128);
                kerberos = new KerberosSettings(importSettings);

                settings = CtcUtility.GetKerberosRuleSettings(testNo, kerberos, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.LowInteroperabilityHighsecurity, defaultAction: DefaultAction.Allow);

                EwsWrapper.Instance().CreateRule(settings, true);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);

                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Create Rule on secondary client
                connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);

                // Rebooting secondary client machine 
                connectivityService.Channel.RebootMachine(IPAddress.Parse(activityData.WindowsSecondaryClientIPAddress));
                TraceFactory.Logger.Debug("Waiting for secondary client to reboot and come to ready state.");
                Thread.Sleep(TimeSpan.FromMinutes(2));

                connectivityService.Channel.CreateIPsecRule(settings, enableRule: true, enableProfiles: false);

                // Enable Domain,Private and public firewall profiles and allow inbound firewall policy to communicate back to the primary client when firewall is enabled in secondary client
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);

                // Since the Kerberos is taking long time to establish connection, given delay of 1 min
                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Debug("Printer IP address does not ping from Secodary client after IPSEC is enabled");
                    return false;
                }
                TraceFactory.Logger.Info("Deleting Ipsec Rules from Secodary client");
                connectivityService.Channel.DeleteAllIPsecRules();
                EwsWrapper.Instance().DeleteAllRules();

                TraceFactory.Logger.Info("Validating IP-Sec- Kerberos in AES-256- configuration mode");
                importSettings = new KerberosImportSettings(KERBEROS_CONFIGFILE.FormatWith(activityData.ProductFamily), KERBEROS_AES256);
                kerberos = new KerberosSettings(importSettings);

                settings = CtcUtility.GetKerberosRuleSettings(testNo, kerberos, addressTemplateSettings, serviceTemplateSettings, IKESecurityStrengths.LowInteroperabilityHighsecurity, defaultAction: DefaultAction.Allow);

                EwsWrapper.Instance().CreateRule(settings, true);

                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);

                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Create Rule on secondary client
                connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);

                // Rebooting secondary client machine 
                connectivityService.Channel.RebootMachine(IPAddress.Parse(activityData.WindowsSecondaryClientIPAddress));
                TraceFactory.Logger.Debug("Waiting for secondary client to reboot and come to ready state.");
                Thread.Sleep(TimeSpan.FromMinutes(2));

                connectivityService.Channel.CreateIPsecRule(settings, enableRule: true, enableProfiles: false);

                // Enable Domain,Private and public firewall profiles and allow inbound firewall policy to communicate back to the primary client when firewall is enabled in secondary client
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);

                // Since the Kerberos is taking long time to establish connection, given delay of 1 min
                Thread.Sleep(TimeSpan.FromMinutes(1));

                return connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address));
            }
            catch (Exception ipSecException)
            {
                EwsWrapper.Instance().SetFipsOption(false);
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress, true);
                return false;
            }
            finally
            {
                EwsWrapper.Instance().SetFipsOption(false);
                TestPostRequisites(activityData, true);
            }
        }
        #endregion

        #region Print with IPSec

        /// <summary>
        /// Verify printing with IPsec enabled (all protocol)
        ///-	Configure IPsec rule for both printer and pc
        ///-	Install the printer (all Protocols)
        ///-	Send few print jobs
        ///-	Printing should be successful.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <param name="testNo">Test Case number</param>
        /// <returns>Returns true if the test case passed else returns false</returns>
        public static bool VerifyPrintWithIPSec(IPSecurityActivityData activityData, int testNo, bool isIPV6 = false)
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

            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            BitArray resultArray = new BitArray(8, true);
            string address = null;            
            string outHostName = string.Empty;
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                if (isIPV6)
                {
                    // To refresh ipv6 page and to get state full address
                    EwsWrapper.Instance().SetDHCPv6OnStartup(true);
                    EwsWrapper.Instance().SetIPv6(false);
                    EwsWrapper.Instance().SetIPv6(true);
                    address = activityData.IPV6StatefullAddress;
                    if (address == null)
                    {
                        TraceFactory.Logger.Info("Failed to retrieve IPV6 State full address");
                        return false;
                    }
                }
                else
                {
                    address = activityData.WiredIPv4Address;
                }
                // Once the rule is created, unable to install drivers hence Installing copying the driver to local machine

                //Now Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(activityData.PrintDriverLocation, "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(activityData.PrintDriverLocation, @"C:" + activityData.PrintDriverLocation));

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(activityData.PrintDriverLocation, "*.*",
                    SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(activityData.PrintDriverLocation, @"C:" + activityData.PrintDriverLocation), true);

                CtcUtility.IPPS_Prerequisite(IPAddress.Parse(activityData.WiredIPv4Address), out outHostName);

                TraceFactory.Logger.Info("Configuring IPSec Rule for both Printer and PC and validating");

                // Get the basic rule settings
                SecurityRuleSettings settings = GetPSKRuleSettings(testNo, defaultAction: DefaultAction.Allow);

                // create rule with basic settings
                EwsWrapper.Instance().CreateRule(settings, true);

                // setting default rule action to Drop				
                EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);

                // setting IPsec option to true
                EwsWrapper.Instance().SetIPsecFirewall(true);

                // Creating Rule in Client
                CtcUtility.CreateIPsecRule(settings, true);

                // accessibility should pass since rule is enabled in printer and same rule is created on both client and printer
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                {
                    return false;
                }

                // Validating Print for P9100 protocol
                TraceFactory.Logger.Info("Installing driver with P9100 and validating print");
                if (!CtcUtility.InstallandPrint(IPAddress.Parse(address), Printer.Printer.PrintProtocol.RAW, activityData.ProductFamily, @"C:" + activityData.PrintDriverLocation, activityData.PrintDriverModel, 9100))
                {
                    TraceFactory.Logger.Info("Failed to Print");
                    return false;
                }

                TraceFactory.Logger.Info("Successfully Printed with P9100 protocol");
                return true;
            }
            catch (Exception ipSecException)
            {
                CleanOnError(printer, ipSecException, activityData.WindowsSecondaryClientIPAddress);
                return false;
            }
            finally
            {
                // clean up the printer and primary, secondary clients to default state
                TestPostRequisites(activityData);
                CtcUtility.IPPS_PostRequisite();
            }
        }

        #endregion

        #region Private Methods

        /*****************************************************************
		 ************* Printer Pre/Post/Error correction section *********
		 *****************************************************************/

        /// <summary>
        /// Performs Windows test pre requisites, it will check the printer, clients and server status
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        /// <returns>Returns true if the environment is correct else returns false</returns>
        private static bool TestPreRequisites(IPSecurityActivityData activityData, bool performLinuxPrerequisites = false)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);

            // If the Printer is not accessible then only we are deleting all rules from client and printer
            if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.WiredIPv4Address), TimeSpan.FromSeconds(20)))
            {
                // Cleaning up the rules [if rules are not deleted in post requisite in exceptional cases]
                ClearRules(activityData);
            }

            //TODO: Need to add the pre requisites after the completing all the tests.

            // Setting advances option telnet,SNMP,ftp..
            EwsWrapper.Instance().SetAdvancedOptions();

            // Setting Security DHCPV4 option and fail safe option[if test case exits] 
            EwsWrapper.Instance().SetSecurityDHCPV4(true);
            EwsWrapper.Instance().SetSecurityFailsafe(true);
            EwsWrapper.Instance().EnableSnmpv1v2ReadWriteAccess();

            if (performLinuxPrerequisites)
            {
                LinuxPrerequisities(activityData);
            }

            return true;
        }

        /// <summary>
        /// Perform Linux pre-requisites
        /// </summary>
        /// <param name="activityData"></param>
        private static void LinuxPrerequisities(IPSecurityActivityData activityData)
        {
            // Nothing to do as of now
        }

        /// <summary>
        /// Performs Windows test post requisites, deletes all the rules on the windows primary and secondary clients and also
        /// it will delete all the rules along with custom templates if it had created any.
        /// </summary>
        /// <param name="activityData"><see cref="IPSecurityActivityData"/></param>
        private static void TestPostRequisites(IPSecurityActivityData activityData, bool secondaryCleanup = false, bool performLinuxPostrequisites = false, bool deleteCertificates = false)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

            // Delete rules from primary, secondary client and from the printer
            ClearRules(activityData, secondaryCleanup, performLinuxPostrequisites, deleteCertificates);

            // TODO: If we enable Firewall on the client side programmatically need to disable here            
        }

        /// <summary>
        /// Reboot linux machine
        /// </summary>
        /// <param name="linuxAddress">Linux client machine address</param>
        private static void LinuxPostrequisities(string linuxAddress)
        {
            LinuxUtils.ExecuteCommand(IPAddress.Parse(linuxAddress), "reboot");
            Thread.Sleep(TimeSpan.FromMinutes(3));
        }

        /// <summary>
        /// This method is called on exception of any test.
        /// It will delete rules on the client side and performs cold reset.
        /// </summary>
        /// <param name="printer">Printer object</param>
        private static void CleanOnError(Printer.Printer printer, Exception ipSecException, string secondaryClientAddress, bool secondaryCleanup = false, bool performLinuxPostrequisites = false, string linuxServerAddress = null, bool deleteCertificates = false)
        {
            // If there are any errors while performing EWS operations or checking the printer accessibility, clean up the environment
            TraceFactory.Logger.Info("Error occurred while performing the test");
            TraceFactory.Logger.Info("Exception Message: {0}".FormatWith(ipSecException));

            if (performLinuxPostrequisites)
            {
                LinuxPostrequisities(linuxServerAddress);
            }

            // Delete all rules on the client side
            CtcUtility.DeleteAllIPsecRules();

            // Delete all rules in secondary client if only required
            if (secondaryCleanup)
            {
                ConnectivityUtilityServiceClient client = ConnectivityUtilityServiceClient.Create(secondaryClientAddress);
                client.Channel.DeleteAllIPsecRules();
                client.Channel.DisableFirewallProfile(FirewallProfile.Public);
                client.Channel.RebootMachine(IPAddress.Parse(secondaryClientAddress));
                Thread.Sleep(TimeSpan.FromMinutes(2));
            }

            // Disable the IPsec/Firewall option, so that client can communicate with printer via SNMP for cold resetting the printer
            EwsWrapper.Instance().SetIPsecFirewall(false);

            //If the product family is Inkjet, restore factory default is performed. Else Cold reset is done.
            if (printer is SiriusPrinter)
            {
                EwsWrapper.Instance().SetFactoryDefaults(false);
            }
            else
            {
                // Cold reset will clean the printer, it will not delete rules or custom templates. Those will be deleted as part of test post-requisites.
                printer.ColdReset();
            }



        }

        /// <summary>
        /// This method is called to clean up all created rules in printer and client        
        /// </summary>
        /// <param name="printer">Printer object</param>
        /// <param name="secondaryCleanup">clean up required for secondary client</param>
        private static void ClearRules(IPSecurityActivityData activityData, bool secondaryCleanup = false, bool performLinuxPostrequisites = false, bool deleteCertificates = false)
        {
            // If there are any errors while performing EWS operations or checking the printer accessibility, clean up the environment
            TraceFactory.Logger.Info("Cleaning up the Rules Created in Printer and Client");

            if (performLinuxPostrequisites)
            {
                LinuxPostrequisities(activityData.LinuxFedoraClientIPAddress);
            }

            // Delete all rules in the client
            if (!CtcUtility.DeleteAllIPsecRules())
            {
                CtcUtility.ShowErrorPopup("Failed to delete Rules in client. \nMake sure rules are deleted from client before proceeding to next test case");
            }

			// In case, EWS doesn't open up; pop up a message to user
			try
			{
                // Delete all rules in printer
                if (!EwsWrapper.Instance().DeleteAllRules())
                {
                    //Utility.ShowErrorPopup("Failed to delete Rules in printer. \nMake sure rules are deleted from printer before proceeding to next test case");
                    Printer.Printer printer = PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
                    printer.PowerCycle();
                    if (!EwsWrapper.Instance().DeleteAllRules())
                    {
                        CtcUtility.ShowErrorPopup("Failed to delete Rules in printer. \nMake sure rules are deleted from printer before proceeding to next test case");
                    }

                }
            }
            catch (Exception ruleException)
            {
                // Utility.ShowErrorPopup("Failed to delete Rules in printer. \nMake sure rules are deleted from printer before proceeding to next test case");
                TraceFactory.Logger.Info("Exception Caught: {0}".FormatWith(ruleException.Message));
            }

			// Delete all rules in secondary client if only required
			if (secondaryCleanup)
			{
				ConnectivityUtilityServiceClient client = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
				client.Channel.DeleteAllIPsecRules();
				client.Channel.DisableFirewallProfile(FirewallProfile.Public);
			}

            if (deleteCertificates)
            {
                DeleteCertificates();
            }

            if (!(ProductFamilies.LFP.ToString() == activityData.ProductFamily))
            {
                EwsWrapper.Instance().SetIPsecFirewall(false);
            }

            // Pinging the printer
            if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(activityData.WiredIPv4Address), TimeSpan.FromSeconds(20)))
            {
                CtcUtility.ShowErrorPopup("Printer is currently not accessible. \nPlease cold reset the printer");
            }
        }

        /// <summary>
        /// Delete all installed certificates on local client and printer
        /// </summary>
        private static void DeleteCertificates()
        {
            TraceFactory.Logger.Debug("Deleting certificates on client machine:");
            CtcUtility.DeleteCACertificate(CACERTIFICATE_SET1);
            CtcUtility.DeleteCACertificate(CACERTIFICATE_SET2);
            CtcUtility.DeleteIDCertificate(IDCERTIFICATE_SET1, IDCERTIFICATE_PSWD_SET1);
            CtcUtility.DeleteIDCertificate(IDCERTIFICATE_SET2, IDCERTIFICATE_PSWD_SET2);

            TraceFactory.Logger.Debug("Deleting certificates on printer:");
            EwsWrapper.Instance().UnInstallCACertificate(CACERTIFICATE_SET1);
            EwsWrapper.Instance().UnInstallCACertificate(CACERTIFICATE_SET2);
            EwsWrapper.Instance().UnInstallIDCertificate(IDCERTIFICATE_SET1, IDCERTIFICATE_PSWD_SET1);
            EwsWrapper.Instance().UnInstallIDCertificate(IDCERTIFICATE_SET2, IDCERTIFICATE_PSWD_SET2);
        }

        /*****************************************************************
		 ******************* Security Rule Settings section **************
		 *****************************************************************/

        /// <summary>
        /// Get SecurityRuleSettings structure for PSK Authentication type
        /// </summary>
        /// <param name="testNo">Test Number</param>
        /// <param name="address">Default Address Template</param>
        /// <param name="service">Default Service Template</param>
        /// <param name="strength">IKE Security Strength</param>
        /// <param name="phase1Settings">Phase 1 settings for Custom strength</param>
        /// <param name="phase2Settings">Phase 2 settings for Custom strength</param>
        /// <returns>Security Rule Settings <see cref="SecurityRuleSettings"/></returns>
        private static SecurityRuleSettings GetPSKRuleSettings(int testNo, DefaultAddressTemplates address = DefaultAddressTemplates.AllIPAddresses, DefaultServiceTemplates service = DefaultServiceTemplates.AllServices,
                                                               IKESecurityStrengths strength = IKESecurityStrengths.HighInteroperabilityLowsecurity, IKEPhase1Settings phase1Settings = null, IKEPhase2Settings phase2Settings = null, DefaultAction defaultAction = DefaultAction.Drop)
        {
            AddressTemplateSettings addressTemplate = new AddressTemplateSettings(address);
            ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(service);

            return GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, strength, phase1Settings, phase2Settings, defaultAction: defaultAction);
        }

        /// <summary>
        /// Get SecurityRuleSettings structure for PSK Authentication type
        /// </summary>
        /// <param name="testNo">Test Number</param>
        /// <param name="address">Address Template</param>
        /// <param name="service">Service Template</param>
        /// <param name="strength">IKE Security Strength</param>
        /// <param name="phase1Settings">Phase 1 settings for Custom strength</param>
        /// <param name="phase2Settings">Phase 2 settings for Custom strength</param>
        /// <returns>Security Rule Settings <see cref="SecurityRuleSettings"/></returns>
        private static SecurityRuleSettings GetPSKRuleSettings(int testNo, AddressTemplateSettings address, ServiceTemplateSettings service, IKESecurityStrengths strength = IKESecurityStrengths.HighInteroperabilityLowsecurity,
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

        /// <summary>
        /// Get SecurityRuleSettings structure for Manual Keys
        /// </summary>
        /// <param name="testNo">Test Number</param>
        /// <param name="address">Address Template</param>
        /// <param name="service">Service Template</param>
        /// <param name="basicSettings">Basic Manual Key Settings</param>
        /// <param name="advancedSetttings">Advance Manual Key Settings</param>		
        /// <returns>Security Rule Settings</returns>
        private static SecurityRuleSettings GetManualRuleSettings(int testNo, AddressTemplateSettings address, ServiceTemplateSettings service, BasicManualKeysSettings basicSettings, AdvancedManualKeysSettings advancedSetttings, DefaultAction defaultAction = DefaultAction.Drop)
        {
            ManualKeysSettings manualKeySettings = new ManualKeysSettings(basicSettings, advancedSetttings);
            IPsecTemplateSettings ipsecTemplate = new IPsecTemplateSettings(IPSECTEMPLATENAME.FormatWith(testNo), manualKeySettings);

            SecurityRuleSettings securitySettings = new SecurityRuleSettings(CLIENT_RULE_NAME.FormatWith(testNo), address, service, IPsecFirewallAction.ProtectedWithIPsec, ipsecTemplate, defaultAction: defaultAction);

            return securitySettings;
        }

        /// <summary>
        /// Get SecurityRuleSettings for Certificates Authentication type
        /// </summary>
        /// <param name="testNo">Test Number</param>
        /// <param name="address">Default Address Template</param>
        /// <param name="service">Default Service Template</param>
        /// <param name="strength">IKE Security Strength</param>
        /// <param name="install">Option to choose between certificate sets</param>
        /// <param name="phase1Settings">Phase 1 settings for Custom strength</param>
        /// <param name="phase2Settings">Phase 2 settings for Custom strength</param>
        /// <param name="installCertificate">Install Certificate on client machine</param>
        /// <returns>Security Rule Settings <see cref="SecurityRuleSettings"/></returns>
        private static SecurityRuleSettings GetCertificatesRuleSettings(int testNo, DefaultAddressTemplates address = DefaultAddressTemplates.AllIPAddresses, DefaultServiceTemplates service = DefaultServiceTemplates.AllServices, IKESecurityStrengths strength = IKESecurityStrengths.HighInteroperabilityLowsecurity,
                                                                        CertificateInstall install = CertificateInstall.ValidSet1, IKEPhase1Settings phase1Settings = null, IKEPhase2Settings phase2Settings = null, bool installCertificate = true, DefaultAction defaultAction = DefaultAction.Drop)
        {
            AddressTemplateSettings addressTemplate = new AddressTemplateSettings(address);
            ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(service);

            return GetCertificatesRuleSettings(testNo, addressTemplate, serviceTemplate, strength, install, phase1Settings, phase2Settings, installCertificate, defaultAction: defaultAction);
        }

        /// <summary>
        /// Get SecurityRuleSettings for Certificates Authentication type
        /// </summary>
        /// <param name="testNo">Test Number</param>
        /// <param name="address">Address Template</param>
        /// <param name="service">Service Template</param>
        /// <param name="strength">IKE Security Strength</param>
        /// <param name="install">Option to choose between certificate sets</param>
        /// <param name="phase1Settings">Phase 1 settings for Custom strength</param>
        /// <param name="phase2Settings">Phase 2 settings for Custom strength</param>
        /// <param name="installCertificate">Install Certificate on client machine</param>
        /// <returns>Security Rule Settings <see cref="SecurityRuleSettings"/></returns>
        private static SecurityRuleSettings GetCertificatesRuleSettings(int testNo, AddressTemplateSettings address, ServiceTemplateSettings service, IKESecurityStrengths strength = IKESecurityStrengths.HighInteroperabilityLowsecurity,
                                                                        CertificateInstall install = CertificateInstall.ValidSet1, IKEPhase1Settings phase1Settings = null, IKEPhase2Settings phase2Settings = null, bool installCertificate = true, DefaultAction defaultAction = DefaultAction.Drop)
        {
            IKEv1Settings ikeV1Settings = null;

            string caCertificatePath, idCertificatePath, idPassword;
            caCertificatePath = idCertificatePath = idPassword = string.Empty;

            switch (install)
            {
                case CertificateInstall.ValidSet1:
                    {
                        caCertificatePath = CACERTIFICATE_SET1;
                        idCertificatePath = IDCERTIFICATE_SET1;
                        idPassword = IDCERTIFICATE_PSWD_SET1;
                        break;
                    }

                case CertificateInstall.ValidSet2:
                    {
                        caCertificatePath = CACERTIFICATE_SET2;
                        idCertificatePath = IDCERTIFICATE_SET2;
                        idPassword = IDCERTIFICATE_PSWD_SET2;
                        break;
                    }

                case CertificateInstall.Invalid:
                    {
                        caCertificatePath = CACERTIFICATE_INVALID;
                        idCertificatePath = IDCERTIFICATE_INVALID;
                        idPassword = IDCERTIFICATE__PSWD_INVALID;
                        break;
                    }

                default: break;
            }

            if (string.IsNullOrEmpty(caCertificatePath) || string.IsNullOrEmpty(idCertificatePath) || string.IsNullOrEmpty(idPassword))
            {
                throw new ArgumentNullException("Invalid certificate(s) path");
            }

            // Based on Strength, construct IKEv1Settings
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

                ikeV1Settings = new IKEv1Settings(caCertificatePath, idCertificatePath, idPassword, phase1Settings, phase2Settings);
            }
            else
            {
                ikeV1Settings = new IKEv1Settings(caCertificatePath, idCertificatePath, idPassword);
            }

            if (installCertificate)
            {
                TraceFactory.Logger.Info("Installing CA Certificate: {0} in Client machine".FormatWith(caCertificatePath));
                if (!(CtcUtility.InstallCACertificate(caCertificatePath) && CtcUtility.InstallIDCertificate(idCertificatePath, idPassword)))
                {
                    TraceFactory.Logger.Info("Failed to install certificate on client machine.");
                }
                else
                {
                    TraceFactory.Logger.Info("Successfully Installed Certificate on Client Machine");
                }
            }

            DynamicKeysSettings dynamicSettings = new DynamicKeysSettings(strength, ikeV1Settings);
            IPsecTemplateSettings ipsecTemplate = new IPsecTemplateSettings(IPSECTEMPLATENAME.FormatWith(testNo), dynamicSettings);

            SecurityRuleSettings securitySettings = new SecurityRuleSettings(CLIENT_RULE_NAME.FormatWith(testNo), address, service, IPsecFirewallAction.ProtectedWithIPsec, ipsecTemplate, defaultAction: defaultAction);

            return securitySettings;
        }

        /// <summary>
        /// Get SecurityRuleSettings for Kerberos Authentication type
        /// </summary>
        /// <param name="testNo">Test Number</param>
        /// <param name="kerberosSettings">Kerberos Settings to be configured</param>
        /// <param name="address">Default Address template</param>
        /// <param name="service">Default Service template</param>
        /// <param name="strength">IKE Security Strength</param>
        /// <returns>Security Rule Settings <see cref="SecurityRuleSettings"/></returns>
        private static SecurityRuleSettings GetKerberosRuleSettings(int testNo, KerberosSettings kerberosSettings, DefaultAddressTemplates address = DefaultAddressTemplates.AllIPAddresses, DefaultServiceTemplates service = DefaultServiceTemplates.AllServices,
                                                                    IKESecurityStrengths strength = IKESecurityStrengths.HighInteroperabilityLowsecurity)
        {
            AddressTemplateSettings addressTemplate = new AddressTemplateSettings(address);
            ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(service);

            return GetKerberosRuleSettings(testNo, kerberosSettings, addressTemplate, serviceTemplate, strength);
        }

        /// <summary>
        /// Get SecurityRuleSettings for Kerberos Authentication type
        /// </summary>
        /// <param name="testNo">Test Number</param>
        /// <param name="kerberosSettings">Kerberos Settings to be configured</param>
        /// <param name="address">Address template</param>
        /// <param name="service">Service template</param>
        /// <param name="strength">IKE Security Strength</param>
        /// <returns>Security Rule Settings <see cref="SecurityRuleSettings"/></returns>
        private static SecurityRuleSettings GetKerberosRuleSettings(int testNo, KerberosSettings kerberosSettings, AddressTemplateSettings address, ServiceTemplateSettings service, IKESecurityStrengths strength, DefaultAction defaultAction = DefaultAction.Drop)
        {
            IKEv1Settings ikeV1Settings = new IKEv1Settings(kerberosSettings);
            DynamicKeysSettings dynamicSettings = new DynamicKeysSettings(strength, ikeV1Settings);
            IPsecTemplateSettings ipsecTemplate = new IPsecTemplateSettings(IPSECTEMPLATENAME.FormatWith(testNo), dynamicSettings);

            SecurityRuleSettings securitySettings = new SecurityRuleSettings(CLIENT_RULE_NAME.FormatWith(testNo), address, service, IPsecFirewallAction.ProtectedWithIPsec, ipsecTemplate, defaultAction: defaultAction);

            return securitySettings;
        }

        /*****************************************************************
		 ********************** Private functions section ****************
		 *****************************************************************/

        /// <summary>
        /// Prints SecurityDebugPage through FrontPanel
        /// Administration --> Network Settings--> Embedded Jet direct Menu--> Security---> Print Sec Page
        /// </summary>
        /// <param name="device">printer</param>
        /// <returns>True if printed, else false.</returns>        
        private static bool PrintSecurityDebugPageFrontPanel(IDevice device, IPSecurityActivityData activityData)
        {

            //check if the device is Omni opus or not
            if (EwsWrapper.Instance().IsOmniOpus)
            {
                PrintSecurityDebugPageFrontPanelOmniOpus(activityData);
                return true;
            }
            TraceFactory.Logger.Debug("Printing Security Page through Front Panel");
            // Navigation based on the selected interface
            int preFix = 0;
            if (activityData.WiredInterface)
            {
                preFix = 4;
            }
            else
            {
                preFix = (activityData.WirelessInterface) ? 2 : 1;
            }

            try
            {
                // Navigating the control back to home page if the control panel has error screen
                JediWindjammerDevice jd = device as JediWindjammerDevice;
                JediWindjammerControlPanel cp = jd.ControlPanel;

                // Validating whether the current page is in Home
                while (!cp.CurrentForm().Contains("Home"))
                {
                    TraceFactory.Logger.Info("Setting the home page");
                    //Pressing hide button to move the page to home
                    cp.Press("mButton1");
                }

                TraceFactory.Logger.Info("Navigating to Security to print Security Page");
                cp.Press("AdminApp");

                //scroll IncrementButton to get networking control
                while (!cp.GetControls().Contains("NetworkingAndIOMenu"))
                {
                    // Even after the increment button is pressed in the front panel exception occurred, so added try catch block
                    try
                    {
                        //Scrolling to click networking and IO menu
                        cp.Press("IncrementButton");
                    }
                    catch
                    {
                        // Do Nothing
                    }
                }

                // Navigating to security to print sec page                                
                cp.Press("NetworkingAndIOMenu"); // Networking
                cp.Press("JetDirectMenu{0}".FormatWith(preFix));      // Embedded Jet direct Menu                
                cp.Press("0x2");                 // Information
                cp.PressToNavigate("0x1c_{0}".FormatWith(preFix), "IOMgrMenuDataSelection", true);
                cp.Press("YES");
                cp.Press("m_OKButton");
                Thread.Sleep(TimeSpan.FromMinutes(2));
                TraceFactory.Logger.Info("Successfully printed the IPsec information,please validate the same manually");
                return true;
            }
            catch (Exception frontPanelException)
            {
                TraceFactory.Logger.Debug("Exception:{0}".FormatWith(frontPanelException));
                return false;
            }
        }

        // PrintSecurityDebugPageFrontPanel For Omni Opus
        public static bool PrintSecurityDebugPageFrontPanelOmniOpus(IPSecurityActivityData activityData)
        {
            JediOmniDevice _device = new JediOmniDevice(activityData.WiredIPv4Address);

            TraceFactory.Logger.Debug("Printing Security Page through Front Panel For Omni Opus");
            // Navigation based on the selected interface
            int preFix = 0;
            if (activityData.WiredInterface)
            {
                preFix = 4;
            }
            else
            {
                preFix = (activityData.WirelessInterface) ? 2 : 1;
            }
            try
            {

                Thread.Sleep(60000);
                _device.PowerManagement.Wake();
                _device.ControlPanel.ScrollToItem("#hpid-settings-homescreen-button");
                _device.ControlPanel.PressWait("#hpid-settings-homescreen-button", "#hpid-keyboard", TimeSpan.FromSeconds(2));


                //click networking
                _device.ControlPanel.PressWait("#hpid-tree-node-listitem-networkingandiomenu", "#hpid-keyboard", TimeSpan.FromSeconds(2));

                //Click on Ethernet
                _device.ControlPanel.PressWait("#hpid-tree-node-listitem-jetdirectmenu4", "#hpid-keyboard", TimeSpan.FromSeconds(2));

                //Click on Information
                _device.ControlPanel.PressWait("#hpid-tree-node-listitem-0x2", "#hpid-keyboard", TimeSpan.FromSeconds(2));

                //Click on Print Sec Report
                _device.ControlPanel.PressWait("#hpid-tree-node-listitem-0x1c_4", "#hpid-keyboard", TimeSpan.FromSeconds(2));
                //Enable IPSec

                //if (state)
                //{
                _device.ControlPanel.PressWait("#0", "#hpid-keyboard", TimeSpan.FromSeconds(2));
                //_device.ControlPanel.Press("#0");
                //}
                // else
                //{
                //  _device.ControlPanel.Press("#1");
                //}

                //Click Done
                _device.ControlPanel.PressWait("#hpid-ok-setting-button", "#hpid-keyboard", TimeSpan.FromSeconds(2));
                //EwsWrapper.Instance().Stop();
                TraceFactory.Logger.Info("Successfully printed the IPsec information,please validate the same manually");
                return true;
            }
            catch
            {
                //returning false if the error message is other than missing Cartridge errors                
                return false;
            }
        }


        /// <summary>
        ///Enable IPsec through FrontPanel
        /// Administration --> Network Settings--> Embedded Jet direct Menu--> Security---> Select IPSEC --->
        /// </summary>
        /// <param name="device">printer</param>
        /// <param name="state">true/false</param>
        /// <returns>True if printed, else false.</returns>        
        private static bool EnableIPSecFrontPanel(IDevice device, bool state, IPSecurityActivityData activityData)
        {
            TraceFactory.Logger.Debug("Enabling IPSecurity through Front Panel");
            // Navigation based on the selected interface
            int preFix = 0;
            if (activityData.WiredInterface)
            {
                preFix = 4;
            }
            else
            {
                preFix = (activityData.WirelessInterface) ? 2 : 1;
            }

            try
            {
                //Navigating the control back to home page if the control panel has error screen			
                JediWindjammerDevice jd = device as JediWindjammerDevice;
                JediWindjammerControlPanel cp = jd.ControlPanel;

                //validating whether the current page is in Home
                while (!cp.CurrentForm().Contains("Home"))
                {
                    //Pressing hide button to move the page to home
                    cp.Press("mButton1");
                }

                cp.Press("AdminApp");

                //scroll IncrementButton to get networking control
                while (!cp.GetControls().Contains("NetworkingAndIOMenu"))
                {
                    // Even after the increment button is pressed in the front panel exception occurred, so added try catch block
                    try
                    {
                        cp.Press("IncrementButton");
                    }
                    catch
                    {
                        // Do Nothing
                    }
                }

                //Enabling IPSec through Front Panel                
                cp.Press("NetworkingAndIOMenu");
                cp.Press("JetDirectMenu{0}".FormatWith(preFix));     //Embedded jet direct option
                cp.Press("0x9");                // Security option	
                cp.PressToNavigate("0x46_{0}".FormatWith(preFix), "IOMgrMenuDataSelection", true);
                if (state)
                {
                    cp.Press("KEEP");
                }
                else
                {
                    cp.Press("DISABLE");
                }
                cp.Press("m_OKButton");
                Thread.Sleep(TimeSpan.FromMinutes(2));

                TraceFactory.Logger.Info("Successfully {0} IPSec through Front Panel".FormatWith(state ? "enabled" : "disabled"));
                return true;
            }
            catch
            {
                //returning false if the error message is other than missing Cartridge errors                
                return false;
            }
        }

        /// <summary>
        /// Validate DHCP Rebind packets
        /// </summary>
        /// <param name="serverAddress">Address where the PacketCapture service is hosted</param>
        /// <param name="packetLocation">Packet location path</param>
        /// <param name="printerAddress">Printer Address</param>
        /// <param name="macAddress">Printer Mac Address</param>
        /// <returns>true if packet validation is successful</returns>
        private static bool ValidateDhcpRebindPacket(string serverAddress, string packetLocation, string printerAddress)
        {
            TraceFactory.Logger.Info(CtcBaseTests.PACKET_VALIDATION);

            string error = string.Empty;
            string filter = "ip.addr == {0} && bootp".FormatWith(printerAddress);

            PacketCaptureServiceClient client = PacketCaptureServiceClient.Create(serverAddress);
            if (client.Channel.Validate(packetLocation, filter, ref error, "DHCP: Request", "DHCP: Ack"))
            {
                TraceFactory.Logger.Info("DHCP request and Acknowledge packet is successfully validated.");
                return true;
            }
            else
            {
                TraceFactory.Logger.Info("Failed to validate DHCP Request/Acknwledge packets.");
                return false;
            }
        }

        /// <summary>
        /// Gets the printer IPV6 address range        
        /// </summary>
        /// <param name="activityData">activityData</param>        
        /// <returns>string with start and end ipv6 range with delimiter.</returns>
        /// ex: start range: 2000:201:201:206::1, end range: 2000:201:201:206:ffff:ffff:ffff:ffff
        private static string GetPrinterIPV6AddressRange(IPAddress statelessIPV6Address, out string statelessAddressSubString)
        {
            TraceFactory.Logger.Debug("Gets the Printer IPV6 start and end address range");

            // Splitting one of the stateless ipv6 address to fetch the starting range
            string[] address = statelessIPV6Address.ToString().Split(':');
            statelessAddressSubString = address[0] + ':' + address[1] + ':' + address[2] + ':' + address[3];

            string startIPV6Range = address[0] + ':' + address[1] + ':' + address[2] + ':' + address[3] + "::" + "1";
            string endIPV6Range = address[0] + ':' + address[1] + ':' + address[2] + ':' + address[3] + ':' + "ffff" + ':' + "ffff" + ':' + "ffff" + ':' + "ffff";

            // returning the start and end ipv6 range with delimiter
            return startIPV6Range + '|' + endIPV6Range;
        }

        /// <summary>
        /// Validates if the given range of ipv6 address exists in client local machine 
        /// </summary>
        /// <param name="value">ipv6 address range from the printer</param>        
        /// <returns>true if exists</returns>        
        private static bool ValidateIPV6AddressRange(string value)
        {
            TraceFactory.Logger.Debug("Validating the ipv6 address range in local machine");
            var result = ProcessUtil.Execute("cmd.exe", "/C ipconfig");
            return result.StandardOutput.Contains(value);
        }

        /// <summary>
        /// Configure PSK, Racoon and IPsec files on Linux
        /// </summary>
        /// <param name="linuxClientAddress">Linux Address</param>
        /// <param name="ruleSettings"><see cref="SecurityRuleSettings"/></param>
        /// <param name="pskValue">PSK Value</param>
        /// <param name="isTunnelMode">true if tunnel mode, false otherwise</param>
        /// <param name="isManualKeys">true if manual keys, false otherwise</param>
        /// <param name="encryption"><see cref="Encryptions"/></param>
        /// <param name="authentication"><see cref="Authentications"/></param>
        private static void ConfigureIPSecRule(IPAddress linuxClientAddress, SecurityRuleSettings ruleSettings, string pskValue, bool isTunnelMode = false, bool isManualKeys = false, Encryptions encryption = Encryptions.AES128, Authentications authentication = Authentications.SHA1)
        {
            /*	Dynamic Keys and Manual Keys configurations are edited on same template files.
			 *  Few settings in configuration files are specific to Manual keys. These are commented for Dynamic keys and uncommented only for Manual keys.
			 *  There are 3 files to be configured:
			 *		1. PSK File:	For Dynamic rule and Windows rule. (PSK value are configured)
			 *		2. Racoon File: For Dynamic rule and Windows rule. (Encryption and Authentication are configured)
			 *		3. IPSec File:	For Dynamic/ Manual rule and Windows rule. (Address, Manual settings are configured)
			 *	While configuring settings, 2 rules are created:
			 *		1. Rule for Linux-Printer authentication based on test case requirement.
			 *		2. Rule for Linux-Windows authentication. This rule is common for all scenarios which is used to communicate between Linux and Windows.
			 *			Once a rule created for Linux-Printer, Windows machine can't access Linux; hence a rule is created for Linux and Windows communication.
			 *			Authentication(Sha1) and Encryption(AES128) are used as default since same is supported on Windows OS too.
			 * */

            IPAddress clientAddress = NetworkUtil.GetLocalAddress(linuxClientAddress.ToString(), linuxClientAddress.GetSubnetMask().ToString());
            string printerAddress = ruleSettings.AddressTemplate.LocalAddress;
            string linuxAddress = ruleSettings.AddressTemplate.RemoteAddress;
            string encapsulationType = EncapsulationType.Transport.ToString().ToLowerInvariant();

            // Manual keys settings
            string tunnelOutAddress = string.Empty;
            string tunnelInAddress = string.Empty;
            string espOut = string.Empty;
            string ahOut = string.Empty;
            string encapsulationMode = string.Empty;
            string printerEncryption = string.Empty;
            string encryptionOut = string.Empty;
            string printerAuthentication = string.Empty;
            string authenticationOut = string.Empty;
            string espIn = string.Empty;
            string ahIn = string.Empty;
            string encryptionIn = string.Empty;
            string authenticationIn = string.Empty;

            if (isTunnelMode)
            {
                encapsulationType = EncapsulationType.Tunnel.ToString().ToLowerInvariant();
                tunnelInAddress = string.Format("{0}-{1}".FormatWith(linuxAddress, printerAddress));
                tunnelOutAddress = string.Format("{0}-{1}".FormatWith(printerAddress, linuxAddress));
                encapsulationMode = string.Format("-m {0}".FormatWith(EncapsulationType.Tunnel.ToString().ToLowerInvariant()));
            }

            if (isManualKeys)
            {
                espOut = ruleSettings.IPsecTemplate.ManualKeysSettings.AdvancedSettings.ESPSPIOut.ToString();
                ahOut = ruleSettings.IPsecTemplate.ManualKeysSettings.AdvancedSettings.AHSPIOut.ToString();
                printerEncryption = Enum<Encryptions>.Value(ruleSettings.IPsecTemplate.ManualKeysSettings.BasicSettings.ESPEncryption);
                encryptionOut = ruleSettings.IPsecTemplate.ManualKeysSettings.AdvancedSettings.EncryptionOut.ToString();
                if ((ruleSettings.IPsecTemplate.ManualKeysSettings.BasicSettings.ESPEnable) && (ruleSettings.IPsecTemplate.ManualKeysSettings.BasicSettings.AHEnable))
                {
                    printerAuthentication = Enum<Authentications>.Value(ruleSettings.IPsecTemplate.ManualKeysSettings.BasicSettings.AHAuthentication);
                }
                else
                {
                    printerAuthentication = Enum<Authentications>.Value(ruleSettings.IPsecTemplate.ManualKeysSettings.BasicSettings.ESPAuthentication);
                }
                authenticationOut = ruleSettings.IPsecTemplate.ManualKeysSettings.AdvancedSettings.AuthenticationOut.ToString();
                espIn = ruleSettings.IPsecTemplate.ManualKeysSettings.AdvancedSettings.ESPSPIIn.ToString();
                ahIn = ruleSettings.IPsecTemplate.ManualKeysSettings.AdvancedSettings.AHSPIIn.ToString();
                encryptionIn = ruleSettings.IPsecTemplate.ManualKeysSettings.AdvancedSettings.EncryptionIn.ToString();
                authenticationIn = ruleSettings.IPsecTemplate.ManualKeysSettings.AdvancedSettings.AuthenticationIn.ToString();
            }

            //********* PSK File *********

            string winFilePath = Path.Combine(Path.GetTempPath(), LinuxUtils.PSK_FILE);
            string linuxFilePath = string.Concat(LinuxUtils.BACKUP_FOLDER_PATH, LinuxUtils.PSK_FILE);

            // Copy to temp directory
            LinuxUtils.CopyFileFromLinux(linuxClientAddress, linuxFilePath, winFilePath);

            Collection<string> keyValue = new Collection<string>();

            keyValue.Add(LinuxUtils.KEY_CLIENTADDRESS);
            keyValue.Add(LinuxUtils.KEY_PRINTERADDRESS);
            keyValue.Add(LinuxUtils.KEY_PSK);

            Collection<string> configureValue = new Collection<string>();

            configureValue.Add(clientAddress.ToString());
            configureValue.Add(printerAddress);
            configureValue.Add(pskValue);

            // To create ipsec rule, psk file should be read-only format. For configuring settings, file permissions are modified and reverted back to read-only.
            LinuxUtils.ExecuteCommand(linuxClientAddress, "chmod 777 {0}".FormatWith(LinuxUtils.LINUX_PSK_PATH));
            LinuxUtils.ConfigurePSKFile(linuxClientAddress, winFilePath, keyValue, configureValue);

            if (!isManualKeys)
            {
                LinuxUtils.RemoveComments(linuxClientAddress, LinuxUtils.LINUX_PSK_PATH);
            }

            LinuxUtils.ExecuteCommand(linuxClientAddress, "chmod 700 {0}".FormatWith(LinuxUtils.LINUX_PSK_PATH));

            keyValue.Clear();
            configureValue.Clear();

            //********* Racoon File *********

            winFilePath = Path.Combine(Path.GetTempPath(), LinuxUtils.RACOON_FILE);
            linuxFilePath = string.Concat(LinuxUtils.BACKUP_FOLDER_PATH, LinuxUtils.RACOON_FILE);

            // Copy to temp directory
            LinuxUtils.CopyFileFromLinux(linuxClientAddress, linuxFilePath, winFilePath);

            keyValue.Add(LinuxUtils.KEY_CLIENTADDRESS);
            keyValue.Add(LinuxUtils.KEY_CLIENTENCRYPTION);
            keyValue.Add(LinuxUtils.KEY_CLIENTAUTHENTICATION);
            keyValue.Add(LinuxUtils.KEY_PRINTERADDRESS);
            keyValue.Add(LinuxUtils.KEY_PRINTERENCRYPTION);
            keyValue.Add(LinuxUtils.KEY_PRINTERAUTHENTICATION);

            // Configuring pre-defined authentication and encryption for all dynamic key rules
            configureValue.Add(clientAddress.ToString());
            configureValue.Add(Encryptions.AES128.ToString().ToLowerInvariant());
            configureValue.Add(Authentications.SHA1.ToString().ToLowerInvariant());
            configureValue.Add(printerAddress);
            configureValue.Add(Enum<Encryptions>.Value(encryption));
            configureValue.Add(Enum<Authentications>.Value(authentication));

            Thread.Sleep(TimeSpan.FromMinutes(1));
            LinuxUtils.ConfigureRacoonFile(linuxClientAddress, winFilePath, keyValue, configureValue);

            if (!isManualKeys)
            {
                LinuxUtils.RemoveComments(linuxClientAddress, LinuxUtils.LINUX_RACOON_PATH);
            }

            keyValue.Clear();
            configureValue.Clear();

            //********* IPsec File *********
            winFilePath = Path.Combine(Path.GetTempPath(), LinuxUtils.IPSEC_FILE);
            if (isManualKeys)
            {
                string ipsecFilePath = ((ruleSettings.IPsecTemplate.ManualKeysSettings.BasicSettings.ESPEnable) && (ruleSettings.IPsecTemplate.ManualKeysSettings.BasicSettings.AHEnable)) ?
                                       LinuxUtils.IPSEC_ESP_AH_FILE : ruleSettings.IPsecTemplate.ManualKeysSettings.BasicSettings.AHEnable ?
                                       LinuxUtils.IPSEC_AH_FILE : LinuxUtils.IPSEC_FILE;
                linuxFilePath = string.Concat(LinuxUtils.BACKUP_FOLDER_PATH, ipsecFilePath);
            }
            else
            {
                linuxFilePath = string.Concat(LinuxUtils.BACKUP_FOLDER_PATH, LinuxUtils.IPSEC_FILE);
            }

            // Copy to temp directory
            LinuxUtils.CopyFileFromLinux(linuxClientAddress, linuxFilePath, winFilePath);

            keyValue.Add(LinuxUtils.KEY_CLIENTADDRESS);
            keyValue.Add(LinuxUtils.KEY_LINUXCLIENTADDRESS);
            keyValue.Add(LinuxUtils.KEY_PRINTERADDRESS);
            keyValue.Add(LinuxUtils.KEY_LINUXPRINTERADDRESS);
            keyValue.Add(LinuxUtils.KEY_ENCAPSULATION);
            keyValue.Add(LinuxUtils.KEY_TUNNELOUTADDRESS);
            keyValue.Add(LinuxUtils.KEY_TUNNELINADDRESS);
            keyValue.Add(LinuxUtils.KEY_ESPOUT);
            keyValue.Add(LinuxUtils.KEY_AHOUT);
            keyValue.Add(LinuxUtils.KEY_ENCAPSULATIONMODE);
            keyValue.Add(LinuxUtils.KEY_PRINTERENCRYPTION);
            keyValue.Add(LinuxUtils.KEY_ENCRYPTIONOUT);
            keyValue.Add(LinuxUtils.KEY_PRINTERAUTHENTICATION);
            keyValue.Add(LinuxUtils.KEY_AUTHENTICATIONOUT);
            keyValue.Add(LinuxUtils.KEY_ESPIN);
            keyValue.Add(LinuxUtils.KEY_AHIN);
            keyValue.Add(LinuxUtils.KEY_ENCRYPTIONIN);
            keyValue.Add(LinuxUtils.KEY_AUTHENTICATIONIN);

            configureValue.Add(clientAddress.ToString());
            configureValue.Add(linuxClientAddress.ToString());
            configureValue.Add(printerAddress);
            configureValue.Add(linuxAddress);
            configureValue.Add(encapsulationType);
            configureValue.Add(tunnelOutAddress);
            configureValue.Add(tunnelInAddress);
            configureValue.Add(espOut);
            configureValue.Add(ahOut);
            configureValue.Add(encapsulationMode);
            configureValue.Add(printerEncryption);
            configureValue.Add(encryptionOut);
            configureValue.Add(printerAuthentication);
            configureValue.Add(authenticationOut);
            configureValue.Add(espIn);
            configureValue.Add(ahIn);
            configureValue.Add(encryptionIn);
            configureValue.Add(authenticationIn);

            Thread.Sleep(TimeSpan.FromMinutes(1));
            LinuxUtils.ConfigureIPsecFile(linuxClientAddress, winFilePath, keyValue, configureValue);

            if (isManualKeys)
            {
                LinuxUtils.RemoveComments(linuxClientAddress, LinuxUtils.LINUX_IPSEC_PATH);
            }
        }

        /// <summary>
        /// Ping IPv6 Stateless Address
        /// </summary>
        /// <param name="linuxClientAddress">Linux address</param>
        /// <param name="statelessAddress">IPv6 Stateless address to ping</param>
        /// <returns>true if more than 50% of ping is successful</returns>
        private static bool PingStatelessAddress(IPAddress linuxClientAddress, IPAddress statelessAddress)
        {
            string pingDetails = LinuxUtils.PingIPv6Address(linuxClientAddress, statelessAddress);

            // Based on the details acquired, split data to get packet loss %
            string[] split = new string[] { "statistics", ",", " packet loss", "%" };
            string[] splitDetails = pingDetails.Split(split, StringSplitOptions.RemoveEmptyEntries);

            // 3rd element will provide the % packet loss, checking if the loss is more than 50%
            if (splitDetails.Length > 3 && int.Parse(splitDetails[3].Trim()) > 50)
            {
                TraceFactory.Logger.Info("Ping with address: {0} from linux client is unsuccessful.".FormatWith(statelessAddress));
                return false;
            }
            else
            {
                TraceFactory.Logger.Info("Ping with address: {0} from linux client is successful.".FormatWith(statelessAddress));
                return true;
            }
        }

        /// <summary>
        /// Ping IPv4 Address
        /// </summary>
        /// <param name="linuxClientAddress">Linux address</param>
        /// <param name="addressToPing">IPv4 address to ping</param>
        /// <returns>true if more than 50% of ping is successful</returns>
        private static bool PingIPv4Address(IPAddress linuxClientAddress, IPAddress addressToPing)
        {
            string pingDetails = LinuxUtils.PingIPv4Address(linuxClientAddress, addressToPing);

            // Based on the details acquired, split data to get packet loss %
            string[] split = new string[] { "statistics", ",", " packet loss", "%" };
            string[] splitDetails = pingDetails.Split(split, StringSplitOptions.RemoveEmptyEntries);

            Thread.Sleep(TimeSpan.FromMinutes(1));
            TraceFactory.Logger.Debug("Percentage of Packet Lost:{0}".FormatWith(splitDetails[3]));
            // 3rd element will provide the % packet loss, checking if the loss is more than 50%
            if (splitDetails.Length > 3 && int.Parse(splitDetails[3]) > 50)
            {
                TraceFactory.Logger.Info("Ping with address: {0} from linux client is unsuccessful.".FormatWith(addressToPing));
                return false;
            }
            else
            {
                TraceFactory.Logger.Info("Ping with address: {0} from linux client is successful.".FormatWith(addressToPing));
                return true;
            }
        }

        /// <summary>
        /// Verify if ping works after rule with specified encryption and authentication are created
        /// </summary>
        /// <param name="linuxClientAddress">Linux address</param>
        /// <param name="printerAddress">Printer Address</param>
        /// <param name="testNo">test number</param>
        /// <param name="ruleSettings"><see cref="SecurityRuleSettings"/></param>
        /// <param name="encryption"><see cref="Encryption"/></param>
        /// <param name="authentication"><see cref="Authentication"/></param>
        /// <returns>true is ping is successful, false otherwise</returns>
        private static bool VerifyPingWithAuthenticationEncryption(IPAddress linuxClientAddress, string printerAddress, int testNo, SecurityRuleSettings ruleSettings, Encryptions encryption, Authentications authentication)
        {
            try
            {
                LinuxUtils.ExecuteCommand(linuxClientAddress, "ping {0} > {1}".FormatWith(printerAddress, LinuxUtils.TEMP_FILE_PATH));
                ConfigureIPSecRule(linuxClientAddress, ruleSettings, PRESHAREDKEY, encryption: encryption, authentication: authentication);
                LinuxUtils.CreateIPSecRule(linuxClientAddress);

                IPAddress clientAddress = NetworkUtil.GetLocalAddress(linuxClientAddress.ToString(), linuxClientAddress.GetSubnetMask().ToString());
                AddressTemplateSettings addressTemplate = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, linuxClientAddress.ToString(), clientAddress.ToString());
                ServiceTemplateSettings serviceTemplate = new ServiceTemplateSettings(DefaultServiceTemplates.AllServices);

                SecurityRuleSettings windowsRuleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.HighInteroperabilityLowsecurity);

                TraceFactory.Logger.Info(windowsRuleSettings.ToString());

                CtcUtility.CreateIPsecRule(windowsRuleSettings);
                // Wait for some time for windows-linux connection
                Thread.Sleep(TimeSpan.FromMinutes(2));

                TraceFactory.Logger.Info("Rule with {0}, {1} configuration:".FormatWith(encryption.ToString(), authentication.ToString()));

                if (!PingIPv4Address(linuxClientAddress, IPAddress.Parse(printerAddress)))
                {
                    return false;
                }

                return true;
            }
            finally
            {
               // LinuxUtils.ExecuteCommand(linuxClientAddress, "reboot");
                Thread.Sleep(TimeSpan.FromMinutes(3));
                CtcUtility.DeleteAllIPsecRules();
            }
        }

        /// <summary>
        /// Start Packet Capture
        /// </summary>
        /// <param name="ipAddress">IP Address of client/ printer for getting interface name</param>
        /// <param name="testNo">Test number</param>
        /// <returns>Guid for packet capture start</returns>
        private static Guid StartPacketCapture(string ipAddress, int testNo)
        {
            return _captureUtility.StartCapture(IPAddress.Parse(ipAddress), testNo.ToString());
        }

        /// <summary>
        /// Stop Packet Capture
        /// </summary>
        /// <param name="guid">Guid processed while Starting Packet Capture</param>
        /// <returns>Packets location</returns>
        private static string StopPacketCapture(Guid guid)
        {
            if (null == guid || Guid.Empty == guid)
            {
                return string.Empty;
            }

            return _captureUtility.StopCapture(guid);
        }

        /// <summary>
        /// Validates the wire shark traces
        /// </summary>
        /// <param name="packetsPath">The packets path.</param>
        /// <param name="printerIPAddress">The IP address of the printer.</param>
        /// <param name="clientIPAddress">The IP address of the client.</param>
        /// <returns>True if the traces contains the search value with the specified filter.</returns>
        private static bool ValidatePackets(string packetsPath, string printerIPAddress, string clientIPAddress, string printerFamily = null)
        {
            if (printerFamily != PrinterFamilies.InkJet.ToString())
            {
                string filter = "(ip.src == {0}||  ip.dst == {1}) && (isakmp.exchangetype == 32 || isakmp.exchangetype == 2)".FormatWith(printerIPAddress, clientIPAddress);
                string mainMaode = "Exchange type: Identity Protection (Main Mode)";
                string quickMode = "Exchange type: Quick Mode";

                TraceFactory.Logger.Debug("Filter to Validate: {0}".FormatWith(filter));

                // Validates both Main mode and quick modes
                if (!_captureUtility.Validate(packetsPath, filter, mainMaode, quickMode))
                {
                    // TODO: remove once validation is turned on
                    //return false;
                }

                filter = "(ip.src == {0}||  ip.dst == {1}) && esp.sequence == 5".FormatWith(printerIPAddress, clientIPAddress);

                TraceFactory.Logger.Debug("Filter To Validate Packets: {0}".FormatWith(filter));
                // TODO: once validation is turned on, return from validate
                // Validates ESP sequence
                _captureUtility.Validate(packetsPath, filter, "ESP Sequence: 5");
                TraceFactory.Logger.Info("Successfull Reauthentication");
                return true;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Create Client rule. 
        /// Note: The commands are hard-coded and used for only 1 test since the Utility.CreateRule() was not working
        /// </summary>
        /// <param name="encryptions"></param>
        /// <param name="authentications"></param>
        private static void CreateClientRule(Encryptions encryptions, Authentications authentications)
        {
            CtcUtility.SetFirewallProfiles(true);
            TraceFactory.Logger.Debug("Lifetime configuration:");
            NetworkUtil.ExecuteCommand("netsh advfirewall set global mainmode mmkeylifetime 480min");
            NetworkUtil.ExecuteCommand("netsh advfirewall set global mainmode mmsecmethods dhgroup14:{0}-{1}".FormatWith(encryptions.ToString().ToLowerInvariant(), authentications.ToString().ToLowerInvariant()));
            NetworkUtil.ExecuteCommand("netsh advfirewall consec add rule name=\"ClientRule-83052\" endpoint1=any endpoint2=any protocol=any auth1=computerpsk auth1psk=\"AutomationPSK\"  mode=transport  action=requireinrequireout enable=yes");
        }

        /// <summary>
        /// Create rule in Printer and Linux machine. 		
        /// </summary>
        private static void CreateRuleinLinux(int testNo, SecurityRuleSettings ruleSettings, IPAddress linuxServerAddress, EncapsulationType encapsulationType,
                                              string printerIPAddress, ServiceTemplateSettings serviceTemplate, bool isManualKeys = false)
        {
            EwsWrapper.Instance().CreateRule(ruleSettings, true);
            EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);
            EwsWrapper.Instance().SetIPsecFirewall(true);

            bool isTunnelMode = EncapsulationType.Tunnel == encapsulationType;

            LinuxUtils.ExecuteCommand(linuxServerAddress, "ping {0} > {1}".FormatWith(printerIPAddress, LinuxUtils.TEMP_FILE_PATH));
            ConfigureIPSecRule(linuxServerAddress, ruleSettings, PRESHAREDKEY, isTunnelMode, isManualKeys);
            LinuxUtils.CreateIPSecRule(linuxServerAddress);

            IPAddress clientAddress = NetworkUtil.GetLocalAddress(linuxServerAddress.ToString(), linuxServerAddress.GetSubnetMask().ToString());
            AddressTemplateSettings addressTemplate = new AddressTemplateSettings(ADDRESSTEMPLATENAME + testNo, CustomAddressTemplateType.IPAddress, linuxServerAddress.ToString(), clientAddress.ToString());

            SecurityRuleSettings windowsRuleSettings = GetPSKRuleSettings(testNo, addressTemplate, serviceTemplate, IKESecurityStrengths.HighInteroperabilityLowsecurity);

            // Create Windows rule to access linux machine
            CtcUtility.CreateIPsecRule(windowsRuleSettings);
            // Wait for some time for windows-linux connection
            Thread.Sleep(TimeSpan.FromMinutes(2));
        }

        /// <summary>
        /// Creating Rule to validate across interfaces using Preshared, Certificates and Kerberos 		
        /// </summary>
        private static bool Rule_AcrossInterface(IPSecurityActivityData activityData, SecurityRuleSettings settings, Printer.Printer printer)
        {
            string clientNetwork = string.Empty;

            // create rule with Custom Service Template settings
            EwsWrapper.Instance().CreateRule(settings, true);

            // setting default rule action to Allow
            EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Allow);

            // setting IPsec option to true
            EwsWrapper.Instance().SetIPsecFirewall(true);

            if (settings.IPsecTemplate.DynamicKeysSettings.V1Settings.KerberosSettings == null)
            {
                // Creating similar rule in client
                CtcUtility.CreateIPsecRule(settings);

                // disabling 1 of the network card of the client so that the client and printer will be in different network
                clientNetwork = CtcUtility.GetClientNetworkName(activityData.PrimaryDhcpServerIPAddress);

                NetworkUtil.DisableNetworkConnection(clientNetwork);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                // Printer accessibility should pass since rule is created in printer and client across subnets
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                {
                    TraceFactory.Logger.Info("Failed to access the Printer across Subnets after disabling network connection");
                    return false;
                }

                // Enabling back
                NetworkUtil.EnableNetworkConnection(clientNetwork);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                CtcUtility.DeleteAllIPsecRules();
                return EwsWrapper.Instance().DeleteAllRules();
            }
            else
            {
                // yet to update the code           
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);
                connectivityService.Channel.CreateIPsecRule(settings, enableRule: true, enableProfiles: false);
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);

                //TraceFactory.Logger.Info("Disabling the Primary Server Client and validating the Printer accessibility in Secondary Client");
                //if(!connectivityService.Channel.DisableNetworkConnectionAndPing(IPAddress.Parse(activityData.WiredIPv4Address), activityData.PrimaryDhcpServerIPAddress))
                //{
                //    TraceFactory.Logger.Info("Fail to access the Printer after disabling the Primary Network Connection in Secondary Client");
                //    return false;
                //}
                //TraceFactory.Logger.Info("Able to access the Printer even after disabling the Primary Network Connection in Secondary Client");
                TraceFactory.Logger.Info("Deleting Ipsec Rules from Secodary client");
                connectivityService.Channel.DeleteAllIPsecRules();
                CtcUtility.DeleteAllIPsecRules();
                return EwsWrapper.Instance().DeleteAllRules();
            }
        }

        /// <summary>
        /// Creating Rule to validate hose break using Preshared, Certificates and Kerberos 		
        /// </summary>
        private static bool RuleWithHoseBreak(int testNo, IPSecurityActivityData activityData, SecurityRuleSettings settings, INetworkSwitch networkSwitch)
        {
            Printer.Printer printer = Printer.PrinterFactory.Create(activityData.ProductFamily, activityData.WiredIPv4Address);
            //MessageBox.Show("Check Statefull address");
            string ipv6StatefullAddress = activityData.IPV6StatefullAddress.ToString();
            string ipv6StatelessAddress = activityData.IPV6StatelessAddress[0].ToString();
            //string ipv6StatefullAddress = printer.IPv6StateFullAddress.ToString();
            //string ipv6StatelessAddress = printer.IPv6StatelessAddresses[0].ToString();

            // Create rule with basic settings
            EwsWrapper.Instance().CreateRule(settings, true);

            // Setting default rule action to Drop				
            EwsWrapper.Instance().SetDefaultRuleAction(DefaultAction.Drop);

            // Setting IPsec option to true
            EwsWrapper.Instance().SetIPsecFirewall(true);

            if (settings.IPsecTemplate.DynamicKeysSettings.V1Settings.KerberosSettings == null)
            {
                // Creating Rule in Client
                CtcUtility.CreateIPsecRule(settings, true);

                // Printer accessibility should pass since rule is created in printer and client
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                {
                    return false;
                }

                if (!CtcUtility.ValidateDeviceServices(printer, ipv6StatefullAddress, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                {
                    return false;
                }

                if (!CtcUtility.ValidateDeviceServices(printer, ipv6StatelessAddress, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                {
                    return false;
                }

                // Disabling the Rule to access Switch IP Adddress
                CtcUtility.DisableIPsecRule(CLIENT_RULE_NAME.FormatWith(testNo));

                // Removing Ethernet cable from the device
                if (!networkSwitch.DisablePort(activityData.PortNo))
                {
                    TraceFactory.Logger.Info("Failed to disable the Port No: {0}".FormatWith(activityData.PortNo));
                    return false;
                }
                Thread.Sleep(TimeSpan.FromMinutes(1));

                CtcUtility.EnableIPsecRule(CLIENT_RULE_NAME.FormatWith(testNo));
                TraceFactory.Logger.Info(" Printer accessibility should fail since Ethernet cable is removed from the device");
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Fail))
                {
                    return false;
                }
                if (!CtcUtility.ValidateDeviceServices(printer, ipv6StatefullAddress, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Fail))
                {
                    return false;
                }
                if (!CtcUtility.ValidateDeviceServices(printer, ipv6StatelessAddress, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Fail))
                {
                    return false;
                }

                // Disabling the Rule to access Switch IP Adddress
                CtcUtility.DisableIPsecRule(CLIENT_RULE_NAME.FormatWith(testNo));

                // Connecting back the Ethernet cable 
                networkSwitch.EnablePort(activityData.PortNo);

                Thread.Sleep(TimeSpan.FromMinutes(1));

                CtcUtility.EnableIPsecRule(CLIENT_RULE_NAME.FormatWith(testNo));

                TraceFactory.Logger.Info("Printer accessibility should pass since Ethernet cable is connected back to the device");
                if (!CtcUtility.ValidateDeviceServices(printer, isMessageBoxChecked: activityData.MessageBoxCheckBox, ping: DeviceServiceState.Pass))
                {
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Since the port is enabled back, able to access the printer with ipv4");
                    return true;
                }
            }
            else
            {
                ConnectivityUtilityServiceClient connectivityService = ConnectivityUtilityServiceClient.Create(activityData.WindowsSecondaryClientIPAddress);

                // Rebooting secondary client machine 
                connectivityService.Channel.RebootMachine(IPAddress.Parse(activityData.WindowsSecondaryClientIPAddress));
                TraceFactory.Logger.Debug("Waiting for secondary client to reboot and come to ready state.");
                Thread.Sleep(TimeSpan.FromMinutes(2));

                connectivityService.Channel.CreateIPsecRule(settings, enableRule: true, enableProfiles: false);
                connectivityService.Channel.EnableFirewallProfile(FirewallProfile.PrivateDomainAndPublic, true);

                TraceFactory.Logger.Info("Validating the accessibility of the Printer in Secondary Client for Kerberos");
                if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    TraceFactory.Logger.Info("Failed to access the Printer IPV4 address from Secondary Client");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Since the rule is created in Secondary Client, able to access the printer with ipv4");
                }

                // Removing Ethernet cable from the device
                networkSwitch.DisablePort(activityData.PortNo);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Since the port is disabled , failed to access the printer with ipv4");
                }

                // Connecting back the Ethernet cable 
                networkSwitch.EnablePort(activityData.PortNo);
                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (!connectivityService.Channel.Ping(IPAddress.Parse(activityData.WiredIPv4Address)))
                {
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Since the port is enabled back, able to access the printer with ipv4");
                }

                return true;
            }

            #endregion
        }
    }
}
