using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using HP.ScalableTest.Core.DataLog.Model;

namespace HP.ScalableTest.Core.DataLog
{
    /// <summary>
    /// Entity framework context for connecting to the Data Log database.
    /// </summary>
    public sealed class DataLogContext : DbContext
    {
        /// <summary>
        /// Initializes the <see cref="DataLogContext" /> class.
        /// </summary>
        static DataLogContext()
        {
            // Disable code-first migration and database creation
            Database.SetInitializer<DataLogContext>(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogContext" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="DataLogConnectionString" />.</param>
        public DataLogContext(DataLogConnectionString connectionString)
            : base(connectionString.ToString())
        {
            // Context will primarily be used for read-only access of large datasets.
            // Explicitly disable lazy loading to avoid accidentally pulling large amounts of data one record at a time.
            Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Gets a collection of <see cref="SessionSummary" /> data from the Data Log database.
        /// </summary>
        public DbSet<SessionSummary> DbSessions => Set<SessionSummary>();

        /// <summary>
        /// Gets a collection of <see cref="SessionProduct" /> data from the Data Log database.
        /// </summary>
        public DbSet<SessionProduct> DbSessionProducts => Set<SessionProduct>();

        /// <summary>
        /// Gets a collection of <see cref="SessionScenario" /> data from the Data Log database.
        /// </summary>
        public DbSet<SessionScenario> DbSessionScenarios => Set<SessionScenario>();

        /// <summary>
        /// Gets a collection of <see cref="SessionInfo" /> objects for all test sessions.
        /// </summary>
        public IQueryable<SessionInfo> Sessions
        {
            get { return DbSessions.Select(SessionInfo.BuildFromDatabase); }
        }

        /// <summary>
        /// Gets the number of activities performed for each test session.
        /// </summary>
        /// <returns>A dictionary containing session IDs and the number of activities performed in that session.</returns>
        public Dictionary<string, int> SessionActivityCounts()
        {
            return Set<ActivityExecution>().GroupBy(n => n.SessionId)
                                           .Select(n => new { SessionId = n.Key, Count = n.Count() })
                                           .ToDictionary(n => n.SessionId, n => n.Count);
        }

        /// <summary>
        /// Gets a <see cref="DataLog.SessionData" /> object for the specified session.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns>
        /// <see cref="DataLog.SessionData" /> for the specified session.
        /// (If session is not found, returns an empty <see cref="DataLog.SessionData" /> instance.
        /// </returns>
        public SessionData SessionData(string sessionId)
        {
            SessionSummary sessionSummary = DbSessions.SingleOrDefault(n => n.SessionId == sessionId);
            if (sessionSummary != null)
            {
                IQueryable<ActivityExecution> sessionActivities = Set<ActivityExecution>().Where(n => n.SessionId == sessionId);
                return new SessionData(sessionSummary, sessionActivities);
            }
            else
            {
                // Return an empty SessionData object
                SessionSummary summary = new SessionSummary { SessionId = sessionId };
                return new SessionData(summary, Enumerable.Empty<ActivityExecution>().AsQueryable());
            }
        }

        /// <summary>
        /// Configures the entity framework model for the context.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Do not use EF pluralizing conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Specify that all Session ID columns are non-nullable varchar(50)
            modelBuilder.Properties().Where(n => n.Name == "SessionId").Configure(n => n.IsRequired());
            modelBuilder.Properties().Where(n => n.Name == "SessionId").Configure(n => n.HasMaxLength(50));
            modelBuilder.Properties().Where(n => n.Name == "SessionId").Configure(n => n.IsUnicode(false));

            // Declare primary keys for cases where EF will not infer them
            modelBuilder.Entity<SessionSummary>().HasKey(n => n.SessionId);
            modelBuilder.Entity<SessionScenario>().HasKey(n => new { n.SessionId, n.RunOrder });

            // Specify table/column naming differences
            modelBuilder.Entity<SessionProduct>().ToTable("SessionProductAssoc").HasKey(n => new { n.SessionId, n.EnterpriseTestAssociatedProductId });
            modelBuilder.Entity<ActivityExecutionRetry>().ToTable("ActivityExecutionRetries").HasKey(n => n.ActivityExecutionRetriesId);

            // Specify varchar strings for columns that may be used for filtering or grouping.
            // EF will work even if this is not specified, but queries will not perform as well.
            modelBuilder.Entity<ActivityExecution>().Property(n => n.ActivityType).IsUnicode(false);
            modelBuilder.Entity<ActivityExecution>().Property(n => n.Status).IsUnicode(false);
            modelBuilder.Entity<ActivityExecutionRetry>().Property(n => n.Status).IsUnicode(false);
            modelBuilder.Entity<SessionSummary>().Property(n => n.Status).IsUnicode(false);
            modelBuilder.Entity<SessionSummary>().Property(n => n.ShutdownState).IsUnicode(false);
        }
    }
}
