using System.ComponentModel.DataAnnotations;
using Northwind.WebRole.Domain.Security;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Queries
{
    public class FindUsersBySearchTextQuery : IQuery<User[]>
    {
        [Required] [StringLength(25)] public string SearchText { get; set; }

        public bool IncludeInactiveUsers { get; set; }
    }
}