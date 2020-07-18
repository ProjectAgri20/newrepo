using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.PSPullPrint
{
    internal class SafecomSiriusTriptane
    {
        private readonly SiriusUIv3Device _siriusDevice;
        private readonly Pacekeeper _pacekeeper = new Pacekeeper(TimeSpan.FromSeconds(2));
        private readonly TimeSpan _mediumDelay = TimeSpan.FromSeconds(20);
        private readonly TimeSpan _longDelay = TimeSpan.FromSeconds(60);
        private readonly NetworkCredential _credential;


        public SafecomSiriusTriptane(IDevice device, NetworkCredential credential)
        {
            _siriusDevice = device as SiriusUIv3Device;
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
                _pacekeeper.Pause();
                if (_siriusDevice.ControlPanel.WaitForScreenLabel("Home", _mediumDelay))
                {
                    if (!_siriusDevice.ControlPanel.GetScreenInfo()
                        .Widgets.Find("_cmdAuth")
                        .HasValue("Sign Out", StringMatch.Contains))
                    {
                        AuthenticateUser(pin);
                    }
                }
                else
                {
                    AuthenticateUser(pin);
                }

                if (_siriusDevice.ControlPanel.WaitForScreenLabel("Home", _mediumDelay))
                {

                    //Scroll to Apps
                    _siriusDevice.ControlPanel.ScrollToItemByValue("sfolderview_p", "Apps");
                    //Press App button
                    _siriusDevice.ControlPanel.PressByValue("Apps");

                    _siriusDevice.ControlPanel.WaitForWidgetByValue("Pull Print", _mediumDelay);

                    _siriusDevice.ControlPanel.ScrollToItemByValue("sfolderview_p", "Pull Print");
                    _pacekeeper.Pause();
                    //Press HP AC
                    _siriusDevice.ControlPanel.PressByValue("Pull Print");
                    _pacekeeper.Pause();

                    if (_siriusDevice.ControlPanel.WaitForWidget("sipsemptyscreentype", _mediumDelay) != null)
                    {
                        _siriusDevice.ControlPanel.PressKey(SiriusSoftKey.Home);
                        _pacekeeper.Pause();
                        throw new Exception("No documents");
                    }
                }
                else
                {
                    throw new Exception("There was an issue with authentication, unable to navigate to Home Screen");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Safecom authentication failed with exception:{ex.Message}");
            }
        }




        /// <summary>
        /// Authenticate User 
        /// </summary>
        private void AuthenticateUser(string pin)
        {
            //Press SignIn button
            _siriusDevice.ControlPanel.PressByValue("Sign In");
            _pacekeeper.Pause();

            var widgets = _siriusDevice.ControlPanel.GetScreenInfo().Widgets;
            if (widgets.FirstOrDefault(x => x.HasValue("Select Sign In Method")) != null)
            {
                if (widgets.FirstOrDefault(x => x.HasValue("Safecom ", StringMatch.StartsWith)) != null)
                {
                    _siriusDevice.ControlPanel.PerformAction(WidgetAction.Select, "model.AlternativeSignInListModel.1");
                    _siriusDevice.ControlPanel.PressByValue("Continue");
                    _siriusDevice.ControlPanel.WaitForWidgetByValue("Present card or enter code");
                }
            }


            //If device is not able to connect to the Safecom Server
            Widget widgetCollection = WidgetFind(_siriusDevice, "textview");

            if (widgetCollection != null)
            {
                if (widgetCollection.Values.First()
                        .Value.Contains("The printer could not connect to the Internet."))
                {
                    _siriusDevice.ControlPanel.PressByValue("Retry");
                    _pacekeeper.Pause();
                    throw new Exception("Unable to connect to Safecom server");
                }
            }

            if (_siriusDevice.ControlPanel.WaitForScreenLabel("vw_sips_apps_state", TimeSpan.FromSeconds(40)))
            {
                widgetCollection = WidgetFind(_siriusDevice, "sips_app_screen_header");
                if (widgetCollection != null)
                {
                    if (widgetCollection.HasValue("Present card or enter code", StringMatch.StartsWith))
                    {

                        _siriusDevice.ControlPanel.WaitForWidget("object0", _mediumDelay);

                        _siriusDevice.ControlPanel.SetValue("object0", _credential.UserName.Substring(1));

                        _siriusDevice.ControlPanel.Press("fb_footerRight");
                        _pacekeeper.Pause();

                        //Login Code Incorrect
                        widgets = _siriusDevice.ControlPanel.GetScreenInfo().Widgets;
                        if (widgets.FirstOrDefault(x => x.HasValue("User is unknown", StringMatch.StartsWith)) != null)
                        {
                            _siriusDevice.ControlPanel.PressKey(SiriusSoftKey.Home);
                            throw new Exception("Login code incorrect");
                        }

                        //If Pin is enabled in server this code handles
                        widgetCollection = WidgetFind(_siriusDevice, "sips_app_screen_header");
                        if (widgetCollection != null)
                        {
                            if (widgetCollection.HasValue("Please enter PIN", StringMatch.StartsWith))
                            {
                                _siriusDevice.ControlPanel.WaitForWidget("object0", _mediumDelay);

                                _siriusDevice.ControlPanel.SetValue("object0", pin);

                                _siriusDevice.ControlPanel.Press("fb_footerRight");
                                _pacekeeper.Pause();
                            }
                        }

                        //PIN Code Incorrect
                        widgets = _siriusDevice.ControlPanel.GetScreenInfo().Widgets;
                        if (widgets.FirstOrDefault(x => x.HasValue("Login Unsuccessful", StringMatch.StartsWith)) != null)
                        {
                            _siriusDevice.ControlPanel.PressByValue("OK");
                            throw new Exception("PIN code incorrect; Login denied");
                        }
                    }
                    else
                    {
                        widgetCollection = WidgetFind(_siriusDevice, "sips_app_screen_header");
                        if (widgetCollection != null)
                        {
                            if (widgetCollection.Values.ElementAt(1)
                                .Value.Contains(_credential.UserName.Substring(1), StringComparison.OrdinalIgnoreCase))
                            {
                                _siriusDevice.ControlPanel.PressKey(SiriusSoftKey.Home);
                                _pacekeeper.Pause();
                            }
                            else
                            {
                                throw new Exception("Different user signed in");
                            }
                        }
                    }
                }
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
                //Getting the control ID for the file name model.0
                string fileID = "model.0";
                //Validating if file id is null or empty
                switch (action)
                {
                    case SolutionOperation.Delete:
                        {
                            if (_siriusDevice.ControlPanel.WaitForScreenLabel("vw_sips_apps_state", _longDelay))
                            {
                                _pacekeeper.Pause();
                                //Press on the File name
                                _siriusDevice.ControlPanel.PerformAction(WidgetAction.Check, fileID);
                                _pacekeeper.Pause();
                                //Press Menu button
                                _siriusDevice.ControlPanel.Press("fb_footerCenter");
                                _pacekeeper.Pause();

                                //Press Delete button
                                _siriusDevice.ControlPanel.Press("model.sipsnamevalmapmodel.2");
                                _pacekeeper.Pause();
                            }
                            else
                            {
                                throw new Exception("Unable to navigate to Safecom main screen");
                            }
                        }
                        break;

                    case SolutionOperation.Print:
                        {
                            if (_siriusDevice.ControlPanel.WaitForScreenLabel("vw_sips_apps_state", _longDelay))
                            {
                                //Press on the File name
                                _siriusDevice.ControlPanel.PerformAction(WidgetAction.Check, fileID);
                                _pacekeeper.Pause();
                                //Press Print Button
                                _siriusDevice.ControlPanel.Press("fb_footerRight");
                                _pacekeeper.Pause();
                            }
                            else
                            {
                                throw new Exception("Unable to navigate to Safecom main screen");
                            }
                        }
                        break;

                    case SolutionOperation.Cancel:
                        {
                            if (_siriusDevice.ControlPanel.WaitForScreenLabel("vw_sips_apps_state", _longDelay))
                            {
                                _pacekeeper.Pause();
                                //Press on the File name
                                _siriusDevice.ControlPanel.PerformAction(WidgetAction.Check, fileID);
                                _pacekeeper.Pause();
                                //Press print button
                                _siriusDevice.ControlPanel.Press("fb_footerRight");
                                _pacekeeper.Pause();
                                //Press on cancel on the Scrren for the specified job
                                _siriusDevice.ControlPanel.Press("sips_status_cancel");
                                _pacekeeper.Pause();
                            }
                            else
                            {
                                throw new Exception("Unable to navigate to Safecom main screen");
                            }
                        }
                        break;

                    case SolutionOperation.PrintAll:
                        {
                            if (_siriusDevice.ControlPanel.WaitForScreenLabel("vw_sips_apps_state", _longDelay))
                            {
                                _siriusDevice.ControlPanel.Press("fb_footerLeft");
                                _pacekeeper.Pause();
                                _siriusDevice.ControlPanel.Press("fb_footerRight");
                                _pacekeeper.Pause();
                            }
                            else
                            {
                                throw new Exception("Unable to navigate to Safecom main screen");
                            }
                        }
                        break;

                    case SolutionOperation.SignOut:
                        {

                            _siriusDevice.ControlPanel.PressKey(SiriusSoftKey.Home);
                            _pacekeeper.Pause();

                            if (_siriusDevice.ControlPanel.WaitForScreenLabel("Home", _longDelay))
                            {
                                SignOut();
                            }
                            else
                            {
                                throw new Exception("Unable to navigate to Safecom main screen");
                            }
                        }
                        break;

                    default:
                        {
                            _siriusDevice.ControlPanel.PressKey(SiriusSoftKey.Home);
                            throw new Exception($"{action} is not supported on Sirius devices");
                        }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Safecom Task failed with exception: { (object)ex.Message}");
            }
        }




        /// <summary>
        /// Device Signout
        /// </summary>
        public void SignOut()
        {
            //Going to home screen
            Thread.Sleep(_mediumDelay);
            _siriusDevice.ControlPanel.PressKey(SiriusSoftKey.Home);
            _pacekeeper.Pause();
            if (_siriusDevice.ControlPanel.WaitForScreenLabel("Home", _mediumDelay))
            {
                if (_siriusDevice.ControlPanel.GetScreenInfo()
                    .Widgets.Find("_cmdAuth")
                    .HasValue("Sign Out", StringMatch.Contains))
                {

                    //Press on Signout _cmdAuth
                    Retry.WhileThrowing(() => _siriusDevice.ControlPanel.Press("_cmdAuth"),
                                                10,
                                                TimeSpan.FromSeconds(25),
                                                new List<Type>() { typeof(Exception) });

                    _pacekeeper.Pause();
                    //Confirm on sign out
                    _siriusDevice.ControlPanel.Press("mdlg_action_button");
                    _pacekeeper.Pause();
                }
                else
                {
                    throw new Exception("User has not signed in");
                }
            }
        }

        /// <summary>
        /// Find the element in widget
        /// </summary>
        /// <param name="device"></param>
        /// <param name="widgetId"></param>
        /// <returns></returns>
        private static Widget WidgetFind(SiriusUIv3Device device, string widgetId)
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