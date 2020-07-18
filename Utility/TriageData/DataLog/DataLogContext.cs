using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.SqlClient;

namespace HP.ScalableTestTriageData.Data.DataLog
{
    public partial class DataLogContext : DbContext
    {
        public DataLogContext()
            : base("name=DataLogContext")
        {
        }
        public DataLogContext(string connectionStringName) : base("name=DataLogContext")// base(connectionStringName)
        {
            this.Database.Connection.ConnectionString = connectionStringName;
        }

        public virtual DbSet<ActivityExecution> ActivityExecutions { get; set; }
        public virtual DbSet<ActivityExecutionAssetUsage> ActivityExecutionAssetUsages { get; set; }

        public virtual DbSet<ActivityExecutionDetail> ActivityExecutionDetails { get; set; }

        public virtual DbSet<ActivityExecutionPerformance> ActivityExecutionPerformances { get; set; }

        public virtual DbSet<SessionDevice> SessionDevices { get; set; }

        public virtual DbSet<SessionSummary> SessionSummaries { get; set; }

        public virtual DbSet<TriageData> TriageDatas { get; set; }
    }
}
