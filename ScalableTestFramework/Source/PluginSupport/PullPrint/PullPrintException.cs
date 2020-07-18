using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.PluginSupport.PullPrint
{
    /// <summary>
    /// Customized Exception thrown when no jobs are displayed in the device control panel.
    /// </summary>
    [Serializable]
    public class NoJobsFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoJobsFoundException"/> class.
        /// </summary>
        public NoJobsFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoJobsFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NoJobsFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoJobsFoundException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public NoJobsFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoJobsFoundException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected NoJobsFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    /// <summary>
    /// Customized Exception thrown when the Plugin times out before the Print All operation is finished.
    /// </summary>
    [Serializable]
    public class PullPrintTimeoutException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PullPrintTimeoutException"/> class.
        /// </summary>
        public PullPrintTimeoutException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PullPrintTimeoutException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PullPrintTimeoutException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PullPrintTimeoutException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public PullPrintTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PullPrintTimeoutException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected PullPrintTimeoutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
