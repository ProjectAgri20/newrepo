using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Synchronization
{
    /// <summary>
    /// Exception thrown when an <see cref="ICriticalSection" /> lock is held for longer than the specified <see cref="LockToken.HoldTimeout" />.
    /// </summary>
    [Serializable]
    public class HoldLockTimeoutException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HoldLockTimeoutException" /> class.
        /// </summary>
        public HoldLockTimeoutException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HoldLockTimeoutException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public HoldLockTimeoutException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HoldLockTimeoutException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public HoldLockTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HoldLockTimeoutException" /> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected HoldLockTimeoutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
