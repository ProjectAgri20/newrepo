using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Utility;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;


namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni Device App Authenticator base class.  Provides common authentication functionality for Jedi Omni devices.
    /// </summary>
    internal abstract class JediOmniAppAuthenticatorBase : IAppAuthenticator
    {
        private const string _topViewAttribute = "[hp-global-top-view=true]";
        private const string _homeScreenLogo = "#hpid-homescreen-logo-icon";
        private const string SignInForm = "#hpid-signin-body";
        private const string _signedFormWithSIO = "#hpid-cloud-status-button";
        private const string _signedFormWithoutSIO = "#hpid-label-username";

        protected string _errorMessage = string.Empty;
        protected const string KeyboardId = "#hpid-keyboard";
        protected JediOmniPopupManager _popupManager = null;

        private Lazy<OxpdBrowserEngine> _oxpdEngine;

        /// <summary>
        /// Gets the <see cref="JediOmniControlPanel" /> for this solution authenticator.
        /// </summary>
        public JediOmniControlPanel ControlPanel { get; }

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
        /// Initializes a new instance of the <see cref="JediOmniAppAuthenticatorBase" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        protected JediOmniAppAuthenticatorBase(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
        {
            ControlPanel = controlPanel;
            Credential = credential;
            Pacekeeper = pacekeeper;
            _popupManager = new JediOmniPopupManager(controlPanel);

            _oxpdEngine = new Lazy<OxpdBrowserEngine>(() => new OxpdBrowserEngine(controlPanel));
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
        /// Verifies the keyboard is really ready for use.
        /// </summary>
        /// <param name="elementIds">The keyboard elements to check.</param>
        /// <returns>true if the keyboard exists and is usable.  False otherwise.</returns>
        /// <exception cref="DeviceInvalidOperationException">Timed out waiting for element {KeyboardId} with state {OmniElementState.Useable}.</exception>
        protected bool VerifyKeyboard(List<string> elementIds)
        {
            bool result = false;
            int i = 0;

            while (result == false && i < elementIds.Count)
            {
                result = PressElementForKeyboard(elementIds[i++]);
            }

            return result;
        }

        /// <summary>
        /// Presses the given element and waits for the keyboard to be usable.
        /// </summary>
        /// <returns>true if the keyboard exists and is usable.  False otherwise.</returns>
        /// <exception cref="DeviceInvalidOperationException">Timed out waiting for element {KeyboardId} with state {OmniElementState.Useable}.</exception>
        protected bool PressElementForKeyboard(string elementId)
        {
            bool success = false;
            if (ControlPanel.CheckState(elementId, OmniElementState.Exists))
            {
                success = ControlPanel.ScrollPressWait(elementId, KeyboardId);
            }

            return success;
        }

        /// <summary>
        /// Utilizes the control panel TypeOnVirutalKeyboard to enter the given text value and then
        /// closes the keyboard if it needs to be closed.
        /// </summary>
        /// <param name="value">The value to enter.</param>
        protected void TypeOnVirtualKeyboard(string value)
        {
            // Explicitly closes the keyboard for the smaller screen size.
            TypeOnVirtualKeyboard(value, ControlPanel.GetScreenSize().Width.Equals(480));
        }

        /// <summary>
        /// Utilizes the control panel TypeOnVirutalKeyboard to enter the given text value and then
        /// closes the keyboard if specified.
        /// </summary>
        /// <param name="value">The value to enter.</param>
        /// <param name="closeKeyboard">Whether or not to close the keyboard.</param>
        protected void TypeOnVirtualKeyboard(string value, bool closeKeyboard)
        {
            if (!ControlPanel.WaitForAvailable(KeyboardId, TimeSpan.FromSeconds(15)))
            {
                throw new DeviceWorkflowException("Keyboard did not show within 15 seconds.");
            }

            ControlPanel.TypeOnVirtualKeyboard(value);
            Pacekeeper.Sync();

            if (closeKeyboard && ControlPanel.CheckState("#hpid-keyboard-key-done", OmniElementState.Useable))
            {
                ControlPanel.Press("#hpid-keyboard-key-done");
            }
        }
        /// <summary>
        /// Returns true if the given element identifier exist.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns>bool</returns>
        protected bool ExistElementIdOxpd(string elementId)
        {
            return _oxpdEngine.Value.ExistElementId(elementId);
        }

        /// <summary>Waits for element identifier to exist for upto the given time span.</summary>
        /// <param name="elementId">The element identifier.</param>
        /// <param name="waitTime">The wait time.</param>
        /// <returns>true if exists</returns>
        protected bool WaitForElementIdOxpd(string elementId, TimeSpan waitTime)
        {
            return _oxpdEngine.Value.WaitToExistElementId(elementId, waitTime);
        }
        /// <summary>
        /// Presses the specified element using the OxpdBrowserEngine.
        /// </summary>
        /// <param name="elementId">The element Id.</param>
        protected void PressElementOxpd(string elementId)
        {
            _oxpdEngine.Value.PressElementById(elementId);
        }

        /// <summary>
        /// Presses the specified element using the OxpdBrowserEngine with classname and index.
        /// </summary>
        /// <param name="elementClass">The element class name.</param>
        /// <param name="index">The element index.</param>
        protected void PressElementOxpdByClassIndex(string elementClass, int index)
        {
            _oxpdEngine.Value.PressElementByClassIndex(elementClass, index);
        }

        /// <summary>
        /// Finds a valid element in the specified element list and presses it using the OxpdBrowserEngine.
        /// </summary>
        /// <param name="elementIds"></param>
        protected void PressElementOxpd(List<string> elementIds)
        {
            BoundingBox boundingBox = new BoundingBox();
            string elementId = string.Empty;

            for (int idx = 0; idx < elementIds.Count; idx++)
            {
                try
                {
                    boundingBox = _oxpdEngine.Value.GetBoundingAreaById(elementIds[idx]);
                    if (ValidBoundingBox(boundingBox))
                    {
                        elementId = elementIds[idx];
                        break; // Found the element so stop iterating the list
                    }
                }
                catch (DeviceWorkflowException)
                {
                    if (idx == elementIds.Count - 1)
                    {
                        //If this is the last element, rethrow the exception
                        throw;
                    }
                }
            }

            if (!ValidBoundingBox(boundingBox))
            {
                throw new DeviceWorkflowException($"No element to press was found in the given list of elements: {string.Join(",",elementIds)}.");
            }

            double zm = _oxpdEngine.Value.GetZoomRequired(elementId);
            _oxpdEngine.Value.PressElementByBoundingArea(boundingBox, zm);
        }

        private static bool ValidBoundingBox(BoundingBox boundingBox)
        {
            bool valid = true;

            if (boundingBox.Width.Equals(0) && boundingBox.Height.Equals(0) && boundingBox.Left.Equals(0) && boundingBox.Right.Equals(0))
            {
                valid = false;
            }

            return valid;
        }

        /// <summary>
        /// Waits for HTML contains.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="waitTime">The wait time.</param>
        /// <returns></returns>
        protected bool WaitForHtmlContains(string value, TimeSpan waitTime)
        {
            return _oxpdEngine.Value.WaitForHtmlContains(value, waitTime);
        }

        /// <summary>
        /// Uses the device control panel to scroll up the specified distance.
        /// </summary>
        /// <param name="distance">The distance to scroll between y-coordinates.</param>
        protected void ScrollUp(int distance)
        {
            Size screenSize = ControlPanel.GetScreenSize();
            int xValue = screenSize.Width / 2;
            int yValue = screenSize.Height - 5;
            Coordinate start = new Coordinate(xValue, yValue);
            Coordinate end = new Coordinate(xValue, yValue + distance);

            ControlPanel.SwipeScreen(start, end, TimeSpan.FromMilliseconds(250));
        }

        /// <summary>
        /// Checks the device control panel for successful authentication.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the authentication operation is valid, <c>false</c> otherwise.
        /// </returns>
        public virtual bool ValidateAuthentication()
        {
            bool signedIn = true; //Assume the login operation was successful

            System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: JediOmniAppAuthenticatorBase::ValidateAuthentication::Checking for indication of login status.");
            _popupManager.WaitForPopup(TimeSpan.FromSeconds(5));

            List<Func<bool>> handlers = new List<Func<bool>>
            {             
                () => UserNotification(out signedIn),
                () => HomeScreenAuthenticationNotification(out signedIn),
                () => OnOxpdApplicationScreen(out signedIn),
                () => InvalidUserIdPassword(ref signedIn),
                () => OnSignInScreen(ref signedIn)
            };

            InvokeValidationHandlers(handlers);

            System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: JediOmniAppAuthenticatorBase::ValidateAuthentication::SignIn status: {signedIn}.");
            return signedIn;
        }

        protected void InvokeValidationHandlers(List<Func<bool>> handlers)
        {
            DateTime expiration = DateTime.Now.Add(TimeSpan.FromSeconds(60));
            bool authNotified = false;

            // Will only run the loop for one minute. If longer, then will break and an error will eventually be thrown
            while (!authNotified && (DateTime.Now < expiration))
            {
                try
                {
                    bool tempPopupHandled = _popupManager.HandleTemporaryPopup("Signing", TimeSpan.FromSeconds(15));
                    System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: Popup with text 'Signing' found: {tempPopupHandled}.");

                    authNotified = handlers.Any(n => n.Invoke() == true);

                    // Give the device a break before looping back through the handler list
                    if (!authNotified)
                    {
                        Thread.Sleep(250);
                    }
                }
                catch (ElementNotFoundException)
                {
                    // Did not find the element (Failed dialog, Sign In button, etc).  Allow to continue checking.                   
                    //System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                catch (Exception)
                {
                    // Some unknown error that we may want to look at during debug testing                   
                    //System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Checks to see if a popup is displayed about the User's the authentication notification.
        /// </summary>
        /// <param name="signedIn">if set to <c>true</c> [signed in].</param>
        /// <returns>bool</returns>
        protected bool UserNotification(out bool signedIn)
        {
            bool found = false;
            signedIn = false;

            if (found = _popupManager.HandleButtonOk(".hp-popup-content-container", "User:") == true)
            {
                signedIn = true;
            }
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: JediOmniAppAuthenticatorBase::UserNotification::Found valid signin notification: {found}.");
            return found;
        }

        /// <summary>
        ///  Checks to see if a popup is displayed showing an invalid user identifier and/or password.
        /// </summary>
        /// <param name="signedIn">if set to <c>true</c> [signed in].</param>
        /// <returns>bool</returns>
        private bool InvalidUserIdPassword(ref bool signedIn)
        {
            bool popupHandled = false;

            bool found = _popupManager.PopupTextContains("Invalid");

            if (found)
            {
                signedIn = false;

                //Now, try to dismiss the popup.
                List<Func<bool>> handlers = new List<Func<bool>>
                {
                    () => _popupManager.HandleButtonOk("#hpid-button-Ok"),
                    () => _popupManager.HandleButtonOk("#hpid-button-signinfailed-ok"),
                    () => _popupManager.HandleButtonOk("#hpid-button-ok"),
                };

                popupHandled = handlers.Any(n => n.Invoke() == true);
            }

            System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: JediOmniAppAuthenticatorBase::InvalidUserIdPassword::Found invalid user notification: {found}.  Dismissed the popup: {popupHandled}.");

            return found;
        }

        /// <summary>
        /// Homes the screen authentication notification.
        /// </summary>
        /// <param name="authenticated">if set to <c>true</c> [authenticated].</param>
        /// <returns>bool</returns>
        protected bool HomeScreenAuthenticationNotification(out bool authenticated)
        {
            bool onHomeScreen = false;
            authenticated = false;

            if (AtHomeScreen())
            {
                string btnSignIn = ControlPanel.GetValue(JediOmniLaunchHelper.SignInOrSignoutButton, "innerText", OmniPropertyType.Property).Trim();
                if (onHomeScreen = btnSignIn.EqualsIgnoreCase("Sign Out") == true)
                {                    
                    authenticated = true;
                }
                else
                {
                    string userName = ControlPanel.GetValue("#hpid-label-username", "innerText", OmniPropertyType.Property);
                    if (onHomeScreen = userName.Contains(Credential.UserName, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        authenticated = true;
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: JediOmniAppAuthenticatorBase::HomeScreenAuthenticationNotification::On Home Screen: {onHomeScreen}");

            return onHomeScreen;
        }

        /// <summary>
        /// Checks to see if access has been denied and that notification is displayed on the sign in screen
        /// </summary>
        /// <param name="authenticated">if set to <c>true</c> [authenticated].</param>
        /// <returns></returns>
        private bool OnSignInScreen(ref bool authenticated)
        {
            bool signInScreen = false;
            DateTime expiration = DateTime.Now.Add(TimeSpan.FromSeconds(20));

            string authenticate = ControlPanel.GetValue(SignInForm, "innerText", OmniPropertyType.Property);
            if (signInScreen = authenticate.Contains("denied", StringComparison.OrdinalIgnoreCase) == true)
            {                
                authenticated = false;
            }

            while (!ControlPanel.WaitForState(_signedFormWithSIO, OmniElementState.ActiveElement, true, TimeSpan.FromMilliseconds(500))
                && ((DateTime.Now < expiration)))
            {
                if (ControlPanel.WaitForAvailable(_signedFormWithoutSIO, TimeSpan.FromMilliseconds(500)))
                {
                    break;
                }
            }
            return signInScreen;
        }

        /// <summary>
        /// A catch all; if not on the home screen or sign in screen, are we on a solution screen? If so,
        /// then assume authentication was successful.
        /// </summary>
        /// <param name="authenticated">if set to <c>true</c> [authenticated].</param>
        /// <returns></returns>
        protected bool OnOxpdApplicationScreen(out bool authenticated)
        {
            bool otherScreen = false;
            authenticated = false;

            if (!_popupManager.PopupDisplayed())
            {
                bool oxpdScreen = ControlPanel.CheckState(".hp-app, .hp-oxpd-app-screen", OmniElementState.Exists);
                if (oxpdScreen || (!OnSignInScreen(ref authenticated) && !AtHomeScreen()))
                {
                    // assume on the application screen
                    otherScreen = true;
                    authenticated = true;
                }
            }
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: JediOmniAppAuthenticatorBase::OnOxpdApplicationScreen::On Oxpd App Screen: {otherScreen}");

            return otherScreen;
        }


        /// <summary>
        /// Whether or not at the home screen.
        /// </summary>
        /// <returns>bool</returns>
        protected bool AtHomeScreen()
        {
            return ControlPanel.CheckState($".hp-homescreen-folder-view{_topViewAttribute}", OmniElementState.Exists)
                && ControlPanel.CheckState(_homeScreenLogo, OmniElementState.VisibleCompletely);
        }

    }
}
