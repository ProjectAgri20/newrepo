using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.Helpers.JediOmni
{
    /// <summary>
    /// Helper class that provides methods for working with the Omni masthead.
    /// </summary>
    public sealed class JediOmniMasthead
    {
        private const string _mastheadTitle = ".hp-masthead-title:last";
        private const string _activeJobsButton = ".hp-button-active-jobs:last";
        private const string _mastheadSpinner = "#hpid-oxpd-spinner";

        private readonly JediOmniControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniMasthead" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device" /> is null.</exception>
        public JediOmniMasthead(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _controlPanel = device.ControlPanel;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniMasthead"/> class.
        /// </summary>
        /// <param name="controlPanel">The control panel.</param>
        public JediOmniMasthead(JediOmniControlPanel controlPanel)
        {
            if (controlPanel == null)
            {
                throw new ArgumentNullException(nameof(controlPanel));
            }
            _controlPanel = controlPanel;
        }

        /// <summary>
        /// Gets the masthead spinner ID string.
        /// </summary>
        /// <value>
        /// The masthead spinner.
        /// </value>
        public string MastheadSpinner 
        {
            get { return _mastheadSpinner; }
        }

        /// <summary>
        /// Determines whether the masthead contains the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns><c>true</c> if the masthead contains the specified text; otherwise, <c>false</c>.</returns>
        public bool ContainsText(string text)
        {
            try
            {
                return _controlPanel.GetValue(_mastheadTitle, "innerText", OmniPropertyType.Property).Contains(text, StringComparison.OrdinalIgnoreCase);
            }
            catch (ElementNotFoundException)
            {
                return false;
            }
        }

        /// <summary>
        /// Waits for the masthead to contain the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="waitTime">The amount of time to wait for the specified text.</param>
        /// <returns><c>true</c> if the masthead contained the specified text, <c>false</c> otherwise.</returns>
        public bool WaitForText(string text, TimeSpan waitTime)
        {
            return Wait.ForTrue(() => ContainsText(text), waitTime);
        }

        /// <summary>
        /// Determines whether the Active Jobs button is visible.
        /// </summary>
        /// <returns><c>true</c> if the active jobs button is visible, <c>false</c> otherwise.</returns>
        public bool ActiveJobsButtonVisible()
        {
            return _controlPanel.GetValue(_activeJobsButton, "display", OmniPropertyType.Css) != "none";
        }

        /// <summary>
        /// Busies the state active.
        /// </summary>
        /// <returns></returns>
        public bool BusyStateActive()
        {
            return _controlPanel.GetValue(_mastheadSpinner, "display", OmniPropertyType.Css) != "none";
        }

        /// <summary>
        /// Waits the state of for busy.
        /// </summary>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        /// <param name="waitTime">The wait time.</param>
        /// <returns></returns>
        public bool WaitForBusyState(bool visible, TimeSpan waitTime)
        {
            return Wait.ForEquals(BusyStateActive, visible, waitTime, TimeSpan.FromMilliseconds(250));
        }

        /// <summary>
        /// Waits for the Active Jobs button to match the specified state.
        /// </summary>
        /// <param name="visible">if set to <c>true</c> wait for the button to visible; otherwise, wait for the button to not be visible.</param>
        /// <param name="waitTime">The amount of time to wait for the Active Jobs button to be in the specified state.</param>
        /// <returns><c>true</c> if the Active Jobs button was in the specified state, <c>false</c> otherwise.</returns>
        public bool WaitForActiveJobsButtonState(bool visible, TimeSpan waitTime)
        {
            return Wait.ForEquals(ActiveJobsButtonVisible, visible, waitTime, TimeSpan.FromMilliseconds(250));
        }
    }
}
