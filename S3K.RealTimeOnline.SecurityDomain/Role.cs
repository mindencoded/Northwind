using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.SecurityDomain
{
    public class Role : Entity
    {
        [Key] [Column("ID")] public int Id { get; set; }

        [Column("ROLE_NAME")]
        [StringLength(50)]
        [Required(AllowEmptyStrings = false)]
        public string RoleName { get; set; }

        public virtual ICollection<RoleDetail> RoleDetails { get; set; }
    }
}