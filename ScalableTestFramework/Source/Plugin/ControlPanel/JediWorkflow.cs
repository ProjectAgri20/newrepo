using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;

namespace HP.ScalableTest.Plugin.ControlPanel
{
    internal class JediWorkflow
    {
        private readonly JediWindjammerDevice _device;
        private readonly NetworkCredential _credential;
        private readonly Pacekeeper _pacekeeper = new Pacekeeper(TimeSpan.FromSeconds(1));
        private readonly string _oidPowerCycle = "1.3.6.1.2.1.43.5.1.1.3.1";

        public JediWorkflow(JediWindjammerDevice device, NetworkCredential credential)
        {
            _credential = credential;
            _device = device;
        }

        public void Authentication(string appName)
        {
            // Create Authenticator
            IAuthenticator auth = AuthenticatorFactory.Create(_device, _credential, AuthenticationProvider.Auto);

            try
            {
                var jediWindJammerPrepManager = new JediWindjammerPreparationManager(_device);
                jediWindJammerPrepManager.InitializeDevice(true);

                switch (appName)
                {
                    case "Sign In":
                        _device.ControlPanel.PressToNavigate("mSignInButton", "SignInForm", ignorePopups: true);

                        if (_device.ControlPanel.GetControls().Contains("m_RadioButton"))
                        {
                            //Using PressScreen since the small screen doesn't seem to like the mOKButton
                            _device.ControlPanel.PressScreen(250, 250);
                        }

                        auth.Authenticate();
                        break;

                    case "Follow-You Printing":
                    case "HP AC Secure Pull Print":
                    case "My workflow (FutureSmart)":
                    case "Pull Print":
                    case "Scan To Me":
                    case "Scan To My Files":
                    case "Scan To Folder":
                    case "Public Distributions":
                    case "Routing Sheet":
                    case "Personal Distributions":
                        _device.ControlPanel.ScrollPressWait("mAccessPointDisplay", "Title", appName, "SignInForm", StringMatch.StartsWith, TimeSpan.FromSeconds(60));
                        auth.Authenticate();
                        break;
                    default:
                        AuthenticationHelper.LaunchApp(_device, appName, auth);
                        break;
                }
            }
            catch (Exception ex)
            {
                var currentScreen = _device.ControlPanel.CurrentForm();
                string message = $"Unable to authenticate.  Verify EWS authentication configuration and that {appName} button invokes the intended auth method. (current screen: {currentScreen}";
                ExecutionServices.SystemTrace.LogDebug(message + $"; Exception: {ex.Message}");
                throw new DeviceInvalidOperationException(message + ")", ex);
            }
        }

