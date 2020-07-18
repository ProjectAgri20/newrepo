using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.Authentication.SiriusUIv3
{
    /// <summary>
    /// Sirius UIv3 Device App Authenticator base class.  Provides common authentication functionality for Sirius UIv3 devices.
    /// </summary>
    internal abstract class SiriusUIv3AppAuthenticatorBase : IAppAuthenticator
    {
        /// <summary>
        /// The error message
        /// </summary>
        protected string _errorMessage = string.Empty;
        /// <summary>
        /// Gets the <see cref="SiriusUIv3ControlPanel" /> for this solution authenticator.
        /// </summary>
        public SiriusUIv3ControlPanel ControlPanel { get; }

        /// <summary>
        /// Gets the <see cref="AuthenticationCredential" /> for this solution authenticator.
        /// </summary>
        public AuthenticationCredential Credential { get; }

        /// <summary>
        /// Gets the <see cref="Pacekeeper" /> for this solution authenticator.
        /// </summary>
        public Pacekeeper Pacekeeper { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv3AppAuthenticatorBase" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="SiriusUIv3ControlPanel"/>.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/>.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/>.</param>
        protected SiriusUIv3AppAuthenticatorBase(SiriusUIv3ControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
        {
            ControlPanel = controlPanel;
            Credential = credential;
            Pacekeeper = pacekeeper;
        }

        /// <summary>
        /// Enters credentials on the device control panel.
        /// </summary>
        public abstract void EnterCredentials();


        /// <summary>
        /// Applies additional authentication parameters before submission of authentication request.
        /// Does nothing by default because the majority of child classes do not apply parameters.
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void ApplyParameters(Dictionary<string, object> parameters)
        {
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public abstract void SubmitAuthentication();

        /// <summary>
        /// Checks the device control panel for successful authentication.
        /// Assumes the operation was successful if indicators are indeterminate.
        /// </summary>
        /// <returns><c>true</c> if the the authentication operation is valid, <c>false</c> otherwise.</returns>
        public virtual bool ValidateAuthentication()
        {
            Widget signOutButton = ControlPanel.WaitForWidgetByValue("Sign Out", TimeSpan.FromSeconds(10));
            return (signOutButton != null);
        }

        /// <summary>
        /// Find the element in widget
        /// </summary>
        /// <param name="widgetId"></param>
        /// <returns></returns>
        protected Widget FindWidget(string widgetId)
        {
            WidgetCollection wc = ControlPanel.GetScreenInfo().Widgets;

            try
            {
                return wc.First(n => StringMatcher.IsMatch(widgetId, n.Id, StringMatch.Exact, true));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// This is a debugging tool to help determine what is showing on the screen
        /// </summary>
        protected void DumpScreen()
        {
            WidgetCollection wc = ControlPanel.GetScreenInfo().Widgets;

            StringBuilder output = new StringBuilder();
            foreach (Widget w in wc)
            {
                output.Clear();
                output.Append(w.Id).Append(": ").AppendLine(w.WidgetType.ToString());
                foreach (string key in w.Values.Keys)
                {
                    output.Append("\t");
                    output.AppendLine(w.Values[key]);
                }
                System.Diagnostics.Debug.WriteLine(output.ToString());
            }
        }
    }
}
