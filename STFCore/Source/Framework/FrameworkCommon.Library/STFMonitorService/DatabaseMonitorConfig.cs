using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Framework.Monitor
{
    /// <summary>
    /// Configuration options for monitoring a database.
    /// </summary>
    [Serializable]
    public class DatabaseMonitorConfig : StfMonitorConfig
    {
        /// <summary>
        /// Creates a new instance of DatabaseMonitorConfig.
        /// </summary>
        public DatabaseMonitorConfig()
        {
            ConnectionPort = -1;
        }

        /// <summary>
        /// Gets or sets the Database Instance Name
        /// </summary>
        public string DatabaseInstanceName { get; set; }

        /// <summary>
        /// Gets or sets the port used by the database.
        /// </summary>
        public int ConnectionPort { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder("Monitor database on ");
            result.Append(MonitorLocation);
            result.Append("\\");
            result.Append(DatabaseInstanceName);
            return result.ToString();
        }
    }
}
