using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.BusinessDomain
{
    [Table("PURCHASE_ORDER")]
    public class PurchaseOrder : Entity
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [ForeignKey("Supplier")]
        [Column("SUPPLIER_ID")]
        public int? SupplierId { get; set; }

        [ForeignKey("Creator")]
        [Column("CREATED_BY")]
        public int? CreatedBy { get; set; }

        [Column("CREATION_DATE")]
        public DateTime? CreationDate { get; set; }

        [ForeignKey("Submitter")]
        [Column("SUBMITTED_BY")]
        public int? SubmittedBy { get; set; }

        [Column("SUBMITTED_DATE")]
        public DateTime? SubmittedDate { get; set; }

        [ForeignKey("Approvator")]
        [Column("APPROVED_BY")]
        public int? ApprovedBy { get; set; }

        [Column("APPROVED_DATE")]
        public DateTime? ApprovedDate { get; set; }

        [ForeignKey("PurchaseOrderStatus")]
        [Column("STATUS_ID")]
        public byte? StatusId { get; set; }

        [Column("EXPECTED_DATE")]
        public DateTime? ExpectedDate { get; set; }

        [Column("SHIPPING_FEE")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal ShippingFee { get; set; }

        [Column("TAX")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal Tax { get; set; }

        [Column("PAYMENT_AMOUNT")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal PaymentAmount { get; set; }

        [Column("PAYMENT_DATE")]
        public DateTime? PaymentDate { get; set; }

        [Column("PAYMENT_METHOD")]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [Column("NOTES")]
        [StringLength(250)]
        public string Notes { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual Employee Creator { get; set; }

        public virtual Employee Submitter { get; set; }

        public virtual Employee Approvator { get; set; }

        public virtual PurchaseOrderStatus PurchaseOrderStatus { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; }

        public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    }
}
