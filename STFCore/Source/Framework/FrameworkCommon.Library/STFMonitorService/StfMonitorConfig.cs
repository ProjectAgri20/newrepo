using System;
using System.Text;

namespace HP.ScalableTest.Framework.Monitor
{
    /// <summary>
    /// Configuration options for an STF Monitor.
    /// This is the default configuration for a monitor.  At the very least,
    /// a monitor needs a location to monitor.  If a monitor needs additional
    /// configuration to run, it will need to inherit from this class.
    /// </summary>
    [Serializable]
    public class StfMonitorConfig
    {
        /// <summary>
        /// Gets or sets the location to monitor. (Folder path, Email address, Database server, etc.)
        /// </summary>
        public string MonitorLocation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StfMonitorConfig"/> class.
        /// </summary>
        public StfMonitorConfig()
        {
            MonitorLocation = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StfMonitorConfig"/> class.
        /// </summary>
        /// <param name="monitorLocation"></param>
        public StfMonitorConfig(string monitorLocation)
        {
            MonitorLocation = monitorLocation;
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
            return result.ToString();
        }

    }
}
