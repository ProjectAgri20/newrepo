using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;

namespace HP.ScalableTest.Plugin.ControlPanel
{
    /// <summary>
    /// Class for returning HTTP variables
    /// </summary>

    internal class SiriusTriptaneWorkFlow
    {
        private readonly SiriusUIv3Device _device;
        private readonly TimeSpan _activeScreenTimeout = TimeSpan.FromSeconds(2);
        private readonly NetworkCredential _credential;
        private SiriusUIv3ControlPanel _controlPanel;
        private readonly Pacekeeper _pacekeeper = new Pacekeeper(TimeSpan.FromSeconds(2));

        //Need to remove this once the HOME press is implemented in DAT dll

        //End of removal

        public SiriusTriptaneWorkFlow(SiriusUIv3Device device, NetworkCredential credential)
        {
            _device = device;
            _credential = credential;
        }

        /// <summary>
        /// Launch all Authentication Task through the Plugin.CommonWorkFlowCandidates
        /// </summary>
        /// <param name="appName"></param>
        [Description("Performs Authentication")]
        public void Authentication(string appName)
        {
            // Create Authenticator
            IAuthenticator auth = AuthenticatorFactory.Create(_device, _credential, AuthenticationProvider.Auto);

            var preparationManager = new SiriusUIv3PreparationManager(_device);
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
                        _device.ControlPanel.ScrollPressByValue("sfolderview_p", "Apps", StringMatch.StartsWith);
                        if (_device.ControlPanel.WaitForScreenLabel("Home", TimeSpan.FromSeconds(30)))
                        {
                            string displayedTitle = GetButtonDisplayedTitle(appName);

                            _device.ControlPanel.ScrollToItemByValue("_s", displayedTitle);
                            _device.ControlPanel.PressByValue(displayedTitle);

                            _device.ControlPanel.WaitForScreenLabel("vw_sips_apps_state", TimeSpan.FromSeconds(30));
                            auth.Authenticate();
                        }
                        break;

                    default:
                        AuthenticationHelper.LaunchApp(_device, appName, auth);
                        break;
                }
            }
            catch (Exception ex)
            {
                var currentScreen = _device.ControlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault();
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
                    // ignored
                }
            }
        }

        /// <summary>
        /// Gets the button displayed title based on an assumed max length with "..." at the end
        /// </summary>
        /// <param name="buttonTitle">The button title.</param>
        /// <param name="maxDisplayLength">Maximum length of the display.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string GetButtonDisplayedTitle(string buttonTitle, int maxDisplayLength = 14)
        {
            var result = buttonTitle;
            if (_device.ControlPanel.WaitForWidgetByValue(buttonTitle, StringMatch.Exact) == null)
            {
                if (buttonTitle.Length >= maxDisplayLength && !buttonTitle.EndsWith("..."))
                {
                    result = buttonTitle.Substring(0, maxDisplayLength - 3) + "...";
                }
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
        /// Copies Document
        /// </summary>
        /// <param name="copiesCount">Number of copies</param>
        /// <param name="trayType">Tray selection</param>
        /// <param name="sidesSelection">Sides Selection</param>
        /// <param name="colorOption">Print Option</param>
        [Description("Performs Document copy")]
        public void DocumentCopy(int copiesCount, Trays trayType, SideOption sidesSelection, ColorOption colorOption)
        {
            _controlPanel = _device.ControlPanel;
            //Clicking on Home
            _controlPanel.PressKey(SiriusSoftKey.Home);
            _pacekeeper.Pause();
            try
            {
                _controlPanel.ScrollPressByValue("sfolderview_p", "Copy");
                _pacekeeper.Pause();

                _controlPanel.PressByValue("Document");
                _pacekeeper.Pause();

                if (!_controlPanel.WaitForScreenLabel("Copy_Home", _activeScreenTimeout))
                {
                    if (_controlPanel.GetScreenInfo().ScreenLabels.Contains("AnA_Login_With_Windows_Authentication") || _controlPanel.GetScreenInfo().ScreenLabels.Contains("AnA_Login_With_LDAP_Authentication"))
                    {
                        throw new DeviceWorkflowException("Document Copy is Protected, Please sign-in first");
                    }
                }

                try
                {
                    _controlPanel.Press("copies");
                    _pacekeeper.Pause();

                    foreach (var copychar in copiesCount.ToString())
                    {
                        _controlPanel.PressByValue(copychar.ToString());
                        _pacekeeper.Pause();
                    }
                    _controlPanel.Press("_done");
                    _pacekeeper.Pause();
                }
                catch
                {
                    throw new DeviceWorkflowException("Setting Copies Count failed");
                }

                for (int i = 1; i < 4; i++)
                {
                    string widgetId = $"item{i}";

                    try
                    {
                        _controlPanel.Press(widgetId);
                        _pacekeeper.Pause();

                        var screenLabel = _controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault();

                        switch (screenLabel)
                        {
                            case "Copy_Settings_TwoSidedCopy":
                                {
                                    SelectSides(sidesSelection);
                                }
                                break;

                            case "Global_TrayAndPaper":
                                {
                                    SelectTray(trayType);
                                }
                                break;

                            case "Copy_Settings_ColorOptions":
                                {
                                    SelectColour(colorOption);
                                }
                                break;
                        }
                    }
                    catch (ElementNotFoundException)
                    {
                        //ignore and continue
                    }
                }

                //try
                //{
                //    _controlPanel.Press("item1");//Sides selection
                //    _controlPanel.WaitForScreenLabel("Copy_Settings_TwoSidedCopy", _activeScreenTimeout);
                //    switch (sidesSelection)
                //    {
                //        case SideOption.Side1To1:
                //            _controlPanel.Press("model.CopyTwoSidedMenuModel.0");
                //            break;

                //        case SideOption.Side1To2:
                //            _controlPanel.Press("model.CopyTwoSidedMenuModel.1");
                //            break;

                //        case SideOption.Side2To1:
                //            _controlPanel.Press("model.CopyTwoSidedMenuModel.2");
                //            break;

                //        case SideOption.Side2To2:
                //            _controlPanel.Press("model.CopyTwoSidedMenuModel.3");
                //            break;

                //        default:
                //            _controlPanel.Press("model.CopyTwoSidedMenuModel.0");
                //            break;
                //    }

                //    _pacekeeper.Pause();
                //}
                //catch
                //{
                //    throw new DeviceWorkflowException("Setting Sides selection failed");
                //}

                //_controlPanel.WaitForScreenLabel("Copy_Home", _activeScreenTimeout);
                //try
                //{
                //    _controlPanel.Press("item2"); //tray selection CopyTraySizeTypeMenuModel.0
                //    _pacekeeper.Pause();
                //    if (_controlPanel.WaitForScreenLabel("Global_TrayAndPaper", _activeScreenTimeout))
                //    {
                //        switch (trayType)
                //        {
                //            case Trays.DefaultTray:
                //                _controlPanel.Press("model.CopyTraySizeTypeMenuModel.0");
                //                break;

                //            case Trays.Tray1:
                //                _controlPanel.Press("model.CopyTraySizeTypeMenuModel.1");
                //                break;

                //            case Trays.Tray2:
                //                _controlPanel.Press("model.CopyTraySizeTypeMenuModel.2");
                //                break;
                //        }

                //        _pacekeeper.Pause();
                //        if (_controlPanel.WaitForScreenLabel("MediaHandling_LoadPaper_MultiTray",
                //            _activeScreenTimeout))
                //        {
                //            _controlPanel.Press("fb_done");
                //        }
                //        _pacekeeper.Pause();
                //    }
                //    else
                //    {
                //        _controlPanel.PressKey(SiriusSoftKey.Back);
                //        _pacekeeper.Pause();
                //    }
                //}
                //catch (ElementNotFoundException)
                //{
                //    //due to a bug in sirius firmware, item2 is missing and can be ignored for time being
                //    ExecutionServices.SystemTrace.LogDebug("Ignoring this issue due to a bug in Sirius firmware for Monochrome devices");
                //}
                //catch
                //{
                //    throw new DeviceWorkflowException("Setting Tray failed");
                //}

                //_controlPanel.WaitForScreenLabel("Copy_Home", _activeScreenTimeout);
                //try
                //{
                //    _controlPanel.Press("item3");
                //    _pacekeeper.Pause();
                //    if (_controlPanel.WaitForScreenLabel("Copy_Settings_ColorOptions", _activeScreenTimeout))
                //    {
                //        switch (colorOption)
                //        {
                //            case ColorOption.AutoDetect:
                //                _controlPanel.Press("model.CopyColorMenuModel.0");
                //                break;

                //            case ColorOption.Color:
                //                _controlPanel.Press("model.CopyColorMenuModel.1");
                //                break;

                //            case ColorOption.Black:
                //                _controlPanel.Press("model.CopyColorMenuModel.2");
                //                break;

                //            default:
                //                _controlPanel.Press("model.CopyColorMenuModel.0");
                //                break;
                //        }
                //    }
                //    else
                //    {
                //        _controlPanel.PressKey(SiriusSoftKey.Back);
                //    }
                //    _pacekeeper.Pause();
                //}
                //catch (ElementNotFoundException)
                //{
                //    //monochrome devices will not have colour option and can be ignored
                //    ExecutionServices.SystemTrace.LogDebug("Monochrome devices will not have colour option. Ignoring...");
                //}

                _controlPanel.WaitForScreenLabel("Copy_Home", _activeScreenTimeout);
                _controlPanel.Press("fb_copy_start");
                _pacekeeper.Pause();
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Document Copy failed with exception: {ex.Message}");
            }
        }

        private void SelectColour(ColorOption colorOption)
        {
            try
            {
                switch (colorOption)
                {
                    case ColorOption.AutoDetect:
                        _controlPanel.Press("model.CopyColorMenuModel.0");
                        break;

                    case ColorOption.Color:
                        _controlPanel.Press("model.CopyColorMenuModel.1");
                        break;

                    case ColorOption.Black:
                        _controlPanel.Press("model.CopyColorMenuModel.2");
                        break;

                    default:
                        _controlPanel.Press("model.CopyColorMenuModel.0");
                        break;
                }

                _pacekeeper.Pause();
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Setting Colour option failed");
            }
        }

        private void SelectTray(Trays trayType)
        {
            try
            {
                switch (trayType)
                {
                    case Trays.DefaultTray:
                        _controlPanel.Press("model.CopyTraySizeTypeMenuModel.0");
                        break;

                    case Trays.Tray1:
                        _controlPanel.Press("model.CopyTraySizeTypeMenuModel.1");
                        break;

                    case Trays.Tray2:
                        _controlPanel.Press("model.CopyTraySizeTypeMenuModel.2");
                        break;
                }

                _pacekeeper.Pause();
                if (_controlPanel.WaitForScreenLabel("MediaHandling_LoadPaper_MultiTray",
                    _activeScreenTimeout))
                {
                    _controlPanel.Press("fb_done");
                }

                _pacekeeper.Pause();
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Setting Tray failed");
            }
        }

        private void SelectSides(SideOption sidesSelection)
        {
            try
            {
                switch (sidesSelection)
                {
                    case SideOption.Side1To1:
                        _controlPanel.Press("model.CopyTwoSidedMenuModel.0");
                        break;

                    case SideOption.Side1To2:
                        _controlPanel.Press("model.CopyTwoSidedMenuModel.1");
                        break;

                    case SideOption.Side2To1:
                        _controlPanel.Press("model.CopyTwoSidedMenuModel.2");
                        break;

                    case SideOption.Side2To2:
                        _controlPanel.Press("model.CopyTwoSidedMenuModel.3");
                        break;

                    default:
                        _controlPanel.Press("model.CopyTwoSidedMenuModel.0");
                        break;
                }

                _pacekeeper.Pause();
            }
            catch (Exception)
            {
                throw new DeviceWorkflowException("Setting Sides selection failed");
            }
        }

        /// <summary>
        /// HPAC Login with Pic Authentication
        /// </summary>
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
                if (_device.ControlPanel.GetScreenInfo().Widgets.Find("textview").Values.First().Value.Contains("The printer could not connect to the Internet."))
                {
                    _device.ControlPanel.PressByValue("Retry");
                    _pacekeeper.Pause();
                    throw new DeviceWorkflowException("Unable to connect to HPAC server");
                }
                if (_device.ControlPanel.WaitForScreenLabel("vw_sips_apps_state", TimeSpan.FromSeconds(40)))
                {
                    if (_device.ControlPanel.GetScreenInfo().Widgets.Find("sips_app_screen_header").Values.ElementAt(1).Value.Contains("Code Required"))
                    {
                        _device.ControlPanel.SetValue("object0", _credential.UserName.Substring(1));
                        _device.ControlPanel.Press("fb_footerRight");
                        _pacekeeper.Pause();

                        if (_device.ControlPanel.GetScreenInfo().ScreenLabels.Contains("Unsuccessful"))
                        {
                            throw new DeviceWorkflowException("Login code incorrect");
                        }
                    }
                    else
                    {
                        if (_device.ControlPanel.GetScreenInfo().Widgets.Find("sips_app_screen_header").Values.ElementAt(1).Value.Contains(_credential.UserName, StringComparison.OrdinalIgnoreCase))
                        {
                            _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                            _pacekeeper.Pause();
                        }
                        else
                        {
                            throw new DeviceWorkflowException("Different user signed in");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"HPAC navigation failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// HPAC SignIn with Pic Authentication
        /// </summary>
        [Description("Performs HPAC authentication with PIC")]
        public void HPACSignInWithPICAuthentication()
        {
            try
            {
                //Press home sceeen
                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();
                //Press SignIn button
                _device.ControlPanel.PressByValue("Sign In");
                _pacekeeper.Pause();
                //// HPAC authenticiation uses a unique personal identification code (PIC) that has been assigned to each user.  Our convention is that it's the username with the u lopped off
                // e.g. u00001  =>  00001; u00038 => 00038
                _device.ControlPanel.SetValue("object0", _credential.UserName.Substring(1));
                _pacekeeper.Pause();
                //Press Ok Button
                _device.ControlPanel.PressByValue("Ok");
                _pacekeeper.Pause();
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"HPAC navigation failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Copies ID Card
        /// </summary>
        /// <param name="copiesCount">Number of copies</param>
        /// <param name="trayType">Tray number</param>
        /// <param name="colorOption">Print Option</param>
        [Description("Performs ID Card Copy")]
        public void IdCardCopy(int copiesCount, Trays trayType, ColorOption colorOption)
        {
            _controlPanel = _device.ControlPanel;
            //Clicking on Home
            _controlPanel.PressKey(SiriusSoftKey.Home);
            _pacekeeper.Pause();
            try
            {
                _controlPanel.ScrollPressByValue("sfolderview_p", "Copy");
                _pacekeeper.Pause();

                _controlPanel.PressByValue("ID Card");
                _pacekeeper.Pause();

                _controlPanel.WaitForScreenLabel("Copy_IDCopy_Home");
                try
                {
                    _controlPanel.Press("copies");
                    _pacekeeper.Pause();

                    foreach (var copychar in copiesCount.ToString())
                    {
                        _controlPanel.PressByValue(copychar.ToString());
                        _pacekeeper.Pause();
                    }

                    _controlPanel.Press("_done");
                    _pacekeeper.Pause();
                }
                catch
                {
                    throw new DeviceWorkflowException("Setting Copies Count failed");
                }

                for (int i = 2; i < 4; i++)
                {
                    string widgetId = $"item{i}";

                    _controlPanel.WaitForScreenLabel("Copy_IDCopy_Home");
                    try
                    {
                        _controlPanel.Press(widgetId);
                        _pacekeeper.Pause();

                        var screenLabel = _controlPanel.GetScreenInfo().ScreenLabels.FirstOrDefault();

                        switch (screenLabel)
                        {
                            case "Global_TrayAndPaper":
                                {
                                    SelectTray(trayType);
                                }
                                break;

                            case "Copy_Settings_ColorOptions":
                                {
                                    SelectColour(colorOption);
                                }
                                break;
                        }
                    }
                    catch (ElementNotFoundException)
                    {
                        //ignore and continue
                    }
                }

                _controlPanel.WaitForScreenLabel("Copy_IDCopy_Home");
                _controlPanel.Press("fb_copy_start");
                _pacekeeper.Pause();

                //the wizard will appear now
                _controlPanel.WaitForScreenLabel("Copy_IDCard_Start", TimeSpan.FromSeconds(5));
                _controlPanel.Press("fb_action");
                _controlPanel.WaitForScreenLabel("Copy_IDCard_FlipCard", TimeSpan.FromSeconds(20));
                _controlPanel.Press("fb_action");
                _pacekeeper.Pause();
                _controlPanel.WaitForScreenLabel("Copy_IDCopy_Home", TimeSpan.FromSeconds(20));
                _controlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();
            }
            catch
            {
                throw new DeviceWorkflowException("ID Card Scan failed");
            }

            //    _controlPanel.Press("item2"); //tray selection CopyTraySizeTypeMenuModel.0
            //    _pacekeeper.Pause();
            //    if (_controlPanel.WaitForScreenLabel("Global_TrayAndPaper", _activeScreenTimeout))
            //    {
            //        switch (trayType)
            //        {
            //            case Trays.DefaultTray:
            //                _controlPanel.Press("model.CopyTraySizeTypeMenuModel.0");
            //                break;

            //            case Trays.Tray1:
            //                _controlPanel.Press("model.CopyTraySizeTypeMenuModel.1");
            //                break;

            //            case Trays.Tray2:
            //                _controlPanel.Press("model.CopyTraySizeTypeMenuModel.2");
            //                break;
            //        }

            //        _pacekeeper.Pause();
            //        if (_controlPanel.WaitForScreenLabel("MediaHandling_LoadPaper_MultiTray", _activeScreenTimeout))
            //        {
            //            _controlPanel.Press("fb_done");
            //        }

            //        _pacekeeper.Pause();
            //    }
            //    else
            //    {
            //        _controlPanel.PressKey(SiriusSoftKey.Back);
            //        _pacekeeper.Pause();
            //    }
            //}
            //catch (ElementNotFoundException)
            //{
            //    //due to a bug in sirius firmware, item2 is missing and can be ignored for time being
            //    ExecutionServices.SystemTrace.LogDebug("Ignoring this issue due to a bug in Sirius firmware for Monochrome devices");
            //}
            //catch
            //{
            //    throw new DeviceWorkflowException("Setting Tray failed");
            //}

            //_controlPanel.WaitForScreenLabel("Copy_IDCopy_Home");
            //try
            //{
            //    _controlPanel.Press("item3");
            //    _pacekeeper.Pause();
            //    if (_controlPanel.WaitForScreenLabel("Copy_Settings_ColorOptions", _activeScreenTimeout))
            //    {
            //        switch (colorOption)
            //        {
            //            case ColorOption.AutoDetect:
            //                _controlPanel.Press("model.CopyColorMenuModel.0");
            //                break;

            //            case ColorOption.Color:
            //                _controlPanel.Press("model.CopyColorMenuModel.1");
            //                break;

            //            case ColorOption.Black:
            //                _controlPanel.Press("model.CopyColorMenuModel.2");
            //                break;

            //            default:
            //                _controlPanel.Press("model.CopyColorMenuModel.0");
            //                break;
            //        }
            //    }
            //    else
            //    {
            //        _controlPanel.PressKey(SiriusSoftKey.Back);
            //    }

            //    _pacekeeper.Pause();
            //}
            //catch (ElementNotFoundException)
            //{
            //    //monochrome devices will not have colour option and can be ignored
            //    ExecutionServices.SystemTrace.LogDebug("Monochrome devices will not have colour option. Ignoring...");
            //}
        }



        /// <summary>
        /// Prints pdf docs from USB device
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="printOption">Print Option</param>
        [Description("Performs Print from USB")]
        public void PrintFromUsb(string fileName, string printOption)
        {
            _controlPanel = _device.ControlPanel;
            //Clicking on Home
            _controlPanel.PressKey(SiriusSoftKey.Home);
            _pacekeeper.Pause();
            try
            {
                _controlPanel.ScrollPressByValue("sfolderview_p", "Print");
                _pacekeeper.Pause();

                _controlPanel.PressByValue("USB Documents");
                _pacekeeper.Pause();

                _controlPanel.WaitForScreenLabel("USBPrint_SelectItem1");

                _controlPanel.Press("fb_search");
                _pacekeeper.Pause();

                _controlPanel.SetValue("lineedit", fileName);

                _controlPanel.Press("text.123?");
                _pacekeeper.Pause();

                _controlPanel.PressByValue("Find");
                _pacekeeper.Pause();

                if (_controlPanel.WaitForScreenLabel("USBPrint_SelectItem1"))
                {
                    _controlPanel.Press("model.0");
                    _pacekeeper.Pause();
                }
                else
                {
                    throw new DeviceWorkflowException($"Unable to Print. File: {fileName} Not Found");
                }

                if (string.Equals(printOption, "Color", StringComparison.CurrentCultureIgnoreCase))
                {
                    _controlPanel.Press("fb_color");
                }
                else
                {
                    _controlPanel.Press("fb_black");
                }

                _pacekeeper.Pause();

                //Clicking on Home
                _controlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();
            }
            catch
            {
                throw new DeviceWorkflowException("Print From USB Failed");
            }
        }

        /// <summary>
        /// restarts the device
        /// </summary>
        [Description("Restarts the device")]
        public void RestartDevice()
        {
            Snmp snmp = new Snmp(_device.Address);
            try
            {
                snmp.Set("1.3.6.1.2.1.43.5.1.1.3", 4);
            }
            catch (SnmpException ex)
            {
                throw new DeviceWorkflowException("Not able to restart printer", ex);
            }
        }

        /// <summary>
        /// Scan to sharepoint path
        /// </summary>
        /// <param name="sharePointFolderName">folder name</param>
        [Description("Performs Scan to Sharepoint")]
        public void ScanToSharepoint(string sharePointFolderName)
        {
            _controlPanel = _device.ControlPanel;

            try
            {
                // Clicking on Home
                _controlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();

                _controlPanel.ScrollPressByValue("sfolderview_p", "Scan");
                _pacekeeper.Pause();

                _controlPanel.WaitForScreenLabel("Home");
                _pacekeeper.Pause();

                _controlPanel.ScrollToItemByValue("sfolderview_p", "SharePoint");
                _pacekeeper.Pause();

                _controlPanel.PressByValue("SharePoint");
                _pacekeeper.Pause();

                if (_controlPanel.WaitForScreenLabel("Scan_Sharepoint_FolderList"))
                {
                    _controlPanel.PressByValue(sharePointFolderName);
                }

                _controlPanel.WaitForScreenLabel("Scan_NetworkFolder_HomeGlass; Scan_NetworkFolder_HomeADF");

                _controlPanel.Press("fb_start_scan");

                _controlPanel.WaitForScreenLabel("Scan_AnotherPage");

                _controlPanel.Press("mdlg_option_button");
                _pacekeeper.Pause();

                //Clicking on Home
                _controlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();
            }
            catch
            {
                throw new DeviceWorkflowException("Scan to Sharepoint failed");
            }
        }

        /// <summary>
        /// Scans document to USB device
        /// </summary>
        [Description("Performs Scan to USB")]
        public void ScanToUsb()
        {
            _controlPanel = _device.ControlPanel;
            //Clicking on Home
            _controlPanel.PressKey(SiriusSoftKey.Home);
            _pacekeeper.Pause();

            try
            {
                _controlPanel.ScrollPressByValue("sfolderview_p", "Scan");
                _pacekeeper.Pause();
                _controlPanel.WaitForScreenLabel("Home");
                _controlPanel.ScrollPress("sfolderview_p", "command.scan_memory");
                _pacekeeper.Pause();

                try
                {
                    if (_controlPanel.GetScreenInfo().Widgets.Find("header").HasValue("Insert USB Device"))
                    {
                        throw new DeviceWorkflowException("Please Insert USB");
                    }
                }
                catch (ElementNotFoundException)
                {
                    //ignored
                }

                _controlPanel.WaitForScreenLabel("Scan_MemoryDevice_HomeGlass");
                _controlPanel.Press("fb_start_scan");

                if (_controlPanel.WaitForScreenLabel("Scan_AnotherPage", TimeSpan.FromSeconds(10)))
                    _controlPanel.Press("mdlg_option_button");

                _pacekeeper.Pause();
                //Clicking on Home
                _controlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();
            }
            catch
            {
                throw new DeviceWorkflowException("Scan to USB failed");
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
                //Need to enable this once the Home Press is implemented in DAT dll
                //_device.ControlPanel.PressSoftKey(SiriusSoftKey.Home);

                //Clicking on Home, need to remove this
                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();

                _device.ControlPanel.Press("_DashboardDragger");
                _pacekeeper.Pause();
                _device.ControlPanel.Press("command.cmdSetup");
                _pacekeeper.Pause();
                _device.ControlPanel.ScrollToItem("slistview", "command.printSettings");
                _pacekeeper.Pause();
                _device.ControlPanel.Press("command.printSettings");
                _pacekeeper.Pause();
                _device.ControlPanel.ScrollToItem("slistview", "command.duplex");
                _pacekeeper.Pause();
                _device.ControlPanel.Press("command.duplex");
                _pacekeeper.Pause();

                if (string.Equals(duplexValue, "Disable"))
                {
                    _device.ControlPanel.Press("model.DevstsMediaDuplexBindingModel.0");
                    CheckErrorScreen();
                }
                else if (string.Equals(duplexValue, "Long-Edge Portrait"))
                {
                    _device.ControlPanel.Press("model.DevstsMediaDuplexBindingModel.1");
                    CheckErrorScreen();
                }
                else
                {
                    _device.ControlPanel.Press("model.DevstsMediaDuplexBindingModel.2");
                    CheckErrorScreen();
                }

                _pacekeeper.Pause();
                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();
            }
            catch (DeviceCommunicationException)
            {
                throw new DeviceWorkflowException("Failed to set Duplex Settings");
            }
        }

        /// <summary>
        /// Set tray setting for copy operation
        /// </summary>
        /// <param name="trayValue">Tray 1, 2</param>
        [Description("Sets Tray settings for copy")]
        public void SetTraySettingsForCopy(Trays trayValue)
        {
            try
            {
                //Clicking on Home
                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();

                _device.ControlPanel.Press("_DashboardDragger");
                _pacekeeper.Pause();
                _device.ControlPanel.Press("command.cmdSetup");
                _pacekeeper.Pause();
                _device.ControlPanel.ScrollToItem("slistview", "command.TrayPaperMgmt");
                _pacekeeper.Pause();
                _device.ControlPanel.Press("command.TrayPaperMgmt");
                _pacekeeper.Pause();
                _device.ControlPanel.Press("command.tray_selection");
                _pacekeeper.Pause();
                _device.ControlPanel.Press("command.for_copy");
                _pacekeeper.Pause();

                try
                {
                    _device.ControlPanel.WaitForScreenLabel("Setup_Pref_Trays", _activeScreenTimeout);
                    switch (trayValue)
                    {
                        case Trays.DefaultTray:
                            _device.ControlPanel.Press("model.TraySizeTypeMenuWithDefaultModel.0");
                            break;

                        case Trays.Tray1:
                            _device.ControlPanel.Press("model.TraySizeTypeMenuWithDefaultModel.1");
                            break;

                        case Trays.Tray2:
                            _device.ControlPanel.Press("model.TraySizeTypeMenuWithDefaultModel.2");
                            break;

                        case Trays.Tray3:
                            _device.ControlPanel.Press("model.TraySizeTypeMenuWithDefaultModel.3");
                            break;

                        default:
                            throw new DeviceWorkflowException("Specified Tray not Found");
                    }
                }
                catch
                {
                    switch (trayValue)
                    {
                        case Trays.Tray1:
                            _device.ControlPanel.Press("model.TraySizeTypeMenuModel.0");
                            break;

                        case Trays.Tray2:
                            _device.ControlPanel.Press("model.TraySizeTypeMenuModel.1");
                            break;

                        case Trays.Tray3:
                            _device.ControlPanel.Press("model.TraySizeTypeMenuModel.2");
                            break;

                        default:
                            throw new DeviceWorkflowException("Specified Tray not Found");
                    }
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
        public void SetTraySettingsForPlugandPrint(Trays trayValue)
        {
            try
            {
                //Clicking on Home
                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();

                _device.ControlPanel.Press("_DashboardDragger");
                _pacekeeper.Pause();
                _device.ControlPanel.Press("command.cmdSetup");
                _pacekeeper.Pause();
                _device.ControlPanel.ScrollToItem("slistview", "command.TrayPaperMgmt");
                _pacekeeper.Pause();
                _device.ControlPanel.Press("command.TrayPaperMgmt");
                _pacekeeper.Pause();
                _device.ControlPanel.Press("command.tray_selection");
                _pacekeeper.Pause();
                _device.ControlPanel.Press("command.for_usb_print");
                _pacekeeper.Pause();
                _device.ControlPanel.WaitForScreenLabel("Setup_Pref_Trays", _activeScreenTimeout);

                try
                {
                    switch (trayValue)
                    {
                        case Trays.DefaultTray:
                            _device.ControlPanel.Press("model.TraySizeTypeMenuWithDefaultModel.0");
                            break;

                        case Trays.Tray1:
                            _device.ControlPanel.Press("model.TraySizeTypeMenuWithDefaultModel.1");
                            break;

                        case Trays.Tray2:
                            _device.ControlPanel.Press("model.TraySizeTypeMenuWithDefaultModel.2");
                            break;

                        case Trays.Tray3:
                            _device.ControlPanel.Press("model.TraySizeTypeMenuWithDefaultModel.3");
                            break;

                        default:
                            throw new DeviceWorkflowException("Specified Tray not Found");
                    }
                }
                catch
                {
                    switch (trayValue)
                    {
                        case Trays.Tray1:
                            _device.ControlPanel.Press("model.TraySizeTypeMenuModel.0");
                            break;

                        case Trays.Tray2:
                            _device.ControlPanel.Press("model.TraySizeTypeMenuModel.1");
                            break;

                        case Trays.Tray3:
                            _device.ControlPanel.Press("model.TraySizeTypeMenuModel.2");
                            break;

                        default:
                            throw new DeviceWorkflowException("Specified Tray not Found");
                    }
                }

                //Clicking on Home
                _device.ControlPanel.PressKey(SiriusSoftKey.Home);
                _pacekeeper.Pause();
            }
            catch
            {
                throw new DeviceWorkflowException("Failed to set Tray settings");
            }
        }

        [Description("Signs in user from home screen")]
        public void SignIn()
        {
            _device.PowerManagement.Wake();

            _device.ControlPanel.PressKey(SiriusSoftKey.Home);
            _pacekeeper.Pause();

            if (_device.ControlPanel.WaitForScreenLabel("Home", TimeSpan.FromSeconds(20)))
            {
                if (_device.ControlPanel.GetScreenInfo().Widgets.Find("_cmdAuth").HasValue("Sign In"))
                {
                    _device.ControlPanel.Press("_cmdAuth");
                    IAuthenticator auth = AuthenticatorFactory.Create(_device, _credential, AuthenticationProvider.Auto);
                    auth.Authenticate();
                    _pacekeeper.Pause();
                    _device.ControlPanel.WaitForWidgetByValue("Sign Out");
                    if (_device.ControlPanel.GetScreenInfo().Widgets.Find("_cmdAuth").HasValue("Sign Out"))
                    {
                        ExecutionServices.SystemTrace.LogDebug("User Signed In");
                    }
                }
                else
                {
                    throw new DeviceWorkflowException("Device has an user Signed In");
                }
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

            if (_device.ControlPanel.WaitForScreenLabel("Home", TimeSpan.FromSeconds(20)))
            {
                if (_device.ControlPanel.GetScreenInfo().Widgets.Find("_cmdAuth").HasValue("Sign Out"))
                {
                    //Press on Signout
                    _device.ControlPanel.PressByValue("Sign Out", StringMatch.StartsWith);
                    _pacekeeper.Pause();
                    //Confirm on sign out
                    _device.ControlPanel.WaitForScreenLabel("view_oxpd_signout",
                        TimeSpan.FromSeconds(5));
                    _device.ControlPanel.Press("mdlg_action_button");
                    _pacekeeper.Pause();
                    //Signout functionality
                }
                else
                {
                    Framework.Logger.LogError("Sign in must be required");
                    throw new DeviceWorkflowException("Sign in must be required");
                }
            }
            else
            {
                Framework.Logger.LogError("Control doesnt return to home page");
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
        public void ValidateLowCartridgeThreshold(int cyan, int magenta, int yellow, int black)
        {
            StringBuilder errorValues = new StringBuilder();

            //Clicking on Home
            _device.ControlPanel.PressKey(SiriusSoftKey.Home);
            _pacekeeper.Pause();
            _device.ControlPanel.Press("_DashboardDragger");
            _pacekeeper.Pause();
            _device.ControlPanel.Press("command.cmdSetup");
            _pacekeeper.Pause();
            _device.ControlPanel.ScrollToItem("slistview", "command.Ink");//slistview
            _pacekeeper.Pause();
            _device.ControlPanel.Press("command.Ink");//command.lowLevel
            _pacekeeper.Pause();
            _device.ControlPanel.Press("command.lowLevel");//
            _pacekeeper.Pause();

            var widgets = _device.ControlPanel.GetScreenInfo().Widgets;

            if (widgets.FindByValue("Cyan").Values["secondarytext"] == "Automatic" ||
             widgets.FindByValue("Cyan").Values["secondarytext"] != cyan.ToString())
            {
                errorValues.AppendLine($"Cyan Specified: {cyan}, Actual: {widgets.FindByValue("Cyan").Values["secondarytext"]}");
            }
            if (widgets.FindByValue("Magenta").Values["secondarytext"] == "Automatic" ||
              widgets.FindByValue("Magenta").Values["secondarytext"] != magenta.ToString())
            {
                errorValues.AppendLine($"Magenta Specified: {magenta}, Actual: {widgets.FindByValue("Magenta").Values["secondarytext"]}");
            }
            if (widgets.FindByValue("Yellow").Values["secondarytext"] == "Automatic" ||
                widgets.FindByValue("Yellow").Values["secondarytext"] != yellow.ToString())
            {
                errorValues.AppendLine($"Yellow Specified: {yellow}, Actual: {widgets.FindByValue("Yellow").Values["secondarytext"]}");
            }
            if (widgets.FindByValue("Black").Values["secondarytext"] == "Automatic" ||
                widgets.FindByValue("Black").Values["secondarytext"] != black.ToString())
            {
                errorValues.AppendLine($"Black Specified: {black}, Actual: {widgets.FindByValue("Black").Values["secondarytext"]}");
            }

            if (!string.IsNullOrEmpty(errorValues.ToString()))
            {
                throw new DeviceWorkflowException($"Follow ink values didn't match the threshold: {Environment.NewLine}{errorValues}");
            }

            //Clicking on Home
            _device.ControlPanel.PressKey(SiriusSoftKey.Home);
        }

        /// <summary>
        /// Select Language in Control Panel
        /// </summary>
        /// <param name="language"></param>
        [Description("Select Language in Control Panel")]
        public void SelectLanguage(Languages language)
        {
            string code = Properties.Resources.ResourceManager.GetString(language.ToString());
            _device.ControlPanel.PressKey(SiriusSoftKey.Home);
            _pacekeeper.Pause();
            _device.ControlPanel.Press("_DashboardDragger");
            _pacekeeper.Pause();
            _device.ControlPanel.Press("command.cmdSetup");
            _pacekeeper.Pause();
            _device.ControlPanel.Press("command.Preferences");
            _pacekeeper.Pause();
            _device.ControlPanel.Press("command.Language");
            _pacekeeper.Pause();
            _device.ControlPanel.ScrollPressByValue("list", code);
            _pacekeeper.Pause();
            _device.ControlPanel.Press("mdlg_action_button");
            _pacekeeper.Pause();
            //Clicking on Home
            _device.ControlPanel.PressKey(SiriusSoftKey.Home);
        }

        public enum Languages
        {
            English, SimpleChinese, Korean, TraditionalChinese, Japanese, Dansk, Deutsch, Espanol, French, Italiano, Magyar, Norsk, Polski, Suomi, Portugues, Slovak, Nederlands, Svenska, Russian, Turkish, Greek, Czech, Romanian, Slovene, Bulgarian, Croatian
        };

        public enum Trays
        {
            DefaultTray, Tray1, Tray2, Tray3, Tray4
        }

        public enum SideOption
        {
            Side1To1, Side1To2, Side2To1, Side2To2
        }

        public enum ColorOption
        {
            AutoDetect, Color, Black
        }

        /// <summary>
        /// LDAP Authentication for Copy App
        /// </summary>
        [Description("Performs LDAP authentication for Copy application")]
        public void LdapAuthenticationforCopyApp()
        {
            _device.ControlPanel.PressKey(SiriusSoftKey.Home);
            _device.ControlPanel.ScrollPress("sfolderview_p", "group.group.copy");
            _pacekeeper.Pause();
            _device.ControlPanel.Press("command.copy_doc");
            _pacekeeper.Pause();
            _device.ControlPanel.Press("lineedit1");
            _pacekeeper.Pause();
            _device.ControlPanel.SetValue("lineedit1", _credential.UserName);
            _pacekeeper.Pause();
            _device.ControlPanel.Press("lineedit2");
            _pacekeeper.Pause();
            _device.ControlPanel.SetValue("lineedit2", _credential.Password);
            _pacekeeper.Pause();
            _device.ControlPanel.Press("_done");
            _pacekeeper.Pause();
            _device.ControlPanel.Press("fb_signIn");
            _device.ControlPanel.WaitForScreenLabel("st_auth_login_result", TimeSpan.FromSeconds(90));
            Framework.Logger.LogInfo(_device.ControlPanel.GetScreenInfo().Widgets.Find("appheader").HasValue("Copy Document") ? "Login Unsuccessful" : "Login Successful");
            _device.ControlPanel.PressKey(SiriusSoftKey.Home);
        }

        /////<summary> Windows auth for SignUp
        [Description("Performs Windows authentication at Home")]
        public void WindowsAuthenticationAtHome()
        {
            _device.ControlPanel.PressKey(SiriusSoftKey.Home);
            _device.ControlPanel.Press("_cmdAuth");
            _pacekeeper.Pause();
            _device.ControlPanel.Press("fb_alternateSignin");
            _pacekeeper.Pause();
            _device.ControlPanel.Press("model.AlternativeSignInListModel.1");
            _pacekeeper.Pause();
            _device.ControlPanel.Press("fb_butcontinue");
            _pacekeeper.Pause();
            _device.ControlPanel.Press("lineedit1");
            _pacekeeper.Pause();
            _device.ControlPanel.SetValue("lineedit1", _credential.UserName);
            _pacekeeper.Pause();
            _device.ControlPanel.Press("lineedit2");
            _pacekeeper.Pause();
            _device.ControlPanel.SetValue("lineedit2", _credential.Password);
            _pacekeeper.Pause();
            _device.ControlPanel.Press("_done");
            _pacekeeper.Pause();
            _device.ControlPanel.Press("fb_signIn");
            _device.ControlPanel.WaitForScreenLabel("st_auth_login_result", TimeSpan.FromSeconds(90));
            Framework.Logger.LogInfo(_device.ControlPanel.GetScreenInfo().Widgets.Find("_DashBoard").HasValue("Sign Out") ? "Login Unsuccessful" : "Login Successful");
        }
    }
}
