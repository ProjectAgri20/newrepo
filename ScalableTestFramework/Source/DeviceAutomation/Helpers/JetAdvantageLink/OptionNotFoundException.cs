using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink
{
    /// <summary>
    /// Exception thrown when an option could not be retrieved from the device control panel
    /// </summary>
    [Serializable]
    public class OptionNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionNotFoundException "/> class.
        /// </summary>
        public OptionNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionNotFoundException "/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public OptionNotFoundException(string message) :
            base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionNotFoundException "/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The exception.</param>
        public OptionNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionNotFoundException "/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected OptionNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

}
