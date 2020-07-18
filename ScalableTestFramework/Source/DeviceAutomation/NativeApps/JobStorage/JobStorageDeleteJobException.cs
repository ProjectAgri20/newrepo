using System;

namespace HP.ScalableTest.DeviceAutomation
    {
    /// <summary>
    /// Exception thrown when an error occurs during a JobStorage Delete operation.
    /// </summary>
    [Serializable]
    public class JobStorageDeleteJobExeception : Exception
        { 
        /// <summary>
        /// Initializes a new instance of the <see cref="JobStorageDeleteJobExeception" /> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public JobStorageDeleteJobExeception(string message)
                : base(message)
            {
            }
        }
    }
