using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.Helpers.JediOmni
{
    /// <summary>
    /// Manages <see cref="JediOmniDevice" /> popup handling.
    /// </summary>
    public sealed class JediOmniPopupManager
    {
        private readonly JediOmniControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniPopupManager"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniPopupManager(JediOmniDevice device)
        {
            _controlPanel = device.ControlPanel;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniPopupManager"/> class.
        /// </summary>
        /// <param name="controlPanel">The control panel.</param>
        public JediOmniPopupManager(JediOmniControlPanel controlPanel)
        {
            _controlPanel = controlPanel;
        }

        /// <summary>
        /// Determines whether the screen is displaying a popup.
        /// </summary>
        /// <returns><c>true</c> if there is a popup displayed; otherwise, <c>false</c>.</returns>
        public bool PopupDisplayed()
        {
            return _controlPanel.CheckState(".hp-popup-modal-overlay,.hp-prompt", OmniElementState.Exists);
        }

        /// <summary>
        /// Determines whether the screen is displaying a popup containing the specified text.
        /// </summary>
        /// <param name="popupText">The text to look for on the popup.</param>
        /// <returns><c>true</c> if there is a popup with the specified text; otherwise, <c>false</c>.</returns>
        public bool PopupDisplayed(string popupText)
        {
            return GetPopupText()?.Contains(popupText, StringComparison.OrdinalIgnoreCase) ?? false;
        }

        /// <summary>
        /// Gets the text displayed on a popup, if one exists.
        /// </summary>
        /// <returns>If there is a popup on the control panel, the text of the popup; otherwise, null.</returns>
        public string GetPopupText()
        {
            if (PopupDisplayed())
            {
                try
                {
                    return _controlPanel.GetValue(".hp-popup-content:last", "textContent", OmniPropertyType.Property);
                }
                catch (DeviceInvalidOperationException)
                {
                    // Race condition: popup disappeared while we were checking
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Determines whether the specified text exists in a popup message.
        /// </summary>
        /// <param name="text">The text contained in the popup message.</param>
        /// <returns><c>true</c> if the specified text is contained in the popup message, otherwise, <c>false</c>.</returns>
        public bool PopupTextContains(string text)
        {
            string popupText = GetPopupText();
            return (!string.IsNullOrEmpty(popupText) && popupText.Contains(text, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Waits for the screen to display a popup.
        /// </summary>
        /// <param name="waitTime">The amount of time to wait for a popup.</param>
        /// <returns><c>true</c> if a popup appeared within the specified time; otherwise, <c>false</c>.</returns>
        public bool WaitForPopup(TimeSpan waitTime)
        {
            return Wait.ForTrue(PopupDisplayed, waitTime);
        }

        /// <summary>
        /// Waits for the screen to display a popup containing the specified text.
        /// </summary>
        /// <param name="popupText">The text to look for on the popup.</param>
        /// <param name="waitTime">The amount of time to wait for a popup.</param>
        /// <returns><c>true</c> if a popup with the specified text appeared within the specified time; otherwise, <c>false</c>.</returns>
        public bool WaitForPopup(string popupText, TimeSpan waitTime)
        {
            return Wait.ForTrue(() => PopupDisplayed(popupText), waitTime);
        }

        /// <summary>
        /// Handles a popup that will go away on its own.
        /// </summary>
        /// <param name="waitTime">The amount of time to wait for the popup to disappear.</param>
        /// <returns><c>true</c> if a temporary popup was found and handled; otherwise, <c>false</c>.</returns>
        public bool HandleTemporaryPopup(TimeSpan waitTime)
        {
            return Wait.ForFalse(PopupDisplayed, waitTime, TimeSpan.FromMilliseconds(250));
        }

        /// <summary>
        /// Handles a popup with the specified text that will go away on its own.
        /// </summary>
        /// <param name="popupText">The text to look for on the popup.</param>
        /// <param name="waitTime">The amount of time to wait for the popup to disappear.</param>
        /// <returns><c>true</c> if a temporary popup with the specified text was found and handled; otherwise, <c>false</c>.</returns>
        public bool HandleTemporaryPopup(string popupText, TimeSpan waitTime)
        {
            return PopupDisplayed(popupText) && Wait.ForFalse(() => PopupDisplayed(popupText), waitTime, TimeSpan.FromMilliseconds(250));
        }

        /// <summary>
        /// Handles the popup asking if the users wants to cancel the current job.
        /// </summary>
        /// <param name="cancelJob">if set to <c>true</c> cancel the job if prompted.</param>
        /// <returns><c>true</c> if the popup was found and handled; otherwise, <c>false</c>.</returns>
        public bool HandleCancelJobPopup(bool cancelJob)
        {
            return HandlePopup("#hpid-cancel-confirm", cancelJob ? "#hpid-button-yes" : "#hpid-button-no");
        }

        /// <summary>
        /// Handles the popup asking whether to retain settings for the next job.
        /// </summary>
        /// <param name="retainSettings">if set to <c>true</c> retain settings; otherwise, clear settings.</param>
        /// <returns><c>true</c> if the popup was found and handled; otherwise, <c>false</c>.</returns>
        public bool HandleRetainSettingsPopup(bool retainSettings)
        {
            return HandlePopup("#hpid-retain-settings-popup", retainSettings ? "#hpid-retain-settings-retain-button" : "#hpid-retain-settings-clear-button");
        }

        /// <summary>
        /// Handles the popup asking whether to add contacts to the address book.
        /// </summary>
        /// <param name="addContact">if set to <c>true</c> add the contact; otherwise, ignore.</param>
        /// <returns><c>true</c> if the popup was found and handled; otherwise, <c>false</c>.</returns>
        public bool HandleAddContactPopup(bool addContact)
        {
            return HandlePopup("#hpid-add-new-email-contacts-popup", addContact ? "#hpid-add-selected-contacts-button" : "#hpid-close-add-selected-contacts-button");
        }

        /// <summary>
        /// Handles the popup asking whether to replace an existing file.
        /// </summary>
        /// <param name="replaceExisting">if set to <c>true</c> replace the existing file; otherwise, cancel.</param>
        /// <returns><c>true</c> if the popup was found and handled; otherwise, <c>false</c>.</returns>
        public bool HandleFileAlreadyExistsPopup(bool replaceExisting)
        {
            return HandlePopup("#hpid-file-already-exists-popup", replaceExisting ? "#hpid-file-already-exists-popup-replace" : "#hpid-file-already-exists-popup-cancel");
        }

        /// <summary>
        /// Handles the prompt indicating that the flatbed cannot detect the original size.
        /// </summary>
        /// <returns><c>true</c> if the popup was found and handled; otherwise, <c>false</c>.</returns>
        public bool HandleFlatbedAutodetectPrompt()
        {
            return HandlePopup("#hpid-prompt-flatbed-autodetect", "#hpid-button-continue");
        }

        /// <summary>
        /// Handles a prompt asking whether the user is done scanning pages.
        /// </summary>
        /// <param name="scanMore">if set to <c>true</c> press "Scan"; otherwise, press "Done.".</param>
        /// <returns><c>true</c> if the popup was found and handled; otherwise, <c>false</c>.</returns>
        public bool HandleScanMorePrompt(bool scanMore)
        {
            return HandlePopup(".hp-prompt", scanMore ? "#hpid-button-scan" : "#hpid-button-done");
        }

        /// <summary>
        /// Handles a prompt asking whether the user is done scanning pages.
        /// </summary>
        /// <returns><c>true</c> if the popup was found and handled; otherwise, <c>false</c>.</returns>
        public bool HandleMaintananceUnavailablePopUp()
        {
            return HandlePopup("#hpid-maintenancemode-failed-feedback-popup", "#hpid-button-ok");
        }

        /// <summary>
        /// Presses the specified Okay button on a popup if the "contains" text exists.
        /// </summary>
        /// <param name="okButtonSelector">The selector for the OK button.</param>
        /// <param name="popupContainsText">A text value that is expected to exist in the popup display text.</param>
        /// <returns><c>true</c> if the popup was found and handled; otherwise, <c>false</c>.</returns>
        public bool HandleButtonOk(string okButtonSelector, string popupContainsText)
        {
            if (PopupTextContains(popupContainsText))
            {
                return HandleButtonOk(okButtonSelector);
            }
            return false;
        }

        /// <summary>
        /// Presses the Okay button on a popup using the specified selector.
        /// </summary>
        /// <param name="okButtonSelector"></param>
        /// <returns><c>true</c> if the OK button was found and handled; otherwise, <c>false</c>.</returns>
        public bool HandleButtonOk(string okButtonSelector)
        {
            try
            {
                _controlPanel.Press(okButtonSelector);
                return true;
            }
            catch (ElementNotFoundException)
            {
            }
            return false;
        }

        private bool HandlePopup(string popupSelector, string buttonSelector)
        {
            if (_controlPanel.CheckState(popupSelector, OmniElementState.Exists) && _controlPanel.WaitForState(buttonSelector, OmniElementState.Useable, TimeSpan.FromSeconds(5)))
            {
                _controlPanel.Press(buttonSelector);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
