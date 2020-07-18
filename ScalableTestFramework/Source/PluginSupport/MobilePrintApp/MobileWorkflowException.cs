using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.PluginSupport.MobilePrintApp
{
    /// <summary>
    /// Workflow exception while Mobile (Android, iOS, Windows Phone) workflow.
    /// </summary>
    [Serializable]
    public class MobileWorkflowException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileWorkflowException "/> class.
        /// </summary>
        public MobileWorkflowException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileWorkflowException "/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MobileWorkflowException(string message) : base(message)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MobileWorkflowException "/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The exception.</param>
        public MobileWorkflowException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileWorkflowException "/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected MobileWorkflowException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
