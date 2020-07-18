using System;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.Helpers.JediOmni
{
    /// <summary>
    /// Helper class that provides methods for working with the Omni notification panel.
    /// </summary>
    public sealed class JediOmniNotificationPanel
    {
        private const string _notificationPanel = ".hp-notification-panel:last";
        private const string _notificationPanelMessage = ".hp-notification-message:last";

        private readonly JediOmniControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniNotificationPanel" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device" /> is null.</exception>
        public JediOmniNotificationPanel(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _controlPanel = device.ControlPanel;
        }

        /// <summary>
        /// Waits for the notification panel to have the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns><c>true</c> if the notification panel had the specified state within the timeout, <c>false</c> otherwise.</returns>
        public bool WaitForState(OmniElementState state)
        {
            return _controlPanel.WaitForState(_notificationPanel, state);
        }

        /// <summary>
        /// Waits for the notification panel to have the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="waitTime">The amount of time to wait for the specified state.</param>
        /// <returns><c>true</c> if the notification panel had the specified state within the timeout, <c>false</c> otherwise.</returns>
        public bool WaitForState(OmniElementState state, TimeSpan waitTime)
        {
            return _controlPanel.WaitForState(_notificationPanel, state, waitTime);
        }

        /// <summary>
        /// Determines whether one of the specified messages is currently displayed on the notification panel.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns><c>true</c> if one of the messages is displayed; otherwise, <c>false</c>.</returns>
        public bool MessageDisplayed(params string[] messages)
        {
            try
            {
                string value = _controlPanel.GetValue(_notificationPanelMessage, "innerText", OmniPropertyType.Property);
                return messages.Any(n => value.Contains(n, StringComparison.OrdinalIgnoreCase));
            }
            catch (ElementNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Waits for the notification panel to be displaying one of the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns><c>true</c> if the notification panel displayed one of the messages within the timeout, <c>false</c> otherwise.</returns>
        public bool WaitForDisplaying(params string[] messages)
        {
            return Wait.ForTrue(() => MessageDisplayed(messages), _controlPanel.DefaultWaitTime, TimeSpan.FromMilliseconds(250));
        }
        /// <summary>
        /// Waits for the notification panel to be displaying one of the specified messages.
        /// </summary>
        /// <param name="waitTime">The wait time.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>
        ///   <c>true</c> if the notification panel displayed one of the messages within the timeout, <c>false</c> otherwise.
        /// </returns>
        public bool WaitForDisplaying(TimeSpan waitTime, params string[] messages)
        {
            return Wait.ForTrue(() => MessageDisplayed(messages), waitTime, TimeSpan.FromMilliseconds(250));
        }

        /// <summary>
        /// Waits for the notification panel to not be displaying any of the specified messages.
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns><c>true</c> if the notification panel stopped displaying any of the messages within the timeout, <c>false</c> otherwise.</returns>
        public bool WaitForNotDisplaying(params string[] messages)
        {
            return Wait.ForFalse(() => MessageDisplayed(messages), _controlPanel.DefaultWaitTime, TimeSpan.FromMilliseconds(250));
        }

        /// <summary>
        /// Waits for the notification panel to not be displaying any of the specified messages.
        /// </summary>
        /// <param name="waitTime">The amount of time to wait.</param>
        /// <param name="messages">The messages.</param>
        /// <returns><c>true</c> if the notification panel stopped displaying any of the messages within the timeout, <c>false</c> otherwise.</returns>
        public bool WaitForNotDisplaying(TimeSpan waitTime, params string[] messages)
        {
            return Wait.ForFalse(() => MessageDisplayed(messages), waitTime, TimeSpan.FromMilliseconds(250));
        }
    }
}
