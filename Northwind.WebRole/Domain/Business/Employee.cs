using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.WebRole.Domain.Business
{
    [Table("EMPLOYEE")]
    public class Employee : Person
    {
        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<PurchaseOrder> CreatedPurchaseOrders { get; set; }

        public virtual ICollection<PurchaseOrder> SubmittedPurchaseOrders { get; set; }

        public virtual ICollection<PurchaseOrder> ApprovedPurchaseOrders { get; set; }
    }
}