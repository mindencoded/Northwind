using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.BusinessDomain
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