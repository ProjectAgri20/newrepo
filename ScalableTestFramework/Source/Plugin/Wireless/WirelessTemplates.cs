using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.AccessPoint;
using HP.ScalableTest.DeviceAutomation.Wireless;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.NetworkSwitch;
using HP.ScalableTest.PluginSupport.Connectivity.Printer;
using HP.ScalableTest.PluginSupport.Connectivity.RadiusServer;
using HP.ScalableTest.PluginSupport.Connectivity.Telnet;
using HP.ScalableTest.Utility;
using CoreUtility = HP.ScalableTest.Utility;
using HP.ScalableTest.PluginSupport.Connectivity.PowerSwitch;

namespace HP.ScalableTest.Plugin.Wireless
{
    public enum WPAKeyType
    {
        Passphrase,
        Presharedkey
    }

    public class WirelessTemplates
    {
        #region Constants

        private const string WPA_PASSPHRASE = "12345678";
        private const string WPA_PRESHAREDKEY = "485BFE660D5F61003E79CD7EBB485BFE660D5F61003E79CD7EBBAA8465E6A4A";
        private const string KEYRENEWAL = "3600";
        private const string SSID0 = "{0}";
        private const string CISCO_USERNAME = "admin";
        private const string CISCO_PASSWORD = "1iso*help";
        private const string RADIUS_PORT = "1812";
        private const string SHARED_SECRET = "xyzzy";
        private const string DOT1X_USERNAME = "{0}user";
        private const string DOT1X_PASSWORD = "1iso*help";
        private const string NETWORK_POLICY = "policy1";
        private const string CA_CERTIFICATEPATH = @"ConnectivityShare\Certificates\802.1x_Certificates\{0}\WirelessEnterprise\{1}\CA_certificate.cer";
        private const string ID_CERTIFICATEPATH = @"ConnectivityShare\Certificates\802.1x_Certificates\{0}\WirelessEnterprise\{1}\ID_certificate.pfx";
        private const string WPS_SUPPORTED_MODEL = "WAP321";
        private const string WIFIAUTO_PASSWORD = "12345678";
        private const string WIFIADVANCED_PASSWORD = "directpassword";
        private const string WIFIDIRECTIP = "192.168.223.1";
        private const string WIFIADMIN_PASSWORD = "WiFiAdmin@123";

        #endregion Constants

        #region Local Variables

        private bool _installCertificates = true;
        private int _testNumber;
        private readonly WirelessActivityData _activityData;

        #endregion Local Variables

        /// <summary>
        /// Constructs the wireless template class for the test
        /// </summary>
        /// <param name="activityData">activityData</param>
        /// <param name="testNumber">Test Id</param>
        public WirelessTemplates(WirelessActivityData activityData, int testNumber)
        {
            _testNumber = testNumber;
            _activityData = activityData;
        }

        public WirelessTemplates(WirelessActivityData activityData)
        {
            _activityData = activityData;
        }

        public void SetTestNumber(int testId)
        {
            _testNumber = testId;
        }

        #region Templates

        #region Wireless Network Security

        #region WPA Personal, WEP Personal, WPA Enterprise, WEP Enterprise

        /// <summary>
        /// 756582	Verify  no-security authentication in infrastructure network.
        /// "1.Open the EWS Page
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable Infrastructre mode with SSID
        /// 4.No-Security authentication method configured."
        /// Device assocaition should be successfull using no secuirty method
        /// </summary>
        /// <returns></returns>
        public bool NoSecurity()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                Profile accessPointProfile = GetAccessPointProfile(_activityData.WirelessRadio);

                WirelessSettings settings = new WirelessSettings { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

                WirelessSecuritySettings securitySettings = new WirelessSecuritySettings();

                return TestWireless(accessPointProfile, settings, securitySettings);
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756631
        /// Verify WPA-Personal authentication with AES using Passphrase
        /// 1.Open the EWS Page.
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WPA-Personal mode with SSID.
        /// 4.Select WPA Authentication and AES as a Encryption.
        /// 5.Enter the passphrase.
        /// Pre-request:1. Create an access point with WPA-Personal and AES settings.
        /// Device to be successfully associate using WPA-Personal mode in Infrastructure mode.
        /// </summary>
        /// <returns></returns>
        public bool WPAPersonal_WPA_AES_Passphrase()
        {
            if (!TestPreRequisites())
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                return ConfigureWpaPersonal(WpaVersion.WPA, WpaAlgorithm.AES, WPAKeyType.Passphrase);
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756587
        /// Verify WPA-Personal authentication with AES using Passphrase
        /// 1.Open the EWS Page.
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WPA-Personal mode with SSID.
        /// 4.Select WPA Authentication and AES as a Encryption.
        /// 5.Enter the passphrase.
        /// Pre-request:1. Create an access point with WPA-Personal and AES settings.
        /// Device to be successfully associate using WPA-Personal mode in Infrastructure mode.
        /// 756587
        /// Verify WPA-Personal authentication using SNMP.
        /// 1.Open MIB Browser.
        /// 2.Select below mentioned oid's list to enable WEP key with open mode
        /// Wireless -WPA-WPA-PSK
        /// 1.2.840.10036.1.1.1.10.1 - Value: 1 // Infrastructure mode
        /// 1.3.6.1.4.1.11.2.4.3.20.5.0 - Value: Hex-String // SSID name
        /// 1.2.840.10036.1.2.1.3.1.1 - Value: 1 // OPEN algo
        /// 1.3.6.1.4.1.11.2.4.3.20.1.0 - Value: 0 // NO authentication server
        /// 1.3.6.1.4.1.11.2.4.3.20.42.0 - Value: 2 // WPA for WPA
        /// 1.3.6.1.4.1.11.2.4.3.20.28.0 - Value: 4 // AUTO for encryption
        /// 1.2.840.10036.7.3.1.3.2 - Value: 1 // WPA enabled
        /// 1.3.6.1.4.1.11.2.4.3.20.36.0 - Value: Hex-String // encrypted Pass-phrase string
        /// Device to be successfully associate using WPA-Personal mode in Infrastructure mode.
        /// </summary>
        /// <returns></returns>
        public bool WPAPersonal_WPA_AESAUTO_Passphrase()
        {
            if (!TestPreRequisites())
            {
                return false;
            }
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Wireless configuration through Web UI {CtcBaseTests.STEP_DELIMETER}");
                if (!ConfigureWpaPersonal(WpaVersion.WPA, WpaAlgorithm.AES, WPAKeyType.Passphrase))
                {
                    return false;
                }

                if (_activityData.ProductFamily == ProductFamilies.VEP)
                {
                    TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Wireless configuration through SNMP {CtcBaseTests.STEP_DELIMETER}");
                    return ConfigureWpaPersonal(WpaVersion.WPA, WpaAlgorithm.TKIP_AES, WPAKeyType.Passphrase, PrinterAccessType.SNMP);
                }
                else
                {
                    return true;
                }
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756589
        /// Verify WPA-Personal authentication using SNMP.
        /// 1.Open MIB Browser.
        /// 2.Select below mentioned oid's list to enable WEP key with open mode
        /// Wireless -AUTO-WPA-PSK
        /// 1.2.840.10036.1.1.1.10.1 - Value: 1 // Infrastructure mode
        /// 1.3.6.1.4.1.11.2.4.3.20.5.0 - Value: Hex-String // SSID name
        /// 1.2.840.10036.1.2.1.3.1.1 - Value: 1 // OPEN algo
        /// 1.3.6.1.4.1.11.2.4.3.20.1.0 - Value: 0 // NO authentication server
        /// 1.3.6.1.4.1.11.2.4.3.20.42.0 - Value: 4 // AUTO for WPA
        /// 1.3.6.1.4.1.11.2.4.3.20.28.0 - Value: 4 // AUTO for encryption
        /// 1.2.840.10036.7.3.1.3.2 - Value: 1 // WPA enabled
        /// 1.3.6.1.4.1.11.2.4.3.20.36.0 - Value: Hex-String // encrypted Pass-phrase string"
        /// Device to be successfully associate using WPA-Personal mode in Infrastructure mode.
        /// Verify WPA-Personal authentication with AUTO encryption method(AES then TKIP)
        /// 1.Open the EWS Page.
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WPA-Personal mode with SSID.
        /// 4.Select WPA Authentication and AUTO as a Encryption.
        /// 5.Enter the pre-shared key.
        /// Pre-request:1. Create an access point with WPA-Personal and AUTO settings.
        /// Device to be successfully associate using WPA-Personal mode in Infrastructure mode.
        /// </summary>
        /// <returns></returns>
        public bool WPAPersonal_AUTOWPA_AUTO_PassphrasePSK()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                if (_activityData.ProductFamily == ProductFamilies.VEP)
                {
                    TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Wireless configuration through Web UI {CtcBaseTests.STEP_DELIMETER}");
                    if (!ConfigureWpaPersonal(WpaVersion.WPA_WPA2, WpaAlgorithm.TKIP_AES, WPAKeyType.Passphrase, PrinterAccessType.SNMP))
                    {
                        return false;
                    }
                }

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Wireless configuration through SNMP {CtcBaseTests.STEP_DELIMETER}");

                return ConfigureWpaPersonal(WpaVersion.WPA, WpaAlgorithm.TKIP_AES, WPAKeyType.Presharedkey);
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756588
        /// Verify WPA-Personal authentication with TKIP using Pre-Shared Key.
        /// 1.Open the EWS Page.
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WPA-Personal mode with SSID.
        /// 4.Select WPA Authentication and TKIP as a Encryption.
        /// 5.Enter the pre-shared key.
        /// Pre-request:1. Create an access point with WPA-Personal and TKIP settings.
        /// Device to be successfully associate using WPA-Personal mode in Infrastructure mode.
        /// 756588
        /// Verify WPA-Personal authentication with TKIP using Passphrase.
        /// 1.Open the EWS Page.
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WPA-Personal mode with SSID.
        /// 4.Select WPA Authentication and TKIP as a Encryption.
        /// 5.Enter the Passphrase.
        /// Pre-request:1. Create an access point with WPA-Personal and TKIP settings.Device to be successfully associate using WPA-Personal mode in Infrastructure mode.
        /// </summary>
        /// <returns></returns>
        public bool WPAPersonal_WPA_TKIP_PSKPassphrase()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                if (!ConfigureWpaPersonal(WpaVersion.WPA, WpaAlgorithm.TKIP, WPAKeyType.Presharedkey))
                {
                    return false;
                }

                if (!EnablePrimaryAddress())
                {
                    return false;
                }

                return ConfigureWpaPersonal(WpaVersion.WPA, WpaAlgorithm.TKIP, WPAKeyType.Passphrase);
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756594
        /// Verify AUTO (WPA2 then WPA) authentication with AES encryption method
        /// 1.Open the EWS Page.
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WPA-Personal mode with SSID.
        /// 4.Select AUTO Authentication and AES as a Encryption.
        /// 5.Enter the pre-shared key.
        /// Pre-request:1. Create an access point with AUTO and AES settings.
        /// Device to be successfully associate using WPA-Personal mode in Infrastructure mode.
        /// </summary>
        /// <returns></returns>
        public bool WPAPersonal_AUTO_AES_PSK()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                return ConfigureWpaPersonal(WpaVersion.WPA_WPA2, WpaAlgorithm.AES, WPAKeyType.Presharedkey);
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756593
        /// Verify AUTO (WPA2 then WPA) authentication with AUTO encryption method (AES then TKIP)
        /// 1.Open the EWS Page.
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WPA-Personal mode with SSID.
        /// 4.Select AUTO Authentication and AUTO as a Encryption.
        /// 5.Enter the pre-shared key.
        /// Pre-request:1. Create an access point with AUTO and AUTO settings.
        /// Device to be successfully associate using WPA-Personal mode in Infrastructure mode.
        /// </summary>
        /// <returns></returns>
        public bool WPAPersonal_AUTO_AUTO_PSK()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                return ConfigureWpaPersonal(WpaVersion.WPA_WPA2, WpaAlgorithm.TKIP_AES, WPAKeyType.Presharedkey);
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756590
        /// Verify WPA2-Personal authentication with AES using Passphrase
        /// 1.Open the EWS Page.
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WPA2-Personal mode with SSID.
        /// 4.Select WPA2 Authentication and AES as a Encryption.
        /// 5.Enter the passphrase.
        /// Pre-request:
        /// 1. Create an access point with WPA2-Personal and AES settings."
        /// Device to be successfully associate using WPA-Personal mode in Infrastructure mode.
        /// Verify WPA2-Personal authentication with AES using Pre-Shared Key.
        /// "1.Open the EWS Page.
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WPA2-Personal mode with SSID.
        /// 4.Select WPA2 Authentication and AES as a Encryption.
        /// 5.Enter the pre-shared key.
        /// Pre-request:
        /// 1. Create an access point with WPA2-Personal and AES settings."
        /// Device to be successfully associate using WPA-Personal mode in Infrastructure mode.
        /// Verify WPA2-Personal authentication using SNMP.
        /// "1.Open MIB Browser.
        /// 2.Select below mentioned oid's list to enable WEP key with open mode
        /// Wireless - WPA2-WPA-PSK
        /// 1.2.840.10036.1.1.1.10.1 - Value: 1 // Infrastructure mode
        /// 1.3.6.1.4.1.11.2.4.3.20.5.0 - Value: Hex-String // SSID name
        /// 1.2.840.10036.1.2.1.3.1.1 - Value: 1 // OPEN algo
        /// 1.3.6.1.4.1.11.2.4.3.20.1.0 - Value: 0 // NO authentication server
        /// 1.3.6.1.4.1.11.2.4.3.20.42.0 - Value: 3 // WPA2 for WPA
        /// 1.3.6.1.4.1.11.2.4.3.20.28.0 - Value: 4 // AUTO for encryption
        ///  1.2.840.10036.7.3.1.3.2 - Value: 1 // WPA enabled
        /// 1.3.6.1.4.1.11.2.4.3.20.36.0 - Value: Hex-String // encrypted Pass-phrase string"
        /// Device to be successfully associate using WPA-Personal mode in Infrastructure mode.
        /// </summary>
        /// <returns></returns>
        public bool WPAPersonal_WPA2_AESAUTO_Passphrase()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Wireless configuration through Web UI {CtcBaseTests.STEP_DELIMETER}");
                if (!ConfigureWpaPersonal(WpaVersion.WPA2, WpaAlgorithm.AES, WPAKeyType.Passphrase))
                {
                    return false;
                }

                //if (!EnablePrimaryAddress(activityData))
                //{
                //	return false;
                //}

                if (_activityData.ProductFamily == ProductFamilies.VEP)
                {
                    TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Wireless configuration through SNMP {CtcBaseTests.STEP_DELIMETER}");
                    return ConfigureWpaPersonal(WpaVersion.WPA2, WpaAlgorithm.TKIP_AES, WPAKeyType.Presharedkey, PrinterAccessType.SNMP);
                }
                else
                {
                    return true;
                }
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756592
        /// Verify WPA2-Personal authentication with AUTO encryption method (AES then TKIP)
        /// 1.Open the EWS Page.
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WPA2-Personal mode with SSID.
        /// 4.Select WPA2 Authentication and AUTO as a Encryption.
        /// 5.Enter the pre-shared key.
        /// Pre-request:1. Create an access point with WPA2-Personal and AUTO settings.
        /// Device to be successfully associate using WPA-Personal mode in Infrastructure mode.
        /// </summary>
        /// <returns></returns>
        public bool WPAPersonal_WPA2_AUTO_PSK()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                return ConfigureWpaPersonal(WpaVersion.WPA2, WpaAlgorithm.TKIP_AES, WPAKeyType.Presharedkey);
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756591
        /// Verify WPA2-Personal authentication with TKIP using Pre-Shared Key.
        /// 1.Open the EWS Page.
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WPA2-Personal mode with SSID.
        /// 4.Select WPA2 Authentication and TKIP as a Encryption.
        /// 5.Enter the pre-shared key.
        /// Pre-request:1. Create an access point with WPA2-Personal and TKIP settings.
        /// Device to be successfully associate using WPA-Personal mode in Infrastructure mode.
        /// Verify WPA2-Personal authentication with TKIP using Passphrase.
        /// 1.Open the EWS Page.
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WPA2-Personal mode with SSID.
        /// 4.Select WPA2 Authentication and TKIP as a Encryption.
        /// 5.Enter the Passphrase.
        /// Pre-request:1. Create an access point with WPA2-Personal and TKIP settings.
        /// Device to be successfully associate using WPA-Personal mode in Infrastructure mode.
        /// </summary>
        /// <returns></returns>
        public bool WPAPersonal_WPA2_TKIP_PSKPassphrase()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                if (!ConfigureWpaPersonal(WpaVersion.WPA2, WpaAlgorithm.TKIP, WPAKeyType.Presharedkey))
                {
                    return false;
                }

                if (!EnablePrimaryAddress())
                {
                    return false;
                }

                return ConfigureWpaPersonal(WpaVersion.WPA2, WpaAlgorithm.TKIP, WPAKeyType.Passphrase);
            }
            finally
            {
                TestPostRequisites();
            }
        }

        public bool WPAPersonal_AUTO_TKIP_PSK()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                return ConfigureWpaPersonal(WpaVersion.WPA_WPA2, WpaAlgorithm.TKIP, WPAKeyType.Presharedkey);
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756586
        /// Verify Shared key authentication with WEP key in Infrastructure mode
        /// 1.Open the EWS Page
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WEP mode with SSID.
        /// 4.Enter SSID->Infrastructure mode->Encryption as WEP enter the key and finish the process
        /// Device association should be successfull using shared authentication with WEP in Infrastructure mode.
        /// Verify Shared key authentication with WEP key in Infrastructure mode using SNMP
        /// 1.Open MIB Browser
        /// 2.Select below mentioned oid's list to enable WEP key with shared mode.
        /// Wireless - WEP:Shared
        /// 1.2.840.10036.1.1.1.10.1 - Value: 1 - Infrastructure mode
        /// 1.2.840.10036.1.1.1.9.1  Value : ""webui"" - SSID name
        /// 1.2.840.10036.1.2.1.3.1.1 Value : 2 - Making algo SHARED
        /// 1.3.6.1.4.1.11.2.4.3.20.1.0 Value : 0 - Making NO authentication server
        /// 1.2.840.10036.1.5.1.1.1 Value: 1 - Making WEP true
        /// 1.2.840.10036.1.5.1.2.1 Value: 0 - Default Key Id.
        /// 1.3.6.1.4.1.11.2.4.3.20.7.1.2.1 Value: Hex-String - encrypted WEP key.
        /// Device association should be successfull using shared authentication with WEP in Infrastructure mode.
        /// </summary>
        /// <returns></returns>
        public bool WEP_SharedKey()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                return ConfigureWepPersonal(WEPModes.Shared, WEPIndices.Key1, KeySize.Bit_64, AuthenticationType.Shared_Key);
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756583
        /// Verify  Open  authentication with 64-bit WEP key in Infrastructure mode
        /// 1.Open the EWS Page
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WEP mode with SSID.
        /// 4.configure a 64-bit hex WEP key"
        /// Device association should be successfull using open authentication with WEP in Infrastructure mode.
        /// 756583	Verify Open  authentication with 64-bit WEP key in Infrastructure mode
        /// "1.Open the EWS Page
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WEP mode with SSID.
        /// 4.conifgure a 64-bit ASCII WEP key"
        /// Device association should be successfull using shared authentication with WEP in Infrastructure mode.
        /// <returns></returns>
        /// </summary>
        public bool WepKeyLengths(KeySize keyLength)
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info($"Step I: Configuring WEP using {keyLength} bit HEX key");
                if (!ConfigureWepPersonal(WEPModes.Shared, WEPIndices.Key1, keyLength, AuthenticationType.Shared_Key))
                {
                    return false;
                }

                if (!_activityData.AccessPointVendor.EqualsIgnoreCase("Cisco"))
                {
                    if (!EnablePrimaryAddress())
                    {
                        return false;
                    }

                    TraceFactory.Logger.Info($"Step II: Configuring WEP using {keyLength} bit ASCII key");
                    return ConfigureWepPersonal(WEPModes.Shared, WEPIndices.Key1, keyLength, AuthenticationType.Shared_Key);
                }

                return true;
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756585
        /// Verify WEP association using WEP key 1
        /// 1.Open the EWS Page
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WEP mode with SSID.
        /// 4.Set the transmit key to 1.
        /// 5.Set the device WEP key 1 to match that of the Access Points
        /// 6. Disable other keys.
        /// 7.WEP key method configured.
        /// Verify WEP association using WEP key 2
        /// 1.Open the EWS Page
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WEP mode with SSID.
        /// 4.Set the transmit key to 2.
        /// 5.Set the device WEP key 2 to match that of the Access Points
        /// 6.Disable other keys.
        /// 7.WEP key method configured.
        /// Verify WEP association using WEP key 3
        /// 1.Open the EWS Page
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WEP mode with SSID.
        /// 4.Set the transmit key to 3.
        /// 5.Set the device WEP key 3 to match that of the Access Points
        /// 6.Disable other keys.
        /// 7.WEP key method configured.
        /// Verify WEP association using WEP key 4
        /// "1.Open the EWS Page
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WEP mode with SSID.
        /// 4.Set the transmit key to 4.
        /// 5.Set the device WEP key 4 to match that of the Access Points
        /// 6. Disable other keys.
        /// 7.WEP key method configured.
        /// 756585	Verify WEP association with Transmit key mismatch
        /// "1.Open the EWS Page
        /// 2.Goto Network->Wireless web page.
        /// 3.Enable WEP mode with SSID.
        /// 4.Set the transmit key to 4 on device.
        /// 5. Associate With Accesspoint configured with WEP and KEY index 4
        /// 6.After Successfull assocation
        /// 7.Set the WEP Key index to 1 on the Device and verify the assocation"
        /// </summary>
        /// <returns></returns>
        public bool WepKeyIndex()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Step I: WEP Association with Key 1");
                Profile accessPointProfile = GetAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), AuthenticationType.Open_System, TransmitKeySelect.KeySelect1, KeySize.Bit_64, _activityData.WirelessRadio);

