using System;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    public static class JediOmniDeviceHelper
    {
        private const string _topViewAttribute = "[hp-global-top-view=true]";
        private const string _homeScreenLogo = "#hpid-homescreen-logo-icon";

        public static void WakeDevice(JediOmniDevice device)
        {
            const string _sleepOverlay = "#hpid-sleep-modal-overlay";
            device.PowerManagement.Wake();

            // The WaitForState call is necessary for Omni because a div overlays the screen during sleep, making the control panel unusable until device wakes enough to remove it.  
            // To simulate UX more closely, button presses ultimately resolve to X-Y coordinate presses on top-level control visible rather than direct javascript 
            // manipulation of the element.  Therefore prematurely trying to press an element will result in an exception.
            device.ControlPanel.WaitForState(_sleepOverlay, OmniElementState.Exists, false);
        }

        public static bool IsOnHomeScreen(JediOmniDevice device)
        {
            return device.ControlPanel.CheckState($".hp-homescreen-folder-view{_topViewAttribute}", OmniElementState.Exists)
                && device.ControlPanel.CheckState(_homeScreenLogo, OmniElementState.VisibleCompletely);
        }

        public static void WaitForHome(JediOmniDevice device, TimeSpan timeout)
        {
            bool success =
                Wait.ForTrue(
                    () =>
                    {
                        if (device.PowerManagement.GetPowerState() == PowerState.Sleep)
                        {
                            WakeDevice(device);
                            Thread.Sleep(500);
                        }
                        return IsOnHomeScreen(device);
                    }
                  , timeout
                  , TimeSpan.FromSeconds(1));

            if (!success)
            {
                throw new DeviceInvalidOperationException($"Device did not navigate home within timeout. (Address: {device.Address}; Timeout: {timeout.ToString()})");
            }
        }

        public static bool WaitForValue(JediOmniDevice device, string elementSelector, string propertyName, OmniPropertyType propertyType, string expectedPropertyValue, StringMatch match, StringComparison comparison, TimeSpan waitTimeForElement, TimeSpan waitTimeForProperty)
        {
            DateTime endTime = DateTime.Now + waitTimeForElement;
            string actualPropertyValue = null;
            bool elementExists = false;

            do
            {
                if (device.ControlPanel.CheckState(elementSelector, OmniElementState.Exists))
                {
                    if (!elementExists)
                    {
                        endTime = DateTime.Now + waitTimeForProperty;
                        elementExists = true;
                    }

                    actualPropertyValue = device.ControlPanel.GetValue(elementSelector, propertyName, propertyType);

                    if (StringMatcher.IsMatch(expectedPropertyValue, actualPropertyValue, match, comparison))
                    {
                        return true;
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            while (DateTime.Now < endTime);

            return false;
        }
    }
}