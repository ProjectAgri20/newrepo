using System;
using System.Linq;
using System.Net;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.Plugin.PSPullPrint
{
    internal class HpacSirius
    {
        private readonly SiriusUIv2Device _siriusDevice;
        private readonly TimeSpan _shortDelay = TimeSpan.FromSeconds(2);
        private readonly TimeSpan _longDelay = TimeSpan.FromSeconds(60);
        private readonly NetworkCredential _credential;
        public HpacSirius(IDevice device, NetworkCredential credential)
        {
            //Getting the Sirius Device
            _siriusDevice = device as SiriusUIv2Device;
            _credential = credential;
        }

        /// <summary>
        /// Gets the device control panel to the HP AC screen using the PIC authentication
        /// </summary>
        /// <returns></returns>
        public void NavigateToHpac()
        {
            try
            {
                _siriusDevice.PowerManagement.Wake();
                Thread.Sleep(_shortDelay);
                //Press home sceeen
                _siriusDevice.ControlPanel.PressKey(SiriusSoftKey.Home);
                Thread.Sleep(_shortDelay);
                //Press App button
                _siriusDevice.ControlPanel.PressByValue("Apps");
                Thread.Sleep(_shortDelay);
                //Press HP AC
                _siriusDevice.ControlPanel.PressByValue("HP AC Secure Pull Print");
                Thread.Sleep(_shortDelay);
                Widget widgetCollection = WidgetFind(_siriusDevice, "oxpd_auth_conn_failed_msg");
                //If device is not able to connect to the HPAC Server

                if (widgetCollection != null)
                {
                    if (widgetCollection.Values.First().Value.Contains("The printer was unable to connect to the server. Check your internet connection and try again.  Would you like to retry?"))
                    {
                        // if (_siriusDevice.ControlPanel.GetScreenInfo().Widgets.Find("oxpd_auth_conn_failed_msg").HasValue(""))
                        {
                            //Press confirmation popup, this generally comes the first time the device is configured to the HP AC
                            _siriusDevice.ControlPanel.Press("gr_footer_yes_no.1");
                            Thread.Sleep(_shortDelay);
                        }
                    }
                }

                if (_siriusDevice.ControlPanel.WaitForActiveScreenLabel("view_sips_form", _longDelay))
                {
                    widgetCollection = WidgetFind(_siriusDevice, "sips_nolistorform_txtonly");
                    var widgetCollectionLoginScreen = WidgetFind(_siriusDevice, "oxpd_auth_result_msg");
                    if (widgetCollection != null)
                    {
                        if (widgetCollection.HasValue("No documents"))
                        {
                            throw new Exception("No Documents");
                        }
                    }
                    else if (widgetCollectionLoginScreen != null)
                    {
                        if (widgetCollectionLoginScreen.HasValue("Incorrect code."))
                        {
                            throw new Exception("Incorrect PIC");
                        }
                    }
                    else
                    {
                        //Handling remote jobs
                        widgetCollection = WidgetFind(_siriusDevice, "sips_common_footer.1");

                        if (widgetCollection != null)
                        {
                            _siriusDevice.ControlPanel.Press("sips_common_footer.1");
                        }
                        //Handling Incorrect PIC and documents not present for the specified user in CP

                        //Press on Enter Password text box
                        _siriusDevice.ControlPanel.Press("sips_form_region1_value");
                        Thread.Sleep(_shortDelay);

                        //set the PIC value in the Password text box
                        _siriusDevice.ControlPanel.SetValue("sips_form_region1_kbd.4", _credential.UserName.Substring(1));
                        Thread.Sleep(_shortDelay);

                        //Press OK
                        _siriusDevice.ControlPanel.Press("sips_common_footer.0");
                        Thread.Sleep(_shortDelay);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"HPAC navigation failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Performs the HPAC tasks on the Control panel
        /// </summary>
        /// <param name="action">action to be performed</param>
        public void PerformHpacTasksOnCp(SolutionOperation action)
        {
            try
            {
                if (_siriusDevice.ControlPanel.WaitForActiveScreenLabel("view_sips_vert1colTxtOnly_multi",
                    _longDelay))
                {
                    //Getting the control ID for the file name
                    string fileID = "sips_multi_vert1colTxtOnly.0";
                    //Validating if file id is null or empty

                    switch (action)
                    {
                        case SolutionOperation.Delete:
                            {
                                //Press on the File name
                                _siriusDevice.ControlPanel.Press(fileID);
                                Thread.Sleep(_shortDelay);
                                //Press delete button
                                _siriusDevice.ControlPanel.Press("sips_common_footer_list.1");
                                Thread.Sleep(_shortDelay);
                            }
                            break;

                        case SolutionOperation.Print:
                            {
                                //Press on the File name
                                _siriusDevice.ControlPanel.Press(fileID);
                                Thread.Sleep(_shortDelay);
                                //Press Print Button
                                _siriusDevice.ControlPanel.Press("sips_common_footer_list.0");
                                Thread.Sleep(_shortDelay);
                            }
                            break;

                        case SolutionOperation.Cancel:
                            {
                                //Press on the File name
                                _siriusDevice.ControlPanel.Press(fileID);
                                Thread.Sleep(_shortDelay);
                                //Press print button
                                _siriusDevice.ControlPanel.Press("sips_common_footer_list.0");
                                Thread.Sleep(_shortDelay);
                                //Going to home screen
                                _siriusDevice.ControlPanel.PressKey(SiriusSoftKey.Home);
                                Thread.Sleep(_shortDelay);
                                //Press on Cancel print
                                _siriusDevice.ControlPanel.PressKey(SiriusSoftKey.Cancel);
                                Thread.Sleep(_shortDelay);
                                //Press on cancel on the Scrren for the specified job
                                _siriusDevice.ControlPanel.Press("concurrent_job_status_nameval.0");
                                Thread.Sleep(_shortDelay);
                            }
                            break;

                        case SolutionOperation.PrintAll:
                            {
                                _siriusDevice.ControlPanel.Press("sips_common_footer_list.2");
                                Thread.Sleep(_shortDelay);
                                _siriusDevice.ControlPanel.Press("sips_common_footer_list.0");
                                Thread.Sleep(_shortDelay);
                            }
                            break;

                        default:
                            {
                                _siriusDevice.ControlPanel.PressKey(SiriusSoftKey.Home);
                                throw new Exception($"{ (object)action} is not supported on Sirius devices");
                            }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"HPAC navigation failed with exception:{ex.Message}");
            }
        }

        /// <summary>
        /// Device Signout 
        /// </summary>
        public void SignOut()
        {
            //Going to home screen
            _siriusDevice.ControlPanel.PressKey(SiriusSoftKey.Home);
            Thread.Sleep(_shortDelay);
            //Press on Signout
            _siriusDevice.ControlPanel.Press("home_oxpd_sign_in_out");
            Thread.Sleep(_shortDelay);
            //Confirm on sign out
            _siriusDevice.ControlPanel.Press("gr_footer_yes_no.1");
            Thread.Sleep(_shortDelay);
        }


        /// <summary>
        /// Find the element in widget
        /// </summary>
        /// <param name="device"></param>
        /// <param name="widgetId"></param>
        /// <returns></returns>
        private static Widget WidgetFind(SiriusUIv2Device device, string widgetId)
        {

            WidgetCollection wc = device.ControlPanel.GetScreenInfo().Widgets;
            try
            {
                return wc.First(n => StringMatcher.IsMatch(widgetId, n.Id, StringMatch.Exact, true));
            }
            catch
            {
                return null;
            }
        }
    }
}
