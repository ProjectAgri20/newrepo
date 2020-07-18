namespace HP.ScalableTest.Utility.BtfMetadataHelper.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class entity_types
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public entity_types()
        {
            entities = new HashSet<entity>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(150)]
        public string name { get; set; }

        [StringLength(500)]
        public string description { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime modified_dt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<entity> entities { get; set; }
    }
}