                WirelessSettings settings = new WirelessSettings()
                { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

                WirelessSecuritySettings securitySettings = new WirelessSecuritySettings(new WEPPersonalSettings() { WEPIndex = WEPIndices.Key1, WEPKey = GetWepKey(accessPointProfile, WEPIndices.Key1), WEPMode = WEPModes.Open });

                if (!TestWireless(accessPointProfile, settings, securitySettings))
                {
                    return false;
                }

                if (!EnablePrimaryAddress())
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: WEP Association with Key 2");

                accessPointProfile.SecuritySettings.TransmitKeySelect = TransmitKeySelect.KeySelect2;

                securitySettings = new WirelessSecuritySettings(new WEPPersonalSettings() { WEPIndex = WEPIndices.Key2, WEPKey = GetWepKey(accessPointProfile, WEPIndices.Key2), WEPMode = WEPModes.Open });

                if (!TestWireless(accessPointProfile, settings, securitySettings))
                {
                    return false;
                }

                if (!EnablePrimaryAddress())
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step III: WEP Association with Key 3");
                accessPointProfile.SecuritySettings.TransmitKeySelect = TransmitKeySelect.KeySelect3;

                securitySettings = new WirelessSecuritySettings(new WEPPersonalSettings() { WEPIndex = WEPIndices.Key3, WEPKey = GetWepKey(accessPointProfile, WEPIndices.Key3), WEPMode = WEPModes.Open });

                if (!TestWireless(accessPointProfile, settings, securitySettings))
                {
                    return false;
                }

                if (!EnablePrimaryAddress())
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step IV: WEP Association with Key 4");
                accessPointProfile.SecuritySettings.TransmitKeySelect = TransmitKeySelect.KeySelect4;

                securitySettings = new WirelessSecuritySettings(new WEPPersonalSettings() { WEPIndex = WEPIndices.Key4, WEPKey = GetWepKey(accessPointProfile, WEPIndices.Key4), WEPMode = WEPModes.Open });

                if (!TestWireless(accessPointProfile, settings, securitySettings))
                {
                    return false;
                }

                if (!EnablePrimaryAddress())
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step V: WEP Association with key index 4 on access point and key 1 on printer.");
                accessPointProfile.SecuritySettings.TransmitKeySelect = TransmitKeySelect.KeySelect4;
                securitySettings = new WirelessSecuritySettings(new WEPPersonalSettings() { WEPIndex = WEPIndices.Key1, WEPKey = GetWepKey(accessPointProfile, WEPIndices.Key1), WEPMode = WEPModes.Open });

                if (!EwsWrapper.Instance().ConfigureWireless(settings, securitySettings, _activityData.PrinterInterfaceType, validate: _activityData.PrinterInterfaceType != ProductType.MultipleInterface))
                {
                    return false;
                }

                if (_activityData.ProductFamily != ProductFamilies.VEP)
                {
                    INetworkSwitch networkSwitch = SwitchFactory.Create(_activityData.SwitchIpAddress);
                    networkSwitch.DisablePort(_activityData.PrimaryInterfaceAddressPortNumber);

                    return !CoreUtility.Retry.UntilTrue(() => !string.IsNullOrEmpty(_activityData.WirelessInterfaceAddress = CtcUtility.GetPrinterIPAddress(_activityData.WirelessMacAddress)), 5, TimeSpan.FromSeconds(20));
                }
                else
                {
                    if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromMinutes(1)))
                    {
                        TraceFactory.Logger.Info($"Ping with IP address: {_activityData.WirelessInterfaceAddress} is successful. Expected: ping failure.");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info($"Ping with IP address: {_activityData.WirelessInterfaceAddress} failed. Expected: ping failure.");
                        return true;
                    }
                }
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756638
        /// With SHA2-512 Certificates
        /// "Install certificates wtih SHA2-512 for CA cert
        /// Configure PEAP on server with TLS as priority"
        /// 802.1x wireless authenctication should be successful
        /// 756638
        /// EAP PEAP associaiton
        /// "Configure an access point in WEP and PEAP authentication mode.
        /// Configure the Jetdirect print server to match the access point.
        ///  Submit a print job."
        /// Print job should be printed After successful association
        /// 756635 EAP-TLS authentication with SHA2-512 Certificates
        /// "Configure the Radius-Server in EAP-TLS authentication mode.
        /// Configure the Jet-direct print server to match the radius authentication.
        /// Submit a print job."
        /// 802.1x wireless authenctication should be successful
        /// 756635	EAP TLS associaiton
        /// "Configure an access point With WEP  and  EAP-TLS authentication mode.
        /// Configure the Jetdirect print server to match the access point.
        /// Submit a print job."
        /// </summary>
        /// <returns></returns>
        public bool VerifyWepEnterprise(AuthenticationMode printerAuthMode, AuthenticationMode serverAuthMode, AuthenticationMode serverPriority = AuthenticationMode.None)
        {
            if (_activityData.PrinterInterfaceType != ProductType.MultipleInterface)
            {
                TraceFactory.Logger.Info("WEP Enterprise is appplicable only for MI Products. Current product: {0}".FormatWith(_activityData.ProductName));
                return false;
            }

            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                return ConfigureEnterpriseWireless(Modes.WepEnterprise, WpaAlgorithm.Undefined, WpaVersion.Undefined, printerAuthMode, serverAuthMode, EncryptionStrengths.Medium, serverPriority: serverPriority);
            }
            finally
            {
                TestPostRequisites(true);
            }
        }

        /// <summary>
        /// 756636
        /// With SHA2-512 Certificates
        /// "Install certificates wtih SHA2-512 for CA cert
        /// Configure PEAP on server with TLS as priority"
        /// 802.1x wireless authenctication should be successful
        /// 756636	EAP PEAP associaiton
        /// "Configure an access point with WPA TKIP and PEAP authentication mode.
        /// Configure the Jetdirect print server to match the access point.
        ///  Submit a print job."
        /// Print job should be printed After successful association
        /// 756633
        /// EAP-TLS authentication with SHA2-512 Certificates
        /// "Configure the Radius-Server in EAP-TLS authentication mode.
        /// Configure the Jet-direct print server to match the radius authentication.
        /// Submit a print job."
        /// 802.1x wireless authenctication should be successful
        /// 756633 EAP TLS associaiton
        /// "Configure an access point With WPA TKIP and EAP-TLS authentication mode.
        /// Configure the Jetdirect print server to match the access point.
        /// Submit a print job."
        /// Print job should be printed After successful association
        /// </summary>
        /// <param name="serverAuthMode"></param>
        /// <param name="printerAuthMode"></param>
        /// <param name="serverPriority"></param>
        /// <returns></returns>
        public bool VerifyWpaEnterprise(AuthenticationMode printerAuthMode, AuthenticationMode serverAuthMode, AuthenticationMode serverPriority = AuthenticationMode.None)
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                return ConfigureEnterpriseWireless(Modes.WpaEnterprise, WpaAlgorithm.TKIP, WpaVersion.WPA, printerAuthMode, serverAuthMode, EncryptionStrengths.Medium, serverPriority);
            }
            finally
            {
                TestPostRequisites(true);
            }
        }

        /// <summary>
        /// 756637
        /// With SHA2-512 Certificates
        /// "Install certificates wtih SHA2-512 for CA cert
        /// Configure PEAP on server with TLS as priority"
        /// 802.1x wireless authenctication should be successful
        /// 756637	EAP PEAP associaiton
        /// "Configure an access point With WPA2 AES and PEAP authentication mode.
        /// Configure the Jetdirect print server to match the access point.
        /// Submit a print job.
        /// Note: Repeate this test with different encryption strengths (High, Medium and Low)."
        /// Print job should be printed After successful association
        /// 756634
        /// EAP-TLS authentication with SHA2-512 Certificates
        /// "Configure the Radius-Server in EAP-TLS authentication mode.
        /// Configure the Jet-direct print server to match the radius authentication.
        /// Submit a print job."
        /// 802.1x wireless authenctication should be successful
        /// 756634	EAP TLS associaiton
        /// "Configure an access point with WPA2 AES and EAP-TLS authentication mode.
        /// Configure the Jetdirect print server to match the access point.
        /// Submit a print job.
        /// Note: Repeate this test with different encryption strengths (High, Medium and Low)."
        /// Print job should be printed After successful association
        /// </summary>
        /// <param name="serverAuthMode"></param>
        /// <param name="printerAuthMode"></param>
        /// <param name="serverPriority"></param>
        /// <returns></returns>
        public bool VerifyWpa2Enterprise(AuthenticationMode printerAuthMode, AuthenticationMode serverAuthMode, AuthenticationMode serverPriority = AuthenticationMode.None)
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            if (!ConfigureRadiusClient(IPAddress.Parse(_activityData.RadiusServerIp), _activityData.AccessPointDetails.FirstOrDefault().Address))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info($"Step I: WPA2 Enterprise with AES as encryption, encryption strength: Medium and {printerAuthMode} authentication.");
                if (!ConfigureEnterpriseWireless(Modes.WpaEnterprise, WpaAlgorithm.AES, WpaVersion.WPA2, printerAuthMode, serverAuthMode, EncryptionStrengths.Medium, serverPriority))
                {
                    return false;
                }

                if (!EnablePrimaryAddress())
                {
                    return false;
                }

                TraceFactory.Logger.Info($"Step II: WPA2 Enterprise with AES as encryption, encryption strength: High and {printerAuthMode} authentication.");

                if (_activityData.PrinterInterfaceType == ProductType.MultipleInterface)
                {
                    if (!EwsWrapper.Instance().ConfigureCiphers(EncryptionStrengths.High))
                    {
                        return false;
                    }

                    if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromMinutes(2)))
                    {
                        TraceFactory.Logger.Info($"Ping is successful with wireless IP address: {_activityData.WirelessInterfaceAddress}.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info($"Ping failed with wireless IP address: {_activityData.WirelessInterfaceAddress}.");
                        return true;
                    }
                }
                else
                {
                    if (!ConfigureEnterpriseWireless(Modes.WpaEnterprise, WpaAlgorithm.AES, WpaVersion.WPA2, printerAuthMode, serverAuthMode, EncryptionStrengths.High, serverPriority))
                    {
                        return false;
                    }
                }

                if (!EnablePrimaryAddress())
                {
                    return false;
                }

                if (_activityData.ProductFamily == ProductFamilies.VEP)
                {
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info($"Step III: WPA2 Enterprise with AES as encryption, encryption strength: Low and {printerAuthMode} authentication.");
                    return ConfigureEnterpriseWireless(Modes.WpaEnterprise, WpaAlgorithm.AES, WpaVersion.WPA2, printerAuthMode, serverAuthMode, EncryptionStrengths.Low, serverPriority);
                }
            }
            finally
            {
                TestPostRequisites(true);
            }
        }

        /// <summary>
        /// 756558
        /// Verify WPA-PSK authentication does not associate with wrong passphrase
        /// 1.Open EWS Page.
        /// 2.Go to network menu->wireless setup wizard->Enter network name (SSID)
        /// 3.Select the SSID and enter the wrong pass phrase
        /// Pre-request:1.Create an access point  with WPA-PSK and TKIP/AES settings
        /// "1.Device should not associate with access point using WPA.
        /// 2.Wireless diagnostics page should be printed"
        /// </summary>
        /// <returns></returns>
        public bool VerifyWpaAssociationWithWrongPassphrase()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                Profile accessPointProfile = GetAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), WpaVersion.WPA, WpaAlgorithm.AES, WPAKeyType.Passphrase, _activityData.WirelessRadio);

                WirelessSettings settings = new WirelessSettings()
                { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

                WirelessSecuritySettings securitySettings = new WirelessSecuritySettings(new WPAPersonalSettings() { Encryption = WPAEncryptions.AES, passphrase = "123456", Version = WPAVersions.Auto });

                if (_activityData.ProductFamily != ProductFamilies.VEP)
                {
                    if (!CreateAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), accessPointProfile))
                    {
                        return false;
                    }

                    if (!EwsWrapper.Instance().ConfigureWireless(settings, securitySettings, _activityData.PrinterInterfaceType, false))
                    {
                        return false;
                    }

                    INetworkSwitch networkSwitch = SwitchFactory.Create(_activityData.SwitchIpAddress);
                    networkSwitch.DisablePort(_activityData.PrimaryInterfaceAddressPortNumber);

                    return !CoreUtility.Retry.UntilTrue(() => !string.IsNullOrEmpty(_activityData.WirelessInterfaceAddress = CtcUtility.GetPrinterIPAddress(_activityData.WirelessMacAddress)), 5, TimeSpan.FromSeconds(20));
                }
                else
                {
                    if (!EwsWrapper.Instance().ConfigureWireless(settings, securitySettings, _activityData.PrinterInterfaceType, validate: false))
                    {
                        return false;
                    }

                    if (!CreateAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), accessPointProfile))
                    {
                        return false;
                    }
                }

                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromMinutes(2)))
                {
                    TraceFactory.Logger.Info($"Ping is successful with wireless IP address: {_activityData.WirelessInterfaceAddress} in contradiction to expected failure.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info($"Ping failed with wireless IP address: {_activityData.WirelessInterfaceAddress} as expected.");
                    return true;
                }
            }
            finally
            {
                TestPostRequisites();
            }
        }

        #endregion WPA Personal, WEP Personal, WPA Enterprise, WEP Enterprise

        #region WPS Push Pin

        /// <summary>
        /// 756554	Verify device association after 2minutes for WPS-PUSH method.
        /// "1. Open the EWS Page
        /// 2. Select WPS(Wi-Fi Protected Setup) configuration method.
        /// 3.Enable WPS-PUSH option from Web UI.
        /// 4.Connect access point in WPS-PUSH method after 2minutes.
        /// Pre-requist:
        /// 1. Access point has to enabled with WPS-PUSH method."
        /// "1.The Wireless association should fail.
        ///  2.Network connection failed message should be displayed in the front panel."
        /// 756555	Verify device association after 4minutes for WPS-PIN method.
        /// "1. Open the EWS Page
        /// 2. Select WPS (Wi-Fi Protected Setup) configuration method.
        /// 3.Enable WPS-PIN method from Web UI.
        /// 4.Connect access point in WPS-PIN method after 4minutes.
        ///  Pre-requist:
        /// 1. Access point has to be enabled with WPS-PIN method."
        /// "1.The Wireless association should be failed.
        /// 2.Network connection failed message should be displayed in front panel."
        /// </summary>
        /// <param name="wpsMethod"></param>
        /// <returns></returns>
        public bool VerifyWpsPushPinTimeOut(WPSMethods wpsMethod)
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!TestWpsMethods(WPSMethods.WPSPush, $"{_testNumber}-{wpsMethod}", GetAccessPointProfile(_activityData.WirelessRadio)))
                {
                    return false;
                }

                EnablePrimaryAddress();

                return TestWpsPush(_activityData.AccessPointDetails.FirstOrDefault(x => (x.Vendor == AccessPointManufacturer.Cisco && x.Model == WPS_SUPPORTED_MODEL)), null, delay: wpsMethod == WPSMethods.WPSPush ? TimeSpan.FromMinutes(3) : TimeSpan.FromMinutes(4), validateEws: true);
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756552
        /// Verify device association for PIN method with invalid PIN entry in access point
        /// "1. Open device EWS Page.
        /// 2.Select Wireless->Wi-Fi Protected Setup method.
        /// 3.Select Wifi-Protected Setup using PIN method.
        /// 4.Wait till the device generates 8 digit Pin on web UI
        /// 5.Web in to the WPS AP and enter wrong pin
        /// Pre-request:
        /// 1.Access Point has to support WPS-PIN method."
        /// "1.The wireless association should fail.
        /// 2.Print the wireless test page and verify the results."
        /// </summary>
        /// <returns></returns>
        public bool VerifyWpsInvalidPin()
        {
            if (!TestPreRequisites(true))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (!TestWpsMethods(WPSMethods.WPSPin, $"{_testNumber}-WPS-Pin", GetAccessPointProfile(_activityData.WirelessRadio)))
                {
                    return false;
                }

                if (!EnablePrimaryAddress())
                {
                    return false;
                }

                return TestWpsPin(_activityData.AccessPointDetails.FirstOrDefault(x => (x.Vendor == AccessPointManufacturer.Cisco && x.Model == WPS_SUPPORTED_MODEL)), null, validPin: false, enableDebug: _activityData.EnableDebug, validateEws: true);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                TestPostRequisites();
                Thread.Sleep(TimeSpan.FromMinutes(2));
            }
        }

        /// <summary>
        /// 756553
        /// Verify device association for PUSH method with unsupported Access Point.
        /// "1. Open device EWS Page.
        /// 2.Select Wireless->Wi-Fi Protected Setup method.
        /// 3.Select Wifi-Protected Setup using PUSH method.
        /// Pre-requisite: 1.Use unsupported access point with WPS-PUSH method."
        ///	"1.The wireless association should fail.
        /// 2.Print the wireless test page and verify the results."
        /// </summary>
        /// <returns></returns>
        public bool VerifyWpsUnsupportedAp()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info("WPS Push configuration with unsupported access point");

                if (_activityData.ProductFamily == ProductFamilies.VEP)
                {
                    using (IDevice device = DeviceFactory.Create(_activityData.PrimaryInterfaceAddress))
                    {
                        IWireless wireles = WirelessFactory.Create(device);
                        wireles.ConfigureWpsPush();
                        return !NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromMinutes(2));
                    }
                }

                return !EwsWrapper.Instance().PerformWpsPushEnrollment(true);
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                TestPostRequisites();
                Thread.Sleep(TimeSpan.FromMinutes(2));
            }
        }

        /// <summary>
        /// 756551
        /// Verify device association when SSID is changed in Access Point.
        /// "1.Associate the device using WPS-PIN method.
        /// 2. Open the EWS page of WPS-PIN method enabled Access Point.
        /// 3.Change the SSID from Access Point and apply the configuration."
        /// "1. The Associated WPS device should disconnect with WPS AP.
        /// 2.  Device should be in disassociated state"
        /// </summary>
        /// <returns></returns>
        public bool VerifyWpsPinSsidChangeinAccessPoint()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info("Step I: WPS Pin association");

                if (!TestWpsMethods(WPSMethods.WPSPin, $"{_testNumber}-WPSPin", GetAccessPointProfile(_activityData.WirelessRadio)))
                {
                    return false;
                }

                TraceFactory.Logger.Info("Step II: Changing SSID configuration on access point");
                if (!CreateAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), GetAccessPointProfile(_activityData.WirelessRadio)))
                {
                    return false;
                }

                if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromMinutes(1)))
                {
                    TraceFactory.Logger.Info("Printer is disconnected after SSID change in access point.Ping failed with wireless ip address: {0}".FormatWith(_activityData.WirelessInterfaceAddress));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Printer is NOT disconnected after SSID change in access point.Ping is successful with wireless ip address: {0}".FormatWith(_activityData.WirelessInterfaceAddress));
                    return false;
                }
            }
            finally
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);
                TestPostRequisites();
                Thread.Sleep(TimeSpan.FromMinutes(2));
            }
        }

        /// <summary>
        /// 781442	Verify device association from WPA-Personal mode to WPS-PIN method.
        /// "1.Associate the device in WPA-Personal mode.
        /// 2.Open EWS page.
        /// 3.Select Wireless ->WPS Configuration menu.
        /// 4.Select WPS-PIN method.
        /// 5.Configure Access Point using PIN method.
        /// Pre-requisite: 1.Access point has to be supported with WPS-PIN method."
        /// "1.Device should associate with WPS-PIN method.
        /// 2.Print the wireless configuration page and verify the settings."
        /// 781442	Device Association from WPS-PIN to WPA personal
        /// "1.Associate device to AP1 with WPS PIN method.
        ///  2.Open EWS page.
        /// 3.Change the wireless settings to WPA-Personal mode from web UI.
        /// 4.Apply the configuration
        /// Pre-requisite:
        /// 1.AP1 has to be supported WPS-PIN method.
        /// 2.AP2 has to be configured with WPA-Personal mode."
        /// 1.Device has to associate with WPA-Personal mode.
        /// 2.Print the wireless configuration page and verify the settings."
        /// 781443	Verify device association from WPA2-Personal mode to WPS-PIN method.
        /// "1.Associate the device in WPA2-Personal mode.
        /// 2.Open EWS page.
        /// 3.Select Wireless ->WPS Configuration menu.
        /// 4.Select WPS-PIN method.
        /// 5.Configure Access Point using PIN method.
        /// Pre-requisite: 1.Access point has to be supported with WPS-PIN method."
        /// "1.Device should associate with WPS-PIN method.
        /// 2.Print the wireless configuration page and verify the settings."
        /// 781443	Device Association from WPS-Pin to WPA2 mode
        /// "1.Associate device to AP1with WPS PIN method.
        /// 2.Open EWS page.
        /// 3.Change the wireless settings to WPA2-Personal mode from web UI.
        /// 4.Apply the configuration
        /// Pre-requisite: 1.AP1 has to be supported WPS-PIN method.
        /// 2.AP2 has to configured with WPA2-Personal mode."
        /// 1.The WPS PIN method connection should establish between device and Access Point in less than 4 minutes of time.
        /// 781448	Verify device association from WPA-Personal mode to WPS-PUSH method connection.
        /// "1.Associate the device in WPA-Personal mode.
        /// 2.Open EWS page.
        /// 3.Enable WPS from Web UI
        /// 4.Press the WPS push button on the device.
        /// 5.Print the wireless configuration page.
        /// Pre-requisite:
        /// 1.Access point has to be configured with WPS-PUSH method."
        /// "1.Device has to associate with WPS settings.
        /// 2.The wireless configuration pages should print with WPS settings"
        /// 781448	Device association from WPS-PUSH to WPA
        /// "1.Associate the device in WPS-PUSH method.
        /// 2.Open EWS page.
        /// 3.Change the wireless settings to WPA-Personal mode.
        /// 4.Apply the configuration.
        /// Pre-requisite:
        /// 1.AP1 has to be configured with WPS-PUSH method.
        /// 2.AP2 has to be configured with WPA-Personal mode."
        /// "1.Device has to associate with WPA-Personal settings.
        ///  2.The wireless configuration pages should print with WPA-Personal settings"
        /// 781449	Verify device association from WPA2-Personal mode to WPS-PUSH method connection.
        /// "1.Associate the device in WPA2-Personal mode.
        /// 2.Open EWS page.
        /// 3.Enable WPS from Web UI
        /// 4.Press the WPS push button on the device.
        /// 5.Print the wireless configuration page.
        /// Pre-requisite:
        /// 1.Access point has to be configured with WPS-PUSH method."
        /// "1.Device has to associate with WPS settings.
        /// 2.The wireless configuration pages should print with WPS settings"
        /// 781449	Device Association from WPS-Push mode to WPA2
        /// "1.Associate the device in WPS-PUSH method.
        /// 2.Open EWS page.
        /// 3.Change the wireless settings to WPA2-Personal mode. 4.Apply the configuration.
        /// Pre-requisite:
        /// 1.AP1 has to be configured with WPS-PUSH method.
        /// 2.AP2 has to be configured with WPA2-Personal mode."
        /// "1.Device has to associate with WPA2-Personal settings.
        /// 2.The wireless configuration pages should print with WPA2-Personal settings"
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="wpaVersion"></param>
        /// <param name="wpsMethod"></param>
        /// <returns></returns>
        public bool VerifyWpaPersonalWpsSwitching(WirelessAuthentications mode = WirelessAuthentications.Wpa, WpaVersion wpaVersion = WpaVersion.Undefined, WPSMethods wpsMethod = WPSMethods.WPSPush)
        {
            if (!TestPreRequisites(true))
            {
                return false;
            }

            if (_activityData.AccessPointDetails.Count != 2)
            {
                TraceFactory.Logger.Info("Two access point are required for the execution.");
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info("Step I: Device association from {0} to {1}".FormatWith(mode == WirelessAuthentications.Wpa ? $"{wpaVersion}-Personal" : WirelessAuthentications.Wep.ToString(), wpsMethod));

                AccessPointInfo accessPointInfo = _activityData.AccessPointDetails.FirstOrDefault(x => (x.Vendor == AccessPointManufacturer.Cisco && x.Model == WPS_SUPPORTED_MODEL));
                WirelessSettings settings = new WirelessSettings();
                WirelessSecuritySettings securitySettings = new WirelessSecuritySettings();

                Profile accessPointProfile;

                if (mode == WirelessAuthentications.Wpa)
                {
                    accessPointProfile = GetAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), wpaVersion, WpaAlgorithm.AES, WPAKeyType.Passphrase, _activityData.WirelessRadio);

                    settings = new WirelessSettings()
                    { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

                    securitySettings = new WirelessSecuritySettings(new WPAPersonalSettings() { Encryption = WPAEncryptions.AES, passphrase = WPA_PASSPHRASE, Version = wpaVersion == WpaVersion.WPA2 ? WPAVersions.WPA2 : WPAVersions.Auto });

                    if (!TestWireless(accessPointProfile, settings, securitySettings, accessPointInfo: accessPointInfo))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!ConfigureWepPersonal(WEPModes.Shared, WEPIndices.Key1, KeySize.Bit_64, AuthenticationType.Shared_Key))
                    {
                        return false;
                    }
                }

                if (mode == WirelessAuthentications.Wpa)
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(_activityData.WirelessInterfaceAddress);

                    if (!TestWpsMethods(wpsMethod, $"{_testNumber}-{wpsMethod}", null, isInWiredIp: false))
                    {
                        return false;
                    }
                }
                else
                {
                    accessPointProfile = GetAccessPointProfile(accessPointInfo, AuthenticationType.Open_System, TransmitKeySelect.KeySelect1, KeySize.Bit_64, _activityData.WirelessRadio);

                    settings = new WirelessSettings()
                    { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

                    securitySettings = new WirelessSecuritySettings(new WEPPersonalSettings()
                    {
                        WEPIndex = WEPIndices.Key1,
                        WEPKey = GetWepKey(accessPointProfile, WEPIndices.Key1),
                        WEPMode = WEPModes.Auto
                    }
                    );

                    EwsWrapper.Instance().ConfigureWireless(settings, securitySettings, validate: _activityData.PrinterInterfaceType != ProductType.MultipleInterface);

                    INetworkSwitch networkSwitch = SwitchFactory.Create(_activityData.SwitchIpAddress);
                    networkSwitch.DisablePort(_activityData.PrimaryInterfaceAddressPortNumber);

                    if (CoreUtility.Retry.UntilTrue(() => !string.IsNullOrEmpty(_activityData.WirelessInterfaceAddress = CtcUtility.GetPrinterIPAddress(_activityData.WirelessMacAddress)), 6, TimeSpan.FromSeconds(20)))
                    {
                        TraceFactory.Logger.Info($"Printer acquired IP address: {_activityData.WirelessInterfaceAddress} with WEP Push configuration.");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info($"Printer didnot acquire IP address with WEP Push configuration as expected.");
                    }
                }

                TraceFactory.Logger.Info("Step II: Device association from {0} to {1} with two access points.".FormatWith(wpsMethod, mode == WirelessAuthentications.Wpa ? "{0}-Personal".FormatWith(wpaVersion) : WirelessAuthentications.Wep.ToString()));

                accessPointInfo = _activityData.AccessPointDetails.FirstOrDefault(x => !Equals(x.Address, accessPointInfo.Address));

                if (mode == WirelessAuthentications.Wpa)
                {
                    accessPointProfile = GetAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), wpaVersion, WpaAlgorithm.AES, WPAKeyType.Passphrase, _activityData.WirelessRadio);

                    return TestWireless(accessPointProfile, settings, securitySettings, accessPointInfo: accessPointInfo, isInWiredIp: false);
                }
                else
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(_activityData.PrimaryInterfaceAddress);

                    return ConfigureWepPersonal(WEPModes.Shared, WEPIndices.Key1, KeySize.Bit_64, AuthenticationType.Shared_Key, accessPointInfo: accessPointInfo);
                }
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 781440
        /// Verify device association for  WPS-PIN method functionality
        /// "1.Open device EWS page.
        /// 2.Select Wireless->Wi-Fi Protected Setup method.
        /// 3.Select Wifi-Protected Setup using PIN method.
        /// 4.Wait till the device generates PIN on web UI
        /// Verify WPS PIN screen appears"
        /// WPS PIN appears on the screen
        /// Step 2	Verify device can generate 4 digit /8 digit(any one method whichever is supprted on device)
        /// Device should generate proper PIN when WPS PIN is started.
        /// Step 3	Verify device PIN is show on EWS page and Control panel when tried WPS pin connection from EWS page and Control Panel respectively.
        /// Device EWS page and Control panel shows PIN genearted.
        /// Step 4	Give the generated PIN on AP page and verify device gets connected successfully.
        /// "Device association should be successfull using WPS PIN method from EWS page and Control Panel
        /// Device association should happen within 4 mins time."
        /// </summary>
        /// <returns></returns>
        public bool VerifyWpsPinLength()
        {
            if (!TestPreRequisites(true))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info("Step I: Verify the device generates pin WPS pin is started.");

                string pin = string.Empty;

                if (_activityData.ProductFamily == ProductFamilies.VEP)
                {
                    using (IDevice device = DeviceFactory.Create(_activityData.PrimaryInterfaceAddress))
                    {
                        IWireless wireles = WirelessFactory.Create(device);
                        pin = wireles.GenerateWpsPin();
                    }
                }
                else
                {
                    EwsWrapper.Instance().GenerateWpsPin();
                }

                if (string.IsNullOrEmpty(pin))
                {
                    TraceFactory.Logger.Info("WPS pin is not generated on web UI.");
                    return false;
                }

                TraceFactory.Logger.Info("Step II: Verify the generated pin is 4 or 6 digit.");

                if (pin.Length == 4 || pin.Length == 8)
                {
                    TraceFactory.Logger.Info("4 or 8 digit pin is generated on the web UI.");
                }
                else
                {
                    TraceFactory.Logger.Info("4 or 8 digit pin is not generated on the web UI.");
                    return false;
                }

                TraceFactory.Logger.Info("Step III: Verify WPS configuration.");

                AccessPointInfo accessPointInfo = _activityData.AccessPointDetails.FirstOrDefault(x => (x.Vendor == AccessPointManufacturer.Cisco && x.Model == WPS_SUPPORTED_MODEL));
                IAccessPoint accessPoint = AccessPointFactory.Create(accessPointInfo.Address, accessPointInfo.Vendor, accessPointInfo.Model);

                try
                {
                    Profile accessPointProfile = GetAccessPointProfile(_activityData.WirelessRadio);
                    accessPoint.Login(CISCO_USERNAME, CISCO_PASSWORD);
                    accessPoint.CreateProfile(accessPointProfile);

                    TraceFactory.Logger.Info($"Starting WPS Pin enrollment on {accessPointInfo.Vendor}-{accessPointInfo.Model} access point with IP address: {accessPointInfo.Address}");
                    accessPoint.StartPinEnrollment(pin, 1, 0);
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Info($"Failed to perform WPS Pin on {accessPointInfo.Vendor}-{accessPointInfo.Model} access point with IP address: {accessPointInfo.Address}");
                    TraceFactory.Logger.Info(ex.Message);
                    return false;
                }
                finally
                {
                    accessPoint.Logout();
                }

                if (_activityData.ProductFamily == ProductFamilies.VEP)
                {
                    using (IDevice device = DeviceFactory.Create(_activityData.PrimaryInterfaceAddress))
                    {
                        IWireless wireles = WirelessFactory.Create(device);
                        wireles.ConfigureWpsPin();
                    }
                }
                else
                {
                    if (!EwsWrapper.Instance().StartWpsPinEnrollment())
                    {
                        return false;
                    }
                }

                CoreUtility.Retry.UntilTrue(() => !string.IsNullOrEmpty(_activityData.WirelessInterfaceAddress = CtcUtility.GetPrinterIPAddress(_activityData.WirelessMacAddress)), 5, TimeSpan.FromSeconds(10));

                if (string.IsNullOrEmpty(_activityData.WirelessInterfaceAddress))
                {
                    TraceFactory.Logger.Info("Printer is not discovered with wireless mac address: {0}".FormatWith(_activityData.WirelessMacAddress));
                    return false;
                }

                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromMinutes(3)))
                {
                    TraceFactory.Logger.Info("Wireless Configuration is successful.Ping with wireless IP address: {0} is successful.".FormatWith(_activityData.WirelessInterfaceAddress));
                }
                else
                {
                    TraceFactory.Logger.Info("Wireless Configuration failed.Ping with wireless IP address: {0} failed.".FormatWith(_activityData.WirelessInterfaceAddress));
                    return false;
                }

                if (_activityData.ProductFamily == ProductFamilies.VEP)
                {
                    return true;
                }

                TraceFactory.Logger.Info("Step IV: Verify WPS pin generated from control panel.");
                ControlPanelUtility utility = new ControlPanelUtility(IPAddress.Parse(_activityData.WirelessInterfaceAddress));
                pin = utility.GetWpsPin();

                if (pin.Length == 4 || pin.Length == 8)
                {
                    TraceFactory.Logger.Info("4 or 8 digit pin is generated on the control panel.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("4 or 8 digit pin is not generated on the control panel.");
                    return false;
                }
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756629
        /// Verify devices association using WPA2 for both push and PIN method
        /// "1. Power on the Printer after performing NVRAM init operation
        /// 2. Configure AP with WPA/WPA2 mixed mode with AES as security method on Access point
        /// 3. Start WPS Push button/PIN method session and sniff the packets to verify the association"
        /// Device association should happen using WPA2 as preffered method
        /// </summary>
        /// <returns></returns>
        public bool VerifyWpa2Push()
        {
            if (!TestPreRequisites(true))
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                Printer printer = PrinterFactory.Create(_activityData.ProductFamily.ToString(), _activityData.PrimaryInterfaceAddress);
                printer.ColdReset();
                Profile accessPointProfile = GetAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), WpaVersion.WPA_WPA2, WpaAlgorithm.AES, WPAKeyType.Passphrase, _activityData.WirelessRadio);
                if (!TestWpsMethods(WPSMethods.WPSPush, $"{_testNumber}-{WPSMethods.WPSPush}", accessPointProfile))
                {
                    return false;
                }

                //TODO: Packet Validation
                return true;
            }
            finally
            {
                TestPostRequisites();
            }
        }

        #endregion WPS Push Pin

        #region Error Handling

        /// <summary>
        /// 756556
        /// Verify Error information for Wrong WEP Key length
        /// "1.Open EWS Page.
        /// 2.Goto network menu->wireless setup wizard->Enter network name(SSID)
        /// 3.Enter SSID->Infrastructure mode->Encryption as WEP enter the key less than 5 alphanumeric or 10 hex characters"
        /// "40/64-bit encryption - Wep Key length must be 5 alphanumeric or 10 hex characters only" and 128-bit encryption - Wep key length must be 10 alphanumeric or 26 hex characters only
        /// 756556	Verify Error information for WPA passphrase length
        /// "1.Open EWS Page.
        /// 2.Goto network menu->wireless setup wizard->Enter network name(SSID)
        /// 3.Enter SSID->Infrastructure mode->Encryption as WPA enter the key less than 8 characters"
        /// WPA passphrase should be between 8 to 63 characters ASCII
        /// 756556	Verify SSID of Unsupported Authentication type is not listed in EWS Page
        /// "1.Open the EWS page.
        /// 2.Setup->network menu->Wireless setup wizard.
        /// 3.Refresh the SSID list"
        /// "SSID of Accesspoint with unsupported authentication is not listed
        /// EG: If devices does not support WPA/WEP enterprise it should not list the SSID"
        /// 756556	Verify SSID of Unsupported Authentication type is not listed in control panel
        /// "1.Launch Wireless setup wizard from control panel
        /// 2.Verify the SSID list"	"SSID of Accesspoint with unsupported authentication is not listed
        /// EG: If devices does not support WPA/WEP enterprise it should not list the SSID"
        /// </summary>
        /// <returns></returns>
        public bool VerifyWrongPasswordLength()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info("Step I: Verify Error information for Wrong WEP Key length");

                WirelessSettings settings = new WirelessSettings()
                { SsidName = "abcd", WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

                WirelessSecuritySettings securitySettings = new WirelessSecuritySettings(new WEPPersonalSettings() { WEPIndex = WEPIndices.Key1, WEPKey = "1234", WEPMode = WEPModes.Open });

                if (_activityData.PrinterInterfaceType != ProductType.SingleInterface)
                {
                    EwsWrapper.Instance().ConfigureWireless(settings, securitySettings, _activityData.PrinterInterfaceType, validate: false, closeBrowser: false);

                    List<string> wepErrors = new List<string>() { "WEP key is not of the correct length", "Characters entered for key or passphrase are invalid", "A valid WEP key is either: 5 or 13 alphanumeric characters or 10 or 26 hexadecimal digits.", "Key 1: WEP key is not of the correct length" };
                    if (wepErrors.Any(x => EwsWrapper.Instance().Adapter.Body.Contains(x)))
                    {
                        TraceFactory.Logger.Info($"Successfully validated the error message: {string.Join("Or ", wepErrors)}");
                    }
                    else
                    {
                        TraceFactory.Logger.Info($"Failed to validate the error message: {string.Join("Or ", wepErrors)}");
                        return false;
                    }
                }
                else
                {
                    TraceFactory.Logger.Info("Step I is not applicable for SI.");
                }

                TraceFactory.Logger.Info("Step II: Verify Error information for WPA passphrase length");

                Profile accessPointProfile = GetAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), WpaVersion.WPA, WpaAlgorithm.AES, WPAKeyType.Passphrase, _activityData.WirelessRadio);
                if (_activityData.ProductFamily != ProductFamilies.VEP)
                {
                    CreateAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), accessPointProfile);
                    settings = new WirelessSettings()
                    { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };
                }
                else
                {
                    settings = new WirelessSettings()
                    { SsidName = SSID0.FormatWith(Guid.NewGuid().ToString().Substring(0, 4)), WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

                }
                securitySettings = new WirelessSecuritySettings(new WPAPersonalSettings() { Encryption = WPAEncryptions.AES, passphrase = "123456", Version = WPAVersions.Auto });

                EwsWrapper.Instance().ConfigureWireless(settings, securitySettings, _activityData.PrinterInterfaceType, validate: false, closeBrowser: false);

                List<string> errors = new List<string>() { "Invalid Pass-phrase", "Cannot connect to your wireless router", "Characters entered for key or passphrase are invalid." };
                if (errors.Any(x => EwsWrapper.Instance().Adapter.Body.Contains(x)))
                {
                    TraceFactory.Logger.Info($"Successfully validated the error message : {string.Join("Or ", errors)}");
                }
                else
                {
                    TraceFactory.Logger.Info($"Failed to validate the error message: {string.Join("Or ", errors)}.");
                    return false;
                }

                if (!(_activityData.ProductFamily == ProductFamilies.VEP && _activityData.PrinterInterfaceType == ProductType.MultipleInterface) && _activityData.AccessPointDetails.FirstOrDefault().Model == "WAP321")
                {
                    TraceFactory.Logger.Info("Step III: Verify Unsupported profile is not listed in access point list.");
                    accessPointProfile = GetEnterpriseProfile(_activityData.AccessPointDetails.FirstOrDefault(), Modes.WepEnterprise);

                    if (!CreateAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), accessPointProfile))
                    {
                        return false;
                    }

                    List<string> ssidList = EwsWrapper.Instance().GetSSIDList();

                    if (ssidList.Contains(accessPointProfile.RadioSettings.SSID))
                    {
                        TraceFactory.Logger.Info("SSID of unsupported authentication is present in the SSID list on web UI.");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("SSID of unsupported authentication is not present in the SSID list on web UI");
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            finally
            { }
        }

        /// <summary>
        /// 756650	Host name change when enterprise wireless is not configured
        /// "1. Do not configure enterprise wireless mode on the printer.
        /// 2. Change the hostname of the printer
        /// 3. Verify Username under Networking->wireless station
        /// 4. Reboot the printer"
        /// "1. The Username should reflect the new host name of the printer
        /// 2. After reboot, the changed host name should be retained as username in wireless station page"
        /// 756650	Hostname change when enterprise wireless is configured on the printer
        /// "1. Configure enterprise wireless mode on the printer.
        /// 2. Connect the printer to an AP configured in WPA enterprise mode and verify EAP-TLS and PEAP works fine.
        /// 3. Change the host name of the printer and verify the username in wireless station page."
        /// 1. When wireless enterprise is configured on the printer, if the hostname is changed, the username in wireless station page should not be changed.
        /// It should retain its previous value. wirless enterprise reauthentication should not happen. Wireless enterprise authentication should work fine
        /// 756650	Username field in wireless station page on restore defaults
        /// "1. Perform restore defaults from Networking-->wireless station page or settings->restore defaults page
        /// 2. Printer will go to ad hoc mode.
        /// 3. Access the EWS page and check the user name field under wireless station page"
        /// 1. After reset username in wireless station page should be cleared.
        /// </summary>
        /// <returns></returns>
        public bool HostnameWithEnterpriseWireless()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                Printer printer = PrinterFactory.Create(_activityData.ProductFamily.ToString(), _activityData.PrimaryInterfaceAddress);
                // Previous configured enterprise user name will be retained in printer. Doing cold reset to clear it.
                if (_activityData.ProductFamily == ProductFamilies.InkJet)
                {
                    EwsWrapper.Instance().SetFactoryDefaults();
                }
                {
                    EwsWrapper.Instance().RestoreSecurityDefaults();
                }

                TraceFactory.Logger.Info("Step I: Host name change while enterprise wireless is not configured");

                string hostName = CtcUtility.GetUniqueHostName();
                EwsWrapper.Instance().SetHostname(hostName);

                if (hostName.EqualsIgnoreCase(EwsWrapper.Instance().GetEnterpriseWirelessUserName()))
                {
                    TraceFactory.Logger.Info("Host name change is reflected in enterprise wireless page");
                }
                else
                {
                    TraceFactory.Logger.Info("Host name change is not reflected in enterprise wireless page");
                    return false;
                }

                TraceFactory.Logger.Info("Step II: Verify Host name change after power cycle while enterprise wireless is not configured");
                printer = PrinterFactory.Create(_activityData.ProductFamily.ToString(), _activityData.PrimaryInterfaceAddress);
                printer.PowerCycle();

                if (hostName.EqualsIgnoreCase(EwsWrapper.Instance().GetEnterpriseWirelessUserName()))
                {
                    TraceFactory.Logger.Info("Host name change is reflected in enterprise wireless page");
                }
                else
                {
                    TraceFactory.Logger.Info("Host name change is not reflected in enterprise wireless page");
                    return false;
                }

                TraceFactory.Logger.Info("Step III: Host name change while enterprise wireless is configured");

                if (!ConfigureEnterpriseWireless(Modes.WpaEnterprise, WpaAlgorithm.TKIP, WpaVersion.WPA, AuthenticationMode.PEAP, AuthenticationMode.PEAP, EncryptionStrengths.Medium))
                {
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(_activityData.WirelessInterfaceAddress);
                hostName = CtcUtility.GetUniqueHostName();

                if (!EwsWrapper.Instance().SetHostname(hostName))
                {
                    return false;
                }

                if (hostName.EqualsIgnoreCase(EwsWrapper.Instance().GetEnterpriseWirelessUserName()))
                {
                    TraceFactory.Logger.Info("Host name change is reflected in enterprise wireless page");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info("Host name change is not reflected in enterprise wireless page");
                }

                TraceFactory.Logger.Info("Step IV: Host name after restore defaults");

                if (!EnablePrimaryAddress())
                {
                    return false;
                }

                EwsWrapper.Instance().ChangeDeviceAddress(_activityData.PrimaryInterfaceAddress);
                if (!EwsWrapper.Instance().RestoreSecurityDefaults())
                {
                    return false;
                }

                string enterpriseHostName = EwsWrapper.Instance().GetEnterpriseWirelessUserName();

                if (string.IsNullOrEmpty(enterpriseHostName) || !hostName.EqualsIgnoreCase(enterpriseHostName))
                {
                    TraceFactory.Logger.Info("Host name is cleared on enterprise wireless page after restore defaults.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Host name is NOT cleared on enterprise wireless page after restore defaults.");
                    return false;
                }
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756559
        /// Verify device stays configured when the transmit key is changed
        /// 1.Open EWS Page.
        /// 2.Configure device with WEP mode and select valid transmit key.
        /// 3.Change the transmit key on device to the one which other than Access Point.
        /// 1.Connection of the device with access point is lost.
        /// 2.The parameters Network Name (SSID), Communication mode, Authentication type
        /// Verify that the Printer can not be configured with less than 10 alphanumeric or 26 hex characters in infrastructure mode
        /// 1.Open EWS Page.
        /// 2.Goto network menu->wireless setup wizard->Enter network name(SSID)
        /// 3.Enter SSID->Infrastructure mode->Encryption as WEP enter the key less than 5 alphanumeric or 10 hex characters
        /// Device display's error message stating for "128-bit encryption - must be 10 alphanumeric or 26 hex characters only".
        /// Verify Error information for Wrong WEP Key length
        /// 1.Open EWS Page.
        /// 2.Goto network menu->wireless setup wizard->Enter network name(SSID)
        /// 3.Enter SSID->Infrastructure mode->Encryption as WEP enter the key less than 5 alphanumeric or 10 hex characters
        /// "40/64-bit encryption - Wep Key length must be 5 alphanumeric or 10 hex characters only" and 128-bit encryption
        /// Wep key length must be 10 alphanumeric or 26 hex characters only
        /// Verify device can not be configured with less than 5 alphanumeric or 10 hex characters for WEP mode in ad-hoce mode
        /// 1.Open EWS Page.
        /// 2.Goto network menu->wireless setup wizard->Enter network name(SSID)
        /// 3.Enter SSID->Infrastructure mode->Encryption as WEP enter the key less than 5 alphanumeric or 10 hex characters
        /// Device display's error message stating for "40/64-bit encryption - must be 5 alphanumeric or 10 hex characters only".
        /// Verify that the Printer can not be configured with less than 10 alphanumeric or 26 hex characters in ad-hoc mode
        /// 1.Open EWS Page.
        /// 2.Goto network menu->wireless setup wizard->Enter network name(SSID)
        /// 3.Enter SSID->Infrastructure mode->Encryption as WEP enter the key less than 5 alphanumeric or 10 hex characters
        /// Device display's error message stating for "128-bit encryption - must be 10 alphanumeric or 26 hex characters only".
        /// </summary>
        /// <returns></returns>
        public bool ErrorCheck()
        {
            if (_activityData.PrinterInterfaceType == ProductType.SingleInterface)
            {
                TraceFactory.Logger.Info("This test is not applicable for SI products");
                return false;
            }

            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER}Step I: Verify WEP association with transmit key1.");

                Profile accessPointProfile = GetAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), AuthenticationType.Open_System, TransmitKeySelect.KeySelect1, KeySize.Bit_64, _activityData.WirelessRadio);

                WirelessSettings settings = new WirelessSettings()
                { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

                WirelessSecuritySettings securitySettings = new WirelessSecuritySettings(new WEPPersonalSettings() { WEPIndex = WEPIndices.Key1, WEPKey = GetWepKey(accessPointProfile, WEPIndices.Key1), WEPMode = WEPModes.Open });

                if (!TestWireless(accessPointProfile, settings, securitySettings))
                {
                    return false;
                }

                if (!EnablePrimaryAddress())
                {
                    return false;
                }

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Step II: Verify WEP with transmit key1 on AP and 2 on printer.");

                securitySettings.WEPPersonalSecurity.WEPIndex = WEPIndices.Key2;
                securitySettings.WEPPersonalSecurity.WEPKey = GetWepKey(accessPointProfile, WEPIndices.Key2);
                if (!EwsWrapper.Instance().ConfigureWireless(settings, securitySettings, _activityData.PrinterInterfaceType, validate: _activityData.PrinterInterfaceType != ProductType.MultipleInterface))
                {
                    return false;
                }

                if (_activityData.ProductFamily != ProductFamilies.VEP)
                {
                    INetworkSwitch networkSwitch = SwitchFactory.Create(_activityData.SwitchIpAddress);
                    networkSwitch.DisablePort(_activityData.PrimaryInterfaceAddressPortNumber);

                    if (CoreUtility.Retry.UntilTrue(() => !string.IsNullOrEmpty(_activityData.WirelessInterfaceAddress = CtcUtility.GetPrinterIPAddress(_activityData.WirelessMacAddress)), 5, TimeSpan.FromSeconds(20)))
                    {
                        TraceFactory.Logger.Info("Wireless ip address is discovred. Expected: Wireless ip address is not available.");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Wireless ip address is not discovred. Expected: Wireless ip address is not available.");
                    }
                }
                else
                {
                    if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromMinutes(1)))
                    {
                        TraceFactory.Logger.Info("Ping with wireless address is successful. Expected: Ping failure.");
                        return false;
                    }
                    else
                    {
                        TraceFactory.Logger.Info("Ping with wireless address failed. Expected: Ping failure.");
                    }
                }


                if (_activityData.ProductFamily == ProductFamilies.VEP && _activityData.PrinterInterfaceType == ProductType.MultipleInterface)
                {
                    accessPointProfile.SecuritySettings.TransmitKeySelect = TransmitKeySelect.KeySelect2;
                    if (!CreateAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), accessPointProfile))
                    {
                        return false;
                    }

                    if (!NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromMinutes(2)))
                    {
                        TraceFactory.Logger.Info("Ping with wireless address failed.");
                        return false;
                    }
                }

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Step III: 64-bit WEP key length validation in infrastructure mode.");

                securitySettings.WEPPersonalSecurity.WEPKey = accessPointProfile.SecuritySettings.WepKeys.Key1.Remove(0, 2);
                EwsWrapper.Instance().ConfigureWireless(settings, securitySettings, _activityData.PrinterInterfaceType, validate: false, closeBrowser: false);

                List<string> errors = new List<string>() { "WEP key is not of the correct length", "Characters entered for key or passphrase are invalid", "A valid WEP key is either: 5 or 13 alphanumeric characters or 10 or 26 hexadecimal digits." };
                if (!errors.Any(x => EwsWrapper.Instance().Adapter.Body.Contains(x)))
                {
                    TraceFactory.Logger.Info($"Failed to validate error message: {string.Join("or", errors)} from web UI.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info($"Successfully validated error message: {string.Join("or", errors)} from web UI.");
                }

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Step IV: 128-bit WEP key length validation in infrastructure mode.");

                accessPointProfile.SecuritySettings.KeySize = KeySize.Bit_128;
                List<string> wepKeys = GetRandomHexString(KeySize.Bit_128);
                accessPointProfile.SecuritySettings.WepKeys.Key1 = wepKeys[0];
                accessPointProfile.SecuritySettings.WepKeys.Key2 = wepKeys[1];
                accessPointProfile.SecuritySettings.WepKeys.Key3 = wepKeys[2];
                accessPointProfile.SecuritySettings.WepKeys.Key4 = wepKeys[3];

                securitySettings.WEPPersonalSecurity.WEPKey = accessPointProfile.SecuritySettings.WepKeys.Key1.Remove(0, 2);
                EwsWrapper.Instance().ConfigureWireless(settings, securitySettings, _activityData.PrinterInterfaceType, validate: false, closeBrowser: false);

                if (!errors.Any(x => EwsWrapper.Instance().Adapter.Body.Contains(x)))
                {
                    TraceFactory.Logger.Info($"Failed to validate error message: {string.Join("or", errors)} from web UI.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info($"Successfully validated error message: {string.Join("or", errors)} from web UI.");
                }

                if (!(_activityData.ProductFamily == ProductFamilies.VEP && _activityData.PrinterInterfaceType == ProductType.MultipleInterface))
                {
                    return true;
                }
                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Step V: 64-bit WEP key length validation in adhoc mode.");
                settings.Mode = WirelessStaModes.Adhoc;
                EwsWrapper.Instance().ConfigureWireless(settings, securitySettings, _activityData.PrinterInterfaceType, validate: false, closeBrowser: false);

                if (!errors.Any(x => EwsWrapper.Instance().Adapter.Body.Contains(x)))
                {
                    TraceFactory.Logger.Info($"Failed to validate error message: {string.Join("or", errors)} from web UI.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info($"Successfully validated error message: {string.Join("or", errors)} from web UI.");
                }

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Step VI: 128-bit WEP key length validation in adhoc mode.");

                EwsWrapper.Instance().ConfigureWireless(settings, securitySettings, _activityData.PrinterInterfaceType, validate: false, closeBrowser: false);

                if (!errors.Any(x => EwsWrapper.Instance().Adapter.Body.Contains(x)))
                {
                    TraceFactory.Logger.Info($"Failed to validate error message: {string.Join("or", errors)} from web UI.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info($"Successfully validated error message: {string.Join("or", errors)} from web UI.");
                    return true;
                }
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756576
        /// verify device wireless radio can be disabled or enabled from UI
        /// "1.Open EWS Page.
        /// 2.Go to network settings->wireless tab->Advanced->Disable radio"
        /// Device Wireless radio to be turned off
        /// verify device wireless radio can be disabled or enabled from SNMP
        /// "1. Open the MIB Browser
        /// 2. Disable wireless radio"
        /// Device Wireless radio to be turned off
        /// verify device wireless radio can be disabled or enabled from Telnet
        /// 1. Open Telnet session.
        /// 2. Disable wireless radio"
        /// Device Wireless radio to be turned off
        /// verify device wireless radio can be disabled or enabled from WJA
        /// 1. Open WJA session.
        /// 2. Disable wireless radio
        /// Device Wireless radio to be turned off
        /// </summary>
        /// <returns></returns>
        public bool CheckRadioStatus()
        {
            if (_activityData.PrinterInterfaceType == ProductType.MultipleInterface)
            {
                TraceFactory.Logger.Info("This test is not applicable for MI products");
                return false;
            }

            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER}Step I: Enable wireless and validate through web UI");

                EwsWrapper.Instance().SetWireless(true, _activityData.PrinterInterfaceType);

                if (!EwsWrapper.Instance().GetWireless(_activityData.PrinterInterfaceType))
                {
                    return false;
                }

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER}Step II: Enable wireless and validate through SNMP");

                if (!SnmpWrapper.Instance().SetWirelessStatus(true))
                {
                    return false;
                }

                return SnmpWrapper.Instance().GetWirelessStatus();
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// Verify the wireless network configurations are set to factory default after Restore Defaults from WEB UI
        /// "1.Start the wireless wizard (Printer scans for the available wireless networks)
        /// 2.Select a valid SSID & associate with it.
        /// 3.Do a Restore default and factory default settings & power it on after few seconds.
        /// 4.Repeat the same with various time intervals ranging from 0-120 seconds."	Connection of the device with access point is lost
        /// Verify the wireless network configurations are set to factory default after Restore Defaults from Front Panel
        /// "1.Start the wireless wizard (Printer scans for the available wireless networks)
        /// 2.Select a valid SSID & associate with it.
        /// 3.Do a Restore default and factory default settings & power it on after few seconds.
        /// 4.Repeat the same with various time intervals ranging from 0-120 seconds."
        /// Connection of the device with access point is lost
        /// Verify the wireless network configurations are set to factory default after Restore Defaults from SNMP
        /// "1.Start the wireless wizard (Printer scans for the available wireless networks)
        /// 2.Select a valid SSID & associate with it.
        /// 3.Do a Restore default and factory default settings & power it on after few seconds.
        /// 4.Repeat the same with various time intervals ranging from 0-120 seconds."
        /// Connection of the device with access point is lost
        /// </summary>       
        /// <returns></returns>
        public bool RestoreFactory()
        {
            if (_activityData.PrinterInterfaceType == ProductType.MultipleInterface)
            {
                TraceFactory.Logger.Info("This test is not applicable for MI products");
                return false;
            }

            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info("Restore defaults through Web UI.");

                return RestoreFactory(PrinterAccessType.EWS);
            }
            finally
            {
                TestPostRequisites();
            }
        }

        private bool RestoreFactory(PrinterAccessType accessType)
        {
            Profile accessPointProfile = GetAccessPointProfile(_activityData.WirelessRadio);

            WirelessSettings settings = new WirelessSettings()
            { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.FiveGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

            WirelessSecuritySettings securitySettings = new WirelessSecuritySettings();

            if (!TestWireless(accessPointProfile, settings, securitySettings))
            {
                return false;
            }

            if (accessType == PrinterAccessType.EWS)
            {
                TraceFactory.Logger.Info("Restore defaults from Web UI.");

                if (!EwsWrapper.Instance().RestoreSecurityDefaults())
                {
                    return false;
                }
            }

            if (accessType == PrinterAccessType.ControlPanel)
            {
                TraceFactory.Logger.Info("Restore defaults from Web UI.");
                //TODO
                TraceFactory.Logger.Info("Restore Factory from Web UI.");
            }

            if (accessType == PrinterAccessType.SNMP)
            {
                TraceFactory.Logger.Info("Restore defaults from Web UI.");
                //TODO
                TraceFactory.Logger.Info("Restore Factory from Web UI.");
            }

            if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromMinutes(2)))
            {
                TraceFactory.Logger.Info($"Ping with wireless IP address: {_activityData.WirelessInterfaceAddress} is successful.");
                return false;
            }
            else
            {
                TraceFactory.Logger.Info($"Ping with wireless IP address: {_activityData.WirelessInterfaceAddress} failed.");
                return true;
            }
        }

        /// <summary>
        /// 756561
        /// EAP TLS associaiton	"
        /// Configure a working EAP  authentication.
        ///  Change the user name in the RADIUS server to something else.
        /// Reboot and print a configuration page."
        /// Authentication should fail and the configuration page should show not authenticated.
        /// The RADIUS server log should show a failed authentication request.
        /// EAP PEAP associaiton
        /// Configure a working EAP  authentication.• Change the user name in the RADIUS server to something else.• Reboot and print a configuration page.
        /// "• Authentication should fail and the configuration page should show not authenticated.
        /// The RADIUS server log should show a failed authentication request."
        /// EAP LEAP associaiton    "• Configure a working EAP  authentication.
        /// • Change the user name in the RADIUS server to something else.
        /// • Reboot and print a configuration page."
        /// "• Authentication should fail and the configuration page should show not authenticated.
        /// • The RADIUS server log should show a failed authentication request."
        /// </summary>
        /// <returns></returns>
        public bool EnterpriseWirelessUserNameChange()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER}Step I: PEAP authentication after user name change in radius server.");

                if (!WirelessUesrNameChange(AuthenticationMode.PEAP))
                {
                    return false;
                }

                if (!EnablePrimaryAddress())
                {
                    return false;
                }

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER}Step II: EAPTLS authentication after user name change in radius server.");
                return WirelessUesrNameChange(AuthenticationMode.EAPTLS);
            }
            finally
            {
                TestPostRequisites(true);
            }
        }

        private bool WirelessUesrNameChange(AuthenticationMode authMode)
        {
            string currentsamAccountName;
            string newSamAccountName = SSID0.FormatWith(Guid.NewGuid().ToString().Substring(0, 6));

            using (RadiusApplicationServiceClient client = RadiusApplicationServiceClient.Create(_activityData.RadiusServerIp))
            {
                currentsamAccountName = client.Channel.GetADUserSamAccountName(DOT1X_USERNAME.FormatWith(_activityData.RadiusServerType.ToString().ToLower(CultureInfo.CurrentCulture)));
            }

            try
            {
                if (!ConfigureEnterpriseWireless(Modes.WpaEnterprise, WpaAlgorithm.TKIP, WpaVersion.WPA, authMode, authMode, EncryptionStrengths.Medium))
                {
                    return false;
                }

                using (RadiusApplicationServiceClient client = RadiusApplicationServiceClient.Create(_activityData.RadiusServerIp))
                {
                    if (client.Channel.RenameADUser(currentsamAccountName, newSamAccountName))
                    {
                        TraceFactory.Logger.Info($"Successfully renamed the user name: {currentsamAccountName} to {newSamAccountName}.");
                        client.Channel.RestartRadiusServices();
                    }
                    else
                    {
                        TraceFactory.Logger.Info($"Failed to rename the user name: {currentsamAccountName} to {newSamAccountName}.");
                        return false;
                    }
                }

                Printer printer = PrinterFactory.Create(_activityData.ProductFamily.ToString(), _activityData.WirelessInterfaceAddress);
                printer.PowerCycle();

                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromSeconds(30)))
                {
                    TraceFactory.Logger.Info($"Ping with wireless interface address : {_activityData.WirelessInterfaceAddress} is successful. Expected: ping failure.");
                    return false;
                }
                else
                {
                    TraceFactory.Logger.Info($"Ping with wireless interface address : {_activityData.WirelessInterfaceAddress} failed. Expected: ping failure.");
                    return true;
                }


                //TODO: Print wireless configuration page n check radius server log.
            }
            finally
            {
                using (RadiusApplicationServiceClient client = RadiusApplicationServiceClient.Create(_activityData.RadiusServerIp))
                {
                    if (client.Channel.RenameADUser(newSamAccountName, currentsamAccountName))
                    {
                        TraceFactory.Logger.Info($"Successfully renamed the user name: {newSamAccountName} to {currentsamAccountName}.");

                        client.Channel.RestartRadiusServices();
                        if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromSeconds(30)))
                        {
                            TraceFactory.Logger.Info($"Ping with wireless interface address : {_activityData.WirelessInterfaceAddress} is successful.");
                        }
                        else
                        {
                            TraceFactory.Logger.Info($"Ping with wireless interface address : {_activityData.WirelessInterfaceAddress} failed.");
                        }
                    }
                    else
                    {
                        TraceFactory.Logger.Info($"Failed to rename the user name: {newSamAccountName} to {currentsamAccountName}.");
                    }
                }

                if (_activityData.ProductFamily == ProductFamilies.VEP && _activityData.PrinterInterfaceType == ProductType.MultipleInterface)
                {
                    if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromMinutes(3)))
                    {
                        TraceFactory.Logger.Info($"Ping with wireless interface address : {_activityData.WirelessInterfaceAddress} is successful.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info($"Ping with wireless interface address : {_activityData.WirelessInterfaceAddress} failed.");
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Interoperability

        /// <summary>
        /// 756648
        /// Association using  Channel 1 to 14
        /// Set Access Point to channel 1.
        /// Set communication mode to infrastructure.
        /// Associate the device with the accesspoint
        /// Send print job.
        /// Note: Repeat the steps form channel 1-14"
        /// Print jobs should complete successfully using channel 1 in both infrastructure mode
        /// </summary>
        /// <param name="_activityData"></param>
        /// <returns></returns>
        public bool CheckLowerChannels()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Step 1: Wireless asscociation with channel 1.");

                Profile accessPointProfile = GetAccessPointProfile(Radio._2dot4Ghz, WirelessChannel._1);

                WirelessSettings settings = new WirelessSettings()
                { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

                WirelessSecuritySettings securitySettings = new WirelessSecuritySettings();

                if (!TestWireless(accessPointProfile, settings, securitySettings))
                {
                    return false;
                }

                for (int i = 2; i <= 13; i++)
                {
                    if (_activityData.ProductFamily == ProductFamilies.TPS && i >= 12)
                    {
                        break;
                    }

                    TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Step {i}: Wireless asscociation with channel {i}.");
                    accessPointProfile.RadioSettings.WirelessChannel = (WirelessChannel)i;

                    if (!CreateAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), accessPointProfile))
                    {
                        return false;
                    }

                    if (_activityData.ProductFamily == ProductFamilies.TPS || _activityData.ProductFamily == ProductFamilies.InkJet)
                    {
                        _activityData.WirelessInterfaceAddress = CtcUtility.GetPrinterIPAddress(_activityData.WirelessMacAddress);

                        if (string.IsNullOrEmpty(_activityData.WirelessInterfaceAddress))
                        {
                            return false;
                        }
                    }

                    if (!CheckPingStatus(IPAddress.Parse(_activityData.WirelessInterfaceAddress)))
                    {
                        return false;
                    }

                    if (!Print())
                    {
                        return false;
                    }
                }

                return true;
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756680
        /// Association using channels 36,40,44,48,149,153,157 and 161
        /// Set Access Point to channel 36.
        /// Set communication mode to infrastructure.
        /// Associate the device with the accesspoint
        /// Send print job.
        /// Note: Repeat the steps for all the supported channels.
        /// Print jobs should complete successfully using channel 36.
        /// </summary>
        /// <param name="_activityData"></param>
        /// <param name="_testNo"></param>
        /// <returns></returns>
        public bool CheckHigherChannels()
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Wireless asscociation with channel 36.");

                Profile accessPointProfile = GetAccessPointProfile(Radio._5Ghz, WirelessChannel._36);

                WirelessSettings settings = new WirelessSettings()
                { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.FiveGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

                WirelessSecuritySettings securitySettings = new WirelessSecuritySettings();

                if (!TestWireless(accessPointProfile, settings, securitySettings))
                {
                    return false;
                }

                for (int i = 40; i <= 48; i = i + 4)
                {
                    TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Wireless asscociation with channel {i}.");
                    accessPointProfile.RadioSettings.WirelessChannel = (WirelessChannel)i;

                    if (!CreateAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), accessPointProfile))
                    {
                        return false;
                    }

                    if (_activityData.ProductFamily == ProductFamilies.TPS || _activityData.ProductFamily == ProductFamilies.InkJet)
                    {
                        _activityData.WirelessInterfaceAddress = CtcUtility.GetPrinterIPAddress(_activityData.WirelessMacAddress);

                        if (string.IsNullOrEmpty(_activityData.WirelessInterfaceAddress))
                        {
                            return false;
                        }
                    }

                    if (!CheckPingStatus(IPAddress.Parse(_activityData.WirelessInterfaceAddress)))
                    {
                        return false;
                    }

                    if (!Print())
                    {
                        return false;
                    }
                }

                return true;
            }
            finally
            {
                TestPostRequisites();
            }
        }

        /// <summary>
        /// 756652	Changing device settings
        /// • Configure access points With WPA PSK mixed mode
        /// • Associate with  AP
        /// • Change the WPA/WPA and AES/TKIP combination from UI on the access point"
        /// • Authentication should be successful after every change
        /// 756652	Idle Time
        /// 1. Associate the device with AP and leave it idle
        /// "device should remain connected to AP
        /// Eg : For the test purpose let the device remain idle for minimum of 48 hours"
        /// 756651	Idle Time
        /// 1. Do not Associate the device with AP and leave it idle
        /// "Device should try to find the AP to conenct . Device should not hang/crash during this time
        /// Eg : For the test purpose let the device remain idle for minimum of 48 hours"
        /// 756651	Changing device settings
        /// • Configure access points With WPA PSK mixed mode
        /// • Associate with  AP
        /// • Change the WPA/WPA and AES/TKIP combination from UI on the device	
        /// • Authentication should be successful after every change
        /// </summary>
        /// <param name="_activityData"></param>
        /// <param name="_testNo"></param>
        /// <param name="keepIdleFirst"></param>
        /// <returns></returns>
        public bool IdleTime(bool keepIdleFirst)
        {
            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (keepIdleFirst)
                {
                    TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Step I: Printer is kept idle for 48 hours");
                    EwsWrapper.Instance().StopAdapter();
                    Thread.Sleep(TimeSpan.FromHours(48));
                }

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Step {(keepIdleFirst ? "II" : "I")}: WPA Personal mixed mode with PSK.");
                EwsWrapper.Instance().Start();

                Profile accessPointProfile = GetAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), WpaVersion.WPA_WPA2, WpaAlgorithm.TKIP_AES, WPAKeyType.Passphrase, _activityData.WirelessRadio);

                WirelessSettings settings = new WirelessSettings()
                { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = (_activityData.WirelessRadio == Radio._2dot4Ghz ? WirelessBands.TwoDotFourGHz : WirelessBands.FiveGHz), WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

                WirelessSecuritySettings securitySettings = new WirelessSecuritySettings(new WPAPersonalSettings() { Encryption = WPAEncryptions.AUTO, passphrase = WPA_PRESHAREDKEY, Version = WPAVersions.Auto });

                if (!TestWireless(accessPointProfile, settings, securitySettings))
                {
                    return false;
                }

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Step {(keepIdleFirst ? "III" : "II")}: WPA Personal with WPA, AES with PSK.");
                accessPointProfile.SecuritySettings.WPAVersion = WpaVersion.WPA;
                accessPointProfile.SecuritySettings.WPAAlgorithm = WpaAlgorithm.AES;

                if (!CreateAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), accessPointProfile))
                {
                    return false;
                }

                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromSeconds(30)))
                {
                    TraceFactory.Logger.Info($"Ping with wireless ip address: {_activityData.WirelessInterfaceAddress} is successful.");
                }
                else
                {
                    TraceFactory.Logger.Info($"Ping with wireless ip address: {_activityData.WirelessInterfaceAddress} failed.");
                    return false;
                }

                Printer printer = PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), _activityData.ProductFamily.ToString()), IPAddress.Parse(_activityData.WirelessInterfaceAddress));

                if (printer.IsEwsAccessible())
                {
                    TraceFactory.Logger.Info($"EWS is accessible with wireless ip address: {_activityData.WirelessInterfaceAddress}.");
                }
                else
                {
                    TraceFactory.Logger.Info($"EWS is not accessible with wireless ip address: {_activityData.WirelessInterfaceAddress}.");
                    return false;
                }

                if (!Print())
                {
                    return false;
                }

                if (!keepIdleFirst)
                {
                    TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Step III: Printer is kept idle for 48 hours");
                    EwsWrapper.Instance().StopAdapter();
                    Thread.Sleep(TimeSpan.FromHours(48));

                    EwsWrapper.Instance().Start();

                    if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromSeconds(30)))
                    {
                        TraceFactory.Logger.Info($"Ping with wireless ip address: {_activityData.WirelessInterfaceAddress} is successful.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info($"Ping with wireless ip address: {_activityData.WirelessInterfaceAddress} failed.");
                        return false;
                    }

                    printer = PrinterFactory.Create((PrinterFamilies)Enum.Parse(typeof(PrinterFamilies), _activityData.ProductFamily.ToString()), IPAddress.Parse(_activityData.WirelessInterfaceAddress));

                    if (printer.IsEwsAccessible())
                    {
                        TraceFactory.Logger.Info($"EWS is accessible with wireless ip address: {_activityData.WirelessInterfaceAddress}.");
                    }
                    else
                    {
                        TraceFactory.Logger.Info($"EWS is not accessible with wireless ip address: {_activityData.WirelessInterfaceAddress}.");
                        return false;
                    }

                    return Print();
                }

                return true;
            }
            finally
            {
                TestPostRequisites();
                EwsWrapper.Instance().Start();
            }
        }

        /// <summary>
        /// 756667	Wireless
        /// "1. Configure device with SSID and security method 
        /// 2. Apply the wireless settings on the device
        /// Note : repeat the steps for Nosecurity,WEP,WPA-personal and WPA-Enterprise on AP and Device"
        /// Updated Steps:
        /// 1. Configure and connect a printer to one access point.
        /// 2. Create the profile with same security settings on a different access point.
        /// 3. turn off/ bring down the first access point.
        /// 4. Connection is established with the other access point.
        /// 5. Validate channel number.
        /// </summary>
        /// <param name="_activityData"></param>
        /// <param name="_testNo"></param>
        /// <returns></returns>
        public bool RoamingSameSsid(bool sameSubnet)
        {
            if (_activityData.AccessPointDetails.Count < 2)
            {
                TraceFactory.Logger.Info("This test requires multiple access points.");
                return false;
            }

            if (!TestPreRequisites())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Roaming with no security.");
                Profile profile = GetAccessPointProfile(_activityData.WirelessRadio, WirelessChannel._1);

                WirelessSettings settings = new WirelessSettings()
                { SsidName = profile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

                WirelessSecuritySettings securitySettings = new WirelessSecuritySettings();

                if (!RoamingSamessid(profile, settings, securitySettings, sameSubnet))
                {
                    return false;
                }

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Roaming with WPA Personal.");
                profile = GetAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), WpaVersion.WPA, WpaAlgorithm.AES, WPAKeyType.Passphrase, _activityData.WirelessRadio, WirelessChannel._1);
                securitySettings = new WirelessSecuritySettings(new WPAPersonalSettings() { Encryption = WPAEncryptions.AES, passphrase = WPA_PASSPHRASE, Version = WPAVersions.Auto });

                if (!RoamingSamessid(profile, settings, securitySettings, sameSubnet))
                {
                    return false;
                }

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Roaming with WPA Enterprise.");
                profile = GetEnterpriseProfile(_activityData.AccessPointDetails.FirstOrDefault(), Modes.WpaEnterprise, WpaVersion.WPA, WpaAlgorithm.AES, WirelessChannel._1);

            securitySettings = new WirelessSecuritySettings(WirelessAuthentications.Wpa, new EnterpriseSecuritySettings
            {
                Encryption = WPAEncryptions.AES,
                Version = WPAVersions.Auto,
                CACertificatePath = Path.Combine(CtcSettings.ConnectivityShare, CA_CERTIFICATEPATH.FormatWith(_activityData.ProductFamily, _activityData.RadiusServerType)),
                IdCertificatePath = Path.Combine(CtcSettings.ConnectivityShare, ID_CERTIFICATEPATH.FormatWith(_activityData.ProductFamily, _activityData.RadiusServerType)),
                IdCertificatePassword = "xyzzy",
                InstallCertificates = _installCertificates,
                EnterpriseConfiguration = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = AuthenticationMode.PEAP,
                    UserName = DOT1X_USERNAME.FormatWith(_activityData.RadiusServerType.ToString().ToLower(CultureInfo.CurrentCulture)),
                    Password = DOT1X_PASSWORD,
                    EncryptionStrength = EncryptionStrengths.Medium,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = true
                }
            });

                if (!RoamingSamessid(profile, settings, securitySettings,sameSubnet))
                {
                    return false;
                }

                if (_activityData.PrinterInterfaceType == ProductType.SingleInterface)
                {
                    return true;
                }

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Roaming with WEP Personal.");

                Profile accessPointProfile = GetAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), AuthenticationType.Open_System, TransmitKeySelect.KeySelect1, KeySize.Bit_64, _activityData.WirelessRadio);

                securitySettings = new WirelessSecuritySettings(new WEPPersonalSettings()
                {
                    WEPIndex = WEPIndices.Key1,
                    WEPKey = GetWepKey(accessPointProfile, WEPIndices.Key1),
                    WEPMode = WEPModes.Open
                });

                return RoamingSamessid(profile, settings, securitySettings, sameSubnet);
            }
            finally
            {
                TestPostRequisites();
            }
        }

        private bool RoamingSamessid(Profile profile, WirelessSettings settings, WirelessSecuritySettings securitySettings, bool sameSubnet)
        {
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                if (securitySettings.WirelessConfigurationType == WirelessTypes.Enterprise)
                {
                    if (!ConfigureRadiusClient(IPAddress.Parse(_activityData.RadiusServerIp), _activityData.AccessPointDetails.FirstOrDefault().Address))
                    {
                        return false;
                    }

                    if (!ConfigureRadiusServer(securitySettings.EnterpriseSecurity.EnterpriseConfiguration.AuthenticationProtocol))
                    {
                        return false;
                    }
                }

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Step a: Wireless association with first access point with channel 1");

                AccessPointInfo firstAp = _activityData.AccessPointDetails.FirstOrDefault();

                if (!CreateAccessPointProfile(firstAp, profile))
                {
                    return false;
                }

                if (!TestWireless(profile, settings, securitySettings))
                {
                    return false;
                }

                // Read the wireless mac address
                if(!sameSubnet && _activityData.ProductFamily == ProductFamilies.VEP)
                {
                    _activityData.WirelessMacAddress = PrinterFactory.Create(_activityData.ProductFamily.ToString(), _activityData.WirelessInterfaceAddress).MacAddress;
                }

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Step b: Creating same profile on second access point with channel 2");
                profile.RadioSettings.WirelessChannel = WirelessChannel._2;

                if (sameSubnet)
                {
                    if (!CreateAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(x => (x.Address != firstAp.Address && x.Address.IsInSameSubnet(firstAp.Address))), profile))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!CreateAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(x => (x.Address != firstAp.Address && !x.Address.IsInSameSubnet(firstAp.Address))), profile))
                    {
                        return false;
                    }
                }

                if (securitySettings.WirelessConfigurationType == WirelessTypes.Enterprise)
                {
                    if (!ConfigureRadiusClient(IPAddress.Parse(_activityData.RadiusServerIp), _activityData.AccessPointDetails[1].Address))
                    {
                        return false;
                    }

                    if (!ConfigureRadiusServer(securitySettings.EnterpriseSecurity.EnterpriseConfiguration.AuthenticationProtocol))
                    {
                        return false;
                    }
                }

                TraceFactory.Logger.Info($"{CtcBaseTests.STEP_DELIMETER} Step c: Bring down first access point");

                IPowerSwitch powerSwitch = PowerSwitchFactory.Create(IPAddress.Parse(_activityData.PowerSwitchIpAddress));
                if (!powerSwitch.PowerOff(_activityData.AccessPointDetails[0].PortNumber))
                {
                    TraceFactory.Logger.Info("Power down the switch port number failed");
                    return false;
                }

                Thread.Sleep(TimeSpan.FromMinutes(1));

                if (sameSubnet)
                {
                    if (_activityData.ProductFamily == ProductFamilies.TPS || _activityData.ProductFamily == ProductFamilies.InkJet)
                    {
                        _activityData.WirelessInterfaceAddress = CtcUtility.GetPrinterIPAddress(_activityData.WirelessMacAddress);

                        if (string.IsNullOrEmpty(_activityData.WirelessInterfaceAddress))
                        {
                            return false;
                        }
                    }

                    if (!CheckPingStatus(IPAddress.Parse(_activityData.WirelessInterfaceAddress)))
                    {
                        return false;
                    }
                }
                else
                {
                    string wirelessAddress = CtcUtility.GetPrinterIPAddress(_activityData.WirelessMacAddress);

                    if(string.IsNullOrEmpty(wirelessAddress))
                    {
                        TraceFactory.Logger.Info("Printer didnot acquire wireless ip address from different subnet");
                        return false;
                    }
                }

                // TODO: Validate channel number
                return Print();
            }
            finally
            {                
                IPowerSwitch powerSwitch = PowerSwitchFactory.Create(IPAddress.Parse(_activityData.PowerSwitchIpAddress));
                if (!powerSwitch.PowerOn(_activityData.AccessPointDetails[0].PortNumber))
                {
                    TraceFactory.Logger.Info("Power on the switch port number failed");
                }

                GetPrinterOnline(true);

                INetworkSwitch networkSwitch = SwitchFactory.Create(_activityData.SwitchIpAddress);
                networkSwitch.EnablePort(_activityData.AccessPointDetails[0].PortNumber);
            }
        }

        /// <summary>
        /// 756673	
        /// Verify printer can be configured for different frequecy band from EWS page
        /// "1. Connect printer in the network.
        /// 2. Access EWS page of the printer.
        /// 3. Configure Wireless settings under wireless configuration.
        /// 4. Select different frequency bands from EWS Wireless configuration page.
        /// 5. Apply settings and verify printer gets configured with the same.
        /// 6. Repeat the steps for AUTO mode, 2.4 GHz mode and 5 GHz mode."
        /// 1. Printer should be able to get configured for different frequency modes from EWS page.
        /// 756673	Verify Printer can be configured for different frequency bands from Control panel.
        /// "1. Connect printer in the network.
        /// 2. Access EWS page of the printer.
        /// 3. Configure Wireless settings under wireless configuration.
        /// 4. Select different frequency bands from EWS Wireless configuration page.
        /// 5. Apply settings and verify printer gets configured with the same.
        /// 6. Repeat the steps for AUTO mode, 2.4 GHz mode and 5 GHz mode."
        /// 1. Printer should be able to get configured for different frequency modes from EWS page.
        /// </summary>
        /// <param name="accessType">The printer access type</param>
        /// <returns></returns>
        public bool ConfigureFrequencyBands()
        {
            if(!TestPreRequisites())
            {
                return false;
            }

            try
            {
                if(!ConfigureFrequencyBands(PrinterAccessType.EWS))
                {
                    return false;
                }

                return ConfigureFrequencyBands(PrinterAccessType.ControlPanel);
            }
            finally
            {
                TestPostRequisites();
            }
        }

        private bool ConfigureFrequencyBands(PrinterAccessType accessType)
        {
            // 2.4 Ghz
            Profile accessPointProfile = GetAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), WpaVersion.WPA, WpaAlgorithm.AES, WPAKeyType.Passphrase, Radio._2dot4Ghz);

            WirelessSettings settings = new WirelessSettings()
            { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

            WirelessSecuritySettings securitySettings = new WirelessSecuritySettings(new WPAPersonalSettings() { Encryption = WPAEncryptions.AES, passphrase = WPA_PASSPHRASE, Version = WPAVersions.Auto });

            if (!TestWireless(accessPointProfile, settings, securitySettings, accessType: accessType))
            {
                return false;
            }

            // 5 GHz
            accessPointProfile = GetAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), WpaVersion.WPA, WpaAlgorithm.AES, WPAKeyType.Passphrase, Radio._5Ghz);

            settings = new WirelessSettings()
            { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.FiveGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

            securitySettings = new WirelessSecuritySettings(new WPAPersonalSettings() { Encryption = WPAEncryptions.AES, passphrase = WPA_PASSPHRASE, Version = WPAVersions.Auto });

            if (!TestWireless(accessPointProfile, settings, securitySettings, accessType: accessType))
            {
                return false;
            }

            // Auto
            settings = new WirelessSettings()
            { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.Both, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

            securitySettings = new WirelessSecuritySettings(new WPAPersonalSettings() { Encryption = WPAEncryptions.AES, passphrase = WPA_PASSPHRASE, Version = WPAVersions.Auto });

            return TestWireless(accessPointProfile, settings, securitySettings, accessType: accessType);
        } 

        #endregion

        #endregion

        #region Private Methods

        private bool ConfigureNoSecurity(WirelessChannel channel = WirelessChannel.Auto, bool print = true)
        {
            Profile accessPointProfile = GetAccessPointProfile(_activityData.WirelessRadio, channel);

            WirelessSettings settings = new WirelessSettings()
            { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

            WirelessSecuritySettings securitySettings = new WirelessSecuritySettings();

            return TestWireless(accessPointProfile, settings, securitySettings, print: print);
        }

        public bool ConfigureWpaPersonal(WpaVersion version, WpaAlgorithm algorithm, WPAKeyType keyType, PrinterAccessType accessType = PrinterAccessType.EWS)
        {
            Profile accessPointProfile = GetAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), version, algorithm, keyType, _activityData.WirelessRadio);

            WirelessSettings settings = new WirelessSettings()
            { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

            WirelessSecuritySettings securitySettings = new WirelessSecuritySettings(new WPAPersonalSettings() { Encryption = algorithm == WpaAlgorithm.AES ? WPAEncryptions.AES : WPAEncryptions.AUTO, passphrase = (keyType == WPAKeyType.Passphrase ? WPA_PASSPHRASE : WPA_PRESHAREDKEY), Version = version == WpaVersion.WPA2 ? WPAVersions.WPA2 : WPAVersions.Auto });

            return TestWireless(accessPointProfile, settings, securitySettings, accessType: accessType);
        }

        public bool ConfigureWepPersonal(WEPModes wepMode, WEPIndices index, KeySize keySize, AuthenticationType authenticationType, PrinterAccessType accessType = PrinterAccessType.EWS, AccessPointInfo? accessPointInfo = null)
        {
            accessPointInfo = accessPointInfo ?? _activityData.AccessPointDetails.FirstOrDefault();
            Profile accessPointProfile = GetAccessPointProfile(accessPointInfo.Value, authenticationType, TransmitKeySelect.KeySelect1, keySize, _activityData.WirelessRadio);

            WirelessSettings settings = new WirelessSettings()
            { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

            WirelessSecuritySettings securitySettings = new WirelessSecuritySettings(new WEPPersonalSettings()
            {
                WEPIndex = index,
                WEPKey = GetWepKey(accessPointProfile, index),
                WEPMode = wepMode
            }
            );

            return TestWireless(accessPointProfile, settings, securitySettings);
        }

        public bool ConfigureEnterpriseWireless(Modes authMode, WpaAlgorithm algorithm, WpaVersion version, AuthenticationMode printerAuthMode, AuthenticationMode serverAuthMode, EncryptionStrengths strength, AuthenticationMode serverPriority = AuthenticationMode.None)
        {
            Profile accessPointProfile = GetEnterpriseProfile(_activityData.AccessPointDetails.FirstOrDefault(), authMode, version, algorithm);

            WirelessSettings settings = new WirelessSettings()
            { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

            WirelessSecuritySettings securitySettings = new WirelessSecuritySettings((authMode == Modes.WepEnterprise ? WirelessAuthentications.Wep : WirelessAuthentications.Wpa), new EnterpriseSecuritySettings
            {
                Encryption = authMode == Modes.WepEnterprise ? WPAEncryptions.None : algorithm == WpaAlgorithm.AES ? WPAEncryptions.AES : WPAEncryptions.AUTO,
                Version = authMode == Modes.WepEnterprise ? WPAVersions.None : (version == WpaVersion.WPA2 ? WPAVersions.WPA2 : WPAVersions.Auto),
                CACertificatePath = Path.Combine(CtcSettings.ConnectivityShare, CA_CERTIFICATEPATH.FormatWith(_activityData.ProductFamily, _activityData.RadiusServerType)),
                IdCertificatePath = Path.Combine(CtcSettings.ConnectivityShare, ID_CERTIFICATEPATH.FormatWith(_activityData.ProductFamily, _activityData.RadiusServerType)),
                IdCertificatePassword = "xyzzy",
                InstallCertificates = _installCertificates,
                EnterpriseConfiguration = new Dot1XConfigurationDetails
                {
                    AuthenticationProtocol = printerAuthMode,
                    UserName = string.Format(DOT1X_USERNAME, _activityData.RadiusServerType.ToString().ToLower(CultureInfo.CurrentCulture)),
                    Password = DOT1X_PASSWORD,
                    EncryptionStrength = strength,
                    FailSafe = FallbackOption.Connect,
                    ReAuthenticate = true
                }
            });

            if (!ConfigureRadiusClient(IPAddress.Parse(_activityData.RadiusServerIp), _activityData.AccessPointDetails.FirstOrDefault().Address))
            {
                return false;
            }

            if (!ConfigureRadiusServer(serverAuthMode, serverPriority))
            {
                return false;
            }

            return TestWireless(accessPointProfile, settings, securitySettings);
        }

        private string GetWepKey(Profile accessPointProfile, WEPIndices index)
        {
            string key = "";
            switch (index)
            {
                case WEPIndices.Key1:
                    key = accessPointProfile.SecuritySettings.WepKeys.Key1;
                    break;

                case WEPIndices.Key2:
                    key = accessPointProfile.SecuritySettings.WepKeys.Key2;
                    break;

                case WEPIndices.Key3:
                    key = accessPointProfile.SecuritySettings.WepKeys.Key3;
                    break;

                case WEPIndices.Key4:
                    key = accessPointProfile.SecuritySettings.WepKeys.Key4;
                    break;
            }
            return key;
        }

        private List<string> GetRandomHexString(KeySize size, WepKeyType keyType = WepKeyType.HEX)
        {
            int length;

            if (keyType == WepKeyType.HEX)
            {
                length = size == KeySize.Bit_64 ? 10 : 26;
            }
            else
            {
                length = size == KeySize.Bit_64 ? 5 : 13;
            }

            Random random = new Random();

            const string chars = "ABCDEF0123456789";

            List<string> values = new List<string>();

            do
            {
                var value = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());

                if (!values.Contains(value))
                    values.Add(value);
            }
            while (values.Count != 4);

            return values;
        }

        private bool TestWireless(Profile accessPointProfile, WirelessSettings settings, WirelessSecuritySettings securitySettings, AccessPointInfo? accessPointInfo = null, PrinterAccessType accessType = PrinterAccessType.EWS, bool configureAccessPoint = true, bool isInWiredIp = true, bool print = true)
        {
            if (accessPointInfo == null)
            {
                accessPointInfo = _activityData.AccessPointDetails.FirstOrDefault();
            }

            // Installing the certificates for enterprise wireless settings and as the printer loses connectivity if 
            if (securitySettings.WirelessConfigurationType == WirelessTypes.Enterprise)
            {
                if (_installCertificates)
                {
                    TraceFactory.Logger.Info("Installing certificates for enterprise wireless");
                    if (!EwsWrapper.Instance().InstallCACertificate(securitySettings.EnterpriseSecurity.CACertificatePath))
                    {
                        return false;
                    }

                    if (!EwsWrapper.Instance().InstallIDCertificate(securitySettings.EnterpriseSecurity.IdCertificatePath, securitySettings.EnterpriseSecurity.IdCertificatePassword))
                    {
                        return false;
                    }

                    _installCertificates = false;
                }

                securitySettings.EnterpriseSecurity.InstallCertificates = _installCertificates;
            }

            if (_activityData.ProductFamily == ProductFamilies.VEP && _activityData.PrinterInterfaceType == ProductType.MultipleInterface)
            {
                if (accessType == PrinterAccessType.EWS)
                {
                    if (!EwsWrapper.Instance().ConfigureWireless(settings, securitySettings, _activityData.PrinterInterfaceType, validate: false))
                    {
                        return false;
                    }
                }

                if (accessType == PrinterAccessType.SNMP)
                {
                    if (!SnmpWrapper.Instance().ConfigureWireless(settings, securitySettings))
                    {
                        return false;
                    }
                }

                if (configureAccessPoint && !CreateAccessPointProfile(accessPointInfo.Value, accessPointProfile))
                {
                    return false;
                }
            }
            else
            {
                if (configureAccessPoint && !CreateAccessPointProfile(accessPointInfo.Value, accessPointProfile))
                {
                    return false;
                }

                if (accessType == PrinterAccessType.EWS)
                {
                    if (!EwsWrapper.Instance().ConfigureWireless(settings, securitySettings, _activityData.PrinterInterfaceType, validate: isInWiredIp))
                    {
                        return false;
                    }
                }

                if (accessType == PrinterAccessType.SNMP)
                {
                    if (!SnmpWrapper.Instance().ConfigureWireless(settings, securitySettings))
                    {
                        return false;
                    }
                }
            }

            if (_activityData.ProductFamily == ProductFamilies.TPS || _activityData.ProductFamily == ProductFamilies.InkJet)
            {
                if (isInWiredIp)
                {
                    // Disable Switch port to get the wireless ip address
                    INetworkSwitch networkSwitch = SwitchFactory.Create(_activityData.SwitchIpAddress);
                    networkSwitch.DisablePort(_activityData.PrimaryInterfaceAddressPortNumber);
                }

                if (_activityData.EnableDebug)
                {
                    System.Windows.Forms.MessageBox.Show("Check the configuration");
                }

                CoreUtility.Retry.UntilTrue(() => !string.IsNullOrEmpty(_activityData.WirelessInterfaceAddress = CtcUtility.GetPrinterIPAddress(_activityData.WirelessMacAddress)), 3, TimeSpan.FromSeconds(20));

                if (string.IsNullOrEmpty(_activityData.WirelessInterfaceAddress))
                {
                    TraceFactory.Logger.Info($"Printer is not discovered with wireless mac address: {_activityData.WirelessMacAddress}");
                    return false;
                }
            }
            else
            {
                if (_activityData.EnableDebug)
                {
                    System.Windows.Forms.MessageBox.Show("Check the configuration");
                }
            }

            if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromMinutes(3)))
            {
                TraceFactory.Logger.Info($"Wireless Configuration is successful.Ping with wireless IP address: {_activityData.WirelessInterfaceAddress} is successful.");
            }
            else
            {
                TraceFactory.Logger.Info($"Wireless Configuration failed.Ping with wireless IP address: {_activityData.WirelessInterfaceAddress} failed.");
                return false;
            }

            if (print)
            {
                return Print();
            }
            else
            {
                return true;
            }
        }

        private bool Print()
        {
            Printer printer = PrinterFactory.Create(_activityData.ProductFamily.ToString(), _activityData.WirelessInterfaceAddress);

            if (!printer.Install(IPAddress.Parse(_activityData.WirelessInterfaceAddress), Printer.PrintProtocol.RAW, _activityData.DriverPath, _activityData.DriverModel))
            {
                return false;
            }

            Thread.Sleep(TimeSpan.FromMinutes(1));

            return printer.Print(CtcUtility.CreateFile("Wireless_Test_{0}_{1}").FormatWith(_activityData.SessionId, _testNumber));
        }

        private bool TestPostRequisites(bool configureNoSecurity = false)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_POSTREQUISITES);

            return GetPrinterOnline(configureNoSecurity);
        }

        private bool GetPrinterOnline(bool configureNoSecurity = false)
        {
            try
            {
                if (_activityData.ProductFamily != ProductFamilies.VEP)
                {
                    return EnablePrimaryAddress();
                }
                else if (_activityData.ProductFamily == ProductFamilies.VEP && _activityData.PrinterInterfaceType == ProductType.MultipleInterface)
                {
                    Profile accessPointProfile = GetAccessPointProfile(_activityData.WirelessRadio);

                    if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromMinutes(1)))
                    {
                        TraceFactory.Logger.Info($"Ping with wireless IP address: {_activityData.WirelessInterfaceAddress} is successful.");


                        if (configureNoSecurity && _activityData.PrinterInterfaceType == ProductType.MultipleInterface)
                        {
                            accessPointProfile = GetAccessPointProfile(_activityData.WirelessRadio);

                            WirelessSettings settings = new WirelessSettings()
                            { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.FiveGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bg, WirelessRadio = true };

                            WirelessSecuritySettings securitySettings = new WirelessSecuritySettings();

                            return TestWireless(accessPointProfile, settings, securitySettings, print: false);
                        }
                    }
                    else
                    {
                        //WirelessSettings settings = new WirelessSettings()
                        //{ SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.BG, WirelessRadio = true };


                        TraceFactory.Logger.Debug($"Ping with wireless IP address: {_activityData.WirelessInterfaceAddress} failed. Configuring wireless from control panel of wired interface : {_activityData.PrimaryInterfaceAddress}.");
                        CtcUtility.ConfigureWirelessControlPanel(IPAddress.Parse(_activityData.PrimaryInterfaceAddress), accessPointProfile.RadioSettings.SSIDConfiguration.SSID0);
                    }

                    if (!CreateAccessPointProfile(_activityData.AccessPointDetails.FirstOrDefault(), accessPointProfile))
                    {
                        return false;
                    }

                    if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromMinutes(3)))
                    {
                        TraceFactory.Logger.Info($"Ping successful with IP address: {_activityData.WirelessInterfaceAddress}");
                        return true;
                    }
                    else
                    {
                        TraceFactory.Logger.Info($"Ping with IP address: {_activityData.WirelessInterfaceAddress} failed.");
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug(ex.Message);
                return false;
            }
            finally
            {
                string ipAddress = _activityData.ProductFamily == ProductFamilies.VEP && _activityData.PrinterInterfaceType == ProductType.MultipleInterface ? _activityData.WirelessInterfaceAddress : _activityData.PrimaryInterfaceAddress;
                EwsWrapper.Instance().ChangeDeviceAddress(ipAddress);
                TelnetWrapper.Instance().Create(ipAddress);
                SnmpWrapper.Instance().Create(ipAddress);
            }
        }

        private bool EnablePrimaryAddress()
        {
            if (_activityData.ProductFamily != ProductFamilies.VEP)
            {
                INetworkSwitch networkSwitch = SwitchFactory.Create(_activityData.SwitchIpAddress);
                networkSwitch.EnablePort(_activityData.PrimaryInterfaceAddressPortNumber);

                if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.PrimaryInterfaceAddress), TimeSpan.FromMinutes(3)))
                {
                    EwsWrapper.Instance().ChangeDeviceAddress(_activityData.PrimaryInterfaceAddress);
                    TraceFactory.Logger.Info($"Ping with wired IP address: {_activityData.PrimaryInterfaceAddress} is successful.");
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info($"Printer is not available.Ping with wired IP address: {_activityData.PrimaryInterfaceAddress} failed.");
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private Profile GetAccessPointProfile(Radio radio, WirelessChannel channel = WirelessChannel.Auto)
        {
            return new Profile()
            {
                RadioSettings = new RadioSettings()
                {
                    SSIDConfiguration = new SSIDSettings()
                    {
                        SSID0 = SSID0.FormatWith(Guid.NewGuid().ToString().Substring(0, 4)),
                        SSID0State = SsidBroadCast.Enabled
                    },
                    WirelessNetworkModes = radio == Radio._2dot4Ghz ? WirelessNetworkModes.BG : WirelessNetworkModes.AN,
                    WirelessChannel = channel,
                    Radio = radio
                },

                SecuritySettings = new SecuritySettings()
                {
                    ClientTypes = Modes.NoSecurity,
                    WirelessIsolation_Between_SSID = WirelessIsolationBetweenSsid.Enable,
                    WirelessIsolation_WithIn_SSID = WirelessIsolationWithInSsid.Enable,
                }
            };
        }

        private Profile GetAccessPointProfile(AccessPointInfo accessPointInfo, WpaVersion version, WpaAlgorithm algorithm, WPAKeyType keyType, Radio radio, WirelessChannel channel = WirelessChannel.Auto)
        {
            return new Profile()
            {
                RadioSettings = new RadioSettings()
                {
                    SSIDConfiguration = new SSIDSettings()
                    {
                        SSID0 = SSID0.FormatWith(Guid.NewGuid().ToString().Substring(0, 4)),
                        SSID0State = SsidBroadCast.Enabled
                    },
                    WirelessNetworkModes = radio == Radio._2dot4Ghz ? WirelessNetworkModes.BG : WirelessNetworkModes.AN,
                    WirelessChannel = channel,
                    Radio = radio
                },

                SecuritySettings = new SecuritySettings()
                {
                    ClientTypes = Modes.WpaPersonal,
                    WpaPersonalSettings = new WpaPersonalSettings() { KeyRenewal = KEYRENEWAL, Pre_shared_Key = keyType == WPAKeyType.Passphrase ? WPA_PASSPHRASE : WPA_PRESHAREDKEY },
                    WPAAlgorithm = algorithm,
                    WPAVersion = version,
                    WirelessIsolation_Between_SSID = WirelessIsolationBetweenSsid.Enable,
                    WirelessIsolation_WithIn_SSID = WirelessIsolationWithInSsid.Enable,
                }
            };
        }

        private Profile GetAccessPointProfile(AccessPointInfo accessPointInfo, AuthenticationType authType, TransmitKeySelect transmitKey, KeySize size, Radio radio, WepKeyType keyType = WepKeyType.HEX, WirelessChannel channel = WirelessChannel.Auto)
        {
            List<string> keys = GetRandomHexString(size, keyType);

            return new Profile()
            {
                RadioSettings = new RadioSettings()
                {
                    SSIDConfiguration = new SSIDSettings()
                    {
                        SSID0 = SSID0.FormatWith(Guid.NewGuid().ToString().Substring(0, 4)),
                        SSID0State = SsidBroadCast.Enabled
                    },
                    WirelessNetworkModes = radio == Radio._2dot4Ghz ? WirelessNetworkModes.BG : WirelessNetworkModes.AN,
                    WirelessChannel = channel,
                    Radio = radio,
                },
                SecuritySettings = new SecuritySettings()
                {
                    ClientTypes = Modes.WepPersonal,
                    AuthenticationType = authType,
                    WepKeys = new WepKeys()
                    {
                        Key1 = keys[0],
                        Key2 = keys[1],
                        Key3 = keys[2],
                        Key4 = keys[3],
                    },
                    IpaddressType = HP.DeviceAutomation.AccessPoint.IPAddressType.ipv4,
                    KeySize = size,
                    WepKeyType = keyType,
                    TransmitKeySelect = transmitKey,
                    WirelessIsolation_Between_SSID = WirelessIsolationBetweenSsid.Enable,
                    WirelessIsolation_WithIn_SSID = WirelessIsolationWithInSsid.Enable,
                }
            };
        }

        private Profile GetEnterpriseProfile(AccessPointInfo accessPointInfo, Modes authMode, WpaVersion version = WpaVersion.WPA, WpaAlgorithm algorithm = WpaAlgorithm.AES, WirelessChannel channel = WirelessChannel.Auto)
        {
            Profile accessPointProfile = GetAccessPointProfile(_activityData.WirelessRadio, channel);
            if (authMode == Modes.WepEnterprise)
            {
                accessPointProfile.SecuritySettings = new SecuritySettings()
                {
                    ClientTypes = Modes.WepEnterprise,

                    IpaddressType = HP.DeviceAutomation.AccessPoint.IPAddressType.ipv4,

                    WepEnterpriseSettings = new WepEnterpriseSettings()
                    {
                        BroadcastkeyRefreshRate = "7060",
                        Primary_RADIUS_Server = _activityData.RadiusServerIp,
                        Primary_RADIUS_Server_Port = RADIUS_PORT,
                        Primary_Shared_Secret = SHARED_SECRET,
                        Active_Servers = ActiveServers.ServerIpAddress1
                    },
                };
            }
            else
            {
                accessPointProfile.SecuritySettings = new SecuritySettings()
                {
                    ClientTypes = authMode,
                    WpaEnterpriceSettings = new WpaEnterpriceSettings()
                    {
                        Primary_RADIUS_Server = _activityData.RadiusServerIp,
                        Primary_RADIUS_Server_Port = RADIUS_PORT,
                        Primary_Shared_Secret = SHARED_SECRET,
                        KeyRenewal = "7000"
                    },
                    WPAVersion = version,
                    WPAAlgorithm = algorithm
                };
            }

            return accessPointProfile;
        }

        private bool TestPreRequisites(bool isWps = false)
        {
            TraceFactory.Logger.Info(CtcBaseTests.TEST_PREREQUISITES);

            if (isWps)
            {
                if (!_activityData.AccessPointDetails.Any(x => (x.Vendor == AccessPointManufacturer.Cisco && x.Model == WPS_SUPPORTED_MODEL)))
                {
                    TraceFactory.Logger.Info("WPS is not supported on any of the selected access points.");
                    return false;
                }
            }

            string ipAddress = _activityData.ProductFamily == ProductFamilies.VEP && _activityData.PrinterInterfaceType == ProductType.MultipleInterface ? _activityData.WirelessInterfaceAddress : _activityData.PrimaryInterfaceAddress;

            if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(ipAddress), TimeSpan.FromSeconds(30)))
            {
                TraceFactory.Logger.Info("Ping with IP address: {0} is successful.".FormatWith(ipAddress));
                return CheckPrinterAccessibility(ipAddress);
            }
            else
            {
                TraceFactory.Logger.Info("Ping with IP address: {0} failed.".FormatWith(ipAddress));

                if (GetPrinterOnline())
                {
                    return CheckPrinterAccessibility(ipAddress);
                }
                else
                {
                    return CheckPrinterAccessibility(ipAddress, true);
                }
            }
        }

        private bool CheckPrinterAccessibility(string ipAddress, bool validatePing = false)
        {
            bool continueTest = true;

            if (validatePing)
            {
                while (continueTest && !NetworkUtil.PingUntilTimeout(IPAddress.Parse(ipAddress), TimeSpan.FromSeconds(10)))
                {
                    continueTest = CtcUtility.ShowErrorPopup($"Printer: {ipAddress} is not available.\n Please cold reset the printer.");
                }
            }

            while (continueTest && !PrinterFactory.Create(_activityData.ProductFamily.ToString(), ipAddress).IsEwsAccessible(ipAddress, "http"))
            {
                continueTest = CtcUtility.ShowErrorPopup($"Printer: {ipAddress} Web UI is not available.\n Please cold reset the printer.");
            }

            return continueTest;
        }

        private bool CreateAccessPointProfile(AccessPointInfo accessPointInfo, Profile accessPointProfile)
        {
            IAccessPoint accessPoint = AccessPointFactory.Create(accessPointInfo.Address, accessPointInfo.Vendor, accessPointInfo.Model);
            //TraceFactory.Logger.Info("Creating access point profile with {0}".FormatWith(accessPointProfile.ToString()));
            TraceFactory.Logger.Info($"Creating profile: {accessPointProfile.RadioSettings.SSIDConfiguration.SSID0} on {accessPointInfo.Vendor}-{accessPointInfo.Model} Access Point with IP address: {accessPointInfo.Address}");

            TraceFactory.Logger.Info($"Radio Settings:- Radio: {accessPointProfile.RadioSettings.Radio}, Mode: {accessPointProfile.RadioSettings.WirelessNetworkModes}, Channel: {accessPointProfile.RadioSettings.WirelessChannel}");

            switch (accessPointProfile.SecuritySettings.ClientTypes)
            {
                case Modes.NoSecurity:
                    break;
                case Modes.WepPersonal:
                    TraceFactory.Logger.Info($"Key Type: {accessPointProfile.SecuritySettings.WepKeyType}, Index: {(int)accessPointProfile.SecuritySettings.TransmitKeySelect}, Key1:{accessPointProfile.SecuritySettings.WepKeys.Key1}, Key2:{accessPointProfile.SecuritySettings.WepKeys.Key2}, Key3:{accessPointProfile.SecuritySettings.WepKeys.Key3}, Key4:{accessPointProfile.SecuritySettings.WepKeys.Key4}");
                    break;
                case Modes.WpaPersonal:
                    TraceFactory.Logger.Info($"Version: {accessPointProfile.SecuritySettings.WPAVersion}, Encryption: {accessPointProfile.SecuritySettings.WPAAlgorithm}, Key: {accessPointProfile.SecuritySettings.WpaPersonalSettings.Pre_shared_Key}");
                    break;
                case Modes.WepEnterprise:
                    TraceFactory.Logger.Info($"Radius Server: {accessPointProfile.SecuritySettings.WepEnterpriseSettings.Primary_RADIUS_Server}, Key: {accessPointProfile.SecuritySettings.WepEnterpriseSettings.Primary_Shared_Secret}");
                    break;
                case Modes.WpaEnterprise:
                    TraceFactory.Logger.Info($"Version: {accessPointProfile.SecuritySettings.WPAVersion}, Encryption: {accessPointProfile.SecuritySettings.WPAAlgorithm}, Radius Server: {accessPointProfile.SecuritySettings.WpaEnterpriceSettings.Primary_RADIUS_Server}, Key: {accessPointProfile.SecuritySettings.WpaEnterpriceSettings.Primary_Shared_Secret}");
                    break;
            }
            try
            {
                accessPoint.Login(CISCO_USERNAME, CISCO_PASSWORD);
                accessPoint.CreateProfile(accessPointProfile);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Failed to create access point profile.");
                TraceFactory.Logger.Debug(ex.Message);
                return false;
            }
            finally
            {
                accessPoint.Logout();
                Thread.Sleep(TimeSpan.FromSeconds(30));
            }

            TraceFactory.Logger.Info("Successfully created access point profile.");
            return true;
        }

        private bool ConfigureRadiusServer(AuthenticationMode authenticationModes, AuthenticationMode priority = AuthenticationMode.None)
        {
            using (RadiusApplicationServiceClient radiusClient = RadiusApplicationServiceClient.Create(_activityData.RadiusServerIp))
            {
                if (radiusClient.Channel.SetAuthenticationMode(NETWORK_POLICY, authenticationModes, priority))
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

        private bool ConfigureRadiusClient(IPAddress radiusServerIp, IPAddress clientIp)
        {
            using (RadiusApplicationServiceClient radiusCient = RadiusApplicationServiceClient.Create(_activityData.RadiusServerIp))
            {
                radiusCient.Channel.DeleteAllClients();

                if (radiusCient.Channel.AddClient("Automation_AccessPoint", _activityData.AccessPointDetails.FirstOrDefault().Address.ToString(), SHARED_SECRET))
                {
                    TraceFactory.Logger.Info("Successfully added the client : {0} on radius server: {1}".FormatWith(_activityData.AccessPointDetails.FirstOrDefault().Address.ToString(), _activityData.RadiusServerIp));
                    return true;
                }
                else
                {
                    TraceFactory.Logger.Info("Failed to add the client : {0} on radius server: {1}".FormatWith(_activityData.AccessPointDetails.FirstOrDefault().Address.ToString(), _activityData.RadiusServerIp));
                    return false;
                }
            }
        }

        /// <summary>
        /// 
        ///  </summary>
        ///  <param name="wpsMethod"></param>
        ///  <param name="printData"></param>
        ///  <param name="accessPointProfile"></param>
        ///  <param name="accessPointInfo"></param>
        /// <param name="isInWiredIp"></param>
        /// <returns></returns>
        private bool TestWpsMethods(WPSMethods wpsMethod, string printData, Profile accessPointProfile, AccessPointInfo? accessPointInfo = null, TimeSpan? delay = null, bool isInWiredIp = true)
        {
            string pin = string.Empty;

            //accessPointProfile = accessPointProfile == null ? GetBasicAccessPintProfile(_activityData) : accessPointProfile;

            accessPointInfo = accessPointInfo ?? _activityData.AccessPointDetails.FirstOrDefault(x => (x.Vendor == AccessPointManufacturer.Cisco && x.Model == WPS_SUPPORTED_MODEL));

            if (wpsMethod == WPSMethods.WPSPin)
            {
                if (!TestWpsPin(accessPointInfo.Value, accessPointProfile, enableDebug: _activityData.EnableDebug))
                {
                    return false;
                }
            }
            else
            {
                if (!TestWpsPush(accessPointInfo.Value, accessPointProfile, enableDebug: _activityData.EnableDebug))
                {
                    return false;
                }
            }

            if (isInWiredIp)
            {
                // Disable Switch port to get the wireless ip address
                INetworkSwitch networkSwitch = SwitchFactory.Create(_activityData.SwitchIpAddress);
                networkSwitch.DisablePort(_activityData.PrimaryInterfaceAddressPortNumber);
                Thread.Sleep(TimeSpan.FromSeconds(30));
            }

            if (_activityData.EnableDebug)
            {
                System.Windows.Forms.MessageBox.Show("Check the configuration");
            }

            CoreUtility.Retry.UntilTrue(() => !string.IsNullOrEmpty(_activityData.WirelessInterfaceAddress = CtcUtility.GetPrinterIPAddress(_activityData.WirelessMacAddress)), 5, TimeSpan.FromSeconds(10));

            if (string.IsNullOrEmpty(_activityData.WirelessInterfaceAddress))
            {
                TraceFactory.Logger.Info("Printer is not discovered with wireless mac address: {0}".FormatWith(_activityData.WirelessMacAddress));
                return false;
            }

            if (NetworkUtil.PingUntilTimeout(IPAddress.Parse(_activityData.WirelessInterfaceAddress), TimeSpan.FromMinutes(3)))
            {
                TraceFactory.Logger.Info("Wireless Configuration is successful.Ping with wireless IP address: {0} is successful.".FormatWith(_activityData.WirelessInterfaceAddress));
            }
            else
            {
                TraceFactory.Logger.Info("Wireless Configuration failed.Ping with wireless IP address: {0} failed.".FormatWith(_activityData.WirelessInterfaceAddress));
                return false;
            }

            Printer printer = PrinterFactory.Create(_activityData.ProductFamily.ToString(), _activityData.WirelessInterfaceAddress);

            if (!printer.Install(IPAddress.Parse(_activityData.WirelessInterfaceAddress), Printer.PrintProtocol.RAW, _activityData.DriverPath, _activityData.DriverModel))
            {
                return false;
            }

            Thread.Sleep(TimeSpan.FromMinutes(1));

            return printer.Print(CtcUtility.CreateFile("Wireless_Test_{0}").FormatWith(printData));
        }

        private bool TestWpsPin(AccessPointInfo accessPointInfo, Profile accessPointProfile = null, TimeSpan? delay = null, bool validPin = true, bool enableDebug = false, bool validateEws = false)
        {
            string pin = string.Empty;

            if (_activityData.ProductFamily == ProductFamilies.VEP)
            {
                using (IDevice device = DeviceFactory.Create(_activityData.PrimaryInterfaceAddress))
                {
                    IWireless wireles = WirelessFactory.Create(device);
                    pin = wireles.GenerateWpsPin();
                }
            }
            else
            {
                EwsWrapper.Instance().GenerateWpsPin();
            }

            if (string.IsNullOrEmpty(pin))
            {
                return false;
            }

            if (!validPin)
            {
                pin = "{0}1".FormatWith(pin.Substring(0, pin.Length - 2));
                TraceFactory.Logger.Info("WPS Pin Enrollment with invalid pin: {0}.".FormatWith(pin));
            }

            IAccessPoint accessPoint = AccessPointFactory.Create(accessPointInfo.Address, accessPointInfo.Vendor, accessPointInfo.Model);

            try
            {
                accessPoint.Login(CISCO_USERNAME, CISCO_PASSWORD);

                if (accessPointProfile != null)
                {
                    accessPoint.CreateProfile(accessPointProfile);
                }

                TraceFactory.Logger.Info("Starting WPS Pin enrollment on {0}-{1} access point with IP address: {2}".FormatWith(accessPointInfo.Vendor, accessPointInfo.Model, accessPointInfo.Address));
                accessPoint.StartPinEnrollment(pin, 1, 0);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Failed to perform WPS Pin on {0}-{1} access point with IP address: {2}".FormatWith(accessPointInfo.Vendor, accessPointInfo.Model, accessPointInfo.Address));
                TraceFactory.Logger.Info(ex.Message);
                return false;
            }
            finally
            {
                accessPoint.Logout();
            }

            if (enableDebug)
            {
                System.Windows.Forms.MessageBox.Show("Check access point configuration");
            }

            if (delay != null)
            {
                Thread.Sleep(delay.Value);
            }

            if (_activityData.ProductFamily == ProductFamilies.VEP)
            {
                using (IDevice device = DeviceFactory.Create(_activityData.PrimaryInterfaceAddress))
                {
                    IWireless wireles = WirelessFactory.Create(device);
                    wireles.ConfigureWpsPin();

                    return (validPin || delay == null) ? CheckPingStatus(IPAddress.Parse(_activityData.WirelessInterfaceAddress)) : !CheckPingStatus(IPAddress.Parse(_activityData.WirelessInterfaceAddress));
                }
            }

            return validPin || delay == null ? EwsWrapper.Instance().StartWpsPinEnrollment() : !EwsWrapper.Instance().StartWpsPinEnrollment(validateEws);
        }

        private bool TestWpsPush(AccessPointInfo accessPointInfo, Profile accessPointProfile, TimeSpan? delay = null, bool enableDebug = false, bool validateEws = false)
        {
            IAccessPoint accessPoint = AccessPointFactory.Create(accessPointInfo.Address, accessPointInfo.Vendor, accessPointInfo.Model);

            try
            {
                accessPoint.Login(CISCO_USERNAME, CISCO_PASSWORD);

                if (accessPointProfile != null)
                {
                    accessPoint.CreateProfile(accessPointProfile);
                }

                TraceFactory.Logger.Info("Starting WPS Push on access point");
                accessPoint.StartPbcEnrollment(1, 0);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Info("Failed to perform WPS Push on access point");
                TraceFactory.Logger.Info(ex.Message);
                return false;
            }
            finally
            {
                accessPoint.Logout();
            }

            if (enableDebug)
            {
                System.Windows.Forms.MessageBox.Show("Check access point configuration");
            }

            if (delay != null)
            {
                TraceFactory.Logger.Info($"Waiting for {delay.Value.TotalMinutes} after starting WPS Push on Access Point.");
                Thread.Sleep(delay.Value);
            }

            if (_activityData.ProductFamily == ProductFamilies.VEP)
            {
                using (IDevice device = DeviceFactory.Create(_activityData.PrimaryInterfaceAddress))
                {
                    IWireless wireles = WirelessFactory.Create(device);
                    wireles.ConfigureWpsPush();
                }

                if (delay == null)
                {
                    return CheckPingStatus(IPAddress.Parse(_activityData.WirelessInterfaceAddress));
                }
                else
                {
                    return !CheckPingStatus(IPAddress.Parse(_activityData.WirelessInterfaceAddress));
                }
            }

            return delay == null ? EwsWrapper.Instance().PerformWpsPushEnrollment(validateEws) : !EwsWrapper.Instance().PerformWpsPushEnrollment(validateEws);
        }

        private bool CheckPingStatus(IPAddress address)
        {
            if (NetworkUtil.PingUntilTimeout(address, TimeSpan.FromMinutes(2)))
            {
                TraceFactory.Logger.Info($"Ping with ip address: {address} is successful.");
                return true;
            }
            else
            {
                TraceFactory.Logger.Info($"Ping with ip address: {address} failed.");
                return false;
            }
        }



        public bool configureFrequencyMode()
        {
            try
            {
                bool result = true;
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                Profile accessPointProfile = GetAccessPointProfile(_activityData.WirelessRadio);

                WirelessSettings settings = new WirelessSettings()
                { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.TwoDotFourGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bgn, WirelessRadio = true };

                WirelessSecuritySettings securitySettings = new WirelessSecuritySettings();
                result &= TestWireless(accessPointProfile, settings, securitySettings);

                TestPostRequisites();

                settings = new WirelessSettings()
                { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.Both, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bgn, WirelessRadio = true };
                result &= TestWireless(accessPointProfile, settings, securitySettings);

                TestPostRequisites();

                settings = new WirelessSettings()
                { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.FiveGHz, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bgn, WirelessRadio = true };
                result &= TestWireless(accessPointProfile, settings, securitySettings);

                return result;
            }
            finally
            {
                TestPostRequisites();
            }

        }


        public bool WirelessSettingOnRestoreDefault()
        {
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                Profile accessPointProfile = GetAccessPointProfile(_activityData.WirelessRadio);

                WirelessSettings settings = new WirelessSettings()
                { SsidName = accessPointProfile.RadioSettings.SSIDConfiguration.SSID0, WirelessBand = WirelessBands.Both, WirelessConfigurationMethod = WirelessConfigMethods.NetworkName, WirelessMode = WirelessModes.Bgn, WirelessRadio = true };

                EwsWrapper.Instance().ResetDefaults();

                WirelessSecuritySettings securitySettings = new WirelessSecuritySettings();

                return TestWireless(accessPointProfile, settings, securitySettings);
            }
            finally
            {
                TestPostRequisites();
            }
        }


        public bool ConfigureWEPMode()
        {
            try
            {
                bool result = true;
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);

                result = ConfigureWepPersonal(WEPModes.Auto, WEPIndices.Key1, KeySize.Bit_64, AuthenticationType.Shared_Key);

                result &= ConfigureWepPersonal(WEPModes.Auto, WEPIndices.Key2, KeySize.Bit_64, AuthenticationType.Shared_Key);

                result &= ConfigureWepPersonal(WEPModes.Auto, WEPIndices.Key3, KeySize.Bit_64, AuthenticationType.Shared_Key);

                result &= ConfigureWepPersonal(WEPModes.Auto, WEPIndices.Key4, KeySize.Bit_64, AuthenticationType.Shared_Key);

                return result;
            }
            finally
            {
                TestPostRequisites();
            }
        }


        public bool ConfigureWPAMode()
        {
            try
            {
                TraceFactory.Logger.Info(CtcBaseTests.TEST_EXECUTION);
                return ConfigureWpaPersonal(WpaVersion.WPA, WpaAlgorithm.AES, WPAKeyType.Passphrase);
            }
            finally
            {
                TestPostRequisites();
            }
        }
        #endregion
    }
}