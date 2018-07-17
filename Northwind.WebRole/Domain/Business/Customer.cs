using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.WebRole.Domain.Business
{
    [Table("CUSTOMER")]
    public class Customer : Person
    {
        public virtual ICollection<Order> Orders { get; set; }
    }
}