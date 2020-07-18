using System;
using System.Threading;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Helpers.JediOmni
{
    /// <summary>
    /// Helper class that provides methods for launching Omni apps.
    /// </summary>
    public sealed class JediOmniLaunchHelper : DeviceWorkflowLogSource
    {
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniNotificationPanel _notificationPanel;

        /// <summary>
        /// Gets screen used to validate successful lazy authentication
        /// </summary>
        public const string LazySuccessScreen = ".hp-app, .hp-oxpd-app-screen";

        /// <summary>
        /// Gets the ID for the Sign In/Out button
        /// </summary>
        public const string SignInOrSignoutButton = "#hp-button-signin-or-signout";

        /// <summary>
        /// Gets Sign in form ID
        /// </summary>
        public const string SignInForm = "#hpid-signin-body";

        /// <summary>
        /// Gets or sets the pacekeeper for this launch helper.
        /// </summary>
        public Pacekeeper Pacekeeper { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniLaunchHelper" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device" /> is null.</exception>
        public JediOmniLaunchHelper(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _controlPanel = device.ControlPanel;
            _notificationPanel = new JediOmniNotificationPanel(device);
        }

        /// <summary>
        /// Presses the sign in button and waits for the Sign In form to appear.
        /// Eager Authentication
        /// </summary>
        public void PressSignInButton()
        {
            try
            {
                string value = _controlPanel.GetValue(SignInOrSignoutButton, "innerText", OmniPropertyType.Property).Trim();
                if (value.Contains("Sign In"))
                {
                    if (_controlPanel.WaitForAvailable(SignInOrSignoutButton))
                    {
                        RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, "Sign In");
                        if( !_controlPanel.PressWait(SignInOrSignoutButton, SignInForm))
                        {
                            if (!_notificationPanel.WaitForNotDisplaying("Initializing scanner", "Clearing settings"))
                            {
                                throw new DeviceWorkflowException($"The device did not come to a ready state within {_controlPanel.DefaultWaitTime.TotalSeconds + 30} seconds.");
                            }
                        }
                    }
                    else if (_notificationPanel.MessageDisplayed("Initializing scanner", "Clearing settings"))
                    {
                        _notificationPanel.WaitForNotDisplaying("Initializing scanner", "Clearing settings");
                        RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, "Sign In");
                        if (!_controlPanel.PressWait(SignInOrSignoutButton, SignInForm))
                        {
                            if (!_notificationPanel.WaitForNotDisplaying("Initializing scanner", "Clearing settings"))
                            {
                                throw new DeviceWorkflowException($"The device did not come to a ready state within {_controlPanel.DefaultWaitTime.TotalSeconds + 30} seconds.");
                            }
                        }
                    }
                    else
                    {
                        throw new DeviceWorkflowException("Unable to press the 'Sign In' Button");
                    }
                }
              
            }
            catch(Exception ex)
            {
                if (_notificationPanel.MessageDisplayed("Initializing scanner", "Clearing settings"))
                {
                    // Sometimes the scanner messages pops up right after the sign in button becomes available.
                    _notificationPanel.WaitForNotDisplaying("Initializing scanner", "Clearing settings");
                    PressSignInButton();
                }
                else
                {
                    throw ex;
                }
            }
        }
        /// <summary>Presses the solution button based on given title. Waits for the given form name to appear.</summary>
        /// <param name="solutionTitle">The solution title of desired button.</param>
        /// <param name=""></param>
        /// <param name="eventRecorderInfo">title for the RecordEvent</param>
        /// <param name="waitForm">The form that sould appear after button press.</param>
        /// <exception cref="DeviceWorkflowException">Device not responding after pressing the {solutionTitle} button
        /// or
        /// Not on the homescreen. Can not press the button {solutionTitle}.</exception>
        public void PressSolutionButton(string solutionTitle, string eventRecorderInfo, string waitForm)
        {
            string elementId = ".hp-homescreen-button:contains(" + solutionTitle + "):first";

            if (_controlPanel.WaitForState(".hp-homescreen-folder-view", OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(5)))
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, eventRecorderInfo);
                if (!_controlPanel.ScrollPressWait(elementId, waitForm))
                {
                    if (_notificationPanel.MessageDisplayed("Initializing scanner", "Clearing settings"))
                    {
                        _notificationPanel.WaitForNotDisplaying("Initializing scanner", "Clearing settings");
                    }
                    else
                    {
                        throw new DeviceWorkflowException($"Device not responding after pressing the {solutionTitle} button");
                    }
                }
            }
            else
            {
                throw new DeviceWorkflowException($"Not on the homescreen. Can not press the button {solutionTitle}.");
            }
        }

        /// <summary>
        /// Presses the application button.
        /// </summary>
        /// <param name="appName">The application name (used for logging).</param>
        /// <param name="appButton">The application button selector.</param>
        /// <param name="expectedDestination">The expected destination selector.</param>
        /// <returns><c>true</c> if the expected destination was reached, <c>false</c> otherwise.</returns>
        public bool PressAppButton(string appName, string appButton, string expectedDestination)
        {
            return PressAppButton(appName, appButton, null, expectedDestination);
        }

        /// <summary>
        /// Presses the application button.
        /// </summary>
        /// <param name="appName">The application name (used for logging).</param>
        /// <param name="appButton">The application button selector.</param>
        /// <param name="folderButton">The folder button selector, if the app button could be inside a home screen folder.</param>
        /// <param name="expectedDestination">The expected destination selector.</param>
        /// <returns><c>true</c> if the expected destination was reached, <c>false</c> otherwise.</returns>
        public bool PressAppButton(string appName, string appButton, string folderButton, string expectedDestination)
        {
            // Find the app button
            if (_controlPanel.CheckState(appButton, OmniElementState.Exists))
            {
                _controlPanel.ScrollToItem(appButton);
                Pacekeeper.Sync();
            }
            else if (folderButton != null && _controlPanel.CheckState(folderButton, OmniElementState.Exists))
            {
                _controlPanel.ScrollPress(folderButton);
                if (_controlPanel.WaitForState(appButton, OmniElementState.Exists, TimeSpan.FromSeconds(10)))
                {
                    Pacekeeper.Pause();
                    _controlPanel.ScrollToItem(appButton);
                    Pacekeeper.Sync();
                }
                else
                {
                    throw new DeviceWorkflowException($"{appName} application button was not found on device home screen or in standard folder.");
                }
            }
            else
            {
                throw new DeviceWorkflowException($"{appName} application button was not found on device home screen.");
            }

            // Press the app button
            RecordEvent(DeviceWorkflowMarker.AppButtonPress, appName);
            return _controlPanel.ScrollPressWait(appButton, expectedDestination);
        }
    }
}
