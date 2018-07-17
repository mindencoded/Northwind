using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.WebRole.Domain.Security
{
    [Table("ROLE_DETAIL")]
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

        [Column("ACTIVE")] public bool Active { get; set; }

        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }
}