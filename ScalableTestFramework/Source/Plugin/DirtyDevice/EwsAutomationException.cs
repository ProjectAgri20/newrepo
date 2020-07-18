using System;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    /// <summary>
    /// Exception thrown when there is an error automating the devices EWS.
    /// </summary>
    public class EwsAutomationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EwsAutomationException"/> class.
        /// </summary>
        public EwsAutomationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EwsAutomationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public EwsAutomationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EwsAutomationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public EwsAutomationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
