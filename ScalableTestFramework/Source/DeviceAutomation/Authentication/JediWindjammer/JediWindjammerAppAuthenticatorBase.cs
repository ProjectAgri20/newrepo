using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Utility;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;


namespace HP.ScalableTest.DeviceAutomation.Authentication.JediWindjammer
{
    /// <summary>
    /// Jedi Windjammer Device App Authenticator base class.  Provides common authentication functionality for Jedi Windjammer devices.
    /// </summary>
    internal abstract class JediWindjammerAppAuthenticatorBase : IAppAuthenticator
    {
        protected const string KeyboardId = "#hpid-keyboard";
        protected const string AuthDropdownId = "#hpid-dropdown-agent";
        protected readonly List<string> UserNameIds = new List<string> { ".hp-credential-control:first:text", ".hp-credential-control:text", ".hp-credential-control:password", ".hp - textbox:last" };
        protected string _errorMessage = string.Empty;
        /// <summary>
        /// Gets the <see cref="JediWindjammerControlPanel" /> for this solution authenticator.
        /// </summary>
        public JediWindjammerControlPanel ControlPanel { get; }

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
        /// Initializes a new instance of the <see cref="JediWindjammerAppAuthenticatorBase" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediWindjammerControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        protected JediWindjammerAppAuthenticatorBase(JediWindjammerControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
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
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void ApplyParameters(Dictionary<string, object> parameters)
        {
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public virtual void SubmitAuthentication()
        {
        }

        /// <summary>
        /// Gets the current title on the device SignIn screen.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <exception cref="HP.DeviceAutomation.DeviceInvalidOperationException">Unable to retrieve current title.</exception>
        protected string GetDeviceTitle()
        {
            string value = ControlPanel.CurrentForm();

            if (value.Equals("HPProgressPopup"))
            {
                ControlPanel.WaitForForm("SigninForm", TimeSpan.FromSeconds(30));
            }

            try
            {
                value = ControlPanel.GetProperty("mAppTitleLabel", "Text");
            }
            catch (ControlNotFoundException ex)
            {
                value = ControlPanel.CurrentForm();
                if (value.Equals("HPProgressPopup"))
                {
                    ControlPanel.WaitForForm("SigninForm", TimeSpan.FromSeconds(30));
                    value = GetDeviceTitle();
                }
                else
                {
                    throw new DeviceInvalidOperationException("Unable to retrieve current title. Current form is: " + value, ex);
                }
            }

            return value;
        }

        protected void CheckForSignInForm()
        {
            const string signInFormId = "SignInForm";
            string currentForm = ControlPanel.CurrentForm();
            if (!currentForm.EqualsIgnoreCase(signInFormId))
            {
                throw new ControlNotFoundException($"Expecting form {signInFormId} but found {currentForm} instead");
            }
        }

        /// <summary>
        /// Presses the specified element using the OxpdBrowserEngine.
        /// </summary>
        /// <param name="elementId">The element Id.</param>
        protected void PressElementOxpd(string elementId)
        {
            PressElementOxpd(new List<string>() { elementId });
        }


        /// <summary>
        /// Finds a valid element in the specified element list and presses it using the OxpdBrowserEngine.
        /// </summary>
        /// <param name="elementIds"></param>
        protected void PressElementOxpd(List<string> elementIds)
        {
            OxpdBrowserEngine engine = new OxpdBrowserEngine(ControlPanel);
            BoundingBox boundingBox = new BoundingBox();

            for (int i = 0; i < elementIds.Count; i++)
            {
                try
                {
                    boundingBox = engine.GetBoundingAreaById(elementIds[i]);
                    break; // Found the element so stop iterating the list
                }
                catch (DeviceWorkflowException)
                {
                    if (i == elementIds.Count - 1)
                    {
                        //If this is the last element, rethrow the exception
                        throw;
                    }
                }
            }

            engine.PressElementByBoundingArea(boundingBox);
        }
        /// <summary>
        /// Checks the device control panel for successful authentication.
        /// Assumes the operation was successful if indicators are indeterminate.
        /// </summary>
        /// <returns><c>true</c> if the the authentication operation is valid, <c>false</c> otherwise.</returns>
        public virtual bool ValidateAuthentication()
        {
            DateTime expiration = DateTime.Now.Add(TimeSpan.FromSeconds(8));
            int tryNext = 1;
            bool signedIn = true; //Assume the login operation was successful

            while (DateTime.Now < expiration)
            {
                Thread.Sleep(250);
                try
                {
                    string cpText;
                    if (tryNext == 1)
                    {
                        cpText = GetDeviceTitle();
                        //System.Diagnostics.Debug.WriteLine($"Title: {cpText}");
                        if (cpText.Contains("Login denied", StringComparison.OrdinalIgnoreCase))
                        {
                            // Found a clear indicator of failed login.
                            signedIn = false;
                            break;
                        }
                        else if (cpText.Contains(Credential.UserName, StringComparison.OrdinalIgnoreCase))
                        {
                            //Username is displayed in the title, so we know it's good.
                            break;
                        }
                    }
                    else if (tryNext == 2)
                    {
                        cpText = ControlPanel.GetProperty(JediWindjammerLaunchHelper.SIGNIN_BUTTON, "Text");
                        if (cpText.EqualsIgnoreCase("Sign Out"))
                        {
                            // The Sign In button indicates the user is signed in
                            break;
                        }
                    }
                    else if (tryNext == 3)
                    {
                        if (ControlPanel.CurrentForm().Contains("oxp", StringComparison.OrdinalIgnoreCase))
                        {
                            // Current form is an OXP solution form.
                            break;
                        }
                    }
                    else //Start over
                    {
                        tryNext = 0;
                    }
                }
                catch (WindjammerInvalidOperationException)
                {
                    // Did not find the control (title, Sign In button, etc).  Allow to continue checking.
                }

                tryNext++;
            }

            return signedIn;
        }


    }
}
