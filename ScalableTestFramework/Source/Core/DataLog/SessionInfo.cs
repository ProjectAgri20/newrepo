using System;
using System.Diagnostics;
using System.Linq.Expressions;
using HP.ScalableTest.Core.DataLog.Model;

namespace HP.ScalableTest.Core.DataLog
{
    /// <summary>
    /// Contains general information about a test session.
    /// </summary>
    [DebuggerDisplay("{SessionId,nq}")]
    public sealed class SessionInfo
    {
        /// <summary>
        /// Gets the unique identifier for the session.
        /// </summary>
        public string SessionId { get; private set; }

        /// <summary>
        /// Gets the session name.
        /// </summary>
        public string SessionName { get; private set; }

        /// <summary>
        /// Gets the session owner.
        /// </summary>
        public string Owner { get; private set; }

        /// <summary>
        /// Gets the dispatcher that launched the session.
        /// </summary>
        public string Dispatcher { get; private set; }

        /// <summary>
        /// Gets the time the session started.
        /// </summary>
        public DateTimeOffset? StartDateTime { get; private set; }

        /// <summary>
        /// Gets the time the session ended.
        /// </summary>
        public DateTimeOffset? EndDateTime { get; private set; }

        /// <summary>
        /// Gets the current runtime status of the session.
        /// </summary>
        public string Status { get; private set; }

        /// <summary>
        /// Gets the state of the session with respect to shutdown.
        /// </summary>
        public string ShutdownState { get; private set; }

        /// <summary>
        /// Gets the projected end time of the session.
        /// </summary>
        public DateTimeOffset? ProjectedEndDateTime { get; private set; }

        /// <summary>
        /// Gets the time when the session data will expire.
        /// </summary>
        public DateTimeOffset? ExpirationDateTime { get; private set; }

        /// <summary>
        /// Gets the user that shut down the session.
        /// </summary>
        public string ShutdownUser { get; private set; }

        /// <summary>
        /// Gets the time the session was shut down.
        /// </summary>
        public DateTimeOffset? ShutdownDateTime { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionInfo" /> class.
        /// </summary>
        private SessionInfo()
        {
            // Required for Entity Framework
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionInfo" /> class.
        /// </summary>
        /// <param name="session">The <see cref="SessionSummary" /> to pull data from.</param>
        internal SessionInfo(SessionSummary session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            SessionId = session.SessionId;
            SessionName = session.SessionName;
            Owner = session.Owner;
            Dispatcher = session.Dispatcher;
            StartDateTime = GetDateTimeOffset(session.StartDateTime);
            EndDateTime = GetDateTimeOffset(session.EndDateTime);
            Status = session.Status;
            ShutdownState = session.ShutdownState;
            ProjectedEndDateTime = GetDateTimeOffset(session.ProjectedEndDateTime);
            ExpirationDateTime = GetDateTimeOffset(session.ExpirationDateTime);
            ShutdownUser = session.ShutdownUser;
            ShutdownDateTime = GetDateTimeOffset(session.ShutdownDateTime);

            DateTimeOffset? GetDateTimeOffset(DateTime? utc)
            {
                // SessionSummary DateTime values are in UTC.  This method correctly converts them to a UTC DateTimeOffset.
                return utc.HasValue ? new DateTimeOffset(utc.Value, TimeSpan.Zero) : (DateTimeOffset?)null;
            }
        }

        /// <summary>
        /// Gets a LINQ-to-entities compatible expression to generate a <see cref="SessionInfo" /> object from a <see cref="SessionSummary" /> object.
        /// </summary>
        internal static Expression<Func<SessionSummary, SessionInfo>> BuildFromDatabase
        {
            get
            {
                return session => new SessionInfo
                {
                    SessionId = session.SessionId,
                    SessionName = session.SessionName,
                    Owner = session.Owner,
                    Dispatcher = session.Dispatcher,
                    StartDateTime = session.StartDateTime,
                    EndDateTime = session.EndDateTime,
                    Status = session.Status,
                    ShutdownState = session.ShutdownState,
                    ProjectedEndDateTime = session.ProjectedEndDateTime,
                    ExpirationDateTime = session.ExpirationDateTime,
                    ShutdownUser = session.ShutdownUser,
                    ShutdownDateTime = session.ShutdownDateTime
                };
            }
        }
    }
}
