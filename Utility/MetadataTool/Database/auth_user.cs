using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HP.ScalableTest.Utility.BtfMetadataHelper.Database
{
    public class auth_user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public auth_user()
        {
            auth_user_groups = new HashSet<auth_user_groups>();
            auth_user_user_permissions = new HashSet<auth_user_user_permissions>();
            authtoken_token = new HashSet<authtoken_token>();
            testcase_repo = new HashSet<testcase_repo>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(128)]
        public string password { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? last_login { get; set; }

        public short is_superuser { get; set; }

        [Required]
        [StringLength(150)]
        public string username { get; set; }

        [Required]
        [StringLength(30)]
        public string first_name { get; set; }

        [Required]
        [StringLength(30)]
        public string last_name { get; set; }

        [Required]
        [StringLength(254)]
        public string email { get; set; }

        public short is_staff { get; set; }

        public short is_active { get; set; }

        [Column(TypeName="datetime")]
        public DateTime? date_joined { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<auth_user_groups> auth_user_groups { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<auth_user_user_permissions> auth_user_user_permissions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<authtoken_token> authtoken_token { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<testcase_repo> testcase_repo { get; set; }
    }
}
