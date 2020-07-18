using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;

namespace HP.ScalableTestTriageData.Data.DataLog
{
    [Table("ActivityExecutionDetail")]
    public partial class ActivityExecutionDetail
    {
        public Guid ActivityExecutionDetailId { get; set; }

        [Required]
        [StringLength(50)]
        public string SessionId { get; set; }

        [Required]
        public Guid ActivityExecutionId { get; set; }

        [StringLength(255)]
        public string Label { get; set; }

        [StringLength(1024)]
        public string Message { get; set; }

        public DateTime? DetailDateTime { get; set; }

        /// <summary>
        /// Gets the ActivityExecutionDetail List by the activity execution identifier.
        /// </summary>
        /// <param name="dlc">The DataLog Context.</param>
        /// <param name="activityExecutionId">The activity execution identifier.</param>
        /// <returns>ActivityExecutionDetail List</returns>
        public static IEnumerable<ActivityExecutionDetail> GetByActivityExecutionId(DataLogContext dlc, Guid? activityExecutionId)
        {
            IEnumerable<ActivityExecutionDetail> aedList = (from aed in dlc.ActivityExecutionDetails where aed.ActivityExecutionId == activityExecutionId select aed).ToList();

            return aedList;
        }
    }
}
