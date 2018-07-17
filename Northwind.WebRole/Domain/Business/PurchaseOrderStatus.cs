using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.WebRole.Domain.Business
{
    [Table("PURCHASE_ORDER_STATUS")]
    public class PurchaseOrderStatus : Entity
    {
        [Key] [Column("ID")] public byte Id { get; set; }

        [Column("STATUS_NAME")]
        [StringLength(50)]
        [Required(AllowEmptyStrings = false)]
        public string StatusName { get; set; }

        public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; }
    }
}