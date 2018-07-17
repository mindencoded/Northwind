using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.WebRole.Domain.Business
{
    [Table("ORDER_DETAIL")]
    public class OrderDetail : Entity
    {
        [Key] [Column("ID")] public int Id { get; set; }

        [ForeignKey("Order")]
        [Column("ORDER_ID")]
        public int OrderId { get; set; }

        [ForeignKey("Product")]
        [Column("PRODUCT_ID")]
        public int? ProductId { get; set; }

        [Column("QUANTITY")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal Quantity { get; set; }

        [Column("UNIT_PRICE")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal UnitPrice { get; set; }

        [Column("DISCOUNT")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal Discount { get; set; }

        [Column("DATE_ALLOCATED")] public DateTime? DateAllocated { get; set; }

        [ForeignKey("OrderDetailStatus")]
        [Column("STATUS_ID")]
        public byte? StatusId { get; set; }

        [ForeignKey("PurchaseOrder")]
        [Column("PURCHASE_ORDER_ID")]
        public int? PurchaseOrderId { get; set; }

        [ForeignKey("InventoryTransaction")]
        [Column("INVENTORY_ID")]
        public int? InventoryId { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }

        public virtual OrderDetailStatus OrderDetailStatus { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public virtual InventoryTransaction InventoryTransaction { get; set; }
    }
}