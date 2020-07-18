using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.DataLog.Service
{
    /// <summary>
    /// Handles the cleanup of expired session data.
    /// </summary>
    public sealed class DataLogCleanup
    {
        private readonly DataLogConnectionString _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogCleanup" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="DataLogConnectionString" />.</param>
        public DataLogCleanup(DataLogConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Deletes expired session data from the specified DataLog database and clears expired Session IDs from VM Reservations.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public IEnumerable<string> RemoveExpiredSessionData()
        {
            LogInfo(new string('-', 60));

            List<string> expiredSessions = new List<string>();
            try
            {
                using (DataLogContext context = new DataLogContext(_connectionString))
                {
                    var expired = context.Sessions.Where(n => n.ExpirationDateTime < DateTime.UtcNow && n.ShutdownState != "NotStarted"
                                                           || n.ExpirationDateTime < DbFunctions.AddDays(DateTime.UtcNow, -30)).ToList();

                    if (expired.Any())
                    {
                        LogInfo($"{expired.Count} expired sessions found.");

                        // This could take a while - set an infinite timout
                        context.Database.CommandTimeout = 0;

                        foreach (SessionInfo session in expired)
                        {
                            DeleteSessionData(context, session);
                        }

                        expiredSessions.AddRange(expired.Select(n => n.SessionId));
                    }
                    else
                    {
                        LogInfo("No expired sessions found.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            LogInfo(new string('-', 60));

            return expiredSessions;
        }

        private static void DeleteSessionData(DataLogContext context, SessionInfo session)
        {
            try
            {
                SqlParameter sessionParameter = new SqlParameter("@sessionId", session.SessionId);
                context.Database.ExecuteSqlCommand("del_SessionData @sessionId", sessionParameter);
                LogInfo(GetLogString(session));
            }
            catch (SqlException ex)
            {
                LogError($"Error executing del_SessionData for {session.SessionId}", ex);
            }
        }

        private static string GetLogString(SessionInfo session)
        {
            StringBuilder result = new StringBuilder(nameof(session.SessionId));
            result.Append(":").Append(session.SessionId);
            result.Append(" ");
            result.Append(nameof(session.SessionName)).Append(":").Append(session.SessionName);
            result.Append(" ");
            result.Append(nameof(session.StartDateTime)).Append(":").Append(session.StartDateTime?.ToString("MM/dd/yy HH:mm tt"));
            result.Append(" ");
            result.Append(nameof(session.Status)).Append(":").Append(session.Status);
            result.Append(" ");
            result.Append(nameof(session.Owner)).Append(":").Append(session.Owner);
            result.Append(" ");
            result.Append(nameof(session.Dispatcher)).Append(":").Append(session.Dispatcher);

            return result.ToString();
        }
    }
}
