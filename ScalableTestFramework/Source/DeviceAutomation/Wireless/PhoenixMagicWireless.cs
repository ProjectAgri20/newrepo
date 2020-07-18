using System;

namespace HP.ScalableTest.DeviceAutomation.Wireless
{
    class PhoenixMagicWireless : DeviceWorkflowLogSource, IWireless
    {
        public bool ConfigureWifiDirect(WiFiDirectConnectionMode mode, string password)
        {
            throw new NotImplementedException();
        }

        public bool ConfigureWirelessStation(string ssid, WirelessModes wirelessMode, WirelessStaModes mode, WirelessBands band, WirelessAuthentications auth, string passphrase, int keyIndex)
        {
            throw new NotImplementedException();
        }

        public string GenerateWpsPin()
        {
            throw new NotImplementedException();
        }

        public bool ConfigureWpsPush()
        {
            throw new NotImplementedException();
        }

        public void ConfigureWpsPin()
        {
            throw new NotImplementedException();
        }
    }
}
