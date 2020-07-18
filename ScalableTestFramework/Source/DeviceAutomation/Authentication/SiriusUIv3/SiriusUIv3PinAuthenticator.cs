using System;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.Authentication.SiriusUIv3
{
    /// <summary>
    /// Sirius UIv3 PIN Authenticator class.  Provides authentication for user PIN on Sirius UIv3 devices.
    /// </summary>
    internal class SiriusUIv3PinAuthenticator : SiriusUIv3WindowsAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv3PinAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="SiriusUIv3ControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public SiriusUIv3PinAuthenticator(SiriusUIv3ControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters the PIN on the device control panel.
        /// </summary>
        public override void EnterCredentials()
        {
            EnterPin();
        }

        /// <summary>
        /// Enters the PIN on the device control panel.
        /// </summary>
        protected void EnterPin()
        {
            if (ControlPanel.WaitForScreenLabel("vw_sips_apps_state", TimeSpan.FromSeconds(40)))
            {
                Widget widget = FindWidget("sips_app_screen_header");
                if (widget != null)
                {
                    //Handling remote jobs
                    widget = FindWidget("fb_footerCenter");

                    if (widget != null)
                    {
                        ControlPanel.Press("fb_footerCenter");
                    }

                    ControlPanel.WaitForWidget("object0", TimeSpan.FromSeconds(20));
                    ControlPanel.SetValue("object0", Credential.Pin);
                    Pacekeeper.Pause();
                    return;
                }
            }

            // Did not find the expected screens to enter a PIN.
            throw new DeviceWorkflowException("Unable to enter PIN code.");
        }

        /// <summary>
        /// Checks the connection to the provider server.
        /// </summary>
        /// <exception cref="HP.ScalableTest.DeviceAutomation.DeviceWorkflowException">Unable to connect to HPAC server.</exception>
        protected void CheckProviderConnection()
        {
            Widget widgetCollection = FindWidget("textview");

            if (widgetCollection != null)
            {
                if (widgetCollection.Values.First().Value.Contains("The printer could not connect to the Internet."))
                {
                    ControlPanel.PressByValue("Retry");
                    Thread.Sleep(_defaultWait);
                    throw new DeviceWorkflowException("Unable to connect to the Solution server.");
                }
            }
        }
    }
}
