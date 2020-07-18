using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HP.ScalableTest.Utility.BtfMetadataHelper.Database
{
    public class authtoken_token
    {
        [Key]
        [StringLength(40)]
        public string key { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime created { get; set; }

        public int user_id { get; set; }

        public virtual auth_user auth_user { get; set; }
    }
}
