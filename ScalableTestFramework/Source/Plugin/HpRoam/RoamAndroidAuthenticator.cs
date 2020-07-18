using System;
using System.Linq;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.Android;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using HP.SPS.SES;
using HP.SPS.SES.Helper;

namespace HP.ScalableTest.Plugin.HpRoam
{
    /// <summary>
    /// Manages Roam authentication on an Android device.
    /// </summary>
    internal class RoamAndroidAuthenticator : DeviceWorkflowLogSource
    {
        private readonly SESLib _controller = null;
        private readonly AndroidHelper _androidHelper = null;

        /// <summary>
        /// Occurs when a status has changed.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusUpdate;

        /// <summary>
        /// Gets the authentication Credential.
        /// </summary>
        public AuthenticationCredential Credential { get; private set; }

        /// <summary>
        /// Gets the authentication Provider.
        /// </summary>
        public AuthenticationProvider Provider { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoamAndroidAuthenticator"/> class.
        /// </summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        /// <param name="activityData">The plugin activity data.</param>
        /// <param name="controller">The SES controller.</param>
        /// <param name="androidHelper">The android helper.</param>
        public RoamAndroidAuthenticator(PluginExecutionData pluginExecutionData, HpRoamActivityData activityData, SESLib controller, AndroidHelper androidHelper)
        {
            Provider = activityData.AuthProvider;
            _controller = controller;
            _androidHelper = androidHelper;

            switch (activityData.AuthProvider)
            {
                case AuthenticationProvider.HpId:
                    ExternalCredentialInfo externalCredential = pluginExecutionData.ExternalCredentials.Where(x => x.Provider == ExternalCredentialType.HpId).FirstOrDefault();
                    Credential = new AuthenticationCredential(externalCredential.UserName, externalCredential.Password, pluginExecutionData.Credential.Domain);
                    break;
                default:
                    Credential = new AuthenticationCredential(pluginExecutionData.Credential);
                    break;
            }
        }

        /// <summary>
        /// Authenticate using the specified provider.
        /// </summary>
        public void Authenticate()
        {
            RecordInfo(DeviceWorkflowMarker.AuthType, Provider.GetDescription());
            RecordEvent(DeviceWorkflowMarker.AuthenticationBegin);

            if (!CheckForRoamQueueScreen(TimeSpan.FromSeconds(5)))
            {
                NavigateWelcomeScreen();
                RecordEvent(DeviceWorkflowMarker.EnterCredentialsBegin);
                switch (Provider)
                {
                    case AuthenticationProvider.HpId:
                        EnterHpIdCredentials();
                        break;
                    default:
                        break;
                }
                SubmitAuthentication();
                if (!ValidateAuthentication())
                {
                    throw new DeviceWorkflowException($"{Credential.UserName} login failed.");
                }
                RecordEvent(DeviceWorkflowMarker.EnterCredentialsEnd);
                OnStatusUpdate("Authentication successful.");                
            }

            RecordEvent(DeviceWorkflowMarker.AuthenticationEnd);
        }

        /// <summary>
        /// Invoke the StatusUpdate Event.
        /// </summary>
        /// <param name="message"></param>
        protected void OnStatusUpdate(string message)
        {
            StatusUpdate?.Invoke(this, new StatusChangedEventArgs(message));
        }

        private bool CheckForRoamQueueScreen(TimeSpan waitTime)
        {
            return _androidHelper.WaitForAvailableResourceId("com.hp.roam:id/my_queue", waitTime);
        }

        private void NavigateWelcomeScreen()
        {
            const string privacyCheckbox = "com.hp.roam:id/value_prop_privacy_checkbox";

            //Ensure we are on the Roam Welcome screen.  We've already waited 5 seconds, so it should be there.
            TimeSpan timeout = TimeSpan.FromSeconds(3);
            if (! _androidHelper.WaitForAvailableResourceId("com.hp.roam:id/value_prop_title", timeout))
            {
                throw new DeviceWorkflowException($"Roam Welcome screen did not display within {timeout.TotalSeconds} seconds.");
            }

            OnStatusUpdate("Welcome screen detected.");

            if (_androidHelper.WaitForAvailableResourceId(privacyCheckbox, TimeSpan.FromSeconds(1)))
            {
                // Accept Privacy Policy
                _controller.Click(new UiSelector().ResourceId(privacyCheckbox));
                // Click the Sign In button
                _controller.Click(new UiSelector().ResourceId("com.hp.roam:id/value_prop_login"));
            }
        }

        private void EnterHpIdCredentials()
        {
            OnStatusUpdate("Enter credentials.");
            TimeSpan timeout = TimeSpan.FromSeconds(15);
            //Wait for the HpId screen, it usually takes a few seconds to connect to the cloud.
            if (_androidHelper.WaitForAvailableText("login.id.hp.com", timeout))
            {
                EnterUserName();
                _controller.Click(new UiSelector().ResourceId("next_button"));
                EnterPassword();
                return;
            }
            throw new DeviceWorkflowException($"HpId login screen did not display within {timeout.TotalSeconds} seconds.");
        }

        private void EnterUserName()
        {
            UiSelector userNameSelector = new UiSelector().ResourceId("username");
            string userNametext = _controller.GetText(userNameSelector);
            if (string.IsNullOrEmpty(userNametext))
            {
                OnStatusUpdate("Enter user name.");
                if (!_controller.SetText(userNameSelector, Credential.UserName))
                {
                    throw new DeviceWorkflowException("Failed to enter user name.");
                }
            }

            UiSelector rememberSelector = new UiSelector().ResourceId("RememberMe");
            if (!_controller.IsChecked(rememberSelector))
            {
                OnStatusUpdate("Set 'Remember Me' checkbox.");
                _controller.Click(rememberSelector);
            }
        }

        private void EnterPassword()
        {
            UiSelector passwordSelector = new UiSelector().ResourceId("password");
            string passwordtext = _controller.GetText(passwordSelector);
            if (string.IsNullOrEmpty(passwordtext))
            {
                OnStatusUpdate("Enter password.");
                if (!_controller.SetText(passwordSelector, Credential.Password))
                {
                    throw new DeviceWorkflowException("Failed to enter password.");
                }
            }

            //Get rid of the keyboard if present
            if (_controller.IsVirtualKeyboardShown().Value)
            {
                _controller.PressKey(KeyCode.KEYCODE_ESCAPE);
            }
        }

        private void SubmitAuthentication()
        {
            OnStatusUpdate("Submit authentication.");
            if (!_controller.Click(new UiSelector().Text("SIGN IN")))
            {
                throw new DeviceWorkflowException("'Sign In' button was not detected.");
            }
        }

        private bool ValidateAuthentication()
        {
            //Should be on the Roam Queue List screen.
            return CheckForRoamQueueScreen(TimeSpan.FromSeconds(10));
        }
    }
}
