using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ControlPanel
{
    internal class SiriusPentaneWorkflow
    {
        private readonly SiriusUIv2Device _device;

        private readonly TimeSpan _activeScreenTimeout = TimeSpan.FromSeconds(3);
        private readonly Pacekeeper _pacekeeper = new Pacekeeper(TimeSpan.FromSeconds(1));
        private readonly NetworkCredential _credential;

        public SiriusPentaneWorkflow(SiriusUIv2Device device, NetworkCredential credential)
        {
            _device = device;
            _credential = credential;
        }

        /// <summary>
        /// Authentication
        /// </summary>
        /// <param name="appName"></param>
        [Description("Performs Authentication for the application")]
        public void Authentication(string appName)
        {
            // Create Authenticator
            IAuthenticator auth = AuthenticatorFactory.Create(_device, _credential, AuthenticationProvider.Windows);

            var preparationManager = new SiriusUIv2PreparationManager(_device);
            try
            {
                preparationManager.InitializeDevice(true);

                switch (appName)
                {
                    case "Sign In":
                        throw new DeviceWorkflowException("Sign In from home screen not supported for this device");

                    case "HP AC Secure Pull Print":
                    case "My workflow (FutureSmart)":
                    case "Pull Print":
                        _device.ControlPanel.PressByValue("Apps");
                        WaitForScreenLabel("view_oxpd_home", TimeSpan.FromSeconds(30));

                        string displayedTitle = GetButtonDisplayedTitle(appName);
                        _device.ControlPanel.ScrollToItemByValue("oxpd_home_table", displayedTitle);
                        _device.ControlPanel.PressByValue(displayedTitle);

                        WaitForScreenLabel("view_sips_form", TimeSpan.FromSeconds(30));
                        auth.Authenticate();
                        break;

                    default:
                        AuthenticationHelper.LaunchApp(_device, appName, auth);
                        break;
                }
            }
            catch (Exception ex)
            {
                var currentScreen = _device.ControlPanel.ActiveScreenLabel();
                ExecutionServices.SystemTrace.LogDebug("Active screen = " + currentScreen);
                throw new DeviceInvalidOperationException($"Unable to authenticate: current screen = {currentScreen}", ex);
            }
            finally
            {
                try
                {
                    preparationManager.NavigateHome();
                }
                catch
                {
                    //ignored
                }
            }
        }

        private void WaitForScreenLabel(string label, TimeSpan waitTime)
        {
            var cp = _device.ControlPanel;
            if (!cp.WaitForActiveScreenLabel(label, waitTime))
            {
                var screenInfo = cp.GetScreenInfo();
                var msg = $"Unexpected screen: expected={label}, actual={screenInfo.ScreenLabels.FirstOrDefault()}";
                throw new DeviceWorkflowException(msg);
            }
        }

        /// <summary>
        /// Gets the button displayed title based on an assumed max length with "..." at the end
        /// </summary>
        /// <param name="buttonTitle">The button title.</param>
        /// <param name="maxDisplayLength">Maximum length of the display.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private static string GetButtonDisplayedTitle(string buttonTitle, int maxDisplayLength = 14)
        {
            var result = buttonTitle;
            if (buttonTitle.Length >= maxDisplayLength && !buttonTitle.EndsWith("..."))
            {
                result = buttonTitle.Substring(0, maxDisplayLength - 3) + "...";
            }
            return result;
        }

        /// <summary>
        /// bypass the error screen
        /// </summary>

        private void CheckErrorScreen()
        {
            _pacekeeper.Pause();
            var screenInfo = _device.ControlPanel.GetScreenInfo();
            try
            {
                if (screenInfo.Widgets.FindByValue("touch OK to continue") != null)
                {
                    _device.ControlPanel.Press("prdsts_msg_footer.0");
                }
            }
            catch (ElementNotFoundException)
            {
                //ignored
            }
        }

        /// <summary>
        /// Checks the scan output file type for the selected shared network folder
        /// </summary>
        /// <param name="networkFolderName">network folder name</param>
        /// <param name="scanOutputFileType">the file type</param>
        /// <param name="scanPaperSize">the paper size</param>
        [Description("Verifies the Scan Filetype and Paper size for SNF workflow")]
        public void CheckScanFileTypeAndPaperSize(string networkFolderName, string scanOutputFileType, string scanPaperSize = "Automatic")
        {
            _device.ControlPanel.PressKey(SiriusSoftKey.Home);
            _pacekeeper.Pause();
            _device.ControlPanel.PressByValue("Scan");
            _pacekeeper.Pause();
            _device.ControlPanel.PressByValue("Network Folder");
            _pacekeeper.Pause();
            var sharedFolderWidget =
                _device.ControlPanel.GetScreenInfo().Widgets.First(x => x.HasValue(networkFolderName));
            _device.ControlPanel.Press(sharedFolderWidget.Id);
            _pacekeeper.Pause();

            var documentWidget =
                _device.ControlPanel.GetScreenInfo().Widgets.First(x => x.Id == "scanfolder_home_value1");
            if (documentWidget != null)
            {
                if (!documentWidget.Values.Values.First().Equals(scanOutputFileType, StringComparison.OrdinalIgnoreCase))
                {
                    throw new DeviceWorkflowException($"The scanfile type: {scanOutputFileType} does not match: {documentWidget.Values.Values.First()} on the device");
                }
            }

            _device.ControlPanel.PressByValue("Settings");

            var fileTypeWidget = _device.ControlPanel.GetScreenInfo().Widgets.First(x => x.Id == "scan_folder_menu_list.5");
            if (fileTypeWidget != null)
            {
                if (!fileTypeWidget.Values.Values.First().Contains(scanPaperSize, StringComparison.OrdinalIgnoreCase))
                {
                    throw new DeviceWorkflowException($"The scan paper size: {scanPaperSize} does not match: {fileTypeWidget.Values.Values.First()} on the device");
                }
            }

            _device.ControlPanel.PressKey(SiriusSoftKey.Back);
        }

        /// <summary>
        /// Copies Document
        /// </summary>
        /// <param name="copiesCount">No of copies</param>
        /// <param name="sidesSelection">Sides Selection</param>
        /// <param name="printOption">Print Option</param>
        [Description("Performs document copy")]
        public void DocumentCopy(int copiesCount, string sidesSelection, string printOption)
        {
            _device.ControlPanel.PressKey(SiriusSoftKey.Home);
            _device.ControlPanel.PressByValue("Copy");

            _device.ControlPanel.WaitForActiveScreenLabel("view_copy_home");
            try
            {
                try
                {
                    _device.ControlPanel.Press("copy_home_numpad_button");
                    _pacekeeper.Pause();

                    _device.ControlPanel.WaitForActiveScreenLabel("view_copy_num_copies");

                    SetDocumentCopy(copiesCount.ToString());

                    _pacekeeper.Pause();
                    _device.ControlPanel.Press("gr_footer_done_back.0");
                    _pacekeeper.Pause();
                }
                catch
                {
                    throw new DeviceWorkflowException("Setting Copies Count failed");
                }

                try
                {
                    _device.ControlPanel.Press("copy_home_value2");
                    _pacekeeper.Pause();
                    _device.ControlPanel.WaitForActiveScreenLabel("view_copy_2sided");

                    if (string.Equals(sidesSelection, "1to1Sided", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _device.ControlPanel.Press("copy_two_sided_options.0");
                    }
                    else if (string.Equals(sidesSelection, "1to2Sided", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _device.ControlPanel.Press("copy_two_sided_options.1");
                    }
                    else if (string.Equals(sidesSelection, "2to1Sided", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _device.ControlPanel.PressKey(SiriusSoftKey.Left);
                        _device.ControlPanel.Press("copy_two_sided_options.2");
                    }
                    else if (string.Equals(sidesSelection, "2to2Sided", StringComparison.CurrentCultureIgnoreCase))
                    {
                        _device.ControlPanel.PressKey(SiriusSoftKey.Left);
                        _device.ControlPanel.Press("copy_two_sided_options.3");
                    }
                    _pacekeeper.Pause();
                }
                catch
                {
                    throw new DeviceWorkflowException("Setting Sides selection failed");
                }

                if (string.Equals(printOption, "Color", StringComparison.CurrentCultureIgnoreCase))
                {
                    _device.ControlPanel.Press("copy_home_group_footer.0");
                }
                else
                {
                    _device.ControlPanel.Press("copy_home_group_footer.1");
                }

                _pacekeeper.Pause();
                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();
            }
            catch
            {
                throw new DeviceWorkflowException("Document Copy failed");
            }
        }

        /// <summary>
        /// Email App Login with SafeCom Pic Authentication
        /// </summary>
        [Description("Performs safecom authentication with PIC for Email application")]
        public void EmailAppLoginWithSafeComPicAuthentication(string pin)
        {
            TimeSpan defaultScreenWait = TimeSpan.FromSeconds(30);
            //Default generated pin for Office Workers 1-20 in the Safecom Server
            try
            {
                //Getting the Sirius Device

                _device.PowerManagement.Wake();

                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _device.ControlPanel.WaitForActiveScreenLabel("view_home", defaultScreenWait);

                //Navigate to Safecom App
                _device.ControlPanel.PressByValue("Scan");
                _device.ControlPanel.WaitForActiveScreenLabel("view_oxpd_home", defaultScreenWait);
                _device.ControlPanel.ScrollToItemByValue("oxpd_home_table", "Email");
                _device.ControlPanel.PressByValue("Email", StringMatch.StartsWith);
                _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                if ((!string.IsNullOrEmpty(_device.ControlPanel.ActiveScreenId())) && _device.ControlPanel.ActiveScreenId().StartsWith("windowsScreen"))
                {
                    //Enter Safecom ID
                    _device.ControlPanel.Press("sips_form_region1_value");
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form_region1_kbd", defaultScreenWait);
                    // SafeCom ID authenticiation uses a unique personal identification code (PIC) that has been assigned to each user.  Our convention is that it's the username with the u lopped off
                    // e.g. u00001  =>  00001; u00038 => 00038
                    _device.ControlPanel.SetValue("sips_form_region1_kbd.4", _credential.UserName.Substring(1));
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                    _device.ControlPanel.Press("sips_common_footer.0");
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                    _device.ControlPanel.Press("sips_form_region1_value");
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form_region1_kbd", defaultScreenWait);
                    _device.ControlPanel.SetValue("sips_form_region1_kbd.4", pin);//Enter PIN
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                }
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Safecom navigation failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Email App Login with SafeCom Windows Authentication
        /// </summary>
        [Description("Performs safecom authentication for Email application")]
        public void EmailAppLoginWithSafeComWindowsAuthentication()
        {
            TimeSpan defaultScreenWait = TimeSpan.FromSeconds(30);
            //Default generated pin for Office Workers 1-20 in the Safecom Server
            try
            {
                //Getting the Sirius Device

                _device.PowerManagement.Wake();

                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _device.ControlPanel.WaitForActiveScreenLabel("view_home", defaultScreenWait);

                //Navigate to Safecom App
                _device.ControlPanel.PressByValue("Scan");
                _device.ControlPanel.WaitForActiveScreenLabel("view_oxpd_home", defaultScreenWait);
                _device.ControlPanel.ScrollToItemByValue("oxpd_home_table", "Email");
                _device.ControlPanel.PressByValue("Email", StringMatch.StartsWith);
                _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                if ((!string.IsNullOrEmpty(_device.ControlPanel.ActiveScreenId())) && _device.ControlPanel.ActiveScreenId().StartsWith("windowsScreen"))
                {
                    _device.ControlPanel.Press("sips_form_region1_value");
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form_region1_kbd", defaultScreenWait);
                    _device.ControlPanel.SetValue("sips_form_region1_kbd.4", _credential.UserName);//Enter Username
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                    _device.ControlPanel.Press("sips_form_region2_value");
                    _device.ControlPanel.SetValue("sips_form_region2_kbd.4", _credential.Password);//Enter password
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                    _device.ControlPanel.Press("sips_form_region3_value");
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form_region3_kbd", defaultScreenWait);
                    _device.ControlPanel.SetValue("sips_form_region3_kbd.4", _credential.Domain);//Enter domain
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                }
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Safecom navigation failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// HPAC Login with Pic Authentication
        /// </summary>
        [Description("Performs HPAC authentication with PIC")]
        public void HPACLoginwithPICAuthentication()
        {
            try
            {
                //Press home sceeen
                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();
                //Press App button
                _device.ControlPanel.PressByValue("Apps");
                _pacekeeper.Pause();
                //Press HP AC
                _device.ControlPanel.PressByValue("HP AC Secure Pull Print");
                _pacekeeper.Pause();
                //If device is not able to connect to the HPAC Server
                try
                {
                    var failedWidget = _device.ControlPanel.GetScreenInfo().Widgets.First(x => x.Id == "oxpd_auth_conn_failed_msg");

                    if (failedWidget != null && failedWidget.HasValue("The printer was unable to connect to the server. Check your internet connection and try again.  Would you like to retry?"))
                    {
                        //Press confirmation popup, this generally comes the first time the device is configured to the HP AC
                        _device.ControlPanel.Press("gr_footer_yes_no.1");
                        _pacekeeper.Pause();
                    }
                }
                catch (ElementNotFoundException)
                {
                }

                if (_device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", TimeSpan.FromSeconds(40)))
                {
                    try
                    {
                        //Handling Incorrect PIC and documents not present for the specified user in CP
                        //if (!_device.ControlPanel.GetScreenInfo().Widgets.Find("sips_nolistorform_txtonly").HasValue("No documents") ||!_device.ControlPanel.GetScreenInfo().Widgets.Find("oxpd_auth_result_msg").HasValue("Incorrect code."))
                        if (_device.ControlPanel.GetScreenInfo().Widgets.First(x => x.Id == "sips_nolistorform_txtonly" && x.HasValue("No documents")) == null && _device.ControlPanel.GetScreenInfo().Widgets.First(x => x.Id == "oxpd_auth_result_msg" && x.HasValue("Incorrect code.")) == null)
                        {
                            //Press on Enter Password text box
                            _device.ControlPanel.Press("sips_form_region1_value");
                            _pacekeeper.Pause();

                            //set the PIC value in the Password text box
                            _device.ControlPanel.SetValue("sips_form_region1_kbd.4", _credential.UserName.Substring(1));
                            _pacekeeper.Pause();

                            //Press OK
                            _device.ControlPanel.Press("sips_common_footer.0");
                            _pacekeeper.Pause();

                            _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                            _pacekeeper.Pause();
                        }
                        else
                        {
                            throw new DeviceWorkflowException("Incorrect PIC");
                        }
                    }
                    catch (ElementNotFoundException)
                    {
                        throw new DeviceWorkflowException("Incorrect PIC");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"HPAC navigation failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// LDAP Authentication for Copy App
        /// </summary>
        [Description("Performs LDAP authentication for Copy application")]
        public void LdapAuthenticationforCopyApp(string password)
        {
            _device.ControlPanel.PressKey(SiriusSoftKey.Home);
            _device.ControlPanel.PressByValue("Copy");
            _device.ControlPanel.Press("ldap_login_username_edit");
            _device.ControlPanel.SetValue("ldap_ldap_login_username_keyboard.4", _credential.UserName);
            _device.ControlPanel.Press("ldap_password_entry_password");
            _device.ControlPanel.SetValue("user_password_entry_password_keyboard.4", password);
            _device.ControlPanel.Press("native_login_footer.0");
            _device.ControlPanel.WaitForActiveScreenLabel("view_oxpd_auth_result", TimeSpan.FromSeconds(75));
            Framework.Logger.LogInfo(_device.ControlPanel.GetScreenInfo().Widgets.Find("oxpd_auth_result_msg_header.1").HasValue("Login Unsuccessful") ? "Login Unsuccessful" : "Login successful");
        }

        /// <summary>
        /// restarts the device
        /// </summary>
        public void RestartDevice()
        {
            Snmp snmp = new Snmp(_device.Address);
            try
            {
                snmp.Set("1.3.6.1.2.1.43.5.1.1.3", 4);
            }
            catch (SnmpException ex)
            {
                throw new SnmpException("Not able to restart printer", ex);
            }
        }

        /// <summary>
        /// Safecom Login with Pic Authentication
        /// </summary>
        /// <param name="pin">Safecom User Pin</param>
        [Description("Performs safecom authentication with PIC")]
        public void SafecomLoginwithPicAuthentication(string pin)
        {
            TimeSpan defaultScreenWait = TimeSpan.FromSeconds(30);
            //Default generated pin for Office Workers 1-20 in the Safecom Server
            try
            {
                //Getting the Sirius Device

                _device.PowerManagement.Wake();

                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _device.ControlPanel.WaitForActiveScreenLabel("view_home", defaultScreenWait);

                //Navigate to Safecom App
                _device.ControlPanel.PressByValue("Apps");
                _device.ControlPanel.WaitForActiveScreenLabel("view_oxpd_home", defaultScreenWait);
                _device.ControlPanel.ScrollToItemByValue("oxpd_home_table", "Pull Print");
                _device.ControlPanel.PressByValue("Pull Print", StringMatch.StartsWith);
                _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                if ((!string.IsNullOrEmpty(_device.ControlPanel.ActiveScreenId())) && _device.ControlPanel.ActiveScreenId().StartsWith("windowsScreen"))
                {
                    //Enter Windows Credentials
                    _device.ControlPanel.Press("sips_form_region1_value");
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form_region1_kbd", defaultScreenWait);
                    _device.ControlPanel.SetValue("sips_form_region1_kbd.4", _credential.UserName);//Enter Username
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                    _device.ControlPanel.Press("sips_form_region2_value");
                    _device.ControlPanel.SetValue("sips_form_region2_kbd.4", _credential.Password);//Enter password
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                    _device.ControlPanel.Press("sips_form_region3_value");
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form_region3_kbd", defaultScreenWait);
                    _device.ControlPanel.SetValue("sips_form_region3_kbd.4", _credential.Domain);//Enter domain
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                }
                else
                {
                    //Enter Safecom ID
                    _device.ControlPanel.Press("sips_form_region1_value");
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form_region1_kbd", defaultScreenWait);
                    // SafeCom ID authenticiation uses a unique personal identification code (PIC) that has been assigned to each user.  Our convention is that it's the username with the u lopped off
                    // e.g. u00001  =>  00001; u00038 => 00038
                    _device.ControlPanel.SetValue("sips_form_region1_kbd.4", _credential.UserName.Substring(1));
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                    _device.ControlPanel.Press("sips_common_footer.0");
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                    _device.ControlPanel.Press("sips_form_region1_value");
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form_region1_kbd", defaultScreenWait);
                    _device.ControlPanel.SetValue("sips_form_region1_kbd.4", pin);//Enter PIN
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                }
                _device.ControlPanel.Press("sips_common_footer.0");//login
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Safecom navigation failed with exception:{ex.Message}");
            }
        }

        ///  <summary>
        ///  Sending Email.
        /// Assuming the From address is already set in EWS page of the Printer.
        ///  </summary>
        ///  <param name="toAddr">To Address</param>
        /// <param name="subject">Subject Line</param>
        [Description("Performs Scan to Email")]
        public void SendEmail(string toAddr,  string subject)
        {
            TimeSpan defaultScreenWait = TimeSpan.FromSeconds(30);
            //Default generated pin for Office Workers 1-20 in the Safecom Server
            try
            {
                //Getting the Sirius Device

                _device.PowerManagement.Wake();

                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _device.ControlPanel.WaitForActiveScreenLabel("view_home", defaultScreenWait);

                //Navigate to Safecom App
                _device.ControlPanel.PressByValue("Scan");
                _device.ControlPanel.WaitForActiveScreenLabel("view_oxpd_home", defaultScreenWait);
                _device.ControlPanel.ScrollToItemByValue("oxpd_home_table", "Email");
                _device.ControlPanel.PressByValue("Email", StringMatch.StartsWith);
                _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                if ((!string.IsNullOrEmpty(_device.ControlPanel.ActiveScreenId())) && _device.ControlPanel.ActiveScreenId().StartsWith("windowsScreen"))
                {
                    _device.ControlPanel.Press("sips_form_region1_value");
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form_region1_kbd", defaultScreenWait);
                    _device.ControlPanel.SetValue("subject_textboxControlname", subject);//Enter subject
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                    _device.ControlPanel.Press("sips_form_region2_value");
                    _device.ControlPanel.SetValue("toAddress_textboxControlname", toAddr);//Enter toaddr
                    _device.ControlPanel.WaitForActiveScreenLabel("view_sips_form", defaultScreenWait);
                }
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Sending Email failed with exception:{ex.Message}");
            }
        }

        private void SetDocumentCopy(string copiesCount)
        {
            char[] copyCount = copiesCount.Take(2).ToArray();
            foreach (char count in copyCount)
            {
                switch (count)
                {
                    case '1':
                        _device.ControlPanel.Press("copy_num_copies_kbd.49");
                        break;

                    case '2':
                        _device.ControlPanel.Press("copy_num_copies_kbd.50");
                        break;

                    case '3':
                        _device.ControlPanel.Press("copy_num_copies_kbd.51");
                        break;

                    case '4':
                        _device.ControlPanel.Press("copy_num_copies_kbd.52");
                        break;

                    case '5':
                        _device.ControlPanel.Press("copy_num_copies_kbd.53");
                        break;

                    case '6':
                        _device.ControlPanel.Press("copy_num_copies_kbd.54");
                        break;

                    case '7':
                        _device.ControlPanel.Press("copy_num_copies_kbd.55");
                        break;

                    case '8':
                        _device.ControlPanel.Press("copy_num_copies_kbd.56");
                        break;

                    case '9':
                        _device.ControlPanel.Press("copy_num_copies_kbd.57");
                        break;

                    case '0':
                        _device.ControlPanel.Press("copy_num_copies_kbd.48");
                        break;
                }
            }
        }

        /// <summary>
        /// Sets Duplex On/Off
        /// </summary>
        /// <param name="duplexValue">whether on or off</param>
        [Description("Sets duplex settings")]
        public void SetDuplexSettings(string duplexValue = "On")
        {
            try
            {
                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();
                _device.ControlPanel.PressKey(SiriusSoftKey.Right);
                _pacekeeper.Pause();
                _device.ControlPanel.WaitForActiveScreenLabel("home_screen_table_menu.9", _activeScreenTimeout);
                _device.ControlPanel.Press("home_screen_table_menu.9");
                CheckErrorScreen();
                _device.ControlPanel.WaitForActiveScreenLabel("setup_list_menu.3", _activeScreenTimeout);
                _device.ControlPanel.Press("setup_list_menu.3");
                CheckErrorScreen();
                _device.ControlPanel.WaitForActiveScreenLabel("Device_Setup_List_Menu.4", _activeScreenTimeout);
                _device.ControlPanel.Press("Device_Setup_List_Menu.4");
                CheckErrorScreen();
                _device.ControlPanel.WaitForActiveScreenLabel("Print_Settings_Menu_List_Display.3", _activeScreenTimeout);
                _device.ControlPanel.Press("Print_Settings_Menu_List_Display.3");
                CheckErrorScreen();

                if (string.Equals(duplexValue, "On"))
                {
                    _device.ControlPanel.WaitForActiveScreenLabel("View_Duplex_Display_Child.0", _activeScreenTimeout);
                    _device.ControlPanel.Press("View_Duplex_Display_Child.0");
                    CheckErrorScreen();
                }
                else
                {
                    _device.ControlPanel.WaitForActiveScreenLabel("View_Duplex_Display_Child.1", _activeScreenTimeout);
                    _device.ControlPanel.Press("View_Duplex_Display_Child.1");
                    CheckErrorScreen();
                }
                _pacekeeper.Pause();
                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();
            }
            catch
            {
                throw new DeviceWorkflowException("Failed to set Duplex Settings");
            }
        }

        /// <summary>
        /// Set tray setting for copy operation
        /// </summary>
        /// <param name="trayValue">Tray 1, 2</param>
        [Description("Sets Tray settings for copy")]
        public void SetTraySettingsForCopy(string trayValue = "Tray 1")
        {
            try
            {
                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();
                _device.ControlPanel.PressKey(SiriusSoftKey.Right);
                _pacekeeper.Pause();
                _device.ControlPanel.WaitForActiveScreenLabel("home_screen_table_menu.9", TimeSpan.FromSeconds(3));
                _device.ControlPanel.Press("home_screen_table_menu.9");
                CheckErrorScreen();
                _device.ControlPanel.WaitForActiveScreenLabel("setup_list_menu.3", _activeScreenTimeout);
                _device.ControlPanel.Press("setup_list_menu.3");
                CheckErrorScreen();
                _device.ControlPanel.WaitForActiveScreenLabel("Device_Setup_List_Menu.1", _activeScreenTimeout);
                _device.ControlPanel.Press("Device_Setup_List_Menu.1");
                CheckErrorScreen();
                _device.ControlPanel.WaitForActiveScreenLabel("paper_handling_list_menu.1", _activeScreenTimeout);
                _device.ControlPanel.Press("paper_handling_list_menu.1");
                CheckErrorScreen();
                _device.ControlPanel.WaitForActiveScreenLabel("default_tray_cc.0", _activeScreenTimeout);
                _device.ControlPanel.Press("default_tray_cc.0");
                CheckErrorScreen();

                if (trayValue.Equals("Default Tray"))
                {
                    _device.ControlPanel.WaitForActiveScreenLabel("Default_Tray_List_For_Copy_tray_options.0", _activeScreenTimeout);
                    _device.ControlPanel.Press("Default_Tray_List_For_Copy_tray_options.0");
                    CheckErrorScreen();
                }
                else if (trayValue.Equals("Tray 1"))
                {
                    _device.ControlPanel.WaitForActiveScreenLabel("Default_Tray_List_For_Copy_tray_options.1", _activeScreenTimeout);
                    _device.ControlPanel.Press("Default_Tray_List_For_Copy_tray_options.1");
                    CheckErrorScreen();
                }
                else if (trayValue.Equals("Tray 2"))
                {
                    _device.ControlPanel.WaitForActiveScreenLabel("Default_Tray_List_For_Copy_tray_options.2", _activeScreenTimeout);
                    _device.ControlPanel.Press("Default_Tray_List_For_Copy_tray_options.2");
                    CheckErrorScreen();
                }
                _pacekeeper.Pause();
                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();
            }
            catch
            {
                throw new DeviceWorkflowException("Failed to set Tray settings");
            }
        }

        /// <summary>
        /// Set tray settings for plug and print
        /// </summary>
        /// <param name="trayValue">Tray 1, 2</param>
        [Description("Sets tray setting for Plug and Print")]
        public void SetTraySettingsForPlugandPrint(string trayValue = "Tray 1")
        {
            try
            {
                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();
                _device.ControlPanel.PressKey(SiriusSoftKey.Right);
                _pacekeeper.Pause();
                _device.ControlPanel.WaitForActiveScreenLabel("home_screen_table_menu.9", _activeScreenTimeout);
                _device.ControlPanel.Press("home_screen_table_menu.9");
                CheckErrorScreen();
                _device.ControlPanel.WaitForActiveScreenLabel("setup_list_menu.3", _activeScreenTimeout);
                _device.ControlPanel.Press("setup_list_menu.3");
                CheckErrorScreen();
                _device.ControlPanel.WaitForActiveScreenLabel("Device_Setup_List_Menu.1", _activeScreenTimeout);
                _device.ControlPanel.Press("Device_Setup_List_Menu.1");
                CheckErrorScreen();
                _device.ControlPanel.WaitForActiveScreenLabel("paper_handling_list_menu.1", _activeScreenTimeout);
                _device.ControlPanel.Press("paper_handling_list_menu.1");
                CheckErrorScreen();
                _device.ControlPanel.WaitForActiveScreenLabel("default_tray_cc.1", _activeScreenTimeout);
                _device.ControlPanel.Press("default_tray_cc.1");
                CheckErrorScreen();

                if (trayValue.Equals("Default Tray"))
                {
                    _device.ControlPanel.WaitForActiveScreenLabel("Default_Tray_List_For_USB_tray_options.0", _activeScreenTimeout);
                    _device.ControlPanel.Press("Default_Tray_List_For_USB_tray_options.0");
                    CheckErrorScreen();
                }
                else if (trayValue.Equals("Tray 1"))
                {
                    _device.ControlPanel.WaitForActiveScreenLabel(" Default_Tray_List_For_USB_tray_options.1", _activeScreenTimeout);
                    _device.ControlPanel.Press(" Default_Tray_List_For_USB_tray_options.1");
                    CheckErrorScreen();
                }
                else if (trayValue.Equals("Tray 2"))
                {
                    _device.ControlPanel.WaitForActiveScreenLabel("Default_Tray_List_For_USB_tray_options.2", _activeScreenTimeout);
                    _device.ControlPanel.Press("Default_Tray_List_For_USB_tray_options.2");
                    CheckErrorScreen();
                }
                _pacekeeper.Pause();
                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();
            }
            catch
            {
                throw new DeviceWorkflowException("Failed to set Tray settings");
            }
        }

        /// <summary>
        /// ControlPanel SignOut
        /// </summary>
        [Description("Signs out the current logged in user")]
        public void SignOut()
        {
            _device.ControlPanel.PressKey(SiriusSoftKey.Home);
            _pacekeeper.Pause();

            if (_device.ControlPanel.WaitForActiveScreenLabel("view_home", TimeSpan.FromSeconds(20)))
            {
                if (_device.ControlPanel.GetScreenInfo().Widgets.Find("home_oxpd_sign_in_out").HasValue("Sign Out"))
                {
                    //Press on Signout
                    _device.ControlPanel.PressByValue("Sign Out", StringMatch.StartsWith);
                    _pacekeeper.Pause();
                    //Confirm on sign out
                    _device.ControlPanel.WaitForActiveScreenLabel("view_oxpd_signout",
                        TimeSpan.FromSeconds(5));
                    _device.ControlPanel.Press("gr_footer_yes_no.1");
                    _pacekeeper.Pause();
                    //Signout functionality
                }
                else
                {
                    throw new DeviceWorkflowException("Sign in must be required");
                }
            }
            else
            {
                throw new DeviceWorkflowException("Control doesnt return to home page");
            }
        }

        /// <summary>
        /// Validates the low level ink warning for sirius devices
        /// </summary>
        /// <param name="cyan">value for cyan</param>
        /// <param name="magenta">value for magenta</param>
        /// <param name="yellow">value for yellow</param>
        /// <param name="black">value for black</param>
        [Description("Validates ink threshold levels for Cyan, Magenta, Yellow and Black")]
        public void ValidateLowCartridgeThreshold(string cyan = "10", string magenta = "10", string yellow = "10", string black = "10")
        {
            bool inkStatus = true;
            const string inkScreenLabelPrefix = "view_Ink_Low_Warning_Level_Custom_";

            if (!cyan.EndsWith("%"))
            {
                cyan += "%";
            }
            if (!magenta.EndsWith("%"))
            {
                magenta += "%";
            }
            if (!yellow.EndsWith("%"))
            {
                yellow += "%";
            }
            if (!black.EndsWith("%"))
            {
                black += "%";
            }

            _device.ControlPanel.PressKey(SiriusSoftKey.Home);
            _pacekeeper.Pause();

            _device.ControlPanel.PressKey(SiriusSoftKey.Right);
            _pacekeeper.Pause();

            _device.ControlPanel.PressByValue("Setup");
            _pacekeeper.Pause();

            _device.ControlPanel.PressByValue("Device Setup|");
            _pacekeeper.Pause();

            _device.ControlPanel.PressByValue("Ink Low Warning Level|");
            _pacekeeper.Pause();

            for (int i = 0; i < 4; i++)
            {
                _device.ControlPanel.Press($"Ink_Low_Warning_Level_List_Item_View_Child.{i}");
                _pacekeeper.Pause();

                string screenLabel = _device.ControlPanel.GetScreenInfo().ScreenLabels.First();
                switch (screenLabel.Substring(inkScreenLabelPrefix.Length, 1))
                {
                    case "C":
                        {
                            if (cyan != _device.ControlPanel.GetScreenInfo().Widgets.First(x => x.Id.Equals("Ink_Low_Warning_Level_Custom_C_Touch_Dial.1")).Values.First().Value)
                            {
                                inkStatus = false;
                            }
                            _device.ControlPanel.Press("Ink_Low_Warning_Level_Custom_C_Menu_Footer.0");
                            _pacekeeper.Pause();
                        }
                        break;

                    case "M":
                        {
                            if (magenta != _device.ControlPanel.GetScreenInfo().Widgets.First(x => x.Id.Equals("Ink_Low_Warning_Level_Custom_M_Touch_Dial_Third.1")).Values.First().Value)
                            {
                                inkStatus = false;
                            }
                            _device.ControlPanel.Press("Ink_Low_Warning_Level_Custom_M_Menu_Footer_Third.0");
                            _pacekeeper.Pause();
                        }
                        break;

                    case "Y":
                        {
                            if (yellow != _device.ControlPanel.GetScreenInfo().Widgets.First(x => x.Id.Equals("Ink_Low_Warning_Level_Custom_Y_Touch_Dial.1")).Values.First().Value)
                            {
                                inkStatus = false;
                            }
                            _device.ControlPanel.Press("Ink_Low_Warning_Level_Custom_Y_Menu_Footer.0");
                            _pacekeeper.Pause();
                        }
                        break;

                    case "K":
                        {
                            if (black != _device.ControlPanel.GetScreenInfo().Widgets.First(x => x.Id.Equals("Ink_Low_Warning_Level_Custom_K_Touch_Dial.1")).Values.First().Value)
                            {
                                inkStatus = false;
                            }
                            _device.ControlPanel.Press("Ink_Low_Warning_Level_Custom_K_Menu_Footer.0");
                            _pacekeeper.Pause();
                        }
                        break;
                }
            }

            if (!inkStatus)
            {
                throw new DeviceWorkflowException("Ink levels did not match");
            }
        }

        /// <summary>
        /// windowsAuthenticatinforCopyApp
        /// </summary>
        [Description("Performs Windows Authentication for Copy")]
        public void WindowsAuthenticationforCopyApp(string password)
        {
            _device.ControlPanel.PressKey(SiriusSoftKey.Home);
            _device.ControlPanel.PressByValue("Copy");
            _pacekeeper.Pause();
            _device.ControlPanel.Press("windows_login_username_entry");
            _pacekeeper.Pause();
            _device.ControlPanel.SetValue("ldap_windows_login_username_keyboard.4", _credential.UserName);
            _pacekeeper.Pause();
            _device.ControlPanel.Press("windows_login_password_entry_password");
            _pacekeeper.Pause();
            _device.ControlPanel.SetValue("windows_password_entry_password_keyboard.4", password);
            _pacekeeper.Pause();
            _device.ControlPanel.Press("native_login_footer.0");
            _pacekeeper.Pause();
            _device.ControlPanel.WaitForActiveScreenLabel("view_oxpd_auth_result", TimeSpan.FromSeconds(150));

            Framework.Logger.LogInfo(_device.ControlPanel.GetScreenInfo().Widgets.Find("oxpd_auth_result_msg_header.1").HasValue("Login Unsuccessful") ? "Login Unsuccessful" : "Login successful");
        }
    }
}
