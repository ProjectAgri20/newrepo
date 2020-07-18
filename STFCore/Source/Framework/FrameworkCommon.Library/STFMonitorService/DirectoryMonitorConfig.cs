using System;
using System.Text;

namespace HP.ScalableTest.Framework.Monitor
{
    /// <summary>
    /// Configuration options for monitoring directory files.
    /// </summary>
    [Serializable]
    public class DirectoryMonitorConfig : StfMonitorConfig
    {
        /// <summary>
        /// Gets or sets the HostName of the Server hosting the log service.
        /// </summary>
        public string LogServiceHostName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryMonitorConfig"/> class.
        /// </summary>
        public DirectoryMonitorConfig() 
            : base()
        {
            LogServiceHostName = null;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder("Monitor ");
            result.Append(MonitorLocation);
            result.Append(".  Log to ");
            result.Append(LogServiceHostName);
            return result.ToString();
        }
    }
}
