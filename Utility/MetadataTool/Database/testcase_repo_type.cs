using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HP.ScalableTest.Utility.BtfMetadataHelper.Database
{
    public class testcase_repo_type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public testcase_repo_type()
        {
            testcase_repo = new HashSet<testcase_repo>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string repo_type { get; set; }

        [Required]
        [StringLength(512)]
        public string repo_type_desc { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime modified_dt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<testcase_repo> testcase_repo { get; set; }
    }
}
