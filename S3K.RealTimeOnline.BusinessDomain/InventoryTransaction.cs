using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.BusinessDomain
{
    [Table("INVENTORY_TRANSACTION")]
    public class InventoryTransaction : Entity
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [ForeignKey("InventoryTransactionType")]
        [Column("TYPE_ID")]
        public byte? TypeId { get; set; }

        [Column("TRANSACTION_CREATED_DATE")]
        public DateTime? TransactionCreatedDate { get; set; }

        [Column("TRANSACTION_MODIFIED_DATE")]
        public DateTime? TransactionModifiedDate { get; set; }

        [ForeignKey("Product")]
        [Column("PRODUCT_ID")]
        public int? ProductId { get; set; }

        [Column("QUANTITY")]
        public int Quantity { get; set; }

        [ForeignKey("PurchaseOrder")]
        [Column("PURCHASE_ORDER_ID")]
        public int? PurchaseOrderId { get; set; }

        [ForeignKey("Order")]
        [Column("CUSTOMER_ORDER_ID")]
        public int? CustomerOrderId { get; set; }

        [Column("COMMENTS")]
        [StringLength(250)]
        public string Comments { get; set; }

        public virtual InventoryTransactionType InventoryTransactionType { get; set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    }
}