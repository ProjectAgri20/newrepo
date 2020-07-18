using System;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    /// <summary>
    /// Allows activity execution engines to notify listeners of status changes.
    /// </summary>
    public static class EngineEventBus
    {
        /// <summary>
        /// Occurs when an activity has posted a new status message.
        /// </summary>
        public static event EventHandler<StatusChangedEventArgs> ActivityStatusMessageChanged;

        /// <summary>
        /// Fires the <see cref="ActivityStatusMessageChanged"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        public static void OnActivityStatusMessageChanged(object sender, string message)
        {
            if (ActivityStatusMessageChanged != null)
            {
                ActivityStatusMessageChanged(sender, new StatusChangedEventArgs(message));
            }
        }
    }
}
