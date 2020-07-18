using System;
using System.Collections.Generic;

namespace HP.ScalableTest.Core.DataLog.Model
{
    /// <summary>
    /// The status or result of a single activity performed during a test session.
    /// </summary>
    public sealed class ActivityExecution
    {
        /// <summary>
        /// Gets or sets the unique identifier for this activity execution.
        /// </summary>
        public Guid ActivityExecutionId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the session that performed this activity.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the VirtualResourceMetadata that defined this activity.
        /// </summary>
        public Guid? ResourceMetadataId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the virtual resource that executed this activity.
        /// </summary>
        public string ResourceInstanceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the activity.
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>
        /// Gets or sets the type of the activity.
        /// </summary>
        public string ActivityType { get; set; }

        /// <summary>
        /// Gets or sets the user name under which this activity was executed.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the name of the system where the activity was executed.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the time this activity started execution.
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the time this activity completed execution.
        /// </summary>
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the status of the activity.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the message describing the result of the activity, if one was specified.
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets the category used to group similar execution results, if one was specified.
        /// </summary>
        public string ResultCategory { get; set; }

        /// <summary>
        /// Gets the assets used for this activity.
        /// </summary>
        public ICollection<ActivityExecutionAssetUsage> AssetUsages { get; private set; } = new HashSet<ActivityExecutionAssetUsage>();

        /// <summary>
        /// Gets the documents used for this activity.
        /// </summary>
        public ICollection<ActivityExecutionDocumentUsage> DocumentUsages { get; private set; } = new HashSet<ActivityExecutionDocumentUsage>();

        /// <summary>
        /// Gets the servers used for this activity.
        /// </summary>
        public ICollection<ActivityExecutionServerUsage> ServerUsages { get; private set; } = new HashSet<ActivityExecutionServerUsage>();

        /// <summary>
        /// Gets the execution details for this activity.
        /// </summary>
        public ICollection<ActivityExecutionDetail> ExecutionDetails { get; private set; } = new HashSet<ActivityExecutionDetail>();

        /// <summary>
        /// Gets the retry attempt information for this activity.
        /// </summary>
        public ICollection<ActivityExecutionRetry> Retries { get; private set; } = new HashSet<ActivityExecutionRetry>();
    }
}
