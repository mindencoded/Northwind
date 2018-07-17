using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.WebRole.Domain.Business
{
    [Table("SUPPLIER")]
    public class Supplier : Person
    {
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
}