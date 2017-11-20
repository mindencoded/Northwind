using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.SecurityDomain
{
    public class UserType : Entity
    {
        [Key]
        [Column("ID")]
        public byte Id { get; set; }

        [Column("TYPE_NAME")]
        [StringLength(25)]
        public PersonType TypeName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
