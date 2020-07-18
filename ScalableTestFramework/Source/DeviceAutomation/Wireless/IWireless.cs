namespace HP.ScalableTest.DeviceAutomation.Wireless
{
    /// <summary>
    /// Interface for the wireless configuration
    /// </summary>
    public interface IWireless
    {
        /// <summary>
        /// Configures wireless station from control panel
        /// </summary>
        /// <param name="ssid">The ssid for authentication</param>
        /// <param name="wirelessMode"><see cref="WirelessModes"/></param>
        /// <param name="mode"><see cref="WirelessStaModes"/></param>
        /// <param name="band"><see cref="WirelessBands"/></param>
        /// <param name="auth"><see cref="WirelessAuthentications"/></param>
        /// <param name="passphrase">The passphrase for authentication</param>
        /// <param name="keyIndex">The key index for WEP configuration</param>
        /// <returns></returns>
        bool ConfigureWirelessStation(string ssid, WirelessModes wirelessMode, WirelessStaModes mode, WirelessBands band, WirelessAuthentications auth, string passphrase, int keyIndex);

        /// <summary>
        /// Generate WPS Pin  from control panel
        /// </summary>
        /// <returns>The Wps pin generated</returns>
        string GenerateWpsPin();

        /// <summary>
        /// Starts Wps Pin on printer from control panel
        /// </summary>
        void ConfigureWpsPin();

        /// <summary>
        /// Configures WPS Push from control panel
        /// </summary>
        /// <returns>True if the operation is successful</returns>
        bool ConfigureWpsPush();

        /// <summary>
        /// Configure WiFi Direct from control panel
        /// </summary>
        /// <param name="mode"><see cref="WiFiDirectConnectionMode"/></param>
        /// <param name="password">The password</param>
        /// <returns></returns>
        bool ConfigureWifiDirect(WiFiDirectConnectionMode mode, string password);

    }
}
