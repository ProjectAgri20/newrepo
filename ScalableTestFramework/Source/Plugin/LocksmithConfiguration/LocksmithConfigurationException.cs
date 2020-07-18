using System;

namespace HP.ScalableTest.Plugin.LocksmithConfiguration
{
    [Serializable]
    class LocksmithConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocksmithConfigurationExeception" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public LocksmithConfigurationException(string message) : base(message)
        {
        }        
    }
}

