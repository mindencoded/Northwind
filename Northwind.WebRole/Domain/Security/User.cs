using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.WebRole.Domain.Security
{
    [Table("USER")]
    public class User : Entity
    {
        public User()
        {
            Active = true;
        }

        [Key] [Column("ID")] public int Id { get; set; }

        [MaxLength(50)]
        [Required(AllowEmptyStrings = false)]
        [Column("USERNAME")]
        public string Username { get; set; }

        [MaxLength(32)]
        [Required(AllowEmptyStrings = false)]
        [Column("PASSWORD")]
        public string Password { get; set; }

        [Column("ACTIVE")] public bool Active { get; set; }

        [ForeignKey("UserType")]
        [Column("USER_TYPE_ID")]
        public byte? UserTypeId { get; set; }

        [ForeignKey("ExternalUser")]
        [Column("EXTERNAL_USER_ID")]
        public int? ExternalUserId { get; set; }

        public virtual UserType UserType { get; set; }

        public virtual Person ExternalUser { get; set; }

        public virtual ICollection<RoleDetail> RoleDetails { get; set; }
    }
}