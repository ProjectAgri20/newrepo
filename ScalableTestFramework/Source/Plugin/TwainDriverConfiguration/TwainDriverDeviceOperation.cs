using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using System;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;

namespace HP.ScalableTest.Plugin.TwainDriverConfiguration
{
    /// <summary>
    /// Implementation of <see cref="TwainDriverDeviceOperation" /> for a <see cref="JediOmniDevice" />.
    /// </summary>
    public static class TwainDriverDeviceOperation
    {
        private static readonly TimeSpan ShortScreenWaitTimeout = new TimeSpan(0, 0, 0, 5);
        private static readonly TimeSpan ScreenWaitTimeout = new TimeSpan(0, 0, 0, 10);

        /// <summary>
        /// Device side operation for Twain Scan
        /// </summary>
        /// <param name="twainDriver"></param>
        /// <param name="deviceDetails"></param>      
        public static void ExecuteJob(IDevice deviceDetails, TwainDriverActivityData twainDriver)
        {
            JediOmniDevice device = (JediOmniDevice)deviceDetails;
            IDevicePreparationManager preparationManager = DevicePreparationManagerFactory.Create(device);
            preparationManager.WakeDevice();
            //Checking For Home Screen
            if (!device.ControlPanel.WaitForState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(1)))
            {
                preparationManager.NavigateHome();
            }

            device.ControlPanel.ScrollPress("#hpid-scan-homescreen-button");
            if (device.ControlPanel.WaitForState("#hpid-remoteScanRequest-homescreen-button", OmniElementState.Exists))
            {
                device.ControlPanel.ScrollToItem("#hpid-remoteScanRequest-homescreen-button");
                device.ControlPanel.PressWait("#hpid-remoteScanRequest-homescreen-button", "#hpid-remote-scan-request-landing-page");
                if (device.ControlPanel.WaitForState("#hpid-remote-scan-request-job-list-item", OmniElementState.Exists))
                {
                    if (device.ControlPanel.WaitForState("#hpid-remote-scan-request-job-list-item", OmniElementState.Useable, ScreenWaitTimeout))
                    {
                        device.ControlPanel.Press("#hpid-remote-scan-request-job-list-item:first");
                    }
                    else
                    {
                        device.ControlPanel.Press("#hpid-remote-scan-request-unlock-private-jobs:first");
                        if (device.ControlPanel.CheckState("#undefined", OmniElementState.Exists))
                        {
                            device.ControlPanel.PressWait("#hpid-pin-to-unlock-job-textbox", "#hpid-keyboard", ShortScreenWaitTimeout);
                            device.ControlPanel.TypeOnVirtualKeyboard(twainDriver.Pin);
                            //Check for 4.3 inch control panel where the keypad/keyboard hides the 'OK' button after entering the Pin/Password.
                            //Close the numeric keypad after PIN is entered
                            if (device.ControlPanel.CheckState("#hpid-keypad-key-close", OmniElementState.Exists))
                            {
                                device.ControlPanel.Press("#hpid-keypad-key-close");
                            }
                            //Close the virtual keyboard after PIN/Password is entered
                            else if (device.ControlPanel.CheckState("#hpid-keyboard-key-done", OmniElementState.Exists))
                            {
                                device.ControlPanel.Press("#hpid-keyboard-key-done");
                            }
                            device.ControlPanel.Press("#hpid-button-ok");
                            //Automatic selection of job was not happening when reservation was enabled and a password is set for Remote Scan Request using Twain driver in the previous device firmware.Now the issue is fixed in device firmware 2458466 and above 
                            //Hence commenting the below line of code
                            //device.ControlPanel.Press("#hpid-remote-scan-request-job-list-item:first");

                            if (device.ControlPanel.CheckState(".hp-popup-modal-overlay", OmniElementState.Exists))
                            {
                                throw new DeviceWorkflowException("Invalid Pin/Pin Not Entered");
                            }
                        }
                    }

                    if (device.ControlPanel.WaitForState("#hpid-button-remote-scan-request-start", OmniElementState.Useable, ScreenWaitTimeout))
                    {
                        device.ControlPanel.Press("#hpid-button-remote-scan-request-start");
                        if (device.ControlPanel.WaitForState("#hpid-button-ok", OmniElementState.Exists, ShortScreenWaitTimeout))
                        {
                            device.ControlPanel.Press("#hpid-button-ok");
                            device.ControlPanel.Press("#hpid-button-remote-scan-request-start");
                        }
                    }
                }
                else
                {
                    throw new DeviceWorkflowException("No Remote scan jobs found.");
                }
            }
            else
            {
                throw new DeviceWorkflowException("Remote Scan Request was not found in the scan folder.");
            }

            if (device.ControlPanel.WaitForState("#hpid-message-center-screen", OmniElementState.Exists, ShortScreenWaitTimeout))
            {
                throw new DeviceWorkflowException("There is an error displayed on the device, Please check the device and try again");
            }
        }
    }
}
