using System;
using System.Net;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.Plugin.PSPullPrint
{
    /// <summary>
    /// Class to perform Safecom Operations like authenticate,print,print all,delete on Sirius devices
    /// </summary>
    internal class SafecomSirius
    {
        private readonly SiriusUIv2Device _siriusDevice;
        private readonly SiriusUIv2ControlPanel _controlPanel;
        private readonly TimeSpan _defaultScreenWait = TimeSpan.FromSeconds(30);
        private readonly NetworkCredential _credential;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="device"></param>
        /// <param name="credential"></param>
        public SafecomSirius(IDevice device, NetworkCredential credential)
        {
            //Getting the Sirius Device
            _siriusDevice = device as SiriusUIv2Device;
            _controlPanel = _siriusDevice.ControlPanel;
            _credential = credential;
        }

        /// <summary>
        /// Gets the device control panel to the HP AC screen using the PIC authentication
        /// </summary>
        /// <param name="pin">Default Safecom User Pin</param>
        /// <returns></returns>
        public void AuthenticateSafecom(string pin)
        {
            try
            {
                _siriusDevice.PowerManagement.Wake();

                //Press home sceeen
                _controlPanel.PressKey(SiriusSoftKey.Home);
                _controlPanel.WaitForActiveScreenLabel("view_home", _defaultScreenWait);

                //Navigate to Safecom App
                _controlPanel.PressByValue("Apps");
                _controlPanel.WaitForActiveScreenLabel("view_oxpd_home", _defaultScreenWait);
                _controlPanel.ScrollToItemByValue("oxpd_home_table", "Pull Print");
                _controlPanel.PressByValue("Pull Print", StringMatch.StartsWith);
                _controlPanel.WaitForActiveScreenLabel("view_sips_form", _defaultScreenWait);
                if ((!string.IsNullOrEmpty(_controlPanel.ActiveScreenId())) && _controlPanel.ActiveScreenId().StartsWith("windowsScreen"))
                {
                    //Enter Windows Credentials
                    _controlPanel.Press("sips_form_region1_value");
                    _controlPanel.WaitForActiveScreenLabel("view_sips_form_region1_kbd", _defaultScreenWait);
                    _controlPanel.SetValue("sips_form_region1_kbd.4", _credential.UserName);//Enter Username
                    _controlPanel.WaitForActiveScreenLabel("view_sips_form", _defaultScreenWait);
                    _controlPanel.Press("sips_form_region2_value");
                    _controlPanel.SetValue("sips_form_region2_kbd.4", _credential.Password);//Enter password
                    _controlPanel.WaitForActiveScreenLabel("view_sips_form", _defaultScreenWait);
                    _controlPanel.Press("sips_form_region3_value");
                    _controlPanel.WaitForActiveScreenLabel("view_sips_form_region3_kbd", _defaultScreenWait);
                    _controlPanel.SetValue("sips_form_region3_kbd.4", _credential.Domain);//Enter domain
                    _controlPanel.WaitForActiveScreenLabel("view_sips_form", _defaultScreenWait);
                }
                else
                {
                    //Enter Safecom ID
                    _controlPanel.Press("sips_form_region1_value");
                    _controlPanel.WaitForActiveScreenLabel("view_sips_form_region1_kbd", _defaultScreenWait);
                    // SafeCom ID authenticiation uses a unique personal identification code (PIC) that has been assigned to each user.  Our convention is that it's the username with the u lopped off
                    // e.g. u00001  =>  00001; u00038 => 00038
                    _controlPanel.SetValue("sips_form_region1_kbd.4", _credential.UserName.Substring(1));
                    _controlPanel.WaitForActiveScreenLabel("view_sips_form", _defaultScreenWait);
                    _controlPanel.Press("sips_common_footer.0");
                    _controlPanel.WaitForActiveScreenLabel("view_sips_form", _defaultScreenWait);
                    _controlPanel.Press("sips_form_region1_value");
                    _controlPanel.WaitForActiveScreenLabel("view_sips_form_region1_kbd", _defaultScreenWait);
                    _controlPanel.SetValue("sips_form_region1_kbd.4", pin);//Enter PIN
                    _controlPanel.WaitForActiveScreenLabel("view_sips_form", _defaultScreenWait);
                }

                _controlPanel.Press("sips_common_footer.0");//login
            }
            catch (Exception ex)
            {
                throw new Exception($"Safecom authentication failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Performs the Safecom tasks on the Control panel
        /// </summary>
        /// <param name="action">action to be performed</param>
        public void DoSafecomAction(SolutionOperation action)
        {
            try
            {
                if (_controlPanel.WaitForActiveScreenLabel("view_sips_vert1colTxtOnly_multi", _defaultScreenWait))
                {
                    switch (action)
                    {
                        case SolutionOperation.Print://Printing single Job
                            {
                                _controlPanel.Press("sips_multi_vert1colTxtOnly.0");
                                _controlPanel.WaitForActiveScreenLabel("view_sips_vert1colTxtOnly_multi", _defaultScreenWait);
                                _controlPanel.Press("sips_common_footer_list.0");//print
                                _controlPanel.WaitForActiveScreenLabel("view_sips_nolistorform_busy_job", _defaultScreenWait);
                            }
                            break;

                        case SolutionOperation.PrintAll://Printing all Jobs
                            {
                                _controlPanel.Press("sips_common_footer_list.2");//print all
                                _controlPanel.WaitForActiveScreenLabel("view_sips_nolistorform_busy_job", _defaultScreenWait);
                            }
                            break;

                        case SolutionOperation.Delete://Deleting single Job
                            {
                                _controlPanel.Press("sips_multi_vert1colTxtOnly.0");
                                _controlPanel.WaitForActiveScreenLabel("view_sips_vert1colTxtOnly_multi", _defaultScreenWait);
                                _controlPanel.Press("sips_common_footer_list.1");//menu
                                _controlPanel.WaitForActiveScreenLabel("view_sips_horizMultiRowImgTxt_single", _defaultScreenWait);
                                _controlPanel.Press("sips_single_horizMultiRowImgTxt.2");//delete
                            }
                            break;

                        case SolutionOperation.PrintKeep://Deleting single Job
                            {
                                _controlPanel.Press("sips_multi_vert1colTxtOnly.0");
                                _controlPanel.WaitForActiveScreenLabel("view_sips_vert1colTxtOnly_multi", _defaultScreenWait);
                                _controlPanel.Press("sips_common_footer_list.1");//menu
                                _controlPanel.WaitForActiveScreenLabel("view_sips_horizMultiRowImgTxt_single", _defaultScreenWait);
                                _controlPanel.Press("Retain_controlName");//delete
                            }
                            break;

                        case SolutionOperation.SignOut:
                            SafeComSignOut();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Safecom operation failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Sign out from Safecom
        /// </summary>
        public void SafeComSignOut()
        {
            try
            {
                //Sign Out
                _controlPanel.PressKey(SiriusSoftKey.Home);
                _controlPanel.WaitForActiveScreenLabel("view_home", _defaultScreenWait);
                //Press on Signout
                _siriusDevice.ControlPanel.Press("home_oxpd_sign_in_out");
                Thread.Sleep(TimeSpan.FromSeconds(5));
                //Confirm on sign out
                _siriusDevice.ControlPanel.Press("gr_footer_yes_no.1");
            }
            catch (Exception ex)
            {
                throw new Exception($"Safecom signout failed with exception:{ex.Message}");
            }
        }
    }
}