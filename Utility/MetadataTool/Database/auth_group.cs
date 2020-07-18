using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HP.ScalableTest.Utility.BtfMetadataHelper.Database
{
    public class auth_group
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public auth_group()
        {
            auth_group_permissions = new HashSet<auth_group_permissions>();
            auth_user_groups = new HashSet<auth_user_groups>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(80)]
        public string name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<auth_group_permissions> auth_group_permissions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<auth_user_groups> auth_user_groups { get; set; }
    }
}
