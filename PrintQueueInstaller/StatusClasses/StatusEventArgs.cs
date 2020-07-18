using System;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Provides a status message
    /// </summary>
    class StatusEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        public StatusEventType EventType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type of event.</param>
        public StatusEventArgs(string message, StatusEventType type)
        {
            Message = message;
            EventType = type;
        }
    }
}
