using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.PluginSupport.PullPrint.Simulation
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class EmptyQueueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyQueueException"/> class.
        /// </summary>
        public EmptyQueueException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyQueueException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EmptyQueueException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyQueueException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public EmptyQueueException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyQueueException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected EmptyQueueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
