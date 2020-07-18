using HP.DeviceAutomation.AccessPoint;
using HP.ScalableTest.DeviceAutomation.Wireless;
using HP.ScalableTest.PluginSupport.Connectivity;
using HP.ScalableTest.PluginSupport.Connectivity.EmbeddedWebServer;
using HP.ScalableTest.PluginSupport.Connectivity.RadiusServer;

namespace HP.ScalableTest.Plugin.Wireless
{
    /// <summary>
    /// IPSecurity Test cases
    /// </summary>
    internal class WirelessTests : CtcBaseTests
    {
        #region Local Variables

        private readonly WirelessTemplates _wirelessTemplate;

        #endregion Local Variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="activityData"></param>
        public WirelessTests(WirelessActivityData activityData)
            : base(activityData.ProductName)
        {
            ProductFamily = activityData.ProductFamily.ToString();
            Sliver = "Wireless";
            _wirelessTemplate = new WirelessTemplates(activityData);
        }

        #endregion Constructor

        #region Tests

        [TestDetails(Id = 756582, Category = "No Security", Description = "Verify No Security", ProductCategory = ProductFamilies.All)]
        public bool Test_756582()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.NoSecurity();
        }

        [TestDetails(Id = 756631, Category = "WPA-Personal", Description = "Verify WPA, AES, Passphrase", ProductCategory = ProductFamilies.All)]
        public bool Test_756631()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.WPAPersonal_WPA_AES_Passphrase();
        }

        [TestDetails(Id = 756587, Category = "WPA-Personal", Description = "Verify WPA, AES and AUTO, Passphrase", ProductCategory = ProductFamilies.All)]
        public bool Test_756587()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.WPAPersonal_WPA_AESAUTO_Passphrase();
        }

        [TestDetails(Id = 756589, Category = "WPA-Personal", Description = "Verify AUTO and WPA, AUTO, Passphrase and PSK", ProductCategory = ProductFamilies.All)]
        public bool Test_756589()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.WPAPersonal_AUTOWPA_AUTO_PassphrasePSK();
        }

        [TestDetails(Id = 756588, Category = "WPA-Personal", Description = "Verify WPA, TKIP, PSK and Passphrase", ProductCategory = ProductFamilies.All)]
        public bool Test_756588()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.WPAPersonal_WPA_TKIP_PSKPassphrase();
        }

        [TestDetails(Id = 756594, Category = "WPA-Personal", Description = "Verify AUTO, AES, PSK", ProductCategory = ProductFamilies.All)]
        public bool Test_756594()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.WPAPersonal_AUTO_AES_PSK();
        }

        [TestDetails(Id = 756593, Category = "WPA-Personal", Description = "Verify AUTO, AUTO, PSK", ProductCategory = ProductFamilies.All)]
        public bool Test_756593()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.WPAPersonal_AUTO_AUTO_PSK();
        }

        [TestDetails(Id = 756558, Category = "WPA-Personal", Description = "WPA with wrong passphrase", ProductCategory = ProductFamilies.All)]
        public bool Test_756558()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpaAssociationWithWrongPassphrase();
        }

        [TestDetails(Id = 756590, Category = "WPA2-Personal", Description = "Verify WPA2, AES and AUTO, Passphrase", ProductCategory = ProductFamilies.All)]
        public bool Test_756590()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.WPAPersonal_WPA2_AESAUTO_Passphrase();
        }

        [TestDetails(Id = 756592, Category = "WPA2-Personal", Description = "Verify WPA2, AUTO, PSK", ProductCategory = ProductFamilies.All)]
        public bool Test_756592()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.WPAPersonal_WPA2_AUTO_PSK();
        }

        [TestDetails(Id = 756591, Category = "WPA2-Personal", Description = "Verify WPA2, AUTO, PSK", ProductCategory = ProductFamilies.All)]
        public bool Test_756591()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.WPAPersonal_WPA2_TKIP_PSKPassphrase();
        }

        [TestDetails(Id = 756595, Category = "WPA2-Personal", Description = "Verify AUTO, TKIP, PSK", ProductCategory = ProductFamilies.All)]
        public bool Test_756595()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.WPAPersonal_AUTO_TKIP_PSK();
        }

        [TestDetails(Id = 756586, Category = "WEP-Personal", Description = "Verify WEP, Shared key, Key1", ProductCategory = ProductFamilies.All)]
        public bool Test_756586()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.WEP_SharedKey();
        }

        [TestDetails(Id = 756583, Category = "WEP-Personal", Description = "Verify WEP with 64-bit HEX/ASCII key", ProductCategory = ProductFamilies.All)]
        public bool Test_756583()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.WepKeyLengths(KeySize.Bit_64);
        }

        [TestDetails(Id = 756585, Category = "WEP-Personal", Description = "Verify WEP with different key index", ProductCategory = ProductFamilies.All)]
        public bool Test_756585()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.WepKeyIndex();
        }

        [TestDetails(Id = 756584, Category = "WEP-Personal", Description = "Verify WEP with 128-bit HEX/ASCII key", ProductCategory = ProductFamilies.All)]
        public bool Test_756584()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.WepKeyLengths(KeySize.Bit_128);
        }

        [TestDetails(Id = 756559, Category = "WEP-Personal", Description = "WEP errors", ProductCategory = ProductFamilies.All)]
        public bool Test_756559()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.ErrorCheck();
        }

        [TestDetails(Id = 756636, Category = "WPA-Enterprise", Description = "Verify WPA, TKIP, PEAP", ProductCategory = ProductFamilies.All)]
        public bool Test_756636()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpaEnterprise(AuthenticationMode.PEAP, AuthenticationMode.PEAP, AuthenticationMode.EAPTLS);
        }

        [TestDetails(Id = 756633, Category = "WPA-Enterprise", Description = "Verify WPA, TKIP, EAPTLS", ProductCategory = ProductFamilies.All)]
        public bool Test_756633()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpaEnterprise(AuthenticationMode.EAPTLS, AuthenticationMode.EAPTLS);
        }

        [TestDetails(Id = 756550, Category = "WPA-Enterprise", Description = "Hostname with wpa enterprise and restore", ProductCategory = ProductFamilies.TPS)]
        public bool Test_756550()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.HostnameWithEnterpriseWireless();
        }

        [TestDetails(Id = 756561, Category = "WPA-Enterprise", Description = "EAPTLS, PEAP authentication with AD Username change", ProductCategory = ProductFamilies.All)]
        public bool Test_756561()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.EnterpriseWirelessUserNameChange();
        }

        [TestDetails(Id = 756637, Category = "WPA2-Enterprise", Description = "Verify WPA2, AES, PEAP", ProductCategory = ProductFamilies.All)]
        public bool Test_756637()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpa2Enterprise(AuthenticationMode.PEAP, AuthenticationMode.PEAP, AuthenticationMode.EAPTLS);
        }

        [TestDetails(Id = 756634, Category = "WPA2-Enterprise", Description = "Verify WPA2, AES, EAPTLS", ProductCategory = ProductFamilies.All)]
        public bool Test_756634()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpa2Enterprise(AuthenticationMode.EAPTLS, AuthenticationMode.EAPTLS);
        }

        [TestDetails(Id = 756556, Category = "WPA-WEP", Description = "Error handling", ProductCategory = ProductFamilies.All)]
        public bool Test_756556()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWrongPasswordLength();
        }

        [TestDetails(Id = 756576, Category = "Error", Description = "Radio Status", ProductCategory = ProductFamilies.All)]
        public bool Test_756576()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.CheckRadioStatus();
        }

        [TestDetails(Id = 756579, Category = "Restore", Description = "Wireless settings with restore", ProductCategory = ProductFamilies.All)]
        public bool Test_756579()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.RestoreFactory();
        }

        [TestDetails(Id = 756554, Category = "WPS-Push", Description = "Verify WPS push timeout", ProductCategory = ProductFamilies.TPS)]
        public bool Test_756554()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpsPushPinTimeOut(WPSMethods.WPSPush);
        }

        [TestDetails(Id = 756555, Category = "WPS-Pin", Description = "Verify WPS pin timeout", ProductCategory = ProductFamilies.TPS)]
        public bool Test_756555()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpsPushPinTimeOut(WPSMethods.WPSPush);
        }

        [TestDetails(Id = 756552, Category = "WPS-Pin", Description = "Verify WPS pin invalid pin", ProductCategory = ProductFamilies.TPS)]
        public bool Test_756552()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpsInvalidPin();
        }

        [TestDetails(Id = 756551, Category = "WPS-Pin", Description = "Verify ssid change after successful configuration", ProductCategory = ProductFamilies.TPS)]
        public bool Test_756551()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpsPinSsidChangeinAccessPoint();
        }

        [TestDetails(Id = 781442, Category = "WPS-Pin", Description = "Verify WPA Personal to WPS Pin Switching", ProductCategory = ProductFamilies.TPS)]
        public bool Test_781442()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpaPersonalWpsSwitching(wpaVersion: WpaVersion.WPA, wpsMethod: WPSMethods.WPSPin);
        }

        [TestDetails(Id = 781443, Category = "WPS-Pin", Description = "Verify WPA2 Personal to WPS Pin Switching", ProductCategory = ProductFamilies.TPS)]
        public bool Test_781443()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpaPersonalWpsSwitching(wpaVersion: WpaVersion.WPA2, wpsMethod: WPSMethods.WPSPin);
        }

        [TestDetails(Id = 781447, Category = "WPS-Pin", Description = "Verify WEP to WPS Push Switching", ProductCategory = ProductFamilies.TPS)]
        public bool Test_781447()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpaPersonalWpsSwitching(WirelessAuthentications.Wep);
        }

        [TestDetails(Id = 781448, Category = "WPS-Pin", Description = "Verify WPA Personal to WPS Push Switching", ProductCategory = ProductFamilies.TPS)]
        public bool Test_781448()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpaPersonalWpsSwitching(wpaVersion: WpaVersion.WPA);
        }

        [TestDetails(Id = 781449, Category = "WPS-Pin", Description = "Verify WPA2 Personal to WPS Push Switching", ProductCategory = ProductFamilies.TPS)]
        public bool Test_781449()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpaPersonalWpsSwitching(wpaVersion: WpaVersion.WPA2);
        }

        [TestDetails(Id = 781440, Category = "WPS-Pin", Description = "Verify WPS pin length from web UI and control panel.", ProductCategory = ProductFamilies.TPS)]
        public bool Test_781440()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpsPinLength();
        }

        [TestDetails(Id = 756553, Category = "WPS-Push", Description = "WPS Push with unsupported access point", ProductCategory = ProductFamilies.TPS)]
        public bool Test_756553()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpsUnsupportedAp();
        }

        [TestDetails(Id = 756629, Category = "WPS-Push", Description = "WPS", ProductCategory = ProductFamilies.TPS)]
        public bool Test_756629()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWpa2Push();
        }

        [TestDetails(Id = 756638, Category = "WEP-Enterprise", Description = "Verify WEP, PEAP", ProductCategory = ProductFamilies.VEP)]
        public bool Test_756638()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWepEnterprise(AuthenticationMode.PEAP, AuthenticationMode.PEAP, AuthenticationMode.EAPTLS);
        }

        [TestDetails(Id = 756635, Category = "WEP-Enterprise", Description = "Verify WEP, EAPTLS", ProductCategory = ProductFamilies.VEP)]
        public bool Test_756635()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.VerifyWepEnterprise(AuthenticationMode.EAPTLS, AuthenticationMode.EAPTLS);
        }

        #endregion Wireless Network Security

        #region Interoperability

        [TestDetails(Id = 756648, Category = "Interoperability", Description = "Multiple Channels 1-13", ProductCategory = ProductFamilies.All)]
        public bool Test_756648()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.CheckLowerChannels();
        }

        [TestDetails(Id = 756680, Category = "Interoperability", Description = "Multiple Channels from 36", ProductCategory = ProductFamilies.All)]
        public bool Test_756680()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.CheckHigherChannels();
        }

        [TestDetails(Id = 756652, Category = "Interoperability", Description = "Printer idle before configuration", ProductCategory = ProductFamilies.All)]
        public bool Test_756652()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.IdleTime(true);
        }

        [TestDetails(Id = 756651, Category = "Interoperability", Description = "Printer idle after configuration", ProductCategory = ProductFamilies.All)]
        public bool Test_756651()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.IdleTime(false);
        }

        [TestDetails(Id = 756667, Category = "Interoperability", Description = "Roaming in same subnet", ProductCategory = ProductFamilies.All)]
        public bool Test_756667()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.RoamingSameSsid(true);
        }

        [TestDetails(Id = 756673, Category = "Interoperability", Description = "Differet frequency bands from EWS, Control Panel", ProductCategory = ProductFamilies.All)]
        public bool Test_756673()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.ConfigureFrequencyBands();
        }

        [TestDetails(Id = 756672, Category = "Interoperability", Description = "Printer Association in Frequency Mode", ProductCategory = ProductFamilies.All)]
        public bool Test_756672()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.configureFrequencyMode();
        }


        [TestDetails(Id = 756675, Category = "Interoperability", Description = "wireless setting on restore default", ProductCategory = ProductFamilies.All)]
        public bool Test_756675()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.WirelessSettingOnRestoreDefault();
        }

        [TestDetails(Id = 756654, Category = "Interoperability", Description = "Printer Association in WEP Mode", ProductCategory = ProductFamilies.VEP)]
        public bool Test_756654()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.ConfigureWEPMode();
        }


        [TestDetails(Id = 756655, Category = "Interoperability", Description = "Printer Association in WPA Mode", ProductCategory = ProductFamilies.All)]
        public bool Test_756655()
        {
            _wirelessTemplate.SetTestNumber(CtcUtility.GetTestId());
            return _wirelessTemplate.ConfigureWPAMode();
        }

        #endregion
    }

}