using System;
using System.Net;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;

namespace HP.ScalableTest.Plugin.PSPullPrint
{
    /// <summary>
    /// Class to perform Safecom Operations on Jedi devices
    /// </summary>
    internal class SafecomJedi
    {
        private readonly JediWindjammerDevice _device;
        private readonly JediWindjammerControlPanel _controlPanel;
        private readonly NetworkCredential _credential;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="device"></param>
        /// <param name="credential"></param>
        public SafecomJedi(IDevice device, NetworkCredential credential)
        {
            //Getting the Sirius Device
            _device = device as JediWindjammerDevice;
            _controlPanel = _device.ControlPanel;
            _credential = credential;
        }

        /// <summary>
        /// Logs in to the Safecom on the device control panel using ID CODE
        /// </summary>     
        /// <param name="pin">PIN</param>
        /// <returns></returns>
        public void AuthenticateSafecom(string pin)
        {
            try
            {
                var preparationManager = DevicePreparationManagerFactory.Create(_device);
                preparationManager.WakeDevice();

                //Go to Home Screen
                _controlPanel.PressKey(JediHardKey.Menu);

                //Enter Credentials
                _controlPanel.PressToNavigate("b7396880-ec17-11df-98cf-0800200c9a66", "SignInForm", true);
                _controlPanel.PressToNavigate("m_textBox", "SignInKeyboardForm", true);
                // SafeCom ID authenticiation uses a unique personal identification code (PIC) that has been assigned to each user.  Our convention is that it's the username with the u lopped off
                // e.g. u00001  =>  00001; u00038 => 00038
                _controlPanel.TypeOnVirtualKeyboard("mKeyboard", _credential.UserName.Substring(1));//Enter Username as ID CODE
                _controlPanel.PressToNavigate("ok", "SignInForm", true);
                _controlPanel.PressToNavigate("mOkButton", "OxpUIAppMainForm800X300", true);//Sign In
            }
            catch (Exception ex)
            {
                throw new Exception($"Safecom navigation failed with exception:{ex.Message}");
            }
        }
    }
}
