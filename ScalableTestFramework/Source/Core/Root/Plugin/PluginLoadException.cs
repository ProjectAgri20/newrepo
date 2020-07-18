using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Core.Plugin
{
    /// <summary>
    /// Exception thrown when an attempt to load a plugin assembly fails.
    /// </summary>
    [Serializable]
    public class PluginLoadException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoadException" /> class.
        /// </summary>
        public PluginLoadException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoadException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PluginLoadException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoadException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public PluginLoadException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoadException" /> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected PluginLoadException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
