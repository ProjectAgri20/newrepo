using HP.ScalableTest.PluginSupport.Connectivity;

namespace HP.ScalableTest.Plugin.WiFiDirect
{
    /// <summary>
    /// IPSecurity Test cases
    /// </summary>
    internal class WiFiDirectTests : CtcBaseTests
    {
        #region Local Variables

        /// <summary>
        /// Instance of IPSecurityActivityData
        /// </summary>
        WiFiDirectActivityData _activityData;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="activityData"></param>
        public WiFiDirectTests(WiFiDirectActivityData activityData)
            : base(activityData.ProductName)
        {
            _activityData = activityData;
            ProductFamily = activityData.ProductFamily.ToString();
            Sliver = "WiFi Direct";
        }

        #endregion

        #region Tests

        [TestDetails(Id = 756409, Category = "WiFiDirect", Description = "Verify AP is configured with the default channel", ProductCategory = ProductFamilies.All)]
        public bool Test_756409()
        {
            return WiFiDirectTemplates.ValidateChannelWithWirelessSta(_activityData, CtcUtility.GetTestId());
        }

        [TestDetails(Id = 756510, Category = "WiFiDirect", Description = "Wifi Direct Association in Advanced connection type", ProductCategory = ProductFamilies.All)]
        public bool Test_756510()
        {
            return WiFiDirectTemplates.ValidateWiFiAdvancedOptions(_activityData, CtcUtility.GetTestId());
        }

        [TestDetails(Id = 756405, Category = "WiFiDirect", Description = "Wifi Direct Association With Self SSID", ProductCategory = ProductFamilies.All)]
        public bool Test_756405()
        {
            return WiFiDirectTemplates.ValidateConnectionWithSelfSSID(_activityData, CtcUtility.GetTestId());
        }

        [TestDetails(Id = 756469, Category = "WiFiDirect", Description = "Verify the AP behavior with Ethernet cable plugged in and out", ProductCategory = ProductFamilies.All)]
        public bool Test_756469()
        {
            return WiFiDirectTemplates.ValidateWiFiWithEthernetCablePluggedInAndOut(_activityData, CtcUtility.GetTestId());
        }

        [TestDetails(Id = 756418, Category = "WiFiDirect", Description = "Wireless Direct or Wifi Direct DHCP-Client list after power cycle", ProductCategory = ProductFamilies.All)]
        public bool Test_756418()
        {
            return WiFiDirectTemplates.ValidateWiFiClientWithPowerCycle(_activityData, CtcUtility.GetTestId());
        }

        [TestDetails(Id = 756402, Category = "WiFiDirect", Description = "Wfifi Direct-SSID-Configuration", ProductCategory = ProductFamilies.All)]
        public bool Test_756402()
        {
            return WiFiDirectTemplates.ValidateWifiSSIDConfiguration(_activityData, CtcUtility.GetTestId());
        }

        [TestDetails(Id = 756423, Category = "WiFiDirect", Description = "Wireless Direct or Wifi Direct -NHOT- Multicast and Discovery", ProductCategory = ProductFamilies.VEP)]
        public bool Test_756423()
        {
            return WiFiDirectTemplates.ValidateWiFiDiscoveryWithAccessories(_activityData, CtcUtility.GetTestId());
        }

        #endregion
    }

}
