using System;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Helpers.Android;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.PullPrint;
using HP.ScalableTest.Utility;
using HP.SPS.SES;
using HP.SPS.SES.Helper;

namespace HP.ScalableTest.Plugin.HpRoam
{
    /// <summary>
    /// Base class providing common Android functionality used by Print and PullPrint activities.
    /// </summary>
    public abstract class AndroidAppManagerBase : DeviceWorkflowLogSource
    {
        protected readonly PluginExecutionData _executionData = null;
        protected readonly HpRoamActivityData _activityData = null;
        protected readonly SESLib _controller = null;
        protected readonly AndroidHelper _androidHelper = null;

        /// <summary>
        /// Occurs when a status has changed.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusUpdate;

        /// <summary>
        /// Occurs when a status has changed that tracks duration.
        /// </summary>
        public event EventHandler<TimeStatusEventArgs> TimeStatusUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidAppManagerBase"/> class.
        /// </summary>
        /// <param name="pluginExecutionData"></param>
        /// <param name="activityData"></param>
        public AndroidAppManagerBase(PluginExecutionData pluginExecutionData, HpRoamActivityData activityData)
        {
            try
            {
                _executionData = pluginExecutionData;
                _activityData = activityData;
                _controller = SESLib.Create(_activityData.MobileEquipmentId);
                _controller.Connect(false, true);

                _androidHelper = new AndroidHelper(_controller);

                _controller.PressKey(KeyCode.KEYCODE_WAKEUP); //Power Button 
                _controller.PressKey(KeyCode.KEYCODE_HOME);

                if (_androidHelper.ExistResourceId("com.android.systemui:id/keyguard_indication_area"))
                {
                    _controller.Swipe(SESLib.To.Up);
                }
            }
            catch (NullReferenceException ex)
            {
                throw new DeviceWorkflowException($"Unable to connect to the android phone using the connection ID {_activityData.MobileEquipmentId}.", ex);
            }
        }

        /// <summary>
        /// Invoke the StatusUpdate Event.
        /// </summary>
        /// <param name="message"></param>
        protected void OnStatusUpdate(string message)
        {
            StatusUpdate?.Invoke(this, new StatusChangedEventArgs(message));
        }

        /// <summary>
        /// Invoke the TimeStatusUpdate Event.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        protected void OnTimeStatusUpdate(string eventName, DateTime start, DateTime end)
        {
            TimeStatusUpdate?.Invoke(this, new TimeStatusEventArgs(eventName, start, end));
        }

        /// <summary>
        /// Launch the Roam App.
        /// </summary>
        public void Launch(bool recordMarkers)
        {
            PressRoamAppButton(recordMarkers);

            RoamAndroidAuthenticator authenticator = new RoamAndroidAuthenticator(_executionData, _activityData, _controller, _androidHelper);
            authenticator.WorkflowLogger = WorkflowLogger;
            authenticator.StatusUpdate += StatusUpdate;

            string msg = $"Authenticating user {authenticator.Credential.UserName} with {authenticator.Provider}.";
            OnStatusUpdate(msg);
            ExecutionServices.SystemTrace.LogDebug(msg);
            authenticator.Authenticate();

        }

        /// <summary> 
        /// Returns the device to the home screen.
        /// </summary>
        public void SendToHomeScreen()
        {
            _controller.PressKey(KeyCode.KEYCODE_HOME);
        }

        /// <summary>
        /// Press the HP Roam app button on the phone.
        /// </summary>
        /// <param name="recordMarkers">Whether or not to record performance markers.</param>
        private void PressRoamAppButton(bool recordMarkers)
        {
            string appText = "HP Roam";
            TimeSpan timeOut = TimeSpan.FromSeconds(30);

            if (_androidHelper.WaitForAvailableText(appText, timeOut))
            {
                string msg = "Pressing the HP Roam App Button";
                OnStatusUpdate(msg);
                ExecutionServices.SystemTrace.LogDebug(msg);
                if (recordMarkers)
                {
                    WorkflowLogger?.RecordEvent(DeviceWorkflowMarker.AppButtonPress, appText);
                }
                _controller.Click(new UiSelector().TextContains(appText));
            }
            else
            {
                new DeviceWorkflowException($"The HP Roam app button was not found within {timeOut.TotalSeconds} seconds.");
            }
        }


    }
}
