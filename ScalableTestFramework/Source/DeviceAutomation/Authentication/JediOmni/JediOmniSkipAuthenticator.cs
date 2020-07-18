using System;
using System.Collections.Generic;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni Skip Authenticator.  Does not perform any authentication operations.
    /// Obtains and logs the user name (from the device control panel) of the currently logged-in user.
    /// </summary>
    public class JediOmniSkipAuthenticator : DeviceWorkflowLogSource, IAuthenticator
    {
        /// <summary>
        /// Gets the <see cref="JediOmniControlPanel" /> for this solution authenticator.
        /// </summary>
        private JediOmniDevice Device { get; }

        /// <summary>
        /// Constructs a new instance of <see cref="JediOmniSkipAuthenticator" />
        /// </summary>
        /// <param name="device"></param>
        /// <param name="credential"></param>
        public JediOmniSkipAuthenticator(IDevice device, AuthenticationCredential credential)
        {
            Device = (JediOmniDevice)device;
            Credential = credential;
            Provider = AuthenticationProvider.Skip;
            InitializationMethod = AuthenticationInitMethod.DoNotSignIn;
            LazyAuthOnly = false;
        }

        /// <summary>
        /// Gets the authentication Credential.
        /// </summary>
        public AuthenticationCredential Credential { get; protected set; }

        /// <summary>
        /// Gets the authentication Provider.
        /// </summary>
        public AuthenticationProvider Provider { get; protected set; }

        /// <summary>
        /// Gets or sets the authentication initialization method.
        /// </summary>
        /// <value>
        /// The authentication initialization method.
        /// </value>
        public AuthenticationInitMethod InitializationMethod { get; set; }

        /// <summary>
        /// Gets or sets the Pace keeper for this authenticator which is used to control pacing of the workflow.
        /// </summary>
        /// <value>The Pace keeper.</value>
        public Pacekeeper Pacekeeper { get; set; } = new Pacekeeper(TimeSpan.Zero);

        /// <summary>
        /// Gets or sets a value indicating whether [lazy authentication only].
        /// This requires that the provider be set with the desired solution.
        /// </summary>
        /// <value>
        /// <c>true</c> if [lazy authentication only]; otherwise, <c>false</c>.
        /// </value>
        public bool LazyAuthOnly { get; set; }

        /// <summary>
        /// Checks the device for a currently logged-in user.  Logs the user information to the system tracelog.
        /// </summary>
        public void Authenticate()
        {
            Authenticate(null);
        }

        /// <summary>
        /// Checks the device for a currently logged-in user.  Logs the user information to the system tracelog.
        /// </summary>
        /// <param name="parameters">Not used in <see cref="JediOmniSkipAuthenticator" />.</param>
        /// <exception cref="DeviceWorkflowException">Throws if not at home screen or if user is not logged in.</exception>
        public void Authenticate(Dictionary<string, object> parameters)
        {
            if (AtHomeScreen())
            {
                string signInButtonText = Device.ControlPanel.GetValue(JediOmniLaunchHelper.SignInOrSignoutButton, "innerText", OmniPropertyType.Property).Trim();
                ExecutionServices.SystemTrace.LogInfo($"Sign In button text: {signInButtonText}");

                string userName = Device.ControlPanel.GetValue("#hpid-label-username", "innerText", OmniPropertyType.Property);
                ExecutionServices.SystemTrace.LogInfo($"Logged-in User: {userName}");

                if (signInButtonText.Equals("Sign In", StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(userName))
                {
                    throw new DeviceWorkflowException($"User not logged in on Device at {Device.Address}.");
                }

                return;
            }

            throw new DeviceWorkflowException($"Device at {Device.Address} not at home screen.");
        }

        private bool AtHomeScreen()
        {
            return Device.ControlPanel.CheckState(".hp-homescreen-folder-view[hp-global-top-view=true]", OmniElementState.Exists)
                && Device.ControlPanel.CheckState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely);
        }
    }
}
