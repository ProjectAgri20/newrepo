using System;

namespace HP.ScalableTest.Service.Monitor
{
    /// <summary>
    /// Class to hold information about registered Sessions
    /// </summary>
    public class RegisteredSessionInfo
    {
        /// <summary>
        /// Creates a new instance of <see cref="RegisteredSessionInfo" />
        /// </summary>
        /// <param name="logServiceHostName"></param>
        public RegisteredSessionInfo(string logServiceHostName)
        {
            LogServiceHostName = logServiceHostName;
            LastUsed = DateTime.Now;
        }

        /// <summary>
        /// Gets the Data Log Service HostName
        /// </summary>
        public string LogServiceHostName { get; }

        /// <summary>
        /// Gets or sets the last DateTime this object was accessed.
        /// </summary>
        public DateTime LastUsed { get; set; }
    }
}
