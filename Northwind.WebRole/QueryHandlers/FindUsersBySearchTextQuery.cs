using System.ComponentModel.DataAnnotations;
using Northwind.WebRole.Domain;
using Northwind.WebRole.Domain.Security;
using Northwind.WebRole.Tools;

namespace Northwind.WebRole.QueryHandlers
{
    public class FindUsersBySearchTextQuery : IQuery<User[]>
    {
        [Required] [StringLength(25)] public string SearchText { get; set; }

        public bool IncludeInactiveUsers { get; set; }
    }
}