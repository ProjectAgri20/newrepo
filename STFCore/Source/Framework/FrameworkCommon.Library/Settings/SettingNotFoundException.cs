using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Settings
{
    /// <summary>
    /// Exception that is thrown when an STF setting cannot be found.
    /// </summary>
    [Serializable]
    public class SettingNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingNotFoundException"/> class.
        /// </summary>
        public SettingNotFoundException()
            :base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SettingNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SettingNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingNotFoundException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        ///   
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected SettingNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
