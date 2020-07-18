using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using HP.ScalableTest.Core.DataLog.Model;

namespace HP.ScalableTest.Core.DataLog
{
    /// <summary>
    /// Data from a single activity performed during a test session.
    /// </summary>
    [DebuggerDisplay("{ActivityName,nq} [{ActivityType,nq}]")]
    public sealed class SessionActivityData
    {
        /// <summary>
        /// Gets the unique identifier for this activity execution.
        /// </summary>
        public Guid ActivityExecutionId { get; private set; }

        /// <summary>
        /// Gets the unique identifier for the VirtualResourceMetadata that defined this activity.
        /// </summary>
        public Guid? ResourceMetadataId { get; private set; }

        /// <summary>
        /// Gets the unique identifier for the virtual resource that executed this activity.
        /// </summary>
        public string ResourceInstanceId { get; private set; }

        /// <summary>
        /// Gets the name of the activity.
        /// </summary>
        public string ActivityName { get; private set; }

        /// <summary>
        /// Gets the type of the activity.
        /// </summary>
        public string ActivityType { get; private set; }

        /// <summary>
        /// Gets the user name under which this activity was executed.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the name of the system where the activity was executed.
        /// </summary>
        public string HostName { get; private set; }

        /// <summary>
        /// Gets the time this activity started execution.
        /// </summary>
        public DateTimeOffset? StartDateTime { get; private set; }

        /// <summary>
        /// Gets the time this activity completed execution.
        /// </summary>
        public DateTimeOffset? EndDateTime { get; private set; }

        /// <summary>
        /// Gets the status of the activity.
        /// </summary>
        public string Status { get; private set; }

        /// <summary>
        /// Gets the message describing the result of the activity, if one was specified.
        /// </summary>
        public string ResultMessage { get; private set; }

        /// <summary>
        /// Gets the category used to group similar execution results, if one was specified.
        /// </summary>
        public string ResultCategory { get; private set; }

        /// <summary>
        /// Gets the assets used for this activity.
        /// </summary>
        public IEnumerable<string> Assets { get; private set; }

        /// <summary>
        /// Gets the documents used for this activity.
        /// </summary>
        public IEnumerable<string> Documents { get; private set; }

        /// <summary>
        /// Gets the servers used for this activity.
        /// </summary>
        public IEnumerable<string> Servers { get; private set; }

        /// <summary>
        /// Gets the execution details associated with this activity.
        /// </summary>
        public IEnumerable<SessionActivityDetail> ExecutionDetails { get; private set; }

        /// <summary>
        /// Gets a LINQ-to-entities compatible expresion to generate a <see cref="SessionActivityData" /> object from an <see cref="ActivityExecution" /> object.
        /// </summary>
        internal static Expression<Func<ActivityExecution, SessionActivityData>> BuildFromDatabase
        {
            get
            {
                return activity => new SessionActivityData
                {
                    ActivityExecutionId = activity.ActivityExecutionId,
                    ResourceMetadataId = activity.ResourceMetadataId,
                    ResourceInstanceId = activity.ResourceInstanceId,
                    ActivityName = activity.ActivityName,
                    ActivityType = activity.ActivityType,
                    UserName = activity.UserName,
                    HostName = activity.HostName,
                    StartDateTime = activity.StartDateTime,
                    EndDateTime = activity.EndDateTime,
                    Status = activity.Status,
                    ResultMessage = activity.ResultMessage,
                    ResultCategory = activity.ResultCategory,
                    Assets = activity.AssetUsages.Select(n => n.AssetId),
                    Documents = activity.DocumentUsages.Select(n => n.DocumentName),
                    Servers = activity.ServerUsages.Select(n => n.ServerName),
                    ExecutionDetails = activity.ExecutionDetails.Select(n => new SessionActivityDetail { Label = n.Label, Message = n.Message, DetailDateTime = n.DetailDateTime })
                };
            }
        }
    }
}
