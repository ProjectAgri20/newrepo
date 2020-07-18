using HP.SPS.SES;
using HP.SPS.SES.Helper;
using System;
using System.Threading;

namespace HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink
{
    /// <summary>
    /// Helper class that provides methods for controlling Link UI such as List
    /// </summary>
    public sealed class JetAdvantageLinkControlHelper
    {
        private readonly JetAdvantageLinkUI _linkUI;
        private readonly string _packageName;

        /// <summary>
        /// Initializes a new instance of the <see cref="JetAdvantageLinkControlHelper" /> class.
        /// </summary>
        /// <param name="linkUI">Android UI</param>
        /// <param name="packageName">Android package name to contorl</param>
        /// <exception cref="ArgumentNullException"><paramref name="linkUI" /> is null.</exception>
        public JetAdvantageLinkControlHelper(JetAdvantageLinkUI linkUI, string packageName)
        {
            if (linkUI == null)
            {
                throw new ArgumentNullException(nameof(linkUI));
            }

            _linkUI = linkUI;
            _packageName = packageName;
        }


        /// <summary>
        /// Find <paramref name="toFind"/> while scrolling up <paramref name="list"/>
        /// </summary>
        /// <param name="list">list to scroll</param>
        /// <param name="toFind">UiSelector() of object to find</param>
        /// <param name="listScrollTimes">number of time to scroll the list. Default is 5</param>
        /// <param name="clickAfterFind">give true if want to click found object. Default is false</param>
        /// <returns>success and fail</returns>
        public bool FindOnListWithScroll(UiSelector list, string toFind, int listScrollTimes = 5, bool clickAfterFind = false)
        {
            // Wait until screen contains list for timeout sec.
            int previousTimeout = _linkUI.Controller.GetTimeout();
            _linkUI.Controller.SetTimeout(5);

            if (!_linkUI.Controller.DoesScreenContains(list))
            {
                throw new DeviceWorkflowException("Can not find list to scroll");
            }            
            _linkUI.Controller.Swipe(list, SESLib.To.Down, 2);

            _linkUI.Controller.SetTimeout(0);

            for (int i = 0; i < listScrollTimes; i++)
            {
                if (_linkUI.Controller.DoesScreenContains(toFind))
                {
                    _linkUI.Controller.SetTimeout(previousTimeout);
                    if(clickAfterFind)
                    {
                        return _linkUI.Controller.Click(toFind);
                    }
                    return true;
                }
                _linkUI.Controller.Swipe(list, SESLib.To.Up, 50);
            }
            _linkUI.Controller.SetTimeout(previousTimeout);
            return false;
        }

        /// <summary>
        /// Click <paramref name="toClick"/> while scrolling up <paramref name="list"/>
        /// </summary>
        /// <param name="list">list to scroll</param>
        /// <param name="toClick">UiSelector() of object to click</param>
        /// <param name="listScrollTimes">number of time to scroll the list. Default is 5</param>
        /// <returns>success and fail</returns>
        public bool ClickOnListWithScroll(UiSelector list, string toClick, int listScrollTimes = 5)
        {
            return FindOnListWithScroll(list, toClick, listScrollTimes, true);
        }

        /// <summary>
        /// Click <paramref name="toClick"/> while scrolling up <paramref name="list"/>
        /// </summary>
        /// <param name="list">list to scroll</param>
        /// <param name="toClick">UiSelector() of object to click</param>
        /// <param name="listScrollTimes">number of time to scroll the list. Default is 5</param>
        /// <returns>success and fail</returns>
        public bool ClickOnSequencialFileListWithScroll(UiSelector list, string toClick, int listScrollTimes = 5)
        {
            // Wait until screen contains list for timeout sec.
            if (!_linkUI.Controller.DoesScreenContains(list))
            {
                throw new DeviceWorkflowException("Can not find list to scroll");
            }
            
            int previousTimeout = _linkUI.Controller.GetTimeout();
            _linkUI.Controller.SetTimeout(0);
            for (int i = 0; i < listScrollTimes; i++)
            {
                if (_linkUI.Controller.DoesScreenContains(toClick))
                {
                    _linkUI.Controller.SetTimeout(previousTimeout);
                    
                    return _linkUI.Controller.Click(toClick);                    
                }
                _linkUI.Controller.Swipe(list, SESLib.To.Up, 50);
            }
            _linkUI.Controller.SetTimeout(previousTimeout);

            return false;
        }

