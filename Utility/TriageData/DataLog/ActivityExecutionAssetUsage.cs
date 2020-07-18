using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;

namespace HP.ScalableTestTriageData.Data.DataLog
{


    [Table("ActivityExecutionAssetUsage")]
    public partial class ActivityExecutionAssetUsage
    {
        public Guid ActivityExecutionAssetUsageId { get; set; }

        [Required]
        [StringLength(50)]
        public string SessionId { get; set; }

        public Guid ActivityExecutionId { get; set; }

        [Required]
        [StringLength(50)]
        public string AssetId { get; set; }

        public virtual SessionSummary SessionSummary { get; set; }

        public static ActivityExecutionAssetUsage GetByActivityExecutionId(DataLogContext dlc, Guid activityExecutionId) => (from ae in dlc.ActivityExecutionAssetUsages where ae.ActivityExecutionId.Equals(activityExecutionId) select ae).FirstOrDefault();
    }
}
