using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.SecurityDomain
{
    [Table("ROLE")]
    public class Role : Entity
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("NAME")]
        [StringLength(50)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Column("DESCRIPTION")]
        [StringLength(250)]
        public string Description { get; set; }

        [ForeignKey("RoleGroup")]
        [Column("ROLE_GROUP_ID")]
        public int RoleGroupId { get; set; }

        public virtual RoleGroup RoleGroup { get; set; }

        public virtual ICollection<RoleDetail> RoleDetails { get; set; }
    }
}