using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// Exception thrown when an <see cref="IDataLoggerMock" /> detects an issue with a submitted data log.
    /// </summary>
    [Serializable]
    public class DataLoggerMockException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataLoggerMockException" /> class.
        /// </summary>
        public DataLoggerMockException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLoggerMockException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DataLoggerMockException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLoggerMockException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public DataLoggerMockException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLoggerMockException" /> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected DataLoggerMockException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
