using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.BusinessDomain
{
    [Table("SUPPLIER")]
    public class Supplier : Person
    {
        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
}
