using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Phoenix;

namespace HP.ScalableTest.PluginSupport.Connectivity
{
    public class ControlPanelUtility
    {
        private readonly PhoenixMagicFrameDevice _phoenixMagicFrameDevice;
        private readonly PhoenixNovaDevice _phoenixNovaDevice;

        private readonly TimeSpan _delay = TimeSpan.FromSeconds(5);

        public ControlPanelUtility(IPAddress address)
        {
            IDevice device = DeviceFactory.Create(address);

            _phoenixNovaDevice = device as PhoenixNovaDevice;
        }

        private void NavigateToSetupNova()
        {
            _phoenixNovaDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);
            Thread.Sleep(_delay);

            do
            {
                _phoenixNovaDevice.ControlPanel.ScrollRight(1);
            }
            while (!_phoenixNovaDevice.ControlPanel.GetVirtualButtons().Any(x => x.Name == "cSetupHomeTouchButton"));
            _phoenixNovaDevice.ControlPanel.Press("cSetupHomeTouchButton");
            Thread.Sleep(_delay);
        }

        private bool NavigateToSetupMagicFrame()
        {
            _phoenixMagicFrameDevice.ControlPanel.PressKey(PhoenixSoftKey.Home);
            Thread.Sleep(_delay);

            var checkScreen = _phoenixMagicFrameDevice.ControlPanel.GetDisplayedStrings().First();
            Thread.Sleep(_delay);

            if (checkScreen == "Ready")
            {
                _phoenixMagicFrameDevice.ControlPanel.Press("SetupButton");
                Thread.Sleep(_delay);

                checkScreen = _phoenixMagicFrameDevice.ControlPanel.GetDisplayedStrings().First();
                Thread.Sleep(_delay);

                if (checkScreen == "Setup Menu")
                {
                    _phoenixMagicFrameDevice.ControlPanel.Press("cSystemSetup");
                    Thread.Sleep(_delay);
                    return true;
                }
            }
            return false;
        }

        public string GetWpsPin()
        {
            NavigateToSetupNova();

            do
            {
                _phoenixNovaDevice.ControlPanel.ScrollDown(50);
            }
            while (!_phoenixNovaDevice.ControlPanel.GetVirtualButtons().Any(x => x.Name == "cNetworkConfig"));

            _phoenixNovaDevice.ControlPanel.Press("cNetworkConfig");
            Thread.Sleep(_delay);
            _phoenixNovaDevice.ControlPanel.Press("cWirelessMenu");
            Thread.Sleep(_delay);
            _phoenixNovaDevice.ControlPanel.Press("cWirelessWPSSetup");
            Thread.Sleep(_delay);
            _phoenixNovaDevice.ControlPanel.Press("cWirelessGeneratePIN");
            IEnumerable<string> items = _phoenixNovaDevice.ControlPanel.GetDisplayedStrings();
            string pin = Regex.Match(items.First(x => x.Contains("PIN")), @"\d+").Value;
            return pin;
        }
    }
}
