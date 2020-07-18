using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HP.ScalableTest.Utility.BtfMetadataHelper.Database
{
    public class testcase_repo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public testcase_repo()
        {
            testcases = new HashSet<testcase>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(200)]
        public string repo_url { get; set; }

        [Required]
        [StringLength(255)]
        public string repo_branch { get; set; }

        public int repo_type_ref_id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime modified_dt { get; set; }

        public int created_by { get; set; }

        public virtual auth_user auth_user { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<testcase> testcases { get; set; }

        public virtual testcase_repo_type testcase_repo_type { get; set; }
    }
}
