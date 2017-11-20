

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.SecurityDomain
{
    public class RoleDetail : Entity
    {
        [Key]
        [ForeignKey("User")]
        [Column("USER_ID")]
        public int UserId { get; set; }

        [Key]
        [ForeignKey("Role")]
        [Column("ROLE_ID")]
        public int RoleId { get; set; }

        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }
}
