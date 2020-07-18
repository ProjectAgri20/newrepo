using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation
{
    /// <summary>
    /// Contains extension methods for DAT device control panels.
    /// </summary>
    /// <remarks>
    /// Methods in this class should be narrow in scope and useable across many workflows.
    /// Code that is more complex or specific to one area (e.g. job execution) should be placed
    /// in a more specific helper class.
    /// </remarks>
    public static class ControlPanelExtension
    {
        /// <summary>
        /// Waits for the specified element to be visible, enabled, and stationary.
        /// Uses <see cref="JediOmniControlPanel.DefaultWaitTime" /> as the length of time to wait.
        /// </summary>
        /// <param name="controlPanel">The control panel.</param>
        /// <param name="elementSelector">The element selector.</param>
        /// <returns><c>true</c> if the element was available within the specified time, <c>false</c> otherwise.</returns>
        public static bool WaitForAvailable(this JediOmniControlPanel controlPanel, string elementSelector)
        {
            return WaitForAvailable(controlPanel, elementSelector, controlPanel.DefaultWaitTime);
        }

        /// <summary>
        /// Waits for the specified element to be visible, enabled, and stationary.
        /// </summary>
        /// <param name="controlPanel">The control panel.</param>
        /// <param name="elementSelector">The element selector.</param>
        /// <param name="waitTime">The amount of time to wait for the element to be available.</param>
        /// <returns><c>true</c> if the element was available within the specified time, <c>false</c> otherwise.</returns>
        public static bool WaitForAvailable(this JediOmniControlPanel controlPanel, string elementSelector, TimeSpan waitTime)
        {
            bool isReady()
            {
                return controlPanel.CheckState(elementSelector, OmniElementState.Enabled)
                    && controlPanel.CheckState(elementSelector, OmniElementState.Useable);
            }

            return Wait.ForTrue(isReady, waitTime);
        }

        /// <summary>
        /// Gets the size of the control panel screen.
        /// </summary>
        /// <param name="controlPanel">The control panel.</param>
        /// <returns>The size of the control panel.</returns>
        public static Size GetScreenSize(this JediOmniControlPanel controlPanel)
        {
            return controlPanel.GetBoundingBox(":root").Size;
        }
    }
}
