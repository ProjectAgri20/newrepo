using System;
using System.Threading;
using HP.SPS.SES;
using HP.SPS.SES.Helper;

namespace HP.ScalableTest.DeviceAutomation.Helpers.Android
{
    /// <summary>Android Helper maintains methods directly supporting the Android solutions</summary>
    public sealed class AndroidHelper
    {
        private SESLib _controller;

        /// <summary>Initializes a new instance of the <see cref="AndroidHelper"/> class.</summary>
        /// <param name="controller">SESLib</param>
        public AndroidHelper(SESLib controller)
        {
            _controller = controller;
        }

        /// <summary>Returns if the given resource identifier exists.</summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <returns>boolean</returns>
        public bool ExistResourceId(string resourceId)
        {
            return _controller.DoesScreenContains(new UiSelector().ResourceId(resourceId));
        }

        /// <summary>
        /// Checks the text of the specified ResourceId for the specified text value.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="text">The text to search for.</param>
        /// <returns>true if the text is equal.</returns>
        public bool ExistTextOnResourceId(string resourceId, string text)
        {
            UiSelector uiSelector = new UiSelector().ResourceId(resourceId);
            bool exist = _controller.GetText(uiSelector).Equals(text);

            return exist;
        }

        /// <summary>Waits for availability of the given resource identifier for up to the given time.</summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="tsWait">The ts wait.</param>
        /// <returns>true if found</returns>
        public bool WaitForAvailableResourceId(string resourceId, TimeSpan tsWait)
        {
            int timeOut = _controller.GetTimeout();
            DateTime dtWait = DateTime.Now.AddSeconds(tsWait.TotalSeconds);

            UiSelector uiSelector = new UiSelector().ResourceId(resourceId);
            bool available = _controller.DoesScreenContains(uiSelector);

            while (!available && dtWait > DateTime.Now)
            {
                Thread.Sleep(250);
                available = _controller.DoesScreenContains(uiSelector);

                double myTime = dtWait.Subtract(DateTime.Now).TotalSeconds;
                if (myTime % 5 == 0)
                {
                    _controller.PressKey(KeyCode.KEYCODE_WAKEUP);
                }
            }

            _controller.SetTimeout(timeOut);
            return available;
        }

        /// <summary>Waits for one of the given resource ids to be displayed on the screen.</summary>
        /// <param name="resourceIdFirst">The resource identifier one.</param>
        /// <param name="resourceIdSecond">The resource identifier two.</param>
        /// <param name="tsWait">The ts wait.</param>
        /// <returns>true if one of the two is displayed</returns>
        public bool WaitForAvailableResourceIds(string resourceIdFirst, string resourceIdSecond, TimeSpan tsWait)
        {
            int timeOut = _controller.GetTimeout();
            DateTime dtWait = DateTime.Now.AddSeconds(tsWait.TotalSeconds);
            UiSelector uiSelector1 = new UiSelector().ResourceId(resourceIdFirst);
            UiSelector uiSelector2 = new UiSelector().ResourceId(resourceIdSecond);

            bool available = ExistResourceId(resourceIdFirst, resourceIdSecond);

            while (!available && dtWait > DateTime.Now)
            {
                Thread.Sleep(250);
                available = ExistResourceId(resourceIdFirst, resourceIdSecond);

                double myTime = dtWait.Subtract(DateTime.Now).TotalSeconds;
                if (myTime % 7 == 0)
                {
                    _controller.PressKey(KeyCode.KEYCODE_WAKEUP);
                }
            }

            _controller.SetTimeout(timeOut);
            return available;
        }

        private bool ExistResourceId(string resourceIdFirst, string resourceIdSecond)
        {
            return (ExistResourceId(resourceIdFirst) || ExistResourceId(resourceIdSecond));
        }

        /// <summary>Waits for availability of the given text on the device for the up to the given time</summary>
        /// <param name="textTitle">The text title.</param>
        /// <param name="tsWait">The ts wait.</param>
        /// <returns>true if found</returns>
        public bool WaitForAvailableText(string textTitle, TimeSpan tsWait)
        {
            int timeOut = _controller.GetTimeout();
            _controller.SetTimeout(0);

            DateTime dtWait = DateTime.Now.AddSeconds(tsWait.TotalSeconds);
            UiSelector uiSelector = new UiSelector().TextContains(textTitle);
            bool available = _controller.DoesScreenContains(uiSelector);

            while (!available && dtWait > DateTime.Now)
            {
                Thread.Sleep(250);

                available = _controller.DoesScreenContains(uiSelector);

                double myTime = dtWait.Subtract(DateTime.Now).TotalSeconds;
                if (myTime % 5 == 0)
                {
                    _controller.PressKey(KeyCode.KEYCODE_WAKEUP);
                }
            }

            _controller.SetTimeout(timeOut);
            return available;
        }

        /// <summary>Returns if given resource identifier is enabled</summary>
        /// <param name="resourceId">Resource ID</param>
        /// <returns>True if found</returns>
        public bool ResourceIdEnabled(string resourceId)
        {
            return _controller.IsEnabled(new UiSelector().ResourceId(resourceId));
        }

        /// <summary>Waits for the given resource identifier to not be displayed on the device</summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="tsWait">The time to wait.</param>
        /// <returns>true on success</returns>
        public bool WaitForNotDisplayedResourceId(string resourceId, TimeSpan tsWait)
        {
            int timeOut = _controller.GetTimeout();
            _controller.SetTimeout(0);

            DateTime dtWait = DateTime.Now.AddSeconds(tsWait.TotalSeconds);
            UiSelector uiSelector = new UiSelector().ResourceId(resourceId);
            bool displaid = _controller.DoesScreenContains(uiSelector);

            while (displaid && dtWait > DateTime.Now)
            {
                Thread.Sleep(250);

                displaid = _controller.DoesScreenContains(uiSelector);
                int myTime = (int)(dtWait.Subtract(DateTime.Now).TotalSeconds);
                if (myTime % 5 == 0)
                {
                    _controller.PressKey(KeyCode.KEYCODE_WAKEUP);
                }
            }

            _controller.SetTimeout(timeOut);
            return !displaid;
        }
    }
}
