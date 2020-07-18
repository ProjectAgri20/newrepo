﻿using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Core.AssetInventory.Reservation
{
    /// <summary>
    /// Exception thrown when a resource reservation cannot be completed.
    /// </summary>
    [Serializable]
    public class ResourceReservationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceReservationException" /> class.
        /// </summary>
        public ResourceReservationException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceReservationException" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ResourceReservationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceReservationException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public ResourceReservationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceReservationException" /> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected ResourceReservationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
