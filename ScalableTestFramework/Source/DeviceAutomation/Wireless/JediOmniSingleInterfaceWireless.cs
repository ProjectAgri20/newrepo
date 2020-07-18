using HP.DeviceAutomation.Jedi;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace HP.ScalableTest.DeviceAutomation.Wireless
{
    class JediOmniSingleInterfaceWireless : DeviceWorkflowLogSource, IWireless
    {
        JediOmniControlPanel _controlPanel;

        private JediOmniSingleInterfaceWireless(JediOmniDevice device)
        {
            _controlPanel = device.ControlPanel;
        }

        public bool ConfigureWirelessStation(string ssid, WirelessModes wirelessMode, WirelessStaModes mode, WirelessBands band, WirelessAuthentications auth, string passphrase, int keyIndex)
        {
            NavigateToWireless();

            // Wireless mode
            _controlPanel.PressWait(GetControlId("WIRELESS STATION", "li"), "#hpid-settings-app-menu-panel .hp-listitem-text:contains(Status)");


            List<string> controls = _controlPanel.GetIds("li", OmniIdCollectionType.Self).ToList();

            // Status
            _controlPanel.PressWait(GetControlId("Status", controls: controls), "#0");
            _controlPanel.Press($"#0");
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
            NavigateToWireless();

            _controlPanel.PressWait(GetControlId("WIRELESS STATION", "li"), "#hpid-settings-app-menu-panel .hp-listitem-text:contains(Status)");
            string control = GetControlId("WI-FI", "li");
            _controlPanel.PressWait(control, "#0");
            _controlPanel.PressWait("#1", "#hpid-ok-setting-button");
            _controlPanel.PressWait("#hpid-ok-setting-button", "#hpid-button-Ok");
            string val = _controlPanel.GetValue(".hp-popup-content", "innerText", OmniPropertyType.Property);
            string pin = Regex.Match(val, @"\d+").Value;
            return pin;
        }

        public void ConfigureWpsPin()
        {
            _controlPanel.Press("#hpid-button-Ok");
            // A delay is required here for successful configuration.
            Thread.Sleep(TimeSpan.FromSeconds(30));
            NavigateHome();
        }

        public bool ConfigureWpsPush()
        {
            NavigateToWireless();
            _controlPanel.PressWait(GetControlId("WIRELESS STATION", "li"), "#hpid-settings-app-menu-panel .hp-listitem-text:contains(Status)");

            string control = GetControlId("WI-FI", "li");

            if (string.IsNullOrEmpty(control))
            {
                // If Wi-Fi Push control is not available, enable wireless and try again
                _controlPanel.PressWait(GetControlId("Status", "li"), "#0");
                _controlPanel.Press("#0");
                _controlPanel.PressWait("#hpid-ok-setting-button", control);
                control = GetControlId("WI-FI", "li");
            }

            if (string.IsNullOrEmpty(control))
            {
                throw new NotSupportedException("Wi-Fi push push option is not available on control panel.");
            }

            _controlPanel.PressWait(control, "#0");
            _controlPanel.Press("#0");
            _controlPanel.PressWait("#hpid-ok-setting-button", "#hpid-button-Ok");
            _controlPanel.PressWait("#hpid-button-Ok", control);

            NavigateHome();

            return true;
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
            _controlPanel.PressWait("#hpid-tree-node-listitem-networkingandiomenu", "#hpid-settings-app-menu-panel .hp-listitem-text:contains(Wireless)");
            _controlPanel.PressWait(GetControlId("Wireless", "li"), "#hpid-settings-app-menu-panel .hp-listitem-text:contains(INFORMATION)");
        }

        /// <summary>
        /// Navigate to home.
        /// </summary>
        private void NavigateHome()
        {
            while (_controlPanel.WaitForState(".hp-header-levelup-button:last", OmniElementState.Useable, TimeSpan.FromSeconds(10)))
            {
                _controlPanel.Press(".hp-header-levelup-button:last");
            }

            _controlPanel.Press(".hp-button-back:last");

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

            _controlPanel.WaitForState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(5));
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
            if (controls == null)
                controls = _controlPanel.GetIds(elementSelector, OmniIdCollectionType.Self).ToList();

            string id = $"#{controls.FirstOrDefault(x => _controlPanel.GetValue($"#{x}", "innerText", OmniPropertyType.Property).StartsWith(identifier))}";
            return id;
        }
    }
}