        /// <summary>
        /// Cancel Print Job
        /// </summary>
        [Description("Cancels the current print job")]
        public void CancelPrintJob()
        {
            try
            {
                //Go Home
                _device.ControlPanel.PressKey(JediHardKey.Menu);

                Retry.WhileThrowing(() => _device.ControlPanel.WaitForForm("HomeScreenForm", true),
                                    5,
                                    TimeSpan.FromSeconds(2),
                                    new List<Type>() { typeof(Exception) });
                //Press Stop Button
                _device.ControlPanel.Press("JobStatus");

                if (_device.ControlPanel.CurrentForm() == "JobStatusMainForm")
                {
                    _device.ControlPanel.PressToNavigate("mCancelJobButton", "TwoButtonMessageBox", true);
                    _device.ControlPanel.PressToNavigate("m_OKButton", "JobStatusMainForm", true);
                    _device.ControlPanel.PressKey(JediHardKey.Menu);
                }
                else
                {
                    throw new DeviceWorkflowException($"Cancel Print Job Failed on screen: {_device.ControlPanel.CurrentForm()}");
                }
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Cancel Print Job failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Copy App Login with HPAC PIC Authentication
        /// </summary>
        public void CopyAppLoginWithHPACPicAuthentication()
        {
            try
            {
                _device.PowerManagement.Wake();
                //Go to Home Screen
                _device.ControlPanel.PressKey(JediHardKey.Menu);

                //Enter Credentials
                _device.ControlPanel.PressToNavigate("CopyApp", "SignInForm", true);
                // HPAC ID authenticiation uses a unique personal identification code (PIC) that has been assigned to each user.
                // Our convention is that it's the username with the 'u' lopped off
                // e.g. u00001  =>  00001; u00038 => 00038
                _device.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", _credential.UserName.Substring(1));//ID CODE is derived from UserName
                _device.ControlPanel.PressToNavigate("ok", "CopyAppMainForm", true);
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"HPAC navigation failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Login to Copy app using Safecom PIC Authentication method
        /// </summary>
        public void CopyAppLoginWithSafeComPicAuthentication()
        {
            try
            {
                _device.PowerManagement.Wake();
                //Go to Home Screen
                _device.ControlPanel.PressKey(JediHardKey.Menu);

                //Enter Credentials
                _device.ControlPanel.PressToNavigate("CopyApp", "SignInForm", true);
                // SafeCom ID authenticiation uses a unique personal identification code (PIC) that has been assigned to each user.
                // Our convention is that it's the username with the 'u' lopped off
                // e.g. u00001  =>  00001; u00038 => 00038
                _device.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", _credential.UserName.Substring(1));//ID CODE is derived from UserName
                _device.ControlPanel.PressToNavigate("ok", "CopyAppMainForm", true);
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Safecom navigation failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Login to Copy app using Safecom Windows Authentication method
        /// </summary>
        public void CopyAppLoginWithSafeComWindowsAuthentication()
        {
            try
            {
                _device.PowerManagement.Wake();
                //Go to Home Screen
                _device.ControlPanel.PressKey(JediHardKey.Menu);

                _device.ControlPanel.PressToNavigate("CopyApp", "SignInForm", true);
                //Enter UserName
                _device.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", _credential.UserName);
                _device.ControlPanel.Type(SpecialCharacter.Tab);
                //Enter Domain
                _device.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", _credential.Domain);
                _device.ControlPanel.Type(SpecialCharacter.Tab);
                //Enter Password
                _device.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", _credential.Password);
                _device.ControlPanel.PressToNavigate("ok", "CopyAppMainForm", true);
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Safecom navigation failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        ///  Trigger the Copy job from native app through glass Bed/ADF
        /// </summary>
        /// <param name="copies"></param>
        /// <param name="colorMode"></param>
        [Description("Performs a copy job through native app")]
        public void PerformCopyJob(int copies, string colorMode)
        {
            try
            {
                if ("CopyAppMainForm" != _device.ControlPanel.CurrentForm())
                {
                    _device.PowerManagement.Wake();
                    //Go to Home Screen
                    _device.ControlPanel.PressKey(JediHardKey.Menu);
                    //Enter Credentials
                    _device.ControlPanel.PressToNavigate("CopyApp", "CopyAppMainForm", true);
                }
                //Selecting the Color Mode for Copy Job
                if ("monochrome" == colorMode || "color" == colorMode)
                {
                    _device.ControlPanel.PressToNavigate("ColorMonoModeDialogButton", "ColorBlackDialog", true);
                    _device.ControlPanel.Press(colorMode);
                    _device.ControlPanel.Press("m_OKButton");
                }

                // select number of copies
                _device.ControlPanel.PressToNavigate("NumberOfCopies", "HPNumericKeypad", true);
                _device.ControlPanel.Type(copies.ToString());
                _device.ControlPanel.Press("mButtonOK");

                //start copy job
                _device.ControlPanel.Press("mStartButton");
                _device.ControlPanel.PressToNavigate("m_OKButton", "CopyAppMainForm", true);
                _device.ControlPanel.Press("mHomeButton");
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Safecom navigation failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Triggers the Wirless connection
        /// </summary>
        /// <param name="ssid"></param>
        /// <param name="networkPassword"></param>
        [Description("Enables Wirless connection")]
        public void EnableWirlessConnection(string ssid, string networkPassword)
        {
            string wirelessModeListitem = "0x6a_2";
            string modeListitem = "0xd_2";
            string ssidListitem = "0xe_2";
            string authenticationListitem = "0xf_2";
            string configurePskListitem = "0x10";
            string passPhraseListitem = "0x14_2";

            try
            {
                //Traversing to WirelessStation
                TraversingToWirelessStation();
                _device.ControlPanel.PressToNavigate(wirelessModeListitem, "IOMgrMenuDataSelection", true);
                _device.ControlPanel.Press("B/G/N MODE");
                _device.ControlPanel.Press("m_OKButton");

                _device.ControlPanel.PressToNavigate(modeListitem, "IOMgrMenuDataSelection", true);
                _device.ControlPanel.Press("INFRASTRUCTURE");
                _device.ControlPanel.Press("m_OKButton");

                //Go to the bottom of screen to click SSID Button
                while (!(_device.ControlPanel.WaitForControl(ssidListitem, TimeSpan.FromSeconds(1))))
                {
                    _device.ControlPanel.Press("IncrementButton");
                }
                //Entering the UserName
                _device.ControlPanel.PressToNavigate(ssidListitem, "IOMgrMenuDataUser", true);
                _device.ControlPanel.Type(ssid);
                _device.ControlPanel.Press("m_OKButton");

                //Go to the bottom of screen to click Authentication Button
                while (!(_device.ControlPanel.WaitForControl(authenticationListitem, TimeSpan.FromSeconds(1))))
                {
                    _device.ControlPanel.Press("IncrementButton");
                }
                _device.ControlPanel.PressToNavigate(authenticationListitem, "IOMgrMenuDataSelection", true);
                _device.ControlPanel.Press("WPA-PSK");
                _device.ControlPanel.Press("m_OKButton");

                //Go to the bottom of screen to clickConfigure PSK Button
                while (!(_device.ControlPanel.WaitForControl(configurePskListitem, TimeSpan.FromSeconds(1))))
                {
                    _device.ControlPanel.Press("IncrementButton");
                }
                _device.ControlPanel.Press(configurePskListitem);
                _device.ControlPanel.PressToNavigate(passPhraseListitem, "IOMgrMenuDataUser", true);
                //Entering the N NetworkPassword
                _device.ControlPanel.Type(networkPassword);
                _device.ControlPanel.Press("m_OKButton");
                _device.ControlPanel.Press("mHomeButton");
                _device.ControlPanel.Press("mResetButton");
                RestartDevice();
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Wirless Connection failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Triggers the Disabling of Wirless Connection
        /// </summary>
        [Description("Disabling Wirless Connection")]
        public void DisableWirlessConnection()
        {
            string wirelessModeListitem = "0x6a_2";
            string authenticationListitem = "0xf_2";

            try
            {
                //Traversing to WirelessStation
                TraversingToWirelessStation();
                _device.ControlPanel.PressToNavigate(wirelessModeListitem, "IOMgrMenuDataSelection", true);
                _device.ControlPanel.Press("B/G MODE");
                _device.ControlPanel.Press("m_OKButton");
                _device.ControlPanel.PressToNavigate(authenticationListitem, "IOMgrMenuDataSelection", true);
                _device.ControlPanel.Press("NO SECURITY");
                _device.ControlPanel.Press("m_OKButton");
                _device.ControlPanel.Press("mHomeButton");
                _device.ControlPanel.Press("mResetButton");
                RestartDevice();
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Disabling of Wirless Connection failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Sending Email.
        ///Assuming the From address is already set in EWS page of the Printer.
        /// </summary>
        /// <param name="toAddress">To Address</param>
        /// <param name="ccAddress">Cc Address</param>
        /// <param name="subject">Subject Line</param>
        /// <param name="filename">File Name</param>
        /// <param name="message">Email body Content</param>
        /// <param name="bccAddress">Bcc Address</param>
        [Description("Scan and send it as an attachement to an email address list")]
        public void SendEmail(string toAddress, string ccAddress, string subject, string filename, string message, string bccAddress)
        {
            try
            {
                _device.PowerManagement.Wake();
                //Go to Home Screen
                _device.ControlPanel.PressKey(JediHardKey.Menu);

                //Enter Credentials
                _device.ControlPanel.PressToNavigate("EmailApp", "EmailForm", true);
                // SafeCom ID authenticiation uses a unique personal identification code (PIC) that has been assigned to each user.  Our convention is that it's the username with the u lopped off
                // e.g. u00001  =>  00001; u00038 => 00038
                _device.ControlPanel.Press("mToTextBox");
                _device.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", toAddress);
                _device.ControlPanel.Press("mCCTextBox");
                _device.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", ccAddress);
                _device.ControlPanel.Press("mSubjectTextBox");
                _device.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", subject);
                _device.ControlPanel.Press("mFileNameTextBox");
                _device.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", filename);
                _device.ControlPanel.Press("mMessageTextBox");
                _device.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", message);
                _device.ControlPanel.Press("mBCCTextBox");
                _device.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", bccAddress);
                _device.ControlPanel.Press("mAppStartControl");
                _device.ControlPanel.PressToNavigate("mOkButton", "EmailForm", true);//Sign In
                _device.ControlPanel.Press("mHomeButton");
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Safecom navigation failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Sending Fax JOb from native app
        /// </summary>
        /// <param name="destinationNumber">Destination fax no</param>
        [Description("Sends Fax to the specified destination")]
        public void SendFax(int destinationNumber)
        {
            try
            {
                _device.PowerManagement.Wake();
                //Go to Home Screen
                _device.ControlPanel.PressKey(JediHardKey.Menu);
                //navigate to Fax App
                _device.ControlPanel.PressToNavigate("SendFaxApp", "SendFaxAppMainForm", true);
                _device.ControlPanel.PressToNavigate("mFaxNumberTextBox", "HPNumericKeypad", true);
                //Provide Destination Fax No
                _device.ControlPanel.Type(destinationNumber.ToString());
                _device.ControlPanel.PressToNavigate("mButtonOK", "SendFaxAppMainForm", true);
                //Trigger Fax Job
                _device.ControlPanel.Press("mStartButton");
                _device.ControlPanel.PressKey(JediHardKey.Menu);//Home Screen
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Safecom navigation failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Send to Network folder Job, Scans from ADF or Glass Bed
        /// </summary>
        /// <param name="networkFolder"> Network Path</param>
        /// <param name="networkUserName"> UserName</param>
        /// <param name="networkPassword"> Password</param>
        /// <param name="domain">Domain</param>
        [Description("Scans and saves it on a network folder")]
        public void SendSnf(string networkFolder, string networkUserName, string networkPassword, string domain)
        {
            try
            {
                _device.PowerManagement.Wake();
                //Go to Home Screen
                _device.ControlPanel.PressKey(JediHardKey.Menu);
                _device.ControlPanel.PressToNavigate("FolderApp", "FolderAppMainForm", true);
                //Click to Add network path
                _device.ControlPanel.PressToNavigate("mAddPathButton", "FolderKeyboardForm", true);
                _device.ControlPanel.Type(networkFolder);
                _device.ControlPanel.PressToNavigate("ok", "SignInForm", true);
                //Enter UserName for Network Path
                _device.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", networkUserName);
                _device.ControlPanel.Type(SpecialCharacter.Tab);
                //Enter Domain credential for Network Path
                _device.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", networkPassword);
                _device.ControlPanel.Type(SpecialCharacter.Tab);
                //Enter Password for Network Path
                _device.ControlPanel.TypeOnVirtualKeyboard("mKeyboard", domain);
                _device.ControlPanel.PressToNavigate("ok", "FolderAppMainForm", true);
                //Trigger SNF Job
                _device.ControlPanel.Press("mAppStartControl");
                _device.ControlPanel.PressKey(JediHardKey.Menu);//Home Screen
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Safecom navigation failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Select Language in Control Panel
        /// </summary>
        /// <param name="language"></param>
        [Description("Select Language in Control Panel")]
        public void SelectLanguage(Languages language)
        {
            try
            {
                string code = Properties.JediResources.ResourceManager.GetString(language.ToString());
                _pacekeeper.Pause();
                _device.ControlPanel.PressKey(JediHardKey.Menu);
                _pacekeeper.Pause();
                _device.ControlPanel.PressToNavigate("mLanguageSelectButton", "QuickLanguageSelectionDialog", true);
                _pacekeeper.Pause();
                _device.ControlPanel.Press(code);
                _pacekeeper.Pause();
                _device.ControlPanel.Press("m_OKButton");
                _pacekeeper.Pause();
                _device.ControlPanel.PressKey(JediHardKey.Menu);

            }
            catch (Exception e)
            {
                throw new DeviceWorkflowException($"Selecting language failed with exception:{e.Message}");

            }
            
        }

        public enum Languages
        {
            Arabic, Catalan, Dansk, Deutsch, English, Espanol, French, Hrvatski, Indonesia, Italiano, Japanese, Magyar, Nederlands, Norsk, Romana, SimpleChinese, Slovancina, Slovanscina, Suomi, Svenska, TraditionalChinese, Portugues, Turkey, Polski, Cestina
        };

        /// <summary>
        /// ControlPanel SignOut
        /// </summary>
        [Description("Signs out the current signed in user")]
        public void SignOut()
        {
            //Press home sceeen
            _device.ControlPanel.PressKey(JediHardKey.Menu);
            _pacekeeper.Pause();

            //Press on Signout
            _device.ControlPanel.Press("mSignInButton");
            _pacekeeper.Pause();
            //Signout functionality
        }

        /// <summary>
        ///Enabling DHCP
        /// </summary>
        [Description("Enabling DHCP Mode")]
        public void EnablingDHCPMode()
        {
            string wirelessMenuListitem = "JetDirectMenu2";
            string _tcpIP = "0x4";
            string _ipv4Settings = "0x20";
            string _configMode = "0x25_2";
            try
            {
                if ("AdminAppMainForm" != _device.ControlPanel.CurrentForm())
                {
                    _device.PowerManagement.Wake();
                    //Go to Home Screen
                    _device.ControlPanel.PressKey(JediHardKey.Menu);
                    //Go to Administration
                    _device.ControlPanel.PressToNavigate("AdminApp", "AdminAppMainForm", true);
                }

                //Go to the bottom of screen to click Network Settings Button
                while (!(_device.ControlPanel.WaitForControl("NetworkingAndIOMenu", TimeSpan.FromSeconds(1))))
                {
                    _device.ControlPanel.Press("IncrementButton");
                }

                //Travesring through the Administration App
                _device.ControlPanel.Press("NetworkingAndIOMenu");
                _device.ControlPanel.Press(wirelessMenuListitem);
                _device.ControlPanel.Press(_tcpIP);
                _device.ControlPanel.Press(_ipv4Settings);
                _device.ControlPanel.PressToNavigate(_configMode, "IOMgrMenuDataSelection", true);
                _device.ControlPanel.Press("DHCP");
                _device.ControlPanel.Press("m_OKButton");
                _device.ControlPanel.PressKey(JediHardKey.Menu);
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Enabling DHCP Mode failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// USB Firmware Upgrade
        /// </summary>
        [Description("Upgrades Firmware from a bundle stored on USB")]
        public void UsbFirmwareUpgrade()
        {
            try
            {
                _device.PowerManagement.Wake();
                //Go to Home Screen
                _device.ControlPanel.PressKey(JediHardKey.Menu);
                _pacekeeper.Pause();

                _device.ControlPanel.PressWait("ServiceabilityApp", "ServiceabilityAppMainForm");
                _pacekeeper.Pause();

                _device.ControlPanel.PressWait("CurrentFirmwareVersion", "UsbFirmwareUpgradeSettings");
                _pacekeeper.Pause();

                if (_device.ControlPanel.CurrentForm().Equals("OneButtonMessageBox"))
                {
                    throw new DeviceWorkflowException("Activity Failed, Please Insert USB");
                }

                _device.ControlPanel.Press("m_OKButton");
                _pacekeeper.Pause();

                if (_device.ControlPanel.CurrentForm().Equals("TwoButtonMessageBox"))
                {
                    _device.ControlPanel.Press("m_OKButton");
                    _pacekeeper.Pause();
                }
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"USB Firmware Upgrade failed with exception:{ex.Message}");
            }
        }

        private void TraversingToWirelessStation()
        {
            string wirelessMenuListitem = "JetDirectMenu2";
            string wirelessStationListitem = "0x3";
            try
            {
                _device.PowerManagement.Wake();
                if ("AdminAppMainForm" != _device.ControlPanel.CurrentForm())
                {
                    //Go to Home Screen
                    _device.ControlPanel.PressKey(JediHardKey.Menu);
                    //Go to Administration
                    _device.ControlPanel.PressToNavigate("AdminApp", "AdminAppMainForm", true);
                }
                //Go to the bottom of screen to click Network Settings Button
                while (!(_device.ControlPanel.WaitForControl("NetworkingAndIOMenu", TimeSpan.FromSeconds(1))))
                {
                    _device.ControlPanel.Press("IncrementButton");
                }

                //Travesring through the Administration App
                _device.ControlPanel.Press("NetworkingAndIOMenu");
                _device.ControlPanel.Press(wirelessMenuListitem);
                _device.ControlPanel.Press(wirelessStationListitem);
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException(string.Format("Traversing To Wireless Station failed with exception:{0}", ex.Message));
            }
        }

        /// <summary>
        /// Restarts the device
        /// </summary>
        [Description("Restarts the device")]
        public void RestartDevice()
        {
            try
            {
                _device.Snmp.Set(_oidPowerCycle, 6);
            }
            catch (SnmpException ex)
            {
                throw new SnmpException("Not able to restart printer", ex);
            }
        }
    }
}
