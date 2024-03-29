﻿using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    /// <summary>
    /// An exception type that indicates that the worker is aborting the run of activities.
    /// </summary>
    [Serializable]
    public class WorkerHaltedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerHaltedException"/> class.
        /// </summary>
        public WorkerHaltedException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerHaltedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WorkerHaltedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerHaltedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public WorkerHaltedException(string message, Exception ex)
            : base(message, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkerHaltedException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected WorkerHaltedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
