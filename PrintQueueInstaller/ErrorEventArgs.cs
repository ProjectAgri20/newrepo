using System;

namespace HP.ScalableTest.Print.Utility
{
    /// <summary>
    /// Provides a status message
    /// </summary>
    class ErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEventArgs"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public ErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
