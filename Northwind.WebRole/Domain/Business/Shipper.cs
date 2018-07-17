using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.WebRole.Domain.Business
{
    [Table("SHIPPER")]
    public class Shipper : Person
    {
        public virtual ICollection<Order> Orders { get; set; }
    }
}