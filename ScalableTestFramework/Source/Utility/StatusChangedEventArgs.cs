using System;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Event args used for simple status change messages.
    /// </summary>
    public class StatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the status message.
        /// </summary>
        public string StatusMessage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusChangedEventArgs" /> class.
        /// </summary>
        /// <param name="statusMessage">The status message.</param>
        public StatusChangedEventArgs(string statusMessage)
        {
            StatusMessage = statusMessage;
        }
    }
}
