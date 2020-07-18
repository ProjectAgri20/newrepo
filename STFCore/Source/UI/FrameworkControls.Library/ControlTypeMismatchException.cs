using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.UI
{
    /// <summary>
    /// Exception thrown if an object of incorrect type is passed to a control.
    /// </summary>
    [Serializable]
    public class ControlTypeMismatchException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlTypeMismatchException"/> class.
        /// </summary>
        public ControlTypeMismatchException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlTypeMismatchException"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="expectedType">The expected type.</param>
        public ControlTypeMismatchException(object entity, Type expectedType)
            : base(GetMessage(entity, expectedType))
        {
        }

        private static string GetMessage(object entity, Type expectedType)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (expectedType == null)
            {
                throw new ArgumentNullException("expectedType");
            }

            return "Object was of type {0}; expected object of type {1}."
                .FormatWith(entity.GetType().Name, expectedType.Name);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlTypeMismatchException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ControlTypeMismatchException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlTypeMismatchException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ControlTypeMismatchException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlTypeMismatchException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        ///   
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected ControlTypeMismatchException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
