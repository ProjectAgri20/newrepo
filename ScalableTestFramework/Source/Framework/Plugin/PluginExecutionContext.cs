using System;

namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// Contextual information about a plugin execution.
    /// </summary>
    public sealed class PluginExecutionContext
    {
        /// <summary>
        /// Gets or sets the unique identifier for the current activity execution.
        /// </summary>
        public Guid ActivityExecutionId { get; set; }

        /// <summary>
        /// Gets the session ID associated with execution of this plugin.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Provides a user name for use within the associated automation plugin.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Provides a user password for use within the associated automation plugin.
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// Provides a user domain for use within the associated automation plugin.
        /// </summary>
        public string UserDomain { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginExecutionContext" /> class.
        /// </summary>
        public PluginExecutionContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginExecutionContext" /> class.
        /// </summary>
        /// <param name="activityExecutionId">The activity execution ID.</param>
        /// <param name="sessionId">The session ID.</param>
        public PluginExecutionContext(Guid activityExecutionId, string sessionId)
        {
            ActivityExecutionId = activityExecutionId;
            SessionId = sessionId;
        }
    }
}
