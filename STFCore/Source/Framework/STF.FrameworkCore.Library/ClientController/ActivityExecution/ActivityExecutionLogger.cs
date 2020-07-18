using System;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    /// <summary>
    /// Logging class to assist with writing data to the ActivityExecution table in the ScalableTestDataLog database.
    /// </summary>
    public class ActivityExecutionLogger : FrameworkDataLog
    {
        private const string _defaultCategory = "Uncategorized";

        public override string TableName => "ActivityExecution";

        public override string PrimaryKeyColumn => nameof(ActivityExecutionId);

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityExecutionLogger"/> class.
        /// </summary>
        public ActivityExecutionLogger(string instanceId, string userName)
        {
            // Set default values
            Status = string.Empty;
            StartDateTime = EndDateTime = DateTime.Now;
            ResourceInstanceId = instanceId;
            UserName = userName;
            HostName = Environment.MachineName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityExecutionLogger"/> class.
        /// </summary>
        /// <param name="activityExecutionId"></param>
        /// <param name="resourceMetadataId">the resource metadata Id</param>
        /// <param name="sessionId">The session id.</param>
        /// <param name="activityName">Name of the activity.</param>
        /// <param name="activityType">Type of the activity.</param>
        /// <param name="instanceId"></param>
        /// <param name="userName"></param>
        public ActivityExecutionLogger(string instanceId, string userName, Guid activityExecutionId, Guid? resourceMetadataId, string sessionId, string activityName, string activityType)
            : this(instanceId, userName)
        {
            ActivityExecutionId = activityExecutionId;
            ResourceMetadataId = resourceMetadataId;
            SessionId = sessionId;
            ActivityName = activityName;
            ActivityType = activityType;
        }

        /// <summary>
        /// Updates this instance with values from the specified <see cref="PluginExecutionResult" />.
        /// </summary>
        /// <param name="result">The <see cref="PluginExecutionResult" />.</param>
        public void UpdateResultValues(PluginExecutionResult result)
        {
            EndDateTime = DateTime.Now;
            Status = result.Result.ToString();

            if (string.IsNullOrWhiteSpace(result.Message))
            {
                // Ensure category is always null if no message is supplied
                ResultMessage = null;
                ResultCategory = null;
            }
            else if (string.IsNullOrWhiteSpace(result.Category))
            {
                // Ensure category is set to default value if none was provided
                ResultMessage = result.Message;
                ResultCategory = _defaultCategory;
            }
            else
            {
                ResultMessage = result.Message;
                ResultCategory = result.Category;
            }
        }

        /// <summary>
        /// Updates the current row using the specified update type and message.
        /// </summary>
        /// <param name="updateType">Type of the update.</param>
        /// <param name="message">The message.</param>
        public void UpdateValues(string updateType, string message)
        {
            Status = updateType;
            EndDateTime = DateTime.Now;

            if (!string.IsNullOrEmpty(message))
            {
                ResultMessage = message;
                ResultCategory = _defaultCategory;
            }
        }


        /// <summary>
        /// Gets or sets the activity execution id for the current row.
        /// </summary>
        [DataLogProperty]
        public Guid ActivityExecutionId { get; private set; }

        /// <summary>
        /// Gets or sets the session id for the current row.
        /// </summary>
        [DataLogProperty]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the Resource Metadata Id associated with this activity
        /// </summary>
        [DataLogProperty]
        public Guid? ResourceMetadataId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataLogProperty]
        public string ResourceInstanceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the activity for the current row.
        /// </summary>
        [DataLogProperty(MaxLength = 255)]
        public string ActivityName { get; set; }

        /// <summary>
        /// Gets or sets the type of the activity for the current row.
        /// </summary>
        [DataLogProperty]
        public string ActivityType { get; set; }

        /// <summary>
        /// Gets or sets the user name for the current row.
        /// </summary>
        [DataLogProperty]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the host name for the current row.
        /// </summary>
        [DataLogProperty]
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the start time for the current row.
        /// </summary>
        [DataLogProperty]
        public DateTimeOffset StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the end time for the current row.
        /// </summary>
        [DataLogProperty(IncludeInUpdates = true)]
        public DateTimeOffset EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the update type for the current row.
        /// </summary>
        [DataLogProperty(IncludeInUpdates = true, MaxLength = 20)]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the error message for the current row.
        /// </summary>
        [DataLogProperty(IncludeInUpdates = true, MaxLength = 1024, TruncationAllowed = true)]
        public string ResultMessage { get; private set; }

        /// <summary>
        /// Gets or sets the label for the current row.
        /// </summary>
        [DataLogProperty(IncludeInUpdates = true, MaxLength = 1024, TruncationAllowed = true)]
        public string ResultCategory { get; private set; }
    }
}