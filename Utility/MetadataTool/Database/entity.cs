namespace HP.ScalableTest.Utility.BtfMetadataHelper.Database
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class entity
    {
        public int id { get; set; }

        [Required]
        [StringLength(150)]
        public string name { get; set; }

        [StringLength(500)]
        public string description { get; set; }

        public int entity_type_id_ref { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime modified_dt { get; set; }

        public virtual entity_types entity_types { get; set; }
    }
}
