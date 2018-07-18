using System.ComponentModel.DataAnnotations;
using Northwind.WebRole.Domain.Security;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Queries
{
    public class FindUserByUsernamePasswordQuery : IQuery<User>
    {
        public FindUserByUsernamePasswordQuery()
        {
            Active = true;
        }

        [Required] [StringLength(50)] public string Username { get; set; }

        [Required] [StringLength(32)] public string Password { get; set; }

        public bool Active { get; set; }
    }
}