using System.Diagnostics;
using System.Linq;
using HP.ScalableTest.Core.DataLog.Model;

namespace HP.ScalableTest.Core.DataLog
{
    /// <summary>
    /// Contains data pertaining to a test session.
    /// </summary>
    [DebuggerDisplay("{SessionId,nq}")]
    public sealed class SessionData
    {
        private readonly IQueryable<ActivityExecution> _sessionActivities;

        /// <summary>
        /// Gets the unique identifier for this session.
        /// </summary>
        public string SessionId { get; }

        /// <summary>
        /// Gets the <see cref="SessionInfo" /> for this session.
        /// </summary>
        public SessionInfo SessionInfo { get; }

        /// <summary>
        /// Gets the activities performed as part of this session.
        /// </summary>
        public IQueryable<SessionActivityData> Activities
        {
            get { return _sessionActivities.Select(SessionActivityData.BuildFromDatabase); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionData" /> class.
        /// </summary>
        /// <param name="sessionSummary">The <see cref="SessionSummary" />.</param>
        /// <param name="sessionActivities">The query for session activities.</param>
        internal SessionData(SessionSummary sessionSummary, IQueryable<ActivityExecution> sessionActivities)
        {
            _sessionActivities = sessionActivities;

            SessionId = sessionSummary.SessionId;
            SessionInfo = new SessionInfo(sessionSummary);
        }

        /// <summary>
        /// Gets a count of activities using the specified <see cref="SessionActivityGroupFields" /> to create a <see cref="SessionActivityGroupKey" />.
        /// </summary>
        /// <param name="groupFields">The <see cref="SessionActivityGroupFields" /> indicated which fields to group by.</param>
        /// <returns>A collection of <see cref="SessionActivityCount" /> objects grouped according to <see cref="SessionActivityGroupFields" />.</returns>
        public IQueryable<SessionActivityCount> ActivityCounts(SessionActivityGroupFields groupFields)
        {
            var keySelector = SessionActivityGroupKey.CreateGroupKey(groupFields);
            return Activities.GroupBy(keySelector).Select(SessionActivityCount.BuildFromGrouping);
        }
    }
}