        /// <summary>
        /// Click <paramref name="toClick"/> while swiping <paramref name="list"/>
        /// </summary>
        /// <param name="list">list to swipe</param>
        /// <param name="toClick">string to click</param>
        /// <returns>success and fail</returns>
        public bool ClickOnListWithSwipe(UiSelector list, string toClick)
        {
            // Wait until screen contains list for timeout sec.
            if (!_linkUI.Controller.DoesScreenContains(list))
            {
                throw new DeviceWorkflowException("Can not find list to swipe");
            }

            int previousTimeout = _linkUI.Controller.GetTimeout();
            _linkUI.Controller.SetTimeout(0);
            bool result;
            for (int i = 0; i < 5; i++)
            {
                if (_linkUI.Controller.DoesScreenContains(toClick))
                {
                    result = _linkUI.Controller.Click(toClick);
                    _linkUI.Controller.SetTimeout(previousTimeout);
                    return result;
                }
                _linkUI.Controller.Swipe(list, SESLib.To.Left, 50);
            }
            _linkUI.Controller.SetTimeout(previousTimeout);
            return false;

        }

        /// <summary>
        /// Returns current selected option for given target option from device.
        /// </summary>
        /// <param name="targetOption">Target Option to get (ex. File Type and Resolution)</param>
        /// <returns>Selected option as string</returns>
        /// <exception cref="DeviceWorkflowException">throws when app screen is not in the option setting pane.</exception>
        public string GetCurrentOption(string targetOption)
        {
            int previousTimeout = _linkUI.Controller.GetTimeout();
            _linkUI.Controller.SetTimeout(1);

            if (!_linkUI.Controller.DoesScreenContains($"//*[@text='{targetOption}']/../*[2]"))
            {
                _linkUI.Controller.Swipe(new UiSelector().ResourceId($"{_packageName}:id/lv_option_list"), SESLib.To.Up, 20);
            }

            _linkUI.Controller.SetTimeout(previousTimeout);
                        
            return _linkUI.Controller.GetText($"//*[@text='{targetOption}']/../*[2]");
        }

