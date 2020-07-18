using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HP.ScalableTest.Utility.BtfMetadataHelper.Database
{
    public class auth_permission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public auth_permission()
        {
            auth_group_permissions = new HashSet<auth_group_permissions>();
            auth_user_user_permissions = new HashSet<auth_user_user_permissions>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public int content_type_id { get; set; }

        [Required]
        [StringLength(100)]
        public string codename { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<auth_group_permissions> auth_group_permissions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<auth_user_user_permissions> auth_user_user_permissions { get; set; }
    }
}
