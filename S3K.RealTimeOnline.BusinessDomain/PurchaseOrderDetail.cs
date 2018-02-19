using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.BusinessDomain
{
    [Table("PURCHASE_ORDER_DETAIL")]
    public class PurchaseOrderDetail : Entity
    {
        public PurchaseOrderDetail()
        {
            PostedToInventory = false;
        }

        [Key] [Column("ID")] public int Id { get; set; }

        [ForeignKey("PurchaseOrder")]
        [Column("PURCHASE_ORDER_ID")]
        public int? PurchaseOrderId { get; set; }

        [ForeignKey("Product")]
        [Column("PRODUCT_ID")]
        public int? ProductId { get; set; }

        [ForeignKey("InventoryTransaction")]
        [Column("INVENTORY_ID")]
        public int? InventoryId { get; set; }

        [Column("QUANTITY")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal Quantity { get; set; }

        [Column("UNIT_COST")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal UnitCost { get; set; }

        [Column("DATE_RECEIVED")] public DateTime? DateReceived { get; set; }

        [Column("POSTED_TO_INVENTORY")] public bool PostedToInventory { get; set; }

        public virtual InventoryTransaction InventoryTransaction { get; set; }

        public virtual Product Product { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }
    }
}