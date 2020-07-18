using HP.DeviceAutomation.Phoenix;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HP.ScalableTest.DeviceAutomation.Wireless
{
    class PhoenixNovaWireless : DeviceWorkflowLogSource, IWireless
    {
        PhoenixNovaControlPanel _controlPanel;
        Pacekeeper _pacekeeper;

        public PhoenixNovaWireless(PhoenixNovaDevice device)
        {
            _controlPanel = device.ControlPanel;
            _pacekeeper =  new Pacekeeper(TimeSpan.FromSeconds(5));
        }

        public bool ConfigureWirelessStation(string ssid, WirelessModes wirelessMode, WirelessStaModes mode, WirelessBands band, WirelessAuthentications auth, string passphrase, int keyIndex)
        {
            NavigateToWireless();
            return false;
        }

        public string GenerateWpsPin()
        {
            NavigateToWireless();

            _controlPanel.Press("cWirelessWPSSetup");
            _pacekeeper.Pause();

            if (_controlPanel.GetDisplayedStrings().ToString().Replace("\n", string.Empty).Contains("not available"))
            {
                throw new NotSupportedException("Wireless configuration is possible when the printer is in wireless connection");
            }

            _controlPanel.Press("cWirelessGeneratePIN");
            IEnumerable<string> items = _controlPanel.GetDisplayedStrings();
            string pin = Regex.Match(items.First(x => x.Contains("PIN")), @"\d+").Value;
            return pin;
        }

        public bool ConfigureWpsPush()
        {
            NavigateToWireless();

            if (_controlPanel.GetDisplayedStrings().ToString().Replace("\n", string.Empty).Contains("not available"))
            {
                throw new NotSupportedException("Wireless configuration is possible when the printer is in wireless connection");
            }

            return false;
        }

        public bool ConfigureWifiDirect(WiFiDirectConnectionMode mode, string password)
        {
            throw new NotImplementedException();
        }

        public void ConfigureWpsPin()
        {
            NavigateToWireless();

            _controlPanel.Press("cOKTouchButton");
            _pacekeeper.Pause();

            if (_controlPanel.GetDisplayedStrings().ToString().Replace("\n", string.Empty).Contains("not available"))
            {
                throw new NotSupportedException("Wireless configuration is possible when the printer is in wireless connection");
            }
        }

        /// <summary>
        /// Navigate to wireless page
        /// </summary>
        private void NavigateToWireless()
        {
            _controlPanel.PressKey(PhoenixSoftKey.Home);

            do
            {
                _controlPanel.ScrollRight(1);
            }
            while (!_controlPanel.GetVirtualButtons().Any(x => x.Name == "cSetupHomeTouchButton"));

            _controlPanel.Press("cSetupHomeTouchButton");
            _pacekeeper.Pause();
            do
            {
                _controlPanel.ScrollDown(50);
            }
            while (!_controlPanel.GetVirtualButtons().Any(x => x.Name == "cNetworkConfig"));

            _controlPanel.Press("cNetworkConfig");
            _pacekeeper.Pause();
            _controlPanel.Press("cWirelessMenu");
            _pacekeeper.Pause();
        }
    }
}
