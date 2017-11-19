using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.BusinessDomain
{
    [Table("INVOICE")]
    public class Invoice : Entity
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [ForeignKey("Order")]
        [Column("ORDER_ID")]
        public int? OrderId { get; set; }

        [Column("INVOICE_DATE")]
        public DateTime? InvoiceDate { get; set; }

        [Column("DUE_DATE")]
        public DateTime? DueDate { get; set; }

        [Column("TAX")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal Tax { get; set; }

        [Column("SHIPPING")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal Shipping { get; set; }

        [Column("AMOUNT_DUE")]
        [RegularExpression(@"^\d+\.\d{0,4}$")]
        public decimal AmountDue { get; set; }

        public virtual Order Order { get; set; }
    }
}
