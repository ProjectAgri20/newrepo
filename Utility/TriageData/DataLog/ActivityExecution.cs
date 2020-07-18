using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;

namespace HP.ScalableTestTriageData.Data.DataLog
{
    [Table("ActivityExecution")]
    public partial class ActivityExecution
    {
        public Guid ActivityExecutionId { get; set; }

        public Guid? ResourceMetadataId { get; set; }

        [Required]
        [StringLength(50)]
        public string SessionId { get; set; }

        [StringLength(255)]
        public string ActivityName { get; set; }

        [StringLength(50)]
        public string ActivityType { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string HostName { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(1024)]
        public string ResultMessage { get; set; }

        [Required]
        [StringLength(50)]
        public string ResourceInstanceId { get; set; }

        [StringLength(1024)]
        public string ResultCategory { get; set; }

        public virtual SessionSummary SessionSummary { get; set; }

        public static ActivityExecution GetById(DataLogContext dlc, Guid activityExecutionId) => (from ae in dlc.ActivityExecutions where ae.ActivityExecutionId.Equals(activityExecutionId) select ae).FirstOrDefault();
    }
}