        /// <summary>
        /// Click the options button when the button is not clicked
        /// </summary>
        public void SetOptionsScreen()
        {
            int previousTimeout = _linkUI.Controller.GetTimeout();
            _linkUI.Controller.SetTimeout(1);

            // If app is not in the option setting pane, throw DeviceWorkFlowException
            if (!_linkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{_packageName}:id/fl_right_base_pane")))
            {
                Thread.Sleep(300);

                if(!_linkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{_packageName}:id/fl_right_pane")))
                {
                    _linkUI.Controller.SetTimeout(previousTimeout);
                    throw new DeviceWorkflowException("App screen is not in the option setting pane.");
                }
            }

            // For scan options, move to detail option view
            if (_linkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{_packageName}:id/bt_hide_options")))
            {
                if ("Options".Equals(_linkUI.Controller.GetText(new UiSelector().ResourceId($"{_packageName}:id/bt_hide_options"))))
                {
                    _linkUI.Controller.Click(new UiSelector().ResourceId($"{_packageName}:id/bt_hide_options"));                    
                }
            }
            _linkUI.Controller.SetTimeout(previousTimeout);
        }
        
        /// <summary>
        /// Waiting until appearing the object. If done, it returns true.
        /// </summary>
        /// <param name="waitingObject"> Wait to disappeared specific object </param>
        /// <param name="intervalTime"> Time for each waiting (Default: 500) </param>        
        /// <param name="waitingCount"> Count for waiting (Default: 20)</param>
        /// <param name="limitTime"> Limit time for test terminated (10% of inacitivity timeout - maximum value is 10sec) </param>
        /// <returns>If object disappeared before limitation, return true. If not, return false </returns>        
        public bool WaitingObjectAppear(string waitingObject, int intervalTime = 500, int waitingCount = 20, int limitTime = 0)
        {   
            int timeOut = _linkUI.Controller.GetTimeout();

            // WaitingObjectAppear will be judge the result by endTime or endByLimitTime
            // endTime: It defined by "intervalTime * waitingCount" by developer's definition.
            // endByLimitTime: It defined by "limitTime" on the variable. It usually use by inactivity timeout.
            DateTime startTime = DateTime.Now;            
            DateTime endTime = startTime.AddMilliseconds(Convert.ToInt32(intervalTime * waitingCount));
            DateTime endByLimitTime = startTime.AddMilliseconds(limitTime);

            _linkUI.Controller.SetTimeout(0);

            if (limitTime != 0)
            {
                while (!_linkUI.Controller.DoesScreenContains(waitingObject))
                {
                    DateTime currentTime = DateTime.Now;

                    if ((currentTime >= endTime) || (currentTime >= endByLimitTime))
                    {
                        _linkUI.Controller.SetTimeout(timeOut);                        
                        return false;
                    }
                    Thread.Sleep(intervalTime);
                }
            }
            else
            {
                while (!_linkUI.Controller.DoesScreenContains(waitingObject))
                {
                    DateTime currentTime = DateTime.Now;

                    if (currentTime >= endTime)
                    {
                        _linkUI.Controller.SetTimeout(timeOut);
                        return false;
                    }
                    Thread.Sleep(intervalTime);
                }
            }

            _linkUI.Controller.SetTimeout(timeOut);

            return true;
        }

        /// <summary>
        /// Waiting until disappearing the object. If done, it returns true.
        /// </summary>
        /// <param name="waitingObject"> Wait to disappeared specific object </param>
        /// <param name="intervalTime"> Time for each waiting (Default: 500) </param>        
        /// <param name="waitingCount"> Count for waiting (Default: 20)</param>
        /// <param name="limitTime"> Limit time for test terminated (Inacitivity timeout - 3) </param>
        /// <returns>If object disappeared before limitation, return true. If not, return false </returns>        
        public bool WaitingObjectDisappear(string waitingObject, int intervalTime = 500, int waitingCount = 20, int limitTime = 0)
        {   
            int timeOut = _linkUI.Controller.GetTimeout();

            // WaitingObjectDisappear will be judge the result by endTime or endByLimitTime
            // endTime: It defined by "intervalTime * waitingCount" by developer's definition.
            // endByLimitTime: It defined by "limitTime" on the variable. It usually use by inactivity timeout.
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddMilliseconds(Convert.ToInt32(intervalTime * waitingCount));
            DateTime endByLimitTime = startTime.AddMilliseconds(limitTime);

            _linkUI.Controller.SetTimeout(0);

            if (limitTime != 0)
            {
                while (_linkUI.Controller.DoesScreenContains(waitingObject))
                {
                    DateTime currentTime = DateTime.Now;

                    if ((currentTime >= endTime) || (currentTime > endByLimitTime))
                    {
                        _linkUI.Controller.SetTimeout(timeOut);
                        return false;
                    }
                    Thread.Sleep(intervalTime);
                }
            }
            else
            {
                while (_linkUI.Controller.DoesScreenContains(waitingObject))
                {
                    DateTime currentTime = DateTime.Now;

                    if (currentTime >= endTime)
                    {
                        _linkUI.Controller.SetTimeout(timeOut);
                        return false;
                    }
                    Thread.Sleep(intervalTime);
                }
            }            
            _linkUI.Controller.SetTimeout(timeOut);

            return true;
        }

        /// <summary>
        /// Force stop the package
        /// </summary>
        public void ForceStop()
        {
            _linkUI.Controller.ExecuteADBCommand($"shell am force-stop {_packageName}");
        }

        /// <summary>
        /// Clears the retain popup.
        /// </summary>
        /// <returns><c>true</c> if close retain popup is success, <c>false</c> otherwise.</returns>
        public bool ClearRetainPopup()
        {
            bool cleared = false;
            if (_linkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{_packageName}:id/bt_dialog_left")))
            {
                _linkUI.Controller.Click(new UiSelector().ResourceId($"{_packageName}:id/bt_dialog_left"));
            }
            if (!_linkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{_packageName}:id/bt_dialog_left")))
            {
                cleared = true;
            }

            return cleared;
        }

        /// <summary>
        /// Checks the retain popup.
        /// </summary>
        /// <returns><c>true</c> if retain popup is exist, <c>false</c> otherwise.</returns>
        public bool CheckRetainPopup()
        {
            bool retainPopup = false;
            if (_linkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{_packageName}:id/iv_dialog_icon")))
            {
                retainPopup = true;
            }
            return retainPopup;
        }

        /// <summary>
        /// Determines whether [has popup message] [the specified message].
        /// </summary>
        /// <returns><c>true</c> if [has popup message] [the specified message]; otherwise, <c>false</c>.</returns>
        public string GetPopupMessage()
        {
            string returnMessage = "Error Popup has no Error Message";
            returnMessage = _linkUI.Controller.GetText(new UiSelector().ResourceId($"{_packageName}:id/tv_contents"));
            return returnMessage;
        }

        /// <summary>
        /// Return Error Message on the Popup after checking popup visible
        /// </summary>
        /// <returns></returns>
        public void CheckErrorPopup()
        {
            if(CheckRetainPopup())
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Error Popup is visible, Error message is \"{GetPopupMessage()}\".");                
                throw e;
            }
        }
    }
}
