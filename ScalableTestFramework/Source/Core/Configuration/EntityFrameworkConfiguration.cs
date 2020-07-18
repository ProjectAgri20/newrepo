using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.Configuration
{
    /// <summary>
    /// Configures custom behavior for Entity Framework contexts.
    /// </summary>
    /// <remarks>
    /// Entity Framework only allows one custom DbConfiguration per AppDomain, regardless of how many
    /// DbContexts are used.  This makes it difficult to place the DbConfiguration with the DbContexts
    /// and let Entity Framework automatically discover and load it.  Instead, this class can be used
    /// to initialize the custom DbConfiguration before any of the contexts need it.
    /// </remarks>
    public static class EntityFrameworkConfiguration
    {
        /// <summary>
        /// Initializes the custom <see cref="DbConfiguration" />.
        /// </summary>
        /// <remarks>
        /// This method should be called early in application execution, before any
        /// DbContext objects have been instantiated.  Calling this method after a DbContext
        /// has loaded will result in an exception.
        /// </remarks>
        public static void Initialize()
        {
            DbConfiguration.SetConfiguration(new CustomDbConfiguration());
        }

        private sealed class CustomDbConfiguration : DbConfiguration
        {
            public CustomDbConfiguration()
            {
                SetExecutionStrategy(SqlProviderServices.ProviderInvariantName, () => new DbContextExecutionStrategy());
            }

            private sealed class DbContextExecutionStrategy : DbExecutionStrategy
            {
                private static readonly int[] _errorsToRetry = new[]
                {
                    1205,   // Deadlock
                };

                protected override bool ShouldRetryOn(Exception exception)
                {
                    if (exception is SqlException sqlException)
                    {
                        SqlError error = sqlException.Errors.Cast<SqlError>().FirstOrDefault(n => _errorsToRetry.Contains(n.Number));
                        if (error != null)
                        {
                            LogWarn($"Retrying database operation on {sqlException.Server} after error {error.Number}.", exception);
                            return true;
                        }
                    }

                    return false;
                }
            }
        }
    }
}
