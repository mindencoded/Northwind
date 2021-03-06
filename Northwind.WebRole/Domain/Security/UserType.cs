﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.WebRole.Domain.Security
{
    [Table("USER_TYPE")]
    public class UserType : Entity
    {
        [Key] [Column("ID")] public byte Id { get; set; }

        [Column("TYPE_NAME")]
        [StringLength(25)]
        public string TypeName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}