using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;

namespace HP.ScalableTestTriageData.Data.DataLog
{
    [Table("ActivityExecutionPerformance")]
    public partial class ActivityExecutionPerformance
    {
        public Guid ActivityExecutionPerformanceId { get; set; }

        [Required]
        [StringLength(50)]
        public string SessionId { get; set; }

        public Guid? ActivityExecutionId { get; set; }

        [StringLength(50)]
        public string EventLabel { get; set; }

        public int? EventIndex { get; set; }

        public DateTime? EventDateTime { get; set; }

        public virtual SessionSummary SessionSummary { get; set; }

        public DateTime LocalEventDateTime
        {
            get
            {
                return new DateTime(((DateTime)EventDateTime).Ticks, DateTimeKind.Utc).ToLocalTime();
            }
        }

        public static IEnumerable<ActivityExecutionPerformance> GetByActivityExecutionId(DataLogContext dlc, Guid? activityExecutionId)
        {
            IEnumerable<ActivityExecutionPerformance> aep = (from td in dlc.ActivityExecutionPerformances where td.ActivityExecutionId == activityExecutionId select td).ToList();

            return aep;
        }

    }
}
