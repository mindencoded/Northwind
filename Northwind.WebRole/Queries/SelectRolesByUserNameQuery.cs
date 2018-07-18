using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Northwind.WebRole.Domain.Security;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Queries
{
    public class SelectRolesByUserNameQuery : IQuery<IEnumerable<Role>>
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Username { get; set; }
    }
}