using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.DeviceAutomation.Wireless
{
    class JediOmniMultipleInterfaceWireless : DeviceWorkflowLogSource, IWireless
    {
        JediOmniControlPanel _controlPanel;
        JediOmniNotificationPanel _notificationPanel;

        public JediOmniMultipleInterfaceWireless(JediOmniDevice device)
        {
            _controlPanel = device.ControlPanel;
            _notificationPanel = new JediOmniNotificationPanel(device);
        }

        public bool ConfigureWirelessStation(string ssid, WirelessModes wirelessMode, WirelessStaModes mode, WirelessBands band, WirelessAuthentications auth, string passphrase, int keyIndex)
        {
            NavigateToWireless();

            // Wireless mode
            _controlPanel.PressWait(GetControlId("WIRELESS STATION", "li"), "#hpid-settings-app-menu-panel .hp-listitem-text:contains(WIRELESS MODE)");


            List<string> controls = _controlPanel.GetIds("li", OmniIdCollectionType.Self).ToList();

            // Wireless Mode
            _controlPanel.PressWait(GetControlId("WIRELESS MODE", controls: controls), "#0");
            _controlPanel.Press($"#{(int)wirelessMode}");
            _controlPanel.PressWait("#hpid-ok-setting-button", GetControlId("MODE", controls: controls));

            // Mode
            _controlPanel.PressWait(GetControlId("MODE", controls: controls), "#0");
            _controlPanel.Press($"#{(int)mode}");
            _controlPanel.PressWait("#hpid-ok-setting-button", GetControlId("SSID", controls: controls));

            // Enter ssid
            _controlPanel.PressWait(GetControlId("SSID", controls: controls), "#hpid-dynamic-setting-panel .hp-dynamic-text-box:contains()");

            _controlPanel.PressWait($"#hpid-dynamic-setting-panel .hp-dynamic-text-box", "#hpid-keyboard");
            _controlPanel.TypeOnVirtualKeyboard(ssid);
            _controlPanel.PressWait("#hpid-keyboard-key-done", "#hpid-ok-setting-button");
            _controlPanel.PressWait("#hpid-ok-setting-button", GetControlId("AUTHENTICATION", controls: controls));

            // Authentication
            _controlPanel.PressWait(GetControlId("AUTHENTICATION", controls: controls), "#0");
            _controlPanel.Press($"#{(int)auth}");
            _controlPanel.PressWait("#hpid-ok-setting-button", GetControlId("AUTHENTICATION", controls: controls));

            NavigateHome();
            return true;
        }

        public string GenerateWpsPin()
        {
            throw new NotImplementedException();
        }

        public bool ConfigureWpsPush()
        {
            throw new NotImplementedException();
        }

        public bool ConfigureWifiDirect(WiFiDirectConnectionMode mode, string password)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Navigate to the wireless page.
        /// </summary>
        private void NavigateToWireless()
        {
            _controlPanel.PressHome();
            _controlPanel.ScrollToItem("#hpid-settings-homescreen-button");
            _controlPanel.PressWait("#hpid-settings-homescreen-button", "#hpid-tree-node-listitem-networkingandiomenu");
            _controlPanel.PressWait("#hpid-tree-node-listitem-networkingandiomenu", "#hpid-settings-app-menu-panel .hp-listitem-text:contains(WIRELESS MENU)");
            _controlPanel.PressWait(GetControlId("WIRELESS MENU", "li"), "#hpid-settings-app-menu-panel .hp-listitem-text:contains(INFORMATION)");
        }

        /// <summary>
        /// Navigate to home.
        /// </summary>
        private void NavigateHome()
        {
            while (_controlPanel.WaitForState(".hp-button-back:last", OmniElementState.Useable, TimeSpan.FromSeconds(10)))
            {
                _controlPanel.Press(".hp-button-back:last");
            }

            var controls = _controlPanel.GetIds("div", OmniIdCollectionType.Children);
            if (controls.Contains("hpid-button-reset"))
            {
                if (_controlPanel.WaitForState("#hpid-button-reset", OmniElementState.Useable, TimeSpan.FromSeconds(10)))
                {
                    _controlPanel.Press("#hpid-button-reset");
                }
                else
                {
                    string value = _controlPanel.GetValue("#hp-button-signin-or-signout", "innerText", OmniPropertyType.Property).Trim();
                    if (value.Contains("Sign Out"))
                    {
                        if (_controlPanel.WaitForState("#hp-button-signin-or-signout", OmniElementState.Useable, TimeSpan.FromSeconds(10)))
                        {
                            _controlPanel.Press("#hp-button-signin-or-signout");
                        }
                    }
                }
            }

            _controlPanel.PressHome();

            if (!_controlPanel.WaitForState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(5)))
            {
                throw new DeviceWorkflowException("Home Page is not accessible");
            }
            bool success = _notificationPanel.WaitForNotDisplaying("Initializing scanner", "Clearing settings");
            if (!success)
            {
                throw new TimeoutException($"Timed out waiting for notification panel state: Contains Initializing scanner, Clearing settings");
            }
        }

        /// <summary>
        /// Gets the dynamic control id
        /// </summary>
        /// <param name="identifier">The identifier usually the inner text for the element</param>
        /// <param name="elementSelector">The element selector</param>
        /// <param name="controls"></param>
        /// <returns></returns>
        private string GetControlId(string identifier, string elementSelector = "", List<string> controls = null)
        {
            if(controls == null)
                controls = _controlPanel.GetIds(elementSelector, OmniIdCollectionType.Self).ToList();

            string id = $"#{controls.FirstOrDefault(x => _controlPanel.GetValue($"#{x}", "innerText", OmniPropertyType.Property).StartsWith(identifier))}";
            return id;
        }

        public void ConfigureWpsPin()
        {
            throw new NotImplementedException();
        }
    }
}
