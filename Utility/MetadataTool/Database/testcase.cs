using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HP.ScalableTest.Utility.BtfMetadataHelper.Database
{
    [Table("testcase")]
    public class testcase
    {
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [Required]
        [StringLength(255)]
        public string path { get; set; }

        public short is_deleted { get; set; }

        public int repo_ref_id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime modified_dt { get; set; }

        public virtual testcase_repo testcase_repo { get; set; }
    }
}
