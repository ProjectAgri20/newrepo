using System.Data.Common;

namespace HP.ScalableTest.Service.EPrintJobMonitor
{
    /// <summary>
    /// Data class for pending ePrint jobs.
    /// </summary>
    internal class PendingJob
    {
        public PendingJob(DbDataReader reader)
        {
            JobId = (int)reader["JobId"];
            IsInsert = (bool)reader["IsInsert"];
            Status = PendingJobStatus.ContinueProcessing;
        }

        /// <summary>
        /// The ePrint JobId
        /// </summary>
        public int JobId { get; set; }

        /// <summary>
        /// Whether or not the job record is an update.
        /// </summary>
        public bool IsInsert { get; set; }

        public PendingJobStatus Status { get; set; }

        public override string ToString()
        {
            return JobId.ToString();
        }
    }

    internal enum PendingJobStatus
    {
        /// <summary>
        /// The ePrint job has not reached an end state.
        /// </summary>
        ContinueProcessing,
        /// <summary>
        /// The ePrint job has reached an End state.
        /// </summary>
        EndstateReached,
        /// <summary>
        /// The ePrint job is not an STF job.
        /// </summary>
        NotAnSTFJob
    }
}
