using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace HP.ScalableTestTriageData.Data.DataLog
{


    [Table("SessionSummary")]
    public partial class SessionSummary
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SessionSummary()
        {
        }

        [Key]
        [StringLength(50)]
        public string SessionId { get; set; }

        [StringLength(255)]
        public string SessionName { get; set; }

        [StringLength(50)]
        public string Owner { get; set; }

        [StringLength(50)]
        public string Dispatcher { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

    }
}
