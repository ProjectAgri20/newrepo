using System;
using System.Data.Entity;
using System.Linq.Expressions;

namespace HP.ScalableTest.Core.DataLog
{
    /// <summary>
    /// Represents a key used to group <see cref="SessionActivityData" /> together.
    /// </summary>
    public sealed class SessionActivityGroupKey
    {
        /// <summary>
        /// Gets the name of the activities in this group.
        /// </summary>
        public string ActivityName { get; private set; }

        /// <summary>
        /// Gets the type of the activities in this group.
        /// </summary>
        public string ActivityType { get; private set; }

        /// <summary>
        /// Gets the user name under which the activities in this group were executed.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the name of the system where the activities in this group were executed.
        /// </summary>
        public string HostName { get; private set; }

        /// <summary>
        /// Gets the time the activities in this group started execution (rounded to the nearest minute).
        /// </summary>
        public DateTimeOffset? StartDateTime { get; private set; }

        /// <summary>
        /// Gets the time the activities in this group completed execution (rounded to the nearest minute).
        /// </summary>
        public DateTimeOffset? EndDateTime { get; private set; }

        /// <summary>
        /// Gets the status of the activities in this group.
        /// </summary>
        public string Status { get; private set; }

        /// <summary>
        /// Gets the result message of the activities in this group.
        /// </summary>
        public string ResultMessage { get; private set; }

        /// <summary>
        /// Gets the result category of the activities in this group.
        /// </summary>
        public string ResultCategory { get; private set; }

        /// <summary>
        /// Gets a LINQ-to-entities compatible expression to group <see cref="SessionActivityData" /> by status.
        /// </summary>
        internal static Expression<Func<SessionActivityData, SessionActivityGroupKey>> CreateGroupKey(SessionActivityGroupFields fields)
        {
            // Check the flags to see which fields should be grouped
            bool groupByName = fields.HasFlag(SessionActivityGroupFields.ActivityName);
            bool groupByType = fields.HasFlag(SessionActivityGroupFields.ActivityType);
            bool groupByUser = fields.HasFlag(SessionActivityGroupFields.UserName);
            bool groupByHost = fields.HasFlag(SessionActivityGroupFields.HostName);
            bool groupByStartTime = fields.HasFlag(SessionActivityGroupFields.StartDateTime);
            bool groupByEndTime = fields.HasFlag(SessionActivityGroupFields.EndDateTime);
            bool groupByStatus = fields.HasFlag(SessionActivityGroupFields.Status);
            bool groupByMessage = fields.HasFlag(SessionActivityGroupFields.ResultMessage);
            bool groupByCategory = fields.HasFlag(SessionActivityGroupFields.ResultCategory);

            // Create a new group key that includes or ignores fields depending on whether the flag was set.
            // StartDateTime and EndDateTime use a DbFunction to create a new DateTime that rounds down to the nearest minute.
            return n => new SessionActivityGroupKey
            {
                ActivityName = groupByName ? n.ActivityName : null,
                ActivityType = groupByType ? n.ActivityType : null,
                UserName = groupByUser ? n.UserName : null,
                HostName = groupByHost ? n.HostName : null,
                Status = groupByStatus ? n.Status : null,
                ResultMessage = groupByMessage ? n.ResultMessage : null,
                ResultCategory = groupByCategory ? n.ResultCategory : null,
                StartDateTime = groupByStartTime ? DbFunctions.CreateDateTime(n.StartDateTime.Value.Year, n.StartDateTime.Value.Month, n.StartDateTime.Value.Day, n.StartDateTime.Value.Hour, n.StartDateTime.Value.Minute, 0) : null,
                EndDateTime = groupByEndTime ? DbFunctions.CreateDateTime(n.EndDateTime.Value.Year, n.EndDateTime.Value.Month, n.EndDateTime.Value.Day, n.EndDateTime.Value.Hour, n.EndDateTime.Value.Minute, 0) : null
            };
        }
    }
}
