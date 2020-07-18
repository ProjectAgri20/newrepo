using System;
using System.Linq;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.Authentication.SiriusUIv3
{
    internal class SiriusUIv3WindowsAuthenticator : SiriusUIv3AppAuthenticatorBase
    {
        private readonly string[] _userNameWidgetId = { "lineedit", "lineedit1" };
        private readonly string[] _userNameButtonId = { "button1", "lineedit1" };

        private readonly string[] _passwordWidgetId = { "lineedit", "lineedit2" };
        private readonly string[] _passwordButtonId = { "button2", "lineedit2" };

        protected readonly TimeSpan _defaultWait = TimeSpan.FromSeconds(2);
        private ScreenSizes _screenSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv3WindowsAuthenticator"/> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="SiriusUIv3ControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public SiriusUIv3WindowsAuthenticator(SiriusUIv3ControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            :base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device by entering Username and Password.
        /// </summary>
        public override void EnterCredentials()
        {
            EnterUserNamePassword();
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public override void SubmitAuthentication()
        {
            SubmitAuthentication("Login");
        }

        /// <summary>
        /// Submits the authentication request by pressing the specified button.
        /// </summary>
        /// <param name="buttonText">The button text.</param>
        protected void SubmitAuthentication(string buttonText)
        {
            //Get widget by text value.
            Widget signInButton = ControlPanel.WaitForWidgetByValue(buttonText);

            if (signInButton != null)
            {
                ControlPanel.Press(signInButton);
                Pacekeeper.Sync();
                return;
            }

            //If we get here, we did not find the sign in button.
            throw new DeviceWorkflowException($"Unable to find widget with value: {buttonText}.");
        }

        /// <summary>
        /// Enters the user name and password on the device control panel.
        /// </summary>
        private void EnterUserNamePassword()
        {
            var currentScreenLabels = ControlPanel.GetScreenInfo().ScreenLabels.ToList();
            if (currentScreenLabels.Contains("AnA_Login_With_Windows_Authentication") || currentScreenLabels.Contains("AnA_Login_With_LDAP_Authentication"))
            {
                string doneButtonId = "_done";

                string userNameButton = _userNameButtonId[(int)_screenSize];
                string userNameWidget = _userNameWidgetId[(int)_screenSize];

                //Enter UserName
                ControlPanel.Press(userNameButton);
                Pacekeeper.Pause();
                ControlPanel.SetValue(userNameWidget, Credential.UserName);

                ControlPanel.WaitForWidget(doneButtonId);

                Pacekeeper.Pause();
                ControlPanel.Press(doneButtonId);
                Pacekeeper.Sync();

                //Enter Password               
                ControlPanel.Press(_passwordButtonId[(int)_screenSize]);
                Pacekeeper.Pause();
                ControlPanel.SetValue(_passwordWidgetId[(int)_screenSize], Credential.Password);

                ControlPanel.WaitForWidget(doneButtonId);

                Pacekeeper.Pause();
                ControlPanel.Press(doneButtonId);
                Pacekeeper.Sync();
            }
        }
    }
}
