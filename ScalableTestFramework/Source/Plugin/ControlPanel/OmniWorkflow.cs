using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.ScalableTest.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml.Linq;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ControlPanel
{
    internal class OmniWorkflow
    {
        private readonly JediOmniDevice _device;
        private readonly NetworkCredential _credential;
        private readonly Pacekeeper _pacekeeper = new Pacekeeper(TimeSpan.FromSeconds(2));
        private readonly TimeSpan _waitTimeSpan = TimeSpan.FromSeconds(10);
        private readonly string _oidPowerCycle = "1.3.6.1.2.1.43.5.1.1.3.1";
        private JediOmniPreparationManager _preparationManager;
        internal event EventHandler<StatusChangedEventArgs> StatusUpdate;
        internal event EventHandler<ScreenCaptureEventArgs> ScreenCapture; 
        public OmniWorkflow(JediOmniDevice device, NetworkCredential credential)
        {
            _credential = credential;
            _device = device;
        }

        /// <summary>
        /// performs authentication for the specified app
        /// </summary>
        /// <param name="appName"></param>
        [Description("Performs Authentication for the specified app")]
        public void Authentication(string appName)
        {
            // Create Authenticator
            IAuthenticator auth = AuthenticatorFactory.Create(_device, _credential, AuthenticationProvider.Auto);

            try
            {
                new JediOmniPreparationManager(_device).InitializeDevice(true);

                switch (appName)
                {
                    case "Sign In":
                        string value = _device.ControlPanel.GetValue("#hp-button-signin-or-signout", "innerText", OmniPropertyType.Property).Trim();
                        if (value.Contains("Sign In"))
                        {
                            _device.ControlPanel.PressWait("#hp-button-signin-or-signout", "#hpid-dropdown-agent", TimeSpan.FromSeconds(10));
                            Thread.Sleep(1000);
                            if (_device.ControlPanel.CheckState("#hpid-dropdown-agent", OmniElementState.Hidden))
                            {
                                _device.ControlPanel.PressScreen(10, 10);
                            }
                        }
                        auth.Authenticate();
                        break;

                    case "HP AC Secure Pull Print":
                    case "My workflow (FutureSmart)":
                    case "Pull Print":
                    case "Follow-You Printing":
                    case "Scan To Me":
                    case "Scan To My Files":
                    case "Scan To Folder":
                    case "Public Distributions":
                    case "Routing Sheet":
                    case "Personal Distributions":
                        string elementId = ".hp-homescreen-button:contains(" + appName + "):first";
                        _device.ControlPanel.PressWait(elementId, "#hpid-signin-app-screen");
                        auth.Authenticate();
                        break;
                    default:
                        AuthenticationHelper.LaunchApp(_device, appName, auth);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExecutionServices.SystemTrace.LogDebug(ex.ToString());
                throw new DeviceInvalidOperationException($"Unable to authenticate: {ex.Message}", ex);
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
                _device.ControlPanel.SignalUserActivity();
                if (_device.ControlPanel.WaitForState(".hp-button-active-jobs:last", OmniElementState.Useable, _waitTimeSpan))
                {
                    // Press Stop Button from home screen
                    _device.ControlPanel.ScrollPressWait(".hp-button-active-jobs:last", "#hpid-active-jobs-screen");

                    // Click Cancel Current Job
                    if (_device.ControlPanel.WaitForState("#hpid-button-cancel-job", OmniElementState.Useable, _waitTimeSpan))
                        _device.ControlPanel.Press("#hpid-button-cancel-job");
                    else
                    {
                        UpdateScreenShot();
                        throw new DeviceWorkflowException("No jobs to cancel");
                    }

                    // Click yes on the Pop Up.
                    if (_device.ControlPanel.CheckState(".hp-popup-modal-overlay", OmniElementState.VisibleCompletely))
                    {
                        _device.ControlPanel.Press("#hpid-button-yes");
                    }

                    // Exit to Home Screen
                    if (_device.ControlPanel.CheckState("#hpid-active-jobs-exit", OmniElementState.Useable))
                    {
                        _device.ControlPanel.PressWait("#hpid-active-jobs-exit", "#hpid-pause-resume-popup");
                    }
                    if (_device.ControlPanel.WaitForState("#hpid-button-resume", OmniElementState.Useable, _waitTimeSpan))
                    {
                        _device.ControlPanel.Press("#hpid-button-resume");
                    }
                }
                else
                {
                    throw new DeviceWorkflowException("Unable to enter Jog Log to cancel the job");
                }
            }
            catch (Exception ex)
            {
                if (_device.ControlPanel.WaitForState("#hpid-message-center-screen", OmniElementState.Exists))
                {
                    throw new DeviceWorkflowException($"There is an error displayed on the device, Please check the device and try again. Inner Exception : { (object)ex.Message}");
                }
                else
                {
                    throw new DeviceWorkflowException($"Unable to cancel the print job, the button is not available. Inner Exception : {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Copy App Login with HPAC PIC Authentication
        /// </summary>
        [Description("Performs Sign in for Copy with HPAC PIC Authentication")]
        public void CopyAppLoginWithHPACPicAuthentication()
        {
            _device.ControlPanel.SignalUserActivity();
            //Checking For Home Screen
            if (_device.ControlPanel.CheckState("#hpid-copy-homescreen-button", OmniElementState.Exists))
            {
                //Click Copy App
                _device.ControlPanel.ScrollPressWait("#hpid-copy-homescreen-button", "hpid-signin-app-screen", _waitTimeSpan);

                _device.ControlPanel.WaitForState("#hpid-keyboard", OmniElementState.Useable, _waitTimeSpan);
                // HPAC IRM authentication uses a unique personal identification code (PIC)
                // that has been assigned to each user.  Our convention is that it's the username with the u lopped off
                // e.g. u00001  =>  00001; u00038 => 00038
                string pic = _credential.UserName.Substring(1);
                _device.ControlPanel.TypeOnVirtualKeyboard(pic);
                Thread.Sleep(10);
                _device.ControlPanel.Press("#hpid-keyboard-key-done");
                Thread.Sleep(10);
            }
            else
            {
                throw new DeviceWorkflowException("Please return to home page");
            }
        }

        /// <summary>
        /// Login to Copy app using Safecom PIC Authentication method
        /// </summary>
        [Description("Performs Sign in for Copy with Safecom PIC Authentication")]
        public void CopyAppLoginWithSafeComPicAuthentication()
        {
            try
            {
                _device.ControlPanel.SignalUserActivity();
                //Click Copy App
                _device.ControlPanel.ScrollPressWait("#hpid-copy-homescreen-button", "#hpid-keyboard", _waitTimeSpan);
                // SafeCom ID authenticiation uses a unique personal identification code (PIC) that has been assigned to each user.
                // e.g. 00001  =>  00001; 00038 => 00038
                _device.ControlPanel.TypeOnVirtualKeyboard(_credential.UserName);
                _pacekeeper.Pause();
                //Click on SignIn
                _device.ControlPanel.PressWait("#hpid-keyboard-key-done", "#hpid-copy-app-screen", _waitTimeSpan);
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Copy login failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Login to Copy app using Safecom Windows Authentication method
        /// </summary>
        [Description("Performs Sign in for Copy with Windows Authentication")]
        public void CopyAppLoginWithSafeComWindowsAuthentication()
        {
            try
            {
                _device.ControlPanel.SignalUserActivity();
                //Click copy App
                _device.ControlPanel.ScrollPressWait("#hpid-copy-homescreen-button", "#hpid-keyboard", _waitTimeSpan);
                //Enter UserName
                _device.ControlPanel.TypeOnVirtualKeyboard(_credential.UserName);
                _device.ControlPanel.Type(SpecialCharacter.Tab);
                _pacekeeper.Pause();
                //Enter Domain
                _device.ControlPanel.TypeOnVirtualKeyboard(_credential.Domain);
                _device.ControlPanel.Type(SpecialCharacter.Tab);
                _pacekeeper.Pause();
                //Enter Password
                _device.ControlPanel.TypeOnVirtualKeyboard(_credential.Password);
                _pacekeeper.Pause();
                //Click SignIn
                _device.ControlPanel.PressWait("#hpid-keyboard-key-done", "#hpid-copy-app-screen", _waitTimeSpan);
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Copy login failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Fax  Native App Login with SafeCom PIC Authentication
        /// </summary>
        [Description("Performs Sign in for Fax with Safecom PIC Authentication")]
        public void FaxAppLoginWithSafeComPicAuthentication()
        {
            try
            {
                _device.ControlPanel.SignalUserActivity();
                //Click Scan App
                _device.ControlPanel.ScrollPressWait("#hpid-scan-homescreen-button", "#hpid-keyboard", _waitTimeSpan);
                //Click Scan to Fax
                _device.ControlPanel.PressWait("#hpid-sendFax-homescreen-button", "#hpid-keyboard", _waitTimeSpan);
                // SafeCom ID authenticiation uses a unique personal identification code (PIC) that has been assigned to each user.
                // e.g. 00001  =>  00001; 00038 => 00038
                _device.ControlPanel.TypeOnVirtualKeyboard(_credential.UserName);
                _pacekeeper.Pause();
                //Click on SignIn
                _device.ControlPanel.PressWait("#hpid-keyboard-key-done", "#hpid-sendfax-app-screen", _waitTimeSpan);
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Fax login failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Fax Native App Login with SafeCom Windows Authentication(Provided on Safecom Server not through EWS)
        /// </summary>
        [Description("Performs Sign in for Fax with Windows Authentication")]
        public void FaxAppLoginWithSafeComWindowsAuthentication()
        {
            try
            {
                _device.ControlPanel.SignalUserActivity();
                //Click Scan App
                _device.ControlPanel.ScrollPressWait("#hpid-scan-homescreen-button", "#hpid-keyboard", _waitTimeSpan);
                //Click Scan to Fax
                _device.ControlPanel.PressWait("#hpid-sendFax-homescreen-button", "#hpid-keyboard", _waitTimeSpan);
                //Enter UserName
                _device.ControlPanel.TypeOnVirtualKeyboard(_credential.UserName);
                _device.ControlPanel.Type(SpecialCharacter.Tab);
                _pacekeeper.Pause();
                //Enter Domain
                _device.ControlPanel.TypeOnVirtualKeyboard(_credential.Domain);
                _device.ControlPanel.Type(SpecialCharacter.Tab);
                _pacekeeper.Pause();
                //Enter Password
                _device.ControlPanel.TypeOnVirtualKeyboard(_credential.Password);
                _pacekeeper.Pause();
                //Click on SignIn
                _device.ControlPanel.PressWait("#hpid-keyboard-key-done", "#hpid-sendfax-app-screen", _waitTimeSpan);
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Fax login failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        ///  Trigger the Copy job from native app through glass Bed/ADF
        /// </summary>
        /// <param name="copies"></param>
        /// <param name="colorMode"></param>
        [Description("Performs Copy")]
        public void PerformCopyJob(int copies, string colorMode)
        {
            try
            {
                UpdateStatus($"Performing a copy job with {copies} number of copies and {colorMode} mode.");
               _device.ControlPanel.SignalUserActivity();
                _device.ControlPanel.PressHome();
                _pacekeeper.Pause();
                //Checking For Home Screen
                if (_device.ControlPanel.CheckState("#hpid-copy-homescreen-button", OmniElementState.Exists))
                {
                    //Click Copy App
                    _device.ControlPanel.ScrollPressWait("#hpid-copy-homescreen-button", "#hpid-copy-app-screen", _waitTimeSpan);

                    //For Color/Black Option
                    if (_device.ControlPanel.CheckState("#hpid-copy-color-mode-button", OmniElementState.Exists))
                    {
                        //Click on Options
                        _device.ControlPanel.PressWait("#hpid-button-options-show-hide", "#hpid-copy-app-screen", _waitTimeSpan);
                        _pacekeeper.Pause();
                        //Select color/Black
                        _device.ControlPanel.PressWait("#hpid-option-color-black", "#hpid-option-color-black-screen", _waitTimeSpan);
                        _pacekeeper.Pause();
                        //Color Option
                        if (string.Equals(colorMode, "Color", StringComparison.CurrentCultureIgnoreCase))
                        {
                            _device.ControlPanel.Press("#hpid-color-black-selection-color");
                            UpdateStatus("Selecting Colour mode.");
                        }
                        //For monochrome
                        else if (string.Equals(colorMode, "monochrome", StringComparison.CurrentCultureIgnoreCase))
                        {
                            _device.ControlPanel.Press("#hpid-color-black-selection-grayscale");
                            UpdateStatus("Selecting Black mode");
                        }
                        //For Automatically detect
                        else
                        {
                            _device.ControlPanel.Press("#hpid-color-black-selection-autodetect");
                            UpdateStatus("Selecting AutoDetect mode.");
                        }
                        _pacekeeper.Pause();
                    }
                    //Click on Copy start
                    if (_device.ControlPanel.WaitForState("#hpid-copy-start-copies", OmniElementState.Useable, _waitTimeSpan))
                    {
                        _device.ControlPanel.Press("#hpid-copy-start-copies");
                        _pacekeeper.Pause();
                        // Enter no. of copies
                        _device.ControlPanel.TypeOnNumericKeypad(copies.ToString());
                        _pacekeeper.Pause();
                        //start copy job
                        _device.ControlPanel.Press("#hpid-button-copy-start");
                        _pacekeeper.Pause();
                        UpdateStatus("Copying Started...");
                        UpdateScreenShot();
                    }
                    else
                    {
                        UpdateScreenShot();
                        throw new DeviceWorkflowException("Copy job function is not available at the moment. Skipping");
                    }
                }
                else
                {
                    UpdateScreenShot();
                    throw new DeviceWorkflowException("Please return to home page.");
                }
            }
            catch (Exception ex)
            {
                UpdateScreenShot();
                throw new DeviceWorkflowException($"Copy Job failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        ///  Trigger the Copy job from native app through glass Bed/ADF
        /// </summary>
        /// <param name="copies"></param>
        /// <param name="colorMode"></param>
        /// <param name="pageCount"># of pages in the document</param>
        [Description("Performs Copy with Job Build option, please page count of the final document")]
        public void PerformCopyJobWithJobBuild(int copies, string colorMode, int pageCount)
        {
            int oldTotalPagesScan = (int)_device.Snmp.GetNext("1.3.6.1.4.1.11.2.3.9.4.2.1.2.2.1.61").Value;

            
            try
            {
                UpdateStatus($"Performing a copy job with {copies} number of copies, {colorMode} mode and {pageCount} of pages.");
                _device.ControlPanel.SignalUserActivity();
                //Checking For Home Screen
                if (_device.ControlPanel.CheckState("#hpid-copy-homescreen-button", OmniElementState.Exists))
                {
                    if (_device.ControlPanel.WaitForState("#hpid-copy-homescreen-button", OmniElementState.Useable, _waitTimeSpan))
                    {
                        //Click Copy App
                        _device.ControlPanel.PressWait("#hpid-copy-homescreen-button", "#hpid-copy-app-screen", _waitTimeSpan);
                    }
                    else
                    {
                        throw new DeviceWorkflowException("Copy function is not available");
                    }

                    //For Color/Black Option
                    if (_device.ControlPanel.CheckState("#hpid-copy-color-mode-button", OmniElementState.Exists))
                    {
                        //Click on Options
                        _device.ControlPanel.PressWait("#hpid-button-options-show-hide", "#hpid-copy-app-screen", _waitTimeSpan);
                        _pacekeeper.Pause();
                        //Select color/Black
                        _device.ControlPanel.PressWait("#hpid-option-color-black", "#hpid-option-color-black-screen", _waitTimeSpan);
                        _pacekeeper.Pause();
                        //Color Option
                        if (string.Equals(colorMode, "Color", StringComparison.CurrentCultureIgnoreCase) || string.Equals(colorMode, "Colour", StringComparison.CurrentCultureIgnoreCase))
                        {
                            _device.ControlPanel.Press("#hpid-color-black-selection-color");
                            UpdateStatus("Selecting Colour mode.");
                        }
                        //For monochrome
                        else if (string.Equals(colorMode, "monochrome", StringComparison.CurrentCultureIgnoreCase) || string.Equals(colorMode, "Black", StringComparison.CurrentCultureIgnoreCase))
                        {
                            _device.ControlPanel.Press("#hpid-color-black-selection-grayscale");
                            UpdateStatus("Selecting Colour mode.");
                        }
                        //For Automatically detect
                        else
                        {
                            _device.ControlPanel.Press("#hpid-color-black-selection-autodetect");
                            UpdateStatus("Selecting Colour mode.");
                        }
                    }

                    _device.ControlPanel.Press("#hpid-preview-touch-to-start-panel");
                    _pacekeeper.Pause();
                    if (_device.ControlPanel.CheckState("#hpid-prompt-flatbed-autodetect", OmniElementState.Exists))
                    {
                        _device.ControlPanel.Press("#hpid-button-continue");
                        _pacekeeper.Pause();
                    }

                    if (_device.ControlPanel.WaitForState("#hpid-preview-toolbar", OmniElementState.Useable,
                        TimeSpan.FromMinutes(1)))
                    {
                        int newtotalPagesScan = (int)_device.Snmp.GetNext("1.3.6.1.4.1.11.2.3.9.4.2.1.2.2.1.61").Value;

                        while (newtotalPagesScan < (oldTotalPagesScan + pageCount))
                        {
                            _device.ControlPanel.Press("#hpid-preview-button-add-pages");

                            _pacekeeper.Pause();
                            _device.ControlPanel.Press("#hpid-button-scan");
                            _pacekeeper.Pause();

                            if (_device.ControlPanel.CheckState("#hpid-prompt-flatbed-autodetect", OmniElementState.Exists))
                            {
                                _device.ControlPanel.Press("#hpid-button-continue");
                                _pacekeeper.Pause();
                            }

                            newtotalPagesScan = (int)_device.Snmp.GetNext("1.3.6.1.4.1.11.2.3.9.4.2.1.2.2.1.61").Value;
                        }

                        //Click on Copy start hpid-button-done hpid-button-scan hpid-prompt-add-pages
                        _device.ControlPanel.Press("#hpid-copy-start-copies");
                        _pacekeeper.Pause();
                        // Enter no. of copies
                        _device.ControlPanel.TypeOnNumericKeypad(copies.ToString());
                        _pacekeeper.Pause();
                        //start copy job
                        _device.ControlPanel.Press("#hpid-button-copy-start");
                        _pacekeeper.Pause();
                        UpdateScreenShot();
                        UpdateStatus("Copying Started...");
                    }
                    else
                    {
                        UpdateScreenShot();
                        throw new DeviceWorkflowException("Unable to preview the scan, job failed");
                    }
                }
                else
                {
                    UpdateScreenShot();
                    throw new DeviceWorkflowException("Please return to home page.");
                }
            }
            catch (Exception ex)
            {
                UpdateScreenShot();
                throw new DeviceWorkflowException($"Copy Job failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        ///  Trigger the Copy job from native app through glass Bed/ADF
        /// </summary>
        /// <param name="copies"></param>
        /// <param name="zoomSize"></param>
        [Description("Performs Copy of Photo with either enlarge or reduction")]
        public void PerformPhotoCopyJob(int copies, int zoomSize)
        {
            try
            {
                UpdateStatus($"Performing copy of photo with {zoomSize}% of original size and {copies} copies");
                _device.ControlPanel.SignalUserActivity();
                _device.ControlPanel.PressHome();
                //Checking For Home Screen
                if (_device.ControlPanel.WaitForState("#hpid-copy-homescreen-button", OmniElementState.Useable, _waitTimeSpan))
                {
                    //Click Copy App
                    _device.ControlPanel.ScrollPressWait("#hpid-copy-homescreen-button", "#hpid-copy-app-screen",
                        _waitTimeSpan);

                    _pacekeeper.Pause();
                    _device.ControlPanel.Press("#hpid-button-options-show-hide");

                    _pacekeeper.Pause();
                    _device.ControlPanel.ScrollPress("#hpid-option-optimize-text-picture");

                    _pacekeeper.Pause();
                    _device.ControlPanel.Press("#hpid-optimize-text-picture-selection-glossy");

                    _pacekeeper.Pause();
                    _device.ControlPanel.ScrollPress("#hpid-option-reduce-enlarge");

                    _pacekeeper.Pause();
                    if (_device.ControlPanel.CheckState(".hp-radio-button-listitem:contains(Manual)", OmniElementState.Exists))
                    {
                        _device.ControlPanel.Press(".hp-radio-button-listitem:contains(Manual)");
                    }

                    _pacekeeper.Pause();

                    _device.ControlPanel.Press("#hpid-scale-x-percent-textbox");
                    _pacekeeper.Pause();

                    _device.ControlPanel.TypeOnNumericKeypad(zoomSize.ToString());
                   
                    _device.ControlPanel.Press("#hpid-keypad-key-close");

                    _device.ControlPanel.Press("#hpid-copy-start-copies");
                    _device.ControlPanel.TypeOnNumericKeypad(copies.ToString());

                    _pacekeeper.Pause();
                    _device.ControlPanel.Press("#hpid-keypad-key-close");

                    _pacekeeper.Pause();
                    _device.ControlPanel.Press("#hpid-button-copy-start");
                    UpdateScreenShot();
                   
                    _pacekeeper.Pause();
                    UpdateStatus("Copying Started...");
                    if (_device.ControlPanel.CheckState("#hpid-button-continue", OmniElementState.Exists))
                    {
                        _device.ControlPanel.Press("#hpid-button-continue");
                    }
                }
                else
                {
                    throw new DeviceWorkflowException("Copy App is not in an useable state");
                }
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Copy Job failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Sending Fax JOb from native app
        /// </summary>
        /// <param name="destinationNumber">Destination fax number</param>
        [Description("Sends Fax job from Native app")]
        public void SendFax(int destinationNumber)
        {
            try
            {
                _device.ControlPanel.SignalUserActivity();
                UpdateStatus($"Sending Fax to {destinationNumber}");
                //Checking For Home screen
                if (!_device.ControlPanel.CheckState("#hpid-sendfax-app-screen", OmniElementState.Exists))
                {
                    //Checking For Scan home screen
                    if (_device.ControlPanel.CheckState("#hpid-scan-homescreen-button", OmniElementState.VisibleCompletely))
                    {
                        _device.ControlPanel.PressWait("#hpid-scan-homescreen-button", "#hpid-keyboard", _waitTimeSpan);
                    }
                    //Click Scan To Fax
                    _device.ControlPanel.PressWait("#hpid-sendFax-homescreen-button", "#hpid-keyboard", _waitTimeSpan);
                    _pacekeeper.Pause();
                }
                _device.ControlPanel.Press("#hpid-sendfax-recipient-textbox");
                //Enter Fax No
                _device.ControlPanel.TypeOnNumericKeypad(destinationNumber.ToString());
                _pacekeeper.Pause();
                //Click Send
                _device.ControlPanel.PressWait("#hpid-button-sendfax-start", "#hpid-retain-settings-popup", _waitTimeSpan);
                _pacekeeper.Pause();
                UpdateScreenShot();
                //Clear Setting For Next Job
                _device.ControlPanel.PressWait("#hpid-retain-settings-clear-button", "#hpid-sendFax-homescreen-button", _waitTimeSpan);
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Send Fax failed with exception:{ex.Message}");
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
            string ssidListitem = "#hpid-tree-node-listitem-0xe_2";
            string authenticationListitem = "#hpid-tree-node-listitem-0xf_2";
            string configurePskListitem = "#hpid-tree-node-listitem-0x10";
            string passPhraseListitem = "#hpid-tree-node-listitem-0x14_2";
            string wpaPSKOlderDevice = "#2";
            string wpaPSKNewerDevice = "#1";

            try
            {
                //Traversing to WirelessStation
                TraversingToWirelessStation();
                //Click on SSID
                _device.ControlPanel.PressWait(ssidListitem, "#hpid-settings-app-screen", _waitTimeSpan);
                _device.ControlPanel.PressWait(".hp-dynamic-text-box:text", "#hpid-keyboard", _waitTimeSpan);
                //Entering the UserName
                _device.ControlPanel.TypeOnVirtualKeyboard(ssid);
                _device.ControlPanel.PressWait("#hpid-keyboard-key-done", "#hpid-settings-app-screen", _waitTimeSpan);
                _device.ControlPanel.PressWait("#hpid-ok-setting-button", "#hpid-settings-app-screen", _waitTimeSpan);
                _device.ControlPanel.PressWait(authenticationListitem, "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                if (_device.ControlPanel.CheckState(wpaPSKOlderDevice, OmniElementState.Exists))
                {
                    _device.ControlPanel.PressWait(wpaPSKOlderDevice, "#hpid-settings-app-screen", _waitTimeSpan);
                }
                else
                {
                    _device.ControlPanel.PressWait(wpaPSKNewerDevice, "#hpid-settings-app-screen", _waitTimeSpan);
                }
                _pacekeeper.Pause();
                _device.ControlPanel.PressWait("#hpid-ok-setting-button", "#hpid-settings-app-screen", _waitTimeSpan);
                _device.ControlPanel.PressWait(configurePskListitem, "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                _device.ControlPanel.PressWait(passPhraseListitem, "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                //Entering the NetworkPassword
                _device.ControlPanel.PressWait(".hp-dynamic-text-box:password", "#hpid-keyboard", _waitTimeSpan);
                _device.ControlPanel.TypeOnVirtualKeyboard(networkPassword);
                _device.ControlPanel.PressWait("#hpid-keyboard-key-done", "#hpid-settings-app-screen", _waitTimeSpan);
                _device.ControlPanel.PressWait("#hpid-ok-setting-button", "#hpid-settings-app-screen", _waitTimeSpan);
                //Going Back to Home Screen
                JediOmniNavHome();
                //Restart the device
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
            string noSecurityRadiobutton = "#0";
            string authenticationListitem = "#hpid-tree-node-listitem-0xf_2";

            try
            {
                //Traversing to WirelessStation
                TraversingToWirelessStation();
                //Click on Authentication
                _device.ControlPanel.PressWait(authenticationListitem, "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                _device.ControlPanel.PressWait(noSecurityRadiobutton, "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                _device.ControlPanel.PressWait("#hpid-ok-setting-button", "#hpid-settings-app-screen", _waitTimeSpan);
                //Going Back to Home Screen
                JediOmniNavHome();
                //Restart the device
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
        public void SendEmail(string toAddress, string ccAddress, string subject, string filename, string message,
            string bccAddress)
        {
            try
            {
                _device.ControlPanel.SignalUserActivity();
                //Go to Home Screen

                //Click Scan App
                _device.ControlPanel.ScrollPressWait("#hpid-scan-homescreen-button", "#hpid-keyboard", _waitTimeSpan);

                //Click Email App
                _device.ControlPanel.PressWait("#hpid-email-homescreen-button", "#hpid-signin-app-screen", _waitTimeSpan);

                //Enter UserName
                _device.ControlPanel.TypeOnVirtualKeyboard(_credential.UserName);
                _device.ControlPanel.Type(SpecialCharacter.Tab);
                //Enter Password
                _device.ControlPanel.TypeOnVirtualKeyboard(_credential.Password);

                _device.ControlPanel.PressWait("hpid-keyboard-key-done", "#hpid-email-app-screen", _waitTimeSpan);

                _device.ControlPanel.Press("#hpid-to-recipient");
                _device.ControlPanel.TypeOnVirtualKeyboard(toAddress);
                _pacekeeper.Pause();
                _device.ControlPanel.Press("#hpid-cc-recipient");
                _device.ControlPanel.TypeOnVirtualKeyboard(ccAddress);
                _pacekeeper.Pause();
                _device.ControlPanel.Press("#hpid-bcc-recipient");
                _device.ControlPanel.TypeOnVirtualKeyboard(bccAddress);
                _pacekeeper.Pause();
                _device.ControlPanel.Press("#hpid-subject");
                _device.ControlPanel.TypeOnVirtualKeyboard(subject);
                _pacekeeper.Pause();
                _device.ControlPanel.Press("#hpid-file-name");
                _device.ControlPanel.TypeOnVirtualKeyboard(filename);
                _pacekeeper.Pause();
                _device.ControlPanel.Press("#hpid-message");
                _device.ControlPanel.TypeOnVirtualKeyboard(message);
                _pacekeeper.Pause();
                //Click Send
                _device.ControlPanel.PressWait("#hpid-button-folder-stop", "#hpid-email-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();

                //Clear the settings
                _device.ControlPanel.PressWait("#hpid-retain-settings-clear-button", "#hpid-keyboard", _waitTimeSpan);
                _pacekeeper.Pause();
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Scan to Email navigation failed with exception:{ex.Message}");
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
                _device.ControlPanel.SignalUserActivity();
                //Go to Home Screen

                //Click Scan App
                _device.ControlPanel.ScrollPressWait("#hpid-scan-homescreen-button", "#hpid-keyboard", _waitTimeSpan);

                //Click Scan to Network App
                _device.ControlPanel.PressWait("#hpid-networkFolder-homescreen-button", "#hpid-keyboard", _waitTimeSpan);

                //Enter Folder Name
                _device.ControlPanel.Press("#hpid-network-folder-recipient-textbox");
                _device.ControlPanel.TypeOnVirtualKeyboard(networkFolder);

                //Click Send
                _device.ControlPanel.PressWait("#hpid-button-folder-stop", "#hpid-email-app-screen", _waitTimeSpan);

                //Enter UserName
                _device.ControlPanel.TypeOnVirtualKeyboard(networkUserName);
                _device.ControlPanel.Type(SpecialCharacter.Tab);
                _pacekeeper.Pause();
                //Enter Domain
                _device.ControlPanel.TypeOnVirtualKeyboard(domain);
                _device.ControlPanel.Type(SpecialCharacter.Tab);
                _pacekeeper.Pause();
                //Enter Password
                _device.ControlPanel.TypeOnVirtualKeyboard(networkPassword);
                _pacekeeper.Pause();

                //Click Ok
                _device.ControlPanel.PressWait("#hpid-keyboard-key-done", "#hpid-network-folder-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();

                //Clear the settings
                _device.ControlPanel.PressWait("#hpid-retain-settings-clear-button", "#hpid-keyboard", _waitTimeSpan);
                _pacekeeper.Pause();
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Scan to folder navigation failed with exception:{ex.Message}");
            }
        }

        [Description("Selects display Language for the session")]
        public void SetUILanguage(Language language)
        {
            _device.ControlPanel.SignalUserActivity();
            var code = Properties.OmniResources.ResourceManager.GetString(language.ToString());
            if (_device.ControlPanel.CheckState("#hpid-settings-homescreen-button", OmniElementState.Exists))
            {
                if (!_device.ControlPanel.WaitForState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(1)))
                {
                    JediOmniNavHome();
                }
                _device.ControlPanel.WaitForState("#hpid-button-device-information", OmniElementState.Useable,
                    _waitTimeSpan);
                _device.ControlPanel.PressWait("#hpid-button-device-information", "#hpid-information-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                _device.ControlPanel.WaitForState(code, OmniElementState.Useable, _waitTimeSpan);
                _device.ControlPanel.Press(code);
                _pacekeeper.Pause();
                _device.ControlPanel.WaitForState(".hp-done-button", OmniElementState.Useable, _waitTimeSpan);
                _device.ControlPanel.Press(".hp-done-button");
                _pacekeeper.Pause();
            }
            else
            {
                throw new DeviceWorkflowException("Home Page is not accessible");
            }
        }

        [Description("Sets the display Language for the device")]
        public void SetLanguage(Language language)
        {
            _device.ControlPanel.SignalUserActivity();
            var code = Properties.OmniResources.ResourceManager.GetString(language.ToString());
            if (_device.ControlPanel.CheckState("#hpid-settings-homescreen-button", OmniElementState.Exists))
            {
                if (!_device.ControlPanel.WaitForState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(1)))
                {
                    JediOmniNavHome();
                }
                _pacekeeper.Pause();
                _device.ControlPanel.ScrollPressWait("#hpid-settings-homescreen-button", "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();

                _device.ControlPanel.WaitForState("#hpid-tree-node-listitem-fe333f6b-c946-43d0-8bef-b78bb518b47d",
                    OmniElementState.Useable, _waitTimeSpan);

                _device.ControlPanel.PressWait("#hpid-tree-node-listitem-fe333f6b-c946-43d0-8bef-b78bb518b47d", "#hpid-settings-app-menu-panel", _waitTimeSpan);
                _pacekeeper.Pause();
                _device.ControlPanel.PressWait("#hpid-tree-node-listitem-displaysettings", "#hpid-display-language-setting", _waitTimeSpan);
                _pacekeeper.Pause();
                _device.ControlPanel.PressWait("#hpid-tree-node-listitem-languagesettings", "#hpid-language-selection-language", _waitTimeSpan);
                _pacekeeper.Pause();
                _device.ControlPanel.PressWait("#hpid-language-selection-language", "#hpid-display-language-setting-controlpanel-language", _waitTimeSpan);
                _pacekeeper.Pause();
                if (_device.ControlPanel.WaitForState(code, OmniElementState.Useable, _waitTimeSpan))
                {
                    _device.ControlPanel.Press(code);
                    _pacekeeper.Pause();
                    UpdateScreenShot();
                    UpdateStatus($"Language has been changed to {language}");
                    JediOmniNavHome();
                    _pacekeeper.Pause();
                }
            }
            else
            {
                throw new DeviceWorkflowException("Home Page is not accessible");
            }
        }

        [Description("Validates the display Language for the device")]
        public void VerifyLanguage(Language language)
        {
            _device.ControlPanel.SignalUserActivity();
            var code = Properties.Resources.ResourceManager.GetString(language.ToString());
            if (_device.ControlPanel.CheckState("#hpid-settings-homescreen-button", OmniElementState.Exists))
            {
                if (!_device.ControlPanel.WaitForState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(1)))
                {
                    JediOmniNavHome();
                }
                _pacekeeper.Pause();
                _device.ControlPanel.ScrollPressWait("#hpid-settings-homescreen-button", "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();

                _device.ControlPanel.WaitForState("#hpid-tree-node-listitem-fe333f6b-c946-43d0-8bef-b78bb518b47d",
                    OmniElementState.Useable, _waitTimeSpan);

                _device.ControlPanel.PressWait("#hpid-tree-node-listitem-fe333f6b-c946-43d0-8bef-b78bb518b47d", "#hpid-settings-app-menu-panel", _waitTimeSpan);
                _pacekeeper.Pause();
                _device.ControlPanel.PressWait("#hpid-tree-node-listitem-displaysettings", "#hpid-display-language-setting", _waitTimeSpan);
                _pacekeeper.Pause();
                _device.ControlPanel.PressWait("#hpid-tree-node-listitem-languagesettings", "#hpid-language-selection-language", _waitTimeSpan);
                _pacekeeper.Pause();
                _device.ControlPanel.PressWait("#hpid-language-selection-language", "#hpid-display-language-setting-controlpanel-language", _waitTimeSpan);
                _pacekeeper.Pause();

                bool itemExists = _device.ControlPanel.CheckState($"#hpid-language-selection-language .hp-listitem-text:contains({code})", OmniElementState.Exists);
                UpdateScreenShot();
                
                if (!itemExists)
                {
                    UpdateScreenShot();
                    throw new DeviceWorkflowException($"Current Language is not {code}");
                }
                UpdateStatus($"Current Language has been verified as {language}");
            }
            else
            {
                UpdateScreenShot();
                throw new DeviceWorkflowException("Home Page is not accessible");
            }

            JediOmniNavHome();
        }

        public void ReturnHome()
        {
            try
            {
                while (!_device.ControlPanel.WaitForState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(1)))
                {
                    _device.ControlPanel.PressScreen(10, 10);
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
            catch (Exception exception)
            {
                throw new DeviceWorkflowException("Unable to return home", exception);
            }
        }

        /// <summary>
        /// Enabling DHCP
        /// </summary>
        [Description("Enabling DHCP Mode")]
        public void EnablingDHCPMode()
        {
            string wirelessMenuListitem = "#hpid-tree-node-listitem-jetdirectmenu2";
            string tcpIPListitem = "#hpid-tree-node-listitem-0x4";
            string ipv4SettingsListitem = "#hpid-tree-node-listitem-0x20";
            string configModeListitem = "#hpid-tree-node-listitem-0x25_2";
            string dhcpRadiobutton = "#1";

            try
            {
                _device.ControlPanel.SignalUserActivity();
                //Checking For Home Screen
                if (!_device.ControlPanel.WaitForState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(1)))
                {
                    JediOmniNavHome();
                }
                //Click Settings App
                _device.ControlPanel.ScrollPressWait("#hpid-settings-homescreen-button", "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                //Click on Networking
                _device.ControlPanel.PressWait("#hpid-tree-node-listitem-networkingandiomenu", "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                //Click on Wireless
                _device.ControlPanel.PressWait(wirelessMenuListitem, "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                //Click on TCP/IP
                _device.ControlPanel.PressWait(tcpIPListitem, "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                //Click on IPV4 settings
                _device.ControlPanel.PressWait(ipv4SettingsListitem, "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                //Click on Config Method
                _device.ControlPanel.PressWait(configModeListitem, "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                //Select DHCP option
                _device.ControlPanel.PressWait(dhcpRadiobutton, "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                _device.ControlPanel.PressWait("#hpid-ok-setting-button", "#hpid-settings-app-screen", _waitTimeSpan);
                //Go Back
                JediOmniNavHome();
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Enabling DHCP Mode failed with exception:{ex.Message}");
            }
        }

        private void TraversingToWirelessStation()
        {
            string wirelessMenuListitem = "#hpid-tree-node-listitem-jetdirectmenu2";
            string wirelessStationListitem = "#hpid-tree-node-listitem-0x3";
            try
            {
                _device.ControlPanel.SignalUserActivity();
                //Checking For Home Screen
                if (!_device.ControlPanel.WaitForState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(1)))
                {
                    JediOmniNavHome();
                }
                //Click Settings App
                _device.ControlPanel.ScrollPressWait("#hpid-settings-homescreen-button", "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                //Click on Networking
                _device.ControlPanel.PressWait("#hpid-tree-node-listitem-networkingandiomenu", "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                //Click on Wireless
                _device.ControlPanel.PressWait(wirelessMenuListitem, "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                //Click on Wireless Station
                _device.ControlPanel.PressWait(wirelessStationListitem, "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Traversing To Wireless Station failed with exception:{ex.Message}");
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

        /// <summary>
        /// USB Firmware Upgrade
        /// </summary>
        [Description("Upgrades Firmware from a bundle stored on USB")]
        public void UsbFirmwareUpgrade()
        {
            _device.ControlPanel.SignalUserActivity();
            _device.ControlPanel.ScrollPressWait("#hpid-supportTools-homescreen-button", "#hpid-supporttools-app-screen", _waitTimeSpan);
            _pacekeeper.Pause();
            _device.ControlPanel.PressWait("#hpid-tree-node-listitem-maintenance", "#hpid-settings-app-menu-panel", _waitTimeSpan);
            _pacekeeper.Pause();
            _device.ControlPanel.PressWait("#hpid-tree-node-listitem-usbfirmwareupgrade", "#hpid-usb-firmware-upgrade-screen", _waitTimeSpan);
            _pacekeeper.Pause();
            if (_device.ControlPanel.WaitForAvailable("#hpid-firmware-bundles-list", _waitTimeSpan))
            {
                if (_device.ControlPanel.WaitForAvailable("#hpid-firmware-bundle-file-0", _waitTimeSpan))
                {
                    _device.ControlPanel.Press("#hpid-firmware-bundle-file-0");
                }
                if (_device.ControlPanel.WaitForState("#hpid-setting-install-button", OmniElementState.Enabled, _waitTimeSpan))
                {
                    _device.ControlPanel.Press("#hpid-setting-install-button");
                    _pacekeeper.Pause();

                    if (_device.ControlPanel.WaitForAvailable("#hpid-upgrade-message", _waitTimeSpan))
                    {
                        if (_device.ControlPanel.WaitForAvailable("#hpid-upgrade-button", _waitTimeSpan))
                        {
                            _device.ControlPanel.Press("#hpid-upgrade-button");
                            _pacekeeper.Pause();
                            while (_device.ControlPanel.WaitForState("#hpid-firmware-install-progress-popup", OmniElementState.VisibleCompletely, _waitTimeSpan))
                            {
                                //do nothing
                            }
                            if (_device.ControlPanel.WaitForAvailable("#hpid-usbfirmwareupgrade-error-msg-popup", _waitTimeSpan))
                            {
                                var errorMessage = _device.ControlPanel.GetValue(".hp-popup-content", "textContent", OmniPropertyType.Property);
                                _device.ControlPanel.Press("#hpid-usbfirmwareupgrade-error-popup-button-ok");
                                throw new DeviceWorkflowException(errorMessage);
                            }
                        }
                    }
                }
            }
            else
            {
                var upgradeMessage = _device.ControlPanel.GetValue("#hpid-usb-firmware-upgrade-message", "innerText", OmniPropertyType.Property);
                throw new DeviceWorkflowException(upgradeMessage);
            }
        }

        /// <summary>
        /// Sleep mode of the device
        /// </summary>
        [Description("Enter device into Sleep mode")]
        public void EnterSleepMode()
        {
            try
            {
                _device.ControlPanel.SignalUserActivity();
                JediOmniNavHome();
                //_device.ControlPanel.PressHome();
                _pacekeeper.Pause();
                if (_device.ControlPanel.CheckState("#hpid-button-device-information", OmniElementState.Exists))
                {
                    _device.ControlPanel.WaitForState("#hpid-button-device-information", OmniElementState.Useable,
                        _waitTimeSpan);
                    _device.ControlPanel.PressWait("#hpid-button-device-information", "#hpid-information-screen",
                        _waitTimeSpan);
                    _device.ControlPanel.Press("#hpid-info-pages-sleep-mode-list-item");
                    _device.ControlPanel.WaitForState("#hpid-sleepmode-screen", OmniElementState.Useable,
                        _waitTimeSpan);
                    _device.ControlPanel.PressWait("#hpid-sleepmode-screen", "#hpid-sleep-button", _waitTimeSpan);
                    _device.ControlPanel.Press("#hpid-sleep-button");
                    Thread.Sleep(TimeSpan.FromSeconds(5));

                    //check if the device has fallen asleep
                    if (!Retry.UntilTrue(() => _device.Snmp.Get("1.3.6.1.4.1.11.2.3.9.4.2.1.1.1.2.0") == "2", 10, _waitTimeSpan))
                    {
                        throw new DeviceWorkflowException("Device hasn't entered into sleep mode");
                    }

                    UpdateStatus("Device has entered sleep mode.");
                }
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException("Device hasn't entered into sleep mode", ex);
            }
        }

        /// <summary>
        /// Resets the device, ensuring it is at the home screen and no user is signed in.
        /// </summary>
        [Description("Reset the device")]
        public void Reset()
        {
            // Return to the home screen and sign out so that the Reset button is available
            _preparationManager = new JediOmniPreparationManager(_device);
            _preparationManager.Reset();
        }

        /// <summary>
        /// Pauses all the current jobs
        /// </summary>
        [Description("Pauses all the current jobs")]
        public void PauseAllJobs()
        {
            try
            {
                _device.ControlPanel.SignalUserActivity();
                if (_device.ControlPanel.WaitForState(".hp-button-active-jobs:last", OmniElementState.Useable, _waitTimeSpan))
                {
                    // Press Active Jobs Button from home screen
                    _device.ControlPanel.ScrollPressWait(".hp-button-active-jobs:last", "#hpid-active-jobs-screen");

                    // Click Pause All
                    if (_device.ControlPanel.WaitForState("#hpid-button-pause-all", OmniElementState.Useable, _waitTimeSpan))
                        _device.ControlPanel.Press("#hpid-button-pause-all");
                    else
                    {
                        throw new DeviceWorkflowException("No jobs to pause");
                    }

                    // Exit to Home Screen
                    if (_device.ControlPanel.CheckState("#hpid-active-jobs-exit", OmniElementState.Useable))
                    {
                        _device.ControlPanel.PressWait("#hpid-active-jobs-exit", "#hpid-pause-resume-popup");
                    }
                    if (_device.ControlPanel.WaitForState("#hpid-button-stay-paused", OmniElementState.Useable, _waitTimeSpan))
                    {
                        _device.ControlPanel.Press("#hpid-button-stay-paused");
                    }
                }
                else
                {
                    throw new DeviceWorkflowException("Unable to enter Jog Log to pause the job");
                }
            }
            catch (Exception ex)
            {
                if (_device.ControlPanel.WaitForState("#hpid-message-center-screen", OmniElementState.Exists))
                {
                    throw new DeviceWorkflowException($"There is an error displayed on the device, Please check the device and try again. Inner Exception : { (object)ex.Message}");
                }
                else
                {
                    throw new DeviceWorkflowException($"Unable to pause the job, the button is not available. Inner Exception : {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Resumes all the current jobs
        /// </summary>
        [Description("Resumes all the current jobs")]
        public void ResumeAllJobs()
        {
            try
            {
                _device.ControlPanel.SignalUserActivity();
                if (_device.ControlPanel.WaitForState(".hp-button-active-jobs:last", OmniElementState.Useable, _waitTimeSpan))
                {
                    // Press Active Jobs Button from home screen
                    _device.ControlPanel.ScrollPressWait(".hp-button-active-jobs:last", "#hpid-active-jobs-screen");

                    // Click Resume All
                    if (_device.ControlPanel.WaitForState("#hpid-button-resume-all", OmniElementState.Useable, _waitTimeSpan))
                        _device.ControlPanel.Press("#hpid-button-resume-all");
                    else
                    {
                        throw new DeviceWorkflowException("No jobs to resume");
                    }
                }
                else
                {
                    throw new DeviceWorkflowException("Unable to enter Jog Log to resume the job");
                }

                // Exit to Home Screen
                if (_device.ControlPanel.CheckState("#hpid-active-jobs-exit", OmniElementState.Useable))
                {
                    _device.ControlPanel.Press("#hpid-active-jobs-exit");
                }
            }
            catch (Exception ex)
            {
                if (_device.ControlPanel.WaitForState("#hpid-message-center-screen", OmniElementState.Exists))
                {
                    throw new DeviceWorkflowException($"There is an error displayed on the device, Please check the device and try again. Inner Exception : {ex.Message}");
                }
                else
                {
                    throw new DeviceWorkflowException($"Unable to resume the job, the button is not available. Inner Exception : {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Exits the message center screen
        /// </summary>
        [Description("Exits the Message Center")]
        public void ExitMessageCenter()
        {
            if (_device.ControlPanel.WaitForState("#hpid-message-center-screen", OmniElementState.VisibleCompletely))
            {
                if (_device.ControlPanel.WaitForState("#hpid-message-center-exit-button", OmniElementState.Useable))
                {
                    _device.ControlPanel.Press("#hpid-message-center-exit-button");
                }
            }
        }

        /// <summary>
        /// Checks the supplies application
        /// <param name="validate">Whether to validate displayed info with Webservice/EWS</param>
        /// <param name="printReport">Flag for printing the supplies report</param>
        /// </summary>
        [Description("Enters supplies application and does a smoke test")]
        public void SuppliesAppCheck(bool validate = false, bool printReport = false)
        {
            bool infoValidated = true;
            _device.ControlPanel.SignalUserActivity();

            JediOmniNavHome();

            if (_device.ControlPanel.WaitForState("#hpid-supplies-homescreen-button", OmniElementState.Enabled, _waitTimeSpan))
            {
                if (_device.ControlPanel.ScrollPressWait("#hpid-supplies-homescreen-button",
                    "#hpid-supplies-app-screen",
                    _waitTimeSpan))
                {
                    List<XElement> agentsElements = null;
                    if (validate)
                    {
                        var suppliesTicket = _device.WebServices.GetDeviceTicket("supplies", "urn:hp:imaging:con:service:supplies:SuppliesService:Supplies:AgentList");
                        agentsElements = suppliesTicket.FindElements("Agent").ToList();
                    }
                    if (_device.ControlPanel.WaitForState("#hpid-supplies-list-item-blackcartridge1",
                        OmniElementState.Useable, _waitTimeSpan))
                    {
                        _device.ControlPanel.Press("#hpid-supplies-list-item-blackcartridge1");
                        var blackAgent = agentsElements?.Find(x => x.Value.Contains("Black"));

                        var blackPages = blackAgent?.Elements()
                            .FirstOrDefault(x => x.Name.LocalName.Equals("ApproximatePagesRemaining"))?.Elements()
                            .FirstOrDefault(x => x.Name.LocalName.Equals("ApproximatePagesRemainingValue"));
                        if (blackPages != null)
                        {
                            if (!_device.ControlPanel.GetValue("#hpid-supply-details-approximate-pages-remaining", "innerText", OmniPropertyType.Property).Contains(blackPages.Value))
                            {
                                infoValidated = false;
                            }
                            UpdateStatus($"Black Cartridge value is {blackPages.Value}");
                            UpdateScreenShot();
                        }
                            
                    }

                    if (_device.ControlPanel.WaitForState("#hpid-supplies-list-item-cyancartridge1",
                        OmniElementState.Useable, _waitTimeSpan))
                    {
                        _device.ControlPanel.Press("#hpid-supplies-list-item-cyancartridge1");
                        var cyanAgent = agentsElements?.Find(x => x.Value.Contains("Cyan"));

                        var cyanPages = cyanAgent?.Elements()
                            .FirstOrDefault(x => x.Name.LocalName.Equals("ApproximatePagesRemaining"))?.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("ApproximatePagesRemainingValue"));

                        if (cyanPages != null)
                        {
                            if (!_device.ControlPanel.GetValue("#hpid-supply-details-approximate-pages-remaining", "innerText", OmniPropertyType.Property).Contains(cyanPages.Value))
                            {
                                infoValidated = false;
                            }
                            UpdateStatus($"Cyan Cartridge value is {cyanPages.Value}");
                            UpdateScreenShot();
                        }
                    }

                    if (_device.ControlPanel.WaitForState("#hpid-supplies-list-item-magentacartridge1",
                        OmniElementState.Useable, _waitTimeSpan))
                    {
                        _device.ControlPanel.Press("#hpid-supplies-list-item-magentacartridge1");
                        var magentaAgent = agentsElements?.Find(x => x.Value.Contains("Magenta"));

                        var magentaPages = magentaAgent?.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("ApproximatePagesRemaining"))?.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("ApproximatePagesRemainingValue"));
                        if (magentaPages != null)
                        {
                            if (!_device.ControlPanel.GetValue("#hpid-supply-details-approximate-pages-remaining", "innerText", OmniPropertyType.Property).Contains(magentaPages.Value))
                            {
                                infoValidated = false;
                            }
                            UpdateStatus($"Magenta Cartridge value is {magentaPages.Value}");
                            UpdateScreenShot();
                        }
                    }

                    if (_device.ControlPanel.WaitForState("#hpid-supplies-list-item-yellowcartridge1",
                        OmniElementState.Useable, _waitTimeSpan))
                    {
                        _device.ControlPanel.Press("#hpid-supplies-list-item-yellowcartridge1");
                        var yellowAgent = agentsElements?.Find(x => x.Value.Contains("Yellow"));
                        var yellowPages = yellowAgent?.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("ApproximatePagesRemaining"))?.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("ApproximatePagesRemainingValue"));
                        if (yellowPages != null)
                        {
                            if (!_device.ControlPanel.GetValue("#hpid-supply-details-approximate-pages-remaining", "innerText", OmniPropertyType.Property).Contains(yellowPages.Value))
                            {
                                infoValidated = false;
                            }
                            UpdateStatus($"Yellow Cartridge value is {yellowPages.Value}");
                            UpdateScreenShot();
                        }
                    }

                    if (printReport && _device.ControlPanel.WaitForState("#hpid-supplies-print-button", OmniElementState.Useable))
                    {
                        _device.ControlPanel.Press("#hpid-supplies-print-button");
                        UpdateStatus("Printing the report...");
                    }

                    if (validate)
                    {
                        if (!infoValidated)
                            throw new DeviceWorkflowException("Supplies information mismatch.");
                    }
                }
                else
                {
                    UpdateScreenShot();
                    throw new DeviceWorkflowException("Could not navigate to Supplies application.");
                }
            }
            else
            {
                UpdateScreenShot();
                throw new DeviceWorkflowException("Supplies application is not ready to be used.");
            }
        }

        /// <summary>
        /// Prints the internal pages from Reports application
        /// <param name="randomPage">Flag for indication whether to print a random page or configuration page</param>
        /// </summary>
        [Description("Prints a random internal page or configuration page from Reports application")]
        public void PrintReports(bool randomPage = true)
        {
            _device.ControlPanel.SignalUserActivity();

            if (!_device.ControlPanel.WaitForState("#hpid-reports-app-screen", OmniElementState.VisibleCompletely,
                TimeSpan.FromSeconds(2)))
            {
                JediOmniNavHome();

                if (_device.ControlPanel.WaitForState("#hpid-reports-homescreen-button", OmniElementState.Enabled,
                    _waitTimeSpan))
                {
                    _device.ControlPanel.ScrollPressWait("#hpid-reports-homescreen-button", "#hpid-reports-app-screen",
                        _waitTimeSpan);
                }
                else

                {
                    UpdateScreenShot();
                    throw new DeviceWorkflowException("Reports application is not ready to be used.");
                }
            }

            if (_device.ControlPanel.WaitForState("#hpid-tree-node-listitem-configurationpages",
                OmniElementState.Useable, _waitTimeSpan))
            {
                _device.ControlPanel.PressWait("#hpid-tree-node-listitem-configurationpages",
                    "#hpid-reportapp-pages-screen", _waitTimeSpan);
                _pacekeeper.Pause();
            }

            if (randomPage)
            {
                int random = new Random(10).Next(1, 9);
                _device.ControlPanel.Press($".hp-checkbox.hp-control:nth-child({random})");
                UpdateScreenShot();
            }
            else
            {
                _device.ControlPanel.Press("#hpid-report-page-checkbox-b64e0a99-e520-4ab6-abf3-40ca120065d9");
                UpdateScreenShot();
            }
            _pacekeeper.Pause();
            _device.ControlPanel.Press("#hpid-print-button");
            UpdateStatus("Printing report...");
            _pacekeeper.Pause();
        }

        /// <summary>
        /// Traverses to Job log screen and selects a random job log for viewing and optionally printing the log
        /// </summary>
        /// <param name="printLog">flag for printing the job log</param>
        [Description("Traverses to Job screen and selects a random log for viewing")]
        public void TraverseJobLogs(bool printLog = false)
        {
            _device.ControlPanel.SignalUserActivity();

            JediOmniNavHome();

            if (_device.ControlPanel.WaitForState("#hpid-jobLog-homescreen-button", OmniElementState.Enabled,
                _waitTimeSpan))
            {
                if (_device.ControlPanel.ScrollPressWait("#hpid-jobLog-homescreen-button", "#hpid-job-log-app-screen",
                    _waitTimeSpan))
                {
                    _pacekeeper.Pause();
                    int random = new Random(10).Next(1, 9);
                    if (_device.ControlPanel.PressWait($".hp-listitem.hp-control:nth-child({random})",
                        "#hpid-job-log-information-panel-label", _waitTimeSpan))
                    {
                        if (printLog)
                        {
                            _device.ControlPanel.Press("#hpid-jobs-log-print-log");
                        }
                        
                    }
                }
                else
                {
                    throw new DeviceWorkflowException("Job log application did not launch.");
                }
            }
        }

        /// <summary>
        /// Does internal calibration for print heads
        /// </summary>
        [Description("Does internal calibration for print heads")]
        public void PrintInternalCleaning()
        {
            _device.ControlPanel.SignalUserActivity();

            JediOmniNavHome();

            if (_device.ControlPanel.WaitForState("#hpid-supportTools-homescreen-button", OmniElementState.Enabled,
                _waitTimeSpan))
            {
                if (_device.ControlPanel.ScrollPressWait("#hpid-supportTools-homescreen-button", "#hpid-supporttools-app-screen",
                    _waitTimeSpan))
                {
                    if (_device.ControlPanel.WaitForState("#hpid-tree-node-listitem-maintenance",
                        OmniElementState.Useable))
                    {
                        _device.ControlPanel.Press("#hpid-tree-node-listitem-maintenance");
                    }

                    if (_device.ControlPanel.WaitForState("#hpid-tree-node-listitem-calibrationcleaning",
                        OmniElementState.Useable))
                    {
                        _device.ControlPanel.Press("#hpid-tree-node-listitem-calibrationcleaning");
                    }

                    if (_device.ControlPanel.WaitForState("#hpid-tree-node-listitem-printheadcleaning",
                        OmniElementState.Useable))
                    {
                        _device.ControlPanel.Press("#hpid-tree-node-listitem-printheadcleaning");
                    }

                    _pacekeeper.Pause();
                    _device.ControlPanel.Press("#hpid-setting-start-button");

                    while (CheckForPopup())
                    {
                        Thread.Sleep(2000);
                    }

                    if (_device.ControlPanel.WaitForState("#hpid-button-Ok", OmniElementState.Useable, _waitTimeSpan))
                    {
                        _device.ControlPanel.Press("#hpid-button-Ok");
                        _pacekeeper.Pause();
                        _device.ControlPanel.Press("#hpid-button-Ok");
                        _pacekeeper.Pause();
                    }

                    while (CheckForPopup())
                    {
                        Thread.Sleep(2000);
                    }

                    if (_device.ControlPanel.WaitForState("#hpid-button-Ok", OmniElementState.Useable, _waitTimeSpan))
                    {
                        _device.ControlPanel.Press("#hpid-button-Ok");
                        _pacekeeper.Pause();
                        _device.ControlPanel.Press("#hpid-button-Ok");
                        _pacekeeper.Pause();
                    }

                    _device.ControlPanel.Press("#hpid-button-Cancel");
                    _pacekeeper.Pause();
                    _device.ControlPanel.Press("#hpid-button-Cancel");
                    _pacekeeper.Pause();
                    _device.ControlPanel.Press("#hpid-button-Ok");
                    _pacekeeper.Pause();
                }
            }
        }
        /// <summary>
        /// Promotes the job
        /// </summary>
        /// <param name="jobIndex">the index of the job to be promoted</param>
        [Description("Promotes the job at the given index")]
        public void PromoteJob(int jobIndex=2)
        {
            _device.ControlPanel.SignalUserActivity();

            if (_device.ControlPanel.WaitForState(".hp-button-active-jobs:last", OmniElementState.Useable,
                _waitTimeSpan))
            {
                // Press Active Jobs Button from home screen
                _device.ControlPanel.ScrollPressWait(".hp-button-active-jobs:last", "#hpid-active-jobs-screen");
                int count = _device.ControlPanel.GetCount(".hp-listitem.hp-listitem-active-job");
                if (jobIndex > count)
                    jobIndex = count;

                _device.ControlPanel.Press($".hp-listitem.hp-listitem-active-job:nth-child({jobIndex})");
                _pacekeeper.Pause();
                var jobBeforePromotion = _device.ControlPanel.GetValue(".hp-listitem.hp-listitem-active-job.hp-selected","innerText",OmniPropertyType.Property);

                if (_device.ControlPanel.WaitForState("#hpid-button-promote-job", OmniElementState.Useable,
                    _waitTimeSpan))
                {
                    _device.ControlPanel.Press("#hpid-button-promote-job");
                    _pacekeeper.Pause();
                }

                var jobAfterPromotion = _device.ControlPanel.GetValue($".hp-listitem.hp-listitem-active-job:nth-child({jobIndex})", "innerText", OmniPropertyType.Property);

                if (jobAfterPromotion == jobBeforePromotion)
                {
                    throw new DeviceWorkflowException("Job Promotion was not successful.");
                }
            }
            else
            {
                throw new DeviceWorkflowException("No active jobs to perform Job Promotion.");
            }
        }
        /// <summary>
        /// Explores the display setting randomly changing a setting to either true or false
        /// </summary>
        [Description("Explores the display setting randomly changing a setting to either true or false")]
        public void ExploreDisplaySettings()
        {
            List<string> displaytreeList = new List<string> { "keypresssound", "informationscreen", "displaydateandtime", "jobstatusnotifications" };
            Random rand = new Random(18);
            _device.ControlPanel.SignalUserActivity();
            if (_device.ControlPanel.CheckState("#hpid-settings-homescreen-button", OmniElementState.Exists))
            {
                if (!_device.ControlPanel.WaitForState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely,
                    TimeSpan.FromSeconds(1)))
                {
                    JediOmniNavHome();
                }

                _pacekeeper.Pause();
                _device.ControlPanel.ScrollPressWait("#hpid-settings-homescreen-button", "#hpid-settings-app-screen",
                    _waitTimeSpan);
                _pacekeeper.Pause();

                _device.ControlPanel.WaitForState("#hpid-tree-node-listitem-fe333f6b-c946-43d0-8bef-b78bb518b47d",
                    OmniElementState.Useable, _waitTimeSpan);

                _device.ControlPanel.PressWait("#hpid-tree-node-listitem-fe333f6b-c946-43d0-8bef-b78bb518b47d",
                    "#hpid-settings-app-menu-panel", _waitTimeSpan);
                _pacekeeper.Pause();
                _device.ControlPanel.PressWait("#hpid-tree-node-listitem-displaysettings",
                    "#hpid-settings-app-screen", _waitTimeSpan);
                _pacekeeper.Pause();
                string displayItem = displaytreeList.ElementAt((int)(DateTime.Now.Ticks %4));
                _device.ControlPanel.PressWait($"#hpid-tree-node-listitem-{displayItem}",
                    $"#hpid-setting-{displayItem}-selection", _waitTimeSpan);
                _pacekeeper.Pause();
                string resultValue = Convert.ToBoolean((int)(DateTime.Now.Ticks%2)).ToString().ToLowerInvariant();
                _device.ControlPanel.Press($"#hpid-setting-{displayItem}-selection-{resultValue}");
                UpdateScreenShot();
                _pacekeeper.Pause();
                UpdateStatus($"Set {displayItem} in display setting to {resultValue}");
                
            }
        }


        private bool CheckForPopup()
        {
            if (_device.ControlPanel.WaitForState("#hpid-button-Ok", OmniElementState.Useable, TimeSpan.FromSeconds(2)))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Jedi Omni method for navigating home.
        /// </summary>
        private void JediOmniNavHome()
        {
            if (_device != null)
            {
                if (!_device.ControlPanel.WaitForState("#hpid-button-reset", OmniElementState.Useable, TimeSpan.FromSeconds(2)))
                {
                    _device.ControlPanel.PressHome();
                    if (_device.ControlPanel.WaitForState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(5)))
                    {
                        return;
                    }
                    while (_device.ControlPanel.WaitForState(".hp-header-levelup-button:last", OmniElementState.Useable,
                        TimeSpan.FromSeconds(2)))
                    {
                        _device.ControlPanel.Press(".hp-header-levelup-button:last");
                        Thread.Sleep(250);
                    }

                    while (_device.ControlPanel.WaitForState(".hp-button-back:last", OmniElementState.Useable,
                        TimeSpan.FromSeconds(2)))
                    {
                        _device.ControlPanel.Press(".hp-button-back:last");
                        Thread.Sleep(250);
                    }
                }


                var controls = _device.ControlPanel.GetIds("div", OmniIdCollectionType.Children);
                if (controls.Contains("hpid-button-reset"))
                {
                    if (_device.ControlPanel.WaitForState("#hpid-button-reset", OmniElementState.Useable, TimeSpan.FromSeconds(2)))
                    {
                        _device.ControlPanel.Press("#hpid-button-reset");
                    }
                    else
                    {
                        string value = _device.ControlPanel.GetValue("#hp-button-signin-or-signout", "innerText", OmniPropertyType.Property).Trim();
                        if (value.Contains("Sign Out"))
                        {
                            if (_device.ControlPanel.WaitForState("#hp-button-signin-or-signout", OmniElementState.Useable, TimeSpan.FromSeconds(2)))
                            {
                                _device.ControlPanel.Press("#hp-button-signin-or-signout");
                            }
                        }
                    }
                }

                while (_device.ControlPanel.WaitForState(".hp-button-back:last", OmniElementState.Useable, TimeSpan.FromSeconds(2)))
                {
                    _device.ControlPanel.Press(".hp-button-back:last");
                    Thread.Sleep(250);
                }

                if (!_device.ControlPanel.WaitForState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(5)))
                {
                    throw new DeviceWorkflowException("Home Page is not accessible");
                }
                JediOmniNotificationPanel notificationPanel = new JediOmniNotificationPanel(_device);
                bool success = notificationPanel.WaitForNotDisplaying("Initializing scanner", "Clearing settings");
                if (!success)
                {
                    throw new TimeoutException("Timed out waiting for notification panel state: Contains Initializing scanner, Clearing settings");
                }
            }
        }

        private void UpdateScreenShot()
        {
            ScreenCapture?.Invoke(this, new ScreenCaptureEventArgs(_device.ControlPanel.ScreenCapture()));
        }

        private void UpdateStatus(string message)
        {
            StatusUpdate?.Invoke(this, new StatusChangedEventArgs(message));
        }
    }

    internal class ScreenCaptureEventArgs: EventArgs
    {
        public Image ScreenShotImage { get; }

        public ScreenCaptureEventArgs(Image image)
        {
            ScreenShotImage = image;
        }

    }
}
