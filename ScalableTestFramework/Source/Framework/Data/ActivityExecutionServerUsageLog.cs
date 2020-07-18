using System;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Framework.Data
{
    /// <summary>
    /// Logs usage of a server by a plugin activity.
    /// </summary>
    public sealed class ActivityExecutionServerUsageLog : ActivityDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "ActivityExecutionServerUsage";

        /// <summary>
        /// Gets the server name.
        /// </summary>
        [DataLogProperty]
        public string ServerName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityExecutionServerUsageLog" /> class.
        /// </summary>
        /// <param name="executionData">The <see cref="PluginExecutionData" />.</param>
        /// <param name="serverInfo">The <see cref="ServerInfo" /> for the server used by the activity.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serverInfo" /> is null.</exception>
        public ActivityExecutionServerUsageLog(PluginExecutionData executionData, ServerInfo serverInfo)
            : base(executionData)
        {
            if (serverInfo == null)
            {
                throw new ArgumentNullException(nameof(serverInfo));
            }

            ServerName = serverInfo.HostName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityExecutionServerUsageLog" /> class.
        /// </summary>
        /// <param name="executionData">The <see cref="PluginExecutionData" />.</param>
        /// <param name="serverName">The name of the server used by the activity.</param>
        public ActivityExecutionServerUsageLog(PluginExecutionData executionData, string serverName)
            : base(executionData)
        {
            ServerName = serverName;
        }
    }
}
